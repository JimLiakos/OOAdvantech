using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl.TableModelDesign
{
    /// <summary>
    /// Specifies the part of the Table the user has clicked
    /// </summary>
    /// <MetaDataID>{f1b3e59e-f060-4626-9aeb-5c9c49e2e2a4}</MetaDataID>
    public enum TableRegion
    {
        /// <summary>
        /// A cell in the Table
        /// </summary>
        Cells = 1,

        /// <summary>
        /// A column header in the Table
        /// </summary>
        ColumnHeader = 2,

        /// <summary>
        /// The non-client area of a Table, such as the border
        /// </summary>
        NonClientArea = 3,

        /// <summary>
        /// The click occured outside ot the Table
        /// </summary>
        NoWhere = 4
    }
}
