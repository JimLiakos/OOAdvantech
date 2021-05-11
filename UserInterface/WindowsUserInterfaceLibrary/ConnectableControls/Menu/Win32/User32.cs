// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetConnectableControls.com
// *****************************************************************************

using System;
using System.Runtime.InteropServices;

namespace ConnectableControls.Menus.Win32
{
	/// <MetaDataID>{7330FF6B-C2EA-413C-A271-1324BD7BF893}</MetaDataID>
    public class User32
    {
		/// <MetaDataID>{DD760D03-43FE-4667-9955-4A776C3CAF9B}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
            
		/// <MetaDataID>{12EC3C84-417B-4410-B7DC-4AE2F78D3B08}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newLong);
            
		/// <MetaDataID>{A3DAA5AA-4D00-4C04-AEFF-775A344236F1}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);

		/// <MetaDataID>{1DB942EC-F415-4F84-8BB2-CA7254200599}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hWnd, uint dwTime, uint dwFlags);

		/// <MetaDataID>{CFE5579C-6F40-4EC9-AE71-A57261BACE04}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

		/// <MetaDataID>{EE3315C8-52BE-45EB-A588-50F6B6C4FDE8}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

		/// <MetaDataID>{E4CA1284-B4AB-407A-98E3-068FF568D2BB}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);

		/// <MetaDataID>{000C1496-9CB8-479B-AA6B-34E4D1BA6C19}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

		/// <MetaDataID>{6421FBBE-4CEC-415B-91DA-883232C83FE0}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

		/// <MetaDataID>{F5259048-01ED-408C-BF59-12CBB620B88C}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ReleaseCapture();

		/// <MetaDataID>{74F33864-5D56-443F-BF15-67C635B8ED7C}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool WaitMessage();

		/// <MetaDataID>{988AC906-DF4B-4035-B6EB-234A14BBB869}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TranslateMessage(ref MSG msg);

		/// <MetaDataID>{EEC6AE80-12CF-42F7-BFD8-B1F40435A4C8}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DispatchMessage(ref MSG msg);

		/// <MetaDataID>{1E9A9C22-B576-4E1C-9D56-BB62A5728D23}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

		/// <MetaDataID>{1CB982FE-F3B4-4FD6-B640-84D66B60D625}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

		/// <MetaDataID>{9F880D04-F409-4100-B530-8F8283142E31}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);
	
		/// <MetaDataID>{C72C552D-0AD2-4DC6-9E02-436E59EDCBEF}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

		/// <MetaDataID>{FB6A7107-3F76-4151-A62B-488FD9817530}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

		/// <MetaDataID>{DD2BF9BA-0B77-4142-945F-364B6F764BEA}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

		/// <MetaDataID>{AB9891B0-875A-4AC1-B5FB-B824BE379F86}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

		/// <MetaDataID>{59F6FB29-CBB3-462A-8413-7A517FDECA48}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		/// <MetaDataID>{8489A519-E148-4F1D-BF29-3689E8D7DB96}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

		/// <MetaDataID>{1F211E52-00FB-499D-9FDE-52F5992CA1D1}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

		/// <MetaDataID>{9539696F-C61D-4895-BA0A-F02E64D5C94D}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, uint flags);

		/// <MetaDataID>{7F7140BF-8C70-47BB-BC5C-41664E8ED943}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

		/// <MetaDataID>{9CDC15D5-5E50-46E0-BE39-2CE5787619A3}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		/// <MetaDataID>{E9963B02-3A90-44AC-A574-506EF760A318}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT pt);

		/// <MetaDataID>{F3421BD2-169C-4F7C-8A4C-696B6A63E9D3}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT pt);

		/// <MetaDataID>{DF416096-D1E4-488F-8BAD-5FBB1E58462F}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

		/// <MetaDataID>{7B3F8D35-98D7-4F44-B51C-B9FB6DB095E9}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

		/// <MetaDataID>{45B8F10B-90DE-4FE7-AEF6-774E4E11C133}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern ushort GetKeyState(int virtKey);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetKeyboardState(ref byte keyState);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ToUnicode(uint wVirtKey,uint wScanCode,ref byte keyState,char[] pwszBuff,int cchBuff,uint wFlags);

        

		/// <MetaDataID>{4E6FC1BE-415E-4317-A47E-7F4BE11BD613}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

		/// <MetaDataID>{D140FB62-C951-4324-B7E1-78760C40D22D}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DrawFocusRect(IntPtr hWnd, ref RECT rect);

		/// <MetaDataID>{4340BA8D-9532-4844-A21C-1BBD12907443}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool HideCaret(IntPtr hWnd);

		/// <MetaDataID>{AEA830FB-9763-4079-B298-9814F499D681}</MetaDataID>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowCaret(IntPtr hWnd);
    }
}