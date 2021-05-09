namespace System.Transactions
{
    /// <MetaDataID>{9ef7f95b-3779-45e6-9b9d-1bc65e0fc5de}</MetaDataID>
    public class Transaction
    {
        public void Rollback()
        {
            
        }
        public void Rollback(object obj)
        {
        }
    }
    /// <MetaDataID>{b114db1d-c55d-4cd3-8aef-6c5adbe0b668}</MetaDataID>
    public class CommittableTransaction : Transaction
    {
        public void Commit()
        { 
        }

    }
    /// <MetaDataID>{99c12a52-5d5e-4c22-8c0b-ea633ab23fcf}</MetaDataID>
    internal class TransactionScope : IDisposable
    {
        public TransactionScope()
        {

        }
        public TransactionScope(Transaction transaction)
        {

        }

        #region IDisposable Members

        public void Dispose()
        {
            
        }
        public void Complete()
        {

        }

        #endregion
    }
    /// <MetaDataID>{8c6fc35b-4076-4c57-ab4e-f083f338d428}</MetaDataID>
public interface IEnlistmentNotification
    {
    }

}