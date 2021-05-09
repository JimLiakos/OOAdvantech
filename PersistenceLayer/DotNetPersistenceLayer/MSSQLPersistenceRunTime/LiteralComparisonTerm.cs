using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{498B8C40-3E41-43B4-B10A-7EFAF0583695}</MetaDataID>
	[Serializable]
    public class LiteralComparisonTermA:MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm
	{
		internal protected LiteralComparisonTermA(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
		
		}

		/// <MetaDataID>{B598D13B-0A05-4744-BB2B-1ED39F4BA995}</MetaDataID>
		public override string TranslatedExpression
		{
			get
			{
				string _SQLExpression=null;
				if(ComparisonTermParserNode["Literal"]["Date_Literal"]!=null)
				{
					System.DateTime dateTime=System.DateTime.Parse((ComparisonTermParserNode["Literal"]["Date_Literal"] as Parser.ParserNode).Value);
					_SQLExpression="CONVERT(DATETIME, '"+dateTime.Year.ToString()+"-"+
						dateTime.Month.ToString()+"-"+
						dateTime.Day.ToString()+" "+
						dateTime.Hour.ToString()+":"+
						dateTime.Minute.ToString()+":"+
						dateTime.Second.ToString()+"',102)";
					return _SQLExpression;
				}
				else
					return (ComparisonTermParserNode["Literal"] as Parser.ParserNode).Value;
			}		
		}
	}
}
