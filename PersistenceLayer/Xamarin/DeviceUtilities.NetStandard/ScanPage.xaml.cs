using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DeviceUtilities.NetStandard
{
    /// <MetaDataID>{0b60add7-aaf9-4efd-9f4f-59612b1ab929}</MetaDataID>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public delegate void ScanResultDelegate(ZXing.Result result);
        public event ScanResultDelegate OnScan;
        public ScanPage()
        {
            InitializeComponent();
            ToggleTorchButton.Clicked += ToggleTorchButton_Clicked;
            SizeChanged += ScanPage_SizeChanged;
            

            ZxingView.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    ZxingView.IsAnalyzing = false;

                    OnScan?.Invoke(result);
                });
            };

         }
        double OrgToggleTorchButtonHeight;

        string _HeaderText;
        public string HeaderText
        {
            get
            {
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        string _FooterText;
        public string FooterText
        {
            get
            {
                return _FooterText;
            }
            set
            {
                _FooterText = value;
                OnPropertyChanged(nameof(FooterText));
            }
        }

        public bool? UseFrontCameraIfAvailable { get=> ZxingView.Options.UseFrontCameraIfAvailable; set=> ZxingView.Options.UseFrontCameraIfAvailable=value; }

        private void ScanPage_SizeChanged(object sender, EventArgs e)
        {
            if (OrgToggleTorchButtonHeight == 0)
                OrgToggleTorchButtonHeight = ToggleTorchButton.Height;

            ToggleTorchButton.HeightRequest = OrgToggleTorchButtonHeight * 1.5;
        }

        private void ToggleTorchButton_Clicked(object sender, EventArgs e)
        {
            ZxingView.IsTorchOn = !ZxingView.IsTorchOn;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ZxingView.IsScanning = true;
        }
        protected override void OnDisappearing()
        {
            ZxingView.IsScanning = false;
            base.OnDisappearing();
        }
    }

    /// <MetaDataID>{470a2c41-4704-45fd-8517-1061f01c66be}</MetaDataID>
    public class ScanCode
    {

        /// <MetaDataID>{acf45ed3-0a82-44b2-8a79-37c323a2290c}</MetaDataID>
        public ScanCode()
        {
            ScanPage.OnScan += async (result) =>
            {
                if (OnScan)
                {
                    TaskCompletionSource<ZXing.Result> connectToServicePointTask = null;
                    lock (this)
                    {
                        connectToServicePointTask = ConnectToServicePointTask;
                        ConnectToServicePointTask = null;
                        OnScan = false;
                    }


                    // Navigate away
                    try
                    {
                        await ScanPage.Navigation.PopAsync();
                        connectToServicePointTask.SetResult(result);
                    }
                    catch (Exception error)
                    {
                        connectToServicePointTask.SetException(error);
                    }
                }
            };
            ScanPage.Disappearing += (object sender, EventArgs e) =>
            {
                OnScan = false;
                if (ConnectToServicePointTask != null)
                    ConnectToServicePointTask.SetResult(null);
            };
        }
        /// <MetaDataID>{48fd0ee3-3425-43a2-9089-f8b1dff67786}</MetaDataID>
        ScanPage ScanPage = new ScanPage();
        /// <MetaDataID>{7d21c2b2-2a87-4746-b6a4-51329478c307}</MetaDataID>
        TaskCompletionSource<ZXing.Result> ConnectToServicePointTask;
        /// <MetaDataID>{2fc3761f-3d5e-488f-9a73-51765746965f}</MetaDataID>
        bool OnScan = false;
        /// <MetaDataID>{28d86519-8c24-4502-afb4-f851ff15a209}</MetaDataID>
        public Task<ZXing.Result> Scan(string headerText, string footerText, bool? useFrontCameraIfAvailable = null)
        {
            if (!(Xamarin.Forms.Application.Current.MainPage is NavigationPage))
            {
                throw new Exception("MainPage must be NavigationPage");
            }
            lock (this)
            {

                if (OnScan && ConnectToServicePointTask != null)
                    return ConnectToServicePointTask.Task;

                OnScan = true;
                ScanPage.HeaderText = headerText;
                ScanPage.FooterText = footerText;
                ScanPage.UseFrontCameraIfAvailable = useFrontCameraIfAvailable;
                ConnectToServicePointTask = new TaskCompletionSource<ZXing.Result>();
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            });


            return ConnectToServicePointTask.Task;


        }
    }
}