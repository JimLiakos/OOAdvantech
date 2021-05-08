namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{493F5EDE-1808-4A90-BDE0-08CE8523F31A}</MetaDataID>
    public class Namespace : OOAdvantech.MetaDataRepository.Namespace
    {

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string));
            }
            else
            {
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }

        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }
        /// <MetaDataID>{288c5fc7-dd29-44f5-ba9b-af0acd451cab}</MetaDataID>
        internal void RefreshCodeElement(EnvDTE.CodeNamespace vsNamespace)
        {
            MetaObjectMapper.RemoveMetaObject(this);
            VSNamespace = vsNamespace;

            MetaObjectMapper.AddTypeMap(VSNamespace.FullName,this);
            MetaObjectMapper.AddTypeMap(VSNamespace, this);

            int nPos = vsNamespace.FullName.LastIndexOf(".");
            if (nPos != -1)
            {
                _Name = vsNamespace.FullName.Substring(nPos + 1);
                string parentNamespaceFullName = vsNamespace.FullName.Substring(0, nPos);
                Namespace parentNamespace = MetaObjectMapper.FindMetaObjectFor(parentNamespaceFullName) as Namespace;

                if (parentNamespace == null)
                    parentNamespace = new Namespace(parentNamespaceFullName);
                SetNamespace(parentNamespace);
                parentNamespace.AddOwnedElement(this);
            }
            else
                _Name = VSNamespace.Name;


        }
        /// <MetaDataID>{2BA192AC-5C45-4AE6-90C0-433D7564EA34}</MetaDataID>
        internal EnvDTE.CodeNamespace VSNamespace;
        /// <MetaDataID>{551D4D56-EF86-4169-80B1-6D10E44B59C8}</MetaDataID>
        public Namespace(EnvDTE.CodeNamespace vsNamespace)
        {
            VSNamespace = vsNamespace;

            MetaObjectMapper.AddTypeMap(VSNamespace.FullName, this);

            MetaObjectMapper.AddTypeMap(VSNamespace, this);

            int nPos = vsNamespace.FullName.LastIndexOf(".");
            if (nPos != -1)
            {
                _Name = vsNamespace.FullName.Substring(nPos + 1);
                string parentNamespaceFullName = vsNamespace.FullName.Substring(0,nPos );
                Namespace parentNamespace = MetaObjectMapper.FindMetaObjectFor(parentNamespaceFullName) as Namespace;

                if (parentNamespace == null)
                    parentNamespace = new Namespace(parentNamespaceFullName);
                SetNamespace(parentNamespace);
                parentNamespace.AddOwnedElement(this);
            }
            else
                _Name = VSNamespace.Name;
        }
        /// <MetaDataID>{24a1d68a-5f6b-4422-8abc-042d0efa8d71}</MetaDataID>
        public Namespace(string NamespaceName)
        {
            int nPos = NamespaceName.LastIndexOf('.');
            if (nPos != -1)
            {
                _Name = NamespaceName.Substring(nPos + 1);
                string NewNameSpaceName = NamespaceName.Substring(0, nPos);
                Namespace NewNameSpace = MetaObjectMapper.FindMetaObjectFor(NewNameSpaceName) as Namespace;
                if (NewNameSpace == null)
                    NewNameSpace = new Namespace(NewNameSpaceName);
                _Namespace.Value = NewNameSpace;
                NewNameSpace.AddOwnedElement(this);
            }
            else
                _Name = NamespaceName;
            MetaObjectMapper.AddTypeMap(FullName, this);
            
        }


        /// <MetaDataID>{F4DBA740-C1A7-4623-B2DF-A8A933CD8910}</MetaDataID>
        private void GetParentNamespaces(EnvDTE.CodeElements codeElements)
        {
            foreach (EnvDTE.CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementNamespace &&
                    VSNamespace.FullName.IndexOf(codeElement.FullName) == 0)
                {
                    Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(codeElement.FullName) as Namespace;
                    if (_namespace == null)
                        _namespace = new Namespace(codeElement as EnvDTE.CodeNamespace);
                    GetParentNamespaces((codeElement as EnvDTE.CodeNamespace).Members);
                    return;
                }
            }
        }

        public override void AddOwnedElement(OOAdvantech.MetaDataRepository.MetaObject ownedElement)
        {
            base.AddOwnedElement(ownedElement);
            if (ownedElement is MetaDataRepository.Classifier && (ownedElement as MetaDataRepository.Classifier).TemplateBinding != null)
                return;
            MetaObjectChangeState();
        }
        public override void RemoveOwnedElement(OOAdvantech.MetaDataRepository.MetaObject ownedElement)
        {
            base.RemoveOwnedElement(ownedElement);
            MetaObjectChangeState();
        }
        /// <MetaDataID>{BA728FFE-46E6-4E8C-8203-D1516FB45D35}</MetaDataID>
        internal void SetNamespace(Namespace parentcodeNamespace)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_Namespace.Value != null)
                    _Namespace.Value.RemoveOwnedElement(this);
                _Namespace.Value = parentcodeNamespace;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
    }
}
