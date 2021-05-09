using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Uml;
using System.Windows.Forms;
using OOAdvantech.VSUMLMetaDataRepository;
using Microsoft.VisualStudio.Modeling;

namespace VisualStudioUMLCodeGenerator
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(VS.100).aspx
    /// <MetaDataID>{d4d95ac9-f185-4d23-a423-78fb6b238f23}</MetaDataID>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension, ComponentDesignerExtensionAttribute] // TODO: Add other diagram types if needed
    class GenerateCodeCommand : ICommandExtension
    {
        [Import]
        IDiagramContext context { get; set; }


        public void Execute(IMenuCommand command)
        {

            var OOAdvantechProfile = (from profile in context.CurrentDiagram.ModelStore.Root.ProfileInstances
                                      where profile.Name == "OOAdvantechProfile"
                                      select profile).FirstOrDefault();
            if (OOAdvantechProfile == null)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("The Profile 'OOAdvantechProfile' must be assigned to '{0}' UML Model \r\nUse UML Model Explorer ", context.CurrentDiagram.ModelStore.Root.Name), context.CurrentDiagram.ModelStore.Root.Name);
                return;
            }

            // TODO: Add the logic for your command extension here

            //OOAdvantech.CodeMetaDataRepository.IDEManager.s
            //OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            //metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
            //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;// new MetaObjectsStack(context.CurrentDiagram.ModelStore.Root);
            //metaObjectsStack.StartSynchronize();
            //OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
            OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = null;
            try
            {
                IShape selshape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
                if (selshape != null)
                {



                    IClassifier vsUMLClassifier = selshape.GetElement() as IClassifier;
                    IOperation vsUMLOperation = selshape.GetElement() as IOperation;
                    IProperty vsUMLAttribute = selshape.GetElement() as IProperty;
                    IAssociation vsUMLAssociation = selshape.GetElement() as IAssociation;
                    Microsoft.VisualStudio.Uml.Components.IComponent vsUMLModule = selshape.GetElement() as Microsoft.VisualStudio.Uml.Components.IComponent;


                    if (vsUMLOperation != null)
                        vsUMLClassifier = vsUMLOperation.Owner as IClassifier;
                    if (vsUMLAttribute != null)
                        vsUMLClassifier = vsUMLAttribute.Owner as IClassifier;

                    if (vsUMLModule != null)
                    {
                        System.Drawing.Point location = new System.Drawing.Point();
                        location.X = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Left + OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Width / 2;
                        location.Y = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Top;

                        ComponentResidentsForm componentResidentsForm = new ComponentResidentsForm(vsUMLModule, location);
                        componentResidentsForm.ShowDialog();
                        return;
                    }
                    if (vsUMLClassifier != null)
                    {
                        IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();
                        IDEManager.StartCodeModelSynchronize();
                        //vsUMLClassifier = selshape.GetElement() as IClassifier;
                        var classifier = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier, context.CurrentDiagram.ModelStore.Root);
                        if (classifier.ImplementationUnit is OOAdvantech.VSUMLMetaDataRepository.Component)
                        {
                            if ((classifier.ImplementationUnit as OOAdvantech.VSUMLMetaDataRepository.Component).AssignedProject != null)
                            {
                                var project = (classifier.ImplementationUnit as OOAdvantech.VSUMLMetaDataRepository.Component).AssignedProject;
                                OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());

                                if (codeClassifier == null)
                                {
                                    string namespaceName = null;
                                    if (classifier.Namespace != null)
                                        namespaceName = classifier.Namespace.FullName;
                                    if (classifier is Class)
                                        codeClassifier = project.CreateClass(classifier.Name, namespaceName);
                                    if (classifier is Interface)
                                        codeClassifier = project.CreateInterface(classifier.Name, namespaceName);
                                    if (classifier is Structure)
                                        codeClassifier = project.CreateStructure(classifier.Name, namespaceName);
                                    codeClassifier.Synchronize(classifier);
                                }
                                else
                                {
                                    if (vsUMLOperation == null && vsUMLAttribute == null && vsUMLAssociation == null)
                                        codeClassifier.Synchronize(classifier);
                                    else
                                    {
                                        if (vsUMLOperation != null)
                                        {

                                            OOAdvantech.MetaDataRepository.Feature feature = VisualStudioUMLHelper.GetClassifierFor(vsUMLOperation.Owner as IClassifier, context.CurrentDiagram.ModelStore.Root).GetFeature((vsUMLOperation as ModelElement).Id.ToString(), false);
                                            OOAdvantech.MetaDataRepository.Feature codeFeature = codeClassifier.GetFeature(feature.Identity.ToString(), false);
                                            if (codeFeature == null && feature is Operation)
                                                codeFeature = codeClassifier.AddOperation(feature.Name, (feature as Operation).ReturnType);
                                            if (codeFeature == null && feature is Method)
                                                codeFeature = codeClassifier.AddOperation(feature.Name, (feature as Method).Specification.ReturnType);
                                            codeFeature.Synchronize(feature);
                                        }

                                        if (vsUMLAttribute != null)
                                        {
                                            OOAdvantech.MetaDataRepository.Feature feature = VisualStudioUMLHelper.GetClassifierFor(vsUMLAttribute.Owner as IClassifier, context.CurrentDiagram.ModelStore.Root).GetFeature((vsUMLAttribute as ModelElement).Id.ToString(), false);
                                            OOAdvantech.MetaDataRepository.Feature codeFeature = codeClassifier.GetFeature(feature.Identity.ToString(), false);
                                            if (codeFeature == null && feature is OOAdvantech.VSUMLMetaDataRepository.Attribute)
                                                codeFeature = codeClassifier.AddAttribute(feature.Name, (feature as OOAdvantech.VSUMLMetaDataRepository.Attribute).Type, "");
                                            if (codeFeature == null && feature is OOAdvantech.VSUMLMetaDataRepository.AttributeRealization)
                                                codeFeature = codeClassifier.AddAttribute(feature.Name, (feature as OOAdvantech.VSUMLMetaDataRepository.AttributeRealization).Type, "");
                                            codeFeature.Synchronize(feature);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }


                //else if (selshape.GetElement() is IInterface)
                //{
                //    IClassifier vsUMLClassifier = selshape.GetElement() as IClassifier;
                //    var classifier = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier, context.CurrentDiagram.ModelStore.Root);
                //    var classifierForm = new OOAdvantech.VSUMLMetaDataRepository.ClassifiertForm(SpecificationFormLocation, classifier);
                //    classifierForm.ShowDialog();
                //}


                //if (selected != null)
                //{

                //    var componentForm = new ComponentForm(PrevMouselocation);
                //    //componentForm
                //    componentForm.ShowDialog();

                //}

            }
            finally
            {
                if (IDEManager != null)
                    IDEManager.EndCodeModelSynchronize();

                //if (metaObjectsStack != null)
                //{
                //    metaObjectsStack.StopSynchronize();
                //    if (MetaObjectsStack.CurrentMetaObjectCreator is MetaObjectsStack)
                //    {
                //        //(MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).Clear();
                //        MetaObjectsStack.CurrentMetaObjectCreator = null;
                //    }

                //}
            }




            // The following example creates a new class in the model store
            // and displays it on the current diagram.
            //IClassDiagram diagram = context.CurrentDiagram as IClassDiagram;
            //IModelStore store = diagram.ModelStore;
            //IPackage rootPackage = store.Root;
            //IClass newClass = rootPackage.CreateClass();
            //newClass.Name = "VisualStudioUMLCodeGenerator";
            //diagram.Display(newClass);
        }

        System.Drawing.Point Mouselocation;
        System.Drawing.Point PrevMouselocation;

        System.Drawing.Point SpecificationFormLocation
        {
            get
            {
                return PrevMouselocation;
            }
        }
        public void QueryStatus(IMenuCommand command)
        {

            // TODO: Add logic to control the display of your menu item

            // The following example will disable the command extension unless the user selected
            // a class shape.
            //
            IShape selshape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
            if (selshape != null)
            {
                PrevMouselocation = Mouselocation;
                Mouselocation = Cursor.Position;
                System.Diagnostics.Debug.WriteLine(Mouselocation.ToString());
                command.Visible = selshape.GetElement() is IClassifier;
                command.Visible |= selshape.GetElement() is IAssociation;
                command.Visible |= selshape.GetElement() is IProperty;
                command.Visible |= selshape.GetElement() is IOperation;
            }


            // Note: Setting command.Visible=false can have unintended interactions with other extensions.
        }

        public string Text
        {
            get { return "Generate C#"; }
        }
    }
}
