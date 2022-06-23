using System;

namespace OOAdvantech.Remoting
{
    /// <summary>
    /// </summary>
    /// <MetaDataID>{EEB9EED0-3ED8-4618-BE4C-4F49E7448B08}</MetaDataID>
    internal class Tracker : System.Runtime.Remoting.Services.ITrackingHandler
    {
        /// <MetaDataID>{F10CDEF0-3608-4CA3-8552-B3A1F6F1A511}</MetaDataID>
        public static System.Collections.Generic.Dictionary<string, System.WeakReference> WeakReferenceOnMarshaledObjects = new System.Collections.Generic.Dictionary<string, WeakReference>();
        /// <MetaDataID>{15058CDE-F4EA-4D75-A62B-224BA0184288}</MetaDataID>
        public void MarshaledObject(System.Object obj, System.Runtime.Remoting.ObjRef objRef)
        {
            if (obj is IExtMarshalByRefObject)
            {

                lock (WeakReferenceOnMarshaledObjects)
                {

                        WeakReferenceOnMarshaledObjects[objRef.URI] = new WeakReference(obj);
#if !DeviceDotNet
                        if (!Remoting.RemotingServices.IsOutOfProcess(obj as MarshalByRefObject))
                            RemotingServices.ReorderServerSideObjRefCahnnelData(objRef);
#endif
#if DEBUG
                    if (!(obj is PersistenceLayer.IPersistencyService))
                    {
                        if (objRef.URI != null && PersistenceLayer.ObjectStorage.PersistencyService != null && PersistenceLayer.ObjectStorage.PersistencyService.ClassOfObjectIsPersistent(obj))
                        {
                            int nPos = objRef.URI.IndexOf("#PID#");
                            if (nPos == -1)
                            {
                                System.Diagnostics.Debug.Assert(false, "Persistent object transient uri ");
                            }
                        }
                    }
#endif

                }
            }
        }




        /// <MetaDataID>{2E503454-835E-452A-B95E-4FAD919BFE0D}</MetaDataID>
        public void UnmarshaledObject(System.Object obj, System.Runtime.Remoting.ObjRef or)
        {

            //System.Runtime.Remoting.Services.EnterpriseServicesHelper.SwitchWrappers.CreateConstructionReturnMessage()
            //System.Runtime.Remoting.Proxies.RealProxy oldRrealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(obj);

            //if (obj is IExtMarshalByRefObject)
            //{
            //    System.Runtime.Remoting.ObjRef objRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(obj as MarshalByRefObject);

            //    string channelUri = RemotingServices.GetChannelUri(obj as MarshalByRefObject);

            //    int hh = 0;
            //}

        }
        /// <MetaDataID>{A13B2437-0ACA-4DFF-8755-73B84055E63C}</MetaDataID>
        public void DisconnectedObject(System.Object obj)
        {
            int hh = 0;

        }

    }
}
