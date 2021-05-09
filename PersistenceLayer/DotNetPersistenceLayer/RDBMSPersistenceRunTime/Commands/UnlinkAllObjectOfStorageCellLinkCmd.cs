namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
    //using ObjectID = OOAdvantech.PersistenceLayer.ObjectID;
    
	/// <MetaDataID>{FA56914D-92B1-4EF1-93F7-E09A655A8FE0}</MetaDataID>
	public class UnlinkAllObjectOfStorageCellLinkCmd : PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand
	{
		/// <MetaDataID>{0AF125BC-F029-487B-A331-33210334BB96}</MetaDataID>
		private RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink;
		/// <MetaDataID>{E952FF71-031A-4C1C-9FFC-E9E29931BF05}</MetaDataID>
		 public UnlinkAllObjectOfStorageCellLinkCmd(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstance, MetaDataRepository.AssociationEnd associationEnd)
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
				return 50;
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
			OOAdvantech.PersistenceLayerRunTime.ObjectStorage objectStorage,
		PersistenceLayer.ObjectID DeletedObjectID,
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
                WhereClause += CurrColumn.Name + " = " + (DeletedStorageInstance.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary.ConvertToSQLString(((ObjectID)DeletedObjectID).GetMemberValue(CurrColumn.ColumnType));
				
			}
			mColumns=KeepColumnTable.ObjectIDColumns;
			FirstValue=true;
			UpdateQuery+=WhereClause;
            var oleDbConnection = (DeletedStorageInstance.ObjectStorage .StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                oleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
//			OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			var OleDbCommand=oleDbConnection.CreateCommand();
			OleDbCommand.CommandText=UpdateQuery;
			int jj=OleDbCommand.ExecuteNonQuery();
			jj=0;

		}

		/// <MetaDataID>{8FD9F502-12E5-469B-BBD5-E64ED343C871}</MetaDataID>
		private void UnlinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			RDBMSMetaDataRepository.AssociationEnd keepAssociationEndColumns = null;
			if(mAssociationEnd.Multiplicity.IsMany)
                keepAssociationEndColumns = mAssociationEnd;
			if(keepAssociationEndColumns == null&&mAssociationEnd.GetOtherEnd().Multiplicity.IsMany)
                keepAssociationEndColumns = mAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
			if(keepAssociationEndColumns == null)
                keepAssociationEndColumns = mAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd;

			RDBMSMetaDataRepository.StorageCell StorageCellWithColumns=null;
			if(keepAssociationEndColumns.IsRoleA)
				StorageCellWithColumns=StorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell;
			else
                StorageCellWithColumns = StorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell;
			if(StorageCellWithColumns.GetType()==typeof(RDBMSMetaDataRepository.StorageCellReference))
				return;
			OOAdvantech.PersistenceLayerRunTime.ObjectStorage objectStorage=null;

            RDBMSMetaDataRepository.Table keepColumnTable=null;

            ObjectID deletedObjectID = (ObjectID)DeletedStorageInstance.PersistentObjectID;
            //TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class

            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns = keepAssociationEndColumns.GetReferenceColumnsFor(StorageCellWithColumns);
			
			foreach(RDBMSMetaDataRepository.Column column in linkColumns)
			{
				keepColumnTable=column.Namespace as RDBMSMetaDataRepository.Table;
				break;
			}

			
			objectStorage=DeletedStorageInstance.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
			UpdateTableColumnsForOneToManyOneToOne(keepColumnTable,linkColumns,objectStorage,deletedObjectID,mAssociationEnd);

		}
		/// <MetaDataID>{382629ED-5316-4E3D-A9D9-B03CC7D7BD7D}</MetaDataID>
		private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			int k=0;
			k++;

			RDBMSMetaDataRepository.Association Association=(RDBMSMetaDataRepository.Association)mAssociationEnd.Association;
			
			RDBMSMetaDataRepository.Table AssociationTable =StorageCellsLink.ObjectLinksTable;
            OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (this.DeletedStorageInstance.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary;

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
                filterString += " [" + column.Name + "] = " + TypeDictionary.ConvertToSQLString(((ObjectID)DeletedStorageInstance.PersistentObjectID).GetMemberValue(column.ColumnType)); ;
			}
            DeleteQuery += filterString;
            var oleDbConnection = (DeletedStorageInstance.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                oleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //			OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            var OleDbCommand = oleDbConnection.CreateCommand();
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
