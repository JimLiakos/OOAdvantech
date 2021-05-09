using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.CommonBehaviors;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{4efad03d-aa67-446c-8c65-9b0e2500617a}</MetaDataID>
    public class Realization : OOAdvantech.MetaDataRepository.Realization,IVSUMLModelItemWrapper
    {

        public ModelElement ModelElement
        {
            get { return InterfaceRealization as ModelElement; }
        }

        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get { return VSUMLModel; }
        }

        public void Refresh()
        {

        } 

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
            (InterfaceRealization as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", Identity.ToString());
        }

        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Realization orgRealization = originMetaObject as MetaDataRepository.Realization;
            if (InterfaceRealization == null)
            {
                IVSUMLModelItemWrapper implementationClass = (from classifier in VSUMLModel.GetTypes()
                                                              where classifier.Identity.ToString() == (orgRealization.Implementor as MetaDataRepository.Classifier).GetPropertyValue<string>("MetaData", "MetaObjectID")
                                                                select classifier).FirstOrDefault() as IVSUMLModelItemWrapper;

                Interface _interface = (from classifier in VSUMLModel.GetTypes()
                                                            where classifier.Identity.ToString() == orgRealization.Abstarction.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                        select classifier).FirstOrDefault() as Interface;

                if (implementationClass != null && _interface != null)
                {
                    _Abstarction = _interface;
                    _Implementor = implementationClass as MetaDataRepository.InterfaceImplementor;
                    


                    System.Collections.Generic.List<IOperation> members = (from operation in (implementationClass.ModelElement as IClassifier).OwnedMembers.OfType<IOperation>()
                                                                           select operation).ToList();

                    ////  (implementationClass.ModelElement as IClassifier).o
                    InterfaceRealization = VSUMLModel.CreateInterfaceRealization(implementationClass.ModelElement as IBehavioredClassifier, _interface.ModelElement as IInterface);
                    (InterfaceRealization as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", Identity.ToString());

                    System.Collections.Generic.List<IOperation> newMembers = (from operation in (implementationClass.ModelElement as IClassifier).OwnedMembers.OfType<IOperation>()
                                                                              select operation).ToList();

                    foreach (var operation in newMembers)
                    {
                        if (!members.Contains(operation))
                            operation.Delete();

                    }


                }

            }
            base.Synchronize(originMetaObject);
        }
    }
}
