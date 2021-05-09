    using System;
using System.Data;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
using Commands = OOAdvantech.RDBMSPersistenceRunTime.Commands;
using System.Data.Common;
namespace OOAdvantech.MSSQLPersistenceRunTime
{



    /// <MetaDataID>{0E9B3734-7335-40D1-B480-1E6BE0F35E94}</MetaDataID>
    public class ObjectStorage : RDBMSPersistenceRunTime.ObjectStorage
    {
        
        /// <MetaDataID>{824449c0-1004-4100-9dc5-17596045db98}</MetaDataID>
        public ObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
            : base(theStorageMetaData)
        {
        }
      
        /// <MetaDataID>{2d845175-cd09-4e0d-a67a-310a7b132252}</MetaDataID>
        protected override void UpdateTableRecords(IDataTable dataTable, System.Collections.Generic.List<string> OIDColumns)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            DataObjects.DataBase dataBase = new DataObjects.DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");
            System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connection.NativeConnaction as System.Data.SqlClient.SqlConnection);
            sqlBulkCopy.DestinationTableName = "#TmpUpdate_" + dataTable.TableName;
            //dataTable.TableName = "Tmp_" + dataTable.TableName;// new System.Data.DataTable("Tmp_" + dataTable.TableName);

            OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable("TmpUpdate_" + dataTable.TableName, dataTable, dataBase);
            sqlBulkCopy.WriteToServer(dataTable as System.Data.DataTable);

            string columnsString = null;
            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                if (OIDColumns.Contains(column.ColumnName))
                    continue;

                if (columnsString != null)
                    columnsString += ", ";
                columnsString += dataTable.TableName + ".[" + column.ColumnName + "] = #TmpUpdate_" + dataTable.TableName + ".[" +  column.ColumnName+"]";
            }
            string commandText = @"UPDATE " + dataTable.TableName + "\n";
            commandText += "SET " + columnsString + "\n";
            commandText += "FROM  " + dataTable.TableName + " INNER JOIN #TmpUpdate_" + dataTable.TableName + " ON ";
            string innerJoinCriterion = null;
            foreach (string OIDColumnName in OIDColumns)
            {
                if (innerJoinCriterion != null)
                    innerJoinCriterion += " AND ";
                innerJoinCriterion += dataTable.TableName + ".[" + OIDColumnName + "] = #TmpUpdate_" + dataTable.TableName + ".[" + OIDColumnName+"]";
            }
            commandText += innerJoinCriterion;
            commandText += "\n DROP TABLE #TmpUpdate_" + dataTable.TableName;
            var command = Connection.CreateCommand();
            //command.CommandText = "Select ID from " + dataTable.TableName;
            //object obj = command.ExecuteScalar();
            command.CommandText = commandText;
            command.ExecuteNonQuery();





            //UPDATE    T_OrderDetail
            //SET              ReferenceCount = T_OrderDetailB.ReferenceCount, TypeID = T_OrderDetailB.TypeID, Quantity_Amount = T_OrderDetailB.Quantity_Amount, 
            //          Quantity_QuantityUnit_ObjectIDA = T_OrderDetailB.Quantity_QuantityUnit_ObjectIDA, Name = T_OrderDetail.Name, 
            //          ItemPrice_ObjectIDA = T_OrderDetailB.ItemPrice_ObjectIDA, OrderDetails_ObjectIDB = T_OrderDetailB.ItemPrice_ObjectIDA, 
            //          OrderDetails_IndexerB = T_OrderDetailB.OrderDetails_IndexerB
            //FROM         T_OrderDetail INNER JOIN
            //          T_OrderDetailB ON T_OrderDetail.ObjectID = T_OrderDetailB.ObjectID
            //            throw new NotImplementedException();
        }

        /// <MetaDataID>{0359c9b3-63f6-4d26-9849-5eda3fea54c8}</MetaDataID>
        protected override void TransferOnTemporaryTableRecords(IDataTable dataTable)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            DataObjects.DataBase dataBase = new DataObjects.DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");
            System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connection.NativeConnaction as System.Data.SqlClient.SqlConnection);
            OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable( dataTable.TableName, dataTable, dataBase);
            sqlBulkCopy.DestinationTableName = "#"+ dataTable.TableName;
            sqlBulkCopy.WriteToServer(dataTable as System.Data.DataTable);
            dataTable.TemporaryTableTransfered = true;
        }

        /// <MetaDataID>{22667484-12f2-425f-9102-f59592fe0c01}</MetaDataID>
        protected override void TransferTableRecords(IDataTable dataTable ,RDBMSMetaDataRepository.Table tableMetadata)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();

            if (tableMetadata.TableCreator is RDBMSMetaDataRepository.StorageCell && 
                (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).OIDIsDBGenerated&&
                (tableMetadata.TableCreator as RDBMSMetaDataRepository.StorageCell).MainTable==tableMetadata
                &&tableMetadata.ObjectIDColumns[0].IsIdentity )
            {

                var command = Connection.CreateCommand();
                string commandText = null;
                string values = null;


                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == tableMetadata.ObjectIDColumns[0].Name)
                        continue;
                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                        continue;
                    var parameter = command.CreateParameter();
                    parameter.DbType = TypeDictionary.ToDbType(column.DataType);
                    parameter.ParameterName = "@" + column.ColumnName.ToUpper();
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

                    commandText += column.ColumnName.ToUpper();
                    values += "@" + column.ColumnName.ToUpper();

                }
                //DbParameter oidParameter = command.CreateParameter();
                //oidParameter.ParameterName = "@pOID";
                //oidParameter.Direction = ParameterDirection.ReturnValue;
                //command.Parameters.Add(oidParameter);
                command.CommandText = (commandText + ")" + values + ")")+"\r\n select SCOPE_IDENTITY()";
                
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                    {

                        if (column.ColumnName == tableMetadata.ObjectIDColumns[0].Name)
                            continue;
                        if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                            continue;
                        object value = row[column.ColumnName.ToUpper()];
                        if (value == null)
                            value = DBNull.Value;
                        command.Parameters["@" + column.ColumnName.ToUpper()].Value = value;
                    }
                    //command.Parameters["@pOID"].Value = 0;
                   object OID= command.ExecuteScalar();
                   
                   PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = row["StorageInstanceRef_" + dataTable.GetHashCode().ToString()] as PersistenceLayerRunTime.StorageInstanceRef;
                   OID = Convert.ChangeType(OID, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType.Parts[0].Type);
                   RDBMSPersistenceRunTime.ObjectID objectID = new ObjectID((storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType, new object[1] { OID }, (storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).SerialNumber);
                   
                   storageInstanceRef.PersistentObjectID = objectID;
                }

            }
            else
            {
                if(dataTable.Columns.Contains("StorageInstanceRef_" + dataTable.GetHashCode().ToString()))
                    dataTable.Columns.Remove("StorageInstanceRef_" + dataTable.GetHashCode().ToString());
                DataObjects.DataBase dataBase = new DataObjects.DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");

                System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connection.NativeConnaction as System.Data.SqlClient.SqlConnection);
                sqlBulkCopy.DestinationTableName = "#TmpInsert_" + dataTable.TableName;
                //dataTable.TableName = "Tmp_" + dataTable.TableName;// new System.Data.DataTable("Tmp_" + dataTable.TableName);
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    System.Data.SqlClient.SqlBulkCopyColumnMapping columnMap =
                 new System.Data.SqlClient.SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName);
                    sqlBulkCopy.ColumnMappings.Add(columnMap);


                }
                OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable("TmpInsert_" + dataTable.TableName, dataTable, dataBase);
                sqlBulkCopy.WriteToServer(dataTable as System.Data.DataTable);

                string columnsString = null;
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (columnsString != null)
                        columnsString += ", ";
                    columnsString +="["+ column.ColumnName+"]";
                }
                string commandText = @"INSERT INTO " + dataTable.TableName + "(" + columnsString + ")\n";
                commandText += "SELECT   " + columnsString + "\nFROM  #TmpInsert_" + dataTable.TableName;
                commandText += "\n DROP TABLE #TmpInsert_" + dataTable.TableName;
                var command = Connection.CreateCommand();
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }





            //INSERT INTO T_OrderDetail
            //          (ObjectID, ReferenceCount, TypeID, Quantity_Amount, Quantity_QuantityUnit_ObjectIDA, Name, ItemPrice_ObjectIDA, OrderDetails_ObjectIDB, 
            //          OrderDetails_IndexerB)
            //SELECT     ObjectID, ReferenceCount, TypeID, Quantity_Amount, Quantity_QuantityUnit_ObjectIDA, Name, ItemPrice_ObjectIDA, OrderDetails_ObjectIDB, 
            //          OrderDetails_IndexerB
            //FROM         T_OrderDetailB

            //dropTemTables += "DROP TABLE #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName;



            //if (criterion.FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
            //{

            //    RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
            //    int length = attribute.GetPropertyValue<int>("Persistent", "SizeOf");
            //    foreach (MetaDataRepository.AttributeRealization attributeRealization in attribute.AttributeRealizations)
            //    {
            //        int attributeRealizationLength = attributeRealization.GetPropertyValue<int>("Persistent", "SizeOf");
            //        if (attributeRealizationLength > length)
            //            length = attributeRealizationLength;
            //    }
            //    table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(attribute.Name, attribute.Type, length, true, false, 0));
            //    foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
            //        dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));


            //    foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
            //    {
            //        System.Data.DataRow dataRow = dataTable.NewRow();
            //        dataRow[attribute.Name] = value;
            //        dataTable.Rows.Add(dataRow);
            //    }

            //}
            //else
            //{


            //    foreach (RDBMSMetaDataRepository.Column column in ((LoadFromStorage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
            //        table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(column));

            //    foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
            //        dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));


            //    foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
            //    {
            //        ObjectID objectID = null;
            //        try
            //        {
            //            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(value) as StorageInstanceRef;
            //            if (storageInstanceRef == null)
            //                objectID = new ObjectID(System.Guid.NewGuid(), 0);
            //            else
            //                objectID = storageInstanceRef.ObjectID as ObjectID;
            //        }
            //        catch
            //        {
            //            objectID = new ObjectID(System.Guid.NewGuid(), 0);
            //        }
            //        StorageInstanceRef.GetStorageInstanceRef(value);
            //        System.Data.DataRow dataRow = dataTable.NewRow();
            //        foreach (RDBMSMetaDataRepository.IdentityColumn column in ((LoadFromStorage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
            //            dataRow[column.Name] = objectID.GetMemberValue(column.ColumnType);
            //        dataTable.Rows.Add(dataRow);
            //    }

            //}

            //tableMetaData.Synchronize(table);

            //tableMetaData.Update();
            //OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
            //MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = oldMetaObjectCreator;
            //sqlBulkCopy.WriteToServer(dataTable);


            // throw new NotImplementedException();
        }

        /// <MetaDataID>{f3aa0830-5a0b-40c8-a0b5-85bf01c9311f}</MetaDataID>
        protected override void DeleteTableRecords(IDataTable dataTable)
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            DataObjects.DataBase dataBase = new DataObjects.DataBase(StorageMetaData as RDBMSMetaDataRepository.Storage, "tmp");
            System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connection.NativeConnaction as System.Data.SqlClient.SqlConnection);
            sqlBulkCopy.DestinationTableName = "#TmpDelete_" + dataTable.TableName;
            //dataTable.TableName = "TmpDelete_" + dataTable.TableName;// new System.Data.DataTable("TmpDelete_" + dataTable.TableName);

            OOAdvantech.RDBMSDataObjects.Table.CreateTemporaryTable("TmpDelete_" + dataTable.TableName, dataTable, dataBase);
            sqlBulkCopy.WriteToServer(dataTable as System.Data.DataTable);

            string columnsString = null;
            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                if (columnsString != null)
                    columnsString += ", ";
                columnsString += dataTable.TableName + "." + column.ColumnName + " = #TmpDelete_" + dataTable.TableName + "." + column.ColumnName;
            }
            string commandText = @"DELETE FROM  " + dataTable.TableName + "\n";
            commandText += "FROM  " + dataTable.TableName + " INNER JOIN #TmpDelete_" + dataTable.TableName + " ON ";
            string innerJoinCriterion = null;
            foreach (System.Data.DataColumn OIDColumn in dataTable.Columns)
            {
                if (innerJoinCriterion != null)
                    innerJoinCriterion += " AND ";
                innerJoinCriterion += dataTable.TableName + "." + OIDColumn.ColumnName + " = #TmpDelete_" + dataTable.TableName + "." + OIDColumn.ColumnName;
            }
            commandText += innerJoinCriterion;
            commandText += "\n DROP TABLE #TmpDelete_" + dataTable.TableName;
            var command = Connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            //DELETE FROM T_OrderDetail
            //FROM         T_OrderDetail INNER JOIN
            //          T_OrderDetailB ON T_OrderDetail.ObjectID = T_OrderDetailB.ObjectID

        }

        /// <MetaDataID>{3b7a6f23-e152-4c3c-a176-c5df31067ab6}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{e4db4b08-f439-4d99-8cf1-2096db2e0ca2}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object objectID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{26998438-f517-44d3-a357-e6bbbfb2b978}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            throw new Exception("The method or operation is not implemented.");
        }

      
      
        /// <MetaDataID>{90EAA924-FC22-4313-A720-A7FDEAE745D4}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;
            }
        }

        /// <MetaDataID>{bcf6565b-9b0f-4551-96da-07d807b38362}</MetaDataID>
       


        /// <MetaDataID>{F91E1D65-51C6-4BD8-A781-EE1A0D960444}</MetaDataID>
        public OOAdvantech.Collections.Map DatabaseConectios = new OOAdvantech.Collections.Map();


        /// <MetaDataID>{2F3D67D9-5D43-40AC-AAE0-97E2F8E41630}</MetaDataID>
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
        }
        /// <MetaDataID>{893C5EAF-37E6-42F4-84B0-C7E8D5078D0D}</MetaDataID>
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }
        public override void PrepareForChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            
        }



        /// <MetaDataID>{b93223fe-4604-4afb-b6ae-60b532fe43ad}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }

     


        /// <MetaDataID>{550265FC-F766-4612-B959-0DFED7F47867}</MetaDataID>
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        /// <MetaDataID>{B10CBFE8-2751-4AFB-8CE8-0619A920C242}</MetaDataID>
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

       



   
     
   
        ///// <MetaDataID>{E757465A-B048-4694-A3DA-A1B24F7F8D1F}</MetaDataID>
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


        
        ///// <MetaDataID>{96BC2B54-62E2-4C55-98EA-C96E3C48AFDB}</MetaDataID>
        //public override PersistenceLayer.ObjectStorage StorageMetaData
        //{
        //    get
        //    {
        //        ObjectStorage mObjectStorage=null;
        //        mObjectStorage.ComponentRegistered+=new PersistenceLayer.ComponentRegisteredEventHandler(OnComponentRegistered);
        //        return null;
        //    }
        //}

        ///// <MetaDataID>{70ba9e18-6302-4491-8424-bd307b65ff3f}</MetaDataID>
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
        ///// <MetaDataID>{350590e5-49f2-43d9-be32-a32b414fe7ce}</MetaDataID>
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
        ///// <MetaDataID>{f1dacbd1-7251-480a-a7b0-d792d98e41cb}</MetaDataID>
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
        ///// <MetaDataID>{510f2a14-4386-4359-8977-f69355e9aca8}</MetaDataID>
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
        ///// <MetaDataID>{82c9a414-efce-4746-8d0d-a4cc1e899fd9}</MetaDataID>
        //static byte[] Buffer = new byte[4096];

        ///// <MetaDataID>{7c43f9dd-3b9a-4c10-81e3-5b13c91bc0e9}</MetaDataID>
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

        ///// <MetaDataID>{5c90bdd7-099a-4834-b94a-1777162f72d5}</MetaDataID>
        //public void GetData(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, RDBMSMetaDataRepository.StorageCell storageCell)
        //{
        //    int ert = 0;

        //}

       
    }
}
