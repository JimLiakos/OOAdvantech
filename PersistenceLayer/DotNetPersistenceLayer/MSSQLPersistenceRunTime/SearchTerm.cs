using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{848999B0-9058-41D5-A120-BBF73DA8B1C1}</MetaDataID>
    [Serializable]
    public class SearchTerm:MetaDataRepository.ObjectQueryLanguage.SearchTerm
	{
      
		
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

        protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchFactor CreateSearchFactor(Parser.ParserNode searchFactorParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainFactor)
		{
			return new SearchFactor(searchFactorParserNode,oqlStatement,constrainFactor);
		}

        internal SearchTerm(Parser.ParserNode searchTermParserNode, MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainTerm)
            : base(searchTermParserNode, oqlStatement, constrainTerm)
		{
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{553D6D42-7207-4E18-8159-FC781FE49219}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formula. 
		/// SearchFactor.SQLExpression AND SearchFactor.SQLExpression   AND…. n </summary>
		/// <MetaDataID>{87C4DC30-6E4C-4F50-82B1-407C6E2A10FD}</MetaDataID>
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
        ///// <MetaDataID>{EE5BA69B-1482-4A1C-8264-6B848714ABAD}</MetaDataID>
        ///// <summary>Check if data node participates in search condition. </summary>
        //public bool HasSQLExpressionFor(DataNode dataNode)
        //{
        //    foreach(SearchFactor searchFactor in _SearchFactors)
        //    {
        //        if(searchFactor.HasSQLExpressionFor(dataNode))
        //            return true;
        //    }
        //    return false;
        //}
        ///// <MetaDataID>{03DB5267-C628-4435-B8BD-A61241355600}</MetaDataID>
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
        //        foreach(SearchFactor searchFactor in _SearchFactors)
        //        {
        //            if(!searchFactor.HasSQLExpressionFor(dataNode))
        //                continue;
        //            if(sqlExpresion==null)
        //                sqlExpresion=searchFactor.GetSQLExpressionFor(dataNode);
        //            else
        //                sqlExpresion+=" AND "+searchFactor.GetSQLExpressionFor(dataNode);
        //        }
        //        return sqlExpresion;
        //    }
        //    else
        //        return "";
        //}

	}
}
