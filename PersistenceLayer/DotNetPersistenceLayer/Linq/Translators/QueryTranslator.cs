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
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Collections.Generic;


namespace OOAdvantech.Linq.Translators
{
    /// <MetaDataID>{74ac7caf-9776-4b10-a3c6-3c0b2f243c54}</MetaDataID>
    internal class QueryTranslator : ExpressionVisitor
    { 
        /// <MetaDataID>{c0bf4ce6-6aef-4de2-b3a3-f3a3fe6d7938}</MetaDataID>
        internal protected override ExpressionTreeNode CreateExpressionTreeNode(ExpressionTreeNodeType expressionTreeNodeType, Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
        {
            switch (expressionTreeNodeType)
            {
                case ExpressionTreeNodeType.BinaryExpression:
                    {
                        return new QueryExpressions.BinaryExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.ArithmeticExpression:
                    {
                        return new QueryExpressions.ArithmeticExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.Constant:
                    {
                        return new QueryExpressions.ConstantExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.MemberAccess:
                    {

                        var memberAccessExpressionTreeNode = new QueryExpressions.MemberAccessExpressionTreeNode(exp, parent, expressionTranslator);

                        //if ((exp as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0)
                        //{
                        //    return memberAccessExpressionTreeNode; 
                        //    ExpressionTreeNode derivedMemberSourceColection = parent;
                        //    while (!(derivedMemberSourceColection is QueryExpressions.SelectExpressionTreeNode))
                        //        derivedMemberSourceColection = derivedMemberSourceColection.Parent;
                        //    derivedMemberSourceColection = derivedMemberSourceColection.Nodes[0];



                        //    System.Linq.Expressions.Expression derivedMemberExpresion = ((exp as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression;

                        //    var root = new OOAdvantech.Linq.ExpressionTreeNode("Root");
                        //    Root.Expression = derivedMemberExpresion;
                        //    expressionTranslator.Visit(derivedMemberExpresion, ref root);

                        //    ExpressionTreeNode deriveMemberExpressionTreeNode = root.Nodes[0];
                        //    ExpressionTreeNode sourceCollection = deriveMemberExpressionTreeNode;
                        //    while (sourceCollection.Nodes.Count > 0)
                        //        sourceCollection = sourceCollection.Nodes[0];

                        //    QueryExpressions.SelectExpressionTreeNode newSelectExpressionTreeNode = new OOAdvantech.Linq.QueryExpressions.SelectExpressionTreeNode(derivedMemberSourceColection.Parent.Expression, null, expressionTranslator);
                        //    parent = new QueryExpressions.ParameterExpressionTreeNode(GetParameterTreeNode(memberAccessExpressionTreeNode).Expression, newSelectExpressionTreeNode, expressionTranslator);
                            
                        //    ReplaceExpressionTreeNode(derivedMemberSourceColection.Parent, newSelectExpressionTreeNode);
                            
                        //    //#########################
                        //    //newSelectExpressionTreeNode.Alias = derivedMemberSourceColection.Alias;
                        //    //#########################

                        //    deriveMemberExpressionTreeNode.Parent = newSelectExpressionTreeNode;
                        //    newSelectExpressionTreeNode.Nodes.Insert(0,deriveMemberExpressionTreeNode);
                        //    deriveMemberExpressionTreeNode.Alias = parent.Name;
                        //    QueryExpressions.MemberAccessExpressionTreeNode memberAccess=derivedMemberSourceColection.Parent.Nodes[1].Nodes[0] as QueryExpressions.MemberAccessExpressionTreeNode;
                        //    sourceCollection.Parent.Nodes[1].Name = memberAccess.Parent.Name;
                        //    string memberAccesName = memberAccess.Name;
                        //    memberAccess=new OOAdvantech.Linq.QueryExpressions.MemberAccessExpressionTreeNode(memberAccess.Expression,sourceCollection.Parent.Nodes[1],expressionTranslator);
                        //    memberAccess.Name = memberAccesName;
                        //    memberAccess.Parent.Nodes[1].Nodes.Add(memberAccess.Parent.Nodes[0]);
                        //    memberAccess.Parent.Nodes[0].Parent = memberAccess.Parent.Nodes[1];
                        //    memberAccess.Parent.Nodes.RemoveAt(0);

                            
                        //    ReplaceExpressionTreeNode(sourceCollection, derivedMemberSourceColection);

                            

                        //}
                        return memberAccessExpressionTreeNode; 

                        
                    }
                case ExpressionTreeNodeType.Parameter:
                    {
                        return new QueryExpressions.ParameterExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.AggregateFunction:
                    {
                        return new QueryExpressions.AggregateFunctionExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.Select:
                    {
                        return new QueryExpressions.SelectExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.SelectMany:
                    {
                        return new QueryExpressions.SelectManyExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.GroupBy:
                    {
                        return new QueryExpressions.GroupByExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.OrderBy:
                    {
                        return new QueryExpressions.OrderByExpressionTreeNode(exp, parent, expressionTranslator,OrderByType.ASC);
                    }
                case ExpressionTreeNodeType.OrderByDescending:
                    {
                        return new QueryExpressions.OrderByExpressionTreeNode(exp, parent, expressionTranslator,OrderByType.DESC);
                    }
                case ExpressionTreeNodeType.Where:
                    {
                        return new QueryExpressions.WhereExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.NewExpression:
                    {
                        if(parent is QueryExpressions.GroupByExpressionTreeNode)
                            return new QueryExpressions.GroupKeyExpressionTreeNode(exp, parent, expressionTranslator);
                        else
                            return new QueryExpressions.NewExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.LikeExpression:
                    {
                        return new QueryExpressions.LikeExpressionTreeNode(exp, parent, expressionTranslator);

                    }
                case ExpressionTreeNodeType.ContainsAny:
                    {
                        
                        return new QueryExpressions.ContainsAnyAllExpressionTreeNode(exp, parent, expressionTranslator);

                    }
                case ExpressionTreeNodeType.ContainsAll:
                    {
                        return new QueryExpressions.ContainsAnyAllExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.EnumerableContains:
                    {
                        return new QueryExpressions.EnumerableContainsExpressionTreeNode(exp, parent, expressionTranslator);
                    }

                case ExpressionTreeNodeType.ObjectCallExpression:
                    {
                        return new QueryExpressions.ObjectMethodCallExpressionTreeNode(exp, parent, expressionTranslator);

                    }
                case ExpressionTreeNodeType.FetchingPlan:
                    {
                        QueryExpressions.FetchingExpressionTreeNode  fetchingExpression=new QueryExpressions.FetchingExpressionTreeNode(exp, parent, expressionTranslator);
                        FetchingExpressions.Add(fetchingExpression);
                        return fetchingExpression;
                    }
                case ExpressionTreeNodeType.RefreshPlan:
                    {
                        QueryExpressions.RefreshExpressionTreeNode refreshExpression = new QueryExpressions.RefreshExpressionTreeNode(exp, parent, expressionTranslator);
                        RefreshExpressions.Add(refreshExpression);
                        return refreshExpression;
                    }
                case ExpressionTreeNodeType.RecursiveLoad:
                    {
                        return new OOAdvantech.Linq.QueryExpressions.RecursiveLoadExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.OfType:
                    {
                        return new OOAdvantech.Linq.QueryExpressions.OfTypeExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.Partition:
                    {
                        return new OOAdvantech.Linq.QueryExpressions.PartitionExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                case ExpressionTreeNodeType.TypeAs:
                    {
                        return new OOAdvantech.Linq.QueryExpressions.TypeAsExpressionTreeNode(exp, parent, expressionTranslator);
                    }
                
                default:
                    throw new System.NotSupportedException();
            }
        }

        //private void ReplaceExpressionTreeNode(ExpressionTreeNode oldExpressionTreeNode, ExpressionTreeNode newExpressionTreeNode)
        //{
        //   // newExpressionTreeNode.Alias = oldExpressionTreeNode.Alias;
        //    int npos= oldExpressionTreeNode.Parent.Nodes.IndexOf(oldExpressionTreeNode);
        //    oldExpressionTreeNode.Parent.Nodes.Remove(oldExpressionTreeNode);
        //    oldExpressionTreeNode.Parent.Nodes.Insert(npos, newExpressionTreeNode);
        //    newExpressionTreeNode.Parent = oldExpressionTreeNode.Parent;
        //    oldExpressionTreeNode.Parent = null;
        //    oldExpressionTreeNode.ReplacingExpressionTreeNode = newExpressionTreeNode;
        //    newExpressionTreeNode.ReplacedExpressionTreeNode = oldExpressionTreeNode;

        //}

        /// <MetaDataID>{6130242b-9e4b-4962-bf78-b00942f6c50c}</MetaDataID>
        private QueryExpressions.ParameterExpressionTreeNode GetParameterTreeNode(ExpressionTreeNode expressionTreeNode)
        {
            while (expressionTreeNode is QueryExpressions.MemberAccessExpressionTreeNode)
                expressionTreeNode = expressionTreeNode.Parent;
            return expressionTreeNode as QueryExpressions.ParameterExpressionTreeNode;

        }

        /// <MetaDataID>{099f1455-283b-42fa-99ad-75f0b9ced6c1}</MetaDataID>
        internal System.Collections.Generic.List<QueryExpressions.FetchingExpressionTreeNode> FetchingExpressions = new System.Collections.Generic.List<OOAdvantech.Linq.QueryExpressions.FetchingExpressionTreeNode>();
        internal System.Collections.Generic.List<QueryExpressions.RefreshExpressionTreeNode> RefreshExpressions = new System.Collections.Generic.List<OOAdvantech.Linq.QueryExpressions.RefreshExpressionTreeNode>();
        //readonly OrderByTranslator _orderByTranslator;
        // ProjectionTranslator _selectTranslator;
        //TakeTranslator _takeTranslator;


        /// <MetaDataID>{9c58e3e2-8628-417a-b810-2caf4b83ba5a}</MetaDataID>
        internal QueryTranslator(ObjectQuery linqObjectQuery)
            : base(null)
        {
            _LINQObjectQuery = linqObjectQuery;
            MainExpressionVisitor = this;
        }
        /// <MetaDataID>{96f6d4ac-602d-4ebb-984f-40325dd66711}</MetaDataID>
        internal static void GetPaths(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ref System.Collections.Generic.List<DataNode> paths)
        {
            paths.Add(dataNode);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                GetPaths(subDataNode, ref paths);
            }
        }

        /// <MetaDataID>{25907e1f-2167-4adb-a0fa-b39fe166ec34}</MetaDataID>
        ExpressionTreeNode Root;
        /// <MetaDataID>{af1deaef-ef1d-42b7-aa63-3a176337a65f}</MetaDataID>
        internal override void Translate(Expression query)
        {
            //ExtendExpressionVisitor expressionVisitor = new ExtendExpressionVisitor();

            query = Visit(query);
            Root =  new OOAdvantech.Linq.ExpressionTreeNode("Root",this);
            Root.Expression = query;
            Visit(query, ref Root);
            ExpressionTreeNode.lastExpressionTree = Root;
            if (query.NodeType == ExpressionType.Lambda)
            {
                Root.Alias = (query as LambdaExpression).Parameters[0].Name;
            }

            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = null;
            dataNode = Root.Nodes[0].BuildDataNodeTree(dataNode, LINQObjectQuery as ILINQObjectQuery);
            //if (dataNode.Type == DataNode.DataNodeType.Group)
            dataNode = dataNode.HeaderDataNode;


            System.Collections.Generic.Dictionary<DataNode, DataNode> replacedDataNodes = new System.Collections.Generic.Dictionary<DataNode, DataNode>();
            RemoveAllTemporaryNodes( dataNode.HeaderDataNode, replacedDataNodes);

            Translators.QueryTranslator.ShowDataNodePathsInOutLog(dataNode);

  
            if (dataNode.HeaderDataNode.Temporary && dataNode.HeaderDataNode.Name == "Root" && dataNode.HeaderDataNode.SubDataNodes.Count == 1)
            {
                replacedDataNodes = new System.Collections.Generic.Dictionary<DataNode, DataNode>();
                dataNode=dataNode.HeaderDataNode;//.SubDataNodes[0];
                RemoveTemporaryNodes(ref dataNode, replacedDataNodes);
                
                dataNode.ParentDataNode = null;
            }

            System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
            RootPaths.Add(dataNode);


             LINQObjectQuery.  DataTrees.Add(dataNode);
             if (LINQObjectQuery is ObjectsContextQuery)
             {
                 string errors = null;
                 (LINQObjectQuery as ObjectsContextQuery).BuildDataNodeTree(ref errors);
                 BuildSearchCondition();
             }
             if (LINQObjectQuery is QueryOnRootObject)
             {
                 string errors = null;
                 (LINQObjectQuery as QueryOnRootObject).BuildDataNodeTree(ref errors);
                 BuildSearchCondition();
             }

            foreach (DataNode rootDataNode in RootPaths)
                rootDataNode.MergeSearchConditions();

            GetPaths(dataNode, ref paths);

            //return;
            //foreach (DataNode mdataNode in paths)
            //{
            //    string message =GetFullName( mdataNode) + " " + mdataNode.Alias;
            //    if (mdataNode.ParticipateInWereClause)
            //        message += " PW";
            //    if (mdataNode.ParticipateInSelectClause)
            //        message += " SL";
            //    System.Diagnostics.Debug.WriteLine(message);
            //}
            ExpressionTreeNode.lastExpressionTree = Root;
        }


        /// <MetaDataID>{4b9c192b-ad37-4d48-bdd0-9f7569927b89}</MetaDataID>
        internal void TranslateDerivedMember(Expression query,DataNode rrootDataNode,ExpressionTreeNode sourceCollectionExpression )
        {

            Root = new OOAdvantech.Linq.ExpressionTreeNode("Root",this);
            Root.Expression = query;
            Visit(query, ref Root);
            ExpressionTreeNode.lastExpressionTree = Root;
            if (query.NodeType == ExpressionType.Lambda)
            {
                Root.Alias = (query as LambdaExpression).Parameters[0].Name;
            }

            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = null;
            dataNode = Root.Nodes[0].BuildDataNodeTree(dataNode, LINQObjectQuery as ILINQObjectQuery);
            //if (dataNode.Type == DataNode.DataNodeType.Group)
            dataNode = dataNode.HeaderDataNode;


            System.Collections.Generic.Dictionary<DataNode, DataNode> replacedDataNodes = new System.Collections.Generic.Dictionary<DataNode, DataNode>();
            RemoveAllTemporaryNodes(dataNode.HeaderDataNode, replacedDataNodes);

            Translators.QueryTranslator.ShowDataNodePathsInOutLog(dataNode);


            if (dataNode.HeaderDataNode.Temporary && dataNode.HeaderDataNode.Name == "Root" && dataNode.HeaderDataNode.SubDataNodes.Count == 1)
            {
                replacedDataNodes = new System.Collections.Generic.Dictionary<DataNode, DataNode>();
                dataNode = dataNode.HeaderDataNode;//.SubDataNodes[0];
                RemoveTemporaryNodes(ref dataNode, replacedDataNodes);

                dataNode.ParentDataNode = null;
            }

            System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
            RootPaths.Add(dataNode);


            LINQObjectQuery.DataTrees.Add(dataNode);
            if (LINQObjectQuery is ObjectsContextQuery)
            {
                string errors = null;
                (LINQObjectQuery as ObjectsContextQuery).BuildDataNodeTree(ref errors);
                BuildSearchCondition();
            }
            if (LINQObjectQuery is QueryOnRootObject)
            {
                string errors = null;
                (LINQObjectQuery as QueryOnRootObject).BuildDataNodeTree(ref errors);
                BuildSearchCondition();
            }

            foreach (DataNode rootDataNode in RootPaths)
                rootDataNode.MergeSearchConditions();
            
            (LINQObjectQuery as ILINQObjectQuery).QueryResult.ParticipateInQueryResults(null);
            foreach (var fetchingExpression in FetchingExpressions)
                fetchingExpression.ParticipateInSelectList();

            foreach (var refreshExpression in RefreshExpressions)
                refreshExpression.ParticipateInSelectList();

            GetPaths(dataNode, ref paths);

            //return;
            foreach (DataNode mdataNode in paths)
            {
                string message = GetFullName(mdataNode) + " " + mdataNode.Alias;
                if (mdataNode.ParticipateInWereClause)
                    message += " PW";
                if (mdataNode.ParticipateInSelectClause)
                    message += " SL";
                System.Diagnostics.Debug.WriteLine(message);
            }
            ExpressionTreeNode.lastExpressionTree = Root;
        }


        /// <MetaDataID>{9c6de6f0-cbad-4457-a9e4-93f8634912ea}</MetaDataID>
        internal void BuildSearchCondition()
        {
            if (Root.Nodes[0] is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                (Root.Nodes[0] as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
            else if (Root.Nodes[0] is QueryExpressions.NewExpressionTreeNode)
            {
                foreach (var newTypeMember in Root.Nodes[0].Nodes)
                {
                    if (newTypeMember is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                        (newTypeMember as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
                }
            }


            System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
            GetPaths(RootPaths[0], ref paths);

            return;
            foreach (DataNode mdataNode in paths)
            {
                string message = GetFullName( mdataNode)+ " " + mdataNode.Alias;
                if (mdataNode.ParticipateInWereClause)
                    message += " PW";
                if (mdataNode.ParticipateInSelectClause)
                    message += " SL";
                //System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <MetaDataID>{60d2122d-1f93-4d78-a54a-c197b519fdde}</MetaDataID>
        internal SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        {
            if (Root.Nodes[0] is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                (Root.Nodes[0] as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
            else if (Root.Nodes[0] is QueryExpressions.NewExpressionTreeNode)
            {
                foreach (var newTypeMember in Root.Nodes[0].Nodes)
                {
                    if (newTypeMember is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                        (newTypeMember as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
                }
            }


            System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
            GetPaths(RootPaths[0], ref paths);

            return searchCondition;
            foreach (DataNode mdataNode in paths)
            {
                string message = GetFullName(mdataNode) + " " + mdataNode.Alias;
                if (mdataNode.ParticipateInWereClause)
                    message += " PW";
                if (mdataNode.ParticipateInSelectClause)
                    message += " SL";
                //System.Diagnostics.Debug.WriteLine(message);
            }
        }
        /// <MetaDataID>{894f220f-b3bc-42c1-af0a-8338a1c33c34}</MetaDataID>
        internal System.Collections.Generic.Dictionary<string, DataNode> MultipleAlias = new System.Collections.Generic.Dictionary<string, DataNode>();


        /// <summary>
        /// Search data nodes tree for data node with alias of parameter alias
        /// </summary>
        /// <param name="dataNode">
        /// Defines the root data node of tree
        /// </param>
        /// <param name="alias">
        /// Defines the alias name of data node 
        /// </param>
        /// <returns>
        /// Returns the data node with alias name
        /// if there isn't returns null
        /// </returns>
        /// <MetaDataID>{37441619-ea64-4b29-b0ee-dde17596e395}</MetaDataID>
        internal static DataNode GetDataNodeWithAlias(DataNode dataNode, string alias)
        {
            try
            {
                if (dataNode.HasAlias(alias))
                    return dataNode;
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.HasAlias(alias))
                        return subDataNode;
                    DataNode aliasDataNode = GetDataNodeWithAlias(subDataNode, alias);
                    if (aliasDataNode != null)
                        return aliasDataNode;
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return null;
        }


        /// <summary>
        /// Search data nodes tree for the corresponding data node of temporary data node
        /// the corresponding has the name of temporary data node as alias
        /// </summary>
        /// <param name="dataNode">
        /// Defines the root data node of tree
        /// </param>
        /// <param name="temporaryDataNode">
        /// Defines the temporary data node.
        /// System search for the corresponding data node of temporary data node
        /// </param>
        /// <returns></returns>
        /// <MetaDataID>{b72d9c20-20e3-45e5-b0d4-85de41190239}</MetaDataID>
        internal static DataNode GetCorrespondingDataNode(DataNode dataNode, DataNode temporaryDataNode)
        {


            try
            {
                //if (!dataNode.Temporary)
                //    return null;
                if (dataNode.HasAlias(temporaryDataNode.Name) && dataNode != temporaryDataNode)
                    return dataNode;
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.HasAlias(temporaryDataNode.Name) && subDataNode != temporaryDataNode)
                        return subDataNode;
                    DataNode aliasDataNode = GetCorrespondingDataNode(subDataNode, temporaryDataNode);
                    if (aliasDataNode != null)
                        return aliasDataNode;
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return null;
        }
   



        //internal static DataNode GetDataNodeWithFullName(DataNode dataNode, string fullName)
        //{
        //    if (dataNode.FullName == fullName)
        //        return dataNode;
        //    foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //    {
        //        if (fullName == subDataNode.FullName)
        //            return subDataNode;
        //        DataNode fullNameDataNode = GetDataNodeWithFullName(subDataNode, fullName);
        //        if (fullNameDataNode != null)
        //            return fullNameDataNode;
        //    }


        //    return null;
        //}




        //internal QueryInfo Translate(Expression query)
        //{
        //  Visit(query);

     

        //  return ConvertToExecutableQuery(query);
        //}
        /// <MetaDataID>{f2aa8a88-f75a-4e1f-bcb5-ebdbcb268201}</MetaDataID>
        private bool GetSourceClassifier(Expression query, out OOAdvantech.MetaDataRepository.Classifier source)
        {
            source = null;

            MethodCallExpression me = query as MethodCallExpression;
            ConstantExpression ce;
            if (me == null)
            {
                ce = query as ConstantExpression;
            }
            else
            {
                // recurse down all the method calls to find the source object
                while (true)
                {
                    if (me.Arguments[0] is MethodCallExpression)
                        me = me.Arguments[0] as MethodCallExpression;
                    else
                        break;
                }

                ce = me.Arguments[0] as ConstantExpression;
            }

            if (ce != null)
            {
                if (ce.Value != null)
                {
                    System.Type type = ce.Value.GetType();
                    if (type.GetMetaData().IsGenericType)
                    {
                        source = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(type.GetMetaData().GetGenericArguments()[0]);
                        return true;
                    }
                }
            }

            return false;
        }







        #region Visitors

        /// <MetaDataID>{f0ad06c1-0b8f-433a-a36e-51e388edb981}</MetaDataID>
        protected override Expression VisitUnary(UnaryExpression u, ref ExpressionTreeNode parent)
        {
            if ((parent is OOAdvantech.Linq.QueryExpressions.WhereExpressionTreeNode || parent is OOAdvantech.Linq.QueryExpressions.BinaryExpressionTreeNode) &&(
               (u.Operand.NodeType == ExpressionType.MemberAccess && u.NodeType == ExpressionType.Not)||
                (u.Operand.NodeType == ExpressionType.Call && u.NodeType == ExpressionType.Not)))
            {
                parent = CreateExpressionTreeNode(ExpressionTreeNodeType.BinaryExpression,u , parent, this);
                (parent as QueryExpressions.BinaryExpressionTreeNode).NotExpression = true;
            }
            Expression unary = null;
            if (u.NodeType == ExpressionType.TypeAs)
            {

                parent = CreateExpressionTreeNode(ExpressionTreeNodeType.TypeAs, u, parent, this);
                ExpressionTreeNode typeAs = parent;
                unary=base.VisitUnary(u, ref parent);
                parent = typeAs;

            }
            else
                unary= base.VisitUnary(u, ref parent);
            switch (u.Operand.NodeType)
            {

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    {
                        if (u.NodeType == ExpressionType.Not)
                            (parent.Nodes[parent.Nodes.Count - 1] as QueryExpressions.BinaryExpressionTreeNode).NotExpression = true;
                        break;
                    }

                default:
                    break;
            }
           

            return unary;

        }

        /// <MetaDataID>{62539c9b-d47c-40ea-8900-fd736ef68381}</MetaDataID>
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
                //case "OrderBy":
                //case "ThenBy":
                //    Visit(mc.Arguments[0], ref parent);
                //    // is this really a proper Order By?
                //    var orderLambda = GetLambdaWithParamCheck(mc);
                //    if (orderLambda == null)
                //        break;

                    //VisitOrderBy(orderLambda, DataNode.OrderByType.ASC, ref parent);
                    //break;
                //case "OrderByDescending":
                //case "ThenByDescending":
                //    Visit(mc.Arguments[0], ref parent);
                //    // is this really a proper Order By Descending?
                //    var orderDescLambda = GetLambdaWithParamCheck(mc);
                //    if (orderDescLambda == null)
                //        break;

                //    VisitOrderBy(orderDescLambda, DataNode.OrderByType.DESC, ref parent);
                //    break;
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
                    if (mc.Arguments.Count != 2)
                        break;

                    VisitTake(mc.Arguments[1], ref parent);
                    break;

                case "First":
                    Visit(mc.Arguments[0], ref parent);
                    // This custom provider does not support the use of a First operator
                    // that takes a predicate. Therefore we check to ensure that no more
                    // than one argument is provided.
                    if (mc.Arguments.Count != 1)
                        break;

                    VisitFirst(false);
                    break;

                case "FirstOrDefault":
                    Visit(mc.Arguments[0], ref parent);
                    // This custom provider does not support the use of a FirstOrDefault
                    // operator that takes a predicate. Therefore we check to ensure that
                    // no more than one argument is provided.
                    if (mc.Arguments.Count != 1)
                        break;

                    VisitFirst(true);
                    break;

                default:
                    return base.VisitMethodCall(mc, ref parent);
            }


            return mc;
        }



        /// <MetaDataID>{d10a5512-9f5b-4be9-86b0-8e7cf13a9f8c}</MetaDataID>
        private void VisitOrderBy(LambdaExpression predicate, OrderByType direction, ref  ExpressionTreeNode parent)
        {
            
        }

        /// <MetaDataID>{a555f732-263b-400e-9b90-221d115d3ec7}</MetaDataID>
        private void VisitTake(Expression takeValue, ref  ExpressionTreeNode parent)
        {
        }


        /// <MetaDataID>{c2e3846f-ce6a-45ae-b82a-19c49fdcfc10}</MetaDataID>
        private void VisitFirst(bool useDefault)
        {
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Check to see if the expression is valid for
        /// Select, Where, OrderBy, ThenBy, OrderByDescending and ThenByDescending
        /// and then return the lmbda section
        /// </summary>
        /// <returns></returns>
        /// <MetaDataID>{5d68de53-f889-4d8c-ba7d-9378e3946ecc}</MetaDataID>
        private LambdaExpression GetLambdaWithParamCheck(MethodCallExpression mc)
        {
            if (mc.Arguments.Count != 2 || !IsLambda(mc.Arguments[1]))
                return null;

            var lambda = GetLambda(mc.Arguments[1]);
            return (lambda.Parameters.Count != 1) ? null : lambda;
        }

        /// <MetaDataID>{6e4a25bb-1abf-48c6-acb0-d787c14bfcf3}</MetaDataID>
        private bool IsLambda(Expression expression)
        {
            return RemoveQuotes(expression).NodeType == ExpressionType.Lambda;
        }

        /// <MetaDataID>{fef7b4b9-8c2a-403e-bb1f-47eef58df277}</MetaDataID>
        private LambdaExpression GetLambda(Expression expression)
        {
            return RemoveQuotes(expression) as LambdaExpression;
        }

        /// <MetaDataID>{966d59cc-f842-49b6-a50d-30a1f42e7453}</MetaDataID>
        private Expression RemoveQuotes(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Quote)
            {
                expression = ((UnaryExpression)expression).Operand;
            }
            return expression;
        }
        #endregion

        /// <MetaDataID>{04a778b4-3a76-434a-bc30-bc6f6ddd99a4}</MetaDataID>
        static string GetFullName(DataNode dataNode)
        {
            if (dataNode.ParentDataNode == null)
                return dataNode.Name;
            else
                return GetFullName(dataNode.ParentDataNode) + "." + dataNode.Name;
        }
        /// <MetaDataID>{7746c2c3-4849-43be-b02d-dabae5190fdf}</MetaDataID>
        internal static void ShowDataNodePathsInOutLog(DataNode dataNode)
        {
            
            System.Collections.Generic.List<DataNode> paths = new System.Collections.Generic.List<DataNode>();
            Translators.QueryTranslator.GetPaths(dataNode, ref paths);
            foreach (DataNode mdataNode in paths)
            {
                string message =GetFullName( mdataNode);
                foreach (string alias in mdataNode.Aliases)
                    message += " " + alias;

                //foreach (string alias in mdataNode.BranchAliass)
                //    message += " " + alias;


                if (mdataNode.ParticipateInWereClause)
                    message += " PW";
                if (mdataNode.ParticipateInSelectClause)
                    message += " SL";
                System.Diagnostics.Debug.WriteLine(message);
            }
        }
        ///// <summary>
        ///// This method check if there is extra route from DataNode header to expression DataNode
        ///// In case where exist extra route return the extra route otherwise return an empty stack
        ///// </summary>
        ///// <param name="expression">
        ///// Defines the expression with the route end DataNode
        ///// </param>
        ///// <returns>
        ///// Return the route as stack collection
        ///// </returns>
        //static Stack<DataNode> GetExpressionDataNodeExtraRoute(ExpressionTreeNode expression)
        //{
        //    OOAdvantech.Linq.QueryExpressions.ParameterExpressionTreeNode parameterExpression = expression as OOAdvantech.Linq.QueryExpressions.ParameterExpressionTreeNode;
        //    if (parameterExpression == null || parameterExpression.MemberAccess == null)
        //        return new Stack<DataNode>();
        //    List<DataNode> path = null;
        //    List<TypeExpressionNode> route = new List<TypeExpressionNode>();


        //    parameterExpression.MemberAccess.GetTypeExpressionRouteToSource(parameterExpression.RootDynamicTypeDataRetrieve, route);
        //    route.Reverse();
        //    path = GetDataNodePath(route[0].ExpressionTreeNode.DataNode);
        //    var orgPath = path;
        //    foreach (var node in route)
        //    {
        //        if (node.SourceDynamicTypeDataRetrieve != null && node.SourceDynamicTypeDataRetrieve.GroupingMetaData != null && node.ExpressionTreeNode is OOAdvantech.Linq.QueryExpressions.MemberAccessExpressionTreeNode &&
        //            (node.ExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member.Name == "Key")
        //        {
        //            DataNode groupKeyDataNode = route[route.IndexOf(node) - 1].ExpressionTreeNode.DataNode;

        //            var keyPath = GetDataNodePath(node.SourceDynamicTypeDataRetrieve.RootDataNode.SubDataNodes[0]);
        //            foreach (var dataNode in path.GetRange(path.IndexOf(groupKeyDataNode), path.Count - path.IndexOf(groupKeyDataNode)))
        //                keyPath.Add(dataNode);
        //            path = keyPath;
        //        }
        //    }
        //    if (path.Count != orgPath.Count)
        //    {
        //        path.Reverse();
        //        return new Stack<DataNode>(path);
        //    }
        //    else
        //    {
        //        for (int i = 0; i != path.Count; i++)
        //        {

        //            if (path[i] != orgPath[i])
        //            {
        //                path.Reverse();
        //                return new Stack<DataNode>(path);
        //            }
        //        }
        //    }
        //    return new Stack<DataNode>();
        //}

        ///// <summary>
        ///// Thi method translate the path of DataNodes from header DataNode to the parameter DataNode
        ///// </summary>
        ///// <param name="dataNode">
        ///// Defines the last DataNode of path
        ///// </param>
        ///// <returns>
        ///// return a collection with DataNodes from HeaderDataNode to the DataNode of parameter 
        ///// </returns>
        // static List<DataNode> GetDataNodePath(DataNode dataNode)
        //{
        //    List<DataNode> path = new List<DataNode>();
        //    while (dataNode != null)
        //    {
        //        path.Insert(0, dataNode);
        //        dataNode = dataNode.ParentDataNode;
        //    }
        //    return path;


        //}

    }
}
