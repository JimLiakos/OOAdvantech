namespace OOAdvantech.Collections
{
	using System;
	/// <MetaDataID>{E88DAE88-D31D-4CF2-9604-483E75BA5BC9}</MetaDataID>
	[Serializable]
	public class Map : OOAdvantech.Collections.Hashtable
	{
		/// <MetaDataID>{2F5F1BB7-F406-4D8C-9A83-3B8DCFFDF6E2}</MetaDataID>
		public override void Clear()
		{
			base.Clear ();
		}
		/// <MetaDataID>{57341FCD-8EEC-46D4-9CA6-7BFFE7271AF1}</MetaDataID>
		public override void Remove(object key)
		{
			base.Remove (key);
		}



		/// <MetaDataID>{28989E01-190E-4DBD-A823-825DD6C7CFB8}</MetaDataID>
		public Map()
		{

		}
		/// <MetaDataID>{906E57F5-C956-4C8A-A34F-F5C57D3F2CA7}</MetaDataID>
		public Map(int capacity):base(capacity)
		{
		}
		/// <MetaDataID>{31664607-595B-4872-8741-68483B162400}</MetaDataID>
		public Map (System.Collections.IDictionary d):base(d)
		{
		}

#if !DeviceDotNet 
		/// <MetaDataID>{87F8E8C9-4360-43E0-8C8D-1E8184143354}</MetaDataID>
		protected Map (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context):base(info,context)
		{

		}
#endif
		/// <MetaDataID>{64265713-0507-498E-8352-BB4BE24F416E}</MetaDataID>
		public override object Clone()
		{
			return new Map(this);
		}


	}
}
