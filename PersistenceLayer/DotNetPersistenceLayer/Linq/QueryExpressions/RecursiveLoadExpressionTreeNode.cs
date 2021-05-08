using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{609caead-56d1-4c0d-b651-4c3041c88fe6}</MetaDataID>
    class RecursiveLoadExpressionTreeNode : ExpressionTreeNode
    {

        public RecursiveLoadExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            //Name = Name.Substring(0, Name.IndexOf(".Fetching("));

            //Name = Name.Substring(Name.LastIndexOf(".")+1);


        }
        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode DataNode
        {
            get
            {
                return Parent.DataNode;
            }
            set
            {

            }
        }

        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode BuildDataNodeTree(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (Parent.Expression.Type == Expression.Type.GetMetaData().GetGenericArguments()[0])
            {
                List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> recursiveSubDataNodes = new List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode>(dataNode.SubDataNodes);

                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode recursiveDataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
                recursiveDataNode.RecursiveSteps = (int)(Nodes[1] as OOAdvantech.Linq.QueryExpressions.ConstantExpressionTreeNode).Value;
                recursiveDataNode.Recursive = true;
                foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode recursiveSubDataNode in recursiveSubDataNodes)
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode newDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery as OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery);
                    newDataNode.Name = recursiveSubDataNode.Name;
                    newDataNode.ParentDataNode = recursiveDataNode;
                    newDataNode.Alias = recursiveSubDataNode.Alias;
                    newDataNode.ParticipateInSelectClause = recursiveSubDataNode.ParticipateInSelectClause;
                }
                return recursiveDataNode;
            }
            else
                return dataNode;


        }

        internal void ParticipateInSelectList()
        {

            for (int i = 1; i < Nodes.Count; i++)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = Nodes[i].DataNode;
                while (dataNode != DataNode)
                {
                    dataNode.MembersFetchingObjectActivation = true;
                    dataNode.ParticipateInSelectClause = true;
                    dataNode.ObjectQuery.AddSelectListItem(dataNode);
                    dataNode = dataNode.ParentDataNode;
                }

            }



            //fetchingDataNode.ParticipateInSelectClause = true;
        }
    }
}
