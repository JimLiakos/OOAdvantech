using System.Linq;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using System.Windows.Forms;
using OOAdvantech.VSUMLMetaDataRepository;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Uml.Classes;

namespace VisualStudioUMLModelElementSpecs
{
    // Custom context menu command extension
    // See http://msdn.microsoft.com/en-us/library/ee329481(VS.100).aspx
    /// <MetaDataID>{d4d95ac9-f185-4d23-a423-78fb6b238f23}</MetaDataID>
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension, ComponentDesignerExtension] // TODO: Add other diagram types if needed
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



                var OOAdvantechProfile = (from profile in context.CurrentDiagram.ModelStore.Root.ProfileInstances
                                          where profile.Name == "OOAdvantechProfile"
                                          select profile).FirstOrDefault();
                if (OOAdvantechProfile == null)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("The Profile 'OOAdvantechProfile' must be assigned to '{0}' UML Model \r\nUse UML Model Explorer ", context.CurrentDiagram.ModelStore.Root.Name), context.CurrentDiagram.ModelStore.Root.Name);
                    return;
                }

                VisualStudioUMLHelper.SubscribeModelEventsConsumer((context.CurrentDiagram.ModelStore.Root as ModelElement).Store);
                
                //Microsoft.VisualStudio.Modeling.ModelElement dd;
                //(dd.Store as IModelStore).Root
                //foreach(IClass Cl context.CurrentDiagram.ModelStore.Root.Members.OfType<IClass>()


                Microsoft.VisualStudio.Uml.Components.IComponent module = selshape.GetElement() as Microsoft.VisualStudio.Uml.Components.IComponent;
                if (module != null)
                {


                    var component = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetComponentFor(module, context.CurrentDiagram.ModelStore.Root);
                    var componentForm = new OOAdvantech.VSUMLMetaDataRepository.ComponentForm(SpecificationFormLocation, component);
                    componentForm.ShowDialog();
                }
                else if (selshape.GetElement() is IAssociation)
                {
                    IAssociation vsUMLAssociation = selshape.GetElement() as IAssociation;
                    var association = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetAssociationFor(vsUMLAssociation, context.CurrentDiagram.ModelStore.Root);
                    var associationForm = new OOAdvantech.VSUMLMetaDataRepository.AssociationForm(SpecificationFormLocation, association);
                    associationForm.ShowDialog();
                }
                else if (selshape.GetElement() is IClassifier)
                {
                    IClassifier vsUMLClassifier = selshape.GetElement() as IClassifier;
                    var classifier = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier, context.CurrentDiagram.ModelStore.Root);
                    var classifierForm = new OOAdvantech.VSUMLMetaDataRepository.ClassifiertForm(SpecificationFormLocation, classifier);
                    classifierForm.ShowDialog();
                }
                else if (selshape.GetElement() is IProperty)
                {
                    IProperty vsUMLAttribute = selshape.GetElement() as IProperty;
                    var classifier = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetClassifierFor(vsUMLAttribute.Classifier, context.CurrentDiagram.ModelStore.Root);
                    OOAdvantech.MetaDataRepository.Feature feature = classifier.GetFeature((vsUMLAttribute as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(), false);

                    OOAdvantech.VSUMLMetaDataRepository.Attribute attribute = null;
                    OOAdvantech.VSUMLMetaDataRepository.AttributeRealization attributeRealization = null;
                    if (feature is OOAdvantech.VSUMLMetaDataRepository.Attribute)
                        attribute = feature as OOAdvantech.VSUMLMetaDataRepository.Attribute;
                    if (feature is OOAdvantech.VSUMLMetaDataRepository.AttributeRealization)
                    {
                        attributeRealization = feature as OOAdvantech.VSUMLMetaDataRepository.AttributeRealization;
                        attribute = (feature as OOAdvantech.VSUMLMetaDataRepository.AttributeRealization).Specification as OOAdvantech.VSUMLMetaDataRepository.Attribute;
                    }
                    var attributeForm = new OOAdvantech.VSUMLMetaDataRepository.AttributeForm(SpecificationFormLocation, attribute, attributeRealization);
                    attributeForm.ShowDialog();
                }
                else if (selshape.GetElement() is IOperation)
                {
                    IOperation vsUMLOperation = selshape.GetElement() as IOperation;
                    var classifier = OOAdvantech.VSUMLMetaDataRepository.VisualStudioUMLHelper.GetClassifierFor(vsUMLOperation.Owner as IClassifier, context.CurrentDiagram.ModelStore.Root);
                    OOAdvantech.VSUMLMetaDataRepository.Operation operation = classifier.GetFeature((vsUMLOperation as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString(), false) as OOAdvantech.VSUMLMetaDataRepository.Operation;
                    OOAdvantech.VSUMLMetaDataRepository.Method method = classifier.GetFeature((vsUMLOperation as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(), false) as OOAdvantech.VSUMLMetaDataRepository.Method;
                  
                    if (method != null)
                        operation = method.Specification as OOAdvantech.VSUMLMetaDataRepository.Operation;

                    var operationForm = new OOAdvantech.VSUMLMetaDataRepository.OperationForm(SpecificationFormLocation, operation, method);
                    operationForm.ShowDialog();
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
                return Mouselocation;
            }
        }
        EnvDTE.DTEEvents DTEEvents;
        bool HookOn;
        public void QueryStatus(IMenuCommand command)
        {



            if (!HookOn)
            {
                DTEEvents = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().Events.DTEEvents;
                //DTEEvents.OnBeginShutdown += new EnvDTE._dispDTEEvents_OnBeginShutdownEventHandler(DTEEvents_OnBeginShutdown);
                //IntPtr Hinst = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
               // WindowsMessageHooks.HookManager.MouseClick += new MouseEventHandler(HookManager_MouseClick);
                //HookOn = true;
                Mouselocation = Cursor.Position;
                Mouselocation.X = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Left + OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Width/2;
                Mouselocation.Y = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.Top;

            }

            // TODO: Add logic to control the display of your menu item

            // The following example will disable the command extension unless the user selected
            // a class shape.
            //





            IShape selshape = context.CurrentDiagram.SelectedShapes.FirstOrDefault();
            if (selshape != null)
            {

                //PrevMouselocation = Mouselocation;
                //Mouselocation = Cursor.Position;
                //System.Diagnostics.Debug.WriteLine(Mouselocation.ToString());
                command.Visible = selshape.GetElement() is IClassifier;
                command.Visible |= selshape.GetElement() is Microsoft.VisualStudio.Uml.Components.IComponent;
                command.Visible |= selshape.GetElement() is IAssociation;
                command.Visible |= selshape.GetElement() is IProperty;
                command.Visible |= selshape.GetElement() is IOperation;


            }
            //int hWnd = OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().ActiveWindow.HWnd;


            // Note: Setting command.Visible=false can have unintended interactions with other extensions.
        }

        void DTEEvents_OnBeginShutdown()
        {
            
            DTEEvents.OnBeginShutdown -= new EnvDTE._dispDTEEvents_OnBeginShutdownEventHandler(DTEEvents_OnBeginShutdown);
            WindowsMessageHooks.HookManager.MouseClick -= new MouseEventHandler(HookManager_MouseClick);
            HookOn = false;
            DTEEvents = null;

        }

        void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Mouselocation = e.Location;

        }

        public string Text
        {
            get { return "Open Specification"; }
        }
    }


    /// <MetaDataID>{229e12e9-b2da-4a85-9e77-058a41984d73}</MetaDataID>
    public class VisualStudioWindow : NativeWindow
    {
        const int WM_RBUTTONDBLCLK = 0x0204;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_RBUTTONDBLCLK)
            {

            }
            base.WndProc(ref m);
        }
    }
}
