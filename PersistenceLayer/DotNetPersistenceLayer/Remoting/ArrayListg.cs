using System;

namespace OOAdvantech.Collections
{
	
	/// <summary>The array list collection has the same functionality with the parent class the only deference is when deserialize data check for objects of type IExtMarshalByRefObject.
	/// If it find then add a proxy class between client code and real proxy for the life time control of the object.
	/// </summary>
	/// <MetaDataID>{6AC43F8D-6C07-4BDF-9DC3-BBABC598C10C}</MetaDataID>
	[Serializable]
	public class ArrayList: System.Collections.ArrayList, System.Runtime.Serialization.IDeserializationCallback
	{
		public override object Clone()
		{
			return new ArrayList(this);
		}

		/// <MetaDataID>{A1808B92-33D9-4CAA-BAE7-AEFFFD5DE017}</MetaDataID>
		 public ArrayList(int capacity):base(capacity)
		{

		}
		/// <MetaDataID>{F4C83270-974C-4DAC-98FE-8BEE0D582E97}</MetaDataID>
		 public ArrayList(System.Collections.ICollection c):base(c)
		{
			

		}
		/// <MetaDataID>{9B61EC16-8F3F-4BF9-8C45-77534F2ADB9A}</MetaDataID>
		public ArrayList()
		{
			
		}
		/// <summary>Check for objects of type IExtMarshalByRefObject in collection.
		/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
		/// <MetaDataID>{0538BA0E-7B1F-4778-8F38-DA8D1066C00B}</MetaDataID>
		public void OnDeserialization ( System.Object sender )
		{
			for(int i=0;i!=this.Count;i++)
			{
				object CurrObject= this[i];
				if(CurrObject is Remoting.IExtMarshalByRefObject &&Remoting.RemotingServices.IsOutOfProcess(CurrObject as MarshalByRefObject))
					this[i]=Remoting.Proxy.GetObject(CurrObject as Remoting.IExtMarshalByRefObject);
			}
		}
	}
}
