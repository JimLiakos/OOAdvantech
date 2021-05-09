using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.OracleMetaDataPersistenceRunTime.Commands
{
    /// <MetaDataID>{9b1b6303-1355-4c2e-88c2-eadbb25ce7f9}</MetaDataID>
    public class UpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
    {
        /// <MetaDataID>{d80eac0d-81ea-4239-946a-65cd4fad153e}</MetaDataID>
        public UpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef)
            : base(updatedStorageInstanceRef)
        {
        }
        /// <MetaDataID>{0b9f0908-8ae2-40d7-9b05-19c4224fa60f}</MetaDataID>
        static object[] itemArr = new object[] { null, null, null };
        /// <MetaDataID>{52ebcf17-b361-4853-99e7-197c982d9ef7}</MetaDataID>
        internal bool FromNewCommand = false;
        /// <MetaDataID>{6D66B69B-AA5A-4AE6-B84A-826D4871F319}</MetaDataID>
        /// <summary>With this method execute the command. </summary>
        public override void Execute()
        {


            if (FromNewCommand)
            {

                System.Data.Common.DbConnection SQLConnection = (UpdatedStorageInstanceRef.ObjectStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;
                string insertStatement =string.Format("INSERT INTO ObjectBLOBS (ID,ClassBLOBSID,ObjectData) Values(:ID,:ClassBLOBSID,:ObjectData)",UpdatedStorageInstanceRef.ObjectStorage.StorageMetaData.StorageName);
                System.Data.Common.DbCommand insertSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, SQLConnection);
                insertSqlCommand.CommandText = insertStatement;

                System.Data.Common.DbParameter parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = ":ID";
                parameter.DbType = System.Data.DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);
                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = ":ClassBLOBSID";
                parameter.DbType = System.Data.DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);

                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                (UpdatedStorageInstanceRef as RDBMSMetaDataPersistenceRunTime.StorageInstanceRef).SaveObjectState(memoryStream);

                int length = 0;
                //if(memoryStream.Length<250)
                //length=250;
                //else
                length = (int)memoryStream.Length;
                (UpdatedStorageInstanceRef.ObjectStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Bytes += length;

                byte[] BLOB = new byte[length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);
                //if (((int)UpdatedStorageInstanceRef.ObjectID) == 667)
                //{
                //    System.IO.FileStream stream = System.IO.File.Open(@"C:\save.bin", System.IO.FileMode.CreateNew);
                //    stream.Write(BLOB, 0, BLOB.Length);
                //    stream.Close();

                //}


                //insertSqlCommand.Parameters.Add("@ObjectData",  System.Data.SqlDbType.Image,length);

                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = ":ObjectData";
                parameter.DbType = System.Data.DbType.Binary;
                parameter.Size = length;
                insertSqlCommand.Parameters.Add(parameter);

                insertSqlCommand.Parameters[":ObjectData"].Value = BLOB;
                insertSqlCommand.Parameters[":ID"].Value = UpdatedStorageInstanceRef.ObjectID;
                insertSqlCommand.Parameters[":ClassBLOBSID"].Value = (UpdatedStorageInstanceRef as RDBMSMetaDataPersistenceRunTime.StorageInstanceRef).SerializationMetada.ID;

                if (SQLConnection.State != System.Data.ConnectionState.Open)
                    SQLConnection.Open();
                //TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
                try
                {
                    try
                    {
                        insertSqlCommand.DesignTimeVisible = false;
                    }
                    catch
                    {
                    }
                    insertSqlCommand.ExecuteNonQuery();

                }
                catch (System.Exception Error)
                {
                    throw;
                }

            }
            else
            {
                bool hasChangeState = false;
                foreach (RDBMSMetaDataPersistenceRunTime.RelResolver relResolver in UpdatedStorageInstanceRef.RelResolvers)
                {
                    if (HasChanges(relResolver))
                        hasChangeState = true;
                }
                if (!hasChangeState && !UpdatedStorageInstanceRef.HasChangeState())
                    return;

                //TODO: Ο τρόπος που χρησιμοποιείται η SQLConnection δεν είναι thread safe;
                System.Data.Common.DbConnection SQLConnection = (UpdatedStorageInstanceRef.ObjectStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;

                string updateStatement = string.Format("UPDATE {0}.ObjectBLOBS SET ObjectData=:ObjectData WHERE ID=:OID", UpdatedStorageInstanceRef.ObjectStorage.StorageMetaData.StorageName);

                System.Data.Common.DbCommand updateSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,SQLConnection) ;
                updateSqlCommand.CommandText = updateStatement;

                System.Data.Common.DbParameter objectDataParameter = updateSqlCommand.CreateParameter();// updateSqlCommand.Parameters.Add("@ObjectData", System.Data.SqlDbType.Image);
                objectDataParameter.ParameterName = ":ObjectData";
                objectDataParameter.DbType = System.Data.DbType.Binary;
                updateSqlCommand.Parameters.Add(objectDataParameter);
                System.Data.Common.DbParameter OIDParameter = updateSqlCommand.CreateParameter();//updateSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
                OIDParameter.ParameterName = ":OID";
                OIDParameter.DbType = System.Data.DbType.Int32;
                updateSqlCommand.Parameters.Add(OIDParameter);

                OIDParameter.Value = UpdatedStorageInstanceRef.ObjectID;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                (UpdatedStorageInstanceRef as RDBMSMetaDataPersistenceRunTime.StorageInstanceRef).SaveObjectState(memoryStream);
                byte[] BLOB = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);
                objectDataParameter.Value = BLOB;
                //if (((int)UpdatedStorageInstanceRef.ObjectID) == 667)
                //{
                //    System.IO.FileStream stream = System.IO.File.Open(@"C:\save.bin", System.IO.FileMode.CreateNew);
                //    stream.Write(BLOB, 0, BLOB.Length);
                //    stream.Close();

                //}

                if (SQLConnection.State != System.Data.ConnectionState.Open)
                    SQLConnection.Open();
                //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
                //TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
                //SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                try
                {
                    updateSqlCommand.ExecuteNonQuery();
                }
                catch (System.Exception Error)
                {
                    throw;
                }
            }

        }
        /// <MetaDataID>{f017b2bd-5042-492b-bacd-d7e4de36a4ea}</MetaDataID>
        public bool HasChanges(PersistenceLayerRunTime.RelResolver relResolver)
        {

            //System.Reflection.FieldInfo associationEndFieldInfo=relResolver.FieldInfo;
            AccessorBuilder.FieldPropertyAccessor associationEndFastFieldAccessor = relResolver.FastFieldAccessor;
            if (relResolver.AssociationEnd.Multiplicity.IsMany)// associationEndFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||associationEndFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            {
                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)associationEndFastFieldAccessor.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                if (theObjectContainer == null)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");
                PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection = RDBMSMetaDataPersistenceRunTime.StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                if (mObjectCollection == null || mObjectCollection.RelResolver != relResolver)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");

                return mObjectCollection.HasChanges;
            }
            else
            {
                //object NewValue=associationEndFieldInfo.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                object NewValue = Member<object>.GetValue(associationEndFastFieldAccessor.GetValue, UpdatedStorageInstanceRef.MemoryInstance);
                object OldValue = relResolver.RelatedObject;

                if (NewValue != OldValue)
                    return true;
                return false;
            }

        }
    }
}
