namespace OOAdvantech.CodeMetaDataRepository
{
    using Transactions;
    using System.Reflection;
    using System.Linq;
    /// <MetaDataID>{090A0499-3DE4-49B3-A260-027010BF3553}</MetaDataID>
    public class Interface : OOAdvantech.MetaDataRepository.Interface, CodeElementContainer
    {
        /// <MetaDataID>{d6ad22b6-df1a-4749-ab67-6759f2500435}</MetaDataID>
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
        /// <MetaDataID>{ddd9ead7-11e8-42c9-a968-c0fa7a68b026}</MetaDataID>
        public string CurrentProgramLanguageFullName
        {
            get
            {
                return LanguageParser.GetTypeFullName(this, _ProjectItem.Project.VSProject);
            }
        }

        /// <MetaDataID>{19758a83-1f75-4d6b-aa68-9099a1e71b34}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return LanguageParser.GetTypeName(this, _ProjectItem.Project.VSProject);
            }
        }
        /// <MetaDataID>{467fc604-0242-4b59-acbf-680b4d9905ac}</MetaDataID>
        protected override void SetIdentity(OOAdvantech.MetaDataRepository.MetaObjectID theIdentity)
        {
            if (_Identity != null && _Identity == theIdentity)
                return;
            _Identity = null;
            base.SetIdentity(theIdentity);
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();



            try
            {
                document.LoadXml(VSInterface.DocComment);
            }
            catch (System.Exception error)
            {
                document.LoadXml("<doc></doc>");
            }
            //string name = _Name;
            foreach (System.Xml.XmlNode node in document.DocumentElement)
            {
                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                if (element == null)
                    continue;
                if (element.Name.ToLower() == "MetaDataID".ToLower())
                {
                    element.InnerText = _Identity.ToString();
                    VSInterface.DocComment = document.OuterXml;
                    //VSInterface.Name = name + name;
                    //VSInterface.Name = _Name ;

                    return;
                }
            }

            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
            VSInterface.DocComment = document.OuterXml;
            //VSInterface.Name = name + name;
            //VSInterface.Name = _Name;


        }

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSInterface as EnvDTE.CodeElement, identity);
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
                if (VSInterface == null || TemplateBinding != null)
                    return base.Identity.ToString();

                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSInterface as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSInterface as EnvDTE.CodeElement, identity);
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

        ///// <summary></summary>
        ///// <MetaDataID>{69ded4fa-2bac-45b6-89b0-0cec0ce89184}</MetaDataID>
        //public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        //{
        //    get
        //    {
        //        if (_Identity != null)
        //            return _Identity;
        //        if (VSInterface == null || TemplateBinding != null)
        //            return base.Identity;
        //        foreach (EnvDTE.CodeAttribute attribute in VSInterface.Attributes)
        //        {
        //            if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute))
        //            {
        //                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(attribute.Value);
        //                break;
        //            }
        //        }


        //        System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //        document.LoadXml(VSInterface.DocComment);

        //        foreach (System.Xml.XmlNode node in document.DocumentElement)
        //        {
        //            System.Xml.XmlElement element = node as System.Xml.XmlElement;
        //            if (element == null)
        //                continue;
        //            if (element.Name.ToLower() == "summary".ToLower())
        //                base.PutPropertyValue("MetaData", "Documentation", element.InnerText);
        //            if (element.Name.ToLower() == "MetaDataID".ToLower())
        //                _Identity = new MetaDataRepository.MetaObjectID(element.InnerText);

        //        }
        //        if (_Identity != null)
        //            return _Identity;
        //        System.Guid guid = System.Guid.NewGuid();
        //        try
        //        {
        //            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("{" + guid.ToString() + "}");
        //            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
        //            VSInterface.DocComment = document.OuterXml;
        //        }
        //        catch
        //        {
        //        }
        //        return _Identity;
        //    }
        //}

        /// <MetaDataID>{2dcd5e9d-cfd3-4da8-a87e-f13af19c282f}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
                return;
            MetaObjectsStack.ActiveProject = ImplementationUnit as Project;

            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(VSInterface.DocComment);

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
                    VSInterface.DocComment = document.OuterXml;
                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("summary")).InnerText = documentation.ToString();
            VSInterface.DocComment = document.OuterXml;
        }

        /// <MetaDataID>{2bca4839-fb90-4af1-aef5-4ffa87cd3d68}</MetaDataID>
        System.Collections.Generic.Queue<EnvDTE.CodeElement> MembersCodeElement = new System.Collections.Generic.Queue<EnvDTE.CodeElement>();
        /// <MetaDataID>{5b5aa3c6-7659-4c85-8224-1d174648bdb4}</MetaDataID>
        bool InGetRole = false;
        /// <MetaDataID>{bb74379a-9fec-4ba8-aa37-056cc9506f0b}</MetaDataID>
        AssociationEnd InGetRoleAssociationEnd;
        /// <MetaDataID>{458bd8b6-30f3-424d-ace2-4b079ccbadac}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.AssociationEnd GetRole(string associationEndIdentity)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("GetRole", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[1] { typeof(string) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[1] { associationEndIdentity }) as OOAdvantech.MetaDataRepository.AssociationEnd;

            }
            try
            {
                if (InLoadRoles || IsRolesLoaded || InGetRole)
                {
                    foreach (MetaDataRepository.AssociationEnd associationEnd in _Roles)
                    {
                        if (associationEnd.Identity.ToString() == associationEndIdentity)
                            return associationEnd;
                    }
                }
                if (InGetRole)
                    return null;
                try
                {
                    InGetRole = true;
                    System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
                    MetaDataRepository.AssociationEnd associationEnd = CodeClassifier.GetClassifierRole(associationEndIdentity, this, _Roles, members);
                    if (associationEnd == null)
                    {
                        foreach (MetaDataRepository.AssociationEnd role in _Roles)
                        {
                            if (role.Identity.ToString() == associationEndIdentity)
                                return role;
                        }
                    }
                    else
                        return associationEnd;

                }
                finally
                {
                    InGetRole = false;

                }

                return null;
            }
            catch (System.Exception error)
            {
                throw;

            }


        }
        /// <MetaDataID>{42ed32dd-494a-464a-bfe6-8d4ffdc9f727}</MetaDataID>
        bool InLoadRoles = false;

        /// <MetaDataID>{a4492b95-7b50-43a4-a287-175dd38babe2}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }


            if (OriginMetaObject.Namespace != null && VSInterface.Namespace == null)
            {
                MetaDataRepository.MetaObjectID identity = _Identity;
                VSInterface.EndPoint.CreateEditPoint().Insert("\r\n}");
                EnvDTE.EditPoint editPoint = VSInterface.StartPoint.CreateEditPoint();
                int line = editPoint.Line + 2;
                editPoint.Insert("namespace " + OriginMetaObject.Namespace.FullName + "\r\n{\r\n");
                VSInterface = editPoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementInterface) as EnvDTE.CodeInterface;
                _Identity = null;
                if (identity != null)
                    SetIdentity(identity);
                Namespace _namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSInterface.Namespace.FullName);
                if (_namespace == null)
                    _namespace = new Namespace(VSInterface.Namespace);
                SetNamespace(_namespace);
                _namespace.AddOwnedElement(this);

                if (ImplementationUnit != null)
                    ImplementationUnit.MetaObjectChangeState();
            }

            MetaObjectsStack.ActiveProject = ImplementationUnit as Project;

            long count = Features.Count;
            count = Roles.Count;
            base.Synchronize(OriginMetaObject);
            VSInterface.Name = _Name;

            #region Update class attributes
            bool linkClass = (OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation != null;
            bool linkClassAttributeExist = false;
            bool backwardCompatibilityIDExist = false;
            string backwardCompatibilityID = GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;


            EnumerateAttributes:
            foreach (EnvDTE.CodeAttribute attribute in VSInterface.Attributes)
            {
                try
                {
                    //string attributeName = null;
                    //try
                    //{
                    //    attributeName = attribute.FullName;
                    //}
                    //catch (System.Exception error)
                    //{
                    //    attributeName = attribute.Name;

                    //}
                    //if ((attributeName == typeof(MetaDataRepository.AssociationClass).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, VSInterface.ProjectItem) && !linkClass))
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationClass), attribute) && !linkClass)
                    {
                        attribute.Delete();
                        goto EnumerateAttributes;
                    }
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationClass), attribute) && linkClass)
                    {
                        linkClassAttributeExist = true;
                        attribute.Value = "typeof(" + LanguageParser.GetShortName((OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleA.Specification.FullName, VSInterface as EnvDTE.CodeElement) + ")," + "typeof(" + LanguageParser.GetShortName((OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleB.Specification.FullName, VSInterface as EnvDTE.CodeElement) + "),\"" + (OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.Name + "\"";

                    }
                    //if ((attributeName == typeof(MetaDataRepository.BackwardCompatibilityID).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSInterface.ProjectItem) && string.IsNullOrEmpty(backwardCompatibilityID)))
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute) && string.IsNullOrEmpty(backwardCompatibilityID))
                    {
                        attribute.Delete();
                        goto EnumerateAttributes;
                    }
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute) && !string.IsNullOrEmpty(backwardCompatibilityID))
                    {

                        if (string.IsNullOrEmpty(backwardCompatibilityID))
                        {
                            attribute.Delete();
                            goto EnumerateAttributes;
                        }
                        else
                            attribute.Value = "\"" + backwardCompatibilityID + "\"";
                        backwardCompatibilityIDExist = true;
                    }
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute))
                        backwardCompatibilityIDExist = true;
                }
                catch (System.Exception error)
                {
                }
            }
            int attributePos = 0;
            if (!backwardCompatibilityIDExist && !string.IsNullOrEmpty(backwardCompatibilityID))
                VSInterface.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSInterface as EnvDTE.CodeElement), "\"" + backwardCompatibilityID + "\"", attributePos++);

            if (linkClass && !linkClassAttributeExist)
                VSInterface.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, VSInterface as EnvDTE.CodeElement), "typeof(" + LanguageParser.GetShortName((OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleA.Specification.FullName, VSInterface as EnvDTE.CodeElement) + ")," + "typeof(" + LanguageParser.GetShortName((OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleB.Specification.FullName, VSInterface as EnvDTE.CodeElement) + "),\"" + (OriginMetaObject as MetaDataRepository.Classifier).LinkAssociation.Name + "\"", attributePos++);
            #endregion
            SetDocumentation(GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);
            SetIdentity(OriginMetaObject.Identity);
            string ful = OriginMetaObject.Namespace.FullName;
            if (VSInterface.Namespace.Name != OriginMetaObject.Namespace.FullName)
                VSInterface.Namespace.Name = OriginMetaObject.Namespace.FullName;

            #region Write generic parameters


            if (IsTemplate)
            {
                string fullName = VSInterface.FullName;
                if (VSInterface.Namespace != null)
                    fullName = fullName.Replace(VSInterface.Namespace.FullName + ".", "");
                EnvDTE.EditPoint edit = VSInterface.StartPoint.CreateEditPoint();

                string templateSignature = LanguageParser.GetTemplateSignature(OwnedTemplateSignature, VSInterface as EnvDTE.CodeElement);
                if (fullName != Name + templateSignature)
                {
                    int startLine = VSInterface.StartPoint.Line;
                    int endLine = VSInterface.EndPoint.Line;
                    int currentLine = startLine;
                    while (currentLine <= endLine)
                    {
                        string lineString = edit.GetLines(currentLine, currentLine + 1);
                        if (lineString.IndexOf(fullName) != -1)
                        {
                            edit.StartOfLine();
                            edit.Delete(lineString.Length);

                            int nPos = lineString.IndexOf(fullName);
                            lineString = lineString.Remove(nPos, fullName.Length);
                            lineString = lineString.Insert(nPos, Name + templateSignature);
                            edit.Insert(lineString);
                            VSInterface = edit.get_CodeElement(EnvDTE.vsCMElement.vsCMElementInterface) as EnvDTE.CodeInterface;
                            break;
                        }
                        if (lineString.IndexOf("{") != -1)
                            break;
                        edit.LineDown(1);
                        currentLine++;
                    }
                }
            }
            #endregion
            MetaObjectChangeState();

        }

        //TODO Να φτιαχτεί test case για την περίπτωση που είναι template class
        //ή instandieted class
        /// <MetaDataID>{45f55e95-6800-4646-8d6a-6bdd64fb2240}</MetaDataID>
        bool IsGeneralizationLoaded = false;

        /// <MetaDataID>{d87c3f42-2ff8-4ef8-98ab-8c27214ba51e}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {

                if (VSInterface != null)
                {
                    try
                    {

                        int baseClassCount = 0;


                        foreach (EnvDTE.CodeElement codeElement in VSInterface.Bases)
                        {
                            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface || codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                                continue;
                            baseClassCount++;
                            bool exist = (from generalization in _Generalizations
                                          where generalization.Parent.FullName == codeElement.FullName
                                          select generalization).FirstOrDefault() != null;
                            if (!exist)
                            {
                                IsGeneralizationLoaded = false;
                                _Generalizations.Clear();
                                break;
                            }
                        }


                        if (baseClassCount != _Generalizations.Count)
                        {
                            IsGeneralizationLoaded = false;
                            _Generalizations.Clear();
                        }

                    }
                    catch (System.Exception error)
                    {

                    }
                }

                if (!IsGeneralizationLoaded)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        #region Load generalization relationships
                        IsGeneralizationLoaded = true;
                        if (TemplateBinding != null)
                        {
                            foreach (MetaDataRepository.Generalization generalization in (TemplateBinding.Signature.Template as MetaDataRepository.Classifier).Generalizations)
                            {
                                if (!generalization.Parent.IsTemplate && !generalization.Parent.IsTemplateInstantiation)
                                {
                                    _Generalizations.Add(new Generalization("", generalization.Parent, this));
                                }
                                else if (generalization.Parent.IsTemplate && generalization.Parent.IsBindedClassifier)
                                {
                                    MetaDataRepository.Classifier classifier = CodeClassifier.GetActualType(generalization.Parent, TemplateBinding);

                                    //Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<MetaDataRepository.IParameterableElement>();
                                    //foreach (MetaDataRepository.TemplateParameterSubstitution orgParameterSubstitution in generalization.Parent.TemplateBinding.ParameterSubstitutions)
                                    //    parameterSubstitutions.Add(TemplateBinding.GetActualParameterFor(generalization.Parent.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) as MetaDataRepository.TemplateParameter));
                                    //MetaDataRepository.Interface classifier = new Interface(new MetaDataRepository.TemplateBinding(generalization.Parent.TemplateBinding.Signature.Template, parameterSubstitutions), ImplementationUnit);
                                    _Generalizations.Add(new Generalization("", classifier, this));
                                }

                            }
                            return base.Generalizations;
                        }
                        else
                        {
                            foreach (EnvDTE.CodeElement codeElement in VSInterface.Bases)
                            {
                                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface)
                                {

                                    MetaDataRepository.Classifier classifier = (ImplementationUnit as Project).GetClassifier(this, codeElement);
                                    _Generalizations.Add(new Generalization("", classifier, this));
                                }
                            }
                        }
                        #endregion
                        stateTransition.Consistent = true;
                    }

                }

                return base.Generalizations;
            }
        }
        //private OOAdvantech.MetaDataRepository.Classifier GetClassifier(EnvDTE.CodeElement codeElement)
        //{
        //    MetaDataRepository.Classifier classifier = null;
        //    if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationExternal)
        //    {
        //        Project project = MetaObjectMapper.FindMetaObjectFor(VSInterface.ProjectItem.ContainingProject) as Project;


        //        if (((codeElement is EnvDTE80.CodeClass2) && (codeElement as EnvDTE80.CodeClass2).IsGeneric) || ((codeElement is EnvDTE80.CodeInterface2) && (codeElement as EnvDTE80.CodeInterface2).IsGeneric))
        //        {
        //            string typeFullName = null;
        //            System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();

        //            LanguageParser.GetGenericMetaDataFromCSharpType(codeElement.FullName, ref typeFullName, ref parameters);
        //            if (OwnedTemplateSignature != null)
        //            {
        //                classifier = project.GetExternalClassifier(typeFullName + "`" + parameters.Count.ToString());
        //            }
        //            else
        //            {
        //                Collections.Generic.List<MetaDataRepository.IParameterableElement> parametersClasifiers = new Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
        //                bool genericCodeType = false;
        //                string genericTypeFullName = typeFullName + "`" + parameters.Count.ToString();
        //                typeFullName += "`" + parameters.Count.ToString() + "[";

        //                foreach (string parameter in parameters)
        //                {
        //                    if (!LanguageParser.IsGeneric(parameter, codeElement.Language))
        //                    {
        //                        MetaDataRepository.Classifier parameterClassifier = project.GetExternalClassifier(parameter);
        //                        parametersClasifiers.Add(parameterClassifier);
        //                        if (parameterClassifier.ImplementationUnit is Project)
        //                            genericCodeType = true;
        //                        typeFullName += "[" + parameterClassifier.FullName + "," + parameterClassifier.ImplementationUnit.Identity + "]";
        //                    }
        //                }
        //                typeFullName += "]";
        //                classifier = project.GetExternalClassifier(typeFullName, genericCodeType);
        //                if (classifier == null)
        //                {
        //                    MetaDataRepository.Classifier genericClassifier = project.GetExternalClassifier(genericTypeFullName);
        //                    MetaDataRepository.TemplateBinding templateBinding = new OOAdvantech.MetaDataRepository.TemplateBinding(genericClassifier, parametersClasifiers);
        //                    if (genericClassifier is MetaDataRepository.Class)
        //                        classifier = new Class( templateBinding, ImplementationUnit);

        //                    if (genericClassifier is MetaDataRepository.Interface)
        //                        classifier = new Interface(codeElement as EnvDTE.CodeInterface);
        //                    if (genericClassifier is MetaDataRepository.Structure)
        //                        classifier = new Structure(codeElement as EnvDTE.CodeStruct);

        //                }

        //                //UserInterfaceTest.Mycollection`2


        //            }
        //        }
        //        else
        //            classifier = project.GetExternalClassifier(codeElement.FullName);


        //        // object dd = codeTypeRef.CodeType.ProjectItem;

        //    }
        //    else if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationProject)
        //    {

        //        classifier = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.Classifier;
        //        if (classifier == null)
        //        {
        //            Project project = MetaObjectMapper.FindMetaObjectFor(VSInterface.ProjectItem.ContainingProject) as Project;
        //            classifier = project.GetExternalClassifier(codeElement.FullName);
        //        }


        //    }
        //    if (classifier == null)
        //        throw new System.Exception("System can't finds classifier for type : " + codeElement.FullName);
        //    else
        //        return classifier;

        //}


        /// <MetaDataID>{2e795f60-9f89-4536-ab13-778145dd0e91}</MetaDataID>
        bool IsFeaturesLoaded = false;
        /// <MetaDataID>{08cb6f2c-fed5-4a9f-926e-abeff74d2ba7}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Feature> Features
        {
            get
            {

                try
                {
                    if (!IsFeaturesLoaded)
                        LoadFeatures();
                }
                catch (System.Exception error)
                {
                    IsFeaturesLoaded = true;
                }
                return base.Features;
            }
        }
        /// <MetaDataID>{6903b37f-a324-493d-8127-5ed4d1b4b658}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{caa82142-be4a-43c6-9f03-9d8a8cccb59b}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.AssociationEnd> Roles
        {
            get
            {
                if (!IsRolesLoaded)
                {
                    LoadRoles();
                    IsRolesLoaded = true;
                }
                return base.Roles;
            }
        }


        /// <MetaDataID>{041bbaf5-8077-4f51-8cab-29a9fef3a291}</MetaDataID>
        private void LoadRoles()
        {

            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("LoadRoles", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[0] { }, null);
                IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[0] { });
                return;
            }

            if (TemplateBinding != null)
            {
                IsRolesLoaded = true;
                return;
            }
            if (VSInterface == null) //Delegation class
                return;
            System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
            System.Collections.Generic.List<AssociationEnd> associatedRoles = new System.Collections.Generic.List<AssociationEnd>();
            foreach (MetaDataRepository.AssociationEnd associationEnd in _Roles)
            {
                if (associationEnd.Navigable && associationEnd is AssociationEnd)
                    associatedRoles.Add(associationEnd as AssociationEnd);
            }
            try
            {
                InLoadRoles = true;
                OOAdvantech.Collections.Generic.Set<MetaDataRepository.AssociationEnd> otherEndRoles = CodeClassifier.LoadRoles(this, members);
                IsRolesLoaded = true;
                CodeClassifier.BuildAssociations(this, otherEndRoles);

                //foreach (MetaDataRepository.AssociationEnd associationEnd in _Roles)
                //{
                //    if (associationEnd.Navigable && associationEnd is AssociationEnd)
                //    {
                //        if (associationEnd.Namespace is CodeElementContainer)
                //        {
                //            try
                //            {
                //                (associationEnd.Namespace as CodeElementContainer).RefreshCodeElement((associationEnd.Namespace as CodeElementContainer).CodeElement);
                //            }
                //            catch (System.Exception error)
                //            {
                //            }
                //        }


                //    }
                //}

                //foreach (MetaDataRepository.AssociationEnd associationEnd in _Roles)
                //{
                //    if (associationEnd is AssociationEnd &&
                //        associationEnd.Navigable &&
                //        !associatedRoles.Contains((associationEnd as AssociationEnd)))
                //    {
                //        try
                //        {
                //            if (associationEnd.Namespace != this && associationEnd.Namespace is CodeElementContainer)
                //                (associationEnd.Namespace as CodeElementContainer).RefreshCodeElement((associationEnd.Namespace as CodeElementContainer).CodeElement);
                //        }
                //        catch (System.Exception error)
                //        {

                //        }

                //    }

                //}
            }
            finally
            {
                InLoadRoles = false;
            }
            IsRolesLoaded = true;

        }

        /// <MetaDataID>{b7afcdc4-7358-440f-810b-b1394359a612}</MetaDataID>
        private void LoadFeatures()
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("LoadFeatures", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[0] { }, null);
                IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[0] { });
                return;
            }

            if (TemplateBinding != null)
            {
                //TODO θα πρέπει να γραφτεί κώδικας και για τα attributes και methods και roles
                CodeClassifier.LoadFeatureFromGenericType(this, _Features);
                IsFeaturesLoaded = true;
                return;
            }
            if (VSInterface == null)
                return;
            System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
            CodeClassifier.LoadFeatures(this, _Features, members);
            IsFeaturesLoaded = true;
        }


        /// <MetaDataID>{435eb99e-78b3-4645-ae5e-dea54d068a9c}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{27a8aa52-aa0c-4fa6-a963-58e2258d0297}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{551f8190-843d-42c2-8632-2cb61c31bf54}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement == VSInterface && codeElement != null)
                return true;
            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementInterface)
                return false;
            if (this.VSInterface == codeElement)
                return true;


            try
            {
                if (itsParent is EnvDTE.CodeNamespace)
                {
                    string fullName = FullName;
                    if (OwnedTemplateSignature != null)
                    {

                        string typeFullName = null;
                        System.Collections.Generic.List<string> parameters = null;
                        LanguageParser.GetGenericMetaData(FullName, ProjectItem.Project.VSProject.CodeModel.Language, ref typeFullName, ref parameters);
                        fullName = typeFullName;

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
                    try
                    {
                        if (codeElement.FullName == fullName)
                            return true;
                    }
                    catch (System.Exception error)
                    {
                    }

                    try
                    {
                        if (codeElement.Name == fullName)
                            return true;
                    }
                    catch (System.Exception error)
                    {
                    }
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

        /// <MetaDataID>{bc336845-2859-4d14-85f3-7ee4620fecae}</MetaDataID>
        public override void RemoveAssociation(OOAdvantech.MetaDataRepository.Association theAssociation)
        {
            base.RemoveAssociation(theAssociation);
            MetaObjectChangeState();
        }


        public override void RemoveOperation(OOAdvantech.MetaDataRepository.Operation operation)
        {
            base.RemoveOperation(operation);
            try
            {
                VSInterface.RemoveMember((operation as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }
        public override void RemoveAttribute(OOAdvantech.MetaDataRepository.Attribute theAttribute)
        {
            base.RemoveAttribute(theAttribute);
            try
            {
                VSInterface.RemoveMember((theAttribute as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }


        /// <MetaDataID>{537cca3e-4ec8-46e4-b854-1ef2afbd8f2d}</MetaDataID>
        protected override void AddRole(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd)
        {
            //if (IsRolesLoaded)
            //{
            //    base.AddRole(associationEnd);
            //    MetaObjectChangeState();
            //}
            //else
            base.AddRole(associationEnd);
        }

        /// <MetaDataID>{7CA7894C-F9D8-47DA-8AF1-4D8CB61587D6}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);

            _ProjectItem = null;

            if (_Namespace.Value != null)
            {
                _Namespace.Value.RemoveOwnedElement(this);
                _Namespace.Value = null;
            }
            ImplementationUnit.RemoveResident(this);
            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in _Roles)
            {
                if (associationEnd.GetOtherEnd() is CodeElementContainer)
                    (associationEnd.GetOtherEnd() as CodeElementContainer).CodeElementRemoved(projectItem);
                else if (associationEnd is CodeElementContainer)
                    (associationEnd as CodeElementContainer).CodeElementRemoved();
            }
            foreach (MetaDataRepository.Feature feature in _Features)
            {
                if (_Features is CodeElementContainer)
                    (_Features as CodeElementContainer).CodeElementRemoved(projectItem);
            }

            VSInterface = null;
            MetaObjectMapper.RemoveMetaObject(this);

        }
        /// <MetaDataID>{cffb7bd6-6248-499a-8d91-dfe7b767b267}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }


        /// <MetaDataID>{ffb94058-aa41-4343-9c2a-570486e9d41d}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Attribute AddAttribute(string attributeName, OOAdvantech.MetaDataRepository.Classifier attributeType, string initialValue)
        {
            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {

                Attribute attribute = new OOAdvantech.CodeMetaDataRepository.Attribute(attributeName, this);
                _Features.Add(attribute);
                stateTransition.Consistent = true;
                return attribute;
            }
        }


        /// <MetaDataID>{34b9e15e-9a13-4005-9fe6-d6cc6c55a8e1}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation AddOperation(string operationName, OOAdvantech.MetaDataRepository.Classifier opertionType)
        {
            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                Operation operation = new OOAdvantech.CodeMetaDataRepository.Operation(operationName, this);
                _Features.Add(operation);
                stateTransition.Consistent = true;
                return operation;
            }
        }

        /// <MetaDataID>{3DBBCCFF-1C2F-4413-B568-C98EA507BCBD}</MetaDataID>
        internal EnvDTE.CodeInterface VSInterface;
        /// <MetaDataID>{E69945E8-047E-4B05-B289-26CF2429F5D8}</MetaDataID>
        public Interface(EnvDTE.CodeInterface vsInterface)
        {
            VSInterface = vsInterface;
            _Kind = VSInterface.Kind;

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(vsInterface as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);



            if ((VSInterface is EnvDTE80.CodeInterface2) && (VSInterface as EnvDTE80.CodeInterface2).IsGeneric)
            {
                System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                string fullName = "";

                LanguageParser.GetGenericMetaData(VSInterface.FullName, VSInterface.Language, ref fullName, ref genericParameters);
                foreach (string parameter in genericParameters)
                {
                    if (OwnedTemplateSignature == null)
                        OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                    OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                }
                _Name = VSInterface.Name + "`" + genericParameters.Count.ToString();
            }
            else
                _Name = vsInterface.Name;
            Visibility = VSAccessTypeConverter.GetVisibilityKind(VSInterface.Access);

            _ImplementationUnit.Value = MetaObjectMapper.FindMetaObjectFor(vsInterface.ProjectItem.ContainingProject) as Project;
            if (_ImplementationUnit.Value != null)
                _ImplementationUnit.Value.AddResident(this);
            else
                _ImplementationUnit.Value = new Project(vsInterface.ProjectItem.ContainingProject);
            if (VSInterface.Namespace != null)
            {
                Namespace _namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSInterface.Namespace.FullName);
                if (_namespace == null)
                    _namespace = new Namespace(VSInterface.Namespace);
                SetNamespace(_namespace);
                _namespace.AddOwnedElement(this);

            }
            FinalInit();
            RefreshStartPoint();

            //string identity = Identity.ToString();

        }
        /// <MetaDataID>{af5b760f-0c33-4448-9115-7af4301dd8dd}</MetaDataID>
        public Interface(MetaDataRepository.TemplateBinding templateBinding, MetaDataRepository.Component implementationUnit)
        {

            //string language = "";
            //if (implementationUnit is Project)
            //    language = (implementationUnit as Project).VSProject.CodeModel.Language;
            _TemplateBinding = templateBinding;
            if (templateBinding != null)
                templateBinding.BoundElement = this;
            string identityString = (templateBinding.Signature.Template as MetaDataRepository.MetaObject).Name + "[";
            foreach (MetaDataRepository.TemplateParameterSubstitution parameterSubstitution in templateBinding.ParameterSubstitutions)
            {
                MetaDataRepository.IParameterableElement parameterableElement = parameterSubstitution.ActualParameters[0];
                if (parameterableElement is MetaDataRepository.Classifier)
                    identityString += "[" + (parameterableElement as MetaDataRepository.Classifier).FullName + "," + (parameterableElement as MetaDataRepository.Classifier).ImplementationUnit.Identity + "]";
                else
                    identityString += "[" + parameterableElement.Name + "]";
            }
            identityString += "]";
            _Name = identityString;

            _Namespace.Value = (templateBinding.Signature.Template as MetaDataRepository.MetaObject).Namespace;
            if (_Namespace.Value != null)
                _Namespace.Value.AddOwnedElement(this);

            _ImplementationUnit.Value = implementationUnit;
            _ImplementationUnit.Value.AddResident(this);
            FinalInit();
            if (Namespace != null)
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.FullName + "." + identityString);
            else
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityString);

        }
        // public EnvDTE.ProjectItem ProjectItem;
        /// <MetaDataID>{16865F4C-F053-474F-B7AD-A35FF3786D23}</MetaDataID>
        int _Line = 0;
        /// <MetaDataID>{8DCF2AC0-21E0-4142-97E8-27A6F2ED6897}</MetaDataID>
        int _LineCharOffset = 0;
        /// <MetaDataID>{36DCB10A-C65F-41C3-8709-F1D3B17F700F}</MetaDataID>
        public int Line
        {
            get
            {
                try
                {
                    if (VSInterface != null)
                        _Line = VSInterface.StartPoint.Line;
                }
                catch (System.Exception error)
                {
                }
                return _Line;
            }
            set
            {
                _Line = value;
            }

        }
        /// <MetaDataID>{770B32C3-DA9B-4A84-A382-09A9F7E6C572}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                try
                {
                    if (VSInterface != null)
                        _LineCharOffset = VSInterface.StartPoint.LineCharOffset;
                }
                catch (System.Exception error)
                {
                }
                return _LineCharOffset;
            }
        }

        /// <MetaDataID>{11d44be8-713b-4222-b203-399c081b7dce}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            return Line;
        }
        /// <MetaDataID>{3a13431e-3c78-493c-ba31-4aaf04cbf496}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return LineCharOffset;
        }
        /// <MetaDataID>{d06e9a78-b638-4389-8cde-07c5ca4cb384}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        private bool OnRefreshCodeElement;

        /// <MetaDataID>{3efbef4e-a99e-479d-ac72-410f231db27b}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }

        /// <MetaDataID>{B5FA769A-E538-4AC8-ADD2-ABCA0B6A9313}</MetaDataID>
        private void FinalInit()
        {
            try
            {
                if (VSInterface != null)
                {
                    MetaObjectMapper.AddTypeMap(VSInterface, this);
                    _LineCharOffset = VSInterface.StartPoint.LineCharOffset;
                    _Line = VSInterface.StartPoint.Line;
                    _ProjectItem = ProjectItem.AddMetaObject(VSInterface.ProjectItem, this);

                }
            }
            catch (System.Exception error)
            {
            }
        }
        /// <MetaDataID>{71685F2A-E154-43BD-A790-3EFA9D3ED5BC}</MetaDataID>

        public override OOAdvantech.MetaDataRepository.Association AddAssociation(string supplierRoleName, OOAdvantech.MetaDataRepository.Roles supplierRole, OOAdvantech.MetaDataRepository.Classifier supplierRoleClass, string associationIdentity)
        {

            OOAdvantech.MetaDataRepository.AssociationEnd codeAssociationEnd = null;
            OOAdvantech.MetaDataRepository.AssociationEnd otherEndCodeAssociationEnd = null;
            if (supplierRole == MetaDataRepository.Roles.RoleA)
            {
                otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(supplierRoleName, supplierRoleClass, OOAdvantech.MetaDataRepository.Roles.RoleA, associationIdentity);
                if (supplierRoleClass is CodeElementContainer)
                    codeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd("", this, OOAdvantech.MetaDataRepository.Roles.RoleB, associationIdentity);
                else
                    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd("", this, OOAdvantech.MetaDataRepository.Roles.RoleB);
            }
            else
            {
                otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(supplierRoleName, supplierRoleClass, OOAdvantech.MetaDataRepository.Roles.RoleB, null);
                if (supplierRoleClass is CodeElementContainer)
                    codeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd("", this, OOAdvantech.MetaDataRepository.Roles.RoleA, associationIdentity);
                else
                    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd("", this, OOAdvantech.MetaDataRepository.Roles.RoleA);

            }

            OOAdvantech.CodeMetaDataRepository.Association association = null;
            if (codeAssociationEnd.IsRoleA)
                association = new OOAdvantech.CodeMetaDataRepository.Association(supplierRoleName, codeAssociationEnd, otherEndCodeAssociationEnd, associationIdentity, null);
            else
                association = new OOAdvantech.CodeMetaDataRepository.Association(supplierRoleName, otherEndCodeAssociationEnd, codeAssociationEnd, associationIdentity, null);
            return association;

        }
        public override OOAdvantech.MetaDataRepository.Feature GetFeature(string identity, bool inherit)
        {
            foreach (OOAdvantech.MetaDataRepository.Feature feature in Features)
            {

                if (feature.Identity.ToString() == identity)
                    return feature;
                else if ((feature as CodeElementContainer).Identity.ToString() == identity)
                    return feature;
            }
            if (inherit)
                foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
                {
                    OOAdvantech.MetaDataRepository.Feature feature = classifier.GetFeature(identity, inherit);
                    if (feature != null)
                        return feature;
                }
            return null;
        }

        /// <MetaDataID>{C6851B5B-AD3C-4100-9652-A52CEE8D90A8}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            if (OnRefreshCodeElement)
                return;
            try
            {
                OnRefreshCodeElement = true;
                _Identity = null;



                #region old slow code


                //if (codeElement is EnvDTE.CodeInterface)
                //{
                //    foreach (EnvDTE.CodeElement memberCodeElement in (codeElement as EnvDTE.CodeInterface).Members)
                //    {
                //        foreach (MetaDataRepository.Feature feature in _Features)
                //        {
                //            if (feature is CodeElementContainer && (feature as CodeElementContainer).ContainCodeElement(memberCodeElement, null))
                //                (feature as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                //        }
                //    }
                //}











                //#region tmp out

                ////System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd> otherEnds = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>();
                ////foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in _Roles)
                ////{
                ////    if (associationEnd.GetOtherEnd() is CodeElementContainer)
                ////        otherEnds.Add(associationEnd.GetOtherEnd());
                ////}

                ////if (codeElement is EnvDTE.CodeInterface)
                ////{
                ////    System.Collections.Generic.List<EnvDTE.CodeElement> membersCodeElement = new System.Collections.Generic.List<EnvDTE.CodeElement>();
                ////    foreach (EnvDTE.CodeElement memberCodeElement in (codeElement as EnvDTE.CodeInterface).Members)
                ////        membersCodeElement.Add(memberCodeElement);

                ////    foreach (EnvDTE.CodeElement memberCodeElement in (codeElement as EnvDTE.CodeInterface).Members)
                ////    {
                ////        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in _Roles)
                ////        {
                ////            if (associationEnd.GetOtherEnd() is CodeElementContainer && (associationEnd.GetOtherEnd() as CodeElementContainer).ContainCodeElement(memberCodeElement, null))
                ////            {
                ////                (associationEnd.GetOtherEnd() as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                ////                membersCodeElement.Remove(memberCodeElement);
                ////            }
                ////            else
                ////            {
                ////                if (associationEnd.GetOtherEnd() is CodeElementContainer)
                ////                    otherEnds.Remove(associationEnd.GetOtherEnd());

                ////            }
                ////        }
                ////    }


                ////    foreach (EnvDTE.CodeElement memberCodeElement in (codeElement as EnvDTE.CodeInterface).Members)
                ////    {
                ////        foreach (MetaDataRepository.Feature feature in _Features)
                ////        {
                ////            if (feature is CodeElementContainer && (feature as CodeElementContainer).ContainCodeElement(memberCodeElement, null))
                ////            {
                ////                (feature as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                ////                membersCodeElement.Remove(memberCodeElement);
                ////            }
                ////        }
                ////    }
                ////}

                ////foreach (MetaDataRepository.AssociationEnd otherEnd in otherEnds)
                ////{
                ////    _Roles.Remove(otherEnd.GetOtherEnd());
                ////}

                ////Collections.Generic.Set<MetaDataRepository.AssociationEnd> newAssociationEnds = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>();
                ////foreach (EnvDTE.CodeElement memberCodeElement in MembersCodeElement)
                ////{
                ////    EnvDTE.CodeProperty codeProperty = memberCodeElement as EnvDTE.CodeProperty;
                ////    if (codeProperty != null)
                ////    {

                ////        MetaDataRepository.AssociationEnd associationEnd = CodeClassifier.GetAssociationEnd(codeProperty, this);
                ////        if (associationEnd != null)
                ////        {
                ////            //associationEnd.Navigable = true;
                ////            newAssociationEnds.Add(associationEnd);
                ////            if (associationEnd.Association != null && associationEnd is AssociationEnd)
                ////            {
                ////                (associationEnd as AssociationEnd).RefreshCodeElement(memberCodeElement);
                ////            }
                ////        }
                ////    }
                ////}
                ////CodeClassifier.BuildAssociations(this, newAssociationEnds);
                //#endregion













                ////try
                ////{
                ////    int line = VSInterface.StartPoint.Line;
                ////    RefreshStartPoint();
                ////    //if (VSInterface == codeElement)
                ////    //    return;
                ////}
                ////catch
                ////{

                ////}

                //IsFeaturesLoaded = false;
                ////foreach (OOAdvantech.MetaDataRepository.Feature feature in _Features)
                ////{
                ////    if (feature is CodeElementContainer)
                ////        MetaObjectMapper.RemoveType((feature as CodeElementContainer).CodeElement);
                ////}
                //_Features.RemoveAll();
                //IsRolesLoaded = false;
                //foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in new System.Collections.Generic.List<MetaDataRepository.AssociationEnd>(_Roles))
                //{
                //    if (associationEnd.GetOtherEnd() is CodeElementContainer)
                //    {
                //        if (associationEnd.GetOtherEnd().Navigable)
                //        {
                //            _Roles.Remove(associationEnd);
                //            (associationEnd.GetOtherEnd().Namespace as MetaDataRepository.Classifier).RemoveAssociation(associationEnd.Association);
                //            associationEnd.GetOtherEnd().Navigable = false;
                //            associationEnd.GetOtherEnd().Name = "";
                //            if (associationEnd is AssociationEnd)
                //                MetaObjectMapper.RemoveType((associationEnd as AssociationEnd).CodeElement);
                //            if (associationEnd.GetOtherEnd() is AssociationEnd)
                //                MetaObjectMapper.RemoveType((associationEnd.GetOtherEnd() as AssociationEnd).CodeElement);
                //        }
                //    }
                //}
                #endregion

                VSInterface = codeElement as EnvDTE.CodeInterface;

                _Identity = null;
                string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
                if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);


                string identity;
                string comments;
                CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
                if (comments != null)
                    PutPropertyValue("MetaData", "Documentation", comments);

                IsGeneralizationLoaded = false;
                _Generalizations.RemoveAll();

                System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(codeElement);
                CodeClassifier.RefreshClassifierMembers(this, _Features, _Roles, codeElement, members);

                try
                {
                    InLoadRoles = true;
                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.AssociationEnd> otherEndRoles = CodeClassifier.LoadRoles(this, members);
                    IsRolesLoaded = true;
                    CodeClassifier.BuildAssociations(this, otherEndRoles);
                }
                finally
                {
                    InLoadRoles = false;
                }
                CodeClassifier.LoadFeatures(this, _Features, members);



                MetaObjectMapper.RemoveMetaObject(this);

                MetaObjectMapper.AddTypeMap(VSInterface, this);
                Visibility = VSAccessTypeConverter.GetVisibilityKind(VSInterface.Access);
                OwnedTemplateSignature = null;

                if ((VSInterface is EnvDTE80.CodeInterface2) && (VSInterface as EnvDTE80.CodeInterface2).IsGeneric)
                {
                    System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                    string fullName = "";
                    LanguageParser.GetGenericMetaDataFromCSharpType(VSInterface.FullName, ref fullName, ref genericParameters);

                    foreach (string parameter in genericParameters)
                    {
                        if (OwnedTemplateSignature == null)
                            OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);

                        foreach (MetaDataRepository.TemplateParameter templatePparameter in OwnedTemplateSignature.OwnedParameters)
                            OwnedTemplateSignature.RemoveOwnedParameter(templatePparameter);
                        MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                        OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                    }
                    _Name = VSInterface.Name + "`" + genericParameters.Count.ToString();
                }
                else
                    _Name = VSInterface.Name;

                long count = Generalizations.Count;

                if (VSInterface.Namespace != null)
                {
                    Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSInterface.Namespace.FullName);
                    if (_Namespace.Value != mNamespace)
                    {
                        if (_Namespace.Value != null)
                            _Namespace.Value.RemoveOwnedElement(this);

                        _Namespace.Value = null;
                        if (mNamespace == null)
                            mNamespace = new Namespace(VSInterface.Namespace);
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
            finally
            {
                OnRefreshCodeElement = false;
            }

        }
        /// <MetaDataID>{CBFD600E-520D-4684-9335-7593012DB0E3}</MetaDataID>
        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSInterface.StartPoint.LineCharOffset;

                if (_Line != VSInterface.StartPoint.Line)
                {
                    _Line = VSInterface.StartPoint.Line;
                    // MetaObjectChangeState();
                }

            }
            catch (System.Exception error)
            {
            }

        }




        /// <MetaDataID>{4AAD814B-0A73-402C-A0D8-4C840438F41E}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSInterface as EnvDTE.CodeElement;
            }
        }




        /// <MetaDataID>{19423242-ce7a-43fa-8e87-f058f82ffffa}</MetaDataID>
        internal void AddMember(EnvDTE.CodeElement memberCodeElement)
        {
            EnvDTE.CodeAttribute associationAttribute = CodeClassifier.GetAssociationAttribute(memberCodeElement);
            try
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in Features)
                {
                    if (feature is CodeElementContainer && (feature as CodeElementContainer).ContainCodeElement(memberCodeElement as EnvDTE.CodeElement, null))
                    {
                        (feature as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                        return;
                    }
                }
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in GetAssociateRoles(false))
                {
                    if (associationEnd is CodeElementContainer && (associationEnd as CodeElementContainer).ContainCodeElement(memberCodeElement as EnvDTE.CodeElement, null))
                    {
                        (associationEnd as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                        return;
                    }
                }

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    System.Collections.Generic.List<EnvDTE.CodeElement> members = new System.Collections.Generic.List<EnvDTE.CodeElement>();
                    members.Add(memberCodeElement as EnvDTE.CodeElement);
                    CodeClassifier.LoadFeatures(this, _Features, members);
                    CodeClassifier.LoadRoles(this, members);
                    stateTransition.Consistent = true;
                }

            }
            finally
            {
                RefreshClassHierarchyCollections();

            }

        }


        public void UpdateRealizations(string oldSignature, string newSignature)
        {
            if (CodeClassifier.UpdateRealizations(oldSignature, newSignature, this, _Features))
                MetaObjectChangeState();

        }
    }
}
