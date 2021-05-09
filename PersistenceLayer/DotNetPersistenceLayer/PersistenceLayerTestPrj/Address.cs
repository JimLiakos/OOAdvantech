using OOAdvantech;
using StorageSession=OOAdvantech.PersistenceLayer.StorageSession;
namespace PersistenceLayerTestPrj
{

	/// <MetaDataID>{18006861-4774-47B3-A398-EE5C53B63571}</MetaDataID>
	/// <summary>asden</summary>
	[OOAdvantech.MetaDataRepository.MetaObjectID("{18006861-4774-47B3-A398-EE5C53B63571}")]
	[OOAdvantech.PersistenceLayer.Persistent("{18006861-4774-47B3-A398-EE5C53B63571}")]
	public class Address :System.MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject
	{
		public override string ToString()
		{
			return "Address ( City: "+City+" ,Area: "+Area+" ,Street: "+Street+" )";
		}
					  

		/// <MetaDataID>{C074CBFF-E369-4BC7-B152-99FB9B86EF2C}</MetaDataID>
		 public Address()
		{
			City="Athens";
		}
		public void foo()
		{
			throw new System.NotImplementedException("mitras");
			OOAdvantech.PersistenceLayer.StructureSet aStructureSet=OOAdvantech.PersistenceLayer.StorageSession.GetStorageOfObject(this).Execute("SELECT theAddress FROM PersistenceLayerTestPrj.Address theAddress");
 
			Properties.SetProperty(typeof(OOAdvantech.Transactions.ObjectStateSnapshot).FullName,new OOAdvantech.Transactions.ObjectStateSnapshot(this));
			int Count=0;
			foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				Count++;
				PersistenceLayerTestPrj.Address theAddress=Rowset["theAddress"] as PersistenceLayerTestPrj.Address;
				object ID=OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(theAddress).ObjectID;
				if(Count>3)
					break;
			}
			Count=0;
			foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				Count++;
				PersistenceLayerTestPrj.Address theAddress=Rowset["theAddress"] as PersistenceLayerTestPrj.Address;
				object ID=OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(theAddress).ObjectID;
				if(Count>10000)
					break;
			}

			aStructureSet=StorageSession.GetStorageOfObject(StorageSession.GetStorageOfObject(this).StorageMetaData).Execute
				("SELECT theClass FROM "+typeof(OOAdvantech.MetaDataRepository.Class)+" theClass");
			Count=0;
			foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				Count++;
				OOAdvantech.MetaDataRepository.Class theClass=Rowset["theClass"] as OOAdvantech.MetaDataRepository.Class;
				object ID=OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(theClass).ObjectID;
				if(Count>3)
					break;

			}

			foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				Count++;
				OOAdvantech.MetaDataRepository.Class theClass=Rowset["theClass"] as OOAdvantech.MetaDataRepository.Class;
				object ID=OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(theClass).ObjectID;
				if(Count>1000)
					break;

			}


		}
		/// <summary></summary>
		/// <MetaDataID>{83FEC4C4-6C26-47DC-B5CC-ABDD1E5B4951}</MetaDataID>
		[OOAdvantech.PersistenceLayer.PersistentField(50)]
		[OOAdvantech.MetaDataRepository.MetaObjectID("1")]
		public string City;
		/// <summary>
		/// </summary>
		
		public OOAdvantech.ExtensionProperties Properties;
		/// <summary></summary>
		/// <MetaDataID>{A685A455-5A3B-4D77-9050-871EDAF96EA3}</MetaDataID>
		[OOAdvantech.PersistenceLayer.PersistentField(50)]
		[OOAdvantech.MetaDataRepository.MetaObjectID("2")]
		public string Street;
		/// <summary></summary>
		/// <MetaDataID>{0D5F724E-DA0E-4988-B367-CAFEACBEA4FF}</MetaDataID>
		[OOAdvantech.PersistenceLayer.PersistentField(50)]
		[OOAdvantech.MetaDataRepository.MetaObjectID("3")]
		public string Area;
	}
}
