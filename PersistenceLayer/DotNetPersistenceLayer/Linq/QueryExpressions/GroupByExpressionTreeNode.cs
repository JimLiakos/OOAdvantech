using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{  
    /// <MetaDataID>{1afa3c97-46da-4747-8f1f-eb8f39913968}</MetaDataID>
    class GroupByExpressionTreeNode : MethodCallAsCollectionProviderExpressionTreeNode
    {
        internal readonly ParameterExpression KeySelectorParameter;
        internal readonly ParameterExpression ResultSelectorParameter;
        /// <MetaDataID>{13e0e597-9f69-49c6-8a69-f1bb9ccbe510}</MetaDataID>
        public GroupByExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            Expression keyExpresion = (exp as MethodCallExpression).Arguments[1];
            if (keyExpresion is UnaryExpression)
                keyExpresion = (keyExpresion as UnaryExpression).Operand;

            KeySelectorParameter = (keyExpresion as LambdaExpression).Parameters[0];
            ExpressionTranslator.ParameterDeclareExpression[KeySelectorParameter]= this;


            if ((exp as MethodCallExpression).Arguments.Count > 2)
            {
                Expression resultSelectorExpression = (exp as MethodCallExpression).Arguments[2];
                if (resultSelectorExpression is UnaryExpression)
                    resultSelectorExpression = (resultSelectorExpression as UnaryExpression).Operand;

                ResultSelectorParameter = (resultSelectorExpression as LambdaExpression).Parameters[0];
                ExpressionTranslator.ParameterDeclareExpression[ResultSelectorParameter] = this;
            }

        }
        /// <MetaDataID>{bc6e190b-3bae-4ab4-bd1c-8870ba363984}</MetaDataID>
        private void AttatchNestedQueryToMainDataTree()
        {
            foreach (ExpressionTreeNode treeNode in GroupKeyExpression.Nodes)
            {
                if (treeNode is MethodCallAsCollectionProviderExpressionTreeNode)
                {
                    ExpressionTreeNode expressionTreeNode = treeNode;
                    while (expressionTreeNode.Nodes.Count > 0)
                    {
                        if (expressionTreeNode is MethodCallAsCollectionProviderExpressionTreeNode && (expressionTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).ReferenceDataNode != null)
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

        /// <MetaDataID>{1d145ec1-5bbd-4289-9c24-4e480c4bb0e4}</MetaDataID>
        GroupDataNode GroupDataNode
        {
            get
            {
                return _DataNode as GroupDataNode;
            }
        }
        /// <MetaDataID>{ab840cb1-2251-47c7-a2a5-a30508b8c2c9}</MetaDataID>
        public IDynamicTypeDataRetrieve KeyDynamicTypeDataRetrieve;
        /// <MetaDataID>{ace49418-b084-4be1-a8de-ff081a441b82}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            DataNode groupDataNode = BuildGroupingSourceDataNodeTree(dataNode, linqObjectQuery);


            if (SourceCollection.DataNode.ParentDataNode != null)
                ReferenceDataNode = SourceCollection.DataNode.RealParentDataNode;

            Dictionary<DataNode, DataNode> replacedDataNodes = new Dictionary<DataNode, DataNode>();
            DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.GroupDataNode(linqObjectQuery.ObjectQuery);
            DataNode.Alias = Alias;// (Parent as MethodCallAsCollectionSourceExpressionTreeNode).SourceCollectionIteratedObjectName;
            if (DataNode.Name == null)
                DataNode.Name = DataNode.Alias;
            DataNode.Temporary = true;

            //if (SourceCollection.DataNode.Type == DataNode.DataNodeType.Key)
            //    SourceCollection.DataNode.ParentDataNode.ParentDataNode = DataNode;
            //else
            //{
            //    DataNode.ParentDataNode = SourceCollection.DataNode.ParentDataNode;
            //    SourceCollection.DataNode.ParentDataNode = DataNode;
            //}

            _DataNode.Type = DataNode.DataNodeType.Group;
            GroupDataNode.GroupedDataNode = groupDataNode;
            if (GroupDataNode.GroupedDataNode != null)
                GroupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;

            if (dataNode != null)
                _DataNode.ParentDataNode = dataNode;
            else
            {
                if (groupDataNode.ParentDataNode == null)
                    groupDataNode.ParentDataNode = _DataNode; // Group data node is header data node
                else
                {
                    if (groupDataNode.HeaderDataNode != _DataNode.HeaderDataNode)
                        groupDataNode.HeaderDataNode.ParentDataNode = _DataNode;
                }
            }



            if (GroupKeyExpression.Expression.NodeType == ExpressionType.New)
            {
                #region Builds bridge enumerator
                Dictionary<object, DynamicTypeProperty> dynamicTypeProperties = new Dictionary<object, DynamicTypeProperty>();
                LoadPropertiesMetaData(dynamicTypeProperties);
                foreach (DynamicTypeProperty dynamicTypeProperty in dynamicTypeProperties.Values)
                {
                    GroupDataNode.AddGroupKeyDataNode(dynamicTypeProperty.SourceDataNode);
                    dynamicTypeProperty.SourceDataNode.ParticipateInAggregateFunction = true;
                }

                IDynamicTypeDataRetrieve bridgeEnumerator = ExpressionTranslator.GetDynamicTypeDataRetriever(TypeHelper.GetElementType(Expression.Type));
                bridgeEnumerator = null;
                if (bridgeEnumerator == null)
                {
                    //bridgeEnumerator = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, dynamicTypeProperties) as IDynamicTypeDataRetrieve;
                    bridgeEnumerator = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(TypeHelper.GetElementType(Expression.Type), linqObjectQuery, DataNode, groupDataNode,this) ;
                    ExpressionTranslator.AddDynamicTypeDataRetriever(TypeHelper.GetElementType(Expression.Type), bridgeEnumerator);
                    //ExpressionTranslator.AddAliasEnumerator(Alias, bridgeEnumerator);
                    KeyDynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(SelectCollectionType, linqObjectQuery, DataNode, dynamicTypeProperties, GroupKeyExpression);
                    
                    ExpressionTranslator.AddDynamicTypeDataRetriever(SelectCollectionType, KeyDynamicTypeDataRetrieve);
                    (GroupKeyExpression as QueryExpressions.GroupKeyExpressionTreeNode).BridgeEnumerator = KeyDynamicTypeDataRetrieve;
                    bridgeEnumerator.GroupingMetaData.KeyDynamicTypeDataRetrieve = KeyDynamicTypeDataRetrieve;
                    bridgeEnumerator.GroupingMetaData.GroupCollectionDynamicTypeDataRetrieve = GroupingCollection.DynamicTypeDataRetrieve;
                }
                _DynamicTypeDataRetrieve = bridgeEnumerator;
                #endregion

                UpdateDataNodesAlias();
                AttatchNestedQueryToMainDataTree();

            }
            else
            {
                DataNode keyPartDataNode = this.GroupKeyExpression.BuildDataNodeTree(groupDataNode, linqObjectQuery);
                GroupDataNode.AddGroupKeyDataNode(keyPartDataNode);

                IDynamicTypeDataRetrieve bridgeEnumerator = ExpressionTranslator.GetDynamicTypeDataRetriever(TypeHelper.GetElementType(Expression.Type));
                bridgeEnumerator = null;
                if (bridgeEnumerator == null)
                {
                    //bridgeEnumerator = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, dynamicTypeProperties) as IDynamicTypeDataRetrieve;
                    bridgeEnumerator = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(TypeHelper.GetElementType(Expression.Type), linqObjectQuery, DataNode, groupDataNode, this);
                    ExpressionTranslator.AddDynamicTypeDataRetriever(TypeHelper.GetElementType(Expression.Type), bridgeEnumerator);

                    //ExpressionTranslator.AddSelecionTypeEnumerator(SelectCollectionType, KeyDynamicTypeDataRetrieve);
                    //(SelectCollection as QueryExpressions.GroupKeyExpressionTreeNode).BridgeEnumerator = KeyDynamicTypeDataRetrieve;
                }
                _DynamicTypeDataRetrieve = bridgeEnumerator;
            }

            #region Merge dataNodes
            ExpressionTranslator.MergeDataNodeTree(DataNode, replacedDataNodes);
            if (ReferenceDataNode != null)
            {
                bool sourceColectionDataNodeTemporary = SourceCollection.DataNode.Temporary;
                if (Parent.Name != "Root")
                    SourceCollection.DataNode.Temporary = false;
                if (_DataNode.SubDataNodes.Count == 1)
                {
                    dataNode = _DataNode.SubDataNodes[0];
                    ExpressionTranslator.RemoveTemporaryNodes(ref dataNode, replacedDataNodes);
                    SourceCollection.DataNode.Temporary = sourceColectionDataNodeTemporary;
                    dataNode.ParentDataNode = _DataNode;
                }
            }
            else
            {
                foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(_DataNode.SubDataNodes))
                {
                    dataNode = subDataNode;
                    ExpressionTranslator.RemoveTemporaryNodes(ref dataNode, replacedDataNodes);
                    dataNode.ParentDataNode = _DataNode;
                }
            }
            #endregion

            Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);
            DataNode.Temporary = false;

            DataNode keyDataNode = new DataNode(linqObjectQuery.ObjectQuery);
            keyDataNode.Name = "Key";
            keyDataNode.Alias = "Key";
            keyDataNode.ParentDataNode = _DataNode;
            keyDataNode.Type = DataNode.DataNodeType.Key;
            if (KeyDynamicTypeDataRetrieve != null)
            {
                foreach (var dynamicProperty in KeyDynamicTypeDataRetrieve.Properties.Values)
                {
                    DerivedDataNode derivedDataNode = new DerivedDataNode(dynamicProperty.SourceDataNode);
                    derivedDataNode.ParentDataNode = keyDataNode;
                    dynamicProperty.SourceDataNode = derivedDataNode;
                }
            }
            else
            {
                foreach (var groupKeyDataNode in (_DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    DerivedDataNode derivedDataNode = new DerivedDataNode(groupKeyDataNode);
                    derivedDataNode.ParentDataNode = keyDataNode;
                }
                if ((_DataNode as GroupDataNode).GroupKeyDataNodes.Count == 1)
                    keyDataNode.Classifier = (_DataNode as GroupDataNode).GroupKeyDataNodes[0].Classifier;
            }
            
            if (!groupDataNode.IsParentDataNode(_DataNode) && Translators.QueryTranslator.GetDataNodeWithAlias(DataNode, groupDataNode.Name) != null)
            {
                GroupDataNode.GroupedDataNode = Translators.QueryTranslator.GetDataNodeWithAlias(DataNode, groupDataNode.Name);
                if (GroupDataNode.GroupedDataNode != null)
                    GroupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;
            }
            new DerivedDataNode(GroupDataNode.GroupedDataNode).ParentDataNode = GroupDataNode;
            return DataNode;


        }

        /// <MetaDataID>{ac448005-093c-4901-9d9f-af204002439d}</MetaDataID>
        private void UpdatePropertiesDataNode(Dictionary<System.Reflection.PropertyInfo, DataNode> propertiesDataNodes)
        {
            foreach (System.Reflection.PropertyInfo propertyInfo in new List<System.Reflection.PropertyInfo>(propertiesDataNodes.Keys))
            {


            }
        }

        



        public override SearchCondition FilterDataCondition
        {
            get
            {
                return _FilterDataCondition;


                //return AncestorsFilterDataCondition;
            }
        }


        /// <MetaDataID>{f21b3383-d157-451b-aec1-f6f3d53c3aa2}</MetaDataID>
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

                    //foreach (DataNode subDataNode in tmpDataNode.ParentDataNode.SubDataNodes)
                    //{
                    //    if (subDataNode.Name == memberNode.Name)
                    //    {
                    //        tmpDataNode = subDataNode;
                    //        break;
                    //    }

                    //}
                    //if (tmpDataNode.Name == memberNode.Name)
                    //    break;
                }

                tmpDataNode = tmpDataNode.ParentDataNode;

            }
            if (tmpDataNode.ParentDataNode != null && memberNode.Parent.Name == tmpDataNode.ParentDataNode.Name)
                return Translators.QueryTranslator.GetDataNodeWithAlias(tmpDataNode.ParentDataNode, memberNode.Name);

            //if (tmpDataNode != null && tmpDataNode.ParentDataNode != null && tmpDataNode.Name == memberNode.Name && memberNode.Parent.Name == tmpDataNode.ParentDataNode.Name)
            //    return tmpDataNode;
            return null;

        }

        /// <MetaDataID>{75e23a26-c1ce-4632-9e0c-48378b81301c}</MetaDataID>
        protected virtual DataNode BuildGroupingSourceDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            if (GroupingCollection != SourceCollection)
                return GroupingCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            else
                return dataNode;
        }
        /// <MetaDataID>{59260e78-4649-47d8-90e4-e9d8fa009986}</MetaDataID>
        public override void BuildDataFilter()
        {

            foreach (ExpressionTreeNode treeNode in GroupKeyExpression.Nodes)
            {
                if (treeNode is IFilteredSource)
                    (treeNode as IFilteredSource).BuildDataFilter();
            }
            if (SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
            {
                (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();

                //if (Parent.Name == "Root")
                //{
                //    // DataNode.SearchCondition =
                //    //DataNode.AddSearchCondition( (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(null));

                //}
                //else if (Parent.Expression != null && Parent.Expression.NodeType == ExpressionType.New)
                //{
                //    (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(null);
                //}
                //else

                //    (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(null);
            }
            
            
        }
        /// <MetaDataID>{628e27ac-0629-45a2-9e71-c2bc15fc0e67}</MetaDataID>
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
                return SelectionCollectionSource.DataNode;
        }
        /// <MetaDataID>{ba9079d0-b13d-4e75-9fef-4673e0c54561}</MetaDataID>
        private void LoadPropertiesMetaData(Dictionary<object, DynamicTypeProperty> dynamicTypeProperties)
        {
            int i = 0;
            DataNode dataNode = null;
            foreach (ExpressionTreeNode treeNode in GroupKeyExpression.Nodes)
            {
                dataNode = GetSelectionItemRootDataNode(treeNode);
                dataNode = treeNode.BuildDataNodeTree(dataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                //dataNode.Alias = treeNode.Alias;
                if (treeNode is SelectExpressionTreeNode)
                {
                    if ((treeNode as SelectExpressionTreeNode).SelectCollection.Expression.NodeType == ExpressionType.New)
                    {
                        dataNode.AddBranchAlias(treeNode.Alias);
                    }
                    else
                        (treeNode as SelectExpressionTreeNode).SelectCollection.DataNode.Alias = treeNode.Alias;
                }



                if (GroupKeyExpression.Expression is System.Linq.Expressions.NewExpression)
                {
                    System.Reflection.PropertyInfo property = SelectCollectionType.GetMetaData().GetProperties()[i];
                    if (treeNode is MethodCallAsCollectionProviderExpressionTreeNode)
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (treeNode as MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve, treeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                        // ExpressionTranslator.AddAliasEnumerator(treeNode.Alias, dynamicTypeProperty.PropertyType);
                    }
                    else if (property.PropertyType.Name.IndexOf("<>f") == 0 && treeNode is ParameterExpressionTreeNode)
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (treeNode as ParameterExpressionTreeNode).DynamicTypeDataRetrieve, treeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }
                    else
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, null, treeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }



                }
                i++;
            }
        }
        /// <MetaDataID>{3070477f-1594-4462-a69d-5f16e8df524d}</MetaDataID>
        private void UpdateDataNodesAlias()
        {
            foreach (ExpressionTreeNode memberNode in GroupKeyExpression.Nodes)
            {
                DataNode dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode, memberNode.Alias);
                if (dataNode != null)
                    continue;
                else
                {
                    ExpressionTreeNode expressionNode = memberNode;
                    dataNode = Translators.QueryTranslator.GetDataNodeWithAlias(SourceCollection.DataNode.ParentDataNode, expressionNode.Name);
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

        /// <MetaDataID>{3372aa27-a834-42c9-9480-19a7cd48e210}</MetaDataID>
        internal protected virtual ExpressionTreeNode GroupKeyExpression
        {
            get
            {
                return Nodes[1];
            }
        }
        /// <MetaDataID>{e02d30aa-7ad3-4538-80f8-73a179042000}</MetaDataID>
        Type SelectCollectionType
        {
            get
            {
                if (GroupKeyExpression.Expression is NewExpression)
                    return (GroupKeyExpression.Expression as NewExpression).Type;
                else
                {
                    ExpressionTreeNode expressionTreeNode = GroupKeyExpression;
                    while (expressionTreeNode.Nodes.Count > 0)
                        expressionTreeNode = expressionTreeNode.Nodes[0];
                    return expressionTreeNode.Expression.Type;
                }
            }
        }

        /// <MetaDataID>{3ebd4464-9f49-49c1-a48e-7feac1f5120b}</MetaDataID>
        protected virtual ExpressionTreeNode SelectionCollectionSource
        {
            get
            {
                return SourceCollection;
            }
        }
        /// <MetaDataID>{8e6f8536-259e-4fcd-83db-4629735fb098}</MetaDataID>
        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                if (string.IsNullOrEmpty(Nodes[0].NamePrefix))
                    Nodes[0].NamePrefix = "SourceCollection";
                return Nodes[0];
            }
        }
        /// <MetaDataID>{b01bf100-5cb4-4c0e-aa41-85cea98696b0}</MetaDataID>
        internal ExpressionTreeNode GroupingCollection
        {
            get
            {
                if (Nodes.Count != 3)
                    return SourceCollection;
                else
                    return Nodes[2];
            }
        }

    }
}
