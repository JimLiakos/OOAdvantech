using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Markup.Primitives;
using System.Windows.Data;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{fe18dec8-add6-4440-990c-016f5dd9e9ef}</MetaDataID>
    public class UIElementInitializationBehavior : Behavior<System.Windows.UIElement>
    {

        public UIElementInitializationBehavior()
        {
        }
        public static readonly DependencyProperty UIElementPropertiesProperty = DependencyProperty.Register(
        "UIElementProperties", typeof(FreezableCollection<MethodParameter>), typeof(CallMethodAction), new PropertyMetadata(new PropertyChangedCallback(OnUIElementPropertiesChanged)));

        protected override void OnAttached()
        {
            base.OnAttached();
            MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(this.AssociatedObject);
            foreach (MarkupProperty mp in markupObject.Properties)
            {
                if (mp.DependencyProperty != null)
                {
                    if (mp.Name == "Text")
                    {
                        Binding binding = BindingOperations.GetBinding(AssociatedObject, mp.DependencyProperty);
                        if (binding == null)
                        {
                            binding = new Binding("RealObject.Name");
                            BindingOperations.SetBinding(AssociatedObject,mp.DependencyProperty, binding);
                        }
                    }
                }
            }
        }



        public FreezableCollection<MethodParameter> UIElementProperties
        {
            get
            {
                return (FreezableCollection<MethodParameter>)this.GetValue(UIElementPropertiesProperty);
            }
            set
            {
                this.SetValue(UIElementPropertiesProperty, value);
            }
        }


        private static void OnUIElementPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElementInitializationBehavior obj = d as UIElementInitializationBehavior;
            if (obj != null)
            {
                if (e.NewValue != null)
                    obj.UIElementProperties = e.NewValue as FreezableCollection<MethodParameter>;
            }
        }
    }
}
