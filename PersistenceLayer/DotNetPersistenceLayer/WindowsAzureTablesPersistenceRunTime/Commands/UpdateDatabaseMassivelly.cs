using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{f26b8bc9-e252-4ea3-8311-052806a21f95}</MetaDataID>
    public class UpdateDatabaseMassivelly : PersistenceLayerRunTime.Commands.Command
    {
        /// <MetaDataID>{59348814-217a-4022-8a7c-3483c029572b}</MetaDataID>
        string _Identity;
        /// <MetaDataID>{434690ea-280b-4a89-8ebb-52bfe7a8681b}</MetaDataID>
        public UpdateDatabaseMassivelly()
        {
            _Identity = "UM_" + Transactions.Transaction.Current.LocalTransactionUri;

        }
        /// <MetaDataID>{d6b51e2c-2524-4f78-82c5-f839d5e19454}</MetaDataID>
        static System.Collections.Generic.Dictionary<string, UpdateDatabaseMassivelly> UpdateDatabaseMassivellyCommands = new Dictionary<string, UpdateDatabaseMassivelly>();
        /// <MetaDataID>{b87055d5-f7a5-433a-86ab-19f2fbfee1d8}</MetaDataID>
        public static UpdateDatabaseMassivelly CurrentTransactionCommandUpdateMassivelly
        {
            get
            {
                string localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                UpdateDatabaseMassivelly updateDatabaseMassivelly = null;
                if (!UpdateDatabaseMassivellyCommands.TryGetValue(Transactions.Transaction.Current.LocalTransactionUri, out updateDatabaseMassivelly))
                {
                    updateDatabaseMassivelly = new UpdateDatabaseMassivelly();
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateDatabaseMassivelly);
                    UpdateDatabaseMassivellyCommands[localTransactionUri] = updateDatabaseMassivelly;

                    //}
                    Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
                }
                return updateDatabaseMassivelly;
            }
        }

        /// <MetaDataID>{585edaad-5239-4b57-87b4-babe07071e54}</MetaDataID>
        static void OnTransactionCompleted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
            UpdateDatabaseMassivellyCommands.Remove(transaction.LocalTransactionUri);

        }
        /// <MetaDataID>{a18e5042-530e-43f4-8543-fb2fae645b20}</MetaDataID>
        public static int CommandOrder
        {
            get
            {
                return 200;
            }
        }
        /// <MetaDataID>{33608ecc-8258-44cd-948f-8a92cdcac574}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }

        public override int ExecutionOrder
        {
            get
            {
                return PersistenceLayerRunTime.TransactionContext.LastCommandOrder;
            }
        }

        /// <MetaDataID>{a101e011-4e8e-439e-86a8-74d8e44f8f98}</MetaDataID>
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
                        objectStorage.TransferTableRecords(dataTable, table);
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

            foreach (RDBMSMetaDataRepository.Table table in TablesWithNewRelationRows.Keys)
            {
                ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as ObjectStorage;
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = TablesWithNewRelationRows[table];
                dataTable.EndLoadData();
                objectStorage.TransferTableRecords(dataTable, table);
            }




            foreach (RDBMSMetaDataRepository.Table table in TablesWithUpdatetingInstances.Keys)
            {
                ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as ObjectStorage;
                List<string> OIDColumns = new List<string>();
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    OIDColumns.Add(column.DataBaseColumnName);

                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = TablesWithUpdatetingInstances[table];
                dataTable.EndLoadData();

                objectStorage.UpdateTableRecords(dataTable, OIDColumns);

            }



            foreach (RDBMSMetaDataRepository.Table table in TablesWithDeletedInstances.Keys)
            {
                ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as ObjectStorage;

                MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = TablesWithDeletedInstances[table];
                dataTable.EndLoadData();
                objectStorage.DeleteTableRecords(dataTable);

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
        System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>> StorageInstanceRefsRows = new Dictionary<PersistenceLayerRunTime.StorageInstanceRef, Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>>();

        /// <MetaDataID>{1b8f703a-fbba-4d9f-bec9-33be456cccde}</MetaDataID>
        System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>> DeletedStorageInstance = new Dictionary<PersistenceLayerRunTime.StorageInstanceRef, Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>>();

        /// <MetaDataID>{eb0baf15-1557-4121-ad61-b756d014342f}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable> TablesWithNewInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable>();

        /// <MetaDataID>{53b2aab4-4a07-4dc4-9cfa-478c9f3b5b00}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable> TablesWithUpdatetingInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable>();

        /// <MetaDataID>{c057e4bd-0fd7-427e-ab26-98fbda2d4d5a}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable> TablesWithDeletedInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable>();
        /// <MetaDataID>{87688f12-ae94-4c57-ab55-b76b7c0d683c}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable> TablesWithNewRelationRows = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataTable>();

        /// <MetaDataID>{85266fdc-b020-45f5-b1eb-3b5752d24f19}</MetaDataID>
        internal void NewStorageInstance(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstance, ObjectID objectID)
        {
            PersistenceLayerRunTime.ObjectStorage objectStorage = storageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            TypeDictionary typeDictionary = new TypeDictionary();
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

                    foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                        dataTable.Columns.Add(column.DataBaseColumnName, typeDictionary.GetDataTransferDotNetType(column.Type.GetExtensionMetaObject<Type>()));
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
                    dataTable.Columns.Add("StorageInstanceRef_" + dataTable.GetHashCode().ToString(), typeof(PersistenceLayer.StorageInstanceRef));

                }

                dataRow = dataTable.NewRow();
                StorageInstanceRefsRows[storageInstance][table] = dataRow;
                dataTable.Rows.Add(dataRow);
                // = dataTable.NewRow();

            }

            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
            {


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
                {
                    dataRow[column.DataBaseColumnName] = value;
                    if (attributeValue.Value is DateTime)
                    {
                     
                        if (((DateTime)dataRow[column.DataBaseColumnName]).ToUniversalTime() != ((DateTime)attributeValue.Value).ToUniversalTime())
                            dataRow[column.DataBaseColumnName] = ((DateTime)attributeValue.Value).ToLocalTime();
                        if (((DateTime)dataRow[column.DataBaseColumnName]).ToUniversalTime() != ((DateTime)attributeValue.Value).ToUniversalTime())
                            dataRow[column.DataBaseColumnName] = ((DateTime)attributeValue.Value).ToUniversalTime();
                    }

                }

                
                    


            }
            if (rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn.DataBaseColumnName] = storageInstance.ReferentialIntegrityCount;

            dataRow = StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable];
            if (dataRow.Table.Columns.Contains("TypeID"))
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

        internal object GetStorageInstanceColumnValue(PersistenceLayerRunTime.StorageInstanceRef storageInstance, string columnName)
        {
            if (StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable];
                return dataRow["columnName"];
            }
            return null;
        }
        internal void SetStorageInstanceColumnValue(PersistenceLayerRunTime.StorageInstanceRef storageInstance, string columnName, object value)
        {
            OOAdvantech.PersistenceLayerRunTime.ObjectStorage objectStorage = storageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
            TypeDictionary typeDictionary = new TypeDictionary();

            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                //if (rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn != null)
                MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable];
                value = typeDictionary.Convert(value, dataRow.Table.Columns[columnName].DataType);
                if (value != null)
                    dataRow[columnName] = value;
            }
            else
            {
                StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
                foreach (RDBMSMetaDataRepository.Table table in (storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MappedTables)
                {
                    MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = null;
                    if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                    {
                        if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                        {
                            dataTable = MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(table.DataBaseTableName);
                            dataTable.ExtendedProperties.Add("TableMetaData", table);
                            dataTable.BeginLoadData();
                            TablesWithUpdatetingInstances[table] = dataTable;

                            foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                                dataTable.Columns.Add(column.DataBaseColumnName, column.Type.GetExtensionMetaObject<Type>());
                            if (table.ReferentialIntegrityColumn != null)
                                dataTable.Columns.Add(table.ReferentialIntegrityColumn.DataBaseColumnName, table.ReferentialIntegrityColumn.Type.GetExtensionMetaObject<Type>());


                            foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                                dataTable.Columns.Add(column.DataBaseColumnName, column.Type.GetExtensionMetaObject<Type>());

                            dataTable.Columns.Add("StorageInstanceRef", typeof(StorageInstanceRef));
                        }
                    }
                    // StorageInstanceRefsRows[storageInstance][table] = dataTable.NewRow();
                    MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = dataTable.NewRow();
                    StorageInstanceRefsRows[storageInstance][table] = dataRow;
                    dataRow["StorageInstanceRef"] = storageInstance;
                    dataTable.Rows.Add(dataRow);

                }
                foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
                {


                    RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attributeValue.Attribute) as RDBMSMetaDataRepository.Attribute;// attributeValue.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                    RDBMSMetaDataRepository.Column column = null;
                    foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor((storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell)))
                    {
                        if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                        {
                            column = attributeColumn;
                            break;
                        }
                    }
                    StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.DataBaseColumnName] = attributeValue.Value;
                }
                if ((storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.GetColumn("TypeID") != null)
                    StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
                //StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
                foreach (RDBMSMetaDataRepository.IdentityColumn column in (storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                    StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable][column.DataBaseColumnName] = ((PersistenceLayer.ObjectID)storageInstance.PersistentObjectID).GetMemberValue(column.ColumnType);
                foreach (RDBMSMetaDataRepository.Column column in (storageInstance as StorageInstanceRef).RelationshipColumnsValues.Keys)
                    StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.DataBaseColumnName] = (storageInstance as StorageInstanceRef).RelationshipColumnsValues[column];
                value = typeDictionary.Convert(value, StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable].Table.Columns[columnName].DataType);
                if (value != null)
                    StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable][columnName] = value;
            }


            //foreach(System.Collections.Generic.KeyValuePair<RDBMSMetaDataRepository.Column, object> entry in (storageInstance as StorageInstanceRef).RelationshipColumnsValues)
            //{
            //    if(entry.Key.Name==columnName)
            //    {
            //        (storageInstance as StorageInstanceRef).RelationshipColumnsValues[entry.Key]=value;
            //        break;
            //    }
            //}

        }



        /// <MetaDataID>{2f37f0ad-b9d5-4261-adfb-da3399aec646}</MetaDataID>
        internal void UpdateStorageInstance(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            OOAdvantech.PersistenceLayerRunTime.ObjectStorage objectStorage = storageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (!StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
                foreach (RDBMSMetaDataRepository.Table table in (storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MappedTables)
                {
                    MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = null;
                    if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                    {
                        dataTable = MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(table.DataBaseTableName);
                        dataTable.ExtendedProperties.Add("TableMetaData", table);
                        dataTable.BeginLoadData();
                        TablesWithUpdatetingInstances[table] = dataTable;

                        foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                            dataTable.Columns.Add(column.DataBaseColumnName, column.Type.GetExtensionMetaObject<Type>());
                        if (table.ReferentialIntegrityColumn != null)
                            dataTable.Columns.Add(table.ReferentialIntegrityColumn.DataBaseColumnName, table.ReferentialIntegrityColumn.Type.GetExtensionMetaObject<Type>());
                        foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                            dataTable.Columns.Add(column.DataBaseColumnName, column.Type.GetExtensionMetaObject<Type>());
                        dataTable.Columns.Add("StorageInstanceRef", typeof(StorageInstanceRef));
                    }
                    // StorageInstanceRefsRows[storageInstance][table] = dataTable.NewRow();
                    MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = dataTable.NewRow();
                    StorageInstanceRefsRows[storageInstance][table] = dataRow;
                    dataRow["StorageInstanceRef"] = storageInstance;
                    dataTable.Rows.Add(dataRow);
                    // = dataTable.NewRow();
                }
            }
            TypeDictionary typeDictionary = new TypeDictionary();

            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attributeValue.Attribute) as RDBMSMetaDataRepository.Attribute;
                RDBMSMetaDataRepository.Column column = null;
                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell))
                {
                    if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }
                MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table];

                object value = typeDictionary.Convert(attributeValue.Value, dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                if (value != null)
                {
                    dataRow[column.DataBaseColumnName] = value;
                    if (attributeValue.Value is DateTime)
                    {
                      
                        if (((DateTime)dataRow[column.DataBaseColumnName]).ToUniversalTime() != ((DateTime)attributeValue.Value).ToUniversalTime())
                            dataRow[column.DataBaseColumnName] = ((DateTime)attributeValue.Value).ToLocalTime();
                        if (((DateTime)dataRow[column.DataBaseColumnName]).ToUniversalTime() != ((DateTime)attributeValue.Value).ToUniversalTime())
                            dataRow[column.DataBaseColumnName] = ((DateTime)attributeValue.Value).ToUniversalTime();
                    }


                }

            }
            if (StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable].Table.Columns.IndexOf("TypeID") != -1)
            {
                //TODO ειναι αργός ο κωδικας κάτι σε cashing
                StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
                StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
            }
            if ((((StorageInstanceRef)storageInstance).StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn.DataBaseColumnName] = storageInstance.ReferentialIntegrityCount;

            foreach (RDBMSMetaDataRepository.Table table in (storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MappedTables)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    StorageInstanceRefsRows[storageInstance][(storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable][column.DataBaseColumnName] = ((PersistenceLayer.ObjectID)storageInstance.PersistentObjectID).GetMemberValue(column.ColumnType);
            }

            foreach (RDBMSMetaDataRepository.Column column in (storageInstance as StorageInstanceRef).RelationshipColumnsValues.Keys)
                StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.DataBaseColumnName] = (storageInstance as StorageInstanceRef).RelationshipColumnsValues[column];

        }
        /// <MetaDataID>{d61fa255-c971-491b-9d96-af0340d1c231}</MetaDataID>
        internal void DeleteStorageInstance(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            OOAdvantech.PersistenceLayerRunTime.ObjectStorage objectStorage = storageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                foreach (KeyValuePair<RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow> entry in StorageInstanceRefsRows[storageInstance])
                    entry.Value.Delete();
                StorageInstanceRefsRows.Remove(storageInstance);
            }

            if (!DeletedStorageInstance.ContainsKey(storageInstance))
            {
                DeletedStorageInstance[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
                foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
                {
                    MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable = null;
                    if (!TablesWithDeletedInstances.TryGetValue(table, out dataTable))
                    {
                        dataTable = MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(table.DataBaseTableName);
                        //dataTable.ExtendedProperties.Add("TableMetaData", table);
                        dataTable.BeginLoadData();
                        TablesWithDeletedInstances[table] = dataTable;

                        foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                            dataTable.Columns.Add(column.DataBaseColumnName, column.Type.GetExtensionMetaObject<Type>());

                        dataTable.Columns.Add("StorageInstanceRef", typeof(OOAdvantech.PersistenceLayer.StorageInstanceRef));

                    }
                    MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = dataTable.NewRow();
                    DeletedStorageInstance[storageInstance][table] = dataRow;
                    dataTable.Rows.Add(dataRow);

                }
            }
            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    DeletedStorageInstance[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][column.DataBaseColumnName] = ((PersistenceLayer.ObjectID)storageInstance.PersistentObjectID).GetMemberValue(column.ColumnType);
            }
            DeletedStorageInstance[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["StorageInstanceRef"] = storageInstance;
        }
        Dictionary<MetaDataRepository.ObjectQueryLanguage.MultiPartKey, MetaDataRepository.ObjectQueryLanguage.IDataRow> RelationRows = new Dictionary<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.MultiPartKey, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
        /// <MetaDataID>{810d9c65-f08b-4809-89f8-878eeb0ab319}</MetaDataID>
        internal void AddRelationRow(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink, PersistenceLayer.ObjectID relationObjectObjectID, Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues, int RoleAIndex, Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues, int RoleBIndex)
        {
            MetaDataRepository.ObjectQueryLanguage.MultiPartKey key = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.MultiPartKey(roleAColumnsValues.Count + roleBColumnsValues.Count);
            int i = 0;
            foreach (object value in roleAColumnsValues.Values)
                key.KeyPartsValues[i] = value;

            foreach (object value in roleBColumnsValues.Values)
                key.KeyPartsValues[i] = value;

            MetaDataRepository.ObjectQueryLanguage.IDataTable relationTable = null;
            if (!TablesWithNewRelationRows.TryGetValue(objectCollectionsLink.ObjectLinksTable, out relationTable))
            {
                relationTable = MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(objectCollectionsLink.ObjectLinksTable.DataBaseTableName);
                relationTable.ExtendedProperties.Add("TableMetaData", objectCollectionsLink.ObjectLinksTable);
                relationTable.BeginLoadData();
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in objectCollectionsLink.ObjectLinksTable.ObjectIDColumns)
                    relationTable.Columns.Add(identityColumn.DataBaseColumnName, (identityColumn as MetaDataRepository.IIdentityPart).Type);
                foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleAColumnsValues)
                    relationTable.Columns.Add(entry.Key.DataBaseColumnName, entry.Value.GetType());
                if (objectCollectionsLink.Type.RoleA.Indexer)
                {
                    RDBMSMetaDataRepository.Column indexerColumn = (objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable);
                    relationTable.Columns.Add(indexerColumn.DataBaseColumnName, typeof(int));
                }

                foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleBColumnsValues)
                    relationTable.Columns.Add(entry.Key.DataBaseColumnName, entry.Value.GetType());
                if (objectCollectionsLink.Type.RoleB.Indexer)
                {
                    RDBMSMetaDataRepository.Column indexerColumn = (objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable);
                    relationTable.Columns.Add(indexerColumn.DataBaseColumnName, typeof(int));
                }
                TablesWithNewRelationRows[objectCollectionsLink.ObjectLinksTable] = relationTable;
            }

            MetaDataRepository.ObjectQueryLanguage.IDataRow row = relationTable.NewRow();
            RelationRows[key] = row;

            foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleAColumnsValues)
                row[entry.Key.DataBaseColumnName] = entry.Value;
            if (objectCollectionsLink.Type.RoleA.Indexer)
                row[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName] = RoleAIndex;

            foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleBColumnsValues)
                row[entry.Key.DataBaseColumnName] = entry.Value;
            if (objectCollectionsLink.Type.RoleB.Indexer)
                row[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName] = RoleBIndex;
            if (relationObjectObjectID != null)
                foreach (MetaDataRepository.IIdentityPart part in relationObjectObjectID.ObjectIdentityType.Parts)
                    row[part.Name] = relationObjectObjectID.GetMemberValue(part.Name);
            relationTable.Rows.Add(row);
        }



        internal void SetRelationRow(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink, PersistenceLayer.ObjectID relationObjectObjectID, Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues, int RoleAIndex, Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues, int RoleBIndex)
        {
            MetaDataRepository.ObjectQueryLanguage.MultiPartKey key = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.MultiPartKey(roleAColumnsValues.Count + roleBColumnsValues.Count);
            int i = 0;
            foreach (object value in roleAColumnsValues.Values)
                key.KeyPartsValues[i] = value;

            foreach (object value in roleBColumnsValues.Values)
                key.KeyPartsValues[i] = value;

            MetaDataRepository.ObjectQueryLanguage.IDataTable relationTable = null;
            if (!TablesWithNewRelationRows.TryGetValue(objectCollectionsLink.ObjectLinksTable, out relationTable))
                return;

            MetaDataRepository.ObjectQueryLanguage.IDataRow row = null;
            if (RelationRows.TryGetValue(key, out row))
            {
                //foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleAColumnsValues)
                //    row[entry.Key.DataBaseColumnName] = entry.Value;
                if (objectCollectionsLink.Type.RoleA.Indexer)
                    row[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName] = RoleAIndex;

                //foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleBColumnsValues)
                //    row[entry.Key.DataBaseColumnName] = entry.Value;
                if (objectCollectionsLink.Type.RoleB.Indexer)
                    row[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName] = RoleBIndex;
                //if (relationObjectObjectID != null)
                //    foreach (MetaDataRepository.IIdentityPart part in relationObjectObjectID.ObjectIdentityType.Parts)
                //        row[part.Name] = relationObjectObjectID.GetMemberValue(part.Name);
            }
        }

    }
}
