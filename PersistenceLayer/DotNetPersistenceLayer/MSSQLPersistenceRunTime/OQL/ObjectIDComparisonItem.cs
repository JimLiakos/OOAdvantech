namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{A21E418D-7C92-485B-BDF9-0E4A4212458F}</MetaDataID>
	public class ObjectIDComparisonTerm : ComparisonTerm
	{
		/// <MetaDataID>{10B302B7-9B05-4036-BCD8-0138474F6CF8}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{
				return null;
			}
		}
		/// <MetaDataID>{08D584C4-25BA-460E-A2C4-4083F599CAE3}</MetaDataID>
		public override string  GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			if(theOtherComparisonTerm is ObjectComparisonTerm)
				return theOtherComparisonTerm.GetCompareExpression(comparisonType,this);
			else
				throw new System.Exception("You can't compare ObjectID with other than Object");
		}
		/// <MetaDataID>{0B83166F-128D-4504-8BFE-D9DAAA71DBE7}</MetaDataID>
		 internal ObjectIDComparisonTerm(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
		
		}
	}
}
