using Microsoft.VisualStudio.Uml.Classes;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{99b0dad3-3b6d-4977-b0c4-622f15699212}</MetaDataID>
    public class Primitive : OOAdvantech.MetaDataRepository.Primitive, IVSUMLModelItemWrapper
    {


        protected Primitive()
        {

        }

        public override OOAdvantech.MetaDataRepository.Namespace Namespace
        {
            get
            {
                if (ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                    return null;
                else
                    return base.Namespace;
            }
        }
        public override string FullName
        {
            get
            {
                if (ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                    return Name;
                return base.FullName;
            }
        }

        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        public readonly IPrimitiveType VSUmlPrivite;


        /// <MetaDataID>{f196d91e-60cc-4a8f-95d7-c215ba9fcf69}</MetaDataID>
        public Primitive(IPrimitiveType vsUmlPrivite, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlPrivite = vsUmlPrivite;
            VSUmlModel = iModel;

            _Name = VSUmlPrivite.Name;
            
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
            MetaObjectMapper.AddTypeMap(VSUmlPrivite as Microsoft.VisualStudio.Modeling.ModelElement, this);

            MetaDataRepository.Namespace _namespace = VisualStudioUMLHelper.GetPackageFor(VSUmlPrivite.Package, iModel);

            SetNamespace(_namespace);
        }




        /// <MetaDataID>{ab5b67a1-8a96-43ca-9e0c-9af2c64b0475}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get
            {
                return VSUmlPrivite as Microsoft.VisualStudio.Modeling.ModelElement;

            }
        }
        /// <MetaDataID>{e636668a-b073-4fac-bfc9-963056c98429}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }

    }
}
