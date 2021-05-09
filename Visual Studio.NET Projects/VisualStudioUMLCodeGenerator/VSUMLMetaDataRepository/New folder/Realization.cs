using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.CommonBehaviors;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{4efad03d-aa67-446c-8c65-9b0e2500617a}</MetaDataID>
    public class Realization : OOAdvantech.MetaDataRepository.Realization
    {


        protected Realization()
        {

        }

        Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUMLModel;
        internal Realization(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUMLModel = iModel;
        }


        internal IInterfaceRealization InterfaceRealization;
        public Realization(IInterfaceRealization interfaceRealization, Interface _interface, Class _class)
            : base(interfaceRealization.Name, _interface, _class)
        {
            InterfaceRealization = interfaceRealization;
        }

        public Realization(IInterfaceRealization interfaceRealization, Interface _interface, Structure _struct)
            : base(interfaceRealization.Name, _interface, _struct)
        {
            InterfaceRealization = interfaceRealization;
        }

        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Realization orgRealization = originMetaObject as MetaDataRepository.Realization;
            if (InterfaceRealization == null)
            {
                IVSUMLModelItemWrapper implementationClass = (from classifier in VSUMLModel.GetTypes()
                                                              where classifier.Identity.ToString() == orgRealization.Implementor.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                                                select classifier).FirstOrDefault() as IVSUMLModelItemWrapper;

                Interface _interface = (from classifier in VSUMLModel.GetTypes()
                                                            where classifier.Identity.ToString() == orgRealization.Abstarction.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                        select classifier).FirstOrDefault() as Interface;

                if (implementationClass != null && _interface != null)
                    InterfaceRealization =   VSUMLModel.CreateInterfaceRealization(implementationClass.ModelElement as IBehavioredClassifier,_interface.ModelElement as IInterface);

            }
            base.Synchronize(originMetaObject);
        }
    }
}
