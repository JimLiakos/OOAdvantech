using FinanceFacade;
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
            Payment= payment;
            var providerJson = payment.PaymentProviderJson;
            if (!string.IsNullOrWhiteSpace(providerJson))
            {
                var orderCode = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(providerJson)?.orderCode;
                this.PayWebView.Uri=$"https://demo.vivapayments.com/web/checkout?ref={orderCode}";

                this.PayWebView.Navigated+=PayWebView_Navigated;
            }
        }

        private void PayWebView_Navigated(object sender, Web.NavigatedEventArgs e)
        {
            string url = e.Address;

            var payment = Payment;

           // url="https://demo.vivapayments.com/web/checkout/result?t=051e0d67-d54e-4f7c-8bad-23180a41757b&s=3288397036572604&lang=en-GB&eventId=0&eci=1";
            if (url.IndexOf("vivapayments.com/web/checkout/result")!=-1)
            {
                if (GetCheckoutData(url, payment))
                {
                    PaySucceeded=true;
                    DeviceApplication.Current.OnBackPressed();
                }
            }
        }

        private static bool GetCheckoutData(string url, IPayment payment)
        {
            int queryStartPos = url.IndexOf("?");
            if (queryStartPos != -1)
            {
                string query = url.Substring(queryStartPos + 1);
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var parameters = System.Web.HttpUtility.ParseQueryString(query);
                    if (parameters.Get("t")!=null)
                    {
                        PaymentOrderResponse paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(payment.PaymentProviderJson);

                        paymentOrderResponse.transactionId = parameters.Get("t");
                        paymentOrderResponse.orderCode =long.Parse(parameters.Get("s"));
                        paymentOrderResponse.eventId = parameters.Get("eventId");
                        paymentOrderResponse.eci = parameters.Get("eci");
                        return true;

                    }
                }
            }
            return false;
        }

        public class PaymentOrderResponse
        {
            internal string eventId;
            internal string eci;

            public long orderCode { get; set; }
            public long expiring { get; set; }
            public string transactionId { get; internal set; }
        }
    }

    public class PaymentService
    {
        PaymentPage PaymentPage;

        public bool OnPay { get; private set; }
        public IPayment Payment { get; private set; }
        TaskCompletionSource<bool> PayServiceTask;
        public Task<bool> Pay(FinanceFacade.IPayment payment)
        {

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
                Payment=payment;
                PayServiceTask = new TaskCompletionSource<bool>();
                OOAdvantech.DeviceApplication.Current.BackPressed+=BackPressed;
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                PaymentPage=new PaymentPage();

                PaymentPage.LoadPayment(payment);

                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(PaymentPage);
            });


            return PayServiceTask.Task;


        }


        private void BackPressed()
        {
            OnPay = false;
            
            if (this.PayServiceTask != null)
                this.PayServiceTask.SetResult(PaymentPage.PaySucceeded);
            DeviceApplication.Current.BackPressed-=BackPressed;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PopAsync();
                PaymentPage=null;
            });


        }

        public PaymentService()
        {
            OnPay = false;


        }
    }




}