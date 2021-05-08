using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{929cb0cc-025f-46fa-b40a-8af15c534f1a}</MetaDataID>
    [Serializable]
    public class ScalarFromLiteral : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Scalar
    {

        internal override ArithmeticExpression Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as ArithmeticExpression;
            var newScalarFromLiteral = new ScalarFromLiteral(LiteralValue);
            clonedObjects[this] = newScalarFromLiteral;
            return newScalarFromLiteral;
        }

        /// <MetaDataID>{ddb53c63-1bd0-4304-a36c-42324068c9a9}</MetaDataID>
        public override System.Collections.Generic.List<DataNode> ArithmeticExpressionDataNodes
        {
            get { return new System.Collections.Generic.List<DataNode>(); }
        }
        /// <MetaDataID>{75500d96-d274-48ec-a945-9b3f8ba603f4}</MetaDataID>
        public object LiteralValue;
        /// <MetaDataID>{349223a6-6a4d-445c-8edc-2ab3f5272dc3}</MetaDataID>
        public ScalarFromLiteral(object literalValue)
        {
            LiteralValue = literalValue;
        }
        /// <MetaDataID>{8c809c95-a2eb-488f-bb9f-455951725270}</MetaDataID>
        internal override object CalculateValue(IDataRow[] compositeRows, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            return LiteralValue;
        }
        /// <MetaDataID>{ce8c276c-642c-41c3-9ad3-c84ece471a19}</MetaDataID>
        public override Type ResultType
        {
            get { return LiteralValue.GetType(); }
        }
 
    }
}
