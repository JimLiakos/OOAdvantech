using MetaDataRepository = OOAdvantech.MetaDataRepository;
namespace RoseMetaDataRepository
{
    /// <MetaDataID>{C23F94B8-2668-4FBA-8B66-EA733A83DD14}</MetaDataID>
    internal class Attribute : OOAdvantech.MetaDataRepository.Attribute
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
                if (RoseAttribute != null)
                    RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", PropertyValue as string);
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

        RationalRose.RoseModel RoseModel;
        internal Attribute(RationalRose.RoseModel roseModel)
        {
            RoseModel = roseModel;
        }

        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Attribute originStructuralFeature = null;
            if (originMetaObject is MetaDataRepository.AttributeRealization)
                originStructuralFeature = (originMetaObject as MetaDataRepository.AttributeRealization).Specification;
            else
                originStructuralFeature = originMetaObject as MetaDataRepository.Attribute;
            try
            {

                base.Synchronize(originMetaObject);
            }
            catch (System.Exception error)
            {
                throw;
            }

            if (_Owner == null)
                _Owner = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as MetaDataRepository.Feature).Owner, this) as MetaDataRepository.Classifier;

            string typeFullName = null;
            if (originStructuralFeature.Type != null)
                typeFullName =MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName( originStructuralFeature.Type));
            if (originStructuralFeature.ParameterizedType != null)
                typeFullName = originStructuralFeature.ParameterizedType.FullName;

            if (RoseAttribute != null)
            {
                RoseAttribute.Name = _Name;
                RoseAttribute.Type = MetaObjectMapper.GetShortNameFor(typeFullName);
            }
            else
            {
                if (_Owner is Interface)
                    RoseAttribute = (_Owner as Interface).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(typeFullName), "");
                if (_Owner is Class)
                    RoseAttribute = (_Owner as Class).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(typeFullName), "");
                if (_Owner is Structure)
                    RoseAttribute = (_Owner as Structure).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(typeFullName), "");


                RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", Identity.ToString());
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());
            }

            if (originStructuralFeature.Getter != null || originStructuralFeature.Setter != null)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                RoseAttribute.OverrideProperty("C#", "As Property", "True");
                if (originStructuralFeature.Getter != null)
                {
                    PutPropertyValue("MetaData", "Getter", true);
                    RoseAttribute.OverrideProperty("C#", "GenerateGetOperation", "True");
                }
                if (originStructuralFeature.Setter != null)
                {
                    PutPropertyValue("MetaData", "Setter", true);
                    RoseAttribute.OverrideProperty("C#", "GenerateSetOperation", "True");
                }

            }
            else
            {
                PutPropertyValue("MetaData", "AsProperty", false);
                RoseAttribute.OverrideProperty("C#", "As Property", "False");

            }
            RoseAttribute.ExportControl.Name = RoseAccessTypeConverter.GetExportControl(Visibility);
            RoseAttribute.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;


            string id = originMetaObject.ToString();
            //LoadType
            
        }


        internal RationalRose.RoseAttribute RoseAttribute;
        public Attribute(RationalRose.RoseAttribute roseAttribute, MetaDataRepository.Classifier owner)
        {
            RoseAttribute = roseAttribute;

            _Name = RoseAttribute.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 
            _Owner = owner;

            if (string.IsNullOrEmpty(RoseAttribute.GetPropertyValue("MetaData", "UniqueID")))
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());

            if (string.IsNullOrEmpty(RoseAttribute.GetPropertyValue("MetaData", "MetaObjectID")) ||
                RoseAttribute.GetPropertyValue("MetaData", "UniqueID") != RoseAttribute.GetUniqueID())
            {
                RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseAttribute.GetPropertyValue("MetaData", "MetaObjectID"));
            // RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseAttribute.ExportControl.Name);
            if (roseAttribute.GetPropertyValue("C#", "As Property") == "True" || _Owner is Interface)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                bool getMethod = roseAttribute.GetPropertyValue("C#", "GenerateGetOperation") == "True";
                bool setMethod = roseAttribute.GetPropertyValue("C#", "GenerateSetOperation") == "True";
                PutPropertyValue("MetaData", "Getter", getMethod);
                PutPropertyValue("MetaData", "Setter", setMethod);

            }
            else
                PutPropertyValue("MetaData", "AsProperty", false);

            PutPropertyValue("MetaData", "Documentation", RoseAttribute.Documentation);
            object nn = RoseAttribute.GetPropertyValue("Persistent", "Persistent");
            nn = RoseAttribute.Name;
            Persistent = RoseAttribute.GetPropertyValue("Persistent", "Persistent") == "True";
            PutPropertyValue("MetaData", "BackwardCompatibilityID", RoseAttribute.GetPropertyValue("C#", "Identity"));

            string wwe = RoseAttribute.ParentClass.Name;

            string persistentMemberName = RoseAttribute.GetPropertyValue("Persistent", "Member that implement");
            if (persistentMemberName == "Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);


            PutPropertyValue("MetaData", "AssociationClassRole", RoseAttribute.GetPropertyValue("Persistent", "Association Class Role"));
            OOAdvantech.MetaDataRepository.Classifier type = Type;
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            //string RoseAttribute.OverrideProperty("MetaData", "BackwardCompatibilityID")




            //Backward ID
            //Generate Backward ID
            //Generate Backward ID



        }
        public override OOAdvantech.MetaDataRepository.Classifier Type
        {
            get
            {
                if (_Type == null)
                {
                    RationalRose.RoseClass typeClass = RoseAttribute.GetTypeClass(); ;
                    OOAdvantech.MetaDataRepository.Classifier returnType = null;
                    if (typeClass != null)
                    {

                        _Type = MetaObjectMapper.FindMetaObjectFor(typeClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                        Component implementationUnit = null;
                        if (_Type == null && typeClass.GetAssignedModules().Count > 0)
                        {
                            implementationUnit = MetaObjectMapper.FindMetaObjectFor(typeClass.GetAssignedModules().GetAt(1).GetUniqueID()) as Component;
                            if (implementationUnit == null)
                                implementationUnit = new Component(typeClass.GetAssignedModules().GetAt(1));

                        }
                        //TODO υπάρχει πρόβλημα με τις stracture κλπ κλπ
                        if (_Type == null && typeClass.Stereotype == "Initerface")
                            _Type = new Interface(typeClass, implementationUnit);
                        else if (_Type == null)
                            _Type = new Class(typeClass, implementationUnit);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(RoseAttribute.Type) && RoseAttribute.Type.Trim().ToLower() != "void")
                            _Type = UnknownClassifier.GetClassifier(RoseAttribute.Type);
                        else
                            _Type = UnknownClassifier.GetClassifier(typeof(void).FullName);


                    }
                    if (_Type == null)
                        _Type = UnknownClassifier.GetClassifier(RoseAttribute.Type);


                    if (base.Type != null)
                    {
                        object obj = base.Type.FullName;
                    }
                }
                return base.Type;
            }
            set
            {
                base.Type = value;
            }
        }
    }
}
