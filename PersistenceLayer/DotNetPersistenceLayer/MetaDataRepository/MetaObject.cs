using System.Xml.Linq;
using System.Linq;
using System.Reflection;
#if DeviceDotNet
using OOAdvantech.Remoting;
#else
using System;
#endif

namespace OOAdvantech.MetaDataRepository
{
    public delegate void MetaObjectChangedEventHandler(object sender);
    /// <MetaDataID>{9b9e0500-abd7-4293-85e2-c7d37957bdb2}</MetaDataID>
    public enum VisibilityKind
    {
        /// <summary>
        /// You can access this member from anywhere.
        /// </summary>
        AccessPublic = 0,
        /// <summary>
        /// You can access this member only from its classifier.
        /// </summary>
        AccessPrivate = 1,
        /// <summary>
        /// You can access this member only from code in the same component.
        /// </summary>
        AccessComponent = 2,
        /// <summary>
        /// You can access this member only from its classifier and subclassifier.
        /// </summary>
        AccessProtected = 3,
        /// <summary>
        /// You can access this member only from code in the same component and subclassifier of its classifier.
        /// </summary>
        AccessComponentOrProtected = 4

    }
    /// <MetaDataID>{697EFC48-3DCD-4675-9B4E-87C8917D80DF}</MetaDataID>
    [BackwardCompatibilityID("{697EFC48-3DCD-4675-9B4E-87C8917D80DF}")]
    [Persistent()]
    public abstract class MetaObject : MarshalByRefObject, Remoting.IExtMarshalByRefObject, IObjectState
    {
        public event MetaObjectChangedEventHandler Changed;

        public override string ToString()
        {
            if (FullName != null)
                return FullName;
            return base.ToString();
        }

        /// <MetaDataID>{913c300a-7a68-4a3a-8f97-6aecbf712310}</MetaDataID>
        public void MetaObjectChangeState()
        {
            OOAdvantech.EventUnderProtection.Invoke<MetaObjectChangedEventHandler>(ref Changed, OOAdvantech.EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers | OOAdvantech.EventUnderProtection.ExceptionHandling.IgnoreExceptions, this);
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F80A16F7-F7BC-44A5-94EF-266A71CD7D70}</MetaDataID>
        protected string _CaseInsensitiveName;
        /// <MetaDataID>{8943E54C-9F86-4041-9087-8C07B0CF0711}</MetaDataID>
        [BackwardCompatibilityID("+49")]
        [PersistentMember("_CaseInsensitiveName")]
        public virtual string CaseInsensitiveName
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_CaseInsensitiveName == null || _CaseInsensitiveName.Trim().Length == 0)
                        return _Name;
                    return _CaseInsensitiveName;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_CaseInsensitiveName != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _CaseInsensitiveName = value;
                            StateTransition.Consistent = true; ;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{252A5D3D-52A9-4694-9552-8A0758E8B281}</MetaDataID>
        protected Member<Component> _ImplementationUnit = new Member<Component>();
        /// <MetaDataID>{357D9FF9-1D42-48F1-A8A2-DA9AF4A50454}</MetaDataID>
        [Association("Implementation", typeof(OOAdvantech.MetaDataRepository.Component), Roles.RoleB, "{8FAF0E7A-D1F5-42A0-9887-25790C5913BB}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_ImplementationUnit")]
        [RoleBMultiplicityRange(0, 1)]
        public virtual Component ImplementationUnit
        {
            get
            {
                return _ImplementationUnit.Value;
            }
        }

        /// <MetaDataID>{3690d879-4a34-44b1-af38-01f0389aedac}</MetaDataID>
        protected object LockObject = new object();
        /// <MetaDataID>{D4C7E50E-38F9-487A-B2EA-5AD544335D10}</MetaDataID>
        protected OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock(true);

        public struct MetaDataError
        {
            public string ErrorMessage;
            public string ErrorPath;

            public MetaDataError(string errorMessage, string errorPath)
            {
                ErrorMessage = errorMessage;
                ErrorPath = errorPath;
            }

        }
        /// <MetaDataID>{7EA46183-0EF4-43CE-9360-52BB99D463D4}</MetaDataID>
        private System.Collections.Generic.Dictionary<string, MetaObject> Identities = new System.Collections.Generic.Dictionary<string, MetaObject>();
        /// <MetaDataID>{9C2CD5EA-65B8-444B-BEFE-830CC0072832}</MetaDataID>
        public virtual void ShallowSynchronize(MetaObject originClassifier)
        {
            if (_Name != originClassifier.Name)
            {
                _Name = originClassifier.Name;
                _CaseInsensitiveName = null;
            }

            if (originClassifier.Namespace != null)
            {
                _Namespace.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originClassifier.Namespace, this) as MetaDataRepository.Namespace;
                if (_Namespace.Value == null)
                {
                    _Namespace.Value = (Namespace)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originClassifier.Namespace, this);
                    _Namespace.Value.ShallowSynchronize(originClassifier.Namespace);
                    if (_Namespace.Value != null)
                        _Namespace.Value.AddOwnedElement(this);
                }
            }

            if (_ImplementationUnit.Value == null && originClassifier.ImplementationUnit != null)
            {
                _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originClassifier.ImplementationUnit, this) as MetaDataRepository.Component;


                if (_ImplementationUnit == null)
                {
                    _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originClassifier.ImplementationUnit, this);
                }
            }
        }

        /// <summary>This method add a new dependency between the this meta object 
        /// and the meta object of supplier parameter with name the dependancyName parameter value. 
        /// Return the new dependency. 
        /// The new dependency has the this meta object as client and the meta object of parameter as supplier. </summary>
        /// <param name="dependencyName">This parameter defines the name of new dependency. </param>
        /// <param name="supplier">This parameter defines the supplier of dependency. 
        /// The value of this parameter must be not null. </param>
        /// <MetaDataID>{364170A6-D7BE-435A-987B-60CE2F652EE7}</MetaDataID>
        public virtual Dependency AddDependency(string dependencyName, MetaObject supplier)
        {

            MetaDataRepository.Dependency dependency = new MetaDataRepository.Dependency(dependencyName, this, supplier);
            _ClientDependencies.Add(dependency);
            supplier._SupplierDependencies.Add(dependency);
            return dependency;
        }
        /// <MetaDataID>{196415FA-E22A-4610-8C30-F7AF05D4117D}</MetaDataID>
        public ObjectStateManagerLink Properties = new ObjectStateManagerLink();


        /// <summary>Check the Meta data if the declaration is consistent or not 
        /// for example you can't declare a class persistent without default constructor. 
        /// This method is virtual all subclasses of MetaObject class can override this function to extent the error check. </summary>
        /// <returns>If there is error the method return true else return false. </returns>
        /// <MetaDataID>{C757EA8D-2FAD-4B81-ACFF-345C6399E091}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            string identity = null;
            try { identity = Identity.ToString(); }
            catch
            {
                lock (identityLock)
                {
                    identity = _Identity.ToString();
                }
            }

            if (Identities.ContainsKey(identity))
            {
                MetaObject metaObject = Identities[identity] as MetaObject;
                if (metaObject != this)
                {
                    errors.Add(new MetaDataError("MDR Error: The Meta Object '" + FullName + "' has the same identity with th '" + metaObject.FullName + "'.", FullName));
                    return true;
                }
            }
            else
                Identities.Add(identity, this);
            return false;
        }

        /// <MetaDataID>{628b82ab-ab44-4c19-a4f0-64f3841dfc30}</MetaDataID>
        public string OQLName
        {
            get
            {
                return "[" + Name + "]";
            }
        }

        /// <exclude>Excluded</exclude>
        protected string _Name = "";
        ///// <MetaDataID>{38B4607C-D8CF-4543-BA96-B60BE0FE8674}</MetaDataID>
        ///// <exclude>Excluded</exclude>
        //protected string _Name
        //{
        //    get
        //    {
        //        return __Name;
        //    }
        //    set
        //    {
        //        __Name = value;
        //    }
        //}
        /// <MetaDataID>{6BC2A77A-7400-4540-92EA-E5C71B69D996}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Name")]
        public virtual string Name
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Name;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_Name != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Name = value;
                            _CaseInsensitiveName = null;
                            StateTransition.Consistent = true; ;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }

        /// <MetaDataID>{71c05f87-ef05-4838-a0f7-e82a2fd2ea6a}</MetaDataID>
        public string GetDynamicPropertiesAsXmlString()
        {
            if (XMLDynamicProperties != null)
                return XMLDynamicProperties.ToString();
            return ""; ;
        }
        /// <MetaDataID>{FAD84C5E-1A1B-42F2-BBC7-1A044ACDA6B0}</MetaDataID>
        [BackwardCompatibilityID("+31")]
        [PersistentMember()]
        protected XDocument XMLDynamicProperties;
        /// <MetaDataID>{4730F291-05CD-4836-BFD7-B75FFE49D474}</MetaDataID>
        public virtual string FullName
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_Namespace.Value != null)
                        return _Namespace.Value.FullName + "." + Name;
                    else
                        return Name;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();

                }
            }
        }





        /// <MetaDataID>{e674e443-cd55-43e6-aed6-655a9c1d58c5}</MetaDataID>
        public string OQLFullName
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_Namespace.Value != null)
                        return _Namespace.Value.OQLFullName + "." + OQLName;
                    else
                        return OQLName;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();

                }
            }
        }
        /// <MetaDataID>{EC372A10-16FC-4B32-9F52-97F1C80583F1}</MetaDataID>
        protected virtual internal void SetIdentity(MetaObjectID theIdentity)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                lock (identityLock)
                {

                    if (_Identity != null && _Identity != theIdentity)
                        throw new System.Exception("The identity of Meta Object is already set.");
                    _Identity = new MetaObjectID(theIdentity.ToString());
                }
                MetaObjectIDStream = theIdentity.ToString();
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{20EDEDAF-A158-481C-8002-26ED60720FBF}</MetaDataID>
        [BackwardCompatibilityID("+23")]
        [PersistentMember()]
        protected string MetaObjectIDStream;
        /// <MetaDataID>{23904521-21E5-4369-9AB6-3B4F91906C02}</MetaDataID>
        public virtual void Synchronize(MetaObject originMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_Name != originMetaObject.Name)
                {
                    _Name = originMetaObject.Name;
                    _CaseInsensitiveName = null;
                }

                Visibility = originMetaObject.Visibility;

                string xml = originMetaObject.GetDynamicPropertiesAsXmlString();
                if (string.IsNullOrEmpty(xml))
                {
                    if (XMLDynamicProperties != null)
                        XMLDynamicProperties = null;
                }
                else
                {
                    //if (XMLDynamicProperties == null)
                    //    XMLDynamicProperties =  new System.Xml.XmlDocument();
                    XMLDynamicProperties = XDocument.Parse(xml);
                }


                ContainedItemsSynchronizer mSynchronizer = null;
                if (originMetaObject.ClientDependencies == null)
                    mSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(new System.Collections.Generic.List<object>(), _ClientDependencies, this);
                else
                    mSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originMetaObject.ClientDependencies, _ClientDependencies, this);
                mSynchronizer.FindModifications();
                mSynchronizer.ExecuteAddCommand();
                mSynchronizer.ExecuteDeleteCommand();
                mSynchronizer.Synchronize();

                if (_Namespace == null && PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties) != null)
                    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Namespace", typeof(MetaDataRepository.MetaObject));
                if ((_Namespace.Value == null|| originMetaObject.Namespace?.FullName!= Namespace?.FullName) && originMetaObject.Namespace != null)
                {
                    _Namespace.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originMetaObject.Namespace, this) as MetaDataRepository.Namespace;


                    if (_Namespace.Value == null)
                    {
                        _Namespace.Value = (Namespace)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.Namespace, this);
                        if (_Namespace.Value != null)
                            _Namespace.Value.ShallowSynchronize(originMetaObject.Namespace);
                    }
                    if (_Namespace.Value != null)
                        _Namespace.Value.AddOwnedElement(this);
                }



                if (_ImplementationUnit.Value == null && originMetaObject.ImplementationUnit != null)
                {
                    _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originMetaObject.ImplementationUnit, this) as MetaDataRepository.Component;
                    if (_ImplementationUnit.Value == null)
                        _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.ImplementationUnit, this);
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        protected object identityLock = new object();
        /// <MetaDataID>{BB7B1145-5377-48CF-B387-76240243AD83}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected MetaObjectID _Identity;
        /// <summary>Είναι η ταυτότητα του αντικειμένου. 
        /// Λόγο του ότι ένα μεταμοντέλο μπορεί να αναπαραχθεί σε πολλά μεταμοντέλα 
        /// όπως (μεταμοντέλο που χρησιμεύη για την αποθήκευση αντικειμένων 
        /// σε σχεσιακή βάση δεδομένων, σε μεταμοντέλο για την αποθήκευση αντικειμένων 
        /// σε XML αρχείο) ή να εξεληχθεί και να παράγει πολές εκδόσης, 
        /// είναι απαραίτητη η ταυτότητα στα MetaObject για λόγου συνγρονισμού. </summary>
        /// <MetaDataID>{9124FEAF-E867-478F-8BB7-EAF476B30C77}</MetaDataID>
        public virtual MetaObjectID Identity
        {
            get
            {


                lock (identityLock)
                {
                    if (_Identity == null)
                    {
                        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                        try
                        {
                            if (MetaObjectIDStream != null && MetaObjectIDStream.Trim().Length > 0)
                                _Identity = new MetaDataRepository.MetaObjectID(MetaObjectIDStream);
                            else
                                if (Namespace == null)
                                _Identity = new MetaDataRepository.MetaObjectID(FullName);
                            else
                                _Identity = new MetaDataRepository.MetaObjectID(Namespace.Identity + "." + Name);
                        }
                        finally
                        {
                            ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                        }
                    }
                    return _Identity;

                }


            }
        }

        /// <summary>Return extension meta object with type MetaObjectType. 
        /// For example System.Reflection.FieldInfo or System.Type. </summary>
        /// <MetaDataID>{B01F89A6-11D3-4CC2-BF63-26D34898BDA3}</MetaDataID>
        public virtual object GetExtensionMetaObject(System.Type MetaObjectType)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                foreach (object CurrExtensionMetaObject in GetExtensionMetaObjects())
                {
                    if (MetaObjectType.GetMetaData().IsInstanceOfType(CurrExtensionMetaObject))
                        return CurrExtensionMetaObject;
                }
                return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
        /// <MetaDataID>{59ad1406-d9ae-4f3e-88d0-9bdc8b8e6319}</MetaDataID>
        protected object ExtensionMetaObjectsLock = new object();
        /// <summary>Return all extension meta objects</summary>
        /// <remarks>
        /// Extension meta object is meta data from other type of meta data repository 
        /// which refer to the same code element as the origin meta object.
        /// For instance if call this function to a class may be retrieve a 
        /// System.Type object from the System.Reflection repository or 
        /// RDBMSMetaDataRepository.Class from RDBMSMetaDataRepository repository.
        /// System.Reflection metadata live in dlls and 
        /// RDBMSMetaDataRepository metadata live in RDBMS databases.     
        /// </remarks>
        /// <MetaDataID>{167492AA-1180-4405-981F-DD21966845A6}</MetaDataID>
        public abstract System.Collections.Generic.List<object> GetExtensionMetaObjects();
        /// <MetaDataID>{BDC95AFB-14F6-4EDE-AB02-8484B2DA9F3D}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Dependency> _ClientDependencies = new OOAdvantech.Collections.Generic.Set<Dependency>();
        /// <summary>Inverse of client. Designates a set of Dependency in which the Meta object is a client. </summary>
        /// <MetaDataID>{E1F28FF6-676B-4D7D-B2CA-F53EE2FF7A76}</MetaDataID>
        [Association("ClientDependency", typeof(OOAdvantech.MetaDataRepository.Dependency), Roles.RoleB, "{54E7CCBA-6C18-4E9A-A69B-7C26B1ACFF41}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_ClientDependencies")]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<Dependency> ClientDependencies
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _ClientDependencies;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{DAF3C85A-48DC-4769-A6CA-CCCA23147057}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Dependency> _SupplierDependencies = new OOAdvantech.Collections.Generic.Set<Dependency>();
        /// <summary>Inverse of supplier. Designates a set of Dependency in which the Meta object is a supplier. </summary>
        /// <MetaDataID>{8A85A6DC-0714-4E10-A240-259894E4D78A}</MetaDataID>
        [Association("SupplierDependency", typeof(OOAdvantech.MetaDataRepository.Dependency), Roles.RoleB, "{945370AF-442A-490A-A336-9EE77D07F621}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_SupplierDependencies")]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<Dependency> SupplierDependencies
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _SupplierDependencies.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{5887e15a-958c-4410-8b5c-8435f36404d0}</MetaDataID>
        protected readonly object NamespaceLock = new object();

        /// <MetaDataID>{02BF6C92-AC48-4EEF-B6E1-74D530351EE8}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Member<Namespace> _Namespace = new Member<Namespace>();
        /// <MetaDataID>{2C4B4E3C-9816-41A6-AED3-083BE397C5A2}</MetaDataID>
        [Association("NamespaceMember", typeof(OOAdvantech.MetaDataRepository.Namespace), Roles.RoleB, "{A70335FA-743A-40DF-B6CB-FF0F4481484C}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Namespace")]
        [RoleBMultiplicityRange(0, 1)]
        public virtual Namespace Namespace
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Namespace;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{1E2A8129-7965-4DF2-BE43-C1A3A6A0C487}</MetaDataID>
        internal protected virtual void SetNamespace(Namespace mNamespace)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                lock (identityLock)
                {
                    if (_Identity != null && _Identity.ToString() == FullName)
                    {
                        if (_Namespace.Value != null && _Namespace.Value != mNamespace)
                        {
                            //TODO Αυτή η γραμμή κώδικα μπορεί να δημιουργήσει προβλήματα.
                            _Identity = null;
                        }
                    }
                }
                _Namespace.Value = mNamespace;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <summary>Specifies the visibility of the Meta object from the viewpoint of the Container Possibilities are:
        /// Public - Client may navigate to the instance of Meta object.
        /// Protected - Descendants of the source Container may navigate to the instance of Meta object.
        /// Private - Only the Container may navigate to the instance of Meta object.
        /// Package - Meta objects with the same Container with the Container of Meta object may navigate to the instance of Meta object. </summary>
        /// <MetaDataID>{70F496D4-C108-4C0F-BD48-2283BBB23D10}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember()]
        public VisibilityKind Visibility;

        /// <MetaDataID>{05cb287a-ca3c-4dfe-802f-3a9631e329fc}</MetaDataID>
        public T GetExtensionMetaObject<T>()
        {
            object value = GetExtensionMetaObject(typeof(T));
            if (value == null)
                return default(T);
            else
                return (T)value;
        }
        /// <MetaDataID>{6380b4af-0bea-4468-a56e-275c4e6dbbe5}</MetaDataID>
        public T GetPropertyValue<T>(string propertyNamespace, string propertyName)
        {
            object value = GetPropertyValue(typeof(T), propertyNamespace, propertyName);
            if (value == null)
                return default(T);
            else
                return (T)value;

        }


        /// <summary>This function retrieves the current value of a property of an metaobject, given a property and group name. </summary>
        /// <param name="propertyNamespace">Name of the Namespace for which a property value is being retrieved </param>
        /// <param name="propertyName">Name of the property whose value is being retrieved </param>
        /// <MetaDataID>{2E15D8FD-12B7-43CA-89C1-DCC8EEB5C75E}</MetaDataID>
        public virtual object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (XMLDynamicProperties == null)
                    return null;
                if (XMLDynamicProperties.Elements().Count() == 0)
                    return null;
                XElement XMLTextElement = XMLDynamicProperties.Root;
                XElement NamespaceElement = null;
                foreach (XElement element in XMLTextElement.Elements())
                {
                    if (element.Name == propertyNamespace)
                    {
                        NamespaceElement = element;
                        break;
                    }
                }
                if (NamespaceElement == null)
                    return null;

                string PropertyValue = null;
                if (NamespaceElement.Attribute(propertyName) != null)
                    PropertyValue = NamespaceElement.Attribute(propertyName).Value;

                if (PropertyValue == null || PropertyValue.Length == 0)
                    return null;

                if (propertyType == PropertyValue.GetType())
                    return PropertyValue;
                if (propertyType.GetMetaData().BaseType == typeof(System.Enum))
                {
                    if (PropertyValue != null)
                        if (PropertyValue.Length > 0)
                            return System.Enum.Parse(propertyType, PropertyValue, false);
                    return null;
                }
                else
                {
                    if (propertyType == typeof(System.DateTime))
                        return System.Convert.ChangeType(PropertyValue, propertyType, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                    else
                        return System.Convert.ChangeType(PropertyValue, propertyType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        /// <summary>This method set the value of a metaobject property. </summary>
        /// <param name="PropertyName">Name of the property whose value is being retrieved </param>
        /// <param name="PropertyValue">Value being set </param>
        /// <MetaDataID>{D4CA1F25-67C8-4C39-945B-1C55988621ED}</MetaDataID>
        public virtual void PutPropertyValue(string PropertyNamespace, string PropertyName, object PropertyValue)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                if (PropertyValue == null)
                    return;
                if (XMLDynamicProperties == null)
                    XMLDynamicProperties = new XDocument();
                XElement XMLTextElement = null;
                XElement NamespaceElement = null;
                if (XMLDynamicProperties.Nodes().Count() == 0)
                {
                    XMLTextElement = new XElement("XMLText");
                    XMLDynamicProperties.Add(XMLTextElement);//.AppendChild(XMLTextElement);
                }
                else
                    XMLTextElement = XMLDynamicProperties.Elements().ToArray()[0];
                foreach (XElement element in XMLTextElement.Elements())
                {
                    if (element.Name == PropertyNamespace)
                    {
                        NamespaceElement = element;
                        break;
                    }
                }
                if (NamespaceElement == null)
                {
                    NamespaceElement = new XElement(PropertyNamespace);
                    XMLTextElement.Add(NamespaceElement);
                }
                if (PropertyValue != null)
                {
                    string StringValue;
                    if (PropertyValue.GetType().GetMetaData().BaseType == typeof(System.Enum))
                    {
                        StringValue = PropertyValue.ToString();
                        NamespaceElement.SetAttributeValue(PropertyName, StringValue);
                    }
                    else
                    {
                        StringValue = PropertyValue.ToString();

                        NamespaceElement.SetAttributeValue(PropertyName, StringValue);

                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        //public virtual object GetMemberValue(object token, MemberInfo member)
        //{


        //    if (member.Name == nameof(Visibility))
        //        return Visibility;
        //    return ObjectMemberGetSet.MemberValueGetFailed;
        //}

        /// <MetaDataID>{65547365-a6a9-40e3-a2b7-a9921e7a85e7}</MetaDataID>
        public virtual ObjectMemberGetSet SetMemberValue(object token, MemberInfo member, object value)
        {
            if (member.Name == nameof(_CaseInsensitiveName))
            {
                if (value == null)
                    _CaseInsensitiveName = default(string);
                else
                    _CaseInsensitiveName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ImplementationUnit))
            {
                if (value == null)
                    _ImplementationUnit = default(OOAdvantech.Member<OOAdvantech.MetaDataRepository.Component>);
                else
                    _ImplementationUnit = (OOAdvantech.Member<OOAdvantech.MetaDataRepository.Component>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(LockObject))
            {
                if (value == null)
                    LockObject = default(string);
                else
                    LockObject = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ReaderWriterLock))
            {
                if (value == null)
                    ReaderWriterLock = default(OOAdvantech.Synchronization.ReaderWriterLock);
                else
                    ReaderWriterLock = (OOAdvantech.Synchronization.ReaderWriterLock)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Identities))
            {
                if (value == null)
                    Identities = default(System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    Identities = (System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Properties))
            {
                if (value == null)
                    Properties = default(OOAdvantech.ObjectStateManagerLink);
                else
                    Properties = (OOAdvantech.ObjectStateManagerLink)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Name))
            {
                if (value == null)
                    _Name = default(string);
                else
                    _Name = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(XMLDynamicProperties))
            {
                if (value == null)
                    XMLDynamicProperties = default(System.Xml.Linq.XDocument);
                else
                    XMLDynamicProperties = (System.Xml.Linq.XDocument)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(MetaObjectIDStream))
            {
                if (value == null)
                    MetaObjectIDStream = default(string);
                else
                    MetaObjectIDStream = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            lock (identityLock)
            {
                if (member.Name == nameof(_Identity))
                {
                    if (value == null)
                        _Identity = default(OOAdvantech.MetaDataRepository.MetaObjectID);
                    else
                        _Identity = (OOAdvantech.MetaDataRepository.MetaObjectID)value;
                    return ObjectMemberGetSet.MemberValueSetted;
                }
            }
            if (member.Name == nameof(_ClientDependencies))
            {
                if (value == null)
                    _ClientDependencies = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>);
                else
                    _ClientDependencies = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_SupplierDependencies))
            {
                if (value == null)
                    _SupplierDependencies = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>);
                else
                    _SupplierDependencies = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Namespace))
            {
                if (value == null)
                    _Namespace = default(OOAdvantech.Member<OOAdvantech.MetaDataRepository.Namespace>);
                else
                    _Namespace = (OOAdvantech.Member<OOAdvantech.MetaDataRepository.Namespace>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Visibility))
            {
                if (value == null)
                    Visibility = default(OOAdvantech.MetaDataRepository.VisibilityKind);
                else
                    Visibility = (OOAdvantech.MetaDataRepository.VisibilityKind)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            return ObjectMemberGetSet.MemberValueSetFailed;
        }

        /// <MetaDataID>{1b754027-aebe-4126-96fb-e9a59190d583}</MetaDataID>
        public virtual object GetMemberValue(object token, MemberInfo member)
        {

            if (member.Name == nameof(_CaseInsensitiveName))
                return _CaseInsensitiveName;

            if (member.Name == nameof(_ImplementationUnit))
                return _ImplementationUnit;

            if (member.Name == nameof(LockObject))
                return LockObject;

            if (member.Name == nameof(ReaderWriterLock))
                return ReaderWriterLock;

            if (member.Name == nameof(Identities))
                return Identities;

            if (member.Name == nameof(Properties))
                return Properties;

            if (member.Name == nameof(_Name))
                return _Name;

            if (member.Name == nameof(XMLDynamicProperties))
                return XMLDynamicProperties;

            if (member.Name == nameof(MetaObjectIDStream))
                return MetaObjectIDStream;
            lock (identityLock)
            {
                if (member.Name == nameof(_Identity))
                    return _Identity;
            }

            if (member.Name == nameof(_ClientDependencies))
                return _ClientDependencies;

            if (member.Name == nameof(_SupplierDependencies))
                return _SupplierDependencies;

            if (member.Name == nameof(_Namespace))
                return _Namespace;

            if (member.Name == nameof(Visibility))
                return Visibility;

            return ObjectMemberGetSet.MemberValueGetFailed;
        }


        /// <MetaDataID>{2D266B08-B9E9-4349-93C6-A1949D6863E0}</MetaDataID>
        public MetaObject()
        {

            AccessorBuilder.TypeMetadata m_typeMetada = AccessorBuilder.LoadTypeMetadata(GetType());
            AccessorBuilder.FieldPropertyAccessor fastFieldPropertyAccessor = new AccessorBuilder.FieldPropertyAccessor();
            m_typeMetada.InitializationRequiredMember = fastFieldPropertyAccessor;
            AccessorBuilder.SetTypeMetadata(GetType(), m_typeMetada);
            m_typeMetada = AccessorBuilder.LoadTypeMetadata(GetType());





            //AccessorBuilder.InitObject(this);
        }



    }
}
