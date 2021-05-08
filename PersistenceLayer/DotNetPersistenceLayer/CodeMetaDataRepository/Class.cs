namespace OOAdvantech.CodeMetaDataRepository
{
    using Transactions;
    using System.Reflection;
    using System.Linq;
    using System.Text;
    using System;
    using System.Collections.Generic;

    /// <MetaDataID>{A4E344F6-D668-4EB5-8874-6E643306F6C6}</MetaDataID>
    public class Class : OOAdvantech.MetaDataRepository.Class, CodeElementContainer
    {


        /// <MetaDataID>{2f18dc65-aefe-477c-b6a3-e2e8bf49cfe7}</MetaDataID>
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {


                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSClass as EnvDTE.CodeElement, identity);
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
            else
            {
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }

        /// <MetaDataID>{1ba6693a-7dd4-4a8d-b43a-955061759796}</MetaDataID>
        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                string identity = base.GetPropertyValue(propertyType, propertyNamespace, propertyName) as string;

                if (identity != null)
                    return identity;
                if (VSClass == null || TemplateBinding != null)
                    return base.Identity.ToString();
                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSClass as EnvDTE.CodeElement, out identity, out document);
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
                    CodeClassifier.SetIdentityToCodeElement(VSClass as EnvDTE.CodeElement, identity);

                }
                catch
                {
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }

        /// <MetaDataID>{56fbad08-4448-449a-ac4d-737dc5960ce2}</MetaDataID>
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

        public void UpdateObjectStateCode()
        {
            var @interface = (from _interface in GetAllInterfaces()
                              where _interface.FullName == typeof(OOAdvantech.IObjectState).FullName
                              select _interface).FirstOrDefault();

            if (@interface != null)
            {
                UpdateGetMemberValue(@interface);
                UpdateSetMemberValue(@interface);
            }

        }
        private void UpdateGetMemberValue(MetaDataRepository.Interface @interface)
        {
            var getOperation = (from operation in @interface.GetOperations(false)
                                where operation.Name == nameof(IObjectState.GetMemberValue)
                                select operation).FirstOrDefault();

            var getMethod = (from method in Features.OfType<Method>()
                             where method.Name == nameof(IObjectState.GetMemberValue)
                             select method).FirstOrDefault();


            if (getMethod == null)
            {
                var vsGetMethod = VSClass.AddFunction(nameof(IObjectState.GetMemberValue), EnvDTE.vsCMFunction.vsCMFunctionFunction, (getOperation as Operation).VSOperation.Type, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);

                List<MetaDataRepository.Method> parentMethods = new List<MetaDataRepository.Method>();
                if (Generalizations.Count == 1)
                    parentMethods = (from method in (Generalizations[0].Parent as MetaDataRepository.Class).GetFeatures(true).OfType<MetaDataRepository.Method>() where method.Specification == getOperation select method).ToList();


                if (parentMethods.Count == 0)
                    (vsGetMethod as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindVirtual;
                else
                    (vsGetMethod as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;

                foreach (EnvDTE.CodeParameter param in (getOperation as Operation).VSOperation.Parameters.Cast<EnvDTE.CodeParameter>().Reverse())
                    vsGetMethod.AddParameter(param.Name, param.Type);
                getMethod = new Method(getOperation, vsGetMethod, this);
            }

            EnvDTE.TextPoint startPoint = ((getMethod as Method).VSOperation as EnvDTE80.CodeFunction2).StartPoint;
            EnvDTE.TextPoint endPoint = ((getMethod as Method).VSOperation as EnvDTE80.CodeFunction2).EndPoint;

            var editPoint = startPoint.CreateEditPoint();
            string text = editPoint.GetText(endPoint);
            text = text.Substring(0, text.IndexOf("{"));
            text += "{\r\n";
            foreach (var member in CodeClassifier.GetMembers(VSClass as EnvDTE.CodeElement))
            {
                var kind = member.Kind;
                if (kind == EnvDTE.vsCMElement.vsCMElementVariable)
                    text += string.Format("\r\nif(member.Name == nameof({0})) \r\nreturn {0};\r\n", member.Name);
            }
            //foreach (var member in CodeClassifier.GetMembers(VSClass as EnvDTE.CodeElement))
            //{
            //    var kind = member.Kind;
            //    if (kind == EnvDTE.vsCMElement.vsCMElementProperty)
            //        text += string.Format("\r\nif(member.Name == nameof({0})) \r\nreturn {0};\r\n", member.Name);
            //}


            if (Generalizations.Count == 1 && (from method in (Generalizations[0].Parent as MetaDataRepository.Class).GetFeatures(true).OfType<MetaDataRepository.Method>() where method.Specification == getMethod.Specification select method).Count() > 0)
                text += "\r\n\r\nreturn base.GetMemberValue(token,member);\r\n}";
            else
                text += "\r\n return ObjectMemberGetSet.MemberValueGetFailed;\r\n}";

            editPoint.ReplaceText(endPoint, text, (int)EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
        }

        private void UpdateSetMemberValue(MetaDataRepository.Interface @interface)
        {
            var setOperation = (from operation in @interface.GetOperations(false)
                                where operation.Name == nameof(IObjectState.SetMemberValue)
                                select operation).FirstOrDefault();

            var setMethod = (from method in Features.OfType<Method>()
                             where method.Name == nameof(IObjectState.SetMemberValue)
                             select method).FirstOrDefault();


            if (setMethod == null)
            {
                var vsSetMethod = VSClass.AddFunction(nameof(IObjectState.SetMemberValue), EnvDTE.vsCMFunction.vsCMFunctionFunction, (setOperation as Operation).VSOperation.Type, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null);

                List<MetaDataRepository.Method> parentMethods = new List<MetaDataRepository.Method>();
                if (Generalizations.Count == 1)
                    parentMethods = (from method in (Generalizations[0].Parent as MetaDataRepository.Class).GetFeatures(true).OfType<MetaDataRepository.Method>() where method.Specification == setOperation select method).ToList();


                if (parentMethods.Count == 0)
                    (vsSetMethod as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindVirtual;
                else
                    (vsSetMethod as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;

                foreach (EnvDTE.CodeParameter param in (setOperation as Operation).VSOperation.Parameters.Cast<EnvDTE.CodeParameter>().Reverse())
                    vsSetMethod.AddParameter(param.Name, param.Type);
                setMethod = new Method(setOperation, vsSetMethod, this);
            }

            EnvDTE.TextPoint startPoint = ((setMethod as Method).VSOperation as EnvDTE80.CodeFunction2).StartPoint;
            EnvDTE.TextPoint endPoint = ((setMethod as Method).VSOperation as EnvDTE80.CodeFunction2).EndPoint;

            var editPoint = startPoint.CreateEditPoint();
            string text = editPoint.GetText(endPoint);
            text = text.Substring(0, text.IndexOf("{"));
            text += "{\r\n";
            foreach (var member in CodeClassifier.GetMembers(VSClass as EnvDTE.CodeElement))
            {
                var kind = member.Kind;
                if (kind == EnvDTE.vsCMElement.vsCMElementVariable)
                {
                    string memberType = (member as EnvDTE80.CodeVariable2).Type.AsString;
                    text += string.Format("if (member.Name == nameof({0}))\r\n{{\r\nif (value == null)\r\n{0} = default({1});\r\nelse\r\n{0} = ({1})value;\r\nreturn ObjectMemberGetSet.MemberValueSetted;\r\n}}", member.Name, memberType);
                }
            }
            //foreach (var member in CodeClassifier.GetMembers(VSClass as EnvDTE.CodeElement))
            //{
            //    var kind = member.Kind;
            //    if (kind == EnvDTE.vsCMElement.vsCMElementProperty)
            //    {
            //        string memberType = (member as EnvDTE80.CodeProperty2).Type.AsString;
            //        text += string.Format("if (member.Name == nameof({0}))\r\n{\r\nif (value == default({1})\r\n{0} = default({1});\r\nelse\r\n{0} = ({1})value;\r\nreturn ObjectMemberGetSet.MemberValueSetted;\r\n}", member.Name, memberType);
            //    }
            //}

            if (Generalizations.Count == 1 && (from method in (Generalizations[0].Parent as MetaDataRepository.Class).GetFeatures(true).OfType<MetaDataRepository.Method>() where method.Specification == setMethod.Specification select method).Count() > 0)
                text += "\r\n\r\nreturn base.SetMemberValue(token,member,value);\r\n}";
            else
                text += "\r\n return ObjectMemberGetSet.MemberValueSetFailed;\r\n}";

            editPoint.ReplaceText(endPoint, text, (int)EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
        }

        //TODO Να φτιαχτεί test case για την περίπτωση που είναι template class
        //ή instandieted class
        /// <MetaDataID>{9c4f2482-aeee-42ff-86e6-a42cb306a204}</MetaDataID>
        public Class()
        {
        }
        /// <MetaDataID>{41880ee4-7d1f-49ef-957b-f90b5689b688}</MetaDataID>
        public string CurrentProgramLanguageFullName
        {
            get
            {
                return LanguageParser.GetTypeFullName(this, _ProjectItem.Project.VSProject);
            }
        }


        /// <MetaDataID>{df4ea4a0-53ef-4922-8ff2-d8f508ca44ab}</MetaDataID>
        public string CurrentProgramLanguageName
        {
            get
            {
                return LanguageParser.GetTypeName(this, _ProjectItem.Project.VSProject);
            }
        }


        /// <MetaDataID>{d060e40e-a9fd-4d51-996b-2017f2b01e23}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{afe49f07-e24b-4759-b4fa-d8689f9b73d4}</MetaDataID>
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

        /// <MetaDataID>{5c50bf82-b1e0-41af-80a8-11e0385e5e5c}</MetaDataID>
        bool InGetRole = false;
        /// <MetaDataID>{cba2f4cc-50f6-4c5a-ae2d-76dfdb057f8b}</MetaDataID>
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
        /// <MetaDataID>{d67e3029-330f-427b-89dc-0c20f0dcfe7c}</MetaDataID>
        bool InLoadRoles = false;

        /// <MetaDataID>{d5436f3a-6fde-446c-8e6a-7aa12cecbc6a}</MetaDataID>
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
            if (_VSClass == null) //Delegation class
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
            RefreshProjectItems();
            IsRolesLoaded = true;
        }


        /// <MetaDataID>{3bd61524-a524-41c1-9fa3-59cfe49af322}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        /// <MetaDataID>{0a24f113-ff45-427b-84de-35c35a701b05}</MetaDataID>
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }

        /// <MetaDataID>{1e36e636-ac08-41a1-8674-509446397ea1}</MetaDataID>
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (codeElement.Kind != EnvDTE.vsCMElement.vsCMElementClass)
                return false;
            if (this._VSClass == codeElement)
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
                if (ProjectItemsLine[ProjectItem.GetProjectItem(codeElement.ProjectItem)] == codeElement.StartPoint.Line &&
                    ProjectItemsLineCharOffset[ProjectItem.GetProjectItem(codeElement.ProjectItem)] == codeElement.StartPoint.LineCharOffset)
                    return true;
            }
            catch (System.Exception error)
            {

            }
            return false;
        }


        /// <MetaDataID>{a2478e43-d5d5-4455-a705-fdeb886b99ac}</MetaDataID>
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
        /// <MetaDataID>{aaaf9cff-f5af-413a-b62e-787bbe8a20e6}</MetaDataID>
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
        public override void RemoveOperation(OOAdvantech.MetaDataRepository.Operation operation)
        {

            base.RemoveOperation(operation);
            try
            {
                VSClass.RemoveMember((operation as CodeElementContainer).CodeElement);
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
                VSClass.RemoveMember((theAttribute as CodeElementContainer).CodeElement);
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
                VSClass.RemoveMember((attributeRealization as CodeElementContainer).CodeElement);
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
                VSClass.RemoveMember((associationEndRealization as CodeElementContainer).CodeElement);
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
                VSClass.RemoveMember((method as CodeElementContainer).CodeElement);
            }
            catch (System.Exception error)
            {
            }
        }

        /// <MetaDataID>{bb7c8f89-b1b8-413d-a2ec-d2bea7ad1240}</MetaDataID>
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



        /// <MetaDataID>{24447430-bc2c-48b5-aeca-c2eb13733e76}</MetaDataID>
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


        /// <MetaDataID>{fac0d300-8c39-45ba-83c6-fc64478fb9d8}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        /// <MetaDataID>{8C0CA8DB-2E91-4308-BDFE-334BE9EC2B38}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            if (projectItem != null &&
                ProjectItem.GetProjectItem(projectItem) != null &&
                ProjectItems.Count > 1 &&
                ProjectItems.Contains(ProjectItem.GetProjectItem(projectItem)))
            {
                #region one part of partial class removed

                ProjectItem projectItemb = ProjectItem.GetProjectItem(projectItem);
                projectItemb.RemoveMetaObject(this);
                ProjectItemsLine.Remove(projectItemb);
                ProjectItemsLineCharOffset.Remove(projectItemb);
                ProjectItems.Remove(projectItemb);
                _ProjectItem = ProjectItems[0];


                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in _Roles)
                {
                    if (associationEnd.GetOtherEnd() is CodeElementContainer)
                        (associationEnd.GetOtherEnd() as CodeElementContainer).CodeElementRemoved();
                    else if (associationEnd is CodeElementContainer)
                        (associationEnd as CodeElementContainer).CodeElementRemoved();
                }
                foreach (MetaDataRepository.Feature feature in _Features)
                {
                    if (feature is CodeElementContainer)
                        (feature as CodeElementContainer).CodeElementRemoved(projectItem);
                }

                MetaObjectMapper.RemoveMetaObject(this);
                EnvDTE.CodeElement partialCodeElement = null;
                try
                {
                    partialCodeElement = (_VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1);
                }
                catch (System.Exception error)
                {
                }
                if (partialCodeElement == null)
                {
                    try
                    {
                        EnvDTE.TextPoint textPoint = ProjectItems[0].VSProjectItem.FileCodeModel.CodeElements.Item(1).StartPoint;
                        EnvDTE.EditPoint editPoint = textPoint.CreateEditPoint();
                        EnvDTE80.Events2 pp;

                        editPoint.MoveToLineAndOffset(GetLine(_ProjectItem), GetLineCharOffset(_ProjectItem));
                        EnvDTE.CodeElement classCodeElement = editPoint.get_CodeElement(Kind);
                        if (classCodeElement.FullName == CurrentProgramLanguageFullName)
                            partialCodeElement = classCodeElement;
                    }
                    catch (System.Exception error)
                    {
                    }

                }
                RefreshCodeElement(partialCodeElement);
                #endregion
                return;

            }

            foreach (ProjectItem partialProjectItem in ProjectItems)
                partialProjectItem.RemoveMetaObject(this);
            ProjectItems.Clear();
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
                if (feature is CodeElementContainer)
                    (feature as CodeElementContainer).CodeElementRemoved(projectItem);
            }
            _VSClass = null;
            MetaObjectMapper.RemoveMetaObject(this);
        }
        /// <MetaDataID>{396E16F1-C72A-4C8C-B15D-AB8AB5BE54B5}</MetaDataID>
        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSClass as EnvDTE.CodeElement;
            }
        }
        ///// <MetaDataID>{8a12b8d3-5d15-4aba-8ee6-9cd4d0bec029}</MetaDataID>
        //public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        //{
        //    get
        //    {

        //        if (_Identity != null)
        //            return _Identity;
        //        if (_VSClass == null || TemplateBinding != null)
        //            return base.Identity;
        //        System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //        try
        //        {
        //            try
        //            {
        //                document.LoadXml(_VSClass.DocComment);
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

        //        }
        //        catch (System.Exception error)
        //        {
        //        }
        //        if (_Identity != null)
        //            return _Identity;



        //        System.Guid guid = System.Guid.NewGuid();
        //        try
        //        {
        //            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("{" + guid.ToString() + "}");
        //            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
        //            _VSClass.DocComment = document.OuterXml;
        //        }
        //        catch
        //        {
        //        }
        //        return _Identity;

        //    }
        //}

        /// <exclude>Excluded</exclude>
        EnvDTE.CodeClass _VSClass;
        /// <MetaDataID>{e802f15e-dae1-48e6-a08c-52d5989a1d77}</MetaDataID>
        internal EnvDTE.CodeClass VSClass
        {
            get
            {
                try
                {
                    if (_VSClass == null)
                        return null;
                    int count = 0;
                    foreach (EnvDTE.CodeClass codeClass in (_VSClass as EnvDTE80.CodeClass2).PartialClasses)
                        count++;
                    if ((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                        return ((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1) as EnvDTE.CodeClass);
                }
                catch (System.Exception error)
                {
                }

                return _VSClass;
            }
        }
        /// <MetaDataID>{85495B3D-73F5-4246-B809-D96ADCD8B7E8}</MetaDataID>
        public Class(MetaDataRepository.TemplateBinding templateBinding, MetaDataRepository.Component implementationUnit)
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


        /// <MetaDataID>{c7a0051b-2803-49fb-8f5a-acc1c2b66d26}</MetaDataID>
        public Class(EnvDTE.CodeDelegate vsDelegate)
        {
            try
            {
                //TODO Να testαριστεί σε περίπτωση template
                if (vsDelegate != null)
                    _Kind = vsDelegate.Kind;
                Visibility = VSAccessTypeConverter.GetVisibilityKind(vsDelegate.Access);
                _Name = vsDelegate.Name;
                _ImplementationUnit.Value = MetaObjectMapper.FindMetaObjectFor(vsDelegate.ProjectItem.ContainingProject) as Project;

                if (vsDelegate.Namespace != null)
                {
                    Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(vsDelegate.Namespace.FullName);
                    if (mNamespace == null)
                        mNamespace = new Namespace(vsDelegate.Namespace);
                    mNamespace.AddOwnedElement(this);
                    SetNamespace(mNamespace);
                }
                //TODO έχει υλοποιηθεί πρόχειρα η functionality για την delegate class;

                //FinalInit();
            }
            catch (System.Exception error)
            {
                throw;
            }


        }
        /// <MetaDataID>{215fd524-0849-4d2a-9ec6-fac1113c03e2}</MetaDataID>
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

        /// <MetaDataID>{7db8becc-2a17-49ed-b10d-9900353f9245}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, originMetaObject);
                return;
            }
            if (!(originMetaObject is OOAdvantech.MetaDataRepository.Class))
                return;
            SetDocumentation(GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);
            SetIdentity(originMetaObject.Identity);

            //Class _class = null;

            //if (Namespace == null && originMetaObject.Namespace)
            //{
            //    VSClass.Namespace
            //    VSClass.ProjectItem.ContainingProject.CodeModel.AddNamespace("test", VSClass.ProjectItem, null);
            //}
            //{
            //    
            //}
            //if (!string.IsNullOrEmpty(namespaceName))
            //{
            //    EnvDTE.CodeNamespace _namespace = VSProject.CodeModel.AddNamespace(namespaceName, className + ".cs", 0);
            //    System.Threading.Thread.Sleep(200);
            //    EnvDTE.CodeClass codeClass = _namespace.AddClass(className, 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic);
            //    System.Threading.Thread.Sleep(200);
            //    _class = new Class(codeClass);
            //}
            //else
            //    _class = new Class(VSProject.CodeModel.AddClass(className, className + ".cs", 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic));


            if (originMetaObject.Namespace != null && VSClass.Namespace == null)
            {
                MetaDataRepository.MetaObjectID identity = _Identity;
                VSClass.EndPoint.CreateEditPoint().Insert("\r\n}");
                EnvDTE.EditPoint editPoint = VSClass.StartPoint.CreateEditPoint();
                int line = editPoint.Line + 2;
                editPoint.Insert("namespace " + originMetaObject.Namespace.FullName + "\r\n{\r\n");
                _VSClass = editPoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementInterface) as EnvDTE.CodeClass;
                _Identity = null;
                if (identity != null)
                    SetIdentity(identity);
                Namespace _Namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(VSClass.Namespace.FullName);
                if (_Namespace == null)
                    _Namespace = new Namespace(VSClass.Namespace);
                SetNamespace(_Namespace);
                _Namespace.AddOwnedElement(this);

                if (ImplementationUnit != null)
                    ImplementationUnit.MetaObjectChangeState();
            }


            MetaObjectsStack.ActiveProject = ImplementationUnit as Project;
            long count = Features.Count;
            count = Roles.Count;
            base.Synchronize(originMetaObject);
            if (_VSClass.Name != _Name)
                _VSClass.Name = _Name;

            #region Update class attributes
            bool persistentAttributeExist = false;
            bool backwardCompatibilityIDExist = false;
            string backwardCompatibilityID = GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;
            bool linkClass = (originMetaObject as MetaDataRepository.Classifier).LinkAssociation != null;
            bool linkClassAttributeExist = false;
            //string attributeName = null;
            System.Collections.Generic.List<EnvDTE.CodeAttribute> attributes = new System.Collections.Generic.List<EnvDTE.CodeAttribute>();

            EnumerateAttributes:
            foreach (EnvDTE.CodeAttribute attribute in _VSClass.Attributes)
            {
                try
                {
                    //attributeName = null;
                    //try
                    //{
                    //    attributeName = attribute.FullName;
                    //}
                    //catch (System.Exception error)
                    //{
                    //    attributeName = attribute.Name;
                    //}
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationClass), attribute) && !linkClass)// (attributeName == typeof(MetaDataRepository.AssociationClass).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, VSClass.ProjectItem)) && !linkClass)
                    {
                        attribute.Delete();
                        goto EnumerateAttributes;
                    }
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationClass), attribute) && linkClass)
                    {
                        linkClassAttributeExist = true;
                        CodeClassifier.UpdateAttributeValue(attribute, "typeof(" + LanguageParser.GetShortName((originMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleA.Specification.FullName, _VSClass as EnvDTE.CodeElement) + ")," + "typeof(" + LanguageParser.GetShortName((originMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleB.Specification.FullName, _VSClass as EnvDTE.CodeElement) + "),\"" + (originMetaObject as MetaDataRepository.Classifier).LinkAssociation.Name + "\"");
                    }
                    //if ((attributeName == typeof(MetaDataRepository.Persistent).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSClass.ProjectItem)) && !Persistent)
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
                            CodeClassifier.UpdateAttributeValue(attribute, "\"" + backwardCompatibilityID + "\"");
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
                VSClass.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSClass as EnvDTE.CodeElement), "\"" + backwardCompatibilityID + "\"", attributePos++);

            if (linkClass && !linkClassAttributeExist)
                VSClass.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, VSClass as EnvDTE.CodeElement), "typeof(" + LanguageParser.GetShortName((originMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleA.Specification.FullName, VSClass as EnvDTE.CodeElement) + ")," + "typeof(" + LanguageParser.GetShortName((originMetaObject as MetaDataRepository.Classifier).LinkAssociation.RoleB.Specification.FullName, _VSClass as EnvDTE.CodeElement) + "),\"" + (originMetaObject as MetaDataRepository.Classifier).LinkAssociation.Name + "\"", attributePos++);


            if (Persistent && !persistentAttributeExist)
                VSClass.AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSClass as EnvDTE.CodeElement), "", attributePos);

            _VSClass.IsAbstract = Abstract;


            #endregion

            #region Write generic parameters

            if (IsTemplate)
            {
                string fullName = _VSClass.FullName;
                if (_VSClass.Namespace != null)
                    fullName = fullName.Replace(_VSClass.Namespace.FullName + ".", "");
                EnvDTE.EditPoint edit = _VSClass.StartPoint.CreateEditPoint();

                string templateSignature = LanguageParser.GetTemplateSignature(OwnedTemplateSignature, _VSClass as EnvDTE.CodeElement);
                if (fullName != Name + templateSignature)
                {
                    int startLine = _VSClass.StartPoint.Line;
                    int endLine = _VSClass.EndPoint.Line;
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
                            _VSClass = edit.get_CodeElement(EnvDTE.vsCMElement.vsCMElementClass) as EnvDTE.CodeClass;
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
        /// <MetaDataID>{561f8400-20c1-41f3-834c-ef2c3bb0eb71}</MetaDataID>
        protected override void SetIdentity(OOAdvantech.MetaDataRepository.MetaObjectID theIdentity)
        {
            if (_Identity != null && _Identity == theIdentity)
                return;
            _Identity = null;
            base.SetIdentity(theIdentity);
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                if (!string.IsNullOrWhiteSpace(_VSClass.DocComment))
                    document.LoadXml(_VSClass.DocComment);
                else
                    document.LoadXml("<doc></doc>");
            }
            catch (System.Exception error)
            {
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
                    _VSClass.DocComment = document.OuterXml;
                    //_VSClass.Name = name + name;
                    //_VSClass.Name = _Name;

                    return;
                }
            }

            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
            _VSClass.DocComment = document.OuterXml;
            //_VSClass.Name = name + name;
            //_VSClass.Name = _Name;


        }

        /// <MetaDataID>{26b3ea82-90f6-4dc4-97a8-85ce24928b6b}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
                return;
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            try
            {
                document.LoadXml(_VSClass.DocComment);
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
                    _VSClass.DocComment = document.OuterXml;
                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("summary")).InnerText = documentation.ToString();
            _VSClass.DocComment = document.OuterXml;
        }


        /// <MetaDataID>{107C1C7A-B721-4474-866E-3AB8A98BD4D5}</MetaDataID>
        public Class(EnvDTE.CodeClass vsClass)
        {
            _VSClass = vsClass;
            if (vsClass != null)
                _Kind = vsClass.Kind;

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(vsClass as EnvDTE.CodeElement, out identity, out comments);
            if (identity != null)
                base.PutPropertyValue("MetaData", "MetaObjectID", identity);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);


            Visibility = VSAccessTypeConverter.GetVisibilityKind(_VSClass.Access);
            if ((_VSClass is EnvDTE80.CodeClass2) && (_VSClass as EnvDTE80.CodeClass2).IsGeneric)
            {
                System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                string fullName = "";
                LanguageParser.GetGenericMetaData(_VSClass.FullName, VSClass.Language, ref fullName, ref genericParameters);
                foreach (string parameter in genericParameters)
                {
                    if (OwnedTemplateSignature == null)
                        OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                    OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                }
                _Name = vsClass.Name + "`" + genericParameters.Count.ToString();
            }
            else
                _Name = vsClass.Name;
            _ImplementationUnit.Value = MetaObjectMapper.FindMetaObjectFor(_VSClass.ProjectItem.ContainingProject) as Project;

            if (_ImplementationUnit.Value == null)
                _ImplementationUnit.Value = new Project(_VSClass.ProjectItem.ContainingProject);

            if (_VSClass.Namespace != null)
            {
                _Namespace.Value = (Namespace)MetaObjectMapper.FindMetaObjectFor(_VSClass.Namespace.FullName);
                if (_Namespace.Value == null)
                    _Namespace.Value = new Namespace(_VSClass.Namespace);
                _Namespace.Value.AddOwnedElement(this);
            }
            FinalInit();
            RefreshStartPoint();
            //  string identity = Identity.ToString();


        }

        /// <exclude>Excluded</exclude>
        int _Line = 0;
        /// <exclude>Excluded</exclude>
        int _LineCharOffset = 0;
        /// <MetaDataID>{FBB27580-DA57-4725-9431-D36E1DB380B2}</MetaDataID>
        public int Line
        {
            get
            {
                try
                {
                    if (_ProjectItem == null)
                        return 0;
                    _Line = GetLine(_ProjectItem);
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
                    if (_ProjectItem == null)
                        return 0;
                    _LineCharOffset = GetLineCharOffset(_ProjectItem);
                }
                catch (System.Exception error)
                {
                }
                return _LineCharOffset;
            }
        }

        /// <MetaDataID>{eec08d49-47fe-4f32-b445-92fccbe960ef}</MetaDataID>
        public int GetLine(ProjectItem projectItem)
        {
            try
            {
                if (ProjectItemsLine.ContainsKey(projectItem))
                    return ProjectItemsLine[projectItem];
                else
                    return 0;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{e0e0a6c1-6e34-42bb-820a-1793e64d2870}</MetaDataID>
        public int GetLineCharOffset(ProjectItem projectItem)
        {
            try
            {
                if (ProjectItemsLine.ContainsKey(projectItem))
                    return ProjectItemsLineCharOffset[projectItem];
                else
                    return 0;
            }
            catch (System.Exception error)
            {

                throw;
            }
        }
        /// <MetaDataID>{4851ef49-41b1-411b-b75d-12af38a5038b}</MetaDataID>
        public void LineChanged(ProjectItem projectItem, int linesDown)
        {
            ProjectItemsLine[projectItem] += linesDown;

        }

        public void UpdateRealizations(string oldSignature, string newSignature)
        {
            if (CodeClassifier.UpdateRealizations(oldSignature, newSignature, this, _Features))
                MetaObjectChangeState();

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
                _VSClass = codeElement as EnvDTE.CodeClass;

                _Identity = null;
                string backwardCompatibilityID = CodeClassifier.GetBackwardCompatibilityID(CodeElement);
                if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID);

                IsGeneralizationLoaded = false;
                _Generalizations.RemoveAll();
                _Realizations.RemoveAll();
                IsRealizationLoaded = false;
                string identity;
                string comments;
                CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
                if (comments != null)
                    PutPropertyValue("MetaData", "Documentation", comments);

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

                MetaObjectMapper.AddTypeMap(_VSClass, this);
                Visibility = VSAccessTypeConverter.GetVisibilityKind(_VSClass.Access);
                OwnedTemplateSignature = null;
                if ((_VSClass is EnvDTE80.CodeClass2) && (_VSClass as EnvDTE80.CodeClass2).IsGeneric)
                {
                    System.Collections.Generic.List<string> genericParameters = new System.Collections.Generic.List<string>();
                    string fullName = "";
                    LanguageParser.GetGenericMetaDataFromCSharpType(_VSClass.FullName, ref fullName, ref genericParameters);

                    if (OwnedTemplateSignature == null)
                        OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);

                    foreach (MetaDataRepository.TemplateParameter templatePparameter in OwnedTemplateSignature.OwnedParameters)
                        OwnedTemplateSignature.RemoveOwnedParameter(templatePparameter);

                    foreach (string parameter in genericParameters)
                    {

                        MetaDataRepository.TemplateParameter templateParameter = new OOAdvantech.MetaDataRepository.TemplateParameter(parameter);
                        OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                    }
                    _Name = _VSClass.Name + "`" + genericParameters.Count.ToString();
                }
                else
                    _Name = _VSClass.Name;

                long count = Realizations.Count;
                count = Generalizations.Count;



                if (_VSClass.Namespace != null)
                {
                    Namespace mNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(_VSClass.Namespace.FullName);
                    if (_Namespace.Value != mNamespace)
                    {
                        if (_Namespace.Value != null)
                            _Namespace.Value.RemoveOwnedElement(this);
                        _Namespace.Value = null;
                        if (mNamespace == null)
                            mNamespace = new Namespace(_VSClass.Namespace);
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
                RefreshProjectItems();
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
                if ((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                {
                    foreach (EnvDTE.CodeElement codeElement in (_VSClass as EnvDTE80.CodeClass2).PartialClasses)
                    {
                        ProjectItem projectItem = ProjectItem.GetProjectItem(codeElement.ProjectItem);
                        ProjectItemsLine[projectItem] = codeElement.StartPoint.Line;
                        ProjectItemsLineCharOffset[projectItem] = codeElement.StartPoint.LineCharOffset;
                    }
                }
                else
                {
                    ProjectItem projectItem = ProjectItem.GetProjectItem(_VSClass.ProjectItem);
                    ProjectItemsLine[projectItem] = _VSClass.StartPoint.Line;
                    ProjectItemsLineCharOffset[projectItem] = _VSClass.StartPoint.LineCharOffset;
                }

            }
            catch (System.Exception error)
            {
            }


        }

        /// <MetaDataID>{d72ff458-7769-4fcb-9f14-7d21d86b5e68}</MetaDataID>
        bool IsRealizationLoaded = false;
        /// <MetaDataID>{65da75b1-5d2d-4725-ad48-fffa5c85ae45}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Realization> Realizations
        {
            get
            {
                try
                {
                    if (VSClass != null)
                    {
                        try
                        {
                            System.Collections.Generic.List<EnvDTE.CodeClass> partialClasses = new System.Collections.Generic.List<EnvDTE.CodeClass>();
                            if ((VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                            {
                                foreach (EnvDTE.CodeClass vsClass in (VSClass as EnvDTE80.CodeClass2).PartialClasses)
                                    partialClasses.Add(vsClass);
                            }
                            else
                                partialClasses.Add(VSClass);

                            int baseClassCount = 0;
                            foreach (EnvDTE.CodeClass vsClass in partialClasses)
                            {
                                foreach (EnvDTE.CodeElement codeElement in vsClass.ImplementedInterfaces)
                                {

                                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface)
                                    {
                                        baseClassCount++;
                                        bool exist = (from realization in _Realizations
                                                      where realization.Abstarction.FullName == codeElement.FullName
                                                      select realization).FirstOrDefault() != null;
                                        if (!exist)
                                        {
                                            IsRealizationLoaded = false;
                                            _Realizations.Clear();
                                            break;
                                        }
                                    }
                                }
                            }

                            if (baseClassCount != _Generalizations.Count)
                            {
                                IsRealizationLoaded = false;
                                _Realizations.Clear();
                            }

                        }
                        catch (System.Exception error)
                        {

                        }
                    }
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
                                    else
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
                            if (VSClass == null)
                                return base.Realizations;
                            else
                            {
                                System.Collections.Generic.List<EnvDTE.CodeClass> partialClasses = new System.Collections.Generic.List<EnvDTE.CodeClass>();
                                if ((VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                                {
                                    foreach (EnvDTE.CodeClass vsClass in (VSClass as EnvDTE80.CodeClass2).PartialClasses)
                                        partialClasses.Add(vsClass);
                                }
                                else
                                    partialClasses.Add(VSClass);

                                foreach (EnvDTE.CodeClass vsClass in partialClasses)
                                {
                                    foreach (EnvDTE.CodeElement codeElement in vsClass.ImplementedInterfaces)
                                    {
                                        if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface)
                                        {
                                            MetaDataRepository.Interface _interface = (ImplementationUnit as Project).GetClassifier(this, codeElement) as MetaDataRepository.Interface;
                                            if (_interface != null)
                                                _Realizations.Add(new Realization("", _interface, this));
                                        }
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
        /// <MetaDataID>{36356320-1e87-4974-ab06-c02e57460e89}</MetaDataID>
        bool IsGeneralizationLoaded = false;

        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier> GetAllGeneralClasifiers()
        {
            ClassifierHierarchyClassifiers = null;
            return base.GetAllGeneralClasifiers();
        }
        /// <MetaDataID>{d26882b4-b12e-4736-bbb2-ea9606da9219}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                try
                {
                    if (VSClass != null)
                    {
                        try
                        {
                            System.Collections.Generic.List<EnvDTE.CodeClass> partialClasses = new System.Collections.Generic.List<EnvDTE.CodeClass>();
                            if ((VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                            {
                                foreach (EnvDTE.CodeClass vsClass in (VSClass as EnvDTE80.CodeClass2).PartialClasses)
                                    partialClasses.Add(vsClass);
                            }
                            else
                                partialClasses.Add(VSClass);

                            int baseClassCount = 0;
                            foreach (EnvDTE.CodeClass vsClass in partialClasses)
                            {
                                foreach (EnvDTE.CodeElement codeElement in vsClass.Bases)
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

                                    //codeElement .FullName
                                    //MetaDataRepository.Classifier classifier = (ImplementationUnit as Project).GetClassifier(this, codeElement);
                                    //from
                                    //_Generalizations.Add(new Generalization("", classifier, this));
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
                            else if (VSClass == null) //Delegation class
                                return base.Generalizations;
                            else
                            {
                                try
                                {
                                    System.Collections.Generic.List<EnvDTE.CodeClass> partialClasses = new System.Collections.Generic.List<EnvDTE.CodeClass>();
                                    if ((VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 0)
                                    {
                                        foreach (EnvDTE.CodeClass vsClass in (VSClass as EnvDTE80.CodeClass2).PartialClasses)
                                            partialClasses.Add(vsClass);
                                    }
                                    else
                                        partialClasses.Add(VSClass);

                                    foreach (EnvDTE.CodeClass vsClass in partialClasses)
                                    {
                                        foreach (EnvDTE.CodeElement codeElement in vsClass.Bases)
                                        {
                                            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface || codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                                                continue;
                                            MetaDataRepository.Classifier classifier = (ImplementationUnit as Project).GetClassifier(this, codeElement);
                                            _Generalizations.Add(new Generalization("", classifier, this));
                                        }
                                    }
                                }
                                catch (System.Exception error)
                                {
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
        /// <MetaDataID>{d851c91f-9515-4dd4-95fe-f687504498ff}</MetaDataID>
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

                foreach (EnvDTE.CodeAttribute attribute in VSClass.Attributes)
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
                    if (attributeName == typeof(MetaDataRepository.Persistent).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSClass as EnvDTE.CodeElement))
                    {
                        if (!_Persistent)
                            attribute.Delete();
                        else
                            exist = true;

                        break;
                    }
                }
                if (!exist && _Persistent)
                    VSClass.AddAttribute(LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Persistent).FullName, VSClass as EnvDTE.CodeElement), "", 0);
            }
        }
        /// <exclude>Excluded</exclude>
        ProjectItem _ProjectItem;
        /// <MetaDataID>{920be4bc-225c-46de-9ab8-00aa37f5d9d6}</MetaDataID>
        public ProjectItem ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }
        /// <MetaDataID>{e3cb9f1b-3f96-4958-a489-a7dfdb98da07}</MetaDataID>
        System.Collections.Generic.List<ProjectItem> ProjectItems = new System.Collections.Generic.List<ProjectItem>();

        /// <MetaDataID>{bc123536-ac1d-40b4-8c30-9fe7ebb7f3a2}</MetaDataID>
        System.Collections.Generic.Dictionary<ProjectItem, int> ProjectItemsLine = new System.Collections.Generic.Dictionary<ProjectItem, int>();
        /// <MetaDataID>{c402446e-a7d3-4e39-adf4-b6d1efab6468}</MetaDataID>
        System.Collections.Generic.Dictionary<ProjectItem, int> ProjectItemsLineCharOffset = new System.Collections.Generic.Dictionary<ProjectItem, int>();


        /// <MetaDataID>{6C02FA6D-9481-4A1A-99E9-9BD871260686}</MetaDataID>
        private void FinalInit()
        {

            if (_VSClass != null)
            {
                try
                {
                    MetaObjectMapper.AddTypeMap(_VSClass, this);
                    RefreshProjectItems();
                }
                catch (System.Exception error)
                {
                }
                foreach (EnvDTE.CodeAttribute vsAttribute in _VSClass.Attributes)
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
                        if (attributeName == typeof(MetaDataRepository.Persistent).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.Persistent).FullName, VSClass as EnvDTE.CodeElement))
                            _Persistent = true;
                    }
                    catch (System.Exception error)
                    {
                    }
                }
                _Abstract = _VSClass.IsAbstract;
                bool isNested = _VSClass.Parent is EnvDTE.CodeClass;
            }
        }

        ///<summary></summary>
        /// <MetaDataID>{10d6f5f1-2486-4df2-9301-9b398e18b52e}</MetaDataID>
        private void RefreshProjectItems()
        {
            try
            {

                System.Collections.Generic.List<ProjectItem> projectItems = new System.Collections.Generic.List<ProjectItem>(ProjectItems);
                foreach (ProjectItem projectItem in ProjectItems)
                    projectItem.RemoveMetaObject(this);
                ProjectItems.Clear();

                if (projectItems.Count > 1 && (_VSClass as EnvDTE80.CodeClass2).PartialClasses.Count < projectItems.Count)
                {
                    ProjectItem codeElementProjectItem = ProjectItem.GetProjectItem(VSClass.ProjectItem);
                    foreach (ProjectItem projectItem in projectItems)
                        if (projectItem != codeElementProjectItem)
                        {
                            ProjectItem.Project.GetClassifiers(projectItem.VSProjectItem);
                        }
                }

                if ((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Count == 0)
                {

                    try
                    {
                        _ProjectItem = ProjectItem.AddMetaObject(_VSClass.ProjectItem, this);
                        ProjectItems.Add(_ProjectItem);
                        ProjectItemsLine[_ProjectItem] = _VSClass.StartPoint.Line;
                        ProjectItemsLineCharOffset[_ProjectItem] = _VSClass.StartPoint.LineCharOffset;
                    }
                    catch (Exception error)
                    {
                    }
                }
                else
                {
                    _ProjectItem = ProjectItem.AddMetaObject((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1).ProjectItem, this);
                    foreach (EnvDTE.CodeClass codeClass in (_VSClass as EnvDTE80.CodeClass2).PartialClasses)
                    {
                        try
                        {
                            ProjectItem projectItem = ProjectItem.AddMetaObject(codeClass.ProjectItem, this);
                            ProjectItems.Add(projectItem);
                            ProjectItemsLine[projectItem] = _VSClass.StartPoint.Line;
                            ProjectItemsLineCharOffset[projectItem] = _VSClass.StartPoint.LineCharOffset;

                        }
                        catch (Exception error)
                        {
                        }
                    }
                }
            }
            catch (Exception error)
            {
            }
        }


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







        /// <MetaDataID>{2bc802ec-4369-4038-9e4e-22699da52b2a}</MetaDataID>
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
            if (VSClass == null)
                return;
            System.Collections.Generic.List<EnvDTE.CodeElement> members = CodeClassifier.GetMembers(CodeElement);
            CodeClassifier.LoadFeatures(this, _Features, members);
            RefreshProjectItems();
            IsFeaturesLoaded = true;

        }

        /// <MetaDataID>{ccc577d3-089d-4d7e-a0b6-8a05421c9697}</MetaDataID>
        internal void MergePartialClasses()
        {

            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                if ((_VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 1)
                {
                    foreach (EnvDTE.CodeClass vsClass in (_VSClass as EnvDTE80.CodeClass2).PartialClasses)
                    {

                        try
                        {
                            document.LoadXml(vsClass.DocComment);
                        }
                        catch (System.Exception error)
                        {
                            document.LoadXml("<doc/>");
                        }
                        bool identitySetted = false;
                        bool noChange = false;
                        foreach (System.Xml.XmlNode node in document.DocumentElement)
                        {
                            System.Xml.XmlElement element = node as System.Xml.XmlElement;
                            if (element == null)
                                continue;

                            if (element.Name.ToLower() == "MetaDataID".ToLower())
                            {
                                if (element.InnerText == Identity.ToString())
                                {
                                    noChange = true;
                                    break;
                                }
                                element.InnerText = Identity.ToString();
                                identitySetted = true;
                                break;
                            }
                        }
                        if (noChange)
                            continue;
                        if (!identitySetted)
                            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = Identity.ToString();
                        vsClass.DocComment = document.OuterXml;
                    }
                }
            }
            catch (System.Exception error)
            {
            }


        }
    }

}
