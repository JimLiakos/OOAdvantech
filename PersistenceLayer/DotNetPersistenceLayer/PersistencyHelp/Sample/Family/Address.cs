using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
namespace Family
{
	using System;
	using OOAdvantech.PersistenceLayer;
	using OOAdvantech.MetaDataRepository;
	using OOAdvantech.Transactions;
	using OOAdvantech.Remoting;

	/// <MetaDataID>{D9DE9C59-9CA5-4C89-8AC0-E0328CC64324}</MetaDataID>
	/// <summary>asden </summary>
	[BackwardCompatibilityID("{D9DE9C59-9CA5-4C89-8AC0-E0328CC64324}")]
	[Persistent()]
	[Serializable]
	public class Address:MarshalByRefObject,IExtMarshalByRefObject
	{
		public Person Person; 
		/// <MetaDataID>{1F558D51-54F7-4125-8E0A-5A5995BE8692}</MetaDataID>
		 public Address()
		{
		}
		/// <MetaDataID>{8B8AFFE8-0A8D-48D6-AA06-F6627F1CC511}</MetaDataID>
		 public Address(string city, string area, string street)
		{
			_City=city;
			_Area=area;
			_Street=street;
		}
	
		/// <MetaDataID>{EC14AA32-F410-424F-B162-436C8454B7F7}</MetaDataID>
		public OOAdvantech.ObjectStateManagerLink Properties;

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{4D3A7E21-04A1-45C4-8323-905973489126}</MetaDataID>
		private string _Area;
		/// <MetaDataID>{7DCC5EF6-ADB9-4C66-9168-15958354D2CF}</MetaDataID>

		[BackwardCompatibilityID("+3")]
		[PersistentMember(255,"_Area")]
		public string Area
		{
			get
			{
				return _Area;
			}
			set
			{
				if(_City!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Area=value;
						objStateTransition.Consistent=true;;
					}
				}			
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{A8F9AC5F-AC98-46BC-A555-3A32B24785A6}</MetaDataID>
		private string _Street;
		/// <MetaDataID>{D5CB8AEF-7906-43E6-8182-D301F208A86E}</MetaDataID>

		[BackwardCompatibilityID("+2")]
		[PersistentMember(255,"_Street")]
		public string Street
		{
			get
			{
				return _Street;
			}
			set
			{
				if(_City!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Street=value;
						objStateTransition.Consistent=true;;
					}
				}			
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{19600753-DE56-4774-B708-8ED2181A7E1B}</MetaDataID>
		private string _City;
		/// <MetaDataID>{5A7D3A5A-7348-43C0-898D-40285A51D98A}</MetaDataID>

		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_City")]
		public string City
		{
			get
			{
				return _City;
			}
			set
			{
				if(_City!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_City=value;
						objStateTransition.Consistent=true;;
					}
				}
			}
		}
	}
}
