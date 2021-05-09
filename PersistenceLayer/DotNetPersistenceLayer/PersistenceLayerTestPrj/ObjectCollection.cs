namespace PersistenceLayerTestPrj
{
	/// <MetaDataID>{68F43D27-7843-4D79-A81C-15A78644E855}</MetaDataID>
	public class ObjectCollection : OOAdvantech.PersistenceLayer.ObjectContainer
	{
		protected System.Collections.ArrayList ObjectArray;

		/// <MetaDataID>{EDE62BB2-76A4-4F5B-A575-325C24551BAE}</MetaDataID>
		public long Count
		{
			get
			{
				
				if (theObjects!=null)
					return theObjects.Count;// Error Prone;
				else
				{
					if(ObjectArray==null)
						ObjectArray=new System.Collections.ArrayList(20);
					return ObjectArray.Count;
				}

				return 0;
			}
		}
		/// <MetaDataID>{C95FFED5-B603-4A11-8AC0-21447D76CEAE}</MetaDataID>
		public bool Contains(object theMetaObject)
		{
			if (theObjects!=null)
				return theObjects.Contains(theMetaObject);
			else
			{
				return ObjectArray.Contains(theMetaObject);
			}
		}
		/// <MetaDataID>{9625A7C1-B8A3-46CF-A583-D34B9C4DF49C}</MetaDataID>
		public virtual void AddCollection(ObjectCollection theObjectCollection)
		{
			foreach (object CurrObject in theObjectCollection)
			{
				Add(CurrObject);
			}
		
		}
		/// <MetaDataID>{266EAD9C-9902-4013-9D16-871D9DFDD9F4}</MetaDataID>
		public ObjectEnumerator GetEnumerator()
		{
			if (theObjects!=null)
				return new ObjectEnumerator(theObjects.GetEnumerator());
			else
			{
				if(ObjectArray==null)
					ObjectArray=new System.Collections.ArrayList(20);
				return new ObjectEnumerator(ObjectArray.GetEnumerator());
			}
		
		}
		/// <MetaDataID>{8622FF45-9FAB-441F-B94D-A87CD0A6C152}</MetaDataID>
		public virtual void Add(object theObject)
		{
			if(theObjects!=null)
				theObjects.Add(theObject);
			else
			{
				if(ObjectArray==null)
					ObjectArray=new System.Collections.ArrayList(20);
				if(!ObjectArray.Contains(theObject))
					ObjectArray.Add(theObject);

			}
		
		}
		/// <MetaDataID>{9F87DE25-CF09-4869-B952-E7088F17EBB0}</MetaDataID>
		public void Remove(object theObject)
		{
			if(theObjects!=null)
				theObjects.Remove(theObject);
			else
			{
				if(ObjectArray!=null)
					ObjectArray.Remove(theObject);
			}
		}
		public class ObjectEnumerator:System.MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject
		{
			public object Current
			{
				get 
				{
					return ObjectsEnumertor.Current;
				}
			}
			private System.Collections.IEnumerator ObjectsEnumertor;
			internal ObjectEnumerator(System.Collections.IEnumerator aEnumertor)
			{
				ObjectsEnumertor=aEnumertor;
			}

			public bool MoveNext()
			{
				return ObjectsEnumertor.MoveNext();
			}
		}

	}
}
