namespace RoseMetaDataRepository
{
    /// <MetaDataID>{3AF798B5-1D92-4212-85E6-BAE86539523C}</MetaDataID>
    internal class Interface : OOAdvantech.MetaDataRepository.Interface
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
                    PutPropertyValue(propertyNamespace, propertyName, identity);
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }



        /// <MetaDataID>{d3acb587-2b90-4f04-bfcb-8402f39775db}</MetaDataID>
        public void LoadCompleteModel()
        {

            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            string fullName = FullName;
            OOAdvantech.MetaDataRepository.Association association = LinkAssociation;
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
                obj = associationEnd.GetOtherEnd().Specification.FullName;
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


        }
        bool LinkAssociationLoaded = false;
        /// <MetaDataID>{1e530616-c35c-4db3-865c-262cb39c8597}</MetaDataID>
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
        /// <MetaDataID>{9f883e43-a31b-48f3-8815-1dd92285353f}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{4d116a99-1215-4448-b937-ea106743c9ff}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd> Roles
        {

            get
            {
                if (!IsRolesLoaded)
                {
                    if (RoseClass == null)
                        return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>();
  
                    for (int i = 0; i < RoseClass.GetRoles().Count; i++)
                    {
                        long count = RoseClass.GetRoles().Count;
                        RationalRose.RoseRole roseRole = RoseClass.GetRoles().GetAt((short)(i + 1));
                        RationalRose.RoseClass associateClass = roseRole.AssociateItem as RationalRose.RoseClass;
                        RationalRose.RoseRole otherEndRoseRole = null;
                        if (roseRole.Association.Role1.Equals(roseRole))
                            otherEndRoseRole = roseRole.Association.Role2;
                        else
                            otherEndRoseRole = roseRole.Association.Role1;
                        if (otherEndRoseRole.Class != null)
                            _Roles.Add(new AssociationEnd(roseRole, this));

                    }
                    IsRolesLoaded = true;
                }

                return base.Roles;

            }
        }
        /// <MetaDataID>{05740f63-dfb5-4e49-aa8d-c1ed2955c06b}</MetaDataID>
        internal RationalRose.RoseClass RoseClass;
        /// <MetaDataID>{80711f9c-b654-41d8-87ee-f56219086fcf}</MetaDataID>
        public Interface(RationalRose.RoseClass roseClass, Component implementationUnit)
        {

            _ImplementationUnit.Value = implementationUnit;
            RoseClass = roseClass;
            MetaObjectMapper.AddTypeMap(roseClass.GetUniqueID(), this);
            if (string.IsNullOrEmpty(RoseClass.GetPropertyValue("MetaData", "MetaObjectID")))
                RoseClass.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");

            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseClass.GetPropertyValue("MetaData", "MetaObjectID"));
            roseClass.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());
            _Name = roseClass.Name;
            if (_Name != null)
                _Name = _Name.Trim();

            if (!roseClass.ParentCategory.Equals( roseClass.Model.RootCategory))
            {
                _Namespace.Value = MetaObjectMapper.FindMetaObjectFor(roseClass.ParentCategory.GetUniqueID()) as Namespace;
                if (_Namespace.Value == null)
                {
                    //object tty = roseClass.ParentCategory.ParentCategory;
                    _Namespace.Value = new Namespace(roseClass.ParentCategory);
                    MetaObjectMapper.AddTypeMap(roseClass.ParentCategory.GetUniqueID(), _Namespace.Value);
                    roseClass.ParentCategory.OverrideProperty("MetaData", "MetaObjectID", _Namespace.Value.Identity.ToString());
                }
            }
            PutPropertyValue("MetaData", "Documentation", RoseClass.Documentation);

            if (RoseClass.GetPropertyValue("C#", "Generate Backward ID") == "True")
            {
                string backwardCompatibilityID = RoseClass.GetPropertyValue("C#", "Identity");
                if (string.IsNullOrEmpty(backwardCompatibilityID))
                {
                    RoseClass.OverrideProperty("C#", "Identity", _Identity.ToString());
                    backwardCompatibilityID = _Identity.ToString();
                }
                PutPropertyValue("MetaData", "BackwardCompatibilityID", backwardCompatibilityID);
            }
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
        }
        /// <MetaDataID>{b2722fa9-d545-4b35-afc6-5b6dc03b47e7}</MetaDataID>
        RationalRose.RoseModel RoseModel;
        /// <MetaDataID>{ca991451-23d4-4898-b993-21e4ab67702e}</MetaDataID>
        internal Interface(RationalRose.RoseModel roseModel)
        {
            RoseModel = roseModel;
        }

        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return base.Identity;
            }
        }
        /// <MetaDataID>{f8d7a9f2-ae51-49ec-a511-fdb26bb13b64}</MetaDataID>
        bool GeneralizationsLoaded = false;

        /// <MetaDataID>{54bc741e-cec1-4cd0-b03b-5eda002c8228}</MetaDataID>
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
                        Interface superClass = MetaObjectMapper.FindMetaObjectFor(roseSuperClass.GetUniqueID()) as Interface;
                        Component component = null;
                        if (roseSuperClass.GetAssignedModules().Count > 0)
                        {
                            RationalRose.RoseModule roseModule = roseSuperClass.GetAssignedModules().GetAt(1);
                            component = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseModule);

                        }
                        if (superClass == null)
                            superClass = new Interface(roseSuperClass, component);
                        _Generalizations.Add(new Generalization("", superClass, this));
                    }
                    GeneralizationsLoaded = true;
                }
                return base.Generalizations;
            }
        }

        /// <MetaDataID>{76a75c23-9bae-4f57-a2e5-517ff00a46f6}</MetaDataID>
        bool IsFeaturesLoaded = false;
        /// <MetaDataID>{5757def0-c865-4354-9369-c861ff0f837c}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> Features
        {
            get
            {
                if (!IsFeaturesLoaded)
                {
                    try
                    {
                        if (RoseClass == null)
                            return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>();
  
                        for (int i = 0; i < RoseClass.Operations.Count; i++)
                        {
                            RationalRose.RoseOperation roseOperation = RoseClass.Operations.GetAt((short)(i + 1));
                            Operation operation = new Operation(roseOperation, this);
                             _Features.Add(operation);
                        }

                        for (int i = 0; i < RoseClass.Attributes.Count; i++)
                        {
                            RationalRose.RoseAttribute roseAttribute = RoseClass.Attributes.GetAt((short)(i + 1));

                            if (roseAttribute.GetPropertyValue("C#", "Generate Backward ID") == "True")
                            {
                                if (string.IsNullOrEmpty(roseAttribute.GetPropertyValue("C#", "Identity")))
                                {
                                    string nextMemberIdentityString = RoseClass.GetPropertyValue("MetaData", "LastMemberID");
                                    if (string.IsNullOrEmpty(nextMemberIdentityString))
                                    {
                                        RoseClass.OverrideProperty("MetaData", "LastMemberID", ((int)1).ToString());
                                        nextMemberIdentityString = "1";
                                    }
                                    roseAttribute.OverrideProperty("C#", "Identity", "+" + nextMemberIdentityString);
                                    int next = int.Parse(nextMemberIdentityString);
                                    next++;
                                    RoseClass.OverrideProperty("MetaData", "LastMemberID", next.ToString());
                                }
                            }



                            _Features.Add(new Attribute(roseAttribute, this));

                        }
                        IsFeaturesLoaded = true;
                    }
                    finally
                    {
                        if (!IsFeaturesLoaded)
                            _Features.RemoveAll();

                    }

                }
                return base.Features;
            }
        }


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
                if ((OriginMetaObject as OOAdvantech.MetaDataRepository.Classifier).TemplateBinding != null)
                {
                    OOAdvantech.MetaDataRepository.Namespace nm = OriginMetaObject.Namespace;
                    return;
                }

                if (Namespace == null)
                    RoseClass = (MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).RoseApplication.CurrentModel.RootCategory.AddClass(OriginMetaObject.Name);
                else
                    RoseClass = (Namespace as Namespace).RoseCategory.AddClass(OriginMetaObject.Name);
                MetaObjectMapper.AddTypeMap(RoseClass.GetUniqueID(), this);

                RoseClass.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());
                _ImplementationUnit.Value = MetaObjectMapper.FindMetaObject(OriginMetaObject.ImplementationUnit.Identity) as Component;
                //RoseObjectProxy rp= System.Runtime.Remoting.RemotingServices.GetRealProxy((_ImplementationUnit.Value as Component).RoseComponent) as RoseObjectProxy;
                try
                {
                    if (_ImplementationUnit.Value!=null&&(_ImplementationUnit.Value as Component).RoseComponent != null)
                        RoseClass.AddAssignedModule((_ImplementationUnit.Value as Component).RoseComponent);
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            base.Synchronize(OriginMetaObject);
            RoseClass.Name = OriginMetaObject.Name;
            RoseClass.Stereotype = "Interface";
            RoseClass.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;

        }


    }
}
