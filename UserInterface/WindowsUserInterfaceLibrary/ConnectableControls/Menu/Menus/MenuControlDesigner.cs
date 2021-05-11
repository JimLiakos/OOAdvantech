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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
//using ConnectableControls.Menus.Controls;

namespace ConnectableControls.Menus
{
	/// <MetaDataID>{4FD5B2C1-144D-46BC-904C-E0D4B7F9117D}</MetaDataID>
    public class MenuControlDesigner :  System.Windows.Forms.Design.ParentControlDesigner
    {
        public override ICollection AssociatedComponents
        {
            get 
            {
				MessageBox.Show("menuCommands = new MenuCommandCollection();");
                if (base.Control is ConnectableControls.Menus.MenuControl)
                    return ((ConnectableControls.Menus.MenuControl)base.Control).MenuCommands;
                else
                    return base.AssociatedComponents;
            }
        }

        protected override bool DrawGrid
        {
            get { return false; }
        }
    }
}
