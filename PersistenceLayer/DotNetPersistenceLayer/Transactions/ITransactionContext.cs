namespace OOAdvantech.Transactions
{
    /// <MetaDataID>{F76833F6-17F0-47A5-BF9A-3629828694EA}</MetaDataID>
    public interface ITransactionContext
    {
        /// <MetaDataID>{AA7F89B0-2DB1-4773-A4AD-3AB443C092A3}</MetaDataID>
        Collections.Generic.List<ITransactionContextExtender> Extenders
        {
            get;
        }
        /// <MetaDataID>{CAB88F32-1685-4525-AF7E-FC7B3A775ACA}</MetaDataID>
        OOAdvantech.Transactions.ObjectsStateManagerStatus Status
        {
            set;
            get;
         }
    }

}
