namespace OOAdvantech.MSSQLFastPersistenceRunTime.Commands
{
	/// <MetaDataID>{A1646E36-A355-4F5C-A04E-F167DEF62DF2}</MetaDataID>
	public class UpdateStorageInstanceCommand:PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
	{
		public UpdateStorageInstanceCommand (PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef):base(updatedStorageInstanceRef)
		{
		}
		static object[] itemArr = new object[]{null,null,null};
		internal bool  FromNewCommand=false;
		/// <MetaDataID>{6D66B69B-AA5A-4AE6-B84A-826D4871F319}</MetaDataID>
		/// <summary>With this method execute the command. </summary>
		public override void Execute()
		{
			
	
			if(FromNewCommand)
			{

                System.Data.Common.DbConnection SQLConnection = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).Connection;
				string insertStatement="INSERT INTO ObjectBLOBS (ID,ClassBLOBSID,ObjectData) Values(@ID,@ClassBLOBSID,@ObjectData) ";
                System.Data.Common.DbCommand insertSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(insertStatement, SQLConnection);
                insertSqlCommand.CommandText = insertStatement;

                System.Data.Common.DbParameter parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = "@ClassBLOBSID";
                parameter.DbType = System.Data.DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);
                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = "@ID";
                parameter.DbType = System.Data.DbType.Int32;
                insertSqlCommand.Parameters.Add(parameter);

                //System.Data.Common.DbParameter OIDParameter = insertSqlCommand.CreateParameter();
                //OIDParameter.ParameterName = "@OID";
                //OIDParameter.DbType = System.Data.DbType.Int32;
                //insertSqlCommand.Parameters.Add(OIDParameter);

                //insertSqlCommand.Parameters.Add("@ClassBLOBSID",  System.Data.SqlDbType.Int);
                //insertSqlCommand.Parameters.Add("@ID",  System.Data.SqlDbType.Int);
                //System.Data.SqlClient.SqlParameter OIDParameter=insertSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
                //OIDParameter.Direction = System.Data.ParameterDirection.Output;

				System.IO.MemoryStream memoryStream=new System.IO.MemoryStream();

				(UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);
			
				int length=0;
				//if(memoryStream.Length<250)
					//length=250;
				//else
					length=(int)memoryStream.Length;
                    (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).Bytes += length;

				byte[] BLOB=new byte[length];
				memoryStream.Position=0;
				memoryStream.Read(BLOB,0,(int)memoryStream.Length);

                //insertSqlCommand.Parameters.Add("@ObjectData",  System.Data.SqlDbType.Image,length);

                parameter = insertSqlCommand.CreateParameter();
                parameter.ParameterName = "@ObjectData";
                parameter.DbType = System.Data.DbType.Binary;
                parameter.Size = length;
                insertSqlCommand.Parameters.Add(parameter);

				insertSqlCommand.Parameters["@ObjectData"].Value=BLOB;
				insertSqlCommand.Parameters["@ID"].Value=UpdatedStorageInstanceRef.ObjectID;
				insertSqlCommand.Parameters["@ClassBLOBSID"].Value=(UpdatedStorageInstanceRef as StorageInstanceRef).SerializationMetada.ID;

				/*itemArr[0]=UpdatedStorageInstanceRef.ObjectID;
				itemArr[1]=(UpdatedStorageInstanceRef as StorageInstanceRef).SerializationMetada.ID;
				itemArr[2]=BLOB;
				(UpdatedStorageInstanceRef.ActiveStorageSession as StorageSession).Engine.AddItem(itemArr) ;*/

				
				if(SQLConnection.State!=System.Data.ConnectionState.Open)
					SQLConnection.Open();
                //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
				//TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
				//SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
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
					//System.Diagnostics.Debug.WriteLine("ExecuteNonQuery");
			
				}
				#if DEBUG
				catch(System.Exception Error)
				{
					throw new System.Exception(Error.Message,Error);
				}
				#endif
				finally
				{
				}

			}
			else
			{
				bool hasChangeState=false;
				foreach(RelResolver relResolver in UpdatedStorageInstanceRef.RelResolvers)
				{
					if(HasChanges(relResolver))
						hasChangeState=true;

				}

				if(!hasChangeState&&!UpdatedStorageInstanceRef.HasChangeState())
					return;

				//TODO: Ο τρόπος που χρησιμοποιείται η SQLConnection δεν είναι thread safe;
                System.Data.Common.DbConnection SQLConnection = (UpdatedStorageInstanceRef.ObjectStorage as AdoNetObjectStorage).Connection;

				string updateStatement="UPDATE ObjectBLOBS SET ObjectData=@ObjectData WHERE ID=@OID";

                System.Data.Common.DbCommand updateSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(updateStatement,SQLConnection) ;
                updateSqlCommand.CommandText = updateStatement;

                System.Data.Common.DbParameter objectDataParameter = updateSqlCommand.CreateParameter();// updateSqlCommand.Parameters.Add("@ObjectData", System.Data.SqlDbType.Image);
                objectDataParameter.ParameterName = "@ObjectData";
                objectDataParameter.DbType = System.Data.DbType.Binary;
                updateSqlCommand.Parameters.Add(objectDataParameter);
                System.Data.Common.DbParameter OIDParameter = updateSqlCommand.CreateParameter();//updateSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
                OIDParameter.ParameterName = "@OID";
                OIDParameter.DbType = System.Data.DbType.Int32;
                updateSqlCommand.Parameters.Add(OIDParameter);

				OIDParameter.Value=UpdatedStorageInstanceRef.ObjectID;
				System.IO.MemoryStream memoryStream=new System.IO.MemoryStream();
				(UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);
				byte[] BLOB=new byte[memoryStream.Length];
				memoryStream.Position=0;
				memoryStream.Read(BLOB,0,(int)memoryStream.Length);
				objectDataParameter.Value=BLOB;

				if(SQLConnection.State!=System.Data.ConnectionState.Open)
					SQLConnection.Open();
                //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
				//TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
				//SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
				try
				{
					updateSqlCommand.ExecuteNonQuery();
				
				}
#if DEBUG
				catch(System.Exception Error)
				{
					throw new System.Exception(Error.Message,Error);
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
			if(relResolver.AssociationEnd.Multiplicity.IsMany)// associationEndFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||associationEndFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
			{
                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)associationEndFastFieldAccessor.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
				if(theObjectContainer==null)
					throw new System.Exception("The collection object "+UpdatedStorageInstanceRef.Class.FullName+"."+relResolver.AssociationEnd.Name+" has loose the connection with storage.");
                PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;
				if(mObjectCollection==null||mObjectCollection.RelResolver!=relResolver)
					throw new System.Exception("The collection object "+UpdatedStorageInstanceRef.Class.FullName+"."+relResolver.AssociationEnd.Name+" has loose the connection with storage.");
				
				return mObjectCollection.HasChanges;
			}
			else
			{
				//object NewValue=associationEndFieldInfo.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                object NewValue = Member<object>.GetValue(associationEndFastFieldAccessor.GetValue, UpdatedStorageInstanceRef.MemoryInstance);
				object OldValue=relResolver.RelatedObject;

				if(NewValue!=OldValue)
					return true;
				return false;
			}
			
		}
	}
}
