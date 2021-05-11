using System;
using System.Collections.Generic;
using System.Text;
using ConnectableControls.SchedulerControl.RowDesign;

namespace ConnectableControls.SchedulerControl.TableModelDesign
{
    /// <MetaDataID>{3357297d-9c55-4828-9697-55e10c597243}</MetaDataID>
    public class Hour
    {
        public Hour(TableModel owner)
        {
            TableModel = owner;
        }

        private RowCollection _HourRows;
        public RowCollection HourRows
        {
            get
            {
                if (_HourRows == null)
                    _HourRows = new RowCollection(TableModel);

                return _HourRows;
            }
        }

        public TableModel TableModel;
    }
}
