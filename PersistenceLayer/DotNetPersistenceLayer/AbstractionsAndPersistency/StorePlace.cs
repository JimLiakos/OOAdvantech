namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using OOAdvantech.Synchronization;
    using OOAdvantech.Remoting;
    using System;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{4A04C25E-8897-4887-925B-41AA257A4864}</MetaDataID>
    [BackwardCompatibilityID("{4A04C25E-8897-4887-925B-41AA257A4864}")]
	[Persistent]
	public class StorePlace :MarshalByRefObject,  OOAdvantech.Remoting.IExtMarshalByRefObject,IStorePlace
	{
        /// <MetaDataID>{c9ab4fb4-d144-4e94-82c9-9e08bb242d75}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
		/// <MetaDataID>{904FCF80-FB1B-423A-93B5-52A927A3F5EB}</MetaDataID>
		 public StorePlace(string name)
		{
			_Name=name;
		}
		/// <MetaDataID>{F9D0FD6A-5524-4FEF-B75E-362BEC6BF399}</MetaDataID>
		 protected StorePlace()
		{

		}

         /// <MetaDataID>{8d328205-678d-460e-b01a-ec1933882054}</MetaDataID>
		private OOAdvantech.ObjectStateManagerLink Properties=new OOAdvantech.ObjectStateManagerLink();
        /// <MetaDataID>{59E290EA-BB78-48F8-9711-0B51A620A1CD}</MetaDataID>
        public virtual void AddProduct(IProduct product)
        {


            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StoredProducts.Add(product);
                    stateTransition.Consistent = true;

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }
        /// <MetaDataID>{4830E343-F075-4433-94CC-610C021F6D70}</MetaDataID>
        public virtual void RemoveProduct(IProduct product)
        {
            
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StoredProducts.Remove(product);
                    stateTransition.Consistent = true;

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{AE6C232F-00DD-4583-8277-6D57B7A5E343}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<IProduct> _StoredProducts=new OOAdvantech.Collections.Generic.Set<IProduct>();
        /// <MetaDataID>{26BAA570-F4DE-4A3C-B6B9-9825F7ACD363}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_StoredProducts")]
        public OOAdvantech.Collections.Generic.Set<IProduct> StoredProducts
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                     return new OOAdvantech.Collections.Generic.Set<IProduct>(_StoredProducts,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
          
        }
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{A34F30AF-AB5D-4642-93EB-B7FFACDDE9D7}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{6FBB6978-EE66-4435-AB1A-243EB1FA1824}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_Name")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}
	}
}
