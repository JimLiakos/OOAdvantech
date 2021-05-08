using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    public class ParameterExpressionTreeNode : ExpressionTreeNode
    {
        public ParameterExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent, objectQuery)
        {
            if (this.Expression.NodeType != ExpressionType.Parameter)
                throw new System.Exception("Wrong expression type");

        }
        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {

            if (Nodes.Count > 0 && Nodes[0].Expression.NodeType == ExpressionType.MemberAccess)
            {
                string name = Nodes[0].Name;
                bool exist = false;
                foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.Name == name)
                    {
                        exist = true;
                        dataNode = subDataNode;
                        break;
                    }
                }
                if (!exist)
                {
                    //Type type = TypeHelper.GetElementType(((Nodes[0] as MyTreeNode).Expression as MemberExpression).Type);
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                    subDataNode.Name = name;
                    subDataNode.ParentDataNode = dataNode;
                    if (dataNode.Temporary)
                        subDataNode.Temporary = true;
                    dataNode = subDataNode;
                    //  DataNode = dataNode;
                }


                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
        }
    }
}
