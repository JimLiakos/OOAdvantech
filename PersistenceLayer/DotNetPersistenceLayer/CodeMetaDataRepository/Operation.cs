//using MetaDataRepository=OOAdvantech.MetaDataRepository;
namespace OOAdvantech.CodeMetaDataRepository
{



    /// <MetaDataID>{7880677F-A683-426C-977E-27A60B1EA823}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{7880677F-A683-426C-977E-27A60B1EA823}")]
    public class Operation : OOAdvantech.MetaDataRepository.Operation, CodeElementContainer
    {

        /// <MetaDataID>{173796f3-e6cb-4a65-ae4d-366d99c26697}</MetaDataID>
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
            {
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }

        /// <MetaDataID>{100b3169-162e-4e7c-a9db-52e603c43b03}</MetaDataID>
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
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    CodeClassifier.SetIdentityToCodeElement(VSOperation as EnvDTE.CodeElement, identity);
                }
                catch
                {
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }
        /// <MetaDataID>{44cd5e09-bd08-4174-8f05-904d99a8f64d}</MetaDataID>
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
        /// <MetaDataID>{bfb1436e-2c6a-4a3d-80e5-4dd5f1a914ce}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return Name;
            }
        }
        /// <MetaDataID>{8b00aad0-988a-4a9e-9627-70f290babd5d}</MetaDataID>
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
        /// <MetaDataID>{f93b040b-0321-4c24-bb98-9db674a72d47}</MetaDataID>
        public static string GetSignature(EnvDTE.CodeFunction method)
        {
            //TODO πως λειτουργεί στη vb που δεν είναι case sensitive
            string methodSignature = null;
            foreach (EnvDTE.CodeParameter codeParameter in method.Parameters)
            {
                if (methodSignature == null)
                    methodSignature = method.Name + "(";
                else
                    methodSignature += ",";
                EnvDTE.CodeTypeRef parameterType = codeParameter.Type;
                if (!string.IsNullOrEmpty(parameterType.AsFullName))
                    methodSignature += parameterType.AsFullName;
                else
                    methodSignature += parameterType.AsString;
            }
            if (methodSignature == null)
                methodSignature = method.Name + "()";
            else
                methodSignature += ")";
            return methodSignature;


        }
        /// <MetaDataID>{f2d36b45-d1b8-4f4a-997e-e565defeddd4}</MetaDataID>
        public static string GetSignature(MetaDataRepository.Operation operation, Project project)
        {

            string operationSignature = null;
            foreach (MetaDataRepository.Parameter parameter in operation.Parameters)
            {
                if (operationSignature == null)
                    operationSignature = operation.Name + "(";
                else
                    operationSignature += ",";
                if (parameter.ParameterizedType != null)
                    operationSignature += parameter.ParameterizedType.FullName;
                else
                    operationSignature += LanguageParser.GetTypeFullName(parameter.Type, project.VSProject);
            }
            if (operationSignature == null)
                operationSignature = operation.Name + "()";
            else
                operationSignature += ")";
            return operationSignature;

        }

        /// <MetaDataID>{75b71abb-753b-41d4-949c-cf6af06edb26}</MetaDataID>
        public static string GetSignature(MetaDataRepository.Operation operation, string language)
        {

            string operationSignature = null;
            foreach (MetaDataRepository.Parameter parameter in operation.Parameters)
            {
                if (operationSignature == null)
                    operationSignature = operation.Name + "(";
                else
                    operationSignature += ",";
                if (parameter.ParameterizedType != null)
                    operationSignature += parameter.ParameterizedType.FullName;
                else
                    operationSignature += LanguageParser.GetTypeFullName(parameter.Type, language);
            }
            if (operationSignature == null)
                operationSignature = operation.Name + "()";
            else
                operationSignature += ")";
            return operationSignature;

        }
        ///// <MetaDataID>{7f0c349a-9bc2-44ed-abc1-0484bd4bdac0}</MetaDataID>
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
        //            try
        //            {
        //                if (!string.IsNullOrEmpty(VSOperation.DocComment))
        //                    document.LoadXml(VSOperation.DocComment);
        //                else
        //                    document.LoadXml("<doc/>");

        //            }
        //            catch (System.Exception error)
        //            {
        //                document.LoadXml("<doc/>");
        //            }

        //            foreach (System.Xml.XmlNode node in document.DocumentElement)
        //            {
        //                System.Xml.XmlElement element = node as System.Xml.XmlElement;
        //                if (element == null)
        //                    continue;
        //                if (element.Name.ToLower() == "summary".ToLower())
        //                    base.PutPropertyValue("MetaData", "Documentation", element.InnerText);

        //                if (element.Name.ToLower() == "MetaDataID".ToLower())
        //                    _Identity = new MetaDataRepository.MetaObjectID(element.InnerText);
        //            }
        //            if (_Identity != null)
        //                return _Identity;

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
        //            return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName + "." + GetSignature(this, ImplementationUnit as Project));
        //        }
        //    }
        //}



        /// <MetaDataID>{af82f427-41b6-4440-a654-1f9ef2b4c2d9}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
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
            foreach (System.Xml.XmlNode node in document.DocumentElement)
            {
                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                if (element == null)
                    continue;
                if (element.Name.ToLower() == "summary".ToLower())
                {
                    if (element.InnerText == documentation)
                        return;
                    element.InnerText = documentation;
                    VSOperation.DocComment = document.OuterXml;
                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("summary")).InnerText = documentation.ToString();
            VSOperation.DocComment = document.OuterXml;
        }

        /// <MetaDataID>{1260a477-17ed-423b-a825-07268fb4b460}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, originMetaObject);
                return;
            }
            int line = 0;
            try
            {
                if (VSOperation != null)
                    line = VSOperation.StartPoint.Line;
            }
            catch (System.Exception error)
            {
                try
                {
                    (Owner as CodeElementContainer).RefreshCodeElement((Owner as CodeElementContainer).CodeElement);
                }
                catch (System.Exception exp)
                {

                }
            }

            bool changeName = _Name != originMetaObject.Name;
            _Name = originMetaObject.Name;
            MetaDataRepository.Operation originOperation = originMetaObject as MetaDataRepository.Operation;
            if (originMetaObject is MetaDataRepository.Method)
                originOperation = (originMetaObject as MetaDataRepository.Method).Specification;

            if (originMetaObject is MetaDataRepository.Method)
            {
                originOperation = (originMetaObject as MetaDataRepository.Method).Specification;
                _OverrideKind = (originMetaObject as MetaDataRepository.Method).OverrideKind;
            }
            else
            {
                originOperation = originMetaObject as MetaDataRepository.Operation;
                _OverrideKind = originOperation.OverrideKind;
            }
            if (VSOperation == null)
            {
                #region Add new CodeFunction to the classifier CodeModel
                try
                {
                    if (_Identity == null)
                        _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(originMetaObject.Identity.ToString());

                    if (_Owner == null || (originMetaObject as MetaDataRepository.Feature).Owner.Identity.ToString() != _Owner.Identity.ToString())
                    {
                        if (originMetaObject is MetaDataRepository.Method)
                            _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as MetaDataRepository.Method).Owner, this) as MetaDataRepository.Classifier;
                        else
                            _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as MetaDataRepository.Operation).Owner, this) as MetaDataRepository.Classifier;
                        _Namespace.Value = _Owner;
                    }
                    _ReturnType = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originOperation.ReturnType, this) as OOAdvantech.MetaDataRepository.Classifier;
                    if (_ReturnType == null)
                        _ReturnType = UnknownClassifier.GetClassifier(originOperation.ReturnType.FullName, ImplementationUnit);

                    if (_Owner is Interface)
                        VSOperation = (_Owner as Interface).VSInterface.AddFunction(_Name, CodeFunctionType, CodeTypeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault);

                    if (_Owner is Class)
                        VSOperation = (_Owner as Class).VSClass.AddFunction(_Name, CodeFunctionType, CodeTypeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);

                    if (_Owner is Structure)
                        VSOperation = (_Owner as Structure).VSStruct.AddFunction(_Name, CodeFunctionType, CodeTypeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);

                    _ProjectItem = ProjectItem.AddMetaObject(VSOperation.ProjectItem, this);
                    RefreshStartPoint();
                    _Kind = EnvDTE.vsCMElement.vsCMElementFunction;
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    System.Threading.Thread.Sleep(100);
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
                catch (System.Exception error)
                {
                    if (MetaDataRepository.SynchronizerSession.ErrorLog != null)
                        MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(_Owner.FullName + " :    " + error.Message);
                    return;
                }
                #endregion
            }
            else if (changeName)
                VSOperation.Name = _Name;

            #region Updates operation and code
            if (originMetaObject is OOAdvantech.MetaDataRepository.Method && originOperation.Owner is OOAdvantech.MetaDataRepository.Class)
                (VSOperation as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;
            else
            {
                if (Owner is OOAdvantech.MetaDataRepository.Class)
                {
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
                }
                //  CodeClassifier.GetOverrideKind(originOperation.ov

            }


            bool changeVisibility = Visibility != originOperation.Visibility;
            Visibility = originOperation.Visibility;
            if (changeVisibility)
            {
                if (VSAccessTypeConverter.GetAccessType(Visibility) == EnvDTE.vsCMAccess.vsCMAccessProject &&
                    VSOperation.Access == EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected)
                {
                    VSOperation.Access = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;
                }
                else
                    VSOperation.Access = VSAccessTypeConverter.GetAccessType(Visibility);
            }


            bool changeReturnType = true;
            if (ReturnType != null && originOperation.ReturnType != null && _ReturnType.FullName == originOperation.ReturnType.FullName)
                changeReturnType = false;

            if (changeReturnType)
            {
                _ReturnType = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originOperation.ReturnType, this) as OOAdvantech.MetaDataRepository.Classifier;
                if (_ReturnType == null)
                    _ReturnType = UnknownClassifier.GetClassifier(originOperation.ReturnType.FullName, ImplementationUnit);

                if (VSOperation.Name != Owner.Name)
                    VSOperation.Type = LanguageParser.CreateCodeTypeRef(_ReturnType, VSOperation.ProjectItem.ContainingProject.CodeModel);
            }

            base.PutPropertyValue("MetaData", "Documentation", originOperation.GetPropertyValue(typeof(string), "MetaData", "Documentation"));

            if (GetSignature(originOperation, ImplementationUnit as Project) != GetSignature(this, ImplementationUnit as Project))
            {
                _Parameters.RemoveAll();
                int i = 0;
                foreach (MetaDataRepository.Parameter parameter in originOperation.Parameters)
                {

                    MetaDataRepository.Classifier classifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(parameter.Type, this) as MetaDataRepository.Classifier;
                    if (classifier == null)
                        classifier = UnknownClassifier.GetClassifier(parameter.Type.FullName, ImplementationUnit);
                    _Parameters.Add(new MetaDataRepository.Parameter(parameter.Name, classifier));
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
            SetDocumentation(GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);
            #endregion

            MetaObjectChangeState();
        }

        /// <MetaDataID>{a54ef4ee-9ef9-4994-b19c-dccbae57b4da}</MetaDataID>
        private string CodeTypeFullName
        {
            get
            {
                if (CodeFunctionType == EnvDTE.vsCMFunction.vsCMFunctionConstructor)
                    return null;
                return LanguageParser.GetTypeFullName(_ReturnType, (ImplementationUnit as Project).VSProject);
            }
        }

        /// <MetaDataID>{00cc2871-c3dc-4f23-92ec-50e9eaf21987}</MetaDataID>
        private EnvDTE.vsCMFunction CodeFunctionType
        {
            get
            {
                if (_Name == Owner.Name)
                    return EnvDTE.vsCMFunction.vsCMFunctionConstructor;
                string name = _Name;
                if (name.Contains("operator"))
                    name = name.Replace("operator", "").Trim();
                if (name == "=" ||
                    name == ">" ||
                    name == "<" ||
                    name == ">=" ||
                    name == "<=" ||
                    name == "==" ||
                    name == "+" ||
                    name == "-" ||
                    name == "*" ||
                    name == "/" ||
                    name == "+=" ||
                    name == "-=" ||
                    name == "*=" ||
                    name == "/=")
                    return EnvDTE.vsCMFunction.vsCMFunctionOperator;







                return EnvDTE.vsCMFunction.vsCMFunctionFunction;
            }


        }
        /// <MetaDataID>{fdc459d4-73c2-4e03-8585-6ecd34d0f3a1}</MetaDataID>
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
                    //VSOperation.Name = name + name;
                    //VSOperation.Name = _Name;
                    return;
                }
            }

            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
            VSOperation.DocComment = document.OuterXml;
            //VSOperation.Name = name + name;
            //VSOperation.Name = _Name;


        }

        /// <MetaDataID>{1c3fa556-b58a-4109-92d5-67b41334815b}</MetaDataID>
        internal Operation()
        {
        }
        /// <MetaDataID>{8280a4f4-ccae-4701-b2b0-0b44c34c2a27}</MetaDataID>
        public Operation(string name, MetaDataRepository.Classifier owner)
        {
            _Name = name;
            _Owner = owner;
            _Namespace.Value = owner;
        }
        /// <MetaDataID>{3bc47624-5bf9-4233-85ca-cdc142795687}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{52e43d9e-4828-462a-b261-2f623fdf3434}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{942ee457-0252-4c9b-9c95-2ce01d56c5a6}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementFunction)
                return false;


            MetaDataRepository.Operation operation = CodeClassifier.GetOperationForMethod(Owner, codeElement as EnvDTE80.CodeFunction2);
            if (operation != null && operation != this)
                return false;
            if (this.VSOperation == codeElement)
                return true;

            if (_ProjectItem != ProjectItem.GetProjectItem(codeElement.ProjectItem))
                return false;


            EnvDTE.CodeElement vsOperationParent = null; ;
            EnvDTE.CodeElement codeElementParent = null;

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
                {
                    if (Parameters.Count != (codeElement as EnvDTE.CodeFunction).Parameters.Count)
                        return false;

                    int i = 0;
                    foreach (EnvDTE.CodeParameter parameter in ((codeElement as EnvDTE.CodeFunction).Parameters))
                    {
                        if (parameter.Type.AsFullName != Parameters[i].Type.FullName)
                            return false;
                        i++;
                    }
                    if (_IsStatic == (codeElement as EnvDTE.CodeFunction).IsShared)
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



        /// <MetaDataID>{A4BFF0AF-AE38-493F-AE37-528600E78A05}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            VSOperation = null;
            MetaObjectMapper.RemoveMetaObject(this);
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);
            if (_Owner != null)
                _Owner.RemoveOperation(this);

            _Owner = null;
            _ProjectItem = null;
        }
        /// <MetaDataID>{eff685ab-486e-4ca6-bc90-6da22ccb85e6}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        /// <MetaDataID>{7D15D093-3062-4E95-B870-249D720EC706}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{54C2C5BE-251B-43BB-9AA6-9DDCAD2934A8}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{C60D0593-8110-4AF5-8531-AEAB7F9CC849}</MetaDataID>
        public int Line
        {
            get
            {
                try
                {
                    _Line = CodeElement.StartPoint.Line;
                }
                catch (System.Exception error)
                {
                }

                return _Line;
            }
            set
            {
                if (_Line != value)
                {
                    _Line = value;
                    // MetaObjectChangeState();
                }
            }
        }
        /// <MetaDataID>{222F5387-6024-47F4-9E83-8EC067BC620A}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                try
                {
                    _LineCharOffset = CodeElement.StartPoint.LineCharOffset;
                }
                catch (System.Exception error)
                {
                }
                return _LineCharOffset;
            }
        }
        /// <MetaDataID>{c6edf2b8-0cfc-4667-9b25-5c48c8730d74}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            return _Line;
        }
        /// <MetaDataID>{8bd65a65-c7ad-4bbe-b825-3b52815b5d73}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return _LineCharOffset;
        }
        /// <MetaDataID>{e2cbf5a5-e7fa-4407-8edc-faa2bba1566e}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }

        /// <MetaDataID>{1B98FB31-9610-4F50-9406-977F3C3F37D7}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            bool changed = false;
            MetaObjectMapper.RemoveMetaObject(this);
            VSOperation = codeElement as EnvDTE.CodeFunction;
            MetaObjectMapper.AddTypeMap(VSOperation, this);

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

            if (_Name != VSOperation.Name)
            {
                _Name = VSOperation.Name;
                changed = true;
            }
            if (_ProjectItem != null && ProjectItem.GetProjectItem(VSOperation.ProjectItem) != _ProjectItem)
                _ProjectItem.RemoveMetaObject(this);
            _ProjectItem = ProjectItem.AddMetaObject(VSOperation.ProjectItem, this);

            if (Operation.GetSignature(VSOperation).ToLower().Trim() != GetSignature(this, ImplementationUnit as Project).ToLower().Trim())
            {
                Collections.Generic.Set<MetaDataRepository.Parameter> oldParameters = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Parameter>(_Parameters);
                _Parameters.RemoveAll();
                LoadParameters();
                changed = true;
            }
            else
            {
                int i = 0;
                foreach (EnvDTE.CodeParameter codeParameter in VSOperation.Parameters)
                {
                    if (_Parameters[i].Name != codeParameter.Name)
                    {
                        _Parameters[i].Name = codeParameter.Name;
                        _Parameters[i].MetaObjectChangeState();
                    }
                    i++;

                }
            }

            if (_IsStatic != VSOperation.IsShared)
            {
                _IsStatic = VSOperation.IsShared;
                changed = true;
            }
            MetaDataRepository.VisibilityKind newVisibilityKind = VSAccessTypeConverter.GetVisibilityKind(VSOperation.Access);
            if (Visibility != newVisibilityKind)
            {
                Visibility = newVisibilityKind;
                changed = true;
            }

            try
            {
                bool sameReturnType = false;
                EnvDTE.CodeTypeRef codeFunctionReturnType = null;
                string codeFunctionReturnTypeFullName = null;
                try
                {
                    codeFunctionReturnType = VSOperation.Type;
                }
                catch (System.Exception error)
                {
                }
                if (codeFunctionReturnType != null)
                {
                    try
                    {
                        codeFunctionReturnTypeFullName = codeFunctionReturnType.AsFullName;
                        if (string.IsNullOrEmpty(codeFunctionReturnTypeFullName))
                            codeFunctionReturnTypeFullName = codeFunctionReturnType.AsString;

                    }
                    catch (System.Exception error)
                    {
                        try
                        {
                            codeFunctionReturnTypeFullName = VSOperation.Type.AsString;
                        }
                        catch (System.Exception errorB)
                        {
                        }
                    }
                }



                if (_ReturnType != null)
                {
                    if (codeFunctionReturnTypeFullName == LanguageParser.GetTypeFullName(_ReturnType, (ImplementationUnit as Project).VSProject))
                        sameReturnType = true;


                }
                if (!sameReturnType)
                {

                    MetaDataRepository.Classifier returnType = (Owner.ImplementationUnit as Project).GetClassifier(codeFunctionReturnType);
                    if (returnType == null)
                    {
                        try
                        {
                            returnType = UnknownClassifier.GetClassifier(codeFunctionReturnTypeFullName, ImplementationUnit);
                        }
                        catch (System.Exception error)
                        {
                            returnType = UnknownClassifier.GetClassifier(codeFunctionReturnTypeFullName, ImplementationUnit);
                        }

                    }

                    if ((_ReturnType == null && returnType != null) ||
                        (_ReturnType == null && returnType != null))
                        changed = true;
                    else if (_ReturnType != null && returnType != null && _ReturnType.FullName != returnType.FullName)
                        changed = true;
                    _ReturnType = returnType;
                }
            }
            catch (System.Exception Error)
            {

            }
            if (changed)
                MetaObjectChangeState();



        }
        /// <MetaDataID>{971FD223-C0F3-475B-A576-2B43EB88A2A0}</MetaDataID>
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
                try
                {
                    //(Owner as CodeElementContainer).RefreshCodeElement((Owner as CodeElementContainer).CodeElement);
                }
                catch (System.Exception exp)
                {


                }
            }

        }
        /// <MetaDataID>{A9539A4D-6C96-4641-98A6-831E0FC08A25}</MetaDataID>
        internal EnvDTE.CodeFunction VSOperation;

        /// <MetaDataID>{3c4681e8-1e18-446e-8923-b830194eb59d}</MetaDataID>
        void LoadParameters()
        {

            if (VSOperation == null)
                return;
            int i = 0;
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                document.LoadXml(VSOperation.DocComment);

            }
            catch (System.Exception error)
            {
                document.LoadXml("<doc></doc>");
            }
            foreach (EnvDTE.CodeParameter vsParameter in VSOperation.Parameters)
            {
                if (vsParameter.Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefOther)
                {
                    if (_Owner.OwnedTemplateSignature != null)
                    {
                        MetaDataRepository.TemplateParameter templateParameter = _Owner.OwnedTemplateSignature.GetParameterWithName(vsParameter.Type.AsString);
                        MetaDataRepository.Parameter parameter = new OOAdvantech.MetaDataRepository.Parameter(vsParameter.Name, templateParameter);
                        _Parameters.Add(parameter);
                        i++;

                        try
                        {
                            foreach (System.Xml.XmlNode node in document.DocumentElement)
                            {
                                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                                if (element == null)
                                    continue;
                                if (element.Name.ToLower() == "param".ToLower() && element.GetAttribute("name") == vsParameter.Name)
                                {
                                    parameter.PutPropertyValue("MetaData", "Documentation", element.InnerText);
                                }
                            }
                        }
                        catch (System.Exception error)
                        {
                        }
                    }
                }
                else
                {
                    MetaDataRepository.Classifier parameterType = null;
                    try
                    {
                        if (vsParameter.Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
                        {
                            if (_Owner.OwnedTemplateSignature != null)
                                parameterType = (_Owner.ImplementationUnit as Project).GetClassifier(_Owner, vsParameter.Type.CodeType as EnvDTE.CodeElement);
                            else
                                parameterType = (Owner.ImplementationUnit as Project).GetClassifier(vsParameter.Type);
                        }
                        if (parameterType == null)
                        {
                            string typeFullName = null;
                            if (!string.IsNullOrEmpty(vsParameter.Type.AsFullName))

                                typeFullName = vsParameter.Type.AsFullName;
                            else if (!string.IsNullOrEmpty(vsParameter.Type.AsString))
                                typeFullName = vsParameter.Type.AsString;

                            if (!string.IsNullOrEmpty(typeFullName))
                                parameterType = UnknownClassifier.GetClassifier(typeFullName, ImplementationUnit);
                        }

                    } 
                    catch (System.Exception error)
                    {
                        parameterType = UnknownClassifier.GetClassifier("object", ImplementationUnit);

                    }
                    MetaDataRepository.Parameter parameter = new OOAdvantech.MetaDataRepository.Parameter(vsParameter.Name, parameterType);
                    i++;
                    _Parameters.Add(parameter);
                    try
                    {
                        foreach (System.Xml.XmlNode node in document.DocumentElement)
                        {
                            System.Xml.XmlElement element = node as System.Xml.XmlElement;
                            if (element == null)
                                continue;
                            if (element.Name.ToLower() == "param".ToLower() && element.GetAttribute("name") == vsParameter.Name)
                            {
                                parameter.PutPropertyValue("MetaData", "Documentation", element.InnerText);
                            }
                        }
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
            ParametersLoaded = true;
        }
        /// <MetaDataID>{7d7ed741-467e-458f-904d-79f2177b79e1}</MetaDataID>
        public Operation(MetaDataRepository.Operation genericOperation, MetaDataRepository.Classifier owner)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            _Name = genericOperation.Name;
            if (genericOperation.Name == genericOperation.Owner.Name)
                _Name = LanguageParser.GetTypeName(owner, (owner.ImplementationUnit as Project).VSProject);
            _IsStatic = genericOperation.IsStatic;
            Visibility = genericOperation.Visibility;

            if (genericOperation.ReturnType != null)
                ReturnType = CodeClassifier.GetActualType(genericOperation.ReturnType, Owner.TemplateBinding);
            else if (genericOperation.ParameterizedReturnType != null)
            {
                MetaDataRepository.IParameterableElement actualParameter = owner.TemplateBinding.GetActualParameterFor(genericOperation.ParameterizedReturnType);
                if (actualParameter is MetaDataRepository.Classifier)
                    ReturnType = actualParameter as MetaDataRepository.Classifier;
                else
                    ParameterizedReturnType = actualParameter as MetaDataRepository.TemplateParameter;
            }

            foreach (MetaDataRepository.Parameter genericOperationParameter in genericOperation.Parameters)
            {
                MetaDataRepository.Parameter parameter = null;
                if (genericOperationParameter.Type != null)
                    parameter = new OOAdvantech.MetaDataRepository.Parameter(genericOperationParameter.Name, CodeClassifier.GetActualType(genericOperationParameter.Type, Owner.TemplateBinding));

                else if (genericOperationParameter.ParameterizedType != null)
                {
                    MetaDataRepository.IParameterableElement actualParameter = owner.TemplateBinding.GetActualParameterFor(genericOperationParameter.ParameterizedType);
                    if (actualParameter is MetaDataRepository.Classifier)
                        parameter = new OOAdvantech.MetaDataRepository.Parameter(genericOperationParameter.Name, actualParameter as MetaDataRepository.Classifier);
                    else
                        parameter = new OOAdvantech.MetaDataRepository.Parameter(genericOperationParameter.Name, actualParameter as MetaDataRepository.TemplateParameter);
                }
                _Parameters.Add(parameter);
            }

        }

        /// <MetaDataID>{9e39e1b6-3966-4d23-8112-b626d6555731}</MetaDataID>
        bool ParametersLoaded = false;
        /// <MetaDataID>{387081df-1ece-4be0-bedc-3c651fffc0c0}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Parameter> Parameters
        {
            get
            {
                try
                {
                    if (!ParametersLoaded)
                        LoadParameters();
                    return base.Parameters;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
        }

        /// <MetaDataID>{b01edc64-6c80-4fd4-9b5f-6a453ad22932}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.TemplateParameter ParameterizedReturnType
        {
            get
            {

                if (!ReturnTypeLoaded)
                {
                    LoadReturnType();
                    ReturnTypeLoaded = true;
                }

                return base.ParameterizedReturnType;
            }
            set
            {
                base.ParameterizedReturnType = value;
            }
        }

        /// <MetaDataID>{4708b2bf-1be9-458d-9302-f034c0e1e626}</MetaDataID>
        bool ReturnTypeLoaded = false;
        /// <MetaDataID>{0edf8c5f-1131-4ae1-8f2c-a0baacb9425a}</MetaDataID>
        void LoadReturnType()
        {
            try
            {
                if (ReturnTypeLoaded || VSOperation == null)
                    return;


                if (VSOperation.Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefOther)
                {
                    if (_Owner.OwnedTemplateSignature != null)
                        _ParameterizedReturnType = _Owner.OwnedTemplateSignature.GetParameterWithName(VSOperation.Type.AsString);
                }

                if (_ParameterizedReturnType == null)
                {

                    if (VSOperation.Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
                    {
                        if (_Owner.OwnedTemplateSignature != null)
                            _ReturnType = (_Owner.ImplementationUnit as Project).GetClassifier(_Owner, VSOperation.Type.CodeType as EnvDTE.CodeElement);
                        else
                            _ReturnType = (Owner.ImplementationUnit as Project).GetClassifier(VSOperation.Type);
                    }

                    if (_ReturnType == null)
                    {
                        string typeFullName = null;
                        if (!string.IsNullOrEmpty(VSOperation.Type.AsFullName))

                            typeFullName = VSOperation.Type.AsFullName;
                        else if (!string.IsNullOrEmpty(VSOperation.Type.AsString))
                            typeFullName = VSOperation.Type.AsString;

                        if (!string.IsNullOrEmpty(typeFullName))
                            _ReturnType = UnknownClassifier.GetClassifier(typeFullName, ImplementationUnit);
                    }
                }
                ReturnTypeLoaded = true;
            }
            catch (System.Exception error)
            {

            }

        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        /// <MetaDataID>{7e63f6bb-da44-4120-b83b-b04c3fbbe30c}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }
        /// <MetaDataID>{C2836FD9-146E-4D8D-BEB4-044C4870463D}</MetaDataID>
        public Operation(EnvDTE.CodeFunction vsOperation, MetaDataRepository.Classifier owner)
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
            _IsStatic = VSOperation.IsShared;
            Visibility = VSAccessTypeConverter.GetVisibilityKind(VSOperation.Access);
            _ProjectItem = ProjectItem.AddMetaObject(VSOperation.ProjectItem, this);
            RefreshStartPoint();
            MetaObjectMapper.AddTypeMap(VSOperation, this);
        }




        /// <MetaDataID>{890FFA64-63CE-4500-B411-2575AAD3EBD9}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier ReturnType
        {
            get
            {
                if (!ReturnTypeLoaded)
                {
                    LoadReturnType();
                    ReturnTypeLoaded = true;
                }
                return base.ReturnType;
            }
            set
            {
                base.ReturnType = value;
            }
        }





        /// <MetaDataID>{D4B6FE09-299C-4DF1-8D0A-A72F102D23F0}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSOperation as EnvDTE.CodeElement;
            }
        }



        /// <MetaDataID>{fbed43da-e218-45f0-8767-55a0845c5836}</MetaDataID>
        internal static string GetSignature(OOAdvantech.MetaDataRepository.Operation operation, OOAdvantech.MetaDataRepository.TemplateBinding TemplateBinding, Project project)
        {

            try
            {
                string operationSignature = null;
                foreach (MetaDataRepository.Parameter parameter in operation.Parameters)
                {
                    if (operationSignature == null)
                        operationSignature = operation.Name + "(";
                    else
                        operationSignature += ",";
                    if (parameter.ParameterizedType != null)
                        operationSignature += TemplateBinding.GetActualParameterFor(parameter.ParameterizedType).FullName;
                    else
                    {
                        if (parameter.Type.IsTemplate)
                        {
                            Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<MetaDataRepository.IParameterableElement>();
                            foreach (MetaDataRepository.TemplateParameterSubstitution orgParameterSubstitution in parameter.Type.TemplateBinding.ParameterSubstitutions)
                                parameterSubstitutions.Add(TemplateBinding.GetActualParameterFor(parameter.Type.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) as MetaDataRepository.TemplateParameter));
                            if (parameter.Type.Namespace == null)
                                operationSignature += LanguageParser.GetTemplateInstantiationName(new MetaDataRepository.TemplateBinding(parameter.Type.TemplateBinding.Signature.Template, parameterSubstitutions), project.VSProject.CodeModel.Language);
                            else
                                operationSignature += parameter.Type.Namespace.FullName + "." + LanguageParser.GetTemplateInstantiationName(new MetaDataRepository.TemplateBinding(parameter.Type.TemplateBinding.Signature.Template, parameterSubstitutions), project.VSProject.CodeModel.Language);

                        }
                        else
                            operationSignature += LanguageParser.GetTypeFullName(parameter.Type, project.VSProject);
                    }
                }
                if (operationSignature == null)
                    operationSignature = operation.Name + "()";
                else
                    operationSignature += ")";
                return operationSignature;
            }
            catch (System.Exception error)
            {
                throw;
            }

        }
    }
}
