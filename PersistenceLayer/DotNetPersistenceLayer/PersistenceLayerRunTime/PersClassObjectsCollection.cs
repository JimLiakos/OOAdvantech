namespace OOAdvantech.PersistenceLayerRunTime
{
	/// <MetaDataID>{BD31F4C8-F06C-46CF-8000-D8724CE86C42}</MetaDataID>
	/// <summary>It is a collection of objects it is one per storage and contains sub collections with objects, 
	/// one per class. You can access a class collection if you give the System.Type object. </summary>
	public class MemoryInstanceCollection
	{
		
		/// <MetaDataID>{4FC8A14C-9D6A-4BB4-AF18-C999B334CA99}</MetaDataID>
		 public MemoryInstanceCollection(ObjectStorage storageSession)
		{
			OwnerStorageSession=storageSession;
		}
         
		/// <summary></summary>
		/// <MetaDataID>{96A92DEC-2D34-4B18-8D56-8C43DDFF9CA4}</MetaDataID>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return new ClassMemoryInstanceCollectionEnumerator(ClassMemoryInstanceCollections.GetEnumerator());
		 
		}
		/// <MetaDataID>{8E559980-9B3A-41CC-9456-17995556C9D4}</MetaDataID>
		protected ObjectStorage OwnerStorageSession;
		/// <summary></summary>
		/// <MetaDataID>{6B23DF85-2452-4038-B69C-52E4759D16AA}</MetaDataID>
		private System.Collections.Generic.Dictionary<System.Type,ClassMemoryInstanceCollection> ClassMemoryInstanceCollections =new System.Collections.Generic.Dictionary<System.Type,ClassMemoryInstanceCollection>();
		
		/// <MetaDataID>{641AF101-754C-4CAE-8BFE-DB1A5B9C6EB8}</MetaDataID>
		/// <summary></summary>
		public class ClassMemoryInstanceCollectionEnumerator:System.Collections.IEnumerator
		{
			/// <summary></summary>
			/// <MetaDataID>{0E522D40-5036-4D3D-B789-49946157C6BD}</MetaDataID>
			private System.Collections.IDictionaryEnumerator RealEnumerator;
		
			/// <summary></summary>
			/// <MetaDataID>{71D31F48-9EBD-4B49-802F-52358EA3AD3D}</MetaDataID>
			public bool MoveNext()
			{
				return RealEnumerator.MoveNext();
			}
			/// <summary></summary>
			/// <MetaDataID>{ADCD02BF-EEE5-498E-A1CF-EE4BABC48E1D}</MetaDataID>
			/// <param name="theRealEnumerator"></param>
			 public ClassMemoryInstanceCollectionEnumerator(System.Collections.IDictionaryEnumerator theRealEnumerator)
			{
				RealEnumerator=theRealEnumerator;
			
			}
			#region IEnumerator Members

			/// <MetaDataID>{CCA43060-474A-4F05-9B19-7337E9200F09}</MetaDataID>
			public void Reset()
			{
				RealEnumerator.Reset();
			}
			/// <summary></summary>
			/// <MetaDataID>{71BD3AAF-8AC1-4875-9FA4-032B332B2B47}</MetaDataID>
			object System.Collections.IEnumerator.Current
			{
				get
				{
					return RealEnumerator.Current;
				}
			}

			#endregion
		}
		/// <MetaDataID>{39289809-F303-44CE-9AA3-16FD2954F87B}</MetaDataID>
		protected virtual ClassMemoryInstanceCollection CreateClassMemoryInstanceCollection(System.Type _Type)
		{
			return new ClassMemoryInstanceCollection(_Type,OwnerStorageSession);
			

		}
		/// <MetaDataID>{BFBC805D-4457-40B7-AB97-62C32D965456}</MetaDataID>
		/// <summary></summary>
		public ClassMemoryInstanceCollection this[System.Type Index ]   // long is a 64-bit integer
		{
			get
			{
                lock (OwnerStorageSession)
                {
                    ClassMemoryInstanceCollection classMemoryInstanceCollection = null;
                    if (!ClassMemoryInstanceCollections.TryGetValue(Index, out classMemoryInstanceCollection))
                    {
                        classMemoryInstanceCollection = CreateClassMemoryInstanceCollection(Index);
                        ClassMemoryInstanceCollections[Index] = classMemoryInstanceCollection;
                    }
                    return classMemoryInstanceCollection;
                }
			}
		}
	}
}
