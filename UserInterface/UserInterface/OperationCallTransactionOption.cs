using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{b8e13acf-708c-4822-81b5-823390c31ecc}</MetaDataID>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class OperationCallTransactionOption:Attribute
    {
       public readonly Transactions.TransactionOption[] TransactionOptions;
        public OperationCallTransactionOption(Transactions.TransactionOption[] transactionOptions)
        {
            TransactionOptions = transactionOptions;

        }

    }
}
