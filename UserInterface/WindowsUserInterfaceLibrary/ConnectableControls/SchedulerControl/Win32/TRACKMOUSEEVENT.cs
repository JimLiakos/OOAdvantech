using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ConnectableControls.SchedulerControl.Win32
{
    /// <summary>
    /// The TRACKMOUSEEVENT structure is used by the TrackMouseEvent function 
    /// to track when the mouse pointer leaves a window or hovers over a window 
    /// for a specified amount of time
    /// </summary>
    /// <MetaDataID>{e27a7e36-bfc0-4c99-ac21-3cf2cd1de136}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    internal class TRACKMOUSEEVENT
    {
        /// <summary>
        /// Specifies the size of the TRACKMOUSEEVENT structure
        /// </summary>
        /// <MetaDataID>{e4fd7375-c1d2-473e-a5b7-c8d0c3f1993f}</MetaDataID>
        public int cbSize;

        /// <summary>
        /// Specifies the services requested
        /// </summary>
        /// <MetaDataID>{ab45b543-b7ee-4029-85e0-f8286b0ecb48}</MetaDataID>
        public int dwFlags;

        /// <summary>
        /// Specifies a handle to the window to track
        /// </summary>
        /// <MetaDataID>{16abeda6-6b08-4e50-84b8-5895a3deeb01}</MetaDataID>
        public IntPtr hwndTrack;

        /// <summary>
        /// Specifies the hover time-out in milliseconds
        /// </summary>
        /// <MetaDataID>{1b22c2ca-d89c-40bc-a4df-cc3dca54aa92}</MetaDataID>
        public int dwHoverTime;


        /// <summary>
        /// Creates a new TRACKMOUSEEVENT struct with default settings
        /// </summary>
        /// <MetaDataID>{0B955AAC-280C-47CB-AD5D-3FE608DDC303}</MetaDataID>
        public TRACKMOUSEEVENT()
        {
            // Marshal.SizeOf() uses SecurityAction.LinkDemand to prevent 
            // it from being called from untrusted code, so make sure we 
            // have permission to call it
            SecurityPermission permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            permission.Demand();

            this.cbSize = Marshal.SizeOf(typeof(TRACKMOUSEEVENT));

            this.dwFlags = 0;
            this.hwndTrack = IntPtr.Zero;
            this.dwHoverTime = 100;
        }
    }
}
