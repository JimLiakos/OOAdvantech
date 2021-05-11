using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ConnectableControls.SchedulerControl.Win32
{
    /// <summary>
    /// Receives dynamic-link library (DLL)-specific version information. 
    /// It is used with the DllGetVersion function
    /// </summary>
    /// <MetaDataID>{e61e4602-9ac2-4497-bc02-59fccc6e5591}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DLLVERSIONINFO
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be filled 
        /// in before calling the function
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Major version of the DLL. If the DLL's version is 4.0.950, 
        /// this value will be 4
        /// </summary>
        public int dwMajorVersion;

        /// <summary>
        /// Minor version of the DLL. If the DLL's version is 4.0.950, 
        /// this value will be 0
        /// </summary>
        public int dwMinorVersion;

        /// <summary>
        /// Build number of the DLL. If the DLL's version is 4.0.950, 
        /// this value will be 950
        /// </summary>
        public int dwBuildNumber;

        /// <summary>
        /// Identifies the platform for which the DLL was built
        /// </summary>
        public int dwPlatformID;
    }
}
