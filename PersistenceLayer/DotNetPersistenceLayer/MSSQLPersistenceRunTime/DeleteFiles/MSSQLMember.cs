namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

	/// <MetaDataID>{B9D7D38B-9B32-45B7-A9D3-C859222C3C46}</MetaDataID>
	public class Member : PersistenceLayer.Member
	{
        protected Member()
        {

        }
        public virtual void LoadRelatedObjects()
        {
        }
        public Member(DataNode dataNode, MemberList owner)
        {
            MemberMedata = dataNode;
            dataNode.StructureSetMember = this;
            Owner = owner;
            if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                _Name=dataNode.Alias;
            else
                _Name=dataNode.Name;
            _ID = (short)dataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.AssignedMetaObject.Name);

        }
		/// <MetaDataID>{91E4B733-A880-4923-BAFD-B585DA0419C6}</MetaDataID>
		public override System.Type Type
		{
			get
			{
				return null;
			}
		}
		/// <MetaDataID>{10BF50A0-CBDB-4A36-B8E5-CE0766F75E9D}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected string _Name;
		/// <MetaDataID>{BFBE412C-9EE1-4D2D-992B-F3EC12CE0EFE}</MetaDataID>
		public override string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name=value;
			}

		}
		/// <MetaDataID>{94E85B00-EE82-48BE-AE95-F1BAC758B9E9}</MetaDataID>
		public DataNode MemberMedata;
		/// <MetaDataID>{1357BDA6-1FA0-40FD-BB27-4A9683D10488}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private short _ID;
		/// <MetaDataID>{A0C075DD-342C-4587-91DC-97FA5587223E}</MetaDataID>
		public override short ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID=value;
			}
		}
		/// <MetaDataID>{03A76AEE-E357-4B3F-997C-DA311060F787}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private object _Value;
		/// <MetaDataID>{A507EFB0-41D3-426B-9AA5-D1068CD0E5EF}</MetaDataID>
		public override object Value
		{
			get
			{
				if(_ID==-1)
					throw new System.Exception("Member isn't initialized properly");
					
				return ((MemberList)Owner).DataRecord[_ID];
			}
			set
			{
			}
		}
	}
}
