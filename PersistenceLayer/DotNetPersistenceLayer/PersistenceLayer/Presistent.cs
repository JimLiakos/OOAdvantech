using System;
namespace OOAdvantech.PersistenceLayer
{
	public enum AssotiationEnd
	{
		RoleA,
		RoleB
	};
	
	public enum PersistencyFlag
	{
		/// <summary>
		///The Persistenta fields or linked objects are loaded,
		///at the time where the object goes from the passive 
		///to operative mode.
		/// </summary>
		OnConstruction=1,
		/// <summary>
		///The persistent fields or linked objects aren’t loaded, at the time where the 
		///object goes from the passive to operative mode, but at the time there are needed.
		/// </summary>
		LazyFetching=2,
		/// <summary>
		///System can’t delete permanently an object of destination Class 
		///if it has link with the object of root class. Root class is the class 
		///that contain the field and destination class is the class (type) of field.
		/// </summary>
		ReferentialIntegrity=4,
		/// <summary>
		///If we decide to delete a link with the object of the field, 
		///system will try to delete the object of the field.
		/// </summary>
		CascadeDelete=8
	};
	public enum Multiplicity {NoLimit=-1};
		

	public enum PersistencyType
	{
		NormaClass =0,
		historyClass =1
	}

	/// <metadataid>{225BFEAA-DB7A-4E32-899E-557A37B7A5E8}</metadataid>
	[AttributeUsage(AttributeTargets.All)]
	public class Persistenta:Transactions.TransactionalAttribute
	{ 
		/// <MetaDataID>{58AF5292-B56A-4EF2-93C0-E8E16BA45B11}</MetaDataID>
		public PersistencyType PersistencyType;
		/// <MetaDataID>{2983E6AE-2DC6-4EAB-8531-D061FFF26187}</MetaDataID>
		public int NumberOfObject;
		/// <MetaDataID>{2E6D0739-E608-48FC-BC72-CBA0D1599367}</MetaDataID>
		public Persistenta()
		{
		}

		/// <MetaDataID>{7FD79568-63F2-4F87-988E-F553559367BA}</MetaDataID>
		public string ExtMetaData=null;
		/// <MetaDataID>{E49548D8-3BD3-4C9F-A353-DC316B2CA0E0}</MetaDataID>
		private PersistencyFlag MemberPersistencyFlag;
		/// <MetaDataID>{90D345A3-A33A-4E9C-92D2-052EB712E4C5}</MetaDataID>
		public Persistenta(string Settings)
		{
			ExtMetaData=Settings;
		}

		/// <MetaDataID>{3250ADB1-750B-4081-B333-42B0E6114825}</MetaDataID>
		public Persistenta(PersistencyType persistencyType,int numberOfObject)
		{
			PersistencyType=persistencyType;
			NumberOfObject=numberOfObject;
			ExtMetaData=null;
			//MemberPersistencyFlag=PersistencyFlag.AtConstruction;
			
		}
		/// <MetaDataID>{90D345A3-A33A-4E9C-92D2-052EB712E4C5}</MetaDataID>
		public Persistenta(string Settings,PersistencyType persistencyType,int numberOfObject)
		{
			PersistencyType=persistencyType;
			NumberOfObject=numberOfObject;
			ExtMetaData=Settings;
		}
	
	}
}
