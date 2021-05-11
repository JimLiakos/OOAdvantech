using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing.Design;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;

using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller ;

using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;

using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;



namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{6A8579F4-2029-47AC-8E43-EA258385FD60}</MetaDataID>
    public class FormControlsSelector : UITypeEditor
    {
        class ControlTreeNode : System.Windows.Forms.TreeNode
        {
            public ControlTreeNode(string name, OOAdvantech.UserInterface.Runtime.IOperationCallerSource memberViewControl, System.Windows.Forms.TreeView treeView)
                : base(name)
            {
                //OOAdvantech.UserInterface.Runtime.IOperetionCallerSource memberViewControl=null;

                System.Drawing.ToolboxBitmapAttribute tba = TypeDescriptor.GetAttributes(memberViewControl.GetType())[typeof(System.Drawing.ToolboxBitmapAttribute)] as System.Drawing.ToolboxBitmapAttribute;

 
                if (tba != null)
                {

                    System.Drawing.Image image = tba.GetImage(memberViewControl.GetType());
                     if (image != null)
                     {
                         if(treeView.ImageList==null)
                             treeView.ImageList=new System.Windows.Forms.ImageList();

                         
                         if(!treeView.ImageList.Images.ContainsKey(memberViewControl.GetType().FullName))
                            treeView.ImageList.Images.Add(memberViewControl.GetType().FullName,image);


                        ImageIndex = treeView.ImageList.Images.IndexOfKey(memberViewControl.GetType().FullName);
                        SelectedImageIndex = ImageIndex;
                     }
                }

                foreach (string propertyName in memberViewControl.PropertiesNames)
                    Nodes.Add(propertyName);
            }
        }
        
        static System.Windows.Forms.Control lastControl;
        //private bool IsDropDownResizable = false;
        private System.Windows.Forms.TreeView TreeView = new System.Windows.Forms.TreeView();
        //private object oSelectedValue = null;
        private IWindowsFormsEditorService EditorService;
        bool InTreeViewLoad;
        object SelectedItem;
        /// <MetaDataID>{E53401BA-0801-4211-AADE-F9D9D087E141}</MetaDataID>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                InTreeViewLoad = true;
                System.ComponentModel.ISite hostForm = null;
                OOAdvantech.UserInterface.Runtime.IOperationCallerSource memberViewControl = null;
                bool isMenuCommand = false;
                if (context.Instance is CustomPropertyCollection)
                {

                    
                    if ((context.Instance as CustomPropertyCollection).Instance is IConnectableControl)
                        hostForm = (((context.Instance as CustomPropertyCollection).Instance as IConnectableControl).UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site;
                    if ((context.Instance as CustomPropertyCollection).Instance is Menus.MenuCommand)
                    {
                        hostForm = (((context.Instance as CustomPropertyCollection).Instance as Menus.MenuCommand).OwnerControl.UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site;
                        isMenuCommand = true;
                    }

                    
                    if ((context.Instance as CustomPropertyCollection).Instance is IConnectableControl)
                        memberViewControl = ((context.Instance as CustomPropertyCollection).Instance as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                    if ((context.Instance as CustomPropertyCollection).Instance is Menus.MenuCommand)
                        memberViewControl = ((context.Instance as CustomPropertyCollection).Instance as Menus.MenuCommand).OwnerControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource;
                }

                if (context.Instance is ConnectableControls.Menus.MenuCommand)
                {
                    hostForm = (((context.Instance as ConnectableControls.Menus.MenuCommand).OwnerControl as IConnectableControl).UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site;
                    memberViewControl = (context.Instance as ConnectableControls.Menus.MenuCommand).OwnerControl as IObjectMemberViewControl;
                }

                if(hostForm!=null)
                {

                    TreeView.PathSeparator = ".";


                    TreeView.Nodes.Clear();
                    TreeView.ImageList = new System.Windows.Forms.ImageList();
                    TreeView.ImageList.Images.Add(System.Drawing.ToolboxBitmapAttribute.GetImageFromResource(typeof(ViewControlObject), "ControlProperty.bmp", false));
                    TreeView.ImageList.Images.Add("ConnectableControls.NoneImage",System.Drawing.ToolboxBitmapAttribute.GetImageFromResource(typeof(ViewControlObject), "None.bmp", false));
                    System.Windows.Forms.TreeNode tn = null;
                    

                    System.Windows.Forms.TreeNode noneNode= TreeView.Nodes.Add("none");
                    noneNode.SelectedImageKey = "ConnectableControls.NoneImage";
                    noneNode.ImageKey= "ConnectableControls.NoneImage";


                    TreeView.Nodes.Add(new ControlTreeNode("this", memberViewControl,TreeView));
                    //foreach(string propertyName in memberViewControl.PropertiesNames)
                    //    TreeView.Items.Add("this." + propertyName);

                    foreach (Component formComponent in hostForm.Container.Components)
                    {
                        
                        if (/*(formComponent is System.Windows.Forms.Control) &&*/ formComponent != (hostForm.Container as System.ComponentModel.Design.IDesignerHost).RootComponent)
                        {

                            //if (formComponent is IObjectMemberViewControl && (formComponent as IObjectMemberViewControl).UserInterfaceObjectConnection != null)
                            //{
                                if (formComponent is ViewControlObject)
                                {

                                    foreach (object controlledComponent in (formComponent as ViewControlObject).UserInterfaceObjectConnection.ControlledComponents)
                                    {
                                        if (controlledComponent is IObjectMemberViewControl)
                                            TreeView.Nodes.Add(new ControlTreeNode((controlledComponent as IObjectMemberViewControl).Name, controlledComponent as OOAdvantech.UserInterface.Runtime.IOperationCallerSource, TreeView));

                                    }
                                    break;
                                }

                                //if (formComponent is System.Windows.Forms.Control && formComponent is IObjectMemberViewControl)
                                //{
                                //    TreeView.Nodes.Add(new ControlTreeNode((formComponent as System.Windows.Forms.Control).Name, formComponent as OOAdvantech.UserInterface.Runtime.IOperetionCallerSource, TreeView));

                                //    //foreach(string propertyName in (formComponent as IObjectMemberViewControl).PropertiesNames)
                                //    //    TreeView.Items.Add((formComponent as System.Windows.Forms.Control).Name + "." + propertyName);
                                //}
                                //else
                                //    TreeView.Nodes.Add((formComponent as System.Windows.Forms.Control).Name);
                            //}
                        }
                    }
                    if(isMenuCommand)
                        TreeView.Nodes.Add("$MenuCommand$");

                    TreeView.Nodes.Add("$ViewControlObject$");

                }

                EditorService = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (EditorService != null)
                {
               
                    
                    TreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    //TreeView.Click += new EventHandler(TreeViewClicked);
                    // TreeView.IntegralHeight = true;
                    TreeView.DoubleClick += new EventHandler(TreeViewClicked);
                    //TreeView.SelectedNode = TreeView.Nodes[0];

                    try
                    {
                        EditorService.DropDownControl(TreeView);
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                    return SelectedItem;
                }
                else
                    return base.EditValue(context, provider, value);
            }
            finally 
            {
                InTreeViewLoad = false;
            }
        }

        void TreeViewClicked(object sender, EventArgs e)
        {
            System.Windows.Forms.TreeNode selectedNode = TreeView.GetNodeAt(TreeView.PointToClient(System.Windows.Forms.Form.MousePosition));
            if (TreeView.SelectedNode != null )
            {
                SelectedItem =TreeView.SelectedNode.FullPath;
                EditorService.CloseDropDown();
            }


        }



   

        /// <MetaDataID>{8AB35C91-E33B-47D7-8A90-C6CE5B3CF2D2}</MetaDataID>
        void OnSelectedItem(object sender, EventArgs e)
        {
        //    if(TreeView.SelectedNode!=null)
        //         SelectedItem = TreeView.SelectedNode.FullPath;
            
        //     if (InTreeViewLoad)
        //     {
        //         TreeView.SelectedNode = null;
        //         InTreeViewLoad = false;
        //     }
        //     else
        //     {
                 

        //     }                
        }

        /// <MetaDataID>{D8AAD2D2-BA82-4CF2-B54A-B3374D8ACF67}</MetaDataID>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context!=null&& context.Instance is IConnectableControl && (context.Instance as IConnectableControl).IsPropertyReadOnly(context.PropertyDescriptor.Name))
                return System.Drawing.Design.UITypeEditorEditStyle.None;

            return UITypeEditorEditStyle.DropDown;

            
        }
    }
}
