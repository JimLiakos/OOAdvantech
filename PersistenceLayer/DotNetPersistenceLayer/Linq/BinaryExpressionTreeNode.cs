using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    public class BinaryExpressionTreeNode : ExpressionTreeNode
    {

        DataNode LeftTermDataNode;
        DataNode RightTermDataNode;

        public BinaryExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent, objectQuery)
        {
            if (!(Expression is BinaryExpression))
                throw new System.Exception("Wrong expression type");
        }
        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[0].Name);
                if (aliasDataNode != null)
                {
                    LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                    LeftTermDataNode.ParticipateInWereClause = true;

                }
            }
            else if (Nodes[0].Expression is BinaryExpression)
            {
                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
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
            else if (Nodes[1].Expression is BinaryExpression)
            {
                dataNode = Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
        }
        //BinaryExpression NextBinaryExpression


        internal void BuildSearchFactor(SearchFactor searchFactor)
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    {
                        if (searchFactor.SearchCondition == null)
                        {
                            SearchTerm searchTerm = new SearchTerm();
                            

                            List<SearchTerm> searchTerms = new List<SearchTerm>();
                            searchTerms.Add(searchTerm);
                            searchFactor.SearchCondition = new SearchCondition(searchTerms,ObjectQuery);

                            SearchFactor firstSearchFactor = new SearchFactor();
                            SearchFactor secondSearchFactor = new SearchFactor();
                            searchTerm.AddSearchFactor(firstSearchFactor);
                            searchTerm.AddSearchFactor(secondSearchFactor);


                            (Nodes[0] as BinaryExpressionTreeNode).BuildSearchFactor(firstSearchFactor);
                            (Nodes[1] as BinaryExpressionTreeNode).BuildSearchFactor(secondSearchFactor);

                        }

                        break;
                    }
                case ExpressionType.OrElse:
                    {
                        if (searchFactor.SearchCondition == null)
                        {
                            SearchTerm firstSearchTerm=new SearchTerm();
                            SearchTerm secondSearchTerm = new SearchTerm();

                            List<SearchTerm> searchTerms=new List<SearchTerm>();
                            searchTerms.Add(firstSearchTerm);
                            searchTerms.Add(secondSearchTerm);
                            searchFactor.SearchCondition = new SearchCondition(searchTerms, ObjectQuery);
                            (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm);
                            (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm);
                            
                        }
                        break;
                    }
                case ExpressionType.NotEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual,null,ObjectQuery,false,searchFactor);
                        break;
                    }
                case ExpressionType.Equal:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ObjectQuery, false, searchFactor);

                        break;
                    }
                case ExpressionType.LessThan:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.LessThanOrEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThan:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThanOrEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                default:
                    break;
            }
                
        }

        private void BuildSearchTerm(SearchTerm searchTerm)
        {
            SearchFactor searchFactor = new SearchFactor();
            searchTerm.AddSearchFactor(searchFactor);
            switch (Expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    {

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
                            searchFactor.SearchCondition = new SearchCondition(searchTerms, ObjectQuery);
                            (Nodes[0] as BinaryExpressionTreeNode).BuildSearchTerm(firstSearchTerm);
                            (Nodes[1] as BinaryExpressionTreeNode).BuildSearchTerm(secondSearchTerm);

                        }
                        break;
                    }
                case ExpressionType.NotEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.NotEqual, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.Equal:
                    {

                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.LessThan:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThan, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.LessThanOrEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.LessThanEqual, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThan:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThan, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                case ExpressionType.GreaterThanOrEqual:
                    {
                        searchFactor.Criterion = new Criterion(Criterion.ComparisonType.GreaterThanEqual, GetComparisonTerm(), ObjectQuery, false, searchFactor);
                        break;
                    }
                default:
                    break;
            }


        }

        private ComparisonTerm[] GetComparisonTerm()
        {
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode, ObjectQuery);
            if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                comparisonTerms[1] = new ObjectAttributeComparisonTerm(RightTermDataNode, ObjectQuery);

            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode, ObjectQuery);
            if (RightTermDataNode != null && RightTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[1] = new ObjectComparisonTerm(RightTermDataNode, ObjectQuery);
            if (LeftTermDataNode == null)
            {
                ConstantExpression constandExpression = Nodes[0].Expression as ConstantExpression;
                string parameterName = "p" + constandExpression.GetHashCode().ToString();
                ObjectQuery.Parameters.Add(parameterName, constandExpression.Value);
                comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ObjectQuery);
            }

            if (RightTermDataNode == null)
            {
                ConstantExpression constandExpression = Nodes[1].Expression as ConstantExpression;
                string parameterName = "p" + constandExpression.GetHashCode().ToString();
                ObjectQuery.Parameters.Add(parameterName, constandExpression.Value);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ObjectQuery);
            }


                


            return comparisonTerms;
        }
    }
}
