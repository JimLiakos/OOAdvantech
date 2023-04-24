using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{7322226c-2414-4817-adbb-2c1955982e5e}</MetaDataID>
    public interface IConnectedCommand
    {

        /// <MetaDataID>{3c7bc280-8d68-40c5-b25b-8f399afc3645}</MetaDataID>
        UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get;
            set;
        }

        /// <MetaDataID>{fa1217ea-f680-4560-8a70-7c97b2704694}</MetaDataID>
        string Name { get; }

    }
}
