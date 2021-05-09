using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
using Commands = OOAdvantech.RDBMSPersistenceRunTime.Commands;
using OOAdvantech.RDBMSMetaDataPersistenceRunTime;

namespace OOAdvantech.SQLitePersistenceRunTime
{
    /// <MetaDataID>{247960b0-9dda-4bf1-a4bc-3924ebe76290}</MetaDataID>
    public class ObjectStorage : RDBMSPersistenceRunTime.ObjectStorage
    {

        ///// <MetaDataID>{8ee21fd5-3455-430b-a05d-5142f1322e48}</MetaDataID>
        //public override DataLoader CreateDataLoader(DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
        //{
        //    return new ObjectQueryLanguage.DataLoader(dataNode, dataLoaderMetadata);
        //}

        /// <exclude>Excluded</exclude>
        OOAdvantech.PersistenceLayer.Storage _StorageMetaData;
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return _StorageMetaData;
            }
        }
        public override void CreateMoveStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }


        /// <MetaDataID>{95193b97-cc55-40da-9b36-bc1c0fe0bc20}</MetaDataID>
        public ObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
            : base(theStorageMetaData)
        {
            _StorageMetaData = theStorageMetaData;
        }




        //public override void CreateNewStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        //{

        //    RDBMSPersistenceRunTime.Commands.NewStorageInstanceCommand newStorageInstanceCommand = new RDBMSPersistenceRunTime.Commands.NewStorageInstanceCommand(StorageInstance as StorageInstanceRef);
        //    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(newStorageInstanceCommand);

        //}
        /// <exclude>Excluded</exclude>
        private OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection _OperativeObjectCollections;
        public override OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection OperativeObjectCollections
        {
            get
            {
                if (_OperativeObjectCollections == null)
                    _OperativeObjectCollections = new RDBMSPersistenceRunTime.MemoryInstanceCollection(this);
                return _OperativeObjectCollections;

            }
        }

        public override void CreateLinkCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleA, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleB, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationObject, OOAdvantech.PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {

            PersistenceLayerRunTime.Commands.LinkObjectsCommand linkObjectsCommand = null;

            string cmdIdentity = OOAdvantech.RDBMSPersistenceRunTime.Commands.LinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.LinkObjectsCommand;
            if (linkObjectsCommand == null)
            {

                linkObjectsCommand = new OOAdvantech.RDBMSPersistenceRunTime.Commands.LinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
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
        public override void CreateUnLinkCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleA, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleB, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationObject, OOAdvantech.PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkCommand unlinkObjectsCommand = null;
            if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage != this)
                throw new System.ArgumentException("There isn't object from this storage");

            string cmdIdentity = PersistenceLayerRunTime.Commands.UnLinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
            {

                if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
                {
                    unlinkObjectsCommand = new RDBMSPersistenceRunTime.Commands.UnLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                }
                else
                {
                    // unlinkObjectsCommand = new Commands.InterSorageUnLinkObjectsCommand(roleA, roleB, relationObject, linkInitiatorAssociationEnd, this);
                }
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(unlinkObjectsCommand);
            }

        }
        //public override void CreateUpdateReferentialIntegrity(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        //{
        //    //TODO:Μεγάλο πρόβλημα αν το transaction έχει πολλές μεταβολές δηλαδή πολλά commands
        //    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //    RDBMSPersistenceRunTime.Commands.UpdateReferentialIntegrity updateReferentialIntegrity = new RDBMSPersistenceRunTime.Commands.UpdateReferentialIntegrity(storageInstanceRef as StorageInstanceRef);
        //    transactionContext.EnlistCommand(updateReferentialIntegrity);

        //}
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies buildContainedObjectIndicies = new RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies(collection);
            transactionContext.EnlistCommand(buildContainedObjectIndicies);

        }
        //public override void CreateUnlinkAllObjectCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef sourceStorageInstance, OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd)
        //{
        //    if (Remoting.RemotingServices.IsOutOfProcess(associationEnd))
        //        associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(associationEnd.Identity) as DotNetMetaDataRepository.AssociationEnd;

        //    RDBMSPersistenceRunTime.Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new RDBMSPersistenceRunTime.Commands.UnlinkAllObjectCommand(sourceStorageInstance);
        //    //mUnlinkAllObjectCommand.DeletedStorageInstance = sourceStorageInstance;
        //    mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

        //    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //    transactionContext.EnlistCommand(mUnlinkAllObjectCommand);

        //}

        public override void CreateUnlinkAllObjectCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent deletedOutStorageInstanceRef, OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd, OOAdvantech.MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {
            throw new NotImplementedException();
        }
        public override void CreateUnlinkAllObjectCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent sourceStorageInstance, OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            if (Remoting.RemotingServices.IsOutOfProcess(associationEnd))
                associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(associationEnd.Identity) as DotNetMetaDataRepository.AssociationEnd;

            RDBMSPersistenceRunTime.Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new RDBMSPersistenceRunTime.Commands.UnlinkAllObjectCommand(sourceStorageInstance);
            //mUnlinkAllObjectCommand.DeletedStorageInstance = sourceStorageInstance;
            mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(mUnlinkAllObjectCommand);

        }


        public override void CreateDeleteStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstance, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption)
        {
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(new RDBMSPersistenceRunTime.Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption));
        }

        public override void CreateUpdateStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(new RDBMSPersistenceRunTime.Commands.UpdateStorageInstanceCommand(storageInstanceRef));
        }

        //public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance,PersistenceLayer.ObjectID  objectID)
        //{
        //    RDBMSMetaDataRepository.StorageCell storageCell = ((StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(memoryInstance.GetType())) as RDBMSMetaDataRepository.Class).ActiveStorageCell;
        //    return new StorageInstanceRef(memoryInstance, storageCell, this, objectID);
        //}
        //public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        //{
        //    return new StorageInstanceRef(memoryInstance, storageCell, this, objectID);
        //}

        public override void AbortChanges(OOAdvantech.PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            GetConnectionFor(theTransaction).AbordTransaction(theTransaction);
            // throw new NotImplementedException();
        }

        public override void CommitChanges(OOAdvantech.PersistenceLayerRunTime.TransactionContext theTransaction)
        {

            RDBMSPersistenceRunTime.Commands.LinkObjectsCommand.Indexer.Remove(theTransaction.Transaction.LocalTransactionUri);

            GetConnectionFor(theTransaction).CommitTransaction(theTransaction);
            //throw new NotImplementedException();
        }

        public override void PrepareForChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            GetConnectionFor(theTransaction).BeginTransaction(theTransaction);
        }
        public override void BeginChanges(OOAdvantech.PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            
            //throw new NotImplementedException();
        }

        public override void MakeChangesDurable(OOAdvantech.PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            //throw new NotImplementedException();
        }

        public override object GetObject(string persistentObjectUri)
        {
            throw new NotImplementedException();
        }

        public override string GetPersistentObjectUri(object obj)
        {
            return obj.GetHashCode().ToString();
        }



        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new NotImplementedException();
        }


        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            if (classifier is MetaDataRepository.Class)
            {
                MetaDataRepository.Class storageClass = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Class)) as MetaDataRepository.Class;
                return storageClass.StorageCellsOfThisType;
            }
            else if (classifier is MetaDataRepository.Interface)
            {
                MetaDataRepository.Interface storageInterface = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Interface)) as MetaDataRepository.Interface;
                return storageInterface.StorageCellsOfThisType;
            }
            throw new Exception("The method or operation is not implemented.");
        }

        ///// <MetaDataID>{d6561872-3d62-4aa8-87ef-0f33a4e5f3eb}</MetaDataID>
        //public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole)
        //{
        //    throw new NotImplementedException();
        //}

        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object ObjectID)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ff81b17b-fbad-404b-ac1a-2e7309eca3bb}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }



        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement)
        {
            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
                mStructureSet.Open(OQLStatement, null);
                //mStructureSet.GetData();
                stateTransition.Consistent = true;
                return mStructureSet;
            }
        }

        /// <MetaDataID>{8ea8da27-b77c-4bee-9a95-ecdb6fa3fd3f}</MetaDataID>
        internal OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                return (StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            }
        }

        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection GetConnectionFor(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            return (StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.GetConnectionFor(theTransaction);
        }


        protected override void TransferOnTemporaryTableRecords(IDataTable dataTable)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            DataBase dataBase = new DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");

            OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable(dataTable.TableName, dataTable, dataBase);
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            string commandText = null;
            string values = null;

            foreach (IDataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    continue;
                var parameter = command.CreateParameter();
                parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                parameter.ParameterName = "@" + column.ColumnName.ToUpper();
                command.Parameters.Add(parameter);
                if (commandText == null)
                {
                    commandText = @"INSERT INTO " + dataTable.TableName + @"(";
                    values = "VALUES (";
                }
                else
                {
                    commandText += ",";
                    values += ",";
                }
                commandText += column.ColumnName.ToUpper();
                values += "@" + column.ColumnName.ToUpper();
            }
            command.CommandText = (commandText + ")" + values + ")").ToUpper();
            foreach (IDataRow row in dataTable.Rows)
            {
                foreach (IDataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                        continue;
                    object value = row[column.ColumnName.ToUpper()];
                    //if (value is Guid)
                    //    value = ((Guid)value).ToByteArray();
                    //if (value is bool)
                    //    if ((bool)value)
                    //        value = "Y";
                    //    else
                    //        value = "N";
                    
                    if (value is System.DBNull)
                        value = null;

                    command.Parameters["@" + column.ColumnName.ToUpper()].Value = value;
                }
                command.ExecuteNonQuery();
            }
            dataTable.TemporaryTableTransfered = true;
        }
        protected override void TransferTableRecords(IDataTable dataTable, RDBMSMetaDataRepository.Table tableMetadata)
        {
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            string commandText = null;
            string values = null;


            foreach (IDataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    continue;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + column.ColumnName;
                parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                command.Parameters.Add(parameter);
                if (commandText == null)
                {
                    commandText = @"INSERT INTO " + dataTable.TableName + "(";
                    values = "VALUES (";
                }
                else
                {
                    commandText += ",";
                    values += ",";
                }

                commandText += column.ColumnName;
                values += "@" + column.ColumnName;

            }
            command.CommandText = commandText + ")" + values + ")";
            var OIDCommand = Connection.CreateCommand();
            OIDCommand.CommandText = "SELECT @@IDENTITY";

            foreach (IDataRow row in dataTable.Rows)
            {
                foreach (IDataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                        continue;

                    object value = row[column.ColumnName];
                    if (value is System.DBNull)
                        value = null;

                    command.Parameters["@" + column.ColumnName].Value = value;
                }

                command.ExecuteNonQuery();
                if (tableMetadata.TableCreator is RDBMSMetaDataRepository.StorageCell &&
                        (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).OIDIsDBGenerated &&
                        (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).MainTable == tableMetadata
                        && tableMetadata.ObjectIDColumns[0].IsIdentity)
                {

                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = row["StorageInstanceRef_" + dataTable.GetHashCode().ToString()] as PersistenceLayerRunTime.StorageInstanceRef;
                    object OID = OIDCommand.ExecuteScalar();
                    OID = Convert.ChangeType(OID, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType.Parts[0].Type);
                    RDBMSPersistenceRunTime.ObjectID objectID = new RDBMSPersistenceRunTime.ObjectID((storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType, new object[1] { OID }, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).SerialNumber);
                    storageInstanceRef.PersistentObjectID = objectID;
                }


            }

        }
        /// <MetaDataID>{7da93434-c1cb-4287-9a97-bbfb2a6777f3}</MetaDataID>
        protected override void UpdateTableRecords(IDataTable dataTable, List<string> OIDColumns)
        {
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            string commandText = null;
            //UPDATE    T_LiquidProduct
            //SET              TypeID =@TypeID 
            //WHERE     (T_LiquidProduct.ObjectID = 'SDSD')
            foreach (IDataColumn column in dataTable.Columns)
            {
                if (!OIDColumns.Contains(column.ColumnName))
                {

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@" + column.ColumnName;
                    command.Parameters.Add(parameter);
                    if (commandText == null)
                        commandText = @"UPDATE  " + dataTable.TableName + "\nSET ";
                    else
                        commandText += ",";

                    commandText += column.ColumnName + " = @" + column.ColumnName;
                }
                else
                {

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@" + column.ColumnName;
                    command.Parameters.Add(parameter);

                }
            }
            string filter = null;
            foreach (string OIDColumn in OIDColumns)
            {
                if (filter == null)
                    filter = "\nWHERE ";
                else
                    filter += " AND ";

                filter += OIDColumn + " = @" + OIDColumn;
            }
            commandText += filter;
            command.CommandText = commandText;


            foreach (IDataRow row in dataTable.Rows)
            {
                foreach (IDataColumn column in dataTable.Columns)
                {
                    object value = row[column.ColumnName];
                    if (value is System.DBNull)
                        value = null;
                    command.Parameters["@" + column.ColumnName].Value = value;
                }
                command.ExecuteNonQuery();
            }

        }
        protected override void DeleteTableRecords(IDataTable dataTable)
        {
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            string filter = null;
            foreach (IDataColumn OIDColumn in dataTable.Columns)
            {
                if (filter == null)
                    filter = "\nWHERE ";
                else
                    filter += " AND ";

                filter += OIDColumn.ColumnName + " = @" + OIDColumn.ColumnName;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + OIDColumn.ColumnName;
                command.Parameters.Add(parameter);

            }
            command.CommandText = "DELETE FROM " + dataTable.TableName + filter;
            foreach (IDataRow row in dataTable.Rows)
            {
                foreach (IDataColumn column in dataTable.Columns)
                    command.Parameters["@" + column.ColumnName].Value = row[column.ColumnName];
                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        protected override void UpdateOperativeObjects()
        {
            throw new NotImplementedException();
        }

        //public static void Test()
        //{

        //    SQLiteDataBaseConnection SQLiteDataBaseConnection = new SQLiteDataBaseConnection(@"c:\Test.sqlite");

        //    SQLiteDataBaseConnection.Open();
        //    try
        //    {
        //        IDataBaseCommand command = SQLiteDataBaseConnection.CreateCommand();
        //        command.CommandText = "BEGIN TRANSACTION;";
        //        command.ExecuteNonQuery();

        //        for (int i = 1; i < 1500; i++)
        //        {
        //            command = SQLiteDataBaseConnection.CreateCommand();
        //            command.CommandText = string.Format("INSERT INTO [ObjectBLOBS] ([ID],[ClassBLOBSID],[ObjectData]) VALUES({0},1,'aas');", i);
        //            command.ExecuteNonQuery();
        //        }

        //        command = SQLiteDataBaseConnection.CreateCommand();
        //        command.CommandText = "COMMIT;";
        //        command.ExecuteNonQuery();
        //    }
        //    catch (Exception error)
        //    {

        //    }
        //    SQLiteDataBaseConnection.Close();

        //}
    }

}
