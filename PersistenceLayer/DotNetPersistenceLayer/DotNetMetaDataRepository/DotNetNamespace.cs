namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{E8BD88C8-01FA-4B77-B767-BDB9DBF52170}</MetaDataID>
	public class Namespace : MetaDataRepository.Namespace
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{19c3f6f5-1e52-4da2-8302-ca8ea6da006b}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{8643C954-5354-45C9-BF3B-F818BE72A7D6}</MetaDataID>
		protected Namespace()
		{
		}
		/// <MetaDataID>{24FA561E-47B7-437F-A11F-1162F7D36197}</MetaDataID>
		public override Collections.Generic.Set<MetaDataRepository.MetaObject> OwnedElements
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
                    Collections.Generic.Set <MetaDataRepository.MetaObject> metaObjects = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>();
                    metaObjects.AddRange(_OwnedElements);
                    return metaObjects;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <summary>Produce the identity of class from the .net metada </summary>
		/// <MetaDataID>{D8D9CF45-0E4D-4660-9891-E118DE1DA01D}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
		{
			get
			{
                return _Identity;
		
			}
		}


		/// <MetaDataID>{000126DF-CD40-4822-96F8-45CC8F3EA08D}</MetaDataID>
		public override void AddOwnedElement(MetaDataRepository.MetaObject ownedElement)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(ownedElement.Namespace!=null&&ownedElement.Namespace!=this)
					throw new System.Exception("the meta object '"+ownedElement.FullName+"' has already namespace");
				_OwnedElements.Add(ownedElement);
				
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		/// <MetaDataID>{E4F913F0-0B8E-4964-B53D-1C459A03CBAF}</MetaDataID>
        public Namespace(string NamespaceName)
        {
            int nPos = NamespaceName.LastIndexOf('.');
            if (nPos != -1)
            {
                _Name = NamespaceName.Substring(nPos + 1);
                string NewNameSpaceName = NamespaceName.Substring(0, nPos);

				Namespace mNamespace = Type.GetNameSpace(NewNameSpaceName);

				//Namespace NewNameSpace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(NewNameSpaceName);
    //            if (NewNameSpace == null)
    //                NewNameSpace = new Namespace(NewNameSpaceName);
                _Namespace.Value = mNamespace;
				mNamespace.AddOwnedElement(this);
            }
            else
                _Name = NamespaceName;
            _Identity = new MetaDataRepository.MetaObjectID(FullName);
            MetaObjectMapper.AddMetaObject(this, FullName);

            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(NamespaceName, this);

        }
	}
}
