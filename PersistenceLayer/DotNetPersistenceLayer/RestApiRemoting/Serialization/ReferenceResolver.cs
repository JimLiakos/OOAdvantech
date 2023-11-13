using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json.Serialization;

namespace OOAdvantech.Remoting.RestApi.Serialization
{

    /// <MetaDataID>{7411cc65-5463-4d51-8890-96ef9dba3ae6}</MetaDataID>
    public class ReferenceResolver : IReferenceResolver
    {

        Dictionary<object, ObjRef> AllowCachingProxyObjectRefs = new Dictionary<object, ObjRef>();

        Dictionary<object, ObjRef> ReferenceOnlyCachingProxyObjectRefs = new Dictionary<object, ObjRef>();

        IReferenceResolver InternalReferenceResolver;
        public ReferenceResolver()
        {
            InternalReferenceResolver = new OOAdvantech.Json.Serialization.DefaultReferenceResolver(); ;
        }
        public void AddReference(object context, string reference, object value)
        {
            InternalReferenceResolver.AddReference(context, reference, value);
        }

        public string GetReference(object context, object value)
        {
            string _ref = InternalReferenceResolver.GetReference(context, value);

            return (int.Parse(_ref) - 1).ToString();
        }

        public bool IsReferenced(object context, object value)
        {
            var isRef = InternalReferenceResolver.IsReferenced(context, value);
            if (isRef)
            {

            }
            return false;


            //return InternalReferenceResolver.IsReferenced(context, value);
        }

        public object ResolveReference(object context, string reference)
        {
            reference = (int.Parse(reference) + 1).ToString();

            return InternalReferenceResolver.ResolveReference(context, reference);
        }

        internal bool IsReferencedTs(object context, object value)
        {
            return InternalReferenceResolver.IsReferenced(context, value);
        }

        internal ObjRef GetPoxyObjRef(object value, bool referenceOnlyCaching)
        {
            if (value is MarshalByRefObject || value is ITransparentProxy)
            {

                ObjRef objRef = null;

                if (!referenceOnlyCaching && AllowCachingProxyObjectRefs.TryGetValue(value, out objRef))
                    return objRef;
                if (referenceOnlyCaching && ReferenceOnlyCachingProxyObjectRefs.TryGetValue(value, out objRef))
                    return objRef;

            }
            return null;


        }

        internal void AssignePoxyObjRef(object value, ObjRef byref)
        {
            if (value is MarshalByRefObject || value is ITransparentProxy)
            {
                if (byref.ReferenceOnlyCaching)
                    ReferenceOnlyCachingProxyObjectRefs[value] = byref;
                else
                    AllowCachingProxyObjectRefs[value] = byref;


            }
        }
    }
}
