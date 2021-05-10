using OOAdvantech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace StyleableWindow
{
    /// <MetaDataID>{4f928208-aa13-46a2-aa95-6ec31ecdbf17}</MetaDataID>
    public class PageDialogViewEmulator : System.Windows.Controls.Page, System.ComponentModel.INotifyPropertyChanged, IPage
    {

        public PageDialogFrame NavigationWindow;
        public PageDialogViewEmulator()
        {
            Window.LoadApplicationResources();

            //this.Loaded += PageDialogViewEmulator_Loaded;
            //this.Unloaded += PageDialogViewEmulator_Unloaded;

        }


        public static readonly DependencyProperty LanguageButtonProperty = DependencyProperty.Register(
                                                           "LanguageButton",
                                                           typeof(bool),
                                                           typeof(PageDialogViewEmulator),
                                                           new UIPropertyMetadata(false, new PropertyChangedCallback(OnLanguageButtonChanged)));
        private static void OnLanguageButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            PageDialogViewEmulator page = d as PageDialogViewEmulator;

            if (((bool)e.NewValue))
            {
                if (page.Template != null)
                {


                    UIElement uiElement = page.DialogContentControl.Template.FindName("LanguageButton", page.DialogContentControl) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (page.Template != null)
                {
                    UIElement uiElement = page.DialogContentControl.Template.FindName("LanguageButton", page.DialogContentControl) as UIElement;
                    if (uiElement != null)
                        uiElement.Visibility = Visibility.Hidden;
                }

            }

        }


        [Description("Language button visibility"), Category("Common")]
        public bool LanguageButton
        {
            get { return (bool)GetValue(LanguageButtonProperty); }
            set { SetValue(LanguageButtonProperty, value); }
        }



        #region SelectedCultureProperty

        public static readonly DependencyProperty SelectedCultureProperty = DependencyProperty.Register(
                                                           "SelectedCulture",
                                                           typeof(CultureInfo),
                                                           typeof(PageDialogViewEmulator),
                                                           new UIPropertyMetadata(CultureInfo.InvariantCulture, new PropertyChangedCallback(OnSelectedCultureChanged)));
        private static void OnSelectedCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            PageDialogViewEmulator page = d as PageDialogViewEmulator;
            if (e.NewValue == null || e.NewValue == CultureInfo.InvariantCulture)
                page.SelectedCulture = CultureContext.CurrentCultureInfo;
            else
                page.SelectedCulture = e.NewValue as CultureInfo;

            if (page.SelectedCulture != null)
            {
                CulturePresentation culturePresentation = page.Cultures.Where(x => x.CultureInfo.Name == page.SelectedCulture.Name).FirstOrDefault();
                if (culturePresentation == null)
                    culturePresentation = page.Cultures.Where(x => x.CultureInfo.Parent != null && page.SelectedCulture.Parent != null && x.CultureInfo.Parent.Name == page.SelectedCulture.Parent.Name).FirstOrDefault();
                if (culturePresentation != null)
                    page.SelectedCulturePresentation = culturePresentation;
            }

        }


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

        CulturePresentation _SelectedCulturePresentation;
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


        static List<CulturePresentation> _Cultures;
        public List<CulturePresentation> Cultures
        {
            get
            {
                if (_Cultures == null)
                    _Cultures = CulturePresentation.Cultures;

                return _Cultures;
            }
        }



        public static string LanguageLabel
        {
            get
            {
                return Properties.Resources.LanguageLabel;
            }
        }

        bool _UseDefaultCultureWhenValueMissing;
        private bool ClearNavigationWindow;
        private bool CloseWindowOnBackAndDontSave;

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




        #region TitleBarComments property
        public static readonly DependencyProperty TitleBarCommentsProperty = DependencyProperty.Register(
                                                           "TitleBarComments",
                                                           typeof(string),
                                                           typeof(PageDialogViewEmulator),
                                                           new UIPropertyMetadata("", new PropertyChangedCallback(OnTitleBarCommentsChanged)));
        private static void OnTitleBarCommentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
     
        }

        [Description("Title bar Comments"), Category("Common")]
        public String TitleBarComments
        {
            get { return GetValue(TitleBarCommentsProperty) as string; }
            set { SetValue(TitleBarCommentsProperty, value); }
        }
        #endregion




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


        //public static void ShowDialogPage(System.Windows.Navigation.NavigationService navigationService, PageDialogViewEmulator pageDialogViewEmulator)
        //{
        //    if (PageTracking.ContainsKey(navigationService)&& PageTracking[navigationService].Count>0)
        //    {
        //        PageTracking[navigationService].Peek().NewPagesChain(pageDialogViewEmulator);
        //    }
        //    else
        //        navigationService.Navigate(pageDialogViewEmulator);
        //}

        //internal void NewPagesChain(PageDialogViewEmulator pageDialogViewEmulator)
        //{
        //    var dialogContentControl = WPFUIElementObjectBind.ObjectContext.FindChilds<DialogContentControl>(this).FirstOrDefault();
        //    if (dialogContentControl != null)
        //    {
        //        if (dialogContentControl.NewPagesChain(pageDialogViewEmulator))
        //        {
        //            if (PageTracking[_NavigationService].Peek() == this)
        //            {
        //                PageTracking[_NavigationService].Pop();
        //                _NavigationService.Navigate(pageDialogViewEmulator);

        //            }
        //            else
        //                PageDialogViewEmulator.ShowDialogPage(NavigationService, pageDialogViewEmulator);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    else
        //    {

        //    }
        //}

        //static Dictionary<System.Windows.Navigation.NavigationService, Stack<PageDialogViewEmulator>> PageTracking = new Dictionary<System.Windows.Navigation.NavigationService, Stack<PageDialogViewEmulator>>();
        //  static Dictionary<System.Windows.Navigation.NavigationService, PageDialogViewEmulator> NewDialog = new Dictionary<System.Windows.Navigation.NavigationService, PageDialogViewEmulator>();

        //private void PageDialogViewEmulator_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (_NavigationService != null)
        //        _NavigationService.Navigating -= NavigationService_Navigating;
        //}

        //internal System.Windows.Navigation.NavigationService _NavigationService;

        public List<IPage> NavigationServicePages
        {
            get
            {
                return NavigationWindow._Pages.OfType<IPage>().ToList();
            }
        }

        public string PagesPath
        {
            get
            {
                string pagesPath = "";
                foreach (var page in NavigationWindow._Pages.OfType<System.Windows.Controls.Page>().Reverse().ToList())
                {
                    if (!string.IsNullOrWhiteSpace(pagesPath) && !string.IsNullOrWhiteSpace(page.Title))
                        pagesPath += " / ";
                    pagesPath += page.Title;
                }
                return pagesPath;
            }
        }

        //private void PageDialogViewEmulator_Loaded(object sender, System.Windows.RoutedEventArgs e)
        //{

        //    _NavigationService = this.NavigationService;
        //    _NavigationService.Navigating += NavigationService_Navigating;


        //    //if(NavigationWindow.CanGoBack&&pageTrackingStack.Count==1)
        //    //    _NavigationService.RemoveBackEntry();
        //}

        internal void InitLanguageButton()
        {

            Button languageButton = DialogContentControl.Template.FindName("LanguageButton", DialogContentControl) as Button;
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
            Grid languageGrid = DialogContentControl.Template.FindName("LanguageLabel", DialogContentControl) as Grid;
            if (languageGrid != null)
            {
                if (LanguageButton && languageGrid != null)
                    languageGrid.DataContext = this;
            }
            System.Windows.Controls.Primitives.Popup languagePopup = DialogContentControl.Template.FindName("LanguagePopup", DialogContentControl) as System.Windows.Controls.Primitives.Popup;
            if (languagePopup != null)
            {
                if (LanguageButton)
                    languagePopup.DataContext = this;
            }
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.Popup languagePopup = DialogContentControl.Template.FindName("LanguagePopup", DialogContentControl) as System.Windows.Controls.Primitives.Popup;
            //System.Windows.Controls.Primitives.Popup languagePopup = Template.FindName("LanguagePopup", this) as System.Windows.Controls.Primitives.Popup;
            if (languagePopup != null)
            {
                System.Windows.Shapes.Line titleBarLine = DialogContentControl.Template.FindName("TitleBarLine", DialogContentControl) as System.Windows.Shapes.Line;
                System.Windows.Controls.Button languageButton = DialogContentControl.Template.FindName("LanguageButton", DialogContentControl) as System.Windows.Controls.Button;
                languagePopup.VerticalOffset = titleBarLine.TranslatePoint(new Point(0, 0), this).Y - languageButton.TranslatePoint(new Point(0, 0), this).Y;
                languagePopup.VerticalOffset += 5;
                languagePopup.HorizontalOffset = languageButton.ActualWidth;
                //var w = languagePopup.Width;

                languagePopup.IsOpen = true;
            }


        }


        private void NavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            //{
            //    if (PageTracking[_NavigationService].Peek() == this)
            //    {
            //        PageTracking[_NavigationService].Pop();
            //        _NavigationService.Navigating -= NavigationService_Navigating;
            //    }
            //}
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (DataContext is WPFUIElementObjectBind.ObjectContext)
                (DataContext as WPFUIElementObjectBind.ObjectContext).Initialize(this);

            Loaded += PageDialogViewEmulator_Loaded;



        }

        private void PageDialogViewEmulator_Loaded(object sendera, RoutedEventArgs e)
        {
            DialogContentControl.Scale = Scale;
            if (DialogContentControl.CloseClickCommand == null)
            {
                DialogContentControl.CloseClickCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
                {
                    var objectContext = this.GetObjectContext();
                    if (objectContext.RollbackOnNegativeAnswer == false || !objectContext.HasChanges(true))
                    {
                        var objectContextConnection = this.GetObjectContextConnection();
                        if (objectContextConnection != null)
                            (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                        NavigationWindow.RemoveLastPage();
                    }
                    else
                    {
                        ClearNavigationWindow = true;
                        DialogContentControl.IsSavePopUpOpen = true;
                    }
                });
            }
            if (DialogContentControl.BackClickCommand == null)
            {
                DialogContentControl.BackClickCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
                {
                    ClosePage();
                });
            }

            if (DialogContentControl.BackAndSaveCommand == null)
            {
                DialogContentControl.BackAndSaveCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
                {
                    if (ClearNavigationWindow)
                    {
                        DialogContentControl.BeforeTransactionCommit?.Execute(sender);
                        var objectContextConnection = this.GetObjectContextConnection();
                        if (objectContextConnection != null)
                            (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.OK);

                        NavigationWindow.RemoveLastPage();

                    }
                    else
                    {
                        var newPage = NewPage;
                        if (NavigationWindow.CanGoBack)
                        {
                            NavigationWindow.GoBack();
                            var objectContextConnection = this.GetObjectContextConnection();
                            if (objectContextConnection != null)
                                (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.OK);
                        }
                        else if (newPage != null)
                        {
                            var objectContextConnection = this.GetObjectContextConnection();
                            if (objectContextConnection != null)
                                (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.OK);

                        }

                        if (newPage != null)
                            NavigationWindow.ShowDialogPageAfter(RootPage, newPage);
                    }



                });
            }


            DialogContentControl.BackAndDontSaveCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var objectContextConnection = this.GetObjectContextConnection();

                if (ClearNavigationWindow)
                {
                    if (objectContextConnection != null)
                        (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                    NavigationWindow.RemoveLastPage();
                }
                else
                {

                    if (NavigationWindow.CanGoBack)
                    {
                        NavigationWindow.GoBack();
                        if (objectContextConnection != null)
                            (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                        if (NewPage != null)
                            NavigationWindow.ShowDialogPageAfter(RootPage, NewPage);
                    }
                    else
                    {
                        if (NewPage != null)
                        {
                            if (objectContextConnection != null)
                                (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                            NavigationWindow.ShowDialogPageAfter(RootPage, NewPage);
                        }
                        else
                        {
                            //if(PageDialogViewEmulator.ShowDialogPage())
                            var objectContext = this.GetObjectContext();
                            if (CloseWindowOnBackAndDontSave)
                            {
                                objectContextConnection.Transaction.Abort();
                                Window.GetWindow(this).Close();
                            }
                            else
                            {

                            }
                        }
                    }
                }
            });
            DialogContentControl.CloseSavePopupCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                DialogContentControl.IsSavePopUpOpen = false;
                NewPage = null;

            });



        }
        private void ClosePage()
        {
            var objectContext = this.GetObjectContext();
            if (NavigationWindow.CanGoBack)
            {
                if (objectContext.RollbackOnExitWithoutAnswer == false || !objectContext.HasChanges(true))
                {
                    NavigationWindow.GoBack();
                    var objectContextConnection = this.GetObjectContextConnection();
                    if (objectContextConnection != null)
                        (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                }
                else
                    DialogContentControl.IsSavePopUpOpen = true;
            }
        }

        DialogContentControl _DialogContentControl;
        internal DialogContentControl DialogContentControl
        {
            get
            {
                if (_DialogContentControl == null)
                    _DialogContentControl = WPFUIElementObjectBind.ObjectContext.FindChilds<DialogContentControl>(this).FirstOrDefault();
                return _DialogContentControl;
            }
        }
        public double Scale { get; internal set; }

        internal PageDialogViewEmulator RootPage;
        internal PageDialogViewEmulator NewPage;
        internal bool NewPagesChain(PageDialogViewEmulator rootPage, PageDialogViewEmulator newPage)
        {

            //(rootPage!=null& NewPage == newPage&& NavigationWindow._Pages.Peek()==rootPage)

            if (NewPage == newPage && !NavigationWindow.CanGoBack)
            {
                NavigationWindow.RemoveLastPage();
                return true;
            }

            RootPage = rootPage;
            NewPage = newPage;
            var objectContext = this.GetObjectContext();
            if (objectContext.RollbackOnExitWithoutAnswer == false || !objectContext.HasChanges(true))
            {
                if (NavigationWindow.CanGoBack)
                    NavigationWindow.GoBack();
                else
                    NavigationWindow.RemoveLastPage();
                var objectContextConnection = this.GetObjectContextConnection();
                if (objectContextConnection != null)
                    (objectContextConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(OOAdvantech.UserInterface.Runtime.DialogResult.Cancel);
                return true;
            }
            else
            {
                DialogContentControl.IsSavePopUpOpen = true;
                return false;
            }
        }


        public bool Closing(CancelEventArgs eventArg)
        {

            var objectContext = this.GetObjectContext();
            if (this.GetObjectContextConnection() != null && this.GetObjectContextConnection().State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Passive)
                return true;

            if (objectContext.RollbackOnExitWithoutAnswer && objectContext.HasChanges(true))
            {
                eventArg.Cancel = true;
                if (DialogContentControl.IsSavePopUpOpen)
                    DialogContentControl.IsSavePopUpOpen = false;
                DialogContentControl.IsSavePopUpOpen = true;

                PageDialogViewEmulator page = WPFUIElementObjectBind.ObjectContext.FindParent<PageDialogViewEmulator>(this);

                if (NavigationWindow._Pages.ToList().IndexOf(this) == 0)
                    CloseWindowOnBackAndDontSave = true;

                return false;
            }
            else
                return true;


        }


        public void Close()
        {
            if (DialogContentControl != null)
                ClosePage();

        }


        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <MetaDataID>{490d773d-dcb2-4ca7-8e76-159c0d955960}</MetaDataID>
    public static class PageExtensions
    {

    }
}
