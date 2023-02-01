using FinanceFacade;
using OOAdvantech.Remoting.RestApi.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using static OOAdvantech.Pay.PaymentPage;
using FlavourBusinessManager.PaymentProviders;
using System.Numerics;

namespace OOAdvantech.Pay.Viva
{
    internal class VivaHelper
    {

        public static bool VivaResponseUrl(string url, IPayment payment, string server)
        {
            int queryStartPos = url.IndexOf("?");
            if (queryStartPos != -1)
            {
                string query = url.Substring(queryStartPos + 1);
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var parameters = System.Web.HttpUtility.ParseQueryString(query);
                    if (parameters.Get("t") != null)
                    {

                        VivaEvent vivaEvent = new VivaEvent();
                        vivaEvent.EventTypeId = 1796;//Transaction Payment Created
                        vivaEvent.MessageTypeId = 512;//
                        vivaEvent.MessageId= parameters.Get("eventId");
                        vivaEvent.Created = DateTime.UtcNow;
                        vivaEvent.EventData.OrderCode= long.Parse(parameters.Get("s"));
                        string transactionId = parameters.Get("t");
                        vivaEvent.EventData.TransactionId=parameters.Get("t");
                        vivaEvent.EventData.ElectronicCommerceIndicator = parameters.Get("eci");
                        var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK", DateTimeZoneHandling = Json.DateTimeZoneHandling.Utc };
                        string json = OOAdvantech.Json.JsonConvert.SerializeObject(vivaEvent, jSetttings);

                        var client = new RestClient($"https://meridiandevicesserver.azurewebsites.net/api/WebHook/VivaPayment/7f9bde62e6da45dc8c5661ee2220a7b0");
                        //var client = new RestClient($"http://{server}:8090/api/WebHook/VivaPayment/7f9bde62e6da45dc8c5661ee2220a7b0");
                        //var client = new RestClient($"http://{server}/DevicesServer/api/WebHook/VivaPayment/7f9bde62e6da45dc8c5661ee2220a7b0");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/json", json, ParameterType.RequestBody);
                        
                        PaymentOrder paymentOrder =payment.GetPaymentOrder();
                        if(string.IsNullOrWhiteSpace(paymentOrder.TransactionId)&&!string.IsNullOrWhiteSpace(transactionId))
                            payment.SetPaymentOrder(paymentOrder);

                        do
                        {
                            
                            payment.CheckForPaymentComplete()

                        } while (true);





                        return true;

                    }
                }
            }
            return false;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EventData
    {
        public bool Moto { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BankId { get; set; }
        public bool Systemic { get; set; }
        public bool Switching { get; set; }
        public object ParentId { get; set; }
        public int Amount { get; set; }
        public string ChannelId { get; set; }
        public int TerminalId { get; set; }
        public string MerchantId { get; set; }
        public long OrderCode { get; set; }
        public string ProductId { get; set; }
        public string StatusId { get; set; }
        public string FullName { get; set; }
        public string ResellerId { get; set; }
        public DateTime InsDate { get; set; }
        public int TotalFee { get; set; }
        public string CardUniqueReference { get; set; }
        public string CardToken { get; set; }
        public string CardNumber { get; set; }
        public int TipAmount { get; set; }
        public string SourceCode { get; set; }
        public string SourceName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CompanyName { get; set; }
        public string TransactionId { get; set; }
        public string CompanyTitle { get; set; }
        public string PanEntryMode { get; set; }
        public int ReferenceNumber { get; set; }
        public string ResponseCode { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderCulture { get; set; }
        public string MerchantTrns { get; set; }
        public string CustomerTrns { get; set; }
        public bool IsManualRefund { get; set; }
        public string TargetPersonId { get; set; }
        public string TargetWalletId { get; set; }
        public bool LoyaltyTriggered { get; set; }
        public int TransactionTypeId { get; set; }
        public int TotalInstallments { get; set; }
        public string CardCountryCode { get; set; }
        public string CardIssuingBank { get; set; }
        public int RedeemedAmount { get; set; }
        public object ClearanceDate { get; set; }
        public int CurrentInstallment { get; set; }
        public List<string> Tags { get; set; }
        public string BillId { get; set; }
        public string ResellerSourceCode { get; set; }
        public string ResellerSourceName { get; set; }
        public string ResellerCompanyName { get; set; }
        public string ResellerSourceAddress { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        public List<object> AssignedMerchantUsers { get; set; }
        public List<object> AssignedResellerUsers { get; set; }
        public int CardTypeId { get; set; }
        public int DigitalWalletId { get; set; }
        public string ResponseEventId { get; set; }
        public string ElectronicCommerceIndicator { get; set; }
    }

    public class VivaEvent
    {
        public string Url { get; set; }
        public EventData EventData { get; set; } = new EventData();
        public DateTime Created { get; set; }
        public string CorrelationId { get; set; }
        public int EventTypeId { get; set; }
        public string Delay { get; set; }
        public string MessageId { get; set; }
        public string RecipientId { get; set; }
        public int MessageTypeId { get; set; }
    }


    public class PaymentOrderResponse
    {

        public long orderCode { get; set; }
        public long expiring { get; set; }
        public string transactionId { get; set; }
        public string eventId { get; set; }
        public string eci { get; set; }

    }


}
