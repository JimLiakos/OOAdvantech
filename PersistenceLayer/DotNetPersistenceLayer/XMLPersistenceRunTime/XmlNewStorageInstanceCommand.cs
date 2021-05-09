namespace OOAdvantech.XMLPersistenceRunTime.Commands
{
	/// <MetaDataID>{93449C9A-2A86-4098-9E8F-D5307EB70C90}</MetaDataID>
	/// <summary></summary>
    public class NewStorageInstanceCommand : PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
	{

        public NewStorageInstanceCommand(StorageInstanceRef storageInstanceRef)
            : base(storageInstanceRef)
		{
		}

        /// <MetaDataID>{16F1108E-74F5-4637-8D84-5721767B46E9}</MetaDataID>
        public override void Execute()
        {
            int ObjID = System.Convert.ToInt32(((System.Xml.XmlElement)NextObjectIDNode).GetAttribute("ObjID"), 10);
            ObjID++;
            ((System.Xml.XmlElement)NextObjectIDNode).SetAttribute("ObjID", ObjID.ToString());
            string FileName = NextObjectIDNode.OwnerDocument.BaseURI;
            System.Xml.XmlElement NewXmlElement = ObjectCollectionNode.OwnerDocument.CreateElement("Object");
            NewXmlElement = (System.Xml.XmlElement)ObjectCollectionNode.AppendChild((System.Xml.XmlNode)NewXmlElement);
            ObjID--;
            NewXmlElement.SetAttribute("oid", ObjID.ToString());
            OnFlyStorageInstance.ObjectID = ObjID;
            ((StorageInstanceRef)OnFlyStorageInstance).StorageIstance = NewXmlElement;
            ((StorageInstanceRef)OnFlyStorageInstance).SaveObjectState();

            ObjectStorage objectStorage=
                (ObjectStorage)OnFlyStorageInstance.ObjectStorage;
            objectStorage.Dirty = true;
            objectStorage.NewNodeUnderTransaction(NewXmlElement, OwnerTransactiont);

        }

		/// <summary></summary>
		/// <MetaDataID>{19AD32F9-37F5-4AEC-A15A-5AFE5116E9D5}</MetaDataID>
		public System.Xml.XmlNode NextObjectIDNode;
		/// <summary></summary>
		/// <MetaDataID>{1522AC28-5641-4B9C-BF7D-05363D9FA95B}</MetaDataID>
		public System.Xml.XmlNode ObjectCollectionNode;

  
    }
}
