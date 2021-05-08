using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Linq.Expressions;

namespace OOAdvantech.Linq.QueryExpressions
{

    /// <MetaDataID>{0ac02ced-7d79-41f6-9dfa-eb945f481062}</MetaDataID>
    class ArithmeticExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{322ed9cc-5ca0-4dcd-bc80-568646e9b224}</MetaDataID>
        ExpressionTreeNode LeftTerm
        {
            get
            {
                return Nodes[0];
            }
        }
        /// <MetaDataID>{6b9f11b9-8f07-4fda-9dda-aec61cc7f996}</MetaDataID>
        ExpressionTreeNode RightTerm
        {
            get
            {
                return Nodes[1];
            }
        }

        /// <MetaDataID>{ca0525f1-a496-4bad-91a7-292a3a6e1d2a}</MetaDataID>
        DataNode LeftTermDataNode;
        /// <MetaDataID>{227fd174-ebd8-4900-81bb-60b5a27e21ad}</MetaDataID>
        DataNode RightTermDataNode;

        /// <MetaDataID>{a8842f07-7107-4b4b-99c7-536cdf79c35d}</MetaDataID>
        public ArithmeticExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {

            expressionTranslator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreesSimplification);
            //if (!(Expression is BinaryExpression))
            //    throw new System.Exception("Wrong expression type");
        }




        /// <MetaDataID>{6fe3c233-3c3b-442a-b242-a818f075a6d8}</MetaDataID>
        void OnDataNodeTreesSimplification(Dictionary<DataNode, DataNode> replacedDataNodes)
        {
            if (LeftTermDataNode != null)
                replacedDataNodes.TryGetValue(LeftTermDataNode, out LeftTermDataNode);

            if (RightTermDataNode != null)
                replacedDataNodes.TryGetValue(RightTermDataNode, out RightTermDataNode);
        }
        /// <MetaDataID>{d38d95ac-0dbb-47ba-a296-397546126858}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        { 
            DataNode = dataNode;
            if (Nodes[0].Expression.NodeType == ExpressionType.Parameter)
            {
                var sourceCollection = (Nodes[0] as ParameterExpressionTreeNode).SourceCollection;
                while (sourceCollection is MemberAccessExpressionTreeNode && (sourceCollection as MemberAccessExpressionTreeNode).SourceCollection.DataNode == null)
                    sourceCollection = sourceCollection.Parent;
                DataNode sourceDataNode = null;
                if (sourceCollection is MemberAccessExpressionTreeNode)
                    sourceDataNode = (sourceCollection as MemberAccessExpressionTreeNode).SourceCollection.DataNode;
                if (sourceCollection is ParameterExpressionTreeNode)
                    sourceDataNode = (sourceCollection as ParameterExpressionTreeNode).SourceCollection.DataNode;
                if (sourceCollection.Nodes.Count > 0 && sourceDataNode != null)
                    LeftTermDataNode = sourceCollection.BuildDataNodeTree(sourceDataNode, linqObjectQuery);
                else
                {
                    if (sourceDataNode == null)
                        sourceDataNode = dataNode.HeaderDataNode;
                    while (sourceDataNode.Type == DataNode.DataNodeType.Group)
                        sourceDataNode = (sourceDataNode as GroupDataNode).GroupedDataNodeRoot;
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(sourceDataNode.HeaderDataNode, Nodes[0].Name);
                    if (aliasDataNode != null)
                        LeftTermDataNode = Nodes[0].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                }
            }
            else if (Nodes[0] is ArithmeticExpressionTreeNode)
            {
                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }

            if (Nodes[1].Expression.NodeType == ExpressionType.Parameter)
            {

                var sourceCollection = (Nodes[1] as ParameterExpressionTreeNode).SourceCollection;
                while (sourceCollection is MemberAccessExpressionTreeNode && (sourceCollection as MemberAccessExpressionTreeNode).SourceCollection.DataNode == null)
                    sourceCollection = sourceCollection.Parent;
                DataNode sourceDataNode = null;
                if (sourceCollection is MemberAccessExpressionTreeNode)
                    sourceDataNode = (sourceCollection as MemberAccessExpressionTreeNode).SourceCollection.DataNode;
                if (sourceCollection is ParameterExpressionTreeNode)
                    sourceDataNode = (sourceCollection as ParameterExpressionTreeNode).SourceCollection.DataNode;
                if (sourceCollection.Nodes.Count > 0 && sourceDataNode != null)
                    RightTermDataNode = sourceCollection.BuildDataNodeTree(sourceDataNode, linqObjectQuery);
                else
                {
                    if (sourceDataNode == null)
                        sourceDataNode = dataNode.HeaderDataNode;
                    while (sourceDataNode.Type == DataNode.DataNodeType.Group)
                        sourceDataNode = (sourceDataNode as GroupDataNode).GroupedDataNodeRoot;
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(sourceDataNode.HeaderDataNode, Nodes[1].Name);
                    if (aliasDataNode != null)
                        RightTermDataNode = Nodes[1].BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                }

            }
            else if (Nodes[1] is ArithmeticExpressionTreeNode)
            {
                dataNode = Nodes[1].BuildDataNodeTree(dataNode, linqObjectQuery);
            }

            return dataNode;
        }
        //BinaryExpression NextBinaryExpression

        

        /// <MetaDataID>{4852b13e-4fa3-486e-9584-ce70bdfb92fd}</MetaDataID>
        internal protected virtual void BuildArithmeticExpression(CompositeArithmeticExpression arithmeticExpression)
        {
            switch (Expression.NodeType)
            {


                case ExpressionType.Add:
                    {
                        arithmeticExpression.Operator = ArithmeticOperator.Add;
                        break;
                    }
                case ExpressionType.Subtract:
                    {
                        arithmeticExpression.Operator = ArithmeticOperator.Subtract;
                        break;
                    }

                case ExpressionType.Multiply:
                    {
                        arithmeticExpression.Operator = ArithmeticOperator.Multiply;
                        break;
                    }

                case ExpressionType.Divide:
                    {
                        arithmeticExpression.Operator = ArithmeticOperator.Divide;
                        break;
                    }
                default :
                    throw new System.Exception("Invalid arithmetic expresion");
            }
            if (LeftTerm is ArithmeticExpressionTreeNode)
            {
                CompositeArithmeticExpression leftArithmeticExpression = new CompositeArithmeticExpression(LeftTerm.Expression.Type);
                arithmeticExpression.Left = leftArithmeticExpression;
                (LeftTerm as ArithmeticExpressionTreeNode).BuildArithmeticExpression(leftArithmeticExpression);
            }
            else if (LeftTerm is ParameterExpressionTreeNode)
            {
                arithmeticExpression.Left = new ScalarFromData(LeftTermDataNode);
            }
            else if (LeftTerm is ConstantExpressionTreeNode)
            {
                arithmeticExpression.Left = new ScalarFromLiteral((LeftTerm.Expression as ConstantExpression).Value);
            }
            if (RightTerm is ArithmeticExpressionTreeNode)
            {
                CompositeArithmeticExpression rightArithmeticExpression = new CompositeArithmeticExpression();
                arithmeticExpression.Right = rightArithmeticExpression;
                (RightTerm as ArithmeticExpressionTreeNode).BuildArithmeticExpression(rightArithmeticExpression);
            }
            else if (RightTerm is ParameterExpressionTreeNode)
            {
                arithmeticExpression.Right = new ScalarFromData(RightTermDataNode);
            }
            else if (RightTerm is ConstantExpressionTreeNode)
            {
                arithmeticExpression.Right = new ScalarFromLiteral((RightTerm.Expression as ConstantExpression).Value);
            }


            
       
        }

     
   
    }
}
