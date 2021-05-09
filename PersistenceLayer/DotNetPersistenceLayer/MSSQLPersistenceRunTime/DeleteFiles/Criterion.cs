using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{83ECB7AC-FC7E-48C3-B2B3-75C8B1F96F17}</MetaDataID>
    [Serializable]
	public class CriterionA:MetaDataRepository.ObjectQueryLanguage.Criterion
	{
		internal CriterionA(Parser.ParserNode criterionParserNode, MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement,bool constrainCriterion):
            base(criterionParserNode,oqlStatement,constrainCriterion)
		{
            string sqlExpression = SQLExpression;

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C12CA69C-7AF2-4387-91EB-3D627F5D84DC}</MetaDataID>
		private string _SQLExpression;
		/// <MetaDataID>{A5D84A28-F5E2-40AF-8B54-2EBACC3D5D3B}</MetaDataID>
		public string SQLExpression
		{
			get
			{
                if (_SQLExpression != null)
                    return _SQLExpression;

				Parser.ParserNode Criterion=ParserNode;
				Parser.ParserNode ComparisonOperator=Criterion.ChildNodes.GetAt(2);
				if(ComparisonOperator.Value=="=")
				{
                    _SQLExpression=_ComparisonTerms[0].GetCompareExpression(ComparisonType.Equal,_ComparisonTerms[1]);
                    return _SQLExpression;
				}
				else if (ComparisonOperator.Value=="<>")
				{
                    _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.NotEqual, _ComparisonTerms[1]);
                    return _SQLExpression;
				}
				else if (ComparisonOperator.Value==">")
				{
					_SQLExpression=_ComparisonTerms[0].GetCompareExpression(ComparisonType.GreaterThan,_ComparisonTerms[1]);
                    return _SQLExpression;

				}
				else if (ComparisonOperator.Value=="<")
				{
                    _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.LessThan, _ComparisonTerms[1]);
                    return _SQLExpression;
				}
				return "";
			}
		}
        ///// <MetaDataID>{34AFAE9C-5DBD-4EEC-AB11-7FAC31C89FFF}</MetaDataID>
        ///// <summary>Check if data node participates in search condition. </summary>
        //public bool HasSQLExpressionFor(DataNode dataNode)
        //{
			
        //    if(_ComparisonTerms[0] is ObjectAttributeComparisonTerm&&!(_ComparisonTerms[1] is ObjectAttributeComparisonTerm))
        //    {
        //        if((_ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode==dataNode)
        //            return true;
        //        else
        //            return false;
        //    }
        //    else if(_ComparisonTerms[1] is ObjectAttributeComparisonTerm&&!(_ComparisonTerms[0] is ObjectAttributeComparisonTerm))
        //    {
        //        if((_ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode==dataNode)
        //            return true;
        //        else
        //            return false;
        //    }

        //    return false;
        //}
		
        ///// <summary>Return a search condition SQL expression which refers to data node. 
        ///// It is useful when the query refer to more than one data bases. 
        ///// There is case where a data node maybe refers to data in remote table. 
        ///// If you have a filter for data node data at search condition the system will 
        ///// get all data from remote table and then it will filter data. 
        ///// But it is better to filter data at remote data base and then get in main data base. </summary>
        ///// <MetaDataID>{55615162-555C-4EA9-A08B-472577BD2F8C}</MetaDataID>
        //public string GetSQLExpressionFor(DataNode dataNode)
        //{
        //    if(!HasSQLExpressionFor(dataNode))
        //        return "";
        //    else
        //        return SQLExpression;
        //}
	}
}
