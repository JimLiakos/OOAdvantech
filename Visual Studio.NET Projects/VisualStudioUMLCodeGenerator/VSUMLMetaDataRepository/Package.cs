using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
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
        internal IPackage VSUmlPackage;
        public Package(IPackage vsUmlPackage, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlPackage = vsUmlPackage;
            VSUmlModel = iModel;
            _Name = vsUmlPackage.Name;
            MetaObjectMapper.AddTypeMap(VSUmlPackage as Microsoft.VisualStudio.Modeling.ModelElement, this);
            SetNamespace(VisualStudioUMLHelper.GetPackageFor(vsUmlPackage.Namespace as IPackage, iModel));

        }

        public Package(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
            
        }

        //static EnvDTE.ProjectItem PackageProjectItem;
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            
            if (VSUmlPackage == null)
            {

                if (originMetaObject.Namespace != null)
                {
                    _Namespace.Value = VSUmlModel.GetPackage(originMetaObject.Namespace.FullName);
                    if (_Namespace.Value == null)
                        _Namespace.Value = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.Namespace, this) as Package;
                    _Namespace.Value.ShallowSynchronize(originMetaObject.Namespace);
                }
                //base.ShallowSynchronize(originMetaObject);

                if (_Namespace.Value != null)
                {
                    
                    //VisualStudioEventBridge.VisualStudioEvents.ProjectItemAdded += new VisualStudioEventBridge.ProjectItemAddedEventHandler(VisualStudioEvents_ProjectItemAdded);
                    VSUmlPackage = (_Namespace.Value as Package).VSUmlPackage.CreatePackage();
                    VSUmlPackage.Name = originMetaObject.Name;
                    //string filepath = PackageProjectItem.FileNames[0];

                    //System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepath);
                    //string fileName = fileInfo.Directory.FullName;
                    //fileName += "\\" + VSUmlPackage.Name + ".uml";

                    //try
                    //{
                    //    PackageProjectItem as EnvDTE80.proj .Name = "mitros";
                    //}
                    //catch (System.Exception error)
                    //{

                    //}

                    
                    
                  
                }
                else
                {
                    
                    //VisualStudioEventBridge.VisualStudioEvents.ProjectItemAdded += new VisualStudioEventBridge.ProjectItemAddedEventHandler(VisualStudioEvents_ProjectItemAdded);
                    VSUmlPackage = VSUmlModel.CreatePackage();
                    VSUmlPackage.Name = originMetaObject.Name;
                    //string fileName = PackageProjectItem.FileNames[0];
                    
                }
            }
        }

        //void VisualStudioEvents_ProjectItemAdded(EnvDTE.ProjectItem projectItem)
        //{
        //        PackageProjectItem = projectItem;
              
        //}


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
