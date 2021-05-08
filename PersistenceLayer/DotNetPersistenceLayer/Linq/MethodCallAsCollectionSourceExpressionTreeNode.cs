using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;



namespace OOAdvantech.Linq.QueryExpressions
{
    public abstract class MethodCallAsCollectionSourceExpressionTreeNode : ExpressionTreeNode
    {
        public MethodCallAsCollectionSourceExpressionTreeNode(Expression exp, ExpressionTreeNode parent, ObjectQuery objectQuery)
            : base(exp, parent,objectQuery)
        {

        }
        abstract internal ExpressionTreeNode SourceCollection
        {
            get;
        }
        abstract  internal string SourceCollectionIteratedObjectName
        {
            get;
        }
        abstract internal SearchCondition BuildSearchCondition(SearchCondition searchCondition);



    }
}
