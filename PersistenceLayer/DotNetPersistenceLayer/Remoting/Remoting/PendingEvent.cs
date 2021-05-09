using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{2e21b219-9f55-410c-ac71-89ea12bc4375}</MetaDataID>
    [System.Serializable]
    public class PendingEvent : System.Runtime.Remoting.Messaging.ILogicalThreadAffinative
    {
        
        public readonly bool Pending = false;
        public PendingEvent(bool pending)
        {
            Pending = pending;
        }
    }

}
