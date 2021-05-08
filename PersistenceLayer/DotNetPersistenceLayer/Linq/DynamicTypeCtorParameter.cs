using System;
using System.Collections.Generic;
using OOAdvantech.Linq.QueryExpressions;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{e9e211f1-a868-4663-8952-0d6b5138e0bb}</MetaDataID>
     class DynamicTypeCtorParameter
    {
        [Association("", Roles.RoleA, "2a2fabb8-f299-48b9-bc7f-2ce249dd4af2")]
        [RoleAMultiplicityRange(1, 1)]
        public readonly IDynamicTypeDataRetrieve ParameterType;


        public readonly System.Reflection.ParameterInfo ParameterInfo;
        internal System.Collections.Generic.List<TypeExpressionNode> TypeExpresionsPathToSource
        {
            get
            {
                System.Collections.Generic.List<TypeExpressionNode> route = new System.Collections.Generic.List<TypeExpressionNode>();
                if (TreeNode is QueryExpressions.ParameterExpressionTreeNode)
                {
                    ExpressionTreeNode parameterSource = TreeNode.ExpressionTranslator.ParameterDeclareExpression[TreeNode.Expression as System.Linq.Expressions.ParameterExpression];
                    IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
                    if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == TreeNode.Expression)
                        dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                    else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == TreeNode.Expression)
                        dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                    else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == TreeNode.Expression)
                        dynamicTypeDataRetrieve = (parameterSource as SelectManyExpressionTreeNode).DerivedCollection.DynamicTypeDataRetrieve;
                    if (dynamicTypeDataRetrieve != null)
                    {
                        route.Add(new TypeExpressionNode(PropertyOwnerType, PropertyOwnerType.Type, TreeNode, dynamicTypeDataRetrieve, dynamicTypeDataRetrieve.Type));
                        if (TreeNode.Nodes.Count > 0)
                            (TreeNode.Nodes[0] as MemberAccessExpressionTreeNode).GetTypeExpressionRouteToSource(dynamicTypeDataRetrieve, route);
                    }
                    else
                    {
                        if (TreeNode.Nodes.Count > 0)
                            (TreeNode.Nodes[0] as MemberAccessExpressionTreeNode).GetTypeExpressionRouteToSource(dynamicTypeDataRetrieve, route);
                    }
                }

                return route;

            }
        }

        /// <exclude>Excluded</exclude>
        DataNode _SourceDataNode;
        /// <MetaDataID>{4d3d2c8a-ee83-4dc4-b2d7-370a2f1f5913}</MetaDataID>
        public DataNode SourceDataNode
        {
            get
            {

                return _SourceDataNode;
            }
            set
            {
                _SourceDataNode = value;
            }
        }

        public SearchCondition FilterDataCondition
        {
            get
            {
                if (TreeNode is QueryExpressions.ParameterExpressionTreeNode)
                    return SearchCondition.JoinSearchConditions(PropertyOwnerType.FilterDataCondition, (TreeNode as QueryExpressions.ParameterExpressionTreeNode).SearchConditionA);

                if (TreeNode is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                    return SearchCondition.JoinSearchConditions(PropertyOwnerType.FilterDataCondition, (TreeNode as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).FilterDataCondition);


                return null;
            }
        }

        internal IDynamicTypeDataRetrieve PropertyOwnerType;
        /// <MetaDataID>{6b291d41-c3fc-48de-852d-2e0099b1a836}</MetaDataID>
        public readonly ExpressionTreeNode TreeNode;
        /// <MetaDataID>{e6dcdb24-e60c-4ef8-914f-3e56c00ecfb7}</MetaDataID>
         DynamicTypeCtorParameter(System.Reflection.ParameterInfo parameterInfo, DataNode sourceDataNode, IDynamicTypeDataRetrieve propertyType, ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            ParameterInfo = parameterInfo;
            _SourceDataNode = sourceDataNode;
            ParameterType = propertyType;

            //if (propertyType != null && !propertyType.IsGrouping && OOAdvantech.TypeHelper.IsEnumerable(PropertyInfo.PropertyType) && propertyType.Properties == null)
            //    PropertyType = null;

            if (ParameterInfo.ParameterType.Name.IndexOf("<>f_") == 0 && ParameterType == null && _SourceDataNode.Type != DataNode.DataNodeType.Key)
                throw new System.Exception("System in incosistent state");
            if (!ParameterTypeIsEnumerable && propertyType != null && propertyType.Type != ParameterInfo.ParameterType)
                throw new System.Exception(ParameterInfo.Name + ": Type mismatch");
        }
        /// <summary>
        /// This property defines that dynamic property is consant value getter when is true
        /// </summary>
        public readonly bool IsLocalScopeValue;
        public readonly object LocalScopeValue;

        public int[] PropertyIndices = new int[2];

         DynamicTypeCtorParameter(System.Reflection.ParameterInfo parameterInfo, DataNode sourceDataNode,  ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            ParameterInfo = parameterInfo;
            IsLocalScopeValue = true;
            _SourceDataNode = sourceDataNode;
            LocalScopeValue = (treeNode as QueryExpressions.ConstantExpressionTreeNode).Value;
        }

        /// <MetaDataID>{b8d39ad0-0273-4a2a-ab0f-e6b7c0a8f832}</MetaDataID>
        public bool ParameterTypeIsDynamic
        {
            get
            {
                return ParameterType != null;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<bool> _ParameterTypeIsEnumerable = new Member<bool>();
        /// <MetaDataID>{ae6c6154-292a-4f22-b1a8-784bb292209d}</MetaDataID>
        public bool ParameterTypeIsEnumerable
        {
            get
            {
                if (_ParameterTypeIsEnumerable.UnInitialized)
                {
                    if (typeof(System.Linq.IGrouping<,>).Name == ParameterInfo.ParameterType.Name)
                        _ParameterTypeIsEnumerable.Value = false;
                    else
                        _ParameterTypeIsEnumerable.Value = TypeHelper.FindIEnumerable(ParameterInfo.ParameterType) != null;
                }
                return _ParameterTypeIsEnumerable;
            }
        }

        public QueryResultPart QueryResultMember;

        internal void LoadPropertyValue(QueryResultLoaderEnumartor queryResultDataLoader, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows)
        {
            if (QueryResultMember is CompositePart)
                queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = ParameterType.InstantiateObject(queryResultDataLoader, dataNodesRows);
            else if (QueryResultMember is EnumerablePart)
            {
                if (ParameterType == null && ParameterTypeIsEnumerable)
                {

                    System.Collections.IList list = System.Activator.CreateInstance(typeof(List<>).MakeGenericType(OOAdvantech.TypeHelper.GetElementType(ParameterInfo.ParameterType))) as System.Collections.IList;
                    var dataLoader = queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as QueryResultDataLoader;
                    dataLoader.ObjectQueryContextReference = this.PropertyOwnerType.QueryResult.ObjectQueryContextReference;
                    foreach (CompositeRowData row in dataLoader)
                    {
                        object item = row[dataLoader.Type.ConventionTypeRowIndex][dataLoader.Type.ConventionTypeColumnIndex];
                        list.Add(item);
                    }

                    queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = Activator.CreateInstance(ParameterInfo.ParameterType, list);
                }
                else
                {

                    //Transform Query result dataloader to DynamicDataRetrieveType 
                    // if(!(queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] is IDynamicTypeDataRetrieve))
                    queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = ParameterType.GetRelatedDataEnumerator(queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as QueryResultDataLoader);
                }
            }
            else if (QueryResultMember is EnumerablePart && (QueryResultMember as EnumerablePart).Type.IsGroupingType)
            {
                if (ParameterInfo.ParameterType.Name == typeof(System.Linq.IGrouping<,>).Name)
                {
                    #region Retrieve IGroup instance
                    IDataRow masterRow = null;
                    //masterRow = dataNodesRows[SourceDataNode.ParentDataNode];
                    //IDynamicTypeDataRetrieve GroupedDataEnumerator = PropertyType.GetRelatedDataEnumerator(masterRow, dataNodesRows) as IDynamicTypeDataRetrieve;
                    //System.Data.DataRow groupRow = dataNodesRows[SourceDataNode];
                    //if (groupRow != null)
                    //{
                    //    object groupInstance = GroupedDataEnumerator.GetGroupInstance(groupRow);
                    //    compositeRow[PropertyIndices[0]][PropertyIndices[1]] = groupInstance;
                    //}
                    #endregion
                }
            }
        }


    }
}