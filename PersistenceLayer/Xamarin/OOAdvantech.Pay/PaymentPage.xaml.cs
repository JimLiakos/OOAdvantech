using FinanceFacade;
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

        internal void LoadPayment(IPayment payment)
        {
            var providerJson = payment.PaymentProviderJson;
            if (!string.IsNullOrWhiteSpace(providerJson))
            {
                var orderCode = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(providerJson)?.orderCode;
                this.PayWebView.Uri=$"https://demo.vivapayments.com/web/checkout?ref={orderCode}";
            }

            
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
        public class PaymentOrderResponse
        {
            public long orderCode { get; set; }
        }

        private void BackPressed()
        {
            OnPay = false;
            if (this.PayServiceTask != null)
                this.PayServiceTask.SetResult(false);
            OOAdvantech.DeviceApplication.Current.BackPressed-=BackPressed;

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