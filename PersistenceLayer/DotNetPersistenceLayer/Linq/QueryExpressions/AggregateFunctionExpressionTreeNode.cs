using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{e850c8d1-8ed0-4ac8-8901-06cdb4cb6f63}</MetaDataID>
    class AggregateFunctionExpressionTreeNode : OOAdvantech.Linq.QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode
    {
        /// <MetaDataID>{576e4487-9074-4965-8cd4-7fc88b600805}</MetaDataID>
        private ArithmeticExpression ArithmeticExpressionConditionFalse;




        /// <MetaDataID>{fffb46fc-dac8-4681-a0ef-27cf63bc3aaa}</MetaDataID>
        public AggregateFunctionExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.Call)
                throw new System.Exception("Wrong expression type");
        }

        /// <exclude>Excluded</exclude>
        ExpressionTreeNode _RootExpressionTreeNode;

        /// <exclude>Excluded</exclude>
        SearchCondition _AggregateFunctionSourceSearchCondition;
        /// <MetaDataID>{f79e7989-31b1-44df-9b56-96de3ffb64c8}</MetaDataID>
        public SearchCondition AggregateFunctionCondition
        {
            get
            {
                return _AggregateFunctionSourceSearchCondition;
            }

        }



        /// <MetaDataID>{822dff38-1124-4f51-8c96-dc52b36fca17}</MetaDataID>
        public override void BuildDataFilter()
        {

            if (Parent.Name == "Root" && Parent.Parent == null)
            {
                if (SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
                    (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).BuildDataFilter();
            }
            
            if (AggregateConditionExpression != null)
            {
                _AggregateFunctionSourceSearchCondition = new SearchCondition(new List<SearchTerm>() { new SearchTerm() }, ExpressionTranslator.LINQObjectQuery);
                SearchFactor searchFactor = new SearchFactor();
                AggregateConditionExpression.BuildSearchFactor(searchFactor, true);
                _AggregateFunctionSourceSearchCondition.SearchTerms[0].AddSearchFactor(searchFactor);
                _AggregateFunctionSourceSearchCondition = SearchCondition.JoinSearchConditions(_AggregateFunctionSourceSearchCondition, SourceCollection.FilterDataCondition);
                foreach (Criterion criterion in _AggregateFunctionSourceSearchCondition.Criterions)
                {
                    if (criterion.LeftTermDataNode != null)
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(criterion.LeftTermDataNode);
                    if (criterion.RightTermDataNode != null)
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(criterion.RightTermDataNode);
                }
            }
            else
                _AggregateFunctionSourceSearchCondition = SourceCollection.FilterDataCondition;
            (_DataNode as AggregateExpressionDataNode).SourceSearchCondition = _AggregateFunctionSourceSearchCondition;
            if (_AggregateFunctionSourceSearchCondition != null)
            {
                foreach (Criterion criterion in _AggregateFunctionSourceSearchCondition.Criterions)
                {
                    if (criterion.LeftTermDataNode != null)
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(criterion.LeftTermDataNode);
                    if (criterion.RightTermDataNode != null)
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(criterion.RightTermDataNode);
                }
            }

        }

        /// <MetaDataID>{d924bd9f-7c69-4fe6-a212-0692d7b565ef}</MetaDataID>
        ExpressionTreeNode RootExpressionTreeNode
        {
            get
            {
                if (_RootExpressionTreeNode == null)
                {
                    _RootExpressionTreeNode = this;
                    while (_RootExpressionTreeNode.Parent != null)
                        _RootExpressionTreeNode = _RootExpressionTreeNode.Parent;
                }
                return _RootExpressionTreeNode;

            }
        }
        /// <MetaDataID>{2a3a54ea-6e5e-4356-bdc8-c7f2b2ec852a}</MetaDataID>
        internal override OOAdvantech.Linq.ExpressionTreeNode SourceCollection
        {
            get
            {
                return Nodes[0];
            }
        }
        protected OOAdvantech.Linq.QueryExpressions.BinaryExpressionTreeNode AggregateConditionExpression
        {
            get
            {
                if (Nodes.Count > 1 && Nodes[1] is BinaryExpressionTreeNode)
                    return Nodes[1] as BinaryExpressionTreeNode;
                else if (Nodes.Count == 1 && Nodes[0] is BinaryExpressionTreeNode)
                    return Nodes[0] as BinaryExpressionTreeNode;
                else if (Nodes[0] is WhereExpressionTreeNode)
                    return (Nodes[0] as WhereExpressionTreeNode).Nodes[1] as BinaryExpressionTreeNode;
                else
                    return null;
            }
        }

        /// <MetaDataID>{5c78c909-daaa-482d-9874-78c3ad5b984f}</MetaDataID>
        protected OOAdvantech.Linq.ExpressionTreeNode AggregateExpression
        {
            get
            {
                if (Nodes.Count == 4 && Nodes[1] is BinaryExpressionTreeNode)
                    return Nodes[2];
                else if (Nodes.Count > 1)
                    return Nodes[1];
                else
                    return null;
            }
        }


        /// <MetaDataID>{69fd4a5b-7243-41d6-bf84-6ff3880efd3c}</MetaDataID>
        protected ExpressionTreeNode AggregateExpressionConditionFalse
        {
            get
            {
                if (Nodes.Count == 4 && Nodes[1] is BinaryExpressionTreeNode)
                    return Nodes[3];
                else
                    return null;

            }
        }


        /// <MetaDataID>{db6166d1-def6-4047-ace7-5c3b65e172a8}</MetaDataID>
        ExpressionTreeNode GetExpressionTreeNode(OOAdvantech.Linq.ExpressionTreeNode expressionTreeNode, string name)
        {
            if (expressionTreeNode.Alias == name)
                return expressionTreeNode;
            foreach (ExpressionTreeNode node in expressionTreeNode.Nodes)
            {
                if (node.Alias == name)
                    return node;
                ExpressionTreeNode nodeWithName = GetExpressionTreeNode(node, name);
                if (nodeWithName != null)
                    return nodeWithName;
            }
            return null;
        }

        /// <MetaDataID>{83258cdb-9782-4b05-9cdc-0002328466f9}</MetaDataID>
        ArithmeticExpression ArithmeticExpression;

        // internal  bool AutoGenGroup;
        /// <MetaDataID>{920d4107-f992-40db-8904-290e742d8b5b}</MetaDataID>
        private DataNode BuildSourceCollectionDataNode(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

            //if (dataNode is GroupDataNode)
            //    groupDataNode = dataNode as GroupDataNode;
            if (Parent.Name == "Root" && Parent.Parent == null)
            {
                GroupDataNode groupDataNode = null;
                #region In case where there is not group by DataNode as Root create a pseudo group by dataNode
                SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
                DataNode groupedDataNode = null;
                if (SourceCollection is SelectExpressionTreeNode)
                    groupedDataNode = (SourceCollection as SelectExpressionTreeNode).SelectCollection.DataNode;
                else if (SourceCollection is ParameterExpressionTreeNode)
                    groupedDataNode = (SourceCollection as ParameterExpressionTreeNode).DataNode;
                else
                    groupedDataNode = (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DataNode;

                if (dataNode != null)
                {
                    foreach (var subDataNode in dataNode.SubDataNodes)
                    {
                        if (subDataNode is GroupDataNode && (subDataNode as GroupDataNode).GroupedDataNode == groupedDataNode)
                        {
                            groupDataNode = subDataNode as GroupDataNode;
                            break;
                        }
                    }
                }
                if (groupDataNode == null)
                {
                    groupDataNode = new GroupDataNode(linqObjectQuery.ObjectQuery);
                    groupDataNode.ParentDataNode = dataNode;
                    groupDataNode.Type = DataNode.DataNodeType.Group;
                    groupDataNode.Name = "AutoGenGroup";
                    groupDataNode.Alias = "AutoGenGroup";
                    //AutoGenGroup = true;
                }


                if (dataNode == null)
                    SourceCollection.DataNode.HeaderDataNode.ParentDataNode = groupDataNode;
                while (groupedDataNode.Type == DataNode.DataNodeType.OjectAttribute || groupDataNode.ValueTypePath.Count > 0)
                    groupedDataNode = groupedDataNode.ParentDataNode;

                groupDataNode.GroupedDataNode = groupedDataNode;
                groupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;
                dataNode = groupDataNode;

                #endregion
            }
            else
            {
                if (SourceCollection.DataNode == null)
                    if (dataNode.Type == DataNode.DataNodeType.Group)
                        SourceCollection.BuildDataNodeTree((dataNode as GroupDataNode).GroupedDataNode, linqObjectQuery);
                    else
                        dataNode = SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery).ParentDataNode;
            }
            return dataNode;
        }

        /// <MetaDataID>{3a478209-03fb-4de4-a4b0-6458bd12534c}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

           DataNode resultDataNode=  BuildSourceCollectionDataNode(dataNode, linqObjectQuery);
            if (dataNode == null) //AutoGenGroup
                dataNode = resultDataNode;

            _DataNode = new AggregateExpressionDataNode(linqObjectQuery.ObjectQuery);
            _DataNode.Name = (Expression as System.Linq.Expressions.MethodCallExpression).Method.Name;
            _DataNode.ParentDataNode = dataNode;

            if (!string.IsNullOrEmpty(Alias))
                _DataNode.Alias = Alias;

            switch ((Expression as System.Linq.Expressions.MethodCallExpression).Method.Name)
            {
                case "Average":
                    _DataNode.Type = DataNode.DataNodeType.Average;
                    break;
                case "Count":
                    _DataNode.Type = DataNode.DataNodeType.Count;
                    break;
                case "Max":
                    _DataNode.Type = DataNode.DataNodeType.Max;
                    break;
                case "Min":
                    _DataNode.Type = DataNode.DataNodeType.Min;
                    break;
                case "Sum":
                    _DataNode.Type = DataNode.DataNodeType.Sum;
                    break;
                default:
                    _DataNode.Type = DataNode.DataNodeType.Unknown;
                    break;
            }

            if (_DataNode.Type != DataNode.DataNodeType.Count && (Nodes[1] is BinaryExpressionTreeNode))
            {
                ///TODO να φτιαχτέί στο μέλλον να υποστιρίζει condition και για τις άλλες aggregation functions  
                throw new System.NotSupportedException(string.Format("Object State Managment System doesn't support condition on {0} method", (Expression as System.Linq.Expressions.MethodCallExpression).Method.Name));
            }

            if (AggregateExpression != null || AggregateConditionExpression != null)
            {

                if (AggregateExpression is ArithmeticExpressionTreeNode)
                {
                    ArithmeticExpression = new CompositeArithmeticExpression(Expression.Type);
                    (_DataNode as AggregateExpressionDataNode).ArithmeticExpression = ArithmeticExpression;
                    (AggregateExpression as ArithmeticExpressionTreeNode).BuildDataNodeTree(_DataNode, linqObjectQuery);
                    (AggregateExpression as ArithmeticExpressionTreeNode).BuildArithmeticExpression(ArithmeticExpression as CompositeArithmeticExpression);
                    foreach (DataNode arithmeticExpressionDataNode in ArithmeticExpression.ArithmeticExpressionDataNodes)
                    {
                        arithmeticExpressionDataNode.ParticipateInAggregateFunction = true;
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(arithmeticExpressionDataNode);
                    }
                }
                else if (AggregateExpression != null && AggregateExpression.Expression.NodeType == ExpressionType.Parameter)
                {

                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = SourceCollection.DataNode;// Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, SourceCollection.Name);
                    //if (aliasDataNode.Type == DataNode.DataNodeType.Group)
                    //    aliasDataNode = aliasDataNode.GroupByDataNode;

                    if (aliasDataNode != null)
                    {
                        DataNode arithmeticExpressionDataNode = AggregateExpression.BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                        arithmeticExpressionDataNode.ParticipateInAggregateFunction = true;
                        //if (arithmeticExpressionDataNode is AggregateExpressionDataNode)
                        //    ArithmeticExpression = (arithmeticExpressionDataNode as AggregateExpressionDataNode).ArithmeticExpression;
                        //else
                        ArithmeticExpression = new ScalarFromData(arithmeticExpressionDataNode);
                        (_DataNode as AggregateExpressionDataNode).ArithmeticExpression = ArithmeticExpression;
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(arithmeticExpressionDataNode);

                    }
                }
                else if (AggregateExpression is ConstantExpressionTreeNode)
                {
                    ArithmeticExpression = new ScalarFromLiteral((AggregateExpression.Expression as ConstantExpression).Value);
                }



                if (AggregateExpressionConditionFalse != null)
                {
                    AggregateConditionExpression.BuildDataNodeTree(_DataNode, linqObjectQuery);

                    if (AggregateExpressionConditionFalse is ArithmeticExpressionTreeNode)
                    {
                        ArithmeticExpressionConditionFalse = new CompositeArithmeticExpression(Expression.Type);
                        (_DataNode as AggregateExpressionDataNode).ArithmeticExpressionConditionFalse = ArithmeticExpression;
                        (AggregateExpressionConditionFalse as ArithmeticExpressionTreeNode).BuildDataNodeTree(_DataNode, linqObjectQuery);
                        (AggregateExpressionConditionFalse as ArithmeticExpressionTreeNode).BuildArithmeticExpression(ArithmeticExpressionConditionFalse as CompositeArithmeticExpression);
                        foreach (DataNode arithmeticExpressionDataNode in ArithmeticExpression.ArithmeticExpressionDataNodes)
                        {
                            arithmeticExpressionDataNode.ParticipateInAggregateFunction = true;
                            (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(arithmeticExpressionDataNode);
                        }
                    }
                    else if (AggregateExpressionConditionFalse.Expression.NodeType == ExpressionType.Parameter)
                    {

                        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode = SourceCollection.DataNode;// Translators.QueryTranslator.GetDataNodeWithAlias(dataNode.HeaderDataNode, SourceCollection.Name);
                        //if (aliasDataNode.Type == DataNode.DataNodeType.Group)
                        //    aliasDataNode = aliasDataNode.GroupByDataNode;

                        if (aliasDataNode != null)
                        {
                            DataNode arithmeticExpressionDataNode = AggregateExpressionConditionFalse.BuildDataNodeTree(aliasDataNode, linqObjectQuery);
                            arithmeticExpressionDataNode.ParticipateInAggregateFunction = true;
                            //if (arithmeticExpressionDataNode is AggregateExpressionDataNode)
                            //    ArithmeticExpression = (arithmeticExpressionDataNode as AggregateExpressionDataNode).ArithmeticExpression;
                            //else
                            ArithmeticExpressionConditionFalse = new ScalarFromData(arithmeticExpressionDataNode);
                            (_DataNode as AggregateExpressionDataNode).ArithmeticExpressionConditionFalse = ArithmeticExpressionConditionFalse;
                            (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(arithmeticExpressionDataNode);

                        }
                    }
                    else if (AggregateExpressionConditionFalse is ConstantExpressionTreeNode)
                    {
                        ArithmeticExpressionConditionFalse = new ScalarFromLiteral((AggregateExpressionConditionFalse.Expression as ConstantExpression).Value);
                    }
                }

            }
            else
            {
                if (_DataNode.ParentDataNode == SourceCollection.DataNode && SourceCollection.DataNode is GroupDataNode)
                {
                    if (!(_DataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Contains((SourceCollection.DataNode as GroupDataNode).GroupedDataNode))
                    {
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode((SourceCollection.DataNode as GroupDataNode).GroupedDataNode);
                        (SourceCollection.DataNode as GroupDataNode).GroupedDataNode.ParticipateInAggregateFunction = true;
                    }
                }
                else
                {
                    if (DataNode.Type == DataNode.DataNodeType.Count)
                    {
                        DataNode countDataNode = null;
                        if (SourceCollection is SelectExpressionTreeNode)
                            countDataNode = (SourceCollection as SelectExpressionTreeNode).SelectCollection.DataNode;
                        else if (SourceCollection is MethodCallAsCollectionProviderExpressionTreeNode)
                            countDataNode = (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DataNode;
                        else
                            countDataNode = (SourceCollection as ParameterExpressionTreeNode).DataNode;
                        while (countDataNode.Type == DataNode.DataNodeType.OjectAttribute || countDataNode.ValueTypePath.Count > 0)
                            countDataNode = countDataNode.ParentDataNode;


                        if (!(_DataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes.Contains(countDataNode))
                        {
                            (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(countDataNode);
                            countDataNode.ParticipateInAggregateFunction = true;
                        }
                    }
                    else
                    {
                        var arithmeticExpressionDataNode = SourceCollection.DataNode;
                        ArithmeticExpression = new ScalarFromData(arithmeticExpressionDataNode);
                        (_DataNode as AggregateExpressionDataNode).ArithmeticExpression = ArithmeticExpression;
                        (_DataNode as AggregateExpressionDataNode).AddAggregateExpressionDataNode(arithmeticExpressionDataNode);
                    }

                }

            }


            if (Parent.Name == "Root")
            {
                _DynamicTypeDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(Expression.Type, linqObjectQuery, DataNode, _DataNode, this);
                (linqObjectQuery as ILINQObjectQuery).QueryResult = _DynamicTypeDataRetrieve;
            }
            return _DataNode;

        }
        ///// <summary>
        ///// Retrieve or
        ///// </summary>
        ///// <param name="dataNode"></param>
        ///// <param name="linqObjectQuery"></param>
        ///// <returns></returns>
        //private GroupDataNode GetGroupDataNode(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        //{
        //    GroupDataNode groupDataNode = null;
        //    if (dataNode is GroupDataNode)
        //        groupDataNode = dataNode as GroupDataNode;
        //    if (groupDataNode == null)// Parent.Name == "Root" && Parent.Parent == null)
        //    {
        //        #region In case where there is not group by DataNode as Root create a pseudo group by dataNode
        //        SourceCollection.BuildDataNodeTree(dataNode, linqObjectQuery);
        //        DataNode groupedDataNode = null;
        //        if (SourceCollection is SelectExpressionTreeNode)
        //            groupedDataNode = (SourceCollection as SelectExpressionTreeNode).SelectCollection.DataNode;
        //        else if (SourceCollection is ParameterExpressionTreeNode)
        //            groupedDataNode = (SourceCollection as ParameterExpressionTreeNode).DataNode;
        //        else
        //            groupedDataNode = (SourceCollection as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection.DataNode;

        //        if (dataNode != null)
        //        {
        //            foreach (var subDataNode in dataNode.SubDataNodes)
        //            {
        //                if (subDataNode is GroupDataNode && (subDataNode as GroupDataNode).GroupedDataNode == groupedDataNode)
        //                {
        //                    groupDataNode = subDataNode as GroupDataNode;
        //                    break;
        //                }
        //            }
        //        }
        //        if (groupDataNode == null)
        //        {
        //            groupDataNode = new GroupDataNode(linqObjectQuery.ObjectQuery);
        //            groupDataNode.ParentDataNode = dataNode;
        //            groupDataNode.Type = DataNode.DataNodeType.Group;
        //            groupDataNode.Name = "AutoGenGroup";
        //            groupDataNode.Alias = "AutoGenGroup";
        //        }


        //        if (dataNode == null)
        //            SourceCollection.DataNode.HeaderDataNode.ParentDataNode = groupDataNode;
        //        while (groupedDataNode.Type == DataNode.DataNodeType.OjectAttribute || groupDataNode.ValueTypePath.Count > 0)
        //            groupedDataNode = groupedDataNode.ParentDataNode;

        //        groupDataNode.GroupedDataNode = groupedDataNode;
        //        groupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;


        //        #endregion
        //    }
        //    else
        //    {
        //        if (SourceCollection.DataNode == null)
        //            SourceCollection.BuildDataNodeTree(groupDataNode.GroupedDataNode, linqObjectQuery);
        //    }
        //    return groupDataNode;
        //}
    }
}
