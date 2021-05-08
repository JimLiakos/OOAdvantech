namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{1BE03660-E41F-4512-A7C8-935810AD28E2}</MetaDataID>
	/// <summary>A namespace is a part of a model that contains a set of Meta objects each of whose names designates a unique element within the namespace.
	/// In the metamodel, a Namespace is a Meta object that can own other Meta object, like Associations and Classifiers. The name of each owned Meta object must be unique within the Namespace. Moreover, each contained
	/// Meta object is owned by at most one Namespace. The concrete subclasses of Namespace have additional constraints on which kind of elements may be contained.
	/// Namespace is an abstract metaclass.
	/// Note that explicit parts of a model element, such as the features of a Classifier, are not modeled as owned elements in a namespace. A namespace is used for unstructured
	/// contents such as the contents of a package or a class declared inside the scope of another class. </summary>
	[BackwardCompatibilityID("{1BE03660-E41F-4512-A7C8-935810AD28E2}")]
	[Persistent()]
	public class Namespace : MetaObject
	{
		/// <MetaDataID>{6c90791b-4006-49c9-954e-ea2453254b66}</MetaDataID>
		public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
		{
			if (member.Name == nameof(_OwnedElements))
			{
				if (value == null)
					_OwnedElements = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>);
				else
					_OwnedElements = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>)value;
				return ObjectMemberGetSet.MemberValueSetted;
			}

			return base.SetMemberValue(token, member, value);
		}

		/// <MetaDataID>{239eb4db-bf37-4cde-9226-345b38064924}</MetaDataID>
		public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
		{

			if (member.Name == nameof(_OwnedElements))
				return _OwnedElements.ToThreadSafeSet();


			return base.GetMemberValue(token, member);
		}

		///<summary>
		///This method ensures that all members have case insensitive unique name.
		///</summary>
		///<remarks>
		///Some of object oriented languages are case insensitive. 
		///Case insensitive means that “name” and “Name” are the same thing. 
		///The BuildCaseInsensitiveNames in that cases change the second from “Name” to “Name_1” 
		///or other case insensitive unique name in namespace.
		///</remarks>
		/// <MetaDataID>{8BF2D570-04F4-4319-80BE-3794E0232C7D}</MetaDataID>
		public virtual void BuildCaseInsensitiveNames()
		{
		}
		/// <MetaDataID>{17773F6E-E42D-4674-9BD9-4B0D37705ADB}</MetaDataID>
		public virtual void MakeNameUnaryInNamesapce(MetaDataRepository.MetaObject metaObject)
		{

			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				bool ensureUnaryName=true;
				string unaryName=metaObject.Name;
				int count=1;
				while(ensureUnaryName)
				{
					ensureUnaryName=false;
					foreach(MetaDataRepository.MetaObject ownedElement in _OwnedElements)
					{
						if(ownedElement==metaObject)
							continue;
						if(ownedElement.Name.Trim()==metaObject.Name.Trim())
						{
							metaObject.Name=unaryName+"_"+count.ToString();
							ensureUnaryName=true;
							count++;
							break;
						}
					}
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
    
		

		/// <MetaDataID>{C0B90E55-3F7C-4843-873F-2854E7726D44}</MetaDataID>
		/// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
		public override void Synchronize(MetaObject originMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				base.Synchronize(originMetaObject);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		
		}
		/// <MetaDataID>{004B16B1-630B-487D-AC27-E836A073ABD9}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected OOAdvantech.Collections.Generic.Set<MetaObject> _OwnedElements=new OOAdvantech.Collections.Generic.Set<MetaObject>();
		/// <summary>A set of MetaObjects owned by the Namespace. Its visibility attribute states whether the element is visible outside the namespace. </summary>
		/// <MetaDataID>{1C9AC2B0-EE6C-43F4-8D61-72AA5FF15DBD}</MetaDataID>
		[Association("NamespaceMember",typeof(OOAdvantech.MetaDataRepository.MetaObject),Roles.RoleA,"{A70335FA-743A-40DF-B6CB-FF0F4481484C}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.LazyFetching|PersistencyFlag.ReferentialIntegrity|PersistencyFlag.CascadeDelete)]
		[PersistentMember("_OwnedElements")]
		[RoleAMultiplicityRange(0)]
		public virtual OOAdvantech.Collections.Generic.Set<MetaObject>  OwnedElements
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _OwnedElements.ToThreadSafeSet();
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{EA267B43-774E-47D2-ABA5-A27552D2102A}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}
		/// <MetaDataID>{73AF648F-69AA-491F-99D4-B6E33932453A}</MetaDataID>
		public virtual void AddOwnedElement(MetaObject ownedElement)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(FullName== "System.Collections.Generic")
                {

                }
				
				if (ownedElement.Namespace!=null&&ownedElement.Namespace!=this)
					throw new System.Exception("the meta object '"+ownedElement.FullName+"' has already namespace");

                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{
					_OwnedElements.Add(ownedElement);
					if(ownedElement.Namespace!=this)
						ownedElement.SetNamespace(this);
					stateTransition.Consistent=true;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}


		/// <MetaDataID>{81A902D1-E0C9-4136-961D-B0ADDC967FBC}</MetaDataID>
		public virtual void RemoveOwnedElement(MetaObject ownedElement)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(ownedElement==null)
					return;

				if(!_OwnedElements.Contains(ownedElement))
					return;
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{
					_OwnedElements.Remove(ownedElement);
					if(ownedElement.Namespace==this)
						ownedElement.SetNamespace(null);
					stateTransition.Consistent=true;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}



		/// <MetaDataID>{05CAAB7F-BD59-4D00-ADA6-6EC9BB85164B}</MetaDataID>
		public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
		{
			bool hasError=base.ErrorCheck (ref errors);
            System.Collections.Generic.Dictionary<string, MetaObject> Names = new System.Collections.Generic.Dictionary<string, MetaObject>();
			foreach(MetaObject ownedElement in OwnedElements)
			{
				//TODO τα features λόγο του μηχανισμού overload θέλουν ιδεαίτερη μεταχίρηση
				if(ownedElement is Feature)
					continue;
				if(!Names.ContainsKey(ownedElement.Name))
					Names.Add(ownedElement.Name,ownedElement);
				else
				{
					hasError=true;
					errors.Add(new MetaDataError("MDR Error: There are two meta object with same name '"+ownedElement.FullName+"' in namespace '"+FullName+"'. ",ownedElement.FullName));
				}
			}
			return hasError;
		}
			
		/// <MetaDataID>{F21D713E-A0DB-4E75-870D-926F5F75F47F}</MetaDataID>
		 public Namespace()
		{
		
		}
	}
}
