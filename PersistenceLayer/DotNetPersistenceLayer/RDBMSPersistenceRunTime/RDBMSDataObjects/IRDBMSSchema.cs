using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSDataObjects
{

    /// <MetaDataID>{8da4d5b9-4bcb-42ce-a380-a61616eeb350}</MetaDataID>
    public interface IRDBMSSQLScriptGenarator
    {
        /// <MetaDataID>{bc4899ae-717c-4513-b457-d11710dba849}</MetaDataID>
        string GetColumnsDefaultValuesSetScript(string tableName, System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues);

        /// <MetaDataID>{fafefac7-de16-46a5-a191-2f3fa714c6cd}</MetaDataID>
        string AliasDefSqlScript
        {
            get;
        }
        /// <MetaDataID>{45cf7062-5baa-4d21-abb1-d1b25fbf6ad3}</MetaDataID>
        bool SupportBatchSQL
        {
            get;
        }
        /// <MetaDataID>{341aed27-1fa1-45be-a1a5-2c8654e10e8d}</MetaDataID>
        /// <summary>
        /// Returns a valid SQL script for name according the RDBMS (oracle,slq server etc)
        /// </summary>
        /// <param name="rdbmsFriendlyNamesDictionary">
        /// Defines a dictionary with the rdbms friedlly names as key and original name as value
        /// </param>
        /// <param name="objectLifeTimeManagerNamesDictionary">
        /// Defines a dictionary with the object lifetime manager names as key and valid for RDBMS name as value
        /// </param>
        /// <param name="objectLifeTimeManagerName">Defines the name where the operation must produce valid alias SQL script </param>
        /// <returns>Valid SQL script for object lifetime manager name </returns>
       // string GetSQLScriptValidAlias(Dictionary<string, string> rdbmsFriendlyNamesDictionary, Dictionary<string, string> objectLifeTimeManagerNamesDictionary, string objectLifeTimeManagerName);
      
        /// <MetaDataID>{f04ada0d-1dc7-44ed-acc9-dd9bd5f91572}</MetaDataID>
        /// <summary>
        /// Returns the sql script for name
        /// </summary>
        /// <param name="name">Defines the name which used from operation to produce sql script name</param>
        /// <param name="objectLifeTimeManagerNamesDictionary"> 
        /// /// Defines a dictionary with the object lifetime manager names as key and valid for RDBMS name as value
        /// </param>
       // string GetSQLScriptForName(string name, Dictionary<string, string> objectLifeTimeManagerNamesDictionary);


        string GetSQLScriptForName(string name);
        /// <MetaDataID>{16d94b3e-5da9-4c60-a5cb-5f5be3606b7f}</MetaDataID>
        System.Collections.Generic.List<string> GetStoreProcedureNames();
        /// <MetaDataID>{185df881-87c2-48a9-a544-160f0cc9bd7e}</MetaDataID>
        System.Collections.Generic.Dictionary<string,string> GetViewsNamesAndDefinition();
        /// <MetaDataID>{a36430c2-3ae3-4997-a65d-de8a6474b324}</MetaDataID>
        System.Collections.Generic.List<string> GetTablesNames();

        List<string> GetDefineKeyScript(Key key);

        /// <MetaDataID>{007275c2-5e99-440e-ad70-092cd9f8a694}</MetaDataID>
        List<string> GetAddKeyScript(Key key);
        /// <MetaDataID>{2370fd5f-6c0c-4d3d-ae06-8ad048871383}</MetaDataID>
        List<string> GetRenameScript(Key key, string newName);
        /// <MetaDataID>{7022cd79-eacc-45fa-b09e-e43e4a2c0132}</MetaDataID>
        List<string> GetDropKeyScript(Key key);
        /// <MetaDataID>{79aae812-1fda-44a0-b15e-c10ec156ec72}</MetaDataID>
        List<string> GetRenameScript(string tableName, string newTableName);
        /// <MetaDataID>{6231ac79-1365-4c7f-a581-2df5eac13e31}</MetaDataID>
        List<string> GetTableDDLScript(string tableName, System.Collections.Generic.List<Column> newColumns, bool newTable, TableType tableType);
        /// <MetaDataID>{f480e911-b2ec-444f-8e84-1a534438851a}</MetaDataID>
        List<string> GetDropTableScript(string tableName, TableType tableType);
        /// <MetaDataID>{e6a9ed15-1b6b-41ab-b422-f5f61e92a51b}</MetaDataID>
        List<string> GetViewDDLScript(RDBMSMetaDataRepository.View view, bool newView);
        /// <MetaDataID>{ffb92f4f-0096-4ea3-8103-738a9a385bfd}</MetaDataID>
        string GetColumDefinitionScript(Column column);
        /// <MetaDataID>{b6ca0b9d-e24b-4470-bc7a-a757ff21db6a}</MetaDataID>
        void UpdateColumnsFormat(Table table);
        /// <MetaDataID>{9dcf680c-3743-4061-bc18-58ef1cf1a365}</MetaDataID>
        string GetChangeColumnNameScript(string tableName, string columnName, string newColumnName);
        /// <MetaDataID>{e6e2fda5-d34a-469e-9dc4-6c4f0d3e0773}</MetaDataID>
        void DropColumns(string tableName, System.Collections.Generic.List<Column> dropedColumns);


           /// <MetaDataID>{f7fdda38-2697-4964-999f-802c0aab2bbd}</MetaDataID>
        List<string> CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns);
        /// <MetaDataID>{508b3939-c4b9-4430-9e35-f6a11115ce84}</MetaDataID>
        List<string> BuildManyManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> mainTableAssociationColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceTableAssociationColumns, RDBMSMetaDataRepository.Table referenceTable);
        /// <MetaDataID>{c13bdc44-9613-459a-a3f6-8f4ca4ead947}</MetaDataID>
        List<string> BuildManyRelationshipReferenceCountCommand(RDBMSMetaDataRepository.Table mainTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns, MetaDataRepository.AssociationEnd associationEnd);
        /// <MetaDataID>{052a99d4-35d4-47dc-b4e2-0400188a4183}</MetaDataID>
        List<string> BuildOneRelationshipReferenceCountCommand(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd);
        /// <MetaDataID>{ba1d844b-a4fa-4277-b7c0-b9ab63685fdf}</MetaDataID>
        string RebuildItemsIndexInTableStatament(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef assignedObject, RDBMSMetaDataRepository.Table KeepColumnTable, System. Collections.Generic.IList<RDBMSMetaDataRepository.IdentityColumn> linkColumns, RDBMSMetaDataRepository.Column IndexerColumn, int startIndex, int endIndex, int change);

        /// <MetaDataID>{ba1d844b-a4fa-4277-b7c0-b9ab63685fdf}</MetaDataID>
        string IndexOfSqlScript(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationOwner, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent assignedObject, RDBMSMetaDataRepository.Table keepColumnTable, System.Collections.Generic.IList<RDBMSMetaDataRepository.IdentityColumn> ownerObjectIDColumns, System.Collections.Generic.IList<RDBMSMetaDataRepository.IdentityColumn> indexOfObjectIDColumns, RDBMSMetaDataRepository.Column IndexerColumn);


 
        /// <MetaDataID>{d469d329-38ab-44da-86fd-42335aa35379}</MetaDataID>
        System.Collections.Generic.List<ColumnData> RetreiveTableColumns(string tableName);
        /// <MetaDataID>{ea32c9af-d755-46b3-b02d-0493f0171de0}</MetaDataID>
        PrimaryKeyData RetrieveDatabasePrimaryKeys(Table table);
        /// <MetaDataID>{771bc5bf-e156-4f53-8bab-d3c68ff5fc3a}</MetaDataID>
        System.Collections.Generic.List<ForeignKeyData> RetrieveDatabaseForeignKeys(Table table);
        /// <MetaDataID>{d4a5ebdc-a2da-4a03-b5cf-8189da298806}</MetaDataID>
        string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure, bool create);


        /// <MetaDataID>{5b339905-090e-4ede-aa43-908d3dd33da9}</MetaDataID>
        string GeValidRDBMSName(string name, System.Collections.Generic.List<string> excludedValidNames);

        /// <MetaDataID>{faecb3cb-0719-4ff3-9fe4-e0c69aa84314}</MetaDataID>
        bool GetValidateColumnName(Column column);
        /// <MetaDataID>{cc477482-b8be-4583-9337-18a758151893}</MetaDataID>
        bool GetValidateTableName(Table table);
        /// <MetaDataID>{930b8002-2938-4d35-973d-c4700e3d3544}</MetaDataID>
        bool GetValidateKeyName(Key key);
        /// <MetaDataID>{30b65bf4-6477-418a-a6f5-bece0b0162b8}</MetaDataID>
        bool GetValidateViewName(View view);
        /// <MetaDataID>{0e0b598c-192c-4bc2-9a61-234c08cc525d}</MetaDataID>
        bool GetValidateStoreprocedureName(StoreProcedure storeProcedure);

        /// <MetaDataID>{6cac8084-a4cb-4632-ae5e-31e01c85d33d}</MetaDataID>
        string GetTemporaryTableNameScript(string tableName);

        /// <MetaDataID>{918147d4-3481-4683-8813-9a1bfe6bf5a1}</MetaDataID>
        string GetRowRemoveCaseScript(string filterString, string alias);

        /// <MetaDataID>{cca71d62-ffb2-44e4-9a16-8d3dcf53f30e}</MetaDataID>
        string GetDatePartSqlScript(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode);

        /// <MetaDataID>{1f15d271-a043-408b-b757-fc51dc311a10}</MetaDataID>
        string GhostTable
        {
            get;
        }


        /// <MetaDataID>{2c4d7775-c236-492a-8399-39b42d8a534a}</MetaDataID>
        bool CriterionResolvedFromNativeSystem(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion criterion);

        /// <summary>
        /// There are RDBMS which supports addendum constraint key on create table
        /// and other RDBMS which not support
        /// This property is true when RDBMS supports addendum
        /// </summary>
        bool SupportAddRemoveKeys { get; }

        bool AlwaysDeclareColumnAlias { get; }
    }
}
