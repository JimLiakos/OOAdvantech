using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.Translators
{
    /// <MetaDataID>{4b6f9958-23b3-4852-8957-b4a4a9352f4e}</MetaDataID>
    class RetrieveObjectMemberTranslator : ExpressionVisitor
    {

        /// <MetaDataID>{a0d07086-0f7e-4ab9-b972-04fa07aed419}</MetaDataID>
        Type RootObjectType;
        /// <MetaDataID>{375cebe2-b600-45fa-be08-56f462023103}</MetaDataID>
        public RetrieveObjectMemberTranslator(Type rootObjectType)
            : base(null)
        {
            RootObjectType = rootObjectType;
             
        }
        /// <MetaDataID>{1731e5c8-2126-48b5-a33f-39e22c0866de}</MetaDataID>
        internal override void Translate(System.Linq.Expressions.Expression expression)
        {

            ExpressionTreeNode root = new OOAdvantech.Linq.ExpressionTreeNode("Root",this);
            // Root = root;
            root.Nodes.Clear();



            Visit(expression, ref root);
            try
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = null;
                dataNode = root.Nodes[0].BuildDataNodeTree(dataNode, LINQObjectQuery as ILINQObjectQuery);

                OOAdvantech.MetaDataRepository.Classifier classifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(RootObjectType);
                RootPaths.Add(dataNode);
                System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
                GetPaths(dataNode, ref paths);
                foreach (DataNode mdataNode in paths)
                {
                    string message = mdataNode.FullName + " " + mdataNode.Alias;
                    //System.Diagnostics.Debug.WriteLine(message);
                }

            }
            catch (System.Exception error)
            {

            }
            List<ValuesRetrieveExpressions.NewExpressionTreeNode> newExpressionTreeNodes = new List<OOAdvantech.Linq.ValuesRetrieveExpressions.NewExpressionTreeNode>();
            GetNewExpressionTreeNodes(root, newExpressionTreeNodes);
            foreach (ValuesRetrieveExpressions.NewExpressionTreeNode newExpressionTreeNode in newExpressionTreeNodes)
            {
                List<DataNode> dataNodes = new List<DataNode>();
                foreach (ExpressionTreeNode expressionTreeNode in newExpressionTreeNode.Nodes)
                    dataNodes.Add(expressionTreeNode.DataNode);
                TypesDataNodes.Add((newExpressionTreeNode.Expression as NewExpression).Type, dataNodes);
            }



            ExpressionTreeNode.lastExpressionTree = root;
        }

        /// <MetaDataID>{8f71e942-c5e5-4442-95e8-cea849f2e0e8}</MetaDataID>
        internal Dictionary<Type, List<DataNode>> TypesDataNodes = new Dictionary<Type, List<DataNode>>();
        /// <MetaDataID>{32fe593f-21cd-41cf-9f56-26723d4deba4}</MetaDataID>
        private void GetNewExpressionTreeNodes(ExpressionTreeNode root, List<OOAdvantech.Linq.ValuesRetrieveExpressions.NewExpressionTreeNode> newExpressionTreeNodes)
        {
            if (root is ValuesRetrieveExpressions.NewExpressionTreeNode)
                newExpressionTreeNodes.Add(root as ValuesRetrieveExpressions.NewExpressionTreeNode);
            foreach (ExpressionTreeNode expressionTreeNode in root.Nodes)
                GetNewExpressionTreeNodes(expressionTreeNode, newExpressionTreeNodes);
        }

        /// <MetaDataID>{b599dd46-3430-4f13-b3a7-4dcc60a7ab6f}</MetaDataID>
        void GetPaths(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> paths)
        {
            // if(dataNode.SubDataNodes.Count==0)
            paths.Add(dataNode);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                GetPaths(subDataNode, ref paths);
            }
        }
        /// <MetaDataID>{f24348c5-bab3-49b8-b90f-fd0e8ccab747}</MetaDataID>
        internal protected override ExpressionTreeNode CreateExpressionTreeNode(ExpressionTreeNodeType expressionTreeNodeType, System.Linq.Expressions.Expression exp, ExpressionTreeNode parent, ExpressionVisitor expressionTranslator)
        {

            switch (expressionTreeNodeType)
            {
                case ExpressionTreeNodeType.Constant:
                    {
                        return new ValuesRetrieveExpressions.ConstantExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.MemberAccess:
                    {
                        return new ValuesRetrieveExpressions.MemberAccessExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.Parameter:
                    {
                        return new ValuesRetrieveExpressions.ParameterExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.Select:
                    {
                        return new ValuesRetrieveExpressions.SelectExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.NewExpression:
                    {
                        return new ValuesRetrieveExpressions.NewExpressionTreeNode(exp, parent, expressionTranslator);
                    }

                default:
                    throw new NotSupportedException(expressionTreeNodeType.ToString() + " operator doesn't supported");
            }
            throw new NotImplementedException();
        }
        /// <MetaDataID>{b1d290ea-55cc-4467-81cf-36be6e61e9c3}</MetaDataID>
        protected override Expression VisitMethodCall(MethodCallExpression mc, ref  ExpressionTreeNode parent)
        {
            // System.Windows.Forms.MessageBox.Show(mc.Method.Name);

            System.Type declaringType = mc.Method.DeclaringType;
            return base.VisitMethodCall(mc, ref parent);

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
