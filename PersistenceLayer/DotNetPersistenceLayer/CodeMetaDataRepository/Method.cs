namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{56E25168-4CC2-4EE2-B518-601D26835A43}</MetaDataID>
    public class Method : OOAdvantech.MetaDataRepository.Method, CodeElementContainer
    {
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSOperation as EnvDTE.CodeElement, identity);
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
            else
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
        }

        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                string identity = base.GetPropertyValue(propertyType, propertyNamespace, propertyName) as string;

                if (identity != null)
                    return identity;
                if (VSOperation == null)
                    return base.Identity;
                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSOperation as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSOperation as EnvDTE.CodeElement, identity);
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

        /// <MetaDataID>{5caeebbb-e0d8-48d3-949c-1f4e2e0e1d16}</MetaDataID>
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

        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            //  base.Synchronize(OriginMetaObject);
            _Name = OriginMetaObject.Name;
            MetaDataRepository.Method originMethod = OriginMetaObject as MetaDataRepository.Method;

            if (VSOperation == null)
            {
                _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as MetaDataRepository.Method).Owner, this) as MetaDataRepository.Classifier;
                (_Owner as Class).GetOperation("", new string[0], true);
                _Specification = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as MetaDataRepository.Method).Specification, this) as MetaDataRepository.Operation;
                if (_Specification == null)
                    return;
                VSOperation = (_Owner as Class).VSClass.AddFunction(_Name, EnvDTE.vsCMFunction.vsCMFunctionFunction, Specification.ReturnType.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);
                if (_Specification.Owner is OOAdvantech.MetaDataRepository.Class)
                    (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;

                System.Xml.XmlDocument document = new System.Xml.XmlDocument();

                try
                {
                    document.LoadXml(VSOperation.DocComment);
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
                        VSOperation.DocComment = document.OuterXml;
                        return;
                    }
                }

                document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
                VSOperation.DocComment = document.OuterXml;


            }
            else
                VSOperation.Name = _Name;

            try
            {
                Specification.Synchronize(originMethod.Specification);

                if (Visibility != originMethod.Visibility)
                {

                    Visibility = originMethod.Visibility;
                    if (VSAccessTypeConverter.GetAccessType(Visibility) == EnvDTE.vsCMAccess.vsCMAccessProject &&
                        VSOperation.Access == EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected)
                    {
                        VSOperation.Access = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;
                    }
                    else
                        VSOperation.Access = VSAccessTypeConverter.GetAccessType(Visibility);
                }
                if (Specification.ReturnType != null && VSOperation.Type.AsFullName != Specification.ReturnType.FullName)
                    VSOperation.Type = LanguageParser.CreateCodeTypeRef(Specification.ReturnType, VSOperation.ProjectItem.ContainingProject.CodeModel);
                base.Synchronize(OriginMetaObject);


                if (Operation.GetSignature(originMethod.Specification, ImplementationUnit as Project) != Operation.GetSignature(VSOperation))
                {

                    int i = 0;
                    foreach (MetaDataRepository.Parameter parameter in originMethod.Specification.Parameters)
                    {
                        MetaDataRepository.Classifier classifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(parameter.Type, this) as MetaDataRepository.Classifier;
                        if (classifier == null)
                            classifier = UnknownClassifier.GetClassifier(parameter.Type.FullName, ImplementationUnit);
                        if (VSOperation.Parameters.Count > i)
                        {
                            EnvDTE.CodeParameter vsParameter = VSOperation.Parameters.Item(i + 1) as EnvDTE.CodeParameter;
                            vsParameter.Name = parameter.Name;
                            vsParameter.Type = LanguageParser.CreateCodeTypeRef(classifier, VSOperation.ProjectItem.ContainingProject.CodeModel);
                        }
                        else
                            VSOperation.AddParameter(parameter.Name, classifier.FullName, i);
                        i++;
                    }

                    while (VSOperation.Parameters.Count > i)
                        VSOperation.RemoveParameter(VSOperation.Parameters.Item(i + 1));
                }


                switch (OverrideKind)
                {
                    case MetaDataRepository.OverrideKind.Abstract:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindAbstract;
                            break;
                        }
                    case MetaDataRepository.OverrideKind.New:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew;
                            break;
                        }

                    case MetaDataRepository.OverrideKind.Override:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
                            break;
                        }

                    case MetaDataRepository.OverrideKind.Sealed:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindSealed;
                            break;
                        }
                    case MetaDataRepository.OverrideKind.Virtual:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindVirtual;
                            break;
                        }
                    case MetaDataRepository.OverrideKind.None:
                        {
                            (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNone;
                            break;
                        }
                }


                MetaObjectMapper.AddTypeMap(VSOperation, this);

            }
            catch (System.Exception error)
            {


            }

        }
        protected override void SetIdentity(OOAdvantech.MetaDataRepository.MetaObjectID theIdentity)
        {
            base.SetIdentity(theIdentity);
            if (VSOperation == null)
                return;
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                document.LoadXml(VSOperation.DocComment);
            }
            catch (System.Exception error)
            {
                document.LoadXml("<doc></doc>");
            }
            string name = _Name;
            foreach (System.Xml.XmlNode node in document.DocumentElement)
            {
                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                if (element == null)
                    continue;
                if (element.Name.ToLower() == "MetaDataID".ToLower())
                {
                    element.InnerText = _Identity.ToString();
                    VSOperation.DocComment = document.OuterXml;
                    VSOperation.Name = name + name;
                    VSOperation.Name = _Name;

                    return;
                }
            }

            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
            VSOperation.DocComment = document.OuterXml;
            VSOperation.Name = name + name;
            VSOperation.Name = _Name;


        }

        /// <MetaDataID>{1EB99EA1-B1C2-4510-8DC6-F425019A38E4}</MetaDataID>
        internal EnvDTE.CodeFunction VSOperation;

        /// <MetaDataID>{ab446c4a-11be-423c-9266-72eddde37c69}</MetaDataID>
        internal Method()
        {

        }

        /// <MetaDataID>{0d79a5b4-f7fb-4009-88a0-ca8f5bce20f2}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }

        /// <MetaDataID>{a96027f0-402b-4c89-b489-92dd2b3137e9}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{1e278361-9863-48cc-8058-c631c957586c}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{11df0a4d-4a1f-494a-b7bb-269ac949dbc1}</MetaDataID>
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
        /// <MetaDataID>{6037a129-4e87-4cf0-87dd-1067579556bb}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                return _LineCharOffset;
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


        //public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (VSOperation == null && _Identity == null)
        //                return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
        //            if (_Identity != null)
        //                return _Identity;
        //            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //            document.LoadXml(VSOperation.DocComment);

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
        //                VSOperation.DocComment = document.OuterXml;
        //            }
        //            catch
        //            {
        //            }
        //            return _Identity;
        //        }
        //        catch (System.Exception error)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }

        /// <MetaDataID>{b0f472aa-e1e7-43c9-8f28-a697454f168c}</MetaDataID>
        public Method(MetaDataRepository.Operation operation, MetaDataRepository.Classifier owner) : base(operation)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            _Name = operation.Name;

            // _Specification=new Operation(
        }


        /// <MetaDataID>{718a9df1-80e6-474e-a8e6-a2979a0465e6}</MetaDataID>
        public Method(MetaDataRepository.Operation operation, EnvDTE.CodeFunction vsOperation, MetaDataRepository.Classifier owner) : base(operation)
        {

            _Owner = owner;
            _Namespace.Value = owner;
            VSOperation = vsOperation;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(VSOperation as EnvDTE.CodeElement, out identity, out comments);
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


            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindAbstract)
                _OverrideKind = MetaDataRepository.OverrideKind.Abstract;

            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew)
                _OverrideKind = MetaDataRepository.OverrideKind.New;

            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNone)
                _OverrideKind = MetaDataRepository.OverrideKind.None;

            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride)
                _OverrideKind = MetaDataRepository.OverrideKind.Override;

            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindSealed)
                _OverrideKind = MetaDataRepository.OverrideKind.Sealed;

            if ((vsOperation as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindVirtual)
                _OverrideKind = MetaDataRepository.OverrideKind.Virtual;

            _Kind = vsOperation.Kind;
            _Name = VSOperation.Name;
            Visibility = VSAccessTypeConverter.GetVisibilityKind(VSOperation.Access);
            try
            {
                if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
                {
                    _LineCharOffset = VSOperation.StartPoint.LineCharOffset;
                    _Line = VSOperation.StartPoint.Line;
                    _ProjectItem = ProjectItem.AddMetaObject(VSOperation.ProjectItem, this);
                }
            }
            catch (System.Exception error)
            {
            }

            MetaObjectMapper.AddTypeMap(VSOperation, this);

        }


        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSOperation as EnvDTE.CodeElement;

            }
        }


        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementFunction)
                return false;
            if (Specification != CodeClassifier.GetOperationForMethod(Owner, codeElement as EnvDTE80.CodeFunction2))
                return false;

            if (this.VSOperation == codeElement)
                return true;
            if (_ProjectItem != ProjectItem.GetProjectItem(codeElement.ProjectItem))
                return false;


            EnvDTE.CodeElement vsOperationParent = null; ;
            EnvDTE.CodeElement codeElementParent = null;
            string codeElementSignature = null;
            string vsOperationSignature = null;

            try
            {
                vsOperationParent = VSOperation.Parent as EnvDTE.CodeElement;
            }
            catch (System.Exception error)
            {
            }

            try
            {
                codeElementParent = (codeElement as EnvDTE.CodeFunction).Parent as EnvDTE.CodeElement;
            }
            catch (System.Exception error)
            {
            }





            try
            {
                if (itsParent is EnvDTE.CodeElement)
                    if ((itsParent as EnvDTE.CodeElement).FullName + "." + codeElement.Name == CurrentProgramLanguageFullName)
                    {
                        if (vsOperationParent == codeElementParent && vsOperationParent == null)
                            return true;
                    }

                if (codeElement.FullName == CurrentProgramLanguageFullName)
                    return true;
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


        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            MetaObjectMapper.RemoveMetaObject(this);
            VSOperation = codeElement as EnvDTE.CodeFunction;


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

            MetaObjectMapper.AddTypeMap(VSOperation, this);
            if (_Name != VSOperation.Name)
            {
                _Name = VSOperation.Name;
                MetaObjectChangeState();
            }
        }

        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSOperation.StartPoint.LineCharOffset;
                if (_Line != VSOperation.StartPoint.Line)
                {
                    _Line = VSOperation.StartPoint.Line;
                    //MetaObjectChangeState();
                }


            }
            catch (System.Exception error)
            {
            }
        }

        /// <MetaDataID>{b6491fc2-c46e-4125-8b14-36e1565a9311}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            VSOperation = null;
            MetaObjectMapper.RemoveMetaObject(this);
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);
            if (_Owner != null)
                (_Owner as OOAdvantech.MetaDataRepository.InterfaceImplementor).RemoveMethod(this);
            if (_Specification != null)
                _Specification.RemoveOperationImplementation(this);

            _ProjectItem = null;
            _Specification = null;
            _Owner = null;

        }
        /// <MetaDataID>{464cf5e5-4fc9-4763-8b29-5f2ce700f121}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }



    }
}
