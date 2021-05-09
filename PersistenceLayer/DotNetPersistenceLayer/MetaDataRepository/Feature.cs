namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{E2632C55-8224-4D48-BFCD-40B5C83DC347}</MetaDataID>
	/// <summary>A feature is a property, like operation or attribute, which is encapsulated within a
	/// Classifier.
	/// In the metamodel, a Feature declares a behavioral or structural characteristic of an
	/// Instance of a Classifier or of the Classifier itself. Feature is an abstract metaclass. </summary>
	[BackwardCompatibilityID("{E2632C55-8224-4D48-BFCD-40B5C83DC347}")]
	[Persistent()]
    public abstract class Feature : MetaObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <MetaDataID>{4be650c5-fccd-44d5-b9af-904d73e4e6d5}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(Test))
            {
                if (value == null)
                    Test = default(int);
                else
                    Test = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IsStatic))
            {
                if (value == null)
                    _IsStatic = default(bool);
                else
                    _IsStatic = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Owner))
            {
                if (value == null)
                    _Owner = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Owner = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{b1f88493-35c2-47db-8025-15e9218a4b85}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(Test))
                return Test;

            if (member.Name == nameof(_IsStatic))
                return _IsStatic;

            if (member.Name == nameof(_Owner))
                return _Owner;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{760e4023-54c4-44f3-b31f-b235ecabd805}</MetaDataID>
        private int Test;

        /// <exclude>Excluded</exclude>
        protected bool _IsStatic=false;
        /// <MetaDataID>{acb32068-4ef4-4806-8bdb-3458988663cb}</MetaDataID>
        public virtual bool IsStatic
        {
            get
            {
                return _IsStatic;
            }
        }

		/// <MetaDataID>{55A144D2-3448-467E-8C43-C20A5975A679}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Classifier _Owner;
	
		/// <summary>The Classifier declaring the Feature. Note that an Attribute may be owned by a Classifier (in which case it is a feature) or an AssociationEnd (in which case it is a qualifier) but not both. </summary>
		/// <MetaDataID>{94F5C312-0AAB-4C0B-81F5-2BEF6A7CC847}</MetaDataID>
		[Association("ClassifierMember",typeof(OOAdvantech.MetaDataRepository.Classifier),Roles.RoleB,"{38679851-A962-46C4-A940-20522E07D301}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("_Owner")]
		[RoleBMultiplicityRange(1,1)]
		public virtual Classifier Owner
		{
			get
			{
 				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Owner;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
        /// <MetaDataID>{1e95a6d4-d7f8-4e14-b444-d0724b84d931}</MetaDataID>
        public override Component ImplementationUnit
        {
            get
            {
                return Owner.ImplementationUnit;
            }
        }
		/// <MetaDataID>{64A542A3-C2BB-4B38-99AE-A8B26C544484}</MetaDataID>
        internal void SetOwner(Classifier value)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{
					_Owner=value;
					stateTransition.Consistent=true;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

        /// <MetaDataID>{78b42366-1c4c-45af-8f76-5d1b7fce5767}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Namespace Namespace
        {
            get
            {

                return _Owner;
            }
        }
	}
}
