using System;

namespace OOAdvantech.Collections
{
	/// <summary>The Stack collection has the same functionality with the parent class the only deference is when deserialize data check for objects of type IExtMarshalByRefObject.
	/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
	/// <MetaDataID>{22643108-76D5-4374-B6E7-BFB6EFFCF340}</MetaDataID>
	[Serializable]
	public class Stack : System.Collections.Stack, System.Runtime.Serialization.IDeserializationCallback
	{
		public override object Clone()
		{
			return new Stack(this);
		}

		/// <MetaDataID>{FE07DD35-319F-46EA-97FB-24CE939EABFF}</MetaDataID>
		 public Stack()
		{
		}
		/// <MetaDataID>{39617C43-858C-4D67-9482-F2C99F84E014}</MetaDataID>
		 public Stack (int initialCapacity):base(initialCapacity)
		{
		}
		/// <MetaDataID>{4B5A1898-A1BC-4E12-B851-265E49BDC508}</MetaDataID>
		 public Stack (System.Collections.ICollection col):base(col)
		{
		}
#if !DeviceDotNet 
		/// <summary>Check for objects of type IExtMarshalByRefObject in collection.
		/// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
		/// <MetaDataID>{6A00CBF3-AF1D-4E3B-83B9-35CC397A07A4}</MetaDataID>
		public void OnDeserialization ( System.Object sender )
		{
			if(Count>0)
			{
				object[] StackElements=this.ToArray();
				Clear(); 
				for(int i=0;i!=StackElements.Length;i++)
				{
                    object CurrObject=StackElements[StackElements.Length-i-1];
                    CurrObject = OOAdvantech.Remoting.Proxy.ControlLifeTime(CurrObject);
                    //object CurrObject=StackElements[StackElements.Length-i-1];
                    //if(CurrObject is Remoting.IExtMarshalByRefObject &&Remoting.RemotingServices.IsOutOfProcess(CurrObject as MarshalByRefObject))
                    //    CurrObject=Remoting.Proxy.GetObject(CurrObject as Remoting.IExtMarshalByRefObject);
					Push(CurrObject);
				}
			}
		}
#endif
	}

}

namespace OOAdvantech.Collections.Generic
{
    /// <summary>The Stack collection has the same functionality with the parent class the only deference is when deserialize data check for objects of type IExtMarshalByRefObject.
    /// If it find then add a proxy class between client code and real proxy for the life time control of the object.</summary>
    /// <MetaDataID>{22643108-76D5-4374-B6E7-BFB6EFFCF340}</MetaDataID>
    [Serializable]
    public class Stack<T> : System.Collections.Generic.Stack<T>, System.Runtime.Serialization.IDeserializationCallback
    {

        /// <MetaDataID>{404385bb-1477-4284-8b2a-764a49912ee0}</MetaDataID>
        public Stack()
        {
        }
        /// <MetaDataID>{0ff82ff8-1f32-45b3-8400-468ddf79ef2c}</MetaDataID>
        public Stack(int initialCapacity)
            : base(initialCapacity)
        {
        }
        /// <MetaDataID>{47168b4a-f816-4f35-a073-40958db6ba24}</MetaDataID>
        public Stack(System.Collections.Generic.IEnumerable<T> collection)
            : base(collection)
        {
        }
#if !DeviceDotNet 
        public void OnDeserialization(System.Object sender)
        {
            if (Count > 0)
            {
                T[] StackElements = ToArray();
                Clear();
                for (int i = 0; i != StackElements.Length; i++)
                {
                    T currObject = StackElements[StackElements.Length - i - 1];
                    if (currObject == null)
                        Push((T)currObject);
                    else if (currObject is Remoting.IExtMarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(currObject as MarshalByRefObject))
                    {
                        currObject = (T)Remoting.Proxy.GetObject((Remoting.IExtMarshalByRefObject)currObject);
                        Push(currObject);
                    }

                }
            }
        }
#endif
    }

}
