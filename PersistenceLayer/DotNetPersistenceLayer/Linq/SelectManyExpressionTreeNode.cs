using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    class SelectManyExpressionTreeNode : MethodCallAsCollectionSourceExpressionTreeNode
    {
        public SelectManyExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
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


        ExpressionTreeNode DerivedCollection
        {
            get
            {
                return Nodes[1];
            }
        }
        string DerivedCollectionIteratedObjectName
        {
            get
            {
                return Nodes[4].Name;
            }
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
                return Nodes[3].Name;
            }
        }
        ExpressionTreeNode DerivedCollectionTypeExpression
        {
            get
            {
                return Nodes[2];
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
            dataNode=SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            dataNode=DerivedCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            dataNode.Alias = DerivedCollectionIteratedObjectName;
            if (Parent is MethodCallAsCollectionSourceExpressionTreeNode)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode selectionCallDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                SourceCollection.DataNode.ParentDataNode = selectionCallDataNode;
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode derivedCollectionDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                DerivedCollection.DataNode = derivedCollectionDataNode;
                derivedCollectionDataNode.Name = DerivedCollectionIteratedObjectName;//((Nodes[2] as ExpresionTreeNode).Expression as System.Linq.Expressions.NewExpression).Type.GetProperties()[1].Name;
                derivedCollectionDataNode.Temporary = true;
                derivedCollectionDataNode.ParentDataNode = selectionCallDataNode;
                if (SourceCollection is ConstantExpressionTreeNode)
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode constSubDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                    constSubDataNode.Name = SourceCollectionIteratedObjectName;//((Nodes[2] as ExpresionTreeNode).Expression as System.Linq.Expressions.NewExpression).Type.GetProperties()[1].Name;
                    constSubDataNode.Temporary = true;
                    constSubDataNode.ParentDataNode = selectionCallDataNode;
                }
                DataNode = selectionCallDataNode;
                DataNode.Alias = (Parent as MethodCallAsCollectionSourceExpressionTreeNode).SourceCollectionIteratedObjectName;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
                DataNode.Temporary = true;
                //dataNode = parentDataNde;
                return DataNode;
            }
            else
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode parentDataNde = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                Nodes[0].DataNode.ParentDataNode = parentDataNde;
                parentDataNde.Name = "Root";
                parentDataNde.Temporary = true;


                dataNode = Nodes[0].DataNode;
                (linqObjectQuery as ILINQObjectQuery).EnumerableType = DerivedCollectionType;
                
                int i = 0;
                foreach (ExpressionTreeNode memberNode in DerivedCollectionTypeExpression.Nodes)
                {
                    if (memberNode.Name == SourceCollectionIteratedObjectName)
                    {
                        dataNode = SourceCollection.DataNode;
                        dataNode=memberNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                        dataNode.Alias = DerivedCollectionType.GetProperties()[i].Name;
                        linqObjectQuery.AddSelectListItem(dataNode);
                    }
                    else
                    {
                        dataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                        dataNode.Name = memberNode.Name;
                        dataNode.Temporary = true;
                        memberNode.DataNode = dataNode;
                        dataNode.ParentDataNode =SourceCollection.DataNode;
                        dataNode=memberNode.BuildDataNodeTree(dataNode, linqObjectQuery);
                        dataNode.Alias = DerivedCollectionType.GetProperties()[i].Name;
                        dataNode.Temporary = true;
                        linqObjectQuery.AddSelectListItem(dataNode);
                    }
                    i++;
                }
                DataNode = parentDataNde;
                return DataNode;
            }
            

        }


    }
}
