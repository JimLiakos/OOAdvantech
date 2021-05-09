using System;

namespace OOAdvantech.XMLPersistenceRunTime
{
    /// <metadataid>{1A5D918C-24F6-4E46-9707-AA6FE61E6132}</metadataid>
    /// <summary>XMLStorageProvider is a communication channel with the XML Runtime. </summary>
    public class StorageProvider : OOAdvantech.PersistenceLayerRunTime.StorageProvider
	{

        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        /// <MetaDataID>{844BEAC1-3032-4481-A473-E3FEA95474CF}</MetaDataID>
        public override Guid ProviderID
        {
            get
            {
                
                return new System.Guid("{3DE012E8-54DD-416B-BB5E-142047C9C66B}");
            }
            set
            {
            }
        }
        /// <MetaDataID>{3AA67C9E-BDE5-47C7-A83C-DBB1E6154A80}</MetaDataID>
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        /// <MetaDataID>{52A22322-9AB7-4C98-9C80-99B274B288AD}</MetaDataID>
        public override bool AllowEmbeddedStorage()
        {
            return true;
        }

        /// <MetaDataID>{B377A104-EC14-49FC-88CF-ED967F8D79AC}</MetaDataID>
        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
            return System.Net.Dns.GetHostName();
        }




        /// <summary>Open a session to access the storage that defined from the parameters. </summary>
        /// <MetaDataID>{0ba3566e-4842-42b4-8092-0d1e157948d4}</MetaDataID>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository. </param>
        /// <param name="StorageName">The name of Object Storage </param>
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation)
        {
            System.Xml.XmlDocument mXmlDocument = new System.Xml.XmlDocument();
            storageLocation = storageLocation.ToLower();
            ObjectStorage mStorageSession = null;
            if (storageLocation == "unitializedrawdata")
            {
                mStorageSession = new ObjectStorage();
                mStorageSession.XMLDocument = mXmlDocument;
                return mStorageSession;
            }
            try
            {
                mXmlDocument.Load(storageLocation);
            }
            catch (System.IO.FileNotFoundException Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("Could not open Storage " + storageName, Error);
            }
            mStorageSession = new ObjectStorage();
            mStorageSession.XMLDocument = mXmlDocument;
            return mStorageSession;
        }

        /// <summary>Create a new Object Storage with schema like original storage and open a storage session with it. </summary>
        /// <MetaDataID>{11a68762-115f-4677-9382-833662ba9c3e}</MetaDataID>
        /// <param name="StorageLocation">The location of new Object storage </param>
        /// <param name="StorageName">The name of new Object Storage </param>
        /// <param name="OriginalStorage">Cloned Metada (scema) </param>
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string storageName, string storageLocation)
        {
            System.Xml.XmlDocument MainDocument = new System.Xml.XmlDocument();
            System.Xml.XmlNode RootElement = MainDocument.AppendChild(MainDocument.CreateElement(storageName));
            System.Xml.XmlNode NextObjID = MainDocument.CreateElement("NextObjID", "");
            NextObjID = RootElement.AppendChild(NextObjID);
            System.Xml.XmlAttribute mXmlAttribute = MainDocument.CreateAttribute("ObjID");
            ((System.Xml.XmlElement)NextObjID).SetAttribute("ObjID", "", System.Convert.ToString(1));
            System.Xml.XmlNode ObjectCollections = MainDocument.CreateElement("ObjectCollections", "");
            ObjectCollections = RootElement.AppendChild(ObjectCollections);
            MainDocument.Save(storageLocation);
            //MainDocument=new System.Xml.XmlDocument();
            MainDocument.Load(storageLocation);

            ObjectStorage mStorageSession = new ObjectStorage();
            mStorageSession.XMLDocument = MainDocument;
            return mStorageSession;
        }

        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "default";
        }
    }
}
