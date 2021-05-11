using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using ConnectableControls.SchedulerControl.RowDesign;
using System.Runtime.InteropServices;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl
{
    /// <summary>
    /// Represents a Cell that is displayed in a Table
    /// </summary>
    /// <MetaDataID>{9FB45B0D-E6D6-49DE-97A1-62046A625666}</MetaDataID>
    [DesignTimeVisible(true)]//,TypeConverter(typeof(CellConverter))
    public class Cell : IDisposable
    {
        #region Constructors

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
        public Column Column;

        /// <MetaDataID>{3E3AE3DF-11D5-4EAE-B658-E51206496B6A}</MetaDataID>
        public Cell(object value, string text, Column column)
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


        ///// <summary>
        ///// Initializes a new instance of the Cell class with the specified text 
        ///// and check value
        ///// </summary>
        ///// <param name="text">The text displayed in the Cell</param>
        ///// <param name="check">Specifies whether the Cell is Checked</param>
        ///// <MetaDataID>{D18B1387-125C-4FB0-A169-C0B510949236}</MetaDataID>
        //public Cell(string text, bool check)
        //{
        //    this.Init();

        //    this.text = text;
        //    this.Checked = check;
        //}


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


        ///// <summary>
        ///// Initializes a new instance of the Cell class with the specified text, 
        ///// check value, fore Color, back Color and Font
        ///// </summary>
        ///// <param name="text">The text displayed in the Cell</param>
        ///// <param name="check">Specifies whether the Cell is Checked</param>
        ///// <param name="foreColor">The foreground Color of the Cell</param>
        ///// <param name="backColor">The background Color of the Cell</param>
        ///// <param name="font">The Font used to draw the text in the Cell</param>
        ///// <MetaDataID>{0FBBB6E3-9A01-4F14-868D-8F425B464738}</MetaDataID>
        //public Cell(string text, bool check, Color foreColor, Color backColor, Font font)
        //{
        //    this.Init();

        //    this.text = text;
        //    this.Checked = check;
        //    this.ForeColor = foreColor;
        //    this.BackColor = backColor;
        //    this.Font = font;
        //}


        ///// <summary>
        ///// Initializes a new instance of the Cell class with the specified text, 
        ///// check value and CellStyle
        ///// </summary>
        ///// <param name="text">The text displayed in the Cell</param>
        ///// <param name="check">Specifies whether the Cell is Checked</param>
        ///// <param name="cellStyle">A CellStyle that specifies the visual appearance 
        ///// of the Cell</param>
        ///// <MetaDataID>{1B8866D2-C940-4E9F-A6A0-6078B7761C48}</MetaDataID>
        //public Cell(string text, bool check, CellStyle cellStyle)
        //{
        //    this.Init();

        //    this.text = text;
        //    this.Checked = check;
        //    this.cellStyle = cellStyle;
        //}


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
            //this.checkStyle = null;
            this.imageStyle = null;
            this.tooltipText = null;

            this.state = (byte)(STATE_EDITABLE | STATE_ENABLED);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Occurs when the value of a Cells property changes
        /// </summary>
        public event CellEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region public string Text
        /// <summary>
        /// Gets or sets the text displayed by the Cell
        /// </summary>
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
        #endregion

        #region public object Data
        /// <summary>
        /// Gets or sets the Cells non-text data
        /// </summary>
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
        #endregion

        #region public object Tag
        /// <summary>
        /// Gets or sets the object that contains data about the Cell
        /// </summary>
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
        #endregion

        #region public CellStyle CellStyle
        /// <summary>
        /// Gets or sets the CellStyle used by the Cell
        /// </summary>
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
        #endregion

        #region public bool Selected
        /// <summary>
        /// Gets or sets whether the Cell is selected
        /// </summary>
        [Browsable(false)]
        public bool Selected
        {
            get
            {
                return this.GetState(STATE_SELECTED);
            }
        } 
        #endregion

        #region internal void SetSelected(bool selected)
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
        #endregion

        #region public Color BackColor
        /// <summary>
        /// Gets or sets the background Color for the Cell
        /// </summary>
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
        
        #endregion

        #region private bool ShouldSerializeBackColor()
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
        #endregion

        #region public Color ForeColor
        /// <summary>
        /// Gets or sets the foreground Color for the Cell
        /// </summary>
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
        #endregion

        #region private bool ShouldSerializeForeColor()
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
        #endregion

        #region public Font Font
        /// <summary>
        /// Gets or sets the Font used by the Cell
        /// </summary>
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
        #endregion

        #region private bool ShouldSerializeFont()
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
        
        #endregion

        #region public CellPadding Padding
        /// <summary>
        /// Gets or sets the amount of space between the Cells Border and its contents
        /// </summary>
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
        #endregion

        #region private bool ShouldSerializePadding()

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
        #endregion

        #region public bool Checked In Comments
        ///// <summary>
        ///// Gets or sets whether the Cell is in the checked state
        ///// </summary>
        //[Category("Appearance"),
        //DefaultValue(false),
        //Description("Indicates whether the cell is checked or unchecked"),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        //RefreshProperties(RefreshProperties.Repaint)]
        //public bool Checked
        //{
        //    get
        //    {
        //        if (this.checkStyle == null)
        //        {
        //            return false;
        //        }

        //        return this.checkStyle.Checked;
        //    }

        //    set
        //    {
        //        if (this.checkStyle == null)
        //        {
        //            this.checkStyle = new CellCheckStyle();
        //        }

        //        if (this.checkStyle.Checked != value)
        //        {
        //            bool oldCheck = this.Checked;

        //            this.checkStyle.Checked = value;

        //            this.OnPropertyChanged(new CellEventArgs(this, CellEventType.CheckStateChanged, oldCheck));
        //        }
        //    }
        //}
        
        #endregion

        #region public CheckState CheckState IN Comments
        ///// <summary>
        ///// Gets or sets the state of the Cells check box
        ///// </summary>
        //[Category("Appearance"),
        //DefaultValue(CheckState.Unchecked),
        //Description("Indicates the state of the cells check box"),
        //RefreshProperties(RefreshProperties.Repaint)]
        //public CheckState CheckState
        //{
        //    get
        //    {
        //        if (this.checkStyle == null)
        //        {
        //            return CheckState.Unchecked;
        //        }

        //        return this.checkStyle.CheckState;
        //    }

        //    set
        //    {
        //        if (!Enum.IsDefined(typeof(CheckState), value))
        //        {
        //            throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
        //        }

        //        if (this.checkStyle == null)
        //        {
        //            this.checkStyle = new CellCheckStyle();
        //        }

        //        if (this.checkStyle.CheckState != value)
        //        {
        //            CheckState oldCheckState = this.CheckState;

        //            this.checkStyle.CheckState = value;

        //            this.OnPropertyChanged(new CellEventArgs(this, CellEventType.CheckStateChanged, oldCheckState));
        //        }
        //    }
        //} 
        #endregion

        #region public bool ThreeState In comments
        ///// <summary>
        ///// Gets or sets a value indicating whether the Cells check box 
        ///// will allow three check states rather than two
        ///// </summary>
        //[Category("Appearance"),
        //DefaultValue(false),
        //Description("Controls whether or not the user can select the indeterminate state of the cells check box"),
        //RefreshProperties(RefreshProperties.Repaint)]
        //public bool ThreeState
        //{
        //    get
        //    {
        //        if (this.checkStyle == null)
        //        {
        //            return false;
        //        }

        //        return this.checkStyle.ThreeState;
        //    }

        //    set
        //    {
        //        if (this.checkStyle == null)
        //        {
        //            this.checkStyle = new CellCheckStyle();
        //        }

        //        if (this.checkStyle.ThreeState != value)
        //        {
        //            bool oldThreeState = this.ThreeState;

        //            this.checkStyle.ThreeState = value;

        //            this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ThreeStateChanged, oldThreeState));
        //        }
        //    }
        //} 
        #endregion

        #region public Image Image
        /// <summary>
        /// Gets or sets the image that is displayed in the Cell
        /// </summary>
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
                }

                if (this.imageStyle.Image != value)
                {
                    Image oldImage = this.Image;

                    this.imageStyle.Image = value;

                    this.OnPropertyChanged(new CellEventArgs(this, CellEventType.ImageChanged, oldImage));
                }
            }
        } 
        #endregion

        #region public ImageSizeMode ImageSizeMode
        /// <summary>
        /// Gets or sets how the Cells image is sized within the Cell
        /// </summary>
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
        
        #endregion

        #region public bool Editable
        /// <summary>
        /// Gets or sets a value indicating whether the Cells contents are able 
        /// to be edited
        /// </summary>
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
        
        #endregion

        #region private bool ShouldSerializeEditable()
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
        #endregion

        #region public bool Enabled
        /// <summary>
        /// Gets or sets a value indicating whether the Cell 
        /// can respond to user interaction
        /// </summary>
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
        #endregion

        #region private bool ShouldSerializeEnabled()
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
        #endregion

        #region public string ToolTipText
        /// <summary>
        /// Gets or sets the text displayed in the Cells tooltip
        /// </summary>
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
        #endregion

        #region protected internal object RendererData
        /// <summary>
        /// Gets or sets the information used by CellRenderers to record the current 
        /// state of the Cell
        /// </summary>
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
        #endregion

        #region public Row Row
        /// <summary>
        /// Gets the Row that the Cell belongs to
        /// </summary>
        [Browsable(false)]
        public Row Row
        {
            get
            {
                return this.row;
            }
        } 
        #endregion

        #region internal Row InternalRow
        /// <summary>
        /// Gets or sets the Row that the Cell belongs to
        /// </summary>
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
        #endregion

        #region public int Index
        /// <summary>
        /// Gets the index of the Cell within its Row
        /// </summary>
        [Browsable(false)]
        public int Index
        {
            get
            {
                return this.index;
            }
        }
        
        #endregion

        #region internal int InternalIndex
        /// <summary>
        /// Gets or sets the index of the Cell within its Row
        /// </summary>
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
        #endregion

        #region protected internal bool CanRaiseEvents
        /// <summary>
        /// Gets whether the Cell is able to raise events
        /// </summary>
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

        #endregion

        #region IDisposable Members

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
                //this.checkStyle = null;
                this.imageStyle = null;
                this.tooltipText = null;

                this.state = (byte)0;

                this.disposed = true;
            }
        }

        #endregion

        #region internal bool GetState(int flag)
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
        #endregion

        #region internal void SetState(int flag, bool value)
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

        #region Class Data

        // Cell state flags
        private static readonly int STATE_EDITABLE = 1;
        private static readonly int STATE_ENABLED = 2;
        private static readonly int STATE_SELECTED = 4;

        /// <summary>
        /// The text displayed in the Cell
        /// </summary>
        private string text;

        /// <summary>
        /// An object that contains data to be displayed in the Cell
        /// </summary>
        private object data;

        /// <summary>
        /// An object that contains data about the Cell
        /// </summary>
        private object tag;

        /// <summary>
        /// Stores information used by CellRenderers to record the current 
        /// state of the Cell
        /// </summary>
        private object rendererData;

        /// <summary>
        /// The Row that the Cell belongs to
        /// </summary>
        private Row row;

        /// <summary>
        /// The index of the Cell
        /// </summary>
        private int index;

        /// <summary>
        /// Contains the current state of the the Cell
        /// </summary>
        private byte state;

        /// <summary>
        /// The Cells CellStyle settings
        /// </summary>
        private CellStyle cellStyle;

        ///// <summary>
        ///// The Cells CellCheckStyle settings
        ///// </summary>
        //private CellCheckStyle checkStyle;

        /// <summary>
        /// The Cells CellImageStyle settings
        /// </summary>
        private CellImageStyle imageStyle;

        /// <summary>
        /// The text displayed in the Cells tooltip
        /// </summary>
        private string tooltipText;

        /// <summary>
        /// Specifies whether the Cell has been disposed
        /// </summary>
        private bool disposed = false;

        #endregion

        #region protected virtual void OnPropertyChanged(CellEventArgs e)
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
    }

    #region public enum CellEventType
    /// <summary>
    /// Specifies the type of event generated when the value of a 
    /// Cell's property changes
    /// </summary>
    /// <MetaDataID>{4ab41006-69ba-4ba7-9430-d029c155096d}</MetaDataID>
    public enum CellEventType
    {
        /// <summary>
        /// Occurs when the Cell's property change type is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Occurs when the value displayed by a Cell has changed
        /// </summary>
        ValueChanged = 1,

        /// <summary>
        /// Occurs when the value of a Cell's Font property changes
        /// </summary>
        FontChanged = 2,

        /// <summary>
        /// Occurs when the value of a Cell's BackColor property changes
        /// </summary>
        BackColorChanged = 3,

        /// <summary>
        /// Occurs when the value of a Cell's ForeColor property changes
        /// </summary>
        ForeColorChanged = 4,

        /// <summary>
        /// Occurs when the value of a Cell's CellStyle property changes
        /// </summary>
        StyleChanged = 5,

        /// <summary>
        /// Occurs when the value of a Cell's Padding property changes
        /// </summary>
        PaddingChanged = 6,

        /// <summary>
        /// Occurs when the value of a Cell's Editable property changes
        /// </summary>
        EditableChanged = 7,

        /// <summary>
        /// Occurs when the value of a Cell's Enabled property changes
        /// </summary>
        EnabledChanged = 8,

        /// <summary>
        /// Occurs when the value of a Cell's ToolTipText property changes
        /// </summary>
        ToolTipTextChanged = 9,

        /// <summary>
        /// Occurs when the value of a Cell's CheckState property changes
        /// </summary>
        CheckStateChanged = 10,

        /// <summary>
        /// Occurs when the value of a Cell's ThreeState property changes
        /// </summary>
        ThreeStateChanged = 11,

        /// <summary>
        /// Occurs when the value of a Cell's Image property changes
        /// </summary>
        ImageChanged = 12,

        /// <summary>
        /// Occurs when the value of a Cell's ImageSizeMode property changes
        /// </summary>
        ImageSizeModeChanged = 13
    } 
    #endregion

    #region CellPadding

    /// <summary>
    /// Specifies the amount of space between the border and any contained 
    /// items along each edge of an object
    /// </summary>
    /// <MetaDataID>{cd06487b-52b4-46c9-bb79-8332d55dae6c}</MetaDataID>
    [Serializable(),StructLayout(LayoutKind.Sequential), TypeConverter(typeof(CellPaddingConverter))]
    public struct CellPadding
    {
        #region Class Data

        /// <summary>
        /// Represents a Padding structure with its properties 
        /// left uninitialized
        /// </summary>
        public static readonly CellPadding Empty = new CellPadding(0, 0, 0, 0);

        /// <summary>
        /// The width of the left padding
        /// </summary>
        private int left;

        /// <summary>
        /// The width of the right padding
        /// </summary>
        private int right;

        /// <summary>
        /// The width of the top padding
        /// </summary>
        private int top;

        /// <summary>
        /// The width of the bottom padding
        /// </summary>
        private int bottom;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Padding class
        /// </summary>
        /// <param name="left">The width of the left padding value</param>
        /// <param name="top">The height of top padding value</param>
        /// <param name="right">The width of the right padding value</param>
        /// <param name="bottom">The height of bottom padding value</param>
        public CellPadding(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests whether obj is a CellPadding structure with the same values as 
        /// this Padding structure
        /// </summary>
        /// <param name="obj">The Object to test</param>
        /// <returns>This method returns true if obj is a CellPadding structure 
        /// and its Left, Top, Right, and Bottom properties are equal to 
        /// the corresponding properties of this CellPadding structure; 
        /// otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CellPadding))
            {
                return false;
            }

            CellPadding padding = (CellPadding)obj;

            if (((padding.Left == this.Left) && (padding.Top == this.Top)) && (padding.Right == this.Right))
            {
                return (padding.Bottom == this.Bottom);
            }

            return false;
        }


        /// <summary>
        /// Returns the hash code for this CellPadding structure
        /// </summary>
        /// <returns>An integer that represents the hashcode for this 
        /// padding</returns>
        public override int GetHashCode()
        {
            return (((this.Left ^ ((this.Top << 13) | (this.Top >> 0x13))) ^ ((this.Right << 0x1a) | (this.Right >> 6))) ^ ((this.Bottom << 7) | (this.Bottom >> 0x19)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the left padding value
        /// </summary>
        public int Left
        {
            get
            {
                return this.left;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Padding value cannot be negative");
                }

                this.left = value;
            }
        }


        /// <summary>
        /// Gets or sets the width of the right padding value
        /// </summary>
        public int Right
        {
            get
            {
                return this.right;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Padding value cannot be negative");
                }

                this.right = value;
            }
        }


        /// <summary>
        /// Gets or sets the height of the top padding value
        /// </summary>
        public int Top
        {
            get
            {
                return this.top;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Padding value cannot be negative");
                }

                this.top = value;
            }
        }


        /// <summary>
        /// Gets or sets the height of the bottom padding value
        /// </summary>
        public int Bottom
        {
            get
            {
                return this.bottom;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Padding value cannot be negative");
                }

                this.bottom = value;
            }
        }


        /// <summary>
        /// Tests whether all numeric properties of this CellPadding have 
        /// values of zero
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (((this.Left == 0) && (this.Top == 0)) && (this.Right == 0))
                {
                    return (this.Bottom == 0);
                }

                return false;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Tests whether two CellPadding structures have equal Left, Top, 
        /// Right, and Bottom properties
        /// </summary>
        /// <param name="left">The CellPadding structure that is to the left 
        /// of the equality operator</param>
        /// <param name="right">The CellPadding structure that is to the right 
        /// of the equality operator</param>
        /// <returns>This operator returns true if the two CellPadding structures 
        /// have equal Left, Top, Right, and Bottom properties</returns>
        public static bool operator ==(CellPadding left, CellPadding right)
        {
            if (((left.Left == right.Left) && (left.Top == right.Top)) && (left.Right == right.Right))
            {
                return (left.Bottom == right.Bottom);
            }

            return false;
        }


        /// <summary>
        /// Tests whether two CellPadding structures differ in their Left, Top, 
        /// Right, and Bottom properties
        /// </summary>
        /// <param name="left">The CellPadding structure that is to the left 
        /// of the equality operator</param>
        /// <param name="right">The CellPadding structure that is to the right 
        /// of the equality operator</param>
        /// <returns>This operator returns true if any of the Left, Top, Right, 
        /// and Bottom properties of the two CellPadding structures are unequal; 
        /// otherwise false</returns>
        public static bool operator !=(CellPadding left, CellPadding right)
        {
            return !(left == right);
        }

        #endregion
    }

    #endregion
}
