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

namespace StyleableWindow
{
    /// <summary>
    /// Interaction logic for PageDialogFrame.xaml
    /// </summary>
    /// <MetaDataID>{7dca2bfa-4306-4dc3-bd50-adb25b5e2ad9}</MetaDataID>
    public partial class PageDialogFrame : UserControl
    {
        static List<PageDialogFrame> _LoadedPageDialogFrames = new List<PageDialogFrame>();
        public static List<PageDialogFrame> LoadedPageDialogFrames
        {
            get
            {
                return _LoadedPageDialogFrames.ToList();
            }
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChange;
        public PageDialogFrame()
        {
            InitializeComponent();
            Frame = new Frame();
            Frame.Name = "PageDialogHost";
            Frame.Background = new SolidColorBrush(Colors.Transparent);
            FrameHost.Children.Add(Frame);

            Frame.Navigated += Frame_Navigated;
            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Loaded += PageDialogFrame_Loaded;
            Unloaded += PageDialogFrame_Unloaded;
        }

        private void PageDialogFrame_Unloaded(object sender, RoutedEventArgs e)
        {
            _LoadedPageDialogFrames.Remove(this);
        }

        private void PageDialogFrame_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadedPageDialogFrames.Add(this);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            while (_Pages.Count == 1 && Frame.CanGoBack)
                Frame.RemoveBackEntry();

            if (OnGoBack)
            {
                if (_Pages.Count > 0 && _Pages.Peek() == Frame.Content as PageDialogViewEmulator)
                {
                    OnGoBack = false;
                    OnGoBackCompleted();
                }
                else
                    System.Diagnostics.Debug.Assert(false, "Navigation page tracking Error");



            }

            if (Frame.Content is PageDialogViewEmulator)
                (Frame.Content as PageDialogViewEmulator).Scale = Scale;

            ObjectChange?.Invoke(this, null);

        }
        PageDialogViewEmulator PeddingPage;
        private void OnGoBackCompleted()
        {
            if (PeddingPage != null)
            {
                _Pages.Push(PeddingPage);
                Frame.Navigate(PeddingPage);
                PeddingPage.NavigationWindow = this;
                PeddingPage = null;
            }
        }

        Frame Frame;

        internal Stack<PageDialogViewEmulator> _Pages = new Stack<PageDialogViewEmulator>();
        public List<PageDialogViewEmulator> Pages
        {
            get
            {
                return _Pages.Reverse().ToList();
            }
        }

        public bool CanGoBack { get => Frame.CanGoBack; }
        public bool CanGoForward { get => Frame.CanGoForward; }
        public double Scale { get; private set; }
        public PageDialogViewEmulator RootPage
        {
            get
            {
                if (Pages.Count > 0)
                    return Pages[0];
                else
                    return null;
            }
        }

        public void ShowDialogPage(PageDialogViewEmulator page)
        {

            ShowDialogPageAfter(null, page);
        }


        public void ShowDialogPageAfter(PageDialogViewEmulator rootPage, PageDialogViewEmulator page)
        {
            if (_Pages.Contains(page))
            {
                System.Diagnostics.Debug.Assert(_Pages.Peek() == page, "Navigation page tracking Error");
                return;
            }
            if (rootPage != null && !_Pages.Contains(rootPage))
            {
                System.Diagnostics.Debug.Assert(_Pages.Contains(rootPage), "Navigation page tracking Error");
                return;
            }

            if (_Pages.Count > 0)
            {
                if (Frame.Content is PageDialogViewEmulator)
                    Scale = (Frame.Content as PageDialogViewEmulator).DialogContentControl.Scale;

                var currentPage = _Pages.Peek();

                if (rootPage != null && currentPage == rootPage)
                {
                    if (OnGoBack)
                        PeddingPage = page;
                    else
                    {
                        _Pages.Push(page);
                        Frame.Navigate(page);
                        page.NavigationWindow = this;
                    }

                }
                else if (currentPage != null)
                {

                    if (currentPage.NewPagesChain(rootPage, page))
                    {

                        //{

                        //    if (OnGoBack)
                        //        PeddingPage = page;
                        //    else
                        //    {
                        //        if (!CanGoBack)
                        //            RemoveLastPage();

                        //        _Pages.Push(page);
                        //        Frame.Navigate(page);
                        //        page.NavigationWindow = this;
                        //    }


                        //}
                        //else
                        ShowDialogPageAfter(rootPage, page);
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            else
            {
                if (OnGoBack)
                    PeddingPage = page;
                else
                {
                    _Pages.Push(page);
                    Frame.Navigate(page);
                    page.NavigationWindow = this;
                }

            }
        }

        public void Navigate(PageDialogViewEmulator page)
        {
            if (Frame.Content is PageDialogViewEmulator)
                Scale = (Frame.Content as PageDialogViewEmulator).DialogContentControl.Scale;
            _Pages.Push(page);
            Frame.Navigate(page);
            page.NavigationWindow = this;

        }

        bool OnGoBack;

        internal void GoBack()
        {
            var currentPage = Frame.Content as PageDialogViewEmulator;
            if (currentPage != null && currentPage.DialogContentControl != null)
            {
                currentPage.Scale = currentPage.DialogContentControl.Scale;
                Scale = currentPage.Scale;
            }
            currentPage = _Pages.Pop();
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                OnGoBack = true;
            }


        }

        internal void RemoveLastPage()
        {
            if (_Pages.Count == 1)
            {
                if (Frame != null)
                    Frame.Navigated -= Frame_Navigated;

                _Pages.Pop();
                OnGoBack = false;
                FrameHost.Children.Remove(Frame);
                Frame = new Frame();
                Frame.Name = "PageDialogHost";
                Frame.Background = new SolidColorBrush(Colors.Transparent);
                Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                FrameHost.Children.Add(Frame);
                Frame.Navigated += Frame_Navigated;
                ObjectChange?.Invoke(this, null);

            }
            else
                throw new NotSupportedException();
        }



        //public static void ShowDialogPage(this System.Windows.Controls.Frame frame, PageDialogViewEmulator page)
        //{
        //    PageDialogViewEmulator.ShowDialogPage(frame.NavigationService, page);
        //}

    }
}
