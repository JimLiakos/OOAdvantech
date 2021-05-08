using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech
{
    public interface IStorageServerInstanceLocator
    {
        string[] GetStorageServerInstances();
        PersistenceLayer.IPersistencyService GetPersistencyService(string instanceName);
        string GetTCPPort(string instanceName);
        
    }
}
