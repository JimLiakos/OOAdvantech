using OOAdvantech.Remoting;
using System;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{930a5e0a-5c1b-436d-b324-e4976e117afb}</MetaDataID>
    public abstract class ObjectQuery : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {


        ///// <MetaDataID>{ac5279ff-faaa-4bce-98c3-99d313c68e72}</MetaDataID>
        //public System.Collections.Generic.List<SearchCondition> SearchConditions
        //{
        //    get
        //    {
        //        System.Collections.Generic.List<SearchCondition> searchConditions = new System.Collections.Generic.List<SearchCondition>();

        //        foreach (var dataNode in DataTrees)
        //        {
        //            foreach (var searchCondition in dataNode.SearchConditions)
        //            {
        //                if (searchCondition != null)
        //                    searchConditions.Add(searchCondition);
        //            }
        //        }
        //        return searchConditions;
        //    }
        //}
        /// <MetaDataID>{372e8436-f737-46bc-98e8-708c898f5628}</MetaDataID>
        public readonly Guid QueryIdentity;
        /// <MetaDataID>{2e50eb07-89e2-4ab3-9e03-69174421300f}</MetaDataID>
        public ObjectQuery(Guid queryIdentity)
        {
            QueryIdentity = queryIdentity;

        }

        /// <MetaDataID>{df08733d-4c3d-421a-9840-4f62137cc07b}</MetaDataID>
        protected QueryResultType _QueryResultType;

        /// <MetaDataID>{ba740d83-e917-455b-a103-5746e24bd50f}</MetaDataID>
        public virtual QueryResultType QueryResultType
        {
            get
            {
                return _QueryResultType;
            }

            set
            {
                _QueryResultType = value;
                if (_QueryResultType != null)
                    _QueryResultType.ObjectQueryContextReference.ObjectQueryContext = this;


            }
        }

        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.List<DataNode> _SelectListItems = new OOAdvantech.Collections.Generic.List<DataNode>();
        [Association("SelectClause", typeof(DataNode), Roles.RoleA, "{4142C88A-424C-426B-B05B-E7164D927A14}")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.List<DataNode> SelectListItems
        {
            get
            {
                return _SelectListItems;
            }
        }

        /// <exclude>Excluded</exclude>
        public OOAdvantech.Collections.Generic.Dictionary<string, object> _Parameters;
        /// <MetaDataID>{799cf33f-06ab-471c-99ee-c6f9de57d456}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<string, object> Parameters
        {
            get
            {
                return _Parameters;
            }
            set
            {
                _Parameters = value;
            }
        }



        /// <summary>DataTrees defines a collection of rootes of data trees. 
        /// The data trees extracted from the "FROM" clause of OQL with the method RetrieveDataTrees.
        /// On DataTree referred the paths from "WHERE" clause for data filters 
        /// and paths from "SELECT" clause for data  selection.</summary>
        [RoleAMultiplicityRange(1)]
        [Association("QueryData", typeof(DataNode), Roles.RoleA, "{DC4B5A5E-3670-435F-894E-AC743AA87358}")]
        public OOAdvantech.Collections.Generic.List<DataNode> DataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
        /// <MetaDataID>{97690c1b-9fa4-4af0-9511-5c54335dbdd5}</MetaDataID>
       // public abstract DataSource CreateDataSourceFor(DataNode dataNode);
        /// <MetaDataID>{9c39e629-0db2-4829-81f6-9c361662d958}</MetaDataID>
       // public abstract DataSource CreateRelationObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd);
        /// <MetaDataID>{360e6f6f-0326-47b4-95f7-09b9c33aad55}</MetaDataID>
       // public  abstract DataSource CreateRelatedObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd);

        //public abstract DataSource CreateValueTypeDataSource(DataNode dataNode, DataNode referenceDataNode, Attribute attribute);



        /// <MetaDataID>{ac8451c9-fe0f-49f8-ae74-3bd677baa732}</MetaDataID>
        public void BookAlias(string alias)
        {

            if (!UsedAliases.Contains(alias.Trim().ToLower()))
                UsedAliases.Add(alias.Trim().ToLower());

        }


        /// <MetaDataID>{ac586cde-4649-4c78-88b2-f9b984ad855a}</MetaDataID>
        public void AddSelectListItem(DataNode selectListItem)
        {
            if (!_SelectListItems.Contains(selectListItem))
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
        internal protected IDataSet ObjectQueryDataSet;
        /// <MetaDataID>{de32e997-ce5a-4dfe-87fe-b04d3a536cd2}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<object, DataNode> PathDataNodeMap = new Collections.Generic.Dictionary<object, DataNode>();

        /// <summary>The UsedDataSourceAliases collection keeps all data source alias names.
        /// In data tree there is alias name for all data source . 
        /// The alias name must be unique in the OQLStatement.
        /// The OQLStatement look at the collection when produce alias names. </summary>
        /// <MetaDataID>{A9A56329-56C9-446C-A1F8-5385B774EEA2}</MetaDataID>
        internal protected System.Collections.Generic.List<string> UsedAliases = new System.Collections.Generic.List<string>();

        ///<summary>
        /// This method loads data in DataSet and perform the search condition filters
        /// </summary>
        /// <MetaDataID>{3684FD19-3FD6-4112-BF75-515F2EC32CBE}</MetaDataID>
        internal protected virtual void LoadData()
        {
            //ObjectQueryDataSet = new System.Data.DataSet();
            //DataTrees[0].LoadData();
            //DataTrees[0].GetData(ObjectQueryDataSet);
            //     FilterData();
            //GroupData();
            //DataTrees[0].RunAggregateFunctions();

        }

        //private void BuildCountDataNodes(DataNode dataNode)
        //{
        //    foreach(DataNode subDataNode in dataNode.SubDataNodes)
        //    {
        //        BuildCountDataNodes(subDataNode);
        //    }
        //    System.Data.DataRow countRow = null;
        //    if (dataNode.CountSelect)
        //    {

        //        int count = 0;

        //        System.Data.DataTable table = dataNode.DataSource.DataTable;
        //        if (table.Columns.Count == 1 && table.Columns[0].ColumnName == "Count")
        //        {
        //            if (table.Rows.Count > 1)
        //            {
        //                foreach (System.Data.DataRow row in table.Rows)
        //                    count += (int)row["Count"];
        //                table.Clear();
        //                table.Rows.Clear();
        //                table.Columns.Add("Count", typeof(int));
        //                countRow = table.NewRow();
        //                countRow["Count"] = count;
        //                table.Rows.Add(countRow);
        //            }

        //            return;
        //        }


        //        if (dataNode.SearchCondition == null)
        //        {
        //            count = table.Rows.Count;
        //        }
        //        else
        //        {
        //            foreach (System.Data.DataRow row in table.Rows)
        //            {
        //                if (!dataNode.SearchCondition.IsRemovedRow(row))
        //                    count++;
        //            }
        //        }
        //        table.Clear();
        //        table.Rows.Clear();
        //        table.Columns.Add("Count", typeof(int));
        //        countRow = table.NewRow();
        //        countRow["Count"] = count;
        //        table.Rows.Add(countRow);

        //    }

        //}
        /// <MetaDataID>{d921fadd-3516-4d24-bb92-0f406c5e8e13}</MetaDataID>
        //protected void FilterData()
        //{
        //    foreach (DataNode dataNode in DataTrees)
        //         OrganizeData(dataNode); 
        //}

        /// <MetaDataID>{b3e7052c-2cf7-44ac-bee2-4727a45e281c}</MetaDataID>
        protected virtual void OrganizeData(DataNode dataNode)
        {
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.Group ||
                    subDataNode.Type == DataNode.DataNodeType.Object)
                {
                    OrganizeData(subDataNode);
                }
            }


            if (DataGrouping.HasGroupingDataResultSubDataNodes(dataNode))
                DataGrouping.LoadGroupingData(dataNode);

            //if (dataNode.SearchConditions.Count > 0)
            //    dataNode.FilterData();

        }







        /// <MetaDataID>{7b595f63-0823-42e5-b2b9-2aed1d50d560}</MetaDataID>
        public virtual System.Type GetType(string classFullName, string assemblyData)
        {
            return ModulePublisher.ClassRepository.GetType(classFullName, assemblyData);
        }
        /// <summary>
        /// When is true the table relation must be contains the storage identity columns in parent and child columns collections 
        /// </summary>
        /// <MetaDataID>{64818d4f-8198-4123-b30a-aaf0bb037b9f}</MetaDataID>
        internal virtual bool UseStorageIdintityInTablesRelations
        {
            get
            {
                return true;
            }
        }
    }
}
