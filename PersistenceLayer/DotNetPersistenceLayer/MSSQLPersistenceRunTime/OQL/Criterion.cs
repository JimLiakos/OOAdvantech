namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{728F8A15-8EC4-4229-BDC2-B6D24A6259D8}</MetaDataID>
	public class Criterion
	{
		/// <MetaDataID>{60B4265A-D199-446D-AD54-F45749B593B0}</MetaDataID>
		/// <summary>Return a search condition SQL expression which refers to data node. 
		/// It is useful when the query refer to more than one data bases. 
		/// There is case where a data node maybe refers to data in remote table. 
		/// If you have a filter for data node data at search condition the system will 
		/// get all data from remote table and then it will filter data. 
		/// But it is better to filter data at remote data base and then get in main data base. </summary>
		public string GetSQLExpressionFor(DataNode dataNode)
		{
			if(!HasSQLExpressionFor(dataNode))
				return "";
			else
				return SQLExpression;
		}
		/// <MetaDataID>{351CC207-F022-472F-8296-B79DC798118C}</MetaDataID>
		/// <summary>Check if data node participates in search condition. </summary>
		public bool HasSQLExpressionFor(DataNode dataNode)
		{
			
			if(_ComparisonTerms[0] is ObjectAttributeComparisonTerm&&!(_ComparisonTerms[1] is ObjectAttributeComparisonTerm))
			{
				if((_ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode==dataNode)
					return true;
				else
					return false;
			}
			else if(_ComparisonTerms[1] is ObjectAttributeComparisonTerm&&!(_ComparisonTerms[0] is ObjectAttributeComparisonTerm))
			{
				if((_ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode==dataNode)
					return true;
				else
					return false;
			}

			return false;
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{6C92F514-E759-4133-82E8-58041E183D0D}</MetaDataID>
		private ComparisonTerm[] _ComparisonTerms;
		/// <MetaDataID>{0BA6C6EA-6AEB-4AD9-ACBF-5FD89179B23D}</MetaDataID>
		public ComparisonTerm[] ComparisonTerms
		{
			get
			{
				return _ComparisonTerms;
			}
		}
		


		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{CD7D845A-8022-4617-B57E-1B9CDDDB8EED}</MetaDataID>
		private string _SQLExpression;
		/// <MetaDataID>{B609C951-F11D-41C8-A93C-31AC0B947969}</MetaDataID>
		public string SQLExpression
		{
			get
			{
				
				Parser.ParserNode Criterion=ParserNode;
				Parser.ParserNode ComparisonOperator=Criterion.ChildNodes.GetAt(2);
				if(ComparisonOperator.Value=="=")
				{
					return _ComparisonTerms[0].GetCompareExpression(ComparisonType.Equal,_ComparisonTerms[1]);
				}
				else if (ComparisonOperator.Value=="<>")
				{
					return _ComparisonTerms[0].GetCompareExpression(ComparisonType.NotEqual,_ComparisonTerms[1]);
				}
				else if (ComparisonOperator.Value==">")
				{
					return _ComparisonTerms[0].GetCompareExpression(ComparisonType.GreaterThan,_ComparisonTerms[1]);

				}
				else if (ComparisonOperator.Value=="<")
				{
					return _ComparisonTerms[0].GetCompareExpression(ComparisonType.LessThan,_ComparisonTerms[1]);
				}
				return "";
			


//				string StrCriterion=null;
//
//				
//				if(_ComparisonTerms[0].Type==ComparisonTerm.ComparisonTermType.Object||_ComparisonTerms[0].Type==ComparisonTerm.ComparisonTermType.Object)
//				{
//					if(FirstTermType!=ComparisonTerm.ComparisonTermType.Object||
//						(ComparisonOperator.Value!="="&&ComparisonOperator.Value!="<>")) //error prone είναι λάθος να συγκρίνο με string π.χ. "<  >" δεν θα το καταλάβει
//					{
//
//						if(FirstTermType!=ComparisonTerm.ComparisonTermType.ObjectID)
//						{
//							if(FirstTermType!=ComparisonTerm.ComparisonTermType.Parameter)
//								throw new System.Exception("There is something wrong with the condition '"+ Criterion.Value+"'");
//						}
//						PersistenceLayer.StorageInstanceRef storageInstanceRef=null;
//						if(FirstTermType==ComparisonTerm.ComparisonTermType.Parameter)
//						{
//							string parameterName=FirstTerm.ChildNodes.GetFirst().Value;
//							if(!OQLStatement.Parameters.Contains(parameterName))
//								throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
//							object parameterValue=OQLStatement.Parameters[parameterName];
//							storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue);
//							if(storageInstanceRef==null)
//								throw new System.ArgumentException("The object of "+parameterName +" isn't persistent",parameterName);
//						}
//
//
//
//						DataNode SecondTermObjectCollection=(DataNode)OQLStatement.PathDataNodeMap[SecondTerm.ChildNodes.GetFirst()];
//						RDBMSMetaDataRepository.Class	SecondTermClass=null;
//						if(typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
//							SecondTermClass=(RDBMSMetaDataRepository.Class)SecondTermObjectCollection.AssignedMetaObject;
//						if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
//							SecondTermClass=(RDBMSMetaDataRepository.Class)((RDBMSMetaDataRepository.AssociationEnd)SecondTermObjectCollection.AssignedMetaObject).Specification;
//						System.Collections.ArrayList SecondTermColumns=new System.Collections.ArrayList();
//						foreach(RDBMSMetaDataRepository.Column CurrColumn in  SecondTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
//							SecondTermColumns.Add(CurrColumn);
//						if(FirstTermType==ComparisonTerm.ComparisonTermType.ObjectID)
//						{
//
//							foreach(Parser.ParserNode CurrColumn in FirstTerm.ChildNodes.GetFirst().ChildNodes)
//							{
//								string type=CurrColumn.ChildNodes.GetFirst().Value;
//								foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
//								{
//									if(CurrColumn.ChildNodes.GetFirst().Value==CorrespondingCurrColumn.ColumnType)
//									{							
//										if(StrCriterion!=null)
//										{
//											if(ComparisonOperator.Value =="=") //error prone
//												StrCriterion+=" AND ";
//											else
//												StrCriterion+=" OR ";//error prone
//										}
//										else
//											StrCriterion+="(";
//
//										StrCriterion+=SecondTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
//										//																									literal		            numeric_literal					numeric
//										//StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
//						
//										if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="single_quotedstring")
//											StrCriterion+=" "+ ComparisonOperator.Value + "  '"+CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Value+"'";
//
//										if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="numeric_literal")
//											StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
//
//									}
//								}
//							}
//						}
//						else
//						{
//							foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
//							{
//								if(StrCriterion!=null)
//								{
//									if(ComparisonOperator.Value =="=") //error prone
//										StrCriterion+=" AND ";
//									else
//										StrCriterion+=" OR ";//error prone
//								}
//								else
//									StrCriterion+="(";
//
//								StrCriterion+=SecondTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
//								//																									literal		            numeric_literal					numeric
//
//								StrCriterion+=" "+ ComparisonOperator.Value + "  ";
//								if(CorrespondingCurrColumn.ColumnType=="IntObjID")
//									StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).IntObjID.ToString();
//								if(CorrespondingCurrColumn.ColumnType=="ObjCellID")
//									StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).ObjCellID.ToString();
//							}
//
//						}
//						return StrCriterion+")";
//
//				
//					}
//					if(SecondTermType!=ComparisonTerm.ComparisonTermType.Object||
//						(ComparisonOperator.Value!="="&&ComparisonOperator.Value!="<>")) //error prone
//					{
//						if(SecondTermType!=ComparisonTerm.ComparisonTermType.ObjectID)
//						{
//							if(SecondTermType!=ComparisonTerm.ComparisonTermType.Parameter)
//								throw new System.Exception("There is something wrong with the condition '"+ Criterion.Value+"'");
//						}
//						PersistenceLayer.StorageInstanceRef storageInstanceRef=null;
//						if(SecondTermType==ComparisonTerm.ComparisonTermType.Parameter)
//						{
//							string parameterName=SecondTerm.ChildNodes.GetFirst().Value;
//							if(!OQLStatement.Parameters.Contains(parameterName))
//								throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
//							object parameterValue=OQLStatement.Parameters[parameterName];
//							storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue);
//							if(storageInstanceRef==null)
//								throw new System.ArgumentException("The object of "+parameterName +" isn't persistent",parameterName);
//						}
//
//						DataNode FirstTermObjectCollection=(DataNode)OQLStatement.PathDataNodeMap[FirstTerm.ChildNodes.GetFirst()];
//						RDBMSMetaDataRepository.Class	FirstTermClass=null;
//
//						if(typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
//							FirstTermClass=(RDBMSMetaDataRepository.Class)FirstTermObjectCollection.AssignedMetaObject;
//						if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
//							FirstTermClass=(RDBMSMetaDataRepository.Class)((RDBMSMetaDataRepository.AssociationEnd)FirstTermObjectCollection.AssignedMetaObject).Specification;
//
//						System.Collections.ArrayList FirstTermColumns=new System.Collections.ArrayList();
//
//						foreach(RDBMSMetaDataRepository.Column CurrColumn in  FirstTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
//							FirstTermColumns.Add(CurrColumn);
//					
//						if(SecondTermType==ComparisonTerm.ComparisonTermType.ObjectID)
//						{
//							foreach(Parser.ParserNode CurrColumn in SecondTerm.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes)
//							{
//								string type=CurrColumn.ChildNodes.GetFirst().Value;
//								foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in FirstTermColumns)
//								{
//									if(CurrColumn.ChildNodes.GetFirst().Value==CorrespondingCurrColumn.ColumnType)
//									{							
//										if(StrCriterion!=null)
//										{
//											if(ComparisonOperator.Value =="=") //error prone
//												StrCriterion+=" AND ";
//											else
//												StrCriterion+=" OR ";//error prone
//										}
//										else
//											StrCriterion+="(";
//
//										StrCriterion+=FirstTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
//										//													literal		                     numeric_literal					numeric
//										if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="single_quotedstring")
//											StrCriterion+=" "+ ComparisonOperator.Value + "  "+CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Value;
//
//										if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="numeric_literal")
//											StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
//									}
//								}
//							}
//						}
//						else
//						{
//							foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in FirstTermColumns)
//							{
//								if(StrCriterion!=null)
//								{
//									if(ComparisonOperator.Value =="=") //error prone
//										StrCriterion+=" AND ";
//									else
//										StrCriterion+=" OR ";//error prone
//								}
//								else
//									StrCriterion+="(";
//
//								StrCriterion+=FirstTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
//								//																									literal		            numeric_literal					numeric
//
//								StrCriterion+=" "+ ComparisonOperator.Value + "  ";
//								if(CorrespondingCurrColumn.ColumnType=="IntObjID")
//									StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).IntObjID.ToString();
//								if(CorrespondingCurrColumn.ColumnType=="ObjCellID")
//									StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).ObjCellID.ToString();
//							}
//						}
//						return StrCriterion+")";
//					}
//				}
//				if(FirstTermType==ComparisonTerm.ComparisonTermType.Object&&SecondTermType==ComparisonTerm.ComparisonTermType.Object)
//				{
//					DataNode FirstTermObjectCollection=(DataNode)OQLStatement.PathDataNodeMap[FirstTerm.ChildNodes.GetFirst()];
//					DataNode SecondTermObjectCollection=(DataNode)OQLStatement.PathDataNodeMap[SecondTerm.ChildNodes.GetFirst()];
//					RDBMSMetaDataRepository.Class	FirstTermClass=null;
//					RDBMSMetaDataRepository.Class	SecondTermClass=null;
//					if(typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
//						FirstTermClass=(RDBMSMetaDataRepository.Class)FirstTermObjectCollection.AssignedMetaObject;
//					if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
//						FirstTermClass=(RDBMSMetaDataRepository.Class)((RDBMSMetaDataRepository.AssociationEnd)FirstTermObjectCollection.AssignedMetaObject).Specification;
//
//					if(typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
//						SecondTermClass=(RDBMSMetaDataRepository.Class)SecondTermObjectCollection.AssignedMetaObject;
//					if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
//						SecondTermClass=(RDBMSMetaDataRepository.Class)((RDBMSMetaDataRepository.AssociationEnd)SecondTermObjectCollection.AssignedMetaObject).Specification;
//
//					System.Collections.ArrayList FirstTermColumns=new System.Collections.ArrayList();
//					System.Collections.ArrayList SecondTermColumns=new System.Collections.ArrayList();
//
//					foreach(RDBMSMetaDataRepository.Column CurrColumn in  FirstTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
//						FirstTermColumns.Add(CurrColumn);
//					foreach(RDBMSMetaDataRepository.Column CurrColumn in  SecondTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
//						SecondTermColumns.Add(CurrColumn);
//					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstTermColumns)
//					{
//						foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
//						{
//							if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
//							{							
//								if(StrCriterion!=null)
//								{
//									if(ComparisonOperator.Value =="=") //error prone
//										StrCriterion+=" AND ";
//									else
//										StrCriterion+=" OR ";//error prone
//								}
//								else
//									StrCriterion+="(";
//								StrCriterion+=FirstTermObjectCollection.Alias+"."+CurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
//								StrCriterion+=" "+ ComparisonOperator.Value + "  "+SecondTermObjectCollection.Alias;//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
//								StrCriterion+="."+CorrespondingCurrColumn.Name;
//							}
//						}
//					}
//					return StrCriterion+")";
//				}
//
//				if(FirstTermType==ComparisonTerm.ComparisonTermType.OjectAttribute)
//				{
//					Parser.ParserNode Path=FirstTerm.ChildNodes.GetFirst();
//					StrCriterion+=((DataNode)OQLStatement.PathDataNodeMap[Path]).ParentDataNode.Alias;
//					StrCriterion+=".";
//					StrCriterion+=((DataNode)OQLStatement.PathDataNodeMap[Path]).Name;
//				}
//				else
//				{
//					if(FirstTermType==ComparisonTerm.ComparisonTermType.Parameter)
//					{
//						if(SecondTermType==ComparisonTerm.ComparisonTermType.OjectAttribute)
//						{
//							Parser.ParserNode Path=SecondTerm.ChildNodes.GetFirst();
//							MetaDataRepository.Attribute attribute =((DataNode)OQLStatement.PathDataNodeMap[Path]).AssignedMetaObject as  MetaDataRepository.Attribute;
//							System.Type attributeType= TypeDictionary.GetDotNetType(attribute.Type.FullName);
//							string parameterName=FirstTerm.ChildNodes.GetFirst().Value;
//							if(!OQLStatement.Parameters.Contains(parameterName))
//								throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
//							object parameterValue=OQLStatement.Parameters[parameterName];
//							if(parameterValue==null)
//								StrCriterion+="IS NULL";//Error prone θα βγαλλει IS NULL = ΧΧΧΧΧ.ΧΧΧΧ
//							else
//							{
//								if(parameterValue.GetType()!=attributeType)
//									throw new System.ArgumentException("The parameter with name "+parameterName+" has invalid type. Must be "+attributeType.FullName+".",parameterName);
//								StrCriterion+=TypeDictionary.ConvertToSQLString(parameterValue);
//							}
//						}
//	
//					}
//					else
//						StrCriterion+=FirstTerm.ChildNodes.GetFirst().Value;
//				}
//
//				StrCriterion+=" "+ComparisonOperator.Value+" ";
//
//				if(SecondTermType==ComparisonTerm.ComparisonTermType.OjectAttribute)
//				{
//					Parser.ParserNode Path=SecondTerm.ChildNodes.GetFirst();
//					StrCriterion+=((DataNode)OQLStatement.PathDataNodeMap[Path]).ParentDataNode.Alias;
//					StrCriterion+=".";
//					StrCriterion+=((DataNode)OQLStatement.PathDataNodeMap[Path]).Name;
//					return StrCriterion;
//				}
//				else
//				{
//					if(SecondTermType==ComparisonTerm.ComparisonTermType.Parameter)
//					{
//						if(FirstTermType==ComparisonTerm.ComparisonTermType.OjectAttribute)
//						{
//							Parser.ParserNode Path=FirstTerm.ChildNodes.GetFirst();
//							MetaDataRepository.Attribute attribute =((DataNode)OQLStatement.PathDataNodeMap[Path]).AssignedMetaObject as  MetaDataRepository.Attribute;
//							System.Type attributeType= TypeDictionary.GetDotNetType(attribute.Type.FullName);
//							string parameterName=SecondTerm.ChildNodes.GetFirst().Value;
//							if(!OQLStatement.Parameters.Contains(parameterName))
//								throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
//							object parameterValue=OQLStatement.Parameters[parameterName];
//							if(parameterValue==null)
//								StrCriterion+="IS NULL";//Error prone θα βγαλλει  ΧΧΧΧΧ.ΧΧΧΧ = IS NULL 
//							else
//							{
//								if(parameterValue.GetType()!=attributeType)
//									throw new System.ArgumentException("The parameter with name "+parameterName+" has invalid type. Must be "+attributeType.FullName+".",parameterName);
//								StrCriterion+=TypeDictionary.ConvertToSQLString(parameterValue);
//							}
//						}
//					}
//					else
//					{
//						if(SecondTerm.ChildNodes.GetFirst().ChildNodes.GetFirst().Name=="date_literal")
//						{
//							System.DateTime dateTime=System.DateTime.Parse(SecondTerm.ChildNodes.GetFirst().Value);
//
//							string dateExpretion="CONVERT(DATETIME, '"+dateTime.Year.ToString()+"-"+
//								dateTime.Month.ToString()+"-"+
//								dateTime.Day.ToString()+" "+
//								dateTime.Hour.ToString()+":"+
//								dateTime.Minute.ToString()+":"+
//								dateTime.Second.ToString()+"',102)";
//							StrCriterion+=dateExpretion;
//
//						}
//						else
//							StrCriterion+=SecondTerm.ChildNodes.GetFirst().Value;
//					}
//				}
//
//				return StrCriterion;
			}
		}
		public enum ComparisonType{Equal=0,NotEqual,GreaterThan,LessThan};
		/// <MetaDataID>{6EB43AA8-2F3D-4685-845B-DD7CF9969A7B}</MetaDataID>
		public Parser.ParserNode ParserNode;
		/// <MetaDataID>{E70C3E53-CCF6-466E-BC49-7F33E43947A9}</MetaDataID>
		internal OQLStatement OQLStatement;
		/// <MetaDataID>{CA8FB35E-6D05-40A6-852A-7E0F4D5C6B7A}</MetaDataID>
		internal Criterion(Parser.ParserNode criterionParserNode, OQLStatement oqlStatement)
		{
			

			ParserNode=criterionParserNode;
			OQLStatement=oqlStatement;

			 _ComparisonTerms=new ComparisonTerm[2];
			 _ComparisonTerms[0]=ComparisonTerm.GetComparisonTermFor(criterionParserNode["comparison_item"][0]as Parser.ParserNode,oqlStatement);
			 _ComparisonTerms[1]= ComparisonTerm.GetComparisonTermFor(criterionParserNode["comparison_item"][1]as Parser.ParserNode,oqlStatement);
			 int ttt=0;


		}

	}
}
