using MetaDataRepository=OOAdvantech.MetaDataRepository;
namespace RoseMetaDataRepository
{

    /// <MetaDataID>{8B7ACB1F-0E2D-43E7-927C-FF34765F994D}</MetaDataID>
    internal class Method : OOAdvantech.MetaDataRepository.Method
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
                if (RoseOperation!= null)
                    RoseOperation.OverrideProperty("MetaData", "MetaObjectID", PropertyValue as string);
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

        internal Method()
        {

        }
        internal RationalRose.RoseOperation RoseOperation;
        public Method(MetaDataRepository.Operation operation, RationalRose.RoseOperation roseOperation, MetaDataRepository.Classifier owner)
            : base(operation)
        {
            _Owner = owner;
            RoseOperation = roseOperation;
            _Name = roseOperation.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 

            if (string.IsNullOrEmpty(RoseOperation.GetPropertyValue("MetaData", "UniqueID")))
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());

            if (string.IsNullOrEmpty(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID")) ||
                RoseOperation.GetPropertyValue("MetaData", "UniqueID") != RoseOperation.GetUniqueID())
            {
                RoseOperation.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID"));
           // RoseOperation.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseOperation.ExportControl.Name);


        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            
            base.Synchronize(OriginMetaObject);

            if (_Owner == null)
                _Owner = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((OriginMetaObject as MetaDataRepository.Method).Owner, this) as MetaDataRepository.Classifier;
            OOAdvantech.MetaDataRepository.Operation originOperation;
            if ((OriginMetaObject as MetaDataRepository.Method) != null)
                originOperation = (OriginMetaObject as MetaDataRepository.Method).Specification;
            else
                originOperation = OriginMetaObject as MetaDataRepository.Operation;



            if (RoseOperation != null)
            {
                try
                {
                    RoseOperation.Name = _Name;
                    RoseOperation.ReturnType = MetaObjectMapper.GetShortNameFor(originOperation.ReturnType.FullName);

                }
                catch (System.Exception error)
                {
                    throw;

                }
            }
            else
            {



                if (this.Specification!=null&&Operation.GetSignature(this.Specification) == Operation.GetSignature(originOperation))
                    return;


                if (_Owner is Interface)
                    RoseOperation = (_Owner as Interface).RoseClass.AddOperation(_Name, MetaObjectMapper.GetShortNameFor((OriginMetaObject as Method).Specification.ReturnType.FullName));
                if (_Owner is Class)
                {
                    var method = (OriginMetaObject as MetaDataRepository.Method);
                    var specification = (OriginMetaObject as MetaDataRepository.Method).Specification;
                    RoseOperation = (_Owner as Class).RoseClass.AddOperation(_Name, MetaObjectMapper.GetShortNameFor((OriginMetaObject as MetaDataRepository.Method).Specification.ReturnType.FullName));
                }

                RoseOperation.OverrideProperty("MetaData", "MetaObjectID", Identity.ToString());
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());
            }
            int i = 0;
            RoseOperation.RemoveAllParameters();
            foreach (MetaDataRepository.Parameter parameter in originOperation.Parameters)
            {
                MetaDataRepository.Classifier parameterType = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(parameter.Type, this) as MetaDataRepository.Classifier;
                if (parameterType == null)
                    parameterType = UnknownClassifier.GetClassifier(parameter.Type.FullName);
                
                if (RoseOperation.Parameters.Count > i)
                {
                    RoseOperation.Parameters.GetAt((short)(i + 1)).Name = parameter.Name;
                    RoseOperation.Parameters.GetAt((short)(i + 1)).Type = MetaObjectMapper.GetShortNameFor(parameter.Type.FullName);
                }
                else
                {
                    RoseOperation.AddParameter(parameter.Name, MetaObjectMapper.GetShortNameFor(parameter.Type.FullName), "", (short)i);
                }
                i++;
            }


        }
    }
}
