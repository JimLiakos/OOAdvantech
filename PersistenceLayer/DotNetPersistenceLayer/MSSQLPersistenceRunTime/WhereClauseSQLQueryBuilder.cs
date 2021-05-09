namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{

	/// <MetaDataID>{DC790563-C452-49AC-AD0D-A2833A5160E0}</MetaDataID>
	internal class WhereClauseSQLQueryBuilder
	{
		private enum CritirionTermType{OjectAttribute=0,Object,Parameter,Literal,ObjectID,Unknown};
		/// <MetaDataID>{812B8412-D334-44A2-91A2-0A20FDA8738D}</MetaDataID>
		private System.Collections.Hashtable PathObjectCollectionMap;
		/// <MetaDataID>{0E73F838-6ED8-491F-B737-BA7443704800}</MetaDataID>
		 public WhereClauseSQLQueryBuilder(System.Collections.Hashtable thePathObjectCollectionMap,OOAdvantech.Collections.Map parameters)
		{
			Parameters=parameters;
			PathObjectCollectionMap=thePathObjectCollectionMap;
		}
		/// <MetaDataID>{C51F13A1-1866-430C-8782-C25B8B8603DC}</MetaDataID>
		private OOAdvantech.Collections.Map Parameters;

		
		/// <MetaDataID>{280A477E-24C3-4AE5-AF2C-A271DEB637B9}</MetaDataID>
		public string BuildWhereClauseSQLQuery(Parser.ParserNode SelectExpression)
		{
			
			string WhereClauseSQLQuery=null;

			for(short i=1;i<SelectExpression.ChildNodes.Count+1;i++)
			{
				if(SelectExpression.ChildNodes.GetAt(i).Name=="Critiria_exp")
				{
					WhereClauseSQLQuery+="\nWHERE ";
					Parser.ParserNode Critiria_exp=SelectExpression.ChildNodes.GetAt(i);
					Parser.ParserNode search_condition=Critiria_exp.ChildNodes.GetFirst();
					WhereClauseSQLQuery+=BuildWhereClauseConditionSQLQuery(search_condition);
					return WhereClauseSQLQuery;
				}
			}
			return null;
		}

		/// <MetaDataID>{FB84B2D6-A821-41D1-AF4B-4861DB04D998}</MetaDataID>
		protected string BuildWhereClauseSearchTermSQLQuery(Parser.ParserNode SearchTerm)
		{
			string Term=null;
			if(SearchTerm.ChildNodes.Count>1)
				Term="(";

			for(short i=1;i<SearchTerm.ChildNodes.Count+1;i++)
			{
				if(i>1)
					Term+=" AND ";

				if(SearchTerm.ChildNodes.GetAt(i).Name=="search_factor")
				{
					Parser.ParserNode SearchFactor=SearchTerm.ChildNodes.GetAt(i);
					Term+=BuildWhereClauseSearchFactorSQLQuery(SearchFactor);
				}
				else
				{
					if(SearchTerm.ChildNodes.GetAt(i).Name=="Criterion")
					{
						Parser.ParserNode Criterion=SearchTerm.ChildNodes.GetAt(i);//.ChildNodes.GetFirst();
						Term+=BuildWhereClauseCriterionSQLQuery(Criterion);
					}
				}
			}
			if(SearchTerm.ChildNodes.Count>1)
				Term+=")";
			return Term;


		}
		/// <MetaDataID>{87FF2739-68EF-4C5D-959B-7663A7C61126}</MetaDataID>
		protected string BuildWhereClauseSearchFactorSQLQuery(Parser.ParserNode SearchFactor)
		{
			string Factor=null;
			//			if(SearchFactor.ChildNodes.Count>1)
			//	="(";

			for(short i=1;i<SearchFactor.ChildNodes.Count+1;i++)
			{
				if(SearchFactor.ChildNodes.GetAt(i).Name=="search_condition")
				{
					Parser.ParserNode SearchCondition=SearchFactor.ChildNodes.GetAt(i);
					Factor+=BuildWhereClauseConditionSQLQuery(SearchCondition);
				}
				else
				{
					if(SearchFactor.ChildNodes.GetAt(i).Name=="Criterion")
					{
						Parser.ParserNode Criterion=SearchFactor.ChildNodes.GetAt(i);//.ChildNodes.GetFirst();
						Factor+=BuildWhereClauseCriterionSQLQuery(Criterion);
					}
				}
			}
			
			//	Factor+=")";
			return Factor;


		}

		/// <MetaDataID>{3F9EE981-1485-402C-B689-231C85F3D600}</MetaDataID>
		private CritirionTermType GetCriterionTermType(Parser.ParserNode CriterionTerm)
		{
			string TypeName=CriterionTerm.ChildNodes.GetFirst().Name;
			if(TypeName=="ObjectID")
				return CritirionTermType.ObjectID;
			
			if(TypeName=="literal")
				return CritirionTermType.Literal;
			if(TypeName=="parameter")
				return CritirionTermType.Parameter;
			if(TypeName=="Path")
			{
				DataNode PathCorespondingObjectCollection=(DataNode)PathObjectCollectionMap[CriterionTerm.ChildNodes.GetFirst()];
				if(PathCorespondingObjectCollection!=null)
				{
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingAttribute).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
						return CritirionTermType.OjectAttribute;
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingClass).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
						return CritirionTermType.Object;
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingAssociationEnd).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
					{
						if(((RDBMSMetaDataRepository.RDBMSMappingAssociationEnd)PathCorespondingObjectCollection.AssignedMetaObject).Specification!=null)
							return CritirionTermType.Object;
					}
				}
			}
			throw new System.Exception("Critirion term with unknown type");
		}
		/// <MetaDataID>{69ACCDC8-113B-4B9D-9022-D70F41ADFFB0}</MetaDataID>
		protected string BuildWhereClauseCriterionSQLQuery(Parser.ParserNode Criterion)
		{
			Parser.ParserNode FirstTerm=Criterion.ChildNodes.GetFirst();
			Parser.ParserNode ComparisonOperator=Criterion.ChildNodes.GetAt(2);
			Parser.ParserNode SecondTerm=Criterion.ChildNodes.GetAt(3);
			CritirionTermType FirstTermType=GetCriterionTermType(FirstTerm);
			CritirionTermType SecondTermType=GetCriterionTermType(SecondTerm);
			string StrCriterion=null;
			if(FirstTermType==CritirionTermType.Object||SecondTermType==CritirionTermType.Object)
			{
				if(FirstTermType!=CritirionTermType.Object||
					(ComparisonOperator.Value!="="&&ComparisonOperator.Value!="<>")) //error prone είναι λάθος να συγκρίνο με string π.χ. "<  >" δεν θα το καταλάβει
				{

					if(FirstTermType!=CritirionTermType.ObjectID)
					{
						if(FirstTermType!=CritirionTermType.Parameter)
							throw new System.Exception("There is something wrong with the condition '"+ Criterion.Value+"'");
					}
					PersistenceLayer.StorageInstanceRef storageInstanceRef=null;
					if(FirstTermType==CritirionTermType.Parameter)
					{
						string parameterName=FirstTerm.ChildNodes.GetFirst().Value;
						if(!Parameters.Contains(parameterName))
							throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
						object parameterValue=Parameters[parameterName];
						storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue);
						if(storageInstanceRef==null)
							throw new System.ArgumentException("The object of "+parameterName +" isn't persistent",parameterName);
					}



					DataNode SecondTermObjectCollection=(DataNode)PathObjectCollectionMap[SecondTerm.ChildNodes.GetFirst()];
					RDBMSMetaDataRepository.RDBMSMappingClass	SecondTermClass=null;
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingClass).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
						SecondTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)SecondTermObjectCollection.AssignedMetaObject;
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingAssociationEnd).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
						SecondTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)((RDBMSMetaDataRepository.RDBMSMappingAssociationEnd)SecondTermObjectCollection.AssignedMetaObject).Specification;
					System.Collections.ArrayList SecondTermColumns=new System.Collections.ArrayList();
					foreach(RDBMSMetaDataRepository.RDBMSColumn CurrColumn in  SecondTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
						SecondTermColumns.Add(CurrColumn);
					if(FirstTermType==CritirionTermType.ObjectID)
					{

						foreach(Parser.ParserNode CurrColumn in FirstTerm.ChildNodes.GetFirst().ChildNodes)
						{
							string type=CurrColumn.ChildNodes.GetFirst().Value;
							foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CorrespondingCurrColumn in SecondTermColumns)
							{
								if(CurrColumn.ChildNodes.GetFirst().Value==CorrespondingCurrColumn.ColumnType)
								{							
									if(StrCriterion!=null)
									{
										if(ComparisonOperator.Value =="=") //error prone
											StrCriterion+=" AND ";
										else
											StrCriterion+=" OR ";//error prone
									}
									else
										StrCriterion+="(";

									StrCriterion+=SecondTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
									//																									literal		            numeric_literal					numeric
									//StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
						
									if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="single_quotedstring")
										StrCriterion+=" "+ ComparisonOperator.Value + "  '"+CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Value+"'";

									if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="numeric_literal")
										StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;

								}
							}
						}
					}
					else
					{
						foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CorrespondingCurrColumn in SecondTermColumns)
						{
							if(StrCriterion!=null)
							{
								if(ComparisonOperator.Value =="=") //error prone
									StrCriterion+=" AND ";
								else
									StrCriterion+=" OR ";//error prone
							}
							else
								StrCriterion+="(";

							StrCriterion+=SecondTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
							//																									literal		            numeric_literal					numeric

							StrCriterion+=" "+ ComparisonOperator.Value + "  ";
							if(CorrespondingCurrColumn.ColumnType=="IntObjID")
								StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).IntObjID.ToString();
							if(CorrespondingCurrColumn.ColumnType=="ObjCellID")
								StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).ObjCellID.ToString();
						}

					}
					return StrCriterion+")";

				
				}
				if(SecondTermType!=CritirionTermType.Object||
					(ComparisonOperator.Value!="="&&ComparisonOperator.Value!="<>")) //error prone
				{
					if(SecondTermType!=CritirionTermType.ObjectID)
					{
						if(SecondTermType!=CritirionTermType.Parameter)
							throw new System.Exception("There is something wrong with the condition '"+ Criterion.Value+"'");
					}
					PersistenceLayer.StorageInstanceRef storageInstanceRef=null;
					if(SecondTermType==CritirionTermType.Parameter)
					{
						string parameterName=SecondTerm.ChildNodes.GetFirst().Value;
						if(!Parameters.Contains(parameterName))
							throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
						object parameterValue=Parameters[parameterName];
						storageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(parameterValue);
						if(storageInstanceRef==null)
							throw new System.ArgumentException("The object of "+parameterName +" isn't persistent",parameterName);
					}

					DataNode FirstTermObjectCollection=(DataNode)PathObjectCollectionMap[FirstTerm.ChildNodes.GetFirst()];
					RDBMSMetaDataRepository.RDBMSMappingClass	FirstTermClass=null;

					if(typeof(RDBMSMetaDataRepository.RDBMSMappingClass).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
						FirstTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)FirstTermObjectCollection.AssignedMetaObject;
					if(typeof(RDBMSMetaDataRepository.RDBMSMappingAssociationEnd).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
						FirstTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)((RDBMSMetaDataRepository.RDBMSMappingAssociationEnd)FirstTermObjectCollection.AssignedMetaObject).Specification;

					System.Collections.ArrayList FirstTermColumns=new System.Collections.ArrayList();

					foreach(RDBMSMetaDataRepository.RDBMSColumn CurrColumn in  FirstTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
						FirstTermColumns.Add(CurrColumn);
					
					if(SecondTermType==CritirionTermType.ObjectID)
					{
						foreach(Parser.ParserNode CurrColumn in SecondTerm.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes)
						{
							string type=CurrColumn.ChildNodes.GetFirst().Value;
							foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CorrespondingCurrColumn in FirstTermColumns)
							{
								if(CurrColumn.ChildNodes.GetFirst().Value==CorrespondingCurrColumn.ColumnType)
								{							
									if(StrCriterion!=null)
									{
										if(ComparisonOperator.Value =="=") //error prone
											StrCriterion+=" AND ";
										else
											StrCriterion+=" OR ";//error prone
									}
									else
										StrCriterion+="(";

									StrCriterion+=FirstTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
									//													literal		                     numeric_literal					numeric
									if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="single_quotedstring")
										StrCriterion+=" "+ ComparisonOperator.Value + "  "+CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Value;

									if(CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().Name=="numeric_literal")
										StrCriterion+=" "+ ComparisonOperator.Value + "  "+ CurrColumn.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
								}
							}
						}
					}
					else
					{
						foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CorrespondingCurrColumn in FirstTermColumns)
						{
							if(StrCriterion!=null)
							{
								if(ComparisonOperator.Value =="=") //error prone
									StrCriterion+=" AND ";
								else
									StrCriterion+=" OR ";//error prone
							}
							else
								StrCriterion+="(";

							StrCriterion+=FirstTermObjectCollection.Alias+"."+CorrespondingCurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
							//																									literal		            numeric_literal					numeric

							StrCriterion+=" "+ ComparisonOperator.Value + "  ";
							if(CorrespondingCurrColumn.ColumnType=="IntObjID")
								StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).IntObjID.ToString();
							if(CorrespondingCurrColumn.ColumnType=="ObjCellID")
								StrCriterion+=(storageInstanceRef.ObjectID as ObjectID).ObjCellID.ToString();
						}
					}
					return StrCriterion+")";
				}
			}
			if(FirstTermType==CritirionTermType.Object&&SecondTermType==CritirionTermType.Object)
			{
				DataNode FirstTermObjectCollection=(DataNode)PathObjectCollectionMap[FirstTerm.ChildNodes.GetFirst()];
				DataNode SecondTermObjectCollection=(DataNode)PathObjectCollectionMap[SecondTerm.ChildNodes.GetFirst()];
				RDBMSMetaDataRepository.RDBMSMappingClass	FirstTermClass=null;
				RDBMSMetaDataRepository.RDBMSMappingClass	SecondTermClass=null;
				if(typeof(RDBMSMetaDataRepository.RDBMSMappingClass).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
					FirstTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)FirstTermObjectCollection.AssignedMetaObject;
				if(typeof(RDBMSMetaDataRepository.RDBMSMappingAssociationEnd).IsInstanceOfType(FirstTermObjectCollection.AssignedMetaObject))
					FirstTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)((RDBMSMetaDataRepository.RDBMSMappingAssociationEnd)FirstTermObjectCollection.AssignedMetaObject).Specification;

				if(typeof(RDBMSMetaDataRepository.RDBMSMappingClass).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
					SecondTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)SecondTermObjectCollection.AssignedMetaObject;
				if(typeof(RDBMSMetaDataRepository.RDBMSMappingAssociationEnd).IsInstanceOfType(SecondTermObjectCollection.AssignedMetaObject))
					SecondTermClass=(RDBMSMetaDataRepository.RDBMSMappingClass)((RDBMSMetaDataRepository.RDBMSMappingAssociationEnd)SecondTermObjectCollection.AssignedMetaObject).Specification;

				System.Collections.ArrayList FirstTermColumns=new System.Collections.ArrayList();
				System.Collections.ArrayList SecondTermColumns=new System.Collections.ArrayList();

				foreach(RDBMSMetaDataRepository.RDBMSColumn CurrColumn in  FirstTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
					FirstTermColumns.Add(CurrColumn);
				foreach(RDBMSMetaDataRepository.RDBMSColumn CurrColumn in  SecondTermClass.ActiveStorageCell.MainTable.ObjectIDColumns)
					SecondTermColumns.Add(CurrColumn);
				foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CurrColumn in FirstTermColumns)
				{
					foreach(RDBMSMetaDataRepository.RDBMSIdentityColumn CorrespondingCurrColumn in SecondTermColumns)
					{
						if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
						{							
							if(StrCriterion!=null)
							{
								if(ComparisonOperator.Value =="=") //error prone
									StrCriterion+=" AND ";
								else
									StrCriterion+=" OR ";//error prone
							}
							else
								StrCriterion+="(";
							StrCriterion+=FirstTermObjectCollection.Alias+"."+CurrColumn.Name;//"Abstract_"+Class.Name+"."+CurrColumn.Name;
							StrCriterion+=" "+ ComparisonOperator.Value + "  "+SecondTermObjectCollection.Alias;//((RDBMSMetaDataRepository.RDBMSMappingAssociation)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
							StrCriterion+="."+CorrespondingCurrColumn.Name;
						}
					}
				}
				return StrCriterion+")";
			}

			if(FirstTermType==CritirionTermType.OjectAttribute)
			{
				Parser.ParserNode Path=FirstTerm.ChildNodes.GetFirst();
				StrCriterion+=((DataNode)PathObjectCollectionMap[Path]).ParentDataNode.Alias;
				StrCriterion+=".";
				StrCriterion+=((DataNode)PathObjectCollectionMap[Path]).Name;
			}
			else
			{
				if(FirstTermType==CritirionTermType.Parameter)
				{
					if(SecondTermType==CritirionTermType.OjectAttribute)
					{
						Parser.ParserNode Path=SecondTerm.ChildNodes.GetFirst();
						MetaDataRepository.Attribute attribute =((DataNode)PathObjectCollectionMap[Path]).AssignedMetaObject as  MetaDataRepository.Attribute;
						System.Type attributeType= TypeDictionary.GetDotNetType(attribute.Type.FullName);
						string parameterName=FirstTerm.ChildNodes.GetFirst().Value;
						if(!Parameters.Contains(parameterName))
							throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
						object parameterValue=Parameters[parameterName];
						if(parameterValue==null)
							StrCriterion+="IS NULL";//Error prone θα βγαλλει IS NULL = ΧΧΧΧΧ.ΧΧΧΧ
						else
						{
							if(parameterValue.GetType()!=attributeType)
								throw new System.ArgumentException("The parameter with name "+parameterName+" has invalid type. Must be "+attributeType.FullName+".",parameterName);
							StrCriterion+=TypeDictionary.ConvertToSQLString(parameterValue);
						}
					}
	
				}
				else
					StrCriterion+=FirstTerm.ChildNodes.GetFirst().Value;
			}

			StrCriterion+=" "+ComparisonOperator.Value+" ";

			if(SecondTermType==CritirionTermType.OjectAttribute)
			{
				Parser.ParserNode Path=SecondTerm.ChildNodes.GetFirst();
				StrCriterion+=((DataNode)PathObjectCollectionMap[Path]).ParentDataNode.Alias;
				StrCriterion+=".";
				StrCriterion+=((DataNode)PathObjectCollectionMap[Path]).Name;
				return StrCriterion;
			}
			else
			{
				if(SecondTermType==CritirionTermType.Parameter)
				{
					if(FirstTermType==CritirionTermType.OjectAttribute)
					{
						Parser.ParserNode Path=FirstTerm.ChildNodes.GetFirst();
						MetaDataRepository.Attribute attribute =((DataNode)PathObjectCollectionMap[Path]).AssignedMetaObject as  MetaDataRepository.Attribute;
						System.Type attributeType= TypeDictionary.GetDotNetType(attribute.Type.FullName);
						string parameterName=SecondTerm.ChildNodes.GetFirst().Value;
						if(!Parameters.Contains(parameterName))
							throw new System.ArgumentException("There isn't value for "+parameterName,parameterName);
						object parameterValue=Parameters[parameterName];
						if(parameterValue==null)
							StrCriterion+="IS NULL";//Error prone θα βγαλλει  ΧΧΧΧΧ.ΧΧΧΧ = IS NULL 
						else
						{
							if(parameterValue.GetType()!=attributeType)
								throw new System.ArgumentException("The parameter with name "+parameterName+" has invalid type. Must be "+attributeType.FullName+".",parameterName);
							StrCriterion+=TypeDictionary.ConvertToSQLString(parameterValue);
						}
					}
				}
				else
				{
					if(SecondTerm.ChildNodes.GetFirst().ChildNodes.GetFirst().Name=="date_literal")
					{
						System.DateTime dateTime=System.DateTime.Parse(SecondTerm.ChildNodes.GetFirst().Value);

						string dateExpretion="CONVERT(DATETIME, '"+dateTime.Year.ToString()+"-"+
							dateTime.Month.ToString()+"-"+
							dateTime.Day.ToString()+" "+
							dateTime.Hour.ToString()+":"+
							dateTime.Minute.ToString()+":"+
							dateTime.Second.ToString()+"',102)";
						StrCriterion+=dateExpretion;

					}
					else
						StrCriterion+=SecondTerm.ChildNodes.GetFirst().Value;
				}
			}

			return StrCriterion;

		}


		/// <MetaDataID>{B68F8652-C39B-455D-804D-E01F9BB87D2A}</MetaDataID>
		protected string BuildWhereClauseConditionSQLQuery(Parser.ParserNode SearchCondition)
		{
			string Condition="(";

			for(short i=1;i<SearchCondition.ChildNodes.Count+1;i++)
			{
				if(i>1)
					Condition+=" OR ";
				Parser.ParserNode SearchTerm=SearchCondition.ChildNodes.GetAt(i);
				Condition+=BuildWhereClauseSearchTermSQLQuery(SearchTerm);
			}
			Condition+=")";
			return Condition;


		}

	}
}
