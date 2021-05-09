namespace OOAdvantech.PersistenceLayer
{
	/// <MetaDataID>{A2FFEC89-AFBE-4A52-ADBA-837A95E80727}</MetaDataID>
	/// <summary></summary>
	public abstract class RelResolver
	{
		/// <summary></summary>
		/// <MetaDataID>{80E252E7-E9DD-4169-AA0D-F722001809FF}</MetaDataID>
		/// <param name="theObject"></param>
		public abstract void UnLinkAllObjects(object theObject);
		/// <summary></summary>
		/// <MetaDataID>{11E20F96-C9C7-4D24-8E2C-87CB42C48B48}</MetaDataID>
		public abstract long GetLinkedObjectsCount();
		/// <summary></summary>
		/// <MetaDataID>{11150283-F21E-4D6C-8ACB-99ECE7080F0B}</MetaDataID>
		/// <param name="theObject"></param>
		public abstract void LinkObject(object theObject);
		/// <summary></summary>
		/// <MetaDataID>{7F5DBD10-A5A1-4B5E-99AC-EFEDC641F048}</MetaDataID>
		/// <param name="Criterion"></param>
		public abstract System.Collections.Hashtable GetLinkedObjects(string Criterion);
		/// <summary></summary>
		/// <MetaDataID>{EC1B0000-C1EF-4F4B-820A-2F995F2D7944}</MetaDataID>
		/// <param name="theObject"></param>
		public abstract void UnLinkObject(object theObject);
		
	}
}		
