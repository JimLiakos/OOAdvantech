namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	/// <MetaDataID>{DD863C1D-2BCB-47DE-B45E-6B61A047F083}</MetaDataID>
    [BackwardCompatibilityID("{DD863C1D-2BCB-47DE-B45E-6B61A047F083}")]
    [AssociationClass(typeof(IProduct), typeof(IPriceList), "ProductPrice")]
	public interface IProductPrice
	{
        /// <MetaDataID>{5c0bd1c8-10a5-4271-a371-0860d150a2dd}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        Quantity Price
        {
            get;
            set;
        }
		/// <MetaDataID>{9BE6F392-F326-41DF-AEDC-4495219ED484}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Name
		{
			get;
			set;
		}
		/// <MetaDataID>{B452FBCF-569C-425E-89FA-43ECCE2DDE45}</MetaDataID>
        [AssociationClassRole(Roles.RoleB), BackwardCompatibilityID("+1")]
		IPriceList PriceList
		{
			get;
			set;
		}
		/// <MetaDataID>{ED774DAC-9FBA-40DC-AEDB-D673AB4728CA}</MetaDataID>
        [AssociationClassRole(Roles.RoleA), BackwardCompatibilityID("+2")]
		IProduct Product
		{
			get;
			set;
		}
	}
}
