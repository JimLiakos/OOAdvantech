using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIBaseEx
{
    /// <MetaDataID>{43d3ff08-79a8-4faf-bbd6-43f6885dabab}</MetaDataID>
    public class SizeUtil
    {
        static double DPI=0;
        //new System.Windows.LengthConverter().ConvertFromString("1in")
        public static double PixelTomm(double px)
        {
            var pixpermm = (double)DPI / 25.4;
            return px / pixpermm;
        }


        public static double mmToPixel(double mm)
        {
            var pixpermm = (double) DPI/ 25.4;
            return mm * pixpermm;
        }

        public static double PixelTocm(double px)
        {
            return PixelTomm(px) / 10;
        }
        public static double cmToPixel(double cm)
        {
            var pixpermm = (double)DPI / 25.4;
            return cm * pixpermm*10;
        }

        public static double PixelToInch(double px)
        {
            return px / DPI;


        }


        public static double InchToPixel(double inch)
        {
            return inch * DPI;
        }
    }
}
