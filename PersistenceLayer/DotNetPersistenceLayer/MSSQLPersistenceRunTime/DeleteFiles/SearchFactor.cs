using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{96754A6C-536D-4C70-B16D-56F801FB4189}</MetaDataID>
    [Serializable]
	public class SearchFactor:MetaDataRepository.ObjectQueryLanguage.SearchFactor
	{
		public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
		{
			if(SearchCondition!=null)
				return (SearchCondition as SearchCondition).GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
			else
			{
				if(Criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm &&Criterion.ComparisonTerms[1]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
					MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm=Criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;

					
					(objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);

					DataNode constrainDataNode= objectComparisonTerm.DataNode as DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;
					
				}
				else if(Criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm &&Criterion.ComparisonTerms[0]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
					MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm=Criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;

					(objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);

					DataNode constrainDataNode= objectComparisonTerm.DataNode as DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;

				}
				else if(Criterion.ComparisonTerms[0] is ParameterComparisonTerm &&Criterion.ComparisonTerms[1]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
					ParameterComparisonTerm parameterComparisonTerm =Criterion.ComparisonTerms[0] as ParameterComparisonTerm;

					
					(objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);

					DataNode constrainDataNode= objectComparisonTerm.DataNode as DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;


				}
				else if(Criterion.ComparisonTerms[1] is ParameterComparisonTerm&&Criterion.ComparisonTerms[0]is ObjectComparisonTerm)
				{
					ObjectComparisonTerm objectComparisonTerm=Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
					ParameterComparisonTerm parameterComparisonTerm =Criterion.ComparisonTerms[1] as ParameterComparisonTerm;
					

					(objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell=objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);

					DataNode constrainDataNode= objectComparisonTerm.DataNode as DataNode;
					if(constrainDataNode.HeaderDataNode==dataNodeTreeHeader)
						return constrainDataNode;
					else
						return null;
				}

			}
			return null;
			
		}

        protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion CreateCriterion(Parser.ParserNode criterionParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainCriterion)
        {
            return new Criterion(criterionParserNode, oqlStatement,constrainCriterion);
        }

        protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchCondition CreateSearchCondition(Parser.ParserNode searchConditionParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainCondition)
		{
			return new SearchCondition(searchConditionParserNode,oqlStatement,constrainCondition);
		}


        internal SearchFactor(Parser.ParserNode searchFactorParserNode, MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement, bool constrainFactor)
            : base(searchFactorParserNode, oqlStatement, constrainFactor)
		{

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{E538008C-B26C-4C3F-9255-F6F2D2D9B10F}</MetaDataID>
		private string _SQLExpression;
		/// <summary>SQLExpression build with the following formulas. 
		/// A.SearchCondition.SQLExpression 
		/// if SearchCondition is not null and Criterion is null
		/// B. Criterion.SQLExpression
		/// if SearchCondition is null and Criterion is null not </summary>
		/// <MetaDataID>{F63440B9-62BF-4453-8E26-CE6E9041CF4E}</MetaDataID>
		public string SQLExpression
		{
			get
			{
				if(SearchCondition!=null)
					return	(SearchCondition as SearchCondition).SQLExpression ;
				else
					return (Criterion as Criterion).SQLExpression;
			}		
		}
        ///// <MetaDataID>{A9860496-AD4E-4ACA-98DF-6F7FD5B2CF23}</MetaDataID>
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
        //        if(SearchCondition!=null)
        //            return (SearchCondition as SearchCondition).GetSQLExpressionFor(dataNode);
        //        else
        //            return (Criterion as Criterion).GetSQLExpressionFor(dataNode);
        //    }
        //    else
        //        return "";

        //}
        ///// <MetaDataID>{5222A58E-3B98-4263-8AC0-7B371E2F0620}</MetaDataID>
        ///// <summary>Check if data node participates in search condition. </summary>
        //public bool HasSQLExpressionFor(DataNode dataNode)
        //{
        //    if(SearchCondition!=null)
        //        return (SearchCondition as SearchCondition).HasSQLExpressionFor(dataNode);
        //    else
        //        return (Criterion as Criterion).HasSQLExpressionFor(dataNode);
        //}
	}
}
