using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{204734a4-6586-477b-b67e-7a9944f2346f}</MetaDataID>
    class ConstantExpressionTreeNode:ExpressionTreeNode
    {
        /// <MetaDataID>{42d6ee90-491e-4028-98e7-b793f0345866}</MetaDataID>
        public ConstantExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Constant)
                throw new System.Exception("Wrong expression type");

        }
        /// <MetaDataID>{69fc8c2b-e2bc-4d09-9e0d-5bee5d3f0926}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if ((Expression as ConstantExpression).Value is System.Linq.IQueryable)
            {

                DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery as ObjectQuery);
                DataNode.Name = ((this.Expression as ConstantExpression).Value as System.Linq.IQueryable).ElementType.Name;
                DataNode.Alias = Alias;
                DataNode.AssignedMetaObject = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(TypeHelper.GetElementType((Expression as ConstantExpression).Type));
                return DataNode;
            }
            else
            {
                if (dataNode.Name == "Root" && dataNode.ParentDataNode == null && linqObjectQuery is QueryOnRootObject&&Nodes.Count>0)
                {
                    DataNode= Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
                    if (!string.IsNullOrEmpty(Alias))
                        DataNode.Alias = Alias;

                    return DataNode;
                }
                else
                {
                    DataNode = new DataNode(linqObjectQuery as ObjectQuery);
                    DataNode.Name = "LocalField";
                }
                
                return null;
            }


        }
        /// <MetaDataID>{0443aee3-25c5-4600-9bd7-fe24bec8ff56}</MetaDataID>
        public object Value
        {
            get
            {
                if (Nodes.Count > 0 && Nodes[0] is MemberAccessExpressionTreeNode)
                    return GetValue((Expression as ConstantExpression).Value, Nodes[0] as MemberAccessExpressionTreeNode);
                else 
                    return (Expression as ConstantExpression).Value;
            }
        }

        /// <MetaDataID>{b52c12cb-1c91-4722-8ed2-497cd6ad7d5d}</MetaDataID>
        private object GetValue(object value, MemberAccessExpressionTreeNode memberAccessExpressionTreeNode)
        {
            if(value==null)
                return null;
            if ((memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member is System.Reflection.PropertyInfo)
                value = ((memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetValue(value, null);
            else
                value = ((memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.FieldInfo).GetValue(value);
            if (memberAccessExpressionTreeNode.Nodes.Count > 0 && memberAccessExpressionTreeNode.Nodes[0] is MemberAccessExpressionTreeNode)
                return GetValue(value, memberAccessExpressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode);
            else
                return value;
            
        }

    }
}
