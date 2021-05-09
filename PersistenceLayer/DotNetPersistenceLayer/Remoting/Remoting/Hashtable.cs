using System;
#if DeviceDotNet 
namespace System.Collections
{

    /// <MetaDataID>{10b63e16-7bee-40af-b832-8616be4810a4}</MetaDataID>
    public class Hashtable : System.Collections.Generic.Dictionary<object,object>
    {
    }
}
#endif
namespace OOAdvantech.Collections
{
	/// <summary>The Hashtable collection has the same functionality with the parent class the only deference is when deserialize data check for objects of type IExtMarshalByRefObject.
	/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
	/// <MetaDataID>{ABCEB6DD-E76D-471D-9CB0-54309EBA0C1B}</MetaDataID>
	[Serializable]
	public class Hashtable : System.Collections.Hashtable
	{
		public override object Clone()
		{
			return new Hashtable(this);
		}

		/// <MetaDataID>{EE65805A-A256-44D2-956F-DE6F7434FCA2}</MetaDataID>
		public Hashtable(int capacity)
		{
		
		}
	
#if !DeviceDotNet 
        	/// <MetaDataID>{3DA5E7F9-8452-42A1-BBDF-1701262AEFCB}</MetaDataID>
		 public Hashtable(System.Collections.IHashCodeProvider hcp, System.Collections.IComparer comparer):base(hcp,comparer)
		{
		
		}
		/// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
		/// <MetaDataID>{0F4B0257-B769-43BB-9051-5B60EA626AB2}</MetaDataID>
		 protected Hashtable (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context):base(info,context)
		{
		}
		/// <MetaDataID>{8F9162C5-EA6C-4FFB-8EA0-C06CFF30CDA1}</MetaDataID>
		 public Hashtable (System.Collections.IDictionary d, float loadFactor, System.Collections.IHashCodeProvider hcp, System.Collections.IComparer comparer):base(d,loadFactor,hcp,comparer)
		{
		}

#endif
        /// <MetaDataID>{E01F4F41-FC50-48B8-A627-924035423B40}</MetaDataID>
		 public Hashtable()
		{
		}

#if !DeviceDotNet 
        		/// <MetaDataID>{E0497E2D-1CE9-4679-829F-250D8AFE8DF6}</MetaDataID>
		 public Hashtable (System.Collections.IDictionary d, System.Collections.IHashCodeProvider hcp, System.Collections.IComparer comparer):base(d,hcp,comparer)
		{
		}
		/// <MetaDataID>{6ACFA889-BDCB-40D5-BFFE-B3D3AAE694D8}</MetaDataID>
		 public Hashtable (System.Collections.IDictionary d, float loadFactor):base(d,loadFactor)
		{
		}
		/// <MetaDataID>{CBEC4A06-0E4F-48B8-ABB8-1F58A8E6C85D}</MetaDataID>
		public Hashtable (System.Collections.IDictionary d):base(d)
		{
		}
        /// <MetaDataID>{9B8D058F-DE7F-41DA-A195-11992812B95B}</MetaDataID>
        public Hashtable(int capacity, float loadFactor)
            : base(capacity, loadFactor)
        {
        }
		/// <MetaDataID>{4057493A-87FA-4C50-A54C-C0689284C173}</MetaDataID>
		 public Hashtable (int capacity, System.Collections.IHashCodeProvider hcp, System.Collections.IComparer comparer):base(capacity,hcp,comparer)
		{
		}
	
		/// <MetaDataID>{EEFF7AF3-A730-4D35-9384-2510D3EBFCCE}</MetaDataID>
		 public Hashtable (int capacity, float loadFactor, System.Collections.IHashCodeProvider hcp, System.Collections.IComparer comparer):base(capacity,loadFactor,hcp,comparer)
		{
		}
	

		/// <summary>Check for objects of type IExtMarshalByRefObject in collection.
		/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
		/// <MetaDataID>{852DD9C8-F7BE-4D19-A333-036A43FE64DE}</MetaDataID>
		public override void OnDeserialization ( System.Object sender )
		{
			base.OnDeserialization(sender);
			if(Count>0)
			{
				System.Collections.Hashtable CloneHashtable=this.Clone() as System.Collections.Hashtable;
				this.Clear();
				foreach(System.Collections.DictionaryEntry CurrEntry in CloneHashtable)
				{
					object Key=CurrEntry.Key;
					object Value=CurrEntry.Value;

                    Key = OOAdvantech.Remoting.Proxy.ControlLifeTime(Key);
                    Value = OOAdvantech.Remoting.Proxy.ControlLifeTime(Value);
                    //if(Key is Remoting.IExtMarshalByRefObject &&Remoting.RemotingServices.IsOutOfProcess(Key as MarshalByRefObject))
                    //    Key=Remoting.Proxy.GetObject(Key as Remoting.IExtMarshalByRefObject);
                    //if(Value is Remoting.IExtMarshalByRefObject &&Remoting.RemotingServices.IsOutOfProcess(Value as MarshalByRefObject))
                    //    Value=Remoting.Proxy.GetObject(Value as Remoting.IExtMarshalByRefObject);
					Add(Key,Value);
				}
			}
		}
#endif

    }
}
