namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{B61BF2C9-D44A-48CD-AAB1-4EF6A0C2C2FC}</MetaDataID>
	internal class OQLStatement
	{
		public OQLStatement(OOAdvantech.Collections.Map parameters)
		{
			Parameters=parameters;

		}
		OOAdvantech.Collections.Map Parameters;
		/// <summary>The UsedDataSourceAliases collection keeps all data source alias names.
		/// In data tree there is alias name for all data source . 
		/// The alias name must be unique in the OQLStatement.
		/// The OQLStatement look at the collection when produce alias names.</summary>
		/// <MetaDataID>{A9A56329-56C9-446C-A1F8-5385B774EEA2}</MetaDataID>
		private System.Collections.ArrayList UsedDataSourceAliases= new System.Collections.ArrayList();

		/// <MetaDataID>{399D0F7B-5C4A-4370-96B4-A2D503D75E3D}</MetaDataID>
		public string  GetValidAlias(string proposedAlias)
		{
			string ValidAlias=null;
			
			ValidAlias=proposedAlias;
			int Count=0;
			//while(UsedTableNames.Contains(ValidTableName))
			while(UsedDataSourceAliases.Contains(ValidAlias))
			{
				Count++;
				ValidAlias=proposedAlias+"_"+Count.ToString();
			}
			UsedDataSourceAliases.Add(ValidAlias);
			return ValidAlias;
		}
		/// <MetaDataID>{BD2EF519-88EA-45C7-91BA-8BC277922E43}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private System.Collections.ArrayList _SelectListItems;
		/// <MetaDataID>{51550934-EEAF-47EA-BB95-CFE404EAB34C}</MetaDataID>
		public System.Collections.ArrayList SelectListItems
		{
			get
			{
				if(SelectQueryExpression==null)
					throw new System.Exception("There isn't translated oql statement");
				return new System.Collections.ArrayList(_SelectListItems);
			}
		}
		/// <MetaDataID>{EC2308C1-0F75-46DB-80BD-73892413B9DD}</MetaDataID>
		internal void AddSelectListItem(DataNode SelectListItem)
		{
			_SelectListItems.Add(SelectListItem);
			SelectListItem.ParticipateInSelectClause=true;
		}
	


		/// <MetaDataID>{E7818100-D52D-4A7D-B1C0-D10C6EB1FFED}</MetaDataID>
		public System.Collections.ArrayList WhereClauseDataNodes = new System.Collections.ArrayList();
			

		
		/// <MetaDataID>{B7133B48-F88F-4D9F-9DA3-54525BEEEDA5}</MetaDataID>
		protected string BuildWhereClauseSQLQuery()
		{
			WhereClauseSQLQueryBuilder mWhereClauseSQLQueryBuilder=new WhereClauseSQLQueryBuilder(PathDataNodeMap,Parameters);
			return mWhereClauseSQLQueryBuilder.BuildWhereClauseSQLQuery(SelectQueryExpression);
		}
		/// <MetaDataID>{5E7FEB3B-C647-429F-A055-32CC5923A9F7}</MetaDataID>
		protected System.Collections.ArrayList GetSelectClausePaths(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList Paths=new System.Collections.ArrayList();
			Parser.ParserNode SelectList=SelectExpression.ChildNodes.GetFirst().ChildNodes.GetFirst();
			GetPathsFromNode(SelectList,Paths);
			
			return Paths;
		}
		/// <MetaDataID>{0FC5BBAA-EDBA-4C02-8DB7-C0287AF5B8B7}</MetaDataID>
		protected System.Collections.ArrayList GetFromClausePaths(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList Paths=new System.Collections.ArrayList();
			//							query_expression->	objectcollection_exp 			
			Parser.ParserNode DataNodesExp=SelectExpression.ChildNodes.GetAt(2);
			GetPathsFromNode(DataNodesExp,Paths);
			
			return Paths;
		}
		/// <MetaDataID>{779D6874-1F83-4717-A869-13FCCB17F25B}</MetaDataID>
		protected System.Collections.ArrayList GetWhereClausePaths(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList Paths=new System.Collections.ArrayList();
			for(short i=1;i<SelectExpression.ChildNodes.Count+1;i++)
			{
				if(SelectExpression.ChildNodes.GetAt(i).Name=="Critiria_exp")
					GetPathsFromNode(SelectExpression.ChildNodes.GetAt(i),Paths);
			}
			return Paths;
		}
		/// <MetaDataID>{AD2F3840-912C-457C-B100-AA774307BE1D}</MetaDataID>
		protected void GetPathsFromNode(Parser.ParserNode ParserNode,System.Collections.ArrayList Paths )
		{
			if(ParserNode.Name=="PathAlias"||ParserNode.Name=="Path" ||ParserNode.Name=="TimePeriodPathAlias")
				Paths.Add(ParserNode);
			else
			{
				for(short i=1;i<ParserNode.ChildNodes.Count+1;i++)
					GetPathsFromNode(ParserNode.ChildNodes.GetAt(i),Paths);
			}
		}
		/// <MetaDataID>{C1BC5F51-5E8E-40CD-809B-DCCF9DEED5A4}</MetaDataID>
		protected System.Collections.ArrayList GetOrderByPaths(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList Paths=new System.Collections.ArrayList();
			return Paths;
		}
		/// <MetaDataID>{25EF1E24-6ADC-40B3-B1FB-6FD0BCA22A80}</MetaDataID>
		protected DataNode CreateDataNodeFor(Parser.ParserNode ParserNode,DataNode ParentDataNode)
		{
			DataNode MyDataNode=new DataNode(this);
			

			Parser.ParserNode PathMember=null;
			switch(ParserNode.Name)
			{
				case "TimePeriodPathAlias":
				{
					//						TimePeriodPathAlias				\PathAlias          \Path                  \ClassOrAlias			\name
					MyDataNode.Name=ParserNode.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					if(ParserNode.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes.Count>=2)
						PathMember=ParserNode.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes.GetAt(2);
					break;
				}

				case "PathAlias":
				{
					//						PathAlias          \Path                  \ClassOrAlias			\name
					MyDataNode.Name=ParserNode.ChildNodes.GetFirst().ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					if(ParserNode.ChildNodes.GetFirst().ChildNodes.Count>=2)
						PathMember=ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(2);
					break;
				}
				case "Path":
				{
					//						Path                  \ClassOrAlias			\name
					MyDataNode.Name=ParserNode.ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					if(ParserNode.ChildNodes.Count>=2)
						PathMember=ParserNode.ChildNodes.GetAt(2);
					break;
				}
				case "PathMember":
				{
					//						PathMember            \name
					MyDataNode.Name=ParserNode.ChildNodes.GetFirst().Value;
					if(ParserNode.ChildNodes.Count>=2)
						PathMember=ParserNode.ChildNodes.GetAt(2);
					break;
				}
			}
			
			if(ParentDataNode!=null)
			{
				if(MyDataNode.Name!=ParentDataNode.Alias)
				{
					MyDataNode.ParentDataNode=ParentDataNode;
				}
				else
				{
					MyDataNode=ParentDataNode;
				}
			}




			if(PathMember!=null)
			{
				MyDataNode= CreateDataNodeFor(PathMember,MyDataNode);
				if(ParserNode.Name=="PathAlias")
					MyDataNode.Alias=ParserNode.ChildNodes.GetAt(2).Value;
				if(ParserNode.Name=="TimePeriodPathAlias")
				{
					MyDataNode.Alias=ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(2).Value;
					 Parser.ParserNode TimePeriodParserNode=ParserNode.ChildNodes.GetAt(2);
					Parser.ParserNode StartDateParserNode=TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(1);
					Parser.ParserNode StartDateLocaleParserNode=null;
					if(TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.Count>1)
						StartDateLocaleParserNode=TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(2);

					Parser.ParserNode EndDateParserNode=TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.GetAt(1);
					Parser.ParserNode EndDateLocaleParserNode=null;
					if(TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.Count>1)
                        EndDateLocaleParserNode=TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.GetAt(2);

					MyDataNode.HasTimePeriodConstrain=true;
					
					if(EndDateLocaleParserNode==null && StartDateLocaleParserNode==null)
					{
						MyDataNode.TimePeriodStartDate=System.DateTime.Parse(StartDateParserNode.Value);
						MyDataNode.TimePeriodEndDate=System.DateTime.Parse(EndDateParserNode.Value);
						// Error Prone εάν δεν περάσει το parse εγείρει exception αλλαντάλλων
					}
					else
					{
						System.Globalization.CultureInfo StartDateLocale=null,EndDateLocale=null;
						if(EndDateLocaleParserNode==null)
							EndDateLocaleParserNode=StartDateLocaleParserNode;
						if(StartDateLocaleParserNode==null)
							StartDateLocaleParserNode=EndDateLocaleParserNode;
						StartDateLocale=new System.Globalization.CultureInfo((int)System.Convert.ChangeType(StartDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int)));
						EndDateLocale=new System.Globalization.CultureInfo((int)System.Convert.ChangeType(EndDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int)));
						MyDataNode.TimePeriodStartDate=System.DateTime.Parse(StartDateParserNode.Value,StartDateLocale);
						MyDataNode.TimePeriodEndDate=System.DateTime.Parse(EndDateParserNode.Value,EndDateLocale);
						// Error Prone εάν δεν περάσει το parse εγείρει exception αλλαντάλλων
 					}
				}
			}
			return MyDataNode;
		}
		
		/// <MetaDataID>{5E3DE1E2-992C-4132-9FB1-FCC48A8BADBE}</MetaDataID>
		protected void RetrieveDataNodesSource(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList FromClausePaths=GetFromClausePaths(SelectExpression);
			System.Collections.ArrayList PreBuildFromDataNodes=new System.Collections.ArrayList();
			foreach(Parser.ParserNode CurrParseNode in FromClausePaths)
			{
				if(CurrParseNode.Name=="PathAlias")
				{
					DataNode mDataNode=CreateDataNodeFor(CurrParseNode,null);
					if(mDataNode.Alias!=null)
						DataNodeAliases.Add(mDataNode.Alias,mDataNode);
					PreBuildFromDataNodes.Add(mDataNode);
				}
				if(CurrParseNode.Name=="TimePeriodPathAlias")
				{
					DataNode mDataNode=CreateDataNodeFor(CurrParseNode,null);
					if(mDataNode.Alias!=null)
						DataNodeAliases.Add(mDataNode.Alias,mDataNode);
					PreBuildFromDataNodes.Add(mDataNode);
				}

			}
			
			foreach(DataNode CurrDataNode in PreBuildFromDataNodes)
			{
				DataNode CurrHeaderDataNode=CurrDataNode.HeaderDataNode;
				if(DataNodeAliases.Contains(CurrHeaderDataNode.Name))
				{
					DataNode AliasDataNode=(DataNode)DataNodeAliases[CurrHeaderDataNode.Name];
					if(CurrHeaderDataNode.SubDataNodes.Count>0)
					{
						System.Collections.ArrayList TempSubDataNodes=new System.Collections.ArrayList(CurrHeaderDataNode.SubDataNodes);
						foreach(DataNode CurrSubDataNode in TempSubDataNodes)
							CurrSubDataNode.ParentDataNode=AliasDataNode;
					}
				}
				else
					DataNodesSource.Add(CurrDataNode.HeaderDataNode) ;
			}
		}
		/// <MetaDataID>{88063C84-06E5-431E-8C5C-827DE85C9E38}</MetaDataID>
		public StorageSession TranslateForStorageSession;

		/// <MetaDataID>{8044E206-C1FB-432E-AA0E-1A621951A897}</MetaDataID>
		protected void BuildWhereClauseDataNodes(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList WhereClausePaths=GetWhereClausePaths(SelectExpression);
			foreach(Parser.ParserNode CurrParseNode in WhereClausePaths)
			{
				string ClassOrClassAliasName= GetClassOrClassAliasName(CurrParseNode);
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(ClassOrClassAliasName))
					ParrentNode=(DataNode)DataNodeAliases[ClassOrClassAliasName];
				else 
					throw new System.Exception("The "+ClassOrClassAliasName+" hasn't declared");

				DataNode mDataNode=CreateDataNodeFor(CurrParseNode,ParrentNode);
				mDataNode.ParticipateInWereClause=true;
				WhereClauseDataNodes.Add(mDataNode); 

				mDataNode.AssignedParserNodes.Add(CurrParseNode);

				if(CurrParseNode.ParentNode.ParentNode.Name=="Criterion")
				{
					Parser.ParserNode Candidate_ObjectID= CurrParseNode.ParentNode.ParentNode.ChildNodes.GetAt(3).ChildNodes.GetFirst();
					if(Candidate_ObjectID.Name=="ObjectID")
					{
						if(!IsThereParentParseNodeWith2SearchTerm( CurrParseNode.ParentNode.ParentNode))
							mDataNode.ObjectIDParserNode=Candidate_ObjectID;
					}
				}
			}
		}
		/// <MetaDataID>{027451D2-D272-4B48-AF80-A89CAC032D5C}</MetaDataID>
		bool IsThereParentParseNodeWith2SearchTerm(Parser.ParserNode ParserNode)
		{
			if(ParserNode.Name=="search_condition")
			{
				if(ParserNode.ChildNodes.Count>=2)
					return true;
				else
				{
					if(ParserNode.ParentNode!=null)
						return IsThereParentParseNodeWith2SearchTerm(ParserNode.ParentNode);
					else
						return false;
				}
			}
			else
			{
				if(ParserNode.ParentNode!=null)
					return IsThereParentParseNodeWith2SearchTerm(ParserNode.ParentNode);
				else
					return false;
				
			}
		}
	

		/// <summary>Retrieve the data type of path. The type can be a class or a path to class which is declared with alias in from clause.</summary>
		/// <MetaDataID>{19BADF3D-05FB-467C-90E2-FAFF07DAAD7A}</MetaDataID>
		string GetClassOrClassAliasName(Parser.ParserNode ParserNode)
		{
			if(ParserNode.Name=="PathAlias")
							// PathAlias	/Path			/ClassOrAlias				/name
				return ParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).Value;
			else
				if(ParserNode.Name=="Path")
							//Path			/ClassOrAlias				/name
					return ParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(1).Value;
			throw new System.Exception("You can't retrieve name or alias name from the ParserNode other than 'Path' or 'PathAlias'");
			

		}
		protected string BuildOrderByClauseSQLQuery()
		{
			string OrderByClauseSQLQuery=null;
			System.Collections.ArrayList OrderByClausePaths=GetOrderByClausePaths(SelectQueryExpression);
			foreach(Parser.ParserNode CurrPath in OrderByClausePaths)
			{
				if(OrderByClauseSQLQuery==null)
					OrderByClauseSQLQuery="\nORDER BY ";
				else
					OrderByClauseSQLQuery+=" , ";

				DataNode CurrDataNode=(DataNode)PathDataNodeMap[CurrPath];

				OrderByClauseSQLQuery+="["+CurrDataNode.ParentDataNode.Alias+"].["+CurrDataNode.Name+"]";
				if(CurrPath.ParentNode.ChildNodes.Count==2) // Has ascending or descending definition
					OrderByClauseSQLQuery+=" "+CurrPath.ParentNode.ChildNodes.GetAt(2).ChildNodes.GetFirst().Value+" ";
			}
			return OrderByClauseSQLQuery;
		}

		protected void BuildOrderByClauseDataNodes(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList OrderByClausePaths=GetOrderByClausePaths(SelectExpression);
			foreach(Parser.ParserNode CurrParseNode in OrderByClausePaths)
			{
				string ClassOrClassAliasName= GetClassOrClassAliasName(CurrParseNode);
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(ClassOrClassAliasName))
					ParrentNode=(DataNode)DataNodeAliases[ClassOrClassAliasName];// error prone γιατί όχι μόνο alias
				else 
					throw new System.Exception("The "+ClassOrClassAliasName+" hasn't declared");
				DataNode mDataNode=CreateDataNodeFor(CurrParseNode,ParrentNode);
				mDataNode.AssignedParserNodes.Add(CurrParseNode);
			}
			int k=0;
		}
		protected System.Collections.ArrayList GetOrderByClausePaths(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList Paths=new System.Collections.ArrayList();
			Parser.ParserNode OrderBy=null;
			foreach(Parser.ParserNode CurrParserNode in SelectExpression.ChildNodes)
			{
				if(CurrParserNode.Name=="order_by_exp")
				{
					OrderBy=CurrParserNode;
					break;
				}
			}
			if(OrderBy!=null)
				GetPathsFromNode(OrderBy,Paths);
			return Paths;
		}

		/// <MetaDataID>{12C940C4-38E6-436A-9139-4471087868DC}</MetaDataID>
		protected void BuildSelectClauseDataNodes(Parser.ParserNode SelectExpression)
		{
			System.Collections.ArrayList SelectClausePaths=GetSelectClausePaths(SelectExpression);
			foreach(Parser.ParserNode CurrParseNode in SelectClausePaths)
			{
				string ClassOrClassAliasName= GetClassOrClassAliasName(CurrParseNode);
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(ClassOrClassAliasName))
					ParrentNode=(DataNode)DataNodeAliases[ClassOrClassAliasName];// error prone γιατί όχι μόνο alias
				else 
					throw new System.Exception("The "+ClassOrClassAliasName+" hasn't declared");
				DataNode mDataNode=CreateDataNodeFor(CurrParseNode,ParrentNode);
				mDataNode.AssignedParserNodes.Add(CurrParseNode);
			}

		}
		/// <MetaDataID>{B1E94682-992F-4558-9D31-7C576D8CBCF1}</MetaDataID>
		private System.Collections.Hashtable DataNodeAliases;
		/// <MetaDataID>{94F40A6B-E6D9-4060-8D66-EDA7FC374214}</MetaDataID>
		/// <summary>DataNodesSource defines a collection of data tree. 
		/// The data trees extracted from the "FROM" clause of OQL with the method RetrieveDataNodesSource. 
		/// On DataNodesSource referred the paths from "WHERE" clause for data filters 
		/// and the paths from "SELECT" clause for the selection of data you want to retrieve.</summary>
		private System.Collections.ArrayList DataNodesSource;

		/// <MetaDataID>{D97DFBCC-A76D-4293-8CD4-F9B1EF797656}</MetaDataID>
		private System.Collections.Hashtable PathDataNodeMap;

		/// <MetaDataID>{61E6716B-BE02-4F79-8D64-07B4B2D680AC}</MetaDataID>
		private string WhereClause;
		/// <MetaDataID>{4C7446C8-21FD-4295-A560-041E8327EC15}</MetaDataID>
		private string FromClause;

		/// <MetaDataID>{90D87771-67A8-4498-B416-BCC911259B3C}</MetaDataID>
		private Parser.Parser myParser;
		/// <MetaDataID>{E7ABF24E-7360-4056-83D4-1FD5932FAF9E}</MetaDataID>
		private string SelectClause;

		/// <MetaDataID>{B8613980-D076-4D0B-8A01-45E03F9D4EF6}</MetaDataID>
		private Parser.ParserNode SelectQueryExpression;
	
	
		/// <MetaDataID>{94B659BD-7785-4062-8798-04BED81A0D5F}</MetaDataID>
		public void Translate(string OQLExpretion,StorageSession aStorageSession)
		{
			bool HasSyntaxError=false;
			string CatchesErrorDescription=null;
			TranslateForStorageSession=aStorageSession;
			if(myParser==null)
			{
				myParser=new Parser.Parser();
			//	myParser.SetGrammarPath("G:\\PersistenceLayer\\OQLParser\\OQLParser.gmr");
				System.Type mType=GetType();
				string[] Resources=System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
				using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSSQLPersistenceRunTime.OQLGrammar"))
				{
					byte[] bytes = new byte[Grammar.Length];
					//NewFile.Write(
					Grammar.Read(bytes,0,(int)Grammar.Length);
					myParser.SetGrammar(bytes,(int)Grammar.Length);
					Grammar.Close();
				}
			}

			try
			{
				myParser.Parse(OQLExpretion);
			}
			catch(System.Exception Error)
			{
				System.Diagnostics.Debug.WriteLine(OQLExpretion);
				HasSyntaxError=true;
			}

			Parser.ParserNode QueryExpressionType=myParser.theRoot.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1);
			if(QueryExpressionType.Name=="Select_expression")			
			{
				try
				{
					SelectQueryExpression=QueryExpressionType;
					PathDataNodeMap=new System.Collections.Hashtable();
					DataNodeAliases=new System.Collections.Hashtable();
					DataNodesSource=new System.Collections.ArrayList();
					RetrieveDataNodesSource(QueryExpressionType);
					BuildSelectClauseDataNodes(QueryExpressionType);
					BuildWhereClauseDataNodes(QueryExpressionType);
					BuildOrderByClauseDataNodes(QueryExpressionType);
					foreach(DataNode CurrDataNode in DataNodesSource)
					{
						CurrDataNode.MergeIdenticalDataNodes();
						CurrDataNode.UpdateParserNodeDataNodeMap(PathDataNodeMap);
					}
				}
				catch(System.Exception Error)
				{
					CatchesErrorDescription=Error.Message;

				}
			}
			else
				throw new System.Exception("You can't execute query other than SELECT");

			string ErrorOutput=null;

			
			GetParserSyntaxErrors(QueryExpressionType,ref ErrorOutput);
			if(CatchesErrorDescription!=null)
				ErrorOutput+="\n"+CatchesErrorDescription;
		

			foreach(DataNode CurrDataNode in DataNodesSource)
			{
				MetaDataRepository.Namespace mNamespace=null;
				string Query ="SELECT Namespace FROM "+typeof(MetaDataRepository.Namespace).FullName+" Namespace WHERE Name = \""+CurrDataNode.Name+"\"";
				PersistenceLayer.StructureSet aStructureSet=PersistenceLayer.StorageSession.GetStorageOfObject(TranslateForStorageSession.StorageMetaData).Execute(Query );
				foreach( PersistenceLayer.StructureSet Rowset  in aStructureSet)
				{
					mNamespace =(MetaDataRepository.Namespace)Rowset.Members["Namespace"].Value;
					if(mNamespace.GetType()==typeof(MetaDataRepository.Namespace)) 
						break;
				}
				if(mNamespace==null)
					ErrorOutput+="There isn't namespace or class with name " +CurrDataNode.Name;
				CurrDataNode.AssignedMetaObject=mNamespace;
				CurrDataNode.IsValid(ref ErrorOutput);
			}
			if(ErrorOutput!=null)
				throw new System.Exception(ErrorOutput);
			else
				if(HasSyntaxError)
					throw new System.Exception("Syntax Error");
			LoadSelectListItems();
			foreach(DataNode CurrDataNode in DataNodesSource)
			{
				DataNode TimePeriodDataNode=CurrDataNode.GetDataNodeWithObjectConstraint();
				if(TimePeriodDataNode==null)
					TimePeriodDataNode=CurrDataNode.GetTimePeriodNodeIfExist();
				if(TimePeriodDataNode!=null)
					TimePeriodDataNode.BuildDataSource(null);
				else
					CurrDataNode.BuildDataSource(null);
			}


			FromClause=BuildFromClauseSQLQuery();
			WhereClause=BuildWhereClauseSQLQuery();

			foreach(DataNode CurrDataNode in WhereClauseDataNodes)
			{
				if(CurrDataNode.DataSource!=null)
					if(CurrDataNode.DataSource.Empty)
					{
						FromClause=WhereClause=SelectClause="";
						return ; //No data for query;
					}
			}

			SelectClause=BuildSelectClauseSQLQuery();

			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debug.WriteLine(BuildSelectClauseSQLQuery()+FromClause+WhereClause+BuildOrderByClauseSQLQuery());
		}

		/// <MetaDataID>{AFC96F1E-B631-45E8-8EA1-8CBC98D21600}</MetaDataID>
		public void GetParserSyntaxErrors(Parser.ParserNode ParserNode,ref string ErrorOutput)
		{

			if(ParserNode.Name=="Data_Selection_Syntax_Error")
			{
				ErrorOutput+="\nSyntax Error in SELECT Clause at line ("+ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value+ "): "+ParserNode.ParentNode.Value;
				return;

			}
			if(ParserNode.Name=="Data_Path_Syntax_Error")
			{
				ErrorOutput+="\nSyntax Error in FROM Clause at line ("+ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value+ "): "+ParserNode.ParentNode.Value;
				return;
			}
			if(ParserNode.Name=="Critiria_Exp_Syntax_Error")
			{
				ErrorOutput+="\nSyntax Error in WHERE Clause at line ("+ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value+ "): "+ParserNode.ParentNode.ParentNode.Value;
				return;
			}
			for(short i=0;i!=ParserNode.ChildNodes.Count;i++)
				GetParserSyntaxErrors(ParserNode.ChildNodes.GetAt(i+1),ref ErrorOutput);
		}
		/// <MetaDataID>{0F3B42B0-B49F-4C5D-9475-19D574D21F35}</MetaDataID>
		private void LoadSelectListItems() 
		{
			_SelectListItems=new System.Collections.ArrayList();
			System.Collections.ArrayList SelectClausePaths=GetSelectClausePaths(SelectQueryExpression);
			foreach(Parser.ParserNode CurrPath in SelectClausePaths)
			{
				DataNode dataNode=(DataNode)PathDataNodeMap[CurrPath];
				dataNode.ParticipateInSelectClause=true;
				_SelectListItems.Add(dataNode);
			}
				
		}
		
		/// <MetaDataID>{98B3D7C4-276A-4912-A821-91986780A6A9}</MetaDataID>
		public string GetObjectTypesQuery(DataNode SelectListItem )
		{
			if(SelectListItem.Type!=DataNode.DataNodeType.Object)
				throw new System.Exception("The type of select list item must be object");
			if(SelectListItem.DataSource.HasOutStorageCell)
				return "SELECT  DISTINCT ["+SelectListItem.Alias+"].[TypeID] ,["+SelectListItem.Alias+"].[OutObjCellID] "+FromClause +WhereClause;
			else
				return "SELECT  DISTINCT ["+SelectListItem.Alias+"].[TypeID] "+FromClause +WhereClause;

			
		}

		
		/// <MetaDataID>{377A5849-2050-445C-B3B0-EA7303598A6A}</MetaDataID>
		public string GetSQLQuery( )
		{
			return 	SelectClause+FromClause+WhereClause;
		}
		/// <MetaDataID>{7BA94C1E-2EEA-4FE1-9FC0-E75868F6D184}</MetaDataID>
		protected string BuildFromClauseSQLQuery()
		{
			string FromClauseSQLQuery=null;
			foreach(DataNode CurrDataNode in DataNodesSource)
			{
				if(FromClauseSQLQuery!=null)
					FromClauseSQLQuery+=",";
				FromClauseSQLQuery+=CurrDataNode.BuildFromClauseSQLQuery();
			}
			return "\nFROM "+FromClauseSQLQuery;
		}

		/// <MetaDataID>{A3C9E34D-D032-415D-88AE-0B4A053F1ACB}</MetaDataID>
		protected string BuildSelectClauseSQLQuery()
		{
			string SelectClauseSQLQuery=null;
			System.Collections.ArrayList SelectClausePaths=GetSelectClausePaths(SelectQueryExpression);
			foreach(DataNode CurrPathDataNode in SelectListItems)
			{
				if(CurrPathDataNode.Type==DataNode.DataNodeType.OjectAttribute)
				{
					if(SelectClauseSQLQuery!=null)
						SelectClauseSQLQuery+=",";
					SelectClauseSQLQuery+="["+CurrPathDataNode.ParentDataNode.Alias+"]";
					SelectClauseSQLQuery+=".";
					SelectClauseSQLQuery+="["+CurrPathDataNode.Name+"]";
					if(CurrPathDataNode.Alias!=null)
						SelectClauseSQLQuery+=" as ["+CurrPathDataNode.Alias+"]";
				} 
				else if(CurrPathDataNode.Type==DataNode.DataNodeType.Object)
				{
								
					foreach(string CurrColumnName in CurrPathDataNode.DataSource.SelectColumnsNames)
					{
						if(SelectClauseSQLQuery!=null)
							SelectClauseSQLQuery+=",";
						SelectClauseSQLQuery+="["+CurrPathDataNode.Alias+"]";
						SelectClauseSQLQuery+=".";
						SelectClauseSQLQuery+="["+CurrColumnName+"]";
						SelectClauseSQLQuery+=" as ["+CurrColumnName+CurrPathDataNode.GetHashCode().ToString()+"]";
					}
					if(CurrPathDataNode.DataSource.HasOutStorageCell)
					{
						SelectClauseSQLQuery+=",["+CurrPathDataNode.Alias+"]";
						SelectClauseSQLQuery+=".[OutObjCellID]"+ " as [OutObjCellID"+CurrPathDataNode.GetHashCode().ToString()+"]";
					}
				} 
				else
					throw new System.Exception("The select list item "+CurrPathDataNode.FullName+" isn't class or class member.");

			}
			return "SELECT DISTINCT "+SelectClauseSQLQuery;
			
			
		}

	}
}
/*
 * SELECT product 
 * FROM Product product,Attribute attribute
 * WHERE product.Attributes.Contains(ALL(attribute.Type.Name=MagneticMassStorageObject) or ANYOF({attribute.Name='writeable'})) and  product.Name='kitsos'
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
