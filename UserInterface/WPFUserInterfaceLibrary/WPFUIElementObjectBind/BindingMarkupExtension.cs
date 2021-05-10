using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using OOAdvantech.UserInterface.Runtime;
using System.Diagnostics;
using System.Collections;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{661d6978-1c50-47d3-b68d-56e0c9f58357}</MetaDataID>
    [MarkupExtensionReturnType(typeof(object)), Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
    public class BindingExtension : MarkupExtension, IMultiValueConverter, IValueConverter, INotifyPropertyChanged
    {
        /// <MetaDataID>{aa0e63c7-f5ae-4d5c-b9c4-e6b0eb517909}</MetaDataID>
        string _path = null;
        /// <MetaDataID>{4724af8c-8e2c-417f-8733-dedebdbcdf61}</MetaDataID>
        string _elementName = null;
        /// <MetaDataID>{717b2ed2-6e77-43a6-b754-af048b00f3ba}</MetaDataID>
        object _source = null;
        /// <MetaDataID>{8759fc76-665c-49ff-b03a-2a4e274d294d}</MetaDataID>
        Collection<object> _parameters = null;
        /// <MetaDataID>{10a70f46-6f7b-4242-8882-601e5afc2bf9}</MetaDataID>
        DependencyProperty _evaluationProperty = null;
        /// <MetaDataID>{20f8af5c-3a06-4845-bb11-eef64d4d2cb8}</MetaDataID>
        bool _disableNotification = false;
        /// <MetaDataID>{afb05c01-672d-41ca-8604-9ccc3f2559c4}</MetaDataID>
        bool _parametersChanged = false;
        /// <MetaDataID>{c8a280c4-a21f-4dcd-b3f6-b6750351336e}</MetaDataID>
        Collection<object> _evaluatedParameters = null;
        /// <MetaDataID>{7f0ef7b1-4628-4d3a-a8e1-77144c5eb8b2}</MetaDataID>
        MultiBindingExpression _multiBindingExpression;
        /// <MetaDataID>{1632be4f-1abb-4dfa-b66f-585b35bba8f9}</MetaDataID>
        DependencyObject _targetObject;
        /// <MetaDataID>{752201ee-f1d4-4f1b-b20f-c02b01337790}</MetaDataID>
        DependencyProperty _targetProperty;
        /// <MetaDataID>{39959a55-bfa1-46b8-a036-b75f979854bc}</MetaDataID>
        BindingMode _mode = BindingMode.Default;

        class ParameterConverterArgs
        {
            public IValueConverter OriginalConverter;
            public object OriginalParameter;
            public int ParameterIndex;
        }

        /// <MetaDataID>{2b5472c0-0efc-4371-a7cc-44ac0a39cdd6}</MetaDataID>
        public BindingExtension()
        {
            _parameters = new Collection<object>();
        }

        /// <MetaDataID>{e454c90c-4022-4636-be95-3f2b5fceae28}</MetaDataID>
        public BindingExtension(string path)
        {
            _path = path;
            _parameters = new Collection<object>();
        }

        /// <MetaDataID>{15caebd0-ed73-4be4-8a44-44a2cf33bb5a}</MetaDataID>
        public BindingExtension(string path, object arg1)
        {
            _path = path;
            _parameters = new Collection<object>();
            _parameters.Add(arg1);
        }

        //public BindingExtension(string path, object arg1, object arg2)
        //{
        //    _path = path;
        //    _parameters = new Collection<object>();

        //    _parameters.Add(arg1);
        //    _parameters.Add(arg2);
        //}

        //public BindingExtension(string path, object arg1, object arg2, object arg3)
        //{
        //    _path = path;
        //    _parameters = new Collection<object>();

        //    _parameters.Add(arg1);
        //    _parameters.Add(arg2);
        //    _parameters.Add(arg3);
        //}

        //[DefaultValue(null), ConstructorArgument("arg1"), EditorBrowsable(EditorBrowsableState.Never)]
        //public object Arg1
        //{
        //    get { EnsureParametersIndex(0); return _parameters[0]; }
        //    set { EnsureParametersIndex(0); _parameters[0] = value; }
        //}

        //[DefaultValue(null), ConstructorArgument("arg2"), EditorBrowsable(EditorBrowsableState.Never)]
        //public object Arg2
        //{
        //    get { EnsureParametersIndex(1); return _parameters[1]; }
        //    set { EnsureParametersIndex(1); _parameters[1] = value; }
        //}

        //[DefaultValue(null), ConstructorArgument("arg3"), EditorBrowsable(EditorBrowsableState.Never)]
        //public object Arg3
        //{
        //    get { EnsureParametersIndex(2); return _parameters[2]; }
        //    set { EnsureParametersIndex(2); _parameters[2] = value; }
        //}

        /// <MetaDataID>{8409823d-511c-4a84-82c1-db87c6ca9868}</MetaDataID>
        string _EditProperty;
        /// <MetaDataID>{fb50556c-f5f5-432b-aeac-b71242a27103}</MetaDataID>
        [DefaultValue(null), ConstructorArgument("path")]
        public string EditProperty
        {
            get { return _EditProperty; }
            set { _EditProperty = value; }
        }

        /// <MetaDataID>{56cec239-c00d-4e40-bfdb-b0bfcd309bf0}</MetaDataID>
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }

        /// <MetaDataID>{48a99600-29b2-4fb2-835d-be36748526a1}</MetaDataID>
        internal DependencyProperty TargetProperty
        {
            get
            {
                return _targetProperty;
            }
        }


        /// <MetaDataID>{dc270e63-3236-4737-a483-0e3e33ff33ce}</MetaDataID>
        Type _AssignPresentationObjectType;
        /// <MetaDataID>{eb8a6812-c0c8-44d1-b240-d24f9824d5a5}</MetaDataID>
        public Type AssignPresentationObjectType
        {
            get
            {
                return _AssignPresentationObjectType;
            }

            set
            {

                _AssignPresentationObjectType = value;
                //if (GetValue(AssignPresentationObjectTypeProperty) != value)
                //{
                //    _Source = null;
                //    this.SetValue(AssignPresentationObjectTypeProperty, value);
                //}

                //if (value is OOAdvantech.MetaDataRepository.Classifier)
                //{

                //    UserInterfaceObjectConnection.PresentationObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                //    _ViewControlObjectAssembly = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name;
                //    //UserInterfaceObjectConnection.AssemblyMetadata = _ViewControlObjectAssembly;
                //    //TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                //}

                //if (UserInterfaceObjectConnection.ObjectType != null)
                //{
                //    UserInterfaceObjectConnection.PresentationObjectTypeFullName = UserInterfaceObjectConnection.ObjectType.FullName;
                //    //_ViewControlObjectAssembly = UserInterfaceObjectConnection.ObjectType.ImplementationUnit.Name;
                //    //TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                //}
                //if (value is string)
                //    UserInterfaceObjectConnection.PresentationObjectTypeFullName = value as string;





            }
        }


        /// <MetaDataID>{4fdb0528-2804-4ec7-9915-1955b9a21ce3}</MetaDataID>
        System.Windows.Style _UnLockStyle;
        /// <MetaDataID>{53879ae5-97fe-4102-83f5-01f2b8aa400a}</MetaDataID>
        public System.Windows.Style UnLockStyle
        {
            get
            {
                return _UnLockStyle;
            }
            set
            {
                _UnLockStyle = value;
            }
        }

        /// <MetaDataID>{a8f5b3f2-5707-45cc-ad36-382265830aa4}</MetaDataID>
        System.Windows.Style _LockStyle;
        /// <MetaDataID>{015603f0-3906-4019-bfa5-20114eff389c}</MetaDataID>
        public System.Windows.Style LockStyle
        {
            get
            {
                return _LockStyle;
            }
            set
            {
                _LockStyle = value;
            }
        }

        /// <MetaDataID>{81e28f04-cf28-4ca9-82f1-5d7ce46b4826}</MetaDataID>
        public object Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <MetaDataID>{c86ccacf-ab88-484b-bad5-e45559d183b7}</MetaDataID>
        public BindingMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        /// <MetaDataID>{2c573407-d517-4748-930f-ca7e01dc7441}</MetaDataID>
        public PropertyPath Path { get; set; }
        /// <MetaDataID>{eb3f884d-0230-440a-9e03-f77ed78c3fde}</MetaDataID>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return this;
            }

            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
            {
                return this;
            }

            _targetObject = provideValueTarget.TargetObject as DependencyObject;

            //if (_targetObject == null)
            //{
            //    return this;
            //}

            _targetProperty = provideValueTarget.TargetProperty as DependencyProperty;

            // create wpf binding
            MultiBinding mbinding = new MultiBinding();
            mbinding.Mode = _mode;
            mbinding.Converter = this;

            Binding binding1 = new Binding();
            binding1.Mode = BindingMode.OneWay;
            mbinding.Bindings.Add(binding1);

            Binding binding2 = new Binding();
            binding2.Mode = BindingMode.OneWay;
            binding2.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
            mbinding.Bindings.Add(binding2);

            Binding binding3 = new Binding();
            binding3.Mode = BindingMode.OneWay;
            binding3.Source = this;
            binding3.Path = new PropertyPath("EffectiveValueChanged");
            mbinding.Bindings.Add(binding3);

            _evaluatedParameters = new Collection<object>();

            for (int i = 0; i < _parameters.Count; i++)
            {
                object pvalue = _parameters[i];

                if (pvalue is Binding)
                {
                    Binding pbinding = pvalue as Binding;

                    if (!(pbinding.ConverterParameter is ParameterConverterArgs))
                    {
                        pbinding.ConverterParameter = new ParameterConverterArgs()
                        {
                            OriginalConverter = pbinding.Converter,
                            OriginalParameter = pbinding.ConverterParameter,
                            ParameterIndex = i
                        };

                        pbinding.Converter = this;
                        pbinding.Mode = BindingMode.OneWay;
                    }

                    mbinding.Bindings.Add(pbinding);
                }

                _evaluatedParameters.Add(pvalue);
            }

            object value = mbinding.ProvideValue(serviceProvider);
            _multiBindingExpression = value as MultiBindingExpression;

            return value;
        }

        /// <MetaDataID>{0f82165c-9f0f-4bb7-b365-42606ebc6719}</MetaDataID>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object EffectiveValueChanged
        {
            get { return null; }
        }

        /// <MetaDataID>{075b9289-f163-4274-bf2c-ba98cdd26a9e}</MetaDataID>
        internal void NotifyEffectiveValueChanged()
        {
            if (!_disableNotification)
            {
                OnPropertyChanged("EffectiveValueChanged");
            }
        }

        /// <MetaDataID>{1dca687a-4f01-4101-b4bb-50ccf28c0d69}</MetaDataID>
        void EnsureParametersIndex(int index)
        {
            while (_parameters.Count <= index)
            {
                _parameters.Add(null);
            }
        }

        #region IMultiValueConverter Members

        /// <MetaDataID>{b212eb30-63f9-4535-a926-1fd3a26eb877}</MetaDataID>
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            _targetObject = values[1] as DependencyObject;
            if (_targetObject == null)
                return null;
           

            // to allow several dependency properties to be bound to single target object
            // use several attached properties, here just find the next available property to use
            // available, means no binding is set to this property
            if (_evaluationProperty == null)
                _evaluationProperty = PathEvaluationProperties.GetFreeEvaluationProperty(_targetObject, this);

            // create path evaluation binding
            PathEvaluationBinding binding = BindingOperations.GetBindingBase(_targetObject, _evaluationProperty) as PathEvaluationBinding;

            if (binding == null)
            {
                binding = new PathEvaluationBinding(this, _targetObject);
                binding.Path = new PropertyPath(_path, _parameters.ToArray());

                // set binding mode according to the specified in extension by user or in property metadata
                if (_multiBindingExpression != null)
                    binding.Mode = _multiBindingExpression.ParentMultiBinding.Mode;
               

                if (binding.Mode == BindingMode.Default)
                {
                    if (_targetProperty != null)
                    {
                        FrameworkPropertyMetadata mt = _targetProperty.GetMetadata(_targetObject) as FrameworkPropertyMetadata;
                        if (mt != null && mt.BindsTwoWayByDefault)
                            binding.Mode = BindingMode.TwoWay;
                    }
                }
                if (string.IsNullOrEmpty(_elementName))
                    binding.Source = _source;
                else
                {
                    binding.ElementName = _elementName;
                    //if (!string.IsNullOrWhiteSpace(_elementName))
                    //{
                        

                    //   // (_targetObject as System.Windows.Controls.Border).p
                    //    object element =null;// Window.GetWindow(_targetObject).FindName(_elementName);
                    //    FrameworkElement topMostFrameworkElement = _targetObject as FrameworkElement;
                    //    if (topMostFrameworkElement != null)
                    //    {
                    //        while (topMostFrameworkElement.Parent is FrameworkElement)
                    //            topMostFrameworkElement = topMostFrameworkElement.Parent as FrameworkElement;
                    //        // topMostFrameworkElement=
                    //        element = topMostFrameworkElement.FindName(_elementName);
                    //    }
                    //    if (element == null)
                    //        element =  Window.GetWindow(_targetObject).FindName(_elementName);
                       


                    //}
               }
            }

            if (_parametersChanged)
            {
                _parametersChanged = false;

                for (int i = 0; i < _evaluatedParameters.Count; i++)
                {
                    binding.Path.PathParameters[i] = _evaluatedParameters[i];
                }
            }

            try
            {
                // notifications are sent when source value is changed and binding is TwoWay or OneWay
                // when we set binding here, notification is sent also, so disable it to prevent infinite loop
                _disableNotification = true;
                
                BindingOperations.SetBinding(_targetObject, _evaluationProperty, binding);
            }
            catch(System.Exception error)
            {

            }
            finally
            {
                _disableNotification = false;
            }

            object value = binding.EffectiveValue;

            // now we have to convert the value
            if (value != null)
            {
                if (!targetType.IsAssignableFrom(value.GetType()))
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(value);
                    value = tc.ConvertTo(value, targetType);
                }
            }

            return value;
        }

        /// <MetaDataID>{dac0bb30-b157-4d69-b60b-0ae83acd8b4e}</MetaDataID>
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            object[] ret = new object[targetTypes.Length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = DependencyProperty.UnsetValue;
            }

            if (_targetObject != null)
            {
                PathEvaluationBinding binding = BindingOperations.GetBindingBase(_targetObject, _evaluationProperty) as PathEvaluationBinding;

                if (binding != null)
                {
                    _targetObject.SetValue(_evaluationProperty, value);
                }
            }

            return ret;
        }

        #endregion

        #region IValueConverter Members

        /// <MetaDataID>{01df77cc-7552-43ee-8292-770d796207d0}</MetaDataID>
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ParameterConverterArgs args = (ParameterConverterArgs)parameter;

            if (args != null)
            {
                if (args.OriginalConverter != null)
                {
                    value = args.OriginalConverter.Convert(value, targetType, args.OriginalParameter, culture);
                }

                _evaluatedParameters[args.ParameterIndex] = value;
                _parametersChanged = true;
            }

            return value;
        }

        /// <MetaDataID>{aa6152bb-c02d-4d2c-9b0c-f907debafede}</MetaDataID>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{61c6ad70-f5a1-495b-b7d1-e37b522adb1a}</MetaDataID>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    /// <MetaDataID>{870d61b7-7cac-4e4a-87cc-08a12c5fe7ea}</MetaDataID>
    class PathEvaluationBinding : Binding, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        BindingExtension _extension;
        DependencyObject _targetObject = null;
        object _effectiveValue;

        /// <MetaDataID>{004b1650-de02-4e63-a81c-a20219647b4a}</MetaDataID>
        internal PathEvaluationBinding(BindingExtension extension, DependencyObject targetObject)
        {
            _extension = extension;
            _targetObject = targetObject;
        }

        internal BindingExtension Extension
        {
            get { return _extension; }
        }

        internal object EffectiveValue
        {
            get
            {
                try
                {
                    if (_extension == null || _targetObject == null)
                        return null;
                    object value = _targetObject.GetValue(Extension.TargetProperty);
                    FrameworkElement UIElement = _targetObject as FrameworkElement;
                    if (!string.IsNullOrEmpty(_extension.ElementName))
                        UIElement = UIElement.FindName(_extension.ElementName) as FrameworkElement;
                    if (UIElement == null)
                        return null;

                    DependencyProperty editProperty = DependencyPropertyDescriptor.FromName(_extension.EditProperty, UIElement.GetType(), UIElement.GetType()).DependencyProperty;
                    if (editProperty == null)
                        return null;
                    Binding binding = BindingOperations.GetBinding(UIElement, editProperty);
                    if (binding == null)
                        return null;

                    if (UIElement == null)
                        return null;

                    UIProxy uiProxy = UIProxy.GetUIProxy(UIElement.DataContext);

                    if (UIElement.DataContext is OOAdvantech.UserInterface.DynamicUIProxy || uiProxy != null)
                    {
                        UserInterfaceObjectConnection userInterfaceObjectConnection = null;
                        DisplayedValue displayedValue = null;
                        OOAdvantech.UserInterface.DynamicUIProxy dynamicUIProxy = UIElement.DataContext as OOAdvantech.UserInterface.DynamicUIProxy;
                        if (dynamicUIProxy != null)
                        {
                            userInterfaceObjectConnection = dynamicUIProxy.UserInterfaceObjectConnection;
                            displayedValue = dynamicUIProxy.DisplayedValue;
                        }
                        else
                        {
                            userInterfaceObjectConnection = uiProxy.UserInterfaceObjectConnection;
                            displayedValue = uiProxy.DisplayedValue;
                        }


                        bool canEdit = userInterfaceObjectConnection.CanEditValue(displayedValue.Value, displayedValue.Value.GetType(), binding.Path.Path, this);
                        if (Extension.TargetProperty.PropertyType != typeof(bool))
                        {
                            if (Extension.TargetProperty.PropertyType == typeof(Style))
                            {
                                if (Extension.LockStyle != null)
                                {
                                    if (!canEdit)
                                    {
                                        if (value is Style && Extension.LockStyle == (Style)value)
                                            Extension.UnLockStyle = value as Style;
                                        value = Extension.LockStyle;
                                    }
                                    else
                                    {
                                        if (Extension.LockStyle == value as Style)

                                            value = Extension.UnLockStyle;
                                    }
                                }

                                if (value == null)
                                {
                                    Style style = Application.Current.TryFindResource(_targetObject.GetType()) as Style;
                                    value = style;
                                }


                            }

                            return value;


                        }
                        else
                            value = canEdit;
                    }
                    return value;
                }
                catch (Exception error)
                {
                    return null;

                }

            }
        }

        /// <MetaDataID>{e739b627-50d2-4a83-bfe0-7a9229c1302a}</MetaDataID>
        internal void NotifyEffectiveValueChanged(object newValue)
        {
            _effectiveValue = newValue;
            _extension.NotifyEffectiveValueChanged();
        }

        #region IPathDataDisplayer Members

        object OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.Path
        {
            get
            {
                return "";
            }
            set
            {

            }
        }

        /// <MetaDataID>{898e4a7a-d195-44b3-a453-20f9d373ef31}</MetaDataID>
        public void LoadControlValues()
        {

        }

        /// <MetaDataID>{29910887-cb14-4daa-816b-faa556b318f2}</MetaDataID>
        public void SaveControlValues()
        {

        }

        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                if (_extension == null || _targetObject == null)
                    return null;

                FrameworkElement UIElement = _targetObject as FrameworkElement;
                if (!string.IsNullOrEmpty(_extension.ElementName))
                    UIElement = UIElement.FindName(_extension.ElementName) as FrameworkElement;
                if (UIElement == null)
                    return null;

                DependencyProperty editProperty = DependencyPropertyDescriptor.FromName(_extension.EditProperty, UIElement.GetType(), UIElement.GetType()).DependencyProperty;
                if (editProperty == null)
                    return null;
                Binding binding = BindingOperations.GetBinding(UIElement, editProperty);
                if (binding == null)
                    return null;

                if (UIElement == null)
                    return null;


                if (UIElement.DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
                    return (UIElement.DataContext as OOAdvantech.UserInterface.DynamicUIProxy).UserInterfaceObjectConnection;
                return null;

            }
        }

        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<string>();
            }
        }

        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            _extension.NotifyEffectiveValueChanged();
        }

        public void LockStateChange(object sender)
        {

        }

        #endregion
    }

    /// <MetaDataID>{adc993e7-5f04-4827-810d-890fdab7d850}</MetaDataID>
    [EditorBrowsable(EditorBrowsableState.Never)]
    class PathEvaluationProperties : DependencyObject
    {
        const int MAX_PROPERTIES = 20;
        public static readonly DependencyProperty[] EvaluationProperty;

        static PathEvaluationProperties()
        {
            EvaluationProperty = new DependencyProperty[MAX_PROPERTIES];

            for (int i = 0; i < MAX_PROPERTIES; i++)
            {
                EvaluationProperty[i] = DependencyProperty.RegisterAttached(
                    string.Format("Evaluation{0}", i + 1), typeof(object),
                    typeof(PathEvaluationProperties),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(EvaluationPropertyChangedCallback)));
            }
        }

        static void EvaluationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PathEvaluationBinding b = BindingOperations.GetBindingBase(d, e.Property) as PathEvaluationBinding;

            if (b != null)
            {
                b.NotifyEffectiveValueChanged(e.NewValue);
            }
        }

        internal static DependencyProperty GetFreeEvaluationProperty(DependencyObject d, BindingExtension e)
        {
            for (int i = 0; i < MAX_PROPERTIES; i++)
            {
                PathEvaluationBinding b = BindingOperations.GetBindingBase(d, EvaluationProperty[i]) as PathEvaluationBinding;

                if (b != null && b.Extension == e)
                {
                    return EvaluationProperty[i];
                }

                if (b == null)
                {
                    return EvaluationProperty[i];
                }
            }

            throw new InvalidOperationException("There are no more free evaluation properties left");
        }
    }

    /// <summary>
    /// Converter to debug the binding values
    /// </summary>
    /// <MetaDataID>{f3970ab5-d6c3-47c0-b23d-1fa437be3ace}</MetaDataID>
    public class PresentationObjectConvertor : IValueConverter
    {
        DebugBindingExtension DebugBindingExtension;
        IProvideValueTarget ProvideValueTarget;
        object _Target;
        public PresentationObjectConvertor(DebugBindingExtension debugBindingExtension, IProvideValueTarget provideValueTarget)
        {
            DebugBindingExtension = debugBindingExtension;
            ProvideValueTarget = provideValueTarget;
            if (provideValueTarget != null)
                _Target = ProvideValueTarget.TargetObject;
        }

        public PresentationObjectConvertor()
        {

        }


        public Type AssignPresentationObjectType
        {
            get;
            set;
        }

        #region IValueConverter Members

        /// <summary>
        /// ask the debugger to break
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable)
                return ConvertCollection(value as IEnumerable);
            return value;
            Debugger.Break();
            return Binding.DoNothing;
        }
        object ConvertCollection(IEnumerable collection)
        {
            if (AssignPresentationObjectType == null && DebugBindingExtension != null)
                AssignPresentationObjectType = DebugBindingExtension.AssignPresentationObjectType;

            Type presentationObjectGeneric = AssignPresentationObjectType;
            while (presentationObjectGeneric!=null&& presentationObjectGeneric != typeof(object) && !presentationObjectGeneric.IsGenericType)
                presentationObjectGeneric = presentationObjectGeneric.BaseType;


           if(presentationObjectGeneric==null || presentationObjectGeneric.GetGenericTypeDefinition()!=typeof(OOAdvantech.UserInterface.Runtime.PresentationObject<>))
               return collection;


            Type orgType=presentationObjectGeneric.GetGenericArguments()[0];
            IList list = null;
            UserInterfaceObjectConnection userInterfaceObjectConnection=null;
            foreach (var item in collection)
            {
                UIProxy uiProxy = UIProxy.GetUIProxy(item);
                if (uiProxy != null)
                {
                    userInterfaceObjectConnection = uiProxy.UserInterfaceObjectConnection;
                    break;
                }
            }

            foreach (var item in collection)
            {

                object realObject=null;
                UIProxy uiProxy = UIProxy.GetUIProxy(item);
                if(uiProxy !=null)
                    realObject=uiProxy.RealTransparentProxy;
                else
                    realObject=item;

               
               

                object wrappedObject = userInterfaceObjectConnection.GetPresentationObject(realObject, OOAdvantech.MetaDataRepository.Classifier.GetClassifier(AssignPresentationObjectType) as OOAdvantech.MetaDataRepository.Class,orgType );
                //object wrappedObject = System.Activator.CreateInstance(AssignPresentationObjectType, realObject);
                object proxyItem = userInterfaceObjectConnection.GetDisplayedValue(wrappedObject).GetUIProxy(userInterfaceObjectConnection).GetTransparentProxy();
                if (list == null)
                {
                    //if (collection.GetType().IsGenericType && typeof(System.Collections.ObjectModel.ReadOnlyCollection<>) == collection.GetType().GetGenericTypeDefinition())
                        list = Activator.CreateInstance(typeof(List<>).MakeGenericType(AssignPresentationObjectType)) as IList;
                    //else
                    //    list = Activator.CreateInstance(collection.GetType().get) as IList;//typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
                }
                list.Add(proxyItem);
            }

            object retValue = null;
            if (list == null)
                retValue = collection;
            else
            {
                if (collection.GetType().IsGenericType && typeof(System.Collections.ObjectModel.ReadOnlyCollection<>) == collection.GetType().GetGenericTypeDefinition())
                    retValue = list.GetType().GetMethod("AsReadOnly").Invoke(list, new object[0]);
                else
                    retValue = list;
            }
            return retValue;
        }

        /// <summary>
        /// ask the debugger to break
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
            Debugger.Break();
            return Binding.DoNothing;
        }

        #endregion
    }

    /// <summary>
    /// Markup extension to debug databinding
    /// </summary>
    /// <MetaDataID>{3f768523-bb49-4a42-82f2-0d9e0c2848c1}</MetaDataID>
    public class DebugBindingExtension : MarkupExtension
    {

        public DebugBindingExtension()
        {

        }


        public DebugBindingExtension(string path)
        {
            _PropertyPath = new PropertyPath(path, new Collection<object>());
        }

        public DebugBindingExtension(string path, object arg1)
        {
            _PropertyPath = new PropertyPath(path, new Collection<object>() { arg1 });
        }
        PropertyPath _PropertyPath;

        public PropertyPath Path
        {
            get
            {
                return _PropertyPath;
            }
            set
            {
                _PropertyPath = value;
            }
        }

        public Type AssignPresentationObjectType
        {
            get;
            set;
        }
        /// <summary>
        /// Creates a new instance of the Convertor for debugging
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>Return a convertor that can be debugged to see the values for the binding</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {


            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            //Binding binding = new Binding();
            //binding.Path = Path;
            //binding.Converter = new DebugConvertor();

            //return binding.ProvideValue(serviceProvider);
            //object value = Path;
            //return value;
            //return "Binding";
            return new PresentationObjectConvertor(this, provideValueTarget);
        }
    }
}
