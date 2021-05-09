using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{

    /// <MetaDataID>{2e06e2c9-3ea0-47bc-ae22-3a3e11ad4c31}</MetaDataID>
    public struct AccountNumber : OOAdvantech.IAccount<AccountNumber>
    {


        /// <MetaDataID>{edce654a-2c86-4546-b326-613b33c3332d}</MetaDataID>
        public AccountNumber(decimal amount)
        {
            Amount = amount;
        }
        /// <MetaDataID>{19031618-1179-458e-93d2-01111976f05d}</MetaDataID>
        public decimal Amount;

        /// <MetaDataID>{96ece64c-eeca-4fa2-aefe-f1897bc4d43e}</MetaDataID>
        public static implicit operator AccountNumber(decimal amount)
        {
            return new AccountNumber(amount);
        }


        /// <MetaDataID>{56c1d33c-ed30-474d-8174-2f3cdf91c245}</MetaDataID>
        public static AccountNumber operator +(AccountNumber left, AccountNumber right)
        {
            //return left.Amount + left.UnitMeasure.Convert(right).Amount;
            return new AccountNumber(left.Amount + right.Amount);
        }

        /// <MetaDataID>{611850e7-3e2e-4aae-bf42-16e433b1f69c}</MetaDataID>
        public static AccountNumber operator -(AccountNumber left, AccountNumber right)
        {
            return new AccountNumber(left.Amount - right.Amount);
        }

        /// <MetaDataID>{de62b44d-9646-433d-9a7b-08264836cb59}</MetaDataID>
        public static AccountNumber operator --(AccountNumber accountNumber)
        {
            return new AccountNumber(--accountNumber.Amount);
        }
        /// <MetaDataID>{418e6b80-636a-4fdf-be76-d0fd9052ff84}</MetaDataID>
        public static AccountNumber operator ++(AccountNumber accountNumber)
        {
            return new AccountNumber(++accountNumber.Amount);
        }
        /// <MetaDataID>{ab7c9a28-e799-4af7-9bbc-b9b179557a35}</MetaDataID>
        public static AccountNumber operator -(AccountNumber left, decimal amount)
        {
            return new AccountNumber(left.Amount - amount);
        }

        /// <MetaDataID>{bac33bbd-d51f-45f4-9b72-b69f39d60316}</MetaDataID>
        public static AccountNumber operator +(AccountNumber left, decimal amount)
        {
            //return left.Amount + left.UnitMeasure.Convert(right).Amount;
            return new AccountNumber(left.Amount + amount);
        }


        /// <MetaDataID>{f239c13f-d24e-4775-8750-b661dd5ef1ef}</MetaDataID>
        public static bool operator ==(AccountNumber left, AccountNumber right)
        {

            //if (left == null && right == null)
            //    return true;
            //if (left == null && right != null)
            //    return false;
            //if (left != null && right == null)
            //    return false;
            if (left.Amount == right.Amount)// && left.UnitMeasure == right.UnitMeasure)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{6d178ad2-2d4b-46b9-8edb-2f60ac53d08e}</MetaDataID>
        public static bool operator !=(AccountNumber left, AccountNumber right)
        {
            return !(left == right);
        }
        /// <MetaDataID>{81dcd519-1d7f-4fe8-b141-8bbb7a50d3b6}</MetaDataID>
        object OOAdvantech.IAccount<AccountNumber>.GetTransaction(AccountNumber newValue)
        {

            return this - newValue;
        }

        /// <MetaDataID>{9a520745-06d7-4faa-9cb9-059e30f95fcd}</MetaDataID>
        AccountNumber OOAdvantech.IAccount<AccountNumber>.MakeTransaction(object transaction)
        {
            return this - ((AccountNumber)transaction);
        }
        /// <MetaDataID>{f0c2a47e-89c9-4898-8655-56215350f4c8}</MetaDataID>
        public static AccountNumber GetQuantity(decimal amount)
        {
            return new AccountNumber(amount);
        }
    }
}
