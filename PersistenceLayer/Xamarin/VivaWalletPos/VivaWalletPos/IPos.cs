﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VivaWalletPos
{
    public interface IPos
    {
        Task<PaymentData> Sale(decimal total,decimal tips);

        void Confing(POSType terminalType, string ipAddress = "", int port = 0);
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

    public enum POSType
    {
        AppPOS=1,
        TerminalPos=2;
    }



}
