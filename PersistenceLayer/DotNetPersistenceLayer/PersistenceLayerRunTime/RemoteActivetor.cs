namespace OOAdvantech.PersistenceLayerRunTime
{

	using System; 
	
	public class RemotingServices:Remoting.RemotingServices 
	{
		/// <MetaDataID>{B967C9FD-3296-4617-BE62-A61DAC92DA3B}</MetaDataID>
		public override object CreateInstance(string TypeFullName,params object[] ctorParams)
		{
			if(TypeFullName=="OOAdvantech.PersistenceLayerRunTime.PersistencyContext")
				return PersistenceLayerRunTime.PersistencyContext.CurrentPersistencyContext;
			return base.CreateInstance(TypeFullName,ctorParams);

		}

	}
}
