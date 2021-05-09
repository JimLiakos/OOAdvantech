using System;
using System.Collections.Generic;
using System.Text;

using OOAdvantech.RDBMSDataObjects;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
//using OOAdvantech.RDBMSPersistenceRunTime;
namespace OOAdvantech.OraclePersistenceRunTime
{

    /// <MetaDataID>{86ec8524-974d-4338-9008-9706dae25ee3}</MetaDataID>
    public class OracleRDBMSSchema : OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator
    {

        public bool AlwaysDeclareColumnAlias
        {
            get
            {
                return false;
            }
        }

        public bool CriterionResolvedFromNativeSystem(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            if (criterion.OverridenComparisonOperator != null)
                return false;
            return true;
        }

        public string GhostTable
        {
            get
            {
                return "dual";
            }
        }
        /// <summary>
        /// There are RDBMS which supports addendum constraint key on create table
        /// and other RDBMS which not support
        /// This property is true when RDBMS supports addendum
        /// </summary>
        public bool SupportAddRemoveKeys { get { return true; } }

        public string GetTemporaryTableNameScript(string tableName)
        {
            return tableName;
        }
        Dictionary<string, PrimaryKeyData> TablesPrimaryKeys = new Dictionary<string, PrimaryKeyData>();
        Dictionary<string, Dictionary<string, ForeignKeyData>> TablesForeignKeys = new Dictionary<string, Dictionary<string, ForeignKeyData>>();
        Dictionary<string, List<ColumnData>> TablesColumns = new Dictionary<string, List<ColumnData>>();
        Dictionary<string, string> TablesSequences = new Dictionary<string, string>();

        public string GetDatePartSqlScript(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.Name == "Year")
            {
                return "Extract(year from " + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Month")
            {
                return "Extract(month from " + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Date")
            {
                return string.Format("TRUNC({0})", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
            }
            if (dataNode.Name == "Day")
            {
                return "Extract(Day from " + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "DayOfWeek")
            {
                return string.Format("to_char({0},'D')", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
                //return "DATEPART(weekday," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "DayOfYear")
            {
              return  string.Format("to_char({0},'DDD')", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
                //return "DATEPART(dayofyear," + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name) + ")";
            }
            if (dataNode.Name == "Hour")
            {
                return string.Format("Extract(Hour from  cast({0}  as timestamp))", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
            }
            if (dataNode.Name == "Minute")
            {
                return string.Format("Extract(Minute from  cast({0}  as timestamp))", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
            }
            if (dataNode.Name == "Second")
            {
                return string.Format("Extract(Second from  cast({0}  as timestamp))", GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.AssignedMetaObject.Name));
            }



            return null;

        }


        public bool SupportBatchSQL
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{6f9c0b11-c340-44d5-9e14-fc2b928b8c47}</MetaDataID>
        string GetValidateColumnName(RDBMSMetaDataRepository.Column column, RDBMSMetaDataRepository.View view)
        {
            if (column.Name.Length > 30)
            {
                int count = 0;
                bool tryAgain = false;
                string newColumnName = column.Name.Substring(0, 30);
                if (column.RealColumn != null)
                    newColumnName = column.RealColumn.DataBaseColumnName;

                do
                {
                    if (count != 0)
                        newColumnName = newColumnName.Substring(0, 30 - count.ToString().Length) + count.ToString();

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
                return newColumnName.ToUpper();

            }
            return column.Name.ToUpper();
        }
        /// <MetaDataID>{053bcde9-33ad-4356-b21a-daf05113c032}</MetaDataID>
        public bool GetValidateColumnName(Column column)
        {
            if (column.Name.Length > 30)
            {
                int count = 0;
                bool tryAgain = false;
                string newColumnName = column.Name.Substring(0, 30);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newColumnName = newColumnName.Substring(0, 30 - count.ToString().Length) + count.ToString();

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
                column.Name = newColumnName.ToUpper();
                return true;
            }
            if (column.Name != column.Name.ToUpper())
            {
                column.Name = column.Name.ToUpper();
                return true;
            }
            return false;

        }
        /// <MetaDataID>{a486cec1-222f-4c46-adf4-ca9caf331c82}</MetaDataID>
        public bool GetValidateTableName(Table table)
        {
            if (table.Name.Length > 30)
            {
                int count = 0;
                bool tryAgain = false;
                string newTableName = table.Name.Substring(0, 30);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newTableName = newTableName.Substring(0, 30 - count.ToString().Length) + count.ToString();
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
                table.Name = newTableName.ToUpper();
                return true;
            }
            if (table.Name != table.Name.ToUpper())
            {
                table.Name = table.Name.ToUpper();
                return true;
            }


            return false;

            //table.Name = table.Name.ToUpper();
            //return true;
        }
        /// <MetaDataID>{48e74e5f-7db1-4f9b-91e8-8838c85700a9}</MetaDataID>
        public bool GetValidateKeyName(Key key)
        {
            if (key.Name.Length > 30)
            {
                int count = 0;
                bool tryAgain = false;
                string newKeyName = key.Name.Substring(0, 30);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newKeyName = newKeyName.Substring(0, 30 - count.ToString().Length) + count.ToString();

                    foreach (Table table in (key.OwnerTable.Namespace as DataBase).Tables)
                    {
                        foreach (Key tableKey in table.Keys)
                        {
                            if (tableKey == key)
                                continue;
                            if (newKeyName.ToLower() == tableKey.Name.ToLower())
                            {
                                count++;
                                tryAgain = true;
                                break;
                            }
                        }
                    }
                }
                while (tryAgain);
                key.Name = newKeyName.ToUpper();
                return true;
            }
            return false;
        }
        /// <MetaDataID>{b688d0ee-c42b-4723-9e93-2980a6255772}</MetaDataID>
        public bool GetValidateViewName(View view)
        {
            if (view.Name.Length > 30)
            {
                int count = 0;
                bool tryAgain = false;
                string newViewName = view.Name.Substring(0, 30);
                do
                {
                    tryAgain = false;
                    if (count != 0)
                        newViewName = newViewName.Substring(0, 30 - count.ToString().Length) + count.ToString();
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
                view.Name = newViewName.ToUpper();
                return true;
            }
            return false;
        }
        /// <MetaDataID>{77265cb9-df63-43bf-8bc4-0cc358b77f7e}</MetaDataID>
        public bool GetValidateStoreprocedureName(StoreProcedure storeProcedure)
        {
            return false;
        }
        /// <MetaDataID>{0de5e3f0-b6be-43fe-81ab-1ba47c13d573}</MetaDataID>
        DataBase DataBase;

        /// <MetaDataID>{db5b1900-f3e6-49ee-91a6-d716cd2dc782}</MetaDataID>
        OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                return DataBase.Connection;
            }
        }

        /// <MetaDataID>{f85e0896-c671-40c6-838c-5f4dc87cc5ff}</MetaDataID>
        public OracleRDBMSSchema(DataBase dataBase)
        {
            DataBase = dataBase;
        }
        #region SQLStatamentBuilder



        /// <MetaDataID>{6dd545ec-05c7-405f-8777-5fe240599bbe}</MetaDataID>
        public List<string> CreateManyToManyTransferDataSQLStatement(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, RDBMSMetaDataRepository.Table relationTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)//, out Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, out RDBMSMetaDataRepository.Table referenceColumnsTable)
        {
            string transferDataSQLStatement = null;
            string insertClause = null;
            string selectClause = null;
            string fromClause = null;

            #region Build INSERT and SELECT clause of transfer data SQL statement.
            RDBMSMetaDataRepository.Column roleAIndexerColumn = (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCellsLink.RoleAStorageCell, "");
            RDBMSMetaDataRepository.Column roleBIndexerColumn = (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCellsLink.RoleBStorageCell, "");
            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                if (insertClause == null)
                    insertClause = @"INSERT INTO """ + relationTable.DataBaseTableName + @""" (";
                else
                    insertClause += ",";

                insertClause += @"""" + relationTableColumn.DataBaseColumnName + @"""";
                if (selectClause == null)
                    selectClause = "\nSELECT ";
                else
                    selectClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                    {
                        foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                        {
                            if (viewColumn.RealColumn == identityColumn)
                            {
                                selectClause += @"""" + (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName + @""".""" + viewColumn.DataBaseColumnName + @"""";
                                break;
                            }
                        }
                        break;
                    }
                }

            }
            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                insertClause += @",""" + relationTableColumn.DataBaseColumnName + @"""";

                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                    {
                        foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                        {
                            if (viewColumn.RealColumn == identityColumn)
                            {
                                selectClause += @",""" + (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName + @""".""" + viewColumn.DataBaseColumnName + @"""";
                                break;
                            }
                        }
                        break;
                    }
                }

            }

            insertClause += ")";
            #endregion


            #region Build FROM clause of transfer data SQL statement.
            Collections.Generic.Set<RDBMSMetaDataRepository.Column> referenceColumns = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>();
            RDBMSMetaDataRepository.View referenceColumnsTable = null;
            RDBMSMetaDataRepository.StorageCell tableWithPrimaryKeyColumns = null;
            if (roleAColumns.Count == 0)
            {
                tableWithPrimaryKeyColumns = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell);
                referenceColumnsTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView;
                foreach (RDBMSMetaDataRepository.Column roleBColumn in roleBColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                    {
                        if (viewColumn.RealColumn == roleBColumn)
                        {
                            referenceColumns.Add(viewColumn);
                            break;
                        }
                    }
                }
            }
            else
            {

                tableWithPrimaryKeyColumns = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell);
                referenceColumnsTable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView;
                foreach (RDBMSMetaDataRepository.Column roleAColumn in roleAColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                    {
                        if (viewColumn.RealColumn == roleAColumn)
                        {
                            referenceColumns.Add(viewColumn);
                            break;
                        }
                    }
                }
            }


            foreach (RDBMSMetaDataRepository.Column referenceColumn in referenceColumns)
            {
                //referenceColumnsTable = referenceColumn.Namespace as RDBMSMetaDataRepository.View;
                if (fromClause == null)
                    fromClause = "\nFROM \"" + tableWithPrimaryKeyColumns.ClassView.DataBaseViewName + @""" INNER JOIN """ + referenceColumnsTable.DataBaseViewName + @""" ON ";
                else
                    fromClause += ",";

                fromClause += @"""" + (referenceColumnsTable).DataBaseViewName + @""".""" + referenceColumn.DataBaseColumnName + @""" = ";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in tableWithPrimaryKeyColumns.MainTable.ObjectIDColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in tableWithPrimaryKeyColumns.ClassView.ViewColumns)
                    {
                        if (identityColumn.ColumnType == (referenceColumn.RealColumn as RDBMSMetaDataRepository.IdentityColumn).ColumnType && viewColumn.RealColumn != null && viewColumn.RealColumn == identityColumn)
                        {
                            fromClause += @"""" + tableWithPrimaryKeyColumns.ClassView.DataBaseViewName + @""".""" + viewColumn.DataBaseColumnName + @""" ";
                            break;
                        }
                    }
                }
            }
            #endregion

            transferDataSQLStatement = insertClause + selectClause + fromClause;
            return new List<string>() { transferDataSQLStatement };
        }

        /// <MetaDataID>{08b57f53-00e1-4c92-a00c-594004a37975}</MetaDataID>
        public List<string> CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)
        {

            #region Build data transfer SQL statement
            string transferQuery = null;
            string fromClause = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            {
                if (transferQuery == null)
                {
                    fromClause += " = (SELECT ";
                    transferQuery += "UPDATE " + GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + " \r\nset (";
                }
                else
                {
                    fromClause += ",";
                    transferQuery += ",";
                }
                foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
                {
                    if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
                        transferQuery += GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + "." + GetSQLScriptForName(roleBColumn.DataBaseColumnName);

                    fromClause += GetSQLScriptForName(roleAObjectIDColumn.Namespace.Name) + "." + GetSQLScriptForName(roleAObjectIDColumn.DataBaseColumnName);
                }
            }
            transferQuery += ")";
            fromClause += "\r\nFROM " + GetSQLScriptForName(roleATable.Name);
            string whereClause = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                if (whereClause == null)
                    whereClause += " WHERE " ;
                else
                    whereClause += " AND ";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
                {
                    if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
                        whereClause += GetSQLScriptForName((roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + "." + GetSQLScriptForName(roleAColumn.DataBaseColumnName) + " = " +
                            GetSQLScriptForName(roleBObjectIDColumn.Namespace.Name) + "." + GetSQLScriptForName(roleBObjectIDColumn.DataBaseColumnName);
                }
            }
            transferQuery += fromClause + whereClause+") ";
            #endregion
            return new List<string>() { transferQuery };



            //#region Build data transfer SQL statement
            //string transferQuery = null;
            //string fromClause = null;
            //foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            //{
            //    if (transferQuery == null)
            //        transferQuery += @"UPDATE   """ + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""" \nset ";
            //    else
            //        transferQuery += ",";
            //    foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
            //    {
            //        if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
            //            transferQuery += @"""" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"""" + @".""" + roleBColumn.DataBaseColumnName + @"""=""" +
            //                roleAObjectIDColumn.Namespace.Name + @""".""" + roleAObjectIDColumn.DataBaseColumnName + @"""";
            //    }
            //}

            //foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            //{
            //    if (fromClause == null)
            //        fromClause += "\nFROM \"" + roleATable.Name + @"""  INNER JOIN   """ + roleBTable.Name + "\" ON ";
            //    else
            //        fromClause += ",";
            //    foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
            //    {
            //        if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
            //            fromClause += "\"" + (roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "\"" + ".\"" + roleAColumn.DataBaseColumnName + "\"=\"" +
            //                roleBObjectIDColumn.Namespace.Name + "\".\"" + roleBObjectIDColumn.DataBaseColumnName + "\"";
            //    }
            //}
            //transferQuery += fromClause;
            //#endregion


            return new List<string>() { transferQuery };
        }

        /// <MetaDataID>{c4f9d1c9-7789-41fe-a6f3-d90c003f247a}</MetaDataID>
        public List<string> BuildManyManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, RDBMSMetaDataRepository.Table referenceTable)
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
            string referenceTableSelectList = null;
            string objectIDsInnerJoinCondition = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn column in mainTable.ObjectIDColumns)
            {
                if (referenceCountTableSelectList != null)
                    referenceCountTableSelectList += @""",""";
                else
                    referenceCountTableSelectList = @"""";
                referenceCountTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName;

                if (referenceCountTableFirstInnerJoinCondition != null)
                    referenceCountTableFirstInnerJoinCondition += @""",""";
                else
                    referenceCountTableFirstInnerJoinCondition = @"""";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in mainTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableFirstInnerJoinCondition += @"REFERENCECOUNTTABLE"".""" + referenceColumn.DataBaseColumnName +
                            @""" = """ + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName;
                }
                if (objectIDsInnerJoinCondition != null)
                    objectIDsInnerJoinCondition += @""",""";
                else
                    objectIDsInnerJoinCondition = @"""";

                objectIDsInnerJoinCondition += @"""REFERENCECOUNTTABLE"".""" + column.DataBaseColumnName +
                    @""" = """ + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName;

            }
            referenceCountTableSelectList += @"""";
            referenceCountTableFirstInnerJoinCondition += @"""";
            foreach (RDBMSMetaDataRepository.IdentityColumn column in referenceTable.ObjectIDColumns)
            {
                if (referenceCountTableSecondInnerJoinCondition != null)
                    referenceCountTableSecondInnerJoinCondition += @""",""";
                else
                    referenceCountTableSecondInnerJoinCondition = @"""";

                if (referenceTableSelectList != null)
                    referenceTableSelectList += @""",""";
                else
                    referenceTableSelectList = @"""";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                    {
                        referenceCountTableSecondInnerJoinCondition += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName +
                            @""" = """ + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + referenceColumn.DataBaseColumnName;
                        referenceTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName;
                    }
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceTableAssociationColumns)
            {
                referenceTableSelectList += @""",""" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + referenceColumn.DataBaseColumnName;
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in mainTableAssociationColumns)
            {
                referenceTableSelectList += @""",""" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + referenceColumn.DataBaseColumnName;
            }
            referenceTableSelectList += @"""";

            //            @"UPDATE ""T_STOREPLACE""
            //            SET ""T_STOREPLACE"."REFERENCECOUNT"" = "T_STOREPLACE"."REFERENCECOUNT"+(SELECT COUNT(*) 
            //            FROM (SELECT "T_PRODUCT"."OBJECTID","T__PRODUCTSINSTORE"."PRODUCTSINSTORE_OBJECTIDB","T__PRODUCTSINSTORE"."PRODUCTSINSTORE_OBJECTIDA"
            //            FROM "T_PRODUCT" INNER JOIN "T__PRODUCTSINSTORE" ON "T_PRODUCT"."OBJECTID" = "T__PRODUCTSINSTORE"."PRODUCTSINSTORE_OBJECTIDB")  "REFERENCECOUNTTABLE"
            //            WHERE "T_STOREPLACE"."OBJECTID" = "REFERENCECOUNTTABLE"."PRODUCTSINSTORE_OBJECTIDA") 

            referenceCountTableSecondInnerJoinCondition += @"""";
            string updateQuery = @"UPDATE """ + mainTable.DataBaseTableName + @"""
                        SET """ + mainTable.DataBaseTableName + @""".""REFERENCECOUNT""=""" + mainTable.DataBaseTableName + @""".""REFERENCECOUNT"" +(SELECT COUNT(*)  
                        FROM (SELECT " + referenceTableSelectList + @"
		                FROM """ + referenceTable.DataBaseTableName + @""" INNER JOIN """ + assoctiaionTable.DataBaseTableName + @""" ON " + referenceCountTableSecondInnerJoinCondition + @"
		                ) ""REFERENCECOUNTTABLE"" WHERE " + referenceCountTableFirstInnerJoinCondition + @")";



            //string updateQuery = "update " + mainTable.DataBaseTableName + "\r\n" +
            //    "set " + mainTable.DataBaseTableName + ".ReferenceCount=" + mainTable.DataBaseTableName + ".ReferenceCount+ReferenceCountTable.ReferenceCount\r\n" +
            //    "FROM  " + mainTable.DataBaseTableName + " INNER JOIN\r\n" +
            //    "(SELECT " + referenceCountTableSelectList + ",COUNT(*) AS ReferenceCount\r\n" +
            //    "FROM " + assoctiaionTable.DataBaseTableName + " INNER JOIN \r\n" +
            //    mainTable.DataBaseTableName + " ON " + referenceCountTableFirstInnerJoinCondition + "\r\n" +
            //    "INNER JOIN " + referenceTable.DataBaseTableName + " as " + referenceTable.DataBaseTableName + "_R  ON " + referenceCountTableSecondInnerJoinCondition + "\r\n" +
            //    "WHERE " + mainTable.DataBaseTableName + ".TypeID=" + ((mainTable.TableCreator as RDBMSMetaDataRepository.StorageCell).Type as RDBMSMetaDataRepository.Class).TypeID.ToString() + "\r\n" +
            //    "GROUP BY " + referenceCountTableSelectList + ") ReferenceCountTable ON " + objectIDsInnerJoinCondition;
            return new List<string>() { updateQuery };


        }

        /// <MetaDataID>{80c76db4-c271-4ad8-af1e-be6e932aa5c6}</MetaDataID>
        public List<string> BuildManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, MetaDataRepository.AssociationEnd associationEnd)
        {


            RDBMSMetaDataRepository.Table referenceColumnTable = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in referenceColumns)
            {
                referenceColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }
            string referenceCountTableInnerJoinCondition = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn column in mainTable.ObjectIDColumns)
            {
                if (referenceCountTableInnerJoinCondition != null)
                    referenceCountTableInnerJoinCondition += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableInnerJoinCondition += @"""" + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + column.DataBaseColumnName +
                            @""" = """ + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @""".""" + referenceColumn.DataBaseColumnName + @"""";
                }
            }

            string updateQuery = @"UPDATE """ + mainTable.DataBaseTableName + @"""
                                SET ""REFERENCECOUNT"" = ""REFERENCECOUNT""+ (SELECT     COUNT(*)
                                FROM """ + referenceColumnTable.DataBaseTableName + @"""
                                WHERE (" + referenceCountTableInnerJoinCondition + @"))";

            return new List<string>() { updateQuery };
        }

        /// <MetaDataID>{aaef5a68-dbdf-4968-8865-98ba193ac174}</MetaDataID>
        public List<string> BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd)
        {
            string updateQuery = @"UPDATE """ + storageCell.MainTable.DataBaseTableName + @"""
                SET ""REFERENCECOUNT""=""REFERENCECOUNT""+1 
                WHERE ";
            string whereCondition = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in storageCellAssociationEnd.GetReferenceColumnsFor(storageCell))
            {
                if (whereCondition != null)
                    whereCondition += " AND ";
                whereCondition += column.DataBaseColumnName + " IS NOT NULL ";
            }
            updateQuery += whereCondition + @" AND ""TYPEID"" = " + (storageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString();
            return new List<string>() { updateQuery };
        }

        public List<string> GetDefineKeyScript(Key key)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d97e77a0-02dc-4b81-b0b8-22ab57f322ab}</MetaDataID>
        public List<string> GetAddKeyScript(Key key)
        {
            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }
            //SET FOREIGN_KEY_CHECKS=0
            //SET FOREIGN_KEY_CHECKS=1
            if (key.IsPrimaryKey)
            {
                string Script = "ALTER TABLE \"" + key.OwnerTable.Namespace.Name + "\".\"" + key.OwnerTable.Name + "\" ADD CONSTRAINT \n" +
                    key.Name + " PRIMARY KEY ( ";
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
                Script += ColumnsScript + ")";
                List<string> commands = new List<string>();
                commands.Add(Script.ToUpper());
                return commands;
            }
            else
            {



                //ALTER TABLE ""dbo"".""testtable"" ADD CONSTRAINT ""FK_testtable_1"" FOREIGN KEY ""FK_testtable_1"" (""RefID"")
                //REFERENCES ""serma"" (""ID"")
                //c
                //ON UPDATE NO ACTION;

                string Script = "ALTER TABLE \"" + key.OwnerTable.Namespace.Name + "\".\"" + key.OwnerTable.Name + "\"  ADD CONSTRAINT \n" +
                    key.Name + " FOREIGN KEY ( ";
                string ColumnsScript = null;
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
                Script += ColumnsScript + " ) DISABLE";
                List<string> commands = new List<string>();
                commands.Add(Script.ToUpper());
                return commands;

            }
        }
        /// <MetaDataID>{12622c69-e88f-40f4-a7db-ddd6f8b62c74}</MetaDataID>
        public List<string> GetRenameScript(Key key, string newName)
        {
            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }
            if (key.IsPrimaryKey)
                return new List<string>() { "" };
            else
                return new List<string>() { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'\n", key.Name, newName) };
        }
        /// <MetaDataID>{e106d3d8-925f-41d5-b2a1-1a0d145e2cb8}</MetaDataID>
        public List<string> GetDropKeyScript(Key key)
        {
            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }
            return new List<string>() { ("ALTER TABLE " + key.OwnerTable.Name + " DROP CONSTRAINT " + key.Name).ToUpper() };
        }

        public string GetColumnsDefaultValuesSetScript(string tableName, System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues)
        {
            string defaultValuesSetScript = null;
            foreach (ColumnDefaultValueData columnDefaultValueData in columnsDefaultValues)
            {
                if (defaultValuesSetScript == null)
                    defaultValuesSetScript = @"UPDATE """ + tableName.ToUpper() + @"""  SET ";
                else
                    defaultValuesSetScript += ",";
                defaultValuesSetScript += columnDefaultValueData.ColumnName + " = " + DataBase.TypeDictionary.ConvertToSQLString(columnDefaultValueData.DefaultValue);
            }
            return defaultValuesSetScript;
        }
        public string GetRowRemoveCaseScript(string filterString, string alias)
        {
            return string.Format(@", CASE WHEN {0} THEN 'N' ELSE 'Y' END AS {1}", filterString, alias);
        }

        /// <MetaDataID>{0f1724c3-3be7-4bff-9615-8f58555e707a}</MetaDataID>
        public void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames)
        {

            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = DataBase.SchemaUpdateConnection;

                System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();
                DataBase dataBase = key.Namespace.Namespace as DataBase;

                string commandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";


                commandString = string.Format(@"select
                col.table_name , col.column_name,
                rel.table_name as REFERENCED_TABLE_NAME , rel.column_name as referenced_column_name,cc.owner, cc.constraint_name
            from
                user_tab_columns col
                join user_cons_columns con 
                  on col.table_name = con.table_name 
                 and col.column_name = con.column_name
                join user_constraints cc 
                  on con.constraint_name = cc.constraint_name
                join user_cons_columns rel 
                  on cc.r_constraint_name = rel.constraint_name 
                 and con.position = rel.position
            where
                cc.constraint_type = 'R'
                and
                lower(col.table_name)=lower('{0}')
                and
                lower(rel.table_name)=lower('{1}')
                and
                lower(cc.owner)=lower('{2}')", key.OwnerTable.Name, key.ReferencedTable.Name, key.OwnerTable.Namespace.Name);

                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
                command.CommandText = commandString;
                var DataReader = command.ExecuteReader();
                GetValidateKeyName(key);
                while (DataReader.Read())
                {
                    if (DataReader["constraint_name"].ToString().ToLower() == key.Name.ToLower())
                    {
                        string PKColumnName = (string)DataReader["referenced_column_name"];
                        string FKColumnName = (string)DataReader["column_name"];
                        columnsNames.Add(FKColumnName);
                        referedColumnsNames.Add(PKColumnName);


                        //Column column = key.OwnerTable.GetColumn(FKColumnName);
                        ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
                        ////Το σωστό είναι  Πρέπει να είναι ordered
                        ////if (column.ColumnType == null)
                        ////    column.ColumnType = Columns.Count.ToString();

                        //Columns.Add(column);

                        //column = key.ReferencedTable.GetColumn(PKColumnName);
                        ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
                        ////Το σωστό είναι  Πρέπει να είναι ordered
                        ////if (column.ColumnType == null)
                        ////    column.ColumnType = ReferedColumns.Count.ToString();
                        //ReferedColumns.Add(column);
                        int kj = 0;

                    }
                }
                DataReader.Close();
                transactionScope.Complete();
            }
            //Connection.Close();
        }
        /// <MetaDataID>{179aa8e9-1402-4018-9375-502a4605ff01}</MetaDataID>
        public void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames)
        {
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = DataBase.SchemaUpdateConnection;

                DataBase dataBase = key.Namespace.Namespace as DataBase;


                //string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
                //            string commandString = string.Format(@"SELECT     TABLE_CONSTRAINTS.CONSTRAINT_NAME, KEY_COLUMN_USAGE.COLUMN_NAME
                //                                FROM         information_schema.TABLE_CONSTRAINTS INNER JOIN
                //                                information_schema.KEY_COLUMN_USAGE ON TABLE_CONSTRAINTS.CONSTRAINT_NAME = KEY_COLUMN_USAGE.CONSTRAINT_NAME
                //                                WHERE     (TABLE_CONSTRAINTS.TABLE_NAME = '{0}') AND (TABLE_CONSTRAINTS.CONSTRAINT_TYPE = 'PRIMARY KEY') AND 
                //                                (KEY_COLUMN_USAGE.TABLE_NAME = '{0}')", key.OwnerTable.Name);

                string commandString = string.Format(@"SELECT cols.column_name, cons.constraint_name
            FROM all_constraints cons, all_cons_columns cols
            WHERE cons.constraint_type = 'P'
            AND cons.constraint_name = cols.constraint_name
            AND cons.owner = cols.owner
            AND lower(cons.constraint_name) =  lower('{0}')
            and lower(cons.owner)= lower('{1}')
            and lower(cols.table_name)= lower('{2}')", key.Name, key.OwnerTable.Namespace.Name, key.OwnerTable.Name);

                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
                command.CommandText = commandString;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    if (dataReader["CONSTRAINT_NAME"].Equals(key.Name))
                    {
                        string PKColumnName = (string)dataReader["COLUMN_NAME"];
                        columnsNames.Add(PKColumnName);
                    }
                }
                dataReader.Close();
                transactionScope.Complete();
            }
        }
        /// <MetaDataID>{ea4f5cca-c267-4b7f-8a25-6ea6c7830f15}</MetaDataID>
        public void RefreshDDLData()
        {
            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = DataBase.SchemaUpdateConnection;
                bool closeConnection = false;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                {
                    connection.Open();
                    closeConnection = true;
                }
                try
                {

                    var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);
                    var parameter = command.CreateParameter();
                    parameter.DbType = RDBMSMetaDataPersistenceRunTime.DbType.DateTime;
                    parameter.ParameterName = ":LastDDLDataRead";
                    parameter.Value = LastDDLDataRead;
                    command.Parameters.Add(parameter);

                    command.CommandText = @"SELECT COUNT(*) FROM DDL_STATS WHERE DDL_DATE>:LastDDLDataRead";
                    int count = (int)(decimal)command.ExecuteScalar();
                    if (count > 0)
                        RetrieveDataBaseDDLData();
                }
                finally
                {
                    if (closeConnection)
                        connection.Close();
                }
                transactionScope.Complete();
            }

        }
        System.DateTime LastDDLDataRead;
        /// <MetaDataID>{4781b5d5-655a-4ee3-992a-f1cc06303005}</MetaDataID>
        void RetrieveDataBaseDDLData()
        {
            TablesPrimaryKeys.Clear();
            TablesForeignKeys.Clear();
            TablesColumns.Clear();
            if (_SequenceNames == null)
                _SequenceNames = new List<string>();
            else
                _SequenceNames.Clear();

            if (_TriggerNames == null)
                _TriggerNames = new List<string>();
            else
                _TriggerNames.Clear();



            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = DataBase.SchemaUpdateConnection;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();

                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);

                command.CommandText = "SELECT SYSDATE  FROM DUAL";
                object value = command.ExecuteScalar();
                LastDDLDataRead = (DateTime)value;

                #region  Retrieves Table Columns
                int identityIncrement = 1;
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader dataReader = null;

                command.CommandText = string.Format("SELECT columns.table_name,columns.COLUMN_NAME,columns.DATA_TYPE,columns.DATA_LENGTH,columns.NULLABLE FROM user_tab_columns  columns inner join USER_TABLES tables ON columns.table_name=tables.table_name where lower(tables.tablespace_name)=lower('{0}')", DataBase.Name);
                dataReader = command.ExecuteReader();
                // Load table columns
                while (dataReader.Read())
                {
                    string dataType = (string)dataReader["DATA_TYPE"];
                    bool identityColumn = false;
                    string columnName = dataReader["COLUMN_NAME"].ToString();
                    int length = (int)(decimal)dataReader["DATA_LENGTH"];
                    if (dataType.ToLower() == "nvarchar2")
                        length = length / 2;
                    string tableName = dataReader["table_name"] as string;
                    ColumnData columnData = new ColumnData(columnName, dataType, length, dataReader["NULLABLE"].ToString().ToLower() == "y", identityColumn, identityIncrement);
                    List<ColumnData> columns = null;
                    if (!TablesColumns.TryGetValue(tableName, out columns))
                    {
                        columns = new List<ColumnData>();
                        TablesColumns[tableName] = columns;
                    }
                    columns.Add(columnData);

                }
                dataReader.Close();
                #endregion

                if (TablesColumns.Count > 0)
                {
                    #region Retrieves PrimaryKeys

                    System.Collections.Generic.List<string> primaryKeys = new System.Collections.Generic.List<string>();
                    //System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
                    if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                        connection.Open();
                    var Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
                    Command.CommandText = string.Format(@"SELECT COLUMNS.column_name COLUMN_NAME, CONSTRAINTS.constraint_name CONSTRAINT_NAME,COLUMNS.table_name TABLE_NAME
                                                            FROM all_constraints CONSTRAINTS, all_cons_columns COLUMNS
                                                            WHERE CONSTRAINTS.constraint_type = 'P'
                                                            AND CONSTRAINTS.constraint_name = COLUMNS.constraint_name
                                                            AND CONSTRAINTS.owner = COLUMNS.owner 
                                                            and lower(CONSTRAINTS.owner)= lower('{0}')", DataBase.Name);
                    dataReader = Command.ExecuteReader();
                    // Load table Primary Keys

                    while(dataReader.Read())
                    {
                        PrimaryKeyData primaryKeyData = null;
                        if (!TablesPrimaryKeys.TryGetValue((string)dataReader["CONSTRAINT_NAME"], out primaryKeyData))
                        {
                            primaryKeyData = new PrimaryKeyData((string)dataReader["CONSTRAINT_NAME"], (string)dataReader["TABLE_NAME"]);
                            TablesPrimaryKeys[primaryKeyData.TableName] = primaryKeyData;
                        }
                        primaryKeyData.ColumnsNames.Add((string)dataReader["COLUMN_NAME"]);

                    }
                    dataReader.Close();
                    #endregion

                    #region Retrieve identity columns

                    Command = connection.CreateCommand();
                    Command.CommandText = @"select dbms_xmlgen.getxml('select TABLE_NAME,TRIGGER_BODY from USER_TRIGGERS WHERE TRIGGER_TYPE=''BEFORE EACH ROW'' and TRIGGERING_EVENT=''INSERT'' AND STATUS =''ENABLED''') TRIGGERS_DATA from dual";

                    dataReader = Command.ExecuteReader();
                    while ( dataReader.Read())
                    {
                        string triggers_data = dataReader["TRIGGERS_DATA"] as string;
                        if (string.IsNullOrEmpty(triggers_data))
                            continue;
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(triggers_data);
                        foreach (System.Xml.XmlElement rowElement in doc.SelectNodes("//ROW"))
                        {
                            //string tableName = record["TABLE_NAME"] as string;
                            //string triggerBody = record["TRIGGER_BODY"] as string;
                            string tableName = rowElement.SelectSingleNode("TABLE_NAME").InnerText;
                            string triggerBody = rowElement.SelectSingleNode("TRIGGER_BODY").InnerText;

                            if (string.IsNullOrEmpty(triggerBody) || triggerBody.ToUpper().IndexOf("NEW.") == -1)
                                continue;
                            if (!string.IsNullOrEmpty(tableName) && TablesColumns.ContainsKey(tableName))
                            {
                                foreach (ColumnData columnData in TablesColumns[tableName])
                                {
                                    if (triggerBody.ToUpper().IndexOf(("NEW." + columnData.Name + " ").ToUpper()) != -1)
                                    {
                                        columnData.Identity = true;
                                        int endPos = triggerBody.IndexOf(".NEXTVAL");
                                        int startPos = triggerBody.Substring(0, endPos).LastIndexOf(" ") + 1;
                                        string sequenceName = triggerBody.Substring(startPos, endPos - startPos);
                                        TablesSequences[tableName] = sequenceName;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    dataReader.Close();
                    #endregion

                    Command = connection.CreateCommand();
                    Command.CommandText = @"select sequence_name from user_sequences".ToUpper();
                    dataReader = Command.ExecuteReader();
                    while(dataReader.Read())
                        _SequenceNames.Add(dataReader["sequence_name".ToUpper()] as string);
                    dataReader.Close();

                    Command = connection.CreateCommand();
                    Command.CommandText = @"select trigger_name  from user_triggers".ToUpper();
                    dataReader = Command.ExecuteReader();
                    while (dataReader.Read())
                        _TriggerNames.Add(dataReader["trigger_name".ToUpper()] as string);
                    dataReader.Close();

                    if (TablesColumns.Count > 1)
                    {
                        #region Retrieves ForeignKeys
                        Command = connection.CreateCommand();
                        Command.CommandText = string.Format(@"select
                                    COLUMNS.TABLE_NAME AS FK_TABLE_NAME, COLUMNS.COLUMN_NAME,
                                    REFERENCE_COLUMNS.TABLE_NAME as REFERENCED_TABLE_NAME , REFERENCE_COLUMNS.column_name as REFERENCED_COLUMN_NAME,CONSTRAINTS.CONSTRAINT_NAME,CONSTRAINTS.OWNER
                                from
                                    user_tab_columns COLUMNS
                                    join user_cons_columns CONSTRAINT_COLUMNS 
                                      on COLUMNS.table_name = CONSTRAINT_COLUMNS.table_name 
                                     and COLUMNS.column_name = CONSTRAINT_COLUMNS.column_name
                                    join user_constraints CONSTRAINTS 
                                      on CONSTRAINT_COLUMNS.constraint_name = CONSTRAINTS.constraint_name
                                    join user_cons_columns REFERENCE_COLUMNS 
                                      on CONSTRAINTS.r_constraint_name = REFERENCE_COLUMNS.constraint_name 
                                     and CONSTRAINT_COLUMNS.position = REFERENCE_COLUMNS.position
                                where
                                    CONSTRAINTS.constraint_type = 'R'
                                    and
                                    lower(CONSTRAINTS.owner)=lower('{0}')", DataBase.Name);

                        dataReader = Command.ExecuteReader();
                        while(dataReader.Read())
                        {
                            Dictionary<string, ForeignKeyData> tableForeignKeys = null;
                            if (!TablesForeignKeys.TryGetValue((string)dataReader["FK_TABLE_NAME"], out tableForeignKeys))
                            {
                                tableForeignKeys = new Dictionary<string, ForeignKeyData>();
                                TablesForeignKeys[(string)dataReader["FK_TABLE_NAME"]] = tableForeignKeys;
                            }
                            ForeignKeyData foreignKeyData = null;
                            if (!tableForeignKeys.TryGetValue((string)dataReader["CONSTRAINT_NAME"], out foreignKeyData))
                            {
                                foreignKeyData = new ForeignKeyData((string)dataReader["CONSTRAINT_NAME"], (string)dataReader["FK_TABLE_NAME"], (string)dataReader["REFERENCED_TABLE_NAME"]);
                                tableForeignKeys[foreignKeyData.Name] = foreignKeyData;
                            }
                            foreignKeyData.ColumnsNames.Add((string)dataReader["COLUMN_NAME"]);
                            foreignKeyData.ReferedColumnsNames.Add((string)dataReader["REFERENCED_COLUMN_NAME"]);

                        }
                        dataReader.Close();
                        #endregion
                    }
                }

                transactionScope.Complete();
            }

        }
        /// <MetaDataID>{7240e8b1-cf73-4f66-80c5-033f06b4515f}</MetaDataID>
        public System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName)
        {
            if (TablesColumns.Count == 0)
                RetrieveDataBaseDDLData();
            List<ColumnData> columns = null;
            if (!TablesColumns.TryGetValue(tableName, out columns))
                return new List<ColumnData>();
            else
                return columns;

            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = DataBase.SchemaUpdateConnection;
                try
                {
                    if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                        connection.Open();

                    var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);

                    int identityIncrement = 1;
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader dataReader = null;

                    command.CommandText = string.Format("SELECT * FROM SYS.all_tab_columns where lower(table_name)=lower('{1}') and lower(owner)=lower('{0}')", DataBase.Name, tableName);
                    dataReader = command.ExecuteReader();
                    // Load table columns
                    while(dataReader.Read())
                    {
                        string dataType = (string)dataReader["DATA_TYPE"];

                        //'', 'abstractions', 'objectblobs', 'ID', 1, '', 'NO', 'int', , , 10, 0, '', '', 'int(11)', 'PRI', 'auto_increment', 'select,insert,update,references', ''

                        bool identityColumn = false;
                        string columnName = dataReader["COLUMN_NAME"].ToString();

                        int length = (int)(decimal)dataReader["DATA_LENGTH"];

                        if (dataType.ToLower() == "nvarchar2")
                            length = length / 2;
                        ColumnData columnData = new ColumnData(columnName, dataType, length, dataReader["NULLABLE"].ToString().ToLower() == "y", identityColumn, identityIncrement);
                        columnsData.Add(columnData);
                    }
                    dataReader.Close();
                }
                catch (System.Exception Error)
                {
                    System.Diagnostics.Debug.Assert(false);
                    throw new System.Exception("Table with name '" + tableName + "'failed to read its columns", Error);
                }
                transactionScope.Complete();
            }
            return columnsData;

        }
        /// <MetaDataID>{fd2dd380-f925-4d8f-b5a6-30da05c0cc50}</MetaDataID>
        public PrimaryKeyData RetrieveDatabasePrimaryKeys(Table table)
        {
            if (TablesColumns.Count == 0)
                RetrieveDataBaseDDLData();
            PrimaryKeyData primaryKeyData = null;
            TablesPrimaryKeys.TryGetValue(table.Name, out primaryKeyData);
            return primaryKeyData;

            System.Collections.Generic.List<string> primaryKeys = new System.Collections.Generic.List<string>();
            //System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;

            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = (table.Namespace as DataBase).SchemaUpdateConnection;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();
                var Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
                //            Command.CommandText = string.Format(@"SELECT     CONSTRAINT_NAME
                //                                            FROM        ""information_schema"".""TABLE_CONSTRAINTS""
                //                                            WHERE     (CONSTRAINT_TYPE = 'PRIMARY KEY') AND (TABLE_NAME = '{0}')", table.Name);
                Command.CommandText = string.Format(@"SELECT cols.column_name, cons.constraint_name
            FROM all_constraints cons, all_cons_columns cols
            WHERE cons.constraint_type = 'P'
            AND cons.constraint_name = cols.constraint_name
            AND cons.owner = cols.owner 
            and lower(cons.owner)= lower('{0}')
            and lower(cols.table_name)= lower('{1}')", table.Namespace.Name, table.Name);
                var dataReader = Command.ExecuteReader();
                // Load table Primary Keys

                while ( dataReader.Read())
                {
                    if (primaryKeyData == null)
                        primaryKeyData = new PrimaryKeyData((string)dataReader["constraint_name"], (string)dataReader["table_name"]);
                    primaryKeyData.ColumnsNames.Add((string)dataReader["column_name"]);
                }
                dataReader.Close();
                transactionScope.Complete();
            }
            return primaryKeyData;
        }
        /// <MetaDataID>{727b3ddf-fcc1-41b4-8450-833fe160a4cc}</MetaDataID>
        public System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table)
        {

            if (TablesColumns.Count == 0)
                RetrieveDataBaseDDLData();
            Dictionary<string, ForeignKeyData> foreignKeys = null;
            if (TablesForeignKeys.TryGetValue(table.Name, out foreignKeys))
                return new List<ForeignKeyData>(foreignKeys.Values);
            else
                return new List<ForeignKeyData>();


            System.Collections.Generic.List<ForeignKeyData> foreignKeysData = new System.Collections.Generic.List<ForeignKeyData>();

            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                var connection = (table.Namespace as DataBase).SchemaUpdateConnection;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();
                var Command = connection.CreateCommand();

                Command.CommandText = string.Format(@"select
                                    col.table_name , col.column_name,
                                    rel.table_name as referenced_table_name , rel.column_name as referenced_column_name,cc.constraint_name,cc.owner
                                from
                                    user_tab_columns col
                                    join user_cons_columns con 
                                      on col.table_name = con.table_name 
                                     and col.column_name = con.column_name
                                    join user_constraints cc 
                                      on con.constraint_name = cc.constraint_name
                                    join user_cons_columns rel 
                                      on cc.r_constraint_name = rel.constraint_name 
                                     and con.position = rel.position
                                where
                                    cc.constraint_type = 'R'
                                    and
                                    lower(col.table_name)=lower('{0}')
                                    and
                                    lower(cc.owner)=lower('{1}')", table.Name, table.Namespace.Name);

                var dataReader = Command.ExecuteReader();
                while (dataReader.Read())
                    foreignKeysData.Add(new ForeignKeyData((string)dataReader["constraint_name"], (string)dataReader["FK_TABLE_NAME"], (string)dataReader["REFERENCED_TABLE_NAME"]));
                dataReader.Close();
                transactionScope.Complete();

            }
            return foreignKeysData;

        }
        /// <MetaDataID>{5e9ee493-dff2-4a05-8bcb-de0459e9c039}</MetaDataID>
        public Dictionary<string, string> GetViewsNamesAndDefinition()
        {
            Dictionary<string, string> viewsNamesAndDefinition = new Dictionary<string, string>();
            try
            {

                //string connectionString = "Data Source=[Location];User ID=system;Password=astraxan;Unicode=True".Replace("[Location]", this.DataBase.Storage.StorageLocation);
                string connectionString = "Data Source=[Location];User ID=system;Password=astraxan".Replace("[Location]", this.DataBase.Storage.StorageLocation);
                //connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=rocket)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=[Location])));User Id=[User];Password=astraxan;".Replace("[Location]", this.DataBase.Storage.StorageLocation).Replace("[User]", "SYSTEM");
                var connection = DataBase.SchemaUpdateConnection;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();
                var command = connection.CreateCommand();

                //command.CommandText = string.Format("SELECT view_name,text FROM SYS.ALL_VIEWS where lower(owner)= lower('{0}')", DataBase.Name);
                command.CommandText = @"select dbms_xmlgen.getxml('select view_name,text from user_views') view_data from dual";


                var dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string viewData = dataReader["view_data"] as string;
                    if (string.IsNullOrEmpty(viewData))
                        continue;
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(viewData);
                    foreach (System.Xml.XmlElement rowElement in doc.SelectNodes("//ROW"))
                    {
                        string viewName = rowElement.SelectSingleNode("VIEW_NAME").InnerText;
                        string viewDefinition = rowElement.SelectSingleNode("TEXT").InnerText;
                        string createView = "CREATE VIEW ";
                        createView += "\"" + DataBase.Name + "\".\"" + viewName + "\"  AS ";
                        viewsNamesAndDefinition[viewName] = (createView + viewDefinition).ToUpper();
                    }

                    //string viewName = dataReader["view_name"].ToString();
                    //string viewDefinition = dataReader["text"].ToString();
                    //viewsNamesAndDefinition[viewName] = viewDefinition;
                }
                dataReader.Close();

            }
            catch (System.Exception Error)
            {
            }
            return viewsNamesAndDefinition;
        }
        public string GetAssociatedSequence(string tableName)
        {
            if (TablesColumns.Count == 0)
                RetrieveDataBaseDDLData();

            string sequenceName = null;
            TablesSequences.TryGetValue(tableName, out sequenceName);
            return sequenceName;
        }
        /// <MetaDataID>{abcee9f0-bb5f-433d-9bcd-46059a958862}</MetaDataID>
        public System.Collections.Generic.List<string> GetTablesNames()
        {
            if (TablesColumns.Count == 0)
                RetrieveDataBaseDDLData();
            else
                RefreshDDLData();
            return new List<string>(TablesColumns.Keys);
            System.Collections.Generic.List<string> tableNames = new System.Collections.Generic.List<string>();
            try
            {
                string connectionString = "Data Source=[Location];User ID=system;Password=astraxan".Replace("[Location]", this.DataBase.Storage.StorageLocation);
                System.Data.Common.DbCommand command = new OracleConnection(connectionString).CreateCommand();
                command.Connection.Open();
                command.CommandText = string.Format("SELECT TABLE_NAME FROM SYS.ALL_TABLES where lower(owner)= lower('{0}')", DataBase.Name);
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string tableName = dataReader["TABLE_NAME"].ToString();
                    tableNames.Add(tableName);
                }
                dataReader.Close();
                command.Connection.Close();
            }
            catch (System.Exception Error)
            {
            }
            return tableNames;
        }
        /// <MetaDataID>{8965ba12-d031-477e-a75a-590cf92936c2}</MetaDataID>
        public System.Collections.Generic.List<string> GetStoreProcedureNames()
        {
            System.Collections.Generic.List<string> storeProcedureNames = new System.Collections.Generic.List<string>();
            try
            {
                //if (Connection.State != System.Data.ConnectionState.Open)
                //    Connection.Open();



                //string connectionString = "Data Source=[Location];User ID=system;Password=astraxan;Unicode=True".Replace("[Location]",this.DataBase.Storage.StorageLocation);
                string connectionString = "Data Source=[Location];User ID=system;Password=astraxan".Replace("[Location]", this.DataBase.Storage.StorageLocation);
                //connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=rocket)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=[Location])));User Id=[User];Password=astraxan;";

                System.Data.Common.DbCommand command = new OracleConnection(connectionString).CreateCommand();
                command.Connection.Open();
                command.CommandText = string.Format("SELECT procedure_name FROM DBA_Procedures WHERE lower( owner)=lower('{0}') and procedure_name is not null", DataBase.Name);
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string storeprocedureName = (string)dataReader["procedure_name"];
                    storeProcedureNames.Add(storeprocedureName);
                }
                dataReader.Close();
                command.Connection.Close();
            }
            catch (System.Exception Error)
            {
            }
            return storeProcedureNames;

        }

        /// <MetaDataID>{27df3860-e6a5-4143-9b75-3ffa6fd0ce12}</MetaDataID>
        void GetKeysInfos()
        {
            string command = @"SELECT KEYS.OWNER,KEYS.CONSTRAINT_NAME,KEYS.CONSTRAINT_TYPE,COLUMNS.TABLE_NAME, COLUMNS.COLUMN_NAME,ref_columns.TABLE_NAME REF_TABLE_NAME,ref_columns.column_name REF_COLUMN_NAME
                            FROM ALL_CONS_COLUMNS COLUMNS INNER JOIN ALL_CONSTRAINTS KEYS  ON COLUMNS.CONSTRAINT_NAME = KEYS.CONSTRAINT_NAME 
                            LEFT OUTER JOIN ALL_CONS_COLUMNS REF_COLUMNS ON ref_columns.constraint_name=KEYS.r_constraint_name
                            WHERE COLUMNS.OWNER ='FIRSTABSTRACTIONS'   AND (KEYS.CONSTRAINT_TYPE = 'R' OR KEYS.CONSTRAINT_TYPE = 'P')";
        }
        System.Collections.Generic.List<Transactions.Transaction> ActiveTransactions = new List<OOAdvantech.Transactions.Transaction>();
        /// <MetaDataID>{2d9c95c6-e783-4f3e-844f-a6573b9fb407}</MetaDataID>
        public List<string> GetTableDDLScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType)
        {

            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }

            string script = null;

            string columnsScript = null;
            Column autoIncrementAddedColumn = null;

            foreach (Column addedColumn in newColumns)
            {
                if (addedColumn.IdentityColumn)
                    autoIncrementAddedColumn = addedColumn;
                if (columnsScript != null)
                    columnsScript += ",\n";
                columnsScript += addedColumn.GetScript();
            }
            if (string.IsNullOrEmpty(columnsScript))
                return new List<string>();

            List<string> sqlScripts = new List<string>();
            if (newTable)
            {
                string dropTempTableScript = string.Format(@"DECLARE v_count NUMBER :=0; BEGIN SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name='{0}'; IF v_count = 1 THEN EXECUTE IMMEDIATE 'DROP TABLE {0}'; END IF; END;", tableName.ToUpper());

                if (tableType == TableType.VaraibleTable)
                {
                    sqlScripts.Add(dropTempTableScript);

                    script = "CREATE GLOBAL TEMPORARY TABLE " + tableName + " (\n" + columnsScript + ")\n ON COMMIT PRESERVE ROWS";
                }
                else if (tableType == TableType.TemporaryTable)
                {
                    sqlScripts.Add(dropTempTableScript);
                    script = "CREATE GLOBAL TEMPORARY TABLE " + tableName + " (\n" + columnsScript + ")\n  ON COMMIT PRESERVE  ROWS";
                }
                else
                    script = "CREATE TABLE " + DataBase.Name + "." + tableName + " (\n" + columnsScript + ")";
            }
            else
            {
                if (tableType == TableType.VaraibleTable)
                    script = "ALTER TABLE @" + tableName + " ADD \n" + columnsScript;
                else if (tableType == TableType.TemporaryTable)
                    script = "ALTER TABLE #" + tableName + " ADD \n" + columnsScript;
                else
                    script = "ALTER TABLE " + DataBase.Name + "." + tableName + " ADD \n" + columnsScript;
            }

            sqlScripts.Add(script.ToUpper());
            if (autoIncrementAddedColumn != null)
            {
                string sequenceName = GetValidateSequenceName(autoIncrementAddedColumn.Namespace.Name);
                string triggerName = GetValidateTriggerName(autoIncrementAddedColumn.Namespace.Name);
                string sequenceScript = string.Format("CREATE SEQUENCE {0} START WITH 1 INCREMENT BY {1}", sequenceName, autoIncrementAddedColumn.IdentityIncrement);
                sqlScripts.Add(sequenceScript.ToUpper());
                string trigerScript = string.Format(@"CREATE OR REPLACE TRIGGER {1} BEFORE INSERT ON {0} REFERENCING NEW AS NEW FOR EACH ROW BEGIN SELECT {2}.nextval INTO :NEW.{3} FROM dual;END;",
                    autoIncrementAddedColumn.Namespace.Name, triggerName, sequenceName, autoIncrementAddedColumn.Name);
                sqlScripts.Add(trigerScript.ToUpper());
            }


            return sqlScripts;
        }

        void TransactionCompleted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
            lock (ActiveTransactions)
            {
                ActiveTransactions.Remove(transaction);

                TablesColumns.Clear();
                TablesForeignKeys.Clear();
                TablesPrimaryKeys.Clear();
                TablesSequences.Clear();
                if (_SequenceNames != null)
                    _SequenceNames.Clear();

                if (_TriggerNames != null)
                    _TriggerNames.Clear();
            }


        }

        private string GetValidateTriggerName(string tableName)
        {
            string triggerName = string.Format("{0}_trig", tableName);
            if (triggerName.Length > 30)
            {
                triggerName = string.Format("{0}_trig", tableName.Substring(0, 30 - "_trig".Length));

                int count = 1;
                while (_TriggerNames.Contains(triggerName.ToUpper()))
                {
                    triggerName = string.Format("{1}{0}_trig", count.ToString(), tableName.Substring(0, 30 - "_trig".Length - count.ToString().Length));
                    count++;
                }
            }
            _TriggerNames.Add(triggerName.ToUpper());
            return triggerName.ToUpper();
        }
        System.Collections.Generic.List<string> _SequenceNames;
        System.Collections.Generic.List<string> SequenceNames
        {
            get
            {
                return _SequenceNames;
            }
        }
        System.Collections.Generic.List<string> _TriggerNames;
        System.Collections.Generic.List<string> TriggerNames
        {
            get
            {
                return _TriggerNames;
            }
        }



        private string GetValidateSequenceName(string tableName)
        {
            string sequenceName = string.Format("{0}_seq", tableName);
            if (sequenceName.Length > 30)
            {
                sequenceName = string.Format("{0}_seq", tableName.Substring(0, 30 - "_seq".Length));
                int count = 1;
                while (_SequenceNames.Contains(sequenceName.ToUpper()))
                {
                    sequenceName = string.Format("{1}{0}_seq", count.ToString(), tableName.Substring(0, 30 - "_seq".Length - count.ToString().Length));
                    count++;
                }
            }
            _SequenceNames.Add(sequenceName.ToUpper());
            return sequenceName.ToUpper();
        }

        /// <MetaDataID>{26859168-7756-42e0-80af-ec450f967405}</MetaDataID>
        string GetDataTypeCoversionStatement(string columnName, int columnLength, string orgType, string newType)
        {
            if (ExcludedConvertion.Contains(orgType.ToLower() + "-" + newType.ToLower()))
                throw new System.Exception("ExcludedConvertion" + orgType.ToLower() + "-" + newType.ToLower());

            string convertStatement = "CAST(" + columnName + " AS " + newType;
            if (columnLength > 0)
                convertStatement += "(" + columnLength.ToString() + ")";
            convertStatement += ")";
            return convertStatement;

        }


        /// <MetaDataID>{80dd3c3a-d675-4838-a814-398d16f1831d}</MetaDataID>
        static System.Collections.ArrayList ExcludedConvertion = new System.Collections.ArrayList();

        /// <MetaDataID>{c775f9e8-3de3-484d-8120-99db3bb16cee}</MetaDataID>
        static OracleRDBMSSchema()
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

        /// <MetaDataID>{b8731ad6-9e93-49c1-b5df-2bc041c0d054}</MetaDataID>
        public void UpdateColumnsFormat(Table table)
        {

            string createTemporaryTable = null;
            string transferDataCommand = null;
            string sourceColumns = null;
            string destColumns = null;


            {
                DataBase dataBase = table.Namespace as DataBase;
                var connection = dataBase.Connection;
                //if(connection==null)
                OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand command = null;
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
                            command.CommandText = GetDropKeyScript(key)[0];
                            resault = command.ExecuteNonQuery();
                        }
                    }
                    foreach (Key key in table.Keys)
                    {
                        if (!key.NewKey && !key.IsPrimaryKey)
                        {
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(key.GetDropKeyScript(), connection);
                            command.CommandText = GetDropKeyScript(key)[0];
                            resault = command.ExecuteNonQuery();
                        }
                    }
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(string.Format("DROP TABLE {0}",Name),connection);
                    command.CommandText = string.Format("DROP TABLE {0}", table.Name);
                    resault = command.ExecuteNonQuery();
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'","TMP_"+Name,Name),connection);
                    command.CommandText = string.Format(@"ALTER TABLE  {0} RENAME TO {1} ", "TMP_" + table.Name, table.Name);
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
        /// <MetaDataID>{4868a01a-5c78-41fe-9eaa-65c5c77bde9d}</MetaDataID>
        public List<string> GetRenameScript(string tableName, string newTableName)
        {
            return new List<string>() { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", tableName, newTableName) };
        }
        /// <MetaDataID>{8ea3985f-59ac-4927-8cf5-416797a19049}</MetaDataID>
        public string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName)
        {
            return string.Format("EXECUTE sp_rename N'{0}.{1}', N'{2}', 'COLUMN'", tableName, columnName, newColumnName);
        }
        /// <MetaDataID>{9da1e9f3-4ab2-4f0b-a6b7-3705af180d76}</MetaDataID>
        public void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns)
        {
            string dropColumnsScript = null;
            foreach (Column column in dropedColumns)
            {
                if (dropColumnsScript == null)
                    dropColumnsScript = string.Format("ALTER TABLE \"{0}\" \nDROP COLUMN ", tableName);
                else
                    dropColumnsScript += ",";
                dropColumnsScript += "\"" + column.Name + "\"";
            }
            if (dropColumnsScript != null)
            {
                var command = DataBase.SchemaUpdateConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropColumnsScript, connection);
                command.CommandText = dropColumnsScript.ToUpper();
                command.ExecuteNonQuery();
            }
        }
        /// <MetaDataID>{32f3cf17-f1d3-4d12-8e84-89a18d01c8b8}</MetaDataID>
        public List<string> GetViewDDLScript(RDBMSMetaDataRepository.View view, bool newView)
        {
            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }
            string AbstractClassView = "";

            if (newView)
                AbstractClassView += "CREATE VIEW ";
            else
                AbstractClassView += "CREATE OR REPLACE VIEW ";
            AbstractClassView += "\"" + view.Namespace.Name + "\".\"" + view.Name + "\"  AS ";
            string SelectList = null;
            SelectList = "Select ";
            string Separator = null;
            bool typeWithIDDefaultValue = false;
            foreach (RDBMSMetaDataRepository.Column CurrColumn in view.ViewColumns)
            {
                if (CurrColumn.Name == "TypeID" && !string.IsNullOrEmpty(CurrColumn.DefaultValue))
                    typeWithIDDefaultValue = true;

                CurrColumn.DataBaseColumnName = GetValidateColumnName(CurrColumn, view);
                if ((CurrColumn.DefaultValue != null && CurrColumn.DefaultValue.Length > 0) && CurrColumn.RealColumn == null)
                {
                    SelectList += Separator + CurrColumn.DefaultValue + @" AS """ + CurrColumn.DataBaseColumnName + @"""";
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
                            SelectList += Separator + CurrColumn.DefaultValue + @" AS """ + CurrColumn.DataBaseColumnName + @"""";
                            if (Separator == null)
                                Separator = ",";
                        }
                        else
                        {
                            if (CurrColumn.RealColumn.Namespace != null)
                                SelectList += Separator + CurrColumn.RealColumn.Namespace.Name + "." + CurrColumn.RealColumn.DataBaseColumnName;
                            else
                                SelectList += Separator + CurrColumn.RealColumn.DataBaseColumnName;

                            SelectList += @" AS """ + CurrColumn.DataBaseColumnName + @"""";
                            if (Separator == null)
                                Separator = ",";
                        }
                    }
                    else
                    {
                        SelectList += Separator + CurrColumn.DataBaseColumnName;
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
                    FromClause = "From \"" + CurrTable.Namespace.Name + "\".\"" + CurrTable.Name + "\" ";
                    MainTable = CurrTable;
                }
                else
                {

                    FromClause += " INNER JOIN \"" + CurrTable.Name + "\" ON \"" + MainTable.Name + "\".IntObjID = " + CurrTable.Name + ".IntObjID ";
                    FromClause += "AND " + MainTable.Name + ".ObjCellID = " + CurrTable.Name + ".ObjCellID ";
                }
            }
            string ViewQuery = null;

            //string ViewQuery="\nWHERE "+ObjectCollection.MainTable.Name+".TypeID = "+ObjectCollection.Type.theStorageInstanceRef.ObjectID.ToString();
            if (view.JoinedTables.Count > 0)
            {
                ViewQuery = SelectList + "\n" + FromClause + "\n";//WHERE "+ObjectCollection.MainTable.Name+".TypeID = "+ObjectCollection.Type.theStorageInstanceRef.ObjectID.ToString();
                if (view.StorageCell != null && !typeWithIDDefaultValue)
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
                    CurrColumn.DataBaseColumnName = GetValidateColumnName(CurrColumn, view);
                    bool exist = false;
                    foreach (string ColumnName in CurrView.ViewColumnsNames)
                    {
                        if (ColumnName.ToLower().Trim() == CurrColumn.Name.ToLower().Trim())
                        {
                            ViewQuery += Separator + CurrColumn.DataBaseColumnName;
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

                        ViewQuery += Separator + nullValue + @" AS """ + CurrColumn.DataBaseColumnName + @"""";
                        if (Separator == null)
                            Separator = ",";
                    }
                }
                ViewQuery += "\nFrom \"" + CurrView.Namespace.Name + @""".""" + CurrView.Name + @"""";

            }

            if (ViewQuery == null)
            {
                string whereClause = null;
                foreach (RDBMSMetaDataRepository.Column column in view.ViewColumns)
                {
                    column.DataBaseColumnName = GetValidateColumnName(column, view);
                    string nullValue = "NULL";
                    if (column.Type != null)
                        nullValue = DataBase.TypeDictionary.GetDBNullScript(column.Type.FullName);

                    if (ViewQuery == null)
                    {
                        whereClause = "WHERE " + column.Name + " <> " + nullValue;
                        ViewQuery += "SELECT " + nullValue + @" AS """ + column.DataBaseColumnName + @"""";
                    }
                    else
                        ViewQuery += "," + nullValue + @" AS """ + column.DataBaseColumnName + @"""";
                }
                ViewQuery = SelectList + " FROM (" + ViewQuery + ") [TABLE] " + whereClause;
            }

            AbstractClassView +=/*"\n"+SelectList+"\n"*/ViewQuery;
            System.Diagnostics.Debug.WriteLine(AbstractClassView);
            return new List<string>() { AbstractClassView.ToUpper() };
        }
        /// <MetaDataID>{a31cf7e3-b294-4ee4-aee9-3ed1e6b444b3}</MetaDataID>
        public string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {
            return "";
        }


        #endregion


        /// <MetaDataID>{2d89a88f-62e1-4371-8a35-c454f1d87dbc}</MetaDataID>
        public string GetColumDefinitionScript(Column column)
        {

            string script = column.Name + " " + column.Datatype;
            if (column.Length > 0)
                script += "(" + column.Length.ToString() + ")";
            script += " ";


            //if (column.IdentityColumn)
            //    script += " AUTO_INCREMENT";

            //if (column.Datatype.ToLower() == "NVARCHAR2".ToLower())
            //    script += " CHARACTER SET utf8 COLLATE utf8_general_ci";
            if (column.AllowNulls)
                script += " NULL";
            else
                script += " NOT NULL";



            //if (column.IdentityColumn)
            //    script += " IDENTITY(1," + IdentityIncrement.ToString() + ")";
            return script;



        }
        #region IRDBMSSchema Members


        ///// <MetaDataID>{0a885141-89f1-400c-974e-dc7063c7a966}</MetaDataID>
        //public string ConvertToSQLString(object value)
        //{
        //    return DataBase.TypeDictionary.ConvertToSQLString(value);
        //}

        ///// <MetaDataID>{00740442-3326-4453-80ae-2e3f5fa13046}</MetaDataID>
        //public int GeDefaultLength(string typeName)
        //{
        //    return DataBase.TypeDictionary.GeDefaultLength(typeName);

        //}

        ///// <MetaDataID>{b01f3af7-c581-4bb2-a815-50a91f4c43e9}</MetaDataID>
        //public string GetDBType(string typeName, bool fixedLength)
        //{
        //    return DataBase.TypeDictionary.GetDBType(typeName, fixedLength);
        //}

        ///// <MetaDataID>{a6a83503-beeb-44e6-8885-fde347723829}</MetaDataID>
        //public bool IsTypeVarLength(string typeName)
        //{
        //    return DataBase.TypeDictionary.IsTypeVarLength(typeName);
        //}

        #endregion








        /// <MetaDataID>{525d4ea2-2982-41c2-a730-d1adc25bb3c0}</MetaDataID>
        public string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable,System.Collections.Generic.IList<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn, int startIndex, int endIndex, int change)
        {
            string IndexCriterion = "";


            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                if (string.IsNullOrEmpty(IndexCriterion))
                    IndexCriterion = " WHERE ";
                else
                    IndexCriterion += " AND ";
                IndexCriterion += @"""" + column.DataBaseColumnName + @""" = " + DataBase.TypeDictionary.ConvertToSQLString(((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType));
            }

            string indexUpdate = null;
            if (change > 0)
            {
                indexUpdate = @"UPDATE """ + KeepColumnTable.DataBaseTableName + @"""
                                SET """ + IndexerColumn.DataBaseColumnName + @""" = """ + IndexerColumn.DataBaseColumnName + @""" + " + change.ToString()
                                     + IndexCriterion + @" AND """ + IndexerColumn.DataBaseColumnName + @""" >= " + startIndex.ToString()
                                     + @" AND """ + IndexerColumn.DataBaseColumnName + @""" <= " + endIndex.ToString() + " \n";
            }
            else
            {
                indexUpdate = @"UPDATE """ + KeepColumnTable.DataBaseTableName + @"""
                                SET """ + IndexerColumn.DataBaseColumnName + @""" = """ + IndexerColumn.DataBaseColumnName + @""" + " + change.ToString()
                + IndexCriterion + @" AND """ + IndexerColumn.DataBaseColumnName + @""" >= " + startIndex.ToString()
                + @" AND """ + IndexerColumn.DataBaseColumnName + @""" <= " + endIndex.ToString() + " \n";
            }

            return indexUpdate;
        }




        public List<string> GetDropTableScript(string tableName, TableType tableType)
        {
            if (Transactions.Transaction.Current != null)
            {
                lock (ActiveTransactions)
                {
                    if (!ActiveTransactions.Contains(Transactions.Transaction.Current))
                    {
                        Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompleted);
                        ActiveTransactions.Add(Transactions.Transaction.Current);
                    }
                }
            }

            if (tableType == TableType.TemporaryTable)
                return new List<string>() { "TRUNCATE TABLE " + tableName.ToUpper(), "DROP TABLE " + tableName.ToUpper() };


            return new List<string>() { "DROP TABLE " + tableName.ToUpper() };
        }

        public string AliasDefSqlScript
        {
            get { return "  "; }
        }

        //public string GetSQLScriptForName(string name, Dictionary<string, string> orgNamesDictionary)
        //{
        //    if(orgNamesDictionary.ContainsKey(name))
        //        return @"""" + orgNamesDictionary[name].ToUpper() + @"""";
        //    else
        //        return @"""" + name.ToUpper() + @"""";

        //}

        public string GetSQLScriptForName(string name)
        {
            return @"""" + name.ToUpper() + @"""";
        }
        //public string GetDataNodeValidAlias(string alias, Dictionary<string, string> aliasesDictionary)
        //{
        //    if (alias.Length > 30)
        //    {
        //        //List<string> aliasNames = new List<string>(aliasesDictionary.Values);
        //        int count = 1;
        //        string validAlias = alias.Substring(0, 30 - count.ToString().Length);
        //        validAlias += count.ToString();
        //        while (aliasesDictionary.ContainsKey(validAlias.ToUpper()))
        //        {
        //            count++;
        //            validAlias = alias.Substring(0, 30 - count.ToString().Length);
        //        }

        //        aliasesDictionary[validAlias.ToUpper()] = alias;
        //        return validAlias.ToUpper();
        //    }
        //    else
        //    {
        //        aliasesDictionary[alias.ToUpper()] = alias;
        //        return alias.ToUpper();
        //    }
        //}
        /// <summary>
        /// Returns a valid script for name according the RDBMS (oracle,slq server etc)
        /// </summary>
        /// <param name="rdbmsFriendlyNamesDictionary">
        /// Defines a dictionary with the rdbms friedlly names as key and original name as value
        /// </param>
        /// <param name="objectLifeTimeManagerNamesDictionary">
        /// Defines a dictionary with the object lifetime manager names as key and valid for RDBMS name as value
        /// </param>
        /// <param name="objectLifeTimeManagerName">Defines the name where the operation must produce valid alias SQL script </param>
        /// <returns>Valid SQL script for object lifetime manager name </returns>
        //public string GetSQLScriptValidAlias(Dictionary<string, string> rdbmsFriendlyNamesDictionary, Dictionary<string, string> objectLifeTimeManagerNamesDictionary, string objectLifeTimeManagerName)
        //{
        //    if (objectLifeTimeManagerName.Length > 30)
        //    {
        //        if (objectLifeTimeManagerNamesDictionary.ContainsKey(objectLifeTimeManagerName))
        //            return GetSQLScriptForName(objectLifeTimeManagerName,objectLifeTimeManagerNamesDictionary);
        //        //List<string> aliasNames = new List<string>(aliasesDictionary.Values);
        //        int count = 1;
        //        string validAlias = objectLifeTimeManagerName.Substring(0, 30 - count.ToString().Length);
        //        validAlias += count.ToString();
        //        while (rdbmsFriendlyNamesDictionary.ContainsKey(validAlias.ToUpper()))
        //        {
        //            count++;
        //            validAlias = objectLifeTimeManagerName.Substring(0, 30 - count.ToString().Length) + count.ToString();
        //        }

        //        rdbmsFriendlyNamesDictionary[validAlias.ToUpper()] = objectLifeTimeManagerName;
        //        objectLifeTimeManagerNamesDictionary[objectLifeTimeManagerName] = validAlias.ToUpper();
        //        return GetSQLScriptForName(validAlias,objectLifeTimeManagerNamesDictionary);
        //    }
        //    else
        //    {
        //        rdbmsFriendlyNamesDictionary[objectLifeTimeManagerName.ToUpper()] = objectLifeTimeManagerName;
        //        objectLifeTimeManagerNamesDictionary[objectLifeTimeManagerName] = objectLifeTimeManagerName.ToUpper();
        //        return GetSQLScriptForName(objectLifeTimeManagerName,objectLifeTimeManagerNamesDictionary);
        //    }
        //}

        public string GeValidRDBMSName(string name, System.Collections.Generic.List<string> excludedValidNames)
        {
            if (name.Length > 30)
            {
                int count = 1;
                string validAlias = name.Substring(0, 30 - count.ToString().Length);
                validAlias += count.ToString();
                while (excludedValidNames.Contains(validAlias.ToUpper()))
                {
                    count++;
                    validAlias = name.Substring(0, 30 - count.ToString().Length) + count.ToString();
                }
                return validAlias.ToUpper();
            }
            else
                return name.ToUpper();
        }
        //public string GeValidName(string name, Dictionary<string, string> aliasesDictionary, Dictionary<string, string> orgNamesDictionary)
        //{
        //    if (name.Length > 30)
        //    {
        //        if (orgNamesDictionary.ContainsKey(name))
        //            return orgNamesDictionary[name];
        //        int count = 1;
        //        string validAlias = name.Substring(0, 30 - count.ToString().Length);
        //        validAlias += count.ToString();
        //        while (aliasesDictionary.ContainsKey(validAlias.ToUpper()))
        //        {
        //            count++;
        //            validAlias = name.Substring(0, 30 - count.ToString().Length) + count.ToString();
        //        }
        //        aliasesDictionary[validAlias.ToUpper()] = name;
        //        orgNamesDictionary[name] = validAlias.ToUpper();
        //        return orgNamesDictionary[name];
        //    }
        //    else
        //    {
        //        aliasesDictionary[name.ToUpper()] = name;
        //        orgNamesDictionary[name] = name.ToUpper();
        //        return orgNamesDictionary[name];
        //    }
        //}

  


        public string IndexOfSqlScript(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationOwner, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent assignedObject, OOAdvantech.RDBMSMetaDataRepository.Table keepColumnTable, IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> ownerObjectIDColumns, IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> indexOfObjectIDColumns, OOAdvantech.RDBMSMetaDataRepository.Column IndexerColumn)
        {
            string indexOfSelectionScript = string.Format(@"SELECT ""{0}"".""{1}"" ", keepColumnTable.DataBaseTableName, IndexerColumn.DataBaseColumnName);
            string fromScript = string.Format("FROM [{0}] ", keepColumnTable.DataBaseTableName);
            string indexCriterion = null;



            foreach (RDBMSMetaDataRepository.IdentityColumn column in ownerObjectIDColumns)
            {
                if (string.IsNullOrEmpty(indexCriterion))
                    indexCriterion = "WHERE ";
                else
                    indexCriterion += " AND ";
                indexCriterion += @"""" + column.DataBaseColumnName + @""" = " + DataBase.TypeDictionary.ConvertToSQLString(assignedObject.ObjectID.GetMemberValue(column.ColumnType));
            }
            return indexOfSelectionScript + fromScript + indexCriterion;
        }

      
    }
}
