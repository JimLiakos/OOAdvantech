using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <summary>
    /// Base class for Renderers that draw Column headers
    /// </summary>
    /// <MetaDataID>{2E63B028-021E-470B-A223-BD7366F05699}</MetaDataID>
    public abstract class HeaderRenderer : Renderer, IHeaderRenderer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HeaderRenderer class with default settings
        /// </summary>
        /// <MetaDataID>{9426BFBF-C832-45EF-BAB6-2172001460FD}</MetaDataID>
        protected HeaderRenderer()
            : base()
        {
            this.StringFormat.Alignment = StringAlignment.Near;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a Rectangle that represents the size and location of the Image 
        /// displayed on the ColumnHeader
        /// </summary>
        /// <returns>A Rectangle that represents the size and location of the Image 
        /// displayed on the ColumnHeader</returns>
        /// <MetaDataID>{EA2E6CB2-F948-4F3D-8F2A-3F181A25FF40}</MetaDataID>
        protected Rectangle CalcImageRect()
        {
            Rectangle imageRect = this.ClientRectangle;

            if (imageRect.Width > 16)
            {
                imageRect.Width = 16;
            }

            if (imageRect.Height > 16)
            {
                imageRect.Height = 16;

                imageRect.Y += (this.ClientRectangle.Height - imageRect.Height) / 2;
            }

            return imageRect;
        }


        /// <summary>
        /// Returns a Rectangle that represents the size and location of the sort arrow
        /// </summary>
        /// <returns>A Rectangle that represents the size and location of the sort arrow</returns>
        /// <MetaDataID>{BAB63532-4E48-4777-A1A0-2534043BE8A9}</MetaDataID>
        protected Rectangle CalcSortArrowRect()
        {
            Rectangle arrowRect = this.ClientRectangle;

            arrowRect.Width = 12;
            arrowRect.X = this.ClientRectangle.Right - arrowRect.Width;

            return arrowRect;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Overrides Renderer.ClientRectangle
        /// </summary>
        [Browsable(false)]
        public override Rectangle ClientRectangle
        {
            get
            {
                Rectangle client = new Rectangle(this.Bounds.Location, this.Bounds.Size);

                //
                client.Inflate(-2, -2);

                return client;
            }
        }

        #endregion

        #region Events

        #region Mouse

        #region MouseEnter

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{3B607C65-43E1-4ED7-A24B-5A65DE8FE5A3}</MetaDataID>
        public virtual void OnMouseEnter(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;

            bool tooltipActive = e.Table.ToolTip.Active;

            if (tooltipActive)
            {
                e.Table.ToolTip.Active = false;
            }

            e.Table.ResetMouseEventArgs();

            e.Table.ToolTip.SetToolTip(e.Table, e.Column.ToolTipText);

            if (tooltipActive)
            {
                e.Table.ToolTip.Active = true;
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{162570BE-8F84-4A03-A153-E4A685A27F27}</MetaDataID>
        public virtual void OnMouseLeave(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{CC3C41C8-8EF7-4C6F-B851-65B024F0ECC9}</MetaDataID>
        public virtual void OnMouseUp(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{FF80B213-871E-4C0E-B682-6F9699986A97}</MetaDataID>
        public virtual void OnMouseDown(HeaderMouseEventArgs e)
        {
            if (!e.Table.Focused)
            {
                e.Table.Focus();
            }

            this.Bounds = e.HeaderRect;
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7DA3F5FA-81F0-4B01-BAE3-720EEC0230B3}</MetaDataID>
        public virtual void OnMouseMove(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{D38AA7D8-E7C8-4BA5-840A-7B8C1640BCD7}</MetaDataID>
        public virtual void OnClick(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;
        }


        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{45F8711A-3906-4AEB-8BCA-AA3380DFAF02}</MetaDataID>
        public virtual void OnDoubleClick(HeaderMouseEventArgs e)
        {
            this.Bounds = e.HeaderRect;
        }

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Raises the PaintHeader event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{8F50E7F2-E14C-4668-9236-D9983AE8C94D}</MetaDataID>
        public virtual void OnPaintHeader(PaintHeaderEventArgs e)
        {
            // paint the Column header's background
            this.OnPaintBackground(e);

            // paint the Column headers foreground
            this.OnPaint(e);
        }


        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{E5B3B645-CB46-42CD-8B6F-602BBB0C261D}</MetaDataID>
        protected virtual void OnPaintBackground(PaintHeaderEventArgs e)
        {

        }


        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{4A9B60AB-DB5C-444E-9A66-3D171C1C21DC}</MetaDataID>
        protected virtual void OnPaint(PaintHeaderEventArgs e)
        {

        }


        /// <summary>
        /// Draws the Image contained in the ColumnHeader
        /// </summary>
        /// <param name="g">The Graphics used to paint the Image</param>
        /// <param name="image">The Image to be drawn</param>
        /// <param name="imageRect">A rectangle that specifies the Size and 
        /// Location of the Image</param>
        /// <param name="enabled">Specifies whether the Image should be drawn 
        /// in an enabled state</param>
        /// <MetaDataID>{438D4406-FCDA-4DCA-B2C7-1F4A4B961E99}</MetaDataID>
        protected void DrawColumnHeaderImage(Graphics g, Image image, Rectangle imageRect, bool enabled)
        {
            if (enabled)
            {
                g.DrawImage(image, imageRect);
            }
            //else
            //{
            //    using (Image im = new Bitmap(image, imageRect.Width, imageRect.Height))
            //    {
            //        ControlPaint.DrawImageDisabled(g, im, imageRect.X, imageRect.Y, this.BackBrush.Color);
            //    }
            //}
        }


        /// <summary>
        /// Draws the ColumnHeader's sort arrow
        /// </summary>
        /// <param name="g">The Graphics to draw on</param>
        /// <param name="drawRect">A Rectangle that specifies the location 
        /// of the sort arrow</param>
        /// <param name="direction">The direction of the sort arrow</param>
        /// <param name="enabled">Specifies whether the sort arrow should be 
        /// drawn in an enabled state</param>
        /// <MetaDataID>{B1E62D80-5B2F-441F-888C-BD254AF8ACC6}</MetaDataID>
        protected virtual void DrawSortArrow(Graphics g, Rectangle drawRect, SortOrder direction, bool enabled)
        {
            //if (direction != SortOrder.None)
            //{
            //    using (Font font = new Font("Marlett", 9f))
            //    {
            //        using (StringFormat format = new StringFormat())
            //        {
            //            format.Alignment = StringAlignment.Far;
            //            format.LineAlignment = StringAlignment.Center;

            //            if (direction == SortOrder.Ascending)
            //            {
            //                if (enabled)
            //                {
            //                    g.DrawString("t", font, SystemBrushes.ControlDarkDark, drawRect, format);
            //                }
            //                else
            //                {
            //                    using (SolidBrush brush = new SolidBrush(SystemPens.GrayText.Color))
            //                    {
            //                        g.DrawString("t", font, brush, drawRect, format);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (enabled)
            //                {
            //                    g.DrawString("u", font, SystemBrushes.ControlDarkDark, drawRect, format);
            //                }
            //                else
            //                {
            //                    using (SolidBrush brush = new SolidBrush(SystemPens.GrayText.Color))
            //                    {
            //                        g.DrawString("u", font, brush, drawRect, format);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        #endregion

        #endregion
    }
}
