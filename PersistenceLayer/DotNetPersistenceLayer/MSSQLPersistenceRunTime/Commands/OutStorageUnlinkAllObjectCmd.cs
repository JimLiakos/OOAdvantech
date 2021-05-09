namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    using ObjectID = RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{E731BCEC-7267-4453-BEDA-A1958397159C}</MetaDataID>
	public class OutStorageUnlinkAllObjectCmd : PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand
	{
		/// <MetaDataID>{E8776E95-9B77-4A41-8328-E33754B0E033}</MetaDataID>
		private ObjectStorage ActOnStorageSession;
		/// <MetaDataID>{3E4C1C86-A0E9-444E-B9DD-D2CB19C443AB}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 10;
			}
		}
		/// <MetaDataID>{E3A0D7CC-C6D6-4AD7-BC97-1BFB6BBB777E}</MetaDataID>
		 public OutStorageUnlinkAllObjectCmd(RDBMSMetaDataRepository.StorageCellsLink  storageCellsLink, StorageInstanceRef deletedStorageInstance, MetaDataRepository.AssociationEnd associationEnd, ObjectStorage actOnStorageSession)
             :base(deletedStorageInstance)
		{
			 //DeletedStorageInstance=deletedStorageInstance;
			 StorageCellsLink=storageCellsLink;
			 ActOnStorageSession=actOnStorageSession;
			 theAssociationEnd=associationEnd;
		
		}
		/// <MetaDataID>{D4FA178D-8FE5-429A-B043-4EE56894F2B9}</MetaDataID>
		private RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink;

		/// <MetaDataID>{0CED6963-BE53-4062-B1B7-3FA8781EE6E2}</MetaDataID>
		public override void GetSubCommands(int CurrentOrder)
		{
		
		}
		/// <MetaDataID>{A5C73316-49BB-4D52-9459-7F459D992952}</MetaDataID>
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
                StorageCellWithColumns = StorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell;
			else
                StorageCellWithColumns = StorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell;
			if(StorageCellWithColumns is RDBMSMetaDataRepository.StorageCellReference)
				return;

			RDBMSMetaDataRepository.Association Association=theAssociationEnd.Association as RDBMSMetaDataRepository.Association;

			ObjectID DeletedObjectID=null;
			RDBMSMetaDataRepository.Table KeepColumnTable=null;
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> linkColumns = null;
			
			int ObjCellID=(int)PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(ActOnStorageSession.GetOutStorageObjColl(DeletedStorageInstance).Properties).ObjectID;
			//TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class
			if(StorageCellWithColumns==StorageCellsLink.RoleAStorageCell)
				linkColumns=((RDBMSMetaDataRepository.AssociationEnd) Association.RoleA).GetReferenceColumnsFor(StorageCellWithColumns);
			else
				linkColumns=((RDBMSMetaDataRepository.AssociationEnd)Association.RoleB).GetReferenceColumnsFor(StorageCellWithColumns);
			
			foreach(RDBMSMetaDataRepository.Column column in linkColumns)
			{
				KeepColumnTable=column.Namespace as RDBMSMetaDataRepository.Table;
				break;
			}


			DeletedObjectID=new ObjectID(((ObjectID)DeletedStorageInstance.ObjectID).IntObjID,ObjCellID);
			KeepColumnTable=StorageCellWithColumns.MainTable;

			UpdateTableColumnsForOneToManyOneToOne(KeepColumnTable,linkColumns,ActOnStorageSession,DeletedObjectID,mAssociationEnd);


		}


		/// <MetaDataID>{CE13FB22-08B0-47BB-B9C1-8E7FE4988A2D}</MetaDataID>
		void UpdateTableColumnsForOneToManyOneToOne(RDBMSMetaDataRepository.Table KeepColumnTable,
			Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mColumns,
			ObjectStorage objectStorage,
            OOAdvantech.RDBMSPersistenceRunTime.ObjectID DeletedObjectID,
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
				if(CurrColumn.ColumnType=="IntObjID")
				{
					UpdateQuery+=CurrColumn.Name+ " = NULL ";
					WhereClause+=CurrColumn.Name+" = " +DeletedObjectID.IntObjID.ToString();
				}
				else
				{
					if(CurrColumn.ColumnType=="StorageCellID")
					{
						UpdateQuery+=CurrColumn.Name+ " = NULL ";
						WhereClause+=CurrColumn.Name+" = " +DeletedObjectID.ObjCellID.ToString();
					}
					else
						throw new System.Exception("Link columns mismatch at '"+ mAssociationEnd.GetOtherEnd().Specification.FullName+"."+mAssociationEnd.FullName+"'");
				}
			}
			mColumns=KeepColumnTable.ObjectIDColumns;
			FirstValue=true;
			UpdateQuery+=WhereClause;
			System.Data.SqlClient.SqlConnection OleDbConnection=objectStorage.DBConnection;
			if(OleDbConnection.State!=System.Data.ConnectionState.Open)
				OleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  OleDbCommand=OleDbConnection.CreateCommand();
			OleDbCommand.CommandText=UpdateQuery;
			int jj=OleDbCommand.ExecuteNonQuery();
			jj=0;

		}
		/// <MetaDataID>{E8A2E6A0-53DB-4569-9918-5A44A3854A5B}</MetaDataID>
		private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{

			int k=0;
			k++;

			RDBMSMetaDataRepository.Association Association=(RDBMSMetaDataRepository.Association)mAssociationEnd.Association;
			
			RDBMSMetaDataRepository.Table AssociationTable =StorageCellsLink.ObjectLinksTable;

            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> DeletedObjectColumns = null;
			DeletedObjectColumns=mAssociationEnd.GetReferenceColumnsFor(AssociationTable);
			string DeleteQuery="";
			DeleteQuery+="DELETE FROM AssociationTable "+
				"WHERE  (Role_IntObjID = Role_Value_IntObjID) AND (Role_ObjCellID = Role_Value_ObjCellID) ";
			DeleteQuery=DeleteQuery.Replace("AssociationTable",AssociationTable.Name);
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in DeletedObjectColumns)
			{
				if(CurrColumn.ColumnType=="IntObjID")
				{
					DeleteQuery=DeleteQuery.Replace("Role_IntObjID",CurrColumn.Name);
					DeleteQuery=DeleteQuery.Replace("Role_Value_IntObjID",((ObjectID)DeletedStorageInstance.ObjectID).IntObjID.ToString());
				}
				if(CurrColumn.ColumnType=="StorageCellID")
				{
					DeleteQuery=DeleteQuery.Replace("Role_ObjCellID",CurrColumn.Name);
					DeleteQuery=DeleteQuery.Replace("Role_Value_ObjCellID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(ActOnStorageSession.GetOutStorageObjColl(DeletedStorageInstance).Properties).ObjectID.ToString());
				}
			}
			System.Data.SqlClient.SqlConnection OleDbConnection=((ObjectStorage)ActOnStorageSession).DBConnection;
			OleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  OleDbCommand=OleDbConnection.CreateCommand();
			OleDbCommand.CommandText=DeleteQuery;
			object Result=OleDbCommand.ExecuteNonQuery();
			k++;
		}


		/// <MetaDataID>{2D00EE2A-074F-47EF-92B6-1F209CBF6BBF}</MetaDataID>
		public override void Execute()
		{
			if(MetaDataRepository.AssociationType.ManyToMany!=StorageCellsLink.Type.MultiplicityType)
				UnlinkOnetoManyOneToOne((RDBMSMetaDataRepository.AssociationEnd)theAssociationEnd);
			else
				UnlinkManyToMany((RDBMSMetaDataRepository.AssociationEnd)theAssociationEnd);
		}
	}
}
