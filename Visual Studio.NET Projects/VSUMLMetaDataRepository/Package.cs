using Microsoft.VisualStudio.Uml.Classes;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{6d734d60-437e-410c-92c7-4080a9e9b02b}</MetaDataID>
    public class Package : OOAdvantech.MetaDataRepository.Package, IVSUMLModelItemWrapper
    {

        public void Refresh()
        {

        }

        protected Package()
        {
              
        }
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        public readonly IPackage VSUmlPackage;
        public Package(IPackage vsUmlPackage, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlPackage = vsUmlPackage;
            VSUmlModel = iModel;
            _Name = vsUmlPackage.Name;
            MetaObjectMapper.AddTypeMap(VSUmlPackage as Microsoft.VisualStudio.Modeling.ModelElement, this);
             

            SetNamespace(VisualStudioUMLHelper.GetPackageFor(vsUmlPackage.Namespace as IPackage, iModel));
        }


        public override string Name
        {
            get
            {
                if (VSUmlPackage.Name != _Name)
                    _Name = VSUmlPackage.Name;
                return base.Name;
            }
            set
            {
                base.Name = value;
                VSUmlPackage.Name = Name;
            }
        }

        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get
            {
                return VSUmlPackage as Microsoft.VisualStudio.Modeling.ModelElement;

            }
        }
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
        
    }
}
