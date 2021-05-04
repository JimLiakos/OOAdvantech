using MetaDataRepository=OOAdvantech.MetaDataRepository;
namespace RoseMetaDataRepository
{
    /// <MetaDataID>{3D1345EF-4D92-4E58-9B25-6944138EF7EB}</MetaDataID>
    public class Operation : OOAdvantech.MetaDataRepository.Operation
    {
        RationalRose.RoseOperation RoseOperation;

        public Operation(RationalRose.RoseOperation operation,MetaDataRepository.Classifier owner)
        {
            _Name = operation.Name;
            _Owner = owner;
            RoseOperation = operation;

            if (string.IsNullOrEmpty(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID")))
                RoseOperation.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID"));
            RoseOperation.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseOperation.ExportControl.Name);

        }





        public override OOAdvantech.MetaDataRepository.Classifier ReturnType
        {
            get
            {
                if (_ReturnType == null)
                {



                    RationalRose.RoseClass typeClass = RoseOperation.GetResultClass(); ;
                    OOAdvantech.MetaDataRepository.Classifier returnType = null;

                    if (typeClass != null)
                    {

                        returnType = MetaObjectMapper.FindMetaObjectFor(typeClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                        Component implementationUnit = null;
                        if (returnType == null && typeClass.GetAssignedModules().Count > 0)
                        {
                            implementationUnit = MetaObjectMapper.FindMetaObjectFor(typeClass.GetAssignedModules().GetAt(1).GetUniqueID()) as Component;
                            if (implementationUnit == null)
                                implementationUnit = new Component(typeClass.GetAssignedModules().GetAt(1));

                        }
                        //TODO υπάρχει πρόβλημα με τις stracture κλπ κλπ
                        if (returnType == null && typeClass.Stereotype == "Initerface")
                            returnType = new Interface(typeClass, implementationUnit);
                        else if (returnType == null)
                            returnType = new Class(typeClass, implementationUnit);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(RoseOperation.ReturnType) && RoseOperation.ReturnType.Trim().ToLower() != "void")
                            returnType = UnknownClassifier.GetClassifier(RoseOperation.ReturnType);
                        else
                            returnType = UnknownClassifier.GetClassifier(typeof(void).FullName);
                        

                    }
                    if (returnType != null)
                        _ReturnType = returnType;
                    else
                        returnType = UnknownClassifier.GetClassifier(RoseOperation.ReturnType);


                }
                
                return base.ReturnType;
            }
            set
            {
                base.ReturnType = value;
            }
        }
        bool IsParametersLoaded = false;
        public override OOAdvantech.MetaDataRepository.MetaObjectCollection Parameters
        {
            get
            {
                
                try
                {
                    for (int i = 0; i < RoseOperation.Parameters.Count; i++)
                    {
                        RationalRose.RoseParameter roseParameter = RoseOperation.Parameters.GetAt((short)(i + 1));
                        RationalRose.RoseClass typeClass = roseParameter.GetTypeClass();
                        OOAdvantech.MetaDataRepository.Classifier parameterType = null;

                        if (typeClass != null)
                        {

                            parameterType = MetaObjectMapper.FindMetaObjectFor(typeClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                            Component implementationUnit = null;
                            if (parameterType ==null && typeClass.GetAssignedModules().Count > 0)
                            {
                                implementationUnit = MetaObjectMapper.FindMetaObjectFor(typeClass.GetAssignedModules().GetAt(1).GetUniqueID()) as Component;
                                if (implementationUnit == null)
                                    implementationUnit = new Component(typeClass.GetAssignedModules().GetAt(1));

                            }
                            //TODO υπάρχει πρόβλημα με τις stracture κλπ κλπ
                            if (parameterType == null && typeClass.Stereotype == "Initerface")
                                parameterType = new Interface(typeClass, implementationUnit);
                            else if (parameterType == null)
                                parameterType = new Class(typeClass, implementationUnit);

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(roseParameter.Type) && roseParameter.Type.Trim().ToLower() != "void")
                                parameterType = UnknownClassifier.GetClassifier(roseParameter.Type);
                        }
                        _Parameters.Add(new MetaDataRepository.Parameter(roseParameter.Name, parameterType, i));

                    }
                    IsParametersLoaded=true;
                }
                finally
                {
                    if (!IsParametersLoaded)
                        _Parameters.RemoveAll();
                }


                return base.Parameters;
            }
        }
    }
}
