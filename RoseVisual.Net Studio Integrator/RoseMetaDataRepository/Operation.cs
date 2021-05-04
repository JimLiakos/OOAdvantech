using MetaDataRepository = OOAdvantech.MetaDataRepository;
namespace RoseMetaDataRepository
{
    /// <MetaDataID>{3D1345EF-4D92-4E58-9B25-6944138EF7EB}</MetaDataID>
    internal class Operation : OOAdvantech.MetaDataRepository.Operation
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
                if (RoseOperation != null)
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

        static internal string GetSignature(RationalRose.RoseOperation method)
        {
            //TODO πως λειτουργεί στη vb που δεν είναι case sensitive
            string methodSignature = null;
            for (int i = 0; i < method.Parameters.Count; i++)
            {
                RationalRose.RoseParameter roseParameter = method.Parameters.GetAt((short)(i + 1));
                if (methodSignature == null)
                    methodSignature = method.Name.Trim() + "(";
                else
                    methodSignature += ",";
                if (roseParameter.GetTypeClass() != null)
                {
                    OOAdvantech.MetaDataRepository.Classifier classifier = MetaObjectMapper.FindMetaObjectFor(roseParameter.GetTypeClass().GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                    if (classifier == null)
                    {
                        Component component = null;
                        if (roseParameter.GetTypeClass().GetAssignedModules().Count > 0)
                        {
                            component = MetaObjectMapper.FindMetaObjectFor(roseParameter.GetTypeClass().GetAssignedModules().GetAt(1).GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseParameter.GetTypeClass().GetAssignedModules().GetAt(1));
                        }
                        if (roseParameter.GetTypeClass().Stereotype == "Interface")
                            classifier = new Interface(roseParameter.GetTypeClass(), component);
                        else
                            classifier = new Class(roseParameter.GetTypeClass(), component);

                    }
                    methodSignature += classifier.FullName.Trim();




                }
                else
                {
                    methodSignature += roseParameter.Type.Trim();
                }
            }
            if (methodSignature == null)
                methodSignature = method.Name.Trim() + "()";
            else
                methodSignature += ")";
            if (method.ReturnType != null)
                return MetaObjectMapper.GetShortNameFor(method.ReturnType).Trim() + " " + methodSignature;

            return methodSignature;
        }

        static internal string GetSignature(OOAdvantech.MetaDataRepository.Operation operation)
        {

            string operationSignature = null;
            foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
            {
                if (parameter.Type == null)
                    parameter.Type = UnknownClassifier.GetClassifier("void");
                if (operationSignature == null)
                    operationSignature = operation.Name.Trim() + "(";
                else
                    operationSignature += ",";
                operationSignature += MetaObjectMapper.GetShortNameFor(parameter.Type.FullName).Trim();
            }
            if (operationSignature == null)
                operationSignature = operation.Name.Trim() + "()";
            else
                operationSignature += ")";
            if (operation.ReturnType != null)
                return MetaObjectMapper.GetShortNameFor(operation.ReturnType.FullName).Trim() + " " + operationSignature;
            return operationSignature;


        }


        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {

            int i = 0;
            long parmsCount = Parameters.Count;
            OOAdvantech.MetaDataRepository.Operation originalOperation = null;
            if (originMetaObject is MetaDataRepository.Method)
            {
                originalOperation = (originMetaObject as MetaDataRepository.Method).Specification;

                _Name = originMetaObject.Name;
                _OverrideKind = (originMetaObject as MetaDataRepository.Method).OverrideKind;

                OOAdvantech.MetaDataRepository. ContainedItemsSynchronizer ParameterSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originalOperation.Parameters, _Parameters, this);
                ParameterSynchronizer.FindModifications();
                ParameterSynchronizer.ExecuteAddCommand();
                ParameterSynchronizer.ExecuteDeleteCommand();
                ParameterSynchronizer.Synchronize();

                if (originalOperation.ReturnType != null)
                {
                    _ReturnType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originalOperation.ReturnType, this) as MetaDataRepository.Classifier;

                    if (_ReturnType == null)
                    {
                        //TODO να τσεκαριστή με test case αν το return type θα έχει σωστό namespace και η διαδικασία συχρονισμού δέν θα προχωρίσει 
                        _ReturnType = (MetaDataRepository.Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originalOperation.ReturnType, this);
                        if (_ReturnType.FullName != originalOperation.ReturnType.FullName)
                        {
                            _ReturnType.Name = originalOperation.ReturnType.Name;
                            _ReturnType.ShallowSynchronize(originalOperation.ReturnType);
                        }
                    }
                }
            }
            else
            {
                i = 0;
                originalOperation = originMetaObject as MetaDataRepository.Operation;
                
                if (RoseOperation != null && GetSignature(this) == GetSignature(originalOperation))
                {
                    base.Synchronize(originMetaObject);
                    RoseOperation.Documentation = originalOperation.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;


                    foreach (MetaDataRepository.Parameter parameter in originalOperation.Parameters)
                    {
                        RoseOperation.Parameters.GetAt((short)(i + 1)).Documentation = parameter.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
                        i++;
                    }

                    return;
                }
                base.Synchronize(originMetaObject);
            }
            // base.Synchronize(originalOperation);


            //_ReturnType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginOperation.ReturnType, this) as MetaDataRepository.Classifier;

            if (originalOperation.ReturnType != null && _ReturnType.FullName != RoseVisualStudioBridge.GetTypeFullName(originalOperation.ReturnType))
            {
                //TODO να τσεκαριστή με test case αν το return type θα έχει σωστό namespace και η διαδικασία συχρονισμού δέν θα προχωρίσει 
                _ReturnType = (OOAdvantech.MetaDataRepository.Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originalOperation.ReturnType, this);
                if (_ReturnType.FullName != RoseVisualStudioBridge.GetTypeFullName(originalOperation.ReturnType))
                {

                    _ReturnType.ShallowSynchronize(originalOperation.ReturnType);
                    _ReturnType.Name = MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeName(originalOperation.ReturnType));
                }
            }

            if (ReturnType != null && RoseOperation != null)
                RoseOperation.ReturnType = MetaObjectMapper.GetShortNameFor(ReturnType.FullName);



            if (_Owner == null)
                _Owner = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originMetaObject as MetaDataRepository.Feature).Owner, this) as MetaDataRepository.Classifier;


            if (RoseOperation != null)
            {
                RoseOperation.Name = _Name;
                RoseOperation.ReturnType = MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(ReturnType));
            }
            else
            {
                if (_Owner is Interface)
                    RoseOperation = (_Owner as Interface).RoseClass.AddOperation(_Name, MetaObjectMapper.GetShortNameFor(originalOperation.ReturnType.FullName));
                if (_Owner is Class)
                    RoseOperation = (_Owner as Class).RoseClass.AddOperation(_Name, MetaObjectMapper.GetShortNameFor(originalOperation.ReturnType.FullName));
                if (_Owner is Structure)
                    RoseOperation = (_Owner as Structure).RoseClass.AddOperation(_Name, MetaObjectMapper.GetShortNameFor(originalOperation.ReturnType.FullName));

                RoseOperation.OverrideProperty("MetaData", "MetaObjectID", MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(originalOperation).ToString());
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());
            }

            i = 0;
            _Parameters.RemoveAll();
            RoseOperation.RemoveAllParameters();
            foreach (MetaDataRepository.Parameter parameter in originalOperation.Parameters)
            {
                MetaDataRepository.Classifier parameterType = null;
                if (parameter.Type != null)
                {
                    parameterType = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(parameter.Type, this) as MetaDataRepository.Classifier;
                    if (parameterType == null)
                        parameterType = UnknownClassifier.GetClassifier(RoseVisualStudioBridge.GetTypeFullName(parameter.Type));
                }
                if (parameterType == null)
                    parameterType = UnknownClassifier.GetClassifier("void");



                _Parameters.Add(new OOAdvantech.MetaDataRepository.Parameter(parameter.Name, parameterType));
                if (RoseOperation.Parameters.Count > i)
                {
                    RoseOperation.Parameters.GetAt((short)(i + 1)).Name = parameter.Name;
                    if (parameter.Type != null)
                        RoseOperation.Parameters.GetAt((short)(i + 1)).Type = MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(parameter.Type));
                    else
                        RoseOperation.Parameters.GetAt((short)(i + 1)).Type = "";
                    RoseOperation.Parameters.GetAt((short)(i + 1)).Documentation = parameter.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;

                }
                else
                {
                    if (parameter.Type != null)
                        RoseOperation.AddParameter(parameter.Name, MetaObjectMapper.GetShortNameFor(RoseVisualStudioBridge.GetTypeFullName(parameter.Type)), "", (short)i).Documentation = parameter.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;
                    else
                        RoseOperation.AddParameter(parameter.Name, "", "", (short)i).Documentation = parameter.GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;

                }
                i++;
            }

            RoseOperation.ExportControl.Name = RoseAccessTypeConverter.GetExportControl(Visibility);
            RoseOperation.Documentation = GetPropertyValue(typeof(string), "MetaData", "Documentation") as string;


        }

        RationalRose.RoseModel RoseModel;
        internal Operation(RationalRose.RoseModel roseModel)
        {
            RoseModel = roseModel;
        }
        internal RationalRose.RoseOperation RoseOperation;

        public Operation(RationalRose.RoseOperation operation, MetaDataRepository.Classifier owner)
        {
            _Name = operation.Name;
            if (_Name != null)
                _Name = _Name.Trim();

            _Owner = owner;
            RoseOperation = operation;

            if (string.IsNullOrEmpty(RoseOperation.GetPropertyValue("MetaData", "UniqueID")))
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());

            if (string.IsNullOrEmpty(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID")) ||
                RoseOperation.GetPropertyValue("MetaData", "UniqueID") != RoseOperation.GetUniqueID())
            {
                RoseOperation.OverrideProperty("MetaData", "MetaObjectID", "{" + System.Guid.NewGuid().ToString() + "}");
                RoseOperation.OverrideProperty("MetaData", "UniqueID", RoseOperation.GetUniqueID());
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseOperation.GetPropertyValue("MetaData", "MetaObjectID"));
            //RoseOperation.OverrideProperty("MetaData", "MetaObjectID", _Identity.ToString());

            Visibility = RoseAccessTypeConverter.GetVisibilityKind(RoseOperation.ExportControl.Name);
            PutPropertyValue("MetaData", "Documentation", RoseOperation.Documentation);
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
                    if (base.ReturnType != null)
                    {
                        object obj = base.ReturnType.FullName;
                    }
                }
                //string tmp = RoseOperation.GetResultClass().ParentCategory.Name;
                return base.ReturnType;
            }
            set
            {
                base.ReturnType = value;
            }
        }
        bool IsParametersLoaded = false;
        public override OOAdvantech.Collections.Generic.Set<MetaDataRepository.Parameter> Parameters
        {
            get
            {

                try
                {
                    if ((_Parameters != null && _Parameters.Count > 0) || RoseOperation == null || IsParametersLoaded)
                        return base.Parameters;
                    for (int i = 0; i < RoseOperation.Parameters.Count; i++)
                    {
                        RationalRose.RoseParameter roseParameter = RoseOperation.Parameters.GetAt((short)(i + 1));
                        RationalRose.RoseClass typeClass = roseParameter.GetTypeClass();
                        OOAdvantech.MetaDataRepository.Classifier parameterType = null;

                        if (typeClass != null)
                        {

                            parameterType = MetaObjectMapper.FindMetaObjectFor(typeClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                            Component implementationUnit = null;
                            if (parameterType == null && typeClass.GetAssignedModules().Count > 0)
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
                        _Parameters.Add(new MetaDataRepository.Parameter(roseParameter.Name, parameterType));
                        if (parameterType != null)
                        {
                            object obj = parameterType.FullName;
                        }

                    }
                    IsParametersLoaded = true;
                }
                catch (System.Exception error)
                {
                    throw;
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
