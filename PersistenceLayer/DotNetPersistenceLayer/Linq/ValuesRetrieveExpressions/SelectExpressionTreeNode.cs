using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.ValuesRetrieveExpressions
{
    /// <MetaDataID>{1dbd2d8e-35e0-4fe7-a540-f6d9ad7f6c40}</MetaDataID>
    class SelectExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{2f22e0b7-dda9-4722-8760-bf57e24a94c3}</MetaDataID>
        public SelectExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {

        }
        //internal override SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        //{
        //    if (SourceCollection is MethodCallAsCollectionSourceExpressionTreeNode)
        //        return (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
        //    else
        //        return searchCondition;
        //}
        //internal override ExpressionTreeNode SourceCollection
        //{
        //    get
        //    {
        //        return Nodes[0];
        //    }
        //}
        //internal override string SourceCollectionIteratedObjectName
        //{
        //    get
        //    {
        //        return Nodes[2].Name;
        //    }
        //}


        /// <MetaDataID>{74470c73-2243-4d73-ae15-d45bd1648421}</MetaDataID>
        ExpressionTreeNode DerivedCollectionTypeExpression
        {
            get
            {
                return Nodes[1];
            }
        }
        /// <MetaDataID>{45e92f0f-e01c-413e-8b40-26d0bb99e443}</MetaDataID>
        Type DerivedCollectionType
        {
            get
            {
                return (DerivedCollectionTypeExpression.Expression as NewExpression).Type; ;
            }
        }


        /// <MetaDataID>{1b0c166b-3e7e-4b09-8257-91d4c01c0dce}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
           DataNode= Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
           Nodes[1].BuildDataNodeTree(DataNode, linqObjectQuery);
           if (!string.IsNullOrEmpty(Alias))
               DataNode.Alias = Alias;
           return DataNode;

            
        }

    }
}
