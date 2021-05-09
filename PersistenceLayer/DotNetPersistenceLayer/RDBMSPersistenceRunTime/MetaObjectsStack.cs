using OOAdvantech.RDBMSDataObjects;
namespace OOAdvantech.RDBMSPersistenceRunTime
{
	/// <MetaDataID>{CEAD9ACE-7259-4D43-B5E0-B496062495CB}</MetaDataID>
	public class MetaObjectsStack : MetaDataRepository.MetaObjectsStack
	{
        /// <MetaDataID>{9ddd3dc2-c5e5-4130-b4d9-d1ae2d57962f}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            return metaObject.Identity;
        }
		/// <MetaDataID>{E8EC2C19-BF4D-43AC-9F5B-EC244E4E1C8D}</MetaDataID>
		public DataBase theSynchronizedDataBase=null;
		/// <MetaDataID>{C67A45D3-C767-4FCD-B21D-0F0524E546B6}</MetaDataID>
		public override MetaDataRepository.MetaObject FindMetaObjectInPLace(MetaDataRepository.MetaObject OriginMetaObject, MetaDataRepository.MetaObject placeIdentifier)
		{
			if(OriginMetaObject is RDBMSMetaDataRepository.Table&&theSynchronizedDataBase!=null)
				return theSynchronizedDataBase.GetTable(OriginMetaObject.Name);
			return null;
		}
		/// <MetaDataID>{F38E83E5-0A0A-46D2-A62F-B933211ED507}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(string MetaObjectID, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
		{
			return null;
		}

		/// <MetaDataID>{1505260A-478A-4655-9631-26AAA59DDB31}</MetaDataID>
		public override MetaDataRepository.MetaObject CreateMetaObjectInPlace(MetaDataRepository.MetaObject OriginMetaObject, MetaDataRepository.MetaObject placeIdentifier)
		{
			MetaDataRepository.MetaObject NewMetaObject=null;
			if(OriginMetaObject is RDBMSMetaDataRepository.Column)
				NewMetaObject=new Column();

			if(OriginMetaObject is RDBMSMetaDataRepository.Key)
				NewMetaObject=new Key();

			InitializeMetaObject(OriginMetaObject,NewMetaObject);
			return NewMetaObject;
		}
	}
}
