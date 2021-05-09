namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{DFDFB0C0-EE21-4ECF-B916-2356E87AF73D}</MetaDataID>
	/// <summary>TransactionCommand is a command that change the state of the system in the lifetime of transaction. TransactionCommand maybe is self-existent command or a compound command. TransactionCommand has priority in execution time. </summary>
	public abstract class Command
	{
		/// <MetaDataID>{1C6DE445-EC82-41C2-8988-79B73205B5DB}</MetaDataID>
		public ITransactionContext OwnerTransactiont;
		/// <summary>Return the auto-produced commands from the main command. Some times must be produced new commands as result of initial command. For instance when delete a link between tow object and relation characterized as cascade delete. </summary>
		/// <MetaDataID>{E8DC61C9-6F81-4B81-86FB-A1ABD9AED57B}</MetaDataID>
		public abstract void GetSubCommands(int currentExecutionOrder);
		/// <MetaDataID>{31F7A23F-CC74-46B7-90C8-C1668BA48A22}</MetaDataID>
		/// <summary>Defines the order in which will be executed the command. 
        /// For instance the new storage instance command must be executed first of all. 
        /// some commands
        ///10 NewStorageInstanceCommand
        ///10 OutStorageUnlinkAllObjectCmd
        ///20 SplitClassActiveStorageCell
        ///20 UpdateStorageSchema
        ///30 InterSorageLinkObjectsCommand
        ///30 LinkObjectsCommand
        ///40 UpdateGlobalObjectCollectionIDs
        ///40 UnlinkAllObjectOfStorageCellLinkCmd
        ///40 InterSorageUnLinkObjectsCommand
        ///40 UnLinkObjectsCommand
        ///40 OnMemoryUnlinkAllObjectCommand
        ///60 DeleteStorageInstanceCommand
        ///50 UpdateStorageInstanceCommand
        ///50 UpdateReferentialIntegrity
        /// </summary>
		public abstract int ExecutionOrder
		{


			get;
		}
       
		/// <summary>With this method execute the command. </summary>
		/// <MetaDataID>{68C8CCB1-D5E9-4E50-8078-8E6F2C80221B}</MetaDataID>
		public abstract void Execute();

		/// <MetaDataID>{2CFF4089-1CF9-43EC-8236-E2BF593CD6EA}</MetaDataID>
		public abstract string Identity
		{
			get;
		}
	}
}
