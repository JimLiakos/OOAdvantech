using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Java.Security;
using Xamarin.Essentials;
using static Android.Content.PM.PackageManager;

using Task = System.Threading.Tasks.Task;

namespace OOAdvantech.Droid
{
    /// <MetaDataID>{3e7a0d67-12af-4707-9a0d-5ccd2a013a7a}</MetaDataID>
    public class DeviceOOAdvantechCore : IDeviceOOAdvantechCore 
    {


        public static IBackgroundService ForegroundServiceManager;

        static event MessageReceivedHandle internalMessageReceived;

        event MessageReceivedHandle IDeviceOOAdvantechCore.MessageReceived
        {
            add
            {
                internalMessageReceived += value;

            }

            remove
            {
                internalMessageReceived -= value;

            }
        }

        public static void MessageReceived(IRemoteMessage message)
        {
            internalMessageReceived?.Invoke(message);



        }

        static event KeyboardChangeStateHandle internalKeyboardChangeState;
        

        event KeyboardChangeStateHandle IDeviceOOAdvantechCore.KeyboardChangeState
        {
            add
            {
                internalKeyboardChangeState += value;

            }

            remove
            {
                internalKeyboardChangeState -= value;

            }
        }

        public static void KeyboordChangeState(KeybordStatus keybordStatus)
        {
            internalKeyboardChangeState?.Invoke(keybordStatus);
        }
        public static void SetFirebaseToken(string firebaseToken)
        {
            _FirebaseToken = firebaseToken;
        }





        public static void BackPressed(BackPressedArgs eventArgs)
        {
            OOAdvantech.DeviceApplication.Current.OnBackPressed(eventArgs);
            HybridWebViewRenderer.BackPressed();
        }


        static string _FirebaseToken;

        public string FirebaseToken
        {
            get
            {

                return _FirebaseToken;
            }
        }


        public void PlaySound()
        {
            var notification = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var r = RingtoneManager.GetRingtone(Android.App.Application.Context, notification);
            r.Play();
        }
        static bool _IsinSleepMode;
        public bool IsinSleepMode { get => _IsinSleepMode; }


        static event EventHandler _ApplicationResuming;

        public event EventHandler ApplicationResuming
        {
            add
            {
                _ApplicationResuming += value;
            }
            remove
            {
                _ApplicationResuming -= value;
            }
        }
        static event EventHandler _ApplicationSleeping;
        public event EventHandler ApplicationSleeping
        {
            add
            {
                _ApplicationSleeping += value;
            }
            remove
            {
                _ApplicationSleeping -= value;
            }

        }


        public void OnResume()
        {

            if (_IsinSleepMode)
            {
                _IsinSleepMode = false;
                _ApplicationResuming?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnSleep()
        {
            if (!_IsinSleepMode)
            {
                _IsinSleepMode = true;
                _ApplicationSleeping?.Invoke(this, EventArgs.Empty);
            }

        }
        public void OnStart()
        {
            if (_IsinSleepMode)
            {
                _IsinSleepMode = false;
                _ApplicationResuming?.Invoke(this, EventArgs.Empty);
            }
        }

        public static void InitFirebase(Context context, string firebaseToken, string googleAuthWebClientID, List<Authentication.SignInProvider> providers = null)
        {
            _FirebaseToken = firebaseToken;
            Authentication.Droid.FirebaseAuthentication.Init(context, googleAuthWebClientID, providers);
        }


        //public System.Threading.Tasks.Task<string> EmailSignUp(string email, string password)
        //{
        //    return Authentication.Droid.FirebaseAuthentication.EmailSignUp(email, password);
        //}
        //public void SendPasswordResetEmail(string email)
        //{
        //    Authentication.Droid.FirebaseAuthentication.SendPasswordResetEmail(email);
        //}

        //public System.Threading.Tasks.Task<string> EmailSignIn(string email, string password)
        //{


        //    return Authentication.Droid.FirebaseAuthentication.EmailSignIn(email, password);

        //}
        //public void SignOut()
        //{
        //    Authentication.Droid.FirebaseAuthentication.SignOut();
        //}

        public static void PrintHashKey(ContextWrapper pContext)
        {

            try
            {

                PackageInfo info = pContext.PackageManager.GetPackageInfo(pContext.PackageName, Android.Content.PM.PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md5 = MessageDigest.GetInstance("MD5");
                    MessageDigest sha = MessageDigest.GetInstance("SHA");
                    MessageDigest sha256 = MessageDigest.GetInstance("SHA256");

                    sha.Update(signature.ToByteArray());
                    var shaDisc = sha.ToString();

                    string sha1Fingerprint = null;
                    sha1Fingerprint = BitConverter.ToString(sha.Digest()).Replace("-", ":");
                    //foreach (byte _byte in sha.Digest())
                    //{
                    //    if (sha1Fingerprint!=null)
                    //        sha1Fingerprint=":";
                    //    sha1Fingerprint+=_byte.ToString();
                    //}
                    //Log.Debug("KeyHash:", Base64.EncodeToString(md.Digest(), Base64.Default));
                    var s = Base64.EncodeToString(sha.Digest(), Base64Flags.Default);
                }
            }
            catch (NameNotFoundException e)
            {

            }
            catch (NoSuchAlgorithmException e)
            {

            }

            //try
            //{
            //    PackageInfo info = pContext.getPackageManager().getPackageInfo(pContext.getPackageName(), PackageManager.GET_SIGNATURES);
            //    for (Signature signature : info.signatures)
            //    {
            //        MessageDigest md = MessageDigest.getInstance("SHA");
            //        md.update(signature.toByteArray());
            //        String hashKey = new String(Base64.encode(md.digest(), 0));
            //        Log.i(TAG, "printHashKey() Hash Key: " + hashKey);
            //    }
            //}
            //catch (NoSuchAlgorithmException e)
            //{
            //    Log.e(TAG, "printHashKey()", e);
            //}
            //catch (Exception e)
            //{
            //    Log.e(TAG, "printHashKey()", e);
            //}
        }


        public DeviceOOAdvantechCore()
        {
            _LinesPhoneNumbers = new List<SIMCardData>();
            //List<string> args = new List<string>() { "SIM Card 6972992632;73488bc19e444f51a031fe2b72bdee38", "SIM Card  6972992635;39a84c60bbf2460cb7e3d1f6b579d75b", "SIM Card 6972992638;2d40cc3a6b4b4626b18ade71132e19a3" };
            //foreach (var arg in args)
            //{
            //    string simDescription = arg.Substring(0, arg.IndexOf(";"));
            //    string simIdentity = arg.Substring(arg.IndexOf(";") + 1);
            //    _LinesPhoneNumbers.Add(new OOAdvantech.SIMCardData() { SIMCardIdentity = simIdentity, SIMCardDescription = simDescription });
            //}


        }

        public static void OnDestroy()
        {
            HybridWebViewRenderer.OnDestroy();
        }

        List<SIMCardData> _LinesPhoneNumbers;
        public IList<SIMCardData> LinesPhoneNumbers
        {
            get
            {
                return _LinesPhoneNumbers;
            }
        }

        public double ScreeHeight
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double ScreeWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string _DeviceID;
        public string DeviceID
        {
            get
            {
                if (_DeviceID == null)
                    _DeviceID = GetDeviceUniqueID();

                return _DeviceID;

            }
        }






        public SIMCardData GetLinePhoneNumber(int lineIndex)
        {
            return _LinesPhoneNumbers[lineIndex];
        }

        static System.Drawing.Color? StatusBarCurrentColor;

        public System.Drawing.Color? StatusBarColor
        {
            get => StatusBarCurrentColor;
            set
            {
                if (value.HasValue)
                {
                    var statusBarColor = value.Value;
                    if (StatusBarCurrentColor != statusBarColor)
                    {
                        Xamarin.Essentials.Platform.CurrentActivity.Window.SetStatusBarColor(Android.Graphics.Color.Argb(statusBarColor.A, statusBarColor.R, statusBarColor.G, statusBarColor.B)); //here
                        StatusBarCurrentColor = statusBarColor;
                    }
                }
            }
        }

        public IReadOnlyList<SimCard> GetSimCards()
        {
            var results = new List<SimCard>();

            var mTelephonyMgr = (TelephonyManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.TelephonyService);

            SubscriptionManager sm = Android.Telephony.SubscriptionManager.From(Android.App.Application.Context);
            IList<SubscriptionInfo> sis = sm.ActiveSubscriptionInfoList;
            if (sis != null)
            {
                foreach (SubscriptionInfo si in sis)
                {
                    string carrier = si.CarrierName;
                    string iccId = si.IccId;
                    string phoneNum = si.Number;
                    SimCard simCard = new SimCard();
                    simCard.PhoneNumber = si.Number;
                    simCard.ICCID = si.IccId;
                    simCard.MCC = si.Mcc.ToString();
                    simCard.MNC = si.Mnc.ToString();
                    simCard.IMSI = mTelephonyMgr.SubscriberId;
                    simCard.IMEI = mTelephonyMgr.Imei;
                    simCard.NetworkOperatorName = si.DisplayName;
                    results.Add(simCard);

                }
            }
            return results.AsReadOnly();
        }



        public static String GetDeviceUniqueID()
        {
            // If all else fails, if the user does have lower than API 9 (lowerba
            // than Gingerbread), has reset their phone or 'Secure.ANDROID_ID'
            // returns 'null', then simply the ID returned will be solely based
            // off their Android device information. This is where the collisions
            // can happen.
            // Thanks http://www.pocketmagic.net/?p=1662!
            // Try not to use DISPLAY, HOST or ID - these items could change.
            // If there are collisions, there will be overlapping data
            String m_szDevIDShort = "35" +
                    (Build.Board.Length % 10)
                    + (Build.Brand.Length % 10)
                    + (Build.CpuAbi.Length % 10)
                    + (Build.Device.Length % 10)
                    + (Build.Manufacturer.Length % 10)
                    + (Build.Model.Length % 10)
                    + (Build.Product.Length % 10);

            // Thanks to @Roman SL!
            // http://stackoverflow.com/a/4789483/950427
            // Only devices with API >= 9 have android.os.Build.SERIAL
            // http://developer.android.com/reference/android/os/Build.html#SERIAL
            // If a user upgrades software or roots their phone, there will be a duplicate entry
            String serial = null;
            try
            {
                serial = Build.Serial;

                // Go ahead and return the serial for api => 9
                return new Java.Util.UUID(m_szDevIDShort.GetHashCode(), serial.GetHashCode()).ToString();
            }
            catch (Exception e)
            {
                // String needs to be initialized
                serial = "serial"; // some value
            }

            // Thanks @Joe!
            // http://stackoverflow.com/a/2853253/950427
            // Finally, combine the values we have found by using the UUID class to create a unique identifier

            // DebugLog..LOGE(new UUID(m_szDevIDShort.hashCode(), serial.hashCode()).toString());

            return new Java.Util.UUID(m_szDevIDShort.GetHashCode(), serial.GetHashCode()).ToString();
        }

        public bool IsBackgroundServiceStarted
        {
            get
            {
                if (ForegroundServiceManager == null)
                    return false;
                else
                    return ForegroundServiceManager.IsServiceStarted;

            }
        }

        public bool RunInBackground(Action action, BackgroundServiceState serviceState)
        {
            if (ForegroundServiceManager != null)
            {
                return ForegroundServiceManager.Run(action, serviceState);

            }
            return false;
        }

        public void StopBackgroundService()
        {
            if (ForegroundServiceManager != null)
                ForegroundServiceManager.Stop();
        }


        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            string package_name = Application.Context.PackageName;
            var uri = Android.Net.Uri.FromParts("package", package_name, null);
            intent.SetData(uri);
            Application.Context.StartActivity(intent);


        }

        public System.Threading.Tasks.Task<PermissionStatus> RemoteNotificationsPermissionsCheck()
        {

            if ((int)Build.VERSION.SdkInt < 33)
                return Task.FromResult(PermissionStatus.Granted);

            if (Platform.CurrentActivity.CheckSelfPermission(Android.Manifest.Permission.PostNotifications) == Android.Content.PM.Permission.Granted)
                return Task.FromResult(PermissionStatus.Granted);
            else
                return Task.FromResult(PermissionStatus.Denied);
        }



        public System.Threading.Tasks.Task<PermissionStatus> RemoteNotificationsPermissionsRequest()
        {
            return NotificationPermissionsRequest();
        }


        static readonly object locker = new object();

        internal const int requestCodeStart = 12000;

        static int requestCode = requestCodeStart;

        internal static int NextRequestCode()
        {
            if (++requestCode >= 12999)
                requestCode = requestCodeStart;

            return requestCode;
        }
        static readonly Dictionary<int, System.Threading.Tasks.TaskCompletionSource<PermissionStatus>> requests =
                new Dictionary<int, System.Threading.Tasks.TaskCompletionSource<PermissionStatus>>();

        internal System.Threading.Tasks.Task<PermissionStatus> NotificationPermissionsRequest()
        {


            string[] notifyPermission = { Android.Manifest.Permission.PostNotifications };

            if (Platform.CurrentActivity.CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Android.Content.PM.Permission.Granted)
            {

                System.Threading.Tasks.TaskCompletionSource<PermissionStatus> tcs;

                int notificationRequestCode = 0;
                lock (locker)
                {
                    tcs = new System.Threading.Tasks.TaskCompletionSource<PermissionStatus>();

                    notificationRequestCode = NextRequestCode();

                    requests.Add(notificationRequestCode, tcs);
                }

                if (!MainThread.IsMainThread)
                    throw new PermissionException("Permission request must be invoked on main thread.");

                Platform.CurrentActivity.RequestPermissions(notifyPermission, notificationRequestCode);
                return tcs.Task;

            }
            else
                return Task.FromResult(PermissionStatus.Granted);


        }

        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {

            lock (locker)
            {
                if (requests.ContainsKey(requestCode))
                {
                    if (grantResults.Any(g => g == Android.Content.PM.Permission.Denied))
                        requests[requestCode].TrySetResult(PermissionStatus.Denied);
                    else
                        requests[requestCode].TrySetResult(PermissionStatus.Granted);

                    requests.Remove(requestCode);
                }
            }
        }

      
    }



    /// <MetaDataID>{155ebe9a-f9fc-4605-9bab-95962a9488b3}</MetaDataID>
    public class RemoteMessage : IRemoteMessage
    {
        public IDictionary<string, string> Data { get; set; }

        public string From { get; set; }

        public string MessageId { get; set; }

        public string MessageType { get; set; }

        public DateTime SentTime { get; set; }

        public string To { get; set; }
    }

    /// <MetaDataID>{b36b90d9-cb65-4b0e-9299-f7fdca46872a}</MetaDataID>
    public class BatteryImplementation : IBatteryInfo
    {
        public bool CheckIsEnableBatteryOptimizations()
        {
            //if (Build.Brand.ToLower()=="xiaomi")
            //{
            //    //https://stackoverflow.com/questions/44383983/how-to-programmatically-enable-auto-start-and-floating-window-permissions
            //    Intent intent = new Intent();
            //    intent.SetComponent(new ComponentName("com.miui.securitycenter", "com.miui.permcenter.autostart.AutoStartManagementActivity"));
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    Android.App.Application.Context.StartActivity(intent);
            //}


            PowerManager pm = (PowerManager)Android.App.Application.Context.GetSystemService(Context.PowerService);
            //enter you package name of your application
            string pName = Xamarin.Essentials.AppInfo.PackageName;
            bool result = pm.IsIgnoringBatteryOptimizations(Xamarin.Essentials.AppInfo.PackageName);
            return result;
        }

        public void StartSetting()
        {

            Intent intent = new Intent();
            intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.PutExtra("package", Application.Context.PackageName);
            intent.PutExtra("uid", Application.Context.ApplicationInfo.Uid);
            Android.App.Application.Context.StartActivity(intent);

            NotificationManagerCompat manager = NotificationManagerCompat.From(Android.App.Application.Context);
            var IsAllowed = manager.AreNotificationsEnabled();


            //navigate to the notification setting page to remind the user to check the option
            intent = new Intent(Android.Provider.Settings.ActionAppNotificationSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.PutExtra("package", Application.Context.PackageName);
            intent.PutExtra("uid", Application.Context.ApplicationInfo.Uid);
            Android.App.Application.Context.StartActivity(intent);


            // StartActivity(intent);
        }
    }
}