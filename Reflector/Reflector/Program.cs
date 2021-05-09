using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Reflector
{
    /// <MetaDataID>{b23e8da0-c2a4-47c2-8a50-651610103ccb}</MetaDataID>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            System.DateTime backupDateTime = System.DateTime.UtcNow;

            SystemDateTime.SetSystameDateTime(new System.DateTime(2007, backupDateTime.Month, backupDateTime.Day));
            backupDateTime.AddSeconds(10);
            System.Threading.Thread.Sleep(3000);
            try
            {
                System.Diagnostics.Process.Start(args[0]);
            }
            catch (Exception error)
            {

                
            }
            System.Threading.Thread.Sleep(7000);
            SystemDateTime.SetSystameDateTime(backupDateTime);
            

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }

    /// <MetaDataID>{231d8259-d629-4f48-b126-170e2e203be8}</MetaDataID>
    static class SystemDateTime
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetSystemTime(ref SYSTEMTIME st);


        public static void SetSystameDateTime(System.DateTime dateTime)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            

            st.wYear =(short)dateTime.Year; // must be short
            st.wMonth = (short)dateTime.Month;
            st.wDay = (short)dateTime.Day;
            st.wHour = (short)dateTime.Hour;
            st.wMinute = (short)dateTime.Minute;
            st.wSecond = (short)dateTime.Second;

            SetSystemTime(ref st);
        }
    }

}
