using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    /// <MetaDataID>{891115e6-cc27-448a-8e67-d949b1f0ce0c}</MetaDataID>
    public interface IPos
    {
        Task<PaymentData> ReceivePayment(decimal total, decimal tips);

        void Confing(POSType terminalType, string ipAddress = "", int port = 0, double waitTimeOutInSec = 0);
    }

    /// <MetaDataID>{cae5e919-5f19-4779-85d5-de2d7d8cbf38}</MetaDataID>
    public class PaymentData
    {
        public string CardType { get; set; }
        public string AccountNum { get; set; }
        public string TransactionID { get; set; }
        public string PayAmount { get; set; }

        public bool Failed { get; set; }

        public string ErrorMessage { get; set; }
    }

    /// <MetaDataID>{bee83aa2-fb7f-40c0-8e77-062804ba16fb}</MetaDataID>
    public enum POSType
    {
        AppPOS,
        TerminalPos
    }


}
