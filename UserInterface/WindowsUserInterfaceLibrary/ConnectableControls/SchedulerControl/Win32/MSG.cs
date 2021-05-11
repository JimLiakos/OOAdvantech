using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ConnectableControls.SchedulerControl.Win32
{
    /// <MetaDataID>{b1ec9ad3-721f-4f8e-a623-0c50cd78a5a3}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }
}
