namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;

	/// <MetaDataID>{D8835DE9-B263-493F-B52B-5D3E27D92431}</MetaDataID>
	//[System.ComponentModel.TypeConverter(typeof(ControlConverter))]
    [BackwardCompatibilityID("{D8835DE9-B263-493F-B52B-5D3E27D92431}")]
    [Persistent()]
    public class Control : Component
	{
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A57CC7C6-2F58-4332-994B-B02AF528555D}</MetaDataID>
        private string _Caption;
        /// <MetaDataID>{FB080AF9-E3FE-438C-9F08-09A0A0568DF4}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember("_Caption")]
        public string Caption
        { 
            get
            { 
                return _Caption;
            }
            set
            {
                if (_Caption != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Caption = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{54E9B5B5-A706-4E41-9516-1D3C355400FF}</MetaDataID>
        public object Implementation;
        /// <MetaDataID>{5DF30F35-333A-47DE-8EEC-8457752E6105}</MetaDataID>
        [Association("HostedControl",typeof(OOAdvantech.UserInterface.ContainerControl),Roles.RoleB,"{5F15136F-FA5A-4D2A-AF59-0B100DC6E6B8}")]
        [PersistentMember("Parent")]
        [RoleBMultiplicityRange(0,1)]
        public ContainerControl Parent;
        /// <MetaDataID>{EECAB2FE-E1E2-4015-A61C-4CCD0198D3A0}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember()]
        private int Height;
        /// <MetaDataID>{1A1867F3-C675-4613-919C-C5BA91D84BF3}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember()]
        private int Ypos;
        /// <MetaDataID>{FAD501A5-06CB-48D0-9993-9FA7F4A51AC5}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember()]
        private int Xpos;
        /// <MetaDataID>{7995FE0E-F775-4E91-B5F2-E518DD09A77E}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember()]
        private int Width;

		/// <MetaDataID>{48CB810F-7EF4-47B1-93DA-3F6BA5781A35}</MetaDataID>
        public System.Drawing.Point Location
		{
			get
			{
                
				return new System.Drawing.Point(Xpos,Ypos);
			}
			set
			{
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    Xpos = value.X;
                    Ypos = value.Y;
                    stateTransition.Consistent = true;
                }


                //if(Implementation!=null&&Implementation.NativeControl!=null)
                //{
                //    System.Reflection.PropertyInfo propertyInfo=this.Implementation.NativeControl.GetType().GetProperty("Location");
                //    if(propertyInfo!=null)
                //        propertyInfo.SetValue(Implementation.NativeControl,Location,null);

                //}
			}
		}
	

		/// <MetaDataID>{CC5588A0-E68D-4280-940E-9B16D8DAE9A1}</MetaDataID>
        public System.Drawing.Size Size
		{
			set
			{
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    Height = value.Height;
                    Width = value.Width;
                    stateTransition.Consistent = true;
                }
			}
			get
			{
				return new System.Drawing.Size (Width,Height);
				
			}
		}
		



  
	}
    /// <MetaDataID>{E28D3960-C494-4F1A-808B-A51C1947288C}</MetaDataID>
    public class ControlConverter : System.ComponentModel.ExpandableObjectConverter
    {
        /// <MetaDataID>{BD40ECF1-19A5-4AED-B1BE-1ADFFD633288}</MetaDataID>
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destType)
        {
            if (destType == typeof(string) && value is Control)
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}
