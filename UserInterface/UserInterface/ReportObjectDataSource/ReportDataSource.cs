using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using System.Reflection.Emit;
using System.Collections.Generic;


namespace OOAdvantech.UserInterface.ReportObjectDataSource
{
    /// <MetaDataID>{f0b6d695-fd9d-4582-b420-2e0d64c3aabf}</MetaDataID>
    [BackwardCompatibilityID("{f0b6d695-fd9d-4582-b420-2e0d64c3aabf}"), Persistent()]
    public class ReportDataSource : OOAdvantech.UserInterface.Component
    {
        /// <exclude>Excluded</exclude>
        AssociationEnd _AssociationEnd;
        /// <MetaDataID>{78b4d2e3-c80b-4a62-8908-b09ecb932110}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd
        {
            get
            {
                return _AssociationEnd;
            }
            set
            {
                _AssociationEnd = value;
                if (_AssociationEnd != null)
                {
                    _MetaObjectIdentity = _AssociationEnd.Identity.ToString();
                    IsCollection = _AssociationEnd.CollectionClassifier != null;
                }
            }
        }
        /// <MetaDataID>{8c4e952b-620f-4f90-b9c7-47f3109bfc66}</MetaDataID>
        Attribute _Attribute;
        /// <MetaDataID>{951df816-0531-422a-92cf-23b5f40a9cd0}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Attribute Attribute
        {
            get
            {
                return _Attribute;
            }
            set
            {
                _Attribute = value;
                IsCollection = false;
                if (_Attribute != null)
                {
                    _MetaObjectIdentity = _Attribute.Identity.ToString();

                    foreach (OOAdvantech.MetaDataRepository.Operation operation in _Attribute.Type.GetOperations("GetEnumerator"))
                    {
                        OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                        if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                        {
                            IsCollection = true;
                            
                            break;
                        }
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        string _MetaObjectIdentity;
        /// <MetaDataID>{efe167a5-fddf-4117-8c5d-80aa22ecd454}</MetaDataID>
        [PersistentMember("_MetaObjectIdentity")]
        [BackwardCompatibilityID("+4")]
        public string MetaObjectIdentity
        {
            get
            {
                return _MetaObjectIdentity;
            }
            set
            {
                if (_MetaObjectIdentity != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MetaObjectIdentity = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }

        public bool HasExtraMembers
        {
            get
            {
                foreach (Member member in this.DataSourceMembers)
                {
                    if (member.MetaObject == null)
                        return true;
                }
                foreach (ReportDataSource detailReportDataSource in this.DetailDataSources)
                {
                    if (detailReportDataSource.MetaObject == null)
                        return true;
                }
                return false;

            }
        }
        /// <MetaDataID>{3039948d-4713-4da8-8505-9eee0d0b1f13}</MetaDataID>
        bool OnEnsurePersistency;
        /// <MetaDataID>{45250885-78c2-4b88-9a44-ef74e158746a}</MetaDataID>
        public void EnsurePersistency()
        {



            if (OnEnsurePersistency)
                return;
            OnEnsurePersistency = true;


            try
            {

                if (!ObjectStorage.IsPersistent(this))
                {
                    RootReportDataSource.EnsurePersistency();
                    return;
                }


                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    foreach (Member member in this.DataSourceMembers)
                    {
                        if (!Report.CanBeTransient(member) || !string.IsNullOrEmpty(member.Path)||
                           member.MetaObject==null|| member.MetaObject.Name!=member.Name)
                        {
                            if (!ObjectStorage.IsPersistent(member))
                                ObjectStorage.GetStorageOfObject(RootReportDataSource).CommitTransientObjectState(member);
                            if (!_DataSourceMembers.Contains(member))
                            {
                                if (TransientMembers.ContainsKey(member.MetaObject))
                                    TransientMembers.Remove(member.MetaObject);

                                _DataSourceMembers.Add(member);
                            }


                            int tto = 0;
                        }
                        else
                        {
                            if (_DataSourceMembers.Contains(member))
                                _DataSourceMembers.Remove(member);
                            if (!TransientMembers.ContainsKey(member.MetaObject))
                                TransientMembers[member.MetaObject] = member;
                        }

                    }
                    foreach (ReportDataSource detailReportDataSource in this.DetailDataSources)
                    {
                        if (!Report.CanBeTransient(detailReportDataSource) ||
                            !string.IsNullOrEmpty(detailReportDataSource.Path) ||
                            detailReportDataSource.HasCustomSetings)
                        {

                            if (!ObjectStorage.IsPersistent(detailReportDataSource))
                                ObjectStorage.GetStorageOfObject(RootReportDataSource).CommitTransientObjectState(detailReportDataSource);
                            if (!_DetailDataSources.Contains(detailReportDataSource))
                                _DetailDataSources.Add(detailReportDataSource);

                            if (detailReportDataSource.MetaObject!=null&&TransientDetailDataSources.ContainsKey(detailReportDataSource.MetaObject))
                                TransientDetailDataSources.Remove(detailReportDataSource.MetaObject);
                            detailReportDataSource.EnsurePersistency();
                            int tto = 0;
                        }
                        else
                        {
                            if (_DetailDataSources.Contains(detailReportDataSource))
                                _DetailDataSources.Remove(detailReportDataSource);
                            if (!TransientDetailDataSources.ContainsKey(detailReportDataSource.MetaObject))
                                TransientDetailDataSources[detailReportDataSource.MetaObject] = detailReportDataSource;
                        }

                    }
                    stateTransition.Consistent = true;
                }

            }
            finally
            {

                OnEnsurePersistency = false;
            }

        }
        /// <exclude>Excluded</exclude>
        private bool _HasCustomSetings;
        /// <MetaDataID>{5cb359a0-5a92-4698-a8e8-2d200a8f5ce8}</MetaDataID>
        public bool HasCustomSetings
        {
            get
            {
                foreach (Member member in DataSourceMembers)
                {
                    if (member.MetaObject != null && member.Name != member.MetaObject.Name)
                        _HasCustomSetings = true;
                    if (!string.IsNullOrEmpty(member.Path))
                        _HasCustomSetings = true;
                }



                return _HasCustomSetings;
            }
            set
            {
                _HasCustomSetings = value;
                if (_HasCustomSetings && MasterDataSource != null)
                    MasterDataSource.HasCustomSetings = true;

            }
        }

        /// <exclude>Excluded</exclude>
        string _Path;
        /// <MetaDataID>{7b10c36f-8859-44bb-90e9-6f3102a29226}</MetaDataID>
        [PersistentMember("_Path"), BackwardCompatibilityID("+3")]
        public string Path
        {
            get
            {
                //object obj = Type;
                return _Path;

            }
            set
            {
                if (_Path != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Type = null;
                        _Path = value;
                        stateTransition.Consistent = true;
                    }
                    if (MasterDataSource != null)
                        MasterDataSource.CheckDataSourceTypeConsistency();


                }
            }
        }

        /// <exclude>Excluded</exclude>
        IReport _Report;
        /// <MetaDataID>{a26065c3-8e21-43d5-99ff-ce4059028c64}</MetaDataID>
        public IReport Report
        {
            get
            {
                if (MasterDataSource == null)
                    return _Report;
                else
                    return MasterDataSource.Report;
            }
            set
            {
                if (MasterDataSource == null)
                    _Report = value;
                else
                    MasterDataSource.Report = value;
            }
        }
        /// <MetaDataID>{ccf681ef-49b2-4ac2-94e9-b8887679c650}</MetaDataID>
        public ReportDataSource()
        {
            _Name = "ReportDataSource";
            IsCollection = true;
        }
        /// <MetaDataID>{ba6403ab-a436-4926-afad-f00657690322}</MetaDataID>
        int AutoIncrementTypeID = 0;
        /// <exclude>Excluded</exclude>
        System.Type _DataSourceType;
        /// <MetaDataID>{73d638f7-a1d4-4316-949c-d280ce320816}</MetaDataID>
        public System.Type DataSourceType
        {
            set
            {

            }
            get
            {

                try
                {
                    //CheckDataSourceTypeConsistency();

                    //if (Type != null)
                    //    return Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;

                    if (_DataSourceType == null)
                    {
                        if (Type != null && RootReportDataSource.ReportDataSources.ContainsKey(Type)&& !HasExtraMembers)
                        {
                            _DataSourceType = RootReportDataSource.ReportDataSources[Type];
                            if(_DataSourceType!=null)
                                return _DataSourceType;
                        }


                        //_DataSourceType =ModulePublisher.ClassRepository.GetType("AbstractionsAndPersistency.IOrder", "");
                        //return _DataSourceType ;
                        AutoIncrementTypeID++;
                        TypeBuilder typeBuilder = OOAdvantech.AccessorBuilder.GetInterfaceTypeBuilder(Name + GetHashCode().ToString() + "_" + AutoIncrementTypeID.ToString());

                        foreach (ReportDataSource reportDataSource in this.ValidDetailDataSources)
                        {
                            if (reportDataSource.IsCollection)
                                OOAdvantech.AccessorBuilder.CreateProperty(typeBuilder, typeof(System.Collections.ArrayList), reportDataSource.Name);
                            else
                            {
                                System.Type dataSourceType = null;
                                if (reportDataSource.Type != null && !reportDataSource.HasExtraMembers)
                                {
                                    

                                    //string fullPath = Type.FullName + "_" + reportDataSource.Name;
                                    if (!RootReportDataSource.ReportDataSources.ContainsKey(reportDataSource.Type))
                                    {
                                        RootReportDataSource.ReportDataSources[reportDataSource.Type] = null;
                                        dataSourceType = reportDataSource.DataSourceType;
                                    }
                                    else
                                        dataSourceType = RootReportDataSource.ReportDataSources[reportDataSource.Type];
                                    if (dataSourceType == null)
                                        dataSourceType = reportDataSource.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                                    OOAdvantech.AccessorBuilder.CreateProperty(typeBuilder, dataSourceType, reportDataSource.Name);
                                }
                                else
                                {
                                    OOAdvantech.AccessorBuilder.CreateProperty(typeBuilder, reportDataSource.DataSourceType, reportDataSource.Name);
                                }
                            }
                        }

                        foreach (Member member in this.ValidDataSourceMembers)
                        {
                            if (member.Type != null)
                            {
                                System.Type propertyType = member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                                if (propertyType != null)
                                    OOAdvantech.AccessorBuilder.CreateProperty(typeBuilder, propertyType, member.Name);
                            }
                        }


                        _DataSourceType = typeBuilder.CreateType();
                        if (Type != null && !HasExtraMembers)
                            RootReportDataSource.ReportDataSources[Type] = _DataSourceType;
                    }
                }
                catch (System.Exception error)
                {


                }
                if(_DataSourceType==null)
                    return typeof(object);
                return _DataSourceType;
            }
        }

        internal void CheckDataSourceTypeConsistency()
        {
            
            if (_DataSourceType != null)
            {
                if (ValidDataSourceMembers.Count + ValidDetailDataSources.Count < _DataSourceType.GetProperties().Length)
                {
                    _DataSourceType = null;
                }
                else
                {
                    foreach (ReportDataSource reportDataSource in ValidDetailDataSources)
                    {
                        if (_DataSourceType.GetProperty(reportDataSource.Name) == null)
                        {
                            _DataSourceType = null;
                            break;
                        }
                    }

                    foreach (Member member in ValidDataSourceMembers)
                    {
                        if (_DataSourceType.GetProperty(member.Name) == null)
                        {
                            _DataSourceType = null;
                            break;
                        }
                    }
                }
            }
            if (MasterDataSource != null)
                MasterDataSource.CheckDataSourceTypeConsistency();

        }
        /// <MetaDataID>{a685d641-2f17-4412-869d-643f3b1e9665}</MetaDataID>
        public ReportDataSource(string name)
        {
            _Name = name;
        }
        /// <MetaDataID>{e7af151c-29d5-40a5-b1f9-5759ea755316}</MetaDataID>
        public OOAdvantech.MetaDataRepository.MetaObject MetaObject
        {
            get
            {
                if (Attribute != null)
                    return Attribute;
                return AssociationEnd;
            }
        }
        bool _IsCollection = false;
       public bool IsCollection
        {
            get
            {
                return _IsCollection;
            }
            private set
            {
                _IsCollection = value;
            }
        }
            
        /// <MetaDataID>{38e2b690-629b-448b-a1a7-0fc833205939}</MetaDataID>
        internal ReportDataSource(OOAdvantech.MetaDataRepository.Attribute attribute, ReportDataSource masterDataSource)
        {
            _MasterDataSource = masterDataSource;

            Attribute = attribute;
            if (Attribute != null)
                _Name = Attribute.Name;
            _MetaObjectIdentity = Attribute.Identity.ToString();

            OOAdvantech.MetaDataRepository.Classifier enumeratorType = null;
            foreach (OOAdvantech.MetaDataRepository.Operation operation in attribute.Type.GetOperations("GetEnumerator"))
            {
                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                {
                    _Type = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                    _TypeFullName = _Type.FullName;
                    IsCollection = true;
                    break;
                }
            }
            if (!IsCollection)
            {
                _Type = attribute.Type;
                _TypeFullName = _Type.FullName;
            }
        }

        /// <MetaDataID>{a0a62b94-d6c6-4af7-b974-89df31aa50b8}</MetaDataID>
        internal ReportDataSource(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, ReportDataSource masterDataSource)
        {
            IsCollection = associationEnd.CollectionClassifier != null;
            _MasterDataSource = masterDataSource;
            AssociationEnd = associationEnd;
            _Type = associationEnd.Specification;
            _TypeFullName = _Type.FullName;
            Name = associationEnd.Name;
            _MetaObjectIdentity = AssociationEnd.Identity.ToString();

        }



        /// <exclude>Excluded</exclude>
        string _AssemblyFullName;
        /// <MetaDataID>{49e48b86-4da5-4c8d-b910-e175d9332cab}</MetaDataID>
        [PersistentMember("_AssemblyFullName")]
        [BackwardCompatibilityID("+1")]
        public string AssemblyFullName
        {
            get
            {
                return _AssemblyFullName;
            }
            set
            {
                if (_AssemblyFullName != value)
                {
                    _DataSourceType = null;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AssemblyFullName = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _TypeFullName;
        /// <MetaDataID>{5e3878f2-ec3f-45ed-89cd-b80b6aba4861}</MetaDataID>
        [PersistentMember("_TypeFullName")]
        [BackwardCompatibilityID("+2")]
        public string TypeFullName
        {
            get
            {
                return _TypeFullName;
            }
            set
            {
                if (_TypeFullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Type = null;
                        _DataSourceType = null;
                        _TypeFullName = value;

                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        protected MetaDataRepository.Classifier _Type;
        /// <MetaDataID>{7aa03ce2-ec9d-4871-a09d-1f081fa29051}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Classifier Type
        {
           
            get
            {
                if (_Type == null && Report != null)
                {

                    if (string.IsNullOrEmpty(TypeFullName))
                    {
                        //if (MetaObject == null)
                        //{
                        //    if (!string.IsNullOrEmpty(_Path) && MasterDataSource.Type != null)
                        //    {
                        //        _Type = OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(MasterDataSource.Type, _Path);
                        //        if (_Type != null)
                        //        {
                        //            foreach (OOAdvantech.MetaDataRepository.Operation operation in _Type.GetOperations("GetEnumerator"))
                        //            {
                        //                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                        //                if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                        //                {
                        //                    _Type = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                        //                    break;
                        //                }
                        //            }
                        //        }

                        //    }
                        //    else if(MasterDataSource!=null)
                        //        return MasterDataSource.Type;
                        //}
                        if (!string.IsNullOrEmpty(_Path) && MasterDataSource != null)
                        {
                            _Type = OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(MasterDataSource.Type, _Path);
                            if (_Type != null)
                            {
                                IsCollection = false;
                                foreach (OOAdvantech.MetaDataRepository.Operation operation in _Type.GetOperations("GetEnumerator"))
                                {
                                    OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                    if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                    {
                                        _Type = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                        IsCollection = true;
                                        break;
                                    }
                                }
                            }



                            return _Type;
                        }


                        return null;
                    }
                    _Type = Report.GetClassifier(TypeFullName, true);
                    if (!string.IsNullOrEmpty(_Path) && _Type != null)
                    {
                        _Type = OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(_Type, _Path);
                        if (_Type != null)
                        {
                            foreach (OOAdvantech.MetaDataRepository.Operation operation in _Type.GetOperations("GetEnumerator"))
                            {
                                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                {
                                    _Type = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                    break;
                                }
                            }
                        }

                    }


                }
                return _Type;
            }
        }

        Dictionary<MetaDataRepository.Classifier, System.Type> ReportDataSources = new Dictionary<Classifier, System.Type>(); 

        /// <exclude>Excluded</exclude>
        ReportDataSource _MasterDataSource;
        [Association("MasterDetailReportDataSource", typeof(ReportDataSource), Roles.RoleB, "e5ccd1eb-e4d7-4c2f-9e3b-a691fb6df8b6")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.LazyFetching)]
        [PersistentMember("_MasterDataSource")]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource MasterDataSource
        {
            get
            {
                return _MasterDataSource;
            }
            set
            {
                if (_MasterDataSource != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MasterDataSource = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        //public OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource RootReportDataSource
        //{
        //    get
        //    {
        //        if (MasterDataSource != null)
        //            return MasterDataSource;
        //        else
        //            return this;
        //    }

        //}

        /// <MetaDataID>{eee5efdb-22b4-4222-9e9d-f1979ea0e878}</MetaDataID>
        public void AddDetailDataSource(ReportDataSource detailDataSource)
        {
            
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _DetailDataSources.Add(detailDataSource);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{ffa09316-2785-476c-aa57-bdbd93836205}</MetaDataID>
        public ReportDataSource NewDetailDataSource(string name)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                ReportDataSource detailDataSource = new ReportDataSource(name);
                ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(RootReportDataSource);
                if (objectStorage != null)
                    objectStorage.CommitTransientObjectState(detailDataSource);
                _DetailDataSources.Add(detailDataSource);
                stateTransition.Consistent = true;
                return detailDataSource;
            }

        }
        /// <MetaDataID>{f8e72b50-ce5b-4872-9243-8ed51b5de216}</MetaDataID>
        public void DeleteDetailDataSource(ReportDataSource detailDataSource)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _DetailDataSources.Remove(detailDataSource);
                stateTransition.Consistent = true;
            }
            CheckDataSourceTypeConsistency();

        }

        /// <MetaDataID>{b2bad9a2-1bba-4ff8-a3bf-8de91d738323}</MetaDataID>
        public void AddMember(Member member)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _DataSourceMembers.Add(member);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{160a94a7-3e27-49ac-97db-4dfab0bd35a7}</MetaDataID>
        public Member NewMember(string name)
        {
            Member member = null;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                member = new Member(name, this);
                ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(RootReportDataSource);
                if (objectStorage != null)
                    objectStorage.CommitTransientObjectState(member);
                _DataSourceMembers.Add(member);
                stateTransition.Consistent = true;

            }


            return member;
        }
        /// <MetaDataID>{cb34c95d-97ce-4dbb-8e03-cc0d8f0be0e9}</MetaDataID>
        public void DeleteMember(Member member)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _DataSourceMembers.Remove(member);
                stateTransition.Consistent = true;
            }
            
            CheckDataSourceTypeConsistency();


        }

        /// <MetaDataID>{5fa087d0-05a7-4060-aecf-b897e295be62}</MetaDataID>
        Dictionary<OOAdvantech.MetaDataRepository.MetaObject, ReportDataSource> TransientDetailDataSources = new Dictionary<MetaObject, ReportDataSource>();
        /// <MetaDataID>{fd0643b9-f85e-405a-b4cc-2b09bcf35e55}</MetaDataID>
        Dictionary<OOAdvantech.MetaDataRepository.MetaObject, Member> TransientMembers = new Dictionary<MetaObject, Member>();

        internal int CurrentNodePathDepth
        {
            get
            {
                if (MasterDataSource == null)
                    return 0;
                else
                    return MasterDataSource.CurrentNodePathDepth + 1;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ReportDataSource> _DetailDataSources = new OOAdvantech.Collections.Generic.Set<ReportDataSource>();
        [Association("MasterDetailReportDataSource", typeof(ReportDataSource), Roles.RoleA, "e5ccd1eb-e4d7-4c2f-9e3b-a691fb6df8b6")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember("_DetailDataSources")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource> DetailDataSources
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<ReportDataSource> dataSources = new OOAdvantech.Collections.Generic.Set<ReportDataSource>(_DetailDataSources);
                if (RootReportDataSource==null|| CurrentNodePathDepth > RootReportDataSource.PathNodesDepth)
                    return dataSources;

                try
                {
                    if (Type != null)
                    {
                        foreach (Attribute attribute in Type.GetAttributes(true))
                        {
                            if (attribute.Visibility != VisibilityKind.AccessPublic)
                                continue;

                            OOAdvantech.MetaDataRepository.Classifier enumeratorType = null;
                            if (attribute.Type.FullName != typeof(string).FullName)
                            {
                                foreach (OOAdvantech.MetaDataRepository.Operation operation in attribute.Type.GetOperations("GetEnumerator"))
                                {
                                    OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                    if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                    {
                                        enumeratorType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                        break;
                                    }
                                }
                            }
                            bool exist = false;
                            foreach (ReportDataSource existingDataSource in dataSources)
                            {
                                if (existingDataSource.Name == attribute.Name || existingDataSource.MetaObjectIdentity == attribute.Identity.ToString())
                                {
                                    existingDataSource.Attribute = attribute;
                                    exist = true;
                                    break;
                                }
                            }
                        if (attribute.Type is MetaDataRepository.Interface ||
                             attribute.Type is MetaDataRepository.Structure ||
                            (attribute.Type is MetaDataRepository.Class &&
                            !((attribute.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type).IsSubclassOf(typeof(string))||
                            (attribute.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type)==typeof(string))))
                        {
                                if (!exist)
                                {
                                    ReportDataSource reportDataSource = null;
                                    if (!TransientDetailDataSources.TryGetValue(attribute, out reportDataSource))
                                    {
                                        reportDataSource = new ReportDataSource(attribute, this);
                                        TransientDetailDataSources[attribute] = reportDataSource;
                                    }
                                    dataSources.Add(reportDataSource);
                                }
                            }
                        }

                        foreach (AssociationEnd associationEnd in Type.GetAssociateRoles(true))
                        {
                            if (associationEnd.Visibility != VisibilityKind.AccessPublic || !associationEnd.Navigable)
                                continue;

                            OOAdvantech.MetaDataRepository.Classifier enumeratorType = null;
                            if (associationEnd.CollectionClassifier != null)
                            {
                                foreach (OOAdvantech.MetaDataRepository.Operation operation in associationEnd.CollectionClassifier.GetOperations("GetEnumerator"))
                                {
                                    OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                    if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                    {
                                        enumeratorType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                        break;
                                    }
                                }
                            }

                            bool exist = false;
                            foreach (ReportDataSource existingDataSource in dataSources)
                            {
                                if (existingDataSource.Name == associationEnd.Name || existingDataSource.MetaObjectIdentity == associationEnd.Identity.ToString())
                                {
                                    existingDataSource.AssociationEnd = associationEnd;
                                    exist = true;
                                    break;
                                }
                            }
                            //if (enumeratorType != null)
                            {

                                if (!exist)
                                {
                                    ReportDataSource reportDataSource = null;
                                    if (!TransientDetailDataSources.TryGetValue(associationEnd, out reportDataSource))
                                    {
                                        reportDataSource = new ReportDataSource(associationEnd, this);
                                        TransientDetailDataSources[associationEnd] = reportDataSource;
                                    }
                                    dataSources.Add(reportDataSource);
                                }
                            }
                        }

                    }
                }
                catch (System.Exception error)
                {

                    throw;
                }
                return dataSources;
                //return new OOAdvantech.Collections.Generic.Set<ReportDataSource>(_DetailDataSources,OOAdvantech.Collections.CollectionAccessType.ReadOnly) ;
            }
        }
        /// <MetaDataID>{eff01a6f-edcc-4f68-8377-d624bfaf9ec4}</MetaDataID>
        ReportRootDataSource RootReportDataSource
        {
            get
            {
                if (MasterDataSource == null)
                    return this as ReportRootDataSource;
                else
                    return MasterDataSource.RootReportDataSource as ReportRootDataSource;
            }
        }

        public OOAdvantech.Collections.Generic.Set<Member> ValidDataSourceMembers
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<Member> dataSourceMembers = DataSourceMembers;
                foreach (Member member in new OOAdvantech.Collections.Generic.Set<Member>(dataSourceMembers))
                {
                    if (member.Attribute == null && member.AssociationEnd == null && member.Type == null)
                        dataSourceMembers.Remove(member);
                }
                return dataSourceMembers;
            }
        }
        public OOAdvantech.Collections.Generic.Set<ReportDataSource> ValidDetailDataSources
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<ReportDataSource> detailDataSources = this.DetailDataSources;
                foreach (ReportDataSource detailDataSource in new OOAdvantech.Collections.Generic.Set<ReportDataSource>(detailDataSources))
                {
                    if (detailDataSource.Attribute == null && detailDataSource.AssociationEnd == null && detailDataSource.Type == null)
                        detailDataSources.Remove(detailDataSource);
                }
                return detailDataSources;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Member> _DataSourceMembers = new OOAdvantech.Collections.Generic.Set<Member>();
        [Association("ReportDataSourceMember", typeof(Member), Roles.RoleA, "d75b3b93-594b-4b30-bf49-cf451809cbbd")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember("_DataSourceMembers")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.UserInterface.ReportObjectDataSource.Member> DataSourceMembers
        {
            get
            {

                OOAdvantech.Collections.Generic.Set<Member> dataSourceMembers = new OOAdvantech.Collections.Generic.Set<Member>(_DataSourceMembers);
                if (RootReportDataSource == null || CurrentNodePathDepth > RootReportDataSource.PathNodesDepth)
                    return dataSourceMembers;

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    foreach (Member member in dataSourceMembers)
                    {
                        if (member.Type is MetaDataRepository.Interface ||
                               (member.Type is MetaDataRepository.Class &&
                               !((member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type).IsSubclassOf(typeof(string)) ||
                               (member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type) == typeof(string))))
                        {
                            _DataSourceMembers.Remove(member);
                        }
                    }
                    stateTransition.Consistent = true;
                }
                dataSourceMembers = new OOAdvantech.Collections.Generic.Set<Member>(_DataSourceMembers);

                if (Type != null)
                {
                    foreach (Attribute attribute in Type.GetAttributes(true))
                    {
                        if (attribute.Visibility != VisibilityKind.AccessPublic)
                            continue;

                        OOAdvantech.MetaDataRepository.Classifier enumeratorType = null;
                        if (attribute.Type.FullName != typeof(string).FullName)
                        {
                            foreach (OOAdvantech.MetaDataRepository.Operation operation in attribute.Type.GetOperations("GetEnumerator"))
                            {
                                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                {
                                    enumeratorType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                    break;
                                }
                            }
                        }
                        if (attribute.Type is MetaDataRepository.Interface ||
                            attribute.Type is MetaDataRepository.Structure ||
                            (attribute.Type is MetaDataRepository.Class &&
                            !((attribute.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type).IsSubclassOf(typeof(string))||
                            (attribute.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type)==typeof(string))))
                        {
                            continue;
                        }
                        if (enumeratorType != null)
                            continue;
                        else
                        {
                            bool exist = false;
                            foreach (Member existingMember in dataSourceMembers)
                            {
                                if (existingMember.Name == attribute.Name || existingMember.MetaObjectIdentity == attribute.Identity.ToString())
                                {
                                    existingMember.Attribute = attribute;
                                    exist = existingMember.Name == attribute.Name;
                                    // break;
                                }
                            }
                            if (!exist)
                            {

                                Member member = null;
                                if (!TransientMembers.TryGetValue(attribute, out member))
                                {
                                    member = new Member(attribute, this);
                                    TransientMembers[attribute] = member;
                                }
                                if (attribute.Name != member.Name)
                                {
                                    if (!_DataSourceMembers.Contains(member))
                                    {
                                        if (!ObjectStorage.IsPersistent(member))
                                        {

                                            if (MasterDataSource != null)
                                                MasterDataSource.EnsurePersistency();
                                            ObjectStorage.GetStorageOfObject(RootReportDataSource).CommitTransientObjectState(member);
                                        }

                                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                                        {
                                            _DataSourceMembers.Add(member);
                                            stateTransition.Consistent = true;
                                        }

                                    }
                                    member = new Member(attribute, this);


                                    TransientMembers[attribute] = member;
                                }

                                dataSourceMembers.Add(member);
                            }

                        }
                    }


                    //foreach (Member member in new  OOAdvantech.Collections.Generic.Set<Member>(dataSourceMembers))
                    //{
                    //    if (member.Attribute == null && member.AssociationEnd == null && member.Type==null)
                    //        dataSourceMembers.Remove(member);
                    //}


                    //foreach (AssociationEnd associationEnd in Type.GetAssociateRoles(true))
                    //{
                    //    if (associationEnd.Visibility == VisibilityKind.AccessPublic && associationEnd.Navigable)
                    //    {
                    //        OOAdvantech.MetaDataRepository.Classifier enumeratorType = null;
                    //        if (associationEnd.CollectionClassifier != null)
                    //        {
                    //            foreach (OOAdvantech.MetaDataRepository.Operation operation in associationEnd.CollectionClassifier.GetOperations("GetEnumerator"))
                    //            {
                    //                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                    //                if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                    //                {
                    //                    enumeratorType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                    //                    break;
                    //                }
                    //            }
                    //        }

                    //        if (enumeratorType != null)
                    //            continue;
                    //        else
                    //        {
                    //            bool exist = false;
                    //            foreach (Member existingMember in dataSourceMembers)
                    //            {
                    //                if (existingMember.Name == associationEnd.Name || existingMember.MetaObjectIdentity == associationEnd.Identity.ToString())
                    //                {
                    //                    existingMember.AssociationEnd = associationEnd;
                    //                    exist = existingMember.Name == associationEnd.Name;
                    //                    // break;
                    //                }
                    //            }
                    //            if (!exist)
                    //            {
                    //                Member member = null;
                    //                if (!TransientMembers.TryGetValue(associationEnd, out member))
                    //                {
                    //                    member = new Member(associationEnd, this);
                    //                    TransientMembers[associationEnd] = member;
                    //                }
                    //                if (associationEnd.Name != member.Name)
                    //                {
                    //                    if (!_DataSourceMembers.Contains(member))
                    //                    {
                    //                        if (!ObjectStorage.IsPersistent(member))
                    //                            ObjectStorage.GetStorageOfObject(RootReportDataSource).CommitTransientObjectState(member);
                    //                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    //                        {
                    //                            _DataSourceMembers.Add(member);
                    //                            stateTransition.Consistent = true;
                    //                        }
                    //                    }
                    //                    member = new Member(associationEnd, this);
                    //                    TransientMembers[associationEnd] = member;
                    //                }
                    //                dataSourceMembers.Add(member);
                    //            }
                    //        }
                    //    }
                    //}
                }
                return dataSourceMembers;

            }
        }
        /// <MetaDataID>{337e0177-a2cd-4950-b5d5-18b14c8fb986}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, bool caseSensitive, System.ComponentModel.Component component)
        {
            return UserInterface.Runtime.UISession.GetClassifier(fullName,"", caseSensitive, component);
        }

        /// <MetaDataID>{474027d4-b4d6-4ccb-8e2d-5a683d9221b2}</MetaDataID>
        System.Collections.Generic.Dictionary<string, Component> Components;
        /// <MetaDataID>{0eea84fe-ffd4-48b3-a46c-52cfba09bb36}</MetaDataID>
        public Component GetMember(string name)
        {
            if (Components == null)
            {
                Components = new Dictionary<string, Component>();
                foreach (Member member in _DataSourceMembers)
                {
                    Components[member.Name] = member;
                }
                foreach (Member member in TransientMembers.Values)
                {
                    Components[member.Name] = member;
                }

                foreach (ReportDataSource detailReportDataSource in DetailDataSources)
                {
                    Components[detailReportDataSource.Name] = detailReportDataSource;
                }
                //foreach (ReportDataSource detailReportDataSource in TransientDetailDataSources.Values)
                //{
                //    Components[detailReportDataSource.Name] = detailReportDataSource;

                //}
            }
            Component component = null;
            Components.TryGetValue(name, out component);
            return component;
        }


        /// <MetaDataID>{278e2d71-62fb-4cac-a6cd-e345345d4293}</MetaDataID>
        public ReportDataSource MakeMemberDetailDataSource(Member reportDataSourceMember, string path)
        { 
            if (reportDataSourceMember.MetaObject != null &&
               TransientMembers.ContainsKey(reportDataSourceMember.MetaObject))
            {
                TransientMembers.Remove(reportDataSourceMember.MetaObject);
            }
            DeleteMember(reportDataSourceMember);
           
            ReportDataSource reportDataSource = null;
            if (reportDataSourceMember.MetaObject is Attribute)
                reportDataSource = new ReportDataSource(reportDataSourceMember.MetaObject as Attribute, this);
            else if (reportDataSourceMember.MetaObject is AssociationEnd)
                reportDataSource = new ReportDataSource(reportDataSourceMember.MetaObject as AssociationEnd, this);
            else
                reportDataSource = new ReportDataSource(reportDataSourceMember.Name);
            ObjectStorage.GetStorageOfObject(RootReportDataSource).CommitTransientObjectState(reportDataSource);
            reportDataSource.Path = path;
            reportDataSource.Name = reportDataSourceMember.Name;

            AddDetailDataSource(reportDataSource);
            _DataSourceType = null;
            return reportDataSource;
        }
    }

    /// <MetaDataID>{3759826a-003d-432b-93d5-3184f2db5681}</MetaDataID>
    [BackwardCompatibilityID("{1949C5E1-E865-4b4e-A214-BCBDC404A76C}"), Persistent()]
    public class ReportRootDataSource : ReportDataSource
    {

        /// <exclude>Excluded</exclude>
        int _PathNodesDepth = 5;
        [PersistentMember("_PathNodesDepth"), BackwardCompatibilityID("+5")]
        public int PathNodesDepth
        {
            get
            {
                if (_PathNodesDepth == 0)
                    PathNodesDepth = 5;
                return _PathNodesDepth;

            }
            set
            {
                if (_PathNodesDepth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PathNodesDepth = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _QueryResultPath;
        [PersistentMember("_QueryResultPath"), BackwardCompatibilityID("+4")]
        public string QueryResultPath
        {
            get
            {
                object obj = Type;
                return _QueryResultPath;

            }
            set
            {
                if (_QueryResultPath != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        
                        _QueryResultPath = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }
        public IQueryResult QueryResult
        {
            get
            {
                if (!string.IsNullOrEmpty(_QueryResultPath))
                {
                    IQueryResult queryResult = null;
                    int nPos = _QueryResultPath.LastIndexOf(".");
                    if (nPos != -1)
                    {
                        string typeFullName = _QueryResultPath.Substring(0, nPos);
                        Classifier classifier = Report.GetClassifier(typeFullName, true);
                        if (classifier != null)
                        {
                            System.Type type = classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                            if (type != null)
                            {
                                string memberName = _QueryResultPath.Substring(nPos + 1);
                                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(memberName);
                                if (propertyInfo != null)
                                {
                                    queryResult = propertyInfo.GetValue(null, null) as IQueryResult;
                                }
                                else
                                {
                                    System.Reflection.FieldInfo fieldInfo = type.GetField(memberName);
                                    if (fieldInfo != null)
                                    {
                                        queryResult = fieldInfo.GetValue(null) as IQueryResult;
                                    }
                                }
                            }
                        }
                    }
                    return queryResult;
                }
                else
                    return null;

            }
        }

        public override Classifier Type
        {
            get
            {
                if (_Type == null && Report != null)
                {

                    if (!string.IsNullOrEmpty(TypeFullName))
                    {
                        _Type = Report.GetClassifier(TypeFullName, true);
                        if (_Type != null)
                        {
                            if(!string.IsNullOrEmpty(Path))
                                _Type = OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(_Type, Path);
                            if (_Type != null)
                            {
                                foreach (OOAdvantech.MetaDataRepository.Operation operation in _Type.GetOperations("GetEnumerator"))
                                {
                                    OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                    if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                    {
                                        _Type = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as Classifier;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(_QueryResultPath))
                    {
                        IQueryResult queryResult = QueryResult;
                        if (queryResult != null)
                        {
                            try
                            {
                                if (queryResult.QueryResultType != null && queryResult.QueryResultType.GetInterface(typeof(System.Collections.Generic.IEnumerable<>).Name) != null)
                                    _Type = Classifier.GetClassifier(queryResult.QueryResultType.GetInterface(typeof(System.Collections.Generic.IEnumerable<>).Name).GetGenericArguments()[0]);
                            }
                            catch (System.Exception error)
                            {

                                _Type =Classifier.GetClassifier( typeof(object));
                            }
                        }
                    }


                }
                return _Type;
            }
        }

    }

    /// <MetaDataID>{7963c3d0-e843-45e2-8616-d0a8e5dd383e}</MetaDataID>
    public interface IReport
    {
        /// <MetaDataID>{462a2896-7e13-45b7-924e-53b0e0658351}</MetaDataID>
        MetaDataRepository.Classifier GetClassifier(string typeFullName, bool caseSensitive);
        /// <MetaDataID>{c7c81c95-4bd4-4145-b2ff-4fb33a5ed1fc}</MetaDataID>
        List<string> Paths
        {
            get;
        }
        /// <MetaDataID>{30d90c0a-8ebc-404e-b45f-1bf3cb707511}</MetaDataID>
        OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource ReportDataSource
        {
            get;
        }

        bool ReportDesignMode
        {
            get;
        }
        bool ReportLoaded
        {
            get;
        }
        bool CanBeTransient(Member member);
        bool CanBeTransient(ReportDataSource detailReportDataSource);

    }


}
