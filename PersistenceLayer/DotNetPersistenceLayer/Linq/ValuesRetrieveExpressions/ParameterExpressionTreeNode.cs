using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.ValuesRetrieveExpressions
{
    /// <MetaDataID>{9ef66bb3-f58d-4a09-80e5-5e54240d4290}</MetaDataID>
    class ParameterExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{4b09f4a3-80e5-43ce-91f4-e728890bd0bd}</MetaDataID>
        public ParameterExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Parameter)
                throw new System.Exception("Wrong expression type");

        }
        /// <MetaDataID>{3f01794b-ed0f-4bbd-bdd4-a5841f926dba}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
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
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery as ObjectQuery);
                    subDataNode.Name = name;
                    subDataNode.ParentDataNode = dataNode;
                    if (dataNode.Temporary)
                        subDataNode.Temporary = true;
                    dataNode = subDataNode;
                    //  DataNode = dataNode;
                }


                DataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return DataNode;
        }
    }
}
