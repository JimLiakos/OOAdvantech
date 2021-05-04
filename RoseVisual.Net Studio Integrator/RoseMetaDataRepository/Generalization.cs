namespace RoseMetaDataRepository
{
    /// <MetaDataID>{A5ABCA76-63DE-4C05-9707-B3F02726F058}</MetaDataID>
    internal class Generalization : OOAdvantech.MetaDataRepository.Generalization
    {
        internal Generalization()
        {
        }
        public Generalization(string name, OOAdvantech.MetaDataRepository.Classifier parentClassifier, OOAdvantech.MetaDataRepository.Classifier childClassifier)
            : base(name, parentClassifier, childClassifier)
        {
        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                OOAdvantech.MetaDataRepository.Generalization OriginGeneralization = OriginMetaObject as OOAdvantech.MetaDataRepository.Generalization;
                if (Parent == null)
                    Parent = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginGeneralization.Parent, this) as OOAdvantech.MetaDataRepository.Classifier;

                if (Child == null)
                    Child = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity( OriginGeneralization.Child).ToString(), this) as OOAdvantech.MetaDataRepository.Classifier;
                if (Parent != null)
                {
                    RationalRose.RoseClass roseChildClass = null;
                    RationalRose.RoseClass roseParentClass = null;
                    if (OriginGeneralization.Parent.ImplementationUnit == OriginGeneralization.Child.ImplementationUnit &&
                        OriginGeneralization.Parent.ImplementationUnit != null)
                        Parent.ShallowSynchronize(OriginGeneralization.Parent);

                    if (Child is Class)
                        roseChildClass = (Child as Class).RoseClass;
                    if (Child is Interface)
                        roseChildClass = (Child as Interface).RoseClass;

                    if (Parent is Class)
                        roseParentClass = (Parent as Class).RoseClass;
                    if (Parent is Interface)
                        roseParentClass = (Parent as Interface).RoseClass;
                    if (roseParentClass != null)
                    {
                        bool exist = false;
                        for (int i = 0; i < roseChildClass.GetSuperclasses().Count; i++)
                        {
                            RationalRose.RoseClass superClass = roseChildClass.GetSuperclasses().GetAt((short)(i + 1));
                            if (roseParentClass.Equals(superClass))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            RationalRose.RoseCategory category = roseParentClass.ParentCategory;
                            string fullName = "";
                            while (!category.Equals((roseParentClass.Application as RationalRose.RoseApplication).CurrentModel.RootCategory))
                            {
                                fullName = category.Name + "::" + fullName;
                                category = category.ParentCategory;
                            }
                            fullName = category.Name + "::" + fullName + roseParentClass.Name;
                            roseChildClass.AddInheritRel(Name, fullName);
                        }
                    }


                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

    }
}
