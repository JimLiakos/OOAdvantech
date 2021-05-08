using System.Linq;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{DC9F97E5-C304-44C5-B442-F42A0BCC11AE}</MetaDataID>
    public class Primitive : MetaDataRepository.Primitive
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_FullName))
            {
                if (value == null)
                    _FullName = default(string);
                else
                    _FullName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ExtensionMetaObjects))
            {
                lock (ExtensionMetaObjectsLock)
                {
                    if (value == null)
                        ExtensionMetaObjects = default(System.Collections.Generic.List<object>);
                    else
                        ExtensionMetaObjects = (System.Collections.Generic.List<object>)value; 
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Refer))
            {
                if (value == null)
                    Refer = default(OOAdvantech.DotNetMetaDataRepository.Type);
                else
                    Refer = (OOAdvantech.DotNetMetaDataRepository.Type)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_FullName))
                return _FullName;

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(Refer))
                return Refer;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{8239cb8f-d348-4379-b628-3605e635bd6f}</MetaDataID>
        public override string FullName
        {
            get
            {
                if (_FullName == null)
                    _FullName = base.FullName;
                return _FullName;
            }
        }

        /// <MetaDataID>{501f02f7-57c4-4ed6-93b9-1c3b0a3504e4}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{7F05C0BF-0675-4B7B-9736-E04D312F3989}</MetaDataID>
		protected Primitive()
        {
        }

        /// <MetaDataID>{b3ad6d51-48f4-430c-beba-c2e230fda2ee}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;
        /// <MetaDataID>{73171b1f-2078-425c-ada4-8a28826f22e4}</MetaDataID>
        public void AddExtensionMetaObject(object Value)
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                    GetExtensionMetaObjects();
                if (!ExtensionMetaObjects.Contains(Value))
                    ExtensionMetaObjects.Add(Value); 
            }
        }

        /// <MetaDataID>{6a0a217c-d098-4704-87ca-3041c91f7e94}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.Generic.List<object>();
                    ExtensionMetaObjects.Add(Refer.WrType);
                }
                return ExtensionMetaObjects.ToList(); 
            }
        }


        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{AB42DFC6-8663-43E3-BB22-A7D72DBE09B8}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }

        /// <MetaDataID>{B7AA2F9B-BB26-4786-B512-E1742DF78DB3}</MetaDataID>
        public Primitive(Type theWrType)
        {
            Refer = theWrType;
            _ImplementationUnit.Value = Assembly.GetComponent(theWrType.WrType.GetMetaData().Assembly);
            _Name = Refer.Name;
            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType, this);

            if (!string.IsNullOrEmpty(theWrType.WrType.Namespace))
            {
                Namespace mNamespace = Type.GetNameSpace(Refer.WrType.Namespace);
                //Namespace mNamespace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(Refer.WrType.Namespace);
                //if (mNamespace == null)
                //    mNamespace = new Namespace(Refer.WrType.Namespace);
                mNamespace.AddOwnedElement(this);
                SetNamespace(mNamespace);
            }

            _Generalizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>();
            _Features = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>();
            _Roles = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>();
            _Specializations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>();
            _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);
            MetaObjectMapper.AddMetaObject(this, Refer.WrType.FullName);

        }
        /// <MetaDataID>{B2942194-7866-4E9A-9ABD-1C74E31686C6}</MetaDataID>
        public void SetNamespace(MetaDataRepository.Namespace OwnerNamespace)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                _Namespace.Value = OwnerNamespace;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


        /// <MetaDataID>{58169265-9FC3-4B2B-9EB8-62D778A0DC61}</MetaDataID>
        internal Type Refer;
    }
}
