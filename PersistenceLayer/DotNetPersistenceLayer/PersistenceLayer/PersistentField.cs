namespace OOAdvantech.PersistenceLayer
{
	using System;
	/// <MetaDataID>{D9CF215F-1047-47B7-89FF-A46954EA68C7}</MetaDataID>
	/// <summary></summary>
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class PersistentFielda:Attribute
	{
		/// <MetaDataID>{210FC50D-0844-46B5-8A95-6CFFC613F350}</MetaDataID>
		public int Length=0;
		/// <summary></summary>
		/// <MetaDataID>{9D664D2B-01EF-4543-95FC-63CCFEE5746C}</MetaDataID>
		public PersistenceLayer.PersistencyFlag FieldFlag=PersistenceLayer.PersistencyFlag.OnConstruction;

	
		/// <MetaDataID>{06140548-6668-4392-9071-613D89D91D4F}</MetaDataID>
		public PersistentFielda ()
		{

		}
		/// <summary></summary>
		/// <MetaDataID>{D89A0807-ABDE-45F8-8658-FD71F626EE4E}</MetaDataID>
		/// <param name="mPersistencyFlag"></param>
		public PersistentFielda (PersistencyFlag mPersistencyFlag)
		{
			Length=0;
			/*int FieldID,*/
			FieldFlag=mPersistencyFlag;
		}
		/// <MetaDataID>{39C397ED-3DE8-4E13-82E2-087E04DF51F2}</MetaDataID>
		public PersistentFielda(int theLength )
		{
			Length=theLength;
			FieldFlag=PersistenceLayer.PersistencyFlag.OnConstruction;
		}
		/// <MetaDataID>{55CA24DB-2957-4F54-98B1-BC2FB692522C}</MetaDataID>
		public PersistentFielda (int theLength,PersistencyFlag mPersistencyFlag)
		{
			Length=theLength;
			/*int FieldID,*/
			FieldFlag=mPersistencyFlag;
		}
	}
}
