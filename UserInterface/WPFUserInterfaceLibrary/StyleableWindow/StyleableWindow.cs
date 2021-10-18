using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech;
using WinInterop = System.Windows.Interop;
using System.Windows.Input;


namespace StyleableWindow
{
    /// <MetaDataID>{765665a6-3e84-4cdd-ac89-cfd3fd44e656}</MetaDataID>
    [TemplatePart(Name = "MinimizeBox", Type = typeof(UIElement))]
    [TemplatePart(Name = "MaximizeBox", Type = typeof(UIElement))]
    [TemplatePart(Name = "SubTitle", Type = typeof(System.Windows.Controls.Label))]
    [TemplatePart(Name = "ShadowBorder", Type = typeof(System.Windows.Controls.Border))]
    [TemplatePart(Name = "LanguageButton", Type = typeof(System.Windows.Controls.Button))]

    public class Window : System.Windows.Window, INotifyPropertyChanged
    {

        /// <MetaDataID>{a51a242d-da6d-4505-8a12-9ea076d881ad}</MetaDataID>
        public virtual void OnBack()
        {


        }
        /// <MetaDataID>{6e5930c3-c0f2-487d-9eef-e1ab59fd293d}</MetaDataID>
        static Window()
        {

            LoadApplicationResources();
        }

        /// <MetaDataID>{abc22560-8979-47d5-92b9-e1f381a1f30a}</MetaDataID>
        static private bool ApplicationResourcesLoaded = false;
        /// <MetaDataID>{8c65fb33-eb96-48d7-8550-59270520fc1a}</MetaDataID>
        static object ApplicationResourcesLock = new object();
        /// <MetaDataID>{f2a86050-21bf-4756-9ef7-16d4dce0ac15}</MetaDataID>
        public static void LoadApplicationResources()
        {
            lock (ApplicationResourcesLock)
            {
                if (!ApplicationResourcesLoaded)
                {
                    string uriString = string.Format(@"/{0};component/CustomWindowStyle.xaml", typeof(Window).Assembly.GetName().Name);
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(uriString, UriKind.Relative) });
                    ApplicationResourcesLoaded = true;
                }
            }
        }

        /// <MetaDataID>{8e694c8a-ec5b-437c-ba75-0108acfa2a9c}</MetaDataID>
        public Window()
        {
            Style = Application.Current.Resources["CustomWindowStyle"] as Style;
            Loaded += StyleableWindow_Loaded;
            SourceInitialized += new EventHandler(win_SourceInitialized);
            StateChanged += Window_StateChanged;
            MouseDown += Window_MouseDown;

            SelectedCulture = OOAdvantech.CultureContext.CurrentCultureInfo;
            UseDefaultCultureWhenValueMissing = CultureContext.UseDefaultCultureValue;
        }

        /// <MetaDataID>{de0e998c-0dac-4335-a8f2-5f74ab32bba8}</MetaDataID>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            var focusedControl = FocusManager.GetFocusedElement(this) as DependencyObject;
            if (focusedControl is FrameworkElement)
            {
                FrameworkElement parent = (focusedControl as FrameworkElement).Parent as FrameworkElement;
                while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                {
                    parent = (FrameworkElement)parent.Parent;
                }

                if (parent != null)
                {
                    DependencyObject scope = FocusManager.GetFocusScope(focusedControl);
                    FocusManager.SetFocusedElement(scope, parent as IInputElement);
                }
            }
            //focusedControl.
            System.Windows.Input.Keyboard.ClearFocus();
            this.Focus();
        }


        /// <MetaDataID>{bf6eacdb-ef1a-4562-baf3-cb55bb53df64}</MetaDataID>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (DataContext is WPFUIElementObjectBind.ObjectContext)
                (DataContext as WPFUIElementObjectBind.ObjectContext).Initialize(this);
        }

        /// <MetaDataID>{3abf0809-1f38-46dd-bda6-d118f3a8c5e9}</MetaDataID>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    {
                        if (Template != null)
                        {
                            System.Windows.Controls.Border border = Template.FindName("ShadowBorder", this) as System.Windows.Controls.Border;
                            if (border != null)
                            {
                                border.BorderThickness = new Thickness(0);
                                border.CornerRadius = new CornerRadius(0);
                            }
                        }
                        break;
                    }
                case WindowState.Minimized:
                    // Do your stuff
                    break;
                case WindowState.Normal:
                    {
                        if (Template != null)
                        {
                            System.Windows.Controls.Border border = Template.FindName("ShadowBorder", this) as System.Windows.Controls.Border;
                            if (border != null)
                            {
                                border.BorderThickness = new Thickness(10);
                                border.CornerRadius = new CornerRadius(10);
                            }
                        }

                        break;
                    }
            }

        }

        /// <MetaDataID>{9770f2b3-36d4-43e9-881f-b09a3350f6ea}</MetaDataID>
        double OrgHeight;
        /// <MetaDataID>{2a450c41-1bb9-4784-ad34-b7270b507e60}</MetaDataID>
        private void StyleableWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Template != null)
            {
                OrgHeight = Height;
                UIElement uiElement = Template.FindName("MaximizeBox", this) as UIElement;
                if (uiElement != null)
                {
                    if (MaximizeBox)
                        uiElement.Visibility = Visibility.Visible;
                    else
                        uiElement.Visibility = Visibility.Collapsed;
                }

                uiElement = Template.FindName("MinimizeBox", this) as UIElement;
                if (uiElement != null)
                {
                    if (MinimizeBox)
                        uiElement.Visibility = Visibility.Visible;
                    else
                        uiElement.Visibility = Visibility.Collapsed;
                }

                System.Windows.Controls.Button languageButton = Template.FindName("LanguageButton", this) as System.Windows.Controls.Button;
                if (languageButton != null)
                {
                    if (LanguageButton)
                    {
                        languageButton.Visibility = Visibility.Visible;
                        languageButton.DataContext = this;
                    }
                    else
                        languageButton.Visibility = Visibility.Hidden;

                    languageButton.Click += LanguageButton_Click;
                }
                Grid languageGrid = Template.FindName("LanguageLabel", this) as Grid;
                if (languageGrid != null)
                {
                    if (LanguageButton && languageGrid != null)
                        languageGrid.DataContext = this;

                }


                System.Windows.Controls.Primitives.Popup languagePopup = Template.FindName("LanguagePopup", this) as System.Windows.Controls.Primitives.Popup;
                if (languagePopup != null)
                {
                    if (LanguageButton)
                        languagePopup.DataContext = this;
                }




                System.Windows.Controls.Label label = Template.FindName("SubTitle", this) as System.Windows.Controls.Label;

                System.Windows.Controls.Grid windowArea = Template.FindName("WindowArea", this) as System.Windows.Controls.Grid;

                System.Windows.Controls.Button backButton = Template.FindName("BackButton", this) as System.Windows.Controls.Button;

                Point windowMargin = windowArea.TranslatePoint(new Point(0, 0), this);



                if (backButton != null)
                {
                    backButton.Visibility = BackButtonVisibility;
                    backButton.Click += BackButton_Click;
                }

                if (label != null)
                {
                    if (string.IsNullOrWhiteSpace(SubTitle))
                    {
                        label.Visibility = Visibility.Collapsed;
                        Height -= 30;
                    }
                    else
                    {
                        label.Visibility = Visibility.Visible;
                        Height += 30;
                    }
                    label.Content = SubTitle;
                }

                UpdateLayout();
                System.Windows.Controls.Grid titleBarArea = Template.FindName("TitleBarArea", this) as System.Windows.Controls.Grid;
                if (titleBarArea != null)
                {
                    System.Windows.Controls.Border border = Template.FindName("ShadowBorder", this) as System.Windows.Controls.Border;
                    double borderThickness = windowMargin.X;
                    if (border != null)
                        borderThickness += border.BorderThickness.Top;

                    double titleBarAreaActualHeight = titleBarArea.ActualHeight;
                    double windowCaptionHeight = SystemParameters.WindowCaptionHeight;
                    Height = OrgHeight + (titleBarAreaActualHeight + borderThickness - SystemParameters.WindowCaptionHeight) + 2;
                }
            }

        }

        /// <MetaDataID>{ccf1c21c-8ec5-48f7-a433-87d738662aae}</MetaDataID>
        public string LanguageLabel
        {
            get
            {
                return Properties.Resources.LanguageLabel;
            }
        }

        /// <MetaDataID>{55fb122d-5f98-4cb2-abe8-70d231de9bd4}</MetaDataID>
        public string DefaultLanguageLabel
        {
            get
            {

                return Properties.Resources.DefaultLanguageLabel;
            }
        }



        /// <MetaDataID>{3f3a6a72-29d7-4ddc-a2a3-19b31fb8d512}</MetaDataID>
        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Primitives.Popup languagePopup = Template.FindName("LanguagePopup", this) as System.Windows.Controls.Primitives.Popup;
            if (languagePopup != null)
            {
                System.Windows.Shapes.Line titleBarLine = Template.FindName("TitleBarLine", this) as System.Windows.Shapes.Line;
                System.Windows.Controls.Button languageButton = Template.FindName("LanguageButton", this) as System.Windows.Controls.Button;
                languagePopup.VerticalOffset = titleBarLine.TranslatePoint(new Point(0, 0), this).Y - languageButton.TranslatePoint(new Point(0, 0), this).Y;
                languagePopup.VerticalOffset += 5;
                languagePopup.HorizontalOffset = languageButton.ActualWidth;
                //var w = languagePopup.Width;


                languagePopup.IsOpen = true;
            }


        }

        /// <MetaDataID>{8ff54bde-3276-4b7a-b6f5-674ac6441bc5}</MetaDataID>
        public static readonly DependencyProperty LanguagePopupProperty =
                 DependencyProperty.Register("LanguagePopup", typeof(System.Windows.Controls.ContentControl), typeof(StyleableWindow.Window), new UIPropertyMetadata(null));

        /// <MetaDataID>{c897642f-09c3-487e-bddd-02fd7fec201a}</MetaDataID>
        public System.Windows.Controls.ContentControl LanguagePopup
        {
            get { return (System.Windows.Controls.ContentControl)GetValue(LanguagePopupProperty); }
            set { SetValue(LanguagePopupProperty, value); }
        }




        /// <MetaDataID>{98b1cfb5-8d0d-4c97-8642-0d66fef67fec}</MetaDataID>
        static List<CulturePresentation> _Cultures;
        /// <MetaDataID>{07f4330b-273c-4d7a-b85c-f516c59796fa}</MetaDataID>
        public List<CulturePresentation> Cultures
        {
            get
            {
                if (_Cultures == null)
                    _Cultures = CulturePresentation.Cultures;

                return _Cultures;
            }
        }

        /// <MetaDataID>{d1cc55d5-ac51-4e0d-9a84-070a763296b5}</MetaDataID>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnBack();
            }
            catch (Exception error)
            {
            }

        }

        /// <MetaDataID>{1468dd6a-5267-4f70-9d1f-bffc766c8201}</MetaDataID>
        public static readonly DependencyProperty MinimizeBoxProperty = DependencyProperty.Register(
                                                                   "MinimizeBox",
                                                                   typeof(bool),
                                                                   typeof(Window),
                                                                   new UIPropertyMetadata(true, new PropertyChangedCallback(OnMinimizeBoxChanged)));
        /// <MetaDataID>{8267fcd9-57d5-4950-958b-9df0e289e61c}</MetaDataID>
        private static void OnMinimizeBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Window window = d as System.Windows.Window;
            if (((bool)e.NewValue))
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("MinimizeBox", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("MinimizeBox", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Collapsed;
                }

            }
        }
        /// <MetaDataID>{10718029-855a-4c94-b886-af6683b1c775}</MetaDataID>
        [Description("Minimize button is visible"), Category("Common")]
        public bool MinimizeBox
        {
            get { return (bool)GetValue(MinimizeBoxProperty); }
            set { SetValue(MinimizeBoxProperty, value); }
        }


        /// <MetaDataID>{ab3e86b4-fbe6-4328-bb16-9e44361e6185}</MetaDataID>
        public static readonly DependencyProperty MaximizeBoxProperty = DependencyProperty.Register(
                                                                   "MaximizeBox",
                                                                   typeof(bool),
                                                                   typeof(Window),
                                                                   new UIPropertyMetadata(true, new PropertyChangedCallback(OnMaximizeBoxChanged)));
        /// <MetaDataID>{04cf1cd5-ceb8-47f4-9bff-55c1192232b4}</MetaDataID>
        private static void OnMaximizeBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Window window = d as System.Windows.Window;
            if (((bool)e.NewValue))
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("MaximizeBox", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("MaximizeBox", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Collapsed;
                }
            }
        }
        /// <MetaDataID>{47fbbbde-2527-4a26-9b45-2426b9fe6f98}</MetaDataID>
        [Description("Maximize button is visible"), Category("Common")]
        public bool MaximizeBox
        {
            get { return (bool)GetValue(MaximizeBoxProperty); }
            set { SetValue(MaximizeBoxProperty, value); }
        }




        /// <MetaDataID>{b2fabaa6-4c1a-4fca-9907-80e8b444b770}</MetaDataID>
        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
                                                                   "SubTitle",
                                                                   typeof(string),
                                                                   typeof(Window),
                                                                   new UIPropertyMetadata("", new PropertyChangedCallback(OnSubTitleChanged)));
        /// <MetaDataID>{15550bc4-d149-47cc-baa0-4d1a7569f566}</MetaDataID>
        private static void OnSubTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StyleableWindow.Window window = d as StyleableWindow.Window;
            if (window.Template != null)
            {
                System.Windows.Controls.Label label = window.Template.FindName("SubTitle", window) as System.Windows.Controls.Label;
                if (label != null)
                {
                    if (string.IsNullOrWhiteSpace(e.NewValue as string))
                    {
                        label.Visibility = Visibility.Collapsed;
                        window.Height -= 30;
                    }
                    else
                    {
                        label.Visibility = Visibility.Visible;
                        window.Height += 30;
                    }
                    label.Content = e.NewValue as string;

                    window.UpdateLayout();
                    System.Windows.Controls.Grid titleBarArea = window.Template.FindName("TitleBarArea", window) as System.Windows.Controls.Grid;
                    if (titleBarArea != null)
                    {
                        double titleBarAreaActualHeight = titleBarArea.ActualHeight;
                        double windowCaptionHeight = SystemParameters.WindowCaptionHeight;
                        window.Height = window.OrgHeight + (titleBarAreaActualHeight - SystemParameters.WindowCaptionHeight);
                    }


                }
            }
        }

        /// <MetaDataID>{660dd7b7-843f-4963-9f0f-037e941d734e}</MetaDataID>
        [Description("Title bar sub title"), Category("Common")]
        public String SubTitle
        {
            get { return GetValue(SubTitleProperty) as string; }
            set { SetValue(SubTitleProperty, value); }
        }




        /// <MetaDataID>{a87b0723-a9ca-4b2b-8203-4f97434bbb34}</MetaDataID>
        public static readonly DependencyProperty LanguageButtonProperty = DependencyProperty.Register(
                                                                   "LanguageButton",
                                                                   typeof(bool),
                                                                   typeof(Window),
                                                                   new UIPropertyMetadata(false, new PropertyChangedCallback(OnLanguageButtonChanged)));
        /// <MetaDataID>{d164080c-e943-431e-b097-e616c1382cb5}</MetaDataID>
        private static void OnLanguageButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            System.Windows.Window window = d as System.Windows.Window;
            if (((bool)e.NewValue))
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("LanguageButton", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (window.Template != null)
                {
                    UIElement uiElement = window.Template.FindName("LanguageButton", window) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Hidden;
                }

            }

        }


        /// <MetaDataID>{9dcd61bc-85b3-49ff-a6fb-2c6b64c526bf}</MetaDataID>
        [Description("Language button visibility"), Category("Common")]
        public bool LanguageButton
        {
            get { return (bool)GetValue(LanguageButtonProperty); }
            set { SetValue(LanguageButtonProperty, value); }
        }



        #region SelectedCultureProperty

        /// <MetaDataID>{230478c0-0833-4b3e-9702-df274058c4ae}</MetaDataID>
        public static readonly DependencyProperty SelectedCultureProperty = DependencyProperty.Register(
                                                                   "SelectedCulture",
                                                                   typeof(CultureInfo),
                                                                   typeof(Window),
                                                                   new UIPropertyMetadata(CultureInfo.InvariantCulture, new PropertyChangedCallback(OnSelectedCultureChanged)));
        /// <MetaDataID>{041052fe-2995-4760-9b48-6c14cb501873}</MetaDataID>
        private static void OnSelectedCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            Window window = d as Window;
            if (e.NewValue == null || e.NewValue == CultureInfo.InvariantCulture)
                window.SelectedCulture = CultureContext.CurrentCultureInfo;
            else
                window.SelectedCulture = e.NewValue as CultureInfo;

            if (window.SelectedCulture != null)
            {
                CulturePresentation culturePresentation = window.Cultures.Where(x => x.CultureInfo.Name == window.SelectedCulture.Name).FirstOrDefault();
                if (culturePresentation == null)
                    culturePresentation = window.Cultures.Where(x => x.CultureInfo.Parent != null && window.SelectedCulture.Parent != null && x.CultureInfo.Parent.Name == window.SelectedCulture.Parent.Name).FirstOrDefault();

                if (culturePresentation != null)
                    window.SelectedCulturePresentation = culturePresentation;
            }

        }


        /// <MetaDataID>{16c9734e-552d-47a0-82dc-be48537b0450}</MetaDataID>
        [Description("Selected Culture"), Category("Common")]
        public CultureInfo SelectedCulture
        {
            get { return GetValue(SelectedCultureProperty) as CultureInfo; }
            set
            {
                SetValue(SelectedCultureProperty, value);
            }
        }

        #endregion

        /// <MetaDataID>{432e70c0-cf01-43b1-acd6-0029145904e3}</MetaDataID>
        CulturePresentation _SelectedCulturePresentation;
        /// <MetaDataID>{9ba3efa3-da51-414e-9c83-075232342920}</MetaDataID>
        public CulturePresentation SelectedCulturePresentation
        {
            get
            {
                if (_SelectedCulturePresentation == null)
                {
                    _SelectedCulturePresentation = Cultures.Where(x => x.CultureInfo.Name == OOAdvantech.CultureContext.CurrentCultureInfo.Name).FirstOrDefault();
                    if (_SelectedCulturePresentation == null)
                        _SelectedCulturePresentation = Cultures.Where(x => x.CultureInfo.Name == OOAdvantech.CultureContext.CurrentCultureInfo.Parent.Name).FirstOrDefault();

                    if (_SelectedCulturePresentation != null)
                    {
                        SelectedCulture = _SelectedCulturePresentation.CultureInfo;
                        this.GetObjectContextConnection().Culture = SelectedCulture;
                        this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing = OOAdvantech.CultureContext.UseDefaultCultureValue;
                        UseDefaultCultureWhenValueMissing = OOAdvantech.CultureContext.UseDefaultCultureValue;
                    }
                    else
                    {
                        this.GetObjectContextConnection().Culture = null;
                    }
                }
                return _SelectedCulturePresentation;
            }
            set
            {

                _SelectedCulturePresentation = value;
                if (value != null)
                {
                    SelectedCulture = value.CultureInfo;
                    if (this.GetObjectContextConnection() != null)
                    {
                        if (this.GetObjectContextConnection().Culture != SelectedCulture)
                        {
                            this.GetObjectContextConnection().Culture = SelectedCulture;
                            RefreshUIElements();
                        }
                    }
                }
                else
                {
                    if (this.GetObjectContextConnection() != null)
                        this.GetObjectContextConnection().Culture = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCulturePresentation)));
            }
        }

        /// <MetaDataID>{bd52b37f-52ea-4cfc-a79a-7ea3c419d633}</MetaDataID>
        private void RefreshUIElements()
        {
            if (this.GetObjectContextConnection() != null)
            {
                using (CultureContext cultureContext = new CultureContext(this.GetObjectContextConnection().Culture, this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing))
                {

                    foreach (var uiProxy in this.GetObjectContextConnection().GetAllUIProxies())
                    {
                        if (uiProxy.DisplayedValue != null)
                        {
                            this.GetObjectContextConnection().ObjectChangeState(uiProxy.DisplayedValue.Value, null);
                        }
                    }
                }

                //using (CultureContext cultureContext = new CultureContext(this.GetObjectContextConnection().Culture, this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing))
                //{
                //    var uIProxies = WPFUIElementObjectBind.ViewControlObject.FindVisualChildren<FrameworkElement>(this).Where(x => OOAdvantech.UserInterface.Runtime.UIProxy.GetUIProxy(x.DataContext) != null).Select(x => OOAdvantech.UserInterface.Runtime.UIProxy.GetUIProxy(x.DataContext).DisplayedValue).Where(x => x != null).Distinct().ToList();
                //    foreach (var displayedValue in uIProxies)
                //    {
                //        this.GetObjectContextConnection().ObjectChangeState(displayedValue.Value, null);
                //    }
                //}
            }
        }

        /// <MetaDataID>{3f237142-1712-4fd4-b00e-bb6cca477333}</MetaDataID>
        bool _UseDefaultCultureWhenValueMissing;

        /// <MetaDataID>{26e58560-84de-42c3-892b-bc59fc55d5b2}</MetaDataID>
        public bool UseDefaultCultureWhenValueMissing
        {
            get
            {
                return _UseDefaultCultureWhenValueMissing;
            }
            set
            {
                if (_UseDefaultCultureWhenValueMissing != value)
                {
                    _UseDefaultCultureWhenValueMissing = value;
                    if (this.GetObjectContextConnection() != null)
                    {
                        this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing = value;
                        if (this.GetObjectContextConnection().Culture != null)
                            RefreshUIElements();
                    }
                }
            }
        }



























        /// <MetaDataID>{77c7ff9b-edbe-4282-bf08-366acfd63e60}</MetaDataID>
        public static readonly DependencyProperty BackButtonVisibilityProperty = DependencyProperty.Register(
                                                                "BackButtonVisibility",
                                                                typeof(Visibility),
                                                                typeof(Window),
                                                                new UIPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(OnBackButtonVisibilityChanged)));

        public event PropertyChangedEventHandler PropertyChanged;


        /// <MetaDataID>{6c682d55-3a9b-4bb0-b9ae-311128942f1a}</MetaDataID>
        protected void RunPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        /// <MetaDataID>{bc09ccf4-281b-40e7-9722-9c08c11445ba}</MetaDataID>
        private static void OnBackButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StyleableWindow.Window window = d as StyleableWindow.Window;
            if (window.Template != null)
            {
                System.Windows.Controls.Button button = window.Template.FindName("BackButton", window) as System.Windows.Controls.Button;
                if (button != null)
                {
                    button.Visibility = (Visibility)e.NewValue;
                    window.UpdateLayout();
                }
            }
        }

        /// <MetaDataID>{94ed4a2e-e089-4062-9618-1d1ed1c10555}</MetaDataID>
        [Description("Back button visibility"), Category("Common")]
        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }


        /// <MetaDataID>{7880cbd7-6771-49c6-8346-5a98ecb0baca}</MetaDataID>
        void win_SourceInitialized(object sender, EventArgs e)
        {
            System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));

        }



        //public override void OnApplyTemplate()
        //{
        //    System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
        //    WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
        //}

        /// <MetaDataID>{cd74119a-c373-44c4-a193-0c4b237bb70b}</MetaDataID>
        private static System.IntPtr WindowProc(
                      System.IntPtr hwnd,
                      int msg,
                      System.IntPtr wParam,
                      System.IntPtr lParam,
                      ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (System.IntPtr)0;
        }

        /// <MetaDataID>{7abe5e77-c312-4e66-a75a-43cc7b16bea3}</MetaDataID>
        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {

                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }


        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };




        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }


        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }
            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }


        }

        /// <MetaDataID>{960af2e6-2f41-4af6-8779-87664bd02002}</MetaDataID>
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        /// <summary></summary>
        /// <MetaDataID>{eba560fa-d378-4873-8329-e3dfd41cd570}</MetaDataID>
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
    }

    /// <MetaDataID>{6129a16d-a2be-422a-b7d3-f3eb33b775dc}</MetaDataID>
    public class CulturePresentation
    {
        /// <MetaDataID>{4b155aed-8054-4fd8-be73-faa13244cf81}</MetaDataID>
        public CultureInfo CultureInfo { get; set; }

        /// <MetaDataID>{53b54e6e-0652-48c8-a29c-63157d4f2ba5}</MetaDataID>
        static List<CulturePresentation> _Cultures;
        /// <MetaDataID>{2fac67ed-595e-41ac-b1c3-5bc776eca70d}</MetaDataID>
        public static List<CulturePresentation> Cultures
        {
            get
            {

                List<string> langs = new List<string>() { "zh-CN", "es-ES", "en-GB", "en-US", "en-AU", "en-CA", "hi-IN", "bn-BD","pt-PT","pt-BR","fr-CA","ru-RU","ja-JP","pa-IN","pa-Arab-PK",
                                                        "mr-IN","te-IN","tr-TR","ko-KR","fr-BE","fr-FR","de-AT","de-DE","vi-VN","ta-IN","ta-LK","ur-IN","jv-Latn-ID","it-IT","fa-IR"};
                if (_Cultures == null)
                {
                    var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(x => new CulturePresentation(x)).ToList();
                    var naturals = CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(x => !string.IsNullOrEmpty(x.Name)).ToDictionary(x => x.Name);
                    _Cultures = new List<CulturePresentation>();
                    for (int i = 0; i < cultures.Count; i++)
                    {
                        if (cultures[i].CultureInfo.Name != null && cultures[i].CultureInfo.Name.Length > 1 && cultures[i].CultureInfo.Name.IndexOf("es") == 0)
                        {

                        }
                        if (cultures[i].CultureInfo.Name != null && cultures[i].CultureInfo.Name.Length > 1 && cultures[i].CultureInfo.Name.IndexOf("ee") == 0)
                        {

                        }
                        if (langs.Contains(cultures[i].CultureInfo.Name))
                        {
                            if (!_Cultures.Contains(cultures[i]))
                            {
                                _Cultures.Add(cultures[i]);
                                System.Diagnostics.Debug.WriteLine("Natural : " + cultures[i].CultureInfo.Parent.EnglishName + ", " + cultures[i].CultureInfo.Parent.Name);
                                System.Diagnostics.Debug.WriteLine(cultures[i].CultureInfo.EnglishName + ", " + cultures[i].CultureInfo.Name);
                            }
                        }
                        else if (naturals.ContainsKey(cultures[i].CultureInfo.Name))
                        {
                            naturals.Remove(cultures[i].CultureInfo.Name);
                            if (langs.Where(x => x.IndexOf(cultures[i].CultureInfo.Name + "-") == 0).FirstOrDefault() != null)
                                continue;

                            if (cultures[i].CultureInfo.Name != null && cultures[i].CultureInfo.Name.Length > 1 && cultures[i].CultureInfo.Name.IndexOf("es") == 0)
                            {

                            }
                            if (cultures[i].CultureInfo.Name != null && cultures[i].CultureInfo.Name.Length > 1 && cultures[i].CultureInfo.Name.IndexOf("en") == 0)
                            {

                            }

                            int k = 1;
                            while (i + k < cultures.Count)
                            {
                                var flagCulture = cultures[i + k];
                                if (flagCulture.CultureInfo.Name != null && flagCulture.CultureInfo.Name.Length > 1 && flagCulture.CultureInfo.Name.IndexOf(cultures[i].CultureInfo.Name) != 0)
                                    break;
                                if (flagCulture.FlagImageUri != CulturePresentation.Blank)
                                {

                                    if (!_Cultures.Contains(cultures[i + k]))
                                    {
                                        _Cultures.Add(cultures[i + k]);
                                        System.Diagnostics.Debug.WriteLine("Natural : " + cultures[i].CultureInfo.EnglishName + ", " + cultures[i].CultureInfo.Name);
                                        System.Diagnostics.Debug.WriteLine(cultures[i + k].CultureInfo.EnglishName + ", " + cultures[i + k].CultureInfo.Name);
                                    }
                                    break;
                                }
                                else
                                    k++;
                            }

                            //if (i + 1 < cultures.Count)
                            //{
                            //    _Cultures.Add(cultures[i + 1]);
                            //    System.Diagnostics.Debug.WriteLine("Natural : " + cultures[i].CultureInfo.EnglishName + ", " + cultures[i].CultureInfo.Name);
                            //    System.Diagnostics.Debug.WriteLine(cultures[i + 1].CultureInfo.EnglishName + ", " + cultures[i + 1].CultureInfo.Name);
                            //}
                        }
                    }
                }

                return _Cultures;
            }
        }



        /// <MetaDataID>{76e0c3f9-bd0a-40a9-a2e5-9d3f9e484c28}</MetaDataID>
        static List<string> ResourceNames = typeof(CulturePresentation).Assembly.GetManifestResourceNames().ToList();

        /// <MetaDataID>{ac8bd457-9003-4fa3-945e-4639af5118bd}</MetaDataID>
        static Dictionary<string, BitmapImage> FlagsImages = new Dictionary<string, BitmapImage>();
        /// <MetaDataID>{97676d22-dda7-4bda-8532-047785739d97}</MetaDataID>
        public CulturePresentation(CultureInfo cultureInfo)
        {

            //if (flag == null)
            //{
            //    string resourceName = "StyleableWindow.Resources.Images.mono_flags.SK24.png";
            //    BitmapImage image = null;
            //    image = GetResourceImage(resourceName);
            //}
            this.CultureInfo = cultureInfo;
            _FlagImage = Blank;

            if (cultureInfo.Name.LastIndexOf('-') != -1)
            {

                string region = cultureInfo.Name.Substring(cultureInfo.Name.LastIndexOf('-') + 1);

                if (!FlagsImages.TryGetValue(cultureInfo.Name, out _FlagImage))
                {
                    string resourceName = string.Format("StyleableWindow.Resources.Images.mono_flags.{0}24.png", region);
                    if (ResourceNames.Where(x => x.ToLower() == resourceName.ToLower()).FirstOrDefault() != null)
                    {
                        _FlagImage = GetResourceImage(resourceName);
                        uri = resourceName;
                        FlagsImages[cultureInfo.Name] = _FlagImage;
                    }
                    else
                        _FlagImage = Blank;
                }
            }


        }

        /// <MetaDataID>{e3f44a92-7759-4ac5-bc58-d514b897a290}</MetaDataID>
        public string uri = "StyleableWindow.Resources.Images.mono_flags.Blank24.png";


        /// <MetaDataID>{a674d5c8-ee06-4553-88ac-679f0ab56e27}</MetaDataID>
        public static string LanguageLabel
        {
            get
            {
                return Properties.Resources.LanguageLabel;
            }
        }

        /// <MetaDataID>{5e434c36-f54a-4844-87c2-dd88fc68b8c1}</MetaDataID>
        public static string DefaultLanguageLabel
        {
            get
            {

                return Properties.Resources.DefaultLanguageLabel;
            }
        }
        /// <MetaDataID>{1bfab2f4-aa00-4ea0-873f-ef3264844db1}</MetaDataID>
        private static BitmapImage GetResourceImage(string resourceName)
        {
            BitmapImage image;
            using (var stream = typeof(CulturePresentation).Assembly.GetManifestResourceStream(resourceName))
            {
                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                image = bitmap;
            }

            return image;
        }

        /// <MetaDataID>{02bc1b82-a4cf-4507-83e9-12e0d8aaa0ba}</MetaDataID>
        public static BitmapImage Blank = GetResourceImage("StyleableWindow.Resources.Images.mono_flags.Blank24.png");

        /// <MetaDataID>{b53397ea-dca9-4f06-978a-49e4eae145eb}</MetaDataID>
        public static BitmapImage flag;//= new BitmapImage(typeof(CulturePresentation).Assembly.GetManifestResourceStream("StyleableWindow.Resources.Images.mono_flags.SK24.png"))
        /// <MetaDataID>{d2c9c210-2699-4f2c-bf39-8ef2d937ef7c}</MetaDataID>
        BitmapImage _FlagImage;
        /// <MetaDataID>{55b1c947-ab42-4c98-9532-814435a0acd0}</MetaDataID>
        public ImageSource FlagImageUri
        {
            get
            {

                return _FlagImage;// new Uri( "/MenuItemsEditor;component/Image/mono-flags/sk.png");
            }
        }

    }
}
