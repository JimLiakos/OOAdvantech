using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.RDBMSDataObjects;

namespace OOAdvantech.SQLitePersistenceRunTime
{
    /// <MetaDataID>{16ef2a8b-8854-460b-affe-060a3b2a3df5}</MetaDataID>
    public class SQLiteRDBMSSchema : OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator
    {

        public bool AlwaysDeclareColumnAlias
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// There are RDBMS which supports addendum constraint key on create table
        /// and other RDBMS which not support
        /// This property is true when RDBMS supports addendum
        /// </summary>
        public bool SupportAddRemoveKeys { get { return false; } }


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
                return "";
            }
        }
        /// <MetaDataID>{a8a2e418-9a88-4333-ba9a-34ca22c3759c}</MetaDataID>
        public string GetTemporaryTableNameScript(string tableName)
        {
            return "#" + tableName;
        }
        /// <MetaDataID>{0d4759f0-0fe5-4015-b260-80f61bca7f3e}</MetaDataID>
        public string GetRowRemoveCaseScript(string filterString, string alias)
        {
            return string.Format(@", CASE WHEN {0} THEN cast(0 as bit) ELSE cast(1 as bit) END AS {1}", filterString, alias);
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





            //System.DateTime.Now.Second
            //weekday
            return null;

        }
        public bool SupportBatchSQL
        {
            get
            {
                return false;
            }
        }
        public string AliasDefSqlScript
        {
            get
            {
                return " AS ";
            }
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

        public string GetColumnsDefaultValuesSetScript(string tableName, System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues)
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

        public List<string> GetDropTableScript(string tableName, TableType tableType)
        {
            if (tableType == TableType.VaraibleTable)
                return new List<string>() { "DROP TABLE @" + tableName };
            else if (tableType == TableType.TemporaryTable)
                return new List<string>() { "DROP TABLE #" + tableName };
            else
                return new List<string>() { "DROP TABLE " + tableName };
        }
        public string GetSQLScriptForName(string name)
        {
            return @"[" + name + @"]";
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
        public bool GetValidateStoreprocedureName(StoreProcedure storeProcedure)
        {
            return false;
        }
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

        /// <MetaDataID>{ceaa1044-ee29-498f-a2b9-1f5015561ca0}</MetaDataID>
        DataBase DataBase;
        /// <MetaDataID>{724094d4-bcb6-46de-9c62-5f249750c709}</MetaDataID>
        RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                return DataBase.Connection;
            }
        }

        Parser.Parser Parser = new Parser.Parser();

        /// <MetaDataID>{ad2be195-0931-4e60-9183-9ee9b6ec6d4e}</MetaDataID>
        public SQLiteRDBMSSchema(DataBase dataBase)
        {
            DataBase = dataBase;

            System.IO.BinaryReader reader = null;
#if DeviceDotNet
            reader = GetResourceReader("OOAdvantech.SQLitePersistenceRunTime.SQLite.cgt");
#else
            reader =GetResourceReader("SQLitePersistenceRunTime.SQLite.cgt");
#endif
            
            byte[] grammar = new byte[reader.BaseStream.Length];
            reader.Read(grammar, 0, (int)reader.BaseStream.Length);
            Parser = new Parser.Parser();
            Parser.SetGrammar(grammar, grammar.Length);
        }

        private System.IO.BinaryReader GetResourceReader(string resourceName)
        {

            System.Reflection.Assembly assembly = this.GetType().GetMetaData().Assembly;
            System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new System.IO.BinaryReader(stream);
        }

        /// <MetaDataID>{e2001bde-e7eb-4f22-883d-2cc39d5d81c2}</MetaDataID>
        public System.Collections.Generic.List<string> GetStoreProcedureNames()
        {
            System.Collections.Generic.List<string> storeProcedureNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    Connection.Open();
                var command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "exec sp_stored_procedures";
                var dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string storeprocedureName = (string)dataReader["PROCEDURE_NAME"];
                    if ((string)dataReader["PROCEDURE_OWNER"] == "sys")
                        continue;
                    storeProcedureNames.Add(storeprocedureName.Substring(0, storeprocedureName.Length - 2));
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return storeProcedureNames;

        }

        public System.Collections.Generic.Dictionary<string, string> GetViewsNamesAndDefinition()
        {
            Dictionary<string, string> viewsNamesAndDefinition = new Dictionary<string, string>();
            try
            {
                if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    Connection.Open();
                var command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "SELECT * FROM sqlite_master where type = 'view'";// "exec sp_tables";
                var dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {
                    string viewName = dataReader["tbl_name"].ToString();
                    viewsNamesAndDefinition.Add(viewName, dataReader["sql"].ToString());
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return viewsNamesAndDefinition;
        }
        /// <MetaDataID>{50a6ca6d-2474-478c-9683-80752ca458cf}</MetaDataID>
        public System.Collections.Generic.List<string> GetTablesNames()
        {
            System.Collections.Generic.List<string> tableNames = new System.Collections.Generic.List<string>();
            try
            {
                if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    Connection.Open();
                var command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("exec sp_stored_procedures", Connection);
                command.CommandText = "SELECT name as TABLE_NAME FROM sqlite_master where type='table'";
                var dataReader = command.ExecuteReader();// .SqlClient.SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(command);
                while (dataReader.Read())
                {

                    string tableName = dataReader["TABLE_NAME"].ToString();
                    tableNames.Add(tableName);
                }
                dataReader.Close();
            }
            catch (System.Exception Error)
            {
            }
            return tableNames;
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
                    transferQuery += "UPDATE " + GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + " \r\nset ";
                else
                    transferQuery += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
                {
                    if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
                        transferQuery += GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + "." + GetSQLScriptForName(roleBColumn.DataBaseColumnName) + " = " +
                            GetSQLScriptForName(roleAObjectIDColumn.Namespace.Name) + "." + GetSQLScriptForName(roleAObjectIDColumn.DataBaseColumnName);
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                if (fromClause == null)
                    fromClause += "\nFROM " + GetSQLScriptForName(roleATable.Name) + " WHERE " + GetSQLScriptForName(roleBTable.Name) + " ";
                else
                    fromClause += " AND ";
                foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
                {
                    if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
                        fromClause += GetSQLScriptForName((roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + "." + GetSQLScriptForName(roleAColumn.DataBaseColumnName) + " = " +
                            GetSQLScriptForName(roleBObjectIDColumn.Namespace.Name) + "." + GetSQLScriptForName(roleBObjectIDColumn.DataBaseColumnName);
                }
            }
            transferQuery += fromClause;
#endregion
            return new List<string>() { transferQuery };






            //#region Build data transfer SQL statement
            //string transferQuery = null;
            //string fromClause = null;
            //foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            //{
            //    if (transferQuery == null)
            //        transferQuery += "UPDATE   [" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "] \nset ";
            //    else
            //        transferQuery += ",";
            //    foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
            //    {
            //        if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
            //            transferQuery += "[" + (roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "]" + ".[" + roleBColumn.DataBaseColumnName + "]=[" +
            //                roleAObjectIDColumn.Namespace.Name + "].[" + roleAObjectIDColumn.DataBaseColumnName + "]";
            //    }
            //}

            //foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            //{
            //    if (fromClause == null)
            //        fromClause += "\nFROM [" + roleATable.Name + "]  INNER JOIN   [" + roleBTable.Name + "] ON ";
            //    else
            //        fromClause += ",";
            //    foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
            //    {
            //        if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
            //            fromClause += "[" + (roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + "]" + ".[" + roleAColumn.DataBaseColumnName + "]=[" +
            //                roleBObjectIDColumn.Namespace.Name + "].[" + roleBObjectIDColumn.DataBaseColumnName + "]";
            //    }
            //}
            //transferQuery += fromClause;
            //#endregion
            //return new List<string>() { transferQuery };
        }
        /// <MetaDataID>{18d4d82b-0e52-4953-9ae5-b57a15314a26}</MetaDataID>
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
                    referenceCountTableSelectList += @"],[";
                else
                    referenceCountTableSelectList = @"[";
                referenceCountTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName;

                if (referenceCountTableFirstInnerJoinCondition != null)
                    referenceCountTableFirstInnerJoinCondition += @"],[";
                else
                    referenceCountTableFirstInnerJoinCondition = @"[";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in mainTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                        referenceCountTableFirstInnerJoinCondition += @"ReferenceCountTable].[" + referenceColumn.DataBaseColumnName +
                            @"] = [" + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName;
                }
                if (objectIDsInnerJoinCondition != null)
                    objectIDsInnerJoinCondition += @"],[";
                else
                    objectIDsInnerJoinCondition = @"[";

                objectIDsInnerJoinCondition += @"[ReferenceCountTable].[" + column.DataBaseColumnName +
                    @"] = [" + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName;
            }
            referenceCountTableSelectList += @"]";
            referenceCountTableFirstInnerJoinCondition += @"]";
            foreach (RDBMSMetaDataRepository.IdentityColumn column in referenceTable.ObjectIDColumns)
            {
                if (referenceCountTableSecondInnerJoinCondition != null)
                    referenceCountTableSecondInnerJoinCondition += @"],[";
                else
                    referenceCountTableSecondInnerJoinCondition = @"[";

                if (referenceTableSelectList != null)
                    referenceTableSelectList += @"],[";
                else
                    referenceTableSelectList = @"[";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceTableAssociationColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                    {
                        referenceCountTableSecondInnerJoinCondition += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName +
                            @"] = [" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + referenceColumn.DataBaseColumnName;
                        referenceTableSelectList += (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName;
                    }
                }
            }

            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceTableAssociationColumns)
                referenceTableSelectList += @"],[" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + referenceColumn.DataBaseColumnName;

            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in mainTableAssociationColumns)
                referenceTableSelectList += @"],[" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + referenceColumn.DataBaseColumnName;
            referenceTableSelectList += @"]";
            referenceCountTableSecondInnerJoinCondition += @"]";
            string updateQuery = @"UPDATE [" + mainTable.DataBaseTableName + @"]
                        SET [" + mainTable.DataBaseTableName + @"].[ReferenceCount]=[" + mainTable.DataBaseTableName + @"].[ReferenceCount] +(SELECT COUNT(*)  
                        FROM (SELECT " + referenceTableSelectList + @"
		                FROM [" + referenceTable.DataBaseTableName + @"] INNER JOIN [" + assoctiaionTable.DataBaseTableName + @"] ON " + referenceCountTableSecondInnerJoinCondition + @"
		                ) [ReferenceCountTable] WHERE " + referenceCountTableFirstInnerJoinCondition + @")";
            return new List<string>() { updateQuery };


        }
        /// <MetaDataID>{44d830d2-5a11-4e37-9a08-c19e1a4e66c5}</MetaDataID>
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
                        referenceCountTableInnerJoinCondition += @"[" + (column.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + column.DataBaseColumnName +
                            @"] = [" + (referenceColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName + @"].[" + referenceColumn.DataBaseColumnName + @"]";
                }

            }

            string updateQuery = @"UPDATE [" + mainTable.DataBaseTableName + @"]
                                SET [ReferenceCount] = [ReferenceCount] + (SELECT     COUNT(*)
                                FROM [" + referenceColumnTable.DataBaseTableName + @"]
                                WHERE (" + referenceCountTableInnerJoinCondition + @"))";

            return new List<string>() { updateQuery };
        }
        /// <MetaDataID>{48a47ab1-4d0e-4a54-8eea-f0df9f455d32}</MetaDataID>
        public List<string> BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd)
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
            return new List<string>() { updateQuery };
        }

        public List<string> GetDefineKeyScript(Key key)
        {
            if (key.IsPrimaryKey)
            {
                string keyColumnsScript = null;
                foreach (var column in key.Columns)
                {
                    if (keyColumnsScript != null)
                        keyColumnsScript += ",";
                    keyColumnsScript += "[" + column.Name + "]";
                }

                string script = string.Format(@"CONSTRAINT [sqlite_autoindex_{0}] PRIMARY KEY ({1})", key.Name, keyColumnsScript);
                return new List<string>() { script };
            }
            else
            {
                string keyColumnsScript = null;
                foreach (var column in key.Columns)
                {
                    if (keyColumnsScript != null)
                        keyColumnsScript += ",";
                    keyColumnsScript += "[" + column.Name + "]";
                }
                string keyReferenceColumnsScript = null;
                foreach (var column in key.ReferedColumns)
                {
                    if (keyReferenceColumnsScript != null)
                        keyReferenceColumnsScript += ",";
                    keyReferenceColumnsScript += "[" + column.Name + "]";
                }
                string script = string.Format("FOREIGN KEY ({0}) REFERENCES [{1}] ({2})", keyColumnsScript, key.ReferencedTable.Name, keyReferenceColumnsScript);
                return new List<string>() { script };
            }

        }


        public List<string> GetAddKeyScript(Key key)
        {
            return new List<string>();
            //if (key.IsPrimaryKey)
            //{
            //    string Script = "ALTER TABLE " + key.OwnerTable.Name + " ADD PRIMARY KEY \n" +
            //        key.Name + " PRIMARY KEY CLUSTERED ( ";
            //    string ColumnsScript = null;

            //    //System.Collections.Hashtable SColumns = new System.Collections.Hashtable();
            //    //foreach (Column CurrColumn in Columns)
            //    //    SColumns.Add(CurrColumn.ColumnType, CurrColumn);

            //    foreach (Column column in key.Columns)
            //    {
            //        if (ColumnsScript != null)
            //            ColumnsScript += ",";
            //        ColumnsScript += column.Name;
            //    }
            //    Script += ColumnsScript + ") ON [PRIMARY]";
            //    List<string> commands = new List<string>();
            //    commands.Add(Script);
            //    return commands;
            //}
            //else
            //{
            //    string Script = "ALTER TABLE " + key.OwnerTable.Name + " WITH NOCHECK ADD CONSTRAINT \n" +
            //        key.Name + " FOREIGN KEY ( ";
            //    string ColumnsScript = null;
            //    //System.Collections.Hashtable SColumns = new System.Collections.Hashtable();
            //    //foreach (Column CurrColumn in Columns)
            //    //    SColumns.Add(CurrColumn.ColumnType, CurrColumn);
            //    foreach (Column column in key.Columns)
            //    {
            //        if (ColumnsScript != null)
            //            ColumnsScript += ",";
            //        ColumnsScript += column.Name;
            //    }
            //    Script += ColumnsScript + " ) REFERENCES " + key.ReferencedTable.Name + " ( ";
            //    ColumnsScript = null;

            //    foreach (Column column in key.ReferedColumns)
            //    {
            //        if (ColumnsScript != null)
            //            ColumnsScript += ",";
            //        ColumnsScript += column.Name;
            //    }
            //    Script += ColumnsScript + " ) NOT FOR REPLICATION \n  ALTER TABLE " + key.OwnerTable.Name +
            //        " NOCHECK CONSTRAINT  " + key.Name;
            //    List<string> commands = new List<string>();
            //    commands.Add(Script);
            //    return commands;

            //}
        }
        public List<string> GetRenameScript(Key key, string newName)
        {
            return new List<string>();// { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'\n", key.Name, newName) };
        }
        public List<string> GetDropKeyScript(Key key)
        {
            return new List<string>() { "ALTER TABLE " + key.OwnerTable.Name + " DROP CONSTRAINT " + key.Name };
        }
        ///// <MetaDataID>{b90f441b-a1d3-445b-81ec-d083ae27f9ce}</MetaDataID>
        //public void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames)
        //{
        //    System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();
        //    DataBase dataBase = key.Namespace.Namespace as DataBase;
        //    System.Data.Common.DbConnection connection = dataBase.Connection;

        //    string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
        //    System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
        //    command.CommandText = CommandString;
        //    System.Data.Common.DbDataReader DataReader = command.ExecuteReader();
        //    foreach (System.Data.Common.DbDataRecord record in DataReader)
        //    {
        //        if (record["FK_NAME"].Equals(key.Name))
        //        {
        //            string PKColumnName = (string)record["PKCOLUMN_NAME"];
        //            string FKColumnName = (string)record["FKCOLUMN_NAME"];
        //            columnsNames.Add(FKColumnName);
        //            referedColumnsNames.Add(PKColumnName);


        //            //Column column = key.OwnerTable.GetColumn(FKColumnName);
        //            ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
        //            ////Το σωστό είναι  Πρέπει να είναι ordered
        //            ////if (column.ColumnType == null)
        //            ////    column.ColumnType = Columns.Count.ToString();

        //            //Columns.Add(column);

        //            //column = key.ReferencedTable.GetColumn(PKColumnName);
        //            ////TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
        //            ////Το σωστό είναι  Πρέπει να είναι ordered
        //            ////if (column.ColumnType == null)
        //            ////    column.ColumnType = ReferedColumns.Count.ToString();
        //            //ReferedColumns.Add(column);
        //            int kj = 0;

        //        }
        //    }
        //    DataReader.Close();
        //    //Connection.Close();
        //}
        ///// <MetaDataID>{4c23f490-42b0-4cc2-9174-657175bedb99}</MetaDataID>
        //public void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames)
        //{
        //    DataBase dataBase = key.Namespace.Namespace as DataBase;
        //    System.Data.Common.DbConnection connection = dataBase.Connection;

        //    //string CommandString = "exec sp_fkeys @pktable_name ='" + key.ReferencedTable.Name + "' ,@fktable_name = '" + key.OwnerTable.Name + "'";
        //    string CommandString = "exec sp_pkeys '" + key.OwnerTable.Name + "'";
        //    System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(CommandString, Connection);
        //    command.CommandText = CommandString;
        //    System.Data.Common.DbDataReader DataReader = command.ExecuteReader();
        //    foreach (System.Data.Common.DbDataRecord record in DataReader)
        //    {
        //        if (record["PK_NAME"].Equals(key.Name))
        //        {
        //            string PKColumnName = (string)record["COLUMN_NAME"];
        //            columnsNames.Add(PKColumnName);

        //        }
        //    }
        //    DataReader.Close();
        //    //Connection.Close();


        //}


        /// <MetaDataID>{5d87a1a0-b06e-4957-aefa-b425f58e0598}</MetaDataID>
        public System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName)
        {
            System.Collections.Generic.List<ColumnData> columnsData = new System.Collections.Generic.List<ColumnData>();

            var connection = Connection;

            try
            {
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();

                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_Columns " + Name, connection);
                command.CommandText = string.Format("pragma table_info({0})", tableName);
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    int size = 0;
                    string name = dataReader["name"] as string;
                    string type = dataReader["type"] as string;
                    bool notNull = (long)dataReader["notnull"] == 1;
                    object default_value = dataReader["dflt_value"];
                    bool primaryKey = (long)dataReader["pk"] == 1;
                    if (type.IndexOf("(") != -1)
                    {
                        string sizeStr = type.Substring(type.IndexOf("("));
                        sizeStr = sizeStr.Replace("(", "").Replace(")", "").Trim();
                        if (!int.TryParse(sizeStr, out size))
                            size = 0;
                        type = type.Substring(0, type.IndexOf("("));
                    }
                    ColumnData columnData = new ColumnData(name, type, size, !notNull, false, 0);
                    columnsData.Add(columnData);
                }
                dataReader.Close();

                //                command.CommandText = string.Format(@"Select IDENT_INCR from (SELECT IDENT_INCR(TABLE_NAME) AS IDENT_INCR 
                //								FROM INFORMATION_SCHEMA.TABLES 
                //								WHERE TABLE_NAME='{0}') IdentityData
                //                                where IdentityData.IDENT_INCR is not NULL", tableName);

                //                int identityIncrement = 0;
                //                var dataReader = command.ExecuteReader();
                //                while (dataReader.Read())
                //                {
                //                    identityIncrement = (int)System.Convert.ChangeType(dataReader["IDENT_INCR"], typeof(int)); ;
                //                    break;
                //                }
                //                dataReader.Close();



                //                command.CommandText = "exec sp_Columns " + tableName;
                //                dataReader = command.ExecuteReader();
                //                // Load table columns
                //                while (dataReader.Read())
                //                {
                //                    string dataType = (string)dataReader["TYPE_NAME"];
                //                    bool identityColumn = false;
                //                    if (dataType.IndexOf("identity") != -1)
                //                    {
                //                        identityColumn = true;
                //                        dataType = dataType.Replace("identity", "").Trim();
                //                    }
                //                    ColumnData columnData;
                //                    if (dataType.ToLower() == "nvarchar")
                //                        columnData = new ColumnData(dataReader["COLUMN_NAME"].ToString(), dataType, (int)dataReader["LENGTH"] / 2, (bool)System.Convert.ChangeType(dataReader["NULLABLE"], typeof(bool)), identityColumn, identityIncrement);
                //                    else
                //                        columnData = new ColumnData(dataReader["COLUMN_NAME"].ToString(), dataType, (int)dataReader["LENGTH"], (bool)System.Convert.ChangeType(dataReader["NULLABLE"], typeof(bool)), identityColumn, identityIncrement);

                //                    columnsData.Add(columnData);
                //                }
                //                dataReader.Close();
            }
            catch (System.Exception Error)
            {
                System.Diagnostics.Debug.Assert(false);
                throw new System.Exception("Table with name '" + tableName + "'failed to read its columns", Error);
            }
            return columnsData;

        }
        public PrimaryKeyData RetrieveDatabasePrimaryKeys(Table table)
        {

            var connection = (table.Namespace as DataBase).Connection;
            if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                connection.Open();
            var Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
            //Command.CommandText = "exec sp_pkeys " + table.Name;
            //            Command.CommandText = string.Format(@"Select s.name As TableSchema,
            //                            PKTable.name As PK_TABLE_NAME,
            //                            i.name As PK_NAME,
            //                            c.name as COLUMN_NAME,
            //                            ic.key_ordinal  COLUMN_ORDER
            //                            From sys.objects PKTable
            //                            Inner Join sys.schemas s On PKTable.schema_id = s.schema_id
            //                            Inner Join sys.indexes i On PKTable.object_id = i.object_id And i.is_primary_key = 1
            //                            Inner Join sys.index_columns ic On i.object_id = ic.object_id And i.index_id = ic.index_id
            //                            inner join sys.columns c on c.object_id = PKTable.object_id and 
            //                            ic.column_id = c.column_id 
            //                            where i.is_primary_key=1 and  PKTable.name='{0}'
            //                            order by i.object_id , COLUMN_ORDER", table.Name);

            Command.CommandText = string.Format("SELECT name as PK_NAME,tbl_name as PK_TABLE_NAME  FROM sqlite_master where type=='index' and tbl_name = '{0}'", table.Name);

            var dataReader = Command.ExecuteReader();
            // Load table Primary Keys
            PrimaryKeyData pkeyData = null;
            while (dataReader.Read())
            {

                pkeyData = new PrimaryKeyData((string)dataReader["PK_NAME"], (string)dataReader["PK_TABLE_NAME"]);
                break;
            }
            dataReader.Close();
            Command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
            Command.CommandText = string.Format("pragma table_info({0})", table.Name);
            dataReader = Command.ExecuteReader();
            while (dataReader.Read())
            {
                long pk =(long)dataReader["pk"];
                string name = dataReader["name"] as string;
                if (pk == 1)
                    pkeyData.ColumnsNames.Add(name);
                
            }





            //pkeyData.ColumnsNames.Add((string)dataReader["COLUMN_NAME"]);
            //foreach (System.Data.Common.DbDataRecord CurrRecord in dataReader)
            //    primaryKeys.Add((string)CurrRecord["PK_NAME"]);
            dataReader.Close();
            return pkeyData;
        }
        public System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table)
        {
            var connection = (table.Namespace as DataBase).Connection;
            if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                connection.Open();
            var Command = connection.CreateCommand();
            //            Command.CommandText = @"SELECT     F_Keys.name AS FK_NAME, FK_Tables.name AS FK_TABLE_NAME, REF_Tables.name AS REF_TABLE_NAME 
            //										FROM         sysobjects FK_Tables INNER JOIN 
            //										sysreferences r INNER JOIN 
            //										sysobjects F_Keys ON r.constid = F_Keys.id ON FK_Tables.id = r.fkeyid INNER JOIN 
            //										sysobjects REF_Tables ON r.rkeyid = REF_Tables.id 
            //										WHERE     (F_Keys.xtype = 'F') AND (FK_Tables.name = N'" + table.Name + "')";
//            Command.CommandText = @"SELECT FKey.name AS FK_NAME,  FK_Tables.name AS FK_TABLE_NAME, COLUMS.name AS COLUMN_NAME, REF_Tables.name AS REF_TABLE_NAME, REF_COLUMS.name AS REF_COLUMN_NAME, ic.key_ordinal as ColumnOrder
//                                    FROM sys.foreign_keys as FKey 
//                                    INNER JOIN sys.foreign_key_columns on FKey.object_id = sys.foreign_key_columns.constraint_object_id
//                                    INNER JOIN sys.columns AS COLUMS ON sys.foreign_key_columns.parent_object_id = COLUMS.object_id AND sys.foreign_key_columns.parent_column_id = COLUMS.column_id
//                                    INNER JOIN sys.columns AS REF_COLUMS ON sys.foreign_key_columns.referenced_object_id = REF_COLUMS.object_id AND sys.foreign_key_columns.referenced_column_id = REF_COLUMS.column_id
//                                    INNER JOIN sys.tables AS FK_Tables ON sys.foreign_key_columns.parent_object_id = FK_Tables.object_id
//                                    INNER JOIN sys.tables AS REF_Tables ON sys.foreign_key_columns.referenced_object_id = REF_Tables.object_id
//                                    INNER JOIN sys.indexes i on (FKey.referenced_object_id = i.object_id and FKey.key_index_id = i.index_id)
//                                    Inner Join sys.index_columns ic On i.object_id = ic.object_id And i.index_id = ic.index_id and REF_COLUMS.column_id=ic.column_id
//                                    WHERE (FK_Tables.name =N'" + table.Name + "') " + "order by FKey.object_id , ColumnOrder";
            
            Command.CommandText =string.Format( @"SELECT *
                                    FROM (SELECT  type type, tbl_name tbl_name, name name,sql sql
                                        FROM sqlite_master
                                        UNION ALL
                                    SELECT sql, type, tbl_name, name
                                        FROM sqlite_temp_master)
                                    WHERE type != 'meta'
                                    AND tbl_name = '{0}'
                                    AND sql NOTNULL
                                AND name NOT LIKE 'sqlite_%'
                                AND sql LIKE '%REFERENCES%'", table.Name);

            var dataReader = Command.ExecuteReader();




            System.Collections.Generic.Dictionary<string, ForeignKeyData> foreignKeysData = new Dictionary<string, ForeignKeyData>();
            List<string> foreignKeysScripts = new List<string>();
            while (dataReader.Read())
            {
                string sql = dataReader["sql"] as string;

                Parser.Parse(sql);
               // Parser.theRoot.
                int i = 0;
                foreach (Parser.ParserNode tableDefinition in (Parser.theRoot["CreateTableStatement"]["TableDefinitionGroup"] as Parser.ParserNode).ChildNodes)
                {
                    var tableConstraint = tableDefinition["TableConstraint"];

                    if (tableConstraint!=null&&tableConstraint["ForeignKey"] != null)
                    {
                        var foreignKey = tableConstraint["ForeignKey"];
                        string referenceTable = (foreignKey["ReferenceTable"] as Parser.ParserNode).Value;
                        referenceTable = referenceTable.Replace("[", "").Replace("]", "");
                        i++;
                        ForeignKeyData fkeyData = new ForeignKeyData("FK_" + table.Name + "_" + referenceTable + "_" + i.ToString(), table.Name, referenceTable);

                        foreach (var tableColumn in (foreignKey["TableColumns"] as Parser.ParserNode).ChildNodes)
                            fkeyData.ColumnsNames.Add((tableColumn as Parser.ParserNode).Value.Replace("[", "").Replace("]", ""));

                        foreach (var tableColumn in (foreignKey["ReferenceTableColumns"] as Parser.ParserNode).ChildNodes)
                            fkeyData.ReferedColumnsNames.Add((tableColumn as Parser.ParserNode).Value.Replace("[", "").Replace("]", ""));

                        foreignKeysData[fkeyData.Name] = fkeyData;
                    }
                }
           }

            dataReader.Close();
            return new List<ForeignKeyData>(foreignKeysData.Values);

        }
        /// <MetaDataID>{404dbacf-6212-4523-821f-db6c47dfcc29}</MetaDataID>
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

            Table table = newColumns[0].Namespace as Table;

            if (newTable)
            {

                if (tableType == TableType.VaraibleTable)
                    script = "CREATE TABLE @" + tableName + " (\n" + columnsScript + ")";
                else if (tableType == TableType.TemporaryTable)
                    script = "CREATE TABLE #" + tableName + " (\n" + columnsScript + ")";
                else
                {
                    string keyScripts = null;
                    foreach (Key key in table.Keys)
                    {
                        foreach (string defineKeyScript in GetDefineKeyScript(key))
                            keyScripts += "," + defineKeyScript;
                    }
                    script = "CREATE TABLE " + tableName + " (\n" + columnsScript + keyScripts + ")";

                }
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

            return new List<string>() { script };
        }
        /// <MetaDataID>{29cc435b-e394-4a22-b972-0170809d7953}</MetaDataID>
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

        /// <MetaDataID>{7cb2a8aa-8ea8-4d1b-950d-545ea3f6493c}</MetaDataID>
        static System.Collections.Generic.List<string> ExcludedConvertion = new List<string>();
        /// <MetaDataID>{01332971-cee9-4b76-b970-f812625b2d8f}</MetaDataID>
        static SQLiteRDBMSSchema()
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
                        destColumns += "[" + column.Name + "]";
                        if (column.HasChangeColumnFormat)
                            sourceColumns += GetDataTypeCoversionStatement(column.Name, column.Length, column.DataBaseColumnDataType, column.Datatype);
                        else
                            sourceColumns += "[" + column.DataBaseColumnName + "]";
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
        public List<string> GetRenameScript(string tableName, string newTableName)
        {
            return new List<string>() { string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'", tableName, newTableName) };
        }
        public string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName)
        {
            return string.Format("EXECUTE sp_rename N'{0}.{1}', N'{2}', 'COLUMN'", tableName, columnName, newColumnName);
        }
        /// <MetaDataID>{f2ce01c7-0d96-4700-bd7b-965e6d70134f}</MetaDataID>
        public void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns)
        {
            string dropColumnsScript = null;
            foreach (Column column in dropedColumns)
            {
                if (dropColumnsScript == null)
                    dropColumnsScript = string.Format("ALTER TABLE [{0}] \nDROP COLUMN ", tableName);
                else
                    dropColumnsScript += ",";
                dropColumnsScript += "[" + column.Name + "]";
            }
            if (dropColumnsScript != null)
            {
                var command = Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropColumnsScript, connection);
                command.CommandText = dropColumnsScript;
                command.ExecuteNonQuery();
            }
        }
        public List<string> GetViewDDLScript(RDBMSMetaDataRepository.View view, bool newView)
        {
            string AbstractClassView = "";

            if (newView)
                AbstractClassView += "CREATE VIEW ";
            else
                AbstractClassView += "ALTER VIEW ";
            AbstractClassView += "[" + view.Name + "]\nAS\n";
            string SelectList = null;
            SelectList = "Select ";
            string Separator = null;
            bool typeWithIDDefaultValue = false;
            foreach (RDBMSMetaDataRepository.Column column in view.ViewColumns)
            {
                if (column.Name == "TypeID" && !string.IsNullOrEmpty(column.DefaultValue))
                    typeWithIDDefaultValue = true;
                column.DataBaseColumnName = GetValidateColumnName(column, view);
                if ((column.DefaultValue != null && column.DefaultValue.Length > 0) && column.RealColumn == null)
                {
                    SelectList += Separator + column.DefaultValue + " AS [" + column.Name + "]";
                    if (Separator == null)
                        Separator = ",";
                }
                else
                {
                    if (view.JoinedTables.Count > 0)
                    {
                        //TODO αυτό γίνεται γιατί κάπιος για κάποιο λόγο σβήνεται η column από το table
                        //θα πρέπει να σβήνουμε τα reference columns όταν σβήνεται ένα table column
                        if (column.RealColumn == null)
                        {
                            column.DefaultValue = DataBase.TypeDictionary.GetDBNullScript(column.Type.FullName);
                            SelectList += Separator + column.DefaultValue + " AS [" + column.Name + "]";
                            if (Separator == null)
                                Separator = ",";
                        }
                        else
                        {
                            if (column.RealColumn.Namespace != null)
                                SelectList += Separator + "[" + column.RealColumn.Namespace.Name + "].[" + column.RealColumn.DataBaseColumnName + "]";
                            else
                                SelectList += Separator + column.RealColumn.DataBaseColumnName;

                            SelectList += " as [" + column.Name + "]";
                            if (Separator == null)
                                Separator = ",";
                        }
                    }
                    else
                    {
                        SelectList += Separator + "[" + column.Name + "]";
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
                foreach (RDBMSMetaDataRepository.Column column in view.ViewColumns)
                {
                    column.DataBaseColumnName = GetValidateColumnName(column, view);
                    bool exist = false;
                    foreach (string ColumnName in CurrView.ViewColumnsNames)
                    {
                        if (ColumnName.ToLower().Trim() == column.Name.ToLower().Trim())
                        {
                            ViewQuery += Separator + "[" + column.Name + "]";
                            if (Separator == null)
                                Separator = ",";
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        string nullValue = "NULL";
                        if (column.Type != null)
                            nullValue = DataBase.TypeDictionary.GetDBNullScript(column.Type.FullName);

                        ViewQuery += Separator + nullValue + " AS [" + column.Name + "]";
                        if (Separator == null)
                            Separator = ",";
                    }
                }
                ViewQuery += "\nFrom [" + CurrView.Name + "]";

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
                        whereClause = "WHERE " + "[" + column.Name + "] <> " + nullValue;
                        ViewQuery += "SELECT " + nullValue + " AS " + column.Name;
                    }
                    else
                        ViewQuery += "," + nullValue + " AS " + column.Name;
                }
                ViewQuery = SelectList + " FROM (" + ViewQuery + ") [TABLE] " + whereClause;
            }

            AbstractClassView +=/*"\n"+SelectList+"\n"*/ViewQuery;

            return new List<string>() { AbstractClassView };
        }

#region Builds Store Procedures
        public string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create)
        {
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
        /// <MetaDataID>{34016814-a500-445e-b6f6-c84d79429c6c}</MetaDataID>
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
        /// <MetaDataID>{f5f4a896-2598-4aa7-9a2d-17e186569425}</MetaDataID>
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
            System.Collections.Generic.Dictionary<string,MetaDataRepository.Parameter> ClassAttriutes = new Dictionary<string, MetaDataRepository.Parameter>();

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


                    if (ClassAttriutes.ContainsKey(parameterName))
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
                if (ClassAttriutes.ContainsKey(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name))
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
                        if (ClassAttriutes.ContainsKey(CurrColumn.MappedAttribute.Name))
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
                foreach (RDBMSMetaDataRepository.IdentityColumn Column in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(storeProcedure).Parts)
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
        /// <MetaDataID>{57d0bd32-88a5-4314-9c70-9689180aee41}</MetaDataID>
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
            System.Collections.Generic.Dictionary<string,MetaDataRepository.Parameter > ClassAttriutes = new Dictionary<string, MetaDataRepository.Parameter>();

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

                    if (ClassAttriutes.ContainsKey(parameterName))
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






        /// <MetaDataID>{33336233-74b6-4097-a6a5-3b58ce8c1f25}</MetaDataID>
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
                                SET [" + IndexerColumn.DataBaseColumnName + @"] = [" + IndexerColumn.DataBaseColumnName + @"] + " + change.ToString()
                + IndexCriterion + @" AND [" + IndexerColumn.DataBaseColumnName + @"] >= " + startIndex.ToString()
                + @" AND [" + IndexerColumn.DataBaseColumnName + @"] <= " + endIndex.ToString() + " \n";
            }

            return indexUpdate;
        }

        public string IndexOfSqlScript(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationOwner, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent assignedObject, OOAdvantech.RDBMSMetaDataRepository.Table keepColumnTable, IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> ownerObjectIDColumns, IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> indexOfObjectIDColumns, OOAdvantech.RDBMSMetaDataRepository.Column IndexerColumn)
        {

            string indexOfSelectionScript = string.Format("SELECT [{0}].[{1}] ", keepColumnTable.DataBaseTableName, IndexerColumn.DataBaseColumnName);
            string fromScript = string.Format("FROM [{0}] ", keepColumnTable.DataBaseTableName);
            string indexCriterion = null;



            foreach (RDBMSMetaDataRepository.IdentityColumn column in ownerObjectIDColumns)
            {
                if (string.IsNullOrEmpty(indexCriterion))
                    indexCriterion = "WHERE ";
                else
                    indexCriterion += " AND ";
                indexCriterion += @"[" + column.DataBaseColumnName + @"] = " + DataBase.TypeDictionary.ConvertToSQLString(assignedObject.ObjectID.GetMemberValue(column.ColumnType));
            }
            return indexOfSelectionScript + fromScript + indexCriterion;
        }
















    }
}
