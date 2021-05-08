using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{

    /// <MetaDataID>{261d0436-4381-4f7e-bc6e-c309a714efa5}</MetaDataID>
    class OfTypeExpressionTreeNode : MethodCallAsCollectionProviderExpressionTreeNode
    {
        public OfTypeExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }


        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode BuildDataNodeTree(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            DataNode = dataNode;
            var ofTypeClisifier=  OOAdvantech.MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(Expression.Type));
            if(DataNode.Classifier==null|| !DataNode.Classifier.IsA(ofTypeClisifier))
                DataNode.Classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(Expression.Type));
            DataNode.Alias = Alias;

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
                Type elementType = TypeHelper.GetElementType(expressionTreeNode.Expression.Type);
                _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(elementType, linqObjectQuery, DataNode, queryResultDataNode, this);


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
                    if (_DynamicTypeDataRetrieve.MemberDataNode != null && _DynamicTypeDataRetrieve.Properties == null)
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
            return dataNode;
        }

        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                if (string.IsNullOrEmpty(Nodes[0].NamePrefix))
                    Nodes[0].NamePrefix = "SourceCollection";
                return Nodes[0];
            }
        }
        public override void BuildDataFilter()
        {
            SearchCondition searchCondition = null;
            if (searchCondition == null)
            {
                SearchTerm searchTerm = new SearchTerm();
                List<SearchTerm> searchTerms = new List<SearchTerm>();
                searchTerms.Add(searchTerm);
                searchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                _FilterDataCondition = searchCondition;
            }
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            DataNode LeftTermDataNode = DataNode;
            //var LeftTermDataNodeRoute = OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]);

            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || LeftTermDataNode is AggregateExpressionDataNode)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode, /*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode, /*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

            object constantValue = TypeHelper.GetElementType(Expression.Type);
            string parameterName = "p" + Nodes[0].GetHashCode().ToString();
            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
            comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);

            SearchFactor searchFactor = new SearchFactor();
            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.TypeIs, comparisonTerms, ExpressionTranslator.LINQObjectQuery, true, searchFactor);
            searchCondition.SearchTerms[0].AddSearchFactor(searchFactor);
            int tt = 0;

        }


        public override SearchCondition FilterDataCondition
        {
            get
            {
                return SearchCondition.JoinSearchConditions(_FilterDataCondition, SourceCollection.FilterDataCondition);
            }
        }

    }
}
