using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{

    /// <MetaDataID>{d3aa9fb1-b99f-4872-8176-bd088d5f14b6}</MetaDataID>
    public class BackPressedArgs
    {
        public bool Handled;

    }

    public delegate void BackPressedHandle(BackPressedArgs eventArgs);
    public delegate void AppLifeCycleHandler();
    /// <MetaDataID>{bab82937-4d36-4198-864b-d3e78f853e8b}</MetaDataID>
    public class DeviceApplication
    {


        public event AppLifeCycleHandler Started;
        public event AppLifeCycleHandler Sleeping;
        public event AppLifeCycleHandler Resumed;

        public event BackPressedHandle BackPressed;



        static DeviceApplication _Current = new DeviceApplication();
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

        public void OnBackPressed(BackPressedArgs eventArgs)
        {
            BackPressed?.Invoke(eventArgs);
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

        List<string> CachedLines = new List<string>();


        public void Log(List<string> lines)
        {
            foreach (var line in lines.ToList())
            {
                var index = lines.IndexOf(line);
                lines[index]=DateTime.Now.ToString()+" : "+line;
            }
            lock (this)
            {

                CachedLines.AddRange(lines);
                int count = 5;
                //do
                //{
                try
                {

                    const string errorFileName = "Common.log";
                    var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                    var errorFilePath = Path.Combine(libraryPath, errorFileName);
                    File.AppendAllLines(errorFilePath, CachedLines);


                    var liness= File.ReadAllLines(errorFilePath);
                    CachedLines.Clear();
                    return;
                }
                catch (Exception error)
                {
                    // System.Threading.Thread.Sleep(200);

                }
                //    count--;
                //} while (count>0);     
            }
        }

        public string ReadLog()
        {
            lock (this)
            {
                const string errorFileName = "Common.log";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                if (File.Exists(errorFilePath))
                    return File.ReadAllText(errorFilePath);
                else
                    return "";
            }
        }

        public void ClearLog()
        {
            lock (this)
            {
                const string errorFileName = "Common.log";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                File.Delete(errorFilePath);
            }
        }

    }
}
