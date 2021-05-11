using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <MetaDataID>{a496fdfd-8e91-4518-a645-a06aebf2be69}</MetaDataID>
    public class PaintActionEventArgs : PaintEventArgs
    {
        public Rectangle CellDisplayArea;

        #region public SchedulerActionView ActionView
        private SchedulerActionView _ActionView;
        public SchedulerActionView ActionView
        {
            get
            {
                return _ActionView;
            }
            set
            {
                _ActionView = value;
            }
        } 
        #endregion

        #region public Font Font
        private Font _Font;
        public Font Font
        {
            get
            {
                return _Font;
            }

            set
            {
                _Font = value;
            }
        } 
        #endregion

        #region public Rectangle ActionRectangle
        private Rectangle _ActionRectangle;
        public Rectangle ActionRectangle
        {
            get
            {
                return _ActionRectangle;
            }
            set
            {
                _ActionRectangle = value;
            }
        } 
        #endregion

        #region public PaintActionEventArgs(Graphics g, Rectangle action_rect)
        public PaintActionEventArgs(Graphics g, SchedulerActionView action_view)
            : base(g, action_view.ActionRectangle)
        {
            ActionView = action_view;
            ActionRectangle = action_view.ActionRectangle;            
        } 
        #endregion
       
    }
}
