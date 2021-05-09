using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using System.Linq;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{df94b6e7-0f02-4296-8c71-8b43039f464a}</MetaDataID>
    public class Generalization : OOAdvantech.MetaDataRepository.Generalization
    {


        protected Generalization()
        {
        }
        Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUMLModel;
        internal Generalization(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUMLModel = iModel;
        }

        internal IGeneralization VSUMLGeneralization;
        public Generalization(IGeneralization generalization, MetaDataRepository.Classifier classifier, MetaDataRepository.Classifier _class)
            : base(generalization.Description, classifier, _class)
        {
            VSUMLGeneralization = generalization;
        }

        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Generalization orgGeneralization=originMetaObject as MetaDataRepository.Generalization;
            if (VSUMLGeneralization == null)
            {
                //IClassifier dfdf;
                //dfdf.CreateGeneralization(

                IVSUMLModelItemWrapper specializedClassifier = (from classifier in VSUMLModel.GetTypes()
                                                     where classifier.Identity.ToString() == orgGeneralization.Child.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                                                select classifier).FirstOrDefault() as IVSUMLModelItemWrapper;

                IVSUMLModelItemWrapper generalClassifier = (from classifier in VSUMLModel.GetTypes()
                                                     where classifier.Identity.ToString() == orgGeneralization.Parent.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                                            select classifier).FirstOrDefault() as IVSUMLModelItemWrapper;

                if(specializedClassifier!=null&&generalClassifier!=null)
                    VSUMLGeneralization=(specializedClassifier.ModelElement as IClassifier).CreateGeneralization(generalClassifier.ModelElement as IClassifier);

            }
            base.Synchronize(originMetaObject);
        }
    }
}
