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

namespace VisualStudioUMLCodeGenerator
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(VS.100).aspx
    /// <MetaDataID>{d4d95ac9-f185-4d23-a423-78fb6b238f23}</MetaDataID>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension, ComponentDesignerExtensionAttribute] // TODO: Add other diagram types if needed
    class ModelItemSpecificationCommand : ICommandExtension
    {
        [Import]
        IDiagramContext context { get; set; }


        public void Execute(IMenuCommand command)
        {
           
            // TODO: Add the logic for your command extension here

            //OOAdvantech.CodeMetaDataRepository.IDEManager.s
            IShape selshape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
            if (selshape != null)
            {
                
                 

                //Microsoft.VisualStudio.Modeling.ModelElement dd;
                //(dd.Store as IModelStore).Root
                //foreach(IClass Cl context.CurrentDiagram.ModelStore.Root.Members.OfType<IClass>()
                
                
                Microsoft.VisualStudio.Uml.Components.IComponent module = selshape.GetElement() as Microsoft.VisualStudio.Uml.Components.IComponent;
                if (module != null)
                {


                    var componentForm = new OOAdvantech.VSUMLMetaDataRepository.ComponentForm(SpecificationFormLocation, new OOAdvantech.VSUMLMetaDataRepository.Component(module, context.CurrentDiagram.ModelStore.Root));
                    componentForm.ShowDialog();
                }

                IClassifier selected = selshape.GetElement() as IClassifier;
                //if (selected != null)
                //{

                //    var componentForm = new ComponentForm(PrevMouselocation);
                //    //componentForm
                //    componentForm.ShowDialog();

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
                   command.Visible = selshape.GetElement() is IClass;
                   command.Visible |= selshape.GetElement() is IAssociation;
               }

            
            // Note: Setting command.Visible=false can have unintended interactions with other extensions.
        }

        public string Text
        {
            get { return "Open Specification"; }
        }
    }
}
