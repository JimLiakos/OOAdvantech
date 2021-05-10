using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Data;
using OOAdvantech.Transactions;
using OOAdvantech.UserInterface.Runtime;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{72ef85bc-6a29-4bdf-a6b4-44a777ad91fd}</MetaDataID>
    public class CallMethodAction : TriggerAction<DependencyObject>
    {

        /// <MetaDataID>{e05f3980-838a-4462-8401-e0f5e9cb0df1}</MetaDataID>
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        /// <MetaDataID>{ee9880f4-c63d-4804-b844-c48b3376a88e}</MetaDataID>
        protected override void Invoke(object parameter)
        {


            object[] _params = new object[MethodParameters.Count];
            int i = 0;
            foreach (var methodParameter in MethodParameters)
                _params[i++] = methodParameter.ParameterValue;

            object calledObject = null;

            if (Binding == null)
                calledObject = (this.AssociatedObject as FrameworkElement).DataContext;
            else
                calledObject = Binding;



            if (calledObject != null)
            {
                System.Reflection.MethodInfo method = calledObject.GetType().GetMethod(MethodName);
                if (method == null)
                    return;
                if (calledObject is OOAdvantech.UserInterface.DynamicUIProxy)
                {
                    (calledObject as OOAdvantech.UserInterface.DynamicUIProxy).UserInterfaceObjectConnection.Invoke(calledObject, method, _params, TransactionOption);
                }
                else if (UIProxy.GetUIProxy( calledObject) !=null)
                {
                    UIProxy uiProxy = UIProxy.GetUIProxy(calledObject);

                    uiProxy.UserInterfaceObjectConnection.Invoke(uiProxy.RealTransparentProxy, method, _params, TransactionOption);
                }
                else
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        method.Invoke(calledObject, _params);
                        stateTransition.Consistent = true;
                    }
                }
        
            }

        }

        public static readonly DependencyProperty TransactionOptionProperty = DependencyProperty.Register(
                                                                "TransactionOption",
                                                                typeof(TransactionOption),
                                                                typeof(CallMethodAction),
                                                                new UIPropertyMetadata(TransactionOption.Supported, new PropertyChangedCallback(OnTransactionOptionChanged)));

        
        public TransactionOption TransactionOption
        {
            get
            {
                object value= GetValue(TransactionOptionProperty);
                if (value is TransactionOption)
                    return (TransactionOption)value;
                else
                    return TransactionOption.Supported;

            }
            set
            {
                SetValue(TransactionOptionProperty, value);
            }
        }

        /// <MetaDataID>{e47c8591-1912-41b1-9820-950fcc2527a9}</MetaDataID>
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
                                                                    "MethodName",
                                                                    typeof(string),
                                                                    typeof(CallMethodAction),
                                                                    new PropertyMetadata(new PropertyChangedCallback(OnMethodNameChanged)));

        //public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        //                                                          "CalledObject",
        //                                                          typeof(string),
        //                                                          typeof(CallMethodAction),
        //                                                          new PropertyMetadata(new PropertyChangedCallback(OnMethodNameChanged)));
 
        /// <MetaDataID>{d16d4011-a358-44e5-bef1-0a942cc5356e}</MetaDataID>
        public string MethodName
        {
            get
            {
                return GetValue(MethodNameProperty) as string;
            }
            set
            {
                SetValue(MethodNameProperty, value);
            }
        }
        private static void OnTransactionOptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CallMethodAction callMethod = d as CallMethodAction;
            if (callMethod != null)
            {
                if (e.NewValue != null)
                    callMethod.TransactionOption =(TransactionOption)e.NewValue ;
            }
        }
        private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CallMethodAction callMethod = d as CallMethodAction;
            if (callMethod != null)
            {
                if (e.NewValue != null)
                    callMethod.Binding = e.NewValue ;
            }
        }


        
        /// <MetaDataID>{1ac1d3b4-c039-473a-bf61-0a1414fd3942}</MetaDataID>
        private static void OnMethodNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CallMethodAction callMethod = d as CallMethodAction;
            if (callMethod != null)
            {
                if (e.NewValue != null)
                    callMethod.MethodName = e.NewValue as string;
            }
        }
        /// <MetaDataID>{8e326ddf-7a7e-4e84-9f50-a225546aeee7}</MetaDataID>
        public CallMethodAction()
        {
            this.MethodParameters = new FreezableCollection<MethodParameter>();
        }
        /// <MetaDataID>{5aeba9b0-2b66-4824-8e3c-5060647a29be}</MetaDataID>
        public static readonly DependencyProperty MethodParametersProperty = DependencyProperty.Register(
            "MethodParameters", typeof(FreezableCollection<MethodParameter>), typeof(CallMethodAction), new PropertyMetadata(new PropertyChangedCallback(OnMethodParametersChanged)));


        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
                 "Binding", typeof(object), typeof(CallMethodAction),new PropertyMetadata(new PropertyChangedCallback(OnBindingChanged)));


        /// <MetaDataID>{e7a48dfc-ab36-447d-9bf4-caa1febee65f}</MetaDataID>
        public FreezableCollection<MethodParameter> MethodParameters
        {
            get
            {
                return (FreezableCollection<MethodParameter>)this.GetValue(MethodParametersProperty);
            }
            set
            {
                this.SetValue(MethodParametersProperty, value);
            }
        }

        public object Binding 
        { 
            get 
            { 
                return GetValue(BindingProperty) ; 
            }
            set
            {
                SetValue(BindingProperty, value);
            }
        }
        /// <MetaDataID>{b53624d7-34f6-48f3-8088-eb63df9e86b9}</MetaDataID>
        private static void OnMethodParametersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CallMethodAction obj = d as CallMethodAction;
            if (obj != null)
            {
                if (e.NewValue != null)
                {
                    obj.MethodParameters = e.NewValue as FreezableCollection<MethodParameter>;
                }
            }
        }
    }
    /// <MetaDataID>{8e71d0d6-3f38-4667-bf4e-c3a3acba3018}</MetaDataID>
    public class MethodParameter : Freezable
    {
        /// <MetaDataID>{fb9145db-664c-42e7-805f-7a0741df64e6}</MetaDataID>
        protected override Freezable CreateInstanceCore()
        {
            return new MethodParameter();
        }
        /// <MetaDataID>{55537414-27b5-4ed0-b2e7-54d6c0c5da23}</MetaDataID>
        public static readonly DependencyProperty ParameterValueProperty = DependencyProperty.Register(
                                                                            "ParameterValue",
                                                                            typeof(object),
                                                                            typeof(MethodParameter),
                                                                            new PropertyMetadata(new PropertyChangedCallback(OnParameterValueChanged)));
        /// <MetaDataID>{f449e1c5-a6c8-4732-b6ec-75ca8ef88313}</MetaDataID>
        public object ParameterValue
        {
            get
            {
                return GetValue(ParameterValueProperty);
            }
            set
            {
                SetValue(ParameterValueProperty, value);
            }
        }
        /// <MetaDataID>{4aa0ad16-18af-4058-9986-6295d75b55e3}</MetaDataID>
        private static void OnParameterValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MethodParameter methodParameter = d as MethodParameter;
            if (methodParameter != null)
            {
                if (e.NewValue != null)
                    methodParameter.ParameterValue = e.NewValue;
            }
        }



        /// <MetaDataID>{a565c563-7db9-4e2a-8341-8528c5a67878}</MetaDataID>
        public static readonly DependencyProperty ParameterNameProperty = DependencyProperty.Register(
                                                                    "ParameterName",
                                                                    typeof(string),
                                                                    typeof(MethodParameter),
                                                                    new PropertyMetadata(new PropertyChangedCallback(OnParameterNameChanged)));

        /// <MetaDataID>{3d82746d-3a69-4a51-a100-060649c7d9d5}</MetaDataID>
        public object ParameterName
        {
            get
            {
                return GetValue(ParameterNameProperty);
            }
            set
            {
                SetValue(ParameterNameProperty, value);
            }
        }
        /// <MetaDataID>{d419866a-b0eb-46b7-9239-3374f0df3a4b}</MetaDataID>
        private static void OnParameterNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MethodParameter methodParameter = d as MethodParameter;
            if (methodParameter != null)
            {
                if (e.NewValue != null)
                    methodParameter.ParameterName = e.NewValue as string;
            }
        }
    }

}
