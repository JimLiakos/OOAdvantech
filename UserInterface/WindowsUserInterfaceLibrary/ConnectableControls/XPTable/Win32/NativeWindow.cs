/*
 * Copyright © 2004, Daniel Turini
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


namespace ConnectableControls.List.Win32
{
    /// <summary>
    /// Summary description for NativeWindow
    /// </summary>
    /// <MetaDataID>{0B5B717A-7AAE-4115-B4A0-6FE53C2549A6}</MetaDataID>
	internal class NativeWindow
	{
		#region Class Data
		
		/// <summary>
		/// 
		/// </summary>
		private IntPtr handle;

		/// <summary>
		/// Prevents the delegate being collected
		/// </summary>
		private WndProcDelegate wndProcDelegate;

		/// <summary>
		/// 
		/// </summary>
		private IntPtr oldWndFunc;

		/// <summary>
		/// 
		/// </summary>
		private delegate IntPtr WndProcDelegate(IntPtr hwnd, int Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// 
		/// </summary>
		private const int GWL_WNDPROC = -4;

		#endregion
		

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the NativeWindow class
        /// </summary>
        /// <MetaDataID>{B74F504A-9FAD-4912-8697-8DBEA84DADB5}</MetaDataID>
		public NativeWindow()
		{
			wndProcDelegate = new WndProcDelegate(this.WndProc);
		}

		#endregion


		#region Methods

        /// <summary>
        /// Assigns a handle to this window
        /// </summary>
        /// <param name="hWnd">The handle to assign to this window</param>
        /// <MetaDataID>{B02ADDB4-0EA2-4B14-8874-49B65005A1CF}</MetaDataID>
		public void AssignHandle(IntPtr hWnd)
		{
			handle = hWnd;
			oldWndFunc = SetWindowLong(hWnd, GWL_WNDPROC, wndProcDelegate);
		}


        /// <summary>
        /// Releases the handle associated with this window
        /// </summary>
        /// <MetaDataID>{0E2CA061-E288-43AE-8AEE-87E5D0FE3D1A}</MetaDataID>
		public void ReleaseHandle()
		{
			SetWindowLong(handle, GWL_WNDPROC, oldWndFunc);
			handle = IntPtr.Zero;
			oldWndFunc = IntPtr.Zero;
		}


        /// <summary>
        /// Invokes the default window procedure associated with this window
        /// </summary>
        /// <param name="msg">A Message that is associated with the current Windows message</param>
        /// <MetaDataID>{63D73CE2-2842-44EB-A484-A34989156106}</MetaDataID>
		protected virtual void WndProc(ref System.Windows.Forms.Message msg)
		{
			DefWndProc(ref msg);
		}


        /// <summary>
        /// Invokes the default window procedure associated with this window. 
        /// It is an error to call this method when the Handle property is 0
        /// </summary>
        /// <param name="m">A Message that is associated with the current Windows message</param>
        /// <MetaDataID>{0E9DA03D-B6CE-4654-9786-410F5E8CB4E1}</MetaDataID>
		public void DefWndProc(ref System.Windows.Forms.Message m)
		{
			m.Result = CallWindowProc(oldWndFunc, m.HWnd, m.Msg, m.WParam, m.LParam);
		}


        /// <summary>
        /// Handler for the WndProcDelegate
        /// </summary>
        /// <param name="hWnd">Handle to the window procedure to receive the message</param>
        /// <param name="msg">Specifies the message</param>
        /// <param name="wParam">Specifies additional message-specific information. The contents 
        /// of this parameter depend on the value of the Msg parameter</param>
        /// <param name="lParam">Specifies additional message-specific information. The contents 
        /// of this parameter depend on the value of the Msg parameter</param>
        /// <returns>The return value specifies the result of the message processing and depends 
        /// on the message sent</returns>
        /// <MetaDataID>{243C77DB-AF72-4EB3-B795-179F8D1ED0D1}</MetaDataID>
		private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			System.Windows.Forms.Message m = System.Windows.Forms.Message.Create(hWnd, msg, wParam, lParam);
			WndProc(ref m);
			return m.Result;
		}


        /// <summary>
        /// The SetWindowLong function changes an attribute of the specified window. The 
        /// function also sets the 32-bit (long) value at the specified offset into the 
        /// extra window memory
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which 
        /// the window belongs</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be set.</param>
        /// <param name="wndProcDelegate">Specifies the replacement value</param>
        /// <returns>If the function succeeds, the return value is the previous value of 
        /// the specified 32-bit integer. If the function fails, the return value is zero</returns>
        /// <MetaDataID>{10BDCFCB-06B7-4267-B9A1-C72B3DEC2741}</MetaDataID>
		[DllImport("User32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WndProcDelegate wndProcDelegate);


        /// <summary>
        /// The SetWindowLong function changes an attribute of the specified window. The 
        /// function also sets the 32-bit (long) value at the specified offset into the 
        /// extra window memory
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which 
        /// the window belongs</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be set.</param>
        /// <param name="wndFunc">Specifies the replacement value</param>
        /// <returns>If the function succeeds, the return value is the previous value of 
        /// the specified 32-bit integer. If the function fails, the return value is zero</returns>
        /// <MetaDataID>{651E9272-D334-4CED-B9F9-55EED85D67BC}</MetaDataID>
		[DllImport("User32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr wndFunc);


        /// <summary>
        /// The CallWindowProc function passes message information to the specified window 
        /// procedure
        /// </summary>
        /// <param name="prevWndFunc">Pointer to the previous window procedure. If this value 
        /// is obtained by calling the GetWindowLong function with the nIndex parameter set to 
        /// GWL_WNDPROC or DWL_DLGPROC, it is actually either the address of a window or dialog 
        /// box procedure, or a special internal value meaningful only to CallWindowProc</param>
        /// <param name="hWnd">Handle to the window procedure to receive the message</param>
        /// <param name="iMsg">Specifies the message</param>
        /// <param name="wParam">Specifies additional message-specific information. The contents 
        /// of this parameter depend on the value of the Msg parameter</param>
        /// <param name="lParam">Specifies additional message-specific information. The contents 
        /// of this parameter depend on the value of the Msg parameter</param>
        /// <returns>The return value specifies the result of the message processing and depends 
        /// on the message sent</returns>
        /// <MetaDataID>{B9E3AFD9-7C35-4ED7-BC2F-4DCE1C16E828}</MetaDataID>
		[DllImport("User32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern IntPtr CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int iMsg, IntPtr wParam, IntPtr lParam);

		#endregion


		#region Properties

		/// <summary>
		/// Gets the handle for this window
		/// </summary>
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}

		#endregion
	}
}
