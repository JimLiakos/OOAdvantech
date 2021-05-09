namespace MetaDataLoadingSystem
{
	/// <MetaDataID>{9D343421-F2BD-4A43-B5F7-917D114CA0CA}</MetaDataID>
	public class kMetaDataDeleteAssociationLinkObjectCmnd : PersistenceLayerRunTime.DeleteAssociationLinkObjectCommand
	{
		/// <MetaDataID>{54ABFA48-E42B-43BC-A4AB-F39C86BF3584}</MetaDataID>
		public override void Execute()
		{
			base.Execute();

			MetaDataStorageSession ObjectStorageSession=
				(MetaDataStorageSession)AssociationObject.ActiveStorageSession;
			ObjectStorageSession.Dirty=true;
			System.Xml.XmlDocument XmlDocument=ObjectStorageSession.XMLDocument;
			string StorageName=XmlDocument.ChildNodes[0].Name;
			System.Xml.XmlElement RoleAStorageInstance=ObjectStorageSession.GetXMLElement(AssociationRoleA.MemoryInstance.GetType(),AssociationRoleA.ObjectID);
			System.Xml.XmlElement RoleBStorageInstance=ObjectStorageSession.GetXMLElement(AssociationRoleB.MemoryInstance.GetType(),AssociationRoleB.ObjectID);
			System.Xml.XmlElement AssociationObjectStorageInstance=ObjectStorageSession.GetXMLElement(AssociationObject.MemoryInstance.GetType(),AssociationObject.ObjectID);
			RoleAStorageInstance.SetAttribute("ReferentialIntegrityCount",AssociationRoleA.ReferentialIntegrityCount.ToString());
			RoleBStorageInstance.SetAttribute("ReferentialIntegrityCount",AssociationRoleB.ReferentialIntegrityCount.ToString());
			StorageName=XmlDocument.ChildNodes[0].Name;

			string StrObjectID=AssociationObject.ObjectID.ToString();
			foreach(System.Xml.XmlNode CurrNode in  RoleAStorageInstance.ChildNodes)
			{
				System.Xml.XmlElement Element=(System.Xml.XmlElement)CurrNode;
				string RoleBName=AssociationClass.LinkAssociation.RoleB.Name;
				if(RoleBName==null)
					RoleBName=AssociationClass.LinkAssociation.Name+"RoleBName";
				if(Element.Name==RoleBName)
				{
					System.Xml.XmlElement RoleBCollection=Element;
					foreach(System.Xml.XmlNode inCurrNode in  RoleBCollection.ChildNodes)
					{
						System.Xml.XmlElement inElement=(System.Xml.XmlElement)inCurrNode;
						if(inElement.GetAttribute("oid")==StrObjectID)
						{
							RoleBCollection.RemoveChild(inElement);
							break;
						}
					}
					break;
				}
			}

			
			foreach(System.Xml.XmlNode CurrNode in  RoleBStorageInstance.ChildNodes)
			{
				System.Xml.XmlElement Element=(System.Xml.XmlElement)CurrNode;
				string RoleAName=AssociationClass.LinkAssociation.RoleA.Name;
				if(RoleAName==null)
					RoleAName=AssociationClass.LinkAssociation.Name+"RoleAName";

				if(Element.Name==RoleAName)
				{
					System.Xml.XmlElement RoleACollection=Element;
					foreach(System.Xml.XmlNode inCurrNode in  RoleACollection.ChildNodes)
					{
						System.Xml.XmlElement inElement=(System.Xml.XmlElement)inCurrNode;
						if(inElement.GetAttribute("oid")==StrObjectID)
						{
							RoleACollection.RemoveChild(inElement);
							break;
						}
					}
					break;
				}
			}
		}
			
	}
}
