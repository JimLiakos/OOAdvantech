using System;
namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{50BAECB0-E113-4F5C-B2F0-68610E1E67A6}</MetaDataID>
	public abstract class MetaObjectsStack
	{


        /// <MetaDataID>{67ac3296-f4c2-472d-a7df-b3bbf53e32eb}</MetaDataID>
        public bool SuspendSychronizationDelete;

        /// <MetaDataID>{8050ddd9-c56d-4167-942b-99f7c3abcea2}</MetaDataID>
        public abstract OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject);
		/// <MetaDataID>{A3E42D29-4DBA-4216-A530-4889E0514BFE}</MetaDataID>
		public abstract MetaDataRepository.MetaObject FindMetaObjectInPLace(MetaObject OriginMetaObject, MetaDataRepository.MetaObject placeIdentifier);
        /// <MetaDataID>{66d98a4f-9666-440e-a96f-50451e5e5353}</MetaDataID>
		public abstract MetaDataRepository.MetaObject FindMetaObjectInPLace(string MetaObjectID, MetaDataRepository.MetaObject placeIdentifier);
		/// <MetaDataID>{441235D9-CFFA-455D-8B09-27F99DF4F8F7}</MetaDataID>
		public virtual void InitializeMetaObject(MetaObject OriginMetaObject,MetaObject NewMetaObject)
		{
			NewMetaObject.SetIdentity(OriginMetaObject.Identity);
            
		}
		/// <MetaDataID>{5AD33E24-C11C-49B7-A45A-9DDF48D350FC}</MetaDataID>
        //[ThreadStatic]
		protected static MetaObjectsStack _CurrentMetaObjectCreator;
		/// <MetaDataID>{F2D176CA-823B-4124-835A-4D51D27DC007}</MetaDataID>
		public static MetaObjectsStack CurrentMetaObjectCreator
		{
			get
			{
				return _CurrentMetaObjectCreator;
			}
			set
			{
				
				_CurrentMetaObjectCreator=value;
			}
		}
        /// <MetaDataID>{51a70cc1-53d7-4803-8473-6bf5ce3350a2}</MetaDataID>
        public virtual ContainedItemsSynchronizer BuildItemsSychronizer(System.Collections.IList theSource, System.Collections.IList theUpdated, MetaDataRepository.MetaObject placeIdentifier)
        {
            return new ContainedItemsSynchronizer(theSource, theUpdated, placeIdentifier);
        }
		/// <MetaDataID>{E90490D1-BDC5-440D-9DF0-EC7642ED67D3}</MetaDataID>
        /// <summary>
        /// This operation creates a corresponding metadata object in objects context which host the metadata model. 
        /// For instance the objects context can be rdbms mapping context.
        /// Usually used from metadata model synchronization process.
        /// Sometimes the target metadata model objects context doesn’t support some metadata objects, 
        /// like the operation in rdbms metadata model objects context. 
        /// In this case operation returns null.     
        /// </summary>
        /// <param name="originMetaObject">
        /// This parameter defines the original metadata object
        /// </param>
        /// <param name="placeIdentifier">
        /// This parameter defines an object which live in target objects contexts.
        /// this object used from the system to retrieve the target objects context
        /// </param>
		public abstract MetaObject CreateMetaObjectInPlace(MetaObject originMetaObject, MetaDataRepository.MetaObject placeIdentifier);
	
	}
}
