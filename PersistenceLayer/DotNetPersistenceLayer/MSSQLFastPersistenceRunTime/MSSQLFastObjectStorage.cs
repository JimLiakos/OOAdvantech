namespace  OOAdvantech.MSSQLFastPersistenceRunTime
{
	
	/// <MetaDataID>{02E16727-2D67-4E2D-B1C0-58B42257A7D2}</MetaDataID>
	public class Storage : OOAdvantech.MetaDataRepository.Storage
    {
        static internal System.Collections.Generic.Dictionary<string, string> BackwardCompatibilities = new System.Collections.Generic.Dictionary<string, string>();
        static Storage()
        {
            BackwardCompatibilities.Add("OOAdvantech, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643",
                                        "OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643");
        }
        //public override object CreateDataLoader(object dataNode, object searchCondition, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells)
        //{
        //    return null;
        //    //return new ObjectQueryLanguage.DataLoader(dataNode as MetaDataRepository.ObjectQueryLanguage.DataNode,searchCondition as MetaDataRepository.ObjectQueryLanguage.SearchCondition, storageCells);
        //}

		protected Storage()
		{

            _StorageType = "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider";

		}
		/// <MetaDataID>{A6851F21-6CDB-4ACB-A4B1-0F50215FC28B}</MetaDataID>
		public Collections.Map ClassBLOBs=new OOAdvantech.Collections.Map();


        readonly System.Data.Common.DbConnection Connection;

        private void CreateMetaDataTables()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

            //bool ClassBLOBSExist = false;
            //bool ObjectBLOBSExist = false;
            //bool IdentityTableExist = false;

            //System.Data.SqlClient.SqlCommand Command = new System.Data.SqlClient.SqlCommand("exec sp_tables", Connection);
            //System.Data.SqlClient.SqlDataReader DataReader = Command.ExecuteReader();
            //foreach (System.Data.Common.DbDataRecord CurrRecord in DataReader)
            //{
            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "ClassBLOBS")
            //    {
            //        ClassBLOBSExist = true;
            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
            //            break;
            //    }
            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "ObjectBLOBS")
            //    {
            //        ObjectBLOBSExist = true;
            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
            //            break;
            //    }
            //    if (CurrRecord["TABLE_TYPE"].ToString() == "TABLE" && CurrRecord["TABLE_NAME"].ToString() == "IdentityTable")
            //    {
            //        IdentityTableExist = true;
            //        if (ClassBLOBSExist && ObjectBLOBSExist && IdentityTableExist)
            //            break;
            //    }


            //}
            //DataReader.Close();

            string Query = "CREATE TABLE ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) " +
                "ALTER TABLE ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY CLUSTERED (ID)"+
                " CREATE TABLE ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) " +
                " ALTER TABLE ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY CLUSTERED (ID)"+
                " CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + _StorageIdentity + "')  ON [PRIMARY]";
          //System.Data.Common.DbTransaction trans=  Connection.BeginTransaction();
            //using (System.Transactions.TransactionScope transScoop = new System.Transactions.TransactionScope())
            //{

#if !NETCompactFramework 
                Connection.EnlistTransaction(System.Transactions.Transaction.Current);
#endif
                System.Data.Common.DbCommand Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
                Command.CommandText = "CREATE TABLE ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) ";
                //Command.Transaction = trans; 
                Command.ExecuteNonQuery();
                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
                Command.CommandText = "ALTER TABLE ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY  (ID)";
                //Command.Transaction = trans;
                Command.ExecuteNonQuery();

                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
                Command.CommandText = "CREATE TABLE ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) ";
                //Command.Transaction = trans;
                Command.ExecuteNonQuery();

                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
                Command.CommandText = "ALTER TABLE ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY  (ID)";
                //Command.Transaction = trans;
                Command.ExecuteNonQuery();

                Command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(Query, Connection);
                Command.CommandText = "CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + _StorageIdentity + "')";
                // Command.Transaction = trans;
                Command.ExecuteNonQuery();
            //    transScoop.Complete();
            //}

            //trans.Commit();
            //Connection.Close();


            




            //Query = " create procedure sp_SelectObjectBLOBS AS Select ID,ClassBLOBSID,ObjectData from ObjectBLOBS";
            //Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
            //Command.ExecuteNonQuery();



            //if (!ClassBLOBSExist)
            //{
            //    string Query = " CREATE TABLE dbo.ClassBLOBS(ID  int NOT NULL IDENTITY (1,1) ,MetaObjectIdentity nvarchar(255),ClassData image NULL) " +
            //    "ALTER TABLE dbo.ClassBLOBS ADD CONSTRAINT PK_ClassBLOBS PRIMARY KEY CLUSTERED (ID)";
            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
            //    Command.ExecuteNonQuery();
            //}
            //if (!ObjectBLOBSExist)
            //{
            //    string Query = "CREATE TABLE dbo.ObjectBLOBS(ID  int NOT NULL ,ClassBLOBSID int,ObjectData image NULL) " +
            //        "ALTER TABLE dbo.ObjectBLOBS ADD CONSTRAINT PK_ObjectBLOBS PRIMARY KEY CLUSTERED (ID)";
            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
            //    Command.ExecuteNonQuery();
            //    Command.CommandText = " create procedure sp_SelectObjectBLOBS AS Select ID,ClassBLOBSID,ObjectData from ObjectBLOBS";
            //    Command.ExecuteNonQuery();
            //}

            //string storageIdentity = System.Guid.NewGuid().ToString();
            //if (!IdentityTableExist)
            //{
            //    string Query = "CREATE TABLE IdentityTable (NEXTID int NOT NULL,StorageIdentity nvarchar(255) NOT NULL	DEFAULT '" + storageIdentity + "')  ON [PRIMARY]";
            //    Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
            //    Command.ExecuteNonQuery();

            //}
        }

		/// <MetaDataID>{93B64DF4-E3DE-4BEC-A1BF-47A91945F856}</MetaDataID>
        public Storage(string storageName, string storageLocation,System.Data.Common.DbConnection connection,bool newStorage )
		{
            _StorageType = "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider";
			_StorageName=storageName;
			_StorageLocation=storageLocation; 
            Connection = connection;

          //  System.Transactions.Transaction mytrans = System.Transactions.Transaction.Current;
           // using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                try
                {
                    //using (System.Transactions.TransactionScope trans = new System.Transactions.TransactionScope())
                    //{

                        if (Connection.State != System.Data.ConnectionState.Open)
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
                            string query = "SELECT DISTINCT StorageIdentity   FROM  IdentityTable";
                            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(query, Connection);
                            command.CommandText = query;
                            System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
                            while(dataReader.Read())
                            {
                                _StorageIdentity = dataReader["StorageIdentity"] as string;
                                break;
                            }
                            dataReader.Close();
                        }
                        //double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                    //    trans.Complete();
                    //}
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

            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
            
            try
			{
			
				string query="SELECT ID, MetaObjectIdentity, ClassData FROM ClassBLOBS";
                System.Data.Common.DbCommand SqlCommand = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(query, Connection);
                SqlCommand.CommandText = query;
                //double tim = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
				System.Data.Common.DbDataReader dataReader=SqlCommand.ExecuteReader();
                //double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
				while( dataReader.Read())
				{
                    int ID = (int)dataReader["ID"];
                    string MetaObjectIdentity = dataReader["MetaObjectIdentity"] as string;
                    byte[] ClassData = (byte[])dataReader["ClassData"];
					int offset=4;
					DataObjects.ClassBLOB classBLOB=new DataObjects.ClassBLOB(ClassData,offset);  
					ClassBLOBs.Add(ID,classBLOB);
					classBLOB.ID=ID;
					int erera=0;
				}
                dataReader.Close();
			}
			catch(System.Exception Error)
			{
				throw new System.Exception(Error.Message,Error);
			}

            //double tim3 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;

		}
		/// <MetaDataID>{F1E7EBE9-3723-40B7-BB83-932A69D70653}</MetaDataID>
		internal DataObjects.ClassBLOB GetClassBLOB(int classBLOBID)
		{
			return ClassBLOBs[classBLOBID] as DataObjects.ClassBLOB;
		}
		/// <MetaDataID>{507A9A07-9DAD-498D-A875-D5CEE60D0840}</MetaDataID>
		private DataObjects.ClassBLOB GetClassBLOBIfExist(DotNetMetaDataRepository.Class _class)
		{
			if(_class==null)
				return null;
			foreach(System.Collections.DictionaryEntry entry in ClassBLOBs)
			{
				DataObjects.ClassBLOB classBLOB=entry.Value as  DataObjects.ClassBLOB;
				if(classBLOB.Class==_class)
					return classBLOB;
			}
			return null;
		}
		/// <MetaDataID>{A02C227C-B479-44D1-8893-CA0F45E5D4F7}</MetaDataID>
		internal DataObjects.ClassBLOB GetClassBLOB(DotNetMetaDataRepository.Class _class)
		{
			DataObjects.ClassBLOB classBLOB=GetClassBLOBIfExist(_class);
			if(classBLOB!=null)
				return classBLOB;
			else
				throw new System.Exception("There isn't metada for class \""+_class.FullName+"\". Register the assembly of class and try again.");
		}






		/// <MetaDataID>{24EC9E67-4FC5-4067-94CC-F7A2E7670D8D}</MetaDataID>
		public override void RegisterComponent(string[] assembliesFullNames)
		{
			DotNetMetaDataRepository.Assembly mAssembly=null;
			System.Collections.ArrayList components=new System.Collections.ArrayList();
			foreach(string Component in assembliesFullNames)
			{
				System.Reflection.Assembly dotNetAssembly=System.Reflection.Assembly.Load(Component);
				object[] objects=dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata),false);
				if(objects.Length==0)
					throw new System.Exception("You must declare in assemblyInfo file of  '"+dotNetAssembly.FullName+" the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute"); 

				mAssembly=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
				if(mAssembly==null)
					mAssembly=new DotNetMetaDataRepository.Assembly(dotNetAssembly);

				System.Collections.ArrayList errors=new System.Collections.ArrayList();
				bool hasErrors=mAssembly.ErrorCheck(ref errors);
				if(hasErrors)
				{
					string ErrorMessage=null;
					foreach(MetaDataRepository.MetaObject.MetaDataError error in errors)
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
				stateTransition.Consistent=true;
			}

		}
		/// <MetaDataID>{30A9F0CD-2364-4BA8-830B-177660770661}</MetaDataID>
		private void GetReferenceToComponents(MetaDataRepository.Component Component, System.Collections.ArrayList components)
		{
		
			foreach(MetaDataRepository.Dependency dependency in Component.ClientDependencies)
			{
				MetaDataRepository.Component  refComponent= dependency.Supplier as MetaDataRepository.Component;

                
				if(!components.Contains(refComponent))
				{
                    object[] objects = (refComponent as DotNetMetaDataRepository.Assembly).WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                    if (objects.Length == 0)
                        continue;
                    components.Add(refComponent);
					GetReferenceToComponents(refComponent,components);
				}
			}
		}

		/// <MetaDataID>{DB9584CB-EABD-4EEC-8390-8DAE65B50E2C}</MetaDataID>
		public override void RegisterComponent(string assemblyFullName)
		{
			DotNetMetaDataRepository.Assembly mAssembly=null;
			System.Reflection.Assembly dotNetAssembly=System.Reflection.Assembly.Load(assemblyFullName);
			object[] objects=dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata),false);
			if(objects.Length==0)
				throw new System.Exception("You must declare in assemblyInfo file of  '"+dotNetAssembly.FullName+" the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute"); 

			mAssembly=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
			if(mAssembly==null)
				mAssembly=new DotNetMetaDataRepository.Assembly(dotNetAssembly);

			System.Collections.ArrayList errors=new System.Collections.ArrayList();
#if !NETCompactFramework 
            bool hasErrors=mAssembly.ErrorCheck(ref errors);
            if(hasErrors)
            {
                string ErrorMessage=null;
                foreach(MetaDataRepository.MetaObject.MetaDataError error in errors)
                {
                    if(ErrorMessage!=null)
                        ErrorMessage+="\n";
                    ErrorMessage+=error.ErrorMessage;
                }
                throw new System.Exception(ErrorMessage);
            }
#endif
			using(Transactions.SystemStateTransition stateTransition=new OOAdvantech.Transactions.SystemStateTransition())
			{
				RegisterComponent(mAssembly);
				stateTransition.Consistent=true;
			}


		}

		

		/// <MetaDataID>{B734A634-10F7-44ED-BEF3-E484849DA8C6}</MetaDataID>
		public void RegisterComponent(MetaDataRepository.Component Component)
		{

            
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

			string insertStatement="INSERT INTO ClassBLOBS (ClassData,MetaObjectIdentity) Values( @ClassData,@MetaObjectIdentity ) ";//+
				//"SELECT ClassBLOBS.ID FROM ClassBLOBS WHERE MetaObjectIdentity=@MetaObjectIdentity";


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
            //insertSqlCommand.Parameters.Add("@ClassData",  System.Data.SqlDbType.b.Image);
            //insertSqlCommand.Parameters.Add("@MetaObjectIdentity",  System.Data.SqlDbType.NVarChar);

			string updateStatement="UPDATE ClassBLOBS SET ClassData = @ClassData WHERE MetaObjectIdentity = @MetaObjectIdentity";
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
            //updateSqlCommand.Parameters.Add("@ClassData",  System.Data.SqlDbType.Image);
            //updateSqlCommand.Parameters.Add("@MetaObjectIdentity",  System.Data.SqlDbType.NVarChar);

			
			
			

			byte[] byteStream=new byte[65536];
			int offset=4;

			System.Collections.ArrayList components=new System.Collections.ArrayList();
			GetReferenceToComponents(Component,components);
			components.Add(Component);
            System.Collections.Generic.Dictionary<string, DataObjects.ClassBLOB> insertedClassBlobs = new System.Collections.Generic.Dictionary<string, OOAdvantech.MSSQLFastPersistenceRunTime.DataObjects.ClassBLOB>();

			foreach(MetaDataRepository.Component _Component in components)
			{
                if (_Component.Identity.ToString() == typeof(DotNetMetaDataRepository.Assembly).Assembly.FullName)
                    continue;
 
				foreach(MetaDataRepository.MetaObject metaObject in  _Component.Residents)
				{
                    
                    if (metaObject.Namespace != null && metaObject.FullName!="OOAdvantech.MetaDataRepository.Namespace"&&
                        metaObject.FullName!="OOAdvantech.MetaDataRepository.MultiplicityRange"&&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Enumeration" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Realization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AssociationEndRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.AttributeRealization" &&
                        metaObject.FullName != "OOAdvantech.MetaDataRepository.Parameter" &&
                        (metaObject.Namespace.FullName == "OOAdvantech.RDBMSDataObjects"||
                        metaObject.Namespace.FullName == "OOAdvantech.MetaDataRepository"))
                        continue;
					offset=4;
                    
					DotNetMetaDataRepository.Class _class=  metaObject as DotNetMetaDataRepository.Class;
					try
					{
						if(_class!=null&&_class.Persistent)
						{

					
							DataObjects.ClassBLOB classBLOB=GetClassBLOBIfExist(_class);
							if(classBLOB==null)
							{
								classBLOB=new OOAdvantech.MSSQLFastPersistenceRunTime.DataObjects.ClassBLOB(_class);
								classBLOB.Serialize(byteStream,offset,out offset);
								int nextpos=0;
                                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset - 4, byteStream, 0, ref nextpos, true);
								byte[] outByteStream=new byte[offset];

								for(int i=0;i!=offset;i++)
									outByteStream[i]=byteStream[i];

								insertSqlCommand.Parameters["@ClassData"].Value=outByteStream;
                              //  insertSqlCommand.Parameters["@ClassData"].Size = outByteStream.Length;
								insertSqlCommand.Parameters["@MetaObjectIdentity"].Value=_class.Identity.ToString();

                                insertedClassBlobs.Add(_class.Identity.ToString(), classBLOB);
                                insertSqlCommand.ExecuteNonQuery();

                                //System.Data.SqlClient.SqlDataReader sqlDataReader=insertSqlCommand.ExecuteReader();
                                //sqlDataReader.Read();
                                //int ID= (int)sqlDataReader["ID"];
                                //sqlDataReader.Close();
                                //classBLOB.ID=ID;
                                //ClassBLOBs.Add(ID,classBLOB);
							}
							else
							{
								if(classBLOB.HasChange)
								{
									classBLOB.Serialize(byteStream,offset,out offset);
									int nextpos=0;
                                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset - 4, byteStream, 0, ref nextpos, true);
									byte[] outByteStream=new byte[offset];
									for(int i=0;i!=offset;i++)
										outByteStream[i]=byteStream[i];

									updateSqlCommand.Parameters["@ClassData"].Value=outByteStream;
                                  //  updateSqlCommand.Parameters["@ClassData"].Size = outByteStream.Length;
									updateSqlCommand.Parameters["@MetaObjectIdentity"].Value=_class.Identity.ToString();
									int res=updateSqlCommand.ExecuteNonQuery();
									classBLOB.HasChange=false;
								}

							}
						}
					}
					catch(System.Exception Error)
					{
						if(_class!=null)
							throw new System.Exception("Error on '"+_class.FullName+"' registration.",Error);
						else
							throw new System.Exception(Error.Message,Error);

					}
				}
			}

            System.Data.Common.DbCommand command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand( 
                    //"SELECT ID,MetaObjectIdentity FROM ClassBLOBS ",Connection);
            command.CommandText = "SELECT ID,MetaObjectIdentity FROM ClassBLOBS ";
            System.Data.Common.DbDataReader dataReader=command.ExecuteReader();
            while(dataReader.Read())
            {
                string MetaObjectIdentity= dataReader["MetaObjectIdentity"] as string;
                if (insertedClassBlobs.ContainsKey(MetaObjectIdentity))
                {
                    DataObjects.ClassBLOB classBLOB = insertedClassBlobs[MetaObjectIdentity];
                    classBLOB.ID = (int)dataReader["ID"];
                    ClassBLOBs.Add(classBLOB.ID, classBLOB);

                }
            }
            dataReader.Close();

		}
	}
}
