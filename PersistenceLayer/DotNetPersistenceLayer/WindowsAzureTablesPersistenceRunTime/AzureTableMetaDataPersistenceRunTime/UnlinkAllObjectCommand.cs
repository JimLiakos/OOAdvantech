namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
	/// <MetaDataID>{3D242F8D-2574-4762-9769-8406C396F23D}</MetaDataID>
	public class UnlinkAllObjectCommand : OOAdvantech.PersistenceLayerRunTime.Commands.OnMemoryUnlinkAllObjectCommand
	{
        public UnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstance)
            :base(deletedStorageInstance)
        {
        }
		/// <MetaDataID>{FD8C3DF8-4C69-4988-B893-39BF9EF6475D}</MetaDataID>
		protected override PersistenceLayerRunTime.RelResolver theResolver
		{
			get
			{
				foreach(RelResolver relResolver  in DeletedStorageInstance.RealStorageInstanceRef.RelResolvers)
				{
                    if (relResolver.AssociationEnd == theAssociationEnd && relResolver.ValueTypePath==DeletedStorageInstance.ValueTypePath)
						return relResolver;
				}
				return null;

			}

		}
		/// <MetaDataID>{29A90F61-02BA-4E33-9B0A-45B7F3C9379E}</MetaDataID>
		public override void GetSubCommands(int CurrentOrder)
		{
			base.GetSubCommands(CurrentOrder);
		
		}


		/// <MetaDataID>{3E9B8E14-9926-4013-815E-3E0D8818F32C}</MetaDataID>
		public override void Execute()
		{
			base.Execute();
		
		}
	}
}
