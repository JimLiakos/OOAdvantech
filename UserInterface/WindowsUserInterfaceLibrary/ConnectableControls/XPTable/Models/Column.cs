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

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Renderers;
using ConnectableControls;
using ConnectableControls.PropertyEditors;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using ConnectableControls.ListView;
using OOAdvantech.UserInterface.Runtime;
using System.Collections.Generic;


namespace ConnectableControls.List.Models
{

    /// <summary>
    /// Summary description for Column.
    /// </summary>
    /// <MetaDataID>{28DC5F79-CB01-4EEA-998F-C3213677AC4F}</MetaDataID>
    [DesignTimeVisible(false),
    ToolboxItem(false)]
    public abstract class Column : Component, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, IColumn
    {
        /// <MetaDataID>{22491286-e716-41d8-97df-843df28f00af}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{0c7688b3-5d63-4fed-89a5-fcb6d8e72db0}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{112c9c37-2d53-4517-bacb-e9e3c4b33f10}</MetaDataID>
        public void DesigneRefresh()
        {
            

        }
        /// <MetaDataID>{740f5f7a-23ec-48f9-980e-518958241753}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{25cc56a5-75d6-4b7c-9b48-88aa250feb61}</MetaDataID>
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }
        //ColumnConnection ConnectableControls.ListView.IColumn.ColumnConnection
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}


        #region Event Handlers

        /// <summary>
        /// Occurs when one of the Column's properties changes
        /// </summary>
        public event ColumnEventHandler PropertyChanged;

        #endregion


        #region Class Data

        // Column state flags
        /// <MetaDataID>{8AE6BE8A-5214-4DE6-92E1-FA1234999900}</MetaDataID>
        private readonly static int STATE_EDITABLE = 1;
        /// <MetaDataID>{EA10870A-1817-4BF9-A425-6D43042B0754}</MetaDataID>
        private readonly static int STATE_ENABLED = 2;
        /// <MetaDataID>{8CCC759F-F72A-4CB3-97E5-EA6E710FAB38}</MetaDataID>
        private readonly static int STATE_VISIBLE = 4;
        /// <MetaDataID>{EB0F33E7-2C9B-4524-AC80-4D9DC83A2213}</MetaDataID>
        private readonly static int STATE_SELECTABLE = 8;
        /// <MetaDataID>{C5F9A66E-E3EA-40D5-8CB4-6305CC3D55E0}</MetaDataID>
        private readonly static int STATE_SORTABLE = 16;

        /// <summary>
        /// The amount of space on each side of the Column that can 
        /// be used as a resizing handle
        /// </summary>
        /// <MetaDataID>{37D164D0-193D-4657-B8B1-098F6F7BBAB2}</MetaDataID>
        public static readonly int ResizePadding = 8;

        /// <summary>
        /// The default width of a Column
        /// </summary>
        /// <MetaDataID>{6C3D4867-C3EB-41EB-8E8C-B3BED25CFD91}</MetaDataID>
        public static readonly int DefaultWidth = 75;

        /// <summary>
        /// The maximum width of a Column
        /// </summary>
        /// <MetaDataID>{865892A9-DB7D-42CF-B900-DE970B3B0D32}</MetaDataID>
        public static readonly int MaximumWidth = 1024;

        /// <summary>
        /// The minimum width of a Column
        /// </summary>
        /// <MetaDataID>{ABC726E7-7BCC-47CC-8DE0-81EBD32B540A}</MetaDataID>
        public static readonly int MinimumWidth = ResizePadding * 2;

        /// <summary>
        /// Contains the current state of the the Column
        /// </summary>
        /// <MetaDataID>{B1C14F8F-4D8B-4397-959A-169FE6D44CEF}</MetaDataID>
        public byte state;

        /// <summary>
        /// The text displayed in the Column's header
        /// </summary>
        /// <MetaDataID>{915C3843-689E-417F-818B-451F3DBAF9DF}</MetaDataID>
        private string text;

        /// <summary>
        /// A string that specifies how a Column's Cell contents are formatted
        /// </summary>
        /// <MetaDataID>{8CE68868-0D95-49AD-AF2E-81502167CF27}</MetaDataID>
        private string format;

        /// <summary>
        /// The alignment of the text displayed in the Column's Cells
        /// </summary>
        /// <MetaDataID>{48710A4A-0637-4CC5-AFA8-0A5FC0CB7929}</MetaDataID>
        private ColumnAlignment alignment;

        /// <summary>
        /// The width of the Column
        /// </summary>
        /// <MetaDataID>{114031E5-44AE-4119-8877-9CE1EA8DC541}</MetaDataID>
        private int width;

        /// <summary>
        /// The Image displayed on the Column's header
        /// </summary>
        /// <MetaDataID>{22B5DF6E-B493-46B8-A233-F4F2FF2B2657}</MetaDataID>
        private Image image;

        /// <summary>
        /// Specifies whether the Image displayed on the Column's header should 
        /// be draw on the right hand side of the Column
        /// </summary>
        /// <MetaDataID>{7A5B8C00-94DB-4C2E-BE53-982165B622DF}</MetaDataID>
        private bool imageOnRight;

        /// <summary>
        /// The current state of the Column
        /// </summary>
        /// <MetaDataID>{6E2094C0-E64D-46CF-B777-65D0E1A4EBBF}</MetaDataID>
        private ColumnState columnState;

        /// <summary>
        /// The text displayed when a ToolTip is shown for the Column's header
        /// </summary>
        /// <MetaDataID>{CEE0428D-2C6A-4938-89B7-263A1EEBAF75}</MetaDataID>
        private string tooltipText;

        /// <summary>
        /// The ColumnModel that the Column belongs to
        /// </summary>
        /// <MetaDataID>{6582037D-4452-4DDD-BD61-C369BA433EC9}</MetaDataID>
        private ColumnModel columnModel;

        /// <summary>
        /// The x-coordinate of the column's left edge in pixels
        /// </summary>
        /// <MetaDataID>{2A344856-47B5-4078-BD0E-A3676D1EE967}</MetaDataID>
        private int x;

        /// <summary>
        /// The current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{B696DF08-61D1-4393-9C1F-F03293F4E32F}</MetaDataID>
        private SortOrder sortOrder;

        /// <summary>
        /// The CellRenderer used to draw the Column's Cells
        /// </summary>
        /// <MetaDataID>{865F7B52-2F6A-482F-AE3D-25415B6DE4C8}</MetaDataID>
        private ICellRenderer renderer;

        /// <summary>
        /// The CellEditor used to edit the Column's Cells
        /// </summary>
        /// <MetaDataID>{71985CBC-122C-403C-9DBA-FFD9218444CE}</MetaDataID>
        private ICellEditor editor;

        /// <summary>
        /// The Type of the IComparer used to compare the Column's Cells
        /// </summary>
        /// <MetaDataID>{A690B623-280E-4F34-A5E0-2E7135BB8CCB}</MetaDataID>
        private Type comparer;

        #endregion


        #region Constructor

        /// <summary>
        /// Creates a new Column with default values
        /// </summary>
        /// <MetaDataID>{11D2CB2B-43F4-425C-B851-D49553D14A39}</MetaDataID>
        public Column()
            : base()
        {
            this.Init();
        }


        /// <summary>
        /// Creates a new Column with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{AE3B1B3D-9B96-4FC5-BB99-55A15B502628}</MetaDataID>
        public Column(string text)
            : base()
        {
            this.Init();

            this.text = text;
        }


        /// <summary>
        /// Creates a new Column with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{3E313300-5E96-4E1A-86A0-938A537A2BC5}</MetaDataID>
        public Column(string text, int width)
            : base()
        {
            this.Init();

            this.text = text;
            this.width = width;
        }


        /// <summary>
        /// Creates a new Column with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{509D9D37-6F35-4BF9-8870-55F5B4748C03}</MetaDataID>
        public Column(string text, int width, bool visible)
            : base()
        {
            this.Init();

            this.text = text;
            this.width = width;
            this.Visible = visible;
        }


        /// <summary>
        /// Creates a new Column with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{51AA87C8-93D5-4DFE-AF2A-9AE516E752D7}</MetaDataID>
        public Column(string text, Image image)
            : base()
        {
            this.Init();

            this.text = text;
            this.image = image;
        }


        /// <summary>
        /// Creates a new Column with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{50B59AC6-4C25-4D9C-8D2C-6AC81AF96D55}</MetaDataID>
        public Column(string text, Image image, int width)
            : base()
        {
            this.Init();

            this.text = text;
            this.image = image;
            this.width = width;
        }


        /// <summary>
        /// Creates a new Column with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{099FDFB0-B7E0-4F8D-BCFE-11F84D2E4C1E}</MetaDataID>
        public Column(string text, Image image, int width, bool visible)
            : base()
        {
            this.Init();

            this.text = text;
            this.image = image;
            this.width = width;
            this.Visible = visible;
        }


        /// <summary>
        /// Initialise default values
        /// </summary>
        /// <MetaDataID>{5009C7AA-41A5-4A40-8B19-477E80423069}</MetaDataID>
        private void Init()
        {
            this.text = null;
            this.width = Column.DefaultWidth;
            this.columnState = ColumnState.Normal;
            this.alignment = ColumnAlignment.Left;
            this.image = null;
            this.imageOnRight = false;
            this.columnModel = null;
            this.x = 0;
            this.tooltipText = null;
            this.format = "";
            this.sortOrder = SortOrder.None;
            this.renderer = null;
            this.editor = null;
            this.comparer = null;

            this.state = (byte)(STATE_ENABLED | STATE_EDITABLE | STATE_VISIBLE | STATE_SELECTABLE | STATE_SORTABLE);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{802EC675-DB91-492D-90DA-798C9456F64E}</MetaDataID>
        public abstract string GetDefaultRendererName();


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{0D9137BD-2BA6-474D-9E92-71B2FA2DD9F0}</MetaDataID>
        public abstract ICellRenderer CreateDefaultRenderer();


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{A3DA711C-0F58-49B5-8D69-28FA1E3833F9}</MetaDataID>
        public abstract string GetDefaultEditorName();


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{EB036DEF-794C-4B15-8313-11198F67A722}</MetaDataID>
        public abstract ICellEditor CreateDefaultEditor();


        /// <summary>
        /// Returns the state represented by the specified state flag
        /// </summary>
        /// <param name="flag">A flag that represents the state to return</param>
        /// <returns>The state represented by the specified state flag</returns>
        /// <MetaDataID>{292880E4-DC18-43AB-8A9A-4A833357ED27}</MetaDataID>
        internal bool GetState(int flag)
        {
            return ((this.state & flag) != 0);
        }


        /// <summary>
        /// Sets the state represented by the specified state flag to the specified value
        /// </summary>
        /// <param name="flag">A flag that represents the state to be set</param>
        /// <param name="value">The new value of the state</param>
        /// <MetaDataID>{4D03E14A-9B8C-44F0-8E1F-0048C30FD13C}</MetaDataID>
        internal void SetState(int flag, bool value)
        {
            this.state = (byte)(value ? (this.state | flag) : (this.state & ~flag));
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the text displayed on the Column header
        /// </summary>
        /// <MetaDataID>{4C7AA8C8-3D56-4C1B-B413-1FE8F2F84CCF}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("The text displayed in the column's header.")]
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.text))
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Text = value;

                    string oldText = this.text;

                    this.text = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.TextChanged, oldText));
                }
            }
        }


        /// <summary>
        /// Gets or sets the string that specifies how a Column's Cell contents 
        /// are formatted
        /// </summary>
        /// <MetaDataID>{45692E1B-BAF4-4CBF-B731-FA880D978BA3}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(""),
        Description("A string that specifies how a column's cell contents are formatted.")]
        public string Format
        {
            get
            {
                return this.format;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.format))
                {

                    string oldFormat = this.format;

                    this.format = value;

                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Format = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.FormatChanged, oldFormat));
                }
            }
        }


        /// <summary>
        /// Gets or sets the horizontal alignment of the Column's Cell contents
        /// </summary>
        /// <MetaDataID>{13591E42-5BE0-475C-BE30-3062681AA862}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(ColumnAlignment.Left),
        Description("The horizontal alignment of the column's cell contents.")]
        public virtual ColumnAlignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ColumnAlignment), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnAlignment));
                }

                if (this.alignment != value)
                {
                    ColumnAlignment oldAlignment = this.alignment;

                    this.alignment = value;
                    if (_ColumnMetaData != null)
                    {
                        switch (this.alignment)
                        {
                            case ColumnAlignment.Center:
                                _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Center;
                                break;
                            case ColumnAlignment.Left:
                                _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Left;
                                break;
                            case ColumnAlignment.Right:
                                _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Right;
                                break;

                        }
                    }
                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.AlignmentChanged, oldAlignment));
                }
            }
        }


        /// <summary>
        /// Gets or sets the width of the Column
        /// </summary>
        /// <MetaDataID>{8B23DB64-0C12-4AB7-9787-E620CDF6DF7D}</MetaDataID>
        [Category("Appearance"),
        Description("The width of the column.")]
        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                if (this.width != value)
                {
                    int oldWidth = this.Width;

                    // Set the width, and check min & max
                    this.width = Math.Min(Math.Max(value, MinimumWidth), MaximumWidth);
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Width = this.width;


                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.WidthChanged, oldWidth));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Width property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Width property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{4FED7AB3-323E-4B51-8885-2AD3C78939CC}</MetaDataID>
        private bool ShouldSerializeWidth()
        {
            return this.Width != Column.DefaultWidth;
        }


        /// <summary>
        /// Gets or sets the Image displayed in the Column's header
        /// </summary>
        /// <MetaDataID>{E8106AC9-EECE-44DF-B798-47040279F0FD}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("Ihe image displayed in the column's header")]
        public Image Image
        {
            get
            {
                return this.image;
            }

            set
            {
                if (this.image != value)
                {
                    Image oldImage = this.Image;

                    this.image = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ImageChanged, oldImage));
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Image displayed on the Column's header should 
        /// be draw on the right hand side of the Column
        /// </summary>
        /// <MetaDataID>{A5409CA3-CA3A-41F9-A89A-540D07B1038C}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(false),
        Description("Specifies whether the image displayed on the column's header should be drawn on the right hand side of the column")]
        public bool ImageOnRight
        {
            get
            {
                return this.imageOnRight;
            }

            set
            {
                if (this.imageOnRight != value)
                {
                    this.imageOnRight = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ImageChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets the state of the Column
        /// </summary>
        /// <MetaDataID>{19466F3C-7C02-4AE6-A42B-6D585CBA4183}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnState ColumnState
        {
            get
            {
                return this.columnState;
            }
        }


        /// <summary>
        /// Gets or sets the state of the Column
        /// </summary>
        /// <MetaDataID>{EF47D9A5-913D-4404-B025-766D13F1E204}</MetaDataID>
        internal ColumnState InternalColumnState
        {
            get
            {
                return this.ColumnState;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ColumnState), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnState));
                }

                if (this.columnState != value)
                {
                    ColumnState oldState = this.columnState;

                    this.columnState = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.StateChanged, oldState));
                }
            }
        }


        /// <summary>
        /// Gets or sets the whether the Column is displayed
        /// </summary>
        /// <MetaDataID>{ACD7DC15-E1F8-4268-AD45-0ABAF22C8F29}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Determines whether the column is visible or hidden.")]
        public bool Visible
        {
            get
            {
                return this.GetState(STATE_VISIBLE);
            }

            set
            {
                bool visible = this.Visible;

                this.SetState(STATE_VISIBLE, value);

                if (visible != value)
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Visible = value;
                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.VisibleChanged, visible));
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Column is able to be sorted
        /// </summary>
        /// <MetaDataID>{3ADF38D7-D0A3-4F0E-9D9E-1F8C2C7BE6AF}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Determines whether the column is able to be sorted.")]
        public virtual bool Sortable
        {
            get
            {
                return this.GetState(STATE_SORTABLE);
            }

            set
            {
                bool sortable = this.Sortable;

                this.SetState(STATE_SORTABLE, value);

                if (sortable != value)
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Sortable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SortableChanged, sortable));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified ICellRenderer that is used to draw the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{0D9E55AB-E4D0-412B-98EE-2316BC907EDA}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICellRenderer Renderer
        {
            get
            {
                return this.renderer;
            }

            set
            {
                if (this.renderer != value)
                {
                    this.renderer = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified ICellEditor that is used to edit the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{FE52B7BC-BA9C-43ED-9B7A-88BDCAC077BE}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICellEditor Editor
        {
            get
            {
                return this.editor;
            }

            set
            {
                if (this.editor != value)
                {
                    this.editor = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EditorChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified Comparer type that is used to edit the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{21BAFAA5-33AB-4143-AD56-DA8793D3EAF2}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type Comparer
        {
            get
            {
                return this.comparer;
            }

            set
            {
                if (this.comparer != value)
                {
                    this.comparer = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ComparerChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets the Type of the default Comparer used to compare the Column's Cells when 
        /// the Column is sorting
        /// </summary>
        /// <MetaDataID>{D7DD1DC6-E10E-4990-B2F7-98748A731C20}</MetaDataID>
        [Browsable(false)]
        public abstract Type DefaultComparerType
        {
            get;
        }


        /// <summary>
        /// Gets the current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{76F30E6A-30F5-4F2A-9214-8FB2131F0FC4}</MetaDataID>
        [Browsable(false)]
        public SortOrder SortOrder
        {
            get
            {
                return this.sortOrder;
            }
        }


        /// <summary>
        /// Gets or sets the current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{D698DFAC-4CAD-4D99-AAE7-11BACFD9C2C9}</MetaDataID>
        internal SortOrder InternalSortOrder
        {
            get
            {
                return this.SortOrder;
            }

            set
            {
                if (!Enum.IsDefined(typeof(SortOrder), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
                }

                if (this.sortOrder != value)
                {
                    SortOrder oldOrder = this.sortOrder;

                    this.sortOrder = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SortOrderChanged, oldOrder));
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells contents 
        /// are able to be edited
        /// </summary>
        /// <MetaDataID>{BE71B337-965A-46E2-86E9-EEA0E1771AC6}</MetaDataID>
        [Category("Appearance"),
        Description("Controls whether the column's cell contents are able to be changed by the user")]
        public virtual bool Editable
        {
            get
            {
                if (!this.GetState(STATE_EDITABLE))
                {
                    return false;
                }

                return this.Visible && this.Enabled;
            }

            set
            {
                bool editable = this.GetState(STATE_EDITABLE);

                this.SetState(STATE_EDITABLE, value);

                if (editable != value)
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Editable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EditableChanged, editable));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Editable property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Editable property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{4178D37E-C414-43FB-BDFD-D066301B3F15}</MetaDataID>
        private bool ShouldSerializeEditable()
        {
            return !this.GetState(STATE_EDITABLE);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells can respond to 
        /// user interaction
        /// </summary>
        /// <MetaDataID>{8B807EF6-795B-48DD-BD4E-594FC22678DC}</MetaDataID>
        [Category("Appearance"),
        Description("Indicates whether the column's cells can respond to user interaction")]
        public bool Enabled
        {
            get
            {
                if (!this.GetState(STATE_ENABLED))
                {
                    return false;
                }

                if (this.ColumnModel == null)
                {
                    return true;
                }

                return this.ColumnModel.Enabled;
            }

            set
            {
                bool enabled = this.GetState(STATE_ENABLED);

                this.SetState(STATE_ENABLED, value);

                if (enabled != value)
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Enabled = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EnabledChanged, enabled));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Enabled property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Enabled property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{B9E90865-3C73-4FF6-A0E3-DAC9F57D71DB}</MetaDataID>
        private bool ShouldSerializeEnabled()
        {
            return !this.GetState(STATE_ENABLED);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells can be selected
        /// </summary>
        /// <MetaDataID>{2D1F3FB4-A798-46F1-BF56-8B52BE3C17DB}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Indicates whether the column's cells can be selected")]
        public virtual bool Selectable
        {
            get
            {
                return this.GetState(STATE_SELECTABLE);
            }

            set
            {
                bool selectable = this.Selectable;

                this.SetState(STATE_SELECTABLE, value);

                if (selectable != value)
                {
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.Selectable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SelectableChanged, selectable));
                }
            }
        }


        /// <summary>
        /// Gets or sets the ToolTip text associated with the Column
        /// </summary>
        /// <MetaDataID>{5BCF2C77-8B2D-440A-B51B-32485C98BE9C}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("The ToolTip text associated with the Column")]
        public string ToolTipText
        {
            get
            {
                return this.tooltipText;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.tooltipText))
                {
                    string oldTip = this.tooltipText;

                    this.tooltipText = value;
                    if (_ColumnMetaData != null)
                        _ColumnMetaData.ToolTipText = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ToolTipTextChanged, oldTip));
                }
            }
        }


        /// <summary>
        /// Gets the x-coordinate of the column's left edge in pixels
        /// </summary>
        /// <MetaDataID>{096A0EFE-D64D-4890-A013-CC98274C24DE}</MetaDataID>
        internal int X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }


        /// <summary>
        /// Gets the x-coordinate of the column's left edge in pixels
        /// </summary>
        /// <MetaDataID>{ED2904F1-B40C-48AC-8CCD-4B6B19ADDAD1}</MetaDataID>
        [Browsable(false)]
        public int Left
        {
            get
            {
                return this.X;
            }
        }


        /// <summary>
        /// Gets the x-coordinate of the column's right edge in pixels
        /// </summary>
        /// <MetaDataID>{A1D0C1AD-0DBB-43B4-AC28-E331A15EA413}</MetaDataID>
        [Browsable(false)]
        public int Right
        {
            get
            {
                return this.Left + this.Width;
            }
        }


        /// <summary>
        /// Gets or sets the ColumnModel the Column belongs to
        /// </summary>
        /// <MetaDataID>{F102C8DD-CD4D-4F4B-AF4C-E5BC278C1EC5}</MetaDataID>
        protected internal ColumnModel ColumnModel
        {
            get
            {
                return this.columnModel;
            }

            set
            {
                this.columnModel = value;
            }
        }



        /// <summary>
        /// Gets the ColumnModel the Column belongs to.  This member is not 
        /// intended to be used directly from your code
        /// </summary>
        /// <MetaDataID>{8BC857B9-A409-4CA3-8A04-DDF567F24F75}</MetaDataID>
        [Browsable(false)]
        public ColumnModel Parent
        {
            get
            {
                return this.ColumnModel;
            }
        }


        /// <summary>
        /// Gets whether the Column is able to raise events
        /// </summary>
        /// <MetaDataID>{624394D0-10C2-41F1-B7F0-14839C88EC46}</MetaDataID>
        protected bool CanRaiseEvents
        {
            get
            {
                // check if the ColumnModel that the Colum belongs to is able to 
                // raise events (if it can't, the Colum shouldn't raise events either)
                if (this.ColumnModel != null)
                {
                    return this.ColumnModel.CanRaiseEvents;
                }

                return true;
            }
        }

        #endregion


        #region Events

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{EEEEAD19-AD3B-44B5-B331-6030E0850A69}</MetaDataID>
        protected virtual void OnPropertyChanged(ColumnEventArgs e)
        {
            if (this.ColumnModel != null)
            {
                e.SetIndex(this.ColumnModel.Columns.IndexOf(this));
            }

            if (this.CanRaiseEvents)
            {
                if (this.ColumnModel != null)
                {
                    this.ColumnModel.OnColumnPropertyChanged(e);
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }
        }

        #endregion


        #region IObjectMemberViewControl Members

        /// <MetaDataID>{148de913-3bd6-4d06-9df9-3dab460113ef}</MetaDataID>
        public string Name
        {
            get
            {
                return text;
            }
            set
            {
            }
        }


        /// <MetaDataID>{58c45f78-67eb-408d-8cdc-0fa710d29e57}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{c17b07a8-d8a1-4ec9-9dc6-590ceb82760e}</MetaDataID>
        public bool AllowDrag
        {
            get
            {
                return _AllowDrag;
            }
            set
            {
                _AllowDrag = value; ;
            }
        }

        /// <MetaDataID>{5fc6db74-3c13-4378-b2b2-f07f17593b8b}</MetaDataID>
        bool _AllowDrop = false;
        /// <MetaDataID>{6ddd2f86-f483-4963-b18e-967565a6d4f6}</MetaDataID>
        public bool AllowDrop
        {
            get
            {
                return _AllowDrop;
            }
            set
            {
                _AllowDrop = value; ;
            }
        }

        /// <MetaDataID>{1a723753-cdad-41cd-bebe-d56f4c842f82}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: List '" + columnModel.Table.Name + "' Column '" + Text + "' has lose the connection.", columnModel.Table.FindForm().GetType().FullName));
                hasErrors = true;
            }
            if (Menu != null)
            {
                hasErrors |= Menu.ErrorCheck(ref errors);
            }
            return false;

        }


        /// <MetaDataID>{613C9161-0BDE-48B0-AB85-58DE2697EDF5}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            //this.columnModel.Table.CollectionObjectType.
            //Type type = ViewControlObject.GetClassifier(this.columnModel.Table.CollectionObjectType, _Path);
            //return AssemblyManager.GetComponent(type.Assembly.FullName).GetClassifier(type.FullName, true);
            return ValueType;
        }






        /// <MetaDataID>{F91522CC-078E-423E-A6E7-28F41EA5DCFB}</MetaDataID>
        public virtual object Value
        {
            get
            {
                if (columnModel.Table.LastMouseCell.Row == -1)
                    return null;
                return columnModel.Table.TableModel.Rows[columnModel.Table.LastMouseCell.Row].Cells[columnModel.Table.LastMouseCell.Column].Data;
            }
            set
            {

            }
        }


        /// <MetaDataID>{18772F93-6CF9-446A-90CA-26B66E569AEB}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{EF120EB9-177F-4E88-A5A7-C1307CB712F8}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {

                if (_ValueType == null)
                {
                    OOAdvantech.MetaDataRepository.Classifier collectionObjectType =Owner.ListConnection.PresentationObjectType;
                    if (collectionObjectType != null)
                        _ValueType = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(this.Owner.ListConnection.PresentationObjectType, _Path);
                }
                return _ValueType;
            }

        }

        /// <MetaDataID>{7D0D5870-24BB-43CC-8A10-3BF2F16415AB}</MetaDataID>
        protected string _Path;
        /// <MetaDataID>{A9A6A158-E7BB-4245-9440-8D240237EDE1}</MetaDataID>
        public object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (value is string)
                    _Path = value as string;
                else if (value is MetaData)
                    _Path = (value as MetaData).Path;


                if (_ColumnMetaData != null)
                    _ColumnMetaData.Path = _Path;

            }
        }


        /// <MetaDataID>{66ed8c3a-bec7-45aa-b1c1-ad72a7239612}</MetaDataID>
        public void LoadControlValues()
        {

        }

        /// <MetaDataID>{7c7c4ec3-c211-4eb9-ace9-6a9271710422}</MetaDataID>
        public void SaveControlValues()
        {

        }

        #endregion

        #region IOperetionCallerSource Members

        /// <MetaDataID>{e0cffce0-1308-4653-83a7-175fd02ae225}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string[] PropertiesNames
        {
            get
            {
                return new string[2]{"RowValue","CellValue"};
            }
        }
        /// <MetaDataID>{b97f3cfd-49ef-433b-b526-e890a85daeca}</MetaDataID>
        public virtual object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "CellValue")
                return Value;
            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{a171f4ce-7f73-4aaa-88ab-f7f29156b906}</MetaDataID>
        public virtual bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;

            if (propertyName == "CellValue")
                return true;
            if (propertyName == "RowValue")
                return true;
            

            return false;
        }
        /// <MetaDataID>{75e202f3-8201-4495-84bd-7cb42727584c}</MetaDataID>
        public virtual void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        /// <MetaDataID>{8452250d-54b9-4e3a-89fb-abb087dec1ba}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            if (propertyName == "CellValue")
                return ValueType;
            if (propertyName == "RowValue")
                return Owner.ListConnection.PresentationObjectType;
            return null;

        }


        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{d439aebf-5fa0-48c6-a3cb-a65d24a7cb7a}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return this.columnModel.Table.ListConnection.UserInterfaceObjectConnection;
            }
            set
            {

            }
        }
        /// <MetaDataID>{371291CE-2A7B-4A81-B83D-51A75ABDF79A}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                return _ColumnMetaData;
            }
            set
            {

            }
        }

        /// <MetaDataID>{a7fd05d9-4e39-4087-8556-2826eefa6fe9}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }


        #endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{E6220A27-A96E-4FBD-8307-CF858E8BBA52}</MetaDataID>
        public virtual bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            else if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
                return true;
            else if (propertyDescriptor == "OperationCall" && metaObject is OOAdvantech.UserInterface.OperationCall)
            {
                if (new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }


        #endregion

        #region IColumn Members

        /// <MetaDataID>{E8F74DAB-1FD3-499F-8C00-D6DD45587B31}</MetaDataID>
        int _Order = 0;
        /// <MetaDataID>{C14828E3-0618-4C8E-94F7-135E9F0D0541}</MetaDataID>
        public int Order
        {
            set
            {
                _Order = value;
                if (_ColumnMetaData != null)
                    _ColumnMetaData.Position = (short)Order;
            }
            get
            {
                return _Order;
            }
        }

        /// <MetaDataID>{22659616-107e-4e50-8c62-9d52a010df7c}</MetaDataID>
        public IListView Owner
        {
            get
            {
                if (columnModel == null)
                    return null;
                return columnModel.Table;
            }
            set
            {
                if (value is ListView)
                    columnModel = (value as ListView).ColumnModel;

            }
        }
        /// <MetaDataID>{C05DBBC7-87AD-49CE-AB8C-7B3E6E19D781}</MetaDataID>
        OOAdvantech.UserInterface.Column _ColumnMetaData;

        /// <MetaDataID>{4533bca3-6507-454c-ab3a-5d4aaa38b1f2}</MetaDataID>
        public OOAdvantech.UserInterface.Column ColumnMetaData
        {
            get
            {
                return _ColumnMetaData;
            }
            set
            {
                _ColumnMetaData = value;

            }
        }

        /// <MetaDataID>{5236DCD3-FF85-41CF-9767-FB4A73FCCF08}</MetaDataID>
        public void SetValue(IRow row, object value)
        {
            UserInterfaceObjectConnection.SetValue(row.PresentationObject, value, Owner.ListConnection.PresentationObjectType, _Path);
        }


        #endregion

        #region Connectble controls



        /// <MetaDataID>{7A516B23-BE5F-4979-8C0D-F35D79CF3F90}</MetaDataID>
        public Column(OOAdvantech.UserInterface.Column column)
        {
            switch (column.Alignment)
            {
                case OOAdvantech.UserInterface.ColumnAlignment.Center:
                    Alignment = ColumnAlignment.Center;
                    break;
                case OOAdvantech.UserInterface.ColumnAlignment.Left:
                    Alignment = ColumnAlignment.Left;
                    break;
                case OOAdvantech.UserInterface.ColumnAlignment.Right:
                    Alignment = ColumnAlignment.Right;
                    break;

            }
            _Order = column.Position;
            Editable = column.Editable;
            Format = column.Format;
            Enabled = column.Enabled;
            Selectable = column.Selectable;
            Text = column.Text;
            Visible = column.Visible;
            width = column.Width;
            Sortable = column.Sortable;
            ToolTipText = column.ToolTipText;
            //UpdateStyle = column.ConnectedObjectAutoUpdate;
            Path = column.Path;
            DisplayMember = column.DisplayMember;
            _ColumnMetaData = column;
            //Init();
        }

        /// <MetaDataID>{5C819A13-6874-4FC7-90DB-FA8EC8B13703}</MetaDataID>
        public Column(Column copyColumn)
        {
            Alignment = copyColumn.Alignment;
            Editable = copyColumn.Editable;
            Format = copyColumn.Format;
            Enabled = copyColumn.Enabled;
            ImageOnRight = copyColumn.ImageOnRight;
            Selectable = copyColumn.Selectable;
            Image = copyColumn.Image;
            Text = copyColumn.Text;
            Visible = copyColumn.Visible;
            width = copyColumn.Width;
            Sortable = copyColumn.Sortable;
            ToolTipText = copyColumn.ToolTipText;

        }






        /// <MetaDataID>{3B8578F2-409F-4DE9-94C7-5D57B49D18F3}</MetaDataID>
        private string _DisplayMember;
        /// <MetaDataID>{D1015AA9-4963-4670-BA12-12258B73B2E5}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Object DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                if (value is string)
                    _DisplayMember = value as string;
                else if (value is MetaData)
                    _DisplayMember = (value as MetaData).Path;


                if (_ColumnMetaData != null)
                    _ColumnMetaData.DisplayMember = _DisplayMember as string;

            }
        }



        /// <MetaDataID>{C41F1479-1FAD-4879-8978-B50C24C01DEF}</MetaDataID>
        internal Cell LoadCell(Row row)
        {
            try
            {
                object columnValue = null;
                object columnText = null;

                object cellDisplayedValue = null; ;
                object displayedValue = null;
                Cell cell = new Cell(null, "", this);
                row.Cells.Add(cell);

                bool returnValueAsCollection = false;

                if (!string.IsNullOrEmpty(DisplayMember as string))
                {
                    cellDisplayedValue = UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Owner.ListConnection.PresentationObjectType, _Path, cell, out returnValueAsCollection);
                    columnValue = cellDisplayedValue;

                    displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Owner.ListConnection.PresentationObjectType, Path + "." + DisplayMember as string, cell, out returnValueAsCollection);
                    columnText = displayedValue;

                }
                else
                {
                    cellDisplayedValue = UserInterfaceObjectConnection.GetDisplayedValue(row.PresentationObject, Owner.ListConnection.PresentationObjectType, _Path, cell, out returnValueAsCollection);
                    columnValue = cellDisplayedValue;
                    columnText = columnValue;
                }
                if (this is CheckBoxColumn && columnValue is bool && (bool)columnValue)
                {
                    cell.CheckState = CheckState.Checked;
                }


                if (columnText != null)
                {
                    cell.Data = columnValue;
                    cell.Text = columnText.ToString();


                    cell.DisplayedValue = cellDisplayedValue;
                    cell.DisplayedObjectMember = displayedValue;

                    return cell;

                }
                else
                {
                    cell.Data = columnValue;
                    cell.Text = "";

                    cell.DisplayedValue = cellDisplayedValue;
                    cell.DisplayedObjectMember = displayedValue;
                    return cell;
                }
            }
            catch (System.Exception error)
            {
                throw;
            }

        }


        /// <MetaDataID>{424ba824-65ef-499b-a9d6-dabfe0955894}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;
        /// <MetaDataID>{7b287153-4765-4bdd-afd2-5c75b545db59}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                if (_AllPaths == null)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    if (!string.IsNullOrEmpty(_DisplayMember))
                        _AllPaths.Add(_Path + '.' + _DisplayMember);
                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    if (!string.IsNullOrEmpty(_DisplayMember))
                        _AllPaths.Add(_Path + '.' + _DisplayMember);
                    return _AllPaths;
                }
                else
                    return _AllPaths;
            }
        }

        /// <MetaDataID>{49358dee-275d-4b27-9d28-e06ba060874c}</MetaDataID>
        public override string ToString()
        {
            return "List '" + columnModel.Table.Name + "' Column '" + Text + "'.";
        }


        /// <MetaDataID>{58996C5B-6CA0-49E1-B448-951436552CD8}</MetaDataID>
        public override ISite Site
        {
            get
            {
                if (columnModel != null && columnModel.Table != null && base.Site == null)
                    return columnModel.Table.Site;
                return base.Site;
            }
            set
            {
                base.Site = value;
            }
        }

        /// <MetaDataID>{B6BB98A3-B144-4291-9866-AFE2F8B04EAF}</MetaDataID>
        [Category("Column Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.EditMenuMetadata), typeof(System.Drawing.Design.UITypeEditor))]
        public object DesignMenu
        {
            get
            {
                return Menu;
            }
            set
            {
            }
        }

        /// <MetaDataID>{E950000D-2BB8-4217-BC9E-242747F0DE1B}</MetaDataID>
        ConnectableControls.Menus.MenuCommand _Menu = null;

        /// <MetaDataID>{C8652560-6562-4BD6-9807-FD78D57C8C51}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectableControls.Menus.MenuCommand Menu
        {
            get
            {
                if (_Menu == null)
                {

                    _Menu = new ConnectableControls.Menus.MenuCommand(_ColumnMetaData.Menu, this);
                    if (Site == null || !Site.DesignMode)
                    {
                        foreach (ConnectableControls.Menus.MenuCommand menuCommand in _Menu.GetAllMenuCommands())
                        {
                            menuCommand.Click += new EventHandler(MenuCommandClicked);

                        }

                    }
                }
                return _Menu;


            }
            set
            {
                //_Menu = value as MetaDataVaue;
            }
        }

        /// <MetaDataID>{3F8CCF73-C779-4749-831C-3ADEAB83A7F5}</MetaDataID>
        void MenuCommandClicked(object sender, EventArgs e)
        {
            ConnectableControls.Menus.MenuCommand menucommand = sender as ConnectableControls.Menus.MenuCommand;
            if (menucommand.OnCommandOperationCaller != null)
            {
                if (UserInterfaceObjectConnection == null)
                    return;
                menucommand.OnCommandOperationCaller.ExecuteOperationCall();
            }


            if (!string.IsNullOrEmpty(menucommand.ViewEditForm as string))
            {
                OOAdvantech.MetaDataRepository.Classifier formClassifier = UserInterfaceObjectConnection.GetClassifier(menucommand.ViewEditForm as string, true);
                foreach (OOAdvantech.MetaDataRepository.Attribute attribute in formClassifier.GetAttributes(true))
                {
                    if (attribute.Type.FullName == typeof(ConnectableControls.FormConnectionControl).FullName)
                    {

                        System.Reflection.PropertyInfo formConnectionControlProperty = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;
                        System.Reflection.FieldInfo formConnectionControlField = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                        if (formConnectionControlProperty != null || formConnectionControlField != null)
                        {
                            Type formType = formClassifier.GetExtensionMetaObject(typeof(Type)) as System.Type;
                            Form form = formType.Assembly.CreateInstance(formType.FullName) as Form;
                            ConnectableControls.FormConnectionControl formConnectionControl = null;

                            if (formConnectionControlProperty != null)
                                formConnectionControl = formConnectionControlProperty.GetValue(form, null) as ConnectableControls.FormConnectionControl;

                            if (formConnectionControlField != null)
                                formConnectionControl = formConnectionControlField.GetValue(form) as ConnectableControls.FormConnectionControl;

                            formConnectionControl.Instance = columnModel.Table.TableModel.Rows[columnModel.Table.LastMouseCell.Row].Cells[columnModel.Table.LastMouseCell.Column].Data;
                            formConnectionControl.ContainerControl = form;
                            System.Reflection.MethodInfo showDialogMethod = formType.GetMethod("ShowDialog", new Type[0]);
                            UserInterfaceObjectConnection.Invoke(form, showDialogMethod, new object[0], menucommand.TransactionOption);
                            columnModel.Table.TableModel.Rows[columnModel.Table.LastMouseCell.Row].Cells[columnModel.Table.LastMouseCell.Column].Data = formConnectionControl.Instance;


                            break;


                        }
                    }
                }
            }


            //}


        }
        /// <MetaDataID>{1579d98e-b3ee-4b43-a415-4a56ad467f40}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{b3996bbd-bd54-46ab-9617-e382c9ddc75a}</MetaDataID>
        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {

        }
        /// <MetaDataID>{7edaa70e-210f-45c3-b878-1abe2b33d02a}</MetaDataID>
        public void LockStateChange(object sender)
        {

        }


        #endregion




        /// <MetaDataID>{a74f86f0-b5d2-4a89-974b-0c0762c5701f}</MetaDataID>
        internal object GetPropertyValue(Cell cell, string propertyName)
        {
            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "CellValue")
                return Value;
            if (propertyName == "Value")
                return Value;
            if (propertyName == "RowValue")
                return cell.Row.PresentationObject;

            if (propertyName == "Text")
                return null;

            throw new Exception("There isn't property with name " + propertyName + ".");
            
        }
    }
}
