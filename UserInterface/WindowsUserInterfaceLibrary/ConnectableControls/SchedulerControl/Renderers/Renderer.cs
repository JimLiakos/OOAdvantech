using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ConnectableControls.SchedulerControl.RowDesign;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{fac4d452-929c-4b97-84dc-1c82ddaadef3}</MetaDataID>
    public abstract class Renderer : IRenderer, IDisposable
    {
        #region Class Data

        /// <summary>
        /// A StringFormat object that specifies how the Renderers 
        /// contents are drawn
        /// </summary>
        private StringFormat stringFormat;

        /// <summary>
        /// The brush used to draw the Renderers background
        /// </summary>
        private SolidBrush backBrush;

        /// <summary>
        /// The brush used to draw the Renderers foreground
        /// </summary>
        private SolidBrush foreBrush;

        /// <summary>
        /// A Rectangle that specifies the size and location of the Renderer
        /// </summary>
        private Rectangle bounds;

        /// <summary>
        /// The Font of the text displayed by the Renderer
        /// </summary>
        private Font font;

        /// <summary>
        /// The width of a Cells border
        /// </summary>
        protected static int BorderWidth = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Renderer class with default settings
        /// </summary>
        /// <MetaDataID>{23A0D3CC-45BC-41BA-8B90-4ED01E3A48A7}</MetaDataID>
        protected Renderer()
        {
            this.bounds = Rectangle.Empty;
            this.font = null;

            this._StringFormat = new StringFormat();
            this._StringFormat.LineAlignment = StringAlignment.Center;
            this._StringFormat.Alignment = StringAlignment.Near;
            //this._StringFormat.FormatFlags = StringFormatFlags.NoWrap;
            this._StringFormat.Trimming = StringTrimming.EllipsisCharacter;

            this.backBrush = new SolidBrush(Color.Transparent);
            this.foreBrush = new SolidBrush(Color.Black);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases the unmanaged resources used by the Renderer and 
        /// optionally releases the managed resources
        /// </summary>
        /// <MetaDataID>{13DE0B99-E973-4AB4-A238-3689401E040D}</MetaDataID>
        public virtual void Dispose()
        {
            if (this.backBrush != null)
            {
                this.backBrush.Dispose();
                this.backBrush = null;
            }

            if (this.foreBrush != null)
            {
                this.foreBrush.Dispose();
                this.foreBrush = null;
            }
        }


        /// <summary>
        /// Sets the color of the brush used to draw the background
        /// </summary>
        /// <param name="color">The color of the brush</param>
        /// <MetaDataID>{BA2B7ECD-A03A-43D3-B6D9-F9675269944C}</MetaDataID>
        protected void SetBackBrushColor(Color color)
        {
            if (this.BackBrush.Color != color)
            {
                this.BackBrush.Color = color;
            }
        }


        /// <summary>
        /// Sets the color of the brush used to draw the foreground
        /// </summary>
        /// <param name="color">The color of the brush</param>
        /// <MetaDataID>{58035991-78EF-4D10-A05E-D4449377AC51}</MetaDataID>
        protected void SetForeBrushColor(Color color)
        {
            if (this.ForeBrush.Color != color)
            {
                this.ForeBrush.Color = color;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the rectangle that represents the client area of the Renderer
        /// </summary>
        [Browsable(false)]
        public abstract Rectangle ClientRectangle
        {
            get;
        }


        /// <summary>
        /// Gets or sets the size and location of the Renderer
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }

            set
            {
                this.bounds = value;
            }
        }


        /// <summary>
        /// Gets or sets the font of the text displayed by the Renderer
        /// </summary>
        [Category("Appearance"),
        Description("The font used to draw the text")]
        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                if (value == null)
                {
                    value = Control.DefaultFont;
                }

                if (this.font != value)
                {
                    this.font = value;
                }
            }
        }


        /// <summary>
        /// Gets the brush used to draw the Renderers background
        /// </summary>
        protected SolidBrush BackBrush
        {
            get
            {
                return this.backBrush;
            }
        }


        /// <summary>
        /// Gets the brush used to draw the Renderers foreground
        /// </summary>
        protected SolidBrush ForeBrush
        {
            get
            {
                return this.foreBrush;
            }
        }


        /// <summary>
        /// Gets or sets the foreground Color of the Renderer
        /// </summary>
        [Category("Appearance"),
        Description("The foreground color used to display text and graphics")]
        public Color ForeColor
        {
            get
            {
                return this.ForeBrush.Color;
            }

            set
            {
                this.SetForeBrushColor(value);
            }
        }


        /// <summary>
        /// Gets or sets the background Color of the Renderer
        /// </summary>
        [Category("Appearance"),
        Description("The background color used to display text and graphics")]
        public Color BackColor
        {
            get
            {
                return this.BackBrush.Color;
            }

            set
            {
                this.SetBackBrushColor(value);
            }
        }

        
        private StringFormat _StringFormat;
        /// <summary>
        /// Gets or sets a StringFormat object that specifies how the Renderers 
        /// contents are drawn
        /// </summary>
        protected StringFormat StringFormat
        {
            get
            {
                return this._StringFormat;
            }

            set
            {
                this._StringFormat = value;
            }
        }


        /// <summary>
        /// Gets or sets a StringTrimming enumeration that indicates how text that 
        /// is drawn by the Renderer is trimmed when it exceeds the edges of the 
        /// layout rectangle
        /// </summary>
        [Browsable(false)]
        public StringTrimming Trimming
        {
            get
            {
                return this._StringFormat.Trimming;
            }

            set
            {
                this._StringFormat.Trimming = value;
            }
        }


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned horizontally
        /// </summary>
        [Browsable(false)]
        public ColumnAlignment Alignment
        {
            get
            {
                switch (this._StringFormat.Alignment)
                {
                    case StringAlignment.Near:
                        return ColumnAlignment.Left;

                    case StringAlignment.Center:
                        return ColumnAlignment.Center;

                    case StringAlignment.Far:
                        return ColumnAlignment.Right;
                }

                return ColumnAlignment.Left;
            }

            set
            {
                switch (value)
                {
                    case ColumnAlignment.Left:
                        this._StringFormat.Alignment = StringAlignment.Near;
                        break;

                    case ColumnAlignment.Center:
                        this._StringFormat.Alignment = StringAlignment.Center;
                        break;

                    case ColumnAlignment.Right:
                        this._StringFormat.Alignment = StringAlignment.Far;
                        break;
                }
            }
        }


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned vertically
        /// </summary>
        [Browsable(false)]
        public RowAlignment LineAlignment
        {
            get
            {
                switch (this._StringFormat.LineAlignment)
                {
                    case StringAlignment.Near:
                        return RowAlignment.Top;

                    case StringAlignment.Center:
                        return RowAlignment.Center;

                    case StringAlignment.Far:
                        return RowAlignment.Bottom;
                }

                return RowAlignment.Center;
            }

            set
            {
                switch (value)
                {
                    case RowAlignment.Top:
                        this._StringFormat.LineAlignment = StringAlignment.Near;
                        break;

                    case RowAlignment.Center:
                        this._StringFormat.LineAlignment = StringAlignment.Center;
                        break;

                    case RowAlignment.Bottom:
                        this._StringFormat.LineAlignment = StringAlignment.Far;
                        break;
                }
            }
        }


        /// <summary>
        /// Gets whether Visual Styles are enabled for the application
        /// </summary>
        protected bool VisualStylesEnabled
        {
            get
            {
                throw new NotImplementedException("VisualStylesEnabled is not implemented");
                //return ThemeManager.VisualStylesEnabled;
            }
        }

        #endregion
    }    
}
