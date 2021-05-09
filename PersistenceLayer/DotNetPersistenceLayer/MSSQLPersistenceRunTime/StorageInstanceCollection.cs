namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{D59BBBB6-9BB7-42C1-9EBC-AA54B12FB5CA}</MetaDataID>
	class StorageInstanceCollection
	{
		/// <MetaDataID>{5DA145F2-35E9-4D65-8F59-B8E73CD5D781}</MetaDataID>
		public object GetObject(int IntObjID, object ObjCellID)
		{
			System.Data.Common.DbDataRecord StorageInstance=null;
			ObjectID mObjectID=null;
			int aObjCellID=0;
			if(ObjCellID is int)
				aObjCellID=(int)ObjCellID;


			mObjectID=new ObjectID(IntObjID,aObjCellID);
			StorageInstanceRef mStorageInstanceRef=(StorageInstanceRef)ActOnStorageSession.OperativeObjectCollections[ObjectsType][mObjectID];
			if(mStorageInstanceRef!=null)
				return mStorageInstanceRef.MemoryInstance;

			if(StorageInstances.Count>0)
				if(StorageInstances.Contains(ObjCellID))
				{
					System.Collections.Hashtable  SubStorageInstances=(System.Collections.Hashtable)StorageInstances[ObjCellID];
					if(SubStorageInstances.Contains(IntObjID))
						StorageInstance=(System.Data.Common.DbDataRecord)SubStorageInstances[IntObjID];
				}

			if(StorageInstance==null)
			{
				StorageInstance=(System.Data.Common.DbDataRecord)DbEnumerator.Current;
				int mIntObjID,mObjCellID;
				mIntObjID=(int)StorageInstance["IntObjID"];
				mObjCellID=(int)StorageInstance["ObjCellID"];
				if(mObjCellID==aObjCellID&&mIntObjID ==IntObjID)
					DbEnumerator.MoveNext();
				
				while(mObjCellID!=aObjCellID||mIntObjID !=IntObjID)
				{
					System.Collections.Hashtable  SubStorageInstances=null;
					if(!StorageInstances.Contains(mObjCellID))
					{
						SubStorageInstances=new  System.Collections.Hashtable(100);
						StorageInstances.Add(mObjCellID,SubStorageInstances);
					}
					else
						SubStorageInstances=(System.Collections.Hashtable) StorageInstances[mObjCellID];
					SubStorageInstances.Add(mIntObjID,StorageInstance);
					StorageInstance=null;
					if(!DbEnumerator.MoveNext())
						break;
					StorageInstance=(System.Data.Common.DbDataRecord)DbEnumerator.Current;
					mIntObjID=(int)StorageInstance["IntObjID"];
					mObjCellID=(int)StorageInstance["ObjCellID"];
				}
			}
			
			if(StorageInstance==null)
				return null;

			
			
			mStorageInstanceRef=(StorageInstanceRef)ActOnStorageSession.CreateStorageInstanceRef(System.Activator.CreateInstance(ObjectsType));
			mStorageInstanceRef.StorageInstanceSet=ClassOfInstances.GetStorageCell(mObjectID.ObjCellID);
			mStorageInstanceRef.ObjectID=mObjectID;
			mStorageInstanceRef.DbDataRecord=StorageInstance;
			mStorageInstanceRef.LoadObjectState("");
			mStorageInstanceRef.SnapshotStorageInstance();
			return mStorageInstanceRef.MemoryInstance;

		}
		/// <MetaDataID>{529A7303-F7FD-46DF-90B5-1B6E702938F8}</MetaDataID>
		private StorageSession ActOnStorageSession;
		/// <MetaDataID>{F79EC8EE-D8BC-4609-B54C-8ECCEAF855E8}</MetaDataID>
		private string StorageInstanceAccessQuery;
		/// <MetaDataID>{7221095F-9BAE-4896-9DB2-45F5CC37681B}</MetaDataID>
		private System.Collections.Hashtable StorageInstances;
		/// <MetaDataID>{CE25B457-0DBC-4E34-BB58-D5BC0BB3C5B9}</MetaDataID>
		private System.Data.OleDb.OleDbDataReader OleDbDataReader;
		/// <MetaDataID>{8B18AE09-4748-48CA-8587-D369001174FB}</MetaDataID>
		private System.Data.Common.DbEnumerator DbEnumerator;
		/// <MetaDataID>{1D162BDE-3958-4C4C-82E0-3BD28F5984FB}</MetaDataID>
		private System.Data.Common.DbDataRecord CurrRecord;
		RDBMSMetaDataRepository.RDBMSMappingClass ClassOfInstances;
		System.Type ObjectsType;
		/// <MetaDataID>{E948837B-1A4C-4B79-B2A6-8321BCBCA987}</MetaDataID>
		public StorageInstanceCollection(string SQLQuery,RDBMSMetaDataRepository.RDBMSMappingClass _ClassOfInstances, StorageSession StorageSession)
		{
			if(SQLQuery==null)
			{
				int k=0;
				k++;
			}
			System.Data.OleDb.OleDbConnection inOleDbConnection=new System.Data.OleDb.OleDbConnection(StorageSession.OleDbConnection.ConnectionString);
			inOleDbConnection.Open();
			System.Data.OleDb.OleDbCommand inmOleDbCommand=inOleDbConnection.CreateCommand();
			inmOleDbCommand.CommandText=SQLQuery;
			System.Diagnostics.Debug.WriteLine(inmOleDbCommand.CommandText);
			System.Data.OleDb.OleDbDataReader OleDbDataReader= inmOleDbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
			DbEnumerator=new System.Data.Common.DbEnumerator(OleDbDataReader,true);

			ClassOfInstances=_ClassOfInstances;

			object TemplateObject=ModulePublisher.ClassRepository.CreateInstance(ClassOfInstances.FullName,"");
			if(TemplateObject==null)
				throw new System.Exception("PersistencyContext Cann't instadiate the "+ClassOfInstances.FullName);
			else
				ObjectsType=TemplateObject.GetType();
			StorageInstances=new System.Collections.Hashtable();
			ActOnStorageSession=StorageSession;

			
			if(!DbEnumerator.MoveNext())
				throw new System.Exception("There isn't storage instances of this type");
		}
	}
}
