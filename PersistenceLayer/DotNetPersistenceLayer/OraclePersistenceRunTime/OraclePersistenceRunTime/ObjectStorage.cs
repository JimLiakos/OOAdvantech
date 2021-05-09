using System;
using System.Data;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
using Commands = OOAdvantech.RDBMSPersistenceRunTime.Commands;
using System.Data.Common;
using Oracle.DataAccess.Client;
using OOAdvantech.Transactions;
namespace OOAdvantech.OraclePersistenceRunTime
{




    /// <MetaDataID>{30648d7b-81dc-4b18-9443-b75286253978}</MetaDataID>
    public class ObjectStorage : RDBMSPersistenceRunTime.ObjectStorage
    {
        /// <MetaDataID>{1c403c45-b223-4de6-9f1f-a0b2973b86cd}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }


        /// <MetaDataID>{0293615d-f61a-4e6c-84b4-c248e4a150d5}</MetaDataID>
        public ObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
            : base(theStorageMetaData)
        {

        }

        /// <MetaDataID>{db44b9fd-5f7e-4980-8b98-4f97f559e4c0}</MetaDataID>
        protected override void TransferOnTemporaryTableRecords(MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            DataObjects.DataBase dataBase = new DataObjects.DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");
            
            OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable(dataTable.TableName, dataTable, dataBase);

            OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            string commandText = null;
            string values = null;

            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    continue;
                OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseParameter parameter = command.CreateParameter();
                parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                parameter.ParameterName = ":" + column.ColumnName.ToUpper();
                command.Parameters.Add(parameter);
                if (commandText == null)
                {
                    commandText = @"INSERT INTO """ + StorageMetaData.StorageName + @""".""" + dataTable.TableName + @"""(";
                    values = "VALUES (";
                }
                else
                {
                    commandText += ",";
                    values += ",";
                }
                commandText += column.ColumnName.ToUpper();
                values += ":" + column.ColumnName.ToUpper();
            }
            command.CommandText = (commandText + ")" + values + ")").ToUpper();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                        continue;
                    object value = row[column.ColumnName.ToUpper()];
                    if (value is Guid)
                        value = ((Guid)value).ToByteArray();
                    if (value is bool)
                        if ((bool)value)
                             value = "Y";
                        else
                            value = "N";
                    command.Parameters[":" + column.ColumnName.ToUpper()].Value = value;
                }
                command.ExecuteNonQuery();
            }
            dataTable.TemporaryTableTransfered = true; 
            //command.CommandText = @"select count(*) from " + dataTable.TableName.ToUpper();
            //object ret=command.ExecuteScalar();
            //System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connection);
            //sqlBulkCopy.DestinationTableName = "#" + dataTable.TableName;
            //sqlBulkCopy.WriteToServer(dataTable);
        }

        /// <MetaDataID>{d22a3e34-d8f3-4afc-9c0d-2cbd298a4f9b}</MetaDataID>
        protected override void TransferTableRecords(IDataTable dataTable, OOAdvantech.RDBMSMetaDataRepository.Table tableMetadata)
        {

            OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            string commandText = null;
            string values = null;


            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    continue;

                var parameter = command.CreateParameter();
                parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                parameter.ParameterName = ":" + column.ColumnName.ToUpper();
                command.Parameters.Add(parameter);
                if (commandText == null)
                {
                    commandText = @"INSERT INTO """ + StorageMetaData.StorageName + @""".""" + dataTable.TableName + @"""(";
                    values = "VALUES (";
                }
                else
                {
                    commandText += ",";
                    values += ",";
                }

                commandText += column.ColumnName.ToUpper();
                values += ":" + column.ColumnName.ToUpper();

            }
            OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand getIDCommand = null;
            if (tableMetadata.TableCreator is RDBMSMetaDataRepository.StorageCell &&
                   (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).OIDIsDBGenerated &&
                   (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).MainTable == tableMetadata
                   && tableMetadata.ObjectIDColumns[0].IsIdentity)
            {
                string sequenceName = ((StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSQLScriptGenarator as OracleRDBMSSchema).GetAssociatedSequence((tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).MainTable.Name.ToUpper());
                getIDCommand = Connection.CreateCommand();
                getIDCommand.CommandText = string.Format("select {0}.CURRVAL  FROM DUAL", sequenceName);

            }

            command.CommandText = (commandText + ")" + values + ")").ToUpper();



            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                        continue;
                    object value = row[column.ColumnName.ToUpper()];
                    if (value is Guid)
                        value = ((Guid)value).ToByteArray();
                    if (value is bool)
                        if ((bool)value)
                            value = "Y";
                        else
                            value = "N";
                    command.Parameters[":" + column.ColumnName.ToUpper()].Value = value;
                }

                command.ExecuteNonQuery();

                if (getIDCommand != null)
                {
                    object OID = getIDCommand.ExecuteScalar();
                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = row["StorageInstanceRef_" + dataTable.GetHashCode().ToString()] as PersistenceLayerRunTime.StorageInstanceRef;
                    OID = Convert.ChangeType(OID, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType.Parts[0].Type);
                    RDBMSPersistenceRunTime.ObjectID objectID = new ObjectID((storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType, new object[1] { OID }, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).SerialNumber);

                    storageInstanceRef.PersistentObjectID = objectID;
                }


            }

        }

      

        /// <MetaDataID>{2d955042-5888-47cc-b14f-2a6106ef330e}</MetaDataID>
        protected override void UpdateTableRecords(IDataTable dataTable, System.Collections.Generic.List<string> OIDColumns)
        {
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();


            //command.CommandText = "SET foreign_key_checks = 0";
            //command.ExecuteNonQuery();

            try
            {
                command = Connection.CreateCommand();
                string commandText = null;
                //UPDATE    T_LiquidProduct
                //SET              TypeID =@TypeID 
                //WHERE     (T_LiquidProduct.ObjectID = 'SDSD')
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (!OIDColumns.Contains(column.ColumnName))
                    {

                        var parameter = command.CreateParameter();
                        parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                        parameter.ParameterName = ":" + column.ColumnName.ToUpper();
                        command.Parameters.Add(parameter);
                        if (commandText == null)
                            commandText = @"UPDATE  """ + StorageMetaData.StorageName.ToUpper() + @""".""" + dataTable.TableName.ToUpper() + @"""  SET ";
                        else
                            commandText += ",";

                        commandText += @"""" + column.ColumnName.ToUpper() + @"""" + " = :" + column.ColumnName.ToUpper();
                    }
                }
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (OIDColumns.Contains(column.ColumnName))
                    {
                        var parameter = command.CreateParameter();
                        parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                        parameter.ParameterName = ":" + column.ColumnName.ToUpper();
                        command.Parameters.Add(parameter);
                    }
                }
                string filter = null;
                foreach (string OIDColumn in OIDColumns)
                {
                    if (filter == null)
                        filter = " WHERE ";
                    else
                        filter += " AND ";

                    filter += @"""" + OIDColumn.ToUpper() + @"""" + " = :" + OIDColumn.ToUpper();
                }
                commandText += filter;
                command.CommandText = commandText;
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                    {
                        object value = row[column.ColumnName];
                        if (value is Guid)
                            value = ((Guid)value).ToByteArray();
                        if (value is bool)
                            if ((bool)value)
                                value = "Y";
                            else
                                value = "N";
                        command.Parameters[":" + column.ColumnName].Value = value;
                    }
                    command.ExecuteNonQuery();
                }

            }
            finally
            {
                //command = Connection.CreateCommand();
                //command.CommandText = "SET foreign_key_checks = 1";
                //command.ExecuteNonQuery();

            }
        }

        /// <MetaDataID>{c8ded30f-2136-4e88-b482-b90e30677b3c}</MetaDataID>
        protected override void DeleteTableRecords(IDataTable dataTable)
        {
            var command = Connection.CreateCommand();
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            //command.CommandText = "SET foreign_key_checks = 1";
            //command.ExecuteNonQuery();


            try
            {
                //command = Connection.CreateCommand();
                string filter = null;
                foreach (System.Data.DataColumn OIDColumn in dataTable.Columns)
                {
              
                    if (filter == null)
                        filter = "\nWHERE ";
                    else
                        filter += " AND ";

                    filter += OIDColumn.ColumnName + " = @" + OIDColumn.ColumnName;
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = ":" + OIDColumn.ColumnName;
                    parameter.DbType = TypeDictionary.ToDbType(OIDColumn.DataType);
                    command.Parameters.Add(parameter);

                }
                command.CommandText = @"DELETE FROM """ + StorageMetaData.StorageName + @""".""" + dataTable.TableName + @""" " + filter;
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                        command.Parameters[":" + column.ColumnName].Value = row[column.ColumnName];
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            finally
            {
                //command = Connection.CreateCommand();
                //command.CommandText = "SET foreign_key_checks = 0";
                //command.ExecuteNonQuery();

            }


        }

        /// <MetaDataID>{aa775ae5-7413-4365-af36-71231d307bd0}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ///// <MetaDataID>{067b5510-bc10-491f-8c65-0ca31479ab1a}</MetaDataID>
        //public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, MetaDataRepository.Roles storageCellsRole)
        //{
        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {
        //        association = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(association) as OOAdvantech.MetaDataRepository.Association;
        //        Collections.Generic.Set<MetaDataRepository.StorageCell> StorageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
        //        foreach (MetaDataRepository.StorageCell storageCell in relatedStorageCells)
        //        {
        //            foreach (MetaDataRepository.StorageCellsLink storageCellsLink in association.StorageCellsLinks)
        //            {
        //                if (storageCellsRole == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleAStorageCell == storageCell)
        //                    StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
        //                else
        //                {
        //                    if (storageCellsLink.RoleBStorageCell == storageCell)
        //                        StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
        //                }
        //            }
        //        }
        //        return StorageCells;
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //}


        /// <MetaDataID>{08786a41-0a23-42a7-962b-9666140a93f0}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object objectID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{dd65a6f8-9746-49a3-8926-35fedcec5b4f}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{c00777aa-058d-4e3d-915e-3154b1d89768}</MetaDataID>
        string OleDBConnectionString = null;

        /// <MetaDataID>{968b3500-5104-4637-97af-a97b675f9f6e}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;
            }
        }


        /// <MetaDataID>{9727a90d-b058-4711-8a1a-085569cf50bf}</MetaDataID>
        public RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).TypeDictionary;
            }
        }



        /// <MetaDataID>{ab29308a-8687-44e5-9212-39804a1f3be8}</MetaDataID>
        public OOAdvantech.Collections.Map DatabaseConectios = new OOAdvantech.Collections.Map();
        /// <MetaDataID>{99b25fae-2038-4efb-bb7f-99ff986b861b}</MetaDataID>
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        /// <MetaDataID>{baf3c519-0d62-4b0d-9518-4cad855002cd}</MetaDataID>
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        public override void PrepareForChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }


        /// <MetaDataID>{7f532a68-93b9-439d-a0ee-6fddb065cdb9}</MetaDataID>
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }


        /// <MetaDataID>{336278c5-9aad-497f-872b-36864d980d30}</MetaDataID>
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }





        ///// <MetaDataID>{7105dae4-25d8-400e-bc0f-34357468be20}</MetaDataID>
        //public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        //{
        //    if (classifier is MetaDataRepository.Class)
        //    {
        //        MetaDataRepository.Class storageClass = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Class)) as MetaDataRepository.Class;
        //        return storageClass.StorageCellsOfThisType;
        //    }
        //    else if (classifier is MetaDataRepository.Interface)
        //    {
        //        MetaDataRepository.Interface storageInterface = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Interface)) as MetaDataRepository.Interface;
        //        return storageInterface.StorageCellsOfThisType;
        //    }
        //    throw new Exception("The method or operation is not implemented.");
        //}


        //public RDBMSMetaDataRepository.StorageCellReference GetOutStorageObjColl(PersistenceLayerRunTime.StorageInstanceRef OutStorageInstanceRef)
        //{

        //    if (OutStorageInstanceRef.GetType() == typeof(StorageInstanceRef))
        //    {
        //        RDBMSMetaDataRepository.StorageCellReference storageCell = ((RDBMSPersistenceRunTime.Storage)_StorageMetaData).GetEquivalentMetaObject("ref_" + (OutStorageInstanceRef as StorageInstanceRef).StorageInstanceSetIdentity, typeof(RDBMSMetaDataRepository.StorageCellReference)) as RDBMSMetaDataRepository.StorageCellReference;
        //        if (storageCell != null)
        //            return storageCell;
        //        RDBMSMetaDataRepository.Class theClass = ((RDBMSPersistenceRunTime.Storage)_StorageMetaData).GetEquivalentMetaObject(OutStorageInstanceRef.Class) as RDBMSMetaDataRepository.Class;
        //        return theClass.GetStorageCellReference((OutStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
        //    }
        //    return null; //Error prone
        //}



     

    


        //public static void StoreMetatData(System.IO.Stream metaDataStream, byte[] pointer, System.Data.SqlClient.SqlConnection Conn)
        //{
        //    int bufferLen = 4096;  // The size of the "chunks" of the image.

        //    System.Data.SqlClient.SqlCommand appendToMetatData = new System.Data.SqlClient.SqlCommand("UPDATETEXT MetaDataTable.MetaData @Pointer @Offset NULL @Bytes", Conn);

        //    System.Data.SqlClient.SqlParameter ptrParm = appendToMetatData.Parameters.Add("@Pointer", System.Data.SqlDbType.Binary, 16);
        //    ptrParm.Value = pointer;
        //    System.Data.SqlClient.SqlParameter photoParm = appendToMetatData.Parameters.Add("@Bytes", System.Data.SqlDbType.Image, bufferLen);
        //    System.Data.SqlClient.SqlParameter offsetParm = appendToMetatData.Parameters.Add("@Offset", System.Data.SqlDbType.Int);
        //    offsetParm.Value = 0;

        //    //''''''''''''''''''''''''''''''''''
        //    // Read the image in and write it to the database 128 (bufferLen) bytes at a time.
        //    // Tune bufferLen for best performance. Larger values write faster, but
        //    // use more system resources.

        //    //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    //System.IO.BinaryReader br = new System.IO.BinaryReader(metaDataStream);


        //    byte[] buffer = new byte[bufferLen];
        //    //br.ReadBytes(bufferLen);
        //    metaDataStream.Position = 0;
        //    int readedLength = metaDataStream.Read(buffer, 0, bufferLen);
        //    int offset_ctr = 0;
        //    while (readedLength > 0)
        //    {
        //        if (readedLength == bufferLen)
        //            photoParm.Value = buffer;
        //        else
        //        {
        //            byte[] tmpBuffer = new byte[readedLength];
        //            for (int i = 0; i != readedLength; i++)
        //                tmpBuffer[i] = buffer[i];
        //            photoParm.Value = tmpBuffer;
        //        }
        //        appendToMetatData.ExecuteNonQuery();
        //        offset_ctr += bufferLen;
        //        offsetParm.Value = offset_ctr;
        //        readedLength = metaDataStream.Read(buffer, 0, bufferLen);
        //    }
        //    //	br.Close();
        //}


        ///// <MetaDataID>{00fc08e4-ef46-44aa-af31-94965b13e2f4}</MetaDataID>
        //void SetObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
        //{

        //    if (_StorageMetaData != null)
        //        throw new System.Exception("StorageMetaData already set");
        //    _StorageMetaData = theStorageMetaData;

        //    //_StorageMetaData.ObjectStorageChanged+=new PersistenceLayer.ObjectStorageChangedEventHandler(OnObjectStorageChanged);

        //}


        //internal int ActiveObjectsCount
        //{
        //    get
        //    {
        //        int activeObjectsCount = 0;
        //        foreach (RDBMSPersistenceRunTime.ClassMemoryInstanceCollection classActiveObjects in OperativeObjectCollections)
        //            activeObjectsCount += classActiveObjects.StorageInstanceRefs.Count;
        //        return activeObjectsCount;
        //    }

        //}
        // Returns a column name that is of length totalSize.

        ///// <MetaDataID>{1c7dcda3-1f3b-475f-b33f-87c296700b26}</MetaDataID>
        //private string GetColumnName(long totalSize, long index)
        //{
        //    string si = index.ToString();
        //    string pad;
        //    if (si.Length >= (totalSize - 1)) return "c" + si;
        //    pad = new string('c', (int)totalSize - si.Length);
        //    return pad + si;
        //}
        //// Returns a column value of specified columnType and dataSize.
        //// Note dataSize only applies to string and byte[] data types.
        //// For all numeric data types, I return their appropriate maximum values.

        ///// <MetaDataID>{02a8f73d-875e-414e-bae0-6271e1ff09ff}</MetaDataID>
        //private object GetColumnValue(Type columnType, long dataSize)
        //{
        //    if (columnType == typeof(Boolean)) return false;
        //    if (columnType == typeof(Byte)) return Byte.MaxValue;
        //    if (columnType == typeof(Char)) return 'x';
        //    if (columnType == typeof(DateTime)) return DateTime.Now;
        //    if (columnType == typeof(Decimal)) return Decimal.MaxValue;
        //    if (columnType == typeof(Double)) return Double.MaxValue;
        //    if (columnType == typeof(Int16)) return Int16.MaxValue;
        //    if (columnType == typeof(Int32)) return Int32.MaxValue;
        //    if (columnType == typeof(Int64)) return Int64.MaxValue;
        //    if (columnType == typeof(Single)) return Single.MaxValue;
        //    if (columnType == typeof(String)) return new string('x', (int)dataSize);
        //    if (columnType == typeof(TimeSpan)) return new TimeSpan(1, 1, 1, 1, 1);
        //    if (columnType == typeof(Guid)) return new Guid("{3381E62F-20C6-462c-87DF-CC11F37C0EC6}");
        //    if (columnType == typeof(Byte[]))
        //    {
        //        byte[] b = new byte[dataSize];
        //        for (long i = 0; i < b.Length; i++) b[i] = (byte)(i % (((int)byte.MaxValue) + 1));
        //        return b;
        //    }
        //    return null;
        //}

        ///// <MetaDataID>{a035d58a-ca27-4d77-9c65-8f044729eaee}</MetaDataID>
        //private DataSet GenerateDataSet(long columnCount, long rowCount, long cnameSize, long dataSize, bool attributed, Type columnType)
        //{
        //    DataSet ds = new DataSet("d");
        //    DataTable t = new DataTable("t");
        //    long r, c;
        //    object[] values = null;

        //    // Create row values array.
        //    values = new object[columnCount];
        //    for (c = 1; c <= columnCount; c++)
        //    {
        //        t.Columns.Add(GetColumnName(cnameSize, c), columnType);
        //        if (attributed) t.Columns[(int)(c - 1)].ColumnMapping = MappingType.Attribute;
        //        values[c - 1] = GetColumnValue(columnType, dataSize);
        //    }

        //    for (r = 1; r <= rowCount; r++)
        //    {
        //        t.Rows.Add(values);
        //    }

        //    t.AcceptChanges();
        //    ds.Tables.Add(t);
        //    ds.AcceptChanges();

        //    return ds;

        //}

        ///// <MetaDataID>{bd2b9a26-915f-4d2b-bc20-8010abcfaa1c}</MetaDataID>
        //static byte[] Buffer = new byte[4096];


        ///// <MetaDataID>{937c439a-4cd0-4f2e-a850-86d1531df2b9}</MetaDataID>
        //public byte[] GetRelatedRows(Guid[] objectIDs)
        //{
        //    System.Data.SqlClient.SqlConnection mOleDbConnection = new System.Data.SqlClient.SqlConnection(Connection.ConnectionString);

        //    //   string SQLQuery = "SELECT  T_OrderDetail.ObjectID,T_OrderDetail.ObjectID as obj2,T_OrderDetail.ObjectID as obj3,T_OrderDetail.ObjectID as obj4 FROM T_OrderDetail";
        //    string SQLQuery = "SELECT  T_OrderDetail.ObjectID FROM T_OrderDetail";
        //    SQLQuery = "SELECT     ObjectID, ReferenceCount, TypeID, Name, OrderDetails_ObjectIDB FROM         T_OrderDetail";


        //    //TOP 50 PERCENT

        //    System.Data.DataSet MainDataReader = new System.Data.DataSet();
        //    mOleDbConnection.Open();
        //    System.Data.SqlClient.SqlCommand DataCommand = mOleDbConnection.CreateCommand();//((StorageSession)SourceStorageSession).OleDbConnection.CreateCommand();
        //    DataCommand.CommandText = SQLQuery;
        //    System.Data.SqlClient.SqlDataAdapter myAdapter = new System.Data.SqlClient.SqlDataAdapter();
        //    myAdapter.SelectCommand = DataCommand;
        //    myAdapter.Fill(MainDataReader);
        //    mOleDbConnection.Close();
        //    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        //    int offset = 0;
        //    foreach (DataRow dataRow in MainDataReader.Tables[0].Rows)
        //    {
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize((Guid)dataRow[0], Buffer, offset, ref offset, true);
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((int)dataRow[1]), Buffer, offset, ref offset, true);
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((int)dataRow[2]), Buffer, offset, ref offset, true);
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((string)dataRow[3]), Buffer, offset, ref offset);
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((Guid)dataRow[4]), Buffer, offset, ref offset, true);

        //        memoryStream.Write(Buffer, 0, offset);
        //        offset = 0;
        //    }
        //    memoryStream.Position = 0;
        //    byte[] buffer = memoryStream.ToArray();
        //    memoryStream.Close();
        //    return buffer;

        //    System.Data.DataTable table = new System.Data.DataTable();


        //    System.TimeSpan NewTimeSpan;
        //    System.TimeSpan ComiteTimeSpan;
        //    System.TimeSpan ComiteTimeSpanB;
        //    System.DateTime before;
        //    System.DateTime After;
        //    before = System.DateTime.Now;

        //    table.Columns.Add("ObjectID", typeof(System.Guid));
        //    foreach (System.Guid objectID in objectIDs)
        //    {
        //        table.Rows.Add(table.NewRow()[0] = objectID);
        //    }
        //    MainDataReader.Tables.Add(table);

        //    System.Data.DataRelation relation = new System.Data.DataRelation("test", table.Columns[0], MainDataReader.Tables[0].Columns["OrderDetails_ObjectIDB"]);
        //    System.Data.DataTable retTable = new DataTable();
        //    foreach (DataColumn col in MainDataReader.Tables[0].Columns)
        //    {
        //        retTable.Columns.Add(col.ColumnName, col.DataType);

        //    }
        //    //System.Data.DataTable retTable= MainDataReader.Tables[0].Clone();
        //    MainDataReader.Relations.Add(relation);
        //    foreach (System.Data.DataRow row in MainDataReader.Tables[0].Rows)
        //    {
        //        DataRow newRow = retTable.NewRow();
        //        int i = 0;
        //        foreach (object obj in row.ItemArray)
        //        {
        //            if (obj is DBNull)
        //                continue;
        //            else
        //                newRow[i] = obj;
        //            i++;
        //        }
        //        retTable.Rows.Add(newRow);


        //        //if (row.GetParentRow(relation) != null)
        //        //    retTable.Rows.Add(row.ItemArray);

        //    }
        //    After = System.DateTime.Now;
        //    ComiteTimeSpan = After - before;
        //    System.Diagnostics.Debug.WriteLine("1 " + ComiteTimeSpan.ToString());
        //    System.Data.DataSet dataSet = new DataSet();
        //    dataSet.Tables.Add(retTable);

        //    DataSet ds = null;
        //    PersistenceLayerRunTime.DataSetSurrogate dss = null;
        //    ds = GenerateDataSet(5, 66000, 4, 50, false, typeof(Guid));
        //    bool k = true;

        //    if (k)
        //        dataSet = ds;

        //    return null;



        //}


        ///// <MetaDataID>{56b286a2-f178-4b43-89bb-ea8372bf2841}</MetaDataID>
        //public void GetData(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, RDBMSMetaDataRepository.StorageCell storageCell)
        //{
        //    int ert = 0;

        //}
    }


}

