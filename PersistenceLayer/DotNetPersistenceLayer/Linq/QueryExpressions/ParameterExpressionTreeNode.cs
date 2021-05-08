using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{15b0f1f7-c023-4e8a-8aff-42820d944c3c}</MetaDataID>
    ///<summary>
    ///Defines a class with a sequence of expresion to access object member value
    ///</summary>
    class ParameterExpressionTreeNode : ExpressionTreeNode
    {

        /// <MetaDataID>{5e1deeac-6a61-436f-8890-1bd96937dc94}</MetaDataID>
        public bool InitCtorParameter { get; set; }

        /// <summary>
        /// Defines the the DynamicTypeDataRetrieve for the parameter expression type 
        /// </summary>
        internal IDynamicTypeDataRetrieve RootDynamicTypeDataRetrieve
        {
            get
            {
                ExpressionTreeNode parameterSource = ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression];
                IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
                if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == Expression)
                    dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == Expression)
                    dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == Expression)
                    dynamicTypeDataRetrieve = (parameterSource as SelectManyExpressionTreeNode).DerivedCollection.DynamicTypeDataRetrieve;
                else if (parameterSource is GroupByExpressionTreeNode && (parameterSource as GroupByExpressionTreeNode).ResultSelectorParameter == Expression)
                    dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.IsGrouping && Parent.DataNode is AggregateExpressionDataNode)
                    dynamicTypeDataRetrieve = dynamicTypeDataRetrieve.GroupingMetaData.GroupedDataRetrieve;



                return dynamicTypeDataRetrieve;
            }
        }


        /// <summary>
        /// Defines the the DynamicTypeDataRetrieve for the type of member access path.
        /// </summary>
        internal override IDynamicTypeDataRetrieve DynamicTypeDataRetrieve
        {
            get
            {
                IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = RootDynamicTypeDataRetrieve;
                //var dynamicTypeRetrive = (ExpressionTranslator.ParameterDeclareExpression[SourceCollection.Expression as ParameterExpression] as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                if (dynamicTypeDataRetrieve != null)
                {
                    if (MemberAccess == null)
                        return dynamicTypeDataRetrieve;
                    else
                        return GetDynamicTypeDataRetriever(dynamicTypeDataRetrieve, MemberAccess);
                }
                return null;
            }
        }

        /// <summary>
        /// Parse members access path and retrieves the corresponding DynamicTypeDataRetrieve of last access member  type. 
        /// In case where there isn't DynamicTypeDataRetrieve for the last access member type return null;
        /// </summary>
        /// <param name="dynamicTypeRetrive">
        /// Defines the DynamicTypeDataRetrieve for MemberExpresion member Declaring Type 
        /// </param>
        /// <param name="memberAccessExpressionTreeNode">
        /// Defines the MemberAccessExpression tree node.
        /// </param>
        /// <returns>
        /// The DynamicTypeDataRetrieve for the last access member type.  
        /// </returns>
        /// <MetaDataID>{2fbe0497-ad34-4cd0-b8eb-2ced2150e2c1}</MetaDataID>
        internal static IDynamicTypeDataRetrieve GetDynamicTypeDataRetriever(IDynamicTypeDataRetrieve dynamicTypeRetrive, MemberAccessExpressionTreeNode memberAccessExpressionTreeNode)
        {
            DynamicTypeProperty dynProperty = null;

            if (dynamicTypeRetrive.Type.GetMetaData().IsGenericType && dynamicTypeRetrive.Type.GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>))
            {
                if ((memberAccessExpressionTreeNode.Expression as MemberExpression).Member.Name == "Key")
                {
                    if (dynamicTypeRetrive.GroupingMetaData.KeyDynamicTypeDataRetrieve != null)
                    {
                        if (memberAccessExpressionTreeNode.Nodes.Count == 0)
                            return dynamicTypeRetrive.GroupingMetaData.KeyDynamicTypeDataRetrieve;
                        else
                            return GetDynamicTypeDataRetriever(dynamicTypeRetrive.GroupingMetaData.KeyDynamicTypeDataRetrieve, memberAccessExpressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode);
                    }
                    else
                        return null;
                }
            }
            if (dynamicTypeRetrive.Properties == null)
                return null;
            dynProperty = dynamicTypeRetrive.Properties[(memberAccessExpressionTreeNode.Expression as MemberExpression).Member as PropertyInfo];
            if (dynProperty.PropertyTypeIsDynamic)
            {
                if (memberAccessExpressionTreeNode.Nodes.Count == 0)
                    return dynProperty.PropertyType;
                else
                    return GetDynamicTypeDataRetriever(dynProperty.PropertyType, memberAccessExpressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode);
            }
            else
                return null;

        }

        ///<summary>
        ///Initialize a ParameterExpressionTreeNode object
        ///</summary>
        ///<param name="expression">
        ///Defines the source linq expression.
        ///</param>
        ///<param name="parent">
        ///Defines the parent of this expresion tree node
        ///</param>
        ///<param name="expressionTranslator">
        ///Defines the expresion translator object which creates this
        ///</param>
        /// <MetaDataID>{2236eeec-a4b7-4317-9e46-5b4bfff9f752}</MetaDataID>
        public ParameterExpressionTreeNode(Expression expression, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(expression, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Parameter)
                throw new System.Exception("Wrong expression type");
        }
        /// <MetaDataID>{7e216583-3dd0-4d24-86e9-18f183d697cf}</MetaDataID>
        bool IsSubNodeOfFetchingExpression(ExpressionTreeNode expressionTreeNode)
        {
            if (expressionTreeNode.Parent is FetchingExpressionTreeNode)
                return true;
            else if (expressionTreeNode.Parent != null)
                return IsSubNodeOfFetchingExpression(expressionTreeNode.Parent);
            else
                return false;
        }
        /// <MetaDataID>{50839201-1b18-4fd8-a9a7-2b30cc33c309}</MetaDataID>
        bool IsSubNodeOfFetchingExpression()
        {
            return IsSubNodeOfFetchingExpression(this);
        }

        //public override SearchCondition GetPropertySearchCondition(PropertyInfo property)
        //{
        //    if (this.ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression] is SelectExpressionTreeNode)
        //    {
        //        if (Nodes.Count == 0)
        //            return (this.ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression] as SelectExpressionTreeNode).SelectCollection.GetPropertySearchCondition(property);
        //        else
        //            return GetPropertySearchCondition(Nodes[0] as MemberAccessExpressionTreeNode, property);

        //    }
        //    return null;
        //}

        //private SearchCondition GetPropertySearchCondition(MemberAccessExpressionTreeNode memberAccessExpressionTreeNode, PropertyInfo property)
        //{
        //    if ((memberAccessExpressionTreeNode.Expression as MemberExpression).Member == property)
        //    {
        //        if (this.ExpressionTranslator.GetDynamicTypeDataRetriever(property.DeclaringType) != null)
        //            return this.ExpressionTranslator.GetDynamicTypeDataRetriever(property.DeclaringType).Properties[property].SearchCondition;
        //    }
        //    else
        //    {
        //        if (memberAccessExpressionTreeNode.Nodes.Count > 0)
        //            return GetPropertySearchCondition(memberAccessExpressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode, property);
        //    }
        //    return null;
        //}

        /// <summary>
        /// This method retrieves all group by dynamic type dataretrievers from pararameter expresion path
        /// </summary>
        /// <returns>
        /// Returns a list with with group by dynamic type dataretrievers
        /// </returns>
        /// <MetaDataID>{1e585fab-529d-4573-823d-8cbb7ba6962f}</MetaDataID>
        internal List<IDynamicTypeDataRetrieve> GetPathGroupByDataRetrievers()
        {
            var pathGroupByDataRetrievers = new List<IDynamicTypeDataRetrieve>();
            ExpressionTreeNode parameterSource = ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression];

            IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
            if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == Expression)
                dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
            else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == Expression)
                dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
            else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == Expression)
                dynamicTypeDataRetrieve = (parameterSource as SelectManyExpressionTreeNode).DerivedCollection.DynamicTypeDataRetrieve;

            if (dynamicTypeDataRetrieve == null)
                return pathGroupByDataRetrievers;
            if (dynamicTypeDataRetrieve.Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                pathGroupByDataRetrievers.Add(dynamicTypeDataRetrieve);
            else if (dynamicTypeDataRetrieve.CollectionProviderMethodExpression.SourceCollection is GroupByExpressionTreeNode)
                pathGroupByDataRetrievers.Add((dynamicTypeDataRetrieve.CollectionProviderMethodExpression.SourceCollection as GroupByExpressionTreeNode).DynamicTypeDataRetrieve);

            if (Nodes.Count > 0 && Nodes[0] is MemberAccessExpressionTreeNode)
                GetPathGroupByDataRetrievers(dynamicTypeDataRetrieve, Nodes[0], pathGroupByDataRetrievers);
            return pathGroupByDataRetrievers;
        }

        /// <MetaDataID>{247072cd-e04b-407b-8ec7-2b9d60f5c4a5}</MetaDataID>
        private void GetPathGroupByDataRetrievers(IDynamicTypeDataRetrieve dynamicTypeDataRetrieve, ExpressionTreeNode member, List<IDynamicTypeDataRetrieve> pathGroupByDataRetrievers)
        {
            if (dynamicTypeDataRetrieve.Properties == null)
                return;

            var dynamicProperty = dynamicTypeDataRetrieve.Properties[(member.Expression as MemberExpression).Member as PropertyInfo];

            if (dynamicProperty.PropertyTypeIsDynamic)
            {
                if (dynamicProperty.PropertyType.Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                    pathGroupByDataRetrievers.Add(dynamicProperty.PropertyType);
                else if (dynamicProperty.PropertyType.CollectionProviderMethodExpression.SourceCollection is GroupByExpressionTreeNode)
                    pathGroupByDataRetrievers.Add((dynamicProperty.PropertyType.CollectionProviderMethodExpression.SourceCollection as GroupByExpressionTreeNode).DynamicTypeDataRetrieve);


                if (member.Nodes.Count > 0 && member.Nodes[0] is MemberAccessExpressionTreeNode)
                    GetPathGroupByDataRetrievers(dynamicProperty.PropertyType, member.Nodes[0], pathGroupByDataRetrievers);

            }

        }
        bool recursive;
        public SearchCondition SearchConditionA
        {
            get
            {
                if (recursive)
                    return null;

                try
                {

                    recursive = true;


                    ExpressionTreeNode parameterSource = ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression];

                    IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
                    if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == Expression)
                        dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                    else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == Expression)
                        dynamicTypeDataRetrieve = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                    else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == Expression)
                        dynamicTypeDataRetrieve = (parameterSource as SelectManyExpressionTreeNode).DerivedCollection.DynamicTypeDataRetrieve;



                    SearchCondition searchCondition = ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression].FilterDataCondition;

                    var selectManyExpression = this.ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression] as SelectManyExpressionTreeNode;

                    if (selectManyExpression != null && selectManyExpression.SelectorCollectionParameter == this.Expression)
                        searchCondition = SearchCondition.JoinSearchConditions(searchCondition, selectManyExpression.DerivedCollection.FilterDataCondition);
                    if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.GroupingMetaData != null)
                        return searchCondition;
                    if (Nodes.Count > 0 && dynamicTypeDataRetrieve != null)
                        searchCondition = JoinWithMemberSearchConditions(dynamicTypeDataRetrieve, Nodes[0] as MemberAccessExpressionTreeNode, searchCondition);

                    return searchCondition;
                }
                finally
                {
                    recursive = false;

                }
            }
        }
        /// <summary>
        /// In case where 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="leftSearchCondition"></param>
        /// <returns></returns>
        /// <MetaDataID>{df198065-55c8-4b06-a637-7c4a84d01a16}</MetaDataID>

        private SearchCondition JoinWithMemberSearchConditions(IDynamicTypeDataRetrieve dynamicTypeDataRetrieve, MemberAccessExpressionTreeNode member, SearchCondition leftSearchCondition)
        {
            //IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = this.ExpressionTranslator.GetDynamicTypeDataRetriever((member.Expression as MemberExpression).Member.DeclaringType);
            //if (dynamicTypeDataRetrieve == null)
            //    return leftSearchCondition;

            if (dynamicTypeDataRetrieve.Type.GetMetaData().IsGenericType && dynamicTypeDataRetrieve.Type.GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>))
            {
                if ((member.Expression as MemberExpression).Member.Name == "Key")
                    return SearchCondition.JoinSearchConditions(leftSearchCondition, dynamicTypeDataRetrieve.FilterDataCondition);
            }

            if (dynamicTypeDataRetrieve.Properties == null)
                return leftSearchCondition;

            var dynamicProperty = dynamicTypeDataRetrieve.Properties[(member.Expression as MemberExpression).Member as PropertyInfo];

            var searchCondition = SearchCondition.JoinSearchConditions(leftSearchCondition, dynamicProperty.FilterDataCondition);

            if (member.Nodes.Count == 0 || !dynamicProperty.PropertyTypeIsDynamic)
                return searchCondition;
            else
                return JoinWithMemberSearchConditions(dynamicProperty.PropertyType, member.Nodes[0] as MemberAccessExpressionTreeNode, searchCondition);

            //Type memberType = (member.Expression as MemberExpression).Type;
            //if (OOAdvantech.TypeHelper.IsEnumerable(memberType))
            //    memberType = OOAdvantech.TypeHelper.GetElementType(memberType);

            //IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = this.ExpressionTranslator.GetDynamicTypeDataRetriever(memberType);
            //if (dynamicTypeDataRetrieve == null)
            //    return leftSearchCondition;
            //else
            //{
            //    var searchCondition =SearchCondition.JoinSearchConditions(leftSearchCondition,dynamicTypeDataRetrieve.SourceCollectionExpression.SearchCondition);
            //    if (member.Nodes.Count == 0)
            //        return searchCondition;
            //    else
            //        return JoinWithMemberSearchConditions(member.Nodes[0] as MemberAccessExpressionTreeNode, searchCondition);
            //}

        }

        public override SearchCondition FilterDataCondition
        {
            get
            {
                return SearchConditionA;
                //    if (HeadNodeSourceCollection != null)
                //    {
                //        if (SourceCollection is IFilteredSource)
                //        {
                //            var searchCondition = SourceCollection.SearchCondition;
                //            if (searchCondition != null)
                //            {
                //                if (HeadNodeSourceCollection.SearchCondition != null)
                //                {
                //                    List<SearchFactor> searchFactors = new List<SearchFactor>();
                //                    searchFactors.Add(new SearchFactor(searchCondition));
                //                    searchFactors.Add(new SearchFactor(HeadNodeSourceCollection.SearchCondition));
                //                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                //                    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, this.ExpressionTranslator.LINQObjectQuery);
                //                    return newSearchCondition;
                //                }
                //                return searchCondition;
                //            }
                //        }
                //        else if (SourceCollection.DerivedMemberLinqQuery != null)
                //            return SourceCollection.DerivedMemberLinqQuery.QueryResult.SourceCollectionExpression.SearchCondition;

                //        return HeadNodeSourceCollection.SearchCondition;
                //    }
                //    else
                //        return null;
            }

        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sourceCollection"></param>
        ///// <param name="searchConditions"></param>
        //private void GetSearchConditionsFromParameterPath(ExpressionTreeNode sourceCollection, List<SearchCondition> searchConditions)
        //{
        //    if (Nodes.Count == 0)
        //        return;
        //    if (sourceCollection is QueryExpressions.ConstantExpressionTreeNode)
        //        return;

        //    if (sourceCollection is QueryExpressions.SelectExpressionTreeNode)
        //    {
        //        foreach (ExpressionTreeNode selectionNode in (sourceCollection as QueryExpressions.SelectExpressionTreeNode).SelectCollection.Nodes)
        //        {

        //            if (selectionNode.Alias == Nodes[0].Name)
        //            {
        //                if (selectionNode.SearchCondition != null && !searchConditions.Contains(selectionNode.SearchCondition))
        //                    searchConditions.Add(selectionNode.SearchCondition);
        //                (Nodes[0] as QueryExpressions.MemberAccessExpressionTreeNode).GetOrgSource(selectionNode, searchConditions);
        //                return;


        //            }
        //        }
        //    }
        //    //if (sourceCollection is QueryExpressions.ParameterExpressionTreeNode &&
        //    //    (sourceCollection as QueryExpressions.ParameterExpressionTreeNode).SearchCondition != null &&
        //    //    !searchConditions.Contains((sourceCollection as QueryExpressions.ParameterExpressionTreeNode).SearchCondition))
        //    //{
        //    //    searchConditions.Add((sourceCollection as QueryExpressions.ParameterExpressionTreeNode).SearchCondition);
        //    //}

        //}



        /// <summary>
        /// Defines the next member access expression treeNode
        /// </summary>
        internal MemberAccessExpressionTreeNode MemberAccess
        {
            get
            {
                if (Nodes.Count == 0)
                    return null;
                else
                    return Nodes[0] as MemberAccessExpressionTreeNode;
            }
        }

        
        /// <summary>
        /// Creates the necessary data nodes to access data of parameter
        /// </summary>
        /// <param name="dataNode">
        /// Deifines the dataNode of previous expression tree node
        /// </param>
        /// <param name="linqObjectQuery">
        /// Defines Linq Object query
        /// </param>
        /// <returns>
        /// The DataNode which retrieves the data of parameter 
        /// </returns>
        /// <MetaDataID>{4e5403d5-60eb-4459-acc6-334ae7bf34c0}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            DataNode parameterDataNode = null;
         
            #region Gets parameter source DataNode 
            ExpressionTreeNode parameterSource = ExpressionTranslator.ParameterDeclareExpression[Expression as ParameterExpression];
            if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == Expression)
                parameterDataNode = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DataNode;
            else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == Expression)
                parameterDataNode = (parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DataNode;
            else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == Expression)
                parameterDataNode = (parameterSource as SelectManyExpressionTreeNode).DerivedCollection.DataNode;
            else if (parameterSource is GroupByExpressionTreeNode && (parameterSource as GroupByExpressionTreeNode).ResultSelectorParameter== Expression)
                parameterDataNode = (parameterSource as GroupByExpressionTreeNode).SourceCollection.DataNode;
            if (parameterDataNode is GroupDataNode && Parent.DataNode is AggregateExpressionDataNode)
                parameterDataNode = RootDynamicTypeDataRetrieve.RootDataNode;
            #endregion

            if (MemberAccess != null)
            {
                if (RootDynamicTypeDataRetrieve == null || (!RootDynamicTypeDataRetrieve.IsGrouping && RootDynamicTypeDataRetrieve.Properties == null))
                {
                    #region Normal type member access
                    DataNode memberDataNode = MemberAccess.GetMemberDataNode(dataNode, linqObjectQuery );
                    DataNode = MemberAccess.BuildDataNodeTree(memberDataNode, null, linqObjectQuery);
                    #endregion
                }
                else
                {
                    DataNode memberDataNode = null;
                    if (RootDynamicTypeDataRetrieve.IsGrouping)
                    {
                        #region   System.Linq.IGrouping member access

                        if (TypeHelper.IsMemberGroupingKey((MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member))
                        {
                            memberDataNode = (RootDynamicTypeDataRetrieve.RootDataNode as GroupDataNode).KeyDataNode;
                            DataNode = MemberAccess.BuildDataNodeTree(memberDataNode, RootDynamicTypeDataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve, linqObjectQuery);

                            #region Removed code
                            //if ((RootDynamicTypeDataRetrieve.RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(MemberAccess.DataNode))
                            //{
                            //    DerivedDataNode dataNodeAgent = null;
                            //    foreach (DerivedDataNode keySubDataNode in keyDataNode.SubDataNodes)
                            //    {
                            //        if (keySubDataNode.OrgDataNode == MemberAccess.DataNode)
                            //        {
                            //            dataNodeAgent = keySubDataNode;
                            //            break;
                            //        }
                            //    }
                            //    if (dataNodeAgent == null)
                            //    {
                            //        dataNodeAgent = new DerivedDataNode(MemberAccess.DataNode);
                            //        dataNodeAgent.ParentDataNode = keyDataNode; 
                            //    }
                            //    DataNode = GroupKeyExpressionTreeNode.BuildDerivedDataNodePath(dataNodeAgent, DataNode);
                            //}

                            //if (DataNode == RootDynamicTypeDataRetrieve.RootDataNode)
                            //    DataNode = RootDynamicTypeDataRetrieve.RootDataNode.SubDataNodes[0];
                            #endregion

                        }
                        else if (TypeHelper.IsMemberGroupingItem((MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member, RootDynamicTypeDataRetrieve.Type))
                        {
                            parameterDataNode = (from derivedDataNde in RootDynamicTypeDataRetrieve.GroupingMetaData.GroupDataNode.SubDataNodes.OfType<DerivedDataNode>()
                                                 where derivedDataNde.OrgDataNode == RootDynamicTypeDataRetrieve.GroupingMetaData.GroupDataNode.GroupedDataNode
                                                 select derivedDataNde).First();

                            
                            if (RootDynamicTypeDataRetrieve.GroupingMetaData.GroupCollectionDynamicTypeDataRetrieve == null)
                            {

                                memberDataNode = MemberAccess.GetMemberDataNode(parameterDataNode, linqObjectQuery);

                                #region Removed code
                                //var netNativeMember = (MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member;
                                //MetaDataRepository.MetaObject classifierMember = TypeHelper.GetClassifierMember(declaringClassifier, netNativeMember);

                                ////RootDynamicTypeDataRetrieve.GroupingMetaData.GroupDataNode.GroupedDataNode;
                                //if (parameterDataNode.Classifier == declaringClassifier || parameterDataNode.Classifier.IsA(declaringClassifier))
                                //{

                                //    memberDataNode = parameterDataNode.SubDataNodes.Where(subDataNode => subDataNode.AssignedMetaObject == classifierMember).FirstOrDefault();

                                //    if (memberDataNode == null)
                                //    {
                                //        memberDataNode = new DataNode(linqObjectQuery as ObjectQuery, (MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member.Name, classifierMember);
                                //        memberDataNode.ParentDataNode = DerivedDataNode.GetOrgDataNode(parameterDataNode);
                                //        if (parameterDataNode is DerivedDataNode)
                                //        {
                                //            memberDataNode = new DerivedDataNode(memberDataNode);
                                //            memberDataNode.ParentDataNode = parameterDataNode;
                                //        }
                                //    }
                                //}
                                #endregion

                                DataNode = MemberAccess.BuildDataNodeTree(memberDataNode, null, linqObjectQuery);
                            }
                            else
                            {

                                memberDataNode = RootDynamicTypeDataRetrieve.GroupingMetaData.GroupCollectionDynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].SourceDataNode;
                                memberDataNode = DerivedDataNode.GetDerivedDataNodeFor(parameterDataNode as DerivedDataNode, memberDataNode);
                                DataNode = MemberAccess.BuildDataNodeTree(memberDataNode, RootDynamicTypeDataRetrieve.GroupingMetaData.GroupCollectionDynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].PropertyType, linqObjectQuery);
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        //Dynamic type member access
                        memberDataNode = RootDynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].SourceDataNode;
                        DataNode = MemberAccess.BuildDataNodeTree(memberDataNode, RootDynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].PropertyType, linqObjectQuery);
                        memberDataNode = MemberAccess.GetMemberDataNode(dataNode, linqObjectQuery);
                    }
                }
            }
            else
                DataNode = parameterDataNode;
             
            if (!string.IsNullOrEmpty(Alias))
                DataNode.Alias = Alias;
            return DataNode;


        }


        /// <summary>
        /// Defines the source collection for parameter first name of parameter path
        /// </summary>
        internal ExpressionTreeNode HeadNodeSourceCollection
        {
            get
            {
                ExpressionTreeNode parent = Parent;
                while (true)
                {
                    if (parent == null)
                        break;

                    if(parent.DynamicTypeDataRetrieve!=null&& parent.DynamicTypeDataRetrieve.Properties!=null)
                    {
                        var sourceProperty=(from property in parent.DynamicTypeDataRetrieve.Properties.Values
                         where property.PropertyName == Name && property.Type == this.Type
                         select property).FirstOrDefault();

                        if (sourceProperty != null && sourceProperty.PropertyType != null)
                            return sourceProperty.PropertyType.CollectionProviderMethodExpression;
                    }
                    if (parent is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode &&
                        this != (parent as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection &&
                        !this.IsExpressionTreeNodeAncestor((parent as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection))
                    {
                        ExpressionTreeNode sourceCollection = (parent as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
                        if (sourceCollection.Alias == Name)
                            return sourceCollection;
                        //if(sourceCollection.ReplacedExpressionTreeNode!=null&&sourceCollection.ReplacedExpressionTreeNode.Alias==Name)
                        //    return sourceCollection;

                        if (parent is QueryExpressions.SelectManyExpressionTreeNode)
                        {
                            if ((parent as QueryExpressions.SelectManyExpressionTreeNode).DerivedCollection.Alias == Name)
                                return (parent as QueryExpressions.SelectManyExpressionTreeNode).DerivedCollection;
                        }
                    }
                    if (parent is AggregateFunctionExpressionTreeNode &&
                        ((parent as AggregateFunctionExpressionTreeNode).Nodes[0] is ParameterExpressionTreeNode) &&
                        (parent as AggregateFunctionExpressionTreeNode).Nodes[0] != this)
                    {
                        return ((parent as AggregateFunctionExpressionTreeNode).Nodes[0] as ParameterExpressionTreeNode).SourceCollection;
                    }
                    if (parent.Parent == null && parent.Alias == Name)
                        return parent;
                    parent = parent.Parent;
                }
                return null;
            }
        }

        public override string ToString()
        {
            string shr = null;
            if (_FilterDataCondition != null)
                shr = "(SHRCH:" + _FilterDataCondition.GetHashCode().ToString() + ")"; ;
            //shr + "(" + GetHashCode().ToString() + ")" + NamePrefix + " : " + 
            var str = "[" + Expression.NodeType.ToString() + " '" + (Expression as ParameterExpression).Name + "' ]  " + TypeDescription; // Name;
            if (!string.IsNullOrEmpty(Alias))
                str += " as " + Alias;
            return str;
        }


        /// <summary>
        /// Defines the original source collection 
        /// some times the parameter referse to derived collection
        /// </summary>
        public ExpressionTreeNode SourceCollection
        {
            get
            {
                if (HeadNodeSourceCollection != null)
                {

                    if (Nodes.Count > 0)
                    {
                        if (HeadNodeSourceCollection is GroupByExpressionTreeNode && Nodes[0].Name == "Key")
                            return (HeadNodeSourceCollection as GroupByExpressionTreeNode).GroupKeyExpression;

                        if (HeadNodeSourceCollection.Expression is LambdaExpression && HeadNodeSourceCollection.Name == "Root")
                            return HeadNodeSourceCollection;

                        return GetSourceCollection(Nodes[0] as MemberAccessExpressionTreeNode);
                    }
                    if (HeadNodeSourceCollection is QueryExpressions.ParameterExpressionTreeNode)
                        return (HeadNodeSourceCollection as QueryExpressions.ParameterExpressionTreeNode).SourceCollection;
                    return HeadNodeSourceCollection;
                }
                return null;
            }
        }


        /// <MetaDataID>{c7fde146-0090-407b-8b78-497ac00e0aa2}</MetaDataID>
        private ExpressionTreeNode GetSourceCollection(MemberAccessExpressionTreeNode expressionTreeNode)
        {
            if (expressionTreeNode.Nodes.Count > 0)
                return GetSourceCollection(expressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode);
            else
                return expressionTreeNode.SourceCollection;
        }


        //#region IFilteredSource Members

        //public SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        //{
        //    if (Nodes.Count > 0)
        //    {
        //        ExpressionTreeNode node = Nodes[0];
        //        if (node.Expression is System.Linq.Expressions.MemberExpression && (node.Expression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0)
        //        {
        //            //searchCondition = node.DerivedMemberLinqQuery.Translator.BuildSearchCondition(searchCondition);
        //        }
        //    }
        //    return searchCondition;
        //}

        //#endregion
    }
}
