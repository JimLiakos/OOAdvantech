namespace MetaDataLoadingSystem
{
	/// <MetaDataID>{3DC6FFEA-4633-451E-9FA1-7356D6021940}</MetaDataID>
	public class kMetaDataAssociationLinkObjectCmnd : PersistenceLayerRunTime.AssociationLinkObjectCommand
	{
		/// <MetaDataID>{6946BB11-66FC-4196-88D8-19F147A9AB66}</MetaDataID>
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
			System.Xml.XmlElement RoleACollection=null;
			System.Xml.XmlElement RoleBCollection=null;


			bool AlreadyExist=false;
			foreach(System.Xml.XmlNode CurrNode in  RoleAStorageInstance.ChildNodes)
			{
				string StrObjectID=AssociationObject.ObjectID.ToString();
				System.Xml.XmlElement Element=(System.Xml.XmlElement)CurrNode;
					string RoleBName=AssociationClass.LinkAssociation.RoleB.Name;
				if(RoleBName==null)
					RoleBName=AssociationClass.LinkAssociation.Name+"RoleBName";
				if(Element.Name==RoleBName)
				{
					RoleBCollection=Element;
					foreach(System.Xml.XmlNode inCurrNode in  RoleBCollection.ChildNodes)
					{
						System.Xml.XmlElement inElement=(System.Xml.XmlElement)inCurrNode;
						if(inElement.GetAttribute("oid")==StrObjectID)
						{
							AlreadyExist=true;
							break;
						}
					}
					break;
				}
			}
			if(!AlreadyExist)
			{
				if(RoleBCollection==null)
				{
					string RoleBName=AssociationClass.LinkAssociation.RoleB.Name;
					if(RoleBName==null)
						RoleBName=AssociationClass.LinkAssociation.Name+"RoleBName";
					RoleBCollection=RoleAStorageInstance.OwnerDocument.CreateElement(RoleBName);
					RoleAStorageInstance.AppendChild(RoleBCollection);
				}
				System.Xml.XmlElement NewXmlElement=RoleBCollection.OwnerDocument.CreateElement("oid");
				RoleBCollection.AppendChild(NewXmlElement);//(System.Xml.XmlNode)
				NewXmlElement.SetAttribute("oid",AssociationObject.ObjectID.ToString());
				NewXmlElement.SetAttribute("ClassInstaditationName",AssociationObject.MemoryInstance.GetType().FullName);
			}
			AlreadyExist=false;

			foreach(System.Xml.XmlNode CurrNode in  RoleBStorageInstance.ChildNodes)
			{
				string StrObjectID=AssociationObject.ObjectID.ToString();
				System.Xml.XmlElement Element=(System.Xml.XmlElement)CurrNode;
				string RoleAName=AssociationClass.LinkAssociation.RoleA.Name;
				if(RoleAName==null)
					RoleAName=AssociationClass.LinkAssociation.Name+"RoleAName";
				if(Element.Name==RoleAName)
				{
					RoleACollection=Element;
					foreach(System.Xml.XmlNode inCurrNode in  RoleACollection.ChildNodes)
					{
						System.Xml.XmlElement inElement=(System.Xml.XmlElement)inCurrNode;
						if(inElement.GetAttribute("oid")==StrObjectID)
						{
							AlreadyExist=true;
							break;
						}
					}
					break;
				}
			}
			if(!AlreadyExist)
			{
				if(RoleACollection==null)
				{
					string RoleAName=AssociationClass.LinkAssociation.RoleA.Name;
					if(RoleAName==null)
						RoleAName=AssociationClass.LinkAssociation.Name+"RoleAName";
					RoleACollection=RoleAStorageInstance.OwnerDocument.CreateElement(RoleAName);
					RoleBStorageInstance.AppendChild(RoleACollection);
				}
				System.Xml.XmlElement NewXmlElement=RoleACollection.OwnerDocument.CreateElement("oid");
				RoleACollection.AppendChild(NewXmlElement);//(System.Xml.XmlNode)
				NewXmlElement.SetAttribute("oid",AssociationObject.ObjectID.ToString());
				NewXmlElement.SetAttribute("ClassInstaditationName",AssociationObject.MemoryInstance.GetType().FullName);
			}
			AlreadyExist=false;
			AssociationObjectStorageInstance.SetAttribute("RoleA",AssociationRoleA.ObjectID.ToString());
			AssociationObjectStorageInstance.SetAttribute("RoleAClassInstaditationName",AssociationRoleA.MemoryInstance.GetType().FullName);
			AssociationObjectStorageInstance.SetAttribute("RoleB",AssociationRoleB.ObjectID.ToString());
			AssociationObjectStorageInstance.SetAttribute("RoleBClassInstaditationName",AssociationRoleB.MemoryInstance.GetType().FullName);
		}
	}
}
