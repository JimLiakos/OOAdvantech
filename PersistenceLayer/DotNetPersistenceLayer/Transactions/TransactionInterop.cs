namespace OOAdvantech.Transactions
{
    /// <MetaDataID>{B8EAB444-28F2-4AD0-8FE6-14E62073CE74}</MetaDataID>
    public class TransactionInterop
    {
        /// <MetaDataID>{0B44E2A6-71F1-4B7B-951C-1B57E04721C3}</MetaDataID>
        public static object GetSystemTransaction(Transaction transaction)
        {
            if (transaction == null)
                return null;
            return (transaction as TransactionRunTime).NativeTransaction;

            
        }
    }
}
