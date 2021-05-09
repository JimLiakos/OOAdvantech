namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using SubDataNodeIdentity = System.Guid;
    using MetaDataRepository.ObjectQueryLanguage;
    using ComparisonTermsType = MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;
    using System.Collections.Generic;
    using OOAdvantech.RDBMSDataObjects;
    //using OOAdvantech.RDBMSPersistenceRunTime;


    /// <MetaDataID>{2E7AE9F5-98AD-480C-82B4-6EBE82400E9D}</MetaDataID>
    public class DataLoader : PersistenceLayerRunTime.StorageDataLoader
    {

        /// <summary>
        ///Recursive relation is container containment relation where container and containment have the same type. 
        ///This property provide the parent reference columns
        /// </summary>
        public override System.Collections.Generic.List<string> RecursiveParentReferenceColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    return new System.Collections.Generic.List<string>();
                else
                {
                    System.Collections.Generic.List<string> recursiveDetailsColumns = new System.Collections.Generic.List<string>();
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = associationEnd.GetReferenceColumns(Classifier);
                    if (associationEnd.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                        referenceColumns.Count == 0)
                    {
                        foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            recursiveDetailsColumns.Add(column.Name);
                    }
                    else
                    {
                        if (associationEnd.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                            referenceColumns = associationEnd.GetReferenceColumns();

                        foreach (RDBMSMetaDataRepository.Column column in referenceColumns)
                            recursiveDetailsColumns.Add(column.Name);

                    }
                    return recursiveDetailsColumns;
                }


            }
        }
        /// <summary>
        ///Recursive relation is container containment relation where container and containment have the same type.
        ///This property provide the parent identity columns.
        /// </summary>
        public override System.Collections.Generic.List<string> RecursiveParentColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    return new System.Collections.Generic.List<string>();
                else
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;


                    System.Collections.Generic.List<string> recursiveParentColumns = new System.Collections.Generic.List<string>();


                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = associationEnd.GetReferenceColumns(Classifier);
                    if (associationEnd.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                        referenceColumns.Count > 0)
                    {

                        foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            recursiveParentColumns.Add(column.Name);
                    }
                    else if (associationEnd.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                    {
                        foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            recursiveParentColumns.Add(column.Name);
                    }
                    else
                    {
                        foreach (RDBMSMetaDataRepository.Column column in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                            recursiveParentColumns.Add(column.Name);

                    }

                    return recursiveParentColumns;

                }

            }
        }
        public override bool HasRelationIdentityColumns
        {
            get
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = associationEnd.GetReferenceColumns(Classifier);
                    return referenceColumns.Count == 0;
                }
                return false;

            }
        }
        public OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage
        {
            get
            {
                StorageProvider storageProvider = new StorageProvider();
                return PersistenceLayer.ObjectStorage.OpenStorage(LoadFromStorage.StorageName, LoadFromStorage.StorageLocation, typeof(StorageProvider).FullName);
            }
        }

        RDBMSMetaDataRepository.StorageCell LastStorageCell = null;
        public override object GetObject(System.Data.DataRow row, out bool loadObjectLinks)
        {

            lock (ObjectStorage)
            {
                loadObjectLinks = false;
                System.Data.DataRow StorageInstance = row;
                object FieldValue = StorageInstance["ObjectID"];
                if (FieldValue is System.DBNull)
                    return null;
                if (FieldValue == null)
                    return null;
                object objectID = (System.Guid)FieldValue;
                int storageCellID = (int)StorageInstance["StorageCellID"];
                if (LastStorageCell == null || LastStorageCell.SerialNumber != storageCellID)
                {
                    LastStorageCell = null;
                    foreach (RDBMSMetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (storageCell.SerialNumber == storageCellID)
                        {
                            LastStorageCell = storageCell;
                            break;
                        }
                    }
                }
                if (LastStorageCell == null)
                    throw new System.Exception("System can't retrieve storage cell");

                objectID = new ObjectID((System.Guid)objectID, storageCellID);
                object _object;

                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                   (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, default(string), LastStorageCell);

                }
                else
                {



                    StorageInstanceRef storageInstanceRef = (ObjectStorage as PersistenceLayerRunTime.ObjectStorage).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(LastStorageCell.Type.FullName, "")][objectID] as StorageInstanceRef;
                    if (storageInstanceRef != null)
                    {
                        _object = storageInstanceRef.MemoryInstance;
                    }
                    else
                    {
                        storageInstanceRef = (StorageInstanceRef)(ObjectStorage as ObjectStorage).CreateStorageInstanceRef(AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(LastStorageCell.Type.FullName, "")), objectID);
                        storageInstanceRef.StorageInstanceSet = LastStorageCell;
                        storageInstanceRef.DbDataRecord = StorageInstance;
                        //if (storageInstanceRef.Class.ClassHierarchyLinkAssociation != null)
                        //{
                        //    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                        //    object RoleA = null;
                        //    object RoleB = null;
                        //    GetRoleObjectsFromRelationObject(ref RoleA, ref RoleB);
                        //    System.Reflection.FieldInfo FieldRoleA = (mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                        //    System.Reflection.FieldInfo FieldRoleB = (mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleBField; ;
                        //    FieldRoleA.SetValue(storageInstanceRef.MemoryInstance, RoleA);
                        //    FieldRoleB.SetValue(storageInstanceRef.MemoryInstance, RoleB);
                        //}
                        //GetRelatedObjects(mStorageInstanceRef);
                        storageInstanceRef.LoadObjectState(null);
                        //storageInstanceRef.SnapshotStorageInstance();
                        storageInstanceRef.ObjectActived();
                        _object = storageInstanceRef.MemoryInstance;

                        // storageInstanceRef.ObjectID = objectID;
                        loadObjectLinks = true;
                    }
                }

                return _object;
            }
        }
        /// <exclude>Excluded</exclude>
        private MetaDataRepository.Storage _LoadFromStorage;
        /// <MetaDataID>{B3A0D5F3-550E-43D3-A2D6-4CB2C1BF3486}</MetaDataID>
        public override MetaDataRepository.Storage LoadFromStorage
        {
            get
            {
                if (_LoadFromStorage == null)
                {
                    foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageCell))
                            throw new System.Exception("System can't access storage when data loader live in different process, than the storage.");
                        _LoadFromStorage = storageCell.Namespace as Storage;
                        break;
                    }
                }
                return _LoadFromStorage;
            }
        }
        /// <exclude>Excluded</exclude>
        private MetaDataRepository.Classifier _Classifier;
        /// <summary>This classifier is the corresponding of the data node classifier and keeps the mapping data of classifier with the data object of RDBMS. </summary>
        /// <MetaDataID>{ADD47CF9-27E6-434E-928E-495C1AEE5FEE}</MetaDataID>
        public override MetaDataRepository.Classifier Classifier
        {
            get
            {
                if (_Classifier == null)
                {
                    if (DataLoaderMetadata.StorageCells.Count == 0)
                        throw new System.Exception("Error in data loading");
                    foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageCell))
                            throw new System.Exception("System can't access classifier when data loader live in different process, than the storage.");

                        _LoadFromStorage = storageCell.Namespace as Storage;
                        if (storageCell.Type.Identity == DataNode.Classifier.Identity)
                        {
                            _Classifier = storageCell.Type;
                        }
                        else
                        {
                            if (DataNode.Classifier is MetaDataRepository.Structure && (DataNode.Classifier as MetaDataRepository.Structure).Persistent)
                                if (storageCell.Type.Identity == DataNode.ParentDataNode.Classifier.Identity)
                                {
                                    _Classifier = storageCell.Type;
                                    return _Classifier;
                                }
                            foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
                            {
                                if (classifier.Identity == DataNode.Classifier.Identity)
                                {
                                    _Classifier = classifier;
                                    break;
                                }
                                else
                                {
                                    if (DataNode.Classifier is MetaDataRepository.Structure && (DataNode.Classifier as MetaDataRepository.Structure).Persistent)
                                        if (classifier.Identity == DataNode.ParentDataNode.Classifier.Identity)
                                        {
                                            _Classifier = classifier;
                                            return _Classifier;
                                        }
                                }
                            }
                        }
                        break;
                    }
                    if (_Classifier == null)
                    {
                        _Classifier = (_LoadFromStorage as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
                        if (_Classifier != null)
                            return _Classifier;
                        throw new System.Exception("System can't find RDBMS meta object for " + DataNode.Classifier.FullName);
                    }


                }
                return _Classifier;

            }
        }
        /// <exclude>Excluded</exclude>
        private OOAdvantech.RDBMSMetaDataRepository.View _View;
        /// <MetaDataID>{88EACB40-1FC2-4C72-AFC1-043F3C388B25}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataRepository.View View
        {
            get
            {
                if (_View == null)
                {
                    RDBMSMetaDataRepository.MappedClassifier mappedClassifier = Classifier as RDBMSMetaDataRepository.MappedClassifier;
                    _View = mappedClassifier.GetTypeView(new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>(DataLoaderMetadata.StorageCells));
                }
                return _View;
            }
        }

        /// <MetaDataID>{C19A1F76-8006-418A-8B93-B02FB5E2AEF6}</MetaDataID>
        private ObjectStorage Storage;

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<DataNode, bool> _RelatedDataNodes;
        System.Collections.Generic.Dictionary<DataNode, bool> RelatedDataNodes
        {
            get
            {
                if (_RelatedDataNodes != null)
                    return new System.Collections.Generic.Dictionary<DataNode, bool>(_RelatedDataNodes);
                _RelatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> localResolvedCriterionsRoutes = LocalResolvedCriterionsRoutes;
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    foreach (System.Collections.Generic.Stack<DataNode> route in entry.Value)
                    {
                        foreach (DataNode dataNode in route)
                        {
                            if (dataNode != DataNode)
                            {
                                if (!_RelatedDataNodes.ContainsKey(dataNode))
                                {
                                    if (entry.Key.IsNULL && (entry.Key.FirstDataNode == dataNode || entry.Key.SecondDataNode == dataNode))
                                        _RelatedDataNodes.Add(dataNode, false);
                                    else
                                        _RelatedDataNodes.Add(dataNode, true);
                                }
                                else
                                {
                                    if (entry.Key.IsNULL && (entry.Key.FirstDataNode == dataNode || entry.Key.SecondDataNode == dataNode))
                                        _RelatedDataNodes[dataNode] = false;
                                }
                            }
                        }
                    }
                }
                return new System.Collections.Generic.Dictionary<DataNode, bool>(_RelatedDataNodes);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4B49971A-4B59-45A2-BD88-507969AE4E8F}</MetaDataID>
        private string _SQLStatament;
        /// <MetaDataID>{BF3A0DFA-447E-4160-80C8-1D344DCD4BD5}</MetaDataID>
        private string SQLStatament
        {
            get
            {
                lock (ObjectStorage)
                {

                    if (_SQLStatament != null)
                        return _SQLStatament;
                    _SQLStatament = null;
                    DataLoader rootDataLoader = this;// GetRootDataLoader();



                    _SQLStatament = " FROM " + rootDataLoader.BuildFromClauseSQLQuery(rootDataLoader, RelatedDataNodes) + " ";

                    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
                    {
                        if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                        {
                            string containsDataName = DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName;
                            _SQLStatament += "\n LEFT OUTER JOIN (" + GetContainsAnyAllSQL(criterion) + ") as [" + containsDataName + "] ";
                            string joinStatament = null;
                            foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            {
                                if (joinStatament == null)
                                    joinStatament = " ON  ";
                                else
                                    joinStatament += ",";
                                joinStatament += "[" + DataNode.Alias + "].[" + column.Name + "]";
                                joinStatament += "=[" + containsDataName + "].[" + column.Name + "]";
                            }
                            _SQLStatament += joinStatament;

                        }
                    }

                    string whereClause = null;
                    if (SearchCondition != null)
                        whereClause = GetSQLFilterStatament(SearchCondition);
                    if (!string.IsNullOrEmpty(whereClause))
                        whereClause = " WHERE " + whereClause;



                    //foreach (SearchCondition filter in filters)
                    //{
                    //    string filterStatement = GetSQLFilterStatament(filter);
                    //    if (filterStatement == null)
                    //        continue;
                    //    if (whereClause == null)
                    //    {
                    //        whereClause = " WHERE ";
                    //    }
                    //    else
                    //        whereClause += " AND ";

                    //    whereClause += filterStatement;
                    //}
                    _SQLStatament += whereClause;
                    string selectClause = null;
                    string orderByClause = null;


                    foreach (DataColumn column in GetSqlStatamentDataColoumns())
                    {


                        if (selectClause == null)
                            selectClause = "SELECT DISTINCT [" + DataNode.Alias + "].[StorageIdentity],";
                        else
                            selectClause += ",";
                        selectClause += "[" + DataNode.Alias + "].[" + column.Name + "]";
                        if (column.Alias != null)
                            selectClause += " as [" + column.Alias + "]";
                    }
                    string dropTemTables = null;
                    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
                    {
                        if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                        {
                            selectClause += ",[" + DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName;
                            if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
                                selectClause += "].[ContainsAny] ";
                            else
                                selectClause += "].[ContainsAll] ";
                            if (dropTemTables != null)
                                dropTemTables += "\n";

                            dropTemTables += "DROP TABLE #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName;
                        }
                    }
                    bool singleRow = false;
                    string countAlias = null;
                    foreach (DataNode memberDataNode in DataNode.SubDataNodes)
                    {
                        if (memberDataNode.Type == DataNode.DataNodeType.Count && GlobalResolveCriterions.Count == 0)
                        {

                            if (memberDataNode.AggregateExpressionDataNode == null)
                            {
                                singleRow = true;
                                foreach (DataNode selectionDataNode in DataNode.SubDataNodes)
                                {
                                    if (memberDataNode != selectionDataNode && selectionDataNode.BranchParticipateInSelectClause)
                                    {
                                        singleRow = false;
                                        break;
                                    }
                                }
                                if (!singleRow)
                                {
                                    if (selectClause == null)
                                        selectClause = "SELECT DISTINCT [" + DataNode.Alias + "].[StorageIdentity],";
                                    else
                                        selectClause += ",";
                                    selectClause += "Count(*) as " + memberDataNode.Alias + " ";
                                }
                                else
                                    countAlias = memberDataNode.Alias;
                            }
                            else
                            {

                                System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                                DataNode parentDataNode = memberDataNode.AggregateExpressionDataNode;
                                while (parentDataNode != DataNode)
                                {
                                    relatedDataNodes.Add(parentDataNode, true);
                                    parentDataNode = parentDataNode.ParentDataNode;
                                }
                                string fromClauseSQLQuery = null;
                                DataLoader headerDataloader = this;

                                string joinWhereAttributes = null;

                                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                                {
                                    if (!relatedDataNodes.ContainsKey(subDataNode))
                                        continue;
                                    bool innerJoin = true;
                                    relatedDataNodes.Remove(subDataNode);

                                    #region Build table join with sub data node if needed.
                                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                                    {

                                        //RDBMSMetaDataRepository.AssociationEnd associationEnd = (RDBMSMetaDataRepository.AssociationEnd)Storage.GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                                        MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                        if (associationEnd.Association.Specializations.Count > 0)
                                        {
                                            fromClauseSQLQuery += BuildManyToManyCountedItems(subDataNode, relatedDataNodes, out joinWhereAttributes);
                                        }
                                        else
                                        {
                                            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                                            {
                                                DotNetMetaDataRepository.Attribute roleAAttribute = null;
                                                DotNetMetaDataRepository.Attribute roleBAttribute = null;
                                                if (associationEnd.Association.LinkClass != null)
                                                {
                                                    foreach (DotNetMetaDataRepository.Attribute attribute in associationEnd.Association.LinkClass.GetAttributes(true))
                                                    {
                                                        if (attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole") != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole"))
                                                        {
                                                            if ((bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA"))
                                                                roleAAttribute = attribute;
                                                            else
                                                                roleBAttribute = attribute;
                                                        }
                                                    }


                                                }
                                                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||	//is type of Class.AssociationClass
                                                    (associationEnd.Association.LinkClass == DataNode.Classifier && (associationEnd.Name == subDataNode.Name || roleAAttribute.Name == subDataNode.Name || roleBAttribute.Name == subDataNode.Name)))									//is type of AssociationClass.Class
                                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                                else
                                                    fromClauseSQLQuery += BuildManyToManyCountedItems(subDataNode, relatedDataNodes, out joinWhereAttributes);
                                            }
                                            else
                                            {
                                                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||//is type of AssociationClass.Class
                                                    (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))								//is type of Class.AssociationClass
                                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                                else
                                                    fromClauseSQLQuery += BuildOneToManyCountedItems(subDataNode, relatedDataNodes, out joinWhereAttributes);
                                            }
                                        }
                                    }
                                    else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                    {
                                        fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, headerDataloader, relatedDataNodes, true);
                                    }
                                    #endregion
                                }
                                //fromClauseSQLQuery = fromClauseSQLQuery.Substring(" INNER JOIN".Length);



                                string whereClauseSQLQuery = null;// whereClause;
                                if (string.IsNullOrEmpty(whereClauseSQLQuery))
                                    whereClauseSQLQuery += "\nWhere ";

                                whereClauseSQLQuery += "(" + joinWhereAttributes + ")";
                                if (memberDataNode.AggregateExpressionDataNode != null && memberDataNode.AggregateExpressionDataNode.SearchCondition != null)
                                    whereClauseSQLQuery += "and (" + GetSQLFilterStatament(memberDataNode.AggregateExpressionDataNode.SearchCondition) + ")";


                                // fromClauseSQLQuery = fromClauseSQLQuery.Substring(0, fromClauseSQLQuery.LastIndexOf("ON"));


                                string selectListItem = "Select Count(*) \nFrom " + fromClauseSQLQuery + whereClauseSQLQuery;
                                if (selectClause == null)
                                    selectClause = "SELECT DISTINCT [" + DataNode.Alias + "].[StorageIdentity],";
                                selectClause += ",(" + selectListItem + ") as [" + memberDataNode.Alias + "] ";
                            }
                        }

                    }


                    foreach (DataColumn column in GetOrderByDataColoumns())
                    {
                        if (orderByClause == null)
                            orderByClause = " ORDER BY ";
                        else
                            orderByClause += ",";
                        orderByClause += "[" + DataNode.Alias + "].[" + column.Name + "]";
                    }


                    //                orderByClause = null;

                    _SQLStatament = selectClause + " " + _SQLStatament + " " + orderByClause;

                    string tmpquery = null;
                    if (DataNode.Recursive)
                    {

                        _SQLStatament = BuildRecursiveLoad((DataNode.ParentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).SQLStatament);
                    }

                    if (singleRow)
                        _SQLStatament = "SELECT COUNT(*) as " + countAlias + " FROM (" + _SQLStatament + ") as Count" + DataNode.Alias;

                    _SQLStatament += dropTemTables;
                    return _SQLStatament;

                }
            }
        }

        private DataLoader GetRootDataLoader()
        {
            DataNode parentDataNode = DataNode.ParentDataNode;
            DataLoader rootDataLoader = this;
            while (parentDataNode.DataSource != null && parentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
            {
                rootDataLoader = parentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

                if (parentDataNode.ParentDataNode == null)
                    return rootDataLoader;
                else
                    parentDataNode = parentDataNode.ParentDataNode;
            }
            return rootDataLoader;





        }


        /// <MetaDataID>{D8F4F951-2F96-414B-83AF-DFB22794FA8D}</MetaDataID>
        public override void LoadDataLocally()
        {
            if (!DataNode.BranchParticipateInSelectClause && !DataNode.ParticipateInSelectClause && GlobalResolveCriterions.Count == 0)
            {
                foreach (RDBMSMetaDataRepository.Column column in View.ViewColumns)
                    Data.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                Data.Columns.Add("StorageCellID", typeof(int));
                Data.Columns.Add("StorageIdentity", typeof(int));
                return;
            }
            //if(!DataNode.BranchParticipateInSelectClause&&!DataNode.ParticipateInSelectClause
            if (!DataNode.ParticipateInSelectClause && !DataNode.DataNodeMembersParticipateInSelectClause)
            {
                bool allCriterionsAppliedLocaly = true;
                foreach (Criterion criterion in DataNode.BranchSearchCriteria)
                {
                    if (!criterion.Applied)
                    {
                        allCriterionsAppliedLocaly = false;
                        break;
                    }
                }
                //if (allCriterionsAppliedLocaly)
                //  return;
            }
            string slqstment = SQLStatament;


            System.Data.SqlClient.SqlConnection connection = (PersistenceLayer.ObjectStorage.GetStorageOfObject(LoadFromStorage.Properties) as MSSQLFastPersistenceRunTime.ObjectStorage).Connection as System.Data.SqlClient.SqlConnection;
            lock (connection)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                TransferCollectionsDataToServer();


                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(SQLStatament, connection);
                System.Data.SqlClient.SqlDataReader dataReader = command.ExecuteReader();
                Data.Load(dataReader);
                dataReader.Close();


                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {

                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && ((
                        (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                        (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != DataNode.Classifier.ClassHierarchyLinkAssociation &&
                        (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != subDataNode.Classifier.ClassHierarchyLinkAssociation) || (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0))
                    {
                        RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                        DataTable ManyToManyRelationData = new DataTable(false);

                        //if (RelatedDataNodes.ContainsKey(subDataNode))
                        {
                            //RelatedDataNodes[subDataNode]

                            if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count == 0)
                                command = new System.Data.SqlClient.SqlCommand(BuildManyToManyTablesJoin(subDataNode, true, null, RelatedDataNodes, true), connection);
                            else
                                command = new System.Data.SqlClient.SqlCommand(BuildAbstractAssociationTablesJoin(subDataNode, true, null, RelatedDataNodes, true), connection);


                            dataReader = command.ExecuteReader();
                            ManyToManyRelationData.Load(dataReader);
                        }
                        if (ManyToManyRelationData.Rows.Count == 0)
                            ManyToManyRelationData = new DataTable(false);

                        dataReader.Close();
                        OOAdvantech.Collections.Generic.List<string> parentRelationColumns = new OOAdvantech.Collections.Generic.List<string>();
                        OOAdvantech.Collections.Generic.List<string> childRelationColumns = new OOAdvantech.Collections.Generic.List<string>();
                        foreach (RDBMSMetaDataRepository.Column column in associationEnd.GetReferenceColumns())
                        {
                            parentRelationColumns.Add(column.Name);
                            if (ManyToManyRelationData.Rows.Count == 0)
                                ManyToManyRelationData.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));

                        }

                        foreach (RDBMSMetaDataRepository.Column column in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                        {
                            childRelationColumns.Add(column.Name);
                            if (ManyToManyRelationData.Rows.Count == 0)
                                ManyToManyRelationData.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));
                        }

                        RelationshipData relationshipData = new RelationshipData(parentRelationColumns, childRelationColumns, ManyToManyRelationData);
                        if (!RelationshipsData.ContainsKey((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString()))
                            RelationshipsData.Add((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString(), relationshipData);

                    }
                }
            }


            //RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;


            //Load ParentRelationColumns
            int count = DataSourceRelationsColumnsWithSubDataNodes.Count;
            //Load ChildRelationColumns
            count = DataSourceRelationsColumnsWithParent.Count;

            if (DataNode.ParticipateInSelectClause)
                LoadObjects();

            int tt = 0;

        }

        string BuildContainsAnyAllInnerJoinRoute(DataNode criterionDataNode)
        {
            string query = BuildSQLStatament() + " AS [" + DataNode.Alias + "]";





            //foreach (DataNode subDataNode in DataNode.SubDataNodes)
            //{
            //    if (criterionDataNode.IsSameOrParentDataNode(subDataNode))
            //    {
            //        if (typeof(MetaDataRepository.AssociationEnd).IsInstanceOfType(subDataNode.AssignedMetaObject))
            //        {
            //            MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

            //            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
            //            {
            //                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||	//is type of Class.AssociationClass
            //                    (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))									//is type of AssociationClass.Class
            //                    query += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
            //                else
            //                    query += BuildManyToManyTablesJoin(subDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
            //            }
            //            else
            //            {
            //                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||//is type of AssociationClass.Class
            //                    (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))								//is type of Class.AssociationClass
            //                    query += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
            //                else
            //                    query += BuildOneToManyTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
            //            }
            //        }
            //    }
            //}

            return null;
        }

        private void TransferCollectionsDataToServer()
        {
            foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
            {
                if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                {

                    string parameterName = null;
                    object parameterValue = null;

                    if (criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
                    {
                        parameterName = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterName;
                        parameterValue = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
                    }

                    MetaDataRepository.MetaObjectsStack oldMetaObjectCreator = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator;
                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack();
                    DataObjects.DataBase dataBase = new DataObjects.DataBase(LoadFromStorage as Storage, "tmp");
                    OOAdvantech.RDBMSMetaDataRepository.Table table = new OOAdvantech.RDBMSMetaDataRepository.Table();
                    table.Name = parameterName;
                    System.Data.SqlClient.SqlConnection connection = (PersistenceLayer.ObjectStorage.GetStorageOfObject(LoadFromStorage.Properties) as MSSQLFastPersistenceRunTime.ObjectStorage).Connection as System.Data.SqlClient.SqlConnection;
                    System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(connection);
                    sqlBulkCopy.DestinationTableName = "#" + parameterName;
                    System.Data.DataTable dataTable = new System.Data.DataTable(parameterName);
                    Table tableMetaData = new Table("B_Test", true, TableType.TemporaryTable, dataBase);

                    if (criterion.FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {

                        RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                        int length = attribute.GetPropertyValue<int>("Persistent", "SizeOf");
                        foreach (MetaDataRepository.AttributeRealization attributeRealization in attribute.AttributeRealizations)
                        {
                            int attributeRealizationLength = attributeRealization.GetPropertyValue<int>("Persistent", "SizeOf");
                            if (attributeRealizationLength > length)
                                length = attributeRealizationLength;
                        }
                        table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(attribute.Name, attribute.Type, length, true, false, 0));
                        foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));


                        foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
                        {
                            System.Data.DataRow dataRow = dataTable.NewRow();
                            dataRow[attribute.Name] = value;
                            dataTable.Rows.Add(dataRow);
                        }

                    }
                    else
                    {


                        foreach (RDBMSMetaDataRepository.Column column in ((LoadFromStorage as Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(column));

                        foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                            dataTable.Columns.Add(column.Name, ModulePublisher.ClassRepository.GetType(column.Type.FullName, ""));


                        foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
                        {
                            ObjectID objectID = null;
                            try
                            {
                                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(value) as StorageInstanceRef;
                                if (storageInstanceRef == null)
                                    objectID = new ObjectID(System.Guid.NewGuid(), 0);
                                else
                                    objectID = storageInstanceRef.ObjectID as ObjectID;
                            }
                            catch
                            {
                                objectID = new ObjectID(System.Guid.NewGuid(), 0);
                            }
                            StorageInstanceRef.GetStorageInstanceRef(value);
                            System.Data.DataRow dataRow = dataTable.NewRow();
                            foreach (RDBMSMetaDataRepository.IdentityColumn column in ((LoadFromStorage as Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                                dataRow[column.Name] = objectID.GetMemberValue(column.ColumnType);
                            dataTable.Rows.Add(dataRow);
                        }

                    }

                    tableMetaData.Synchronize(table);

                    tableMetaData.Update();
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = oldMetaObjectCreator;
                    sqlBulkCopy.WriteToServer(dataTable);

                }

            }


        }

        private string GetContainsAnyAllSQL(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
            {
                System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                DataNode dataNode = criterion.FilteredDataNode;
                if (dataNode.Type == DataNode.DataNodeType.Object)
                    relatedDataNodes.Add(dataNode, true);
                while (dataNode.ParentDataNode != DataNode)
                {
                    dataNode = dataNode.ParentDataNode;
                    relatedDataNodes.Add(dataNode, true);
                }


                string query = BuildFromClauseSQLQuery(this, relatedDataNodes);

                string selectionList = null;
                foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                {
                    if (selectionList == null)
                        selectionList = "SELECT DISTINCT ";
                    else
                        selectionList += ",";
                    selectionList += "[" + DataNode.Alias + "].[" + column.Name + "]";
                }


                if (criterion.FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (criterion.FilteredDataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    {
                        query = selectionList + " FROM " + query + "INNER JOIN #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + " on ";
                        query += "[" + criterion.FilteredDataNode.ParentDataNode.Alias + "].[" + criterion.FilteredDataNode.Name + "] = #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + ".[" + criterion.FilteredDataNode.Name + "]";
                    }
                    else
                    {
                        selectionList += ",0 as [ContainsAny] ";
                        query = selectionList + " FROM " + query;
                    }

                }
                else
                {
                    if (criterion.FilteredDataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    {
                        query = selectionList + " FROM " + query + "INNER JOIN #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + " on ";
                        string joinCriterion = null;
                        foreach (RDBMSMetaDataRepository.Column column in (criterion.FilteredDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        {
                            if (joinCriterion != null)
                                joinCriterion += " AND ";
                            joinCriterion += "[" + criterion.FilteredDataNode.Alias + "].[" + column.Name + "] = #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + ".[" + column.Name + "]";
                        }
                        query += joinCriterion;
                    }
                    {
                        selectionList += ",0 as [ContainsAny] ";
                        query = selectionList + " FROM " + query;
                    }
                }
                return query;
            }
            else
            {
                System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                DataNode dataNode = criterion.FilteredDataNode;
                if (dataNode.Type == DataNode.DataNodeType.Object)
                    relatedDataNodes.Add(dataNode, true);
                while (dataNode.ParentDataNode != DataNode)
                {
                    dataNode = dataNode.ParentDataNode;
                    relatedDataNodes.Add(dataNode, true);
                }

                string query = BuildFromClauseSQLQuery(this, relatedDataNodes);
                string selectionList = null;
                foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                {
                    if (selectionList == null)
                        selectionList = "SELECT DISTINCT ";
                    else
                        selectionList += ",";
                    selectionList += "[" + DataNode.Alias + "].[" + column.Name + "]";
                }





                if (criterion.FilteredDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (criterion.FilteredDataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    {
                        selectionList += ",[" + criterion.FilteredDataNode.ParentDataNode.Alias + "].[" + criterion.FilteredDataNode.Name + "] as [FilteredWith" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + "]";
                        query = selectionList + " FROM " + query + "INNER JOIN #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + " ON ";
                        query += "[" + criterion.FilteredDataNode.ParentDataNode.Alias + "].[" + criterion.FilteredDataNode.Name + "] = #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + ".[" + criterion.FilteredDataNode.Name + "]";
                    }
                    else
                        query = selectionList + "\n FROM " + query;
                }
                else
                {
                    if (criterion.FilteredDataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    {
                        foreach (RDBMSMetaDataRepository.Column column in ((LoadFromStorage as Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            selectionList += ",[" + criterion.FilteredDataNode.Alias + "].[" + column.Name + "] as [FilteredWith" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + "]";


                        query = selectionList + " FROM " + query + "INNER JOIN #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + " ON ";
                        string joinCriterion = null;
                        foreach (RDBMSMetaDataRepository.Column column in ((LoadFromStorage as Storage).GetEquivalentMetaObject(criterion.FilteredDataNode.Classifier) as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        {
                            if (joinCriterion != null)
                                joinCriterion += " AND ";
                            joinCriterion += "[" + criterion.FilteredDataNode.Alias + "].[" + column.Name + "] = #" + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + ".[" + column.Name + "]";
                            //slectionList += ",[" + criterion.FilteredDataNode.Alias + "].[" + column.Name + "]";
                        }
                        query += joinCriterion;
                    }
                    else
                        query = selectionList + "\n FROM " + query;



                }
                query = " FROM ( " + query + " ) as [" + DataNode.Alias + "HasAll]";

                string containsSelect = null;
                //query = "SELECT [" + DataNode.Alias + "HasAll].* " + ",1 as [ContainsAll] \n" + query;

                foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                {
                    if (containsSelect == null)
                        containsSelect = "SELECT ";
                    else
                        containsSelect += ",";
                    containsSelect += "[" + DataNode.Alias + "HasAll].[" + column.Name + "]";
                }
                if (criterion.FilteredDataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    containsSelect += ",1 as [ContainsAll] \n";
                else
                {
                    containsSelect += ",0 as [ContainsAll] \n";
                    query = containsSelect + query;
                    return query;
                }


                string groupBy = null;
                foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                {
                    if (groupBy == null)
                        groupBy = "\nGROUP BY ";
                    else
                        groupBy += ",";
                    groupBy += "[" + DataNode.Alias + "HasAll].[" + column.Name + "]";
                }
                query = query + groupBy;
                query = query + string.Format("\nHAVING  COUNT(*) =(SELECT COUNT(*) from #{0})", (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName);

                return query;
                //slectionList += ",1 as [ContainsAny] ";

            }
        }

        ///// <MetaDataID>{AF37747C-46FD-40EB-8B10-3480839B847D}</MetaDataID>
        //private System.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.SearchCondition> GetFilters(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        //{
        //    System.Collections.Generic.List<SearchCondition> filters = new System.Collections.Generic.List<SearchCondition>();
        //    if (dataNode.LocalFilter != null)
        //        filters.Add(dataNode.LocalFilter);
        //    foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //    {
        //        filters.AddRange(GetFilters(subDataNode));
        //    }
        //    return filters;


        //}



        /// <MetaDataID>{41744191-A0EB-4CFC-A916-6B2CFFA85990}</MetaDataID>
        private string GetSQLFilterStatament(MetaDataRepository.ObjectQueryLanguage.SearchCondition searchCondition)
        {
            string sqlExpresion = null;
            foreach (MetaDataRepository.ObjectQueryLanguage.SearchTerm searchTerm in searchCondition.SearchTerms)
            {

                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    if (searchTerm.Criterions.Contains(entry.Key))
                    {
                        string searchTermSQLExpression = GetSearchTermSQLExpression(searchTerm);

                        if (sqlExpresion == null)
                        {
                            if (searchTermSQLExpression != null)
                                sqlExpresion = "(" + searchTermSQLExpression;
                        }
                        else
                        {

                            if (searchTermSQLExpression != null)
                                sqlExpresion += " OR " + searchTermSQLExpression;
                        }
                        break;

                    }
                }


            }
            if (sqlExpresion != null)
                sqlExpresion += ")";

            return sqlExpresion;

        }

        /// <MetaDataID>{C8695950-5DCE-4E5C-8AB1-C484EFBF04D8}</MetaDataID>
        private string GetSearchTermSQLExpression(MetaDataRepository.ObjectQueryLanguage.SearchTerm searchTerm)
        {
            string sqlExpression = null;
            foreach (MetaDataRepository.ObjectQueryLanguage.SearchFactor searchFactor in searchTerm.SearchFactors)
            {
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    if (searchFactor.Criterions.Contains(entry.Key))
                    {


                        if (sqlExpression == null)
                            sqlExpression = GetSearchFactorSQLExpression(searchFactor);
                        else
                        {
                            string searchFactorSQLExpression = GetSearchFactorSQLExpression(searchFactor);
                            if (searchFactorSQLExpression != null)
                                sqlExpression += " AND " + searchFactorSQLExpression;
                        }
                        break;
                    }
                }
            }
            if (searchTerm.IsNotExpression && sqlExpression != null)
                sqlExpression = "NOT " + sqlExpression;

            return sqlExpression;

        }

        /// <MetaDataID>{EB6554D5-2D0D-4677-89F1-CA3F8F888AF5}</MetaDataID>
        private string GetSearchFactorSQLExpression(MetaDataRepository.ObjectQueryLanguage.SearchFactor searchFactor)
        {
            string sqlExpression = null;
            if (searchFactor.SearchCondition != null)
                sqlExpression = GetSQLFilterStatament(searchFactor.SearchCondition);
            else
                sqlExpression = GetCriterionSQLExpression(searchFactor.Criterion);

            if (searchFactor.IsNotExpression)
                sqlExpression = "NOT " + sqlExpression;
            return sqlExpression;


        }

        public string GetCriterionSQLExpression(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            string compareExpression = null;
            string comparisonOperator = null;
            if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal)
                comparisonOperator = " = ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual)
                comparisonOperator = " <> ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThan)
                comparisonOperator = " > ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThanEqual)
                comparisonOperator = " >= ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThan)
                comparisonOperator = " < ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThanEqual)
                comparisonOperator = " <= ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                comparisonOperator += " LIKE ";
            if (criterion.ComparisonOperator == MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType.Equal)
                comparisonOperator = " = ";
            if (criterion.IsNULL)
                comparisonOperator = " IS ";
            if (criterion.IsNotNULL)
                comparisonOperator = " IS NOT ";


            switch (criterion.CriterionType)
            {

                case ComparisonTermsType.ObjectsAttributes:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm firstComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm secondComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        compareExpression += "[" + firstComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                        compareExpression += ".";
                        compareExpression += "[" + firstComparisonTerm.DataNode.Name + "]";
                        compareExpression += comparisonOperator;
                        compareExpression += "[" + secondComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                        compareExpression += ".";
                        compareExpression += "[" + secondComparisonTerm.DataNode.Name + "]";
                        return compareExpression;

                    }
                case ComparisonTermsType.ObjectAttributeWithLiteral:
                case ComparisonTermsType.LiteralWithObjectAttribute:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm objectAttributeComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        else
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm literalComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm)
                            literalComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm;
                        else
                            literalComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm;

                        string literalValue = null;
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                            literalValue = GetLikeString(literalComparisonTerm.Value as string, literalValue);
                        else
                            literalValue = TypeDictionary.ConvertToSQLString(literalComparisonTerm.Value);

                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm ||
                            (criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm &&
                            (criterion.IsNotNULL || criterion.IsNotNULL)))
                        {
                            compareExpression = "[" + objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                            compareExpression += ".";

                            if (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                            {
                                RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                                string columnName = attribute.GetAttributeColumnName(objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                                compareExpression += "[" + columnName + "]";
                            }
                            else
                                compareExpression += "[" + (objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "]";

                            compareExpression += comparisonOperator;
                            compareExpression += literalValue;
                        }
                        else
                        {
                            compareExpression += literalValue;
                            compareExpression += comparisonOperator;
                            compareExpression += "[" + objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                            compareExpression += ".";
                            if (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                            {
                                RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                                string columnName = attribute.GetAttributeColumnName(objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                                compareExpression += "[" + columnName + "]";
                            }
                            else
                                compareExpression += "[" + (objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "]";

                        }
                        return compareExpression;
                    }
                case ComparisonTermsType.ObjectAttributeWithParameter:
                case ComparisonTermsType.ParameterWithObjectAttribute:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm objectAttributeComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        else
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;

                        MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm parameterComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
                            parameterComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm;
                        else
                            parameterComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm;

                        string parameterValue = null;
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                            parameterValue = GetLikeString(parameterComparisonTerm.ParameterValue as string, parameterValue);
                        else
                            parameterValue = TypeDictionary.ConvertToSQLString(parameterComparisonTerm.ParameterValue);

                        if ((parameterValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.Equal))
                        {
                            compareExpression = "[" + objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                            compareExpression += ".";
                            compareExpression += "[" + objectAttributeComparisonTerm.DataNode.Name + "]";
                            compareExpression += "IS NULL";
                            return compareExpression;
                        }
                        else if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                        {
                            compareExpression = "[" + objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                            compareExpression += ".";
                            compareExpression += "[" + objectAttributeComparisonTerm.DataNode.Name + "]";
                            compareExpression += comparisonOperator;
                            compareExpression += parameterValue;
                            return compareExpression;
                        }
                        else
                        {
                            compareExpression += parameterValue;
                            compareExpression += comparisonOperator;
                            compareExpression += "[" + objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias + "]";
                            compareExpression += ".";
                            compareExpression += "[" + objectAttributeComparisonTerm.DataNode.Name + "]";
                            return compareExpression;
                        }
                    }
                case ComparisonTermsType.LiteralWithObject:
                case ComparisonTermsType.ObjectWithLiteral:
                    {

                        #region Build comparison expresion with ObjectIDComparisonTerm
                        MetaDataRepository.ObjectQueryLanguage.DataNode objectTermDataNode = null;
                        MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm = null;
                        if (criterion.FirstDataNode != null)
                        {
                            objectTermDataNode = criterion.FirstDataNode;
                            objectIDComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;
                        }
                        else
                        {
                            objectTermDataNode = criterion.SecondDataNode;
                            objectIDComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;
                        }

                        foreach (RDBMSMetaDataRepository.IdentityColumn column in (objectTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        {
                            if (compareExpression != null)
                            {
                                if (criterion.IsNULL)
                                    compareExpression += " OR ";
                                else if (criterion.IsNotNULL)
                                    compareExpression += " AND ";
                                else if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                    compareExpression += " AND ";
                                else
                                    compareExpression += " OR ";
                            }
                            else
                                compareExpression += "(";

                            compareExpression += " [" + objectTermDataNode.Alias + "].[" + column.Name + "]";


                            object literalFieldValue = null;
                            if (!criterion.IsNULL && !criterion.IsNotNULL)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, object> entry in objectIDComparisonTerm.MultiPartObjectID)
                                {
                                    if (entry.Key == column.ColumnType)
                                    {
                                        literalFieldValue = entry.Value;
                                        break;
                                    }
                                }
                                if (literalFieldValue is string)
                                    compareExpression += " " + comparisonOperator + "  '" + literalFieldValue + "'";
                                else if (literalFieldValue != null)
                                    compareExpression += " " + comparisonOperator + "  " + literalFieldValue + " ";
                                else if (literalFieldValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.Equal)
                                    compareExpression += " " + comparisonOperator + " NULL ";
                                else if (literalFieldValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual)
                                    compareExpression += " " + comparisonOperator + " NULL ";

                            }
                            else if (criterion.IsNULL)
                                compareExpression += " " + comparisonOperator + " NULL ";
                            else if (criterion.IsNotNULL)
                                compareExpression += " " + comparisonOperator + " NULL ";
                        }
                        #endregion
                        return compareExpression + ")";

                    }

                case ComparisonTermsType.ParameterWithObject:
                case ComparisonTermsType.ObjectWithParameter:
                    {
                        #region Build comparison expresion with ParameterComparisonTerm
                        MetaDataRepository.ObjectQueryLanguage.DataNode objectTermDataNode = null;

                        object parameterValue = null;
                        if (criterion.FirstDataNode != null)
                        {
                            objectTermDataNode = criterion.FirstDataNode;
                            parameterValue = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
                        }
                        else
                        {
                            objectTermDataNode = criterion.SecondDataNode;
                            parameterValue = (criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
                        }


                        ObjectID objectID = null;
                        StorageInstanceRef storageInstanceRef = null;

                        if (parameterValue != null)
                        {
                            if (!ModulePublisher.ClassRepository.GetType((objectTermDataNode.Classifier as MetaDataRepository.MetaObject).FullName, "").IsInstanceOfType(parameterValue))
                            {
                                //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
                                throw new System.Exception("Type mismatch at ");// + ComparisonTermParserNode.ParentNode.Value);
                            }
                            try
                            {
                                storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
                                if (storageInstanceRef == null)
                                    objectID = new ObjectID(System.Guid.NewGuid(), 0);
                            }
                            catch
                            {

                                objectID = new ObjectID(System.Guid.NewGuid(), 0);
                            }
                            if (storageInstanceRef != null)
                                objectID = storageInstanceRef.ObjectID as ObjectID;
                        }

                        foreach (RDBMSMetaDataRepository.IdentityColumn column in (objectTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        {
                            if (compareExpression != null)
                            {
                                if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                    compareExpression += " AND ";
                                else
                                    compareExpression += " OR ";
                            }
                            else
                                compareExpression += "(";
                            compareExpression += "[" + objectTermDataNode.Alias + "].[" + column.Name + "]";
                            if (objectID == null)
                                compareExpression += " IS NULL ";
                            else
                                compareExpression += " " + comparisonOperator + "  '" + objectID.GetMemberValue(column.ColumnType).ToString() + "'";
                        }
                        #endregion

                        return compareExpression + ")";
                    }
                case ComparisonTermsType.Objects:
                    {

                        #region Build comparison expresion with ObjectComparisonTerm
                        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = criterion.FirstDataNode;
                        MetaDataRepository.ObjectQueryLanguage.DataNode secondTermDataNode = criterion.SecondDataNode;
                        Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstTermColumns = (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                        Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondTermColumns = (secondTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                        foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstTermColumns)
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
                            {
                                if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                                {
                                    if (compareExpression != null)
                                    {
                                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                            compareExpression += " AND ";
                                        else
                                            compareExpression += " OR ";
                                    }
                                    else
                                        compareExpression += "(";
                                    compareExpression += "[" + firstTermDataNode.Alias + "].[" + CurrColumn.Name + "]";
                                    compareExpression += " " + comparisonOperator + "  [" + secondTermDataNode.Alias;
                                    compareExpression += "].[" + CorrespondingCurrColumn.Name + "]";
                                }
                            }
                        }
                        #endregion
                        return compareExpression + ")";
                    }
                case ComparisonTermsType.CollectionContainsAnyAll:
                    {
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
                            return "([" + DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + "].[ContainsAny] = 1 )";
                        else
                            return "([" + DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName + "].[ContainsAll] = 1 )";
                    }
            }
            return "";
        }

        private static string GetLikeString(string likeValue, string literalValue)
        {
            likeValue = likeValue.Replace("/", "//").Replace("%", "/%");
            int npos = 0;
            npos = likeValue.IndexOf("*");
            while (npos != -1)
            {
                if (likeValue.Length > npos + 1 && likeValue[npos + 1] == '*')
                    likeValue = likeValue.Remove(npos + 1, 1);
                else
                {
                    char[] chrs = likeValue.ToCharArray();
                    chrs[npos] = '%';
                    likeValue = new string(chrs);
                }
                npos = likeValue.IndexOf("*", npos + 1);
            }
            literalValue += "'" + likeValue + "' ESCAPE '/' ";
            return literalValue;
        }

        ///// <MetaDataID>{30601CB4-BED9-4501-BCF3-41054A0DF93A}</MetaDataID>
        //public string GetCompareExpression(MetaDataRepository.ObjectQueryLanguage.Criterion comparisonType, MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm firstComparisonTerm, MetaDataRepository.ObjectQueryLanguage.ComparisonTerm secondComparisonTerm, Criterion criterion)
        //{
        //    if (comparisonType == Criterion.ComparisonType.GreaterThan)
        //        return GetCompareExpression(Criterion.ComparisonType.LessThan, secondComparisonTerm as ObjectAttributeComparisonTerm, firstComparisonTerm, criterion);

        //    if (comparisonType == Criterion.ComparisonType.LessThan)
        //        return GetCompareExpression(Criterion.ComparisonType.GreaterThan, secondComparisonTerm as ObjectAttributeComparisonTerm, firstComparisonTerm, criterion);
        //    return GetCompareExpression(comparisonType, secondComparisonTerm as ObjectAttributeComparisonTerm, firstComparisonTerm, criterion);

        //}
        ///// <MetaDataID>{5EA5A031-D3F9-4797-94BD-CD3B47F76DA8}</MetaDataID>
        //public string GetCompareExpression(MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType comparisonType, MetaDataRepository.ObjectQueryLanguage.ObjectComparisonTerm firstComparisonTerm, MetaDataRepository.ObjectQueryLanguage.ComparisonTerm secondComparisonTerm)
        //{
        //    //TODO Υλοποίηση του like operator.


        //    string compareExpression = null;
        //    string comparisonOperator = null;
        //    if (comparisonType == MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType.Equal)
        //        comparisonOperator = "=";
        //    else
        //        comparisonOperator = "<>";



        //    if (secondComparisonTerm is ObjectComparisonTerm)
        //    {
        //        #region Build comparison expresion with ObjectComparisonTerm
        //        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = firstComparisonTerm.DataNode;
        //        MetaDataRepository.ObjectQueryLanguage.DataNode secondTermDataNode = (secondComparisonTerm as ObjectComparisonTerm).DataNode;
        //        MetaDataRepository.MetaObjectCollection FirstTermColumns = (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
        //        MetaDataRepository.MetaObjectCollection SecondTermColumns = (secondTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
        //        foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstTermColumns)
        //        {
        //            foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondTermColumns)
        //            {
        //                if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
        //                {
        //                    if (compareExpression != null)
        //                    {
        //                        if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //                            compareExpression += " AND ";
        //                        else
        //                            compareExpression += " OR ";
        //                    }
        //                    else
        //                        compareExpression += "(";
        //                    compareExpression +="["+ firstTermDataNode.Alias + "].[" + CurrColumn.Name +"]";
        //                    compareExpression += " " + comparisonOperator + "  [" + secondTermDataNode.Alias;
        //                    compareExpression += "].[" + CorrespondingCurrColumn.Name +"]";
        //                }
        //            }
        //        }
        //        #endregion
        //        return compareExpression + ")";
        //    }
        //    else if (secondComparisonTerm is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm)
        //    {
        //        #region Build comparison expresion with ObjectIDComparisonTerm
        //        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = firstComparisonTerm.DataNode;
        //        foreach (RDBMSMetaDataRepository.IdentityColumn column in (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
        //        {
        //            foreach(System.Collections.Generic.KeyValuePair<string,object> entry in (secondComparisonTerm as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm).MultiPartObjectID)

        //            {
        //                if (entry.Key == column.ColumnType)
        //                {
        //                    if (compareExpression != null)
        //                    {
        //                        if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //                            compareExpression += " AND ";
        //                        else
        //                            compareExpression += " OR ";
        //                    }
        //                    else
        //                        compareExpression += "(";

        //                    compareExpression +="["+ firstTermDataNode.Alias + "].[" + column.Name+"]";
        //                    if(entry.Value is string)
        //                        compareExpression += " " + comparisonOperator + "  '" + entry.Value+"'";
        //                    else
        //                        compareExpression += " " + comparisonOperator + "  " + entry.Value + "";
        //                }
        //            }
        //        }
        //        #endregion
        //        return compareExpression + ")";
        //    }
        //    else if (secondComparisonTerm is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
        //    {
        //        #region Build comparison expresion with ParameterComparisonTerm
        //        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = firstComparisonTerm.DataNode;
        //        ObjectID objectID = null;
        //        StorageInstanceRef storageInstanceRef = null;
        //        object parameterValue = (secondComparisonTerm as ParameterComparisonTerm).ParameterValue;
        //        if (parameterValue != null)
        //        {
        //            if (!ModulePublisher.ClassRepository.GetType((firstTermDataNode.Classifier as MetaDataRepository.MetaObject).FullName, "").IsInstanceOfType(parameterValue))
        //            {
        //                //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
        //                throw new System.Exception("Type mismatch at ");// + ComparisonTermParserNode.ParentNode.Value);
        //            }
        //            try
        //            {
        //                storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
        //                if (storageInstanceRef == null)
        //                    objectID = new ObjectID(System.Guid.NewGuid(), 0);
        //            }
        //            catch
        //            {

        //                objectID = new ObjectID(System.Guid.NewGuid(), 0);
        //            }
        //            if (storageInstanceRef != null)
        //                objectID = storageInstanceRef.ObjectID as ObjectID;
        //        }

        //        foreach (RDBMSMetaDataRepository.IdentityColumn column in (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
        //        {
        //            if (compareExpression != null)
        //            {
        //                if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //                    compareExpression += " AND ";
        //                else
        //                    compareExpression += " OR ";
        //            }
        //            else
        //                compareExpression += "(";
        //            compareExpression += "["+firstTermDataNode.Alias + "].[" + column.Name +"]";
        //            if (objectID == null)
        //                compareExpression += " IS NULL ";
        //            else
        //                compareExpression += " " + comparisonOperator + "  '" + objectID.GetMemberValue(column.ColumnType).ToString() + "'";
        //        }
        //        #endregion

        //        return compareExpression + ")";
        //    }
        //    else if (secondComparisonTerm is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm && (secondComparisonTerm as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm).Value == null)
        //    {
        //        #region Build comparison expresion with null
        //        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = firstComparisonTerm.DataNode;

        //        foreach (RDBMSMetaDataRepository.IdentityColumn column in (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
        //        {
        //            if (compareExpression != null)
        //            {
        //                if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //                    compareExpression += " AND ";
        //                else
        //                    compareExpression += " OR ";
        //            }
        //            else
        //                compareExpression += "(";
        //            compareExpression += "["+firstTermDataNode.Alias + "].[" + column.Name +"]";
        //            if (comparisonType == MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType.Equal)
        //                compareExpression += " IS ";
        //            else
        //                compareExpression += " IS NOT ";

        //        }
        //        #endregion

        //        return compareExpression + ")";
        //    }



        //    //else if (secondComparisonTerm is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm)
        //    //{
        //    //    #region Build comparison expresion with ParameterComparisonTerm
        //    //    MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = firstComparisonTerm.DataNode;
        //    //    ObjectID objectID = null;
        //    //    StorageInstanceRef storageInstanceRef = null;
        //    //    object literalValue =  (secondComparisonTerm as LiteralComparisonTerm).Value;


        //    //    foreach (RDBMSMetaDataRepository.IdentityColumn column in (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
        //    //    {
        //    //        if (compareExpression != null)
        //    //        {
        //    //            if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //    //                compareExpression += " AND ";
        //    //            else
        //    //                compareExpression += " OR ";
        //    //        }
        //    //        else
        //    //            compareExpression += "(";
        //    //        compareExpression += firstTermDataNode.Alias + "." + column.Name;
        //    //        if (literalValue == null)
        //    //            compareExpression += " IS NULL ";
        //    //        else
        //    //            compareExpression += " " + comparisonOperator + "  " + TypeDictionary.ConvertToSQLString(literalValue);
        //    //    }
        //    //    #endregion
        //    //    return compareExpression + ")";
        //    //}

        //    else
        //    {
        //        //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
        //        throw new System.Exception("Comparison error ");// + ComparisonTermParserNode.ParentNode.Value + " .");
        //    }

        //}


        ///// <MetaDataID>{5217A4D8-129F-4197-AFCF-9CFEE4E58BC9}</MetaDataID>
        //public string GetCompareExpression(MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType comparisonType, MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm firstComparisonTerm, MetaDataRepository.ObjectQueryLanguage.ComparisonTerm secondComparisonTerm,Criterion criterion)
        //{
        //    //TODO Υλοποίηση του like operator.

        //    string compareExpression = null;
        //    compareExpression += "[" + firstComparisonTerm.DataNode.ParentDataNode.Alias + "]";
        //    compareExpression += ".";
        //    compareExpression += "[" + firstComparisonTerm.DataNode.Name + "]";


        //    if (comparisonType == Criterion.ComparisonType.Equal&&criterion.IsNULL)
        //        compareExpression += " IS ";
        //    else if (comparisonType == Criterion.ComparisonType.Equal )
        //        compareExpression += " = ";


        //    if (comparisonType == Criterion.ComparisonType.NotEqual&&criterion.IsNotNULL)
        //        compareExpression += " IS NOT ";
        //    else if (comparisonType == Criterion.ComparisonType.NotEqual)
        //        compareExpression += " <> ";

        //    if (comparisonType == Criterion.ComparisonType.GreaterThan)
        //        compareExpression += " > ";
        //    if (comparisonType == Criterion.ComparisonType.LessThan)
        //        compareExpression += " < ";

        //    if (comparisonType == Criterion.ComparisonType.Like)
        //        compareExpression += " LIKE ";

        //    if (secondComparisonTerm is ObjectAttributeComparisonTerm)
        //    {
        //        compareExpression += "[" + (secondComparisonTerm as ObjectAttributeComparisonTerm).DataNode.ParentDataNode.Alias + "]";
        //        compareExpression += ".";
        //        compareExpression += "[" + (secondComparisonTerm as ObjectAttributeComparisonTerm).DataNode.Name + "]";
        //        return compareExpression;

        //    }
        //    else if (secondComparisonTerm is LiteralComparisonTerm)
        //    {
        //        if (comparisonType == Criterion.ComparisonType.Like)
        //        {
        //            string value = TypeDictionary.ConvertToSQLString((secondComparisonTerm as LiteralComparisonTerm).Value);
        //            value=value.Replace("%", "%%");
        //            int npos = 0;
        //            npos = value.IndexOf("*");
        //            while (npos != -1)
        //            {
        //                if (value.Length > npos + 1 && value[npos + 1] == '*')
        //                    value = value.Remove(npos + 1, 1);
        //                else
        //                {
        //                    char[] chrs = value.ToCharArray();
        //                    chrs[npos] = '%';
        //                    value = new string(chrs);
        //                }
        //                npos = value.IndexOf("*", npos + 1);
        //            }
        //            compareExpression += value + " ESCAPE '%' ";
        //        }
        //        else
        //            compareExpression += TypeDictionary.ConvertToSQLString((secondComparisonTerm as LiteralComparisonTerm).Value);
        //    }
        //    else if (secondComparisonTerm is ParameterComparisonTerm)
        //    {
        //        compareExpression += TypeDictionary.ConvertToSQLString((secondComparisonTerm as ParameterComparisonTerm).ParameterValue);
        //    }

        //    return compareExpression;


        //}


        ///// <MetaDataID>{63B38E13-08D9-44EB-AD90-B57E32E33D59}</MetaDataID>
        //public string GetCompareExpression(MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType comparisonType, MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm firstComparisonTerm, MetaDataRepository.ObjectQueryLanguage.ComparisonTerm secondComparisonTerm)
        //{
        //    //TODO Υλοποίηση του like operator.
        //    //TODO Test case for axception.
        //    //if (!(secondComparisonTerm is ObjectAttributeComparisonTerm || secondComparisonTerm is LiteralComparisonTerm || secondComparisonTerm is ParameterComparisonTerm))
        //    //    throw new System.Exception("Comparison error " + ComparisonTermParserNode.ParentNode.Value + " .");

        //    string compareExpression = null;// TypeDictionary.ConvertToSQLString(firstComparisonTerm.ParameterValue);
        //    string comparisonOperator = null;
        //    if (comparisonType == Criterion.ComparisonType.Equal)
        //        comparisonOperator = " = ";
        //    if (comparisonType == Criterion.ComparisonType.NotEqual)
        //        comparisonOperator = " <> ";
        //    if (comparisonType == Criterion.ComparisonType.GreaterThan)
        //        comparisonOperator = " > ";
        //    if (comparisonType == Criterion.ComparisonType.LessThan)
        //        comparisonOperator = " < ";
        //    if (comparisonType == Criterion.ComparisonType.Like)
        //        comparisonOperator += " LIKE ";

        //    if (secondComparisonTerm is ObjectAttributeComparisonTerm)
        //    {
        //        compareExpression = TypeDictionary.ConvertToSQLString(firstComparisonTerm.ParameterValue);
        //        compareExpression += comparisonOperator;
        //        compareExpression += "["+(secondComparisonTerm as ObjectAttributeComparisonTerm).DataNode.ParentDataNode.Alias;
        //        compareExpression += "].[";
        //        compareExpression += (secondComparisonTerm as ObjectAttributeComparisonTerm).DataNode.Name+"]";
        //        return compareExpression;

        //    }
        //    else if (secondComparisonTerm is ObjectComparisonTerm)
        //    {
        //        #region Build comparison expresion with ParameterComparisonTerm
        //        compareExpression = null;
        //        MetaDataRepository.ObjectQueryLanguage.DataNode seconfTermDataNode = (secondComparisonTerm as ObjectComparisonTerm).DataNode;
        //        ObjectID objectID = null;
        //        StorageInstanceRef storageInstanceRef = null;
        //        object parameterValue = (firstComparisonTerm as ParameterComparisonTerm).ParameterValue;
        //        if (parameterValue != null)
        //        {
        //            if (!ModulePublisher.ClassRepository.GetType((seconfTermDataNode.Classifier as MetaDataRepository.MetaObject).FullName, "").IsInstanceOfType(parameterValue))
        //            {
        //                //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
        //                throw new System.Exception("Type mismatch at ");// + ComparisonTermParserNode.ParentNode.Value);
        //            }
        //            try
        //            {
        //                storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
        //                if (storageInstanceRef == null)
        //                    objectID = new ObjectID(System.Guid.NewGuid(), 0);
        //            }
        //            catch
        //            {

        //                objectID = new ObjectID(System.Guid.NewGuid(), 0);
        //            }
        //            if (storageInstanceRef != null)
        //                objectID = storageInstanceRef.ObjectID as ObjectID;
        //        }

        //        foreach (RDBMSMetaDataRepository.IdentityColumn column in (seconfTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
        //        {
        //            if (compareExpression != null)
        //            {
        //                if (comparisonType == Criterion.ComparisonType.Equal) //error prone
        //                    compareExpression += " AND ";
        //                else
        //                    compareExpression += " OR ";
        //            }
        //            else
        //                compareExpression += "(";
        //            compareExpression += "[" + seconfTermDataNode.Alias + "].[" + column.Name + "]";
        //            if (objectID == null)
        //                compareExpression += " IS NULL ";
        //            else
        //                compareExpression += " "  + comparisonOperator+"  '" + objectID.GetMemberValue(column.ColumnType).ToString() + "'";
        //        }
        //        #endregion
        //        return compareExpression + ")";
        //    }


        //    return compareExpression;
        //}



        ///// <MetaDataID>{94F41F19-F85A-429B-BC3C-F3E2BF2E3708}</MetaDataID>
        //private string GetCriterionSQLExpression(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        //{
        //    //TODO Υλοποίηση του like operator.

        //    if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectComparisonTerm &&
        //        !(criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectComparisonTerm).DataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
        //    {
        //        return null;
        //    }

        //    if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm &&
        //        !(criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm).DataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
        //    {
        //        return null;
        //    }
        //    if (criterion.ComparisonTerms[0] is LiteralComparisonTerm)
        //    {
        //        return GetCompareExpression(criterion.ComparisonOperator,
        //             criterion.ComparisonTerms[0] as LiteralComparisonTerm,
        //             criterion.ComparisonTerms[1],
        //             criterion);

        //    }
        //    else if (criterion.ComparisonTerms[0] is ObjectComparisonTerm)
        //    {
        //        return GetCompareExpression(criterion.ComparisonOperator,
        //             criterion.ComparisonTerms[0] as ObjectComparisonTerm,
        //             criterion.ComparisonTerms[1]);

        //    }
        //    else if (criterion.ComparisonTerms[0] is ObjectAttributeComparisonTerm)
        //    {
        //        return GetCompareExpression(criterion.ComparisonOperator,
        //             criterion.ComparisonTerms[0] as ObjectAttributeComparisonTerm,
        //             criterion.ComparisonTerms[1],
        //             criterion);
        //    }
        //    else if (criterion.ComparisonTerms[0] is ParameterComparisonTerm)
        //    {
        //        return GetCompareExpression(criterion.ComparisonOperator,
        //             criterion.ComparisonTerms[0] as ParameterComparisonTerm,
        //             criterion.ComparisonTerms[1]);
        //    }
        //    else if (criterion.ComparisonTerms[0] is ObjectIDComparisonTerm)
        //    {
        //        return GetCompareExpression(criterion.ComparisonOperator,
        //             criterion.ComparisonTerms[1] as ObjectComparisonTerm,
        //             criterion.ComparisonTerms[0]);
        //    }

        //    throw new System.Exception("Syntax error : "+criterion.ParserNode.Value);

        //    /*
        //    if (criterion.ComparisonOperator== "=")
        //    {
        //        _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.Equal, _ComparisonTerms[1]);
        //        return _SQLExpression;
        //    }
        //    else if (Ccriterion.ComparisonOperator == "<>")
        //    {
        //        _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.NotEqual, _ComparisonTerms[1]);
        //        return _SQLExpression;
        //    }
        //    else if (criterion.ComparisonOperator == ">")
        //    {
        //        _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.GreaterThan, _ComparisonTerms[1]);
        //        return _SQLExpression;

        //    }
        //    else if (criterion.ComparisonOperator == "<")
        //    {
        //        _SQLExpression = _ComparisonTerms[0].GetCompareExpression(ComparisonType.LessThan, _ComparisonTerms[1]);
        //        return _SQLExpression;
        //    }
        //    return "";*/

        //}

        /// <MetaDataID>{BA7812A0-A136-42ED-8E06-1D388CAC28DA}</MetaDataID>
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, StorageDataSource.DataLoaderMetadata storageCells)
            : base(dataNode, storageCells)
        {

            string errors = null;
            if (dataNode.AssignedMetaObject == null)
            {

                string query = "#OQL SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject WHERE MetaObject.MetaObjectIDStream = \"" + dataNode.AssignedMetaObjectIdenty + "\" #";
                Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(LoadFromStorage).Execute(query);
                foreach (Collections.StructureSet Rowset in structureSet)
                {
                    dataNode.AssignedMetaObject = (MetaDataRepository.MetaObject)Rowset.Members["MetaObject"].Value;
                    break;
                }

            }


        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.List<string> _ChildRelationColumns;

        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with parent data source. 
        ///</summary>
        public override System.Collections.Generic.List<string> DataSourceRelationsColumnsWithParent
        {
            get
            {
                if (_ChildRelationColumns != null)
                    return _ChildRelationColumns;
                _ChildRelationColumns = new OOAdvantech.Collections.Generic.List<string>();

                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = associationEnd.GetReferenceColumns(Classifier);
                    if (referenceColumns.Count > 0 && associationEnd.Association.Specializations.Count == 0)
                    {
                        foreach (RDBMSMetaDataRepository.Column column in referenceColumns)
                            _ChildRelationColumns.Add(column.Name);
                    }
                    else
                    {
                        foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            _ChildRelationColumns.Add(column.Name);

                    }
                }
                else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        _ChildRelationColumns.Add(column.Name);
                }


                return _ChildRelationColumns;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>> _ParentRelationColumns;
        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with subDataNodes data source. 
        ///</summary>
        public override OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {
                if (_ParentRelationColumns != null)
                    return _ParentRelationColumns;
                _ParentRelationColumns = new OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>>();

                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.SubDataNodes)
                {
                    if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(dataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                        associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
                        Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = null;
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            referenceColumns = attribute.GetReferenceColumns(associationEnd);
                        }
                        else
                            referenceColumns = associationEnd.GetReferenceColumns(Classifier);

                        if (referenceColumns.Count > 0 && associationEnd.Association.Specializations.Count == 0)
                        {
                            System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                            int i = 0;
                            foreach (RDBMSMetaDataRepository.Column column in referenceColumns)
                                dataColumns.Add(column.Name);
                            _ParentRelationColumns[dataNode.Identity] = dataColumns;
                        }
                        else
                        {
                            System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                            int i = 0;
                            foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                                dataColumns.Add(column.Name);
                            _ParentRelationColumns[dataNode.Identity] = dataColumns;
                        }

                    }
                    else if (dataNode.AssignedMetaObject is MetaDataRepository.Attribute && (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        int i = 0;
                        foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                            dataColumns.Add(column.Name);
                        _ParentRelationColumns[dataNode.Identity] = dataColumns;

                    }
                }
                return _ParentRelationColumns;


            }
        }
        struct DataColumn
        {
            public DataColumn(string name, string alias)
            {
                Name = name;
                Alias = alias;
            }

            public readonly string Name;
            public readonly string Alias;
        }

        //struct RelatedDataNode
        //{
        //    public RelatedDataNode(bool contrainRelation, DataNode relatedDataNode)
        //    {
        //        ContrainRelation = contrainRelation;
        //        RelatedDataNode = relatedDataNode;
        //    }

        //    public readonly bool ContrainRelation;
        //    public readonly DataNode RelatedDataNode;
        //}

        public override System.Collections.ArrayList GetDataColoumns()
        {
            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            foreach (DataColumn column in GetSqlStatamentDataColoumns())
            {
                if (column.Alias != null)
                    columns.Add(column.Alias);
                else
                    columns.Add(column.Name);

            }
            return columns;

        }
        System.Collections.Generic.List<DataColumn> GetOrderByDataColoumns()
        {
            System.Collections.Generic.List<DataColumn> coloumns = new System.Collections.Generic.List<DataColumn>();
            {

                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    if (associationEnd.Indexer && associationEnd.GetReferenceColumns(Classifier).Count > 0)
                        coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null));
                }
            }
            return coloumns;
        }

        /// <MetaDataID>{0EF21F47-B73E-4D70-A960-191D61E8DF8A}</MetaDataID>
        System.Collections.Generic.List<DataColumn> GetSqlStatamentDataColoumns()
        {

            System.Collections.ArrayList columnNames = new System.Collections.ArrayList();

            System.Collections.Generic.List<DataColumn> coloumns = new System.Collections.Generic.List<DataColumn>();
            DataNode recursiveSubDataNode = null;
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Recursive)
                {
                    recursiveSubDataNode = subDataNode;
                    break;
                }
            }
            if (/*DataNode.ParticipateInWereClause ||*/ DataNode.ParticipateInSelectClause | (recursiveSubDataNode != null && (/*recursiveSubDataNode.BranchParticipateInWereClause |*/ recursiveSubDataNode.ParticipateInSelectClause)))
            {
                System.Collections.Generic.Dictionary<string, DataColumn> columnsWithAlias = new System.Collections.Generic.Dictionary<string, DataColumn>();
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                    {
                        if (!columnNames.Contains(column.Name))
                        {
                            coloumns.Add(new DataColumn(column.Name, null));
                            columnNames.Add(column.Name);
                        }
                    }

                    coloumns.Add(new DataColumn("StorageCellID", null));

                    RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                    foreach (MetaDataRepository.Attribute innerAttribute in attribute.Type.GetAttributes(true))
                    {
                        string columnName = attribute.GetAttributeColumnName(innerAttribute);
                        if (!columnNames.Contains(columnName))
                        {
                            coloumns.Add(new DataColumn(columnName, innerAttribute.Name));
                            columnNames.Add(columnName);
                        }
                    }
                    foreach (MetaDataRepository.AssociationEnd associationEnd in attribute.Type.GetRoles(true))
                    {
                        foreach (RDBMSMetaDataRepository.Column column in attribute.GetReferenceColumns(associationEnd as RDBMSMetaDataRepository.AssociationEnd))
                        {
                            if (!columnNames.Contains(column.Name))
                            {
                                coloumns.Add(new DataColumn(column.Name, null));
                                columnNames.Add(column.Name);
                            }
                        }
                    }
                }
                else
                {
                    foreach (RDBMSMetaDataRepository.View CurrSubView in View.SubViews)
                    {
                        if (coloumns == null)
                        {
                            foreach (string columnName in CurrSubView.ViewColumnsNames)
                            {
                                if (columnsWithAlias.ContainsKey(columnName))
                                    coloumns.Add(columnsWithAlias[columnName]);
                                else
                                {
                                    if (!columnNames.Contains(columnName))
                                    {
                                        coloumns.Add(new DataColumn(columnName, null));
                                        columnNames.Add(columnName);
                                    }

                                }
                            }
                        }
                        else if (coloumns.Count < CurrSubView.ViewColumnsNames.Count)
                        {
                            foreach (string columnName in CurrSubView.ViewColumnsNames)
                            {
                                if (columnsWithAlias.ContainsKey(columnName))
                                    coloumns.Add(columnsWithAlias[columnName]);
                                else
                                {
                                    if (!columnNames.Contains(columnName))
                                    {
                                        coloumns.Add(new DataColumn(columnName, null));
                                        columnNames.Add(columnName);
                                    }

                                }

                            }

                        }
                    }
                }
            }
            else
            {
                //coloumns = new System.Collections.ArrayList();
                bool IsThereSubDataNodeAsAssociation = false;
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.SubDataNodes)
                {
                    if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        IsThereSubDataNodeAsAssociation = true;
                        break;
                    }
                }
                if (IsThereSubDataNodeAsAssociation ||
                    DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd ||
                    (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType) ||
                    DataNode.ParticipateInWereClause)
                {
                    foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        coloumns.Add(new DataColumn(column.Name, null));

                }
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    foreach (RDBMSMetaDataRepository.Column column in associationEnd.GetReferenceColumns(Classifier))
                        coloumns.Add(new DataColumn(column.Name, null));

                    if (associationEnd.IndexerColumn != null)
                        coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null));

                }

                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.SubDataNodes)
                {
                    if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(dataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                        associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
                        Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = null;
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            referenceColumns = attribute.GetReferenceColumns(associationEnd);

                        }
                        else
                        {
                            referenceColumns = associationEnd.GetReferenceColumns(Classifier);
                            if (referenceColumns.Count > 0 && associationEnd.Indexer)
                                coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null));
                        }
                        foreach (RDBMSMetaDataRepository.Column column in referenceColumns)
                            coloumns.Add(new DataColumn(column.Name, null));


                    }
                }
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.SubDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        (dataNode.ParticipateInSelectClause || dataNode.ParticipateInWereClause))
                    {
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            string columnName = attribute.GetAttributeColumnName(dataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                            coloumns.Add(new DataColumn(columnName, (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name));

                        }
                        else
                            coloumns.Add(new DataColumn((dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name, null));
                    }
                }
            }

            //if (coloumns.Count == 0 && DataNode.CountSelect)
            //{
            //    foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
            //        coloumns.Add(new DataColumn(column.Name, null));
            //}

            return coloumns;
        }

        static string GetEmptyDataSQLStatament(MetaDataRepository.Classifier classifier, RDBMSPersistenceRunTime.Storage storage)
        {
            RDBMSMetaDataRepository.MappedClassifier mappedClassifier = (storage as Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Classifier)) as RDBMSMetaDataRepository.MappedClassifier;

            string sqlStatament = null;
            string whereClause = null;

            foreach (RDBMSMetaDataRepository.Column column in mappedClassifier.TypeView.ViewColumns)
            {

                string nullValue = "NULL";
                if (column.Type != null)
                    nullValue = OOAdvantech.RDBMSPersistenceRunTime.TypeDictionary.GetDBNullValue(column.Type.FullName);

                if (sqlStatament == null)
                {
                    whereClause = "WHERE " + column.Name + " <> " + nullValue;
                    sqlStatament += "(SELECT " + nullValue + " AS " + column.Name;
                }
                else
                    sqlStatament += "," + nullValue + " AS " + column.Name;


            }
            sqlStatament = "(SELECT * FROM " + sqlStatament + ") [TABLE] " + whereClause;
            sqlStatament += ")";
            return sqlStatament;
        }


        //string BuildSQLStatament( )
        //{
        //    BuildSQLStatament(false);
        //}

        /// <MetaDataID>{5ED2E587-D86D-49E8-A029-C2D806782EA8}</MetaDataID>
        string BuildSQLStatament()
        {
            string sqlStatament = null;
            string whereClause = null;

            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            columns.Add("StorageIdentity");
            foreach (DataColumn dataColumn in GetSqlStatamentDataColoumns())
                columns.Add(dataColumn.Name);
            if (View.SubViews.Count > 0)
            {


                foreach (RDBMSMetaDataRepository.View CurrSubView in View.SubViews)
                {
                    if (sqlStatament != null)
                        sqlStatament += "\nUNION ALL \n";
                    else
                        sqlStatament = "(";
                    RDBMSMetaDataRepository.StorageCell storageCell = null;
                    if (View.ViewStorageCell != null && View.ViewStorageCell.ContainsKey(CurrSubView))
                    {
                        storageCell = View.ViewStorageCell[CurrSubView];
                        //Temporary of.
                        //(this.DataNode.ObjectQuery as OQLStatement).StorageCells.Add(storageCell.GetHashCode(), storageCell);
                    }
                    sqlStatament += BuildSelectQuery(columns, CurrSubView, storageCell);
                }
            }
            else
            {

                foreach (RDBMSMetaDataRepository.Column column in columns)
                {
                    string nullValue = "NULL";
                    if (column.Type != null)
                        nullValue = OOAdvantech.RDBMSPersistenceRunTime.TypeDictionary.GetDBNullValue(column.Type.FullName);

                    if (sqlStatament == null)
                    {
                        whereClause = "WHERE " + column.Name + " <> " + nullValue;
                        sqlStatament += "(SELECT " + nullValue + " AS " + column.Name;
                    }
                    else
                        sqlStatament += "," + nullValue + " AS " + column.Name;
                }
                sqlStatament = "(SELECT * FROM " + sqlStatament + ") [TABLE] " + whereClause;
            }
            sqlStatament += ")";

            return sqlStatament;
        }
        ///// <MetaDataID>{7ACDA7C8-C757-412F-A485-F87C17C3C039}</MetaDataID>
        //string _SQLStatament;

        ///// <MetaDataID>{7051A449-2ED2-423F-A69C-E8E7B0B906D0}</MetaDataID>
        //string SQLStatament
        //{
        //    get
        //    {

        //        if (_SQLStatament != null)
        //            return _SQLStatament;
        //        _SQLStatament = null;
        //        _SQLStatament =" FROM "+ BuildFromClauseSQLQuery()+" ";

        //        //_SQLStatament += " as " + DataNode.Alias + " ";
        //        System.Collections.Generic.List<SearchCondition> filters = GetFilters(DataNode);
        //        string whereClause = null;
        //        foreach (SearchCondition filter in filters)
        //        {
        //            if (whereClause == null)
        //            {
        //                whereClause = " WHERE ";
        //            }
        //            else
        //                whereClause += " AND ";

        //            whereClause += GetSQLFilterStatament(filter);
        //        }
        //        _SQLStatament += whereClause;
        //        string selectClause = null;
        //        foreach (string columnName in GetDataColoumns())
        //        {

        //            if (selectClause == null)
        //                selectClause = "SELECT ";
        //            else
        //                selectClause += ",";
        //            selectClause += DataNode.Alias + "." + columnName;
        //        }

        //        _SQLStatament = selectClause + " " + _SQLStatament;

        //        return _SQLStatament;
        //    }

        //}
        ///// <MetaDataID>{CD49CBAB-2D75-4432-ACDD-37F649B2DBA4}</MetaDataID>
        //private void AppendSelectItem()
        //{
        //}
        /// <MetaDataID>{99B1878E-A42C-4663-8738-7084BE6D54F8}</MetaDataID>
        private string BuildSelectQuery(System.Collections.ArrayList ColumnsNames, RDBMSMetaDataRepository.View view, RDBMSMetaDataRepository.StorageCell storageCell)
        {
            string SelectClause = null;
            System.Collections.ArrayList ColumnsWithNull = new System.Collections.ArrayList();

            //			When data node is type object and participates in select clause of query then 
            //			the procedure of construction UNION TABLES must be allocate columns for objects 
            //			with maximum number of columns.
            //			For object that belong to the class in class hierarchy with less columns, 
            //			we will add extra columns which missed with null values. 
            //			This happen because in the union table statement all select must be 
            //			contain the same number and same type of columns
            ColumnsWithNull.AddRange(ColumnsNames);

            foreach (string columnName in view.ViewColumnsNames)
            {
                if (ColumnsWithNull.Contains(columnName))
                    ColumnsWithNull.Remove(columnName);

            }
            /*************************************/

            foreach (string columnName in ColumnsNames)
            {
                if (SelectClause == null)
                {
                    if (ColumnsWithNull.Contains(columnName) && columnName != "StorageIdentity")
                        SelectClause = "SELECT null as [" + columnName + "] ";
                    else
                        SelectClause = "SELECT null as [" + columnName + "]";
                }
                else
                {
                    if (ColumnsWithNull.Contains(columnName) && columnName != "StorageIdentity")
                        SelectClause += ", null as [" + columnName + "]";
                    else
                        SelectClause += ", null as [" + columnName + "]";
                }
            }

            string StorageName = null;
            // string ViewAlias = null;
            //PersistenceLayer.Storage ViewObjectStorage = view.Namespace as PersistenceLayer.Storage;
            //string ViewStorageName = ViewObjectStorage.StorageName;
            //string ViewStorageLocation = (ViewObjectStorage as MetaDataRepository.Storage).GetPropertyValue(typeof(string), "StorageMetadata", "MSSQLInstancePath") as string;
            //string ViewStorageLocation=ViewObjectStorage .StorageLocation;
            string ViewName = view.Name;

            //if (DataNode != null && ViewObjectStorage != DataNode.ObjectQuery.ObjectStorage.StorageMetaData)
            //{

            //    if (ViewStorageLocation == DataNode.ObjectQuery.ObjectStorage.StorageMetaData.StorageLocation)
            //        StorageName = ViewStorageName + ".dbo." + ViewName;
            //    else
            //        StorageName = @"OPENROWSET('SQLNCLI', 'Server=" + ViewStorageLocation + ";Trusted_Connection=yes;','SELECT * FROM " + ViewStorageName + ".dbo." + ViewName + "')";
            //}
            //          ViewAlias = ViewStorageLocation.Replace(@"\", "") + "_" + ViewStorageName + "_" + ViewName;
            string FromClause = null;
            if (storageCell != null)
                FromClause = "\nFROM ( SELECT " + DataLoaderMetadata.QueryStorageID + " as StorageIdentity ,* FROM " + StorageName + ViewName + ") " + StorageName + ViewName;// + ViewAlias;
            else
                FromClause = "\nFROM ( SELECT  NULL as StorageIdentity ,* FROM " + /*StorageName +*/ViewName + ") " + ViewName;// +ViewAlias;


            string WhereClause = null;





            return SelectClause + FromClause + WhereClause;
        }

        string BuildRecursiveLoad(string loadDataSQLQuery)
        {
            if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
            {
                return BuildRecursiveLoadManyToMany(loadDataSQLQuery, RelatedDataNodes);
            }
            else
            {
                return BuildRecursiveLoadOneToMany(loadDataSQLQuery, RelatedDataNodes);
            }

        }
        string BuildRecursiveLoadManyToMany(string loadDataSQLQuery, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {

            RDBMSMetaDataRepository.AssociationEnd associationEnd = (LoadFromStorage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in this.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                    ObjectsLinks.AddRange(((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell));
            }
            //ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = BuildObjectLinksSQLStatament(associationEnd.Association, ObjectsLinks);
            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);

            #endregion




            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;

            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
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

                        InnerJoinAttributes += DataNode.Alias + ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + AliasAssociationTableName + "]";
                        InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }




            string objectIDColumnsString = null;
            string refernceColumnsString = null;
            string childObjectIDColumnsString = null;
            string childRefernceColumnsString = null;
            string parentObjectIDColumnsString = null;
            string parentRefernceColumnsString = null;

            string dataColumnsString = null;
            string parentDataColumnsString = null;
            string childDataColumnsString = null;
            string innerJoinColumnsString = null;

            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            columns.Add("StorageIdentity");
            foreach (DataColumn dataColumn in GetSqlStatamentDataColoumns())
                columns.Add(dataColumn.Name);



            MetaDataRepository.Classifier classifier = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
            //foreach (RDBMSMetaDataRepository.AssociationEnd associationEnd in classifier.GetRoles(true))

            //if (associationEnd.Identity == DataNode.AssignedMetaObject.Identity)

            //System.Collections.Generic.List<string> recursiveRelationColumns = new System.Collections.Generic.List<string>();
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referencColumns = associationEnd.GetReferenceColumns();
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> objectIDColumns = (classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
            foreach (RDBMSMetaDataRepository.Column column in objectIDColumns)
            {
                if (objectIDColumnsString != null)
                    objectIDColumnsString += ",";
                objectIDColumnsString += column.Name;
                columns.Remove(column.Name);

                if (parentObjectIDColumnsString != null)
                    parentObjectIDColumnsString += ",";
                parentObjectIDColumnsString += "[" + DataNode.Alias + "].[" + column.Name + "]";


                if (childObjectIDColumnsString != null)
                    childObjectIDColumnsString += ",";
                childObjectIDColumnsString += "child." + column.Name;
            }

            foreach (RDBMSMetaDataRepository.Column column in referencColumns)
            {
                if (refernceColumnsString != null)
                    refernceColumnsString += ",";
                refernceColumnsString += column.Name;


                if (parentRefernceColumnsString != null)
                    parentRefernceColumnsString += ",";
                parentRefernceColumnsString += "[" + DataNode.Alias + "].[" + column.Name + "]";

                columns.Remove(column.Name);
                if (childRefernceColumnsString != null)
                    childRefernceColumnsString += ",";
                childRefernceColumnsString += "child." + column.Name;
            }
            foreach (RDBMSMetaDataRepository.IdentityColumn column in objectIDColumns)
            {
                if (innerJoinColumnsString != null)
                    innerJoinColumnsString += " AND ";

                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referencColumns)
                {
                    if (referenceColumn.ColumnType == column.ColumnType)
                    {
                        innerJoinColumnsString += "parent." + column.Name + " = child." + referenceColumn.Name;
                        break;
                    }
                }
            }
            foreach (string dataColumnName in columns)
            {
                if (dataColumnsString != null)
                    dataColumnsString += ",";
                dataColumnsString += "[" + dataColumnName + "]";


                if (parentDataColumnsString != null)
                    parentDataColumnsString += ",";
                parentDataColumnsString += "[" + DataNode.Alias + "].[" + dataColumnName + "]";




                if (childDataColumnsString != null)
                    childDataColumnsString += ",";
                childDataColumnsString += "child." + dataColumnName;





            }


            string RecursiveDataSource = "select " + parentDataColumnsString + "," + objectIDColumnsString + "," + refernceColumnsString + " from (" + loadDataSQLQuery + ") " + DataNode.Alias + " LEFT OUTER JOIN " + AssociationTableStatement + " " + AliasAssociationTableName + " ON " + InnerJoinAttributes;

            string RecursiveDataSourceB = "select  " + parentDataColumnsString + "," + objectIDColumnsString + "," + refernceColumnsString + "  FROM  " + BuildSQLStatament() + "  " + DataNode.Alias + "  INNER JOIN " + AssociationTableStatement + " " + AliasAssociationTableName + " ON " + InnerJoinAttributes;
            //break;




            //WHERE     ([StartPointObjectIDFilter])


            string recursiveSQLQuery = @"WITH TemporaryTable([RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]) 
                AS (
                SELECT     [ParentRefernceColumns], [ParentObjectIDColumns], 0 AS iteration,[ParentDataColumns]
                FROM         [RecursiveDataSource] 
                UNION ALL
                SELECT     [ChildRefernceColumns], [ChildObjectIDColumns], parent.iteration + 1 AS iteration,[ChildDataColumns]
                FROM       TemporaryTable AS parent INNER JOIN
                            [RecursiveDataSourceB] AS child ON [InnerJoinColumns]
                WHERE     (parent.iteration < [RecursiveStep]))

                SELECT     [RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]
                FROM         TemporaryTable";


            string whereClause = null;
            //System.Collections.Generic.List<SearchCondition> filters = GetFilters(DataNode);
            //foreach (SearchCondition filter in filters)
            //{
            //    string filterStatement = GetSQLFilterStatament(filter);
            //    if (filterStatement == null)
            //        continue;
            //    if (whereClause == null)
            //    {
            //        whereClause = " WHERE ";
            //    }
            //    else
            //        whereClause += " AND ";

            //    whereClause += filterStatement;
            //}



            recursiveSQLQuery = recursiveSQLQuery.Replace("[ObjectIDColumns]", objectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RefernceColumns]", refernceColumnsString);

            string temp = (this.DataNode.ParentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery((this.DataNode.ParentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader), relatedDataNodes);
            temp = "SELECT " + refernceColumnsString + "," + objectIDColumnsString + "," + dataColumnsString + "\n" + temp;

            //System.Collections.Generic.List<SearchCondition> filters = GetFilters(this.DataNode.ParentDataNode);

            //foreach (SearchCondition filter in filters)
            //{
            //    string filterStatement = GetSQLFilterStatament(filter);
            //    if (filterStatement == null)
            //        continue;
            //    if (whereClause == null)
            //    {
            //        whereClause = " WHERE ";
            //    }
            //    else
            //        whereClause += " AND ";

            //    whereClause += filterStatement;
            //}

            if (SearchCondition != null)
                whereClause = GetSQLFilterStatament(SearchCondition);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " WHERE " + whereClause;

            temp += "\n" + whereClause;



            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveDataSource]", "(" + RecursiveDataSource + ") as [" + DataNode.Alias + "] ");
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveDataSourceB]", "(" + RecursiveDataSourceB + ")");
            //recursiveSQLQuery = recursiveSQLQuery.Replace("[StartPointObjectIDFilter]", "SubCategory_ObjectIDB ='557D0F5B-A562-47F5-AB81-9F9DB6E70AC6'");
            recursiveSQLQuery = recursiveSQLQuery.Replace("[DataColumns]", dataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentDataColumns]", parentDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentObjectIDColumns]", parentObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentRefernceColumns]", parentRefernceColumnsString);


            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildDataColumns]", childDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildObjectIDColumns]", childObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildRefernceColumns]", childRefernceColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[InnerJoinColumns]", innerJoinColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveStep]", DataNode.RecursiveSteps.ToString());
            return recursiveSQLQuery;


        }


        string BuildRecursiveLoadOneToMany(string loadDataSQLQuery, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {


            string objectIDColumnsString = null;
            string refernceColumnsString = null;
            string childObjectIDColumnsString = null;
            string childRefernceColumnsString = null;
            string parentObjectIDColumnsString = null;
            string parentRefernceColumnsString = null;

            string dataColumnsString = null;
            string parentDataColumnsString = null;
            string childDataColumnsString = null;
            string innerJoinColumnsString = null;

            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            columns.Add("StorageIdentity");
            foreach (DataColumn dataColumn in GetSqlStatamentDataColoumns())
                columns.Add(dataColumn.Name);



            MetaDataRepository.Classifier classifier = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
            foreach (RDBMSMetaDataRepository.AssociationEnd associationEnd in classifier.GetRoles(true))
            {
                if (associationEnd.Identity == DataNode.AssignedMetaObject.Identity)
                {
                    //System.Collections.Generic.List<string> recursiveRelationColumns = new System.Collections.Generic.List<string>();
                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referencColumns = associationEnd.GetReferenceColumns(classifier);
                    Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> objectIDColumns = (classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    if (referencColumns.Count == 0)
                    {
                        referencColumns = objectIDColumns;
                        objectIDColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
                    }
                    foreach (RDBMSMetaDataRepository.Column column in objectIDColumns)
                    {
                        if (objectIDColumnsString != null)
                            objectIDColumnsString += ",";
                        objectIDColumnsString += column.Name;
                        columns.Remove(column.Name);

                        if (parentObjectIDColumnsString != null)
                            parentObjectIDColumnsString += ",";
                        parentObjectIDColumnsString += "[" + DataNode.Alias + "].[" + column.Name + "]";


                        if (childObjectIDColumnsString != null)
                            childObjectIDColumnsString += ",";
                        childObjectIDColumnsString += "child." + column.Name;
                    }

                    foreach (RDBMSMetaDataRepository.Column column in referencColumns)
                    {
                        if (refernceColumnsString != null)
                            refernceColumnsString += ",";
                        refernceColumnsString += column.Name;


                        if (parentRefernceColumnsString != null)
                            parentRefernceColumnsString += ",";
                        parentRefernceColumnsString += "[" + DataNode.Alias + "].[" + column.Name + "]";

                        columns.Remove(column.Name);
                        if (childRefernceColumnsString != null)
                            childRefernceColumnsString += ",";
                        childRefernceColumnsString += "child." + column.Name;
                    }
                    foreach (RDBMSMetaDataRepository.IdentityColumn column in objectIDColumns)
                    {
                        if (innerJoinColumnsString != null)
                            innerJoinColumnsString += " AND ";

                        foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referencColumns)
                        {
                            if (referenceColumn.ColumnType == column.ColumnType)
                            {
                                innerJoinColumnsString += "parent." + column.Name + " = child." + referenceColumn.Name;
                                break;
                            }
                        }
                    }
                    foreach (string dataColumnName in columns)
                    {
                        if (dataColumnsString != null)
                            dataColumnsString += ",";
                        dataColumnsString += "[" + dataColumnName + "]";


                        if (parentDataColumnsString != null)
                            parentDataColumnsString += ",";
                        parentDataColumnsString += "[" + DataNode.Alias + "].[" + dataColumnName + "]";




                        if (childDataColumnsString != null)
                            childDataColumnsString += ",";
                        childDataColumnsString += "child." + dataColumnName;





                    }
                    break;

                }

            }
            //WHERE     ([StartPointObjectIDFilter])


            string recursiveSQLQuery = @"WITH TemporaryTable([RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]) 
                AS (
                SELECT     [ParentRefernceColumns], [ParentObjectIDColumns], 0 AS iteration,[ParentDataColumns]
                FROM         [RecursiveDataSource] 
                UNION ALL
                SELECT     [ChildRefernceColumns], [ChildObjectIDColumns], parent.iteration + 1 AS iteration,[ChildDataColumns]
                FROM       TemporaryTable AS parent INNER JOIN
                            [RecursiveDataSourceB] AS child ON [InnerJoinColumns]
                WHERE     (parent.iteration < [RecursiveStep]))

                SELECT     [RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]
                FROM         TemporaryTable";


            string whereClause = null;
            //System.Collections.Generic.List<SearchCondition> filters = GetFilters(DataNode);
            //foreach (SearchCondition filter in filters)
            //{
            //    string filterStatement = GetSQLFilterStatament(filter);
            //    if (filterStatement == null)
            //        continue;
            //    if (whereClause == null)
            //    {
            //        whereClause = " WHERE ";
            //    }
            //    else
            //        whereClause += " AND ";

            //    whereClause += filterStatement;
            //}



            recursiveSQLQuery = recursiveSQLQuery.Replace("[ObjectIDColumns]", objectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RefernceColumns]", refernceColumnsString);

            string temp = (this.DataNode.ParentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery((this.DataNode.ParentDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader), relatedDataNodes);
            temp = "SELECT " + refernceColumnsString + "," + objectIDColumnsString + "," + dataColumnsString + "\n" + temp;

            //System.Collections.Generic.List<SearchCondition> filters = GetFilters(this.DataNode.ParentDataNode);

            //foreach (SearchCondition filter in filters)
            //{
            //    string filterStatement = GetSQLFilterStatament(filter);
            //    if (filterStatement == null)
            //        continue;
            //    if (whereClause == null)
            //    {
            //        whereClause = " WHERE ";
            //    }
            //    else
            //        whereClause += " AND ";

            //    whereClause += filterStatement;
            //}

            if (SearchCondition != null)
                whereClause = GetSQLFilterStatament(SearchCondition);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " WHERE " + whereClause;

            temp += "\n" + whereClause;



            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveDataSource]", "(" + loadDataSQLQuery + ") as [" + DataNode.Alias + "] ");
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveDataSourceB]", BuildSQLStatament());
            //recursiveSQLQuery = recursiveSQLQuery.Replace("[StartPointObjectIDFilter]", "SubCategory_ObjectIDB ='557D0F5B-A562-47F5-AB81-9F9DB6E70AC6'");
            recursiveSQLQuery = recursiveSQLQuery.Replace("[DataColumns]", dataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentDataColumns]", parentDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentObjectIDColumns]", parentObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ParentRefernceColumns]", parentRefernceColumnsString);


            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildDataColumns]", childDataColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildObjectIDColumns]", childObjectIDColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildRefernceColumns]", childRefernceColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[InnerJoinColumns]", innerJoinColumnsString);
            recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveStep]", DataNode.RecursiveSteps.ToString());
            return recursiveSQLQuery;

        }

        //string BuildFromClauseSQLQuery()
        //{
        //    return BuildFromClauseSQLQuery(this);
        //}

        string BuildFromClauseSQLQuery(DataLoader headerDataloader, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes)
        {
            string fromClauseSQLQuery = null;
            if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {
                if ((DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type is MetaDataRepository.Structure &&
                    ((DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type as MetaDataRepository.Structure).Persistent)
                {
                    if (headerDataloader == this)//!typeof(MetaDataRepository.AssociationEnd).IsInstanceOfType(DataNode.AssignedMetaObject))
                        fromClauseSQLQuery = BuildSQLStatament() + " AS [" + DataNode.Alias + "]";
                    foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                    {
                        if (!relatedDataNodes.ContainsKey(subDataNode))
                            continue;
                        bool innerJoin = relatedDataNodes[subDataNode];
                        relatedDataNodes.Remove(subDataNode);
                        #region Build table join with sub data node if needed.

                        // if (subDataNode.ActOnConstrainCriterion)
                        {

                            if (typeof(MetaDataRepository.AssociationEnd).IsInstanceOfType(subDataNode.AssignedMetaObject))
                            {
                                //RDBMSMetaDataRepository.AssociationEnd associationEnd = (RDBMSMetaDataRepository.AssociationEnd)Storage.GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                                MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

                                if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                                {
                                    if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||	//is type of Class.AssociationClass
                                        (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))									//is type of AssociationClass.Class
                                        fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                    else
                                        fromClauseSQLQuery += BuildManyToManyTablesJoin(subDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                                }
                                else
                                {
                                    if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||//is type of AssociationClass.Class
                                        (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))								//is type of Class.AssociationClass
                                        fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                    else
                                        fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                }
                            }
                        }
                        #endregion
                    }

                    if (relatedDataNodes.ContainsKey(DataNode.ParentDataNode))
                    {
                        bool innerJoin = relatedDataNodes[DataNode.ParentDataNode];
                        relatedDataNodes.Remove(DataNode.ParentDataNode);

                        #region Build table join with parent data node if needed.
                        if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                        {
                            //RDBMSMetaDataRepository.AssociationEnd associationEnd = (RDBMSMetaDataRepository.AssociationEnd)Storage.GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                            if (associationEnd.Association.Specializations.Count > 0)
                            {
                                fromClauseSQLQuery += BuildAbstractAssociationTablesJoin(DataNode.ParentDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                            }
                            else
                            {
                                if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                                {
                                    MetaDataRepository.Attribute roleAAttribute = null;
                                    MetaDataRepository.Attribute roleBAttribute = null;
                                    if (associationEnd.Association.LinkClass != null)
                                    {
                                        foreach (MetaDataRepository.Attribute attribute in associationEnd.Association.LinkClass.GetAttributes(true))
                                        {
                                            if (attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole") != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole"))
                                            {
                                                if ((bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA"))
                                                    roleAAttribute = attribute;
                                                else
                                                    roleBAttribute = attribute;
                                            }
                                        }
                                    }
                                    if ((associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && associationEnd.Association.Name == DataNode.Name) ||	//is type of Class.AssociationClass
                                        (associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && (associationEnd.Name == DataNode.Name || roleAAttribute.Name == DataNode.Name || roleBAttribute.Name == DataNode.Name)))									//is type of AssociationClass.Class
                                        fromClauseSQLQuery += BuildAssociationClassTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                    else
                                        fromClauseSQLQuery += BuildManyToManyTablesJoin(DataNode.ParentDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                                }
                                else
                                {
                                    if ((associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && associationEnd.Association.Name == DataNode.Name) ||//is type of AssociationClass.Class
                                        (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == DataNode.Name))								//is type of Class.AssociationClass
                                        fromClauseSQLQuery += BuildAssociationClassTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                    else
                                        fromClauseSQLQuery += BuildOneToManyTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                }
                            }
                        }
                        else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            fromClauseSQLQuery += BuildOneToManyTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                        }
                        #endregion
                    }
                    return fromClauseSQLQuery;
                }
                else
                    return null;
            }
            else if (Classifier == null && (typeof(MetaDataRepository.Namespace).IsInstanceOfType(DataNode.AssignedMetaObject)))
            {
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (!relatedDataNodes.ContainsKey(subDataNode))
                        continue;
                    bool innerJoin = relatedDataNodes[subDataNode];
                    relatedDataNodes.Remove(subDataNode);
                    if (subDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                        fromClauseSQLQuery += (subDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);
                }
            }
            else
            {
                if (headerDataloader == this)//!typeof(MetaDataRepository.AssociationEnd).IsInstanceOfType(DataNode.AssignedMetaObject))
                    fromClauseSQLQuery = BuildSQLStatament() + " AS [" + DataNode.Alias + "]";
                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (!relatedDataNodes.ContainsKey(subDataNode))
                        continue;
                    bool innerJoin = relatedDataNodes[subDataNode];
                    relatedDataNodes.Remove(subDataNode);
                    #region Build table join with sub data node if needed.
                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        //RDBMSMetaDataRepository.AssociationEnd associationEnd = (RDBMSMetaDataRepository.AssociationEnd)Storage.GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                        MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        if (associationEnd.Association.Specializations.Count > 0)
                        {
                            fromClauseSQLQuery += BuildAbstractAssociationTablesJoin(subDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                        }
                        else
                        {
                            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                            {
                                DotNetMetaDataRepository.Attribute roleAAttribute = null;
                                DotNetMetaDataRepository.Attribute roleBAttribute = null;
                                if (associationEnd.Association.LinkClass != null)
                                {
                                    foreach (DotNetMetaDataRepository.Attribute attribute in associationEnd.Association.LinkClass.GetAttributes(true))
                                    {
                                        if (attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole") != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole"))
                                        {
                                            if ((bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA"))
                                                roleAAttribute = attribute;
                                            else
                                                roleBAttribute = attribute;
                                        }
                                    }


                                }
                                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||	//is type of Class.AssociationClass
                                    (associationEnd.Association.LinkClass == DataNode.Classifier && (associationEnd.Name == subDataNode.Name || roleAAttribute.Name == subDataNode.Name || roleBAttribute.Name == subDataNode.Name)))									//is type of AssociationClass.Class
                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                else
                                    fromClauseSQLQuery += BuildManyToManyTablesJoin(subDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                            }
                            else
                            {
                                if ((associationEnd.Association.LinkClass == subDataNode.Classifier && associationEnd.Association.Name == subDataNode.Name) ||//is type of AssociationClass.Class
                                    (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == subDataNode.Name))								//is type of Class.AssociationClass
                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                else
                                    fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, headerDataloader, relatedDataNodes, innerJoin);
                            }
                        }
                    }
                    else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        fromClauseSQLQuery += BuildOneToManyTablesJoin(subDataNode, headerDataloader, relatedDataNodes, true);
                    }
                    #endregion
                }
                if (relatedDataNodes.ContainsKey(DataNode.ParentDataNode))
                {
                    bool innerJoin = relatedDataNodes[DataNode.ParentDataNode];
                    relatedDataNodes.Remove(DataNode.ParentDataNode);

                    #region Build table join with parent data node if needed.
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        //RDBMSMetaDataRepository.AssociationEnd associationEnd = (RDBMSMetaDataRepository.AssociationEnd)Storage.GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                        MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        if (associationEnd.Association.Specializations.Count > 0)
                        {
                            fromClauseSQLQuery += BuildAbstractAssociationTablesJoin(DataNode.ParentDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                        }
                        else
                        {
                            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                            {
                                MetaDataRepository.Attribute roleAAttribute = null;
                                MetaDataRepository.Attribute roleBAttribute = null;
                                if (associationEnd.Association.LinkClass != null)
                                {
                                    foreach (MetaDataRepository.Attribute attribute in associationEnd.Association.LinkClass.GetAttributes(true))
                                    {
                                        if (attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole") != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole"))
                                        {
                                            if ((bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA"))
                                                roleAAttribute = attribute;
                                            else
                                                roleBAttribute = attribute;
                                        }
                                    }
                                }
                                if ((associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && associationEnd.Association.Name == DataNode.Name) ||	//is type of Class.AssociationClass
                                    (associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && (associationEnd.Name == DataNode.Name || roleAAttribute.Name == DataNode.Name || roleBAttribute.Name == DataNode.Name)))									//is type of AssociationClass.Class
                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                else
                                    fromClauseSQLQuery += BuildManyToManyTablesJoin(DataNode.ParentDataNode, false, headerDataloader, relatedDataNodes, innerJoin);
                            }
                            else
                            {
                                if ((associationEnd.Association.LinkClass == DataNode.ParentDataNode.Classifier && associationEnd.Association.Name == DataNode.Name) ||//is type of AssociationClass.Class
                                    (associationEnd.Association.LinkClass == DataNode.Classifier && associationEnd.Name == DataNode.Name))								//is type of Class.AssociationClass
                                    fromClauseSQLQuery += BuildAssociationClassTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                                else
                                    fromClauseSQLQuery += BuildOneToManyTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                            }
                        }
                    }
                    else if (DataNode.ParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.ParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        fromClauseSQLQuery += BuildOneToManyTablesJoin(DataNode.ParentDataNode, headerDataloader, relatedDataNodes, innerJoin);
                    }
                    #endregion
                }
            }

            return fromClauseSQLQuery;
        }




        private string BuildOneToManyCountedItems(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode,
                                                Dictionary<DataNode, bool> relatedDataNodes,
                                                out string joinWhereAttributes)
        {
            joinWhereAttributes = null;

            if ((relatedDataNode != DataNode.ParentDataNode && relatedDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) ||
                (relatedDataNode == DataNode.ParentDataNode && DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
            {


                RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

                if (relatedDataNode == DataNode.ParentDataNode)
                    associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                else
                    associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();

                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = null;
                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = null;

                if (associationEnd == associationEndWithReferenceColumns)
                {
                    //The reference columns are in sub data node table.
                    if (relatedDataNode == DataNode.ParentDataNode)
                    {
                        if (relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (relatedDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            SecondJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            SecondJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();
                    }
                    else
                    {
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                        (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            SecondJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            SecondJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();

                    }
                    FirstJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                }
                else
                {
                    //The reference columns are in data node table.
                    if (relatedDataNode == DataNode.ParentDataNode)
                    {
                        if (relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (relatedDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            FirstJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            FirstJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();
                    }
                    else
                    {
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                            (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            FirstJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            FirstJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();

                    }
                    SecondJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                }

                string FromClauseSQLQuery = null;


                // FromClauseSQLQuery = " INNER JOIN ";


                DataLoader subDataNodeDataLoader = null;
                if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

                if (subDataNodeDataLoader != null)
                {
                    //FromClauseSQLQuery += "(";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildFromClauseSQLQuery(this, relatedDataNodes);
                    //FromClauseSQLQuery += " ON ";
                }
                else
                    FromClauseSQLQuery += GetEmptyDataSQLStatament(relatedDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + relatedDataNode.Alias + "] ";//ON ";


                if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                    throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");


                foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
                {
                    foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                    {
                        if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                        {
                            if (joinWhereAttributes != null)
                                joinWhereAttributes += " AND ";

                            if (relatedDataNode == DataNode.ParentDataNode)
                            {
                                joinWhereAttributes += "[" + relatedDataNode.Alias + "].[" + CurrColumn.Name + "]";
                                joinWhereAttributes += " = [" + DataNode.Alias + "]";
                                joinWhereAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                            }
                            else
                            {
                                joinWhereAttributes += "[" + DataNode.Alias + "].[" + CurrColumn.Name + "]";
                                joinWhereAttributes += " = [" + relatedDataNode.Alias + "]";
                                joinWhereAttributes += ".[" + CorrespondingCurrColumn.Name + "]";

                            }
                        }
                    }
                }
                //FromClauseSQLQuery += joinWhereAttributes;

                return FromClauseSQLQuery;
            }
            if ((relatedDataNode != DataNode.ParentDataNode && relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute) ||
                (relatedDataNode == DataNode.ParentDataNode && DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
            {


                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = null;
                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = null;
                foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                {
                    FirstJoinTableColumns = (storageCell.Type as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    SecondJoinTableColumns = (storageCell.Type as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;

                }

                string FromClauseSQLQuery = null;


                FromClauseSQLQuery = " INNER JOIN ";


                DataLoader subDataNodeDataLoader = null;
                if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

                if (subDataNodeDataLoader != null)
                {
                    //FromClauseSQLQuery += "(";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildFromClauseSQLQuery(this, relatedDataNodes);
                    // FromClauseSQLQuery += " ON ";
                }
                else
                    FromClauseSQLQuery += GetEmptyDataSQLStatament(relatedDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + relatedDataNode.Alias + "] ";//ON ";


                if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                    throw new System.Exception("Incorrect mapping of " + relatedDataNode.AssignedMetaObject.FullName + " association");

                foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
                {
                    foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                    {
                        if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                        {
                            if (joinWhereAttributes != null)
                                joinWhereAttributes += " AND ";
                            joinWhereAttributes += "[" + DataNode.Alias + "].[" + CurrColumn.Name + "]";
                            joinWhereAttributes += " = [" + relatedDataNode.Alias + "]";
                            joinWhereAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                        }
                    }
                }
                //FromClauseSQLQuery += InnerJoinAttributes;

                return FromClauseSQLQuery;
            }
            throw new System.Exception("Unknown sub data node");

        }




        /// <summary>Build a join between the table from data source and data source table of sub data node. </summary>
        /// <MetaDataID>{7C44A9DF-B512-49BE-ABEF-D96BB0C8146F}</MetaDataID>
        private string BuildOneToManyTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode, DataLoader headerDataloader, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes, bool innerJoin)
        {

            if ((relatedDataNode != DataNode.ParentDataNode && relatedDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) ||
                (relatedDataNode == DataNode.ParentDataNode && DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
            {


                RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

                if (relatedDataNode == DataNode.ParentDataNode)
                    associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                else
                    associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();

                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = null;
                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = null;

                if (associationEnd == associationEndWithReferenceColumns)
                {
                    //The reference columns are in sub data node table.
                    if (relatedDataNode == DataNode.ParentDataNode)
                    {
                        if (relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (relatedDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            SecondJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            SecondJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();
                    }
                    else
                    {
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                        (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            SecondJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            SecondJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();

                    }
                    FirstJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                }
                else
                {
                    //The reference columns are in data node table.
                    if (relatedDataNode == DataNode.ParentDataNode)
                    {
                        if (relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (relatedDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            FirstJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            FirstJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();
                    }
                    else
                    {
                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                            (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            RDBMSMetaDataRepository.Attribute attribute = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                            FirstJoinTableColumns = attribute.GetReferenceColumns(associationEndWithReferenceColumns);
                        }
                        else
                            FirstJoinTableColumns = associationEndWithReferenceColumns.GetReferenceColumns();

                    }
                    SecondJoinTableColumns = (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                }

                string FromClauseSQLQuery = null;


                if (innerJoin)
                    FromClauseSQLQuery = " INNER JOIN ";
                else
                    FromClauseSQLQuery = " LEFT OUTER JOIN ";


                //if (subDataNode.ActOnConstrainCriterion)
                //{
                //    FromClauseSQLQuery = null;
                //    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in subDataNode.SearchCriteria)
                //    {
                //        if (criterion.IsNULL)
                //        {
                //            FromClauseSQLQuery = " LEFT OUTER JOIN ";
                //            break;
                //        }
                //    }
                //    if (FromClauseSQLQuery == null)
                //        FromClauseSQLQuery = " INNER JOIN ";
                //}
                //else
                //    FromClauseSQLQuery = " LEFT OUTER JOIN ";


                DataLoader subDataNodeDataLoader = null;
                if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

                if (subDataNodeDataLoader != null)
                {
                    //FromClauseSQLQuery += "(";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);
                    FromClauseSQLQuery += " ON ";
                }
                else
                    FromClauseSQLQuery += GetEmptyDataSQLStatament(relatedDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + relatedDataNode.Alias + "] ON ";


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

                            if (relatedDataNode == DataNode.ParentDataNode)
                            {
                                InnerJoinAttributes += "[" + relatedDataNode.Alias + "].[" + CurrColumn.Name + "]";
                                InnerJoinAttributes += " = [" + DataNode.Alias + "]";
                                InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                            }
                            else
                            {
                                InnerJoinAttributes += "[" + DataNode.Alias + "].[" + CurrColumn.Name + "]";
                                InnerJoinAttributes += " = [" + relatedDataNode.Alias + "]";
                                InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";

                            }
                        }
                    }
                }
                FromClauseSQLQuery += InnerJoinAttributes;

                return FromClauseSQLQuery;
            }
            if ((relatedDataNode != DataNode.ParentDataNode && relatedDataNode.AssignedMetaObject is MetaDataRepository.Attribute) ||
                (relatedDataNode == DataNode.ParentDataNode && DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
            {


                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = null;
                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = null;
                foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                {
                    FirstJoinTableColumns = (storageCell.Type as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    SecondJoinTableColumns = (storageCell.Type as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;

                }

                string FromClauseSQLQuery = null;


                if (innerJoin)
                    FromClauseSQLQuery = " INNER JOIN ";
                else
                    FromClauseSQLQuery = " LEFT OUTER JOIN ";


                DataLoader subDataNodeDataLoader = null;
                if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                    subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

                if (subDataNodeDataLoader != null)
                {
                    //FromClauseSQLQuery += "(";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";
                    FromClauseSQLQuery += subDataNodeDataLoader.BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);
                    FromClauseSQLQuery += " ON ";
                }
                else
                    FromClauseSQLQuery += GetEmptyDataSQLStatament(relatedDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + relatedDataNode.Alias + "] ON ";


                if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                    throw new System.Exception("Incorrect mapping of " + relatedDataNode.AssignedMetaObject.FullName + " association");
                string InnerJoinAttributes = null;
                foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
                {
                    foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                    {
                        if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                        {
                            if (InnerJoinAttributes != null)
                                InnerJoinAttributes += " AND ";
                            InnerJoinAttributes += "[" + DataNode.Alias + "].[" + CurrColumn.Name + "]";
                            InnerJoinAttributes += " = [" + relatedDataNode.Alias + "]";
                            InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                        }
                    }
                }
                FromClauseSQLQuery += InnerJoinAttributes;

                return FromClauseSQLQuery;
            }
            throw new System.Exception("Unknown sub data node");

        }

        /// <MetaDataID>{C22E771B-D3C6-43A0-96F5-AACE440D2E8C}</MetaDataID>
        private string BuildAssociationClassTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode, DataLoader headerDataloader, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes, bool innerJoin)
        {
            MetaDataRepository.ObjectQueryLanguage.DataNode rootDataNode = DataNode;




            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

            if (relatedDataNode == DataNode.ParentDataNode)
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
            else
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = null;

            MetaDataRepository.Attribute roleAAttribute = null;
            MetaDataRepository.Attribute roleBAttribute = null;
            if (associationEnd.Association.LinkClass != null)
            {
                foreach (MetaDataRepository.Attribute attribute in associationEnd.Association.LinkClass.GetAttributes(true))
                {
                    if (attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole") != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole"))
                    {
                        if ((bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA"))
                            roleAAttribute = attribute;
                        else
                            roleBAttribute = attribute;
                    }
                }
            }

            if (relatedDataNode != DataNode.ParentDataNode)
            {
                if (associationEnd.Name == relatedDataNode.Name || roleAAttribute.Name == relatedDataNode.Name || roleBAttribute.Name == relatedDataNode.Name)
                {

                    //is type of AssociationClass.Class
                    SecondJoinTableColumns = (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    FirstJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
                }
                else if (associationEnd.Association.Name == relatedDataNode.Name)
                {
                    //is type of Class.AssociationClass
                    FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    SecondJoinTableColumns = ((RDBMSMetaDataRepository.AssociationEnd)associationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
                }
                else
                    throw new System.Exception("Data tree Error");//Error Prone
            }
            else
            {
                if (associationEnd.Name == DataNode.Name || roleAAttribute.Name == DataNode.Name || roleBAttribute.Name == DataNode.Name)
                {

                    //is type of AssociationClass.Class
                    SecondJoinTableColumns = (DataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    FirstJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
                }
                else if (associationEnd.Association.Name == DataNode.Name)
                {
                    //is type of Class.AssociationClass
                    FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                    SecondJoinTableColumns = ((RDBMSMetaDataRepository.AssociationEnd)associationEnd).GetReferenceColumns(associationEnd.Association.LinkClass);
                }
                else
                    throw new System.Exception("Data tree Error");//Error Prone

            }



            string FromClauseSQLQuery = null;

            if (innerJoin)
                FromClauseSQLQuery = " INNER JOIN ";
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";

            //if (subDataNode.ActOnConstrainCriterion)
            //    FromClauseSQLQuery = " INNER JOIN ";
            //else if (associationEnd.Name == subDataNode.Name)
            //    FromClauseSQLQuery = " INNER JOIN ";//LEFT OUTER JOIN 
            //else
            //    FromClauseSQLQuery = " LEFT OUTER JOIN ";//LEFT OUTER JOIN 


            DataLoader relatedDataNodeDataLoader = null;
            if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                relatedDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;

            //if (subDataNodeDataLoader != null)
            //{
            //    FromClauseSQLQuery += "(";
            //    FromClauseSQLQuery += subDataNodeDataLoader.BuildSQLStatament() + " AS [" + subDataNode.Alias + "] ";
            //    FromClauseSQLQuery += subDataNodeDataLoader.BuildFromClauseSQLQuery();
            //    FromClauseSQLQuery += ") ON ";
            //}
            //else
            //    FromClauseSQLQuery += GetEmptyDataSQLStatament(subDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + subDataNode.Alias + "] ON ";



            //string subDataNodeFromClauseSQLQuery = (subDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(); ;
            if (relatedDataNodeDataLoader != null)
            {
                //  FromClauseSQLQuery += "( ";
                FromClauseSQLQuery += (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";
                FromClauseSQLQuery += relatedDataNodeDataLoader.BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);
                FromClauseSQLQuery += " ON ";
            }
            else
                FromClauseSQLQuery += GetEmptyDataSQLStatament(relatedDataNode.Classifier, LoadFromStorage as Storage) + " AS [" + relatedDataNode.Alias + "] ON ";


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
                        if (relatedDataNode == rootDataNode.ParentDataNode)
                        {
                            InnerJoinAttributes += "[" + relatedDataNode.Alias + "].[" + CurrColumn.Name + "]";//"Abstract_"+Class.Name+"."+CurrColumn.Name;
                            InnerJoinAttributes += " = [" + rootDataNode.Alias + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                            InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                        }
                        else
                        {
                            InnerJoinAttributes += "[" + rootDataNode.Alias + "].[" + CurrColumn.Name + "]";//"Abstract_"+Class.Name+"."+CurrColumn.Name;
                            InnerJoinAttributes += " = [" + relatedDataNode.Alias + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                            InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                        }
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;

            return FromClauseSQLQuery;
        }
        /// <MetaDataID>{33876730-B6FF-4950-B20F-15AE32FF9D5C}</MetaDataID>
        private string BuildObjectLinksSQLStatament(MetaDataRepository.Association association, Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> objectsLinks)
        {
            string sqlStatament = null;
            if (objectsLinks.Count == 0)
            {

                foreach (RDBMSMetaDataRepository.Column column in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                {
                    if (sqlStatament == null)
                        sqlStatament = " (SELECT ";
                    else
                        sqlStatament += ",";
                    sqlStatament += " NULL as " + column.Name;
                }

                foreach (RDBMSMetaDataRepository.Column column in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                {
                    if (sqlStatament == null)
                        sqlStatament = " (SELECT ";
                    else
                        sqlStatament += ",";
                    sqlStatament += " NULL as " + column.Name;
                }


                if (association.RoleA.Indexer && (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament = " (SELECT ";
                    else
                        sqlStatament += ",";
                    sqlStatament += " NULL as " + (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                }
                if (association.RoleB.Indexer && (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                {
                    if (sqlStatament == null)
                        sqlStatament = " (SELECT ";
                    else
                        sqlStatament += ",";
                    sqlStatament += " NULL as " + (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                }



                return sqlStatament + ") ";

            }
            System.Collections.Generic.List<string> translatedObjectsLinks = new System.Collections.Generic.List<string>();
            RDBMSMetaDataRepository.AssociationEnd assEnd = association.RoleA as RDBMSMetaDataRepository.AssociationEnd;
            OOAdvantech.RDBMSMetaDataRepository.View view = null;
            if (association.LinkClass != null)
            {
                Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                foreach (RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks)
                    storageCells.AddRange(objectsLink.AssotiationClassStorageCells);
                view = (association.LinkClass as RDBMSMetaDataRepository.MappedClassifier).GetTypeView(storageCells);
            }
            else
            {
                if (objectsLinks != null && objectsLinks.Count > 0)
                {
                    view = new OOAdvantech.RDBMSMetaDataRepository.View("Temporary");
                    System.Collections.Generic.List<RDBMSMetaDataRepository.Table> objectsLinksTables = new System.Collections.Generic.List<RDBMSMetaDataRepository.Table>();


                    foreach (RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks)
                    {
                        if (objectsLink.ObjectLinksTable != null && !objectsLinksTables.Contains(objectsLink.ObjectLinksTable))
                            objectsLinksTables.Add(objectsLink.ObjectLinksTable);
                        if (objectsLink.ObjectLinksTable == null & objectsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        {

                            RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns = (objectsLink.Type as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                            OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Column> RoleATableColumns = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>();
                            OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Column> RoleBTableColumns = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>();
                            if (associationEndWithReferenceColumns.IsRoleA)
                            {
                                foreach (RDBMSMetaDataRepository.Column column in associationEndWithReferenceColumns.GetReferenceColumns())
                                    RoleATableColumns.Add(column);

                                foreach (RDBMSMetaDataRepository.Column column in (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                                    RoleBTableColumns.Add(column);



                            }
                            else
                            {
                                foreach (RDBMSMetaDataRepository.Column column in associationEndWithReferenceColumns.GetReferenceColumns())
                                    RoleBTableColumns.Add(column);

                                foreach (RDBMSMetaDataRepository.Column column in (associationEndWithReferenceColumns.GetOtherEnd().Specification as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                                    RoleATableColumns.Add(column);


                            }
                            string formClause = null;
                            if (associationEndWithReferenceColumns.IsRoleA)
                                formClause = (objectsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name;// +" INNER JOIN " + (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name;
                            else
                                formClause = (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name;

                            //formClause += " ON ";
                            //for (int i = 0; i != RoleATableColumns.Count; i++)
                            //{
                            //    if (i != 0)
                            //        formClause += " AND ";
                            //    formClause += (objectsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name + "." + RoleATableColumns[i].Name;
                            //    formClause += " = ";
                            //    formClause += (objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name + "." + RoleBTableColumns[i].Name;
                            //}
                            int i = 0;
                            string selectClause = "";
                            foreach (RDBMSMetaDataRepository.Column column in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                            {
                                if (i > 0)
                                    selectClause += ",";
                                selectClause += /*(objectsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name + "."+*/ RoleATableColumns[i].Name + " as " + column.Name;
                            }
                            foreach (RDBMSMetaDataRepository.Column column in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                            {

                                selectClause += ",";
                                selectClause += /*(objectsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.Name + "." + */RoleBTableColumns[i].Name + " as " + column.Name;
                            }



                            string associationTableStatement = "select " + selectClause + " from " + formClause;
                            translatedObjectsLinks.Add(associationTableStatement);


                            int tt = 0;



                        }
                    }


                    foreach (RDBMSMetaDataRepository.Table objectsLinkTable in objectsLinksTables)
                    {
                        if (view.ViewColumns == null || view.ViewColumns.Count == 0)
                        {
                            foreach (RDBMSMetaDataRepository.Column column in objectsLinkTable.ContainedColumns)
                                view.AddColumn(new RDBMSMetaDataRepository.Column(column));
                        }

                        OOAdvantech.RDBMSMetaDataRepository.View subView = new OOAdvantech.RDBMSMetaDataRepository.View(objectsLinkTable.Name);
                        subView.AddJoinedTable(objectsLinkTable);
                        foreach (RDBMSMetaDataRepository.Column column in objectsLinkTable.ContainedColumns)
                            subView.AddColumn(new RDBMSMetaDataRepository.Column(column));

                        view.AddSubView(subView);

                    }
                }
                else
                {
                    view = new OOAdvantech.RDBMSMetaDataRepository.View("Temporary");
                    foreach (RDBMSMetaDataRepository.Column column in (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                        view.AddColumn(column);
                    foreach (RDBMSMetaDataRepository.Column column in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                        view.AddColumn(column);
                }
            }

            System.Collections.ArrayList columns = new System.Collections.ArrayList();

            foreach (RDBMSMetaDataRepository.Column column in assEnd.GetReferenceColumns())
                columns.Add(column.Name);


            foreach (RDBMSMetaDataRepository.Column column in (assEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                columns.Add(column.Name);
            if (assEnd.Indexer)
                columns.Add(assEnd.IndexerColumn.Name);

            if (assEnd.GetOtherEnd().Indexer)
                columns.Add((assEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);



            if (view.SubViews.Count > 0)
            {

                foreach (RDBMSMetaDataRepository.View CurrSubView in view.SubViews)
                {
                    if (sqlStatament != null)
                        sqlStatament += "\nUNION ALL \n";
                    else
                        sqlStatament = "(";
                    RDBMSMetaDataRepository.StorageCell storageCell = null;
                    if (view.ViewStorageCell != null && view.ViewStorageCell.ContainsKey(CurrSubView))
                    {
                        storageCell = view.ViewStorageCell[CurrSubView];
                        //Temporary of.
                        //(this.DataNode.ObjectQuery as OQLStatement).StorageCells.Add(storageCell.GetHashCode(), storageCell);
                    }
                    sqlStatament += BuildSelectQuery(columns, CurrSubView, storageCell);
                }
            }
            foreach (string translatedObjectsLink in translatedObjectsLinks)
            {
                if (sqlStatament != null)
                    sqlStatament += "\nUNION ALL \n";
                else
                    sqlStatament = "(";
                sqlStatament += translatedObjectsLink;
            }

            return sqlStatament + ") ";
        }



        private string BuildManyToManyCountedItems(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode,
                                Dictionary<DataNode, bool> relatedDataNodes, out string joinWhereAttributes)
        {
            joinWhereAttributes = null;
            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

            if (relatedDataNode == DataNode.ParentDataNode)
            {
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
            }
            else
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

            #region precondition check
            if (associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            #endregion

            string FromClauseSQLQuery = null;
            associationEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in this.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in association.GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                    ObjectsLinks.AddRange(association.GetStorageCellsLinks(CurrStorageCell));
            }
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = BuildObjectLinksSQLStatament(association, ObjectsLinks);
            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(association.Name);

            #endregion

            #region Construct data source to association table join.

            FromClauseSQLQuery += AssociationTableStatement + " AS [" + AliasAssociationTableName + "] ";




            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");


            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (joinWhereAttributes != null)
                            joinWhereAttributes += " AND ";

                        joinWhereAttributes += DataNode.Alias + ".[" + CurrColumn.Name + "]";
                        joinWhereAttributes += " = [" + AliasAssociationTableName + "]";
                        joinWhereAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }
            FromClauseSQLQuery += " ";

            if (ObjectsLinks.Count == 0)
                return FromClauseSQLQuery;
            #endregion

            #region Construct association table to sub node data source join.


            DataLoader subDataNodeDataLoader = null;
            if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;


            FromClauseSQLQuery += " INNER JOIN ";

            string subDataNodeFromClauseSQLQuery = null;
            if (subDataNodeDataLoader != null)
                subDataNodeFromClauseSQLQuery = (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(this, relatedDataNodes);




            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += "( ";
            RDBMSMetaDataRepository.MappedClassifier OtherEndClass = (RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
            RDBMSMetaDataRepository.AssociationEnd OtherEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            if (subDataNodeDataLoader != null)
                FromClauseSQLQuery += (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";//" Abstract_"+OtherEndClass.Name+" ON ";


            FromClauseSQLQuery += subDataNodeFromClauseSQLQuery;
            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += ") ON ";
            else
                FromClauseSQLQuery += " ON ";



            FirstJoinTableColumns = (OtherEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();

            SecondJoinTableColumns = OtherEndClass.ObjectIDColumns;

            string innerJoinAttributes = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (innerJoinAttributes != null)
                            innerJoinAttributes += " AND ";
                        innerJoinAttributes += "[" + AliasAssociationTableName + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                        innerJoinAttributes += ".[" + CurrColumn.Name + "]";
                        innerJoinAttributes += " = [" + relatedDataNode.Alias + "].[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }

            FromClauseSQLQuery += " " + innerJoinAttributes;
            #endregion


            return FromClauseSQLQuery;

        }




        /// <summary>Build a join between the table from data source and
        /// association table and a join between association table end data source table of sub data node. </summary>
        /// <MetaDataID>{97E28D6A-7A8E-40B2-81FF-B4B2AE305DD5} </MetaDataID>
        private string BuildManyToManyTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode,
                                        bool forRelationDataOnly,
                                        DataLoader headerDataloader,
                                        Dictionary<DataNode, bool> relatedDataNodes,
                                        bool innerJoin)
        {
            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;

            if (relatedDataNode == DataNode.ParentDataNode)
            {

                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
            }
            else
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

            #region precondition check
            if (associationEnd == null || associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany && associationEnd.Association.LinkClass == null)
                throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + relatedDataNode.Name);
            #endregion

            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;

            string FromClauseSQLQuery = null;
            associationEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in this.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in association.GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                    ObjectsLinks.AddRange(association.GetStorageCellsLinks(CurrStorageCell));
            }
            //ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = BuildObjectLinksSQLStatament(association, ObjectsLinks);
            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(association.Name);

            #endregion

            #region Construct data source to association table join.


            if (innerJoin)
                FromClauseSQLQuery = " INNER JOIN ";
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";

            FromClauseSQLQuery += AssociationTableStatement + " AS [" + AliasAssociationTableName + "] ON ";



            string InnerJoinAttributes = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");


            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";

                        InnerJoinAttributes += DataNode.Alias + ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + AliasAssociationTableName + "]";
                        InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;
            string filter = null;
            string orderByClause = null;

            if (ObjectsLinks.Count == 0)
            {

                if (forRelationDataOnly)
                {
                    string selectList = null;
                    foreach (RDBMSMetaDataRepository.Column cloumn in associationEnd.GetReferenceColumns())
                    {
                        if (selectList == null)
                            selectList = "SELECT ";
                        else
                            selectList += "'";
                        selectList += cloumn.Name;
                        if (filter != null)
                            filter += " AND ";
                        filter += cloumn.Name + " IS NOT NULL ";
                    }
                    foreach (RDBMSMetaDataRepository.Column cloumn in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                    {
                        selectList += ",";
                        selectList += cloumn.Name;
                        if (filter != null)
                            filter += " AND ";
                        filter += cloumn.Name + " IS NOT NULL ";

                    }
                    if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                    {
                        if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.Identity)
                        {
                            if (associationEnd.IndexerColumn != null)
                            {
                                selectList += "," + associationEnd.IndexerColumn.Name;
                                orderByClause = " ORDER BY " + associationEnd.IndexerColumn.Name;
                            }
                        }
                        else if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.GetOtherEnd().Identity)
                        {
                            if ((associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                            {
                                selectList += "," + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                                orderByClause = " ORDER BY " + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                            }

                        }
                    }
                    //                    orderByClause = null;
                    return selectList + " FROM " + BuildSQLStatament() + " AS [" + DataNode.Alias + "] " + FromClauseSQLQuery + " WHERE " + filter + orderByClause;

                }

                return FromClauseSQLQuery;
            }
            #endregion

            #region Construct association table to sub node data source join.


            DataLoader subDataNodeDataLoader = null;
            if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;


            if (forRelationDataOnly)
            {
                string selectList = null;
                foreach (RDBMSMetaDataRepository.Column cloumn in associationEnd.GetReferenceColumns())
                {
                    if (selectList == null)
                        selectList = "SELECT DISTINCT ";
                    else
                        selectList += "'";
                    selectList += cloumn.Name;
                    if (filter != null)
                        filter += " AND ";
                    filter += cloumn.Name + " IS NOT NULL ";

                }
                foreach (RDBMSMetaDataRepository.Column cloumn in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                {
                    selectList += ",";
                    selectList += cloumn.Name;
                    if (filter != null)
                        filter += " AND ";
                    filter += cloumn.Name + " IS NOT NULL ";

                }
                if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                {
                    if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.Identity)
                    {
                        selectList += "," + associationEnd.IndexerColumn.Name;
                        orderByClause = " ORDER BY " + associationEnd.IndexerColumn.Name;
                    }
                    else if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.GetOtherEnd().Identity)
                    {
                        selectList += "," + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                        orderByClause = " ORDER BY " + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;

                    }

                }
                //                orderByClause = null;
                return selectList + " FROM " + BuildSQLStatament() + " AS [" + DataNode.Alias + "] " + FromClauseSQLQuery + " WHERE " + filter + orderByClause;

            }






            if (innerJoin)
                FromClauseSQLQuery += " INNER JOIN ";
            else
                FromClauseSQLQuery += " LEFT OUTER JOIN ";

            string subDataNodeFromClauseSQLQuery = null;
            if (subDataNodeDataLoader != null)
                subDataNodeFromClauseSQLQuery = (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);




            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += "( ";
            RDBMSMetaDataRepository.MappedClassifier OtherEndClass = (RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
            RDBMSMetaDataRepository.AssociationEnd OtherEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            if (subDataNodeDataLoader != null)
                FromClauseSQLQuery += (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";//" Abstract_"+OtherEndClass.Name+" ON ";


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
                        InnerJoinAttributes += ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + relatedDataNode.Alias + "].[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }

            FromClauseSQLQuery += " " + InnerJoinAttributes;
            #endregion


            return FromClauseSQLQuery;

        }



        private string BuildAbstractAssociationCountedItems(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode,
                                Dictionary<DataNode, bool> relatedDataNodes, out string joinWhereAttributes)
        {
            joinWhereAttributes = null;
            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            if (relatedDataNode == DataNode.ParentDataNode)
            {
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
            }
            else
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

            #region precondition check
            //if (associationend == null || associationend.association.multiplicitytype != metadatarepository.associationtype.manytomany && associationend.association.linkclass == null)
            //    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + subDataNode.Name);
            #endregion

            string FromClauseSQLQuery = null;
            associationEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in this.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                    ObjectsLinks.AddRange(((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell));
            }
            //ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = BuildObjectLinksSQLStatament(associationEnd.Association, ObjectsLinks);
            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);

            #endregion

            #region Construct data source to association table join.



            FromClauseSQLQuery += AssociationTableStatement + " AS [" + AliasAssociationTableName + "] ";



            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;

            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
            if (SecondJoinTableColumns.Count != FirstJoinTableColumns.Count)
                throw new System.Exception("Incorrect mapping of " + associationEnd.FullName + " association");


            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (joinWhereAttributes != null)
                            joinWhereAttributes += " AND ";

                        joinWhereAttributes += DataNode.Alias + ".[" + CurrColumn.Name + "]";
                        joinWhereAttributes += " = [" + AliasAssociationTableName + "]";
                        joinWhereAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }
            FromClauseSQLQuery += " ";
            string filter = null;
            string orderByClause = null;

            if (ObjectsLinks.Count == 0)
                return FromClauseSQLQuery;
            #endregion


            #region Construct association table to sub node data source join.


            DataLoader subDataNodeDataLoader = null;
            if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;




            FromClauseSQLQuery += " INNER JOIN ";

            string subDataNodeFromClauseSQLQuery = null;
            if (subDataNodeDataLoader != null)
                subDataNodeFromClauseSQLQuery = (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(this, relatedDataNodes);




            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += "( ";
            RDBMSMetaDataRepository.MappedClassifier OtherEndClass = (RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
            RDBMSMetaDataRepository.AssociationEnd OtherEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            if (subDataNodeDataLoader != null)
                FromClauseSQLQuery += (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";//" Abstract_"+OtherEndClass.Name+" ON ";


            FromClauseSQLQuery += subDataNodeFromClauseSQLQuery;
            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += ") ON ";
            else
                FromClauseSQLQuery += " ON ";



            FirstJoinTableColumns = (OtherEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();

            SecondJoinTableColumns = OtherEndClass.ObjectIDColumns;

            string InnerJoinAttributes = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn CurrColumn in FirstJoinTableColumns)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn CorrespondingCurrColumn in SecondJoinTableColumns)
                {
                    if (CurrColumn.ColumnType == CorrespondingCurrColumn.ColumnType)
                    {
                        if (InnerJoinAttributes != null)
                            InnerJoinAttributes += " AND ";
                        InnerJoinAttributes += "[" + AliasAssociationTableName + "]";//((RDBMSMetaDataRepository.Association)AssociationEnd.Association).ActiveObjectLinksStorage.ObjectLinksTable.Name;
                        InnerJoinAttributes += ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + relatedDataNode.Alias + "].[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }

            FromClauseSQLQuery += " " + InnerJoinAttributes;
            #endregion


            return FromClauseSQLQuery;
        }


        private string BuildAbstractAssociationTablesJoin(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode, bool forRelationDataOnly, DataLoader headerDataloader, System.Collections.Generic.Dictionary<DataNode, bool> relatedDataNodes, bool innerJoin)
        {

            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            if (relatedDataNode == DataNode.ParentDataNode)
            {
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                associationEnd = associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
            }
            else
                associationEnd = (LoadFromStorage as Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

            #region precondition check
            //if (associationend == null || associationend.association.multiplicitytype != metadatarepository.associationtype.manytomany && associationend.association.linkclass == null)
            //    throw new System.Exception("You can't build many to many SQL statement because there isn't the proper relationsip between " + DataNode.FullName + " and " + subDataNode.Name);
            #endregion

            string FromClauseSQLQuery = null;
            associationEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();

            #region Construct ObjectLinksDataSource object
            Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>();

            foreach (RDBMSMetaDataRepository.StorageCell CurrStorageCell in this.DataLoaderMetadata.StorageCells)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    valueTypePath.Push(DataNode.AssignedMetaObject.Identity);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell))
                    {
                        if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                            ObjectsLinks.Add(storageCellsLink);
                    }

                }
                else
                    ObjectsLinks.AddRange(((RDBMSMetaDataRepository.Association)associationEnd.Association).GetStorageCellsLinks(CurrStorageCell));
            }
            //ObjectLinksDataSource objectLinksDataSource = new ObjectLinksDataSource(associationEnd.Association, ObjectsLinks);
            #endregion

            #region Construct the joined assoctition table.
            string AssociationTableStatement = BuildObjectLinksSQLStatament(associationEnd.Association, ObjectsLinks);
            string AliasAssociationTableName = null;
            AliasAssociationTableName = DataNode.ObjectQuery.GetValidAlias(((RDBMSMetaDataRepository.Association)associationEnd.Association).Name);

            #endregion

            #region Construct data source to association table join.

            if (innerJoin)
                FromClauseSQLQuery = " INNER JOIN ";
            else
                FromClauseSQLQuery = " LEFT OUTER JOIN ";


            FromClauseSQLQuery += AssociationTableStatement + " AS [" + AliasAssociationTableName + "] ON ";



            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> FirstJoinTableColumns = (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;

            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> SecondJoinTableColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns();
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

                        InnerJoinAttributes += DataNode.Alias + ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + AliasAssociationTableName + "]";
                        InnerJoinAttributes += ".[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }
            FromClauseSQLQuery += InnerJoinAttributes;
            string filter = null;
            string orderByClause = null;

            if (ObjectsLinks.Count == 0)
            {

                if (forRelationDataOnly)
                {
                    string selectList = null;
                    foreach (RDBMSMetaDataRepository.Column cloumn in associationEnd.GetReferenceColumns())
                    {
                        if (selectList == null)
                            selectList = "SELECT ";
                        else
                            selectList += "'";
                        selectList += "[" + AliasAssociationTableName + "]." + cloumn.Name;
                        if (filter != null)
                            filter += " AND ";
                        filter += "[" + AliasAssociationTableName + "]." + cloumn.Name + " IS NOT NULL ";
                    }
                    foreach (RDBMSMetaDataRepository.Column cloumn in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                    {
                        selectList += ",";
                        selectList += "[" + AliasAssociationTableName + "]." + cloumn.Name;
                        if (filter != null)
                            filter += " AND ";
                        filter += "[" + AliasAssociationTableName + "]." + cloumn.Name + " IS NOT NULL ";

                    }
                    if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                    {
                        if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.Identity)
                        {
                            if (associationEnd.IndexerColumn != null)
                            {
                                selectList += "," + associationEnd.IndexerColumn.Name;
                                orderByClause = " ORDER BY " + associationEnd.IndexerColumn.Name;
                            }
                        }
                        else if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.GetOtherEnd().Identity)
                        {
                            if ((associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn != null)
                            {
                                selectList += "," + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                                orderByClause = " ORDER BY " + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                            }

                        }
                    }
                    //                    orderByClause = null;
                    return selectList + " FROM " + BuildSQLStatament() + " AS [" + DataNode.Alias + "] " + FromClauseSQLQuery + " WHERE " + filter + orderByClause;

                }

                return FromClauseSQLQuery;
            }
            #endregion


            #region Construct association table to sub node data source join.


            DataLoader subDataNodeDataLoader = null;
            if (relatedDataNode.DataSource.DataLoaders.ContainsKey(LoadFromStorage.StorageIdentity))
                subDataNodeDataLoader = relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader;


            if (forRelationDataOnly)
            {
                string selectList = null;
                foreach (RDBMSMetaDataRepository.Column cloumn in associationEnd.GetReferenceColumns())
                {
                    if (selectList == null)
                        selectList = "SELECT DISTINCT ";
                    else
                        selectList += "'";
                    selectList += "[" + AliasAssociationTableName + "]." + cloumn.Name;
                    if (filter != null)
                        filter += " AND ";
                    filter += "[" + AliasAssociationTableName + "]." + cloumn.Name + " IS NOT NULL ";

                }
                foreach (RDBMSMetaDataRepository.Column cloumn in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
                {
                    selectList += ",";
                    selectList += "[" + AliasAssociationTableName + "]." + cloumn.Name;
                    if (filter != null)
                        filter += " AND ";
                    filter += "[" + AliasAssociationTableName + "]." + cloumn.Name + " IS NOT NULL ";

                }
                if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                {
                    if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.Identity)
                    {
                        selectList += "," + associationEnd.IndexerColumn.Name;
                        orderByClause = " ORDER BY " + associationEnd.IndexerColumn.Name;
                    }
                    else if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity == associationEnd.GetOtherEnd().Identity)
                    {
                        selectList += "," + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;
                        orderByClause = " ORDER BY " + (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name;

                    }

                }
                //                orderByClause = null;
                return selectList + " FROM " + BuildSQLStatament() + " AS [" + DataNode.Alias + "] " + FromClauseSQLQuery + " WHERE " + filter + orderByClause;

            }

            if (innerJoin)
                FromClauseSQLQuery += " INNER JOIN ";
            else
                FromClauseSQLQuery += " LEFT OUTER JOIN ";

            string subDataNodeFromClauseSQLQuery = null;
            if (subDataNodeDataLoader != null)
                subDataNodeFromClauseSQLQuery = (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildFromClauseSQLQuery(headerDataloader, relatedDataNodes);




            if (subDataNodeFromClauseSQLQuery != null && subDataNodeFromClauseSQLQuery.Trim().Length > 0)
                FromClauseSQLQuery += "( ";
            RDBMSMetaDataRepository.MappedClassifier OtherEndClass = (RDBMSMetaDataRepository.MappedClassifier)associationEnd.GetOtherEnd().Specification;
            RDBMSMetaDataRepository.AssociationEnd OtherEnd = (RDBMSMetaDataRepository.AssociationEnd)associationEnd.GetOtherEnd();
            if (subDataNodeDataLoader != null)
                FromClauseSQLQuery += (relatedDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity] as DataLoader).BuildSQLStatament() + " AS [" + relatedDataNode.Alias + "] ";//" Abstract_"+OtherEndClass.Name+" ON ";


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
                        InnerJoinAttributes += ".[" + CurrColumn.Name + "]";
                        InnerJoinAttributes += " = [" + relatedDataNode.Alias + "].[" + CorrespondingCurrColumn.Name + "]";
                    }
                }
            }

            FromClauseSQLQuery += " " + InnerJoinAttributes;
            #endregion


            return FromClauseSQLQuery;
        }

    }
}