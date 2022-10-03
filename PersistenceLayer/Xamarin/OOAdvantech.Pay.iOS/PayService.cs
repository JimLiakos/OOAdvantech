using UIKit;
using System;
using System.Threading.Tasks;
using BraintreeCard;
using BraintreeCore;

using Xamarin.Forms;
using PassKit;
using BraintreeApplePay;
using Acr.UserDialogs;
using Foundation;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(OOAdvantech.Pay.iOS.PayService))]
namespace OOAdvantech.Pay.iOS
{
    public class PayService : PKPaymentAuthorizationViewControllerDelegate, IPayService
    {
        TaskCompletionSource<string> payTcs;
        public event EventHandler<string> OnTokenizationSuccessful = delegate { };
        public event EventHandler<string> OnTokenizationError = delegate { };

        bool isReady;
        BTAPIClient braintreeClient;

        public bool CanPay
        {
            get
            {
                return isReady;
            }
        }
        public static void Init()
        {

        }
        public async Task<bool> InitializeAsync(string clientToken)
        {
            var initializeTcs = new TaskCompletionSource<bool>();
            try
            {
                braintreeClient = new BTAPIClient(clientToken);
                isReady = true;
                initializeTcs.TrySetResult(isReady);
            }
            catch (Exception e)
            {
                initializeTcs.TrySetException(e);
            }
            return await initializeTcs.Task;
        }


        public async Task<string> TokenizePlatform(double totalPrice, string merchantId)
        {
            payTcs = new TaskCompletionSource<string>();
            if (isReady)
            {
                var applePayClient = new BTApplePayClient(braintreeClient);
                applePayClient.PaymentRequest((request, error) =>
                {

                    if (error == null)
                    {
                        RequestPaymentAuthorization(request, new Dictionary<string, double>{
                               { "My App",totalPrice}
                         }, merchantId);
                    }
                    else
                    {
                        OnTokenizationError?.Invoke(this, "Error: Couldn't create payment request.");
                        payTcs.TrySetException(new Exception("Error: Couldn't create payment request."));
                    }
                });

            }
            else
            {

                OnTokenizationError?.Invoke(this, "Platform is not ready to accept payments");
                payTcs.TrySetException(new Exception("Platform is not ready to accept payments"));
            }

            return await payTcs.Task;
        }

        void RequestPaymentAuthorization(PKPaymentRequest paymentRequest, IDictionary<string, double> summaryItems, string merchantId)
        {
            UserDialogs.Instance.ShowLoading("Loading");

            paymentRequest.MerchantIdentifier = merchantId;
            paymentRequest.MerchantCapabilities = PKMerchantCapability.ThreeDS;
            paymentRequest.CountryCode = "US";
            paymentRequest.CurrencyCode = "USD";

            if (summaryItems != null)
            {
                paymentRequest.PaymentSummaryItems = summaryItems.Select(i => new PKPaymentSummaryItem()
                {
                    Label = i.Key,
                    Amount = new NSDecimalNumber(i.Value)
                }).ToArray();
            }

            var window = UIApplication.SharedApplication.KeyWindow;
            var _viewController = window.RootViewController;
            while (_viewController.PresentedViewController != null)
                _viewController = _viewController.PresentedViewController;


            var vc = new PKPaymentAuthorizationViewController(paymentRequest);
            UserDialogs.Instance.HideLoading();
            if (vc != null)
            {
                vc.Delegate = this;
                _viewController?.PresentViewController(vc, true, null);
            }
            else
            {
                OnTokenizationError?.Invoke(this, "Error: Payment request is invalid.");
                payTcs?.SetException(new Exception("Error: Payment request is invalid."));
            }
        }


        public override void DidAuthorizePayment(PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        {
            var applePayClient = new BTApplePayClient(braintreeClient);
            applePayClient.TokenizeApplePayPayment(payment, (tokenizedApplePayPayment, error) =>
            {
                if (error == null)
                {
                    if (string.IsNullOrEmpty(tokenizedApplePayPayment.Nonce))
                    {
                        payTcs?.SetCanceled();

                    }
                    else
                    {
                        OnTokenizationSuccessful?.Invoke(this, tokenizedApplePayPayment.Nonce);
                        payTcs?.TrySetResult(tokenizedApplePayPayment.Nonce);
                    }

                    completion(PKPaymentAuthorizationStatus.Success);
                }
                else
                {
                    OnTokenizationError?.Invoke(this, "Error - Payment tokenization failed");
                    payTcs?.TrySetException(new Exception("Error - Payment tokenization failed"));

                    completion(PKPaymentAuthorizationStatus.Failure);
                }
            });
        }

        public override void PaymentAuthorizationViewControllerDidFinish(PKPaymentAuthorizationViewController controller)
        {
            controller.DismissViewController(true, null);
        }

        public override void WillAuthorizePayment(PKPaymentAuthorizationViewController controller)
        {

        }

        public Task<string> TokenizeCard(string panNumber = "4111111111111111", string expirationMonth = "12", string expirationYear = "2018", string cvv = null)
        {
            throw new NotImplementedException();
        }
    }

}