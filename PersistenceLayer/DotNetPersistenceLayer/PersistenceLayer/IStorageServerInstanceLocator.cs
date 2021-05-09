namespace OOAdvantech.PersistenceLayer
{
    /// <MetaDataID>{DCF408B2-1C3E-4847-9165-BE250247C566}</MetaDataID>
	public interface IStorageServerInstanceLocator
	{
		/// <MetaDataID>{571A3AAD-212F-44CC-A309-C49BFD545A13}</MetaDataID>
		int GetInstancePort(string InstanceName);
		/// <MetaDataID>{7D1C7E3C-2A03-4F77-99B5-7BFDB9AA48EA}</MetaDataID>
		void AddInstance(int port, string InstanceName);
	}
}
