namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	using OOAdvantech.Transactions;

	/// <MetaDataID>{B4F7F8A3-53F8-46C3-971B-906ED7136751}</MetaDataID>
	[BackwardCompatibilityID("{B4F7F8A3-53F8-46C3-971B-906ED7136751}")]
	[Persistent()]
	public class SubProductPrice : ProductPrice
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{9D1DBD5E-571C-430A-8BAF-30A481776A61}</MetaDataID>
		private string _name;
		/// <MetaDataID>{A1659921-3101-4F6F-A401-D9BA9A859F2A}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember(255,"_name")]
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C060F506-C9C4-49D6-A05B-7797661C8FF2}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{514ED4BA-1684-4A92-8510-5929009373DC}</MetaDataID>
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
		
		/// <MetaDataID>{8619D44A-7013-449F-B585-602A00B2F484}</MetaDataID>
		 protected SubProductPrice()
		{
		}
		/// <MetaDataID>{F3B6278D-CEE1-4D2C-BA74-D0E2DDF2B7B2}</MetaDataID>
		 public SubProductPrice(string name):base( name)
		{
			 this._name=name+" name";
			 this._Name=name+" new Name";

		}
	}
}
