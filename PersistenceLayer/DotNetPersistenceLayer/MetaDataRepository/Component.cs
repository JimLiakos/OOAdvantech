using System.Reflection;
namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{31426AE4-54E2-4EF3-9D8F-C3A1F4072167}</MetaDataID>
    [BackwardCompatibilityID("{31426AE4-54E2-4EF3-9D8F-C3A1F4072167}")]
    [Persistent()]
    public class Component : Classifier
    {


        /// <exclude>Excluded</exclude>
        protected string _MappingVersion;

        /// <MetaDataID>{f3669a6f-6754-45a4-8e7f-702acc9fd037}</MetaDataID>
        [PersistentMember(nameof(_MappingVersion))]
        [BackwardCompatibilityID("+7")]
        public string MappingVersion
        {
            get
            {
                if (_MappingVersion == null)
                    return "";
                return _MappingVersion;
            }
            set
            {
                if (_MappingVersion != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MappingVersion = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{2e7f0151-e76c-48f6-a553-4c5da7e6508e}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, MemberInfo member, object value)
        {
            if (member.Name == nameof(GetGetComponentFastInvoke))
            {
                if (value == null)
                    GetGetComponentFastInvoke = default(OOAdvantech.AccessorBuilder.FastInvokeHandler);
                else
                    GetGetComponentFastInvoke = (OOAdvantech.AccessorBuilder.FastInvokeHandler)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Context))
            {
                if (value == null)
                    _Context = default(OOAdvantech.MetaDataRepository.Storage);
                else
                    _Context = (OOAdvantech.MetaDataRepository.Storage)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_AssemblyString))
            {
                if (value == null)
                    _AssemblyString = default(string);
                else
                    _AssemblyString = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(InErrorCheck))
            {
                if (value == null)
                    InErrorCheck = default(bool);
                else
                    InErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Residents))
            {
                if (value == null)
                    _Residents = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    _Residents = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{9914cdac-0133-4512-86dc-915b1a7d286e}</MetaDataID>
        public override object GetMemberValue(object token, MemberInfo member)
        {

            if (member.Name == nameof(GetGetComponentFastInvoke))
                return GetGetComponentFastInvoke;

            if (member.Name == nameof(_Context))
                return _Context;

            if (member.Name == nameof(_AssemblyString))
                return _AssemblyString;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(_Residents))
                return _Residents;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{1d2e3ce8-dcda-4089-aff8-5ac2de48d6a7}</MetaDataID>
        [IgnoreErrorCheck]
        [AssociationEndBehavior(PersistencyFlag.LazyFetching)]
        public override Component ImplementationUnit
        {
            get
            {
                return this;
            }
        }
        /// <MetaDataID>{024de589-99c6-45b3-8789-ae207be27d79}</MetaDataID>
        [IgnoreErrorCheck]
        [AssociationEndBehavior(PersistencyFlag.LazyFetching)]
        public override Namespace Namespace
        {
            get
            {
                return base.Namespace;
            }
        }


        /// <MetaDataID>{7d7cef09-8173-4c6e-aa88-d31cd82163ce}</MetaDataID>
        public virtual Class CreateClass(string className, string namespaceName)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{98ea6ece-7202-4464-984c-63ad111190d3}</MetaDataID>
        public virtual Interface CreateInterface(string interfaceName, string namespaceName)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{9c1933ab-f495-49e0-ac93-71acffb644b6}</MetaDataID>
        public virtual Structure CreateStructure(string structureName, string namespaceName)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{b685437e-c377-4faf-87fe-350015390f4e}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Enumeration CreateEnumeration(string enumerationName, string namespaceName)
        {
            throw new System.NotImplementedException();
        }





        /// <MetaDataID>{e9a24942-1b48-4d67-bcbe-9bd6ed0645fb}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler GetGetComponentFastInvoke;
        /// <MetaDataID>{d43406d4-78d7-4d0d-a21c-8b956259ba76}</MetaDataID>
        static Component()
        {

            //System.Windows.Forms.MessageBox.Show("dfdf");
            //#if Net4
            //            System.Type type = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c");
            //            if (type != null)
            //                GetGetComponentFastInvoke = AccessorBuilder.GetMethodInvoker(type.GetMethod("GetComponent"));
            //#else
#if !DeviceDotNet
            System.Type type = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository,  Culture=neutral, PublicKeyToken=11a79ce55c18c4e7");
            if (type != null)
                GetGetComponentFastInvoke = AccessorBuilder.GetMethodInvoker(type.GetMethod("GetComponent"));
#else
            System.Type type = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            if(type!=null)
                GetGetComponentFastInvoke = AccessorBuilder.GetMethodInvoker(type.GetMetaData().GetMethod("GetComponent"));
#endif
            //#endif
        }
        /// <MetaDataID>{68b2e27e-7de4-4952-81cf-7b6e9f7eb09a}</MetaDataID>
        public static Component GetComponent(System.Reflection.Assembly assembly)
        {
            return GetGetComponentFastInvoke.Invoke(null, new object[1] { assembly }) as Component;
        }

        /// <MetaDataID>{54eddc23-f3ff-4231-9104-382c28f82257}</MetaDataID>
        public virtual Classifier GetClassifier(string fullName, bool caseSensitive)
        {
            foreach (Classifier classifier in Residents)
            {
                if (caseSensitive && classifier.FullName == fullName)
                    return classifier;
                if (!caseSensitive && classifier.FullName.ToLower() == fullName.ToLower())
                    return classifier;
            }
            return null;
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{41B689A6-9004-4C8F-A343-7E6BF749EB63}</MetaDataID>
        private Storage _Context;
        /// <MetaDataID>{0C4BF278-4175-464B-B1D8-BCE5C2429E6C}</MetaDataID>
        [MetaDataRepository.Association("StorageComponent", typeof(OOAdvantech.MetaDataRepository.Storage), MetaDataRepository.Roles.RoleB, "{32C3C895-BD31-4CB4-983A-3D342EF936E1}")]
        [MetaDataRepository.AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Context")]
        [RoleBMultiplicityRange(0, 1)]
        public Storage Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;

            }
        }


        /// <exclude>Excluded</exclude>
        protected string _AssemblyString = "";

        /// <MetaDataID>{0325d424-2532-46a2-8384-098f52ca99bc}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_AssemblyString")]
        public virtual string AssemblyString
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _AssemblyString;
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
                    if (_AssemblyString != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _AssemblyString = value;
                            StateTransition.Consistent = true;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }

        /// <MetaDataID>{ae177a9f-7a8b-4a84-be69-abb854508ead}</MetaDataID>
        [IgnoreErrorCheck]
        [AssociationEndBehavior(PersistencyFlag.LazyFetching)]
        public override Association LinkAssociation
        {
            get
            {
                return base.LinkAssociation;
            }
            set
            {
                base.LinkAssociation = value;
            }
        }






        /// <MetaDataID>{67F45CB6-3CF4-4731-83E2-54D7B4C7A60C}</MetaDataID>
        /// <summary>HasPersistentClasses defines a flag.
        /// If there are persistent classes in component the flag is true otherwise is false. </summary>
        public virtual bool HasPersistentClasses
        {
            get
            {
                foreach (MetaObject metaObject in Residents)
                {
                    if ((metaObject is Class) && (metaObject as Class).Persistent)
                        return true;
                }
                return false;
            }
        }
        /// <MetaDataID>{C8BA631C-2377-429D-93A5-41138D5EC622}</MetaDataID>
        /// <summary>InErrorCheck attribute protected the ErrorCheck method from recursive call. </summary>
        private bool InErrorCheck = false;


        /// <MetaDataID>{9E590595-D38F-4B44-B967-019D756C12C2}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            if (InErrorCheck)
                return false;
            try
            {
                InErrorCheck = true;
                bool hasError = base.ErrorCheck(ref errors);
                foreach (MetaObject metaObject in Residents)
                    hasError |= metaObject.ErrorCheck(ref errors);
                return hasError;
            }
            catch (System.Exception error)
            {
                errors.Add(new MetaObject.MetaDataError("MDR Error: " + error.Message + " " + error.StackTrace, "(A:)" + FullName));
                return true;
            }
            finally
            {
                InErrorCheck = false;
            }
        }




        /// <MetaDataID>{7B956F48-4265-427F-8514-C805326B5D21}</MetaDataID>
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        public override void Synchronize(MetaObject OriginMetaObject)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (SynchronizerSession.IsSynchronized(this))
                    return;
                SynchronizerSession.MetaObjectUnderSynchronization(this);
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Component mSourceComponent = (Component)OriginMetaObject;
                    int count = mSourceComponent.Residents.Count;
                    if (_Identity == null)
                    {
                        _Identity = new MetaObjectID(OriginMetaObject.Identity.ToString());
                        MetaObjectIDStream = _Identity.ToString();
                    }
                    AssemblyString = (OriginMetaObject as OOAdvantech.MetaDataRepository.Component).AssemblyString;
                    base.Synchronize(OriginMetaObject);
                    OOAdvantech.Collections.Generic.Set<MetaObject> residents = new OOAdvantech.Collections.Generic.Set<MetaObject>(_Residents);
                    ContainedItemsSynchronizer mSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(mSourceComponent.Residents, _Residents, this);
                    mSynchronizer.FindModifications();
                    mSynchronizer.ExecuteAddCommand();
                    mSynchronizer.ExecuteDeleteCommand();
                    mSynchronizer.Synchronize();
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{19dff7cc-7dfb-476b-aba6-c5567a02ac18}</MetaDataID>
        public virtual void AddResident(MetaObject metaObject)
        {
            if (metaObject == null)
                return;

            if (!_Residents.Contains(metaObject))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Residents.Add(metaObject);
                    stateTransition.Consistent = true;
                }
            }

        }
        /// <MetaDataID>{200ab817-bf58-48ab-98fd-4aee2870f73d}</MetaDataID>
        public virtual void RemoveResident(MetaObject metaObject)
        {
            if (metaObject == null)
                return;

            if (_Residents.Contains(metaObject))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Residents.Remove(metaObject);
                    stateTransition.Consistent = true;
                }


            }

        }

        /// <MetaDataID>{A7A1DA53-F796-499B-8414-8BA0D0CB4A9F}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<MetaObject> _Residents = new OOAdvantech.Collections.Generic.Set<MetaObject>();

        /// <MetaDataID>{32776610-97E1-49ED-ABEB-D87B8638DDC9}</MetaDataID>
        /// <summary>
        /// Defines a collection with Meta objects which live in component. 
        /// </summary>
        [MetaDataRepository.Association("Implementation", typeof(OOAdvantech.MetaDataRepository.MetaObject), MetaDataRepository.Roles.RoleA, "{8FAF0E7A-D1F5-42A0-9887-25790C5913BB}")]
        [MetaDataRepository.AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Residents")]
        [RoleAMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<MetaObject> Residents
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Residents.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
    }
}

