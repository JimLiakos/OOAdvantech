using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Menus
{
    /// <MetaDataID>{483F7457-1B11-4C2C-A4E3-B0DAA544B63B}</MetaDataID>
    class CreateMenuCommand:MenuCommand
    {
        /// <MetaDataID>{CC5437A7-30F7-44F5-8A77-06AEC627EA59}</MetaDataID>
        public CreateMenuCommand(string text)
            : base(text)
        {
            Enabled = false;
            
        }

    }
}
