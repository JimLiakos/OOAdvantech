using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Remoting
{
   
    /// <MetaDataID>{e02e6994-5a52-4d9b-9f9f-bd9331c22ade}</MetaDataID>
    public class RemoteEventHandler:MarshalByRefObject, IRemoteEventHandler,IExtMarshalByRefObject
    {
        IRemoteEventHandler LocalHostProcessEventHandler;
        public RemoteEventHandler(IRemoteEventHandler localHostProcessEventHandler)
        {
            LocalHostProcessEventHandler = localHostProcessEventHandler;
        }
        public void ThereArePendingEvents(int pendingEventsHandlingCode)
        {
            LocalHostProcessEventHandler.ThereArePendingEvents(pendingEventsHandlingCode);
        }

    }
}
