using System;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
namespace OOAdvantech.Linq.Translators
{
  
         class TakeTranslator : ExpressionVisitor
        {
             protected override ExpressionTreeNode CreateExpressionTreeNode(ExpressionVisitor.ExpressionTreeNodeType expressionTreeNodeType, Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
             {
                 throw new NotImplementedException();
             }

            private int? _count;
            private bool _useDefault;

            public TakeTranslator(ExpressionVisitor mainExpressionVisitor)
                : base(mainExpressionVisitor)
            {
            }


            internal static TakeTranslator GetNewFirstTranslator(bool useDefault,ExpressionVisitor mainExpressionVisito)
            {
                return new TakeTranslator(mainExpressionVisito)
                {
                    _count = 1,
                    _useDefault = useDefault
                };
            }

            internal int? Count
            {
                get { return _count; }
            }

            internal bool UseDefault
            {
                get { return _useDefault; }
            }

              

            internal override void Translate(Expression exp, ExpressionTreeNode parent)
            {
                Visit(exp,ref parent);
            }

            protected override Expression VisitConstant(ConstantExpression c,ref  ExpressionTreeNode parent)
            {
                if (c.Type == typeof(Int32))
                {
                    _count = (Int32)c.Value;
                }

                return c;
            }
        }
    
}
