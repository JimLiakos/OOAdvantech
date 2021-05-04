using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{4ab4ccc4-50fb-4162-a8fc-6618eb5c5da1}</MetaDataID>
    internal class Structure : OOAdvantech.MetaDataRepository.Structure
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

        /// <MetaDataID>{92a5e583-70a0-4ef4-8257-331f0ef19b72}</MetaDataID>
        public void LoadCompleteModel()
        {
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            _Persistent = RoseClass.Persistence;
            string fullName = FullName;
            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Roles)
            {
                object obj = associationEnd.GetOtherEnd().Association.LinkClass;
                if (associationEnd.GetOtherEnd().Association.LinkClass != null)
                    obj = associationEnd.Association.LinkClass.FullName;
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

        /// <MetaDataID>{dcaa50e0-0523-4541-9afa-6af45281124b}</MetaDataID>
        bool IsRolesLoaded = false;
        /// <MetaDataID>{c48c8449-fc66-4af3-b53f-43d2df0f76e8}</MetaDataID>
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
        /// <MetaDataID>{c7afa5a5-3548-4604-a3f6-11ba58d39a73}</MetaDataID>
        bool GeneralizationsLoaded = false;
        /// <MetaDataID>{c1c78ec7-3df0-430b-b75b-49781bfeb198}</MetaDataID>
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
        /// <MetaDataID>{6159dcf4-f5be-4fd1-8abf-b3990e1e7cb9}</MetaDataID>
        bool IsFeaturesLoaded = false;
        /// <MetaDataID>{6cae9a3a-1024-42a1-a6e7-f43b2509979a}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> Features
        {
            get
            {
                if (!IsFeaturesLoaded)
                {

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
                            if (roseAttribute.GetPropertyValue("Persistent", "Persistent") == "True")
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
                        if (!IsFeaturesLoaded)
                            _Features.RemoveAll();

                    }

                }
                return base.Features;
            }
        }

        /// <MetaDataID>{5e7f828a-eac1-47c3-b0e0-bf0644e2c796}</MetaDataID>
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

        /// <MetaDataID>{c5b134f0-7622-47e9-b5ea-c02ddc46a18a}</MetaDataID>
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

        /// <MetaDataID>{e9373bfd-8b68-4c2d-a0e7-4507d4e0ef8d}</MetaDataID>
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




        /// <MetaDataID>{58f14fcb-dcf2-468c-9eee-f066b992786b}</MetaDataID>
        public RationalRose.RoseClass RoseClass;
        /// <MetaDataID>{918a2eaf-ba9f-43c5-800b-9d404da87506}</MetaDataID>
        public Structure(RationalRose.RoseClass roseClass, Component implementationUnit)
        {

            _ImplementationUnit.Value = implementationUnit;
            RoseClass = roseClass;
            _Persistent = RoseClass.Persistence;
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
            if (_Persistent)
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
        /// <MetaDataID>{5564b4ca-0421-48a8-91ef-38f5bb47a4ce}</MetaDataID>
        RationalRose.RoseModel RoseModel;
        /// <MetaDataID>{da564110-9d18-4bcf-aee2-1c41acfa6af2}</MetaDataID>
        internal Structure(RationalRose.RoseModel roseModel )
        {
            RoseModel = roseModel;
        }
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
                    return;

                RoseClass = (Namespace as Namespace).RoseCategory.AddClass(OriginMetaObject.Name);
                MetaObjectMapper.AddTypeMap(RoseClass.GetUniqueID(), this);


                RoseClass.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());
                _ImplementationUnit.Value = MetaObjectMapper.FindMetaObject(OriginMetaObject.ImplementationUnit.Identity) as Component;
                //RoseObjectProxy rp= System.Runtime.Remoting.RemotingServices.GetRealProxy((_ImplementationUnit.Value as Component).RoseComponent) as RoseObjectProxy;

                if(_ImplementationUnit.Value!=null&&(_ImplementationUnit.Value as Component).RoseComponent!=null)
                    RoseClass.AddAssignedModule((_ImplementationUnit.Value as Component).RoseComponent);

            }
            RoseClass.Name = OriginMetaObject.Name;
            RoseClass.Stereotype = "Structure";
            base.Synchronize(OriginMetaObject);
            RoseClass.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
            
            RoseClass.Persistence = Persistent;

            
        }
    }
}
