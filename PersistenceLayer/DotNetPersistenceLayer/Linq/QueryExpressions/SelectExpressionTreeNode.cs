using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Linq.Translators;
using System.Reflection;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{9fe3f8ce-3eb2-4a91-925e-91ec5c8e1165}</MetaDataID>
    class SelectExpressionTreeNode : MethodCallAsCollectionProviderExpressionTreeNode
    {
        /// <MetaDataID>{a7b786b9-80cc-47d3-866b-92db0df0794b}</MetaDataID>
        public SelectExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
        }

        ///<summary>
        ///Attach nested query expression DataNodes to main DataNode tree.
        ///This method used when in selection list of query there are members, 
        ///where are the result of a nested query.  
        ///</summary>
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
        /// <MetaDataID>{1d21ef6f-cc78-470b-898c-883d0ecdb1bd}</MetaDataID>
        private void AttatchNestedQueryToMainDataTree()
        {
            foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
            {
                if (treeNode is MethodCallAsCollectionProviderExpressionTreeNode)
                {
                    ExpressionTreeNode expressionTreeNode = treeNode;
                    while (expressionTreeNode.Nodes.Count > 0)
                    {
                        if (expressionTreeNode is MethodCallAsCollectionProviderExpressionTreeNode &&
                            (expressionTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).ReferenceDataNode != null &&
                            !treeNode.DataNode.IsSameOrParentDataNode((expressionTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).ReferenceDataNode))
                        {
                            treeNode.DataNode.ParentDataNode = (expressionTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).ReferenceDataNode;
                            break;
                        }
                        else
                            expressionTreeNode = expressionTreeNode.Nodes[0];
                    }
                }
            }
        }






        /// <MetaDataID>{f080ff47-3e26-4fb6-a0d1-e481602db137}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {



            BuildSelectionSourceDataNodeTree(dataNode, linqObjectQuery);

            if (SourceCollection.DataNode.ParentDataNode != null)
                ReferenceDataNode = SourceCollection.DataNode.ParentDataNode;

            if (Parent is MethodCallAsCollectionProviderExpressionTreeNode)
            {
                if (SourceCollection.DataNode.Alias == Alias)
                {
                    DataNode = SourceCollection.DataNode;
                    dataNode = DataNode;
                }
                else
                {

                    //  DataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, Alias);
                    //DataNode = SelectCollection.BuildDataNodeTree(SourceCollection.DataNode, linqObjectQuery);
                    DataNode = SelectCollection.BuildDataNodeTree(SelectionCollectionSource.DataNode, linqObjectQuery);
                    if (!string.IsNullOrEmpty(Alias) && DataNode != null)
                        DataNode.Alias = Alias;
                    if (DataNode == null)
                    {
                        DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                        DataNode.Alias = Alias;// (Parent as MethodCallAsCollectionSourceExpressionTreeNode).SourceCollectionIteratedObjectName;
                        if (DataNode.Name == null)
                            DataNode.Name = DataNode.Alias;
                        DataNode.Temporary = true;
                        SourceCollection.DataNode.ParentDataNode = DataNode;
                    }
                }

                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                {
                    #region Builds bridge enumerator
                    Dictionary<object, DynamicTypeProperty> dynamicTypeProperties = new Dictionary<object, DynamicTypeProperty>();

                    LoadPropertiesMetaData(dynamicTypeProperties, SelectCollection as NewExpressionTreeNode, linqObjectQuery);

                    //BridgeEnumerator = ExpressionTranslator.GetSelecionTypeEnumerator(SelectCollectionType);
                    //BridgeEnumerator = null;
                    if (_DynamicTypeDataRetrieve == null)
                    {
                        //Type[] types = new Type[4] {typeof(LINQStorageObjectQuery), typeof(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode), typeof(Dictionary<System.Reflection.PropertyInfo, DynamicTypeProperty>), typeof(ExpressionTreeNode) };
                        //BridgeEnumerator = OOAdvantech.AccessorBuilder.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(SelectCollectionType), types, linqObjectQuery, DataNode, dynamicTypeProperties, SourceCollection) as IDynamicTypeDataRetrieve;

                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, dynamicTypeProperties, SelectCollection);
                        ExpressionTranslator.AddDynamicTypeDataRetriever(SelectCollectionType, _DynamicTypeDataRetrieve);
                        // ExpressionTranslator.AddAliasEnumerator(Alias, BridgeEnumerator);

                    }
                    #endregion

                    UpdateDataNodesAlias();
                    AttatchNestedQueryToMainDataTree();

                }
                else
                {

                    #region Creates BridgeEnumerator when the select collection is alias

                    _DynamicTypeDataRetrieve = SelectCollection.DynamicTypeDataRetrieve;
                    if (_DynamicTypeDataRetrieve == null)
                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, SelectCollection.DataNode, SourceCollection);


                    //SelectExpressionTreeNode selectExpressionTreeNode = ExpressionTranslator.GetSourceCollection(SourceCollection, SelectCollection.Name) as SelectExpressionTreeNode;
                    ////QueryExpressions.ParameterExpressionTreeNode parm = (SelectCollection as QueryExpressions.ParameterExpressionTreeNode).ParameterSourceCollection as QueryExpressions.ParameterExpressionTreeNode;
                    ////var param = parm.ParameterSourceCollection;
                    //if (selectExpressionTreeNode != null && selectExpressionTreeNode.BridgeEnumerator != null)
                    //{
                    //    if (selectExpressionTreeNode.BridgeEnumerator.Type == SelectCollectionType)
                    //        BridgeEnumerator = selectExpressionTreeNode.BridgeEnumerator;
                    //    else
                    //    //if (selectExpressionTreeNode.SourceCollection is ConstantExpressionTreeNode)
                    //    {
                    //        #region the real collection is select on constant collection
                    //        #endregion

                    //        string propertyName = null;
                    //        if (SelectCollection is ParameterExpressionTreeNode)
                    //        {
                    //            ExpressionTreeNode node = SelectCollection;
                    //            while (node.Nodes.Count > 0)
                    //                node = node.Nodes[0];
                    //            propertyName = node.Name;

                    //        }

                    //        foreach (DynamicTypeProperty dynamicTypeProperty in selectExpressionTreeNode.BridgeEnumerator.Properties.Values)
                    //        {
                    //            if (SelectCollectionType == dynamicTypeProperty.PropertyInfo.PropertyType && propertyName == dynamicTypeProperty.PropertyInfo.Name)
                    //            {

                    //                BridgeEnumerator = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(SelectCollectionType), linqObjectQuery, selectExpressionTreeNode.BridgeEnumerator.RootDataNode, dynamicTypeProperty.SourceDataNode, SourceCollection) as IDynamicTypeDataRetrieve;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    dataNode = GeSourceDataNodeFromRefernce(SelectCollection as ParameterExpressionTreeNode);
                    //    if (dataNode != null)
                    //        SelectCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
                    //    else
                    //        SelectCollection.BuildDataNodeTree(SourceCollection.DataNode, linqObjectQuery);

                    //    BridgeEnumerator = ExpressionTranslator.GetDynamicTypeDataRetriever(SelectCollectionType);
                    //    if (BridgeEnumerator == null)
                    //        BridgeEnumerator = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, SelectCollection.DataNode, SourceCollection) as IDynamicTypeDataRetrieve;
                    //}
                    #endregion

                }
                Translators.QueryTranslator.ShowDataNodePathsInOutLog(SourceCollection.DataNode);
                //if (DataNode.IsSameOrParentDataNode(SourceCollection.DataNode))
                //    return SourceCollection.DataNode;
                //else
                return DataNode;

            }
            else
            {

                //DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                //SourceCollection.DataNode.HeaderDataNode.ParentDataNode = DataNode;
                //DataNode.Name = "Root";
                //DataNode.Temporary = true;

                _DataNode = SourceCollection.DataNode;
                Dictionary<object, DynamicTypeProperty> propertiesDataNodes = new Dictionary<object, DynamicTypeProperty>();



                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                    LoadPropertiesMetaData(propertiesDataNodes, SelectCollection as NewExpressionTreeNode, linqObjectQuery);
                else
                {

                    if (SelectCollection.Expression is System.Linq.Expressions.MethodCallExpression && (SelectCollection.Expression as System.Linq.Expressions.MethodCallExpression).Method.DeclaringType.FullName + "." + (SelectCollection.Expression as System.Linq.Expressions.MethodCallExpression).Method.Name == "OOAdvantech.Linq.ExtraOperators.Fetching")
                        dataNode = GeSourceDataNodeFromRefernce(SelectCollection.Nodes[0] as ParameterExpressionTreeNode);
                    else if (SelectCollection is TypeAsExpressionTreeNode)
                        dataNode = GeSourceDataNodeFromRefernce(SelectCollection.Nodes[0] as ParameterExpressionTreeNode);
                    else if (SelectCollection is ParameterExpressionTreeNode)
                        dataNode = GeSourceDataNodeFromRefernce(SelectCollection as ParameterExpressionTreeNode);
                    else
                        dataNode = GetSelectionItemRootDataNode(SelectCollection);

                    if (dataNode != null)
                        SelectCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
                    else
                    {
                        if (_DataNode == null)
                            _DataNode = SelectCollection.BuildDataNodeTree(Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, SelectCollection.Name), linqObjectQuery);
                        else
                        {

                            SelectCollection.BuildDataNodeTree(Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, SelectCollection.Name), linqObjectQuery);
                        }

                    }

                }


                #region Builds bridge enumerator
                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                {
                    //BridgeEnumerator = ExpressionTranslator.GetSelecionTypeEnumerator(SelectCollectionType);
                    //BridgeEnumerator = null;
                    if (_DynamicTypeDataRetrieve == null)
                    {
                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, propertiesDataNodes, SelectCollection);
                        ExpressionTranslator.AddDynamicTypeDataRetriever(SelectCollectionType, _DynamicTypeDataRetrieve);

                        (SelectCollection as NewExpressionTreeNode).BridgeEnumerator = _DynamicTypeDataRetrieve;
                    }
                }
                else
                {



                    _DynamicTypeDataRetrieve = SelectCollection.DynamicTypeDataRetrieve;

                    var tmp = SelectCollectionType;
                    if (_DynamicTypeDataRetrieve == null || SelectCollectionType != _DynamicTypeDataRetrieve.Type)
                        _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, SelectCollection.DataNode, SelectCollection);
                    else if (_DynamicTypeDataRetrieve != null && SelectCollectionType == _DynamicTypeDataRetrieve.Type)
                    {
                        if (_DynamicTypeDataRetrieve.MemberDataNode != null)
                        {
                            if(_DynamicTypeDataRetrieve.GroupingMetaData==null)
                                _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.MemberDataNode, SelectCollection);
                            else
                                _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.RootDataNode, SelectCollection);

                        }
                        else
                            _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, _DynamicTypeDataRetrieve.Properties, SelectCollection);


                    }


                }

                // BridgeEnumerator.ParticipateInSelectClass();
                if (Parent.Name == "Root")
                    (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;
                #endregion


                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                    AttatchNestedQueryToMainDataTree();
                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);
                Dictionary<DataNode, DataNode> replacedDataNodes = new Dictionary<DataNode, DataNode>();
                #region Merge dataNodes
                ExpressionTranslator.MergeDataNodeTree(DataNode, replacedDataNodes);
                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);
                if (ReferenceDataNode != null)
                {
                    bool sourceColectionDataNodeTemporary = SourceCollection.DataNode.Temporary;
                    if (Parent.Name != "Root")
                        SourceCollection.DataNode.Temporary = false;
                    ExpressionTranslator.RemoveTemporaryNodes(ref _DataNode, replacedDataNodes);
                    SourceCollection.DataNode.Temporary = sourceColectionDataNodeTemporary;
                }
                else
                    ExpressionTranslator.RemoveTemporaryNodes(ref _DataNode, replacedDataNodes);
                #endregion

                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);


                return DataNode;
            }

        }



        /// <MetaDataID>{ac2ddba4-2c78-4d84-bbc8-a7dec884eb8c}</MetaDataID>
        private DataNode GeSourceDataNodeFromRefernce(ParameterExpressionTreeNode parameterNode)
        {
            if (parameterNode == null)
                return null;

            MethodCallAsCollectionProviderExpressionTreeNode expresionTreeNodeWitReference = this;
            while (expresionTreeNodeWitReference.ReferenceDataNode == null &&
                expresionTreeNodeWitReference.Nodes.Count > 0 &&
                expresionTreeNodeWitReference.Nodes[0] is MethodCallAsCollectionProviderExpressionTreeNode)
                expresionTreeNodeWitReference = expresionTreeNodeWitReference.Nodes[0] as MethodCallAsCollectionProviderExpressionTreeNode;
            if (expresionTreeNodeWitReference.ReferenceDataNode == null)
                return null;
            DataNode tmpDataNode = expresionTreeNodeWitReference.ReferenceDataNode;
            if (parameterNode.Nodes.Count != 1)
                if (tmpDataNode != null && tmpDataNode.HasAlias(parameterNode.Name))
                    return tmpDataNode;
                else
                    return null;
            ExpressionTreeNode memberNode = parameterNode.Nodes[0];
            while (!(tmpDataNode != null && tmpDataNode.ParentDataNode != null && tmpDataNode.Name == memberNode.Name && memberNode.Parent.Name == tmpDataNode.ParentDataNode.Name))
            {
                if (tmpDataNode.ParentDataNode == null)
                    break;
                if (memberNode.Parent.Name == tmpDataNode.ParentDataNode.Name)
                {
                    tmpDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(tmpDataNode.ParentDataNode, memberNode.Name);
                    return tmpDataNode;
                }
                tmpDataNode = tmpDataNode.ParentDataNode;
            }
            if (tmpDataNode.ParentDataNode != null && memberNode.Parent.Name == tmpDataNode.ParentDataNode.Name)
                return Translators.QueryTranslator.GetDataNodeWithAlias(tmpDataNode.ParentDataNode, memberNode.Name);
            if (parameterNode.HeadNodeSourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
                return parameterNode.HeadNodeSourceCollection.DataNode;
            return null;

        }

        /// <MetaDataID>{84bb14ea-0f30-4c88-9e22-1965c0c58127}</MetaDataID>
        protected virtual void BuildSelectionSourceDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
        }
        /// <MetaDataID>{95d05a5e-03ee-4970-a35f-249a0f90883a}</MetaDataID>
        public override void BuildDataFilter()
        {

            SearchCondition searchCondition = null;
            if (SourceCollection is IFilteredSource)
            {
                if (Parent.Name == "Root")
                    (SourceCollection as IFilteredSource).BuildDataFilter();
                else if (Parent.Expression != null && Parent.Expression.NodeType == ExpressionType.New && !(SourceCollection is GroupByExpressionTreeNode))
                    (SourceCollection as IFilteredSource).BuildDataFilter();
                else
                    (SourceCollection as IFilteredSource).BuildDataFilter();
            }
            if (SelectCollection is AggregateFunctionExpressionTreeNode)
                (SelectCollection as AggregateFunctionExpressionTreeNode).BuildDataFilter();
            else
            {
                foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
                {
                    if (treeNode is IFilteredSource)
                        (treeNode as IFilteredSource).BuildDataFilter();
                }
            }




        }



        /// <MetaDataID>{aa456dd1-8a3d-4ae7-9776-d29c0c2dcaf0}</MetaDataID>
        DataNode GetSelectionItemRootDataNode(ExpressionTreeNode selectionItemTreeNode)
        {
            DataNode derivedCollectionDataNode = null;
            if (selectionItemTreeNode is ParameterExpressionTreeNode)
                derivedCollectionDataNode = GeSourceDataNodeFromRefernce(selectionItemTreeNode as ParameterExpressionTreeNode);
            if (derivedCollectionDataNode != null)
                return derivedCollectionDataNode;

            string selectionItemName = selectionItemTreeNode.Name;
            derivedCollectionDataNode = SelectionCollectionSource.DataNode;
            //
            while (derivedCollectionDataNode.ParentDataNode != null && derivedCollectionDataNode != SourceCollection.DataNode &&
                (!derivedCollectionDataNode.ParentDataNode.HasAlias(selectionItemName) && !(derivedCollectionDataNode.ParentDataNode.Temporary && derivedCollectionDataNode.ParentDataNode.Name == selectionItemName)))
            {
                derivedCollectionDataNode = derivedCollectionDataNode.ParentDataNode;
            }
            if (derivedCollectionDataNode.ParentDataNode != null && (derivedCollectionDataNode.ParentDataNode.HasAlias(selectionItemName) || (derivedCollectionDataNode.ParentDataNode.Temporary && derivedCollectionDataNode.ParentDataNode.Name == selectionItemName)))
                return derivedCollectionDataNode.ParentDataNode;
            else
            {
                DataNode referenceDataNode = ReferenceDataNode;
                while (referenceDataNode != null)
                {
                    if (referenceDataNode.HasAlias(selectionItemName))
                        return referenceDataNode;
                    referenceDataNode = referenceDataNode.ParentDataNode;
                }

                return SelectionCollectionSource.DataNode;

            }
        }
        /// <MetaDataID>{6d9395ce-4fd2-4fc6-a2a2-5ddfd86e20e9}</MetaDataID>
        private void LoadPropertiesMetaData(Dictionary<object, DynamicTypeProperty> dynamicTypeProperties, NewExpressionTreeNode newExpression, ILINQObjectQuery linqObjectQuery)
        {
            int i = 0;
            DataNode dataNode = null;
            foreach (ExpressionTreeNode propertyTreeNode in newExpression.Nodes)
            {
                dataNode = GetSelectionItemRootDataNode(propertyTreeNode);
                dataNode = propertyTreeNode.BuildDataNodeTree(dataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                //dataNode.Alias = treeNode.Alias;
                if (propertyTreeNode is SelectExpressionTreeNode)
                {
                    if ((propertyTreeNode as SelectExpressionTreeNode).SelectCollection.Expression.NodeType == ExpressionType.New)
                        dataNode.AddBranchAlias(propertyTreeNode.Alias);
                    else
                        (propertyTreeNode as SelectExpressionTreeNode).SelectCollection.DataNode.Alias = propertyTreeNode.Alias;
                }
                System.Reflection.PropertyInfo property = newExpression.Expression.Type.GetMetaData().GetProperty(propertyTreeNode.Alias);
                if (propertyTreeNode is MethodCallAsCollectionProviderExpressionTreeNode)
                {
                    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (propertyTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve, propertyTreeNode);
                    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    //ExpressionTranslator.AddAliasEnumerator(treeNode.Alias, dynamicTypeProperty.PropertyType);
                }
                else if (propertyTreeNode is QueryExpressions.ConstantExpressionTreeNode)
                {
                    int parameterIndex = GetCtorParameterIndexFor(newExpression, propertyTreeNode);
                    if (parameterIndex != -1)
                    {
                        var parameterInfo = (newExpression.Expression as System.Linq.Expressions.NewExpression).Constructor.GetParameters()[parameterIndex];
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(parameterInfo, (propertyTreeNode as QueryExpressions.ConstantExpressionTreeNode).DataNode, propertyTreeNode);
                        dynamicTypeProperties.Add(parameterInfo, dynamicTypeProperty);
                    }
                    else
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, (propertyTreeNode as QueryExpressions.ConstantExpressionTreeNode).DataNode, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }
                }
                else if (propertyTreeNode is QueryExpressions.NewExpressionTreeNode)
                {

                    Dictionary<object, DynamicTypeProperty> propertyTypeDynamicTypeProperties = new Dictionary<object, DynamicTypeProperty>();

                    LoadPropertiesMetaData(propertyTypeDynamicTypeProperties, propertyTreeNode as QueryExpressions.NewExpressionTreeNode, linqObjectQuery);
                    //MethodCallAsCollectionSourceExpressionTreeNode propertySource  = SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode;
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode commonParentDataNode = null;
                    foreach (var dynamicProperty in propertyTypeDynamicTypeProperties.Values)
                    {
                        if (dynamicProperty.IsLocalScopeValue)
                            continue;
                        if (commonParentDataNode == null)
                            commonParentDataNode = dynamicProperty.SourceDataNode;
                        else
                            if (commonParentDataNode.IsParentDataNode(dynamicProperty.SourceDataNode))
                            commonParentDataNode = dynamicProperty.SourceDataNode;

                    }
                    if (commonParentDataNode.ParentDataNode != null)
                        commonParentDataNode = commonParentDataNode.ParentDataNode;

                    IDynamicTypeDataRetrieve bridgeEnumerator = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve((propertyTreeNode as QueryExpressions.NewExpressionTreeNode).Expression.Type, linqObjectQuery, commonParentDataNode, propertyTypeDynamicTypeProperties, propertyTreeNode);
                    ExpressionTranslator.AddDynamicTypeDataRetriever((propertyTreeNode as QueryExpressions.NewExpressionTreeNode).Expression.Type, bridgeEnumerator);
                    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, commonParentDataNode, bridgeEnumerator, propertyTreeNode);
                    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    (propertyTreeNode as QueryExpressions.NewExpressionTreeNode).BridgeEnumerator = bridgeEnumerator;

                }
                else if (/*treeNode.Alias == "Key" &&*/ dataNode.Type == DataNode.DataNodeType.Key && dataNode.ParentDataNode.Type == DataNode.DataNodeType.Group)
                {
                    IDynamicTypeDataRetrieve propertyType = null;
                    if (propertyTreeNode is ParameterExpressionTreeNode)
                    {
                        propertyType = propertyTreeNode.DynamicTypeDataRetrieve;
                        //var source = (treeNode as ParameterExpressionTreeNode).SourceCollection;
                        //if (source is MethodCallAsCollectionSourceExpressionTreeNode)
                        //    propertyType = (source as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                        //if (source is GroupKeyExpressionTreeNode)
                        //    propertyType = (source as GroupKeyExpressionTreeNode).BridgeEnumerator;
                    }
                    if (propertyType == null)
                        propertyType = ExpressionTranslator.GetDynamicTypeDataRetriever(property.PropertyType);
                    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, propertyType, propertyTreeNode);
                    dynamicTypeProperties.Add(property, dynamicTypeProperty);

                }
                else if (propertyTreeNode is QueryExpressions.ParameterExpressionTreeNode)
                {


                    IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = propertyTreeNode.DynamicTypeDataRetrieve;
                    if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.IsGrouping && property.PropertyType != dynamicTypeDataRetrieve.Type)
                    {
                        dataNode = dynamicTypeDataRetrieve.GroupingMetaData.GroupDataNode.SubDataNodes.OfType<DerivedDataNode>().Where(deriveDataNode => deriveDataNode.OrgDataNode == dynamicTypeDataRetrieve.GroupingMetaData.GroupDataNode.GroupedDataNode).First();
                        dynamicTypeDataRetrieve = dynamicTypeDataRetrieve.GroupingMetaData.GroupCollectionDynamicTypeDataRetrieve;
                    }

                    int parameterIndex = GetCtorParameterIndexFor(newExpression, propertyTreeNode);
                    if (parameterIndex != -1)
                    {
                        var parameterInfo = (newExpression.Expression as System.Linq.Expressions.NewExpression).Constructor.GetParameters()[parameterIndex];

                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(parameterInfo, dataNode, dynamicTypeDataRetrieve, propertyTreeNode);
                        dynamicTypeProperties.Add(parameterInfo, dynamicTypeProperty);

                    }
                    else
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, dynamicTypeDataRetrieve, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);


                    }
                }
                //else if (property.PropertyType.Name.IndexOf("<>f") == 0 && treeNode is QueryExpressions.ParameterExpressionTreeNode)
                //{
                //    MethodCallAsCollectionSourceExpressionTreeNode propertySource = null;
                //    if ((treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection is NewExpressionTreeNode)
                //    {
                //        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, ((treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as NewExpressionTreeNode).BridgeEnumerator, treeNode);
                //        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                //    }
                //    else
                //    {
                //        propertySource = (treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode;
                //        //ExpressionTreeNode propertySource = ExpressionTranslator.GetSourceCollection(SourceCollection, treeNode.Alias);
                //        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (propertySource as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator, treeNode);
                //        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                //    }
                //}
                //else if (treeNode is QueryExpressions.ParameterExpressionTreeNode &&
                //    (treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection is NewExpressionTreeNode)
                //{
                //    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, ((treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as NewExpressionTreeNode).BridgeEnumerator, treeNode);
                //    dynamicTypeProperties.Add(property, dynamicTypeProperty);

                //}
                //else if (treeNode is ParameterExpressionTreeNode &&
                //    (treeNode as ParameterExpressionTreeNode).SourceCollection is SelectExpressionTreeNode &&
                //    ((treeNode as ParameterExpressionTreeNode).SourceCollection as SelectExpressionTreeNode).SelectCollection is NewExpressionTreeNode)
                //{
                //    MethodCallAsCollectionSourceExpressionTreeNode propertySource = (treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode;
                //    //ExpressionTreeNode propertySource = ExpressionTranslator.GetSourceCollection(SourceCollection, treeNode.Alias);
                //    DynamicTypeProperty dynamicTypeProperty = null;
                //    //if (propertySource.ReplacedExpressionTreeNode != null && propertySource.ReplacedExpressionTreeNode is QueryExpressions.ConstantExpressionTreeNode)
                //    //    dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, null, treeNode);
                //    //else
                //    dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (propertySource as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator, treeNode);

                //    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                //}
                ////else if (treeNode is ParameterExpressionTreeNode &&
                ////    (treeNode as ParameterExpressionTreeNode).SourceCollection.DerivedMemberLinqQuery != null)
                ////{
                ////    dataNode = (treeNode as ParameterExpressionTreeNode).SourceCollection.DerivedMemberLinqQuery.QueryResult.RootDataNode;
                ////    MethodCallAsCollectionSourceExpressionTreeNode propertySource = (treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode;
                ////    //ExpressionTreeNode propertySource = ExpressionTranslator.GetSourceCollection(SourceCollection, treeNode.Alias);
                ////    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (treeNode as ParameterExpressionTreeNode).SourceCollection.DerivedMemberLinqQuery.QueryResult, treeNode);
                ////    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                ////}

                //else if (TypeHelper.FindIEnumerable(property.PropertyType) != null /*&& property.PropertyType.Name!=typeof(System.Linq.IGrouping<,>).Name */&& treeNode is ParameterExpressionTreeNode)
                //{
                //    ExpressionTreeNode expressionTreeNode = treeNode;
                //    ExpressionTreeNode propertySource = SourceCollection;
                //    if (treeNode is ParameterExpressionTreeNode)
                //    {
                //        propertySource = (treeNode as ParameterExpressionTreeNode).SourceCollection;
                //    }
                //    else
                //    {
                //        while (expressionTreeNode != null)
                //        {

                //            propertySource = ExpressionTranslator.GetSourceCollection(propertySource, expressionTreeNode.Name);
                //            if (expressionTreeNode.Nodes.Count > 0)
                //                expressionTreeNode = expressionTreeNode.Nodes[0];
                //            else
                //                expressionTreeNode = null;
                //        }
                //    }
                //    IDynamicTypeDataRetrieve propertyType = null;
                //    if (propertySource != null && propertySource is MethodCallAsCollectionSourceExpressionTreeNode)
                //        propertyType = (propertySource as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                //    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, propertyType, treeNode);
                //    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                //    //ExpressionTranslator.AddAliasEnumerator(treeNode.Alias, dynamicTypeProperty.PropertyType);


                //}
                else
                {
                    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, null, propertyTreeNode);
                    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                }



                i++;
            }
        }

        /// <MetaDataID>{0fb3d881-56df-4f44-94b5-dab1307b8c10}</MetaDataID>
        private int GetCtorParameterIndexFor(NewExpressionTreeNode newExpression, ExpressionTreeNode propertyTreeNode)
        {


            if (newExpression.Type.Name.IndexOf("<>f__AnonymousType") == 0)
                return -1;

            int i = 0;
            foreach (var param in (newExpression.Expression as System.Linq.Expressions.NewExpression).Constructor.GetParameters())
            {
                if (propertyTreeNode.Alias == param.Name)
                    return i;
                i++;
            }


            return -1;
        }


        /// <MetaDataID>{2eeeb96a-0732-4095-ab54-da1408e2110f}</MetaDataID>
        private void UpdateDataNodesAlias()
        {
            foreach (ExpressionTreeNode memberNode in SelectCollection.Nodes)
            {
                DataNode dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, memberNode.Alias);
                if (dataNode != null)
                    continue;
                else
                {
                    ExpressionTreeNode expressionNode = memberNode;
                    if (SourceCollection.DataNode.ParentDataNode != null)
                        dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode.ParentDataNode, expressionNode.Name);
                    else
                        dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, expressionNode.Name);

                    while (dataNode != null && expressionNode.Nodes.Count > 0)
                    {
                        expressionNode = expressionNode.Nodes[0];
                        dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode.ParentDataNode, expressionNode.Name);
                    }
                    if (dataNode != null)
                        dataNode.Alias = memberNode.Alias;
                }
            }

        }

        /// <MetaDataID>{b67ff48e-559d-4340-bcb6-5c67568ed737}</MetaDataID>
        internal protected virtual ExpressionTreeNode SelectCollection
        {
            get
            {
                return Nodes[1];
            }
        }
        /// <MetaDataID>{eb11e34f-a829-4fd2-b983-e37b910b07af}</MetaDataID>
        Type SelectCollectionType
        {
            get
            {
                if (SelectCollection.Expression is NewExpression)
                    return (SelectCollection.Expression as NewExpression).Type;
                else
                {
                    ExpressionTreeNode expressionTreeNode = SelectCollection;
                    if (expressionTreeNode is ParameterExpressionTreeNode)
                    {
                        while (expressionTreeNode.Nodes.Count > 0)
                        {
                            if (expressionTreeNode is TypeAsExpressionTreeNode)
                            {
                                if (expressionTreeNode.Nodes.Count == 1)
                                    return expressionTreeNode.Expression.Type;
                                else
                                    expressionTreeNode = expressionTreeNode.Nodes[1];

                            }
                            else
                                expressionTreeNode = expressionTreeNode.Nodes[0];
                        }
                    }
                    return expressionTreeNode.Expression.Type;
                }
            }
        }
        /// <MetaDataID>{92e08a84-43e5-4bee-ba95-2cfc246be88c}</MetaDataID>
        protected virtual ExpressionTreeNode SelectionCollectionSource
        {
            get
            {
                return SourceCollection;
            }
        }
        /// <MetaDataID>{4fc7ce70-25a2-4781-a68b-5e6bef008ed4}</MetaDataID>
        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                if (string.IsNullOrEmpty(Nodes[0].NamePrefix))
                    Nodes[0].NamePrefix = "SourceCollection";
                return Nodes[0];
            }
        }
    }



    /// <MetaDataID>{294f9f4b-5f83-4601-a073-4762b071c229}</MetaDataID>
    class SelectManyExpressionTreeNode : SelectExpressionTreeNode
    {
        public override void BuildDataFilter()
        {
            base.BuildDataFilter();
            if (DerivedCollection is IFilteredSource)
                (DerivedCollection as IFilteredSource).BuildDataFilter();

        }

        /// <MetaDataID>{dc816578-8129-4eb7-8250-90025a33e02f}</MetaDataID>
        public override SearchCondition FilterDataCondition
        {
            get
            {
                if (DerivedCollection is IFilteredSource || DerivedCollection is ParameterExpressionTreeNode)
                    return SearchCondition.JoinSearchConditions(DerivedCollection.FilterDataCondition, base.FilterDataCondition);
                else
                    return base.FilterDataCondition;
            }
        }
        /// <MetaDataID>{f1179af7-26bd-4487-a8a7-301b32620896}</MetaDataID>
        public SelectManyExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            SelectorSourceParameter = Translators.ExpressionVisitor.GetLambdaExpression(MethodCallExpression.Arguments[2]).Parameters[0];
            expressionTranslator.ParameterDeclareExpression[SelectorSourceParameter] = this;
            SelectorCollectionParameter = Translators.ExpressionVisitor.GetLambdaExpression(MethodCallExpression.Arguments[2]).Parameters[1];
            expressionTranslator.ParameterDeclareExpression.Add(SelectorCollectionParameter, this);
        }

        public readonly ParameterExpression SelectorSourceParameter;
        public readonly ParameterExpression SelectorCollectionParameter;
        /// <MetaDataID>{4880670b-0b6c-48bb-b1d8-85b66b846a0e}</MetaDataID>
        protected override void BuildSelectionSourceDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            //var trt =ExtendExpressionVisitor.GetParameter( ExtendExpressionVisitor.GetLambdaExpression((Expression as MethodCallExpression).Arguments[1]).Body);
            //var trty = ExtendExpressionVisitor.GetLambdaExpression((Expression as MethodCallExpression).Arguments[1]).Parameters[0];
            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            DerivedCollection.BuildDataNodeTree(SourceCollection.DataNode, linqObjectQuery);
        }


        /// <MetaDataID>{c17285f4-ed4a-46fd-84af-2250b6101171}</MetaDataID>
        internal ExpressionTreeNode DerivedCollection
        {
            get
            {
                return Nodes[1];
            }
        }
        /// <MetaDataID>{b13873b7-be93-42f7-8270-157729e50368}</MetaDataID>
        internal protected override ExpressionTreeNode SelectCollection
        {
            get
            {
                return Nodes[2];
            }
        }
        /// <MetaDataID>{51ece3f1-938a-4f1f-b196-50094aca444d}</MetaDataID>
        protected override ExpressionTreeNode SelectionCollectionSource
        {
            get
            {
                return DerivedCollection;
            }
        }




    }
}
