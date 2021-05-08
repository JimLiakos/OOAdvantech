using System;

namespace OOAdvantech.MetaDataRepository
{
    /// <summary></summary>
    /// <MetaDataID>{51c3e712-3f5e-4475-86b6-e5d620774b70}</MetaDataID>
	public class AssociationEndBehavior : System.Attribute
	{
        /// <MetaDataID>{3878ab50-5f09-415f-a117-bac8bd10e578}</MetaDataID>
		public PersistencyFlag PersistencyFlag=PersistencyFlag.LazyFetching;
        /// <MetaDataID>{795359d7-ed47-4227-96ff-6ab95f2414d6}</MetaDataID>
		public AssociationEndBehavior(PersistencyFlag mPersistencyFlag)
		{
			PersistencyFlag=mPersistencyFlag;
		}
	}
}
