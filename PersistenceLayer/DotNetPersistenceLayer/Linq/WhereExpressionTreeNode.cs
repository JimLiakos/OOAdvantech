using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    public class WhereExpressionTreeNode : MethodCallAsCollectionSourceExpressionTreeNode
    {
        public WhereExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent, objectQuery)
        {
        }
        BinaryExpressionTreeNode BinaryExpression
        {
            get
            {
                return Nodes[1] as BinaryExpressionTreeNode;
            }
        }

        internal override SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        {
            if (searchCondition == null)
            {
                SearchTerm searchTerm = new SearchTerm();

                List<SearchTerm> searchTerms = new List<SearchTerm>();
                searchTerms.Add(searchTerm);
                searchCondition = new SearchCondition(searchTerms,ObjectQuery);
            }


            SearchFactor searchFactor = GetSearchFactor();
            searchCondition.SearchTerms[0].AddSearchFactor(searchFactor);


            
            //if(searchCondition==null)
            //{
            //    OOAdvantech.Collections.Generic.List<SearchTerm> searchTerms= new OOAdvantech.Collections.Generic.List<SearchTerm>();
 
            //    searchCondition=new SearchCondition(

            if (SourceCollection is MethodCallAsCollectionSourceExpressionTreeNode)
                return (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            else
                return searchCondition;
        }

        private SearchFactor GetSearchFactor()
        {
            SearchFactor SearchFactor=new SearchFactor();
            BinaryExpression.BuildSearchFactor(SearchFactor);
            return SearchFactor;
            
        }



        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {
            dataNode=Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            DataNode = dataNode;

            if (Nodes[2] .Expression.NodeType == ExpressionType.Parameter)
            {
                DataNode.Alias = Nodes[2].Name;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
                DataNode.Temporary = true;
            }


            Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);

            dataNode = DataNode;
            return dataNode;
        }

        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                return Nodes[0];
            }
        }
        internal override string SourceCollectionIteratedObjectName
        {
            get
            {
                return Nodes[2].Name;
            }
        }
    }
}
