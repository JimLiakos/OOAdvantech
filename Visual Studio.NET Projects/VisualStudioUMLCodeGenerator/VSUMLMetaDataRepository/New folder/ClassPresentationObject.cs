using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.Classes;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{e57f55dc-1ed6-4178-8aca-e5a00fbe6194}</MetaDataID>
    public class ClassifierPresentationObject : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Classifier>
    {

        /// <MetaDataID>{c6ae25b7-94fc-4780-b7ea-ada2df293d47}</MetaDataID>
        public ClassifierPresentationObject(OOAdvantech.MetaDataRepository.Classifier classifier)
            : base(classifier)
        {
            int count = classifier.Features.Count; 
        }

        /// <MetaDataID>{63948676-0e69-474a-9942-82ed14c15985}</MetaDataID>
        public List<MetaDataRepository.BehavioralFeature> Operations
        {
            get
            {
               return (from behavioralFeature in RealObject.Features.OfType<MetaDataRepository.BehavioralFeature>()
                 select behavioralFeature).ToList();
            }
        }
        /// <MetaDataID>{eaffcfbc-2882-48a0-893d-df915da472cd}</MetaDataID>
        public List<MetaDataRepository.StructuralFeature> Attributes
        {
            get
            {
                 return (from behavioralFeature in RealObject.Features.OfType<MetaDataRepository.StructuralFeature>()
                 select behavioralFeature).ToList();;
            }
        }


        /// <MetaDataID>{edf02672-4c5c-4a63-b467-aa0f645d4623}</MetaDataID>
        public void Delete(MetaDataRepository.StructuralFeature structuralFeature)
        {
            RealObject.RemoveFeature(structuralFeature);
            if (ObjectChangeState != null)
                ObjectChangeState(this,"Attributes");

        }
        /// <MetaDataID>{3d26a10a-af1f-45fc-a0ea-b06e978bd294}</MetaDataID>
        public void Delete(MetaDataRepository.BehavioralFeature behavioralFeature)
        {
            RealObject.RemoveFeature(behavioralFeature);

            if (ObjectChangeState != null)
                ObjectChangeState(this, "Operations");
        }



        /// <MetaDataID>{2d285b29-ea85-47de-8569-72bea30f6a8c}</MetaDataID>
        public void Edit(MetaDataRepository.BehavioralFeature behavioralFeature)
        {
            if (behavioralFeature is Operation)
            {
                OperationForm OperationForm = new OperationForm((this.UserInterfaceObjectConnection.ContainerControl as System.Windows.Forms.Control).FindForm().Location, (behavioralFeature as Operation), null);
                OperationForm.ShowDialog();
            }
            if (behavioralFeature is Method)
            {
                OperationForm OperationForm = new OperationForm((this.UserInterfaceObjectConnection.ContainerControl as System.Windows.Forms.Control).FindForm().Location, (behavioralFeature as Method).Specification as Operation, (behavioralFeature as Method));
                OperationForm.ShowDialog();
            }
            
        }

        /// <MetaDataID>{53dfa5b1-2795-4a86-b664-835766a0d67a}</MetaDataID>
        public void Edit(MetaDataRepository.StructuralFeature structuralFeature)
        {
            if (structuralFeature is Attribute)
            {
                AttributeForm OperationForm = new AttributeForm((this.UserInterfaceObjectConnection.ContainerControl as System.Windows.Forms.Control).FindForm().Location, (structuralFeature as Attribute), null);
                OperationForm.ShowDialog();
            }
            //if (structuralFeature is Method)
            //{
            //    OperationForm OperationForm = new OperationForm((this.UserInterfaceObjectConnection.ContainerControl as System.Windows.Forms.Control).FindForm().Location, (behavioralFeature as Method).Specification as Operation, (behavioralFeature as Method));
            //    OperationForm.ShowDialog();
            //}

        }
        /// <MetaDataID>{d2a16e60-5861-44e0-a31b-8e4fff71832e}</MetaDataID>
        public MetaDataRepository.BehavioralFeature NewOperation()
        {

            var newOperation = RealObject.AddOperation("opname", null);
            if (ObjectChangeState != null)
                ObjectChangeState(this, "Operations");
            return newOperation;
        }
        /// <MetaDataID>{405f09bb-3441-4348-a6eb-2378fd175abb}</MetaDataID>
        public MetaDataRepository.StructuralFeature NewAttribute()
        {

            var newAttribute= RealObject.AddAttribute("name", null,null);
            if (ObjectChangeState != null)
                ObjectChangeState(this, "Attributes");
            return newAttribute;
        }


        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{f62b2855-df13-41b1-91c5-101b2711dc62}</MetaDataID>
        public List<ClassifierAssignComponent> Components
        {
            get
            {


                List<ClassifierAssignComponent> classes = (from vsUMLComponent in (RealObject as IVSUMLModelItemWrapper).UMLModel.Members.OfType<Microsoft.VisualStudio.Uml.Components.IComponent>()
                                                           from appliedStereotype in vsUMLComponent.AppliedStereotypes
                                                           where appliedStereotype.Profile == "OOAdvantechProfile"
                                                           select new ClassifierAssignComponent(RealObject, VisualStudioUMLHelper.GetComponentFor(vsUMLComponent, (RealObject as IVSUMLModelItemWrapper).UMLModel))).ToList();

                return (from componentClass in classes
                        orderby componentClass.Assigned
                        select componentClass).ToList();


                //if (!ShowAllClasses)
                //{
                //    return (from componentClass in classes
                //            where componentClass.Assigned
                //            select componentClass).ToList();
                //}
                //else
                //    return classes;
            }
        }


        /// <MetaDataID>{8bcaee28-95f6-47e5-b861-01eb5f2a1741}</MetaDataID>
        public bool CanBeAbstract
        {
            get
            {
                return RealObject is MetaDataRepository.Class;
            }
        }


        /// <MetaDataID>{513235b3-28f9-4f6d-be3d-cff4fe31941e}</MetaDataID>
        public bool CanBePersistent
        {
            get
            {
                return RealObject is MetaDataRepository.Class || RealObject is MetaDataRepository.Structure;
            }
        }
        /// <MetaDataID>{4fe66f01-b949-47b8-b9ee-eb34ff1131c4}</MetaDataID>
        public bool Abstract
        {
            get
            {
                if (RealObject is Class)
                    return (RealObject as MetaDataRepository.Class).Abstract;
                return false;
            }
            set
            {
               if(RealObject is MetaDataRepository.Class)
                   (RealObject as MetaDataRepository.Class).Abstract = value;
                
            }
        }

        /// <MetaDataID>{f20a79e0-ea2c-4050-b090-ac68a32dd109}</MetaDataID>
        public bool Persistent
        {
            get
            {
                if(RealObject is Structure)
                    return (RealObject as Structure).Persistent;
                if (RealObject is Class)
                    return (RealObject as Class).Persistent;

                return false;

            }
            set
            {
                
                if (RealObject is MetaDataRepository.Class)
                    (RealObject as MetaDataRepository.Class).Persistent = value;
                if (RealObject is MetaDataRepository.Structure)
                    (RealObject as MetaDataRepository.Structure).Persistent = value;
            }
        }

        /// <MetaDataID>{1f614e86-2128-459c-a0ee-8aa176e5e2be}</MetaDataID>
        public bool Public
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility == VisibilityKind.Public;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility = VisibilityKind.Public;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }

        /// <MetaDataID>{0642ce9b-ae59-4e80-9e9c-70d57119b7f9}</MetaDataID>
        public bool Private
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility == VisibilityKind.Private;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility = VisibilityKind.Private;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }
         

        /// <MetaDataID>{8bd91f25-ec54-4c09-a56c-d2e56f4c2614}</MetaDataID>
        public bool Protected
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility == VisibilityKind.Protected;
            }
            set
            {
                if (value)
                {

                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility = VisibilityKind.Protected;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }

        /// <MetaDataID>{b8479773-eeac-486a-865d-b52934943e0d}</MetaDataID>
        public bool Package
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility == VisibilityKind.Package;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IClassifier).Visibility = VisibilityKind.Package;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessComponent;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }


    }
}
