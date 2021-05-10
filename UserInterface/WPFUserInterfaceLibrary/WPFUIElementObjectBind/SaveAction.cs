using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using OOAdvantech.UserInterface.Runtime;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{9a7e0731-6e30-4ca2-88c5-4ba88c1e41ed}</MetaDataID>
    public class SaveAction : TriggerAction<DependencyObject>
    {

        /// <MetaDataID>{e05f3980-838a-4462-8401-e0f5e9cb0df1}</MetaDataID>
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        /// <MetaDataID>{ee9880f4-c63d-4804-b844-c48b3376a88e}</MetaDataID>
        protected override void Invoke(object parameter)
        {
            if ((this.AssociatedObject as FrameworkElement).DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
            {
                ((this.AssociatedObject as FrameworkElement).DataContext as OOAdvantech.UserInterface.DynamicUIProxy).UserInterfaceObjectConnection.Save();
            }
            if (Window.GetWindow(this.AssociatedObject) != null)
            {
                if (UIProxy.GetUIProxy((this.AssociatedObject as FrameworkElement).DataContext ) != null)
                    UIProxy.GetUIProxy((this.AssociatedObject as FrameworkElement).DataContext).UserInterfaceObjectConnection.Save();
            }
        }



    }
}
