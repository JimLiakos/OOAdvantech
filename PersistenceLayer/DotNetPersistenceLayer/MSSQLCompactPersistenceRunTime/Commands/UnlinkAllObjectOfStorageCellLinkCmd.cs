namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{
    using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
	/// <MetaDataID>{FA56914D-92B1-4EF1-93F7-E09A655A8FE0}</MetaDataID>
	public class UnlinkAllObjectOfStorageCellLinkCmd : PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand
	{
		/// <MetaDataID>{0AF125BC-F029-487B-A331-33210334BB96}</MetaDataID>
		private RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink;
		/// <MetaDataID>{E952FF71-031A-4C1C-9FFC-E9E29931BF05}</MetaDataID>
		 public UnlinkAllObjectOfStorageCellLinkCmd(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, StorageInstanceRef deletedStorageInstance, MetaDataRepository.AssociationEnd associationEnd)
             :base(deletedStorageInstance)
		{
			StorageCellsLink=storageCellsLink;
			//DeletedStorageInstance=deletedStorageInstance;
            theAssociationEnd=associationEnd;
		}

		/// <MetaDataID>{AE7F43D3-C137-4667-888B-A516F7FCB39B}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 40;
			}
		}
		public override string Identity
		{
			get
			{
				return "unlinkallstoragecell"+DeletedStorageInstance.MemoryID.ToString()+theAssociationEnd.Identity+StorageCellsLink.Identity ;
			}
		}


		/// <MetaDataID>{13CDCF90-1CE8-4E8E-BFAF-DA5EFF5C1439}</MetaDataID>
		public override void Execute()
		{
			if(MetaDataRepository.AssociationType.ManyToMany!=StorageCellsLink.Type.MultiplicityType)
				UnlinkOnetoManyOneToOne((RDBMSMetaDataRepository.AssociationEnd)theAssociationEnd);
			else
				UnlinkManyToMany((RDBMSMetaDataRepository.AssociationEnd)theAssociationEnd);

		
		}
		/// <MetaDataID>{420F725D-1F41-4CB0-9F61-2CDDA464F99E}</MetaDataID>
		void UpdateTableColumnsForOneToManyOneToOne(RDBMSMetaDataRepository.Table KeepColumnTable,
			Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn>  mColumns,
			ObjectStorage objectStorage,
			ObjectID DeletedObjectID,
			RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			string TableName=KeepColumnTable.Name;
			string UpdateQuery="UPDATE "+TableName +" SET ";
			string WhereClause=" WHERE ";
			bool FirstValue=true;
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in mColumns)
			{
				if(!FirstValue)
				{
					UpdateQuery+=",";
					WhereClause+=" AND ";
				}
				FirstValue=false;
                UpdateQuery += CurrColumn.Name + " = NULL ";
                WhereClause += CurrColumn.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)DeletedObjectID).GetMemberValue(CurrColumn.ColumnType));
				
			}
			mColumns=KeepColumnTable.ObjectIDColumns;
			FirstValue=true;
			UpdateQuery+=WhereClause;
            System.Data.Common.DbConnection oleDbConnection = ((DeletedStorageInstance.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
//			OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.Common.DbCommand OleDbCommand=oleDbConnection.CreateCommand();
			OleDbCommand.CommandText=UpdateQuery;
			int jj=OleDbCommand.ExecuteNonQuery();
			jj=0;

		}

		/// <MetaDataID>{8FD9F502-12E5-469B-BBD5-E64ED343C871}</MetaDataID>
		private void UnlinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			RDBMSMetaDataRepository.AssociationEnd KeepColumnsAssociationEnd=null;
			if(mAssociationEnd.Multiplicity.IsMany)
				KeepColumnsAssociationEnd=mAssociationEnd;
			if(KeepColumnsAssociationEnd==null&&mAssociationEnd.GetOtherEnd().Multiplicity.IsMany) 
				KeepColumnsAssociationEnd=mAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
			if(KeepColumnsAssociationEnd==null) 
				KeepColumnsAssociationEnd=mAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd;

			RDBMSMetaDataRepository.StorageCell StorageCellWithColumns=null;
			if(KeepColumnsAssociationEnd.IsRoleA)
				StorageCellWithColumns=StorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell;
			else
                StorageCellWithColumns = StorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell;
			if(StorageCellWithColumns.GetType()==typeof(RDBMSMetaDataRepository.StorageCellReference))
				return;
			ObjectStorage objectStorage=null;
			ObjectID DeletedObjectID=null;
			RDBMSMetaDataRepository.Table KeepColumnTable=null;
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> linkColumns = null;
			DeletedObjectID=(ObjectID)DeletedStorageInstance.ObjectID;
			//TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class
            if (KeepColumnsAssociationEnd.IsRoleA)
				linkColumns=((RDBMSMetaDataRepository.AssociationEnd)mAssociationEnd.Association.RoleA).GetReferenceColumnsFor(StorageCellWithColumns);
			else
				linkColumns=((RDBMSMetaDataRepository.AssociationEnd)mAssociationEnd.Association.RoleB).GetReferenceColumnsFor(StorageCellWithColumns);
			foreach(RDBMSMetaDataRepository.Column column in linkColumns)
			{
				KeepColumnTable=column.Namespace as RDBMSMetaDataRepository.Table;
				break;
			}

			
			objectStorage=(ObjectStorage)DeletedStorageInstance.ObjectStorage;
			UpdateTableColumnsForOneToManyOneToOne(KeepColumnTable,linkColumns,objectStorage,DeletedObjectID,mAssociationEnd);

		}
		/// <MetaDataID>{382629ED-5316-4E3D-A9D9-B03CC7D7BD7D}</MetaDataID>
		private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			int k=0;
			k++;

			RDBMSMetaDataRepository.Association Association=(RDBMSMetaDataRepository.Association)mAssociationEnd.Association;
			
			RDBMSMetaDataRepository.Table AssociationTable =StorageCellsLink.ObjectLinksTable;

			Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> DeletedObjectColumns=null;
			DeletedObjectColumns=mAssociationEnd.GetReferenceColumnsFor(AssociationTable);
			string DeleteQuery="";
            DeleteQuery += "DELETE FROM [" + AssociationTable.Name + "] ";
            string filterString =null;
			DeleteQuery=DeleteQuery.Replace("AssociationTable",AssociationTable.Name);
			foreach(RDBMSMetaDataRepository.IdentityColumn column in DeletedObjectColumns)
			{
                if(filterString ==null)
                    filterString ="\nWHERE  ";
                else
                    filterString=" AND ";
                filterString += " [" + column.Name + "] = " + TypeDictionary.ConvertToSQLString(((ObjectID)DeletedStorageInstance.ObjectID).GetMemberValue(column.ColumnType)); ;
			}
            DeleteQuery += filterString;
            System.Data.Common.DbConnection oleDbConnection = ((DeletedStorageInstance.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //			OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            System.Data.Common.DbCommand OleDbCommand = oleDbConnection.CreateCommand();
            OleDbCommand.CommandText = DeleteQuery;
			object Result=OleDbCommand.ExecuteNonQuery();
			k++;

		}

		/// <MetaDataID>{0FF5AD67-A5C7-4DA1-BBE3-3B5FC0AB5777}</MetaDataID>
		public override  void GetSubCommands(int currentOrder)
		{
			
		}
	
	}
}
