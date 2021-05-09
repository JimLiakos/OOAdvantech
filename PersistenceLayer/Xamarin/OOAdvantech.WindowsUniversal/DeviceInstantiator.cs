using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(OOAdvantech.WindowsUniversal.DeviceInstantiator))]
namespace OOAdvantech.WindowsUniversal
{


    /// <MetaDataID>{a63effd4-2a59-480e-be8c-1f3fb2eb519c}</MetaDataID>
    public class DeviceInstantiator : IDeviceInstantiator
    {
        public object GetDeviceSpecific(System.Type type)
        {

            if (type == typeof(OOAdvantech.IDeviceOOAdvantechCore))
                return new DeviceOOAdvantechCore();


            if (type == typeof(OOAdvantech.IFileSystem))
                return new DeviceFileSystem();

            if (type == typeof(OOAdvantech.Localization.ILocalize))
                return new Localize();

            if (type == typeof(OOAdvantech.Speech.ITextToSpeech))
                return new TextToSpeechImplementation();

            if (type == typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection))
            {
                var sds= typeof(OOAdvantech.Web.INativeWebBrowser);
                var connection = new OOAdvantech.SQLitePersistenceRunTime.SQLiteDataBaseConnection();
                return connection;
            }
            // Windows.Storage.StorageFile.

            //System.IO.Path path= System.IO.Path.GetDirectoryName()
            //Windows.Storage.ApplicationData.Current.LocalFolder.
            return null;
        }

        public static void Init()
        {
            OOAdvantech.SQLitePersistenceRunTime.StorageProvider.init();
            HybridWebViewRenderer.Init();
            Websockets.Universal.WebsocketConnection.Link();
        }
    }
}
