namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{8AA3F245-A1C9-4668-A9B9-FCE60477EA8F}</MetaDataID>
	/// <summary>The main work of UpdateReferentialIntegrity is to update storage instance for the referential integrity count. 
	/// It is deferent  from the UpdateStorageInstanceCommand because use ShortTimeTransaction of storage instance reference.
	/// Some words for the ShortTimeTransaction. The user can use BeginObjectStateTransition for object state transition in transaction. 
	/// From this time the object is locked for other transactions. The transaction of this type maybe is long time because controlled from the end user. 
	/// The UpdateStorageInstanceCommand use this transaction mechanism. 
	/// But the referential integrity is something related to persistency layer and change at the time of commit of transaction. 
	/// The persistency layer only this time wants lock the object from other transaction to change referential integrity count. 
	/// For this reasons used the ShortTimeTransaction.    
	/// </summary>
	public abstract class UpdateReferentialIntegrity : Command
	{
		/// <MetaDataID>{4C2A750C-DC3C-4FF5-875A-01E76D3C4E6A}</MetaDataID>
		public StorageInstanceRef UpdatedStorageInstanceRef;
		/// <summary>Priority defines the order in which will be executed the command. </summary>
		/// <MetaDataID>{A2E8A717-4198-4353-AC09-1ACA3CC1CB87}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 60;
			}
		}
		/// <MetaDataID>{879BBF14-F021-478E-8881-853871A03B51}</MetaDataID>
		public override void GetSubCommands(int CurrentOrder)
		{
		}
		/// <MetaDataID>{26A54A7D-C2D5-4836-BE25-15DE62B50A09}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "refIntegrity"+UpdatedStorageInstanceRef.MemoryID.ToString();
			}
		}

		/// <MetaDataID>{6C4EABD7-2496-4147-AB59-F38F93DBF492}</MetaDataID>
		public static string GetIdentity(StorageInstanceRef storageInstanceRef)
		{
			return "refIntegrity"+storageInstanceRef.MemoryID.ToString();
		}


	}
}
