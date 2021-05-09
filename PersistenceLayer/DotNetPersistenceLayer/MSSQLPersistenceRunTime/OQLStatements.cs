namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{F6E4C6EB-2E9C-46FC-B99A-C04DFD47C011}</MetaDataID>
	internal class OQLStatement
	{
		/// <MetaDataID>{EE96B750-9ECC-438B-A1D4-00CC3970B3F1}</MetaDataID>
		public SearchCondition SearchCondition;
		/// <MetaDataID>{8AB37F5C-DED5-4B71-9F1F-3DE282C9BFD6}</MetaDataID>
		private void LoadSelectListItems()
		{
		
		}
		/// <MetaDataID>{4FB8C100-0C81-4877-95D6-57BEA0C6DB5C}</MetaDataID>
		internal OOAdvantech.Collections.Map PathDataNodeMap=new OOAdvantech.Collections.Map();
		/// <MetaDataID>{62FDBAD1-8C6B-4107-A9C4-0D990CFC0C0F}</MetaDataID>
		internal OOAdvantech.Collections.Map DataNodeAliases=new OOAdvantech.Collections.Map();
		/// <MetaDataID>{C4D7D39E-CC1F-461A-A97C-2335C8A3DA9C}</MetaDataID>
		internal OOAdvantech.Collections.Map Parameters;
		/// <MetaDataID>{60AC46E1-8EB0-44B9-98A5-47E5B00A2241}</MetaDataID>
		 public OQLStatement(OOAdvantech.Collections.Map parameters)
		{
			Parameters=parameters;
		}

		/// <MetaDataID>{88063C84-06E5-431E-8C5C-827DE85C9E38}</MetaDataID>
		public ObjectStorage ObjectStorage;

		/// <MetaDataID>{B8613980-D076-4D0B-8A01-45E03F9D4EF6}</MetaDataID>
		private Parser.ParserNode SelectQueryExpression;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{932A6E0E-E1C0-4AAF-B812-CB599DD6F933}</MetaDataID>
		private Parser.Parser _OQLParser;
		/// <MetaDataID>{90D87771-67A8-4498-B416-BCC911259B3C}</MetaDataID>
		private Parser.Parser OQLParser
		{
			get
			{
				if(_OQLParser==null)
				{
					_OQLParser=new Parser.Parser();
					//	myParser.SetGrammarPath("G:\\PersistenceLayer\\OQLParser\\OQLParser.gmr");
					System.Type mType=GetType();
					string[] Resources=System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
					using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSSQLPersistenceRunTime.OQLGrammar"))
					{
						byte[] bytes = new byte[Grammar.Length];
						//NewFile.Write(
						Grammar.Read(bytes,0,(int)Grammar.Length);
						_OQLParser.SetGrammar(bytes,(int)Grammar.Length);
						Grammar.Close();
					}
				}
				return _OQLParser;
			}
		}
		/// <summary>DataTrees defines a collection of rootes of data trees. 
		/// The data trees extracted from the "FROM" clause of OQL with the method RetrieveDataTrees.
		/// On DataTree referred the paths from "WHERE" clause for data filters 
		/// and paths from "SELECT" clause for data  selection. </summary>
		/// <MetaDataID>{94F40A6B-E6D9-4060-8D66-EDA7FC374214}</MetaDataID>
		private System.Collections.ArrayList DataTrees=new System.Collections.ArrayList();
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{48228186-84D5-4E71-994D-5D76687A9EC7}</MetaDataID>
		private System.Collections.ArrayList _SelectListItems=new System.Collections.ArrayList();
		/// <MetaDataID>{51550934-EEAF-47EA-BB95-CFE404EAB34C}</MetaDataID>
		internal System.Collections.ArrayList SelectListItems
		{
			get
			{
				
				if(SelectQueryExpression==null)
					throw new System.Exception("There isn't translated oql statement");
				return _SelectListItems;
			
			}
		}
		/// <summary>The UsedDataSourceAliases collection keeps all data source alias names.
		/// In data tree there is alias name for all data source . 
		/// The alias name must be unique in the OQLStatement.
		/// The OQLStatement look at the collection when produce alias names. </summary>
		/// <MetaDataID>{A9A56329-56C9-446C-A1F8-5385B774EEA2}</MetaDataID>
		private System.Collections.ArrayList UsedAliases=new System.Collections.ArrayList();
		/// <MetaDataID>{E7818100-D52D-4A7D-B1C0-D10C6EB1FFED}</MetaDataID>
		public System.Collections.ArrayList WhereClauseDataNodes=new System.Collections.ArrayList();
		/// <MetaDataID>{4C7446C8-21FD-4295-A560-041E8327EC15}</MetaDataID>
		private string FromClause;
		/// <MetaDataID>{61E6716B-BE02-4F79-8D64-07B4B2D680AC}</MetaDataID>
		private string WhereClause;
		/// <MetaDataID>{E7ABF24E-7360-4056-83D4-1FD5932FAF9E}</MetaDataID>
		private string SelectClause;
		/// <MetaDataID>{782C71FC-674F-4144-8A7C-C737218EA789}</MetaDataID>
		internal string GetValidAlias(string proposedAlias)
		{
			
			string ValidAlias=null;
			
			ValidAlias=proposedAlias;
			int Count=0;
			//while(UsedTableNames.Contains(ValidTableName))
			while(UsedAliases.Contains(ValidAlias))
			{
				Count++;
				ValidAlias=proposedAlias+"_"+Count.ToString();
			}
			UsedAliases.Add(ValidAlias);
			return ValidAlias;
		}
		internal void BookAlias(string alias)
		{
			if(!UsedAliases.Contains(alias))
				UsedAliases.Add(alias);

		}
	
		/// <MetaDataID>{21558B7A-C754-4898-990C-03348BBC1597}</MetaDataID>
		protected DataNode CreateDataNodeFor(Path path, DataNode ParentDataNode)
		{
			DataNode dataNode=new DataNode(this,path);
			
			dataNode.ParentDataNode=ParentDataNode;
			if(path.SubPath!=null)
				dataNode=CreateDataNodeFor(path.SubPath,dataNode);
			return dataNode;



		}
		/// <MetaDataID>{0FB6EDDC-BC11-48DF-81E4-C599BE2F108B}</MetaDataID>
		protected DataNode CreateDataNodeFor(PathHead path, DataNode aliasDataNode)
		{

			DataNode dataNode=null;
			if(aliasDataNode!=null)
			{
				dataNode=aliasDataNode;
				if(path.SubPath!=null)
					dataNode=CreateDataNodeFor(path.SubPath,dataNode);
			}
			else
			{
				dataNode=new DataNode(this,path);
				if(path.SubPath!=null)
					dataNode=CreateDataNodeFor(path.SubPath,dataNode);
				
			}
			if(path.HasTimePeriodConstrain)
			{
				dataNode.TimePeriodStartDate=path.TimePeriodStartDate;
				dataNode.TimePeriodEndDate=path.TimePeriodEndDate;
			}
			return dataNode;
		}
		/// <MetaDataID>{77DE53D8-27C7-41D5-B45B-1E2254A8AF57}</MetaDataID>
		protected string BuildFromClauseSQLQuery()
		{
			string FromClauseSQLQuery=null;
			foreach(DataNode CurrDataNode in DataTrees)
			{
				if(FromClauseSQLQuery!=null)
					FromClauseSQLQuery+=",";
				FromClauseSQLQuery+=CurrDataNode.BuildFromClauseSQLQuery();
			}
			return "\nFROM "+FromClauseSQLQuery;
		}
		/// <MetaDataID>{BF5D88D5-09D4-4660-AE52-672AEDF8866F}</MetaDataID>
		protected void RetrieveWhereClauseDataNodes()
		{
	
			System.Collections.ArrayList WhereClausePaths=new System.Collections.ArrayList();
			if(SelectQueryExpression["Critiria_exp"]!=null)
				GetPathsFromNode(SelectQueryExpression["Critiria_exp"] as Parser.ParserNode,WhereClausePaths);

			foreach(PathHead pathHead in WhereClausePaths)
			{
				
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(pathHead.Name))
					ParrentNode=(DataNode)DataNodeAliases[pathHead.Name];
				else 
				{
					if(pathHead.AliasName!=null)
						throw new System.Exception("The "+pathHead.AliasName+" hasn't declared");
					else
						throw new System.Exception("The "+pathHead.Name+" hasn't declared");

				}

				DataNode mDataNode=CreateDataNodeFor(pathHead,ParrentNode);
				mDataNode.ParticipateInWereClause=true;
				WhereClauseDataNodes.Add(mDataNode); 

				mDataNode.RelatedPaths.Add(pathHead);//.ParserNode);


				if(pathHead.ParserNode.ParentNode.ParentNode.Name=="Criterion")
					mDataNode.Restrictions.Add(pathHead.ParserNode.ParentNode.ParentNode);
				
					
			}
		}
		/// <MetaDataID>{BE6C18CD-192D-4292-A7DB-D72D2713AA84}</MetaDataID>
		public string GetSQLQuery( )
		{

			return 	SelectClause+FromClause+WhereClause;
		}
		/// <MetaDataID>{4D3F43CB-B22C-4644-B09E-42A82E8DBBB6}</MetaDataID>
		internal void AddSelectListItem(DataNode SelectListItem)
		{
			_SelectListItems.Add(SelectListItem);
			SelectListItem.ParticipateInSelectClause=true;
		}
		/// <MetaDataID>{31862367-C2D1-4466-9007-F29104B28118}</MetaDataID>
		internal void RemoveSelectListItem(DataNode SelectListItem)
		{
			_SelectListItems.Remove(SelectListItem);
			SelectListItem.ParticipateInSelectClause=false;
		}
	
	
		/// <MetaDataID>{3F00A2B9-05B6-470B-ADC7-D5160194D816}</MetaDataID>
		protected void RetrieveSelectClauseDataNodes()
		{
		
			System.Collections.ArrayList SelectPaths=new System.Collections.ArrayList();
			GetPathsFromNode(SelectQueryExpression["Select_claue"] as Parser.ParserNode,SelectPaths);
		
			foreach(PathHead pathHead in SelectPaths)
			{
				
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(pathHead.Name))
					ParrentNode=(DataNode)DataNodeAliases[pathHead.Name];// error prone γιατί όχι μόνο alias
				else 
				{
					if(pathHead.AliasName!=null)
						throw new System.Exception("The "+pathHead.AliasName+" hasn't declared");
					else
						throw new System.Exception("The "+pathHead.Name+" hasn't declared");

				}
				DataNode dataNode=CreateDataNodeFor(pathHead,ParrentNode);
				dataNode.RelatedPaths.Add(pathHead);//.ParserNode);
				dataNode.ParticipateInSelectClause=true;
				_SelectListItems.Add(dataNode);

			}

		}
		/// <MetaDataID>{6E0635A8-8604-4932-8E2A-9C4CB5E9891A}</MetaDataID>
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

		/// <MetaDataID>{E4AA8EA4-1E29-49EA-8E0D-2235B43C1CEB}</MetaDataID>
		protected void RetrieveDataTrees()
		{
			System.Collections.ArrayList DataTreePaths=new System.Collections.ArrayList();
			GetPathsFromNode(SelectQueryExpression["objectcollection_exp"] as Parser.ParserNode,DataTreePaths);
			System.Collections.ArrayList DataNodes=new System.Collections.ArrayList();

			foreach(PathHead pathHead in DataTreePaths)
			{
				DataNode dataNode=CreateDataNodeFor(pathHead,null);
				if(dataNode.Alias!=null)
					DataNodeAliases.Add(dataNode.Alias,dataNode);
				DataNodes.Add(dataNode);
			}

			//build data trees and link alias data trees with the main data trees
			//For example for 
			//FORM Family.Person person,person.Address personAddress ,person.Childrens personChildrens,person.Parents personParents,person.Parents.Address parentsAdress
			//we retrieve five DataNodes "person,personAddress,personChildrens,personParents,parentsAddress
			//and produce one data tree
			//Family
			//  |
			//	|_Person	[person]
			//		|
			//		|___Address		[personAddress]
			//		|  
			//		|___Childrens	[personChildrens]
			//		|
			//		|___Parents		[personParents]
			//				|	
			//				|___Address	[parentsAdress]


			foreach(DataNode dataNode in DataNodes)
			{
				DataNode headerDataNode=dataNode.HeaderDataNode;
				if(DataNodeAliases.Contains(headerDataNode.Name))
				{
					DataNode aliasDataNode=(DataNode)DataNodeAliases[headerDataNode.Name];
					//link an alias based DataNode for example the Family.Person person with person.Address personAddress
					if(headerDataNode.SubDataNodes.Count>0)
					{
						System.Collections.ArrayList subDataNodes=new System.Collections.ArrayList(headerDataNode.SubDataNodes);
						foreach(DataNode subDataNode in subDataNodes)
							subDataNode.ParentDataNode=aliasDataNode;
					}
				}
				else
					DataTrees.Add(dataNode.HeaderDataNode) ;
			}
		}

	
		
		/// <MetaDataID>{BAA30997-B778-4511-B8F0-E08C7A77DBF5}</MetaDataID>
		protected void GetPathsFromNode(Parser.ParserNode ParserNode,System.Collections.ArrayList Paths )
		{
			if(ParserNode.Name=="PathAlias"||ParserNode.Name=="Path" ||ParserNode.Name=="TimePeriodPathAlias")
				Paths.Add(new PathHead(ParserNode));
			else
			{
				for(short i=1;i<ParserNode.ChildNodes.Count+1;i++)
					GetPathsFromNode(ParserNode.ChildNodes.GetAt(i),Paths);
			}
		}
		/// <MetaDataID>{67D6B38D-0B56-4A3F-9C6D-16E4ABC43A67}</MetaDataID>
		protected string BuildSelectClauseSQLQuery()
		{
			string SelectClauseSQLQuery=null;
			System.Collections.ArrayList SelectClausePaths=new System.Collections.ArrayList();
			GetPathsFromNode(SelectQueryExpression["Select_claue"]["select_list"] as Parser.ParserNode,SelectClausePaths);
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
				} 
				else
					throw new System.Exception("The select list item "+CurrPathDataNode.FullName+" isn't class or class member.");

			}
			return "SELECT DISTINCT "+SelectClauseSQLQuery;
			
			
		}

		/// <MetaDataID>{679E413F-D02E-4355-A393-A2A33E655A08}</MetaDataID>
		public void Translate(string OQLExpretion, ObjectStorage objectStorage)
		{
			bool HasSyntaxError=false;
			string CatchesErrorDescription=null;
			ObjectStorage=objectStorage;
			try
			{
				OQLParser.Parse(OQLExpretion);
				int trt=0;
			}
			catch(System.Exception Error)
			{
				System.Diagnostics.Debug.WriteLine(OQLExpretion);
				HasSyntaxError=true;
			}
			
			Parser.ParserNode Select_expression=OQLParser.theRoot["start"]["OQLStatament"]["query_expression"]["Select_expression"] as Parser.ParserNode;
			string ErrorOutput=null;
			GetParserSyntaxErrors(Select_expression,ref ErrorOutput);
				
			try
			{
				SelectQueryExpression=Select_expression;
				RetrieveDataTrees();
				RetrieveSelectClauseDataNodes();
				RetrieveWhereClauseDataNodes();

				RetrieveOrderByClauseDataNodes();
				foreach(DataNode CurrDataNode in DataTrees)
					CurrDataNode.BuildDataNodeTree((ObjectStorage.StorageMetaData as Storage),ref ErrorOutput);


				if(SelectQueryExpression["Critiria_exp"]!=null)
					SearchCondition=new SearchCondition(SelectQueryExpression["Critiria_exp"]["search_condition"] as Parser.ParserNode,this);

				foreach(DataNode CurrDataNode in DataTrees)
					CurrDataNode.BuildDataSource();

			}
			catch(System.Exception Error)
			{
				CatchesErrorDescription=Error.Message;
			}
			if(CatchesErrorDescription!=null)
				ErrorOutput+="\n"+CatchesErrorDescription;

			if(ErrorOutput!=null)
				throw new System.Exception(ErrorOutput);
			else
				if(HasSyntaxError)
					throw new System.Exception("Syntax Error");

			FromClause=BuildFromClauseSQLQuery();
			if(SearchCondition!=null)
				WhereClause="\nWHERE "+SearchCondition.SQLExpression;;
			SelectClause=BuildSelectClauseSQLQuery();

			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debug.WriteLine(BuildSelectClauseSQLQuery()+FromClause+WhereClause+BuildOrderByClauseSQLQuery());
		}

		/// <MetaDataID>{E678278C-34BB-4F4A-A915-DCA9803248DE}</MetaDataID>
		protected string BuildOrderByClauseSQLQuery()
		{
			string OrderByClauseSQLQuery=null;
			
			System.Collections.ArrayList OrderByClausePaths=new System.Collections.ArrayList();
			if(SelectQueryExpression["order_by_exp"]!=null)
				GetPathsFromNode(SelectQueryExpression["order_by_exp"] as Parser.ParserNode,OrderByClausePaths);

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


		/// <MetaDataID>{8995124A-C0F4-4F0C-822D-A9D1E6BA55BD}</MetaDataID>
		protected void RetrieveOrderByClauseDataNodes()
		{
			System.Collections.ArrayList OrderByClausePaths=new System.Collections.ArrayList();
			if(SelectQueryExpression["order_by_exp"]!=null)
				GetPathsFromNode(SelectQueryExpression["order_by_exp"] as Parser.ParserNode,OrderByClausePaths);

			foreach(PathHead pathHead in OrderByClausePaths)
			{
				
				DataNode ParrentNode=null;
				if(DataNodeAliases.Contains(pathHead.Name))
					ParrentNode=(DataNode)DataNodeAliases[pathHead.Name];// error prone γιατί όχι μόνο alias
				else 
				{
					if(pathHead.AliasName!=null)
						throw new System.Exception("The "+pathHead.AliasName+" hasn't declared");
					else
						throw new System.Exception("The "+pathHead.Name+" hasn't declared");
				}
				DataNode mDataNode=CreateDataNodeFor(pathHead,ParrentNode);
				mDataNode.RelatedPaths.Add(pathHead);//.ParserNode);
			}
			int k=0;
		}



//		/// <MetaDataID>{A870B90F-51F9-436A-9CBC-DB6F995525E7}</MetaDataID>
//		private void LoadSelectListItems() 
//		{
//			_SelectListItems=new System.Collections.ArrayList();
//			System.Collections.ArrayList SelectClausePaths=new System.Collections.ArrayList();
//			GetPathsFromNode(SelectQueryExpression["Select_claue"]["select_list"] as Parser.ParserNode,SelectClausePaths);
//			foreach(PathHead pathHead in SelectClausePaths)
//			{
//				DataNode dataNode=(DataNode)PathDataNodeMap[pathHead .ParserNode];
//				dataNode.ParticipateInSelectClause=true;
//				_SelectListItems.Add(dataNode);
//			}
//		}
	}
}
