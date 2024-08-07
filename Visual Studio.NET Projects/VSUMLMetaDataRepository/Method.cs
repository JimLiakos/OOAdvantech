using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{67ad69d0-7ead-468c-96d7-98a90c777c8c}</MetaDataID>
    public class Method : OOAdvantech.MetaDataRepository.Method, IVSUMLModelItemWrapper
    {
        /// <MetaDataID>{b9e1959a-a2ad-4ec9-950f-9c26d3214049}</MetaDataID>
        protected Method()
        {

        }

        public void Refresh()
        {

        }
        internal Method(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }

        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {

            MetaDataRepository.Operation orgOperation = (originMetaObject as MetaDataRepository.Method).Specification;
            if (VSUmlOperation == null)
            {
                string metaObjectID = GetPropertyValue<string>("MetaData", "MetaObjectID");
                if ((Owner as IVSUMLModelItemWrapper).ModelElement is IClass)
                {
                    VSUmlOperation = ((Owner as IVSUMLModelItemWrapper).ModelElement as IClass).CreateOperation();
                    VSUmlOperation.Name = orgOperation.Name;
                }
                VSUmlModel = (Owner as IVSUMLModelItemWrapper).UMLModel;
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", metaObjectID);
                MetaObjectMapper.AddTypeMap(ModelElement , this);

            }

            foreach (IParameter parameter in VSUmlOperation.OwnedParameters.ToArray())
            {
                if (parameter.Direction != ParameterDirectionKind.Return)
                    parameter.Delete();
            }

            var returnType = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgOperation.ReturnType.FullName);
            VSUmlOperation.Type = returnType.GetUMLType();
            VSUmlOperation.Visibility = VisualStudioUMLHelper.GetVisibilityKind(orgOperation.Visibility);


            foreach (var parameter in orgOperation.Parameters)
            {
                IParameter vsParameter = VSUmlOperation.CreateParameter();
                vsParameter.Name = parameter.Name;
                var parameterType = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(parameter.Type.FullName);
                vsParameter.Type = parameterType.GetUMLType();
            }
           
            //_Parameters.Clear();
            ////GetParameters Again
            //GetParameters();

            //base.Synchronize(originMetaObject);
        }


        /// <MetaDataID>{875ccecd-e690-43b6-ace5-3e6a65cb5b8a}</MetaDataID>
        internal Microsoft.VisualStudio.Uml.Classes.IOperation VSUmlOperation;

        /// <MetaDataID>{3ed14427-82c0-43bd-8f74-a3f4b51f739e}</MetaDataID>
        Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{0b5c21aa-4086-4696-b652-6a5ba0a5fe4d}</MetaDataID>
        public Method(MetaDataRepository.Operation operation, Microsoft.VisualStudio.Uml.Classes.IOperation vsUmlOperation, Class owner, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
            : base(operation)
        {
            // TODO: Complete member initialization
            _Owner = owner;
            VSUmlOperation = vsUmlOperation;
            

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

            VSUmlModel = iModel;

            Visibility = VisualStudioUMLHelper.GetVisibilityKind(vsUmlOperation.Visibility);


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
            MetaObjectMapper.AddTypeMap(VSUmlOperation as ModelElement, this);


        }
        /// <MetaDataID>{61f2a45e-696c-49d7-945e-1f1349f030f8}</MetaDataID>
        public override string Name
        {
            get
            {
                if (_Name != Specification.Name)
                    _Name = Specification.Name;

                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <MetaDataID>{8f76d564-4cbb-42ae-a024-4b4445499956}</MetaDataID>
        public Method(MetaDataRepository.Operation operation, Microsoft.VisualStudio.Uml.Classes.IOperation vsUmlOperation, Structure owner)
            : base(operation)
        {
            // TODO: Complete member initialization
            _Owner = owner;
            VSUmlOperation = vsUmlOperation;
            MetaObjectMapper.AddTypeMap(VSUmlOperation as ModelElement, this);
        }

        /// <MetaDataID>{e59039d7-1be6-4915-a92c-c7530fb8dad6}</MetaDataID>
        public ModelElement ModelElement
        {
            get
            {
                return VSUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }

        /// <MetaDataID>{234626d0-3b60-42d8-a355-a3040045fd74}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
    }
}
