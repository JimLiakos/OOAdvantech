using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSDataObjects
{

    /// <MetaDataID>{8da4d5b9-4bcb-42ce-a380-a61616eeb350}</MetaDataID>
    public interface IRDBMSSchema
    {
        /// <MetaDataID>{16d94b3e-5da9-4c60-a5cb-5f5be3606b7f}</MetaDataID>
        System.Collections.Generic.List<string> GetStoreProcedureNames();
        /// <MetaDataID>{defff056-b031-4092-aa99-f51e56512ce0}</MetaDataID>
        System.Collections.Generic.List<string> GetViewsNames();
        /// <MetaDataID>{a36430c2-3ae3-4997-a65d-de8a6474b324}</MetaDataID>
        System.Collections.Generic.List<string> GetTablesNames();
        string CreateManyToManyTransferDataSQLStatement(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, RDBMSMetaDataRepository.Table relationTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns);
        string CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns);
        string BuildManyManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, RDBMSMetaDataRepository.Table referenceTable);
        string BuildManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, MetaDataRepository.AssociationEnd associationEnd);
        /// <MetaDataID>{052a99d4-35d4-47dc-b4e2-0400188a4183}</MetaDataID>
        string BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd);
        /// <MetaDataID>{007275c2-5e99-440e-ad70-092cd9f8a694}</MetaDataID>
        List<string> GetAddKeyScript(Key key);
        /// <MetaDataID>{2370fd5f-6c0c-4d3d-ae06-8ad048871383}</MetaDataID>
        string GetRenameScript(Key key, string newName);
        /// <MetaDataID>{7022cd79-eacc-45fa-b09e-e43e4a2c0132}</MetaDataID>
        string GetDropKeyScript(Key key);
        void ReadForeignKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames, System.Collections.Generic.List<string> referedColumnsNames);
        void ReadPrimaryKeyColumns(Key key, System.Collections.Generic.List<string> columnsNames);
        /// <MetaDataID>{d469d329-38ab-44da-86fd-42335aa35379}</MetaDataID>
        System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName);
        /// <MetaDataID>{ea32c9af-d755-46b3-b02d-0493f0171de0}</MetaDataID>
        System.Collections.Generic.List<string> RetrieveDatabasePrimaryKeys(Table table);
        /// <MetaDataID>{771bc5bf-e156-4f53-8bab-d3c68ff5fc3a}</MetaDataID>
        System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table);
        string GetDataBaseUpdateScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType);
        /// <MetaDataID>{b6ca0b9d-e24b-4470-bc7a-a757ff21db6a}</MetaDataID>
        void UpdateColumnsFormat(Table table);
        /// <MetaDataID>{79aae812-1fda-44a0-b15e-c10ec156ec72}</MetaDataID>
        string GetChangeTableNameScript(string tableName, string newTableName);
        /// <MetaDataID>{9dcf680c-3743-4061-bc18-58ef1cf1a365}</MetaDataID>
        string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName);
        void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns);
        /// <MetaDataID>{e6a9ed15-1b6b-41ab-b422-f5f61e92a51b}</MetaDataID>
        string GetDefinition(RDBMSMetaDataRepository.View view, bool newView);
        /// <MetaDataID>{d4a5ebdc-a2da-4a03-b5cf-8189da298806}</MetaDataID>
        string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create);
        string BuildCountItemsInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns);
        string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn,int startIndex,int endIndex,int change);

        /// <MetaDataID>{220d3b8f-7ec3-4abc-b33a-4e1925ea5990}</MetaDataID>
        string ConvertToSQLString(object p);

        /// <MetaDataID>{184993ee-43f3-44e5-9c5a-f1295e6db167}</MetaDataID>
        int GeDefaultLength(string ColumnDotNetType);

        /// <MetaDataID>{6f1b83d7-e348-449a-a96b-4feeb2b60365}</MetaDataID>
        string GetDBType(string ColumnDotNetType, bool p);

        /// <MetaDataID>{68045627-7be9-4bb1-aa86-2f583cc68600}</MetaDataID>
        bool IsTypeVarLength(string ColumnDotNetType);

        void ValidateColumnName(Column column);
    }
}
