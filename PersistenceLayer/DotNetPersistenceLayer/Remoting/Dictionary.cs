using System;
using System.Text;

namespace OOAdvantech.Collections.Generic
{
    /// <MetaDataID>{bb71f794-e65b-483f-8983-58616fe498e4}</MetaDataID>
    [Serializable]
    public class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
    {
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see> 
        ///     class that is empty, has the default initial capacity, and uses the default
        ///      equality comparer for the key type.
        /// </summary>
        /// <MetaDataID>{59338f7d-725c-4f5d-a5aa-bfabb518bb97}</MetaDataID>
        public Dictionary()
        {
            //Dictionary<int,int> era=new Dictionary<int,int>(
        }
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"></see>
        ///      and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">
        ///      The <see cref="T:System.Collections.Generic.IDictionary`2"></see> whose elements are
        ///      copied to the new <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///      dictionary contains one or more duplicate keys.
        ///  </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///      dictionary is null.
        ///  </exception>
        /// <MetaDataID>{e8cbfe0c-a10c-4df4-a209-f2dc09cf9216}</MetaDataID>
        public Dictionary(System.Collections.Generic.IDictionary<TKey, TValue> dictionary):base(dictionary)
        {
        }
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class that is empty, has the default initial capacity, and uses the specified
        ///      <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.
        /// </summary>
        /// <param name="comparer">
        ///      The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use
        ///      when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see>
        ///      for the type of the key.
        /// </param>
        /// <MetaDataID>{75a9e570-446f-4a2e-9740-05ce6396d9d1}</MetaDataID>
        public Dictionary(System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(comparer)
        {

        }
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class that is empty, has the specified initial capacity, and uses the default
        ///      equality comparer for the key type.
        ///  </summary>
        /// <param name="capacity">
        ///      The initial number of elements that the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      can contain.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///      capacity is less than 0.
        /// </exception>
        /// <MetaDataID>{e2a7940d-2ced-475e-aac4-da333991aa89}</MetaDataID>
        public Dictionary(int capacity)
            : base(capacity) 
        {
        }

        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"></see>
        ///      and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.
        ///  </summary>
        /// <param name="dictionary">
        ///      The <see cref="T:System.Collections.Generic.IDictionary`2"></see> whose elements are
        ///      copied to the new <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>.
        /// </param>
        /// <param name="comparer">
        ///      The  <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use
        ///      when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see>
        ///      for the type of the key.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///      dictionary contains one or more duplicate keys.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException:">
        ///      dictionary is null.
        /// </exception>
        /// <MetaDataID>{3baf9f1a-9c1b-4686-8f49-d2213db32098}</MetaDataID>

        public Dictionary(System.Collections.Generic.IDictionary<TKey, TValue> dictionary, System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {

        }
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class that is empty, has the specified initial capacity, and uses the specified
        ///      <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.
        ///  </summary>
        /// <param name="capacity">
        ///      The initial number of elements that the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      can contain.
        /// </param>
        /// <param name="comparer">
        ///      The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use
        ///      when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see>
        ///      for the type of the key.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///    System.ArgumentOutOfRangeException:
        ///      capacity is less than 0.
        /// </exception>
        /// <MetaDataID>{098ffd95-ced1-4a4f-8112-fec2d24ffa91}</MetaDataID>
        public Dictionary(int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {

        }

#if !DeviceDotNet 
        /// <summary>
        ///      Initializes a new instance of the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>
        ///      class with serialized data.
        ///  </summary>
        /// <param name="context">
        ///      A System.Runtime.Serialization.StreamingContext structure containing the
        ///      source and destination of the serialized stream associated with the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>.
        /// </param>
        /// <param name="info">
        ///      A System.Runtime.Serialization.SerializationInfo object containing the information
        ///      required to serialize the <see cref="T:OOAdvantech.Collections.Generic.Dictionary`2"></see>.
        /// </param>
        /// <MetaDataID>{9950d669-148d-4b98-b668-bac45ea0a276}</MetaDataID>
        protected Dictionary(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

        /// <MetaDataID>{d309bc79-7da1-4489-8382-da1c7a65aac1}</MetaDataID>
        public override void  OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
            //bool lifeTimeProxyForKey=typeof(TKey).GetInterface(typeof (OOAdvantech.Remoting.IExtMarshalByRefObject).FullName)!=null;
            //bool lifeTimeProxyForValue = typeof(TValue).IsInstanceOfType(typeof(OOAdvantech.Remoting.IExtMarshalByRefObject));
           
            if (Count > 0)
            {
                System.Collections.Generic.Dictionary<TKey, TValue> cloneDictionary= new System.Collections.Generic.Dictionary<TKey,TValue>(this);
                this.Clear();
                foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> entry in cloneDictionary)
                {
                    TKey key = entry.Key;
                    TValue value = entry.Value;

                    if(key!=null) 
                        key =(TKey)OOAdvantech.Remoting.Proxy.ControlLifeTime(key );
                    if(value!=null)
                        value = (TValue)OOAdvantech.Remoting.Proxy.ControlLifeTime(value);

                    //if (key is OOAdvantech.Remoting.IExtMarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(key as MarshalByRefObject))
                    //    key = (TKey)Remoting.Proxy.GetObject(key as Remoting.IExtMarshalByRefObject);
                    //if (value is OOAdvantech.Remoting.IExtMarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(value as MarshalByRefObject))
                    //    value = (TValue)Remoting.Proxy.GetObject(value as Remoting.IExtMarshalByRefObject);
                    Add(key, value);
                }
            }
        }
#endif


    }
}
