using System; 
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{66381CFA-6DD5-48BE-91AE-376661CFFC42}</MetaDataID>
    [Serializable]
    public class ObjectAttributeComparisonTermA:MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm
	{
		internal ObjectAttributeComparisonTermA(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{

		}
		
		/// <MetaDataID>{20A58FAA-F333-4412-8418-522441345D02}</MetaDataID>
		public override string TranslatedExpression
		{
			get
			{
				string _SQLExpression=null;
				_SQLExpression+=DataNode.ParentDataNode.Alias;
				_SQLExpression+=".";
				_SQLExpression+=DataNode.Name;
				return _SQLExpression;
			}
		}
	}
}
