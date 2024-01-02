using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Debug
{
    public class Logger
    {
        public static void Log(List<string> lines)
        {
#if DeviceDotNet
            DeviceApplication.Current.Log(lines);
#endif
        }
        public static void ClearLog()
        {
#if DeviceDotNet
            DeviceApplication.Current.ClearLog();
#endif
        }
        public static string ReadLog()
        {
#if DeviceDotNet
            return DeviceApplication.Current.ReadLog();
#endif

        }
    }
}
