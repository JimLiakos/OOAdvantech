
using System.Linq;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{03F6215E-82ED-4CF9-A26C-34FF630F1D67}</MetaDataID>
    public class Enumeration : MetaDataRepository.Enumeration
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

        /// <MetaDataID>{cfd02120-aeea-42e8-b104-1ec493139170}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }

        /// <MetaDataID>{A6E875D9-DD93-451C-8CD8-163255541FB5}</MetaDataID>
		protected Enumeration()
        {




        }
        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{9216dd24-c303-4739-a70b-c3181ed83f29}</MetaDataID>
        public override string FullName
        {
            get
            {
                if (_FullName == null)
                    _FullName = base.FullName;
                return _FullName;
            }
        }

        /// <MetaDataID>{54191d32-c79b-45ad-a8dc-74e80a50cc3e}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;
        /// <MetaDataID>{5e44b7ca-b138-4a28-b921-70b74edf8537}</MetaDataID>
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


        /// <MetaDataID>{224BF7CD-5A70-4FB2-9FEB-031030C049FE}</MetaDataID>
        public override MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }
        /// <MetaDataID>{5BA9E8E0-F184-45F8-B1F8-0623AB606523}</MetaDataID>
        public Enumeration(Type theWrType)
        {
            _ImplementationUnit.Value = Assembly.GetComponent(theWrType.WrType.GetMetaData().Assembly);
            Refer = theWrType;
            _Name = Refer.Name;

            if (!string.IsNullOrEmpty(theWrType.WrType.Namespace))
            {
                Namespace mNamespace = Type.GetNameSpace(theWrType.WrType.Namespace);
                mNamespace.AddOwnedElement(this);
                SetNamespace(mNamespace);
            }

            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType, this);
            MetaDataRepository.MetaObjectID oldIdentity = null;
            _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);
            MetaObjectMapper.AddMetaObject(this, Refer.WrType.FullName);


        }

        /// <MetaDataID>{979D5B00-F2BC-4A23-A7AC-3E6ACCB80AF5}</MetaDataID>
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
        /// <MetaDataID>{FE811F67-6A18-4DCD-9679-8981DA124405}</MetaDataID>
        internal Type Refer;
    }
}
