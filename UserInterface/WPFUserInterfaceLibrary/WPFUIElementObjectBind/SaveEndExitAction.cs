using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using OOAdvantech.UserInterface.Runtime;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{dbffa05a-69d9-44ef-937e-c2d5c4d63a3e}</MetaDataID>
    public class SaveEndExitAction : TriggerAction<DependencyObject>
    {

        protected override void Invoke(object parameter)
        {

            if (Window.GetWindow(this.AssociatedObject) != null)
            {
                Window.GetWindow(this.AssociatedObject).DialogResult = true;
                Window.GetWindow(this.AssociatedObject).Close();
            }
            //if ((this.AssociatedObject as FrameworkElement).DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
            //{
            //    ((this.AssociatedObject as FrameworkElement).DataContext as OOAdvantech.UserInterface.DynamicUIProxy).UserInterfaceObjectConnection.HostFormClosed(DialogResult.OK);
            //}

            //if (UIProxy.GetUIProxy(this.AssociatedObject as FrameworkElement) != null)
            //{
            //}
            //    UIProxy.GetUIProxy(this.AssociatedObject as FrameworkElement).UserInterfaceObjectConnection.HostFormClosed(DialogResult.OK);

        }
    }
}
