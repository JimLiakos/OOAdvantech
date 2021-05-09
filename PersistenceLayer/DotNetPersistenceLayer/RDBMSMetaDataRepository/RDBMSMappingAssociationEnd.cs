namespace OOAdvantech.RDBMSMetaDataRepository
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using System.Collections.Generic;
    using System.Linq;

    /// <MetaDataID>{F0FAC89B-5098-4615-A603-5C814FE362D0}</MetaDataID>
    [BackwardCompatibilityID("{F0FAC89B-5098-4615-A603-5C814FE362D0}")]
    [Persistent()]
    public class AssociationEnd : MetaDataRepository.AssociationEnd
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_IdentityTypes))
            {
                if (value == null)
                    _IdentityTypes = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.ObjectIdentityType>);
                else
                    _IdentityTypes = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.ObjectIdentityType>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IndexerColumns))
            {
                if (value == null)
                    _IndexerColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _IndexerColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MappedColumns))
            {
                if (value == null)
                    _MappedColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _MappedColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ReferenceColumns))
            {
                if (value == null)
                    ReferenceColumns = default(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.StorageCell, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn>>>);
                else
                    ReferenceColumns = (System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.StorageCell, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn>>>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(PathIndexerColumns))
            {
                if (value == null)
                    PathIndexerColumns = default(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.StorageCell, System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataRepository.Column>>);
                else
                    PathIndexerColumns = (System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.StorageCell, System.Collections.Generic.Dictionary<string, OOAdvantech.RDBMSMetaDataRepository.Column>>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IndexerColumn))
            {
                if (value == null)
                    _IndexerColumn = default(OOAdvantech.RDBMSMetaDataRepository.Column);
                else
                    _IndexerColumn = (OOAdvantech.RDBMSMetaDataRepository.Column)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_IdentityTypes))
                return _IdentityTypes;

            if (member.Name == nameof(_IndexerColumns))
                return _IndexerColumns;

            if (member.Name == nameof(_MappedColumns))
                return _MappedColumns;

            if (member.Name == nameof(ReferenceColumns))
                return ReferenceColumns;

            if (member.Name == nameof(PathIndexerColumns))
                return PathIndexerColumns;

            if (member.Name == nameof(_IndexerColumn))
                return _IndexerColumn;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{afdae6a0-4e2c-4c80-b670-df90e7a65551}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.ObjectIdentityType> _IdentityTypes = new OOAdvantech.Collections.Generic.Set<ObjectIdentityType>();
        /// <MetaDataID>{acbf0ee8-9638-4686-83b8-451341893bb3}</MetaDataID>
        [MetaDataRepository.Association("AssociationEndColumnIdentityType", typeof(MetaDataRepository.ObjectIdentityType), MetaDataRepository.Roles.RoleA, "bd96657f-3926-443e-b381-23fb8d8fe2bc")]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [PersistentMember("_IdentityTypes")]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.ObjectIdentityType> IdentityTypes
        {
            get
            {
                return _IdentityTypes;
            }
        }





        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Column> _IndexerColumns;
        /// <MetaDataID>{91d53309-148f-49f8-8c6f-dcba63f48aae}</MetaDataID>
        [OOAdvantech.MetaDataRepository.Association("ColumnForIndex", Roles.RoleA, "750984d2-8340-486f-b377-131c2f9a7128")]
        [PersistentMember("_IndexerColumns")]
        [AssociationEndBehavior(MetaDataRepository.PersistencyFlag.OnConstruction | MetaDataRepository.PersistencyFlag.CascadeDelete)]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<Column> IndexerColumns
        {
            set
            {

            }
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return new Collections.Generic.Set<Column>(_IndexerColumns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <MetaDataID>{2C496385-CF85-4D7F-9A9B-C7FB1A381F50}</MetaDataID>
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        public override void Synchronize(MetaObject OriginMetaObject)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                MetaDataRepository.AssociationEnd originAssociationEnd = (MetaDataRepository.AssociationEnd)OriginMetaObject;
                if (_Multiplicity != null)
                {
                    if (_Multiplicity.IsMany && !originAssociationEnd.Multiplicity.IsMany)
                    {
                        if (!(OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator is OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack) || !(OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack).ManualMapping)
                        {
                            if(MappedColumns.ToList().Count!=0|| (GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).MappedColumns.ToList().Count!=0)
                                throw new System.Exception("System can't implicitly convert  the multiplicity of association end '" + GetOtherEnd().Specification.FullName + "." + Name + "' from 'many' to 'exactly One' or 'Zero or One'.");
                        }
                    }

                }
                ReferenceColumns = null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            base.Synchronize(OriginMetaObject);
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6871B94C-5E27-448B-99E6-779876FA1801}</MetaDataID>
        private Collections.Generic.Set<Column> _MappedColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{FAF20458-7A79-4927-A5FC-52C8DD89BFA4}</MetaDataID>
        [OOAdvantech.MetaDataRepository.Association("AssociationEndColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{732E13CB-2C8D-4E7B-8219-89B25403A7CE}")]
        [AssociationEndBehavior(PersistencyFlag.AllowTransient)]
        [PersistentMember("_MappedColumns")]
        [TransactionalMember(LockOptions.Shared, "_MappedColumns")]
        [RoleAMultiplicityRange(1)]
        public Collections.Generic.Set<Column> MappedColumns
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new Collections.Generic.Set<Column>(_MappedColumns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }



        /// <MetaDataID>{9323a877-1df9-4814-909c-9fe09be36c69}</MetaDataID>
        public void RemoveReferenceColumsFor(StorageCellsLink storageCellsLink, OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity)
        {
            if (ReferenceColumns != null)
                ReferenceColumns.Clear();
            List<MetaDataRepository.StorageCellsLink> storageCellsLinks = new List<MetaDataRepository.StorageCellsLink>(Association.StorageCellsLinks);
            storageCellsLinks.Remove(storageCellsLink);
            using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                foreach (Column column in new Collections.Generic.Set<Column>(_MappedColumns))
                {
                    if (column.CreatorIdentity == pathIdentity.ToString())
                    {
                        if ((column.Namespace as Table).TableCreator == storageCellsLink)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _MappedColumns.Remove(column);
                        }
                        else if (storageCellsLink.RoleAStorageCell == (column.Namespace as Table).TableCreator ||
                            storageCellsLink.RoleBStorageCell == (column.Namespace as Table).TableCreator)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _MappedColumns.Remove(column);

                        }
                        else if (IsRoleA && (column.Namespace as Table).TableCreator == storageCellsLink)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _MappedColumns.Remove(column);
                        }
                    }
                }
                StateTransition.Consistent = true;
            }
        }


        public void RemoveIndexerColumsFor(StorageCellsLink storageCellsLink, OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity)
        {
            if (PathIndexerColumns != null)
                PathIndexerColumns.Clear();
            List<MetaDataRepository.StorageCellsLink> storageCellsLinks = new List<MetaDataRepository.StorageCellsLink>(Association.StorageCellsLinks);
            storageCellsLinks.Remove(storageCellsLink);
            using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                foreach (Column column in new Collections.Generic.Set<Column>(_IndexerColumns))
                {
                    if (column.CreatorIdentity == pathIdentity.ToString())
                    {
                        if ((column.Namespace as Table).TableCreator == storageCellsLink)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _IndexerColumns.Remove(column);
                        }
                        else if (storageCellsLink.RoleAStorageCell == (column.Namespace as Table).TableCreator ||
                            storageCellsLink.RoleBStorageCell == (column.Namespace as Table).TableCreator)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _IndexerColumns.Remove(column);

                        }
                        else if (IsRoleA && (column.Namespace as Table).TableCreator == storageCellsLink)
                        {
                            (column.Namespace as Table).RemoveColumn(column);
                            _IndexerColumns.Remove(column);
                        }
                    }
                }
                StateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{784ACC97-B2C4-410C-A7F7-E98FDA9A8B36}</MetaDataID>
        public void RemoveUnusedReferenceColums()
        {
            if (_MappedColumns.Count > 0)
            {
                if (PathIndexerColumns != null)
                    PathIndexerColumns.Clear();
                //MetaDataRepository.ObjectIdentityType objectIdentity = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this);
                //System.Collections.ArrayList classes = new System.Collections.ArrayList();
                //System.Collections.ArrayList Interfaces = new System.Collections.ArrayList();
                foreach (Column column in _MappedColumns)
                {
                    if ((column.Namespace as Table).TableCreator is StorageCell && ((column.Namespace as Table).TableCreator as StorageCell).AutoGenarated)
                    {

                        MetaDataRepository.ObjectIdentityType objectIdentityTypeReference = GetReferenceObjectIdentityType(((column.Namespace as Table).TableCreator as StorageCell).Type);
                        //There isn't collumns for the classifier of storage cell  
                        if (objectIdentityTypeReference == null)
                        {
                            Table table = column.Namespace as Table;
                            table.RemoveColumn(column);
                            _MappedColumns.Remove(column);
                            foreach (Column viewColumn in (table.TableCreator as StorageCell).ClassView.ViewColumns)
                            {
                                if (viewColumn.RealColumn == column)
                                {
                                    (table.TableCreator as StorageCell).ClassView.RemoveColumn(viewColumn);
                                    break;
                                }
                            }
                        }
                    }
                }

                foreach (Column column in _IndexerColumns)
                {
                    if ((column.Namespace as Table).TableCreator is StorageCell && ((column.Namespace as Table).TableCreator as StorageCell).AutoGenarated)
                    {
                        MetaDataRepository.ObjectIdentityType objectIdentityTypeReference = GetReferenceObjectIdentityType(((column.Namespace as Table).TableCreator as StorageCell).Type);
                        //There isn't collumns for the classifier of storage cell  
                        if (objectIdentityTypeReference == null)
                        {
                            Table table = column.Namespace as Table;
                            table.RemoveColumn(column);
                            _IndexerColumns.Remove(column);
                            foreach (Column viewColumn in (table.TableCreator as StorageCell).ClassView.ViewColumns)
                            {
                                if (viewColumn.RealColumn == column)
                                {
                                    (table.TableCreator as StorageCell).ClassView.RemoveColumn(viewColumn);
                                    break;
                                }
                            }

                        }
                    }
                }


            }
        }

        /// <summary>This method creates and the columns of association end and add them to table if needed. </summary>
        /// <param name="table">Define the table which the association end will add the columns if needed. </param>
        /// <remarks>
        /// If the class of storage cell of table is link class of association then 
        /// the columns added to table always. 
        /// In case where the class of storage cell of table is association end specification class,
        /// the columns addition is up to the multiplicity of association end.
        /// </remarks>
        /// <MetaDataID>{3b934d16-52a4-456f-a138-e9b8ce1f997c}</MetaDataID>
        public Collections.Generic.Set<IdentityColumn> AddReferenceColumnsToTable(Table table)
        {
            return AddReferenceColumnsToTable(table, "", new ValueTypePath(), null);
        }

        /// <summary>This method creates and the columns of association end and add them to table if needed. </summary>
        /// <param name="table">Define the table which the association end will add the columns if needed. </param>
        /// <remarks>
        /// If the class of storage cell of table is link class of association then 
        /// the columns added to table always. 
        /// In case where the class of storage cell of table is association end specification class,
        /// the columns addition is up to the multiplicity of association end.
        /// </remarks>
        /// <MetaDataID>{ce85b673-6b8b-4546-a7f0-8f0bf84a4973}</MetaDataID>
        public Collections.Generic.Set<IdentityColumn> AddReferenceColumnsToTable(Table table, MetaDataRepository.ObjectIdentityType objectIdentityType)
        {
            //TODO Να γραφτεί ένα Test case όπου θα χρειαστεί να φτιαχτούν δύο colums με το ίδιο όνομα
            //από δύο διαφορετικές assotiation με assotiation end με το ιδιο όνομα.
            return AddReferenceColumnsToTable(table, "", new ValueTypePath(), objectIdentityType);

        }


        /// <MetaDataID>{23041506-d53b-4e96-bed2-7dba0115bec0}</MetaDataID>
        internal Collections.Generic.Set<IdentityColumn> AddReferenceColumnsToTable(Table table, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            return AddReferenceColumnsToTable(table, path, pathIdentity, null);
        }

        /// <summary>This method creates and the columns of association end and add them to table if needed. </summary>
        /// <param name="table">Define the table which the association end will add the columns if needed. </param>
        /// <remarks>
        /// If the class of storage cell of table is link class of association then 
        /// the columns added to table always. 
        /// In case where the class of storage cell of table is association end specification class,
        /// the columns addition is up to the multiplicity of association end.
        /// </remarks>
        /// <MetaDataID>{BF90C0BA-F4AE-4428-AB52-71A851C76916}</MetaDataID>
        internal Collections.Generic.Set<IdentityColumn> AddReferenceColumnsToTable(Table table, string path, MetaDataRepository.ValueTypePath pathIdentity, MetaDataRepository.ObjectIdentityType objectIdentityType)
        {

            try
            {
                if (PathIndexerColumns != null)
                    PathIndexerColumns.Clear();
                if (ReferenceColumns != null)
                    ReferenceColumns.Clear();

                if (!string.IsNullOrEmpty(path))
                    path += "_";

                if (pathIdentity.Count > 0) //the owner isn't valueType
                    pathIdentity.Push(Identity);

                Collections.Generic.Set<IdentityColumn> columns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>();
                foreach (IdentityColumn column in GetReferenceColumnsFor(table))
                {
                    if (column.CreatorIdentity == pathIdentity.ToString())
                        columns.Add(column);
                }


                if (columns.Count > 0)
                {
                    //Collections.Generic.Set<IdentityColumn> associationEndColumns;
                    //if (table.TableCreator is StorageCell)
                    //{
                    //    associationEndColumns = GetReferenceColumns((table.TableCreator as StorageCell).Type);
                    //}
                    //else if (table.TableCreator is StorageCellsLink)
                    //{
                    //    associationEndColumns = GetReferenceColumns();
                    //}
                    //return columns;
                }


                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        if (table.TableCreator is StorageCell)
                        {
                            MetaDataRepository.AssociationEndRealization associationEndRealization = (table.TableCreator as StorageCell).Type.GetAssociationEndRealization(this.GetOtherEnd());

                            columns.Clear();
                            if (objectIdentityType == null)
                                objectIdentityType = GetReferenceObjectIdentityType((table.TableCreator as StorageCell).Type);
                            else
                            {
                                var objectIdentityTypes = GetReferenceObjectIdentityTypes((table.TableCreator as StorageCell).Type, new List<MetaDataRepository.ObjectIdentityType>() { objectIdentityType });
                                if (objectIdentityTypes.Count > 0)
                                    objectIdentityType = objectIdentityTypes[0];
                                else
                                    objectIdentityType = null;
                            }

                            if (objectIdentityType != null)
                            {

                                if (GetReferenceColumnsFor(table).OfType<IdentityColumn>().Where(x => x.ObjectIdentityType == objectIdentityType).Count() == 0)
                                {
                                    foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                    {
                                        IdentityColumn identityColumn = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetIdentityColumn(identityPart, table);
                                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                                        columns.Add(identityColumn);
                                        identityColumn.Name = path + identityColumn.Name;
                                        identityColumn.AllowNulls = true;

                                        _MappedColumns.Add(identityColumn);
                                        table.AddColumn(identityColumn);
                                        identityColumn.CreatorIdentity = pathIdentity.ToString();
                                        identityColumn.ObjectIdentityType = objectIdentityType;
                                        if (associationEndRealization != null)
                                            identityColumn.MappedAssociationEndRealizationIdentity = associationEndRealization.Identity.ToString();
                                    }
                                }

                            }
                            if (Indexer && Multiplicity.IsMany && Association.MultiplicityType != AssociationType.ManyToMany && Navigable)
                            {
                                if (GetIndexerColumnFor(table) == null)
                                {

                                    MetaDataRepository.Primitive systemInt32Type = (Namespace.ImplementationUnit.Context as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(typeof(int))) as MetaDataRepository.Primitive;
                                    if (systemInt32Type == null || systemInt32Type.Name == null)
                                    {
                                   
                                        MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                                        systemInt32Type = new OOAdvantech.RDBMSMetaDataRepository.Primitive();
                                        PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(systemInt32Type);
                                        systemInt32Type.Synchronize(mPrimitive);
                                    }
                                    Column indexerColumn = new Column(Name + "_" + path + "Indexer", systemInt32Type);
                                    indexerColumn.CreatorIdentity = pathIdentity.ToString();
                                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                                    _IndexerColumns.Add(indexerColumn);
                                    table.AddColumn(indexerColumn);
                                    if (associationEndRealization != null)
                                        indexerColumn.MappedAssociationEndRealizationIdentity = associationEndRealization.Identity.ToString();
                                }
                            }
                        }
                        else if (table.TableCreator is StorageCellsLink)
                        {
                            if (IsRoleA)
                            {
                                if (GetReferenceColumnsFor(table).OfType<IdentityColumn>().Where(x => x.ObjectIdentityType == (table.TableCreator as StorageCellsLink).RoleBStorageCell.ObjectIdentityType).Count() == 0)
                                {
                                    foreach (IIdentityPart identityPart in (table.TableCreator as StorageCellsLink).RoleBStorageCell.ObjectIdentityType.Parts)
                                    {
                                        MetaDataRepository.Classifier systemType = (ImplementationUnit.Context as Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(identityPart.Type)) as MetaDataRepository.Classifier;

                                        if (systemType == null)
                                        {
                                            systemType = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(MetaDataRepository.Classifier.GetClassifier(identityPart.Type), table) as Classifier;
                                            if (systemType == null)
                                            {
                                                systemType = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(MetaDataRepository.Classifier.GetClassifier(identityPart.Type), this);
                                                systemType.ShallowSynchronize(MetaDataRepository.Classifier.GetClassifier(identityPart.Type));
                                            }
                                        }
                                        IdentityColumn existIdentityColumn = table.ContainedColumns.OfType<IdentityColumn>().Where(x => x.Name == Association.CaseInsensitiveName + "_" + identityPart.Name + "B" &&
                                        x.Type == systemType && x.ColumnType == identityPart.Name).FirstOrDefault();
                                        if (existIdentityColumn != null && existIdentityColumn.MappedAttribute == null && existIdentityColumn.MappedAssociationEnd == null)
                                        {
                                            columns.Add(existIdentityColumn);
                                            existIdentityColumn.MappedAssociationEnd = this;
                                            _MappedColumns.Add(existIdentityColumn);
                                            table.AddColumn(existIdentityColumn);

                                            existIdentityColumn.CreatorIdentity = pathIdentity.ToString();
                                            existIdentityColumn.Name = path + existIdentityColumn.Name;
                                        }
                                        else
                                        {
                                            IdentityColumn identityColumn = new IdentityColumn(Association.CaseInsensitiveName + "_" + identityPart.Name + "B", systemType, identityPart.Name, false);
                                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                                            columns.Add(identityColumn);
                                            identityColumn.MappedAssociationEnd = this;
                                            _MappedColumns.Add(identityColumn);
                                            table.AddColumn(identityColumn);
                                            identityColumn.CreatorIdentity = pathIdentity.ToString();
                                            identityColumn.Name = path + identityColumn.Name;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (GetReferenceColumnsFor(table).OfType<IdentityColumn>().Where(x => x.ObjectIdentityType == (table.TableCreator as StorageCellsLink).RoleAStorageCell.ObjectIdentityType).Count() == 0)
                                {
                                    foreach (IIdentityPart identityPart in (table.TableCreator as StorageCellsLink).RoleAStorageCell.ObjectIdentityType.Parts)
                                    {
                                        MetaDataRepository.Classifier systemType = (table.TableCreator.Namespace as Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(identityPart.Type)) as MetaDataRepository.Classifier;


                                        if (systemType == null)
                                        {
                                            systemType = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(MetaDataRepository.Classifier.GetClassifier(identityPart.Type), table) as Classifier;
                                            if (systemType == null)
                                            {
                                                systemType = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(MetaDataRepository.Classifier.GetClassifier(identityPart.Type), this);
                                                systemType.ShallowSynchronize(MetaDataRepository.Classifier.GetClassifier(identityPart.Type));
                                            }
                                        }
                                        IdentityColumn existIdentityColumn = table.ContainedColumns.OfType<IdentityColumn>().Where(x => x.Name == Association.CaseInsensitiveName + "_" + identityPart.Name + "A" &&
                                          x.Type == systemType && x.ColumnType == identityPart.Name).FirstOrDefault();
                                        if (existIdentityColumn != null && existIdentityColumn.MappedAttribute == null && existIdentityColumn.MappedAssociationEnd == null)
                                        {
                                            columns.Add(existIdentityColumn);
                                            existIdentityColumn.MappedAssociationEnd = this;
                                            _MappedColumns.Add(existIdentityColumn);
                                            table.AddColumn(existIdentityColumn);
                                            existIdentityColumn.CreatorIdentity = pathIdentity.ToString();
                                            existIdentityColumn.Name = path + existIdentityColumn.Name;
                                        }
                                        else
                                        {
                                            IdentityColumn identityColumn = new IdentityColumn(Association.CaseInsensitiveName + "_" + identityPart.Name + "A", systemType, identityPart.Name, false);
                                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                                            columns.Add(identityColumn);
                                            identityColumn.MappedAssociationEnd = this;
                                            _MappedColumns.Add(identityColumn);
                                            table.AddColumn(identityColumn);
                                            identityColumn.CreatorIdentity = pathIdentity.ToString();
                                            identityColumn.Name = path + identityColumn.Name;
                                        }
                                    }
                                }
                            }
                       
                            if (Indexer && Multiplicity.IsMany && Navigable)
                            {
                                if (GetIndexerColumnFor(table) == null)
                                {
                                    MetaDataRepository.Primitive systemInt32Type = (Namespace.ImplementationUnit.Context as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(typeof(int))) as MetaDataRepository.Primitive;
                                    if (systemInt32Type == null || systemInt32Type.Name == null)
                                    {
                               
                                        MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                                        systemInt32Type = new OOAdvantech.RDBMSMetaDataRepository.Primitive();
                                        PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(systemInt32Type);
                                        systemInt32Type.Synchronize(mPrimitive);
                                    }
                                    Column indexerColumn = new Column(Name + "_" + path + "Indexer", systemInt32Type);
                                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                                    _IndexerColumns.Add(indexerColumn);
                                    table.AddColumn(indexerColumn);
                                }
                            }
                        }
                        else
                            throw new System.Exception("The table '" + table.Name + "' hasn't table creator");

                        StateTransition.Consistent = true;
                    }
                    return columns;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            finally
            {
                if (pathIdentity.Count > 1) //the owner isn't valueType
                    pathIdentity.Pop();
            }

        }



        /// <summary>This method returns the ssociation end mapped columns  of table, 
        /// which is one from the mapped tables of storage cell parameter. </summary>
        /// <param name="storageCell">Define the storage cell which has a mapped table 
        /// which may be has the association end mapped columns. </param>
        /// <MetaDataID>{E3F5B97F-F821-44D4-BF03-4490CB253E27}</MetaDataID>
        public Collections.Generic.Set<IdentityColumn> GetReferenceColumnsFor(MetaDataRepository.StorageCell storageCell)
        {

            Collections.Generic.Set<IdentityColumn> columns = null;//GetColumns(storageCell.Type);
            //if(columns!=null&&columns.Count>0)
            {
                columns = GetReferenceColumnsFor((storageCell as StorageCell).MainTable);
                if (columns == null || columns.Count == 0)
                {
                    foreach (Table table in (storageCell as StorageCell).MappedTables)
                    {
                        columns = GetReferenceColumnsFor(table);
                        if (columns != null && columns.Count > 0)
                            break;
                    }
                }
            }
            return columns;
        }
        /// <MetaDataID>{9dfadfc3-4ca7-42fe-be39-1260d6f06784}</MetaDataID>
        Dictionary<MetaDataRepository.StorageCell, Dictionary<string, List<IdentityColumn>>> ReferenceColumns;

        /// <MetaDataID>{eec568ab-a193-4aae-b065-a5b818c85ab8}</MetaDataID>
        Dictionary<MetaDataRepository.StorageCell, Dictionary<string, Column>> PathIndexerColumns;


        /// <summary>This method returns the ssociation end mapped columns  of table, 
        /// which is one from the mapped tables of storage cell parameter. </summary>
        /// <param name="storageCell">Define the storage cell which has a mapped table 
        /// which may be has the association end mapped columns. </param>
        /// <MetaDataID>{E3F5B97F-F821-44D4-BF03-4490CB253E27}</MetaDataID>
        public System.Collections.Generic.List<IdentityColumn> GetReferenceColumnsFor(MetaDataRepository.StorageCell storageCell, string valueTypePath)
        {
            if (storageCell == null)
            {

            }
            if (valueTypePath == null)
                valueTypePath = "";

            List<IdentityColumn> columns = null;
            if (ReferenceColumns == null || !ReferenceColumns.ContainsKey(storageCell) || !ReferenceColumns[storageCell].ContainsKey(valueTypePath))
            {
                lock (this)
                {
                    if (ReferenceColumns == null)
                        ReferenceColumns = new Dictionary<MetaDataRepository.StorageCell, Dictionary<string, List<IdentityColumn>>>();
                }
                lock (ReferenceColumns)
                {
                    if (!ReferenceColumns.ContainsKey(storageCell))
                        ReferenceColumns[storageCell] = new Dictionary<string, List<IdentityColumn>>();





                    columns = new List<IdentityColumn>();

                    if (storageCell is StorageCellReference)
                    {
                        ReferenceColumns[storageCell][valueTypePath] = columns;
                        return columns;
                    }

                    Collections.Generic.Set<IdentityColumn> referenceColumns = GetReferenceColumnsFor((storageCell as StorageCell).MainTable);
                    foreach (IdentityColumn column in referenceColumns)
                    {
                        if (!string.IsNullOrEmpty(valueTypePath))
                        {

                            if (column.CreatorIdentity == valueTypePath + ".(" + Identity.ToString() + ")")
                                columns.Add(column);
                        }
                        else
                            columns.Add(column);
                    }


                    if (referenceColumns != null && referenceColumns.Count > 0)
                    {
                        if (columns.Count > 0)
                        {

                        }
                        foreach (Table table in (storageCell as StorageCell).MappedTables.Where(x => x != (storageCell as StorageCell).MainTable))
                        {
                            referenceColumns = GetReferenceColumnsFor(table);
                            foreach (IdentityColumn column in referenceColumns)
                            {
                                if (!string.IsNullOrEmpty(valueTypePath))
                                {

                                    if (column.CreatorIdentity == valueTypePath + ".(" + Identity.ToString() + ")" && !columns.Contains(column))
                                        columns.Add(column);
                                }
                                else
                                    columns.Add(column);

                            }
                            if (referenceColumns != null && referenceColumns.Count > 0)
                                break;
                        }
                    }

                    ReferenceColumns[storageCell][valueTypePath] = columns;
                    return columns;
                }
            }
            else
                return ReferenceColumns[storageCell][valueTypePath];
        }



        ///<summary>
        ///Return the indexer column for table
        ///</summary>
        ///<param name="table">
        ///Defines table which has the indexer column. 
        ///</param>
        /// <MetaDataID>{b579edcc-c1b9-41d1-afeb-7c4770cd034a}</MetaDataID>
        public Column GetIndexerColumnFor(Table table)
        {
            return GetIndexerColumnFor(table, "");
        }


        ///<summary>
        ///Return the indexer column for table
        ///</summary>
        ///<param name="table">
        ///Defines table which has the indexer column. 
        ///</param>
        ///<param name="valueTypePath">
        ///Defines the value type path of attribute with type the structure which contains the AssociationEnd 
        ///</param>
        /// <MetaDataID>{f61c2dfe-9986-463c-a839-d6955cdae598}</MetaDataID>
        public Column GetIndexerColumnFor(Table table, string valueTypePath)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                string creatorIdentity = null;
                if (!string.IsNullOrEmpty(valueTypePath))
                    creatorIdentity = valueTypePath + ".(" + Identity.ToString() + ")";
                else
                    creatorIdentity = "";
                foreach (Column column in _IndexerColumns)
                {
                    if (column.Namespace == table && column.CreatorIdentity == creatorIdentity)
                        return column;
                }
                return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        ///<summary>
        ///Return the indexer column from storage cell tables
        ///</summary>
        ///<param name="storageCell">
        ///Defines the storagecell as owner of table which has the indexer column. 
        ///</param>
        /// <MetaDataID>{fa112c60-f1b0-4023-b30a-953a5872905a}</MetaDataID>
        public Column GetIndexerColumnFor(MetaDataRepository.StorageCell storageCell)
        {
            return GetIndexerColumnFor(storageCell, "");
        }




        ///<summary>
        ///Return the indexer column from storage cell tables
        ///</summary>
        ///<param name="storageCell">
        ///Defines the storagecell as owner of table which has the indexer column. 
        ///</param>
        ///<param name="valueTypePath">
        ///Defines the value type path of attribute with type the structure which contains the AssociationEnd 
        ///</param>
        /// <MetaDataID>{a0fe28b6-7086-456a-a848-091916c023ac}</MetaDataID>
        public Column GetIndexerColumnFor(MetaDataRepository.StorageCell storageCell, string valueTypePath)
        {


            if (PathIndexerColumns == null || !PathIndexerColumns.ContainsKey(storageCell) || !PathIndexerColumns[storageCell].ContainsKey(valueTypePath))
            {
                if (PathIndexerColumns == null)
                    PathIndexerColumns = new Dictionary<MetaDataRepository.StorageCell, Dictionary<string, Column>>();

                if (!PathIndexerColumns.ContainsKey(storageCell))
                    PathIndexerColumns[storageCell] = new Dictionary<string, Column>();

                Column indexerColumn = GetIndexerColumnFor((storageCell as StorageCell).MainTable, valueTypePath);
                if (indexerColumn != null)
                {
                    if (!PathIndexerColumns.ContainsKey(storageCell))
                        PathIndexerColumns[storageCell] = new Dictionary<string, Column>();

                    PathIndexerColumns[storageCell][valueTypePath] = indexerColumn;
                    return indexerColumn;
                }
                foreach (Table table in (storageCell as StorageCell).MappedTables)
                {
                    indexerColumn = GetIndexerColumnFor(table, valueTypePath);
                    if (indexerColumn != null)
                    {
                        if (!PathIndexerColumns.ContainsKey(storageCell))
                            PathIndexerColumns[storageCell] = new Dictionary<string, Column>();

                        PathIndexerColumns[storageCell][valueTypePath] = indexerColumn;
                        return indexerColumn;
                    }
                }
                if (!PathIndexerColumns.ContainsKey(storageCell))
                    PathIndexerColumns[storageCell] = new Dictionary<string, Column>();
                PathIndexerColumns[storageCell][valueTypePath] = null;
                return null;
            }
            else
                return PathIndexerColumns[storageCell][valueTypePath];
        }


        public Collections.Generic.Set<IdentityColumn> GetReferenceColumnsFor(Table table)
        {
            return GetReferenceColumnsFor(table, "");
        }

        /// <summary>This method returns the mapped columns of association end, 
        /// which belongs to the table of parameter. </summary>
        /// <param name="table">Define the table for which we want the mapped columns. </param>
        /// <MetaDataID>{39B4EED6-2689-40CD-A4BE-83A016885C06}</MetaDataID>
        public Collections.Generic.Set<IdentityColumn> GetReferenceColumnsFor(Table table, string valueTypePath)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                Collections.Generic.Set<IdentityColumn> columns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>();
                foreach (Column column in _MappedColumns)
                {
                    if (column.Namespace == table && column is IdentityColumn)
                    {
                        if (!string.IsNullOrEmpty(valueTypePath))
                        {

                            if (column.CreatorIdentity == valueTypePath + ".(" + Identity.ToString() + ")")
                                columns.Add(column as IdentityColumn);
                        }
                        else
                            columns.Add(column as IdentityColumn);
                    }
                }
                foreach (var column in table.ContainedColumns.OfType<IdentityColumn>())
                {
                    if (column.MappedAssociationEnd == this)
                    {
                        if (!_MappedColumns.Contains(column))
                            _MappedColumns.Add(column);

                        if (!columns.Contains(column))
                            columns.Add(column);

                    }

                }
                return columns;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }



        }


        /// <MetaDataID>{e0ce9935-0a29-44da-a004-b63e49bcae71}</MetaDataID>
        public ObjectIdentityType GetReferenceObjectIdentityType(Classifier classifier)
        {
            MetaDataRepository.ObjectIdentityType objectIdentityType = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(classifier);
            List<ObjectIdentityType> referenceObjectIdentityTypes = GetReferenceObjectIdentityTypes(classifier, new List<ObjectIdentityType>() { objectIdentityType });
            if (referenceObjectIdentityTypes.Count == 0)
                return null;
            else
                return referenceObjectIdentityTypes[0];
        }



        /// <summary>This method returns the columns for association end. </summary>
        /// <remarks>
        /// This method is useful for the many to many relationships because 
        /// returns the columns of association table.
        /// Usually used for the build of pseudo table in SQL statement for 
        /// OQL query on abstract class or interface without concrete object 
        /// of this type in object storage.
        /// </remarks>
        /// <MetaDataID>{14573E6C-CEE5-4BFD-8110-0552C01FDD6E}</MetaDataID>
        public MetaDataRepository.ObjectIdentityType GetReferenceObjectIdentityType()
        {
            return AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityRefernce(this, this, this.Association.CaseInsensitiveName);
        }

        /// <MetaDataID>{14614a83-a794-48e2-b4ac-856b16626153}</MetaDataID>
        public Collections.Generic.List<ObjectIdentityType> GetReferenceObjectIdentityTypes(Classifier classifier, List<ObjectIdentityType> objectIdentityTypes)
        {
            if (!HasClassifierDataOIDReferenceColums(classifier))
                return new OOAdvantech.Collections.Generic.List<ObjectIdentityType>();
            else
            {
                OOAdvantech.Collections.Generic.List<ObjectIdentityType> referenceObjectIdentityTypes = new Collections.Generic.List<ObjectIdentityType>();
                foreach (var objectIdentityType in objectIdentityTypes)
                    referenceObjectIdentityTypes.Add(GetReferenceObjectIdentityType(classifier, classifier.GetClassHerarchyCaseInsensitiveUniqueNames(this.Association), objectIdentityType));
                return referenceObjectIdentityTypes;
            }
        }

        /// <MetaDataID>{fe434e70-2324-4d67-90d6-b69c131b240f}</MetaDataID>
        private ObjectIdentityType GetReferenceObjectIdentityType(Classifier classifier, string caseInsensitiveAssotiationName, ObjectIdentityType objectIdentityType)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (!HasClassifierDataOIDReferenceColums(classifier))
                    return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                System.Collections.Generic.List<IIdentityPart> parts = new List<IIdentityPart>();
                foreach (IIdentityPart part in objectIdentityType.Parts)
                {
                    {
                        if (IsRoleA)
                            parts.Add(new IdentityPart(caseInsensitiveAssotiationName + "_" + part.Name + "B", part.PartTypeName, part.Type));
                        else
                            parts.Add(new IdentityPart(caseInsensitiveAssotiationName + "_" + part.Name + "A", part.PartTypeName, part.Type));
                    }
                }
                return new ObjectIdentityType(parts);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{5403845d-62ea-44ee-9082-b8a6c319ac38}</MetaDataID>
        private bool HasClassifierDataOIDReferenceColums(Classifier classifier)
        {
            if (Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && Association.LinkClass == null)
                return false; // no coulumns
            if (Association.LinkClass != null && Association != classifier.ClassHierarchyLinkAssociation)
                return false; // no coulumns
            if (Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne)
            {
                if (!IsRoleA)
                    return false; // no coulumns
            }
            else
            {
                if (!_Multiplicity.IsMany)
                    return false; // no coulumns
            }
            return true;
        }




        ///// <summary>This method returns the columns for the classifier. </summary>
        ///// <param name="classifier">Define the classifier for which the association end, 
        ///// will decide if build columns or not. </param>
        ///// <remarks>
        ///// If the classifier is link class of association then the association end returns columns always. 
        ///// In case where the classifier is association end specification classifier, 
        ///// the columns which returns the association end is up to the multiplicity 
        ///// of association end.
        ///// </remarks>
        ///// <MetaDataID>{9D69040C-6F52-4C63-AC21-3FDC22441DFC}</MetaDataID>
        //public MetaDataRepository.ObjectIdentityType GetReferenceColumns(MetaDataRepository.Classifier classifier, string caseInsensitiveAssotiationName)
        //{
        //  //  Collections.Generic.Set<IdentityColumn> columns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>();
        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {
        //        if (Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && Association.LinkClass == null)
        //            return null; // no coulumns
        //        if (Association.LinkClass != null && Association != classifier.ClassHierarchyLinkAssociation)
        //            return null; // no coulumns
        //        if (Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne)
        //        {
        //            if (!IsRoleA)
        //                return null; // no coulumns
        //        }
        //        else
        //        {
        //            if (!_Multiplicity.IsMany)
        //                return null; // no coulumns
        //        }
        //    }
        //    finally 
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //    try
        //    {
        //        return AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityRefernce(this, this, caseInsensitiveAssotiationName);

        //    }
        //    finally
        //    {
        //        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //    }
        //}





        /// <MetaDataID>{539bd840-2240-46fd-8c7d-2a74e1272e69}</MetaDataID>
        public Column GetIndexerColumn(MetaDataRepository.Classifier classifier, string path)
        {
            if (!Indexer)
                return null;


            if (Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && Association.LinkClass == null)
                return null; // no coulumns
            if (Association.LinkClass != null && Association != classifier.ClassHierarchyLinkAssociation)
                return null; // no coulumns
            if (Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne)
            {
                if (!IsRoleA)
                    return null; // no coulumns
            }
            else
            {
                if (!_Multiplicity.IsMany)
                    return null; // no coulumns
            }

            return new Column(Name+"_"+ path + "Indexer", Classifier.GetClassifier(typeof(int)));
        }



        /// <MetaDataID>{7d5b4fc8-b43e-496d-be69-b7fa979fe51b}</MetaDataID>
        Column _IndexerColumn;
        /// <MetaDataID>{15bffd0e-3fe7-4873-b77b-a6c17dcc5d74}</MetaDataID>
        public Column IndexerColumn
        {
            get
            {
                if (Indexer)
                {
                    if (_IndexerColumn != null)
                        return _IndexerColumn;



                    Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(this.Properties).Execute("SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject WHERE MetaObject.MetaObjectIDStream = \'" + Classifier.GetClassifier(typeof(int)).Identity.ToString() + "\' ");//WHERE MetaObjectIDStream = \""+OriginMetaObject.Identity.ToString()+"\" ");

                    OOAdvantech.MetaDataRepository.Classifier columnType = null;
                    foreach (Collections.StructureSet Rowset in aStructureSet)
                    {
                        columnType = Rowset["MetaObject"] as Classifier;
                        break;
                    }
                    if (columnType == null)
                    {
                        if (IndexerColumns.Count > 0)
                            columnType = IndexerColumns[0].Type;
                        else
                        {
                            System.Diagnostics.Debug.Assert(false);
                            columnType = new Primitive();
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(this.Properties).CommitTransientObjectState(columnType);
                            columnType.Synchronize(Classifier.GetClassifier(typeof(int)));
                        }
                    }


                    _IndexerColumn = new Column(CaseInsensitiveName + "_Indexer", columnType);

                    return _IndexerColumn;
                }
                else
                    return null;

            }
        }
        /// <MetaDataID>{76afc02e-6ae5-48d7-9dd5-c0e7a55c333a}</MetaDataID>
        public string GetColumnName(IIdentityPart identityPart)
        {
            if (IsRoleA)
                return Association.CaseInsensitiveName + "_" + identityPart.PartTypeName + "B";
            else
                return Association.CaseInsensitiveName + "_" + identityPart.PartTypeName + "A";
        }






        /// <MetaDataID>{d55cf283-aa29-4dca-86d5-f467277e9372}</MetaDataID>
        public void AddReferenceColumn(IdentityColumn refIdentityColumn)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MappedColumns.Add(refIdentityColumn);
                stateTransition.Consistent = true;
            }

        }


        public void AddIndexerColumn(Column indexerColumn)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _IndexerColumns.Add(indexerColumn);

                if (PathIndexerColumns != null)
                    PathIndexerColumns.Clear();
                stateTransition.Consistent = true;
            }

        }

    }
}
