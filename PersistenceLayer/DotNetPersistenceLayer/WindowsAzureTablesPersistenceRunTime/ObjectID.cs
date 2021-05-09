using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;



namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{c7527658-16ef-4443-b843-41c0a938b7b0}</MetaDataID>
    public class ObjectID : OOAdvantech.PersistenceLayer.ObjectID
    {

        /// <MetaDataID>{b5f0107e-5b07-479d-b675-040429e894ca}</MetaDataID>
        public static MetaDataRepository.ObjectIdentityType DefultObjectIdentityType = new MetaDataRepository.ObjectIdentityType(new System.Collections.Generic.List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(Guid)) });

        /// <MetaDataID>{84b15932-74a4-47c1-b58f-effa0bb7624b}</MetaDataID>
        public ObjectID(Guid OID)
                    : base(DefultObjectIdentityType, new object[1] { OID }, 0)
        {
             
        }

        public ObjectID(ObjectIdentityType objectIdentityType, object[] objectIDPartsValues, int ObjCellIDValue) : base(objectIdentityType, objectIDPartsValues, ObjCellIDValue)
        {
        }

        /// <MetaDataID>{bb176732-8b4f-4803-9b8e-592efe17dd13}</MetaDataID>
        public override string ToString()
        {
            return ObjectIDPartsValues[0].ToString();
        }
    }
}
