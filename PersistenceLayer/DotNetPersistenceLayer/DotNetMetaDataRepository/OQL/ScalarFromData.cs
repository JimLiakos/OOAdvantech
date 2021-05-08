using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{57376a17-233e-4bdc-aa00-977d22a1d3a8}</MetaDataID>
    [Serializable]
    public class ScalarFromData : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Scalar
    {


        internal override ArithmeticExpression Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as ArithmeticExpression;
            var newScalarFromData = new ScalarFromData(DataNode.Clone(clonedObjects));
            clonedObjects[this] = newScalarFromData;

            return newScalarFromData;
        }


        /// <MetaDataID>{b04cf787-aae9-47d9-ad14-5c9c5ab3c164}</MetaDataID>
        public ScalarFromData(DataNode dataNode)
        {
            DataNode = dataNode;
        }
        /// <MetaDataID>{a10863a9-7599-4921-8bce-437f1b7abf2b}</MetaDataID>
        internal override object CalculateValue(IDataRow[] compositeRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            int dataSourceRowIndex = -1;
            DataNode dataSourceDataNode = DataNode;
            while (dataSourceDataNode.Type == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.OjectAttribute)
                dataSourceDataNode = dataSourceDataNode.ParentDataNode;
            if (dataSourceDataNode is AggregateExpressionDataNode)
                dataSourceDataNode = dataSourceDataNode.ParentDataNode;

            dataSourceRowIndex = dataNodeRowIndices[dataSourceDataNode];
            return compositeRow[dataSourceRowIndex][DataNode.DataSourceColumnIndex];
        }

        /// <MetaDataID>{417ad3b3-6af8-46f7-9808-8df4dd144d9c}</MetaDataID>
        public override System.Collections.Generic.List<DataNode> ArithmeticExpressionDataNodes
        {
            get { return new System.Collections.Generic.List<DataNode>() { DataNode }; }
        }
        /// <MetaDataID>{212e49a2-b9b5-4118-a6a2-7d1606e5a57a}</MetaDataID>
        public override Type ResultType
        {
            get
            {
                if (DataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    return DataNode.Classifier.GetExtensionMetaObject<System.Type>();// .RealParentDataNode.DataSource.DataTable.Columns[DataNode.Name].DataType;
                else if (DataNode is AggregateExpressionDataNode)
                    return (DataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType;
                else 
                    return DataNode.RealParentDataNode.DataSource.DataTable.Columns[DataNode.RealParentDataNode.DataSource.ObjectIndex].DataType.GetType();

            }
        }
        /// <MetaDataID>{23ed4d98-2e88-46d8-b41e-27bfe44875cc}</MetaDataID>
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [Association("", typeof(DataNode), Roles.RoleA, "12c37719-1a05-424c-997c-e356ab3fe763")]
        [IgnoreErrorCheck]
        public DataNode DataNode;
    }
}
