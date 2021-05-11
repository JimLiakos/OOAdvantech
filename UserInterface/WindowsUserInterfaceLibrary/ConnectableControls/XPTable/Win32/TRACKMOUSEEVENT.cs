/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;


namespace ConnectableControls.List.Win32
{
    /// <summary>
    /// The TRACKMOUSEEVENT structure is used by the TrackMouseEvent function 
    /// to track when the mouse pointer leaves a window or hovers over a window 
    /// for a specified amount of time
    /// </summary>
    /// <MetaDataID>{37F5993C-143C-4A0E-A563-3517FAF2A03E}</MetaDataID>
	[StructLayout(LayoutKind.Sequential)]
	internal class TRACKMOUSEEVENT
	{
        /// <summary>
        /// Specifies the size of the TRACKMOUSEEVENT structure
        /// </summary>
        /// <MetaDataID>{f96e6d40-f5a7-4316-b0ef-38b68b0bd3bd}</MetaDataID>
		public int cbSize;

        /// <summary>
        /// Specifies the services requested
        /// </summary>
        /// <MetaDataID>{a7572a68-e8da-4454-beed-a0f8aa806eff}</MetaDataID>
		public int dwFlags;

        /// <summary>
        /// Specifies a handle to the window to track
        /// </summary>
        /// <MetaDataID>{9e1af816-f443-45fc-8560-a758ca6c92a7}</MetaDataID>
		public IntPtr hwndTrack;

        /// <summary>
        /// Specifies the hover time-out in milliseconds
        /// </summary>
        /// <MetaDataID>{819840e7-46be-45e1-be17-febd1c418734}</MetaDataID>
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
