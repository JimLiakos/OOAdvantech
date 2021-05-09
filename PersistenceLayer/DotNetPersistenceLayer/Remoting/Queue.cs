using System;

namespace OOAdvantech.Collections
{
	/// <summary>The Queue collection has the same functionality with the parent class the only deference is when deserialize data check for objects of type IExtMarshalByRefObject.
	/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
	/// <MetaDataID>{DDE9B1D7-29DA-4037-A134-3A5FA7D7326A}</MetaDataID>
	[Serializable]
	public class Queue : System.Collections.Queue, System.Runtime.Serialization.IDeserializationCallback
	{
		public override object Clone()
		{
			return new Queue(this);
		}

		/// <MetaDataID>{DF1D8351-4021-40FF-BC13-EAFB5FDE421F}</MetaDataID>
		 public Queue()
		{
		}
		/// <MetaDataID>{436F94FD-AFAE-47AB-8481-54B4D6F3F363}</MetaDataID>
		 public Queue (int initialCapacity):base(initialCapacity)
		{
		}
		/// <MetaDataID>{974048F6-20AC-4EF5-A811-1D694C100599}</MetaDataID>
		 public Queue (System.Collections.ICollection col):base(col)
		{
		}
		/// <MetaDataID>{AFFC83CB-824B-490D-A29C-3F8E4CAB035E}</MetaDataID>
		 public Queue (int capacity, float growFactor):base(capacity,growFactor)
		{
		}
#if !DeviceDotNet 
		/// <summary>Check for objects of type IExtMarshalByRefObject in collection.
		/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
		/// <MetaDataID>{F0A1F14F-EE17-4718-AE6D-066F862346CE}</MetaDataID>
		public void OnDeserialization ( System.Object sender )
		{
			if(Count>0)
			{
				object[] QueueElements=this.ToArray();
				Clear(); 
				for(int i=0;i!=QueueElements.Length;i++)
				{
					object CurrObject=QueueElements[i];
                    CurrObject = OOAdvantech.Remoting.Proxy.ControlLifeTime(CurrObject);
                    //if(CurrObject is Remoting.IExtMarshalByRefObject &&Remoting.RemotingServices.IsOutOfProcess(CurrObject as MarshalByRefObject))
                    //    CurrObject=Remoting.Proxy.GetObject(CurrObject as Remoting.IExtMarshalByRefObject);
					this.Enqueue(CurrObject);
				}
			}
		}
#endif
	}
}
