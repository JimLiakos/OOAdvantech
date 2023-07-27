using FinanceFacade;
using OOAdvantech.Remoting.RestApi;
//using OOAdvantech.Pay.Viva;
using OOAdvantech.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OOAdvantech.Pay
{
    /// <MetaDataID>{acfa0cc9-bef7-4cd8-b41e-9dd6f06f4a19}</MetaDataID>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage(PaymentService paymentService)
        {
            InitializeComponent();
            this.BindingContext = new PaymentPageViewModel();
            PaymentService=paymentService;
        }

        private readonly PaymentService PaymentService;

        protected override bool OnBackButtonPressed()
        {
            var response = base.OnBackButtonPressed();
            var suspendGoBackEvent = PaymentService.SuspendGoBackEvent;
            suspendGoBackEvent=true;
            if (suspendGoBackEvent)
                return true;

            return response;
        }




    }

    /// <MetaDataID>{d2c424d2-96cd-4a4c-b3a5-d5c4bba74596}</MetaDataID>
    public class PaymentService
    {
        PaymentPage PaymentPage;

        public bool OnPay { get; private set; }
        public IPayment Payment { get; private set; }
        TaskCompletionSource<bool> PayServiceTask;
        public Task<bool> Pay(IPayment payment, decimal tipAmount, string server, bool hasNavigationBar = true)
        {
            if (payment?.State==PaymentState.Completed)
                return Task<bool>.FromResult(true);

            //payment.cred
            payment.PaymentGateway.CreatePaymentOrder(payment, tipAmount, @"{""color"": ""607d8b""}");


            RemotingServices.InvalidateCacheData(payment as OOAdvantech.Remoting.MarshalByRefObject);


            if (string.IsNullOrWhiteSpace(payment?.PaymentProviderJson))
                return Task<bool>.FromResult(false);

            if (!(Xamarin.Forms.Application.Current.MainPage is NavigationPage))
            {
                throw new Exception("MainPage must be NavigationPage");
            }
            lock (this)
            {
                if (OnPay && PayServiceTask != null)
                    return PayServiceTask.Task;
                OnPay = true;
                Payment = payment;
                PayServiceTask = new TaskCompletionSource<bool>();
                OOAdvantech.DeviceApplication.Current.BackPressed += BackPressed;
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                PaymentPage = new PaymentPage(this);
                NavigationPage.SetHasNavigationBar(PaymentPage, hasNavigationBar);
                Server = server;
                LoadPayment(payment);
                (Xamarin.Forms.Application.Current.MainPage as NavigationPage).Popped += NavigationBack;
                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(PaymentPage);
            });
            return PayServiceTask.Task;


        }
        string PaymentGatewayUrl;
        public string Server { get; internal set; }
        internal void LoadPayment(IPayment payment)
        {
            Payment = payment;
            PaymentGatewayUrl =payment.PaymentOrderUrl;// VivaHelper.GetPaymentGatewayUrl(payment);
            this.PaymentPage.PayWebView.Uri =PaymentGatewayUrl;
            this.PaymentPage.PayWebView.Navigated += PayWebView_Navigated;
        }
        private async void PayWebView_Navigated(object sender, Web.NavigatedEventArgs e)
        {
            string url = e.Address;
            var payment = Payment;
            if (PaymentGatewayUrl!=url)
            {
                lock (ResponseLock)
                {
                    //_ResponseTask = VivaHelper.VivaResponseUrl(url, payment, Server);
                    _ResponseTask= Task<PaymentActionState>.Run(async () =>
                    {
                        int tries = 30;
                        while (tries > 0)
                        {

                            try
                            {
                                var response = payment.ParseResponse(url);
                                return response;

                            }
                            catch (System.Net.WebException commError)
                            {
                                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                            }
                            catch (Exception commError)
                            {
                                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                            }
                            tries--;

                        }
                        return PaymentActionState.Continue;
                    });
                }

                var paymentState = await _ResponseTask;
                if (paymentState==PaymentActionState.Succeeded)
                {
                    lock (ResponseLock)
                    {
                        _ResponseTask = null;
                        _PaySucceeded = true;
                    }
                    await Close();
                    //DeviceApplication.Current.OnBackPressed(new BackPressedArgs());
                }
                else if (paymentState==PaymentActionState.Canceled)
                {
                    lock (ResponseLock)
                    {
                        _ResponseTask = null;
                        _PaySucceeded = false;
                    }
                    await Close();
                    //DeviceApplication.Current.OnBackPressed(new BackPressedArgs());
                }
                else
                {
                    lock (ResponseLock)
                    {
                        _ResponseTask = null;
                    }
                }
            }
        }
        bool _PaySucceeded;
        public async Task<bool> IsPaySucceeded()
        {

            {
                var responseTask = ResponseTask;
                if (responseTask != null)
                    await responseTask;

                return _PaySucceeded;
            }
        }



        object ResponseLock = new object();
        Task<PaymentActionState> _ResponseTask;
        public Task<PaymentActionState> ResponseTask
        {
            get
            {
                lock (ResponseLock) { return _ResponseTask; }
            }
        }

        //public Task<bool> ResponseTask
        //{
        //    get
        //    {
        //        lock (ResponseLock) { return _ResponseTask; }
        //    }
        //}

        public bool SuspendGoBackEvent { get => ResponseTask!=null; }

        private void NavigationBack(object sender, NavigationEventArgs e)
        {
            BackPressedArgs eventArgs = new BackPressedArgs();
            BackPressed(eventArgs);
        }

        private async void BackPressed(BackPressedArgs eventArgs)
        {


            if (!SuspendGoBackEvent)
            {
                if (this.PaymentPage.PayWebView.CanGoBack)
                    this.PaymentPage.PayWebView.GoBack();
                else
                {

                    eventArgs.Handled=true;
                    await Close();
                }
            }


        }

        private async Task Close()
        {
            (Xamarin.Forms.Application.Current.MainPage as NavigationPage).Popped -= NavigationBack;
            OnPay = false;
            if (this.PayServiceTask != null)
                this.PayServiceTask.SetResult(await IsPaySucceeded());
            DeviceApplication.Current.BackPressed -= BackPressed;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PopAsync();
                PaymentPage = null;
            });
        }

        public PaymentService()
        {
            OnPay = false;
        }
    }




}