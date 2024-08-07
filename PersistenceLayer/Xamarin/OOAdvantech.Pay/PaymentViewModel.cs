﻿using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OOAdvantech.Pay
{
    /// <MetaDataID>{68e07139-5c50-4252-bb8b-60e2e16e6d0a}</MetaDataID>
    public class PaymentPageViewModel
    {
        public ICommand PayCommand { get; set; }
        public ICommand OnPaymentOptionSelected { get; set; }
        public PaymentOptionEnum PaymentOptionEnum { get; set; }

        public CardInfo CardInfo { get; set; } = new CardInfo();
        IPayService _payService;

        string paymentClientToken = "<<Add payment client token here>>";
        string MerchantId = "<<Add merchant ID here>>";
        const double AmountToPay = 200;

        public PaymentPageViewModel()
        {
            //PaymentOptionEnum = PaymentOptionEnum.CreditCard;
            _payService = Xamarin.Forms.DependencyService.Get<IPayService>();
            PayCommand = new Command(async () => await CreatePayment());
            OnPaymentOptionSelected = new Command<PaymentOptionEnum>((data) =>
            {
                PaymentOptionEnum = data;

                if (PaymentOptionEnum != PaymentOptionEnum.CreditCard)
                    PayCommand.Execute(null);
            });
            GetPaymentConfig();
        }

        async Task GetPaymentConfig()
        {

            //using (var httpClient = new HttpClient())
            //{

            //    //Uri uri = new Uri("http://192.168.2.8:8090/api/Payment/BraintreeClientToken");
            //    Uri uri = new Uri("http://10.0.0.13:8090/api/Payment/BraintreeClientToken");
            //    //await getUserData();
            //    System.Diagnostics.Debug.WriteLine("Uri = " + uri.AbsoluteUri);

            //    var response = await httpClient.GetStringAsync(uri);
            //    paymentClientToken = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(response);
            //    MerchantId = "nyqdtchc77jt6z52";


            //    await _payService.InitializeAsync(paymentClientToken);

            //}
        }

        async Task CreatePayment()
        {
            UserDialogs.Instance.ShowLoading("Loading");

            if (_payService.CanPay)
            {
                try
                {
                    _payService.OnTokenizationSuccessful += OnTokenizationSuccessful;
                    _payService.OnTokenizationError += OnTokenizationError;

                    switch (PaymentOptionEnum)
                    {
                        case PaymentOptionEnum.Platform:
                            await _payService.TokenizePlatform(AmountToPay, MerchantId);
                            break;
                        case PaymentOptionEnum.CreditCard:
                            await _payService.TokenizeCard();//( CardInfo.CardNumber.Replace(" ", string.Empty), CardInfo.Expiry.Substring(0, 2), $"{DateTime.Now.ToString("yyyy").Substring(0, 2)}{CardInfo.Expiry.Substring(3, 2)}", CardInfo.Cvv);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to process payment", "Ok");
                    System.Diagnostics.Debug.WriteLine(ex);
                }

            }
            else
            {
                try
                {
                    string response = await _payService.TokenizeCard();//( CardInfo.CardNumber.Replace(" ", string.Empty), CardInfo.Expiry.Substring(0, 2), $"{DateTime.Now.ToString("yyyy").Substring(0, 2)}{CardInfo.Expiry.Substring(3, 2)}", CardInfo.Cvv);

                }
                catch (Exception error)
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to process payment", "Ok");
                    System.Diagnostics.Debug.WriteLine(error);


                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Payment not available", "Ok");
                });
            }
        }

        async void OnTokenizationSuccessful(object sender, string e)
        {
            _payService.OnTokenizationSuccessful -= OnTokenizationSuccessful;
            System.Diagnostics.Debug.WriteLine($"Payment Authorized - {e}");
            UserDialogs.Instance.HideLoading();
            await Application.Current.MainPage.DisplayAlert("Success", $"Payment Authorized: the token is{e}", "Ok");

        }

        async void OnTokenizationError(object sender, string e)
        {
            _payService.OnTokenizationError -= OnTokenizationError;
            System.Diagnostics.Debug.WriteLine(e);
            UserDialogs.Instance.HideLoading();
            await Application.Current.MainPage.DisplayAlert("Error", "Unable to process payment", "Ok");
        }

        public event PropertyChangedEventHandler PropertyChanged;



    }

    /// <MetaDataID>{745db944-8864-47f2-b973-ef38ce9af865}</MetaDataID>
    public enum PaymentOptionEnum
    {
        CreditCard, Platform, PayPal
    }

}
