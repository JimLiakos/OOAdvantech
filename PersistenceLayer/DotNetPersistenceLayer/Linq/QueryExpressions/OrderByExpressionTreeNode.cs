using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{


    /// <MetaDataID>{2ad80d14-dad9-402a-8ec7-20f4be2a8ee7}</MetaDataID>
    class OrderByExpressionTreeNode : MethodCallAsCollectionProviderExpressionTreeNode, IFilteredSource
    {

        public override SearchCondition FilterDataCondition
        {
            get
            {
                return SourceCollection.FilterDataCondition;

            }
        }

        /// <MetaDataID>{b8fd5015-3d45-4ca5-b05a-f5b9246b0c11}</MetaDataID>
        DataNode LeftTermDataNode;
        /// <MetaDataID>{03f7dabb-811d-439f-bc4b-c700710ef3ad}</MetaDataID>
        DataNode RightTermDataNode;

        /// <MetaDataID>{1ee0c071-359f-49c5-802d-beb43022fd8c}</MetaDataID>
        public OrderByExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator, OrderByType orderByType)
            : base(exp, parent, expressionTranslator)
        {
            OrderByType = orderByType;
            expressionTranslator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreesSimplification);
            //if (!(Expression is BinaryExpression))
            //    throw new System.Exception("Wrong expression type");
        }
        OrderByType OrderByType;

        /// <MetaDataID>{1fe5ae06-d5f1-45b8-85b9-fb9bdce7a71f}</MetaDataID>
        void OnDataNodeTreesSimplification(Dictionary<DataNode, DataNode> replacedDataNodes)
        {
            if (LeftTermDataNode != null)
            {
                DataNode leftTermDataNode = null;
                if (replacedDataNodes.TryGetValue(LeftTermDataNode, out leftTermDataNode))
                    LeftTermDataNode = leftTermDataNode;
            }
            if (RightTermDataNode != null)
            {
                DataNode rightTermDataNode = null;
                if (replacedDataNodes.TryGetValue(RightTermDataNode, out rightTermDataNode))
                    RightTermDataNode = rightTermDataNode;
            }
        } 
        /// <MetaDataID>{a544ba56-4d10-4ba2-8508-f4e24f45d65f}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            DataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            DataNode orderByDataNode = Nodes[1].BuildDataNodeTree(DataNode, linqObjectQuery);
            DataNode.OrderByDataNodes.Add(orderByDataNode);
            orderByDataNode.OrderBy = OrderByType;

            if (!(SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode))
            {

               

                ExpressionTreeNode expressionTreeNode = SourceCollection;
                while (expressionTreeNode.Nodes.Count > 0)
                    expressionTreeNode = expressionTreeNode.Nodes[0];
                DataNode queryResultDataNode = expressionTreeNode.DataNode;

                Type elementType = TypeHelper.GetElementType(expressionTreeNode.Expression.Type);
                _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(elementType, linqObjectQuery, DataNode, queryResultDataNode, this);

                _DynamicTypeDataRetrieve.OrderBy = new DataOrderBy();

                OrderByField orderByField = new OrderByField();
                orderByField.DataNode = orderByDataNode;
                orderByField.OrderByType = OrderByType;
                _DynamicTypeDataRetrieve.OrderBy.AddField(orderByField);


                //_DynamicTypeDataRetrieve.OrderByDataNodes.AddRange(DataNode.OrderByDataNodes);


                if (Parent.Name == "Root")
                    (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;
            }
            else
            {
                _DynamicTypeDataRetrieve = (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve;

                if (_DynamicTypeDataRetrieve.MemberDataNode != null && _DynamicTypeDataRetrieve.Properties == null)
                    _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(_DynamicTypeDataRetrieve.Type, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.MemberDataNode, this);
                else
                    _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(_DynamicTypeDataRetrieve.Type, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.Properties, this);


                _DynamicTypeDataRetrieve.OrderBy = new DataOrderBy();

                OrderByField orderByField = new OrderByField();
                orderByField.DataNode = orderByDataNode;
                orderByField.OrderByType = OrderByType;
                _DynamicTypeDataRetrieve.OrderBy.AddField(orderByField);


                //_DynamicTypeDataRetrieve.OrderBy = OrderByType;
                //_DynamicTypeDataRetrieve.OrderByDataNodes.AddRange(DataNode.OrderByDataNodes);
                if (Parent.Name == "Root")
                {
                    dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                    DataNode = SourceCollection.DataNode;
                    SourceCollection.DataNode.HeaderDataNode.ParentDataNode = dataNode;
                    dataNode.Name = "Root";
                    dataNode.Temporary = true;

                    (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;
                }
                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);
            }


            return DataNode;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[0].Name);
                if (aliasDataNode != null)
                {
                    LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                }
            }
            else if (Nodes[0] is BinaryExpressionTreeNode || (Nodes[0] is LikeExpressionTreeNode) || (Nodes[0] is AggregateFunctionExpressionTreeNode))
            {
                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
                LeftTermDataNode = dataNode;
            }

            if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[1].Name);
                if (aliasDataNode != null)
                {
                    RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                    RightTermDataNode.ParticipateInWereClause = true;
                }
            }
            else if (Nodes[1] is BinaryExpressionTreeNode || (Nodes[1] is LikeExpressionTreeNode))
            {
                dataNode = Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);
            }

            return dataNode;
        }
        //BinaryExpression NextBinaryExpression


        ///// <MetaDataID>{159e1059-5602-42b6-a8b9-9173f83bc7f8}</MetaDataID>
        //internal protected virtual void BuildSearchFactor(SearchFactor searchFactor,bool constrain)
        //{


        //    searchFactor.IsNotExpression = NotExpression;
        //    if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
        //    {
        //        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
        //        foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
        //        {
        //            aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[0].Name);
        //            if (aliasDataNode != null)
        //                break;
        //        }
        //        if (aliasDataNode != null)
        //        {
        //            LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery);
        //            LeftTermDataNode.ParticipateInWereClause = true;

        //        }
        //    }


        //    if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
        //    {
        //        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
        //        foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
        //        {
        //            aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[1].Name);
        //            if (aliasDataNode != null)
        //                break;
        //        }

        //        if (aliasDataNode != null)
        //        {
        //            RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery);
        //            RightTermDataNode.ParticipateInWereClause = true;
        //        }
        //    }

        //    switch (Expression.NodeType)
        //    {
        //        case ExpressionType.AndAlso:
        //            {
        //                if (searchFactor.SearchCondition == null)
        //                {
        //                    SearchTerm searchTerm = new SearchTerm();


        //                    List<SearchTerm> searchTerms = new List<SearchTerm>();
        //                    searchTerms.Add(searchTerm);
        //                    searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);

        //                    SearchFactor firstSearchFactor = new SearchFactor();
        //                    SearchFactor secondSearchFactor = new SearchFactor();
        //                    searchTerm.AddSearchFactor(firstSearchFactor);
        //                    searchTerm.AddSearchFactor(secondSearchFactor);


        //                    (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(firstSearchFactor,constrain);
        //                    (Nodes[1] as BinaryExpressionTreeNode).BuildSearchFactor(secondSearchFactor,constrain);

        //                }

        //                break;
        //            }
        //        case ExpressionType.OrElse:
        //            {
        //                if (searchFactor.SearchCondition == null)
        //                {
        //                    SearchTerm firstSearchTerm = new SearchTerm();
        //                    SearchTerm secondSearchTerm = new SearchTerm();

        //                    List<SearchTerm> searchTerms = new List<SearchTerm>();
        //                    searchTerms.Add(firstSearchTerm);
        //                    searchTerms.Add(secondSearchTerm);
        //                    searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
        //                    (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm,false);
        //                    (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm,false);

        //                }
        //                break;
        //            }
        //        case ExpressionType.NotEqual:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, null, ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.Equal:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);

        //                break;
        //            }
        //        case ExpressionType.LessThan:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.LessThanOrEqual:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.GreaterThan:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.GreaterThanOrEqual:
        //            {
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        default:
        //            break;
        //    }


        //    if (LeftTermDataNode != null)
        //        LeftTermDataNode.ParticipateInWereClause = true;
        //    if (RightTermDataNode != null)
        //        RightTermDataNode.ParticipateInWereClause = true;


        //}

        ///// <MetaDataID>{994abbb4-c852-44f8-9476-d595f97a4e01}</MetaDataID>
        //internal protected virtual void BuildSearchTerm(SearchTerm searchTerm,bool constrain)
        //{
        //    searchTerm.IsNotExpression = NotExpression;
        //    if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
        //    {
        //        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
        //        foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
        //        {
        //            aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[0].Name);
        //            if (aliasDataNode != null)
        //                break;
        //        }
        //        if (aliasDataNode != null)
        //        {
        //            LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery);
        //            LeftTermDataNode.ParticipateInWereClause = true;

        //        }
        //    }


        //    if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
        //    {
        //        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
        //        foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
        //        {
        //            aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[1].Name);
        //            if (aliasDataNode != null)
        //                break;
        //        }

        //        if (aliasDataNode != null)
        //        {
        //            RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery);
        //            RightTermDataNode.ParticipateInWereClause = true;
        //        }
        //    }
        //    switch (Expression.NodeType)
        //    {
        //        case ExpressionType.AndAlso:
        //            {
        //                SearchFactor firstSearchFactor = new SearchFactor();
        //                SearchFactor secondSearchFactor = new SearchFactor();


        //                //List<SearchTerm> searchTerms = new List<SearchTerm>();
        //                //searchTerms.Add(firstSearchTerm);
        //                //searchTerms.Add(secondSearchTerm);
        //                //searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
        //                (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(firstSearchFactor, constrain);
        //                (Nodes[1] as BinaryExpressionTreeNode).BuildSearchFactor(secondSearchFactor, constrain);
        //                searchTerm.AddSearchFactor(firstSearchFactor);
        //                searchTerm.AddSearchFactor(secondSearchFactor);
        //                string tyrt = searchTerm.ToString();



        //                break;
        //            }
        //        case ExpressionType.OrElse:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);

        //                if (searchFactor.SearchCondition == null)
        //                {
        //                    SearchTerm firstSearchTerm = new SearchTerm();
        //                    SearchTerm secondSearchTerm = new SearchTerm();

        //                    List<SearchTerm> searchTerms = new List<SearchTerm>();
        //                    searchTerms.Add(firstSearchTerm);
        //                    searchTerms.Add(secondSearchTerm);
        //                    searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
        //                    (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm,false);
        //                    (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm,false);

        //                }
        //                break;
        //            }
        //        case ExpressionType.NotEqual:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.Equal:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);

        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.LessThan:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.LessThanOrEqual:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.GreaterThan:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        case ExpressionType.GreaterThanOrEqual:
        //            {
        //                SearchFactor searchFactor = new SearchFactor();
        //                searchTerm.AddSearchFactor(searchFactor);
        //                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
        //                break;
        //            }
        //        default:
        //            break;
        //    }

        //    if (LeftTermDataNode != null)
        //        LeftTermDataNode.ParticipateInWereClause = true;
        //    if (RightTermDataNode != null)
        //        RightTermDataNode.ParticipateInWereClause = true;



        //}

        ///// <MetaDataID>{674c0116-1157-4051-9772-ac554adb6741}</MetaDataID>
        //private ComparisonTerm[] GetComparisonTerm()
        //{
        //    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
        //    if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || LeftTermDataNode is AggregateExpressionDataNode)
        //        comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode, ExpressionTranslator.LINQObjectQuery);
        //    if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || RightTermDataNode is AggregateExpressionDataNode)
        //        comparisonTerms[1] = new ObjectAttributeComparisonTerm(RightTermDataNode, ExpressionTranslator.LINQObjectQuery);

        //    if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.Object)
        //        comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode, ExpressionTranslator.LINQObjectQuery);
        //    if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.Object)
        //        comparisonTerms[1] = new ObjectComparisonTerm(RightTermDataNode, ExpressionTranslator.LINQObjectQuery);
        //    if (LeftTermDataNode == null)
        //    {
        //        if (Nodes[0] is ConstantExpressionTreeNode)
        //        {
        //            ConstantExpression constandExpression = Nodes[0].Expression as ConstantExpression;
        //            object constantValue = (Nodes[0] as ConstantExpressionTreeNode).Value;
        //            string parameterName = "p" + Nodes[0].GetHashCode().ToString();
        //            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
        //            comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
        //        }
        //        else if (Nodes[0] is ObjectMethodCallExpressionTreeNode)
        //        {
        //            object constantValue = (Nodes[0] as ObjectMethodCallExpressionTreeNode).Value;
        //            string parameterName = "p" + Nodes[0].GetHashCode().ToString();
        //            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
        //            comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
        //        }

        //    }

        //    if (RightTermDataNode == null)
        //    {
        //        if (Nodes[1] is ConstantExpressionTreeNode)
        //        {
        //            object constantValue = (Nodes[1] as ConstantExpressionTreeNode).Value;
        //            string parameterName = "p" + Nodes[1].GetHashCode().ToString();
        //            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
        //            comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
        //        }
        //        else if (Nodes[1] is ObjectMethodCallExpressionTreeNode)
        //        {
        //            object constantValue = (Nodes[1] as ObjectMethodCallExpressionTreeNode).Value;
        //            string parameterName = "p" + Nodes[1].GetHashCode().ToString();
        //            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
        //            comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
        //        }

        //    }

        //    return comparisonTerms;
        //}

        ///// <MetaDataID>{aee6b7b6-a6f6-407b-aaa0-6af2011714c3}</MetaDataID>
        //internal bool NotExpression = false;

        internal override ExpressionTreeNode SourceCollection
        {
            get
            {

                if (string.IsNullOrEmpty(Nodes[0].NamePrefix))
                    Nodes[0].NamePrefix = "SourceCollection";
                return Nodes[0];
            }
        }



        #region IFilteredSource Members

        public override void BuildDataFilter()
        {

            ExpressionTreeNode sourceCollection = null;
            if (SourceCollection is ParameterExpressionTreeNode)
                sourceCollection = (SourceCollection as ParameterExpressionTreeNode).HeadNodeSourceCollection;
            else
                sourceCollection = SourceCollection;

            if(sourceCollection!=null&& sourceCollection.DynamicTypeDataRetrieve!=null)
            {
                var sourceCollectionOrderByFilter = sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;

                if (sourceCollectionOrderByFilter != null)
                    _DynamicTypeDataRetrieve.OrderByFilter.CombineWith(sourceCollectionOrderByFilter);
            }

            SearchCondition searchCondition = null;
            if (SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
            {
                if (Parent.Name == "Root")
                {
                    (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
                    //DataNode.AddSearchCondition(searchCondition);
                    return;
                }
                else if (Parent.Expression != null && Parent.Expression.NodeType == ExpressionType.New && !(SourceCollection is GroupByExpressionTreeNode))
                {
                    (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
                    // DataNode.AddSearchCondition(searchCondition);
                    //return DataNode.SearchCondition;

                    return;
                }
                else
                {
                    (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();

                    return;
                }
            }
            if (SourceCollection is ParameterExpressionTreeNode)
            {

                //ExpressionTreeNode source = (SourceCollection as ParameterExpressionTreeNode).OrgSourceCollection;

                //if (source is MethodCallAsCollectionSourceExpressionTreeNode)
                //    searchCondition = (source as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);

                return
                    ;
            }

            else
            {
                //SearchCondition = searchCondition;
                return;
            }

        }



        #endregion
    }
}
