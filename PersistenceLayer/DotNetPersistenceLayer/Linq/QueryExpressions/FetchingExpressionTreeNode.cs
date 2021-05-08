using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{1251e367-04c8-4632-9a05-aeb1df6c554a}</MetaDataID>
    class FetchingExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{9fae711e-9de2-4cb6-be4b-17b0abd0e498}</MetaDataID>
        public FetchingExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
                    : base(exp, parent, expressionTranslator)
        {
            Name = Name.Substring(0, Name.IndexOf(".Fetching("));
            Name = Name.Substring(Name.LastIndexOf(".") + 1);
        }

        /// <MetaDataID>{5c738249-e0d5-4e3f-93f6-89f1935cf6dc}</MetaDataID>
        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode DataNode
        {
            get
            {


                return Nodes[0].DataNode;
            }
            set
            {

            }
        }
        /// <MetaDataID>{1fa504d2-83fc-411f-9501-20ac4f043a55}</MetaDataID>
        public override SearchCondition FilterDataCondition
        {
            get
            {
                return Nodes[0].FilterDataCondition;
                //return SearchCondition.JoinSearchConditions(ParentSearchCondition, Nodes[0].SearchCondition);

            }
        }

        /// <MetaDataID>{292a1381-7023-4b98-aee2-4825073e8bdb}</MetaDataID>
        public ExpressionTreeNode SourceCollection
        {
            get
            {
                if (Nodes[0] is ParameterExpressionTreeNode)
                    return (Nodes[0] as ParameterExpressionTreeNode).SourceCollection;
                else
                    return Nodes[0];
            }

        }
        /// <MetaDataID>{e982ba73-7ed6-4d42-9224-4181afe3229c}</MetaDataID>
        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode BuildDataNodeTree(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].BuildDataNodeTree(dataNode, linqObjectQuery);

            }

            return base.BuildDataNodeTree(dataNode, linqObjectQuery);


        }

        /// <MetaDataID>{7d0389a0-aa70-4796-9908-0d5d7ac6e024}</MetaDataID>
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
