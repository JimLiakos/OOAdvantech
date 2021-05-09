using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.Classes;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{e57f55dc-1ed6-4178-8aca-e5a00fbe6194}</MetaDataID>
    public class ComponentPresentationObject : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.VSUMLMetaDataRepository.Component>
    {

        /// <MetaDataID>{c6ae25b7-94fc-4780-b7ea-ada2df293d47}</MetaDataID>
        public ComponentPresentationObject(OOAdvantech.VSUMLMetaDataRepository.Component component)
            : base(component)
        {
           
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        bool _ShowAllClasses;
        /// <MetaDataID>{c8cd6277-4709-423e-914d-21ba36fdd12c}</MetaDataID>
        public bool ShowAllClasses
        {
            get
            {
                return _ShowAllClasses;
            }
            set
            {
                _ShowAllClasses = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "Classifiers");
                

            }
        }

        /// <MetaDataID>{f62b2855-df13-41b1-91c5-101b2711dc62}</MetaDataID>
        public List<ComponentAssignClassifier> Classifiers
        {
            get
            {

                List<ComponentAssignClassifier> classes = (from classifier in RealObject.VSUmlModel.GetAllClassifiers()
                                                           select new ComponentAssignClassifier(classifier, RealObject)).ToList();

                if (!ShowAllClasses)
                {
                    return (from componentClass in classes
                            where componentClass.Assigned
                            select componentClass).ToList();
                }
                else
                    return classes;
            }
        }

        /// <MetaDataID>{4b47481f-30a8-49ba-a986-145beb36a3c9}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Component AssignedProject
        {
            get
            {
                return RealObject.AssignedProject;
            }
            set
            {
                //value.Identity
                RealObject.AssignedProject = value as CodeMetaDataRepository.Project;
            }
        }

        /// <MetaDataID>{7c7243f4-cae3-4c6e-a1b8-db1399cf92e5}</MetaDataID>
        public List<OOAdvantech.MetaDataRepository.Component> SolutionProjects
        {
            get
            {
                return (from project in new OOAdvantech.CodeMetaDataRepository.IDEManager().Solution.Projects
                 where project.Laguage==CodeMetaDataRepository.ProjectLanguage.CSharp
                 select project).OfType<MetaDataRepository.Component>().ToList();
            }
        }
        
    }
}
