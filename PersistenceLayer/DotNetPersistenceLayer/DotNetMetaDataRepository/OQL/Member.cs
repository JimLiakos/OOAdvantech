namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

	/// <MetaDataID>{B9D7D38B-9B32-45B7-A9D3-C859222C3C46}</MetaDataID>
    [System.Serializable]
	public class Member : OOAdvantech.Collections.Member
	{
        /// <MetaDataID>{4eb03465-6c40-471f-ad37-e98e2ab5328c}</MetaDataID>
        protected Member(DataNode dataNode) 
        {
            DataPath = new DataPath();

            MemberMedata = dataNode;
             
        }
        /// <MetaDataID>{fa0fa025-7114-416a-9f94-2e8792a60911}</MetaDataID>
        public override bool DerivedMember
        {
            get 
            {
                if (DataPath == null || DataPath.Count == 0)
                    return false;
                else
                    return true;
            }
        }

        /// <MetaDataID>{7ea11f85-8a89-4da7-a3ea-a6a4eb726bf7}</MetaDataID>
        public virtual void LoadRelatedObjects()
        {
        }
        /// <MetaDataID>{a91f74c8-c013-4d3f-8ef9-2968a3725db1}</MetaDataID>
        protected DataPath DataPath;
        /// <MetaDataID>{9cdce49a-4393-4af2-8740-1d4cf7aeef02}</MetaDataID>
        bool MemberValueIsStructureSet;

        /// <MetaDataID>{1710d342-29aa-4194-bd1f-19bffa7ab759}</MetaDataID>
        public Member(DataNode dataNode, MemberList owner,DataPath dataPath, bool memberValueIsStructureSet)
        {
            MemberValueIsStructureSet = memberValueIsStructureSet;
            DataPath = dataPath;
            MemberMedata = dataNode;
            ValueTypePathDiscription = dataNode.ValueTypePathDiscription;
            
            Owner = owner;
            if (!string.IsNullOrEmpty(dataNode.Alias))
                _Name=dataNode.Alias;
            else
                _Name=dataNode.Name;
            try
            {
                if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) == typeof(System.DateTime))
                {
                    _ID = (short)dataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.ParentDataNode.Name);
                }
                else
                _ID = (short)dataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.ParentDataNode.ValueTypePathDiscription+ dataNode.AssignedMetaObject.Name);
            }
            catch (System.Exception error)
            {

            }
            bool hasLockRequest = HasLockRequest;

        }

        /// <MetaDataID>{f3c2cd29-f1b9-41cc-8f9d-d6183c2bda74}</MetaDataID>
        protected string ValueTypePathDiscription;
        /// <MetaDataID>{e6835b0d-2cca-4ba4-a158-e6fabd88596d}</MetaDataID>
        public Member(DataNode dataNode, MemberList owner)
        {
            DataPath = new DataPath();
     
            MemberMedata = dataNode;
            ValueTypePathDiscription = dataNode.ValueTypePathDiscription;
            Owner = owner;
            //if (dataNode.CountSelect)
            //{
            //    _Name = "Count";
            //    _ID = (short)dataNode.DataSource.DataTable.Columns.IndexOf(_Name);
            //}
            //else
            {


                if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                    _Name = dataNode.Alias;
                else
                    _Name = dataNode.Name;
                try
                {
                    if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) == typeof(System.DateTime))
                    {
                        _ID =(short)dataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.ParentDataNode.Name);
                    }
                    else if(dataNode.Type==DataNode.DataNodeType.Count)
                        _ID = (short)dataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.Alias);
                    else
                        _ID = (short)dataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.ParentDataNode.ValueTypePathDiscription+ dataNode.AssignedMetaObject.Name);
                }
                catch (System.Exception error)
                {

                }
            }
            DataPath = new DataPath();
            bool hasLockRequest = HasLockRequest;

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
        //[System.NonSerialized]
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

        /// <MetaDataID>{c3d8189a-47df-4b79-865c-548a3ea38a2c}</MetaDataID>
        int _HasLockRequest = -1;
        /// <MetaDataID>{ad0505ee-c2ac-4041-8fc6-65c6ba4bae91}</MetaDataID>
        internal bool HasLockRequest
        {
            get
            {
                if (_HasLockRequest == -1)
                {
                    foreach (DataNode dataNode in MemberMedata.SubDataNodes)
                    {
                        
                            if (dataNode.Name == "[Lock]")
                            {
                                _HasLockRequest = 1;
                                return true;
                            }
                        
                    }
                    _HasLockRequest = 0;
                    return false;
                }
                return _HasLockRequest == 1;

            }

        }

        //public bool IsLocked
        //{
        //    get
        //    { 
        //        if (!HasLockRequest)
        //            return false;
        //        else
        //            return (((MemberList)Owner).DataRecord["Locked"] as Transactions.Transaction)!=null;
        //    }
        //}
        //public Transactions.Transaction LockTransaction
        //{
        //    get
        //    {
        //        if (!HasLockRequest)
        //            return null;
        //        else
        //            return ((MemberList)Owner).DataRecord["Locked"] as Transactions.Transaction ;
        //    }
        //}

        /// <MetaDataID>{de672a24-b543-4026-897f-2a4392a96160}</MetaDataID>
        bool? IsEnum;
        /// <MetaDataID>{87c6a462-02b7-4f19-9584-ebfdadb23feb}</MetaDataID>
        System.Type EnumType;
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

                IDataRow StorageInstance = ((MemberList)Owner).DataRecord;
                DataNode rootDataNode = ((MemberList)Owner).RootDataNode;
                if (!MemberValueIsStructureSet)
                {
                    foreach (DataNode dataPath in DataPath)
                    {
                        System.Collections.Generic.ICollection<IDataRow> dataRow= rootDataNode.DataSource.GetRelatedRows(StorageInstance, dataPath);
                        if (dataRow.Count == 0)
                            return null;
                        else
                        {
                            foreach (IDataRow row in dataRow)
                            {
                                StorageInstance = row;
                                break;
                            }
                        }
                        rootDataNode = dataPath;

                        //if (StorageInstance.Table.ChildRelations.Contains(dataPath))
                        //{
                        //    System.Data.DataRow[] dataRow = StorageInstance.GetChildRows(dataPath);
                        //    if (dataRow.Length == 0)
                        //        return null;
                        //    StorageInstance = dataRow[0];
                        //}
                        //else if (StorageInstance.Table.ParentRelations.Contains(dataPath))
                        //    StorageInstance = StorageInstance.GetParentRow(dataPath);

                    }
                    if (!IsEnum.HasValue)
                    {
                        System.Type columnType=StorageInstance.Table.Columns[_ID].DataType;
                        IsEnum = columnType.GetMetaData().IsEnum;
                        EnumType=columnType;
                    }
                    if ((bool)IsEnum)
                    {
                        object value = System.Enum.ToObject(EnumType, StorageInstance[_ID]);
                        return value;
                    }
                    if (MemberMedata.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && MemberMedata.ParentDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) == typeof(System.DateTime))
                    {
                        System.Reflection.PropertyInfo propertyInfo = typeof(System.DateTime).GetMetaData().GetProperty(MemberMedata.Name);
                        if(!(StorageInstance[_ID] is System.DateTime))
                            return propertyInfo.GetValue(default(System.DateTime), null);
                        return propertyInfo.GetValue(StorageInstance[_ID],null);

                    }

                    return StorageInstance[_ID];
                }
                else
                {
                    System.Collections.Generic.ICollection<IDataRow> storageInstances = new System.Collections.Generic.List<IDataRow>();
                    storageInstances.Add(StorageInstance);

                    foreach (DataNode dataPath in DataPath)
                    {
                        //System.Collections.Generic.List<System.Data.DataRow> relatedStorageInstances = new System.Collections.Generic.List<System.Data.DataRow>();
                        //foreach (System.Data.DataRow dataRow in storageInstances)
                        //{

                        //    if (dataRow.Table.ChildRelations.Contains(dataPath))
                        //    {
                        //        System.Data.DataRow[] dataRows = dataRow.GetChildRows(dataPath);
                        //        if (dataRows.Length == 0)
                        //            return new System.Collections.ArrayList();
                        //        else
                        //            relatedStorageInstances.AddRange(dataRows);

                        //    }
                        //    else if (dataRow.Table.ParentRelations.Contains(dataPath))
                        //        relatedStorageInstances.AddRange(dataRow.GetParentRows(dataPath));
                        //}
                        //storageInstances = relatedStorageInstances;
                        storageInstances=rootDataNode.DataSource.GetRelatedRows(storageInstances, dataPath);
                        rootDataNode = dataPath;

                    }

                    System.Collections.Generic.List<object> values = new System.Collections.Generic.List<object>();
                    foreach (IDataRow dataRow in storageInstances)
                        values.Add(dataRow[_ID]);
                    if (values == null || values.Count == 0)
                        return null;
                    return values[0];
                }


					
				return StorageInstance[_ID];
			}
			set
			{
			}
		}
	}
}
