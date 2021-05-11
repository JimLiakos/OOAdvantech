using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl.Events
{
    public delegate void ActionMouseEventHandler(SchedulerActionView sender, ActionMouseEventArgs e);

    /// <MetaDataID>{3d9f3148-0dc1-4cfb-89b3-3cb0a0c819ea}</MetaDataID>
    public class ActionMouseEventArgs : System.Windows.Forms.MouseEventArgs
    {
        public ActionMouseEventArgs(List<SchedulerActionView> action_views, System.Windows.Forms.MouseEventArgs mea)
            : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            ActionViews = action_views;
            if(Action==null)
                Action = ActionViews[0].Action;            
        }
        public bool ActionFocused = false;
        public List<SchedulerActionView> ActionViews;
        public ISchedulerAction Action;        
    }
}
