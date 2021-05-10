using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PushBindingExtension
{
    /// <MetaDataID>{7a2c533c-e886-483f-8f20-ed52252166ce}</MetaDataID>
    public class PushBindingManager
    {
        public static DependencyProperty PushBindingsProperty =
            DependencyProperty.RegisterAttached("PushBindingsInternal",
                                                typeof(PushBindingCollection),
                                                typeof(PushBindingManager),
                                                new UIPropertyMetadata(null));

        public static PushBindingCollection GetPushBindings(FrameworkElement obj)
        {
            if (obj.GetValue(PushBindingsProperty) == null)
            {
                obj.SetValue(PushBindingsProperty, new PushBindingCollection(obj));
            }
            return (PushBindingCollection)obj.GetValue(PushBindingsProperty);
        }
        public static void SetPushBindings(FrameworkElement obj, PushBindingCollection value)
        {
            obj.SetValue(PushBindingsProperty, value);
        }
    }
}
