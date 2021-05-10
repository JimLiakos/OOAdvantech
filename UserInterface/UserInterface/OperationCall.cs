namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using Transactions;

    /// <MetaDataID>{87ff459b-c665-4ed7-86d2-de59c19844bd}</MetaDataID>
    public enum CallType
    {
        HostingFormOperationCall,
        ViewControlObjectOperationCall,
        StaticOperationCall,
        ControlDisplayObjectCall
    }




    /// <MetaDataID>{166043BA-D409-4F16-9B7B-3DC583474A98}</MetaDataID>
    [BackwardCompatibilityID("{166043BA-D409-4F16-9B7B-3DC583474A98}")]
    [Persistent()]
    public class OperationCall:MetaObject
    {
        public OperationCall()
        {

        }
        /// <MetaDataID>{f3430e1f-c264-4c67-a2e9-50171051d1bc}</MetaDataID>
        public static Classifier GetElementType(Classifier seqType)
        {
            if (seqType == null)
                return null;
            foreach (OOAdvantech.MetaDataRepository.Operation operation in seqType.GetOperations("GetEnumerator"))
            {
                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
                    return  enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
            }
            return null;
        }

        /// <MetaDataID>{66b66211-627f-4412-9105-6abca2e0f445}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }

        /// <exclude>Excluded</exclude>
        private OOAdvantech.Transactions.TransactionOption _TransactionOption=TransactionOption.Supported;
 
        /// <MetaDataID>{74acbb34-1af9-41a9-b5c6-85935cd53e78}</MetaDataID>
        [PersistentMember("_TransactionOption"), BackwardCompatibilityID("+17")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return _TransactionOption;
            }
            set
            {
                if (_TransactionOption != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _TransactionOption = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        private bool _RefreshFromForReturnObject;
        /// <MetaDataID>{e420d652-102f-4acf-a9c5-336b88ea8834}</MetaDataID>
        /// <summary>
        /// When is true the user interface subsystem refresh control that display values 
        /// which are related with the object of return value. 
        /// The refresh action happens after the operation call and in transaction of the form.
        /// </summary>
        [PersistentMember("_RefreshFromForReturnObject")]
        [BackwardCompatibilityID("+22")]
        public bool RefreshFromForReturnObject
        {
            get
            {
                return _RefreshFromForReturnObject;
            }
            set
            {
                if (_RefreshFromForReturnObject != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RefreshFromForReturnObject = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DF7C4386-D5A4-4E7B-A912-050B304AABC9}</MetaDataID>
        private string _CalledObjectPath;
        /// <MetaDataID>{5563382D-6DCA-4094-ACB7-DA5EE68E4C66}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        [PersistentMember("_CalledObjectPath")]
        public string CalledObjectPath
        {
            get
            {
                return _CalledObjectPath;
            }
            set
            {
                if (_CalledObjectPath != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CalledObjectPath = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0C72E5B6-5C7E-43FE-A4E0-6A8FD7903A57}</MetaDataID>
        private string _Type;
        /// <MetaDataID>{FCC553A0-A30F-4093-99F9-178C234E174B}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        [PersistentMember("_Type")]
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (_Type != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Type = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{D15E49D4-E3F6-4049-83CD-BACB626FA3F3}</MetaDataID>
        public ParameterLoader GetParameterLoaderAt(int index)
        {
            SortParameterLoaders();
            if (SortedParameterLoaders.Count > index)
                return SortedParameterLoaders[index];
            else
                return null;
        }
        /// <MetaDataID>{882E551D-C1AB-43F2-9DC3-E81A9A408BC3}</MetaDataID>
        private System.Collections.Generic.List<OOAdvantech.UserInterface.ParameterLoader> SortedParameterLoaders = new System.Collections.Generic.List<ParameterLoader>();
        /// <MetaDataID>{BE35D94A-1E9B-4BAB-B241-9FB7B4216283}</MetaDataID>
        void SortParameterLoaders()
        {
            
            SortedParameterLoaders.Clear();
            foreach (ParameterLoader parameterLoader in _ParameterLoaders)
                SortedParameterLoaders.Add(parameterLoader);
            SortedParameterLoaders.Sort(new PositionCompare<ParameterLoader>());
            for (short k  = 0; k != SortedParameterLoaders.Count; k++)
            {
                
               ParameterLoader parameterLoader = SortedParameterLoaders[k];
               if (parameterLoader.Position != k)
                {
                   
                    using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition())
                    {
                        for (short i = 0; i != SortedParameterLoaders.Count; i++)
                        {
                            if (parameterLoader != null)
                            {
                                parameterLoader.Position = i;
                                stateTransition.Consistent = true;
                            }
                        }

                        stateTransition.Consistent = true;
                    }
                    return;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8576E9AD-FCC5-4BB8-B44F-79117ED5ED11}</MetaDataID>
        private string _AssemblyFullName = "";
        /// <MetaDataID>{275DF680-A20D-476B-A962-93F9A3D2288D}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        [PersistentMember("_AssemblyFullName")]
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
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AssemblyFullName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{AD3D6A33-447D-44F9-93DB-FD4FC664E022}</MetaDataID>
        public System.Reflection.MemberInfo Method
        {
            get
            {
                return default(System.Reflection.MemberInfo);
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4156B4CF-524B-40F7-B937-492DC30DF740}</MetaDataID>
        private string _OperationPath = "";
        /// <MetaDataID>{C081D1FC-838A-4857-9B41-CE4A21BDADB2}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember("_OperationPath")]
        public string OperationPath
        {
            get
            {

                return _OperationPath;
            }
            set
            {
                if (_OperationPath != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OperationPath = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{AA30AB02-BD0C-4004-A2B4-058BFE1A8215}</MetaDataID>
        private CallType _CallType;
        /// <MetaDataID>{9B13C8C4-ADDD-4078-B1AD-CD11390BAA8E}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_CallType")]
        public CallType CallType
        {
            get
            {
                return _CallType;
            }
            set
            {
                if (_CallType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CallType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{921BA39A-7757-4960-AFBE-4305F9E5A815}</MetaDataID>
        private string _ReturnValueDestination;
        /// <MetaDataID>{6ED7C368-B8F4-4780-A274-D9BEC38C3E44}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember("_ReturnValueDestination")]
        public string ReturnValueDestination
        {
            get
            {
                return _ReturnValueDestination;
            }
            set
            {
                if (_ReturnValueDestination != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ReturnValueDestination = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3977884B-5FA5-437D-8049-A17755A318D2}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<ParameterLoader> _ParameterLoaders = new OOAdvantech.Collections.Generic.Set<ParameterLoader>();
        /// <MetaDataID>{F93B1439-1277-42F2-866D-D2BA64B9A7EF}</MetaDataID>
        [Association("LoadParameter", typeof(OOAdvantech.UserInterface.ParameterLoader), Roles.RoleA, "{ECD02DE1-2C17-48FA-9B48-7CC27FDF4E3C}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_ParameterLoaders")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<ParameterLoader> ParameterLoaders
        {



            get
            {
                OOAdvantech.Collections.Generic.Set<ParameterLoader> parameterLoaders = new OOAdvantech.Collections.Generic.Set<ParameterLoader>();

                SortParameterLoaders();
                foreach (ParameterLoader parameterLoader in SortedParameterLoaders)
                    parameterLoaders.Add(parameterLoader);
                return parameterLoaders;
            }
        }
        /// <MetaDataID>{D9D095B5-ABE8-44E5-B462-689463D46075}</MetaDataID>
        public void AddParameterLoader(ParameterLoader parameterLoader)
        {
            if (!_ParameterLoaders.Contains(parameterLoader))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ParameterLoaders.Add(parameterLoader);
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{EA37ECEA-46AF-46D7-9749-2A2FE0CCAC53}</MetaDataID>
        public void AddParameterLoader(short index, ParameterLoader parameterLoader)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                SortParameterLoaders();

                if (SortedParameterLoaders.Contains(parameterLoader))
                {
                    SortedParameterLoaders.Remove(parameterLoader);
                    SortedParameterLoaders.Insert(index, parameterLoader);
                }
                else
                    SortedParameterLoaders.Insert(index, parameterLoader);

                if (!_ParameterLoaders.Contains(parameterLoader))
                    _ParameterLoaders.Add(parameterLoader);
                for (short i = 0; i != SortedParameterLoaders.Count; i++)
                {
                    parameterLoader = SortedParameterLoaders[i] as ParameterLoader;
                    if (parameterLoader != null)
                        parameterLoader.Position = i;
                }
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{86370AF8-E1F8-4AD4-B629-B2240D03C840}</MetaDataID>
        public ParameterLoader NewParameterLoader(string name)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                ParameterLoader newParameterLoader = objectStorage.NewObject(typeof(ParameterLoader)) as ParameterLoader;
                newParameterLoader.Name = name;
                AddParameterLoader(newParameterLoader);
                stateTransition.Consistent = true;
                return newParameterLoader;
            }
        }

        /// <MetaDataID>{E0049261-EF55-41A1-BE59-5BDC80E328D3}</MetaDataID>
        public ParameterLoader NewParameterLoader(short index, string name)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                ParameterLoader newParameterLoader = objectStorage.NewObject(typeof(ParameterLoader)) as ParameterLoader;
                newParameterLoader.Name = name;
                AddParameterLoader(index, newParameterLoader);
                stateTransition.Consistent = true;
                return newParameterLoader;
            }
        }


        /// <MetaDataID>{159D084C-466A-4BFC-8057-517C913FD28D}</MetaDataID>
        public void RemoveParameterLoader(ParameterLoader parameterLoader)
        {
            if (_ParameterLoaders.Contains(parameterLoader))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ParameterLoaders.Remove(parameterLoader);
                    stateTransition.Consistent = true;
                }
            }

        }


        /// <MetaDataID>{BB0D4B68-123E-47B5-B5D4-2926B7BBF5E4}</MetaDataID>
        private ObjectStateManagerLink Properties;
    }
}
