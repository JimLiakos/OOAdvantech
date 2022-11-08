namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{37A44035-2E10-4E30-9DA4-65ACB0C12E17}</MetaDataID>
    public class AttributeRealization : OOAdvantech.MetaDataRepository.AttributeRealization, CodeElementContainer
    {

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

        /// <MetaDataID>{6fe3fbdf-9a04-465d-80d3-d786858f0f13}</MetaDataID>
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

        public int GetLine(ProjectItem projectItem)
        {
            return _Line;
        }
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return _LineCharOffset;
        }
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }

        public string CurrentProgramLanguageName
        {
            get
            {
                return Name;
            }
        }

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

        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            _Name = OriginMetaObject.Name;
            MetaDataRepository.Attribute originAttribute = null;

            if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                originAttribute = (OriginMetaObject as OOAdvantech.MetaDataRepository.AttributeRealization).Specification;
            if (OriginMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                originAttribute = OriginMetaObject as OOAdvantech.MetaDataRepository.Attribute;





            string setter = null;
            string getter = null;
            // if ((bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") || _Owner is Interface)
            {
                if (originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Getter") != null && (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Getter"))
                    getter = _Name;
                if (originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Setter") != null && (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Setter"))
                    setter = _Name;
                if (setter == getter && setter == null)
                    getter = _Name;

            }


            if (VSProperty == null)
            {
                if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                    _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as OOAdvantech.MetaDataRepository.AttributeRealization).Owner, this) as MetaDataRepository.Classifier;
                else
                    _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner, this) as MetaDataRepository.Classifier;

                _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Type, this) as OOAdvantech.MetaDataRepository.Classifier;
                if (_Type == null)
                    _Type = UnknownClassifier.GetClassifier(originAttribute.Type.FullName, ImplementationUnit);



                //if (_Owner is Interface)
                //    VSOperation = (_Owner as Interface).VSInterface.AddFunction(_Name, EnvDTE.vsCMFunction.vsCMFunctionFunction, ReturnType.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault);


                if (_Owner is Class)
                {
                    if (setter == getter && setter == null)
                        getter = _Name;
                    VSProperty = (_Owner as Class).VSClass.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);
                }
                if (_Owner is Structure)
                {
                    if (setter == getter && setter == null)
                        getter = _Name;
                    VSProperty = (_Owner as Structure).VSStruct.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);
                }

                if (_Owner is Interface)
                {
                    if (setter == getter && setter == null)
                        getter = _Name;

                    VSProperty = (_Owner as Interface).VSInterface.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);
                }


                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                System.Threading.Thread.Sleep(100);

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
                VSProperty.Name = _Name;
            if (Visibility != originAttribute.Visibility)
            {
                Visibility = originAttribute.Visibility;
                EnvDTE.vsCMAccess access;

                if (_Owner is Class)
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


            _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Type, this) as OOAdvantech.MetaDataRepository.Classifier;
            if (_Type == null)
                _Type = UnknownClassifier.GetClassifier(originAttribute.Type.FullName, ImplementationUnit);
            VSProperty.Type = LanguageParser.CreateCodeTypeRef(_Type, VSProperty.ProjectItem.ContainingProject.CodeModel);
            if (originAttribute.Owner is MetaDataRepository.Class)
            {
                if (Specification != null && (Specification.Namespace is OOAdvantech.MetaDataRepository.Class))
                {
                    if (VSProperty.Setter != null)
                        (VSProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
                    if (VSProperty.Getter != null)
                        (VSProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
                }
            }


            if (OriginMetaObject is MetaDataRepository.AttributeRealization)
                base.Synchronize(OriginMetaObject);
            else if(originAttribute!=null)
                _Persistent = originAttribute.Persistent;
            bool persistentAttributeExist = false;

            EnvDTE.CodeElements attributes = null;
            if (VSProperty != null)
            {
                attributes = VSProperty.Attributes;

                string backwardCompatibilityID = OriginMetaObject.GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;
                string persistentMemberName = "nameof(" + OriginMetaObject.GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")";

                string lengthValue = GetPropertyValue(typeof(string), "MetaData", "Length") as string;
                
                if (!string.IsNullOrWhiteSpace(lengthValue))
                {
                    int length = 0;
                    if (int.TryParse(lengthValue, out length))
                    {
                        if (length > 0)
                        {
                            if (string.IsNullOrWhiteSpace(persistentMemberName))
                                persistentMemberName = length.ToString();
                            else
                                persistentMemberName = length.ToString() + "," + persistentMemberName;
                        }
                    }
                }
                bool backwardCompatibilityIDExist = false;
                EnumerateAttributes:
                attributes = VSProperty.Attributes;
                foreach (EnvDTE.CodeAttribute attribute in attributes)
                {

                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute) && Persistent)
                    {
                        if (VSProperty is EnvDTE.CodeProperty)
                            CodeClassifier.UpdateAttributeValue(attribute, persistentMemberName);
                        else
                            CodeClassifier.UpdateAttributeValue(attribute, "");

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
                    VSProperty.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSProperty as EnvDTE.CodeElement), "\"" + backwardCompatibilityID + "\"", attributePosition++); ;
            }

            MetaObjectChangeState();
        }
        /// <MetaDataID>{62AB037F-8728-4B30-948D-5890F1EFF5DA}</MetaDataID>
        private EnvDTE.CodeProperty VSProperty;

        /// <MetaDataID>{694B3FE6-CB42-43B6-934F-19A00519BC9A}</MetaDataID>
        internal AttributeRealization()
        {
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }
        /// <MetaDataID>{bd3b0323-3396-4944-bc53-6d0402aa2e7e}</MetaDataID>
        public AttributeRealization(MetaDataRepository.Attribute attribute, MetaDataRepository.Classifier owner)
        {

            _Owner = owner;
            _Namespace.Value = owner;
            _Name = attribute.Name;
            _Specification = attribute;
        }
        /// <MetaDataID>{360A0492-B8D2-42C6-9AD0-8F395AD07075}</MetaDataID>
        public AttributeRealization(EnvDTE.CodeProperty property, MetaDataRepository.Attribute attribute, MetaDataRepository.Classifier owner)
        {

            _Owner = owner;
            _Namespace.Value = owner;
            _Name = attribute.Name;
            _Specification = attribute;
            attribute.AddAttributeRealization(this);
            VSProperty = property;
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

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(VSProperty as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            _Kind = VSProperty.Kind;
            _Type = attribute.Type;
            _ParameterizedType = attribute.ParameterizedType;

            try
            {
                if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
                {

                    _LineCharOffset = VSProperty.StartPoint.LineCharOffset;
                    _Line = VSProperty.StartPoint.Line;
                    _ProjectItem = ProjectItem.AddMetaObject(VSProperty.ProjectItem, this);
                }
            }
            catch (System.Exception error)
            {
            }
            MetaObjectMapper.AddTypeMap(VSProperty, this);
            RefreshStartPoint();


            EnvDTE.CodeElements attributes = null;
            attributes = VSProperty.Attributes;
            foreach (EnvDTE.CodeAttribute codeAttribute in attributes)
            {
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), codeAttribute))
                {
                    Persistent = true;
                    break;
                }
            }

        }

        /// <MetaDataID>{59DEB7E6-3132-4C35-AA73-554DE9219487}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{225E7CA5-4913-452E-A7B0-D19A79FD0C74}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{1EB6736D-396F-4DFA-BEEA-300A4BF6E1FD}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            VSProperty = null;
            //MetaObjectMapper.RemoveType(VSProperty);
            MetaObjectMapper.RemoveMetaObject(this);
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);
            if (_Owner != null)
                (_Owner as OOAdvantech.MetaDataRepository.InterfaceImplementor).RemoveAttributeRealization(this);
            if (_Specification != null)
                _Specification.RemoveAttributeRealization(this);
            _Specification = null;
            _Owner = null;
            _ProjectItem = null;

        }
        /// <MetaDataID>{3cf3348e-ae89-4513-bca6-9d5b947336e5}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        /// <MetaDataID>{56D5F3F0-1591-41AD-B82D-7CE5AA6EE21C}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSProperty as EnvDTE.CodeElement;
            }
        }
        /// <MetaDataID>{55F1B8AE-9833-453B-9462-806BDD224576}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (Specification != CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty))
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
        /// <MetaDataID>{78862156-F3ED-4402-BDF0-EA1E268492F2}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            MetaObjectMapper.RemoveMetaObject(this);
            VSProperty = codeElement as EnvDTE.CodeProperty;
            MetaObjectMapper.AddTypeMap(VSProperty, this);
            if (_Name != VSProperty.Name)
            {
                _Name = VSProperty.Name;
                MetaObjectChangeState();
            }
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
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

            try
            {
                object obj = ParameterizedType;
            }
            catch (System.Exception error)
            {
            }
            _ProjectItem = ProjectItem.AddMetaObject(VSProperty.ProjectItem, this);
            try
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();

                try
                {
                    if (VSProperty is EnvDTE.CodeVariable)
                        document.LoadXml((VSProperty as EnvDTE.CodeVariable).DocComment);
                    else
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
                }
            }
            catch (System.Exception error)
            {
            }


        }
        /// <MetaDataID>{ACC4F952-1C65-4BB7-A3B1-18F82763B8F5}</MetaDataID>
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
        /// <MetaDataID>{5CFEA65C-AC3B-4256-839A-F15969F04ECA}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{7A3C39C5-494A-481D-A7FF-4F82CC89BFAB}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{4488E989-ABB6-4D38-B087-86D9C7B700AE}</MetaDataID>
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

        /// <MetaDataID>{FA393E8A-85E6-43AD-887B-A072580B8AB9}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                return _LineCharOffset;
            }
        }
    }
}
