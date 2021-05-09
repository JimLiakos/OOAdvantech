namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{DCF3A519-D2DE-496C-8EAD-6A0CAB64D034}</MetaDataID>
	public class ParameterComparisonTerm : ComparisonTerm
	{
		/// <MetaDataID>{3818C56F-DEB2-44AB-A99A-FB32D5177786}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{
				return ParameterValue.GetType();;
			}
		}
		/// <MetaDataID>{E1EF55D5-1155-4900-BAC1-F86AF20C9E15}</MetaDataID>
		public string ParameterName
		{
			get
			{
				return (ComparisonTermParserNode["parameter"] as Parser.ParserNode).Value;;
			}

		}
		/// <MetaDataID>{0708E8A7-0306-4E41-9A4D-6A36B1668EB7}</MetaDataID>
		public object ParameterValue
		{
			get
			{
				return OQLStatement.Parameters[ParameterName];
			}
		}
		/// <MetaDataID>{F705D9A2-A726-48A3-911E-BFA087DE1B01}</MetaDataID>
		public override string SQLExpression
		{
			get
			{
				//TODO Type check;
				string _SQLExpression=null;

				object parameterValue=ParameterValue;
				if(parameterValue==null)
					_SQLExpression+="IS NULL";//Error prone èá âãáëëåé  ×××××.×××× = IS NULL 
				else
				{
					//if(parameterValue.GetType()!=attributeType)
					//	throw new System.ArgumentException("The parameter with name "+parameterName+" has invalid type. Must be "+attributeType.FullName+".",parameterName);
					_SQLExpression+=TypeDictionary.ConvertToSQLString(parameterValue);
				}

				return _SQLExpression;
			}
		}
		/// <MetaDataID>{9560B8F4-DC68-4915-BB8A-630B4227BAFA}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			if(!(theOtherComparisonTerm is ObjectAttributeComparisonTerm||theOtherComparisonTerm is ObjectComparisonTerm))
				throw new System.Exception("Error at "+ComparisonTermParserNode.ParentNode.Value);
			if(comparisonType==Criterion.ComparisonType.GreaterThan)
				return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.LessThan,this);
			if(comparisonType==Criterion.ComparisonType.LessThan)
				return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.GreaterThan,this);
			return theOtherComparisonTerm.GetCompareExpression(comparisonType,this);
		}
		/// <MetaDataID>{F97577C3-5D18-4565-8271-7F71601D9C20}</MetaDataID>
		 internal ParameterComparisonTerm(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
			 
			 if(!OQLStatement.Parameters.Contains(ParameterName))
				 throw new System.ArgumentException("There isn't value for "+ParameterName,ParameterName);

		
		}
	}
}
