using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection;



namespace OOAdvantech.Linq.QueryExpressions
{

    /// <MetaDataID>{153c805a-05f8-42f3-9a26-a1bee905ec9c}</MetaDataID>
    interface IFilteredSource
    {
        /// <MetaDataID>{f3e3ee0b-386b-4945-bfae-8201b8f66316}</MetaDataID>
        void BuildDataFilter();
    }
    /// <MetaDataID>{4179f14d-6222-4a56-a65f-e7e1a17880c7}</MetaDataID>
    abstract class MethodCallAsCollectionProviderExpressionTreeNode : ExpressionTreeNode, IFilteredSource
    {
        /// <MetaDataID>{5475d1eb-c2be-473a-b8f4-6b8fe9d1d3a0}</MetaDataID>
        protected IDynamicTypeDataRetrieve _DynamicTypeDataRetrieve;

        /// <MetaDataID>{2a43305e-48d7-485a-b89c-71b7538c0716}</MetaDataID>
        internal override IDynamicTypeDataRetrieve DynamicTypeDataRetrieve
        {
            get
            {
                return _DynamicTypeDataRetrieve;
            }
        }
        /// <MetaDataID>{03eeaa91-fdf2-4221-a6d9-0b49c481ca1a}</MetaDataID>
        public MethodCallAsCollectionProviderExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {



            MethodCallExpression = exp as MethodCallExpression;

            if (MethodCallExpression.Arguments[0] is MemberExpression)
            {

                ParameterExpression parameter = Translators.ExpressionVisitor.GetParameter(MethodCallExpression.Arguments[0]);
                if (!expressionTranslator.ParameterDeclareExpression.ContainsKey(parameter))
                    expressionTranslator.ParameterDeclareExpression.Add(parameter, this);
            }


            if (MethodCallExpression.Arguments.Count > 1)
            {
                if (Translators.ExpressionVisitor.GetLambdaExpression(MethodCallExpression.Arguments[1]) != null)
                {
                    SourceCollectionParameter = Translators.ExpressionVisitor.GetLambdaExpression(MethodCallExpression.Arguments[1]).Parameters[0];
                    expressionTranslator.ParameterDeclareExpression.Add(SourceCollectionParameter, this);
                }
                else
                {

                }
            }
        }
        /// <MetaDataID>{b824700f-69c5-4657-8df4-1b5f77d35480}</MetaDataID>
        public readonly ParameterExpression SourceCollectionParameter;
        /// <MetaDataID>{26f48765-bfda-4ef3-b174-046e1fcf2503}</MetaDataID>
        internal protected readonly MethodCallExpression MethodCallExpression;
        /// <MetaDataID>{39477a4a-d460-4218-b678-bc4f09f5ef1e}</MetaDataID>
        abstract internal ExpressionTreeNode SourceCollection
        {
            get;
        }
        /// <MetaDataID>{bffa23fb-75e2-4b23-872e-f2321507eb7f}</MetaDataID>
        abstract public void BuildDataFilter();

        /// <MetaDataID>{8e005e4b-8793-4a5d-a8f8-0a3969b2b90e}</MetaDataID>
        /// <summary>
        /// ReferenceDataNode is a root DataNode for selection expression. 
        /// Used in case where there are sub expressions which use data from root DataNode tree.
        /// In following example the main from sequence for Orders produce the IClient.Orders
        /// The second from sequence for the orderDetails use as ReferenceDataNode the Orders DataNode
        /// </summary>
        /// <example>
        /// var orderDetails = from client in clients
        ///                       from order in client.Orders
        ///                       select new
        ///                       {
        ///                           order,
        ///                           orderDetails = from orderDetail in order.OrderDetails
        ///                                          select new { orderDetail.Price, orderDetail }
        ///                       };
        /// </example>
        internal protected DataNode ReferenceDataNode = null;






        /// <MetaDataID>{8bd23f32-ddf4-470d-ab54-4e1440c7f06d}</MetaDataID>
        public override SearchCondition FilterDataCondition
        {
            get
            {
                //var parentExpression = Parent;
                //SearchCondition searchCondition = null;
                //while (parentExpression != null && parentExpression.Name != "Root")
                //{
                //    searchCondition = SearchCondition.JoinSearchConditions(searchCondition, parentExpression._FilterDataCondition);
                //    parentExpression = parentExpression.Parent;
                //}

                return SourceCollection.FilterDataCondition;
                //return SearchCondition.JoinSearchConditions(AncestorsFilterDataCondition, SourceCollection.FilterDataCondition);

                
            }

        }
    }
}
