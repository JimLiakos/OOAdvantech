namespace XMLPersistenceRunTime
{
	/// <MetaDataID>{74A8490B-DC8C-4587-98D7-F89BDE8AFC47}</MetaDataID>
	public class XMLRelResolver : PersistenceLayerRunTime.RelResolver
	{
		/// <summary>GetLinkedObjectStoragerRefs</summary>
		/// <MetaDataID>{D19B2741-C3AA-43CE-8AFE-16C0979CB044}</MetaDataID>
		public override System.Collections.ArrayList GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
		{
			return null;
		}
		/// <MetaDataID>{6CCAC576-3160-4F7B-9E52-3520370B1946}</MetaDataID>
		//public METAMODELLib.MRole DestinationObjectRole;
		/// <MetaDataID>{28E4A3E0-1B0B-4EA0-AF9B-91067E98F720}</MetaDataID>
		public override PersistenceLayerRunTime.TransactionCommandCollection UnLinkObject(PersistenceLayerRunTime.StorageInstanceRef theObject)
		{
			/*
			XMLUnLinkObjectsCommand mXMLUnLinkObjectsCommand=new XMLUnLinkObjectsCommand();
			if(DestinationObjectRole.IsRoleA!=0)
			{
				mXMLUnLinkObjectsCommand.RoleA=theObject;
				mXMLUnLinkObjectsCommand.RoleB=this.Owner;
			}
			else
			{
				mXMLUnLinkObjectsCommand.RoleB=theObject;
				mXMLUnLinkObjectsCommand.RoleA=this.Owner;

			}
			mXMLUnLinkObjectsCommand.theResolver=this;
			PersistenceLayerRunTime.TransactionCommandCollection TempTransactionCommands=new PersistenceLayerRunTime.TransactionCommandCollection();
			TempTransactionCommands.Add(mXMLUnLinkObjectsCommand);
			return TempTransactionCommands;*/
			return new PersistenceLayerRunTime.TransactionCommandCollection();

		}
		/// <MetaDataID>{E6AF9FBA-6D4B-47E7-8BD8-EBFC9078A1DB}</MetaDataID>
		public override PersistenceLayerRunTime.TransactionCommandCollection LinkObject(PersistenceLayerRunTime.StorageInstanceRef theObject)
		{
			/*XMLLinkObjectsCommand mXMLLinkObjectsCommand=new XMLLinkObjectsCommand();
			if(DestinationObjectRole.IsRoleA!=0)
			{
				mXMLLinkObjectsCommand.RoleA=theObject;
				mXMLLinkObjectsCommand.RoleB=this.Owner;
			}
			else
			{
				mXMLLinkObjectsCommand.RoleB=theObject;
				mXMLLinkObjectsCommand.RoleA=this.Owner;

			}
			mXMLLinkObjectsCommand.theResolver=this;*/
			PersistenceLayerRunTime.TransactionCommandCollection TempTransactionCommands=new PersistenceLayerRunTime.TransactionCommandCollection();
			//TempTransactionCommands.Add(mXMLLinkObjectsCommand);
			return TempTransactionCommands;
		}
		/// <MetaDataID>{F8623482-84EF-454B-8432-3D721FEA3071}</MetaDataID>
		public override System.Collections.Hashtable GetLinkedObjects(string Criterion)
		{
			return null;

		}
		/// <MetaDataID>{C0959556-7176-4C1F-A0E5-2B2534156B04}</MetaDataID>
		public override long GetLinkedObjectsCount()
		{
			return 0;
		}
		/// <MetaDataID>{881F5005-1AB9-42E1-99DF-350672BFC07C}</MetaDataID>
		public override void UnLinkAllObjects(object theObject)
		{
		
		}
	}
}
