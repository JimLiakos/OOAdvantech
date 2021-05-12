namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	/// <MetaDataID>{078B342A-2AC0-49AB-A929-1948C2267392}</MetaDataID>
	[BackwardCompatibilityID("{078B342A-2AC0-49AB-A929-1948C2267392}")]
	public interface IPriceList
	{
	

		/// <MetaDataID>{A8A6B152-57E5-434D-B390-29C2CAF0B77B}</MetaDataID>
		string Name
		{
			get;
			set;
		}
		/// <MetaDataID>{8BC2369C-3E87-4079-905C-3528CFC06694}</MetaDataID>
        [Association("ProductPrice", typeof(IProduct), Roles.RoleA, "{82EF20D6-8AF9-494E-B661-0384D66A7F27}")]
		[RoleAMultiplicityRange(0)]
        [AssociationClass(typeof(IProductPrice))]
        
		[BackwardCompatibilityID("+1")]
		OOAdvantech.Collections.Generic.Set<IProductPrice> Products
		{
			get;
		}
		/// <MetaDataID>{BF8EB886-B245-4E21-B156-9668AAC691A3}</MetaDataID>
		void AddProduct(IProductPrice productPrice);
	}
}
