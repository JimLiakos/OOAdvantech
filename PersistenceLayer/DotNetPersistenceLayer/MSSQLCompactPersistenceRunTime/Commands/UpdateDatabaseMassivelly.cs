using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
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
        /// <MetaDataID>{b87055d5-f7a5-433a-86ab-19f2fbfee1d8}</MetaDataID>
        public static UpdateDatabaseMassivelly CurrentTransactionCommandUpdateMassivelly
        {
            get
            {
                string identity = "UM_" + Transactions.Transaction.Current.LocalTransactionUri;


                if (PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.ContainCommand(identity))
                    return PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands[identity] as UpdateDatabaseMassivelly;
                else
                {
                    UpdateDatabaseMassivelly updateDatabaseMassivelly = new UpdateDatabaseMassivelly();
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateDatabaseMassivelly);
                    return updateDatabaseMassivelly;
                }
            }
        }



        /// <MetaDataID>{a18e5042-530e-43f4-8543-fb2fae645b20}</MetaDataID>
        public static int CommandOrder
        {
            get
            {
                return 200;
            }
        }

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

        public override void Execute()
        {

            List<RDBMSMetaDataRepository.Table> tablesWithNewInstances = new List<OOAdvantech.RDBMSMetaDataRepository.Table>(TablesWithNewInstances.Keys);

            while (tablesWithNewInstances.Count > 0)
            {
                bool tableTransfered = false;
                foreach (RDBMSMetaDataRepository.Table table in new List<RDBMSMetaDataRepository.Table>(tablesWithNewInstances))
                {
                    bool skip=false;
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
                        OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage;
                        objectStorage.TransferTableRecords(TablesWithNewInstances[table]);
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
                OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage;
                objectStorage.TransferTableRecords(TablesWithNewInstances[table]);
            }

            foreach (RDBMSMetaDataRepository.Table table in TablesWithNewRelationRows.Keys)
            {
                OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage;
                objectStorage.TransferTableRecords(TablesWithNewRelationRows[table]);
            }


            

            foreach (RDBMSMetaDataRepository.Table table in TablesWithUpdatetingInstances.Keys)
            {
                OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage;
                List<string> OIDColumns = new List<string>();
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    OIDColumns.Add(column.DataBaseColumnName);
                objectStorage.UpdateTableRecords(TablesWithUpdatetingInstances[table], OIDColumns);

            }

            foreach (RDBMSMetaDataRepository.Table table in TablesWithDeletedInstances.Keys)
            {
                OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((table.Namespace as RDBMSMetaDataRepository.Storage).StorageName,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageLocation,
                                                            (table.Namespace as RDBMSMetaDataRepository.Storage).StorageType) as OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectStorage;

                objectStorage.DeleteTableRecords(TablesWithDeletedInstances[table]);

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
        System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataRow>> StorageInstanceRefsRows = new Dictionary<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef, Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>>();

        /// <MetaDataID>{1b8f703a-fbba-4d9f-bec9-33be456cccde}</MetaDataID>
        System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataRow>> DeletedStorageInstance = new Dictionary<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef, Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>>();

        /// <MetaDataID>{eb0baf15-1557-4121-ad61-b756d014342f}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataTable> TablesWithNewInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataTable>();

        /// <MetaDataID>{53b2aab4-4a07-4dc4-9cfa-478c9f3b5b00}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataTable> TablesWithUpdatetingInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataTable>();

        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataTable> TablesWithDeletedInstances = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataTable>();
        /// <MetaDataID>{87688f12-ae94-4c57-ab55-b76b7c0d683c}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Table, System.Data.DataTable> TablesWithNewRelationRows = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataTable>();

        /// <MetaDataID>{85266fdc-b020-45f5-b1eb-3b5752d24f19}</MetaDataID>
        internal void NewStorageInstance(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstance, RDBMSPersistenceRunTime.ObjectID objectID)
        {
            ObjectStorage objectStorage = (ObjectStorage)storageInstance.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>();
            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                System.Data.DataTable dataTable = null;
                if (!TablesWithNewInstances.TryGetValue(table, out dataTable))
                {
                    dataTable = new System.Data.DataTable(table.Name);
                    TablesWithNewInstances[table] = dataTable;

                    foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                        dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                    if (table.ReferentialIntegrityColumn != null)
                        dataTable.Columns.Add(table.ReferentialIntegrityColumn.Name, ModulePublisher.ClassRepository.GetType(table.ReferentialIntegrityColumn.Type.FullName, ""));

                    foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                    {
                        if (column.MappedAttribute != null)
                        {
                            if (column.Type is OOAdvantech.MetaDataRepository.Enumeration)
                                dataTable.Columns.Add(column.Name, typeof(int));
                            else
                                dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));

                        }
                        else
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                    }
                }

                System.Data.DataRow dataRow = dataTable.NewRow();
                StorageInstanceRefsRows[storageInstance][table] = dataRow;
                dataTable.Rows.Add(dataRow);
                // = dataTable.NewRow();

            }

            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
            {


                RDBMSMetaDataRepository.Attribute rdbmsAttribute = attributeValue.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                RDBMSMetaDataRepository.Column column = null;
                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(rdbmsMetadataClass.ActiveStorageCell))
                {
                    if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }

                StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.Name] = attributeValue.Value;

            }
            if (rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn.Name] = storageInstance.ReferentialIntegrityCount;

            StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;

            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][column.Name] = objectID.GetMemberValue(column.ColumnType);
            }

        }



        /// <MetaDataID>{ead68558-bfc4-4383-b4a3-b14b1486697b}</MetaDataID>
        internal void SetStorageInstanceColumnValue(PersistenceLayerRunTime.StorageInstanceRef storageInstance, string columnName, object value)
        {
            ObjectStorage objectStorage = (ObjectStorage)storageInstance.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                //if (rdbmsMetadataClass.ActiveStorageCell.MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][columnName] = value;
            }
            else
            {
                StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>();
                foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
                {
                    System.Data.DataTable dataTable = null;
                    if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                    {
                        if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                        {
                            dataTable = new System.Data.DataTable(table.Name);
                            TablesWithUpdatetingInstances[table] = dataTable;

                            foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                                dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                            if (table.ReferentialIntegrityColumn != null)
                                dataTable.Columns.Add(table.ReferentialIntegrityColumn.Name, ModulePublisher.ClassRepository.GetType(table.ReferentialIntegrityColumn.Type.FullName, ""));


                            foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                                dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                        }
                    }
                    // StorageInstanceRefsRows[storageInstance][table] = dataTable.NewRow();
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    StorageInstanceRefsRows[storageInstance][table] = dataRow;
                    dataTable.Rows.Add(dataRow);

                }
                foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
                {


                    RDBMSMetaDataRepository.Attribute rdbmsAttribute = attributeValue.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                    RDBMSMetaDataRepository.Column column = null;
                    foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(rdbmsMetadataClass.ActiveStorageCell))
                    {
                        if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                        {
                            column = attributeColumn;
                            break;
                        }
                    }
                    StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.Name] = attributeValue.Value;
                }
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
                foreach (RDBMSMetaDataRepository.IdentityColumn column in rdbmsMetadataClass.ActiveStorageCell.MainTable.ObjectIDColumns)
                    StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][column.Name] = ((RDBMSPersistenceRunTime.ObjectID)storageInstance.ObjectID).GetMemberValue(column.ColumnType);
                foreach (RDBMSMetaDataRepository.Column column in (storageInstance as StorageInstanceRef).RelationshipColumnsValues.Keys)
                    StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.Name] = (storageInstance as StorageInstanceRef).RelationshipColumnsValues[column];

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
            ObjectStorage objectStorage = (ObjectStorage)storageInstance.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (!StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                StorageInstanceRefsRows[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>();
                foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
                {
                    System.Data.DataTable dataTable = null;
                    if (!TablesWithUpdatetingInstances.TryGetValue(table, out dataTable))
                    {
                        dataTable = new System.Data.DataTable(table.Name);
                        TablesWithUpdatetingInstances[table] = dataTable;

                        foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                        if (table.ReferentialIntegrityColumn != null)
                            dataTable.Columns.Add(table.ReferentialIntegrityColumn.Name, ModulePublisher.ClassRepository.GetType(table.ReferentialIntegrityColumn.Type.FullName, ""));


                        foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                    }
                    // StorageInstanceRefsRows[storageInstance][table] = dataTable.NewRow();
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    StorageInstanceRefsRows[storageInstance][table] = dataRow;
                    dataTable.Rows.Add(dataRow);

                    // = dataTable.NewRow();

                }
            }

            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstance.GetPersistentAttributeValues())
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = attributeValue.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                RDBMSMetaDataRepository.Column column = null;
                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(rdbmsMetadataClass.ActiveStorageCell))
                {
                    if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }

                StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.Name] = attributeValue.Value;

            }
            StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
            StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable]["TypeID"] = rdbmsMetadataClass.TypeID;
            if (((StorageInstanceRef)storageInstance).StorageInstanceSet.MainTable.ReferentialIntegrityColumn != null)
                StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][((StorageInstanceRef)storageInstance).StorageInstanceSet.MainTable.ReferentialIntegrityColumn.DataBaseColumnName] = storageInstance.ReferentialIntegrityCount;

            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    StorageInstanceRefsRows[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][column.Name] = ((RDBMSPersistenceRunTime.ObjectID)storageInstance.ObjectID).GetMemberValue(column.ColumnType);
            }

            foreach (RDBMSMetaDataRepository.Column column in (storageInstance as StorageInstanceRef).RelationshipColumnsValues.Keys)
                StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.Name] = (storageInstance as StorageInstanceRef).RelationshipColumnsValues[column];

        }



        /// <MetaDataID>{d61fa255-c971-491b-9d96-af0340d1c231}</MetaDataID>
        internal void DeleteStorageInstance(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            ObjectStorage objectStorage = (ObjectStorage)storageInstance.ObjectStorage;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
            if (StorageInstanceRefsRows.ContainsKey(storageInstance))
            {
                foreach (KeyValuePair<RDBMSMetaDataRepository.Table, System.Data.DataRow> entry in StorageInstanceRefsRows[storageInstance])
                    entry.Value.Delete();
                StorageInstanceRefsRows.Remove(storageInstance);
            }

            if (!DeletedStorageInstance.ContainsKey(storageInstance))
            {
                DeletedStorageInstance[storageInstance] = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.Table, System.Data.DataRow>();
                foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
                {
                    System.Data.DataTable dataTable = null;
                    if (!TablesWithDeletedInstances.TryGetValue(table,out dataTable))
                    {
                        dataTable = new System.Data.DataTable(table.Name);
                        TablesWithDeletedInstances[table] = dataTable;

                        foreach (RDBMSMetaDataRepository.Column column in table.ObjectIDColumns)
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                    }
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    DeletedStorageInstance[storageInstance][table] = dataRow;
                    dataTable.Rows.Add(dataRow);

                }
            }
            foreach (RDBMSMetaDataRepository.Table table in rdbmsMetadataClass.ActiveStorageCell.MappedTables)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn column in table.ObjectIDColumns)
                    DeletedStorageInstance[storageInstance][rdbmsMetadataClass.ActiveStorageCell.MainTable][column.Name] = ((RDBMSPersistenceRunTime.ObjectID)storageInstance.ObjectID).GetMemberValue(column.ColumnType);
            }

        }

        internal void AddRelationRow(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink, Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues, int RoleAIndex, Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues, int RoleBIndex)
        {
            System.Data.DataTable relationTable=null;
            if (!TablesWithNewRelationRows.TryGetValue(objectCollectionsLink.ObjectLinksTable, out relationTable))
            {
                relationTable = new System.Data.DataTable(objectCollectionsLink.ObjectLinksTable.DataBaseTableName);
                foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleAColumnsValues)
                    relationTable.Columns.Add(entry.Key.DataBaseColumnName, entry.Value.GetType());
                if (objectCollectionsLink.Type.RoleA.Indexer)
                    relationTable.Columns.Add((objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, typeof(int));

                foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleBColumnsValues)
                    relationTable.Columns.Add(entry.Key.DataBaseColumnName, entry.Value.GetType());
                if (objectCollectionsLink.Type.RoleB.Indexer)
                    relationTable.Columns.Add((objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, typeof(int));
                TablesWithNewRelationRows[objectCollectionsLink.ObjectLinksTable] = relationTable;
            }
            System.Data.DataRow row= relationTable.NewRow();

            foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleAColumnsValues)
                row[entry.Key.DataBaseColumnName]=entry.Value;
            if (objectCollectionsLink.Type.RoleA.Indexer)
                row[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName] = RoleAIndex;

            foreach (KeyValuePair<RDBMSMetaDataRepository.IdentityColumn, object> entry in roleBColumnsValues)
                row[entry.Key.DataBaseColumnName] = entry.Value;
            if (objectCollectionsLink.Type.RoleB.Indexer)
                row[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName] = RoleBIndex;
            relationTable.Rows.Add(row);




            
        }
    }
}
