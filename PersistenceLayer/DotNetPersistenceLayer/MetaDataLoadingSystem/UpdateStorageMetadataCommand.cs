using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
	

	public class UpdateStorageMetadataCommand : PersistenceLayerRunTime.Commands.Command
	{
		/// <MetaDataID>{81471910-9BE5-4611-9317-EF6F6ED7D0AA}</MetaDataID>
		public MetaDataStorageSession UpdatingStorage;
		/// <summary>Priority defines the order in which will be executed the command.</summary>
		/// <MetaDataID>{0E14CD33-E430-480C-A8F7-2FEA0BE09A4D}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 15;
			}
		}
		/// <MetaDataID>{4F784CAE-5F7E-4934-9B4A-5C598FE82FEB}</MetaDataID>
		public override void GetSubCommands(int currentOrder)
		{

		}
	


		/// <MetaDataID>{2F737F99-5BB2-4C57-A6D1-C33F897D5C7B}</MetaDataID>
		public UpdateStorageMetadataCommand(MetaDataStorageSession Storage)
		{
			UpdatingStorage = Storage;

		}
		/// <MetaDataID>{7BFFED5B-5D81-422C-864E-8ABF2F4C340E}</MetaDataID>
		public override void Execute()
		{

			UpdatingStorage.XMLDocument.Root.SetAttribute("Culture", UpdatingStorage.StorageMetaData.Culture);
		}

		public override string Identity
		{
			get
			{
				return "UpdateStorageMetadata" + UpdatingStorage.StorageMetaData.StorageIdentity;
			}
		}
		/// <MetaDataID>{b1c29b3c-72c1-4adf-88d6-22c0b3cd52c4}</MetaDataID>
		public static string GetIdentity(OOAdvantech.PersistenceLayerRunTime.ObjectStorage updatingStorage)
		{
			return "UpdateStorageMetadata" + updatingStorage.StorageMetaData.StorageIdentity;
		}
	}
}
