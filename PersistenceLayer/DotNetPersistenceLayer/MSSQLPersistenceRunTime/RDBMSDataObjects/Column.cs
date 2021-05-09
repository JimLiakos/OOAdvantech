//using OOAdvantech.MSSQLPersistenceRunTime;
namespace OOAdvantech.RDBMSDataObjects
{
	/// <MetaDataID>{60B4AA00-ADB1-4F67-B396-9AA4DF54E45A}</MetaDataID>
	/// <summary>
	/// 	<see cref="Class">my comments </see> and <see cref="Class with no com">
	/// 	</see> dfsdf ECDL </summary>
	public class Column : OOAdvantech.MetaDataRepository.MetaObject
	{
		/// <summary>Produce the identity of class from the .net metada </summary>
		/// <MetaDataID>{543332FA-96A6-4F5C-A0FD-4C2A345F623A}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
		{
			get
			{
                if (_Identity == null)
                    _Identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + Name);
				return _Identity;
			}

		}
		/// <MetaDataID>{DE409AC1-238A-4007-865C-19EAC5E02DCA}</MetaDataID>
		public string Datatype;
		/// <MetaDataID>{B16A2E3A-AC80-40D9-A54B-F9E24BD9D5CC}</MetaDataID>
		public int Length=0;
		/// <MetaDataID>{6AD3070D-7B4F-41B9-ABF2-DEF130695EFC}</MetaDataID>
		public bool AllowNulls=true;
		/// <MetaDataID>{BF7E7968-9585-4179-A775-6D1D527FBE51}</MetaDataID>
		public bool IdentityColumn=false;
		/// <MetaDataID>{2C0A8EFD-B11E-466C-AA15-B4FD3B054AF5}</MetaDataID>
		public int IdentityIncrement=0;
		/// <MetaDataID>{9E234994-2018-4E21-A632-4377441CCEE5}</MetaDataID>
		public string ColumnType;


		/// <MetaDataID>{C9FA3623-54CE-47D8-9707-AF3538FF61EC}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			return new System.Collections.ArrayList();
		}
		/// <MetaDataID>{89BAC1FC-1D2F-4BE6-9983-9C7EA73B03E9}</MetaDataID>
		public string GetScript()
		{
			string script= Name +" "+ Datatype;
			if(Length>0)
				script+="("+Length.ToString()+")";
			script+=" ";
			if(AllowNulls)
				script+="NULL";
			else
				script+="NOT NULL";
			if(IdentityColumn)
				script+=" IDENTITY(1,"+IdentityIncrement.ToString()+")";
			return  script;
		}
		internal RDBMSMetaDataRepository.Column OriginColumn;

		/// <MetaDataID>{93CA11C5-686D-47D5-8F5E-450A31829EBA}</MetaDataID>
		public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
		{
			OriginColumn= (RDBMSMetaDataRepository.Column)OriginMetaObject;
			DataBaseColumnName=Name;
			Name=OriginColumn.Name;
			Length=OriginColumn.Length;
			AllowNulls=OriginColumn.AllowNulls;

			RDBMSMetaDataRepository.IdentityColumn IDColumn= OriginColumn as RDBMSMetaDataRepository.IdentityColumn;
			if(IDColumn!=null)
				ColumnType=IDColumn.ColumnType;
			if(OriginColumn.IsIdentity)
			{
				IdentityColumn=OriginColumn.IsIdentity;
				IdentityIncrement=OriginColumn.IdentityIncrement;
			}
			string ColumnDotNetType= OriginColumn.Type.FullName;
            
            if (OriginColumn.Type is MetaDataRepository.Enumeration)
                ColumnDotNetType = "enum";

			if(ColumnDotNetType==null)
				throw new System.Exception("MSSQLPersistenceRunTime can't find type of column "+OriginColumn.Name);

            if (Length > 0)
                Datatype = (Namespace.Namespace as DataBase).RDBMSSchema.GetDBType(ColumnDotNetType,true); //TypeDictionary.GetDBType(ColumnDotNetType);
            else
            {
                Datatype = (Namespace.Namespace as DataBase).RDBMSSchema.GetDBType(ColumnDotNetType, false); // TypeDictionary.GetDBType(ColumnDotNetType, false);
                Length = (Namespace.Namespace as DataBase).RDBMSSchema.GeDefaultLength(ColumnDotNetType); //TypeDictionary.GeDefaultLength(ColumnDotNetType);
            }
            if (!(Namespace.Namespace as DataBase).RDBMSSchema.IsTypeVarLength(ColumnDotNetType))//if (!TypeDictionary.IsTypeVarLength(ColumnDotNetType))
                Length = 0;

            (Namespace.Namespace as DataBase).RDBMSSchema.ValidateColumnName(this);
            
			OriginColumn.DataBaseColumnName=_Name;
			
		
		}
		public string DataBaseColumnName;
        public string DataBaseColumnDataType;
        public int DataBaseColumnLength;
        public bool DataBaseColumnAllowNulls;
        public bool DataBaseColumnIdentityColumn;

		/// <MetaDataID>{E0612D3C-A22F-4965-9315-6D9511A6E0F9}</MetaDataID>
		private bool NewColumn=true;
		/// <MetaDataID>{E95491A7-EE8B-45D2-BAE3-724C6E7AF164}</MetaDataID>
		public Column(string name,string dataType,int length,bool allowNulls,bool identityColumn, bool newColumn)
		{
			Datatype=dataType;
			Length=length;
			AllowNulls=allowNulls;
			Name=name;
			NewColumn=newColumn;
			IdentityColumn=identityColumn;
			if(!NewColumn)
			{
				DataBaseColumnName=name;
				DataBaseColumnDataType=dataType;
				DataBaseColumnLength=length;
				DataBaseColumnAllowNulls=allowNulls;
				DataBaseColumnIdentityColumn=identityColumn;
			}
		}
        //static System.Collections.ArrayList ExcludedConvertion=new System.Collections.ArrayList();
        //static Column()
        //{
        //    ExcludedConvertion.Add("int-image");
        //    ExcludedConvertion.Add("nchar-image");
        //    ExcludedConvertion.Add("nvarchar-image");
        //    ExcludedConvertion.Add("datetime-image");
        //    ExcludedConvertion.Add("bigint-image");
        //    ExcludedConvertion.Add("smallint-image");
        //    ExcludedConvertion.Add("tinyint-image");
        //    ExcludedConvertion.Add("decimal-image");
        //    ExcludedConvertion.Add("numeric-image");
        //    ExcludedConvertion.Add("money-image");
        //    ExcludedConvertion.Add("smallmoney-image");
        //    ExcludedConvertion.Add("float-image");
        //    ExcludedConvertion.Add("real-image");
        //    ExcludedConvertion.Add("smalldatetime-image");
        //    ExcludedConvertion.Add("ntext-image");
        //    ExcludedConvertion.Add("text-image");
        //    ExcludedConvertion.Add("sql_variant-image");
        //    ExcludedConvertion.Add("uniqueidentifier-image");


        //}

        //internal string ConvertStatement
        //{
        //    get
        //    {
        //        if(ExcludedConvertion.Contains(DataBaseColumnDataType.ToLower()+"-"+Datatype.ToLower()))
        //            throw new System.Exception("ExcludedConvertion"+DataBaseColumnDataType.ToLower()+"-"+Datatype.ToLower());

        //        string convertStatement= "CONVERT("+Datatype;
        //        if(Length>0)
        //            convertStatement+="("+Length.ToString()+")";
        //        convertStatement+=","+Name+")";
        //        return convertStatement;
        //    }
        //}
		public bool HasChangeColumnFormat
		{
			get
			{
				return !NewColumn&&(DataBaseColumnDataType.ToLower().Trim()!=Datatype.ToLower().Trim()||
				(DataBaseColumnLength!=Length&&Length!=0)||
				DataBaseColumnAllowNulls!=AllowNulls||
				DataBaseColumnIdentityColumn!=IdentityColumn);
			}
		}
		/// <MetaDataID>{5D05CA3E-6166-4FED-8783-1E6DA516E722}</MetaDataID>
		public Column()
		{
		}

	}
}

