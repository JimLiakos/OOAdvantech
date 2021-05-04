namespace RoseMetaDataRepository
{
    /// <MetaDataID>{6C4F7DDB-B528-499B-A394-8DA97A7CAA5A}</MetaDataID>
    internal class Class : OOAdvantech.MetaDataRepository.Class
    {

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                if (base.GetPropertyValue(typeof(string), propertyNamespace, propertyName) != null && GetPropertyValue(typeof(string), propertyNamespace, propertyName) == PropertyValue)
                    return;
                if (PropertyValue == null)
                    PropertyValue = "";
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string);
                if (RoseClass != null)
                    RoseClass.OverrideProperty("MetaData", "MetaObjectID", PropertyValue as string);
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
                if (identity == null && _Identity != null)
                {
                    identity = _Identity.ToString();
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }

        /// <MetaDataID>{1b0f462b-542b-4b37-b0dd-47af31b077a3}</MetaDataID>
        bool LinkAssociationLoaded = false;
        /// <MetaDataID>{e9823ff7-0862-482d-9e3a-3252bf1b6815}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Association LinkAssociation
        {
            get
            {
                if (!LinkAssociationLoaded)
                {
                    if (RoseClass.IsALinkClass() && _LinkAssociation == null)
                    {
                        RationalRose.RoseClass assosiactionRoseEndClass = null;
                        if (RoseClass.GetLinkAssociation().Role1.Navigable)
                            assosiactionRoseEndClass = RoseClass.GetLinkAssociation().Role1.GetSupplier() as RationalRose.RoseClass;
                        else
                            assosiactionRoseEndClass = RoseClass.GetLinkAssociation().Role2.GetSupplier() as RationalRose.RoseClass;
                        OOAdvantech.MetaDataRepository.Classifier assosiactionEndClass = MetaObjectMapper.FindMetaObjectFor(assosiactionRoseEndClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                        Component component = null;
                        if (assosiactionRoseEndClass.GetAssignedModules().Count > 0)
                        {
                            RationalRose.RoseModule roseModule = assosiactionRoseEndClass.GetAssignedModules().GetAt(1);
                            component = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseModule);

                        }
                        if (assosiactionEndClass == null)
                        {
                            if (assosiactionRoseEndClass.Stereotype == "Interface")
                                assosiactionEndClass = new Interface(assosiactionRoseEndClass, null);
                            else
                                assosiactionEndClass = new Class(assosiactionRoseEndClass, null);
                        }
                        foreach (AssociationEnd role in assosiactionEndClass.Roles)
                        {
                            if ((role.Association as Association).RoseAssociation.GetUniqueID() == RoseClass.GetLinkAssociation().GetUniqueID())
                            {
                                _LinkAssociation = role.Association;
                                _LinkAssociation.LinkClass = this;
                            }
                        }


                    }
                    LinkAssociationLoaded = true;
                }
                return base.LinkAssociation;
            }
            set
            {
                base.LinkAssociation = value;
            }
        }


        /// <MetaDataID>{261b643f-92f4-487f-8dea-74636b82dc69}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{1357403c-e24c-4bef-b8ee-63f5b6384a21}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd> Roles
        {

            get
            {
                if (!IsRolesLoaded)
                {
                    if (RoseClass == null)
                        return base.Roles;
                    for (int i = 0; i < RoseClass.GetRoles().Count; i++)
                    {
                        long count = RoseClass.GetRoles().Count;
                        RationalRose.RoseRole roseRole = RoseClass.GetRoles().GetAt((short)(i + 1));

                        
                        RationalRose.RoseRole otherEndRoseRole = null;
                        if (roseRole.Association.Role1.Equals(roseRole))
                            otherEndRoseRole = roseRole.Association.Role2;
                        else
                            otherEndRoseRole = roseRole.Association.Role1;
                        
                        if (otherEndRoseRole.Class != null)
                        {
                            bool exist=false;
                            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in _Roles)
                            {
                                if (associationEnd is AssociationEnd && (associationEnd as AssociationEnd).RoseRole.Equals(roseRole))
                                {
                                    exist = true;
                                    break;
                                }
                            }
                            if (!exist)
                                _Roles.Add(new AssociationEnd(roseRole, this));
                            else
                            {
                                int tyrt = 0;
                            }
                        }


                        

                    }
                    IsRolesLoaded = true;
                }
                //foreach (AssociationEnd associationEnd in _Roles)
                //{
                //    OOAdvantech.MetaDataRepository.Classifier classif = associationEnd.GetOtherEnd().Specification;
                //    OOAdvantech.MetaDataRepository.Namespace classife = associationEnd.GetOtherEnd().Namespace;


                //}
                return base.Roles;
                
            }
        }
        /// <MetaDataID>{1e6adf91-f61a-4925-b603-a06d5e2b467a}</MetaDataID>
        bool GeneralizationsLoaded = false;
        /// <MetaDataID>{20a4e8af-f28f-44c5-a4be-61c70e35c51f}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                if (!GeneralizationsLoaded)
                {
                    if (RoseClass == null)
                        return base.Generalizations;
                    _Generalizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>();
                    RationalRose.RoseClassCollection superClasses = RoseClass.GetSuperclasses();
                    for (int i = 0; i < superClasses.Count; i++)
                    {
                        RationalRose.RoseClass roseSuperClass = superClasses.GetAt((short)(i + 1));
                        Class superClass = MetaObjectMapper.FindMetaObjectFor(roseSuperClass.GetUniqueID()) as Class;
                        Component component=null;
                        if(roseSuperClass.GetAssignedModules().Count>0)
                        {
                            RationalRose.RoseModule roseModule=roseSuperClass.GetAssignedModules().GetAt(1);
                            component = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                            if(component==null)
                                component=new Component(roseModule);

                        }
                        if (roseSuperClass.Stereotype == "Interface")
                            continue;
                        if (superClass == null)
                            superClass = new Class(roseSuperClass, component);
                        _Generalizations.Add(new Generalization("",superClass,this));
                    }
                    GeneralizationsLoaded = true;
                }
                return base.Generalizations;
            }
        }
        /// <MetaDataID>{31f4c138-c769-463f-808b-64ca049413f7}</MetaDataID>
        bool RealizationLoaded = false;
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization> Realizations
        {
            get
            {

                if (!RealizationLoaded)
                {
                    if (RoseClass == null)
                        return base.Realizations;
                    _Realizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>();
                    RationalRose.RoseRealizeRelationCollection realizeRelations = RoseClass.GetRealizeRelations();
                    for (int i = 0; i < realizeRelations.Count; i++)
                    {
                        RationalRose.RoseClass roseSuperClass = realizeRelations.GetAt((short)(i + 1)).GetSupplierClass();
                        if (roseSuperClass == null)
                            continue;
                        Interface _interface = MetaObjectMapper.FindMetaObjectFor(roseSuperClass.GetUniqueID()) as Interface;
                        Component component = null;
                        if (roseSuperClass.GetAssignedModules().Count > 0)
                        {
                            RationalRose.RoseModule roseModule = roseSuperClass.GetAssignedModules().GetAt(1);
                            component = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseModule);

                        }
                        if (_interface == null)
                            _interface = new Interface(roseSuperClass, component);
                        _Realizations.Add(new Realization("", _interface, this));
                    }
                    RealizationLoaded = true;
                }
                return base.Realizations;

            }
        }
        bool onFeaturesLoad; 
        /// <MetaDataID>{d0029a94-2097-4010-b642-ff83b8bd2814}</MetaDataID>
        bool IsFeaturesLoaded = false;
        /// <MetaDataID>{59d04933-efe5-4d9a-8346-6d37b218509f}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> Features
        {
            get
            {
                if (!IsFeaturesLoaded && !onFeaturesLoad)
                {
                    onFeaturesLoad = true;
                    try
                    {
                        if (RoseClass == null)
                            return base.Features;
                        for (int i = 0; i < RoseClass.Operations.Count; i++)
                        {
                            RationalRose.RoseOperation roseOperation = RoseClass.Operations.GetAt((short)(i + 1));
                            OOAdvantech.MetaDataRepository.Operation operation = GetOperationForMethod(roseOperation);
                            if (operation == null)
                                continue;
                            if (operation.Owner == this)
                                _Features.Add(operation);
                            else
                                _Features.Add(new Method(operation, roseOperation, this));
                        }

                        for (int i = 0; i < RoseClass.Attributes.Count; i++)
                        {
                            RationalRose.RoseAttribute roseAttribute = RoseClass.Attributes.GetAt((short)(i + 1));
                            if (roseAttribute.GetPropertyValue("Persistent", "Persistent") == "True"||
                                roseAttribute.GetPropertyValue("C#", "Generate Backward ID") == "True")
                            {
                                if (string.IsNullOrEmpty(roseAttribute.GetPropertyValue("C#", "Identity")))
                                {
                                    string nextMemberIdentityString = RoseClass.GetPropertyValue("MetaData", "LastMemberID");
                                    if (string.IsNullOrEmpty(nextMemberIdentityString))
                                    {
                                        RoseClass.OverrideProperty("MetaData", "LastMemberID", ((int)1).ToString());
                                        nextMemberIdentityString = "1";
                                    }
                                    roseAttribute.OverrideProperty("C#", "Identity","+"+ nextMemberIdentityString);
                                    int next= int.Parse(nextMemberIdentityString);
                                    next++;
                                    RoseClass.OverrideProperty("MetaData", "LastMemberID", next.ToString());

                                }

                            }
                          //  if (roseAttribute.GetPropertyValue("C#", "As Property") == "True")
                            {
                                OOAdvantech.MetaDataRepository.AssociationEnd associationEnd=GetAssociationEndFor(roseAttribute);
                                if (associationEnd != null)
                                {
                                    _Features.Add(new AssociationEndRealization(roseAttribute, this, associationEnd));
                                    continue;
                                 
                                }
                                OOAdvantech.MetaDataRepository.Attribute attribute = GetAttributeFor(roseAttribute);
                                if (attribute.Owner != this)
                                    _Features.Add(new AttributeRealization(roseAttribute, this, attribute));
                                else
                                    _Features.Add(attribute);
                                
                            }
                            //else
                            //    _Features.Add(new Attribute(roseAttribute,this));

                        }

                        IsFeaturesLoaded = true;
                    }
                    finally
                    {
                        onFeaturesLoad = false;
                        if (!IsFeaturesLoaded)
                            _Features.RemoveAll();

                    }

                }
                return base.Features;
            }
        }

        /// <MetaDataID>{a2baba99-03fd-465e-9d64-aca241656a36}</MetaDataID>
        private OOAdvantech.MetaDataRepository.Attribute GetAttributeFor(RationalRose.RoseAttribute roseAttribute)
        {
            string roseAttributeName = roseAttribute.Name;
            foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
                {
                    
                    OOAdvantech.MetaDataRepository.Attribute attribute = feature as OOAdvantech.MetaDataRepository.Attribute;
                    if (attribute != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") && attribute.Name == roseAttribute.Name && MetaObjectMapper.GetShortNameFor(attribute.Type.FullName) == roseAttribute.Type)
                        return attribute;
                }

            }


            foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
                {
                    OOAdvantech.MetaDataRepository.Attribute attribute = feature as OOAdvantech.MetaDataRepository.Attribute;
                    if (attribute != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") && attribute.Name == roseAttribute.Name && MetaObjectMapper.GetShortNameFor(attribute.Type.FullName) == roseAttribute.Type)
                        return attribute;
                }
            }
            return new Attribute(roseAttribute, this);
        }

        /// <MetaDataID>{4d94ca3c-8e92-429e-8df3-494f982f636e}</MetaDataID>
        private OOAdvantech.MetaDataRepository.AssociationEnd GetAssociationEndFor(RationalRose.RoseAttribute roseAttribute)
        {

            foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            {
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(false))
                {

                    string typeFullName = associationEnd.Specification.FullName;
                    if (associationEnd.CollectionClassifier != null)
                        typeFullName = associationEnd.CollectionClassifier.FullName;

                    string roseAttributeTypeFullName = roseAttribute.Type;
                    RationalRose.RoseClass typeClass=roseAttribute.GetTypeClass();
                    if (typeClass != null)
                    {
                        OOAdvantech.MetaDataRepository.Classifier typeClassifier = MetaObjectMapper.FindMetaObjectFor(typeClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                        if (typeClassifier != null)
                            roseAttributeTypeFullName = typeClassifier.FullName;
                    }
                    string tt = roseAttribute.Name;
                    if (((bool)associationEnd.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") || associationEnd.GetOtherEnd().Specification is Interface) && associationEnd.Name == roseAttribute.Name && MetaObjectMapper.GetShortNameFor(typeFullName) == roseAttributeTypeFullName)
                        return associationEnd;
                }
            }

            return null;
        }
        /// <MetaDataID>{2eac7302-874e-47a7-bfee-609012412d18}</MetaDataID>
        public void LoadCompleteModel()
        {
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            _Persistent = RoseClass.Persistence;
            OOAdvantech.MetaDataRepository.Association association = LinkAssociation;
            string fullName = FullName;
            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Roles)
            {
                object obj = associationEnd.Association.LinkClass;
                if (associationEnd.GetOtherEnd().Association.LinkClass != null)
                    obj = associationEnd.GetOtherEnd().Association.LinkClass.FullName;
                obj = associationEnd.Multiplicity;
                obj = associationEnd.Specification.FullName;
                obj = associationEnd.GetOtherEnd().Multiplicity;
                obj = associationEnd.GetOtherEnd().FullName;
                obj = associationEnd.Specification.Generalizations;
                if (associationEnd.Specification is Class)
                    obj = (associationEnd.Specification as Class).Realizations;
                if (associationEnd.Specification is Structure)
                    obj = (associationEnd.Specification as Structure).Realizations;
                obj = associationEnd.GetOtherEnd().Specification.Generalizations;
                if (associationEnd.GetOtherEnd().Specification is Class)
                    obj = (associationEnd.GetOtherEnd().Specification as Class).Realizations;
                if (associationEnd.GetOtherEnd().Specification is Structure)
                    obj = (associationEnd.GetOtherEnd().Specification as Structure).Realizations;
                associationEnd.GetOtherEnd().Specification.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");


            }
            foreach (OOAdvantech.MetaDataRepository.Feature feature in Features)
            {
                if (feature is OOAdvantech.MetaDataRepository.Operation)
                {
                    object obj = (feature as OOAdvantech.MetaDataRepository.Operation).Parameters;
                    obj = (feature as OOAdvantech.MetaDataRepository.Operation).ReturnType;
                    obj = (feature as OOAdvantech.MetaDataRepository.Operation).ParameterizedReturnType;
                }
                if (feature is OOAdvantech.MetaDataRepository.Method)
                {
                    object obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.Parameters;
                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ReturnType;
                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ParameterizedReturnType;
                }
                if (feature is OOAdvantech.MetaDataRepository.AttributeRealization)
                {
                    object obj = (feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification.Type;
                }
            }
            foreach (Generalization generalization in Generalizations)
            {
                if (generalization.Parent != null)
                {
                     fullName = generalization.Parent.FullName;
                }
            }
            foreach (Realization realization in Realizations)
            {
                if (realization.Abstarction != null)
                {
                     fullName = realization.Abstarction.FullName;
                }
            }

        }

        /// <MetaDataID>{59b3bec3-8ccc-49a8-9c80-f8f61d2ba97c}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Operation GetOperationForMethod(RationalRose.RoseOperation method)
        {
            string methodSignature = Operation.GetSignature(method);
            foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
                {
                    string operationSignature = null;
                    OOAdvantech.MetaDataRepository.Operation operation = feature as OOAdvantech.MetaDataRepository.Operation;
                    if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
                    {
                        operationSignature =Operation.GetSignature(operation);
                        if (operationSignature == methodSignature)
                            return operation;
                    }
                }

            }



            //if (method.OverrideKind != EnvDTE80.vsCMOverrideKind.vsCMOverrideKindOverride)
            //    return new Operation(method, this);



            foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
                {
                    string operationSignature = null;
                    OOAdvantech.MetaDataRepository.Operation operation = feature as OOAdvantech.MetaDataRepository.Operation;
                    if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
                    {
                        operationSignature = Operation.GetSignature(operation);
                        if (operationSignature == methodSignature)
                            return operation;
                    }
                }
            }
            Operation tmp = new Operation(method,this);
            long mm = tmp.Parameters.Count;
            return tmp;


        }





        /// <MetaDataID>{4d34401f-6028-430f-9677-f3629eef3ace}</MetaDataID>
        public RationalRose.RoseClass RoseClass;
        /// <MetaDataID>{3fd5bc02-ac4f-4d88-bee9-ccaf7c88265f}</MetaDataID>
        public Class(RationalRose.RoseClass roseClass, Component implementationUnit)
        {
            _ImplementationUnit.Value = implementationUnit;
            RoseClass = roseClass;
            MetaObjectMapper.AddTypeMap(roseClass.GetUniqueID(), this);
            if (string.IsNullOrEmpty(RoseClass.GetPropertyValue("MetaData", "MetaObjectID")))
                RoseClass.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
            _Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(RoseClass.GetPropertyValue("MetaData", "MetaObjectID"));
          //  roseClass.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());
            _Name = roseClass.Name;
            if (_Name != null)
                _Name = _Name.Trim();

            if (!roseClass.ParentCategory.Equals(roseClass.Model.RootCategory))
            {
                _Namespace.Value = MetaObjectMapper.FindMetaObjectFor(roseClass.ParentCategory.GetUniqueID()) as Namespace;
                if (_Namespace.Value == null&&!roseClass.ParentCategory.IsRootPackage())
                {
                    _Namespace.Value = new Namespace(roseClass.ParentCategory);
                    MetaObjectMapper.AddTypeMap(roseClass.ParentCategory.GetUniqueID(), _Namespace.Value);
                    roseClass.ParentCategory.OverrideProperty("MetaData", "MetaObjectID", _Namespace.Value.Identity.ToString());
                }


            }
            PutPropertyValue("MetaData", "Documentation", RoseClass.Documentation);

            _Persistent = RoseClass.Persistence;

            if (RoseClass.GetPropertyValue("MetaData", "BackwardCompatibilityIDTransfered") != "True")
            {
                RoseClass.OverrideProperty("MetaData", "BackwardCompatibilityIDTransfered", "True");
                RoseClass.OverrideProperty("MetaData", "BackwardCompatibilityID", RoseClass.GetPropertyValue("C#", "Identity"));
            }
            if (_Persistent||RoseClass.GetPropertyValue("C#", "Generate Backward ID") == "True")
            {
                string backwardCompatibilityID = RoseClass.GetPropertyValue("C#", "Identity");
                if(string.IsNullOrEmpty(backwardCompatibilityID ))
                {
                    RoseClass.OverrideProperty("C#", "Identity", _Identity.ToString());
                    backwardCompatibilityID=_Identity.ToString();
                }
                PutPropertyValue("MetaData", "BackwardCompatibilityID", backwardCompatibilityID);
            }
            string nextMemberIdentityString = RoseClass.GetPropertyValue("MetaData","LastMemberID");
            if (string.IsNullOrEmpty(nextMemberIdentityString))
                RoseClass.OverrideProperty("MetaData", "LastMemberID", ((int)1).ToString());
            _Persistent = RoseClass.Persistence;
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");


        }
        /// <MetaDataID>{53b61cb1-fedb-432c-aaa9-ba756c0d4967}</MetaDataID>
        RationalRose.RoseModel RoseModel;
        /// <MetaDataID>{68fa9afd-2f8c-47c2-a6e5-5fb18cdb3c39}</MetaDataID>
        internal Class(RationalRose.RoseModel roseModel )
        {
            RoseModel = roseModel;
        }
        //bool _Persistent;
        //public override bool Persistent
        //{
        //    get
        //    {
        //        if (RoseClass != null)
        //            return RoseClass.Persistence;
        //        return base.Persistent;
        //    }
        //    set
        //    {
        //        if (RoseClass != null)
        //            RoseClass.Persistence=value;

        //        base.Persistent = value;
        //    }
        //}
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if ((OriginMetaObject as OOAdvantech.MetaDataRepository.Classifier).IsTemplateInstantiation)
                return;

            if (_Namespace.Value == null)
            {
                if (OriginMetaObject.Namespace != null)
                {
                    _Namespace.Value = MetaObjectMapper.FindMetaObject(OriginMetaObject.Namespace.Identity) as Namespace;
                    if (_Namespace.Value == null)
                        _Namespace.Value = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginMetaObject.Namespace, this) as Namespace;

                    _Namespace.Value.Synchronize(OriginMetaObject.Namespace);
                }

            }

            if (RoseClass == null)
            {
                if ((OriginMetaObject as OOAdvantech.MetaDataRepository.Classifier).TemplateBinding!=null)
                    return;
                if(Namespace==null)
                    RoseClass = (MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).RoseApplication.CurrentModel.RootCategory.AddClass(OriginMetaObject.Name);
                else
                    RoseClass = (Namespace as Namespace).RoseCategory.AddClass(OriginMetaObject.Name);
                MetaObjectMapper.AddTypeMap(RoseClass.GetUniqueID(), this);

                RoseClass.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());
                _Name = OriginMetaObject.Name;
                _ImplementationUnit.Value = MetaObjectMapper.FindMetaObject(OriginMetaObject.ImplementationUnit.Identity) as Component;
                //RoseObjectProxy rp= System.Runtime.Remoting.RemotingServices.GetRealProxy((_ImplementationUnit.Value as Component).RoseComponent) as RoseObjectProxy;

                if(_ImplementationUnit.Value!=null&&(_ImplementationUnit.Value as Component).RoseComponent!=null)
                    RoseClass.AddAssignedModule((_ImplementationUnit.Value as Component).RoseComponent);

            }
            RoseClass.Name = OriginMetaObject.Name;
            
            base.Synchronize(OriginMetaObject);

            if ((OriginMetaObject as OOAdvantech.MetaDataRepository.Classifier).IsTemplate)
            {
                string name = RoseClass.Name;
                if (name.IndexOf("`") != -1)
                    RoseClass.Name = name.Substring(0, name.IndexOf("`"));
                RoseClass.ClassKind.Name = "ParameterizedClass";
                while (RoseClass.Parameters.Count > 0)
                    RoseClass.Parameters.Remove(RoseClass.Parameters.GetAt(1));

                int i=0;
                foreach (OOAdvantech.MetaDataRepository.TemplateParameter parameter in (OriginMetaObject as OOAdvantech.MetaDataRepository.Classifier).OwnedTemplateSignature.OwnedParameters)
                {
                    RoseClass.AddParameter(parameter.Name, "", "", (short)i++);

                }

                



                
            }
            RoseClass.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
             
            RoseClass.Persistence = Persistent;
            //string TT = RoseClass.ClassKind.Name;
            //for(int i=0;i<RoseClass.ClassKind.Types.Count;i++)
            //{
            //   string trt= RoseClass.ClassKind.Types.GetAt((short)(i+1));
            //}
            


            
        }
    }
}
