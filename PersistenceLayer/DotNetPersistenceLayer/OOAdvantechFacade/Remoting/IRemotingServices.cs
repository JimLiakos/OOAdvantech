using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Remoting
{
    public interface IRemotingServices
    {
        public IExtMarshalByRefObject ReconnectWithObject(ExtObjectUri extObjectUri);
        public void RenewObjects(ref System.Collections.ArrayList MarshaledByRefObjects);
        public object CreateInstance(string TypeFullName, string assemblyData, Type[] paramsTypes, params object[] ctorParams);
        public object CreateInstance(string TypeFullName, string assemblyData);
        public ServerSessionPart GetServerSession(Guid clientProcessIdentity)
    }
}
