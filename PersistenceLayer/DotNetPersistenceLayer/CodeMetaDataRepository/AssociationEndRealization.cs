namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{EAA6CD79-AE61-4CF8-BE12-D89A15163A44}</MetaDataID>
    public class AssociationEndRealization : OOAdvantech.MetaDataRepository.AssociationEndRealization, CodeElementContainer
    {


        /// <MetaDataID>{85f75e16-85ce-4ae0-8d1e-eff3fd0f90ea}</MetaDataID>
        private System.Xml.XmlDocument LoadDocDocumentItems()
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                document.LoadXml((VSProperty as EnvDTE.CodeProperty).DocComment);

            }
            catch (System.Exception error)
            {
                document.LoadXml("<doc></doc>");
            }
            foreach (System.Xml.XmlNode node in document.DocumentElement)
            {
                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                if (element == null)
                    continue;

                if (element.Name.ToLower() == "summary".ToLower())
                    base.PutPropertyValue("MetaData", "Documentation", element.InnerText);

                if (element.Name.ToLower() == "MetaDataID".ToLower())
                    _Identity = new MetaDataRepository.MetaObjectID(element.InnerText);
            }
            return document;
        }
        /// <MetaDataID>{44b03555-d498-4919-87fb-df741aee4064}</MetaDataID>
        MetaDataRepository.MetaObjectID CodeElementContainer.Identity
        {
            get
            {
                string identity = GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string;
                if (!string.IsNullOrEmpty(identity))
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(identity);
                else
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
            }
        }
        /// <MetaDataID>{35AC2C8D-57C2-4036-8F5B-EEAD16F96E41}</MetaDataID>
        private EnvDTE.CodeProperty VSProperty;

        /// <MetaDataID>{5dd0d26d-9850-4ccf-81fb-2d8f45fe998f}</MetaDataID>
        internal AssociationEndRealization()
        {
        }
        /// <MetaDataID>{78dd8584-198d-4aef-94ad-90c76fd68be8}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return Name;
            }
        }

        /// <MetaDataID>{4cd6b7fb-c618-447e-af39-cbb7beb46e4f}</MetaDataID>
        public string CurrentProgramLanguageFullName
        {
            get
            {
                if (Namespace is CodeElementContainer)
                    return (Namespace as CodeElementContainer).CurrentProgramLanguageFullName + "." + CurrentProgramLanguageName;
                else
                    return FullName;
            }
        }

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSProperty as EnvDTE.CodeElement, identity);
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
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
                string identity = base.GetPropertyValue(propertyType, propertyNamespace, propertyName) as string;

                if (identity != null)
                    return identity;
                if (VSProperty == null)
                    return base.Identity;

                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSProperty as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSProperty as EnvDTE.CodeElement, identity);
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


        ///// <MetaDataID>{93d1f08c-dd42-4ba3-bd24-2b49fa71a737}</MetaDataID>
        //public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (VSProperty == null && _Identity == null)
        //                return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
        //            if (_Identity != null)
        //                return _Identity;
        //            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //            document.LoadXml(VSProperty.DocComment);


        //            foreach (System.Xml.XmlNode node in document.DocumentElement)
        //            {
        //                System.Xml.XmlElement element = node as System.Xml.XmlElement;
        //                if (element == null)
        //                    continue;
        //                if (element.Name.ToLower() == "MetaDataID".ToLower())
        //                    return new MetaDataRepository.MetaObjectID(element.InnerText);
        //            }
        //            System.Guid guid = System.Guid.NewGuid();
        //            try
        //            {
        //                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("{" + guid.ToString() + "}");
        //                document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
        //                VSProperty.DocComment = document.OuterXml;
        //            }
        //            catch
        //            {
        //            }
        //            return _Identity;
        //        }
        //        catch (System.Exception error)
        //        {
        //            return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
        //        }
        //    }
        //}

        /// <MetaDataID>{7d50a3f8-3586-4d83-8719-349ab741fd1a}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, originMetaObject);
                return;
            }
            _Name = originMetaObject.Name;
            MetaDataRepository.AssociationEnd originAssociationEnd = null;

            if (originMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                originAssociationEnd = (originMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification;
            if (originMetaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                originAssociationEnd = originMetaObject as OOAdvantech.MetaDataRepository.AssociationEnd;
            if (originAssociationEnd == null)
                return;

            string setter = null;
            string getter = null;
            if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter"))
                getter = _Name;
            if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter"))
                setter = _Name;
            if (setter == getter && setter == null)
                getter = _Name;



            OOAdvantech.MetaDataRepository.Classifier _Type = null;
            if (originMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
            {
                if (_Owner == null || _Owner.Identity.ToString() != (originMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner.Identity.ToString())
                    _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner, this) as MetaDataRepository.Classifier;
            }
            else
            {
                if (_Owner == null || _Owner.Identity.ToString() != (originMetaObject as MetaDataRepository.AssociationEnd).Namespace.Identity.ToString())
                    _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Namespace, this) as MetaDataRepository.Classifier;
            }
            if (originAssociationEnd.CollectionClassifier == null)
            {
                _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as OOAdvantech.MetaDataRepository.Classifier;
                if (_Type == null)
                    _Type = UnknownClassifier.GetClassifier(originAssociationEnd.Specification.FullName, ImplementationUnit);
            }
            else
            {
                _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.CollectionClassifier, this) as OOAdvantech.MetaDataRepository.Classifier;
                if (_Type == null)
                    _Type = UnknownClassifier.GetClassifier(originAssociationEnd.CollectionClassifier.FullName, ImplementationUnit);
            }

            if (VSProperty == null)
            {
                if (setter == getter && setter == null)
                    getter = _Name;
                if (_Owner is Class)
                    VSProperty = (_Owner as Class).VSClass.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);
                if (_Owner is Structure)
                    VSProperty = (_Owner as Structure).VSStruct.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);
                if (_Owner is Interface)
                    VSProperty = (_Owner as Interface).VSInterface.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);


                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                try
                {
                    document.LoadXml(VSProperty.DocComment);

                }
                catch (System.Exception error)
                {
                    document.LoadXml("<doc></doc>");
                }
                foreach (System.Xml.XmlNode node in document.DocumentElement)
                {
                    System.Xml.XmlElement element = node as System.Xml.XmlElement;
                    if (element == null)
                        continue;
                    if (element.Name.ToLower() == "MetaDataID".ToLower())
                    {
                        element.InnerText = _Identity.ToString();
                        VSProperty.DocComment = document.OuterXml;
                        return;
                    }
                }
                document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
                VSProperty.DocComment = document.OuterXml;
            }
            else
            {
                VSProperty.Name = _Name;


                if (Visibility != originAssociationEnd.Visibility)
                {
                    Visibility = originAssociationEnd.Visibility;
                    EnvDTE.vsCMAccess access;

                    if (_Owner is Class || _Owner is Structure)
                    {
                        access = VSProperty.Access;
                        if (VSAccessTypeConverter.GetAccessType(Visibility) == EnvDTE.vsCMAccess.vsCMAccessProject &&
                            access == EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected)
                        {
                            access = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;
                        }
                        else
                            access = VSAccessTypeConverter.GetAccessType(Visibility);
                        VSProperty.Access = access;
                    }
                }
                if (VSProperty.Setter == null && setter != null)
                    LanguageParser.AddSetter(VSProperty, (_Owner as CodeElementContainer).CodeElement);
                if (VSProperty.Getter == null && getter != null)
                    LanguageParser.AddGetter(VSProperty, (_Owner as CodeElementContainer).CodeElement);
                VSProperty.Type = LanguageParser.CreateCodeTypeRef(_Type, VSProperty.ProjectItem.ContainingProject.CodeModel);
                if (_Owner is Class || _Owner is Structure)
                {
                    if (Specification != null && (Specification.Namespace is OOAdvantech.MetaDataRepository.Class))
                    {
                        if (VSProperty.Setter != null)
                            (VSProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
                        if (VSProperty.Getter != null)
                            (VSProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
                    }
                }
            }
            EnumerateAttributes:
            EnvDTE.CodeElements attributes = VSProperty.Attributes;
            bool persistentAttributeExist = false;
            bool backwardCompatibilityIDExist = false;
            string persistentMemberName = "nameof(" + GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")";
            if (originAssociationEnd != null)
                persistentMemberName = "nameof(" + (originMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")";

            string backwardCompatibilityID = originMetaObject.GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;


            foreach (EnvDTE.CodeAttribute attribute in attributes)
            {

                if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute) && !Persistent)
                {
                    attribute.Delete();
                    goto EnumerateAttributes;
                }
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute) && Persistent)
                {
                    CodeClassifier.UpdateAttributeValue(attribute, persistentMemberName);
                    persistentAttributeExist = true;
                }
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute) && Persistent)
                {

                    if (string.IsNullOrEmpty(backwardCompatibilityID))
                    {
                        attribute.Delete();
                        goto EnumerateAttributes;
                    }
                    else
                        CodeClassifier.UpdateAttributeValue(attribute, "\"" + backwardCompatibilityID + "\"");
                    backwardCompatibilityIDExist = true;
                }
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute))
                {
                    CodeClassifier.UpdateAttributeValue(attribute, "\"" + backwardCompatibilityID + "\"");
                    backwardCompatibilityIDExist = true;
                }
            }


            int attributePosition = 0;
            if (Persistent && !persistentAttributeExist)
                VSProperty.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.PersistentMember).FullName, VSProperty as EnvDTE.CodeElement), persistentMemberName, attributePosition++);

            if (!backwardCompatibilityIDExist && !string.IsNullOrEmpty(backwardCompatibilityID))
                (VSProperty as EnvDTE.CodeProperty).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSProperty as EnvDTE.CodeElement), "\"" + backwardCompatibilityID + "\"", attributePosition++);

            if (originMetaObject is MetaDataRepository.AssociationEndRealization)
                base.Synchronize(originMetaObject);
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        /// <MetaDataID>{abb13fb2-1ae8-4c8e-b2d5-380ef70d23a8}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }


        /// <MetaDataID>{d0f4da25-1d44-4e6d-8f7d-41cd9cb6cd8f}</MetaDataID>
        public AssociationEndRealization(EnvDTE.CodeProperty property, MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.Classifier owner)
        {

            _Owner = owner;
            _Namespace.Value = owner;
            _Name = associationEnd.Name;
            _Specification = associationEnd;
            associationEnd.AddAssociationEndRealization(this);
            VSProperty = property;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(VSProperty as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            _Identity = null;
            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                if (backwardCompatibilityID[0] == '+')
                {
                    backwardCompatibilityID = backwardCompatibilityID.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity + "." + backwardCompatibilityID);
                }
                else
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);
            }

            _Kind = VSProperty.Kind;
            MetaObjectMapper.AddTypeMap(VSProperty, this);
            _ProjectItem = ProjectItem.AddMetaObject(VSProperty.ProjectItem, this);
            MetaObjectMapper.AddTypeMap(VSProperty, this);
            RefreshStartPoint();
        }
        /// <MetaDataID>{fc5e4dcd-9c16-4ef2-a9ab-107439769a61}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{50df2d90-2641-4525-9d64-c75e89198358}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{8131474a-84c9-4d8f-b3e1-987cec11ce14}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            VSProperty = null;
            MetaObjectMapper.RemoveMetaObject(this);
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);
            if (_Owner != null)
                (_Owner as OOAdvantech.MetaDataRepository.InterfaceImplementor).RemoveAssociationEndRealization(this);
            if (_Specification != null)
                _Specification.RemoveAssociationEndRealization(this);
            _Specification = null;

            _Owner = null;
            _ProjectItem = null;
        }
        /// <MetaDataID>{4da38619-c55a-40ca-b0b0-2f8085827788}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        /// <MetaDataID>{c5a02197-0946-40c4-8f11-b4c35241f77c}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSProperty as EnvDTE.CodeElement;
            }
        }
        /// <MetaDataID>{c190bfdd-b7c5-4514-b787-f769f63f7220}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (Specification != CodeClassifier.GetAssociaionEndForAssociatioEndRealization(Owner, codeElement as EnvDTE.CodeProperty))
                return false;


            if (this.VSProperty == codeElement)
                return true;
            if (_ProjectItem != ProjectItem.GetProjectItem(codeElement.ProjectItem))
                return false;


            EnvDTE.CodeAttribute associationAttribute = null, associationClassAttribute = null;
            CodeClassifier.GetAssociationAttributes(codeElement, out associationAttribute, out associationClassAttribute);
            if (associationAttribute != null)
                return false;

            try
            {
                if (Line == codeElement.StartPoint.Line &&
                    LineCharOffset == codeElement.StartPoint.LineCharOffset)
                    return true;
            }
            catch (System.Exception error)
            {

            }


            EnvDTE.CodeElement vsPropertyParent = null; ;
            EnvDTE.CodeElement codeElementParent = null;

            try
            {
                vsPropertyParent = VSProperty.Parent as EnvDTE.CodeElement;
            }

            catch (System.Exception error)
            {
            }

            try
            {
                codeElementParent = (codeElement as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
            }
            catch (System.Exception error)
            {
            }

            try
            {
                if (itsParent is EnvDTE.CodeElement)
                    if ((itsParent as EnvDTE.CodeElement).FullName + "." + codeElement.Name == CurrentProgramLanguageFullName)
                    {
                        if (vsPropertyParent == codeElementParent && vsPropertyParent == null)
                            return true;
                    }

                if (codeElement.FullName == CurrentProgramLanguageFullName)
                    return true;
            }
            catch (System.Exception error)
            {
            }
            return false;

        }
        /// <MetaDataID>{6f98a0eb-605b-4ad4-a9a0-21eedcc2aec2}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            MetaObjectMapper.RemoveMetaObject(this);
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            VSProperty = codeElement as EnvDTE.CodeProperty;

            _Identity = null;
            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                if (backwardCompatibilityID[0] == '+')
                {
                    backwardCompatibilityID = backwardCompatibilityID.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity + "." + backwardCompatibilityID);
                }
                else
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);
            }

            MetaObjectMapper.AddTypeMap(VSProperty, this);
            if (_Name != VSProperty.Name)
            {
                _Name = VSProperty.Name;
                MetaObjectChangeState();
            }
        }
        /// <MetaDataID>{6620592c-af36-46cb-be7c-fefb0f175b82}</MetaDataID>
        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSProperty.StartPoint.LineCharOffset;

                if (_Line != VSProperty.StartPoint.Line)
                {
                    _Line = VSProperty.StartPoint.Line;
                    // MetaObjectChangeState();
                }


            }
            catch (System.Exception error)
            {
            }
        }
        /// <MetaDataID>{712a25ed-937b-4131-8ada-638d0cf6ef17}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{612c35be-661b-4477-827a-a0169923c5a2}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{2c2a4d01-c6e1-4b0d-8efb-cd3ff64f80b3}</MetaDataID>
        public int Line
        {
            get
            {
                return _Line;
            }
            set
            {
                if (_Line != value)
                {
                    _Line = value;
                    //MetaObjectChangeState();
                }
            }
        }
        /// <MetaDataID>{599c255e-3436-4bdb-ad62-718b9d4a45d0}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                return _LineCharOffset;
            }
        }

        /// <MetaDataID>{9932c409-51c8-4cdf-9f77-786167ddeba6}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            return _Line;
        }
        /// <MetaDataID>{b9c934d5-e6fc-444f-b202-8ee50f6e4709}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return _LineCharOffset;
        }

        /// <MetaDataID>{429d9f1c-e9da-402b-a3cd-71c962f5bb18}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }
    }
}
