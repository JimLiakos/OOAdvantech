namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{CE0ACD87-789C-4037-8B46-162B9ADFBCD6}</MetaDataID>
	public class OQLStatement:MetaDataRepository.ObjectQueryLanguage.OQLStatement
	{
        public Collections.Generic.Dictionary<int,RDBMSMetaDataRepository.StorageCell> StorageCells =new OOAdvantech.Collections.Generic.Dictionary<int,OOAdvantech.RDBMSMetaDataRepository.StorageCell>();

		public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ComparisonTerm CreateComparisonTerm(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType type, Parser.ParserNode comparisonTermParserNode)
		{
            return base.CreateComparisonTerm(type, comparisonTermParserNode);
				
            //if(type==MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType.Object)
            //    return new ObjectComparisonTerm(comparisonTermParserNode,this);

            //if(type==MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType.ObjectAttribute)
            //    return new ObjectAttributeComparisonTerm(comparisonTermParserNode,this);

            //if(type==MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType.ObjectID)
            //    return base.CreateComparisonTerm(type,comparisonTermParserNode);

            //if(type==MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType.Literal)
            //    return new LiteralComparisonTerm(comparisonTermParserNode,this);

            //if(type==MetaDataRepository.ObjectQueryLanguage.ComparisonTerm.ComparisonTermType.Parameter)
            //    return new ParameterComparisonTerm(comparisonTermParserNode,this);
			return null;
		}

		public OQLStatement(OOAdvantech.Collections.Hashtable parameters):base(parameters)
		{

		}
        public string SelectList = null;

		/// <MetaDataID>{533838F6-2FFA-4ED0-9D55-1E1B86963B68}</MetaDataID>
		protected string BuildSelectClauseSQLQuery()
		{
			string SelectClauseSQLQuery=null;
			System.Collections.ArrayList SelectClausePaths=new System.Collections.ArrayList();
            GetPathsFromNode(SelectQueryExpression["Select_Clause"]["Select_List"] as Parser.ParserNode, SelectClausePaths);
			foreach(DataNode dataNode in SelectListItems)
			{
				if(dataNode.Type==DataNode.DataNodeType.OjectAttribute)
				{
					if(SelectClauseSQLQuery!=null)
						SelectClauseSQLQuery+=",";
					SelectClauseSQLQuery+="["+dataNode.ParentDataNode.Alias+"]";
					SelectClauseSQLQuery+=".";
					SelectClauseSQLQuery+="["+dataNode.Name+"]";
					if(dataNode.Alias!=null)
						SelectClauseSQLQuery+=" as ["+dataNode.Alias+"]";
				} 
				else if(dataNode.Type==DataNode.DataNodeType.Object)
				{
								
					foreach(string CurrColumnName in dataNode.DataSource.SelectColumnsNames)
					{
						if(SelectClauseSQLQuery!=null)
							SelectClauseSQLQuery+=",";
						SelectClauseSQLQuery+="["+dataNode.Alias+"]";
						SelectClauseSQLQuery+=".";
						SelectClauseSQLQuery+="["+CurrColumnName+"]";
						SelectClauseSQLQuery+=" as ["+CurrColumnName+dataNode.GetHashCode().ToString()+"]";
					}
				} 
				else
					throw new System.Exception("The select list item "+dataNode.FullName+" isn't class or class member.");

			}

			return "SELECT DISTINCT "+SelectList +" into [Table]";
		}
        public string GetDataGroupsSQLQueries(ref System.Collections.Generic.List<DataNode> tableDataNodes)
        {
            string dataGroupsSQLQueries=null;

            foreach (DataNode dataNode in DataTrees)
            {
                dataNode.GetDataGroupsSQLQueries(ref dataGroupsSQLQueries,ref tableDataNodes);
            }
            if (dataGroupsSQLQueries != null)
                dataGroupsSQLQueries = "\n" + dataGroupsSQLQueries + "\n drop table [table]";
            return dataGroupsSQLQueries;
        }

		/// <MetaDataID>{FB75DFF2-84D5-4F33-9CAF-507D103B3BB1}</MetaDataID>
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
        public string GetSQLQuery(ref System.Collections.Generic.List<DataNode> tableDatanodes)
		{
            return SelectClause + FromClause + WhereClause + GetDataGroupsSQLQueries(ref tableDatanodes);
		}

		public override void Build(string OQLExpretion, OOAdvantech.PersistenceLayer.ObjectStorage objectStorage)
		{
            base.Build(OQLExpretion, objectStorage);
            return;
            //foreach (DataNode dataNode in DataTrees)
            //    dataNode.BuildDataSource();

            //FromClause=BuildFromClauseSQLQuery();
            //if(SearchCondition!=null)
            //    WhereClause="\nWHERE "+(SearchCondition as SearchCondition).SQLExpression;;
            //SelectClause=BuildSelectClauseSQLQuery();
			
            //if(System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debug.WriteLine(BuildSelectClauseSQLQuery()+FromClause+WhereClause+BuildOrderByClauseSQLQuery());

		}

		private string FromClause;
		private string WhereClause;
		private string SelectClause;


		/// <MetaDataID>{87CCF6DC-FAAF-4318-8087-D6D382CA92F1}</MetaDataID>
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
		protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode CreateDataNodeFor(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path path, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode ParentDataNode)
		{
			MetaDataRepository.ObjectQueryLanguage.DataNode dataNode=new MetaDataRepository.ObjectQueryLanguage.DataNode(this,path);
			
			dataNode.ParentDataNode=ParentDataNode;
			if(path.SubPath!=null)
				dataNode=CreateDataNodeFor(path.SubPath,dataNode) ;
			return dataNode;

		}
		protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode CreateDataNodeFor(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.PathHead path, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode aliasDataNode)
		{
			MetaDataRepository.ObjectQueryLanguage.DataNode dataNode=null;
			if(aliasDataNode!=null)
			{
				dataNode=aliasDataNode;
				if(path.SubPath!=null)
					dataNode=CreateDataNodeFor(path.SubPath,dataNode);
			}
			else
			{
				dataNode=new MetaDataRepository.ObjectQueryLanguage.DataNode(this,path);
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



        internal void BuildTablesRelations(System.Data.DataSet dataSet)
        {
            foreach (DataNode dataNode in DataTrees)
                dataNode.BuildTablesRelations(dataSet);
            
        }
    }
}
