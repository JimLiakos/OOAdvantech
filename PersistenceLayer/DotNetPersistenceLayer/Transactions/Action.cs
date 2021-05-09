namespace OOAdvantech.Transactions
{
	/// <MetaDataID>{70552102-4EF8-441F-9FEA-D1442AB46CCD}</MetaDataID>
	public abstract class Action
	{
		/// <MetaDataID>{D466800F-D2A9-45D0-BB7C-9B29FA2D0114}</MetaDataID>
		public Action(object actOnObject)
		{
			ActOnObject=actOnObject;
		}
		/// <MetaDataID>{757D7964-F524-42C5-B4E4-C28FE1602E30}</MetaDataID>
		public readonly object ActOnObject;
		/// <MetaDataID>{E12A31DE-A566-4674-A3B6-C53C630D5F27}</MetaDataID>
		public abstract void Do();
		/// <MetaDataID>{BA6FD15D-AD77-4368-921F-2C74E0832F42}</MetaDataID>
		public abstract void Undo();
	}
}
