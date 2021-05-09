using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MySQLMetaDataPersistenceRunTime
{
    /// <MetaDataID>{679e3cb4-25e1-4544-b708-dfe4f4283842}</MetaDataID>
    public class Storage : RDBMSMetaDataPersistenceRunTime.Storage
    {
        protected Storage()
        {

        }

        /// <MetaDataID>{a8ec58ad-7b83-4e8a-a3dc-410f15472c90}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, System.Data.Common.DbConnection connection, bool newStorage)
            : base(storageName, storageLocation, storageType, connection, newStorage)
        {

        }


        /// <MetaDataID>{ca12e73c-bbca-47de-81a6-98fa087a714b}</MetaDataID>
        protected override void RegisterClass(System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB> insertedClassBlobs, DotNetMetaDataRepository.Class _class)
        {
            string insertStatement =string.Format("INSERT INTO {0}.ClassBLOBS (ClassData,MetaObjectIdentity) Values( @ClassData,@MetaObjectIdentity )",StorageName);//+
            System.Data.Common.DbCommand insertSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, Connection);
            insertSqlCommand.CommandText = insertStatement;
            System.Data.Common.DbParameter parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = "@ClassData";
            parameter.DbType = System.Data.DbType.Binary;
            insertSqlCommand.Parameters.Add(parameter);
            parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = "@MetaObjectIdentity";
            parameter.DbType = System.Data.DbType.String;
            insertSqlCommand.Parameters.Add(parameter);

            string updateStatement =string.Format( "UPDATE {0}.ClassBLOBS SET ClassData = @ClassData WHERE MetaObjectIdentity = @MetaObjectIdentity",StorageName);
            System.Data.Common.DbCommand updateSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,Connection) ;
            updateSqlCommand.CommandText = updateStatement;
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = "@ClassData";
            parameter.DbType = System.Data.DbType.Binary;
            updateSqlCommand.Parameters.Add(parameter);
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = "@MetaObjectIdentity";
            parameter.DbType = System.Data.DbType.String;
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

        protected override string LoadClassBlobObjectIdentitiesSQLStatement
        {
            get
            {
                return string.Format( "SELECT ID,MetaObjectIdentity FROM {0}.ClassBLOBS ",StorageName);
            }
        }


        /// <MetaDataID>{4736d1a9-82f2-4e30-aac0-32d0b3ff3b17}</MetaDataID>
        protected override string LoadObjectBLOBS
        {
            get
            {
                return string.Format("SELECT ID, ClassBLOBSID, ObjectData FROM {0}.ObjectBLOBS", StorageName); ;
            }
        }
        /// <MetaDataID>{08539f7e-1202-4495-8010-dacb3fd1e427}</MetaDataID>
        protected override string LoadClassBlobsDataSQLStatement
        {
            get
            {
                return  string.Format( "SELECT ID, MetaObjectIdentity, ClassData FROM {0}.ClassBLOBS",StorageName);;
            }
        }

        /// <MetaDataID>{9679436f-e90b-45ef-9dd2-6005469fe89e}</MetaDataID>
        protected override string LoadStorageIdentities
        {
            get
            {
                return string.Format("SELECT DISTINCT StorageIdentity   FROM  {0}.IdentityTable", StorageName); 
            }
        }
        protected override void CreateMetaDataTables()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();


            System.Data.Common.DbCommand Command = Connection.CreateCommand();
            Command.CommandText = string.Format(@"CREATE TABLE `{0}`.`ClassBLOBS` (`ID` INTEGER  NOT NULL AUTO_INCREMENT,`MetaObjectIdentity` VARCHAR(255)  CHARACTER SET utf8 COLLATE utf8_general_ci,`ClassData` BLOB ,PRIMARY KEY (`ID`))", StorageName);
            Command.ExecuteNonQuery();
            Command = Connection.CreateCommand();
            Command.CommandText = string.Format(@"CREATE TABLE {0}.`ObjectBLOBS` (`ID` INTEGER  NOT NULL ,`ClassBLOBSID` INTEGER ,`ObjectData` BLOB,PRIMARY KEY (`ID`))", StorageName);
            Command.ExecuteNonQuery();
            Command = Connection.CreateCommand();
            Command.CommandText = string.Format("CREATE TABLE {0}.IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + _StorageIdentity + "')", StorageName);
            Command.ExecuteNonQuery();

        }


    }
}
