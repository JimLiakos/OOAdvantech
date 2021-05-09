namespace XMLPersistenceRunTime
{
	/// <MetaDataID>{116A09F8-743E-4907-8799-43FFDDB666CF}</MetaDataID>
	/// <summary></summary>
	public class XMLUpdateStorageInstanceCommand : PersistenceLayerRunTime.UpdateStorageInstanceCommand
	{
		/// <summary></summary>
		/// <MetaDataID>{F67631D4-AD6B-42C1-AABD-C780EA6A1F05}</MetaDataID>
		public override void Execute()
		{
			XMLPersistenceRunTime.XMLStorageSession ObjectStorageSession=
				(XMLPersistenceRunTime.XMLStorageSession)UpdatedStorageInstanceRef.ActiveStorageSession;
			string XQuery="Root/ObjectCollections/"+UpdatedStorageInstanceRef.MemoryInstance.GetType().FullName+
				"/Object[@oid = "+UpdatedStorageInstanceRef.ObjectID.ToString()+"]";
			System.Xml.XmlElement StorageInstance=(System.Xml.XmlElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);
			((XMLStorageInstanceRef)UpdatedStorageInstanceRef).SaveObjectState(StorageInstance);
			string FileName=StorageInstance.OwnerDocument.BaseURI;
			System.Uri FileUri =new System.Uri(FileName);
			FileName=FileUri.LocalPath;
			StorageInstance.OwnerDocument.Save(FileName);

		}
	}
}
