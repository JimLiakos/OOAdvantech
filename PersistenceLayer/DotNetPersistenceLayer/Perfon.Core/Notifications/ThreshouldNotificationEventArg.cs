using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfon.Interfaces.Notifications;

namespace Perfon.Core.Notifications
{
    /// <summary>
    /// Event arg for Notification events
    /// </summary>
    /// <MetaDataID>{84ecfc44-c72e-4e02-852e-31d9018b1928}</MetaDataID>
    public class ThreshouldNotificationEventArg : EventArgs, IThreshouldNotificationEventArg
    {
        public string Message { get; private set; }

        public ThreshouldNotificationEventArg(string message)
        {
            Message = message;
        }
    }
}
