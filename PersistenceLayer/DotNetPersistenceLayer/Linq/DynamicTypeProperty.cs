using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.MetaDataRepository;
using System.Linq.Expressions;
using OOAdvantech.Linq.QueryExpressions;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{1cb21dd1-4db2-46d0-a41d-2e08fb63fc88}</MetaDataID>
    class DynamicTypeProperty
    {


        /// <MetaDataID>{35b601f6-3acd-4a2c-878b-652bb9be23bf}</MetaDataID>
        public QueryResultPart QueryResultMember;

        [Association("", Roles.RoleA, "dff139cc-44cc-49de-86f7-daa6379c62dc")]
        public readonly IDynamicTypeDataRetrieve PropertyType;


        /// <MetaDataID>{511a9877-c2ec-40ac-801d-2f20c7be876f}</MetaDataID>
        public Type Type
        {
            get
            {
                if (PropertyInfo != null)
                    return PropertyInfo.PropertyType;
                if (ParameterInfo != null)
                    return ParameterInfo.ParameterType;

                return null;
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

        /// <MetaDataID>{a4480144-5b0f-4507-a020-ceb707d07324}</MetaDataID>
        public readonly System.Reflection.ParameterInfo ParameterInfo;
        ///// <MetaDataID>{ffcdb0a1-c4d1-4786-9377-c8c6f66eff8d}</MetaDataID>
        //public readonly IDynamicTypeDataRetrieve PropertyType;

        /// <MetaDataID>{13cd4bc6-74f6-4c57-a360-94e344859556}</MetaDataID>
        public readonly System.Reflection.PropertyInfo PropertyInfo;
        /// <MetaDataID>{b95616dc-240c-4747-8ce9-cd4f6c7a3ea1}</MetaDataID>
        DynamicTypeProperty()
        {

        }


        /// <MetaDataID>{e046020a-58c5-4a43-926a-6ec33f38039a}</MetaDataID>
        internal List<TypeExpressionNode> TypeExpresionsPathToSource
        {
            get
            {
                List<TypeExpressionNode> route = new List<TypeExpressionNode>();
                if (TreeNode is QueryExpressions.ParameterExpressionTreeNode)
                {
                    ExpressionTreeNode parameterSource = TreeNode.ExpressionTranslator.ParameterDeclareExpression[TreeNode.Expression as ParameterExpression];
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



        /// <MetaDataID>{2c4ecc82-a881-489a-8a7b-61bfac63e0a9}</MetaDataID>
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

        /// <MetaDataID>{672e9b74-40fa-46c7-b4ba-b562a87608d1}</MetaDataID>
        internal IDynamicTypeDataRetrieve PropertyOwnerType;
        /// <MetaDataID>{6b291d41-c3fc-48de-852d-2e0099b1a836}</MetaDataID>
        public readonly ExpressionTreeNode TreeNode;
        /// <MetaDataID>{e6dcdb24-e60c-4ef8-914f-3e56c00ecfb7}</MetaDataID>
        public DynamicTypeProperty(System.Reflection.PropertyInfo propertyInfo, DataNode sourceDataNode, IDynamicTypeDataRetrieve propertyType, ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            PropertyInfo = propertyInfo;
            _SourceDataNode = sourceDataNode;
            PropertyType = propertyType;

            //if (propertyType != null && !propertyType.IsGrouping && OOAdvantech.TypeHelper.IsEnumerable(PropertyInfo.PropertyType) && propertyType.Properties == null)
            //    PropertyType = null;

            if (PropertyInfo.PropertyType.Name.IndexOf("<>f_") == 0 && PropertyType == null && _SourceDataNode.Type != DataNode.DataNodeType.Key)
                throw new System.Exception("System in incosistent state");
            if (!PropertyTypeIsEnumerable && propertyType != null && propertyType.Type != propertyInfo.PropertyType)
                throw new System.Exception(propertyInfo.Name + ": Type mismatch");
        }


        /// <MetaDataID>{71695e84-0b28-4e2e-bdb0-431bf8416aab}</MetaDataID>
        public DynamicTypeProperty(System.Reflection.ParameterInfo parameterInfo, DataNode sourceDataNode, IDynamicTypeDataRetrieve propertyType, ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            ParameterInfo = parameterInfo;
            _SourceDataNode = sourceDataNode;
            PropertyType = propertyType;

            //if (propertyType != null && !propertyType.IsGrouping && OOAdvantech.TypeHelper.IsEnumerable(PropertyInfo.PropertyType) && propertyType.Properties == null)
            //    PropertyType = null;

            if (parameterInfo.ParameterType.Name.IndexOf("<>f_") == 0 && PropertyType == null && _SourceDataNode.Type != DataNode.DataNodeType.Key)
                throw new System.Exception("System in incosistent state");
            if (!PropertyTypeIsEnumerable && propertyType != null && propertyType.Type != ParameterInfo.ParameterType)
                throw new System.Exception(ParameterInfo.Name + ": Type mismatch");
        }
        /// <summary>
        /// This property defines that dynamic property is consant value getter when is true
        /// </summary>
        /// <MetaDataID>{585480a4-1a60-43e1-a2bb-95bf47b26555}</MetaDataID>
        public readonly bool IsLocalScopeValue;
        /// <MetaDataID>{47a80613-4a90-4bad-a801-683d4892e81b}</MetaDataID>
        public readonly object LocalScopeValue;

        /// <MetaDataID>{20c4ecfe-f1de-4353-9344-70a504ddee60}</MetaDataID>
        public int[] PropertyIndices = new int[2];

        /// <MetaDataID>{14a8706c-8be5-463d-9495-69267c977835}</MetaDataID>
        public DynamicTypeProperty(System.Reflection.PropertyInfo propertyInfo, DataNode sourceDataNode, ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            PropertyInfo = propertyInfo;
            IsLocalScopeValue = true;
            _SourceDataNode = sourceDataNode;
            LocalScopeValue = (treeNode as QueryExpressions.ConstantExpressionTreeNode).Value;
        }


        /// <MetaDataID>{abd94d9e-465d-4316-a4a1-ae4d9b684c1a}</MetaDataID>
        public DynamicTypeProperty(System.Reflection.ParameterInfo parameterInfo, DataNode sourceDataNode, ExpressionTreeNode treeNode)
        {
            TreeNode = treeNode;
            ParameterInfo = parameterInfo;
            IsLocalScopeValue = true;
            _SourceDataNode = sourceDataNode;
            LocalScopeValue = (treeNode as QueryExpressions.ConstantExpressionTreeNode).Value;
        }


        /// <MetaDataID>{b8d39ad0-0273-4a2a-ab0f-e6b7c0a8f832}</MetaDataID>
        public bool PropertyTypeIsDynamic
        {
            get
            {
                return PropertyType != null;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<bool> _PropertyTypeIsEnumerable = new Member<bool>();
        /// <MetaDataID>{ae6c6154-292a-4f22-b1a8-784bb292209d}</MetaDataID>
        public bool PropertyTypeIsEnumerable
        {
            get
            {
                if (_PropertyTypeIsEnumerable.UnInitialized)
                {
                    if (typeof(System.Linq.IGrouping<,>).Name == Type.Name)
                        _PropertyTypeIsEnumerable.Value = false;
                    else
                        _PropertyTypeIsEnumerable.Value = TypeHelper.FindIEnumerable(Type) != null;
                }
                return _PropertyTypeIsEnumerable;
            }
        }

        /// <MetaDataID>{5d2fef94-af05-4943-9255-27de400bb835}</MetaDataID>
        public string PropertyName
        {
            get
            {
                if (PropertyInfo != null)
                    return PropertyInfo.Name;
                else
                    return ParameterInfo.Name;
            }
        }

        /// <MetaDataID>{fec07808-edd7-484f-8100-cdfee0ea3af4}</MetaDataID>
        internal void LoadPropertyValue(QueryResultLoaderEnumartor queryResultDataLoader, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows)
        {
            if (QueryResultMember is CompositePart)
                queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = PropertyType.InstantiateObject(queryResultDataLoader, dataNodesRows);
            else if (QueryResultMember is EnumerablePart)
            {
                if (PropertyType == null && PropertyTypeIsEnumerable)
                {

                    System.Collections.IList list = System.Activator.CreateInstance(typeof(List<>).MakeGenericType(OOAdvantech.TypeHelper.GetElementType(PropertyInfo.PropertyType))) as System.Collections.IList;
                    var dataLoader = queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as QueryResultDataLoader;
                    dataLoader.ObjectQueryContextReference = this.PropertyOwnerType.QueryResult.ObjectQueryContextReference;
                    foreach (CompositeRowData row in dataLoader)
                    {
                        object item = row[dataLoader.Type.ConventionTypeRowIndex][dataLoader.Type.ConventionTypeColumnIndex];
                        list.Add(item);
                    }

                    queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = Activator.CreateInstance(PropertyInfo.PropertyType, list);
                }
                else
                {

                    //Transform Query result dataloader to DynamicDataRetrieveType 

                    var relatedQueryResultDataLoader = queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as QueryResultDataLoader;
                    queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] = PropertyType.GetRelatedDataEnumerator(relatedQueryResultDataLoader);
                    if (relatedQueryResultDataLoader.Type.IsGroupingType && PropertyType.GroupingMetaData != null && queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] is IGroupingDataRetriever)
                    {
                        if (PropertyType.GroupingMetaData.KeyDynamicTypeDataRetrieve != null)
                            (queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as IGroupingDataRetriever).KeyValue = PropertyType.GroupingMetaData.KeyDynamicTypeDataRetrieve.InstantiateObject(queryResultDataLoader, dataNodesRows);
                        else
                        {
                            if (PropertyType.QueryResult.Members[0].SourceDataNode.Type == DataNode.DataNodeType.Key&&
                                DerivedDataNode.GetOrgDataNode( PropertyType.QueryResult.Members[0].SourceDataNode.SubDataNodes[0]).Type==DataNode.DataNodeType.OjectAttribute&&
                                PropertyType.QueryResult.Members[0] is SinglePart)
                            {
                                GroupingEntry groupingEntry = queryResultDataLoader.Current[PropertyType.QueryResult.Members[0].PartIndices[0]][PropertyType.QueryResult.Members[0].PartIndices[1]] as GroupingEntry;
                                (queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as IGroupingDataRetriever).KeyValue = groupingEntry.GroupingKey.KeyPartsValues[0];
                            }
                            else
                                (queryResultDataLoader.Current[QueryResultMember.PartIndices[0]][QueryResultMember.PartIndices[1]] as IGroupingDataRetriever).KeyValue = queryResultDataLoader.Current[PropertyType.QueryResult.Members[0].PartIndices[0]][PropertyType.QueryResult.Members[0].PartIndices[1]];
                        }

                    }
                }
            }
            else if (QueryResultMember is EnumerablePart && (QueryResultMember as EnumerablePart).Type.IsGroupingType)
            {
                if (PropertyInfo.PropertyType.Name == typeof(System.Linq.IGrouping<,>).Name)
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

        //private object GetRelatedDataCollection(System.Data.DataRow[] compositeRow, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows parentsDataNodesRows)
        //{

        //    if (!PropertyTypeIsEnumerable)
        //        throw new System.Exception("method can load collection only for enumerable properties");


        //  return  PropertyType.GetRelatedDataEnumerator(new QueryResultDataLoader((QueryResultMember as EnumerablePart).Type,(QueryResultMember as EnumerablePart).Type.RootTypeReference, parentsDataNodesRows),);

        //    //System.Collections.IList collection = null;
        //    //if (PropertyTypeIsDynamic)//  entry.Value.PropertyType!=null)//  (elementType.Name.IndexOf("<>f__AnonymousType") == 0)
        //    //    return PropertyType.GetRelatedDataEnumerator(masterRow, parentsDataNodesRows);
        //    //else
        //    //{

        //    //    ICollection<System.Data.DataRow> rows = SourceDataNode.RealParentDataNode.DataSource.GetRelatedRows(masterRow, SourceDataNode);
        //    //    //Δεν θα φτάσει ποτέ ο κωδικας εδώ γιατί οι callers αυτής της function με συνθήκες if αποκλύουν αυτό το γεγονός.  
        //    //    collection = Activator.CreateInstance(PropertyInfo.PropertyType) as System.Collections.IList;
        //    //    //int rowRemoveIndex = dynamicTypeProperty.SourceDataNode.DataSource.RowRemoveIndex;
        //    //    if (masterRow != null)
        //    //    {

        //    //        foreach (System.Data.DataRow row in rows)
        //    //        {
        //    //            if (SourceDataNode.SearchCondition != null && SourceDataNode.SearchCondition.IsRemovedRow(row, SourceDataNode.DataSource))
        //    //                continue;
        //    //            collection.Add(row[SourceDataNode.DataSource.ObjectIndex]);
        //    //        }
        //    //    }
        //    //    return collection;
        //    //}

        //}
    }
    /// <MetaDataID>{6adb4dda-f3b7-4772-8ec1-457926c20842}</MetaDataID>
    class TypeExpressionNode
    {
        public override string ToString()
        {
            string str = null;
            foreach (var property in SourceType.GetMetaData().GetProperties())
            {
                if (str != null)
                    str += " , " + property.Name;
                else
                    str += property.Name;
            }
            str = "(" + str + ") " + ExpressionTreeNode.ToString();
            return str;
        }

        public TypeExpressionNode(IDynamicTypeDataRetrieve sourceDynamicTypeDataRetrieve, Type sourceType, ExpressionTreeNode treeNode, IDynamicTypeDataRetrieve dynamicTypeDataRetrieve, Type type)
        {
            ExpressionTreeNode = treeNode;
            DynamicTypeDataRetrieve = dynamicTypeDataRetrieve;
            Type = type;
            SourceDynamicTypeDataRetrieve = sourceDynamicTypeDataRetrieve;
            SourceType = sourceType;
        }
        public IDynamicTypeDataRetrieve SourceDynamicTypeDataRetrieve;
        public ExpressionTreeNode ExpressionTreeNode;
        public IDynamicTypeDataRetrieve DynamicTypeDataRetrieve;
        public Type Type;
        public Type SourceType;
    }

}
