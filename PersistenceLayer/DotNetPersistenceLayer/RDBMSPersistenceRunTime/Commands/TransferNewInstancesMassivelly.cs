using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
    /// <MetaDataID>{73924773-1a2f-4971-b31d-03ce2c7bdfc8}</MetaDataID>
    class TransferNewInstancesMassivelly: PersistenceLayerRunTime.Commands.Command
    {
        /// <MetaDataID>{59348814-217a-4022-8a7c-3483c029572b}</MetaDataID>
        string _Identity;
        /// <MetaDataID>{434690ea-280b-4a89-8ebb-52bfe7a8681b}</MetaDataID>
        public TransferNewInstancesMassivelly()
        {
            _Identity = "TNM_" + Transactions.Transaction.Current.LocalTransactionUri;

        }
        /// <MetaDataID>{d6b51e2c-2524-4f78-82c5-f839d5e19454}</MetaDataID>
        static System.Collections.Generic.Dictionary<string, TransferNewInstancesMassivelly> TransferNewInstancesMassivellyCommands = new Dictionary<string, TransferNewInstancesMassivelly>();
        /// <MetaDataID>{b87055d5-f7a5-433a-86ab-19f2fbfee1d8}</MetaDataID>
        public static TransferNewInstancesMassivelly CurrentTransferNewInstancesMassivelly
        {
            get
            {
                string localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                TransferNewInstancesMassivelly transferNewInstancesMassivelly = null;
                if (!TransferNewInstancesMassivellyCommands.TryGetValue(Transactions.Transaction.Current.LocalTransactionUri, out transferNewInstancesMassivelly))
                {
                    transferNewInstancesMassivelly = new TransferNewInstancesMassivelly();
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(transferNewInstancesMassivelly);
                    TransferNewInstancesMassivellyCommands[localTransactionUri] = transferNewInstancesMassivelly;
                    Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
                }
                return transferNewInstancesMassivelly;
            }
        }

        /// <MetaDataID>{585edaad-5239-4b57-87b4-babe07071e54}</MetaDataID>
        static void OnTransactionCompleted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
            TransferNewInstancesMassivellyCommands.Remove(transaction.LocalTransactionUri);

        }
        /// <MetaDataID>{a18e5042-530e-43f4-8543-fb2fae645b20}</MetaDataID>
        public static int CommandOrder
        {
            get
            {
                return 200;
            }
        }
        /// <MetaDataID>{be044b5b-c4b5-4090-a95d-1d3d0906be72}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }

        public override int ExecutionOrder
        {
            get
            {
                return  20;
            }
        }

        /// <MetaDataID>{2f1386ba-44c1-4059-8d2a-4a788a3d1063}</MetaDataID>
        public override void Execute()
        {

            List<RDBMSMetaDataRepository.Table> tablesWithNewInstances = new List<OOAdvantech.RDBMSMetaDataRepository.Table>(TablesWithNewInstances.Keys);

            while (tablesWithNewInstances.Count > 0)
            {
                bool tableTransfered = false;
                foreach (RDBMSMetaDataRepository.Table table in new List<RDBMSMetaDataRepository.Table>(tablesWithNewInstances))
                {
                    bool skip = false;
                    foreach (RDBMSMetaDataRepository.Key key in table.ForeignKeys)
                    {
                        if (!key.IsPrimaryKey && tablesWithNewInstances.Contains(key.ReferedTable))
                        {
                            skip = true;
                            break;
                        }
                    }
                    if (!skip)
                    {

                        ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as ObjectStorage;
                        MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = TablesWithNewInstances[table];
                        dataTable.EndLoadData();
                        objectStorage.TransferTableRecords(dataTable,table);
                        tablesWithNewInstances.Remove(table);
                        tableTransfered = true;

                    }

                }
                if (!tableTransfered)
                    break;

            }
            //TODO Θα πρέπει να γραφτεί ένα test case όπου θα υπάρχει κυκλική εξάρτιση μέσω foreign keys 
            foreach (RDBMSMetaDataRepository.Table table in tablesWithNewInstances)
            {
                ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as ObjectStorage;
                MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = TablesWithNewInstances[table];
                dataTable.EndLoadData();
                objectStorage.TransferTableRecords(dataTable, table);
            }
        }

        public override string Identity
        {
            get
            {
                return _Identity;
            }
        }
        /// <MetaDataID>{b51f0659-fff3-45b1-866a-c94a23e5155b}</MetaDataID>
        System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>> StorageInstanceRefsRows = new Dictionary<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef, Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>>();

           /// <MetaDataID>{eb0baf15-1557-4121-ad61-b756d014342f}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable> TablesWithNewInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable>();

 
 
        /// <MetaDataID>{85266fdc-b020-45f5-b1eb-3b5752d24f19}</MetaDataID>
        internal void NewStorageInstance(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstance, RDBMSPersistenceRunTime.ObjectID objectID)
        {
            PersistenceLayerRunTime.ObjectStorage objectStorage = storageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            RDBMSMetaDataPersistenceRunTime.TypeDictionary typeDictionary = (objectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary;
            MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = null;
            StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = null;
                if (!TablesWithNewInstances.TryGetValue(table, out dataTable))
                {
                    dataTable = MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(table.DataBaseTableName);
                    dataTable.ExtendedProperties.Add("TableMetaData", table);
                    dataTable.BeginLoadData();
                    TablesWithNewInstances[table] = dataTable;

                    if (!rdbmsMetadataClass.ActiveStorageCell.OIDIsDBGenerated)
                    {
                        foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                            dataTable.Columns.Add(column.DataBaseColumnName, typeDictionary.GetDataTransferDotNetType(column.Type.GetExtensionMetaObject<Type>()));
                    }
                    if (table.ReferentialIntegrityColumn != null)
                        dataTable.Columns.Add(table.ReferentialIntegrityColumn.DataBaseColumnName, typeDictionary.GetDataTransferDotNetType(table.ReferentialIntegrityColumn.Type.GetExtensionMetaObject<Type>()));
                    
                    foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                    {
                        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataColumn dataColumn = null;
                        if (column.MappedAttribute != null)
                        {
                            if (column.Type is OOAdvantech.MetaDataRepository.Enumeration)
                                dataColumn = dataTable.Columns.Add(column.DataBaseColumnName, typeof(int));
                            else
                                dataColumn = dataTable.Columns.Add(column.DataBaseColumnName, typeDictionary.GetDataTransferDotNetType(column.Type.GetExtensionMetaObject<Type>()));
                        }
                        else
                        {
                            dataColumn = dataTable.Columns.Add(column.DataBaseColumnName, typeDictionary.GetDataTransferDotNetType(column.Type.GetExtensionMetaObject<Type>()));
                            //if (dataColumn.DataType == typeof(System.Guid))
                            //    dataColumn.DefaultValue = System.Guid.Empty;
                        }
                    }
                    dataTable.Columns.Add("StorageInstanceRef_" + dataTable.GetHashCode().ToString(),typeof(PersistenceLayer.StorageInstanceRef));
                    
                }

                dataRow = dataTable.NewRow();
                StorageInstanceRefsRows[storageInstance][table] = dataRow;
                dataTable.Rows.Add(dataRow);
                // = dataTable.NewRow();

            }

            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
            {

                //TODO Σε customing mapping κοπανάει όταν δεν έχω κάνει mapping ενα property
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (storageInstance.ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attributeValue.Attribute) as RDBMSMetaDataRepository.Attribute;
                RDBMSMetaDataRepository.Column column = null;
                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(rdbmsMetadataClass.ActiveStorageCell))
                {
                    if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }
                dataRow = StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table];


                object value = typeDictionary.Convert(attributeValue.Value, dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                if (value != null)
                    dataRow[column.DataBaseColumnName] = value;

                //if(attributeValue.Value!=null)
                //    dataRow[column.DataBaseColumnName] = typeDictionary.Convert(attributeValue.Value, dataRow.Table.Columns[column.DataBaseColumnName].DataType);


            }
            if (rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn.DataBaseColumnName] = storageInstance.ReferentialIntegrityCount;

            dataRow = StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable];
            if(dataRow.Table.Columns.Contains("TypeID")) 
                dataRow["TypeID"] = rdbmsMetadataClass.TypeID;
            dataRow["StorageInstanceRef_" + dataRow.Table.GetHashCode().ToString()] = storageInstance;
                
             
            if (objectID != null)
            {
                foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
                {
                    foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                        dataRow[column.DataBaseColumnName] = typeDictionary.Convert(objectID.GetMemberValue(column.ColumnType), dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                }
            }

        }
    
    }
}
