namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{930a5e0a-5c1b-436d-b324-e4976e117afb}</MetaDataID>
    public abstract class ObjectQuery
    {
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [Association("", typeof(SearchCondition), Roles.RoleA, "{32981580-7380-4EBB-BCC4-CF3E865E329B}")]
        public SearchCondition SearchCondition;
        /// <exclude>Excluded</exclude>
        protected System.Collections.ArrayList _SelectListItems = new System.Collections.ArrayList();
        [Association("SelectClause", typeof(DataNode), Roles.RoleA, "{4142C88A-424C-426B-B05B-E7164D927A14}")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public virtual System.Collections.ArrayList SelectListItems
        {
            get
            {
                return _SelectListItems;
            }
        }

        /// <summary>DataTrees defines a collection of rootes of data trees. 
        /// The data trees extracted from the "FROM" clause of OQL with the method RetrieveDataTrees.
        /// On DataTree referred the paths from "WHERE" clause for data filters 
        /// and paths from "SELECT" clause for data  selection.</summary>
        [RoleAMultiplicityRange(1)]
        [Association("QueryData", typeof(DataNode), Roles.RoleA, "{DC4B5A5E-3670-435F-894E-AC743AA87358}")]
        public System.Collections.ArrayList DataTrees = new System.Collections.ArrayList();
        /// <MetaDataID>{97690c1b-9fa4-4af0-9511-5c54335dbdd5}</MetaDataID>
        public abstract DataSource CreateDataSourceFor(DataNode dataNode);
        /// <MetaDataID>{9c39e629-0db2-4829-81f6-9c361662d958}</MetaDataID>
        public abstract DataSource CreateRelationObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd);
        /// <MetaDataID>{360e6f6f-0326-47b4-95f7-09b9c33aad55}</MetaDataID>
        public  abstract DataSource CreateRelatedObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd);

        /// <MetaDataID>{ac8451c9-fe0f-49f8-ae74-3bd677baa732}</MetaDataID>
        public void BookAlias(string alias)
        {
            if (!UsedAliases.Contains(alias.Trim().ToLower()))
                UsedAliases.Add(alias.Trim().ToLower());

        }
	
     
        /// <MetaDataID>{ac586cde-4649-4c78-88b2-f9b984ad855a}</MetaDataID>
        public void AddSelectListItem(DataNode selectListItem)
        {
            _SelectListItems.Add(selectListItem);
            selectListItem.ParticipateInSelectClause = true;
        }
        /// <MetaDataID>{0e188e9d-09ab-4f52-8915-fba6c34aac53}</MetaDataID>
        public void RemoveSelectListItem(DataNode SelectListItem)
        {
            _SelectListItems.Remove(SelectListItem);
            SelectListItem.ParticipateInSelectClause = false;
        }
        /// <MetaDataID>{27e9b23c-7e4c-404f-a16a-b986c0ec44b1}</MetaDataID>
        public string GetValidAlias(string proposedAlias)
        {
            string ValidAlias = null;
            ValidAlias = proposedAlias;
            int Count = 0;
            //while(UsedTableNames.Contains(ValidTableName))
            while (UsedAliases.Contains(ValidAlias.ToLower().Trim()))
            {
                Count++;
                ValidAlias = proposedAlias + "_" + Count.ToString();
            }
            UsedAliases.Add(ValidAlias.Trim().ToLower());
            return ValidAlias;
        } 
        /// <MetaDataID>{c373a16c-0cc5-4dcd-a596-1736efe3dbf0}</MetaDataID>
        internal System.Data.DataSet MainDataReader;
        /// <MetaDataID>{e08caf08-b9b2-4563-87d9-471d9aab2b8c}</MetaDataID>
        public abstract bool IsRemovedRow(System.Data.DataRow row);
        /// <MetaDataID>{de32e997-ce5a-4dfe-87fe-b04d3a536cd2}</MetaDataID>
        public OOAdvantech.Collections.Map PathDataNodeMap = new OOAdvantech.Collections.Map();

        /// <summary>The UsedDataSourceAliases collection keeps all data source alias names.
        /// In data tree there is alias name for all data source . 
        /// The alias name must be unique in the OQLStatement.
        /// The OQLStatement look at the collection when produce alias names. </summary>
        /// <MetaDataID>{A9A56329-56C9-446C-A1F8-5385B774EEA2}</MetaDataID>
        private System.Collections.ArrayList UsedAliases = new System.Collections.ArrayList();




      
    }
}
