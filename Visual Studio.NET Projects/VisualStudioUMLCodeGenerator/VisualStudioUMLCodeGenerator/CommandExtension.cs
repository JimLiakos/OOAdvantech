using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;
using System.Windows.Forms;

namespace VisualStudioUMLCodeGenerator
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(VS.100).aspx
    /// <MetaDataID>{dad69d4e-3b45-48f6-94bc-693872b2fa71}</MetaDataID>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension] // TODO: Add other diagram types if needed
    class CommandExtension : ICommandExtension
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
            IShape selshape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
            if (selshape != null)
            {
                IClassifier selected = selshape.GetElement() as IClassifier;

                IAssociation selectedAss = selshape.GetElement() as IAssociation;
                if (selected != null)
                {

                   // var componentForm = new OOAdvantech.VSUMLMetaDataRepository.ComponentForm(PrevMouselocation);
                    //componentForm
                   // componentForm.ShowDialog();

                }

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
            get { return "Generate C#"; }
        }
    }
}
