using FinanceFacade;
using OOAdvantech.Pay.Viva;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
            this.BindingContext = new PaymentPageViewModel();
        }
        public bool PaySucceeded;
        IPayment Payment;
        internal void LoadPayment(IPayment payment)
        {
            Payment = payment;
            var providerJson = payment.PaymentProviderJson;
            if (!string.IsNullOrWhiteSpace(providerJson))
            {
                var orderCode = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(providerJson)?.orderCode;
                string colorCode = "607d8b";
                this.PayWebView.Uri = $"https://demo.vivapayments.com/web/checkout?ref={orderCode}&color={colorCode}";

                this.PayWebView.Navigated += PayWebView_Navigated;
            }
        }

        private void PayWebView_Navigated(object sender, Web.NavigatedEventArgs e)
        {
            string url = e.Address;

            var payment = Payment;

            // url="https://demo.vivapayments.com/web/checkout/result?t=051e0d67-d54e-4f7c-8bad-23180a41757b&s=3288397036572604&lang=en-GB&eventId=0&eci=1";

            if (url.IndexOf("vivapayments.com/web/checkout/result") != -1)
            {
                //https://demo.vivapayments.com/web2/success
                if (VivaHelper.VivaResponseUrl(url, payment, Server))
                {
                    PaySucceeded = true;
                    DeviceApplication.Current.OnBackPressed();
                }
            }
        }

        public string Server { get; internal set; }
    }

    public class PaymentService
    {
        PaymentPage PaymentPage;

        public bool OnPay { get; private set; }
        public IPayment Payment { get; private set; }
        TaskCompletionSource<bool> PayServiceTask;
        public Task<bool> Pay(FinanceFacade.IPayment payment, string server, bool hasNavigationBar = true)
        {

            if (string.IsNullOrWhiteSpace(payment?.PaymentProviderJson))
                return Task<bool>.FromResult(false);

            //string url = "https://demo.vivapayments.com/web/checkout/result?t=873170f9-0b87-4124-86e8-0def66ac12ef&s=7138070025172605&lang=en-GB&eventId=0&eci=1";
            //if (VivaHelper.VivaResponseUrl(url, payment, server))
            //{
            //}
            //return Task<bool>.FromResult(true);

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
                PaymentPage = new PaymentPage();
                NavigationPage.SetHasNavigationBar(PaymentPage, hasNavigationBar);
                PaymentPage.Server = server;
                PaymentPage.LoadPayment(payment);
                (Xamarin.Forms.Application.Current.MainPage as NavigationPage).Popped += NavigationBack;
                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(PaymentPage);
            });


            return PayServiceTask.Task;


        }

        private void NavigationBack(object sender, NavigationEventArgs e)
        {
            BackPressed();
        }

        private void BackPressed()
        {
            (Xamarin.Forms.Application.Current.MainPage as NavigationPage).Popped -= NavigationBack;
            OnPay = false;

            if (this.PayServiceTask != null)
                this.PayServiceTask.SetResult(PaymentPage.PaySucceeded);
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