using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{


    /// <MetaDataID>{b89f8e25-1438-4327-aa6a-4c7bd1f2f075}</MetaDataID>
    class LikeExpressionTreeNode :BinaryExpressionTreeNode
    {

        ///// <MetaDataID>{0fe00607-a426-471a-a7a8-c3785c6f1924}</MetaDataID>
        //DataNode LeftTermDataNode;
        ///// <MetaDataID>{11cba208-14a2-40f3-ae5b-1b2573556fe7}</MetaDataID>
        //DataNode RightTermDataNode;




        /// <MetaDataID>{ca330481-c206-4ddf-91cf-21db30d1c876}</MetaDataID>
        public LikeExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }
        /// <MetaDataID>{d221f5db-f3fd-4f3b-ad55-af2b15f835ce}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, Nodes[0].Name);
                if (aliasDataNode == null)
                {
                    ExpressionTreeNode parent = Parent;
                    while (parent.Parent.Name != "Root")
                        parent = parent.Parent;
                    aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(parent.DataNode, Nodes[0].Name);
                }


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

        /// <MetaDataID>{880e3bf1-f1cd-4aaf-b439-4e445b94e00e}</MetaDataID>
        protected internal override void BuildSearchFactor(SearchFactor searchFactor,bool constrain)
        {
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
            try
            {
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Like, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);
            }
            catch (System.Exception error)
            {
                throw;
            }
            if (LeftTermDataNode != null)
                LeftTermDataNode.ParticipateInWereClause = true;
            if (RightTermDataNode != null)
                RightTermDataNode.ParticipateInWereClause = true;


        }
        /// <MetaDataID>{21ca5cf2-e549-4909-908c-392ba08f84a7}</MetaDataID>
        protected internal override void BuildSearchTerm(SearchTerm searchTerm, bool constrain)
        {
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
            SearchFactor searchFactor = new SearchFactor();
            searchTerm.AddSearchFactor(searchFactor);
            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Like, GetComparisonTerm(), ExpressionTranslator.LINQObjectQuery, constrain, searchFactor);

            if (LeftTermDataNode != null)
                LeftTermDataNode.ParticipateInWereClause = true;
            if (RightTermDataNode != null)
                RightTermDataNode.ParticipateInWereClause = true;

        }


        /// <MetaDataID>{f7cb1aa8-ca25-4f0a-b935-72a49f55d6d5}</MetaDataID>
        private ComparisonTerm[] GetComparisonTerm()
        {
            if (LeftTermDataNode != null && LeftTermDataNode.AssignedMetaObject == null)
            {
                string errors = null;
                (LeftTermDataNode.ObjectQuery as LINQStorageObjectQuery).BuildDataNodeTree(ref errors);
            }
            if (RightTermDataNode != null && RightTermDataNode.AssignedMetaObject == null)
            {
                string errors = null;
                (RightTermDataNode.ObjectQuery as LINQStorageObjectQuery).BuildDataNodeTree(ref errors);
            }

            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            if (LeftTermDataNode != null && DerivedDataNode.GetOrgDataNode(LeftTermDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode,/*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (RightTermDataNode != null && DerivedDataNode.GetOrgDataNode(RightTermDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                comparisonTerms[1] = new ObjectAttributeComparisonTerm(RightTermDataNode,/*RightTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);

            if (LeftTermDataNode != null && DerivedDataNode.GetOrgDataNode(LeftTermDataNode).Type == DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode,/*LeftTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (RightTermDataNode != null && DerivedDataNode.GetOrgDataNode(RightTermDataNode).Type == DataNode.DataNodeType.Object)
                comparisonTerms[1] = new ObjectComparisonTerm(RightTermDataNode,/*RightTermDataNodeRoute,*/ ExpressionTranslator.LINQObjectQuery);
            if (LeftTermDataNode == null)
            {
                if (Nodes[0] is ConstantExpressionTreeNode)
                {
                    object constantValue = (Nodes[0] as ConstantExpressionTreeNode).Value;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
                    comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                }
                else if (Nodes[0] is ObjectMethodCallExpressionTreeNode)
                {
                    object constantValue = (Nodes[0] as ObjectMethodCallExpressionTreeNode).Value;
                    string parameterName = "p" + Nodes[0].GetHashCode().ToString();
                    ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
                    comparisonTerms[0] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);
                }
            }

            if (RightTermDataNode == null)
            {
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

            }






            return comparisonTerms;
        }
    }
}
