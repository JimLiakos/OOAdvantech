using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
    
    public interface IRDBMSSchema
    {
        System.Collections.Generic.List<string> GetStoreProcedureNames();
        System.Collections.Generic.List<string> GetViewsNames();
        System.Collections.Generic.List<string> GetTablesNames();
        string CreateManyToManyTransferDataSQLStatement(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, RDBMSMetaDataRepository.Table relationTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns);
        string CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns);
        string BuildManyManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, RDBMSMetaDataRepository.Table referenceTable);
        string BuildManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, MetaDataRepository.AssociationEnd associationEnd);
        string BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd);
        string GetAddKeyScript(Key key);
        string GetRenameScript(Key key, string newName);
        string GetDropKeyScript(Key key);
        void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames);
        void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames);
        System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName);
        System.Collections.Generic.List<string> RetrieveDatabasePrimaryKeys(Table table);
        System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table);
        string GetDataBaseUpdateScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType);
        void UpdateColumnsFormat(Table table);
        string GetChangeTableNameScript(string tableName, string newTableName);
        string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName);
        void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns);
        string GetDefinition(RDBMSMetaDataRepository.View view, bool newView);
        string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create);

    }
}
