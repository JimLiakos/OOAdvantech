namespace XMLPersistenceRunTime
{
	/// <MetaDataID>{92B16053-489D-4295-838E-6082E47B60E6}</MetaDataID>
	public class XMLUnLinkObjectsCommand : PersistenceLayerRunTime.UnLinkObjectsCommand
	{
		/// <MetaDataID>{AC6283C8-81DC-4177-A3E5-EDF15016B52B}</MetaDataID>
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
			XQuery="oid[@oid = "+RoleB.ObjectID.ToString()+"]";
			if(RoleBCollection!=null)
			{
				System.Xml.XmlNodeList Nodes=RoleBCollection.SelectNodes(XQuery);
				foreach(System.Xml.XmlNode currNode in Nodes)
					RoleBCollection.RemoveChild(currNode);
			}

			XQuery=MRoleA.Name;
			System.Xml.XmlElement RoleACollection=(System.Xml.XmlElement)RoleBStorageInstance.SelectSingleNode(XQuery);
			XQuery="oid[@oid = "+RoleA.ObjectID.ToString()+"]";
			if(RoleACollection!=null)
			{
				System.Xml.XmlNodeList Nodes=RoleACollection.SelectNodes(XQuery);
				foreach(System.Xml.XmlNode currNode in Nodes)
					RoleACollection.RemoveChild(currNode);
	
			}
			string FileName=ObjectStorageSession.XMLDocument.BaseURI;
			System.Uri FileUri =new System.Uri(FileName);
			FileName=FileUri.LocalPath;
			ObjectStorageSession.XMLDocument.Save(FileName);
*/

		}
	}
}
