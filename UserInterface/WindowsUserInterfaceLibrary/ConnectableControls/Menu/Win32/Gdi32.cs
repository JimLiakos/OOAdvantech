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
    
	/// <MetaDataID>{FD7B9263-E1BD-4D30-AC0A-31F11C6D317F}</MetaDataID>
    public class Gdi32
    {
		/// <MetaDataID>{F0434F0F-E527-4CB9-B9DB-585DFA62ABEF}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int CombineRgn(IntPtr dest, IntPtr src1, IntPtr src2, int flags);

		/// <MetaDataID>{03388B05-08D1-41A6-BB33-3696A48DEEF2}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateRectRgnIndirect(ref Win32.RECT rect); 

		/// <MetaDataID>{37D17CDE-C832-421F-9E5C-A1B5E791F1F3}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClipBox(IntPtr hDC, ref Win32.RECT rectBox); 

		/// <MetaDataID>{778FC8A9-58F9-4819-93B7-28D23F7657A6}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn); 

		/// <MetaDataID>{8E6F70F3-EC58-4F8B-81B7-7A33BF4FEF23}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateBrushIndirect(ref LOGBRUSH brush); 

		/// <MetaDataID>{12CAF2EC-23B7-49CA-A3C4-CEC4CED37EC7}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool PatBlt(IntPtr hDC, int x, int y, int width, int height, uint flags); 

		/// <MetaDataID>{5BDC9C86-3C4D-4758-B28B-A3693BA2E4EB}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr DeleteObject(IntPtr hObject);

		/// <MetaDataID>{25A4FCA1-A15A-4FBB-A89A-AA16DA1D0250}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hDC);

		/// <MetaDataID>{9E878D1A-456A-4F87-8726-D9CFD3ED09ED}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		/// <MetaDataID>{C532AE44-B0B8-4955-88D3-DBA336425DDB}</MetaDataID>
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
    }
}