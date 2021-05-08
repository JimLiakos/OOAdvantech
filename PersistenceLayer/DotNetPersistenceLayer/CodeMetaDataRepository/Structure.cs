namespace OOAdvantech.CodeMetaDataRepository
{
    using Transactions;
    using System.Reflection;

    /// <MetaDataID>{BE8CF8C2-5172-4586-A631-96A9363473C5}</MetaDataID>
    public class Structure : OOAdvantech.MetaDataRepository.Structure, CodeElementContainer
    {

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSStruct as EnvDTE.CodeElement, identity);
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
                if (VSStruct == null || TemplateBinding != null)
                    return base.Identity.ToString();
                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSStruct as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSStruct as EnvDTE.CodeElement, identity);
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
        /// <MetaDataID>{2e56a84d-ae45-4f6c-bbe8-8ce0b9d7000a}</MetaDataID>
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
        /// <MetaDataID>{eb304409-266a-4077-9e85-bf5160a96e55}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return LanguageParser.GetTypeName(this, _ProjectItem.Project.VSProject);
            }
        }
        /// <MetaDataID>{eea30438-665e-4a76-a6f7-b50d656b8df8}</MetaDataID>
        public string CurrentProgramLanguageFullName
        {
            get
            {
                return LanguageParser.GetTypeFullName(this, _ProjectItem.Project.VSProject);
            }
        }


        /// <MetaDataID>{add6682a-334d-4ab0-bc28-63bb6eed7ee1}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{8cb9b18a-39b7-4fc0-b07c-81af109b3614}</MetaDataID>
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



        /// <MetaDataID>{2133f499-2e3c-4126-9a63-873cc243c251}</MetaDataID>
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


        ///// <MetaDataID>{264bba4e-be8c-4cbb-8021-daacd71baf8f}</MetaDataID>
        //System.Collections.Generic.List<AssociationAttriuteData> AssociationAttriutesData = null;
        /// <MetaDataID>{c2716fba-a4da-4d7e-bd0f-c220029d9b2e}</MetaDataID>
        bool InGetRole = false;
        /// <MetaDataID>{5b17e577-70b3-416e-8e28-4c090c9cbda6}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.AssociationEnd GetRole(string associationEndIdentity)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("GetRole", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[1] { typeof(string) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[1] { associationEndIdentity }) as OOAdvantech.MetaDataRepository.AssociationEnd;
            }
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
                return CodeClassifier.GetClassifierRole(associationEndIdentity, this, _Roles, members);
            }
            finally
            {
                InGetRole = false;
            }
        }
        /// <MetaDataID>{034948ee-75c6-43ae-b7b7-e355dac48c3f}</MetaDataID>
        bool InLoadRoles = false;

        /// <MetaDataID>{275f7032-ce44-4267-a646-0fa25053f7c6}</MetaDataID>
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
            if (VSStruct == null) //Delegation class
                return;
            System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
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
            IsRolesLoaded = true;
        }



        /// <MetaDataID>{e37bee14-363c-4081-81cf-5f632077214c}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{bfcd8245-fd22-4171-9866-107a8b0d6bdb}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }

        /// <MetaDataID>{0d6048fb-e463-43a8-b65b-d49991332663}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementStruct)
                return false;
            if (VSStruct == codeElement)
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
        /// <MetaDataID>{4745db42-9c12-4419-b0a2-75be456f7626}</MetaDataID>
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


        /// <MetaDataID>{8C0CA8DB-2E91-4308-BDFE-334BE9EC2B38}</MetaDataID>
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
            VSStruct = null;
            MetaObjectMapper.RemoveMetaObject(this);
        }

        /// <MetaDataID>{aca5be59-d324-44c6-9fb9-74ab41268995}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        /// <MetaDataID>{396E16F1-C72A-4C8C-B15D-AB8AB5BE54B5}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSStruct as EnvDTE.CodeElement;
            }
        }
        /// <MetaDataID>{86298291-176d-4bb7-b8d2-c89f2ec2230f}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {

                if (_Identity != null)
                    return _Identity;
                if (VSStruct == null || TemplateBinding != null)
                    return base.Identity;

                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                try
                {
                    document.LoadXml(VSStruct.DocComment);
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
                if (_Identity != null)
                    return _Identity;

                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("{" + guid.ToString() + "}");
                    document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
                    VSStruct.DocComment = document.OuterXml;
                }
                catch
                {
                }
                return _Identity;
            }
        }
        /// <MetaDataID>{53038B58-1EB1-4AF1-A10F-1209CB270C58}</MetaDataID>
        internal EnvDTE.CodeStruct VSStruct;
        /// <MetaDataID>{85495B3D-73F5-4246-B809-D96ADCD8B7E8}</MetaDataID>
        public Structure(MetaDataRepository.TemplateBinding templateBinding, MetaDataRepository.Component implementationUnit)
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
            // FinalInit();
            if (Namespace != null)
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.FullName + "." + identityString);
            else
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityString);
        }






        /// <MetaDataID>{b5982749-a021-441f-8f03-79142bb180cd}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            if (!(OriginMetaObject is OOAdvantech.MetaDataRepository.Structure))
                return;
            try
            {
                if (OriginMetaObject.Namespace != null && VSStruct.Namespace == null)
                {
                    MetaDataRepository.MetaObjectID identity = _Identity;
                    VSStruct.EndPoint.CreateEditPoint().Insert("\r\n}");
                    EnvDTE.EditPoint editPoint = VSStruct.StartPoint.CreateEditPoint();
                    int line = editPoint.Line + 2;
                    editPoint.Insert("namespace " + OriginMetaObject.Namespace.FullName + "\r\n{\r\n");
                    VSStruct = editPoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementInterface) as EnvDTE.CodeStruct;
                    _Identity = null;
                    if (identity != null)
                        SetIdentity(identity);
                    Namespace _namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSStruct.Namespace.FullName);
                    if (_namespace == null)
                        _namespace = new Namespace(VSStruct.Namespace);
                    SetNamespace(_namespace);
                    _namespace.AddOwnedElement(this);

                    if (ImplementationUnit != null)
                        ImplementationUnit.MetaObjectChangeState();
                }


                MetaObjectsStack.ActiveProject = ImplementationUnit as Project;
                long count = Features.Count;
                count = Roles.Count;
                base.Synchronize(OriginMetaObject);
                if (VSStruct.Name != _Name)
                    VSStruct.Name = _Name;
                #region Update class attributes
                bool persistentAttributeExist = false;
                bool backwardCompatibilityIDExist = false;
                string backwardCompatibilityID = GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;

                EnumerateAttributes:
                foreach (EnvDTE.CodeAttribute attribute in VSStruct.Attributes)
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

                        if (LanguageParser.IsEqual(typeof(MetaDataRepository.Persistent), attribute) && !Persistent)
                        {
                            attribute.Delete();
                            goto EnumerateAttributes;
                        }
                        if (LanguageParser.IsEqual(typeof(MetaDataRepository.Persistent), attribute) && Persistent)
                            persistentAttributeExist = true;


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
                int attributePosition = 0;
                if (!backwardCompatibilityIDExist && !string.IsNullOrEmpty(backwardCompatibilityID))
                    VSStruct.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSStruct as EnvDTE.CodeElement), "\"" + backwardCompatibilityID + "\"", attributePosition++);
                if (Persistent && !persistentAttributeExist)
                    VSStruct.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSStruct as EnvDTE.CodeElement), "", attributePosition++);


                #endregion


                SetDocumentation(GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);
                SetIdentity(OriginMetaObject.Identity);
                MetaObjectChangeState();

            }
            catch (System.Exception error)
            {
                //throw;
            }

        }
        /// <MetaDataID>{435039cd-af5a-42bc-9921-ef110859b410}</MetaDataID>
        protected override void SetIdentity(OOAdvantech.MetaDataRepository.MetaObjectID theIdentity)
        {
            if (_Identity != null && _Identity == theIdentity)
                return;
            _Identity = null;
            base.SetIdentity(theIdentity);
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            try
            {
                document.LoadXml(VSStruct.DocComment);
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
                    VSStruct.DocComment = document.OuterXml;
                    //VSStruct.Name = name + name;
                    //VSStruct.Name = _Name;

                    return;
                }
            }

            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
            VSStruct.DocComment = document.OuterXml;

            //VSStruct.Name = name + name;
            //VSStruct.Name = _Name;


        }

        /// <MetaDataID>{f4bf91c4-9e1e-43f0-b8a7-d0c9d44755fb}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
                return;
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                document.LoadXml(VSStruct.DocComment);
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
                    VSStruct.DocComment = document.OuterXml;
                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("summary")).InnerText = documentation.ToString();
            VSStruct.DocComment = document.OuterXml;
        }

        /// <MetaDataID>{107C1C7A-B721-4474-866E-3AB8A98BD4D5}</MetaDataID>
        public Structure(EnvDTE.CodeStruct vsStruct)
        {

            VSStruct = vsStruct;
            if (vsStruct != null)
                _Kind = vsStruct.Kind;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(VSStruct as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);
            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);


            Visibility = VSAccessTypeConverter.GetVisibilityKind(VSStruct.Access);
            if ((VSStruct is EnvDTE80.CodeStruct2) && (VSStruct as EnvDTE80.CodeStruct2).IsGeneric)
            {
                System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                string fullName = "";
                LanguageParser.GetGenericMetaDataFromCSharpType(VSStruct.FullName, ref fullName, ref genericParameters);

                foreach (string parameter in genericParameters)
                {
                    if (OwnedTemplateSignature == null)
                        OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                    OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                }
                _Name = vsStruct.Name + "`" + genericParameters.Count.ToString();
            }
            else
                _Name = vsStruct.Name;
            _ImplementationUnit.Value = MetaObjectMapper.FindMetaObjectFor(VSStruct.ProjectItem.ContainingProject) as Project;

            if (VSStruct.Namespace != null)
            {
                Namespace _Namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSStruct.Namespace.FullName);
                if (_Namespace == null)
                    _Namespace = new Namespace(VSStruct.Namespace);
                _Namespace.AddOwnedElement(this);
            }
            FinalInit();
            RefreshStartPoint();

            //string identity = Identity.ToString();
        }

        /// <MetaDataID>{8DC2D85C-63E3-46F6-8DD4-82D5B777E93F}</MetaDataID>
        int _Line = 0;
        /// <MetaDataID>{506E81A7-B780-4EDC-B021-89270481B5C1}</MetaDataID>
        int _LineCharOffset = 0;
        /// <MetaDataID>{FBB27580-DA57-4725-9431-D36E1DB380B2}</MetaDataID>
        public int Line
        {
            get
            {
                try
                {
                    _Line = VSStruct.StartPoint.Line;
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
        /// <MetaDataID>{D7D3637F-1C97-4D15-85C1-34AB8B0A6640}</MetaDataID>
        public int LineCharOffset
        {
            get
            {
                try
                {
                    _LineCharOffset = VSStruct.StartPoint.LineCharOffset;
                }
                catch (System.Exception error)
                {
                }
                return _LineCharOffset;
            }
        }

        /// <MetaDataID>{08f6ec95-d64c-4990-9bf2-382ff2a5e852}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            return Line;
        }
        /// <MetaDataID>{fc9db0a2-3643-4559-b3c5-a9d940b00539}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            return LineCharOffset;
        }
        /// <MetaDataID>{20999c38-8c92-4539-b48a-c5ce11efa9e5}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            _Line += linesDown;
        }

        bool OnRefreshCodeElement;

        /// <MetaDataID>{A943BEB2-A9AF-413A-B3F2-E86A026ADC00}</MetaDataID>
        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            if (OnRefreshCodeElement)
                return;
            try
            {

                OnRefreshCodeElement = true;
                string identity;
                string comments;
                CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
                if (comments != null)
                    PutPropertyValue("MetaData", "Documentation", comments);

                IsGeneralizationLoaded = false;
                _Generalizations.RemoveAll();
                _Realizations.RemoveAll();
                IsRealizationLoaded = false;
                VSStruct = codeElement as EnvDTE.CodeStruct;

                _Identity = null;
                string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
                if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);


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

                MetaObjectMapper.AddTypeMap(VSStruct, this);



                Visibility = VSAccessTypeConverter.GetVisibilityKind(VSStruct.Access);
                OwnedTemplateSignature = null;
                if ((VSStruct is EnvDTE80.CodeStruct2) && (VSStruct as EnvDTE80.CodeStruct2).IsGeneric)
                {
                    System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                    string fullName = "";
                    LanguageParser.GetGenericMetaDataFromCSharpType(VSStruct.FullName, ref fullName, ref genericParameters);

                    foreach (string parameter in genericParameters)
                    {
                        if (OwnedTemplateSignature == null)
                            OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                        MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                        OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                    }
                    _Name = VSStruct.Name + "`" + genericParameters.Count.ToString();

                }
                else
                    _Name = VSStruct.Name;

                long count = Realizations.Count;
                count = Generalizations.Count;

                if (VSStruct.Namespace != null)
                {
                    Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSStruct.Namespace.FullName);
                    if (_Namespace.Value != mNamespace)
                    {
                        if (_Namespace.Value != null)
                            _Namespace.Value.RemoveOwnedElement(this);
                        _Namespace.Value = null;
                        if (mNamespace == null)
                            mNamespace = new Namespace(VSStruct.Namespace);
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
                RefreshClassHierarchyCollections();
                RefreshStartPoint();
                MetaObjectChangeState();

            }
            finally
            {
                OnRefreshCodeElement = false;
            }


        }
        /// <MetaDataID>{1D1409A2-AEB6-4CF5-9C57-B7483354F92F}</MetaDataID>
        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSStruct.StartPoint.LineCharOffset;


                if (_Line != VSStruct.StartPoint.Line)
                {
                    _Line = VSStruct.StartPoint.Line;
                    // MetaObjectChangeState();
                }
            }
            catch (System.Exception error)
            {
            }


            //try
            //{
            //    if ((VSStruct is EnvDTE80.CodeClass2) && (VSStruct as EnvDTE80.CodeClass2).IsGeneric)
            //    {
            //        System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
            //        string fullName = "";
            //        GenericsParser.GetGenericMetaDataFromCSharpType(VSStruct.FullName, ref fullName, ref genericParameters);
            //        if (OwnedTemplateSignature == null || genericParameters.Count != OwnedTemplateSignature.Parameters.Count)
            //        {
            //            OwnedTemplateSignature = null;
            //            foreach (string parameter in genericParameters)
            //            {
            //                if (OwnedTemplateSignature == null)
            //                    OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature();
            //                MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
            //                OwnedTemplateSignature.AddParameter(templateParameter);
            //            }

            //        }
            //        _Name = VSStruct.Name + "`" + genericParameters.Count.ToString();
            //    }
            //    else
            //        _Name = VSStruct.Name;

            //}
            //catch (System.Exception error)
            //{


            //}

        }

        /// <MetaDataID>{9a5c3901-b8ba-43a6-8f13-d81e1eb0246e}</MetaDataID>
        bool IsRealizationLoaded = false;
        /// <MetaDataID>{e0dda138-5921-4e6f-805a-d05c93771bf1}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Realization> Realizations
        {
            get
            {
                try
                {
                    if (!IsRealizationLoaded)
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            #region Load realization relationships
                            IsRealizationLoaded = true;
                            if (TemplateBinding != null)
                            {
                                foreach (MetaDataRepository.Realization realization in (TemplateBinding.Signature.Template as MetaDataRepository.InterfaceImplementor).Realizations)
                                {
                                    if (!realization.Abstarction.IsTemplate && !realization.Abstarction.IsTemplateInstantiation)
                                    {
                                        _Realizations.Add(new Realization("", realization.Abstarction, this));
                                    }
                                    else if (realization.Abstarction.IsTemplate && realization.Abstarction.IsBindedClassifier)
                                    {
                                        MetaDataRepository.Interface classifier = CodeClassifier.GetActualType(realization.Abstarction, TemplateBinding) as MetaDataRepository.Interface;

                                        //Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<MetaDataRepository.IParameterableElement>();

                                        //foreach (MetaDataRepository.TemplateParameterSubstitution orgParameterSubstitution in realization.Abstarction.TemplateBinding.ParameterSubstitutions)
                                        //    parameterSubstitutions.Add(TemplateBinding.GetActualParameterFor(realization.Abstarction.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) as MetaDataRepository.TemplateParameter));
                                        //MetaDataRepository.Interface classifier = new Interface(new MetaDataRepository.TemplateBinding(realization.Abstarction.TemplateBinding.Signature.Template, parameterSubstitutions), ImplementationUnit);
                                        _Realizations.Add(new Realization("", classifier, this));

                                    }
                                }
                                return base.Realizations;
                            }
                            if (VSStruct == null)
                                return base.Realizations;
                            else
                            {
                                foreach (EnvDTE.CodeElement codeElement in VSStruct.ImplementedInterfaces)
                                {
                                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface)
                                    {

                                        MetaDataRepository.Interface _interface = (ImplementationUnit as Project).GetClassifier(this, codeElement) as MetaDataRepository.Interface;
                                        if (_interface != null)
                                            _Realizations.Add(new Realization("", _interface, this));
                                    }
                                }
                            }
                            #endregion 
                            stateTransition.Consistent = true;
                        }

                    }
                }
                catch (System.Exception error)
                {
                    _Realizations.RemoveAll();
                    IsRealizationLoaded = false;
                    throw;
                }

                return base.Realizations;
            }
        }
        /// <MetaDataID>{749355aa-bac8-4bdc-a342-592bafc6851d}</MetaDataID>
        bool IsGeneralizationLoaded = false;


        /// <MetaDataID>{f15bec6f-ef34-4dd6-96ac-1948f4b25e14}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                try
                {
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
                                    else
                                    {
                                        MetaDataRepository.Classifier classifier = CodeClassifier.GetActualType(generalization.Parent, TemplateBinding);
                                        //Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<MetaDataRepository.IParameterableElement>();
                                        //foreach (MetaDataRepository.TemplateParameterSubstitution orgParameterSubstitution in generalization.Parent.TemplateBinding.ParameterSubstitutions)
                                        //    parameterSubstitutions.Add(TemplateBinding.GetActualParameterFor(generalization.Parent.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) as MetaDataRepository.TemplateParameter));
                                        //MetaDataRepository.Class classifier = new Class(new MetaDataRepository.TemplateBinding(generalization.Parent.TemplateBinding.Signature.Template, parameterSubstitutions), ImplementationUnit);
                                        _Generalizations.Add(new Generalization("", classifier, this));
                                    }

                                }
                                return base.Generalizations;
                            }
                            else if (VSStruct == null) //Delegation class
                                return base.Generalizations;
                            else
                            {
                                foreach (EnvDTE.CodeElement codeElement in VSStruct.Bases)
                                {
                                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface || codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                                        continue;
                                    MetaDataRepository.Classifier classifier = (ImplementationUnit as Project).GetClassifier(this, codeElement);
                                    _Generalizations.Add(new Generalization("", classifier, this));
                                }
                            }
                            #endregion 
                            stateTransition.Consistent = true;
                        }

                    }
                }
                catch (System.Exception error)
                {
                    _Generalizations.RemoveAll();
                    IsGeneralizationLoaded = false;
                    throw;
                }

                return base.Generalizations;
            }
        }
        /// <MetaDataID>{3ea83215-dc88-4364-b18b-de64b407b474}</MetaDataID>
        public override bool Persistent
        {
            get
            {
                return base.Persistent;
            }
            set
            {
                base.Persistent = value;
                bool exist = false;

                foreach (EnvDTE.CodeAttribute attribute in VSStruct.Attributes)
                {
                    string attributeName = null;
                    try
                    {
                        attributeName = attribute.FullName;
                    }
                    catch (System.Exception error)
                    {
                        attributeName = attribute.Name;
                    }
                    if (attributeName == typeof(MetaDataRepository.Persistent).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSStruct as EnvDTE.CodeElement))
                    {
                        if (!_Persistent)
                            attribute.Delete();
                        else
                            exist = true;

                        break;
                    }
                }
                if (!exist && _Persistent)
                    VSStruct.AddAttribute(LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Persistent).FullName, VSStruct as EnvDTE.CodeElement), "", 0);
            }
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        /// <MetaDataID>{d82007c1-db4c-48a1-b079-d9ba3eceff48}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }

        /// <MetaDataID>{6C02FA6D-9481-4A1A-99E9-9BD871260686}</MetaDataID>
        private void FinalInit()
        {
            try
            {


                MetaObjectMapper.AddTypeMap(VSStruct, this);
                if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
                {
                    _LineCharOffset = VSStruct.StartPoint.LineCharOffset;
                    _Line = VSStruct.StartPoint.Line;
                    _ProjectItem = ProjectItem.AddMetaObject(VSStruct.ProjectItem, this);
                }
            }
            catch (System.Exception error)
            {
            }
            try
            {
                foreach (EnvDTE.CodeAttribute vsAttribute in VSStruct.Attributes)
                {
                    try
                    {
                        string attributeName = null;
                        try
                        {
                            attributeName = vsAttribute.FullName;
                        }
                        catch (System.Exception error)
                        {
                            attributeName = vsAttribute.Name;
                        }
                        if (attributeName == typeof(MetaDataRepository.Persistent).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSStruct as EnvDTE.CodeElement))
                            _Persistent = true;
                    }
                    catch (System.Exception error)
                    {
                    }
                }
                bool isNested = VSStruct.Parent is EnvDTE.CodeClass;
            }
            catch (System.Exception error)
            {


            }
        }

        /// <MetaDataID>{003C87B9-E6CB-4318-B87D-30605EB54B20}</MetaDataID>

        /// <MetaDataID>{89D3479C-AF40-4179-A675-5FC7F0DDBE36}</MetaDataID>
        bool IsFeaturesLoaded = false;
        /// <MetaDataID>{9D8635E1-5C91-45B0-965C-7E99F6DEF0F3}</MetaDataID>
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

        //private OOAdvantech.MetaDataRepository.Classifier GetClassifier(EnvDTE.CodeElement codeElement)
        //{
        //    MetaDataRepository.Classifier classifier = null;
        //    if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationExternal)
        //    {
        //        Project project = null;
        //        try
        //        {
        //            project = MetaObjectMapper.FindMetaObjectFor(VSStruct.ProjectItem.ContainingProject) as Project;
        //        }
        //        catch (System.Exception error)
        //        {
        //            project = IDEManager.GetActiveWindowProject() as Project;

        //        }


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
        //                        classifier = new Class(templateBinding, ImplementationUnit);

        //                    if (genericClassifier is MetaDataRepository.Interface)
        //                    {
        //                        // if (LanguageParser.IsGeneric(codeTypeRef))
        //                        //{
        //                        //    string typeFullName = null;
        //                        //    System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();

        //                        //    LanguageParser.GetGenericMetaDataFromCSharpType(codeTypeRef.AsString, ref typeFullName, ref parameters);
        //                        //}

        //                        classifier = new Interface(templateBinding, this.ImplementationUnit);

        //                    }
        //                    if (genericClassifier is MetaDataRepository.Structure)
        //                        classifier = new Structure(codeElement as EnvDTE.CodeStruct);

        //                }

        //                //UserInterfaceTest.Mycollection`2


        //            }
        //        }
        //        else
        //        {
        //            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
        //                classifier = project.GetExternalClassifier(codeElement.Name);
        //            else
        //                classifier = project.GetExternalClassifier(codeElement.FullName);

        //        }


        //        // object dd = codeTypeRef.CodeType.ProjectItem;

        //    }
        //    else if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationProject)
        //    {

        //        classifier = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.Classifier;
        //        if (classifier == null)
        //        {
        //            Project project = MetaObjectMapper.FindMetaObjectFor(VSStruct.ProjectItem.ContainingProject) as Project;
        //            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
        //                classifier = project.GetExternalClassifier(codeElement.Name);
        //            else
        //                classifier = project.GetExternalClassifier(codeElement.FullName);
        //        }


        //    }
        //    if (classifier == null)
        //        return null;
        //    else
        //        return classifier;

        //}



        /// <MetaDataID>{f96b1e2d-bb14-4ba1-a034-a0988c446adb}</MetaDataID>

        /// <MetaDataID>{80ABDA9A-1483-488A-A880-D1AADF3207E2}</MetaDataID>
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
                CodeClassifier.LoadFeatureFromGenericType(this, _Features);
                IsFeaturesLoaded = true;
                return;
            }
            if (VSStruct == null)
                return;
            System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
            CodeClassifier.LoadFeatures(this, _Features, members);
            IsFeaturesLoaded = true;


        }


        /// <MetaDataID>{c7604f7d-77a5-4f7f-9ae0-3f1f1c422ed6}</MetaDataID>
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



        public override void RemoveOperation(OOAdvantech.MetaDataRepository.Operation operation)
        {
            base.RemoveOperation(operation);
            try
            {
                VSStruct.RemoveMember((operation as CodeElementContainer).CodeElement);
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
                VSStruct.RemoveMember((theAttribute as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }
        public override void RemoveAttributeRealization(MetaDataRepository.AttributeRealization attributeRealization)
        {
            base.RemoveAttributeRealization(attributeRealization);
            try
            {
                VSStruct.RemoveMember((attributeRealization as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }
        public override void RemoveAssociationEndRealization(OOAdvantech.MetaDataRepository.AssociationEndRealization associationEndRealization)
        {
            base.RemoveAssociationEndRealization(associationEndRealization);
            try
            {
                VSStruct.RemoveMember((associationEndRealization as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }

        public override void RemoveMethod(MetaDataRepository.Method method)
        {
            base.RemoveMethod(method);
            try
            {
                VSStruct.RemoveMember((method as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }
    }
}
