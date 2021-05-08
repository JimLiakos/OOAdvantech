using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    class SelectManyExpressionTreeNodeb : MethodCallAsCollectionSourceExpressionTreeNode
    {
        public SelectManyExpressionTreeNodeb(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {

        }

        private void AttatchNestQueryToMainDataTree()
        {
            foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
            {
                if (treeNode is MethodCallAsCollectionSourceExpressionTreeNode)
                {
                    ExpressionTreeNode expressionTreeNode = treeNode;
                    while (expressionTreeNode.Nodes.Count > 0)
                    {
                        if (expressionTreeNode is MethodCallAsCollectionSourceExpressionTreeNode && (expressionTreeNode as MethodCallAsCollectionSourceExpressionTreeNode).ReferenceDataNode != null)
                        {
                            treeNode.DataNode.ParentDataNode = (expressionTreeNode as MethodCallAsCollectionSourceExpressionTreeNode).ReferenceDataNode;
                            break;
                        }
                        else
                            expressionTreeNode = expressionTreeNode.Nodes[0];
                    }
                }
            }
        }
        public override DataNode BuildDataNodeTree(DataNode dataNode, ObjectQuery linqObjectQuery)
        {
            dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
            DerivedCollection.BuildDataNodeTree(dataNode, linqObjectQuery);

            if (SourceCollection.DataNode.ParentDataNode != null)
                ReferenceDataNode = SourceCollection.DataNode.ParentDataNode;

            if (Parent is MethodCallAsCollectionSourceExpressionTreeNode)
            {
                DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                DataNode.Alias = Alias;// (Parent as MethodCallAsCollectionSourceExpressionTreeNode).SourceCollectionIteratedObjectName;
                if (DataNode.Name == null)
                    DataNode.Name = DataNode.Alias;
                DataNode.Temporary = true;
                SourceCollection.DataNode.ParentDataNode = DataNode;

                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                {
                    #region Builds bridge enumerator
                    Dictionary<System.Reflection.PropertyInfo, DataNode> propertiesDataNodes = new Dictionary<System.Reflection.PropertyInfo, DataNode>();
                    Dictionary<System.Reflection.PropertyInfo, IBridgeEnumerator> propertiesBridgeEnumerator = new Dictionary<System.Reflection.PropertyInfo, IBridgeEnumerator>();
                    LoadPropertiesMetaData(propertiesDataNodes, propertiesBridgeEnumerator);
                    IBridgeEnumerator bridgeEnumerator = Activator.CreateInstance(typeof(BridgeEnumerator<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, propertiesDataNodes, propertiesBridgeEnumerator) as IBridgeEnumerator;
                    (linqObjectQuery as ILINQObjectQuery).AddSelecionTypeEnumerator(SelectCollectionType, bridgeEnumerator);
                    #endregion

                    UpdateDataNodesAlias();
                }
                return DataNode;
            }
            else
            {

                DataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery);
                SourceCollection.DataNode.ParentDataNode = DataNode;
                DataNode.Name = "Root";
                DataNode.Temporary = true;

                Dictionary<System.Reflection.PropertyInfo, DataNode> propertiesDataNodes = new Dictionary<System.Reflection.PropertyInfo, DataNode>();
                Dictionary<System.Reflection.PropertyInfo, IBridgeEnumerator> propertiesBridgeEnumerator = new Dictionary<System.Reflection.PropertyInfo, IBridgeEnumerator>();
                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                    LoadPropertiesMetaData(propertiesDataNodes, propertiesBridgeEnumerator);
                else
                    SelectCollection.BuildDataNodeTree(SourceCollection.DataNode, linqObjectQuery);

                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                    AttatchNestQueryToMainDataTree();

                Dictionary<DataNode, DataNode> replacedDataNodes = new Dictionary<DataNode, DataNode>();
                #region Merge dataNodes
                ExpressionTranslator.MergeDataNodeTree(DataNode, replacedDataNodes);
                if (ReferenceDataNode != null)
                {
                    bool sourceColectionDataNodeTemporary = SourceCollection.DataNode.Temporary;
                    if (Parent.Name != "Root")
                        SourceCollection.DataNode.Temporary = false;
                    ExpressionTranslator.RemoveTemporaryNodes(ref DataNode, replacedDataNodes);
                    SourceCollection.DataNode.Temporary = sourceColectionDataNodeTemporary;
                }
                else
                    ExpressionTranslator.RemoveTemporaryNodes(ref DataNode, replacedDataNodes);
                #endregion

                Translators.QueryTranslator.ShowDataNodePathsInOutLog(DataNode);


                #region Builds bridge enumerator
                if (SelectCollection.Expression.NodeType == ExpressionType.New)
                {
                    BridgeEnumerator = Activator.CreateInstance(typeof(BridgeEnumerator<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, propertiesDataNodes, propertiesBridgeEnumerator) as IBridgeEnumerator;
                    (linqObjectQuery as ILINQObjectQuery).AddSelecionTypeEnumerator(SelectCollectionType, BridgeEnumerator);
                }
                else
                {
                    BridgeEnumerator = (linqObjectQuery as ILINQObjectQuery).GetSelecionTypeEnumerator(SelectCollectionType);
                    if (BridgeEnumerator == null)
                        BridgeEnumerator = Activator.CreateInstance(typeof(BridgeEnumerator<>).MakeGenericType(SelectCollectionType), linqObjectQuery, DataNode, SelectCollection.DataNode) as IBridgeEnumerator;
                }
                BridgeEnumerator.UpdateDataNodes(replacedDataNodes);
                BridgeEnumerator.ParticipateInSelectClass();
                if (Parent.Name == "Root")
                    (linqObjectQuery as ILINQObjectQuery).QueryResultEnumerator = BridgeEnumerator as System.Collections.IEnumerator;
                #endregion

                return DataNode;
            }

        }
        internal override SearchCondition BuildSearchCondition(SearchCondition searchCondition)
        {
            foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
            {
                if (treeNode is MethodCallAsCollectionSourceExpressionTreeNode)
                    (treeNode as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(null);
            }
            if (SourceCollection is MethodCallAsCollectionSourceExpressionTreeNode)
            {
                if (Parent.Name == "Root")
                {
                    DataNode.SearchCondition = (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
                    return DataNode.SearchCondition;
                }
                else if (Parent.Expression != null && Parent.Expression.NodeType == ExpressionType.New)
                {
                    DataNode.SearchCondition = (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
                    return DataNode.SearchCondition;
                }
                else
                    return (SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode).BuildSearchCondition(searchCondition);
            }
            else
                return searchCondition;
        }
        DataNode GetSelectionItemRootDataNode(string selectionItemName)
        {
            DataNode derivedCollectionDataNode = SelectionCollectionSource.DataNode;
            while (derivedCollectionDataNode.ParentDataNode != null && derivedCollectionDataNode != SourceCollection.DataNode &&
                (!derivedCollectionDataNode.ParentDataNode.HasAlias(selectionItemName) && !(derivedCollectionDataNode.ParentDataNode.Temporary && derivedCollectionDataNode.ParentDataNode.Name == selectionItemName)))
            {
                derivedCollectionDataNode = derivedCollectionDataNode.ParentDataNode;
            }
            if (derivedCollectionDataNode.ParentDataNode != null && derivedCollectionDataNode.ParentDataNode.HasAlias(selectionItemName) || (derivedCollectionDataNode.ParentDataNode.Temporary && derivedCollectionDataNode.ParentDataNode.Name == selectionItemName))
                return derivedCollectionDataNode.ParentDataNode;
            else
                return SelectionCollectionSource.DataNode;
        }
        private void LoadPropertiesMetaData(Dictionary<System.Reflection.PropertyInfo, DataNode> propertiesDataNodes, Dictionary<System.Reflection.PropertyInfo, IBridgeEnumerator> propertiesBridgeEnumerator)
        {
            int i = 0;
            DataNode dataNode = null;
            foreach (ExpressionTreeNode treeNode in SelectCollection.Nodes)
            {
                dataNode = GetSelectionItemRootDataNode(treeNode.Name);
                dataNode = treeNode.BuildDataNodeTree(dataNode, ExpressionTranslator.LINQObjectQuery);
                if (SelectCollection.Expression is System.Linq.Expressions.NewExpression)
                {
                    System.Reflection.PropertyInfo property = SelectCollectionType.GetProperties()[i];
                    propertiesDataNodes.Add(property, dataNode);
                    if (treeNode is MethodCallAsCollectionSourceExpressionTreeNode)
                        propertiesBridgeEnumerator[property] = (treeNode as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                }
                i++;
            }

        }
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

        ExpressionTreeNode SelectCollection
        {
            get
            {
                return Nodes[2];
            }
        }
        Type SelectCollectionType
        {
            get
            {
                if (SelectCollection.Expression is NewExpression)
                    return (SelectCollection.Expression as NewExpression).Type;
                else
                {
                    ExpressionTreeNode expressionTreeNode = SelectCollection;
                    while (expressionTreeNode.Nodes.Count > 0)
                        expressionTreeNode = expressionTreeNode.Nodes[0];
                    return expressionTreeNode.Expression.Type;
                }
            }
        }
        ExpressionTreeNode SelectionCollectionSource
        {
            get
            {
                return DerivedCollection;
            }
        }
        internal override ExpressionTreeNode SourceCollection
        {
            get
            {
                return Nodes[0];
            }
        }

        ExpressionTreeNode DerivedCollection
        {
            get
            {
                return Nodes[1];
            }
        }
    }
}
