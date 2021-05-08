using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.Translators
{
    public  class RetrieveObjectMemberTranslator : ExpressionVisitor
    {

        Type RootObjectType;
        public RetrieveObjectMemberTranslator(Type rootObjectType):base(null)
        {
            RootObjectType = rootObjectType;

        }
        internal override void Translate(System.Linq.Expressions.Expression expression, ExpressionTreeNode root)
        {
            if (root == null)
                root = new OOAdvantech.Linq.ExpressionTreeNode("Root");
           // Root = root;
            root.Nodes.Clear();



            Visit(expression, ref root);
            try
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = null;
                dataNode = root.Nodes[0].BuildDataNodeTree(dataNode, LINQObjectQuery);

                OOAdvantech.MetaDataRepository.Classifier classifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(RootObjectType);

                //MetaDataRepository.Namespace _namespace = classifier.Namespace;
                //while (_namespace != null)
                //{
                //    DataNode namespaceDataNode = new DataNode(LINQObjectQuery);
                //    namespaceDataNode.Name = _namespace.Name;
                //    dataNode.ParentDataNode = namespaceDataNode;
                //    dataNode = namespaceDataNode;
                //    _namespace = _namespace.Namespace;
                //}
                RootPaths.Add(dataNode);





                System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();

                 GetPaths(dataNode, ref paths);
                 foreach (DataNode mdataNode in paths)
                 {
                     string message = mdataNode.FullName + " " + mdataNode.Alias;
                     //if (mdataNode.ParticipateInWereClause)
                     //    message += " PW";
                     //if (mdataNode.ParticipateInSelectClause)
                     //    message += " SL";
                     System.Diagnostics.Debug.WriteLine(message);
                 }

            }
            catch (System.Exception error)
            {

            }


            lastExpressionTree = root;
        }

        void GetPaths(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> paths)
        {
            // if(dataNode.SubDataNodes.Count==0)
            paths.Add(dataNode);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                GetPaths(subDataNode, ref paths);
            }
        }
        protected override ExpressionTreeNode CreateExpressionTreeNode(ExpressionTreeNodeType expressionTreeNodeType, System.Linq.Expressions.Expression exp, ExpressionTreeNode parent, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery objectQuery)
        {

            switch (expressionTreeNodeType)
            {
                case ExpressionTreeNodeType.Constant:
                    {
                        return new ValuesRetrieveExpressions.ConstantExpressionTreeNode(exp, parent, objectQuery);
                    }
                case ExpressionTreeNodeType.MemberAccess:
                    {
                        return new ValuesRetrieveExpressions.MemberAccessExpressionTreeNode(exp, parent, objectQuery);
                    }
                case ExpressionTreeNodeType.Parameter:
                    {
                        return new ValuesRetrieveExpressions.ParameterExpressionTreeNode(exp, parent, objectQuery);
                    }
                case ExpressionTreeNodeType.Select:
                    {
                        return new ValuesRetrieveExpressions.SelectExpressionTreeNode(exp, parent, objectQuery);
                    }
                case ExpressionTreeNodeType.NewExpression:
                    {
                        return new ValuesRetrieveExpressions.NewExpressionTreeNode(exp, parent, objectQuery);
                    }


                default:
                    throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }
        protected override Expression VisitMethodCall(MethodCallExpression mc, ref  ExpressionTreeNode parent)
        {
            // System.Windows.Forms.MessageBox.Show(mc.Method.Name);

            System.Type declaringType = mc.Method.DeclaringType;
            //if (declaringType != typeof(Queryable))
            //    throw new NotSupportedException(
            //      "Invalid Sequence Operator Call. The type for the operator is not Queryable!");

            switch (mc.Method.Name)
            {
                //case "Where":
                //    Visit(mc.Arguments[0],ref parent);
                //    // is this really a proper Where?
                //    var whereLambda = GetLambdaWithParamCheck(mc);
                //    if (whereLambda == null)
                //        break;

                //    VisitWhere(whereLambda,ref parent);
                //    break;
                case "OrderBy":
                case "ThenBy":
                    Visit(mc.Arguments[0], ref parent);
                    //// is this really a proper Order By?
                    //var orderLambda = GetLambdaWithParamCheck(mc);
                    //if (orderLambda == null)
                    //    break;

                    //VisitOrderBy(orderLambda, OrderDirection.Ascending, ref parent);
                    break;
                case "OrderByDescending":
                case "ThenByDescending":
                    Visit(mc.Arguments[0], ref parent);
                    // is this really a proper Order By Descending?
                    //var orderDescLambda = GetLambdaWithParamCheck(mc);
                    //if (orderDescLambda == null)
                    //    break;

                    //VisitOrderBy(orderDescLambda, OrderDirection.Descending, ref parent);
                    break;
                //case "Select":
                //    Visit(mc.Arguments[0],ref parent);
                //    // is this really a proper Select?
                //    var selectLambda = GetLambdaWithParamCheck(mc);
                //    if (selectLambda == null)
                //        break;

                //    VisitSelect(selectLambda,ref parent);
                //    break;

                case "Take":
                    Visit(mc.Arguments[0], ref parent);
                    //if (mc.Arguments.Count != 2)
                    //    break;

                    //VisitTake(mc.Arguments[1], ref parent);
                    break;

                case "First":
                    Visit(mc.Arguments[0], ref parent);
                    // This custom provider does not support the use of a First operator
                    // that takes a predicate. Therefore we check to ensure that no more
                    // than one argument is provided.
                    //if (mc.Arguments.Count != 1)
                    //    break;

                    //VisitFirst(false);
                    break;

                case "FirstOrDefault":
                    Visit(mc.Arguments[0], ref parent);
                    // This custom provider does not support the use of a FirstOrDefault
                    // operator that takes a predicate. Therefore we check to ensure that
                    // no more than one argument is provided.
                    //if (mc.Arguments.Count != 1)
                    //    break;

                    //VisitFirst(true);
                    break;

                default:
                    return base.VisitMethodCall(mc, ref parent);
            }


            return mc;
        }

    }
}
