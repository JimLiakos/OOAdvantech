using System;
using System.Xml.Linq;
using OOAdvantech.PersistenceLayer;
#if DeviceDotNet
using Xamarin.Forms.Internals;
#endif

namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{3DE012E8-54DD-416B-BB5E-142047C9C66B}</MetaDataID>
    public class MetaDataStorageProvider : PersistenceLayerRunTime.StorageProvider
    {

        public MetaDataStorageProvider()
        {
#if DeviceDotNet
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(MetaDataStorageProvider));

            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.PersistenceLayerRunTime.StorageProvider));
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.DotNetMetaDataRepository.Assembly));
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.MetaDataRepository.Storage));
#endif

        }

        public override string GetNativeStorageID(string storageDataLocation)
        {

            throw new System.NotImplementedException();
        }

        public override OOAdvantech.PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new System.NotImplementedException();
        }
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        /// <MetaDataID>{643EB5DE-D598-4E5D-ABB6-93CF1B697673}</MetaDataID>
        private System.Guid ProviderIDData;

        public static void Init()
        {

        }

        /// <summary>The Provider identity. Globally unique.</summary>
        /// <MetaDataID>{CA9D0AE3-E822-4805-B3E0-F31267E42335}</MetaDataID>
        public override System.Guid ProviderID
        {
            get
            {
                return new System.Guid("{3DE012E8-54DD-416B-BB5E-142047C9C66B}");
            }
            set
            {
            }
        }
        /// <summary>Create a storage access session.</summary>
        /// <param name="StorageName">The name of Object Storage</param>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository.
        /// </param>
        /// <MetaDataID>{0B62EECE-303D-49FC-9FAC-D81DFB1D8082}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            XDocument mXmlDocument = new XDocument();
            //StorageLocation = StorageLocation.ToLower();
            MetaDataStorageSession mStorageSession = null;
            if (StorageLocation.ToLower() == "unitializedrawdata")
            {
                mStorageSession = new MetaDataStorageSession(StorageName, StorageLocation, mXmlDocument);
                mStorageSession.LoadMetadata();
                //mStorageSession.XMLDocument = mXmlDocument;
                return mStorageSession;
            }
            try
            {
#if DeviceDotNet
                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
                if (fileSystem.FileExists(StorageLocation))
                {
                    using (System.IO.Stream stream = fileSystem.Open(StorageLocation))
                    {

#if DEBUGA
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            string firstLine = "";// reader.ReadLine();

                            string xml = reader.ReadToEnd();
                            try
                            {
                                if(firstLine.IndexOf("<?xml ")!=0)
                                    mXmlDocument = XDocument.Parse(firstLine+xml);
                                else
                                    mXmlDocument = XDocument.Parse(xml);

                            }
                            catch (Exception error)
                            {
                                throw;
                            }
                            // Use reader.
                        }
#else
                        mXmlDocument = XDocument.Load(stream);
#endif


                    }
                }
                else
                {
                    throw new System.IO.FileNotFoundException();// OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);
                }
#else
                mXmlDocument = XDocument.Load(StorageLocation);

#endif
            }
            catch (System.IO.FileNotFoundException Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("Could not open Storage " + StorageName, Error);
            }
            mStorageSession = new MetaDataStorageSession(StorageName, StorageLocation, mXmlDocument);
            mStorageSession.LoadMetadata();
            //mStorageSession.XMLDocument = mXmlDocument;
            return mStorageSession;
        }
        /// <summary>Create a new Object Storage with schema like original storage and open a storage session with it.</summary>
        /// <param name="OriginalStorage">Cloned Metada (scema)</param>
        /// <param name="StorageName">The name of new Object Storage</param>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository.
        /// </param>
        /// <MetaDataID>{32780274-4BD0-45EB-B9FA-11055783F6F9}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            XDocument MainDocument = new XDocument();
            XElement storageElement = new XElement(StorageName);
            MainDocument.Add(storageElement);
            XElement nextObjID = new XElement("NextObjID", "");
            MainDocument.Root.SetAttribute("Version", "1020");
            storageElement.Add(nextObjID);
            //System.Xml.XmlAttribute mXmlAttribute=MainDocument.CreateAttribute("ObjID");
            nextObjID.SetAttribute("ObjID", System.Convert.ToString(1));
            XElement objectCollections = new XElement("ObjectCollections");
            MainDocument.Root.SetAttribute("StorageIdentity", System.Guid.NewGuid().ToString());
            storageElement.Add(objectCollections);
            MainDocument.SaveToFile(StorageLocation);



            MetaDataStorageSession mStorageSession = new MetaDataStorageSession(StorageName, StorageLocation, MainDocument);
            mStorageSession.LoadMetadata();
            //mStorageSession.XMLDocument = MainDocument;
            return mStorageSession;
        }
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        public override bool AllowEmbeddedStorage()
        {
            return true;
        }

        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
#if DeviceDotNet
            return "default";
#else
            return System.Net.Dns.GetHostName();
#endif
        }

        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "default";
        }


        public override PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            XDocument MainDocument = null;
            if (rawStorageData is PersistenceLayer.IRawStorageData)
                MainDocument = (rawStorageData as PersistenceLayer.IRawStorageData).RawData as XDocument;
            else
                MainDocument = rawStorageData as XDocument;

            XElement storageElement = new XElement(StorageName);
            MainDocument.Add(storageElement);
            XElement nextObjID = new XElement("NextObjID", "");
            MainDocument.Root.SetAttribute("Version", "1020");
            storageElement.Add(nextObjID);
            //System.Xml.XmlAttribute mXmlAttribute=MainDocument.CreateAttribute("ObjID");
            nextObjID.SetAttribute("ObjID", System.Convert.ToString(1));
            XElement objectCollections = new XElement("ObjectCollections");
            MainDocument.Root.SetAttribute("StorageIdentity", System.Guid.NewGuid().ToString());
            storageElement.Add(objectCollections);





            MetaDataStorageSession mStorageSession = new MetaDataStorageSession(StorageName, null, MainDocument, rawStorageData as PersistenceLayer.IRawStorageData);
            mStorageSession.LoadMetadata();
            //mStorageSession.XMLDocument = MainDocument;
            mStorageSession.IsRawStorageData = true;

            if (mStorageSession.RawStorageData != null)
            {
                mStorageSession.RawStorageData.SaveRawData();
                mStorageSession.StorageMetaData.StorageLocation = mStorageSession.RawStorageData.StorageLocation;
            }
            return mStorageSession;

            //XElement RootElement = MainDocument.AppendChild(MainDocument.CreateElement(StorageName));
            //XElement NextObjID = MainDocument.CreateElement("NextObjID", "");
            //MainDocument.Root.SetAttribute("Version", "1020");
            //NextObjID = RootElement.AppendChild(NextObjID);
            //System.Xml.XmlAttribute mXmlAttribute = MainDocument.CreateAttribute("ObjID");
            //((System.Xml.XmlElement)NextObjID).SetAttribute("ObjID", "", System.Convert.ToString(1));
            //XElement ObjectCollections = MainDocument.CreateElement("ObjectCollections", "");
            //MainDocument.Root.SetAttribute("StorageIdentity", System.Guid.NewGuid().ToString());
            //ObjectCollections = RootElement.AppendChild(ObjectCollections);


            //MetaDataStorageSession mStorageSession = new MetaDataStorageSession(StorageName, null, MainDocument.Root.GetAttribute("StorageIdentity"));
            //mStorageSession.XMLDocument = MainDocument;
            //mStorageSession.RawStorageData = true;
            //return mStorageSession;

        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            XDocument mXmlDocument = null;
            if (rawStorageData is PersistenceLayer.IRawStorageData)
                mXmlDocument = (rawStorageData as PersistenceLayer.IRawStorageData).RawData as XDocument;
            else
                mXmlDocument = rawStorageData as XDocument;

            MetaDataStorageSession mStorageSession = null;

            if (string.IsNullOrEmpty(mXmlDocument.Root.GetAttribute("StorageIdentity")))
                mXmlDocument.Root.SetAttribute("StorageIdentity", System.Guid.NewGuid().ToString());
            string storageLocation = null;
            if (rawStorageData is PersistenceLayer.IRawStorageData)
                storageLocation = (rawStorageData as PersistenceLayer.IRawStorageData).StorageLocation;


            mStorageSession = new MetaDataStorageSession(StorageName, storageLocation, mXmlDocument, rawStorageData as PersistenceLayer.IRawStorageData);
            mStorageSession.IsRawStorageData = true;
            try
            {
                mStorageSession.LoadMetadata();
                //mStorageSession.XMLDocument = mXmlDocument;
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("System can't open storage " + StorageName, OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);
            }
            return mStorageSession;
        }

        public override OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new System.NotImplementedException();
        }

        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
