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
using System.Text;
using System.Drawing;

namespace ConnectableControls.Menus.Common
{
	/// <MetaDataID>{F6F279F8-8919-48AF-BB98-E415CF4FDE21}</MetaDataID>
    public class ConversionHelper
    {
		// Faster performance to cache the converters and type objects, rather
		// than keep recreating them each time a conversion is required
		protected static SizeConverter _sc = new SizeConverter();
		protected static PointConverter _pc = new PointConverter();
		protected static Type _stringType = Type.GetType("System.String");

		/// <MetaDataID>{E99E2DC3-1927-4E36-AD58-1D2485CC01E1}</MetaDataID>
		public static string SizeToString(Size size)
		{
			return (string)_sc.ConvertTo(size, _stringType);
		}

		/// <MetaDataID>{31CF5052-BEA4-466F-8249-4C20B40FC559}</MetaDataID>
		public static Size StringToSize(string str)
		{
			return (Size)_sc.ConvertFrom(str);
		}

		/// <MetaDataID>{C475EE42-61AD-4112-8C99-E8970846FA2C}</MetaDataID>
		public static string PointToString(Point point)
		{
			return (string)_pc.ConvertTo(point, _stringType);
		}

		/// <MetaDataID>{DD3E8CD4-E446-4445-93D7-EBB50D4EAB45}</MetaDataID>
		public static Point StringToPoint(string str)
		{
			return (Point)_pc.ConvertFrom(str);
		}
    }
}