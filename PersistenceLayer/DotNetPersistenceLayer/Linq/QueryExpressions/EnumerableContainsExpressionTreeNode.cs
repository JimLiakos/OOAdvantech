using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Linq.Expressions;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{806364ea-7ada-4c39-a6ac-700ac2e9e09e}</MetaDataID>
    class EnumerableContainsExpressionTreeNode : BinaryExpressionTreeNode
    {

        /// <MetaDataID>{fe9de02c-bed4-4f84-8e9f-12e29166b251}</MetaDataID>
        internal override DataNode DataNode
        {
            get
            {
                return base.DataNode;
            }
            set
            {
                base.DataNode = value;
            }
        }
        /// <MetaDataID>{8a32bda9-753d-4c64-b230-41cf9f6adfa6}</MetaDataID>
        public EnumerableContainsExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }
        /// <MetaDataID>{02551630-5add-48af-b5e0-512a658916e8}</MetaDataID>
        System.Collections.IEnumerable EnumerableCollection;

        object Item;
        /// <MetaDataID>{95224820-6642-4c57-9836-74092d11c97a}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            //if(Nodes[0].Expression.NodeType==ExpressionType.Constant)
            //    throw new System.Exception("'Contains' expression supported only for storage collections"); 
            _FilteredDataNode = base.BuildDataNodeTree(dataNode, linqObjectQuery);

            DataNode = dataNode;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[0].Name);
                if (aliasDataNode != null)
                {
                    _FilteredDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                }
            }
            else if (Nodes[0] is BinaryExpressionTreeNode || (Nodes[0] is LikeExpressionTreeNode))
            {
                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            else if (Nodes[0].Expression.NodeType == ExpressionType.Constant)
            {
                EnumerableCollection = (Nodes[0] as Linq.QueryExpressions.ConstantExpressionTreeNode).Value as System.Collections.IEnumerable;
               // FilteredDataNodeRoute = OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[1]);
            }

            if (Nodes[1].Expression.NodeType == ExpressionType.Constant)
            {
                EnumerableCollection = (Nodes[1] as Linq.QueryExpressions.ConstantExpressionTreeNode).Value as System.Collections.IEnumerable;
              //  FilteredDataNodeRoute = OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]);
                if (EnumerableCollection == null)
                    Item = (Nodes[1] as Linq.QueryExpressions.ConstantExpressionTreeNode).Value;
            }
            else if (Nodes[1] is BinaryExpressionTreeNode || (Nodes[1] is LikeExpressionTreeNode))
            {
                dataNode = Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
            
        }
        /// <MetaDataID>{cff86671-3986-4e5b-83f8-33f500a8ab22}</MetaDataID>
        protected internal override void BuildSearchTerm(SearchTerm searchTerm,bool constrain)
        {
            int i=0;
            SearchFactor searchFactor = new SearchFactor();
            //TODO να γραφτεί testcase όπου η collection ειναι άδεια
            searchTerm.AddSearchFactor(searchFactor);
            List<SearchTerm> searchTerms=new List<SearchTerm>();
            if (EnumerableCollection != null)
            {
                foreach (object constantValue in EnumerableCollection)
                {
                    i++;
                    SearchTerm collectionObjectSearchTerm = new SearchTerm();
                    searchTerms.Add(collectionObjectSearchTerm);
                    SearchFactor collectionObjectSearchFactor = new SearchFactor();
                    collectionObjectSearchTerm.AddSearchFactor(collectionObjectSearchFactor);

                    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                    if (FilteredDataNode.Type == DataNode.DataNodeType.Object)
                        comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
                    if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

                    ParameterComparisonTerm paramTerm = new ParameterComparisonTerm("param_" + i.ToString() + "_" + Nodes[1].GetHashCode().ToString(), ExpressionTranslator.LINQObjectQuery);
                    ExpressionTranslator.LINQObjectQuery.Parameters.Add(paramTerm.ParameterName, constantValue);
                    comparisonTerms[1] = paramTerm;
                    collectionObjectSearchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, FilteredDataNode, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, collectionObjectSearchFactor);
                }
            }
            else
            {
                SearchTerm collectionObjectSearchTerm = new SearchTerm();
                searchTerms.Add(collectionObjectSearchTerm);
                SearchFactor collectionObjectSearchFactor = new SearchFactor();
                collectionObjectSearchTerm.AddSearchFactor(collectionObjectSearchFactor);

                ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                if (FilteredDataNode.Type == DataNode.DataNodeType.Object)
                    comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
                if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

                ParameterComparisonTerm paramTerm = new ParameterComparisonTerm("param_" + i.ToString() + "_" + Nodes[1].GetHashCode().ToString(), ExpressionTranslator.LINQObjectQuery);
                ExpressionTranslator.LINQObjectQuery.Parameters.Add(paramTerm.ParameterName, Item);
                comparisonTerms[1] = paramTerm;
                collectionObjectSearchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, FilteredDataNode, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, collectionObjectSearchFactor);
    

            }
            searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
            FilteredDataNode.ParticipateInWereClause = true;
        }
        /// <MetaDataID>{34f6817f-12b3-4efd-a571-47f0e0febbc1}</MetaDataID>
        protected internal override void BuildSearchFactor(SearchFactor searchFactor,bool constrain)
        {
            int i = 0;
            List<SearchTerm> searchTerms = new List<SearchTerm>();
            //TODO να γραφτεί testcase όπου η collection ειναι άδεια
            if (EnumerableCollection != null)
            {
                //if (searchFactor.IsNotExpression)
                //{
                //    SearchTerm collectionObjectSearchTerm = new SearchTerm();
                //    searchTerms = new List<SearchTerm>() { collectionObjectSearchTerm };
                //    searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                //    foreach (object constantValue in EnumerableCollection)
                //    {
                //        i++;
                //        SearchFactor collectionObjectSearchFactor = new SearchFactor();
                //        collectionObjectSearchTerm.AddSearchFactor(collectionObjectSearchFactor);

                //        ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                //        if (FilteredDataNode.Type == DataNode.DataNodeType.Object)
                //            comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode, ExpressionTranslator.LINQObjectQuery);
                //        if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                //            comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode, ExpressionTranslator.LINQObjectQuery);

                //        ParameterComparisonTerm paramTerm = new ParameterComparisonTerm("param_" + i.ToString() + "_" + Nodes[1].Expression.GetHashCode().ToString(), ExpressionTranslator.LINQObjectQuery);
                //        ExpressionTranslator.LINQObjectQuery.Parameters.Add(paramTerm.ParameterName, constantValue);
                //        comparisonTerms[1] = paramTerm;
                //        collectionObjectSearchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, FilteredDataNode, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, collectionObjectSearchFactor);
                //    }
                //}                    
                //else
                {
                    foreach (object constantValue in EnumerableCollection)
                    {
                        i++;
                        SearchTerm collectionObjectSearchTerm = new SearchTerm();
                        searchTerms.Add(collectionObjectSearchTerm);
                        SearchFactor collectionObjectSearchFactor = new SearchFactor();
                        collectionObjectSearchTerm.AddSearchFactor(collectionObjectSearchFactor);

                        ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                        if (FilteredDataNode.Type == DataNode.DataNodeType.Object)
                            comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode, /*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
                        if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

                        ParameterComparisonTerm paramTerm = new ParameterComparisonTerm("param_" + i.ToString() + "_" + Nodes[1].GetHashCode().ToString(), ExpressionTranslator.LINQObjectQuery);
                        ExpressionTranslator.LINQObjectQuery.Parameters.Add(paramTerm.ParameterName, constantValue);
                        comparisonTerms[1] = paramTerm;
                        collectionObjectSearchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, FilteredDataNode, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, collectionObjectSearchFactor);
                    }
                }
                

            }
            else
            {
                SearchTerm collectionObjectSearchTerm = new SearchTerm();
                searchTerms.Add(collectionObjectSearchTerm);
                SearchFactor collectionObjectSearchFactor = new SearchFactor();
                collectionObjectSearchTerm.AddSearchFactor(collectionObjectSearchFactor);

                ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                if (FilteredDataNode.Type == DataNode.DataNodeType.Object)
                    comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode, /*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
                if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

                ParameterComparisonTerm paramTerm = new ParameterComparisonTerm("param_" + i.ToString() + "_" + Nodes[1].GetHashCode().ToString(), ExpressionTranslator.LINQObjectQuery);
                ExpressionTranslator.LINQObjectQuery.Parameters.Add(paramTerm.ParameterName, Item);
                comparisonTerms[1] = paramTerm;
                collectionObjectSearchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, FilteredDataNode, comparisonTerms, ExpressionTranslator.LINQObjectQuery, constrain, collectionObjectSearchFactor);
            }
            searchFactor.SearchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
            FilteredDataNode.ParticipateInWereClause = true;
        }


        /// <MetaDataID>{5fc989f6-ea29-4704-a2be-17dee7a79c4b}</MetaDataID>
        private ComparisonTerm[] GetComparisonTerm()
        {
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            if(FilteredDataNode.Type==DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(FilteredDataNode,/*FilteredDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

            
            if (Nodes[1] is ConstantExpressionTreeNode)
            {
                object constantValue = (Nodes[1] as ConstantExpressionTreeNode).Value;
                string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
            }
            else if (Nodes[1] is ObjectMethodCallExpressionTreeNode)
            {
                object constantValue = (Nodes[1] as ObjectMethodCallExpressionTreeNode).Value;
                string parameterName = "p" + Nodes[1].GetHashCode().ToString();
                ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
            }
            return comparisonTerms;
        }


       // Stack<DataNode> FilteredDataNodeRoute;

        /// <MetaDataID>{184ba19e-7b6c-4540-8c82-9b75c9335114}</MetaDataID>
        DataNode _FilteredDataNode;

        /// <MetaDataID>{fe33b2a9-bdea-4e4d-8b57-98780e28daa8}</MetaDataID>
        public DataNode FilteredDataNode
        {
            get
            {
                return _FilteredDataNode;
            }
        }

    }
}
