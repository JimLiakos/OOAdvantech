using System.Xml.Linq;
using System.Linq;
namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
	/// <MetaDataID>{1E14AA64-CBE4-4611-AAA8-CE7F24D7F1E0}</MetaDataID>
	public class MetaDataUpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
	{
		public MetaDataUpdateStorageInstanceCommand (PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef):base(updatedStorageInstanceRef)
		{ 
		}
		/// <MetaDataID>{5AC83263-7436-4CB8-ADAC-0C762867C6F9}</MetaDataID>
		public override void Execute()
		{
			if(!UpdatedStorageInstanceRef.HasChangeState())
				return;
			MetaDataStorageSession ObjectStorageSession=
				(MetaDataStorageSession)UpdatedStorageInstanceRef.ObjectStorage;
			ObjectStorageSession.Dirty=true;

            if (((MetaDataStorageInstanceRef)UpdatedStorageInstanceRef).TheStorageIstance == null)
            {
                string StorageName = ObjectStorageSession.XMLDocument.Root.Name.LocalName;


                XElement storageInstance = (from element in ObjectStorageSession.XMLDocument.Root.Element("ObjectCollections").Element(UpdatedStorageInstanceRef.MemoryInstance.GetType().FullName).Elements()
                                            where element.Attribute("oid") != null && element.Attribute("oid").Value == UpdatedStorageInstanceRef.PersistentObjectID.ToString()
                                            select element).FirstOrDefault();

                //string XQuery =StorageName+"/ObjectCollections/"+UpdatedStorageInstanceRef.MemoryInstance.GetType().FullName+
                //	"/Object[@oid = "+UpdatedStorageInstanceRef.PersistentObjectID.ToString()+"]";
                //System.Xml.XmlElement StorageInstance=ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);


                ((MetaDataStorageInstanceRef)UpdatedStorageInstanceRef).TheStorageIstance = storageInstance;
                ObjectStorageSession.NodeChangedUnderTransaction(storageInstance, this.OwnerTransactiont);
            }
			((MetaDataStorageInstanceRef)UpdatedStorageInstanceRef).SaveObjectState();

			/*
			string FileName=((MetaDataStorageInstanceRef)UpdatedStorageInstanceRef).TheStorageIstance.OwnerDocument.BaseURI;
			System.Uri FileUri =new System.Uri(FileName);
			FileName=FileUri.LocalPath;
			((MetaDataStorageInstanceRef)UpdatedStorageInstanceRef).TheStorageIstance.OwnerDocument.Save(FileName);
			*/
		}
	}
}
