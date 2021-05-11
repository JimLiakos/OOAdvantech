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
    /// <MetaDataID>{3fe9d3e9-9a78-4a7f-8700-f09821b4510d}</MetaDataID>
    class ViewControlObjectSelector : UITypeEditor
    {
        private System.Windows.Forms.ListBox ListBox = new System.Windows.Forms.ListBox();
        
        private IWindowsFormsEditorService EditorService;

        object SelectedItem;
        /// <MetaDataID>{E53401BA-0801-4211-AADE-F9D9D087E141}</MetaDataID>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is CustomPropertyCollection)
            {

                OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection  viewControlObject = null;
                if ((context.Instance as CustomPropertyCollection).Instance is IConnectableControl)
                    viewControlObject= ((context.Instance as CustomPropertyCollection).Instance as IConnectableControl).UserInterfaceObjectConnection;
                if ((context.Instance as CustomPropertyCollection).Instance is Menus.MenuCommand)
                    viewControlObject= ((context.Instance as CustomPropertyCollection).Instance as Menus.MenuCommand).OwnerControl.UserInterfaceObjectConnection;

                if (viewControlObject == null)
                    return value;
                ListBox.Items.Clear();

                foreach (OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection item in viewControlObject.GetHostContolViewControlObjects())
                {
                   if(item.ObjectType!=null)
                       ListBox.Items.Add(item);
                }
            }
            EditorService = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (EditorService != null)
            {
                ListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
                ListBox.IntegralHeight = true;
                ListBox.SelectedIndexChanged +=new EventHandler(OnSelectedItem);
                ListBox.Click +=new EventHandler(SelectItemClick);
                EditorService.DropDownControl(ListBox);
                return SelectedItem;
            }
            else
                return base.EditValue(context, provider, value);
        }

        /// <MetaDataID>{3A5B0B36-E5D6-4AD3-9D91-5B39CBA753CC}</MetaDataID>
        void SelectItemClick(object sender, EventArgs e)
        {
            EditorService.CloseDropDown();
            
        }

        /// <MetaDataID>{8AB35C91-E33B-47D7-8A90-C6CE5B3CF2D2}</MetaDataID>
        void OnSelectedItem(object sender, EventArgs e)
        {
            
            SelectedItem = ListBox.Items[ListBox.SelectedIndex];
            
        }
        public override bool IsDropDownResizable
        {
            get
            {
                return base.IsDropDownResizable;
            }
        }
        /// <MetaDataID>{D8AAD2D2-BA82-4CF2-B54A-B3374D8ACF67}</MetaDataID>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
    

}
