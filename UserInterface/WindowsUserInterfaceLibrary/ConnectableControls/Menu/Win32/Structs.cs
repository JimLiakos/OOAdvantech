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
using System.Drawing;
using System.Runtime.InteropServices;

namespace ConnectableControls.Menus.Win32
{
    /// <MetaDataID>{a452e412-5d8a-4c7f-8b69-2c467d0a2d5c}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG 
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    }

    /// <MetaDataID>{c35b02e5-87c7-4419-9dd8-5ade2f1e9e10}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public Rectangle rcPaint;
        public int fRestore;
        public int fIncUpdate;
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
        public int Reserved5;
        public int Reserved6;
        public int Reserved7;
        public int Reserved8;
    }

    /// <MetaDataID>{90f95c2d-fce5-4f87-85df-7ba6a489473d}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    /// <MetaDataID>{6ff6496c-01ec-4f92-884f-df14cd20460e}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    /// <MetaDataID>{af4bcda8-87be-42a3-9772-60b11ebb7e37}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
    }

    /// <MetaDataID>{22b31d53-2190-41dc-8169-de45563ae3c4}</MetaDataID>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    /// <MetaDataID>{24160aae-5290-42c0-900b-5e418cde2bb7}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENTS
    {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hWnd;
        public uint dwHoverTime;
    }

    /// <MetaDataID>{f66a537f-372a-4507-b929-ef5fbe44eea2}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    public struct LOGBRUSH
    {
        public uint lbStyle; 
        public uint lbColor; 
        public uint lbHatch; 
    }
}
