namespace AbstractionsAndPersistency
{
    using OOAdvantech.Transactions;
    using OOAdvantech.Synchronization;
    /// <MetaDataID>{b2e9e059-b198-4b64-9012-1cbbb5335e50}</MetaDataID>
    public class LiquidStore : AbstractionsAndPersistency.StorePlace
    {
        /// <MetaDataID>{1284e045-35d0-4058-8dcb-4a6799994c10}</MetaDataID>
        public LiquidStore(string name)
            : base(name)
        { 
        }
        /// <MetaDataID>{8a369737-6c5e-4e09-976a-6e5a80f98890}</MetaDataID>
        protected LiquidStore()
        {
        }
        /// <MetaDataID>{46980438-882a-4d28-ab63-d0a698c9132a}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<AbstractionsAndPersistency.LiquidProduct> _StoredLiquidProduct;
        [OOAdvantech.MetaDataRepository.Association("LiquidProductsInStore", typeof(LiquidProduct), OOAdvantech.MetaDataRepository.Roles.RoleB, "8bdd1986-7fe0-4593-870a-2ab6e76dc433", "{932F5707-C060-4CB9-86C9-D7E9296813BE}")]
        [OOAdvantech.MetaDataRepository.PersistentMember("_StoredLiquidProduct")]
        //[OOAdvantech.MetaDataRepository.AssociationEndBehavior(OOAdvantech.MetaDataRepository.PersistencyFlag.OnConstruction)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public AbstractionsAndPersistency.LiquidProduct StoredLiquidProduct
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _StoredLiquidProduct;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_StoredLiquidProduct.Value != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _StoredLiquidProduct.Value = value;
                            stateTransition.Consistent = true;
                        }

                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
    }
}
