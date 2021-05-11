/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Win32;


namespace ConnectableControls
{
    /// <summary>
    /// Summary description for DropDownContainer.
    /// </summary>
    /// <MetaDataID>{D95F59B0-5934-41CB-8DBE-6ABC4FF2642F}</MetaDataID>
    [ToolboxItem(false)]
    public class DropDownContainer : System.Windows.Forms.Form
    {
        /// <MetaDataID>{501A7DB8-AFD1-4840-A826-952541A9DBCA}</MetaDataID>
        public class ListBoxItem
        {
            /// <MetaDataID>{AE640E44-D040-47B8-AC5E-354387384BF2}</MetaDataID>
            public readonly object DisplayiedObject;

            string _Name;
            /// <MetaDataID>{3EC8E6A9-3AA9-47DD-A563-3A61775DB164}</MetaDataID>
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    _Name = value;
                }
            }
            /// <exclude>Excluded</exclude>
            object _PresentationObect;
            public object PresentationObect
            {
                get
                {
                    if (_PresentationObect != null)
                        return _PresentationObect;
                    else
                        return DisplayiedObject;

                }
            }


            /// <MetaDataID>{42CCA917-F4A2-4610-B89A-7301222D6CFF}</MetaDataID>
            public ListBoxItem(object obj,object presentationObect, string name)
            {
                _PresentationObect = presentationObect;
                DisplayiedObject = obj;
                Name = name;
            }
            /// <MetaDataID>{D5B1D1E7-C780-482C-87B9-0ADEAF2F65C4}</MetaDataID>
            public override string ToString()
            {
                if (Name == null)
                    return "";
                else
                    return Name;
            }
        }

        public class ListBoxNoneItem : ListBoxItem
        {

            public ListBoxNoneItem(string noneName)
                : base(null,null, noneName)
            {

            }

        }






        /// <MetaDataID>{4C839756-15CF-459B-9357-A63AD0B94756}</MetaDataID>
        public override bool Focused
        {
            get
            {
                return base.Focused | listBox.Focused;
            }
        }

        #region Class Data

        ///// <summary>
        ///// The DropDownCellEditor that owns the DropDownContainer
        ///// </summary>
        //private DropDownCellEditor editor;

        /// <summary>
        /// The Control displayed in the DropDownContainer
        /// </summary>
        /// <MetaDataID>{0F9FE24E-4052-4B1B-B80F-352D362B19D8}</MetaDataID>
        private Control dropdownControl;
        /// <MetaDataID>{19722AAB-15FA-4D0D-A20A-9E6E80DD7A10}</MetaDataID>
        private ListBox listBox;

        /// <summary>
        /// A Panel that provides the black border around the DropDownContainer
        /// </summary>
        /// <MetaDataID>{9A39DAA0-DDE3-48D1-90FB-9A8D7D8AA20F}</MetaDataID>
        private Panel panel;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DropDownContainer class with the 
        /// specified DropDownCellEditor owner
        /// </summary>
        /// <MetaDataID>{89E5F8A9-FBA5-466B-913C-CA19A20E0AC2}</MetaDataID>
        public DropDownContainer()//(DropDownCellEditor editor) : base()
        {
            InitializeComponent();
            //if (editor == null)
            //{
            //    throw new ArgumentNullException("editor", "DropDownCellEditor cannot be null");
            //}

            //this.editor = editor;

            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.TabStop = false;
            this.TopMost = true;

            this.dropdownControl = null;
            listBox.Location = new Point(0, 0);
            listBox.LostFocus += new EventHandler(listBox_LostFocus);
            listBox.SizeChanged += new EventHandler(listBox_SizeChanged);
            listBox.KeyUp += new KeyEventHandler(listBox_KeyUp);
            listBox.MouseClick += new MouseEventHandler(listBox_MouseClick);

            // listBox.Size = Size;


            //this.panel = new Panel();
            //this.panel.AutoScroll = false;
            //this.panel.BorderStyle = BorderStyle.None;
            //this.panel.Size = this.Size;
            ////this.Controls.Add(this.panel);
            //this.SizeChanged += new EventHandler(DropDownContainer_SizeChanged);
        }

        #endregion



        #region Methods


        /// <MetaDataID>{FC844BE4-7DAA-4C5E-89E2-74B55A1C0696}</MetaDataID>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            listBox.Width = Width;
        }
        /// <MetaDataID>{514AD8A9-2045-4BEF-9C89-5F64B7A48364}</MetaDataID>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                listBox.Font = Font;
            }
        }
        public event EventHandler ItemSelected;

        /// <MetaDataID>{F915424C-9042-4057-85B6-8714AB6B885B}</MetaDataID>
        public new Size Size
        {
            get
            {
                if (_Height == 0)
                {
                    listBox.Height = (listBox.Items.Count + 2) * listBox.ItemHeight;
                    base.Size = listBox.Size;
                }
                return base.Size;

            }
            set
            {
                base.Size = value;
            }
        }
        /// <MetaDataID>{51532E95-9D1D-485D-868A-4CC3EF10F89B}</MetaDataID>
        public int _Height = 0;
        /// <MetaDataID>{8C675433-4529-4068-AED8-BB206E3EE662}</MetaDataID>
        public new int Height
        {
            get
            {
                if (_Height == 0)
                {
                    listBox.Height = (listBox.Items.Count + 2) * listBox.ItemHeight;
                    base.Size = listBox.Size;
                    return base.Height;
                }
                else
                    return _Height;

            }
            set
            {
                base.Height = value;
                listBox.Height = value;
                _Height = value;
            }
        }



        /// <summary>
        /// Displays the DropDownContainer to the user
        /// </summary>
        /// <MetaDataID>{08865798-6519-4EF5-B117-C604470568D2}</MetaDataID>
        public void ShowDropDown()
        {



            this.FlushPaintMessages();
            SuspendLayout();
            if (_Height == 0)
            {
                listBox.Height = (listBox.Items.Count + 2) * listBox.ItemHeight;
                Size = listBox.Size;
            }


            listBox.Location = new Point(0, 0);

            this.Show();
            listBox.Focus();
            Size = listBox.Size;

            listBox.SelectedItem = SelectedObject;



            //ResumeLayout();
        }
        /// <MetaDataID>{ABF7154E-FC4D-48FB-8B6E-57DE9FAFF00B}</MetaDataID>
        public object SelectedObject;

        /// <MetaDataID>{DBC519C9-897B-43E2-B053-A1AE332CE5EC}</MetaDataID>
        void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox.SelectedIndices.Count > 0)
            {
                SelectedObject = listBox.Items[listBox.SelectedIndices[0]];
                Hide();
                listBox.Items.Clear();

                if (ItemSelected != null)
                    ItemSelected(this, null);

            }


        }

        /// <MetaDataID>{BCF769F8-F441-40DE-BF22-E20903DB1F23}</MetaDataID>
        void listBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && listBox.SelectedIndices.Count > 0)
            {
                SelectedObject = listBox.Items[listBox.SelectedIndices[0]];
                Hide();
                listBox.Items.Clear();

                if (ItemSelected != null)
                    ItemSelected(this, null);

            }
            if (e.KeyData == Keys.Escape )
            {
                if(listBox.SelectedIndices.Count>0)
                    SelectedObject = listBox.Items[listBox.SelectedIndices[0]];
                Hide();
                listBox.Items.Clear();

            }

        }

        /// <MetaDataID>{9C0938B7-90BF-4223-8D21-9175B7788108}</MetaDataID>
        void listBox_SizeChanged(object sender, EventArgs e)
        {
            Size = listBox.Size;


        }

        /// <MetaDataID>{2ED9D550-BD94-4A5D-B2BE-59DBEEABD7E4}</MetaDataID>
        void listBox_LostFocus(object sender, EventArgs e)
        {
            Hide();
            listBox.Items.Clear();

        }



        /// <summary>
        /// Hides the DropDownContainer from the user
        /// </summary>
        /// <MetaDataID>{84C6D2CD-08E4-403D-9E8B-564E9F6FA87A}</MetaDataID>
        public void HideDropDown()
        {
            this.FlushPaintMessages();

            this.Hide();
        }


        /// <summary>
        /// Processes any Paint messages in the message queue
        /// </summary>
        /// <MetaDataID>{E6B1AF3B-CC67-4153-ACB6-7D45BD506793}</MetaDataID>
        private void FlushPaintMessages()
        {
            MSG msg = new MSG();

            while (NativeMethods.PeekMessage(ref msg, IntPtr.Zero, (int)WindowMessage.WM_PAINT, (int)WindowMessage.WM_PAINT, 1 /*PM_REMOVE*/))
            {
                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessage(ref msg);
            }
        }

        #endregion


        #region Properties

        ///// <summary>
        ///// Gets or sets the Control displayed in the DropDownContainer
        ///// </summary>
        //public Control Control
        //{
        //    get
        //    {
        //        return this.dropdownControl;
        //    }

        //    set
        //    {
        //        if (value != this.dropdownControl)
        //        {
        //            this.panel.Controls.Clear();

        //            this.dropdownControl = value;
        //            value.SizeChanged +=new EventHandler(value_SizeChanged);

        //            if (value != null)
        //            {
        //                //this.Controls.Add(value);
        //            }
        //        }
        //    }
        //}

        /// <MetaDataID>{F195DB9B-6C19-40C4-B081-C7EE73B281AA}</MetaDataID>
        void value_SizeChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Gets the required creation parameters when the control handle is created
        /// </summary>
        /// <MetaDataID>{DFA99ACF-63AE-4A8E-B1D7-499DCCEB68E3}</MetaDataID>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cparams = base.CreateParams;

                cparams.ExStyle |= (int)WindowExtendedStyles.WS_EX_TOOLWINDOW;

                if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major > 5)
                {
                    cparams.ExStyle |= (int)WindowExtendedStyles.WS_EX_NOACTIVATE;
                }

                cparams.ClassStyle |= 0x800 /*CS_SAVEBITS*/;

                return cparams;
            }
        }


        /// <summary>
        /// Handler for the DropDownContainer's SizeChanged event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{63B3332F-31F4-4E09-8864-0AF84575BC14}</MetaDataID>
        private void DropDownContainer_SizeChanged(object sender, EventArgs e)
        {
            this.panel.Size = this.Size;
        }

        #endregion

        /// <MetaDataID>{35B63F77-91E1-4E26-983B-9B2FDA9E0180}</MetaDataID>
        private void InitializeComponent()
        {
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(-2, -2);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(296, 303);
            this.listBox.TabIndex = 0;
            // 
            // DropDownContainer
            // 
            this.ClientSize = new System.Drawing.Size(292, 313);
            this.Controls.Add(this.listBox);
            this.Name = "DropDownContainer";
            this.ResumeLayout(false);

        }

        /// <MetaDataID>{C9B93CC0-F738-4B8E-9970-D0414ABD872B}</MetaDataID>
        internal ListBox.ObjectCollection Items
        {
            get
            {
                return listBox.Items;
            }

        }
    }
}
