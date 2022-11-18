using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{


    public delegate void AppLifeCycleHandler();
    /// <MetaDataID>{bab82937-4d36-4198-864b-d3e78f853e8b}</MetaDataID>
    public class DeviceApplication
    {


        public event AppLifeCycleHandler Started;
        public event AppLifeCycleHandler Sleeping;
        public event AppLifeCycleHandler Resumed;


        static DeviceApplication _Current=new DeviceApplication();
        public static DeviceApplication Current
        {
            get
            {
                return _Current;
            }
        }

        public void OnStart()
        {
            Started?.Invoke();
            // Handle when your app starts
        }

        public void OnSleep()
        {
            Sleeping?.Invoke();
            // Handle when your app sleeps
        }

        public void OnResume()
        {
            Resumed?.Invoke();
            // Handle when your app resumes
        }

        public void Log(List<string> lines)
        {
            const string errorFileName = "Common.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            File.AppendAllLines(errorFilePath, lines);
        }

        public string ReadLog()
        {
            const string errorFileName = "Common.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            if (File.Exists(errorFilePath))
                return File.ReadAllText(errorFilePath);
            else
                return "";
        }

        public void ClearLog()
        {
            const string errorFileName = "Common.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            File.Delete(errorFilePath);
        }

    }
}
