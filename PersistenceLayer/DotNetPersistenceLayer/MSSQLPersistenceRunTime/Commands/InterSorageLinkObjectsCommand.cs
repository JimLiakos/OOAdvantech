namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    using ObjectID = RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{31CD9D25-A60C-4AF7-BEA5-96C50208D2DB}</MetaDataID>
	public class InterSorageLinkObjectsCommand : OOAdvantech.PersistenceLayerRunTime.Commands.InterSorageLinkObjectsCommand
	{
		/// <MetaDataID>{E695EB85-935F-4116-AD7C-F53C756295CC}</MetaDataID>
		public RDBMSMetaDataRepository.StorageCellReference OutStorageObjectCollection;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{0100B51E-8BAC-47B9-9925-BDB0FF237C57}</MetaDataID>
		private RDBMSMetaDataRepository.AssociationEnd _LinkInitiatorAssociationEnd;
		/// <MetaDataID>{DF025A1B-8533-489D-B427-956332A69504}</MetaDataID>
		private RDBMSMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
		{
			get
			{
				if(_LinkInitiatorAssociationEnd==null)
					_LinkInitiatorAssociationEnd=base.LinkInitiatorAssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
				return _LinkInitiatorAssociationEnd;
			}
		}
		/// <MetaDataID>{BD0AB90E-EF9D-4D12-8FFE-A9D5C4F0CCD0}</MetaDataID>
		private UnLinkObjectsCommand FindUnLinkObjectsCommand(StorageInstanceRef KeepsRelationColumns)
		{
			return null;
		}

		/// <MetaDataID>{0DC83B64-1A05-4B36-98EB-68AA8AFBFB0B}</MetaDataID>
		 public InterSorageLinkObjectsCommand (PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, ObjectStorage commandInitiatorStorage):
			base(roleA,roleB,relationObject,linkInitiatorAssociationEnd,commandInitiatorStorage)
		{

		}

	
			/// <MetaDataID>{8D6D336D-EF97-4A3E-9A21-28D7A2A3A53F}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 40;
			}
		}

		/// <MetaDataID>{7B8E3CF5-3614-44B1-9F59-AEE52F713E9B}</MetaDataID>
		private void UpdateMappingDataIfNeeded()
		{
			RDBMSMetaDataRepository.Association Association= LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association;
			RDBMSMetaDataRepository.StorageCellsLink ObjectCollectionsLink=null;

            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

			if(CommandInitiatorStorage!=RoleA.ObjectStorage)
                ObjectCollectionsLink = Association.GetStorageCellsLink(OutStorageObjectCollection, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath);
			else
                ObjectCollectionsLink = Association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, OutStorageObjectCollection, vaueTypePath);


			if(ObjectCollectionsLink==null)
			{
				if(CommandInitiatorStorage!=RoleA.ObjectStorage)
                    ObjectCollectionsLink = Association.AddStorageCellsLink(OutStorageObjectCollection, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath);
				else
                    ObjectCollectionsLink = Association.AddStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, OutStorageObjectCollection, vaueTypePath);

				//Error prone θεωρούμε λανθασμένα ότι έαν το relation object δεν είναι στη  storagesession του 
				//outstorage instance είναι οποσδήποτε στην storage του instorage instance ενόμπορεί να είναι σε μια τρίτη

				if(Association.LinkClass!=null&&RelationObject.ObjectStorage==CommandInitiatorStorage)
				{
					if(!ObjectCollectionsLink.AssotiationClassStorageCells.Contains(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
					{
						ObjectCollectionsLink.AddAssotiationClassStorageCell(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet);
						ObjectCollectionsLink.UpdateForeignKeys();
                        RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
						if(!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
							OwnerTransactiont.EnlistCommand(updateStorageSchema);

					}
				}
				else
				{
					ObjectStorage theActionStorage=null;
					if(CommandInitiatorStorage!=RoleA.ObjectStorage)
						theActionStorage=(ObjectStorage)RoleB.ObjectStorage;
					else
						theActionStorage=(ObjectStorage)RoleA.ObjectStorage;
					if(Association.LinkClass!=null)
					{
						RDBMSMetaDataRepository.StorageCellReference  OutStorageObjectCell=theActionStorage.GetOutStorageObjColl(RelationObject.RealStorageInstanceRef);
						if(!ObjectCollectionsLink.AssotiationClassStorageCells.Contains(OutStorageObjectCell))
							ObjectCollectionsLink.AddAssotiationClassStorageCell(OutStorageObjectCell);
					}
					if(StorageInstanceRef.GetStorageInstanceRef(ObjectCollectionsLink.Properties).ObjectID==null)
					{
						ObjectCollectionsLink.UpdateForeignKeys();
                        RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
						if(!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
							OwnerTransactiont.EnlistCommand(updateStorageSchema);
					}
				}

			}
		}
		/// <MetaDataID>{2AA3CA7C-49EB-41C0-9F48-971E097FDC4F}</MetaDataID>
		public override void Execute()
		{
			#region Preconditions Chechk
			if(RoleA==null||RoleB==null)
				throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
			//if(theResolver==null)
			//	throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
			#endregion

			base.Execute();

			UpdateMappingDataIfNeeded();


			if(MetaDataRepository.AssociationType.ManyToMany!=LinkInitiatorAssociationEnd.Association.MultiplicityType&&LinkInitiatorAssociationEnd.Association.LinkClass==null)
				LinkOnetoManyOneToOne();
			else
				LinkManyToMany(LinkInitiatorAssociationEnd as RDBMSMetaDataRepository.AssociationEnd);
		}
		/// <MetaDataID>{654FC756-46F4-402E-A944-BE200B554779}</MetaDataID>
		private void LinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
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
                ObjectCollectionsLink = Association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, OutStorageObjectCollection, vaueTypePath);

			RDBMSMetaDataRepository.Table AssociationTable =null;

			if(Association.LinkClass!=null)
			{
				if(RelationObject.ObjectStorage!=CommandInitiatorStorage)//Error prone θεωρούμε λανθασμένα ότι έαν το relation object δεν είναι στη  storagesession του 
					return;																			//outstorage instance είναι οποσδήποτε στην storage του instorage instance ενόμπορεί να είναι σε μια τρίτη
				AssociationTable=((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet.MainTable;
			}
			else
				AssociationTable =ObjectCollectionsLink.ObjectLinksTable;

			//RDBMSMetaDataRepository.Table AssociationTable =ObjectCollectionsLink.ObjectLinksTable;
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> RoleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)Association.RoleA).GetReferenceColumnsFor(AssociationTable);
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> RoleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)Association.RoleB).GetReferenceColumnsFor(AssociationTable);
			string LinkQuery="";
			LinkQuery="declare @Rows int; "+
				"set @Rows=0; "+
				"SELECT   @Rows=COUNT(*) "+
				"FROM         AssociationTable "+
				"WHERE     (RoleA_IntObjID = RoleA_Value_IntObjID) AND (RoleA_ObjCellID = RoleA_Value_ObjCellID) AND (RoleB_IntObjID = RoleB_Value_IntObjID) AND (RoleB_ObjCellID = RoleB_Value_ObjCellID) "+
				"if @Rows =0 "+
				"begin ";

			if(Association.LinkClass!=null)
				LinkQuery+="UPDATE    AssociationTable "+
					"SET              RoleA_IntObjID = RoleA_Value_IntObjID, RoleA_ObjCellID = RoleA_Value_ObjCellID, RoleB_IntObjID = RoleB_Value_IntObjID, RoleB_ObjCellID = RoleB_Value_ObjCellID "+
					"WHERE IntObjID=IntObjID_Value AND ObjCellID=ObjCellID_Value ";
			else
				LinkQuery+="INSERT INTO AssociationTable "+
					"                      (RoleA_IntObjID, RoleA_ObjCellID, RoleB_IntObjID, RoleB_ObjCellID) "+
					"VALUES     (RoleA_Value_IntObjID, RoleA_Value_ObjCellID, RoleB_Value_IntObjID, RoleB_Value_ObjCellID) ";

			LinkQuery+="end "+
				"SELECT  COUNT(*) "+
				"FROM         AssociationTable "+
				"WHERE     (RoleA_IntObjID = RoleA_Value_IntObjID) AND (RoleA_ObjCellID = RoleA_Value_ObjCellID) AND (RoleB_IntObjID = RoleB_Value_IntObjID) AND (RoleB_ObjCellID = RoleB_Value_ObjCellID) ";

			LinkQuery=LinkQuery.Replace("AssociationTable",AssociationTable.Name);
			if(Association.LinkClass!=null)
			{
				LinkQuery=LinkQuery.Replace("IntObjID_Value",((ObjectID)RelationObject.ObjectID).IntObjID.ToString());
				LinkQuery=LinkQuery.Replace("ObjCellID_Value",((ObjectID)RelationObject.ObjectID).ObjCellID.ToString());
			}

			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in RoleAColumns)
			{
				if(CurrColumn.ColumnType=="IntObjID")
				{
					LinkQuery=LinkQuery.Replace("RoleA_IntObjID",CurrColumn.Name);
					LinkQuery=LinkQuery.Replace("RoleA_Value_IntObjID",((ObjectID)RoleA.ObjectID).IntObjID.ToString());
					
				}
				if(CurrColumn.ColumnType=="StorageCellID")
				{
					LinkQuery=LinkQuery.Replace("RoleA_ObjCellID",CurrColumn.Name);
					if(RoleA.ObjectStorage!=CommandInitiatorStorage)
						LinkQuery=LinkQuery.Replace("RoleA_Value_ObjCellID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID.ToString());
					else
						LinkQuery=LinkQuery.Replace("RoleA_Value_ObjCellID",((ObjectID)RoleA.ObjectID).ObjCellID.ToString());
				}
			}
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in RoleBColumns)
			{
				if(CurrColumn.ColumnType=="IntObjID")
				{
					LinkQuery=LinkQuery.Replace("RoleB_IntObjID",CurrColumn.Name);
					LinkQuery=LinkQuery.Replace("RoleB_Value_IntObjID",((ObjectID)RoleB.ObjectID).IntObjID.ToString());
				}
				if(CurrColumn.ColumnType=="StorageCellID")
				{
					LinkQuery=LinkQuery.Replace("RoleB_ObjCellID",CurrColumn.Name);
					if(RoleB.ObjectStorage!=CommandInitiatorStorage)
						LinkQuery=LinkQuery.Replace("RoleB_Value_ObjCellID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID.ToString());
					else
						LinkQuery=LinkQuery.Replace("RoleB_Value_ObjCellID",((ObjectID)RoleB.ObjectID).ObjCellID.ToString());
				}
			}


			System.Data.SqlClient.SqlConnection OleDbConnection=(CommandInitiatorStorage as ObjectStorage).DBConnection;

//			System.Data.SqlClient.SqlConnection OleDbConnection=((StorageSession)theResolver.Owner.ActiveStorageSession).OleDbConnection;
			if(OleDbConnection.State!=System.Data.ConnectionState.Open)
				OleDbConnection.Open();
            //OleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

			//OleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  OleDbCommand=OleDbConnection.CreateCommand();
			OleDbCommand.CommandText=LinkQuery;
			int Result=OleDbCommand.ExecuteNonQuery();
			if(Result!=0&&Association.LinkClass==null)
				ObjectCollectionsLink.ObjectsLinksCount++;

			int lo=0;

		}
		
		/// <MetaDataID>{3AACD824-74E6-438C-A616-7AD50864AFE2}</MetaDataID>
		private void LinkOnetoManyOneToOne()
		{

			RDBMSMetaDataRepository.AssociationEnd associationEnd=(LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();


			#region Precondition check
			if(!IsTheReferralStorageInstanceOutOfStorage())
				return;
			if(associationEnd.IsRoleA)
			{
				if(CommandInitiatorStorage==RoleB.ObjectStorage)
					throw new System.Exception("Bad InterStorage Link Command");
			}
			else
			{
				if(CommandInitiatorStorage==RoleA.ObjectStorage)
					throw new System.Exception("Bad InterStorage Link Command");
			}
			#endregion

			StorageInstanceRef RecordOwnerObject=null;
			StorageInstanceRef AssignedObject=null;
			RDBMSMetaDataRepository.Table KeepColumnTable=null;
			#region Gets metadata informations 
			
			if(associationEnd.IsRoleA)
			{
				RecordOwnerObject=RoleA.RealStorageInstanceRef as StorageInstanceRef;
				AssignedObject=RoleB.RealStorageInstanceRef as StorageInstanceRef;
			}
			else
			{
				RecordOwnerObject=RoleB.RealStorageInstanceRef as StorageInstanceRef;
				AssignedObject=RoleA.RealStorageInstanceRef as StorageInstanceRef;
			}
			//TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(RecordOwnerObject.StorageInstanceSet);
			foreach(RDBMSMetaDataRepository.Column column in linkColumns)
			{
				KeepColumnTable=column.Namespace as RDBMSMetaDataRepository.Table;
				break;
			}
			#endregion

			string UpdateQuery;
			#region Build SQL command

            RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (RecordOwnerObject.ObjectStorage as ObjectStorage).TypeDictionary;

			UpdateQuery="UPDATE "+KeepColumnTable.Name+" SET ";
			string FirstMultiplicityConstraint=" AND ((";
			string SecondMultiplicityConstraint=") or (";
			string WhereClause=" WHERE ";
			bool FirstValue=true;
			foreach(RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
			{
				if(!FirstValue)
				{
					FirstMultiplicityConstraint+=" AND ";
					SecondMultiplicityConstraint+=" AND ";
					UpdateQuery+=",";
				}
				FirstValue=false;
				FirstMultiplicityConstraint+=column.Name+ " = " +TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));
				SecondMultiplicityConstraint+=column.Name+ " IS NULL";
				UpdateQuery+=column.Name+ " = " +TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));
			
			}
			FirstValue=true;
			foreach(RDBMSMetaDataRepository.IdentityColumn column in KeepColumnTable.ObjectIDColumns)
			{
				if(!FirstValue)
					WhereClause+=" AND ";
				FirstValue=false;
				WhereClause+=column.Name+ " = " +TypeDictionary.ConvertToSQLString( ((ObjectID)RecordOwnerObject.ObjectID).GetMemberValue(column.ColumnType));
			}
			
			UpdateQuery+=WhereClause+FirstMultiplicityConstraint+SecondMultiplicityConstraint+"))";
			#endregion

			int RowAffected;
			#region Open connection and execute the command 
			System.Data.SqlClient.SqlConnection oleDbConnection=(RecordOwnerObject.ObjectStorage as ObjectStorage).DBConnection;
			if(oleDbConnection.State!=System.Data.ConnectionState.Open)
				oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
			//oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  oleDbCommand=oleDbConnection.CreateCommand();
			oleDbCommand.CommandText=UpdateQuery;
			RowAffected=oleDbCommand.ExecuteNonQuery();
			#endregion

			#region Postcondition check
			if(RowAffected==0)
			{
				oleDbCommand=oleDbConnection.CreateCommand();
				oleDbCommand.CommandText="SELECT count(*) FROM "+KeepColumnTable.Name+" "+WhereClause;
				int Rows=(int)oleDbCommand.ExecuteNonQuery();
				if(Rows>0)
					throw new System.Exception("Multiplicity constraint mismatch at association '"+associationEnd.Association.Name+"' on object "+RecordOwnerObject.MemoryInstance.ToString());
				else
					throw new System.Exception("You try link object '"+associationEnd.Name+"' which doesnot exist.");
			}
			#endregion
			
		}
		/// <MetaDataID>{A0D6AAF0-5920-4620-961D-7BE172EE0382}</MetaDataID>
		internal bool SubTransactionCmdsProduced=false;

		/// <MetaDataID>{9B80C740-AF8C-4653-9665-746D4719F297}</MetaDataID>
		bool IsTheReferralStorageInstanceOutOfStorage()
		{
			RDBMSMetaDataRepository.AssociationEnd ReferralStorageInstanceAssociationEnd=(LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns().GetOtherEnd()as RDBMSMetaDataRepository.AssociationEnd;
			
			if(CommandInitiatorStorage!=RoleA.ObjectStorage&& (!ReferralStorageInstanceAssociationEnd.IsRoleA)||CommandInitiatorStorage!=RoleB.ObjectStorage&& (ReferralStorageInstanceAssociationEnd.IsRoleA))
				return false;
			else
				return true;
		}
		/// <MetaDataID>{30796838-2760-409C-BBA3-BAA8E395F400}</MetaDataID>
		public override void GetSubCommands(int currentExecutionOrder)
		{
			if(currentExecutionOrder<=10)
				return ;

			if(SubTransactionCmdsProduced)
				return ;
			SubTransactionCmdsProduced=true;

			ObjectStorage theActionStorage=null;
			if(CommandInitiatorStorage==RoleA.ObjectStorage)
				OutStorageObjectCollection=(CommandInitiatorStorage as ObjectStorage).GetOutStorageObjColl(RoleB.RealStorageInstanceRef);
			else
				OutStorageObjectCollection=(CommandInitiatorStorage as ObjectStorage).GetOutStorageObjColl(RoleA.RealStorageInstanceRef);

			

			 
            //if(PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(OutStorageObjectCollection.Properties).ObjectID==null)
            //{
            //    PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            //    transactionContext.EnlistCommand((new UpdateGlobalObjectCollectionIDs(theActionStorage,OutStorageObjectCollection)));

            //}
			
		}

	}
}
