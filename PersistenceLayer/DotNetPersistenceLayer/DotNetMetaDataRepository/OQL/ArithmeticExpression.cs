namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{7b350d8e-b566-47ec-ab04-90b836800299}</MetaDataID>
    /// <summary>
    /// Describes a arithmetic expression
    /// </summary>
    [System.Serializable]
    abstract public class ArithmeticExpression
    {

        internal abstract  ArithmeticExpression Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects);
        /// <MetaDataID>{a2e6af79-e108-4538-ba4a-4e243e2d390f}</MetaDataID>
        /// <summary>
        /// Defines a collection with all DataNodes where the  arithmetic expression uses 
        /// </summary>
        public abstract System.Collections.Generic.List<DataNode> ArithmeticExpressionDataNodes
        {
            get;
        }
        /// <MetaDataID>{a8f65a42-64c8-46dd-8a5d-50c3b56038b5}</MetaDataID>
        /// <summary>
        /// This method takes the scalar values from compositeRow and calculate the arithmetic expression result value 
        /// </summary>
        /// <param name="compositeRow">
        /// The parameter defines a composite row with the data which use the method to calculate the result 
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// This parameter defines the indices of DataNode rows
        /// </param>
        /// <returns>
        /// Returns the result of expresion calculation with the compositeRow data 
        /// </returns>
        internal abstract object CalculateValue(IDataRow[] compositeRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices);

        /// <MetaDataID>{a14e1a60-2779-4e35-9105-c3de08530588}</MetaDataID>
        /// <summary> Defines the type of expression results</summary>
        public abstract System.Type ResultType
        {
            get;
        }

        /// <MetaDataID>{5c24c0c9-a172-423a-9f9d-385874092a6d}</MetaDataID>
        /// <summary>
        /// Determines whether two specified ArithmeticExpression objects have different values.
        /// </summary>
        /// <param name="left">
        /// Defines an ArithmeticExpression or null
        /// </param>
        /// <param name="right">
        /// Defines an ArithmeticExpression or null
        /// </param>
        /// <returns>
        /// true if the value of left is different from the value of right; otherwise, false.
        /// </returns>
        public static bool operator !=(ArithmeticExpression left, ArithmeticExpression right)
        {
            return !(left == right);
        }

        /// <MetaDataID>{af070f4c-16b6-4d79-83d1-38e4bedc1729}</MetaDataID>
        /// <summary>
        /// Determines whether two specified ArithmeticExpression objects have same values.
        /// </summary>
        /// <param name="left">
        /// Defines an ArithmeticExpression or null
        /// </param>
        /// <param name="right">
        /// Defines an ArithmeticExpression or null
        /// </param>
        /// <returns>
        /// true if the value of left is the same as the value of right; otherwise, false.
        /// </returns>
        public static bool operator ==(ArithmeticExpression left, ArithmeticExpression right)
        {
            
            if (object.ReferenceEquals(left , null) && object.ReferenceEquals(right , null))
                return true;
            if (!object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return false;

            if (object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null))
                return false;


            if (left.GetType() != right.GetType())
                return false;
            if (left is ScalarFromLiteral && (left as ScalarFromLiteral).LiteralValue != (right as ScalarFromLiteral).LiteralValue)
                return false;
            if (left is ScalarFromData && (left as ScalarFromData).DataNode != (right as ScalarFromData).DataNode)
                return false;

            if (left is CompositeArithmeticExpression && (left as CompositeArithmeticExpression).Operator != (right as CompositeArithmeticExpression).Operator)
                return false;
            if (left is CompositeArithmeticExpression && (left as CompositeArithmeticExpression).Left != (right as CompositeArithmeticExpression).Left)
                return false;
            if (left is CompositeArithmeticExpression && (left as CompositeArithmeticExpression).Right != (right as CompositeArithmeticExpression).Right)
                return false;
            return true;
        }
    }
}
