namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	/// <MetaDataID>{69971759-C0EA-4499-B3F0-0597B0F5DC97}</MetaDataID>
	[BackwardCompatibilityID("{69971759-C0EA-4499-B3F0-0597B0F5DC97}")]
	[Persistent()]
	public class SubPriceList : PriceList
	{

		/// <MetaDataID>{E447EC02-7D5E-4337-83DE-FB7B4E3E7FB2}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[PersistentMember()]
		public long ID=0;
		/// <MetaDataID>{708B3968-E00E-43CA-912A-EF24D6789214}</MetaDataID>
		protected SubPriceList()
		{
		}
		/// <MetaDataID>{A62208FC-2605-4DD1-AA2C-F23C14687528}</MetaDataID>
		public SubPriceList(string name):base( name)
		{

		}
	}
    /// <MetaDataID>{61bdd512-569a-411d-bd79-d789924084bb}</MetaDataID>
    public class SubPriceListB : SubPriceList
    {
        /// <MetaDataID>{2408ad04-05b6-42ac-862a-3dd552100044}</MetaDataID>
        public int trtrr; 
    }
}
