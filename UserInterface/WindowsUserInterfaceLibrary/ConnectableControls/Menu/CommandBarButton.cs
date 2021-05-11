using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.Menus.Collections;


namespace ConnectableControls.Menus
{
    
	/// <summary>
	/// </summary>
	/// <MetaDataID>{601BC6D5-F6D5-412D-B151-3227F9683A83}</MetaDataID>
	public class CommandBarButton:MenuCommand
	{
		/// <MetaDataID>{79D4DB73-9169-4DCB-ACB4-94BE33CC3DFA}</MetaDataID>
		public CommandBarButton ( ImageList imageList, int imageIndex):
			base("", imageList, imageIndex)
		{
            
			
			// 
			// TODO: Add constructor logic here
			//
		}
		/// <MetaDataID>{F2026037-B992-4982-AC2F-C55DC014358A}</MetaDataID>
		public CommandBarButton ( ImageList imageList, int imageIndex,EventHandler clickHandler):
			base("", imageList, imageIndex,clickHandler)
		{
		}

		
	}
}
