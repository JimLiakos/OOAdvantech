namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{f695529e-bc61-423c-8acc-386bbfc6c48a}</MetaDataID>
    public interface IErrorLog
    {
        /// <MetaDataID>{927bc830-bcb8-4256-a300-2b34b3924052}</MetaDataID>
        void WriteError(string error);

    }
	/// <MetaDataID>{9D2ED99F-A89E-4000-A6EF-BBB116CD5DBA}</MetaDataID>
	public class SynchronizerSession
	{
        /// <MetaDataID>{93b4707d-1b6f-4ef2-b3d5-fa36ba1cd0fd}</MetaDataID>
        public static IErrorLog ErrorLog;
		/// <MetaDataID>{558E153E-8E8E-402E-8DA2-FC8C5A3F1D88}</MetaDataID>
		private static System.Collections.Generic.Dictionary<MetaObject, MetaObject> SynchronizedMetaObjects;
		/// <MetaDataID>{3722D9BB-115E-4B36-B3A1-1691DDC506DB}</MetaDataID>
		public static void StartSynchronize()
		{
            if (SynchronizedMetaObjects == null)
                SynchronizedMetaObjects = new System.Collections.Generic.Dictionary<MetaObject, MetaObject>();
			SynchronizedMetaObjects.Clear();
		
		}
		/// <MetaDataID>{77A39ED3-E238-4DB8-B751-5C09E2981CDF}</MetaDataID>
		public static void MetaObjectUnderSynchronization(MetaObject theMetaObject)
		{
			if(!SynchronizedMetaObjects.ContainsKey(theMetaObject))
				SynchronizedMetaObjects.Add(theMetaObject,theMetaObject);
		}
		/// <MetaDataID>{EE3A68AB-C700-4FF8-9C73-4978FE6FB667}</MetaDataID>
		public static bool IsSynchronized(MetaDataRepository.MetaObject theMetaObject)
		{
			
			return SynchronizedMetaObjects.ContainsKey(theMetaObject);
		}
		/// <MetaDataID>{4C7A5D7E-F740-47DA-8300-038923D3F930}</MetaDataID>
		public static void StopSynchronize()
		{
			if(SynchronizedMetaObjects!=null)
				SynchronizedMetaObjects.Clear();
		
		}
	}
}
