using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.UserInterface.Runtime;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{28aec8db-bb98-416d-80dd-35e197432dad}</MetaDataID>

    public class ObjectContext : OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl, System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <MetaDataID>{3d111abe-4757-4b53-8770-8072161418ca}</MetaDataID>
        System.Windows.FrameworkElement HosttingElement;
        #region IPresentationContextViewControl Members


        /// <MetaDataID>{4b4f1e9d-7558-440d-86d1-c6126baae5e1}</MetaDataID>
        System.Windows.Threading.Dispatcher Dispatcher
        {
            get
            {
                return HosttingElement.Dispatcher;
            }
        }
        /// <MetaDataID>{0dcb8f65-72f9-4918-99bd-1b8999317250}</MetaDataID>
        public bool InvokeRequired
        {
            get
            {
                return !Dispatcher.CheckAccess();
            }
        }
        /// <MetaDataID>{794e30cc-cbbd-4e9f-b46f-7976f48ba6a8}</MetaDataID>
        public System.Globalization.CultureInfo Culture
        {
            get
            {
                if (FormObjectConnection != null)
                    return FormObjectConnection.Culture;
                else return System.Globalization.CultureInfo.CurrentCulture;
            }
        }

        /// <MetaDataID>{797e5b65-a62a-4a3e-bfda-5041c372aebb}</MetaDataID>
        public bool UseDefaultCultureWhenValueMissing
        {
            get
            {
                if (FormObjectConnection != null)
                    return FormObjectConnection.UseDefaultCultureWhenValueMissing;

                return false;
            }
            set
            {

                if (FormObjectConnection != null)
                    FormObjectConnection.UseDefaultCultureWhenValueMissing = value;
            }
        }

        /// <MetaDataID>{fe4ab87b-954f-40f3-a693-120dac393b11}</MetaDataID>
        RelayCommand _OnOKCommand;
        /// <MetaDataID>{27eb464c-b731-46cd-a9b2-90b3d6c6ff6f}</MetaDataID>
        public RelayCommand OnOKCommand { get { return _OnOKCommand; } private set { _OnOKCommand = value; } }

        /// <MetaDataID>{4b9db6f3-912f-429e-950f-408a442f9aeb}</MetaDataID>
        RelayCommand _OnCancelCommand;
        /// <MetaDataID>{7d26f35e-bcfe-4106-9798-95db9cf8a116}</MetaDataID>
        public RelayCommand OnCancelCommand { get { return _OnCancelCommand; } private set { _OnCancelCommand = value; } }


        /// <MetaDataID>{a0ff9845-07c4-412d-809b-ab3014bb27b0}</MetaDataID>
        public RelayCommand UseStyleDefaultsCommand;
        /// <MetaDataID>{d62b4186-fd8c-48c4-b6e9-70d06a8139ed}</MetaDataID>
        public void OnBeforeViewControlObjectInitialization()
        {

        }
        /// <MetaDataID>{0c682eee-e441-4e38-adc6-ad4108083fbf}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{e081410c-885f-4a2c-97f3-43e1ce23556d}</MetaDataID>
        public static object GetRealObject(object _obj)
        {
            object realObject = null;
            UIProxy uiProxy = UIProxy.GetUIProxy(_obj);
            if (uiProxy != null)
                realObject = uiProxy.RealTransparentProxy;
            else
                realObject = _obj;
            return realObject;
        }

        /// <MetaDataID>{b6bdbe21-ec25-43cd-832d-27e0aec8f6c5}</MetaDataID>
        public T GetRealObject<T>(object _obj)
        {
            object realObject = GetRealObject(_obj);
            if (realObject is T)
                return (T)realObject;
            else
                return default(T);

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
            get { return HosttingElement.Name; }
        }

        /// <MetaDataID>{ff15e8ee-174e-4352-a332-b244aea4ea6b}</MetaDataID>
        public object ContainerControl
        {
            get
            {
                if (IsInDesignMode)
                    return null;
                //return Window.GetWindow(HosttingElement);
                return HosttingElement;
            }
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
            if (HosttingElement is IPage)
            {
                (HosttingElement as IPage).ObjectContextTransactionAborted();
            }
            else
            {
                foreach (var uiElement in FindVisualChildren<UIElement>(Window.GetWindow(HosttingElement)))
                {
                    uiElement.IsEnabled = false;
                    if (uiElement is System.Windows.Controls.Button)
                    {
                        var button = uiElement as System.Windows.Controls.Button;
                        if (button.Command == OnCancelCommand || button.Command is WindowCloseCommand || button.Command is WindowMinimizeCommand || button.Command is WindowMaximizeCommand)
                        {
                            button.IsEnabled = true;
                            UIElement parent = VisualTreeHelper.GetParent(button) as UIElement;
                            while (parent != null)
                            {
                                parent.IsEnabled = true;
                                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                            }
                        }
                    }
                }

                if (Window.GetWindow(HosttingElement) != null)
                    Window.GetWindow(HosttingElement).Title = Window.GetWindow(HosttingElement).Title + "    ( Transaction Aborted )";
            }
        }
        /// <MetaDataID>{fd0e01b2-df94-4a8d-9271-b22651aa15c8}</MetaDataID>
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


        /// <MetaDataID>{e9bca494-ec81-4fae-8911-d4486e31ac22}</MetaDataID>
        public static List<T> FindChilds<T>(DependencyObject parent) where T : DependencyObject
        {

            List<T> foundChilds = new List<T>();

            if (parent == null)
                return foundChilds;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChilds.AddRange(FindChilds<T>(child));

                    // If the child is found, break so we do not overwrite the found child.
                    //if (foundChild != null) break;
                }
                else
                {
                    // child element found.
                    foundChilds.Add((T)child);

                    foundChilds.AddRange(FindChilds<T>(child));

                }
            }
            return foundChilds;
        }
        /// <MetaDataID>{4a4bce12-c837-4f1f-9d74-dd1f082bc5e3}</MetaDataID>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {

            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        /// <MetaDataID>{4e9f556b-644c-47b4-9e64-13a7d180e53d}</MetaDataID>
        public static List<DependencyObject> GetAncestors(DependencyObject child)
        {
            List<DependencyObject> ancestors = new List<DependencyObject>();
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            while (parentObject != null)
            {
                ancestors.Add(parentObject);
                child = parentObject;
                parentObject = VisualTreeHelper.GetParent(child);


            }
            return ancestors;

        }

        /// <MetaDataID>{e20ec612-8723-4267-bc5b-887afe385543}</MetaDataID>
        public bool HasChanges(bool checkOnlyPersistentClassInstances)
        {

            {
                if (FormObjectConnection != null && FormObjectConnection.Transaction != null && FormObjectConnection.Transaction.HasChangesOnElistedObjects(checkOnlyPersistentClassInstances))
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{ad634864-1d61-4702-b477-444af4eddea6}</MetaDataID>
        public void Save()
        {
            FormObjectConnection.Save();
        }


        /// <MetaDataID>{b285de5c-5b6f-420f-8b49-5a802a5963ca}</MetaDataID>
        public ObjectContext()
        {
            FormObjectConnection = new FormObjectConnection(this);

            OnOKCommand = new RelayCommand((object sender) =>
            {
                OnOK();
            });
            OnCancelCommand = new RelayCommand((object sender) =>
            {
                OnCancel();
            });

            try
            {
                typeof(UIBaseEx.SizeUtil).GetField("DPI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, new System.Windows.LengthConverter().ConvertFromString("1in"));
            }
            catch (Exception error)
            {
            }

            // typeof(UIBaseEx.SizeUtil).GetField("DPI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null,new System.Windows.LengthConverter().ConvertFromString("1in"), null);

        }
        /// <MetaDataID>{aff6152b-5b19-49df-83df-4481e0085df0}</MetaDataID>
        protected void OnOK()
        {
            if (HosttingElement != null)
            {
                Window.GetWindow(HosttingElement).DialogResult = true;
                Window.GetWindow(HosttingElement).Close();
            }
        }


        /// <MetaDataID>{c0f01618-821c-4889-8fb0-3181ffab7246}</MetaDataID>
        public object RunUnderContextTransaction(Delegate method, params object[] args)
        {

            return FormObjectConnection.Invoke(method.Target, method.Method, args, OOAdvantech.Transactions.TransactionOption.Supported);

        }

        /// <MetaDataID>{37365853-1517-47b8-8acb-39b9189d2c4e}</MetaDataID>
        protected void OnCancel()
        {
            if (HosttingElement != null)
            {
                Window.GetWindow(HosttingElement).DialogResult = false;
                Window.GetWindow(HosttingElement).Close();
            }

        }

        delegate void RefreshUserInterfaceHandle();

        /// <MetaDataID>{605fb146-b68f-4c66-a83e-e12007884730}</MetaDataID>
        System.Timers.Timer RefreshControlsTimer = new System.Timers.Timer();
        /// <MetaDataID>{b95498d1-400a-4439-95b7-1a530f2f885f}</MetaDataID>
        bool WindowLoaded;

        /// <MetaDataID>{e35699d3-b02a-4dc5-86c8-9a1debe0a773}</MetaDataID>
        internal OOAdvantech.UserInterface.Runtime.FormObjectConnection FormObjectConnection;
        /// <MetaDataID>{d49b4fb5-688e-491e-abc5-7d713ea2e75b}</MetaDataID>
        void OnRefreshControls(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {

                try
                {
                    FormObjectConnection.RefreshUserInterface();
                }
                catch (Exception error)
                {

                }

            }));

            //if (!Dispatcher.CheckAccess())
            //{
            //    Dispatcher.BeginInvoke()
            //    if (FormObjectConnection.PresentationContextViewControl.InvokeRequired)
            //        FormObjectConnection.PresentationContextViewControl.SynchroInvoke(new RefreshUserInterfaceHandle(RefreshUserInterface));
            //    else
            //        RefreshUserInterface();
            //}
        }


        /// <MetaDataID>{10fa83a0-6aa9-4578-bc37-3db3c9256dce}</MetaDataID>
        bool IsInitialized;
        /// <MetaDataID>{2d687e01-6f45-44ee-bb01-9cfb0e9d60be}</MetaDataID>
        public void Initialize(FrameworkElement hostingElement)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                HosttingElement = hostingElement;

                if (!IsInDesignMode)
                {
                    HosttingElement.Initialized += new EventHandler(ViewControlObject_Initialized);
                    HosttingElement.Loaded += new RoutedEventHandler(OnHostFormLoad);


                    if (Window.GetWindow(HosttingElement) != null && Window.GetWindow(HosttingElement).IsLoaded)
                        OnHostFormLoad(Window.GetWindow(HosttingElement), null);
                }
            }
        }



        /// <MetaDataID>{80ed8425-cf45-49a8-ad4d-f7aa49d4c0d7}</MetaDataID>
        public void SetContextInstance(object instance)
        {
            if (!IsInitialized)
                throw new Exception("Call Initialize method first and then set object context instance");


            UIProxy uiProxy = UIProxy.GetUIProxy(instance);
            if (uiProxy != null)
                instance = uiProxy.RealTransparentProxy;



            FormObjectConnection.Instance = instance;

            _Value = null;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));

            //HosttingElement = hostingElement;
            //if (Window.GetWindow(HosttingElement) != null)
            //{
            //    Window.GetWindow(HosttingElement).Initialized += new EventHandler(ViewControlObject_Initialized);
            //    Window.GetWindow(HosttingElement).Closed += new EventHandler(ViewControlObject_Closed);
            //    HosttingElement.Loaded += new RoutedEventHandler(OnHostFormLoad);
            //}
            //FormObjectConnection.Instance = instance;
            //_Value = null;
            //if (PropertyChanged != null)
            //    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            //if (Window.GetWindow(HosttingElement).IsLoaded)
            //    OnHostFormLoad(Window.GetWindow(HosttingElement), null);

        }


        /// <MetaDataID>{8fed906a-4ce7-4db3-8da1-0148bc31ed1b}</MetaDataID>
        void OnHostFormLoad(object sender, RoutedEventArgs e)
        {
            if (!WindowLoaded)
                FormObjectConnection.HostFormLoad();
            if (!RefreshControlsTimer.Enabled)
            {
                RefreshControlsTimer.Elapsed += OnRefreshControls;
                // RefreshControlsTimer.Tick += new EventHandler(OnRefreshControls);
                RefreshControlsTimer.Interval = (500);
                RefreshControlsTimer.Enabled = true;
                RefreshControlsTimer.Start();
            }


            //(Parent as FrameworkElement).DataContext = Source;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Source"));

            if (HosttingElement is FrameworkElement)
                Window.GetWindow(HosttingElement).Closing += new CancelEventHandler(ViewControlObject_Closing);

        }



        /// <MetaDataID>{c48e0a19-eac7-4559-95d5-a04c25060fa3}</MetaDataID>
        void ViewControlObject_Closing(object sender, CancelEventArgs e)
        {

            var hostingWindow = sender as Window;
            if (hostingWindow != null)
            {
                foreach (IPage pageUIElement in ObjectContext.FindChilds<Page>(hostingWindow).OfType<IPage>())
                {
                    foreach (var page in pageUIElement.NavigationServicePages)
                    {
                        if (page.Closing(e))
                            page.Close();
                        else
                        {
                            break;
                        }

                    }

                }
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
                FormObjectConnection.HostFormClosed(dialogResult);
            }
            else
            {

            }

        }

        /// <MetaDataID>{c8f35cdc-ec69-4925-8e04-b2e1a98bd94b}</MetaDataID>
        void ViewControlObject_Closed(object sender, EventArgs e)
        {

            RefreshControlsTimer.Elapsed -= OnRefreshControls;
            RefreshControlsTimer.Enabled = false;
            RefreshControlsTimer.Stop();
        }

        /// <MetaDataID>{1c5eba76-805f-490f-bfaa-467b49a8fcf8}</MetaDataID>
        void ViewControlObject_Initialized(object sender, EventArgs e)
        {
            //  GetUIElementHandlers(this.Parent as FrameworkElement);
        }

        /// <MetaDataID>{4550e005-2818-4837-b821-6888b1c01a5f}</MetaDataID>
        object _CrossSessionValue;
        /// <MetaDataID>{87b76b0f-3e1f-4053-a5e1-6ef53b1f693c}</MetaDataID>
        public object CrossSessionValue
        {
            get
            {
                var proxy = UIProxy.GetUIProxy(Value);
                if (proxy != null)
                    return new UIProxy(proxy, true).GetTransparentProxy();
                if (Value != null)
                {
                    DisplayedValue displayedValue = FormObjectConnection.GetDisplayedValue(Value);
                    return displayedValue.GetUIProxy(FormObjectConnection, true).GetTransparentProxy();
                }
                return Value;
            }
        }

        /// <MetaDataID>{b83ad592-0645-4af3-8786-020edddffde4}</MetaDataID>
        object _Value;
        /// <MetaDataID>{a39eab8e-f1d1-4dbc-ae08-2bd9ff3d8980}</MetaDataID>
        public object Value
        {
            get
            {
                if (HosttingElement != null && DesignerProperties.GetIsInDesignMode(HosttingElement) && FormObjectConnection != null)
                    FormObjectConnection.State = ViewControlObjectState.DesigneMode;

                if (FormObjectConnection == null || FormObjectConnection.ObjectType == null)
                    return null;
                else
                {

                    if (FormObjectConnection.Instance != null)
                    {
                        if (!WindowLoaded)
                        {
                            FormObjectConnection.HostFormLoad();


                            WindowLoaded = true;
                            if (!RefreshControlsTimer.Enabled)
                            {
                                RefreshControlsTimer.Elapsed += OnRefreshControls;
                                // RefreshControlsTimer.Tick += new EventHandler(OnRefreshControls);
                                RefreshControlsTimer.Interval = (500);
                                RefreshControlsTimer.Enabled = true;
                                RefreshControlsTimer.Start();
                            }

                        }
                        if (_Value == null)
                        {
                            //_Source = UserInterfaceObjectConnection.GetDisplayedValue(UserInterfaceObjectConnection.PresentationObject).GetDynamicUIProxy(UserInterfaceObjectConnection, UserInterfaceObjectConnection.PresentationObjectType.GetExtensionMetaObject<Type>());
                            object value = null;
                            DisplayedValue displayedValue = FormObjectConnection.GetDisplayedValue(FormObjectConnection.PresentationObject);
                            UIProxy uiProxy = displayedValue.GetUIProxy(FormObjectConnection);
                            if (uiProxy != null)

                                value = uiProxy.GetTransparentProxy();
                            else
                                value = displayedValue.Value;

                            _Value = value;
                            //OOAdvantech.IMember member = Activator.CreateInstance(typeof(OOAdvantech.Member<>).MakeGenericType(value.GetType())) as OOAdvantech.IMember;
                            //member.Value = value;
                            //_Source = member;

                        }

                        return _Value;
                    }
                    else
                    {
                        if (_Value == null)
                        {
                            Type type = OOAdvantech.UserInterface.Runtime.UIProxy.GetClassifierType(FormObjectConnection.PresentationObjectType);
                            UIProxy uiProxy = new UIProxy(type);
                            // type = CodeInjection.EmiProxyType(type);
                            _Value = uiProxy.GetTransparentProxy();// Activator.CreateInstance(typeof(OOAdvantech.Member<>).MakeGenericType(type)) as OOAdvantech.IMember;
                        }
                        //object obj = new OOAdvantech.UserInterface.Runtime.UIProxy(type).GetTransparentProxy();
                        return _Value;
                    }
                }
            }
        }


        #endregion
        /// <MetaDataID>{21ea89fa-4349-43dd-a92c-3f3a7f77e4cc}</MetaDataID>
        private bool IsInDesignMode
        {
            get
            {
                if (HosttingElement != null)
                    return DesignerProperties.GetIsInDesignMode(HosttingElement);
                else
                    return true;
            }
        }

        /// <MetaDataID>{24a5191c-b543-46bf-9356-f7e2ef7430a7}</MetaDataID>
        public object Control(object _obj)
        {
            var uiProxy = FormObjectConnection.GetDisplayedValue(_obj).GetUIProxy(FormObjectConnection);
            if (uiProxy != null)
                _obj = uiProxy.GetTransparentProxy();

            return _obj;
        }


        /// <MetaDataID>{e4c6d8a9-1be4-4cc1-bc3e-24c17549b94b}</MetaDataID>
        [Category("Transaction Settings")]
        public bool RollbackOnNegativeAnswer
        {
            get
            {
                return FormObjectConnection.RollbackOnNegativeAnswer;
            }
            set
            {
                FormObjectConnection.RollbackOnNegativeAnswer = value;
            }
        }
        /// <MetaDataID>{65c00da8-2b42-4ff4-963b-96f2fcc56d4a}</MetaDataID>
        [Category("Transaction Settings")]
        public bool RollbackOnExitWithoutAnswer
        {
            get
            {
                return FormObjectConnection.RollbackOnExitWithoutAnswer;
            }
            set
            {
                FormObjectConnection.RollbackOnExitWithoutAnswer = value;
            }
        }



        /// <MetaDataID>{89618be6-36a1-4b72-a761-1f601b8aedb8}</MetaDataID>
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
                if (_AssignPresentationObjectType != value)
                {
                    _Value = null;
                    _AssignPresentationObjectType = value;
                }
                FormObjectConnection.PresentationObjectType = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(value);
                FormObjectConnection.PresentationObjectTypeFullName = value.FullName;

                if (FormObjectConnection.ObjectType != null)
                    FormObjectConnection.PresentationObjectTypeFullName = FormObjectConnection.ObjectType.FullName;

            }
        }


        /// <exclude>Excluded</exclude>
        Type _ViewControlObjectType;

        /// <MetaDataID>{e2619bc6-9daa-4683-9bed-e74a4e2bd1df}</MetaDataID>
        public Type ViewControlObjectType
        {
            get
            {
                return _ViewControlObjectType;// (string)this.GetValue(ViewControlObjectTypeProperty);
            }

            set
            {
                if (_ViewControlObjectType != value)
                {
                    _Value = null;
                    _ViewControlObjectType = value;
                }


                if (FormObjectConnection.ObjectType != null)
                    FormObjectConnection.ViewObjectTypeFullName = FormObjectConnection.ObjectType.FullName;


                if (_ViewControlObjectType != null)
                {

                    FormObjectConnection.ViewObjectTypeFullName = _ViewControlObjectType.FullName;
                    FormObjectConnection.AssemblyMetadata = _ViewControlObjectType.Assembly.FullName;
                }
            }

        }


        /// <MetaDataID>{a365707d-fbc5-498a-b6bd-d7e791be44cf}</MetaDataID>
        [Category("Transaction Settings")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return FormObjectConnection.TransactionOption;
            }
            set
            {
                FormObjectConnection.TransactionOption = value;
            }
        }

        /// <MetaDataID>{222d73c2-b878-4c63-ad1e-451a9da11456}</MetaDataID>
        public bool OpenStateTransitionsInOtherThreads
        {
            get
            {
                if (!FormObjectConnection.TransactionInitialized)
                    return false;
                return FormObjectConnection.Transaction.OpenStateTransitionsInOtherThreads;
            }
        }

        /// <MetaDataID>{6aef92fd-2c3e-4978-a54c-4d510853733e}</MetaDataID>
        public bool InvalidData { get; set; }
    }

}

namespace System.Windows
{
    /// <MetaDataID>{8405d40a-b080-46b1-8908-7cb730d8307e}</MetaDataID>
    public static class DataContextExtraOperators
    {
        /// <MetaDataID>{a37d9b03-1f51-4c4c-aef7-1b39fdcc4e04}</MetaDataID>
        public static object GetDataContextObject(this FrameworkElement frameworkElement)
        {
            object dataContext = frameworkElement.DataContext;
            if (dataContext == null)
                return null;

            if (dataContext is WPFUIElementObjectBind.ObjectContext)
                dataContext = (dataContext as WPFUIElementObjectBind.ObjectContext).Value;

            object realObject = null;
            UIProxy uiProxy = UIProxy.GetUIProxy(dataContext);
            if (uiProxy != null)
                realObject = uiProxy.RealTransparentProxy;
            else
                realObject = dataContext;
            return realObject;
        }

        /// <MetaDataID>{7be3af55-a147-4b72-8246-2e9b4a337b0e}</MetaDataID>
        public static OOAdvantech.UserInterface.Runtime.FormObjectConnection GetObjectContextConnection(this FrameworkElement frameworkElement)
        {
            DependencyObject dependencyObject = frameworkElement;
            while (dependencyObject != null)
            {
                if (dependencyObject is FrameworkElement && (dependencyObject as FrameworkElement).DataContext is WPFUIElementObjectBind.ObjectContext)
                {
                    if (DesignerProperties.GetIsInDesignMode((dependencyObject as FrameworkElement)) && ((dependencyObject as FrameworkElement).DataContext as WPFUIElementObjectBind.ObjectContext).FormObjectConnection != null)
                        ((dependencyObject as FrameworkElement).DataContext as WPFUIElementObjectBind.ObjectContext).FormObjectConnection.State = ViewControlObjectState.DesigneMode;

                    return ((dependencyObject as FrameworkElement).DataContext as WPFUIElementObjectBind.ObjectContext).FormObjectConnection;
                }

                var parent = Media.VisualTreeHelper.GetParent(dependencyObject);
                if (parent == null && dependencyObject is FrameworkElement && (dependencyObject as FrameworkElement).Parent != null)
                    parent = (dependencyObject as FrameworkElement).Parent;
                dependencyObject = parent;
            }
            return null;
        }
        /// <MetaDataID>{76fa09c6-54ed-41f4-a6b4-402f71d048d0}</MetaDataID>
        public static WPFUIElementObjectBind.ObjectContext GetObjectContext(this FrameworkElement frameworkElement)
        {
            DependencyObject dependencyObject = frameworkElement;
            while (dependencyObject != null)
            {
                if (dependencyObject is FrameworkElement && (dependencyObject as FrameworkElement).DataContext is WPFUIElementObjectBind.ObjectContext)
                    return ((dependencyObject as FrameworkElement).DataContext as WPFUIElementObjectBind.ObjectContext);

                var parent = Media.VisualTreeHelper.GetParent(dependencyObject);
                if (parent == null)
                    parent = LogicalTreeHelper.GetParent(dependencyObject);
                dependencyObject = parent;
            }
            return null;
        }

        /// <MetaDataID>{483dd6b4-6bc8-4d8f-aa74-b11746dc71d5}</MetaDataID>
        public static void SetObjectContextInstance<T>(this FrameworkElement frameworkElement, T instance)
        {
            object dataContext = frameworkElement.DataContext;
            if (dataContext is WPFUIElementObjectBind.ObjectContext)
                (dataContext as WPFUIElementObjectBind.ObjectContext).SetContextInstance(instance);
        }
        /// <MetaDataID>{9f9fbb53-143b-4ef8-b227-4f3216ae155a}</MetaDataID>
        public static T GetDataContextObject<T>(this FrameworkElement frameworkElement)
        {
            object realObject = frameworkElement.GetDataContextObject();
            if (realObject is T)
                return (T)realObject;
            else
                return default(T);
        }



    }
}
