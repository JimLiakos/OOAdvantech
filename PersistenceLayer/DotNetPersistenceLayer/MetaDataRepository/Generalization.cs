namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{B4945084-65EC-4A00-AF7D-54836A055F4A}</MetaDataID>
	/// <summary>A generalization is a taxonomic relationship between a more general element and a more specific element. The more specific element is fully consistent with the more general element (it has all of its properties, members, and relationships) and may contain additional information.
	/// In the metamodel, a Generalization is a directed inheritance relationship, uniting a GeneralizableElement with a more general GeneralizableElement in a hierarchy.
	/// Generalization is a subtyping relationship (i.e., an Instance of the more general GeneralizableElement may be substituted by an Instance of the more specific GeneralizableElement). See Inheritance for the consequences of Generalization relationships. </summary>
	[BackwardCompatibilityID("{B4945084-65EC-4A00-AF7D-54836A055F4A}")]
	[Persistent()]
	public class Generalization : Relationship
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Parent))
            {
                if (value == null)
                    _Parent = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Parent = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Child))
            {
                if (value == null)
                    _Child = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Child = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Parent))
                return _Parent;

            if (member.Name == nameof(_Child))
                return _Child;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{C22DC4C3-826C-40FC-82CC-FC0183156410}</MetaDataID>
        public override MetaObjectID  Identity
		{
			get
			{
				lock (identityLock)
				{
					if (MetaObjectIDStream!=null)
						if(MetaObjectIDStream.Length>0)
							_Identity=new MetaObjectID(MetaObjectIDStream);
					if(_Identity==null)
						_Identity=new MetaObjectID(Child.Identity.ToString()+"."+Parent.Identity.ToString());
					return _Identity;
				}
				
			}
		}
		/// <MetaDataID>{82E74063-F132-452B-8A3F-078D865864C1}</MetaDataID>
		 protected Generalization()
		{
		}

		/// <MetaDataID>{DA86E4FF-BF03-48A1-A715-D37CEC58357A}</MetaDataID>
		 public Generalization(string name, Classifier parentClassifier, Classifier childClassifier)
		{
			_Parent=parentClassifier;
			_Child=childClassifier;
			 _Parent.AddSpecialization(this);
		}

		/// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
		/// <MetaDataID>{17EAE576-EE06-4462-B7AC-4002D026729C}</MetaDataID>
		public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				
				Generalization OriginGeneralization=(Generalization)OriginMetaObject;

				if(Parent==null)
				{
					Parent=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginGeneralization.Parent,this) as MetaDataRepository.Classifier;
                    if (Parent == null )//&& OriginGeneralization.Child.ImplementationUnit == OriginGeneralization.Parent.ImplementationUnit && OriginGeneralization.Parent.ImplementationUnit!=null)
					{
						Parent=(Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginGeneralization.Parent,this);
                        if (Parent != null)
                        {
                            Parent.AddSpecialization(this);
                            Parent.ShallowSynchronize(OriginGeneralization.Parent);

                        }
					}
					
				}

                if (Child == null)
                    Child = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginGeneralization.Child.Identity.ToString(), this) as MetaDataRepository.Classifier;

                if (Parent != null)
				    base.Synchronize(OriginMetaObject);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

		/// <MetaDataID>{B9B4D4D8-D51A-40A8-B714-89AB74F18157}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}
		/// <MetaDataID>{71E4839E-984B-4855-9CBA-6C1901B9407E}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Classifier _Parent;
		/// <summary>Designates a GeneralizableElement that is the generalized version of the child GeneralizableElement. </summary>
		/// <MetaDataID>{E647F967-E8F6-4318-A1C1-454D5CB19569}</MetaDataID>
		[Association("Specialization",typeof(OOAdvantech.MetaDataRepository.Classifier),Roles.RoleA,"{E591FA9E-B3D8-4699-B6FC-3BD5BB2EA4AC}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("_Parent")]
		[RoleAMultiplicityRange(1,1)]
		public virtual Classifier Parent
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Parent;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
			set
			{
				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					
					_Parent =value;				
					if(_Parent!=null)
						_Parent.AddSpecialization(this);
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
		/// <MetaDataID>{2D1E115C-26B0-435E-997C-A6535872C7FA}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Classifier _Child;
		
		/// <summary>Designates a GeneralizableElement that is the specialized version of the parent GeneralizableElement. </summary>
		/// <MetaDataID>{74C4DD35-01C2-4CB0-AD2A-AF86ACA3812C}</MetaDataID>
		[Association("Generalization",typeof(OOAdvantech.MetaDataRepository.Classifier),Roles.RoleA,"{31EE00E4-640A-493C-8662-08326CE3D600}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("_Child")]
		[RoleAMultiplicityRange(1,1)]
		public virtual Classifier Child
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Child;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}

			}
			set
			{
				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					
					_Child =value;				
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
				
			}		
		}
	}
}
