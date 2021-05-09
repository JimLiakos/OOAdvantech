namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{4D861763-A7D5-4649-9A26-8C15AE817618}</MetaDataID>
	/// <summary></summary>
	public abstract class UpdateStorageInstanceCommand : Command
	{
		/// <MetaDataID>{A72BB7E6-E45D-48BE-A201-30DDD4608F77}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "update"+UpdatedStorageInstanceRef.MemoryID.ToString();
			}
		}
		/// <MetaDataID>{603707D9-824F-4A38-9C87-D04E359C97EA}</MetaDataID>
		public static string GetIdentity(StorageInstanceRef storageInstanceRef)
		{
			return "update"+storageInstanceRef.MemoryID.ToString();
		}

		/// <MetaDataID>{93F3AA1A-E814-4F10-AFBB-B05953488044}</MetaDataID>
		private bool SubTransactionCmdsProduced;
		/// <MetaDataID>{0DA842FE-BFE2-4DBD-999D-9954B2697E5B}</MetaDataID>
		public UpdateStorageInstanceCommand (StorageInstanceRef updatedStorageInstanceRef)
		{
			SubTransactionCmdsProduced=false;
			_UpdatedStorageInstanceRef=updatedStorageInstanceRef;
		}

	

		/// <MetaDataID>{585BB08B-CD83-4221-9A0C-D9F2ABEDB52F}</MetaDataID>
		private StorageInstanceRef _UpdatedStorageInstanceRef;
		/// <MetaDataID>{D0729F5E-09EF-4E71-8B20-FE09D168D5F4}</MetaDataID>
		public StorageInstanceRef UpdatedStorageInstanceRef
		{
			get
			{
				return _UpdatedStorageInstanceRef;
			}
			/*
			set
			{
				if(_UpdatedStorageInstanceRef!=null&&_UpdatedStorageInstanceRef!=value)
					throw new System.Exception("System tries to create one update command for tow persistent object.");//message
				_UpdatedStorageInstanceRef=value;
				
			}*/
		}
		/// <MetaDataID>{AB96A972-1EAB-47C4-A872-CD0F394CCBB2}</MetaDataID>
		public override void GetSubCommands(int currentOrder)
		{
			if(!SubTransactionCmdsProduced)
			{
				SubTransactionCmdsProduced=true;

                OOAdvantech.MetaDataRepository.Operation operation = UpdatedStorageInstanceRef.Class.BeforeCommitObjectStateInStorage;
                if (operation != null)
                {
                    System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                    OOAdvantech.AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(UpdatedStorageInstanceRef.MemoryInstance, new object[0]);
                }
                else if (UpdatedStorageInstanceRef.MemoryInstance is OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer)
                    (UpdatedStorageInstanceRef.MemoryInstance as OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer).BeforeCommitObjectState();

				UpdatedStorageInstanceRef.MakeRelationChangesCommands();
			}
		}
		/// <MetaDataID>{B1BA5C3F-147A-4A8C-9F6D-BC55038A80FE}</MetaDataID>
		public override int ExecutionOrder
		{
			get 
			{
				return 60;
			}
		}

        public void RefreshSubCommands()
        {
            SubTransactionCmdsProduced = false;
        }

	}
}
