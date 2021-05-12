namespace AbstractionsAndPersistency
{
    using OOAdvantech.Transactions;
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Remoting;
    using System;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{5D6E3DAA-ECC6-48DC-9E68-CD9CD61A0CFA}</MetaDataID>
    [BackwardCompatibilityID("{5D6E3DAA-ECC6-48DC-9E68-CD9CD61A0CFA}")]
    [Persistent]
    public class UnitMeasure : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IUnitMeasure
	{
        /// <MetaDataID>{90610f58-9ebd-4be8-8b7f-4a0b1c33e574}</MetaDataID>
        public UnitMeasure()
        {

        }

		/// <MetaDataID>{104C1537-8BDF-4C7C-B693-E5493737CC4E}</MetaDataID>
		private OOAdvantech.ObjectStateManagerLink Properties=new OOAdvantech.ObjectStateManagerLink();
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B6B5D1BE-DA04-4E23-A254-9E506E39304C}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{459BEA06-9BB5-41B6-B8FB-3BCDEC5A6961}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_Name")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}




        /// <MetaDataID>{94e5d655-9f70-4a3c-aa5c-aa6a863db3be}</MetaDataID>
        public Quantity Convert(Quantity value)
        {
            return value;
        }






        /// <MetaDataID>{bfcb318d-a7e8-4ee9-a8d4-27adfd0522b1}</MetaDataID>
        public bool CanConvert(Quantity value)
        {
            return false;
        }

        
    }
}
