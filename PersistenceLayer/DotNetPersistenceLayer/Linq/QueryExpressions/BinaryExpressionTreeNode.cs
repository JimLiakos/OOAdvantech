using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Linq.Translators;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{2d775677-6233-45b5-a44e-29d23514d0db}</MetaDataID>
    class BinaryExpressionTreeNode : ExpressionTreeNode
    {

        /// <MetaDataID>{b8fd5015-3d45-4ca5-b05a-f5b9246b0c11}</MetaDataID>
        DataNode _LeftTermDataNode;

        /// <MetaDataID>{83cf56a2-f884-4933-956a-2c2a474d512d}</MetaDataID>
        protected DataNode LeftTermDataNode
        {
            get
            {
                return _LeftTermDataNode;
            }
            set
            {
                _LeftTermDataNode = value;
            }
        }
        //Stack<DataNode> _LeftTermDataNodeRoute;
        //protected Stack<DataNode> LeftTermDataNodeRoute
        //{
        //    get
        //    {
        //        if (_LeftTermDataNodeRoute == null)
        //            _LeftTermDataNodeRoute = QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]);
        //        return _LeftTermDataNodeRoute;
        //    }
        //}


        //Stack<DataNode> _RightTermDataNodeRoute;
        //protected Stack<DataNode> RightTermDataNodeRoute
        //{
        //    get
        //    {
        //        if (_RightTermDataNodeRoute == null)
        //            _RightTermDataNodeRoute = QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[1]);
        //        return _RightTermDataNodeRoute;
        //    }
        //}


        /// <MetaDataID>{03f7dabb-811d-439f-bc4b-c700710ef3ad}</MetaDataID>
        DataNode _RightTermDataNode;

        protected DataNode RightTermDataNode
        {
            get
            {
                return _RightTermDataNode;
            }
            set
            {
                _RightTermDataNode = value;
            }
        }

        /// <MetaDataID>{1ee0c071-359f-49c5-802d-beb43022fd8c}</MetaDataID>
        public BinaryExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
 
            expressionTranslator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreesSimplification);
            //if (!(Expression is BinaryExpression))
            //    throw new System.Exception("Wrong expression type");
        }

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
            DataNode = dataNode;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[0].Name);
                if (aliasDataNode != null)
                {
                    LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                    LeftTermDataNode.ParticipateInWereClause = true;
                }
            }
            else if (Nodes[0] is BinaryExpressionTreeNode || (Nodes[0] is LikeExpressionTreeNode) || (Nodes[0] is AggregateFunctionExpressionTreeNode))
            {
                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
                LeftTermDataNode = dataNode;
                LeftTermDataNode.ParticipateInWereClause = true;
            }

            if (Nodes.Count > 1)
            {
                if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[1].Name);
                    if (aliasDataNode != null)
                    {
                        RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                        RightTermDataNode.ParticipateInWereClause = true;
                        return RightTermDataNode;
                    }
                }
                else if (Nodes[1] is BinaryExpressionTreeNode || (Nodes[1] is LikeExpressionTreeNode))
                {
                    dataNode = Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);
                }
            }

            return dataNode;
        }
        //BinaryExpression NextBinaryExpression


        /// <MetaDataID>{159e1059-5602-42b6-a8b9-9173f83bc7f8}</MetaDataID>
        internal protected virtual void BuildSearchFactor(SearchFactor searchFactor, bool constrain)
        {


            searchFactor.IsNotExpression = NotExpression;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
                foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
                {
                    aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[0].Name);
                    if (aliasDataNode != null)
                        break;
                }
                if (aliasDataNode != null)
                {
                    LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                    LeftTermDataNode.ParticipateInWereClause = true;

                }
            }

            if (Nodes.Count == 1 && Nodes[0] is ParameterExpressionTreeNode)
            {
                if (Expression.NodeType == ExpressionType.TypeIs)
                {
                    searchFactor.Criterion = new Criterion(Criterion.ComparisonType.TypeIs, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                }
                else
                {
                    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                    comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode,/*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
                    object constantValue = true;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                    comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                    searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                }


            }
            else if (Nodes.Count == 1 && Nodes[0] is BinaryExpressionTreeNode)
            {
                (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(searchFactor, constrain);
            }
            else
            {
                if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
                    foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
                    {
                        aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[1].Name);
                        if (aliasDataNode != null)
                            break;
                    }

                    if (aliasDataNode != null)
                    {
                        RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                        RightTermDataNode.ParticipateInWereClause = true;
                    }
                }


                switch (Expression.NodeType)
                {
                    case ExpressionType.AndAlso:
                        {
                            if (searchFactor.SearchCondition == null)
                            {
                                SearchTerm searchTerm = new SearchTerm();


                                List<SearchTerm> searchTerms = new List<SearchTerm>();
                                searchTerms.Add(searchTerm);
                                searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);

                                SearchFactor firstSearchFactor = new SearchFactor();
                                SearchFactor secondSearchFactor = new SearchFactor();
                                searchTerm.AddSearchFactor(firstSearchFactor);
                                searchTerm.AddSearchFactor(secondSearchFactor);
                                
                                if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
                                    BuildBooleanParameterSearchFactor(Nodes[0] as ParameterExpressionTreeNode, firstSearchFactor, constrain );
                                else
                                    (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(firstSearchFactor, constrain);

                                if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
                                    BuildBooleanParameterSearchFactor(Nodes[1] as ParameterExpressionTreeNode,  secondSearchFactor, constrain);
                                else
                                    (Nodes[1] as BinaryExpressionTreeNode).BuildSearchFactor(secondSearchFactor, constrain);

                            }

                            break;
                        }
                    case ExpressionType.OrElse:
                        {
                            if (searchFactor.SearchCondition == null)
                            {
                                SearchTerm firstSearchTerm = new SearchTerm();
                                SearchTerm secondSearchTerm = new SearchTerm();

                                List<SearchTerm> searchTerms = new List<SearchTerm>();
                                searchTerms.Add(firstSearchTerm);
                                searchTerms.Add(secondSearchTerm);
                                searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                                //if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
                                //    BuildBooleanParameterSearchFactor(firstSearchTerm, false);
                                if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
                                {
                                    SearchFactor newSearchFactor = new SearchFactor();
                                    firstSearchTerm.AddSearchFactor(newSearchFactor);
                                    BuildBooleanParameterSearchFactor(Nodes[0] as ParameterExpressionTreeNode, newSearchFactor, false);
                                }
                                else
                                    (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm, false);

                                if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
                                {
                                    SearchFactor newSearchFactor = new SearchFactor();
                                    secondSearchTerm.AddSearchFactor(newSearchFactor);
                                    BuildBooleanParameterSearchFactor(Nodes[1] as ParameterExpressionTreeNode, newSearchFactor, false);
                                }
                                else
                                    (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm, false);

                            }
                            break;
                        }
                    case ExpressionType.NotEqual:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                            break;
                        }
                    case ExpressionType.Equal:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);

                            break;
                        }
                    case ExpressionType.LessThan:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                            break;
                        }
                    case ExpressionType.LessThanOrEqual:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                            break;
                        }
                    case ExpressionType.GreaterThan:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                            break;
                        }
                    case ExpressionType.GreaterThanOrEqual:
                        {
                            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                            break;
                        }
                    default:
                        break;
                }
            }


            if (LeftTermDataNode != null)
                LeftTermDataNode.ParticipateInWereClause = true;
            if (RightTermDataNode != null)
                RightTermDataNode.ParticipateInWereClause = true;


        }

        /// <MetaDataID>{8d736b9c-1787-4001-b9bf-1894a82afca4}</MetaDataID>
        private void BuildBooleanParameterSearchFactor(ParameterExpressionTreeNode parameterTreeNode, SearchFactor searchFactor, bool constrain)
        {
            DataNode searchExpressionDataNode;
            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
            foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
            {
                aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, parameterTreeNode.Name);
                if (aliasDataNode != null)
                    break;
            }
            if (aliasDataNode != null)
                searchExpressionDataNode = parameterTreeNode.BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
            else
                searchExpressionDataNode = parameterTreeNode.DataNode;

            searchExpressionDataNode.ParticipateInWereClause = true;
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            comparisonTerms[0] = new ObjectAttributeComparisonTerm(searchExpressionDataNode, /*OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[1]),*/ ExpressionTranslator.LINQObjectQuery);
            object constantValue = true;
            string parameterName = "p" + parameterTreeNode.GetHashCode().ToString();
            ExpressionTranslator.LINQObjectQuery.Parameters[parameterName] = constantValue;
            comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);

        }

        /// <MetaDataID>{994abbb4-c852-44f8-9476-d595f97a4e01}</MetaDataID>
        internal protected virtual void BuildSearchTerm(SearchTerm searchTerm, bool constrain)
        {
            searchTerm.IsNotExpression = NotExpression;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
                foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
                {
                    aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[0].Name);
                    if (aliasDataNode != null)
                        break;
                }
                if (aliasDataNode != null)
                {
                    LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                    LeftTermDataNode.ParticipateInWereClause = true;

                }
            }


            if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
                foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
                {
                    aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[1].Name);
                    if (aliasDataNode != null)
                        break;
                }

                if (aliasDataNode != null)
                {
                    RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                    RightTermDataNode.ParticipateInWereClause = true;
                }
            }
            switch (Expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    {
                        SearchFactor firstSearchFactor = new SearchFactor();
                        SearchFactor secondSearchFactor = new SearchFactor();


                        //List<SearchTerm> searchTerms = new List<SearchTerm>();
                        //searchTerms.Add(firstSearchTerm);
                        //searchTerms.Add(secondSearchTerm);
                        //searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                        (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(firstSearchFactor, constrain);
                        (Nodes[1] as BinaryExpressionTreeNode).BuildSearchFactor(secondSearchFactor, constrain);
                        searchTerm.AddSearchFactor(firstSearchFactor);
                        searchTerm.AddSearchFactor(secondSearchFactor);
                        string tyrt = searchTerm.ToString();



                        break;
                    }
                case ExpressionType.OrElse:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);

                        if (searchFactor.SearchCondition == null)
                        {
                            SearchTerm firstSearchTerm = new SearchTerm();
                            SearchTerm secondSearchTerm = new SearchTerm();

                            List<SearchTerm> searchTerms = new List<SearchTerm>();
                            searchTerms.Add(firstSearchTerm);
                            searchTerms.Add(secondSearchTerm);
                            searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                            (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm, false);
                            (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm, false);

                        }
                        break;
                    }
                case ExpressionType.NotEqual:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                case ExpressionType.Equal:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);

                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                case ExpressionType.LessThan:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                case ExpressionType.LessThanOrEqual:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThan:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThanOrEqual:
                    {
                        SearchFactor searchFactor = new SearchFactor();
                        searchTerm.AddSearchFactor(searchFactor);
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
                        break;
                    }
                default:
                    break;
            }

            if (LeftTermDataNode != null)
                LeftTermDataNode.ParticipateInWereClause = true;
            if (RightTermDataNode != null)
                RightTermDataNode.ParticipateInWereClause = true;



        }

        /// <MetaDataID>{674c0116-1157-4051-9772-ac554adb6741}</MetaDataID>
        private ComparisonTerm[] GetComparisonTerm()
        {
            if (LeftTermDataNode != null && LeftTermDataNode.AssignedMetaObject == null)
            {
                string errors = null;
                if(LeftTermDataNode.ObjectQuery is LINQStorageObjectQuery)
                    (LeftTermDataNode.ObjectQuery as LINQStorageObjectQuery).BuildDataNodeTree(ref errors);
                //if (LeftTermDataNode.ObjectQuery is LinqQueryOnRootObject)
                //    (LeftTermDataNode.ObjectQuery as LinqQueryOnRootObject).BuildDataNodeTree(ref errors);

            }

            if (RightTermDataNode != null && RightTermDataNode.AssignedMetaObject == null)
            {
                string errors = null;
                if(RightTermDataNode.ObjectQuery is LINQStorageObjectQuery)
                    (RightTermDataNode.ObjectQuery as LINQStorageObjectQuery).BuildDataNodeTree(ref errors);

                //if (RightTermDataNode.ObjectQuery is LinqQueryOnRootObject)
                //    (RightTermDataNode.ObjectQuery as LinqQueryOnRootObject).BuildDataNodeTree(ref errors);

            }
             
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];

            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || LeftTermDataNode is AggregateExpressionDataNode || LeftTermDataNode is DerivedDataNode)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode,/*LeftTermDataNodeRoute ,*/ ExpressionTranslator.LINQObjectQuery);

            if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || RightTermDataNode is AggregateExpressionDataNode || RightTermDataNode is DerivedDataNode)
                comparisonTerms[1] = new ObjectAttributeComparisonTerm(RightTermDataNode,/*RightTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode,/*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[1] = new ObjectComparisonTerm(RightTermDataNode,/*RightTermDataNodeRoute,*/ExpressionTranslator.LINQObjectQuery);
            if (LeftTermDataNode == null)
            {
                if (Nodes[0] is ConstantExpressionTreeNode)
                {
                    ConstantExpression constandExpression = Nodes[0].Expression as ConstantExpression;
                    object constantValue = (Nodes[0] as ConstantExpressionTreeNode).Value;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                    comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                }
                else if (Nodes[0] is ObjectMethodCallExpressionTreeNode)
                {
                    object constantValue = (Nodes[0] as ObjectMethodCallExpressionTreeNode).Value;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                    comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                }
                else if (Nodes[0] is MemberAccessExpressionTreeNode)
                {
                    object constantValue = (Nodes[0] as MemberAccessExpressionTreeNode).Value;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]=constantValue;
                    comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                }

            }

            if (RightTermDataNode == null)
            {
                if (Expression.NodeType == ExpressionType.TypeIs)
                {

                    object constantValue = (Expression as System.Linq.Expressions.TypeBinaryExpression).TypeOperand;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                    comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);

                }
                else
                {
                    if (Nodes[1] is ConstantExpressionTreeNode)
                    {
                        object constantValue = (Nodes[1] as ConstantExpressionTreeNode).Value;
                        string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                        ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                        comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                    }
                    else if (Nodes[1] is ObjectMethodCallExpressionTreeNode)
                    {
                        object constantValue = (Nodes[1] as ObjectMethodCallExpressionTreeNode).Value;
                        string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                        ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                        comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                    }
                    else if (Nodes[1] is MemberAccessExpressionTreeNode)
                    {
                        object constantValue = (Nodes[1] as MemberAccessExpressionTreeNode).Value;
                        string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                        ExpressionTranslator.LINQObjectQuery.Parameters[parameterName]= constantValue;
                        comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);

                    }
                }

            }

            return comparisonTerms;
        }

 
        /// <MetaDataID>{aee6b7b6-a6f6-407b-aaa0-6af2011714c3}</MetaDataID>
        internal bool NotExpression = false;
    }
}
