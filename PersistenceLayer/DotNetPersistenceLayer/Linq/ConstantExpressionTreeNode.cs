using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    public class ConstantExpressionTreeNode:ExpressionTreeNode
    {
        public ConstantExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent, objectQuery)
        {
            if (this.Expression.NodeType != ExpressionType.Constant)
                throw new System.Exception("Wrong expression type");


        }
        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {

            dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
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
