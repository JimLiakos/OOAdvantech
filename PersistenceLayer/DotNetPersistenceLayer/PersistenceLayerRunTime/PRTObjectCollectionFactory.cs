using System;
using System.Collections;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.PersistenceLayerRunTime
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{6AC7D87E-6B9D-4D47-B7DB-6EE1CA132770}</MetaDataID>
	public class ObjectCollectionFactory:PersistenceLayer.ObjectCollectionFactory
	{
		
		/// <MetaDataID>{D632273F-99E3-4211-80BC-D72107A48D44}</MetaDataID>
		public ObjectCollectionFactory()
		{  
			if(MonoStateObjectCollectionFactory==null)
				MonoStateObjectCollectionFactory=this;
		} 
		/// <MetaDataID>{BA364D26-263F-4434-99B4-5506F88943EB}</MetaDataID>
		internal protected override OOAdvantech.PersistenceLayer.ObjectCollection CreateOnMemoryCollection()
		{
			return new OnMemoryObjectCollection();
		}
        /// <MetaDataID>{0ea67645-b302-4824-85b5-1d969b407024}</MetaDataID>
        protected internal override OOAdvantech.PersistenceLayer.ObjectCollection CreateOnMemoryCollection(System.Collections.ICollection collection)
        {
            return new OnMemoryObjectCollection(collection);
        }

        protected internal override ObjectCollection CreateMultilingualOnMemoryCollection()
        {
            return new OnMemoryObjectCollection(true);
        }

        protected internal override ObjectCollection CreateMultilingualOnMemoryCollection(ICollection collection)
        {
            return new OnMemoryObjectCollection(collection, true);
        }
    }
}
