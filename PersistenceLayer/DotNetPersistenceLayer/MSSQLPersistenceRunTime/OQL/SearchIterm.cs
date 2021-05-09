namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{7FE09E4E-0EF1-4403-B68F-798EFA5F7CDF}</MetaDataID>
	/// <summary>Define a condition with result true or false. Contain one or more conditions the search factors. The search term can be true only if all factor conditions are true. </summary>
	public class SearchTerm
	{
		/// <MetaDataID>{AFA4A981-C1CC-486B-9203-2E474424F9E5}</MetaDataID>
		public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
		{
			foreach(SearchFactor searchFacor in _SearchFactors)
			{
				DataNode dataNode=searchFacor.GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
				if(dataNode!=null)
					return dataNode;
			}
			return null;
		}
		/// <MetaDataID>{32670678-5391-4BA6-A038-58DFDB1B92C8}</MetaDataID>
		/// <summary>Return a search condition SQL expression which refers to data node. 
		/// It is useful when the query refer to more than one data bases. 
		/// There is case where a data node maybe refers to data in remote table. 
		/// If you have a filter for data node data at search condition the system will 
		/// get all data from remote table and then it will filter data. 
		/// But it is better to filter data at remote data base and then get in main data base. </summary>
		public string GetSQLExpressionFor(DataNode dataNode)
		{
			if(HasSQLExpressionFor(dataNode))
			{
				string sqlExpresion=null;
				foreach(SearchFactor searchFactor in _SearchFactors)
				{
					if(!searchFactor.HasSQLExpressionFor(dataNode))
						continue;
					if(sqlExpresion==null)
						sqlExpresion=searchFactor.GetSQLExpressionFor(dataNode);
					else
						sqlExpresion+=" AND "+searchFactor.GetSQLExpressionFor(dataNode);
				}
				return sqlExpresion;
			}
			else
                return "";
		}
		/// <MetaDataID>{821F6A47-C773-4DC9-A5A9-87347CC2E8A9}</MetaDataID>
		/// <summary>Check if data node participates in search condition. </summary>
		public bool HasSQLExpressionFor(DataNode dataNode)
		{
			foreach(SearchFactor searchFactor in _SearchFactors)
			{
				if(searchFactor.HasSQLExpressionFor(dataNode))
					return true;
			}
			return false;
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{052DB215-851E-4811-A8B3-3A36030392FF}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formula. 
		/// SearchFactor.SQLExpression AND SearchFactor.SQLExpression   AND…. n </summary>
		/// <MetaDataID>{C9BD1E93-67EB-4267-BA59-A941AA19E225}</MetaDataID>
		public string SQLExpression
		{
			get
			{
				string sqlExpresion=null;
				foreach(SearchFactor searchFactor in _SearchFactors)
				{
					if(sqlExpresion==null)
						sqlExpresion=searchFactor.SQLExpression;
					else
						sqlExpresion+=" AND "+searchFactor.SQLExpression;
				}
				return sqlExpresion;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{15C35A77-D2BD-4015-999F-9D093FA93DA0}</MetaDataID>
		private OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage.SearchFactor[] _SearchFactors;
		/// <MetaDataID>{F7E35B1A-CE64-4726-A6D3-4AD74F0553C9}</MetaDataID>
		public SearchFactor[] SearchFactors
		{
			get
			{
				return _SearchFactors.Clone() as SearchFactor[];
			}
		}
		/// <MetaDataID>{61A63579-FA08-4246-BDA7-90F9305C2D47}</MetaDataID>
		 internal SearchTerm(Parser.ParserNode searchTermParserNode, OQLStatement oqlStatement)
		{

			 if(searchTermParserNode==null||searchTermParserNode["search_factor"]==null)
				 throw new System.Exception("There is Search term without search factors");

			 if(searchTermParserNode["search_factor"] is Parser.ParserNode)
				_SearchFactors=new SearchFactor[1];
			 else
				 _SearchFactors=new SearchFactor[(searchTermParserNode["search_factor"] as Parser.ParserNodeCollection).Count];


			int i=0;
			foreach(Parser.ParserNode searchFactorParserNode in searchTermParserNode.ChildNodes)
			{
				if(searchFactorParserNode.Name!="search_factor")
					continue;

				_SearchFactors[i]=new SearchFactor(searchFactorParserNode,oqlStatement);
				i++;
			}

		}
	}
}
