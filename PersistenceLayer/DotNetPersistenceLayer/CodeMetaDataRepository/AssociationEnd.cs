namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{FB5DEA3D-848B-4779-A46E-7AFAC513CF55}</MetaDataID>
    public class AssociationEnd : OOAdvantech.MetaDataRepository.AssociationEnd, CodeElementContainer
    {
        /// <MetaDataID>{ce24ac80-b5a3-4ef8-bab2-5e521169090b}</MetaDataID>
        MetaDataRepository.MetaObjectID CodeElementContainer.Identity
        {
            get
            { 
                return Identity;
            }
        }

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object propertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(propertyValue as string));
            }
            else
            {
                base.PutPropertyValue(propertyNamespace, propertyName, propertyValue);
            }
        }

        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
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



        /// <MetaDataID>{afe96c9c-4469-450d-8147-29a1fb47c529}</MetaDataID>
        public AssociationEnd(string name, OOAdvantech.MetaDataRepository.Classifier specification, OOAdvantech.MetaDataRepository.Roles roleType, string associationIdentity)
            : base(name, specification, roleType)
        {
            AssociationIdentity = associationIdentity;
        }
        /// <MetaDataID>{bd0067ea-8ae8-484b-840c-e252d87be2e8}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MultiplicityRange Multiplicity
        {
            get
            {
                EnvDTE.CodeElements attributes = null;
                if (_Multiplicity == null)
                {
                    if (IDEManager.SynchroForm.InvokeRequired)
                        return IDEManager.SynchroForm.SynchroInvoke(GetType().GetProperty("Multiplicity").GetGetMethod(), this) as OOAdvantech.MetaDataRepository.MultiplicityRange;
                    if (VSMember != null)
                    {

                        if (VSMember is EnvDTE.CodeVariable)
                            attributes = (VSMember as EnvDTE.CodeVariable).Attributes;
                        else
                            attributes = (VSMember as EnvDTE.CodeProperty).Attributes;
                    }
                    else if (GetOtherEnd() != null && GetOtherEnd() is AssociationEnd && (GetOtherEnd() as AssociationEnd).VSMember != null)
                        if ((GetOtherEnd() as AssociationEnd).VSMember is EnvDTE.CodeVariable)
                            attributes = ((GetOtherEnd() as AssociationEnd).VSMember as EnvDTE.CodeVariable).Attributes;
                        else
                            attributes = ((GetOtherEnd() as AssociationEnd).VSMember as EnvDTE.CodeProperty).Attributes;

                    if (attributes != null)
                    {
                        foreach (EnvDTE.CodeAttribute attribute in attributes)
                        {

                            if ((IsRoleA && LanguageParser.IsEqual(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute), attribute))
                                || !IsRoleA && LanguageParser.IsEqual(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute), attribute))
                            {
                                string argumentsString = attribute.Value;
                                if (!string.IsNullOrEmpty(argumentsString))
                                {
                                    System.Collections.Generic.List<string> arguments = new System.Collections.Generic.List<string>();
                                    while (argumentsString.IndexOf(",") != -1)
                                    {
                                        arguments.Add(argumentsString.Substring(0, argumentsString.IndexOf(",")).Trim());
                                        argumentsString = argumentsString.Substring(argumentsString.IndexOf(",") + 1);
                                    }
                                    arguments.Add(argumentsString.Trim());
                                    if (arguments.Count == 1)
                                        _Multiplicity = new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(arguments[0]));
                                    else
                                        _Multiplicity = new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(arguments[0]), ulong.Parse(arguments[1]));
                                }
                                else
                                {
                                    _Multiplicity = new MetaDataRepository.MultiplicityRange();
                                    try
                                    {
                                        if (VSMember is EnvDTE.CodeVariable)
                                            if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeVariable))
                                                _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);
                                        if (VSMember is EnvDTE.CodeProperty)
                                            if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeProperty))
                                                _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);

                                    }
                                    catch (System.Exception error)
                                    {
                                    }
                                }
                                break;
                            }
                            else
                            {
                                _Multiplicity = new MetaDataRepository.MultiplicityRange();
                                try
                                {
                                    if (VSMember is EnvDTE.CodeVariable)
                                        if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeVariable))
                                            _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);
                                    if (VSMember is EnvDTE.CodeProperty)
                                        if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeProperty))
                                            _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);

                                }
                                catch (System.Exception error)
                                {
                                }

                            }

                            if ((GetOtherEnd()!=null && GetOtherEnd().IsRoleA && LanguageParser.IsEqual(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute), attribute))
                              || GetOtherEnd()!=null && !GetOtherEnd().IsRoleA && LanguageParser.IsEqual(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute), attribute))
                            {
                                string argumentsString = attribute.Value;
                                if (!string.IsNullOrEmpty(argumentsString))
                                {
                                    System.Collections.Generic.List<string> arguments = new System.Collections.Generic.List<string>();
                                    while (argumentsString.IndexOf(",") != -1)
                                    {
                                        arguments.Add(argumentsString.Substring(0, argumentsString.IndexOf(",")).Trim());
                                        argumentsString = argumentsString.Substring(argumentsString.IndexOf(",") + 1);
                                    }
                                    arguments.Add(argumentsString.Trim());
                                    if (arguments.Count == 1)
                                       (GetOtherEnd() as AssociationEnd).SetMultiplicity(new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(arguments[0])));
                                    else
                                        (GetOtherEnd() as AssociationEnd).SetMultiplicity(new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(arguments[0]), ulong.Parse(arguments[1])));
                                }
                                else
                                {
                                   (GetOtherEnd() as AssociationEnd).SetMultiplicity(new MetaDataRepository.MultiplicityRange());
                                    try
                                    {
                                        if (VSMember is EnvDTE.CodeVariable)
                                            if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeVariable))
                                                (GetOtherEnd() as AssociationEnd).SetMultiplicity( new MetaDataRepository.MultiplicityRange(0, 1));
                                        if (VSMember is EnvDTE.CodeProperty)
                                            if (!LanguageParser.IsEnumerable(VSMember as EnvDTE.CodeProperty))
                                                (GetOtherEnd() as AssociationEnd).SetMultiplicity( new MetaDataRepository.MultiplicityRange(0, 1));

                                    }
                                    catch (System.Exception error)
                                    {
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                return base.Multiplicity;
            }
        }

        private void SetMultiplicity(MetaDataRepository.MultiplicityRange multiplicityRange)
        {
            _Multiplicity = multiplicityRange;
        }
        /// <MetaDataID>{84436884-2793-45a6-8d75-10e659e8382e}</MetaDataID>
        void SetDocumentation(string documentation)
        {
            if (string.IsNullOrEmpty(documentation) || VSMember == null)
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


        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (_Identity == null)
                {
                    if (IDEManager.SynchroForm.InvokeRequired)
                        return IDEManager.SynchroForm.SynchroInvoke(GetType().GetProperty("Identity").GetGetMethod(), this) as MetaDataRepository.MetaObjectID;



                    if (Association != null)
                    {
                        if (IsRoleA)
                            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Association.Identity + "RoleA");
                        else
                            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Association.Identity + "RoleB");
                    }
                    else if (!string.IsNullOrEmpty(AssociationIdentity))
                    {
                        if (IsRoleA)
                            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(AssociationIdentity + "RoleA");
                        else
                            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(AssociationIdentity + "RoleB");
                    }
                }
                return base.Identity;
            }
        }
        /// <MetaDataID>{d092064d-b997-469c-a78e-32d54c1d851d}</MetaDataID>
        internal AssociationEnd()
        {
        }



        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, originMetaObject);
                return;
            }
            if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                return;

            MetaDataRepository.AssociationEnd originAssociationEnd = originMetaObject as MetaDataRepository.AssociationEnd;


          
            AssociationIdentity = originAssociationEnd.Association.Identity.ToString();
            if (originAssociationEnd.CollectionClassifier != null)
            {
                _CollectionClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.CollectionClassifier, this) as MetaDataRepository.Classifier;
                if (_CollectionClassifier == null)
                    _CollectionClassifier = UnknownClassifier.GetClassifier(originAssociationEnd.CollectionClassifier.FullName, ImplementationUnit);
            }
            else
                _CollectionClassifier = null;

            if (_Specification == null || _Specification.Identity.ToString() != originAssociationEnd.Specification.Identity.ToString())
            {
                _Specification = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as MetaDataRepository.Classifier;
                if (_Specification == null)
                {
                    _Specification = UnknownClassifier.GetClassifier(originAssociationEnd.Specification.FullName, ImplementationUnit);
                    // return;
                }
            }
            _Namespace.Value = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Namespace, this) as MetaDataRepository.Classifier;
            base.Synchronize(originMetaObject);


            string setter = null;
            string getter = null;
            if (_Namespace.Value == null)
                _Namespace.Value = GetOtherEnd().Specification;

            string value = originAssociationEnd.GetPropertyValue(typeof(string), "MetaData", "Synchronize") as string;
            if (value == false.ToString())
                return;


            if ((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") || _Namespace.Value is Interface)
            {
                if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Getter"))
                    getter = _Name;
                if (originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter") != null && (bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "Setter"))
                    setter = _Name;
                if (setter == getter && setter == null)
                    getter = _Name;

            }

            string multiplicityAttribute = null;
            string multiplicityValue = null;
            string otherEndMultiplicityAttribute = null;
            string otherEndMultiplicityValue = null;
            string associationEndBehaviorAttribute = null;
            string associationEndBehaviorValue = null;

            string associationClassAttribute = null;
            string associationClassAttributeValue = null;


            if (_Multiplicity.NoHighLimit)
                multiplicityValue = _Multiplicity.LowLimit.ToString();
            else if (!_Multiplicity.Unspecified)
                multiplicityValue = _Multiplicity.LowLimit.ToString() + ", " + _Multiplicity.HighLimit;
            else if (_Multiplicity.Unspecified)
                multiplicityValue = "0";


            if (!GetOtherEnd().Navigable)
            {
                if (GetOtherEnd().Multiplicity.NoHighLimit)
                    otherEndMultiplicityValue = GetOtherEnd().Multiplicity.LowLimit.ToString();
                else if (!GetOtherEnd().Multiplicity.Unspecified)
                    otherEndMultiplicityValue = GetOtherEnd().Multiplicity.LowLimit.ToString() + ", " + GetOtherEnd().Multiplicity.HighLimit;
                else if (GetOtherEnd().Multiplicity.Unspecified)
                    otherEndMultiplicityValue = "0";
            }


            if (_Namespace.Value is CodeElementContainer && Navigable)
            {
                //EnvDTE.ProjectItem projectItem = (_Namespace.Value as CodeElementContainer).CodeElement.ProjectItem;
                EnvDTE.CodeElement ownerCodeElement = (_Namespace.Value as CodeElementContainer).CodeElement;
                string persistentMemberAttribute = null;
                bool persistentMemberAttributeExist = false;
                if (_Namespace.Value is OOAdvantech.MetaDataRepository.Interface)
                    _Persistent = false;
                if (_Persistent)
                    persistentMemberAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.PersistentMember).FullName, ownerCodeElement);

                _HasBehavioralSettings = true;
                if (!LazyFetching || CascadeDelete || ReferentialIntegrity||AllowTransient)
                {
                    associationEndBehaviorAttribute = LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.AssociationEndBehavior).FullName, ownerCodeElement);
                    associationEndBehaviorValue = null;
                    if (!LazyFetching)
                        associationEndBehaviorValue = LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.OnConstruction.ToString();
                    if (ReferentialIntegrity)
                        if (associationEndBehaviorValue != null)
                            associationEndBehaviorValue += " | " + LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.ReferentialIntegrity.ToString();
                        else
                            associationEndBehaviorValue = LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.ReferentialIntegrity.ToString();

                    if (CascadeDelete)
                        if (associationEndBehaviorValue != null)
                            associationEndBehaviorValue += " | " + LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.CascadeDelete.ToString();
                        else
                            associationEndBehaviorValue = LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.CascadeDelete.ToString();

                    if (AllowTransient)
                        if (associationEndBehaviorValue != null)
                            associationEndBehaviorValue += " | " + LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.AllowTransient.ToString();
                        else
                            associationEndBehaviorValue = LanguageParser.GetShortName(typeof(MetaDataRepository.PersistencyFlag).FullName, ownerCodeElement) + "." + MetaDataRepository.PersistencyFlag.AllowTransient.ToString();

                }
                string associationAttributeValue = null;
                if (_Indexer)
                {
                    if (originAssociationEnd.IsRoleA)
                        associationAttributeValue = "\"" + Association.Name + "\"," +/* typeof(" + LanguageParser.GetShortName(Specification.FullName, ownerCodeElement) + "), " +*/ LanguageParser.GetShortName(typeof(MetaDataRepository.Roles).Name, ownerCodeElement) + "." + MetaDataRepository.Roles.RoleA.ToString() + ", true, \"" + Association.Identity.ToString() + "\"";
                    else
                        associationAttributeValue = "\"" + Association.Name + "\"," +/*, typeof(" + LanguageParser.GetShortName(Specification.FullName, ownerCodeElement) + "), " +*/ LanguageParser.GetShortName(typeof(MetaDataRepository.Roles).Name, ownerCodeElement) + "." + MetaDataRepository.Roles.RoleB.ToString() + ", true, \"" + Association.Identity.ToString() + "\"";
                }
                else
                {
                    if (IsRoleA)
                        associationAttributeValue = "\"" + Association.Name + "\"," +/*, typeof(" + LanguageParser.GetShortName(Specification.FullName, ownerCodeElement) + "), "*/ LanguageParser.GetShortName(typeof(MetaDataRepository.Roles).Name, ownerCodeElement) + "." + MetaDataRepository.Roles.RoleA.ToString() + ", \"" + Association.Identity.ToString() + "\"";
                    else
                        associationAttributeValue = "\"" + Association.Name + "\"," +/*, typeof(" + LanguageParser.GetShortName(Specification.FullName, ownerCodeElement) + "), " +*/ LanguageParser.GetShortName(typeof(MetaDataRepository.Roles).Name, ownerCodeElement) + "." + MetaDataRepository.Roles.RoleB.ToString() + ", \"" + Association.Identity.ToString() + "\"";
                }
                if (Association != null && Association.General != null)
                    associationAttributeValue += ", \"" + Association.General.Identity.ToString() + "\"";

                string associationAttribute = LanguageParser.GetShortName(typeof(OOAdvantech.MetaDataRepository.AssociationAttribute).FullName, ownerCodeElement).Replace("Attribute", "");
                associationAttribute = typeof(OOAdvantech.MetaDataRepository.AssociationAttribute).Name.Replace("Attribute", "");
                if (IsRoleA)
                {
                    multiplicityAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute).FullName, ownerCodeElement).Replace("Attribute", "");
                    otherEndMultiplicityAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute).FullName, ownerCodeElement).Replace("Attribute", "");
                }
                else
                {
                    multiplicityAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute).FullName, ownerCodeElement).Replace("Attribute", "");
                    otherEndMultiplicityAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute).FullName, ownerCodeElement).Replace("Attribute", "");

                }

                if (Association.LinkClass != null)
                {

                    associationClassAttribute = LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, ownerCodeElement).Replace("Attribute", "");
                    associationClassAttributeValue = "typeof(" + LanguageParser.GetShortName(Association.LinkClass.FullName, ownerCodeElement) + ")";
                }
                string typeFullName = Specification.FullName;
                if (_CollectionClassifier != null)
                    typeFullName = _CollectionClassifier.FullName;
                else
                    if (Multiplicity.IsMany)
                        typeFullName = "System.Collections.Generic.List" + "<" + typeFullName + ">";

                foreach (EnvDTE.CodeElement codeMember in CodeClassifier.GetMembers((_Namespace.Value as CodeElementContainer).CodeElement))
                {
                    if (codeMember.Name == _Name &&
                        codeMember is EnvDTE.CodeProperty &&
                        (codeMember as EnvDTE.CodeProperty).Type.AsFullName == typeFullName)
                    {
                        VSMember = codeMember;
                    }
                    if (codeMember.Name == _Name &&
                        codeMember is EnvDTE.CodeVariable &&
                        (codeMember as EnvDTE.CodeVariable).Type.AsFullName == typeFullName)
                    {
                        VSMember = codeMember;
                    }
                    if (VSMember != null)
                    {
                        CodeElementContainer codeElementContainer = MetaObjectMapper.FindMetaObjectFor(VSMember) as CodeElementContainer;
                        if (codeElementContainer == null)
                        {

                            foreach (MetaDataRepository.MetaObject metaObject in ProjectItem.GetProjectItem(VSMember.ProjectItem).MetaObjectImplementations)
                            {
                                if (metaObject is CodeElementContainer && (metaObject as CodeElementContainer).ContainCodeElement(VSMember, null))
                                {
                                    codeElementContainer = metaObject as CodeElementContainer;
                                    break;
                                }
                            }
                        }
                        if (codeElementContainer != null && codeElementContainer != this)
                            codeElementContainer.CodeElementRemoved(VSMember.ProjectItem);
                    }
                }
                if (VSMember == null)
                {
                    if (string.IsNullOrEmpty(_Name))
                        return;
                    try
                    {
                        #region Adds Member

                        if (_Namespace.Value is Class && !((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty")))
                            VSMember = (_Namespace.Value as Class).VSClass.AddVariable(_Name, typeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;

                        if (_Namespace.Value is Class && ((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty")))
                            VSMember = (_Namespace.Value as Class).VSClass.AddProperty(getter, setter, typeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;

                        if (_Namespace.Value is Structure && !((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty")))
                            VSMember = (_Namespace.Value as Structure).VSStruct.AddVariable(_Name, typeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;

                        if (_Namespace.Value is Structure && ((bool)originAssociationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty")))
                            VSMember = (_Namespace.Value as Structure).VSStruct.AddProperty(getter, setter, typeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessPublic, null) as EnvDTE.CodeElement;


                        if (_Namespace.Value is Interface)
                            VSMember = (_Namespace.Value as Interface).VSInterface.AddProperty(getter, setter, typeFullName, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null) as EnvDTE.CodeElement;
                        #endregion

                        System.Threading.Thread.Sleep(50);

                        #region Adds Association attribute

                        int attributePosition = 0;
                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).AddAttribute(associationAttribute, associationAttributeValue, attributePosition++);
                        if (VSMember is EnvDTE.CodeProperty)
                            (VSMember as EnvDTE.CodeProperty).AddAttribute(associationAttribute, associationAttributeValue, attributePosition++);

                        if (associationClassAttribute != null)
                        {
                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(associationClassAttribute, associationClassAttributeValue, attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(associationClassAttribute, associationClassAttributeValue, attributePosition++);
                        }

                        #endregion

                        #region Adds Multiplicity attribute
                        if (_Multiplicity.IsMany && _Multiplicity.NoHighLimit && _Multiplicity.LowLimit == 0)
                            multiplicityAttribute = null;
                        if (_Multiplicity.Unspecified)
                            multiplicityAttribute = null;
                        if (!Multiplicity.IsMany && Multiplicity.LowLimit == 0)//No Multiplicity Constrain
                            multiplicityAttribute = null;

                        if (GetOtherEnd().Multiplicity.IsMany && GetOtherEnd().Multiplicity.NoHighLimit && GetOtherEnd().Multiplicity.LowLimit == 0)
                            otherEndMultiplicityAttribute = null;
                        if (GetOtherEnd().Multiplicity.Unspecified)
                            otherEndMultiplicityAttribute = null;

                        if (multiplicityAttribute != null)
                        {

                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(multiplicityAttribute, multiplicityValue, attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(multiplicityAttribute, multiplicityValue, attributePosition++);
                        }
                        if (otherEndMultiplicityAttribute != null)
                        {
                            if (!GetOtherEnd().Navigable)
                            {
                                if (VSMember is EnvDTE.CodeVariable)
                                    (VSMember as EnvDTE.CodeVariable).AddAttribute(otherEndMultiplicityAttribute, otherEndMultiplicityValue, attributePosition++);
                                if (VSMember is EnvDTE.CodeProperty)
                                    (VSMember as EnvDTE.CodeProperty).AddAttribute(otherEndMultiplicityAttribute, otherEndMultiplicityValue, attributePosition++);
                            }
                        }

                        #endregion

                        #region Adds AssociationEndBehavior attribute
                        if (associationEndBehaviorValue != null)
                        {
                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(associationEndBehaviorAttribute, associationEndBehaviorValue, attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(associationEndBehaviorAttribute, associationEndBehaviorValue, attributePosition++);
                        }
                        #endregion

                        #region Adds persistent member attribute
                        if (!string.IsNullOrEmpty(persistentMemberAttribute))
                        {
                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(persistentMemberAttribute, "", attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(persistentMemberAttribute, "nameof(" + GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")", attributePosition++);
                        }

                        #endregion
                    }
                    catch (System.Exception error)
                    {
                        if (MetaDataRepository.SynchronizerSession.ErrorLog != null)
                            MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(_Namespace.Value.FullName + " :    " + error.Message);
                        return;

                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(_Name))
                    {
                        if (MetaDataRepository.SynchronizerSession.ErrorLog != null)
                            MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(_Namespace.Value.FullName + " :    System can't change name of " + VSMember.Name + " to nothing.");
                        return;
                    }
                    //string typeFullName = Specification.FullName;
                    //if (_CollectionClassifier != null)
                    //    typeFullName = _CollectionClassifier.FullName;
                    //else
                    //    if (Multiplicity.IsMany)
                    //        typeFullName = "System.Collections.Generic.List" + "<" + typeFullName + ">";


                    EnvDTE.CodeTypeRef typeRef = LanguageParser.CreateCodeTypeRef(typeFullName, VSMember.ProjectItem.ContainingProject.CodeModel);
                    //if (_CollectionClassifier != null)
                    //    typeRef = LanguageParser.CreateCodeTypeRef(_CollectionClassifier, VSMember.ProjectItem.ContainingProject.CodeModel);
                    if (VSMember is EnvDTE.CodeVariable)
                    {
                        if (CodeClassifier.HasTypeChanged((VSMember as EnvDTE.CodeVariable).Type, typeRef))
                            (VSMember as EnvDTE.CodeVariable).Type = typeRef;
                    }
                    else
                    {
                        if (CodeClassifier.HasTypeChanged((VSMember as EnvDTE.CodeProperty).Type, typeRef))
                            (VSMember as EnvDTE.CodeProperty).Type = typeRef;

                    }
                    if (VSMember.Name != _Name)
                        VSMember.Name = _Name;

                    EnvDTE.CodeElements attributes = null;
                    if (VSMember is EnvDTE.CodeVariable)
                        attributes = (VSMember as EnvDTE.CodeVariable).Attributes;
                    else
                        attributes = (VSMember as EnvDTE.CodeProperty).Attributes;

                    bool multiplicityAttributeExist = false;
                    bool otherEndmultiplicityAttributeExist = false;
                    bool associacionEndBehaviorAttributeExist = false;
                    bool associacionClassAttributeExist = false;

                    foreach (EnvDTE.CodeAttribute codeAttribute in attributes)
                    {
                        if (codeAttribute.FullName == typeof(MetaDataRepository.AssociationAttribute).FullName)
                            CodeClassifier.UpdateAttributeValue(codeAttribute, associationAttributeValue);

                        #region Update multiplicity attributes
                        if (codeAttribute.FullName == typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute).FullName && IsRoleA)
                        {
                            CodeClassifier.UpdateAttributeValue(codeAttribute, multiplicityValue);
                            multiplicityAttributeExist = true;
                        }
                        if (codeAttribute.FullName == typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute).FullName && !IsRoleA)
                        {
                            CodeClassifier.UpdateAttributeValue(codeAttribute, multiplicityValue);
                            multiplicityAttributeExist = true;
                        }


                        if (codeAttribute.FullName == typeof(MetaDataRepository.AssociationClass).FullName)
                        {
                            CodeClassifier.UpdateAttributeValue(codeAttribute, associationClassAttributeValue);
                            associacionClassAttributeExist = true;
                        }



                        if (!GetOtherEnd().Navigable)
                        {
                            if (codeAttribute.FullName == typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute).FullName && !IsRoleA)
                            {
                                CodeClassifier.UpdateAttributeValue(codeAttribute, otherEndMultiplicityValue);
                                otherEndmultiplicityAttributeExist = true;
                            }
                            if (codeAttribute.FullName == typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute).FullName && IsRoleA)
                            {
                                CodeClassifier.UpdateAttributeValue(codeAttribute, otherEndMultiplicityValue);
                                otherEndmultiplicityAttributeExist = true;
                            }
                        }
                        if (LanguageParser.IsEqual(typeof(MetaDataRepository.AssociationEndBehavior), codeAttribute))
                        {
                            associacionEndBehaviorAttributeExist = true;
                            if (!string.IsNullOrEmpty(associationAttributeValue))
                                CodeClassifier.UpdateAttributeValue(codeAttribute, associationEndBehaviorValue);
                        }

                        if (LanguageParser.IsEqual(typeof(MetaDataRepository.PersistentMember), codeAttribute))
                        {
                            persistentMemberAttributeExist = true;
                            if (!string.IsNullOrEmpty(persistentMemberAttribute))
                                CodeClassifier.UpdateAttributeValue(codeAttribute, "nameof(" + GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")");
                        }





                        #endregion
                    }

                    #region Adds Multiplicity attribute if doesn't exist
                    if (Multiplicity.IsMany && CollectionClassifier != null && Multiplicity.LowLimit == 0 && Multiplicity.NoHighLimit)
                        multiplicityAttributeExist = true;
                    if (!Multiplicity.IsMany && Multiplicity.LowLimit == 0)//No Multiplicity Constrain
                        multiplicityAttributeExist = true;
                    int attributePosition = 0;
                    if (!multiplicityAttributeExist)
                    {
                        if (!Multiplicity.Unspecified)
                        {

                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(multiplicityAttribute, multiplicityValue, attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(multiplicityAttribute, multiplicityValue, attributePosition++);
                        }
                    }
                    if (!GetOtherEnd().Navigable && !otherEndmultiplicityAttributeExist)
                    {

                        if (!GetOtherEnd().Multiplicity.Unspecified)
                        {
                            if (VSMember is EnvDTE.CodeVariable)
                                (VSMember as EnvDTE.CodeVariable).AddAttribute(otherEndMultiplicityAttribute, otherEndMultiplicityValue, attributePosition++);
                            if (VSMember is EnvDTE.CodeProperty)
                                (VSMember as EnvDTE.CodeProperty).AddAttribute(otherEndMultiplicityAttribute, otherEndMultiplicityValue, attributePosition++);
                        }
                    }
                    #endregion

                    #region Adds AssociationEndBehavior attribute

                    if (!associacionClassAttributeExist && associationClassAttribute != null)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).AddAttribute(associationClassAttribute, associationClassAttributeValue, attributePosition++);
                        if (VSMember is EnvDTE.CodeProperty)
                            (VSMember as EnvDTE.CodeProperty).AddAttribute(associationClassAttribute, associationClassAttributeValue, attributePosition++);
                    }

                    if (associationEndBehaviorValue != null && !associacionEndBehaviorAttributeExist)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).AddAttribute(associationEndBehaviorAttribute, associationEndBehaviorValue, attributePosition++);
                        if (VSMember is EnvDTE.CodeProperty)
                            (VSMember as EnvDTE.CodeProperty).AddAttribute(associationEndBehaviorAttribute, associationEndBehaviorValue, attributePosition++);
                    }
                    #endregion

                    #region Adds persistent member attribute
                    if (!string.IsNullOrEmpty(persistentMemberAttribute) && !persistentMemberAttributeExist)
                    {
                        if (VSMember is EnvDTE.CodeVariable)
                            (VSMember as EnvDTE.CodeVariable).AddAttribute(persistentMemberAttribute, "", attributePosition++);
                        if (VSMember is EnvDTE.CodeProperty)
                            (VSMember as EnvDTE.CodeProperty).AddAttribute(persistentMemberAttribute, "nameof(" + GetPropertyValue(typeof(string), "MetaData", "ImplementationMember") as string + ")", attributePosition++);
                    }
                    #endregion

                    if (VSMember is EnvDTE.CodeProperty && (VSMember as EnvDTE.CodeProperty).Setter == null && setter != null)
                        LanguageParser.AddSetter(VSMember as EnvDTE.CodeProperty, (_Namespace.Value as CodeElementContainer).CodeElement);

                    if (VSMember is EnvDTE.CodeProperty && (VSMember as EnvDTE.CodeProperty).Getter == null && getter != null)
                        LanguageParser.AddGetter(VSMember as EnvDTE.CodeProperty, (_Namespace.Value as CodeElementContainer).CodeElement);

                }

            }
            SetDocumentation(GetPropertyValue(typeof(string), "MetaData", "Documentation") as string);
            MetaObjectChangeState();
            //base.Synchronize(OriginMetaObject);
        }
        /// <MetaDataID>{7988A454-6749-4A4F-BA94-E972EF66A75D}</MetaDataID>
        internal EnvDTE.CodeElement VSMember;
        /// <MetaDataID>{f6698c7b-dd7c-411e-94ea-8b921af1e92c}</MetaDataID>
        public string AssociationIdentity;
        /// <MetaDataID>{d00b5130-7b3d-4c39-a151-5d52118cdd5f}</MetaDataID>
        public string AssociationName;
        /// <MetaDataID>{5007b855-cff0-4645-947d-8fad9ebf0bf0}</MetaDataID>
        public MetaDataRepository.Association GeneralAssociation;
        /// <MetaDataID>{617b7abb-ee28-4615-a03a-a88c9fcee1b7}</MetaDataID>
        public MetaDataRepository.Classifier AssociationClass;

        /// <MetaDataID>{af14321c-24c3-49b3-9e40-92c3b7d7a328}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Getter
        {
            get
            {

                if (!(VSMember is EnvDTE.CodeProperty))
                    return base.Getter;

                if (_Getter == null)
                {
                    if (IDEManager.SynchroForm.InvokeRequired)
                        return IDEManager.SynchroForm.SynchroInvoke(GetType().GetProperty("Getter").GetGetMethod(), this) as MetaDataRepository.Operation;
                }
                if (_Getter == null && (VSMember as EnvDTE.CodeProperty).Getter != null)
                {
                    _Getter = new Operation((VSMember as EnvDTE.CodeProperty).Getter, _Namespace.Value as OOAdvantech.MetaDataRepository.Classifier);
                    _Getter.Name = "get_" + _Getter.Name;
                }
                return _Getter;

            }
        }
        /// <MetaDataID>{ad09264d-43e8-4224-a903-056d3eb863bb}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Setter
        {
            get
            {
                if (!(VSMember is EnvDTE.CodeProperty))
                    return base.Setter;

                if (_Setter == null)
                {
                    if (IDEManager.SynchroForm.InvokeRequired)
                        return IDEManager.SynchroForm.SynchroInvoke(GetType().GetProperty("Setter").GetGetMethod(), this) as MetaDataRepository.Operation;
                }
                if (_Setter == null && (VSMember as EnvDTE.CodeProperty).Setter != null)
                {
                    _Setter = new Operation((VSMember as EnvDTE.CodeProperty).Setter, _Namespace.Value as OOAdvantech.MetaDataRepository.Classifier);
                    _Setter.Name = "set_" + _Setter.Name;
                    _Setter.AddParameter("value", Specification, "");
                }
                return _Setter;


            }
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

        /// <MetaDataID>{a514b7cf-d806-428c-8d54-ff6cc0d3852e}</MetaDataID>
        public AssociationEnd(EnvDTE.CodeVariable vsMember,
            MetaDataRepository.Classifier owner,
            MetaDataRepository.Classifier specification,
            MetaDataRepository.Roles associationEndRole,
            string associationIdentity,
            string associationName,
            MetaDataRepository.Classifier associationClass,
            MetaDataRepository.Association generalAssociation,
            bool indexer)
            : base(vsMember.Name, specification, associationEndRole)
        {
            _Indexer = indexer;
            GeneralAssociation = generalAssociation;

            //Role = associationEndRole;
            AssociationClass = associationClass;

            AssociationIdentity = associationIdentity;
            AssociationName = associationName;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(vsMember as EnvDTE.CodeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            // _Owner = owner;
            _Namespace.Value = owner;
            VSMember = vsMember as EnvDTE.CodeElement;
            _Kind = VSMember.Kind;
            Visibility = VSAccessTypeConverter.GetVisibilityKind(vsMember.Access);
            Navigable = true;
            if (specification.FullName != vsMember.Type.AsFullName)
                _CollectionClassifier = (owner.ImplementationUnit as Project).GetClassifier(vsMember.Type);

            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
            MetaObjectMapper.AddTypeMap(VSMember, this);
            RefreshStartPoint();



        }
        /// <MetaDataID>{609c599c-9c24-4106-92cd-71ce010f480b}</MetaDataID>
        public AssociationEnd(
            EnvDTE.CodeProperty vsMember,
            MetaDataRepository.Classifier owner,
            MetaDataRepository.Classifier specification,
            MetaDataRepository.Roles associationEndRole,
            string associationIdentity,
            string associationName,
            MetaDataRepository.Classifier associationClass,
            MetaDataRepository.Association generalAssociation,
            bool indexer)
            : base(vsMember.Name, specification, associationEndRole)
        {

            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(vsMember as EnvDTE.CodeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            _Indexer = indexer;
            AssociationClass = associationClass;
            AssociationName = associationName;
            AssociationIdentity = associationIdentity;
            GeneralAssociation = generalAssociation;
            //  _Owner = owner;
            _Namespace.Value = owner;
            VSMember = vsMember as EnvDTE.CodeElement;
            _Kind = VSMember.Kind;
            Navigable = true;
            Visibility = VSAccessTypeConverter.GetVisibilityKind(vsMember.Access);
            if (specification.FullName != vsMember.Type.AsFullName)
                _CollectionClassifier = (owner.ImplementationUnit as Project).GetClassifier(vsMember.Type);

            _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);

            MetaObjectMapper.RemoveMetaObject(this);
            //if (!(MetaObjectMapper.FindMetaObjectFor(VSMember) is AssociationEnd))
            //    MetaObjectMapper.RemoveType(VSMember);

            MetaObjectMapper.AddTypeMap(VSMember, this);
            RefreshStartPoint();
            //if(vsMember.Type.AsFullName!=specification.FullName)
        }


        /// <MetaDataID>{a7afba03-8b26-40cb-86cc-4764ba00236a}</MetaDataID>
        EnvDTE.vsCMElement _Kind;
        public EnvDTE.vsCMElement Kind
        {
            get
            {
                return _Kind;
            }
        }
        /// <MetaDataID>{61ce4bf5-70c7-4c23-8254-718945b093d3}</MetaDataID>
        public void CodeElementRemoved(EnvDTE.ProjectItem projectItem)
        {
            if (_ProjectItem != null)
                _ProjectItem.RemoveMetaObject(this);

            if (Association == null)
            {
                SetSpecification(null);
                MetaObjectMapper.RemoveMetaObject(this);
            }
            else
            {
                (Specification as MetaDataRepository.Classifier).RemoveAssociation(Association);
                (GetOtherEnd().Specification as MetaDataRepository.Classifier).RemoveAssociation(Association);
                GetOtherEnd().Navigable = false;
                GetOtherEnd().Name = "";
                MetaObjectMapper.RemoveMetaObject(this);
                if (GetOtherEnd() is AssociationEnd)
                {
                    MetaObjectMapper.RemoveMetaObject(GetOtherEnd());
                    if ((GetOtherEnd() as AssociationEnd).ProjectItem != null)
                        (GetOtherEnd() as AssociationEnd).ProjectItem.RemoveMetaObject(GetOtherEnd());

                }
                if (Association != null)
                    MetaObjectMapper.RemoveMetaObject(Association);
            }

            VSMember = null;
        }
        /// <MetaDataID>{808941a8-e50e-46ca-a63d-a0752ed22dcd}</MetaDataID>
        public void CodeElementRemoved()
        {
            CodeElementRemoved(null);
        }

        public EnvDTE.CodeElement CodeElement
        {
            get
            {
                return VSMember;
            }
        }
        public bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent)
        {
            if (codeElement == null)
                return false;

            if (codeElement.Kind != _Kind)
                return false;
            if (this.VSMember == codeElement)
                return true;
            if (_ProjectItem != ProjectItem.GetProjectItem(codeElement.ProjectItem))
                return false;


            if (!(codeElement is EnvDTE.CodeProperty) && !(codeElement is EnvDTE.CodeVariable))
                return false;
            EnvDTE.CodeAttribute associationAttribute = CodeClassifier.GetAssociationAttribute(codeElement);
            if (associationAttribute != null)
            {
                MetaDataRepository.Roles associationEndRole;
                string identity = null;
                string associationName = null;
                LanguageParser.GetAssociationData(associationAttribute, out associationName, out associationEndRole, out identity);
                if (Association == null)
                    return false;
                if (identity == null || identity.ToLower() != Association.Identity.ToString().ToLower())
                    return false;
                if (Role != associationEndRole)
                    return false;
            }
            else
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
                if (itsParent is EnvDTE.CodeElement)
                    if ((itsParent as EnvDTE.CodeElement).FullName + "." + codeElement.Name == CurrentProgramLanguageFullName)
                    {
                        if (vsMemberParent == codeElementParent && vsMemberParent == null)
                            return true;
                    }
                if (codeElement.FullName == FullName)
                    return true;
            }
            catch (System.Exception error)
            {
            }
            return false;

        }

        public void RefreshCodeElement(EnvDTE.CodeElement codeElement)
        {
            if (codeElement == null)
                return;
            bool changed = false;
            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            if (VSMember == codeElement)
            {
                if (_Name != VSMember.Name)
                {
                    _Name = VSMember.Name;
                    changed = true;
                }
                if (codeElement is EnvDTE.CodeProperty)
                {
                    if (Visibility != VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeProperty).Access))
                    {
                        Visibility = VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeProperty).Access);
                        changed = true;
                    }
                }
                if (codeElement is EnvDTE.CodeVariable)
                {
                    if (Visibility != VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeVariable).Access))
                    {
                        Visibility = VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeVariable).Access);
                        changed = true;

                    }
                }
                Navigable = true;
                ReAssigneRole();
                if (Association != null && GetOtherEnd() is AssociationEnd)
                    (GetOtherEnd() as AssociationEnd).ReAssigneRole();
                if (changed)
                    MetaObjectChangeState();
                EnvDTE.CodeTypeRef type = null;
                if (VSMember is EnvDTE.CodeVariable)
                    type = (VSMember as EnvDTE.CodeVariable).Type;
                if (VSMember is EnvDTE.CodeProperty)
                    type = (VSMember as EnvDTE.CodeProperty).Type;

                if (type != null && Specification.FullName != type.AsFullName)
                {
                    if (_CollectionClassifier == null || LanguageParser.GetTypeFullName(_CollectionClassifier, codeElement.ProjectItem.ContainingProject) != type.AsFullName)
                        _CollectionClassifier = (Namespace.ImplementationUnit as Project).GetClassifier(type);
                }


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
                    Indexer = indexer;
                }
                RefreshStartPoint();
            }
            else
            {
                VSMember = codeElement;
                _Name = VSMember.Name;
                if (codeElement is EnvDTE.CodeProperty)
                    Visibility = VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeProperty).Access);
                if (codeElement is EnvDTE.CodeVariable)
                    Visibility = VSAccessTypeConverter.GetVisibilityKind((codeElement as EnvDTE.CodeVariable).Access);

                Navigable = true;
                ReAssigneRole();
                if (Association != null && GetOtherEnd() != null)
                    (GetOtherEnd() as AssociationEnd).ReAssigneRole();
                MetaObjectChangeState();
                RefreshStartPoint();
            }
        }



        public void RefreshStartPoint()
        {
            try
            {
                _LineCharOffset = VSMember.StartPoint.LineCharOffset;
                if (_Line != VSMember.StartPoint.Line)
                {
                    _Line = VSMember.StartPoint.Line;
                    // MetaObjectChangeState();
                }
            }
            catch (System.Exception error)
            {
            }
        }


        /// <MetaDataID>{5a305577-c872-4ffe-85fc-4dc62d2a12c8}</MetaDataID>
        public int _Line = 0;
        /// <MetaDataID>{187f5f17-83d8-4880-8571-5014d3526881}</MetaDataID>
        public int _LineCharOffset = 0;
        /// <MetaDataID>{7f9fb456-f5ce-4ee1-a896-67d5e5443f58}</MetaDataID>
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
        /// <MetaDataID>{9ba73199-803b-4257-9b7d-6bb4c92da6a9}</MetaDataID>
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

        //string AssociationAtributeDefenition;
        /// <MetaDataID>{7c9748bd-8586-4fb2-93fe-b614a2a33538}</MetaDataID>
        internal void SetVSMember(EnvDTE.CodeElement codeElement, MetaDataRepository.Classifier owner)
        {
            bool changed = false;
            bool parentChanged = false;
            if (VSMember == null)
                parentChanged = true;


            string identity;
            string comments;
            CodeClassifier.LoadDocDocumentItems(codeElement, out identity, out comments);
            if (comments != null)
                PutPropertyValue("MetaData", "Documentation", comments);

            VSMember = codeElement;
            if (_Name != VSMember.Name)
            {
                _Name = VSMember.Name;
                changed = true;
            }
            EnvDTE.CodeAttribute associationAttribute = null;
            EnvDTE.CodeAttribute associationClassAttribute = null;

            CodeClassifier.GetAssociationAttributes(codeElement, out associationAttribute, out associationClassAttribute);
            string associationName = null;
            MetaDataRepository.Classifier otherEndType = null;
            MetaDataRepository.Roles associationEndRole;
            string generalAssociationIdentity = null;
            LanguageParser.GetAssociationData(associationAttribute, out associationName, out otherEndType, out associationEndRole, out AssociationIdentity, out _Indexer, out generalAssociationIdentity);
            MetaDataRepository.Classifier generalAssociationHostType = null;
            foreach (MetaDataRepository.Generalization generalization in owner.Generalizations)
            {
                generalAssociationHostType = generalization.Parent;
                break;

            }
            if (generalAssociationHostType != null && generalAssociationIdentity != null)
            {
                foreach (MetaDataRepository.AssociationEnd generalAssociationEnd in generalAssociationHostType.GetRoles(true))
                {
                    if (generalAssociationEnd.Association.Identity.ToString().ToLower() == generalAssociationIdentity.ToLower())
                    {
                        GeneralAssociation = generalAssociationEnd.Association;
                        break;
                    }
                }
            }

            if (Association != null)
                Association.General = GeneralAssociation;

            if (Association != null && !string.IsNullOrEmpty(associationName))
                Association.Name = associationName;


            _Kind = VSMember.Kind;
            if (!Navigable)
                changed = true;
            Navigable = true;
            _Namespace.Value = owner;

            #region Sets CollectionClassifier
            _CollectionClassifier = null;
            if (VSMember is EnvDTE.CodeProperty)
            {
                Visibility = VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeProperty).Access);
                if (Specification.FullName != (VSMember as EnvDTE.CodeProperty).Type.AsFullName && Namespace is MetaDataRepository.Classifier)
                {
                    if (_CollectionClassifier == null || LanguageParser.GetTypeFullName(_CollectionClassifier, (ImplementationUnit as Project).VSProject) != (VSMember as EnvDTE.CodeProperty).Type.AsFullName)
                    {
                        MetaDataRepository.Classifier classifier = (owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeProperty).Type);
                        if (classifier != _CollectionClassifier)
                        {
                            _CollectionClassifier = classifier;
                            changed = true;
                        }
                    }
                }

            }
            if (VSMember is EnvDTE.CodeVariable)
            {
                Visibility = VSAccessTypeConverter.GetVisibilityKind((VSMember as EnvDTE.CodeVariable).Access);
                if (Specification.FullName != (VSMember as EnvDTE.CodeVariable).Type.AsFullName)
                {
                    if (_CollectionClassifier == null || LanguageParser.GetTypeFullName(_CollectionClassifier, (ImplementationUnit as Project).VSProject) != (VSMember as EnvDTE.CodeVariable).Type.AsFullName)
                    {
                        MetaDataRepository.Classifier classifier = (owner.ImplementationUnit as Project).GetClassifier((VSMember as EnvDTE.CodeVariable).Type);
                        if (_CollectionClassifier != classifier)
                        {
                            _CollectionClassifier = classifier;
                            changed = true;
                        }
                    }
                }
            }
            #endregion

            try
            {
                _ProjectItem = ProjectItem.AddMetaObject(VSMember.ProjectItem, this);
            }
            catch (System.Exception error)
            {
            }
            RefreshStartPoint();

            if (parentChanged)
                Namespace.MetaObjectChangeState();
            if (changed)
                MetaObjectChangeState();


        }
    }
}
