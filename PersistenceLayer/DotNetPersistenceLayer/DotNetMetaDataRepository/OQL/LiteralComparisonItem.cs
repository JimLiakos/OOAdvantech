using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{FDA8A3BE-948E-486F-8309-2C9CB165121C}</MetaDataID>
    [Serializable]
    public class LiteralComparisonTerm : ComparisonTerm
    {


        LiteralComparisonTerm()
            : base(null)
        {

        }

        internal override ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            LiteralComparisonTerm newLiteralComparisonTerm = new LiteralComparisonTerm();
            newLiteralComparisonTerm._ValueType = _ValueType;
            newLiteralComparisonTerm.Value = Value;
            return newLiteralComparisonTerm;
        }


        /// <MetaDataID>{f15e892e-1b07-446a-89a2-55cb5d54c2cc}</MetaDataID>
        public static System.Type GetLiteralValueType(Parser.ParserNode leteral)
        {
            if (leteral["Date_Literal"] != null)
                return typeof(System.DateTime);
            else if (leteral["NumericLiteral"] != null)
                return typeof(System.Int64);
            else if (leteral["RealLiteral"] != null)
                return typeof(System.Double);
            else if (leteral["StringLiteral"] != null)
                return typeof(System.String);
            else if (leteral["SingleQuatedStringLiteral"] != null)
                return typeof(System.String);
            else if (leteral["BooleanLiteral"] != null)
                return typeof(bool);
            else if (leteral["NULL"] != null)
                return typeof(System.DBNull);


            throw new System.Exception("Unknown Type");

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0C28FC9D-6ADA-4E99-A7E1-7FA6843789CC}</MetaDataID>
        private System.Type _ValueType;
        /// <MetaDataID>{78AC7AEE-F3AC-4323-B991-58A04118DF7A}</MetaDataID>
        public override System.Type ValueType
        {
            get
            {
                if (_ValueType != null)
                    return _ValueType;

                _ValueType = GetLiteralValueType(ComparisonTermParserNode["Literal"] as Parser.ParserNode);

                //if (ComparisonTermParserNode["Literal"]["Date_Literal"] != null)
                //    _ValueType = typeof(System.DateTime);
                //else if (ComparisonTermParserNode["Literal"]["NumericLiteral"] != null)
                //    _ValueType = typeof(System.Int64);
                //else if (ComparisonTermParserNode["Literal"]["RealLiteral"] != null)
                //    _ValueType = typeof(System.Double);
                //else if (ComparisonTermParserNode["Literal"]["StringLiteral"] != null)
                //    _ValueType = typeof(System.String);
                //else if (ComparisonTermParserNode["Literal"]["SingleQuatedStringLiteral"] != null)
                //    _ValueType = typeof(System.String);
                //else if (ComparisonTermParserNode["Literal"]["BooleanLiteral"] != null)
                //    _ValueType = typeof(bool);
                     

                return _ValueType;
            }
        }

        /// <MetaDataID>{BBFD1D1D-215F-4AE4-B908-36519B887F55}</MetaDataID>
        public object Value;

        /// <MetaDataID>{ABBA70C5-6220-4213-8B3A-9D84747B7784}</MetaDataID>
        public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
        {
            if (!(theOtherComparisonTerm is ObjectAttributeComparisonTerm))
                throw new System.Exception("Error at " + ComparisonTermParserNode.ParentNode.Value);
            if (comparisonType == Criterion.ComparisonType.GreaterThan)
                return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.LessThan, this);

            if (comparisonType == Criterion.ComparisonType.LessThan)
                return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.GreaterThan, this);
            return theOtherComparisonTerm.GetCompareExpression(comparisonType, this);
        }
        /// <MetaDataID>{3b685c65-285d-4ab9-abe5-189bca6a67c1}</MetaDataID>
        public static object GetLiteralValue(Parser.ParserNode leteral)
        {
            System.Type valueType = GetLiteralValueType(leteral);
            if (valueType == typeof(DateTime))
                return System.DateTime.Parse(leteral.Value);
            else
            {
                if (valueType == typeof(string))
                {
                    string strValue = leteral.Value;
                    return strValue.Substring(1, strValue.Length - 2);
                }
                else
                {
#if DeviceDotNet
                    if (valueType == typeof(bool))
                        return System.Convert.ChangeType(leteral.Value.ToLower(), valueType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                    else
                        return System.Convert.ChangeType(leteral.Value, valueType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat); 
#else
                    if (valueType == typeof(bool))
                        return System.Convert.ChangeType(leteral.Value.ToLower(), valueType);
                    else
                    {
                        if (leteral["NULL"] != null)
                            return null;
                        else
                            return System.Convert.ChangeType(leteral.Value, valueType);
                    }
                    return null;

#endif
                }
            }
        }

        /// <MetaDataID>{22F23D1F-899B-444A-8AB5-A8F7F1BE1AA2}</MetaDataID>
        internal protected LiteralComparisonTerm(Parser.ParserNode ComparisonTermParserNode, ObjectsContextQuery oqlStatement)
            : base(ComparisonTermParserNode, oqlStatement)
        {
            Value = GetLiteralValue(ComparisonTermParserNode["Literal"] as Parser.ParserNode);
            //if (ValueType == typeof(DateTime))
            //    Value = System.DateTime.Parse((ComparisonTermParserNode["Literal"]["Date_Literal"] as Parser.ParserNode).Value);
            //else
            //{
            //    if (ValueType == typeof(string))
            //    {
            //        string strValue = (ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value;
            //        Value = strValue.Substring(1, strValue.Length - 2);
            //    }
            //    else
            //    {
            //        #if DeviceDotNet
            //        if(ValueType==typeof(bool))
            //            Value = System.Convert.ChangeType((ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value.ToLower(), ValueType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
            //        else
            //            Value = System.Convert.ChangeType((ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value, ValueType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat); 
            //        #else
            //        if (ValueType == typeof(bool))
            //            Value = System.Convert.ChangeType((ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value.ToLower(), ValueType);
            //        else
            //        {
            //            if (ComparisonTermParserNode["Literal"]["NULL"] != null)
            //                Value = null;
            //            else 
            //                Value = System.Convert.ChangeType((ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value, ValueType);
            //        }

            //        #endif
            //    }
            //}
        }
    }
}

