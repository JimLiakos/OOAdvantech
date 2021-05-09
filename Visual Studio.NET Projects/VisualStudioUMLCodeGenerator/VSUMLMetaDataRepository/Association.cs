using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{0bb5d4d7-767a-4d17-a5e5-563b86d9344f}</MetaDataID>
    public class Association : OOAdvantech.MetaDataRepository.Association,IVSUMLModelItemWrapper
    {
        /// <MetaDataID>{ac11201b-7e4e-4978-adb2-cec6f7612ce6}</MetaDataID>
        protected Association()
        {
        }

        public override string Name
        {
            get
            {
                if (VSUMLAssociation != null)
                    _Name = VSUMLAssociation.Name;
                return base.Name;
            }
            set
            {
                if (VSUMLAssociation != null)
                    VSUMLAssociation.Name = value;
                base.Name = value;
            }
        }

        public void Refresh()
        {

        }
        /// <MetaDataID>{6931ed6c-8a41-472f-9719-954bbbfd72c5}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{acf8d076-8ce3-4337-9829-aae5dc372646}</MetaDataID>
        public readonly IAssociation VSUMLAssociation;
        /// <MetaDataID>{0773f7f7-98c1-4c0c-a824-ead4cbf1355f}</MetaDataID>
        public Association(IAssociation vsUMLAssociation, AssociationEnd roleA, AssociationEnd roleB, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel, string identity)
            : base(vsUMLAssociation.Name, roleA, roleB, identity)
        {
            VSUMLAssociation=vsUMLAssociation;
            VSUmlModel=iModel;
            //_Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
            MetaObjectMapper.AddTypeMap(VSUMLAssociation as Microsoft.VisualStudio.Modeling.ModelElement, this);
            //_Name = vsUMLAssociation.Name;

        }


        /// <MetaDataID>{cde8d8b0-e8e4-4051-9232-1ec8ca8de36c}</MetaDataID>
        public override MetaDataRepository.AssociationEnd RoleA
        {
            get
            {
                return base.RoleA;
            }
        }
        /// <MetaDataID>{9b4c837b-2fb7-45c1-b731-280d267a1f41}</MetaDataID>
        public override MetaDataRepository.AssociationEnd RoleB
        {
            get
            {
                return base.RoleB;
            }
        }


        /// <MetaDataID>{b0b02ef6-7b88-425a-8f48-c63a7f8a71f7}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get 
            { 
                return VSUMLAssociation as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }

        /// <MetaDataID>{13b612ac-6a7c-48a9-aa0b-fe335fcd0f24}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get { return VSUmlModel; }
        }

        
    }
}
