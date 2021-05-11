﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Win32
{
    /// <summary>
    /// The RECT structure defines the coordinates of the upper-left 
    /// and lower-right corners of a rectangle
    /// </summary>
    /// <MetaDataID>{b85c8388-6e5b-450b-94c7-6ce8d70b6baa}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        /// <summary>
        /// Specifies the x-coordinate of the upper-left corner of the RECT
        /// </summary>
        public int left;

        /// <summary>
        /// Specifies the y-coordinate of the upper-left corner of the RECT
        /// </summary>
        public int top;

        /// <summary>
        /// Specifies the x-coordinate of the lower-right corner of the RECT
        /// </summary>
        public int right;

        /// <summary>
        /// Specifies the y-coordinate of the lower-right corner of the RECT
        /// </summary>
        public int bottom;


        /// <summary>
        /// Creates a new RECT struct with the specified location and size
        /// </summary>
        /// <param name="left">The x-coordinate of the upper-left corner of the RECT</param>
        /// <param name="top">The y-coordinate of the upper-left corner of the RECT</param>
        /// <param name="right">The x-coordinate of the lower-right corner of the RECT</param>
        /// <param name="bottom">The y-coordinate of the lower-right corner of the RECT</param>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }


        /// <summary>
        /// Creates a new RECT struct from the specified Rectangle
        /// </summary>
        /// <param name="rect">The Rectangle to create the RECT from</param>
        /// <returns>A RECT struct with the same location and size as 
        /// the specified Rectangle</returns>
        public static RECT FromRectangle(Rectangle rect)
        {
            return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }


        /// <summary>
        /// Creates a new RECT struct with the specified location and size
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the RECT</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the RECT</param>
        /// <param name="width">The width of the RECT</param>
        /// <param name="height">The height of the RECT</param>
        /// <returns>A RECT struct with the specified location and size</returns>
        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }


        /// <summary>
        /// Returns a Rectangle with the same location and size as the RECT
        /// </summary>
        /// <returns>A Rectangle with the same location and size as the RECT</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
        }
    }
}
