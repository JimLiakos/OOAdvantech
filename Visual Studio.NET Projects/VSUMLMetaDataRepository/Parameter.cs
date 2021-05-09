using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
namespace OOAdvantech.VSUMLMetaDataRepository
{

    /// <MetaDataID>{0FBB55AE-331E-4385-A7A2-C92D4B5A10A1}</MetaDataID>
    public class Parameter : MetaDataRepository.Parameter, IVSUMLModelItemWrapper
    {
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{9100fc74-6d8f-4e2b-be62-77a74a2af884}</MetaDataID>
      internal  IParameter VSUmlparameter;
        /// <MetaDataID>{b7efd59b-332b-45e0-8749-939bc4a59fd9}</MetaDataID>
        public Parameter(IParameter vsUmlparameter, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlparameter = vsUmlparameter;
            VSUmlModel = iModel;
            _Name = vsUmlparameter.Name;

        }

        public void Refresh()
        {

        }
 

        public override MetaDataRepository.Classifier Type
        {
            get
            {
                //  if (_Type == null)
                //{
                IType typeClass = VSUmlparameter.Type;
                if (typeClass is IClassifier)
                {
                    _Type = VisualStudioUMLHelper.GetClassifierFor(typeClass as IClassifier, UMLModel);
                }
                else
                {
                    if (typeClass == null)
                    {
                        _Type = UnknownClassifier.GetClassifier(typeof(void).FullName);
                    }
                    else
                    {
                        _Type = MetaObjectMapper.FindMetaObjectFor(typeClass as ModelElement) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Type == null)
                            _Type = UnknownClassifier.GetClassifier(typeClass);
                    }
                }

                if (_Type != null)
                {
                    object obj = _Type.FullName;
                }
                //}

                return base.Type;
            }
            set
            {
                if (value != null)
                    VSUmlparameter.Type = value.GetUMLType();

                base.Type = value;
            }
        }

        public override string Name
        {
            get
            {
                if (_Name != VSUmlparameter.Name)
                    _Name = VSUmlparameter.Name;
                return base.Name;
            }
            set
            {
                VSUmlparameter.Name = value;
                base.Name = value;
            }
        }

        /// <MetaDataID>{51509bf7-8d13-40af-8c1b-9537f6ab495a}</MetaDataID>
        public ModelElement ModelElement
        {
            get
            {
                return VSUmlparameter as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }
        /// <MetaDataID>{87ca37e2-d42c-4617-9807-47566660e3f0}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
    }
}
