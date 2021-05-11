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
	/// <MetaDataID>{44E178D5-8D4B-4B8A-8B0E-FD5C2D945BB8}</MetaDataID>
    public class ControlHelper
	{
		/// <MetaDataID>{3814C92E-C735-4F1D-8FAA-7BB8B4DE922C}</MetaDataID>
		public static void RemoveAll(Control control)
		{
			if ((control != null) && (control.Controls.Count > 0))
			{
                Button tempButton = null;
                Form parentForm = control.FindForm();
                
				if (parentForm != null)
				{
					// Create a hidden, temporary button
					tempButton = new Button();
					tempButton.Visible = false;

					// Add this temporary button to the parent form
					parentForm.Controls.Add(tempButton);

					// Must ensure that temp button is the active one
					parentForm.ActiveControl = tempButton;
                }

  				// Remove all entries from target
				control.Controls.Clear();

                if (parentForm != null)
                {
                    // Remove the temporary button
					tempButton.Dispose();
					parentForm.Controls.Remove(tempButton);
				}
			}
		}

		/// <MetaDataID>{23DF00C0-748D-4456-ABCB-D92F1506BC76}</MetaDataID>
		public static void Remove(Control.ControlCollection coll, Control item)
		{
			if ((coll != null) && (item != null))
			{
                Button tempButton = null;
                Form parentForm = item.FindForm();

				if (parentForm != null)
				{
					// Create a hidden, temporary button
					tempButton = new Button();
					tempButton.Visible = false;

					// Add this temporary button to the parent form
					parentForm.Controls.Add(tempButton);

					// Must ensure that temp button is the active one
					parentForm.ActiveControl = tempButton;
                }
                
				// Remove our target control
				coll.Remove(item);

                if (parentForm != null)
                {
                    // Remove the temporary button
					tempButton.Dispose();
					parentForm.Controls.Remove(tempButton);
				}
			}
		}

		/// <MetaDataID>{5F66DA91-04BE-4479-8ACF-481CED56C604}</MetaDataID>
		public static void RemoveAt(Control.ControlCollection coll, int index)
		{
			if (coll != null)
			{
				if ((index >= 0) && (index < coll.Count))
				{
					Remove(coll, coll[index]);
				}
			}
		}
    
		/// <MetaDataID>{48F1438C-CD20-4143-9FB4-EEE642128605}</MetaDataID>
        public static void RemoveForm(Control source, Form form)
        {
            ContainerControl container = source.FindForm() as ContainerControl;
            
            if (container == null)
                container = source as ContainerControl;

            Button tempButton = new Button();
            tempButton.Visible = false;

            // Add this temporary button to the parent form
            container.Controls.Add(tempButton);

            // Must ensure that temp button is the active one
            container.ActiveControl = tempButton;

            // Remove Form parent
            form.Parent = null;
            
            // Remove the temporary button
            tempButton.Dispose();
            container.Controls.Remove(tempButton);
        }
    }
}