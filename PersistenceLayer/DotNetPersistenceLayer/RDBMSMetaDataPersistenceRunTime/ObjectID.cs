using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{

    /// <MetaDataID>{d8d19722-5d49-4e8a-b75e-3b1204bf1253}</MetaDataID>
    public class ObjectID :OOAdvantech.PersistenceLayer.ObjectID
    {
        public static MetaDataRepository.ObjectIdentityType ObjectIdentityType = new MetaDataRepository.ObjectIdentityType(new System.Collections.Generic.List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(int)) });

        public ObjectID( int OID)
            :base(ObjectIdentityType,new object[1]{OID},0)
        {
           
        }
        public override string ToString()
        {
            return ObjectIDPartsValues[0].ToString();
        }
    }
}
