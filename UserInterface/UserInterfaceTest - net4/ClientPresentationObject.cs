using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterfaceTest
{
    /// <MetaDataID>{f1b9b68b-ddfe-4d8e-b880-a5228eb80a66}</MetaDataID>
    public class ClientPresentationObject:OOAdvantech.UserInterface.Runtime.PresentationObject<AbstractionsAndPersistency.IClient>
    {
        public ClientPresentationObject(AbstractionsAndPersistency.IClient client):base(client)
        {
            
        }
        public string Name
        {
            get
            {
                return "Text_" + RealObject.Name;
            }
        }

    }
}
