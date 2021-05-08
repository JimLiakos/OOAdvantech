namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{B363B99A-8B38-4F3F-A4D4-E8660E2B5E14}</MetaDataID>
	/// <summary>A method is the implementation of an operation. It specifies the algorithm or procedure that effects the results of an operation.
	/// In the metamodel, a Method is a declaration of a named piece of behavior in a Class and realizes one (directly) or a set (indirectly) of Operations of the Classifier. </summary>
	[BackwardCompatibilityID("{B363B99A-8B38-4F3F-A4D4-E8660E2B5E14}")]
	[Persistent()]
	public class Method : BehavioralFeature
	{
		//TODO να γραφτεί sychronize method
		public Method()
		{
		}

		public Method(Operation operation)
		{
			_Specification=operation;
		}
	
		/// <MetaDataID>{8A5796BC-012E-463C-AF91-4B823A839E68}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			return new System.Collections.ArrayList(); 
		}

		/// <MetaDataID>{2033F988-B116-4A21-8CE3-718A4D8EA098}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Operation _Specification;
	
		/// <summary>Designates an Operation that the Method implements. The Operation must be owned by the Classifier that owns the Method or be inherited by it. The signatures of the Operation and Method must match. </summary>
		/// <MetaDataID>{3B8D64EF-5B2E-4895-9D59-268446B3EAEE}</MetaDataID>
		public virtual Operation Specification
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Specification;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
	}
}
