namespace OOAdvantech.MSSQLPersistenceRunTime
{
	using System;

	/// <MetaDataID>{6C5A1B95-909E-4990-896B-B36ABEC9A503}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{6C5A1B95-909E-4990-896B-B36ABEC9A503}")]
	[Serializable()]
	public class ObjectID:IConvertible
	{


        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
            
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Guid))
                return this.IntObjID;
            throw new Exception("The method or operation is not implemented.");
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


		/// <MetaDataID>{B869009E-6876-495C-8D65-D0027606BE8F}</MetaDataID>
		public ObjectID()
		{

		}
		/// <MetaDataID>{5F03EE3F-4ADB-4E18-BC78-5C6EB848929D}</MetaDataID>
		public override string ToString()
		{

			return "ObjectID(ObjectID = "+IntObjID.ToString()+")";
		}
		/// <MetaDataID>{1E5807A9-A52F-41EA-A130-8DC2AB24C13D}</MetaDataID>
		public ObjectID(Guid IntObjIDValue,int ObjCellIDValue)
		{
			if(IntObjIDValue==System.Guid.Empty)
				IntObjID=System.Guid.NewGuid();
			else
				IntObjID=IntObjIDValue;
			ObjCellID=ObjCellIDValue;
		}
		/// <MetaDataID>{7685C941-BE1E-4683-A2FB-A06CC6D65461}</MetaDataID>
		public System.Guid IntObjID;
		/// <MetaDataID>{E2C562AD-6B81-45CB-A73F-33C7BD3F1519}</MetaDataID>
		public int ObjCellID;
		/// <MetaDataID>{4EABECF4-A338-44AB-A7BF-282B3531CF95}</MetaDataID>
		public override bool Equals(object obj)
		{
			if(obj is ObjectID)
			{
				ObjectID objectID=obj as ObjectID;
				if((objectID.IntObjID==IntObjID))//&&ObjCellID==objectID.ObjCellID)
					return true;
			}
			return false;
		}
		/// <MetaDataID>{77A99809-3851-4DA3-8EB9-002D1D6C5AB8}</MetaDataID>
		public object GetMemberValue(string memberName)
		{
			if(memberName=="ObjectID") 
				return IntObjID;
			throw new System.Exception("The "+memberName+" isn't member of Object ID.");
		}
		/// <MetaDataID>{B32488EF-2718-4930-9C00-9CB0B66675E8}</MetaDataID>
		public void SetMemberValue(string memberName,object value)
		{
			if(memberName=="ObjectID")
				IntObjID=(System.Guid)value;
		}



        
    }
}
