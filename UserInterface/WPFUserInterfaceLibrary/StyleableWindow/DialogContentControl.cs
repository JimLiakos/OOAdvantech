using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StyleableWindow
{
    /// <MetaDataID>{a835fe73-9322-47d9-b604-db15a22f506e}</MetaDataID>
    public class DialogContentControl : System.Windows.Controls.ContentControl, System.ComponentModel.INotifyPropertyChanged
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public DialogContentControl()
        {

            Window.LoadApplicationResources();

            PagePadding = new Thickness(20);
            BorderMargin = new Thickness(10);
            CornerRadius = new CornerRadius(10);

            SaveClickCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                BeforeTransactionCommit?.Execute(sender);
                var objectContext = this.GetObjectContext();
                if (objectContext != null)
                    objectContext.Save();
            });

            OKClickCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                BeforeTransactionCommit?.Execute(sender);
                var objectContext = this.GetObjectContext();
                if (objectContext != null)
                    objectContext.OnOKCommand.CallExecute(sender);


            }, (object sender) => PreventTransactionCommit);



            CancelClickCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var objectContext = this.GetObjectContext();
                if (objectContext != null)
                    objectContext.OnCancelCommand.CallExecute(sender);

            });






            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dispatcherTimer.Start();

            Loaded += DialogContentControl_Loaded;
            Unloaded += DialogContentControl_Unloaded;

        }




        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        double _Scale = 0;
        public double Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                if (_Scale > 1)
                    _Scale = 1;
                if (_Scale < 0)
                    _Scale = 0;

                if (HostArea != null)
                {
                    var scale = _Scale;
                    if (double.IsInfinity(InitialHeight) || double.IsInfinity(InitialWidth))
                        scale = 1;

                    var hostArea = HostArea;
                    if (hostArea.ActualHeight - InitialHeight > 0 && (hostArea.ActualWidth - InitialWidth) > 0)
                    {
                        if (hostArea.ActualHeight - InitialHeight < (hostArea.ActualWidth - InitialWidth))
                        {
                            var dif = hostArea.ActualHeight - InitialHeight;
                            ContentArea.MaxHeight = InitialHeight + dif * scale;
                            ContentArea.MaxWidth = InitialWidth + dif * scale;
                        }
                        else
                        {
                            var dif = hostArea.ActualWidth - InitialWidth;
                            ContentArea.MaxHeight = InitialHeight + dif * scale;
                            ContentArea.MaxWidth += InitialWidth + dif * scale;
                        }
                    }

                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Scale)));
            }
        }
        public Visibility PageTitleBarVisibility
        {
            get
            {
                if (Icon != null || !string.IsNullOrWhiteSpace(this.Title))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }



        public Visibility BorderVisibility
        {
            get
            {
                if (Flat)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public Visibility ScalingControlVisibility
        {
            get
            {
                if (Flat)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }


        private void RunPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var ancestors = WPFUIElementObjectBind.ObjectContext.GetAncestors(this);
                if (ancestors.Where(x => x.GetType().FullName == "Microsoft.VisualStudio.DesignTools.WpfDesigner.InstanceBuilders.PageInstance").FirstOrDefault() != null)
                {
                    if (!double.IsInfinity(InitialHeight) && !double.IsInfinity(InitialWidth))
                    {
                        if (ContentArea != null)
                        {
                            ContentArea.MaxHeight = this.InitialHeight;
                            ContentArea.MaxWidth = this.InitialWidth;
                        }
                    }
                }
            }
            else
            {
                PageDialogViewEmulator pageDialogViewEmulator = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (pageDialogViewEmulator != null)
                {
                    var scale = _Scale;
                    if (double.IsInfinity(InitialHeight) || double.IsInfinity(InitialWidth))
                        scale = 1;

                    if (ContentArea != null)
                    {
                        var hostArea = HostArea;

                        if (hostArea.ActualHeight - InitialHeight > 0 && (hostArea.ActualWidth - InitialWidth) > 0)
                        {
                            if (hostArea.ActualHeight - InitialHeight < (hostArea.ActualWidth - InitialWidth))
                            {
                                var dif = hostArea.ActualHeight - InitialHeight;
                                ContentArea.MaxHeight = InitialHeight + dif * scale;
                                ContentArea.MaxWidth = InitialWidth + dif * scale;
                            }
                            else
                            {
                                var dif = hostArea.ActualWidth - InitialWidth;
                                ContentArea.MaxHeight = InitialHeight + dif * scale;
                                ContentArea.MaxWidth += InitialWidth + dif * scale;
                            }
                        }
                        else
                        {
                            ContentArea.MaxHeight = this.InitialHeight;
                            ContentArea.MaxWidth = this.InitialWidth;
                        }
                        hostArea.SizeChanged += HostArea_SizeChanged;
                    }

                    pageDialogViewEmulator.InitLanguageButton();
                }
            }


        }

        private void HostArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scale = _Scale;
            if (double.IsInfinity(InitialHeight) || double.IsInfinity(InitialWidth))
                scale = 1;

            var hostArea = HostArea;
            if (hostArea.ActualHeight - InitialHeight > 0 && (hostArea.ActualWidth - InitialWidth) > 0)
            {
                if (hostArea.ActualHeight - InitialHeight < (hostArea.ActualWidth - InitialWidth))
                {
                    var dif = hostArea.ActualHeight - InitialHeight;
                    ContentArea.MaxHeight = InitialHeight + dif * scale;
                    ContentArea.MaxWidth = InitialWidth + dif * scale;
                }
                else
                {
                    var dif = hostArea.ActualWidth - InitialWidth;
                    ContentArea.MaxHeight = InitialHeight + dif * scale;
                    ContentArea.MaxWidth += InitialWidth + dif * scale;
                }
            }
        }

        public string PagesPath
        {
            get
            {
                PageDialogViewEmulator pageDialogViewEmulator = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (pageDialogViewEmulator != null)
                    return pageDialogViewEmulator.PagesPath;
                else
                    return "";

            }
        }

        public string CurrentPageComments
        {
            get
            {
                PageDialogViewEmulator pageDialogViewEmulator = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (pageDialogViewEmulator != null)
                    return pageDialogViewEmulator.TitleBarComments;
                else
                    return "";

            }
        }

        System.Windows.Window HostWindow;

        private void DialogContentControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // HostWindow.Closing -= HostWindow_Closing;
        }

        private void DialogContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var ancestors = WPFUIElementObjectBind.ObjectContext.GetAncestors(this);
                if (ancestors.Where(x => x.GetType().FullName == "Microsoft.VisualStudio.DesignTools.WpfDesigner.InstanceBuilders.PageInstance").FirstOrDefault() != null)
                {
                    Style = Application.Current.Resources["PageDialog"] as Style;
                }
                else
                    Style = Application.Current.Resources["ModalDialog"] as Style;
            }
            else
            {

                PageDialogViewEmulator pageDialogViewEmulator = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (pageDialogViewEmulator != null)
                {
                    Style = Application.Current.Resources["PageDialog"] as Style;
                    HostWindow = System.Windows.Window.GetWindow(this);


                }
                else
                    Style = Application.Current.Resources["ModalDialog"] as Style;
            }

        }




        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            OKClickCommand.Refresh();

        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            System.Diagnostics.Debug.WriteLine("Delta " + e.Delta);
            if (e.Delta > 0)
                Scale += 0.1;
            else
                Scale -= 0.1;

        }



        DialogButtons _Buttons = DialogButtons.OKCancel;

        public Visibility OkButtonVisibility
        {
            get
            {
                if (_Buttons == DialogButtons.OK || _Buttons == DialogButtons.OKCancel)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility BackButtonVisibility
        {
            get
            {
                PageDialogViewEmulator page = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (page != null && page.NavigationWindow != null && page.NavigationWindow.CanGoBack)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public Visibility CloseButtonVisibility
        {
            get
            {
                PageDialogViewEmulator page = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                if (page == null)
                    return Visibility.Collapsed;
                if (page.NavigationWindow == null || page.NavigationWindow.CanGoBack)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }


        public Visibility CancelButtonVisibility
        {
            get
            {
                if (_Buttons == DialogButtons.OKCancel)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility SaveButtonVisibility
        {
            get
            {
                if (_Buttons == DialogButtons.OnlyBack)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }


        bool _IsSavePopUpOpen;

        public bool IsSavePopUpOpen
        {
            get => _IsSavePopUpOpen;
            set
            {
                System.Diagnostics.Debug.WriteLine("IsSavePopUpOpen " + value.ToString());
                _IsSavePopUpOpen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSavePopUpOpen)));
                if (!_IsSavePopUpOpen)
                {
                    PageDialogViewEmulator page = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);
                    
                }

            }
        }

        //static DialogContentControl()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContentControl), new FrameworkPropertyMetadata(typeof(DialogContentControl)));
        //}


        WPFUIElementObjectBind.RelayCommand _BackAndSaveCommand;
        public WPFUIElementObjectBind.RelayCommand BackAndSaveCommand
        {
            get => _BackAndSaveCommand;
            internal set
            {
                _BackAndSaveCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackAndSaveCommand)));
            }
        }

        WPFUIElementObjectBind.RelayCommand _BackAndDontSaveCommand;
        public WPFUIElementObjectBind.RelayCommand BackAndDontSaveCommand
        {
            get => _BackAndDontSaveCommand;
            internal set
            {
                _BackAndDontSaveCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackAndDontSaveCommand)));
            }
        }
        public WPFUIElementObjectBind.RelayCommand SaveAndBackCommand { get; protected set; }
        public Thickness _PagePadding;
        public Thickness PagePadding
        {
            get => _PagePadding;
            set
            {
                _PagePadding = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PagePadding)));
            }
        }

        public CornerRadius _CornerRadius;
        public CornerRadius CornerRadius
        {
            get => _CornerRadius;
            set
            {
                _CornerRadius = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CornerRadius)));
            }
        }



        public Thickness _BorderMargin;
        public Thickness BorderMargin
        {
            get => _BorderMargin;
            set
            {
                _BorderMargin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
            }
        }
        public WPFUIElementObjectBind.RelayCommand SaveClickCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand OKClickCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand CancelClickCommand { get; protected set; }


        WPFUIElementObjectBind.RelayCommand _BackClickCommand;
        public WPFUIElementObjectBind.RelayCommand BackClickCommand
        {
            get => _BackClickCommand;
            internal set
            {
                _BackClickCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackClickCommand)));
            }
        }

        WPFUIElementObjectBind.RelayCommand _CloseSavePopupCommand;
        public WPFUIElementObjectBind.RelayCommand CloseSavePopupCommand
        {
            get => _CloseSavePopupCommand;
            internal set
            {
                _CloseSavePopupCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CloseSavePopupCommand)));
            }
        }


        WPFUIElementObjectBind.RelayCommand _CloseClickCommand;
        public WPFUIElementObjectBind.RelayCommand CloseClickCommand
        {
            get => _CloseClickCommand;
            internal set
            {
                _CloseClickCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CloseClickCommand)));
            }
        }


        // Using a DependencyProperty as the backing store for Footer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(System.Windows.Controls.ContentControl), typeof(DialogContentControl), new UIPropertyMetadata(null));




        #region InitialHeight property

        public static readonly DependencyProperty InitialHeightProperty = DependencyProperty.Register(
                                                                    "InitialHeight",
                                                                    typeof(double),
                                                                    typeof(DialogContentControl),
                                                                    new PropertyMetadata((double)300, new PropertyChangedCallback(OnInitialHeightChanged)));
        public double InitialHeight { get { var value = GetValue(InitialHeightProperty); return value is double ? (double)value : default(double); } set => SetValue(InitialHeightProperty, value); }

        private static void OnInitialHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DialogContentControl).Scale = (d as DialogContentControl).Scale;
        }

        #endregion


        #region InitialWidth property

        public static readonly DependencyProperty InitialWidthProperty = DependencyProperty.Register(
                                                                    "InitialWidth",
                                                                    typeof(double),
                                                                    typeof(DialogContentControl),
                                                                    new PropertyMetadata((double)500, new PropertyChangedCallback(OnInitialWidthChanged)));
        public double InitialWidth { get { var value = GetValue(InitialWidthProperty); return value is double ? (double)value : default(double); } set => SetValue(InitialWidthProperty, value); }

        private static void OnInitialWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DialogContentControl).Scale = (d as DialogContentControl).Scale;
        }

        #endregion


        #region Title property
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
                                                           "Title",
                                                           typeof(string),
                                                           typeof(DialogContentControl),
                                                           new UIPropertyMetadata("", new PropertyChangedCallback(OnTitleChanged)));
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //StyleableWindow.Window window = d as StyleableWindow.Window;
            //if (window.Template != null)
            //{
            //    System.Windows.Controls.Label label = window.Template.FindName("Title", window) as System.Windows.Controls.Label;
            //    if (label != null)
            //    {
            //        if (string.IsNullOrWhiteSpace(e.NewValue as string))
            //        {
            //            label.Visibility = Visibility.Collapsed;
            //            window.Height -= 30;
            //        }
            //        else
            //        {
            //            label.Visibility = Visibility.Visible;
            //            window.Height += 30;
            //        }
            //        label.Content = e.NewValue as string;

            //        window.UpdateLayout();
            //        System.Windows.Controls.Grid titleBarArea = window.Template.FindName("TitleBarArea", window) as System.Windows.Controls.Grid;
            //        if (titleBarArea != null)
            //        {
            //            double titleBarAreaActualHeight = titleBarArea.ActualHeight;
            //            double windowCaptionHeight = SystemParameters.WindowCaptionHeight;
            //            window.Height = window.OrgHeight + (titleBarAreaActualHeight - SystemParameters.WindowCaptionHeight);
            //        }
            //    }
            //}
        }

        [Description("Title bar title"), Category("Common")]
        public String Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region SubTitle property
        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
                                                           "SubTitle",
                                                           typeof(string),
                                                           typeof(DialogContentControl),
                                                           new UIPropertyMetadata("", new PropertyChangedCallback(OnSubTitleChanged)));
        private static void OnSubTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //StyleableWindow.Window window = d as StyleableWindow.Window;
            //if (window.Template != null)
            //{
            //    System.Windows.Controls.Label label = window.Template.FindName("SubTitle", window) as System.Windows.Controls.Label;
            //    if (label != null)
            //    {
            //        if (string.IsNullOrWhiteSpace(e.NewValue as string))
            //        {
            //            label.Visibility = Visibility.Collapsed;
            //            window.Height -= 30;
            //        }
            //        else
            //        {
            //            label.Visibility = Visibility.Visible;
            //            window.Height += 30;
            //        }
            //        label.Content = e.NewValue as string;

            //        window.UpdateLayout();
            //        System.Windows.Controls.Grid titleBarArea = window.Template.FindName("TitleBarArea", window) as System.Windows.Controls.Grid;
            //        if (titleBarArea != null)
            //        {
            //            double titleBarAreaActualHeight = titleBarArea.ActualHeight;
            //            double windowCaptionHeight = SystemParameters.WindowCaptionHeight;
            //            window.Height = window.OrgHeight + (titleBarAreaActualHeight - SystemParameters.WindowCaptionHeight);
            //        }
            //    }
            //}
        }

        [Description("Title bar sub title"), Category("Common")]
        public String SubTitle
        {
            get { return GetValue(SubTitleProperty) as string; }
            set { SetValue(SubTitleProperty, value); }
        }
        #endregion

        #region Icon property
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
                                                         "Icon",
                                                         typeof(ImageSource),
                                                         typeof(DialogContentControl),
                                                         new UIPropertyMetadata(default(ImageSource), new PropertyChangedCallback(OnIconChanged)));

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public ImageSource Icon
        {
            get { return GetValue(IconProperty) as ImageSource; }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        #region Flat property

        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register(
                                                                    "Flat",
                                                                    typeof(bool),
                                                                    typeof(DialogContentControl),
                                                                    new PropertyMetadata(false, new PropertyChangedCallback(OnFlatChanged)));
        public bool Flat { get { var value = GetValue(FlatProperty); return value is bool ? (bool)value : default(bool); } set => SetValue(FlatProperty, value); }

        private static void OnFlatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as DialogContentControl).BorderVisibility = (d as DialogContentControl).BorderVisibility;
            if ((d as DialogContentControl).Flat)
                (d as DialogContentControl).PagePadding = new Thickness(0);
            else
                (d as DialogContentControl).PagePadding = new Thickness(20);

            if ((d as DialogContentControl).Flat)
            {
                (d as DialogContentControl).BorderMargin = new Thickness(0);
                (d as DialogContentControl).CornerRadius = new CornerRadius(0);
            }
            else
            {
                (d as DialogContentControl).BorderMargin = new Thickness(10);
                (d as DialogContentControl).CornerRadius = new CornerRadius(10);
            }

            (d as DialogContentControl).RunPropertyChanged(d, new PropertyChangedEventArgs(nameof(DialogContentControl.BorderVisibility)));
            (d as DialogContentControl).RunPropertyChanged(d, new PropertyChangedEventArgs(nameof(DialogContentControl.Padding)));

        }

        #endregion



        #region Before Save command property

        public static readonly DependencyProperty BeforeTransactionCommitProperty = DependencyProperty.Register(
                                                                    "BeforeTransactionCommit",
                                                                    typeof(ICommand),
                                                                    typeof(DialogContentControl),
                                                                    new PropertyMetadata(null, new PropertyChangedCallback(OnBeforeTransactionCommitChanged)));
        public ICommand BeforeTransactionCommit { get { return GetValue(BeforeTransactionCommitProperty) as ICommand; } set => SetValue(BeforeTransactionCommitProperty, value); }

        private static void OnBeforeTransactionCommitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion



        public System.Windows.Controls.ContentControl Footer
        {
            get { return (System.Windows.Controls.ContentControl)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public System.Windows.Controls.Grid HostArea
        {
            get
            {
                if (this.Template != null)
                    return this.Template.FindName("HostArea", this) as Grid;
                else
                    return null;
            }

        }
        public System.Windows.Controls.Grid ContentArea
        {
            get
            {
                if (this.Template != null)
                    return this.Template.FindName("ContentArea", this) as Grid;
                else
                    return null;

            }
        }

        public System.Windows.Controls.Button OkButton
        {
            get
            {

                return (System.Windows.Controls.Button)this.FindName("OkButton");
            }
        }
        public bool PreventTransactionCommit
        {
            get
            {

                object value = GetValue(PreventTransactionCommitProperty);
                if (value is bool)
                {
                    if ((bool)value)
                    {
                        if (this.GetObjectContext() == null)
                            return false;

                        //System.Diagnostics.Debug.WriteLine(string.Format("PreventTransactionCommit {0}", !this.GetObjectContext().OpenStateTransitionsInOtherThreads));
                        return !this.GetObjectContext().OpenStateTransitionsInOtherThreads;
                    }
                    //System.Diagnostics.Debug.WriteLine(string.Format("PreventTransactionCommit {0}", (bool)value));
                    return (bool)value;
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("PreventTransactionCommit {0}", true));
                    return true;
                }
            }
            set
            {

                SetValue(PreventTransactionCommitProperty, value);
            }
        }
        public static readonly DependencyProperty PreventTransactionCommitProperty =
                    DependencyProperty.Register(
                    "PreventTransactionCommit",
                    typeof(bool),
                    typeof(DialogContentControl),
                    new PropertyMetadata(true, new PropertyChangedCallback(PreventTransactionCommitPropertyChangedCallback)));

        public event PropertyChangedEventHandler PropertyChanged;

        public static void PreventTransactionCommitPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is DialogContentControl)
                (d as DialogContentControl).PreventTransactionCommitPropertyChanged();
        }
        private void PreventTransactionCommitPropertyChanged()
        {
            OKClickCommand.Refresh();
        }


        public DialogButtons Buttons
        {
            get
            {
                object value = GetValue(ButtonsProperty);
                if (value is DialogButtons)
                {
                    return (DialogButtons)value;
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("PreventTransactionCommit {0}", true));
                    return DialogButtons.OKCancel;
                }
            }
            set
            {

                SetValue(ButtonsProperty, value);
            }
        }
        public static readonly DependencyProperty ButtonsProperty =
                    DependencyProperty.Register(
                    "DialogButtons",
                    typeof(DialogButtons),
                    typeof(DialogContentControl),
                    new PropertyMetadata(DialogButtons.OKCancel, new PropertyChangedCallback(ButtonsPropertyChangedCallback)));



        public static void ButtonsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is DialogContentControl)
                (d as DialogContentControl).ButtonsPropertyChanged();
        }
        private void ButtonsPropertyChanged()
        {
            _Buttons = Buttons;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkButtonVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CancelButtonVisibility)));
        }


        public enum DialogButtons
        {
            //
            // Summary:
            //     The message box displays an OK button.
            OK = 0,
            //
            // Summary:
            //     The message box displays OK and Cancel buttons.
            OKCancel = 1,

            OnlyBack = 3
            ////
            //// Summary:
            ////     The message box displays Yes, No, and Cancel buttons.
            //YesNoCancel = 3,
            ////
            //// Summary:
            ////     The message box displays Yes and No buttons.
            //YesNo = 4
        }

    }
}
