namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{50014F55-97F4-4AA3-A67F-9D59356AFE85}</MetaDataID>
	/// <summary>Search condition is a Boolean expression where the result can be true or false. Search condition contains search terms. Search term is also a Boolean expression. Search condition is true if any of search terms which contains is true. </summary>
	public class SearchCondition
	{
		/// <MetaDataID>{7A272245-7ADE-434F-B2B6-51AFB31D271F}</MetaDataID>
		public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
		{
			if(_SearchTerms.Length!=1)
				return null;
			else
				return _SearchTerms[0].GetObjectIDDataNodeConstrain(dataNodeTreeHeader);

		}
		/// <MetaDataID>{46C3FC63-ABCC-42C0-A78B-9DEEA47AFE4D}</MetaDataID>
		 internal SearchCondition(Parser.ParserNode searchConditionParserNode, OQLStatement oqlStatement)
		{

			if(searchConditionParserNode==null||searchConditionParserNode["search_term"]==null)
				throw new System.Exception("There is Search Condition without search terms");


			if(searchConditionParserNode["search_term"] is Parser.ParserNode)
				_SearchTerms=new SearchTerm[1];
			else
				_SearchTerms=new SearchTerm[(searchConditionParserNode["search_term"] as Parser.ParserNodeCollection).Count];

			int i=0;
			foreach(Parser.ParserNode searchTermsParserNode  in searchConditionParserNode.ChildNodes)
			{
				if(searchTermsParserNode.Name!="search_term")
					continue;
				_SearchTerms[i]=new SearchTerm(searchTermsParserNode,oqlStatement);
				i++;
			}

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B2E3B085-0CFB-4283-9F0B-B9CACBEE7901}</MetaDataID>
		private OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage.SearchTerm[] _SearchTerms;
		/// <MetaDataID>{148421CE-FCDD-4A3B-AE9D-CC9DC5E921DB}</MetaDataID>
		public SearchTerm[] SearchTerms
		{
			get
			{
				return _SearchTerms.Clone()as SearchTerm[];
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B06D5465-E59C-4957-8DE7-7FE53B71479A}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formula. 
		/// (SearchTerm.SQLExpression OR SearchTerm.SQLExpression OR …. n) </summary>
		/// <MetaDataID>{FBA14885-EBCD-40F2-A60D-D1DAEBC371A9}</MetaDataID>
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

		/// <summary>Return a search condition SQL expression which refers to data node. 
		/// It is useful when the query refer to more than one data bases. 
		/// There is case where a data node maybe refers to data in remote table. 
		/// If you have a filter for data node data at search condition the system will 
		/// get all data from remote table and then it will filter data. 
		/// But it is better to filter data at remote data base and then get in main data base. </summary>
		/// <param name="dataNode"></param>
		/// <returns></returns>
		/// <MetaDataID>{CBF51DD5-A383-417F-A499-DFC35E97AC41}</MetaDataID>
		public string GetSQLExpressionFor(DataNode dataNode)
		{
			if(HasSQLExpressionFor(dataNode))
			{
				string sqlExpresion=null;
				foreach(SearchTerm searchTerm in _SearchTerms)
				{
					if(sqlExpresion==null)
						sqlExpresion="("+searchTerm.GetSQLExpressionFor(dataNode);
					else
						sqlExpresion+=" OR "+searchTerm.GetSQLExpressionFor(dataNode);
				}
				sqlExpresion+=")";

				return sqlExpresion;
			}
			else
				return "";

		}
		/// <summary>Check if data node participates in search condition. </summary>
		/// <param name="dataNode"></param>
		/// <returns></returns>
		/// <MetaDataID>{5543C16F-F866-43B3-BB2C-EA05D803A9BA}</MetaDataID>
		public bool HasSQLExpressionFor(DataNode dataNode)
		{
			foreach(SearchTerm searchTerm in _SearchTerms)
			{
				if(!searchTerm.HasSQLExpressionFor(dataNode))
					return false;
			}
			return true;
		}
	
	
	}
}
