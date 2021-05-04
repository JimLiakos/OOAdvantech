using OOAdvantech.Collections.Generic;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{E17FEF3E-E08F-4FF9-A16F-4195B2AB9DC1}</MetaDataID>
    internal class Association : OOAdvantech.MetaDataRepository.Association
    {
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string));
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
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }
        /// <MetaDataID>{e5e8c76c-9935-4761-83f6-6d9eaf43a51e}</MetaDataID>
        new OOAdvantech.Member<OOAdvantech.MetaDataRepository.Classifier> _LinkClass = new OOAdvantech.Member<OOAdvantech.MetaDataRepository.Classifier>();
        public override OOAdvantech.MetaDataRepository.Classifier LinkClass
        {
            get
            {
                if (!_LinkClass.UnInitialized)
                    return _LinkClass.Value;
                object tmp = RoseAssociation.LinkClass;
                if (RoseAssociation.LinkClass != null )
                {
                    RationalRose.RoseClass roseLinkClass = RoseAssociation.LinkClass;
                    _LinkClass.Value = MetaObjectMapper.FindMetaObjectFor(roseLinkClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                    Component component = null;
                    if (roseLinkClass.GetAssignedModules().Count > 0)
                    {
                        RationalRose.RoseModule roseModule = roseLinkClass.GetAssignedModules().GetAt(1);
                        component = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                        if (component == null)
                            component = new Component(roseModule);

                    }
                    if (_LinkClass.Value == null)
                    {
                        if (roseLinkClass.Stereotype == "Interface")
                            _LinkClass.Value = new Interface(roseLinkClass, null);
                        else
                            _LinkClass.Value = new Class(roseLinkClass, null);
                    }
                }
                else
                    _LinkClass.Value = null;

                return _LinkClass.Value;
            }
            set
            {
                base.LinkClass = value;
            }
        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            base.Synchronize(OriginMetaObject);
            RoseAssociation.Name = _Name;
            if (_General.Value != null)
                RoseAssociation.OverrideProperty("C#", "General Association Identity", _General.Value.Identity.ToString());

        }
        internal RationalRose.RoseAssociation RoseAssociation;
        public Association(RationalRose.RoseAssociation roseAssociation, AssociationEnd roleA, AssociationEnd roleB)
        {
            RoseAssociation = roseAssociation;
            _RoleA = roleA;
            _RoleB = roleB;
            _Name = roseAssociation.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 
            if (string.IsNullOrEmpty(RoseAssociation.GetPropertyValue("MetaData", "UniqueID")))
                RoseAssociation.OverrideProperty("MetaData", "UniqueID", RoseAssociation.GetUniqueID());

            if (string.IsNullOrEmpty(RoseAssociation.GetPropertyValue("C#", "Identity")) ||
                RoseAssociation.GetPropertyValue("MetaData", "UniqueID") != RoseAssociation.GetUniqueID())
            {
                RoseAssociation.OverrideProperty("C#", "Identity", System.Guid.NewGuid().ToString());
                RoseAssociation.OverrideProperty("MetaData", "UniqueID", RoseAssociation.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseAssociation.GetPropertyValue("C#", "Identity"));
            string generalAssociationIdentity = RoseAssociation.GetPropertyValue("C#", "General Association Identity");
            if (!string.IsNullOrEmpty(generalAssociationIdentity))
            {
                if (_RoleB != null && _RoleB.Specification != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Classifier generalClassifier in _RoleB.Specification.GetAllGeneralClasifiers())
                    {
                        foreach (OOAdvantech.MetaDataRepository.AssociationEnd generalAssociationEnd in generalClassifier.GetRoles(true))
                        {
                            if (generalAssociationEnd.Association.Identity.ToString().ToLower() == generalAssociationIdentity.ToLower())
                            {
                                _General.Value = generalAssociationEnd.Association;
                                break;
                            }
                        }
                        break;

                    }
                }

                if (_RoleA != null && _RoleA.Specification != null && _General==null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Classifier generalClassifier in _RoleA.Specification.GetAllGeneralClasifiers())
                    {
                        foreach (OOAdvantech.MetaDataRepository.AssociationEnd generalAssociationEnd in generalClassifier.GetRoles(true))
                        {
                            if (generalAssociationEnd.Association.Identity.ToString().ToLower() == generalAssociationIdentity.ToLower())
                            {
                                _General.Value = generalAssociationEnd.Association;
                                break;
                            }
                        }
                        break;
                    }
                    
                }

            }
           // RoseAssociation.OverrideProperty("C#", "Identity", _Identity.ToString());

            MetaObjectMapper.AddTypeMap(roseAssociation.GetUniqueID(), this);
        }
        bool LoadConnection;
        public override Set<OOAdvantech.MetaDataRepository.AssociationEnd> Connections
        {
            get
            {
                if(_Connections.Count!=2)
                {

                    if (!LoadConnection)
                    {
                        try
                        {
                            LoadConnection = true;
                            if (RoleA != null && !_Connections.Contains(RoleA))
                                _Connections.Add(RoleA);
                            if (RoleB != null && !_Connections.Contains(RoleB))
                                _Connections.Add(RoleB);
                        }
                        finally
                        {
                            LoadConnection = false;
                        }
                    }

                }
                return base.Connections;
            }
        }

        public override OOAdvantech.MetaDataRepository.AssociationEnd RoleA
        {
            get
            {
                try
                {
                    if (_RoleA == null)
                    {
                        string tt = RoseAssociation.Role1.Name;

                        OOAdvantech.MetaDataRepository.Classifier classifier = MetaObjectMapper.FindMetaObjectFor(RoseAssociation.Role1.Class.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;

                        if (classifier == null)
                        {
                            RationalRose.RoseClass roseClass = RoseAssociation.Role1.Class;
                            Component implementetionUnit = null;
                            if (roseClass.GetAssignedModules().Count > 0)
                            {
                                implementetionUnit = MetaObjectMapper.FindMetaObjectFor(roseClass.GetAssignedModules().GetAt(1).GetUniqueID()) as Component;
                                if (implementetionUnit == null)
                                    implementetionUnit = new Component(roseClass.GetAssignedModules().GetAt(1));
                            }
                            if (roseClass.Stereotype == "Interface")
                                classifier = new Interface(roseClass, implementetionUnit);
                            else
                                classifier = new Class(roseClass, implementetionUnit);
                        }



                        foreach (AssociationEnd associationEnd in classifier.GetRoles(false))
                        {

                            if ((associationEnd.RoseRole != null && associationEnd.RoseRole.Equals(RoseAssociation.Role1))
                                || associationEnd.Identity.ToString() == Identity.ToString() + ".RoleA")
                            {
                                _RoleA = associationEnd;
                                break;
                            }
                        }
                        if (_RoleA == null)
                        {
                            _RoleA = new AssociationEnd(RoseAssociation.Role1, classifier);

                        }

                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                return base.RoleA;
            }
        }
        public override OOAdvantech.MetaDataRepository.AssociationEnd RoleB
        {
            get
            {
                if (_RoleB == null)
                {
                    string tt = RoseAssociation.Role2.Name;
                    tt = RoseAssociation.Role2.Class.Name;
                    OOAdvantech.MetaDataRepository.Classifier classifier = MetaObjectMapper.FindMetaObjectFor(RoseAssociation.Role2.Class.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                    if (classifier == null)
                    {
                        Component implementationUnit =null;
                        if (RoseAssociation.Role2.Class.GetAssignedModules().Count > 0)
                        {
                            RationalRose.RoseModule roseModule = RoseAssociation.Role2.Class.GetAssignedModules().GetAt(1);

                            implementationUnit = MetaObjectMapper.FindMetaObjectFor(roseModule.GetUniqueID()) as Component;
                            if (implementationUnit == null)
                                implementationUnit = new Component(roseModule);
                        }
                        if (RoseAssociation.Role2.Class.Stereotype == "Interface")
                            classifier = new Interface(RoseAssociation.Role2.Class, implementationUnit);
                        else
                            classifier = new Class(RoseAssociation.Role2.Class, implementationUnit);
                    }
                    foreach (AssociationEnd associationEnd in classifier.GetRoles(false))
                    {
                        if ((associationEnd.RoseRole != null && associationEnd.RoseRole.Equals(RoseAssociation.Role2))
                             || associationEnd.Identity.ToString() == Identity.ToString() + ".RoleB")
                        {

                            _RoleB = associationEnd;
                            break;
                        }
                    }
                    if(_RoleB==null)
                    {
                       _RoleB= new AssociationEnd(RoseAssociation.Role2,classifier);
                        
                    }
                }
                return base.RoleB;
            }
        }


        internal void SetIdentity(OOAdvantech.MetaDataRepository.MetaObjectID theIdentity)
        {
            _Identity = theIdentity;
            RoseAssociation.OverrideProperty("C#", "Identity", theIdentity.ToString());
        }
        //
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
