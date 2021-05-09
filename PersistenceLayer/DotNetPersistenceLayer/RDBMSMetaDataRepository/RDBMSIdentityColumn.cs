namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{9905C8C0-BF3E-4E32-802B-D63BA406C93A}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{9905C8C0-BF3E-4E32-802B-D63BA406C93A}")]
	[MetaDataRepository.Persistent()]
    public class IdentityColumn : Column, MetaDataRepository.IIdentityPart
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Alias))
            {
                if (value == null)
                    _Alias = default(string);
                else
                    _Alias = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ObjectIdentityTypeID))
            {
                if (value == null)
                    ObjectIdentityTypeID = default(string);
                else
                    ObjectIdentityTypeID = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ProducedFromRDBMS))
            {
                if (value == null)
                    ProducedFromRDBMS = default(bool);
                else
                    ProducedFromRDBMS = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ColumnType))
            {
                if (value == null)
                    ColumnType = default(string);
                else
                    ColumnType = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectIdentityType))
            {
                if (value == null)
                    _ObjectIdentityType = default(OOAdvantech.MetaDataRepository.ObjectIdentityType);
                else
                    _ObjectIdentityType = (OOAdvantech.MetaDataRepository.ObjectIdentityType)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Alias))
                return _Alias;

            if (member.Name == nameof(ObjectIdentityTypeID))
                return ObjectIdentityTypeID;

            if (member.Name == nameof(ProducedFromRDBMS))
                return ProducedFromRDBMS;

            if (member.Name == nameof(ColumnType))
                return ColumnType;

            if (member.Name == nameof(_ObjectIdentityType))
                return _ObjectIdentityType;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{2aa7e429-2932-4dde-853c-ca755b515eb7}</MetaDataID>
        string _Alias;
        /// <MetaDataID>{e3c5a3ef-87ee-4105-8e12-feb18134f9b4}</MetaDataID>
        public string Alias
        {
            get
            {
                return _Alias;
            }
            set
            {
                _Alias = value; 
            }
        }


        /// <MetaDataID>{c8ec85a5-1bca-4270-ba54-a0f40a2cef4e}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+3")]
        [MetaDataRepository.PersistentMember()]
        public string ObjectIdentityTypeID;


        ///<summary> 
        ///If the identity produced from RDBMS the value is true
        ///If the identity produced from object life time managment system the value is false
        ///</summary>
		/// <MetaDataID>{C38C7EF7-D868-4731-8CA3-A214708BAEBD}</MetaDataID>
		[MetaDataRepository.BackwardCompatibilityID("+2")]
		[MetaDataRepository.PersistentMember()]
		public bool ProducedFromRDBMS=false;
		//Error Prone thread safety
		/// <MetaDataID>{C3D3A42A-191B-4491-804A-693A57D4D37D}</MetaDataID>
		[MetaDataRepository.BackwardCompatibilityID("+1")]
		[MetaDataRepository.PersistentMember()]
		public string ColumnType;

        /// <MetaDataID>{06d4d96a-6724-4854-8c14-da0157e5ad64}</MetaDataID>
        protected IdentityColumn()
        {
            _AllowNulls = false;
             
        }

        /// <MetaDataID>{53db460a-b054-41d9-a4e1-7cb43172e289}</MetaDataID>
        MetaDataRepository.ObjectIdentityType _ObjectIdentityType;
        /// <MetaDataID>{c7d1bedd-3468-491f-8300-965e8256ad30}</MetaDataID>
        public MetaDataRepository.ObjectIdentityType ObjectIdentityType
        {
            get 
            {
                if (_ObjectIdentityType == null&& Namespace is RDBMSMetaDataRepository.Table)
                    _ObjectIdentityType=(Namespace as RDBMSMetaDataRepository.Table).GetObjectIdentityType(this);
                return _ObjectIdentityType;
            }
            set
            {
                _ObjectIdentityType = value;
            }
        }
        
        /// <MetaDataID>{dcd456b8-9b24-4327-a59d-1e678fe79109}</MetaDataID>
        public IdentityColumn(string name,  MetaDataRepository.Classifier type, string columnType,bool producedFromRDBMS)
        {
            
            _Name = name;
            _Type = type;
            ColumnType = columnType;
            ProducedFromRDBMS = producedFromRDBMS;
            _AllowNulls = false;
        }

        /// <MetaDataID>{aa3c904a-41c8-401e-8bf2-51f129788857}</MetaDataID>
        public IdentityColumn(string name, RDBMSMetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.Classifier type, string columnType, bool producedFromRDBMS)
        {
            MappedAssociationEnd = associationEnd;
            _Name = name;
            _Type = type;
            ColumnType = columnType;
            ProducedFromRDBMS = producedFromRDBMS;
            _AllowNulls = false;
        }

        #region IIdentityPart Members

        /// <MetaDataID>{26f48af7-d002-401d-941e-5b8e4fa54383}</MetaDataID>
        string OOAdvantech.MetaDataRepository.IIdentityPart.Name
        {
            get { return Name; }
        }

        /// <MetaDataID>{f473a037-5788-4acd-bd47-f701ce935348}</MetaDataID>
        string OOAdvantech.MetaDataRepository.IIdentityPart.PartTypeName
        {
            get { return ColumnType; }
        }

        /// <MetaDataID>{5d3b2a39-a252-4d28-8a33-45999d629125}</MetaDataID>
        System.Type OOAdvantech.MetaDataRepository.IIdentityPart.Type
        {
            get 
            {
                System.Type type = _Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                if (type == null)
                    type = _Type.GetExtensionMetaObject<System.Type>();
                return type;
            }
        }

        #endregion
    }
}
