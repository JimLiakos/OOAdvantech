using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <summary>
    ///  Contains the binding elements that specify the protocols, transports, and message
    ///  encoders used for communication between clients and services.
    /// </summary>
    /// <MetaDataID>{dc4cd108-0827-4b15-825a-7e5bb0a87266}</MetaDataID>
    public class Binding : IDefaultCommunicationTimeouts
    {

        /// <MetaDataID>{20632625-12fa-49a8-ae15-d354efddfb21}</MetaDataID>
        public static Binding DefaultBinding = new Binding();
        /// <exclude>Excluded</exclude>
        TimeSpan _CloseTimeout = TimeSpan.FromMinutes(1);
        /// <MetaDataID>{dfab9515-d37f-43fc-93a1-35a0a2812a24}</MetaDataID>
        public TimeSpan CloseTimeout
        {
            get
            {
                return _CloseTimeout;
            }
            set
            {
                _CloseTimeout = value;
            }

        }

        /// <exclude>Excluded</exclude>
        TimeSpan _OpenTimeout = TimeSpan.FromMinutes(2);

        /// <MetaDataID>{a67b6cd5-034a-4f03-a59b-0543f26dd168}</MetaDataID>
        public TimeSpan OpenTimeout
        {
            get
            {
                return _OpenTimeout;
            }
            set
            {
                _OpenTimeout = value;
            }
        }

        /// <exclude>Excluded</exclude>
        TimeSpan _RetryOpenTimeout = TimeSpan.FromSeconds(13);

        /// <MetaDataID>{a67b6cd5-034a-4f03-a59b-0543f26dd168}</MetaDataID>
        public TimeSpan RetryOpenTimeout
        {
            get
            {
                return _RetryOpenTimeout;
            }
            set
            {
                _RetryOpenTimeout = value;
            }
        }

        /// <exclude>Excluded</exclude>
        TimeSpan _ReceiveTimeout = TimeSpan.FromMinutes(10);
        /// <MetaDataID>{e4c19d56-33ec-4854-be72-0dec5cfe154f}</MetaDataID>
        public TimeSpan ReceiveTimeout
        {
            get
            {
                return _ReceiveTimeout;
            }
            set
            {
                _ReceiveTimeout = value;
            }
        }

        /// <MetaDataID>{d8c92fbc-0f30-4f2a-98b6-3fef424e2c02}</MetaDataID>
        TimeSpan _SendTimeout = TimeSpan.FromMinutes(2);
        /// <MetaDataID>{a8fe77c6-9a57-4a2e-b045-cab6deccd9fd}</MetaDataID>
        public TimeSpan SendTimeout
        {
            get
            {
                return _SendTimeout;
            }
            set
            {
                _SendTimeout = value;
            }
        }
    }
}