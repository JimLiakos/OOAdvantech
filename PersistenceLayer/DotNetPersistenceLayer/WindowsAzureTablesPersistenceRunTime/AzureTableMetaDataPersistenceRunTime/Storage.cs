using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Azure;
using Azure.Data.Tables.Models;

using OOAdvantech.DotNetMetaDataRepository;
using OOAdvantech.Transactions;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{a93d9a6d-8813-4c38-9f1c-2b736d1d8cfb}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{a93d9a6d-8813-4c38-9f1c-2b736d1d8cfb}")]
    [MetaDataRepository.Persistent()]
    public class Storage : MetaDataRepository.Storage
    {
        /// <MetaDataID>{99fa3257-50f1-4183-a276-fca284309cf6}</MetaDataID>
        public string StorageType
        {
            get
            {
                return default(string);
            }

            set
            {
            }
        }

        /// <MetaDataID>{cd739d4f-90a0-4dce-827c-6881cc3b1d73}</MetaDataID>
        private bool newStorage;

        /// <MetaDataID>{cb961e4c-5bbc-4c81-a497-95ebc2d1be45}</MetaDataID>
        //public readonly CloudTable ClassBLOBDataTable;
        /// <MetaDataID>{3345967a-891f-4096-9822-90cf0b78d61f}</MetaDataID>
        //public readonly CloudTable ObjectBLOBDataTable;

        //public readonly CloudTable MetadataIdentitiesTable;

        public readonly Azure.Data.Tables.TableClient ClassBLOBDataTable_a;

        public readonly Azure.Data.Tables.TableClient ObjectBLOBDataTable_a;

        public readonly Azure.Data.Tables.TableClient MetadataIdentitiesTable_a;

        ///// <MetaDataID>{65a1dbc5-fd0c-4e00-99e3-b67a5be6f106}</MetaDataID>
        //CloudStorageAccount Account;

        Azure.Data.Tables.TableServiceClient TablesAccount;
        /// <MetaDataID>{c23223b4-7b23-42af-b02e-56f3d813eb80}</MetaDataID>
        StorageMetadata StorageMetadata;

        /// <MetaDataID>{30b03425-58ad-4ab0-9a0d-7db915cd613e}</MetaDataID>
        public readonly string ClassBLOBDataTableName;
        /// <MetaDataID>{91ed94bd-e646-4803-b8b8-0f55de5c5826}</MetaDataID>
        public readonly string ObjectBLOBDataTableName;

        public readonly string MetadataIdentityTableName;


        protected Storage()
        {

        }

        internal static string GetClassBLOBDataTableNameFor(string storagePrefix)
        {
            return storagePrefix + "ClassBLOBData";
        }

        internal static string GetObjectBLOBDataTableNameFor(string storagePrefix)
        {
            return storagePrefix + "ObjectBLOBData";
        }

        internal static string GetMetadataIdentityTableNameFro(string storagePrefix)
        {
            return storagePrefix + "MetadataIdentitiesTable";
        }

        /// <MetaDataID>{9e7e3f3b-7172-42c9-b8f2-a102c2954693}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, bool newStorage,  Azure.Data.Tables.TableServiceClient tablesAccount, StorageMetadata storageMetadata = null)
        {
            StorageName = storageName;
            StorageLocation = storageLocation;
            StorageType = storageType;
            this.newStorage = newStorage;
            //Account = account;
            TablesAccount = tablesAccount;
            StorageMetadata = storageMetadata;
            //CloudTableClient tableClient = account.CreateCloudTableClient();


        

            if (StorageMetadata == null)
            {
                //CloudTable storageMetadataTable = tableClient.GetTableReference("StoragesMetadata");

                Azure.Data.Tables.TableClient storageMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");

                var storageMetadataEntity = (from storageMetada in storageMetadataTable_a.Query<Azure.Data.Tables.TableEntity>()
                                                 //from storageMetada in storageMetadaPage
                                             where storageMetada.GetString("StorageName") == StorageName
                                             select storageMetada).FirstOrDefault();

                StorageMetadata = (from storageMetada in storageMetadataTable_a.Query<StorageMetadata>()
                                   where storageMetada.StorageName == StorageName
                                   select storageMetada).FirstOrDefault();
            }

            ClassBLOBDataTableName = GetClassBLOBDataTableNameFor(StorageMetadata.StoragePrefix);// StorageMetadata.StoragePrefix + "ClassBLOBData";
            ObjectBLOBDataTableName = GetObjectBLOBDataTableNameFor(StorageMetadata.StoragePrefix);//  StorageMetadata.StoragePrefix + "ObjectBLOBData";

            MetadataIdentityTableName = GetMetadataIdentityTableNameFro(StorageMetadata.StoragePrefix);// StorageMetadata.StoragePrefix + "MetadataIdentitiesTable";




            //ClassBLOBDataTable = tableClient.GetTableReference(ClassBLOBDataTableName);
            //ObjectBLOBDataTable = tableClient.GetTableReference(ObjectBLOBDataTableName);
            //MetadataIdentitiesTable = tableClient.GetTableReference(MetadataIdentityTableName);
            ClassBLOBDataTable_a = TablesAccount.GetTableClient(ClassBLOBDataTableName);
            ObjectBLOBDataTable_a = TablesAccount.GetTableClient(ObjectBLOBDataTableName);
            MetadataIdentitiesTable_a = TablesAccount.GetTableClient(MetadataIdentityTableName);

            Pageable<TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", MetadataIdentityTableName));
            bool metadataIdentitiesTable_exist = queryTableResults.Count() > 0;

            queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", ClassBLOBDataTableName));
            bool classBLOBDataTable_exist = queryTableResults.Count() > 0;

            queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", ObjectBLOBDataTableName));
            bool objectBLOBDataTable_exist = queryTableResults.Count() > 0;

            if (newStorage)
            {

                //if (!ClassBLOBDataTable.Exists())
                //    ClassBLOBDataTable.CreateIfNotExists();

                if (!classBLOBDataTable_exist)
                    ClassBLOBDataTable_a.CreateIfNotExists();

                //if (!ObjectBLOBDataTable.Exists())
                //    ObjectBLOBDataTable.CreateIfNotExists();

                if (objectBLOBDataTable_exist)
                    ObjectBLOBDataTable_a.CreateIfNotExists();


                if (!metadataIdentitiesTable_exist)
                {
                    MetadataIdentitiesTable_a.CreateIfNotExists();
                    MetadataIdentities = new MetadataIdentities("AAA", Guid.NewGuid().ToString());
                    MetadataIdentities.NextOID = 1;
                    MetadataIdentitiesTable_a.AddEntity(MetadataIdentities);
                }

                //if (!MetadataIdentitiesTable.Exists())
                //{
                //    MetadataIdentitiesTable.CreateIfNotExists();


                //    MetadataIdentities = new MetadataIdentities("AAA", Guid.NewGuid().ToString());
                //    MetadataIdentities.NextOID = 1;
                //    MetadataIdentitiesTable.InsertEntity(MetadataIdentities);
                //}
                else
                {
                    foreach (var metadataIdentities in (from metadataIdentities in MetadataIdentitiesTable_a.Query<MetadataIdentities>()
                                                        select metadataIdentities))
                    {
                        MetadataIdentities = metadataIdentities;
                        break;
                    }

                }


            }
            else
            {
                if (!metadataIdentitiesTable_exist)
                {
                    MetadataIdentitiesTable_a.CreateIfNotExists();
                    MetadataIdentities = new MetadataIdentities("AAA", Guid.NewGuid().ToString());
                    MetadataIdentities.NextOID = 1;
                    MetadataIdentitiesTable_a.AddEntity(MetadataIdentities);
                }
                //if (!MetadataIdentitiesTable.Exists())
                //{
                //    MetadataIdentitiesTable.CreateIfNotExists();


                //    MetadataIdentities = new MetadataIdentities("AAA", Guid.NewGuid().ToString());
                //    MetadataIdentities.NextOID = 1;
                //    MetadataIdentitiesTable.InsertEntity(MetadataIdentities);
                //}

                LoadClassBlobs();
            }


        }
        public MetadataIdentities MetadataIdentities;
        /// <MetaDataID>{85362768-1a9b-4b7d-a168-3c635efb36c0}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{75aac540-7624-4a9d-9860-822a2c1b8e32}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName)
        {
            DotNetMetaDataRepository.Assembly mAssembly = null;
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            //mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            //if (mAssembly == null)
            //    mAssembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);

            System.Collections.Generic.List<MetaDataError> errors = new System.Collections.Generic.List<MetaDataError>();
#if !DeviceDotNet
            bool hasErrors = mAssembly.ErrorCheck(ref errors);
            if (hasErrors)
            {
                string ErrorMessage = null;
                foreach (MetaDataRepository.MetaObject.MetaDataError error in errors)
                {
                    if (ErrorMessage != null)
                        ErrorMessage += "\n";
                    ErrorMessage += error.ErrorMessage;
                }
                throw new System.Exception(ErrorMessage);
            }
#endif
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                RegisterComponent(mAssembly);
                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{2cc93afb-06dd-4499-962f-4aa90806a4ef}</MetaDataID>
        private void GetReferenceToComponents(MetaDataRepository.Component Component, System.Collections.Generic.List<MetaDataRepository.Component> components)
        {

            foreach (MetaDataRepository.Dependency dependency in Component.ClientDependencies)
            {
                MetaDataRepository.Component refComponent = dependency.Supplier as MetaDataRepository.Component;


                if (!components.Contains(refComponent))
                {
                    object[] objects = (refComponent as DotNetMetaDataRepository.Assembly).WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                    if (objects.Length == 0)
                        continue;
                    components.Add(refComponent);
                    GetReferenceToComponents(refComponent, components);
                }
            }
        }

        /// <MetaDataID>{afe64c88-69e4-4666-a20f-33a317daaa1b}</MetaDataID>
        public void RegisterComponent(MetaDataRepository.Component Component)
        {


            System.Collections.Generic.List<MetaDataRepository.Component> components = new System.Collections.Generic.List<MetaDataRepository.Component>();
            GetReferenceToComponents(Component, components);
            components.Add(Component);
            System.Collections.Generic.Dictionary<string, ClassBLOB> insertedClassBlobs = new System.Collections.Generic.Dictionary<string, ClassBLOB>();
            //System.Collections.Generic.List<TableBatchOperation> tableBatchOperations = new List<TableBatchOperation>();
            List<List<Azure.Data.Tables.TableTransactionAction>> tableBatchOperations_a = new List<List<Azure.Data.Tables.TableTransactionAction>>();
            foreach (MetaDataRepository.Component _Component in components)
            {
                if (_Component.Identity.ToString() == typeof(DotNetMetaDataRepository.Assembly).GetMetaData().Assembly.FullName)
                    continue;

                foreach (MetaDataRepository.MetaObject metaObject in _Component.Residents)
                {

                    if (metaObject.Namespace != null && metaObject.FullName != "OOAdvantech.MetaDataRepository.Namespace" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.MultiplicityRange" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Enumeration" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Realization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AssociationEndRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AttributeRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Parameter" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.ObjectIdentityType" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.IdentityPart" &&
                        metaObject.FullName != typeof(OOAdvantech.MetaDataRepository.StorageReference).FullName &&
                        (metaObject.Namespace.FullName == "OOAdvantech.RDBMSDataObjects" ||
                        metaObject.Namespace.FullName == "OOAdvantech.MetaDataRepository"))
                        continue;
                    DotNetMetaDataRepository.Class _class = metaObject as DotNetMetaDataRepository.Class;
                    try
                    {
                        if (_class != null && _class.Persistent)
                            RegisterClass(insertedClassBlobs, _class, tableBatchOperations_a);
                    }
                    catch (System.Exception Error)
                    {
                        if (_class != null)
                            throw new System.Exception("Error on '" + _class.FullName + "' registration.", Error);
                        else
                            throw new System.Exception(Error.Message, Error);

                    }
                }
            }
            //foreach (var tableBatchOperation in tableBatchOperations)
            //    ClassBLOBDataTable.ExecuteBatch(tableBatchOperation);


            foreach (var tableBatchOperation in tableBatchOperations_a)
                ClassBLOBDataTable_a.SubmitTransaction(tableBatchOperation);




            foreach (var classBLOBData in (from classBLOBData in ClassBLOBDataTable_a.Query<ClassBLOBData>()
                                           select classBLOBData))
            {

                Guid ID = Guid.Parse(classBLOBData.RowKey);
                string metaObjectIdentity = classBLOBData.MetaObjectIdentity;
                byte[] ClassData = classBLOBData.ClassData;
                int offset = 4;
                ClassBLOB classBLOB = new ClassBLOB(ClassData, offset);
                ClassBLOBs.Add(ID, classBLOB);
                classBLOB.ID = ID;
            }


            //var command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand( 
            ////"SELECT ID,MetaObjectIdentity FROM ClassBLOBS ",Connection);
            //command.CommandText = LoadClassBlobObjectIdentitiesSQLStatement;// "SELECT ID,MetaObjectIdentity FROM ClassBLOBS ";
            //var dataReader = command.ExecuteReader();
            //while (dataReader.Read())
            //{
            //    string MetaObjectIdentity = dataReader["MetaObjectIdentity"] as string;
            //    if (insertedClassBlobs.ContainsKey(MetaObjectIdentity))
            //    {
            //        DataObjects.ClassBLOB classBLOB = insertedClassBlobs[MetaObjectIdentity];
            //        classBLOB.ID = System.Convert.ToInt32(dataReader["ID"]);
            //        ClassBLOBs.Add(classBLOB.ID, classBLOB);

            //    }
            //}
            //dataReader.Close();

        }

        //protected ClassBLOB GetClassBLOBIfExist(DotNetMetaDataRepository.Class _class)
        //{
        //    if (_class == null)
        //        return null;
        //    foreach (var entry in ClassBLOBs)
        //    {
        //        ClassBLOB classBLOB = entry.Value as ClassBLOB;
        //        if (classBLOB.Class == _class)
        //            return classBLOB;
        //    }
        //    return null;
        //}


        //public Collections.Generic.Dictionary<string, ClassBLOB> ClassBLOBs = new Collections.Generic.Dictionary<string, ClassBLOB>();
        /// <MetaDataID>{d1e28d46-818c-4260-a30e-888cd7d8e85e}</MetaDataID>
         protected void RegisterClass(System.Collections.Generic.Dictionary<string, OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ClassBLOB> insertedClassBlobs, OOAdvantech.DotNetMetaDataRepository.Class _class, List<List<Azure.Data.Tables.TableTransactionAction>> tableBatchOperations)
        {
            //TableBatchOperation_a.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.UpsertReplace, objectBLOBData));

            ClassBLOB classBLOB = GetClassBLOBIfExist(_class);



            if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
                tableBatchOperations.Add(new List<Azure.Data.Tables.TableTransactionAction>());
            List<Azure.Data.Tables.TableTransactionAction> tableBatchOperation = tableBatchOperations.Last();

            byte[] byteStream = new byte[65536];
            int offset = 4;

            if (classBLOB == null)
            {
                ClassBLOBData classBLOBData = new ClassBLOBData("AAA", Guid.NewGuid().ToString());
                classBLOB = new ClassBLOB(_class, classBLOBData);
                classBLOB.Serialize(byteStream, offset, out offset);
                int nextpos = 0;
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset - 4, byteStream, 0, ref nextpos, true);
                byte[] outByteStream = new byte[offset];

                for (int i = 0; i != offset; i++)
                    outByteStream[i] = byteStream[i];

                classBLOB.ClassBLOBData.ClassData = outByteStream;
                classBLOB.ClassBLOBData.MetaObjectIdentity = _class.Identity.ToString();

                insertedClassBlobs.Add(_class.Identity.ToString(), classBLOB);

                tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.Add, classBLOB.ClassBLOBData));

            }
            else
            {
                if (classBLOB.HasChange)
                {
                    classBLOB.Serialize(byteStream, offset, out offset);
                    int nextpos = 0;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset - 4, byteStream, 0, ref nextpos, true);
                    byte[] outByteStream = new byte[offset];
                    for (int i = 0; i != offset; i++)
                        outByteStream[i] = byteStream[i];

                    classBLOB.ClassBLOBData.ClassData = outByteStream;
                    classBLOB.ClassBLOBData.MetaObjectIdentity = _class.Identity.ToString();

                    insertedClassBlobs.Add(_class.Identity.ToString(), classBLOB);

                    tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.UpdateReplace, classBLOB.ClassBLOBData));

                }
            }


        }



        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, XDocument> assembliesMappingData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{a71c036b-39eb-4baf-97e3-28453cae9154}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{1d03306c-3d6c-443c-8a17-443550fa5af2}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, XDocument mappingData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ffe4120f-4398-4819-a2ee-dfeee17e3860}</MetaDataID>
        internal ClassBLOB GetClassBLOB(Class _class)
        {

            ClassBLOB classBLOB = GetClassBLOBIfExist(_class);
            if (classBLOB != null)
                return classBLOB;
            else
                throw new System.Exception("There isn't metada for class \"" + _class.FullName + "\". Register the assembly of class and try again.");

        }

        /// <MetaDataID>{c412e943-a63e-4b22-a9f5-49401b880b1a}</MetaDataID>
        public ClassBLOB GetClassBLOB(Guid classBLOBID)
        {
            return ClassBLOBs[classBLOBID];
        }


        /// <MetaDataID>{63988f37-da7e-48a0-8919-42a21e8f3add}</MetaDataID>
        void LoadClassBlobs()
        {

            try
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {

                    foreach (var classBLOBData in ClassBLOBDataTable_a.Query<ClassBLOBData>())
                    {
                        Guid ID = Guid.Parse(classBLOBData.RowKey);
                        string metaObjectIdentity = classBLOBData.MetaObjectIdentity;
                        byte[] ClassData = classBLOBData.ClassData;
                        int offset = 4;
                        ClassBLOB classBLOB = new ClassBLOB(ClassData, offset);
                        ClassBLOBs.Add(ID, classBLOB);
                        classBLOB.ID = ID;
                    }
                    //foreach (var classBLOBData in (from classBLOBData in ClassBLOBDataTable.CreateQuery<ClassBLOBData>()
                    //                               select classBLOBData))
                    //{

                    //    Guid ID = Guid.Parse(classBLOBData.RowKey);
                    //    string metaObjectIdentity = classBLOBData.MetaObjectIdentity;
                    //    byte[] ClassData = classBLOBData.ClassData;
                    //    int offset = 4;
                    //    ClassBLOB classBLOB = new ClassBLOB(ClassData, offset);
                    //    ClassBLOBs.Add(ID, classBLOB);
                    //    classBLOB.ID = ID;
                    //}

                    foreach (ClassBLOB classBlob in ClassBLOBs.Values)
                    {
                        var Generalizations = classBlob.Class.Generalizations;
                        var features = classBlob.Class.Features;
                        var roles = classBlob.Class.Roles;
                    }
                    stateTransition.Consistent = true;
                }


            }
            catch (System.Exception Error)
            {
                throw;
            }
            foreach (var metadataIdentities in (from metadataIdentities in MetadataIdentitiesTable_a.Query<MetadataIdentities>()
                                                select metadataIdentities))
            {
                MetadataIdentities = metadataIdentities;
                break;
            }

            //double tim3 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;

        }

        /// <MetaDataID>{99824da2-b713-4583-bb14-945c586fac26}</MetaDataID>
        static Storage()
        {
            BackwardCompatibilities.Add("OOAdvantech, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643",
                                        "OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643");

            BackwardCompatibilities.Add("PersistenceLayerRunTime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b",
                                        "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b");

            BackwardCompatibilities.Add("RDBMSMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=ab7ad9c2d64354ad",
                                   "RDBMSMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=483eb08c93287fcd");


        }

        public Storage(string storageName, string storageLocation, string storageType, bool newStorage, Azure.Data.Tables.TableServiceClient tablesAccount, Azure.Data.Tables.TableClient storageMetadataEntry) : this(storageName, storageLocation, storageType, newStorage,  tablesAccount)
        {
            this.storageMetadataEntry = storageMetadataEntry;
        }

        /// <MetaDataID>{6e1d93bc-2481-4ad0-9964-62b6f4a28b7b}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<System.Guid, OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ClassBLOB> ClassBLOBs = new Collections.Generic.Dictionary<Guid, ClassBLOB>();

        /// <MetaDataID>{9200c161-65c8-4ff0-bd3c-023e07e7268c}</MetaDataID>
        static internal System.Collections.Generic.Dictionary<string, string> BackwardCompatibilities = new System.Collections.Generic.Dictionary<string, string>();
        private Azure.Data.Tables.TableClient storageMetadataEntry;

        /// <MetaDataID>{ef2d8242-25a2-4c94-9718-54c021548fcf}</MetaDataID>
        protected ClassBLOB GetClassBLOBIfExist(DotNetMetaDataRepository.Class _class)
        {
            if (_class == null)
                return null;
            foreach (var entry in ClassBLOBs)
            {
                ClassBLOB classBLOB = entry.Value;
                if (classBLOB.Class == _class)
                    return classBLOB;
            }
            return null;
        }

        public override bool CheckForVersionUpgrate(string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
