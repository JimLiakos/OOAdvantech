namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime.Commands
{
    /// <MetaDataID>{A1646E36-A355-4F5C-A04E-F167DEF62DF2}</MetaDataID>
    public class UpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
    {
        public UpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef)
            : base(updatedStorageInstanceRef)
        {
        }
        static object[] itemArr = new object[] { null, null, null };
        internal bool FromNewCommand = false;
        /// <MetaDataID>{6D66B69B-AA5A-4AE6-B84A-826D4871F319}</MetaDataID>
        /// <summary>With this method execute the command. </summary>
        public override void Execute()
        {

            //TODO: Ο τρόπος που χρησιμοποιείται η SQLConnection δεν είναι thread safe;
            var SQLConnection = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).Connection;
            string ClassBLOBSID = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetSQLScriptForName("ClassBLOBSID");
            string ClassBLOBSIDParName = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetAdoNetParameterName("ClassBLOBSID");
            string ID = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetSQLScriptForName("ID");
            string IDParName = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetAdoNetParameterName("ID");
            string ObjectData = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetSQLScriptForName("ObjectData");
            string ObjectDataParName = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).GetAdoNetParameterName("ObjectData");

            if (FromNewCommand)
            {
                //INSERT INTO ObjectBLOBS (ID,ClassBLOBSID,ObjectData) Values(@ID,@ClassBLOBSID,@ObjectData
                string insertStatement = string .Format( "INSERT INTO ObjectBLOBS ({0},{1},{2}) Values({3},{4},{5})",ID,ClassBLOBSID,ObjectData,IDParName,ClassBLOBSIDParName,ObjectDataParName);
                var insertSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, SQLConnection);
                insertSqlCommand.CommandText = insertStatement;

                IDataBaseParameter parameter = null;

                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = IDParName;
                parameter.DbType = DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);

                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = ClassBLOBSIDParName;
                parameter.DbType = DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);


                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                (UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);

                int length = 0;
                length = (int)memoryStream.Length;
                (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).Bytes += length;

                byte[] BLOB = new byte[length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);


                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = ObjectDataParName;
                parameter.DbType = DbType.Binary;
                parameter.Size = length;
                insertSqlCommand.Parameters.Add(parameter);

                insertSqlCommand.Parameters[ObjectDataParName].Value = BLOB;
                insertSqlCommand.Parameters[IDParName].Value = UpdatedStorageInstanceRef.PersistentObjectID.GetMemberValue("ObjectID");
                insertSqlCommand.Parameters[ClassBLOBSIDParName].Value = (UpdatedStorageInstanceRef as StorageInstanceRef).SerializationMetada.ID;




                if (SQLConnection.State != ConnectionState.Open)
                    SQLConnection.Open();
                //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
                //TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
                //SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                try
                {

                    try
                    {
                        if(insertSqlCommand.DesignTimeVisible )
                            insertSqlCommand.DesignTimeVisible = false;
                    }
                    catch
                    {


                    }
                    insertSqlCommand.ExecuteNonQuery();
                    //System.Diagnostics.Debug.WriteLine("ExecuteNonQuery");

                }
#if DEBUG
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
#endif
                finally
                {
                }

            }
            else
            {
                bool hasChangeState = false;
                foreach (RelResolver relResolver in UpdatedStorageInstanceRef.RelResolvers)
                {
                    if (HasChanges(relResolver))
                        hasChangeState = true;

                }

                if (!hasChangeState && !UpdatedStorageInstanceRef.HasChangeState())
                    return;


                //"UPDATE ObjectBLOBS SET ObjectData=@ObjectData WHERE ID=@OID";

                string updateStatement = string.Format("UPDATE ObjectBLOBS SET {0}={1} WHERE {2}={3}",ObjectData,ObjectDataParName,ID,IDParName);

                var updateSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,SQLConnection) ;
                updateSqlCommand.CommandText = updateStatement;

                var objectDataParameter = updateSqlCommand.CreateParameter();// updateSqlCommand.Parameters.Add("@ObjectData", System.Data.SqlDbType.Image);
                objectDataParameter.ParameterName = ObjectDataParName;
                objectDataParameter.DbType = DbType.Binary;
                updateSqlCommand.Parameters.Add(objectDataParameter);
                var OIDParameter = updateSqlCommand.CreateParameter();//updateSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
                OIDParameter.ParameterName = IDParName;
                OIDParameter.DbType = DbType.Int32;
                updateSqlCommand.Parameters.Add(OIDParameter);

                OIDParameter.Value = UpdatedStorageInstanceRef.PersistentObjectID.GetMemberValue("ObjectID");
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                (UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);
                byte[] BLOB = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);
                objectDataParameter.Value = BLOB;

                if (SQLConnection.State != ConnectionState.Open)
                    SQLConnection.Open();
                //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
                //TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
                //SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                try
                {
                    updateSqlCommand.ExecuteNonQuery();

                }
#if DEBUG
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
#endif
                finally
                {
                }
            }

        }
        public bool HasChanges(PersistenceLayerRunTime.RelResolver relResolver)
        {

            //System.Reflection.FieldInfo associationEndFieldInfo=relResolver.FieldInfo;
            AccessorBuilder.FieldPropertyAccessor associationEndFastFieldAccessor = relResolver.FastFieldAccessor;
            if (relResolver.AssociationEnd.Multiplicity.IsMany)// associationEndFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||associationEndFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            {
                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)associationEndFastFieldAccessor.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                if (theObjectContainer == null)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");
                PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                if (mObjectCollection == null || mObjectCollection.RelResolver != relResolver)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");

                return mObjectCollection.HasChanges;
            }
            else
            {
                //object NewValue=associationEndFieldInfo.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                object NewValue = Member<object>.GetValue(associationEndFastFieldAccessor.GetValue, UpdatedStorageInstanceRef.MemoryInstance);
                object OldValue = relResolver.OriginalRelatedObject;

                if (NewValue != OldValue)
                    return true;
                return false;
            }

        }
    }
}
