using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{91158FEA-4A23-4095-8F10-0E53787BFE25}</MetaDataID>
    [Serializable]
    public class DataNode : MetaDataRepository.ObjectQueryLanguage.DataNode
    {
        [NonSerialized]
        public Member StructureSetMember;
        protected OOAdvantech.MetaDataRepository.Namespace GetNamespace(OOAdvantech.PersistenceLayer.Storage objectStorage, string _namespace)
        {
            string Query = "SELECT Namespace FROM " + typeof(MetaDataRepository.Namespace).FullName + " Namespace WHERE Name = \"" + Name + "\"";
            PersistenceLayer.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(objectStorage).Execute(Query);
            foreach (PersistenceLayer.StructureSet Rowset in aStructureSet)
            {
                MetaDataRepository.Namespace mNamespace = (MetaDataRepository.Namespace)Rowset.Members["Namespace"].Value;
                if (mNamespace.GetType() == typeof(MetaDataRepository.Namespace))
                    return mNamespace;
            }
            return null;
        }

        internal DataNode(MetaDataRepository.ObjectQueryLanguage.OQLStatement mOQLStatement)
            : base(mOQLStatement)
        {

        }
        internal DataNode(MetaDataRepository.ObjectQueryLanguage.OQLStatement mOQLStatement, MetaDataRepository.ObjectQueryLanguage.Path path)
            : base(mOQLStatement, path)
        {
        }
        /// <MetaDataID>{884B5EC2-1897-4AB8-A174-5EB5D0DD5B3F}</MetaDataID>
        private RDBMSMetaDataRepository.StorageCell GetStorageCellFromObjectID(Parser.ParserNode objectIDParserNode, RDBMSMetaDataRepository.MappedClassifier mClass)
        {
            if (objectIDParserNode == null)
                throw new System.Exception("Can't retrieve Storage Cell for null objectID");
            int Count = objectIDParserNode.ChildNodes.GetFirst().ChildNodes.Count;
            for (int i = 0; i != Count; i++)
            {
                Parser.ParserNode ObjectIDField = objectIDParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(i + 1);
                string ObjectIDFieldName = ObjectIDField.ChildNodes.GetAt(1).Value;
                if (ObjectIDFieldName == "StorageCellID")
                {
                    string ObjCellID = ObjectIDField.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
                    return mClass.GetStorageCell(System.Convert.ToInt32(ObjCellID));
                }
            }
            return null;

        }
        /// <summary>Define the type of data of the data node (System.String, System.Int32 etc.) </summary>
        /// <MetaDataID>{CB8DF3D9-DA86-4F45-B084-D6327BF229D5}</MetaDataID>
        public new OOAdvantech.RDBMSMetaDataRepository.MappedClassifier Classifier
        {
            get
            {
                return (OOAdvantech.RDBMSMetaDataRepository.MappedClassifier)base.Classifier;
            }
        }

        
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B8A42660-F4B9-4E5F-B938-3CA5BA2E0BFC}</MetaDataID>
        [NonSerialized]        
        private ObjectsDataSource _DataSource;
        /// <summary>Data Source has information about the storage cells 
        /// which keeps the data for the data node. </summary>
        /// <MetaDataID>{E85D5269-5478-4967-9315-87712C38DCA9}</MetaDataID>
        internal protected ObjectsDataSource DataSource
        {
            get
            {
                return _DataSource;
            }
        }

        /// <MetaDataID>{F9C05D56-640A-463E-8E5C-20FC0B212A28}</MetaDataID>
        [NonSerialized]
        public RDBMSMetaDataRepository.StorageCell ObjectIDConstrainStorageCell;


        //SubClass
        /// <MetaDataID>{A7283B7A-4A9B-4538-8EB3-10843408292D}</MetaDataID>
        public void BuildDataSource()
        {
            if (ObjectQuery.SearchCondition != null)
                DataNodeWithRootDataSource = (ObjectQuery.SearchCondition as MetaDataRepository.ObjectQueryLanguage.SearchCondition).GetObjectIDDataNodeConstrain(HeaderDataNode as DataNode);
            (DataNodeWithRootDataSource as DataNode).BuildDataSource(null);
        }

  
        //SubClass
        /// <summary>Some relationships between classes marked as on 
        /// contractions. This means when system load an object in 
        /// memory have to load and related object of relationship with 
        /// on construction marking. 
        /// This method extends the data node tree to load the on construction related object. </summary>
        /// <MetaDataID>{3587E023-471E-4EEB-8B50-CE1B097BB6D0}</MetaDataID>
        private void CreateOnConstructionSubDataNodeFor(RDBMSMetaDataRepository.AssociationEnd associationEnd)
        {
            if (associationEnd.HasLazyFetchingRealization)
                return;

            #region checks if already exist sub data node for association end
            foreach (DataNode CurrSubDatNode in SubDataNodes)
                if (CurrSubDatNode.AssignedMetaObject.Identity == associationEnd.Identity)
                {
                    if (!ObjectQuery.SelectListItems.Contains(CurrSubDatNode))
                    {
                        ObjectQuery.AddSelectListItem(CurrSubDatNode);
                        CurrSubDatNode.AutoGenerated = true;
                    }
                    return;
                }
            #endregion

            #region Abord recursion
            //TODO να γραφτούν test cases για αυτές τις περιπτώσεις

            //You can't go back on relationship because you go into recursive loop
            if (AssignedMetaObject is MetaDataRepository.AssociationEnd)
                if (((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.Identity == associationEnd.Association.Identity)
                    return;
            //Check parents datanode for auto generated datanode with the same association end
            //if there is data node with the same association and return to avoid recursive loop
            if (ParentDataNode != null && (ParentDataNode as DataNode).IsThereAutoGenDataNodeInHierarchy(associationEnd))
                return;
            #endregion

            #region Add new sub data node for  association end
            DataNode MyDataNode = new DataNode(ObjectQuery);
            MyDataNode.ParentDataNode = this;
            MyDataNode._AssignedMetaObject = associationEnd;
            if (associationEnd.Association.LinkClass == null)
                MyDataNode.Name = associationEnd.Name;
            else
                MyDataNode.Name = associationEnd.Association.Name;
            MyDataNode.Alias = associationEnd.Name + MyDataNode.GetHashCode().ToString();
            MyDataNode.AutoGenerated = true;
            ObjectQuery.AddSelectListItem(MyDataNode);
            #endregion
        }

        /// <summary>Some relationships between classes marked as on 
        /// contractions. This means that, when system load an object in 
        /// memory have to load also related object of relationship with 
        /// on construction marking. 
        /// This method extends the data node tree to load the on construction related object. </summary>
        /// <MetaDataID>{E4298C9B-F794-4062-B333-4D248EAA4675}</MetaDataID>
        protected override void CreateOnConstructionSubDataNode()
        {

            if (Type == DataNodeType.Object && ObjectQuery.SelectListItems.Contains(this))
            {
                foreach (RDBMSMetaDataRepository.AssociationEnd CurrAssociationEnd in (Classifier as MetaDataRepository.Classifier).GetAssociateRoles(true))
                {
                    if (CurrAssociationEnd.Association.HasPersistentObjectLink && !CurrAssociationEnd.HasLazyFetchingRealization && !CurrAssociationEnd.Multiplicity.IsMany && CurrAssociationEnd.Navigable)
                        CreateOnConstructionSubDataNodeFor(CurrAssociationEnd);
                }
            }

            if (Classifier != null && (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation != null && ObjectQuery.SelectListItems.Contains(this))
                CreateLinkClassSubDataNodes();
            for (int i = 0; i < SubDataNodes.Count; i++)
                (SubDataNodes[i] as DataNode).CreateOnConstructionSubDataNode();
        }
        protected override bool MergeIfIdentical(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode MergeInDataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode MergedDataNode)
        {
            bool retVal = base.MergeIfIdentical(MergeInDataNode, MergedDataNode);
            (MergeInDataNode as DataNode).ObjectIDConstrainStorageCell = (MergedDataNode as DataNode).ObjectIDConstrainStorageCell;
            return retVal;
        }


        /// <summary>When you load a relation object in memory system must be 
        /// load the related object also.The CreateLinkClassSubDataNodes 
        /// method checks the existence of data nodes in data tree for related 
        /// objects. If there aren't create and add them in select list. </summary>
        /// <MetaDataID>{8865D564-5AE1-45B3-8335-8408435083B1}</MetaDataID>
        void CreateLinkClassSubDataNodes()
        {
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            bool RoleAExist = false;
            bool RoleBExist = false;

            if (AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
            {
                RDBMSMetaDataRepository.AssociationEnd AssociationEnd = AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
                //if(AssociationEnd.Association.LinkClass==null)
                //return;
                if (AssociationEnd.Identity == (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity)
                {
                    RoleBExist = true;
                    if (!ObjectQuery.SelectListItems.Contains(ParentDataNode))
                    {
                        ObjectQuery.AddSelectListItem(ParentDataNode);
                        ParentDataNode.ParticipateInSelectClause = false;
                        (ParentDataNode as DataNode).CreateOnConstructionSubDataNode();
                    }
                }
                if (AssociationEnd.Identity == (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity)
                {
                    RoleAExist = true;
                    if (!ObjectQuery.SelectListItems.Contains(ParentDataNode))
                    {
                        ObjectQuery.AddSelectListItem(ParentDataNode);
                        ParentDataNode.ParticipateInSelectClause = false;
                        (ParentDataNode as DataNode).CreateOnConstructionSubDataNode();
                    }
                }
            }
            else
                return;
            foreach (DataNode CurrDataNode in SubDataNodes)
            {
                if (CurrDataNode.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd AssociationEnd = CurrDataNode.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
                    if (AssociationEnd.Identity == (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity && !RoleAExist)
                    {
                        RoleAExist = true;
                        if (!ObjectQuery.SelectListItems.Contains(CurrDataNode))
                            ObjectQuery.AddSelectListItem(CurrDataNode);
                    }
                    if (AssociationEnd.Identity == (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity && !RoleBExist)
                    {
                        RoleBExist = true;
                        if (!ObjectQuery.SelectListItems.Contains(CurrDataNode))
                            ObjectQuery.AddSelectListItem(CurrDataNode);
                    }
                }
            }
            if (!RoleAExist)
            {
                DataNode MyDataNode = new DataNode(ObjectQuery);
                MyDataNode.ParentDataNode = this;
                MyDataNode._AssignedMetaObject = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA;
                MyDataNode.Name = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name;
                MyDataNode.Alias = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name + MyDataNode.GetHashCode().ToString();
                MyDataNode.AutoGenerated = true;
                ObjectQuery.AddSelectListItem(MyDataNode);
            }
            if (!RoleBExist)
            {
                DataNode MyDataNode = new DataNode(ObjectQuery);
                MyDataNode.ParentDataNode = this;
                MyDataNode._AssignedMetaObject = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB;
                MyDataNode.Name = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name;
                MyDataNode.Alias = (Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name + MyDataNode.GetHashCode().ToString();
                MyDataNode.AutoGenerated = true;
                ObjectQuery.AddSelectListItem(MyDataNode);
            }
        }



        /// <MetaDataID>{D27A1F05-C1EE-4C7C-892A-6BB73837B59E}</MetaDataID>
        void BuildDataSource(DataNode ReferenceDataNode)
        {
            if (ReferenceDataNode == null)
            {
                int tt = 0;
            }

            if (_DataSource != null)
                return; // the data source already builded
            if (AssignedMetaObject is RDBMSMetaDataRepository.Attribute)
            {
                if (HasTimePeriodConstrain)
                    throw new System.Exception("You can,t apply 'TIMEPERIOD' keyword on Class primitive member");
                return;
            }
            if (ObjectIDConstrainStorageCell != null)
            {
                Collections.Generic.Set<MetaDataRepository.StorageCell>  storageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
                storageCells.Add(ObjectIDConstrainStorageCell);
                _DataSource = new ObjectsDataSource(this, storageCells);

            }
            else
            {
                if (ReferenceDataNode == null)
                {
                    if (Classifier != null)
                    {
                        if (HasTimePeriodConstrain)
                            _DataSource = new ObjectsDataSource(this, Classifier.GetStorageCells(TimePeriodStartDate, TimePeriodEndDate));
                        else
                        {
                            MetaDataRepository.MetaObjectCollection StorageCells = new MetaDataRepository.MetaObjectCollection();
                            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in Classifier.LocalStorageCells)
                            {
                                if (CurrStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                    continue;
                                StorageCells.Add(CurrStorageCell);
                            }
                            //TODO να ελεγχθει αν είναι καλύτερα αυτό 
                            //_DataSource=new DataSource(Classifier.TypeView,this,Classifier.StorageCells);

                            Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                            foreach (MetaDataRepository.StorageCell storageCell in Classifier.LocalStorageCells)
                                storageCells.Add(storageCell);
                            _DataSource = new ObjectsDataSource(this, storageCells);
                        }
                    }
                }
                else
                {
                    if (Classifier == null)
                        return;
                    Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>() ;
                    RDBMSMetaDataRepository.Association association = null;
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

                    if (ReferenceDataNode == ParentDataNode && AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
                    {
                        associationEnd = ((RDBMSMetaDataRepository.AssociationEnd)AssignedMetaObject);
                        association = associationEnd.Association as RDBMSMetaDataRepository.Association;
                    }
                    else if (ReferenceDataNode.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
                    {
                        associationEnd = (RDBMSMetaDataRepository.AssociationEnd)ReferenceDataNode.AssignedMetaObject;
                        associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
                        association = associationEnd.Association as RDBMSMetaDataRepository.Association;
                    }
                    if (associationEnd != null)
                    {
                        if (ReferenceDataNode.DataSource.StorageCells != null)
                        {
                            //TODO τί γίνεται όταν το associationEnd έχει το ίδιο ονομα με την association
                            if (Name == association.Name && Name != associationEnd.Name)
                            {
                                foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in ReferenceDataNode.DataSource.StorageCells)
                                {
                                    if (associationEnd.IsRoleA)
                                        storageCells.AddCollection(association.GetLinkedStorageCells(CurrStorageCell, MetaDataRepository.Roles.RoleB));
                                    else
                                        storageCells.AddCollection(association.GetLinkedStorageCells(CurrStorageCell, MetaDataRepository.Roles.RoleA));

                                    //if (associationEnd.IsRoleA)
                                    //    StorageCells.AddCollection( CurrStorageCell.GetLinkedStorageCells(association.Identity, MetaDataRepository.Roles.RoleB) as System.Collections.ICollection);
                                    //else
                                    //    StorageCells.AddCollection(CurrStorageCell.GetLinkedStorageCells(association.Identity, MetaDataRepository.Roles.RoleA) as System.Collections.ICollection);
                                }
                                _DataSource = new ObjectsDataSource(this, storageCells);

                            }
                            else
                            {

                                foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in ReferenceDataNode.DataSource.StorageCells)
                                {
                                    if (associationEnd.IsRoleA)
                                        storageCells.AddCollection(association.GetLinkedStorageCells(CurrStorageCell, MetaDataRepository.Roles.RoleA) );
                                    else
                                        storageCells.AddCollection(association.GetLinkedStorageCells(CurrStorageCell,  MetaDataRepository.Roles.RoleB));

                                    //if (associationEnd.IsRoleA)
                                    //    StorageCells.AddCollection(CurrStorageCell.GetLinkedStorageCells(association.Identity, MetaDataRepository.Roles.RoleA) as System.Collections.ICollection);
                                    //else
                                    //    StorageCells.AddCollection(CurrStorageCell.GetLinkedStorageCells(association.Identity, MetaDataRepository.Roles.RoleB) as System.Collections.ICollection);
                                }
                                _DataSource = new ObjectsDataSource(this, storageCells);
                            }
                            bool ThereIsOutStorageCell = false;
                            if (_DataSource.StorageCells != null)
                            {
                                foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in _DataSource.StorageCells)
                                {
                                    if (CurrStorageCell.GetType() == typeof(RDBMSMetaDataRepository.StorageCellReference))
                                    {
                                        ThereIsOutStorageCell = true;
                                        break;
                                    }
                                }
                            }
                      
                        }
                        else
                        {
                            if (ParticipateInSelectClause)
                                _DataSource = new ObjectsDataSource(this, Classifier.LocalStorageCells);
                            else
                                _DataSource = new ObjectsDataSource(this);
                        }
                        //TODO τι γίνεται όταν η data node είναι στην select list
                    }
                    else
                        throw new System.Exception("Error on Data tree");
                }
            }
            foreach (DataNode CurrSubDataNode in SubDataNodes)
            {
                if (CurrSubDataNode != ReferenceDataNode)
                {
                    if (Classifier == null)
                        CurrSubDataNode.BuildDataSource(null);
                    else
                        CurrSubDataNode.BuildDataSource(this);
                }
            }

            if (ParentDataNode != null && ParentDataNode.Classifier != null)
                (ParentDataNode as DataNode).BuildDataSource(this);
        }

        /// <summary>Build a join between the table from data source and data source table of sub data node. </summary>
        /// <MetaDataID>{7C44A9DF-B512-49BE-ABEF-D96BB0C8146F}</MetaDataID>
        private string BuildOneToManyTablesJoin(DataNode SubObjectCollection)
        {
            RDBMSMetaDataRepository.AssociationEnd associationEnd = SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();

            MetaDataRepository.MetaObjectCollection FirstJoinTableColumns = null;
            MetaDataRepository.MetaObjectCollection SecondJoinTableColumns = null;

            if (associationEnd == associationEndWithReferenceColumns)
            {
                SecondJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns(Classifier as MetaDataRepository.Classifier);
                FirstJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
            }
            else
            {
                FirstJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns(Classifier as MetaDataRepository.Classifier);
                SecondJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
            }

            string FromClauseSQLQuery = null;


            if (SubObjectCollection.ParticipateInWereClause || (!ThisOrAnyOfParentParticipateInSelectClause))
                FromClauseSQLQuery = " INNER JOIN ";
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";



            string query = string.Copy("(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )");

            string subDataNodeFromClauseSQLQuery = SubObjectCollection.BuildFromClauseSQLQuery();
            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
            {

                FromClauseSQLQuery += "(";

                FromClauseSQLQuery += SubObjectCollection.DataSource.SQLStatament + " AS [" + SubObjectCollection.Alias + "] ";

                FromClauseSQLQuery += SubObjectCollection.BuildFromClauseSQLQuery();
                FromClauseSQLQuery += ") ON ";
            }
            else
                FromClauseSQLQuery += SubObjectCollection.DataSource.SQLStatament + " AS [" + SubObjectCollection.Alias + "] ON ";


            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");
            string InnerJoinAttributes = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";
                        InnerJoinAttributes += "[" + Alias + "].[" + _DataSource.GetColumnName(CurrColumn) + "]";
                        InnerJoinAttributes += " = [" + SubObjectCollection.Alias + "]";
                        InnerJoinAttributes += ".[" + SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn) + "]";
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;

            return FromClauseSQLQuery;
        }

        /// <MetaDataID>{C22E771B-D3C6-43A0-96F5-AACE440D2E8C}</MetaDataID>
        private string BuildAssociationClassTablesJoin(DataNode SubObjectCollection)
        {
            RDBMSMetaDataRepository.AssociationEnd associationEnd = SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;

            MetaDataRepository.MetaObjectCollection FirstJoinTableColumns = null;
            MetaDataRepository.MetaObjectCollection SecondJoinTableColumns = null;

            if (associationEnd.Name == SubObjectCollection.Name)
            {
                //is type of AssociationClass.Class
                SecondJoinTableColumns = SubObjectCollection.Classifier.ObjectIDColumns;
                FirstJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
            }
            else if (associationEnd.Association.Name == SubObjectCollection.Name)
            {
                //is type of Class.AssociationClass
                FirstJoinTableColumns = Classifier.ObjectIDColumns;
                SecondJoinTableColumns = ((RDBMSMetaDataRepository.AssociationEnd)associationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
            }
            else
                throw new System.Exception("Data tree Error");//Error Prone



            string FromClauseSQLQuery = null;
            if (SubObjectCollection.ParticipateInWereClause || (!ThisOrAnyOfParentParticipateInSelectClause))
                FromClauseSQLQuery = " INNER JOIN ";
            else if (associationEnd.Name == SubObjectCollection.Name)
                FromClauseSQLQuery = " INNER JOIN ";//LEFT OUTER JOIN 
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";//LEFT OUTER JOIN 


            string subDataNodeFromClauseSQLQuery = SubObjectCollection.BuildFromClauseSQLQuery();
            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
            {
                FromClauseSQLQuery += "( ";

                FromClauseSQLQuery += SubObjectCollection.DataSource.SQLStatament + " AS [" + SubObjectCollection.Alias + "] ";
                FromClauseSQLQuery += subDataNodeFromClauseSQLQuery;
                FromClauseSQLQuery += ") ON ";
            }
            else
                FromClauseSQLQuery += SubObjectCollection.DataSource.SQLStatament + " AS [" + SubObjectCollection.Alias + "] ON";


            string query = "(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )";


            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");
            string InnerJoinAttributes = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";
                        InnerJoinAttributes += "[" + Alias + "].[" + _DataSource.GetColumnName(CurrColumn) + "]";//"Abstract_"+Class.Name+"."+CurrColumn.Name;
                        InnerJoinAttributes += " = [" + SubObjectCollection.Alias + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                        InnerJoinAttributes += ".[" + SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn) + "]";
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;

            return FromClauseSQLQuery;
        }

        /// <summary>Build a join between the table from data source and
        /// association table and a join between association table end data source table of sub data node. </summary>
        /// <MetaDataID>{97E28D6A-7A8E-40B2-81FF-B4B2AE305DD5}</MetaDataID>
        private string BuildManyToManyTablesJoin(DataNode SubObjectCollection)
        {


            RDBMSMetaDataRepository.AssociationEnd associationEnd = SubObjectCollection.AssignedMetaObject as RDBMSMetaDataRepository.AssociationEnd;
            #region precondition check
            if (associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + FullName + " and " + SubObjectCollection.Name);
            #endregion

            string FromClauseSQLQuery = null;
            associationEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();

            #region Construct ObjectLinksDataSource object
            MetaDataRepository.MetaObjectCollection ObjectsLinks = new MetaDataRepository.MetaObjectCollection();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in DataSource.StorageCells)
                ObjectsLinks.AddCollection(((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell));
            ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = objectLinksDataSource.SQLStatament;
            string AliasAssociationTableName = null;
            AliasAssociationTableName = ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);

            #endregion

            #region Construct data source to association table join.

            if (SubObjectCollection.ParticipateInWereClause || (!ThisOrAnyOfParentParticipateInSelectClause))
                FromClauseSQLQuery = " INNER JOIN ";
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";

            FromClauseSQLQuery += AssociationTableStatement + " AS [" + AliasAssociationTableName + "] ON ";

            MetaDataRepository.MetaObjectCollection FirstJoinTableColumns = Classifier.ObjectIDColumns;
            MetaDataRepository.MetaObjectCollection SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");

            string InnerJoinAttributes = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";

                        InnerJoinAttributes += Alias + ".[" + _DataSource.GetColumnName(CurrColumn) + "]";
                        InnerJoinAttributes += " = [" + AliasAssociationTableName + "]";
                        InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;
            #endregion

            #region Construct association table to sub node data source join.
            if (SubObjectCollection.ParticipateInWereClause || (!ThisOrAnyOfParentParticipateInSelectClause))
                FromClauseSQLQuery += " INNER JOIN ";
            else
                FromClauseSQLQuery += " LEFT OUTER JOIN ";

            string subDataNodeFromClauseSQLQuery = SubObjectCollection.BuildFromClauseSQLQuery();

            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += "( ";
            RDBMSMetaDataRepository.MappedClassifier OtherEndClass = (RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
            RDBMSMetaDataRepository.AssociationEnd OtherEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            FromClauseSQLQuery += SubObjectCollection.DataSource.SQLStatament + " AS [" + SubObjectCollection.Alias + "] ";//" Abstract_"+OtherEndClass.Name+" ON ";

            FromClauseSQLQuery += subDataNodeFromClauseSQLQuery;
            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += ") ON ";
            else
                FromClauseSQLQuery += " ON ";



            FirstJoinTableColumns = (OtherEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();

            SecondJoinTableColumns = OtherEndClass.ObjectIDColumns;

            InnerJoinAttributes = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";
                        InnerJoinAttributes += "[" + AliasAssociationTableName + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                        InnerJoinAttributes += ".[" + objectLinksDataSource.GetColumnName(CurrColumn) + "]";
                        InnerJoinAttributes += " = [" + SubObjectCollection.Alias + "].[" + SubObjectCollection.DataSource.GetColumnName(CorrespondingCurrColumn) + "]";
                    }
                }
            }

            FromClauseSQLQuery += " " + InnerJoinAttributes;
            #endregion

            return FromClauseSQLQuery;
        }


        //SubClass
        /// <MetaDataID>{0A885D5D-1809-47A6-8FB9-E22DCD6ACE47}</MetaDataID>
        public string BuildFromClauseSQLQuery()
        {

            string FromClauseSQLQuery = null;
            if (typeof(RDBMSMetaDataRepository.Attribute).IsInstanceOfType(AssignedMetaObject))
                return null;
            if (Classifier == null && (typeof(MetaDataRepository.Namespace).IsInstanceOfType(AssignedMetaObject)))
            {
                foreach (DataNode CurrObjectCollection in SubDataNodes)
                    FromClauseSQLQuery += CurrObjectCollection.BuildFromClauseSQLQuery();
            }

            string query = "(@SELECT order.Name,order.OrderDetails.Price price FROM AbstractionsAndPersistency.Order  order )";
            if (Classifier != null)
            {
                if (DataSource.Empty)
                    return "";

                if (!typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(AssignedMetaObject))
                    FromClauseSQLQuery = DataSource.SQLStatament + " AS [" + Alias + "]";
                foreach (DataNode CurrObjectCollection in SubDataNodes)
                {
                    #region Build table join with sub data node if needed.
                    if (typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(CurrObjectCollection.AssignedMetaObject))
                    {
                        RDBMSMetaDataRepository.AssociationEnd AssociationEnd = (RDBMSMetaDataRepository.AssociationEnd)CurrObjectCollection.AssignedMetaObject;
                        if (AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                        {
                            if ((AssociationEnd.Association.LinkClass == CurrObjectCollection.Classifier && AssociationEnd.Association.Name == CurrObjectCollection.Name) ||	//is type of Class.AssociationClass
                                (AssociationEnd.Association.LinkClass == Classifier && AssociationEnd.Name == CurrObjectCollection.Name))									//is type of AssociationClass.Class
                                FromClauseSQLQuery += BuildAssociationClassTablesJoin(CurrObjectCollection);
                            else
                                FromClauseSQLQuery += BuildManyToManyTablesJoin(CurrObjectCollection);
                        }
                        else
                        {
                            if ((AssociationEnd.Association.LinkClass == CurrObjectCollection.Classifier && AssociationEnd.Association.Name == CurrObjectCollection.Name) ||//is type of AssociationClass.Class
                                (AssociationEnd.Association.LinkClass == Classifier && AssociationEnd.Name == CurrObjectCollection.Name))								//is type of Class.AssociationClass
                                FromClauseSQLQuery += BuildAssociationClassTablesJoin(CurrObjectCollection);
                            else
                                FromClauseSQLQuery += BuildOneToManyTablesJoin(CurrObjectCollection);
                        }
                    }
                    #endregion

                    //Get the sub data node joined tables  
                    //					FromClauseSQLQuery+=CurrObjectCollection.BuildFromClauseSQLQuery();					
                }
            }
            return FromClauseSQLQuery;
        }

        internal void GetDataGroupsSQLQueries(ref string dataGroupsSQLQueries,ref System.Collections.Generic.List<DataNode> tableDatanodes )
        {
            if (Type == DataNodeType.Namespace)
            {
                foreach (DataNode dataNode in SubDataNodes)
                    dataNode.GetDataGroupsSQLQueries(ref dataGroupsSQLQueries, ref tableDatanodes);
            }
            else
            {

                if (ParticipateInSelectClause && Type == DataNodeType.Object && DataSource.DataSourceSelectList != null)
                {

                    dataGroupsSQLQueries += "\nSELECT DISTINCT " + DataSource.DataSourceSelectList + " FROM [TABLE]";
                    tableDatanodes.Add(this); 
                }
                foreach (DataNode dataNode in SubDataNodes)
                    dataNode.GetDataGroupsSQLQueries(ref dataGroupsSQLQueries, ref tableDatanodes);

            }
        }


        internal void BuildTablesRelations(System.Data.DataSet dataSet)
        {
            foreach(DataNode subDataNode in SubDataNodes)
            { 
                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                    (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany
                    &&base.Classifier.LinkAssociation != (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association) 
                {
                    System.Data.DataTable dataNodeTable = null;
                    System.Data.DataTable subDataNodeTable = null;
                    foreach (System.Data.DataTable table in dataSet.Tables)
                    {
                        if (table.TableName == Name + GetHashCode().ToString())
                            dataNodeTable = table;

                        if (table.TableName == subDataNode.Name + subDataNode.GetHashCode().ToString())
                            subDataNodeTable = table;

                        if (dataNodeTable != null && subDataNodeTable != null)
                            break;
                    }
                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    MetaDataRepository.MetaObjectCollection referenceColumns = (associationEnd as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(subDataNode.Classifier as MetaDataRepository.Classifier);
                    if (referenceColumns.Count > 0)
                    {
                        System.Data.DataColumn[] identityDataColumns = new System.Data.DataColumn[referenceColumns.Count];
                        System.Data.DataColumn[] referenceDataColumns = new System.Data.DataColumn[referenceColumns.Count];
                        int i = 0;
                        foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in Classifier.ObjectIDColumns)
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
                            {
                                if (identityColumn.ColumnType == referenceColumn.ColumnType)
                                {
                                    identityDataColumns[i] = dataNodeTable.Columns[identityColumn.Name + GetHashCode().ToString()];
                                    referenceDataColumns[i] = subDataNodeTable.Columns[referenceColumn.Name + subDataNode.GetHashCode().ToString()];
                                    i++;
                                }
                            }
                        }
                        System.Data.DataRelation relation = new System.Data.DataRelation(subDataNode.Name, identityDataColumns, referenceDataColumns);
                        dataSet.Relations.Add(relation);
                    }
                    else
                    {
                        referenceColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(Classifier as MetaDataRepository.Classifier);

                        System.Data.DataColumn[] identityDataColumns = new System.Data.DataColumn[referenceColumns.Count];
                        System.Data.DataColumn[] referenceDataColumns = new System.Data.DataColumn[referenceColumns.Count];
                        int i = 0;
                        foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in subDataNode.Classifier.ObjectIDColumns)
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
                            {
                                if (identityColumn.ColumnType == referenceColumn.ColumnType)
                                {
                                    identityDataColumns[i] = subDataNodeTable.Columns[identityColumn.Name + subDataNode.GetHashCode().ToString()];
                                    referenceDataColumns[i] = dataNodeTable.Columns[referenceColumn.Name + GetHashCode().ToString()];
                                    i++;
                                }
                            }
                        }
                        System.Data.DataRelation relation = new System.Data.DataRelation(subDataNode.Name, identityDataColumns, referenceDataColumns);
                        dataSet.Relations.Add(relation);
                    }
                }
                subDataNode.BuildTablesRelations(dataSet);
            }
        }
    }
}


////SubClass
///// <MetaDataID>{26A4B50D-3712-4ED0-95E8-624C4B73BE37}</MetaDataID>
//private MetaDataRepository.MetaObjectCollection GetStorageCellsOfThisType(MetaDataRepository.Classifier classifier)
//{
//    if (classifier is RDBMSMetaDataRepository.Class)
//        return (classifier as RDBMSMetaDataRepository.Class).StorageCellsOfThisType;

//    if (classifier is MetaDataRepository.Interface)
//    {
//        MetaDataRepository.MetaObjectCollection StorageCells = new OOAdvantech.MetaDataRepository.MetaObjectCollection();
//        foreach (MetaDataRepository.Realization realization in (classifier as MetaDataRepository.Interface).Realizations)
//        {
//            if (realization.Implementor is MetaDataRepository.Class && (realization.Implementor as MetaDataRepository.Class).Persistent)
//            {
//                StorageCells.AddCollection((realization.Implementor as RDBMSMetaDataRepository.Class).StorageCellsOfThisType);
//            }
//        }
//        return StorageCells;
//    }
//    throw new System.Exception("system can't retrieve data from " + classifier.FullName);

//}