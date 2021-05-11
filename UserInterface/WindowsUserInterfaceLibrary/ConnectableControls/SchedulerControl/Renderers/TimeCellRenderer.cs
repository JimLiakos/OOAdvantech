using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{0952c723-2254-4f6f-8318-11ae7905adab}</MetaDataID>
    public class TimeCellRenderer : CellRenderer
    {
        protected override void OnPaint(ConnectableControls.SchedulerControl.Events.PaintCellEventArgs e)
        {
            base.OnPaint(e);

            // don't bother going any further if the Cell is null 
            if (e.Cell == null)
            {
                return;
            }

            string text = e.Cell.Text;
            string hm = "00";
            Font fhm = new Font(FontFamily.GenericSerif, 9);//"Arial"
            
            //string s1 = "12 pm";
            using (Brush brush = new LinearGradientBrush(e.CellRect, Color.WhiteSmoke, SystemColors.Control, 0.0f))
            {
                e.Graphics.FillRectangle(brush, e.CellRect);
                
                Font f = new Font(FontFamily.GenericSerif, 14,FontStyle.Bold);//"Arial"
                StringFormat str_form = new StringFormat(StringFormatFlags.NoWrap);
                str_form.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(text, f, Brushes.Gray, e.CellRect, str_form);

                SizeF sz = e.Graphics.MeasureString(text, f);
                int pos = (e.CellRect.Width / 2) + System.Convert.ToInt32((sz.Width / 2));

                if (!string.IsNullOrEmpty(text))
                {
                    e.Graphics.DrawString(hm, fhm, Brushes.Gray, e.CellRect.X + pos, e.CellRect.Y);
                }
                f.Dispose();
                fhm.Dispose();
            }
                        
        }

        /// <summary>
        /// Initializes a new instance of the TimeCellRenderer class with default settings
        /// </summary>
        public TimeCellRenderer()
            : base()
        {

        }
    }
}
