using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{d3a8b5aa-631e-412a-9fd9-e6f08dacf117}</MetaDataID>
    class ContainsAnyAllExpressionTreeNode:BinaryExpressionTreeNode
    {
        /// <MetaDataID>{afe52efb-5072-42f8-af6c-7f04da8240b2}</MetaDataID>
        bool ContainsAny ;
        /// <MetaDataID>{54405a1e-21fd-46c7-9b44-1cd3efde6443}</MetaDataID>
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
        /// <MetaDataID>{900785fe-5342-496d-8e7d-17af897ae2b4}</MetaDataID>
        public ContainsAnyAllExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if ((exp as MethodCallExpression).Method.Name == "ContainsAny")
                ContainsAny = true;
            else
                ContainsAny = false;
        }

        /// <MetaDataID>{d631b413-6fd4-4199-aff2-a56465762f3b}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (Nodes[0].Expression.NodeType == ExpressionType.Constant)
            {
                if (ContainsAny)
                    throw new System.Exception("'ContainsAny' expression supported only for storage collections");
                else
                    throw new System.Exception("'ContainsAll' expression supported only for storage collections");

            }
            return base.BuildDataNodeTree(dataNode, linqObjectQuery);
        }
        /// <MetaDataID>{419006b3-dd56-49ab-8da0-07ff7202b40a}</MetaDataID>
        protected internal override void BuildSearchTerm(SearchTerm searchTerm,bool constrain)
        {
            SearchFactor searchFactor = new SearchFactor();
            searchTerm.AddSearchFactor(searchFactor);
            if (ContainsAny)
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.ContainsAny, ContainsAnyAllDataNode, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
            else
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.ContainsAll, ContainsAnyAllDataNode, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
            ContainsAnyAllDataNode.ParticipateInWereClause = true;
        }
        /// <MetaDataID>{84dd024b-c5ea-468e-b016-d119ac5e0ad0}</MetaDataID>
        protected internal override void BuildSearchFactor(SearchFactor searchFactor,bool constrain)
        {
            if(ContainsAny)
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.ContainsAny, ContainsAnyAllDataNode, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
            else
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.ContainsAll, ContainsAnyAllDataNode, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);

            ContainsAnyAllDataNode.ParticipateInWereClause = true;
        }


        /// <MetaDataID>{9f998ccc-113a-4b34-a651-9c2c08d4c607}</MetaDataID>
        private ComparisonTerm[] GetComparisonTerm()
        {
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            comparisonTerms[0] = new ObjectComparisonTerm(FilteredDataNode,/* OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]),*/ ExpressionTranslator.LINQObjectQuery);
         
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




        /// <MetaDataID>{02dcbc17-8b8e-4208-a389-5ab0b5d55998}</MetaDataID>
        public DataNode FilteredDataNode
        {
            get
            {
                return DataNode;
            }
        }
        /// <MetaDataID>{ce0de14a-2f9f-4793-a85b-5c9c3252a123}</MetaDataID>
        DataNode _ContainsAnyAllDataNode;
        /// <MetaDataID>{751a84d1-73ab-4e17-8c4e-0fde63037075}</MetaDataID>
        public DataNode ContainsAnyAllDataNode
        {
            get
            {

                if (_ContainsAnyAllDataNode == null)
                {
                    ExpressionTreeNode treeNode = Nodes[0];
                    while (treeNode.Nodes.Count > 0)
                        treeNode = treeNode.Nodes[0];
                    _ContainsAnyAllDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(DataNode, treeNode.Name);
                    if (_ContainsAnyAllDataNode == null && treeNode.DataNode != null && treeNode.Name == treeNode.DataNode.Name)
                        _ContainsAnyAllDataNode = treeNode.DataNode;
                }


                return _ContainsAnyAllDataNode;

            }
        }



    }
}
