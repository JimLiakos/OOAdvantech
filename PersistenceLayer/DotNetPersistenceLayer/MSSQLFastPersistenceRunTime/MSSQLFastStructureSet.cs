namespace  OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <MetaDataID>{4FEE996A-F9AD-4811-B9DC-D48D6FF3C114}</MetaDataID>
	public class StructureSet : OOAdvantech.PersistenceLayerRunTime.StructureSet
	{
		/// <MetaDataID>{B1CD602F-851F-4675-A80C-4CF4B18D1D70}</MetaDataID>
		public override object GetData()
		{
			//LoadMembers();
			DataBlock mDataBlock=GetInstanceOfDataBlock();
			PersistenceLayer.Member member=null;

            mDataBlock.Data = new System.Data.DataSet();
            mDataBlock.Data.Tables.Add("Table");

			foreach( PersistenceLayer.StructureSet Rowset  in this)
			{
				if(member==null)
				{
					foreach(object _object in Members)
					{
						member=_object as PersistenceLayer.Member;
						break;
					}
                    System.Data.DataColumn objectColumn = mDataBlock.Data.Tables[0].Columns.Add(member.Name);
                    objectColumn.DataType = typeof(int);
                    mDataBlock.ColumnsWithObject.Add(objectColumn.Table.TableName + "_" + objectColumn.ColumnName);
				}

                System.Data.DataRow NewDataRow = mDataBlock.Data.Tables[0].NewRow();
				object Value=Rowset[member.Name];
				if(Value !=null)
				{
					if(Value is System.MarshalByRefObject)
					{
						if(!mDataBlock.Objects.Contains(Value.GetHashCode()))
							mDataBlock.Objects.Add(Value.GetHashCode(),Value);
						NewDataRow[member.Name]=Value.GetHashCode();
					}
				}
				mDataBlock.Data.Tables[0].Rows.Add(NewDataRow);

			}
			return mDataBlock;
		}

		/// <MetaDataID>{46F166A3-6E2C-48A9-AA83-40A16560E0B7}</MetaDataID>
		public override int PagingActivated
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}
 
		/// <MetaDataID>{8A8E22FE-6774-4786-BF07-C37ED303DC66}</MetaDataID>
		public override int PageSize
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}
 
		/// <MetaDataID>{184CEF98-D5F7-4A89-99C8-2C001EC58A1B}</MetaDataID>
		public override int PageCount
		{
			get
			{
				return 0;
			}
		}
 
		/// <MetaDataID>{0216EBB6-6DEE-4C48-84CF-361FA1521DA8}</MetaDataID>
		public override void MoveToPage(int pageNumber)
		{

		}

		/// <MetaDataID>{CF428592-FD74-4945-A35E-BF05E616518D}</MetaDataID>
		public override bool MoveNextPage()
		{
			return false;
		}

		/// <MetaDataID>{8B61633D-B468-461C-903E-1F0A0E1202FD}</MetaDataID>
		public override void Close()
		{

		}
 
		/// <MetaDataID>{F7D8FC9F-95FB-4536-BE81-D2473640E85D}</MetaDataID>
		public override void MoveFirst()
		{
			Index=-1;
		}
		/// <MetaDataID>{91E62988-C715-4E73-94C0-4DFD5BCEDE78}</MetaDataID>
		protected void LoadMembers()
		{
			#region Preconditions Chechk
			
			string ExceptionMessage=null;
			if(SourceStorageSession==null)
				ExceptionMessage="There isn't logical connection with a storage check StorageSession.";
			#endregion


			if(Members==null)
				Members=new MemberList();

			((MemberList)Members).AddMember(MemberAlias);
			PersistenceLayer.Member aMember=Members[MemberAlias];
			object NewObject=Objects[Index];

			aMember.Value=NewObject;

		}
		bool IsAttheEnd()
		{
			if(Objects.Count==0)
				return true;
			if(Index+1>Objects.Count)
				return true;
			return false;
		}
		/// <MetaDataID>{E4B75D3D-1036-4BE9-9F20-C8F380D6ADBD}</MetaDataID>
		public override bool MoveNext()
		{
			Index++;
			if(IsAttheEnd())
				return false;
			else
			{
				LoadMembers();
				return true;
			}
		}

		static private Parser.Parser OQLParser;
		
		System.Collections.ArrayList Objects=new System.Collections.ArrayList(1000);
		private int Index=-1;
		private string MemberAlias; 
		

		/// <MetaDataID>{B9E84716-1AE8-417F-BFBE-6D466B6827B4}</MetaDataID>
		/// <summary>Mitsos </summary>
		/// <param name="Query">Kitsos Lala </param>
		public void Open(string Query)
		{

			

			if(OQLParser==null)
			{
				OQLParser=new Parser.Parser();
                using (System.IO.Stream Grammar = GetType().Assembly.GetManifestResourceStream("OOAdvantech.MSSQLFastPersistenceRunTime.OQL.cgt"))
				{
					byte[] bytes = new byte[Grammar.Length];
					Grammar.Read(bytes,0,(int)Grammar.Length);
					OQLParser.SetGrammar(bytes,(int)Grammar.Length);
					Grammar.Close();
				}
			}
			OQLParser.Parse(Query);
			Parser.ParserNode QueryExpressionType=OQLParser.theRoot.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1);
			Parser.ParserNode SelectList=QueryExpressionType.ChildNodes.GetAt(1);
            Parser.ParserNode objectcollection = OQLParser.theRoot.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(2);
			Parser.ParserNode Critiria=null;
            if (OQLParser.theRoot.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.Count > 2)
                Critiria = OQLParser.theRoot.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(3);
            string Alias = (objectcollection["Objectcollection_Source_List"]["Objectcollection_Source"]["PathAlias"]["Name"] as Parser.ParserNode).Value;
            string ClassFullName = (objectcollection["Objectcollection_Source_List"]["Objectcollection_Source"]["PathAlias"]["Path"] as Parser.ParserNode).Value;
			MemberAlias=SelectList.ChildNodes.GetAt(1).ChildNodes.GetAt(1).Value;

			if(Alias!=MemberAlias)
				throw new System.Exception("The '"+MemberAlias+"' isn't declcared.");
			System.Type mType= ModulePublisher.ClassRepository.GetType(ClassFullName,"");
			
			//TODO:Η GetCustomAttributes καταναλώνει
			object[] objects=mType.Assembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata),false);
			if(objects.Length==0)
				throw new System.Exception("You must declare in assemblyInfo file of  '"+mType.Assembly.FullName+" the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute"); 


			MetaDataRepository.Class  mClass= (MetaDataRepository.Class)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ModulePublisher.ClassRepository.GetType(ClassFullName,""));
            if(mClass==null)
                mClass=new DotNetMetaDataRepository.Class(new OOAdvantech.DotNetMetaDataRepository.Type(ModulePublisher.ClassRepository.GetType(ClassFullName,"")));


			Critirion  critirion =null;
			if(Critiria!=null)
			{
				critirion =new Critirion(Critiria.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1),mClass as DotNetMetaDataRepository.Class,Alias);
				int erwer=0;
			}


			System.Collections.ArrayList SearchClasses=GetAllSubClasses(mClass);
			SearchClasses.Add(mClass);
			

			int count =0;
			foreach(MetaDataRepository.Class _class in SearchClasses)
			{
				System.Type type=_class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
				PersistenceLayerRunTime.ClassMemoryInstanceCollection ClassObjects=(SourceStorageSession as ObjectStorage).OperativeObjectCollections[type] as PersistenceLayerRunTime.ClassMemoryInstanceCollection;
				foreach(System.Collections.DictionaryEntry entry in ClassObjects.StorageInstanceRefs)
				{
					StorageInstanceRef storageInstanceRef=(entry.Value as System.WeakReference).Target as StorageInstanceRef;
					if(storageInstanceRef.ObjectID==null)
						continue;
					if(critirion!=null)
					{

						
						if(critirion.IsTrue(storageInstanceRef.MemoryInstance))
							Objects.Add(storageInstanceRef.MemoryInstance);
					}
					else
						Objects.Add(storageInstanceRef.MemoryInstance);
				}

			}
			System.DateTime befor= System.DateTime.Now;
			System.DateTime after = System.DateTime.Now;
			System.TimeSpan tt=after-befor;
			string ertwer=tt.ToString();
			System.Diagnostics.Debug.WriteLine(ertwer);


								

		}

		/// <MetaDataID>{3D92087A-248F-40FE-ADE1-C85EDD97AA7A}</MetaDataID>
		private System.Collections.ArrayList GetAllSubClasses(MetaDataRepository.Class mClass)
		{
			System.Collections.ArrayList mArrayList=new System.Collections.ArrayList();

			foreach(MetaDataRepository.Generalization CurrGeneralization in mClass.Specializations)
			{
				if(CurrGeneralization.Child!=null)
				{
					mArrayList.Add(CurrGeneralization.Child);
					mArrayList.AddRange(GetAllSubClasses((MetaDataRepository.Class)CurrGeneralization.Child));
				}
			}
			return mArrayList;
		}

	}
}
