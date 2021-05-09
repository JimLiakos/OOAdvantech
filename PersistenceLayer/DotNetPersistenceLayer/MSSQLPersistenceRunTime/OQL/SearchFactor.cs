namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{73BC1058-17AB-432B-8E33-66C8BCB91E2D}</MetaDataID>
	/// <summary>Define a condition with result true or false. Condition can be a simple comparison expression or a more complex condition, a composite search condition. </summary>
	public class SearchFactor
	{
		/// <MetaDataID>{E4625F30-405B-4BDE-8982-8B0512EEABCC}</MetaDataID>
		public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
		{
			if(SearchCondition!=null)
				return SearchCondition.GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
			else
			{
				if(Criterion.ComparisonTerms[0] is ObjectIDComparisonTerm &&Criterion.ComparisonTerms[1]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
					ObjectIDComparisonTerm objectIDComparisonTerm=Criterion.ComparisonTerms[0] as ObjectIDComparisonTerm;
					objectComparisonTerm.DataNode.ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);
					DataNode constrainDataNode= objectComparisonTerm.DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;
					
				}
				else if(Criterion.ComparisonTerms[1] is ObjectIDComparisonTerm &&Criterion.ComparisonTerms[0]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
					ObjectIDComparisonTerm objectIDComparisonTerm=Criterion.ComparisonTerms[1] as ObjectIDComparisonTerm;
					objectComparisonTerm.DataNode.ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);
					DataNode constrainDataNode= objectComparisonTerm.DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;

				}
				else if(Criterion.ComparisonTerms[0] is ParameterComparisonTerm &&Criterion.ComparisonTerms[1]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
					ParameterComparisonTerm parameterComparisonTerm =Criterion.ComparisonTerms[0] as ParameterComparisonTerm;
					objectComparisonTerm.DataNode.ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);
						DataNode constrainDataNode= objectComparisonTerm.DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;


				}
				else if(Criterion.ComparisonTerms[1] is ParameterComparisonTerm&&Criterion.ComparisonTerms[0]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
					ParameterComparisonTerm parameterComparisonTerm =Criterion.ComparisonTerms[1] as ParameterComparisonTerm;
					objectComparisonTerm.DataNode.ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);
						DataNode constrainDataNode= objectComparisonTerm.DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;
				}

			}
			return null;
			
		}
		/// <MetaDataID>{2ED8C4F4-2ED2-4315-9DD4-1E569BB9EB1D}</MetaDataID>
		/// <summary>Check if data node participates in search condition. </summary>
		public bool HasSQLExpressionFor(DataNode dataNode)
		{
			if(SearchCondition!=null)
				return SearchCondition.HasSQLExpressionFor(dataNode);
			else
				return Criterion.HasSQLExpressionFor(dataNode);
		}
		/// <MetaDataID>{8447DADC-4BCB-4801-8D50-88FC19E342F3}</MetaDataID>
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
				if(SearchCondition!=null)
					return SearchCondition.GetSQLExpressionFor(dataNode);
				else
					return Criterion.GetSQLExpressionFor(dataNode);
			}
			else
				return "";
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{6F21001B-242C-45A8-A26E-F52456CDFB07}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formulas. 
		/// A.SearchCondition.SQLExpression 
		/// if SearchCondition is not null and Criterion is null
		/// B. Criterion.SQLExpression
		/// if SearchCondition is null and Criterion is null not </summary>
		/// <MetaDataID>{9A17AFE9-62E6-4275-8F42-F8897BA286EA}</MetaDataID>
		public string SQLExpression
		{
			get
			{
				if(SearchCondition!=null)
					return	SearchCondition.SQLExpression ;
				else
					return Criterion.SQLExpression;
			}
		}
		/// <MetaDataID>{F69531C9-B4A5-4A5B-A5CF-A3737716D356}</MetaDataID>
		 internal SearchFactor(Parser.ParserNode searchFactorParserNode, OQLStatement oqlStatement)
		{
			Parser.ParserNode searchConditionParserNode=searchFactorParserNode["search_condition"]as Parser.ParserNode;
			if(searchConditionParserNode!=null)
				SearchCondition=new SearchCondition(searchConditionParserNode,oqlStatement);

			Parser.ParserNode criterionParserNode=searchFactorParserNode["Criterion"]as Parser.ParserNode;
			if(criterionParserNode!=null)
				Criterion=new Criterion(criterionParserNode,oqlStatement);


		}
		/// <MetaDataID>{E7CD71E7-FFA0-4806-8CF3-DCFFD3659647}</MetaDataID>
		public Criterion Criterion;
		/// <MetaDataID>{39A19DCB-5419-4D34-A722-87739F883877}</MetaDataID>
		public SearchCondition SearchCondition;
	}
}
