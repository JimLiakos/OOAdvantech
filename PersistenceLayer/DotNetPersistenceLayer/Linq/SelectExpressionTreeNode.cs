using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    class SelectExpressionTreeNode : MethodCallAsCollectionSourceExpressionTreeNode
    {
        public SelectExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent, objectQuery)
        {

        }
        internal override SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        {
            if (SourceCollection is MethodCallAsCollectionSourceExpressionTreeNode)
                return (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            else
                return searchCondition;
        }
        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                return Nodes[0];
            }
        }
        internal override string SourceCollectionIteratedObjectName
        {
            get
            {
                return Nodes[2].Name;
            }
        }


        ExpressionTreeNode DerivedCollectionTypeExpression
        {
            get
            {
                return Nodes[1];
            }
        }
        Type DerivedCollectionType
        {
            get
            {
                return (DerivedCollectionTypeExpression.Expression as NewExpression).Type; ;
            }
        }


        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {
            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            ////(Nodes[0] as MyTreeNode).DataNode = dataNode;
            //if ((Nodes[1] as ExpressionTreeNode).Expression.NodeType == ExpressionType.Parameter)
            //{

            //    dataNode = (Nodes[1] as ExpressionTreeNode).BuildDataNodeTree(dataNode, linqObjectQuery);

            //    if (Nodes.Count > 2 && (Nodes[2] as ExpressionTreeNode).Expression.NodeType == ExpressionType.New)
            //        dataNode.Alias = (Nodes[4] as ExpressionTreeNode).Name;//.Nodes[1] as ExpresionTreeNode).Name; //.Expression as System.Linq.Expressions.NewExpression).Type.GetProperties()[1].Name;

            //}
            //else
            //{

            //}
            if (Parent is MethodCallAsCollectionSourceExpressionTreeNode)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode parentDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                SourceCollection.DataNode.ParentDataNode = parentDataNode;
                DataNode = parentDataNode;
                DataNode.Alias = (Parent as MethodCallAsCollectionSourceExpressionTreeNode).SourceCollectionIteratedObjectName;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
                DataNode.Temporary = true;
                int i = 0;
                foreach (ExpressionTreeNode memberNode in DerivedCollectionTypeExpression.Nodes)
                {
                    if (memberNode.Name == SourceCollectionIteratedObjectName)
                    {
                        dataNode = SourceCollection.DataNode;
                        dataNode = memberNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                        dataNode.Alias = DerivedCollectionType.GetProperties()[i].Name;
                    }
                    else
                    {
                        dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                        dataNode.Name = memberNode.Name;
                        memberNode.DataNode = dataNode;
                        dataNode.ParentDataNode = DataNode;
                        dataNode = memberNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                        dataNode.Alias = DerivedCollectionType.GetProperties()[i].Name;
                        dataNode.Temporary = true;
                    }

                    i++;
                }

                dataNode = parentDataNode;
            }
            else
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode parentDataNde = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                Nodes[0].DataNode.ParentDataNode = parentDataNde;
                parentDataNde.Name = "Root";
                parentDataNde.Temporary = true;


                dataNode = Nodes[0].DataNode;
                (linqObjectQuery as ILINQObjectQuery).EnumerableType = (Nodes[1].Expression as NewExpression).Type;
                int i = 0;
                foreach (ExpressionTreeNode treeNode in Nodes[1].Nodes)
                {
                    dataNode = Nodes[0].DataNode;
                    dataNode = treeNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                    dataNode.Alias = (Nodes[1].Expression as System.Linq.Expressions.NewExpression).Type.GetProperties()[i++].Name;
                    linqObjectQuery.AddSelectListItem(dataNode);
                }
                dataNode = parentDataNde;
                DataNode = parentDataNde;


            }



            dataNode = DataNode;
            return dataNode;
        }
    }
}
