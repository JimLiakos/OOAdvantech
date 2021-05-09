using System;

namespace OOAdvantech.PersistenceLayer
{
    /// <summary></summary>
    /// <MetaDataID>{22774ed5-7cb0-4ad1-8ac8-83fbcf23645f}</MetaDataID>
	public abstract class ObjectCollectionFactory
	{
        /// <MetaDataID>{2be43b1e-7b9e-4c52-affc-14ddb2fcac94}</MetaDataID>
		static internal protected ObjectCollectionFactory MonoStateObjectCollectionFactory;
        /// <MetaDataID>{71726d51-da7a-4a82-a64f-078395f72b1b}</MetaDataID>
		abstract internal protected ObjectCollection CreateOnMemoryCollection();

        /// <MetaDataID>{45486d9c-a903-4e40-bd87-f35349592a27}</MetaDataID>
        abstract internal protected ObjectCollection CreateOnMemoryCollection(System.Collections.ICollection collection);


        abstract internal protected ObjectCollection CreateMultilingualOnMemoryCollection();
        abstract internal protected ObjectCollection CreateMultilingualOnMemoryCollection(System.Collections.ICollection collection);

    }
}
