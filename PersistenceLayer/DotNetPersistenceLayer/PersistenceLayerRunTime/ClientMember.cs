namespace OOAdvantech.PersistenceLayerRunTime.ClientSide
{
	/// <MetaDataID>{74C27FB3-833E-4348-A50C-12A7770DE010}</MetaDataID>
	public class MemberAgent : PersistenceLayer.Member
	{
		/// <MetaDataID>{ED608F3E-0D08-4F1D-BE49-2E55BC8D16F8}</MetaDataID>
		public override System.Type Type
		{
			get
			{
				return null;
			}
		}

		/// <MetaDataID>{667CC207-DB12-466A-B0E9-CF478D5B28AD}</MetaDataID>
		public MemberAgent(string name, PersistenceLayerRunTime.StructureSet.DataBlock dataSource, System.Collections.IEnumerator rowEnumerator)
		{
			_Name=name;
			DataSource=dataSource;
			RowEnumerator=rowEnumerator;
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{463589CB-F48C-41F3-8CCF-4805EDD9DB45}</MetaDataID>
		private short _ID;
		/// <MetaDataID>{E2A6FEE2-E35E-47D4-A895-F31DAA83C377}</MetaDataID>
		public override short ID
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{4043BACA-DEDB-4343-ADE1-E087A56E5D83}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{9C68C63A-DC5D-4023-B492-9F9428943B7E}</MetaDataID>
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
		/// <MetaDataID>{456C56AB-C456-4870-8AD5-E0B4CE7B6C31}</MetaDataID>
		private StructureSet.DataBlock DataSource;
	
		/// <MetaDataID>{76036E3D-E920-4FF4-8018-A291105759D6}</MetaDataID>
		private System.Collections.IEnumerator RowEnumerator;

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{20762F65-08C4-42FA-8DD8-2DAA7B2D4D83}</MetaDataID>
		private object _Value;
		/// <MetaDataID>{27442133-418D-4C7A-B73D-1190CAA0A3ED}</MetaDataID>
		public override object Value
		{
			get
			{
				System.Data.DataRow mDataRow =RowEnumerator.Current as System.Data.DataRow;
                //TODO θα να αλλάξει γιατί είνα αργό
                if (DataSource.ColumnsWithObject.Contains(mDataRow.Table.TableName+"_"+Name))
				{
					object Value=mDataRow[Name];
					if(Value==null||Value is System.DBNull)
						return null;
					return DataSource.Objects[Value];
				}
				else
					return mDataRow[Name];

			}
			set
			{
			}
		}
	}
}
