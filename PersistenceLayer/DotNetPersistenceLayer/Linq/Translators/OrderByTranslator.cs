#region Copyright (c) 2008 Microsoft Corporation.  All rights reserved.
// Copyright (c) 2008 Microsoft Corporation.  All rights reserved.
// 
// THIS SOFTWARE COMES "AS IS", WITH NO WARRANTIES.  THIS
// MEANS NO EXPRESS, IMPLIED OR STATUTORY WARRANTY, INCLUDING
// WITHOUT LIMITATION, WARRANTIES OF MERCHANTABILITY OR FITNESS
// FOR A PARTICULAR PURPOSE OR ANY WARRANTY OF TITLE OR
// NON-INFRINGEMENT.
//
// MICROSOFT WILL NOT BE LIABLE FOR ANY DAMAGES RELATED TO
// THE SOFTWARE, INCLUDING DIRECT, INDIRECT, SPECIAL,
// CONSEQUENTIAL OR INCIDENTAL DAMAGES, TO THE MAXIMUM EXTENT
// THE LAW PERMITS, NO MATTER WHAT LEGAL THEORY IT IS
// BASED ON.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

using System.Linq.Expressions;

namespace OOAdvantech.Linq.Translators
{
  
     class OrderByTranslator : ExpressionVisitor
    {
      private readonly List<OrderByItem> _memebers;
      private OrderDirection _currentDirection;
      protected override ExpressionTreeNode CreateExpressionTreeNode(ExpressionVisitor.ExpressionTreeNodeType expressionTreeNodeType, Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
      {
          throw new NotImplementedException();
      }
      internal override void Translate(Expression expression, ExpressionTreeNode root)
      {
          throw new NotImplementedException();
      }


      public OrderByTranslator(ExpressionVisitor mainExpressionVisitor):base(mainExpressionVisitor)
      {
        _memebers = new List<OrderByItem>();
      }

      internal string OrderByClause
      {
        get
        {
          if (_memebers.Count == 0)
            return string.Empty;

          StringBuilder sb = new StringBuilder();
          bool isFirst = true;
          foreach (var m in _memebers.Reverse<OrderByItem>())
          {
            if (!isFirst)
              sb.Append(", ");
            else
              isFirst = false;

            //sb.Append(FormatHelper.WrapInBrackets(m.DataMember.MappedName));

            switch (m.Direction)
            {
              case OrderDirection.Ascending:
                sb.Append(" ASC");
                break;
              case OrderDirection.Descending:
                sb.Append(" DESC");
                break;
              default:
                throw new NotSupportedException("The selected sorting direction is not supported");
            }  
          }
          return sb.ToString();
        }
      }

      internal virtual Expression Visit(Expression e, OrderDirection direction,ref  ExpressionTreeNode parent)
      {
        _currentDirection = direction;
        return Visit(e,ref parent);
      }

      protected override Expression VisitMemberAccess(MemberExpression m,ref  ExpressionTreeNode parent)
      {
        // get the selected columns

        if (m.Expression == null || m.Expression.NodeType != ExpressionType.Parameter)
          throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));

        // use the mapping metadata and find the name of this member in the database
        System.Type declaringType = m.Member.DeclaringType;
        OOAdvantech.MetaDataRepository.Classifier classifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(declaringType);
        
          
        foreach(OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(false))
        {
            if (attribute.Name == m.Member.Name)
            {
                _memebers.Add(new OrderByItem{DataMember = attribute,Direction = _currentDirection});
                return m;
                break;
            }
            
        }




        //DatabaseMetaTable metaTable = _model.GetTable(declaringType);

        //if (metaTable == null)
        //  throw new Exception(string.Format("It was not possible to get metadat for {0}!", declaringType.Name));

        //DatabaseMetaDataMember metaMember = metaTable.GetPersistentDataMember(m.Member);

        //if (metaMember == null)
        //  throw new Exception(string.Format("The member {0} in where expression cannot be found on the {1} class", m.Member.Name, declaringType.Name));

        //_memebers.Add(
        //  new OrderByItem
        //  {
        //    DataMember = metaMember,
        //    Direction = _currentDirection
        //  });

        return m;
      }

      private class OrderByItem
      {
        public OOAdvantech.MetaDataRepository.Attribute DataMember;
        public OrderDirection Direction;
      }
    }

    internal enum OrderDirection
    {
      Ascending,
      Descending
    }
  
}
