using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OOAdvantech.Remoting;
using OOAdvantech.Remoting.RestApi;
[assembly: Xamarin.Forms.Dependency(typeof(OOAdvantech.Droid.DeviceInstantiator))]
namespace OOAdvantech.Droid
{
    /// <MetaDataID>{8d6abafd-9980-418d-83db-dc2946d99465}</MetaDataID>
    public class DeviceInstantiator : IDeviceInstantiator
    {
        public object GetDeviceSpecific(System.Type type)
        {
            if (type == typeof(OOAdvantech.IDeviceOOAdvantechCore))
                return new DeviceOOAdvantechCore();

            if (type == typeof(OOAdvantech.Speech.ITextToSpeech))
                return new TextToSpeechImplementation();

            if (type == typeof(OOAdvantech.Localization.ILocalize))
                return new Localize();

            if (type == typeof(OOAdvantech.IFileSystem))
                return new DeviceFileSystem();

            if (type == typeof(OOAdvantech.IBatteryInfo))
                return new BatteryImplementation();

            if (type == typeof(Authentication.IAuth))
                return new Authentication.Droid.Auth();

            if(type==typeof(OOAdvantech.IRingtoneService))
                return new OOAdvantech.Droid.RingtoneService();

            if (type == typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection))
            {
                var connection=new OOAdvantech.SQLitePersistenceRunTime.SQLiteDataBaseConnection();
                connection.SQLiteFilePath = @"\Abstractions.sqlite";
                return connection;
            }

            return null;
        }

        static KeyboardService.GlobalLayoutListener GlobalLayoutListener;
        public static void Init()
        {

            if (GlobalLayoutListener == null)
            {
                GlobalLayoutListener = new KeyboardService.GlobalLayoutListener();
                Xamarin.Essentials.Platform.CurrentActivity.Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(GlobalLayoutListener);
            }

            var deviceAuthentication = MonoStateClass.GetInstance(typeof(DeviceAuthentication)) as DeviceAuthentication;


            ThreadHelper.Initialize( System.Environment.CurrentManagedThreadId);
            OOAdvantech.SQLitePersistenceRunTime.StorageProvider.init();

            OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider.Init();
            HybridWebViewRenderer.Init();
#if NetStandard

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
#endif

#if PORTABLE
            Websockets.Droid.WebsocketConnection.Link();
#endif

        }
    }


    /// <MetaDataID>{e8922ff2-c20e-4c81-b900-21c9ece00018}</MetaDataID>
    public static class ThreadHelper
    {
        public static int MainThreadId { get; private set; }

        public static void Initialize(int mainThreadId)
        {
            MainThreadId = mainThreadId;
        }

        public static bool IsOnMainThread => System.Environment.CurrentManagedThreadId == MainThreadId;
    }
}