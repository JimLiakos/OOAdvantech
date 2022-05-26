using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Markup.Primitives;
using System.Reflection.Emit;
using OOAdvantech.UserInterface;
using OOAdvantech.UserInterface.Runtime;
namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{ebb9d023-9803-4389-a8f4-d36b13998bb9}</MetaDataID>
    public class ViewControlObject : UserControl, IPresentationContextViewControl, System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <MetaDataID>{94de540a-1c22-423b-9fde-236e0eb849c0}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection;

        /// <MetaDataID>{50c71995-21e4-4a3e-bd31-2f759e5bd2af}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.FormObjectConnection FormObjectConnection;

        /// <MetaDataID>{42b21ee9-7175-46a5-9868-0105edd75558}</MetaDataID>
        public ViewControlObject()
        {
            DynamicUIProxy.ExcludeAssemblyTypes(typeof(BitmapImage).Assembly);
            DynamicUIProxy.ExcludeAssemblyTypes(typeof(Window).Assembly);

            Initialize();
            FormObjectConnection = new OOAdvantech.UserInterface.Runtime.FormObjectConnection(this);
            UserInterfaceObjectConnection = FormObjectConnection;
            //DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ViewControlObjectTypeProperty, typeof(ViewControlObject));
            //if (dpd != null)
            //{
            //    dpd.AddValueChanged(this, delegate
            //    {
            //        ViewControlObjectType = this.GetValue(ViewControlObjectTypeProperty);
            //        // Add property change logic.
            //    });
            //}

            this.Loaded += new RoutedEventHandler(OnHostFormLoad);




        }
        /// <MetaDataID>{28e75c06-b799-4536-956b-a2c0f709d0cd}</MetaDataID>
        public override void EndInit()
        {
            base.EndInit();
        }

        /// <MetaDataID>{88e89d4e-4d8f-425e-937f-c2ba2d2a542c}</MetaDataID>
        void ViewControlObject_Unloaded(object sender, RoutedEventArgs e)
        {

        }
        /// <MetaDataID>{5b372c65-6c86-476b-bc06-5c5e00d0a5cd}</MetaDataID>
        private void Initialize()
        {

            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/WPFConnectableControls;component/ViewControlObject.xaml",
                                    UriKind.Relative);
            this.Resources = resources;
        }

        /// <MetaDataID>{c05cfaa0-4524-46ba-9878-c127b3115186}</MetaDataID>
        bool FromLoaded;
        /// <MetaDataID>{631fc4a9-4b28-4ae6-a90a-c0fdc6049d6d}</MetaDataID>
        ObjectDataProvider _ObjectDataProvider;
        /// <MetaDataID>{efb3d6bb-5b04-4068-9769-57df4b798bcc}</MetaDataID>
        protected override void OnInitialized(EventArgs e)
        {


            base.OnInitialized(e);
            if (Window.GetWindow(this) != null)
            {
                Window.GetWindow(this).Initialized += new EventHandler(ViewControlObject_Initialized);
                Window.GetWindow(this).Closed += new EventHandler(ViewControlObject_Closed);
            }
        }

        void ViewControlObject_Closed(object sender, EventArgs e)
        {
            RefreshControlsTimer.Tick -= new EventHandler(OnRefreshControls);
            RefreshControlsTimer.IsEnabled = false;
            RefreshControlsTimer.Stop();
        }

        /// <MetaDataID>{1c5eba76-805f-490f-bfaa-467b49a8fcf8}</MetaDataID>
        void ViewControlObject_Initialized(object sender, EventArgs e)
        {
            //  GetUIElementHandlers(this.Parent as FrameworkElement);
        }

        System.Windows.Threading.DispatcherTimer RefreshControlsTimer = new System.Windows.Threading.DispatcherTimer();
        /// <MetaDataID>{e97dd6a2-358a-419b-84f2-c468e60ef259}</MetaDataID>
        OOAdvantech.IMember _Source;
        /// <MetaDataID>{a05d3d48-bdb4-4b3b-aa2d-974a4e6e1811}</MetaDataID>
        public OOAdvantech.IMember Source
        {
            get
            {
                if (DesignerProperties.GetIsInDesignMode(this) && UserInterfaceObjectConnection != null)
                    UserInterfaceObjectConnection.State = ViewControlObjectState.DesigneMode;

                if (UserInterfaceObjectConnection == null || UserInterfaceObjectConnection.ObjectType == null)
                    return null;
                else
                {

                    if (UserInterfaceObjectConnection.Instance != null)
                    {
                        if (!FromLoaded)
                        {
                            FormObjectConnection.HostFormLoad();


                            FromLoaded = true;
                            if (!RefreshControlsTimer.IsEnabled)
                            {
                                RefreshControlsTimer.Tick += new EventHandler(OnRefreshControls);
                                RefreshControlsTimer.Interval = TimeSpan.FromMilliseconds(500);
                                RefreshControlsTimer.IsEnabled = true;
                                RefreshControlsTimer.Start();
                            }
                        }
                        if (_Source == null)
                        {
                            //_Source = UserInterfaceObjectConnection.GetDisplayedValue(UserInterfaceObjectConnection.PresentationObject).GetDynamicUIProxy(UserInterfaceObjectConnection, UserInterfaceObjectConnection.PresentationObjectType.GetExtensionMetaObject<Type>());
                            object value = null;
                            DisplayedValue displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(UserInterfaceObjectConnection.PresentationObject);
                            UIProxy uiProxy = displayedValue.GetUIProxy(UserInterfaceObjectConnection);
                            if (uiProxy != null)

                                value = uiProxy.GetTransparentProxy();
                            else
                                value = displayedValue.Value;

                            //_Source = new OOAdvantech.UserInterface.Runtime.UIProxy(UserInterfaceObjectConnection.GetDisplayedValue(UserInterfaceObjectConnection.PresentationObject),UserInterfaceObjectConnection.ObjectType.GetExtensionMetaObject<Type>()).GetTransparentProxy();
                            OOAdvantech.IMember member = Activator.CreateInstance(typeof(OOAdvantech.Member<>).MakeGenericType(_Source.GetType())) as OOAdvantech.IMember;
                            member.Value = value;
                            _Source = member;

                        }

                        return _Source;
                    }
                    else
                    {
                        if (_Source == null)
                        {
                            Type type = OOAdvantech.UserInterface.Runtime.UIProxy.GetClassifierType(UserInterfaceObjectConnection.PresentationObjectType);

                            // type = CodeInjection.EmiProxyType(type);
                            _Source = Activator.CreateInstance(typeof(OOAdvantech.Member<>).MakeGenericType(type)) as OOAdvantech.IMember;
                        }
                        //object obj = new OOAdvantech.UserInterface.Runtime.UIProxy(type).GetTransparentProxy();
                        return _Source;
                    }
                }
            }
        }

        void OnRefreshControls(object sender, EventArgs e)
        {
            UserInterfaceObjectConnection.RefreshUserInterface();
        }



        /// <MetaDataID>{69c0cdf6-2883-4ae4-a849-7b66252b33ae}</MetaDataID>
        void OnHostFormLoad(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {


                FrameworkElement parent = Parent as FrameworkElement;
                while (parent is FrameworkElement &&
                    (parent as FrameworkElement).Parent != null &&
                    (parent as FrameworkElement).Parent.GetType().FullName != "Microsoft.Expression.Platform.WPF.InstanceBuilders.WindowInstance")
                    parent = (parent as FrameworkElement).Parent as FrameworkElement;

                if (parent is FrameworkElement)
                    (parent as FrameworkElement).DataContext = Source;

                return;
            }


            FormObjectConnection.HostFormLoad();
            if (!RefreshControlsTimer.IsEnabled)
            {
                RefreshControlsTimer.Tick += new EventHandler(OnRefreshControls);
                RefreshControlsTimer.Interval = TimeSpan.FromMilliseconds(500);
                RefreshControlsTimer.IsEnabled = true;
                RefreshControlsTimer.Start();
            }


            //(Parent as FrameworkElement).DataContext = Source;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Source"));

            if (Parent is FrameworkElement)
            {
                FrameworkElement parent = Parent as FrameworkElement;
                while (parent is FrameworkElement && !(parent is Window))
                    parent = (parent.Parent as FrameworkElement);

                Window.GetWindow(this).Closing += new CancelEventHandler(ViewControlObject_Closing);
            }
        }

        [Category("Transaction Settings")]
        public bool RollbackOnNegativeAnswer
        {
            get
            {
                return (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnNegativeAnswer;
            }
            set
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnNegativeAnswer = value;
            }
        }
        [Category("Transaction Settings")]
        public bool RollbackOnExitWithoutAnswer
        {
            get
            {
                return (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnExitWithoutAnswer;
            }
            set
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnExitWithoutAnswer = value;
            }
        }


        /// <MetaDataID>{33501cf6-125c-4f0f-b713-6c852013307a}</MetaDataID>
        void ViewControlObject_Closing(object sender, CancelEventArgs e)
        {
            var hostingWindow = sender as Window;
            if (hostingWindow != null)
            {
                foreach (IPage page in ObjectContext.FindChilds<Page>(hostingWindow).OfType<IPage>())
                    page.Closing(e);
            }
            if (!e.Cancel)
            {
                OOAdvantech.UserInterface.Runtime.DialogResult dialogResult = OOAdvantech.UserInterface.Runtime.DialogResult.Cancel;
                if (hostingWindow.DialogResult == null)
                    dialogResult = OOAdvantech.UserInterface.Runtime.DialogResult.None;
                else if (hostingWindow.DialogResult.Value)
                    dialogResult = OOAdvantech.UserInterface.Runtime.DialogResult.OK;
                else
                    dialogResult = OOAdvantech.UserInterface.Runtime.DialogResult.Cancel;
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(dialogResult);
            }
            else
            {

            }
        }


        /// <MetaDataID>{01d1b353-521c-4fe3-beb0-6153794a55a5}</MetaDataID>
        private void GetUIElementHandlers(FrameworkElement frameworkElement)
        {
            GetDependencyProperties(frameworkElement);
            if (frameworkElement == null)
                return;
            UserInterfaceObjectConnection.AddControlledComponent(new UIElementManager(frameworkElement, this));
            if (frameworkElement is ContentControl && (frameworkElement as ContentControl).Content is FrameworkElement)
                GetUIElementHandlers((frameworkElement as ContentControl).Content as FrameworkElement);
            else
                if (frameworkElement is Panel)
            {
                foreach (UIElement childElement in (frameworkElement as Panel).Children)
                    if (childElement is FrameworkElement)
                        GetUIElementHandlers(childElement as FrameworkElement);
            }

        }
        /// <MetaDataID>{16a4cc7c-3990-4db5-9045-8cf1fa3ff253}</MetaDataID>
        private bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }

        /// <MetaDataID>{32abad11-1c61-4fd5-8fbd-982d05879617}</MetaDataID>
        public static List<Binding> GetDependencyProperties(Object element)
        {
            List<Binding> propertyBindings = new List<Binding>();
            //if (element == null)
            return propertyBindings;
            //MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
            //if (markupObject != null)
            //{
            //    foreach (MarkupProperty mp in markupObject.Properties)
            //    {
            //        if (mp.DependencyProperty != null)
            //        {
            //            Binding binding = BindingOperations.GetBinding(element as DependencyObject, mp.DependencyProperty);
            //            if (binding != null)
            //                propertyBindings.Add(binding);
            //        }
            //    }
            //}

            //if (element is ContentControl)
            //    propertyBindings.AddRange(GetDependencyProperties((element as ContentControl).Content));
            //else
            //    if (element is Panel)
            //    {
            //        foreach (UIElement childElement in (element as Panel).Children)
            //            propertyBindings.AddRange(GetDependencyProperties(childElement));
            //    }


            //return propertyBindings;
        }





        ///// <MetaDataID>{ca5ac3a8-1d17-4892-956b-9eb8f4cb1117}</MetaDataID>
        //public static readonly DependencyProperty ViewControlObjectTypeProperty = DependencyProperty.Register(
        //                                                                            "ViewControlObjectType",
        //                                                                            typeof(string),
        //                                                                            typeof(ViewControlObject),
        //                                                                            new PropertyMetadata("ViewControlObjectType name not set."));

        ///// <MetaDataID>{a98df898-6b8a-4129-af7d-eb972e81a3b2}</MetaDataID>
        //public static readonly DependencyProperty AssignPresentationObjectTypeProperty = DependencyProperty.Register(
        //                                                                            "AssignPresentationObjectType",
        //                                                                            typeof(Type),
        //                                                                            typeof(ViewControlObject),
        //                                                                            new PropertyMetadata("AssignPresentationObjectType name not set."));





        //public static readonly DependencyProperty InstanceProperty = DependencyProperty.Register(
        //                                                                    "Instance",
        //                                                                    typeof(string),
        //                                                                    typeof(ViewControlObject),
        //                                                                    new PropertyMetadata("View Control Object Instace"));

        /// <MetaDataID>{8b2daf81-cbbe-4697-a24f-462c0f7da257}</MetaDataID>
        public object Instance
        {
            get
            {

                return UserInterfaceObjectConnection.Instance;
            }
            set
            {

                UserInterfaceObjectConnection.Instance = value;
            }
        }



        /// <MetaDataID>{cc567ff9-d549-46e1-80ab-04aa0b32c987}</MetaDataID>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            //if (e.Property == ViewControlObjectTypeProperty)
            //    ViewControlObjectType = e.NewValue;

            //if (e.Property == AssignPresentationObjectTypeProperty)
            //    AssignPresentationObjectType = e.NewValue as Type;
            base.OnPropertyChanged(e);
        }



        Type _AssignPresentationObjectType;

        /// <MetaDataID>{eb8a6812-c0c8-44d1-b240-d24f9824d5a5}</MetaDataID>
        public Type AssignPresentationObjectType
        {
            get
            {
                return _AssignPresentationObjectType; //this.GetValue(AssignPresentationObjectTypeProperty) as Type;
            }

            set
            {
                if (_AssignPresentationObjectType != value)
                {
                    _Source = null;
                    _AssignPresentationObjectType = value;
                }
                //if (GetValue(AssignPresentationObjectTypeProperty) != value)
                //{
                //    _Source = null;
                //    this.SetValue(AssignPresentationObjectTypeProperty, value);
                //}

                UserInterfaceObjectConnection.PresentationObjectType = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(value);
                UserInterfaceObjectConnection.PresentationObjectTypeFullName = value.FullName;

                //if (value is OOAdvantech.MetaDataRepository.Classifier)
                //{

                //    UserInterfaceObjectConnection.PresentationObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                //    _ViewControlObjectAssembly = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name;

                //}

                if (UserInterfaceObjectConnection.ObjectType != null)
                {
                    UserInterfaceObjectConnection.PresentationObjectTypeFullName = UserInterfaceObjectConnection.ObjectType.FullName;
                    //_ViewControlObjectAssembly = UserInterfaceObjectConnection.ObjectType.ImplementationUnit.Name;
                    //TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                }
                //if (value is string)
                //    UserInterfaceObjectConnection.PresentationObjectTypeFullName = value as string;




                //if (IsInDesignMode)
                //{

                //    FrameworkElement parent = Parent as FrameworkElement;
                //    while (parent is FrameworkElement &&
                //        (parent as FrameworkElement).Parent != null &&
                //        (parent as FrameworkElement).GetType().FullName != "Microsoft.Expression.Platform.WPF.InstanceBuilders.WindowInstance")
                //        parent = (parent as FrameworkElement).Parent as FrameworkElement;

                //    if (parent is FrameworkElement)
                //        (parent as FrameworkElement).DataContext = Source;

                //    return;
                //}



            }
        }

        /// <exclude>Excluded</exclude>
        Type _ViewControlObjectType;

        /// <MetaDataID>{d53dc5cb-bb58-4870-8619-98740f92cb23}</MetaDataID>
        public Type ViewControlObjectType
        {
            get
            {
                return _ViewControlObjectType;
            }

            set
            {
                if (_ViewControlObjectType != value)
                {
                    _Source = null;
                    _ViewControlObjectType = value;
                }
                //if (GetValue(ViewControlObjectTypeProperty) != value)
                //{
                //    _Source = null;
                //    this.SetValue(ViewControlObjectTypeProperty, value);
                //}

                //if (value is OOAdvantech.MetaDataRepository.Classifier)
                //{

                //    UserInterfaceObjectConnection.ViewObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                //    //_ViewControlObjectAssembly = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name;
                //    UserInterfaceObjectConnection.AssemblyMetadata = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name; ;
                //    //TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                //}

                if (UserInterfaceObjectConnection.ObjectType != null)
                {
                    UserInterfaceObjectConnection.ViewObjectTypeFullName = UserInterfaceObjectConnection.ObjectType.FullName;
                    //_ViewControlObjectAssembly = UserInterfaceObjectConnection.ObjectType.ImplementationUnit.Name;
                    //TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                }

                if (_ViewControlObjectType != null)
                {

                    UserInterfaceObjectConnection.ViewObjectTypeFullName = _ViewControlObjectType.FullName;
                    UserInterfaceObjectConnection.AssemblyMetadata = _ViewControlObjectType.Assembly.FullName;
                }
                //if (value is string)
                //    UserInterfaceObjectConnection.ViewObjectTypeFullName = value as string;

                if (IsInDesignMode)
                {

                    FrameworkElement parent = Parent as FrameworkElement;
                    while (parent is FrameworkElement &&
                        (parent as FrameworkElement).Parent != null &&
                        (parent as FrameworkElement).GetType().FullName != "Microsoft.Expression.Platform.WPF.InstanceBuilders.WindowInstance")
                        parent = (parent as FrameworkElement).Parent as FrameworkElement;

                    if (parent is FrameworkElement)
                        (parent as FrameworkElement).DataContext = Source;

                    return;
                }



            }
        }


        /// <MetaDataID>{a365707d-fbc5-498a-b6bd-d7e791be44cf}</MetaDataID>
        [Category("Transaction Settings")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return UserInterfaceObjectConnection.TransactionOption;
            }
            set
            {
                UserInterfaceObjectConnection.TransactionOption = value;
            }
        }





        #region IPresentationContextViewControl Members

        /// <MetaDataID>{0dcb8f65-72f9-4918-99bd-1b8999317250}</MetaDataID>
        public bool InvokeRequired
        {
            get
            {
                return !Dispatcher.CheckAccess();
            }
        }

        /// <MetaDataID>{d62b4186-fd8c-48c4-b6e9-70d06a8139ed}</MetaDataID>
        public void OnBeforeViewControlObjectInitialization()
        {

        }

        /// <MetaDataID>{ce5256f1-1ac1-4d32-a469-a743af77f010}</MetaDataID>
        public void OnAfterViewControlObjectInitialization()
        {

        }

        /// <MetaDataID>{d66e65e6-0af7-4cdc-a4e1-683136ef8194}</MetaDataID>
        public object SynchroInvoke(Delegate method, params object[] args)
        {

            return null;
        }

        /// <MetaDataID>{0516a3e6-95f6-4e5b-8c81-5b58e5032d1e}</MetaDataID>
        public string HostControlName
        {
            get { return Name; }
        }

        /// <MetaDataID>{ff15e8ee-174e-4352-a332-b244aea4ea6b}</MetaDataID>
        public object ContainerControl
        {
            get { return Window.GetWindow(this); }
        }

        /// <MetaDataID>{1607e150-9343-4402-aac1-e7a44242b998}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier ContainerControlType
        {
            get { return null; }
        }

        /// <MetaDataID>{304d6ee6-ca16-48d1-b8c0-8fbc385e0309}</MetaDataID>
        public bool Removed
        {
            get { return false; }
        }

        /// <MetaDataID>{86025192-9362-4357-8eb6-4070013fd7d2}</MetaDataID>
        public void DisableAllControls()
        {

            foreach (var uiElement in FindVisualChildren<UIElement>(Window.GetWindow(this)))
                uiElement.IsEnabled = false;


        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion

        public static OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection GetUserInterfaceObjectConnection(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
            {
                var uiProxy = OOAdvantech.UserInterface.Runtime.UIProxy.GetUIProxy(frameworkElement.DataContext);
                if (uiProxy != null)
                    return uiProxy.UserInterfaceObjectConnection;
            }
            return null;

        }
    }


}

