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
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller ;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Cell that is displayed in a Table
    /// </summary>
    /// <MetaDataID>{9FB45B0D-E6D6-49DE-97A1-62046A625666}</MetaDataID>
    [DesignTimeVisible(true),
    TypeConverter(typeof(CellConverter))]
    public class Cell : IDisposable, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        /// <MetaDataID>{32745348-2ff3-4eb6-87d0-13722855c22d}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                //if (Column is TextColumn)
                //    return true;
                //else
                    return false;
            }
        }
        /// <MetaDataID>{0771cfcf-315c-472a-940f-52d6228ad268}</MetaDataID>
        public void LockStateChange(object sender)
        {

        }
        /// <MetaDataID>{403a5e0a-de87-4955-a97e-68360f407072}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return Column.UserInterfaceObjectConnection;
            }
        }





        /// <MetaDataID>{88ffdbe7-02b8-4bd2-998d-5a7ff31c0d37}</MetaDataID>
        internal object DisplayedValue;
        /// <MetaDataID>{2e6a13be-7fb7-480e-a8fe-36dd4114f935}</MetaDataID>
        internal object DisplayedObjectMember;
   

        /// <MetaDataID>{D4DADE17-B343-4F90-BCF5-0F219104C26E}</MetaDataID>
        public void DisplayedValueChanged(object sender,OOAdvantech.UserInterface.Runtime.MemberChangeEventArg  memberChangeEventArg)
        {
            try
            {
                object displayedValue = null; ;
                object displayedObjectMember = null;

                Column.UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;

                if (!string.IsNullOrEmpty(this.Column.DisplayMember as string))
                {
                    displayedValue = Column.UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Column.Owner.ListConnection.PresentationObjectType, Column.Path as string, this, out returnValueAsCollection);
                    displayedObjectMember = Column.UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Column.Owner.ListConnection.PresentationObjectType, Column.Path + "." + Column.DisplayMember as string, this, out returnValueAsCollection);

                }
                else
                    displayedValue = Column.UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Column.Owner.ListConnection.PresentationObjectType, Column.Path as string, this, out returnValueAsCollection);


                DisplayedValue = displayedValue;
                DisplayedObjectMember = displayedObjectMember;

                // DataOwnerObject = DisplayedValue.OwnerObject;
                Text = "";
                Data = DisplayedValue;
                if (DisplayedObjectMember == null)
                    if (DisplayedValue!= null)
                        Text = DisplayedValue.ToString();

                if (DisplayedObjectMember != null && DisplayedObjectMember != null)
                    Text = DisplayedObjectMember.ToString();
                if (Column is CheckBoxColumn && data is bool && (bool)data)
                    CheckState = CheckState.Checked;
                else
                    if ((Column is CheckBoxColumn) && (data == null || !(bool)data))
                        CheckState = CheckState.Unchecked;
            }
            catch (System.Exception error)
            {
                throw;
            }

                


            return;


            //if (DisplayedValue == sender)
            //{
            //    DataOwnerObject = DisplayedValue.OwnerObject;
            //    data = DisplayedValue.Value;
            //    if (DisplayedObjectMember == null)
            //        if (DisplayedValue.Value != null)
            //            Text = DisplayedValue.Value.ToString();
            //}
            //if (DisplayedObjectMember == sender)
            //{
            //    data = DisplayedObjectMember.OwnerObject;
            //    if (DisplayedObjectMember.Value != null)
            //        Text = DisplayedObjectMember.Value.ToString();
            //}
            //string memeber = (Column.Path as string).Substring((Column.Path as string).LastIndexOf(".") + 1);
            //string path = null;
            //if ((Column.Path as string).LastIndexOf(".") >= 0)
            //    path = (Column.Path as string).Substring(0, (Column.Path as string).LastIndexOf("."));
            //else
            //    path = "";
            //System.Type type = ConnectableControls.ViewControlObject.GetType(Column.ColumnModel.Table.CollectionObjectType, path);
            //ConnectableControls.DisplayedValue displayedValue=Column.ViewControlObject.GetValue(DataOwnerObject, type, memeber);
            //data = displayedValue.Value;
            //if (!string.IsNullOrEmpty(Column.DisplayMember as string) && data != null)
            //{

            //    displayedValue = Column.ViewControlObject.GetValue(data, Column.ValueType, Column.DisplayMember as string);
            //    if (displayedValue.Value == null)
            //        Text = "";
            //    else
            //        Text = displayedValue.Value.ToString();
            //}
            //else
            //{
            //    if (displayedValue.Value == null)
            //        Text = "";
            //    else
            //        Text = displayedValue.Value.ToString();
            //}


           // ViewControlObject.SetValue(obj, value, type, memeber);

            

        }

        #region EventHandlers

        /// <summary>
        /// Occurs when the value of a Cells property changes
        /// </summary>
        public event CellEventHandler PropertyChanged;

        #endregion


        #region Class Data

        // Cell state flags
        /// <MetaDataID>{146180aa-be20-4b09-893e-3661aa929f1e}</MetaDataID>
        private static readonly int STATE_EDITABLE = 1;
        /// <MetaDataID>{e97a938a-4180-4826-a97b-ffc91c8cab8f}</MetaDataID>
        private static readonly int STATE_ENABLED = 2;
        /// <MetaDataID>{221925c7-2e52-493f-aab8-8dccda2a4a20}</MetaDataID>
        private static readonly int STATE_SELECTED = 4;

        /// <summary>
        /// The text displayed in the Cell
        /// </summary>
        /// <MetaDataID>{120eadd4-2e02-45e7-840a-753ff0827baf}</MetaDataID>
        private string text;

        /// <summary>
        /// An object that contains data to be displayed in the Cell
        /// </summary>
        /// <MetaDataID>{9a4d0358-2e6e-4900-89d9-78a277923bd7}</MetaDataID>
        private object data;

        /// <summary>
        /// An object that contains data about the Cell
        /// </summary>
        /// <MetaDataID>{af3cb90d-9124-42be-9578-e6798064b922}</MetaDataID>
        private object tag;

        /// <summary>
        /// Stores information used by CellRenderers to record the current 
        /// state of the Cell
        /// </summary>
        /// <MetaDataID>{f3d56e75-fe86-40e3-97ba-98ba3383e907}</MetaDataID>
        private object rendererData;

        /// <summary>
        /// The Row that the Cell belongs to
        /// </summary>
        /// <MetaDataID>{cf484df8-3324-40c1-9b94-a09efb0c14ee}</MetaDataID>
        private Row row;

        /// <summary>
        /// The index of the Cell
        /// </summary>
        /// <MetaDataID>{93cee07e-6b96-47bc-94b4-3e8db9c01705}</MetaDataID>
        private int index;

        /// <summary>
        /// Contains the current state of the the Cell
        /// </summary>
        /// <MetaDataID>{4fe19ec6-c8e3-486d-9558-9b2ab46027d7}</MetaDataID>
        private byte state;

        /// <summary>
        /// The Cells CellStyle settings
        /// </summary>
        /// <MetaDataID>{6cf97a54-a657-4357-8ac5-d6bc62d687d1}</MetaDataID>
        private CellStyle cellStyle;

        /// <summary>
        /// The Cells CellCheckStyle settings
        /// </summary>
        /// <MetaDataID>{4ba069a6-4758-4991-b49d-5cf719113be7}</MetaDataID>
        private CellCheckStyle checkStyle;

        /// <summary>
        /// The Cells CellImageStyle settings
        /// </summary>
        /// <MetaDataID>{99489743-8745-41b8-8109-8ecfe709b82c}</MetaDataID>
        private CellImageStyle imageStyle;

        /// <summary>
        /// The text displayed in the Cells tooltip
        /// </summary>
        /// <MetaDataID>{3f1ad205-cab0-4dc4-99bc-09f65db2c1a1}</MetaDataID>
        private string tooltipText;

        /// <summary>
        /// Specifies whether the Cell has been disposed
        /// </summary>
        /// <MetaDataID>{39866efc-d9bb-4969-b083-4c2bbff670f4}</MetaDataID>
        private bool disposed = false;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Cell class with default settings
        /// </summary>
        /// <MetaDataID>{CBB89EC8-D81B-421E-B4BD-4B95D0A6BBE2}</MetaDataID>
        public Cell()
            : base()
        {
            this.Init();
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <MetaDataID>{395A220B-BD4C-4747-86D8-1A65842A36BC}</MetaDataID>
        public Cell(string text)
        {
            this.Init();

            this.text = text;
        }



        /// <summary>
        /// Initializes a new instance of the Cell class with the specified object
        /// </summary>
        /// <param name="value">The object displayed in the Cell</param>
        /// <MetaDataID>{20B51870-3F73-4F11-A6DF-1D505F26B5B9}</MetaDataID>
        public Cell(object value)
        {
            this.Init();

            this.data = value;
        }
        /// <MetaDataID>{714014c9-6921-4c98-85ca-495bc4d76f0d}</MetaDataID>
        public Column Column;

        /// <MetaDataID>{3E3AE3DF-11D5-4EAE-B658-E51206496B6A}</MetaDataID>
        public Cell( object value,string text, Column column)
        {

            this.Init();
            this.text = text;
            
            Column = column;
            this.data = value;

            this.text = text;
            

        }

        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text 
        /// and object
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="value">The object displayed in the Cell</param>
        /// <MetaDataID>{4FF2F0CB-1F31-4D1B-8F92-C97C699A15F1}</MetaDataID>
        public Cell(string text, object value)
        {
            this.Init();

            this.text = text;
            this.data = value;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text 
        /// and check value
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="check">Specifies whether the Cell is Checked</param>
        /// <MetaDataID>{D18B1387-125C-4FB0-A169-C0B510949236}</MetaDataID>
        public Cell(string text, bool check)
        {
            this.Init();

            this.text = text;
            this.Checked = check;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text 
        /// and Image value
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="image">The Image displayed in the Cell</param>
        /// <MetaDataID>{A9738A85-1416-4327-84AB-BFE765152FC3}</MetaDataID>
        public Cell(string text, Image image)
        {
            this.Init();

            this.text = text;
            this.Image = image;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// fore Color, back Color and Font
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="foreColor">The foreground Color of the Cell</param>
        /// <param name="backColor">The background Color of the Cell</param>
        /// <param name="font">The Font used to draw the text in the Cell</param>
        /// <MetaDataID>{5DA3F20B-15FC-4C65-A07D-232622DFC33A}</MetaDataID>
        public Cell(string text, Color foreColor, Color backColor, Font font)
        {
            this.Init();

            this.text = text;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text 
        /// and CellStyle
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        /// of the Cell</param>
        /// <MetaDataID>{BB00DB62-46FE-42DE-A949-6BBFDF34BC40}</MetaDataID>
        public Cell(string text, CellStyle cellStyle)
        {
            this.Init();

            this.text = text;
            this.cellStyle = cellStyle;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified object, 
        /// fore Color, back Color and Font
        /// </summary>
        /// <param name="value">The object displayed in the Cell</param>
        /// <param name="foreColor">The foreground Color of the Cell</param>
        /// <param name="backColor">The background Color of the Cell</param>
        /// <param name="font">The Font used to draw the text in the Cell</param>
        /// <MetaDataID>{9A4B38F4-685A-47C8-A447-A54412A78092}</MetaDataID>
        public Cell(object value, Color foreColor, Color backColor, Font font)
        {
            this.Init();

            this.data = value;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text 
        /// and CellStyle
        /// </summary>
        /// <param name="value">The object displayed in the Cell</param>
        /// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        /// of the Cell</param>
        /// <MetaDataID>{9E6C3A93-FF00-4E69-837C-C15AA40F87C7}</MetaDataID>
        public Cell(object value, CellStyle cellStyle)
        {
            this.Init();

            this.data = value;
            this.cellStyle = cellStyle;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// object, fore Color, back Color and Font
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="value">The object displayed in the Cell</param>
        /// <param name="foreColor">The foreground Color of the Cell</param>
        /// <param name="backColor">The background Color of the Cell</param>
        /// <param name="font">The Font used to draw the text in the Cell</param>
        /// <MetaDataID>{4972D536-A4F1-48FE-AF4A-0CA10883593A}</MetaDataID>
        public Cell(string text, object value, Color foreColor, Color backColor, Font font)
        {
            this.Init();

            this.text = text;
            this.data = value;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// object and CellStyle
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="value">The object displayed in the Cell</param>
        /// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        /// of the Cell</param>
        /// <MetaDataID>{87C591EF-D1B6-4242-AAA0-09A7E2B01C53}</MetaDataID>
        public Cell(string text, object value, CellStyle cellStyle)
        {
            this.Init();

            this.text = text;
            this.data = value;
            this.cellStyle = cellStyle;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// check value, fore Color, back Color and Font
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="check">Specifies whether the Cell is Checked</param>
        /// <param name="foreColor">The foreground Color of the Cell</param>
        /// <param name="backColor">The background Color of the Cell</param>
        /// <param name="font">The Font used to draw the text in the Cell</param>
        /// <MetaDataID>{0FBBB6E3-9A01-4F14-868D-8F425B464738}</MetaDataID>
        public Cell(string text, bool check, Color foreColor, Color backColor, Font font)
        {
            this.Init();

            this.text = text;
            this.Checked = check;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// check value and CellStyle
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="check">Specifies whether the Cell is Checked</param>
        /// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        /// of the Cell</param>
        /// <MetaDataID>{1B8866D2-C940-4E9F-A6A0-6078B7761C48}</MetaDataID>
        public Cell(string text, bool check, CellStyle cellStyle)
        {
            this.Init();

            this.text = text;
            this.Checked = check;
            this.cellStyle = cellStyle;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// Image, fore Color, back Color and Font
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="image">The Image displayed in the Cell</param>
        /// <param name="foreColor">The foreground Color of the Cell</param>
        /// <param name="backColor">The background Color of the Cell</param>
        /// <param name="font">The Font used to draw the text in the Cell</param>
        /// <MetaDataID>{AC0D597D-68F9-4078-B16E-B4612CD77C82}</MetaDataID>
        public Cell(string text, Image image, Color foreColor, Color backColor, Font font)
        {
            this.Init();

            this.text = text;
            this.Image = image;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
        }


        /// <summary>
        /// Initializes a new instance of the Cell class with the specified text, 
        /// Image and CellStyle
        /// </summary>
        /// <param name="text">The text displayed in the Cell</param>
        /// <param name="image">The Image displayed in the Cell</param>
        /// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        /// of the Cell</param>
        /// <MetaDataID>{13C8F789-22E8-4925-9EE0-B9E89772CFC9}</MetaDataID>
        public Cell(string text, Image image, CellStyle cellStyle)
        {
            this.Init();

            this.text = text;
            this.Image = image;
            this.cellStyle = cellStyle;
        }


        /// <summary>
        /// Initialise default values
        /// </summary>
        /// <MetaDataID>{F7BB2079-7A33-4054-824C-8CE6D0DDB4E9}</MetaDataID>
        private void Init()
        {
            this.text = null;
            this.data = null;
            this.rendererData = null;
            this.tag = null;
            this.row = null;
            this.index = -1;
            this.cellStyle = null;
            this.checkStyle = null;
            this.imageStyle = null;
            this.tooltipText = null;

            this.state = (byte)(STATE_EDITABLE | STATE_ENABLED);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Releases all resources used by the Cell
        /// </summary>
        /// <MetaDataID>{D21CB61E-2451-471C-9E3D-B6652FBC0601}</MetaDataID>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.text = null;
                this.data = null;
                this.tag = null;
                this.rendererData = null;

                if (this.row != null)
                {
                    this.row.Cells.Remove(this);
                }

                this.row = null;
                this.index = -1;
                this.cellStyle = null;
                this.checkStyle = null;
                this.imageStyle = null;
                this.tooltipText = null;

                this.state = (byte)0;

                this.disposed = true;
            }
        }


        /// <summary>
        /// Returns the state represented by the specified state flag
        /// </summary>
        /// <param name="flag">A flag that represents the state to return</param>
        /// <returns>The state represented by the specified state flag</returns>
        /// <MetaDataID>{63551E33-565D-4902-A97D-752FB51E6168}</MetaDataID>
        internal bool GetState(int flag)
        {
            return ((this.state & flag) != 0);
        }


        /// <summary>
        /// Sets the state represented by the specified state flag to the specified value
        /// </summary>
        /// <param name="flag">A flag that represents the state to be set</param>
        /// <param name="value">The new value of the state</param>
        /// <MetaDataID>{CAF1FBB3-26C7-4F68-B437-9FFE0B8D2871}</MetaDataID>
        internal void SetState(int flag, bool value)
        {
            this.state = (byte)(value ? (this.state | flag) : (this.state & ~flag));
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the text displayed by the Cell
        /// </summary>
        /// <MetaDataID>{28e51dd9-51e0-4256-bbc8-2b34aea1c43e}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
		Description("The text displayed by the cell")]
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == null || !this.text.Equals(value))
                {
                    string oldText = this.Text;

                    this.text = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ValueChanged, oldText));
                }
            }
        }


        /// <summary>
        /// Gets or sets the Cells non-text data
        /// </summary>
        /// <MetaDataID>{14e1d400-6f85-499c-b197-e57a676b2560}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
		Description("The non-text data displayed by the cell"),
		TypeConverter(typeof(StringConverter))]
        public object Data
        {
            get
            {
                return this.data;
            }

            set
            {
                if (this.data != value)
                {
                    object oldData = this.Data;

                    this.data = value;
                    if (value is Image)
                        Image = value as Image;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ValueChanged, oldData));
                }
            }
        }


        /// <summary>
        /// Gets or sets the object that contains data about the Cell
        /// </summary>
        /// <MetaDataID>{8e219134-cc85-4b9f-985c-fe2031b5bbe5}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
		Description("User defined data associated with the cell"),
		TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get
            {
                return this.tag;
            }

            set
            {
                this.tag = value;
            }
        }


        /// <summary>
        /// Gets or sets the CellStyle used by the Cell
        /// </summary>
        /// <MetaDataID>{b8eaf17d-708c-4345-8ac7-89f6ff18a3ff}</MetaDataID>
        [Browsable(false),
        DefaultValue(null)]
        public CellStyle CellStyle
        {
            get
            {
                return this.cellStyle;
            }

            set
            {
                if (this.cellStyle != value)
                {
                    CellStyle oldStyle = this.CellStyle;

                    this.cellStyle = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.StyleChanged, oldStyle));
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Cell is selected
        /// </summary>
        /// <MetaDataID>{22642e25-0cd2-4c53-91b8-5336f8053cc4}</MetaDataID>
        [Browsable(false)]
        public bool Selected
        {
            get
            {
                return this.GetState(STATE_SELECTED);
            }
        }


        /// <summary>
        /// Sets whether the Cell is selected
        /// </summary>
        /// <param name="selected">A boolean value that specifies whether the 
        /// cell is selected</param>
        /// <MetaDataID>{9747D0C1-8BFE-4F2C-83FF-9DA63A06115F}</MetaDataID>
        internal void SetSelected(bool selected)
        {
            this.SetState(STATE_SELECTED, selected);
        }


        /// <summary>
        /// Gets or sets the background Color for the Cell
        /// </summary>
        /// <MetaDataID>{9a784ad0-328d-4a35-aefa-498b42c47e26}</MetaDataID>
        [Category("Appearance"),
        Description("The background color used to display text and graphics in the cell")]
        public Color BackColor
        {
            get
            {
                if (this.CellStyle == null)
                {
                    if (this.Row != null)
                    {
                        return this.Row.BackColor;
                    }

                    return Color.Transparent;
                }

                return this.CellStyle.BackColor;
            }

            set
            {
                if (this.CellStyle == null)
                {
                    this.CellStyle = new CellStyle();
                }

                if (this.CellStyle.BackColor != value)
                {
                    Color oldBackColor = this.BackColor;

                    this.CellStyle.BackColor = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.BackColorChanged, oldBackColor));
                }
            }
        }


        /// <summary>
        /// Specifies whether the BackColor property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the BackColor property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{27FA01CD-A46A-4B6A-BF3F-A347B0ED85B4}</MetaDataID>
        private bool ShouldSerializeBackColor()
        {
            return (this.cellStyle != null && this.cellStyle.BackColor != Color.Empty);
        }


        /// <summary>
        /// Gets or sets the foreground Color for the Cell
        /// </summary>
        /// <MetaDataID>{f6b652ce-932f-4cda-ad40-fd7e9ad9e88c}</MetaDataID>
        [Category("Appearance"),
        Description("The foreground color used to display text and graphics in the cell")]
        public Color ForeColor
        {
            get
            {
                if (this.CellStyle == null)
                {
                    if (this.Row != null)
                    {
                        return this.Row.ForeColor;
                    }

                    return Color.Transparent;
                }
                else
                {
                    if (this.CellStyle.ForeColor == Color.Empty || this.CellStyle.ForeColor == Color.Transparent)
                    {
                        if (this.Row != null)
                        {
                            return this.Row.ForeColor;
                        }
                    }

                    return this.CellStyle.ForeColor;
                }
            }

            set
            {
                if (this.CellStyle == null)
                {
                    this.CellStyle = new CellStyle();
                }

                if (this.CellStyle.ForeColor != value)
                {
                    Color oldForeColor = this.ForeColor;

                    this.CellStyle.ForeColor = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ForeColorChanged, oldForeColor));
                }
            }
        }


        /// <summary>
        /// Specifies whether the ForeColor property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the ForeColor property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{05CC825D-0FF0-42A6-9400-0A3C33E5DC20}</MetaDataID>
        private bool ShouldSerializeForeColor()
        {
            return (this.cellStyle != null && this.cellStyle.ForeColor != Color.Empty);
        }


        /// <summary>
        /// Gets or sets the Font used by the Cell
        /// </summary>
        /// <MetaDataID>{2ea5f0c9-04bb-4717-89c8-e79a59063ad5}</MetaDataID>
        [Category("Appearance"),
        Description("The font used to display text in the cell")]
        public Font Font
        {
            get
            {
                if (this.CellStyle == null)
                {
                    if (this.Row != null)
                    {
                        return this.Row.Font;
                    }

                    return null;
                }
                else
                {
                    if (this.CellStyle.Font == null)
                    {
                        if (this.Row != null)
                        {
                            return this.Row.Font;
                        }
                    }

                    return this.CellStyle.Font;
                }
            }

            set
            {
                if (this.CellStyle == null)
                {
                    this.CellStyle = new CellStyle();
                }

                if (this.CellStyle.Font != value)
                {
                    Font oldFont = this.Font;

                    this.CellStyle.Font = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.FontChanged, oldFont));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Font property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Font property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{35589F1D-3BDA-4121-9784-78E58AD035AE}</MetaDataID>
        private bool ShouldSerializeFont()
        {
            return (this.cellStyle != null && this.cellStyle.Font != null);
        }


        /// <summary>
        /// Gets or sets the amount of space between the Cells Border and its contents
        /// </summary>
        /// <MetaDataID>{52d80cc6-9a7d-4694-acf5-fa2396bc997c}</MetaDataID>
        [Category("Appearance"),
        Description("The amount of space between the cells border and its contents")]
        public CellPadding Padding
        {
            get
            {
                if (this.CellStyle == null)
                {
                    return CellPadding.Empty;
                }

                return this.CellStyle.Padding;
            }

            set
            {
                if (this.CellStyle == null)
                {
                    this.CellStyle = new CellStyle();
                }

                if (this.CellStyle.Padding != value)
                {
                    CellPadding oldPadding = this.Padding;

                    this.CellStyle.Padding = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.PaddingChanged, oldPadding));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Padding property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Padding property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{AA6E7BC6-DF7B-4638-BB80-F20153D8077D}</MetaDataID>
        private bool ShouldSerializePadding()
        {
            return this.Padding != CellPadding.Empty;
        }


        /// <summary>
        /// Gets or sets whether the Cell is in the checked state
        /// </summary>
        /// <MetaDataID>{ec8a76a8-8cc6-4547-8731-709ddf4f4f00}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(false),
        Description("Indicates whether the cell is checked or unchecked"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		RefreshProperties(RefreshProperties.Repaint)]
        public bool Checked
        {
            get
            {
                if (this.checkStyle == null)
                {
                    return false;
                }

                return this.checkStyle.Checked;
            }

            set
            {
                if (this.checkStyle == null)
                {
                    this.checkStyle = new CellCheckStyle();
                }

                if (this.checkStyle.Checked != value)
                {
                    bool oldCheck = this.Checked;

                    this.checkStyle.Checked = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.CheckStateChanged, oldCheck));
                }
            }
        }


        /// <summary>
        /// Gets or sets the state of the Cells check box
        /// </summary>
        /// <MetaDataID>{31e1fd90-d2e7-4e1d-8b3b-885920967d20}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(CheckState.Unchecked),
		Description("Indicates the state of the cells check box"),
		RefreshProperties(RefreshProperties.Repaint)]
        public CheckState CheckState
        {
            get
            {
                if (this.checkStyle == null)
                {
                    return CheckState.Unchecked;
                }

                return this.checkStyle.CheckState;
            }

            set
            {
                if (!Enum.IsDefined(typeof(CheckState), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
                }

                if (this.checkStyle == null)
                {
                    this.checkStyle = new CellCheckStyle();
                }

                if (this.checkStyle.CheckState != value)
                {
                    CheckState oldCheckState = this.CheckState;

                    this.checkStyle.CheckState = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.CheckStateChanged, oldCheckState));
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Cells check box 
        /// will allow three check states rather than two
        /// </summary>
        /// <MetaDataID>{0fb63d24-2a46-4ca3-bba1-b43298e011ba}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(false),
		Description("Controls whether or not the user can select the indeterminate state of the cells check box"),
		RefreshProperties(RefreshProperties.Repaint)]
        public bool ThreeState
        {
            get
            {
                if (this.checkStyle == null)
                {
                    return false;
                }

                return this.checkStyle.ThreeState;
            }

            set
            {
                if (this.checkStyle == null)
                {
                    this.checkStyle = new CellCheckStyle();
                }

                if (this.checkStyle.ThreeState != value)
                {
                    bool oldThreeState = this.ThreeState;

                    this.checkStyle.ThreeState = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ThreeStateChanged, oldThreeState));
                }
            }
        }


        /// <summary>
        /// Gets or sets the image that is displayed in the Cell
        /// </summary>
        /// <MetaDataID>{a5473db6-8a86-4bd9-9e44-6fd68df2b4c2}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
		Description("The image that will be displayed in the cell")]
        public Image Image
        {
            get
            {
                if (this.imageStyle == null)
                {
                    return null;
                }

                return this.imageStyle.Image;
            }

            set
            {
                if (this.imageStyle == null)
                {
                    this.imageStyle = new CellImageStyle();

                    
                     this.imageStyle.ImageSizeMode = Models.ImageSizeMode.Normal;
                    if ((this.Column as ImageColumn).SizeMode == OOAdvantech.UserInterface.ImageBoxSizeMode.StretchImage)
                        this.imageStyle.ImageSizeMode = Models.ImageSizeMode.ScaledToFit;
                    if ((this.Column as ImageColumn).SizeMode == OOAdvantech.UserInterface.ImageBoxSizeMode.AutoSize)
                        this.imageStyle.ImageSizeMode = Models.ImageSizeMode.SizedToFit;


                    
                }

                if (this.imageStyle.Image != value)
                {
                    Image oldImage = this.Image;

                    this.imageStyle.Image = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ImageChanged, oldImage));
                }
            }
        }


        /// <summary>
        /// Gets or sets how the Cells image is sized within the Cell
        /// </summary>
        /// <MetaDataID>{2da5183d-99ca-44dd-af0e-80de8cf11148}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(ImageSizeMode.Normal),
		Description("Controls how the image is sized within the cell")]
        public ImageSizeMode ImageSizeMode
        {
            get
            {
                if (this.imageStyle == null)
                {
                    return ImageSizeMode.Normal;
                }

                return this.imageStyle.ImageSizeMode;
            }

            set
            {
                if (this.imageStyle == null)
                {
                    this.imageStyle = new CellImageStyle();
                }

                if (this.imageStyle.ImageSizeMode != value)
                {
                    ImageSizeMode oldSizeMode = this.ImageSizeMode;

                    this.imageStyle.ImageSizeMode = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ImageSizeModeChanged, oldSizeMode));
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Cells contents are able 
        /// to be edited
        /// </summary>
        /// <MetaDataID>{4d305f87-62d8-4821-b88b-04e7016f7d30}</MetaDataID>
        [Category("Appearance"),
        Description("Controls whether the cells contents are able to be changed by the user")]
        public bool Editable
        {
            get
            {
                if (!this.GetState(STATE_EDITABLE))
                {
                    return false;
                }

                if (this.Row == null)
                {
                    return this.Enabled;
                }

                return this.Enabled && this.Row.Editable;
            }

            set
            {
                bool editable = this.Editable;

                this.SetState(STATE_EDITABLE, value);

                if (editable != value)
                {
                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.EditableChanged, editable));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Editable property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Editable property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{6CC4019A-A7A2-4E77-83ED-CF99657B7CA5}</MetaDataID>
        private bool ShouldSerializeEditable()
        {
            return !this.GetState(STATE_EDITABLE);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Cell 
        /// can respond to user interaction
        /// </summary>
        /// <MetaDataID>{2eb8009c-600f-4cf7-85ac-439f9601c404}</MetaDataID>
        [Category("Appearance"),
        Description("Indicates whether the cell is enabled")]
        public bool Enabled
        {
            get
            {
                if (!this.GetState(STATE_ENABLED))
                {
                    return false;
                }

                if (this.Row == null)
                {
                    return true;
                }

                return this.Row.Enabled;
            }

            set
            {
                bool enabled = this.Enabled;

                this.SetState(STATE_ENABLED, value);

                if (enabled != value)
                {
                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.EnabledChanged, enabled));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Enabled property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Enabled property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{9BB70EBB-9BCD-4AD8-A94C-A31BE1373519}</MetaDataID>
        private bool ShouldSerializeEnabled()
        {
            return !this.GetState(STATE_ENABLED);
        }


        /// <summary>
        /// Gets or sets the text displayed in the Cells tooltip
        /// </summary>
        /// <MetaDataID>{a658f057-7ea5-4f91-96e0-b36f9cb24c36}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
		Description("The text displayed in the cells tooltip")]
        public string ToolTipText
        {
            get
            {
                return this.tooltipText;
            }

            set
            {
                if (this.tooltipText != value)
                {
                    string oldToolTip = this.tooltipText;

                    this.tooltipText = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ToolTipTextChanged, oldToolTip));
                }
            }
        }


        /// <summary>
        /// Gets or sets the information used by CellRenderers to record the current 
        /// state of the Cell
        /// </summary>
        /// <MetaDataID>{e3fe4974-7f5a-482d-87e1-3a33eed2bd0d}</MetaDataID>
        protected internal object RendererData
        {
            get
            {
                return this.rendererData;
            }

            set
            {
                this.rendererData = value;
            }
        }


        /// <summary>
        /// Gets the Row that the Cell belongs to
        /// </summary>
        /// <MetaDataID>{3183d0af-26e0-4e26-bac2-023d0abd4f08}</MetaDataID>
        [Browsable(false)]
        public Row Row
        {
            get
            {
                return this.row;
            }
        }


        /// <summary>
        /// Gets or sets the Row that the Cell belongs to
        /// </summary>
        /// <MetaDataID>{82898900-30c7-42f0-89bf-5751bc8297a6}</MetaDataID>
        internal Row InternalRow
        {
            get
            {
                return this.row;
            }

            set
            {
                this.row = value;
            }
        }


        /// <summary>
        /// Gets the index of the Cell within its Row
        /// </summary>
        /// <MetaDataID>{38764b25-948e-4263-98ee-cb6847c9ecc4}</MetaDataID>
        [Browsable(false)]
        public int Index
        {
            get
            {
                return this.index;
            }
        }


        /// <summary>
        /// Gets or sets the index of the Cell within its Row
        /// </summary>
        /// <MetaDataID>{538e121b-76fe-466e-a8fa-0103b5df28d3}</MetaDataID>
        internal int InternalIndex
        {
            get
            {
                return this.index;
            }

            set
            {
                this.index = value;
            }
        }


        /// <summary>
        /// Gets whether the Cell is able to raise events
        /// </summary>
        /// <MetaDataID>{c90262a9-496d-4ece-8908-54894779cef4}</MetaDataID>
        protected internal bool CanRaiseEvents
        {
            get
            {
                // check if the Row that the Cell belongs to is able to 
                // raise events (if it can't, the Cell shouldn't raise 
                // events either)
                if (this.Row != null)
                {
                    return this.Row.CanRaiseEvents;
                }

                return true;
            }
        }

        #endregion


        #region Events

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{D8F1F7D0-BE5F-4518-9EF0-1BB29344020F}</MetaDataID>
        protected virtual void OnPropertyChanged(CellEventArgs e)
        {
            e.SetColumn(this.Index);

            if (this.Row != null)
            {
                e.SetRow(this.Row.Index);
            }

            if (this.CanRaiseEvents)
            {
                if (this.Row != null)
                {
                    this.Row.OnCellPropertyChanged(e);
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }
        }

        #endregion

        #region IPathDataDisplayer Members


        /// <MetaDataID>{785f9c0f-1b17-4471-a781-9c099c3f02fb}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get { return new OOAdvantech.Collections.Generic.List<string>(); }
        }

        #endregion

        #region IPathDataDisplayer Members

        /// <MetaDataID>{3ef2cfdc-ded8-4cf0-bf11-d94603869310}</MetaDataID>
        public object Path
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{c1227a5f-d272-4edf-bbe3-d44257e68b4a}</MetaDataID>
        public void LoadControlValues()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{f90950c7-a494-467e-81af-5fe9c40fcc81}</MetaDataID>
        public void SaveControlValues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
