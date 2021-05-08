namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{4D84C202-001E-4D21-A83D-CF594F6A0786}</MetaDataID>
	public class MetaObjectCollectionA : MetaDataRepository.MetaObjectCollectionA
	{
		/// <MetaDataID>{C12BC8EE-3A65-4382-B6E2-91835D6AD1C8}</MetaDataID>
		public override void AddCollection(MetaDataRepository.MetaObjectCollectionA  theObjectCollection)
		{
			if(theObjectCollection!=null&&theObjectCollection.Count>0)
				base.AddCollection(theObjectCollection);


			
		
		}
		/// <MetaDataID>{702E7224-41CD-4DD9-A607-63FF1084844E}</MetaDataID>
		public override void Add(MetaDataRepository.MetaObject theObject)
		{
			base.Add(theObject);
		}
	}
}
