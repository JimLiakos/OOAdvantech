using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ConnectableControls.SchedulerControl.Events;
using System.Drawing.Drawing2D;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{cd4f3814-b2be-42f0-a976-a6181670796f}</MetaDataID>
    public class XPHeaderRenderer : HeaderRenderer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the XPHeaderRenderer class 
        /// with default settings
        /// </summary>
        /// <MetaDataID>{8748C125-54E1-4AB2-8EDE-04241C2526EF}</MetaDataID>
        public XPHeaderRenderer()
            : base()
        {

        }

        #endregion

        #region Events

        #region Paint

        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{3CFEFE30-E88B-4DA1-B8CA-AD4FE2714154}</MetaDataID>
        protected override void OnPaintBackground(PaintHeaderEventArgs e)
        {
            base.OnPaintBackground(e);
            using (LinearGradientBrush brush = new LinearGradientBrush(e.HeaderRect, Color.WhiteSmoke, SystemColors.Control, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, e.HeaderRect);
                e.Graphics.DrawRectangle(Pens.Brown, e.HeaderRect);
            }           
        }


        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{7FF48EEA-8215-4C2E-8E5E-AB2D2DE2853E}</MetaDataID>
        protected override void OnPaint(PaintHeaderEventArgs e)
        {
            base.OnPaint(e);

            if (e.Column == null || (e.Column as DayColumn) == null)
            {
                return;
            }
            if (e.Column.Text == null)
            {
                return;
            }

            if ((e.Column as DayColumn).DetailDayViewRectange.IsEmpty)
                return;

            Rectangle textRect = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height - (e.Column as DayColumn).DetailDayViewRectange.Height);

            #region Image paint
            Rectangle imageRect = (e.Column as DayColumn).DetailDayViewRectange; // Rectangle.Empty;

            if (e.Column.ZoomInImage != null)
            {
                if (e.Column.Parent.Table.TableView == TableViewState.DayView)
                    e.Graphics.DrawImage(e.Column.ZoomOutImage, imageRect);
                else
                    e.Graphics.DrawImage(e.Column.ZoomInImage, imageRect);
                //imageRect = this.CalcImageRect();

                //textRect.Width -= imageRect.Width;
                //textRect.X += imageRect.Width;

                //if (e.Column.ImageOnRight)
                //{
                //    imageRect.X = this.ClientRectangle.Right - imageRect.Width;
                //    textRect.X = this.ClientRectangle.X;
                //}

                //if (!ThemeManager.VisualStylesEnabled && e.Column.ColumnState == ColumnState.Pressed)
                //{
                //    imageRect.X += 1;
                //    imageRect.Y += 1;
                //}

                //this.DrawColumnHeaderImage(e.Graphics, e.Column.DetailDayImage, imageRect, e.Column.Enabled);
            }
            
            #endregion

            #region Text Paint
            if (e.Column.Text.Length > 0 && textRect.Width > 0)
            {
                this.StringFormat.Alignment = StringAlignment.Center;
                SizeF string_size = e.Graphics.MeasureString(e.Column.Text, this.Font);
                if (string_size.Width > textRect.Width)
                {
                    string[] datebreak = e.Column.Text.Split(new char[] { ' ' });
                    int commaint = datebreak[0].IndexOf(",");
                    string day3 = string.Empty;
                    if (commaint == -1)
                        day3 = datebreak[0].Substring(0, 3);
                    else
                        day3 = datebreak[0].Remove(3, commaint - 3);

                    string mon3 = datebreak[2].Substring(0, 3);
                    string total = day3 + " " + datebreak[1] + " " + mon3 + " " + datebreak[3];

                    e.Graphics.DrawString(total, this.Font, this.ForeBrush, textRect, this.StringFormat);
                }
                else
                    e.Graphics.DrawString(e.Column.Text, this.Font, this.ForeBrush, textRect, this.StringFormat);
            } 
            #endregion

            //using (LinearGradientBrush brush = new LinearGradientBrush((e.Column as DayColumn).DetailDayViewRectange, Color.AliceBlue, Color.AliceBlue, LinearGradientMode.Vertical))
            //{
            //    e.Graphics.FillRectangle(brush, (e.Column as DayColumn).DetailDayViewRectange);
            //    e.Graphics.DrawRectangle(Pens.Brown, (e.Column as DayColumn).DetailDayViewRectange);
            //}
        }

        #endregion

        #endregion
    }
}
