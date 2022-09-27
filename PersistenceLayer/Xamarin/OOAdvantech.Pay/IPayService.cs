﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Pay
{
    public interface IPayService
    {
        event EventHandler<string> OnTokenizationSuccessful;

        event EventHandler<string> OnTokenizationError;

        bool CanPay { get; }

        Task<bool> InitializeAsync(string clientToken);

        Task<string> TokenizeCard(string panNumber = "4111111111111111", string expirationMonth = "12", string expirationYear = "2018", string cvv = null);
    }
}
