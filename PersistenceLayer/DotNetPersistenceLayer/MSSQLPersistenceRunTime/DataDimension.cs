namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{

	/// <MetaDataID>{198402B8-8210-4285-8121-D27A8EDE8827}</MetaDataID>
	/// <summary>The OQL query retrieves data from a data tree.
	///  In data tree there is data nodes and data paths.
	/// The data node is a source of data and data path is the relations between the data source.
	/// For example in family model the query "Select thePersons From Person thePersons Where thePersons.
	/// TheChildrens.Address= thePersons" produce a data tree with two paths with root the source of data Person and paths 
	/// 1.Person (Address relation) Address and 
	/// 2.Person (TheChildrens relation) Person (Address relation) Address. </summary>
	public class DataNode
	{
		/// <MetaDataID>{4A1FF308-9668-4C7B-B86F-4DC0B0151931}</MetaDataID>
		/// <summary>Define the name of data node, actually is the name of path in most of cases. </summary>
		public string Name;

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{916F1383-16C3-4F38-A03F-C8BE60A7E9DC}</MetaDataID>
		private ObjectsDataSource _DataSource;
		/// <summary>Data Source has information about the storage cells 
		/// which keeps the data for the data node. </summary>
		/// <MetaDataID>{E85D5269-5478-4967-9315-87712C38DCA9}</MetaDataID>
		internal ObjectsDataSource DataSource
		{
			get
			{
				return _DataSource;
			}
		}

	
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{0B3C43B0-19DB-4E10-A3C8-5FE40BAB1F49}</MetaDataID>
		private DataNode _DataNodeWithRootDataSource;
		/// <MetaDataID>{A547B520-FDD3-4F29-9D5B-DE391DA1D0E9}</MetaDataID>
		/// <summary>Defines the root data node for the data sources building. 
		/// We can define a path as time period in OQL query. 
		/// This definition tells to the system to search a part of data and the data node now is partial data node . 
		/// In this case the data sources of all data node build relative to the partial data node. </summary>
		public DataNode DataNodeWithRootDataSource
		{
			get
			{
				if(_DataNodeWithRootDataSource==null)
					return HeaderDataNode;
				else
					return _DataNodeWithRootDataSource;
			}
			set
			{
				
				if(_DataNodeWithRootDataSource==null)
					_DataNodeWithRootDataSource=value;
				else
				{
					//if there is more than one data nodes with storage cell constrain the first will be
					//the root.
					if(_DataNodeWithRootDataSource.HasStorageCellConstrain)
						return;

					//TODO test case for more than one time period Constrains
					if(_DataNodeWithRootDataSource.HasTimePeriodConstrain&&value.HasTimePeriodConstrain)
						throw new System.Exception("There is more than one time period Constrains");
					_DataNodeWithRootDataSource=value;
				}
			}
		}

		
		/// <MetaDataID>{641002DA-5162-44A7-B4C2-096CC3BD6361}</MetaDataID>
		public OOAdvantech.PersistenceLayer.Set Restrictions=new OOAdvantech.PersistenceLayer.Set();
		/// <MetaDataID>{8DCD252D-6866-4E4F-BC62-F12CBBEF727E}</MetaDataID>
		public Path Path;
		/// <MetaDataID>{4CFB1AFA-4446-4AB5-8635-F92940D5A90C}</MetaDataID>
		public System.Collections.ArrayList SubDataNodes=new System.Collections.ArrayList();
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{80C86EA4-5554-4522-9754-BFEE58EF3FB2}</MetaDataID>
		private DataNode _ParentDataNode;
		/// <MetaDataID>{8BC29782-0FD5-4BF4-B26D-096C35E1C865}</MetaDataID>
		public DataNode ParentDataNode
		{
			get
			{
				return _ParentDataNode;
			}
			set
			{
				if(_ParentDataNode!=null)
					if(_ParentDataNode.SubDataNodes.Contains(this))
						_ParentDataNode.SubDataNodes.Remove(this);
				_ParentDataNode=value;
				if(_ParentDataNode!=null)
					_ParentDataNode.SubDataNodes.Add(this);

			}
		}
		/// <summary>This member defines the metadata of DataNode. 
		/// It is useful for DataNode to build the scenario to retrieve the data. 
		/// For example it is different to retrieve data from class of object 
		/// and retrieve data from the class of object through a on to many relationship 
		/// and retrieve data from the class through a many to many relationship. </summary>
		/// <MetaDataID>{9A8A6DC6-22B7-43D8-AC7E-216CB4F103C3}</MetaDataID>
		public OOAdvantech.MetaDataRepository.MetaObject AssignedMetaObject;
		/// <MetaDataID>{B47B0433-7021-4907-8A82-999E1F89DC37}</MetaDataID>
		public RDBMSMetaDataRepository.StorageCell ObjectIDConstrainStorageCell;
		/// <MetaDataID>{88B6BCC3-8F4C-43BE-842C-344DEEA663CD}</MetaDataID>
		/// <summary>Define the root data node of data tree which contains the data node with this member. </summary>
		public DataNode HeaderDataNode
		{
			get
			{
				if(ParentDataNode!=null)
					return ParentDataNode.HeaderDataNode;
				else
					return this;
			}
		}
		/// <summary>The DataNode can be any of three types. 
		/// 1. Namespace and can not retrieve data from this DataNode. 
		/// 2. Object and from this DataNode you can retrieve objects. 
		/// 3. OjectAttribute and from this DataNode you can retrieve a field of objects.
		/// 4. Unknown in this case the translator can't find the DataNode name in metadata. 
		/// The recent result of this situation is error of type "There isn't namespace or class with name XXX" or "XXX isn't member of ". </summary>
		/// <MetaDataID>{A27CC08C-E5EE-46F0-ADAC-2425873777FD}</MetaDataID>
		public DataNodeType Type
		{
			get
			{
				if(AssignedMetaObject is RDBMSMetaDataRepository.Attribute)
					return DataNodeType.OjectAttribute;
				if(AssignedMetaObject is RDBMSMetaDataRepository.MappedClassifier)
					return DataNodeType.Object;


				if(AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
					if(((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject).Specification!=null)
						return DataNodeType.Object;
				return DataNodeType.Unknown;
			}		
		}
		/// <MetaDataID>{ACF74472-A6AC-4810-86B9-1EC45E9EB599}</MetaDataID>
		/// <summary>Define all the paths in query, which referred to the data of data node. </summary>
		public System.Collections.ArrayList RelatedPaths =new System.Collections.ArrayList();
	

		//Liakos
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{11C0B478-07E3-4285-8F30-7B17987FA631}</MetaDataID>
		private OOAdvantech.RDBMSMetaDataRepository.MappedClassifier _Classifier;
		/// <MetaDataID>{CB8DF3D9-DA86-4F45-B084-D6327BF229D5}</MetaDataID>
		/// <summary>Define the type of data of the data node (System.String, System.Int32 etc.) </summary>
		public OOAdvantech.RDBMSMetaDataRepository.MappedClassifier Classifier
		{
			get
			{
				
				if(_Classifier!=null)
					return _Classifier;
				if(typeof(RDBMSMetaDataRepository.MappedClassifier).IsInstanceOfType(AssignedMetaObject))
					_Classifier=(RDBMSMetaDataRepository.MappedClassifier)AssignedMetaObject ;


				if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(AssignedMetaObject))
				{
					//TODO ���� ������� �� LinkClass ��� �� ��� ����� ����� �� ������ �� ������������ ��� �� �����
					//��� association �� ����� ����������� ��� �� Assocition end �������.
					
					if(((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject).Association.LinkClass!=null&&Name==((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject).Association.Name)
						_Classifier=(RDBMSMetaDataRepository.MappedClassifier)((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject).Association.LinkClass;
					else
                        _Classifier=(RDBMSMetaDataRepository.MappedClassifier)((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject).Specification;
				}

				return _Classifier;
			}
		}
	
	
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B20748CA-7D5C-4033-9621-149B3EBCBCF0}</MetaDataID>
		private System.DateTime _TimePeriodEndDate;
		/// <MetaDataID>{941E1AE0-1C1B-4F1A-A87C-145D7E696DD1}</MetaDataID>
		/// <summary>Define the end date of a time period. Time period refer to the creation date of objects.
		/// This member has meaning  only if  flag HasTimePeriodConstrain is true. </summary>
		public System.DateTime TimePeriodEndDate
		{
			set
			{
				_TimePeriodEndDate=value;
			}
			get
			{
				if(!HasTimePeriodConstrain)
					throw new System.Exception("There isn't Time period constrain");
				return _TimePeriodEndDate;
			}

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{7DB3278B-D5DE-4AE1-8350-E7999E8CB3B2}</MetaDataID>
		private System.DateTime _TimePeriodStartDate;
		/// <MetaDataID>{0236A91A-F182-42A3-BE71-38C990382C0F}</MetaDataID>
		/// <summary>Define the start date of a time period. Time period refer to the creation date of objects.
		/// This member has meaning  only if  flag HasTimePeriodConstrain is true. </summary>
		public System.DateTime TimePeriodStartDate
		{
			set
			{
				_TimePeriodStartDate=value;
			}
			get
			{
				if(!HasTimePeriodConstrain)
					throw new System.Exception("There isn't Time period constrain");
				return _TimePeriodStartDate;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{56745D4F-EE8A-4F67-9B88-FA915D51AA5D}</MetaDataID>
		private bool _ParticipateInWereClause;
		/// <MetaDataID>{F3082673-621E-4044-9829-CFBF7514B9C4}</MetaDataID>
		/// <summary>Indicate when the data node participate in where clause. It is useful when system build the tables joins. </summary>
		public bool ParticipateInWereClause
		{
			get
			{
				if(_ParticipateInWereClause)
					return true;
				foreach(DataNode CurrSubNode in SubDataNodes)
				{
					if(CurrSubNode.ParticipateInWereClause)
						return true;
				}
				return false;
			}
			set
			{
				_ParticipateInWereClause=true;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{629F8E11-1946-456D-A596-B583C85E57D3}</MetaDataID>
		private string _Alias;
		/// <MetaDataID>{D8DD5D16-A616-40A3-B336-C1DD0B7561C3}</MetaDataID>
		/// <summary>Define the alias name. The name of data node is the last string in path 
		/// for example the paths person.Age and persont.Parents.Age, 
		/// have the same name, to avoid names conflict we use the Alias for example ParentAge for  persont.Parents.Age. </summary>
		public string Alias
		{
			set
			{
				_Alias=value;
			}
			get
			{
				if(Path!=null&&Path.AliasName!=null)
					return Path.AliasName;
				if(typeof(RDBMSMetaDataRepository.Attribute).IsInstanceOfType(AssignedMetaObject))
					return _Alias;
				if(Classifier!=null)
				{
					if(_Alias==null)
						_Alias=ObjectQuery.GetValidAlias("Abstract_"+Classifier.Name);
				}
				return _Alias;
			}
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{6A4CC6AD-B347-4CD9-8F87-B1E27FA8C5DB}</MetaDataID>
		private bool _ParticipateInSelectClause=false;
		/// <MetaDataID>{16A1B9CB-B0A8-4B1D-A679-EA85DC932D75}</MetaDataID>
		/// <summary>This member tells as if the data node is in select list. </summary>
		public bool ParticipateInSelectClause
		{
			get
			{
				if(_ParticipateInSelectClause)
					return true;
				foreach(DataNode CurrSubNode in SubDataNodes)
				{
					if(CurrSubNode.Type==DataNodeType.OjectAttribute)
						return CurrSubNode.ParticipateInSelectClause;
				}
				
				return false;
			}
			set
			{
				_ParticipateInSelectClause=value;
			}
		}

		/// <MetaDataID>{5079B7D5-F19F-47A2-B1C5-8F9BCCD1346D}</MetaDataID>
		private RDBMSMetaDataRepository.StorageCell GetStorageCellFromObjectID(Parser.ParserNode objectIDParserNode, RDBMSMetaDataRepository.MappedClassifier mClass)
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
					return mClass.GetStorageCell(System.Convert.ToInt32(ObjCellID));
				}
			}
			return null;

		}
		

		/// <MetaDataID>{7BD46C75-2F12-4D0D-8A8B-C1E3A0413859}</MetaDataID>
		public void BuildDataNodeTree(Storage objectStorage,ref string errorOutput)
		{
			MergeIdenticalDataNodes();
			UpdateParserNodeDataNodeMap(ObjectQuery.PathDataNodeMap);

			MetaDataRepository.Namespace mNamespace=null;
			string Query ="SELECT Namespace FROM "+typeof(MetaDataRepository.Namespace).FullName+" Namespace WHERE Name = \""+Name+"\"";
			PersistenceLayer.StructureSet aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(objectStorage).Execute(Query );
			foreach( PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				mNamespace =(MetaDataRepository.Namespace)Rowset.Members["Namespace"].Value;
				if(mNamespace.GetType()==typeof(MetaDataRepository.Namespace)) 
					break;
			}
			if(mNamespace==null)
				errorOutput+="There isn't namespace or class with name " +Name;
			AssignedMetaObject=mNamespace;
			Validate(ref errorOutput);
			MergeIdenticalDataNodes();
			if(errorOutput!=null&&errorOutput.Length>0)
				return;

			CreateOnConstructionSubDataNode();
			MergeIdenticalDataNodes();
			
		}

		/// <MetaDataID>{9EE5ADEE-29D9-441A-9E4E-ADDC84A92512}</MetaDataID>
		public void BuildDataSource()
		{
			if(ObjectQuery.SearchCondition!=null)
				DataNodeWithRootDataSource=ObjectQuery.SearchCondition.GetObjectIDDataNodeConstrain(HeaderDataNode);
			DataNodeWithRootDataSource.BuildDataSource(null);
		}

		/// <MetaDataID>{A0B72706-4CEF-4A28-B1AC-C5489B2A9CB6}</MetaDataID>
		void BuildOutStorageIDs()
		{
			{
				RDBMSMetaDataRepository.AssociationEnd AssociationEnd=AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
				if(AssociationEnd==null)
					return ;
				if(AssociationEnd.Association.LinkClass!=null&&Name==AssociationEnd.Association.LinkClass.Name)
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in  ((RDBMSMetaDataRepository.AssociationEnd)AssociationEnd.GetOtherEnd()).GetColumns(true,AssociationEnd.Association.LinkClass))
					{
						if(CurrColumn.ColumnType=="ObjCellID")
							_DataSource.AddOutStorageColumn(CurrColumn.Name,"","","");
					}
				}
				else
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in  AssociationEnd.GetColumns(true,AssociationEnd.Specification))
					{
						if(CurrColumn.ColumnType=="ObjCellID")
							_DataSource.AddOutStorageColumn(CurrColumn.Name,"","","");
					}
				}
				if(AssociationEnd.Association.LinkClass!=null&&AssociationEnd.Association.LinkClass==ParentDataNode.Classifier)
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in  AssociationEnd.GetColumns(true,AssociationEnd.Association.LinkClass))
					{
						if(CurrColumn.ColumnType=="ObjCellID")
							ParentDataNode.DataSource.AddOutStorageColumn(CurrColumn.Name,"","","");
					}
				}
				else
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in  ParentDataNode.Classifier.ObjectIDColumns)
					{
						if(CurrColumn.ColumnType=="ObjCellID")
							ParentDataNode.DataSource.AddOutStorageColumn(CurrColumn.Name,"","","");
					}
				}
				if(Type==DataNode.DataNodeType.Object)
				{
					foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in  Classifier.ObjectIDColumns)
					{
						if(CurrColumn.ColumnType=="ObjCellID")
							_DataSource.AddOutStorageColumn(CurrColumn.Name,"","","");
					}
				}

			}
		}

		/// <MetaDataID>{E9B4E7F6-B2FA-4BA0-8FD4-B6D4229F60F0}</MetaDataID>
		/// <summary>Tel as when the data node is auto generated from on construction procedure. 
		/// It is useful in structure set building, because the data from auto generated data nodes has no meaning for the client. </summary>
		public bool AutoGenerated=false;

		/// <MetaDataID>{C055C387-9E59-457D-854E-C786745013AD}</MetaDataID>
		/// <summary>Some relationships between classes marked as on 
		/// contractions. This means when system load an object in 
		/// memory have to load and related object of relationship with 
		/// on construction marking. 
		/// This method extends the data node tree to load the on construction related object. </summary>
		void CreateOnConstructionSubDataNodeFor(RDBMSMetaDataRepository.AssociationEnd AssociationEnd)
		{
			if(AssociationEnd.HasLazyFetchingRealization)
				return;

			#region checks if already exist sub data node for association end
			foreach(DataNode CurrSubDatNode in SubDataNodes)
				if(CurrSubDatNode.AssignedMetaObject.Identity==AssociationEnd.Identity)
				{
					if(!ObjectQuery.SelectListItems.Contains(CurrSubDatNode))
					{
						ObjectQuery.AddSelectListItem(CurrSubDatNode);
						CurrSubDatNode.AutoGenerated=true;
					}
					return;
				}
			#endregion

			#region Abord recursion
			//TODO �� �������� test cases ��� ����� ��� �����������

			//You can't go back on relationship because you go into recursive loop
			if(AssignedMetaObject is MetaDataRepository.AssociationEnd)
				if(((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.Identity==AssociationEnd.Association.Identity)
					return;
			//Check parents datanode for auto generated datanode with the same association end
			//if there is data node with the same association and return to avoid recursive loop
			if(ParentDataNode!=null&&ParentDataNode.IsThereAutoGenDataNodeInHierarchy(AssociationEnd))
				return;
			#endregion

			#region Add new sub data node for  association end
			DataNode MyDataNode=new DataNode(ObjectQuery);
			MyDataNode.ParentDataNode=this;
			MyDataNode.AssignedMetaObject=AssociationEnd;
			if(AssociationEnd.Association.LinkClass==null)
				MyDataNode.Name=AssociationEnd.Name;
			else
				MyDataNode.Name=AssociationEnd.Association.Name;
			MyDataNode.Alias=AssociationEnd.Name+MyDataNode.GetHashCode().ToString();
			MyDataNode.AutoGenerated=true;
			ObjectQuery.AddSelectListItem(MyDataNode);
			#endregion
		}

		/// <MetaDataID>{8F908939-A77D-412B-ABF6-861F41BA0E9E}</MetaDataID>
		/// <summary>Look data tree backward for auto generated data node for the association end of parameter. 
		/// This method is useful when we want to avoid recursive generation of data node for the association end of parameter. </summary>
		bool IsThereAutoGenDataNodeInHierarchy(MetaDataRepository.AssociationEnd associationEnd)
		{
			if(AutoGenerated&&AssignedMetaObject.Identity==associationEnd.Identity)
				return true;
			else
			{
				if(ParentDataNode==null)
					return false;
				else
					return ParentDataNode.IsThereAutoGenDataNodeInHierarchy(associationEnd);

			}
		}
	



		/// <MetaDataID>{0F4E331E-1B88-41AA-8238-56EC93A00338}</MetaDataID>
		/// <summary>Some relationships between classes marked as on 
		/// contractions. This means that, when system load an object in 
		/// memory have to load also related object of relationship with 
		/// on construction marking. 
		/// This method extends the data node tree to load the on construction related object. </summary>
		void CreateOnConstructionSubDataNode()
		{

			if(Type==DataNodeType.Object&&ObjectQuery.SelectListItems.Contains(this))
			{
				foreach(RDBMSMetaDataRepository.AssociationEnd CurrAssociationEnd in (Classifier as MetaDataRepository.Classifier) .GetAssociateRoles(true))
				{
					if(CurrAssociationEnd.Association.HasPersistentObjectLink&&!CurrAssociationEnd.HasLazyFetchingRealization&&!CurrAssociationEnd.Multiplicity.IsMany&&CurrAssociationEnd.Navigable)
						CreateOnConstructionSubDataNodeFor(CurrAssociationEnd);
				}
			}

			if(Classifier!=null&&(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation!=null&&ObjectQuery.SelectListItems.Contains(this))
				CreateLinkClassSubDataNodes();
			for(int i=0;i<SubDataNodes.Count;i++)
				(SubDataNodes[i] as DataNode).CreateOnConstructionSubDataNode();
		}

		/// <MetaDataID>{3FD0356B-A2A6-4093-952D-A8680A637F00}</MetaDataID>
		MetaDataRepository.MetaObjectCollection GetStorageCellsOfThisType(MetaDataRepository.Classifier classifier)
		{
			if(classifier is RDBMSMetaDataRepository.Class)
				return (classifier as RDBMSMetaDataRepository.Class).StorageCellsOfThisType;

			if(classifier is MetaDataRepository.Interface)
			{
				MetaDataRepository.MetaObjectCollection StorageCells=new OOAdvantech.MetaDataRepository.MetaObjectCollection();
				foreach(MetaDataRepository.Realization realization in (classifier as MetaDataRepository.Interface).Realizations)
				{
					if(realization.Implementor is MetaDataRepository.Class&& (realization.Implementor as MetaDataRepository.Class).Persistent)
					{
						StorageCells.AddCollection((realization.Implementor as RDBMSMetaDataRepository.Class).StorageCellsOfThisType);
					}
				}
				return StorageCells;
			}
			throw new System.Exception("system can't retrieve data from "+classifier.FullName);

		}


		/// <MetaDataID>{549772D4-1AD1-4081-8631-8B0B2986D551}</MetaDataID>
		/// <summary>When you load a relation object in memory system must be 
		/// load the related object also.The CreateLinkClassSubDataNodes 
		/// method checks the existence of data nodes in data tree for related 
		/// objects. If there aren't create and add them in select list. </summary>
		void CreateLinkClassSubDataNodes()
		{
			//TODO:�� �������� �� �������� ���� ��������� �������� ��� LinkClass
			bool RoleAExist=false;
			bool RoleBExist=false;

			if(AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
			{
				RDBMSMetaDataRepository.AssociationEnd AssociationEnd =AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
				//if(AssociationEnd.Association.LinkClass==null)
					//return;
				if(AssociationEnd.Identity==(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity)
				{
					RoleBExist=true;
					if(!ObjectQuery.SelectListItems.Contains(ParentDataNode))
					{
						ObjectQuery.AddSelectListItem(ParentDataNode);
						ParentDataNode.ParticipateInSelectClause=false;
						ParentDataNode.CreateOnConstructionSubDataNode();
					}
				}
				if(AssociationEnd.Identity==(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity)
				{
					RoleAExist=true;
					if(!ObjectQuery.SelectListItems.Contains(ParentDataNode))
					{
						ObjectQuery.AddSelectListItem(ParentDataNode);
						ParentDataNode.ParticipateInSelectClause=false;
						ParentDataNode.CreateOnConstructionSubDataNode();
					}
				}
			}
			else
				return;
			foreach(DataNode CurrDataNode in SubDataNodes)
			{
				if(CurrDataNode.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd )
				{
					RDBMSMetaDataRepository.AssociationEnd AssociationEnd =CurrDataNode.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
					if(AssociationEnd.Identity==(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity&&!RoleAExist)
					{
						RoleAExist=true;
						if(!ObjectQuery.SelectListItems.Contains(CurrDataNode))
							ObjectQuery.AddSelectListItem(CurrDataNode);
					}
					if(AssociationEnd.Identity==(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity&&!RoleBExist)
					{
						RoleBExist=true;
						if(!ObjectQuery.SelectListItems.Contains(CurrDataNode))
							ObjectQuery.AddSelectListItem(CurrDataNode);
					}
				}
			}
			if(!RoleAExist)
			{
				DataNode MyDataNode=new DataNode(ObjectQuery);
				MyDataNode.ParentDataNode=this;
				MyDataNode.AssignedMetaObject=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA;
				MyDataNode.Name=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name;
				MyDataNode.Alias=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name+MyDataNode.GetHashCode().ToString();
				MyDataNode.AutoGenerated=true;
				ObjectQuery.AddSelectListItem(MyDataNode);
			}
			if(!RoleBExist)
			{
				DataNode MyDataNode=new DataNode(ObjectQuery);
				MyDataNode.ParentDataNode=this;
				MyDataNode.AssignedMetaObject=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB;
				MyDataNode.Name=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name;
				MyDataNode.Alias=(Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name+MyDataNode.GetHashCode().ToString();
				MyDataNode.AutoGenerated=true;
				ObjectQuery.AddSelectListItem(MyDataNode);
			}
		}
		/// <MetaDataID>{E5AE68B3-3BA5-4515-834D-F14A7C60C3A8}</MetaDataID>
		void BuildDataSource(DataNode ReferenceDataNode)
		{ 
			if(_DataSource!=null)
				return; // the data source already builded
			if(AssignedMetaObject is RDBMSMetaDataRepository.Attribute)
			{
				if(HasTimePeriodConstrain)
					throw new System.Exception("You can,t apply 'TIMEPERIOD' keyword on Class primitive member");
				return;
			}
			//RDBMSMetaDataRepository.StorageCell ObjectIDStorageCell=null;
			if(ObjectIDConstrainStorageCell!=null)
			{
				
//				ObjectIDStorageCell=GetStorageCellFromObjectID(ObjectIDParserNode,Classifier);
//				if(ObjectIDStorageCell==null)
//					throw new System.Exception("the ObjectID "+ObjectIDParserNode+" is invalid.");
				MetaDataRepository.MetaObjectCollection StorageCells=new MetaDataRepository.MetaObjectCollection();
				StorageCells.Add(ObjectIDConstrainStorageCell);
				_DataSource=new ObjectsDataSource(this,StorageCells);

			}
			else
			{
				if(ReferenceDataNode==null)
				{
					if(Classifier!=null)
					{
						if(HasTimePeriodConstrain)
							_DataSource=new ObjectsDataSource(this,Classifier.GetStorageCells(TimePeriodStartDate,TimePeriodEndDate));
						else
						{
							MetaDataRepository.MetaObjectCollection StorageCells=new MetaDataRepository.MetaObjectCollection();
							foreach(RDBMSMetaDataRepository.StorageCell CurrStorageCell in Classifier.StorageCells)
							{
								if(CurrStorageCell is RDBMSMetaDataRepository.OutStorageStorageCell)
									continue;
								StorageCells.Add(CurrStorageCell);
							}
							//TODO �� �������� �� ����� �������� ���� 
							//_DataSource=new DataSource(Classifier.TypeView,this,Classifier.StorageCells);

							_DataSource=new ObjectsDataSource(this,Classifier.StorageCells);
						}
					}
				}
				else
				{
					if(Classifier==null)
						return;
					MetaDataRepository.MetaObjectCollection StorageCells=new MetaDataRepository.MetaObjectCollection();
					RDBMSMetaDataRepository.Association association=null;
					RDBMSMetaDataRepository.AssociationEnd associationEnd=null;

					if(ReferenceDataNode==ParentDataNode &&AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
					{
						associationEnd=((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject);
						association=associationEnd.Association as RDBMSMetaDataRepository.Association;
					}
					else if(ReferenceDataNode.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
					{
						associationEnd=(RDBMSMetaDataRepository.AssociationEnd)ReferenceDataNode.AssignedMetaObject ;
						associationEnd=associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd; 
						association=associationEnd.Association as RDBMSMetaDataRepository.Association;
					}
					if(associationEnd!=null)
					{
						if(ReferenceDataNode.DataSource.StorageCells!=null)
						{
							if(Name==association.Name)
							{
								foreach(RDBMSMetaDataRepository.StorageCell CurrStorageCell in  ReferenceDataNode.DataSource.StorageCells)
									StorageCells.AddCollection(	association.GetRelationObjectCells(CurrStorageCell));
								_DataSource = new ObjectsDataSource(this,StorageCells);

							}
							else
							{
								foreach(RDBMSMetaDataRepository.StorageCell CurrStorageCell in  ReferenceDataNode.DataSource.StorageCells)
									StorageCells.AddCollection(	association.GetLinkedObjectCells(CurrStorageCell,associationEnd));
									_DataSource = new ObjectsDataSource(this,StorageCells);
							}
							bool ThereIsOutStorageCell=false;
							if(_DataSource.StorageCells!=null)
							{
								foreach(RDBMSMetaDataRepository.StorageCell CurrStorageCell in  _DataSource.StorageCells)
								{
									if(CurrStorageCell.GetType()==typeof(RDBMSMetaDataRepository.OutStorageStorageCell))
									{
										ThereIsOutStorageCell=true;
										break;
									}
								}
							}
							if(ThereIsOutStorageCell)
								BuildOutStorageIDs();
						}
						else
						{
							if(ParticipateInSelectClause)
								_DataSource = new ObjectsDataSource(this,Classifier.StorageCells);
							else
                                _DataSource = new ObjectsDataSource(this);
						}
						//TODO �� ������� ���� � data node ����� ���� select list
					}
					else
						throw new System.Exception("Error on Data tree");
				}
			}
			foreach(DataNode CurrSubDataNode in SubDataNodes)
			{
				if(CurrSubDataNode!=ReferenceDataNode)
				{
					if(Classifier==null)
						CurrSubDataNode.BuildDataSource(null);
					else
						CurrSubDataNode.BuildDataSource(this);
				}
			}
			if(_DataSource!=null&&_DataSource.HasOutStorageCell)
			{

				bool ThereIsOutStorageCell=false;
				foreach(DataNode CurrSubDataNode in SubDataNodes)
				{
					if(CurrSubDataNode!=ReferenceDataNode)
					{
						if(Classifier!=null&&CurrSubDataNode.Classifier!=null&&CurrSubDataNode.DataSource.HasOutStorageCell)
						{
							CurrSubDataNode.BuildOutStorageIDs();
							ThereIsOutStorageCell=true;
						}
					}	
				}

				if(ThereIsOutStorageCell)
					BuildOutStorageIDs();


			}
			if(ParentDataNode!=null&&ParentDataNode.Classifier!=null)
				ParentDataNode.BuildDataSource(this);
			


		}

		/// <MetaDataID>{15669C6C-CF87-48E5-8611-F3697A821841}</MetaDataID>
		/// <summary>Define time period constrain flag. In case where the system produce data massively it is useful the data partitioning. With this technique you can search data rapidly when you refer to a time period. </summary>
		public bool HasTimePeriodConstrain=false;

		/// <MetaDataID>{D7BB9696-86CF-4443-B7C2-2A2AF9AF3A32}</MetaDataID>
		/// <summary>This member indicates if the data node has storage cell constrain. 
		/// When the data node has storage cell constrain the data node retrieve data only from this storage cell. </summary>
		public bool HasStorageCellConstrain=false;

	
		/// <MetaDataID>{DF0C0DA4-5E6C-45CF-969B-0E20337974ED}</MetaDataID>
		internal OQLStatement ObjectQuery;

		
		/// <MetaDataID>{646F5541-0460-4AA0-A8F5-ACCA9BAC716D}</MetaDataID>
		internal DataNode(OQLStatement mOQLStatement, Path path)
		{
			Path=path;
			ObjectQuery=mOQLStatement;
			Name=path.Name;
			RelatedPaths.Add(path);
			if(path.AliasName!=null)
				ObjectQuery.BookAlias(path.AliasName);
			
		}
		/// <MetaDataID>{32472CC0-314E-4C27-ABE4-184051333FDC}</MetaDataID>
		private DataNode(OQLStatement mOQLStatement)
		{
			ObjectQuery=mOQLStatement;
		}


		/// <MetaDataID>{13E079C4-0ACD-4DCE-AD04-8A591EA756FD}</MetaDataID>
		/// <summary>Define the full name of data node which is the full name of parent data node plus dot plus the name of this data node. </summary>
		public string FullName
		{
			get
			{
				if(ParentDataNode!=null)
					return ParentDataNode.FullName+"."+Name;
				return Name;
			}
		}


		public enum DataNodeType
		{
			/// <summary>The OQL query retrieves data</summary>
			Namespace=0,
			Object,
			OjectAttribute,
			Unknown
		};

		bool ThisOrAnyOfParentParticipateInSelectClause
		{
			get
			{
				if(ParticipateInSelectClause)
					return true;
				if(ParentDataNode!=null)
					return ParentDataNode.ThisOrAnyOfParentParticipateInSelectClause;
				return false;
			}
		}
		
		
		/// <MetaDataID>{DF9589FA-71B3-4509-99D6-B3BFC9CC056D}</MetaDataID>
		/// <summary>Build a join between the table from data source and data source table of sub data node. </summary>
		private string BuildOneToManyTablesJoin(DataNode SubObjectCollection)
		{
			RDBMSMetaDataRepository.AssociationEnd associationEnd=SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
			RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns=(associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
			
			MetaDataRepository.MetaObjectCollection FirstJoinTableColumns=null;
			MetaDataRepository.MetaObjectCollection SecondJoinTableColumns=null;

			if(associationEnd==associationEndWithReferenceColumns)
			{
				SecondJoinTableColumns=associationEndWithReferenceColumns.GetColumns(true,Classifier as MetaDataRepository.Classifier);
				FirstJoinTableColumns=(associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
			}
			else
			{
				FirstJoinTableColumns=associationEndWithReferenceColumns.GetColumns(true,Classifier as MetaDataRepository.Classifier);
				SecondJoinTableColumns=(associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
			}

			string FromClauseSQLQuery=null;

			
			if(SubObjectCollection.ParticipateInWereClause||(!ThisOrAnyOfParentParticipateInSelectClause))
				FromClauseSQLQuery=" INNER JOIN " ;
			else
                FromClauseSQLQuery=" LEFT OUTER JOIN " ;



			string query=string.Copy("(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )");

			string subDataNodeFromClauseSQLQuery=SubObjectCollection.BuildFromClauseSQLQuery();
			if(subDataNodeFromClauseSQLQuery!=null&&subDataNodeFromClauseSQLQuery.Trim().Length>0)
			{

				FromClauseSQLQuery+="(";
			
				FromClauseSQLQuery+=SubObjectCollection.DataSource.SQLStatament+" AS ["+SubObjectCollection.Alias+"] ";
			
				FromClauseSQLQuery+=SubObjectCollection.BuildFromClauseSQLQuery();
				FromClauseSQLQuery+=") ON ";
			}
			else
				FromClauseSQLQuery+=SubObjectCollection.DataSource.SQLStatament+" AS ["+SubObjectCollection.Alias+"] ON ";

			
			if(SecondJoinTableColumns.Count!=FirstJoinTableColumns.Count)
				throw new System.Exception("Incorrect mapping of "+associationEnd.FullName+" association");
			string InnerJoinAttributes=null;
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
			{
				foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
				{
					if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
					{							
						if(InnerJoinAttributes!=null)
							InnerJoinAttributes+=" AND ";
						InnerJoinAttributes+="["+Alias+"].["+_DataSource.GetColumnName(CurrColumn)+"]";
						InnerJoinAttributes+=" = ["+SubObjectCollection.Alias+"]";
						InnerJoinAttributes+=".["+SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn)+"]";
					}
				}
			}
			FromClauseSQLQuery+=InnerJoinAttributes;

			return FromClauseSQLQuery;
		}

		/// <MetaDataID>{861AB591-9E5D-4361-8AD7-1CB71847085E}</MetaDataID>
		private string BuildAssociationClassTablesJoin(DataNode SubObjectCollection)
		{
			RDBMSMetaDataRepository.AssociationEnd associationEnd=SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;

			MetaDataRepository.MetaObjectCollection FirstJoinTableColumns=null;
			MetaDataRepository.MetaObjectCollection SecondJoinTableColumns=null;
			
			if(associationEnd.Name==SubObjectCollection.Name)
			{
				//is type of AssociationClass.Class
				SecondJoinTableColumns=SubObjectCollection.Classifier.ObjectIDColumns;
				FirstJoinTableColumns=associationEnd.GetColumns(true,associationEnd.Association.LinkClass);
			}
			else if(associationEnd.Association.Name==SubObjectCollection.Name)
			{
				//is type of Class.AssociationClass
				FirstJoinTableColumns=Classifier.ObjectIDColumns;
				SecondJoinTableColumns=((RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd()).GetColumns(true,associationEnd.Association.LinkClass);
			}
			else
				throw new System.Exception("Data tree Error");//Error Prone
	
 

			string FromClauseSQLQuery=null;
			if(SubObjectCollection.ParticipateInWereClause||(!ThisOrAnyOfParentParticipateInSelectClause))
				FromClauseSQLQuery=" INNER JOIN " ;
			else if(associationEnd.Name==SubObjectCollection.Name)
				FromClauseSQLQuery=" INNER JOIN " ;//LEFT OUTER JOIN 
			else
				FromClauseSQLQuery=" LEFT OUTER JOIN " ;//LEFT OUTER JOIN 


			string subDataNodeFromClauseSQLQuery=SubObjectCollection.BuildFromClauseSQLQuery();
			if(subDataNodeFromClauseSQLQuery!=null&&subDataNodeFromClauseSQLQuery.Trim().Length>0)
			{
				FromClauseSQLQuery+="( ";

				FromClauseSQLQuery+=SubObjectCollection.DataSource.SQLStatament+" AS ["+SubObjectCollection.Alias+"] ";
				FromClauseSQLQuery+=subDataNodeFromClauseSQLQuery;
				FromClauseSQLQuery+=") ON ";
			}
			else
				FromClauseSQLQuery+=SubObjectCollection.DataSource.SQLStatament+" AS ["+SubObjectCollection.Alias+"] ON";


			string query="(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )";
			
			 
			
			if(SecondJoinTableColumns.Count!=FirstJoinTableColumns.Count)
				throw new System.Exception("Incorrect mapping of "+associationEnd.FullName+" association");
			string InnerJoinAttributes=null;
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
			{
				foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
				{
					if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
					{							
						if(InnerJoinAttributes!=null)
							InnerJoinAttributes+=" AND ";
						InnerJoinAttributes+="["+Alias+"].["+_DataSource.GetColumnName(CurrColumn)+"]";//"Abstract_"+Class.Name+"."+CurrColumn.Name;
						InnerJoinAttributes+=" = ["+SubObjectCollection.Alias+"]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
						InnerJoinAttributes+=".["+SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn)+"]";
					}
				}
			}
			FromClauseSQLQuery+=InnerJoinAttributes;

			return FromClauseSQLQuery;
		}
		/// <MetaDataID>{430F0D12-D624-4B93-B980-D35B2E3256FA}</MetaDataID>
		/// <summary>Build a join between the table from data source and
		/// association table and a join between association table end data source table of sub data node. </summary>
		private string BuildManyToManyTablesJoin(DataNode SubObjectCollection)
		{
			

			RDBMSMetaDataRepository.AssociationEnd associationEnd=SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
			#region precondition check
			if(associationEnd==null||associationEnd.Association.MultiplicityType!=MetaDataRepository.AssociationType.ManyToMany&&associationEnd.Association.LinkClass==null)
				throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between "+FullName+" and "+SubObjectCollection.Name);
			#endregion
			
			string FromClauseSQLQuery=null;	
			associationEnd=(RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();

			#region Construct ObjectLinksDataSource object
			MetaDataRepository.MetaObjectCollection ObjectsLinks=new MetaDataRepository.MetaObjectCollection();
			if(DataSource.StorageCells!=null)
			{
				foreach(RDBMSMetaDataRepository.StorageCell CurrStorageCell in  DataSource.StorageCells)
					ObjectsLinks.AddCollection(	((RDBMSMetaDataRepository.Association)associationEnd.Association).GetObjectLinks(CurrStorageCell));

				if(ObjectsLinks.Count==0)
					ObjectsLinks.Add(((RDBMSMetaDataRepository.Association)associationEnd.Association).ActiveObjectLinksStorage);
			}
			else
			{
				if(((RDBMSMetaDataRepository.Association)associationEnd.Association).ActiveObjectLinksStorage!=null)
					ObjectsLinks.Add(((RDBMSMetaDataRepository.Association)associationEnd.Association).ActiveObjectLinksStorage);
			}
			ObjectLinksDataSource objectLinksDataSource=new ObjectLinksDataSource(associationEnd.Association,ObjectsLinks);
			#endregion

			#region Construct the joined assoctition table.
			string AssociationTableStatement=objectLinksDataSource.SQLStatament;
			string AliasAssociationTableName=null;
			AliasAssociationTableName=ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);

			#endregion

			#region Construct data source to association table join.

			if(SubObjectCollection.ParticipateInWereClause||(!ThisOrAnyOfParentParticipateInSelectClause))
				FromClauseSQLQuery=" INNER JOIN " ;
			else
				FromClauseSQLQuery=" LEFT OUTER JOIN " ;

			FromClauseSQLQuery+=AssociationTableStatement+ " AS ["+AliasAssociationTableName+"] ON ";
			
			MetaDataRepository.MetaObjectCollection FirstJoinTableColumns=Classifier.ObjectIDColumns;
			MetaDataRepository.MetaObjectCollection SecondJoinTableColumns=associationEnd.GetAssociationTableColumnsFor(((RDBMSMetaDataRepository.Association)associationEnd.Association).ActiveObjectLinksStorage);
			if(SecondJoinTableColumns.Count!=FirstJoinTableColumns.Count)
				throw new System.Exception("Incorrect mapping of "+associationEnd.FullName+" association");

			string InnerJoinAttributes=null;
			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
			{
				foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
				{
					if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
					{							
						if(InnerJoinAttributes!=null)
							InnerJoinAttributes+=" AND ";

						InnerJoinAttributes+=Alias+".["+_DataSource.GetColumnName(CurrColumn)+"]";
						InnerJoinAttributes+=" = ["+AliasAssociationTableName+"]";
						InnerJoinAttributes+=".["+objectLinksDataSource.GetColumnName(CorrespondingCurrColumn)+"]";
					}
				}
			}
			FromClauseSQLQuery+=InnerJoinAttributes;
			#endregion

			#region Construct association table to sub node data source join. 
			if(SubObjectCollection.ParticipateInWereClause||(!ThisOrAnyOfParentParticipateInSelectClause))
				FromClauseSQLQuery+=" INNER JOIN " ;
			else
				FromClauseSQLQuery+=" LEFT OUTER JOIN " ;
			FromClauseSQLQuery+="( ";	
			RDBMSMetaDataRepository.MappedClassifier OtherEndClass=(RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
			RDBMSMetaDataRepository.AssociationEnd OtherEnd=(RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
			FromClauseSQLQuery+= SubObjectCollection.DataSource.SQLStatament+" AS ["+SubObjectCollection.Alias +"] ";//" Abstract_"+OtherEndClass.Name+" ON ";

			FromClauseSQLQuery+=SubObjectCollection.BuildFromClauseSQLQuery();
			FromClauseSQLQuery+=") ON ";


			FirstJoinTableColumns=OtherEnd.GetAssociationTableColumnsFor(((RDBMSMetaDataRepository.Association)associationEnd.Association).ActiveObjectLinksStorage);
			
			SecondJoinTableColumns=OtherEndClass.ObjectIDColumns;

			InnerJoinAttributes=null;

			foreach(RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
			{
				foreach(RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
				{
					if(CurrColumn.ColumnType==CorrespondingCurrColumn.ColumnType)
					{							
						if(InnerJoinAttributes!=null)
							InnerJoinAttributes+=" AND ";
						InnerJoinAttributes+="["+AliasAssociationTableName+"]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
						InnerJoinAttributes+=".["+objectLinksDataSource.GetColumnName(CurrColumn)+"]";
						InnerJoinAttributes+=" = ["+SubObjectCollection.Alias+"].["+SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn)+"]";
					}
				}
			}
							
			FromClauseSQLQuery+=" "+InnerJoinAttributes;
			#endregion

			return FromClauseSQLQuery;
		}
		/// <MetaDataID>{B4196328-F443-4E92-9B75-0D996B3D3B61}</MetaDataID>
		public string BuildFromClauseSQLQuery()
		{

			string FromClauseSQLQuery=null;
			if(typeof(RDBMSMetaDataRepository.Attribute).IsInstanceOfType(AssignedMetaObject))
				return null;
			if(Classifier==null&&(typeof(MetaDataRepository.Namespace).IsInstanceOfType(AssignedMetaObject)))
			{
				foreach(DataNode CurrObjectCollection in SubDataNodes)
					FromClauseSQLQuery+=CurrObjectCollection.BuildFromClauseSQLQuery(); 
			}
			
			string query="(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )";
			if(Classifier!=null)
			{
				if(DataSource.Empty)
					return "";

				if(!typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(AssignedMetaObject))
					FromClauseSQLQuery=DataSource.SQLStatament+" AS ["+Alias+"]";
				foreach(DataNode CurrObjectCollection in SubDataNodes)
				{
					#region Build table join with sub data node if needed. 
					if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(CurrObjectCollection.AssignedMetaObject))
					{
						RDBMSMetaDataRepository.AssociationEnd AssociationEnd=(RDBMSMetaDataRepository.AssociationEnd)CurrObjectCollection.AssignedMetaObject;
						if(AssociationEnd.Association.MultiplicityType ==MetaDataRepository.AssociationType.ManyToMany)
						{
							if((AssociationEnd.Association.LinkClass==CurrObjectCollection.Classifier && AssociationEnd.Association.Name==CurrObjectCollection.Name)||	//is type of Class.AssociationClass
								(AssociationEnd.Association.LinkClass==Classifier && AssociationEnd.Name==CurrObjectCollection.Name))									//is type of AssociationClass.Class
								FromClauseSQLQuery+=BuildAssociationClassTablesJoin(CurrObjectCollection);
							else
								FromClauseSQLQuery+=BuildManyToManyTablesJoin(CurrObjectCollection);
						}
						else
						{
							if((AssociationEnd.Association.LinkClass==CurrObjectCollection.Classifier && AssociationEnd.Association.Name==CurrObjectCollection.Name)||//is type of AssociationClass.Class
								(AssociationEnd.Association.LinkClass==Classifier && AssociationEnd.Name==CurrObjectCollection.Name))								//is type of Class.AssociationClass
								FromClauseSQLQuery+=BuildAssociationClassTablesJoin(CurrObjectCollection);
							else
								FromClauseSQLQuery+=BuildOneToManyTablesJoin(CurrObjectCollection);
						}
					}
					#endregion

					//Get the sub data node joined tables  
//					FromClauseSQLQuery+=CurrObjectCollection.BuildFromClauseSQLQuery();					
				}
			}
			return FromClauseSQLQuery;
		}


				 
			/// <MetaDataID>{6396CD7E-A4FA-4FF2-8CA1-B77E29FCB84D}</MetaDataID>
		void UpdateParserNodeDataNodeMap(System.Collections.Hashtable ParserNodeObjectCollectionMap)
		{
			foreach(Path path  in RelatedPaths)
			{
				if(!ParserNodeObjectCollectionMap.Contains(path.ParserNode))
					ParserNodeObjectCollectionMap.Add(path.ParserNode,this);
				else
					ParserNodeObjectCollectionMap[path.ParserNode]=this;
			}
			foreach(DataNode CurrObjectCollection in SubDataNodes)
				CurrObjectCollection.UpdateParserNodeDataNodeMap(ParserNodeObjectCollectionMap);
		}

		
		/// <MetaDataID>{27AAEAFB-3CDF-46CE-AF30-7A2597089418}</MetaDataID>
		/// <summary>The work of this method is to find the corresponding Meta object for the sub data node. 
		/// If data node is namespace then sub node is class or namespace. 
		/// If data node is class then the sub data node is attribute or association end or nested class. 
		/// If data note is association end then the sub data node is attribute or association end of data node association end specification. </summary>
		MetaDataRepository.MetaObject GetMataObjectForDataNode(DataNode dataNode)
		{
			if(AssignedMetaObject is MetaDataRepository.Namespace)
			{
				MetaDataRepository.Namespace _namespace=(MetaDataRepository.Namespace)AssignedMetaObject;
				foreach(MetaDataRepository.MetaObject metaObject in _namespace.OwnedElements)
				{
					if(metaObject.FullName==dataNode.FullName&&metaObject is MetaDataRepository.Classifier )
						return metaObject;					
				}
				if(Classifier==null)
					return null;
			}

			MetaDataRepository.Classifier classifier=Classifier as MetaDataRepository.Classifier;
			if(classifier!=null)
			foreach(MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
			{
				if(classifier.LinkAssociation!=null&&classifier.LinkAssociation.HasPersistentObjectLink)
				{
					object Value=attribute.GetPropertyValue(typeof(bool),"MetaData","AssociationClassRole");
					bool IsAssociationClassRole =false;
					if(Value!=null)
						IsAssociationClassRole =(bool)Value;
					if(IsAssociationClassRole&&attribute.Name==dataNode.Name)
					{
						bool IsRoleA=(bool)attribute.GetPropertyValue(typeof(bool),"MetaData","IsRoleA");
						if(IsRoleA)
						{
							dataNode.Name=classifier.LinkAssociation.RoleA.Name;
							return classifier.LinkAssociation.RoleA;
						}
						else
						{
							dataNode.Name=classifier.LinkAssociation.RoleB.Name;
							return classifier.LinkAssociation.RoleB;
						}
					}
				}
				//if(!attribute.Persistent)
					//continue;
				if(attribute.Name==dataNode.Name)
					return attribute;
			}

			foreach(MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
			{
				//if(!associationEnd.Association.HasPersistentObjectLink)
				//	continue;

				if(associationEnd .Name==dataNode.Name)
					return associationEnd; 

				if( associationEnd.Association.LinkClass!=null)
				{
					if(associationEnd.Association.Name==dataNode.Name)
						return associationEnd;//.GetOtherEnd();
				}
			}

			return null;
		}
	
		/// <MetaDataID>{F594ED27-10E6-42F6-8B14-600F0095AC80}</MetaDataID>
		/// <summary>Check the data node and sub data nodes against the Meta data of object storage. 
		/// If it is valid assign the Meta object to data node. </summary>
		bool Validate(ref string ErrorOutput)
		{
			bool Valid =true;
			try
			{
				foreach(DataNode dataNode in SubDataNodes)
				{
					MetaDataRepository.MetaObject metaObject=GetMataObjectForDataNode(dataNode);
					if(metaObject==null)
					{
						if(Alias!=null)
							ErrorOutput+="\nError on '"+Alias+"'";
						else
						{
							if(Classifier!=null)
                                ErrorOutput+="\n'"+dataNode.Name+"' isn't persistent member of '"+(Classifier as MetaDataRepository.Classifier).FullName+"'";
							else
								ErrorOutput+="\n'"+dataNode.Name+"' isn't persistent member of '"+AssignedMetaObject.FullName+"'";
						}
						Valid=false;
					}
					else
					{
						dataNode.AssignedMetaObject=metaObject;
						if(!dataNode.Validate(ref ErrorOutput))
							Valid=false;
					}

				}

				if(HasTimePeriodConstrain)
					HeaderDataNode.DataNodeWithRootDataSource=this;
				
			}
			catch(System.Exception Error)
			{
				int j=0;
				throw Error;
			}

			return Valid;
		}
				



		/// <summary>In OQL statement there is data paths in three clauses 
		/// (SELECT, FROM, WHERE) with the root in FROM clause.
		/// At the time of collection of data paths from the parse tree 
		/// can be produce duplicated sub data nodes. 
		/// This method merges the duplicated sub data nodes. </summary>
		/// <MetaDataID>{7E7E8819-04EB-4133-B968-91EFC5001F9E}</MetaDataID>
		public void MergeIdenticalDataNodes()
		{
			System.Collections.ArrayList subDataNodesCash=new System.Collections.ArrayList(SubDataNodes);

			#region Merge SubDataNodes with same name
			foreach(DataNode CurrObjectCollection in subDataNodesCash)
			{ 
				if(SubDataNodes.Contains(CurrObjectCollection))
				{
					foreach(DataNode CandidateForMergeCollection in subDataNodesCash)
						MergeIfIdentical(CurrObjectCollection,CandidateForMergeCollection);
				}
			}
			#endregion

			#region Merge SubDataNodes with same link class association

			//for example if there is Query "SELECT employee.Job.StartingDate, employee.Employers.Name FROM Employee employee
			//the DataNode tree is like this
			//Employee 
			//	|
			//	|_________Job
			//	|			|
			//	|			|______StartingDate
			//	|
			//	|_________Employers
			//	|			|
			//	|			|______Name 
			//
			//After merging the DataNode tree will be like this
			//			
			//Employee 
			//	|
			//	|_________Job
			//	|			|
			//	|			|______StartingDate
			//	|			|
			//	|			|______Employers
			//	|					|
			//	|					|______Name 

			if(AssignedMetaObject!=null)
			{
				foreach(DataNode dataNode in subDataNodesCash)
				{
					if(dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
					{
						MetaDataRepository.AssociationEnd associationEnd=dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
						if(associationEnd.Association.LinkClass!=null&&dataNode.Name!=associationEnd.Association.Name)
						{
							foreach(DataNode canditateForMergeDataNode in SubDataNodes)
							{
								if(dataNode==canditateForMergeDataNode )
									continue;

								if(canditateForMergeDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
								{
									MetaDataRepository.AssociationEnd canditateForMergedataAssociationEnd=canditateForMergeDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
									if(canditateForMergedataAssociationEnd.Association.LinkClass!=null&&canditateForMergeDataNode.Name==canditateForMergedataAssociationEnd.Association.Name)
									{
										bool Merged=false;
										foreach( DataNode subNode in canditateForMergeDataNode.SubDataNodes)
										{
											if(subNode.AssignedMetaObject==dataNode.AssignedMetaObject)
											{
												MergeIfIdentical(subNode,dataNode);
												Merged=true;
												break;
											}
										}
										if(!Merged)
										{
											SubDataNodes.Remove(dataNode);
											dataNode.ParentDataNode=canditateForMergeDataNode;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			#endregion

			foreach(DataNode CurrObjectCollection in SubDataNodes)
				CurrObjectCollection.MergeIdenticalDataNodes();

		}
		/// <MetaDataID>{183753B2-8578-49B1-9C30-F9F54FF377C2}</MetaDataID>
		/// <summary>Check the data nodes if they are identical. If they are identical merge them in one. </summary>
		protected bool MergeIfIdentical(DataNode MergeInDataNode, DataNode MergedDataNode)
		{
			string query="(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )";
			if(MergeInDataNode==MergedDataNode)
				return false;
			if(MergeInDataNode.Name!=MergedDataNode.Name)
				return false;
			SubDataNodes.Remove(MergedDataNode);
			System.Collections.ArrayList TempSubDataNodes=new System.Collections.ArrayList(MergedDataNode.SubDataNodes);
			foreach(DataNode CurrDataNode in TempSubDataNodes)
				CurrDataNode.ParentDataNode=MergeInDataNode;
			
			foreach(Path path in MergedDataNode.RelatedPaths)
				MergeInDataNode.RelatedPaths.Add(path);
			if(MergedDataNode.ParticipateInWereClause)
				MergeInDataNode.ParticipateInWereClause=true;
			MergeInDataNode.ObjectIDConstrainStorageCell=MergedDataNode.ObjectIDConstrainStorageCell;
			MergeInDataNode.Restrictions.AddCollection(MergedDataNode.Restrictions);
			//MergeInDataNode.Type=MergeInDataNode.Type;

			if(MergedDataNode.ParticipateInSelectClause&&ObjectQuery.SelectListItems.Contains(MergedDataNode))
			{
				ObjectQuery.RemoveSelectListItem(MergedDataNode);
				if(!ObjectQuery.SelectListItems.Contains(MergeInDataNode))
				{
					ObjectQuery.AddSelectListItem(MergeInDataNode);
					MergeInDataNode.ParticipateInSelectClause=true;
				}
			}
			return true;
		}
		
	}
}
