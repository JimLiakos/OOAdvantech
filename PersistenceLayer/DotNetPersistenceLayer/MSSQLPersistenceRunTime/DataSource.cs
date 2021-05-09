namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{22CF50EA-C36E-4023-A453-E65403BD9EF6}</MetaDataID>
	public class DataSource
	{
		
		struct ReferenceColumn
		{
			public string ColumnName;
			public string ColumnNameInRefernceTable;
			public string ReferenceColumnName;
			public string RefernceTableName;
		}
		/// <MetaDataID>{FD854071-6974-4658-B0E7-4BFD907D7746}</MetaDataID>
		private System.Collections.Hashtable ReferenceColumns=null;

		/// <MetaDataID>{1C584769-B7E1-4207-812D-96626340D9A7}</MetaDataID>
		System.Collections.ArrayList GetSelectColumnsNamesFromView()
		{
			if(_SelectColumnsNames!=null)
				return _SelectColumnsNames;

			_SelectColumnsNames=new System.Collections.ArrayList();
			if(DataNode!=null&&DataNode.Type==DataNode.DataNodeType.Object&&DataNode.ObjectQuery.SelectListItems.Contains(DataNode)&&View.SubViews.Count>0)
			{
				foreach(RDBMSMetaDataRepository.View CurrView in View.SubViews)
				{
					foreach(string CurrColumn in CurrView.ViewColumnsNames)
					{
						if(!_SelectColumnsNames.Contains(CurrColumn))
							_SelectColumnsNames.Add(CurrColumn);
					}
				}
                _SelectColumnsNames.Add("StorageCellHashCode");
			}
			else
			{
				foreach(string CurrColumn in View.ViewColumnsNames)
				{
					if(!_SelectColumnsNames.Contains(CurrColumn))
						_SelectColumnsNames.Add(CurrColumn);
				}
			}

			return _SelectColumnsNames;

		}
		/// <MetaDataID>{9F28E9BB-8D2E-436F-BD02-2287AF6D9FB6}</MetaDataID>
		public OOAdvantech.RDBMSMetaDataRepository.View View;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{CB971830-7B86-4428-9FDC-574A3F4A2133}</MetaDataID>
		private System.Collections.ArrayList  _SelectColumnsNames;
		/// <MetaDataID>{FF0C325A-EF80-4B38-9F26-B9A1E00CD9B8}</MetaDataID>
		public System.Collections.ArrayList  SelectColumnsNames
		{
			get
			{
				System.Collections.ArrayList columnsNames=new System.Collections.ArrayList(GetSelectColumnsNamesFromView());
				if(ReferenceColumns!=null)
				{
					foreach(System.Collections.DictionaryEntry entry in ReferenceColumns)
					{
						ReferenceColumn refColumn=(ReferenceColumn)entry.Value;
						columnsNames.Add("ref_"+refColumn.ColumnName);
					}
				}

				return columnsNames;
			}		
		}
		/// <MetaDataID>{B97082E3-2ADE-4BBB-9C7E-FA99A94E896F}</MetaDataID>
		public DataNode DataNode;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{73EC33CD-044C-4896-BC09-2AEA4689C58D}</MetaDataID>
		private string _SQLStatament;
		/// <MetaDataID>{54545831-F306-496A-B4A8-04063AA00DBF}</MetaDataID>
		public string SQLStatament
		{
			get
			{
				if(_SQLStatament!=null)
					return _SQLStatament;
				if(HasOutStorageCell)
					if(DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
						AddOutStorageColumn("StorageCellID","","","");
				_SQLStatament=null;
				if(View.SubViews.Count>0)
				{
                    foreach (RDBMSMetaDataRepository.View CurrSubView in View.SubViews)
                    {
                        if (_SQLStatament != null)
                            _SQLStatament += "\nUNION ALL \n";
                        else
                            _SQLStatament = "(";
                        RDBMSMetaDataRepository.StorageCell storageCell = null;
                        if (View.ViewStorageCell != null && View.ViewStorageCell.ContainsKey(CurrSubView))
                        {
                            storageCell = View.ViewStorageCell[CurrSubView];
                            //(this.DataNode.ObjectQuery as OQLStatement).StorageCells.Add(storageCell.GetHashCode(), storageCell);
                        }
                        _SQLStatament += BuildSelectQuery(CurrSubView,storageCell);
                    }
				}
				else
				{
					string whereClause=null;
					foreach(RDBMSMetaDataRepository.Column column in View.ViewColumns)
					{
                        if(_SelectColumnsNames==null)
                            _SelectColumnsNames=new System.Collections.ArrayList();
                        _SelectColumnsNames.Add(column.Name);
						string nullValue="NULL";
						if(column.Type!=null)
							nullValue=OOAdvantech.MSSQLPersistenceRunTime.TypeDictionary.GetDBNullValue(column.Type.FullName);

						if(_SQLStatament==null)
						{
							whereClause="WHERE "+column.Name+" <> "+nullValue;
							_SQLStatament+="(SELECT "+nullValue +" AS "+column.Name;
						}
						else
							_SQLStatament+=","+nullValue +" AS "+column.Name;
					}
					_SQLStatament="(SELECT * FROM "+_SQLStatament+") [TABLE] "+whereClause;
				}
				_SQLStatament+=")";

                AppendSelectItem();

				return _SQLStatament;
			}
		}

        public string DataSourceSelectList = null;

        public System.Collections.Generic.List<string> SelectListColumnsNames = new System.Collections.Generic.List<string>(); 


        private void AppendSelectItem()
        {
            return;
 
            if (DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                foreach (string columnName in _SelectColumnsNames)
                {
                    if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ",";


                    (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.Alias + "]";
                    (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                    (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                    (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.GetHashCode().ToString() + "]";


                    DataNode currDataNode = DataNode;

                    while (currDataNode != null && 
                        currDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                        (!(currDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany || currDataNode.ParentDataNode.Classifier.LinkAssociation == (currDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association))
                        currDataNode = currDataNode.ParentDataNode as DataNode;

                    if (currDataNode.DataSource.DataSourceSelectList != null)
                        currDataNode.DataSource.DataSourceSelectList += ",";

                    currDataNode.DataSource.DataSourceSelectList += " [" + columnName + DataNode.GetHashCode().ToString() + "]";
                    currDataNode.DataSource.SelectListColumnsNames.Add("[" + columnName + DataNode.GetHashCode().ToString() + "]");
                }

                if (!DataNode.ObjectQuery.SelectListItems.Contains(DataNode.ParentDataNode)&&DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && 
                    (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                {
                    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    //DataNode.LazyFetching = true;

                    foreach (RDBMSMetaDataRepository.Column column in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(DataNode.ParentDataNode.Classifier))
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.ParentDataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                        if ((DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList != null)
                            DataSourceSelectList += ",";

                        (DataNode.ParentDataNode as DataNode).DataSource.SelectListColumnsNames.Add("[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]");
                        (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += "[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                    }
                    foreach (RDBMSMetaDataRepository.Column column in (DataNode.ParentDataNode as DataNode).Classifier.ObjectIDColumns)
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.ParentDataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                        if ((DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList != null)
                            (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += ",";

                        (DataNode.ParentDataNode as DataNode).DataSource.SelectListColumnsNames.Add("[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]");
                        (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += " [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                    }
                }
            }
            else
            {
                //foreach (DataNode subDataNode in DataNode.SubDataNodes)
                //{
                //    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                //        DataNode.LazyFetching = true;

                //}
                if(DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd&&(DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                {
                    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    //DataNode.LazyFetching = true;

                    foreach (RDBMSMetaDataRepository.Column column in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(DataNode.ParentDataNode.Classifier))
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.ParentDataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";
                        
                        if ((DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList != null)
                            DataSourceSelectList += ",";

                        (DataNode.ParentDataNode as DataNode).DataSource.SelectListColumnsNames.Add("[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]");
                        (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += "[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                    }


                    foreach (RDBMSMetaDataRepository.Column column in (DataNode.ParentDataNode as DataNode).Classifier.ObjectIDColumns)
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.ParentDataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                        if ((DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList != null)
                            (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += ",";

                        (DataNode.ParentDataNode as DataNode).DataSource.SelectListColumnsNames.Add("[" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]");
                        (DataNode.ParentDataNode as DataNode).DataSource.DataSourceSelectList += " [" + columnName + DataNode.ParentDataNode.GetHashCode().ToString() + "]";

                    }


                    foreach (RDBMSMetaDataRepository.Column column in (associationEnd as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(DataNode.Classifier as MetaDataRepository.Classifier))
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.GetHashCode().ToString() + "]";

                        if (DataSourceSelectList != null)
                            DataSourceSelectList += ",";

                        SelectListColumnsNames.Add("[" + columnName + DataNode.GetHashCode().ToString() + "]");
                        DataSourceSelectList += " [" + columnName + DataNode.GetHashCode().ToString() + "]";

                    }

                    foreach (RDBMSMetaDataRepository.Column column in DataNode.Classifier.ObjectIDColumns)
                    {
                        string columnName = column.Name;
                        if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                            (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + DataNode.Alias + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + columnName + "]";
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + columnName + DataNode.GetHashCode().ToString() + "]";

                        if (DataSourceSelectList != null)
                            DataSourceSelectList += ",";

                        SelectListColumnsNames.Add("[" + columnName + DataNode.GetHashCode().ToString() + "]");
                        DataSourceSelectList += " [" + columnName + DataNode.GetHashCode().ToString() + "]";
                    }
                }
            }
            foreach (DataNode dataNode in this.DataNode.SubDataNodes)
            {
                if (dataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.OjectAttribute
                    && DataNode.ObjectQuery.SelectListItems.Contains(dataNode))
                {
                    if ((DataNode.ObjectQuery as OQLStatement).SelectList != null)
                        (DataNode.ObjectQuery as OQLStatement).SelectList += ",";

                    (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + dataNode.ParentDataNode.Alias + "]";
                    (DataNode.ObjectQuery as OQLStatement).SelectList += ".";
                    (DataNode.ObjectQuery as OQLStatement).SelectList += "[" + dataNode.Name + "]";
                    if (dataNode.Alias != null)
                        (DataNode.ObjectQuery as OQLStatement).SelectList += " as [" + dataNode.Alias + "]";

                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                    {
                        if (DataSourceSelectList != null)
                            DataSourceSelectList += ",";
                        if (dataNode.Alias != null)
                        {
                            SelectListColumnsNames.Add("[" + dataNode.Alias + "]");
                            DataSourceSelectList += " [" + dataNode.Alias + "]";
                        }
                        else
                        {
                            SelectListColumnsNames.Add("[" + dataNode.Name + "]");
                            DataSourceSelectList += " [" + dataNode.Name + "]";
                            
                        }
                    }
                    else
                    {
                        DataNode currDataNode = DataNode;
                        while (currDataNode != null &&
                            currDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                            (!(currDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany || currDataNode.ParentDataNode.Classifier.LinkAssociation == (currDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association))
                            currDataNode = currDataNode.ParentDataNode as DataNode;
                        

                        if (currDataNode.DataSource.DataSourceSelectList != null)
                            currDataNode.DataSource.DataSourceSelectList += ",";
                        if (dataNode.Alias != null)
                        {
                            currDataNode.DataSource.SelectListColumnsNames.Add("[" + dataNode.Alias+ "]");
                            currDataNode.DataSource.DataSourceSelectList += " [" + dataNode.Alias + "]";
                        }
                        else
                        {
                            currDataNode.DataSource.SelectListColumnsNames.Add("[" + dataNode.Name + "]");
                            currDataNode.DataSource.DataSourceSelectList += " [" + dataNode.Name + "]";
                        }

                    }
                }

            }

        }

		/// <MetaDataID>{D9568380-7A07-4BE9-86FA-FC989C2129D1}</MetaDataID>
		public bool Empty=false;

		/// <MetaDataID>{564EEB99-9986-4F46-86DB-D5AF99CE3CD5}</MetaDataID>
		public string GetColumnName(RDBMSMetaDataRepository.Column column)
		{
			if(ReferenceColumns!=null&&ReferenceColumns.Contains(column.Name))
				return ((ReferenceColumn)ReferenceColumns[column.Name]).ReferenceColumnName;
			return column.Name;
		}
		/// <MetaDataID>{55A75EAF-B799-46B1-A5B1-B93477D76FC4}</MetaDataID>
		private System.Collections.ArrayList OutStorageColumns=null;
		/// <MetaDataID>{674E5CD8-5894-445C-BFC0-0EF73905A070}</MetaDataID>
		public void AddOutStorageColumn(string columnName,string columnNameInRefernceTable,string refernceTableName,string referenceColumnName)
		{
            return;

			DataSource.ReferenceColumn referenceColumn;
			referenceColumn.ReferenceColumnName=referenceColumnName;
			referenceColumn.ColumnName=columnName;
			referenceColumn.RefernceTableName=refernceTableName;
			referenceColumn.ColumnNameInRefernceTable=columnNameInRefernceTable;
			//StorageCellID
			if(ReferenceColumns==null)
			{
				ReferenceColumns=new System.Collections.Hashtable();
				ReferenceColumns.Add(referenceColumn.ColumnName, referenceColumn);
			}
			else
			{
				if(!ReferenceColumns.Contains(referenceColumn.ColumnName))
				{
					if(SelectColumnsNames.Contains(referenceColumn.ColumnName))
						ReferenceColumns.Add(referenceColumn.ColumnName,referenceColumn);
					else
						throw new System.Exception("There isn't column with name "+referenceColumn.ColumnName);
				}
			}
			_SelectColumnsNames=null;
//			if(OutStorageColumns==null)
//			{
//				OutStorageColumns=new System.Collections.ArrayList();
//				OutStorageColumns.Add(columnName);
//			}
//			else
//			{
//				if(!OutStorageColumns.Contains(columnName))
//					OutStorageColumns.Add(columnName);
//			}

		}
        protected bool _HasOutStorageCell = false;
		/// <MetaDataID>{EA7FBF16-5B92-4FDA-8ACE-44A287067D12}</MetaDataID>
		public bool HasOutStorageCell
		{
			get
			{
                return _HasOutStorageCell;
			}

		}
		/// <MetaDataID>{9FB6682D-7E16-4B74-AE14-B804EE724A1B}</MetaDataID>
        string BuildSelectQuery(RDBMSMetaDataRepository.View view, RDBMSMetaDataRepository.StorageCell storageCell)
		{
			string SelectClause=null; 
			System.Collections.ArrayList ColumnsWithNull=new System.Collections.ArrayList();

//			When data node is type object and participates in select clause of query then 
//			the procedure of construction UNION TABLES must be allocate columns for objects 
//			with maximum number of columns.
//			For object that belong to the class in class hierarchy with less columns, 
//			we will add extra columns which missed with null values. 
//			This happen because in the union table statement all select must be 
//			contain the same number and same type of columns
			ColumnsWithNull.AddRange(GetSelectColumnsNamesFromView());
            ColumnsWithNull.Remove("StorageCellHashCode");

			foreach(string columnName in view.ViewColumnsNames)
			{
				if(ColumnsWithNull.Contains(columnName))
					ColumnsWithNull.Remove(columnName);

//				if(SelectClause==null)
//					SelectClause="SELECT ["+columnName+"]";
//				else
//					SelectClause+=", ["+columnName+"]";
			}
/*************************************/

			foreach(string columnName in GetSelectColumnsNamesFromView())
			{
				if(SelectClause==null)
				{
					if(ColumnsWithNull.Contains(columnName))
						SelectClause="SELECT ["+columnName+"] = null";
					else
						SelectClause="SELECT ["+columnName+"]";
				}
				else
				{
					if(ColumnsWithNull.Contains(columnName))
						SelectClause+=", ["+columnName+"] = null";
					else
						SelectClause+=", ["+columnName+"]";
				}
			}

			string StorageName=null;
			string ViewAlias=null;
			PersistenceLayer.Storage ViewObjectStorage=view.Namespace as PersistenceLayer.Storage;
			string ViewStorageName=ViewObjectStorage.StorageName;
            string ViewStorageLocation = (ViewObjectStorage as MetaDataRepository.Storage).GetPropertyValue(typeof(string), "StorageMetadata", "MSSQLInstancePath") as string;
			//string ViewStorageLocation=ViewObjectStorage .StorageLocation;
			string ViewName=view.Name;

			if(DataNode!=null&&ViewObjectStorage!=DataNode.ObjectQuery.ObjectStorage.StorageMetaData)
			{

				if(ViewStorageLocation==DataNode.ObjectQuery.ObjectStorage.StorageMetaData.StorageLocation)
					StorageName=ViewStorageName+".dbo."+ViewName;
				else
                    StorageName = @"OPENROWSET('SQLNCLI', 'Server=" + ViewStorageLocation+ ";Trusted_Connection=yes;','SELECT * FROM " + ViewStorageName + ".dbo." + ViewName + "')";
			}
			ViewAlias =ViewStorageLocation.Replace(@"\","")+"_"+ViewStorageName+"_"+ViewName;
			string FromClause=null;
            if(storageCell!=null)
                FromClause = "\nFROM ( SELECT " + storageCell.GetHashCode().ToString() + " as StorageCellHashCode ,* FROM " + StorageName + ViewName + ") " + ViewAlias;
            else
                FromClause = "\nFROM ( SELECT  -1 as StorageCellHashCode ,* FROM " + StorageName + ViewName + ") " + ViewAlias;

			
            //if(OutStorageColumns!=null)
            //{
            //    if(OutStorageColumns.Count>0)
            //    {
            //        foreach(string ColumnName in OutStorageColumns)
            //            SelectClause+=","+ColumnName +"IDs.ObjectCollectionID  as Out"+ColumnName ;
            //        foreach(string ColumnName in OutStorageColumns)
            //        {
            //            FromClause+=" INNER JOIN "+StorageName+"T_GlobalObjectCollectionIDs "+ColumnName+"IDs ON ";
            //            FromClause+=ViewAlias ;
            //            FromClause+="."+ColumnName;
            //            FromClause+=" = "+ColumnName+"IDs.InStoragelID ";
            //        }
            //    }
            //}

            //if(ReferenceColumns!=null)
            //{
            //    if(ReferenceColumns.Count>0)
            //    {
            //        foreach(System.Collections.DictionaryEntry entry in ReferenceColumns)
            //        {
            //            ReferenceColumn refColumn=(ReferenceColumn)entry.Value;

            //            SelectClause+=","+refColumn.ColumnName +"_ref."+refColumn.ReferenceColumnName+" as ref_"+refColumn.ColumnName ;
            //        }
            //        foreach(System.Collections.DictionaryEntry entry in ReferenceColumns)
            //        {
            //            ReferenceColumn refColumn=(ReferenceColumn)entry.Value ;

            //            FromClause+=" INNER JOIN "+StorageName+refColumn.RefernceTableName+" "+refColumn.ColumnName+"_ref ON ";
            //            FromClause+=ViewAlias ;
            //            FromClause+="."+refColumn.ColumnName;
            //            FromClause+=" = "+refColumn.ColumnName+"_ref."+refColumn.ColumnNameInRefernceTable;
            //        }
            //    }
            //}


			///TODO Η OPENDATASOURCE που χρησιμοποιείται τώρα δεν βοηθάει στην περίπτωση που μπορούμε να φιλτράρουμε
			///τα δεδομένα γιατί όπως δουλεύει φέρνει τα data και μετά τα φιλτράρει. Έτσι όμως δεν κερδίζουμε τίποτα.
			///Η κατάλληλη μέθοδος είναι η παρακάτω γιατί ..
			///Sent and executed by the provider. 
			///Microsoft® SQL Server™ does not process this query, 
			///but processes query results returned by the provider (a pass-through query).
			///Παράδειγμα
			///GO
			///SELECT a.*
			///FROM OPENROWSET('SQLOLEDB','seattle1';'sa';'MyPass',
			///'SELECT * FROM pubs.dbo.authors ORDER BY au_lname, au_fname') AS a


			
			string WhereClause=null;
			
//			if(DataNode!=null&&DataNode.ParticipateInWereClause&& DataNode.ObjectQuery.SearchCondition!=null&&DataNode.ObjectQuery.SearchCondition.HasSQLExpressionFor(DataNode))
//			{
//				WhereClause= "\n WHERE "+DataNode.ObjectQuery.SearchCondition.GetSQLExpressionFor(DataNode);
//				int tttt=0;
//
//			}
				


			

			return SelectClause+FromClause +WhereClause;
		}
	
	}
}
