using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{e9d4ad96-42da-4934-a000-e4c364f73119}</MetaDataID>
    public interface IRemoteEventHandler
    { 
        /// <MetaDataID>{8c3167de-5529-4f34-9b10-5b0a89e17ca7}</MetaDataID>
        void ThereArePendingEvents(int pendingEventsHandlingCode);
    }

}
