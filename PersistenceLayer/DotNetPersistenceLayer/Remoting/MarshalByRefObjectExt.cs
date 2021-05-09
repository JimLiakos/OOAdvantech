using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Remoting
{
    
    public class MarshalByRefObjectExt:MarshalByRefObject,IExtMarshalByRefObject
    {

        MemberValue<bool> _IsPersistent;
        internal bool IsPersistent
        {
            get
            {
                if (_IsPersistent.UnInitialized)
                {
                    if (GetType().GetCustomAttributes(typeof(MetaDataRepository.Persistent), true).Length > 0)
                        _IsPersistent.Value = true;
                    else
                        _IsPersistent.Value = false;

                }
                return _IsPersistent.Value;
            }
        }
        //bool InCreateObjRef = false;
        //public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        //{
        //    if (InCreateObjRef)
        //        return base.CreateObjRef(requestedType);
        //    try
        //    {
        //        InCreateObjRef = true;
        //        if (IsPersistent)
        //        {
        //            try
        //            {
        //                string persistentObjectUri = OOAdvantech.Remoting.RemotingServices.PersistentObjectLifeTimeController.GetPersistentObjectUri(this);
        //                if (!string.IsNullOrEmpty(persistentObjectUri))
        //                {
        //                    System.Runtime.Remoting.ObjRef objRef = base.CreateObjRef(requestedType);
        //                    objRef.URI += "#POID#(" + persistentObjectUri + ")";
        //                    return objRef;
        //                }
        //            }
        //            catch (System.Exception error)
        //            {

        //            }
        //            int tt = 0;
        //        }
        //        else
        //        {
        //            if (GetType().GetCustomAttributes(typeof(MonoStateClassAttribute), true).Length > 0)
        //            {
        //                string persistentObjectUri = "#MonoStateClass#" + RemotingServices.MonoStateClassChannelUri;
        //                if (!string.IsNullOrEmpty(persistentObjectUri))
        //                {
        //                    //System.Runtime.Remoting.ObjRef objRef = base.CreateObjRef(requestedType);
        //                    //objRef.URI += persistentObjectUri;

        //                    System.Runtime.Remoting.ObjRef objRef = System.Runtime.Remoting.RemotingServices.Marshal(this, persistentObjectUri);
        //                    return objRef;
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception error)
        //    {
        //        InCreateObjRef = false;

        //    }

        //    return base.CreateObjRef(requestedType);
        //}
        
    }
}
