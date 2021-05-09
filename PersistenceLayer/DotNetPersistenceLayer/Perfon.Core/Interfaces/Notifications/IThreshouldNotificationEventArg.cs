using System;

namespace Perfon.Interfaces.Notifications
{
    /// <summary>
    /// Agrument of Perf. Counter Threshold violation Notification
    /// </summary>
    /// <MetaDataID>{d60475b9-491d-4a75-ad3a-6538266a3331}</MetaDataID>
    public interface IThreshouldNotificationEventArg
    {
        string Message { get; }
    }
}
