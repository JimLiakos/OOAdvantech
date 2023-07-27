using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Pay
{
    /// <MetaDataID>{76da7369-f34f-44ac-8226-03f4b4023b32}</MetaDataID>
    public interface IPayService
    {
        event EventHandler<string> OnTokenizationSuccessful;

        event EventHandler<string> OnTokenizationError;

        bool CanPay { get; }

        Task<bool> InitializeAsync(string clientToken);

        Task<string> TokenizeCard(string panNumber = "4111111111111111", string expirationMonth = "12", string expirationYear = "2018", string cvv = null);

        Task<string> TokenizePlatform(double totalPrice, string merchantId);
    }
}
