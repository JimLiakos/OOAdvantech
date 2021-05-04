using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ModulePublisher
{
    /// <MetaDataID>{309a04ff-c732-4d09-816b-871454be2716}</MetaDataID>
    [ServiceContract] 
    public interface IModulePublisher
    {
        [OperationContract] 
        int ExecudeModulePublishCommand(string fileName, string arguments);
    } 
}

