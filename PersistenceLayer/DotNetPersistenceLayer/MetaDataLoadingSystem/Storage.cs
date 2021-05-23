using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using OOAdvantech.Transactions;

namespace OOAdvantech.MetaDataLoadingSystem
{

    /// <MetaDataID>{c0539711-d58d-4020-bc91-5f07bc0d514a}</MetaDataID>
    public class Storage : MetaDataRepository.Storage
    {

        Storage()
        {
        }
        internal MetaDataStorageSession ObjectStorage;
        public Storage(string storageIdentity, string storageLocation, string storageName, string storageType,string culture, MetaDataStorageSession objectStorage)
        {
            ObjectStorage = objectStorage;
            _StorageIdentity = storageIdentity;
            _StorageLocation = storageLocation;
            _StorageName = storageName;
            _StorageType = storageType;
            _Culture = culture;
        }

        public override string Culture
        {
            get
            {
                return base.Culture;
            }
            set
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    base.Culture = value;

                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                    transactionContext.EnlistCommand(new Commands.UpdateStorageMetadataCommand(this.ObjectStorage));

                    stateTransition.Consistent = true;
                }

            }
        }

        internal System.Collections.Generic.Dictionary<string, StorageCellReference> StorageCellsReference = new Dictionary<string, StorageCellReference>();
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject> OwnedElements
        {
            get
            {
                //ObjectStorage.StorageCells.Values
                XElement linkedStorages = ObjectStorage.XMLDocument.Root.Element("LinkedStorages");
                if (linkedStorages != null)
                {
                    foreach (XElement linkedStorageElement in linkedStorages.Elements())
                    {
                        ;
                        if (linkedStorageElement.Element("StorageCellsReference") != null)
                        {
                            foreach (XElement storageCellReferenceElement in linkedStorageElement.Element("StorageCellsReference").Elements("StorageCellReference"))
                            {
                                var storageCellReference = new StorageCellReference(storageCellReferenceElement);
                                if (!StorageCellsReference.ContainsKey(storageCellReference.OID))//.o .StorageIntentity + "-" + storageCellReference.SerialNumber.ToString()))
                                {
                                    storageCellReference.SetNamespace(this);
                                    //StorageCellsReference[storageCellReference.StorageIntentity + "-" + storageCellReference.SerialNumber.ToString()] = storageCellReference;
                                    StorageCellsReference[storageCellReference.OID] = storageCellReference;
                                }
                            }
                        }
                    }


                }


                return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>((from storageCellRef in StorageCellsReference.Values.ToList() select storageCellRef as MetaDataRepository.MetaObject).ToList());
                return base.OwnedElements;
            }
        }

        //public override object CreateDataLoader(object dataNode, object searchCondition, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells)
        //{
        //    return null;
        //    //return new ObjectQueryLanguage.DataLoader(dataNode as MetaDataRepository.ObjectQueryLanguage.DataNode,searchCondition as MetaDataRepository.ObjectQueryLanguage.SearchCondition, storageCells);
        //}


        public override void RegisterComponent(string assemblyFullName)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void RegisterComponent(string[] assembliesFullNames)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData)
        {
            //throw new NotImplementedException();
        }
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            //throw new NotImplementedException();
        }

        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData)
        {
            // throw new NotImplementedException();
        }

        public override bool CheckForVersionUpgrate(string assemblyFullName)
        {
            DotNetMetaDataRepository.Assembly mAssembly = null;
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;

            return false;
        }

        new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageReference> _LinkedStorages;

        public override Collections.Generic.Set<MetaDataRepository.StorageReference> LinkedStorages
        {
            get
            {
                if (ObjectStorage.XMLDocument != null)
                {
                    if (_LinkedStorages == null)
                    {
                        _LinkedStorages = new Collections.Generic.Set<MetaDataRepository.StorageReference>();
                        XElement linkedStorages = ObjectStorage.XMLDocument.Root.Element("LinkedStorages");
                        if (linkedStorages != null)
                        {
                            foreach (XElement linkedStorageElement in linkedStorages.Elements())
                            {
                                MetaDataRepository.StorageReference storageReference = new MetaDataRepository.StorageReference();
                                storageReference.StorageType = linkedStorageElement.GetAttribute("StorageType");
                                storageReference.StorageName = linkedStorageElement.GetAttribute("StorageName");
                                storageReference.StorageLocation = linkedStorageElement.GetAttribute("StorageLocation");
                                storageReference.NativeStorageID = linkedStorageElement.GetAttribute("StorageIdentity");
                                _LinkedStorages.Add(storageReference);
                            }
                        }
                    }
                    return _LinkedStorages;
                }

                return base.LinkedStorages;
            }
        }
    }



    /// <MetaDataID>{5ba0755e-e84a-421c-abdd-38883f650f6e}</MetaDataID>
    static class XElementExtension
    {
        public static void SetAttribute(this System.Xml.Linq.XElement element, System.Xml.Linq.XName name, string value)
        {
            element.SetAttributeValue(name, value);
        }
        public static string GetAttribute(this System.Xml.Linq.XElement element, System.Xml.Linq.XName name)
        {
            if (element.Attribute(name) == null)
                return "";
            else
                return element.Attribute(name).Value;
        }
        public static bool HasAttribute(this System.Xml.Linq.XElement element, string attributeName)
        {
            return element.Attribute(attributeName) != null;
        }

        public static void SaveToFile(this System.Xml.Linq.XDocument xDocument, string fileName)
        {
#if DeviceDotNet
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
            using (System.IO.Stream stream = fileSystem.Open(fileName))
            {
                stream.SetLength( 0);
                xDocument.Save(stream);
            }


#else
            xDocument.Save(fileName);
#endif

        }

        //#if DeviceDotNet
        //        public static void Save(this System.Xml.Linq.XDocument xDocument, string fileName)
        //        {

        //            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
        //            OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
        //            using (System.IO.Stream stream = fileSystem.Open(fileName))
        //            {
        //                xDocument.Save(stream);
        //            }
        //        }
        //#endif

    }
}
