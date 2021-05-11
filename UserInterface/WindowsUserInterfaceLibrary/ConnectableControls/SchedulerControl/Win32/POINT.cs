using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Win32
{
    /// <summary>
    /// The POINT structure defines the x- and y- coordinates of a point
    /// </summary>
    /// <MetaDataID>{31a5e2d8-dffa-4d17-b9ed-b0ea5119dc95}</MetaDataID>
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        /// <summary>
        /// Specifies the x-coordinate of the point
        /// </summary>
        public int x;

        /// <summary>
        /// Specifies the y-coordinate of the point
        /// </summary>
        public int y;


        /// <summary>
        /// Creates a new RECT struct with the specified x and y coordinates
        /// </summary>
        /// <param name="x">The x-coordinate of the point</param>
        /// <param name="y">The y-coordinate of the point</param>
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        /// <summary>
        /// Creates a new POINT struct from the specified Point
        /// </summary>
        /// <param name="p">The Point to create the POINT from</param>
        /// <returns>A POINT struct with the same x and y coordinates as 
        /// the specified Point</returns>
        public static POINT FromPoint(Point p)
        {
            return new POINT(p.X, p.Y);
        }


        /// <summary>
        /// Returns a Point with the same x and y coordinates as the POINT
        /// </summary>
        /// <returns>A Point with the same x and y coordinates as the POINT</returns>
        public Point ToPoint()
        {
            return new Point(this.x, this.y);
        }
    }
}
