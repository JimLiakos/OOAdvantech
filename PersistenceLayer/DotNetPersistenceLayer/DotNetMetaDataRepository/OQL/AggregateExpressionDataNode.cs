using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    
    /// <MetaDataID>{53db2d17-a7c9-45e6-9ff5-82abca6fbeb1}</MetaDataID>
    ///<summary>
    ///Class AggregateExpressionDataNode refer to data which produced from aggregate function
    ///</summary>
    [Serializable]
    public class AggregateExpressionDataNode : DataNode
    {

        AggregateExpressionDataNode(Guid identity):base(identity)
        {

        }
        internal override DataNode Clone(Dictionary<object, object> clonedObjects)
        {

            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataNode;

            var newDataNode = new AggregateExpressionDataNode(Identity);
            clonedObjects[this] = newDataNode;
            Copy(newDataNode, clonedObjects);

            if (_AggregateExpressionDataNodes != null)
            {
                newDataNode._AggregateExpressionDataNodes = new List<DataNode>();
                foreach (var aggrDataNode in _AggregateExpressionDataNodes)
                    newDataNode._AggregateExpressionDataNodes.Add(aggrDataNode.Clone(clonedObjects));
            }
            if(ArithmeticExpression!=null)
                newDataNode.ArithmeticExpression = ArithmeticExpression.Clone(clonedObjects);
            if (ArithmeticExpressionConditionFalse != null)
                newDataNode.ArithmeticExpressionConditionFalse = ArithmeticExpressionConditionFalse.Clone(clonedObjects);
            if (SourceSearchCondition != null)
                newDataNode.SourceSearchCondition = SourceSearchCondition.Clone(clonedObjects);
            
            return newDataNode;

        }
        internal override bool IsDataSource
        {
            get
            {
                return false;
            }
        }

        internal override bool IsDataSourceMember
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Defines the condition which must fulfill the items of aggregation source 
        /// </summary>
        [Association("", Roles.RoleA, "6b3f8478-7ed8-42de-b8ae-114fea990977")]
        [IgnoreErrorCheck]
        public SearchCondition SourceSearchCondition;

        /// <summary>
        /// Defines the arithmetic expression which uses the 
        /// aggregate function to calculates the function result when condition is false.
        /// </summary>
        [Association("", Roles.RoleA, "2c2d7542-1a3a-4d10-a644-869112edf4c1")]
        [IgnoreErrorCheck]
        public ArithmeticExpression ArithmeticExpressionConditionFalse;

        /// <summary>
        /// Defines the arithmetic expression which uses the 
        /// aggregate function to calculates the function result.
        /// </summary>
        [Association("", Roles.RoleA, "55ce5914-68ee-467c-89a6-2ad0ed4388de")]
        [IgnoreErrorCheck]
        public ArithmeticExpression ArithmeticExpression;
        

   
        /// <MetaDataID>{60d243d1-af64-4e24-a575-bd942588714b}</MetaDataID>
        public AggregateExpressionDataNode(ObjectQuery mOQLStatement)
            : base(mOQLStatement)
        {

        }

      

        /// <MetaDataID>{19cb4b47-c23c-48e4-97bf-7c119e3b7032}</MetaDataID>
        public override string Alias
        {
            get
            {
                if (!string.IsNullOrEmpty(_Alias))
                    return _Alias;
                else if (ParentDataNode!=null)
                    _Alias = ObjectQuery.GetValidAlias(ParentDataNode.Alias + "_" + Name);
                return _Alias;
            }
            set
            {
                base.Alias = value;
            }
        }

        /// <MetaDataID>{85a087b2-17ac-42fa-a3aa-22b2c3eb624c}</MetaDataID>
        public override void MergeIdenticalDataNodes()
        {
            foreach (DataNode subDataNode in new List<DataNode>(SubDataNodes))
            {
                foreach (DataNode parentSubDataNode in new List<DataNode>(ParentDataNode.SubDataNodes))
                {
                    if (SubDataNodes.Contains(subDataNode) && ParentDataNode.SubDataNodes.Contains(parentSubDataNode))
                    {
                        if (parentSubDataNode.Name == subDataNode.Name)
                        {
                            SubDataNodes.Remove(subDataNode);
                            System.Collections.Generic.List<DataNode> TempSubDataNodes = new List<DataNode>(subDataNode.SubDataNodes);
                            foreach (DataNode CurrDataNode in TempSubDataNodes)
                                CurrDataNode.ParentDataNode = parentSubDataNode;

                            foreach (Path path in subDataNode.RelatedPaths)
                                parentSubDataNode.RelatedPaths.Add(path);
                            if (subDataNode.ParticipateInWereClause)
                                parentSubDataNode.ParticipateInWereClause = true;

                            parentSubDataNode.OrderBy = subDataNode.OrderBy;
                            foreach (string alias in subDataNode.Aliases)
                                parentSubDataNode.Alias = alias;

                            if (_AggregateExpressionDataNodes != null && _AggregateExpressionDataNodes.Contains(subDataNode))
                            {
                                _AggregateExpressionDataNodes.Remove(subDataNode);
                                _AggregateExpressionDataNodes.Add(parentSubDataNode);
                            }


                        }

                    }
                }
            }
        }



        /// <MetaDataID>{30d6aa96-69b0-4157-bda2-f64d91579255}</MetaDataID>
        internal AggregateExpressionDataNode(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery objectQuery, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path path)
            : base(objectQuery, path)
        {

            string aggregateExpressionName = null;
            if (_Type == DataNodeType.Count)
                Name = "Count(" + aggregateExpressionName.Replace(".", "_") + ")";
                
            if (Path.AggregationPath)
            {
                if ((Path.ParserNode[0][0] as Parser.ParserNode).Name.ToLower() == "count")
                {
                    _Type = DataNodeType.Count;
                    if (Path.AggregateExpressionPath != null)
                    {
                        Path aggregateExpressionPath = null;
                        GetAggregateExpression(Path.AggregateExpressionPath, out aggregateExpressionPath, ref aggregateExpressionName);

                        AggregateExpressionPaths.Add(aggregateExpressionPath);
                    }
                    else
                    {
                        aggregateExpressionName = "";
                    }
                    if (Alias == null)
                        _Alias = "Count";// ObjectQuery.GetValidAlias("Count");
                }
            }

            if (_Type == DataNodeType.Count)
                Name = "Count(" + aggregateExpressionName.Replace(".", "_") + ")";


        }


        /// <MetaDataID>{9ae95944-ec1a-4258-be2a-d5595184bcde}</MetaDataID>
        public void AddAggregateExpressionDataNode(DataNode dataNode)
        {
            if (dataNode == null)
                throw new System.NullReferenceException("dataNode parameter is null");
            dataNode.ParticipateInAggregateFunction = true;
            if (_AggregateExpressionDataNodes == null)
                CreateAggregateExpressionDataNodesCollection();
            if (!_AggregateExpressionDataNodes.Contains(dataNode))
                _AggregateExpressionDataNodes.Add(dataNode);
            
        }
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<DataNode> _AggregateExpressionDataNodes;
        /// <MetaDataID>{302ae13d-4e68-4220-8780-d3d3491ea073}</MetaDataID>
        public ReadOnlyCollection<DataNode> AggregateExpressionDataNodes
        {
            get
            {
                if (_AggregateExpressionDataNodes == null)
                    CreateAggregateExpressionDataNodesCollection();
                return new System.Collections.ObjectModel.ReadOnlyCollection<DataNode>(_AggregateExpressionDataNodes);
            }
        }

        /// <MetaDataID>{40983941-bc2d-4c1d-813a-308982609d42}</MetaDataID>
        private void CreateAggregateExpressionDataNodesCollection()
        {
            if (Type == DataNodeType.Count && _AggregateExpressionDataNodes == null)
            {
                _AggregateExpressionDataNodes = new System.Collections.Generic.List<DataNode>();
                if (AggregateExpressionPaths != null)
                {
                    foreach (Path path in AggregateExpressionPaths)
                    {
                        DataNode dataNode = GetAggregateExpression(this.ParentDataNode, path);
                        if (!_AggregateExpressionDataNodes.Contains(dataNode))
                            _AggregateExpressionDataNodes.Add(dataNode);
                        while (dataNode.ParentDataNode != this && dataNode.ParentDataNode != null)
                            dataNode = dataNode.ParentDataNode;
                        dataNode.ParentDataNode = ParentDataNode;
                    }
                }
            }
            else
                _AggregateExpressionDataNodes = new System.Collections.Generic.List<DataNode>();
        }
        /// <MetaDataID>{c75c03b3-844c-4002-99d4-ecb2005ddcc2}</MetaDataID>
        [NonSerialized]
        System.Collections.Generic.List<Path> AggregateExpressionPaths = new System.Collections.Generic.List<Path>();



        /// <MetaDataID>{5566916a-5f5f-42a7-82e8-9ce413214555}</MetaDataID>
        void GetAggregateExpression(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path path, out OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path aggregateExpressionPath, ref string aggregateExpressionName)
        {
            aggregateExpressionName += path.Name;
            if (path.SubPath != null)
            {
                aggregateExpressionName += ".";
                GetAggregateExpression(path.SubPath, out aggregateExpressionPath, ref aggregateExpressionName);
            }
            else
                aggregateExpressionPath = path;
        }

        /// <MetaDataID>{448769ee-0370-4c52-9f57-b408e9d43db0}</MetaDataID>
        public override void AddSearchCondition(SearchCondition searchCondition)
        {
            //if (Type == DataNodeType.Count)
            //    AggregateExpressionDataNodes[0].AddSearchCondition(searchCondition);
            //else
            ParentDataNode.AddSearchCondition(searchCondition);
            //base.AddSearchCondition(searchCondition);
        }
        /// <MetaDataID>{30ed61c7-d318-4658-8d33-b67a3e520638}</MetaDataID>
        public override void RemoveSearchCondition(SearchCondition searchCondition)
        {
            if (Type == DataNodeType.Count)
                AggregateExpressionDataNodes[0].RemoveSearchCondition(searchCondition);
            else
                base.RemoveSearchCondition(searchCondition);

            //base.RemoveSearchCondition(searchCondition);
        }
    
        /// <MetaDataID>{e589c90f-ddbc-4c6c-98a0-485e96f6f415}</MetaDataID>
        DataNode GetAggregateExpression(DataNode rootDataNode, Path aggregateExpressionPath)
        {
            if (aggregateExpressionPath == null && ParentDataNode.Type == DataNodeType.Group && Type == DataNodeType.Count)
            {
                foreach (DataNode parentSubDataNode in ParentDataNode.SubDataNodes)
                {
                    if (parentSubDataNode.Type == DataNodeType.Object)
                        return parentSubDataNode;
                }
            }
            if (aggregateExpressionPath == null)
                return null;
            if (rootDataNode.RelatedPaths.Contains(aggregateExpressionPath))
                return rootDataNode;
            foreach (DataNode subDataNode in rootDataNode.SubDataNodes)
            {
                DataNode dataNode = GetAggregateExpression(subDataNode, aggregateExpressionPath);
                if (dataNode != null)
                    return dataNode;
            }
            return null;
        }

        /// <MetaDataID>{cdbb77f3-24ef-4d31-8f6e-3b029e26615f}</MetaDataID>
        internal object CalculateValue(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode, int> dataNodeRowIndices, System.Collections.Generic.List<IDataRow[]> compositeRows)
        {

            if (ArithmeticExpression == null)
            {

                DataNode aggregateExpressionDataNode = AggregateExpressionDataNodes[0];
                int dataSourceRowIndex =DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices,aggregateExpressionDataNode);
                System.Type type = (aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                if (compositeRows.Count > 0)
                    type = compositeRows[0][dataSourceRowIndex].Table.Columns[aggregateExpressionDataNode.DataSourceColumnIndex].DataType;
                int count = 0;
                if (Type == DataNodeType.Sum || Type == DataNodeType.Average)
                {
                    ICalculatedValue sum = Add.GetTypedCalculator(type);
                    foreach (IDataRow[] compositeRow in compositeRows)
                    {
                        if (SourceSearchCondition != null)
                        {
                            if (SourceSearchCondition.DoesRowPassCondition(compositeRow, dataNodeRowIndices))
                            {
                                sum.Calculate(compositeRow[dataSourceRowIndex][aggregateExpressionDataNode.DataSourceColumnIndex]);
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                            sum.Calculate(compositeRow[dataSourceRowIndex][aggregateExpressionDataNode.DataSourceColumnIndex]);
                        }
                    }
                    if (Type == DataNodeType.Sum)
                        return sum.Value;
                    if (compositeRows.Count > 0)
                        return Divide.GetTypedCalculator(type).Calculate(sum.Value, count);
                    else
                        return sum.Value;
                }
                if (Type == DataNodeType.Min)
                {

                }
                if (Type == DataNodeType.Max)
                {

                }

            }
            else
            {
                if (Type == DataNodeType.Sum || Type == DataNodeType.Average)
                {
                    ICalculatedValue sum = Add.GetTypedCalculator(ArithmeticExpression.ResultType);
                    foreach (IDataRow[] compositeRow in compositeRows)
                    {
                        if (SourceSearchCondition != null)
                        {
                            if (SourceSearchCondition.DoesRowPassCondition(compositeRow, dataNodeRowIndices))
                                sum.Calculate(ArithmeticExpression.CalculateValue(compositeRow, dataNodeRowIndices));
                        }
                        else
                            sum.Calculate(ArithmeticExpression.CalculateValue(compositeRow, dataNodeRowIndices));
                    }
                    if (Type == DataNodeType.Sum)
                        return sum.Value;
                    if (compositeRows.Count > 0)
                        return Divide.GetTypedCalculator(ArithmeticExpression.ResultType).Calculate(sum.Value, compositeRows.Count);
                    else
                        return sum.Value;
                }
                if (Type == DataNodeType.Min)
                {
                    decimal min = 0;
                    bool first = true;
                    foreach (IDataRow[] compositeRow in compositeRows)
                    {
                        decimal value = Convert.ToDecimal(ArithmeticExpression.CalculateValue(compositeRow, dataNodeRowIndices));
                        if (SourceSearchCondition != null)
                        {
                            if (!SourceSearchCondition.DoesRowPassCondition(compositeRow, dataNodeRowIndices))
                                continue;
                        }
                        
                        if (first)
                        {
                            min = value;
                            first=false;
                        }

                        if (value < min)
                            min = value;
                    }
                    return min;

                }
                if (Type == DataNodeType.Max)
                {
                    decimal max = 0;
                    foreach (IDataRow[] compositeRow in compositeRows)
                    {
                        if (SourceSearchCondition != null)
                        {
                            if (!SourceSearchCondition.DoesRowPassCondition(compositeRow, dataNodeRowIndices))
                                continue;
                        }
                        
                        decimal value = Convert.ToDecimal(ArithmeticExpression.CalculateValue(compositeRow, dataNodeRowIndices));
                        if (value > max)
                            max = value;
                    }
                    return max;

                }

            }

            return 0;
        }

        /// <MetaDataID>{7877dc10-a5ee-4b5b-8c96-ddbafda75f7f}</MetaDataID>
        internal object CalculateValue(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode, int> dataNodeRowIndices, System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.CompositeRowData> compositeRows)
        {

            if (ArithmeticExpression == null)
            {

                DataNode aggregateExpressionDataNode = AggregateExpressionDataNodes[0];
                int dataSourceRowIndex = DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, aggregateExpressionDataNode);
                System.Type type = (aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                if (compositeRows.Count > 0)
                    type = compositeRows[0][dataSourceRowIndex].DataRow.Table.Columns[aggregateExpressionDataNode.DataSourceColumnIndex].DataType;
                if (Type == DataNodeType.Sum || Type == DataNodeType.Average)
                {
                    ICalculatedValue sum = Add.GetTypedCalculator(type);
                    foreach (var compositeRow in compositeRows)
                        sum.Calculate(compositeRow[dataSourceRowIndex][aggregateExpressionDataNode.DataSourceColumnIndex]);
                    if (Type == DataNodeType.Sum)
                        return sum.Value;
                    if (compositeRows.Count > 0)
                        return Divide.GetTypedCalculator(type).Calculate(sum.Value, compositeRows.Count);
                    else
                        return sum.Value;
                }
                if (Type == DataNodeType.Min)
                {

                }
                if (Type == DataNodeType.Max)
                {

                }

            }
            else
            {
                if (Type == DataNodeType.Sum || Type == DataNodeType.Average)
                {
                    ICalculatedValue sum = Add.GetTypedCalculator(ArithmeticExpression.ResultType);
                    foreach (CompositeRowData compositeRow in compositeRows)
                        sum.Calculate(ArithmeticExpression.CalculateValue(compositeRow.CompositeRow, dataNodeRowIndices));
                    if (Type == DataNodeType.Sum)
                        return sum.Value;
                    if (compositeRows.Count > 0)
                        return Divide.GetTypedCalculator(ArithmeticExpression.ResultType).Calculate(sum.Value, compositeRows.Count);
                    else
                        return sum.Value;
                }
                if (Type == DataNodeType.Min)
                {
                    decimal min = 0;
                    bool first = true;
                    foreach (var compositeRow in compositeRows)
                    {
                        decimal value = Convert.ToDecimal(ArithmeticExpression.CalculateValue(compositeRow.CompositeRow, dataNodeRowIndices));
                        if (first)
                        {
                            min = value;
                            first = false;
                        }

                        if (value < min)
                            min = value;
                    }
                    return min;

                }
                if (Type == DataNodeType.Max)
                {
                    decimal max = 0;
                    foreach (var compositeRow in compositeRows)
                    {
                        decimal value = Convert.ToDecimal(ArithmeticExpression.CalculateValue(compositeRow.CompositeRow, dataNodeRowIndices));
                        if (value > max)
                            max = value;
                    }
                    return max;

                }

            }

            return 0;
        }

        /// <MetaDataID>{089526ff-cd52-4d64-9c17-6e117eb8e375}</MetaDataID>
        internal bool IsEquivalent(AggregateExpressionDataNode aggregateExpressionDataNode)
        {
            if (AggregateExpressionDataNodes.Count != aggregateExpressionDataNode.AggregateExpressionDataNodes.Count)
                return false;

            if (ArithmeticExpression == null && AggregateExpressionDataNodes[0] != aggregateExpressionDataNode.AggregateExpressionDataNodes[0])
                return false;
            if (ArithmeticExpression != aggregateExpressionDataNode.ArithmeticExpression)
                return false;
            return true;

        }
    }
}
