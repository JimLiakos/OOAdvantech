using System;
using System.Collections.Generic;
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

    }
}
