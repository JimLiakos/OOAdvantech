namespace RoseMetaDataRepository
{
    /// <MetaDataID>{4DB0B1F8-D74B-4910-A591-D9EDE4092D76}</MetaDataID>
    internal class AttributeRealization : OOAdvantech.MetaDataRepository.AttributeRealization
    {
        internal RationalRose.RoseAttribute RoseAttribute;
        internal AttributeRealization()
        {
        }

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
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }


        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                base.Synchronize(OriginMetaObject);

            _Name = OriginMetaObject.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 
            if (_Owner == null)
                _Owner = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as OOAdvantech.MetaDataRepository.Feature).Owner, this) as OOAdvantech.MetaDataRepository.Classifier;


            if (RoseAttribute!= null)
            {
                RoseAttribute.Name = _Name;
                RoseAttribute.Type= MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName( (OriginMetaObject as OOAdvantech.MetaDataRepository.StructuralFeature ).Type));
                RoseAttribute.Documentation = OriginMetaObject.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
            }
            else
            {
                string typeFullName = null;
                OOAdvantech.MetaDataRepository.Classifier type = (OriginMetaObject as OOAdvantech.MetaDataRepository.StructuralFeature).Type;
                if (type == null)
                    typeFullName = (OriginMetaObject as OOAdvantech.MetaDataRepository.StructuralFeature).ParameterizedType.FullName;
                else
                    typeFullName = type.FullName;
                
                if (_Owner is Interface)
                    RoseAttribute= (_Owner as Interface).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(typeFullName),"");
                if (_Owner is Class)
                    RoseAttribute = (_Owner as Class).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(typeFullName),"");

                RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", Identity.ToString());
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());
                
            }
 
        }
 
        public AttributeRealization(RationalRose.RoseAttribute roseAttribute, OOAdvantech.MetaDataRepository.Classifier owner, OOAdvantech.MetaDataRepository.Attribute specification)
        {
            RoseAttribute = roseAttribute;

            _Name = RoseAttribute.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 
            _Owner = owner;
            _Specification = specification;
            if (string.IsNullOrEmpty(RoseAttribute.GetPropertyValue("MetaData", "UniqueID")))
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());

            if (string.IsNullOrEmpty(RoseAttribute.GetPropertyValue("MetaData", "MetaObjectID")) ||
                RoseAttribute.GetPropertyValue("MetaData", "UniqueID") != RoseAttribute.GetUniqueID())
            {
                RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseAttribute.GetPropertyValue("MetaData", "MetaObjectID"));
          //  RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseAttribute.ExportControl.Name);
            Persistent = RoseAttribute.GetPropertyValue("Persistent", "Persistent") == "True";
            PutPropertyValue("MetaData", "BackwardCompatibilityID", RoseAttribute.GetPropertyValue("C#", "Identity"));
            PutPropertyValue("MetaData", "Documentation", RoseAttribute.Documentation);
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");

            string persistentMemberName = RoseAttribute.GetPropertyValue("Persistent", "Member that implement");
            if (persistentMemberName == "Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);
        }


    }
}
