using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{cf9e24fb-7e97-45c5-892f-24ecf5c821e4}</MetaDataID>
    public class TCPChannel : IChannel
    {
        /// <MetaDataID>{55fc9448-1d45-4a84-a226-13c39f47edd7}</MetaDataID>
        private string channelUri;
        /// <MetaDataID>{cb15ecbf-3c4f-492f-a822-b998b6baac76}</MetaDataID>
        IMessageDispatcher TcpIMessageDispatcher;

        public IEndPoint EndPoint
        {
            get
            {
                return null;
            }
            set
            {
            }
        }



        /// <MetaDataID>{c0a8b906-a076-4483-b717-dbc16fa12ae9}</MetaDataID>
        public TCPChannel(string channelUri)
        {
            this.channelUri = channelUri;
            TcpIMessageDispatcher = GetWcfMessageDispatcherProxy(channelUri + "WCFMessageDispatcher");
        }

        /// <MetaDataID>{2063ad0d-0df4-427f-a82c-b5fbd7ed9a06}</MetaDataID>
        public Task<ResponseData> AsyncProcessRequest(RequestData requestData)
        {
            return Task<ResponseData>.Factory.StartNew(() =>
            {
                return TcpIMessageDispatcher.MessageDispatch(requestData);
            });
        }

        /// <MetaDataID>{6e1569a1-2a38-49a9-a96d-335a759a9b5a}</MetaDataID>
        public ResponseData ProcessRequest(RequestData requestData)
        {
            try
            {
                return TcpIMessageDispatcher.MessageDispatch(requestData);
            }
            catch (System.ServiceModel.CommunicationException communicationError)
            {
                try
                {
                    // Try to reconnect
                    TcpIMessageDispatcher = GetWcfMessageDispatcherProxy(channelUri + "WCFMessageDispatcher");
                    return TcpIMessageDispatcher.MessageDispatch(requestData);
                }
                catch (Exception innerError)
                {
                    throw;
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

#if !DeviceDotNet
        /// <MetaDataID>{822a05b7-1afd-4c80-bcc2-888ef657ab33}</MetaDataID>
        private static IMessageDispatcher GetWcfMessageDispatcherProxy(string serviceUrl)
        {
            System.ServiceModel.NetTcpBinding binding = new System.ServiceModel.NetTcpBinding(System.ServiceModel.SecurityMode.None);
            System.ServiceModel.EndpointAddress endpointAddress
                = new System.ServiceModel.EndpointAddress(serviceUrl);

            return new System.ServiceModel.ChannelFactory<IMessageDispatcher>
                (binding, endpointAddress).CreateChannel();
        }

        /// <MetaDataID>{b3938701-200f-41b7-9cf9-f2650eae7b81}</MetaDataID>
        public void Close()
        {

        }

        /// <MetaDataID>{6e6b333f-b817-40a4-a81f-8ad7764eeb8a}</MetaDataID>
        public void PhysicalConnectionDropped()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
