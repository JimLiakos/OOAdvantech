using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    public interface IPos
    {
        Task<PaymentData> Sale(double total,double tips);
    }

    public class PaymentData
    {
        public string CardType { get; set; }
        public string AccountNum { get; set; }
        public string TransactionID { get; set; }
        public string PayAmount { get; set; }

        public bool Failed { get; set; }

        public string ErrorMessage { get; set; }
    }



}
