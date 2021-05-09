using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{b9b5487c-13ab-45f9-8f2f-503bd63bc50b}</MetaDataID>
    public class Operation : OOAdvantech.MetaDataRepository.Operation, IVSUMLModelItemWrapper
    {
        /// <MetaDataID>{b47807c1-6e65-428a-b8dc-fa36ef4bd2e2}</MetaDataID>
        protected Operation()
        {

        }
        public void Refresh()
        {

        }

        /// <MetaDataID>{be3f091d-4a79-40f3-8a9d-93638a49bbd7}</MetaDataID>
        static internal string GetSignature(IOperation operation,Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel umlModel)
        {
            //TODO πως λειτουργεί στη vb που δεν είναι case sensitive
            string methodSignature = null;
            foreach (var vsUmlparameter in operation.OwnedParameters)
            {
                if (vsUmlparameter.Direction == ParameterDirectionKind.Return)
                    continue;

                if (methodSignature == null)
                    methodSignature = operation.Name.Trim() + "(";
                else
                    methodSignature += ",";
                if (vsUmlparameter.Type != null)
                {
                    OOAdvantech.MetaDataRepository.Classifier classifier = VisualStudioUMLHelper.GetClassifierFor(vsUmlparameter.Type, umlModel);
                    if (classifier == null)
                        classifier = UnknownClassifier.GetClassifier(typeof(void).FullName);
                    methodSignature += classifier.FullName.Trim();
                }
                else
                    methodSignature += vsUmlparameter.Type.Name.Trim();
            }
            if (methodSignature == null)
                methodSignature = operation.Name.Trim() + "()";
            else
                methodSignature += ")";
            if (operation.Type != null)
                return MetaObjectMapper.GetShortNameFor(operation.Type.Name).Trim() + " " + methodSignature;
            else
                return MetaObjectMapper.GetShortNameFor(typeof(void).FullName).Trim() + " " + methodSignature;
            

            //return methodSignature;
        }

        /// <MetaDataID>{05fa44e5-cc48-4edc-afe0-9939421454cc}</MetaDataID>
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
        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {

            MetaDataRepository.Operation orgOperation = originMetaObject as MetaDataRepository.Operation;
            if (VSUMLOperation == null)
            {
                string metaObjectID = GetPropertyValue<string>("MetaData", "MetaObjectID");
                if ((Owner as IVSUMLModelItemWrapper).ModelElement is IInterface)
                {
                    VSUMLOperation = ((Owner as IVSUMLModelItemWrapper).ModelElement as IInterface).CreateOperation();
                    VSUMLOperation.Name = orgOperation.Name;
                }
                else if ((Owner as IVSUMLModelItemWrapper).ModelElement is IClass)
                {
                    VSUMLOperation = ((Owner as IVSUMLModelItemWrapper).ModelElement as IClass).CreateOperation();
                    VSUMLOperation.Name = orgOperation.Name;
                }
                VSUmlModel = (Owner as IVSUMLModelItemWrapper).UMLModel;
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", metaObjectID);
                MetaObjectMapper.AddTypeMap(ModelElement, this);

            }

            foreach (IParameter parameter in VSUMLOperation.OwnedParameters.ToArray())
                parameter.Delete();

            _ReturnType = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgOperation.ReturnType.FullName);
            VSUMLOperation.Type = _ReturnType.GetUMLType();
            VSUMLOperation.Visibility = VisualStudioUMLHelper.GetVisibilityKind(orgOperation.Visibility);

     
            foreach (var parameter in orgOperation.Parameters)
            {
                IParameter vsParameter = VSUMLOperation.CreateParameter();
                vsParameter.Name = parameter.Name;
                var parameterType=(Owner as IVSUMLModelItemWrapper).UMLModel.GetType(parameter.Type.FullName) ;
                vsParameter.Type = parameterType.GetUMLType();
            }
            _Parameters.Clear();
            //GetParameters Again
            GetParameters();

            //base.Synchronize(originMetaObject);
        }

        internal Operation(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }

        /// <MetaDataID>{5d3de602-ebfd-4421-8b38-93ed45a88000}</MetaDataID>
       internal IOperation VSUMLOperation;
        /// <MetaDataID>{1d4dc8de-b70d-4b2c-9881-0480a93a0c4f}</MetaDataID>
        Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{25f61ba4-2bc3-4172-bcd7-0317c6db7d56}</MetaDataID>
        public Operation(IOperation operation, MetaDataRepository.Classifier owner, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
            _Name = operation.Name;
            if (_Name != null)
                _Name = _Name.Trim();

            _Owner = owner;
            VSUMLOperation = operation;

    
            string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (!string.IsNullOrEmpty(metaObjectID))
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
            else
            {
                _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", ModelElement.Id.ToString());
            }
            PutPropertyValue("MetaData", "MetaObjectID", _Identity.ToString());
            //_Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
            Visibility =VisualStudioUMLHelper.GetVisibilityKind(operation.Visibility);
           // PutPropertyValue("MetaData", "Documentation", RoseOperation.Documentation);


            if (ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Sealed") == true.ToString())
                OverrideKind = MetaDataRepository.OverrideKind.Sealed;
            if (ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Abstract") == true.ToString())
                OverrideKind = MetaDataRepository.OverrideKind.Abstract;
            if (ModelElement.GetExtensionData().GetPropertyValue("MetaData", "New") == true.ToString())
                OverrideKind = MetaDataRepository.OverrideKind.New;
            if (ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Override") == true.ToString())
                OverrideKind = MetaDataRepository.OverrideKind.Override;
            if (ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Virtual") == true.ToString())
                OverrideKind = MetaDataRepository.OverrideKind.Virtual;
            
            var pars = Parameters;
            var ret = ReturnType;
            MetaObjectMapper.AddTypeMap(VSUMLOperation as ModelElement, this);


        }

        /// <MetaDataID>{46b6fcf9-4c10-41df-9575-1111083e9aab}</MetaDataID>
        bool IsParametersLoaded;



        /// <MetaDataID>{22ec4fc2-8b8e-4640-95f4-541e92e6024e}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier ReturnType
        {
            get
            {
                //VSUMLOperation.cre
                //IClass df;
                //df.CreateOperation

                //Microsoft.VisualStudio.Modeling.Design.ElementTypeDescriptor.
             
                IType typeClass = VSUMLOperation.Type;
               _ReturnType= VisualStudioUMLHelper.GetClassifierFor(typeClass, UMLModel);
               if (_ReturnType != null)
               {
                   object obj = _ReturnType.FullName;
               }
               else
                   _ReturnType = UnknownClassifier.GetClassifier(typeof(void).FullName);



                return base.ReturnType;
            }
            set
            {
                if (value != null)
                    VSUMLOperation.Type = value.GetUMLType();
                else
                    VSUMLOperation.Type = null;

              
                base.ReturnType = value;
            }
        }



        /// <MetaDataID>{19b4f1fe-7300-43da-8fc6-bd3dfcab0e19}</MetaDataID>
        public override string Name
        {
            get
            {
                _Name = VSUMLOperation.Name;
                return base.Name;
            }
            set
            {
                base.Name = value;
                VSUMLOperation.Name = value;
            }
        }
      

        /// <MetaDataID>{df924592-0336-4eea-81e9-f94550b0a8be}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<MetaDataRepository.Parameter> Parameters
        {
            get
            {

                try
                {
                    GetParameters();
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

        private void GetParameters()
        {
            if (_Parameters != null && VSUMLOperation != null && VSUMLOperation.GetNumOfParameters() != _Parameters.Count)
            {
                _Parameters.Clear();
                IsParametersLoaded = false;
            }

            //if ((_Parameters != null && _Parameters.Count > 0) || VSUMLOperation == null || IsParametersLoaded)
            //    return base.Parameters;
            int i = 0;
            foreach (IParameter vsUmlparameter in VSUMLOperation.OwnedParameters)
            {
                if (vsUmlparameter.Direction == ParameterDirectionKind.Return)
                    continue;

                //RationalRose.RoseParameter roseParameter = RoseOperation.Parameters.GetAt((short)(i + 1));
                IType typeClass = vsUmlparameter.Type;

                OOAdvantech.MetaDataRepository.Classifier parameterType = null;

                if (typeClass is IClassifier)
                {
                    parameterType = VisualStudioUMLHelper.GetClassifierFor(typeClass as IClassifier, UMLModel);
                }
                else
                {
                    if (typeClass != null)
                    {
                        parameterType = MetaObjectMapper.FindMetaObjectFor(typeClass as ModelElement) as OOAdvantech.MetaDataRepository.Classifier;
                        if (parameterType == null)
                            parameterType = UnknownClassifier.GetClassifier(typeClass);
                    }
                }
                if (_Parameters.Count > i)
                {
                    _Parameters[i].Name = vsUmlparameter.Name;
                    _Parameters[i].Type = parameterType;
                }
                else
                    _Parameters.Add(new Parameter(vsUmlparameter, UMLModel));
                i++;
                if (parameterType != null)
                {
                    object obj = parameterType.FullName;
                }

            }
            IsParametersLoaded = true;
        }
        /// <MetaDataID>{074d133f-cdb7-413b-af59-dabf5bdac05e}</MetaDataID>
        public ModelElement ModelElement
        {
            get 
            {
                return VSUMLOperation as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }
        /// <MetaDataID>{b6a3eb28-125d-4c13-a1a7-d600f0ac5b97}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get 
            {
                return VSUmlModel;
            }
        }
    }
}
