using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{4D97FFB1-A307-4865-A0FD-1DCDA0A81B73}</MetaDataID>
    [Serializable]
    public class ParameterComparisonTermA:MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm
	{
		internal ParameterComparisonTermA(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{

		}
		/// <MetaDataID>{F16CA350-5D60-42EB-8BFA-27C6875F4A99}</MetaDataID>
		public override string TranslatedExpression
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
	}
}
