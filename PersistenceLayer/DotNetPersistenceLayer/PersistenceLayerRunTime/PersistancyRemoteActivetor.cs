namespace OOAdvantech.PersistenceLayerRunTime
{

	using System; 
	
	/// <MetaDataID>{B95CE235-EC09-46C6-9568-B715C93E61D5}</MetaDataID>
	public class RemotingServices:Remoting.RemotingServices 
	{
		/// <MetaDataID>{B967C9FD-3296-4617-BE62-A61DAC92DA3B}</MetaDataID>
		public override object CreateInstance(string TypeFullName,params object[] ctorParams)
		{
			//if(TypeFullName== typeof(PersistencyContext).FullName)
			//	return PersistenceLayerRunTime.PersistencyContext.CurrentPersistencyContext;

			object NewInstance= base.CreateInstance(TypeFullName,ctorParams);
			return NewInstance;

		}

	}
}
