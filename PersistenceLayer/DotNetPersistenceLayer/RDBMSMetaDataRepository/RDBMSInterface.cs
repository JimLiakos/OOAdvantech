using System.Linq;
namespace OOAdvantech.RDBMSMetaDataRepository
{
    using MetaDataRepository;
    /// <MetaDataID>{AB25D958-A397-4F27-89B2-57E30B93D09D}</MetaDataID>
    [BackwardCompatibilityID("{AB25D958-A397-4F27-89B2-57E30B93D09D}")]
    [Persistent()]
    public class Interface : MetaDataRepository.Interface, MappedClassifier
    {
        public override void Synchronize(MetaObject originMetaObject)
        {
            if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                return;

            Classifier OriginClassifier = (Classifier)originMetaObject;

            if (OriginClassifier.IsTemplate)
            {
                if (_Name != OriginClassifier.Name)
                {
                    _Name = OriginClassifier.Name;
                    _CaseInsensitiveName = null;
                }


                if (_Namespace == null && PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties) != null)
                    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Namespace", typeof(MetaDataRepository.MetaObject));
                if (_Namespace.Value == null && OriginClassifier.Namespace != null)
                {
                    _Namespace.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originMetaObject.Namespace, this) as MetaDataRepository.Namespace;


                    if (_Namespace.Value == null)
                    {
                        _Namespace.Value = (Namespace)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.Namespace, this);
                        if (_Namespace.Value != null)
                            _Namespace.Value.ShallowSynchronize(OriginClassifier.Namespace);
                    }
                    if (_Namespace.Value != null)
                        _Namespace.Value.AddOwnedElement(this);
                }



                if (_ImplementationUnit.Value == null && OriginClassifier.ImplementationUnit != null)
                {
                    _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originMetaObject.ImplementationUnit, this) as MetaDataRepository.Component;
                    if (_ImplementationUnit.Value == null)
                        _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.ImplementationUnit, this);
                }
                return;
            }
            base.Synchronize(originMetaObject);
        }
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                var implementationUnit= base.ImplementationUnit;
                if(implementationUnit!=null&& implementationUnit.Context==null)
                {
                    if (PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                    {
                        OOAdvantech.Linq.Storage theStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                        var storage = (from metastorage in theStorage.GetObjectCollection<Storage>() select metastorage).FirstOrDefault();
                        implementationUnit.Context = storage;
                      
                    }
                }
                return implementationUnit;
            }
        }
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {

            if (member.Name == nameof(_TypeView))
            {
                if (value == null)
                    _TypeView = default(OOAdvantech.RDBMSMetaDataRepository.View);
                else
                    _TypeView = (OOAdvantech.RDBMSMetaDataRepository.View)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectIdentityTypes))
            {
                if (value == null)
                    _ObjectIdentityTypes = default(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>);
                else
                    _ObjectIdentityTypes = (System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_TypeView))
                return _TypeView;

            if (member.Name == nameof(_ObjectIdentityTypes))
                return _ObjectIdentityTypes;


            return base.GetMemberValue(token, member);
        }

        /// <summary>Define a collection with the storage cells 
        /// which the type of storage cell is subtype of classifier. </summary>
        /// <MetaDataID>{91FC608E-939E-4482-98EA-2EEF5E113409}</MetaDataID>
        public Collections.Generic.Set<MetaDataRepository.StorageCell> ClassifierLocalStorageCells
        {
            get
            {
                Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new Collections.Generic.Set<MetaDataRepository.StorageCell>();
                foreach (MetaDataRepository.Realization realization in Realizations)
                {
                    if (realization.Implementor is MetaDataRepository.Class && (realization.Implementor as MetaDataRepository.Class).Persistent)
                    {
                        foreach (StorageCell storageCell in (realization.Implementor as MappedClassifier).ClassifierLocalStorageCells)
                            storageCells.Add(storageCell);
                    }
                }
                return storageCells;
            }
        }
        /// <MetaDataID>{49629521-af3e-4139-b175-bd54971224cd}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> GetObjectIdentityTypes(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells)
        {
            System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
            foreach (StorageCell storageCell in (this as MappedClassifier).ClassifierLocalStorageCells)
            {
                MetaDataRepository.ObjectIdentityType objectIdentityType = storageCell.ObjectIdentityType;
                if (!objectIdentityTypes.Contains(objectIdentityType))
                    objectIdentityTypes.Add(objectIdentityType);
            }
            return objectIdentityTypes;
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{69C48649-65F0-4826-A04D-80660615D4EC}</MetaDataID>
        private View _TypeView;
        /// <MetaDataID>{855BD248-927E-4422-BFF8-EE64BF65E2A1}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember("_TypeView")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.LazyFetching)]
        public View TypeView
        {
            get
            {
                return null;
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_TypeView == null)
                    {
                        if (ImplementationUnit.Context == null)
                            return null;
                        


                        _TypeView = new View( CaseInsensitiveName, ImplementationUnit.Context);
                        if (Name == null)
                            throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_TypeView);
                    }
                    return _TypeView;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{50EC2681-FA8E-4155-AC2D-3951A8AFB81F}</MetaDataID>
        private System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> _ObjectIdentityTypes;
        /// <MetaDataID>{CC25FF6C-AD36-4FAA-BA33-F264F73677BD}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get
            {
                //TODO Θα πρέπει να διασφαλιστή ότι όλη class ιεραρχία έχει κοινό ObjectID Format
                if (_ObjectIdentityTypes == null)
                {
                    Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = ClassifierLocalStorageCells;
                    if (storageCells.Count > 0)
                    {
                        foreach (StorageCell storageCell in storageCells)
                        {
                            if (_ObjectIdentityTypes == null)
                                _ObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();

                            if (!_ObjectIdentityTypes.Contains(storageCell.ObjectIdentityType))
                                _ObjectIdentityTypes.Add(storageCell.ObjectIdentityType);
                        }
                    }
                    else
                    {
                        _ObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
                        _ObjectIdentityTypes.Add(AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this));
                    }
                }


                return _ObjectIdentityTypes;
            }
        }
        //public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> ObjectIDColumns
        //{
        //    get
        //    {
        //        //TODO Θα πρέπει να διασφαλιστή ότι όλη class ιεραρχία έχει κοινό ObjectID Format
        //        if (_ObjectIDColumns == null)
        //        {
        //            if (HasPersistentObjects)
        //            {
        //                 _ObjectIDColumns = new System.Collections.Generic.List<ObjectIdentityType>();
        //                foreach(StorageCell storageCell in (this as MappedClassifier).LocalStorageCells)
        //                {
                            
        //                    if(!_ObjectIDColumns.Contains(storageCell.ObjectIdentityType))
        //                        _ObjectIDColumns.Add(storageCell.ObjectIdentityType);
        //                }
        //            }
        //            else
        //            {
        //                _ObjectIDColumns = new System.Collections.Generic.List<ObjectIdentityType>();
        //                System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
        //                foreach (IdentityColumn column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(this))
        //                    parts.Add(column);
        //                MetaDataRepository.ObjectIdentityType oType=new ObjectIdentityType(parts);
        //                //foreach (Column column in TypeView.ViewColumns)
        //                //{

        //                //    foreach ( Column identityColumn in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(this))
        //                //        if (identityColumn.Name == column.Name)
        //                //            _ObjectIDColumns.Add(column as IdentityColumn);
        //                //}
        //            }
        //        }
        //        return _ObjectIDColumns;
        //    }
        //}
        /// <MetaDataID>{8C1CCBAB-A2ED-4359-A13B-700CDCC77C8C}</MetaDataID>
        public StorageCell GetStorageCell(object ObjectID)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {

                foreach (StorageCell CurrStorageCell in ClassifierLocalStorageCells)
                {

                    if (PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(CurrStorageCell.Properties).PersistentObjectID.Equals(ObjectID))
                        return CurrStorageCell;
                }
                throw new System.Exception("There is not StorageCell with ObjectID '" + ObjectID.ToString() + "'");
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }




        /// <MetaDataID>{7C1E62D6-9090-406C-801E-8CFE1DBB3147}</MetaDataID>
        public void BuildMappingElement()
        {
            return;
            //TODO οι objectID colums έχουν αν αλλάξη κάτι στην ιεραρχία
            bool hasPersistentObjects = HasPersistentObjects;


            #region Create abstract class view if there isn't
            if (_TypeView == null)
            {
                _TypeView = new View( CaseInsensitiveName, ImplementationUnit.Context as MetaDataRepository.Namespace);
                if (Name == null)
                    throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_TypeView);
            }
            else
            {
                _TypeView.Name =  Name;
                if (_TypeView.Namespace == null)
                    (ImplementationUnit.Context as Namespace).AddOwnedElement(_TypeView);
            }
            

            System.Collections.Generic.Dictionary<string, Column> deleteColumns = new System.Collections.Generic.Dictionary<string, Column>();

            foreach (Column viewColumn in TypeView.ViewColumns)
            {
                //if(viewColumn.RealColumn!=null)
                //    deleteColumns.Add(viewColumn.RealColumn.Name, viewColumn);
                //else
                    deleteColumns.Add(viewColumn.Name, viewColumn);
            }
            #endregion

            if (hasPersistentObjects)
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                {
                    foreach (IdentityColumn column in objectIdentityType.Parts)
                    {
                        //Column newColumnAlias = new Column(column);
                        //PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(newColumnAlias);
                        TypeView.AddColumn(column);
                        ((MetaDataRepository.Namespace)ImplementationUnit.Context).AddOwnedElement(TypeView);
                        foreach (System.Collections.Generic.KeyValuePair<string, Column> entry in deleteColumns)
                        {
                            if (entry.Value.RealColumn is IdentityColumn &&
                                (entry.Value.RealColumn as IdentityColumn).ColumnType == column.ColumnType &&
                                (entry.Value.RealColumn as IdentityColumn).Type.FullName == column.Type.FullName)
                            {
                                deleteColumns.Remove(entry.Key);
                                break;
                            }

                        }
                        if (deleteColumns.ContainsKey(column.Name))
                            deleteColumns.Remove(column.Name);
                    }
                }
            }
            else
            {
                foreach (Column column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this).Parts)
                {
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                    TypeView.AddColumn(column);
                    if (deleteColumns.ContainsKey(column.Name))
                        deleteColumns.Remove(column.Name);
                }
            }


            foreach (Column column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this))
            {
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                TypeView.AddColumn(column);
                if (deleteColumns.ContainsKey(column.Name))
                    deleteColumns.Remove(column.Name);

            }

            foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in GetAssociateRoles(true))
            {
                //if(!CurrAssociationEnd.Association.HasPersistentObjectLink)
                //	continue;
                System.Collections.Generic.List < MetaDataRepository.ObjectIdentityType > objectIdentityTypes =new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>() { AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this) };
                objectIdentityTypes = ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceObjectIdentityTypes(this, objectIdentityTypes);
                foreach (ObjectIdentityType objectIdentityType in objectIdentityTypes)
                {
                    foreach (Column column in objectIdentityType.Parts)
                    {
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                        TypeView.AddColumn(column);
                        if (deleteColumns.ContainsKey(column.Name))
                            deleteColumns.Remove(column.Name);
                    }
                }

                if (((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn != null && objectIdentityTypes.Count > 0)
                {

                    Column indexerColumn = new Column(((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn.Name, ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn.Type);
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn.Type);
                    TypeView.AddColumn(indexerColumn);
                    if (deleteColumns.ContainsKey(indexerColumn.Name))
                        deleteColumns.Remove(indexerColumn.Name);
                }


            }
            foreach (MetaDataRepository.Attribute attribute in GetAttributes(true))
            {
                foreach (Column column in GetColumnsForAttribute(attribute, ""))
                {
                    if (deleteColumns.ContainsKey(column.Name))
                        deleteColumns.Remove(column.Name);
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                    TypeView.AddColumn(column);
                    ((MetaDataRepository.Namespace)ImplementationUnit.Context).AddOwnedElement(TypeView);
                }
            }
            if (ClassHierarchyLinkAssociation != null)
            {
                System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>() { AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this) };

                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).GetReferenceObjectIdentityTypes(this, objectIdentityTypes))
                {
                    foreach (Column column in objectIdentityType.Parts)
                    {
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                        TypeView.AddColumn(column);
                        if (deleteColumns.ContainsKey(column.Name))
                            deleteColumns.Remove(column.Name);


                    }
                }

                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).GetReferenceObjectIdentityTypes(this, objectIdentityTypes))
                {
                    foreach (Column column in objectIdentityType.Parts)
                    {
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                        TypeView.AddColumn(column);
                        if (deleteColumns.ContainsKey(column.Name))
                            deleteColumns.Remove(column.Name);
                    }
                }
            }
            foreach (Column column in deleteColumns.Values)
                TypeView.RemoveColumn(column);

        }

        #region MappedClassifier Members

        /// <MetaDataID>{8cd2381d-f215-433e-9e68-ac5c64b8521a}</MetaDataID>
        System.Collections.Generic.List<Column> GetColumnsForAttribute(MetaDataRepository.Attribute attribute, string path)
        {
            
            if (!string.IsNullOrEmpty(path))
                path += "_";

            System.Collections.Generic.List<Column> columns = new System.Collections.Generic.List<Column>();
            MetaDataRepository.ObjectIdentityType objectIdentityType = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this);

            if (attribute.Type is MetaDataRepository.Primitive ||
                attribute.Type.FullName.Trim() == typeof(string).FullName ||
                attribute.Type.FullName.Trim() == typeof(System.DateTime).FullName ||
                attribute.Type.FullName.Trim() == typeof(System.Guid).FullName)
            {
                Column newColumnAlias = new Column();
                newColumnAlias.Name = path + attribute.CaseInsensitiveName;
                newColumnAlias.Type = attribute.Type;
                columns.Add(newColumnAlias);
            }
            else if (attribute.IsPersistentValueType)
            {
                foreach (MetaDataRepository.Attribute valueTypeAttribute in attribute.Type.GetAttributes(true))
                    columns.AddRange(GetColumnsForAttribute(valueTypeAttribute, path + attribute.Name));
                
                foreach (MetaDataRepository.AssociationEnd associationEnd in attribute.Type.GetAssociateRoles(true))
                {
                    foreach (Column column in ((AssociationEnd)associationEnd.GetOtherEnd()).GetReferenceObjectIdentityTypes(this,new System.Collections.Generic.List<ObjectIdentityType>(){objectIdentityType})[0].Parts)
                    {
                        column.Name = path + attribute.Name + "_" + column.Name;
                        columns.Add(column);
                    }
                    if (((AssociationEnd)associationEnd.GetOtherEnd()).IndexerColumn != null && ((AssociationEnd)associationEnd.GetOtherEnd()).GetReferenceObjectIdentityTypes(this, new System.Collections.Generic.List<ObjectIdentityType>() { objectIdentityType }).Count>0)
                        columns.Add(new Column(path + attribute.Name + "_" + ((AssociationEnd)associationEnd.GetOtherEnd()).IndexerColumn.Name, ((AssociationEnd)associationEnd.GetOtherEnd()).IndexerColumn.Type));
                }

            }

            return columns;
        }



        /// <MetaDataID>{F6F3B65E-72CC-4129-ABB5-BEEA31A8F2E9}</MetaDataID>
        public Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(System.DateTime TimePeriodStartDate, System.DateTime TimePeriodEndDate)
        {
            return new Collections.Generic.Set<MetaDataRepository.StorageCell>();
        }

        /// <MetaDataID>{A53FAE7E-BED7-49B6-906F-115883C5F1A6}</MetaDataID>
        public bool HasPersistentObjects
        {
            get
            {
                return GetPersistentSubClasses(this).Count > 0;
            }
        }
        /// <MetaDataID>{DE5C9EC9-17A9-426C-AD03-F329BF953940}</MetaDataID>
        private Collections.Generic.Set<MetaDataRepository.Class> GetPersistentSubClasses(Interface _interface)
        {
            Collections.Generic.Set <MetaDataRepository.Class>  persistentClasses = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Class>();
            foreach (Generalization generalizationRelation in _interface.Specializations)
            {
                Interface subInterface = generalizationRelation.Child as Interface;
                persistentClasses.AddRange(GetPersistentSubClasses(subInterface));
            }

            foreach (MetaDataRepository.Realization realization in Realizations)
            {
                Class implementationClass = realization.Implementor as Class;


                if (implementationClass != null)
                {
                    if (implementationClass.Persistent)
                        persistentClasses.Add(implementationClass);
                    persistentClasses.AddRange(GetPersistentSubClasses(implementationClass));
                }
            }
            return persistentClasses;

        }

        /// <MetaDataID>{4C7A1429-235A-4C82-9518-C4495D0C0EAC}</MetaDataID>
        private Collections.Generic.Set<MetaDataRepository.Class> GetPersistentSubClasses(Class _class)
        {
            Collections.Generic.Set <MetaDataRepository.Class> persistentClasses = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Class>();
            foreach (Generalization generalizationRelation in _class.Specializations)
            {
                Class subClass = generalizationRelation.Child as Class;
                if (subClass.Persistent)
                    persistentClasses.Add(subClass);
                persistentClasses.AddRange(GetPersistentSubClasses(subClass));
            }
            return persistentClasses;
        }


        ///// <MetaDataID>{C882BFEB-1FC8-4212-9A7C-4D0108E32647}</MetaDataID>
        //public View GetTypeView(Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells)
        //{

        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {
        //        #region Precondition check
        //        if (storageCells == null)
        //            throw new System.ArgumentException("The parameter 'storageCells' must be not null or empty");
        //        foreach (StorageCell storageCell in storageCells)
        //        {
        //            if (!storageCell.Type.IsA(this))
        //                throw new System.ArgumentException("Storage Cell '" + storageCell.Name + "' type isn't subtype of +'" + FullName + "'.");
        //        }
        //        #endregion

              
        //        //TODO σε περιπτωση που υπαρχεί out storage collection και δεν υπάρχει επικοινωνεία με
        //        //τον άλλο server θα πρέπει να εγειρεται κατάλληλη exception
        //        View view = new View("Temp_Abstract_" + Name);
        //        foreach (Column CurrColumn in TypeView.ViewColumns)
        //            view.AddColumn(CurrColumn);
        //        foreach (MetaDataRepository.StorageCell CurrStorageCell in storageCells)
        //        {
        //            if (CurrStorageCell is StorageCellReference)
        //            {
        //                StorageCellReference OutStorageCell = CurrStorageCell as StorageCellReference;
        //                view.AddSubView(OutStorageCell.ConcreteClassView);
        //                if (view.ViewStorageCell == null)
        //                    view.ViewStorageCell = new OOAdvantech.Collections.Generic.Dictionary<View, StorageCell>();
        //                view.ViewStorageCell.Add(OutStorageCell.ConcreteClassView, CurrStorageCell);

        //            }
        //            else
        //            {
        //                view.AddSubView(CurrStorageCell.ClassView);
        //                if (view.ViewStorageCell == null)
        //                    view.ViewStorageCell = new OOAdvantech.Collections.Generic.Dictionary<View, StorageCell>();
        //                view.ViewStorageCell.Add(CurrStorageCell.ClassView, CurrStorageCell);

        //            }
        //        }
        //        return view;
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //}


        #endregion
    }
}
