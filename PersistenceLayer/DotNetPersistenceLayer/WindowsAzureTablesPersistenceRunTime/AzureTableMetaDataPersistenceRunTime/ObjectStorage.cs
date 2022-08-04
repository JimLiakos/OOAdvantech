using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
using OOAdvantech.Collections;

using OOAdvantech.DotNetMetaDataRepository;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.PersistenceLayerRunTime;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{9b821af1-931e-4ef6-bb98-b4b12f7d606a}</MetaDataID>
    public class ObjectStorage : PersistenceLayerRunTime.ObjectStorage
    {
        /// <MetaDataID>{6d440757-0146-4f76-80b6-861c056eeb7f}</MetaDataID>
        public override PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return _StorageMetaData;
            }
        }

        public int GetNextOID()
        {
            lock (this)
            {
                int nextOID = (this.StorageMetaData as Storage).MetadataIdentities.NextOID;
                (this.StorageMetaData as Storage).MetadataIdentities.NextOID = nextOID + 1;
                return nextOID;
            }
        }

        /// <MetaDataID>{9e9fe403-9cf3-4455-a6a2-89d782c3c3b9}</MetaDataID>
        internal CloudStorageAccount Account;
        internal Azure.Data.Tables.TableServiceClient TablesAccount;

        /// <MetaDataID>{51f19b4e-9c7d-4fd6-bfa5-b2f8a9756f50}</MetaDataID>
        internal protected Collections.Generic.Dictionary<PersistenceLayer.ObjectID, StorageInstanceRef> StorageObjects = new Collections.Generic.Dictionary<PersistenceLayer.ObjectID, StorageInstanceRef>();


        /// <MetaDataID>{6daf1980-29ee-4c4e-ba40-68df3049126d}</MetaDataID>
        Storage _StorageMetaData;
        /// <MetaDataID>{e4799edd-7fb5-4153-9b78-34cde92800dc}</MetaDataID>
        public ObjectStorage(string storageName, string storageLocation, bool newStorage, CloudStorageAccount account, global::Azure.Data.Tables.TableServiceClient tablesAccount)
        {
            Account = account;
            TablesAccount = tablesAccount;
            _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider", newStorage, account, tablesAccount);
            System.DateTime start = System.DateTime.Now;
            if (!newStorage)
                LoadStorageObjects();



        }

        public ObjectStorage(string storageName, string storageLocation, bool newStorage, CloudStorageAccount account, global::Azure.Data.Tables.TableServiceClient tablesAccount, OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageMetadata storageMetadataEntry)
        {
            Account = account;
            TablesAccount = tablesAccount;
            _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider", newStorage, account, tablesAccount, storageMetadataEntry);
            System.DateTime start = System.DateTime.Now;
            if (!newStorage)
                LoadStorageObjects();



        }





        /// <MetaDataID>{df561233-c7f1-40db-8ea1-639beb0caee5}</MetaDataID>
        protected void LoadStorageObjects()
        {

            var objectBLOBDataTable = (StorageMetaData as Storage).ObjectBLOBDataTable;
            var objectBLOBDataTable_a = (StorageMetaData as Storage).ObjectBLOBDataTable_a;
            int lastOID = 0;
            int count = 0;
            //foreach (var objectBLOBData in (from objectBLOBData in objectBLOBDataTable.CreateQuery<ObjectBLOBData>()
            //                                select objectBLOBData))
            foreach (var objectBLOBData in objectBLOBDataTable_a.Query<ObjectBLOBData>())
            {

                Guid OID = Guid.Parse(objectBLOBData.RowKey);
                Guid classBLOBSID = Guid.Parse(objectBLOBData.ClassBLOBSID);
                byte[] byteStream = objectBLOBData.ObjectData;
                System.Type memoryInstanceType = (_StorageMetaData as Storage).GetClassBLOB(classBLOBSID).Class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                object newObject = AccessorBuilder.CreateInstance(memoryInstanceType);
                StorageInstanceRef storageInstanceRef = CreateStorageInstanceRef(newObject, new OOAdvantech.WindowsAzureTablesPersistenceRunTime.ObjectID(OID)) as StorageInstanceRef;
                storageInstanceRef.ObjectBLOBData = objectBLOBData;
                int offset = 0;
                storageInstanceRef.LoadObjectState(byteStream, offset, out offset);

                storageInstanceRef.ObjectActived();

                if (newObject is RDBMSMetaDataRepository.Component)
                {
#if!DeviceDotNet
                    if (!string.IsNullOrEmpty((newObject as RDBMSMetaDataRepository.Component).AssemblyString))
                        System.Reflection.Assembly.Load((newObject as RDBMSMetaDataRepository.Component).AssemblyString);
#else
                    if (!string.IsNullOrEmpty((NewObject as RDBMSMetaDataRepository.Component).AssemblyString))
                        ModulePublisher.ClassRepository.LoadedAssemblies.Add(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName((NewObject as RDBMSMetaDataRepository.Component).AssemblyString)));
#endif
                }
                count++;
            }

            //double tim3 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;

            foreach (var entry in StorageObjects)
            {
                StorageInstanceRef storageInstanceRef = entry.Value as StorageInstanceRef;
                storageInstanceRef.ResolveRelationships();
            }
        }

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>> TableBatchOperations = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>>();

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Azure.Data.Tables.TableClient, System.Collections.Generic.List<System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction>>>> TableBatchOperations_a = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Azure.Data.Tables.TableClient, System.Collections.Generic.List<System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction>>>>();



        internal System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction> GetTableBatchOperation_a(Azure.Data.Tables.TableClient azureTable)
        {
            string localTransactionUri = "null_transaction";

            localTransactionUri = OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri.ToLower();

            Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>> transactionTableBatchOperations = null;
            

            if (!TableBatchOperations_a.TryGetValue(localTransactionUri, out transactionTableBatchOperations))
            {
                transactionTableBatchOperations = new Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>>();
                TableBatchOperations_a[localTransactionUri] = transactionTableBatchOperations;
            }

            List<List<Azure.Data.Tables.TableTransactionAction>> tableBatchOperations = null;

            if (!transactionTableBatchOperations.TryGetValue(azureTable, out tableBatchOperations))
            {
                tableBatchOperations = new List<List<Azure.Data.Tables.TableTransactionAction>> ();
                transactionTableBatchOperations[azureTable] = tableBatchOperations;
            }


            if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
                tableBatchOperations.Add(new List<Azure.Data.Tables.TableTransactionAction>());
            return tableBatchOperations.Last();
        }

        internal TableBatchOperation GetTableBatchOperation(CloudTable azureTable)
        {
            string localTransactionUri = "null_transaction";

            localTransactionUri = OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri.ToLower();

            System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>> transactionTableBatchOperations = null;

            if (!TableBatchOperations.TryGetValue(localTransactionUri, out transactionTableBatchOperations))
            {
                transactionTableBatchOperations = new System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>();
                TableBatchOperations[localTransactionUri] = transactionTableBatchOperations;
            }

            System.Collections.Generic.List<TableBatchOperation> tableBatchOperations = null;

            if (!transactionTableBatchOperations.TryGetValue(azureTable, out tableBatchOperations))
            {
                tableBatchOperations = new System.Collections.Generic.List<TableBatchOperation>();
                transactionTableBatchOperations[azureTable] = tableBatchOperations;
            }


            if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
                tableBatchOperations.Add(new TableBatchOperation());
            return tableBatchOperations.Last();

            //return tableBatchOperation;

        }







        /// <MetaDataID>{42d05b9f-e9b2-4525-8f89-e86cfc33a92b}</MetaDataID>
        public override void AbortChanges(TransactionContext theTransaction)
        {
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{508a84f5-c499-4e87-8e46-e0c616c763ed}</MetaDataID>
        public override void BeginChanges(TransactionContext theTransaction)
        {
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{d42ab603-1e76-4214-8c1c-568d87962719}</MetaDataID>
        public override void CommitChanges(TransactionContext theTransaction)
        {
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{5562bb79-ce50-4880-8312-ab4888faee4d}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(IndexedCollection collection)
        {

        }

        /// <MetaDataID>{866a7294-9b18-437e-95ac-949c6ca48d6b}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            return new DataLoader(dataNode, dataLoaderMetadata);

        }

        /// <MetaDataID>{70b10743-6daa-4116-90a7-d3f8b6d6dc01}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, DeleteOptions deleteOption)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstance))
            {
                DeleteStorageInstanceCommand Command = new DeleteStorageInstanceCommand(storageInstance, deleteOption);
                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                if (!transactionContext.ContainCommand(Command.Identity))
                    transactionContext.EnlistCommand(Command);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{c8133e3f-a1d9-41f6-81d6-ea77db5fd540}</MetaDataID>
        public override void CreateLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(LinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association), out command);

            PersistenceLayerRunTime.Commands.LinkObjectsCommand linkObjectsCommand = null;
            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.LinkObjectsCommand;
            if (linkObjectsCommand == null)
            {
                linkObjectsCommand = new LinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
            }
            else
            {
                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }
        }

        /// <MetaDataID>{7569a474-2719-4ccd-a479-6318b2d979aa}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{6319d67a-0965-483a-a816-1b95f457332f}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            NewStorageInstanceCommand newStorageInstanceCommand = new NewStorageInstanceCommand(storageInstance as StorageInstanceRef);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(newStorageInstanceCommand);

        }




        /// <MetaDataID>{d720e308-773d-46c0-b3bd-2dbcd2448478}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent deletedStorageInstanceRef, DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(deletedStorageInstanceRef.RealStorageInstanceRef))
            {
                UnlinkAllObjectCommand mUnlinkAllObjectCommand = new UnlinkAllObjectCommand(deletedStorageInstanceRef);
                //mUnlinkAllObjectCommand.DeletedStorageInstance = deletedStorageInstanceRef;
                mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                transactionContext.EnlistCommand(mUnlinkAllObjectCommand);

                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{a285268c-d551-43c2-bb9b-ac2e90eaf5f3}</MetaDataID>
        public override void CreateUnLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            string cmdIdentity = PersistenceLayerRunTime.Commands.UnLinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
            {
                PersistenceLayerRunTime.Commands.UnLinkObjectsCommand mLinkObjectsCommand = new UnLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mLinkObjectsCommand);
            }

        }

        /// <MetaDataID>{40bfb28f-47d8-4510-ad89-d71cfb1516f6}</MetaDataID>
        public override void CreateUpdateLinkIndexCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {

        }

        /// <MetaDataID>{72740f37-03ef-48f8-966b-f7f2f526b428}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

        }

        /// <MetaDataID>{ddf6ba58-4cdc-46e3-ba45-596608963778}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            UpdateStorageInstanceCommand updateStorageInstanceCommand = new UpdateStorageInstanceCommand(storageInstanceRef);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);

        }

        /// <MetaDataID>{06fa8799-689c-4bb6-baf5-9c10f8a162db}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, null);
#if !DeviceDotNet
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
#else
            return mStructureSet;
#endif
        }

        public override Collections.StructureSet Execute(string OQLStatement, Collections.Generic.Dictionary<string, object> parameters)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, parameters);
#if !DeviceDotNet
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
#else
            return mStructureSet;
#endif
        }


        /// <MetaDataID>{cec201f9-5077-410c-8423-414fdef41926}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            Guid objectID = Guid.Parse(persistentObjectUri);
            StorageInstanceRef storageInstanceRef = StorageObjects[new OOAdvantech.WindowsAzureTablesPersistenceRunTime.ObjectID(objectID)] as StorageInstanceRef;
            if (storageInstanceRef == null)
                return null;
            else
                return storageInstanceRef.MemoryInstance;
        }

        /// <MetaDataID>{096141be-50b4-486a-b8d4-eeb98eb28108}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is StorageInstanceRef)
            {

                StorageInstanceRef storageInstanceRef = obj as StorageInstanceRef;
                //var stCell = storageInstanceRef.ObjectStorage.GetStorageCell(storageInstanceRef.StorageInstanceSet.SerialNumber);

                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            return null;
        }









        /// <MetaDataID>{c502ddbf-75aa-4122-8d1c-c3a9beddb193}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.ObjectID GetTemporaryObjectID()
        {
            return new OOAdvantech.WindowsAzureTablesPersistenceRunTime.ObjectID(System.Guid.NewGuid());
        }

        /// <MetaDataID>{89ebcce7-fc6a-485b-9173-f889b43c842d}</MetaDataID>
        public override void MakeChangesDurable(TransactionContext theTransaction)
        {

            (this.StorageMetaData as Storage).MetadataIdentitiesTable.UpdateEntity((this.StorageMetaData as Storage).MetadataIdentities);

            if (TableBatchOperations.ContainsKey(theTransaction.Transaction.LocalTransactionUri.ToLower()))
            {
                DateTime start = DateTime.Now;
                foreach (var transactionTableBatchOperationsEntry in TableBatchOperations[theTransaction.Transaction.LocalTransactionUri.ToLower()])
                {
                    foreach (var tableBatchOperation in transactionTableBatchOperationsEntry.Value)
                    {
                        bool retry = false;
                        try
                        {
                            transactionTableBatchOperationsEntry.Key.ExecuteBatch(tableBatchOperation);
                        }
                        catch (Exception error)
                        {
                            if (!retry)
                                throw;
                        }

                    }
                }
                TimeSpan timeSpan = DateTime.Now - start;
                //System.Windows.Forms.MessageBox.Show(timeSpan.ToString());
                TableBatchOperations.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());
            }


            if (TableBatchOperations_a.ContainsKey(theTransaction.Transaction.LocalTransactionUri.ToLower()))
            {
                DateTime start = DateTime.Now;
                foreach (var transactionTableBatchOperationsEntry in TableBatchOperations_a[theTransaction.Transaction.LocalTransactionUri.ToLower()])
                {
                    foreach (var tableBatchOperation in transactionTableBatchOperationsEntry.Value)
                    {
                        bool retry = false;
                        try
                        {
                            
                            transactionTableBatchOperationsEntry.Key.SubmitTransaction(tableBatchOperation);
                        }
                        catch (Exception error)
                        {
                            if (!retry)
                                throw;
                        }

                    }
                }
                TimeSpan timeSpan = DateTime.Now - start;
                //System.Windows.Forms.MessageBox.Show(timeSpan.ToString());
                TableBatchOperations.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());
            }

        }

        /// <MetaDataID>{57256822-3c6e-496f-8bfd-02a743181bc4}</MetaDataID>
        public override void PrepareForChanges(TransactionContext theTransaction)
        {

        }



        /// <MetaDataID>{8466c18a-4bbd-4f76-aa3a-47f0cdab015f}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID)
        {
            return new StorageInstanceRef(memoryInstance, this, objectID);
        }

        /// <MetaDataID>{33559478-0cf7-4776-9662-96eff4354def}</MetaDataID>
        public override MetaDataRepository.StorageCell GetStorageCell(StorageInstanceAgent storageInstanceAgent)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{8d191d9e-8ee9-493b-b715-89f8c54ec9af}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent deletedOutStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd, MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{24b5ba53-8bde-4132-b6b5-628df047722c}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c8428f95-1b6f-4e6a-a7a8-04e084265593}</MetaDataID>
        protected override StorageCellReference GetStorageCellReference(MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0950f5e9-fce9-4c91-8d0d-23ca85808001}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new NotImplementedException();
        }



        System.Collections.Generic.Dictionary<MetaDataRepository.Class, OOAdvantech.MetaDataRepository.StorageCell> StorageCells = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Class, OOAdvantech.MetaDataRepository.StorageCell>();

        /// <MetaDataID>{2f3242bb-a92b-4a5a-93c1-a953eb1a5f3f}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(Classifier classifier)
        {
            OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

            if ((classifier is MetaDataRepository.Class) && (!(classifier as MetaDataRepository.Class).Abstract))
            {

                System.Type type = classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                PersistenceLayerRunTime.ClassMemoryInstanceCollection classObjects = OperativeObjectCollections[type] as PersistenceLayerRunTime.ClassMemoryInstanceCollection;

                if (StorageCells.ContainsKey(classifier as MetaDataRepository.Class))
                    storageCells.Add(StorageCells[classifier as MetaDataRepository.Class]);
                else
                {

                    StorageCell storageCell = new StorageCell(_StorageMetaData.StorageIdentity, classifier as MetaDataRepository.Class, _StorageMetaData as MetaDataRepository.Namespace, classObjects);

                    storageCells.Add(storageCell);
                    StorageCells[classifier as MetaDataRepository.Class] = storageCell;
                }

            }
            foreach (MetaDataRepository.Classifier subClassifier in classifier.GetAllSpecializeClasifiers())
            {
                if (!(subClassifier is MetaDataRepository.Class) || (subClassifier as MetaDataRepository.Class).Abstract)
                    continue;

                System.Type type = subClassifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                PersistenceLayerRunTime.ClassMemoryInstanceCollection classObjects = OperativeObjectCollections[type] as PersistenceLayerRunTime.ClassMemoryInstanceCollection;

                if (StorageCells.ContainsKey(subClassifier as MetaDataRepository.Class))
                    storageCells.Add(StorageCells[subClassifier as MetaDataRepository.Class]);
                else
                {

                    StorageCell storageCell = new StorageCell(_StorageMetaData.StorageIdentity, subClassifier as MetaDataRepository.Class, _StorageMetaData as MetaDataRepository.Namespace, classObjects);

                    storageCells.Add(storageCell);
                    StorageCells[subClassifier as MetaDataRepository.Class] = storageCell;
                }
            }

            return storageCells;
        }


        /// <MetaDataID>{5ed051db-f882-4c6e-bd97-5b9758272eac}</MetaDataID>
        public override MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {
            PersistenceLayer.ObjectStorage ObjectStorage = this;
            string Query = "SELECT StorageCell FROM " + typeof(StorageCell).FullName + " StorageCell WHERE StorageCell.SerialNumber = " + storageCellSerialNumber.ToString();

            Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorage(ObjectStorage.StorageMetaData).Execute(Query);
            foreach (Collections.StructureSet Rowset in aStructureSet)
            {
                //TODO: να τσεκαριστή εάν υπάρχει συμβατότητα μεταξύ της class που τρέχει τοπικά
                //και αυτής που τρέχει remotely
                //WHERE oid = "+StorageCellID.ToString();
                StorageCell storageCell = (StorageCell)Rowset["StorageCell"];
                return storageCell;

            }
            return null;
        }

        /// <MetaDataID>{c3dbb657-b75f-49ef-828e-5d80ff8b86e0}</MetaDataID>
        public override MetaDataRepository.StorageCell GetStorageCell(object ObjectID)
        {
            throw new NotImplementedException();
        }

        public override Collections.Generic.Set<RelatedStorageCell> GetLinkedStorageCells(MetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCell, string ofTypeIdentity = null)
        {
            throw new NotImplementedException();
        }

        public override Collections.Generic.Set<RelatedStorageCell> GetRelationObjectsStorageCells(MetaDataRepository.Association association, Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCells, Roles storageCellsRole, string ofTypeIdentity = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateOperativeObjects()
        {
        }
    }
}
