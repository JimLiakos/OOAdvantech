using System;

namespace Perfon.Interfaces.Common
{
    /// <summary>
    /// User for reporting errors occured inside PerfMonLib to clients
    /// </summary>
    /// <MetaDataID>{54925eb4-69f5-4017-97d1-64219b20be69}</MetaDataID>
    public interface IPerfonErrorEventArgs
    {
        string Message { get; set; }
    }
}
