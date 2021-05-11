using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{ff31a16d-8101-4e6d-93c6-a6c3ba222207}</MetaDataID>
    public class LongActionsRectRenderer : HeaderRenderer
    {
        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{3CFEFE30-E88B-4DA1-B8CA-AD4FE2714154}</MetaDataID>
        protected override void OnPaintBackground(PaintHeaderEventArgs e)
        {
            base.OnPaintBackground(e);
            if (e.DayRect.IsEmpty)
                return;
            using (LinearGradientBrush brush = new LinearGradientBrush(e.DayRect, Color.Silver, Color.Silver, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, e.DayRect);
                e.Graphics.DrawRectangle(Pens.Brown, e.DayRect);
            }
        }

    }
}
