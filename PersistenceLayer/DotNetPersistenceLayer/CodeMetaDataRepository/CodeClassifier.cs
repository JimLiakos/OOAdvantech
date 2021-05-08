using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{8a2c749a-f0c1-464b-aa5f-64bcb7712c9e}</MetaDataID>
    internal static class CodeClassifier
    {


        /// <MetaDataID>{c34e3479-622d-43ef-9df4-f728918368bd}</MetaDataID>
        public static void SetIdentityToCodeElement(EnvDTE.CodeElement codeElement, string identity)
        {

            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            string xmlString = null;
            xmlString = GetDocDocument(codeElement);
            if (xmlString != null)
                xmlString = xmlString.Trim();
            if (string.IsNullOrEmpty(xmlString))
                document.LoadXml("<doc></doc>");
            else
                document.LoadXml(xmlString);


            foreach (System.Xml.XmlNode node in document.DocumentElement)
            {
                System.Xml.XmlElement element = node as System.Xml.XmlElement;
                if (element == null)
                    continue;
                if (element.Name.ToLower() == "MetaDataID".ToLower())
                {
                    if (element.InnerText != identity)
                        SetDocDocument(codeElement, document.OuterXml);
                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = identity;
            SetDocDocument(codeElement, document.OuterXml);
        }
        /// <MetaDataID>{736031d4-ddf3-4c2b-b2b1-19f2ff85a484}</MetaDataID>
        private static void SetDocDocument(EnvDTE.CodeElement codeElement, string xmlString)
        {
            if (codeElement is EnvDTE.CodeInterface)
                (codeElement as EnvDTE.CodeInterface).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeClass)
                (codeElement as EnvDTE.CodeClass).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeStruct)
                (codeElement as EnvDTE.CodeStruct).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeEnum)
                (codeElement as EnvDTE.CodeEnum).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeDelegate)
                (codeElement as EnvDTE.CodeDelegate).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeFunction)
                (codeElement as EnvDTE.CodeFunction).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeProperty)
                (codeElement as EnvDTE.CodeProperty).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeVariable)
                (codeElement as EnvDTE.CodeVariable).DocComment = xmlString;
            if (codeElement is EnvDTE.CodeParameter)
                (codeElement as EnvDTE.CodeParameter).DocComment = xmlString;
        }
        /// <MetaDataID>{2a4eb1a9-c4da-40a8-bf3e-e4a5e98565c6}</MetaDataID>
        private static string GetDocDocument(EnvDTE.CodeElement codeElement)
        {
            string xmlString = "";
            if (codeElement is EnvDTE.CodeInterface)
                xmlString = (codeElement as EnvDTE.CodeInterface).DocComment;
            if (codeElement is EnvDTE.CodeClass)
                xmlString = (codeElement as EnvDTE.CodeClass).DocComment;
            if (codeElement is EnvDTE.CodeStruct)
                xmlString = (codeElement as EnvDTE.CodeStruct).DocComment;
            if (codeElement is EnvDTE.CodeEnum)
                xmlString = (codeElement as EnvDTE.CodeEnum).DocComment;
            if (codeElement is EnvDTE.CodeDelegate)
                xmlString = (codeElement as EnvDTE.CodeDelegate).DocComment;
            if (codeElement is EnvDTE.CodeFunction)
                xmlString = (codeElement as EnvDTE.CodeFunction).DocComment;
            if (codeElement is EnvDTE.CodeProperty)
                xmlString = (codeElement as EnvDTE.CodeProperty).DocComment;
            if (codeElement is EnvDTE.CodeVariable)
                xmlString = (codeElement as EnvDTE.CodeVariable).DocComment;
            if (codeElement is EnvDTE.CodeParameter)
                xmlString = (codeElement as EnvDTE.CodeParameter).DocComment;
            return xmlString;
        }
        /// <MetaDataID>{68a2aece-daba-4394-9cc6-61b5087eb48a}</MetaDataID>
        public static string GetBackwardCompatibilityID(EnvDTE.CodeElement codeElement)
        {
            EnvDTE.CodeElements attributes = null;
            if (codeElement is EnvDTE.CodeInterface)
                attributes = (codeElement as EnvDTE.CodeInterface).Attributes;
            if (codeElement is EnvDTE.CodeClass)
                attributes = (codeElement as EnvDTE.CodeClass).Attributes;
            if (codeElement is EnvDTE.CodeStruct)
                attributes = (codeElement as EnvDTE.CodeStruct).Attributes;
            if (codeElement is EnvDTE.CodeEnum)
                attributes = (codeElement as EnvDTE.CodeEnum).Attributes;
            if (codeElement is EnvDTE.CodeDelegate)
                attributes = (codeElement as EnvDTE.CodeDelegate).Attributes;
            if (codeElement is EnvDTE.CodeFunction)
                attributes = (codeElement as EnvDTE.CodeFunction).Attributes;
            if (codeElement is EnvDTE.CodeProperty)
                attributes = (codeElement as EnvDTE.CodeProperty).Attributes;
            if (codeElement is EnvDTE.CodeVariable)
                attributes = (codeElement as EnvDTE.CodeVariable).Attributes;
            if (attributes == null)
                return null;
            foreach (EnvDTE.CodeAttribute attribute in attributes)
            {
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.BackwardCompatibilityID), attribute))
                {
                    string backwardCompatibilityID = attribute.Value.Substring(1, attribute.Value.Length - 2);
                    if (string.IsNullOrEmpty(backwardCompatibilityID))
                        return null;
                    else
                        return backwardCompatibilityID;
                }
            }
            return null;

        }


        /// <MetaDataID>{7f44dd80-4c0d-4352-a21b-227f7d9bf9eb}</MetaDataID>
        public static void LoadDocDocumentItems(EnvDTE.CodeElement codeElement, out string identity, out string comments)
        {
            identity = null;
            comments = null;
            try
            {
                if (codeElement == null)
                    return;
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                string xmlString = GetDocDocument(codeElement);
                if (xmlString != null)
                    xmlString = xmlString.Trim();
                if (string.IsNullOrEmpty(xmlString))
                    return;
                document.LoadXml(xmlString);
                foreach (System.Xml.XmlNode node in document.DocumentElement)
                {
                    System.Xml.XmlElement element = node as System.Xml.XmlElement;
                    if (element == null)
                        continue;

                    if (element.Name.ToLower() == "summary".ToLower())
                        comments = element.InnerText;

                    if (element.Name.ToLower() == "MetaDataID".ToLower())
                        identity = element.InnerText;
                }
            }
            catch (System.Exception error)
            {
            }
        }

        /// <MetaDataID>{4ac50455-98da-442e-ac9e-25986e68431f}</MetaDataID>
        public static string GetSignature(MetaDataRepository.MetaObject member, Project project)
        {

            if (member is MetaDataRepository.Operation)
                return Operation.GetSignature(member as MetaDataRepository.Operation, project);

            if (member is MetaDataRepository.Method)
                return Operation.GetSignature((member as MetaDataRepository.Method).Specification, project);

            if (member is MetaDataRepository.Attribute)
            {
                string signature = null;
                if ((member as MetaDataRepository.Attribute).ParameterizedType != null)
                    signature += (member as MetaDataRepository.Attribute).ParameterizedType;
                else
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.Attribute).Type, project.VSProject);
                signature += " " + member.Name;
                return signature;
            }
            if (member is MetaDataRepository.AttributeRealization)
            {
                string signature = null;
                if ((member as MetaDataRepository.AttributeRealization).ParameterizedType != null)
                    signature += (member as MetaDataRepository.AttributeRealization).ParameterizedType;
                else
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.AttributeRealization).Type, project.VSProject);
                signature += " " + member.Name;
                return signature;
            }

            if (member is MetaDataRepository.AssociationEndRealization)
            {
                string signature = null;
                if ((member as MetaDataRepository.AssociationEndRealization).Specification.CollectionClassifier != null)
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.AssociationEndRealization).Specification.CollectionClassifier, project.VSProject);
                else
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.AssociationEndRealization).Specification.Specification, project.VSProject);
                signature += " " + member.Name;
                return signature;
            }
            if (member is MetaDataRepository.AssociationEnd)
            {
                string signature = null;
                if ((member as MetaDataRepository.AssociationEnd).CollectionClassifier != null)
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.AssociationEnd).CollectionClassifier, project.VSProject);
                else
                    signature += LanguageParser.GetTypeFullName((member as MetaDataRepository.AssociationEnd).Specification, project.VSProject);
                signature += " " + member.Name;
                return signature;
            }

            return member.FullName;


        }

        /// <MetaDataID>{09e724f4-a9da-40f9-8115-4a7e433f5bd9}</MetaDataID>
        static bool InAssociationRemove = false;
        /// <MetaDataID>{68907cb7-5931-4abd-9b75-f2a7a45d8c4e}</MetaDataID>
        public static void RefreshClassifierMembers(MetaDataRepository.Classifier classifier, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> features, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd> roles, EnvDTE.CodeElement codeElement, System.Collections.Generic.List<EnvDTE.CodeElement> members)
        {
            System.Collections.Generic.List<MetaDataRepository.AssociationEnd> unRefreshedRoles = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>(roles);
            System.Collections.Generic.List<MetaDataRepository.Feature> unRefreshedFeatures = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.Feature>(features);
            foreach (EnvDTE.CodeElement memberCodeElement in new System.Collections.Generic.List<EnvDTE.CodeElement>(members))
            {
                foreach (MetaDataRepository.Feature feature in features)
                {
                    if (unRefreshedFeatures.Contains(feature))
                    {
                        if (feature is CodeElementContainer && (feature as CodeElementContainer).ContainCodeElement(memberCodeElement, null))
                        {
                            if (feature.Owner == classifier)
                            {
                                members.Remove(memberCodeElement);
                                if (!IsExcluded(memberCodeElement))
                                {
                                    (feature as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                                    unRefreshedFeatures.Remove(feature);
                                }
                                break;
                            }
                        }
                    }
                }
            }

            foreach (EnvDTE.CodeElement memberCodeElement in new System.Collections.Generic.List<EnvDTE.CodeElement>(members))
            {
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in new System.Collections.Generic.List<MetaDataRepository.AssociationEnd>(roles))
                {
                    if (unRefreshedRoles.Contains(associationEnd))
                    {
                        if (associationEnd.GetOtherEnd() is CodeElementContainer)
                        {
                            if (associationEnd.GetOtherEnd().Navigable)
                            {
                                if ((associationEnd.GetOtherEnd() as CodeElementContainer).ContainCodeElement(memberCodeElement, null))
                                {
                                    (associationEnd.GetOtherEnd() as CodeElementContainer).RefreshCodeElement(memberCodeElement);
                                    members.Remove(memberCodeElement);
                                    unRefreshedRoles.Remove(associationEnd);
                                    break;
                                }
                            }
                            else
                                unRefreshedRoles.Remove(associationEnd);

                        }
                    }
                }

            }
            foreach (MetaDataRepository.Feature feature in unRefreshedFeatures)
            {
                if (feature is CodeElementContainer)
                    (feature as CodeElementContainer).CodeElementRemoved();
                features.Remove(feature);
            }
            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in unRefreshedRoles)
            {
                if (associationEnd.GetOtherEnd() is CodeElementContainer)
                {
                    (associationEnd.GetOtherEnd() as CodeElementContainer).CodeElementRemoved();
                    if (associationEnd.GetOtherEnd().Specification is CodeElementContainer)
                    {
                        if (!InAssociationRemove)
                        {
                            try
                            {
                                InAssociationRemove = true;
                                int line = (associationEnd.GetOtherEnd().Specification as CodeElementContainer).CodeElement.StartPoint.Line;
                                if (associationEnd.GetOtherEnd().Specification != classifier)
                                    (associationEnd.GetOtherEnd().Specification as CodeElementContainer).RefreshCodeElement((associationEnd.GetOtherEnd().Specification as CodeElementContainer).CodeElement);
                            }
                            catch (System.Exception error)
                            {
                            }
                            finally
                            {
                                InAssociationRemove = false;
                            }
                        }
                        else
                        {


                        }

                    }
                }
            }
        }


        /// <MetaDataID>{49685f3c-c44a-45dd-b0c0-c42de55407f4}</MetaDataID>
        public static Collections.Generic.Set<MetaDataRepository.AssociationEnd> LoadRoles(MetaDataRepository.Classifier classifier, System.Collections.Generic.List<EnvDTE.CodeElement> members)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                stateTransition.Consistent = true;

                Collections.Generic.Set<MetaDataRepository.AssociationEnd> otherEndRoles = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>();

                if (classifier.TemplateBinding != null)
                    return otherEndRoles;
                foreach (EnvDTE.CodeElement codeElement in members)
                {

                    EnvDTE.CodeProperty codeProperty = codeElement as EnvDTE.CodeProperty;
                    if (codeProperty != null)
                    {

                        MetaDataRepository.AssociationEnd associationEnd = CodeClassifier.GetAssociationEnd(codeProperty, classifier);
                        if (associationEnd != null)
                            otherEndRoles.Add(associationEnd);
                    }
                    EnvDTE.CodeVariable codeVariable = codeElement as EnvDTE.CodeVariable;
                    if (codeVariable != null)
                    {
                        MetaDataRepository.AssociationEnd associationEnd = CodeClassifier.GetAssociationEnd(codeVariable, classifier);
                        if (associationEnd != null)
                            otherEndRoles.Add(associationEnd);
                    }

                }
                return otherEndRoles;
                //CodeClassifier.CheckAssociations(this, otherEndRoles);

            }


        }


        /// <MetaDataID>{80ABDA9A-1483-488A-A880-D1AADF3207E2}</MetaDataID>
        static public void LoadFeatures(MetaDataRepository.Classifier classifier, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> features, System.Collections.Generic.List<EnvDTE.CodeElement> members)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                #region Load behavioral feature, operations and methods
                try
                {
                    foreach (EnvDTE.CodeElement codelement in new System.Collections.Generic.List<EnvDTE.CodeElement>(members))
                    {


                        if (IsExcluded(codelement))
                            members.Remove(codelement);


                    }
                }
                catch (System.Exception error)
                {
                    //members.Clear();
                    //foreach (EnvDTE.CodeElement codelement in VSClass.Members)
                    //    members.Add(codelement);
                }




                foreach (EnvDTE.CodeElement codelement in members)
                {

                    if (codelement.Kind == EnvDTE.vsCMElement.vsCMElementFunction)
                    {
                        MetaDataRepository.Feature feature = MetaObjectMapper.FindMetaObjectFor(codelement) as MetaDataRepository.Feature;
                        if (feature != null && !features.Contains(feature))
                        {

                            features.Add(feature);
                            if (feature is CodeElementContainer)
                                (feature as CodeElementContainer).RefreshCodeElement(codelement);
                        }
                        if (feature != null)
                            continue;

                        MetaDataRepository.Operation operation = GetOperationForMethod(classifier, codelement as EnvDTE80.CodeFunction2);
                        if (operation == null)
                        {
                            if ((codelement as EnvDTE80.CodeFunction2).OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride)
                            {
                                operation = new Operation(codelement as EnvDTE80.CodeFunction2, classifier);
                                features.Add(operation);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            Method method = new Method(operation, codelement as EnvDTE.CodeFunction, classifier);
                            //MetaObjectMapper.AddTypeMap((method as CodeElementContainer).CodeElement, method);
                            features.Add(method);
                        }

                        int fdd = 0;
                    }
                }
                #endregion

                #region Load attributes from proerties
                foreach (EnvDTE.CodeElement codeElement in members)
                {
                    MetaDataRepository.Feature feature = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.Feature;
                    if (feature != null && !features.Contains(feature))
                    {
                        features.Add(feature);
                        if (feature is CodeElementContainer)
                            (feature as CodeElementContainer).RefreshCodeElement(codeElement);

                    }
                    if (feature != null)
                        continue;

                    EnvDTE.CodeProperty codeProperty = codeElement as EnvDTE.CodeProperty;
                    if (codeProperty != null)
                    {
                        bool HasAssocitionAttribute = false;
                        foreach (EnvDTE.CodeAttribute codeAttribute in codeProperty.Attributes)
                        {
                            if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationAttribute), codeAttribute))
                            {
                                HasAssocitionAttribute = true;
                                break;
                            }
                        }

                        if (!HasAssocitionAttribute)
                        {

                            MetaDataRepository.AssociationEnd specificationRole = null;

                            #region Search for role realizations in parent interfaces

                            if (!(codeProperty.Getter != null && (codeProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew) &&
                                !(codeProperty.Setter != null && (codeProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew))
                            {

                                //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
                                //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
                                //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository
                                MetaDataRepository.AssociationEnd specificationAssociationEnd = GetAssociaionEndForAssociatioEndRealization(classifier, codeProperty);

                                if (specificationAssociationEnd != null)
                                {
                                    MetaDataRepository.AssociationEndRealization mAssociationRealization = MetaObjectMapper.FindMetaObjectFor(codeProperty) as MetaDataRepository.AssociationEndRealization;
                                    if (mAssociationRealization == null)
                                        mAssociationRealization = new AssociationEndRealization(codeProperty, specificationAssociationEnd, classifier);
                                    features.Add(mAssociationRealization);
                                    continue;
                                }
                            }
                            #endregion

                            MetaDataRepository.Attribute specificationAttribute = null;

                            #region Search for attribute realization in parent interfaces

                            if (!(codeProperty.Getter != null && (codeProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew) &&
                                !(codeProperty.Setter != null && (codeProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind == EnvDTE80.vsCMOverrideKind.vsCMOverrideKindNew))
                            {
                                specificationAttribute = GetAttributeForAttributeRealization(classifier, codeProperty);
                            }
                            #endregion

                            if (specificationAttribute != null)
                            {
                                MetaDataRepository.AttributeRealization mAttributeRealization = MetaObjectMapper.FindMetaObjectFor(codeProperty) as MetaDataRepository.AttributeRealization;
                                if (mAttributeRealization == null)
                                    mAttributeRealization = new AttributeRealization(codeProperty, specificationAttribute, classifier);
                                features.Add(mAttributeRealization);
                                object obj = mAttributeRealization.Type;
                                obj = mAttributeRealization.ParameterizedType;
                            }
                            else
                            {
                                MetaDataRepository.Attribute mAttribute = MetaObjectMapper.FindMetaObjectFor(codeProperty) as MetaDataRepository.Attribute;
                                if (mAttribute == null)
                                {
                                    if (MetaObjectMapper.FindMetaObjectFor(codeProperty) != null)
                                        MetaObjectMapper.RemoveType(codeProperty);
                                    mAttribute = new Attribute(codeProperty, classifier);
                                }
                                //string name = mAttribute.Type.Name;
                                features.Add(mAttribute);

                                object obj = mAttribute.Type;
                                obj = mAttribute.ParameterizedType;
                            }
                        }
                    }
                }


                #endregion

                #region Load attributes from fields
                foreach (EnvDTE.CodeElement codeElement in members)
                {
                    MetaDataRepository.Feature feature = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.Feature;
                    if (feature != null && !features.Contains(feature))
                    {
                        features.Add(feature);
                        if (feature is CodeElementContainer)
                            (feature as CodeElementContainer).RefreshCodeElement(codeElement);

                    }
                    if (feature != null)
                        continue;
                    EnvDTE.CodeVariable codeVariable = codeElement as EnvDTE.CodeVariable;
                    if (codeVariable != null)
                    {
                        bool HasAssocitionAttribute = false;
                        foreach (EnvDTE.CodeAttribute codeAttribute in codeVariable.Attributes)
                        {

                            if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationAttribute), codeAttribute))
                            {
                                HasAssocitionAttribute = true;
                                break;
                            }
                        }

                        if (!HasAssocitionAttribute)
                        {

                            MetaDataRepository.Attribute mAttribute = MetaObjectMapper.FindMetaObjectFor(codeVariable) as MetaDataRepository.Attribute;

                            if (mAttribute == null)
                            {
                                if (MetaObjectMapper.FindMetaObjectFor(codeVariable) != null)
                                    MetaObjectMapper.RemoveType(codeVariable);


                                mAttribute = new Attribute(codeVariable, classifier);
                            }
                            features.Add(mAttribute);
                            object obj = mAttribute.Type;
                            obj = mAttribute.ParameterizedType;

                        }
                    }
                }


                #endregion

                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{555cf504-fdcf-495c-aa9b-fcdc822bdb60}</MetaDataID>
        private static bool IsExcluded(EnvDTE.CodeElement codelement)
        {
            bool exclude = false;
            try
            {
                System.Xml.XmlDocument docDocument = new System.Xml.XmlDocument();


                try
                {
                    if (codelement is EnvDTE.CodeVariable)
                        docDocument.LoadXml((codelement as EnvDTE.CodeVariable).DocComment);
                    if (codelement is EnvDTE.CodeProperty)
                        docDocument.LoadXml((codelement as EnvDTE.CodeProperty).DocComment);
                    if (codelement is EnvDTE.CodeFunction)
                        docDocument.LoadXml((codelement as EnvDTE.CodeFunction).DocComment);
                }
                catch (System.Exception error)
                {
                    docDocument.LoadXml("<doc></doc>");
                }

                if (docDocument.DocumentElement != null)
                {
                    foreach (System.Xml.XmlNode node in docDocument.DocumentElement.ChildNodes)
                    {
                        if ("exclude".ToLower() == node.Name.ToLower())
                        {
                            exclude = true;
                            break;

                        }
                    }
                }
            }
            catch (System.Exception error)
            {
            }
            return exclude;
        }

        /// <MetaDataID>{c7f076c4-670a-40ef-a842-109468ce2dd6}</MetaDataID>
        static public MetaDataRepository.Attribute GetAttributeForAttributeRealization(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE.CodeProperty codeProperty)
        {
            //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
            //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
            //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository
            try
            {
                string typeFullName = codeProperty.Type.AsFullName;
            }
            catch (System.Exception error)
            {
                //Bad or removed code element
                return null;
            }
            if (classifier is MetaDataRepository.InterfaceImplementor)
            {
                foreach (MetaDataRepository.Interface _interface in (classifier as MetaDataRepository.InterfaceImplementor).GetAllInterfaces())
                {
                    foreach (MetaDataRepository.Attribute attribute in _interface.GetAttributes(false))
                    {
                        if (attribute.Name == codeProperty.Name && attribute.Type != null && codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(attribute.Type, (classifier.ImplementationUnit as Project).VSProject))
                            return attribute;
                        if (attribute.Name == codeProperty.Name && attribute.ParameterizedType != null && codeProperty.Type.AsFullName == attribute.ParameterizedType.FullName)//.Owner.TemplateBinding.GetIntermediateParameterFor(attribute.ParameterizedType).Name
                            return attribute;
                    }
                }
                if ((codeProperty.Setter is EnvDTE80.CodeFunction2 &&
                (codeProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride) ||
                (codeProperty.Getter is EnvDTE80.CodeFunction2 &&
                (codeProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride))
                    return null;

                foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                {
                    foreach (MetaDataRepository.Attribute attribute in generalClassifier.GetAttributes(false))
                    {
                        if (attribute.Name == codeProperty.Name && codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(attribute.Type, codeProperty.ProjectItem.ContainingProject))
                            return attribute;
                    }
                }
            }


            return null;
        }

        /// <MetaDataID>{1002e51f-724a-4ba7-b9ce-2d83d8a1cfc3}</MetaDataID>
        static public MetaDataRepository.AssociationEnd GetAssociaionEndForAssociatioEndRealization(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE.CodeProperty codeProperty)
        {
            //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
            //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
            //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository
            try
            {
                string typeFullName = codeProperty.Type.AsFullName;
            }
            catch (System.Exception error)
            {
                //Bad or removed code element
                return null;
            }
            if (classifier is MetaDataRepository.InterfaceImplementor)
            {
                foreach (MetaDataRepository.Interface _interface in (classifier as MetaDataRepository.InterfaceImplementor).GetAllInterfaces())
                {
                    foreach (MetaDataRepository.AssociationEnd attribute in _interface.GetAssociateRoles(false))
                    {
                        if (attribute.Name == codeProperty.Name &&
                            (codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(attribute.Specification, (classifier.ImplementationUnit as Project).VSProject) ||
                            (attribute.CollectionClassifier != null && codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(attribute.CollectionClassifier, (classifier.ImplementationUnit as Project).VSProject))))
                            return attribute;
                    }
                }
                if ((codeProperty.Setter is EnvDTE80.CodeFunction2 &&
                    (codeProperty.Setter as EnvDTE80.CodeFunction2).OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride) ||
                    (codeProperty.Getter is EnvDTE80.CodeFunction2 &&
                    (codeProperty.Getter as EnvDTE80.CodeFunction2).OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride))
                    return null;


                foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                {
                    foreach (MetaDataRepository.AssociationEnd associationEnd in generalClassifier.GetAssociateRoles(false))
                    {
                        if (associationEnd.Name == codeProperty.Name &&
                         (codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(associationEnd.Specification, (classifier.ImplementationUnit as Project).VSProject) ||
                         (associationEnd.CollectionClassifier != null && codeProperty.Type.AsFullName == LanguageParser.GetTypeFullName(associationEnd.CollectionClassifier, (classifier.ImplementationUnit as Project).VSProject))))
                            return associationEnd;
                    }
                }
            }

            return null;
        }

        /// <MetaDataID>{a4bbca86-b674-4e54-b2e6-b3fd4edf8c4a}</MetaDataID>
        static internal OOAdvantech.MetaDataRepository.Operation GetOperationForMethod(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE80.CodeFunction2 method)
        {

            string methodSignature = null;
            try
            {
                methodSignature = Operation.GetSignature(method);
            }
            catch (System.Exception error)
            {
                //Bad or removed code element
                return null;
            }
            if (classifier is MetaDataRepository.InterfaceImplementor)
            {
                foreach (MetaDataRepository.Interface _Interface in (classifier as MetaDataRepository.InterfaceImplementor).GetAllInterfaces())
                {
                    foreach (MetaDataRepository.Feature feature in _Interface.Features)
                    {
                        string operationSignature = null;
                        MetaDataRepository.Operation operation = feature as MetaDataRepository.Operation;
                        if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
                        {
                            operationSignature = Operation.GetSignature(operation, classifier.ImplementationUnit as Project);
                            if (operationSignature == methodSignature)
                                return operation;
                        }
                    }
                }

                if (method.OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride)
                    return null;




                foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                {
                    foreach (MetaDataRepository.Feature feature in generalClassifier.Features)
                    {
                        string operationSignature = null;
                        MetaDataRepository.Operation operation = feature as MetaDataRepository.Operation;
                        if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
                        {
                            operationSignature = Operation.GetSignature(operation, classifier.ImplementationUnit as Project);
                            if (operationSignature == methodSignature)
                                return operation;
                        }
                    }
                }
            }
            return null;

        }
        /// <MetaDataID>{566aad84-fa57-4b95-a630-2929746b5ded}</MetaDataID>
        static public OOAdvantech.MetaDataRepository.Classifier GetActualType(MetaDataRepository.Classifier genericType, MetaDataRepository.TemplateBinding templateBinding)
        {

            if ((!genericType.IsTemplate && !genericType.IsTemplateInstantiation) || templateBinding == null)
            {
                return genericType;
            }
            else
            {
                Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<MetaDataRepository.IParameterableElement>();
                foreach (MetaDataRepository.TemplateParameterSubstitution orgParameterSubstitution in genericType.TemplateBinding.ParameterSubstitutions)
                {
                    if (genericType.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) is MetaDataRepository.TemplateParameter)
                        parameterSubstitutions.Add(templateBinding.GetActualParameterFor(genericType.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal) as MetaDataRepository.TemplateParameter));
                    else
                        parameterSubstitutions.Add(genericType.TemplateBinding.GetActualParameterFor(orgParameterSubstitution.Formal));
                }
                if (genericType is MetaDataRepository.Interface)
                    return new Interface(new MetaDataRepository.TemplateBinding(genericType.TemplateBinding.Signature.Template, parameterSubstitutions), (templateBinding.BoundElement as MetaDataRepository.MetaObject).ImplementationUnit);
                if (genericType is MetaDataRepository.Class)
                    return new Class(new MetaDataRepository.TemplateBinding(genericType.TemplateBinding.Signature.Template, parameterSubstitutions), (templateBinding.BoundElement as MetaDataRepository.MetaObject).ImplementationUnit);
                if (genericType is MetaDataRepository.Structure)
                    return new Structure(new MetaDataRepository.TemplateBinding(genericType.TemplateBinding.Signature.Template, parameterSubstitutions), (templateBinding.BoundElement as MetaDataRepository.MetaObject).ImplementationUnit);
                return genericType;

            }
        }


        /// <MetaDataID>{4ee8df78-72b0-48f0-ab2a-07c654f68e71}</MetaDataID>
        static public void LoadFeatureFromGenericType(MetaDataRepository.Classifier classifier, OOAdvantech.Collections.Generic.Set<MetaDataRepository.Feature> _Features)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                System.Collections.Generic.List<MetaDataRepository.Operation> operations = new System.Collections.Generic.List<MetaDataRepository.Operation>();
                System.Collections.Generic.List<MetaDataRepository.Attribute> attributes = new System.Collections.Generic.List<MetaDataRepository.Attribute>();
                foreach (MetaDataRepository.Feature feature in (classifier.TemplateBinding.Signature.Template as MetaDataRepository.Classifier).Features)
                {
                    if (feature is MetaDataRepository.Operation)
                    {
                        MetaDataRepository.Operation genericOperation = feature as MetaDataRepository.Operation;
                        Operation operation = new Operation(genericOperation, classifier);
                        _Features.Add(operation);
                        operations.Add(operation);
                    }
                    if (feature is MetaDataRepository.Attribute)
                    {
                        MetaDataRepository.Attribute genericAttribute = feature as MetaDataRepository.Attribute;
                        Attribute attribute = new Attribute(genericAttribute, classifier);
                        _Features.Add(attribute);
                    }
                }
                foreach (MetaDataRepository.Feature feature in (classifier.TemplateBinding.Signature.Template as MetaDataRepository.Classifier).Features)
                {

                    if (feature is MetaDataRepository.Method)
                    {
                        MetaDataRepository.Operation operation = GetOperationForMethod(classifier, operations, feature as MetaDataRepository.Method);
                        Method method = new Method(operation, classifier);
                        _Features.Add(method);
                    }
                    if (feature is MetaDataRepository.AttributeRealization)
                    {
                        MetaDataRepository.Attribute specificationAttribute = null;
                        specificationAttribute = GetAttributeForAttributeRealization(classifier, attributes, feature as MetaDataRepository.AttributeRealization);
                        _Features.Add(new AttributeRealization(specificationAttribute, classifier));
                    }
                }
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{2e6b419a-4455-4596-b58d-c4ba94b478f5}</MetaDataID>
        static internal OOAdvantech.MetaDataRepository.Operation GetOperationForMethod(OOAdvantech.MetaDataRepository.Classifier classifier, System.Collections.Generic.List<MetaDataRepository.Operation> clasifierOperations, MetaDataRepository.Method genericMethod)
        {

            string methodSignature = Operation.GetSignature(genericMethod.Specification, classifier.TemplateBinding, classifier.ImplementationUnit as Project);
            if (classifier.OwnedTemplateSignature == null)
            {
                if (classifier is MetaDataRepository.InterfaceImplementor)
                {

                    foreach (MetaDataRepository.Interface _Interface in (classifier as MetaDataRepository.InterfaceImplementor).GetAllInterfaces())
                    {
                        foreach (MetaDataRepository.Feature feature in _Interface.Features)
                        {
                            string operationSignature = null;
                            MetaDataRepository.Operation operation = feature as MetaDataRepository.Operation;
                            if (operation != null && (operation.Name == genericMethod.Name || operation.FullName == genericMethod.Name) && operation.Parameters.Count == genericMethod.Specification.Parameters.Count)
                            {
                                operationSignature = Operation.GetSignature(operation, classifier.ImplementationUnit as Project);
                                if (operationSignature == methodSignature)
                                    return operation;
                            }
                        }
                    }
                }
            }

            foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
            {
                foreach (MetaDataRepository.Feature feature in generalClassifier.Features)
                {
                    string operationSignature = null;
                    MetaDataRepository.Operation operation = feature as MetaDataRepository.Operation;
                    if (operation != null && operation.Name == genericMethod.Name && operation.Parameters.Count == genericMethod.Specification.Parameters.Count)
                    {
                        operationSignature = Operation.GetSignature(operation, classifier.ImplementationUnit as Project);
                        if (operationSignature == methodSignature)
                            return operation;
                    }
                }
            }

            foreach (MetaDataRepository.Operation operation in clasifierOperations)
            {
                string operationSignature = null;
                if (operation.Name == genericMethod.Name && operation.Parameters.Count == genericMethod.Specification.Parameters.Count)
                {
                    operationSignature = Operation.GetSignature(operation, classifier.ImplementationUnit as Project);
                    if (operationSignature == methodSignature)
                        return operation;
                }
            }
            return null;
        }
        /// <MetaDataID>{5c13f448-c12d-4c0c-8362-688b9f41ae54}</MetaDataID>
        static private MetaDataRepository.Attribute GetAttributeForAttributeRealization(MetaDataRepository.Classifier classifier, System.Collections.Generic.List<MetaDataRepository.Attribute> classifierAttributes, MetaDataRepository.AttributeRealization genericAttributeRealization)
        {
            if (classifier is MetaDataRepository.InterfaceImplementor)
            {
                foreach (MetaDataRepository.Interface _interface in (classifier as MetaDataRepository.InterfaceImplementor).GetAllInterfaces())
                {
                    foreach (MetaDataRepository.Attribute attribute in _interface.GetAttributes(false))
                    {
                        MetaDataRepository.Classifier type = null;
                        MetaDataRepository.TemplateParameter parameterizedType = null;
                        if (genericAttributeRealization.ParameterizedType != null)
                        {
                            MetaDataRepository.IParameterableElement parameterable = classifier.TemplateBinding.GetActualParameterFor(genericAttributeRealization.ParameterizedType);
                            if (parameterable is MetaDataRepository.Classifier)
                                type = parameterable as MetaDataRepository.Classifier;
                            else
                                parameterizedType = parameterable as MetaDataRepository.TemplateParameter;
                        }
                        else if (genericAttributeRealization.Type != null)
                            type = genericAttributeRealization.Type;
                        if (attribute.Name == genericAttributeRealization.Name &&
                            attribute.Type != null &&
                            type != null &&
                            type.FullName == attribute.Type.FullName)
                        {
                            return attribute;
                        }
                        if (attribute.Name == genericAttributeRealization.Name &&
                            attribute.ParameterizedType != null &&
                            parameterizedType != null &&
                            parameterizedType.FullName == attribute.ParameterizedType.FullName)//.Owner.TemplateBinding.GetIntermediateParameterFor(attribute.ParameterizedType).Name
                        {
                            return attribute;
                        }
                    }

                }
            }
            foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
            {
                foreach (MetaDataRepository.Attribute attribute in generalClassifier.GetAttributes(false))
                {
                    MetaDataRepository.Classifier type = null;
                    MetaDataRepository.TemplateParameter parameterizedType = null;
                    if (genericAttributeRealization.ParameterizedType != null)
                    {
                        MetaDataRepository.IParameterableElement parameterable = classifier.TemplateBinding.GetActualParameterFor(genericAttributeRealization.ParameterizedType);
                        if (parameterable is MetaDataRepository.Classifier)
                            type = parameterable as MetaDataRepository.Classifier;
                        else
                            parameterizedType = parameterable as MetaDataRepository.TemplateParameter;
                    }
                    if (attribute.Name == genericAttributeRealization.Name &&
                        attribute.Type != null &&
                        type != null &&
                        type.FullName == attribute.Type.FullName)
                    {
                        return attribute;

                    }
                    if (attribute.Name == genericAttributeRealization.Name &&
                        attribute.ParameterizedType != null &&
                        parameterizedType != null &&
                        parameterizedType.FullName == attribute.ParameterizedType.FullName)//.Owner.TemplateBinding.GetIntermediateParameterFor(attribute.ParameterizedType).Name
                    {
                        return attribute;
                    }
                }

            }

            return null;
        }

        //static OOAdvantech.MetaDataRepository.Classifier voidClassifier;
        //static CodeClassifier()
        //{
        //    voidClassifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(void));
        //}
        /// <MetaDataID>{11b264a0-b6b4-4d7a-a2a6-f58e50df297c}</MetaDataID>
        static internal bool HasTypeChanged(EnvDTE.CodeTypeRef codeTypeRef, OOAdvantech.MetaDataRepository.Classifier type)
        {
            string orgTypeFullName = null;
            string newTypeFullName = null; try
            {

                if (codeTypeRef != null)
                    orgTypeFullName = codeTypeRef.AsFullName;
                if (type != null)
                    newTypeFullName = type.FullName;

            }
            catch (System.Exception error)
            {
            }
            return orgTypeFullName != newTypeFullName;
        }

        /// <MetaDataID>{6561bedf-d28c-48e3-b65f-cfb64180a9cc}</MetaDataID>
        static internal bool HasTypeChanged(EnvDTE.CodeTypeRef codeTypeRef, EnvDTE.CodeTypeRef newCodeTypeRef)
        {
            string orgTypeFullName = null;
            string newTypeFullName = null; try
            {

                if (codeTypeRef != null)
                    orgTypeFullName = codeTypeRef.AsFullName;
                if (newCodeTypeRef != null)
                    newTypeFullName = newCodeTypeRef.AsFullName;

            }
            catch (System.Exception error)
            {
            }
            return orgTypeFullName != newTypeFullName;
        }

        /// <MetaDataID>{da166d32-1065-4a22-8e5a-341da7be8d83}</MetaDataID>
        internal static void UpdateAttributeValue(EnvDTE.CodeAttribute codeAttribute, string value)
        {
            string oldAttributeValue = codeAttribute.Value;
            if (codeAttribute == null)
                return;
            if (codeAttribute.Value != value)
                codeAttribute.Value = value;
        }


        /// <MetaDataID>{c3ca0194-1deb-4e74-bcce-20d02ee01a62}</MetaDataID>
        internal static void BuildAssociations(MetaDataRepository.Classifier classifier, Collections.Generic.Set<MetaDataRepository.AssociationEnd> _Roles)
        {
            foreach (MetaDataRepository.AssociationEnd associationEnd in _Roles)
            {
                if (associationEnd is AssociationEnd)
                    BuildAssociation(classifier, associationEnd as AssociationEnd);

                //if (associationEnd.Association == null)
                //{
                //    if (associationEnd is AssociationEnd)
                //        (associationEnd as AssociationEnd).CodeElementRemoved();

                //}
            }

        }

        /// <MetaDataID>{13c2568c-b48c-4d87-8a63-40137cffa814}</MetaDataID>
        internal static OOAdvantech.MetaDataRepository.AssociationEnd GetClassifierRole(string associationEndIdentity, MetaDataRepository.Classifier classifier, Collections.Generic.Set<MetaDataRepository.AssociationEnd> classifierRoles, System.Collections.Generic.List<EnvDTE.CodeElement> codeClasifierMembers)
        {
            #region Retrieve code element members with assocition attributes
            System.Collections.Generic.List<AssociationAttriuteData> AssociationAttriutesData = new System.Collections.Generic.List<AssociationAttriuteData>();
            foreach (EnvDTE.CodeElement codeElement in codeClasifierMembers)
            {
                EnvDTE.CodeProperty codeProperty = codeElement as EnvDTE.CodeProperty;
                if (codeElement is EnvDTE.CodeProperty || codeElement is EnvDTE.CodeVariable)
                {
                    EnvDTE.CodeAttribute associationAttribute = null, associationClassAttribute = null;
                    CodeClassifier.GetAssociationAttributes(codeElement, out associationAttribute, out associationClassAttribute);
                    if (associationAttribute != null)
                    {
                        string associationName = null;
                        string currentAssociationIdentity = null;
                        MetaDataRepository.Roles associationEndRole = default(MetaDataRepository.Roles);
                        bool indexer = false;
                        string generalAssociationIdentity = null;
                        LanguageParser.GetAssociationData(associationAttribute, out associationName, out associationEndRole, out currentAssociationIdentity, out indexer, out generalAssociationIdentity);
                        if (associationEndRole == OOAdvantech.MetaDataRepository.Roles.RoleA)
                            associationEndRole = OOAdvantech.MetaDataRepository.Roles.RoleB;
                        else
                            associationEndRole = OOAdvantech.MetaDataRepository.Roles.RoleA;
                        AssociationAttriutesData.Add(new AssociationAttriuteData(associationName, currentAssociationIdentity, associationEndRole, codeElement, indexer, generalAssociationIdentity));
                    }
                }
            }
            #endregion

            #region Search for association end with identity the associationEndIdentity operation parameter
            foreach (AssociationAttriuteData associationAttriuteData in AssociationAttriutesData)
            {
                if (associationAttriuteData.AssociationIdentity + associationAttriuteData.AssociationEndRole.ToString() == associationEndIdentity)
                {
                    EnvDTE.CodeElement codeElement = associationAttriuteData.CodeElement;
                    if (codeElement is EnvDTE.CodeProperty)
                    {
                        AssociationEnd otherAssociationEnd = CodeClassifier.GetAssociationEnd(codeElement as EnvDTE.CodeProperty, classifier) as AssociationEnd;
                        if (otherAssociationEnd == null)
                            return null;
                        foreach (MetaDataRepository.AssociationEnd associationEnd in classifierRoles)
                        {
                            if ((associationEnd is AssociationEnd
                                && (associationEnd as AssociationEnd).Identity.ToString() == associationEndIdentity))
                                return associationEnd;
                        }

                        CodeClassifier.BuildAssociation(classifier, otherAssociationEnd);
                        foreach (MetaDataRepository.AssociationEnd associationEnd in classifierRoles)
                        {
                            if ((associationEnd is AssociationEnd
                                && (associationEnd as AssociationEnd).Identity.ToString() == associationEndIdentity))
                                return associationEnd;
                        }

                    }
                    if (codeElement is EnvDTE.CodeVariable)
                    {

                        AssociationEnd otherAssociationEnd = CodeClassifier.GetAssociationEnd(codeElement as EnvDTE.CodeVariable, classifier) as AssociationEnd;
                        if (otherAssociationEnd == null)
                            return null;
                        foreach (MetaDataRepository.AssociationEnd associationEnd in classifierRoles)
                        {
                            if ((associationEnd is AssociationEnd
                                && (associationEnd as AssociationEnd).Identity.ToString() == associationEndIdentity))
                                return associationEnd;
                        }
                        CodeClassifier.BuildAssociation(classifier, otherAssociationEnd);
                        foreach (MetaDataRepository.AssociationEnd associationEnd in classifierRoles)
                        {
                            if ((associationEnd is AssociationEnd
                                && (associationEnd as AssociationEnd).Identity.ToString() == associationEndIdentity))
                                return associationEnd;
                        }

                    }
                }
            }
            foreach (MetaDataRepository.AssociationEnd associationEnd in classifierRoles)
            {
                if (associationEnd.Identity.ToString() == associationEndIdentity)
                    return associationEnd;
            }
            return null;
            #endregion


        }

        /// <MetaDataID>{f7c5f4a7-dffd-436c-ac0b-e17406146eb0}</MetaDataID>
        static System.Collections.Generic.List<string> BuildAssociationIdentities = new List<string>();

        /// <MetaDataID>{07a452f1-5fef-40ff-ae52-42ae3825219f}</MetaDataID>
        internal static void BuildAssociation(MetaDataRepository.Classifier classifier, AssociationEnd associationEnd)
        {

            string associationIdentity = null;
            if (associationEnd is AssociationEnd)
                associationIdentity = (associationEnd as AssociationEnd).AssociationIdentity;

            if (BuildAssociationIdentities.Contains(associationIdentity))
                return;
            try
            {
                BuildAssociationIdentities.Add(associationIdentity);
                MetaDataRepository.AssociationEnd otherEnd = null;
                MetaDataRepository.AssociationEnd classifierEnd = null;
                if (associationEnd.IsRoleA)
                {
                    otherEnd = associationEnd.Specification.GetRole(associationEnd.AssociationIdentity + "RoleA");
                    classifierEnd = classifier.GetRole(associationEnd.AssociationIdentity + "RoleB");
                }
                else
                {
                    otherEnd = associationEnd.Specification.GetRole(associationEnd.AssociationIdentity + "RoleB");
                    classifierEnd = classifier.GetRole(associationEnd.AssociationIdentity + "RoleA");

                }

                if (classifierEnd != null)
                {
                    if (otherEnd == null && classifierEnd.Association != null)
                        return;
                    if (otherEnd.Association == null || otherEnd.Association != classifierEnd.Association)
                    {
                        if (associationEnd.Role == OOAdvantech.MetaDataRepository.Roles.RoleA)
                            new Association(associationEnd.AssociationName, otherEnd, classifierEnd, associationEnd.AssociationIdentity, associationEnd.GeneralAssociation);
                        else
                            new Association(associationEnd.AssociationName, classifierEnd, otherEnd, associationEnd.AssociationIdentity, associationEnd.GeneralAssociation);
                    }
                }
                else
                {
                    if (associationEnd.Role == MetaDataRepository.Roles.RoleA)
                    {
                        if (otherEnd == null)
                            otherEnd = associationEnd;

                        if (otherEnd.Specification is CodeElementContainer)
                            classifierEnd = new AssociationEnd("", classifier, MetaDataRepository.Roles.RoleB, associationIdentity);
                        else
                            classifierEnd = new MetaDataRepository.AssociationEnd("", classifier, MetaDataRepository.Roles.RoleB);
                        if (otherEnd.Association == null || otherEnd.Association != classifierEnd.Association)
                            new Association(associationEnd.AssociationName, otherEnd, classifierEnd, associationEnd.AssociationIdentity, associationEnd.GeneralAssociation);
                    }
                    else
                    {
                        if (otherEnd == null)
                            otherEnd = associationEnd;

                        if (otherEnd.Specification is CodeElementContainer)
                            classifierEnd = new AssociationEnd("", classifier, MetaDataRepository.Roles.RoleA, associationIdentity);
                        else
                            classifierEnd = new MetaDataRepository.AssociationEnd("", classifier, MetaDataRepository.Roles.RoleA);

                        // classifierEnd = new MetaDataRepository.AssociationEnd("", classifier, MetaDataRepository.Roles.RoleA);
                        if (otherEnd.Association == null || otherEnd.Association != classifierEnd.Association)
                            new Association(associationEnd.AssociationName, classifierEnd, otherEnd, associationEnd.AssociationIdentity, associationEnd.GeneralAssociation);
                    }
                }
            }
            finally
            {
                BuildAssociationIdentities.Remove(associationIdentity);
            }
        }
        /// <MetaDataID>{f6ed532d-ca1b-4719-b5dd-8fd45236136b}</MetaDataID>
        internal static MetaDataRepository.AssociationEnd GetAssociationEnd(EnvDTE.CodeVariable codeVariable, MetaDataRepository.Classifier owner)
        {
            return GetAssociationEnd(codeVariable as EnvDTE.CodeElement, owner);
        }
        /// <MetaDataID>{3f2173a2-742d-4c9f-81ef-b6d0078618bd}</MetaDataID>
        internal static MetaDataRepository.AssociationEnd GetAssociationEnd(EnvDTE.CodeProperty codeProperty, MetaDataRepository.Classifier owner)
        {

            return GetAssociationEnd(codeProperty as EnvDTE.CodeElement, owner);
        }


        /// <MetaDataID>{85ef01e1-6f15-4459-96bc-775d65e8242d}</MetaDataID>
        static MetaDataRepository.AssociationEnd GetAssociationEnd(EnvDTE.CodeElement codeElement, MetaDataRepository.Classifier owner)
        {

            MetaDataRepository.AssociationEnd associationEnd = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.AssociationEnd;
            if (associationEnd != null)
            {
                (associationEnd as AssociationEnd).RefreshCodeElement(codeElement);
                return associationEnd;
            }

            EnvDTE.CodeAttribute associationAttribute = null;
            EnvDTE.CodeAttribute associationClassAttribute = null;

            GetAssociationAttributes(codeElement, out associationAttribute, out associationClassAttribute);
            if (associationAttribute == null)
                return null;
            string associationName = null;
            MetaDataRepository.Classifier otherEndType = null;
            MetaDataRepository.Roles associationEndRole;
            string identity = null;
            MetaDataRepository.Classifier associationClass = null;
            bool indexer = false;
            string generalAssociationIdentity = null;
            MetaDataRepository.Association generalAssociation = null;
            //MetaDataRepository.Classifier generalAssociationHostType = null;
            //foreach (MetaDataRepository.Generalization generalization in owner.Generalizations)
            //{
            //    generalAssociationHostType = generalization.Parent;
            //    break;
            //}
            LanguageParser.GetAssociationData(associationAttribute, out associationName, out otherEndType, out associationEndRole, out identity, out indexer, out generalAssociationIdentity);
            if (otherEndType == null)
                return null;
            if (associationClassAttribute != null)
                LanguageParser.GetAssociationClassData(associationClassAttribute, out associationClass);
            associationEnd = otherEndType.GetRole(identity + associationEndRole.ToString());
            //if (associationEnd == null)
            //    return associationEnd;
            if (generalAssociationIdentity != null)
            {
                foreach (MetaDataRepository.Classifier generalAssociationHostType in owner.GetAllGeneralClasifiers())
                {
                    if (generalAssociationHostType != null && generalAssociationIdentity != null)
                    {
                        foreach (MetaDataRepository.AssociationEnd generalAssociationEnd in generalAssociationHostType.GetRoles(true))
                        {
                            if (generalAssociationEnd.Association.Identity.ToString().ToLower() == generalAssociationIdentity.ToLower())
                            {
                                generalAssociation = generalAssociationEnd.Association;
                                break;
                            }
                        }
                        if (generalAssociation != null)
                            break;
                    }
                }
            }
            if (!(associationEnd is AssociationEnd))
            {

                MetaDataRepository.AssociationEnd oldAssociationEnd = associationEnd;

                if (MetaObjectMapper.FindMetaObjectFor(codeElement) != null)
                    MetaObjectMapper.RemoveType(codeElement);

                if (codeElement is EnvDTE.CodeProperty)
                    associationEnd = new AssociationEnd(codeElement as EnvDTE.CodeProperty, owner, otherEndType, associationEndRole, identity, associationName, associationClass, generalAssociation, indexer);
                else
                    associationEnd = new AssociationEnd(codeElement as EnvDTE.CodeVariable, owner, otherEndType, associationEndRole, identity, associationName, associationClass, generalAssociation, indexer);
                if (oldAssociationEnd != null)
                {
                    CodeMetaDataRepository.Association association = associationEnd.Association as CodeMetaDataRepository.Association;
                    if (association != null)
                    {
                        if (oldAssociationEnd.IsRoleA)
                            association.ReplaceRoleA(associationEnd);
                        else
                            association.ReplaceRoleB(associationEnd);
                    }
                }
            }
            else
            {
                (associationEnd as AssociationEnd).SetVSMember(codeElement, owner);
                (associationEnd as AssociationEnd).Indexer = indexer;
            }
            return associationEnd;

        }

        ///// <MetaDataID>{748531b4-b1fe-44a1-b87c-4892c7ea4c5c}</MetaDataID>
        //public static void ReBuildAssociation(AssociationEnd associationEnd)
        //{
        //    //TODO implement this functionality
        //    EnvDTE.CodeAttribute associationAttribute = null;
        //    EnvDTE.CodeAttribute associationClassAttribute = null;
        //    GetAssociationAttributes(associationEnd.CodeElement, ref associationAttribute, ref associationClassAttribute);
        //}

        /// <MetaDataID>{b61dcbe6-99ad-45ea-a829-4e8aab3e4baa}</MetaDataID>
        public static System.Collections.Generic.List<EnvDTE.CodeElement> GetMembers(EnvDTE.CodeElement codeClassifier)
        {
            System.Collections.Generic.List<EnvDTE.CodeElement> members = new System.Collections.Generic.List<EnvDTE.CodeElement>();
            switch (codeClassifier.Kind)
            {
                case EnvDTE.vsCMElement.vsCMElementClass:
                    {
                        foreach (EnvDTE.CodeClass vsClass in (codeClassifier as EnvDTE80.CodeClass2).PartialClasses)
                        {
                            foreach (EnvDTE.CodeElement codelement in vsClass.Members)
                                members.Add(codelement);
                        }
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementInterface:
                    {
                        foreach (EnvDTE.CodeElement codelement in (codeClassifier as EnvDTE.CodeInterface).Members)
                            members.Add(codelement);
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementStruct:
                    {
                        foreach (EnvDTE.CodeElement codelement in (codeClassifier as EnvDTE.CodeStruct).Members)
                            members.Add(codelement);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return members;

        }
        /// <summary>
        /// Retrieves the Association attribute and AssociationClass attribute from code element.
        /// The code element must be code property or code variable.
        /// </summary>
        /// <param name="codeElement">
        /// The codeElement parameter defines the code element which maybe has association attributes
        /// </param>
        /// <param name="associationAttribute">
        /// The associationAttribute is an out parameter where operation loads the association attribute 
        /// if it is defined in code element, otherwise the value of parameter is null.
        /// </param>
        /// <param name="associationClassAttribute">
        /// The associationClassAttribute is an out parameter where operation loads the association attribute 
        /// if it is defined in code element, otherwise the value of parameter is null.
        /// </param>
        /// <MetaDataID>{a39757e8-3c0b-48ac-8a7f-afd21f884cee}</MetaDataID>
        public static void GetAssociationAttributes(EnvDTE.CodeElement codeElement, out EnvDTE.CodeAttribute associationAttribute, out EnvDTE.CodeAttribute associationClassAttribute)
        {
            associationAttribute = null;
            associationClassAttribute = null;

            try
            {
                if (codeElement is EnvDTE.CodeProperty)
                {
                    foreach (EnvDTE.CodeAttribute codeAttribute in (codeElement as EnvDTE.CodeProperty).Attributes)
                    {
                        if (LanguageParser.IsAssociationAttribute(codeAttribute))
                            associationAttribute = codeAttribute;
                        if (LanguageParser.IsAssociationClassAttribute(codeAttribute))
                            associationClassAttribute = codeAttribute;
                    }
                }
                else
                {
                    foreach (EnvDTE.CodeAttribute codeAttribute in (codeElement as EnvDTE.CodeVariable).Attributes)
                    {
                        if (LanguageParser.IsAssociationAttribute(codeAttribute))
                            associationAttribute = codeAttribute;
                        if (LanguageParser.IsAssociationClassAttribute(codeAttribute))
                            associationClassAttribute = codeAttribute;
                    }
                }
            }
            catch (System.Exception error)
            {
            }
        }


        /// <summary>
        /// Retrieves the Association attribute from code element.
        /// The code element must be code property or code variable.
        /// </summary>
        /// <param name="codeElement">
        /// The codeElement parameter defines the code element which maybe has association attribute
        /// </param>
        /// <returns>
        /// The operation returns the the association attribute 
        /// if it is defined in code element, otherwise returns null.
        /// </returns>
        /// <MetaDataID>{1bc76fb3-a9ad-4ea8-8127-8025cd835b51}</MetaDataID>
        public static EnvDTE.CodeAttribute GetAssociationAttribute(EnvDTE.CodeElement codeElement)
        {
            EnvDTE.CodeAttribute associationAttribute = null;
            try
            {
                if (codeElement is EnvDTE.CodeProperty)
                {
                    foreach (EnvDTE.CodeAttribute codeAttribute in (codeElement as EnvDTE.CodeProperty).Attributes)
                    {
                        if (LanguageParser.IsAssociationAttribute(codeAttribute))
                        {
                            associationAttribute = codeAttribute;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (EnvDTE.CodeAttribute codeAttribute in (codeElement as EnvDTE.CodeVariable).Attributes)
                    {
                        if (LanguageParser.IsAssociationAttribute(codeAttribute))
                        {
                            associationAttribute = codeAttribute;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
            }
            return null;
        }

        /// <MetaDataID>{9f566d49-93bf-41f3-817e-d7540f6218fc}</MetaDataID>
        internal static bool UpdateRealizations(string oldSignature, string newSignature, MetaDataRepository.Classifier classifier, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> features)
        {
            bool updated = false;
            if (classifier.ImplementationUnit is Project)
            {
                System.Collections.Generic.List<EnvDTE.CodeElement> members = new List<EnvDTE.CodeElement>();
                foreach (MetaDataRepository.Feature feature in new List<MetaDataRepository.Feature>(features))
                {
                    if (oldSignature == CodeClassifier.GetSignature(feature, classifier.ImplementationUnit as Project))
                    {
                        members.Add((feature as CodeElementContainer).CodeElement);
                        (feature as CodeElementContainer).CodeElementRemoved();
                    }
                    else if (newSignature == CodeClassifier.GetSignature(feature, classifier.ImplementationUnit as Project))
                    {
                        members.Add((feature as CodeElementContainer).CodeElement);
                        (feature as CodeElementContainer).CodeElementRemoved();
                    }

                }
                if (members.Count > 0)
                {
                    updated = true;
                    LoadFeatures(classifier, features, members);
                }
            }
            return updated;
        }

        internal static bool HasDeclarationChange(CodeElementContainer codeElementContainer, EnvDTE.CodeElement element)
        {
            if (codeElementContainer is Class)
            {
                EnvDTE.CodeFunction functionMember = (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementFunction) as EnvDTE.CodeFunction;
                if (functionMember == null)
                    return true;
                foreach (MetaDataRepository.Feature feature in (codeElementContainer as Class).Features)
                {
                    if (feature is CodeElementContainer && (feature as CodeElementContainer).CodeElement == functionMember)
                    {
                        if (functionMember.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                            return true;
                        foreach (EnvDTE.CodeAttribute codeAttribute in functionMember.Attributes)
                        {
                            if (codeAttribute.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                                return true;
                        }
                        foreach (EnvDTE.CodeParameter codeParameter in functionMember.Parameters)
                        {
                            if (codeParameter.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                                return true;
                        }

                        return false;

                    }
                }



                EnvDTE.CodeProperty propertyMember = (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementFunction) as EnvDTE.CodeProperty;
                if (propertyMember == null)
                    return true;
                foreach (MetaDataRepository.Feature feature in (codeElementContainer as Class).Features)
                {
                    if (feature is CodeElementContainer && (feature as CodeElementContainer).CodeElement == propertyMember)
                    {
                        if (propertyMember.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                            return true;
                        foreach (EnvDTE.CodeAttribute codeAttribute in propertyMember.Attributes)
                        {
                            if (codeAttribute.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                                return true;
                        }
                        //foreach (EnvDTE.CodeParameter codeParameter in propertyMember.pa.Parameters)
                        //{
                        //    if (codeParameter.StartPoint.Line == (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.Line)
                        //        return true;
                        //}

                        return false;

                    }
                }


                return true;
            }
            else
                return true;


        }
    }
    /// <MetaDataID>{65fa8d89-a282-4a74-a0f2-a2529ac24495}</MetaDataID>
    class AssociationAttriuteData
    {
        /// <MetaDataID>{b751ae6e-2389-40e1-8b96-e02418d0749b}</MetaDataID>
        public AssociationAttriuteData(string associationName, string associationIdentity, MetaDataRepository.Roles associationEndRole, EnvDTE.CodeElement codeElement, bool indexer,/* string generalAssociationHostType,*/ string generalAssociationIdentity)
        {
            AssociationEndRole = associationEndRole;
            AssociationName = associationName;
            AssociationIdentity = associationIdentity;
            CodeElement = codeElement;
            Indexer = indexer;
            GeneralAssociationIdentity = generalAssociationIdentity;
        }


        /// <MetaDataID>{04927434-7ff0-48b6-b262-63e4704639f4}</MetaDataID>
        public readonly EnvDTE.CodeElement CodeElement;
        /// <MetaDataID>{3e9ce9c6-3159-4764-a16f-c8b126feb39f}</MetaDataID>
        public readonly string AssociationName = null;
        /// <MetaDataID>{09fae205-fcbc-403b-b72f-00ec280f98ad}</MetaDataID>
        public readonly string AssociationIdentity = null;
        /// <MetaDataID>{a3d0f589-ed20-4821-83ca-342045bdff10}</MetaDataID>
        public readonly bool Indexer;
        /// <MetaDataID>{23779dcb-2817-4b08-a900-22878c803a0f}</MetaDataID>
        public readonly MetaDataRepository.Roles AssociationEndRole;
        //  public readonly string GeneralAssociationHostType;
        /// <MetaDataID>{60119add-577a-4b59-ba2f-d4ebcb955a73}</MetaDataID>
        public readonly string GeneralAssociationIdentity;

    }
}
