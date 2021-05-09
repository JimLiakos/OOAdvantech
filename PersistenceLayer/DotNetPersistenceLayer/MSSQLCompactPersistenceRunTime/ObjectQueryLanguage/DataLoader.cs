using System;
using System.Linq;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.ObjectQueryLanguage
{
    using MetaDataRepository.ObjectQueryLanguage;

    /// <MetaDataID>{6eba7d78-d67e-4bb4-aa30-3b0b46cbf958}</MetaDataID>
    public class DataLoader : RDBMSPersistenceRunTime.DataLoader
    {

        //protected override PersistenceLayer.ObjectID GetTemporaryObjectID()
        //{
        //    return new OOAdvantech.RDBMSPersistenceRunTime.ObjectID(System.Guid.NewGuid(), 0);
        //}

        /// <MetaDataID>{fa9bfd82-7aee-4c76-be72-0f16b70705fc}</MetaDataID>
        protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        {
            if (DataNode.Type == DataNode.DataNodeType.Group)
                return true;
            else
                return false;
        }
        protected override string GetStorageCellDataSource(OOAdvantech.RDBMSMetaDataRepository.StorageCell storageCell)
        {
            return storageCell.MainTable.Name ;
        }


        /// <MetaDataID>{BA7812A0-A136-42ED-8E06-1D388CAC28DA}</MetaDataID>
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
        }
        protected override string BuildStorageCellDataRetriveSQL(System.Collections.Generic.List<DataColumn> columns, OOAdvantech.RDBMSMetaDataRepository.StorageCell storageCell)
        {
            string selectClause = null;

            //			When data node is type object and participates in select clause of query then 
            //			the procedure of construction UNION TABLES must be allocate columns for objects 
            //			with maximum number of columns.
            //			For object that belong to the class in class hierarchy with less columns, 
            //			we will add extra columns which missed with null values. 
            //			This happen because in the union table statement all select must be 
            //			contain the same number and same type of columns
            System.Collections.Generic.List<RDBMSMetaDataRepository.Column> tableColumns = new System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column>(storageCell.MainTable.ContainedColumns);

            foreach (RDBMSMetaDataRepository.Column column in storageCell.MainTable.ObjectIDColumns)
                tableColumns.Add(column);

            foreach (DataColumn dataLoaderColumn in columns)
            {

                if (selectClause == null)
                    selectClause = "SELECT ";
                else
                    selectClause += " , ";
                bool columnExist = false;


                foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in tableColumns)
                {
                    if (dataLoaderColumn.MappedObject == column.MappedAttribute && dataLoaderColumn.MappedObject != null && dataLoaderColumn.CreatorIdentity == column.CreatorIdentity)
                    {
                        string aliasName = dataLoaderColumn.Alias;
                        // if (string.IsNullOrEmpty(aliasName))
                        aliasName = dataLoaderColumn.Name;
                        selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                        columnExist = true;
                        break;
                    }
                    if (dataLoaderColumn.MappedObject == column.MappedAssociationEnd && dataLoaderColumn.MappedObject != null && dataLoaderColumn.IdentityPart != null &&
                        (column is MetaDataRepository.IIdentityPart) &&
                        dataLoaderColumn.CreatorIdentity == column.CreatorIdentity &&
                        dataLoaderColumn.IdentityPart.PartTypeName == (column as MetaDataRepository.IIdentityPart).PartTypeName &&
                        dataLoaderColumn.IdentityPart.Type.FullName == (column as MetaDataRepository.IIdentityPart).Type.FullName)
                    {
                        string aliasName = dataLoaderColumn.Name;
                        //if (string.IsNullOrEmpty(aliasName))
                        //    aliasName = columnName.Name;
                        selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                        columnExist = true;

                        break;
                    }

                    if (dataLoaderColumn.MappedObject is RDBMSMetaDataRepository.AssociationEnd &&
                        (dataLoaderColumn.MappedObject as RDBMSMetaDataRepository.AssociationEnd).Indexer && column != null)
                    {
                        string associationEndPathIdentity = column.CreatorIdentity;
                        if (associationEndPathIdentity != null && !string.IsNullOrEmpty(associationEndPathIdentity.Trim()))
                        {
                            MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath(associationEndPathIdentity);
                            valueTypePath.Pop();
                            associationEndPathIdentity = valueTypePath.ToString();
                        }
                        if (!dataLoaderColumn.IdentityColumn &&
                        (dataLoaderColumn.MappedObject as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCell, associationEndPathIdentity) == column)
                        {
                            string aliasName = dataLoaderColumn.Name;
                            //if (string.IsNullOrEmpty(aliasName))
                            //    aliasName = columnName.Name;
                            selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                            columnExist = true;
                            break;
                        }
                    }
                    if (dataLoaderColumn.MappedObject == null)
                    {
                        if (!dataLoaderColumn.IdentityColumn && dataLoaderColumn.Name == column.Name)
                        {
                            string aliasName = dataLoaderColumn.Name;
                            //if (string.IsNullOrEmpty(aliasName))
                            //    aliasName = columnName.Name;

                            selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                            columnExist = true;
                            break;
                        }
                        else
                            if (dataLoaderColumn.IdentityColumn &&
                                //                                     columnName.Name == column.Name&&
                                column != null &&
                                column.MappedAssociationEnd == null &&
                                column is MetaDataRepository.IIdentityPart &&
                                dataLoaderColumn.IdentityPart.PartTypeName == (column as MetaDataRepository.IIdentityPart).PartTypeName &&
                                dataLoaderColumn.IdentityPart.Type == (column as MetaDataRepository.IIdentityPart).Type)
                            {
                                string aliasName = dataLoaderColumn.Name;
                                //if (string.IsNullOrEmpty(aliasName))
                                //    aliasName = columnName.Name;
                                selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                                columnExist = true;
                                break;
                            }

                    }
                }
                if(dataLoaderColumn.Name=="StorageCellID")
                {
                    selectClause += storageCell.SerialNumber.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(dataLoaderColumn.Name);
                    columnExist = true;
                }
                if (!columnExist)
                {
                    string defaultValue = (ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary.GetDBNullScript(dataLoaderColumn.Type.FullName);// .GetDBDefaultValue(dataLoaderColumn.Type.FullName) as string;
                    selectClause += defaultValue + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(dataLoaderColumn.Name);
                }
            }
            string fromClause = "\r\nFROM " + storageCell.MainTable.Name;
            return selectClause + fromClause;

           //string sqlScript=  base.BuildStorageCellDataRetriveSQL(columns, storageCell);
           //return sqlScript;

        }


    }
}
