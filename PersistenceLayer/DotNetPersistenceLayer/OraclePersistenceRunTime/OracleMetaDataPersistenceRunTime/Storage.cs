using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;

namespace OOAdvantech.OracleMetaDataPersistenceRunTime
{
    /// <MetaDataID>{679e3cb4-25e1-4544-b708-dfe4f4283842}</MetaDataID>
    public class Storage : RDBMSMetaDataPersistenceRunTime.Storage
    {
        protected Storage()
        {

        }

        /// <MetaDataID>{a8ec58ad-7b83-4e8a-a3dc-410f15472c90}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection, bool newStorage)
            : base(storageName, storageLocation, storageType, connection, newStorage)
        {

        }


        /// <MetaDataID>{ca12e73c-bbca-47de-81a6-98fa087a714b}</MetaDataID>
        protected override void RegisterClass(System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB> insertedClassBlobs, DotNetMetaDataRepository.Class _class)
        {
            string insertStatement = @"INSERT INTO ClassBLOBS (ClassData,MetaObjectIdentity) Values( :ClassData,:MetaObjectIdentity )";//+
            var insertSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, Connection);
            insertSqlCommand.CommandText = insertStatement;
            var parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = ":ClassData";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.Binary;
            insertSqlCommand.Parameters.Add(parameter);
            parameter = insertSqlCommand.CreateParameter();
            parameter.ParameterName = ":MetaObjectIdentity";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.String;
            insertSqlCommand.Parameters.Add(parameter);

            string updateStatement = string.Format("UPDATE ClassBLOBS SET ClassData = @ClassData WHERE MetaObjectIdentity = @MetaObjectIdentity", StorageName);
            var updateSqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,Connection) ;
            updateSqlCommand.CommandText = updateStatement;
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = ":ClassData";
            parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.Binary;
            updateSqlCommand.Parameters.Add(parameter);
            parameter = updateSqlCommand.CreateParameter();
            parameter.ParameterName = ":MetaObjectIdentity";
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

                insertSqlCommand.Parameters[":ClassData"].Value = outByteStream;
                //  insertSqlCommand.Parameters["@ClassData"].Size = outByteStream.Length;
                insertSqlCommand.Parameters[":MetaObjectIdentity"].Value = _class.Identity.ToString();

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
                return string.Format("SELECT ID,MetaObjectIdentity FROM ClassBLOBS ", StorageName);
            }
        }

        /// <MetaDataID>{4736d1a9-82f2-4e30-aac0-32d0b3ff3b17}</MetaDataID>
        protected override string LoadObjectBLOBS
        {
            get
            {
                return string.Format("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", StorageName); ;
            }
        }
        /// <MetaDataID>{08539f7e-1202-4495-8010-dacb3fd1e427}</MetaDataID>
        protected override string LoadClassBlobsDataSQLStatement
        {
            get
            {
                return string.Format("SELECT ID, MetaObjectIdentity, ClassData FROM ClassBLOBS", StorageName); ;
            }
        }

        /// <MetaDataID>{9679436f-e90b-45ef-9dd2-6005469fe89e}</MetaDataID>
        protected override string LoadStorageIdentities
        {
            get
            {
                return string.Format("SELECT DISTINCT StorageIdentity   FROM  IdentityTable", StorageName);
            }
        }
        protected override void CreateMetaDataTables()
        {


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                        Connection.Open();
                     

                    System.Data.Common.DbCommand Command = new OracleConnection(Connection.ConnectionString + ";Password = astraxan").CreateCommand();
                    Command.Connection.Open();
                    Command.CommandText = @"CREATE TABLE CLASSBLOBS
                                (
                                  ID INT NOT NULL,
                                  METAOBJECTIDENTITY NVARCHAR2(255),
                                  CLASSDATA BLOB, 
                                  CONSTRAINT CLASSBLOBS_PK PRIMARY KEY(ID) ENABLE
                                )";
                    Command.ExecuteNonQuery();
                    Command.CommandText = @"CREATE TABLE OBJECTBLOBS
                                (
                                  ID INTEGER NOT NULL,
                                  CLASSBLOBSID INTEGER,
                                  OBJECTDATA BLOB, 
                                  CONSTRAINT OBJECTBLOBS_PK PRIMARY KEY(ID) ENABLE
                                )";
                    Command.ExecuteNonQuery();




                    Command.CommandText = @"CREATE SEQUENCE ID_seq";
                    Command.ExecuteNonQuery();
                    Command.CommandText = @"CREATE SEQUENCE  ""MetaObjectID_Seq""  MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 100 START WITH 2001 CACHE 20 NOORDER  NOCYCLE ";
                    Command.ExecuteNonQuery();
                    Command.CommandText = @"CREATE  TRIGGER CLASSBLOBS_NEW_ROW BEFORE INSERT ON CLASSBLOBS FOR EACH ROW BEGIN SELECT ID_SEQ.NEXTVAL INTO :NEW.ID FROM DUAL; END;";

                    Command.ExecuteNonQuery();

                    Command.CommandText = "CREATE TABLE IDENTITYTABLE ( NEXTID INTEGER NOT NULL,STORAGEIDENTITY NVARCHAR2(255) DEFAULT '" + _StorageIdentity + "')";
                    Command.ExecuteNonQuery();



                    Command.CommandText = @"CREATE TABLE ""DDL_STATS""
                                      (
                                        ""USER_NAME"" NVARCHAR2(255),
                                        ""DDL_DATE"" DATE,
                                        ""DDL_TYPE""    NVARCHAR2(255),
                                        ""OBJECT_TYPE"" NVARCHAR2(255),
                                        ""OWNER""       NVARCHAR2(255),
                                        ""OBJECT_NAME"" NVARCHAR2(255)
                                      )";
                    Command.ExecuteNonQuery();

                    Command.CommandText = @"create or replace TRIGGER DDLTRIGGER AFTER DDL ON DATABASE BEGIN INSERT INTO DDL_STATS (USER_NAME,DDL_DATE,DDL_TYPE,OBJECT_TYPE,OWNER,OBJECT_NAME) VALUES(ORA_LOGIN_USER,SYSDATE,ORA_SYSEVENT,ORA_DICT_OBJ_TYPE,ORA_DICT_OBJ_OWNER,ORA_DICT_OBJ_NAME); END;"; 
//                                        @"create or replace
//                                        trigger DDLTrigger
//                                        AFTER DDL ON DATABASE
//                                        BEGIN
//                                        INSERT INTO
//                                        DDL_STATS (USER_NAME,DDL_DATE,DDL_TYPE,OBJECT_TYPE,OWNER,OBJECT_NAME)
//                                        VALUES(ora_login_user,sysdate,ora_sysevent,ora_dict_obj_type,ora_dict_obj_owner,ora_dict_obj_name);
//                                        END;";
                    Command.ExecuteNonQuery();
                    Command.CommandText = "ALTER TRIGGER DDLTRIGGER ENABLE";
                    Command.ExecuteNonQuery();


                    Command.Connection.Close();
                    transactionScope.Complete();

                }
                stateTransition.Consistent = true;
            }

        }

     
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            throw new NotImplementedException();
        }



        public override void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData)
        {
            throw new NotImplementedException();
        }

        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData)
        {
            throw new NotImplementedException();
        }
    }
}
