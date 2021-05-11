using System;
using System.Collections.Generic;
using System.Text;
using ConnectableControls.SchedulerControl.Renderers;
using System.ComponentModel;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{450b4293-fb88-4ffa-8df0-a05a919e4f74}</MetaDataID>
    [DesignTimeVisible(false), ToolboxItem(false)]
    public class TimeColumn : Column
    {
        public override ConnectableControls.SchedulerControl.Renderers.ICellRenderer CreateDefaultRenderer()
        {
            return new TimeCellRenderer();
        }

        public override string GetDefaultRendererName()
        {
            return "TIME";
        }

        public TimeColumn(string text)
            : base(text)
        {
            
        }

        public TimeColumn(string text, int width)
            : base(text, width)
        {
           
        }
    }
}
