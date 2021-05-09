using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubDataNodeIdentity = System.Guid;
using ComparisonTermsType = OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;
using PartTypeName = System.String;
using CreatorIdentity = System.String;
using OOAdvantech.Transactions;
namespace OOAdvantech.RDBMSPersistenceRunTime
{

    using MetaDataRepository.ObjectQueryLanguage;
    /// <MetaDataID>{5BF26B13-FD76-40c0-9BFF-495165B867C1}</MetaDataID>
    public class SQLScriptsBuilder
    {
        /// <MetaDataID>{732a41aa-5ac1-4e1e-8a4a-cd32e32f28b0}</MetaDataID>
        DataLoader DataLoader;
        /// <MetaDataID>{29da94f0-b395-42df-9e6b-7a23d3f42374}</MetaDataID>
        DataNode DataNode;
        /// <MetaDataID>{5e4cf710-4a76-43eb-88dc-00d4994ad3c0}</MetaDataID>
        RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator;
        /// <MetaDataID>{5fe91d47-76ed-4355-8b75-f7f9e3bdba78}</MetaDataID>
        RDBMSMetaDataRepository.Storage Storage;
        /// <MetaDataID>{9b76d903-2583-45ac-841b-8edc2f82f3fc}</MetaDataID>
        SQLFilterScriptBuilder SQLFilterScriptBuilder;
        /// <MetaDataID>{2a727b47-ffa4-4d40-b700-22cc24838334}</MetaDataID>
        public SQLScriptsBuilder(DataLoader dataLoader)
        {
            DataLoader = dataLoader;
            DataNode = dataLoader.DataNode;
            RDBMSSQLScriptGenarator = dataLoader.RDBMSSQLScriptGenarator;
            Storage = DataLoader.Storage as RDBMSMetaDataRepository.Storage;
            SQLFilterScriptBuilder = new SQLFilterScriptBuilder(dataLoader);
        }

        /// <MetaDataID>{7e34099f-c984-4b8b-99f3-f55b0f5fc5a9}</MetaDataID>
        public string GetSQLScriptForName(string name)
        {
            if (DataLoader.OrgNamesDictionary.ContainsKey(name))
                return RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.OrgNamesDictionary[name]);
            else
                return RDBMSSQLScriptGenarator.GetSQLScriptForName(name);
        }



        /// <MetaDataID>{e323cc0d-4a10-48f4-b1cb-ef2c7ba3e9dd}</MetaDataID>
        public string GeValidName(string name)
        {
            if (DataLoader.OrgNamesDictionary.ContainsKey(name))
                return DataLoader.OrgNamesDictionary[name];
            else
            {
                string validName = RDBMSSQLScriptGenarator.GeValidRDBMSName(name, new List<string>(DataLoader.AliasesDictionary.Keys));
                DataLoader.AliasesDictionary[validName] = name;
                DataLoader.OrgNamesDictionary[name] = validName;
                return validName;
            }
        }


        #region RowRemove code

        ///// <MetaDataID>{45dd4b32-916a-4c9c-820c-0df6f00fbb05}</MetaDataID>
        //internal string GetRowRemoveCaseStatament(SearchCondition searchCondition)
        //{
        //    if (searchCondition != null)
        //    {
        //        if ((DataNode.FilterNotActAsLoadConstraint && searchCondition == DataNode.SearchCondition) ||
        //            DataNode.SearchConditions.Contains(searchCondition))
        //        {

        //            string filterString = null;
        //            if (searchCondition != null)
        //                filterString = SQLFilterScriptBuilder.GetSQLFilterStatament(searchCondition);

        //            if (!string.IsNullOrEmpty(filterString))
        //                return RDBMSSQLScriptGenarator.GetRowRemoveCaseScript(filterString, GeValidName(DataSource.GetRowRemoveColumnName(DataNode, searchCondition)));
        //        }
        //    }
        //    return null;
        //    //if (searchCondition == DataNode.SearchCondition)
        //    //{


        //    //    if (DataNode.FilterAsRowRemove)
        //    //    {
        //    //        string filterString = null;
        //    //        if (DataLoader.SearchCondition != null)
        //    //            filterString = SQLFilterScriptBuilder.GetSQLFilterStatament(DataLoader.SearchCondition);

        //    //        if (!string.IsNullOrEmpty(filterString))
        //    //            return RDBMSSQLScriptGenarator.GetRowRemoveCaseScript(filterString, GeValidName(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove")); //string.Format(@", CASE WHEN {0} THEN cast(0 as bit) ELSE cast(1 as bit) END AS {1}", filterString, DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove");
        //    //        return null;
        //    //    }
        //    //    else
        //    //        return null;
        //    //}
        //    //else
        //    //{
        //    //    string filterString = null;
        //    //    if (searchCondition != null)
        //    //        filterString = SQLFilterScriptBuilder.GetSQLFilterStatament(searchCondition);

        //    //    if (!string.IsNullOrEmpty(filterString))
        //    //        return RDBMSSQLScriptGenarator.GetRowRemoveCaseScript(filterString, GeValidName(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove_" + DataNode.SearchConditions.IndexOf(searchCondition).ToString())); //string.Format(@", CASE WHEN {0} THEN cast(0 as bit) ELSE cast(1 as bit) END AS {1}", filterString, DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove");
        //    //    return null;
        //    //}
        //}

        #endregion

        #region Builds  relation table SQL script



        /// <summary> Builds a sql script to retrieve relation data for StorageCellLinks</summary>
        /// <param name="relatedDataNode">Defines related data node </param>
        /// <param name="association">Defines the assocition of relation data</param>
        /// <param name="objectsLinks">StorageCellLinks used from system to find the relation data source</param>
        /// <MetaDataID>{dedf6007-8f41-4ff6-9f08-2d497e1acbe3}</MetaDataID>
        protected string BuildRelationObjectsObjectLinksSQLScript(DataNode relatedDataNode, MetaDataRepository.AssociationEnd associationEnd, Collections.Generic.Set<MetaDataRepository.StorageCellsLink> objectsLinks)
        {
            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;


            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> OrgRoleAObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> OrgRoleBObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            if (relatedDataNode == DataNode.ParentDataNode)
            {
                if ((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                {
                    OrgRoleAObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);// ObjectIdentityTypes;
                    OrgRoleBObjectIdentityTypes = (GetDataLoader(relatedDataNode) as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
                else
                {
                    OrgRoleAObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                    OrgRoleBObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                }
            }
            else
            {
                if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                {
                    Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> temp = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                    OrgRoleBObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                    OrgRoleAObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
                else
                {
                    OrgRoleAObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                    OrgRoleBObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
            }
            string sqlStatament = null;

            if (objectsLinks.Count == 0)
            {
                sqlStatament = null;

                #region Build an empty association table sql script

                foreach (string relationPartIdentity in OrgRoleBObjectIdentityTypes.Keys)
                {
                    foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in OrgRoleBObjectIdentityTypes[relationPartIdentity])
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (associationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart column in aliasParts)
                        {
                            if (sqlStatament == null)
                                sqlStatament += "SELECT ";
                            else
                                sqlStatament += ",";
                            string nullScript = DataLoader.TypeDictionary.GetDBNullScript(column.Type.FullName);
                            sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);
                        }
                    }
                    foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in OrgRoleAObjectIdentityTypes[relationPartIdentity])
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (associationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart column in aliasParts)
                        {
                            if (sqlStatament == null)
                                sqlStatament += "SELECT ";
                            else
                                sqlStatament += ",";
                            string nullScript = DataLoader.TypeDictionary.GetDBNullScript(column.Type.FullName);

                            sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);
                        }
                    }
                }
                if (associationEnd.Association.RoleA.Indexer && (associationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament += "SELECT ";
                    else
                        sqlStatament += ",";
                    string nullScript = DataLoader.TypeDictionary.GetDBNullScript((associationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Type.FullName);
                    sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias((associationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                }
                if (associationEnd.Association.RoleB.Indexer && (associationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament += "SELECT ";
                    else
                        sqlStatament += ",";
                    string nullScript = DataLoader.TypeDictionary.GetDBNullScript((associationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Type.FullName);
                    sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias((associationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                }
                #endregion

                return "(" + sqlStatament + ") ";
            }
            else
            {
                RDBMSMetaDataRepository.AssociationEnd roleAAssociationEnd = associationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                System.Collections.Generic.List<string> derivedAssocitionTablesSQLScripts = new System.Collections.Generic.List<string>();
                //if (objectsLinks != null && objectsLinks.Count > 0 && association.LinkClass == null)
                //{
                //    foreach (RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks)
                //    {
                //        Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleAObjectIdentityTypes = OrgRoleAObjectIdentityTypes;
                //        Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleBObjectIdentityTypes = OrgRoleBObjectIdentityTypes;

                //        if (objectsLink.ObjectLinksTable == null && objectsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                //            derivedAssocitionTablesSQLScripts.Add(CreateDerivedAssociationTableScrit(OrgRoleAObjectIdentityTypes, OrgRoleBObjectIdentityTypes, objectsLink));
                //    }
                //}

                System.Collections.Generic.List<RDBMSMetaDataRepository.Table> associationTables = new List<OOAdvantech.RDBMSMetaDataRepository.Table>();

                foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in objectsLinks)
                {
                    if (storageCellsLink.UsedAssociationTables.Count == 0) //(storageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        continue;
                    List<DataLoader.DataColumn> associationTableColumns = new List<DataLoader.DataColumn>();

                    foreach (RDBMSMetaDataRepository.Table objectLinksTable in storageCellsLink.UsedAssociationTables)
                    {

                        foreach (string relationPartIdentity in OrgRoleBObjectIdentityTypes.Keys)
                        {

                            foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in OrgRoleBObjectIdentityTypes[relationPartIdentity])
                            {
                                if (associationEnd.Role == OOAdvantech.MetaDataRepository.Roles.RoleB)
                                {
                                    if ((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == roleBObjectIdentityType)
                                    {
                                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts;
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(objectLinksTable))
                                        {
                                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                                            {
                                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                                {
                                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    }
                                }
                                else
                                {
                                    //TODO να εξεταστεί το γιατί και τις δύο φορές χρησιμοποιό roleBObjectIdentityType
                                    if (storageCellsLink.AssotiationClassStorageCells.Count > 1)
                                        throw new System.NotSupportedException("The Build Relation Objects ObjectLinks SQLScript not supported for association class storageCells greater than 1");
                                    if ((storageCellsLink.AssotiationClassStorageCells[0] as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == roleBObjectIdentityType)
                                    {
                                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts;
                                        foreach (MetaDataRepository.IIdentityPart part in (storageCellsLink.AssotiationClassStorageCells[0] as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType.Parts)
                                        {
                                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                                            {
                                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                                {
                                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (roleAAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    }

                                }

                            }
                        }
                        foreach (string relationPartIdentity in OrgRoleAObjectIdentityTypes.Keys)
                        {

                            foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in OrgRoleAObjectIdentityTypes[relationPartIdentity])
                            {
                                if (associationEnd.Role == OOAdvantech.MetaDataRepository.Roles.RoleA)
                                {
                                    if ((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == roleAObjectIdentityType)
                                    {
                                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts;
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(objectLinksTable))
                                        {
                                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                                            {
                                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                                {
                                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    }
                                }
                                else
                                {

                                    if (storageCellsLink.AssotiationClassStorageCells.Count > 1)
                                        throw new System.NotSupportedException("The Build Relation Objects ObjectLinks SQLScript not supported for association class storageCells greater than 1");
                                    if ((storageCellsLink.AssotiationClassStorageCells[0] as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == roleAObjectIdentityType)
                                    {
                                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts;
                                        foreach (MetaDataRepository.IIdentityPart part in (storageCellsLink.AssotiationClassStorageCells[0] as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType.Parts)
                                        {
                                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                                            {
                                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                                {
                                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (roleAAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    }
                                }
                            }
                        }
                    }
                    if (roleAAssociationEnd.Indexer)
                        associationTableColumns.Add(new DataLoader.DataColumn(roleAAssociationEnd.IndexerColumn.DataBaseColumnName, roleAAssociationEnd.IndexerColumn.DataBaseColumnName, typeof(int), DataLoader.DataNode.ObjectQuery));
                    if (roleAAssociationEnd.GetOtherEnd().Indexer)
                        associationTableColumns.Add(new DataLoader.DataColumn((roleAAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, (roleAAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, typeof(int), DataLoader.DataNode.ObjectQuery));

                    foreach (RDBMSMetaDataRepository.Table associationTable in storageCellsLink.UsedAssociationTables)
                    {
                        if (!associationTables.Contains(associationTable))
                        {
                            if (sqlStatament != null)
                                sqlStatament += "\r\nUNION ALL \r\n";
                            sqlStatament += BuildAssociationTableRetrieveSQLScript(associationTableColumns, associationTable, DataLoader.QueryStorageIdentities.IndexOf(storageCellsLink.RoleAStorageCell.StorageIdentity), DataLoader.QueryStorageIdentities.IndexOf(storageCellsLink.RoleBStorageCell.StorageIdentity));
                            associationTables.Add(associationTable);
                        }
                    }
                }
                foreach (string derivedAssocitionTableSQLScript in derivedAssocitionTablesSQLScripts)
                {
                    if (sqlStatament != null)
                        sqlStatament += "\r\nUNION ALL \r\n";
                    sqlStatament += derivedAssocitionTableSQLScript;
                }
                IDataTable tableWithRelationChangesInTransaction = null;
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                    (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Identity == associationEnd.Association.Identity && DataNode.IsParentDataNode(relatedDataNode))
                    tableWithRelationChangesInTransaction = (GetDataLoader(DataNode.RealParentDataNode) as DataLoader).ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(DataNode);
                else
                {
                    foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                    {
                        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Identity == associationEnd.Association.Identity)
                        {
                            tableWithRelationChangesInTransaction = DataLoader.ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(subDataNode);
                            break;
                        }
                    }
                }

                if (tableWithRelationChangesInTransaction != null)
                {
                    #region Merge Transaction relation changes
                    string storageDataName = GetSQLScriptValidAlias("Storage_" + DataNode.Alias);
                    sqlStatament = "(" + sqlStatament + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + storageDataName + "\r\nLEFT OUTER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + " ON ";
                    string joinCriteria = null;
                    string temporarySelectionList = null;
                    string selectionList = null;
                    bool firstObjectIdentityType = true;
                    string outOfTransactionOldRelationsFilter = null;
                    foreach (string relationPartIdentity in OrgRoleBObjectIdentityTypes.Keys)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in roleAAssociationEnd.GetReferenceObjectIdentityTypes(OrgRoleBObjectIdentityTypes[relationPartIdentity]))
                        {
                            string objectIdentityTypeJoinCriteria = null;
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            {
                                if (outOfTransactionOldRelationsFilter != null)
                                    outOfTransactionOldRelationsFilter += " AND ";
                                if (objectIdentityTypeJoinCriteria != null)
                                    objectIdentityTypeJoinCriteria += " AND ";
                                objectIdentityTypeJoinCriteria += storageDataName + "." + GetSQLScriptForName(column.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name);
                                if (temporarySelectionList != null)
                                {
                                    temporarySelectionList += ",";
                                    selectionList += ",";
                                }


                                temporarySelectionList += GetSQLScriptForName(column.Name);
                                selectionList += storageDataName + "." + GetSQLScriptForName(column.Name);
                                outOfTransactionOldRelationsFilter += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name) + " IS NULL ";
                            }
                            if (!firstObjectIdentityType)
                                joinCriteria += " OR ";
                            joinCriteria += objectIdentityTypeJoinCriteria;
                            firstObjectIdentityType = false;
                        }
                    }
                    joinCriteria = "(" + joinCriteria + ") AND (";
                    firstObjectIdentityType = true;
                    foreach (string relationPartIdentity in OrgRoleAObjectIdentityTypes.Keys)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (roleAAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(OrgRoleAObjectIdentityTypes[relationPartIdentity]))
                        {
                            string objectIdentityTypeJoinCriteria = null;
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            {
                                if (objectIdentityTypeJoinCriteria != null)
                                    objectIdentityTypeJoinCriteria += " AND ";
                                temporarySelectionList += ",";
                                selectionList += ",";
                                objectIdentityTypeJoinCriteria += storageDataName + "." + GetSQLScriptForName(column.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name);
                                temporarySelectionList += GetSQLScriptForName(column.Name);
                                selectionList += storageDataName + "." + GetSQLScriptForName(column.Name);
                                outOfTransactionOldRelationsFilter += "AND " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name) + " IS NULL ";
                            }
                            if (!firstObjectIdentityType)
                                joinCriteria += " OR ";
                            joinCriteria += objectIdentityTypeJoinCriteria;
                            firstObjectIdentityType = false;
                        }
                    }
                    selectionList = "SELECT " + storageDataName + "." + GetSQLScriptForName("RoleAStorageIdentity") + ", " + selectionList;
                    selectionList += ", " + storageDataName + "." + GetSQLScriptForName("RoleBStorageIdentity");
                    temporarySelectionList = "SELECT " + GetSQLScriptForName("RoleAStorageIdentity") + ", " + temporarySelectionList;
                    temporarySelectionList += ", " + GetSQLScriptForName("RoleBStorageIdentity");

                    joinCriteria += ")";
                    sqlStatament += joinCriteria;
                    sqlStatament += "\r\nWHERE " + outOfTransactionOldRelationsFilter;
                    sqlStatament += "\r\nUNION ALL \r\n";
                    sqlStatament += temporarySelectionList;
                    sqlStatament += "\r\nFROM " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName);
                    sqlStatament += "\r\nWHERE ChangeType = " + (int)PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added;
                    sqlStatament = selectionList + "\r\nFROM " + sqlStatament;
                    #endregion
                    return "(" + sqlStatament + ") ";

                }
                else
                    return "(" + sqlStatament + ") ";
            }
        }

        /// <MetaDataID>{dedf6007-8f41-4ff6-9f08-2d497e1acbe3}</MetaDataID>
        /// <summary> Builds a sql script to retrieve relation data for StorageCellLinks</summary>
        /// <param name="relatedDataNode">Defines related data node </param>
        /// <param name="association">Defines the assocition of relation data</param>
        /// <param name="objectsLinks">StorageCellLinks used from system to find the relation data source</param>
        protected string BuildObjectLinksSQLScript(DataNode relatedDataNode, RDBMSMetaDataRepository.Association association, Collections.Generic.Set<MetaDataRepository.StorageCellsLink> objectsLinks)
        {
            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> OrgRoleAObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> OrgRoleBObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            if (relatedDataNode == DataNode.RealParentDataNode)
            {
                if ((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                {
                    OrgRoleAObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);// ObjectIdentityTypes;
                    OrgRoleBObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
                else
                {
                    OrgRoleAObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                    OrgRoleBObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                }
            }
            else
            {
                if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                {
                    Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> temp = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                    OrgRoleBObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                    OrgRoleAObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
                else
                {
                    OrgRoleAObjectIdentityTypes = DataLoader.GetRelationPartsObjectIdentityTypes(relatedDataNode);
                    OrgRoleBObjectIdentityTypes = (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                }
            }
            string sqlStatament = null;

            if (objectsLinks.Count == 0)
            {
                sqlStatament = null;

                #region Build an empty association table sql script

                foreach (string relationPartIdentity in OrgRoleBObjectIdentityTypes.Keys)
                {
                    foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in OrgRoleBObjectIdentityTypes[relationPartIdentity])
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart column in aliasParts)
                        {
                            if (sqlStatament == null)
                                sqlStatament += "SELECT ";
                            else
                                sqlStatament += ",";
                            string nullScript = DataLoader.TypeDictionary.GetDBNullScript(column.Type.FullName);
                            sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);
                        }
                    }
                    foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in OrgRoleAObjectIdentityTypes[relationPartIdentity])
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart column in aliasParts)
                        {
                            if (sqlStatament == null)
                                sqlStatament += "SELECT ";
                            else
                                sqlStatament += ",";
                            string nullScript = DataLoader.TypeDictionary.GetDBNullScript(column.Type.FullName);
                            sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);
                        }
                    }
                }
                if (association.RoleA.Indexer && (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament += "SELECT ";
                    else
                        sqlStatament += ",";
                    string nullScript = DataLoader.TypeDictionary.GetDBNullScript((association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Type.FullName);
                    sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias((association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                }
                if (association.RoleB.Indexer && (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament += "SELECT ";
                    else
                        sqlStatament += ",";
                    string nullScript = DataLoader.TypeDictionary.GetDBNullScript((association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Type.FullName);
                    sqlStatament += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias((association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                }
                #endregion

                return "(" + sqlStatament + ") ";
            }
            else
            {
                // RDBMSMetaDataRepository.AssociationEnd roleAAssociationEnd = association.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                System.Collections.Generic.List<string> derivedAssocitionTablesSQLScripts = new System.Collections.Generic.List<string>();
                if (objectsLinks != null && objectsLinks.Count > 0 && association.LinkClass == null)
                {
                    foreach (RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks)
                    {
                        Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleAObjectIdentityTypes = OrgRoleAObjectIdentityTypes;
                        Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleBObjectIdentityTypes = OrgRoleBObjectIdentityTypes;

                        if (objectsLink.ObjectLinksTable == null && objectsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                            derivedAssocitionTablesSQLScripts.Add(CreateDerivedAssociationTableScrit(OrgRoleAObjectIdentityTypes, OrgRoleBObjectIdentityTypes, objectsLink, association));
                    }
                }

                System.Collections.Generic.List<RDBMSMetaDataRepository.Table> associationTables = new List<OOAdvantech.RDBMSMetaDataRepository.Table>();

                foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in objectsLinks)
                {
                    if (storageCellsLink.ObjectLinksTable == null) //(storageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        continue;

                    List<DataLoader.DataColumn> associationTableColumns = GetAssociationTableColumns(association, OrgRoleAObjectIdentityTypes, OrgRoleBObjectIdentityTypes, storageCellsLink);

                    foreach (RDBMSMetaDataRepository.Table associationTable in storageCellsLink.UsedAssociationTables)
                    {
                        if (!associationTables.Contains(associationTable))
                        {
                            if (sqlStatament != null)
                                sqlStatament += "\r\nUNION ALL \r\n";

                            sqlStatament += BuildAssociationTableRetrieveSQLScript(associationTableColumns, associationTable, DataLoader.QueryStorageIdentities.IndexOf(storageCellsLink.RoleAStorageCell.StorageIdentity), DataLoader.QueryStorageIdentities.IndexOf(storageCellsLink.RoleBStorageCell.StorageIdentity));
                            associationTables.Add(associationTable);
                        }
                    }
                }
                foreach (string derivedAssocitionTableSQLScript in derivedAssocitionTablesSQLScripts)
                {
                    if (sqlStatament != null)
                        sqlStatament += "\r\nUNION ALL \r\n";
                    sqlStatament += derivedAssocitionTableSQLScript;
                }
                IDataTable tableWithRelationChangesInTransaction = null;
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Identity == association.Identity)
                    tableWithRelationChangesInTransaction = (GetDataLoader(DataNode.RealParentDataNode) as DataLoader).ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(DataNode);
                else
                {
                    foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                    {
                        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Identity == association.Identity)
                        {
                            tableWithRelationChangesInTransaction = DataLoader.ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(subDataNode);
                            break;
                        }
                    }
                }

                if (tableWithRelationChangesInTransaction != null)
                {
                    #region Merge Transaction relation changes
                    string storageDataName = GetSQLScriptValidAlias("Storage_" + DataNode.Alias);
                    sqlStatament = "(" + sqlStatament + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + storageDataName + "\r\nLEFT OUTER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + " ON ";
                    string joinCriteria = null;
                    string temporarySelectionList = null;
                    string selectionList = null;
                    bool firstObjectIdentityType = true;
                    string outOfTransactionOldRelationsFilter = null;
                    foreach (string relationPartIdentity in OrgRoleBObjectIdentityTypes.Keys)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(OrgRoleBObjectIdentityTypes[relationPartIdentity]))
                        {
                            string objectIdentityTypeJoinCriteria = null;
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            {
                                if (outOfTransactionOldRelationsFilter != null)
                                    outOfTransactionOldRelationsFilter += " AND ";
                                if (objectIdentityTypeJoinCriteria != null)
                                    objectIdentityTypeJoinCriteria += " AND ";
                                objectIdentityTypeJoinCriteria += storageDataName + "." + GetSQLScriptForName(column.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name);
                                if (temporarySelectionList != null)
                                {
                                    temporarySelectionList += ",";
                                    selectionList += ",";
                                }


                                temporarySelectionList += GetSQLScriptForName(column.Name);
                                selectionList += storageDataName + "." + GetSQLScriptForName(column.Name);
                                outOfTransactionOldRelationsFilter += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name) + " IS NULL ";
                            }
                            if (!firstObjectIdentityType)
                                joinCriteria += " OR ";
                            joinCriteria += objectIdentityTypeJoinCriteria;
                            firstObjectIdentityType = false;
                        }
                    }
                    joinCriteria = "(" + joinCriteria + ") AND (";
                    firstObjectIdentityType = true;
                    foreach (string relationPartIdentity in OrgRoleAObjectIdentityTypes.Keys)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ((association.RoleB as RDBMSMetaDataRepository.AssociationEnd)).GetReferenceObjectIdentityTypes(OrgRoleAObjectIdentityTypes[relationPartIdentity]))
                        {
                            string objectIdentityTypeJoinCriteria = null;
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            {
                                if (objectIdentityTypeJoinCriteria != null)
                                    objectIdentityTypeJoinCriteria += " AND ";
                                temporarySelectionList += ",";
                                selectionList += ",";
                                objectIdentityTypeJoinCriteria += storageDataName + "." + GetSQLScriptForName(column.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name);
                                temporarySelectionList += GetSQLScriptForName(column.Name);
                                selectionList += storageDataName + "." + GetSQLScriptForName(column.Name);
                                outOfTransactionOldRelationsFilter += "AND " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName) + "." + GetSQLScriptForName(column.Name) + " IS NULL ";
                            }
                            if (!firstObjectIdentityType)
                                joinCriteria += " OR ";
                            joinCriteria += objectIdentityTypeJoinCriteria;
                            firstObjectIdentityType = false;
                        }
                    }
                    selectionList = "SELECT " + storageDataName + "." + GetSQLScriptForName("RoleAStorageIdentity") + ", " + selectionList;
                    selectionList += ", " + storageDataName + "." + GetSQLScriptForName("RoleBStorageIdentity");
                    temporarySelectionList = "SELECT " + GetSQLScriptForName("RoleAStorageIdentity") + ", " + temporarySelectionList;
                    temporarySelectionList += ", " + GetSQLScriptForName("RoleBStorageIdentity");
                    joinCriteria += ")";
                    sqlStatament += joinCriteria;
                    sqlStatament += "\r\nWHERE " + outOfTransactionOldRelationsFilter;
                    sqlStatament += "\r\nUNION ALL \r\n";
                    sqlStatament += temporarySelectionList;
                    sqlStatament += "\r\nFROM " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(tableWithRelationChangesInTransaction.TableName);
                    sqlStatament += "\r\nWHERE ChangeType = " + (int)PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added;
                    sqlStatament = selectionList + "\r\nFROM " + sqlStatament;
                    #endregion
                    return "(" + sqlStatament + ") ";

                }
                else
                    return "(" + sqlStatament + ") ";
            }
        }

        private List<DataLoader.DataColumn> GetAssociationTableColumns(RDBMSMetaDataRepository.Association association, Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> roleAObjectIdentityTypes, Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> roleBObjectIdentityTypes, RDBMSMetaDataRepository.StorageCellsLink storageCellsLink)
        {
            List<DataLoader.DataColumn> associationTableColumns = new List<DataLoader.DataColumn>();
            foreach (string relationPartIdentity in roleBObjectIdentityTypes.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in roleBObjectIdentityTypes[relationPartIdentity])
                {
                    if (storageCellsLink.RoleBStorageCell.ObjectIdentityType == roleBObjectIdentityType
                        && (storageCellsLink.Type.RoleA.Identity.ToString() == relationPartIdentity || storageCellsLink.Type.RoleB.Identity.ToString() == relationPartIdentity))
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable))
                        {
                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                            {
                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                {
                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MetaDataRepository.Association relationPartAssociation = null;
                        if (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity)
                            relationPartAssociation = association;
                        else
                        {
                            foreach (var speciliazedAssociation in association.Specializations)
                            {
                                if (speciliazedAssociation.RoleA.Identity.ToString() == relationPartIdentity ||
                                    speciliazedAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                                {
                                    relationPartAssociation = speciliazedAssociation;
                                    break;
                                }
                            }
                        }
                        foreach (MetaDataRepository.IIdentityPart part in (relationPartAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (association.RoleA as RDBMSMetaDataRepository.AssociationEnd), "", part, roleBObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                    }
                }
            }

            foreach (string relationPartIdentity in roleAObjectIdentityTypes.Keys)
            {

                foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in roleAObjectIdentityTypes[relationPartIdentity])
                {
                    if (storageCellsLink.RoleAStorageCell.ObjectIdentityType == roleAObjectIdentityType
                        && (storageCellsLink.Type.RoleA.Identity.ToString() == relationPartIdentity || storageCellsLink.Type.RoleB.Identity.ToString() == relationPartIdentity))
                    {
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> aliasParts = (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts;
                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable))
                        {
                            foreach (MetaDataRepository.IIdentityPart aliasPart in aliasParts)
                            {
                                if (aliasPart.PartTypeName == part.PartTypeName && aliasPart.Type.FullName == part.Type.FullName)
                                {
                                    associationTableColumns.Add(new DataLoader.DataColumn(part.Name, aliasPart.Name, part.Type, (association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MetaDataRepository.Association relationPartAssociation = null;
                        if (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity)
                            relationPartAssociation = association;
                        else
                        {
                            foreach (var speciliazedAssociation in association.Specializations)
                            {
                                if (speciliazedAssociation.RoleA.Identity.ToString() == relationPartIdentity ||
                                    speciliazedAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                                {
                                    relationPartAssociation = speciliazedAssociation;
                                    break;
                                }
                            }
                        }
                        foreach (MetaDataRepository.IIdentityPart part in (relationPartAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                            associationTableColumns.Add(new DataLoader.DataColumn(null, part.Name, part.Type, (association.RoleB as RDBMSMetaDataRepository.AssociationEnd), "", part, roleAObjectIdentityType, DataLoader.DataNode.ObjectQuery));
                    }
                }
            }
            if ((association.RoleA as RDBMSMetaDataRepository.AssociationEnd).Indexer)
                associationTableColumns.Add(new DataLoader.DataColumn((association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, typeof(int), DataLoader.DataNode.ObjectQuery));
            if ((association.RoleB as RDBMSMetaDataRepository.AssociationEnd).Indexer)
                associationTableColumns.Add(new DataLoader.DataColumn(((association.RoleB as RDBMSMetaDataRepository.AssociationEnd)).IndexerColumn.DataBaseColumnName, (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, typeof(int), DataLoader.DataNode.ObjectQuery));
            return associationTableColumns;
        }

        /// <MetaDataID>{26d5a9e3-7292-49d7-a01b-28aedea5c61d}</MetaDataID>
        protected string GetStorageCellDataSource(RDBMSMetaDataRepository.StorageCell storageCell)
        {
            return storageCell.ClassView.Name;
        }
        /// <MetaDataID>{0af496e7-a98f-4a23-b57c-5e339c87a545}</MetaDataID>
        protected string GetEmptyColumnScript(string columnName, Type columnType)
        {
            return "";
        }
        /// <summary>
        /// Create a derived association table from one to many relation.
        /// </summary>
        /// <param name="roleAObjectIdentityTypes"></param>
        /// <param name="roleBObjectIdentityTypes"></param>
        /// <param name="objectsLink"></param>
        /// <returns>
        /// SQL script
        /// </returns>
        /// <MetaDataID>{4a9c4810-7fcb-4cb0-91b1-d29378e04e93}</MetaDataID>
        private string CreateDerivedAssociationTableScrit(Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleAObjectIdentityTypes, Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> roleBObjectIdentityTypes, RDBMSMetaDataRepository.StorageCellsLink objectsLink, RDBMSMetaDataRepository.Association generalAssociation)
        {
            RDBMSMetaDataRepository.Association association = (objectsLink.Type as RDBMSMetaDataRepository.Association);
            RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns = objectsLink.GetAssociationEndWithReferenceColumns();
            string formClause = null;
            RDBMSMetaDataRepository.StorageCell storageCellWithRelationData = null;
            RDBMSMetaDataRepository.StorageCell storageCellWithOutRelationData = null;
            if (associationEndWithReferenceColumns.IsRoleA)
            {
                storageCellWithRelationData = (objectsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell);// +" INNER JOIN " + (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name;
                storageCellWithOutRelationData = (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell);
            }
            else
            {
                storageCellWithRelationData = (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell);
                storageCellWithOutRelationData = (objectsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell);
            }
            formClause = GetStorageCellDataSource(storageCellWithRelationData);// +" INNER JOIN " + (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name;
            int i = 0;
            string selectClause = "";
            string whereClause = null;
            foreach (string relationPartIdentity in roleBObjectIdentityTypes.Keys)
            {
                RDBMSMetaDataRepository.AssociationEnd roleAAssociationEnd = null;
                if (generalAssociation.RoleA.Identity.ToString() == relationPartIdentity || generalAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                    roleAAssociationEnd = generalAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                else
                {
                    foreach (var speciliazedAssociation in generalAssociation.Specializations)
                    {
                        if (speciliazedAssociation.RoleA.Identity.ToString() == relationPartIdentity ||
                            speciliazedAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                        {

                            roleAAssociationEnd = speciliazedAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                            break;
                        }
                    }
                }
                foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in roleBObjectIdentityTypes[relationPartIdentity])
                {
                    if (!associationEndWithReferenceColumns.IsRoleA)
                    {
                        if (storageCellWithRelationData.ObjectIdentityType == roleBObjectIdentityType
                            && (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity))
                        {
                            //The storage cell id columns are the RoleB columns.
                            foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in storageCellWithRelationData.ObjectIdentityType.Parts)
                            {
                                foreach (RDBMSMetaDataRepository.Column column in storageCellWithRelationData.ClassView.ViewColumns)
                                {
                                    if (column.RealColumn == identityColumn)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                        {
                                            if (part.PartTypeName == (identityColumn as MetaDataRepository.IIdentityPart).PartTypeName)
                                            {
                                                if (i > 0)
                                                    selectClause += ",";
                                                selectClause += GetSQLScriptForName(column.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                                if (whereClause == null)
                                                    whereClause = " WHERE ";
                                                else
                                                    whereClause += " AND ";
                                                whereClause += GetSQLScriptForName(column.Name);
                                                whereClause += " IS NOT NULL";
                                                i++;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {

                            foreach (MetaDataRepository.IIdentityPart sourcePart in roleBObjectIdentityType.Parts)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in roleAAssociationEnd. /*(association.RoleA as RDBMSMetaDataRepository.AssociationEnd).*/GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                {
                                    if (part.PartTypeName == sourcePart.PartTypeName)
                                    {
                                        if (i > 0)
                                            selectClause += ",";
                                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(part.Type.FullName);

                                        selectClause += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                        i++;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (storageCellWithOutRelationData.ObjectIdentityType == roleBObjectIdentityType
                            && (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity))
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellWithRelationData))
                            {
                                foreach (RDBMSMetaDataRepository.Column column in storageCellWithRelationData.ClassView.ViewColumns)
                                {
                                    if (column.RealColumn == identityColumn)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                        {
                                            if (part.PartTypeName == (identityColumn as MetaDataRepository.IIdentityPart).PartTypeName)
                                            {
                                                if (i > 0)
                                                    selectClause += ",";
                                                selectClause += GetSQLScriptForName(column.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                                if (whereClause == null)
                                                    whereClause = " WHERE ";
                                                else
                                                    whereClause += " AND ";
                                                whereClause += GetSQLScriptForName(column.Name);
                                                whereClause += " IS NOT NULL";
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {

                            foreach (MetaDataRepository.IIdentityPart sourcePart in roleBObjectIdentityType.Parts)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in roleAAssociationEnd./*(association.RoleA as RDBMSMetaDataRepository.AssociationEnd)*/GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleBObjectIdentityType })[0].Parts)
                                {
                                    if (part.PartTypeName == sourcePart.PartTypeName)
                                    {

                                        if (i > 0)
                                            selectClause += ",";
                                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(part.Type.FullName);
                                        selectClause += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                        i++;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            if ((association.RoleB as RDBMSMetaDataRepository.AssociationEnd).Indexer)
            {
                OOAdvantech.RDBMSMetaDataRepository.Column column = (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectsLink.RoleBStorageCell, "");
                if (column != null)
                    selectClause += column.Name + RDBMSSQLScriptGenarator.AliasDefSqlScript + (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                else
                    selectClause += "0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
            }


            foreach (string relationPartIdentity in roleAObjectIdentityTypes.Keys)
            {
                RDBMSMetaDataRepository.AssociationEnd roleBAssociationEnd = null;
                if (generalAssociation.RoleA.Identity.ToString() == relationPartIdentity || generalAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                    roleBAssociationEnd = generalAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd;
                else
                {
                    foreach (var speciliazedAssociation in generalAssociation.Specializations)
                    {
                        if (speciliazedAssociation.RoleA.Identity.ToString() == relationPartIdentity ||
                            speciliazedAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                        {
                            roleBAssociationEnd = speciliazedAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd;
                            break;
                        }
                    }
                }
                foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in roleAObjectIdentityTypes[relationPartIdentity])
                {
                    if (associationEndWithReferenceColumns.IsRoleA)
                    {
                        if (storageCellWithRelationData.ObjectIdentityType == roleAObjectIdentityType
                            && (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity))
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in storageCellWithRelationData.ObjectIdentityType.Parts)
                            {
                                foreach (RDBMSMetaDataRepository.Column column in storageCellWithRelationData.ClassView.ViewColumns)
                                {
                                    if (column.RealColumn == identityColumn)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                        {
                                            if (part.PartTypeName == (identityColumn as MetaDataRepository.IIdentityPart).PartTypeName)
                                            {
                                                if (i > 0)
                                                    selectClause += ",";
                                                selectClause += GetSQLScriptForName(column.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                                if (whereClause == null)
                                                    whereClause = " WHERE ";
                                                else
                                                    whereClause += " AND ";
                                                whereClause += GetSQLScriptForName(column.Name);
                                                whereClause += " IS NOT NULL";
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (MetaDataRepository.IIdentityPart sourcePart in roleAObjectIdentityType.Parts)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in roleBAssociationEnd. /*(association.RoleB as RDBMSMetaDataRepository.AssociationEnd)*/GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                {
                                    if (part.PartTypeName == sourcePart.PartTypeName)
                                    {
                                        if (i > 0)
                                            selectClause += ",";
                                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(part.Type.FullName);
                                        selectClause += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                        i++;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (storageCellWithOutRelationData.ObjectIdentityType == roleAObjectIdentityType
                            && (association.RoleA.Identity.ToString() == relationPartIdentity || association.RoleB.Identity.ToString() == relationPartIdentity))
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellWithRelationData))
                            {
                                foreach (RDBMSMetaDataRepository.Column column in storageCellWithRelationData.ClassView.ViewColumns)
                                {
                                    if (column.RealColumn == identityColumn)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                        {
                                            if (part.PartTypeName == (identityColumn as MetaDataRepository.IIdentityPart).PartTypeName)
                                            {
                                                if (i > 0)
                                                    selectClause += ",";
                                                selectClause += GetSQLScriptForName(column.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                                if (whereClause == null)
                                                    whereClause = " WHERE ";
                                                else
                                                    whereClause += " AND ";
                                                whereClause += GetSQLScriptForName(column.Name);
                                                whereClause += " IS NOT NULL";
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (MetaDataRepository.IIdentityPart sourcePart in roleAObjectIdentityType.Parts)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in roleBAssociationEnd./* (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).*/GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType> { roleAObjectIdentityType })[0].Parts)
                                {
                                    if (part.PartTypeName == sourcePart.PartTypeName)
                                    {
                                        if (i > 0)
                                            selectClause += ",";
                                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(part.Type.FullName);
                                        selectClause += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(part.Name);
                                        i++;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            if ((association.RoleA as RDBMSMetaDataRepository.AssociationEnd).Indexer)
            {
                OOAdvantech.RDBMSMetaDataRepository.Column column = (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectsLink.RoleAStorageCell, "");
                selectClause += ",";
                if (column != null)
                    selectClause += column.Name + RDBMSSQLScriptGenarator.AliasDefSqlScript + (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                else
                    selectClause += "0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
            }
            selectClause = DataLoader.QueryStorageIdentities.IndexOf(objectsLink.RoleAStorageCell.StorageIdentity).ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("RoleAStorageIdentity") + ", " +
                selectClause + ", " + DataLoader.QueryStorageIdentities.IndexOf(objectsLink.RoleBStorageCell.StorageIdentity).ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("RoleBStorageIdentity");
            string associationTableStatement = "select " + selectClause + " from " + formClause + whereClause;
            return associationTableStatement;
        }

        /// <MetaDataID>{37b83e66-696e-45f3-804d-3fd1d59900ab}</MetaDataID>
        private string BuildAssociationTableRetrieveSQLScript(List<DataLoader.DataColumn> associationTableColumns, RDBMSMetaDataRepository.Table associationTable, int roleAStorageIdentity, int roleBStorageIdentity)
        {
            string selectClause = null;
            foreach (DataLoader.DataColumn column in associationTableColumns)
            {
                if (selectClause == null)
                    selectClause = "SELECT  " + roleAStorageIdentity.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("RoleAStorageIdentity") + ", ";
                else
                    selectClause += ", ";
                if (string.IsNullOrEmpty(column.Name))
                {
                    string nullScript = DataLoader.TypeDictionary.GetDBNullScript(column.Type.FullName);
                    selectClause += nullScript + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Alias);
                }
                else
                    selectClause += GetSQLScriptForName(column.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Alias);


            }
            selectClause += ", " + roleBStorageIdentity.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("RoleBStorageIdentity");
            return selectClause + "\r\nFROM " + associationTable.Name;
        }
        #endregion


        #region Operations which construct Data loader data SQL scripts.
        /// <MetaDataID>{5ED2E587-D86D-49E8-A029-C2D806782EA8}</MetaDataID>
        ///<summary>
        ///Construct the SQL scrip statement for the classifier data of data loader.
        ///</summary>
        ///<returns>
        ///SQL script retrieves necessary classifier data for the data loader. 
        ///</returns>
        protected string BuildClassifierDataRetrieveSQLScript()
        {
            if (DataNode.Type == DataNode.DataNodeType.Group)
                return "(" + DataLoader.SQLStatament + ")";

            System.Collections.Generic.List<string> columns = new List<string>();

            bool hasInStorageStorageCell = false;
            foreach (MetaDataRepository.StorageCell storageCell in DataLoader.DataLoaderMetadata.StorageCells)
            {
                if (storageCell is RDBMSMetaDataRepository.StorageCell)
                {
                    hasInStorageStorageCell = true;
                    break;
                }
            }


            if (hasInStorageStorageCell)
            {
                foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
                    columns.Add(dataColumn.Name);
                string sqlStatament = null;
                if (DataLoader.DataLoaderMetadata.MemoryCell != null)
                {
                    string selectSQL = null;
                    foreach (string columnName in columns)
                    {
                        if (selectSQL == null)
                            selectSQL = @"SELECT  ";
                        else
                            selectSQL += ",";
                        if (columnName == "OSM_StorageIdentity")
                            selectSQL += DataLoader.DataLoaderMetadata.QueryStorageID + " as OSM_StorageIdentity ";
                        else
                            selectSQL += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + GetSQLScriptValidAlias(columnName);
                    }
                    string sql = selectSQL + "\r\n FROM " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName);
                    sql += "\r\nWHERE " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + DataLoader.ObjectsUnderTransactionData.ChangeTypeColumnName + "<>2";
                    sqlStatament = "(" + sql + ")";
                    DataLoader.TransferTemporaryTableToDataBase();
                    return sqlStatament;
                }


                foreach (MetaDataRepository.StorageCell storageCell in DataLoader.DataLoaderMetadata.StorageCells)
                {
                    if (storageCell is RDBMSMetaDataRepository.StorageCell)
                    {
                        if (sqlStatament != null)
                            sqlStatament += "\r\nUNION ALL \r\n";
                        else
                            sqlStatament = "(";
                        sqlStatament += BuildStorageCellDataRetriveSQL(DataLoader.ClassifierDataColumns, storageCell as RDBMSMetaDataRepository.StorageCell);
                    }
                }
                sqlStatament += ")";


                DataLoader.TransferTemporaryTableToDataBase();
                if (DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction != null)
                {
                    string storageDataName = GetSQLScriptValidAlias("Storage_" + DataNode.Alias);
                    string sql = null;
                    string selectSQL = null;
                    foreach (string columnName in columns)
                    {
                        if (selectSQL == null)
                            selectSQL = @"SELECT  ";
                        else
                            selectSQL += ",";
                        selectSQL += storageDataName + "." + GetSQLScriptValidAlias(columnName);
                    }
                    sql = selectSQL + "\r\nFROM\r\n" + sqlStatament;

                    sql += RDBMSSQLScriptGenarator.AliasDefSqlScript + storageDataName + "\r\nLEFT OUTER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + " ON ";
                    string joinCriterion = null;
                    string filterStorageDataString = null;
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                    {
                        string objectIdentityTypeJoinCriteria = null;
                        string objectIdentityTypefilterStorageDataString = null;
                        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                        {
                            if (objectIdentityTypeJoinCriteria != null)
                                objectIdentityTypeJoinCriteria += " AND ";

                            if (objectIdentityTypefilterStorageDataString != null)
                                objectIdentityTypefilterStorageDataString += " AND ";

                            objectIdentityTypeJoinCriteria += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + GetSQLScriptValidAlias(column.Name);
                            objectIdentityTypeJoinCriteria += " = ";
                            objectIdentityTypeJoinCriteria += storageDataName + "." + column.Name;
                            objectIdentityTypefilterStorageDataString += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + GetSQLScriptValidAlias(column.Name) + " IS NULL";

                        }
                        if (joinCriterion != null)
                            joinCriterion += " OR ";
                        joinCriterion += "(" + objectIdentityTypeJoinCriteria + ")";
                        if (filterStorageDataString != null)
                            filterStorageDataString += " OR ";
                        filterStorageDataString += "(" + objectIdentityTypefilterStorageDataString + ")";
                    }
                    sql += joinCriterion + "\r\nWHERE " + filterStorageDataString;
                    sql += "\r\nUNION ALL\r\n";

                    selectSQL = null;
                    foreach (string columnName in columns)
                    {
                        if (selectSQL == null)
                            selectSQL = @"SELECT  ";
                        else
                            selectSQL += ",";
                        if (columnName == "OSM_StorageIdentity")
                            selectSQL += DataLoader.DataLoaderMetadata.QueryStorageID + " as OSM_StorageIdentity ";
                        else
                            selectSQL += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + GetSQLScriptValidAlias(columnName);
                    }
                    sql += selectSQL + "\r\n FROM " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName);
                    sql += "\r\nWHERE " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + DataLoader.ObjectsUnderTransactionData.ChangeTypeColumnName + "<>2";
                    sqlStatament = "(" + sql + ")";
                }
                return sqlStatament;
            }
            else
            {
                if (DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction != null)
                {
                    foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
                        columns.Add(dataColumn.Name);
                    DataLoader.TransferTemporaryTableToDataBase();
                    string sql = null;
                    string selectSQL = null;
                    foreach (string columnName in columns)
                    {
                        if (selectSQL == null)
                            selectSQL = @"SELECT  ";
                        else
                            selectSQL += ",";
                        if (columnName == "OSM_StorageIdentity")
                            selectSQL += DataLoader.DataLoaderMetadata.QueryStorageID + " as OSM_StorageIdentity ";
                        else
                            selectSQL += RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + GetSQLScriptValidAlias(columnName);
                    }
                    sql += selectSQL + "\r\n FROM " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName);
                    sql += "\r\nWHERE " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript(DataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName) + "." + DataLoader.ObjectsUnderTransactionData.ChangeTypeColumnName + "<>2";
                    return "(" + sql + ")";

                }
                else
                    return GetEmptyDataSQL();
            }
        }



        /// <MetaDataID>{99B1878E-A42C-4663-8738-7084BE6D54F8}</MetaDataID>
        /// <summary> 
        /// Construct the data retrieve SQL script for Storage Cell. 
        /// </summary>
        /// <param name="columns">
        /// The columns parameter defines the columns which is necessary.  
        /// If the storage cell Data Object hasn’t some columns from the column collection, 
        /// defines a null alias column on SQL script.
        /// </param>
        /// <remarks>
        /// Storage Cell data retrieve may be participating in union all SQL script, 
        /// with other storage cell of class other than the class of operation parameter storage cell.
        /// </remarks>
        /// <param name="storageCell">
        /// This parameter defines the storage cell where operation uses to construct SQL scrip to retrieve data.
        /// </param>
        protected string BuildStorageCellDataRetriveSQL(System.Collections.Generic.List<DataLoader.DataColumn> columns, RDBMSMetaDataRepository.StorageCell storageCell)
        {
            string selectClause = null;

            //			When data node is type object and participates in select clause of query then 
            //			the procedure of construction UNION TABLES must be allocate columns for objects 
            //			with maximum number of columns.
            //			For object that belong to the class in class hierarchy with less columns, 
            //			we will add extra columns which missed with null values. 
            //			This happen because in the union table statement all select must be 
            //			contain the same number and same type of columns

            // if storage cell mapped on more than one table data loader use the ClassView to retrieve data
            // else use the mapped table 

            List<OOAdvantech.RDBMSMetaDataRepository.Column> mappedColumns = null;
            if (storageCell.ClassView.JoinedTables.Count > 1)
                mappedColumns = new List<OOAdvantech.RDBMSMetaDataRepository.Column>(storageCell.ClassView.ViewColumns);
            else
            {
                mappedColumns = new List<OOAdvantech.RDBMSMetaDataRepository.Column>(storageCell.ClassView.JoinedTables[0].ContainedColumns);
                foreach (var identityColumn in storageCell.ClassView.JoinedTables[0].ObjectIDColumns)
                    mappedColumns.Add(identityColumn);
                if (storageCell.ClassView.JoinedTables[0].ReferentialIntegrityColumn != null)
                    mappedColumns.Add(storageCell.ClassView.JoinedTables[0].ReferentialIntegrityColumn);
            }
            foreach (DataLoader.DataColumn dataColumn in columns)
            {
                if (selectClause == null)
                    selectClause = "SELECT ";
                else
                    selectClause += " , ";
                bool columnExist = false;
                foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in mappedColumns)
                {
                    OOAdvantech.RDBMSMetaDataRepository.Column tableColumn = column.RealColumn;
                    if (column.Namespace is RDBMSMetaDataRepository.Table)
                        tableColumn = column;
                    if (dataColumn.MappedObject == column.MappedAttribute && dataColumn.MappedObject != null && (dataColumn.CreatorIdentity == tableColumn.CreatorIdentity))
                    {
                        string aliasName = dataColumn.Alias;

                        aliasName = dataColumn.Name;
                        selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                        columnExist = true;
                        break;
                    }
                    if (dataColumn.MappedObject == column.MappedAssociationEnd && dataColumn.MappedObject != null && dataColumn.IdentityPart != null &&
                        (tableColumn is MetaDataRepository.IIdentityPart) &&
                         dataColumn.CreatorIdentity == tableColumn.CreatorIdentity &&
                        dataColumn.IdentityPart.PartTypeName == (tableColumn as MetaDataRepository.IIdentityPart).PartTypeName &&
                        dataColumn.IdentityPart.Type.FullName == (tableColumn as MetaDataRepository.IIdentityPart).Type.FullName)
                    {
                        string aliasName = dataColumn.Name;
                        //if (string.IsNullOrEmpty(aliasName))
                        //    aliasName = columnName.Name;
                        selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                        columnExist = true;

                        break;
                    }

                    if (dataColumn.MappedObject is RDBMSMetaDataRepository.AssociationEnd &&
                        (dataColumn.MappedObject as RDBMSMetaDataRepository.AssociationEnd).Indexer && tableColumn != null)
                    {
                        string associationEndPathIdentity = tableColumn.CreatorIdentity;
                        if (associationEndPathIdentity != null && !string.IsNullOrEmpty(associationEndPathIdentity.Trim()))
                        {
                            MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath(associationEndPathIdentity);
                            valueTypePath.Pop();
                            associationEndPathIdentity = valueTypePath.ToString();
                        }
                        if (!dataColumn.IdentityColumn &&
                        (dataColumn.MappedObject as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCell, associationEndPathIdentity) == tableColumn)
                        {
                            string aliasName = dataColumn.Name;
                            //if (string.IsNullOrEmpty(aliasName))
                            //    aliasName = columnName.Name;
                            selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                            columnExist = true;
                            break;
                        }
                    }
                    if (dataColumn.MappedObject == null)
                    {
                        if (!dataColumn.IdentityColumn && dataColumn.Name == column.Name)
                        {
                            string aliasName = dataColumn.Name;
                            //if (string.IsNullOrEmpty(aliasName))
                            //    aliasName = columnName.Name;

                            selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                            columnExist = true;
                            break;
                        }
                        else
                            if (dataColumn.IdentityColumn &&
                                //                                     columnName.Name == column.Name&&
                                tableColumn != null &&
                                tableColumn.MappedAssociationEnd == null &&
                                tableColumn is MetaDataRepository.IIdentityPart &&
                                dataColumn.IdentityPart.PartTypeName == (tableColumn as MetaDataRepository.IIdentityPart).PartTypeName &&
                                dataColumn.IdentityPart.Type == (tableColumn as MetaDataRepository.IIdentityPart).Type)
                            {
                                string aliasName = dataColumn.Name;
                                //if (string.IsNullOrEmpty(aliasName))
                                //    aliasName = columnName.Name;
                                selectClause += GetSQLScriptForName(column.DataBaseColumnName) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasName);
                                columnExist = true;
                                break;
                            }

                        if (!columnExist && dataColumn.Name == "StorageCellID")
                        {
                            selectClause += storageCell.SerialNumber.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(dataColumn.Name);
                            columnExist = true;
                        }

                    }
                }

                if (!columnExist)
                {
                    if (dataColumn.Name == "TypeID")
                    {
                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(dataColumn.Type.FullName);
                        selectClause += string.Format(" {0} ", (storageCell.Type as RDBMSMetaDataRepository.Class).TypeID) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(dataColumn.Name);
                    }
                    else
                    {
                        string nullScript = DataLoader.TypeDictionary.GetDBNullScript(dataColumn.Type.FullName);
                        selectClause += string.Format(" {0} ", nullScript) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(dataColumn.Name);
                    }
                }
            }
            string fromClause = null;
            if (storageCell.ClassView.JoinedTables.Count > 1)
                fromClause = "\r\nFROM " + storageCell.ClassView.Name;
            else
                fromClause = "\r\nFROM " + storageCell.ClassView.JoinedTables[0].Name;

            return selectClause + fromClause;
        }


        /// <MetaDataID>{dc8ae99b-5759-4e1d-8fb1-8807c8689513}</MetaDataID>
        /// <summary>Build a SQL script which returns an empty table.</summary>
        protected string GetEmptyDataSQL()
        {
            string sqlStatament = null;
            string whereClause = null;

            foreach (DataLoader.DataColumn column in DataLoader.ClassifierDataColumns)
            {
                string @default = "NULL";
                if (column.Type != null)
                {
                    if (column.Type.GetMetaData().IsEnum)
                        @default = DataLoader.TypeDictionary.GetDBDefaultValue(typeof(int).FullName);
                    else
                        @default = DataLoader.TypeDictionary.GetDBDefaultValue(column.Type.FullName);
                }
                if (sqlStatament == null)
                {
                    whereClause = "WHERE " + GetSQLScriptForName(column.Name) + " <> " + @default;
                    sqlStatament += "(SELECT " + @default + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);
                }
                else
                    sqlStatament += "," + @default + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Name);

            }
            if (!string.IsNullOrEmpty(RDBMSSQLScriptGenarator.GhostTable))
                sqlStatament += " FROM " + RDBMSSQLScriptGenarator.GhostTable;



            sqlStatament = "(SELECT * FROM " + sqlStatament + ") " + GetSQLScriptForName("TABLE") + whereClause;
            sqlStatament += ")";
            return sqlStatament;
        }
        #endregion

        /// <MetaDataID>{b417e4c0-9d80-4d67-8844-9d18b48f58b3}</MetaDataID>
        internal string GetContainsAnyAllSQL(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
            {
                System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                DataNode dataNode = criterion.CollectionContainsDataNode;
                if (dataNode.Type == DataNode.DataNodeType.Object)
                    relatedDataNodes.Add(dataNode, true);
                while (dataNode.RealParentDataNode != DataNode)
                {
                    dataNode = dataNode.RealParentDataNode;
                    relatedDataNodes.Add(dataNode, true);
                }


                string query = BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias);
                query += BuildJoinedTablesSQLScript(relatedDataNodes);

                string selectionList = null;

                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                {
                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    {
                        if (selectionList == null)
                            selectionList = "SELECT DISTINCT ";
                        else
                            selectionList += ",";
                        selectionList += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                    }
                }


                if (criterion.CollectionContainsDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (criterion.CollectionContainsDataNode.RealParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    {
                        query = selectionList + ",1 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + " " + GetSQLScriptForName("ContainsAny") + " FROM " + query + "INNER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + " on ";
                        query += GetSQLScriptForName(criterion.CollectionContainsDataNode.RealParentDataNode.Alias) + "." + GetSQLScriptForName(criterion.CollectionContainsDataNode.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + GetSQLScriptForName(criterion.CollectionContainsDataNode.Name);
                    }
                    else
                    {
                        selectionList += ",0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + " " + GetSQLScriptForName("ContainsAny");
                        query = selectionList + " FROM " + query;
                    }

                }
                else
                {
                    if (criterion.CollectionContainsDataNode.RealParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    {
                        query = selectionList + " FROM " + query + "INNER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + " on ";
                        string joinCriterion = null;
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in criterion.CollectionContainsDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (joinCriterion != null)
                                    joinCriterion += " AND ";
                                joinCriterion += GetSQLScriptForName(criterion.CollectionContainsDataNode.Alias) + "." + GetSQLScriptForName(part.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + GetSQLScriptForName(part.Name);
                            }
                        }
                        query += joinCriterion;
                    }
                    {
                        selectionList += ",1 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + " " + GetSQLScriptForName("ContainsAny");
                        query = selectionList + " FROM (" + query + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(DataNode.Alias);
                    }
                }
                return query;
            }
            else
            {
                System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                DataNode dataNode = criterion.CollectionContainsDataNode;
                if (dataNode.Type == DataNode.DataNodeType.Object)
                    relatedDataNodes.Add(dataNode, true);
                while (dataNode.RealParentDataNode != DataNode)
                {
                    dataNode = dataNode.RealParentDataNode;
                    relatedDataNodes.Add(dataNode, true);
                }

                string query = BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias);
                query += BuildJoinedTablesSQLScript(relatedDataNodes);
                string selectionList = null;
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                {
                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    {
                        if (selectionList == null)
                            selectionList = "SELECT DISTINCT ";
                        else
                            selectionList += ",";
                        selectionList += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                    }
                }


                if (criterion.CollectionContainsDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (criterion.CollectionContainsDataNode.RealParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    {
                        selectionList += "," + GetSQLScriptForName(criterion.CollectionContainsDataNode.RealParentDataNode.Alias) + "." + GetSQLScriptForName(criterion.CollectionContainsDataNode.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("FilteredWith" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName);
                        query = selectionList + " FROM " + query + "INNER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + " ON ";
                        query += GetSQLScriptForName(criterion.CollectionContainsDataNode.RealParentDataNode.Alias) + "." + GetSQLScriptForName(criterion.CollectionContainsDataNode.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + GetSQLScriptForName(criterion.CollectionContainsDataNode.Name);
                    }
                    else
                        query = selectionList + "\r\nFROM " + query;
                }
                else
                {
                    if (criterion.CollectionContainsDataNode.RealParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in criterion.CollectionContainsDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                selectionList += "," + GetSQLScriptForName(criterion.CollectionContainsDataNode.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("FilteredWith" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName);


                        query = selectionList + " FROM " + query + "INNER JOIN " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + " ON ";
                        string joinCriterion = null;
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in criterion.CollectionContainsDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (joinCriterion != null)
                                    joinCriterion += " AND ";
                                joinCriterion += GetSQLScriptForName(criterion.CollectionContainsDataNode.Alias) + "." + GetSQLScriptForName(part.Name) + " = " + RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + GetSQLScriptForName(part.Name);
                                //slectionList += ",[" + criterion.FilteredDataNode.Alias + "].[" + column.Name + "]";
                            }
                        }
                        query += joinCriterion;
                    }
                    else
                        query = selectionList + "\r\n FROM " + query;



                }
                query = " FROM ( " + query + " ) " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias + "HasAll");

                string containsSelect = null;
                //query = "SELECT [" + DataNode.Alias + "HasAll].* " + ",1 as [ContainsAll] \n" + query;
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                {
                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    {
                        if (containsSelect == null)
                            containsSelect = "SELECT ";
                        else
                            containsSelect += ",";
                        containsSelect += GetSQLScriptForName(DataNode.Alias + "HasAll") + "." + GetSQLScriptForName(part.Name);
                    }
                }
                if (criterion.CollectionContainsDataNode.RealParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    containsSelect += ",1 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("ContainsAll") + " \r\n";
                else
                {
                    containsSelect += ",0" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("ContainsAll") + " \r\n";
                    query = containsSelect + query;
                    return query;
                }

                string groupBy = null;
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                {

                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    {
                        if (groupBy == null)
                            groupBy = "\r\nGROUP BY ";
                        else
                            groupBy += ",";
                        groupBy += GetSQLScriptForName(DataNode.Alias + "HasAll") + "." + GetSQLScriptForName(part.Name);
                    }
                }
                query = query + groupBy;
                query = query + string.Format("\r\nHAVING  COUNT(*) =(SELECT COUNT(*) from {0})", RDBMSSQLScriptGenarator.GetTemporaryTableNameScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName));

                return containsSelect + query;
                //slectionList += ",1 as [ContainsAny] ";

            }
        }


        /// <MetaDataID>{1ba494a9-5246-4528-835b-62c4243b72d1}</MetaDataID>
        internal string BuildRecursiveLoad()
        {
            if (DataNode.ThroughRelationTable)
                return BuildRecursiveLoadManyToMany(DataLoader.RelatedDataNodes);
            else
                return BuildRecursiveLoadOneToMany(DataLoader.RelatedDataNodes);
        }
        /// <MetaDataID>{078eebb3-06fb-4dd5-8586-1048d86c5068}</MetaDataID>
        string BuildRecursiveLoadManyToMany(System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {

            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<MetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink>();

            foreach (MetaDataRepository.StorageCell CurrStorageCell in DataLoader.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                {
                    foreach (var storageCellsLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell))
                        ObjectsLinks.Add(storageCellsLink);
                }

            }
            //ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.

            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);
            string AssociationTableStatement = BuildObjectLinksSQLScript(DataNode, associationEnd.Association as RDBMSMetaDataRepository.Association, ObjectsLinks) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(AliasAssociationTableName);
            #endregion

            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> leftJoinTableColumns = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            leftJoinTableColumns[associationEnd.Identity.ToString()] = DataLoader.ObjectIdentityTypes;

            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> rightJoinTableColumns = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();

            rightJoinTableColumns[associationEnd.Identity.ToString()] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(DataLoader.ObjectIdentityTypes);
            if (rightJoinTableColumns.Count != leftJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

            string objectIDColumnsString = null;
            string refernceColumnsString = null;
            string relationTableRefernceColumnsString = null;
            string childObjectIDColumnsString = null;
            string childRefernceColumnsString = null;
            string parentObjectIDColumnsString = null;
            string parentRefernceColumnsString = null;

            string dataColumnsString = null;
            string parentDataColumnsString = null;
            string childDataColumnsString = null;
            string innerJoinColumnsString = null;

            List<string> columns = new List<string>();
            //columns.Add("StorageIdentity");
            foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
                columns.Add(dataColumn.Name);



            MetaDataRepository.Classifier classifier = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
            //foreach (RDBMSMetaDataRepository.AssociationEnd associationEnd in classifier.GetRoles(true))

            //if (associationEnd.Identity == DataNode.AssignedMetaObject.Identity)

            //System.Collections.Generic.List<string> recursiveRelationColumns = new System.Collections.Generic.List<string>();
            List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
            foreach (List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes in (DataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode).Values)
                referenceObjectIdentityTypes.AddRange(associationEnd.GetReferenceObjectIdentityTypes(objectIdentityTypes));

            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
            {
                foreach (MetaDataRepository.IdentityPart part in objectIdentityType.Parts)
                {
                    if (objectIDColumnsString != null)
                        objectIDColumnsString += ",";
                    objectIDColumnsString += part.Name;
                    columns.Remove(part.Name);

                    if (parentObjectIDColumnsString != null)
                        parentObjectIDColumnsString += ",";
                    parentObjectIDColumnsString += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(part.Name);


                    if (childObjectIDColumnsString != null)
                        childObjectIDColumnsString += ",";
                    childObjectIDColumnsString += "child." + part.Name;
                }
            }
            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
            {
                foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                {
                    if (refernceColumnsString != null)
                        refernceColumnsString += ",";
                    refernceColumnsString += GetSQLScriptForName(column.Name);

                    if (relationTableRefernceColumnsString != null)
                        relationTableRefernceColumnsString += ",";
                    relationTableRefernceColumnsString += GetSQLScriptForName(AliasAssociationTableName) + "." + GetSQLScriptForName(column.Name);


                    if (parentRefernceColumnsString != null)
                        parentRefernceColumnsString += ",";
                    parentRefernceColumnsString += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(column.Name);

                    columns.Remove(column.Name);
                    if (childRefernceColumnsString != null)
                        childRefernceColumnsString += ",";
                    childRefernceColumnsString += "child." + column.Name;
                }
            }
            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
            {
                foreach (MetaDataRepository.ObjectIdentityType referenceObjectIdentityType in referenceObjectIdentityTypes)
                {
                    if (referenceObjectIdentityType == objectIdentityType)
                    {
                        if (innerJoinColumnsString != null)
                            innerJoinColumnsString += " OR (";
                        else
                            innerJoinColumnsString += "( ";
                        string partialInnerJoinColumnsString = null;
                        foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                        {
                            if (partialInnerJoinColumnsString != null)
                                partialInnerJoinColumnsString += " AND ";
                            foreach (MetaDataRepository.IdentityPart referenceColumn in referenceObjectIdentityType.Parts)
                            {
                                if (referenceColumn.PartTypeName == column.PartTypeName)
                                {
                                    partialInnerJoinColumnsString += "parent." + column.Name + " = child." + referenceColumn.Name;
                                    break;
                                }
                            }
                        }
                        innerJoinColumnsString += partialInnerJoinColumnsString + ")";
                    }
                }
            }

            foreach (string dataColumnName in columns)
            {
                if (dataColumnsString != null)
                    dataColumnsString += ",";
                dataColumnsString += GetSQLScriptForName(dataColumnName);


                if (parentDataColumnsString != null)
                    parentDataColumnsString += ",";
                parentDataColumnsString += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataColumnName);




                if (childDataColumnsString != null)
                    childDataColumnsString += ",";
                childDataColumnsString += "child." + dataColumnName;





            }

            string InnerJoinAttributes = BuildJoinFilter("parent", leftJoinTableColumns, AliasAssociationTableName, rightJoinTableColumns);


            //string RecursiveDataSource = "select " + parentDataColumnsString + "," + objectIDColumnsString + "," + refernceColumnsString + " from (" + loadDataSQLQuery + ") " + DataNode.Alias + " LEFT OUTER JOIN " + AssociationTableStatement + " " + AliasAssociationTableName + " ON " + InnerJoinAttributes;
            string RecursiveDataSource = "select " + parentDataColumnsString + "," + objectIDColumnsString + "," + relationTableRefernceColumnsString + " FROM (" + (DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).SQLStatament + ") " + DataNode.Alias + BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Left, AssociationTableStatement, DataNode.Alias, leftJoinTableColumns, AliasAssociationTableName, rightJoinTableColumns) + " AND " + BuildJoinFilter(DataNode.Alias, leftJoinTableColumns, AliasAssociationTableName, rightJoinTableColumns, "<>");

            string RecursiveDataSourceB = "select  " + parentDataColumnsString + "," + objectIDColumnsString + "," + relationTableRefernceColumnsString + "  FROM  " + BuildClassifierDataRetrieveSQLScript() + "  " + DataNode.Alias + BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner, AssociationTableStatement, DataNode.Alias, leftJoinTableColumns, AliasAssociationTableName, rightJoinTableColumns); ;
            //break;




            //WHERE     ([StartPointObjectIDFilter])

            string recursiveSQLQuery = @"WITH TemporaryTable(#RefernceColumns#, #ObjectIDColumns#, iteration,#DataColumns#) 
                AS (
                SELECT     #ParentRefernceColumns#, #ParentObjectIDColumns#, 0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" iteration,#ParentDataColumns#
                FROM         #RecursiveDataSource# 
                UNION ALL
                SELECT     #ChildRefernceColumns#, #ChildObjectIDColumns#, parent.iteration + 1 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" iteration,#ChildDataColumns#
                FROM       TemporaryTable " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" parent INNER JOIN
                            #RecursiveDataSourceB# " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" child ON #InnerJoinColumns#
                WHERE     (parent.iteration < #RecursiveStep#))

                SELECT    #StorageIdentityNumber# " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("OSM_StorageIdentity") + @", #RefernceColumns#, #ObjectIDColumns#, iteration,#DataColumns#
                FROM         TemporaryTable";


            string whereClause = null;


            recursiveSQLQuery = recursiveSQLQuery.Replace("#ObjectIDColumns#", objectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RefernceColumns#", refernceColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#StorageIdentityNumber#", DataLoader.DataLoaderMetadata.QueryStorageID.ToString());


            if (DataLoader.SearchCondition != null)
                whereClause = SQLFilterScriptBuilder.GetSQLFilterStatament(DataLoader.SearchCondition);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " WHERE " + whereClause;

            //temp += "\r\n" + whereClause;



            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveDataSource#", "(" + RecursiveDataSource + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias));
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveDataSourceB#", "(" + RecursiveDataSourceB + ")");
            recursiveSQLQuery = recursiveSQLQuery.Replace("#DataColumns#", dataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ParentDataColumns#", parentDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ParentObjectIDColumns#", parentObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ParentRefernceColumns#", parentRefernceColumnsString);


            recursiveSQLQuery = recursiveSQLQuery.Replace("#ChildDataColumns#", childDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ChildObjectIDColumns#", childObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ChildRefernceColumns#", childRefernceColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#InnerJoinColumns#", innerJoinColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveStep#", DataNode.RecursiveSteps.ToString());
            return recursiveSQLQuery;


        }
        /// <MetaDataID>{259b1bcb-27e7-4c1a-b722-ccb5113c8083}</MetaDataID>
        string BuildRecursiveLoadOneToMany(System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {

            string selectListColumns = null;
            string childColumns = null;
            string parentColumnsSelectionList = null;
            string innerJoinColumnsString = null;
            List<string> parentColumns = new List<string>();
            foreach (DataLoader.DataColumn column in (DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).ClassifierDataColumns)
                parentColumns.Add(column.Name);

            List<string> columns = new List<string>();

            foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
                columns.Add(dataColumn.Name);

            MetaDataRepository.Classifier classifier = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;

            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> referencColumns = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            referencColumns[associationEnd.Identity.ToString()] = associationEnd.GetReferenceObjectIdentityTypes(DataLoader.ObjectIdentityTypes);
            Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> objectIDColumns = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            objectIDColumns[associationEnd.Identity.ToString()] = DataLoader.ObjectIdentityTypes;

            selectListColumns = GetSQLScriptForName("iteration");
            parentColumnsSelectionList = "0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + " " + GetSQLScriptForName("iteration");
            childColumns += GetSQLScriptForName("parent") + "." + GetSQLScriptForName("iteration") + "+1 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + " " + GetSQLScriptForName("iteration");
            foreach (string relationPartIdentity in objectIDColumns.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in objectIDColumns[relationPartIdentity])
                {
                    foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                    {
                        columns.Remove(column.Name);
                        selectListColumns += ",";
                        selectListColumns += GetSQLScriptForName(column.Name);
                        parentColumnsSelectionList += ",";
                        parentColumnsSelectionList += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(column.Name);
                        childColumns += ",";
                        childColumns += GetSQLScriptForName("child") + "." + GetSQLScriptForName(column.Name);
                    }
                }
            }
            foreach (string relationPartIdentity in referencColumns.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referencColumns[relationPartIdentity])
                {
                    foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                    {
                        columns.Remove(column.Name);
                        selectListColumns += "," + GetSQLScriptForName(column.Name);
                        parentColumnsSelectionList += "," + GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(column.Name);
                        childColumns += "," + GetSQLScriptForName("child") + "." + GetSQLScriptForName(column.Name);
                    }
                }
            }


            foreach (string dataColumnName in columns)
            {

                selectListColumns += "," + GetSQLScriptForName(dataColumnName);
                parentColumnsSelectionList += "," + GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataColumnName);

                childColumns += "," + GetSQLScriptForName("child") + "." + GetSQLScriptForName(dataColumnName);

            }
            innerJoinColumnsString = BuildJoinFilter("parent", objectIDColumns, "child", referencColumns);
            string recursiveSQLQuery = @"WITH TemporaryTable(#ColumnsList#) 
                                        AS (
                                        SELECT     #ParentColumnsList# 
                                        FROM       #RecursiveDataSource# 
                                        UNION ALL
                                        SELECT     #ChildColumnsList# 
                                        FROM       TemporaryTable " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" parent INNER JOIN
                                                    #RecursiveDataSourceB# " + RDBMSSQLScriptGenarator.AliasDefSqlScript + @" child ON #InnerJoinColumns#
                                        WHERE     (parent.iteration < #RecursiveStep#))
                                        SELECT     #StorageIdentityNumber# " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("OSM_StorageIdentity") + @",#ColumnsList# 
                                        FROM       TemporaryTable";

            string whereClause = null;
            if (DataLoader.SearchCondition != null)
                whereClause = SQLFilterScriptBuilder.GetSQLFilterStatament(DataLoader.SearchCondition);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " WHERE " + whereClause;

            recursiveSQLQuery = recursiveSQLQuery.Replace("#StorageIdentityNumber#", DataLoader.DataLoaderMetadata.QueryStorageID.ToString());
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ColumnsList#", selectListColumns);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ParentColumnsList#", parentColumnsSelectionList);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#ChildColumnsList#", childColumns);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveDataSource#", "(" + (GetDataLoader(DataNode.RealParentDataNode) as DataLoader).SQLStatament + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias));
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveDataSourceB#", BuildClassifierDataRetrieveSQLScript());
            recursiveSQLQuery = recursiveSQLQuery.Replace("#InnerJoinColumns#", innerJoinColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("#RecursiveStep#", DataNode.RecursiveSteps.ToString());
            return recursiveSQLQuery;

        }

        ///<summary>
        ///Returns the data loader for data node of parameter which retrieve data from the same objecct context with the called storage dataloader 
        ///</summary>
        ///<param name="dataNode">
        ///Defines the data node of data loader where operation calller wants
        ///</param>
        /// <MetaDataID>{6dba7461-b42e-4fdd-a935-6792f6ab560f}</MetaDataID>
        protected OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader GetDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                return GetDataLoader(dataNode.ParentDataNode);
            MetaDataRepository.ObjectQueryLanguage.DataLoader dataLoader = null;
            dataNode.DataSource.DataLoaders.TryGetValue(DataLoader.DataLoaderMetadata.ObjectsContextIdentity, out dataLoader);
            return dataLoader;
        }


        /// <summary >Create the sql script for the relation table with the relation data for the relation between the data lodater and related data node data loader</summary>
        /// <MetaDataID>{6b5c0b4c-7b86-4d7c-a096-f42b0d8894ee}</MetaDataID>
        internal string BulidRelationTableSQlScript(DataNode relatedDataNode)
        {

            Dictionary<DataNode, bool> relatedDataNodes = new Dictionary<DataNode, bool>();
            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            if (relatedDataNode == DataNode.RealParentDataNode)
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;

            string sqlScript = null;
            string associationTableName = null;
            sqlScript = BuildManyToManyTablesJoin(relatedDataNode, true, relatedDataNodes, true, out associationTableName);

            if (string.IsNullOrEmpty(sqlScript))
                return null;

            //string filter = null;
            string orderByClause = null;


            string selectList = null;
            if (relatedDataNode != DataNode.RealParentDataNode)
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity].Keys)
                {
                    associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity][relationPartIdentity].Keys)))
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (selectList == null)
                                selectList = "SELECT DISTINCT " + GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName("RoleAStorageIdentity") + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName("RoleAStorageIdentity") + ", ";
                            else
                                selectList += ",";
                            selectList += GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(part.Name);
                        }
                    }
                }
                foreach (string relationPartIdentity in GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithParent.Keys)
                {
                    associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys)))
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (selectList == null)
                                selectList = "SELECT DISTINCT " + GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName("RoleAStorageIdentity") + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName("RoleAStorageIdentity") + ", ";
                            else
                                selectList += ",";
                            selectList += GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(part.Name);

                        }
                    }
                }
            }
            else
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                {
                    associationEnd = (((Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd).GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd);
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys)))
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (selectList == null)
                                selectList = "SELECT DISTINCT " + GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName("RoleAStorageIdentity") + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName("RoleAStorageIdentity") + ", ";
                            else
                                selectList += ",";
                            selectList += GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(part.Name);
                        }
                    }
                }
                foreach (string relationPartIdentity in GetDataLoader(DataNode.RealParentDataNode).DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity].Keys)
                {
                    associationEnd = (((Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd).GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd);

                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(DataNode.RealParentDataNode).DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys)))
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (selectList == null)
                                selectList = "SELECT DISTINCT " + GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName("RoleAStorageIdentity") + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName("RoleAStorageIdentity") + ", ";
                            else
                                selectList += ",";
                            selectList += GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(part.Name);

                        }
                    }
                }

            }
            selectList += ", " + GetSQLScriptForName(associationTableName) + "." + GetSQLScriptForName("RoleBStorageIdentity") + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName("RoleBStorageIdentity");
            if (relatedDataNode == DataNode.RealParentDataNode)
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;

            if (associationEnd.Indexer)
            {
                selectList += "," + associationEnd.IndexerColumn.Name;
                orderByClause = " ORDER BY " + associationEnd.IndexerColumn.Name;
            }
            return selectList + " FROM " + sqlScript + /*" WHERE " + filter +*/ orderByClause;
        }

        /// <MetaDataID>{23b80ce9-5df2-4e6a-bcc8-8d63c7b75540}</MetaDataID>
        public string GetSQLScriptValidAlias(string objectLifeTimeManagerName)
        {
            if (DataLoader.OrgNamesDictionary.ContainsKey(objectLifeTimeManagerName))
                return GetSQLScriptForName(DataLoader.OrgNamesDictionary[objectLifeTimeManagerName]);
            else
            {
                string validAlias = RDBMSSQLScriptGenarator.GeValidRDBMSName(objectLifeTimeManagerName, new List<string>(DataLoader.AliasesDictionary.Keys));
                DataLoader.AliasesDictionary[validAlias] = objectLifeTimeManagerName;
                DataLoader.OrgNamesDictionary[objectLifeTimeManagerName] = validAlias;
                return GetSQLScriptForName(validAlias);
            }
        }

        /// <MetaDataID>{a14899a3-6189-41ed-99da-ce0dd118276c}</MetaDataID>
        internal string SelectClause
        {
            get
            {
                if (DataNode.ObjectQuery.SelectListItems.Count == 1 &&
                    DataNode.ObjectQuery.SelectListItems[0].Type == DataNode.DataNodeType.Count &&
                    (DataNode.ObjectQuery.SelectListItems[0].ParentDataNode == DataNode &&
                    (DataNode.ObjectQuery.SelectListItems[0] as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count == 0))
                {
                    return "SELECT COUNT(*) " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.ObjectQuery.SelectListItems[0].Alias);
                }

                string distinctClause = "";
                if (DataLoader.DisctionResolveOnRdbms)
                    distinctClause = " DISTINCT ";
                string selectClause = "SELECT " + distinctClause + SelectClauseColumnsList;
                foreach (DataLoader hostedDataDataLoader in DataLoader.HostedDataDataLoaders)
                    selectClause += "," + hostedDataDataLoader.SqlScriptsBuilder.SelectClauseColumnsList;

                return selectClause;

            }
        }

        /// <MetaDataID>{d2f018bb-4878-4fac-bcc5-bceade8fd4c8}</MetaDataID>
        internal string DataLoaderColumnsList
        {
            get
            {
                DataNode tableOwnerDataNode = DataNode;
                while (tableOwnerDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    tableOwnerDataNode = tableOwnerDataNode.ParentDataNode;
                string tableAlias = tableOwnerDataNode.Alias;
                return GetDataLoaderColumnList(tableAlias);
            }
        }

        internal CreatorIdentity GetDataLoaderColumnList(string tableAlias)
        {
            string selectClause = null;
            if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                #region Select clause for group
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (selectClause != null)
                                    selectClause += ",";
                                selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(dataNode.Alias + "_" + part.Name);
                            }
                        }
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (selectClause != null)
                            selectClause += ",";
                        if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                            selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name);
                        else
                            selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));// GetSQLScriptForName(dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                    }
                }
                if (!(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode.RealParentDataNode) && DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (selectClause != null)
                                selectClause += ",";
                            selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(DataNode.RealParentDataNode.Alias + "_" + part.Name); ;
                        }
                    }
                }

                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                    if (subDataNode is AggregateExpressionDataNode)
                        selectClause += ", " + GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias(subDataNode.Alias);
                selectClause += ", " + GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias("OSM_StorageIdentity");

                return selectClause;
                #endregion
            }
            else
            {




                foreach (DataLoader.DataColumn column in DataLoader.ClassifierDataColumns)
                {
                    if (selectClause == null)
                    {
                        if (!(DataLoader.DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute)))
                            selectClause = GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias("OSM_StorageIdentity") + ",";
                    }
                    else
                        selectClause += ",";
                    if (column.Alias != null)
                        selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias(column.Alias);
                    else
                        selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(column.Name);
                }


                foreach (DataNode memberDataNode in DataNode.SubDataNodes)
                {
                    if (memberDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        memberDataNode.SubDataNodes.Count > 0 &
                        memberDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                    {
                        foreach (DataNode dateMemberDataNode in memberDataNode.SubDataNodes)
                            selectClause += "," + GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias(memberDataNode.Name + "_" + dateMemberDataNode.Name);
                    }
                    if (memberDataNode.Type == DataNode.DataNodeType.Group)
                        continue;
                    if (memberDataNode is AggregateExpressionDataNode)
                        selectClause += "," + GetSQLScriptForName(tableAlias) + "." + GetSQLScriptValidAlias(memberDataNode.Alias);
                }
                #region RowRemove code
                //if (DataLoader.SearchCondition != null && !string.IsNullOrEmpty(GetRowRemoveCaseStatament(DataLoader.SearchCondition)))
                //    selectClause += GeValidName(DataSource.GetRowRemoveColumnName(DataNode, DataLoader.SearchCondition));

                //foreach (SearchCondition searchCondition in DataLoader.SearchConditions)
                //{
                //    if (searchCondition != DataLoader.SearchCondition && !string.IsNullOrEmpty(GetRowRemoveCaseStatament(searchCondition)))
                //        selectClause += GeValidName(DataSource.GetRowRemoveColumnName(DataNode, DataLoader.SearchCondition));
                //}
                #endregion

                foreach (DataLoader hostedDataDataLoader in DataLoader.HostedDataDataLoaders)
                    selectClause += "," + hostedDataDataLoader.SqlScriptsBuilder.GetDataLoaderColumnList(tableAlias);

                return selectClause;
            }
        }

        /// <MetaDataID>{79dfee93-3b68-4657-a447-22afa5857d58}</MetaDataID>
        private string SelectClauseColumnsList
        {
            get
            {

                string selectClause = null;

                #region Select clause for group
                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        if (dataNode.Type == DataNode.DataNodeType.Object)
                        {
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (selectClause != null)
                                        selectClause += ",";
                                    selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataNode.Alias + "_" + part.Name);
                                }
                            }
                        }
                        else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            if (selectClause != null)
                                selectClause += ",";
                            if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                                selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name);
                            else
                                selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));// GetSQLScriptForName(dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                        }
                    }
                    if (!(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode.RealParentDataNode) && DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (selectClause != null)
                                    selectClause += ",";
                                selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataNode.RealParentDataNode.Alias + "_" + part.Name); ;
                            }
                        }
                    }

                    foreach (DataNode memberDataNode in DataNode.SubDataNodes)
                    {
                        if (memberDataNode is AggregateExpressionDataNode)
                        {

                            string aggregateFunction = null;
                            switch (memberDataNode.Type)
                            {
                                case DataNode.DataNodeType.Sum:
                                    {
                                        aggregateFunction = "SUM";
                                        break;
                                    }
                                case DataNode.DataNodeType.Min:
                                    {
                                        aggregateFunction = "MIN";
                                        break;
                                    }
                                case DataNode.DataNodeType.Max:
                                    {
                                        aggregateFunction = "MAX";
                                        break;
                                    }
                                case DataNode.DataNodeType.Average:
                                    {
                                        aggregateFunction = "AVG";
                                        break;
                                    }
                                case DataNode.DataNodeType.Count:
                                    {
                                        aggregateFunction = "COUNT";
                                        break;
                                    }

                            }
                            if ((memberDataNode as AggregateExpressionDataNode).ArithmeticExpression == null)
                            {
                                string aggregationSQLScript = null;

                                foreach (DataNode aggregateExpressionDataNode in (memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                                {
                                    string aggregateExpression = null;
                                    if (aggregateExpressionDataNode.RealParentDataNode != null && aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                            (aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                    {
                                        RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                                        string columnName = attribute.GetAttributeColumnName(aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                                        aggregateExpression += columnName;
                                    }
                                    else
                                    {
                                        if ((memberDataNode as AggregateExpressionDataNode).Type != DataNode.DataNodeType.Count)
                                            aggregateExpression += (aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name;
                                    }
                                    //string aggregate = 
                                    if (!string.IsNullOrEmpty(selectClause))
                                        selectClause += ",";

                                    if ((memberDataNode as AggregateExpressionDataNode).Type == DataNode.DataNodeType.Count)
                                        aggregationSQLScript += aggregateFunction + "(*)";
                                    else
                                        aggregationSQLScript += aggregateFunction + "(" + GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(aggregateExpressionDataNode)) + ")";
                                    break;
                                }

                                if ((memberDataNode as AggregateExpressionDataNode).SourceSearchCondition != null)
                                {
                                    DataNode rootDataNode = (DataNode as GroupDataNode).GroupedDataNodeRoot;
                                    if (rootDataNode.RealParentDataNode != null)
                                        rootDataNode = rootDataNode.RealParentDataNode;

                                    System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                                    DataNode parentDataNode = rootDataNode;
                                    foreach (DataNode aggregateExpressionDataNode in (memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                                    {
                                        parentDataNode = aggregateExpressionDataNode;
                                        while (parentDataNode != rootDataNode)
                                        {
                                            if (!relatedDataNodes.ContainsKey(parentDataNode))
                                                relatedDataNodes.Add(parentDataNode, true);
                                            parentDataNode = parentDataNode.RealParentDataNode;
                                        }
                                    }
                                    if (DataNode.Type == DataNode.DataNodeType.Group && DataNode.ParentDataNode != null)
                                        relatedDataNodes[DataNode.RealParentDataNode] = true;


                                    string fromClauseSQLQuery = null;
                                    string joinWhereAttributes = null;
                                    DataLoader rootDataLoader = GetDataLoader(rootDataNode) as DataLoader;
                                    foreach (DataNode subDataNode in rootDataNode.SubDataNodes)
                                    {
                                        if (!relatedDataNodes.ContainsKey(subDataNode))
                                            continue;
                                        bool innerJoin = true;
                                        relatedDataNodes.Remove(subDataNode);

                                        #region Build table join with sub data node if needed.
                                        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                                        {
                                            MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                            if (subDataNode.ThroughRelationTable)
                                                fromClauseSQLQuery += rootDataLoader.SqlScriptsBuilder.BuildManyToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                            else if (associationEnd.Association.LinkClass != null &&
                                                    (associationEnd.Association.LinkClass == subDataNode.Classifier ||                        //is type of AssociationClass.Class
                                                    associationEnd.Association.LinkClass == DataNode.Classifier))							 //is type of Class.AssociationClass
                                                fromClauseSQLQuery += rootDataLoader.SqlScriptsBuilder.BuildAssociationClassTablesJoin(subDataNode, relatedDataNodes, innerJoin);///TODO Test Case for aggregation on association class
                                            else if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                                                fromClauseSQLQuery += rootDataLoader.SqlScriptsBuilder.BuildManyToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                            else
                                                fromClauseSQLQuery += rootDataLoader.SqlScriptsBuilder.BuildOneToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                        }
                                        else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                        {
                                            fromClauseSQLQuery += rootDataLoader.SqlScriptsBuilder.BuildOneToManyTablesJoin(subDataNode, relatedDataNodes, true); ///TODO Test Case for aggregation on value type
                                        }

                                        #endregion
                                    }

                                    string whereClauseSQLQuery = "(" + joinWhereAttributes + ")";
                                    string selectListItem = null;
                                    if ((memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count > 0)
                                        selectListItem += "\r\nFROM " + fromClauseSQLQuery + " WHERE " + whereClauseSQLQuery;
                                    string aggregateExpression = null;
                                    aggregateExpression = DataLoader.GetArithmeticTermSqlScript((memberDataNode as AggregateExpressionDataNode).ArithmeticExpression);
                                    if (memberDataNode.Type == DataNode.DataNodeType.Count)
                                        aggregateExpression = "(*) ";
                                    selectClause += "( SELECT " + aggregateFunction + aggregateExpression + selectListItem + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);

                                }
                                else
                                {
                                    selectClause += aggregationSQLScript;
                                    selectClause += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);
                                }




                                //string aggregateExpression = null;
                                //aggregateExpression = DataLoader.GetArithmeticTermSqlScript((memberDataNode as AggregateExpressionDataNode).ArithmeticExpression);
                                //if (memberDataNode.Type == DataNode.DataNodeType.Count)
                                //    aggregateExpression = "(*) ";
                                //selectClause += "( SELECT " + aggregateFunction + aggregateExpression + selectListItem + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);



                            }
                            else
                            {
                                string expression = DataLoader.GetArithmeticTermSqlScript((memberDataNode as AggregateExpressionDataNode).ArithmeticExpression);
                                string aggregationSQLScript = null;
                                if (!string.IsNullOrEmpty(selectClause))
                                    selectClause += ",";
                                aggregationSQLScript = aggregateFunction + "(" + expression + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);



                                if ((memberDataNode as AggregateExpressionDataNode).SourceSearchCondition != null)
                                {
                                    System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                                    DataNode parentDataNode = (DataNode as GroupDataNode).GroupedDataNodeRoot;
                                    foreach (DataNode aggregateExpressionDataNode in (memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                                    {
                                        parentDataNode = aggregateExpressionDataNode;
                                        while (parentDataNode != (DataNode as GroupDataNode).GroupedDataNodeRoot)
                                        {
                                            if (!relatedDataNodes.ContainsKey(parentDataNode))
                                                relatedDataNodes.Add(parentDataNode, true);
                                            parentDataNode = parentDataNode.RealParentDataNode;
                                        }
                                    }

                                }
                                selectClause += aggregationSQLScript;

                            }
                        }
                    }
                    selectClause = selectClause + ", " + DataLoader.DataLoaderMetadata.QueryStorageID.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("OSM_StorageIdentity");
                    return selectClause;
                }
                #endregion


                DataNode tableOwnerDataNode = DataNode;
                while (tableOwnerDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    tableOwnerDataNode = tableOwnerDataNode.ParentDataNode;
                string tableAlias = tableOwnerDataNode.Alias;
                foreach (DataLoader.DataColumn column in DataLoader.ClassifierDataColumns)
                {


                    if (selectClause == null)
                    {
                        if (!(DataLoader.DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute)))
                            selectClause = DataLoader.DataLoaderMetadata.QueryStorageID.ToString() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias("OSM_StorageIdentity") + ",";
                    }
                    else
                        selectClause += ",";
                    selectClause += GetSQLScriptForName(tableAlias) + "." + GetSQLScriptForName(column.Name);
                    if (column.Alias != null)
                        selectClause += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(column.Alias);
                }

                //foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
                //{
                //    if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                //    {
                //        selectClause += "," + GetSQLScriptForName(DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName);
                //        if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
                //            selectClause += "." + GetSQLScriptForName("ContainsAny") + " ";
                //        else
                //            selectClause += "." + GetSQLScriptForName("ContainsAll") + " ";
                //    }
                //}

                foreach (DataNode memberDataNode in DataNode.SubDataNodes)
                {
                    if (memberDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        memberDataNode.SubDataNodes.Count > 0 &
                        memberDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                    {
                        foreach (DataNode dateMemberDataNode in memberDataNode.SubDataNodes)
                        {
                            selectClause += "," + RDBMSSQLScriptGenarator.GetDatePartSqlScript(dateMemberDataNode) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Name + "_" + dateMemberDataNode.Name);

                            Type datePartType = typeof(int);
                            if (dateMemberDataNode.Name == "Date")
                                datePartType = typeof(DateTime);
                            if (dateMemberDataNode.Name == "DayOfWeek")
                                datePartType = typeof(System.DayOfWeek);

                            DataLoader.DerivedDataColoumns.Add(new DataLoader.DataColumn(memberDataNode.Name + "_" + dateMemberDataNode.Name, null, datePartType, null, null, null, null, DataLoader.DataNode.ObjectQuery));
                        }

                    }
                    if (memberDataNode.Type == DataNode.DataNodeType.Group)
                        continue;
                    if (memberDataNode is AggregateExpressionDataNode)
                    {
                        if (DataLoader.CheckAggregateFunctionForLocalResolve(memberDataNode as AggregateExpressionDataNode))
                        {


                            string aggregateFunction = null;
                            switch (memberDataNode.Type)
                            {
                                case DataNode.DataNodeType.Sum:
                                    {
                                        aggregateFunction = "SUM";
                                        break;
                                    }
                                case DataNode.DataNodeType.Min:
                                    {
                                        aggregateFunction = "MIN";
                                        break;
                                    }
                                case DataNode.DataNodeType.Max:
                                    {
                                        aggregateFunction = "MAX";
                                        break;
                                    }
                                case DataNode.DataNodeType.Average:
                                    {
                                        aggregateFunction = "AVG";
                                        break;
                                    }
                                case DataNode.DataNodeType.Count:
                                    {
                                        aggregateFunction = "COUNT";
                                        break;
                                    }
                            }
                            System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                            DataNode parentDataNode = DataNode;
                            foreach (DataNode aggregateExpressionDataNode in (memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                            {
                                parentDataNode = aggregateExpressionDataNode;
                                while (parentDataNode != DataNode)
                                {
                                    if (!relatedDataNodes.ContainsKey(parentDataNode))
                                        relatedDataNodes.Add(parentDataNode, true);
                                    parentDataNode = parentDataNode.RealParentDataNode;
                                }
                            }
                            string fromClauseSQLQuery = null;
                            string joinWhereAttributes = null;
                            foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                            {
                                if (!relatedDataNodes.ContainsKey(subDataNode))
                                    continue;
                                bool innerJoin = true;
                                relatedDataNodes.Remove(subDataNode);

                                #region Build table join with sub data node if needed.
                                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                                {
                                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                    if (subDataNode.ThroughRelationTable)
                                        fromClauseSQLQuery += BuildManyToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                    else if ((associationEnd.Association.LinkClass == subDataNode.Classifier) ||                        //is type of AssociationClass.Class
                                            (associationEnd.Association.LinkClass == DataNode.Classifier))								//is type of Class.AssociationClass
                                        fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, relatedDataNodes, innerJoin);///TODO Test Case for aggregation on association class
                                    else if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                                        fromClauseSQLQuery += BuildManyToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                    else
                                        fromClauseSQLQuery += BuildOneToManyTablesJoinForAgregation(memberDataNode as AggregateExpressionDataNode, subDataNode, relatedDataNodes, out joinWhereAttributes);
                                }
                                else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                {
                                    fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, relatedDataNodes, true); ///TODO Test Case for aggregation on value type
                                }
                                #endregion
                            }
                            //string whereClauseSQLQuery = null;// whereClause;
                            //if (string.IsNullOrEmpty(whereClauseSQLQuery))
                            //    whereClauseSQLQuery += "\r\nWHERE ";

                            string whereClauseSQLQuery = "\r\nWHERE (" + joinWhereAttributes + ")";
                            string selectListItem = null;
                            if ((memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count > 0)
                                selectListItem += "\r\nFROM " + fromClauseSQLQuery + whereClauseSQLQuery;
                            if (selectClause == null)
                            {
                                if ((memberDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count > 0)
                                    selectClause = GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName("OSM_StorageIdentity") + ",";
                                else
                                    selectClause = "SELECT ";
                            }
                            else
                                selectClause += ",";
                            string aggregateExpression = null;
                            aggregateExpression = DataLoader.GetArithmeticTermSqlScript((memberDataNode as AggregateExpressionDataNode).ArithmeticExpression);
                            if (memberDataNode.Type == DataNode.DataNodeType.Count)
                                aggregateExpression = "(*) ";
                            selectClause += "( SELECT " + aggregateFunction + aggregateExpression + selectListItem + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);

                            //string tt = 
                        }
                        else
                            selectClause += ",0.0 " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(memberDataNode.Alias);
                    }
                }

                //if (DataLoader.SearchCondition != null)
                //    selectClause += GetRowRemoveCaseStatament(DataLoader.SearchCondition);
                #region RowRemove code
                //foreach (SearchCondition searchCondition in DataLoader.SearchConditions)
                //{
                //    if (searchCondition != DataLoader.SearchCondition)
                //        selectClause += GetRowRemoveCaseStatament(searchCondition);

                //}
                #endregion

                return selectClause;
            }
        }

        /// <MetaDataID>{a48427b3-1f38-46ad-9bc5-c9c4955cf09b}</MetaDataID>
        internal string GroupResultFilterConditionDataSqlScript
        {
            get
            {
                if (DataNode.Type == DataNode.DataNodeType.Group)
                    return DataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(DataLoader.GroupResultsRelatedDataNodes);
                else
                    return "";
            }
        }

        /// <MetaDataID>{0a889ab8-3dd2-420d-9556-a54004f350f3}</MetaDataID>
        internal string FromClause
        {
            get
            {
                string fromClause = null;
                if (DataNode.Type == DataNode.DataNodeType.Group)
                {



                    DataLoader rootDataLoader = GetDataLoader((DataNode as GroupDataNode).GroupedDataNodeRoot) as DataLoader;
                    fromClause = " FROM " + rootDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(rootDataLoader.DataNode.Alias);
                    fromClause += rootDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(DataLoader.RelatedDataNodes) + " ";

                    string tmp = DataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(DataLoader.GroupResultsRelatedDataNodes);
                    System.Collections.Generic.List<DataNode> aggregateExpressionDataNodes = new List<DataNode>();
                    string selectClause = GroupColumnsSelectionList;

                    if ((DataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader((DataNode as GroupDataNode).GroupedDataNode).ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (selectClause != null)
                                    selectClause += ",";
                                selectClause += GetSQLScriptForName((DataNode as GroupDataNode).GroupedDataNode.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName((DataNode as GroupDataNode).GroupedDataNode, part));// .RealParentDataNode.Alias + "_" + part.Name); 
                            }
                        }
                        if ((DataNode as GroupDataNode).GroupedDataNodeRoot != (DataNode as GroupDataNode).GroupedDataNode)
                        {
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader((DataNode as GroupDataNode).GroupedDataNodeRoot).ObjectIdentityTypes)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (selectClause != null)
                                        selectClause += ",";
                                    selectClause += GetSQLScriptForName((DataNode as GroupDataNode).GroupedDataNodeRoot.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName((DataNode as GroupDataNode).GroupedDataNodeRoot, part));// .RealParentDataNode.Alias + "_" + part.Name); 
                                }
                            }
                        }
                    }

                    else if (!(DataNode as GroupDataNode).GroupKeyDataNodes.Contains((DataNode as GroupDataNode).GroupedDataNodeRoot) &&
                        ((DataNode as GroupDataNode).GroupedDataNodeRoot != DataNode.RealParentDataNode || DataNode.RealParentDataNode.Type != MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object))
                    {

                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader((DataNode as GroupDataNode).GroupedDataNodeRoot).ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (selectClause != null)
                                    selectClause += ",";
                                selectClause += GetSQLScriptForName((DataNode as GroupDataNode).GroupedDataNodeRoot.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName((DataNode as GroupDataNode).GroupedDataNodeRoot, part));// .RealParentDataNode.Alias + "_" + part.Name); 
                            }
                        }
                    }

                    foreach (DataNode subDataNode in DataNode.SubDataNodes)
                    {
                        if (subDataNode is AggregateExpressionDataNode)
                        {
                            foreach (DataNode dataNode in (subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                            {
                                if (!aggregateExpressionDataNodes.Contains(dataNode))
                                    aggregateExpressionDataNodes.Add(dataNode);
                            }
                        }
                    }
                    foreach (DataNode dataNode in aggregateExpressionDataNodes)
                    {
                        if (!(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(dataNode)) //already added columns for group key dataNode
                        {
                            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                if (!string.IsNullOrEmpty(selectClause))
                                    selectClause += ",";
                                DataNode dataLoaderDataNode = dataNode.RealParentDataNode;
                                while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                                if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                    selectClause += RDBMSSQLScriptGenarator.GetDatePartSqlScript(dataNode) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode));
                                else
                                    selectClause += GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).GetColumn(dataNode).Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + DataSource.GetDataTreeUniqueColumnName(dataNode); ;
                            }
                            else if (dataNode.Type == DataNode.DataNodeType.Object && GetDataLoader(dataNode) != null && dataNode != (DataNode as GroupDataNode).GroupedDataNodeRoot)
                            {
                                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(dataNode).ObjectIdentityTypes)
                                {
                                    foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                    {
                                        if (!string.IsNullOrEmpty(selectClause))
                                            selectClause += ",";
                                        selectClause += GetSQLScriptForName(dataNode.Alias) + "." + GetSQLScriptForName(identityPart.PartTypeName);
                                        selectClause += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode, identityPart));
                                    }
                                }
                            }
                            else if (dataNode is AggregateExpressionDataNode)
                            {
                                if (!string.IsNullOrEmpty(selectClause))
                                    selectClause += ",";
                                DataNode dataLoaderDataNode = dataNode.ParentDataNode;
                                selectClause += GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + dataNode.Alias + RDBMSSQLScriptGenarator.AliasDefSqlScript + DataSource.GetDataTreeUniqueColumnName(dataNode); ;
                            }
                        }

                    }
                    selectClause = "SELECT DISTINCT " + selectClause;
                    string whereClause = null;

                    var searchCondition = (DataNode as GroupDataNode).GroupingSourceSearchCondition;// GetSearchConditionGroupingDataSource(SearchCondition);

                    if (searchCondition != null)
                        whereClause = SQLFilterScriptBuilder.GetSQLFilterStatament(searchCondition);
                    if (!string.IsNullOrEmpty(whereClause))
                        whereClause = " WHERE " + whereClause;
                    fromClause = " FROM (" + selectClause + " " + fromClause + whereClause + " ) " + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias);
                    return fromClause;
                }
                else
                {
                    fromClause = " FROM " + BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias);
                    var relatedDataNodes = DataLoader.RelatedDataNodes;
                    fromClause += BuildJoinedTablesSQLScript(relatedDataNodes) + " ";
                    foreach (DataNode subDataNode in DataNode.SubDataNodes)
                    {
                        if (relatedDataNodes.ContainsKey(subDataNode))
                        {
                            bool innerJoin = relatedDataNodes[subDataNode];
                            relatedDataNodes.Remove(subDataNode);
                            fromClause += BuildOneToManyTablesJoin(subDataNode, relatedDataNodes, innerJoin);
                        }
                    }

                    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in DataLoader.LocalResolvedCriterions.Keys)
                    {
                        if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                        {
                            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> tableJoinObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                            tableJoinObjectIdentityTypes[DataNode.AssignedMetaObject.Identity.ToString()] = DataLoader.ObjectIdentityTypes;
                            string containsDataName = DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName;
                            fromClause += BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Left,
                                            "(" + GetContainsAnyAllSQL(criterion) + ")" + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(containsDataName) + " ",
                                            DataNode.Alias,
                                            tableJoinObjectIdentityTypes,
                                            containsDataName,
                                            tableJoinObjectIdentityTypes);
                        }
                    }
                }
                return fromClause;
            }
        }


        /// <MetaDataID>{20129bbd-4fcb-4b26-bafc-dcb95cf3fc31}</MetaDataID>
        internal string WhereClause
        {
            get
            {
                string whereClause = null;
                if (!DataNode.FilterNotActAsLoadConstraint)
                {
                    if (DataLoader.SearchCondition != null)
                    {
                        if (DataNode.Type != DataNode.DataNodeType.Group)
                            whereClause = SQLFilterScriptBuilder.GetSQLFilterStatament(DataLoader.SearchCondition);
                        else
                        {
                            var searchCondition = (DataNode as GroupDataNode).GroupingResultSearchCondition;// GetSearchConditionForGroupedData(SearchCondition);
                            if (searchCondition != null)
                                whereClause = SQLFilterScriptBuilder.GetSQLFilterStatament(searchCondition);
                        }

                    }
                    if (!string.IsNullOrEmpty(whereClause))
                        whereClause = " WHERE " + whereClause;
                    else
                        whereClause = null;
                }
                return whereClause;
            }
        }



        /// <MetaDataID>{db92f502-c00a-4265-9ff0-45219ca8a051}</MetaDataID>
        internal string GroupByClause
        {
            get
            {
                string groupByClause = null;
                if (!string.IsNullOrEmpty(GroupColumnsNamesList))
                {
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        groupByClause = GroupColumnsNamesList;
                        groupByClause = " GROUP BY " + groupByClause;
                    }
                }
                return groupByClause;
            }
        }
        /// <MetaDataID>{e3ef9c60-ea23-42ee-b305-ea65b1b0ca02}</MetaDataID>
        protected string GroupColumnsNamesList
        {
            get
            {
                string groupByClause = null;
                if (DataNode.Type == DataNode.DataNodeType.Group && (DataNode as GroupDataNode).GroupKeyDataNodes.Count > 0)
                {
                    foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        //DataNode dataLoaderDataNode = dataNode.ParentDataNode;
                        //while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        //    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                        if (dataNode.Type == DataNode.DataNodeType.Object)
                        {
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (groupByClause != null)
                                        groupByClause += ",";
                                    groupByClause += GetSQLScriptForName(DataNode.Alias) + "." + DataSource.GetDataTreeUniqueColumnName(dataNode, part);// GetSQLScriptForName(dataNode.Alias + "_" + column.Name);
                                }
                            }
                        }
                        else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            if (groupByClause != null)
                                groupByClause += ",";
                            //if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            //    groupByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(GetDataTreeUniqueColumnName(dataNode));
                            //else
                            groupByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));//dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                        }
                    }
                    if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (groupByClause != null)
                                    groupByClause += ",";
                                groupByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(DataNode.RealParentDataNode, part));
                            }
                        }
                    }
                }
                return groupByClause;
            }
        }


        internal string GroupColumnsJoinWhereAttributes(string aggregatedDataAlias)
        {

            {
                string JoinWhereAttributes = null;
                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        if (dataNode.Type == DataNode.DataNodeType.Object)
                        {

                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (JoinWhereAttributes != null)
                                        JoinWhereAttributes += " AND ";
                                    JoinWhereAttributes += GetSQLScriptForName(DataNode.Alias) + "." + DataSource.GetDataTreeUniqueColumnName(dataNode, part);
                                    JoinWhereAttributes += " = ";
                                    //string teert = GetDataTreeUniqueColumnName(dataNode, part);
                                    JoinWhereAttributes += GetSQLScriptForName(aggregatedDataAlias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode, part));// dataNode.Alias + "_" + part.Name);
                                }
                            }
                        }
                        else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            DataNode dataLoaderDataNode = dataNode.ParentDataNode;
                            while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                            if (JoinWhereAttributes != null)
                                JoinWhereAttributes += " AND ";
                            JoinWhereAttributes += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));//dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                            JoinWhereAttributes += " = ";
                            JoinWhereAttributes += GetSQLScriptForName(aggregatedDataAlias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));//dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                        }
                    }
                    if (DataNode.RealParentDataNode != null
                        && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object
                        && !(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode.RealParentDataNode))
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (JoinWhereAttributes != null)
                                    JoinWhereAttributes += " AND ";
                                JoinWhereAttributes += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(DataNode.RealParentDataNode, part));
                                JoinWhereAttributes += " = ";
                                JoinWhereAttributes += GetSQLScriptForName(aggregatedDataAlias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(DataNode.RealParentDataNode, part));
                            }
                        }
                    }
                }
                return JoinWhereAttributes;
            }
        }


        /// <summary>
        /// Defines the columns of selection clause that needed group data
        /// </summary>
        /// <MetaDataID>{a413adea-cf9d-46d7-af01-88eec51deb1d}</MetaDataID>
        internal string GroupColumnsSelectionList
        {
            get
            {

                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    string groupColumnsSelectionList = GroupKeysColumnsSelectionList;

                    if (DataNode.RealParentDataNode != null
                        && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object
                        && !(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode.RealParentDataNode))
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (groupColumnsSelectionList != null)
                                    groupColumnsSelectionList += ",";
                                groupColumnsSelectionList += GetSQLScriptForName(DataNode.RealParentDataNode.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(DataNode.RealParentDataNode, part));// .RealParentDataNode.Alias + "_" + part.Name); 
                            }
                        }
                    }
                    return groupColumnsSelectionList;
                }
                else return null;

            }
        }

        /// <summary>
        /// Defines the columns of select clause for group keys
        /// </summary>
        private string GroupKeysColumnsSelectionList
        {
            get
            {
                string selectionList = null;
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (selectionList != null)
                                    selectionList += ",";
                                //string teert = GetDataTreeUniqueColumnName(dataNode, part);
                                selectionList += GetSQLScriptForName(dataNode.Alias) + "." + GetSQLScriptForName(part.Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode, part));// dataNode.Alias + "_" + part.Name);
                            }
                        }
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        DataNode dataLoaderDataNode = dataNode.ParentDataNode;
                        while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                            dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                        if (selectionList != null)
                            selectionList += ",";
                        if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                        {
                            selectionList += RDBMSSQLScriptGenarator.GetDatePartSqlScript(dataNode) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode));

                            Type datePartType = typeof(int);
                            if (dataNode.Name == "Date")
                                datePartType = typeof(DateTime);
                            if (dataNode.Name == "DayOfWeek")
                                datePartType = typeof(System.DayOfWeek);
                            DataLoader.DerivedDataColoumns.Add(new DataLoader.DataColumn(DataSource.GetDataTreeUniqueColumnName(dataNode), null, datePartType, null, null, null, null, DataLoader.DataNode.ObjectQuery));
                        }
                        else
                            selectionList += GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).GetColumn(dataNode).Name) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataSource.GetDataTreeUniqueColumnName(dataNode));// GetSQLScriptValidAlias(dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                    }
                }
                return selectionList;
            }
        }


        /// <MetaDataID>{a27dcab1-e86f-4d5b-ad11-1a776465eb22}</MetaDataID>
        internal string OrderByClause
        {
            get
            {
                string orderByClause = null;

                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    if (associationEnd.Indexer && DataLoader.HasRelationReferenceColumns)
                    {
                        if (orderByClause == null)
                            orderByClause = " ORDER BY ";
                        else
                            orderByClause += ",";
                        orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(associationEnd.IndexerColumn.Name);
                    }
                }
                foreach (DataNode orderByDataNode in DataNode.OrderByDataNodes)
                {
                    if (orderByDataNode.Type == DataNode.DataNodeType.OjectAttribute && orderByDataNode.OrderBy != OrderByType.None)
                    {
                        if (orderByDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            orderByDataNode.ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                        {
                            if (orderByClause == null)
                                orderByClause = " ORDER BY ";
                            else
                                orderByClause += ",";

                            if (DataNode.Type == DataNode.DataNodeType.Group && (DataNode as GroupDataNode).GroupKeyDataNodes.Contains(orderByDataNode))
                                orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(orderByDataNode.ParentDataNode.ParentDataNode.Alias + "_" + orderByDataNode.ParentDataNode.Name + "_" + orderByDataNode.Name) + " " + orderByDataNode.OrderBy.ToString();
                            else
                                orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(orderByDataNode.ParentDataNode.Name + "_" + orderByDataNode.Name) + " " + orderByDataNode.OrderBy.ToString();
                        }
                        else
                        {
                            if (orderByClause == null)
                                orderByClause = " ORDER BY ";
                            else
                                orderByClause += ",";
                            if (DataNode.Type == DataNode.DataNodeType.Group && (DataNode as GroupDataNode).GroupKeyDataNodes.Contains(orderByDataNode))
                                orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(orderByDataNode.ParentDataNode.Alias + "_" + orderByDataNode.Name) + " " + orderByDataNode.OrderBy.ToString();
                            else
                                orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(orderByDataNode.Name) + " " + orderByDataNode.OrderBy.ToString();

                        }
                    }
                    if (orderByDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                       orderByDataNode.SubDataNodes.Count > 0 &&
                        orderByDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                    {
                        if (orderByClause == null)
                            orderByClause = " ORDER BY ";
                        else
                            orderByClause += ",";
                        foreach (DataNode dateMemberDataNode in orderByDataNode.SubDataNodes)
                            orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(orderByDataNode.Name + "_" + dateMemberDataNode.Name) + " " + orderByDataNode.OrderBy.ToString();
                    }
                }


                if (!DataLoader.DisctionResolveOnRdbms)// || DataLoader.RowRemoveColumns.Count > 0) RowRemove code
                {
                    if (DataNode is GroupDataNode)
                        foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                        {
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (orderByClause == null)
                                        orderByClause = " ORDER BY ";
                                    else
                                        orderByClause += ",";
                                    orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                                }
                            }
                        }
                    else
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataLoader.ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (orderByClause == null)
                                    orderByClause = " ORDER BY ";
                                else
                                    orderByClause += ",";
                                orderByClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                            }
                        }
                }
                return orderByClause;
            }
        }

        /// <MetaDataID>{b7c87a64-fd56-42ea-89a1-3f0202159eca}</MetaDataID>
        internal string BuildJoinedTablesSQLScript(System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {
            return BuildJoinedTablesSQLScript(null, relatedDataNodes);
        }

        /// <summary>
        /// Construct the joined tables SQL script. 
        /// </summary>
        /// <param name="relatedDataNodes">
        /// Defines the related data nodes which used for construction of joined tables SQL script
        /// The value for each data node (key) defines the join type.  
        /// true for INNER JOIN and false for LEFT OUTER JOIN
        /// </param>
        /// <MetaDataID>{c6af473f-94db-4c27-be30-5f8b0cac672f}</MetaDataID>
        internal string BuildJoinedTablesSQLScript(DerivedDataNode dataLoaderDerivedDataNode, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {
            if (dataLoaderDerivedDataNode != null)
            {
                if (relatedDataNodes.ContainsKey(dataLoaderDerivedDataNode))
                    relatedDataNodes.Remove(dataLoaderDerivedDataNode);
            }
            else
            {
                if (relatedDataNodes.ContainsKey(DataNode))
                    relatedDataNodes.Remove(DataNode);
            }

            string fromClauseSQLQuery = null;
            if (DataNode.Type == DataNode.DataNodeType.Object)
            {
                System.Collections.Generic.List<DataNode> subDataNodesForJoin = new List<DataNode>();
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.RealSubDataNodes)
                {
                    MetaDataRepository.ObjectQueryLanguage.DerivedDataNode derivedSubDataNode = null;
                    if (dataLoaderDerivedDataNode != null)
                    {
                        foreach (MetaDataRepository.ObjectQueryLanguage.DerivedDataNode derivedDataNode in dataLoaderDerivedDataNode.RealSubDataNodes)
                        {
                            if (derivedDataNode.OrgDataNode == subDataNode)
                            {
                                derivedSubDataNode = derivedDataNode;
                                break;
                            }
                        }
                        if (derivedSubDataNode == null || !relatedDataNodes.ContainsKey(derivedSubDataNode))
                            continue;

                    }
                    else if (!relatedDataNodes.ContainsKey(subDataNode))
                        continue;

                    bool innerJoin = false;
                    if (derivedSubDataNode != null)
                    {
                        innerJoin = relatedDataNodes[derivedSubDataNode];
                        relatedDataNodes.Remove(derivedSubDataNode);
                    }
                    else
                    {
                        innerJoin = relatedDataNodes[subDataNode];
                        if (subDataNode.Type != DataNode.DataNodeType.Key)
                            relatedDataNodes.Remove(subDataNode);
                    }
                    if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object)
                    {

                        //The value type data hosted in non value type parent object DataNode table.
                        //System continue with subDaNodes
                        DataLoader subDataNodeDataLoader = GetDataLoader(subDataNode) as DataLoader;
                        if (subDataNodeDataLoader != null)
                            fromClauseSQLQuery += subDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(derivedSubDataNode, relatedDataNodes);
                        continue;
                    }
                    else if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        #region Build table join with sub data node if needed.
                        MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        MetaDataRepository.Association rdbmsAssociation = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd.Association) as RDBMSMetaDataRepository.Association;
                        if (subDataNode.ThroughRelationTable)// associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || rdbmsAssociation.Specializations.Count > 0)
                        {
                            string aliasAssociationTableName = null;
                            if (derivedSubDataNode != null)
                                fromClauseSQLQuery += BuildManyToManyTablesJoin(derivedSubDataNode, false, relatedDataNodes, innerJoin, out aliasAssociationTableName);
                            else
                                fromClauseSQLQuery += BuildManyToManyTablesJoin(subDataNode, false, relatedDataNodes, innerJoin, out aliasAssociationTableName);

                        }
                        else if ((associationEnd.Association.LinkClass == subDataNode.Classifier) ||	//is type of Class.AssociationClass
                                (associationEnd.Association.LinkClass == DataNode.Classifier))	//is type of AssociationClass.Class
                        {
                            if (derivedSubDataNode != null)
                                fromClauseSQLQuery += BuildAssociationClassTablesJoin(derivedSubDataNode, relatedDataNodes, innerJoin);
                            else
                                fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, relatedDataNodes, innerJoin);
                        }

                        else
                        {
                            if (derivedSubDataNode != null)
                                fromClauseSQLQuery += BuildOneToManyTablesJoin(derivedSubDataNode, relatedDataNodes, innerJoin);
                            else
                                fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, relatedDataNodes, innerJoin);

                        }
                        #endregion
                    }


                }
                DataNode parendDataNode = DataNode.RealParentDataNode;
                while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                    parendDataNode = parendDataNode.RealParentDataNode;

                if (dataLoaderDerivedDataNode == null && parendDataNode != null && relatedDataNodes.ContainsKey(parendDataNode))
                {
                    bool innerJoin = relatedDataNodes[parendDataNode];
                    relatedDataNodes.Remove(parendDataNode);
                    #region Build table join with parent data node if needed.
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        MetaDataRepository.Association rdbmsAssociation = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd.Association) as RDBMSMetaDataRepository.Association;
                        if (rdbmsAssociation.Specializations.Count > 0)
                        {
                            string aliasAssociationTableName = null;
                            fromClauseSQLQuery += BuildManyToManyTablesJoin(parendDataNode, false, relatedDataNodes, innerJoin, out aliasAssociationTableName);
                        }
                        else if ((associationEnd.Association.LinkClass == parendDataNode.Classifier) ||//is type of AssociationClass.Class
                                    (associationEnd.Association.LinkClass == DataNode.Classifier))								//is type of Class.AssociationClass
                            fromClauseSQLQuery += BuildAssociationClassTablesJoin(parendDataNode, relatedDataNodes, innerJoin);
                        else if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || DataNode.ThroughRelationTable)
                        {
                            string aliasAssociationTableName = null;
                            fromClauseSQLQuery += BuildManyToManyTablesJoin(parendDataNode, false, relatedDataNodes, innerJoin, out aliasAssociationTableName);
                        }
                        else
                            fromClauseSQLQuery += BuildOneToManyTablesJoin(parendDataNode, relatedDataNodes, innerJoin);
                    }
                    else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                        fromClauseSQLQuery += BuildOneToManyTablesJoin(parendDataNode, relatedDataNodes, innerJoin);

                    #endregion
                }
            }
            else if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                System.Collections.Generic.List<DataNode> subDataNodesForJoin = new List<DataNode>();
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in (DataNode as GroupDataNode).SubDataNodes)
                {
                    if (!relatedDataNodes.ContainsKey(subDataNode))
                        continue;
                    if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        relatedDataNodes.Remove(subDataNode);
                        DataLoader subDataNodeDataLoader = GetDataLoader(subDataNode) as DataLoader;
                        if (subDataNodeDataLoader != null)
                            fromClauseSQLQuery += subDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDataNodes);
                        continue;
                    }

                    bool innerJoin = relatedDataNodes[subDataNode];
                    relatedDataNodes.Remove(subDataNode);

                    #region Build table join with sub data node if needed.
                    if (subDataNode.Type == DataNode.DataNodeType.Key)
                    {
                        foreach (MetaDataRepository.ObjectQueryLanguage.DerivedDataNode groupKeyDataNode in subDataNode.SubDataNodes)
                        {
                            if (!relatedDataNodes.ContainsKey(groupKeyDataNode))
                                continue;
                            if (groupKeyDataNode.OrgDataNode.AssignedMetaObject is MetaDataRepository.Attribute && groupKeyDataNode.OrgDataNode.Type == DataNode.DataNodeType.Object)
                            {
                                relatedDataNodes.Remove(groupKeyDataNode);
                                DataLoader subDataNodeDataLoader = GetDataLoader(groupKeyDataNode) as DataLoader;
                                if (subDataNodeDataLoader != null)
                                    fromClauseSQLQuery += subDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDataNodes);
                                continue;
                            }

                            innerJoin = relatedDataNodes[groupKeyDataNode];
                            relatedDataNodes.Remove(groupKeyDataNode);

                            #region Build table join with sub data node if needed.
                            fromClauseSQLQuery += BuildOneToManyTablesJoin(groupKeyDataNode, relatedDataNodes, true);
                            #endregion
                        }

                    }
                    //fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, relatedDataNodes, true);
                    #endregion
                }

                DataNode parendDataNode = DataNode.RealParentDataNode;
                while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                    parendDataNode = parendDataNode.RealParentDataNode;


                if (parendDataNode != null && relatedDataNodes.ContainsKey(parendDataNode))
                {

                    bool innerJoin = relatedDataNodes[parendDataNode];
                    relatedDataNodes.Remove(parendDataNode);
                    #region Build table join with parent data node if needed.

                    fromClauseSQLQuery += BuildOneToManyTablesJoin(parendDataNode, relatedDataNodes, innerJoin);

                    #endregion
                }

            }

            return fromClauseSQLQuery;
        }


        /// <summary>Build a join between the table from data source and data source table of sub data node. </summary>
        /// <MetaDataID>{7C44A9DF-B512-49BE-ABEF-D96BB0C8146F}</MetaDataID>
        internal string BuildOneToManyTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes, bool innerJoin)
        {

            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            if ((relatedDataNode == DataNode.RealParentDataNode && DataNode.AssignedMetaObject is MetaDataRepository.Attribute) ||
               (DataNode.RealSubDataNodes.Contains(relatedDataNode) && relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                innerJoin = false;

            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            if (DataNode.Type == DataNode.DataNodeType.Group)
            {

                if (relatedDataNode == DataNode.RealParentDataNode)
                {
                    foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                    {
                        leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                        rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);
                    }
                }
                else
                {
                    leftJoinTableObjectIdentityTypes[relatedDataNode.Identity.ToString()] = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                    rightJoinTableObjectIdentityTypes[relatedDataNode.Identity.ToString()] = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                    foreach (var keyRelationColumn in DataLoader.GroupByKeyRelationColumns[relatedDataNode.Identity])
                    {
                        System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                        for (int i = 0; i != keyRelationColumn.Value.GroupingColumnsNames.Count; i++)
                        {
                            MetaDataRepository.IdentityPart part = new OOAdvantech.MetaDataRepository.IdentityPart(keyRelationColumn.Value.GroupingColumnsNames[i], keyRelationColumn.Key.Parts[i].PartTypeName, keyRelationColumn.Key.Parts[i].Type);
                            parts.Add(part);
                        }
                        MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                        leftJoinTableObjectIdentityTypes[relatedDataNode.Identity.ToString()].Add(objectIdentityType);
                        objectIdentityType = GetDataLoader(relatedDataNode).ObjectIdentityTypes[GetDataLoader(relatedDataNode).ObjectIdentityTypes.IndexOf(keyRelationColumn.Key)];
                        rightJoinTableObjectIdentityTypes[relatedDataNode.Identity.ToString()].Add(objectIdentityType);
                    }
                }
            }
            else
            {
                DataNode parendDataNode = DataNode.RealParentDataNode;
                while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                    parendDataNode = parendDataNode.RealParentDataNode;


                if (relatedDataNode == parendDataNode)
                {
                    foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                    {
                        leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                        rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);
                    }
                }
                else
                {
                    foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity].Keys)
                    {
                        leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity][relationPartIdentity].Keys);
                        rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                    }
                }
            }
            string fromClauseSQLQuery = null;
            DataLoader relatedDataNodeDataLoader = GetDataLoader(relatedDataNode) as DataLoader;

            string relatedDataNodeAlias = relatedDataNode.Alias;
            if (relatedDerivedDataNode != null)
                relatedDataNodeAlias = relatedDerivedDataNode.Alias;

            DataNode dataNode = DataNode;
            while (dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNode.DataNodeType.Object)
                dataNode = dataNode.RealParentDataNode;
            string dataLoaderDataNodeAlias = dataNode.Alias;
            if (dataNode == DataNode && relatedDerivedDataNode != null && relatedDerivedDataNode.ParentDataNode is DerivedDataNode && (relatedDerivedDataNode.ParentDataNode as DerivedDataNode).OrgDataNode == DataNode)
                dataLoaderDataNodeAlias = relatedDerivedDataNode.ParentDataNode.Alias;
            else
            {
                DerivedDataNode derivedDataNode = relatedDerivedDataNode;
                while (derivedDataNode != null && derivedDataNode.OrgDataNode != dataNode)
                    derivedDataNode = derivedDataNode.ParentDataNode as DerivedDataNode;
                if (derivedDataNode != null)
                    dataLoaderDataNodeAlias = derivedDataNode.Alias;
            }



            if (relatedDataNodeDataLoader != null)
            {
                string joinFilterScript = " ON " + BuildJoinFilter(dataLoaderDataNodeAlias, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);

                fromClauseSQLQuery += relatedDataNodeDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(relatedDataNodeAlias);
                fromClauseSQLQuery += joinFilterScript + " " + relatedDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDerivedDataNode, relatedDataNodes);

                if (innerJoin)
                    fromClauseSQLQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Inner) + fromClauseSQLQuery;
                else
                    fromClauseSQLQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Left) + fromClauseSQLQuery;

            }



            return fromClauseSQLQuery;


            if (innerJoin)
                fromClauseSQLQuery = BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner,
                                                    fromClauseSQLQuery,
                                                    dataLoaderDataNodeAlias,
                                                    leftJoinTableObjectIdentityTypes,
                                                    relatedDataNodeAlias,
                                                    rightJoinTableObjectIdentityTypes);
            else
                fromClauseSQLQuery = BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Left,
                                                    fromClauseSQLQuery,
                                                    dataLoaderDataNodeAlias,
                                                    leftJoinTableObjectIdentityTypes,
                                                    relatedDataNodeAlias,
                                                    rightJoinTableObjectIdentityTypes);
            return fromClauseSQLQuery;
        }


        /// <summary>Build a join between the table from data source and
        /// association table and a join between association table end data source table of sub data node. </summary>
        /// <MetaDataID>{97E28D6A-7A8E-40B2-81FF-B4B2AE305DD5} </MetaDataID>
        protected string BuildManyToManyTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode,
                                        bool forRelationDataOnly,
                                        Dictionary<DataNode, bool> relatedDataNodes,
                                        bool innerJoin,
                                        out string aliasAssociationTableName)
        {
            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            ///TODO Να γίνουν test case για όλων των ειδών relation με DerivedDataNode

            DataNode parendDataNode = DataNode.RealParentDataNode;
            while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                parendDataNode = parendDataNode.RealParentDataNode;

            #region Precondition check
            if (relatedDataNode == parendDataNode)
            {
                if (!DataNode.ThroughRelationTable)//associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            }
            else
            {
                if (!relatedDataNode.ThroughRelationTable)//associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            }
            #endregion

            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            MetaDataRepository.Roles subDataNodeRole;

            if (relatedDataNode == parendDataNode)
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;

            Collections.Generic.Set<MetaDataRepository.StorageCellsLink> ObjectsLinks = DataLoader.GetStorageCellsLinks(relatedDataNode);

            #region Construct  association data sql script.
            aliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(association.Name);
            string AssociationTableStatement = null;
            if (DataLoader.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
            {
                //Builds relationtable between DataLoader (assocition class) table and relatedDataNode  Table
                AssociationTableStatement = BuildRelationObjectsObjectLinksSQLScript(relatedDataNode, associationEnd, ObjectsLinks) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasAssociationTableName);
            }
            else if (relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
            {
                //Builds relationtable between dataloader table and relatedDataNode (assocition class)  Table
                AssociationTableStatement = BuildRelationObjectsObjectLinksSQLScript(relatedDataNode, associationEnd.GetOtherEnd(), ObjectsLinks) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasAssociationTableName);
            }
            else
                AssociationTableStatement = BuildObjectLinksSQLScript(relatedDataNode, association, ObjectsLinks) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasAssociationTableName);
            #endregion

            string fromClauseSQLQuery =null;
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            string joinFilterScript = null;
            if (/*ObjectsLinks.Count > 0 &&*/ !forRelationDataOnly)
            {
                #region Construct association dataTable, relatedDataNode dataTable join sql script.
                DataLoader relatedNodeDataLoader = null;
                if (relatedDataNode == parendDataNode)
                {
                    foreach (string relationPartIdentity in relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity].Keys)
                        rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);
                }
                else
                {
                    foreach (string relationPartIdentity in relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent.Keys)
                        rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                }
                //if (relatedDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.StorageIdentity))
                relatedNodeDataLoader = GetDataLoader(relatedDataNode) as DataLoader;

                foreach (string relationPartIdentity in rightJoinTableObjectIdentityTypes.Keys)
                {
                    if (associationEnd.Association.RoleA.Identity.ToString() == relationPartIdentity || associationEnd.Association.RoleB.Identity.ToString() == relationPartIdentity)
                        leftJoinTableObjectIdentityTypes[relationPartIdentity] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                    else
                    {
                        foreach (MetaDataRepository.Association subAssociation in associationEnd.Association.Specializations)
                        {
                            if (subAssociation.RoleA.Identity.ToString() == relationPartIdentity || subAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                            {
                                if (associationEnd.IsRoleA)
                                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                                else
                                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                                break;
                            }
                        }
                    }
                }

                string relatedDataNodeAlias = relatedDataNode.Alias;
                if (relatedDerivedDataNode != null)
                    relatedDataNodeAlias = relatedDerivedDataNode.Alias;

                if (rightJoinTableObjectIdentityTypes.Count != leftJoinTableObjectIdentityTypes.Count)
                    throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

                joinFilterScript = " ON " + BuildJoinFilter(aliasAssociationTableName, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);

                string rightJoinedTable = relatedNodeDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(relatedDataNodeAlias);
                rightJoinedTable += joinFilterScript + relatedNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDerivedDataNode, relatedDataNodes);
                fromClauseSQLQuery += GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Inner) + rightJoinedTable;

                //fromClauseSQLQuery += BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner,
                //                                    rightJoinedTable,
                //                                    aliasAssociationTableName,
                //                                    leftJoinTableObjectIdentityTypes,
                //                                     relatedDataNodeAlias,
                //                                    rightJoinTableObjectIdentityTypes);
                #endregion
            }


            if (ObjectsLinks.Count == 0 && forRelationDataOnly)
                return null;

            #region Construct DataLoader dataTable, association dataTable join sql script.

            if (relatedDataNode == parendDataNode)
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
            }
            else
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity].Keys)
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity][relationPartIdentity].Keys);
            }
            foreach (string relationPartIdentity in leftJoinTableObjectIdentityTypes.Keys)
            {
                if (associationEnd.Association.RoleA.Identity.ToString() == relationPartIdentity || associationEnd.Association.RoleB.Identity.ToString() == relationPartIdentity)
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = associationEnd.GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                else
                {
                    foreach (MetaDataRepository.Association subAssociation in associationEnd.Association.Specializations)
                    {
                        if (subAssociation.RoleA.Identity.ToString() == relationPartIdentity || subAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                        {
                            if (associationEnd.IsRoleA)
                                rightJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                            else
                                rightJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                            break;
                        }
                    }
                }
            }
            if (rightJoinTableObjectIdentityTypes.Count != leftJoinTableObjectIdentityTypes.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

            DataNode dataNode = DataNode;
            while (dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNode.DataNodeType.Object)
                dataNode = dataNode.RealParentDataNode;

            string dataLoaderDataNodeAlias = dataNode.Alias;
            if (dataNode == DataNode && relatedDerivedDataNode != null && relatedDerivedDataNode.ParentDataNode is DerivedDataNode && (relatedDerivedDataNode.ParentDataNode as DerivedDataNode).OrgDataNode == DataNode)
                dataLoaderDataNodeAlias = relatedDerivedDataNode.ParentDataNode.Alias;
            else
            {
                DerivedDataNode derivedDataNode = relatedDerivedDataNode;
                while (derivedDataNode != null && derivedDataNode.OrgDataNode != dataNode)
                    derivedDataNode = derivedDataNode.ParentDataNode as DerivedDataNode;
                if (derivedDataNode != null)
                    dataLoaderDataNodeAlias = derivedDataNode.Alias;

            }

            

            joinFilterScript = " ON " + BuildJoinFilter(dataLoaderDataNodeAlias, leftJoinTableObjectIdentityTypes, aliasAssociationTableName, rightJoinTableObjectIdentityTypes);

            fromClauseSQLQuery = AssociationTableStatement + joinFilterScript + fromClauseSQLQuery;

            if (innerJoin)
                fromClauseSQLQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Inner) + fromClauseSQLQuery;
            else
                fromClauseSQLQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Left) + fromClauseSQLQuery;


            //if (innerJoin)
            //    fromClauseSQLQuery = BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner,
            //                                        fromClauseSQLQuery,
            //                                        dataLoaderDataNodeAlias,
            //                                        leftJoinTableObjectIdentityTypes,
            //                                        aliasAssociationTableName,
            //                                        rightJoinTableObjectIdentityTypes);
            //else
            //    fromClauseSQLQuery = BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Left,
            //                                        fromClauseSQLQuery,
            //                                        dataLoaderDataNodeAlias,
            //                                        leftJoinTableObjectIdentityTypes,
            //                                        aliasAssociationTableName,
            //                                        rightJoinTableObjectIdentityTypes);
            #endregion

            if (forRelationDataOnly)
                return BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(DataNode.Alias) + fromClauseSQLQuery;
            return fromClauseSQLQuery;

        }


        /// <MetaDataID>{d03042d4-f5af-498a-8069-35584016cbe1}</MetaDataID>
        internal string BuildAssociationClassTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes, bool innerJoin)
        {

            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            MetaDataRepository.ObjectQueryLanguage.DataNode rootDataNode = DataNode;

            DataLoader relatedDataNodeDataLoader = GetDataLoader(relatedDataNode) as DataLoader;
            string relatedDataNodeAlias = relatedDataNode.Alias;
            if (relatedDerivedDataNode != null)
                relatedDataNodeAlias = relatedDerivedDataNode.Alias;

            //if (relatedDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.StorageIdentity))
            //    relatedDataNodeDataLoader =GetDataLoader( relatedDataNode).DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity] as DataLoader;
            DataNode parendDataNode = DataNode.RealParentDataNode;
            while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                parendDataNode = parendDataNode.RealParentDataNode;

            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            if (relatedDataNode == parendDataNode)
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;

            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            // throw new System.Exception("Data tree Error");//TODOD Error Prone

            if (relatedDataNode == parendDataNode)
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                {
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);
                }
            }
            else
            {
                Guid relatedDataNodeIdentity = relatedDataNode.Identity;
                if (relatedDataNode is DerivedDataNode)
                    relatedDataNodeIdentity = (relatedDataNode as DerivedDataNode).OrgDataNode.Identity;

                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNodeIdentity].Keys)
                {
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNodeIdentity][relationPartIdentity].Keys);
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                }
            }
            if (rightJoinTableObjectIdentityTypes.Count != leftJoinTableObjectIdentityTypes.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

            string dataLoaderDataNodeAlias = DataNode.Alias;
            if (relatedDerivedDataNode != null && relatedDerivedDataNode.ParentDataNode is DerivedDataNode && (relatedDerivedDataNode.ParentDataNode as DerivedDataNode).OrgDataNode == DataNode)
                dataLoaderDataNodeAlias = relatedDerivedDataNode.ParentDataNode.Alias;

            string fromClauseSqlQuery = null;
            if (relatedDataNodeDataLoader != null)
            {
                string joinFilterScript = " ON " + BuildJoinFilter(dataLoaderDataNodeAlias, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);

                fromClauseSqlQuery = relatedDataNodeDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(relatedDataNodeAlias);
                fromClauseSqlQuery += joinFilterScript + relatedDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDerivedDataNode, relatedDataNodes);

                if (innerJoin)
                    fromClauseSqlQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Inner) + fromClauseSqlQuery;
                else
                    fromClauseSqlQuery = GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Left) + fromClauseSqlQuery;
            }

            return fromClauseSqlQuery;
            if (innerJoin)
                return BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner, fromClauseSqlQuery, dataLoaderDataNodeAlias, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);
            else
                return BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Left, fromClauseSqlQuery, dataLoaderDataNodeAlias, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);

        }

        internal string GetJoinScript(TableJoinType joinType)
        {
            string joinSQLScript = null;
            switch (joinType)
            {
                case RDBMSPersistenceRunTime.TableJoinType.Inner:
                    {
                        joinSQLScript = "\r\nINNER JOIN ";
                        break;
                    }
                case RDBMSPersistenceRunTime.TableJoinType.Left:
                    {
                        joinSQLScript = "\r\nLEFT OUTER JOIN ";
                        break;
                    }
                case RDBMSPersistenceRunTime.TableJoinType.Right:
                    {
                        joinSQLScript = "\r\nRIGHT OUTER JOIN ";
                        break;
                    }
            }
            return joinSQLScript;
        }
        ///<summary>
        ///Build join sql script between right and left joined tables
        ///</summary>
        /// <MetaDataID>{8916c256-97a8-448b-84b4-ffb89ed310d1}</MetaDataID>
        internal string BuildTablesJoin(
                                TableJoinType joinType,
                                string rightJoinedTable,
                                string leftJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes,
                                string rightJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes)
        {
            string joinSQLScript = null;
            switch (joinType)
            {
                case RDBMSPersistenceRunTime.TableJoinType.Inner:
                    {
                        joinSQLScript = "\r\nINNER JOIN ";
                        break;
                    }
                case RDBMSPersistenceRunTime.TableJoinType.Left:
                    {
                        joinSQLScript = "\r\nLEFT OUTER JOIN ";
                        break;
                    }
                case RDBMSPersistenceRunTime.TableJoinType.Right:
                    {
                        joinSQLScript = "\r\nRIGHT OUTER JOIN ";
                        break;
                    }
            }
            if (rightJoinedTable.IndexOf("INNER JOIN") != -1 || rightJoinedTable.IndexOf("OUTER JOIN") != -1)
                rightJoinedTable = string.Format("({0})", rightJoinedTable);

            joinSQLScript += rightJoinedTable;

            //string joinFilterSQLScript = " ON ";
            //bool firstIteration = true;
            //foreach (string relationPartIdentity in leftJoinTableObjectIdentityTypes.Keys)
            //{
            //    foreach (MetaDataRepository.ObjectIdentityType leftJoinTableObjectIdentityType in leftJoinTableObjectIdentityTypes[relationPartIdentity])
            //    {
            //        if (!firstIteration)
            //            joinFilterSQLScript += " OR ";
            //        firstIteration = false;

            //        joinFilterSQLScript += "(";
            //        foreach (MetaDataRepository.ObjectIdentityType rightJoinTableObjectIdentityType in rightJoinTableObjectIdentityTypes[relationPartIdentity])
            //        {
            //            if (leftJoinTableObjectIdentityType == rightJoinTableObjectIdentityType)
            //            {
            //                for (int i = 0; i != leftJoinTableObjectIdentityType.Parts.Count; i++)
            //                {
            //                    if (i > 0)
            //                        joinSQLScript += " AND ";
            //                    joinFilterSQLScript += GetSQLScriptForName(leftJoinedTableName) + "." + GetSQLScriptForName(leftJoinTableObjectIdentityType.Parts[i].Name) + " = " + GetSQLScriptForName(rightJoinedTableName) + "." + GetSQLScriptForName(rightJoinTableObjectIdentityType.Parts[i].Name);
            //                }
            //            }
            //        }
            //        joinFilterSQLScript += ")";
            //    }
            //}

            return joinSQLScript + " ON " + BuildJoinFilter(leftJoinedTableName, leftJoinTableObjectIdentityTypes, rightJoinedTableName, rightJoinTableObjectIdentityTypes);
            // return joinSQLScript + joinFilterSQLScript;

        }


        /// <MetaDataID>{d984982b-dec3-4130-856f-e5322c82eb22}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+24")]
        protected string BuildJoinFilter(
                                string leftJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> leftJoinedTableObjectIdentityTypes,
                                string rightJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> rightJoinedTableObjectIdentityTypes)
        {
            return BuildJoinFilter(leftJoinedTableName, leftJoinedTableObjectIdentityTypes, rightJoinedTableName, rightJoinedTableObjectIdentityTypes, "=");
        }

        /// <MetaDataID>{2db1021a-bceb-4c6a-a94f-d9cddd8aabb3}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+25")]
        protected string BuildJoinFilter(
                                string leftJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> leftJoinedTableObjectIdentityTypes,
                                string rightJoinedTableName,
                                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> rightJoinedTableObjectIdentityTypes,
                                string conditionSQLScript)
        {
            string joinSQLScript = null;

            bool firstIteration = true;
            foreach (string relationPartIdentity in leftJoinedTableObjectIdentityTypes.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType leftJoinTableObjectIdentityType in leftJoinedTableObjectIdentityTypes[relationPartIdentity])
                {
                    if (!firstIteration)
                        joinSQLScript += " OR ";
                    firstIteration = false;

                    joinSQLScript += "(";
                    foreach (MetaDataRepository.ObjectIdentityType rightJoinTableObjectIdentityType in rightJoinedTableObjectIdentityTypes[relationPartIdentity])
                    {
                        if (leftJoinTableObjectIdentityType == rightJoinTableObjectIdentityType)
                        {
                            for (int i = 0; i != leftJoinTableObjectIdentityType.Parts.Count; i++)
                            {
                                if (i > 0)
                                    joinSQLScript += " AND ";
                                joinSQLScript += GetSQLScriptForName(leftJoinedTableName) + "." + GetSQLScriptForName(leftJoinTableObjectIdentityType.Parts[i].Name) + " " + conditionSQLScript + " " + GetSQLScriptForName(rightJoinedTableName) + "." + GetSQLScriptForName(rightJoinTableObjectIdentityType.Parts[i].Name);
                            }
                        }
                    }
                    joinSQLScript += ")";
                }
            }
            return joinSQLScript;

        }




        /// <MetaDataID>{8bce690f-6377-46a5-962c-707031d340e4}</MetaDataID>
        internal string BuildManyToManyTablesJoinForAgregation(AggregateExpressionDataNode aggregateExpressionDataNode,
                                DataNode relatedDataNode,
                                Dictionary<DataNode, bool> relatedDataNodes, out string joinWhereAttributes)
        {
            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            List<DataNode> selectListDataNodes = new List<DataNode>(relatedDataNodes.Keys);
            if (!selectListDataNodes.Contains(relatedDataNode))
                selectListDataNodes.Add(relatedDataNode);

            if (aggregateExpressionDataNode.ParentDataNode is GroupDataNode)
            {
                #region Add group keys DataNodes to selection list

                foreach (DataNode keyDataNode in (aggregateExpressionDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (!selectListDataNodes.Contains(keyDataNode))
                        selectListDataNodes.Add(keyDataNode);
                }
                if (aggregateExpressionDataNode.ParentDataNode.RealParentDataNode != null &&
                    aggregateExpressionDataNode.ParentDataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    if (!selectListDataNodes.Contains(aggregateExpressionDataNode.ParentDataNode.RealParentDataNode))
                        selectListDataNodes.Add(aggregateExpressionDataNode.ParentDataNode.RealParentDataNode);
                }

                #endregion
            }


            //if (relatedDataNode.SearchCondition != null && relatedDataNode.ParentDataNode.SearchCondition != relatedDataNode.SearchCondition)
            //{
            //    foreach (KeyValuePair<DataNode, bool> relatedDataNodeEntry in (GetDataLoader(relatedDataNode) as DataLoader).RelatedDataNodes)
            //    {
            //        if (!relatedDataNodes.ContainsKey(relatedDataNodeEntry.Key) || relatedDataNode != relatedDataNodeEntry.Key)
            //            relatedDataNodes[relatedDataNodeEntry.Key] = relatedDataNodeEntry.Value;
            //    }
            //}

            DataNode parendDataNode = DataNode.RealParentDataNode;
            while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                parendDataNode = parendDataNode.RealParentDataNode;


            #region precondition check
            if (relatedDataNode == parendDataNode)
            {
                if (!DataNode.ThroughRelationTable)//associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            }
            else
            {
                if (!relatedDataNode.ThroughRelationTable)//associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            }
            #endregion

            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            if (relatedDataNode == parendDataNode)
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;
            Collections.Generic.Set<MetaDataRepository.StorageCellsLink> ObjectsLinks = DataLoader.GetStorageCellsLinks(relatedDataNode);

            #region Construct ObjectLinksDataSource object
            //Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            //foreach (RDBMSMetaDataRepository.StorageCell storageCell in this.DataLoaderMetadata.StorageCells)
            //{
            //    if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
            //    {
            //        OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            //        valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
            //        foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in association.GetStorageCellsLinks(storageCell))
            //        {
            //            if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
            //                ObjectsLinks.Add(storageCellsLink);
            //        }
            //    }
            //    else
            //        ObjectsLinks.AddRange(association.GetStorageCellsLinks(storageCell));
            //}
            #endregion

            #region Construct  association data sql script.
            string aliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(association.Name);
            string associationTableStatement = BuildObjectLinksSQLScript(relatedDataNode, association, ObjectsLinks) + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(aliasAssociationTableName);
            #endregion

            string fromClauseSQLQuery = associationTableStatement;
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();

            #region Construct association dataTable, relatedDataNode dataTable join sql script.
            DataLoader relatedNodeDataLoader = null;
            if (relatedDataNode == parendDataNode)
            {
                foreach (string relationPartIdentity in relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity].Keys)
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);//GetDataNodeObjectIdentityTypes(relatedDataNode, ObjectIdentityTypes);
            }
            else
            {
                foreach (string relationPartIdentity in relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent.Keys)
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(relatedDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
            }
            // if (relatedDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.StorageIdentity))
            relatedNodeDataLoader = GetDataLoader(relatedDataNode) as DataLoader;//.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity] as DataLoader;

            foreach (string relationPartIdentity in rightJoinTableObjectIdentityTypes.Keys)
            {
                if (associationEnd.Association.RoleA.Identity.ToString() == relationPartIdentity || associationEnd.Association.RoleB.Identity.ToString() == relationPartIdentity)
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                else
                {
                    foreach (MetaDataRepository.Association subAssociation in associationEnd.Association.Specializations)
                    {
                        if (subAssociation.RoleA.Identity.ToString() == relationPartIdentity || subAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                        {
                            if (associationEnd.IsRoleA)
                                leftJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                            else
                                leftJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(rightJoinTableObjectIdentityTypes[relationPartIdentity]);
                            break;
                        }
                    }
                }
            }




            if (rightJoinTableObjectIdentityTypes.Count != leftJoinTableObjectIdentityTypes.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

            string relatedDataNodeAlias = relatedDataNode.Alias;
            if (relatedDerivedDataNode != null)
                relatedDataNodeAlias = relatedDerivedDataNode.Alias;


            string joinFilterScript = " ON " + BuildJoinFilter(aliasAssociationTableName, leftJoinTableObjectIdentityTypes, relatedDataNodeAlias, rightJoinTableObjectIdentityTypes);

            string rightJoinedTable = relatedNodeDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(relatedDataNode.Alias);
            rightJoinedTable += joinFilterScript + relatedNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDerivedDataNode, relatedDataNodes);
            fromClauseSQLQuery += GetJoinScript(RDBMSPersistenceRunTime.TableJoinType.Inner) + rightJoinedTable;

            //fromClauseSQLQuery += BuildTablesJoin(RDBMSPersistenceRunTime.TableJoinType.Inner,
            //                                    rightJoinedTable,
            //                                    aliasAssociationTableName,
            //                                    leftJoinTableObjectIdentityTypes,
            //                                     relatedDataNodeAlias,
            //                                    rightJoinTableObjectIdentityTypes);

            #endregion


            joinWhereAttributes = null;
            if (ObjectsLinks.Count == 0)
                return null;

            #region Construct DataLoader dataTable, association dataTable join sql script.

            if (relatedDataNode == parendDataNode)
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
            }
            else
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity].Keys)
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity][relationPartIdentity].Keys);
            }

            foreach (string relationPartIdentity in leftJoinTableObjectIdentityTypes.Keys)
            {
                if (associationEnd.Association.RoleA.Identity.ToString() == relationPartIdentity || associationEnd.Association.RoleB.Identity.ToString() == relationPartIdentity)
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = associationEnd.GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                else
                {
                    foreach (MetaDataRepository.Association subAssociation in associationEnd.Association.Specializations)
                    {
                        if (subAssociation.RoleA.Identity.ToString() == relationPartIdentity || subAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                        {
                            if (associationEnd.IsRoleA)
                                rightJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                            else
                                rightJoinTableObjectIdentityTypes[relationPartIdentity] = (subAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(leftJoinTableObjectIdentityTypes[relationPartIdentity]);
                            break;
                        }
                    }
                }
            }

            if (rightJoinTableObjectIdentityTypes.Count != leftJoinTableObjectIdentityTypes.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");
            if (aggregateExpressionDataNode.ParentDataNode is GroupDataNode)
            {
                joinWhereAttributes = (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.GroupColumnsJoinWhereAttributes(aliasAssociationTableName);
            }
            else
            {
                joinWhereAttributes = BuildJoinFilter(DataNode.Alias,
                                                        leftJoinTableObjectIdentityTypes,
                                                        aliasAssociationTableName,
                                                        rightJoinTableObjectIdentityTypes);
            }
            #endregion

            #region  Build sql script for Aggregation
            string distinctSelect = null;
            foreach (string relationPartIdentity in rightJoinTableObjectIdentityTypes.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in rightJoinTableObjectIdentityTypes[relationPartIdentity])
                {
                    foreach (MetaDataRepository.IdentityPart part in objectIdentityType.Parts)
                    {
                        if (distinctSelect == null)
                            distinctSelect = "SELECT DISTINCT ";
                        else
                            distinctSelect += ",";
                        distinctSelect += GetSQLScriptForName(aliasAssociationTableName) + "." + GetSQLScriptForName(part.Name);
                    }
                }
            }
            List<string> selectedListColumnNames = new List<string>();
            foreach (DataNode selectListDataNode in selectListDataNodes)
            {
                DataNode dataLoaderDataNode = selectListDataNode;
                if (dataLoaderDataNode.Type != DataNode.DataNodeType.Object && dataLoaderDataNode.Type != DataNode.DataNodeType.Group)
                    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                if (selectListDataNode.DataSource != null)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(selectListDataNode).ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (!selectedListColumnNames.Contains(GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name)))
                            {
                                selectedListColumnNames.Add(GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name));
                                distinctSelect += "," + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                                if (selectListDataNode != relatedDataNode)
                                    distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(dataLoaderDataNode.Alias + "_" + part.Name);
                            }
                        }
                    }
                }

                if ((selectListDataNode.ParticipateInAggregateFunction || (aggregateExpressionDataNode.ParentDataNode is GroupDataNode && (aggregateExpressionDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Contains(selectListDataNode)))
                  && selectListDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {

                    string columnName = DataSource.GetDataTreeUniqueColumnName(selectListDataNode);
                    if (selectListDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && selectListDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                        distinctSelect += "," + RDBMSSQLScriptGenarator.GetDatePartSqlScript(selectListDataNode);
                    else
                        distinctSelect += "," + GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).GetColumn(selectListDataNode).Name);
                    distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + columnName;
                }

                //if (selectListDataNode.ParticipateInAggregateFunction && selectListDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                //{
                //    //distinctSelect += "," + GetSQLScriptForName((GetDataLoader(selectListDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(GetDataLoader(selectListDataNode.ParentDataNode).GetColumnName(selectListDataNode));
                //    //distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(GetDataLoader(selectListDataNode).DataNode.Alias + "_" + GetDataLoader(selectListDataNode.ParentDataNode).GetColumnName(selectListDataNode));
                //    string columnName = DataSource.GetDataTreeUniqueColumnName(selectListDataNode);
                //    distinctSelect += "," + GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).GetColumn(selectListDataNode).Name);
                //    distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + columnName;
                //}


            }
            #endregion

            string whereClause = null;
            //if (relatedDataNode.SearchCondition != null && relatedDataNode.ParentDataNode.SearchCondition != relatedDataNode.SearchCondition)
            //    whereClause += "\r\n WHERE " + (GetDataLoader(relatedDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(relatedDataNode.SearchCondition);
            if (aggregateExpressionDataNode.SourceSearchCondition != null)
            {
                if (string.IsNullOrEmpty(whereClause))
                    whereClause += "\r\n WHERE " + (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(aggregateExpressionDataNode.SourceSearchCondition);
                else
                    whereClause += " AND (" + (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(aggregateExpressionDataNode.SourceSearchCondition);
            }
            return "(" + distinctSelect + "\r\nFROM " + fromClauseSQLQuery + whereClause + ")" + aliasAssociationTableName;


        }

        /// <MetaDataID>{95750e31-61e9-4c5b-8f63-4cc83e0307fc}</MetaDataID>
        internal string BuildOneToManyTablesJoinForAgregation(AggregateExpressionDataNode aggregateExpressionDataNode,
                                                DataNode relatedDataNode,
                                                Dictionary<DataNode, bool> relatedDataNodes,
                                                out string joinWhereAttributes)
        {

            DerivedDataNode relatedDerivedDataNode = relatedDataNode as DerivedDataNode;
            if (relatedDerivedDataNode != null)
                relatedDataNode = relatedDerivedDataNode.OrgDataNode;

            List<DataNode> selectListDataNodes = new List<DataNode>(relatedDataNodes.Keys);
            if (!selectListDataNodes.Contains(relatedDataNode))
                selectListDataNodes.Add(relatedDataNode);
            if (aggregateExpressionDataNode.ParentDataNode is GroupDataNode)
            {
                #region Add group keys DataNodes to selection list

                foreach (DataNode keyDataNode in (aggregateExpressionDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (!selectListDataNodes.Contains(keyDataNode))
                        selectListDataNodes.Add(keyDataNode);
                }
                if (aggregateExpressionDataNode.ParentDataNode.RealParentDataNode != null &&
                    aggregateExpressionDataNode.ParentDataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    if (!selectListDataNodes.Contains(aggregateExpressionDataNode.ParentDataNode.RealParentDataNode))
                        selectListDataNodes.Add(aggregateExpressionDataNode.ParentDataNode.RealParentDataNode);
                }

                #endregion
            }

            //if (relatedDataNode.SearchCondition != null && relatedDataNode.ParentDataNode.SearchCondition != relatedDataNode.SearchCondition)
            //{
            //    foreach (KeyValuePair<DataNode, bool> relatedDataNodeEntry in (GetDataLoader(relatedDataNode) as DataLoader).RelatedDataNodes)
            //    {
            //        if (!relatedDataNodes.ContainsKey(relatedDataNodeEntry.Key) || relatedDataNode != relatedDataNodeEntry.Key)
            //            relatedDataNodes[relatedDataNodeEntry.Key] = relatedDataNodeEntry.Value;
            //    }
            //}

            DataNode parendDataNode = DataNode.RealParentDataNode;
            while (parendDataNode != null && parendDataNode.AssignedMetaObject is MetaDataRepository.Attribute && parendDataNode.Type == DataNode.DataNodeType.Object)
                parendDataNode = parendDataNode.RealParentDataNode;
            joinWhereAttributes = null;
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> leftJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> rightJoinTableObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            if (relatedDataNode == parendDataNode)
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithParent.Keys)
                {
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys);
                }
                selectListDataNodes.Remove(relatedDataNode);
            }
            else
            {
                foreach (string relationPartIdentity in DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity].Keys)
                {
                    leftJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[relatedDataNode.Identity][relationPartIdentity].Keys);
                    rightJoinTableObjectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>(GetDataLoader(relatedDataNode).DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys);
                }
                selectListDataNodes.Remove(relatedDataNode);
            }

            string fromClauseSQLQuery = null;
            DataLoader subDataNodeDataLoader = GetDataLoader(relatedDataNode) as DataLoader;

            string relatedDataNodeAlias = relatedDataNode.Alias;
            if (relatedDerivedDataNode != null)
                relatedDataNodeAlias = relatedDerivedDataNode.Alias;

            if (subDataNodeDataLoader != null)
            {
                fromClauseSQLQuery += subDataNodeDataLoader.SqlScriptsBuilder.BuildClassifierDataRetrieveSQLScript() + RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptValidAlias(relatedDataNodeAlias);
                fromClauseSQLQuery += subDataNodeDataLoader.SqlScriptsBuilder.BuildJoinedTablesSQLScript(relatedDerivedDataNode, relatedDataNodes);
            }

            DataNode dataNode = DataNode;
            while (dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNode.DataNodeType.Object)
                dataNode = dataNode.RealParentDataNode;


            if (aggregateExpressionDataNode.ParentDataNode is GroupDataNode)
            {
                joinWhereAttributes = (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.GroupColumnsJoinWhereAttributes(relatedDataNodeAlias);
            }
            else
            {
                joinWhereAttributes = BuildJoinFilter(DataNode.Alias,
                                        leftJoinTableObjectIdentityTypes,
                                        relatedDataNodeAlias,
                                        rightJoinTableObjectIdentityTypes);
            }


            #region  Build sql script for Aggregation
            string distinctSelect = null;
            foreach (string relationPartIdentity in rightJoinTableObjectIdentityTypes.Keys)
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in rightJoinTableObjectIdentityTypes[relationPartIdentity])
                {
                    foreach (MetaDataRepository.IdentityPart part in objectIdentityType.Parts)
                    {
                        if (distinctSelect == null)
                            distinctSelect = "SELECT DISTINCT ";
                        else
                            distinctSelect += ",";
                        distinctSelect += GetSQLScriptForName(relatedDataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                    }
                }
            }
            List<string> selectedListColumnNames = new List<string>();
            foreach (DataNode selectListDataNode in selectListDataNodes)
            {
                DataNode dataLoaderDataNode = selectListDataNode;
                if (dataLoaderDataNode.Type != DataNode.DataNodeType.Object && dataLoaderDataNode.Type != DataNode.DataNodeType.Group)
                    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                while (dataLoaderDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataLoaderDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;

                if (selectListDataNode.DataSource != null)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(dataLoaderDataNode).ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (!selectedListColumnNames.Contains(GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name)))
                            {
                                selectedListColumnNames.Add(GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name));
                                distinctSelect += "," + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).DataNode.Alias) + "." + GetSQLScriptForName(part.Name);
                                if (selectListDataNode != relatedDataNode)
                                    distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + GetSQLScriptForName(dataLoaderDataNode.Alias + "_" + part.Name);
                            }
                        }
                    }
                }
                if ((selectListDataNode.ParticipateInAggregateFunction || (aggregateExpressionDataNode.ParentDataNode is GroupDataNode && (aggregateExpressionDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Contains(selectListDataNode)))
                    && selectListDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {

                    string columnName = DataSource.GetDataTreeUniqueColumnName(selectListDataNode);
                    if (selectListDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && selectListDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                        distinctSelect += "," + RDBMSSQLScriptGenarator.GetDatePartSqlScript(selectListDataNode);
                    else
                        distinctSelect += "," + GetSQLScriptForName(dataLoaderDataNode.Alias) + "." + GetSQLScriptForName((GetDataLoader(dataLoaderDataNode) as DataLoader).GetColumn(selectListDataNode).Name);
                    distinctSelect += RDBMSSQLScriptGenarator.AliasDefSqlScript + columnName;
                }
            }
            #endregion
            string whereClause = null;
            //if (relatedDataNode.SearchCondition != null && relatedDataNode.ParentDataNode.SearchCondition != relatedDataNode.SearchCondition)
            //    whereClause += "\r\n WHERE " + (GetDataLoader(relatedDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(relatedDataNode.SearchCondition);
            SearchCondition dataFilter = aggregateExpressionDataNode.SourceSearchCondition;
            if (aggregateExpressionDataNode.ParentDataNode is GroupDataNode)
                dataFilter = SearchCondition.JoinSearchConditions((aggregateExpressionDataNode.ParentDataNode as GroupDataNode).GroupingSourceSearchCondition, dataFilter);

            if (dataFilter != null)
            {
                if (string.IsNullOrEmpty(whereClause))
                    whereClause += "\r\n WHERE " + (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(dataFilter);
                else
                    whereClause += " AND (" + (GetDataLoader(aggregateExpressionDataNode.ParentDataNode) as DataLoader).SqlScriptsBuilder.SQLFilterScriptBuilder.GetSQLFilterStatament(dataFilter);
            }

            return "(" + distinctSelect + "\r\nFROM " + fromClauseSQLQuery + whereClause + ")" + GetSQLScriptForName(relatedDataNode.Alias);
        }


        internal bool HasCriterionOnAggregationFunc
        {
            get
            {
                if (!DataNode.FilterNotActAsLoadConstraint)
                {
                    if (DataLoader.SearchCondition != null)
                    {
                        foreach (DataNode criterionDataNode in DataLoader.SearchCondition.DataNodes)
                        {
                            if (criterionDataNode is AggregateExpressionDataNode)
                                return true;
                        }
                    }
                }
                return false;
            }

        }
    }

}
