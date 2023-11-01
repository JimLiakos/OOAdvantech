using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{103f7c68-a5ec-47bb-9bb6-e60b7f0a594c}</MetaDataID>
    internal class ObjectMethodCallExpressionTreeNode : ExpressionTreeNode
    {
        /// <MetaDataID>{e335a957-f345-4bff-ad47-d165aaa57cd0}</MetaDataID>
        internal ObjectMethodCallExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Call)
                throw new System.Exception("Wrong expression type");
        }
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            if (dataNode!=null)
                return base.BuildDataNodeTree(dataNode, linqObjectQuery);
            else
            {

                //ExpressionTreeNode expressionTreeNode = this;
                //while(string.IsNullOrWhiteSpace(expressionTreeNode.Alias)&&expressionTreeNode.Parent!=null)
                //    expressionTreeNode= expressionTreeNode.Parent; 

                //Alias

                Type declarationType = (this.Expression as MethodCallExpression).Method.DeclaringType;

                Type returnType = TypeHelper.GetElementType(this.Expression.Type);

                
                dataNode = new DataNode(linqObjectQuery as ObjectQuery);
                dataNode.Name = (this.Expression as MethodCallExpression).Method.Name;
                //dataNode.Alias = Alias;
                dataNode.AssignedMetaObject = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(returnType);

                DataNode rootDataNode= new DataNode(linqObjectQuery as ObjectQuery);
                rootDataNode.AssignedMetaObject = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(declarationType);
                rootDataNode.Name = rootDataNode.Classifier.Name;
                dataNode.ParentDataNode = rootDataNode;

                DataNode =dataNode;
                return dataNode;

            }
        }
        //virtual DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        //{
        //    return dataNode;
        //}



        /// <MetaDataID>{818320b5-e995-4060-b133-94b9f4fe7213}</MetaDataID>
        public object Value
        {
            get
            {
                System.Reflection.ParameterInfo[] parameters = (Expression as MethodCallExpression).Method.GetParameters();
                object[] paramsValue = new object[parameters.Length];
                int paramOffset = 1;
                if ((Expression as MethodCallExpression).Method.IsStatic)
                    paramOffset = 0;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (Nodes[i + paramOffset] is ConstantExpressionTreeNode)
                        paramsValue[i] = (Nodes[i + paramOffset] as ConstantExpressionTreeNode).Value;
                    else
                        paramsValue[i] = (Nodes[i + paramOffset] as ObjectMethodCallExpressionTreeNode).Value;
                }

                if ((Expression as MethodCallExpression).Object != null)
                {
                    object methodInstance = null;
                    if (!(Expression as MethodCallExpression).Method.IsStatic)
                    {
                        if ((Nodes[0] is ConstantExpressionTreeNode))
                            methodInstance = (Nodes[0] as ConstantExpressionTreeNode).Value;
                        else
                            methodInstance = (Nodes[0] as ObjectMethodCallExpressionTreeNode).Value;
                    }
                    return (Expression as MethodCallExpression).Method.Invoke(methodInstance, paramsValue);
                }
                else
                    return (Expression as MethodCallExpression).Method.Invoke(null, paramsValue);
            }
        }



    }
}
