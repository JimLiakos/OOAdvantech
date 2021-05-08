using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{DCF3A519-D2DE-496C-8EAD-6A0CAB64D034}</MetaDataID>
    [Serializable]
    public class ParameterComparisonTerm : ComparisonTerm
    {

        ParameterComparisonTerm():base(null)
        {

        }
        internal override ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as ParameterComparisonTerm;

            var newParameterComparisonTerm= new ParameterComparisonTerm();
            newParameterComparisonTerm._ParameterName = _ParameterName;
            clonedObjects[this] =newParameterComparisonTerm;
            return newParameterComparisonTerm;
        }
        /// <MetaDataID>{3818C56F-DEB2-44AB-A99A-FB32D5177786}</MetaDataID>
        public override System.Type ValueType
        {
            get
            {
                if (ParameterValue == null)
                    return typeof(void);
                return ParameterValue.GetType(); ;
            }
        }
        /// <MetaDataID>{5689f6c4-6b58-4d18-89fd-0ba0543ba6b5}</MetaDataID>
        string _ParameterName=null;
        /// <MetaDataID>{E1EF55D5-1155-4900-BAC1-F86AF20C9E15}</MetaDataID>
        public string ParameterName
        {
            get
            {
                if (_ParameterName == null)
                {
                    _ParameterName = (ComparisonTermParserNode["Parameter"] as Parser.ParserNode).Value;
                    _ParameterName = _ParameterName.Trim();
                    if (_ParameterName[0] == '@')
                        _ParameterName = _ParameterName.Substring(1);
                    _ParameterName = _ParameterName.Trim();
                    if (_ParameterName[0] == '[')
                        _ParameterName = _ParameterName.Substring(1);
                    if (_ParameterName[_ParameterName.Length - 1] == ']')
                        _ParameterName = _ParameterName.Substring(0, _ParameterName.Length - 1);
                    _ParameterName = '@' + _ParameterName;
                }
                return _ParameterName;
            }

        }
        /// <MetaDataID>{0d2352e3-cf4c-4404-b4e2-6aaf7143591b}</MetaDataID>
        [NonSerialized]
        object _ParameterValue;
        /// <MetaDataID>{0708E8A7-0306-4E41-9A4D-6A36B1668EB7}</MetaDataID>
        public object ParameterValue
        {
            get
            {
                if (_ParameterValue == null && OQLStatement!=null)
                    _ParameterValue = OQLStatement.Parameters[ParameterName];
                return _ParameterValue;
                //return OQLStatement.Parameters[ParameterName];
            }
        }

        /// <MetaDataID>{9560B8F4-DC68-4915-BB8A-630B4227BAFA}</MetaDataID>
        public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
        {
            if (!(theOtherComparisonTerm is ObjectAttributeComparisonTerm || theOtherComparisonTerm is ObjectComparisonTerm))
                throw new System.Exception("Error at " + ComparisonTermParserNode.ParentNode.Value);
            if (comparisonType == Criterion.ComparisonType.GreaterThan)
                return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.LessThan, this);
            if (comparisonType == Criterion.ComparisonType.LessThan)
                return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.GreaterThan, this);
            return theOtherComparisonTerm.GetCompareExpression(comparisonType, this);
        }
        /// <MetaDataID>{F97577C3-5D18-4565-8271-7F71601D9C20}</MetaDataID>
        internal protected ParameterComparisonTerm(Parser.ParserNode ComparisonTermParserNode, ObjectsContextQuery oqlStatement)
            : base(ComparisonTermParserNode, oqlStatement)
        {

            if (!oqlStatement.InParseMode && !oqlStatement.Parameters.ContainsKey(ParameterName))
                throw new System.ArgumentException("There isn't value for " + ParameterName, ParameterName);
            //_ParameterValue = oqlStatement.Parameters[ParameterName];



        }

        /// <MetaDataID>{7c77e477-13a5-4dea-b10c-81d39e15bcc8}</MetaDataID>
        public ParameterComparisonTerm(string parameterName, ObjectQuery oqlStatement)
            : base(oqlStatement)
        {
            _ParameterName = parameterName;
            //_ParameterValue = oqlStatement.Parameters[ParameterName];

        }
        /// <MetaDataID>{132cdea9-4949-4241-838c-573c265e88ba}</MetaDataID>
        public override string ToString()
        {
            if (this.ParameterValue != null)
            {
                if (ParameterValue.GetType().GetMetaData().IsPrimitive)
                    return this.ParameterValue.ToString();
                else if (this.ParameterValue is string)
                    return "'" + this.ParameterValue.ToString() + "'";
                else if (this.ParameterValue is Type)
                    return (this.ParameterValue as Type).FullName;
                else
                    return this.ParameterValue.GetType().ToString();
            }
            else
                return "null";
            
        }
    }
}
