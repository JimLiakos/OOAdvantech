namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	/// <MetaDataID>{FD97F970-C6B7-45AB-8F23-CE33CD7DE51F}</MetaDataID>
	[BackwardCompatibilityID("{FD97F970-C6B7-45AB-8F23-CE33CD7DE51F}")]
	[Persistent()]
	public class MaterialProduct : Product
	{
		/// <MetaDataID>{67E50FE4-338B-442A-B3B2-07A11DF23925}</MetaDataID>
		 protected MaterialProduct()
		{
		}
		/// <MetaDataID>{008E5DC8-2409-4008-8C6D-CC2F8F277015}</MetaDataID>
		public MaterialProduct(string name):base( name)
		{
            

		}



	}
}
