using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.RDBMSDataObjects;


namespace OOAdvantech.MSSQLCompactPersistenceRunTime.DataObjects
{
    /// <MetaDataID>{cff02c6b-9b05-4bb6-86ec-d52f1fa162be}</MetaDataID>
    class MSSQLCompactRDBMSSchema : OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator
    {

        public bool CriterionResolvedFromNativeSystem(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            if (criterion.OverridenComparisonOperator != null)
                return false;
            if (criterion.CriterionType == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType.CollectionContainsAnyAll)
                return false;
            return true;
        }

        public string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, System.Collections.Generic.IList<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn, int startIndex, int endIndex, int change)
        {

            string IndexCriterion = "";


            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                if (string.IsNullOrEmpty(IndexCriterion))
                    IndexCriterion = " WHERE ";
                else
                    IndexCriterion += " AND ";
                IndexCriterion += @"[" + column.DataBaseColumnName + @"] = " + DataBase.TypeDictionary.ConvertToSQLString(((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType));
            }

            string indexUpdate = null;
            if (change > 0)
            {
                indexUpdate = @"UPDATE [" + KeepColumnTable.DataBaseTableName + @"]
                                SET [" + IndexerColumn.DataBaseColumnName + @"] = [" + IndexerColumn.DataBaseColumnName + @"] + " + change.ToString()
                                     + IndexCriterion + @" AND [" + IndexerColumn.DataBaseColumnName + @"] >= " + startIndex.ToString()
                                     + @" AND [" + IndexerColumn.DataBaseColumnName + @"] <= " + endIndex.ToString() + " \n";
            }
            else
            {
                indexUpdate = @"UPDATE [" + KeepColumnTable.DataBaseTableName + @"]
                                SET """ + IndexerColumn.DataBaseColumnName + @"] = [" + IndexerColumn.DataBaseColumnName + @"] + " + change.ToString()
                + IndexCriterion + @" AND [" + IndexerColumn.DataBaseColumnName + @"] >= " + startIndex.ToString()
                + @" AND [" + IndexerColumn.DataBaseColumnName + @"] <= " + endIndex.ToString() + " \n";
            }

            return indexUpdate;
        }

        public string GhostTable
        {
            get
            {
                return "";
            }
        }

        public string GetDatePartSqlScript(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.Name == "Year")
            {
                return "DATEPART(year," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Month")
            {
                return "DATEPART(month," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Date")
            {
                return string.Format("cast({0} as date)", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
            }
            if (dataNode.Name == "Day")
            {
                return "DATEPART(day," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "DayOfWeek")
            {
                return "DATEPART(weekday," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "DayOfYear")
            {
                return "DATEPART(dayofyear," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Hour")
            {
                return "DATEPART(hour," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Minute")
            {
                return "DATEPART(minute," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Second")
            {
                return "DATEPART(second," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            return null;

        }
        ///// <MetaDataID>{44a648c2-cc08-4a1f-9ca9-662382645695}</MetaDataID>
        //public bool ValidateColumnName(Column column)
        //{
        //    return false;
        //}
        ///// <MetaDataID>{8c6ae810-7c73-4dd0-a6ba-64b3543d4dab}</MetaDataID>
        //public void ValidateTableName(Table table)
        //{
        //}
        ///// <MetaDataID>{0795b712-c0aa-4f4b-9143-fbcfd3b4b516}</MetaDataID>
        //public void ValidateKeyName(Key key)
        //{
        //}
        ///// <MetaDataID>{2443a3ce-c69a-4f9b-a8a7-b9e174876420}</MetaDataID>
        //public void ValidateViewName(View view)
        //{

        //}
        //public void ValidateStoreprocedureName(StoreProcedure storeProcedure)
        //{
        //}

        public string GetColumDefinitionScript(Column column)
        {

            string script = "[" + column.Name + "] " + column.Datatype;
            if (column.Length > 0)
                script += "(" + column.Length.ToString() + ")";
            script += " ";
            if (column.AllowNulls)
                script += "NULL";
            else
                script += "NOT NULL";
            if (column.IdentityColumn)
                script += " IDENTITY(1," + column.IdentityIncrement.ToString() + ")";
            return script;
        }

//        /// <MetaDataID>{d9557e0d-56fe-469e-bad3-204fbc0b0b4e}</MetaDataID>
//        public string BuildCountItemsInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table keepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns)
//        {
//            string indexCalculation = null;
//            string IndexCriterion = "";

//            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
//            {
//                if (string.IsNullOrEmpty(IndexCriterion))
//                    IndexCriterion = " WHERE ";
//                else
//                    IndexCriterion += " AND ";
//                IndexCriterion += column.Name + " = " + DataBase.TypeDictionary.ConvertToSQLString(((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.ObjectID).GetMemberValue(column.ColumnType));
//            }

//            indexCalculation = null;
//            indexCalculation = @"select COUNT(*) as [index] 						
//                                        FROM     " + keepColumnTable.Name + "\n";
//            indexCalculation = indexCalculation + IndexCriterion + "\n";

//            return indexCalculation;
//        }


        DataBase DataBase;
        System.Data.Common.DbConnection Connection
        {
            get
            {
                return DataBase.Connection;
            }
        }
        public MSSQLCompactRDBMSSchema(DataBase dataBase)
        {
            DataBase = dataBase;
        }
        #region SQLStatamentBuilder
        /// <MetaDataID>{e2001bde-e7eb-4f22-883d-2cc39d5d81c2}</MetaDataID>
        public System.Collections.Generic.List<string> GetStoreProcedureNames()
        {
            System.Collections.Generic.List<string> storeProcedureNames = new System.Collections.Generic.List<string>();
            return storeProcedureNames;
            //try
            //{
            //    if (Connection.State != System.Data.ConnectionState.Open)
            //        Connection.Open();
            //    System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
            //    command.CommandText = "exec sp_stored_procedures";
            //    System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
            //    while (dataReader.Read())
            //    {
            //        string storeprocedureName = (string)dataReader["PROCEDURE_NAME"];
            //        if ((string)dataReader["PROCEDURE_OWNER"] == "sys")
            //            continue;
            //        storeProcedureNames.Add(storeprocedureName.Substring(0, storeprocedureName.Length - 2));
            //    }
            //    dataReader.Close();
            //}
            //catch (System.Exception Error)
            //{
            //}
            //return storeProcedureNames;

        }
        /// <MetaDataID>{c1fa94dc-bdee-49df-abda-7154354b5c62}</MetaDataID>
        public System.Collections.Generic.List<string> GetViewsNames()
        {
            System.Collections.Generic.List<string> viewsNames = new System.Collections.Generic.List<string>();
            return viewsNames;
            //try
            //{
            //    if (Connection.State != System.Data.ConnectionState.Open)
            //        Connection.Open();
            //    System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
            //    command.CommandText = "exec sp_tables";
            //    System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
            //    while (dataReader.Read())
            //    {
            //        if ((string)dataReader["TABLE_TYPE"] == "VIEW")
            //        {
            //            string viewName = dataReader["TABLE_NAME"].ToString();
            //            viewsNames.Add(viewName);
            //        }
            //    }
            //    dataReader.Close();
            //}
            //catch (System.Exception Error)
            //{
            //}
            //return viewsNames;
        }
        /// <MetaDataID>{50a6ca6d-2474-478c-9683-80752ca458cf}</MetaDataID>
        public System.Collections.Generic.List<string> GetTablesNames()
        {
            System.Collections.Generic.List<string> tableNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
                    Connection.Open();
                System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "SELECT     * FROM         INFORMATION_SCHEMA.Tables";
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    if ((string)dataReader["TABLE_TYPE"] == "TABLE")
                    {
                        string tableName = dataReader["TABLE_NAME"].ToString();
                        tableNames.Add(tableName);
                    }
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return tableNames;
        }
        /// <MetaDataID>{0504f997-69eb-4cd1-bd15-cd36b34deb14}</MetaDataID>
        public string CreateManyToManyTransferDataSQLStatement(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, RDBMSMetaDataRepository.Table relationTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)//, out Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, out RDBMSMetaDataRepository.Table referenceColumnsTable)
        {
            string transferDataSQLStatement = null;
            string insertClause = null;
            string selectClause = null;
            string fromClause = null;

            #region Build INSERT and SELECT clause of transfer data SQL statement.
            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                if (insertClause == null)
                    insertClause = "INSERT INTO [" + relationTable.DataBaseTableName + "] (";
                else
                    insertClause += ",";

                insertClause += "[" + relationTableColumn.Name + "]";
                if (selectClause == null)
                    selectClause = "\nSELECT ";
                else
                    selectClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                        selectClause += "[" + (identityColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "].[" + identityColumn.Name + "]";
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                insertClause += ",[" + relationTableColumn.Name + "]";

                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                        selectClause += ",[" + (identityColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "].[" + identityColumn.Name + "]";
                }
            }
            insertClause += ")";
            #endregion


            #region Build FROM clause of transfer data SQL statement.
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = null;
            RDBMSMetaDataRepository.Table referenceColumnsTable = null;
            RDBMSMetaDataRepository.Table tableWithPrimaryKeyColumns = null;
            if (roleAColumns.Count == 0)
            {
                referenceColumns = roleBColumns;
                tableWithPrimaryKeyColumns = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
            }
            else
            {
                referenceColumns = roleAColumns;
                tableWithPrimaryKeyColumns = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
            }


            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
            {
                referenceColumnsTable = referenceColumn.Namespace as RDBMSMetaDataRepository.Table;
                if (fromClause == null)
                    fromClause = "\nFROM [" + tableWithPrimaryKeyColumns.DataBaseTableName + "]" + " INNER JOIN [" + referenceColumnsTable.DataBaseTableName + "] ON ";
                else
                    fromClause += ",";

                fromClause += "[" + (referenceColumnsTable).DataBaseTableName + "].[" + referenceColumn.DataBaseColumnName + "] = ";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in tableWithPrimaryKeyColumns.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == referenceColumn.ColumnType)
                        fromClause += "[" + tableWithPrimaryKeyColumns.DataBaseTableName + "].[" + identityColumn.DataBaseColumnName + "] ";
                }
            }
            #endregion

            transferDataSQLStatement = insertClause + selectClause + fromClause;
            return transferDataSQLStatement;
        }




        /// <MetaDataID>{ba2e2205-7ec0-486f-84de-5f97bf57a835}</MetaDataID>
        public List<string> CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)
        {
            #region Build data transfer SQL statement
            string transferQuery = null;
            string fromClause = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            {
                if (transferQuery == null)
                    transferQuery += "UPDATE   [" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "] \nset ";
                else
                    transferQuery += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
                {
                    if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
                        transferQuery += "[" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "]" + ".[" + roleBColumn.DataBaseColumnName + "]=[" +
                            roleAObjectIDColumn.Namespace.Name + "].[" + roleAObjectIDColumn.DataBaseColumnName + "]";
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                if (fromClause == null)
                    fromClause += "\nFROM [" + roleATable.Name + "]  INNER JOIN   [" + roleBTable.Name + "] ON ";
                else
                    fromClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
                {
                    if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
                        fromClause += "[" + (roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "]" + ".[" + roleAColumn.DataBaseColumnName + "]=[" +
                            roleBObjectIDColumn.Namespace.Name + "].[" + roleBObjectIDColumn.DataBaseColumnName + "]";
                }
            }
            transferQuery += fromClause;
            #endregion
            return new List<string>{ transferQuery};
        }
        /// <MetaDataID>{18d4d82b-0e52-4953-9ae5-b57a15314a26}</MetaDataID>
        public string BuildManyManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, RDBMSMetaDataRepository.Table referenceTable)
        {


            RDBMSMetaDataRepository.Table assoctiaionTable = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in mainTableAssociationColumns)
            {
                assoctiaionTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }



            string referenceCountTableSelectList = null;
            string referenceCountTableFirstInnerJoinCondition = null;
            string referenceCountTableSecondInnerJoinCondition = null;
            string objectIDsInnerJoinCondition = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn column in mainTable.ObjectIDColumns)
            {
                if (referenceCountTableSelectList != null)
                    referenceCountTableSelectList += ",";
                referenceCountTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName;

                if (referenceCountTableFirstInnerJoinCondition != null)
                    referenceCountTableFirstInnerJoinCondition += ",";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in mainTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableFirstInnerJoinCondition = (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName +
                            " = " + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + referenceColumn.DataBaseColumnName;
                }
                if (objectIDsInnerJoinCondition != null)
                    objectIDsInnerJoinCondition += ",";
                objectIDsInnerJoinCondition += "ReferenceCountTable." + column.DataBaseColumnName +
                    " = " + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName;

            }

            foreach (RDBMSMetaDataRepository.IdentityColumn column in referenceTable.ObjectIDColumns)
            {
                if (referenceCountTableSecondInnerJoinCondition != null)
                    referenceCountTableSecondInnerJoinCondition += ",";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableSecondInnerJoinCondition = (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "_R." + column.DataBaseColumnName +
                            " = " + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + referenceColumn.DataBaseColumnName;
                }
            }

            string updateQuery = "update " + mainTable.DataBaseTableName + "\r\n" +
                "set " + mainTable.DataBaseTableName + ".ReferenceCount=" + mainTable.DataBaseTableName + ".ReferenceCount+ReferenceCountTable.ReferenceCount\r\n" +
                "FROM  " + mainTable.DataBaseTableName + " INNER JOIN\r\n" +
                "(SELECT " + referenceCountTableSelectList + ",COUNT(*) AS ReferenceCount\r\n" +
                "FROM " + assoctiaionTable.DataBaseTableName + " INNER JOIN \r\n" +
                mainTable.DataBaseTableName + " ON " + referenceCountTableFirstInnerJoinCondition + "\r\n" +
                "INNER JOIN " + referenceTable.DataBaseTableName + " as " + referenceTable.DataBaseTableName + "_R  ON " + referenceCountTableSecondInnerJoinCondition + "\r\n" +
                "WHERE " + mainTable.DataBaseTableName + ".TypeID=" + ((mainTable.TableCreator as RDBMSMetaDataRepository.StorageCell).Type as RDBMSMetaDataRepository.Class).TypeID.ToString() + "\r\n" +
                "GROUP BY " + referenceCountTableSelectList + ") ReferenceCountTable ON " + objectIDsInnerJoinCondition;
            return updateQuery;


        }
        /// <MetaDataID>{44d830d2-5a11-4e37-9a08-c19e1a4e66c5}</MetaDataID>
        public string BuildManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, MetaDataRepository.AssociationEnd associationEnd)
        {

            RDBMSMetaDataRepository.Table referenceColumnTable = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in referenceColumns)
            {
                referenceColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }
            string referenceCountTableSelectList = null;
            string referenceCountTableInnerJoinCondition = null;
            string objectIDsInnerJoinCondition = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn column in mainTable.ObjectIDColumns)
            {
                if (referenceCountTableSelectList != null)
                    referenceCountTableSelectList += ",";
                referenceCountTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName;

                if (referenceCountTableInnerJoinCondition != null)
                    referenceCountTableInnerJoinCondition += ",";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableInnerJoinCondition = (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName +
                            " = " + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + referenceColumn.DataBaseColumnName;
                }
                if (objectIDsInnerJoinCondition != null)
                    objectIDsInnerJoinCondition += ",";
                objectIDsInnerJoinCondition += "ReferenceCountTable." + column.DataBaseColumnName +
                    " = " + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "." + column.DataBaseColumnName;

            }
            string updateQuery = "update " + mainTable.DataBaseTableName + "\r\n" +
                "set " + mainTable.DataBaseTableName + ".ReferenceCount=" + mainTable.DataBaseTableName + ".ReferenceCount+ReferenceCountTable.ReferenceCount\r\n" +
                "FROM  " + mainTable.DataBaseTableName + " INNER JOIN\r\n" +
                "(SELECT " + referenceCountTableSelectList + ",COUNT(*) AS ReferenceCount\r\n" +
                "FROM " + referenceColumnTable.DataBaseTableName + " INNER JOIN \r\n" +
                mainTable.DataBaseTableName + " ON " + referenceCountTableInnerJoinCondition + "\r\n" +
                "WHERE " + mainTable.DataBaseTableName + ".TypeID=" + ((mainTable.TableCreator as RDBMSMetaDataRepository.StorageCell).Type as RDBMSMetaDataRepository.Class).TypeID.ToString() + "\r\n" +
                "GROUP BY " + referenceCountTableSelectList + ") ReferenceCountTable ON " + objectIDsInnerJoinCondition;
            return updateQuery;
        }
        /// <MetaDataID>{48a47ab1-4d0e-4a54-8eea-f0df9f455d32}</MetaDataID>
        public string BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd)
        {

            string updateQuery = "UPDATE " + storageCell.MainTable.DataBaseTableName + "\r\n" +
                "Set ReferenceCount=ReferenceCount+1\r\n" +
                "FROM " + storageCell.MainTable.DataBaseTableName + "\r\nWHERE ";
            string whereCondition = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in storageCellAssociationEnd.GetReferenceColumnsFor(storageCell))
            {
                if (whereCondition != null)
                    whereCondition += " AND ";
                whereCondition += column.DataBaseColumnName + " is not null ";
            }
            updateQuery += whereCondition + " AND TypeID = " + (storageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString();
            return updateQuery;
        }



        public List<string> GetAddKeyScript(Key key)
        {
            if (key.IsPrimaryKey)
            {
                string Script = "ALTER TABLE " + key.OwnerTable.Name + " ADD CONSTRAINT \n" +
                    key.Name + " PRIMARY KEY  ( ";
                string ColumnsScript = null;

                //System.Collections.Hashtable SColumns = new System.Collections.Hashtable();
                //foreach (Column CurrColumn in Columns)
                //    SColumns.Add(CurrColumn.ColumnType, CurrColumn);

                foreach (Column column in key.Columns)
                {
                    if (ColumnsScript != null)
                        ColumnsScript += ",";
                    ColumnsScript += column.Name;
                }
                Script += ColumnsScript + ") ";
                List<string> commands = new List<string>();
                commands.Add(Script);
                return commands;
            }
            else
            {
                string Script = "ALTER TABLE " + key.OwnerTable.Name + " ADD CONSTRAINT \n" +
                    key.Name + " FOREIGN KEY ( ";
                string ColumnsScript = null;
                foreach (Key tableKey in key.OwnerTable.Keys)
                {
                    if (!tableKey.IsPrimaryKey && key != tableKey)
                    {
                        foreach (Column column in tableKey.Columns)
                        {
                            foreach (Column scriptKeyColumn in key.Columns)
                            {
                                if (scriptKeyColumn == column)
                                    return new List<string>();
                            }
                        }

                    }
                }
                //System.Collections.Hashtable SColumns = new System.Collections.Hashtable();
                //foreach (Column CurrColumn in Columns)
                //    SColumns.Add(CurrColumn.ColumnType, CurrColumn);
                foreach (Column column in key.Columns)
                {
                    if (ColumnsScript != null)
                        ColumnsScript += ",";
                    ColumnsScript += column.Name;
                }
                Script += ColumnsScript + " ) REFERENCES " + key.ReferencedTable.Name + " ( ";
                ColumnsScript = null;

                foreach (Column column in key.ReferedColumns)
                {
                    if (ColumnsScript != null)
                        ColumnsScript += ",";
                    ColumnsScript += column.Name;
                }
                Script += ColumnsScript + " )  ";
                //+"\n  ALTER TABLE " + key.OwnerTable.Name +
                //" NOCHECK CONSTRAINT  " + key.Name;
                List<string> commands = new List<string>();
                commands.Add(Script);
                return commands;
            }
        }

        public string GetDropKeyScript(Key key)
        {
            return "ALTER TABLE " + key.OwnerTable.Name + " DROP CONSTRAINT " + key.Name;
        }

//        public void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames)
//        {
//            System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();
//            DataBase dataBase = key.Namespace.Namespace as DataBase;
//            System.Data.Common.DbConnection connection = dataBase.Connection;

//            string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
//            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
//            command.CommandText = CommandString;
//            System.Data.Common.DbDataReader DataReader = command.ExecuteReader();
//            foreach (System.Data.Common.DbDataRecord record in DataReader)
//            {
//                if (record["FK_NAME"].Equals(key.Name))
//                {
//                    string PKColumnName = (string)record["PKCOLUMN_NAME"];
//                    string FKColumnName = (string)record["FKCOLUMN_NAME"];
//                    columnsNames.Add(FKColumnName);
//                    referedColumnsNames.Add(PKColumnName);


//                    //Column column = key.OwnerTable.GetColumn(FKColumnName);
//                    ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
//                    ////Το σωστό είναι  Πρέπει να είναι ordered
//                    ////if (column.ColumnType == null)
//                    ////    column.ColumnType = Columns.Count.ToString();

//                    //Columns.Add(column);

//                    //column = key.ReferencedTable.GetColumn(PKColumnName);
//                    ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
//                    ////Το σωστό είναι  Πρέπει να είναι ordered
//                    ////if (column.ColumnType == null)
//                    ////    column.ColumnType = ReferedColumns.Count.ToString();
//                    //ReferedColumns.Add(column);
//                    int kj = 0;

//                }
//            }
//            DataReader.Close();
//            //Connection.Close();
//        }
//        public void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames)
//        {
//            DataBase dataBase = key.Namespace.Namespace as DataBase;
//            System.Data.Common.DbConnection connection = dataBase.Connection;

//            //string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
//            //string CommandString = "exec sp_pkeys '" + key.OwnerTable.Name + "'";

//            string CommandString = string.Format(@"SELECT  INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME, 
//                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME AS PK_NAME
//                      FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE CROSS JOIN
//                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
//                      WHERE     (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = '{0}') AND 
//                      (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME <> INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME)", key.OwnerTable.Name);

//            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
//            command.CommandText = CommandString;
//            System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
//            while (dataReader.Read())
//            {
//                if (dataReader["PK_NAME"].Equals(key.Name))
//                {
//                    string PKColumnName = (string)dataReader["COLUMN_NAME"];
//                    columnsNames.Add(PKColumnName);

//                }
//            }
//            dataReader.Close();
//            //Connection.Close();


//        }


        /// <MetaDataID>{5d87a1a0-b06e-4957-aefa-b425f58e0598}</MetaDataID>
        public System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName)
        {
            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();

            System.Data.Common.DbConnection connection = Connection;

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);


                //                command.CommandText = string.Format(@"Select IDENT_INCR from (SELECT IDENT_INCR(TABLE_NAME) AS IDENT_INCR 
                //								FROM INFORMATION_SCHEMA.TABLES 
                //								WHERE TABLE_NAME='{0}') IdentityData
                //                                where IdentityData.IDENT_INCR is not NULL", tableName);

                //                int identityIncrement = 0;
                //                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
                //                foreach (System.Data.Common.DbDataRecord record in dataReader)
                //                {
                //                    identityIncrement = (int)System.Convert.ChangeType(record["IDENT_INCR"], typeof(int)); ;
                //                    break;
                //                }
                //                dataReader.Close();


                command.CommandText = string.Format(@"SELECT     *
                            FROM         INFORMATION_SCHEMA.columns
                            WHERE     (Table_Name = '{0}')", tableName);

                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
                // Load table columns
                while (dataReader.Read())
                {
                    string dataType = (string)dataReader["DATA_TYPE"];
                    bool identityColumn = !(dataReader["AUTOINC_SEED"] is System.DBNull);
                    int identityIncrement = 0;
                    if (identityColumn)
                        identityIncrement = (int)(long)dataReader["AUTOINC_INCREMENT"];

                    int length = 0;
                    if (!(dataReader["CHARACTER_MAXIMUM_LENGTH"] is DBNull))
                        length = (int)dataReader["CHARACTER_MAXIMUM_LENGTH"];

                    ColumnData columnData = new ColumnData(dataReader["COLUMN_NAME"].ToString(), dataType, length, (dataReader["IS_NULLABLE"] as string) == "YES", identityColumn, identityIncrement);
                    columnsData.Add(columnData);
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
                System.Diagnostics.Debug.Assert(false);
                throw new System.Exception("Table with name '" + tableName + "'failed to read its columns", Error);
            }
            return columnsData;

        }
//        public System.Collections.Generic.List<string> RetrieveDatabasePrimaryKeys(Table table)
//        {
//            System.Collections.Generic.List<string> primaryKeys = new System.Collections.Generic.List<string>();
//            System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
//            if (connection.State != System.Data.ConnectionState.Open)
//                connection.Open();
//            System.Data.Common.DbCommand Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
//            Command.CommandText = string.Format(@"SELECT     INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME
//                                        FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE LEFT OUTER JOIN
//                                                              INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
//                                                              INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME <> INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME
//                                        WHERE     (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = '{0}') AND 
//                                                              (INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME IS NULL)", table.Name);



//            //                string.Format(@"SELECT     INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME
//            //                                                FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE CROSS JOIN
//            //                                                                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
//            //                                                WHERE     (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = '{0}') AND 
//            //                                              (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME <> INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME)", table.Name);





//            //            @"SELECT     INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME, 
//            //                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME AS REFERENCE_COLUMNS, 
//            //                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_TABLE_NAME, 
//            //                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_TABLE_NAME, 
//            //                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME, PK_COLUMNS.COLUMN_NAME
//            //                      FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE CROSS JOIN
//            //                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS CROSS JOIN
//            //                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK_COLUMNS
//            //                      WHERE     (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = 'Table2') AND 
//            //                      (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME) 
//            //                      AND (PK_COLUMNS.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME)";


//            System.Data.Common.DbDataReader dataReader = Command.ExecuteReader();
//            // Load table Primary Keys
//            while (dataReader.Read())
//                primaryKeys.Add((string)dataReader["CONSTRAINT_NAME"]);
//            dataReader.Close();
//            return primaryKeys;
//        }

        public System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table)
        {
            System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            System.Data.Common.DbCommand Command = connection.CreateCommand();
            Command.CommandText = @"SELECT     INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME AS FK_NAME, 
                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME AS COLUMN_NAME , PK_COLUMNS.COLUMN_NAME AS REF_COLUMN_NAME,
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_TABLE_NAME AS FK_TABLE_NAME, 
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_TABLE_NAME AS REF_TABLE_NAME,
                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE.ORDINAL_POSITION as ColumnOrder
                      FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE CROSS JOIN
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS CROSS JOIN
                      INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK_COLUMNS
                      WHERE     (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = N'" + table.Name + @"') AND 
                      (INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME) 
                      AND (PK_COLUMNS.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME) 
                      ORDER BY FK_NAME ,ColumnOrder";


            System.Data.Common.DbDataReader dataReader = Command.ExecuteReader();
            System.Collections.Generic.Dictionary<string, ForeignKeyData> foreignKeysData = new Dictionary<string, ForeignKeyData>();

            foreach (System.Data.Common.DbDataRecord CurrRecord in dataReader)
            {
                string foreignKeyName = CurrRecord["FK_NAME"] as string;
                ForeignKeyData fkeyData = null;
                if (!foreignKeysData.TryGetValue(foreignKeyName, out fkeyData))
                {
                    fkeyData = new ForeignKeyData((string)CurrRecord["FK_NAME"], (string)CurrRecord["FK_TABLE_NAME"], (string)CurrRecord["REF_TABLE_NAME"]);
                    foreignKeysData[foreignKeyName] = fkeyData;
                }
                fkeyData.ColumnsNames.Add((string)CurrRecord["COLUMN_NAME"]);
                fkeyData.ReferedColumnsNames.Add((string)CurrRecord["REF_COLUMN_NAME"]);
            }

            dataReader.Close();
            return new List<ForeignKeyData>(foreignKeysData.Values);

        }
        public string GetDataBaseUpdateScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType)
        {
            string script = null;

            string columnsScript = null;
            foreach (Column addedColumn in newColumns)
            {
                if (columnsScript != null)
                    columnsScript += ",\n";
                columnsScript += addedColumn.GetScript();

            }
            if (string.IsNullOrEmpty(columnsScript))
                return null;


            if (newTable)
            {
                if (tableType == TableType.VaraibleTable)
                    script = "CREATE TABLE @" + tableName + " (\n" + columnsScript + ")";
                else if (tableType == TableType.TemporaryTable)
                    script = "CREATE TABLE #" + tableName + " (\n" + columnsScript + ")";
                else
                    script = "CREATE TABLE " + tableName + " (\n" + columnsScript + ")";
            }
            else
            {
                if (tableType == TableType.VaraibleTable)
                    script = "ALTER TABLE @" + tableName + " ADD \n" + columnsScript;
                else if (tableType == TableType.TemporaryTable)
                    script = "ALTER TABLE #" + tableName + " ADD \n" + columnsScript;
                else
                    script = "ALTER TABLE " + tableName + " ADD \n" + columnsScript;
            }

            return script;
        }



        string GetDataTypeCoversionStatement(string columnName, int columnLength, string orgType, string newType)
        {
            if (ExcludedConvertion.Contains(orgType.ToLower() + "-" + newType.ToLower()))
                throw new System.Exception("ExcludedConvertion" + orgType.ToLower() + "-" + newType.ToLower());

            string convertStatement = "CONVERT(" + newType;
            if (columnLength > 0)
                convertStatement += "(" + columnLength.ToString() + ")";
            convertStatement += "," + columnName + ")";
            return convertStatement;

        }

        static System.Collections.ArrayList ExcludedConvertion = new System.Collections.ArrayList();
        static MSSQLCompactRDBMSSchema()
        {
            ExcludedConvertion.Add("int-image");
            ExcludedConvertion.Add("nchar-image");
            ExcludedConvertion.Add("nvarchar-image");
            ExcludedConvertion.Add("datetime-image");
            ExcludedConvertion.Add("bigint-image");
            ExcludedConvertion.Add("smallint-image");
            ExcludedConvertion.Add("tinyint-image");
            ExcludedConvertion.Add("decimal-image");
            ExcludedConvertion.Add("numeric-image");
            ExcludedConvertion.Add("money-image");
            ExcludedConvertion.Add("smallmoney-image");
            ExcludedConvertion.Add("float-image");
            ExcludedConvertion.Add("real-image");
            ExcludedConvertion.Add("smalldatetime-image");
            ExcludedConvertion.Add("ntext-image");
            ExcludedConvertion.Add("text-image");
            ExcludedConvertion.Add("sql_variant-image");
            ExcludedConvertion.Add("uniqueidentifier-image");


        }


        public void UpdateColumnsFormat(Table table)
        {

            string createTemporaryTable = null;
            string transferDataCommand = null;
            string sourceColumns = null;
            string destColumns = null;


            {
                DataBase dataBase = table.Namespace as DataBase;
                System.Data.Common.DbConnection connection = dataBase.Connection;
                //if(connection==null)
                System.Data.Common.DbCommand command = null;
                object resault = null;
                try
                {
                    foreach (Column column in table.Columns)
                    {
                        if (createTemporaryTable != null)
                        {
                            sourceColumns += ",";
                            destColumns += ",";
                            createTemporaryTable += ",\n\r";
                        }
                        else
                            createTemporaryTable += "CREATE TABLE TMP_" + table.Name + " (\n\r";
                        destColumns += column.Name;
                        if (column.HasChangeColumnFormat)
                            sourceColumns += GetDataTypeCoversionStatement(column.Name, column.Length, column.DataBaseColumnDataType, column.Datatype);
                        else
                            sourceColumns += column.DataBaseColumnName;
                        createTemporaryTable += column.GetScript();
                    }
                    createTemporaryTable += ")";
                    transferDataCommand = "INSERT INTO TMP_" + table.Name + "(" + destColumns + ")\nSELECT " + sourceColumns + " FROM " + table.Name + " TABLOCKX";
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(createTemporaryTable, connection);
                    command.CommandText = createTemporaryTable;
                    resault = command.ExecuteNonQuery();
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(transferDataCommand, connection);
                    command.CommandText = transferDataCommand;
                    resault = command.ExecuteNonQuery();
                    foreach (Key key in table.GetReferenceKeys())
                    {
                        if (!key.NewKey)
                        {
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(key.GetDropKeyScript(),connection);
                            command.CommandText = GetDropKeyScript(key);
                            resault = command.ExecuteNonQuery();
                        }
                    }
                    foreach (Key key in table.Keys)
                    {
                        if (!key.NewKey && !key.IsPrimaryKey)
                        {
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(key.GetDropKeyScript(), connection);
                            command.CommandText = GetDropKeyScript(key);
                            resault = command.ExecuteNonQuery();
                        }
                    }
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(string.Format("DROP TABLE {0}",Name),connection);
                    command.CommandText = string.Format("DROP TABLE {0}", table.Name);
                    resault = command.ExecuteNonQuery();
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'","TMP_"+Name,Name),connection);
                    command.CommandText = string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", "TMP_" + table.Name, table.Name);
                    resault = command.ExecuteNonQuery();
                    foreach (Key key in table.Keys)
                    {
                        if (!key.NewKey)
                        {
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(key.GetAddKeyScript(),connection);
                            foreach (string addKeyScript in GetAddKeyScript(key))
                            {
                                command.CommandText = addKeyScript;
                                resault = command.ExecuteNonQuery();
                            }
                        }
                    }
                    foreach (Key key in table.GetReferenceKeys())
                    {
                        if (!key.NewKey)
                        {
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(key.GetAddKeyScript(),connection);
                            foreach (string addKeyScript in GetAddKeyScript(key))
                            {
                                command.CommandText = addKeyScript;
                                resault = command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw new System.Exception("It can't save the table '" + table.Name + "' changes.", error);
                }
            }
        }
        public string GetChangeTableNameScript(string tableName, string newTableName)
        {
            return string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", tableName, newTableName);
        }
        public string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName)
        {
            return string.Format("EXECUTE sp_rename N'{0}.{1}', N'{2}', 'COLUMN'", tableName, columnName, newColumnName);
        }
        public void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns)
        {
            string dropColumnsScript = null;
            foreach (Column column in dropedColumns)
            {
                if (dropColumnsScript == null)
                    dropColumnsScript = string.Format("ALTER TABLE [{0}] \nDROP COLUMN ", tableName);
                else
                    dropColumnsScript += "'";
                dropColumnsScript += "[" + column.Name + "]";
            }
            if (dropColumnsScript != null)
            {
                System.Data.Common.DbCommand command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropColumnsScript, connection);
                command.CommandText = dropColumnsScript;
                command.ExecuteNonQuery();
            }
        }
        public string GetDefinition(RDBMSMetaDataRepository.View view, bool newView)
        {
            string AbstractClassView = "";
            return "";

            if (newView)
                AbstractClassView += "CREATE VIEW ";
            else
                AbstractClassView += "ALTER VIEW ";
            AbstractClassView += "[" + view.Name + "]\nAS\n";
            string SelectList = null;
            SelectList = "Select ";
            string Separator = null;
            foreach (RDBMSMetaDataRepository.Column CurrColumn in view.ViewColumns)
            {

                if ((CurrColumn.DefaultValue != null && CurrColumn.DefaultValue.Length > 0) && CurrColumn.RealColumn == null)
                {
                    SelectList += Separator + CurrColumn.DefaultValue + " AS " + CurrColumn.Name;
                    if (Separator == null)
                        Separator = ",";
                }
                else
                {
                    if (view.JoinedTables.Count > 0)
                    {
                        //TODO αυτό γίνεται γιατί κάπιος για κάποιο λόγο σβήνεται η column από το table
                        //θα πρέπει να σβήνουμε τα reference columns όταν σβήνεται ένα table column
                        if (CurrColumn.RealColumn == null)
                        {
                            CurrColumn.DefaultValue = DataBase.TypeDictionary.GetDBNullScript(CurrColumn.Type.FullName);
                            SelectList += Separator + CurrColumn.DefaultValue + " AS " + CurrColumn.Name;
                            if (Separator == null)
                                Separator = ",";
                        }
                        else
                        {
                            if (CurrColumn.RealColumn.Namespace != null)
                                SelectList += Separator + CurrColumn.RealColumn.Namespace.Name + "." + CurrColumn.RealColumn.Name;
                            else
                                SelectList += Separator + CurrColumn.RealColumn.Name;

                            SelectList += " as " + CurrColumn.Name;
                            if (Separator == null)
                                Separator = ",";
                        }
                    }
                    else
                    {
                        SelectList += Separator + "[" + CurrColumn.Name + "]";
                        if (Separator == null)
                            Separator = ",";
                    }
                }
            }
            string FromClause = null;//"From "+ObjectCollection.MainTable.Name+" "; 
            RDBMSMetaDataRepository.Table MainTable = null;
            foreach (RDBMSMetaDataRepository.Table CurrTable in view.JoinedTables)
            {
                if (FromClause == null)
                {
                    FromClause = "From " + CurrTable.Name + " ";
                    MainTable = CurrTable;
                }
                else
                {
                    FromClause += " INNER JOIN " + CurrTable.Name + " ON " + MainTable.Name + ".IntObjID = " + CurrTable.Name + ".IntObjID ";
                    FromClause += "AND " + MainTable.Name + ".ObjCellID = " + CurrTable.Name + ".ObjCellID ";
                }
            }
            string ViewQuery = null;

            //string ViewQuery="\nWHERE "+ObjectCollection.MainTable.Name+".TypeID = "+ObjectCollection.Type.theStorageInstanceRef.ObjectID.ToString();
            if (view.JoinedTables.Count > 0)
            {
                ViewQuery = SelectList + "\n" + FromClause + "\n";//WHERE "+ObjectCollection.MainTable.Name+".TypeID = "+ObjectCollection.Type.theStorageInstanceRef.ObjectID.ToString();
                if (view.StorageCell != null)
                    ViewQuery += "WHERE " + view.StorageCell.MainTable.Name + ".TypeID = " + (view.StorageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString();
            }

            foreach (RDBMSMetaDataRepository.View CurrView in view.SubViews)
            {
                if (ViewQuery == null)
                    ViewQuery += "\nSelect ";
                else
                {
                    ViewQuery += "\nUnion All\nSelect ";
                }
                Separator = null;
                foreach (RDBMSMetaDataRepository.Column CurrColumn in view.ViewColumns)
                {
                    bool exist = false;
                    foreach (string ColumnName in CurrView.ViewColumnsNames)
                    {
                        if (ColumnName.ToLower().Trim() == CurrColumn.Name.ToLower().Trim())
                        {
                            ViewQuery += Separator + CurrColumn.Name;
                            if (Separator == null)
                                Separator = ",";
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        string nullValue = "NULL";
                        if (CurrColumn.Type != null)
                            nullValue = DataBase.TypeDictionary.GetDBNullScript(CurrColumn.Type.FullName);

                        ViewQuery += Separator + nullValue + " AS " + CurrColumn.Name;
                        if (Separator == null)
                            Separator = ",";
                    }
                }
                ViewQuery += "\nFrom " + CurrView.Name;

            }

            if (ViewQuery == null)
            {
                string whereClause = null;
                foreach (RDBMSMetaDataRepository.Column column in view.ViewColumns)
                {
                    string nullValue = "NULL";
                    if (column.Type != null)
                        nullValue = DataBase.TypeDictionary.GetDBNullScript(column.Type.FullName);

                    if (ViewQuery == null)
                    {
                        whereClause = "WHERE " + column.Name + " <> " + nullValue;
                        ViewQuery += "SELECT " + nullValue + " AS " + column.Name;
                    }
                    else
                        ViewQuery += "," + nullValue + " AS " + column.Name;
                }
                ViewQuery = SelectList + " FROM (" + ViewQuery + ") [TABLE] " + whereClause;
            }

            AbstractClassView +=/*"\n"+SelectList+"\n"*/ViewQuery;
            System.Diagnostics.Debug.WriteLine(AbstractClassView);
            return AbstractClassView;
        }
        public string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {
            return "";
            switch (storeProcedure.Type)
            {
                case RDBMSMetaDataRepository.StoreProcedure.Types.New:
                    return BuildNewInstanceStoreProcedure(storeProcedure, create);
                case RDBMSMetaDataRepository.StoreProcedure.Types.Update:
                    return BuildUpdateInstanceStoreProcedure(storeProcedure, create);
                case RDBMSMetaDataRepository.StoreProcedure.Types.Delete:
                    return BuildDeleteInstanceStoreProcedure(storeProcedure, create);
            }
            return "";

        }
        private string BuildDeleteInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {
            /*####################### Create StoreProcedure parameters ##################*/
            string DeleteStoreProcedure = null;
            if (create)
                DeleteStoreProcedure = "Create ";
            else
                DeleteStoreProcedure = "Alter ";

            DeleteStoreProcedure += "Procedure dbo." + storeProcedure.Name + "\n";
            string PrameterList = null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";

            foreach (MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
            {
                if (PrameterList != null)
                    PrameterList += ",\n";
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

                PrameterList += "@" + CurrParameter.Name + " " + DataBase.TypeDictionary.GetDBType(parameterType);
                if (DataBase.TypeDictionary.IsTypeVarLength(parameterType))
                {
                    object mLength = CurrParameter.GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                    if (mLength != null)
                    {
                        if (((int)mLength) == 0)
                            PrameterList += "(" + DataBase.TypeDictionary.GeDefaultLength(parameterType).ToString() + ")";
                        else
                            PrameterList += "(" + mLength.ToString() + ")";
                    }
                }
            }
            DeleteStoreProcedure += PrameterList;
            DeleteStoreProcedure += "\nAS\n";
            /*####################### Create StoreProcedure parameters ##################*/



            /*####################### Update main table columns ##################*/
            DeleteStoreProcedure += "DELETE FROM " + storeProcedure.ActsOnStorageCell.MainTable.Name;
            DeleteStoreProcedure += "\nWHERE(";
            int Count = 0;
            foreach (RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
            {
                if (Count != 0)
                    DeleteStoreProcedure += " AND ";
                DeleteStoreProcedure += Column.Name + " = @" + Column.Name;
                Count++;
            }
            DeleteStoreProcedure += ")";

            /*####################### Update main table columns ##################*/


            /*####################### Update mapped tables columns ##################*/
            foreach (RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
            {
                if (CurrTable == storeProcedure.ActsOnStorageCell.MainTable)
                    continue;
                DeleteStoreProcedure += "\nDELETE FROM " + CurrTable.Name;
                DeleteStoreProcedure += "\nWHERE(";
                Count = 0;
                foreach (RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
                {
                    if (Count != 0)
                        DeleteStoreProcedure += " AND ";
                    DeleteStoreProcedure += Column.Name + " = @" + Column.Name;
                    Count++;
                }
                DeleteStoreProcedure += ")";
            }
            /*####################### Update mapped tables columns ##################*/
            return DeleteStoreProcedure;

        }
        private string BuildUpdateInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {

            /*####################### Create StoreProcedure parameters ##################*/

            string UpdateStoreProcedure = null;
            if (create)
                UpdateStoreProcedure = "Create ";
            else
                UpdateStoreProcedure = "Alter ";

            UpdateStoreProcedure += "Procedure dbo." + storeProcedure.Name + "\n";


            string PrameterList = null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";
            System.Collections.Specialized.HybridDictionary ClassAttriutes = new System.Collections.Specialized.HybridDictionary();

            foreach (MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
            {
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

                ClassAttriutes.Add(CurrParameter.Name, CurrParameter);
                if (PrameterList != null)
                    PrameterList += ",\n";
                PrameterList += "@" + CurrParameter.Name + " " + DataBase.TypeDictionary.GetDBType(parameterType);
                if (DataBase.TypeDictionary.IsTypeVarLength(parameterType))
                {
                    object mLength = CurrParameter.GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                    if (mLength != null)
                    {
                        if (((int)mLength) == 0)
                            PrameterList += "(" + DataBase.TypeDictionary.GeDefaultLength(parameterType).ToString() + ")";
                        else
                            PrameterList += "(" + mLength.ToString() + ")";
                    }
                }
            }
            UpdateStoreProcedure += PrameterList;
            UpdateStoreProcedure += "\nAS\n";

            /*####################### Create StoreProcedure parameters ##################*/

            /*####################### Update main table columns ##################*/
            UpdateStoreProcedure += "UPDATE   " + storeProcedure.ActsOnStorageCell.MainTable.Name + "\nSet ";
            bool FirstColumn = true;
            foreach (RDBMSMetaDataRepository.Column CurrColumn in storeProcedure.ActsOnStorageCell.MainTable.ContainedColumns)
            {
                if (CurrColumn.MappedAttribute != null)
                {
                    //string parameterName = CurrColumn.MappedAttribute.CaseInsensitiveName;
                    string parameterName = CurrColumn.Name;


                    //					if(CurrColumn.Name.Trim().ToLower()!=CurrColumn.MappedAttribute.CaseInsensitiveName.Trim().ToLower())
                    //						parameterName+=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
                    //					else
                    //						parameterName=CurrColumn.Name;


                    if (ClassAttriutes.Contains(parameterName))
                    {
                        if (FirstColumn)
                        {
                            UpdateStoreProcedure += CurrColumn.Name + " = " + "@";
                            FirstColumn = false;
                        }
                        else
                            UpdateStoreProcedure += "," + CurrColumn.Name + " = " + "@";
                        UpdateStoreProcedure += parameterName;
                    }
                }
            }




            if (storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn != null)
            {
                if (ClassAttriutes.Contains(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name))
                {
                    if (FirstColumn)
                    {
                        UpdateStoreProcedure += storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name + " = " + "@" + storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
                        FirstColumn = false;
                    }
                    else
                        UpdateStoreProcedure += "," + storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name + " = " + "@" + storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
                }
            }

            UpdateStoreProcedure += "\nWHERE(";
            int Count = 0;
            foreach (RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
            {
                if (Count != 0)
                    UpdateStoreProcedure += " AND ";
                UpdateStoreProcedure += Column.Name + " = @" + Column.Name;
                Count++;
            }
            UpdateStoreProcedure += ")";


            /*####################### Update main table columns ##################*/


            /*####################### Update mapped tables columns ##################*/
            foreach (RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
            {
                if (CurrTable == storeProcedure.ActsOnStorageCell.MainTable)
                    continue;
                UpdateStoreProcedure += "\nUPDATE   " + CurrTable.Name + "\nSet ";
                FirstColumn = true;
                foreach (RDBMSMetaDataRepository.Column CurrColumn in CurrTable.ContainedColumns)
                {
                    if (CurrColumn.MappedAttribute != null)
                    {
                        if (ClassAttriutes.Contains(CurrColumn.MappedAttribute.Name))
                        {
                            if (FirstColumn)
                            {
                                UpdateStoreProcedure += CurrColumn.Name + " = " + "@";
                                FirstColumn = false;
                            }
                            else
                                UpdateStoreProcedure += "," + CurrColumn.Name + " = " + "@";
                        }
                        //UpdateStoreProcedure += CurrColumn.MappedAttribute.CaseInsensitiveName;
                        UpdateStoreProcedure += CurrColumn.Name;
                        //						if(CurrColumn.Name.Trim().ToLower()==CurrColumn.MappedAttribute.Name.Trim().ToLower())
                        //							UpdateStoreProcedure+=CurrColumn.Name;
                        //						else
                        //							UpdateStoreProcedure+=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;

                    }
                }
                UpdateStoreProcedure += "\nWHERE(";
                Count = 0;
                foreach (RDBMSMetaDataRepository.Column Column in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(storeProcedure).Parts)
                {
                    if (Count != 0)
                        UpdateStoreProcedure += " AND ";
                    UpdateStoreProcedure += Column.Name + " = @" + Column.Name;
                    Count++;
                }
                UpdateStoreProcedure += ")";

            }
            return UpdateStoreProcedure;
            /*####################### Update mapped tables columns ##################*/

        }
        private string BuildNewInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {

            /*####################### Create StoreProcedure parameters ##################*/

            string NewStoreProcedureDef = null;
            if (create)
                NewStoreProcedureDef = "Create ";
            else
                NewStoreProcedureDef = "Alter ";

            NewStoreProcedureDef += "Procedure dbo." + storeProcedure.Name + "\n";
            string PrameterList = null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";
            System.Collections.Specialized.HybridDictionary ClassAttriutes = new System.Collections.Specialized.HybridDictionary();

            foreach (MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
            {
                ClassAttriutes.Add(CurrParameter.Name, CurrParameter);
                if (PrameterList != null)
                    PrameterList += ",\n";
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

                PrameterList += "@" + CurrParameter.Name + " " + DataBase.TypeDictionary.GetDBType(parameterType);
                if (DataBase.TypeDictionary.IsTypeVarLength(parameterType))
                {
                    object mLength = CurrParameter.GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                    if (mLength != null)
                    {
                        if (((int)mLength) == 0)
                            PrameterList += "(" + DataBase.TypeDictionary.GeDefaultLength(parameterType).ToString() + ")";
                        else
                            PrameterList += "(" + mLength.ToString() + ")";
                    }
                }
                if (CurrParameter.Direction == MetaDataRepository.Parameter.DirectionType.Out)
                    PrameterList += " output";

            }
            NewStoreProcedureDef += PrameterList;



            NewStoreProcedureDef += "\nAS\n";

            NewStoreProcedureDef += "DECLARE @TypeID int\n";
            NewStoreProcedureDef += "set @TypeID=" + (storeProcedure.ActsOnStorageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString() + "\n";




            /*####################### Insert main table columns ##################*/
            string InsertClause = "INSERT INTO " + storeProcedure.ActsOnStorageCell.MainTable.Name + "(";//ObjCellID, IntObjID, TypeID";
            string ValueClause = "VALUES(";//ObjCellID, @TableID, @TypeID";
            int count = 0;
            foreach (RDBMSMetaDataRepository.Column column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
            {
                if (count != 0)
                {
                    InsertClause += ",";
                    ValueClause += ",";
                }
                count++;
                InsertClause += column.Name;
                ValueClause += "@" + column.Name;
            }
            foreach (string columnName in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumnsName())
            {
                if (count != 0)
                {
                    InsertClause += ",";
                    ValueClause += ",";
                }
                count++;
                InsertClause += columnName;
                ValueClause += "@" + columnName;
            }
            if (storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn != null)
            {
                InsertClause += "," + storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
                ValueClause += ",@" + storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
            }
            foreach (RDBMSMetaDataRepository.Column CurrColumn in storeProcedure.ActsOnStorageCell.MainTable.ContainedColumns)
            {
                if (CurrColumn.MappedAttribute != null)
                {
                    //string parameterName=CurrColumn.MappedAttribute.CaseInsensitiveName;
                    string parameterName = CurrColumn.Name;

                    //					if(CurrColumn.Name.Trim().ToLower()!=CurrColumn.MappedAttribute.CaseInsensitiveName.Trim().ToLower())
                    //						parameterName=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
                    //					else
                    //						parameterName=CurrColumn.Name;

                    if (ClassAttriutes.Contains(parameterName))
                    {
                        InsertClause += "," + CurrColumn.Name;
                        ValueClause += ",@" + parameterName;
                    }
                }
            }
            InsertClause += ")\n";
            ValueClause += ")\n";
            NewStoreProcedureDef += InsertClause + ValueClause;
            /*####################### Insert main table columns ##################*/

            InsertClause = null;
            ValueClause = null;

            /*####################### Insert mapped tables columns ##################*/
            foreach (RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
            {
                if (CurrTable == storeProcedure.ActsOnStorageCell.MainTable)
                    continue;
                InsertClause += "INSERT INTO " + CurrTable.Name + "(";//ObjCellID, IntObjID, TypeID";
                ValueClause += "VALUES(";//@ObjCellID, @TableID, @TypeID";

                foreach (RDBMSMetaDataRepository.Column column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
                {
                    if (count != 0)
                    {
                        InsertClause += ",";
                        ValueClause += ",";
                    }
                    count++;
                    InsertClause += column.Name;
                    ValueClause += "@" + column.Name;
                }
                foreach (string columnName in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumnsName())
                {
                    if (count != 0)
                    {
                        InsertClause += ",";
                        ValueClause += ",";
                    }
                    count++;
                    InsertClause += columnName;
                    ValueClause += "@" + columnName;
                }
                foreach (RDBMSMetaDataRepository.Column CurrColumn in CurrTable.ContainedColumns)
                {
                    if (CurrColumn.MappedAttribute != null)
                    {
                        InsertClause += "," + CurrColumn.Name;
                        //ValueClause+=",@"+CurrColumn.MappedAttribute.CaseInsensitiveName;
                        ValueClause += ",@" + CurrColumn.Name;
                        //						if(CurrColumn.Name.Trim().ToLower()==CurrColumn.MappedAttribute.Name.Trim().ToLower())
                        //							ValueClause+=",@"+CurrColumn.Name;
                        //						else
                        //							ValueClause+=",@"+CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
                    }
                }
                InsertClause += ")\n";
                ValueClause += ")\n";
                NewStoreProcedureDef += InsertClause + ValueClause;
            }
            /*####################### Insert mapped tables columns ##################*/
            return NewStoreProcedureDef;
        }

        #endregion

        public string ConvertToSQLString(object value)
        {
            return DataBase.TypeDictionary.ConvertToSQLString(value);
        }

        public int GeDefaultLength(string typeName)
        {
            return DataBase.TypeDictionary.GeDefaultLength(typeName);

        }

        public string GetDBType(string typeName, bool fixedLength)
        {
            return DataBase.TypeDictionary.GetDBType(typeName, fixedLength);
        }

        public bool IsTypeVarLength(string typeName)
        {
            return DataBase.TypeDictionary.IsTypeVarLength(typeName);
        }

        public string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn, int startIndex, int endIndex, int change)
        {


            string IndexCriterion = "";


            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                if (string.IsNullOrEmpty(IndexCriterion))
                    IndexCriterion = " WHERE ";
                else
                    IndexCriterion += " AND ";
                IndexCriterion += column.Name + " = " + DataBase.TypeDictionary.ConvertToSQLString(((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType));
            }

            string indexUpdate = null;


            string roleUpdateRelationIndex = @"UPDATE [KeepColumnTable]
                                            SET     [IndexerColumn] = [IndexerColumn] - 1
                                            WHERE  ([IndexerColumn] >= @roleIndex and [ListOwnerCriterion])";
            if (change > 0)
            {
                indexUpdate = @"UPDATE " + KeepColumnTable.Name + @"
                                SET " + IndexerColumn.Name + " = " + IndexerColumn.Name + @" + " + change.ToString()
                                     + IndexCriterion + " AND " + IndexerColumn.Name + " >= " + startIndex.ToString()
                                     + " AND " + IndexerColumn.Name + " <= " + endIndex.ToString() + " \n";
            }
            else
            {
                indexUpdate = @"UPDATE " + KeepColumnTable.Name + @"
                                SET " + IndexerColumn.Name + " = " + IndexerColumn.Name + @" " + change.ToString()
                + IndexCriterion + " AND " + IndexerColumn.Name + " >= " + startIndex.ToString()
                + " AND " + IndexerColumn.Name + " <= " + endIndex.ToString() + " \n";
            }

            return indexUpdate;

        }


        #region IRDBMSSQLScriptGenarator Members

        public string AliasDefSqlScript
        {
            get
            {
                return " AS ";
            }
        }

        public string GetColumnsDefaultValuesSetScript(string tableName, List<ColumnDefaultValueData> columnsDefaultValues)
        {
            string defaultValuesSetScript = null;
            foreach (ColumnDefaultValueData columnDefaultValueData in columnsDefaultValues)
            {
                if (defaultValuesSetScript == null)
                    defaultValuesSetScript = @"UPDATE [" + tableName + @"]  SET ";
                else
                    defaultValuesSetScript += ",";
                defaultValuesSetScript += columnDefaultValueData.ColumnName + " = " + DataBase.TypeDictionary.ConvertToSQLString(columnDefaultValueData.DefaultValue);
            }
            return defaultValuesSetScript;
        }

        public bool SupportBatchSQL
        {
            get { return false; }
        }

        public string GetSQLScriptForName(string name)
        {
            return @"[" + name + @"]";
        }

        public System.Collections.Generic.Dictionary<string, string> GetViewsNamesAndDefinition()
        {
            Dictionary<string, string> viewsNamesAndDefinition = new Dictionary<string, string>();
            return viewsNamesAndDefinition;
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
                    Connection.Open();
                System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "SELECT TABLE_NAME as ViewName,VIEW_DEFINITION as ViewDefinition FROM  INFORMATION_SCHEMA.Views";// "exec sp_tables";
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string viewName = dataReader["ViewName"].ToString();
                    viewsNamesAndDefinition.Add(viewName, dataReader["ViewDefinition"].ToString());
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return viewsNamesAndDefinition;
        }




        public List<string> GetRenameScript(Key key, string newName)
        {
            return new List<string>() { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'\n", key.Name, newName) };
        }

        List<string> IRDBMSSQLScriptGenarator.GetDropKeyScript(Key key)
        {
            return new List<string>() { "ALTER TABLE " + key.OwnerTable.Name + " DROP CONSTRAINT " + key.Name };
        }

        public List<string> GetRenameScript(string tableName, string newTableName)
        {
            return new List<string>() { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", tableName, newTableName) };
        }

        public List<string> GetTableDDLScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType)
        {
            string script = null;

            string columnsScript = null;
            foreach (Column addedColumn in newColumns)
            {
                if (columnsScript != null)
                    columnsScript += ",\n";
                columnsScript += addedColumn.GetScript();

            }
            if (string.IsNullOrEmpty(columnsScript))
                return new List<string>();


            if (newTable)
            {
                if (tableType == TableType.VaraibleTable)
                    script = "CREATE TABLE " + tableName + " (\n" + columnsScript + ")";
                else if (tableType == TableType.TemporaryTable)
                    script = "CREATE TABLE " + tableName + " (\n" + columnsScript + ")";
                else
                    script = "CREATE TABLE " + tableName + " (\n" + columnsScript + ")";
            }
            else
            {
                if (tableType == TableType.VaraibleTable)
                    script = "ALTER TABLE " + tableName + " ADD \n" + columnsScript;
                else if (tableType == TableType.TemporaryTable)
                    script = "ALTER TABLE " + tableName + " ADD \n" + columnsScript;
                else
                    script = "ALTER TABLE " + tableName + " ADD \n" + columnsScript;
            }

            return new List<string>() { script };
        }


        public List<string> GetDropTableScript(string tableName, TableType tableType)
        {
            if (tableType == TableType.VaraibleTable)
                return new List<string>() { "DROP TABLE " + tableName };
            else if (tableType == TableType.TemporaryTable)
                return new List<string>() { "DROP TABLE " + tableName };
            else
                return new List<string>() { "DROP TABLE " + tableName };
        }

        public List<string> GetViewDDLScript(RDBMSMetaDataRepository.View view, bool newView)
        {
            string AbstractClassView = "";
            return new List<string>() { null };
        }

  

        List<string> IRDBMSSQLScriptGenarator.BuildManyManyRelationshipReferenceCountCommand(OOAdvantech.RDBMSMetaDataRepository.Table mainTable, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, OOAdvantech.RDBMSMetaDataRepository.Table referenceTable)
        {
            throw new NotImplementedException();
        }

        List<string> IRDBMSSQLScriptGenarator.BuildManyRelationshipReferenceCountCommand(OOAdvantech.RDBMSMetaDataRepository.Table mainTable, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> referenceColumns, OOAdvantech.MetaDataRepository.AssociationEnd associationEnd)
        {
            throw new NotImplementedException();
        }

        List<string> IRDBMSSQLScriptGenarator.BuildOneRelationshipReferenceCountCommand(OOAdvantech.RDBMSMetaDataRepository.StorageCell storageCell, OOAdvantech.RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd)
        {
            throw new NotImplementedException();
        }

        public PrimaryKeyData RetrieveDatabasePrimaryKeys(Table table)
        {
            System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            System.Data.Common.DbCommand Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
            Command.CommandText = string.Format(@"SELECT TABLE_SCHEMA AS TableSchema,TABLE_NAME AS PK_TABLE_NAME,INDEX_NAME PK_NAME,COLUMN_NAME,ORDINAL_POSITION COLUMN_ORDER
                                                 FROM INFORMATION_SCHEMA.INDEXES
                                                 WHERE PRIMARY_KEY=1 and  TABLE_NAME = '{0}' ORDER BY PK_NAME,COLUMN_ORDER", table.Name);
            System.Data.Common.DbDataReader dataReader = Command.ExecuteReader();
            // Load table Primary Keys
            PrimaryKeyData pkeyData = null;
            foreach (System.Data.Common.DbDataRecord currRecord in dataReader)
            {
                if (pkeyData == null)
                    pkeyData = new PrimaryKeyData((string)currRecord["PK_NAME"], (string)currRecord["PK_TABLE_NAME"]);
                pkeyData.ColumnsNames.Add((string)currRecord["COLUMN_NAME"]);
            }
            dataReader.Close();
            return pkeyData;
        }

        public bool GetValidateStoreprocedureName(StoreProcedure storeProcedure)
        {
            throw new NotImplementedException();
        }


        string GetValidateColumnName(RDBMSMetaDataRepository.Column column, RDBMSMetaDataRepository.View view)
        {
            //int maxColumnLength = 28;
            int maxColumnLength = 128;
            if (column.Name.Length > maxColumnLength)
            {
                int count = 0;
                bool tryAgain = false;
                string newColumnName = column.Name.Substring(0, maxColumnLength);
                if (column.RealColumn != null)
                    newColumnName = column.RealColumn.DataBaseColumnName;

                do
                {
                    if (count != 0)
                        newColumnName = newColumnName.Substring(0, maxColumnLength - count.ToString().Length) + count.ToString();

                    foreach (RDBMSMetaDataRepository.Column tableColumn in view.ViewColumns)
                    {
                        if (tableColumn == column)
                            continue;
                        if (newColumnName == tableColumn.Name)
                        {
                            count++;
                            tryAgain = true;
                            break;
                        }
                    }
                } while (tryAgain);
                return newColumnName;
                //return newColumnName.ToUpper();

            }
            return column.Name;
            //return column.Name.ToUpper();
        }
        public bool GetValidateColumnName(Column column)
        {
            //int maxColumnLength = 28;
            int maxColumnLength = 128;
            if (column.Name.Length > maxColumnLength)
            {
                int count = 0;
                bool tryAgain = false;
                string newColumnName = column.Name.Substring(0, maxColumnLength);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newColumnName = newColumnName.Substring(0, maxColumnLength - count.ToString().Length) + count.ToString();

                    foreach (Column tableColumn in (column.Namespace as Table).Columns)
                    {
                        if (tableColumn == column)
                            continue;
                        if (newColumnName == tableColumn.Name)
                        {
                            count++;
                            tryAgain = true;
                            break;
                        }
                    }
                } while (tryAgain);
                //column.Name = newColumnName.ToUpper(); ;
                column.Name = newColumnName;
                return true;
            }
            return false;

        }
        public bool GetValidateTableName(Table table)
        {
            //int maxColumnLength = 28;
            int maxColumnLength = 128;
            if (table.Name.Length > maxColumnLength)
            {
                int count = 0;
                bool tryAgain = false;
                string newTableName = table.Name.Substring(0, maxColumnLength);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newTableName = newTableName.Substring(0, maxColumnLength - count.ToString().Length) + count.ToString();
                    foreach (Table dataBaseTable in (table.Namespace as DataBase).Tables)
                    {
                        if (dataBaseTable == table)
                            continue;

                        if (newTableName.ToLower() == dataBaseTable.Name.ToLower())
                        {
                            count++;
                            tryAgain = true;
                            break;
                        }
                    }
                }
                while (tryAgain);
                table.Name = newTableName;
                //table.Name = newTableName.ToUpper();
                return true;
            }
            return false;
        }
        public bool GetValidateKeyName(Key key)
        {
            //int maxColumnLength = 28;
            int maxColumnLength = 128;
            if (key.Name.Length > maxColumnLength)
            {
                int count = 0;
                bool tryAgain = false;
                string newKeyName = key.Name.Substring(0, maxColumnLength);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newKeyName = newKeyName.Substring(0, maxColumnLength - count.ToString().Length) + count.ToString();
                    foreach (Key tableColumn in key.OwnerTable.Keys)
                    {
                        if (tableColumn == key)
                            continue;
                        if (newKeyName.ToLower() == tableColumn.Name.ToLower())
                        {
                            count++;
                            tryAgain = true;
                            break;
                        }
                    }
                }
                while (tryAgain);
                key.Name = newKeyName;
                //key.Name = newKeyName.ToUpper();
                return true;
            }
            return false;

        }
        public bool GetValidateViewName(View view)
        {
            //int maxColumnLength = 28;
            int maxColumnLength = 128;
            if (view.Name.Length > maxColumnLength)
            {
                int count = 0;
                bool tryAgain = false;
                string newViewName = view.Name.Substring(0, maxColumnLength);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newViewName = newViewName.Substring(0, maxColumnLength - count.ToString().Length) + count.ToString();
                    foreach (View dataBaseTable in (view.Namespace as DataBase).Views)
                    {
                        if (dataBaseTable == view)
                            continue;

                        if (newViewName.ToLower() == dataBaseTable.Name.ToLower())
                        {
                            count++;
                            tryAgain = true;
                            break;
                        }
                    }
                }
                while (tryAgain);
                //view.Name = newViewName.ToUpper();
                view.Name = newViewName;
                return true;
            }
            return false;
        }


        public string GeValidRDBMSName(string name, System.Collections.Generic.List<string> excludedValidNames)
        {
            if (name.Length > 128)
            {
                int count = 1;
                string validAlias = name.Substring(0, 128 - count.ToString().Length);
                validAlias += count.ToString();
                while (excludedValidNames.Contains(validAlias))
                {
                    count++;
                    validAlias = name.Substring(0, 128 - count.ToString().Length) + count.ToString();
                }
                return validAlias;
            }
            else
                return name;
        }



        public string GetTemporaryTableNameScript(string tableName)
        {
            return tableName; 
        }

        public string GetRowRemoveCaseScript(string filterString, string alias)
        {
            return string.Format(@", CASE WHEN {0} THEN cast(0 as bit) ELSE cast(1 as bit) END AS {1}", filterString, alias);
        }

        #endregion
    }
}
