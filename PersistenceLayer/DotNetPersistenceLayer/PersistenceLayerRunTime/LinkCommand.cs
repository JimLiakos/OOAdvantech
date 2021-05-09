namespace OOAdvantech.PersistenceLayerRunTime.Commands
{

    //TODO: Οταν έχει μια classA στο AssemblyA στην storageA που έχει σχέση με την classB
	//στην AssemblyB στην storageB και η AssemblyA εχει reference στην AssemblyB, υπάρχει 
	//περίπτωση στο storageB να έχει metadata μόνο για την AssemblyB ενώ η storageA εχει και τα
	//δύο τότε τα link και unlink commands θα έχουν κατα πάσα πιθανότητα 
	//να γίνει test case.
	/// <MetaDataID>{46DF6E02-E45C-4E2D-96A2-81D4F4E7F376}</MetaDataID>
	public abstract class LinkCommand : Command
	{
		/// <MetaDataID>{133856C2-5460-4762-81D4-FB4978F22BA5}</MetaDataID>
		public OOAdvantech.DotNetMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
		{
			get
			{ 
				DotNetMetaDataRepository.AssociationEnd associationEnd;
				associationEnd=LinkInitiatorAssociationEndAgent.RealAssociationEnd;
				if(associationEnd!=null)
					return associationEnd;
				else
					return LinkInitiatorAssociationEndAgent.RealAssociationEnd;
			}
		}
		/// <MetaDataID>{CF27C280-BC38-4A27-9299-99543B8EF396}</MetaDataID>
		protected AssociationEndAgent LinkInitiatorAssociationEndAgent;
        /// <MetaDataID>{8c01658b-67cb-4b69-9938-5e840a861510}</MetaDataID>
        public ObjectStorage ObjectStorage;
		/// <MetaDataID>{A45ABD16-FE67-4D6A-B65D-40E83496ED3C}</MetaDataID>
		public LinkCommand(ObjectStorage objectStorage,StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd)
		{
            ObjectStorage = objectStorage;
			RoleA=roleA;
			RoleB=roleB;
			RelationObject=relationObject;
			LinkInitiatorAssociationEndAgent=linkInitiatorAssociationEnd;
		}

		/// <MetaDataID>{17F49A0F-E93A-4F41-ADF5-A4E05C0251CA}</MetaDataID>
		public StorageInstanceAgent RelationObject;

		/// <MetaDataID>{E7B032F0-B01E-49BC-B13F-8A10F21ED0A6}</MetaDataID>
		//public RelResolver theResolver;
		/// <MetaDataID>{FFBE9DEA-6BD6-41A9-A28D-AC3D771B85A2}</MetaDataID>
		public StorageInstanceAgent RoleB;
		/// <MetaDataID>{037C5269-0050-43DE-9C04-8D770AF52D69}</MetaDataID>
		public StorageInstanceAgent RoleA;



		public static bool IsMultilingualLink(StorageInstanceAgent roleA, StorageInstanceAgent roleB, DotNetMetaDataRepository.Association association)
		{
			bool multilingual = false;
			if (roleB.RealStorageInstanceRef != null && roleB.RealStorageInstanceRef.Class.IsMultilingual(association.RoleA))
				multilingual = true;
			else if (roleA.RealStorageInstanceRef != null && roleA.RealStorageInstanceRef.Class.IsMultilingual(association.RoleB))
				multilingual = true;
			else if (roleA.ValueTypePath.Multilingual)
				multilingual = true;
			else if (roleB.ValueTypePath.Multilingual)
				multilingual = true;



			if (multilingual&&roleA.StorageIdentity != roleB.StorageIdentity)
				throw new System.NotSupportedException("the multilingual link doesn't supported");

			return multilingual;
		}


	}
}
