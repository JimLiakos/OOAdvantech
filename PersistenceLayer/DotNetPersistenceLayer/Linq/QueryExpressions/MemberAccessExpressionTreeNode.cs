using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection;

namespace OOAdvantech.Linq.QueryExpressions
{

    /// <MetaDataID>{4183ab0e-93f6-41aa-9643-a3ef465bb44f}</MetaDataID>
    class MemberAccessExpressionTreeNode : ExpressionTreeNode
    {

        /// <MetaDataID>{eb9d85c0-782a-4e15-a6d7-54a1f6142eea}</MetaDataID>
        public MemberAccessExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.MemberAccess)
                throw new System.Exception("Wrong expression type");
        }
        /// <summary>
        /// This method load TypeExpressionsRoute with all TypeExpressionNodes for subsequent parameter expression path
        /// </summary>
        /// <param name="dynamicTypeDataRetrieve">
        /// Defines the corresponding DynamicTypeDataRetrieve of MemberExpresion member DeclareType
        /// </param>
        /// <param name="route">
        /// Defines the route where loeded the TypeExpressionNodes
        /// </param>
        /// <MetaDataID>{6f9fef41-ad05-45fc-b725-0afa1aed2d23}</MetaDataID>
        internal void GetTypeExpressionRouteToSource(IDynamicTypeDataRetrieve dynamicTypeDataRetrieve, List<TypeExpressionNode> route)
        {
            System.Reflection.PropertyInfo propertyInfo = (Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo;
            if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.Properties != null)
            {
                //route.Add(new ExpressionNodeType(memberAccessExpressionTreeNode,dynamicTypeDataRetrieve.Properties[propertyInfo].PropertyType,propertyInfo.PropertyType));

                List<TypeExpressionNode> dynamicPropertyRoute = dynamicTypeDataRetrieve.Properties[propertyInfo].TypeExpresionsPathToSource;

                if (dynamicPropertyRoute.Count == 0)
                    route.Add(new TypeExpressionNode(dynamicTypeDataRetrieve, dynamicTypeDataRetrieve.Type, this, dynamicTypeDataRetrieve.Properties[propertyInfo].PropertyType, propertyInfo.PropertyType));
                //    dynamicPropertyRoute[0].ExpressionTreeNode = memberAccessExpressionTreeNode;

                route.AddRange(dynamicPropertyRoute);

                if (MemberAccess != null)
                {
                    MemberAccess.GetTypeExpressionRouteToSource(dynamicTypeDataRetrieve.Properties[propertyInfo].PropertyType, route);
                }
            }
            else if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.Type.Name == typeof(System.Linq.IGrouping<,>).Name)
            {

                if (propertyInfo.Name == "Key")
                {
                    route.Add(new TypeExpressionNode(dynamicTypeDataRetrieve, dynamicTypeDataRetrieve.Type, this, dynamicTypeDataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve, dynamicTypeDataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve.Type));
                    if (MemberAccess != null)
                        MemberAccess.GetTypeExpressionRouteToSource(dynamicTypeDataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve, route);

                }

            }
            else
            {
                route.Add(new TypeExpressionNode(dynamicTypeDataRetrieve, propertyInfo.DeclaringType, this, null, propertyInfo.PropertyType));
                if (MemberAccess != null)
                    MemberAccess.GetTypeExpressionRouteToSource(null, route);

            }
        }


        public override string ToString()
        {
            return NamePrefix + " : " + "[" + Expression.NodeType.ToString() + " '" + (Expression as MemberExpression).Member.Name + "' ]  " + TypeDescription;// Name;
        }


        /// <summary>
        /// When member access expression referred to static member, the property is true otherwise is false 
        /// </summary>
        /// <MetaDataID>{9e3698e4-dc9d-48c5-a41a-2e888a0bd46b}</MetaDataID>
        public bool IsStaticMember
        {
            get
            {

                if ((Expression as System.Linq.Expressions.MemberExpression).Member.IsField())//.MemberType == System.Reflection.MemberTypes.Field)
                    return ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.FieldInfo).IsStatic;
                if ((Expression as System.Linq.Expressions.MemberExpression).Member.IsProperty() &&
                    ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetGetMethod() != null)
                {
                    return ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetGetMethod().IsStatic;
                }
                if ((Expression as System.Linq.Expressions.MemberExpression).Member.IsProperty()/*.MemberType == System.Reflection.MemberTypes.Property*/ &&
                    ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetSetMethod() != null)
                {
                    return ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetSetMethod().IsStatic;
                }
                if ((Expression as System.Linq.Expressions.MemberExpression).Member.IsMethod())
                    return ((Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.MethodInfo).IsStatic;
                return false;

            }
        }

        public object Value
        {
            get
            {
                if (IsStaticMember)
                {
                    return GetValue(this);
                }
                return null;
            }
        }

        /// <MetaDataID>{31f89230-a8c5-4df0-adec-b529db24d8c0}</MetaDataID>
        private object GetValue(MemberAccessExpressionTreeNode memberAccessExpressionTreeNode)
        {
            object value = null;
            if (memberAccessExpressionTreeNode.IsStaticMember && (memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member is System.Reflection.FieldInfo)
                value = ((memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.FieldInfo).GetValue(null);

            if (memberAccessExpressionTreeNode.IsStaticMember && (memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member is System.Reflection.PropertyInfo)
                value = ((memberAccessExpressionTreeNode.Expression as System.Linq.Expressions.MemberExpression).Member as System.Reflection.PropertyInfo).GetValue(null, null);

            if (memberAccessExpressionTreeNode.Nodes.Count > 0 && memberAccessExpressionTreeNode.Nodes[0] is MemberAccessExpressionTreeNode)
                return GetValue(value, memberAccessExpressionTreeNode.Nodes[0] as MemberAccessExpressionTreeNode);
            else
                return value;
        }

        /// <MetaDataID>{b52c12cb-1c91-4722-8ed2-497cd6ad7d5d}</MetaDataID>
        private object GetValue(object value, MemberAccessExpressionTreeNode memberAccessExpressionTreeNode)
        {
            if (value == null)
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

        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            return BuildDataNodeTree(dataNode, null, linqObjectQuery);

        }

        /// <summary>
        /// Defines the next member access expression treeNode
        /// </summary>
        MemberAccessExpressionTreeNode MemberAccess
        {
            get
            {
                if (Nodes.Count == 0)
                    return null;
                else
                {
                    ExpressionTreeNode node = Nodes[0];
                    if (Nodes[0].Name == "Item()")
                        return Nodes[1] as MemberAccessExpressionTreeNode;
                    else
                        return Nodes[0] as MemberAccessExpressionTreeNode;
                }
            }
        }

        ///<summary>
        ///Builds the data nodes for object access subsequence
        ///</summary>
        ///<param name="dataNode">
        ///Defines the DataNode of Member DeclaringType
        ///</param>
        ///<param name="dynamicTypeDataRetrieve">
        ///Dfines the dynamic type data retriever for member access member DeclareType
        ///</param>
        ///<param name="linqObjectQuery">
        ///Defines the Linq ObjectQuery
        ///</param>
        /// <MetaDataID>{09e3e50d-5b0a-4a13-bb29-8b586b0e2eb8}</MetaDataID>
        public DataNode BuildDataNodeTree(DataNode dataNode, IDynamicTypeDataRetrieve dynamicTypeDataRetrieve, ILINQObjectQuery linqObjectQuery)
        {
            DataNode = dataNode;
            if (MemberAccess != null)
            {
                if (dynamicTypeDataRetrieve == null || (!dynamicTypeDataRetrieve.IsGrouping && dynamicTypeDataRetrieve.Properties == null))
                {
                    ///Native Type
                    DataNode memberDataNode = null;
                    if (OOAdvantech.TypeHelper.IsNullableType((MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member.DeclaringType))
                        memberDataNode = dataNode;
                    else
                    {
                        if (dataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Key && dataNode.Classifier == dataNode.SubDataNodes[0].Classifier)
                            dataNode = dataNode.SubDataNodes[0];

                        memberDataNode = MemberAccess.GetMemberDataNode(dataNode, linqObjectQuery);
                    }
                    return MemberAccess.BuildDataNodeTree(memberDataNode, null, linqObjectQuery);

                }
                else
                {
                    

                    #region Removed code
                    //if (dataNode.Type == DataNode.DataNodeType.Key)
                    //{
                        
                    //    memberDataNode = dynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].SourceDataNode;
                    //    DataNode = memberDataNode;
                    //    DataNode parameterDataNode = MemberAccess.BuildDataNodeTree(memberDataNode, dynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].PropertyType, linqObjectQuery);
                 
                    //    #region Removed code
                    //    //if ((keyDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode))
                    //    //{
                    //    //    DerivedDataNode dataNodeAgent = null;
                    //    //    foreach (DerivedDataNode keySubDataNode in keyDataNode.SubDataNodes)
                    //    //    {
                    //    //        if (keySubDataNode.OrgDataNode == DataNode)
                    //    //        {
                    //    //            dataNodeAgent = keySubDataNode;
                    //    //            break;
                    //    //        }
                    //    //    }
                    //    //    if (dataNodeAgent == null)
                    //    //    {
                    //    //        dataNodeAgent = new DerivedDataNode(DataNode);
                    //    //        dataNodeAgent.ParentDataNode = keyDataNode;
                    //    //    }
                    //    //    parameterDataNode = GroupKeyExpressionTreeNode.BuildDerivedDataNodePath(dataNodeAgent, parameterDataNode);
                    //    //}
                    //    #endregion
                        
                    //    return parameterDataNode;
                    //}
                    #endregion

                    if (dynamicTypeDataRetrieve.IsGrouping && TypeHelper.IsMemberGroupingKey((MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member))
                    {
                        ///Grouping Type
                        DataNode memberDataNode = dynamicTypeDataRetrieve.RootDataNode;
                        DataNode parameterDataNode = MemberAccess.BuildDataNodeTree(memberDataNode, dynamicTypeDataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve, linqObjectQuery);

                        #region Removed code
                        //if ((dynamicTypeDataRetrieve.RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(MemberAccess.DataNode))
                        //{
                        //    DerivedDataNode dataNodeAgent = null;
                        //    foreach (DerivedDataNode keySubDataNode in keyDataNode.SubDataNodes)
                        //    {
                        //        if (keySubDataNode.OrgDataNode == MemberAccess.DataNode)
                        //        {
                        //            dataNodeAgent = keySubDataNode;
                        //            break;
                        //        }
                        //    }
                        //    if (dataNodeAgent == null)
                        //    {
                        //        dataNodeAgent = new DerivedDataNode(MemberAccess.DataNode);
                        //        dataNodeAgent.ParentDataNode = keyDataNode;
                        //    }
                        //    parameterDataNode = GroupKeyExpressionTreeNode.BuildDerivedDataNodePath(dataNodeAgent, parameterDataNode);
                        //}
                        #endregion

                        return parameterDataNode;
                    }
                    else
                    {
                        ///Dynamic Type
                        DataNode memberDataNode = dynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].SourceDataNode;
                        DataNode parameterDataNode = MemberAccess.BuildDataNodeTree(memberDataNode, dynamicTypeDataRetrieve.Properties[(MemberAccess.Expression as System.Linq.Expressions.MemberExpression).Member as PropertyInfo].PropertyType, linqObjectQuery);
                        return parameterDataNode;
                    }
                }
            }

            
            return dataNode;
        }

        /// <MetaDataID>{22f7c68e-94aa-4537-a951-1f48ef3fa91d}</MetaDataID>
        internal DataNode GetMemberDataNode(DataNode parrentDataNode, ILINQObjectQuery linqObjectQuery)
        {

            DataNode memberDataNode = null;
            var netNativeMember = (Expression as System.Linq.Expressions.MemberExpression).Member;
            MetaDataRepository.MetaObject classifierMember = TypeHelper.GetClassifierMember(DeclaringClassifier, netNativeMember);

            if (DerivedDataNode.GetOrgDataNode(parrentDataNode).Classifier == DeclaringClassifier || DerivedDataNode.GetOrgDataNode(parrentDataNode).Classifier.IsA(DeclaringClassifier))
            {
                memberDataNode = DerivedDataNode.GetOrgDataNode(parrentDataNode).SubDataNodes.Where(subDataNode => subDataNode.AssignedMetaObject == classifierMember).FirstOrDefault();
                if (memberDataNode == null)
                {
                    memberDataNode = new DataNode(linqObjectQuery as ObjectQuery, (Expression as System.Linq.Expressions.MemberExpression).Member.Name, classifierMember);
                    if (memberDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (memberDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass != null)
                    {
                        if (MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(Expression.Type)) == (memberDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass)
                            memberDataNode.Classifier = (memberDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass;
                    }
                    memberDataNode.ParentDataNode = DerivedDataNode.GetOrgDataNode(parrentDataNode);
                    if (parrentDataNode is DerivedDataNode)
                    {
                        memberDataNode = new DerivedDataNode(memberDataNode);
                        memberDataNode.ParentDataNode = parrentDataNode;
                    }
                }
                else
                {
                    if (parrentDataNode is DerivedDataNode)
                    {
                        bool memberDerivedDataNodeExist = false;
                        foreach (var memberDerivedDataNode in parrentDataNode.SubDataNodes.OfType<DerivedDataNode>())
                        {
                            if (memberDerivedDataNode.OrgDataNode == memberDataNode)
                            {
                                memberDataNode = memberDerivedDataNode;
                                memberDerivedDataNodeExist = true;
                                break;
                            }
                        }
                        if (!memberDerivedDataNodeExist)
                        {
                            memberDataNode = new DerivedDataNode(memberDataNode);
                            memberDataNode.ParentDataNode = parrentDataNode;
                        }

                    }
                }
            }
            return memberDataNode;
        }


        public ExpressionTreeNode SourceCollection
        {
            get
            {
                ExpressionTreeNode sourceCollection = null;

                //if (DerivedMemberLinqQuery != null)
                //    return this;
                //else  
                if (Parent is ParameterExpressionTreeNode)
                    sourceCollection = (Parent as ParameterExpressionTreeNode).HeadNodeSourceCollection;
                else
                    sourceCollection = (Parent as MemberAccessExpressionTreeNode).SourceCollection;

                if (sourceCollection is QueryExpressions.WhereExpressionTreeNode)
                    sourceCollection = (sourceCollection as QueryExpressions.WhereExpressionTreeNode).SourceCollection;

                if (sourceCollection is QueryExpressions.ParameterExpressionTreeNode)
                    sourceCollection = (sourceCollection as QueryExpressions.ParameterExpressionTreeNode).SourceCollection;


                if (sourceCollection is QueryExpressions.GroupByExpressionTreeNode)
                    sourceCollection = (sourceCollection as QueryExpressions.GroupByExpressionTreeNode).SourceCollection;




                if (sourceCollection.Nodes.Count == 0)
                    return this;
                foreach (ExpressionTreeNode subExpressionTreeNode in sourceCollection.Nodes)
                {

                    if (Name == subExpressionTreeNode.Alias)
                    {
                        if (subExpressionTreeNode is QueryExpressions.ParameterExpressionTreeNode)
                            return (subExpressionTreeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection;

                        return subExpressionTreeNode;
                    }
                    if (subExpressionTreeNode.Expression.NodeType == ExpressionType.New)
                    {
                        foreach (var newExpressionMember in subExpressionTreeNode.Nodes)
                        {
                            if (Name == newExpressionMember.Alias)
                            {
                                if (newExpressionMember is QueryExpressions.ParameterExpressionTreeNode)
                                    return (newExpressionMember as QueryExpressions.ParameterExpressionTreeNode).SourceCollection;

                                //if (newExpressionMember is QueryExpressions.NewExpressionTreeNode)
                                //{
                                //    ExpressionTreeNode newExpressionSource = newExpressionMember.Parent;
                                //    while (newExpressionSource is QueryExpressions.NewExpressionTreeNode)
                                //        newExpressionSource = newExpressionSource.Parent;

                                //    if (newExpressionSource is SelectExpressionTreeNode)
                                //        return (newExpressionSource as SelectExpressionTreeNode).SourceCollection;
                                //}
                                return newExpressionMember;
                            }
                        }
                    }
                }
                return this;

            }
        }




        #region Old Code
        /// <MetaDataID>{8850d730-00fd-4efd-9271-724cf081e8f2}</MetaDataID>
        bool IsSubNodeOfFetchingExpression(ExpressionTreeNode expressionTreeNode)
        {
            if (expressionTreeNode.Parent is FetchingExpressionTreeNode)
                return true;
            else if (expressionTreeNode.Parent != null)
                return IsSubNodeOfFetchingExpression(expressionTreeNode.Parent);
            else
                return false;

        }
        /// <MetaDataID>{35198af1-6586-47a7-8d33-bb3384ad66f9}</MetaDataID>
        bool IsSubNodeOfFetchingExpression()
        {
            return IsSubNodeOfFetchingExpression(this);
        }
        #endregion


        //#region IFilteredSource Members

        //public SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        //{
        //    if (Nodes.Count > 0)
        //    {

        //        ExpressionTreeNode node = Nodes[0];

        //        if (node.Name == "Item()")
        //            node = Nodes[1];
        //        if (node.Expression is System.Linq.Expressions.MemberExpression && (node.Expression as System.Linq.Expressions.MemberExpression).Member.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false).Length > 0)
        //        {

        //        }
        //    }

        //    return searchCondition;

        //}

        //#endregion
        /// <exclude>Excluded</exclude>
        MetaDataRepository.Classifier _DeclaringClassifier;
        
        /// <summary>
        /// Define the classifier of declaring type of member
        /// </summary>
        public MetaDataRepository.Classifier DeclaringClassifier
        {
            get
            {
                if(_DeclaringClassifier==null)
                    _DeclaringClassifier= OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject((Expression as System.Linq.Expressions.MemberExpression).Member.DeclaringType);
                return _DeclaringClassifier;
            }
            
        }
    }
}
