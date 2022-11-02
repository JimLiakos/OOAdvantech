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
        private string channelUri;
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

        

        public TCPChannel(string channelUri)
        {
            this.channelUri = channelUri;
            TcpIMessageDispatcher = GetWcfMessageDispatcherProxy(channelUri + "WCFMessageDispatcher");
        }

        public Task<ResponseData> AsyncProcessRequest(RequestData requestData)
        {
            return Task<ResponseData>.Factory.StartNew(() =>
            {
                return TcpIMessageDispatcher.MessageDispatch(requestData);
            });
        }

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
        private static IMessageDispatcher GetWcfMessageDispatcherProxy(string serviceUrl)
        {
            System.ServiceModel.NetTcpBinding binding = new System.ServiceModel.NetTcpBinding(System.ServiceModel.SecurityMode.None);
            System.ServiceModel.EndpointAddress endpointAddress
                = new System.ServiceModel.EndpointAddress(serviceUrl);

            return new System.ServiceModel.ChannelFactory<IMessageDispatcher>
                (binding, endpointAddress).CreateChannel();
        }

        public void Close()
        {

        }

        public void DropPhysicalConnection()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
