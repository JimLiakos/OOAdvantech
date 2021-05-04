using MetaDataRepository = OOAdvantech.MetaDataRepository;
namespace RoseMetaDataRepository
{
    /// <MetaDataID>{9C933CAF-941E-498E-8FCE-DBF07CA51920}</MetaDataID>
    internal class AssociationEnd : OOAdvantech.MetaDataRepository.AssociationEnd
    {
        internal AssociationEnd()
        {

        }
        bool MultiplicityLoaded = false;
        public override OOAdvantech.MetaDataRepository.MultiplicityRange Multiplicity
        {
            get
            {
                if (!MultiplicityLoaded)
                {
                    _Multiplicity = GetMultiplicityRange(RoseRole.Cardinality);
                    MultiplicityLoaded = true;
                }

                
                return base.Multiplicity;
            }
        }

        static Parser.Parser _MultiplicityParser;
        internal static Parser.Parser MultiplicityParser
        {
            get
            {
                if (_MultiplicityParser == null)
                {
                    _MultiplicityParser = new Parser.Parser();

                    
                    string[] Resources = typeof(AssociationEnd).Assembly.GetManifestResourceNames();
                    //using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.CompositeOQL.GMR"))
                    using (System.IO.Stream Grammar = typeof(AssociationEnd).Assembly.GetManifestResourceStream("RoseMetaDataRepository.Multiplicity.cgt"))
                    {
                        byte[] bytes = new byte[Grammar.Length];
                        Grammar.Read(bytes, 0, (int)Grammar.Length);
                        _MultiplicityParser.SetGrammar(bytes, (int)Grammar.Length);
                        Grammar.Close();
                    }
                }

                return _MultiplicityParser;
            }

        }

        MetaDataRepository.MultiplicityRange GetMultiplicityRange(string multilicityString)
        {
            try
            {
                if(string.IsNullOrEmpty(multilicityString))
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange();
                MultiplicityParser.Parse(multilicityString);
                if(MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Exactly"]!=null)
                {
                    string value = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Exactly"] as Parser.ParserNode).Value;
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value), ulong.Parse(value));
                }
                else if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Unspecified"] != null)
                {
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange(0);
                    int adsasd = 0;
                }
                else if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"] != null)
                {
                    string value = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"] as Parser.ParserNode).Value;
                    if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["TillStatament"]["UpLimit"]["Many"] != null)
                    {
                        return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value));
                    }
                    else
                    {
                        string upValue = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["TillStatament"]["UpLimit"] as Parser.ParserNode).Value;

                        return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value), ulong.Parse(upValue));
                    }
                    //MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"]["TillStatament"]["UpLimit"]
                    int adsasd = 0;
                }



            }
            catch (System.Exception error)
            {

            }
            return new OOAdvantech.MetaDataRepository.MultiplicityRange();


        }
 
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {

            try
            {
                MetaDataRepository.MultiplicityRange temp = GetMultiplicityRange("1");
                temp = GetMultiplicityRange("n");
                temp = GetMultiplicityRange("1..n");
                temp = GetMultiplicityRange("1..5");

            }
            catch (System.Exception error)
            {

                
            }
            MetaDataRepository.AssociationEnd originAssociationEnd = null;

            originAssociationEnd = originMetaObject as MetaDataRepository.AssociationEnd;
            if (RoseRole == null)
            {
                RationalRose.RoseClass roseClientClass = null, roseSupplierClass = null;
                MetaDataRepository.Classifier clientClassifier = null, supplierClassifier = null;
                _IsRoleA = originAssociationEnd.IsRoleA;
                string specName =originAssociationEnd.Specification.FullName;
                string specNameb = originAssociationEnd.GetOtherEnd().Specification.FullName;

                specName = originAssociationEnd.Specification.Identity.ToString();
                specNameb = originAssociationEnd.GetOtherEnd().Specification.Identity.ToString();


                if (_IsRoleA)
                {
                    supplierClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as MetaDataRepository.Classifier;
                    clientClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.GetOtherEnd().Specification, this) as MetaDataRepository.Classifier;
                }
                else
                {
                    clientClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as MetaDataRepository.Classifier;
                    supplierClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.GetOtherEnd().Specification, this) as MetaDataRepository.Classifier;
                }
                if (clientClassifier == null || supplierClassifier == null)
                    return;
                if (clientClassifier is Interface)
                    roseClientClass = (clientClassifier as Interface).RoseClass;
                else if (clientClassifier is Structure)
                    roseClientClass = (clientClassifier as Structure).RoseClass;
                else
                    roseClientClass = (clientClassifier as Class).RoseClass;


                if (supplierClassifier is Interface)
                    roseSupplierClass = (supplierClassifier as Interface).RoseClass;
                else if (supplierClassifier is Structure)
                    roseSupplierClass = (supplierClassifier as Structure).RoseClass;
                else
                    roseSupplierClass = (supplierClassifier as Class).RoseClass;

                    if (roseClientClass == null || roseSupplierClass == null)
                        return;

                    RationalRose.RoseCategory category = roseSupplierClass.ParentCategory;
                    string supplierClassFullName = "";
                    while (!category.Equals((roseSupplierClass.Application as RationalRose.RoseApplication).CurrentModel.RootCategory))
                    {
                        supplierClassFullName = category.Name + "::" + supplierClassFullName;
                        category = category.ParentCategory;
                    }
                    supplierClassFullName = category.Name + "::" + supplierClassFullName + roseSupplierClass.Name;
                    string clientClassName = roseClientClass.Name;

                    RationalRose.RoseAssociation roseAssociation = null;
                    if (_IsRoleA)
                    {
                        roseAssociation = roseClientClass.AddAssociation(originAssociationEnd.Name, supplierClassFullName);
                        RoseRole = roseAssociation.Role1;
                        _Name = originAssociationEnd.Name;
                        string tmp = originAssociationEnd.GetOtherEnd().Name;
                        tmp = roseAssociation.Role1.Class.Name;
                        AssociationEnd otherEndAssociationEnd = new AssociationEnd(roseAssociation.Role2, clientClassifier);
                        roseAssociation.OverrideProperty("C#", "Identity", originAssociationEnd.Association.Identity.ToString());
                        _Association = new Association(roseAssociation, this, otherEndAssociationEnd);
                        
                    }
                    else
                    {

                        roseAssociation = roseClientClass.AddAssociation(originAssociationEnd.GetOtherEnd().Name, supplierClassFullName);
                        RoseRole = roseAssociation.Role2;
                        _Name = originAssociationEnd.Name;
                        if (_Name != null)
                            _Name = _Name.Trim();

                        AssociationEnd otherEndAssociationEnd = new AssociationEnd(roseAssociation.Role1, supplierClassifier);
                        roseAssociation.OverrideProperty("C#", "Identity", originAssociationEnd.Association.Identity.ToString());
                        _Association = MetaObjectMapper.FindMetaObjectFor(RoseRole.Association.GetUniqueID()) as Association;
                        if(_Association ==null)
                            _Association = new Association(roseAssociation, otherEndAssociationEnd, this);
                        (_Association as Association) .SetIdentity(originAssociationEnd.Association.Identity);

                    }


                    

            }




            base.Synchronize(originAssociationEnd);
            string tt = originAssociationEnd.Name;
            _Multiplicity =new OOAdvantech.MetaDataRepository.MultiplicityRange( originAssociationEnd.Multiplicity);

            if (_Namespace.Value == null && originAssociationEnd.Namespace!=null)
                _Namespace.Value = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Namespace, this) as MetaDataRepository.Classifier;


            if (RoseRole != null)
            {
                RoseRole.Name = _Name;
                if (!_Multiplicity.Unspecified)
                {
                    if (_Multiplicity.NoHighLimit)
                        RoseRole.Cardinality = _Multiplicity.LowLimit.ToString() + "..*";
                    else if (_Multiplicity.LowLimit != _Multiplicity.HighLimit)
                        RoseRole.Cardinality = _Multiplicity.LowLimit.ToString() + ".." + _Multiplicity.HighLimit.ToString();
                    else
                    {
                        string cardinality = _Multiplicity.LowLimit.ToString();
                        RoseRole.Cardinality = cardinality;
                    }
                }
                RoseRole.Navigable = Navigable;
            }
   

            if (originAssociationEnd.Getter != null || originAssociationEnd.Setter != null)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                RoseRole.OverrideProperty("C#", "As Property", "True");
                if (originAssociationEnd.Getter != null)
                {
                    PutPropertyValue("MetaData", "Getter", true);
                    RoseRole.OverrideProperty("C#", "GenerateGetOperation", "True");
                }
                if (originAssociationEnd.Setter != null)
                {
                    PutPropertyValue("MetaData", "Setter", true);
                    RoseRole.OverrideProperty("C#", "GenerateSetOperation", "True");
                }

            }
            else
            {
                PutPropertyValue("MetaData", "AsProperty", false);
                RoseRole.OverrideProperty("C#", "As Property", "False");

            }
            if (originAssociationEnd.CollectionClassifier != null)
            {
                string containerClass=null;
                if (originAssociationEnd.CollectionClassifier.TemplateBinding != null)
                {
                    if (originAssociationEnd.CollectionClassifier.FullName.LastIndexOf('`') != -1 ||
                        originAssociationEnd.CollectionClassifier.FullName.LastIndexOf('>') != originAssociationEnd.CollectionClassifier.FullName.Length-1)
                    {
                        if (originAssociationEnd.CollectionClassifier.Namespace != null)
                     
                            containerClass = originAssociationEnd.CollectionClassifier.FullName.Substring(0, originAssociationEnd.CollectionClassifier.FullName.LastIndexOf('`'));
                        
                        containerClass += "<";

                        bool firstParameter = true;
                        foreach (MetaDataRepository.TemplateParameterSubstitution parameter in originAssociationEnd.CollectionClassifier.TemplateBinding.ParameterSubstitutions)
                        {
                            if (!firstParameter)
                                containerClass += ",";
                            containerClass += (parameter.ActualParameters[0] as OOAdvantech.MetaDataRepository.MetaObject).FullName;


                            firstParameter = false;
                        }
                        containerClass += ">";
                    }
                    else
                        containerClass = originAssociationEnd.CollectionClassifier.FullName;
                }
                else
                    containerClass = originAssociationEnd.CollectionClassifier.FullName;




                RoseRole.OverrideProperty("C#", "ContainerClass", containerClass);
            }

            RoseRole.ExportControl.Name = RoseAccessTypeConverter.GetExportControl(Visibility);
            Navigable = originAssociationEnd.Navigable;
            RoseRole.Navigable = Navigable;
            RoseRole.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
            if(Indexer)
                RoseRole.OverrideProperty("C#", "Indexer", "True");
            else
                RoseRole.OverrideProperty("C#", "Indexer", "False");



        }
 
        public override OOAdvantech.MetaDataRepository.AssociationEnd GetOtherEnd()
        {
            if (IsRoleA)
                return Association.RoleB;
            else
                return Association.RoleA;

        }


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
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (RoseRole == null)
                    if (_Identity != null)
                        return _Identity;
                    else
                    {
                        return base.Identity;
                    }

                if (Association != null)
                {
                    if (IsRoleA)
                        return new OOAdvantech.MetaDataRepository.MetaObjectID(Association.Identity + "RoleA");
                    else
                        return new OOAdvantech.MetaDataRepository.MetaObjectID(Association.Identity + "RoleB");
                }

                return base.Identity;
            }
        }
        public override OOAdvantech.MetaDataRepository.Namespace Namespace
        {
            get
            {
                if (_Namespace.Value != null)
                    return _Namespace.Value;
                return GetOtherEnd().Specification;
            }
        }
        public override OOAdvantech.MetaDataRepository.Association Association
        {
            get
            {
                if (_Association == null)
                {
                    _Association = MetaObjectMapper.FindMetaObjectFor(RoseRole.Association.GetUniqueID()) as Association;
                    if (_Association == null)
                    {

                        if (IsRoleA)
                        {
                            
                            _Association = new Association(RoseRole.Association, this, null);
                            var loadedCount = (Namespace as OOAdvantech.MetaDataRepository.Classifier).Roles.Count;
                        }
                        else
                        {
                            _Association = new Association(RoseRole.Association, null, this);
                            var loadedCount = (Namespace as OOAdvantech.MetaDataRepository.Classifier).Roles.Count;
                        }
                    }
                    else
                    {

                    }

                }
                return _Association;
            }
        }
       internal RationalRose.RoseRole RoseRole;
        
        
        public AssociationEnd(RationalRose.RoseRole roseRole,MetaDataRepository.Classifier specification):base(roseRole.Name,specification,OOAdvantech.MetaDataRepository.Roles.RoleA)
        {
            if (roseRole.Equals(roseRole.Association.Role1))
                _IsRoleA = true;
            else
                _IsRoleA = false;

            RoseRole = roseRole;
            _Specification = specification;



            _Name = roseRole.Name;
            if (_Name != null)
                _Name=_Name.Trim();
 
            

            

            if (string.IsNullOrEmpty(roseRole.GetPropertyValue("MetaData", "UniqueID")))
                roseRole.OverrideProperty("MetaData", "UniqueID", roseRole.GetUniqueID());

            if (string.IsNullOrEmpty(roseRole.GetPropertyValue("MetaData", "MetaObjectID")) ||
                roseRole.GetPropertyValue("MetaData", "UniqueID") != roseRole.GetUniqueID())
            {
                roseRole.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
                roseRole.OverrideProperty("MetaData", "UniqueID", roseRole.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(roseRole.GetPropertyValue("MetaData", "MetaObjectID"));
            //roseRole.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(roseRole.ExportControl.Name);
            Navigable = roseRole.Navigable;
            if (roseRole.GetPropertyValue("C#", "As Property") == "True" || _Namespace.Value is Interface)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                bool getMethod = roseRole.GetPropertyValue("C#", "GenerateGetOperation") == "True";
                bool setMethod = roseRole.GetPropertyValue("C#", "GenerateSetOperation") == "True";
                PutPropertyValue("MetaData", "Getter", getMethod);
                PutPropertyValue("MetaData", "Setter", setMethod);

            }
            else
                PutPropertyValue("MetaData", "AsProperty", false);
            string containerClassName = roseRole.GetPropertyValue("C#", "ContainerClass");
            if (!string.IsNullOrEmpty(containerClassName) && containerClassName.Trim().ToLower() != "void")
                _CollectionClassifier = UnknownClassifier.GetClassifier(containerClassName);
            else
                _CollectionClassifier= null;

            PutPropertyValue("MetaData", "Documentation", roseRole.Documentation);


            if(RoseRole.GetPropertyValue("C#", "Synchronize") == "False")
                PutPropertyValue("MetaData", "Synchronize", false.ToString());
            else
                PutPropertyValue("MetaData", "Synchronize", true.ToString());


            _LazyFetching = RoseRole.GetPropertyValue("Persistent", "Loading Type") != "On Construction";
            _CascadeDelete = RoseRole.GetPropertyValue("Persistent", "Cascade Delete") == "True";
            _ReferentialIntegrity= RoseRole.GetPropertyValue("Persistent", "Referential Integrity") == "True";
            string persistentMemberName=roseRole.GetPropertyValue("Persistent", "Member that implement");
            if(persistentMemberName=="Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);

            _Persistent = RoseRole.Association.GetPropertyValue("Persistent", "Persistent") == "True";
            _Indexer= RoseRole.GetPropertyValue("C#", "Indexer")== "True";
            _HasBehavioralSettings = true;
            string  generalAssociation = RoseRole.Association.Constraints;
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            string loadFullName = specification.FullName;

        }
    }
}
