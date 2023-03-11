using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(VivaWalletPos.Android.Pos))]
namespace VivaWalletPos.Android
{


    public class Pos : VivaWalletPos.IPos
    {



        internal static TaskCompletionSource<PaymentData> SalesTask;
        public static String callback = "mycallbackscheme://result";

        static int TransactionID = 000000000001019781;
        public Task<PaymentData> Sale(double total, double tips)
        {


            InputPageType = Xamarin.Forms.Application.Current.MainPage.GetType().FullName;
            if (Xamarin.Forms.Application.Current.MainPage is Xamarin.Forms.NavigationPage)
                InputPageType = (Xamarin.Forms.Application.Current.MainPage as Xamarin.Forms.NavigationPage).CurrentPage.GetType().FullName;
            SalesSuccessfulCompleted = false;
            Xamarin.Forms.Application.Current.PageAppearing += Current_PageAppearing;

            var taskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<PaymentData>();

            
            var packageName = AppInfo.PackageName;

            int transactionID = TransactionID++;
            string po = "PO";


#if DEBUG
            total = 0.01;
            if (tips > 0)
                tips = 0.01;
#endif
            string req = string.Format("vivapayclient://pay/v1?merchantKey=SG23323424EXS3&appId={0}&action=sale&clientTransactionId={1}&amount={2}&tipAmount={3}&withInstallments=false&preferredInstallments=0&callback={4}", packageName, transactionID, (int)(total * 100), (int)(tips * 100), callback);


            // string req = string.Format("vivapayclient://pay/v1?merchantKey=SG23323424EXS3&appId={0}&action=sale&clientTransactionId={1}&amount={2}&tipAmount={3}&withInstallments=false&preferredInstallments=0&callback={4}", packageName, transactionID, 1 /*(int)(total*100)*/, 0/*(int)(tips*100)*/, callback);
            //Log.d(TAG, "deeplinkPath:" + req);

            Intent payIntent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse(req));
            payIntent.AddFlags(ActivityFlags.NewTask);
            payIntent.AddFlags(ActivityFlags.ExcludeFromRecents);
            SalesTask = taskCompletionSource;
            try
            {
                Platform.CurrentActivity.StartActivity(payIntent);
            }
            catch (Exception e)
            {

            }
            return taskCompletionSource.Task;
            //string transactionData = "mycallbackscheme://result?status=success&message=Transaction successful&action=sale&clientTransactionId=1019781&amount=1&tipAmount=0&verificationMethod=CONTACTLESS - NO CVM&rrn=121616056603&cardType=Visa Debit&referenceNumber=56603&accountNumber=430589******5005&authorisationCode=226397&tid=16145986&orderCode=1216190047145986&transactionDate=2021-08-04T19:44:59.3476137+03:00&transactionId=9313b8db-57fa-407b-a84e-0a3116af12ff&shortOrderCode=1216190047";

            //PaymentData paymentData = Parse(transactionData);
            //return Task<PaymentData>.FromResult(paymentData);
        }

        private PaymentData Parse(string transactionData)
        {
            try
            {
                if (transactionData.IndexOf("://result?") != -1)
                {
                    var properties = transactionData.Substring(transactionData.IndexOf("://result?") + "://result?".Length).Split('&').Select(x => x.Split('=').ToArray()).ToList();

                    PaymentData paymentData = new PaymentData();

                    paymentData.AccountNum = properties.Where(x => x[0] == "accountNumber").Select(x => x[1]).FirstOrDefault();
                    paymentData.CardType = properties.Where(x => x[0] == "cardType").Select(x => x[1]).FirstOrDefault();
                    paymentData.PayAmount = properties.Where(x => x[0] == "amount").Select(x => x[1]).FirstOrDefault();
                    paymentData.TransactionID = properties.Where(x => x[0] == "referenceNumber").Select(x => x[1]).FirstOrDefault();
                    paymentData.PayAmount = (double.Parse(paymentData.PayAmount) / 100).ToString(System.Globalization.CultureInfo.GetCultureInfo(1033));


                    return paymentData;
                }

                return null;

            }
            catch (Exception error)
            {

                throw;
            }
        }

        private void Current_PageAppearing(object sender, Xamarin.Forms.Page e)
        {

            if (InputPageType == e.GetType().FullName)
            {
                var salesTask = SalesTask;
                var salesSuccessfulCompleted = SalesSuccessfulCompleted;
                var salesTransactionData = SalesTransactionData;
                SalesTask = null;
                SalesSuccessfulCompleted = false;
                if (salesTask != null)
                {
                    if (salesSuccessfulCompleted)
                        salesTask.SetResult(Parse(salesTransactionData));
                    else
                    {
                        var resutlParams = salesTransactionData.Split('?')[1].Split('&');
                        PaymentData paymentData = new PaymentData();
                        paymentData.Failed = true;
                        string messageParameter = resutlParams.Where(x => x.IndexOf("message") == 0).FirstOrDefault();
                        if (messageParameter != null)
                        {
                            string messageValue = messageParameter.Split('=')[1];
                            if (messageValue != null && messageValue.Contains("-12"))
                            {
                                paymentData.ErrorMessage = Properties.Resources.CardDeclined ;//"Trapeza to poulo";
                            }
                            if (messageValue != null && messageValue.Contains("-4"))
                            {
                                paymentData.ErrorMessage = Properties.Resources.PaymentCanceled;
                            }

                        }

                        
                        salesTask.SetResult(paymentData);
                    }


                }
            }
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
        static string InputPageType;
        static string SalesTransactionData;
        static bool SalesSuccessfulCompleted;
        public static void SalesCompleted(bool successful, string transactionData)
        {
            SalesTransactionData = transactionData;
            SalesSuccessfulCompleted = successful;

        }


    }
}