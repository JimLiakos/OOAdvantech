using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{





    /// <summary>The OQL query retrieves data from a data tree.
    ///  In data tree there is data nodes and data paths.
    /// The data node is a source of data and data path is the relations between the data source.
    /// For example in family model the query "Select thePersons From Person thePersons Where thePersons.
    /// TheChildrens.Address= thePersons" produce a data tree with two paths with root the source of data Person and paths 
    /// 1.Person (Address relation) Address and 
    /// 2.Person (TheChildrens relation) Person (Address relation) Address. </summary>
    /// <MetaDataID>{198402B8-8210-4285-8121-D27A8EDE8827}</MetaDataID>
    [Serializable]
    public class DataNode
    {

        /// <MetaDataID>{609179de-51e2-46bd-a50e-2cb45063a99c}</MetaDataID>
        public static bool CutClone;

        /// <MetaDataID>{079fda87-e6eb-430d-a488-e718c8e2fada}</MetaDataID>
        protected DataNode(Guid identity)
        {
            Identity = identity;
        }
        /// <MetaDataID>{fc33588f-4f87-494a-81d1-890ec3d049b4}</MetaDataID>
        virtual internal DataNode Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataNode;

            DataNode dataNode = new DataNode(Identity);
            clonedObjects[this] = dataNode;
            Copy(dataNode, clonedObjects);
            return dataNode;
        }
        /// <MetaDataID>{b31fc885-9e21-43d9-9451-8d512d750ea5}</MetaDataID>
        protected void Copy(DataNode newDataNode, Dictionary<object, object> clonedObjects)
        {
            newDataNode._Alias = _Alias;
            newDataNode._RefreshObjectState = _RefreshObjectState;
            if (_AssignedMetaObjectIdenty != null)
                newDataNode._AssignedMetaObjectIdenty = new MetaObjectID(_AssignedMetaObjectIdenty.ToString());

            newDataNode._AutoGenaratedForMembersFetching = _AutoGenaratedForMembersFetching;
            if (_BackRouteMemberFetchingObjectActivation != null)
                newDataNode._BackRouteMemberFetchingObjectActivation = new List<Guid>(_BackRouteMemberFetchingObjectActivation);

            foreach (DataNode subDataNode in SubDataNodes)
            {
                object createdSubDataNode = null;
                if (clonedObjects.TryGetValue(subDataNode, out createdSubDataNode))
                    newDataNode.SubDataNodes.Add(createdSubDataNode as DataNode);
                else
                    newDataNode.SubDataNodes.Add(subDataNode.Clone(clonedObjects));
            }




            if (_DataNodeWithRootDataSource != null)
            {
                object dataNodeWithRootDataSource = null;
                if (clonedObjects.TryGetValue(_DataNodeWithRootDataSource, out dataNodeWithRootDataSource))
                    newDataNode._DataNodeWithRootDataSource = dataNodeWithRootDataSource as DataNode;
                else
                    newDataNode._DataNodeWithRootDataSource = _DataNodeWithRootDataSource.Clone(clonedObjects);
            }

            newDataNode._DataSourceColumnIndex = _DataSourceColumnIndex;
            newDataNode._FilterNotActAsLoadConstraint = _FilterNotActAsLoadConstraint;
            newDataNode._MembersFetchingObjectActivation = _MembersFetchingObjectActivation;
            newDataNode._Name = _Name;
            newDataNode._OfTypeFilterClassifierFullName = _OfTypeFilterClassifierFullName;



            if (_ParentDataNode != null)
            {
                object parentDataNode = null;
                if (clonedObjects.TryGetValue(_ParentDataNode, out parentDataNode))
                    newDataNode._ParentDataNode = parentDataNode as DataNode;
                else
                    newDataNode._ParentDataNode = _ParentDataNode.Clone(clonedObjects);
            }
            newDataNode._ParticipateInAggregateFunction = _ParticipateInAggregateFunction;
            newDataNode._ParticipateInGroopBy = _ParticipateInGroopBy;
            newDataNode._ParticipateInGroopByAsKey = _ParticipateInGroopByAsKey;
            newDataNode._ParticipateInSelectClause = _ParticipateInSelectClause;
            newDataNode._ParticipateInWereClause = _ParticipateInWereClause;
            newDataNode._Recursive = _Recursive;
            newDataNode._RecursiveMemberName = _RecursiveMemberName;
            newDataNode._RecursiveSteps = _RecursiveSteps;
            if (!_SearchCondition.UnInitialized && _SearchCondition.Value != null)
                newDataNode._SearchCondition.Value = _SearchCondition.Value.Clone(clonedObjects);
            newDataNode._SearchConditions = new List<SearchCondition>();
            foreach (var searchCondition in _SearchConditions)
            {
                if (searchCondition == null)
                    newDataNode._SearchConditions.Add(null);
                else
                    newDataNode._SearchConditions.Add(searchCondition.Clone(clonedObjects));
            }




            if (_DataSource != null)
            {
                object dataSource = null;
                if (clonedObjects.TryGetValue(_DataSource, out dataSource))
                    newDataNode._DataSource = dataSource as DataSource;
                else
                    newDataNode._DataSource = _DataSource.Clone(clonedObjects);
            }

            newDataNode._Temporary = _Temporary;

            newDataNode._ThroughRelationTable = _ThroughRelationTable;
            newDataNode._TimePeriodEndDate = _TimePeriodEndDate;
            newDataNode._TimePeriodStartDate = _TimePeriodStartDate;
            newDataNode._Type = _Type;
            if (_ValueTypePath != null)
                newDataNode._ValueTypePath = _ValueTypePath.Clone(clonedObjects);
            if (!_ValueTypePathDiscription.UnInitialized && _ValueTypePathDiscription.Value != null)
                newDataNode._ValueTypePathDiscription.Value = _ValueTypePathDiscription.Value;
            newDataNode.Aliases = new List<string>(Aliases);
            newDataNode.BranchAliass = new List<string>(BranchAliass);
            newDataNode.CastingParentType = CastingParentType;
            newDataNode.ClassifierFullName = ClassifierFullName;
            newDataNode.ClassifierImplementationUnitName = ClassifierImplementationUnitName;
            newDataNode.HasStorageCellConstrain = HasStorageCellConstrain;
            newDataNode.HasTimePeriodConstrain = HasTimePeriodConstrain;

            newDataNode.InDataSources = InDataSources;
            newDataNode.OrderBy = OrderBy;


            newDataNode.OrderByDataNodes = new List<DataNode>();
            foreach (DataNode orderByDataNode in OrderByDataNodes)
            {
                object createdOrderByDataNode = null;
                if (clonedObjects.TryGetValue(orderByDataNode, out createdOrderByDataNode))
                    newDataNode.OrderByDataNodes.Add(createdOrderByDataNode as DataNode);
                else
                    newDataNode.OrderByDataNodes.Add(orderByDataNode.Clone(clonedObjects));
            }

            foreach (var criterion in SearchCriterions)
            {
                object createdCriterion = null;
                if (clonedObjects.TryGetValue(criterion, out createdCriterion))
                    newDataNode.SearchCriterions.Add(createdCriterion as Criterion);
                else
                    newDataNode.SearchCriterions.Add(criterion.Clone(clonedObjects));
            }
        }

        /// <summary>
        /// Returns valuetype subDatatNodes and recursively valuetype subDataNodes of subDataNode 
        /// Excluded ObjectAttribute DataNodes and value type DataNodes
        /// </summary>
        /// <MetaDataID>{0031c7fa-69a6-405c-9505-ca34dd89974c}</MetaDataID>
        public List<DataNode> GetValueTypeRelatedDataNodes()
        {

            List<DataNode> relatedDataNodes = new List<DataNode>();
            if (ValueTypePath.Count == 0)
                return relatedDataNodes;

            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    continue;
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                {
                    relatedDataNodes.AddRange(subDataNode.GetValueTypeRelatedDataNodes());
                    continue;
                }
                relatedDataNodes.Add(subDataNode);
            }
            return relatedDataNodes;
        }


        /// <MetaDataID>{323eb76c-1966-461b-8da3-cec3bf4025f9}</MetaDataID>
        public override string ToString()
        {
            return "DataNode : " + FullName;
        }



        /// <MetaDataID>{0d630d43-acae-4b7c-9dd0-ef5bd9466caa}</MetaDataID>
        private SearchCondition GetGroupingResultSearchCondition(SearchCondition searchCondition)
        {
            if (searchCondition == null)
                return null;
            bool groupDataSourceNodeSearchCondition = true;
            foreach (DataNode dataNode in searchCondition.DataNodes)
            {
                if (!dataNode.IsParentDataNode(this))
                {
                    groupDataSourceNodeSearchCondition = false;
                    break;
                }
            }

            if (!groupDataSourceNodeSearchCondition && searchCondition.SearchTerms.Count == 1 && searchCondition.SearchTerms[0].SearchFactors.Count > 1)
            {
                List<SearchFactor> searchFactors = new List<SearchFactor>();
                foreach (SearchFactor searchFactor in searchCondition.SearchTerms[0].SearchFactors)
                {
                    if (searchFactor.SearchCondition != null)
                    {
                        var partialSearchCondition = GetGroupingResultSearchCondition(searchFactor.SearchCondition);
                        if (partialSearchCondition != null)
                            searchFactors.Add(new SearchFactor(partialSearchCondition));
                    }
                    else
                    {
                        SearchTerm searchTerm = new SearchTerm(new List<SearchFactor>() { searchFactor });
                        var partialSearchCondition = GetGroupingResultSearchCondition(new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery));
                        if (partialSearchCondition != null)
                            searchFactors.Add(searchFactor);
                    }
                }
                if (searchFactors.Count > 0)
                {
                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery);
                    return newSearchCondition;

                }
            }


            if (groupDataSourceNodeSearchCondition)
                return searchCondition;
            else
                return null;

        }


        /// <MetaDataID>{dcca76dc-ce2f-489b-a891-3cd671577a72}</MetaDataID>
        private SearchCondition GetGroupingSourceSearchCondition(SearchCondition searchCondition)
        {
            if (searchCondition == null)
                return null;
            bool groupDataNodeSearchCondition = true;
            foreach (DataNode dataNode in searchCondition.DataNodes)
            {
                if (dataNode.IsParentDataNode(this))
                {
                    groupDataNodeSearchCondition = false;
                    break;
                }
            }

            if (!groupDataNodeSearchCondition && searchCondition.SearchTerms.Count == 1 && searchCondition.SearchTerms[0].SearchFactors.Count > 1)
            {
                List<SearchFactor> searchFactors = new List<SearchFactor>();
                foreach (SearchFactor searchFactor in searchCondition.SearchTerms[0].SearchFactors)
                {
                    if (searchFactor.SearchCondition != null)
                    {
                        var partialSearchCondition = GetGroupingSourceSearchCondition(searchFactor.SearchCondition);
                        if (partialSearchCondition != null)
                            searchFactors.Add(new SearchFactor(partialSearchCondition));
                    }
                    else
                    {
                        SearchTerm searchTerm = new SearchTerm(new List<SearchFactor>() { searchFactor });
                        var partialSearchCondition = GetGroupingSourceSearchCondition(new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery));
                        if (partialSearchCondition != null)
                            searchFactors.Add(searchFactor);
                    }
                }
                if (searchFactors.Count > 0)
                {
                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery);
                    return newSearchCondition;

                }
            }

            if (groupDataNodeSearchCondition)
                return searchCondition;
            else
                return null;
        }

        /// <MetaDataID>{4db51d67-e8ef-449f-8199-6bcea914e677}</MetaDataID>
        internal protected bool HasGroupResultCriterions(SearchCondition searchCondition)
        {
            if (searchCondition == null)
                return false;
            foreach (var criterion in searchCondition.Criterions)
            {
                if (IsGroupResultCriterion(criterion))
                    return true;
            }
            return false;
        }
        /// <MetaDataID>{5b23c502-c48d-4c55-949d-4c1a2691ef45}</MetaDataID>
        bool IsGroupResultCriterion(Criterion criterion)
        {
            if (criterion.LeftTermDataNode is AggregateExpressionDataNode && !criterion.LeftTermDataNode.IsSameOrParentDataNode(this))
                return true;
            if (criterion.RightTermDataNode is AggregateExpressionDataNode && !criterion.RightTermDataNode.IsSameOrParentDataNode(this))
                return true;
            return false;
        }

        /// <MetaDataID>{676832b6-5617-4875-beb7-7bad50f5fc58}</MetaDataID>
        public SearchCondition RemoveGroupResultCriterions(SearchCondition searchCondition)
        {
            if (!HasGroupResultCriterions(searchCondition))
                return searchCondition;

            if (searchCondition.SearchTerms.Count > 1)
            {
                return null;
            }
            else
            {
                List<SearchFactor> searchFactors = new List<SearchFactor>();
                foreach (var searchFactor in searchCondition.SearchTerms[0].SearchFactors.ToArray())
                {
                    if (searchFactor.Criterion != null && !IsGroupResultCriterion(searchFactor.Criterion))
                        searchFactors.Add(searchFactor.Clone());

                    if (searchFactor.SearchCondition != null && HasGroupResultCriterions(searchFactor.SearchCondition))
                    {
                        searchCondition = searchFactor.SearchCondition;
                        searchCondition = RemoveGroupResultCriterions(searchCondition);
                        if (searchCondition != null)
                            searchFactors.Add(new SearchFactor(searchCondition));

                    }
                }
                searchCondition = new SearchCondition(new System.Collections.Generic.List<SearchTerm>() { new SearchTerm(searchFactors) }, ObjectQuery);
                return searchCondition;
            }
        }

        /// <MetaDataID>{b4b5373c-9aa2-4656-9dd4-0983000307a5}</MetaDataID>
        public virtual void AddSearchCondition(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchCondition searchCondition)
        {

            if (Type == DataNodeType.OjectAttribute)
            {
                ParentDataNode.AddSearchCondition(searchCondition);
                return;
            }
            bool alreadyExist = false;
            foreach (var existingSearchCondition in _SearchConditions)
            {

                if (existingSearchCondition == null && searchCondition == null)
                {
                    alreadyExist = true;
                    break;
                }
                if (existingSearchCondition != null && searchCondition != null && existingSearchCondition.ToString() == searchCondition.ToString())
                {
                    alreadyExist = true;
                    break;
                }
            }
            if (!alreadyExist)
            {
                _SearchConditions.Add(searchCondition);
                if (searchCondition != null)
                {
                    foreach (var criterion in searchCondition.Criterions)
                    {
                        if (criterion.RightTermDataNode != null)
                            criterion.RightTermDataNode.SearchCriterions.Add(criterion);
                        if (criterion.LeftTermDataNode != null)
                            criterion.LeftTermDataNode.SearchCriterions.Add(criterion);
                    }
                }
            }
            RemoveSearchConditionCashingData();

            //if (RealParentDataNode != null && RealParentDataNode.Type == DataNodeType.Object)
            //    RealParentDataNode.AddSearchCondition(searchCondition);

        }



        /// <MetaDataID>{12ff6b60-99df-418c-b54e-c1aee252c2e4}</MetaDataID>
        public virtual void RemoveSearchCondition(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchCondition searchCondition)
        {
            _SearchConditions.Remove(searchCondition);
            RemoveSearchConditionCashingData();
        }





        /// <MetaDataID>{fde05a85-37af-4f67-9f3c-27758b257088}</MetaDataID>
        protected Member<SearchCondition> _SearchCondition = new Member<SearchCondition>();
        ///<summary>
        ///Define a constrain condition wich derived from all search conditions of data node 
        ///</summary>
        /// <MetaDataID>{89c79552-3505-46da-9d77-f8463d29f85b}</MetaDataID>
        [Association("ConstrainCondition", Roles.RoleA, "6e04fd20-5ab1-44c0-9138-e6b9e9868349")]
        [IgnoreErrorCheck]
        public virtual SearchCondition SearchCondition
        {
            get
            {
                if (_SearchCondition.UnInitialized)
                {
                    var branchSearchConditions = BranchSearchConditions;

                    if (branchSearchConditions.Count > 0)
                    {
                        if (FilterNotActAsLoadConstraint)
                        {
                            _SearchCondition.Value = null;
                            return null;
                        }

                        if (branchSearchConditions.Count == 1)
                        {
                            if (branchSearchConditions[0] == null)
                                return branchSearchConditions[0];
                            if (Type != DataNodeType.Group)
                                return branchSearchConditions[0];
                        }

                        foreach (var searchCondition in branchSearchConditions)
                        {

                            if (searchCondition == null)
                            {
                                _SearchCondition.Value = null;
                                return null;
                            }
                            bool allContainsAsPart = true;
                            foreach (var innerSearchCondition in branchSearchConditions)
                            {
                                if (innerSearchCondition == null)
                                {
                                    _SearchCondition.Value = null;
                                    return null;
                                }
                                if (searchCondition.ToString() == innerSearchCondition.ToString())
                                    continue;
                                if (innerSearchCondition != null && !innerSearchCondition.ContainsSearchCondition(searchCondition))
                                {
                                    allContainsAsPart = false;
                                    break;
                                }
                            }
                            if (allContainsAsPart)
                            {
                                _SearchCondition.Value = searchCondition;
                                return searchCondition;
                            }
                        }

                    }
                    else if (RealParentDataNode != null)
                    {
                        _SearchCondition.Value = RealParentDataNode.SearchCondition;
                        return RealParentDataNode.SearchCondition;
                    }
                    _SearchCondition.Value = null;
                    return null;
                }
                else
                    return _SearchCondition.Value;
            }
        }

        /// <MetaDataID>{f0610ebb-455c-4cb6-8766-f1f230a5d23f}</MetaDataID>
        void RemoveSearchConditionCashingData()
        {
            if (!_SearchCondition.UnInitialized)
                _SearchCondition = new Member<SearchCondition>();
            if (ParentDataNode != null)
                ParentDataNode.RemoveSearchConditionCashingData();

        }

        /// <exclude>Excluded</exclude>
        private List<SearchCondition> _SearchConditions = new List<SearchCondition>();
        /// <summary>
        /// Defines a collection with searchCondition (filter data expression). 
        /// When query retrieves data from data node and use more than one search condition  (filter data expressions)
        /// SearchCondition collections contains all that conditions
        /// </summary>
        [RoleBMultiplicityRange(0)]
        [Association("Filter", Roles.RoleA, "{32981580-7380-4EBB-BCC4-CF3E865E329B}")]
        public virtual List<SearchCondition> SearchConditions
        {
            get
            {
                //if (_SearchCondition != null)
                return new List<SearchCondition>(_SearchConditions);
                //if (ParentDataNode != null)
                //    return ParentDataNode.SearchCondition;
                //return null;
            }

        }


        /// <MetaDataID>{e6692416-47dc-4570-83cb-b5f2de37a0b8}</MetaDataID>
        public virtual List<SearchCondition> BranchSearchConditions
        {
            get
            {

                List<SearchCondition> searchConditions = new List<SearchCondition>(_SearchConditions);

                foreach (DataNode dataNode in SubDataNodes)
                {

                    foreach (SearchCondition searchCondition in dataNode.BranchSearchConditions)
                    {
                        bool alreadyExist = false;
                        foreach (var existingSearchCondition in searchConditions)
                        {

                            if (existingSearchCondition == null && searchCondition == null)
                            {
                                alreadyExist = true;
                                break;
                            }

                            if (existingSearchCondition != null)
                            {
                                foreach (Criterion criterion in existingSearchCondition.Criterions)
                                {
                                    foreach (ComparisonTerm comparisonTerm in criterion.ComparisonTerms)
                                    {
                                        if (comparisonTerm.OQLStatement == null)
                                            comparisonTerm.OQLStatement = dataNode.ObjectQuery;
                                    }
                                }
                            }
                            if (existingSearchCondition != null && searchCondition != null && existingSearchCondition == searchCondition)
                            {
                                alreadyExist = true;
                                break;
                            }
                        }
                        if (!alreadyExist)
                            searchConditions.Add(searchCondition);
                    }
                }

                //if (RealParentDataNode != null && RealParentDataNode.Type == DataNodeType.Object)
                //    RealParentDataNode.AddSearchCondition(searchCondition);


                //if (_SearchCondition != null)
                return searchConditions;
                //if (ParentDataNode != null)
                //    return ParentDataNode.SearchCondition;
                //return null;
            }

        }

        /// <MetaDataID>{96ee3d2f-13ef-4e6c-a34e-13d5187eee39}</MetaDataID>
        public virtual void RemoveAllSearchCondition()
        {
            _SearchConditions.Clear();
        }



        /// <exclude>Excluded</exclude>
        public bool _Temporary = false;
        /// <MetaDataID>{ba1447ac-2a20-4f04-8e45-c3edaaeaa1cd}</MetaDataID>
        /// <summary>
        /// The DataNode created temporally for anonymous type of Linq. 
        /// </summary>
        public bool Temporary
        {
            get
            {
                return _Temporary;
            }
            set
            {
                _Temporary = value;
            }
        }

        /// <MetaDataID>{48860182-77b4-49da-88fa-bf00948ecf34}</MetaDataID>
        /// <summary>
        /// Defines the DataNode Identity
        /// It is useful for query distribution. 
        /// </summary>
        public readonly Guid Identity = Guid.NewGuid();

        /// <summary>
        /// This member defines the parsed path from OQL script.
        /// </summary>
        [Association("DataNodePath", typeof(Path), Roles.RoleA, "{BA63E733-7635-40AE-97C5-66E6A4D5F45A}")]
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [NonSerialized]
        public Path Path;

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery _ObjectQuery;

        /// <MetaDataID>{7ae783cf-761b-4175-bfef-8403f60e72e1}</MetaDataID>
        /// <summary>All DataNode produced and belong to an ObjectQuery.</summary>
        [RoleBMultiplicityRange(1, 1)]
        [Association("QueryData", typeof(ObjectQuery), Roles.RoleB, "{DC4B5A5E-3670-435F-894E-AC743AA87358}")]
        public OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery ObjectQuery
        {
            get
            {
                if (ParentDataNode != null)
                    return ParentDataNode.ObjectQuery;
                else
                    return _ObjectQuery;
            }
            set
            {
                _ObjectQuery = value;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0F155022-98BE-4313-9753-91CD65279BAA}</MetaDataID>
        protected DataNodeType _Type;
        /// <summary>The DataNode can be any of three types. 
        /// 1. Namespace and can not retrieve data from this DataNode. 
        /// 2. Object and from this DataNode you can retrieve objects. 
        /// 3. OjectAttribute and from this DataNode you can retrieve a field of objects.
        /// 4. Unknown in this case the translator can't find the DataNode name in metadata. 
        /// The recent result of this situation is error of type "There isn't namespace or class with name XXX" or "XXX isn't member of ". </summary>
        /// <MetaDataID>{59024337-42FB-4273-9B6D-DB08F0E9FCFF}</MetaDataID>
        public virtual DataNodeType Type
        {
            get
            {

                if (_Type == DataNodeType.Unknown)
                {
                    if (AssignedMetaObject is MetaDataRepository.Attribute && (AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        _Type = DataNodeType.Object;
                        return _Type;
                    }
                    if (AssignedMetaObject is MetaDataRepository.Attribute)
                    {
                        if (SubDataNodes.Count==0)
                            _Type = DataNodeType.OjectAttribute;
                        else
                            _Type = DataNodeType.Object;

                        return _Type;
                    }

                    if (AssignedMetaObject is MetaDataRepository.Classifier)
                    {
                        _Type = DataNodeType.Object;
                        return _Type;
                    }

                    if (AssignedMetaObject is MetaDataRepository.AssociationEnd)
                        if (((MetaDataRepository.AssociationEnd)AssignedMetaObject).Specification != null)
                        {
                            _Type = DataNodeType.Object;
                            return _Type;
                        }

                    if (AssignedMetaObject is MetaDataRepository.Namespace)
                    {
                        _Type = DataNodeType.Namespace;
                        return _Type;
                    }

                }
                return _Type;
            }
            ///TODO 
            set
            {
                _Type = value;
            }

        }


        /// <exclude>Excluded</exclude>
        internal int? _DataSourceColumnIndex;
        /// <MetaDataID>{7848ede0-5a3f-417a-bd5a-16579acc17fc}</MetaDataID>
        /// <summary>
        /// Defines the index of column where contains the data for DataNode
        /// </summary>
        internal int DataSourceColumnIndex
        {
            get
            {
                if (!_DataSourceColumnIndex.HasValue)
                {
                    if (DataSource == null)
                    {
                        DataNode dataNodeWithDataSource = ParentDataNode;
                        while (dataNodeWithDataSource.Type == DataNodeType.OjectAttribute)
                            dataNodeWithDataSource = dataNodeWithDataSource.ParentDataNode;
                        if (dataNodeWithDataSource.DataSource != null)
                            _DataSourceColumnIndex = dataNodeWithDataSource.DataSource.GetColumnIndex(this);
                    }
                    else
                        _DataSourceColumnIndex = DataSource.ObjectIndex;
                }
                return _DataSourceColumnIndex.Value;
            }

        }


        /// <MetaDataID>{52c6aa0b-9039-4157-8517-f972b0d3eee4}</MetaDataID>
        /// <summary>
        /// Check the parameter data node if it is the same or  ancestor DataNode 
        /// </summary>
        public bool IsSameOrParentDataNode(DataNode dataNode)
        {

            if (dataNode == this)
                return true;
            if (ParentDataNode != null)
                return ParentDataNode.IsSameOrParentDataNode(dataNode);
            return false;
        }


        /// <MetaDataID>{f03f8ab1-4a29-429f-9a93-3727e9df823d}</MetaDataID>
        /// <summary>
        /// /// Check the parameter data node if it is ancestor DataNode 
        /// </summary>
        public bool IsParentDataNode(DataNode dataNode)
        {

            if (ParentDataNode != null)
                return ParentDataNode.IsSameOrParentDataNode(dataNode);
            return false;
        }
        /// <MetaDataID>{3c46b36e-3d8f-4df2-b1f0-10a4cd5a3bfc}</MetaDataID>
        /// Parent Data Node with data in storage.
        /// In DataNodes tree there are DataNodes with data which produced 
        /// on the fly and for example group DataNode and DataNodes with data in storage.
        /// </summary>
        public DataNode RealParentDataNode
        {
            get
            {
                if (_ParentDataNode != null && (_ParentDataNode.Type == DataNodeType.Group || _ParentDataNode.Type == DataNodeType.OjectAttribute))
                    return _ParentDataNode.RealParentDataNode;
                else
                    return _ParentDataNode;
            }
        }
        /// <MetaDataID>{87cb467a-a5ae-440e-a284-766734ba0ed1}</MetaDataID>
        /// <summary>
        /// Sub Data Nodes with data in storage.
        /// In DataNodes tree there are DataNodes with data which produced 
        /// on the fly and for example group DataNode and DataNodes with data in storage.
        /// </summary>
        public System.Collections.Generic.List<DataNode> RealSubDataNodes
        {
            get
            {
                System.Collections.Generic.List<DataNode> realSubDataNodes = new System.Collections.Generic.List<DataNode>();
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode is DerivedDataNode || subDataNode.Type == DataNodeType.Key)
                        continue;

                    if (subDataNode.Type == DataNodeType.Group)
                    {
                        foreach (DataNode groupingSubDataNode in subDataNode.RealSubDataNodes)
                            realSubDataNodes.Add(groupingSubDataNode);
                    }
                    else
                        realSubDataNodes.Add(subDataNode);
                }
                return realSubDataNodes;
            }
        }

        /// <exclude>Excluded</exclude>
        private DataNode _ParentDataNode;
        /// <MetaDataID>{E0A0F0E1-7F3A-44B6-89B0-E2E1FF63CF79}</MetaDataID>
        public DataNode ParentDataNode
        {
            get
            {

                return _ParentDataNode;
            }
            set
            {
                if (_ParentDataNode == value)
                    return;
                if (value != null && value.IsSameOrParentDataNode(this))
                    throw new System.Exception("Parent and sub datanode are the same.");
                if (_ParentDataNode != null)
                    if (_ParentDataNode.SubDataNodes.Contains(this))
                        _ParentDataNode.SubDataNodes.Remove(this);
                _ParentDataNode = value;
                if (_ParentDataNode != null)
                    _ParentDataNode.SubDataNodes.Add(this);
            }

        }

        /// <MetaDataID>{7bc3a3e1-4ba8-477e-b091-98410fca4c99}</MetaDataID>
        MetaObjectID _AssignedMetaObjectIdenty;
        /// <MetaDataID>{f31053b5-d447-4397-9af0-35f165fb025c}</MetaDataID>
        public MetaObjectID AssignedMetaObjectIdenty
        {
            get
            {
                return _AssignedMetaObjectIdenty;
            }
            set
            {
                _AssignedMetaObjectIdenty = value;
            }
        }




        /// <exclude>Excluded</exclude>
        [NonSerialized]
        private OOAdvantech.MetaDataRepository.MetaObject _AssignedMetaObject;
        /// <MetaDataID>{DB970712-CBA1-4D9A-BAD1-2ACAD17CC176}</MetaDataID>
        public virtual MetaObject AssignedMetaObject
        {
            get
            {
                return _AssignedMetaObject;
            }
            set
            {
                _AssignedMetaObject = value;
                if (_AssignedMetaObject != null)
                    AssignedMetaObjectIdenty = _AssignedMetaObject.Identity;

                if (_AssignedMetaObject is MetaDataRepository.Namespace && ParentDataNode != null && ParentDataNode.AssignedMetaObject == null)
                    ParentDataNode.AssignedMetaObject = value.Namespace;


                if (!String.IsNullOrEmpty(OfTypeFilterClassifierFullName))
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(OfTypeFilterClassifierFullName, ClassifierImplementationUnitName));

                if ((_AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                    _Classifier != null &&
                    (_AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass == _Classifier) &&
                    string.IsNullOrEmpty(_Name))
                    _Name = (_AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name;
                _Classifier = null;
                object initClassifier = Classifier;
                //string valueTypePathDiscription = ValueTypePathDiscription;
                //ValueTypePath valueTypePath = ValueTypePath;
            }

        }

        /// <MetaDataID>{4CFB1AFA-4446-4AB5-8635-F92940D5A90C}</MetaDataID>
        public System.Collections.Generic.List<DataNode> SubDataNodes = new System.Collections.Generic.List<DataNode>();


        /// <MetaDataID>{a1338e32-c1c5-4992-ad02-d410c1912fc6}</MetaDataID>
        internal string ClassifierFullName;
        /// <MetaDataID>{dd5c6651-26ab-4e31-991c-21da1ed05b88}</MetaDataID>
        internal string ClassifierImplementationUnitName;

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        private OOAdvantech.MetaDataRepository.Classifier _Classifier;
        /// <MetaDataID>{8E92C81E-A805-4561-9686-4C611E9B633E}</MetaDataID>
        public virtual Classifier Classifier
        {
            get
            {
                if (_Classifier != null)
                    return _Classifier;

                if (!string.IsNullOrEmpty(ClassifierFullName) && _Classifier == null)
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(ClassifierFullName, ClassifierImplementationUnitName));

                if (_Classifier != null)
                    return _Classifier;

                if (typeof(MetaDataRepository.Classifier).GetMetaData().IsInstanceOfType(AssignedMetaObject))
                    _Classifier = (MetaDataRepository.Classifier)AssignedMetaObject;
                if (AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    //TODO Όταν υπάρχει σε LinkClass για να μήν γίνει λάθος θα πρέπει να διασφαλιστεί ότι το όμομα
                    //της association Θα είναι διαφορετικό από τα Assocition end ονόματα.

                    if (((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.LinkClass != null && Name == ((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.Name)
                        _Classifier = ((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.LinkClass;
                    else
                        _Classifier = ((MetaDataRepository.AssociationEnd)AssignedMetaObject).Specification;

                }
                if (AssignedMetaObject is MetaDataRepository.Attribute)
                {
                    _Classifier = (AssignedMetaObject as MetaDataRepository.Attribute).Type;
                    System.Type type = _Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;



                    if (TypeHelper.IsEnumerable(type))
                    {
                        type = TypeHelper.GetElementType(type);
                        //}

                        //    if (type.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]) != null
                        //    && (DotNetMetaDataRepository.Type.GetInterface(type.GetMetaData().GetMethod("GetEnumerator").ReturnType, typeof(System.Collections.IEnumerator).FullName) != null
                        //    || type.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().IsSubclassOf(typeof(System.Collections.IEnumerator)))
                        //    && type.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().IsGenericType
                        //    && type.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().GetGenericArguments().Length == 1)
                        //{
                        //    type = type.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().GetGenericArguments()[0];
                        _Classifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Classifier == null)
                        {
                            DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.Assembly.GetComponent(type.GetMetaData().Assembly) as DotNetMetaDataRepository.Assembly;
                            long count = assembly.Residents.Count;
                            _Classifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;

                        }
                    }
                }
                if (_Classifier == null && Type == DataNodeType.Group)
                {
                    foreach (DataNode subDataNode in SubDataNodes)
                    {
                        if (subDataNode.Type != DataNodeType.Key)
                        {
                            _Classifier = subDataNode.Classifier;
                            break;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(OfTypeFilterClassifierFullName))
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(OfTypeFilterClassifierFullName, ClassifierImplementationUnitName));

                if (_Classifier != null)
                {

                    ClassifierFullName = _Classifier.FullName;
                    ClassifierImplementationUnitName = _Classifier.ImplementationUnit.FullName;
                    if (ClassifierFullName != null && ClassifierFullName.IndexOf('.') == -1)
                    {
                        System.Type type = _Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                    }
                }


                return _Classifier;

            }
            set
            {
                _Classifier = value;
                if (_Classifier != null)
                {
                    ClassifierFullName = _Classifier.FullName;
                    ClassifierImplementationUnitName = _Classifier.ImplementationUnit.FullName;
                    if (ClassifierFullName != null && ClassifierFullName.IndexOf('.') == -1)
                    {
                        System.Type type = _Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4FBCF911-3D31-4218-B11D-CC1281D1A5FA}</MetaDataID>
        private DataNode _DataNodeWithRootDataSource;



        /// <summary>Defines the root data node for the data sources building. 
        /// The system starts to collect the storage cells which needed to retrieve the data of query, 
        /// from data node with the less number of objects (for example data node with time period constrain or object ID constrain etc). 
        /// This technique is useful because reduce the number of storage cells where the system search for data. </summary>
        /// <MetaDataID>{FE59605A-48F4-427F-BFD7-3B1D24272DAD}</MetaDataID>
        public DataNode DataNodeWithRootDataSource
        {
            get
            {
                if (_DataNodeWithRootDataSource == null)
                    return HeaderDataNode;
                else
                    return _DataNodeWithRootDataSource;
            }
            set
            {

                if (_DataNodeWithRootDataSource == null)
                    _DataNodeWithRootDataSource = value;
                else
                {
                    //if there is more than one data nodes with storage cell constrain the first will be
                    //the root.
                    if (_DataNodeWithRootDataSource.HasStorageCellConstrain)
                        return;

                    //TODO test case for more than one time period Constrains
                    if (_DataNodeWithRootDataSource.HasTimePeriodConstrain && value.HasTimePeriodConstrain)
                        throw new System.Exception("There is more than one time period Constrains");
                    _DataNodeWithRootDataSource = value;
                }
            }
        }


        #region DataNode Participation

        /// <MetaDataID>{97504eb0-38aa-4ed7-a8a6-4b798acf89a2}</MetaDataID>
        public bool BranchParticipateInSelectClause
        {
            get
            {
                if (_ParticipateInSelectClause)
                    return true;
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode.BranchParticipateInSelectClause)
                    {
                        #region Code Removed
                        //if (GroupingParentDataNode != null && !GroupingParentDataNode.ParticipateInSelectClause)
                        //{
                        //    foreach (DataNode groupKeyDataNode in GroupingParentDataNode.GroupKeyDataNodes)
                        //    {
                        //        if (IsParentDataNode(groupKeyDataNode) || groupKeyDataNode == this)
                        //            return true;
                        //    }
                        //    return false;
                        //}
                        #endregion
                        return true;
                    }
                }
                return _ParticipateInSelectClause;
            }

        }


        /// <exclude>Excluded</exclude>
        private bool _ParticipateInWereClause;
        /// <MetaDataID>{F3082673-621E-4044-9829-CFBF7514B9C4}</MetaDataID>
        /// <summary>Indicate when the data node participate in where clause.
        /// It is useful when system build the tables joins. </summary>
        public virtual bool ParticipateInWereClause
        {
            get
            {
                return _ParticipateInWereClause;
            }
            set
            {
                _ParticipateInWereClause = value;
            }
        }


        /// <MetaDataID>{A3579A59-0631-4D84-B952-D142AB528236}</MetaDataID>
        /// <summary>The property BranchParticipateInWereClause tel as,
        /// if some one from sub data nodes or sub data node of sub data node etc. 
        /// participates in search constrain. </summary>
        public bool BranchParticipateInWereClause
        {
            get
            {
                if (_ParticipateInWereClause)
                    return true;
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode.BranchParticipateInWereClause)
                        return true;
                }
                return _ParticipateInWereClause;
            }
        }




        /// <exclude>Excluded</exclude>
        private bool _ParticipateInSelectClause;
        /// <summary>This member tells as if the data node is in select list. </summary>
        /// <MetaDataID>{214A760E-5F31-42F9-A134-CA670111DC23}</MetaDataID>
        public virtual bool ParticipateInSelectClause
        {
            get
            {
                return _ParticipateInSelectClause;
            }
            set
            {
                _ParticipateInSelectClause = value;
            }
        }

        #region Code removed
        ///// <MetaDataID>{7eea5165-e2e2-47bf-871b-a5269a9ccf66}</MetaDataID>
        //private void RefreshGroupedItemPartitipation()
        //{
        //    if (Type != DataNodeType.Group)
        //        return;

        //    foreach (DataNode groupItemDataNode in GroupKeyDataNodes)
        //    {
        //        foreach (DataNode subDataNode in SubDataNodes)
        //        {
        //            if (subDataNode.Name == groupItemDataNode.Name || subDataNode.HasAlias(groupItemDataNode.Alias))
        //            {
        //                if (subDataNode.BranchParticipateInSelectClause)
        //                    SychronizeSelectPartitipation(subDataNode, groupItemDataNode);
        //                break;
        //            }
        //        }
        //    }
        //}
        ///// <MetaDataID>{9d329ac7-cbec-45bf-9ba5-740815ae14dc}</MetaDataID>
        //private void SychronizeSelectPartitipation(DataNode groupKeyDataNode, DataNode dataTreeDataNode)
        //{
        //    if (groupKeyDataNode.ParticipateInSelectClause && !dataTreeDataNode.ParticipateInSelectClause)
        //        ObjectQuery.AddSelectListItem(dataTreeDataNode);
        //    foreach (DataNode keySubDataNode in groupKeyDataNode.SubDataNodes)
        //    {
        //        if (keySubDataNode.BranchParticipateInSelectClause)
        //        {
        //            DataNode equivalentDataNode = null;
        //            foreach (DataNode subDataNode in dataTreeDataNode.SubDataNodes)
        //            {
        //                if (subDataNode.Name == keySubDataNode.Name || subDataNode.HasAlias(keySubDataNode.Alias))
        //                {
        //                    equivalentDataNode = subDataNode;
        //                    break;
        //                }
        //            }
        //            if (equivalentDataNode == null)
        //            {
        //                equivalentDataNode = new DataNode(keySubDataNode.ObjectQuery);
        //                equivalentDataNode.Name = keySubDataNode.Name;
        //                equivalentDataNode.AssignedMetaObject = keySubDataNode.AssignedMetaObject;
        //                equivalentDataNode.Alias = keySubDataNode.Alias;
        //                foreach (string alias in keySubDataNode.Aliases)
        //                    equivalentDataNode.Alias = alias;
        //                equivalentDataNode.ParentDataNode = dataTreeDataNode;

        //            }
        //            SychronizeSelectPartitipation(keySubDataNode, equivalentDataNode);


        //        }

        //    }

        //}
        #endregion


        #endregion

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5649EB96-0292-4F96-A8EA-A509F2F13303}</MetaDataID>
        private System.DateTime _TimePeriodEndDate;
        /// <summary>Define the end date of a time period. Time period refer to the creation date of objects.
        /// This member has meaning  only if  flag HasTimePeriodConstrain is true. </summary>
        /// <MetaDataID>{6DF1C6B9-15B7-4E14-B3CF-94AE79A2B4FA}</MetaDataID>
        public System.DateTime TimePeriodEndDate
        {
            set
            {
                _TimePeriodEndDate = value;
            }
            get
            {
                if (!HasTimePeriodConstrain)
                    throw new System.Exception("There isn't Time period constrain");
                return _TimePeriodEndDate;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{605450DF-D108-4660-9238-A716CFB30D99}</MetaDataID>
        private System.DateTime _TimePeriodStartDate;
        /// <summary>Define the start date of a time period. Time period refer to the creation date of objects.
        /// This member has meaning  only if  flag HasTimePeriodConstrain is true. </summary>
        /// <MetaDataID>{81A761B3-EAF6-46BB-8275-F376AD13AD91}</MetaDataID>
        public System.DateTime TimePeriodStartDate
        {
            set
            {
                _TimePeriodStartDate = value;
            }
            get
            {
                if (!HasTimePeriodConstrain)
                    throw new System.Exception("There isn't Time period constrain");
                return _TimePeriodStartDate;
            }
        }



        /// <summary>Define the root data node of data tree which contains the data node with this member. </summary>
        /// <MetaDataID>{6F8725B5-3EE5-45D9-ACED-852AE9A44F88}</MetaDataID>
        public DataNode HeaderDataNode
        {
            get
            {
                if (ParentDataNode != null)
                    return ParentDataNode.HeaderDataNode;
                else
                    return this;
            }
        }


        /// <summary>Define all the paths in query, which referred to the data of data node. </summary>
        /// <MetaDataID>{A7A7C3C3-9C6E-44A1-846E-5762DA9DB7C0}</MetaDataID>
        [NonSerialized]
        public System.Collections.Generic.List<Path> RelatedPaths = new System.Collections.Generic.List<Path>();

        ///// <MetaDataID>{09667185-D46D-4898-BC73-6E836EDCBC86}</MetaDataID>
        //[NonSerialized]
        //public OOAdvantech.Collections.Member StructureSetMember;



        /// <exclude>Excluded</exclude>
        protected DataSource _DataSource;
        /// <MetaDataID>{48180662-F687-48BF-B6EE-023897107C8E}</MetaDataID>
        public virtual DataSource DataSource
        {
            set
            {
                if (value != null && value.DataNode != null && value.DataNode != this)
                    throw new System.Exception("Invalid DataSource");
                _DataSource = value;


                string valueTypePathDiscription = ValueTypePathDiscription;
                ValueTypePath valueTypePath = ValueTypePath;

            }
            get
            {
                return _DataSource;
            }
        }
        /// <MetaDataID>{a617ffd1-8548-4526-9e04-742a55fc9ed1}</MetaDataID>
        public DataNode GetDataNode(Guid identity)
        {
            if (Identity == identity)
                return this;
            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.Identity == identity)
                {
                    return subDataNode;
                }
            }
            foreach (DataNode subDataNode in SubDataNodes)
            {
                DataNode dataNode = subDataNode.GetDataNode(identity);
                if (dataNode != null)
                    return dataNode;
            }
            return null;
        }


        #region Retrieves Data

        ///// <MetaDataID>{0ce26cea-2e65-417a-895c-07062f31d379}</MetaDataID>
        //public void LoadData()
        //{
        //    LoadDataLocally();
        //}
        /// <MetaDataID>{373A1CA3-F456-482E-B9AF-C818EC952D74}</MetaDataID>
        public void GetData(IDataSet dataSet)
        {
            GetData(dataSet, null);

        }
        /// <MetaDataID>{e5b5af41-d2cf-484d-a35f-92452eb814ca}</MetaDataID>
        internal void GetData(IDataSet ObjectQueryData, OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.Collections.Generic.List<StorageDataLoader>> dataLoaders)
        {
            CollectRemoteDataInProcess(ObjectQueryData, dataLoaders);
            BuildTablesRelations();
        }





        ///// <MetaDataID>{4949BD46-B6F0-430C-9744-CA42B2026441}</MetaDataID>
        //void LoadDataLocally()
        //{
        //    foreach (DataNode dataNode in SubDataNodes) //Value types share parent data source
        //        dataNode.LoadDataLocally();

        //    if (_DataSource != null)
        //    {
        //        if (_DataSource.DataNode == this)
        //            _DataSource.LoadDataLocally();
        //    }
        //}

        /// <MetaDataID>{6213DC94-592D-498B-99A8-E11C809B1E72}</MetaDataID>
        /// <summary>This method collects all data from the context which are 
        /// in different processes even in different machines, 
        /// in the process of main object context of OQL query. </summary>
        internal void CollectRemoteDataInProcess(IDataSet dataSet, OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.Collections.Generic.List<StorageDataLoader>> queryResultDataLoaders)
        {
            ///Collect data for subdatanodes first
            foreach (DataNode dataNode in SubDataNodes)
                dataNode.CollectRemoteDataInProcess(dataSet, queryResultDataLoaders);
            if (DataSource != null && DataSource.DataNode == this)
            {
                if (queryResultDataLoaders != null)
                {
                    if (queryResultDataLoaders.ContainsKey(this.Identity))
                        DataSource.CollectRemoteDataInProcess(dataSet, queryResultDataLoaders[this.Identity]);
                    else
                        DataSource.CollectRemoteDataInProcess(dataSet, new OOAdvantech.Collections.Generic.List<StorageDataLoader>());
                }
                else
                    DataSource.CollectRemoteDataInProcess(dataSet, null);
            }
        }

        /// <MetaDataID>{7A062549-09C3-4E2E-8983-A8D5CBBF8E23}</MetaDataID>
        internal void BuildTablesRelations()
        {

            foreach (DataNode subDataNode in SubDataNodes)
                if (!(subDataNode is DerivedDataNode)) /// To avoid to build relation tow times for one data node
                    subDataNode.BuildTablesRelations();

            if (DataSource != null)
                DataSource.BuildTablesRelations();
        }


        //internal void AddTablesRelations()
        //{
        //    foreach (DataNode subDataNode in SubDataNodes)
        //        subDataNode.AddTablesRelations();
        //    if (DataSource != null)
        //        DataSource.AddTablesRelations();
        //}


        ///// <MetaDataID>{220e507f-4db2-413d-819a-37a51b5fe8f5}</MetaDataID>
        //public void RunAggregateFunctions()
        //{
        //    foreach (DataNode subDataNode in SubDataNodes)
        //        subDataNode.RunAggregateFunctions();
        //    if (DataSource != null)
        //        DataSource.RunAgregateFunctions();
        //}





        ///// <summary>
        ///// This method sets the associations 
        ///// fields of relate object to reproduce the objects link in memory. 
        ///// System must be load the data sources before call this function. 
        ///// </summary>
        ///// <MetaDataID>{41E06DAD-376D-4C88-AFEB-325718CF69EA}</MetaDataID>
        //internal void LoadObjectRelationLinks()
        //{

        //    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.NotSupported))
        //    {
        //        foreach (DataNode subDataNode in SubDataNodes)
        //        {
        //            if (Type == DataNodeType.Object &&
        //                subDataNode.Type == DataNodeType.Object)
        //            {

        //                if ((subDataNode.Prefetching || IsThereBackOnConstructionRoute(subDataNode)) &&
        //                    DataSource.HaveObjectsToActivate)
        //                {
        //                    (DataSource as StorageDataSource).LoadObjectRelationLinksEx(subDataNode);
        //                }
        //            }
        //            subDataNode.LoadObjectRelationLinks();
        //        }
        //        stateTransition.Consistent = true;
        //    }


        //}


        #endregion

        /// <MetaDataID>{E642B341-D4FA-4484-9918-D02EB0EB3421}</MetaDataID>
        [NonSerialized]
        internal StorageCell ObjectIDConstrainStorageCell;

        internal List<string> StorageCellIDsConstrain = new List<string>();


        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{4A1FF308-9668-4C7B-B86F-4DC0B0151931}</MetaDataID>
        /// <summary>Define the name of data node, actually is the name of path in most of cases. </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }






        /// <MetaDataID>{8bf88933-5d96-4640-ad2f-9bf46243fd1c}</MetaDataID>
        public System.Collections.Generic.List<string> Aliases = new System.Collections.Generic.List<string>();

        /// <MetaDataID>{69fbed77-5832-494a-adf4-3bace545804f}</MetaDataID>
        public bool HasAlias(string alias)
        {
            return BranchAliass.Contains(alias);
        }

        /// <MetaDataID>{237dd579-bf42-46e2-8b28-96bda4f5023d}</MetaDataID>
        public System.Collections.Generic.List<string> BranchAliass = new System.Collections.Generic.List<string>();


        /// <MetaDataID>{fd6697d0-2c26-4c9d-9f9f-3eddc8511600}</MetaDataID>
        public void AddBranchAlias(string branchAlias)
        {

            if (!string.IsNullOrEmpty(branchAlias) && !BranchAliass.Contains(branchAlias))
                BranchAliass.Add(branchAlias);
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{629F8E11-1946-456D-A596-B583C85E57D3}</MetaDataID>
        protected string _Alias;
        /// <MetaDataID>{D8DD5D16-A616-40A3-B336-C1DD0B7561C3}</MetaDataID>
        /// <summary>Define the alias name. The name of data node is the last string in path 
        /// for example the paths person.Age and persont.Parents.Age, 
        /// have the same name, to avoid names conflict we use the Alias for example ParentAge for  persont.Parents.Age. </summary>
        public virtual string Alias
        {
            set
            {
                if (value != null && _Alias != value && !Aliases.Contains(value))
                    Aliases.Add(value);
                AddBranchAlias(value);

                _Alias = value;

            }
            get
            {
                if (!string.IsNullOrEmpty(_Alias))
                    return _Alias;


                if (Path != null && Path.AliasName != null)
                {
                    Path path = Path;
                    while (!path.AggregationPath && path.Parent != null)
                        path = path.Parent;
                    if (!path.AggregationPath)
                        _Alias = Path.AliasName;// ObjectQuery.GetValidAlias(Path.AliasName);

                }

                if (AssignedMetaObject is MetaDataRepository.Attribute && _Alias == null
                    && !(AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    return _Alias;



                if (Classifier != null && ObjectQuery != null)
                {
                    if (_Alias == null && Type == DataNodeType.Object)
                        _Alias = ObjectQuery.GetValidAlias("Abstract_" + Classifier.Name);

                }
                return _Alias;
            }
        }



        ///// <summary>If it is true then the data source table has the identity coloumns of foreign key relation,
        ///// otherwise table has the reference columns. Always the identity columns has unique constrain </summary>
        ///// <MetaDataID>{8767FD05-7B2A-455D-981F-62548F3B064E}</MetaDataID>
        //public readonly bool HasRelationIdentityColumns = true;





        ///// <MetaDataID>{5AD1D519-87AD-483A-9F77-131857F36CB8}</MetaDataID>
        ///// <summary>The property DataNodeMembersParticipateInSelectClause 
        ///// tel as if some one from sub data nodes or sub data node of sub data node etc. 
        ///// participates in select list. </summary>
        //public bool DataNodeMembersParticipateInSelectClause
        //{

        //    set
        //    {
        //    }
        //    get
        //    {
        //        if (ParticipateInSelectClause)
        //            return true;
        //        foreach (DataNode dataNode in SubDataNodes)
        //        {
        //            //TODO Να ελεγχθεί η περίπτωση που έχουμε member object
        //            if (dataNode.Type == DataNodeType.OjectAttribute && dataNode.ParticipateInSelectClause)
        //                return true;
        //            else
        //                if (dataNode.DataNodeMembersParticipateInSelectClause)
        //                    return true;


        //        }

        //        return false;
        //    }
        //}



        //		}
        ///// <MetaDataID>{0B74B36A-20A6-4F64-875B-554290FC82C7}</MetaDataID>
        // public void RetrieveMetadataFromStorage(PersistenceLayer.Storage objectStorage, ref string errorOutput)
        // {
        //     MetaDataRepository.Namespace mNamespace = GetNamespace(objectStorage, Name);
        //     AssignedMetaObject = mNamespace;
        //     Validate(ref errorOutput);

        // }


        ///// <MetaDataID>{7BD46C75-2F12-4D0D-8A8B-C1E3A0413859}</MetaDataID>
        //public void BuildDataNodeTree(PersistenceLayer.Storage objectStorage, ref string errorOutput)
        //{
        //    MergeIdenticalDataNodes();
        //    ΑssignDataNodeToParserPaths(ObjectQuery.PathDataNodeMap);

        //    MetaDataRepository.Namespace mNamespace = GetNamespace(objectStorage, Name);

        //    if (mNamespace == null)
        //    {
        //        errorOutput += "There isn't namespace or class with name " + Name;
        //        return;
        //    }
        //    AssignedMetaObject = mNamespace;
        //    //AssignedMetaObjectIdentity = mNamespace.Identity.ToString();
        //    Validate(ref errorOutput);
        //    MergeIdenticalDataNodes();
        //    if (errorOutput != null && errorOutput.Length > 0)
        //        return;

        //}
        ///// <MetaDataID>{840A2F44-C84D-4329-8B65-399CDD011F1E}</MetaDataID>
        //protected virtual Namespace GetNamespace(PersistenceLayer.Storage objectStorage, string _namespace)
        //{
        //    //TODO θα πρέπει να βρεθεί ένας τρόπος να αντιμετωπίζεται ενιαία τα meta data. 
        //    //Τώρα υπάρχουν δύο περιπτώσεις, η περίπτωση που διαβάζουμε τα meta data από το storage όπου 
        //    //το πρόβλημα είναι ότι αργότερα πρέπει να κάνω access τα fields από τα objects πρέπει DotnetMetadaRepository classes 
        //    //και δεύτερον η περίπτωση που δεν υπάρχουν metadata στην storage και εκεί δεν ξέρω 
        //    //αν το assembly που έχει τα types που θέλει to query έχει φορτωθεί.       
        //    if (objectStorage != null && PersistenceLayer.ObjectStorage.GetStorageOfObject(objectStorage) != null)
        //    {
        //        string Query = "SELECT Namespace FROM " + typeof(MetaDataRepository.Namespace).FullName + " Namespace WHERE  Namespace.Name = \"" + Name + "\"";
        //        Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(objectStorage).Execute(Query);
        //        foreach (Collections.StructureSet Rowset in aStructureSet)
        //        {
        //            MetaDataRepository.Namespace mNamespace = (MetaDataRepository.Namespace)Rowset.Members["Namespace"].Value;
        //            if (mNamespace.GetType() == typeof(MetaDataRepository.Namespace))
        //                return mNamespace;
        //        }
        //        return null;

        //    }
        //    else
        //    {
        //        MetaObjectID metaObjectID = new MetaObjectID(_namespace);
        //        MetaDataRepository.Namespace namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //        System.Collections.ArrayList assemblies = new System.Collections.ArrayList();
        //        if (namespaceMetaData == null)
        //        {
        //            foreach (System.Reflection.Assembly dotNetAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
        //            {

        //                if (dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                {
        //                    DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
        //                    if (assembly == null)
        //                        assembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
        //                    assemblies.Add(assembly);
        //                    long load = assembly.Residents.Count;
        //                    namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                    if (namespaceMetaData != null)
        //                        return namespaceMetaData;
        //                }
        //            }
        //        }
        //        if (namespaceMetaData == null)
        //        {
        //            foreach (DotNetMetaDataRepository.Assembly assembly in assemblies)
        //            {
        //                foreach (Dependency dependency in assembly.ClientDependencies)
        //                {
        //                    DotNetMetaDataRepository.Assembly referAssembly = dependency.Supplier as DotNetMetaDataRepository.Assembly;
        //                    if (referAssembly.WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                    {
        //                        long load = referAssembly.Residents.Count;
        //                        namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                        if (namespaceMetaData != null)
        //                            return namespaceMetaData;
        //                    }
        //                }
        //            }
        //        }
        //        return namespaceMetaData;
        //    }
        //}

        /// <MetaDataID>{da33f2dc-eea7-443b-92de-5e9a1c9fbd60}</MetaDataID>
        public void BuildDataNodeTree(OOAdvantech.MetaDataRepository.Classifier dataNodeClassifier, ref string errorOutput)
        {
            AssignedMetaObject = dataNodeClassifier;
            Validate(ref errorOutput);
        }




        /// <MetaDataID>{d049cdcf-713c-4312-9678-42724b88d0ac}</MetaDataID>
        internal bool CheckForRecursion(MetaDataRepository.AssociationEnd associationEnd)
        {
            System.Collections.Generic.Stack<MetaDataRepository.AssociationEnd> chain = new System.Collections.Generic.Stack<AssociationEnd>();
            chain.Push(associationEnd);
            GetPathChain(this, chain);
            System.Collections.Generic.List<MetaDataRepository.Classifier> classifiers = new System.Collections.Generic.List<Classifier>();
            foreach (AssociationEnd chainAssociationEnd in chain.ToArray())
            {
                if (classifiers.Contains(chainAssociationEnd.Specification))
                    return true;
                classifiers.Add(chainAssociationEnd.Specification);
            }
            return false;
        }
        /// <MetaDataID>{C79835D4-A35A-499F-BC9E-AA266BD65D5C}</MetaDataID>
        internal bool CheckForRecursion(MetaDataRepository.Association association)
        {



            if ((AssignedMetaObject is MetaDataRepository.AssociationEnd) &&
                (association.Identity == (AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Identity))
            {
                return true;
            }
            if (ParentDataNode != null)
                return ParentDataNode.CheckForRecursion(association);
            else
                return false;
        }

        /// <MetaDataID>{f696f33f-f762-4bee-b402-58edb4f8ddc2}</MetaDataID>
        private void GetPathChain(DataNode dataNode, System.Collections.Generic.Stack<AssociationEnd> chain)
        {
            if (dataNode.MembersFetchingObjectActivation && dataNode.AssignedMetaObject is AssociationEnd)
            {
                chain.Push(dataNode.AssignedMetaObject as AssociationEnd);
                if (dataNode.ParentDataNode != null)
                    GetPathChain(dataNode.ParentDataNode, chain);
            }

        }


        #region RowRemove code
        /// <exclude>Excluded</exclude>
        bool _FilterNotActAsLoadConstraint = false;
        ///<summary>
        ///This property informs query engine when search condition act as constraint condition.
        ///When is true search condition act as constraint condition otherwise transformed row remove column.
        ///Has meaning only in case where data node participate in pre fetching mechanism.
        ///</summary>
        /// <MetaDataID>{8548df6f-5b83-4c63-9590-52f846ffafe8}</MetaDataID>
        public bool FilterNotActAsLoadConstraint
        {
            get
            {
                return _FilterNotActAsLoadConstraint;
            }
            internal set
            {
                if (_FilterNotActAsLoadConstraint != value)
                    _SearchCondition = new Member<SearchCondition>();
                _FilterNotActAsLoadConstraint = value;

            }
        }
        #endregion


        ///<summary>
        ///This method defines that subDataNode has member fetching relation with parent 
        ///and the datanode objects must be activated 
        ///</summary>
        /// <MetaDataID>{e98108ac-3755-43ba-a0b4-abf3d7d79fc4}</MetaDataID>
        internal void BackRouteActivationMembersFetching(DataNode subDataNode)
        {
            if (_BackRouteMemberFetchingObjectActivation == null)
                _BackRouteMemberFetchingObjectActivation = new List<Guid>();

            if (!_BackRouteMemberFetchingObjectActivation.Contains(subDataNode.Identity))
                _BackRouteMemberFetchingObjectActivation.Add(subDataNode.Identity);
        }

        /// <exclude>Excluded</exclude>
        static System.Collections.Generic.List<Guid> EmptyBackRouteList;
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<Guid> _BackRouteMemberFetchingObjectActivation = null;
        /// <summary>
        ///  Defines a collection with the subdatanodes identities which they have member fetching relation with parent 
        /// and the datanode objects must be activated 
        ///  </summary>
        /// <MetaDataID>{695ab0d3-1762-4369-9240-2fba5e064967}</MetaDataID>
        public System.Collections.Generic.IList<Guid> BackRouteMemberFetchingObjectActivation
        {
            get
            {
                if (_BackRouteMemberFetchingObjectActivation == null)
                {
                    if (EmptyBackRouteList == null)
                        EmptyBackRouteList = new List<Guid>();
                    return new System.Collections.ObjectModel.ReadOnlyCollection<Guid>(EmptyBackRouteList);
                }
                else
                    return new System.Collections.ObjectModel.ReadOnlyCollection<Guid>(_BackRouteMemberFetchingObjectActivation);


            }
        }

        /// <exclude>Excluded</exclude>
        bool _MembersFetchingObjectActivation = false;
        /// <summary>
        /// This property defines that DataNode must ensure the  loading and activation of objects, for related DataNode objects members fetching mechanism
        /// </summary>
        /// <remarks>
        /// The query engine is responsible to update all MembersFetchingObjectActivation properties with new value 
        /// for all distributed query through NotifyMasterQueryMetaDataChange method 
        /// </remarks>
        /// <MetaDataID>{E9B4E7F6-B2FA-4BA0-8FD4-B6D4229F60F0}</MetaDataID>
        public bool MembersFetchingObjectActivation
        {
            get
            {
                return _MembersFetchingObjectActivation;
            }
            set
            {
                _MembersFetchingObjectActivation = value;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _AutoGenaratedForMembersFetching = false;
        /// <summary>
        /// This property defines that DataNode Genarated for member fetching
        /// </summary>
        /// <MetaDataID>{E9B4E7F6-B2FA-4BA0-8FD4-B6D4229F60F0}</MetaDataID>
        public bool AutoGenaratedForMembersFetching
        {
            get
            {
                return _AutoGenaratedForMembersFetching;
            }
            set
            {
                _AutoGenaratedForMembersFetching = value;
            }
        }



        /// <MetaDataID>{5cfed660-8206-4c41-bd07-136ca33c1b83}</MetaDataID>
        /// <summary>
        /// If ParentLoadsObjectsLinks is true query engine load relation object links on parent datanode objects activation.
        /// </summary>
        /// Has deferent meaning from MembersFetchingObjectActivation property because the MembersFetchingObjectActivation 
        /// defines that the datanode forced from query engine to activate datanode object for prefetching reasons. 
        /// The query engine load relation object links for the data nodes with MembersFetchingObjectActivation true and data nodes 
        /// which originally participate in selection list(activate objects) and has not searching filters.  
        internal bool ParentLoadsObjectsLinks
        {
            get
            {
                if (MembersFetchingObjectActivation)
                    return true;
                if (ParticipateInSelectClause && !BranchParticipateInWereClause && ValueTypePath.Count == 0)
                    return true;
                return false;
            }

        }


        /// <MetaDataID>{8F908939-A77D-412B-ABF6-861F41BA0E9E}</MetaDataID>
        /// <summary>Look data tree backward for auto generated data node for the association end of parameter. 
        /// This method is useful when we want to avoid recursive generation of data node for the association end of parameter. </summary>
        internal protected bool IsThereAutoGenDataNodeInHierarchy(AssociationEnd associationEnd)
        {
            if (MembersFetchingObjectActivation && AssignedMetaObjectIdenty == associationEnd.Identity)
                return true;
            else
            {
                if (ParentDataNode == null)
                    return false;
                else
                    return ParentDataNode.IsThereAutoGenDataNodeInHierarchy(associationEnd);

            }
        }


        /// <MetaDataID>{15669C6C-CF87-48E5-8611-F3697A821841}</MetaDataID>
        /// <summary>Define time period constrain flag. In case where the system produce data massively it is useful the data partitioning. With this technique you can search data rapidly when you refer to a time period. </summary>
        public bool HasTimePeriodConstrain = false;

        /// <MetaDataID>{D7BB9696-86CF-4443-B7C2-2A2AF9AF3A32}</MetaDataID>
        /// <summary>
        /// This member indicates if the data node has storage cell constrain. 
        /// When the data node has storage cell constrain the data node retrieve data only from this storage cell. 
        /// </summary>
        public bool HasStorageCellConstrain = false;


        ///// <MetaDataID>{DF0C0DA4-5E6C-45CF-969B-0E20337974ED}</MetaDataID>
        //[NonSerialized]
        //public OQLStatement ObjectQuery;

        /// <exclude>Excluded</exclude>
        bool _Recursive = false;

        /// <MetaDataID>{3a340c5f-f0d1-4613-9416-0d68d73e61ef}</MetaDataID>
        public bool Recursive
        {
            get
            {
                return _Recursive;
            }
            internal set
            {
                _Recursive = value;
            }
        }
        /// <exclude>Excluded</exclude>
        int _RecursiveSteps = 0;
        /// <MetaDataID>{7608de91-3655-4f52-9b48-99a8ed42c2b3}</MetaDataID>
        public int RecursiveSteps
        {
            get
            {
                return _RecursiveSteps;
            }
            internal set
            {
                _RecursiveSteps = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _RecursiveMemberName;
        /// <MetaDataID>{8c50e710-fd12-4bd4-bd78-6eb120cab216}</MetaDataID>
        public string RecursiveMemberName
        {
            get
            {
                return _RecursiveMemberName;
            }
            internal set
            {
                _RecursiveMemberName = value;
            }
        }




        /// <MetaDataID>{99896AE7-FD43-4D8E-813D-6C7C6B26DDBB}</MetaDataID>
        public DataNode(ObjectQuery objectQuery)
        {
            ObjectQuery = objectQuery;
        }
        /// <MetaDataID>{55a3f1ee-0ca3-4a2b-bb44-aac119d57757}</MetaDataID>
        public DataNode(ObjectQuery objectQuery, string name, MetaDataRepository.MetaObject assignedMetaObject)
        {
            _Name = name;
            ObjectQuery = objectQuery;
            _AssignedMetaObject = assignedMetaObject;
            _AssignedMetaObjectIdenty = assignedMetaObject.Identity;
        }

        /// <MetaDataID>{646F5541-0460-4AA0-A8F5-ACCA9BAC716D}</MetaDataID>
        internal DataNode(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery objectQuery, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path path)
        {

            ObjectQuery = objectQuery;
            Path = path;
            RecursiveSteps = Path.RecursiveSteps;
            Recursive = Path.Recursive;
            Name = path.Name;
            RelatedPaths.Add(path);

            if (path is PathHead)
            {
                if ((path as PathHead).HasTimePeriodConstrain)
                {
                    TimePeriodStartDate = (path as PathHead).TimePeriodStartDate;
                    TimePeriodEndDate = (path as PathHead).TimePeriodEndDate;
                }
            }

        }





        /// <MetaDataID>{13E079C4-0ACD-4DCE-AD04-8A591EA756FD}</MetaDataID>
        /// <summary>Define the full name of data node which is the full name of parent data node plus dot plus the name of this data node. </summary>
        public string FullName
        {
            get
            {
                if (ParentDataNode != null && ParentDataNode.Type != DataNodeType.Group)
                    return ParentDataNode.FullName + "." + Name;
                return Name;
            }
        }


        [Serializable]
        public enum DataNodeType
        {
            /// <summary>The OQL query retrieves data</summary>
            Unknown,
            Namespace,
            Object,
            OjectAttribute,
            Count,
            Sum,
            Average,
            Min,
            Max,
            Group,
            Key,
            DerivedDataNode

        };




        /// <MetaDataID>{cd367e28-ed4c-4fc5-b282-d455072e746c}</MetaDataID>
        public OrderByType OrderBy;


        /// <MetaDataID>{6396CD7E-A4FA-4FF2-8CA1-B77E29FCB84D}</MetaDataID>
        /// <summary>This method assigns the data node to the parser nodes. 
        /// This is useful because the system can find the corresponding data node from a parser node. 
        /// One data node can correspond to one or more parser node. </summary>
        internal void AssignDataNodeToParserPaths(System.Collections.Generic.Dictionary<object, DataNode> ParserNodeObjectCollectionMap)
        {

            /// <seealso cref="RelatedPaths"/>
            if (RelatedPaths != null) //serialization
            {
                foreach (Path path in RelatedPaths)
                {
                    if (!ParserNodeObjectCollectionMap.ContainsKey(path.ParserNode))
                        ParserNodeObjectCollectionMap.Add(path.ParserNode, this);
                    else
                        ParserNodeObjectCollectionMap[path.ParserNode] = this;
                }
            }
            foreach (DataNode CurrObjectCollection in SubDataNodes)
                CurrObjectCollection.AssignDataNodeToParserPaths(ParserNodeObjectCollectionMap);
        }


        /// <MetaDataID>{27AAEAFB-3CDF-46CE-AF30-7A2597089418}</MetaDataID>
        /// <summary>The work of this method is to find the corresponding Meta object for the sub data node. 
        /// If data node is namespace then sub node is class or namespace. 
        /// If data node is class then the sub data node is attribute or association end or nested class. 
        /// If data node is association end then the sub data node is attribute or association end of data node association end specification. </summary>
        MetaDataRepository.MetaObject GetMataObjectForDataNode(DataNode dataNode)
        {

            if (AssignedMetaObject is MetaDataRepository.Namespace && !(AssignedMetaObject is MetaDataRepository.Classifier))
            {
                MetaDataRepository.Namespace _namespace = (MetaDataRepository.Namespace)AssignedMetaObject;
                foreach (MetaDataRepository.MetaObject metaObject in _namespace.OwnedElements)
                {
                    if (metaObject.FullName == dataNode.FullName && (metaObject is MetaDataRepository.Classifier || metaObject is MetaDataRepository.Namespace))
                        return metaObject;
                }
                try
                {
                    System.Type type = ObjectQuery.GetType(dataNode.FullName, "");
                    //TODO να γίνει τεστ στην περίπτωση που έχω δύο namespaces στην σειρά
                    //System.Type type = ModulePublisher.ClassRepository.GetType(dataNode.FullName, "");

                    if (type != null)
                    {
                        OOAdvantech.DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.Assembly.GetComponent(type.GetMetaData().Assembly) as DotNetMetaDataRepository.Assembly;


                        long count = assembly.Residents.Count;

                    }
                    foreach (MetaDataRepository.MetaObject metaObject in _namespace.OwnedElements)
                    {
                        if (metaObject.FullName == dataNode.FullName && (metaObject is MetaDataRepository.Classifier || metaObject is MetaDataRepository.Namespace))
                            return metaObject;
                    }
                }
                catch (System.Exception error)
                {
                }
                if (Classifier == null)
                    return null;
            }

            MetaDataRepository.Classifier classifier = Classifier as MetaDataRepository.Classifier;
            if (classifier != null)
            {

                #region MetaObject is Assocation class roleA or roleB
                if (dataNode.AssignedMetaObjectIdenty != null && classifier.LinkAssociation != null &&
                    (dataNode.AssignedMetaObjectIdenty == classifier.LinkAssociation.RoleA.Identity ||
                    dataNode.AssignedMetaObjectIdenty == classifier.LinkAssociation.RoleB.Identity))
                {
                    if (dataNode.AssignedMetaObjectIdenty == classifier.LinkAssociation.RoleA.Identity)
                        return classifier.LinkAssociation.RoleA;
                    if (dataNode.AssignedMetaObjectIdenty == classifier.LinkAssociation.RoleB.Identity)
                        return classifier.LinkAssociation.RoleB;
                }
                #endregion

                if (dataNode.CastingParentType != null)
                {
                    MetaDataRepository.Classifier castingClassifier = MetaDataRepository.Classifier.GetClassifier(dataNode.CastingParentType);
                    if (castingClassifier.IsA(classifier) || castingClassifier == classifier)
                        classifier = castingClassifier;

                    if (classifier.IsA(castingClassifier) || castingClassifier == classifier)
                        classifier = castingClassifier;
                }

                foreach (MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                {
                    if (dataNode.AssignedMetaObjectIdenty == null)
                    {
                        if (classifier.LinkAssociation != null)
                        {
                            object Value = attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole");
                            bool IsAssociationClassRole = false;
                            if (Value != null)
                                IsAssociationClassRole = (bool)Value;
                            if (IsAssociationClassRole && attribute.Name == dataNode.Name)
                            {
                                bool IsRoleA = (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA");
                                if (IsRoleA)
                                {
                                    //TODO η πιο κάτω γραμμη κώδικα βγήκει χωρίς να τεσταριστή πός δουλεύει σε query σε storge xml και sql 
                                    return classifier.LinkAssociation.RoleA;
                                }
                                else
                                {
                                    //TODO η πιο κάτω γραμμη κώδικα βγήκει χωρίς να τεσταριστή πός δουλεύει σε query σε storge xml και sql 
                                    return classifier.LinkAssociation.RoleB;
                                }
                            }
                        }
                        if (attribute.Name == dataNode.Name)
                            return attribute;
                    }
                    else
                    {

                        //TODO Να γραφτεί test case για αυτήν την περίπτωση
                        //ΤΟΔΟ Να βρεθεί εναλακτικό τρόπος του attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole")
                        if (attribute.Identity == dataNode.AssignedMetaObjectIdenty)
                            return attribute;
                    }
                }
            }

            foreach (MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            {
                if (dataNode.AssignedMetaObjectIdenty == null &&
                    (associationEnd.Name == dataNode.Name && associationEnd.Navigable))
                {
                    return associationEnd;
                }
                if (dataNode.AssignedMetaObjectIdenty == null &&
                    associationEnd.Association.LinkClass != null)
                {
                    if (associationEnd.Association.Name == dataNode.Name)
                        return associationEnd;//.GetOtherEnd();
                }

                if (dataNode.AssignedMetaObjectIdenty != null &&
                    !string.IsNullOrEmpty(dataNode.AssignedMetaObjectIdenty.ToString()) &&
                    associationEnd.Identity == dataNode.AssignedMetaObjectIdenty)
                {
                    return associationEnd;
                }

            }
            if (Classifier.LinkAssociation != null)
            {

                if (Classifier.LinkAssociation.RoleA.Name == dataNode.Name)
                    return Classifier.LinkAssociation.RoleA;
                if (Classifier.LinkAssociation.RoleB.Name == dataNode.Name)
                    return Classifier.LinkAssociation.RoleB;
            }

            if (dataNode.Classifier != null)
            {
                foreach (MetaDataRepository.AssociationEnd associationEnd in dataNode._Classifier.GetAssociateRoles(true))
                {
                    if (associationEnd.GetOtherEnd().Identity == dataNode.AssignedMetaObjectIdenty)
                        return associationEnd;
                }
            }

            //if (dataNode.OnConstuctionFetching)
            //{
            //    foreach (MetaDataRepository.AssociationEnd associationEnd in DataSource.PrefetchingAssociationEnds)
            //    {

            //        if (dataNode.AssignedMetaObjectIdenty != null)
            //        {
            //            if (dataNode.AssignedMetaObjectIdenty == associationEnd.Identity)
            //                return associationEnd;
            //        }
            //        else
            //        {

            //            if (associationEnd.Name == dataNode.Name && associationEnd.Navigable)
            //                return associationEnd;

            //            if (associationEnd.Association.LinkClass != null)
            //            {
            //                if (associationEnd.Association.Name == dataNode.Name)
            //                    return associationEnd;//.GetOtherEnd();
            //            }
            //        }
            //    }
            //}
            return null;
        }



        /// <MetaDataID>{8718e73b-9cb4-471e-87cd-f86e776767d8}</MetaDataID>
        public System.Collections.Generic.List<DataNode> OrderByDataNodes = new System.Collections.Generic.List<DataNode>();



        /// <MetaDataID>{F594ED27-10E6-42F6-8B14-600F0095AC80}</MetaDataID>
        /// <summary>Check the data node and sub data nodes against the Meta data of object storage. 
        /// If it is valid assign the Meta object to data node. </summary>
        public bool Validate(ref string ErrorOutput)
        {

            if ((Type == DataNodeType.Object) &&
                (!(ObjectQuery is DistributedObjectQuery) || DataSource.HasInObjectContextData))
            {
                if (!string.IsNullOrEmpty(ClassifierFullName) && _Classifier == null)
                {
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(ClassifierFullName, ClassifierImplementationUnitName));
                    if (Type == DataNodeType.Object && ParentDataNode == null)
                        _AssignedMetaObject = _Classifier;
                    if ((ParentDataNode != null && ParentDataNode.Type == DataNodeType.Group && ParentDataNode.ParentDataNode == null))
                        _AssignedMetaObject = _Classifier;
                }
            }

            #region DataNode with root DataSource
            if (ObjectQuery is DistributedObjectQuery && Type == DataNodeType.Object &&
                (RealParentDataNode == null || RealParentDataNode.Type == DataNodeType.Namespace) &&
                !string.IsNullOrEmpty(ClassifierImplementationUnitName) &&
                !string.IsNullOrEmpty(ClassifierFullName))
            {
                Type type = ModulePublisher.ClassRepository.GetType(ClassifierFullName, ClassifierImplementationUnitName);
                if (type != null)
                {
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(type);
                    _AssignedMetaObject = _Classifier;
                }
                else
                {

                }
            }
            #endregion

            //TODO τι γίνεται στην περίπτωση που client δουλεύει με άλλη version assembly

            bool Valid = true;
            foreach (DataNode dataNode in new System.Collections.Generic.List<DataNode>(RealSubDataNodes))
            {

                if (dataNode is AggregateExpressionDataNode || dataNode.Type == DataNodeType.Key)
                    continue;
                MetaDataRepository.MetaObject metaObject = null;
                if (Type == DataNodeType.Group)
                {
                    if (RealParentDataNode != null)
                        metaObject = RealParentDataNode.GetMataObjectForDataNode(dataNode);
                    else
                    {
                        if (!dataNode.Validate(ref ErrorOutput))
                            Valid = false;
                        continue;
                    }
                }
                else
                {
                    if (dataNode.AssignedMetaObject == null)
                    {
                        if (dataNode.ObjectQuery is DistributedObjectQuery && dataNode.Type == DataNodeType.Object)
                        {

                            if ((!(DataSource as DataSource).HasInObjectContextData && MembersFetchingObjectActivation) &&
                                dataNode.AssignedMetaObjectIdenty != null &&
                                dataNode.DataSource is DataSource &&
                                (!(dataNode.DataSource as DataSource).HasInObjectContextData && dataNode.MembersFetchingObjectActivation))
                            {
                                if (!dataNode.Validate(ref ErrorOutput))
                                    Valid = false;
                                //else
                                //    dataNode.AssignedMetaObject = GetMataObjectForDataNode(dataNode);
                                continue;
                            }
                            else if (AssignedMetaObject == null &&
                                AssignedMetaObjectIdenty != null &&
                                Classifier == null &&
                                (!(DataSource as StorageDataSource).HasInObjectContextData && MembersFetchingObjectActivation))
                            {
                                dataNode._Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(dataNode.ClassifierFullName, dataNode.ClassifierImplementationUnitName));
                                foreach (MetaDataRepository.AssociationEnd associationEnd in dataNode._Classifier.GetAssociateRoles(true))
                                {
                                    if (associationEnd.GetOtherEnd().Identity == dataNode.AssignedMetaObjectIdenty)
                                    {
                                        if (!associationEnd.Navigable)
                                        {

                                        }
                                        dataNode.AssignedMetaObject = associationEnd.GetOtherEnd();
                                        break;
                                    }
                                }
                                if (!dataNode.Validate(ref ErrorOutput))
                                    Valid = false;
                                continue;
                            }
                            else if (dataNode.AssignedMetaObject == null)
                                dataNode.AssignedMetaObject = GetMataObjectForDataNode(dataNode);
                        }
                        else
                            dataNode.AssignedMetaObject = GetMataObjectForDataNode(dataNode);
                    }


                }

                if (dataNode.AssignedMetaObject == null && dataNode.Type != DataNodeType.Group)
                {
                    if (Alias != null)
                    {
                        if (Classifier != null)
                            ErrorOutput += "\r\n'" + dataNode.Name + "' isn't member of '" + (Classifier as MetaDataRepository.Classifier).FullName + "'";
                        else
                        {
                            if (Path != null)
                                ErrorOutput += "\r\n'" + dataNode.Name + "' isn't member of '" + Path.Name + "'";
                            else
                                ErrorOutput += "\nError on '" + Alias + "'";
                        }
                    }
                    else
                    {
                        if (Classifier != null)
                            ErrorOutput += "\r\n'" + dataNode.Name + "' isn't member of '" + (Classifier as MetaDataRepository.Classifier).FullName + "'";
                        else
                            ErrorOutput += "\r\n'" + dataNode.Name + "' isn't  member of '" + AssignedMetaObject.FullName + "'";
                    }
                    Valid = false;
                }
                else
                {
                    //dataNode.AssignedMetaObject = metaObject;
                    if (!dataNode.Validate(ref ErrorOutput))
                        Valid = false;
                }
            }
            if (HasTimePeriodConstrain)
                HeaderDataNode.DataNodeWithRootDataSource = this;

            //foreach (DataNode dataNode in new System.Collections.Generic.List<DataNode>(SubDataNodes))
            //{
            //    if (dataNode.Type == DataNodeType.Count)
            //    {
            //        DataNode aggregateExpressionDataNode = null;
            //        if ((dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count > 0)
            //            aggregateExpressionDataNode = (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes[0];
            //        if (aggregateExpressionDataNode != null && !aggregateExpressionDataNode.IsSameOrParentDataNode(this))
            //        {
            //            aggregateExpressionDataNode.ParentDataNode = this;
            //            foreach (DataNode subDataNode in SubDataNodes)
            //            {
            //                if (subDataNode != aggregateExpressionDataNode && subDataNode.FullName == aggregateExpressionDataNode.FullName)
            //                {
            //                    aggregateExpressionDataNode.ParentDataNode = null;
            //                    (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Remove(aggregateExpressionDataNode);
            //                    if (!(dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Contains(subDataNode))
            //                        (dataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(subDataNode);
            //                    aggregateExpressionDataNode = subDataNode;
            //                    break;
            //                }
            //            }
            //            if (Type != DataNodeType.Group && aggregateExpressionDataNode.AssignedMetaObject == null)
            //            {
            //                MetaDataRepository.MetaObject metaObject = GetMataObjectForDataNode(aggregateExpressionDataNode);
            //                aggregateExpressionDataNode.AssignedMetaObject = metaObject;
            //            }
            //            if (!aggregateExpressionDataNode.Validate(ref ErrorOutput))
            //                Valid = false;
            //        }
            //    }


            //}

            return Valid;
        }




        /// <summary>In OQL statement there is data paths in three clauses 
        /// (SELECT, FROM, WHERE) with the root in FROM clause.
        /// At the time of collection of data paths from the parse tree 
        /// can be produce duplicated sub data nodes. 
        /// This method merges the duplicated sub data nodes. </summary>
        /// <MetaDataID>{7E7E8819-04EB-4133-B968-91EFC5001F9E}</MetaDataID>
        public virtual void MergeIdenticalDataNodes()
        {



            System.Collections.Generic.List<DataNode> subDataNodesCash = new System.Collections.Generic.List<DataNode>(SubDataNodes);

            #region Merge SubDataNodes with same name
            foreach (DataNode subDataNode in subDataNodesCash)
            {
                if (SubDataNodes.Contains(subDataNode))
                {
                    foreach (DataNode candidateForMergeSubDataNode in subDataNodesCash)
                        MergeIfIdentical(subDataNode, candidateForMergeSubDataNode);
                }
            }
            #endregion

            #region Merge SubDataNodes with same link class association

            //for example if there is Query "SELECT employee.Job.StartingDate, employee.Employers.Name FROM Employee employee
            //the DataNode tree is like this
            //Employee 
            //	|
            //	|_________Job
            //	|			|
            //	|			|______StartingDate
            //	|
            //	|_________Employers
            //	|			|
            //	|			|______Name 
            //
            //After merging the DataNode tree will be like this
            //			
            //Employee 
            //	|
            //	|_________Job
            //	|			|
            //	|			|______StartingDate
            //	|			|
            //	|			|______Employers
            //	|					|
            //	|					|______Name 

            if (AssignedMetaObject != null)
            {
                foreach (DataNode dataNode in subDataNodesCash)
                {
                    if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        MetaDataRepository.AssociationEnd associationEnd = dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        if (associationEnd.Association.LinkClass != null && dataNode.Name != associationEnd.Association.Name)
                        {
                            foreach (DataNode canditateForMergeDataNode in SubDataNodes)
                            {
                                if (dataNode == canditateForMergeDataNode)
                                    continue;

                                if (canditateForMergeDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                                {
                                    MetaDataRepository.AssociationEnd canditateForMergedataAssociationEnd = canditateForMergeDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                    if (canditateForMergedataAssociationEnd.Association.LinkClass != null && canditateForMergeDataNode.Name == canditateForMergedataAssociationEnd.Association.Name)
                                    {
                                        bool Merged = false;
                                        foreach (DataNode subNode in canditateForMergeDataNode.SubDataNodes)
                                        {
                                            if (subNode.AssignedMetaObject == dataNode.AssignedMetaObject)
                                            {
                                                MergeIfIdentical(subNode, dataNode);
                                                Merged = true;
                                                break;
                                            }
                                        }
                                        if (!Merged)
                                        {
                                            SubDataNodes.Remove(dataNode);
                                            dataNode.ParentDataNode = canditateForMergeDataNode;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            foreach (DataNode CurrObjectCollection in SubDataNodes)
                CurrObjectCollection.MergeIdenticalDataNodes();

        }
        /// <MetaDataID>{183753B2-8578-49B1-9C30-F9F54FF377C2}</MetaDataID>
        /// <summary>Check the data nodes if they are identical. If they are identical merge them in one. </summary>
        protected virtual bool MergeIfIdentical(DataNode MergeInDataNode, DataNode MergedDataNode)
        {
            if (MergeInDataNode == MergedDataNode)
                return false;
            if (MergeInDataNode.Name != MergedDataNode.Name)
                return false;



            if (MergeInDataNode.Alias != null && MergedDataNode.Alias != null && MergedDataNode.Alias != MergeInDataNode.Alias)
            {
                if (MergeInDataNode.Type != DataNodeType.Object || MergedDataNode.Type != DataNodeType.Object ||
                  (!MergeInDataNode.MembersFetchingObjectActivation && !MergedDataNode.MembersFetchingObjectActivation))
                {
                    return false;
                }
                if (MergeInDataNode.MembersFetchingObjectActivation)
                {
                    DataNode tempDataNode = MergeInDataNode;
                    MergeInDataNode = MergedDataNode;
                    MergedDataNode = tempDataNode;
                }
                MergeInDataNode.MembersFetchingObjectActivation = true;

            }
            if (MergeInDataNode.Type != MergedDataNode.Type)
                return false;
            if (MergeInDataNode is AggregateExpressionDataNode && MergeInDataNode is AggregateExpressionDataNode)
            {
                if (!(MergeInDataNode as AggregateExpressionDataNode).IsEquivalent(MergeInDataNode as AggregateExpressionDataNode))
                    return false;
            }

            SubDataNodes.Remove(MergedDataNode);
            System.Collections.Generic.List<DataNode> TempSubDataNodes = new System.Collections.Generic.List<DataNode>(MergedDataNode.SubDataNodes);
            foreach (DataNode CurrDataNode in TempSubDataNodes)
                CurrDataNode.ParentDataNode = MergeInDataNode;

            foreach (Path path in MergedDataNode.RelatedPaths)
                MergeInDataNode.RelatedPaths.Add(path);
            if (MergedDataNode.ParticipateInWereClause)
                MergeInDataNode.ParticipateInWereClause = true;

            MergeInDataNode.OrderBy = MergedDataNode.OrderBy;
            foreach (string alias in MergedDataNode.Aliases)
                MergeInDataNode.Alias = alias;

            //if (MergedDataNode.Countable)
            //{
            //    MergeInDataNode.Countable = true;
            //    MergeInDataNode.CountablePathFullName = MergeInDataNode.CountablePathFullName;
            //}



            if (ObjectQuery != null && ObjectQuery.SelectListItems.Contains(MergedDataNode))
            {
                ObjectQuery.RemoveSelectListItem(MergedDataNode);
                if (!ObjectQuery.SelectListItems.Contains(MergeInDataNode))
                {
                    ObjectQuery.AddSelectListItem(MergeInDataNode);
                    MergeInDataNode.ParticipateInSelectClause = true;
                }
            }
            return true;
        }







        /// <MetaDataID>{2F638901-5602-4796-B485-40F7789C151B}</MetaDataID>
        internal bool ExistSubNode(DataNode subDataNode)
        {
            foreach (DataNode dataNode in SubDataNodes)
            {
                if (dataNode == subDataNode)
                    return true;
            }
            foreach (DataNode dataNode in SubDataNodes)
            {
                if (dataNode.ExistSubNode(subDataNode))
                    return true;
            }
            return false;
        }

        /// <MetaDataID>{019B0F3F-015B-4FF0-9D00-25B010DC5D43}</MetaDataID>
        internal void GetDataNodePath(System.Collections.Generic.LinkedList<DataNode> dataNodePath, DataNode subDataNode)
        {
            foreach (DataNode dataNode in SubDataNodes)
            {
                if (dataNode == subDataNode)
                {
                    dataNodePath.AddLast(subDataNode);
                    return;
                }
            }
            foreach (DataNode dataNode in SubDataNodes)
            {
                if (dataNode.ExistSubNode(subDataNode))
                {
                    dataNodePath.AddLast(dataNode);
                    dataNode.GetDataNodePath(dataNodePath, subDataNode);
                    return;
                }
            }

        }
        /// <MetaDataID>{e22fc08e-d063-4aad-99d3-07cbc4285c0b}</MetaDataID>
        protected bool InDataSources = false;

        ///<summary>
        ///Traverse data tree and collect data source and load them in dataSources dictionary
        ///</summary>
        ///<param name="dataSources">
        ///Defines a DataSource Dictionary with key the DataSource identity
        ///</param>
        /// <MetaDataID>{4156eeef-c980-4ae8-bf24-d675134237a8}</MetaDataID>
        internal virtual void GetDataSources(ref System.Collections.Generic.Dictionary<Guid, DataSource> dataSources)
        {

            if (!InDataSources)
            {
                InDataSources = true;
                try
                {
                    if (dataSources == null)
                        dataSources = new System.Collections.Generic.Dictionary<Guid, DataSource>();

                    if (_DataSource != null)
                        dataSources[_DataSource.Identity] = _DataSource;

                    foreach (DataNode subDataNode in SubDataNodes)
                        subDataNode.GetDataSources(ref dataSources);

                }
                finally
                {
                    InDataSources = false;
                }
            }
        }


        ///<summary>
        ///Check parameter dataNode if it is one of DataNode ancestor
        ///</summary>
        ///<param name="dataNode">
        ///Defines the checked DataNode 
        ///</param>
        ///<returns>
        ///Return true when parameter dataNode is one of Datanode ancestors else return false
        ///</returns>
        /// <MetaDataID>{af44bf5f-dfc0-4761-89af-b005f8d76abc}</MetaDataID>
        internal bool IsPathNode(DataNode dataNode)
        {
            DataNode ancestor = ParentDataNode;
            while (ancestor != null && ancestor != dataNode)
                ancestor = ancestor.ParentDataNode;
            return ancestor == dataNode;
        }

        ///<summary>
        ///Define all criterions where datanode participate
        ///</summary>
        /// <MetaDataID>{088f525c-8257-4805-8b56-65230ff74e02}</MetaDataID>
        public System.Collections.Generic.List<Criterion> SearchCriterions = new System.Collections.Generic.List<Criterion>();


        ///<summary>
        ///Defines the criterions where participate the data node and data nodes which are under this on DataNode tree
        ///</summary>
        /// <MetaDataID>{2d6a9fae-72fc-4f9e-982f-7f04010ecf4d}</MetaDataID>
        public System.Collections.Generic.List<Criterion> BranchSearchCriterions
        {
            get
            {
                System.Collections.Generic.List<Criterion> searchCriteria = new System.Collections.Generic.List<Criterion>(this.SearchCriterions);
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    foreach (Criterion criterion in subDataNode.BranchSearchCriterions)
                        searchCriteria.Add(criterion);
                }
                return searchCriteria;
            }
        }

        /// <MetaDataID>{2b836229-a455-45f4-995e-ba0855c1216c}</MetaDataID>
        public System.Collections.Generic.Stack<DataNode> BuildRoute(DataNode routeEndDataNode)
        {
            System.Collections.Generic.Stack<DataNode> route = new Stack<DataNode>();
            System.Collections.Generic.List<DataNode> passThroughDataNodes = new List<DataNode>();
            BuildRoute(routeEndDataNode, passThroughDataNodes, route);
            return route;
        }
        /// <MetaDataID>{9c96e291-d08d-433f-a4de-27ccccec3ba6}</MetaDataID>
        public bool BuildRoute(DataNode routeEndDataNode, System.Collections.Generic.Stack<DataNode> route)
        {
            System.Collections.Generic.List<DataNode> passThroughDataNodes = new List<DataNode>();
            return BuildRoute(routeEndDataNode, passThroughDataNodes, route);
        }

        /// <MetaDataID>{a2bfebea-376d-44cd-9af9-9ab7bbb64028}</MetaDataID>
        /// <summary>
        /// This method participates in route building mechanism.
        /// Building route mechanism walk on data tree through BuildRoute until find the routeEndDataNode 
        /// then unwind function call and build the DataNode route.
        /// </summary>
        /// <param name="routeEndDataNode">
        /// Defines the last DataNode of built route 
        /// </param>
        /// <param name="passThroughDataNodes">
        /// Defines a collection that keeps all Data Nodes where mechanism passes from them to find routeEndDataNode.
        /// </param>
        /// <param name="route">
        /// Defines the built route
        /// </param>
        internal virtual bool BuildRoute(DataNode routeEndDataNode, System.Collections.Generic.List<DataNode> passThroughDataNodes, System.Collections.Generic.Stack<DataNode> route)
        {
            if (routeEndDataNode == this)
            {
                route.Push(routeEndDataNode);
                return true;
            }
            else
            {
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (passThroughDataNodes.Contains(subDataNode))
                        continue;
                    passThroughDataNodes.Add(this);
                    if (subDataNode.BuildRoute(routeEndDataNode, passThroughDataNodes, route))
                    {
                        route.Push(this);
                        return true;
                    }
                }

            }

            if (ParentDataNode != null)
            {
                if (ParentDataNode == routeEndDataNode)
                {
                    route.Push(ParentDataNode);
                    route.Push(this);
                    return true;
                }
                passThroughDataNodes.Add(this);
                if (passThroughDataNodes.Contains(ParentDataNode))
                    return false;
                if (ParentDataNode.BuildRoute(routeEndDataNode, passThroughDataNodes, route))
                {
                    route.Push(this);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }





        /// <MetaDataID>{4514fcf6-4daa-4f44-aad4-d1b518aa87f4}</MetaDataID>
        internal bool BuildRoute(DataNode routeEndDataNode, Stack<DataNode> extraRoute, System.Collections.Generic.List<DataNode> passThroughDataNodes, System.Collections.Generic.Stack<DataNode> route)
        {
            if (routeEndDataNode == this)
            {
                route.Push(routeEndDataNode);
                return true;
            }
            else
            {
                if (!DataNodeOutOfExtraRoute(passThroughDataNodes, extraRoute))
                {
                    foreach (DataNode subDataNode in SubDataNodes)
                    {
                        if (passThroughDataNodes.Contains(subDataNode) || (extraRoute.Count > 0 && !extraRoute.Contains(subDataNode)))
                            continue;
                        passThroughDataNodes.Add(this);
                        if (subDataNode.BuildRoute(routeEndDataNode, extraRoute, passThroughDataNodes, route))
                        {
                            route.Push(this);
                            return true;
                        }
                    }
                    if (Type == DataNodeType.Key && extraRoute.Contains(this))
                    {

                        foreach (DataNode subDataNode in (ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                        {
                            if (passThroughDataNodes.Contains(subDataNode) || (extraRoute.Count > 0 && !extraRoute.Contains(subDataNode)))
                                continue;
                            passThroughDataNodes.Add(this);
                            if (subDataNode.BuildRoute(routeEndDataNode, extraRoute, passThroughDataNodes, route))
                            {
                                route.Push(this);
                                return true;
                            }

                        }
                    }
                }

            }

            if (ParentDataNode != null)
            {
                if (ParentDataNode == routeEndDataNode)
                {
                    route.Push(ParentDataNode);
                    route.Push(this);
                    return true;
                }
                passThroughDataNodes.Add(this);
                if (passThroughDataNodes.Contains(ParentDataNode))
                    return false;
                if (ParentDataNode.BuildRoute(routeEndDataNode, extraRoute, passThroughDataNodes, route))
                {
                    route.Push(this);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <MetaDataID>{e58082cd-69a5-462d-b65b-6c256b29f95d}</MetaDataID>
        private bool DataNodeOutOfExtraRoute(List<DataNode> passThroughDataNodes, Stack<DataNode> extraRoute)
        {
            foreach (var keyDataNode in extraRoute.ToArray())
            {
                if (keyDataNode.Type == DataNodeType.Key && !passThroughDataNodes.Contains(keyDataNode) && !keyDataNode.IsSameOrParentDataNode(this))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Retrievs all storage where query pump objects
        /// </summary>
        /// <param name="objectQueryStorages">
        /// Defines dictionary collection with object context matada distribution manager
        /// </param>
        /// <MetaDataID>{dbe85160-9dae-4862-b1fb-25d194c04cfc}</MetaDataID>
        internal virtual void GetObjectsContexts(Collections.Generic.Dictionary<string, ObjectsContextMetadataDistributionManager> queryObjectsContexts)
        {
            if (DataSource is StorageDataSource)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> entry in (DataSource as StorageDataSource).DataLoadersMetadata)
                {

                    if (entry.Value.MemoryCell != null)
                    {
                        if (!queryObjectsContexts.ContainsKey(entry.Value.ObjectsContextIdentity))
                        {

                            ObjectsContext objectsContext = null;
                            //if (entry.Value.MemoryCell is InProcessMemoryCell)
                            objectsContext = entry.Value.ObjectsContext;
                            ObjectsContextMetadataDistributionManager objectsContextMetadataDistributionManager = new ObjectsContextMetadataDistributionManager(entry.Value.ObjectsContextIdentity, objectsContext);
                            queryObjectsContexts.Add(objectsContextMetadataDistributionManager.ObjectsContextIdentity, objectsContextMetadataDistributionManager);
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                        }
                        else
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);

                    }
                    else if (entry.Value.StorageCells.Count > 0)
                    {
                        string storageName = null;
                        string storageLocation = null;
                        string storageType = null;
                        string storageIdntity = null;

                        entry.Value.StorageCells[0].GetStorageConnectionData(out storageIdntity, out storageName, out storageLocation, out storageType);
                        ///TODO Ο κώδικας γράφτηκε γιατί έχει προβλημα όταν η storage είναι πάνω σε memory stream   
                        PersistenceLayer.ObjectStorage objectStorage = null;
                        try
                        {
                            var sds = (ObjectQuery as ObjectsContextQuery).ObjectsContext.Identity;
                            var sdssss = entry.Value;
                            var pls = entry.Value.StorageCells[0];
                            var sdd = pls.StorageIdentity;
                            var sdss = entry.Value.StorageCells[0].StorageIdentity;

                            if (entry.Value.StorageCells[0].StorageIdentity == (ObjectQuery as ObjectsContextQuery).ObjectsContext.Identity)
                                objectStorage = (ObjectQuery as ObjectsContextQuery).ObjectStorage;
                            else
                                objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

                        }
                        catch (Exception error)
                        {


                        }
                        if (!queryObjectsContexts.ContainsKey(entry.Value.ObjectsContextIdentity))
                        {

                            ObjectsContextMetadataDistributionManager objectsContextMetadataDistributionManager = new ObjectsContextMetadataDistributionManager(entry.Value.ObjectsContextIdentity, objectStorage);
                            queryObjectsContexts.Add(objectsContextMetadataDistributionManager.ObjectsContextIdentity, objectsContextMetadataDistributionManager);
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                        }
                        else
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                    }
                    else
                    {
                        PersistenceLayer.ObjectStorage objectStorage = null;
                        if (entry.Value.ObjectsContextIdentity == (ObjectQuery as ObjectsContextQuery).ObjectsContext.Identity)
                            objectStorage = (ObjectQuery as ObjectsContextQuery).ObjectStorage;
                        else
                            objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(entry.Value.StorageName, entry.Value.StorageLocation, entry.Value.StorageType);
                        if (!queryObjectsContexts.ContainsKey(entry.Value.ObjectsContextIdentity))
                        {
                            ObjectsContextMetadataDistributionManager objectsContextMetadataDistributionManager = new ObjectsContextMetadataDistributionManager(entry.Value.ObjectsContextIdentity, objectStorage);
                            queryObjectsContexts.Add(objectsContextMetadataDistributionManager.ObjectsContextIdentity, objectsContextMetadataDistributionManager);
                            objectsContextMetadataDistributionManager.DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                        }
                        else
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                    }
                }
            }
            if (Type != DataNodeType.Key)
            {
                foreach (DataNode dataNode in SubDataNodes)
                    dataNode.GetObjectsContexts(queryObjectsContexts);
            }
        }


        /// <exclude>Excluded</exclude>
        bool _ThroughRelationTable = false;
        /// <MetaDataID>{45ca4a07-f3c6-4803-8141-faa83cd9c707}</MetaDataID>
        public bool ThroughRelationTable
        {
            get
            {
                return _ThroughRelationTable;
            }
            set
            {
                _ThroughRelationTable = value;
            }
        }

        /// <MetaDataID>{6ee8d466-071a-4ba4-bbc2-638d18924f39}</MetaDataID>
        public System.Collections.Generic.List<DataNode> RemoveNamespacesDataNodes()
        {
            OOAdvantech.Collections.Generic.List<DataNode> subDataNodes = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode dataNode in new System.Collections.Generic.List<DataNode>(SubDataNodes))
            {
                subDataNodes.AddRange(dataNode.RemoveNamespacesDataNodes());
                if (dataNode.Type != DataNodeType.Namespace && Type == DataNodeType.Namespace)
                {
                    DataNode parentDataNode = this.ParentDataNode;
                    while (parentDataNode != null && parentDataNode.Type == DataNodeType.Namespace)
                        parentDataNode = parentDataNode.ParentDataNode;

                    dataNode.ParentDataNode = parentDataNode;

                }
                if (dataNode.Type == DataNodeType.Namespace)
                    dataNode.ParentDataNode = null;


            }
            if (Type == DataNodeType.Namespace)
                return subDataNodes;
            else
            {
                OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
                dataTrees.Add(this);
                return dataTrees;
            }



        }

        /////<summary>
        /////</summary>
        ///// <MetaDataID>{b96d1000-6799-407c-8d72-bbf9bf361b99}</MetaDataID>
        //internal virtual void FilterData()
        //{
        //    foreach (var searchCondition in SearchConditions)
        //    {
        //        if (searchCondition != null)
        //        {
        //            DataNode commonHeaderDataNode = GetCommonHeaderDatanode(searchCondition);
        //            while (commonHeaderDataNode.ParentDataNode != null &&
        //                commonHeaderDataNode.ParentDataNode.SearchCondition == searchCondition)
        //            {
        //                commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;
        //            }
        //            searchCondition.FilterData(commonHeaderDataNode);
        //        }
        //    }
        //}

        internal DataNode GetCommonHeaderDatanode(DataOrderBy orderByFilter)
        {
            DataNode commonHeaderDataNode = this;
            bool allOrderbyFieldsHasCommonHeaderDataNode = false;
            while (!allOrderbyFieldsHasCommonHeaderDataNode)
            {
                allOrderbyFieldsHasCommonHeaderDataNode = true;
                foreach (DataNode orderByFieldDataNode in orderByFilter.DataNodes)
                {
                    if (!orderByFieldDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
                        allOrderbyFieldsHasCommonHeaderDataNode = false;
                }
                if (!allOrderbyFieldsHasCommonHeaderDataNode)
                    commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;
            }
            return commonHeaderDataNode;
        }
        /// <MetaDataID>{9e0a67d5-3ee2-40f6-a482-9ef1f18a4fab}</MetaDataID>
        internal DataNode GetCommonHeaderDatanode(SearchCondition searchCondition)
        {
            DataNode commonHeaderDataNode = this;
            bool allCrirerionsHasCommonHeaderDataNode = false;
            while (!allCrirerionsHasCommonHeaderDataNode)
            {
                allCrirerionsHasCommonHeaderDataNode = true;
                foreach (DataNode searchConditionDataNode in searchCondition.DataNodes)
                {
                    if (!searchConditionDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
                        allCrirerionsHasCommonHeaderDataNode = false;
                }
                if (!allCrirerionsHasCommonHeaderDataNode)
                    commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;
            }
            return commonHeaderDataNode;
        }
        /// <MetaDataID>{56d11e3d-ff8d-428c-90d9-7a0073fa7c11}</MetaDataID>
        public void MergeSearchConditions()
        {
            foreach (DataNode dataNode in SubDataNodes)
                dataNode.MergeSearchConditions();

            if (SearchCondition != null && ParentDataNode != null && ParentDataNode.SearchCondition != null && SearchCondition != ParentDataNode.SearchCondition)
            {

                foreach (Criterion criterion in ParentDataNode.SearchCondition.Criterions)
                {

                    if (criterion.LeftTermDataNode != null && criterion.LeftTermDataNode.IsSameOrParentDataNode(this))
                    {
                        bool searchConditionExist = false;
                        foreach (SearchFactor searchFactor in criterion.Owner.OwnerSearchTerm.SearchFactors)
                        {
                            if (searchFactor.SearchCondition == SearchCondition)
                            {
                                searchConditionExist = true;
                                break;
                            }
                        }
                        if (!searchConditionExist)
                            criterion.Owner.OwnerSearchTerm.AddSearchFactor(new SearchFactor(SearchCondition));
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        Member<string> _ValueTypePathDiscription = new Member<string>();
        /// <MetaDataID>{60b2d1ab-65e0-456d-90b5-898254ec5555}</MetaDataID>
        /// <summary>
        /// Defines the name which describe the value type path
        /// </summary>
        public string ValueTypePathDiscription
        {
            get
            {
                if (_ValueTypePathDiscription == null)
                    _ValueTypePathDiscription = new Member<string>();
                if (_ValueTypePathDiscription.UnInitialized)
                {
                    DataNode dataNode = this;
                    string valueTypePathDiscription = "";
                    while (dataNode != null && dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNodeType.Object)
                    {
                        if (string.IsNullOrEmpty(valueTypePathDiscription))
                            valueTypePathDiscription = dataNode.Name;
                        else
                            valueTypePathDiscription = dataNode.Name + "_" + valueTypePathDiscription;
                        dataNode = dataNode.ParentDataNode;
                    }
                    _ValueTypePathDiscription.Value = valueTypePathDiscription;
                }
                return _ValueTypePathDiscription;
            }
        }
        /// <exclude>Excluded</exclude>
        MetaDataRepository.ValueTypePath _ValueTypePath;
        /// <MetaDataID>{e7bba08b-8b23-4a0e-8213-e36ab9159e81}</MetaDataID>
        /// <summary> 
        /// Value type path is a collection with the member’s identities which used to access value type data node. 
        /// The start point is the first non value type ancestor data node. 
        /// </summary>
        public MetaDataRepository.ValueTypePath ValueTypePath
        {
            get
            {
                if (_ValueTypePath == null)
                {
                    _ValueTypePath = new ValueTypePath();
                    DataNode dataNode = this;
                    System.Collections.Generic.List<DataNode> dataNodePath = new System.Collections.Generic.List<DataNode>();
                    if (Type == DataNodeType.OjectAttribute && dataNode.RealParentDataNode != null && dataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.RealParentDataNode.Type == DataNodeType.Object)
                    {
                        dataNodePath.Insert(0, dataNode);
                        dataNode = dataNode.RealParentDataNode;
                    }
                    while (dataNode != null && dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNodeType.Object)
                    {
                        dataNodePath.Insert(0, dataNode);
                        dataNode = dataNode.ParentDataNode;
                    }
                    foreach (DataNode pathDataNode in dataNodePath)
                        _ValueTypePath.Push(pathDataNode.AssignedMetaObject.Identity);
                }
                return _ValueTypePath;

            }
        }
        /// <MetaDataID>{452d844f-0a6d-437d-921d-6f9949fbe9ea}</MetaDataID>
        public Type CastingParentType;



        /// <exclude>Excluded</exclude>
        string _OfTypeFilterClassifierFullName;
        /// <MetaDataID>{e71042cc-a125-4126-a2d2-9b62a043ee71}</MetaDataID>
        /// <summary>
        /// When
        /// </summary>
        public string OfTypeFilterClassifierFullName
        {
            get
            {
                return _OfTypeFilterClassifierFullName;
            }
            set
            {
                _OfTypeFilterClassifierFullName = value;
                if (!String.IsNullOrEmpty(OfTypeFilterClassifierFullName))
                {
                    _Classifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(OfTypeFilterClassifierFullName, ClassifierImplementationUnitName));
                    if (_Classifier != null)
                    {
                        ClassifierFullName = _Classifier.FullName;
                        ClassifierImplementationUnitName = _Classifier.ImplementationUnit.FullName;
                    }
                }
            }
        }

        /// <MetaDataID>{83989b80-19c0-4736-ae87-c9bdaf4db764}</MetaDataID>
        /// <summary>
        /// This operation books alias of data node and sub data nodes. 
        /// The aliases must be unary in query.  
        /// </summary>
        internal void BookAlias()
        {
            if (!string.IsNullOrEmpty(Alias))
                Alias = ObjectQuery.GetValidAlias(Alias);

            if (Type == DataNodeType.Object && (ParticipateInSelectClause || ParticipateInWereClause))
            {
                ObjectQuery.BookAlias(ValueTypePathDiscription + "Object");
                ObjectQuery.BookAlias(ValueTypePathDiscription + "LoadObjectLinks");
                if (MembersFetchingObjectActivation && ParticipateInWereClause)
                    ObjectQuery.BookAlias(Alias + "_" + ValueTypePathDiscription + "RowRemove");
                foreach (var searchCondition in SearchConditions)
                {
                    if (searchCondition != SearchCondition)
                        ObjectQuery.BookAlias(Alias + "_" + ValueTypePathDiscription + "RowRemove_" + SearchConditions.IndexOf(searchCondition).ToString());
                }
            }

            foreach (DataNode subDataNode in SubDataNodes)
                subDataNode.BookAlias();
        }


        #region Participate in Group

        /// <exclude>Excluded</exclude>
        private bool _ParticipateInAggregateFunction;
        /// <MetaDataID>{35d6f4d4-db23-4534-8602-144f64624828}</MetaDataID>
        public virtual bool ParticipateInAggregateFunction
        {
            get
            {
                return _ParticipateInAggregateFunction;
            }
            set
            {
                _ParticipateInAggregateFunction = value;
            }
        }



        /// <MetaDataID>{070736fe-6fe5-4371-8c8b-05a2c33390d6}</MetaDataID>
        public bool BranchParticipateInAggregateFunction
        {
            get
            {
                if (_ParticipateInAggregateFunction)
                    return true;
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode.BranchParticipateInAggregateFunction)
                        return true;
                }
                return _ParticipateInAggregateFunction;
            }
        }

        /// <MetaDataID>{d040e96d-4e02-4d47-b2d5-f44b79ede47a}</MetaDataID>
        public bool BranchParticipateInGroopByAsKey
        {
            get
            {
                if (_ParticipateInGroopByAsKey)
                    return true;
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode.BranchParticipateInGroopByAsKey)
                        return true;
                }
                return _ParticipateInGroopByAsKey;
            }
        }
        /// <MetaDataID>{f2b1154d-f639-41ed-a2e4-0e4681d3843e}</MetaDataID>
        public bool BranchParticipateInGroopBy
        {
            get
            {
                if (_ParticipateInGroopBy)
                    return true;
                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (subDataNode.BranchParticipateInGroopBy)
                        return true;
                }
                return _ParticipateInGroopBy;
            }
        }


        /// <exclude>Excluded</exclude>
        private bool _ParticipateInGroopByAsKey;
        /// <MetaDataID>{01348634-6e11-4f30-9213-06ee6f49f4ab}</MetaDataID>
        public virtual bool ParticipateInGroopByAsKey
        {
            get
            {
                return _ParticipateInGroopByAsKey;
            }
            set
            {
                _ParticipateInGroopByAsKey = value;
            }
        }

        /// <exclude>Excluded</exclude>
        private bool _ParticipateInGroopBy;
        /// <MetaDataID>{baf1170d-3063-4167-ba76-b159f095e281}</MetaDataID>
        public virtual bool ParticipateInGroopByAsGrouped
        {
            get
            {
                return _ParticipateInGroopBy;
            }
            set
            {
                _ParticipateInGroopBy = value;
            }
        }


        //bool _AggregateFanctionResultsCalculatedLocally;
        ///// <MetaDataID>{23aac27b-a760-46b5-aba2-ac009537b281}</MetaDataID>
        //public bool AggregateFanctionResultsCalculatedLocally
        //{
        //    get
        //    {
        //        if (ParticipateInAggregateFanction)
        //        {
        //            //return false;
        //            if (_AggregateFanctionResultsCalculatedLocally)
        //                return true;
        //            else
        //            {
        //                if (GroupingAncestorDataNode != null && GroupingAncestorDataNode.GroupKeyDataNodes.Contains(this))
        //                {
        //                    foreach (DataLoader dataLoade in GroupingAncestorDataNode.DataSource.DataLoaders.Values)
        //                    {
        //                        if (!(dataLoade as StorageDataLoader).ParticipateInGlobalResolvedGroup)
        //                        {
        //                            _AggregateFanctionResultsCalculatedLocally = true;
        //                            return true;
        //                        }
        //                    }
        //                }
        //                else
        //                    return false;
        //            }
        //        }
        //        foreach (DataNode dataNode in SubDataNodes)
        //        {
        //            Member<bool> aggregateFanctionResultsCalculatedLocally = new Member<bool>();
        //            if (dataNode.ParticipateInAggregateFanction)
        //            {
        //                aggregateFanctionResultsCalculatedLocally.Value = dataNode.AggregateFanctionResultsCalculatedLocally;
        //                if (!dataNode.AggregateFanctionResultsCalculatedLocally)
        //                    return false;
        //            }
        //            if (!aggregateFanctionResultsCalculatedLocally.UnInitialized)
        //                return aggregateFanctionResultsCalculatedLocally.Value;
        //        }

        //        foreach (DataNode subDataNode in SubDataNodes)
        //        {
        //            if (subDataNode.AggregateFanctionResultsCalculatedLocally)
        //                return true;
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        _AggregateFanctionResultsCalculatedLocally = value;
        //    }

        //}




        /// <MetaDataID>{b495dd9b-02ab-4d89-b7c4-4a47d51c49cd}</MetaDataID>
        /// <summary>
        /// This property defines the first ancestor grouping data node in data tree hierarchy.
        /// If there isn’t grouping data node ancestor return null.
        /// </summary>
        GroupDataNode GroupingAncestorDataNode
        {
            get
            {
                DataNode parentDataNode = ParentDataNode;
                while (parentDataNode != null && parentDataNode.Type != DataNodeType.Group)
                    parentDataNode = parentDataNode.ParentDataNode;
                return parentDataNode as GroupDataNode;
            }
        }



        ///<summary>
        ///Check if data node participate as aggrecation expression member in aggregation expression  DataNode which is subDataNode of  aggregationDataNodesOwner parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines a DataNode which has one or more AggregateExpressionDataNode as subDataNode
        ///</param>
        /// <MetaDataID>{c0a003fa-2587-49d7-901a-bb4706a985d6}</MetaDataID>
        public bool ParticipateInMemberAggregateFunctionOn(DataNode aggregationDataNodesOwner)
        {

            if (aggregationDataNodesOwner == null)
                return false;

            foreach (DataNode dataNode in aggregationDataNodesOwner.SubDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode)
                {
                    if ((dataNode as AggregateExpressionDataNode).ArithmeticExpression != null &&
                        (dataNode as AggregateExpressionDataNode).ArithmeticExpression.ArithmeticExpressionDataNodes.Contains(this))
                    {
                        return true;
                    }
                    else
                    {
                        if ((dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Contains(this))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <MetaDataID>{58dcb96d-780e-4f38-b644-004e68ae3cdd}</MetaDataID>
        public bool ParticipateInAggregateFunctionOn(AggregateExpressionDataNode aggregationDataNode)
        {
            if (aggregationDataNode == null)
                return false;
            if (aggregationDataNode.ArithmeticExpression != null &&
                aggregationDataNode.ArithmeticExpression.ArithmeticExpressionDataNodes.Contains(this))
            {
                return true;
            }
            return false;
        }

        ///<summary>
        ///Check if data node participate as Key in grouping expression which defined from groupingDataNode parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines the group data node
        ///</param>
        /// <MetaDataID>{dd92f64f-a70d-4323-9197-e503b2a3ae01}</MetaDataID>
        public bool ParticipateInGroopByAsKeyOn(GroupDataNode groupingDataNode)
        {
            if (groupingDataNode == null)
                return false;
            return groupingDataNode.GroupKeyDataNodes.Contains(this);
        }


        ///<summary>
        ///Check if data node participate as grouped datanode in grouping expression which defined from groupingDataNode parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines the group data node
        ///</param>
        /// <MetaDataID>{0b407a61-fdcd-4fbf-9408-d3a755550622}</MetaDataID>
        public bool ParticipateAsGroopedDataNodeOn(GroupDataNode groupingDataNode)
        {
            if (groupingDataNode == null)
                return false;
            return groupingDataNode.GroupedDataNode == this;
        }


        ///<summary>
        ///Check if data node or any of subDataNodes participate as aggrecation expression member in aggregation expression DataNode which is subDataNode of  aggregationDataNodesOwner parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines a DataNode which has one or more AggregateExpressionDataNode as subDataNode
        ///</param>
        /// <MetaDataID>{de853fde-9513-4938-9024-873208138b97}</MetaDataID>
        public bool BranchParticipateInMemberAggregateFunctionOn(DataNode groupingDataNode)
        {
            if (ParticipateInMemberAggregateFunctionOn(groupingDataNode))
                return true;
            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.BranchParticipateInMemberAggregateFunctionOn(groupingDataNode))
                    return true;
            }
            return false;

        }

        /// <MetaDataID>{810808c4-e299-4bba-b4a1-2e244e9f095c}</MetaDataID>
        public bool BranchParticipateInAggregateFunctionOn(AggregateExpressionDataNode aggregationDataNode)
        {
            if (ParticipateInAggregateFunctionOn(aggregationDataNode))
                return true;
            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.BranchParticipateInAggregateFunctionOn(aggregationDataNode))
                    return true;
            }
            return false;
        }

        ///<summary>
        ///Check if data node or any of subDataNodes  participate as Key in grouping expression which defined from groupingDataNode parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines the group data node
        ///</param>
        /// <MetaDataID>{dad2a84d-8efd-4712-88eb-0b8990b90c0e}</MetaDataID>
        public bool BranchParticipateInGroopByAsKeyOn(GroupDataNode groupingDataNode)
        {
            if (groupingDataNode == null)
                return false;


            if (ParticipateInGroopByAsKeyOn(groupingDataNode))
                return true;
            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.BranchParticipateInGroopByAsKeyOn(groupingDataNode))
                    return true;
            }
            return false;
        }


        ///<summary>
        ///Check if data node  or any of subDataNodes participate as grouped datanode in grouping expression which defined from groupingDataNode parameter
        ///</summary>
        ///<param name="groupingDataNode">
        ///Defines the group data node
        ///</param>        
        /// <MetaDataID>{35c615ee-dcf9-40d1-82ae-0a5f44da04d9}</MetaDataID>
        public bool BranchParticipateAsGroopedDataNodeOn(GroupDataNode groupingDataNode)
        {
            if (groupingDataNode == null)
                return false;
            if (ParticipateAsGroopedDataNodeOn(groupingDataNode))
                return true;
            foreach (DataNode subDataNode in SubDataNodes)
            {
                if (subDataNode.BranchParticipateAsGroopedDataNodeOn(groupingDataNode))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calculate the execution model for aggregate function DataNodes.
        /// The value for members which derived from aggregation function Datanode canbe retrieve from data loader
        /// locally or can be calculated from object query engine globally
        /// </summary>
        /// <MetaDataID>{adafbe7a-fa2b-499d-8ff0-c2bbcb65a11b}</MetaDataID>
        internal void ResolveAggregateFunctionsExecutionModel()
        {
            foreach (DataNode dataNode in SubDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode)
                {

                    foreach (DataNode aggregateExpressionDataNode in (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                    {
                        if (aggregateExpressionDataNode is AggregateExpressionDataNode)
                            continue;
                        foreach (DataLoader dataLoader in DataSource.DataLoaders.Values)
                            if (dataLoader is StorageDataLoader)
                                if (!(dataLoader as StorageDataLoader).CheckAggregateFunctionForLocalResolve(dataNode as AggregateExpressionDataNode))
                                    aggregateExpressionDataNode.ParticipateInAggregateFunction = true;
                    }
                }

            }
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in SubDataNodes)
                dataNode.ResolveAggregateFunctionsExecutionModel();
        }

        #region Code moved to DataGroup Class


        /////<summary>
        /////Retrieve group data for group data node and load the data source table 
        /////</summary>
        ///// <MetaDataID>{2ee9d2db-46c1-408d-bd06-03a612ff2e89}</MetaDataID>
        // void GroupData()
        //{

        //    if (Type == DataNodeType.Group && !DataSource.GroupedDataLoaded)
        //    {
        //        if ((this as GroupDataNode).GroupingSourceSearchCondition != null)
        //        {
        //            var commonHeaderDataNode = GetCommonHeaderDatanode((this as GroupDataNode).GroupingSourceSearchCondition);
        //            (this as GroupDataNode).GroupingSourceSearchCondition.FilterData(commonHeaderDataNode);
        //        }


        //        //System.Collections.Generic.LinkedList<GroupingDataNode> groupingDataPath = new System.Collections.Generic.LinkedList<GroupingDataNode>();
        //        System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
        //        Dictionary<DataNode, int> dataNodeRowIndices = new Dictionary<DataNode, int>();

        //        #region Builds the data retrieve path for the data which will grouped
        //        if (RealParentDataNode != null && RealParentDataNode.Type == DataNodeType.Object)
        //        {
        //            dataNodeRowIndices[RealParentDataNode] = retrieveDataPath.Count;
        //            retrieveDataPath.AddLast(new DataRetrieveNode(RealParentDataNode, null, dataNodeRowIndices));
        //        }
        //        BuildRetrieveDataPath(retrieveDataPath, new DataRetrieveNode((this as GroupDataNode).GroupByDataNodeRoot, DataRetrieveNode.GetDataRetrieveNode(RealParentDataNode, retrieveDataPath), dataNodeRowIndices), new List<DataRetrieveNode>());




        //        #endregion

        //        int columnsCount = DataSource.DataTable.Columns.Count;
        //        //groupDataIndecies defines a index table wich map the indecies of columns in group table 
        //        //with the columns  indecies in composite rows. 
        //        int[,] groupDataIndecies = new int[columnsCount, 2];

        //        #region Creates the grouping data keys indecies
        //        for (int i = 0; i < columnsCount; i++)
        //        {
        //            groupDataIndecies[i, 0] = -1;
        //            groupDataIndecies[i, 1] = -1;
        //        }

        //        if (ParentDataNode != null)
        //        {

        //            foreach (ObjectIdentityType identityType in ParentDataNode.DataSource.ObjectIdentityTypes)
        //            {
        //                foreach (IIdentityPart identityPart in identityType.Parts)
        //                {
        //                    int i = DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(RealParentDataNode, identityPart));
        //                    groupDataIndecies[i, 0] = retrieveDataPath.First.Value.DataNodeRowIndices[RealParentDataNode];// RealParentDataNode.GroupedDataRowIndex;
        //                    groupDataIndecies[i, 1] = RealParentDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(RealParentDataNode, identityPart));
        //                }
        //            }
        //        }
        //        foreach (DataNode groupKeyDataNode in (this as GroupDataNode).GroupKeyDataNodes)
        //        {
        //            if (groupKeyDataNode.Type == DataNodeType.OjectAttribute)
        //            {
        //                int i = DataSource.GetColumnIndex(groupKeyDataNode);
        //                if (groupKeyDataNode.ParentDataNode.Type == DataNodeType.Object)
        //                    groupDataIndecies[i, 0] = retrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;
        //                else
        //                    groupDataIndecies[i, 0] = retrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;

        //                if (groupKeyDataNode.ParentDataNode.Type == DataNodeType.OjectAttribute)
        //                    groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
        //                else
        //                    groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
        //            }
        //            else if (groupKeyDataNode.Type == DataNodeType.Object)
        //            {

        //                foreach (ObjectIdentityType identityType in groupKeyDataNode.DataSource.ObjectIdentityTypes)
        //                {
        //                    foreach (IIdentityPart identityPart in identityType.Parts)
        //                    {
        //                        int i = DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(groupKeyDataNode, identityPart));
        //                        groupDataIndecies[i, 0] = retrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode];
        //                        groupDataIndecies[i, 1] = groupKeyDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(groupKeyDataNode, identityPart));
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        System.Collections.Generic.List<System.Data.DataRow[]> composedRows = new System.Collections.Generic.List<System.Data.DataRow[]>();

        //        #region Retrieves Group data
        //        foreach (System.Data.DataRow row in retrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
        //        {
        //            System.Data.DataRow[] composedRow = new System.Data.DataRow[retrieveDataPath.Count];
        //            composedRow[retrieveDataPath.First.Value.DataRowIndex] = row;
        //            //composedRow[groupingDataPath.First.Value.DataNode.GroupedDataRowIndex] = row;
        //            //RetrieveData(composedRow, groupingDataPath.First.Next, composedRows);
        //            RetrieveData(composedRow, retrieveDataPath.First.Next, composedRows);
        //        }
        //        #endregion

        //        System.Collections.Generic.Dictionary<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>> dataGroup = new System.Collections.Generic.Dictionary<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>>();

        //        foreach (System.Data.DataRow[] composedRow in composedRows)
        //        {
        //            GroupingKey groupingKey;

        //            #region Constructs the groupingKey
        //            if (ParentDataNode != null)
        //                groupingKey = new GroupingKey((this as GroupDataNode).GroupKeyDataNodes.Count + 1);
        //            else
        //                groupingKey = new GroupingKey((this as GroupDataNode).GroupKeyDataNodes.Count);

        //            int i = 0;
        //            if (ParentDataNode != null)
        //                groupingKey.KeyPartsValues[i++] = composedRow[dataNodeRowIndices[RealParentDataNode]][RealParentDataNode.DataSourceColumnIndex];
        //            foreach (DataNode groupKeyDataNode in (this as GroupDataNode).GroupKeyDataNodes)
        //            {
        //                int dataSourceRowIndex = -1;
        //                dataSourceRowIndex = DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, groupKeyDataNode);
        //                if (composedRow[dataSourceRowIndex] == null)
        //                    groupingKey.KeyPartsValues[i++] = null;
        //                else
        //                    groupingKey.KeyPartsValues[i++] = composedRow[dataSourceRowIndex][groupKeyDataNode.DataSourceColumnIndex];
        //            }
        //            #endregion

        //            System.Collections.Generic.List<System.Data.DataRow[]> groupedCompositeRows = null;
        //            if (!dataGroup.TryGetValue(groupingKey, out groupedCompositeRows))
        //            {
        //                groupedCompositeRows = new System.Collections.Generic.List<System.Data.DataRow[]>();
        //                dataGroup[groupingKey] = groupedCompositeRows;
        //            }
        //            groupedCompositeRows.Add(composedRow);
        //        }
        //        System.Collections.Generic.List<AggregateExpressionDataNode> aggregateDataNodes = new System.Collections.Generic.List<AggregateExpressionDataNode>();
        //        foreach (DataNode subDataNode in SubDataNodes)
        //        {
        //            if (subDataNode is AggregateExpressionDataNode)
        //                aggregateDataNodes.Add(subDataNode as AggregateExpressionDataNode);
        //        }

        //        foreach (System.Collections.Generic.KeyValuePair<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>> entry in dataGroup)
        //        {
        //            System.Data.DataRow row = DataSource.DataTable.NewRow();
        //            System.Data.DataRow[] composedRow = entry.Value[0];

        //            #region load key values on data row
        //            for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
        //            {
        //                if (groupDataIndecies[columnIndex, 0] != -1)
        //                    row[columnIndex] = composedRow[groupDataIndecies[columnIndex, 0]][groupDataIndecies[columnIndex, 1]];
        //            }
        //            #endregion

        //            #region Load aggregation expression results
        //            foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
        //            {
        //                if (aggregateDataNode.Type == DataNodeType.Count)
        //                {
        //                    int count = entry.Value.Count;
        //                    row[aggregateDataNode.DataSourceColumnIndex] = count;
        //                }
        //                if (aggregateDataNode.Type == DataNodeType.Sum)
        //                {
        //                    object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
        //                    row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                }
        //                if (aggregateDataNode.Type == DataNodeType.Average)
        //                {
        //                    object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
        //                    row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                }
        //                if (aggregateDataNode.Type == DataNodeType.Max)
        //                {
        //                    object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
        //                    row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                }
        //                if (aggregateDataNode.Type == DataNodeType.Min)
        //                {
        //                    object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
        //                    row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                }

        //            }
        //            #endregion

        //            DataSource.DataTable.Rows.Add(row);
        //        }

        //    }
        //    else
        //    {
        //        bool runAggregateFunctions = false;
        //        foreach (DataNode dataNode in SubDataNodes)
        //        {
        //            if (dataNode is AggregateExpressionDataNode && !DataSource.AggregateExpressionDataNodeResolved(dataNode as AggregateExpressionDataNode))
        //            {
        //                runAggregateFunctions = true;
        //                break;
        //            }

        //        }


        //        if (runAggregateFunctions)
        //        {

        //            //System.Collections.Generic.LinkedList<GroupingDataNode> groupingDataPath = new System.Collections.Generic.LinkedList<GroupingDataNode>();

        //            System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
        //            Dictionary<DataNode, int> dataNodeRowIndices = new Dictionary<DataNode, int>();
        //            //GroupedDataRowIndex = groupingDataPath.Count;


        //            dataNodeRowIndices[this] = retrieveDataPath.Count;
        //            retrieveDataPath.AddLast(new DataRetrieveNode(this, null, dataNodeRowIndices));
        //            //groupingDataPath.AddLast(new GroupingDataNode(this, groupingDataPath));
        //            System.Collections.Generic.List<AggregateExpressionDataNode> aggregateDataNodes = new System.Collections.Generic.List<AggregateExpressionDataNode>();


        //            foreach (DataNode dataNode in SubDataNodes)
        //            {
        //                if (dataNode is AggregateExpressionDataNode && !DataSource.AggregateExpressionDataNodeResolved(dataNode as AggregateExpressionDataNode))
        //                    aggregateDataNodes.Add(dataNode as AggregateExpressionDataNode);
        //                //if (dataNode.BranchParticipateInAggregateFanction)
        //                //    CreateAggregationDataPath(dataNode, groupingDataPath, new System.Collections.Generic.List<DataNode>());
        //                BuildRetrieveDataPath(retrieveDataPath, new DataRetrieveNode(dataNode, DataRetrieveNode.GetDataRetrieveNode(this, retrieveDataPath), dataNodeRowIndices), new List<DataRetrieveNode>());
        //            }
        //            //foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
        //            //{
        //            //    aggregateDataNode.GroupedDataRowIndex = GroupedDataRowIndex;
        //            //    //aggregateDataNode.DataSourceColumnIndex = DataSource.DataTable.Columns.IndexOf(aggregateDataNode.Alias);
        //            //}


        //            System.Collections.Generic.List<System.Data.DataRow[]> composedRows = new System.Collections.Generic.List<System.Data.DataRow[]>();
        //            foreach (System.Data.DataRow row in retrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
        //            {
        //                composedRows.Clear();
        //                //System.Data.DataRow[] composedRow = new System.Data.DataRow[groupingDataPath.Count];
        //                System.Data.DataRow[] composedRow = new System.Data.DataRow[retrieveDataPath.Count];

        //                //composedRow[GroupedDataRowIndex] = row;
        //                composedRow[retrieveDataPath.First.Value.DataRowIndex] = row;

        //                //RetrieveData(composedRow, groupingDataPath.First.Next, composedRows);
        //                RetrieveData(composedRow, retrieveDataPath.First.Next, composedRows);
        //                composedRows.Clear();
        //                composedRow = new System.Data.DataRow[retrieveDataPath.Count];
        //                composedRow[retrieveDataPath.First.Value.DataRowIndex] = row;

        //                RetrieveData(composedRow, retrieveDataPath.First.Next, composedRows);

        //                foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
        //                {
        //                    if (aggregateDataNode.Type == DataNodeType.Count)
        //                    {
        //                        int count = composedRows.Count;
        //                        row[aggregateDataNode.DataSourceColumnIndex] = count;
        //                    }
        //                    if (aggregateDataNode.Type == DataNodeType.Sum)
        //                    {
        //                        object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, composedRows);
        //                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                    }
        //                    if (aggregateDataNode.Type == DataNodeType.Average)
        //                    {
        //                        object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, composedRows);
        //                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                    }
        //                    if (aggregateDataNode.Type == DataNodeType.Max)
        //                    {
        //                        object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, composedRows);
        //                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                    }
        //                    if (aggregateDataNode.Type == DataNodeType.Min)
        //                    {
        //                        object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, composedRows);
        //                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //}

        ///// <MetaDataID>{e478a6dc-578d-4406-b30a-4fd1ef3d1f6e}</MetaDataID>
        //private void RetrieveData(System.Data.DataRow[] composedRow,
        //                  System.Collections.Generic.LinkedListNode<DataRetrieveNode> groupingDataPath,
        //                  System.Collections.Generic.List<System.Data.DataRow[]> composedRows)
        //{

        //    if (groupingDataPath == null)
        //    {
        //        composedRows.Add(composedRow.Clone() as System.Data.DataRow[]);
        //    }
        //    else
        //    {
        //        System.Collections.Generic.ICollection<System.Data.DataRow> rows = null;
        //        if (composedRow[groupingDataPath.Value.DataNodeRowIndices[groupingDataPath.Previous.Value.DataNode]] != null)
        //            rows = groupingDataPath.Value.MasterDataNode.DataNode.DataSource.GetRelatedRows(composedRow[groupingDataPath.Value.MasterDataNode.DataRowIndex], groupingDataPath.Value.DataNode);

        //        if (rows == null || rows.Count == 0)
        //        {
        //            //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
        //            composedRow[groupingDataPath.Value.DataRowIndex] = null;
        //            return;

        //        }
        //        else
        //        {
        //            foreach (System.Data.DataRow row in rows)
        //            {
        //                if (groupingDataPath.Value.DataNode.SearchCondition != null && groupingDataPath.Value.DataNode.SearchCondition.IsRemovedRow(row, -1))
        //                    continue;
        //                if (this is GroupDataNode && (this as GroupDataNode).GroupingSourceSearchCondition != null &&
        //                    ((groupingDataPath.Value.DataNode.SearchCondition != null &&
        //                    !groupingDataPath.Value.DataNode.SearchCondition.ContainsSearchCondition((this as GroupDataNode).GroupingSourceSearchCondition)) ||
        //                    groupingDataPath.Value.DataNode.SearchCondition == null))
        //                {
        //                    if ((this as GroupDataNode).GroupingSourceSearchCondition.IsRemovedRow(row, -1))
        //                        continue;
        //                }


        //                composedRow[groupingDataPath.Value.DataRowIndex] = row;
        //                RetrieveData(composedRow, groupingDataPath.Next, composedRows);
        //            }
        //        }
        //    }
        //}

        ///// <MetaDataID>{35186213-bc08-4165-9a0b-bb915937245c}</MetaDataID>
        ///// <summary>
        ///// Builds data retrieve path to reads data for grouping 
        ///// </summary>
        ///// <param name="groupingDataPath">
        ///// groupingDataPath parameter defines a link list with data retrieve nodes which used to collect data for grouping.
        ///// </param>
        ///// <param name="dataRetrieveNode">
        ///// Defines the next data retrieve node of groupingDataPath
        ///// </param>
        ///// <param name="unAssignedNodes">
        ///// unAssignedNodes defines pending nodes to add in groupingDataPath.
        ///// </param>

        //private void BuildRetrieveDataPath(System.Collections.Generic.LinkedList<DataRetrieveNode> groupingDataPath, DataRetrieveNode dataRetrieveNode, System.Collections.Generic.List<DataRetrieveNode> unAssignedNodes)
        //{
        //    Dictionary<DataNode, int> dataNodeRowIndices = null;
        //    if (groupingDataPath.Count > 0)
        //        dataNodeRowIndices = groupingDataPath.First.Value.DataNodeRowIndices;
        //    else
        //    {
        //        if (dataRetrieveNode != null)
        //            dataNodeRowIndices = dataRetrieveNode.DataNodeRowIndices;
        //        else
        //            dataNodeRowIndices = new Dictionary<DataNode, int>();
        //    }

        //    if (dataRetrieveNode == null)
        //        dataRetrieveNode = new DataRetrieveNode(this, null, dataNodeRowIndices);
        //    if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Namespace)
        //    {
        //        BuildRetrieveDataPath(groupingDataPath, new DataRetrieveNode(dataRetrieveNode.DataNode.SubDataNodes[0], null, dataNodeRowIndices), unAssignedNodes);
        //        return;
        //    }

        //    System.Collections.Generic.List<DataRetrieveNode> subNodes = new System.Collections.Generic.List<DataRetrieveNode>();
        //    if ((dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Object || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.OjectAttribute) &&
        //       (dataRetrieveNode.DataNode.BranchParticipateInMemberAggregateFanctionOn(this) || dataRetrieveNode.DataNode.BranchParticipateInGroopByAsKeyOn(this as GroupDataNode) || dataRetrieveNode.DataNode.BranchParticipateAsGroopedDataNodeOn(this as GroupDataNode)))
        //    {
        //        if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Object || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group)
        //        {
        //            dataNodeRowIndices[dataRetrieveNode.DataNode] = groupingDataPath.Count;
        //            groupingDataPath.AddLast(dataRetrieveNode);
        //        }


        //        #region retrieves all sub data nodes which participate in search condition as branch
        //        if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group)
        //        {
        //            foreach (DataNode subDataNode in (dataRetrieveNode.DataNode as GroupDataNode).GroupKeyDataNodes)
        //            {
        //                if ((subDataNode.BranchParticipateInMemberAggregateFanctionOn(this) || subDataNode.BranchParticipateInGroopByAsKeyOn(this as GroupDataNode)) &&
        //                    (subDataNode.Type == DataNode.DataNodeType.Object))
        //                    subNodes.Add(new DataRetrieveNode(subDataNode, dataRetrieveNode, dataNodeRowIndices));
        //            }
        //        }
        //        else
        //        {
        //            foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
        //            {
        //                if ((subDataNode.BranchParticipateInMemberAggregateFanctionOn(this) || subDataNode.BranchParticipateInGroopByAsKeyOn(this as GroupDataNode) || subDataNode.BranchParticipateAsGroopedDataNodeOn(this as GroupDataNode)) &&
        //                    subDataNode.Type == DataNode.DataNodeType.Object)
        //                    subNodes.Add(new DataRetrieveNode(subDataNode, dataRetrieveNode, dataNodeRowIndices));
        //            }
        //        }
        //        #endregion

        //        if (subNodes.Count > 0)
        //        {
        //            #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
        //            DataRetrieveNode subDataNode = subNodes[0];
        //            subNodes.RemoveAt(0);

        //            unAssignedNodes.AddRange(subNodes);

        //            BuildRetrieveDataPath(groupingDataPath, subDataNode, unAssignedNodes);
        //            #endregion
        //        }
        //        else if (unAssignedNodes.Count > 0)
        //        {
        //            #region Continues recursively with first unassigned datanode the others will be added at the end.
        //            DataRetrieveNode subDataNode = unAssignedNodes[0];
        //            unAssignedNodes.RemoveAt(0);
        //            BuildRetrieveDataPath(groupingDataPath, subDataNode, unAssignedNodes);
        //            #endregion
        //        }
        //    }
        //}

        #endregion

        #endregion






        /// <MetaDataID>{07b4ea4b-af51-4f51-aede-45177e8090ef}</MetaDataID>
        internal virtual bool IsDataSource
        {
            get
            {


                if (Type == DataNodeType.Object)
                    return true;
                else
                    return false;
            }
        }
        /// <MetaDataID>{a35c4871-8762-4756-9de5-9937a529385f}</MetaDataID>
        internal virtual bool IsDataSourceMember
        {
            get
            {


                if (Type == DataNodeType.OjectAttribute)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{f82a2a90-e31c-4cbe-92a0-e476958264c6}</MetaDataID>
        bool _RefreshObjectState;
        /// <MetaDataID>{bd61f9e5-8d5f-4811-8519-4ca6ff2a7ccb}</MetaDataID>
        public bool RefreshObjectState
        {
            get
            {
                return _RefreshObjectState;
            }
            internal set
            {
                _RefreshObjectState = value;
            }
        }
    }


}
