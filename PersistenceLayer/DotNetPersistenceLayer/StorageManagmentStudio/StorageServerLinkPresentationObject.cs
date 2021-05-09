using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;
using System.ComponentModel;
using OOAdvantech.PersistenceLayer;

namespace StorageManagmentStudio
{
    /// <MetaDataID>{7a2ad925-5320-4c76-a07c-3e3bab6cedcc}</MetaDataID>
    public class StorageServerLinkPresentationObject : PresentationObject<StorageServerLink>
    {
        /// <MetaDataID>{639fad91-8cbe-42cb-b454-647ba1dd5990}</MetaDataID>
        public StorageServerLinkPresentationObject()
            : base(null)
        {
        }
        /// <MetaDataID>{ca1cbeed-648a-48b9-8907-43741a640ab6}</MetaDataID>
        public StorageServerLinkPresentationObject(StorageServerLink connection)
            : base(connection)
        {
            //Enum.GetValues(AbstractionsAndPersistency.OrderState)
        }


        /// <MetaDataID>{5d26a210-393a-41d2-8f78-956adddf8308}</MetaDataID>
        public List<string> StorageServers
        {
            get
            {
                List<string> storageServers=new List<string>();

                OOAdvantech.Remoting.RemotingServices theLocalRemotingServices = OOAdvantech.Remoting.RemotingServices.GetRemotingServices("tcp://localhost:9060") as OOAdvantech.Remoting.RemotingServices;
                OOAdvantech.PersistenceLayer.StorageServerInstanceLocator locator = theLocalRemotingServices.CreateInstance(typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).ToString(), typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).Assembly.FullName) as OOAdvantech.PersistenceLayer.StorageServerInstanceLocator;

                foreach (string instanceName in locator.GetStorageServerInstances().ToList())
                {
                    if (instanceName.ToLower().Trim() == "default")
                        storageServers.Add(System.Environment.MachineName);
                    else
                        storageServers.Add(System.Environment.MachineName+@"\"+instanceName);
                }
                return storageServers;
            }
        }
    }
}
