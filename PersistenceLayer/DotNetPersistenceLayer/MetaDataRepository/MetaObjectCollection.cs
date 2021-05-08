using OOAdvantech.Transactions;

namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{66659450-A56E-45F9-9EE7-85E6C43F2A9B}</MetaDataID>
	[OOAdvantech.Transactions.ContainByValue]
    public class MetaObjectCollectionA : PersistenceLayer.ObjectContainer, OOAdvantech.Transactions.ITransactionalObject, System.Collections.ICollection, ICollectionMember, IMemberInitialization
	{
        /// <MetaDataID>{3d8ea090-d532-4e35-b921-df1ac83558b3}</MetaDataID>
        protected override System.Collections.IEnumerator GetObjectEnumerator()
        {
            return (this as System.Collections.IEnumerable).GetEnumerator();
        }
		public enum AccessType{ReadOnly,ReadWrite}
		
		//TODO make MetaObjectCollection thread safe

		/// <MetaDataID>{3460B173-EC96-4841-85B7-5BB5F3B403A2}</MetaDataID>
		public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
		{
			if(theObjects!=null)
				theObjects.MarkChanges(transaction);
		}


        /// <MetaDataID>{75010ab2-143a-41d2-82fe-32df78b8dc19}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            if (theObjects != null)
                theObjects.MergeChanges(mergeInTransaction, mergedTransaction);
        }
        public void EnsureSnapshot(Transactions.Transaction transaction)
        {
            if (theObjects != null)
                theObjects.EnsureSnapshot(transaction);
        }

        /// <MetaDataID>{3E9F0670-6EC0-4987-8BAB-3441168EE32E}</MetaDataID>
        public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
		{
			if(theObjects!=null)
				theObjects.UndoChanges(transaction);
		}

		/// <MetaDataID>{020BD32D-C3C3-4414-95E0-C8B31DD30FC0}</MetaDataID>
		public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
		{
			if(theObjects!=null)
				theObjects.CommitChanges(transaction);
		}

		/// <MetaDataID>{434A1053-8014-489D-BC03-ED6992B52B9C}</MetaDataID>
		bool Readonly=false; 
		/// <MetaDataID>{E76A139B-16A2-4CC1-A3D9-1D2F01FACB04}</MetaDataID>
		MetaObjectCollectionA AssignedCollection;
		
		/// <MetaDataID>{8A2ABF2E-880A-42F0-9B41-FA9A9A04B7F0}</MetaDataID>
		public MetaObjectCollectionA(MetaObjectCollectionA assignedCollection,AccessType accessType)
		{
			if(assignedCollection==null)
				throw new System.Exception("The assignedCollection is null");
			AssignedCollection=assignedCollection;
			if(accessType==AccessType.ReadOnly)
				Readonly=true;
		}

		/// <MetaDataID>{FD32B812-680D-4912-808D-A1245A843CBB}</MetaDataID>
		public MetaObjectCollectionA()
		{
			
		}
		/// <MetaDataID>{3060F214-A383-4C86-A48C-62488395C56C}</MetaDataID>
		public MetaObjectCollectionA(MetaObjectCollectionA CopyCollection)
		{
			if(CopyCollection==null)
				throw new System.Exception("The CopyCollection is null");
			AddCollection(CopyCollection);
		}


        /// <MetaDataID>{4f4d04ac-50fd-4ceb-8675-b867ca79be8d}</MetaDataID>
        public MetaObjectCollectionA(System.Collections.ICollection copyCollection)
        {
            
            if (copyCollection == null)
                throw new System.Exception("The CopyCollection is null");
            foreach (MetaObject CurrObject in copyCollection)
                Add(CurrObject);
        }


		/// <MetaDataID>{5EA5015C-C81D-4DB0-8BDB-A20A7CDCBFD0}</MetaDataID>
		public void RemoveAll()
		{
			if(Readonly)
				throw new System.Exception("The collection is read only");
			if(AssignedCollection!=null)
			{
				AssignedCollection.RemoveAll();
				return;
			}
			if(_theObjects!=null)
				_theObjects.RemoveAll();
		}
		/// <MetaDataID>{66C69845-91C9-4C27-89C6-79841226686D}</MetaDataID>
		public bool Contains(MetaObject theMetaObject)
		{
			if(AssignedCollection!=null)
				return AssignedCollection.Contains(theMetaObject);
			
			if (_theObjects!=null)
				return _theObjects.Contains(theMetaObject);
			else
				return false;
		}
		
	
		/// <MetaDataID>{75B474C3-C8F0-4C58-9140-831C77BEEDD9}</MetaDataID>
		public long Count
		{
			get
			{
				
				if(AssignedCollection!=null)
					return AssignedCollection.Count;

				if (_theObjects!=null)
					return _theObjects.Count;// Error Prone;
				return 0;
			}
		}
        /// <MetaDataID>{304befde-cd42-4f1e-9efe-505cfaef8140}</MetaDataID>
        public virtual void AddCollection(System.Collections.ICollection copyCollection)
        {
            if (copyCollection == null)
                return;
            if (Readonly)
                throw new System.Exception("The collection is read only");
            if (copyCollection == null)
                throw new System.Exception("The CopyCollection is null");
            foreach (MetaObject CurrObject in copyCollection)
                Add(CurrObject);

        }

		
		/// <MetaDataID>{AF824E80-2693-4C77-B489-095C72881D59}</MetaDataID>
		public virtual void AddCollection(MetaObjectCollectionA theObjectCollection)
		{

			if(Readonly)
				throw new System.Exception("The collection is read only");
				if(AssignedCollection!=null)
			{
				AssignedCollection.AddCollection(theObjectCollection);
				return;
			}
			if(theObjectCollection.AssignedCollection==null&&theObjectCollection.theObjects!=null)
			{
				theObjects.AddObjects(theObjectCollection.theObjects);
				return;
			}

			foreach (MetaObject CurrObject in theObjectCollection)
				Add(CurrObject);
		}
	
		/// <MetaDataID>{9CC5C764-209B-477E-BC7B-1CBEC2394D86}</MetaDataID>
		public class MetaObjectEnumerator:System.Collections.IEnumerator
		{
			/// <MetaDataID>{E7377261-84A6-4C89-A50C-0AEF3A322EAD}</MetaDataID>
			public MetaDataRepository.MetaObject Current
			{
				get 
				{
					try
					{
						return (MetaDataRepository.MetaObject)ObjectsEnumertor.Current;
					}
					catch(System.Exception Error)
					{
						//System.Windows.Forms.MessageBox.Show(ObjectsEnumertor.Current.GetType().ToString());
						throw ;
					}
				}
			}
			/// <MetaDataID>{706E87E9-D239-44EC-9EE9-F978603F9516}</MetaDataID>
			private System.Collections.IEnumerator ObjectsEnumertor;
			/// <MetaDataID>{35A4A7B4-19EA-466C-BA86-E94BFB68A114}</MetaDataID>
			internal MetaObjectEnumerator(System.Collections.IEnumerator aEnumertor)
			{
				ObjectsEnumertor=aEnumertor;
			}

			/// <MetaDataID>{58A2ACD3-2B56-4B8D-9440-70A0D9AA1B8C}</MetaDataID>
			public bool MoveNext()
			{
				if(ObjectsEnumertor==null)
					return false;
				return ObjectsEnumertor.MoveNext();
			}

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            bool System.Collections.IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            void System.Collections.IEnumerator.Reset()
            {
                
            }

            #endregion
        }

		/// <MetaDataID>{4084B8E3-D245-4E6E-94F9-93E547E977CF}</MetaDataID>
		public void Remove(MetaObject theObject)
		{
			if(Readonly)
				throw new System.Exception("The collection is read only");
			if(AssignedCollection!=null)
			{
				AssignedCollection.Remove(theObject);
				return;
			}

			if(theObjects!=null)
				theObjects.Remove(theObject);
		
		}
		/// <MetaDataID>{E40128C9-2EAF-44B9-82A5-338BB4843A0F}</MetaDataID>
		public virtual void Add(MetaObject theObject)
		{
			if(Readonly)
				throw new System.Exception("The collection is read only");
            if (theObject == null)
                throw new System.ArgumentException("the parameter must be not null", "theObject");
			if(AssignedCollection!=null)
			{
				AssignedCollection.Add(theObject);
				return;
			}
			theObjects.Add(theObject);
		}
		/// <MetaDataID>{D9D4EDA4-461F-4CBF-9B72-3EDDC6C46D79}</MetaDataID>
		public MetaObjectEnumerator GetEnumerator()
		{
			if(AssignedCollection!=null)
				return AssignedCollection.GetEnumerator();

			if (theObjects!=null)
				return new MetaObjectEnumerator(theObjects.GetEnumerator());
			else
				return new MetaObjectEnumerator(null);
		}

        #region ICollection Members

        /// <MetaDataID>{5d737d07-911c-4bd0-aacf-6fc2559ad19d}</MetaDataID>
        void System.Collections.ICollection.CopyTo(System.Array array, int index)
        {
            foreach (MetaObject item in this)
            {
                array.SetValue(item, index++);
            }
        }

        /// <MetaDataID>{31a368ae-5ca1-486c-a6ed-8d0c4c974275}</MetaDataID>
        int System.Collections.ICollection.Count
        {
            get
            {
                return (int) Count;

            }
        }

        /// <MetaDataID>{79ee8f6c-9276-471b-ada5-8c056f497c4f}</MetaDataID>
        bool System.Collections.ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{d83481a3-5311-4d5a-96f7-b1cda91980a0}</MetaDataID>
        object System.Collections.ICollection.SyncRoot
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{60bdbf70-8df6-4d98-b677-85ddd8f13615}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if(AssignedCollection!=null)
				return (AssignedCollection as System.Collections.IEnumerable).GetEnumerator();

			if (theObjects!=null)
				return  theObjects.GetEnumerator();
			else
				return new System.Collections.Generic.List<object>().GetEnumerator();

            }

        #endregion

        #region TransactionalObject Members


        /// <MetaDataID>{cf6115c1-d16d-4be1-835b-a24d335413df}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new System.NotImplementedException();
        }

        #endregion




        #region ICollectionMember Members

        /// <MetaDataID>{8b1fc025-3d49-4c12-9e3e-9c70db46fd56}</MetaDataID>
        void ICollectionMember.AddImplicitly(object _object)
        {
            if (_object == null)
                return;
            if (_object != null && !(_object is MetaObject))
                throw new System.Exception("The value isn't type of " + typeof(MetaObject).FullName);
            (theObjects as ICollectionMember).AddImplicitly(_object);

        }

        /// <MetaDataID>{9f8dbd03-0b1a-4f6c-87da-1747a6687b17}</MetaDataID>
        void ICollectionMember.RemoveImplicitly(object _object)
        {

            if (_object == null)
                return;

            if (_object != null && !(_object is MetaObject))
                throw new System.Exception("The value isn't type of " + typeof(MetaObject).FullName);
            (theObjects as ICollectionMember).RemoveImplicitly(_object);
        }

        /// <MetaDataID>{58fdeddb-5f06-42bd-ab61-40bf903569f3}</MetaDataID>
        System.Collections.IEnumerator ICollectionMember.GetEnumeretor()
        {
            return GetEnumerator();
        }

        #endregion

        #region IMemberInitialization Members

        /// <MetaDataID>{2066d860-24f2-4659-a090-ca3cf67ce2db}</MetaDataID>
        void IMemberInitialization.SetOwner(object owner)
        {
            (theObjects as IMemberInitialization).SetOwner(owner);
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{8d879c41-02ac-4651-b101-0f80484704d5}</MetaDataID>
        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata)
        {
            (theObjects as IMemberInitialization).SetMetadata(metadata);
            //throw new NotImplementedException();
        }
        /// <MetaDataID>{7a1c4d15-af1f-43a8-ad7e-6ea16c928ed7}</MetaDataID>
        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata,object relResolver)
        {
            (theObjects as IMemberInitialization).SetMetadata(metadata);
            //throw new NotImplementedException();
        }

        public bool ObjectHasGhanged(TransactionRunTime transaction)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{2e72671b-6eb6-4bfb-9cfa-4815677dc8b9}</MetaDataID>
        bool IMemberInitialization.Initialized
        {
            get
            {
                return (theObjects as IMemberInitialization).Initialized;
            }
        }

        #endregion
    }
}
