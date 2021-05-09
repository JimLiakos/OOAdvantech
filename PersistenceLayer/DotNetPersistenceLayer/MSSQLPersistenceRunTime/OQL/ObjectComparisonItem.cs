namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{96984B3C-6EFF-441C-BF78-B258398EC5EE}</MetaDataID>
	/// <summary>Define ComparisonTerm specialization for Object </summary>
	public class ObjectComparisonTerm : ComparisonTerm
	{
		/// <MetaDataID>{84C4282E-AA22-4F0A-82F0-E312A879B4EE}</MetaDataID>
		public DataNode DataNode;
		/// <MetaDataID>{77CAB1D9-4E10-40BF-8E7E-0D8D0EACDC48}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{
				DataNode dataNode=(DataNode)OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]];
				return ModulePublisher.ClassRepository.GetType((dataNode.Classifier as MetaDataRepository.MetaObject).FullName,"");
			}
		}
		/// <MetaDataID>{7735C2F9-F4BD-40D8-AEF6-EE0E89B8275F}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			string compareExpression=null;
			string comparisonOperator=null;
			if(comparisonType==Criterion.ComparisonType.Equal)
				comparisonOperator="=";
			else
				comparisonOperator="<>";

			if(theOtherComparisonTerm is ObjectComparisonTerm)
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
			else if( theOtherComparisonTerm is ObjectIDComparisonTerm)
			{
				#region Build comparison expresion with ObjectIDComparisonTerm
				DataNode firstTermDataNode=(DataNode)OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]];
				foreach(RDBMSMetaDataRepository.IdentityColumn column in firstTermDataNode.Classifier.ObjectIDColumns)
				{
					foreach(Parser.ParserNode objectIDField in (theOtherComparisonTerm.ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"] as Parser.ParserNode).ChildNodes)
					{
						if((objectIDField["name"] as Parser.ParserNode).Value==column.ColumnType)
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
							if(objectIDField["literal"]["single_quotedstring"]!=null)
								compareExpression+=" "+ comparisonOperator+ "  "+ (objectIDField["literal"]["single_quotedstring"] as Parser.ParserNode).Value;
							if(objectIDField["literal"]["numeric_literal"]!=null)
								compareExpression+=" "+ comparisonOperator+ "  "+ (objectIDField["literal"]["numeric_literal"] as Parser.ParserNode).Value;
						}
					}
				}
				#endregion
				return compareExpression+")";
			}
			else if(theOtherComparisonTerm is ParameterComparisonTerm)
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
		/// <MetaDataID>{C1280473-B5C5-432E-B1AC-83918D129A3F}</MetaDataID>
		internal ObjectComparisonTerm(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
            DataNode=OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
		}

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
		public RDBMSMetaDataRepository.StorageCell GetStorageCellFromObjectID(Parser.ParserNode objectIDParserNode)
		{
			if(objectIDParserNode==null)
				throw new System.Exception("Can't retrieve Storage Cell for null objectID");
			int Count=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.Count;
			for(int i=0;i!=Count;i++)
			{
				Parser.ParserNode ObjectIDField=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(i+1);
				string  ObjectIDFieldName=ObjectIDField.ChildNodes.GetAt(1).Value;
				if(ObjectIDFieldName=="ObjCellID")
				{
					string ObjCellID=ObjectIDField.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					return (DataNode.Classifier as RDBMSMetaDataRepository.Class).GetStorageCell(int.Parse(ObjCellID));
				}
			}
			return null;
		}
	}
}
