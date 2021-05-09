using System;

namespace Perfon.Interfaces.PerfCounterStorage
{
    /// <summary>
    /// Performance Counter Data returned by Storages
    /// Storage returns a collection of such data items for perf. counter track record.
    /// </summary>
    /// <MetaDataID>{3c4e4e9d-e046-405f-a29b-006d362cda5c}</MetaDataID>
    public interface IPerfCounterValue
    {
        DateTime Timestamp { get; }

        float Value { get; }
    }
}
