
namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
    using OOAdvantech.DotNetMetaDataRepository;
    /// <MetaDataID>{02E16727-2D67-4E2D-B1C0-58B42257A7D2}</MetaDataID>
    public abstract class Storage : OOAdvantech.MetaDataRepository.Storage
    {


        protected abstract string LoadClassBlobObjectIdentitiesSQLStatement
        {
            get;
        }
        protected abstract string LoadClassBlobsDataSQLStatement
        {
            get;
        }
        protected abstract string LoadObjectBLOBS
        {
            get;
        }
        protected abstract string LoadStorageIdentities
        {
            get;
        }
        protected abstract void CreateMetaDataTables();

        /// <MetaDataID>{ff635c17-256f-4290-989d-56091caf2ab9}</MetaDataID>
        static internal System.Collections.Generic.Dictionary<string, string> BackwardCompatibilities = new System.Collections.Generic.Dictionary<string, string>();
        /// <MetaDataID>{66d25cee-c5e8-44dd-80cf-f014b0b35b19}</MetaDataID>
        static Storage()
        {
            BackwardCompatibilities.Add("OOAdvantech, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643",
                                        "OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643");

        #if Net4
            //BackwardCompatibilities.Add("OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643",
            //                            "OOAdvantech, Version=4.0.0.0, Culture=neutral, PublicKeyToken=1e9d634bb78f5bfb");

            BackwardCompatibilities.Add("RDBMSMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=483eb08c93287fcd",
                                        "RDBMSMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=ab7ad9c2d64354ad");

            BackwardCompatibilities.Add("RDBMSPersistenceRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=6e6511f6710c92c8",
                                       "RDBMSPersistenceRunTime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=1745cbd856bbe2d9");
        #endif
        }
        //public override object CreateDataLoader(object dataNode, object searchCondition, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells)
        //{
        //    return null;
        //    //return new ObjectQueryLanguage.DataLoader(dataNode as MetaDataRepository.ObjectQueryLanguage.DataNode,searchCondition as MetaDataRepository.ObjectQueryLanguage.SearchCondition, storageCells);
        //}

        /// <MetaDataID>{b183eddc-3730-43bc-ba39-ecbf097bae7f}</MetaDataID>
        protected Storage()
        {

            _StorageType = "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider";

        }
        /// <MetaDataID>{A6851F21-6CDB-4ACB-A4B1-0F50215FC28B}</MetaDataID>
        public Collections.Generic.Dictionary<int, DataObjects.ClassBLOB> ClassBLOBs = new Collections.Generic.Dictionary<int, DataObjects.ClassBLOB>();


        /// <MetaDataID>{f22de09e-ab39-4ec7-9377-e2fb0583c621}</MetaDataID>
        protected readonly IDataBaseConnection Connection;


        /// <MetaDataID>{93B64DF4-E3DE-4BEC-A1BF-47A91945F856}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, IDataBaseConnection  connection, bool newStorage)
        {
            _StorageType = storageType;
            _StorageName = storageName;
            _StorageLocation = storageLocation;
            Connection = connection;

            //  System.Transactions.Transaction mytrans = System.Transactions.Transaction.Current;
            // using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                try
                {
                    //using (System.Transactions.TransactionScope trans = new System.Transactions.TransactionScope())
                    //{
                    int count = 2;
                    while (count > 0)
                    {
                        count--;
                        try
                        {
                            if (Connection.State != ConnectionState.Open)
                            {
                                //Connection.EnlistTransaction(System.Transactions.Transaction.Current);
                                Connection.Open();
                            }
                            break;
                        }
                        catch (System.Exception error)
                        {
#if !DeviceDotNet
                            System.Threading.Thread.Sleep(1000);
#endif
                        }
                    }
                    if (Connection.State != ConnectionState.Open)
                    {
                        //Connection.EnlistTransaction(System.Transactions.Transaction.Current);
                        Connection.Open();

                    }
                    //double tim = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                    if (newStorage)
                        CreateMetaDataTables();
                    else
                    {

                        LoadClassBlobs();
                        string query = LoadStorageIdentities;// "SELECT DISTINCT StorageIdentity   FROM  IdentityTable";
                        var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(query, Connection);
                        command.CommandText = query;
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            _StorageIdentity = dataReader["StorageIdentity"] as string;
                            break;
                        }
                        dataReader.Close();
                    }
                    //double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                    //    trans.Complete();                    //}
                }
                catch (System.Exception error)
                {

                    throw;
                }

            }



        }
        /// <MetaDataID>{70C31C2C-ECDE-426D-83B0-BA492E2D47B1}</MetaDataID>
        void LoadClassBlobs()
        {

            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            try
            {

                string query = LoadClassBlobsDataSQLStatement;// "SELECT ID, MetaObjectIdentity, ClassData FROM ClassBLOBS";
                var SqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(query, Connection);
                SqlCommand.CommandText = query;
                //double tim = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                var dataReader = SqlCommand.ExecuteReader();
                //double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                while (dataReader.Read())
                {
                    int ID = System.Convert.ToInt32(dataReader["ID"]);
                    string MetaObjectIdentity = dataReader["MetaObjectIdentity"] as string;
                    byte[] ClassData = (byte[])dataReader["ClassData"];
                    int offset = 4;
                    DataObjects.ClassBLOB classBLOB = new DataObjects.ClassBLOB(ClassData, offset);
                    ClassBLOBs.Add(ID, classBLOB);
                    classBLOB.ID = ID;
                    int erera = 0;
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
                throw new System.Exception(Error.Message, Error);
            }

            //double tim3 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;

        }
        /// <MetaDataID>{F1E7EBE9-3723-40B7-BB83-932A69D70653}</MetaDataID>
        public DataObjects.ClassBLOB GetClassBLOB(int classBLOBID)
        {
            return ClassBLOBs[classBLOBID] as DataObjects.ClassBLOB;
        }
        /// <MetaDataID>{507A9A07-9DAD-498D-A875-D5CEE60D0840}</MetaDataID>
        protected DataObjects.ClassBLOB GetClassBLOBIfExist(DotNetMetaDataRepository.Class _class)
        {
            if (_class == null)
                return null;
            foreach (var entry in ClassBLOBs)
            {
                DataObjects.ClassBLOB classBLOB = entry.Value as DataObjects.ClassBLOB;
                if (classBLOB.Class == _class)
                    return classBLOB;
            }
            return null;
        }
        /// <MetaDataID>{A02C227C-B479-44D1-8893-CA0F45E5D4F7}</MetaDataID>
        internal DataObjects.ClassBLOB GetClassBLOB(DotNetMetaDataRepository.Class _class)
        {
            DataObjects.ClassBLOB classBLOB = GetClassBLOBIfExist(_class);
            if (classBLOB != null)
                return classBLOB;
            else
                throw new System.Exception("There isn't metada for class \"" + _class.FullName + "\". Register the assembly of class and try again.");
        }






        /// <MetaDataID>{24EC9E67-4FC5-4067-94CC-F7A2E7670D8D}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames)
        {
            DotNetMetaDataRepository.Assembly mAssembly = null;
            System.Collections.Generic.List<MetaDataRepository.Component> components = new System.Collections.Generic.List<MetaDataRepository.Component>();
            foreach (string Component in assembliesFullNames)
            {
                System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( Component));
                object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                if (objects.Length == 0)
                    throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

                mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
                if (mAssembly == null)
                    mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;

                System.Collections.Generic.List<MetaDataError> errors = new System.Collections.Generic.List<MetaDataError>();
                bool hasErrors = mAssembly.ErrorCheck(ref errors);
                if (hasErrors)
                {
                    string ErrorMessage = null;
                    foreach (MetaDataRepository.MetaObject.MetaDataError error in errors)
                    {
                        if (ErrorMessage != null)
                            ErrorMessage += "\n";
                        ErrorMessage += error.ErrorMessage;
                    }
                    throw new System.Exception(ErrorMessage);
                }
                components.Add(mAssembly);
            }

            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                foreach (DotNetMetaDataRepository.Assembly component in components)
                    RegisterComponent(component);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{30A9F0CD-2364-4BA8-830B-177660770661}</MetaDataID>
        private void GetReferenceToComponents(MetaDataRepository.Component Component, System.Collections.Generic.List<MetaDataRepository.Component> components)
        {

            foreach (MetaDataRepository.Dependency dependency in Component.ClientDependencies)
            {
                MetaDataRepository.Component refComponent = dependency.Supplier as MetaDataRepository.Component;


                if (!components.Contains(refComponent))
                {
                    object[] objects = (refComponent as DotNetMetaDataRepository.Assembly).WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                    if (objects.Length == 0)
                        continue;
                    components.Add(refComponent);
                    GetReferenceToComponents(refComponent, components);
                }
            }
        }

        /// <MetaDataID>{DB9584CB-EABD-4EEC-8390-8DAE65B50E2C}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName)
        {
            DotNetMetaDataRepository.Assembly mAssembly = null;
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0|| !(objects[0] is MetaDataRepository.BuildAssemblyMetadata))
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            if (mAssembly == null)
                mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;

            System.Collections.Generic.List<MetaDataError> errors = new System.Collections.Generic.List<MetaDataError>();
#if !DeviceDotNet
            bool hasErrors = mAssembly.ErrorCheck(ref errors);
            if (hasErrors)
            {
                string ErrorMessage = null;
                foreach (MetaDataRepository.MetaObject.MetaDataError error in errors)
                {
                    if (ErrorMessage != null)
                        ErrorMessage += "\n";
                    ErrorMessage += error.ErrorMessage;
                }
                throw new System.Exception(ErrorMessage);
            }
#endif
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                RegisterComponent(mAssembly);
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{abd2f4b4-19b1-4a2a-9277-b96e82a0c37b}</MetaDataID>
        protected abstract void RegisterClass(System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB> insertedClassBlobs, DotNetMetaDataRepository.Class _class);


        /// <MetaDataID>{B734A634-10F7-44ED-BEF3-E484849DA8C6}</MetaDataID>
        public void RegisterComponent(MetaDataRepository.Component Component)
        {


            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            System.Collections.Generic.List<MetaDataRepository.Component> components = new System.Collections.Generic.List<MetaDataRepository.Component>();
            GetReferenceToComponents(Component, components);
            components.Add(Component);
            System.Collections.Generic.Dictionary<string, DataObjects.ClassBLOB> insertedClassBlobs = new System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects.ClassBLOB>();

            foreach (MetaDataRepository.Component _Component in components)
            {
                if (_Component.Identity.ToString() == typeof(DotNetMetaDataRepository.Assembly).GetMetaData().Assembly.FullName)
                    continue;

                System.Reflection.Assembly dotNetAssembly = (_Component as Assembly).WrAssembly;
                object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                if (objects.Length == 0 || !(objects[0] is MetaDataRepository.BuildAssemblyMetadata))
                    continue;
                    //throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

                foreach (MetaDataRepository.MetaObject metaObject in _Component.Residents)
                {
                    if (metaObject.Namespace != null && metaObject.FullName != "OOAdvantech.MetaDataRepository.Namespace" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.MultiplicityRange" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Enumeration" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Realization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AssociationEndRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AttributeRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Parameter" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.ObjectIdentityType" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.IdentityPart" &&
                        metaObject.FullName != typeof(OOAdvantech.MetaDataRepository.StorageReference).FullName &&
                        (metaObject.Namespace.FullName == "OOAdvantech.RDBMSDataObjects" ||
                        metaObject.Namespace.FullName == "OOAdvantech.MetaDataRepository"))
                        continue;
                    DotNetMetaDataRepository.Class _class = metaObject as DotNetMetaDataRepository.Class;
                    try
                    {
                        if (_class != null && _class.Persistent)
                            RegisterClass(insertedClassBlobs, _class);
                    }
                    catch (System.Exception Error)
                    {
                        if (_class != null)
                            throw new System.Exception("Error on '" + _class.FullName + "' registration.", Error);
                        else
                            throw new System.Exception(Error.Message, Error);

                    }
                }
            }

            var command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand( 
            //"SELECT ID,MetaObjectIdentity FROM ClassBLOBS ",Connection);
            command.CommandText = LoadClassBlobObjectIdentitiesSQLStatement;// "SELECT ID,MetaObjectIdentity FROM ClassBLOBS ";
            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                string MetaObjectIdentity = dataReader["MetaObjectIdentity"] as string;
                if (insertedClassBlobs.ContainsKey(MetaObjectIdentity))
                {
                    DataObjects.ClassBLOB classBLOB = insertedClassBlobs[MetaObjectIdentity];
                    classBLOB.ID = System.Convert.ToInt32(dataReader["ID"]);
                    ClassBLOBs.Add(classBLOB.ID, classBLOB);

                }
            }
            dataReader.Close();

        }
    }
}



//   private void CreateMetaDataTables()
//        {
//            if (Connection.State != System.Data.ConnectionState.Open)
//                Connection.Open();

//            //bool ClassBLOBSExist = false;
//            //bool ObjectBLOBSExist = false;
//            //bool IdentityTableExist = false;

//            //System.Data.SqlClient.SqlCommand Command = new System.Data.SqlClient.SqlCommand("exec sp_tables", Connection);
//            //System.Data.SqlClient.SqlDataReader DataReader = Command.ExecuteReader();
//            //foreach (System.Data.Common.DbDataRecord CurrRecord in DataReader)
//            //{
//            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "ClassBLOBS")
//            //    {
//            //        ClassBLOBSExist = true;
//            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
//            //            break;
//            //    }
//            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "ObjectBLOBS")
//            //    {
//            //        ObjectBLOBSExist = true;
//            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
//            //            break;
//            //    }
//            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "IdentityTable")
//            //    {
//            //        IdentityTableExist = true;
//            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
//            //            break;
//            //    }


//            //}
//            //DataReader.Close();

//            string Query = "CREATE TABLE ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) " +
//                "ALTER TABLE ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY CLUSTERED (ID)"+
//                " CREATE TABLE ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) " +
//                " ALTER TABLE ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY CLUSTERED (ID)"+
//                " CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + _StorageIdentity + "')  ON [PRIMARY]";
//          //System.Data.Common.DbTransaction trans=  Connection.BeginTransaction();
//            //using (System.Transactions.TransactionScope transScoop = new System.Transactions.TransactionScope())
//            //{

//#if !DeviceDotNet 
//                Connection.EnlistTransaction(System.Transactions.Transaction.Current);
//#endif
//                System.Data.Common.DbCommand Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
//                Command.CommandText = "CREATE TABLE ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) ";
//                //Command.Transaction = trans; 
//                Command.ExecuteNonQuery();
//                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
//                Command.CommandText = "ALTER TABLE ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY  (ID)";
//                //Command.Transaction = trans;
//                Command.ExecuteNonQuery();

//                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
//                Command.CommandText = "CREATE TABLE ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) ";
//                //Command.Transaction = trans;
//                Command.ExecuteNonQuery();

//                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
//                Command.CommandText = "ALTER TABLE ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY  (ID)";
//                //Command.Transaction = trans;
//                Command.ExecuteNonQuery();

//                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
//                Command.CommandText = "CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + _StorageIdentity + "')";
//                // Command.Transaction = trans;
//                Command.ExecuteNonQuery();
//            //    transScoop.Complete();
//            //}

//            //trans.Commit();
//            //Connection.Close();







//            //Query = " create procedure sp_SelectObjectBLOBS AS Select ID,ClassBLOBSID,ObjectData from ObjectBLOBS";
//            //Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
//            //Command.ExecuteNonQuery();



//            //if (!ClassBLOBSExist)
//            //{
//            //    string Query = " CREATE TABLE dbo.ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) " +
//            //    "ALTER TABLE dbo.ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY CLUSTERED (ID)";
//            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
//            //    Command.ExecuteNonQuery();
//            //}
//            //if (!ObjectBLOBSExist)
//            //{
//            //    string Query = "CREATE TABLE dbo.ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) " +
//            //        "ALTER TABLE dbo.ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY CLUSTERED (ID)";
//            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
//            //    Command.ExecuteNonQuery();
//            //    Command.CommandText = " create procedure sp_SelectObjectBLOBS AS Select ID,ClassBLOBSID,ObjectData from ObjectBLOBS";
//            //    Command.ExecuteNonQuery();
//            //}

//            //string storageIdentity = System.Guid.NewGuid().ToString();
//            //if (!IdentityTableExist)
//            //{
//            //    string Query = "CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + storageIdentity + "')  ON [PRIMARY]";
//            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
//            //    Command.ExecuteNonQuery();

//            //}
//        }
