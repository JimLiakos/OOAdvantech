using System;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Soap;

namespace OOAdvantech.PersistenceLayerRunTime
{
    //[Serializable]
    //public struct MemberObjeData
    //{
    //    public MemberObjeData(object _object, Collections.Hashtable objects)
    //    {
    //        ObjectHashCode = 0;
    //        Objects = null;
    //        if (_object != null)
    //        {
    //            Objects = objects;
    //            ObjectHashCode = _object.GetHashCode();
    //            if (!Objects.ContainsKey(ObjectHashCode))
    //                Objects[ObjectHashCode] = _object;
    //        }
    //    }
    //    int ObjectHashCode ;
    //    Collections.Hashtable Objects;
    //    public object Value
    //    {
    //        get
    //        {
    //            if (ObjectHashCode == 0)
    //                return null;
    //            else
    //                return Objects[ObjectHashCode];
    //        }
    //    }

    //}

	/// <MetaDataID>{0E21ED76-B02B-46D2-99AA-1A87BA3E06DA}</MetaDataID>
	public abstract class StructureSet : PersistenceLayer.StructureSet
	{ 
		/// <MetaDataID>{DD0DCD74-0AEF-4EDA-A875-2FBA8B6C7D57}</MetaDataID>
		protected DataBlock GetInstanceOfDataBlock()
		{
			
			return new DataBlock();
		}
		/// <MetaDataID>{95AE0CDA-9B69-4812-95BC-18DF90011B82}</MetaDataID>
		public abstract object GetData();
		/// <MetaDataID>{AB446478-6967-4BEE-9F95-BE0AEEA5AE37}</MetaDataID>
		[Serializable]
		public class DataBlock
		{
			/// <MetaDataID>{C9352DE0-43F9-4F96-B0F8-45C635E17BA8}</MetaDataID>
			internal DataBlock()
			{
			}
			/// <MetaDataID>{BC4BAD25-4CFE-4DA9-A98A-2C9B724E02E5}</MetaDataID>
			public System.Collections.Generic.List<object> ColumnsWithObject=new System.Collections.Generic.List<object>();
			/// <MetaDataID>{902BC0F7-3BA8-4F72-84C7-03CDC4BDE69F}</MetaDataID>
			public OOAdvantech.Collections.Generic.Dictionary<object,object> Objects =new Collections.Generic.Dictionary<object, object>();
			/// <MetaDataID>{C3E4F28C-FAAD-4DDA-9AA8-BFE6DAC58A31}</MetaDataID>
			public MetaDataRepository.ObjectQueryLanguage.IDataSet Data =null;
		} 
	}
}
