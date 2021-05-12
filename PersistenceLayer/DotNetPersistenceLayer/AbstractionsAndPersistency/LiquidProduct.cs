namespace AbstractionsAndPersistency
{
    using OOAdvantech.Synchronization;
    using OOAdvantech.Transactions;
    /// <MetaDataID>{18ebd1a6-07e3-4bb9-bdf0-8f179f15548f}</MetaDataID>
    public class LiquidProduct : AbstractionsAndPersistency.MaterialProduct
    {

        /// <MetaDataID>{82412d87-f32c-41b2-b577-3b38a5c4d29d}</MetaDataID>
        public LiquidProduct(string name)
            : base(name)
		{
              
              
		}
        /// <MetaDataID>{6aa0443b-9d07-4c48-8734-5522e389d2c9}</MetaDataID>
        public override void AddStorePlace(IStorePlace storePlace)
        {

            if (storePlace != null && !_LiquidStorePlaces.Contains(storePlace as LiquidStore))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _LiquidStorePlaces.Add(storePlace as LiquidStore);
                    objStateTransition.Consistent = true;
                }
            }

        }
        /// <MetaDataID>{56c833b9-073c-4a14-93ea-9f5c22845284}</MetaDataID>
        protected LiquidProduct()
        {
        }
        [OOAdvantech.MetaDataRepository.Association("LiquidProductsInStore", typeof(LiquidStore), OOAdvantech.MetaDataRepository.Roles.RoleA, "8bdd1986-7fe0-4593-870a-2ab6e76dc433", "{932F5707-C060-4CB9-86C9-D7E9296813BE}")]
        [OOAdvantech.MetaDataRepository.PersistentMember("_LiquidStorePlaces")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.LiquidStore> LiquidStorePlaces
        {


            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.LiquidStore>(_LiquidStorePlaces, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{0ce1fb62-91a8-4090-875d-5027f7694161}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.LiquidStore> _LiquidStorePlaces=new OOAdvantech.Collections.Generic.Set<LiquidStore>();

    
    }
}
