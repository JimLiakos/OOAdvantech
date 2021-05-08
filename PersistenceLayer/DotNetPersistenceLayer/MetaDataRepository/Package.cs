namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{BA6D6CDC-81FF-406B-9AB7-7C13CE176E20}</MetaDataID>
	/// <summary>A package is a grouping of model elements. Packages themselves may be nested
	/// within other packages. A package may contain subordinate packages as well as other
	/// kinds of model elements. All kinds MetaObject can be organized into
	/// packages. </summary>
	[BackwardCompatibilityID("{BA6D6CDC-81FF-406B-9AB7-7C13CE176E20}")]
	[Persistent()]
	public class Package : Namespace
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{F3C869C1-B60D-4CC8-84CC-DE23718E4413}</MetaDataID>
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        public override void Synchronize(MetaObject originMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				
				if(MetaDataRepository.SynchronizerSession.IsSynchronized(this))
					return;
				base.Synchronize(originMetaObject);
				MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);

                MetaDataRepository.Namespace originPackage = (MetaDataRepository.Namespace)originMetaObject;
				ContainedItemsSynchronizer OwnedElementsSynchronizer=MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originPackage.OwnedElements,_OwnedElements,this);
				OwnedElementsSynchronizer.FindModifications();
				OwnedElementsSynchronizer.ExecuteAddCommand();
				OwnedElementsSynchronizer.ExecuteDeleteCommand();
				OwnedElementsSynchronizer.Synchronize();
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		
		}
	}
}
