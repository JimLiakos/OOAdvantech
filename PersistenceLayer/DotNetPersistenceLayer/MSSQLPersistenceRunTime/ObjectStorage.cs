using OOAdvantech.RDBMSDataObjects;
namespace OOAdvantech.MSSQLPersistenceRunTime
{
	/// <MetaDataID>{CB25317A-EFDC-4CB2-A5D7-A31C1F91FA77}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{CB25317A-EFDC-4CB2-A5D7-A31C1F91FA77}")]
	[MetaDataRepository.Persistent("<ExtMetaData><RDBMSInheritanceMapping>OneTablePerConcreteClass</RDBMSInheritanceMapping></ExtMetaData>")]
	public class Storage : RDBMSMetaDataRepository.Storage
	{
		/// <MetaDataID>{C04BE9D6-F5DD-4980-ADE0-4306FA4B0A91}</MetaDataID>
		Storage()
		{
			MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator=new RDBMSMetaDataRepository.MetaObjectsStack();
            //_StorageType = "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider";
		}


        public Storage(string storageName, string storageLocation,string storageType,RDBMSDataObjects.DataBase dataBase)
        { 
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
            _StorageName = storageName;
            _StorageLocation = storageLocation; 
            _StorageType = storageType;
            _StorageDataBase = dataBase;
            dataBase.Storage = this;

        }

     

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{89977103-8713-4F33-862A-C3E5B90F5182}</MetaDataID>
		private DataBase _StorageDataBase;
		/// <MetaDataID>{B70D7637-4B6C-41AB-B0DF-EAB9D9CD354B}</MetaDataID>
		public DataBase StorageDataBase
		{
			get
			{
                //if(_StorageDataBase==null)
                //    _StorageDataBase=GetDataBase(_StorageLocation, _StorageName);
				return _StorageDataBase;
			}
            set
            {
                _StorageDataBase = value;
                value.Storage = this;
            }
		}

        PersistenceLayerRunTime.ObjectStorage _MetadataStorage = null;
        internal PersistenceLayerRunTime.ObjectStorage MetadataStorage
        {
            get
            {
                if(_MetadataStorage ==null)
                    _MetadataStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this.Properties) as PersistenceLayerRunTime.ObjectStorage;
                return _MetadataStorage;
            }
        }
	

		
		/// <MetaDataID>{8EE847F7-21C3-4DA3-8453-A96BE14225C8}</MetaDataID>
		void Build()
		{
			//TODO: υπάρχει πρόβλημα όταν υπάρχει class σε δύο διαφορετικά namespaces με το ιδιο όνομα
			UpdateSchema();
		
		}

        /// <MetaDataID>{276C7983-B380-4CD2-B887-0306D331384B}</MetaDataID>
        public void RegisterComponent(MetaDataRepository.Component Component)
        {
            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {

                try
                {
                    if (_StorageIdentity == null)
                    {
                        _StorageIdentity = System.Guid.NewGuid().ToString();
                        PersistenceLayer.ObjectStorage.CommitObjectState(this);
                    }

                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();

                    RDBMSMetaDataRepository.Component mComponent = null;
                    mComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(Component, this) as RDBMSMetaDataRepository.Component;


                    //					PersistenceLayer.ObjectStorage objectStorage=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
                    //					PersistenceLayer.StructureSet aStructureSet=objectStorage.Execute("SELECT Components FROM "+typeof(MetaDataRepository.MetaObject).FullName+" Components WHERE MetaObjectIDStream = \""+Component.Identity.ToString()+"\" ");

                    //					foreach( RDBMSMetaDataRepository.Component CurrComponent  in Components)
                    //					{								
                    //						if(CurrComponent.Identity==Component.Identity)
                    //						{
                    //							mComponent=CurrComponent;
                    //							break;
                    //						}
                    //					}

                    if (mComponent == null)
                    {
                        mComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(Component, this);
                        mComponent.Context = this;
                        _Components.Add(mComponent);
                    }

                    System.Collections.Hashtable dependencies = new System.Collections.Hashtable();
                    GetAllDependencies(ref dependencies, Component);

                    foreach (System.Collections.DictionaryEntry entry in dependencies)
                    {
                        MetaDataRepository.Component referenceComponent = entry.Value as MetaDataRepository.Component;
                        RDBMSMetaDataRepository.Component rdbmsReferenceComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(referenceComponent, this) as RDBMSMetaDataRepository.Component;
                        if (rdbmsReferenceComponent == null)
                        {
                            rdbmsReferenceComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(referenceComponent, this);
                            rdbmsReferenceComponent.Context = this;
                            _Components.Add(rdbmsReferenceComponent);
                        }
                    }

                    mComponent.Synchronize(Component);

                    string myName = (string)mComponent.GetPropertyValue(typeof(string), "Persosnal", "Myname");
                    mComponent.PutPropertyValue("Persosnal", "Myname", "mitsos");

                    MetaDataRepository.SynchronizerSession.StopSynchronize();

                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();

                    mComponent.BuildMappingElement(this);

                    ///}
                    //catch(System.Exception Error)
                    //{


                    //}
                    MetaDataRepository.SynchronizerSession.StopSynchronize();

                    StateTransition.Consistent = true; ;
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
            }

        }
		/// <MetaDataID>{5999A140-4A44-47AF-A00F-8FF88343ACC9}</MetaDataID>
		public override void RegisterComponent(string[] assembliesFullNames)
		{

			System.Collections.ArrayList components=new System.Collections.ArrayList();
			foreach(string Component in assembliesFullNames)
			{
				System.Reflection.Assembly dotNetAssembly=System.Reflection.Assembly.Load(Component);
				object[] objects=dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata),false);
				if(objects.Length==0)
					throw new System.Exception("You must declare in assemblyInfo file of  '"+dotNetAssembly.FullName+" the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute"); 

				DotNetMetaDataRepository.Assembly mAssembly=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
				if(mAssembly==null)
					mAssembly=new DotNetMetaDataRepository.Assembly(dotNetAssembly);
				System.Collections.ArrayList errors=new System.Collections.ArrayList();
				bool hasErrors=mAssembly.ErrorCheck(ref errors);
				if(hasErrors)
				{
					string ErrorMessage=null;
					foreach(MetaDataError error in errors)
					{
						if(ErrorMessage!=null)
							ErrorMessage+="\n";
						ErrorMessage+=error.ErrorMessage;
					}
					throw new System.Exception(ErrorMessage);
				}
				components.Add(mAssembly);
			}


			using(Transactions.SystemStateTransition stateTransition=new OOAdvantech.Transactions.SystemStateTransition())
			{
				foreach(DotNetMetaDataRepository.Assembly  component in components)
					RegisterComponent(component);
				Build();
				stateTransition.Consistent=true;
			}


		}
		/// <MetaDataID>{C9620952-390E-4374-8E7F-672767DCA872}</MetaDataID>
		public override void RegisterComponent(string Component)
		{ 
			//TODO Error prone  εάν περάσει λάθος string ...
			System.Reflection.Assembly dotNetAssembly=System.Reflection.Assembly.Load(Component);
			object[] objects=dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata),false);
			if(objects.Length==0)
				throw new System.Exception("You must declare in assemblyInfo file of  '"+dotNetAssembly.FullName+" the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute"); 

			DotNetMetaDataRepository.Assembly mAssembly=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
			if(mAssembly==null)
				mAssembly=new DotNetMetaDataRepository.Assembly(dotNetAssembly);
			System.Collections.ArrayList errors=new System.Collections.ArrayList();
            bool hasErrors = mAssembly.ErrorCheck(ref errors);
            if (hasErrors)
            {
                string ErrorMessage = null;
                foreach (MetaDataError error in errors)
                {
                    if (ErrorMessage != null)
                        ErrorMessage += "\n";
                    ErrorMessage += error.ErrorMessage;
                }
                throw new System.Exception(ErrorMessage);
            }
			using(Transactions.SystemStateTransition stateTransition=new OOAdvantech.Transactions.SystemStateTransition())
			{

				RegisterComponent(mAssembly);
				Build();
				stateTransition.Consistent=true;
			}
		}



		/// <MetaDataID>{8B17003C-DCFC-4982-A521-9D9FEA1CBA22}</MetaDataID>
		//internal System.ServiceProcess.ServiceController SQLServices; //MSSQLSERVER
	//	/// <MetaDataID>{0A284092-1E22-4464-ACCB-DB0927DFF3CA}</MetaDataID>
	//	internal SQLDMO.SQLServer mSQLServer;
		/// <MetaDataID>{D1B03755-4047-4BC1-8A74-136A89B27146}</MetaDataID>
	//	internal SQLDMO.Database mDatabase;

		
        ///// <MetaDataID>{BDA64E3B-8A51-4ADA-AD39-4069AA104D02}</MetaDataID>
        //void CreateDataBase(string ServerName,string DataBaseName)
        //{
        //    string ConnectionString = "Integrated Security=True;Initial Catalog=master;Data Source=" + ServerName + @"\SQLExpress";
        //    System.Data.SqlClient.SqlConnection Connection=new System.Data.SqlClient.SqlConnection(ConnectionString);
        ////	System.Data.SqlClient.SqlTransaction Transaction;
        //    try
        //    {
        //        Connection.Open();
        //    }
        //    catch(System.Exception Error)
        //    {
        //        throw new System.Exception("The storage location "+ServerName+" can't be accessed.",Error);
        //    }
	
        //    System.Data.SqlClient.SqlCommand Command=new System.Data.SqlClient.SqlCommand("CREATE DATABASE "+DataBaseName  ,Connection);
        //    string CommandText=null;
        //    try
        //    {

        //        Command.ExecuteNonQuery();
        //        Connection.Close();
        //        //Connection.ConnectionString = "Integrated Security=True;Initial Catalog=" + DataBaseName + ";Data Source=" + ServerName + @"\SQLExpress";
        //        //Connection.Open();


        //        //Transaction= Connection.BeginTransaction();
        //        //Command.Connection=Connection;
        //        //Command.Transaction=Transaction;
        //        //CommandText="CREATE TABLE MetaDataTable("+
        //        //    "ID int NOT NULL,"+
        //        //    "MetaData image NULL)  "+
        //        //    "ON [PRIMARY]	 TEXTIMAGE_ON [PRIMARY] "+
        //        //    "CREATE TABLE T_GlobalObjectCollectionIDs ("+
        //        //    "InStoragelID int NOT NULL  ,"+
        //        //    "ObjectCollectionID binary (20) NOT NULL, "+
        //        //    "OutStorageID int NOT NULL  "+
        //        //    ") ON [PRIMARY] ";


        //    }
        //    catch(System.Exception Error)
        //    {
        //        Connection.Close();
        //        if(StorageDataBase!=null)
        //            throw new System.Exception("DataBase with name '"+DataBaseName+"' already exist");
        //        else
        //            throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //    }
        //    //try
        //    //{
        //    //    Command.CommandText=CommandText;
        //    //    Command.ExecuteNonQuery();

        //    //    CommandText="CREATE Procedure dbo.UpdateMetaData "+
        //    //        "@MetaData image "+
        //    //        "AS "+
        //    //        "UPDATE    MetaDataTable SET MetaData =@MetaData  WHERE     (ID = 1) ";
						
        //    //    Command.CommandText=CommandText;
        //    //    Command.ExecuteNonQuery();

        //    //    Transaction.Commit();

        //    //}
        //    //catch(System.Exception Error)
        //    //{
        //    //    Connection.Close();
        //    //    throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //    //}
        //    //Connection.Close();
        //    if(StorageDataBase==null)
        //        throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //}

	
		
//        /// <MetaDataID>{189CA0C4-1E58-4B07-BD5E-8386D2853B36}</MetaDataID>
//        internal void DatabaseConnect(bool Create)
//        {
//            if(StorageDataBase==null&&!Create)
//                throw new System.Exception("The storage with name '"+_StorageName+"' doesn't exist.");
			

//            if(StorageDataBase==null&&Create)
//                CreateDataBase( _StorageLocation,_StorageName);

//            return;
///*
//            try
//            {
//                if(mSQLServer==null)
//                    throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.");
//                SQLDMO.SQLDMO_SVCSTATUS_TYPE Status=mSQLServer.Status;
//                if(Status!=SQLDMO.SQLDMO_SVCSTATUS_TYPE.SQLDMOSvc_Running)
//                    throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.");
//            }
//            catch(System.Runtime.InteropServices.ExternalException Error)
//            {
//                throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.",Error);
//            }

//            foreach(SQLDMO.Database CurrDatabase in mSQLServer.Databases)
//            {
//                if(CurrDatabase.Name==_StorageName)
//                {
//                    mDatabase=CurrDatabase;
//                    break;
//                }
//            }
//            if(mDatabase==null&&Create)
//            {
//                mDatabase=new SQLDMO.DatabaseClass();
//                mDatabase.Name=_StorageName;
//                mSQLServer.Databases.Add(mDatabase);
//            }*/
//        }
        ///// <MetaDataID>{A8E5101B-13F7-44E8-B1B1-571FBF5BF533}</MetaDataID>
        //internal bool ServerContainsDatabase(string DatabaseName)
        //{
        //    if(GetDataBase(_StorageLocation, _StorageName)==null)
        //        return false;
        //    else
        //        return true;
        //}


	
		/// <MetaDataID>{8B388409-FC2E-438C-BD0C-81CB940DE717}</MetaDataID>
		public void UpdateDataBaseMetadata()
		{
			using (Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
			{

				MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator=new MetaObjectsStack();
				MetaDataRepository.SynchronizerSession.StartSynchronize();
				(MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase=StorageDataBase;
				try
				{
					StorageDataBase.Update();
				}
				catch(System.Exception Error)
				{
					throw new System.Exception(Error.Message,Error);
				}
				finally
				{

					(MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase=null;
					MetaDataRepository.SynchronizerSession.StopSynchronize();
					bool throwexception=false;
					if(throwexception)
					{
						throw new System.Exception("Liakos");
					}

					
					StateTransition.Consistent=true;
					MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator=new RDBMSMetaDataRepository.MetaObjectsStack();
				}
			}
		}
	//	/// <MetaDataID>{710A6B05-5540-4453-BBC4-8239E64CAABE}</MetaDataID>
	//	private COMPlusTransaction TransactionObject=null;
		/// <MetaDataID>{D760F717-623E-41B0-803B-A0D854F574DA}</MetaDataID>
		public void UpdateSchema()
		{
			// Error prone να τσεκαριστή όταν δεν καλλειται απο new storage

			//DatabaseConnect(true);
			object result=null;
#if! NETCompactFramework 
	        using(System.Transactions.TransactionScope transactionScope=new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
            {
                UpdateDataBaseMetadata();
                transactionScope.Complete();
            }
#else
                UpdateDataBaseMetadata();
#endif


            /*
			if(TransactionObject==null)
				TransactionObject=new COMPlusTransaction();
			try
			{
				//UpdateDataBaseMetadata();
				DatabaseConnect(true);
				TransactionObject.UpdateDataBaseMetadata(this);
			}
			catch(System.Exception Error)
			{
				int k=0;

			}*/
			return;

/*
			DatabaseTables=new System.Collections.Specialized.HybridDictionary();
			foreach(SQLDMO.Table CurrTable in mDatabase.Tables)
				DatabaseTables.Add(CurrTable.Name,CurrTable);
			UpdateTables();
			UpdateKeys();
			UpdateStoreProcedures();
			UpdateViews();
			DatabaseTables=null;
			AppPersistencyContext.CommitObjectStateTransition(this,ObjectStateTransitionID);*/
		}
	
		void GetAllDependencies(ref System.Collections.Hashtable  dependencies, MetaDataRepository.Component component	)
		{
			
			foreach(MetaDataRepository.Dependency dependency in component.ClientDependencies)
			{
				if(!dependencies.Contains(dependency.Supplier))
				{
                    object[] objects = (dependency.Supplier as DotNetMetaDataRepository.Assembly).WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                    if (objects.Length == 0)
                        continue;

					dependencies.Add(dependency.Supplier,dependency.Supplier);
					GetAllDependencies(ref dependencies,dependency.Supplier as MetaDataRepository.Component);
				}
			}
		}


	
	
	
	}
}
