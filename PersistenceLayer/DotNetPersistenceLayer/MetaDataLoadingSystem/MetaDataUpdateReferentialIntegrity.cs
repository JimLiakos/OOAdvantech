using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
	/// <MetaDataID>{8DA84816-10EE-4B75-BD50-48441BB9FEFC}</MetaDataID>
	public class MetaDataUpdateReferentialIntegrity : PersistenceLayerRunTime.Commands.UpdateReferentialIntegrity
	{
		/// <MetaDataID>{12ACF894-C3DB-4BC8-A496-1D1B0AC1DF02}</MetaDataID>
		public override void Execute()
		{
			
			if(!UpdatedStorageInstanceRef.ReferentialIntegrityCountHasChanged)
				return;

			MetaDataStorageSession ObjectStorageSession=
				(MetaDataStorageSession)UpdatedStorageInstanceRef.ObjectStorage;

			MetaDataStorageInstanceRef mMetaDataStorageInstanceRef=(MetaDataStorageInstanceRef)UpdatedStorageInstanceRef;
			XElement theXmlElement=mMetaDataStorageInstanceRef.TheStorageIstance;
			ObjectStorageSession.NodeChangedUnderTransaction(theXmlElement,this.OwnerTransactiont);

			theXmlElement.SetAttribute("ReferentialIntegrityCount",mMetaDataStorageInstanceRef.ReferentialIntegrityCount.ToString());


			ObjectStorageSession.Dirty=true;

		
		}
	}
}
