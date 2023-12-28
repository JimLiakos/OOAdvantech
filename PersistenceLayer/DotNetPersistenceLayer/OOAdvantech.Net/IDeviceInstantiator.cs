using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Xamarin.Forms.Dependency(typeof(OOAdvantech.Net.DeviceInstantiator))]


namespace OOAdvantech
{
    /// <MetaDataID>{2bff54ae-3cdc-462a-9638-4d9c5248adce}</MetaDataID>
    public interface IDeviceInstantiator
    {
        /// <MetaDataID>{9aaf41ea-ece6-44d3-9225-2beaed75b12d}</MetaDataID>
        object GetDeviceSpecific(System.Type type);
    }
}
namespace OOAdvantech.Net
{


    /// <MetaDataID>{0351e306-3087-46d0-88f6-2e558ba7f369}</MetaDataID>
    public class DeviceInstantiator : IDeviceInstantiator
    {

   

        /// <MetaDataID>{ddb5e8ce-0827-477f-b101-761eb1e16e1e}</MetaDataID>
        public object GetDeviceSpecific(System.Type type)
        {

            if (type == typeof(OOAdvantech.Speech.ITextToSpeech))
                return new TextToSpeechImplementation();

            if (type == typeof(OOAdvantech.IDeviceOOAdvantechCore))
                return new DeviceOOAdvantechCore();


            //if (type == typeof(OOAdvantech.Localization.ILocalize))
            //    return new Localize();

            //if (type == typeof(OOAdvantech.IFileSystem))
            //    return new DeviceFileSystem();

            //if (type == typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection))
            //{
            //    var connection = new OOAdvantech.SQLitePersistenceRunTime.SQLiteDataBaseConnection();
            //    connection.SQLiteFilePath = @"\Abstractions.sqlite";
            //    return connection;
            //}

            return null;
        }

        //public static void Init()
        //{
        //    OOAdvantech.SQLitePersistenceRunTime.StorageProvider.init();
        //    HybridWebViewRenderer.Init();
        //    Websockets.Droid.WebsocketConnection.Link();
        //}
    }
}
