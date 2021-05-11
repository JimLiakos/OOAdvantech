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
using System.Windows.Forms;

namespace ConnectableControls.Menus.Common
{ 
	/// <MetaDataID>{21EC60F5-7407-403C-A5C5-087E27B6DF18}</MetaDataID>
    public class DebugHelper
    {
		/// <MetaDataID>{17BFE16A-7C34-47D4-A5E7-B876AA32130B}</MetaDataID>
        public static void ListControls(Control.ControlCollection controls)
        {
			ListControls("Control Collection", controls, false);
		}

		/// <MetaDataID>{E0883D89-4333-48B8-BA96-48CE20088BD8}</MetaDataID>
        public static void ListControls(string title, Control.ControlCollection controls)
        {
			ListControls(title, controls, false);
		}

		/// <MetaDataID>{21A125B0-9292-44C1-88BF-6B68B4952D2D}</MetaDataID>
        public static void ListControls(string title, Control.ControlCollection controls, bool fullName)
        {
			// Output title first
			Console.WriteLine("\n{0}", title);

			// Find number of controls in the collection
			int count = controls.Count;

			// Output details for each 
			for(int index=0; index<count; index++)
			{
				Control c = controls[index];

				string typeName;
				
				if (fullName)
					typeName = c.GetType().FullName;
				else
					typeName = c.GetType().Name;

				Console.WriteLine("{0} V:{1} T:{2} N:{3}", index, c.Visible, typeName, c.Name);
			}
        }
    }
}


