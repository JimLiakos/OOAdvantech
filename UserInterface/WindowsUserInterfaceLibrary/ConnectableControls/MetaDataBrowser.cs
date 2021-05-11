using System;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using ConnectableControls.ListView;
using System.Windows.Forms;

namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{06a1d191-1f0c-4e43-9013-5f05b5e5a1f9}</MetaDataID>
    public class MetaData
    {
        public readonly string Path;
        public readonly OOAdvantech.MetaDataRepository.MetaObject MetaObject;
        public MetaData(string path, OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            Path = path;
            MetaObject = metaObject;
            if (MetaObject == null)
                Path = "";
        }




    }
    /// <MetaDataID>{EC6300CA-9B90-4CE1-85B5-BECC528F6223}</MetaDataID>
    public class EditMetaData : System.Drawing.Design.UITypeEditor
    {
        /// <MetaDataID>{E2DEB570-726A-4F96-B2B9-E4B2371FD2F6}</MetaDataID>
        public override System.Object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, System.Object value)
        {
            try
            {

                string propertyName = context.PropertyDescriptor.Name;

                OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection viewControlObject = null;
                if (context.Instance is DynamicViewContainer && propertyName == "DefaultHostedViewType")
                {
                    viewControlObject = (context.Instance as DynamicViewContainer).UserInterfaceObjectConnection;
                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), viewControlObject, MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return mMetaDataNavigator.SelectedMetaObject;
                }
                else if (context.Instance is OOAdvantech.UserInterface.Runtime.IConnectableControl&&!(context.Instance is ViewControlObject))
                {

                    OOAdvantech.MetaDataRepository.Classifier classifier = (context.Instance as OOAdvantech.UserInterface.Runtime.IConnectableControl).GetClassifierFor(context);
                    if (classifier == null)
                    {
                        if ((context.Instance as IObjectMemberViewControl).UserInterfaceObjectConnection != null&&
                            (context.Instance as IObjectMemberViewControl).UserInterfaceObjectConnection.PresentationObjectType==null)
                        {
                            string message = string.Format("The property ViewControlObjectType of view control object ({0}) has no value.\nPlease declare view control object type.", (context.Instance as IObjectMemberViewControl).UserInterfaceObjectConnection.Name);
                            System.Windows.Forms.MessageBox.Show(message, "Connectable Controls", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        return value;
                    }

                    MetaDataNavigator mMetaDataNavigator = null;
                    if (classifier is OOAdvantech.MetaDataRepository.Component)
                        mMetaDataNavigator = new MetaDataNavigator(classifier, context.Instance as IConnectableControl, MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);
                    else
                        mMetaDataNavigator = new MetaDataNavigator(classifier, context.Instance as IConnectableControl, MetaDataNavigator.NavigatorType.InstanceMember, context.PropertyDescriptor.Name);

                    mMetaDataNavigator.Location = System.Windows.Forms.Control.MousePosition;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (mMetaDataNavigator.SelectedMember == "None")
                            return "";
                        if (mMetaDataNavigator.SelectedMember.IndexOf(classifier.Name + ".") == 0)
                            return new MetaData(mMetaDataNavigator.SelectedMember.Substring(classifier.Name.Length + 1), mMetaDataNavigator.SelectedMetaObject);
                        else
                            return new MetaData(mMetaDataNavigator.SelectedMember, mMetaDataNavigator.SelectedMetaObject);
                    }


                    viewControlObject = (context.Instance as IConnectableControl).UserInterfaceObjectConnection;
                }
                else if ((context.Instance is CustomPropertyCollection) && ((context.Instance as CustomPropertyCollection).Instance is IConnectableControl || (context.Instance as CustomPropertyCollection).Instance is ConnectableControls.Menus.MenuCommand))
                {
                    OOAdvantech.MetaDataRepository.MetaObject browseMetaObject = null;
                    MetaDataNavigator.NavigatorType navigatorType = MetaDataNavigator.NavigatorType.Type;
                    OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver objectMemberViewControl = (context.Instance as CustomPropertyCollection).PropertiesOwner as OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver;
                    //if (!string.IsNullOrEmpty((context.Instance as CustomPropertyCollection).PropertyName))
                    //    propertyName = (context.Instance as CustomPropertyCollection).PropertyName;
                    //if (objectMemberViewControl == null)
                    //    objectMemberViewControl = ((context.Instance as CustomPropertyCollection).Instance as Menus.MenuCommand).OwnerControl;

                    GetBrowseRootMetaObject((context.Instance as CustomPropertyCollection), ref browseMetaObject, ref navigatorType);

                    if (browseMetaObject == null)
                        return value;

                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(browseMetaObject, objectMemberViewControl, navigatorType, propertyName);
                    mMetaDataNavigator.Location = System.Windows.Forms.Control.MousePosition;

                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return new MetaData(mMetaDataNavigator.SelectedMember, mMetaDataNavigator.SelectedMetaObject);


                    return value;

                }
                else if (context.Instance is ViewControlObject)
                {
                    viewControlObject = (context.Instance as ViewControlObject).UserInterfaceObjectConnection;
                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), viewControlObject, MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return mMetaDataNavigator.SelectedMetaObject;
                }
                else if (context.Instance is ConnectedTypeMetaData)
                {

                    viewControlObject = (context.Instance as ConnectedTypeMetaData).ConnectableControl.UserInterfaceObjectConnection;
                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), viewControlObject, MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return mMetaDataNavigator.SelectedMetaObject;
                }
                else if (context.Instance is DependencyProperty)
                {
                    //viewControlObject = (context.Instance as DependencyProperty).ConnectableControl.UserInterfaceObjectConnection;
                    //MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), viewControlObject, MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);

                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator((context.Instance as DependencyProperty).ConnectableControl.UserInterfaceObjectConnection.PresentationObjectType, (context.Instance as DependencyProperty).ConnectableControl, MetaDataNavigator.NavigatorType.InstanceMember, (context.Instance as DependencyProperty).PropertyName);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return new MetaData(mMetaDataNavigator.SelectedMember, mMetaDataNavigator.SelectedMetaObject);
                }
                else if (context.Instance is ConnectableControls.Menus.MenuCommand || ((context.Instance is CustomPropertyCollection) && (context.Instance as CustomPropertyCollection).Instance is Menus.MenuCommand))
                {

                    MetaDataNavigator mMetaDataNavigator = null;
                    if (context.Instance is ConnectableControls.Menus.MenuCommand)
                        mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), (context.Instance as Menus.MenuCommand), MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);
                    else
                        mMetaDataNavigator = new MetaDataNavigator(AssemblyManager.GetActiveWindowProject(), ((context.Instance as CustomPropertyCollection).Instance as Menus.MenuCommand), MetaDataNavigator.NavigatorType.Type, context.PropertyDescriptor.Name);

                    mMetaDataNavigator.Location = System.Windows.Forms.Control.MousePosition;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return mMetaDataNavigator.SelectedMetaObject;
                }
                else if (context.Instance is ReportNode)
                {
                    OOAdvantech.MetaDataRepository.Component component = null;
                    if (AssemblyManager.InVisualStudio)
                        component = AssemblyManager.GetActiveWindowProject();
                    else
                    {
                        component = (context.Instance as ReportNode).Component;
                    }
                    MetaDataNavigator.NavigatorType navigatorType = MetaDataNavigator.NavigatorType.Type;
                    if (context.PropertyDescriptor.Name == "QueryResultPath")
                        navigatorType = MetaDataNavigator.NavigatorType.StaticMember;


                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(component, context.Instance as OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver, navigatorType, context.PropertyDescriptor.Name);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return mMetaDataNavigator.SelectedMetaObject;
                }
                else if (context.Instance is ReportDataNode)
                {
                    MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator((context.Instance as ReportDataNode).Type, context.Instance as OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver, MetaDataNavigator.NavigatorType.InstanceMember, context.PropertyDescriptor.Name);
                    mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                    mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                    if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return new MetaData(mMetaDataNavigator.SelectedMember, mMetaDataNavigator.SelectedMetaObject);
                }
                else
                    return value;
                return value;
            }
            catch (System.Exception error)
            {

            }
            return value;
        }

        private static void GetBrowseRootMetaObject(CustomPropertyCollection properties, ref OOAdvantech.MetaDataRepository.MetaObject browseMetaObject, ref MetaDataNavigator.NavigatorType navigatorType)
        {
            if (((OOAdvantech.UserInterface.CallType)(properties["Operation Type"]).Value) == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
            {
                OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection userInterfaceObjectConnection = null;
                navigatorType = MetaDataNavigator.NavigatorType.InstanceMember;
                if (properties.Instance is IConnectableControl)
                    userInterfaceObjectConnection = (properties.Instance as IConnectableControl).UserInterfaceObjectConnection;
                else
                    userInterfaceObjectConnection = (properties.Instance as Menus.MenuCommand).OwnerControl.UserInterfaceObjectConnection;
                if (properties["ViewControl components"] != null && properties["ViewControl components"].Value is OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection)
                    userInterfaceObjectConnection = (properties["ViewControl components"].Value as OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection);
                if (userInterfaceObjectConnection != null)
                {
                    browseMetaObject = userInterfaceObjectConnection.PresentationObjectType;
                    if (browseMetaObject == null)
                    {
                        string message = string.Format("The property ViewControlObjectType of view control object ({0}) has no value.\nPlease declare view control object type.", userInterfaceObjectConnection.Name);
                        System.Windows.Forms.MessageBox.Show(message, "Connectable Controls", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    }
                }
            }
            else if (((OOAdvantech.UserInterface.CallType)(properties["Operation Type"]).Value) == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall)
            {
                if (properties["Controls"] != null && properties["Controls"].Value is string)
                {
                    string controlPath = properties["Controls"].Value as string;
                    IObjectMemberViewControl control = null;
                    if (properties.Instance is IObjectMemberViewControl)
                        control = properties.Instance as IObjectMemberViewControl;
                    else if (properties.Instance is Menus.MenuCommand)
                        control = (properties.Instance as Menus.MenuCommand).OwnerControl;

                    if ((control is ConnectableControls.List.Models.Column) && controlPath == "this")
                        browseMetaObject = (control as ConnectableControls.List.Models.Column).Owner.ListConnection.PresentationObjectType;
                    else
                    {
                        if (string.IsNullOrEmpty(controlPath) || controlPath == "this" || controlPath == control.Name)
                            return;
                        if (controlPath.IndexOf("this.") == 0)
                        {
                            controlPath = controlPath.Substring("this.".Length);
                            browseMetaObject = (control as OOAdvantech.UserInterface.Runtime.IOperationCallerSource).GetPropertyType(controlPath);
                        }
                        else
                        {
                            string controlName = controlPath;
                            if (controlPath.IndexOf('.') != -1)
                                controlName = controlPath.Substring(0, controlPath.IndexOf('.'));
                            control = control.UserInterfaceObjectConnection.GetControlWithName(controlName) as IObjectMemberViewControl;
                            if (controlPath.IndexOf(control.Name + ".") == 0)
                                controlPath = controlPath.Substring(control.Name.Length + 1);
                            browseMetaObject = control.GetPropertyType(controlPath);
                        }
                    }
                }
                navigatorType = MetaDataNavigator.NavigatorType.InstanceMember;
            }
            else if (((OOAdvantech.UserInterface.CallType)(properties["Operation Type"]).Value) == OOAdvantech.UserInterface.CallType.StaticOperationCall)
            {
                browseMetaObject = AssemblyManager.GetActiveWindowProject();
                navigatorType = MetaDataNavigator.NavigatorType.StaticMember;
            }
            else if (((OOAdvantech.UserInterface.CallType)(properties["Operation Type"]).Value) == OOAdvantech.UserInterface.CallType.HostingFormOperationCall)
            {
                navigatorType = MetaDataNavigator.NavigatorType.InstanceMember;
                if (properties.Instance is IConnectableControl)
                    browseMetaObject = (properties.Instance as IConnectableControl).UserInterfaceObjectConnection.GetClassifier((((properties.Instance as IConnectableControl).UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site.Container as System.ComponentModel.Design.IDesignerHost).RootComponentClassName, true);
                else
                    browseMetaObject = (properties.Instance as IConnectableControl).UserInterfaceObjectConnection.GetClassifier((((properties.Instance as Menus.MenuCommand).OwnerControl.UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site.Container as System.ComponentModel.Design.IDesignerHost).RootComponentClassName, true);
            }
        }
        /// <MetaDataID>{0E82F4F1-9B18-4C5F-BC48-EFCA5A98CA7D}</MetaDataID>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is IConnectableControl && (context.Instance as IConnectableControl).IsPropertyReadOnly(context.PropertyDescriptor.Name))
                return System.Drawing.Design.UITypeEditorEditStyle.None;

            return System.Drawing.Design.UITypeEditorEditStyle.Modal;

        }

    }

    /// <MetaDataID>{EC6300CA-9B90-4CE1-85B5-BECC528F6223}</MetaDataID>
    public class EditListMetaData : System.Drawing.Design.UITypeEditor
    {
        /// <MetaDataID>{E2DEB570-726A-4F96-B2B9-E4B2371FD2F6}</MetaDataID>
        public override System.Object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, System.Object value)
        {
            if (context.Instance is ListConnection && (context.Instance as ListConnection).PresentationObjectType != null)
            {
                string metaData = value as string;
                ListConnection listView = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    listView = (value as UserInterfaceMetaData.MetaDataValue).MetaDataAsObject as ListConnection;
                }



                ListMetaDataEditor mMetaDataNavigator = new ListMetaDataEditor((context.Instance as ListConnection).PresentationObjectType, context.Instance as IObjectMemberViewControl, listView, context.PropertyDescriptor.Name);
                mMetaDataNavigator.treeView.CheckBoxes = true;

                mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
                mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
                try
                {
                    mMetaDataNavigator.ShowDialog();
                }
                catch (Exception error)
                {


                }

                return listView.MetaData;

            }
            else if (context.Instance is ListConnection)
                return value;



            foreach (System.ComponentModel.Component CurrComponent in context.Container.Components)
            {
                if (CurrComponent is ViewControlObject)
                {

                    OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection mViewControlObject = (CurrComponent as ViewControlObject).UserInterfaceObjectConnection;
                    if (mViewControlObject.PresentationObjectType != null)
                    {
                        MetaDataNavigator mMetaDataNavigator = new MetaDataNavigator(mViewControlObject.PresentationObjectType, context.Instance as IConnectableControl, MetaDataNavigator.NavigatorType.InstanceMember, context.PropertyDescriptor.Name);

                        mMetaDataNavigator.treeView.CheckBoxes = true;


                        mMetaDataNavigator.Location = System.Windows.Forms.Control.MousePosition;

                        if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (mMetaDataNavigator.SelectedMember.IndexOf(mViewControlObject.PresentationObjectType.Name + ".") == 0)
                                return mMetaDataNavigator.SelectedMember.Substring(mViewControlObject.PresentationObjectType.Name.Length + 1);
                            else
                                return mMetaDataNavigator.SelectedMember;
                        }
                        return value;



                    }
                    break;

                }
            }
            return value;


            //if (context.Instance is List.Models.Table)
            //{
            //    bool temp = (context.Instance as List.Models.Table).IsConnectionDataCorrect;
            //    if ( (context.Instance as List.Models.Table).CollectionType!=null)
            //    {

            //        MetaDataNavigator mMetaDataNavigator =new MetaDataNavigator((context.Instance as List.Models.Table).CollectionType);
            //        mMetaDataNavigator.treeView.CheckBoxes = true;
            //        mMetaDataNavigator.Left = System.Windows.Forms.Control.MousePosition.X;
            //        mMetaDataNavigator.Top = System.Windows.Forms.Control.MousePosition.Y;
            //        if (mMetaDataNavigator.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        {
            //            if (mMetaDataNavigator.SelectedMember.IndexOf((context.Instance as List.Models.Table).CollectionType.Name) == 0)
            //                return mMetaDataNavigator.SelectedMember.Substring((context.Instance as List.Models.Table).CollectionType.Name.Length + 1);
            //            else
            //                return mMetaDataNavigator.SelectedMember;
            //        }


            //    }

            //}
            //return null;
        }
        /// <MetaDataID>{0E82F4F1-9B18-4C5F-BC48-EFCA5A98CA7D}</MetaDataID>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;

        }

    }

    /// <MetaDataID>{EC6300CA-9B90-4CE1-85B5-BECC528F6223}</MetaDataID>
    public class EditOperationCallMetaData : System.Drawing.Design.UITypeEditor
    {
        /// <MetaDataID>{E2DEB570-726A-4F96-B2B9-E4B2371FD2F6}</MetaDataID>
        public override System.Object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, System.Object value)
        {
            if (value is UserInterfaceMetaData.MetaDataValue)
                value = (value as UserInterfaceMetaData.MetaDataValue).MetaDataAsObject;


            OperationCallMetaDataEditor searchBoxMetaDataEditor = new OperationCallMetaDataEditor(context, value as OOAdvantech.UserInterface.OperationCall);
            searchBoxMetaDataEditor.Location = System.Windows.Forms.Control.MousePosition;
            searchBoxMetaDataEditor.ShowDialog();
            if (searchBoxMetaDataEditor.BrowseCode && searchBoxMetaDataEditor.Operation != null)
                AssemblyManager.BrowseCode(searchBoxMetaDataEditor.Operation);
            return context.PropertyDescriptor.GetValue(context.Instance);

        }
        /// <MetaDataID>{0E82F4F1-9B18-4C5F-BC48-EFCA5A98CA7D}</MetaDataID>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is IConnectableControl && (context.Instance as IConnectableControl).IsPropertyReadOnly(context.PropertyDescriptor.Name))
                return System.Drawing.Design.UITypeEditorEditStyle.None;

            return System.Drawing.Design.UITypeEditorEditStyle.Modal;

        }

    }
}
