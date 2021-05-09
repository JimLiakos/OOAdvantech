namespace OOAdvantech.XMLPersistenceRunTime
{
	/// <MetaDataID>{A9F24B4D-5E4F-47ED-89C0-E121F8E76E46}</MetaDataID>
	/// <summary></summary>
	public class StructureSet : PersistenceLayer.StructureSet
	{
        public override void Close()
        {

        }

        public override void MoveToPage(int pageNumber)
        {
            throw new System.NotImplementedException("Paging feature doesn,t supported yet");

        }
        public override bool MoveNextPage()
        {
            throw new System.NotImplementedException("Paging feature doesn,t supported yet");

        }
        public override int PagingActivated
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");

            }
            set
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
        }


        /// <MetaDataID>{0EACFB5A-DE10-48D5-BF3D-0932F1382ABB}</MetaDataID>
        private int PageCountData;
        /// <MetaDataID>{3225BBEF-19B3-40EA-BE33-5F006BD3DB59}</MetaDataID>
        public override int PageCount
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
        }
        /// <MetaDataID>{1E07752E-D7ED-459A-AF9A-2C91654188D4}</MetaDataID>
        private int PageSizeData;
        /// <MetaDataID>{5D5893BA-F308-4D82-A4A9-17F53CCD43FB}</MetaDataID>
        public override int PageSize
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
            set
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");

            }

        }
        /// <MetaDataID>{CCC244A0-FAB9-4348-8203-F5DB19381659}</MetaDataID>
        private System.Collections.ArrayList XmlNodes;

        private string NodeMemberAlias;
        private System.Type NodeMemberType;
        private int Index = -1;

        /// <MetaDataID>{E6568EED-D8D6-4833-BAE0-1BD37F1718F4}</MetaDataID>
        protected void LoadMembers()
        {
            #region Preconditions Chechk

            string ExceptionMessage = null;
            if (SourceStorageSession == null)
                ExceptionMessage = "There isn't logical connection with a storage check StorageSession.";
            if (XmlNodes == null)
            {
                if (ExceptionMessage == null)
                    ExceptionMessage += "\n";
                ExceptionMessage = "The Logical connection with storage data doesn't established.";
            }
            #endregion

            if (Members == null)
                Members = new MemberList();

            ((MemberList)Members).AddMember(NodeMemberAlias);
            PersistenceLayer.Member aMember = Members[NodeMemberAlias];
            System.Xml.XmlElement CurrXmlElement = (System.Xml.XmlElement)XmlNodes[Index];
            string ClassInstaditationName = ((System.Xml.XmlElement)CurrXmlElement.ParentNode).GetAttribute("ClassInstaditationName");
            if (NodeMemberType == null || ClassInstaditationName != NodeMemberType.FullName)
            {
                NodeMemberType = ModulePublisher.ClassRepository.GetType(ClassInstaditationName, "");
                if (NodeMemberType == null)
                    throw new System.Exception("PersistencyService can't instadiate the " + ClassInstaditationName);
            }
            int ObjID = System.Convert.ToInt32(CurrXmlElement.GetAttribute("oid"), 10);
            PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)SourceStorageSession).OperativeObjectCollections[NodeMemberType][ObjID];
            if (StorageInstanceRef != null)
            {
                aMember.Value = StorageInstanceRef.MemoryInstance;
                return;
            }
            object NewObject = ModulePublisher.ClassRepository.CreateInstance(ClassInstaditationName, "");
            if (NewObject == null)
                throw new System.Exception("PersistencyService can't instadiate the " + ClassInstaditationName);


            StorageInstanceRef = (StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)SourceStorageSession).CreateStorageInstanceRef(NewObject, ObjID);
            //StorageInstanceRef.ObjectID=ObjID;
            ((StorageInstanceRef)StorageInstanceRef).StorageIstance = CurrXmlElement;
            ((StorageInstanceRef)StorageInstanceRef).LoadObjectState();
            StorageInstanceRef.SnapshotStorageInstance();
            StorageInstanceRef.ObjectActived();
            aMember.Value = NewObject;

        }

        private System.Collections.ArrayList GetAllSubClasses(MetaDataRepository.Class mClass)
        {
            System.Collections.ArrayList mArrayList = new System.Collections.ArrayList();

            foreach (MetaDataRepository.Generalization CurrGeneralization in mClass.Specializations)
            {
                if (CurrGeneralization.Child != null)
                {
                    mArrayList.Add(CurrGeneralization.Child);
                    mArrayList.AddRange(GetAllSubClasses((MetaDataRepository.Class)CurrGeneralization.Child));
                }
            }
            return mArrayList;
        }
        /// <summary>Mitsos</summary>
        /// <param name="Query">Kitsos Lala</param>
        /// <MetaDataID>{9C62C0E3-8978-4B2D-81EF-1A636F7C1669}</MetaDataID>
        public void Open(string Query)
        {



            string mOQLSelectClause = null;
            string mOQLFromClause = null;
            string mWhereClause = null;
            int nPos = Query.IndexOf("FROM ");
            if (nPos == -1)
                return; //Exception
            mOQLSelectClause = Query.Substring(0, nPos);
            mOQLSelectClause.Trim();
            mOQLFromClause = Query.Substring(nPos, Query.Length - nPos);
            nPos = mOQLFromClause.IndexOf("WHERE ");
            if (nPos != -1)
            {
                mWhereClause = mOQLFromClause.Substring(nPos, mOQLFromClause.Length - nPos);
                mOQLFromClause = mOQLFromClause.Substring(0, nPos);
            }
            mOQLFromClause = mOQLFromClause.Replace("FROM", " ");
            mOQLFromClause = mOQLFromClause.Trim();
            mOQLSelectClause = mOQLSelectClause.Replace("SELECT", " ");
            mOQLSelectClause = mOQLSelectClause.Trim();
            NodeMemberAlias = mOQLSelectClause;
            NodeMemberAlias = NodeMemberAlias.Trim();

            if (mWhereClause != null)
            {
                mWhereClause = mWhereClause.Replace("WHERE", " ");
                mWhereClause = mWhereClause.Trim();
            }
            nPos = mOQLFromClause.IndexOf(" ");
            if (nPos == -1)
            {
                nPos = mOQLFromClause.IndexOf("\t");
                if (nPos == -1)
                    nPos = mOQLFromClause.IndexOf("\n");
            }
            if (nPos == -1)
                return;//exception
            string ClassFullName = mOQLFromClause.Substring(0, nPos);
            string Alias = mOQLFromClause.Substring(nPos, mOQLFromClause.Length - nPos);
            Alias = Alias.Trim();
            if (Alias != NodeMemberAlias)
                throw new System.Exception("The '" + NodeMemberAlias + "' isn't declcared.");
            System.Xml.XmlDocument XMLDocumentData = ((ObjectStorage)SourceStorageSession).XMLDocument;
            string StorageName = XMLDocumentData.ChildNodes[0].Name;
            string XMLQuery = StorageName + "/ObjectCollections/" + ClassFullName + "/Object";

            XmlNodes = new System.Collections.ArrayList();


            if (mWhereClause != null)
                XMLQuery += "[@" + mWhereClause + "]";
            foreach (System.Xml.XmlNode CurrXmlNode in XMLDocumentData.SelectNodes(XMLQuery))
                XmlNodes.Add(CurrXmlNode);

            System.Type mType = ModulePublisher.ClassRepository.GetType(ClassFullName, "");
            object[] objects = mType.Assembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + mType.Assembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");


            MetaDataRepository.Class mClass = (MetaDataRepository.Class)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ModulePublisher.ClassRepository.GetType(ClassFullName, ""));


            if (mClass == null)
            {
                MetaDataRepository.MetaObject AssemblyMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mType.Assembly);
                if (AssemblyMetaObject == null)
                    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(mType.Assembly, new DotNetMetaDataRepository.Assembly(mType.Assembly));
                mClass = (MetaDataRepository.Class)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mType);
            }


            System.Collections.ArrayList AllSubClasses = GetAllSubClasses(mClass);


            foreach (MetaDataRepository.Class CurrClass in AllSubClasses)
            {
                ClassFullName = CurrClass.FullName;
                XMLQuery = StorageName + "/ObjectCollections/" + ClassFullName + "/Object";
                if (mWhereClause != null)
                    XMLQuery += "[@" + mWhereClause + "]";
                foreach (System.Xml.XmlNode CurrXmlNode in XMLDocumentData.SelectNodes(XMLQuery))
                    XmlNodes.Add(CurrXmlNode);

            }


        }


        /// <MetaDataID>{A0FA5C7E-4DB9-4004-9B76-115DA967C7A1}</MetaDataID>
        public override void MoveFirst()
        {
            Index = -1;
        }
        /// <MetaDataID>{95B09B7A-89A9-40C3-8110-439ADC84308E}</MetaDataID>
        bool IsAttheEnd()
        {
            if (XmlNodes == null)
                return true;
            if (XmlNodes.Count == 0)
                return true;
            if (Index + 1 > XmlNodes.Count)
                return true;
            return false;
        }

        /// <MetaDataID>{1AB20727-252E-455E-B1B8-FD9CF2AD7DAF}</MetaDataID>
        public override bool MoveNext()
        {
            Index++;
            if (IsAttheEnd())
                return false;
            else
            {
                LoadMembers();
                return true;
            }
        }

	}
}
