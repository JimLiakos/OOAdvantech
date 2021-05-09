namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    //using OOAdvantech.RDBMSPersistenceRunTime;
    /// <MetaDataID>{C0BD13A5-7C20-4A9A-8AD5-E357D2DACA38}</MetaDataID>
    public class LinkObjectsCommand : PersistenceLayerRunTime.Commands.LinkObjectsCommand
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{80E0A290-7601-4EF9-BA9C-534C02713C4F}</MetaDataID>
        private RDBMSMetaDataRepository.AssociationEnd _LinkInitiatorAssociationEnd;
        /// <MetaDataID>{971FD658-73C8-42E5-9CFC-71AB68E0A2DB}</MetaDataID>
        new private RDBMSMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
        {
            get
            {
                if (_LinkInitiatorAssociationEnd == null)
                    _LinkInitiatorAssociationEnd = base.LinkInitiatorAssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                return _LinkInitiatorAssociationEnd;
            }
        }
        /// <MetaDataID>{BC959ED3-AABD-4F5A-978F-C87AEF43389D}</MetaDataID>
        private UnLinkObjectsCommand FindUnLinkObjectsCommand(StorageInstanceRef KeepsRelationColumns)
        {
            return null;
        }
        /// <MetaDataID>{0CD0E6BE-8E42-4668-AFF2-14F8F7F25422}</MetaDataID>
        public LinkObjectsCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
            :
           base(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {

        }


        /// <MetaDataID>{E36D550A-294A-4D31-9EFF-8DB4B771BCF7}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            if (currentExecutionOrder <= 10)
                return;
            if (!SubTransactionCmdsProduced)
            {
                base.GetSubCommands(currentExecutionOrder);
                SubTransactionCmdsProduced = true;
                UpdateMappingDataIfNeeded();
            }
            else
                base.GetSubCommands(currentExecutionOrder);


        }




        /// <MetaDataID>{AFE64815-5E6B-4832-8539-69EACC437A24}</MetaDataID>
        private void LinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {
            #region Precondition check
            if (mAssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || mAssociationEnd.Association.LinkClass != null)
                throw new System.Exception("It Can’t  link many to many relationship");
            #endregion

            RDBMSMetaDataRepository.AssociationEnd associationEnd = (mAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
            StorageInstanceRef RecordOwnerObject = null;
            StorageInstanceRef AssignedObject = null;
            RDBMSMetaDataRepository.Table KeepColumnTable = null;

            #region Gets metadata informations

            if (associationEnd.IsRoleA)
            {
                RecordOwnerObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
                AssignedObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
            }
            else
            {
                RecordOwnerObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
                AssignedObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
            }

            //TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class

            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(RecordOwnerObject.StorageInstanceSet, vaueTypePath);
            foreach (RDBMSMetaDataRepository.Column column in linkColumns)
            {
                KeepColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }


            #endregion

            string UpdateQuery;

            #region Build SQL command


						                


            
            UpdateQuery = "UPDATE " + KeepColumnTable.Name + " SET ";
            //			string FirstMultiplicityConstraint=" AND ((";
            //			string SecondMultiplicityConstraint=") or (";
            string WhereClause = " WHERE ";
            string IndexCriterion = " WHERE ";
            bool FirstValue = true;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                //				if(!FirstValue)
                //				{
                //					FirstMultiplicityConstraint+=" AND ";
                //					SecondMultiplicityConstraint+=" AND ";
                //					UpdateQuery+=",";
                //				}
                //				FirstValue=false;
                //				FirstMultiplicityConstraint+=column.Name+ " = " +TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));
                //				SecondMultiplicityConstraint+=column.Name+ " IS NULL";
                  if (!FirstValue)
                      UpdateQuery += ",";

                UpdateQuery += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));

                if (!FirstValue)
                    IndexCriterion += " AND ";
                FirstValue = false;
                IndexCriterion += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));


                //				"UPDATE T_Dog1 SET PersonDog_ObjectIDA = 'ec142104-70ec-4541-b838-b0c254def88b' WHERE ObjectID = 'eb2ab458-f1a8-4897-ae38-072fc43ec81e' AND ((PersonDog_ObjectIDA = 'ec142104-70ec-4541-b838-b0c254def88b') or (PersonDog_ObjectIDA IS NULL))"	string

            }
            string indexCalculation = null;
            bool AddToTheEnd = false;
            int index = -1;
            if (associationEnd.Indexer)
            {
                AddToTheEnd = true;
                if ((associationEnd.IsRoleA && RoleAIndex == -1) || (!associationEnd.IsRoleA && RoleBIndex == -1))
                {
                    indexCalculation = @"declare @index int;
                                        select @index=COUNT(*) 						
                                        FROM     " + KeepColumnTable.Name + "\n";
                    indexCalculation = indexCalculation + IndexCriterion + "\n";
                }
                else
                {

                    if (associationEnd.IsRoleA)
                        index = RoleAIndex;
                    else
                        index = RoleBIndex;
                    indexCalculation = @"declare @index int;
                                       set @index = " + index.ToString()+ @"
                                       UPDATE " + KeepColumnTable.Name + @"
                                        SET    " + associationEnd.IndexerColumn.Name + " = " + associationEnd.IndexerColumn.Name + @" + 1 
                                        " + IndexCriterion + "AND  " + associationEnd.IndexerColumn.Name + " >= @index \n";
                    
                    


                }
            }

            
            FirstValue = true;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in KeepColumnTable.ObjectIDColumns)
            {
                if (!FirstValue)
                    WhereClause += " AND ";
                FirstValue = false;
                WhereClause += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)RecordOwnerObject.ObjectID).GetMemberValue(column.ColumnType));
            }

            if (associationEnd.Indexer)
            {
                UpdateQuery += "," + associationEnd.IndexerColumn.Name + " = @index";
                UpdateQuery = indexCalculation + UpdateQuery;
            }

            UpdateQuery += WhereClause;//+FirstMultiplicityConstraint+SecondMultiplicityConstraint+"))";
            #endregion

            int RowAffected;
            #region Open database connection and execute the command
            System.Data.SqlClient.SqlConnection oleDbConnection = (RecordOwnerObject.ObjectStorage as ObjectStorage).DBConnection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //			oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            System.Data.SqlClient.SqlCommand oleDbCommand = oleDbConnection.CreateCommand();
            oleDbCommand.CommandText = UpdateQuery;
            RowAffected = oleDbCommand.ExecuteNonQuery();
            #endregion

            #region Postcondition check
            if (RowAffected == 0)
            {
                oleDbCommand = oleDbConnection.CreateCommand();
                oleDbCommand.CommandText = "SELECT count(*) FROM " + KeepColumnTable.Name + " " + WhereClause;
                int Rows = (int)oleDbCommand.ExecuteNonQuery();
                if (Rows > 0)
                    throw new System.Exception("Multiplicity constraint mismatch at association '" + associationEnd.Association.Name + "' on object " + RecordOwnerObject.MemoryInstance.ToString());
            }
            #endregion

        }

        /// <MetaDataID>{386626BF-F008-49CB-90F5-CF171658278F}</MetaDataID>
        private void UpdateMappingDataIfNeeded()
        {

            //Get storage cells link for roleA,roleB storage cells
            RDBMSMetaDataRepository.Association Association = LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association;
            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            RDBMSMetaDataRepository.StorageCellsLink ObjectCollectionsLink = Association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath, true);

            //if storage cells ling produce new table then you must update MSSQL database schema. 
            //			if(ObjectCollectionsLink.NewTableCreated)
            //				this.OwnerTransactiont.EnlistCommand(new UpdateStorageSchema((ObjectStorage )RoleA.ObjectStorage));

            if (Association.LinkClass != null)
            {
                //Adds the storage cell of relation object to the association class storage cells of storage cells link 
                if (!ObjectCollectionsLink.AssotiationClassStorageCells.Contains(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
                {
                    ObjectCollectionsLink.AddAssotiationClassStorageCell(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet);
                    ObjectCollectionsLink.UpdateForeignKeys();
                    RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
                    if (!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                        OwnerTransactiont.EnlistCommand(updateStorageSchema);
                }
            }
            else if (StorageInstanceRef.GetStorageInstanceRef(ObjectCollectionsLink.Properties).ObjectID == null)
            {
                ObjectCollectionsLink.UpdateForeignKeys();

                RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
                if (!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    OwnerTransactiont.EnlistCommand(updateStorageSchema);
            }




        }

        /// <MetaDataID>{502555A3-C60E-44DA-B601-E3B381AA486D}</MetaDataID>
        public override void Execute()
        {
            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion

            base.Execute();
            if (MetaDataRepository.AssociationType.ManyToMany != LinkInitiatorAssociationEnd.Association.MultiplicityType && LinkInitiatorAssociationEnd.Association.LinkClass == null)
                LinkOnetoManyOneToOne(LinkInitiatorAssociationEnd);
            else
                LinkManyToMany(LinkInitiatorAssociationEnd);
        }
        /// <MetaDataID>{0F0C2C6E-9BA1-4B4C-B4E9-92A8B288EE6E}</MetaDataID>
        private void LinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
		{
			
			RDBMSMetaDataRepository.Association association=(RDBMSMetaDataRepository.Association)mAssociationEnd.Association;

			#region Gets metadata informations 
            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

            RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath);
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> roleAColumns = null;
			Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns=null;

			if(RelationObject!=null)
			{
				roleAColumns=((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
				roleBColumns=((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
			}
			else
			{
				roleAColumns=((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
				roleBColumns=((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);

			}
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> relationObjectIDColumns = null;
			if(association.LinkClass!=null)
				relationObjectIDColumns=(RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.ObjectIDColumns;
			#endregion

			#region Build SQL command

			string existCriterion=null;
            string indexRoleBCriterion=null;
            string indexRoleACriterion=null;
			string relationObjectIdentification=null;
			string setValues=null;
			string insertColumns=null;
			string insertValues=null;
			//With this SQL statement chack if the link between already exist.
			//if doesn't exist write it with the insert

			string LinkQuery="";
            bool setRoleAIndex = false;
            bool setRoleBIndex = false;
            if (this._LinkInitiatorAssociationEnd.Indexer)
                if (this._LinkInitiatorAssociationEnd.IsRoleA)
                    setRoleAIndex = true;
                else
                    setRoleBIndex = true;

            if (this._LinkInitiatorAssociationEnd.GetOtherEnd().Indexer)
                if (this._LinkInitiatorAssociationEnd.IsRoleA)
                    setRoleBIndex = true;
                else
                    setRoleAIndex = true;




			LinkQuery=@"declare @Rows int; 
						set @Rows=0; 
						declare @insertNewRow int; 
						set @insertNewRow=0; 
						SELECT   @Rows=COUNT(*) 
						FROM     [AssociationTable] 
						WHERE    ([ExistCriterion]) 
						if @Rows =0 
						begin ";

            string roleAIndexCalculation = @"declare @roleAIndex int;
                                        select @roleAIndex=COUNT(*) 						
                                        FROM     [AssociationTable] 
						                WHERE    ([IndexCriterion])" ;

            string preDefinedRoleAIndex= @"declare @roleAIndex int;
                                           set @roleAIndex = "+RoleAIndex.ToString(); 


            string roleBIndexCalculation = @"declare @roleBIndex int;
                                        select @roleBIndex=COUNT(*) 						
                                        FROM     [AssociationTable] 
						                WHERE    ([IndexCriterion])";

            string preDefinedRoleBIndex = @"declare @roleBIndex int;
                                           set @roleBIndex = " + RoleBIndex.ToString(); 

            string roleBUpdateRelationIndex = @"UPDATE [AssociationTable]
                                            SET     [IndexerColumn] = [IndexerColumn] + 1
                                            WHERE  ([IndexerColumn] >= @roleBIndex and [ListOwnerCriterion])";
            string roleAUpdateRelationIndex = @"UPDATE [AssociationTable]
                                            SET     [IndexerColumn] = [IndexerColumn] + 1
                                            WHERE  ([IndexerColumn] >= @roleAIndex and [ListOwnerCriterion])";




			
			if(association.LinkClass!=null)
				LinkQuery+="UPDATE  [AssociationTable] "+
							"SET    [SetValues] "+
							"WHERE  [RelationObjectIdentification] ";
			else
				LinkQuery+="set @insertNewRow=1; "+
							"INSERT INTO [AssociationTable] "+
							"			 ([InsertColumns])  "+
							"VALUES      ([InsertValues]) ";

			//If system add a new row return 1 else return 0 
			LinkQuery+="end SELECT  @insertNewRow as ROW_COUNT ";

			//[ExistCriterion]				: RoleA_FirstID = RoleA_Value_FirstID AND RoleA_SecondID = RoleA_Value_SecondID AND RoleB_FirstID = RoleB_Value_FirstID AND RoleB_SecondID = RoleB_Value_SecondID
			//[SetValues]					: RoleA_FirstID= RoleA_Value_FirstID, RoleA_SecondID= RoleA_Value_SecondID, RoleB_FirstID= RoleB_Value_FirstID, RoleB_SecondID= RoleB_Value_SecondID
			//[RelationObjectIdentification]: FirstID=FirstID_Value AND SecondID=SecondID_Value
			//[InsertColumns]				:RoleA_FirstID, RoleA_SecondID, RoleB_FirstID, RoleB_SecondID
			//[InsertValues]				:RoleA_Value_FirstID, RoleA_Value_SecondID, RoleB_Value_FirstID, RoleB_Value_SecondID

			if(association.LinkClass!=null)
			{
				//FirstID=FirstID_Value AND SecondID=SecondID_Value
				foreach(RDBMSMetaDataRepository.IdentityColumn column in relationObjectIDColumns)
				{
					if(relationObjectIdentification!=null)
						relationObjectIdentification+=" AND ";
					relationObjectIdentification+=" "+column.Name+" = "+TypeDictionary.ConvertToSQLString((RelationObject.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
					relationObjectIdentification+=" ";
				}
				LinkQuery=LinkQuery.Replace("[RelationObjectIdentification]",relationObjectIdentification);
			
			}
            if (RelationObject == null)
            {
                LinkQuery = LinkQuery.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleBIndexCalculation = roleBIndexCalculation.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleAIndexCalculation = roleAIndexCalculation.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                
            }
            else
            {
                LinkQuery = LinkQuery.Replace("[AssociationTable]", (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.Name);
                roleBIndexCalculation = roleBIndexCalculation.Replace("[AssociationTable]", (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.Name);
                roleAIndexCalculation = roleAIndexCalculation.Replace("[AssociationTable]", (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[AssociationTable]", (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[AssociationTable]", (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.Name);
            }

			foreach(RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
			{
				if(existCriterion!=null)
					existCriterion+=" AND ";
				string column_value=TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
				existCriterion+=" "+ column.Name+" = "+column_value;
				
				if(setValues!=null)
					setValues+=",";
				setValues+=" "+ column.Name+" = "+column_value;

				if(insertColumns!=null)
					insertColumns+=",";
				insertColumns+=" "+ column.Name+" ";

				if(insertValues!=null)
					insertValues+=",";
				insertValues+=" "+ column_value+" ";
                if (setRoleAIndex)
                {
                    if (indexRoleACriterion != null)
                        indexRoleACriterion += " AND ";
				    column_value=TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
				    indexRoleACriterion+=" "+ column.Name+" = "+column_value;
                }
			}
            if (setRoleAIndex)
            {
                if (insertColumns != null)
                    insertColumns += ",";
                insertColumns += " " + (_LinkInitiatorAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name + " ";
                if (insertValues != null)
                    insertValues += ",";
                insertValues += " @roleAIndex ";

                roleAIndexCalculation = roleAIndexCalculation.Replace("[IndexCriterion]", indexRoleACriterion);
                setValues += ", " + (_LinkInitiatorAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name + " = @roleAIndex" ;
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[IndexerColumn]", (_LinkInitiatorAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleACriterion);
                if(RoleAIndex==-1)
                    LinkQuery = roleAIndexCalculation + "\n" + roleAUpdateRelationIndex + "\n" + LinkQuery;
                else
                    LinkQuery = preDefinedRoleAIndex+ "\n" + roleAUpdateRelationIndex + "\n" + LinkQuery;

            }
            
			foreach(RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
			{
				if(existCriterion!=null)
					existCriterion+=" AND ";
				string column_value=TypeDictionary.ConvertToSQLString((RoleA.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
				existCriterion+=" "+ column.Name+" = "+column_value;
				
				if(setValues!=null)
					setValues+=",";
				setValues+=" "+ column.Name+" = "+column_value;

				if(insertColumns!=null)
					insertColumns+=",";
				insertColumns+=" "+ column.Name+" ";

				if(insertValues!=null)
					insertValues+=",";
				insertValues+=" "+ column_value+" ";

                if (setRoleBIndex)
                {
                    if (indexRoleBCriterion != null)
                        indexRoleBCriterion += " AND ";
				    column_value=TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
				    indexRoleBCriterion+=" "+ column.Name+" = "+column_value;
                }

			}
            if (setRoleBIndex)
            {
                if (insertColumns != null)
                    insertColumns += ",";
                insertColumns += " " + (_LinkInitiatorAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name + " ";
                if (insertValues != null)
                    insertValues += ",";
                insertValues += " @roleBIndex ";

                roleBIndexCalculation = roleBIndexCalculation.Replace("[IndexCriterion]", indexRoleBCriterion);
                setValues += ", " + (_LinkInitiatorAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name + " = @roleBIndex";
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[IndexerColumn]", (_LinkInitiatorAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleBCriterion);
                if(RoleBIndex==-1)
                    LinkQuery = roleBIndexCalculation + "\n" + roleAUpdateRelationIndex + "\n" + LinkQuery;
                else
                    LinkQuery = preDefinedRoleBIndex + "\n" + roleAUpdateRelationIndex + "\n" + LinkQuery;
            }



//            string roleBIndexCalculation = @"select @roleBIndex=COUNT(*) 						
//                                        FROM     [AssociationTable] 
//						                WHERE    ([IndexCriterion])";
            
			LinkQuery=LinkQuery.Replace("[ExistCriterion]",existCriterion);
			LinkQuery=LinkQuery.Replace("[SetValues]",setValues);
			LinkQuery=LinkQuery.Replace("[InsertColumns]",insertColumns);
			LinkQuery=LinkQuery.Replace("[InsertValues]",insertValues);
			#endregion


			#region Open database connection and execute the command 
			System.Data.SqlClient.SqlConnection oleDbConnection=((ObjectStorage)RoleA.ObjectStorage).DBConnection;
			if(oleDbConnection .State!=System.Data.ConnectionState.Open)
				oleDbConnection .Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
			//oleDbConnection .EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  oleDbCommand =oleDbConnection.CreateCommand();
			oleDbCommand.CommandText=LinkQuery;

			int insertedRows=0;
			System.Data.SqlClient.SqlDataReader myReader=oleDbCommand.ExecuteReader();
			while(myReader.Read())
			{
				insertedRows=(int)myReader["ROW_COUNT"];

				break;
			}
				myReader.Close();
			if(insertedRows!=0&&association.LinkClass==null)
				objectCollectionsLink.ObjectsLinksCount++;
			#endregion



			int lo=0;
		}

    }
}
