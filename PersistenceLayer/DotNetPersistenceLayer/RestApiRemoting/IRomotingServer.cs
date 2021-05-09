using System;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{997b2dba-9473-427d-b70e-66f1ac7bc7e8}</MetaDataID>
    [MetaDataRepository.HttpVisible]
#if DeviceDotNet
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
#endif
    public interface IRemotingServer
    {
        /// <MetaDataID>{93f500f7-fb07-4ff1-b2fa-561615d469c5}</MetaDataID>
        object CreateInstance(string TypeFullName, string assemblyData, Type[] paramsTypes, params object[] ctorParams);

        // object CreateInstance(string TypeFullName, string assemblyData );

        ///// <MetaDataID>{f584b86e-8ed9-4335-bc56-8f750cdc6308}</MetaDataID>
        //IServerSessionPart GetServerSession(Guid processIdentity);

        MarshalByRefObject RefreshCacheData(MarshalByRefObject obj);

        MarshalByRefObject GetPersistentObject(string persistentUri);
    }
}