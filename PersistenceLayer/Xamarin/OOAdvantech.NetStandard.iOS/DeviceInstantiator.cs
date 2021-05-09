using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(OOAdvantech.iOS.DeviceInstantiator))]
namespace OOAdvantech.iOS
{
    /// <MetaDataID>{2b9079d4-92b4-4e36-b12f-011a053528cd}</MetaDataID>
    public class DeviceInstantiator : IDeviceInstantiator
    {

        public static void Init()
        {
            


            ThreadHelper.Initialize(System.Environment.CurrentManagedThreadId);
            OOAdvantech.SQLitePersistenceRunTime.StorageProvider.init();

            OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider.Init();
            HybridWebViewRenderer.Init();
#if NetStandard

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#endif

#if PORTABLE
            Websockets.Droid.WebsocketConnection.Link();
#endif

        }
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

            if (type == typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection))
            {
                var connection = new OOAdvantech.SQLitePersistenceRunTime.SQLiteDataBaseConnection();
                connection.SQLiteFilePath = @"\Abstractions.sqlite";
                return connection;
            }
            return null;
        }
    }
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
