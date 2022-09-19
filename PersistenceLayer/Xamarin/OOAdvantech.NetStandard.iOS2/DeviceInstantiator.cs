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
            OOAdvantech.SQLitePersistenceRunTime.StorageProvider.init();
            HybridWebViewRenderer.Init();
            WebSocket.io.WebsocketConnection.Link();
        }
        public object GetDeviceSpecific(System.Type type)
        {


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
}
