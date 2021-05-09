using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{DE14A6BC-5027-4952-875A-CFD89C306CEC}</MetaDataID>
    [Serializable]
    public class ObjectComparisonTermA:MetaDataRepository.ObjectQueryLanguage.ObjectComparisonTerm
	{

		internal  ObjectComparisonTermA(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{

		}

		
		/// <MetaDataID>{D95E078B-725A-4A36-84D5-3A926F525E16}</MetaDataID>
		public RDBMSMetaDataRepository.StorageCell GetStorageCellFromParameterValue(object parameterValue)
		{
			StorageInstanceRef storageInstanceRef=null;
			if(parameterValue!=null)
			{
				if(!ModulePublisher.ClassRepository.GetType((DataNode.Classifier as MetaDataRepository.MetaObject).FullName,"").IsInstanceOfType(parameterValue))
					throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				try
				{
					storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
					if(storageInstanceRef==null)
						return null;
					else
						return storageInstanceRef.StorageInstanceSet;

				}
				catch
				{
				}
			}
			return null;

		}
		
		/// <MetaDataID>{7DF16EB8-5968-4359-B239-F62F6FFF7729}</MetaDataID>
		public RDBMSMetaDataRepository.StorageCell GetStorageCellFromObjectID(Parser.ParserNode objectIDParserNode)
		{
			if(objectIDParserNode==null)
				throw new System.Exception("Can't retrieve Storage Cell for null objectID");
			int Count=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.Count;
			for(int i=0;i!=Count;i++)
			{
				Parser.ParserNode ObjectIDField=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(i+1);
				string  ObjectIDFieldName=ObjectIDField.ChildNodes.GetAt(1).Value;
				if(ObjectIDFieldName=="StorageCellID")
				{
					string ObjCellID=ObjectIDField.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					return (DataNode.Classifier as RDBMSMetaDataRepository.Class).GetStorageCell(int.Parse(ObjCellID));
				}
			}
			return null;
		}

		
		/// <MetaDataID>{7735C2F9-F4BD-40D8-AEF6-EE0E89B8275F}</MetaDataID>
		public override string GetCompareExpression(MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType comparisonType, MetaDataRepository.ObjectQueryLanguage.ComparisonTerm theOtherComparisonTerm)
		{
			string compareExpression=null;
			string comparisonOperator=null;
            if (comparisonType == MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType.Equal)
				comparisonOperator="=";
			else
				comparisonOperator="<>";

            if (theOtherComparisonTerm is MetaDataRepository.ObjectQueryLanguage.ObjectComparisonTerm)
			{
				#region Build comparison expresion with ObjectComparisonTerm
				DataNode firstTermDataNode=(DataNode)OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]];
				DataNode secondTermDataNode=(DataNode)OQLStatement.PathDataNodeMap[theOtherComparisonTerm.ComparisonTermParserNode["Path"]];
				MetaDataRepository.MetaObjectCollection FirstTermColumns=firstTermDataNode.Classifier.ObjectIDColumns;
				MetaDataRepository.MetaObjectCollection SecondTermColumns=secondTermDataNode.Classifier.ObjectIDColumns;
				foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstTermColumns)
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
					{
						if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
						{							
							if(compareExpression!=null)
							{
								if(comparisonType==Criterion.ComparisonType.Equal) //error prone
									compareExpression+=" AND ";
								else
									compareExpression+=" OR ";
							}
							else
								compareExpression+="(";
							compareExpression+=firstTermDataNode.Alias+"."+CurrColumn.Name;
							compareExpression+=" "+ comparisonOperator + "  "+secondTermDataNode.Alias;
							compareExpression+="."+CorrespondingCurrColumn.Name;
						}
					}
				}
				#endregion
				return compareExpression+")";
			}
			else if( theOtherComparisonTerm is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm)
			{
				#region Build comparison expresion with ObjectIDComparisonTerm
				DataNode firstTermDataNode=(DataNode)OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]];
				foreach(RDBMSMetaDataRepository.IdentityColumn column in firstTermDataNode.Classifier.ObjectIDColumns)
				{
					foreach(Parser.ParserNode objectIDField in (theOtherComparisonTerm.ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"] as Parser.ParserNode).ChildNodes)
					{
						if((objectIDField["Name"] as Parser.ParserNode).Value==column.ColumnType)
						{							
							if(compareExpression!=null)
							{
								if(comparisonType==Criterion.ComparisonType.Equal) //error prone
									compareExpression+=" AND ";
								else
									compareExpression+=" OR ";
							}
							else
								compareExpression+="(";
							compareExpression+=firstTermDataNode.Alias+"."+column.Name;
                            if (objectIDField["Literal"]["SingleQuatedStringLiteral"] != null)
                                compareExpression += " " + comparisonOperator + "  " + (objectIDField["Literal"]["SingleQuatedStringLiteral"] as Parser.ParserNode).Value;
                            if (objectIDField["Literal"]["NumericLiteral"] != null)
                                compareExpression += " " + comparisonOperator + "  " + (objectIDField["Literal"]["NumericLiteral"] as Parser.ParserNode).Value;
						}
					}
				}
				#endregion
				return compareExpression+")";
			}
            else if (theOtherComparisonTerm is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
			{
				#region Build comparison expresion with ParameterComparisonTerm
				DataNode firstTermDataNode=(DataNode)OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]];
				ObjectID objectID=null;
				StorageInstanceRef storageInstanceRef=null;
				object parameterValue=(theOtherComparisonTerm as ParameterComparisonTerm).ParameterValue;
				if(parameterValue!=null)
				{
					if(!ModulePublisher.ClassRepository.GetType((firstTermDataNode.Classifier as MetaDataRepository.MetaObject).FullName,"").IsInstanceOfType(parameterValue))
						throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
					try
					{
						storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
						if(storageInstanceRef==null)
							objectID=new ObjectID(System.Guid.NewGuid(),0);
					}
					catch
					{
						
						objectID=new ObjectID(System.Guid.NewGuid(),0);
					}
					if(storageInstanceRef!=null)
						objectID=storageInstanceRef.ObjectID as ObjectID;
				}
				
				foreach(RDBMSMetaDataRepository.IdentityColumn column in firstTermDataNode.Classifier.ObjectIDColumns)
				{
					if(compareExpression!=null)
					{
						if(comparisonType==Criterion.ComparisonType.Equal) //error prone
							compareExpression+=" AND ";
						else
							compareExpression+=" OR ";
					}
					else
						compareExpression+="(";
					compareExpression+=firstTermDataNode.Alias+"."+column.Name;
					if(objectID==null)
						compareExpression+=" IS NULL ";
					else
						compareExpression+=" "+ comparisonOperator+ "  '"+ objectID.GetMemberValue(column.ColumnType).ToString()+"'";
				}
				#endregion

				return compareExpression+")";
			}
			else
				throw new System.Exception("Comparison error "+ComparisonTermParserNode.ParentNode.Value+" .");
		}

	}
}
