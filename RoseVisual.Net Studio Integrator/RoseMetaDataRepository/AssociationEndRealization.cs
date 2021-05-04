namespace RoseMetaDataRepository
{
    /// <MetaDataID>{3A7F801D-8A42-4A34-89EF-422C91AC8D7F}</MetaDataID>
    internal class AssociationEndRealization : OOAdvantech.MetaDataRepository.AssociationEndRealization
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
                    base.PutPropertyValue(propertyNamespace, propertyName, identity);
                }
                return identity;
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }

        internal RationalRose.RoseAttribute RoseAttribute;
        internal AssociationEndRealization()
        {
        }


        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (!(OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization))
                return;

            base.Synchronize(OriginMetaObject);

            if (_Owner == null)
                _Owner = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as OOAdvantech.MetaDataRepository.Feature).Owner, this) as OOAdvantech.MetaDataRepository.Classifier;


            if (RoseAttribute!= null)
            {
                RoseAttribute.Name = _Name;
                OOAdvantech.MetaDataRepository.Classifier classifier = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.CollectionClassifier;
                if (classifier == null)
                    classifier = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.Specification;
                
                RoseAttribute.Type= MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(classifier));
            }
            else
            {
                OOAdvantech.MetaDataRepository.Classifier classifier = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.CollectionClassifier;
                if (classifier == null)
                    classifier = (OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.Specification;
                

                if (_Owner is Interface)
                    RoseAttribute= (_Owner as Interface).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(classifier)),"");
                if (_Owner is Class)
                    RoseAttribute = (_Owner as Class).RoseClass.AddAttribute(_Name, MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(classifier)),"");

                RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", Identity.ToString());
                RoseAttribute.OverrideProperty("MetaData", "UniqueID", RoseAttribute.GetUniqueID());
            }
 
        }

        public AssociationEndRealization(RationalRose.RoseAttribute roseAttribute, OOAdvantech.MetaDataRepository.Classifier owner, OOAdvantech.MetaDataRepository.AssociationEnd specification)
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
           // RoseAttribute.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

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

            string persistentMemberName = RoseAttribute.GetPropertyValue("Persistent", "Member that implement");
            if (persistentMemberName == "Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);


            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseAttribute.ExportControl.Name);
            _Persistent = RoseAttribute.GetPropertyValue("Persistent", "Persistent") == "True";
            PutPropertyValue("MetaData", "BackwardCompatibilityID", RoseAttribute.GetPropertyValue("C#", "Identity"));
            object rr = RoseAttribute.GetPropertyValue("Persistent", "Persistent");
            object ghgh = RoseAttribute.ParentClass.Name;
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");

            


        }

    }
}
