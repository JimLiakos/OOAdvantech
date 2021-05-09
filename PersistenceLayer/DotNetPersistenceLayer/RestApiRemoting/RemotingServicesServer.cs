using OOAdvantech.MetaDataRepository;
using System;
using System.IO;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{0f1dba1c-e863-4105-9132-58cf894835ee}</MetaDataID>

    public class RemotingServicesServer : MonoStateClass, IRemotingServer
    {

        public RemotingServicesServer()
        {

        }

        public MarshalByRefObject RefreshCacheData(MarshalByRefObject obj)
        {
            return obj;
        }
        public MarshalByRefObject GetPersistentObject(string persistentUri)
        {
           var  _object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(persistentUri);
            return _object as MarshalByRefObject;
        }
        /// <MetaDataID>{36fcbfd5-d7f8-4c53-854c-3e3e29e016c1}</MetaDataID>
        public object CreateInstance(string TypeFullName, string assemblyData)
        {

            throw new NotImplementedException();
        }

        /// <MetaDataID>{4feebecf-d4c3-4bba-8c6e-47b464a92b57}</MetaDataID>
        public object CreateInstance(string TypeFullName, string assemblyData, Type[] paramsTypes, params object[] ctorParams)
        {
            bool isHttpCall = false;
            Type type = null;
            string typeUri = TypeFullName;

#if DeviceDotNet
            string[] typeUriParts = typeUri.Split('/');
#else
            string[] typeUriParts = typeUri.Split(Path.AltDirectorySeparatorChar);
#endif
            //string[] typeUriParts = typeUri.Split(Path.AltDirectorySeparatorChar);
            if (typeUriParts.Length > 1)
            {
                type = Type.GetType(typeUriParts[1] + "," + typeUriParts[0]);
                if(type==null)
                    type = Type.GetType(typeUriParts[1]);
                if (type == null)
                    Serialization.SerializationBinder.NamesTypesDictionary.TryGetValue(typeUriParts[1], out type);
                isHttpCall = true;
            }
            else
                type = ModulePublisher.ClassRepository.GetType(TypeFullName, assemblyData);


            if (ctorParams != null && ctorParams.Length > 0)
            {

                object NewInstance = AccessorBuilder.CreateInstance(type, paramsTypes, ctorParams);
                if (NewInstance != null && (NewInstance as MarshalByRefObject) == null)
                    throw new Exception("The " + TypeFullName + " isn't type of System.MarshalByRefObject");
                return NewInstance;
            }
            else
            {
                
                if (Classifier.GetClassifier(type).IsA(Classifier.GetClassifier(typeof(MonoStateClass))))
                {
                    MonoStateClass instance = MonoStateClass.GetInstance(type,true);
                    return instance;
                }
                object NewInstance = AccessorBuilder.CreateInstance(type);
                if (NewInstance != null && (NewInstance as MarshalByRefObject) == null)
                    throw new Exception("The " + TypeFullName + " isn't type of System.MarshalByRefObject");
                return NewInstance;

            }

        }

        /// <MetaDataID>{484036a1-cb42-4824-848c-b6722a8aa005}</MetaDataID>
        static public System.Collections.Generic.Dictionary<System.Guid, System.WeakReference> Sessions = new System.Collections.Generic.Dictionary<Guid, WeakReference>();
        /// <MetaDataID>{941102cb-0067-45a6-a00e-166497edf371}</MetaDataID>
        static OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

//        /// <MetaDataID>{b5c2fa5c-9440-4730-b760-6880ecd7746e}</MetaDataID>
//        public IServerSessionPart GetServerSession(Guid clientProcessIdentity)
//        {

//            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
//            try
//            {
//                ServerSessionPart serverSessionPart = null;
//                if (Sessions.ContainsKey(clientProcessIdentity))
//                {
//                    serverSessionPart = Sessions[clientProcessIdentity].Target as ServerSessionPart;
//                    serverSessionPart = Sessions[clientProcessIdentity].Target as ServerSessionPart;
//                    if (serverSessionPart == null)
//                    {
//                        serverSessionPart = new ServerSessionPart(clientProcessIdentity);
//                        Sessions[clientProcessIdentity] = new WeakReference(serverSessionPart);
//                    }
//                }
//                else
//                {
//                    serverSessionPart = new ServerSessionPart(clientProcessIdentity);
//                    Sessions[clientProcessIdentity] = new WeakReference(serverSessionPart);
//                }

//                System.Runtime.Remoting.RemotingServices.Marshal(serverSessionPart);

//#if !DeviceDotNet
//                System.Runtime.Remoting.Lifetime.ILease lease = GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
//                if (lease != null)
//                    lease.Renew(System.TimeSpan.FromMinutes(0.5));
//#endif

//                return serverSessionPart;
//            }
//            finally
//            {
//                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
//            }


//        }
    }
}