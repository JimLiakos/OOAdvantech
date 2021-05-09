using System;
using System.Collections.Generic;
using System.Text;

using OOAdvantech.RDBMSDataObjects;
//using OOAdvantech.RDBMSPersistenceRunTime;
namespace OOAdvantech.MySQLPersistenceRunTime
{

    /// <MetaDataID>{86ec8524-974d-4338-9008-9706dae25ee3}</MetaDataID>
    public class MySQLRDBMSSchema : OOAdvantech.RDBMSDataObjects.IRDBMSSchema
    {
        public bool ValidateColumnName(Column column)
        {
            return false;
        }
        public void ValidateTableName(Table table)
        {
        }
        public void ValidateKeyName(Key key)
        {
        }

        public void ValidateViewName(View view)
        {

        }
        public void ValidateStoreprocedureName(StoreProcedure storeProcedure)
        {
        }

        /// <MetaDataID>{9ab20de6-001d-4b41-902f-c6f84d044ef5}</MetaDataID>
        public void ValidateStoreprocedureName(Column column)
        {
        }
        /// <MetaDataID>{8413eafb-26fc-43ed-91dc-b4ea4de1c188}</MetaDataID>
        public string BuildIndexUpdateStatament(RDBMSMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef AssignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns, int roleAIndex, int roleBIndex)
        {
            return "";
        }

        /// <MetaDataID>{a6873ce3-a54a-45ac-b481-f825e0c6eaa2}</MetaDataID>
        DataBase DataBase;

        /// <MetaDataID>{7ec64382-758d-4a19-b8a4-1302a70e3e1c}</MetaDataID>
        System.Data.Common.DbConnection Connection
        {
            get
            {
                return DataBase.Connection;
            }
        }

        /// <MetaDataID>{df2c448c-2986-4591-a241-04571eab2bcf}</MetaDataID>
        public MySQLRDBMSSchema(DataBase dataBase)
        {
            DataBase = dataBase;
        }
        #region SQLStatamentBuilder
        
        public System.Collections.Generic.List<string> GetStoreProcedureNames()
        {
            System.Collections.Generic.List<string> storeProcedureNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
                    Connection.Open();
                System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText =string.Format("SELECT * FROM information_schema.ROUTINES where ROUTINE_SCHEMA='{0}' and ROUTINE_TYPE='PROCEDURE'",DataBase.Name);
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string storeprocedureName = (string)dataReader["ROUTINE_NAME"];
                    if ((string)dataReader["PROCEDURE_OWNER"] == "sys")
                        continue;
                    storeProcedureNames.Add(storeprocedureName);
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return storeProcedureNames;

        }
        
        public System.Collections.Generic.List<string> GetViewsNames()
        {
            System.Collections.Generic.List<string> viewsNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
                    Connection.Open();
                System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "SELECT * FROM information_schema.VIEWS";
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                        string viewName = dataReader["TABLE_NAME"].ToString();
                        viewsNames.Add(viewName);
                    
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return viewsNames;
        }
        
        public System.Collections.Generic.List<string> GetTablesNames()
        {
            System.Collections.Generic.List<string> tableNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
                    Connection.Open();
                System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = string.Format("SELECT * FROM information_schema.`TABLES` where TABLE_SCHEMA ='{0}'",DataBase.Name);
                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    if ((string)dataReader["TABLE_TYPE"] == "BASE TABLE")
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

        /// <MetaDataID>{2a9dc768-e960-467b-af3d-e30f6352fab7}</MetaDataID>
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
                    insertClause = "INSERT INTO `" + relationTable.DataBaseTableName + "` (";
                else
                    insertClause += ",";

                insertClause += "`" + relationTableColumn.Name + "`";
                if (selectClause == null)
                    selectClause = "\nSELECT ";
                else
                    selectClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                        selectClause += "`" + (identityColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "`.`" + identityColumn.Name + "`";
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                insertClause += ",`" + relationTableColumn.Name + "`";

                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                        selectClause += ",`" + (identityColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "`.`" + identityColumn.Name + "`";
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
                    fromClause = "\nFROM `" + tableWithPrimaryKeyColumns.DataBaseTableName + "`" + " INNER JOIN `" + referenceColumnsTable.DataBaseTableName + "` ON ";
                else
                    fromClause += ",";

                fromClause += "`" + (referenceColumnsTable).DataBaseTableName + "`.`" + referenceColumn.DataBaseColumnName + "` = ";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in tableWithPrimaryKeyColumns.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == referenceColumn.ColumnType)
                        fromClause += "`" + tableWithPrimaryKeyColumns.DataBaseTableName + "`.`" + identityColumn.DataBaseColumnName + "` ";
                }
            }
            #endregion

            transferDataSQLStatement = insertClause + selectClause + fromClause;
            return transferDataSQLStatement;
        }

        /// <MetaDataID>{00aa63d2-9439-4fa0-a764-4c166e35d1dd}</MetaDataID>
        public string CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)
        {


            #region Build data transfer SQL statement
            string transferQuery = null;
            string fromClause = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            {
                if (transferQuery == null)
                    transferQuery += "UPDATE   `" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "` \nset ";
                else
                    transferQuery += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
                {
                    if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
                        transferQuery += "`" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "`" + ".`" + roleBColumn.DataBaseColumnName + "`=`" +
                            roleAObjectIDColumn.Namespace.Name + "`.`" + roleAObjectIDColumn.DataBaseColumnName + "`";
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                if (fromClause == null)
                    fromClause += "\nFROM `" + roleATable.Name + "`  INNER JOIN   `" + roleBTable.Name + "` ON ";
                else
                    fromClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
                {
                    if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
                        fromClause += "`" + (roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "`" + ".`" + roleAColumn.DataBaseColumnName + "`=`" +
                            roleBObjectIDColumn.Namespace.Name + "`.`" + roleBObjectIDColumn.DataBaseColumnName + "`";
                }
            }
            transferQuery += fromClause;
            #endregion
            return transferQuery;
        }

        /// <MetaDataID>{2514e382-ee68-4d19-8069-c6190a73c185}</MetaDataID>
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

        /// <MetaDataID>{242b5d28-ce1c-4f52-af24-38cfd0d6e5bb}</MetaDataID>
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

            //SET FOREIGN_KEY_CHECKS=0
            //SET FOREIGN_KEY_CHECKS=1
            if (key.IsPrimaryKey)
            {
                string Script = "ALTER TABLE `" + key.OwnerTable.Namespace.Name+"`.`"+ key.OwnerTable.Name + "` ADD CONSTRAINT \n" +
                    key.Name + " PRIMARY KEY CLUSTERED ( ";
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
                commands.Add(Script);
                return commands;
            }
            else
            {


                
    //ALTER TABLE `dbo`.`testtable` ADD CONSTRAINT `FK_testtable_1` FOREIGN KEY `FK_testtable_1` (`RefID`)
    //REFERENCES `serma` (`ID`)
    //c
    //ON UPDATE NO ACTION;
                string Script = "ALTER TABLE `" + key.OwnerTable.Namespace.Name + "`.`" + key.OwnerTable.Name + "`  ADD CONSTRAINT \n" +
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
                Script += ColumnsScript + " ) ON DELETE NO ACTION ON UPDATE NO ACTION";
                List<string> commands = new List<string>();
                commands.Add(Script);
                return commands;
  
            }
        }
        public string GetRenameScript(Key key, string newName)
        {
            if (key.IsPrimaryKey)
                return "";
            else
                return string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'\n", key.Name, newName);
        }
        public string GetDropKeyScript(Key key)
        {
            return "ALTER TABLE " + key.OwnerTable.Name + " DROP CONSTRAINT " + key.Name;
        }
        /// <MetaDataID>{09d6d399-1ede-474d-9fa8-adfe626b89e2}</MetaDataID>
        public void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames)
        {
            System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();
            DataBase dataBase = key.Namespace.Namespace as DataBase;
            System.Data.Common.DbConnection connection = dataBase.Connection;

            string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
            command.CommandText = CommandString;
            System.Data.Common.DbDataReader DataReader = command.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in DataReader)
            {
                if (record["FK_NAME"].Equals(key.Name))
                {
                    string PKColumnName = (string)record["PKCOLUMN_NAME"];
                    string FKColumnName = (string)record["FKCOLUMN_NAME"];
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
            //Connection.Close();
        }
        /// <MetaDataID>{63d4e15d-9811-4848-b6bd-80a500c94522}</MetaDataID>
        public void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames)
        {
            DataBase dataBase = key.Namespace.Namespace as DataBase;
            System.Data.Common.DbConnection connection = dataBase.Connection;

            //string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
            string CommandString = string.Format(@"SELECT     TABLE_CONSTRAINTS.CONSTRAINT_NAME, KEY_COLUMN_USAGE.COLUMN_NAME
                                FROM         information_schema.TABLE_CONSTRAINTS INNER JOIN
                                information_schema.KEY_COLUMN_USAGE ON TABLE_CONSTRAINTS.CONSTRAINT_NAME = KEY_COLUMN_USAGE.CONSTRAINT_NAME
                                WHERE     (TABLE_CONSTRAINTS.TABLE_NAME = '{0}') AND (TABLE_CONSTRAINTS.CONSTRAINT_TYPE = 'PRIMARY KEY') AND 
                                (KEY_COLUMN_USAGE.TABLE_NAME = '{0}')", key.OwnerTable.Name);
                
            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
            command.CommandText = CommandString;
            System.Data.Common.DbDataReader DataReader = command.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in DataReader)
            {
                if (record["CONSTRAINT_NAME"].Equals(key.Name))
                {
                    string PKColumnName = (string)record["COLUMN_NAME"];
                    columnsNames.Add(PKColumnName);

                }
            }
            DataReader.Close();
            //Connection.Close();


        }
        
        public System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName)
        {
            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();

            System.Data.Common.DbConnection connection = Connection;

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);

                int identityIncrement = 1;
                System.Data.Common.DbDataReader dataReader = null;
//                command.CommandText = string.Format(@"Select IDENT_INCR from (SELECT IDENT_INCR(TABLE_NAME) AS IDENT_INCR 
//								FROM INFORMATION_SCHEMA.TABLES 
//								WHERE TABLE_NAME='{0}') IdentityData
//                                where IdentityData.IDENT_INCR is not NULL", tableName);

                
//                System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
//                foreach (System.Data.Common.DbDataRecord record in dataReader)
//                {
//                    identityIncrement = (int)System.Convert.ChangeType(record`"IDENT_INCR"`, typeof(int)); ;
//                    break;
//                }
//                dataReader.Close();



                command.CommandText =string.Format("SELECT * FROM information_schema.`COLUMNS` where table_schema ='{0}' and table_name='{1}'",DataBase.Name,tableName);
                dataReader = command.ExecuteReader();
                // Load table columns
                foreach (System.Data.Common.DbDataRecord CurrRecord in dataReader)
                {
                    string dataType = (string)CurrRecord["COLUMN_TYPE"];

                    //'', 'abstractions', 'objectblobs', 'ID', 1, '', 'NO', 'int', , , 10, 0, '', '', 'int(11)', 'PRI', 'auto_increment', 'select,insert,update,references', ''

                    bool identityColumn = false;
                    string extra = CurrRecord["EXTRA"] as string;

                    if (!string.IsNullOrEmpty(extra) && extra.IndexOf("auto_increment")!=null)
                        identityColumn = true;

                    string columnName = CurrRecord["COLUMN_NAME"].ToString();
                    int length = 0;
                    if (dataType.IndexOf("(") != -1)
                    {
                        string lengthStr = dataType.Substring(dataType.IndexOf("(") + 1).Trim();
                        lengthStr = lengthStr.Substring(0, lengthStr.Length - 1);
                        length = int.Parse(lengthStr);
                        dataType = CurrRecord["DATA_TYPE"].ToString();
                    }


                    ColumnData columnData = new ColumnData(columnName, dataType, length, CurrRecord["IS_NULLABLE"].ToString().ToLower()=="yes",identityColumn, identityIncrement);
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
        public System.Collections.Generic.List<string> RetrieveDatabasePrimaryKeys(Table table)
        {
            System.Collections.Generic.List<string> primaryKeys = new System.Collections.Generic.List<string>();
            System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            System.Data.Common.DbCommand Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
            Command.CommandText = string.Format(@"SELECT     CONSTRAINT_NAME
                                            FROM        `information_schema`.`TABLE_CONSTRAINTS`
                                            WHERE     (CONSTRAINT_TYPE = 'PRIMARY KEY') AND (TABLE_NAME = '{0}')", table.Name);
            System.Data.Common.DbDataReader dataReader = Command.ExecuteReader();
            // Load table Primary Keys
            foreach (System.Data.Common.DbDataRecord CurrRecord in dataReader)
                primaryKeys.Add((string)CurrRecord["CONSTRAINT_NAME"]);
            dataReader.Close();
            return primaryKeys;
        }
        public System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table)
        {
            System.Data.Common.DbConnection connection = (table.Namespace as DataBase).Connection;
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            System.Data.Common.DbCommand Command = connection.CreateCommand();
            Command.CommandText =string.Format(@"SELECT     TABLE_CONSTRAINTS.CONSTRAINT_NAME, REFERENTIAL_CONSTRAINTS.REFERENCED_TABLE_NAME
                                                    FROM         information_schema.TABLE_CONSTRAINTS INNER JOIN
                                                                          information_schema.REFERENTIAL_CONSTRAINTS ON TABLE_CONSTRAINTS.CONSTRAINT_NAME = REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME
                                                    WHERE     (TABLE_CONSTRAINTS.TABLE_NAME = '{0}') AND (TABLE_CONSTRAINTS.CONSTRAINT_TYPE = 'FOREIGN KEY')", table.Name);




            System.Data.Common.DbDataReader dataReader = Command.ExecuteReader();


            System.Collections.Generic.List<ForeignKeyData> foreignKeysData = new System.Collections.Generic.List<ForeignKeyData>();

            foreach (System.Data.Common.DbDataRecord CurrRecord in dataReader)
                foreignKeysData.Add(new ForeignKeyData((string)CurrRecord["CONSTRAINT_NAME"], (string)CurrRecord["REFERENCED_TABLE_NAME"]));

            dataReader.Close();
            return foreignKeysData;

        }
        /// <MetaDataID>{e1d0a927-02a5-40c5-b513-c115591db30e}</MetaDataID>
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
                    script = "CREATE TABLE " +DataBase.Name+"."+  tableName + " (\n" + columnsScript + ")";
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

            return script;
        }

        /// <MetaDataID>{2453d5b9-1a51-4da0-9b7b-0608283ebe0e}</MetaDataID>
        string GetDataTypeCoversionStatement(string columnName,int columnLength, string orgType, string newType)
        {
            if (ExcludedConvertion.Contains(orgType.ToLower() + "-" + newType.ToLower()))
                throw new System.Exception("ExcludedConvertion" + orgType.ToLower() + "-" + newType.ToLower());

            string convertStatement = "CONVERT(" + newType;
            if (columnLength > 0)
                convertStatement += "(" + columnLength.ToString() + ")";
            convertStatement += "," + columnName + ")";
            return convertStatement;

        }


        /// <MetaDataID>{a289a220-3d53-46f0-a29f-d12cb07e3a56}</MetaDataID>
       static System.Collections.ArrayList ExcludedConvertion=new System.Collections.ArrayList();

       /// <MetaDataID>{4645ff3d-9a2a-4adc-9b3b-729451f6aa1d}</MetaDataID>
		static MySQLRDBMSSchema ()
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
                            sourceColumns +=GetDataTypeCoversionStatement(column.Name,column.Length,column.DataBaseColumnDataType,column.Datatype);
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
        public  string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName)
        {
            return string.Format("EXECUTE sp_rename N'{0}.{1}', N'{2}', 'COLUMN'", tableName, columnName, newColumnName);
        }
        /// <MetaDataID>{e09e44c8-34ba-4a74-bb82-eaf4e4af06e8}</MetaDataID>
        public  void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns)
        {
            string dropColumnsScript = null;
            foreach (Column column in dropedColumns)
            {
                if (dropColumnsScript == null)
                    dropColumnsScript = string.Format("ALTER TABLE [{0}] \nDROP COLUMN ", tableName);
                else
                    dropColumnsScript += "'";
                dropColumnsScript += "`" + column.Name + "`";
            }
            if (dropColumnsScript != null)
            {
                System.Data.Common.DbCommand command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropColumnsScript, connection);
                command.CommandText = dropColumnsScript;
                command.ExecuteNonQuery();
            }
        }
        public  string GetDefinition(RDBMSMetaDataRepository.View view, bool newView)
        {
            string AbstractClassView = "";

            if (newView)
                AbstractClassView += "CREATE VIEW ";
            else
                AbstractClassView += "ALTER VIEW ";
            AbstractClassView += "`"+view.Namespace.Name+ "`.`" + view.Name + "`  AS ";
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
                            CurrColumn.DefaultValue =DataBase.TypeDictionary.GetDBNullScript(CurrColumn.Type.FullName);
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
                        SelectList += Separator + CurrColumn.Name;
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
                    FromClause = "From `"+CurrTable.Namespace.Name+"`.`" + CurrTable.Name + "` ";
                    MainTable = CurrTable;
                }
                else
                {
                    FromClause += " INNER JOIN `" + CurrTable.Name + "` ON `" + MainTable.Name + "`.IntObjID = " + CurrTable.Name + ".IntObjID ";
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
                ViewQuery += "\nFrom `" + CurrView.Namespace.Name+"`.`" + CurrView.Name+"`";

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
        public  string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {
            return "";
         
         

        }
        
    
        #endregion


        public string GetColumDefinitionScript(Column column)
        {

            string script = column.Name + " " + column.Datatype;
            if (column.Length > 0)
                script += "(" + column.Length.ToString() + ")";
            script += " ";
           

            if (column.IdentityColumn)
                script += " AUTO_INCREMENT";

            if (column.Datatype.ToLower() == "VARCHAR".ToLower())
                script += " CHARACTER SET utf8 COLLATE utf8_general_ci";
            if (column.AllowNulls)
                script += " NULL";
            else
                script += " NOT NULL";



            //if (column.IdentityColumn)
            //    script += " IDENTITY(1," + IdentityIncrement.ToString() + ")";
            return script;


          
        }
        #region IRDBMSSchema Members


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

        #endregion

        #region IRDBMSSchema Members



        /// <MetaDataID>{910c6eef-bb46-46d1-be9a-bfe428147bca}</MetaDataID>
        public string BuildCountItemsInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, OOAdvantech.RDBMSMetaDataRepository.Table KeepColumnTable, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> linkColumns)
        {
            return "";
        }

        #endregion

        #region IRDBMSSchema Members


        /// <MetaDataID>{9ee5cf02-a679-4e7f-b0f6-6af3b95eeeda}</MetaDataID>
        public string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn, int startIndex, int endIndex, int change)
        {
            return "";
        }

        #endregion
    }
}
