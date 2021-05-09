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

        /// <MetaDataID>{a9b8e7f0-312f-40ab-b5e9-3f47579d6366}</MetaDataID>
        public void CheckAll(System.Collections.Generic.IEnumerable<SynchronizationComponentItem> selected)
        {

            foreach (var syncItem in selected)
            {
                syncItem.Synchronize = true;
            }
        }

        /// <MetaDataID>{0f123350-c998-4d9b-ad60-5e1974a80a8c}</MetaDataID>
        public void UnCheckAll(System.Collections.Generic.IEnumerable<SynchronizationComponentItem> selected)
        {

            foreach (var syncItem in selected)
            {
                syncItem.Synchronize = false;
            }
        }


        /// <MetaDataID>{324ac512-1479-477a-8f29-f71e81750bc8}</MetaDataID>
        public void UpdateModel()
        {
            ComponentResidentsForm componentResidentsForm = UserInterfaceObjectConnection.PresentationContextViewControl.ContainerControl as ComponentResidentsForm;
            if (componentResidentsForm.UpdateModel)
            {
                OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(RealObject.UMLModel);
                try
                {
                    List<MetaDataRepository.Classifier> sourceCollection = new List<MetaDataRepository.Classifier>();
                    List<MetaDataRepository.Classifier> targetCollection = new List<MetaDataRepository.Classifier>();
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                    foreach (var synchItem in Residents)
                    {
                        if (synchItem.Synchronize)
                        {
                            if (synchItem.RealObject is IVSUMLModelItemWrapper)
                            {
                                MetaDataRepository.Classifier classifier = synchItem.RealObject as MetaDataRepository.Classifier;
                                targetCollection.Add(classifier);
                                MetaDataRepository.Classifier codeClassifier = RealObject.AssignedProject.GetClassifier(classifier.Identity.ToString());
                                sourceCollection.Add(codeClassifier);
                                //classifier.Synchronize(codeClassifier);
                            }
                            else
                                sourceCollection.Add(synchItem.RealObject as MetaDataRepository.Classifier);
                        }
                    }
                    ContainedItemsSynchronizer classifiersSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(sourceCollection, targetCollection, RealObject) as ContainedItemsSynchronizer;
                    classifiersSynchronizer.FindModifications();
                    classifiersSynchronizer.ExecuteAddCommand();
                    //classifiersSynchronizer.ExecuteDeleteCommand();
                    classifiersSynchronizer.Synchronize();
                }
                finally
                {
                    if (MetaObjectsStack.CurrentMetaObjectCreator is MetaObjectsStack)
                        MetaObjectsStack.CurrentMetaObjectCreator = null;
                }
            }
            else
            {
               var IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();
                IDEManager.StartCodeModelSynchronize();

                try
                {
                    List<MetaDataRepository.Classifier> sourceCollection = new List<MetaDataRepository.Classifier>();
                    List<MetaDataRepository.Classifier> targetCollection = new List<MetaDataRepository.Classifier>();
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                    foreach (var synchItem in Residents)
                    {
                        if (synchItem.Synchronize)
                        {
                            try
                            {
                                if (synchItem.RealObject is IVSUMLModelItemWrapper)
                                {
                                    MetaDataRepository.Classifier classifier = synchItem.RealObject as MetaDataRepository.Classifier;
                                    MetaDataRepository.Classifier codeClassifier = RealObject.AssignedProject.GetClassifier(classifier.Identity.ToString());
                                    if (codeClassifier == null)
                                    {
                                        string namespaceName = null;
                                        if (classifier.Namespace != null)
                                            namespaceName = classifier.Namespace.FullName;
                                        if (classifier is Class)
                                            codeClassifier = RealObject.AssignedProject.CreateClass(classifier.Name, namespaceName);
                                        if (classifier is Interface)
                                            codeClassifier = RealObject.AssignedProject.CreateInterface(classifier.Name, namespaceName);
                                        if (classifier is Structure)
                                            codeClassifier = RealObject.AssignedProject.CreateStructure(classifier.Name, namespaceName);
                                    }
                                    codeClassifier.Synchronize(classifier);
                                }
                            }
                            catch (Exception error)
                            {
                            }
                            
                        }
                    }
                   
                }
                finally
                {
                    if (IDEManager != null)
                        IDEManager.EndCodeModelSynchronize();
                }
                    


            }
       

        }

        public string UpdateButtonText
        {
            get
            {
                ComponentResidentsForm componentResidentsForm = UserInterfaceObjectConnection.PresentationContextViewControl.ContainerControl as ComponentResidentsForm;
                if (componentResidentsForm.UpdateModel)
                    return "Update Model";
                else
                    return "Generate Code";

            }
        }

        /// <MetaDataID>{1b363b7d-7ee7-4866-80a8-d3beee5b1a4b}</MetaDataID>
        List<SynchronizationComponentItem> _Residents;
        /// <MetaDataID>{0c2c3f2e-ef50-4688-a4f4-bd0d75fb2e64}</MetaDataID>
        public List<SynchronizationComponentItem> Residents
        {
            get 
            {

                if (_Residents == null)
                {
                    ComponentResidentsForm componentResidentsForm = UserInterfaceObjectConnection.PresentationContextViewControl.ContainerControl as ComponentResidentsForm;
                    if (componentResidentsForm.UpdateModel)
                    {
                        List<MetaDataRepository.Classifier> classifiers = new List<MetaDataRepository.Classifier>();
                        if (RealObject.AssignedProject != null)
                        {
                            foreach (var codeClassifier in (from classifier in RealObject.AssignedProject.Residents.OfType<MetaDataRepository.Classifier>()
                                                            where classifier.TemplateBinding == null
                                                            select classifier))
                            {
                                bool exist = false;
                                foreach (var classifier in RealObject.Residents)
                                {
                                    if (classifier.Identity == MetaObjectMapper.GetIdentity(codeClassifier))
                                    {
                                        classifiers.Add(classifier as MetaDataRepository.Classifier);
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist)
                                    classifiers.Add(codeClassifier);


                            }
                            _Residents = (from classifier in classifiers select new SynchronizationComponentItem(classifier, componentResidentsForm.UpdateModel)).ToList();
                            //return (from classifier in RealObject.AssignedProject.Residents.OfType<MetaDataRepository.Classifier>()
                            // where classifier.TemplateBinding == null
                            //        select classifier).OfType<MetaDataRepository.MetaObject>().ToList();
                        }
                        else
                            _Residents=new List<SynchronizationComponentItem>();
                    }
                    else
                    {
                         List<MetaDataRepository.Classifier> classifiers = new List<MetaDataRepository.Classifier>();
                         if (RealObject.AssignedProject != null)
                         {
                             foreach (var classifier in (from classifier in RealObject.Residents.OfType<MetaDataRepository.Classifier>()
                                                         where classifier.TemplateBinding == null
                                                         select classifier))
                             {
                                 bool exist = false;
                                 foreach (var codeClassifier in RealObject.AssignedProject.Residents)
                                 {
                                     if (classifier.Identity == MetaObjectMapper.GetIdentity(codeClassifier))
                                     {
                                         classifiers.Add(codeClassifier as MetaDataRepository.Classifier);
                                         exist = true;
                                         break;
                                     }
                                 }
                                 if (!exist)
                                     classifiers.Add(classifier);


                             }
                             _Residents = (from classifier in classifiers select new SynchronizationComponentItem(classifier, componentResidentsForm.UpdateModel)).ToList();
                         }
                    }
                }
                 
                return _Residents;
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
