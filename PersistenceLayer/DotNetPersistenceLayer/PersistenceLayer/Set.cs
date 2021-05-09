namespace OOAdvantech.Collections
{
    /// <MetaDataID>{0194f518-1fe7-4f74-809c-d8bc5c714e42}</MetaDataID>
    public enum CollectionAccessType { ReadOnly, ReadWrite }
}
namespace OOAdvantech.Collections.Generic
{
    using System;
    
    using System.Linq;
    


	//using T=System.Object;
	//public class T{}
    /// <MetaDataID>{DDC137B9-F095-44C0-863A-C00B9631623C}</MetaDataID>
    /// // ,OOAdvantech.Transactions.TransactionalObject
    [Serializable]
    public class Set<T> : PersistenceLayer.ObjectContainer, OOAdvantech.Transactions.ITransactionalObject, System.Collections.Generic.ICollection<T>, System.Collections.ICollection, System.Collections.IList, System.Collections.Generic.IList<T>, System.Runtime.Serialization.ISerializable, ICollectionMember, IMemberInitialization
	{
        ///// <MetaDataID>{03e33b65-1642-410e-b002-6c419409bf5e}</MetaDataID>
        //Set<T> ReadOnlySet;
        /// <MetaDataID>{18e62a5a-f0e5-4a53-aee7-426d10b1ec05}</MetaDataID>
        public Set<T> AsReadOnly()
        {
            //if (ReadOnlySet == null)
            //    ReadOnlySet = new Set<T>(this, CollectionAccessType.ReadOnly);
            //return ReadOnlySet;
            return new Set<T>(this, CollectionAccessType.ReadOnly);
        }
        /// <MetaDataID>{046f589d-3adb-4924-9475-88f31974ad5c}</MetaDataID>
        protected override System.Collections.IEnumerator GetObjectEnumerator()
        {
            return (this as System.Collections.IEnumerable).GetEnumerator();
        }
        /// <MetaDataID>{c5733cf9-ec0c-4ec2-bb52-842719322735}</MetaDataID>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                array[arrayIndex++] = item;
            }
        }
        /// <MetaDataID>{b38f271c-fbe1-43ec-9967-365c9c819c36}</MetaDataID>
        public void Clear()
        {
            RemoveAll();
        }
        /// <MetaDataID>{01656366-dd48-44b4-bf59-3a1d3ddfbb5f}</MetaDataID>
        public bool IsReadOnly
        {
            get
            {
                return AccessType == CollectionAccessType.ReadOnly;
            }
        }
        /// <MetaDataID>{41093797-cad9-4a24-ab52-9cf8a752c335}</MetaDataID>
        public T this[int index]
        {
            get
            {
                object tmpObj=theObjects[index];
                if (tmpObj == null)
                    return default(T);
                else
                    return (T)tmpObj;
            }
            set
            {
                if (Contains((T)value) && IndexOf((T)value) != index)
                    throw new Exception("Duplicate value");
                RemoveAt(index);
                Insert(index, (T)value);
            }

        }
        class Enumerator<T>:System.Collections.Generic.IEnumerator<T> 
        {
            System.Collections.IEnumerator InnerEnumerator ;
            public Enumerator(System.Collections.IEnumerator enumerator)
            {
                InnerEnumerator = enumerator;
            }
        

            #region IEnumerator<T> Members

            public T Current
            {
                get { return (T)InnerEnumerator.Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return InnerEnumerator.Current; }
            }

            public bool MoveNext()
            {
                return InnerEnumerator.MoveNext();
            }

            public void Reset()
            {
                InnerEnumerator.Reset();
            }

            #endregion
        }
        /// <MetaDataID>{4E1801C2-3339-4CF1-A51F-7DBA6236F84B}</MetaDataID>
        public Set()
		{

		}
        /// <MetaDataID>{AC677D21-BF5B-4421-A274-345A31453121}</MetaDataID>
        public Set(System.Collections.Generic.ICollection<T> objectSet)
		{
            if (objectSet == null)
                return;
            if (objectSet is System.Collections.ICollection)
            {
                base.Init(objectSet as System.Collections.ICollection);
            }
            else if (objectSet.Count > 0)
            {
                Collections.Generic.List<object> collection = new List<object>();
                foreach (object obj in objectSet)
                    collection.Add(obj);
                base.Init(collection);
            }
            
		}


        /// <MetaDataID>{2545de3d-182b-4e7d-ab58-a406e9ad0ba8}</MetaDataID>
        CollectionAccessType AccessType = CollectionAccessType.ReadWrite;

        /// <MetaDataID>{762f700a-f5b8-42a2-bb1b-118a79804308}</MetaDataID>
        public Set(Set<T> objectSet, CollectionAccessType accessType)
        {
            AccessType=accessType;
            if (accessType == CollectionAccessType.ReadWrite)
                AddRange(objectSet);
            else if (OOAdvantech.Transactions.Transaction.Current != null)
                base.Init(objectSet);
            else
                _theObjects = objectSet.theObjects;
            
        }

        /// <MetaDataID>{7b90d16c-26a1-4698-bd4d-65fed129533b}</MetaDataID>
        public int IndexOf(T item)
        {
            if (item == null)
                return -1;
            return theObjects.IndexOf(item);
        }
        /// <MetaDataID>{9ba68bd8-0e8f-4735-bad4-50187a2024f1}</MetaDataID>
        public void Insert(int index, T item)
        {
            if (item == null)
                throw new System.Exception("System can't add null object in Set");
            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
            if (theObjects != null)
                theObjects.Insert(index,item);
        }
        /// <MetaDataID>{12f62787-4724-4bef-aadf-a610c203170b}</MetaDataID>
        public void InsertRange(int index,System.Collections.Generic.IEnumerable<T> collection)
        {

            if (collection == null)
                throw new System.Exception("System can't add null collection in Set");
            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
            if (theObjects != null)
            {
                foreach (T item in collection)
                    theObjects.Insert(index++, item);
            }

        }
        /// <MetaDataID>{90ff40b6-8baa-4f92-8a7a-2c1e0d3b8301}</MetaDataID>
        public void RemoveAt(int index)
        {
            theObjects.RemoveAt(index);

        }
        /// <MetaDataID>{96f30efc-1055-4804-bd86-09d129ca47bf}</MetaDataID>
        public void RemoveRange(int index, int count)
        {

        }
        /// <MetaDataID>{e7b5ee34-6c27-48c2-8196-9c6a022f9efd}</MetaDataID>
        public void Sort()
        {

        }

        /// <MetaDataID>{84484022-aa89-47bd-8ee6-d437d45a8ef2}</MetaDataID>
        public void Sort(Comparison<T> comparison)
        {

        }

        /// <MetaDataID>{9483b908-64e7-4d8b-b06a-c994767689ac}</MetaDataID>
        public void Sort(System.Collections.Generic.IComparer<T> comparer)
        {

        }

        /// <MetaDataID>{bf4562bc-9c4b-4f1a-b4e9-5a74d08a0a32}</MetaDataID>
        public void Sort(int index, int count,System.Collections.Generic.IComparer<T> comparer)
        {

        }
		
		#region TransactionalObject Members
        /// <MetaDataID>{51DA4A13-40B9-4F6E-A63C-B77591A3705E}</MetaDataID>
		public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
		{
            if (AccessType == CollectionAccessType.ReadOnly)
                return;
			if(theObjects!=null)
				theObjects.MarkChanges(transaction);
		}



        /// <MetaDataID>{a46e56c5-e9d4-44fa-8263-bb66ffe4917c}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            if (AccessType == CollectionAccessType.ReadOnly)
                return;

            if (theObjects != null)
                theObjects.MergeChanges(mergeInTransaction,mergedTransaction);
        }

        /// <MetaDataID>{a865d996-cb63-4b6a-abca-8479254937d3}</MetaDataID>
        public void EnsureSnapshot(Transactions.Transaction transaction)
        {
            if (AccessType == CollectionAccessType.ReadOnly)
                return;

            if (theObjects != null)
                theObjects.EnsureSnapshot(transaction);

        }
        /// <MetaDataID>{b61b87a7-e7a1-4838-8dbc-52ef74228142}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            if (AccessType == CollectionAccessType.ReadOnly)
                return;

            if (theObjects != null)
                theObjects.MarkChanges(transaction,fields);
        }



        /// <MetaDataID>{193741C2-A0B5-4B3D-B031-8668DDC44B9E}</MetaDataID>
		public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
		{
            if (AccessType == CollectionAccessType.ReadOnly)
                return;

			if(theObjects!=null)
				theObjects.UndoChanges(transaction);
		}
        /// <MetaDataID>{1160a1fd-10b0-487e-b568-15fe08bceabf}</MetaDataID>
        public bool ObjectHasGhanged(Transactions.TransactionRunTime transaction)
        {
            if (theObjects != null)
                return theObjects.ObjectHasGhanged(transaction);
            else
                return false;

        }
#if !DeviceDotNet
        /// <MetaDataID>{4605a74c-b1a4-4c55-b364-67ef10d107e2}</MetaDataID>
        public System.Linq.IQueryable<T> AsObjectContextQueryable
        {
            get
            {
                System.Linq.IQueryable queryable = null;
                if (theObjects != null)
                    queryable = theObjects.QueryableCollection;
                if (queryable == null)
                    return this.AsQueryable<T>();
                else
                    return (System.Linq.IQueryable<T>)queryable;
            }
        }
#endif
        /// <MetaDataID>{31F4A507-FF52-4F8F-8F4C-CA5C6C25FD28}</MetaDataID>
		public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
		{
            if (AccessType == CollectionAccessType.ReadOnly)
                return;

			if(theObjects!=null)
				theObjects.CommitChanges(transaction);
		}
		#endregion

        /// <MetaDataID>{2C57EB41-2A6D-4686-AB7D-BFF728F98307}</MetaDataID>
		public void RemoveAll()
		{
            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
			if(theObjects!=null)
				theObjects.RemoveAll();
		}
        /// <MetaDataID>{F19E7643-C890-4FF2-8B63-193B6EF39B3D}</MetaDataID>
        public bool Contains(T Object)
		{
			if (theObjects!=null)
				return theObjects.Contains(Object);
			else
				return false;
		}
		
	
		/// <MetaDataID>{69D0D784-692A-46BE-85D9-30CE7A901A11}</MetaDataID>
        public int Count
		{
			get
			{
				if (theObjects!=null)
					return (int)theObjects.Count;
				else
					return 0;
			}
		}

        /// <MetaDataID>{AD23DBE5-5061-4095-AA0E-88FD4F770F94}</MetaDataID>
        public virtual void AddRange(System.Collections.Generic.IEnumerable<T> theObjectCollection)
		{
            if (theObjectCollection == null)
                return;

            if (AccessType==CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
			foreach (T CurrObject in theObjectCollection)
				Add(CurrObject);
		}
        /// <MetaDataID>{4c94f865-1cf8-433c-a39c-4ee134e12a09}</MetaDataID>
        public System.Collections.Generic.List<T> ToThreadSafeList()
        {
            if (theObjects != null)
            {
                return theObjects.ToThreadSafeList().OfType<T>().ToList();
            }
            else
                return null;
        }

        /// <MetaDataID>{c8896973-ecb5-45e5-9820-350195351e80}</MetaDataID>
        public Set<T> ToThreadSafeSet()
        {
            if (theObjects != null)
            {
                return theObjects.ToThreadSafeSet(typeof(T)) as Set<T>;
            }
            else
                return null;
        }

        /// <MetaDataID>{5D15257F-062B-479E-B050-8DB6AF20ACE2}</MetaDataID>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
  		{
            
			if(theObjects!=null)
				return new Enumerator<T>(theObjects.GetEnumerator());
			else
			{
				System.Collections.Generic.List<object> tmp=new System.Collections.Generic.List<object>();
				return new Enumerator<T>(tmp.GetEnumerator());
			}
		}

        /// <MetaDataID>{a6c2ac13-d5e0-44c2-81ab-d1d55474afb3}</MetaDataID>
        public bool CanDeletePermanently(T theObject)
        {
            if (theObjects != null)
                return theObjects.CanDeletePermanently(theObject);
            return false;
        }
        /// <MetaDataID>{4084B8E3-D245-4E6E-94F9-93E547E977CF}</MetaDataID>
        public bool Remove(T theObject)
		{
            if (theObject == null)
                throw new System.Exception("System can't remove null object in Set");

            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
			if(theObjects!=null)
				theObjects.Remove(theObject);

            //TODO θα πρέπει να γίνει σωστά
            return true;
		}
        /// <MetaDataID>{E40128C9-2EAF-44B9-82A5-338BB4843A0F}</MetaDataID>
        public virtual void Add(T theObject)
		{
            if (theObject == null)
                throw new System.Exception("System can't add null object in Set");

            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");

			if(theObjects!=null)
				theObjects.Add(theObject);

        }
        #region Excluded for .Net CompactFramework
#if !PORTABLE 
        /// <MetaDataID>{9f25a028-05ec-4730-80ae-2014a75502c2}</MetaDataID>
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (AccessType == CollectionAccessType.ReadOnly)
                _theObjects=new Set<T>(this)._theObjects;
            info.AddValue("_theObjects", this._theObjects, typeof(OOAdvantech.PersistenceLayer.ObjectCollection));

        }


        /// <MetaDataID>{b3c61ab0-0ade-4de7-8119-e2207b6e9e7d}</MetaDataID>
        private Set(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctxt)
        {
            this._theObjects = info.GetValue("_theObjects", typeof(OOAdvantech.PersistenceLayer.ObjectCollection)) as OOAdvantech.PersistenceLayer.ObjectCollection;
        }
#endif
        #endregion








        #region IEnumerable<T> Members

        /// <MetaDataID>{e8a26c85-647f-4c1b-95d9-de2452b12792}</MetaDataID>
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{3b738bba-d5c5-4b4e-b5a4-664b06871c0e}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
            
        }

        #endregion

        #region ICollection Members

        /// <MetaDataID>{69617194-7344-4366-a126-9bc05ff06474}</MetaDataID>
        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            foreach (T item in this)
            {
                array.SetValue(item,index++);
            }
        }

        /// <MetaDataID>{9bc25801-9030-489d-8f7a-726be4216499}</MetaDataID>
        int System.Collections.ICollection.Count
        {
            get { return Count; }
        }

        /// <MetaDataID>{9fd61965-8e23-44c5-9999-45681b8c2cd3}</MetaDataID>
        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <MetaDataID>{5c4d047f-a8da-42de-80d3-11abf5a2aba7}</MetaDataID>
        object _SyncRoot = new object();
        /// <MetaDataID>{75f1399d-33c0-4fa9-873d-8449cc2a75ad}</MetaDataID>
        object System.Collections.ICollection.SyncRoot
        {

            get { return _SyncRoot; }
        }

        #endregion






        #region ICollectionMember Members

        /// <MetaDataID>{a474f11d-3229-4b95-a027-733e73254e05}</MetaDataID>
        void ICollectionMember.AddImplicitly(object _object)
        {
            if (_object == null)
                return;
            if (_object != null && !(_object is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            (theObjects as ICollectionMember).AddImplicitly(_object);
            
        }

        /// <MetaDataID>{f36625c6-1799-4106-8606-a2b62c8027a6}</MetaDataID>
        void ICollectionMember.RemoveImplicitly(object _object)
        {
            
            if (_object == null)
                return;

            if (_object != null && !(_object is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            (theObjects as ICollectionMember).RemoveImplicitly(_object);
        }

        /// <MetaDataID>{80c6a2d3-2b9c-4eca-8a12-d3acc21a85f2}</MetaDataID>
        System.Collections.IEnumerator ICollectionMember.GetEnumeretor()
        {
            return GetEnumerator();
        }

        #endregion

        #region IMemberInitialization Members

        /// <MetaDataID>{492cb1ff-299e-4737-a184-9adede4d3c92}</MetaDataID>
        void IMemberInitialization.SetOwner(object owner)
        {
            (theObjects as IMemberInitialization).SetOwner(owner);
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{29e407db-ec02-4407-b5e9-eb74ae007edd}</MetaDataID>
        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata)
        {
            (theObjects as IMemberInitialization).SetMetadata(metadata);
            //throw new NotImplementedException();
        }
        /// <MetaDataID>{b4a7bf78-5275-48d2-868f-f35ed7c338b7}</MetaDataID>
        void IMemberInitialization.SetMetadata(MetaDataRepository.MetaObject metadata, object relResolver)
        {
            (theObjects as IMemberInitialization).SetMetadata(metadata);
        }

        /// <MetaDataID>{df799c44-e335-4674-81d1-2b142bb90beb}</MetaDataID>
        bool IMemberInitialization.Initialized
        {
            get 
            {
                return (theObjects as IMemberInitialization).Initialized;
            }
        }

        #endregion

        #region IList Members

        /// <MetaDataID>{430bc516-02d7-4b51-b9b6-212684d95dbc}</MetaDataID>
        int System.Collections.IList.Add(object value)
        {
            if (value is T)
            {
                //TODO θα πρέπει να επιστρέφει από την add
                Add((T)value);
            }
            else
            {

            }
            return Count-1;
        }

        /// <MetaDataID>{0798098a-5c1a-4105-bf50-32101b1c3366}</MetaDataID>
        bool System.Collections.IList.Contains(object value)
        {
            return Contains((T)value);
        }

        /// <MetaDataID>{72bf295d-2656-431b-8cd4-6da7b44812b2}</MetaDataID>
        int System.Collections.IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        /// <MetaDataID>{9b0fa5cc-356f-4d63-8cd2-875a88554840}</MetaDataID>
        public void Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        /// <MetaDataID>{e6f3951e-794d-4780-b90f-43a32564f73c}</MetaDataID>
        bool System.Collections.IList.IsFixedSize
        {
            get 
            {
                return false;
            }
        }

        /// <MetaDataID>{e560a753-d345-4c01-baa1-3c0c58aca214}</MetaDataID>
        void System.Collections.IList.Remove(object value)
        {
            Remove((T)value);
            
        }

        /// <MetaDataID>{01c497f9-ffd4-453c-97c1-ba0e65ee1ab3}</MetaDataID>
        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                RemoveAt(index);
                Insert(index, (T)value);
            }
        }

        #endregion



  
       
    }
}
