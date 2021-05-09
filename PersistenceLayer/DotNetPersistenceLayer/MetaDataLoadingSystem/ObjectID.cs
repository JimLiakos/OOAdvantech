using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataLoadingSystem
{

    /// <MetaDataID>{c902ec57-399a-40f0-b9b5-14cf7bf73a44}</MetaDataID>
    public class ObjectID :OOAdvantech.PersistenceLayer.ObjectID
    {
        public static MetaDataRepository.ObjectIdentityType XMLObjectIdentityType = new MetaDataRepository.ObjectIdentityType(new System.Collections.Generic.List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });

        public ObjectID( ulong OID)
            :base(XMLObjectIdentityType,new object[1]{OID},0)
        {
           
        }
        public override string ToString()
        {
            return ObjectIDPartsValues[0].ToString();
        }
        public override string ToString(IFormatProvider provider)
        {
            return ((ulong) ObjectIDPartsValues[0]).ToString(provider);
        }
        
    }
}
