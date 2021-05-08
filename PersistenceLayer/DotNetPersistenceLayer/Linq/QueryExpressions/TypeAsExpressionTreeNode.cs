using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{878e0eda-5b58-4a87-b404-7bdd2e534925}</MetaDataID>
    class TypeAsExpressionTreeNode : ExpressionTreeNode, IFilteredSource
    {

        public TypeAsExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {

        }
        internal override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode BuildDataNodeTree(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

            dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            DataNode = dataNode;
            //Nodes[0].DataNode.CastingTypes.Add(Expression.Type);
            if (Nodes.Count > 1)
            {
                if (Nodes[1] is MemberAccessExpressionTreeNode)
                {

                    ExpressionTreeNode node = Nodes[1];

                    if (node.Name == "Item()")
                        node = Nodes[1];


                    string name = node.Name;
                    bool exist = false;
                    if (dataNode.Alias == name && Expression.Type.Name.IndexOf("<>f__AnonymousType") == 0)
                    {
                        exist = true;
                    }
                    if (Expression.Type.GetMetaData().IsGenericType &&
                        !Expression.Type.GetMetaData().IsGenericTypeDefinition &&
                        Expression.Type.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                    {
                        exist = true;
                    }
                    else
                    {
                        foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in dataNode.SubDataNodes)
                        {
                            if ((subDataNode.Name == name&&subDataNode.CastingParentType==Expression.Type) || subDataNode.HasAlias(name))
                            {
                                if (Expression.Type.Name.IndexOf("<>f__An") != 0 && IsSubNodeOfFetchingExpression())
                                {
                                    // if (!subDataNode.BranchParticipateInWereClause)
                                    {
                                        exist = true;
                                        dataNode = subDataNode;
                                    }
                                }
                                else
                                {
                                    exist = true;
                                    dataNode = subDataNode;
                                }
                                break;

                            }

                        }
                    }
                    if (dataNode.Type == DataNode.DataNodeType.Key)
                    {
                        foreach (DataNode groopKeyDataNode in (dataNode.ParentDataNode as GroupDataNode) .GroupKeyDataNodes)
                        {
                            if (groopKeyDataNode.HasAlias(name))
                            {
                                dataNode = groopKeyDataNode;
                                exist = true;
                                break;
                            }
                        }
                    }
                    if (!exist)
                    {
                        //if (node.Expression is System.Linq.Expressions.MemberExpression && (node.Expression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0)
                        //{
                        //    #region Build DataNodes Tree for derived member
                        //    System.Reflection.PropertyInfo propertyInfo = (node.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo;
                        //    DataNode dervideMemberDataNodeRoot = null;
                        //    DataNode dervideMemberDataNode = null;
                        //    if (!(linqObjectQuery as LINQStorageObjectQuery).DervideMembersDataNodeRoots.ContainsKey(dataNode))
                        //        (linqObjectQuery as LINQStorageObjectQuery).DervideMembersDataNodeRoots[dataNode] = new Dictionary<System.Reflection.PropertyInfo, DataNode>();

                        //    if (!(linqObjectQuery as LINQStorageObjectQuery).DervideMembersDataNodeRoots[dataNode].TryGetValue(propertyInfo, out dervideMemberDataNode))
                        //    {
                        //        //dervideMemberDataNodeRoot = (new LINQStorageObjectQuery(((node.Expression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression, (this.ExpressionTranslator.LINQObjectQuery as LINQStorageObjectQuery).DataContext).QueryResult as IDynamicTypeDataRetrieve).RootDataNode;

                        //        node.DerivedMemberLinqQuery = new LINQStorageObjectQuery(((node.Expression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression, (this.ExpressionTranslator.LINQObjectQuery as LINQStorageObjectQuery));
                        //        dervideMemberDataNodeRoot = (node.DerivedMemberLinqQuery.QueryResult as IDynamicTypeDataRetrieve).RootDataNode;

                        //        if (OOAdvantech.MetaDataRepository.Classifier.GetClassifier(propertyInfo.DeclaringType).IsA(dervideMemberDataNodeRoot.Classifier) ||
                        //            dervideMemberDataNodeRoot.Classifier == OOAdvantech.MetaDataRepository.Classifier.GetClassifier(propertyInfo.DeclaringType))
                        //        {
                        //            dervideMemberDataNode = dervideMemberDataNodeRoot.ObjectQuery.SelectListItems[0];
                        //            (linqObjectQuery as LINQStorageObjectQuery).DervideMembersDataNodeRoots[dataNode][propertyInfo] = dervideMemberDataNode;
                        //            foreach (DataNode subDataNode in new List<DataNode>(dervideMemberDataNodeRoot.SubDataNodes))
                        //            {
                        //                subDataNode.ParentDataNode = dataNode;
                        //                //subDataNode.SearchCondition = dervideMemberDataNodeRoot.SearchCondition;
                        //            }
                        //            foreach (string paramName in dervideMemberDataNodeRoot.ObjectQuery.Parameters.Keys)
                        //            {
                        //                if (dataNode.ObjectQuery.Parameters.ContainsKey(paramName))
                        //                    throw new System.Exception(string.Format("Parameter {0} already exist", paramName));
                        //                else
                        //                    dataNode.ObjectQuery.Parameters[paramName] = dervideMemberDataNodeRoot.ObjectQuery.Parameters[paramName];
                        //            }
                        //        }
                        //        else
                        //            throw new System.Exception("Derived member query source type mismatch");
                        //        node.DerivedMemberLinqQuery.QueryResult.RootDataNode = dataNode;
                        //    }
                        //    dataNode = dervideMemberDataNode;
                        //    #endregion
                        //}
                        //else
                        {

                            //Type type = TypeHelper.GetElementType(((Nodes[0] as MyTreeNode).Expression as MemberExpression).Type);
                            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                            subDataNode.Name = name;
                            if ((node.Expression as System.Linq.Expressions.MemberExpression).Type.Name.IndexOf("<>f__An") != 0)
                            {
                                if ((node.Expression as System.Linq.Expressions.MemberExpression).Type.GetMetaData().IsGenericType &&
                                    !(node.Expression as System.Linq.Expressions.MemberExpression).Type.GetMetaData().IsGenericTypeDefinition &&
                                   (node.Expression as System.Linq.Expressions.MemberExpression).Type.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                                {
                                    subDataNode.Classifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(TypeHelper.GetElementType((node.Expression as System.Linq.Expressions.MemberExpression).Type.GetMetaData().GetGenericArguments()[0]));
                                }
                                else
                                    subDataNode.Classifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(TypeHelper.GetElementType((node.Expression as System.Linq.Expressions.MemberExpression).Type));
                            }
                            subDataNode.ParentDataNode = dataNode;
                            if ((Expression.Type.Name.IndexOf("<>f__An") == 0 || Expression.Type.Name.IndexOf("IGrouping") == 0) && dataNode.Type != DataNode.DataNodeType.Key)// && Parent.Expression.NodeType==ExpressionType.Constant)
                                subDataNode.Temporary = true;
                            dataNode = subDataNode;
                        }
                        dataNode.CastingParentType = Expression.Type;
                    }
                    dataNode = node.BuildDataNodeTree(dataNode, linqObjectQuery);
                    _DataNode = dataNode;
                }
                _DataNode.Alias = Alias;
                return dataNode;
            }

            return dataNode;
        }



        public  void BuildDataFilter()
        {
            SearchCondition searchCondition = null;
            if (searchCondition == null)
            {
                SearchTerm searchTerm = new SearchTerm();

                List<SearchTerm> searchTerms = new List<SearchTerm>();
                searchTerms.Add(searchTerm);
                searchCondition = new SearchCondition(searchTerms, ExpressionTranslator.LINQObjectQuery);
                _FilterDataCondition = searchCondition;
            }
            ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
            DataNode LeftTermDataNode = Nodes[0].DataNode;
            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.OjectAttribute || LeftTermDataNode is AggregateExpressionDataNode)
                comparisonTerms[0] = new ObjectAttributeComparisonTerm(LeftTermDataNode, /*OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]),*/ ExpressionTranslator.LINQObjectQuery);
            if (LeftTermDataNode != null && LeftTermDataNode.Type == DataNode.DataNodeType.Object)
                comparisonTerms[0] = new ObjectComparisonTerm(LeftTermDataNode, /*OOAdvantech.Linq.Translators.QueryTranslator.GetExpressionDataNodeExtraRoute(Nodes[0]),*/ ExpressionTranslator.LINQObjectQuery);

            object constantValue = TypeHelper.GetElementType(Expression.Type);
            string parameterName = "p" + Nodes[0].GetHashCode().ToString();
            ExpressionTranslator.LINQObjectQuery.Parameters.Add(parameterName, constantValue);
            comparisonTerms[1] = new ParameterComparisonTerm(parameterName, ExpressionTranslator.LINQObjectQuery);

            SearchFactor searchFactor = new SearchFactor();
            searchFactor.Criterion = new Criterion(Criterion.ComparisonType.TypeIs, comparisonTerms, ExpressionTranslator.LINQObjectQuery, true, searchFactor);
            searchCondition.SearchTerms[0].AddSearchFactor(searchFactor);
           

            
        }




        public override SearchCondition FilterDataCondition
        {
            get
            {
                if (_FilterDataCondition != null)
                    return _FilterDataCondition;
                else
                    return Nodes[0].FilterDataCondition;

            }
        }


        bool IsSubNodeOfFetchingExpression(ExpressionTreeNode expressionTreeNode)
        {
            if (expressionTreeNode.Parent is FetchingExpressionTreeNode)
                return true;
            else if (expressionTreeNode.Parent != null)
                return IsSubNodeOfFetchingExpression(expressionTreeNode.Parent);
            else
                return false;

        }
        bool IsSubNodeOfFetchingExpression()
        {
            return IsSubNodeOfFetchingExpression(this);
        }

        public override string Name
        {
            get
            {
                if (Nodes.Count == 0)
                    return base.Name;
                else
                    return Nodes[0].Name;
            }
            set
            {
                base.Name = value;
            }
        }


    }
}
