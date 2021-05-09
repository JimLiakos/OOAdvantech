using OOAdvantech.Remoting;

namespace OOAdvantech.PersistenceLayerRunTime
{
	/// <MetaDataID>{E790C3AB-BEE1-47C0-A5E3-4C7B01D61BC6}</MetaDataID>
	public class StorageServerInstanceLocator: MarshalByRefObject, Remoting.IExtMarshalByRefObject,PersistenceLayer.IStorageServerInstanceLocator
	{
        /// <MetaDataID>{0FB359C4-A5F4-4C85-AF18-BE658A79C1C9}</MetaDataID>
        static private Collections.Generic.Dictionary<string, int> InstancesPort = new Collections.Generic.Dictionary<string, int>();
		
		/// <MetaDataID>{1616F3EF-9122-411F-8238-1075657B8794}</MetaDataID>
		public int GetInstancePort(string InstanceName)
		{
			if(InstancesPort.ContainsKey(InstanceName.Trim().ToLower()))
				return InstancesPort[InstanceName.Trim().ToLower()];
			else
				return 0;
		}
		/// <MetaDataID>{A1F4BCCF-6100-489D-A5E1-8C1446D49B91}</MetaDataID>
		public void AddInstance(int port, string InstanceName)
		{
			InstancesPort[InstanceName.Trim().ToLower()]=port;
		}
	}
}
