namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{9B4004FE-C2E3-414C-8A72-41EFF24BF4D6}</MetaDataID>
    public class Enumeration : MetaDataRepository.Enumeration, CodeElementContainer
    {

        /// <MetaDataID>{0d1be2ed-70a4-4746-9197-9ad8b2ebe525}</MetaDataID>
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSCodeEnum as EnvDTE.CodeElement, identity);
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
            else
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
        }


         /// <MetaDataID>{2eabb4b6-4f0d-4b96-85a5-5cd3feaacd6e}</MetaDataID>
        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                string identity = base.GetPropertyValue(propertyType, propertyNamespace, propertyName) as string;

                if (identity != null)
                    return identity;
                if (VSCodeEnum == null || TemplateBinding != null)
                    return base.Identity;
                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSCodeEnum as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSCodeEnum as EnvDTE.CodeElement, identity);
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                }
                catch
                {
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }

        /// <MetaDataID>{58766c11-3e76-4e90-9e8b-4c421f7506e9}</MetaDataID>
        MetaDataRepository.MetaObjectID CodeElementContainer.Identity
        {
            get
            {
                string identity = GetPropertyValue(typeof(string), "MetaData",  "MetaObjectID") as string;
                if (!string.IsNullOrEmpty(identity))
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(identity);
                else
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
            }
        }
        /// <MetaDataID>{e238def4-be15-406d-a466-c42e4c221ff1}</MetaDataID>
        public string CurrentProgramLanguageFullName
        {
            get
            {
                return LanguageParser.GetTypeFullName(this, _ProjectItem.Project.VSProject);
            }
        }

        /// <MetaDataID>{af2a4478-6b07-49c9-8644-aa50fe91d61d}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return LanguageParser.GetTypeName(this, _ProjectItem.Project.VSProject);
            }
        }

        /// <MetaDataID>{da068afc-47e4-4f67-ad3d-73237a5eeedc}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);

            if (_Namespace.Value != null)
            {
                _Namespace.Value.RemoveOwnedElement(this);
                _Namespace.Value = null;
            }
            ImplementationUnit.RemoveResident(this);
            VSCodeEnum = null;
            MetaObjectMapper.RemoveMetaObject(this);
            
        }
        /// <MetaDataID>{919f5622-63fd-4c3b-aca1-3cebc357c288}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }


        /// <MetaDataID>{57c1f3a0-c881-4290-b0e2-d677e50a2835}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            MetaObjectMapper.RemoveMetaObject(this);
            VSCodeEnum = codeElement as EnvDTE.CodeEnum;

            _Identity = null;
            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID); 


            MetaObjectMapper.AddTypeMap(VSCodeEnum, this);
            Visibility = VSAccessTypeConverter.GetVisibilityKind(VSCodeEnum.Access);
            OwnedTemplateSignature = null;
            _Name = VSCodeEnum.Name;

            if (VSCodeEnum.Namespace != null)
            {
                Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSCodeEnum.Namespace.FullName);
                if (_Namespace.Value != mNamespace)
                {
                    if (_Namespace.Value != null)
                        _Namespace.Value.RemoveOwnedElement(this);

                    _Namespace.Value = null;
                    if (mNamespace == null)
                        mNamespace = new Namespace(VSCodeEnum.Namespace);
                    mNamespace.AddOwnedElement(this);
                    SetNamespace(mNamespace);
                }
            }
            else
            {
                if (_Namespace.Value != null)
                    _Namespace.Value.RemoveOwnedElement(this);
                _Namespace.Value = null;
            }
            RefreshStartPoint();
            MetaObjectChangeState();
        }


        /// <MetaDataID>{da458091-72dc-4f7a-bfd3-9e3349dde439}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSCodeEnum as EnvDTE.CodeElement;
            }
        }
        /// <MetaDataID>{83383D92-D812-4A61-92AA-90367AA6B72C}</MetaDataID>
        private EnvDTE.CodeEnum VSCodeEnum;


        /// <MetaDataID>{3D957710-DD61-4AF5-AA44-20DF3760F8D2}</MetaDataID>
        private EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{48E893A2-26B3-446C-9B11-65DEB8758C1E}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{EDA476C3-C2B2-4E7F-B02E-A74DA1255F58}</MetaDataID>
        public Enumeration(EnvDTE.CodeEnum codeEnum)
        {
            //TODO να ελεγχθεί όταν είναι nested
            VSCodeEnum = codeEnum;
            _Name = codeEnum.Name;
            _Kind = codeEnum.Kind;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(VSCodeEnum as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID); 



            _ImplementationUnit.Value = MetaObjectMapper.FindMetaObjectFor(VSCodeEnum.ProjectItem.ContainingProject) as Project;
            Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSCodeEnum.Namespace.FullName);
            if (mNamespace == null)
                mNamespace = new Namespace(VSCodeEnum.Namespace);
            mNamespace.AddOwnedElement(this);
            SetNamespace(mNamespace);
            MetaObjectMapper.AddTypeMap(VSCodeEnum,this);
            _ProjectItem = ProjectItem.AddMetaObject(VSCodeEnum.ProjectItem, this);
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        /// <MetaDataID>{74026cd1-b2ac-4f51-9ed2-598533f9030b}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }
        /// <MetaDataID>{883f5c93-b0a1-4db2-836f-6202af945df6}</MetaDataID>
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
            string ert = FullName;
        }

        /// <MetaDataID>{87e9a5cf-7bac-48c4-9294-45438dca23a6}</MetaDataID>
        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSCodeEnum.StartPoint.LineCharOffset;


                if (_Line != VSCodeEnum.StartPoint.Line)
                {
                    _Line = VSCodeEnum.StartPoint.Line;
                   // MetaObjectChangeState();
                }
            }
            catch (System.Exception error)
            {
            }
        }


        /// <MetaDataID>{105f8ccc-2fcd-4239-b6df-c844175db1a6}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;

            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementClass)
                return false;
            if (this.VSCodeEnum == codeElement)
                return true;


            try
            {
                if (itsParent is EnvDTE.CodeNamespace)
                {
                    string fullName = FullName;
                    if (OwnedTemplateSignature != null)
                    {
                        int nPos = fullName.IndexOf("`");
                        if (nPos > 0)
                            fullName = fullName.Substring(0, nPos);
                    }

                    if (fullName == (itsParent as EnvDTE.CodeNamespace).FullName + "." + codeElement.Name)
                        return true;
                }
                else
                {
                    string fullName = FullName;
                    if (OwnedTemplateSignature != null)
                    {
                        int nPos = fullName.IndexOf("`");
                        if (nPos > 0)
                            fullName = fullName.Substring(0, nPos);
                    }
                    if (codeElement.Name == fullName)
                        return true;
                }
            }
            catch (System.Exception error)
            {
            }

            try
            {
                if (Line == codeElement.StartPoint.Line &&
                    LineCharOffset == codeElement.StartPoint.LineCharOffset)
                    return true;
            }
            catch (System.Exception error)
            {

            }
            return false;
        }



        /// <exclude>Excluded</exclude>
        int _Line = 0;
        /// <MetaDataID>{FBB27580-DA57-4725-9431-D36E1DB380B2}</MetaDataID>
        public int Line
        {
            get
            {
                return _Line;
            }
            set
            {
                _Line = value;
            }

        }
        /// <exclude>Excluded</exclude>
        int _LineCharOffset = 0;
        /// <MetaDataID>{D7D3637F-1C97-4D15-85C1-34AB8B0A6640}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                return _LineCharOffset;
            }
        }

        /// <MetaDataID>{39611389-56c2-4c52-a73d-6de1408a8e6a}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            return _Line;
        }
        /// <MetaDataID>{11e47a23-0c14-4138-b762-6cebb6e1bd4b}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return _LineCharOffset;
        }
        /// <MetaDataID>{6398c1da-29ea-4149-84ec-9fac379e96a1}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }







    }
}
