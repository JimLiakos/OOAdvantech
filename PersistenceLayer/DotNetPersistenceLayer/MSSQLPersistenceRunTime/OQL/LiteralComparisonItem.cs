namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{FDA8A3BE-948E-486F-8309-2C9CB165121C}</MetaDataID>
	public class LiteralComparisonTerm : ComparisonTerm
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{0C28FC9D-6ADA-4E99-A7E1-7FA6843789CC}</MetaDataID>
		private System.Type _ValueType;
		/// <MetaDataID>{78AC7AEE-F3AC-4323-B991-58A04118DF7A}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{
				if(_ValueType!=null)
					return _ValueType;

				if(ComparisonTermParserNode["literal"]["date_literal"]!=null)
					_ValueType= typeof(System.DateTime);
				else if(ComparisonTermParserNode["literal"]["numeric_literal"]!=null)
					_ValueType= typeof(System.Int16);
				else if(ComparisonTermParserNode["literal"]["float_numeric_literal"]!=null)
					_ValueType= typeof(System.Double);
				else if(ComparisonTermParserNode["literal"]["quotedstring"]!=null)
                    _ValueType= typeof(System.String);
				else if(ComparisonTermParserNode["literal"]["single_quotedstring"]!=null)
					_ValueType=typeof(System.String);
								
				return _ValueType;			
			}
		}
//		/// <MetaDataID>{5BF48D72-2FC8-488B-AFD4-B6A40A46370A}</MetaDataID>
//		public override System.Type ValueType
//		{
//			get
//			{
//				if(ComparisonTermParserNode["literal"]["date_literal"]!=null)
//					return typeof(System.DateTime);
//
//				if(ComparisonTermParserNode["literal"]["numeric_literal"]!=null)
//					return typeof(System.Int16);
//
//				if(ComparisonTermParserNode["literal"]["float_numeric_literal"]!=null)
//					return typeof(System.Double);
//
//				if(ComparisonTermParserNode["literal"]["quotedstring"]!=null)
//					return typeof(System.String);
//
//				if(ComparisonTermParserNode["literal"]["single_quotedstring"]!=null)
//					return typeof(System.String);
//				
//				return null;
//			}
//		}
		/// <MetaDataID>{0650C502-771A-4317-BA05-85E2318D808C}</MetaDataID>
		public override string SQLExpression
		{
			get
			{
				string _SQLExpression=null;
				if(ComparisonTermParserNode["literal"]["date_literal"]!=null)
				{
					System.DateTime dateTime=System.DateTime.Parse((ComparisonTermParserNode["literal"]["date_literal"] as Parser.ParserNode).Value);
					_SQLExpression="CONVERT(DATETIME, '"+dateTime.Year.ToString()+"-"+
						dateTime.Month.ToString()+"-"+
						dateTime.Day.ToString()+" "+
						dateTime.Hour.ToString()+":"+
						dateTime.Minute.ToString()+":"+
						dateTime.Second.ToString()+"',102)";
					return _SQLExpression;
				}
				else
					return (ComparisonTermParserNode["literal"] as Parser.ParserNode).Value;
			}
		}
		/// <MetaDataID>{ABBA70C5-6220-4213-8B3A-9D84747B7784}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			if(!(theOtherComparisonTerm is ObjectAttributeComparisonTerm))
				throw new System.Exception("Error at "+ComparisonTermParserNode.ParentNode.Value);
			if(comparisonType==Criterion.ComparisonType.GreaterThan)
				return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.LessThan,this);

			if(comparisonType==Criterion.ComparisonType.LessThan)
				return theOtherComparisonTerm.GetCompareExpression(Criterion.ComparisonType.GreaterThan,this);
			return theOtherComparisonTerm.GetCompareExpression(comparisonType,this);
		}
		/// <MetaDataID>{22F23D1F-899B-444A-8AB5-A8F7F1BE1AA2}</MetaDataID>
		 internal LiteralComparisonTerm(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
		
		}
	}
}
