using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace OOAdvantech.SQLiteMetaDataPersistenceRunTime
{
    /// <MetaDataID>{fe294a10-df87-4779-9e42-b147773856f6}</MetaDataID>
    public class Storage : RDBMSMetaDataPersistenceRunTime.Storage
    {
        /// <MetaDataID>{700fb08f-a980-41f0-a74c-fee28397cf85}</MetaDataID>
        protected Storage()
        {
            

        }
        protected override string LoadObjectBLOBS
        {
            get 
            {
                return "SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS";
            }
        }

        /// <MetaDataID>{fdfe3378-89d7-493f-80e1-a5f083f0b95e}</MetaDataID>
        protected override void RegisterClass(System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB> insertedClassBlobs, DotNetMetaDataRepository.Class _class)
        {
            string insertStatement = "INSERT INTO ClassBLOBS (ClassData,MetaObjectIdentity) Values( @ClassData,@MetaObjectIdentity ) ";//+
            var insertSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, Connection);
            insertSqlCommand.CommandText = insertStatement;
            var parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = "@ClassData";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.Binary;
            insertSqlCommand.Parameters.Add(parameter);
            parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = "@MetaObjectIdentity";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.String;
            insertSqlCommand.Parameters.Add(parameter);

            string updateStatement = "UPDATE ClassBLOBS SET ClassData = @ClassData WHERE MetaObjectIdentity = @MetaObjectIdentity";
            var updateSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,Connection) ;
            updateSqlCommand.CommandText = updateStatement;
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = "@ClassData";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.Binary;
            updateSqlCommand.Parameters.Add(parameter);
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = "@MetaObjectIdentity";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.String;
            updateSqlCommand.Parameters.Add(parameter);

            byte[] byteStream = new byte[65536];
            int offset = 4;


            OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB classBLOB = GetClassBLOBIfExist(_class);
            if (classBLOB == null)
            {
                classBLOB = new OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB(_class);
                classBLOB.Serialize(byteStream, offset, out offset);
                int nextpos = 0;
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset - 4, byteStream, 0, ref nextpos, true);
                byte[] outByteStream = new byte[offset];

                for (int i = 0; i != offset; i++)
                    outByteStream[i] = byteStream[i];

                insertSqlCommand.Parameters["@ClassData"].Value = outByteStream;
                //  insertSqlCommand.Parameters["@ClassData"].Size = outByteStream.Length;
                insertSqlCommand.Parameters["@MetaObjectIdentity"].Value = _class.Identity.ToString();

                insertedClassBlobs.Add(_class.Identity.ToString(), classBLOB);
                insertSqlCommand.ExecuteNonQuery();
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

                    updateSqlCommand.Parameters["@ClassData"].Value = outByteStream;
                    //  updateSqlCommand.Parameters["@ClassData"].Size = outByteStream.Length;
                    updateSqlCommand.Parameters["@MetaObjectIdentity"].Value = _class.Identity.ToString();
                    int res = updateSqlCommand.ExecuteNonQuery();
                    classBLOB.HasChange = false;
                }
            }

        }


        /// <MetaDataID>{a8ec58ad-7b83-4e8a-a3dc-410f15472c90}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection, bool newStorage)
            : base(storageName, storageLocation, storageType, connection, newStorage)
        {

        }
        protected override string LoadClassBlobObjectIdentitiesSQLStatement
        {
            get
            {
                return "SELECT ID,MetaObjectIdentity FROM ClassBLOBS ";
            }
        }
    

        /// <MetaDataID>{08539f7e-1202-4495-8010-dacb3fd1e427}</MetaDataID>
        protected override string LoadClassBlobsDataSQLStatement
        {
            get
            {
                return "SELECT ID, MetaObjectIdentity, ClassData FROM ClassBLOBS";
            }
        }

        /// <MetaDataID>{9679436f-e90b-45ef-9dd2-6005469fe89e}</MetaDataID>
        protected override string LoadStorageIdentities
        {
            get
            {
                return "SELECT DISTINCT StorageIdentity   FROM  IdentityTable";
            }
        }
        protected override void CreateMetaDataTables()
        {
            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();



#if !DeviceDotNet
            Connection.EnlistTransaction(System.Transactions.Transaction.Current);
#endif
            var Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);

            Command.CommandText = "CREATE TABLE ClassBLOBS(ID  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  ,MetaObjectIdentity nvarchar(255),ClassData BLOB NULL)";//"CREATE TABLE ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) ";
            //Command.Transaction = trans; 
            Command.ExecuteNonQuery();
            //Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
            //Command.CommandText = "ALTER TABLE ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY  (ID)";
            ////Command.Transaction = trans;
            //Command.ExecuteNonQuery();

            Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
            Command.CommandText = "CREATE TABLE ObjectBLOBS(ID  INTEGER PRIMARY KEY NOT NULL ,ClassBLOBSID int,ObjectData BLOB NULL) ";
            //Command.Transaction = trans;
            Command.ExecuteNonQuery();

            //Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
            //Command.CommandText = "ALTER TABLE ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY  (ID)";
            ////Command.Transaction = trans;
            //Command.ExecuteNonQuery();

            Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
            Command.CommandText =string.Format( "CREATE TABLE IdentityTable (NEXTID INTEGER NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '{0}')",  _StorageIdentity );
            // Command.Transaction = trans;
            Command.ExecuteNonQuery();

        }

        public override void RegisterComponent(string assemblyFullName, XDocument mappingData)
        {
            throw new NotImplementedException();
        }
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            throw new NotImplementedException();
        }

        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, XDocument> assembliesMappingData)
        {
            throw new NotImplementedException();
        }

        public override bool CheckForVersionUpgrate(string fullName)
        {
            return true;
        }
    }
}
