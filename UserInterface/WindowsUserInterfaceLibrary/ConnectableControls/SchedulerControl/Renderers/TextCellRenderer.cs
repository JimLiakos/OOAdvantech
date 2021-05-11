using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{30f6745f-2780-49ff-af99-cd50db74569e}</MetaDataID>
    public class TextCellRenderer : CellRenderer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TextCellRenderer class with 
        /// default settings
        /// </summary>
        /// <MetaDataID>{F98A1F08-A23D-4290-AB3D-F7D50E93031F}</MetaDataID>
        public TextCellRenderer()
            : base()
        {

        }

        #endregion

        #region Events

        #region Paint

        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{2EE62925-F905-455D-8DC6-55831FFF5A58}</MetaDataID>
        protected override void OnPaint(PaintCellEventArgs e)
        {
            base.OnPaint(e);

            // don't bother going any further if the Cell is null 
            if (e.Cell == null)
            {
                return;
            }

            string text = e.Cell.Text;

            if (text != null && text.Length != 0)
            {
                if (e.Enabled)
                {
                    e.Graphics.DrawString(text, this.Font, this.ForeBrush, this.ClientRectangle, this.StringFormat);
                }
                else
                {
                    e.Graphics.DrawString(text, this.Font, this.GrayTextBrush, this.ClientRectangle, this.StringFormat);
                }
            }

            if (e.Focused && e.Enabled)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
            }
        }

        #endregion

        #endregion
    }
}
