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
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection.Emit;
using System.Threading;
using System.Reflection;
using System.Linq;
namespace OOAdvantech.Linq.Translators
{

    public delegate void DataNodeTreesSimplificationHandler(Dictionary<DataNode, DataNode> replacedDataNodes);


    /// <MetaDataID>OOAdvantech.Linq.Translators.ExpressionVisitor</MetaDataID>
    internal abstract class ExpressionVisitor
    {

        /// <summary>
        /// Defines all types where declared or initialized from new expression
        /// </summary>
        internal Dictionary<System.Linq.Expressions.Expression, ExpressionTreeNode> ExpressionTreeNodes = new Dictionary<Expression, ExpressionTreeNode>();

        /// <summary>
        /// Defines the Expressions which  declare the parameter expresion
        /// </summary>
        internal Dictionary<ParameterExpression, ExpressionTreeNode> ParameterDeclareExpression = new Dictionary<ParameterExpression, ExpressionTreeNode>();
        /// <MetaDataID>{e44b7f93-c364-4203-b271-bc3143a42879}</MetaDataID>
        /// <summary>
        /// This member defines the dictionary with dynamic type -dynamic type data retrievers pairs 
        /// </summary>
        Dictionary<Type, List<IDynamicTypeDataRetrieve>> DynamicTypeDataRetrievers = new Dictionary<Type, List<IDynamicTypeDataRetrieve>>();
        ///// <MetaDataID>{054be47a-3026-4fa9-a4b6-dd9f0ce84b99}</MetaDataID>
        //internal protected Dictionary<string, IDynamicTypeDataRetrieve> AliasEnumerators = new Dictionary<string, IDynamicTypeDataRetrieve>();
        /// <MetaDataID>{7b26d7cd-e258-4f23-b22a-ad9b3d526af3}</MetaDataID>
        /// <summary> 
        /// Add a dynamic type - dynamic type data retriever pair
        /// </summary>
        internal void AddDynamicTypeDataRetriever(System.Type type, IDynamicTypeDataRetrieve dynamicTypeDataRetriever)
        {
            if (DynamicTypeDataRetrievers.ContainsKey(type) &&
                !DynamicTypeDataRetrievers[type].Contains(dynamicTypeDataRetriever))
                DynamicTypeDataRetrievers[type].Add(dynamicTypeDataRetriever);
            else
            {
                if (!DynamicTypeDataRetrievers.ContainsKey(type))
                    DynamicTypeDataRetrievers[type] = new List<IDynamicTypeDataRetrieve>() { dynamicTypeDataRetriever };
                else
                    DynamicTypeDataRetrievers[type].Add(dynamicTypeDataRetriever);
            }
        }
        /// <MetaDataID>{581efb2d-6191-4c0f-ba0c-b6712777aa6b}</MetaDataID>
        /// <summary>
        /// This method return the dynamic data retriever for type
        /// In case where there isn't  dynamic data retriever for type the method return null
        /// </summary>
         internal IDynamicTypeDataRetrieve GetDynamicTypeDataRetriever(System.Type type)
        {
            string typename = type.Name;
            if (type.Name.IndexOf("<>f__AnonymousType") == -1)
            {
                int tt = 0;
                //System.Diagnostics.Debug.Assert(false, "dynamic data retriever for type");
            }
           List<IDynamicTypeDataRetrieve> dynamicTypesDataRetriever = null;
           DynamicTypeDataRetrievers.TryGetValue(type, out dynamicTypesDataRetriever);
           if (dynamicTypesDataRetriever == null)
               return null;
           else
               return dynamicTypesDataRetriever[0];
        }
        ///// <MetaDataID>{eecdb24d-1831-4346-ae0d-335f2f1a7bee}</MetaDataID>
        //internal void AddAliasEnumerator(string alias, IDynamicTypeDataRetrieve enumerator)
        //{
        //    IDynamicTypeDataRetrieve existingAliasEnumerator;
        //    if (AliasEnumerators.TryGetValue(alias, out existingAliasEnumerator))
        //        if (existingAliasEnumerator == enumerator)
        //            return;

        //    AliasEnumerators.Add(alias, enumerator);
        //}
        ///// <MetaDataID>{5cf8adb0-d939-4db3-ae83-dfb3013e40a9}</MetaDataID>
        //internal IDynamicTypeDataRetrieve GetAliasEnumerator(string alias)
        //{
        //    IDynamicTypeDataRetrieve enumerator = null;
        //    AliasEnumerators.TryGetValue(alias, out enumerator);
        //    return enumerator;
        //}

        ///// <MetaDataID>{b59040e6-b395-49ba-ac25-fe1875ed2e45}</MetaDataID>
        //internal ExpressionTreeNode GetSourceCollection(ExpressionTreeNode expressionTreeNode, QueryExpressions.MemberAccessExpressionTreeNode member)
        //{
        //    if (expressionTreeNode is QueryExpressions.SelectExpressionTreeNode)
        //    {
        //        if ((expressionTreeNode as QueryExpressions.SelectExpressionTreeNode).SelectCollection.Expression.NodeType == ExpressionType.New)
        //        {
        //            foreach (ExpressionTreeNode newExpresionMember in (expressionTreeNode as QueryExpressions.SelectExpressionTreeNode).SelectCollection.Nodes)
        //            {
        //                if (newExpresionMember.Alias == member.Name)
        //                {
        //                    if (newExpresionMember is QueryExpressions.ParameterExpressionTreeNode)
        //                    {
        //                        var sourceTreeNode = GetSourceCollection(newExpresionMember, newExpresionMember.Name);
        //                        if (sourceTreeNode == null)
        //                            return sourceTreeNode;
        //                        if (member.Nodes.Count > 0)
        //                            return GetSourceCollection(sourceTreeNode, member.Nodes[0] as QueryExpressions.MemberAccessExpressionTreeNode);
        //                        else
        //                            return sourceTreeNode;

        //                    }
        //                    return newExpresionMember;
        //                }
        //            }
        //        }
        //        else if ((expressionTreeNode as QueryExpressions.SelectExpressionTreeNode).SelectCollection.Name == member.Name)
        //        {
        //            return (expressionTreeNode as QueryExpressions.SelectExpressionTreeNode).SelectCollection;
        //        }
        //    }
        //    if (expressionTreeNode is QueryExpressions.ParameterExpressionTreeNode)
        //    {

        //        QueryExpressions.ParameterExpressionTreeNode parameterExpressionTreeNode = expressionTreeNode as QueryExpressions.ParameterExpressionTreeNode;
        //        ExpressionTreeNode parent = expressionTreeNode.Parent;
        //        while (parent != null)
        //        {
        //            if (parent is QueryExpressions.SelectExpressionTreeNode)
        //            {
        //                if ((parent as QueryExpressions.SelectExpressionTreeNode).SourceCollection.Alias == expressionTreeNode.Name)
        //                {
        //                    expressionTreeNode = (parent as QueryExpressions.SelectExpressionTreeNode).SourceCollection;
        //                    if (parameterExpressionTreeNode.Nodes.Count > 0)
        //                        expressionTreeNode = GetSourceCollection(expressionTreeNode, parameterExpressionTreeNode.Nodes[0] as QueryExpressions.MemberAccessExpressionTreeNode);
        //                    if (expressionTreeNode == null)
        //                        return parameterExpressionTreeNode;
        //                    return expressionTreeNode;
        //                }
        //                parent = parent.Parent;
        //            }
        //            else
        //                parent = parent.Parent;
        //        }
        //        return null;
        //    }
        //    if (expressionTreeNode.Alias == member.Name)
        //        return expressionTreeNode;
        //    if (expressionTreeNode is QueryExpressions.WhereExpressionTreeNode)
        //        return GetSourceCollection((expressionTreeNode as QueryExpressions.WhereExpressionTreeNode).SourceCollection, member);
        //    return null;
        //}
        ///// <MetaDataID>{74baca22-ee9a-4d7d-b042-a315a22f52eb}</MetaDataID>
        ExpressionTreeNode GetSourceCollection(ExpressionTreeNode expressionTreeNode, string alias)
        {


            if (expressionTreeNode.Alias == alias)
            {
                //if (expressionTreeNode is QueryExpressions.ParameterExpressionTreeNode)
                //{

                //    QueryExpressions.ParameterExpressionTreeNode parameterExpressionTreeNode = expressionTreeNode as QueryExpressions.ParameterExpressionTreeNode;
                //    var tmp = parameterExpressionTreeNode.ParameterSourceCollection;
                //    ExpressionTreeNode parent = expressionTreeNode.Parent;
                //    while (parent != null)
                //    {
                //        if (parent is QueryExpressions.SelectExpressionTreeNode)
                //        {
                //            if ((parent as QueryExpressions.SelectExpressionTreeNode).SourceCollection.Alias == expressionTreeNode.Name)
                //            {
                //                expressionTreeNode = (parent as QueryExpressions.SelectExpressionTreeNode).SourceCollection;
                //                if (parameterExpressionTreeNode.Nodes.Count > 0)
                //                    expressionTreeNode = GetSourceCollection(expressionTreeNode, parameterExpressionTreeNode.Nodes[0] as QueryExpressions.MemberAccessExpressionTreeNode);
                //                if (expressionTreeNode == null)
                //                    return parameterExpressionTreeNode;
                //                return expressionTreeNode;
                //            }
                //            parent = parent.Parent;
                //        }
                //        else 
                //            parent = parent.Parent;
                //    }
                //    return null;
                //}
                //else
                return expressionTreeNode;
            }
            else
            {
                foreach (ExpressionTreeNode subExpressionTreeNode in expressionTreeNode.Nodes)
                {
                    expressionTreeNode = GetSourceCollection(subExpressionTreeNode, alias);
                    if (expressionTreeNode != null)
                        return expressionTreeNode;
                }
            }
            return null;


        }

        /// <summary>
        ///The event subscribers consume the event and replace 
        ///the reference to the temporary data nodes with real data nodes.
        ///In DataNode tree building phase, the expressions build the necessary data nodes.
        ///Sometimes the linq expression builds temporary DataNodes. 
        ///The linq expressions manager removes the temporary data nodes and 
        ///raises the simplification event.   
        /// </summary>
        public event DataNodeTreesSimplificationHandler DataNodeTreeSimplification;





        /// <MetaDataID>{eb1a146c-5eb3-4e7e-81fd-4e55b3b0b59d}</MetaDataID>
        abstract internal void Translate(Expression expression);
        /// <exclude>Excluded</exclude>
        protected ObjectQuery _LINQObjectQuery;
        /// <MetaDataID>{c6ec7e93-b936-434b-b3e4-d91761ce2ccf}</MetaDataID>
        internal ObjectQuery LINQObjectQuery
        {
            get
            {
                if (MainExpressionVisitor != null && MainExpressionVisitor != this)
                    return MainExpressionVisitor.LINQObjectQuery;
                else
                    return _LINQObjectQuery;
            }
            set
            {
                _LINQObjectQuery = value;
            }
        }

        /// <MetaDataID>{318b56df-5a84-485c-9485-ad37ceca7fcf}</MetaDataID>
        protected ExpressionVisitor MainExpressionVisitor;
        /// <MetaDataID>{01cf7768-b434-4b94-adce-83db6ffd3329}</MetaDataID>
        internal ExpressionVisitor(ExpressionVisitor mainExpressionVisitor)
        {
            MainExpressionVisitor = mainExpressionVisitor;
        }

        /// <MetaDataID>{f6d974bf-0c04-40f5-8b9b-06cf3c4ee94f}</MetaDataID>
        internal protected virtual Expression Visit(Expression exp, ref  ExpressionTreeNode parent)
        {


            //ExpressionTreeNode oldparent = parent;


            if (exp == null)
            {
                return exp;
            }
            try
            {
                ExpressionTreeNode expresionTreeNode = parent;


                switch (exp.NodeType)
                {

                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Power:
                    case ExpressionType.Divide:
                        return VisitArithmeticExpression((BinaryExpression)exp, ref parent);




                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.Coalesce:
                    case ExpressionType.Equal:
                    case ExpressionType.ExclusiveOr:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LeftShift:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.Modulo:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.RightShift:
                        return VisitBinary((BinaryExpression)exp, ref parent);
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                        return VisitUnary((UnaryExpression)exp, ref parent);
                    case ExpressionType.Call:
                        return VisitMethodCall((MethodCallExpression)exp, ref parent);

                    case ExpressionType.Conditional:
                        return VisitConditional((ConditionalExpression)exp, ref parent);

                    case ExpressionType.Constant:
                        return VisitConstant((ConstantExpression)exp, ref parent); ;

                    case ExpressionType.Invoke:
                        return VisitInvocation((InvocationExpression)exp, ref parent);

                    case ExpressionType.Lambda:
                        return VisitLambda((LambdaExpression)exp, ref parent);

                    case ExpressionType.ListInit:
                        return VisitListInit((ListInitExpression)exp, ref parent);

                    case ExpressionType.MemberAccess:
                        return VisitMemberAccess((MemberExpression)exp, ref parent);

                    case ExpressionType.MemberInit:
                        return VisitMemberInit((MemberInitExpression)exp, ref parent);

                    case ExpressionType.New:
                        return VisitNew((NewExpression)exp, ref parent);

                    case ExpressionType.NewArrayInit:
                    case ExpressionType.NewArrayBounds:
                        return VisitNewArray((NewArrayExpression)exp, ref parent);

                    case ExpressionType.Parameter:
                        return VisitParameter((ParameterExpression)exp, ref parent);


                    case ExpressionType.TypeIs:
                        return VisitTypeIs((TypeBinaryExpression)exp, ref expresionTreeNode);
                    default:
                        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
                }
            }
            finally
            {
                //if (!(exp is MethodCallExpression) &&
                //    exp.NodeType != ExpressionType.Lambda &&
                //    exp.NodeType != ExpressionType.Quote &&
                //    exp.NodeType != ExpressionType.New &&
                //    !(exp is BinaryExpression))

                //if (exp is MemberExpression||
                //    exp is ParameterExpression||
                //    exp is ConstantExpression) 
                //        parent = new ExpresionTreeNode(exp, parent);

            }

        }

        /// <MetaDataID>{d8eca2b9-5bca-4718-9f71-3faa6fedcedd}</MetaDataID>
        private Expression VisitQuote(Expression exp)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{7813f615-2148-4557-b3f2-a3f93ffffd3a}</MetaDataID>
        protected virtual Expression VisitBinary(BinaryExpression b, ref  ExpressionTreeNode parent)
        {

            ExpressionTreeNode expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.BinaryExpression, b, parent, this);// new BinaryExpressionTreeNode(b, parent, LINQObjectQuery);
            ExpressionTreeNode tmpexpresionTreeNode = expresionTreeNode;
            Expression left = Visit(b.Left, ref tmpexpresionTreeNode);
            tmpexpresionTreeNode = expresionTreeNode;
            Expression right = Visit(b.Right, ref expresionTreeNode);
            if ((left == b.Left) && (right == b.Right))
            {
                return b;
            }
            return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }

        /// <MetaDataID>{6e0a8415-227a-4784-9345-1192249e6fa9}</MetaDataID>
        protected virtual Expression VisitArithmeticExpression(BinaryExpression b, ref  ExpressionTreeNode parent)
        {

            ExpressionTreeNode expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.ArithmeticExpression, b, parent, this);// new BinaryExpressionTreeNode(b, parent, LINQObjectQuery);
            ExpressionTreeNode tmpexpresionTreeNode = expresionTreeNode;
            Expression left = Visit(b.Left, ref tmpexpresionTreeNode);
            tmpexpresionTreeNode = expresionTreeNode;
            Expression right = Visit(b.Right, ref expresionTreeNode);
            if ((left == b.Left) && (right == b.Right))
            {
                return b;
            }
            return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }
        /// <MetaDataID>{99a4e120-3eba-4e76-ae99-41531537dde6}</MetaDataID>
        protected virtual Expression VisitLike(MethodCallExpression likeExpresion, ref  ExpressionTreeNode parent)
        {

            return likeExpresion;

            //ExpressionTreeNode expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.BinaryExpression, b, parent, LINQObjectQuery);// new BinaryExpressionTreeNode(b, parent, LINQObjectQuery);
            //ExpressionTreeNode tmpexpresionTreeNode = expresionTreeNode;
            //Expression left = Visit(b.Left, ref tmpexpresionTreeNode);
            //tmpexpresionTreeNode = expresionTreeNode;
            //Expression right = Visit(b.Right, ref expresionTreeNode);
            //if ((left == b.Left) && (right == b.Right))
            //{
            //    return b;
            //}
            //return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }

        /// <MetaDataID>{c4f29728-aacc-4408-8954-a5c74e5a9c20}</MetaDataID>
        protected virtual MemberBinding VisitBinding(MemberBinding binding, ref  ExpressionTreeNode parent)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment)binding, ref parent);

                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding)binding, ref parent);

                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding)binding, ref parent);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }
        private QueryExpressions.ParameterExpressionTreeNode GetParameter(ExpressionTreeNode expressionTreeNode)
        {
            while (expressionTreeNode is QueryExpressions.MemberAccessExpressionTreeNode)
                expressionTreeNode = expressionTreeNode.Parent;
            return expressionTreeNode as QueryExpressions.ParameterExpressionTreeNode;
        }

        /// <MetaDataID>{92c827de-a04e-4ca8-b3c8-a278daf1da5d}</MetaDataID>
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original, ref  ExpressionTreeNode parent)
        {
            List<MemberBinding> list = null;
            int num = 0x0;
            int count = original.Count;
            ExpressionTreeNode expresionTreeNode = parent;
            while (num < count)
            {
                expresionTreeNode = parent;
                MemberBinding item = VisitBinding(original[num], ref expresionTreeNode);
                if (list != null)
                {
                    list.Add(item);
                }
                else if (item != original[num])
                {
                    list = new List<MemberBinding>(count);
                    for (int i = 0; i < num; i++)
                    {
                        list.Add(original[i]);
                    }
                    list.Add(item);
                }
                num++;
            }
            if (list != null)
            {
                return list;
            }
            return original;
        }

        /// <MetaDataID>{eeb48f23-6d53-403d-a80b-e0ed51441970}</MetaDataID>
        protected virtual Expression VisitConditional(ConditionalExpression c, ref  ExpressionTreeNode parent)
        {
            ExpressionTreeNode expresionTreeNode = parent;
            Expression test = Visit(c.Test, ref expresionTreeNode);
            expresionTreeNode = parent;
            Expression ifTrue = Visit(c.IfTrue, ref expresionTreeNode);
            expresionTreeNode = parent;
            Expression ifFalse = Visit(c.IfFalse, ref expresionTreeNode);

            if (((test == c.Test) && (ifTrue == c.IfTrue)) && (ifFalse == c.IfFalse))
            {
                return c;
            }
            return Expression.Condition(test, ifTrue, ifFalse);
        }

        /// <MetaDataID>{d8a177a9-ecb9-4080-87d3-9d5bca837b1d}</MetaDataID>
        public System.Collections.Generic.List<DataNode> RootPaths = new List<DataNode>();




        /// <MetaDataID>{9c62d1c8-1ac7-49b5-ba71-5c9c69ec0490}</MetaDataID>
        protected virtual Expression VisitConstant(ConstantExpression c, ref  ExpressionTreeNode parent)
        {
            parent = CreateExpressionTreeNode(ExpressionTreeNodeType.Constant, c, parent, this);// new ConstantExpressionTreeNode(c, parent, LINQObjectQuery);
            return c;
        }

        /// <MetaDataID>{51cd8199-1d81-44b6-8374-9daba8389645}</MetaDataID>
        protected virtual ElementInit VisitElementInitializer(ElementInit initializer, ref  ExpressionTreeNode parent)
        {
            ReadOnlyCollection<Expression> arguments = VisitExpressionList(initializer.Arguments, ref parent);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }
            return initializer;
        }

        /// <MetaDataID>{c3de1ba2-968e-4a77-9e54-1644acb970e4}</MetaDataID>
        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original, ref  ExpressionTreeNode parent)
        {
            List<ElementInit> list = null;
            int num = 0;
            int count = original.Count;
            while (num < count)
            {
                ElementInit item = VisitElementInitializer(original[num], ref parent);
                if (list != null)
                {
                    list.Add(item);
                }
                else if (item != original[num])
                {
                    list = new List<ElementInit>(count);
                    for (int i = 0; i < num; i++)
                    {
                        list.Add(original[i]);
                    }
                    list.Add(item);
                }
                num++;
            }
            if (list != null)
            {
                return list;
            }
            return original;
        }

        /// <MetaDataID>{7dee50d5-9b6e-4e0a-8342-3e8f72ebbd35}</MetaDataID>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original, ref  ExpressionTreeNode parent)
        {
            List<Expression> list = null;
            int num = 0x0;
            int count = original.Count;


            ExpressionTreeNode expressionTreeNode = parent;

            while (num < count)
            {
                expressionTreeNode = parent;
                Expression item = Visit(original[num], ref expressionTreeNode);

                #region Assign Aliases
                //if (parent.Expression.NodeType == ExpressionType.New)
                //{
                //    // New type instance must be initialized through Constructor
                //    //Αντικαθηστά των κώδικα που κάνει την ιδια δουλεία στον constructor ExpressionTreeNode 
                //    if ((parent.Expression as NewExpression).Constructor.GetParameters().Length == (parent.Expression as NewExpression).Arguments.Count)
                //    {

                //        var args = (from arg in (parent.Expression as NewExpression).Arguments
                //                    where arg == item
                //                    select arg).ToArray();
                //        if (args.Length == 1)
                //        {
                //            int argIndex = (parent.Expression as NewExpression).Arguments.IndexOf(args[0]);

                //            if (GetParameter(expressionTreeNode) != null)
                //                GetParameter(expressionTreeNode).Alias = (parent.Expression as NewExpression).Constructor.GetParameters()[argIndex].Name;
                //            else
                //            {
                //                if (item.NodeType == ExpressionType.MemberInit)
                //                {
                //                    foreach (var memberExpresionTreeNode in parent.Nodes)
                //                    {
                //                        if (memberExpresionTreeNode is QueryExpressions.NewExpressionTreeNode && (memberExpresionTreeNode as QueryExpressions.NewExpressionTreeNode).MemberInitExpression == item)
                //                        {
                //                            memberExpresionTreeNode.Alias=(parent.Expression as NewExpression).Constructor.GetParameters()[argIndex].Name;
                //                            break;
                //                        }
                //                    }
                //                }
                //            }
                //        }

                //    }

                //}
                #endregion

                //.Operand as LambdaExpression).Body
                if (list != null)
                {
                    list.Add(item);

                }
                else if (item != original[num])
                {
                    list = new List<Expression>(count);
                    for (int i = 0; i < num; i++)
                    {
                        list.Add(original[i]);
                    }
                    list.Add(item);
                }
                num++;
            }


            if (parent.Expression.NodeType == ExpressionType.New)
            {
                // New type instance must be initialized through Constructor
                //Αντικαθηστά των κώδικα που κάνει την ιδια δουλεία στον constructor ExpressionTreeNode 
                if ((parent.Expression as NewExpression).Constructor.GetParameters().Length == (parent.Expression as NewExpression).Arguments.Count)
                {
                    int i = 0;
                    foreach (ExpressionTreeNode memberNode in parent.Nodes)
                    {
                        memberNode.Alias = (parent.Expression as NewExpression).Constructor.GetParameters()[i].Name;
                        i++;
                    }

                }

            }
            if (list != null)
            {
                return new ReadOnlyCollection<Expression>(list);
            }
            return original;
        }

        /// <MetaDataID>{6ae82e38-d3d5-4b92-81df-84d7ff655e6b}</MetaDataID>
        protected virtual Expression VisitInvocation(InvocationExpression iv, ref  ExpressionTreeNode parent)
        {
            ExpressionTreeNode expresionTreeNode = parent;
            IEnumerable<Expression> arguments = VisitExpressionList(iv.Arguments, ref expresionTreeNode);
            Expression expression = Visit(iv.Expression, ref expresionTreeNode);
            if ((arguments == iv.Arguments) && (expression == iv.Expression))
            {
                return iv;
            }
            return Expression.Invoke(expression, arguments);
        }

        /// <MetaDataID>{e18797f7-3fc6-41a3-9eee-c81d3527545c}</MetaDataID>
        protected virtual Expression VisitLambda(LambdaExpression lambda, ref  ExpressionTreeNode parent)
        {

            ExpressionTreeNode expresionTreeNode = parent;
            Expression body = Visit(lambda.Body, ref expresionTreeNode);
            if (lambda.Body.NodeType == ExpressionType.New || (parent.Expression.NodeType == ExpressionType.Call && ((parent.Expression as MethodCallExpression).Method.Name == "Where" || (parent.Expression as MethodCallExpression).Method.Name == "Select")))
            {
                /* temporary
                foreach (ParameterExpression parameterExpression in lambda.Parameters)
                {
                    expresionTreeNode = parent;
                    Visit(parameterExpression, ref expresionTreeNode);
                }
                expresionTreeNode = parent;
                 */
            }
            if (body != lambda.Body)
                return Expression.Lambda(lambda.Type, body, lambda.Parameters);
            return lambda;
        }

        /// <MetaDataID>{e8de4e34-bc47-481d-acf9-71c24806e7c0}</MetaDataID>
        protected virtual Expression VisitListInit(ListInitExpression init, ref  ExpressionTreeNode parent)
        {
            ExpressionTreeNode expresionTreeNode = parent;
            NewExpression newExpression = VisitNew(init.NewExpression, ref expresionTreeNode);
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(init.Initializers, ref expresionTreeNode);
            if ((newExpression == init.NewExpression) && (initializers == init.Initializers))
            {
                return init;
            }
            return Expression.ListInit(newExpression, initializers);
        }
        //DataNode GetDataNode(ParameterExpression expression)
        //{
        //    if (expression == null)
        //        return null;
        //    if (expression is ParameterExpression)
        //    {
        //        foreach (DataNode dataNode in Paths)
        //        {
        //            if (dataNode.Alias == (expression as ParameterExpression).Name)
        //                return dataNode;
        //        }
        //    }
        //    return null;
        //}


        //internal protected DataNode GetDataNode(MemberExpression m)
        //{
        //    DataNode dataNode = null;// GetDataNode(m as ParameterExpression);
        //    MainExpressionVisitor.MemberPaths.TryGetValue(m.ToString(), out dataNode);
        //    if (dataNode != null)
        //        return dataNode;

        //    DynamicType dynamicType = null;
        //    if (dataNode == null)
        //    {
        //        if (m.Expression is System.Linq.Expressions.MemberExpression)
        //        {
        //            if (MainExpressionVisitor.DynamicTypes.TryGetValue(m.Expression.Type, out dynamicType))
        //            {
        //                foreach (DataNode mDataNode in dynamicType.Members.Values)
        //                {
        //                    if (mDataNode.Alias == m.Member.Name)
        //                        return mDataNode;
        //                }

        //            }
        //            else
        //            {
        //                DynamicTypes.TryGetValue((m.Expression as System.Linq.Expressions.MemberExpression).Member.DeclaringType, out dynamicType);

        //                if (dynamicType != null)
        //                {
        //                    foreach (DataNode mDataNode in dynamicType.Members.Values)
        //                    {
        //                        if (mDataNode.Alias == (m.Expression as System.Linq.Expressions.MemberExpression).Member.Name)
        //                            return mDataNode;
        //                    }
        //                }
        //                if (dynamicType == null && (m.Expression as System.Linq.Expressions.MemberExpression).Member is System.Reflection.PropertyInfo)
        //                {
        //                    MainExpressionVisitor.DynamicTypes.TryGetValue(((m.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).DeclaringType, out dynamicType);



        //                    foreach (DataNode mDataNode in dynamicType.Members.Values)
        //                    {
        //                        if (mDataNode.Alias == (m.Expression as System.Linq.Expressions.MemberExpression).Member.Name)
        //                            return mDataNode;
        //                    }

        //                }

        //                if (dynamicType == null && (m.Expression as System.Linq.Expressions.MemberExpression).Member is System.Reflection.PropertyInfo)
        //                {
        //                    //Paths.TryGetValue(

        //                }


        //            }
        //        }
        //        else if (m.Expression.NodeType == ExpressionType.Parameter)
        //        {
        //            DynamicType dynamiType = null;
        //            if (MainExpressionVisitor.DynamicTypes.TryGetValue(m.Expression.Type, out dynamiType))
        //            {
        //                return null;
        //            }
        //            else
        //            {


        //                foreach (DataNode mDataNode in Paths)
        //                {
        //                    if (mDataNode.Alias == m.Member.Name)
        //                    {
        //                        return mDataNode;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return null;


        //}

        /// <MetaDataID>{fbab622e-47a4-4d1c-822b-ad483d1ddfd2}</MetaDataID>
        protected virtual Expression VisitMemberAccess(MemberExpression m, ref  ExpressionTreeNode parent)
        {
            Expression expression = null;
            if (m.NodeType == ExpressionType.Call && m.Member.Name == "Item")
            {
                parent = CreateExpressionTreeNode(ExpressionTreeNodeType.MemberAccess, m, parent, this);// new MemberAccessExpressionTreeNode(m, parent, LINQObjectQuery);
                expression = Visit(m.Expression, ref parent);
            }
            else
            {
                expression = Visit(m.Expression, ref parent);
                parent = CreateExpressionTreeNode(ExpressionTreeNodeType.MemberAccess, m, parent, this);// new MemberAccessExpressionTreeNode(m, parent, LINQObjectQuery);
            }

            if (expression != m.Expression)
            {
                return Expression.MakeMemberAccess(expression, m.Member);
            }
            return m;
        }


        /// <MetaDataID>{a3bdf79f-8762-44f2-9ab0-f69938afd0b0}</MetaDataID>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment, ref  ExpressionTreeNode parent)
        {
            Expression expression = Visit(assignment.Expression, ref parent);
            if (expression != assignment.Expression)
            {
                return Expression.Bind(assignment.Member, expression);
            }
            return assignment;
        }

        /// <MetaDataID>{06031360-a0bf-4f4c-8292-d2a4660d5683}</MetaDataID>
        protected virtual Expression VisitMemberInit(MemberInitExpression init, ref  ExpressionTreeNode parent)
        {
            //ExpressionTreeNode expresionTreeNode = parent;
            //IEnumerable<MemberBinding> bindings = VisitBindingList(init.Bindings, ref parent);
            //NewExpression newExpression = VisitNew(Expression.MemberInit(init.NewExpression, bindings).NewExpression, ref expresionTreeNode);

            //if ((newExpression == init.NewExpression) && (bindings == init.Bindings))
            //{
            //    return init;
            //}
            //return Expression.MemberInit(newExpression, bindings);

            ExpressionTreeNode expresionTreeNode = parent;
            NewExpression newExpression = VisitNew(init.NewExpression, ref expresionTreeNode);
            QueryExpressions.NewExpressionTreeNode newExpressionTreeNode = expresionTreeNode as QueryExpressions.NewExpressionTreeNode;

            if (newExpressionTreeNode != null)
                newExpressionTreeNode.MemberInitExpression = init;

            IEnumerable<MemberBinding> bindings = VisitBindingList(init.Bindings, ref expresionTreeNode);
            if (newExpressionTreeNode != null)
            {
                int i = 0;
                foreach (MemberBinding memberBinding in bindings)
                {
                    newExpressionTreeNode.Nodes[i].Alias = memberBinding.Member.Name;
                    i++;
                }
            }
            else
            {
            }

            if ((newExpression == init.NewExpression) && (bindings == init.Bindings))
            {
                return init;
            }
            return Expression.MemberInit(newExpression, bindings);

        }

        /// <MetaDataID>{65a5d5c2-da19-4ce1-b43e-a1fe704b19aa}</MetaDataID>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding, ref  ExpressionTreeNode parent)
        {
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(binding.Initializers, ref parent);
            if (initializers != binding.Initializers)
            {
                return Expression.ListBind(binding.Member, initializers);
            }
            return binding;
        }

        /// <MetaDataID>{657523ad-7a17-4dcf-944c-50e7fd27154b}</MetaDataID>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding, ref  ExpressionTreeNode parent)
        {
            IEnumerable<MemberBinding> bindings = VisitBindingList(binding.Bindings, ref parent);
            if (bindings != binding.Bindings)
            {
                return Expression.MemberBind(binding.Member, bindings);
            }
            return binding;
        }
        public enum ExpressionTreeNodeType
        {
            Select,
            SelectMany,
            GroupBy,
            OrderBy,
            OrderByDescending,
            Where,
            Parameter,
            MemberAccess,
            Constant,
            AggregateFunction,
            BinaryExpression,
            ArithmeticExpression,
            NewExpression,
            LikeExpression,
            ContainsAny,
            ContainsAll,
            EnumerableContains,
            ObjectCallExpression,
            FetchingPlan,
            RecursiveLoad,
            OfType,
            TypeAs
        }

        /// <MetaDataID>{8eaa8b65-4526-4016-8efa-bbced55e304f}</MetaDataID>
        internal protected abstract ExpressionTreeNode CreateExpressionTreeNode(ExpressionTreeNodeType expressionTreeNodeType, System.Linq.Expressions.Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator);

        //private System.Linq.Expressions.MemberExpression GetDerividedMemberAccess(Expression selector)
        //{
        //    if (selector is UnaryExpression)
        //        selector = (selector as UnaryExpression).Operand;

        //    if (selector is LambdaExpression)
        //        selector = (selector as LambdaExpression).Body;
        //    while ((selector is System.Linq.Expressions.MemberExpression) && !((selector as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0))
        //        selector = (selector as System.Linq.Expressions.MemberExpression).Expression;

        //    if (selector is System.Linq.Expressions.MemberExpression && ((selector as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0))
        //        return selector as System.Linq.Expressions.MemberExpression;

        //    return null;

        //}
        //private MethodCallExpression ExtendExpressionDerivedMembers(MethodCallExpression orgSelectManyExpression)
        //{
        //    Expression source = orgSelectManyExpression.Arguments[0];
        //    Expression selector = orgSelectManyExpression.Arguments[1];
        //    ParameterExpression selectorParam = ((selector as UnaryExpression).Operand as LambdaExpression).Parameters[0] as ParameterExpression;
        //    MemberExpression memberExpression = GetDerividedMemberAccess(selector);

        //    System.Linq.Expressions.MethodCallExpression derivedMemberExpresion = ((memberExpression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression as System.Linq.Expressions.MethodCallExpression;
        //    Expression[] arguments = derivedMemberExpresion.Arguments.ToArray();
        //    arguments[0] = MergeExpresions(derivedMemberExpresion.Arguments[0] as MethodCallExpression, source, memberExpression.Expression);
        //    arguments[1] = CopyExpression(arguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(arguments[0].Type));

        //    Expression results = null;

        //    #region Builds new result expression
        //    Expression resultDerivedMemberInitiator = ((arguments[2] as UnaryExpression).Operand as LambdaExpression).Body as MemberInitExpression;
        //    var mergeExpresionSelector = ((arguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
        //    var mergeExpresionSelectorSourceParam = ((arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
        //    var mergeExpresionSelectorParam = ((arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];
        //    List<string> path = new List<string>();
        //    GetDerivedMemberSourcePath(mergeExpresionSelectorSourceParam.Type, selectorParam, path);
        //    Expression resultSourcePropertyInitiator = mergeExpresionSelectorSourceParam;
        //    foreach (string memberName in path)
        //        resultSourcePropertyInitiator = Expression.PropertyOrField(resultSourcePropertyInitiator, memberName);

        //    Type resultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);
        //    var resultsBody = Expression.New(resultType.GetConstructors()[0], resultSourcePropertyInitiator, resultDerivedMemberInitiator);
        //    results = Expression.Quote(Expression.Lambda(resultsBody, mergeExpresionSelectorSourceParam, mergeExpresionSelectorParam));
        //    #endregion


        //    arguments[2] = results;





        //    System.Reflection.MethodInfo methodInfo = null;
        //    if (orgSelectManyExpression.Method.DeclaringType == typeof(Queryable))
        //        methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod(orgSelectManyExpression.Method.Name, ref arguments);
        //    else
        //        methodInfo = derivedMemberExpresion.Method;

        //    MethodCallExpression extendedExpresion = Expression.Call(methodInfo, arguments);
        //    return extendedExpresion;


        // //   Expression source = orgSelectManyExpression.Arguments[0];
        // //   Expression selector = orgSelectManyExpression.Arguments[1];
        // //   ParameterExpression selectorParam= ((selector as UnaryExpression).Operand as LambdaExpression).Parameters[0] as ParameterExpression;
        // //   MemberExpression memberExpression = GetDerividedMemberAccess(selector);

        // //   System.Linq.Expressions.MethodCallExpression derivedMemberExpresion = ((memberExpression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression as System.Linq.Expressions.MethodCallExpression;

        // //   Expression[] arguments = derivedMemberExpresion.Arguments.ToArray();
        // //   arguments[0] = MergeExpresions(derivedMemberExpresion.Arguments[0] as MethodCallExpression, source, memberExpression.Expression);
        // //   arguments[1] = CopyExpression(arguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(arguments[0].Type));

        // //   Expression results = null;

        // //   #region Builds new result expression
        // //   Expression resultDerivedMemberInitiator = ((arguments[2] as UnaryExpression).Operand as LambdaExpression).Body as MemberInitExpression;
        // //   var mergeExpresionSelectorSourceParam = ((arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
        // //   List<string> path = new List<string>();
        // //   GetDerivedMemberSourcePath(mergeExpresionSelectorSourceParam.Type, selectorParam, path);
        // //   Expression resultSourcePropertyInitiator = mergeExpresionSelectorSourceParam;
        // //   foreach (string memberName in path)
        // //       resultSourcePropertyInitiator = Expression.PropertyOrField(resultSourcePropertyInitiator, memberName);

        // //   Type resultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);
        // //   var resultsBody = Expression.New(resultType.GetConstructors()[0], resultSourcePropertyInitiator, resultDerivedMemberInitiator);
        // //   results = Expression.Quote(Expression.Lambda(resultsBody, ((arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters.ToArray()));
        // //   #endregion


        // //   arguments[2] = results;


        // //   System.Reflection.MethodInfo methodInfo = null;
        // //   if (orgSelectManyExpression.Method.DeclaringType == typeof(Queryable))
        // //       methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod(orgSelectManyExpression.Method.Name, ref arguments);
        // //   else
        // //       methodInfo = derivedMemberExpresion.Method;

        // //MethodCallExpression   extendedExpresion = Expression.Call(methodInfo, arguments);

        // //   return extendedExpresion;
        //}
        //private void GetDerivedMemberSourcePath(Type type, Expression derivedMemberSelectorSource, List<string> path)
        //{
        //    foreach (var property in type.GetProperties())
        //    {
        //        if (property.Name.IndexOf("<>h__TransparentIdentifier") == 0 && property.PropertyType.Name.IndexOf("DeriveMember") != property.PropertyType.Name.Length - "DeriveMember".Length - 1)
        //        {
        //            path.Add(property.Name);
        //            GetDerivedMemberSourcePath(property.PropertyType, derivedMemberSelectorSource, path);
        //            return;
        //        }
        //    }
        //    string propertyName = null;
        //    Type propertyType = null;
        //    if (derivedMemberSelectorSource is MemberExpression)
        //    {
        //        propertyName = (derivedMemberSelectorSource as MemberExpression).Member.Name;
        //        propertyType = (derivedMemberSelectorSource as MemberExpression).Type;
        //    }
        //    if (derivedMemberSelectorSource is ParameterExpression)
        //    {
        //        propertyName = (derivedMemberSelectorSource as ParameterExpression).Name;
        //        propertyType = (derivedMemberSelectorSource as ParameterExpression).Type;
        //    }
        //    foreach (var property in type.GetProperties())
        //    {
        //        if (propertyType == property.PropertyType && propertyName == property.Name)
        //        {
        //            path.Add(property.Name);
        //            break;
        //        }
        //    }



        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="derivedMemberExpresion"></param>
        ///// <param name="source">
        ///// Derived member SelectMany source
        ///// </param>
        ///// <param name="selectorSourceParm">
        ///// Derived member selector source parameter
        ///// </param>
        ///// <returns></returns>
        //private Expression MergeExpresions(MethodCallExpression derivedMemberExpresion, Expression source, Expression selectorSourceParm)
        //{
        //    if (derivedMemberExpresion.Arguments[0] is MethodCallExpression)
        //    {
        //        System.Linq.Expressions.Expression extendedExpresionSource = MergeExpresions(derivedMemberExpresion.Arguments[0] as MethodCallExpression, source, selectorSourceParm);

        //        System.Diagnostics.Debug.WriteLine("Expression replace");
        //        System.Diagnostics.Debug.WriteLine(derivedMemberExpresion.ToString());
        //        System.Linq.Expressions.Expression extendedExpresion = CopyMethodCallExpression(derivedMemberExpresion, extendedExpresionSource);

        //        //System.Linq.Expressions.Expression extendedExpresion= System.Linq.Expressions.Expression.Call((derivedMemberExpresion as MethodCallExpression).Method, arguments);
        //        System.Diagnostics.Debug.WriteLine(extendedExpresion.ToString());
        //        return extendedExpresion;
        //    }
        //    else
        //    {
        //        Expression derivedExpressionSelector = derivedMemberExpresion.Arguments[1];
        //        if (derivedExpressionSelector is UnaryExpression)
        //            derivedExpressionSelector = (derivedExpressionSelector as UnaryExpression).Operand;

        //        if (derivedExpressionSelector is LambdaExpression)
        //            derivedExpressionSelector = (derivedExpressionSelector as LambdaExpression).Body;
        //        List<MemberExpression> path = new List<MemberExpression>();
        //        while (derivedExpressionSelector is MemberExpression)
        //        {
        //            path.Insert(0, derivedExpressionSelector as MemberExpression);
        //            derivedExpressionSelector = (derivedExpressionSelector as MemberExpression).Expression;
        //        }
        //        bool hasTheSameSourceParameterName = GetParameter(selectorSourceParm).Name == GetParameter(derivedExpressionSelector).Name;

        //        var derivedMemberSourceParam = selectorSourceParm;
        //        var mergeSelectorBody = selectorSourceParm;
        //        foreach (var memberExpresion in path)
        //            mergeSelectorBody = Expression.PropertyOrField(mergeSelectorBody, memberExpresion.Member.Name);


        //        Type resultType = null;
        //        if (!hasTheSameSourceParameterName)
        //        {
        //            Dictionary<string, Type> dynamicTypeParameters = new Dictionary<string, Type>();
        //            dynamicTypeParameters.Add(GetParameter(selectorSourceParm).Name, GetParameter(selectorSourceParm).Type);
        //            foreach (var parameter in ((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters)
        //                dynamicTypeParameters.Add(parameter.Name, parameter.Type);


        //            var mergeSourceParam = Expression.Parameter(GetParameter(selectorSourceParm).Type, GetParameter(selectorSourceParm).Name);


        //            var mergeSelectorParam = ((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];
        //            resultType = LinqRuntimeTypeBuilder.GetDynamicType(((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body.Type.Name + "DeriveMember", dynamicTypeParameters);
        //            var mergeResultsBody = Expression.New(resultType.GetConstructors()[0], mergeSourceParam, derivedMemberSourceParam, mergeSelectorParam);
        //            var mergeResults = Expression.Lambda(mergeResultsBody, mergeSourceParam, mergeSelectorParam);

        //            MethodCallExpression extendedExpresion = LinqRuntimeTypeBuilder.GetSelectManyExprsession(source, Expression.Lambda(mergeSelectorBody, mergeSourceParam), mergeResults);

        //            return extendedExpresion;
        //            //var results = Expression.Lambda(resultsBody, sourceParam, selector);
        //        }

        //        // MethodCallExpression methodCallExpression = LinqExt.LinqRuntimeTypeBuilder.GetSelectManyExprsession(source, selector, results);
        //    }

        //    return null;

        //}
        //Expression CopyExpression(Expression expression, Type oldSourceType, Type newSourceType)
        //{
        //    if (expression is MethodCallExpression)
        //    {
        //        var arguments = (expression as MethodCallExpression).Arguments.ToArray();
        //        int i = 0;
        //        foreach (var argument in arguments)
        //            arguments[i++] = CopyExpression(argument, oldSourceType, newSourceType);
        //        return Expression.Call((expression as MethodCallExpression).Method, arguments);
        //    }
        //    if (expression is UnaryExpression)
        //    {
        //        return Expression.Quote(CopyExpression((expression as UnaryExpression).Operand, oldSourceType, newSourceType));
        //    }
        //    if (expression is LambdaExpression)
        //    {


        //        Expression body = CopyExpression((expression as LambdaExpression).Body, oldSourceType, newSourceType);
        //        var parameters = (expression as LambdaExpression).Parameters.ToArray();
        //        int i = 0;
        //        foreach (var parameter in parameters)
        //            parameters[i++] = CopyExpression(parameter, oldSourceType, newSourceType) as ParameterExpression;

        //        Expression copyExpression = Expression.Lambda(body, parameters);
        //        return copyExpression;


        //    }
        //    if (expression is BinaryExpression)
        //    {
        //        Expression left = CopyExpression((expression as BinaryExpression).Left, oldSourceType, newSourceType);
        //        Expression Right = CopyExpression((expression as BinaryExpression).Right, oldSourceType, newSourceType);
        //        BinaryExpression binaryExpression = Expression.MakeBinary(expression.NodeType, left, Right);
        //        return binaryExpression;


        //    }
        //    if (expression is MemberExpression)
        //    {
        //        MemberExpression orgMemberExpression = expression as MemberExpression;
        //        List<MemberExpression> path = new List<MemberExpression>();
        //        while (!(expression is ParameterExpression))
        //        {
        //            path.Insert(0, expression as MemberExpression);
        //            expression = (expression as MemberExpression).Expression;
        //        }
        //        ParameterExpression parameterExpression = expression as ParameterExpression;
        //        if (parameterExpression.Type == oldSourceType)
        //        {
        //            Expression expressionCopy = Expression.Parameter(newSourceType, parameterExpression.Name);

        //            foreach (var memberExpresion in path)
        //                expressionCopy = Expression.PropertyOrField(expressionCopy, memberExpresion.Member.Name);
        //            return expressionCopy;
        //        }
        //        else
        //            return orgMemberExpression;


        //    }
        //    if (expression is ParameterExpression)
        //    {
        //        ParameterExpression parameterExpression = expression as ParameterExpression;
        //        if (parameterExpression.Type == oldSourceType)
        //        {
        //            Expression expressionCopy = Expression.Parameter(newSourceType, parameterExpression.Name);
        //            return expressionCopy;
        //        }
        //        else
        //            return parameterExpression;
        //    }
        //    if (expression is ConstantExpression)
        //        return expression;

        //    if (expression is MemberInitExpression)
        //    {
        //        var memberBindigs = (expression as MemberInitExpression).Bindings.ToArray();
        //        int i = 0;
        //        foreach (MemberAssignment memberBindig in memberBindigs)
        //            memberBindigs[i++] = Expression.Bind(memberBindig.Member, CopyExpression(memberBindig.Expression, oldSourceType, newSourceType));
        //        Expression expressionCopy = Expression.MemberInit((expression as MemberInitExpression).NewExpression, memberBindigs);
        //        return expressionCopy;
        //    }
        //    if (expression is NewExpression)
        //    {

        //        Dictionary<string, Type> dynamicTypeParameters = new Dictionary<string, Type>();

        //        var arguments = (expression as NewExpression).Arguments.ToArray();
        //        int i = 0;
        //        foreach (var argument in arguments)
        //        {
        //            arguments[i] = CopyExpression(argument, oldSourceType, newSourceType);
        //            dynamicTypeParameters.Add((expression as NewExpression).Constructor.GetParameters()[i].Name, GetParameter(arguments[i]).Type);
        //            i++;
        //        }

        //        Type resultType = LinqRuntimeTypeBuilder.GetDynamicType((expression as NewExpression).Type.Name + "DeriveMember", dynamicTypeParameters);
        //        Expression expressionCopy = Expression.New(resultType.GetConstructors()[0], arguments);
        //        return expressionCopy;



        //    }


        //    //switch (expression.NodeType)
        //    //{

        //    //    case ExpressionType.Add:
        //    //    case ExpressionType.AddChecked:
        //    //    case ExpressionType.Multiply:
        //    //    case ExpressionType.MultiplyChecked:
        //    //    case ExpressionType.Subtract:
        //    //    case ExpressionType.SubtractChecked:
        //    //    case ExpressionType.Power:
        //    //    case ExpressionType.Divide:
        //    //    case ExpressionType.And:
        //    //    case ExpressionType.AndAlso:
        //    //    case ExpressionType.ArrayIndex:
        //    //    case ExpressionType.Coalesce:
        //    //    case ExpressionType.Equal:
        //    //    case ExpressionType.ExclusiveOr:
        //    //    case ExpressionType.GreaterThan:
        //    //    case ExpressionType.GreaterThanOrEqual:
        //    //    case ExpressionType.LeftShift:
        //    //    case ExpressionType.LessThan:
        //    //    case ExpressionType.LessThanOrEqual:
        //    //    case ExpressionType.Modulo:
        //    //    case ExpressionType.NotEqual:
        //    //    case ExpressionType.Or:
        //    //    case ExpressionType.OrElse:
        //    //    case ExpressionType.RightShift:
        //    //        return CopyBinary((BinaryExpression)expression, sourceType);
        //    //    case ExpressionType.ArrayLength:
        //    //    case ExpressionType.Convert:
        //    //    case ExpressionType.ConvertChecked:
        //    //    case ExpressionType.Negate:
        //    //    case ExpressionType.NegateChecked:
        //    //    case ExpressionType.Not:
        //    //    case ExpressionType.Quote:
        //    //    case ExpressionType.TypeAs:
        //    //        return VisitUnary((UnaryExpression)exp, ref parent);
        //    //    case ExpressionType.Call:
        //    //        return VisitMethodCall((MethodCallExpression)exp, ref parent);

        //    //    case ExpressionType.Conditional:
        //    //        return VisitConditional((ConditionalExpression)exp, ref parent);

        //    //    case ExpressionType.Constant:
        //    //        return VisitConstant((ConstantExpression)exp, ref parent); ;

        //    //    case ExpressionType.Invoke:
        //    //        return VisitInvocation((InvocationExpression)exp, ref parent);

        //    //    case ExpressionType.Lambda:
        //    //        return VisitLambda((LambdaExpression)exp, ref parent);

        //    //    case ExpressionType.ListInit:
        //    //        return VisitListInit((ListInitExpression)exp, ref parent);

        //    //    case ExpressionType.MemberAccess:
        //    //        return VisitMemberAccess((MemberExpression)exp, ref parent);

        //    //    case ExpressionType.MemberInit:
        //    //        return VisitMemberInit((MemberInitExpression)exp, ref parent);

        //    //    case ExpressionType.New:
        //    //        return VisitNew((NewExpression)exp, ref parent);

        //    //    case ExpressionType.NewArrayInit:
        //    //    case ExpressionType.NewArrayBounds:
        //    //        return VisitNewArray((NewArrayExpression)exp, ref parent);

        //    //    case ExpressionType.Parameter:
        //    //        return VisitParameter((ParameterExpression)exp, ref parent);


        //    //    case ExpressionType.TypeIs:
        //    //        return VisitTypeIs((TypeBinaryExpression)exp, ref expresionTreeNode);
        //    //    default:
        //    //        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
        //    //}

        //    return null;
        //}
        //private Expression CopyMethodCallExpression(MethodCallExpression derivedMemberExpresion, Expression extendedExpresionSource)
        //{

        //    Expression[] arguments = derivedMemberExpresion.Arguments.ToArray();
        //    arguments[0] = extendedExpresionSource;
        //    for (int i = 1; i < arguments.Length; i++)
        //        arguments[i] = CopyExpression(arguments[i], OOAdvantech.TypeHelper.GetElementType(derivedMemberExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(extendedExpresionSource.Type));


        //    System.Reflection.MethodInfo methodInfo = null;
        //    if (derivedMemberExpresion.Method.DeclaringType == typeof(Queryable))
        //        methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod(derivedMemberExpresion.Method.Name, ref arguments);
        //    else
        //        methodInfo = derivedMemberExpresion.Method;

        //    return Expression.Call(methodInfo, arguments);

        //}
        //private ParameterExpression GetParameter(Expression expression)
        //{
        //    while (expression is MemberExpression)
        //        expression = (expression as MemberExpression).Expression;
        //    return (expression as ParameterExpression);
        //}

        /// <MetaDataID>{d46b35d9-3195-434f-8bda-d406f824b5af}</MetaDataID>
        protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExpression, ref  ExpressionTreeNode parent)
        {


            ExpressionTreeNode expresionTreeNode = null;
            if (methodCallExpression.Method.Name == "Where" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.Where, methodCallExpression, parent, this);// new WhereExpressionTreeNode(m, parent,LINQObjectQuery);

            else if (methodCallExpression.Method.Name == "Select" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.Select, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "OfType" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.OfType, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "SelectMany" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
            {
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.SelectMany, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);
                //Expression selector = methodCallExpression.Arguments[1];
                //MemberExpression memberExpression = GetDerividedMemberAccess(selector);
                //if (memberExpression != null)
                //{
                //    MethodCallExpression extendedmethodCallExpression = ExtendExpressionDerivedMembers(methodCallExpression);
                //    //ExpressionTreeNode updateExpressionTreeNode = ExpressionTreeNodes[methodCallExpression];
                //    //ExpressionTreeNodes.Remove(methodCallExpression);
                //    //updateExpressionTreeNode.Expression = extendedmethodCallExpression;
                //    //expresionTreeNode.
                //}
            }
            else if (methodCallExpression.Method.Name == "GroupBy" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.GroupBy, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Like" && methodCallExpression.Method.DeclaringType == typeof(System.Linq.OOAdvantechExtraOperators))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.LikeExpression, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "ContainsAny" && methodCallExpression.Method.DeclaringType == typeof(System.Linq.OOAdvantechExtraOperators))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.ContainsAny, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "ContainsAll" && methodCallExpression.Method.DeclaringType == typeof(System.Linq.OOAdvantechExtraOperators))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.ContainsAll, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Sum" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.AggregateFunction, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Average" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.AggregateFunction, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Min" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.AggregateFunction, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Max" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.AggregateFunction, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Count" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.AggregateFunction, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Contains" && methodCallExpression.Method.DeclaringType.GetInterface("ICollection`1") != null)
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.EnumerableContains, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Contains" && methodCallExpression.Method.DeclaringType.GetInterface("ICollection`1") == null)
                throw new ExpressionException("You can use 'Contains' expression only in Generic collections");
            else if ((methodCallExpression.Method.Name == "OrderBy" || methodCallExpression.Method.Name == "ThenBy") && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.OrderBy, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if (methodCallExpression.Method.Name == "Fetching")
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.FetchingPlan, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
            else if ((methodCallExpression.Method.Name == "OrderByDescending" || methodCallExpression.Method.Name == "ThenByDescending") && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.OrderByDescending, methodCallExpression, parent, this);
            else if (methodCallExpression.Method.Name == "Recursive" && methodCallExpression.Method.DeclaringType == typeof(System.Linq.OOAdvantechExtraOperators))
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.RecursiveLoad, methodCallExpression, parent, this);
            else if (methodCallExpression.Method.Name == "Item" && methodCallExpression.Arguments.Count == 1)
            {
                Visit(methodCallExpression.Arguments[0], ref parent);
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.ObjectCallExpression, methodCallExpression, parent, this);//  new SelectExpressionTreeNode(m, parent, LINQObjectQuery);
                return methodCallExpression;
            }
            else
                expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.ObjectCallExpression, methodCallExpression, parent, this);// new SelectManyExpressionTreeNode(m, parent, LINQObjectQuery);


            ExpressionTreeNode tmpExpresionTreeNode = expresionTreeNode;
            if (methodCallExpression.Object != null)
                Visit(methodCallExpression.Object, ref tmpExpresionTreeNode);
            tmpExpresionTreeNode = expresionTreeNode;
            int i = 0;
            foreach (Expression exp in methodCallExpression.Arguments)
            {
                Visit(exp, ref tmpExpresionTreeNode);
                i++;
                //if (expresionTreeNode.ReplacingExpressionTreeNode != null)
                //    expresionTreeNode = expresionTreeNode.ReplacingExpressionTreeNode;

                if (methodCallExpression.Arguments.Count > i && ((methodCallExpression.Arguments[i] is UnaryExpression && (methodCallExpression.Arguments[i] as UnaryExpression).Operand is LambdaExpression) || methodCallExpression.Arguments[i] is LambdaExpression))
                {
                    if (expresionTreeNode.Nodes.Count > 0 &&
                        (expresionTreeNode.Nodes[i - 1] is QueryExpressions.ParameterExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.SelectManyExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.SelectExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.GroupByExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.ConstantExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.OfTypeExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.TypeAsExpressionTreeNode ||
                        expresionTreeNode.Nodes[i - 1] is QueryExpressions.WhereExpressionTreeNode))
                    {
                        if (methodCallExpression.Arguments[i] is UnaryExpression)
                        {
                            if (((methodCallExpression.Arguments[i] as UnaryExpression).Operand as LambdaExpression).Parameters.Count > i - 1)
                                expresionTreeNode.Nodes[i - 1].Alias = ((methodCallExpression.Arguments[i] as UnaryExpression).Operand as LambdaExpression).Parameters[i - 1].Name;
                        }
                        else
                        {
                            if ((methodCallExpression.Arguments[i] as LambdaExpression).Parameters.Count > i - 1)
                                expresionTreeNode.Nodes[i - 1].Alias = (methodCallExpression.Arguments[i] as LambdaExpression).Parameters[i - 1].Name;
                        }
                    }
                    else
                    {
                    }
                }

                tmpExpresionTreeNode = expresionTreeNode;

            }



            return methodCallExpression;


            //if ((instance == m.Object) && (arguments == m.Arguments))
            //{
            //    return m;
            //}
            //return Expression.Call(instance, m.Method, arguments);
        }
        //public readonly System.Collections.Generic.Dictionary<DataNode,List< System.Reflection.PropertyInfo >> NewTypesPropertiesDataNode = new Dictionary<DataNode,List<System.Reflection.PropertyInfo>>();



        /// <MetaDataID>{7e402bad-6606-47aa-83ee-d7bfc3516e3a}</MetaDataID>
        protected virtual NewExpression VisitNew(NewExpression nex, ref  ExpressionTreeNode parent)
        {
            parent = CreateExpressionTreeNode(ExpressionTreeNodeType.NewExpression, nex, parent, this);
            ExpressionTreeNode expresionTreeNode = parent;

            IEnumerable<Expression> arguments = VisitExpressionList(nex.Arguments, ref expresionTreeNode);
            if (arguments == nex.Arguments)
            {
                return nex;
            }
            if (nex.Members != null)
            {
                return Expression.New(nex.Constructor, arguments, nex.Members);
            }
            return Expression.New(nex.Constructor, arguments);
        }

        /// <MetaDataID>{0774f3af-4349-40e0-b0cd-ed3f69fb09de}</MetaDataID>
        protected virtual Expression VisitNewArray(NewArrayExpression na, ref  ExpressionTreeNode parent)
        {
            ExpressionTreeNode expresionTreeNode = parent;
            IEnumerable<Expression> initializers = VisitExpressionList(na.Expressions, ref expresionTreeNode);
            if (initializers == na.Expressions)
            {
                return na;
            }
            if (na.NodeType == ExpressionType.NewArrayInit)
            {
                return Expression.NewArrayInit(na.Type.GetElementType(), initializers);
            }
            return Expression.NewArrayBounds(na.Type.GetElementType(), initializers);
        }

        /// <MetaDataID>{0d729cbd-ee99-4980-ba3b-87e6c0144855}</MetaDataID>
        protected virtual Expression VisitParameter(ParameterExpression p, ref  ExpressionTreeNode parent)
        {
            parent = CreateExpressionTreeNode(ExpressionTreeNodeType.Parameter, p, parent, this);//  new ParameterExpressionTreeNode(p, parent, LINQObjectQuery);
            //if (RootPaths.ContainsKey(p.Type))
            //    RootPaths[p.Type].Alias = p.Name;
            return p;
        }

        /// <MetaDataID>{b42dced8-633d-4c15-b4d0-ad3f253e6cba}</MetaDataID>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression b, ref  ExpressionTreeNode parent)
        {


            ExpressionTreeNode expresionTreeNode = CreateExpressionTreeNode(ExpressionTreeNodeType.BinaryExpression, b, parent, this);// new BinaryExpressionTreeNode(b, parent, LINQObjectQuery);
            ExpressionTreeNode tmpexpresionTreeNode = expresionTreeNode;
            Expression left = Visit(b.Expression, ref tmpexpresionTreeNode);
            Expression expression = Visit(b.Expression, ref parent);
            if (expression != b.Expression)
            {
                return Expression.TypeIs(expression, b.TypeOperand);
            }
            return b;
        }

        /// <MetaDataID>{7b55891c-3ca3-4e94-a2dc-3c8c546949ff}</MetaDataID>
        protected virtual Expression VisitUnary(UnaryExpression u, ref  ExpressionTreeNode parent)
        {
            Expression operand = Visit(u.Operand, ref parent);
            if (operand != u.Operand)
            {
                return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
            }
            return u;
        }


        /// <MetaDataID>{1a460eb3-99de-4b57-acec-bf05d7a33252}</MetaDataID>
        internal void MergeDataNodeTree(DataNode dataNode, Dictionary<DataNode, DataNode> replacedDataNodes)
        {
            //if (dataNode.Temporary)
            {
                foreach (DataNode tmpDataNode in new System.Collections.Generic.List<DataNode>(dataNode.SubDataNodes))
                {
                    //if (dataNode.Type == DataNode.DataNodeType.Key)
                    //    tmpDataNode.Temporary = true;

                    DataNode subDataNode = tmpDataNode;
                    if (subDataNode.Temporary)
                    {
                        DataNode corespondingDataNode = null;
                        if (dataNode.Type == DataNode.DataNodeType.Key)
                            corespondingDataNode = Translators.QueryTranslator.GetCorrespondingDataNode(dataNode.ParentDataNode, subDataNode);
                        else
                            corespondingDataNode = Translators.QueryTranslator.GetCorrespondingDataNode(dataNode, subDataNode);

                        if (corespondingDataNode != null && corespondingDataNode != subDataNode)
                        {

                            foreach (DataNode mergSubDataNode in new System.Collections.Generic.List<DataNode>(subDataNode.SubDataNodes))
                            {
                                if (!corespondingDataNode.IsSameOrParentDataNode(mergSubDataNode))
                                    mergSubDataNode.ParentDataNode = corespondingDataNode;
                                /* προσορινά
                                if (GetAliasEnumerator(subDataNode.Name) == null && corespondingDataNode.Temporary == false)
                                    mergSubDataNode.Temporary = false;
                                  */
                            }
                            if (corespondingDataNode.IsParentDataNode(subDataNode))
                                subDataNode.SubDataNodes[0].ParentDataNode = subDataNode.ParentDataNode;
                            subDataNode.SubDataNodes.Clear();
                            subDataNode.ParentDataNode = null; ;
                            //dataNode.SubDataNodes.Remove(subDataNode);
                            replacedDataNodes[subDataNode] = corespondingDataNode;
                            if (subDataNode.ParticipateInWereClause)
                            {
                                corespondingDataNode.ParticipateInWereClause = true;
                            }
                            //if (subDataNode.ParticipateInSelectClause)
                            //{
                            //    LINQObjectQuery.RemoveSelectListItem(subDataNode);
                            //    LINQObjectQuery.AddSelectListItem(corespondingDataNode);
                            //}

                            if (subDataNode.Alias != null)
                            {
                                ///if (corespondingDataNode.Alias == null)
                                corespondingDataNode.Alias = subDataNode.Alias;
                                //else
                                //    if(corespondingDataNode.Alias != subDataNode.Alias)
                                //    throw new System.Exception("MultipleAlias");//MultipleAlias[subDataNode.Alias] = corespondingDataNode;
                            }
                            foreach (string alias in subDataNode.Aliases)
                            {
                                corespondingDataNode.Alias = alias;

                            }

                            subDataNode = corespondingDataNode;

                        }
                    }
                    //**************//
                    MergeDataNodeTree(subDataNode, replacedDataNodes);
                    continue;
                    //***************//

                    foreach (DataNode corespondingDataNode in new System.Collections.Generic.List<DataNode>(dataNode.SubDataNodes))
                    {
                        if (subDataNode.ParentDataNode == dataNode && corespondingDataNode.Name == subDataNode.Name && corespondingDataNode != subDataNode)
                        {

                            foreach (DataNode mergSubDataNode in new System.Collections.Generic.List<DataNode>(subDataNode.SubDataNodes))
                            {
                                mergSubDataNode.ParentDataNode = corespondingDataNode;
                                if (corespondingDataNode.Temporary == false)
                                    mergSubDataNode.Temporary = false;


                            }
                            subDataNode.SubDataNodes.Clear();
                            //dataNode.SubDataNodes.Remove(subDataNode);
                            subDataNode.ParentDataNode = null;
                            replacedDataNodes[subDataNode] = corespondingDataNode;
                            if (subDataNode.ParticipateInWereClause)
                            {
                                corespondingDataNode.ParticipateInWereClause = true;
                            }
                            //if (subDataNode.ParticipateInSelectClause)
                            //{
                            //    LINQObjectQuery.RemoveSelectListItem(subDataNode);
                            //    LINQObjectQuery.AddSelectListItem(corespondingDataNode);
                            //}
                            if (subDataNode.Alias != null)
                            {
                                if (corespondingDataNode.Alias == null)
                                    corespondingDataNode.Alias = subDataNode.Alias;
                                //else
                                //    throw new System.Exception("MultipleAlias");//MultipleAlias[subDataNode.Alias] = corespondingDataNode;
                            }

                        }

                    }


                    MergeDataNodeTree(subDataNode, replacedDataNodes);
                }
            }
        }

        /// <MetaDataID>{9cb90fda-7de7-46b1-8937-53312ae73318}</MetaDataID>
        internal void RemoveAllTemporaryNodes(DataNode dataNode, Dictionary<DataNode, DataNode> replacedDataNodes)
        {


            if (dataNode.Type == DataNode.DataNodeType.Key)
            {
                //foreach (DataNode keySubDataNode in new List<DataNode>(dataNode.SubDataNodes))
                //{
                //    foreach (DataNode groupByKeyDataNode in dataNode.ParentDataNode.GroupKeyDataNodes)
                //    {
                //        if(groupByKeyDataNode.HasAlias(keySubDataNode.Name))
                //        {
                //            replacedDataNodes[keySubDataNode] = groupByKeyDataNode;
                //            break;
                //        }
                //    }

                //}
                //    keySubDataNode.ParentDataNode = dataNode.ParentDataNode;
                //replacedDataNodes[dataNode] = dataNode.ParentDataNode;

            }
            // Temporary out
            if (dataNode.Temporary && dataNode.ParentDataNode != null)//||dataNode.Type == DataNode.DataNodeType.Key)
            {
                replacedDataNodes[dataNode] = dataNode.ParentDataNode;

            }

            foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.SubDataNodes))
            {
                DataNode tmpDataNode = subDataNode;
                RemoveAllTemporaryNodes(tmpDataNode, replacedDataNodes);
            }


            // dataNode.ParentDataNode = null;


            if (DataNodeTreeSimplification != null)
                DataNodeTreeSimplification(replacedDataNodes);


            foreach (DataNode removedDataNode in replacedDataNodes.Keys)
                removedDataNode.ParentDataNode = null;


            foreach (string alias in new System.Collections.Generic.List<string>(dataNode.Aliases))
            {
                if (alias != null && alias.IndexOf("<>h__Tran") != -1)
                {
                    dataNode.Aliases.Remove(alias);
                    dataNode.Alias = null;
                    if (dataNode.Aliases.Count > 0)
                        dataNode.Alias = dataNode.Aliases[dataNode.Aliases.Count - 1];


                }
            }
            return;

            replacedDataNodes.Clear();
            if (dataNode.Type == DataNode.DataNodeType.Group)
            {
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Key)
                    {
                        replacedDataNodes[subDataNode] = dataNode;
                        //foreach (DataNode keySubDataNode in subDataNode.SubDataNodes)
                        //{

                        //    foreach (DataNode groupedDataNode in dataNode.GroupKeyDataNodes)
                        //    {

                        //        DataNode actualDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(groupedDataNode, keySubDataNode.Name);
                        //        if (actualDataNode != null)
                        //        {
                        //            replacedDataNodes[keySubDataNode] = actualDataNode;
                        //            foreach (DataNode mDataNode in new List<DataNode>( keySubDataNode.SubDataNodes))
                        //            {
                        //                mDataNode.ParentDataNode = actualDataNode;
                        //            }
                        //            break;
                        //        }
                        //    }
                        //}
                        break;
                    }

                }
                if (DataNodeTreeSimplification != null)
                    DataNodeTreeSimplification(replacedDataNodes);

                foreach (DataNode removedDataNode in replacedDataNodes.Keys)
                    removedDataNode.ParentDataNode = null;

            }


        }


        /// <MetaDataID>{2b10a4ec-61eb-49f7-a77b-8cbe7e057337}</MetaDataID>
        internal void RemoveTemporaryNodes(ref DataNode dataNode, Dictionary<DataNode, DataNode> replacedDataNodes)
        {

            Translators.QueryTranslator.ShowDataNodePathsInOutLog(dataNode.HeaderDataNode);

            //while (dataNode.Temporary )
            //{
            //    if (dataNode.SubDataNodes.Count == 1)
            //    {
            //       // Translators.QueryTranslator.ShowDataNodePathsInOutLog(dataNode);
            //        replacedDataNodes[dataNode] = dataNode.SubDataNodes[0];
            //        dataNode = dataNode.SubDataNodes[0];
            //    }
            //    else if (dataNode.ParentDataNode != null)
            //    {
            //        replacedDataNodes[dataNode] = dataNode.ParentDataNode;
            //        foreach (DataNode subDataNode in dataNode.SubDataNodes)
            //        {
            //            DataNode tmpDataNode = subDataNode;
            //            RemoveTemporaryNodes(ref tmpDataNode, replacedDataNodes);
            //        }
            //    }
            //}
            //// dataNode.ParentDataNode = null;




            // Temporary out
            while (dataNode.Temporary && dataNode.SubDataNodes.Count == 1)
            {
                replacedDataNodes[dataNode] = dataNode.SubDataNodes[0];
                //dataNode.ParentDataNode = null;
                dataNode = dataNode.SubDataNodes[0];
            }
            // dataNode.ParentDataNode = null;


            if (DataNodeTreeSimplification != null)
                DataNodeTreeSimplification(replacedDataNodes);


            foreach (DataNode removedDataNode in replacedDataNodes.Keys)
            {


                removedDataNode.ParentDataNode = null;
            }


        }
        /// <MetaDataID>{e496607f-0484-4853-b4d4-97bf93217d33}</MetaDataID>
        internal static DataNode GetActualDataNode(DataNode dataNode, Dictionary<DataNode, DataNode> replacedDataNodes)
        {
            while (replacedDataNodes.ContainsKey(dataNode))
                dataNode = replacedDataNodes[dataNode];
            return dataNode;
        }

        internal static LambdaExpression GetLambdaExpression(Expression expression)
        {
            if (expression is LambdaExpression)
                return expression as LambdaExpression;
            if (expression is UnaryExpression)
                return GetLambdaExpression( (expression as UnaryExpression).Operand );
            return null;
            
        }
    }

    /// <MetaDataID>{30e78f63-037d-41c3-9cd7-70bf73ec9238}</MetaDataID>
    public class ExpressionException : System.Exception
    {
        public ExpressionException()
        {
        }
        public ExpressionException(string message)
            : base(message)
        {
        }
        public ExpressionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
    /// <MetaDataID>{b3a20871-4101-49b7-ade3-c5482ac94a0c}</MetaDataID>
    public static class LinqRuntimeTypeBuilder
    {
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static AssemblyName assemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
        private static ModuleBuilder moduleBuilder = null;
        private static Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

        static LinqRuntimeTypeBuilder()
        {
            moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
            string key = string.Empty;
            foreach (var field in fields)
                key += field.Key + ";" + field.Value.Name + ";";

            return key;
        }

        public static System.Reflection.MethodInfo GetQuarableMethod(string methodName, ref System.Linq.Expressions.Expression[] expressions)
        {
            var methods = (from method in typeof(Queryable).GetMethods()
                           where method.Name == methodName
                           select method).ToArray();
            foreach (var method in methods)
            {
                Dictionary<Type, Type> typeInstantiation = new Dictionary<Type, Type>();
                if (method.GetParameters().Length == expressions.Length)
                {
                    var findedMethod = method;
                    for (int i = 0; i < expressions.Length; i++)
                    {
                        if (!isTheSameType(method.GetParameters()[i].ParameterType, expressions[i].Type, typeInstantiation))
                        {
                            findedMethod = null;
                            break;
                        }
                    }
                    if (findedMethod != null)
                    {
                        if (findedMethod.IsGenericMethod)
                        {
                            List<Type> argumentTypes = new List<Type>();
                            foreach (Type genArg in findedMethod.GetGenericArguments())
                            {
                                argumentTypes.Add(typeInstantiation[genArg]);
                            }
                            findedMethod = findedMethod.MakeGenericMethod(argumentTypes.ToArray());
                        }

                        int i = 0;
                        foreach (var expression in expressions)
                        {
                            if ((expression is UnaryExpression) && (expression as UnaryExpression).Operand is LambdaExpression)
                            {
                                LambdaExpression lambda = ((expression as UnaryExpression).Operand as LambdaExpression);
                                expressions[i] = Expression.Quote(Expression.Lambda(findedMethod.GetParameters()[i].ParameterType.GetGenericArguments()[0], lambda.Body, lambda.Parameters));

                            }
                            i++;
                        }

                        return findedMethod;
                    }
                }
            }
            return null;

        }

        private static bool isTheSameType(Type type, Type type_2, Dictionary<Type, Type> typeInstantiation)
        {
            if (type_2 == null || type == null)
                return false;
            if (type.Name == type_2.Name && type.IsGenericType == type_2.IsGenericType && type.GetGenericArguments().Length == type_2.GetGenericArguments().Length)
            {
                bool isEqual = true;
                for (int i = 0; i < type.GetGenericArguments().Length; i++)
                {
                    if (!type.GetGenericArguments()[i].IsGenericParameter)
                    {
                        if (!isTheSameType(type.GetGenericArguments()[i], type_2.GetGenericArguments()[i], typeInstantiation))
                            return false;
                    }
                    else
                    {
                        typeInstantiation[type.GetGenericArguments()[i]] = type_2.GetGenericArguments()[i];
                    }
                }
                if (isEqual)
                    return true;


            }
            else
            {
                if (isTheSameType(type, type_2.BaseType, typeInstantiation))
                    return true;
                else
                {
                    foreach (var _interface in type_2.GetInterfaces())
                    {
                        if (isTheSameType(type, _interface, typeInstantiation))
                            return true;
                    }
                }
            }

            return false;
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            if (null == fields)
                throw new ArgumentNullException("fields");
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

            try
            {

                Monitor.Enter(builtTypes);
                string className = GetTypeKey(fields);

                if (builtTypes.ContainsKey(className))
                    return builtTypes[className];

                TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                foreach (var field in fields)
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                builtTypes[className] = typeBuilder.CreateType();

                return builtTypes[className];
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
            finally
            {
                Monitor.Exit(builtTypes);
            }

            return null;
        }

        public static Type GetDynamicType(string className, Dictionary<string, Type> fields)
        {

            Dictionary<string, FieldBuilder> fieldBuilders = new Dictionary<string, FieldBuilder>();
            if (null == fields)
                throw new ArgumentNullException("fields");
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

            try
            {
                Monitor.Enter(builtTypes);

                if (builtTypes.ContainsKey(className))
                    return builtTypes[className];

                TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                //GenericTypeParameterBuilder[] typeParams =typeBuilder.DefineGenericParameters(fields.Keys.ToArray());

                foreach (var field in fields)
                {
                    fieldBuilders[field.Key] = AddProperty(typeBuilder, field.Key, field.Value);


                }
                //typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);
                Type[] constructorArgs = fields.Values.ToArray();
                ConstructorBuilder myConstructorBuilder =
                   typeBuilder.DefineConstructor(MethodAttributes.Public,
                                      CallingConventions.Standard, constructorArgs);

                int i = 0;
                foreach (var field in fields)
                {

                    myConstructorBuilder.DefineParameter(i + 1, ParameterAttributes.HasDefault, field.Key);
                    i++;
                }




                ILGenerator myConstructorIL = myConstructorBuilder.GetILGenerator();
                myConstructorIL.Emit(OpCodes.Ldarg_0);
                myConstructorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

                i = 0;
                foreach (string propertyName in fields.Keys)
                {
                    myConstructorIL.Emit(OpCodes.Ldarg_0);
                    byte i1b = Convert.ToByte(i + 1);
                    myConstructorIL.Emit(OpCodes.Ldarg_S, i1b);
                    myConstructorIL.Emit(OpCodes.Stfld, fieldBuilders[propertyName]);
                    i++;
                }
                myConstructorIL.Emit(OpCodes.Ret);


                builtTypes[className] = typeBuilder.CreateType();

                return builtTypes[className];
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
            finally
            {
                Monitor.Exit(builtTypes);
            }

            return null;
        }


        public static FieldBuilder AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType,
                new[] { propertyType });

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType,
                Type.EmptyTypes);
            ILGenerator getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null,
                new[] { propertyType });
            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
            return field;

        }


        private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
        {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        internal static MethodCallExpression GetSelectManyExprsession(Expression source, LambdaExpression selector, LambdaExpression results)
        {
            Expression[] arguments = new Expression[] { source, Expression.Quote(selector), Expression.Quote(results) };
            System.Reflection.MethodInfo methodInfo = GetQuarableMethod("SelectMany", ref arguments);
            //selector = Expression.Lambda(methodInfo.GetParameters()[1].ParameterType.GetGenericArguments()[0], selector.Body, selector.Parameters);
            //results = Expression.Lambda(methodInfo.GetParameters()[2].ParameterType.GetGenericArguments()[0], results.Body, results.Parameters);
            return System.Linq.Expressions.Expression.Call(methodInfo, arguments);
        }
    }

    /// <MetaDataID>08195A6A-FD9C-490c-B029-782839833FFC</MetaDataID>
    public class ExtendExpressionVisitor
    {
        /// <MetaDataID>{bb264bf9-4fd6-4883-b794-ee3b78fa60fc}</MetaDataID>
        public ExtendExpressionVisitor()
        {
        }

        /// <MetaDataID>{af066b5a-2388-4fe4-9154-7223856bb522}</MetaDataID>
        public virtual Expression Visit(Expression exp)
        {
            if (exp == null)
                return exp;
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                    return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)exp);
                default:
                    return this.VisitUnknown(exp);
            }
        }

        /// <MetaDataID>{28b380ef-6718-4f83-b130-cd54eecb7871}</MetaDataID>
        protected virtual Expression VisitUnknown(Expression expression)
        {
            throw new Exception(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
        }

        /// <MetaDataID>{6793a936-a2d4-42a1-a109-dccaebbed89d}</MetaDataID>
        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        /// <MetaDataID>{95f4acae-8061-465d-9bd6-c29aa878658a}</MetaDataID>
        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }
            return initializer;
        }

        /// <MetaDataID>{e82e899d-dea1-4c47-847c-95ac00c2d5b8}</MetaDataID>
        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            Expression operand = this.Visit(u.Operand);
            return this.UpdateUnary(u, operand, u.Type, u.Method);
        }

        /// <MetaDataID>{a6a7e0bd-5641-4229-a0f3-ef15f8828b81}</MetaDataID>
        protected UnaryExpression UpdateUnary(UnaryExpression u, Expression operand, Type resultType, MethodInfo method)
        {
            if (u.Operand != operand || u.Type != resultType || u.Method != method)
            {
                return Expression.MakeUnary(u.NodeType, operand, resultType, method);
            }
            return u;
        }

        /// <MetaDataID>{cfe709c7-36a2-4921-ba35-3541e1c612db}</MetaDataID>
        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            Expression left = this.Visit(b.Left);
            Expression right = this.Visit(b.Right);
            Expression conversion = this.Visit(b.Conversion);
            return this.UpdateBinary(b, left, right, conversion, b.IsLiftedToNull, b.Method);
        }

        /// <MetaDataID>{c17403fc-06cb-4982-8a06-331b6f6fcb94}</MetaDataID>
        protected BinaryExpression UpdateBinary(BinaryExpression b, Expression left, Expression right, Expression conversion, bool isLiftedToNull, MethodInfo method)
        {
            if (left != b.Left || right != b.Right || conversion != b.Conversion || method != b.Method || isLiftedToNull != b.IsLiftedToNull)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                else
                {
                    return Expression.MakeBinary(b.NodeType, left, right, isLiftedToNull, method);
                }
            }
            return b;
        }

        /// <MetaDataID>{443a0336-b22b-47f3-bf1d-3e0df84a32ac}</MetaDataID>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            Expression expr = this.Visit(b.Expression);
            return this.UpdateTypeIs(b, expr, b.TypeOperand);
        }

        /// <MetaDataID>{4191a9ce-94ea-4d17-8e67-69ead13b27b9}</MetaDataID>
        protected TypeBinaryExpression UpdateTypeIs(TypeBinaryExpression b, Expression expression, Type typeOperand)
        {
            if (expression != b.Expression || typeOperand != b.TypeOperand)
            {
                return Expression.TypeIs(expression, typeOperand);
            }
            return b;
        }

        /// <MetaDataID>{9c39da9f-ed87-4fa2-bdb2-beeff522385d}</MetaDataID>
        protected virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        /// <MetaDataID>{41fe06b5-e3e0-4ac2-8b9e-fbdd2c0f8922}</MetaDataID>
        protected virtual Expression VisitConditional(ConditionalExpression c)
        {
            Expression test = this.Visit(c.Test);
            Expression ifTrue = this.Visit(c.IfTrue);
            Expression ifFalse = this.Visit(c.IfFalse);
            return this.UpdateConditional(c, test, ifTrue, ifFalse);
        }

        /// <MetaDataID>{1888af99-a3fe-49d2-a15e-7f3d4962b037}</MetaDataID>
        protected ConditionalExpression UpdateConditional(ConditionalExpression c, Expression test, Expression ifTrue, Expression ifFalse)
        {
            if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return c;
        }

        /// <MetaDataID>{1524f94f-96fa-4dc2-8313-30d1547adccb}</MetaDataID>
        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        /// <MetaDataID>{ae455ea7-49fa-443a-9a9b-b7c2e3736de1}</MetaDataID>
        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            Expression exp = this.Visit(m.Expression);
            return this.UpdateMemberAccess(m, exp, m.Member);
        }

        /// <MetaDataID>{95dbf694-9313-4206-8168-490ea155d0e7}</MetaDataID>
        protected MemberExpression UpdateMemberAccess(MemberExpression m, Expression expression, MemberInfo member)
        {
            if (expression != m.Expression || member != m.Member)
            {
                return Expression.MakeMemberAccess(expression, member);
            }
            return m;
        }

        /// <MetaDataID>{c648b348-2801-41dc-9d5f-d55a0f35d678}</MetaDataID>
        protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            Expression obj = this.Visit(methodCallExpression.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(methodCallExpression.Arguments);


            if (methodCallExpression.Method.Name == "SelectMany" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
            {

                Expression selector = methodCallExpression.Arguments[1];
                MemberExpression memberExpression = GetDerividedMemberAccess(selector);
                if (memberExpression != null)
                {
                    methodCallExpression = MergeDerivedMembersQueryExpresionForSelectMany(methodCallExpression);
                    args = methodCallExpression.Arguments;
                }
            }
            else if (methodCallExpression.Method.Name == "Select" && (methodCallExpression.Method.DeclaringType == typeof(System.Linq.Queryable) || methodCallExpression.Method.DeclaringType == typeof(System.Linq.Enumerable)))
            {

                Expression selector = methodCallExpression.Arguments[0];
                MemberExpression memberExpression = GetDerividedMemberAccess(selector);
                if (memberExpression != null)
                {
                    methodCallExpression = MergeDerivedMembersQueryExpresionForSelect(methodCallExpression);
                    args = methodCallExpression.Arguments;
                }
            }

            return this.UpdateMethodCall(methodCallExpression, obj, methodCallExpression.Method, args);
        }


        /// <MetaDataID>{f0b04608-e43b-4820-aa3a-706b24a7d0fe}</MetaDataID>
        private MethodCallExpression MergeDerivedMembersQueryExpresionForSelectMany(MethodCallExpression orgSelectManyExpression)
        {


            // SelectMany:
            //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable<T>
            //     that incorporates the index of the source element that produced it. A result
            //     selector function is invoked on each element of each intermediate sequence,
            //     and the resulting values are combined into a single, one-dimensional sequence
            //     and returned.
            //
            // Parameters:
            //   source:
            //     A sequence of values to project.
            //
            //   collectionSelector:
            //     A projection function to apply to each element of the input sequence; the
            //     second parameter of this function represents the index of the source element.
            //
            //   resultSelector:
            //     A projection function to apply to each element of each intermediate sequence.
            //
            //   collectionSelector paramater is a Lambda expression with parameter Type the source sequence element type and body the collection access expression.
            //
            //   resultSelector paramater is a Lambda expression with two parameters, first parameter for source sequence item,second parameter for selector collection item. 
            //   Lambda body expression select data for members of result type






            System.Linq.Expressions.MethodCallExpression derivedMemberQueryExpresion = (GetDerividedMemberAccess(orgSelectManyExpression.Arguments[1]).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression as System.Linq.Expressions.MethodCallExpression;

            if ((derivedMemberQueryExpresion as MethodCallExpression).Method.Name == "SelectMany")
            {
                MethodCallExpression mergedSelectManyExpression = NewMethod(orgSelectManyExpression, derivedMemberQueryExpresion);
                Type mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(mergedSelectManyExpression.Type);
                
                #region Builds select expresion which transform the result type of mergedSelectMany expresion to orgSelectManyExpression result type
                List<string> path = new List<string>();
                Type selectionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);


                Expression[] selectionArguments = new Expression[2];
                selectionArguments[0] = mergedSelectManyExpression;
                if ((((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body is NewExpression))
                {

                    Expression[] selectionResultTypeArguments = (((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body as NewExpression).Arguments.ToArray();

                    ParameterExpression selectionSourceParameter = Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1");

                    for (int i = 0; i != selectionResultTypeArguments.Length; i++)
                    {
                        path.Clear();
                        Expression argExpression = selectionResultTypeArguments[i];

                        while (argExpression is MemberExpression)
                        {
                            path.Insert(0, (argExpression as MemberExpression).Member.Name);
                            argExpression = (argExpression as MemberExpression).Expression;
                        }

                        path.Insert(0, (argExpression as ParameterExpression).Name);
                        argExpression = selectionSourceParameter;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);
                        selectionResultTypeArguments[i] = argExpression;
                    }


                    var selectionResultsBody = Expression.New(selectionResultType.GetConstructors()[0], selectionResultTypeArguments);

                    selectionArguments[1] = Expression.Quote(Expression.Lambda(selectionResultsBody, Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));
                }
                else
                {
                    path.Clear();
                    Expression argExpression = ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body;
                    while (argExpression is MemberExpression)
                    {
                        path.Insert(0, (argExpression as MemberExpression).Member.Name);
                        argExpression = (argExpression as MemberExpression).Expression;
                    }
                    path.Insert(0, (argExpression as ParameterExpression).Name);
                    ParameterExpression selectionSourceParameter = Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1");


                    argExpression = selectionSourceParameter;
                    foreach (string memberName in path)
                        argExpression = Expression.PropertyOrField(argExpression, memberName);
                    if ((argExpression as MemberExpression).Expression is ParameterExpression)
                        selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, (argExpression as MemberExpression).Expression as ParameterExpression));
                    else
                        selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));

                }

                MethodInfo  methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);

                return Expression.Call(methodInfo, selectionArguments);
                #endregion

            }
            else if ((derivedMemberQueryExpresion as MethodCallExpression).Method.Name == "Select")
            {
                Expression orgSource = orgSelectManyExpression.Arguments[0];
                Expression orgCollectionSelector = orgSelectManyExpression.Arguments[1];
                ParameterExpression orgSourceCollectionParam = ((orgCollectionSelector as UnaryExpression).Operand as LambdaExpression).Parameters[0] as ParameterExpression;
                MemberExpression orgDerivedMemberAccessExpression = GetDerividedMemberAccess(orgSelectManyExpression.Arguments[1]);

                
                List<string> path = new List<string>();
                Expression mergedExpressionSelectSource = null;
                Expression[] MergedSelectManyArguments = derivedMemberQueryExpresion.Arguments.ToArray();



                if (derivedMemberQueryExpresion.Arguments[0] is MethodCallExpression)
                    mergedExpressionSelectSource = MergeExpresions(derivedMemberQueryExpresion.Arguments[0] as MethodCallExpression, orgSource, orgDerivedMemberAccessExpression.Expression);
                else
                    mergedExpressionSelectSource = MergeExpresions(derivedMemberQueryExpresion, orgSource, orgDerivedMemberAccessExpression.Expression);
                
                MergedSelectManyArguments[0] = mergedExpressionSelectSource;





                Expression mergedExpressionResultsSelector = null;

                #region Builds mergedExpression result selector
                Expression derivedMemberSelector = ReplaceSourceType(derivedMemberQueryExpresion.Arguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberQueryExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(mergedExpressionSelectSource.Type));

                //// Defines MergedSelectMany collectionSelector
                //var mergedExpressionCollectionSelector = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
                //// Defines source sequence item parameter which use the collection selector to retrivie collection 
                //var mergedExpressionCollectionSelectorParam = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                ////Defines selector collection item parameter used from result selector in conjunction with  mergedExpresionCollectionSelectorParam to produce SelectMany result 
                //var mergedExpressionSelectorParam = ((MergedSelectManyArguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];

                //// MergedSelectMany operation return sequence with element type which contains one property with type the orgExpresion source sequence element type
                //// and another property with type the derived member sequence element Type
                //path.Clear();
                //GetDerivedMemberSourcePath(mergedExpressionCollectionSelectorParam.Type, orgSourceCollectionParam, path);

                Type mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);



                ParameterExpression selectionSourceParameter = Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1");

                // mergedExpressionResultSelector_SourcePropertyInitiator initiate property for orgExpresion source sequence element type 
                Expression mergedExpressionResultSelector_SourcePropertyInitiator = selectionSourceParameter;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourcePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourcePropertyInitiator, memberName);

                // mergedResultSelector_DerivedMemberInitiator initiate property for derived member sequence element type 
                Expression mergedResultSelector_DerivedMemberInitiator = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;

                var mergedExpressionResultTypeMembers = new Dictionary<string, Type>() { { ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name, mergedExpressionResultSelector_SourcePropertyInitiator.Type},
                {((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1].Name,mergedResultSelector_DerivedMemberInitiator.Type}};
                
                mergedExpressionResultType = LinqRuntimeTypeBuilder.GetDynamicType(mergedExpressionResultType.Name + "Source", mergedExpressionResultTypeMembers);
                //mergedExpressionResultSelector_SourcePropertyInitiator
                var mergedExpressionResultsSelectorBody = Expression.New(mergedExpressionResultType.GetConstructors()[0], mergedResultSelector_DerivedMemberInitiator);
                mergedExpressionResultsSelector = Expression.Quote(Expression.Lambda(mergedExpressionResultsSelectorBody, selectionSourceParameter));
                #endregion

                MergedSelectManyArguments[1] = mergedExpressionResultsSelector;
                System.Reflection.MethodInfo methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref MergedSelectManyArguments);
                MethodCallExpression mergedSelectManyExpression = Expression.Call(methodInfo, MergedSelectManyArguments);







                //############################




                var mergedExpressionResultSelectorParam = ((derivedMemberSelector as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                

                path.Clear();
                GetDerivedMemberSourcePath(mergedExpressionResultSelectorParam.Type, orgSourceCollectionParam, path);

                // mergedExpressionResultSelector_SourceTypePropertyInitiator initiate property for orgExpresion source sequence element type 
                Expression mergedExpressionResultSelector_SourceTypePropertyInitiator = mergedExpressionResultSelectorParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourceTypePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourceTypePropertyInitiator, memberName);

                Expression orgExpressionResultSelector_SourceTypeParam = ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                Expression mergedExpressionResultSelector_DerivedMemberInitiator = ((derivedMemberSelector as UnaryExpression).Operand as LambdaExpression).Body;
                Expression orgExpressionResultSelector_DerivedMemberParam = ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];



                mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(mergedExpressionSelectSource.Type);
                ParameterExpression mergedExpressionResultSelector_SourceTypeParam = Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1");

                path.Clear();
                Expression pathExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                while (pathExpression is MemberExpression)
                {
                    path.Insert(0, (pathExpression as MemberExpression).Member.Name);
                    pathExpression = (pathExpression as MemberExpression).Expression;
                }
                mergedExpressionResultSelector_SourceTypePropertyInitiator = mergedExpressionResultSelector_SourceTypeParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourceTypePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourceTypePropertyInitiator, memberName);

                path.Clear();
                pathExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                while (pathExpression is MemberExpression)
                {
                    path.Insert(0, (pathExpression as MemberExpression).Member.Name);
                    pathExpression = (pathExpression as MemberExpression).Expression;
                }
                mergedExpressionResultSelector_DerivedMemberInitiator = mergedExpressionResultSelector_SourceTypeParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_DerivedMemberInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_DerivedMemberInitiator, memberName);




                if (((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body is NewExpression)
                {

                    Expression[] selectionResultTypeArguments = (((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body as NewExpression).Arguments.ToArray();
                    for (int i = 0; i != selectionResultTypeArguments.Length; i++)
                    {
                        path.Clear();
                        Expression argExpression = selectionResultTypeArguments[i];
                        while (argExpression is MemberExpression)
                        {
                            path.Insert(0, (argExpression as MemberExpression).Member.Name);
                            argExpression = (argExpression as MemberExpression).Expression;
                        }

                        if (argExpression == orgExpressionResultSelector_SourceTypeParam)
                        {
                            argExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                            foreach (string memberName in path)
                                argExpression = Expression.PropertyOrField(argExpression, memberName);


                        }
                        else if (argExpression == orgExpressionResultSelector_DerivedMemberParam)
                        {
                            argExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                            foreach (string memberName in path)
                                argExpression = Expression.PropertyOrField(argExpression, memberName);

                        }
                        selectionResultTypeArguments[i] = argExpression;

                    }
                    Type selectionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);
                    var selectionResultsBody = Expression.New(selectionResultType.GetConstructors()[0], selectionResultTypeArguments);
                    Expression[] selectionArguments = new Expression[2];
                    selectionArguments[0] = mergedExpressionSelectSource;
                    selectionArguments[1] = Expression.Quote(Expression.Lambda(selectionResultsBody, Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));


                    methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);

                    return Expression.Call(methodInfo, selectionArguments);

                }
                else
                {
                    Expression[] selectionArguments = new Expression[2];
                    selectionArguments[0] = mergedExpressionSelectSource;


                    path.Clear();
                    Expression argExpression = ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body;
                    while (argExpression is MemberExpression)
                    {
                        path.Insert(0, (argExpression as MemberExpression).Member.Name);
                        argExpression = (argExpression as MemberExpression).Expression;
                    }

                    if (argExpression == orgExpressionResultSelector_SourceTypeParam)
                    {
                        argExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);


                    }
                    else if (argExpression == orgExpressionResultSelector_DerivedMemberParam)
                    {
                        argExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);

                    }

                    selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, Expression.Parameter(mergedExpressionResultType, ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));
                    methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);
                    return Expression.Call(methodInfo, selectionArguments);


                }


                return orgSelectManyExpression;

            }
            else
                return orgSelectManyExpression;










        }

        private MethodCallExpression NewMethod(MethodCallExpression orgSelectManyExpression, System.Linq.Expressions.MethodCallExpression derivedMemberQueryExpresion)
        {
            Expression orgSource = orgSelectManyExpression.Arguments[0];
            Expression orgCollectionSelector = orgSelectManyExpression.Arguments[1];
            ParameterExpression orgSourceCollectionParam = ((orgCollectionSelector as UnaryExpression).Operand as LambdaExpression).Parameters[0] as ParameterExpression;
            MemberExpression orgDerivedMemberAccessExpression = GetDerividedMemberAccess(orgSelectManyExpression.Arguments[1]);

            List<string> path = new List<string>();
            Expression[] MergedSelectManyArguments = derivedMemberQueryExpresion.Arguments.ToArray();
            if (derivedMemberQueryExpresion.Arguments[0] is MethodCallExpression)
            {
                //defines the source collection expression for the new select many expression 
                MergedSelectManyArguments[0] = MergeExpresions(derivedMemberQueryExpresion.Arguments[0] as MethodCallExpression, orgSource, orgDerivedMemberAccessExpression.Expression);
                //builds collection selector expression for the necessary data to build the derivemamber Type
                MergedSelectManyArguments[1] = ReplaceSourceType(MergedSelectManyArguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberQueryExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(MergedSelectManyArguments[0].Type));
            }
            else
            {
                //defines the source collection expression for the new select many expression 
                MergedSelectManyArguments[0] = orgSource;

                #region Builds collection selector expression for the necessary data to build the derivemamber Type
                Expression collectionSelector = orgDerivedMemberAccessExpression.Expression;

                Expression derivedMemberQuerySelector = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
                while (derivedMemberQuerySelector is MemberExpression)
                {
                    path.Insert(0, (derivedMemberQuerySelector as MemberExpression).Member.Name);
                    derivedMemberQuerySelector = (derivedMemberQuerySelector as MemberExpression).Expression;
                }
                foreach (var memberName in path)
                    collectionSelector = Expression.PropertyOrField(collectionSelector, memberName);
                #endregion

                MergedSelectManyArguments[1] = Expression.Quote(Expression.Lambda(collectionSelector, GetParameter(collectionSelector)));
            }
            Expression mergedExpressionResultsSelector = null;

            #region Builds mergedExpression result selector
            // Defines MergedSelectMany collectionSelector
            var mergedExpressionCollectionSelector = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
            // Defines source sequence item parameter which use the collection selector to retrivie collection 
            var mergedExpressionCollectionSelectorParam = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
            //Defines selector collection item parameter used from result selector in conjunction with  mergedExpresionCollectionSelectorParam to produce SelectMany result 
            var mergedExpressionSelectorParam = ((MergedSelectManyArguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];

            // MergedSelectMany operation return sequence with element type which contains one property with type the orgExpresion source sequence element type
            // and another property with type the derived member sequence element Type
            path.Clear();
            GetDerivedMemberSourcePath(mergedExpressionCollectionSelectorParam.Type, orgSourceCollectionParam, path);

            // mergedExpressionResultSelector_SourcePropertyInitiator initiate property for orgExpresion source sequence element type 
            Expression mergedExpressionResultSelector_SourcePropertyInitiator = mergedExpressionCollectionSelectorParam;
            foreach (string memberName in path)
                mergedExpressionResultSelector_SourcePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourcePropertyInitiator, memberName);

            // mergedResultSelector_DerivedMemberInitiator initiate property for derived member sequence element type 
            Expression mergedResultSelector_DerivedMemberInitiator = ((MergedSelectManyArguments[2] as UnaryExpression).Operand as LambdaExpression).Body;

            var mergedExpressionResultTypeMembers = new Dictionary<string, Type>() { { ((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name, mergedExpressionResultSelector_SourcePropertyInitiator.Type},
                                                        {((orgSelectManyExpression.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1].Name,mergedResultSelector_DerivedMemberInitiator.Type}};
            Type mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectManyExpression.Type);
            mergedExpressionResultType = LinqRuntimeTypeBuilder.GetDynamicType(mergedExpressionResultType.Name + "Source", mergedExpressionResultTypeMembers);

            var mergedExpressionResultsSelectorBody = Expression.New(mergedExpressionResultType.GetConstructors()[0], mergedExpressionResultSelector_SourcePropertyInitiator, mergedResultSelector_DerivedMemberInitiator);
            mergedExpressionResultsSelector = Expression.Quote(Expression.Lambda(mergedExpressionResultsSelectorBody, mergedExpressionCollectionSelectorParam, mergedExpressionSelectorParam));
            #endregion

            MergedSelectManyArguments[2] = mergedExpressionResultsSelector;
            System.Reflection.MethodInfo methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("SelectMany", ref MergedSelectManyArguments);
            MethodCallExpression mergedSelectManyExpression = Expression.Call(methodInfo, MergedSelectManyArguments);
            return mergedSelectManyExpression;
        }

        /// <MetaDataID>{3ac84683-a1e4-48cd-a0b6-0f2c269648ea}</MetaDataID>
        private MethodCallExpression MergeDerivedMembersQueryExpresionForSelect(MethodCallExpression orgSelectExpression)
        {


            //
            // Select:
            //     Projects each element of a sequence into a new form by incorporating the
            //     element's index.
            //
            // Parameters:
            //   source:
            //     A sequence of values to invoke a transform function on.
            //
            //   selector:
            //     A transform function to apply to each source element; the second parameter
            //     of the function represents the index of the source element.


            Expression orgSource = (orgSelectExpression.Arguments[0] as MemberExpression).Expression;
            ParameterExpression orgSourceCollectionParam = (orgSelectExpression.Arguments[0] as MemberExpression).Expression as ParameterExpression;// ((orgSource as UnaryExpression).Operand as LambdaExpression).Parameters[0] as ParameterExpression;
            MemberExpression orgDerivedMemberAccessExpression = orgSelectExpression.Arguments[0] as MemberExpression;
            System.Linq.Expressions.MethodCallExpression derivedMemberQueryExpresion = ((orgDerivedMemberAccessExpression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression as System.Linq.Expressions.MethodCallExpression;

            if ((derivedMemberQueryExpresion as MethodCallExpression).Method.Name == "SelectMany")
            {
                List<string> path = new List<string>();
                Expression[] MergedSelectManyArguments = derivedMemberQueryExpresion.Arguments.ToArray();
                if (derivedMemberQueryExpresion.Arguments[0] is MethodCallExpression)
                {
                    MergedSelectManyArguments[0] = MergeExpresions(derivedMemberQueryExpresion.Arguments[0] as MethodCallExpression, orgSource, orgDerivedMemberAccessExpression.Expression);
                    MergedSelectManyArguments[1] = ReplaceSourceType(MergedSelectManyArguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberQueryExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(MergedSelectManyArguments[0].Type));
                }
                else
                {
                    MergedSelectManyArguments[0] = orgSource;
                    Expression collectionSelector = orgDerivedMemberAccessExpression.Expression;

                    Expression derivedMemberQuerySelector = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
                    while (derivedMemberQuerySelector is MemberExpression)
                    {
                        path.Insert(0, (derivedMemberQuerySelector as MemberExpression).Member.Name);
                        derivedMemberQuerySelector = (derivedMemberQuerySelector as MemberExpression).Expression;
                    }
                    foreach (var memberName in path)
                        collectionSelector = Expression.PropertyOrField(collectionSelector, memberName);
                    MergedSelectManyArguments[1] = Expression.Quote(Expression.Lambda(collectionSelector, GetParameter(collectionSelector)));
                }
                Expression mergedExpressionResultsSelector = null;

                #region Builds mergedExpression result selector
                // Defines MergedSelectMany collectionSelector
                var mergedExpressionCollectionSelector = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
                // Defines source sequence item parameter which use the collection selector to retrivie collection 
                var mergedExpressionCollectionSelectorParam = ((MergedSelectManyArguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                //Defines selector collection item parameter used from result selector in conjunction with  mergedExpresionCollectionSelectorParam to produce SelectMany result 
                var mergedExpressionSelectorParam = ((MergedSelectManyArguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];


                //resultType
                //{
                //   PropertyType: orgExpresion source sequence element type
                //   PropertyType: derived member sequence element Type
                //}

                // MergedSelectMany operation return sequence with element type which contains one property with type the orgExpresion source sequence element type
                // and another property with type the derived member sequence element Type
                path.Clear();
                GetDerivedMemberSourcePath(mergedExpressionCollectionSelectorParam.Type, orgSourceCollectionParam, path);

                // mergedExpressionResultSelector_SourcePropertyInitiator initiate property for orgExpresion source sequence element type 
                Expression mergedExpressionResultSelector_SourcePropertyInitiator = mergedExpressionCollectionSelectorParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourcePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourcePropertyInitiator, memberName);

                // mergedResultSelector_DerivedMemberInitiator initiate property for derived member sequence element type 
                Expression mergedResultSelector_DerivedMemberInitiator = ((MergedSelectManyArguments[2] as UnaryExpression).Operand as LambdaExpression).Body;

                var mergedExpressionResultTypeMembers = new Dictionary<string, Type>() { { orgSourceCollectionParam.Name, mergedExpressionResultSelector_SourcePropertyInitiator.Type },
                                                                                        {orgDerivedMemberAccessExpression.Member.Name,mergedResultSelector_DerivedMemberInitiator.Type}};
                Type mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectExpression.Type);
                mergedExpressionResultType = LinqRuntimeTypeBuilder.GetDynamicType(mergedExpressionResultType.Name + "Source", mergedExpressionResultTypeMembers);

                var mergedExpressionResultsSelectorBody = Expression.New(mergedExpressionResultType.GetConstructors()[0], mergedExpressionResultSelector_SourcePropertyInitiator, mergedResultSelector_DerivedMemberInitiator);
                mergedExpressionResultsSelector = Expression.Quote(Expression.Lambda(mergedExpressionResultsSelectorBody, mergedExpressionCollectionSelectorParam, mergedExpressionSelectorParam));
                #endregion

                MergedSelectManyArguments[2] = mergedExpressionResultsSelector;
                System.Reflection.MethodInfo methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("SelectMany", ref MergedSelectManyArguments);
                MethodCallExpression mergedSelectManyExpression = null;// Expression.Call(methodInfo, MergedSelectManyArguments);


                #region Builds select expresion which transform the result type of mergedSelectMany expresion to orgSelectManyExpression result type

                Type selectionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectExpression.Type);


                Expression[] selectionArguments = new Expression[2];
                selectionArguments[0] = mergedSelectManyExpression;
                if (((orgSelectExpression.Arguments[1] as LambdaExpression).Body is NewExpression))
                {

                    Expression[] selectionResultTypeArguments = ((orgSelectExpression.Arguments[1] as LambdaExpression).Body as NewExpression).Arguments.ToArray();

                    ParameterExpression selectionSourceParameter = Expression.Parameter(mergedExpressionResultType, (orgSelectExpression.Arguments[1] as LambdaExpression).Parameters[0].Name + "_1");

                    for (int i = 0; i != selectionResultTypeArguments.Length; i++)
                    {
                        path.Clear();
                        Expression argExpression = selectionResultTypeArguments[i];

                        while (argExpression is MemberExpression)
                        {
                            path.Insert(0, (argExpression as MemberExpression).Member.Name);
                            argExpression = (argExpression as MemberExpression).Expression;
                        }

                        path.Insert(0, (argExpression as ParameterExpression).Name);
                        argExpression = selectionSourceParameter;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);
                        selectionResultTypeArguments[i] = argExpression;
                    }


                    var selectionResultsBody = Expression.New(selectionResultType.GetConstructors()[0], selectionResultTypeArguments);

                    selectionArguments[1] = Expression.Quote(Expression.Lambda(selectionResultsBody, Expression.Parameter(mergedExpressionResultType, (orgSelectExpression.Arguments[1] as LambdaExpression).Parameters[0].Name + "_1")));
                }
                else
                {
                    path.Clear();
                    Expression argExpression = (orgSelectExpression.Arguments[1] as LambdaExpression).Body;
                    while (argExpression is MemberExpression)
                    {
                        path.Insert(0, (argExpression as MemberExpression).Member.Name);
                        argExpression = (argExpression as MemberExpression).Expression;
                    }
                    path.Insert(0, (argExpression as ParameterExpression).Name);
                    ParameterExpression selectionSourceParameter = Expression.Parameter(mergedExpressionResultType, (orgSelectExpression.Arguments[1] as LambdaExpression).Parameters[0].Name + "_1");


                    argExpression = selectionSourceParameter;
                    foreach (string memberName in path)
                        argExpression = Expression.PropertyOrField(argExpression, memberName);
                    if ((argExpression as MemberExpression).Expression is ParameterExpression)
                        selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, (argExpression as MemberExpression).Expression as ParameterExpression));
                    else
                        selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, Expression.Parameter(mergedExpressionResultType, (orgSelectExpression.Arguments[1] as LambdaExpression).Parameters[0].Name + "_1")));

                }

                methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);

                return Expression.Call(methodInfo, selectionArguments);
                #endregion

            }
            else if ((derivedMemberQueryExpresion as MethodCallExpression).Method.Name == "Select")
            {

                Expression mergedExpressionSelectSource = null;
                if (derivedMemberQueryExpresion.Arguments[0] is MethodCallExpression)
                    mergedExpressionSelectSource = MergeExpresions(derivedMemberQueryExpresion.Arguments[0] as MethodCallExpression, orgSource, orgDerivedMemberAccessExpression.Expression);
                else
                    mergedExpressionSelectSource = MergeExpresions(derivedMemberQueryExpresion, orgSource, orgDerivedMemberAccessExpression.Expression);


                Expression mergedExpressionResultSelector = ReplaceSourceType(derivedMemberQueryExpresion.Arguments[1], OOAdvantech.TypeHelper.GetElementType(derivedMemberQueryExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(mergedExpressionSelectSource.Type));


                var mergedExpressionResultSelectorParam = ((mergedExpressionResultSelector as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                List<string> path = new List<string>();
                GetDerivedMemberSourcePath(mergedExpressionResultSelectorParam.Type, orgSourceCollectionParam, path);

                // mergedExpressionResultSelector_SourceTypePropertyInitiator initiate property for orgExpresion source sequence element type 
                Expression mergedExpressionResultSelector_SourceTypePropertyInitiator = mergedExpressionResultSelectorParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourceTypePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourceTypePropertyInitiator, memberName);

                Expression orgExpressionResultSelector_SourceTypeParam = ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0];
                Expression mergedExpressionResultSelector_DerivedMemberInitiator = ((mergedExpressionResultSelector as UnaryExpression).Operand as LambdaExpression).Body;
                Expression orgExpressionResultSelector_DerivedMemberParam = ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[1];



                Type mergedExpressionResultType = OOAdvantech.TypeHelper.GetElementType(mergedExpressionSelectSource.Type);
                ParameterExpression mergedExpressionResultSelector_SourceTypeParam = Expression.Parameter(mergedExpressionResultType, ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1");

                path.Clear();
                Expression pathExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                while (pathExpression is MemberExpression)
                {
                    path.Insert(0, (pathExpression as MemberExpression).Member.Name);
                    pathExpression = (pathExpression as MemberExpression).Expression;
                }
                mergedExpressionResultSelector_SourceTypePropertyInitiator = mergedExpressionResultSelector_SourceTypeParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_SourceTypePropertyInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_SourceTypePropertyInitiator, memberName);

                path.Clear();
                pathExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                while (pathExpression is MemberExpression)
                {
                    path.Insert(0, (pathExpression as MemberExpression).Member.Name);
                    pathExpression = (pathExpression as MemberExpression).Expression;
                }
                mergedExpressionResultSelector_DerivedMemberInitiator = mergedExpressionResultSelector_SourceTypeParam;
                foreach (string memberName in path)
                    mergedExpressionResultSelector_DerivedMemberInitiator = Expression.PropertyOrField(mergedExpressionResultSelector_DerivedMemberInitiator, memberName);


                if (((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Body is NewExpression)
                {

                    Expression[] selectionResultTypeArguments = (((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Body as NewExpression).Arguments.ToArray();
                    for (int i = 0; i != selectionResultTypeArguments.Length; i++)
                    {
                        path.Clear();
                        Expression argExpression = selectionResultTypeArguments[i];
                        while (argExpression is MemberExpression)
                        {
                            path.Insert(0, (argExpression as MemberExpression).Member.Name);
                            argExpression = (argExpression as MemberExpression).Expression;
                        }

                        if (argExpression == orgExpressionResultSelector_SourceTypeParam)
                        {
                            argExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                            foreach (string memberName in path)
                                argExpression = Expression.PropertyOrField(argExpression, memberName);


                        }
                        else if (argExpression == orgExpressionResultSelector_DerivedMemberParam)
                        {
                            argExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                            foreach (string memberName in path)
                                argExpression = Expression.PropertyOrField(argExpression, memberName);

                        }
                        selectionResultTypeArguments[i] = argExpression;

                    }
                    Type selectionResultType = OOAdvantech.TypeHelper.GetElementType(orgSelectExpression.Type);
                    var selectionResultsBody = Expression.New(selectionResultType.GetConstructors()[0], selectionResultTypeArguments);
                    Expression[] selectionArguments = new Expression[2];
                    selectionArguments[0] = mergedExpressionSelectSource;
                    selectionArguments[1] = Expression.Quote(Expression.Lambda(selectionResultsBody, Expression.Parameter(mergedExpressionResultType, ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));


                    System.Reflection.MethodInfo methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);

                    return Expression.Call(methodInfo, selectionArguments);

                }
                else
                {
                    Expression[] selectionArguments = new Expression[2];
                    selectionArguments[0] = mergedExpressionSelectSource;


                    path.Clear();
                    Expression argExpression = ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Body;
                    while (argExpression is MemberExpression)
                    {
                        path.Insert(0, (argExpression as MemberExpression).Member.Name);
                        argExpression = (argExpression as MemberExpression).Expression;
                    }

                    if (argExpression == orgExpressionResultSelector_SourceTypeParam)
                    {
                        argExpression = mergedExpressionResultSelector_SourceTypePropertyInitiator;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);


                    }
                    else if (argExpression == orgExpressionResultSelector_DerivedMemberParam)
                    {
                        argExpression = mergedExpressionResultSelector_DerivedMemberInitiator;
                        foreach (string memberName in path)
                            argExpression = Expression.PropertyOrField(argExpression, memberName);

                    }

                    selectionArguments[1] = Expression.Quote(Expression.Lambda(argExpression, Expression.Parameter(mergedExpressionResultType, ((orgSelectExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression).Parameters[0].Name + "_1")));
                    System.Reflection.MethodInfo methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod("Select", ref selectionArguments);
                    return Expression.Call(methodInfo, selectionArguments);


                }


                return orgSelectExpression;

            }
            else
                return orgSelectExpression;



        }


        /// <MetaDataID>{e55f6848-f434-4b58-bb91-c4a32fa680d6}</MetaDataID>
        private void GetDerivedMemberSourcePath(Type type, Expression derivedMemberSelectorSource, List<string> path)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.Name.IndexOf("<>h__TransparentIdentifier") == 0 && property.PropertyType.Name.IndexOf("DeriveMember") != property.PropertyType.Name.Length - "DeriveMember".Length - 1)
                {
                    path.Add(property.Name);
                    GetDerivedMemberSourcePath(property.PropertyType, derivedMemberSelectorSource, path);
                    return;
                }
            }
            string propertyName = null;
            Type propertyType = null;
            if (derivedMemberSelectorSource is MemberExpression)
            {
                propertyName = (derivedMemberSelectorSource as MemberExpression).Member.Name;
                propertyType = (derivedMemberSelectorSource as MemberExpression).Type;
            }
            if (derivedMemberSelectorSource is ParameterExpression)
            {
                propertyName = (derivedMemberSelectorSource as ParameterExpression).Name;
                propertyType = (derivedMemberSelectorSource as ParameterExpression).Type;
            }
            foreach (var property in type.GetProperties())
            {
                if (propertyType == property.PropertyType && propertyName == property.Name)
                {
                    path.Add(property.Name);
                    break;
                }
            }

        }




        /// <MetaDataID>{7aad3579-f89e-4cb2-bb28-ad70e2b31f0e}</MetaDataID>
        private System.Linq.Expressions.MemberExpression GetDerividedMemberAccess(Expression selector)
        {
            if (selector is UnaryExpression)
                selector = (selector as UnaryExpression).Operand;

            if (selector is LambdaExpression)
                selector = (selector as LambdaExpression).Body;
            while ((selector is System.Linq.Expressions.MemberExpression) && !((selector as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0))
                selector = (selector as System.Linq.Expressions.MemberExpression).Expression;

            if (selector is System.Linq.Expressions.MemberExpression && ((selector as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0))
                return selector as System.Linq.Expressions.MemberExpression;

            return null;

        }

        /// <summary></summary>
        /// <param name="derivedMemberExpresion"></param>
        /// <param name="source">
        /// Derived member SelectMany source
        /// </param>
        /// <param name="selectorSourceParm">
        /// Derived member selector source parameter
        /// </param>
        /// <returns></returns>
        /// <MetaDataID>{839c8f8b-9711-4d53-b05a-35a2c9f9c928}</MetaDataID>
        private Expression MergeExpresions(MethodCallExpression derivedMemberExpresion, Expression source, Expression selectorSourceParm)
        {

            if ((derivedMemberExpresion as MethodCallExpression).Arguments[0] is MethodCallExpression)
            {
                System.Linq.Expressions.Expression extendedExpresionSource = MergeExpresions(derivedMemberExpresion.Arguments[0] as MethodCallExpression, source, selectorSourceParm);

                System.Diagnostics.Debug.WriteLine("Expression replace");
                System.Diagnostics.Debug.WriteLine(derivedMemberExpresion.ToString());
                System.Linq.Expressions.Expression extendedExpresion = CopyMethodCallExpression((derivedMemberExpresion as MethodCallExpression), extendedExpresionSource);

                //System.Linq.Expressions.Expression extendedExpresion= System.Linq.Expressions.Expression.Call((derivedMemberExpresion as MethodCallExpression).Method, arguments);
                System.Diagnostics.Debug.WriteLine(extendedExpresion.ToString());
                return extendedExpresion;
            }
            else
            {
                Expression derivedExpressionSelector = derivedMemberExpresion.Arguments[1];
                if (derivedExpressionSelector is UnaryExpression)
                    derivedExpressionSelector = (derivedExpressionSelector as UnaryExpression).Operand;

                if (derivedExpressionSelector is LambdaExpression)
                    derivedExpressionSelector = (derivedExpressionSelector as LambdaExpression).Body;
                List<MemberExpression> path = new List<MemberExpression>();
                while (derivedExpressionSelector is MemberExpression)
                {
                    path.Insert(0, derivedExpressionSelector as MemberExpression);
                    derivedExpressionSelector = (derivedExpressionSelector as MemberExpression).Expression;
                }
                bool hasTheSameSourceParameterName = GetParameter(selectorSourceParm).Name == GetParameter(derivedExpressionSelector).Name;

                var derivedMemberSourceParam = selectorSourceParm;
                var mergeSelectorBody = selectorSourceParm;
                foreach (var memberExpresion in path)
                    mergeSelectorBody = Expression.PropertyOrField(mergeSelectorBody, memberExpresion.Member.Name);

                Type resultType = null;
                Dictionary<string, Type> dynamicTypeParameters = new Dictionary<string, Type>();
                if (!hasTheSameSourceParameterName)
                    dynamicTypeParameters.Add(GetParameter(selectorSourceParm).Name, GetParameter(selectorSourceParm).Type);
                foreach (var parameter in ((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters)
                    dynamicTypeParameters.Add(parameter.Name, parameter.Type);
                var mergeSourceParam = Expression.Parameter(GetParameter(selectorSourceParm).Type, GetParameter(selectorSourceParm).Name);
                var mergeSelectorParam = ((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Parameters[1];
                resultType = LinqRuntimeTypeBuilder.GetDynamicType(((derivedMemberExpresion.Arguments[2] as UnaryExpression).Operand as LambdaExpression).Body.Type.Name + "DeriveMember", dynamicTypeParameters);
                MethodCallExpression extendedExpresion = null;
                if (!hasTheSameSourceParameterName)
                {
                    Expression mergeResultsBody = Expression.New(resultType.GetConstructors()[0], mergeSourceParam, derivedMemberSourceParam, mergeSelectorParam);
                    var mergeResults = Expression.Lambda(mergeResultsBody, mergeSourceParam, mergeSelectorParam);
                    extendedExpresion = LinqRuntimeTypeBuilder.GetSelectManyExprsession(source, Expression.Lambda(mergeSelectorBody, mergeSourceParam), mergeResults);
                }
                else
                {
                    Expression mergeResultsBody = Expression.New(resultType.GetConstructors()[0], mergeSourceParam, mergeSelectorParam);
                    var mergeResults = Expression.Lambda(mergeResultsBody, mergeSourceParam, mergeSelectorParam);
                    extendedExpresion = LinqRuntimeTypeBuilder.GetSelectManyExprsession(source, Expression.Lambda(mergeSelectorBody, mergeSourceParam), mergeResults);
                }

                return extendedExpresion;
            }
        }


        /// <MetaDataID>{158d02d1-7425-4c4d-b84a-722e3d1d04ef}</MetaDataID>
        Expression ReplaceSourceType(Expression expression, Type oldSourceType, Type newSourceType)
        {
            if (expression is MethodCallExpression)
            {
                var arguments = (expression as MethodCallExpression).Arguments.ToArray();
                int i = 0;
                foreach (var argument in arguments)
                    arguments[i++] = ReplaceSourceType(argument, oldSourceType, newSourceType);
                return Expression.Call((expression as MethodCallExpression).Method, arguments);
            }
            if (expression is UnaryExpression)
            {
                return Expression.Quote(ReplaceSourceType((expression as UnaryExpression).Operand, oldSourceType, newSourceType));
            }
            if (expression is LambdaExpression)
            {


                Expression body = ReplaceSourceType((expression as LambdaExpression).Body, oldSourceType, newSourceType);
                var parameters = (expression as LambdaExpression).Parameters.ToArray();
                int i = 0;
                foreach (var parameter in parameters)
                    parameters[i++] = ReplaceSourceType(parameter, oldSourceType, newSourceType) as ParameterExpression;

                Expression copyExpression = Expression.Lambda(body, parameters);
                return copyExpression;


            }
            if (expression is BinaryExpression)
            {
                Expression left = ReplaceSourceType((expression as BinaryExpression).Left, oldSourceType, newSourceType);
                Expression Right = ReplaceSourceType((expression as BinaryExpression).Right, oldSourceType, newSourceType);
                BinaryExpression binaryExpression = Expression.MakeBinary(expression.NodeType, left, Right);
                return binaryExpression;


            }
            if (expression is MemberExpression)
            {
                MemberExpression orgMemberExpression = expression as MemberExpression;
                List<MemberExpression> path = new List<MemberExpression>();
                while (!(expression is ParameterExpression))
                {
                    path.Insert(0, expression as MemberExpression);
                    expression = (expression as MemberExpression).Expression;
                }
                ParameterExpression parameterExpression = expression as ParameterExpression;
                if (parameterExpression.Type == oldSourceType)
                {
                    Expression expressionCopy = Expression.Parameter(newSourceType, parameterExpression.Name);

                    foreach (var memberExpresion in path)
                        expressionCopy = Expression.PropertyOrField(expressionCopy, memberExpresion.Member.Name);
                    return expressionCopy;
                }
                else
                    return orgMemberExpression;


            }
            if (expression is ParameterExpression)
            {
                ParameterExpression parameterExpression = expression as ParameterExpression;
                if (parameterExpression.Type == oldSourceType)
                {
                    Expression expressionCopy = Expression.Parameter(newSourceType, parameterExpression.Name);
                    return expressionCopy;
                }
                else
                    return parameterExpression;
            }
            if (expression is ConstantExpression)
                return expression;

            if (expression is MemberInitExpression)
            {
                var memberBindigs = (expression as MemberInitExpression).Bindings.ToArray();
                int i = 0;
                foreach (MemberAssignment memberBindig in memberBindigs)
                    memberBindigs[i++] = Expression.Bind(memberBindig.Member, ReplaceSourceType(memberBindig.Expression, oldSourceType, newSourceType));
                Expression expressionCopy = Expression.MemberInit((expression as MemberInitExpression).NewExpression, memberBindigs);
                return expressionCopy;
            }
            if (expression is NewExpression)
            {

                Dictionary<string, Type> dynamicTypeParameters = new Dictionary<string, Type>();

                var arguments = (expression as NewExpression).Arguments.ToArray();
                int i = 0;
                foreach (var argument in arguments)
                {
                    arguments[i] = ReplaceSourceType(argument, oldSourceType, newSourceType);
                    dynamicTypeParameters.Add((expression as NewExpression).Constructor.GetParameters()[i].Name, GetParameter(arguments[i]).Type);
                    i++;
                }

                Type resultType = LinqRuntimeTypeBuilder.GetDynamicType((expression as NewExpression).Type.Name + "DeriveMember", dynamicTypeParameters);
                Expression expressionCopy = Expression.New(resultType.GetConstructors()[0], arguments);
                return expressionCopy;



            }


            //switch (expression.NodeType)
            //{

            //    case ExpressionType.Add:
            //    case ExpressionType.AddChecked:
            //    case ExpressionType.Multiply:
            //    case ExpressionType.MultiplyChecked:
            //    case ExpressionType.Subtract:
            //    case ExpressionType.SubtractChecked:
            //    case ExpressionType.Power:
            //    case ExpressionType.Divide:
            //    case ExpressionType.And:
            //    case ExpressionType.AndAlso:
            //    case ExpressionType.ArrayIndex:
            //    case ExpressionType.Coalesce:
            //    case ExpressionType.Equal:
            //    case ExpressionType.ExclusiveOr:
            //    case ExpressionType.GreaterThan:
            //    case ExpressionType.GreaterThanOrEqual:
            //    case ExpressionType.LeftShift:
            //    case ExpressionType.LessThan:
            //    case ExpressionType.LessThanOrEqual:
            //    case ExpressionType.Modulo:
            //    case ExpressionType.NotEqual:
            //    case ExpressionType.Or:
            //    case ExpressionType.OrElse:
            //    case ExpressionType.RightShift:
            //        return CopyBinary((BinaryExpression)expression, sourceType);
            //    case ExpressionType.ArrayLength:
            //    case ExpressionType.Convert:
            //    case ExpressionType.ConvertChecked:
            //    case ExpressionType.Negate:
            //    case ExpressionType.NegateChecked:
            //    case ExpressionType.Not:
            //    case ExpressionType.Quote:
            //    case ExpressionType.TypeAs:
            //        return VisitUnary((UnaryExpression)exp, ref parent);
            //    case ExpressionType.Call:
            //        return VisitMethodCall((MethodCallExpression)exp, ref parent);

            //    case ExpressionType.Conditional:
            //        return VisitConditional((ConditionalExpression)exp, ref parent);

            //    case ExpressionType.Constant:
            //        return VisitConstant((ConstantExpression)exp, ref parent); ;

            //    case ExpressionType.Invoke:
            //        return VisitInvocation((InvocationExpression)exp, ref parent);

            //    case ExpressionType.Lambda:
            //        return VisitLambda((LambdaExpression)exp, ref parent);

            //    case ExpressionType.ListInit:
            //        return VisitListInit((ListInitExpression)exp, ref parent);

            //    case ExpressionType.MemberAccess:
            //        return VisitMemberAccess((MemberExpression)exp, ref parent);

            //    case ExpressionType.MemberInit:
            //        return VisitMemberInit((MemberInitExpression)exp, ref parent);

            //    case ExpressionType.New:
            //        return VisitNew((NewExpression)exp, ref parent);

            //    case ExpressionType.NewArrayInit:
            //    case ExpressionType.NewArrayBounds:
            //        return VisitNewArray((NewArrayExpression)exp, ref parent);

            //    case ExpressionType.Parameter:
            //        return VisitParameter((ParameterExpression)exp, ref parent);


            //    case ExpressionType.TypeIs:
            //        return VisitTypeIs((TypeBinaryExpression)exp, ref expresionTreeNode);
            //    default:
            //        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            //}

            return null;
        }
        /// <MetaDataID>{d5ff5100-4d3b-424c-99fb-d111bb22c70c}</MetaDataID>
        private Expression CopyMethodCallExpression(MethodCallExpression derivedMemberExpresion, Expression extendedExpresionSource)
        {

            Expression[] arguments = derivedMemberExpresion.Arguments.ToArray();
            arguments[0] = extendedExpresionSource;
            for (int i = 1; i < arguments.Length; i++)
                arguments[i] = ReplaceSourceType(arguments[i], OOAdvantech.TypeHelper.GetElementType(derivedMemberExpresion.Arguments[0].Type), OOAdvantech.TypeHelper.GetElementType(extendedExpresionSource.Type));


            System.Reflection.MethodInfo methodInfo = null;
            if (derivedMemberExpresion.Method.DeclaringType == typeof(Queryable))
                methodInfo = LinqRuntimeTypeBuilder.GetQuarableMethod(derivedMemberExpresion.Method.Name, ref arguments);
            else
                methodInfo = derivedMemberExpresion.Method;

            return Expression.Call(methodInfo, arguments);

        }


        /// <MetaDataID>{5177a015-69ed-4318-95ba-451b6f663c8f}</MetaDataID>
        private ParameterExpression GetParameter(Expression expression)
        {
            while (expression is MemberExpression)
                expression = (expression as MemberExpression).Expression;
            return (expression as ParameterExpression);
        }


        /// <MetaDataID>{f02abcf0-b117-4bdf-95de-e286fed78dcf}</MetaDataID>
        protected MethodCallExpression UpdateMethodCall(MethodCallExpression m, Expression obj, MethodInfo method, IEnumerable<Expression> args)
        {
            if (obj != m.Object || method != m.Method || args != m.Arguments)
            {
                return Expression.Call(obj, method, args);
            }
            return m;
        }

        /// <MetaDataID>{6b78f99d-2011-4412-91fd-cd8411b47f33}</MetaDataID>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            if (original != null)
            {
                List<Expression> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    Expression p = this.Visit(original[i]);
                    if (list != null)
                    {
                        list.Add(p);
                    }
                    else if (p != original[i])
                    {
                        list = new List<Expression>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(p);
                    }
                }
                if (list != null)
                {
                    return list.AsReadOnly();
                }
            }
            return original;
        }

        /// <MetaDataID>{66fe01c0-5a19-481d-9965-f2018b9ac892}</MetaDataID>
        protected virtual ReadOnlyCollection<Expression> VisitMemberAndExpressionList(ReadOnlyCollection<MemberInfo> members, ReadOnlyCollection<Expression> original)
        {
            if (original != null)
            {
                List<Expression> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    Expression p = this.VisitMemberAndExpression(members != null ? members[i] : null, original[i]);
                    if (list != null)
                    {
                        list.Add(p);
                    }
                    else if (p != original[i])
                    {
                        list = new List<Expression>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(p);
                    }
                }
                if (list != null)
                {
                    return list.AsReadOnly();
                }
            }
            return original;
        }

        /// <MetaDataID>{0ec6c548-e8a3-4be5-9703-e3d4c792d644}</MetaDataID>
        protected virtual Expression VisitMemberAndExpression(MemberInfo member, Expression expression)
        {
            return this.Visit(expression);
        }

        /// <MetaDataID>{fefb55d4-6371-46dd-a20e-dbfa4a3a5bfe}</MetaDataID>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression e = this.Visit(assignment.Expression);
            return this.UpdateMemberAssignment(assignment, assignment.Member, e);
        }

        /// <MetaDataID>{30c9bbdc-dc1a-4431-8ac0-1ef4f60be84b}</MetaDataID>
        protected MemberAssignment UpdateMemberAssignment(MemberAssignment assignment, MemberInfo member, Expression expression)
        {
            if (expression != assignment.Expression || member != assignment.Member)
            {
                return Expression.Bind(member, expression);
            }
            return assignment;
        }

        /// <MetaDataID>{04496dc0-6c0f-445d-8682-04f5bbd770fa}</MetaDataID>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
            return this.UpdateMemberMemberBinding(binding, binding.Member, bindings);
        }

        /// <MetaDataID>{94105dd0-2201-476b-8b45-3809b3c085ac}</MetaDataID>
        protected MemberMemberBinding UpdateMemberMemberBinding(MemberMemberBinding binding, MemberInfo member, IEnumerable<MemberBinding> bindings)
        {
            if (bindings != binding.Bindings || member != binding.Member)
            {
                return Expression.MemberBind(member, bindings);
            }
            return binding;
        }

        /// <MetaDataID>{d993ed12-6f18-49b7-8d0b-bac32c93c74b}</MetaDataID>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
            return this.UpdateMemberListBinding(binding, binding.Member, initializers);
        }

        /// <MetaDataID>{21715d5e-10d5-49a6-ad14-406acfd0dc26}</MetaDataID>
        protected MemberListBinding UpdateMemberListBinding(MemberListBinding binding, MemberInfo member, IEnumerable<ElementInit> initializers)
        {
            if (initializers != binding.Initializers || member != binding.Member)
            {
                return Expression.ListBind(member, initializers);
            }
            return binding;
        }

        /// <MetaDataID>{4eb0e8a3-619e-49b5-a958-1618c86befbb}</MetaDataID>
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                MemberBinding b = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(b);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        /// <MetaDataID>{fb615df7-8a11-4c39-ad42-2a6639fc2f7a}</MetaDataID>
        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                ElementInit init = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(init);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        /// <MetaDataID>{1532ab0b-3106-4b3b-9419-1d3f25f2c92c}</MetaDataID>
        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression body = this.Visit(lambda.Body);
            return this.UpdateLambda(lambda, lambda.Type, body, lambda.Parameters);
        }

        /// <MetaDataID>{af11eede-89a0-4da8-9c63-bb2a3428c5a2}</MetaDataID>
        protected LambdaExpression UpdateLambda(LambdaExpression lambda, Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters)
        {
            if (body != lambda.Body || parameters != lambda.Parameters || delegateType != lambda.Type)
            {
                return Expression.Lambda(delegateType, body, parameters);
            }
            return lambda;
        }

        /// <MetaDataID>{5332ff97-68bc-4846-9f67-83ac1cc166f5}</MetaDataID>
        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> args = this.VisitMemberAndExpressionList(nex.Members, nex.Arguments);
            return this.UpdateNew(nex, nex.Constructor, args, nex.Members);
        }

        /// <MetaDataID>{10759b20-4aa2-4462-837b-7191cad880a4}</MetaDataID>
        protected NewExpression UpdateNew(NewExpression nex, ConstructorInfo constructor, IEnumerable<Expression> args, IEnumerable<MemberInfo> members)
        {
            if (args != nex.Arguments || constructor != nex.Constructor || members != nex.Members)
            {
                if (nex.Members != null)
                {
                    return Expression.New(constructor, args, members);
                }
                else
                {
                    return Expression.New(constructor, args);
                }
            }
            return nex;
        }

        /// <MetaDataID>{559dfe19-4e4c-4ce8-a72f-baaa92562ce5}</MetaDataID>
        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
            return this.UpdateMemberInit(init, n, bindings);
        }

        /// <MetaDataID>{d42c5c95-ea23-4b2f-9f03-6116ebb0a334}</MetaDataID>
        protected MemberInitExpression UpdateMemberInit(MemberInitExpression init, NewExpression nex, IEnumerable<MemberBinding> bindings)
        {
            if (nex != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(nex, bindings);
            }
            return init;
        }

        /// <MetaDataID>{637a0252-8815-4ca0-93d7-c6a25e0e143d}</MetaDataID>
        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
            return this.UpdateListInit(init, n, initializers);
        }

        /// <MetaDataID>{0a31cedd-d1b9-4247-ae5b-316aef97243b}</MetaDataID>
        protected ListInitExpression UpdateListInit(ListInitExpression init, NewExpression nex, IEnumerable<ElementInit> initializers)
        {
            if (nex != init.NewExpression || initializers != init.Initializers)
            {
                return Expression.ListInit(nex, initializers);
            }
            return init;
        }

        /// <MetaDataID>{dec83b79-51a4-4bd7-abf7-4d62f96f1a7e}</MetaDataID>
        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
            return this.UpdateNewArray(na, na.Type, exprs);
        }

        /// <MetaDataID>{3db031ef-9ac5-4417-9515-73ba64c29f10}</MetaDataID>
        protected NewArrayExpression UpdateNewArray(NewArrayExpression na, Type arrayType, IEnumerable<Expression> expressions)
        {
            if (expressions != na.Expressions || na.Type != arrayType)
            {
                if (na.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(arrayType.GetElementType(), expressions);
                }
                else
                {
                    return Expression.NewArrayBounds(arrayType.GetElementType(), expressions);
                }
            }
            return na;
        }

        /// <MetaDataID>{5e8c3a17-af0b-4493-a42d-e3e99d00f0ea}</MetaDataID>
        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
            Expression expr = this.Visit(iv.Expression);
            return this.UpdateInvocation(iv, expr, args);
        }

        /// <MetaDataID>{8adc5407-ace0-4e36-8e4b-d5c7abc7772a}</MetaDataID>
        protected InvocationExpression UpdateInvocation(InvocationExpression iv, Expression expression, IEnumerable<Expression> args)
        {
            if (args != iv.Arguments || expression != iv.Expression)
            {
                return Expression.Invoke(expression, args);
            }
            return iv;
        }
    }
}
