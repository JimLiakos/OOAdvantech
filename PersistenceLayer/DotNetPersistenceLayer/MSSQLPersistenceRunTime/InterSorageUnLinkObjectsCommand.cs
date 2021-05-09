namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{500AFB00-22EC-47AE-BC41-49816490C6E5}</MetaDataID>
	public class InterSorageUnLinkObjectsCommand :PersistenceLayerRunTime.Commands.InterSorageUnLinkObjectsCommand
	{
		/// <MetaDataID>{1594CDA7-3537-4A16-927D-8D3661045FAE}</MetaDataID>
		protected RDBMSMetaDataRepository.StorageCellReference OutStorageObjectCollection;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{27F30AC6-9904-41CA-882A-DF27612ECDB3}</MetaDataID>
		private RDBMSMetaDataRepository.AssociationEnd _LinkInitiatorAssociationEnd;
		/// <MetaDataID>{C8885824-417E-4A26-85DA-878708950F57}</MetaDataID>
		new private RDBMSMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
		{
			get
			{
				if(_LinkInitiatorAssociationEnd==null)
					_LinkInitiatorAssociationEnd=base.LinkInitiatorAssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
				return _LinkInitiatorAssociationEnd;

			}
		}

		/// <MetaDataID>{DDA6525A-9536-49B7-B8C3-3966565DCBA1}</MetaDataID>
		 public InterSorageUnLinkObjectsCommand (PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, ObjectStorage commandInitiatorStorage):
			base(roleA,roleB,relationObject,linkInitiatorAssociationEnd,commandInitiatorStorage)
		{

		}

		/// <summary>Priority defines the order in which will be executed the command. </summary>
		/// <MetaDataID>{17EF0FCD-854E-4924-A906-5146F714CB85}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 40;
			}
		}
		/// <MetaDataID>{81196B11-A0CB-4F7A-B6B9-F3CFC9C869A9}</MetaDataID>
		bool IsTheReferralStorageInstanceOutOfStorage()
		{
			RDBMSMetaDataRepository.AssociationEnd ReferralStorageInstanceAssociationEnd=(LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns().GetOtherEnd()as RDBMSMetaDataRepository.AssociationEnd;
			if(CommandInitiatorStorage!=RoleA.ObjectStorage&& (!ReferralStorageInstanceAssociationEnd.IsRoleA)||CommandInitiatorStorage!=RoleB.ObjectStorage&& (ReferralStorageInstanceAssociationEnd.IsRoleA))
				return false;
			else
				return true;
		}

		/// <MetaDataID>{8774B3BF-8551-4A9D-8A8C-933931AB2B12}</MetaDataID>
		private void UnlinkOnetoManyOneToOne()
		{

			RDBMSMetaDataRepository.AssociationEnd mAssociationEnd=(LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
			if(!IsTheReferralStorageInstanceOutOfStorage())
				return;
		
			string UpdateQuery=null;

			RDBMSMetaDataRepository.Table KeepColumnTable=null;
            RDBMSPersistenceRunTime.ObjectID RecordOwnerObjectID = null;
			StorageInstanceRef RecordOwnerObject=null;
			if(mAssociationEnd.IsRoleA)
			{
				RecordOwnerObjectID=(ObjectID)RoleA.ObjectID;
				RecordOwnerObject=(StorageInstanceRef)RoleA.RealStorageInstanceRef;
			}
			else
			{
				RecordOwnerObjectID=(ObjectID)RoleB.ObjectID;
				RecordOwnerObject=(StorageInstanceRef)RoleB.RealStorageInstanceRef;
			}

			//TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> linkColumns = mAssociationEnd.GetReferenceColumnsFor(RecordOwnerObject.StorageInstanceSet);
			foreach(RDBMSMetaDataRepository.Column column in linkColumns)
			{
				KeepColumnTable=column.Namespace as RDBMSMetaDataRepository.Table;
				break;
			}



			string TableName=KeepColumnTable.Name;
			UpdateQuery="UPDATE "+TableName +" SET ";
			string WhereClause=" WHERE ";
			bool FirstValue=true;
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in linkColumns)
			{
				if(!FirstValue)
				{
					WhereClause+=" AND ";
					UpdateQuery+=",";
				}
				FirstValue=false;
				if(CurrColumn.ColumnType=="IntObjID")
				{
					UpdateQuery+=CurrColumn.Name+ " = NULL ";
					PersistenceLayerRunTime.StorageInstanceAgent outStorageInstance;
					if(RoleA.ObjectStorage!=CommandInitiatorStorage)
						outStorageInstance=RoleA;
					else
						outStorageInstance=RoleB;

					WhereClause+=CurrColumn.Name+ " = "+((ObjectID)outStorageInstance.ObjectID).IntObjID.ToString();
				}
				else
				{
					if(CurrColumn.ColumnType=="StorageCellID")
					{
						UpdateQuery+=CurrColumn.Name+ " = NULL ";
						WhereClause+=CurrColumn.Name+ " = "+PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID.ToString();
					}
					else
						throw new System.Exception("Link columns mismatch at '"+ LinkInitiatorAssociationEnd.GetOtherEnd().Specification.FullName+"."+LinkInitiatorAssociationEnd.FullName+"'");
				}
			}
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> objectIDColumns = KeepColumnTable.ObjectIDColumns;
			FirstValue=true;
			//UpdateQuery+=" WHERE ";
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in objectIDColumns)
			{
			//	if(!FirstValue)
				WhereClause+=" AND ";
			//	FirstValue=false;
				if(CurrColumn.ColumnType=="IntObjID")
					WhereClause+=CurrColumn.Name+ " = " +RecordOwnerObjectID.IntObjID.ToString();
				else
				{
					if(CurrColumn.ColumnType=="StorageCellID")
						WhereClause+=CurrColumn.Name+ " = " +RecordOwnerObjectID.ObjCellID.ToString();
					else
						throw new System.Exception("Link columns mismatch at '"+ LinkInitiatorAssociationEnd.GetOtherEnd().Specification.FullName+"." +LinkInitiatorAssociationEnd.FullName+"'");
				}
			}
			UpdateQuery+=WhereClause;
			

			System.Data.SqlClient.SqlConnection OleDbConnection=(CommandInitiatorStorage as ObjectStorage).DBConnection;

			if(OleDbConnection.State!=System.Data.ConnectionState.Open)
				OleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  OleDbCommand=OleDbConnection.CreateCommand();
			OleDbCommand.CommandText=UpdateQuery;
			int Result=OleDbCommand.ExecuteNonQuery();
			int k=0;
		


		}
		/// <MetaDataID>{DE36319F-6545-4F35-AFB6-773C2403CFD0}</MetaDataID>
		private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{

			RDBMSMetaDataRepository.Association Association=(RDBMSMetaDataRepository.Association)mAssociationEnd.Association;
			RDBMSMetaDataRepository.StorageCellsLink ObjectCollectionsLink;
            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

			if(CommandInitiatorStorage!=RoleA.ObjectStorage)
				ObjectCollectionsLink=Association.GetStorageCellsLink( OutStorageObjectCollection,((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet,vaueTypePath);
			else
				ObjectCollectionsLink=Association.GetStorageCellsLink( ((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet,OutStorageObjectCollection,vaueTypePath);

			RDBMSMetaDataRepository.Table AssociationTable =null;

			if(Association.LinkClass!=null)
			{
				if(RelationObject.ObjectStorage!=CommandInitiatorStorage)//Error prone θεωρούμε λανθασμένα ότι έαν το relation object δεν είναι στη  storagesession του 
					return;																			//outstorage instance είναι οποσδήποτε στην storage του instorage instance ενόμπορεί να είναι σε μια τρίτη
				AssociationTable=((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet.MainTable;
			}
			else
				AssociationTable =ObjectCollectionsLink.ObjectLinksTable;

            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> RoleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)Association.RoleA).GetReferenceColumnsFor(AssociationTable);
			Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> RoleBColumns=((RDBMSMetaDataRepository.AssociationEnd)Association.RoleB).GetReferenceColumnsFor(AssociationTable);
			string DeleteQuery="";
			DeleteQuery+="DELETE FROM AssociationTable "+
				"WHERE     (RoleA_IntObjID = RoleA_Value_IntObjID) AND (RoleA_ObjCellID = RoleA_Value_ObjCellID) AND (RoleB_IntObjID = RoleB_Value_IntObjID) AND (RoleB_ObjCellID = RoleB_Value_ObjCellID) ";


			DeleteQuery=DeleteQuery.Replace("AssociationTable",AssociationTable.Name);
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in RoleAColumns)
			{
				if(CurrColumn.ColumnType=="IntObjID")
				{
					DeleteQuery=DeleteQuery.Replace("RoleA_IntObjID",CurrColumn.Name);
					DeleteQuery=DeleteQuery.Replace("RoleA_Value_IntObjID",((ObjectID)RoleA.ObjectID).IntObjID.ToString());
				}
				if(CurrColumn.ColumnType=="StorageCellID")
				{
					DeleteQuery=DeleteQuery.Replace("RoleA_ObjCellID",CurrColumn.Name);
					if(RoleA.ObjectStorage!=CommandInitiatorStorage)
						DeleteQuery=DeleteQuery.Replace("RoleA_Value_ObjCellID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID.ToString());
					else
						DeleteQuery=DeleteQuery.Replace("RoleA_Value_ObjCellID",((ObjectID)RoleA.ObjectID).ObjCellID.ToString());

				}
			}
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in RoleBColumns)
			{
				if(CurrColumn.ColumnType=="IntObjID")
				{
					DeleteQuery=DeleteQuery.Replace("RoleB_IntObjID",CurrColumn.Name);
					DeleteQuery=DeleteQuery.Replace("RoleB_Value_IntObjID",((ObjectID)RoleB.ObjectID).IntObjID.ToString());
				}
				if(CurrColumn.ColumnType=="StorageCellID")
				{
					DeleteQuery=DeleteQuery.Replace("RoleB_ObjCellID",CurrColumn.Name);
					if(RoleB.ObjectStorage!=CommandInitiatorStorage)
						DeleteQuery=DeleteQuery.Replace("RoleB_Value_ObjCellID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID.ToString());
					else
						DeleteQuery=DeleteQuery.Replace("RoleB_Value_ObjCellID",((ObjectID)RoleB.ObjectID).ObjCellID.ToString());
				}
			}


			System.Data.SqlClient.SqlConnection OleDbConnection;
			OleDbConnection=(CommandInitiatorStorage as ObjectStorage).DBConnection;

            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  OleDbCommand=OleDbConnection.CreateCommand();
			OleDbCommand.CommandText=DeleteQuery;
			object Result=OleDbCommand.ExecuteNonQuery();
		}
		/// <MetaDataID>{B459B76E-6D84-471F-AEDF-B6F5FEC794D7}</MetaDataID>
		public override void Execute()
		{
			int j=0;
			base.Execute();

			ObjectStorage theActionStorage=null;
			if(CommandInitiatorStorage!=RoleA.ObjectStorage)
				OutStorageObjectCollection=(CommandInitiatorStorage as ObjectStorage).GetOutStorageObjColl(RoleA.RealStorageInstanceRef);
			else
				OutStorageObjectCollection=(CommandInitiatorStorage as ObjectStorage).GetOutStorageObjColl(RoleB.RealStorageInstanceRef);
			

			if(MetaDataRepository.AssociationType.ManyToMany!=LinkInitiatorAssociationEnd.Association.MultiplicityType&&LinkInitiatorAssociationEnd.Association.LinkClass==null )
				UnlinkOnetoManyOneToOne();
			else
				UnlinkManyToMany(LinkInitiatorAssociationEnd as RDBMSMetaDataRepository.AssociationEnd);

		
		}
	}
}
