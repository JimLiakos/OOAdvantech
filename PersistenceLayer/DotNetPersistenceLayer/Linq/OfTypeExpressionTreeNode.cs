using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    class OfTypeExpressionTreeNode : MethodCallAsCollectionSourceExpressionTreeNode
    {
        public OfTypeExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode BuildDataNodeTree(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery linqObjectQuery)
        {
            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            dataNode.ClassifierOfTypeFilterFullName =TypeHelper.GetElementType( Expression.Type).FullName;
            DataNode = dataNode;
            DataNode.AssignedMetaObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(Expression .Type));
            DataNode.Alias = Alias;
            return dataNode;
        }

        internal override ExpressionTreeNode SourceCollection
        {
            get 
            {
                return Nodes[0];
            }
        }
        internal override SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        {
            //foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
            //{
            //    if (treeNode is MethodCallAsCollectionSourceExpressionTreeNode)
            //        (treeNode as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(null);
            //}
            //if (SourceCollection is MethodCallAsCollectionSourceExpressionTreeNode)
            //{
            //    if (Parent.Name == "Root")
            //    {
            //        DataNode.SearchCondition = (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            //        return DataNode.SearchCondition;
            //    }
            //    else if (Parent.Expression != null && Parent.Expression.NodeType == ExpressionType.New && !(SourceCollection is GroupByExpressionTreeNode))
            //    {
            //        DataNode.SearchCondition = (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            //        return DataNode.SearchCondition;
            //    }
            //    else
            //        return (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            //}
            //else
            //    return searchCondition;

            return null;
        }
        
    }
}
