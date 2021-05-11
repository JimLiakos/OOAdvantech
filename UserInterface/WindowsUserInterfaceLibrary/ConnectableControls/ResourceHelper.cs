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
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ConnectableControls
{
	/// <MetaDataID>{6D84C10E-4D46-4C73-8A4C-F1DA4233B817}</MetaDataID>
    public class ResourceHelper
    {
		/// <MetaDataID>{5AEE2ABB-D04F-4316-8E88-2751E68D7D35}</MetaDataID>
        public static Cursor LoadCursor(Type assemblyType, string cursorName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream iconStream = myAssembly.GetManifestResourceStream(cursorName);

            // Load the Icon from the stream
            return new Cursor(iconStream);
        }
    
		/// <MetaDataID>{B90FFD09-BE29-4AE4-A623-74F498966A18}</MetaDataID>
        public static Icon LoadIcon(Type assemblyType, string iconName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream iconStream = myAssembly.GetManifestResourceStream(iconName);

            // Load the Icon from the stream
            return new Icon(iconStream);
        }

		/// <MetaDataID>{5A3E9870-4A9A-43B2-B302-B56C89544AB4}</MetaDataID>
        public static Icon LoadIcon(Type assemblyType, string iconName, Size iconSize)
        {
            // Load the entire Icon requested (may include several different Icon sizes)
            Icon rawIcon = LoadIcon(assemblyType, iconName);
			
            // Create and return a new Icon that only contains the requested size
            return new Icon(rawIcon, iconSize); 
        }

		/// <MetaDataID>{BD5C6C41-C89B-4C0F-9BA8-0ED87B69A826}</MetaDataID>
        public static Bitmap LoadBitmap(Type assemblyType, string imageName)
        {
            return LoadBitmap(assemblyType, imageName, false, new Point(0,0));
        }

		/// <MetaDataID>{80889F64-8A1E-4EE4-9D03-46647C2970EC}</MetaDataID>
        public static Bitmap LoadBitmap(Type assemblyType, string imageName, Point transparentPixel)
        {
            return LoadBitmap(assemblyType, imageName, true, transparentPixel);
        }

		/// <MetaDataID>{7A517D27-72C7-4FB2-B764-FF2098AA83D8}</MetaDataID>
        public static ImageList LoadBitmapStrip(Type assemblyType, string imageName, Size imageSize)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, false, new Point(0,0));
        }

		/// <MetaDataID>{5EFB6CE5-8F75-47EF-A493-927B53F4A080}</MetaDataID>
        public static ImageList LoadBitmapStrip(Type assemblyType, 
                                                string imageName, 
                                                Size imageSize,
                                                Point transparentPixel)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, true, transparentPixel);
        }

		/// <MetaDataID>{96FD8822-0534-4C5A-AC68-4DE678A8DECD}</MetaDataID>
        protected static Bitmap LoadBitmap(Type assemblyType, 
                                           string imageName, 
                                           bool makeTransparent, 
                                           Point transparentPixel)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap from stream
            Bitmap image = new Bitmap(imageStream);

            if (makeTransparent)
            {
                Color backColor = image.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                image.MakeTransparent(backColor);
            }
			    
            return image;
        }

		/// <MetaDataID>{497E2A74-F28F-4568-B42A-1C7A6F438D97}</MetaDataID>
        protected static ImageList LoadBitmapStrip(Type assemblyType, 
                                                   string imageName, 
                                                   Size imageSize,
                                                   bool makeTransparent,
                                                   Point transparentPixel)
        {
            // Create storage for bitmap strip
            ImageList images = new ImageList();

            // Define the size of images we supply
            images.ImageSize = imageSize;

            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap strip from resource
            Bitmap pics = new Bitmap(imageStream);

            if (makeTransparent)
            {
                Color backColor = pics.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                pics.MakeTransparent(backColor);
            }
			    
            // Load them all !
            images.Images.AddStrip(pics);

            return images;
        }
    }
}