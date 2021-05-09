namespace XMLPersistenceRunTime
{
	/// <MetaDataID>{55742672-0E77-4A1E-AAC7-36E96B7226F3}</MetaDataID>
	public class XMLLinkObjectsCommand : PersistenceLayerRunTime.LinkObjectsCommand
	{
		/// <MetaDataID>{24A5021F-9E8C-4B29-BB45-8F698B4BF87F}</MetaDataID>
		public override void Execute()
		{
			#region Preconditions Chechk
			if(RoleA==null||RoleB==null)
				throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
			if(theResolver==null)
				throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
			#endregion
			base.Execute();
			/*XMLRelResolver mResolver=(XMLRelResolver)theResolver;
			METAMODELLib.MRole MRoleA;
			METAMODELLib.MRole MRoleB;
			if(mResolver.DestinationObjectRole.IsRoleA!=0)
			{
				MRoleA=mResolver.DestinationObjectRole;
				MRoleB=mResolver.DestinationObjectRole.GetOtherEndRole();
			}
			else
			{
				MRoleB=mResolver.DestinationObjectRole;
				MRoleA=mResolver.DestinationObjectRole.GetOtherEndRole();
			}
			XMLPersistenceRunTime.XMLStorageSession ObjectStorageSession=
				(XMLPersistenceRunTime.XMLStorageSession)RoleA.ActiveStorageSession;
			string XQuery="Root/ObjectCollections/"+RoleA.MemoryInstance.GetType().FullName+
				"/Object[@oid = "+RoleA.ObjectID.ToString()+"]";
			System.Xml.XmlElement RoleAStorageInstance=(System.Xml.XmlElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);

			ObjectStorageSession=
				(XMLPersistenceRunTime.XMLStorageSession)RoleB.ActiveStorageSession;
			XQuery="Root/ObjectCollections/"+RoleB.MemoryInstance.GetType().FullName+
				"/Object[@oid = "+RoleB.ObjectID.ToString()+"]";
			System.Xml.XmlElement RoleBStorageInstance=(System.Xml.XmlElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);
			//XQuery="Object/"+MRoleB.Name;
			XQuery=MRoleB.Name;
			System.Xml.XmlElement RoleBCollection=(System.Xml.XmlElement)RoleAStorageInstance.SelectSingleNode(XQuery);
			if(RoleBCollection==null)
			{
				RoleBCollection=RoleAStorageInstance.OwnerDocument.CreateElement(MRoleB.Name);
				RoleAStorageInstance.AppendChild(RoleBCollection);
			}
			XQuery=MRoleB.Name+"/oid[@oid = "+RoleB.ObjectID.ToString()+"]";
			if(RoleBCollection.SelectNodes(XQuery).Count==0)
			{
				System.Xml.XmlElement NewXmlElement=RoleBCollection.OwnerDocument.CreateElement("oid");
				RoleBCollection.AppendChild(NewXmlElement);//(System.Xml.XmlNode)
				NewXmlElement.SetAttribute("oid",RoleB.ObjectID.ToString());
			}

			//XQuery="Object/"+MRoleA.Name;
			XQuery=MRoleA.Name;
			System.Xml.XmlElement RoleACollection=(System.Xml.XmlElement)RoleBStorageInstance.SelectSingleNode(XQuery);
			if(RoleACollection==null)
			{
				RoleACollection=RoleBStorageInstance.OwnerDocument.CreateElement(MRoleA.Name);
				RoleBStorageInstance.AppendChild(RoleACollection);
			}
			XQuery=MRoleA.Name+"/oid[@oid = "+RoleA.ObjectID.ToString()+"]";
			if(RoleACollection.SelectNodes(XQuery).Count==0)
			{
				System.Xml.XmlElement NewXmlElement=RoleACollection.OwnerDocument.CreateElement("oid");
				
				RoleACollection.AppendChild(NewXmlElement);//(System.Xml.XmlNode)
				NewXmlElement.SetAttribute("oid",RoleA.ObjectID.ToString());
			}
			string FileName=RoleACollection.OwnerDocument.BaseURI;
			System.Uri FileUri =new System.Uri(FileName);
			FileName=FileUri.LocalPath;
			RoleACollection.OwnerDocument.Save(FileName);



*/


			
			//RoleA;
			//RoleB;
		}
	}
}
