namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{35A80C56-8461-45A5-90E9-F1FECDA06E45}</MetaDataID>
    public class Attribute : OOAdvantech.MetaDataRepository.Attribute, CodeElementContainer
    {

        /// <MetaDataID>{fb037df8-3b83-4964-85e0-3ce2fb0798ae}</MetaDataID>
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

        /// <MetaDataID>{17c0346c-a7ed-4281-a533-c171d4bd5448}</MetaDataID>
        public Attribute(string name, MetaDataRepository.Classifier owner)
        {
            _Name = name;
            _Owner = owner;
        }

        /// <MetaDataID>{dd488fc6-aa87-4317-a84e-3a4ee4df5623}</MetaDataID>
        internal Attribute()
        {
        }

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                string identity = PropertyValue as string;
                CodeClassifier.SetIdentityToCodeElement(VSMember, identity);
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
                if (VSMember == null)
                    return base.Identity;
                string document = null;
                CodeClassifier.LoadDocDocumentItems(VSMember as EnvDTE.CodeElement, out identity, out document);

                if (identity != null)
                {
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                    return identity;
                }
                System.Guid guid = System.Guid.NewGuid();
                try
                {
                    identity = "{" + guid.ToString() + "}";
                    CodeClassifier.SetIdentityToCodeElement(VSMember, identity);
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

        //public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (VSMember == null && _Identity == null)
        //                return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
        //            if (_Identity != null)
        //                return _Identity;
        //            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //            if (VSMember is EnvDTE.CodeVariable)
        //                document.LoadXml((VSMember as EnvDTE.CodeVariable).DocComment);
        //            else
        //                document.LoadXml((VSMember as EnvDTE.CodeProperty).DocComment);

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
        //            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("{" + guid.ToString() + "}");
        //            document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
        //            if (VSMember is EnvDTE.CodeVariable)
        //                (VSMember as EnvDTE.CodeVariable).DocComment = document.OuterXml;
        //            else
        //                (VSMember as EnvDTE.CodeProperty).DocComment = document.OuterXml;

        //            return _Identity;
        //        }
        //        catch (System.Exception error)
        //        {
        //            return new OOAdvantech.MetaDataRepository.MetaObjectID(FullName);
        //        }
        //    }
        //}

        /// <MetaDataID>{99540973-71ab-4bc5-8deb-d854fa5cee40}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
                return;


            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            try
            {
                if (VSMember is EnvDTE.CodeVariable)
                    document.LoadXml((VSMember as EnvDTE.CodeVariable).DocComment);
                if (VSMember is EnvDTE.CodeProperty)
                    document.LoadXml((VSMember as EnvDTE.CodeProperty).DocComment);

            }
            catch (System.Exception error)
            {
                document.LoadXml("<doc></doc>");
            }

            string documentXml = document.OuterXml;

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


                    if (VSMember is EnvDTE.CodeVariable)
                        (VSMember as EnvDTE.CodeVariable).DocComment = document.OuterXml;
                    if (VSMember is EnvDTE.CodeProperty)
                        (VSMember as EnvDTE.CodeProperty).DocComment = document.OuterXml;

                    return;
                }
            }
            document.DocumentElement.AppendChild(document.CreateElement("summary")).InnerText = documentation.ToString();
            if (VSMember is EnvDTE.CodeVariable)
                (VSMember as EnvDTE.CodeVariable).DocComment = document.OuterXml;
            if (VSMember is EnvDTE.CodeProperty)
                (VSMember as EnvDTE.CodeProperty).DocComment = document.OuterXml;

        }


        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            //base.Synchronize(OriginMetaObject);
            try
            {
                MetaObjectsStack.ActiveProject = ImplementationUnit as Project;
                bool changeName = _Name != OriginMetaObject.Name;
                _Name = OriginMetaObject.Name;

                MetaDataRepository.Attribute originAttribute = OriginMetaObject as MetaDataRepository.Attribute;
                MetaDataRepository.AssociationEnd originAssociationEnd = null; ;
                if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                    originAttribute = (OriginMetaObject as OOAdvantech.MetaDataRepository.AttributeRealization).Specification;

                if (OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                    originAssociationEnd = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification;


                bool isProperty = false;
                MetaDataRepository.VisibilityKind originVisibilityKind = default(MetaDataRepository.VisibilityKind);
                string backwardCompatibilityID = OriginMetaObject.GetPropertyValue(typeof(string), "MetaData", "BackwardCompatibilityID") as string;

                if (originAssociationEnd != null)
                {
                    isProperty = (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty");
                    originVisibilityKind = originAssociationEnd.Visibility;
                    Persistent = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Persistent;

                }
                if (originAttribute != null)
                {
                    isProperty = (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty");
                    originVisibilityKind = originAttribute.Visibility;
                    if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                        Persistent = (OriginMetaObject as OOAdvantech.MetaDataRepository.AttributeRealization).Persistent;
                    else
                        Persistent = originAttribute.Persistent;

                }

                string setter = null;
                string getter = null;
                if (originAttribute != null && ((bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") || _Owner is Interface))
                {
                    if (originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Getter") != null && (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Getter"))
                        getter = _Name;
                    if (originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Setter") != null && (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "Setter"))
                        setter = _Name;
                    if (setter == getter && setter == null)
                        getter = _Name;

                }
                if (originAssociationEnd != null && ((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") || _Owner is Interface))
                {
                    if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter"))
                        getter = _Name;
                    if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter"))
                        setter = _Name;
                    if (setter == getter && setter == null)
                        getter = _Name;

                }

                if (originAttribute != null)
                {

                    if (OriginMetaObject is MetaDataRepository.AttributeRealization)
                    {
                        if (_Owner == null || _Owner.Identity.ToString() != (OriginMetaObject as MetaDataRepository.AttributeRealization).Owner.Identity.ToString())
                            _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as MetaDataRepository.AttributeRealization).Owner, this) as MetaDataRepository.Classifier;
                    }
                    else
                    {
                        if (_Owner == null || _Owner.Identity.ToString() != originAttribute.Owner.Identity.ToString())
                            _Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Owner, this) as MetaDataRepository.Classifier;
                    }

                    if (originAttribute.Type == null)
                        _Type = null;
                    else
                    {

                        if (_Type != null && _Type.Identity.ToString() != originAttribute.Type.Identity.ToString())
                            _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Type, this) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Type == null)
                            _Type = UnknownClassifier.GetClassifier(originAttribute.Type.FullName, ImplementationUnit);
                    }
                }

                if (originAssociationEnd != null)
                {
                    //_Owner = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Namespace, this) as MetaDataRepository.Classifier;

                    if (originAssociationEnd.CollectionClassifier != null)
                    {

                        _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.CollectionClassifier, this) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Type == null)
                            _Type = UnknownClassifier.GetClassifier(originAssociationEnd.CollectionClassifier.FullName, ImplementationUnit);
                    }
                    else
                    {

                        _Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Type == null)
                            _Type = UnknownClassifier.GetClassifier(originAssociationEnd.Specification.FullName, ImplementationUnit);
                    }
                }

                if (OriginMetaObject is MetaDataRepository.AttributeRealization && VSMember is EnvDTE.CodeVariable)
                {
                    try
                    {

                        if ((VSMember as EnvDTE.CodeVariable).Parent is EnvDTE.CodeClass)
                            ((VSMember as EnvDTE.CodeVariable).Parent as EnvDTE.CodeClass).RemoveMember(VSMember);
                        else if ((VSMember as EnvDTE.CodeVariable).Parent is EnvDTE.CodeStruct)
                            ((VSMember as EnvDTE.CodeVariable).Parent as EnvDTE.CodeStruct).RemoveMember(VSMember);

                    }
                    catch (System.Exception error)
                    {


                    }
                    VSMember = null;


                }

                if (VSMember == null)
                {
                    try
                    {
                        if (_Identity == null)
                            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(OriginMetaObject.Identity.ToString());

                        if (isProperty)
                            _Kind = EnvDTE.vsCMElement.vsCMElementProperty;
                        else
                            _Kind = EnvDTE.vsCMElement.vsCMElementVariable;

                        if (_Owner is Class && !isProperty)
                        {
                            VSMember = (_Owner as Class).VSClass.AddVariable(_Name, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;
                            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
                            MetaObjectMapper.AddTypeMap(VSMember, this);

                        }

                        if (_Owner is Class && isProperty)
                        {
                            VSMember = (_Owner as Class).VSClass.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;
                            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
                            //EnvDTE.EditPoint edit = (VSMember as EnvDTE80.CodeProperty2).Getter.StartPoint.CreateEditPoint();
                            //edit.


                            //EnvDTE.EditPoint edit = VSMember.StartPoint.CreateEditPoint();
                            //string tmp = edit.GetLines(VSMember.StartPoint.Line, VSMember.EndPoint.Line);
                            //int lines = VSMember.StartPoint.Line - VSMember.EndPoint.Line;
                            //string newProperty = tmp.Substring(0, tmp.IndexOf("{"));
                            //newProperty += "{";
                            //if (string.IsNullOrWhiteSpace(getter))
                            //    newProperty += "get;";
                            //if (string.IsNullOrWhiteSpace(setter))
                            //    newProperty += "set;";
                            //newProperty += "}";
                            //while(lines>=0)
                            //{
                            //    lines--;
                            //    edit.Delete(edit.LineLength);

                            //}

                            //edit.Insert(newProperty);
                            ////VSMember.StartPoint.CreateEditPoint


                            MetaObjectMapper.AddTypeMap(VSMember, this);
                        }

                        if (_Owner is Structure && !isProperty)
                        {
                            VSMember = (_Owner as Structure).VSStruct.AddVariable(_Name, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;
                            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
                            MetaObjectMapper.AddTypeMap(VSMember, this);
                        }

                        if (_Owner is Structure && isProperty)
                        {
                            VSMember = (_Owner as Structure).VSStruct.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;
                            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
                            MetaObjectMapper.AddTypeMap(VSMember, this);
                        }


                        if (_Owner is Interface)
                        {
                            if (setter == getter && setter == null)
                                getter = _Name;

                            VSMember = (_Owner as Interface).VSInterface.AddProperty(getter, setter, _Type.FullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null) as EnvDTE.CodeElement;

                            MetaObjectMapper.AddTypeMap(VSMember, this);
                        }
                        int line = Line;
                        int lineCharOffset = LineCharOffset;


                        CodeClassifier.SetIdentityToCodeElement(VSMember, _Identity.ToString());

                        //System.Xml.XmlDocument document = new System.Xml.XmlDocument();

                        //System.Threading.Thread.Sleep(100);

                        //try
                        //{

                        //    if (VSMember is EnvDTE.CodeVariable)
                        //        document.LoadXml((VSMember as EnvDTE.CodeVariable).DocComment);
                        //    else
                        //        document.LoadXml((VSMember as EnvDTE.CodeProperty).DocComment);
                        //}
                        //catch (System.Exception error)
                        //{
                        //    document.LoadXml("<doc></doc>");
                        //}

                        //foreach (System.Xml.XmlNode node in document.DocumentElement)
                        //{
                        //    System.Xml.XmlElement element = node as System.Xml.XmlElement;
                        //    if (element == null)
                        //        continue;

                        //    if (element.Name.ToLower() == "MetaDataID".ToLower())
                        //    {
                        //        element.InnerText = _Identity.ToString();
                        //        if (VSMember is EnvDTE.CodeVariable)
                        //            (VSMember as EnvDTE.CodeVariable).DocComment = document.OuterXml;
                        //        else
                        //            (VSMember as EnvDTE.CodeProperty).DocComment = document.OuterXml;

                        //        return;
                        //    }
                        //}

                        //document.DocumentElement.AppendChild(document.CreateElement("MetaDataID")).InnerText = _Identity.ToString();
                        //if (VSMember is EnvDTE.CodeVariable)
                        //    (VSMember as EnvDTE.CodeVariable).DocComment = document.OuterXml;
                        //else
                        //    (VSMember as EnvDTE.CodeProperty).DocComment = document.OuterXml;
                    }
                    catch (System.Exception error)
                    {
                        if (MetaDataRepository.SynchronizerSession.ErrorLog != null)
                            MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(_Owner.FullName + " :    " + error.Message);
                        return;
                    }

                }
                else
                {
                    if (changeName)
                        VSMember.Name = _Name;
                    if (VSMember is EnvDTE.CodeVariable)
                    {
                        if (CodeClassifier.HasTypeChanged((VSMember as EnvDTE.CodeVariable).Type, _Type))
                            (VSMember as EnvDTE.CodeVariable).Type = LanguageParser.CreateCodeTypeRef(_Type, VSMember.ProjectItem.ContainingProject.CodeModel);

                    }
                    else
                    {
                        if (CodeClassifier.HasTypeChanged((VSMember as EnvDTE.CodeProperty).Type, _Type))
                            (VSMember as EnvDTE.CodeProperty).Type = LanguageParser.CreateCodeTypeRef(_Type, VSMember.ProjectItem.ContainingProject.CodeModel);
                    }
                }




                if (Visibility != originVisibilityKind)
                {
                    Visibility = originVisibilityKind;
                    EnvDTE.vsCMAccess access;

                    if (_Owner is Class)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                            access = (VSMember as EnvDTE.CodeVariable).Access;
                        else
                            access = (VSMember as EnvDTE.CodeProperty).Access;

                        if (VSAccessTypeConverter.GetAccessType(Visibility) == EnvDTE.vsCMAccess.vsCMAccessProject &&
                            access == EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected)
                        {
                            access = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;
                        }
                        else
                            access = VSAccessTypeConverter.GetAccessType(Visibility);

                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).Access = access;
                        else
                            (VSMember as EnvDTE.CodeProperty).Access = access;
                    }
                    if (_Owner is Structure)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                            access = (VSMember as EnvDTE.CodeVariable).Access;
                        else
                            access = (VSMember as EnvDTE.CodeProperty).Access;

                        if (VSAccessTypeConverter.GetAccessType(Visibility) == EnvDTE.vsCMAccess.vsCMAccessProject &&
                            access == EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected)
                        {
                            access = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;
                        }
                        else
                            access = VSAccessTypeConverter.GetAccessType(Visibility);

                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).Access = access;
                        else
                            (VSMember as EnvDTE.CodeProperty).Access = access;
                    }
                }

                if (VSMember is EnvDTE.CodeProperty && (VSMember as EnvDTE.CodeProperty).Setter == null && setter != null)
                    LanguageParser.AddSetter(VSMember as EnvDTE.CodeProperty, (_Owner as CodeElementContainer).CodeElement);

                if (VSMember is EnvDTE.CodeProperty && (VSMember as EnvDTE.CodeProperty).Getter == null && getter != null)
                    LanguageParser.AddGetter(VSMember as EnvDTE.CodeProperty, (_Owner as CodeElementContainer).CodeElement);

                if (OriginMetaObject is MetaDataRepository.AttributeRealization
                && (OriginMetaObject as MetaDataRepository.AttributeRealization).Specification.Owner is OOAdvantech.MetaDataRepository.Class)
                {

                    if ((VSMember as EnvDTE.CodeProperty).Getter is EnvDTE80.CodeFunction2)
                        ((VSMember as EnvDTE.CodeProperty).Getter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;

                    if ((VSMember as EnvDTE.CodeProperty).Setter is EnvDTE80.CodeFunction2)
                        ((VSMember as EnvDTE.CodeProperty).Setter as EnvDTE80.CodeFunction2).OverrideKind = EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride;

                }


                //_Type = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Type, this) as OOAdvantech.MetaDataRepository.Classifier;
                //if (_Type == null)
                //    _Type = UnknownClassifier.GetClassifier(originAttribute.Type.FullName);

               

                if (originAttribute != null)
                    base.Synchronize(originAttribute);

                if (originAttribute != null)
                {
                    isProperty = (bool)originAttribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty");
                    originVisibilityKind = originAttribute.Visibility;
                    if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                        Persistent = (OriginMetaObject as OOAdvantech.MetaDataRepository.AttributeRealization).Persistent;
                    else
                        Persistent = originAttribute.Persistent;

                }


                string persistentMemberName = "nameof(" + GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")";
                SetDocumentation(OriginMetaObject.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);

                if (originAssociationEnd != null)
                    persistentMemberName = "nameof(" + (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")";


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


                bool persistentAttributeExist = false;
                bool backwardCompatibilityIDExist = false;
                bool associationClassRoleAttributeExist = false;


                EnumerateAttributes:
                EnvDTE.CodeElements attributes = null;
                if (VSMember is EnvDTE.CodeVariable)
                    attributes = (VSMember as EnvDTE.CodeVariable).Attributes;
                if (VSMember is EnvDTE.CodeProperty)
                    attributes = (VSMember as EnvDTE.CodeProperty).Attributes;


                string attributeRole = GetPropertyValue(typeof(string), "MetaData", "AssociationClassRole") as string;
                

                foreach (EnvDTE.CodeAttribute attribute in attributes)
                {

                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute) && !Persistent)
                    {
                        attribute.Delete();
                        goto EnumerateAttributes;
                    }
                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute) && Persistent)
                    {
                        if (VSMember is EnvDTE.CodeProperty)
                            CodeClassifier.UpdateAttributeValue(attribute, persistentMemberName);
                        else
                            CodeClassifier.UpdateAttributeValue(attribute, "");

                        persistentAttributeExist = true;
                    }


                    if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationClassRole), attribute))
                    {
                        if (attributeRole == "None" || string.IsNullOrEmpty(attributeRole))
                        {
                            attribute.Delete();
                            goto EnumerateAttributes;
                        }


                        EnvDTE.ProjectItem projectItem = null;
                        if (VSMember is EnvDTE.CodeVariable)
                            projectItem = (VSMember as EnvDTE.CodeVariable).ProjectItem;

                        if (VSMember is EnvDTE.CodeProperty)
                            projectItem = (VSMember as EnvDTE.CodeProperty).ProjectItem;


                        if (attributeRole == "RoleA")
                            CodeClassifier.UpdateAttributeValue(attribute, LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Roles).FullName, VSMember) + ".RoleA");
                        if (attributeRole == "RoleB")
                            CodeClassifier.UpdateAttributeValue(attribute, LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Roles).FullName, VSMember) + ".RoleB");

                        if (OriginMetaObject is MetaDataRepository.AttributeRealization && !string.IsNullOrEmpty(persistentMemberName))
                            CodeClassifier.UpdateAttributeValue(attribute, attribute.Value + "," + persistentMemberName);

                        associationClassRoleAttributeExist = true;
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
                {
                    if (VSMember is EnvDTE.CodeVariable)
                        (VSMember as EnvDTE.CodeVariable).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.PersistentMember).FullName, VSMember), persistentMemberName, attributePosition++);
                    if (VSMember is EnvDTE.CodeProperty)
                        (VSMember as EnvDTE.CodeProperty).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.PersistentMember).FullName, VSMember), persistentMemberName, attributePosition++);
                }
            
                if (!string.IsNullOrEmpty(attributeRole) && attributeRole != "None" && !associationClassRoleAttributeExist)
                {
                    EnvDTE.ProjectItem projectItem = null;
                    if (VSMember is EnvDTE.CodeVariable)
                        projectItem = (VSMember as EnvDTE.CodeVariable).ProjectItem;

                    if (VSMember is EnvDTE.CodeProperty)
                        projectItem = (VSMember as EnvDTE.CodeProperty).ProjectItem;

                    string attributeValue = null;
                    if (attributeRole == "RoleA")
                        attributeValue = LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Roles).FullName, VSMember) + ".RoleA";
                    if (attributeRole == "RoleB")
                        attributeValue = LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.Roles).FullName, VSMember) + ".RoleB";
                    if (OriginMetaObject is MetaDataRepository.AttributeRealization && !string.IsNullOrEmpty(persistentMemberName))
                        attributeValue = attributeValue + "," + persistentMemberName;


                    if (VSMember is EnvDTE.CodeVariable)
                        (VSMember as EnvDTE.CodeVariable).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClassRole).FullName, VSMember), attributeValue, attributePosition++);
                    if (VSMember is EnvDTE.CodeProperty)
                        (VSMember as EnvDTE.CodeProperty).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClassRole).FullName, VSMember), attributeValue, attributePosition++); ;
                }


                if (!backwardCompatibilityIDExist && !string.IsNullOrEmpty(backwardCompatibilityID))
                {
                    if (VSMember is EnvDTE.CodeVariable)
                        (VSMember as EnvDTE.CodeVariable).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSMember), "\"" + backwardCompatibilityID + "\"", attributePosition++);
                    if (VSMember is EnvDTE.CodeProperty)
                        (VSMember as EnvDTE.CodeProperty).AddAttribute(LanguageParser.GetShortName(typeof(MetaDataRepository.BackwardCompatibilityID).FullName, VSMember), "\"" + backwardCompatibilityID + "\"", attributePosition++); ;

                }


                MetaObjectChangeState();

            }
            catch (System.Exception error)
            {
                throw;

            }



        }







        /// <MetaDataID>{F458C226-E3DE-4033-A628-D771B07E6D42}</MetaDataID>
        private EnvDTE.CodeElement VSMember;

        /// <MetaDataID>{6b7feaea-d124-4656-9617-9d16339037a8}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Getter
        {
            get
            {
                if (!(VSMember is EnvDTE.CodeProperty))
                    return base.Getter;
                if (_Getter == null && (VSMember as EnvDTE.CodeProperty).Getter != null)
                {
                    _Getter = new Operation((VSMember as EnvDTE.CodeProperty).Getter, _Namespace.Value as OOAdvantech.MetaDataRepository.Classifier);
                    _Getter.Name = "get_" + _Getter.Name;
                }
                return _Getter;
            }
            set
            {
                base.Getter = value;
            }
        }
        /// <MetaDataID>{5c07c685-be67-4e6e-9cf9-33f5ae4fd6de}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Setter
        {
            get
            {
                if (!(VSMember is EnvDTE.CodeProperty))
                    return base.Setter;
                if (_Setter == null && (VSMember as EnvDTE.CodeProperty).Setter != null)
                {
                    _Setter = new Operation((VSMember as EnvDTE.CodeProperty).Setter, _Namespace.Value as OOAdvantech.MetaDataRepository.Classifier);
                    _Setter.Name = "set_" + _Setter.Name;
                    _Setter.AddParameter("value", Type, "");
                }
                return _Setter;
            }
            set
            {
                base.Setter = value;
            }
        }
        /// <MetaDataID>{e8eeb7aa-0cf1-4702-9c13-2fdc0d27b794}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier Type
        {
            get
            {
                try
                {
                    if (_Type == null)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                        {

                            string typeFullName = null;
                            if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeVariable).Type.AsFullName))
                                typeFullName = (VSMember as EnvDTE.CodeVariable).Type.AsFullName;
                            if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeVariable).Type.AsString))
                                typeFullName = (VSMember as EnvDTE.CodeVariable).Type.AsString;


                            if ((VSMember as EnvDTE.CodeVariable).Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefOther)
                            {
                                if (_Owner.OwnedTemplateSignature != null)
                                    _ParameterizedType = _Owner.OwnedTemplateSignature.GetParameterWithName((VSMember as EnvDTE.CodeVariable).Type.AsString);
                            }
                            else
                            {
                                if (_Owner.OwnedTemplateSignature != null)
                                {
                                    if ((VSMember as EnvDTE.CodeVariable).Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
                                        _Type = (_Owner.ImplementationUnit as Project).GetClassifier(_Owner, (VSMember as EnvDTE.CodeVariable).Type.CodeType as EnvDTE.CodeElement);
                                    else
                                        _Type = (Owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeVariable).Type);

                                }
                                else
                                    _Type = (Owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeVariable).Type);
                                if (_Type == null && !string.IsNullOrEmpty(typeFullName))
                                    _Type = UnknownClassifier.GetClassifier(typeFullName, Owner.ImplementationUnit);

                            }

                        }
                        if (VSMember is EnvDTE.CodeProperty)
                        {
                            string typeFullName = null;
                            if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeProperty).Type.AsString))
                                typeFullName = (VSMember as EnvDTE.CodeProperty).Type.AsFullName;
                            if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeProperty).Type.AsString))
                                typeFullName = (VSMember as EnvDTE.CodeProperty).Type.AsFullName;

                            if ((VSMember as EnvDTE.CodeProperty).Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefOther)
                            {
                                if (_Owner.OwnedTemplateSignature != null)
                                    _ParameterizedType = _Owner.OwnedTemplateSignature.GetParameterWithName((VSMember as EnvDTE.CodeProperty).Type.AsString);
                            }
                            else
                            {
                                _Type = (Owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeProperty).Type);

                                if (_Type == null && !string.IsNullOrEmpty(typeFullName))
                                    _Type = UnknownClassifier.GetClassifier(typeFullName, Owner.ImplementationUnit);
                            }
                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                return base.Type;
            }
            set
            {
                base.Type = value;
            }
        }

        /// <MetaDataID>{ea0c1273-8670-432f-aebb-63a6ba336b38}</MetaDataID>
        public Attribute(EnvDTE.CodeVariable vsMember, MetaDataRepository.Classifier owner) :
            this(vsMember as EnvDTE.CodeElement, owner)
        {

        }

        /// <MetaDataID>{02b6b4b1-769f-4093-bdb0-c5d5a8026702}</MetaDataID>
        public Attribute(EnvDTE.CodeProperty vsMember, MetaDataRepository.Classifier owner)
            : this(vsMember as EnvDTE.CodeElement, owner)
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

        /// <MetaDataID>{87a10de5-3728-41b9-a782-5d0db29ab91c}</MetaDataID>
        Attribute(EnvDTE.CodeElement vsMember, MetaDataRepository.Classifier owner)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            VSMember = vsMember as EnvDTE.CodeElement;

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(vsMember as EnvDTE.CodeElement, out identity, out comments);
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

            _Kind = VSMember.Kind;
            _Name = VSMember.Name;
            if (vsMember is EnvDTE.CodeVariable)
                Visibility = VSAccessTypeConverter.GetVisibilityKind((vsMember as EnvDTE.CodeVariable).Access);
            if (vsMember is EnvDTE.CodeProperty)
                Visibility = VSAccessTypeConverter.GetVisibilityKind((vsMember as EnvDTE.CodeProperty).Access);

            try
            {

                if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
                {
                    _LineCharOffset = VSMember.StartPoint.LineCharOffset;
                    _Line = VSMember.StartPoint.Line;
                    _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
                }
            }
            catch (System.Exception error)
            {
            }
            MetaObjectMapper.AddTypeMap(VSMember, this);
            EnvDTE.CodeElements attributes = null;
            if (VSMember is EnvDTE.CodeVariable)
                attributes = (VSMember as EnvDTE.CodeVariable).Attributes;
            if (VSMember is EnvDTE.CodeProperty)
                attributes = (VSMember as EnvDTE.CodeProperty).Attributes;
            foreach (EnvDTE.CodeAttribute attribute in attributes)
            {
                if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), attribute))
                {
                    Persistent = true;
                    break;
                }
            }
        }
        /// <MetaDataID>{61bd7116-645c-4f96-86a1-b5590104ca4e}</MetaDataID>
        public Attribute(MetaDataRepository.Attribute genericAttribute, MetaDataRepository.Classifier owner)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            _Name = genericAttribute.Name;
            _IsStatic = genericAttribute.IsStatic;
            Visibility = genericAttribute.Visibility;
            if (genericAttribute.Type != null)
                _Type = CodeClassifier.GetActualType(genericAttribute.Type, owner.TemplateBinding);
            else
            {
                MetaDataRepository.IParameterableElement actualParameter = owner.TemplateBinding.GetActualParameterFor(genericAttribute.ParameterizedType);
                if (actualParameter is MetaDataRepository.Classifier)
                    _Type = actualParameter as MetaDataRepository.Classifier;
                else
                    _ParameterizedType = actualParameter as MetaDataRepository.TemplateParameter;

            }
            object obj = Type;
            obj = ParameterizedType;
        }

        ///// <MetaDataID>{fd44861c-de4c-4dea-b73b-d36b77a2d4a2}</MetaDataID>
        //private OOAdvantech.MetaDataRepository.Classifier GetActualType(MetaDataRepository.Classifier genericType)
        //{
        //    OOAdvantech.MetaDataRepository.Classifier actualType = null;
        //    if (genericType.OwnedTemplateSignature != null)
        //    {
        //        Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>(genericType.OwnedTemplateSignature.OwnedParameters.Count);

        //        for (int i = 0; i < genericType.OwnedTemplateSignature.OwnedParameters.Count; i++)
        //        {
        //            parameterSubstitutions[i] = Owner.TemplateBinding.GetActualParameterFor(genericType.OwnedTemplateSignature.OwnedParameters[i].Name);
        //        }
        //        if (genericType is MetaDataRepository.Interface)
        //            actualType = new Interface(new OOAdvantech.MetaDataRepository.TemplateBinding(genericType, parameterSubstitutions), Owner.ImplementationUnit);
        //        else
        //            actualType = new Class(new OOAdvantech.MetaDataRepository.TemplateBinding(genericType, parameterSubstitutions), Owner.ImplementationUnit);

        //    }
        //    else if (genericType.ContainsTemplateParameter)
        //    {
        //        try
        //        {
        //            Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>(genericType.TemplateBinding.ParameterSubstitutions.Count);

        //            for (int i = 0; i < genericType.TemplateBinding.ParameterSubstitutions.Count; i++)
        //            {
        //                if (genericType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] is MetaDataRepository.Classifier)
        //                    parameterSubstitutions.Add(genericType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0]);
        //                else
        //                    parameterSubstitutions.Add(Owner.TemplateBinding.GetActualParameterFor((genericType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as MetaDataRepository.TemplateParameter).Name));
        //            }
        //            if (genericType is MetaDataRepository.Interface)
        //                actualType = new Interface(new OOAdvantech.MetaDataRepository.TemplateBinding(genericType, parameterSubstitutions), Owner.ImplementationUnit);
        //            else
        //                actualType = new Class(new OOAdvantech.MetaDataRepository.TemplateBinding(genericType, parameterSubstitutions), Owner.ImplementationUnit);
        //        }
        //        catch (System.Exception error)
        //        {
        //            int rtytr = 0;
        //        }
        //    }
        //    else
        //        actualType = genericType;
        //    return actualType;
        //}
        //public Attribute(EnvDTE.CodeProperty vsMember, MetaDataRepository.Classifier owner,MetaDataRepository.Classifier type)
        //    : base(vsMember.Name, type)
        //{
        //    _Owner = owner;
        //    _Namespace = owner;
        //    VSMember = vsMember as EnvDTE.CodeElement;
        //    _Kind = VSMember.Kind;
        //    _Name = VSMember.Name;
        //    Visibility = VSAccessTypeConverter.GetVisibilityKind(vsMember.Access);
        //    try
        //    {
        //        if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
        //        {
        //            _LineCharOffset = VSMember.StartPoint.LineCharOffset;
        //            _Line = VSMember.StartPoint.Line;
        //            (MetaObjectMapper.FindMetaObjectFor(VSMember.ProjectItem) as ProjectItem).MetaObjectImplementations.Add(this);
        //        }
        //    }
        //    catch (System.Exception error)
        //    {
        //    }
        //}

        /// <MetaDataID>{700347e7-5b35-4bfc-b419-009240f1c55a}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{c3f8aa59-77af-4b17-bdbf-f9c3687a532c}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            VSMember = null;
            MetaObjectMapper.RemoveMetaObject(this);
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);

            if (_Owner != null)
                _Owner.RemoveAttribute(this);

            _ProjectItem = null;

            _Owner = null;

        }
        /// <MetaDataID>{6aa82931-4a2a-44ef-9029-c30d70cc3d8b}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                if (VSMember == null && Owner.TemplateBinding != null && Owner.ImplementationUnit is Project)
                {

                    EnvDTE.CodeElement cdelement = (Owner.ImplementationUnit as Project).VSProject.CodeModel as EnvDTE.CodeElement;
                    return (Owner.ImplementationUnit as Project).VSProject as EnvDTE.CodeElement;
                }
                return VSMember;
            }
        }
        /// <MetaDataID>{219f03eb-3284-4ff8-b483-46b51f9f1012}</MetaDataID>
        bool HaveTheSameFullName(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (itsParent is EnvDTE.CodeElement)
            {
                EnvDTE.CodeElement vsMemberParent = null; ;
                EnvDTE.CodeElement codeElementParent = null;
                try
                {
                    if (VSMember is EnvDTE.CodeProperty)
                        vsMemberParent = (VSMember as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
                    if (VSMember is EnvDTE.CodeVariable)
                        vsMemberParent = (VSMember as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
                }
                catch (System.Exception error)
                {
                }
                try
                {
                    if (codeElement is EnvDTE.CodeProperty)
                        codeElementParent = (codeElement as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
                    if (codeElement is EnvDTE.CodeVariable)
                        codeElementParent = (codeElement as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
                }
                catch (System.Exception error)
                {
                }
                try
                {
                    if ((itsParent as EnvDTE.CodeElement).FullName + "." + codeElement.Name == CurrentProgramLanguageFullName)
                    {
                        if (vsMemberParent == codeElementParent && vsMemberParent == null)
                            return true;
                    }
                }
                catch (System.Exception error)
                {
                }
            }
            return false;
        }
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {

            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;

            if (codeElement is EnvDTE.CodeProperty)
            {
                MetaDataRepository.Attribute attribute = CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty);
                if (attribute != null && attribute != this)
                    return false;
                MetaDataRepository.AssociationEnd associationEnd = CodeClassifier.GetAssociaionEndForAssociatioEndRealization(Owner, codeElement as EnvDTE.CodeProperty);
                if (associationEnd != null)
                    return false;

            }

            if (this.VSMember == codeElement)
            {
                if (codeElement is EnvDTE.CodeProperty)
                {
                    MetaDataRepository.Attribute attribute = CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty);
                    if (attribute != null && attribute != this)
                        return false;
                }

                return true;
            }
            if (_ProjectItem != ProjectItem.GetProjectItem(codeElement.ProjectItem))
                return false;


            EnvDTE.CodeAttribute associationAttribute = CodeClassifier.GetAssociationAttribute(codeElement);

            if (associationAttribute != null)
                return false;
            try
            {
                if (Line == codeElement.StartPoint.Line &&
                    LineCharOffset == codeElement.StartPoint.LineCharOffset)
                {
                    if (codeElement is EnvDTE.CodeProperty)
                    {
                        MetaDataRepository.Attribute attribute = CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty);
                        if (attribute != null && attribute != this)
                            return false;
                    }

                    return true;
                }
            }
            catch (System.Exception error)
            {
            }

            if (itsParent is EnvDTE.CodeElement)
            {
                EnvDTE.CodeElement vsMemberParent = null; ;
                EnvDTE.CodeElement codeElementParent = null;
                try
                {
                    if (VSMember is EnvDTE.CodeProperty)
                        vsMemberParent = (VSMember as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
                    if (VSMember is EnvDTE.CodeVariable)
                        vsMemberParent = (VSMember as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
                }
                catch (System.Exception error)
                {
                }
                try
                {
                    if (codeElement is EnvDTE.CodeProperty)
                        codeElementParent = (codeElement as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
                    if (codeElement is EnvDTE.CodeVariable)
                        codeElementParent = (codeElement as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
                }
                catch (System.Exception error)
                {
                }
                try
                {
                    if ((itsParent as EnvDTE.CodeElement).FullName + "." + codeElement.Name == CurrentProgramLanguageFullName)
                    {
                        if (vsMemberParent == codeElementParent && vsMemberParent == null)
                        {
                            if (codeElement is EnvDTE.CodeProperty)
                            {
                                MetaDataRepository.Attribute attribute = CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty);
                                if (attribute != null && attribute != this)
                                    return false;
                            }

                            return true;
                        }
                    }
                }
                catch (System.Exception error)
                {
                }
            }
            //if (HaveTheSameFullName())
            //    return true;

            try
            {


                if (codeElement.FullName == FullName)
                {
                    if (codeElement is EnvDTE.CodeProperty)
                    {
                        MetaDataRepository.Attribute attribute = CodeClassifier.GetAttributeForAttributeRealization(Owner, codeElement as EnvDTE.CodeProperty);
                        if (attribute != null && attribute != this)
                            return false;
                    }

                    return true;
                }
            }
            catch (System.Exception error)
            {
            }
            return false;

        }

        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {

            //if (VSMember == codeElement)
            //    return;
            bool changed = false;
            MetaObjectMapper.RemoveMetaObject(this);
            VSMember = codeElement;
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

            MetaObjectMapper.AddTypeMap(VSMember, this);
            if (_Name != VSMember.Name)
            {
                _Name = VSMember.Name;
                changed = true;
            }

            //string id = Identity.ToString();
            OOAdvantech.MetaDataRepository.Classifier classifier = null;
            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementVariable)
            {
                if (Visibility != VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeVariable).Access))
                {
                    Visibility = VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeVariable).Access);
                    changed = true;
                }
                if (Type != null)
                {
                    try
                    {
                        string typeFullName = LanguageParser.GetTypeFullName(Type, (ImplementationUnit as Project).VSProject);
                        if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeVariable).Type.AsFullName) && typeFullName == (VSMember as EnvDTE.CodeVariable).Type.AsFullName)
                            classifier = Type;
                        else if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeVariable).Type.AsFullName) && typeFullName == (VSMember as EnvDTE.CodeVariable).Type.AsString)
                            classifier = Type;
                        if (classifier != null)
                        {
                            int mm = classifier.Features.Count;
                        }
                    }
                    catch (System.Exception error)
                    {
                    }
                }
                if (classifier == null)
                    classifier = (Owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeVariable).Type);
            }
            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementProperty)
            {
                if (Visibility != VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeProperty).Access))
                {
                    Visibility = VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeProperty).Access);
                    changed = true;
                }
                if (Type != null)
                {
                    try
                    {
                        string typeFullName = LanguageParser.GetTypeFullName(Type, (ImplementationUnit as Project).VSProject);
                        if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeProperty).Type.AsFullName) && typeFullName == (VSMember as EnvDTE.CodeProperty).Type.AsFullName)
                            classifier = Type;
                        else if (!string.IsNullOrEmpty((VSMember as EnvDTE.CodeProperty).Type.AsFullName) && typeFullName == (VSMember as EnvDTE.CodeProperty).Type.AsString)
                            classifier = Type;

                    }
                    catch (System.Exception error)
                    {
                    }
                }
                if (classifier == null)
                    classifier = (Owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeProperty).Type);
            }
            if (Type != classifier)
            {
                changed = true;
                Type = classifier;
            }
            if (changed)
                MetaObjectChangeState();
            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);


        }

        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSMember.StartPoint.LineCharOffset;
                if (_Line != VSMember.StartPoint.Line)
                {
                    _Line = VSMember.StartPoint.Line;
                    //MetaObjectChangeState();
                }

            }
            catch (System.Exception error)
            {
            }
        }


        /// <MetaDataID>{d68cfe3c-d2d6-43c6-9a3e-ed0a1627c0fb}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{20d7df47-42e9-40a9-b5fb-d74e5e023f40}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{9b7647e4-c2c1-4979-93e0-cd0bdb9688eb}</MetaDataID>
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
                    //MetaObjectChangeState();
                }
            }
        }
        /// <MetaDataID>{6b186995-4e58-412e-bed7-af07b1a9af42}</MetaDataID>
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


    }
}
