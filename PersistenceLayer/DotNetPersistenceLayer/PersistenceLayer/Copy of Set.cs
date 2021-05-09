namespace OOAdvantech.Collections
{
    public enum CollectionAccessType { ReadOnly, ReadWrite }
}
namespace OOAdvantech.Collections.Generic
{
    using System;



	//using T=System.Object;
	//public class T{}
    /// <MetaDataID>{DDC137B9-F095-44C0-863A-C00B9631623C}</MetaDataID>
    /// // ,OOAdvantech.Transactions.TransactionalObject
    [Serializable]
    public class Set<T> : PersistenceLayer.ObjectContainer, OOAdvantech.Transactions.ITransactionalObject, System.Collections.Generic.ICollection<T>, System.Collections.ICollection, System.Collections.IList, System.Collections.Generic.IList<T>, System.Runtime.Serialization.ISerializable, ICollectionMember, IMemberInitialization
	{
        protected override System.Collections.IEnumerator GetObjectEnumerator()
        {
            return (this as System.Collections.IEnumerable).GetEnumerator();
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                array[arrayIndex++] = item;
            }
        }
        public void Clear()
        {
            RemoveAll();
        }
        public bool IsReadOnly
        {
            get
            {
                return AccessType == CollectionAccessType.ReadOnly;
            }
        }
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
			AddRange(objectSet);
		}
        
        CollectionAccessType AccessType = CollectionAccessType.ReadWrite;

        public Set(Set<T> objectSet, CollectionAccessType accessType)
        {
            AccessType=accessType;
            if (accessType == CollectionAccessType.ReadWrite)
                AddRange(objectSet);
            else
                _theObjects = objectSet.theObjects;
            
        }
        public int IndexOf(T item)
        {
            if (item == null)
                throw new System.Exception("System can't add null object in Set");

            return theObjects.IndexOf(item);
        }
        public void Insert(int index, T item)
        {
            if (item == null)
                throw new System.Exception("System can't add null object in Set");
            if (AccessType == CollectionAccessType.ReadOnly)
                throw new System.Exception("The collection is read only");
            if (theObjects != null)
                theObjects.Insert(index,item);
        }
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
        public void RemoveAt(int index)
        {
            theObjects.RemoveAt(index);

        }
        public void RemoveRange(int index, int count)
        {

        }
        public void Sort()
        {

        }

        public void Sort(Comparison<T> comparison)
        {

        }

        public void Sort(System.Collections.Generic.IComparer<T> comparer)
        {

        }

        public void Sort(int index, int count,System.Collections.Generic.IComparer<T> comparer)
        {

        }
		
		#region TransactionalObject Members
        /// <MetaDataID>{51DA4A13-40B9-4F6E-A63C-B77591A3705E}</MetaDataID>
		public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
		{
			if(theObjects!=null)
				theObjects.MarkChanges(transaction);
		}

        public void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            if (theObjects != null)
                theObjects.MergeChanges(mergeInTransaction,mergedTransaction);
        }
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            if (theObjects != null)
                theObjects.MarkChanges(transaction,fields);
        }



        /// <MetaDataID>{193741C2-A0B5-4B3D-B031-8668DDC44B9E}</MetaDataID>
		public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
		{
			if(theObjects!=null)
				theObjects.UndoChanges(transaction);
		}

        /// <MetaDataID>{31F4A507-FF52-4F8F-8F4C-CA5C6C25FD28}</MetaDataID>
		public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
		{
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
        /// <MetaDataID>{5D15257F-062B-479E-B050-8DB6AF20ACE2}</MetaDataID>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
  		{
            
			if(theObjects!=null)
				return new Enumerator<T>(theObjects.GetEnumerator());
			else
			{
				System.Collections.ArrayList tmp=new System.Collections.ArrayList();
				return new Enumerator<T>(tmp.GetEnumerator());
			}
		}

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

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if(AccessType==CollectionAccessType.ReadOnly)
                _theObjects=new Set<T>(this)._theObjects;
            info.AddValue("_theObjects", this._theObjects, typeof(OOAdvantech.PersistenceLayer.ObjectCollection));

        }
        private Set(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctxt)
        {
            this._theObjects = info.GetValue("_theObjects", typeof(OOAdvantech.PersistenceLayer.ObjectCollection)) as OOAdvantech.PersistenceLayer.ObjectCollection;
        }


    





   
        #region IEnumerable<T> Members

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
            
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            foreach (T item in this)
            {
                array.SetValue(item,index++);
            }
        }

        int System.Collections.ICollection.Count
        {
            get { return Count; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion






        #region ICollectionMember Members

        void ICollectionMember.AddImplicitly(object _object)
        {
            if (_object == null)
                return;
            if (_object != null && !(_object is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            (theObjects as ICollectionMember).AddImplicitly(_object);
            
        }

        void ICollectionMember.RemoveImplicitly(object _object)
        {
            
            if (_object == null)
                return;

            if (_object != null && !(_object is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            (theObjects as ICollectionMember).RemoveImplicitly(_object);
        }

        System.Collections.IEnumerator ICollectionMember.GetEnumeretor()
        {
            return GetEnumerator();
        }

        #endregion

        #region IMemberInitialization Members

        void IMemberInitialization.SetOwner(object owner)
        {
            (theObjects as IMemberInitialization).SetOwner(owner);
            //throw new NotImplementedException();
        }

        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata)
        {
            (theObjects as IMemberInitialization).SetMetadata(metadata);
            //throw new NotImplementedException();
        }

        bool IMemberInitialization.Initialized
        {
            get 
            {
                return (theObjects as IMemberInitialization).Initialized;
            }
        }

        #endregion

        #region IList Members

        int System.Collections.IList.Add(object value)
        {
            //TODO θα πρέπει να επιστρέφει από την add
            Add((T)value);
            return Count-1;
        }

        bool System.Collections.IList.Contains(object value)
        {
            return Contains((T)value);
        }

        int System.Collections.IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        bool System.Collections.IList.IsFixedSize
        {
            get 
            {
                return false;
            }
        }

        void System.Collections.IList.Remove(object value)
        {
            Remove((T)value);
            
        }

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
