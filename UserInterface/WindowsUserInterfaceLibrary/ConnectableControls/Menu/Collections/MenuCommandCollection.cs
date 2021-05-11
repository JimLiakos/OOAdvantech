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
using System.Windows.Forms;
using ConnectableControls.Menus.Collections;
using ConnectableControls.Menus;

namespace ConnectableControls.Menus.Collections
{
	/// <MetaDataID>{94EBA4A0-D016-458A-871D-09550B19A9A9}</MetaDataID>
    public class MenuCommandCollection : CollectionWithEvents
    {
        // Instance fields
        /// <MetaDataID>{7b9c4bfe-6730-477c-b8da-cd34c9dc47d3}</MetaDataID>
        protected string _extraText;
        /// <MetaDataID>{1b13b1fb-ca59-488f-95c3-8b1c2aa6d9a0}</MetaDataID>
        protected Font _extraFont;
        /// <MetaDataID>{d776afe5-e4ac-4bf8-b7c6-91de60161748}</MetaDataID>
        protected Color _extraTextColor;
        /// <MetaDataID>{6a8b462f-83d3-4368-b64e-3601433ba617}</MetaDataID>
        protected Brush _extraTextBrush;
        /// <MetaDataID>{4c01157a-56df-4d0e-a3b0-eac084af72ba}</MetaDataID>
        protected Color _extraBackColor;
        /// <MetaDataID>{077a7855-548c-4f34-b5a2-15907a221b37}</MetaDataID>
        protected Brush _extraBackBrush;
        /// <MetaDataID>{bd52e128-6215-403c-8829-1b7dc310e108}</MetaDataID>
        protected bool _showInfrequent;

		/// <MetaDataID>{A6668C6A-C1BC-4314-96B7-B56789B5A3B3}</MetaDataID>
        public MenuCommandCollection()
        {
            // Define defaults for internal state
            _extraText = "";
            _extraFont = SystemInformation.MenuFont;
            _extraTextColor = SystemColors.ActiveCaptionText;
            _extraTextBrush = null;
            _extraBackColor = SystemColors.ActiveCaption;
            _extraBackBrush = null;
            _showInfrequent = false;
        }
        /// <MetaDataID>{0edb328d-e834-4c1f-b61a-2c5651f7c34f}</MetaDataID>
        public MenuCommandCollection(MenuCommandCollection copyMenuCommands)
        {
            // Define defaults for internal state
            _extraText =copyMenuCommands._extraText;
            _extraFont = copyMenuCommands._extraFont;
            _extraTextColor = copyMenuCommands._extraTextColor;
            _extraTextBrush = copyMenuCommands._extraTextBrush;
            _extraBackColor = copyMenuCommands._extraBackColor;
            _extraBackBrush = copyMenuCommands._extraBackBrush;
            _showInfrequent = copyMenuCommands._showInfrequent;
            foreach(MenuCommand menuCommand in copyMenuCommands)
                Add(menuCommand);

            
        }


		/// <MetaDataID>{6A77345F-BF94-4D06-B3FA-C7B4EA541D23}</MetaDataID>
        public MenuCommand Add(MenuCommand value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <MetaDataID>{B9A11DD7-CF61-4A60-B7AA-6DAD20F63F26}</MetaDataID>
        public void AddRange(MenuCommand[] values)
        {

            // Use existing method to add each array entry
            foreach(MenuCommand page in values)
                Add(page);
        }

		/// <MetaDataID>{8725A7BE-EB29-4199-A1E7-E590F4D14D3A}</MetaDataID>
        public void Remove(MenuCommand value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <MetaDataID>{67569DC2-EF01-47A6-B697-C17BD8D0F32A}</MetaDataID>
        public void Insert(int index, MenuCommand value)
        {
            // Use base class to process actual collection operation

            base.List.Insert(index, value as object);
        }

		/// <MetaDataID>{B7EE90C7-2583-4397-9AAD-8E693A68346F}</MetaDataID>
        public bool Contains(MenuCommand value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        /// <MetaDataID>{6a62de7b-f160-48d3-9af0-cc77960b77b0}</MetaDataID>
        public MenuCommand this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as MenuCommand); }
        }

        /// <MetaDataID>{c40bd434-aa3a-418c-951c-de770e0c1440}</MetaDataID>
        public MenuCommand this[string text]
        {
            get 
            {
                // Search for a MenuCommand with a matching title
                foreach(MenuCommand mc in base.List)
                    if (mc.Text == text)
                        return mc;

                return null;
            }
        }

		/// <MetaDataID>{094A8CEB-0CBE-45E5-AF1C-F1A507491072}</MetaDataID>
        public int IndexOf(MenuCommand value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		/// <MetaDataID>{87F8FC14-2C36-4575-B23B-C82C4E3E342E}</MetaDataID>
        public bool VisibleItems()
        {
            foreach(MenuCommand mc in base.List)
            {
                // Is the item visible?
                if (mc.Visible)
                {
                    // And its not a separator...
                    if (mc.Text != "-")
                    {
                        // Then should return 'true' except when we are a sub menu item ourself
                        // in which case there might not be any visible children which means that
                        // this item would not be visible either.
                        if ((mc.MenuCommands.Count > 0) && (!mc.MenuCommands.VisibleItems()))
                            continue;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <MetaDataID>{f4e119fb-d275-47a7-af28-7b27e1a9f1fd}</MetaDataID>
        public string ExtraText
        {
            get { return _extraText; }
            set { _extraText = value; }
        }

        /// <MetaDataID>{57268ef4-1f07-4127-b883-8b37acdac179}</MetaDataID>
        public Font ExtraFont
        {
            get { return _extraFont; }
            set { _extraFont = value; }
        }

        /// <MetaDataID>{4cd2a576-6577-4978-8596-3bdeaae94d3d}</MetaDataID>
        public Color ExtraTextColor
        {
            get { return _extraTextColor; }
            set { _extraTextColor = value; }
        }

        /// <MetaDataID>{f8ea3b42-750b-4534-ae11-a675fa134b9f}</MetaDataID>
        public Brush ExtraTextBrush
        {
            get { return _extraTextBrush; }
            set { _extraTextBrush = value; }
        }

        /// <MetaDataID>{db18d39e-bcb2-4333-8dea-855600e87d55}</MetaDataID>
        public Color ExtraBackColor
        {
            get { return _extraBackColor; }
            set { _extraBackColor = value; }
        }

        /// <MetaDataID>{5c989fc1-ef65-45f2-b408-63ed00584288}</MetaDataID>
        public Brush ExtraBackBrush
        {
            get { return _extraBackBrush; }
            set { _extraBackBrush = value; }
        }

        /// <MetaDataID>{87b832b1-dab6-437f-bfb0-e661d5487f8b}</MetaDataID>
        public bool ShowInfrequent
        {
            get { return _showInfrequent; }
            set { _showInfrequent = value; }
        }
    }
}
