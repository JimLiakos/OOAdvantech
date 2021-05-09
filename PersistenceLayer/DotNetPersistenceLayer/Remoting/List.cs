using System;

using System.Text;

namespace OOAdvantech.Collections.Generic
{
    /// <MetaDataID>{fc0384c5-4e13-420f-91a6-e186ce9832be}</MetaDataID>
    [Serializable]
    public class List<T> : System.Collections.Generic.List<T>, System.Runtime.Serialization.IDeserializationCallback
    {

        /// <summary>
        ///     Initializes a new instance of the System.Collections.Generic.List<T> class
        ///     that is empty and has the default initial capacity.
        ///</summary>
        public List()
        {
        }
        ///
        ///<summary>
        ///     Initializes a new instance of the System.Collections.Generic.List<T> class
        ///     that contains elements copied from the specified collection and has sufficient
        ///     capacity to accommodate the number of elements copied.
        ///</summary>
        ///
        ///<param name="collection">
        ///    The collection whose elements are copied to the new list.
        /// </param>
        ///<exception cref="System.ArgumentNullException">
        ///     collection is null.
        ///</exception>     
        public List(System.Collections.Generic.IEnumerable<T> collection)
            : base(collection)
        {
        }
        
        ///<summary>
        ///Initializes a new instance of the System.Collections.Generic.List<T> class
        ///that is empty and has the specified initial capacity.
        ///</summary>
        ///<param name="capacity">
        ///     The number of elements that the new list can initially store.
        ///</param> 
        ///<exception cref="System.ArgumentOutOfRangeException">
        ///capacity is less than 0.
        ///</exception>
        public List(int capacity)
            : base(capacity)
        {
            
        }

#if !DeviceDotNet
        /// <MetaDataID>{2f503fc6-f31e-4c48-bfd4-9ee0dc83484c}</MetaDataID>
        void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender)
        {
            for (int i = 0; i != this.Count; i++)
            {
                T currObject = this[i];
                if(currObject!=null)
                    this[i] =(T)OOAdvantech.Remoting.Proxy.ControlLifeTime(currObject);
                //if (currObject is Remoting.IExtMarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(currObject as MarshalByRefObject))
                //    this[i] = (T)Remoting.Proxy.GetObject(currObject as Remoting.IExtMarshalByRefObject);
            }
        }
#endif

#if NetStandard
        void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender)
        {
            //for (int i = 0; i != this.Count; i++)
            //{
            //    T currObject = this[i];
            //    if(currObject!=null)
            //        this[i] =(T)OOAdvantech.Remoting.Proxy.ControlLifeTime(currObject);
            //    //if (currObject is Remoting.IExtMarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(currObject as MarshalByRefObject))
            //    //    this[i] = (T)Remoting.Proxy.GetObject(currObject as Remoting.IExtMarshalByRefObject);
            //}
        }
#endif

    }
}
