using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{7C5EE268-5941-44CC-B55E-C621DEAD3C6A}</MetaDataID>
    [Serializable]
	public class SearchCondition:MetaDataRepository.ObjectQueryLanguage.SearchCondition
	{
		public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
		{
			if(_SearchTerms.Length!=1)
				return null;
			else
				return (_SearchTerms[0] as SearchTerm).GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
		}

        protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchTerm CreateSearchTerm(Parser.ParserNode searchTermsParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainCondition)
		{
			return new SearchTerm(searchTermsParserNode,oqlStatement,constrainCondition);
		}

        internal SearchCondition(Parser.ParserNode searchConditionParserNode, MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainCondition)
            : base(searchConditionParserNode, oqlStatement, constrainCondition)
		{

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{F6C07344-6262-4472-B6DD-A52AFEB4BF39}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formula. 
		/// (SearchTerm.SQLExpression OR SearchTerm.SQLExpression OR …. n) </summary>
		/// <MetaDataID>{BEA2BEE4-44BC-4BA3-896B-A44CAD6EA1F1}</MetaDataID>
		public string SQLExpression
		{
			get
			{
				string sqlExpresion=null;
				foreach(SearchTerm searchTerm in _SearchTerms)
				{
					if(sqlExpresion==null)
						sqlExpresion="("+searchTerm.SQLExpression;
					else
						sqlExpresion+=" OR "+searchTerm.SQLExpression;
				}
				sqlExpresion+=")";

				return sqlExpresion;
			}		
		}
        ///// <MetaDataID>{8C3F92F9-1286-4ADF-90DE-10671BE07BE0}</MetaDataID>
        ///// <summary>Check if data node participates in search condition. </summary>
        //public bool HasSQLExpressionFor(DataNode dataNode)
        //{
        //    foreach(SearchTerm searchTerm in _SearchTerms)
        //    {
        //        if(!searchTerm.HasSQLExpressionFor(dataNode))
        //            return false;
        //    }
        //    return true;
        //}
        ///// <MetaDataID>{01D34F24-7109-442F-BA0E-ACF87C170643}</MetaDataID>
        ///// <summary>Return a search condition SQL expression which refers to data node. 
        ///// It is useful when the query refer to more than one data bases. 
        ///// There is case where a data node maybe refers to data in remote table. 
        ///// If you have a filter for data node data at search condition the system will 
        ///// get all data from remote table and then it will filter data. 
        ///// But it is better to filter data at remote data base and then get in main data base. </summary>
        //public string GetSQLExpressionFor(DataNode dataNode)
        //{
        //    if(HasSQLExpressionFor(dataNode))
        //    {
        //        string sqlExpresion=null;
        //        foreach(SearchTerm searchTerm in _SearchTerms)
        //        {
        //            if(sqlExpresion==null)
        //                sqlExpresion="("+searchTerm.GetSQLExpressionFor(dataNode);
        //            else
        //                sqlExpresion+=" OR "+searchTerm.GetSQLExpressionFor(dataNode);
        //        }
        //        sqlExpresion+=")";

        //        return sqlExpresion;
        //    }
        //    else
        //        return "";
        //}
	}
}
