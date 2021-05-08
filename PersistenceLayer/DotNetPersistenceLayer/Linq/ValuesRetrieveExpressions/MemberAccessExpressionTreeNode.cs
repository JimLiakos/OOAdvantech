using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.ValuesRetrieveExpressions
{
    /// <MetaDataID>{d3962349-151e-4735-9018-441a95928b4b}</MetaDataID>
    class MemberAccessExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{f87ab160-73c5-4643-836e-430ad52fd8c8}</MetaDataID>
        public MemberAccessExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.MemberAccess)
                throw new System.Exception("Wrong expression type");
        }
        /// <MetaDataID>{ff81f898-ccdd-48a7-9d9c-8c70b21834a9}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (Nodes.Count > 0)
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
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery as ObjectQuery);
                    subDataNode.Name = name;
                    subDataNode.ParentDataNode = dataNode;
                    dataNode = subDataNode;
                }

                dataNode = Nodes[0] .BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
        }
    }
}
