namespace OOAdvantech.MetaDataLoadingSystem
{
	/// <MetaDataID>{249B582D-ED06-4A55-9718-D6EBFA952D75}</MetaDataID>
	public class kMetaDataAssociationClassRelResolver : PersistenceLayerRunTime.RelResolver
	{
		public kMetaDataAssociationClassRelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner,DotNetMetaDataRepository.AssociationEnd associationEnd):base(theOwner,associationEnd)
		{

		}

		/// <MetaDataID>{BF29BEFF-C6F5-4D4B-857C-4C9348249DC3}</MetaDataID>
		public override void UnLinkAllObjects(object theObject)
		{
		
		}
		/// <MetaDataID>{A548F4ED-AF7F-4414-A9A9-76AF526D27A2}</MetaDataID>
		public override PersistenceLayerRunTime.Commands.TransactionCommandCollection LinkObject(PersistenceLayerRunTime.StorageInstanceRef  theObject)
		{
			return new PersistenceLayerRunTime.Commands.TransactionCommandCollection();

		}
		/// <MetaDataID>{0DBD3CED-9C74-46D3-9458-BD40EEB2D47F}</MetaDataID>
		public override System.Collections.Hashtable GetLinkedObjects(string Criterion)
		{

			bool SomeRefObjectCantFind=false;
			System.Collections.Hashtable ObjectCollection=new System.Collections.Hashtable();
			if(Owner.ObjectID==null)
				return ObjectCollection;
			MetaDataStorageSession OwnerStorageSession=(MetaDataStorageSession)Owner.ActiveStorageSession;
			System.Xml.XmlElement Element= OwnerStorageSession.GetXMLElement(Owner.MemoryInstance.GetType(),Owner.ObjectID);
			foreach(System.Xml.XmlElement CurrElement in Element.ChildNodes)
			{
				string RoleName=AssociationEnd.Name;
				if(RoleName==null)
					if(AssociationEnd.IsRoleA)
						RoleName=AssociationEnd.Association.Name+"RoleAName";
					else
						RoleName=AssociationEnd.Association.Name+"RoleBName";

				if(CurrElement!=null)
					if(CurrElement.Name==RoleName)
					{
						foreach(System.Xml.XmlElement RefElement in CurrElement.ChildNodes)
						{
							object RefObjectID=System.Convert.ChangeType(RefElement.GetAttribute("oid"),Owner.ObjectID.GetType());
							string ClassInstaditationName=RefElement.GetAttribute("ClassInstaditationName");
							System.Type StorageInstanceType=ModulePublisher.ClassRepository.GetType(ClassInstaditationName,"");

							//PersistenceLayerRunTime.ClassMemoryInstanceCollection ClassObjects=((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).OperativeObjectCollections[StorageInstanceType];
							
							
							PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef=(PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(ClassInstaditationName,"Version")][RefObjectID];
							if(StorageInstanceRef!=null)
							{
								ObjectCollection.Add (StorageInstanceRef.MemoryInstance,StorageInstanceRef.MemoryInstance);
								continue;
							}
							System.Xml.XmlElement mElement= OwnerStorageSession.GetXMLElement(StorageInstanceType,RefObjectID);
							if(mElement==null)
							{
								SomeRefObjectCantFind=true;
								continue;
							}

							object NewObject=ModulePublisher.ClassRepository.CreateInstance(ClassInstaditationName,"");
							if(NewObject==null)
								throw new System.Exception("PersistencyContext can't instadiate the "+ClassInstaditationName);
							StorageInstanceRef=(MetaDataStorageInstanceRef)((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).CreateStorageInstanceRef(NewObject);
							StorageInstanceRef.ObjectID=RefObjectID;
							((MetaDataStorageInstanceRef)StorageInstanceRef).TheStorageIstance=mElement;
							((MetaDataStorageInstanceRef)StorageInstanceRef).LoadObjectState();
							StorageInstanceRef.SnapshotStorageInstance();
							ObjectCollection.Add (NewObject,NewObject);
						}
					}
			}
			return ObjectCollection;

		}
		/// <MetaDataID>{FDBF2AE0-0E12-442A-A1D6-E5F500BBF1BC}</MetaDataID>
		public override PersistenceLayerRunTime.Commands.TransactionCommandCollection UnLinkObject(PersistenceLayerRunTime.StorageInstanceRef  theObject)
		{
			return null;
		}
		/// <MetaDataID>{A28E1DCB-EF9B-4FC4-8C9E-5A5A6BE3E702}</MetaDataID>
		public override System.Collections.ArrayList GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
		{
			return new System.Collections.ArrayList();
		}
		/// <MetaDataID>{8D513509-DB9E-4F3D-BEDB-BA06FA0B61E7}</MetaDataID>
		public override long GetLinkedObjectsCount()
		{
			return 0;
		}
	}
}
