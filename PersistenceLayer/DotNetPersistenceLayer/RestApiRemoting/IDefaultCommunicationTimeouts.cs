using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{

    /// <summary>
    /// Defines the interface for specifying communication timeouts used by channels,
    /// channel managers such as channel listeners and channel factories, and service
    /// hosts.
    ///  </summary>
    /// <MetaDataID>{db3a2655-c0ea-402b-befe-6b34ed39a8b2}</MetaDataID>
    public interface IDefaultCommunicationTimeouts
    {



        /// <summary>
        ///     Gets the interval of time after which the close method, invoked by a communication
        ///     object, times out.
        /// </summary>
        /// <returns>
        ///     The System.TimeSpan that specifies the interval of time to wait for the close
        ///     method to time out.
        /// </returns>
        /// <MetaDataID>{28652bd5-0df6-4ab2-9c3e-28c42a1378a0}</MetaDataID>
        TimeSpan CloseTimeout { get; }


        /// <summary>
        ///     Gets the interval of time after which the open method, invoked by a communication
        ///     object, times out.
        /// </summary>
        /// <returns>
        ///     The System.TimeSpan that specifies the interval of time to wait for the open
        ///     method to time out.
        /// </returns>
        /// <MetaDataID>{cae4a641-a78a-49f2-8572-ef1970d0adad}</MetaDataID>

        TimeSpan OpenTimeout { get; }
        /// <summary>
        ///     Gets the interval of time after which the receive method, invoked by a communication
        ///     object, times out.
        /// </summary>
        /// <returns>
        ///     The System.TimeSpan that specifies the interval of time to wait for the receive
        ///     method to time out.
        /// </returns>
        /// <MetaDataID>{7c74b1f4-cc83-42b0-95e4-3aa69e2a74a7}</MetaDataID>
        TimeSpan ReceiveTimeout { get; }
        /// <summary>
        ///     Gets the interval of time after which the send method, invoked by a communication
        ///     object, times out.
        /// </summary>
        /// <returns>
        ///     The System.TimeSpan that specifies the interval of time to wait for the send
        ///     method to time out.
        /// </returns>
        /// <MetaDataID>{c293a87f-1a11-4466-946d-13344d8bcb84}</MetaDataID>
        TimeSpan SendTimeout { get; }
    }
}
