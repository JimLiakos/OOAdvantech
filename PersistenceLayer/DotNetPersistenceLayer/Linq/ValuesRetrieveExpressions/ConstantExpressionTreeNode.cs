using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.ValuesRetrieveExpressions
{

    /// <MetaDataID>{13572cc2-2fa5-4ed7-b2e6-0a21472e2fa3}</MetaDataID>
    class ConstantExpressionTreeNode:ExpressionTreeNode
    {
        /// <MetaDataID>{8e1e51cc-6c86-43c1-9d65-e0e84b6cd6bc}</MetaDataID>
        public ConstantExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Constant)
                throw new System.Exception("Wrong expression type");


        }
        /// <MetaDataID>{63220452-bb68-4690-8724-30885b99534b}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            DataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            return DataNode;
            dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
            dataNode.Name = ((this.Expression as ConstantExpression).Value as System.Linq.IQueryable).ElementType.Name;
            DataNode = dataNode;

            if (Parent.Nodes.Count > 3 && Parent.Nodes[3].Expression.NodeType == ExpressionType.Parameter)
            {
                DataNode.Alias = Parent.Nodes[3].Name;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
            }
            if (Parent.Nodes.Count == 3 && Parent.Nodes[2].Expression.NodeType == ExpressionType.Parameter)
            {
                DataNode.Alias = Parent.Nodes[2].Name;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
            }

            DataNode.AssignedMetaObject = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(TypeHelper.GetElementType((Expression as ConstantExpression).Type));
            return dataNode;

        }
    }
}
