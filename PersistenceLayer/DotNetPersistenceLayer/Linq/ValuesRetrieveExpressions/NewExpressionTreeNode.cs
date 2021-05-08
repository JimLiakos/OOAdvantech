using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.ValuesRetrieveExpressions
{
    /// <MetaDataID>{de4674fb-122f-40df-9dec-99c3f73dc132}</MetaDataID>
    class NewExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{65c88cab-568b-423c-b205-47823c89acf6}</MetaDataID>
        public NewExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.New)
                throw new System.Exception("Wrong expression type");
        }
        /// <MetaDataID>{820ffa32-6491-4b61-a56a-6f3ed90db8ae}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (dataNode == null)
            {
                dataNode = new DataNode(linqObjectQuery.ObjectQuery);
                dataNode.Name = "Root";
                dataNode.Temporary = true;
                //(linqObjectQuery as ILINQObjectQuery).EnumerableType = (Expression as NewExpression).Type;
            }
            int i = 0;
            foreach (ExpressionTreeNode expressionTreeNode in Nodes)
            {
                DataNode memberDataNode = expressionTreeNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                memberDataNode.Alias = (Expression as NewExpression).Type.GetMetaData().GetProperties()[i].Name;
                i++;
            }


            return dataNode;


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
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                    subDataNode.Name = name;
                    subDataNode.ParentDataNode = dataNode;
                    dataNode = subDataNode;
                }

                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
        }
    }
}
