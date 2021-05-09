using System;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{ba67f8b9-e2d4-4f1b-adc6-e6616329d3bf}</MetaDataID>
    public class ClientSessionPart
    {

       override 
        public static System.Collections.Generic.Dictionary<string, ClientSessionPart> Sessions = new System.Collections.Generic.Dictionary<string, ClientSessionPart>();
        private string channelUri;

         ClientSessionPart(string channelUri)
        {
            this.channelUri = channelUri;
            ClientSessionPartID = Guid.NewGuid().ToString();
        }

        public string ClientSessionPartID { get; private set; }

        internal static ClientSessionPart GetCommunicationSession(string channelUri)
        {

            ClientSessionPart clientSessionPart = null;

            if (!Sessions.TryGetValue(channelUri, out clientSessionPart))
            {

                clientSessionPart = new ClientSessionPart(channelUri);
                Sessions[channelUri] = clientSessionPart;
            }
            return clientSessionPart;
        }

        internal object Unmarshal(ReturnMessage returnMessage)
        {

            Proxy proxy = Proxy.GetProxy(returnMessage.ChannelUri, returnMessage.ReturnObjectUri, returnMessage.ReturnType, returnMessage.ReturnTypeMetaData);

            return null;
        }
    }
}