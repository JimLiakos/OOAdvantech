namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	/// <MetaDataID>{5E7D55F1-0CDF-4E0D-A9B7-FAF2F135E160}</MetaDataID>
    [BackwardCompatibilityID("{5E7D55F1-0CDF-4E0D-A9B7-FAF2F135E160}")]
    public interface IStorePlace
	{

        /// <MetaDataID>{859EFA47-F873-4A9A-AD55-850D8349D4DC}</MetaDataID>
        void AddProduct(IProduct product);
        /// <MetaDataID>{8AFB1287-0330-4772-80E7-495CACE0E744}</MetaDataID>
        void RemoveProduct(IProduct product);

        /// <MetaDataID>{88D890C8-C1B4-4152-92DA-E4A4942A1A09}</MetaDataID>
        [Association("ProductsInStore", typeof(IProduct), Roles.RoleB, "{932F5707-C060-4CB9-86C9-D7E9296813BE}")]
        [RoleBMultiplicityRange(0)]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
        OOAdvantech.Collections.Generic.Set<IProduct> StoredProducts
        {
            get;
         
        }
		/// <MetaDataID>{8341A62F-64BF-4C48-A7FA-4C7E8BC15037}</MetaDataID>
		string Name
		{
			get;
			set;
		}
	}
}
