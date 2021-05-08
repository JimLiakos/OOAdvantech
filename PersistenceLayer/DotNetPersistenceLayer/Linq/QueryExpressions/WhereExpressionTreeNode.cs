using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{5e11a3d1-e282-4ef9-b838-31010f5b23f4}</MetaDataID>
    class WhereExpressionTreeNode : MethodCallAsCollectionProviderExpressionTreeNode
    {
        /// <MetaDataID>{00480388-fa1e-4a50-8e79-f55b6c88d95c}</MetaDataID>
        public WhereExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }
        /// <MetaDataID>{4a66e676-58e0-4550-937e-a3f7ace267b9}</MetaDataID>
        ExpressionTreeNode BinaryExpression
        {
            get
            {
                return Nodes[1];
            }
        }

        public override SearchCondition FilterDataCondition
        {
            get
            {


                return SearchCondition.JoinSearchConditions(_FilterDataCondition, SourceCollection.FilterDataCondition);
                //return SearchCondition.JoinSearchConditions(AncestorsFilterDataCondition, SearchCondition.JoinSearchConditions(_FilterDataCondition, SourceCollection.FilterDataCondition));

                //var searchCondition = SourceCollection.SearchCondition;

                //if (searchCondition != null)
                //{
                //    List<SearchFactor> searchFactors = new List<SearchFactor>();
                //    searchFactors.Add(new SearchFactor(searchCondition));
                //    searchFactors.Add(new SearchFactor(_SearchCondition));
                //    SearchTerm searchTerm = new SearchTerm(searchFactors);
                //    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, this.ExpressionTranslator.LINQObjectQuery);
                //    return newSearchCondition;

                //}

                //return _SearchCondition;
            }
        }
        /// <MetaDataID>{aef3e742-395d-47e1-8bf8-8572ef1b12b8}</MetaDataID>
        public override void BuildDataFilter()
        {

            if (SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
                (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();

            List<SearchTerm> searchTerms = new List<SearchTerm>() { new SearchTerm() };
            _FilterDataCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
            SearchFactor searchFactor = GetSearchFactor();
            _FilterDataCondition.SearchTerms[0].AddSearchFactor(searchFactor);

        }

        /// <MetaDataID>{a963b4d4-268d-467d-83aa-8c7e60f4a783}</MetaDataID>
        private SearchFactor GetSearchFactor()
        {
            SearchFactor searchFactor = new SearchFactor();
            if (!(BinaryExpression is BinaryExpressionTreeNode))
            {
                DataNode searchExpressionDataNode = null;
                if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = null;
                    foreach (DataNode dataNode in ExpressionTranslator.RootPaths)
                    {
                        aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode, Nodes[0].Name);
                        if (aliasDataNode == dataNode)
                            aliasDataNode = null;
                        if (aliasDataNode != null)
                            break;
                    }
                    if (aliasDataNode != null)
                        searchExpressionDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                    else
                        searchExpressionDataNode = Nodes[1].DataNode;

                    searchExpressionDataNode.ParticipateInWereClause = true;
                    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                    comparisonTerms[0] = new ObjectAttributeComparisonTerm(searchExpressionDataNode, /*OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[1]),*/ ExpressionTranslator.LINQObjectQuery);
                    object constantValue = true;
                    string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
                    comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                    searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, ExpressionTranslator.LINQObjectQuery, true, searchFactor);

                }
            }
            else
                (BinaryExpression as BinaryExpressionTreeNode).BuildSearchFactor(searchFactor, true);
            return searchFactor;

        }




        /// <MetaDataID>{75df0a6b-3587-45f5-b477-09e69792b179}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
   
           DataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            if (!string.IsNullOrEmpty(Alias))
                DataNode.Alias = Alias;

            //if (Nodes.Count>2&& Nodes[2].Expression.NodeType == ExpressionType.Parameter)
            //{
            //    if (DataNode.Name == null)
            //        DataNode.Name = DataNode.Alias;
            //    if (SourceCollection.Expression.NodeType == ExpressionType.Call)
            //        DataNode.Temporary = true;
            //}
            //else
            //{

            //}
            BinaryExpression.BuildDataNodeTree(DataNode, linqObjectQuery);
            if (!(SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode))
            {
                ReferenceDataNode = SourceCollection.DataNode.ParentDataNode;
                if (SourceCollection.DataNode.ParentDataNode != null)
                {
                    if (SourceCollection is ParameterExpressionTreeNode)
                    {
                        if ((SourceCollection as ParameterExpressionTreeNode).RootDynamicTypeDataRetrieve != null)
                            ReferenceDataNode = (SourceCollection as ParameterExpressionTreeNode).RootDynamicTypeDataRetrieve.RootDataNode;
                    }

                    
                }
                ExpressionTreeNode expressionTreeNode = SourceCollection;
                while (expressionTreeNode.Nodes.Count > 0)
                    expressionTreeNode = expressionTreeNode.Nodes[0];
                DataNode queryResultDataNode = expressionTreeNode.DataNode;


                //_DynamicTypeDataRetrieve = SourceCollection.DynamicTypeDataRetrieve;
                //if (_DynamicTypeDataRetrieve == null)
                {
                    Type elementType = TypeHelper.GetElementType(expressionTreeNode.Expression.Type);
                    _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(elementType, linqObjectQuery, DataNode, queryResultDataNode, this);
                }
                //else
                //{
                //    if (_DynamicTypeDataRetrieve.Properties != null)
                //        _DynamicTypeDataRetrieve = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(_DynamicTypeDataRetrieve.Type), linqObjectQuery, DataNode, queryResultDataNode, this) as IDynamicTypeDataRetrieve;
                //    else
                //        _DynamicTypeDataRetrieve = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(_DynamicTypeDataRetrieve.Type), linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.Properties, this) as IDynamicTypeDataRetrieve;
                //}



                //BridgeEnumerator.ParticipateInSelectClass();

                if (Parent.Name == "Root")
                    (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;

                Dictionary<DataNode, DataNode> replacedDataNodes = new Dictionary<DataNode, DataNode>();
                ExpressionTranslator.MergeDataNodeTree(DataNode, replacedDataNodes);

                if (ReferenceDataNode != null)
                {
                    bool sourceColectionDataNodeTemporary = SourceCollection.DataNode.Temporary;
                    if (Parent.Name != "Root")
                        SourceCollection.DataNode.Temporary = false;
                    ExpressionTranslator.RemoveTemporaryNodes(ref _DataNode, replacedDataNodes);
                    SourceCollection.DataNode.Temporary = sourceColectionDataNodeTemporary;
                }
                else
                    ExpressionTranslator.RemoveTemporaryNodes(ref _DataNode, replacedDataNodes);


                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);


            }
            else
            {


                _DynamicTypeDataRetrieve = (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve;
                if (Parent.Name == "Root")
                {
                    if(_DynamicTypeDataRetrieve.MemberDataNode!=null&& _DynamicTypeDataRetrieve.Properties==null)
                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(_DynamicTypeDataRetrieve.Type, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.MemberDataNode, this);
                    else
                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(_DynamicTypeDataRetrieve.Type, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.Properties, this);
                    dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                    DataNode = SourceCollection.DataNode;
                    SourceCollection.DataNode.HeaderDataNode.ParentDataNode = dataNode;
                    dataNode.Name = "Root";
                    dataNode.Temporary = true;

                    (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;
                    Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);

                }

            }



            return DataNode;
        }





        /// <MetaDataID>{97e52646-805d-44d4-a040-a11eee6178f6}</MetaDataID>
        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                if (string.IsNullOrEmpty(Nodes[0].NamePrefix))
                    Nodes[0].NamePrefix = "SourceCollection";
                return Nodes[0];
            }
        }


    }
}
