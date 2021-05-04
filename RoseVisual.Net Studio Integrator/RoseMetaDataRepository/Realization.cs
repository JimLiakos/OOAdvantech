namespace RoseMetaDataRepository
{
    /// <MetaDataID>{24ACF244-9531-4FDC-86A9-46BE432A2AF9}</MetaDataID>
    internal class Realization : OOAdvantech.MetaDataRepository.Realization
    {
        RationalRose.RoseRealizeRelation RoseRealizeRelation;
        public Realization(RationalRose.RoseRealizeRelation roseRealizeRelation, Interface _interface, Class _class):base(roseRealizeRelation.Name,_interface,_class)
        {
            RoseRealizeRelation = roseRealizeRelation;
        }
        public Realization(string name, Interface _interface, Class _class):base(name,_interface,_class)
        {

        }
        public Realization(string name, Interface _interface, Structure _struct)
            : base(name, _interface, _struct)
        {

        }


        internal Realization()
        {

        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                OOAdvantech.MetaDataRepository.Realization originRealization = OriginMetaObject as OOAdvantech.MetaDataRepository.Realization;
                if (Abstarction == null)
                    Abstarction = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originRealization.Abstarction, this) as OOAdvantech.MetaDataRepository.Interface;

                if (Implementor == null)
                    Implementor = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(originRealization.Implementor as OOAdvantech.MetaDataRepository.MetaObject).ToString(), this) as OOAdvantech.MetaDataRepository.Class;
                if (Abstarction != null)
                {
                    RationalRose.RoseClass roseImplementorClass = (Implementor as Class).RoseClass;
                    
                    if (originRealization.Abstarction.ImplementationUnit == (originRealization.Implementor as OOAdvantech.MetaDataRepository.MetaObject) .ImplementationUnit &&
                        originRealization.Abstarction.ImplementationUnit != null)
                        Abstarction.ShallowSynchronize(originRealization.Abstarction);
                    RationalRose.RoseClass roseAbstarctionClass = (Abstarction as Interface).RoseClass;

                    if (roseAbstarctionClass != null)
                    {
                        bool exist = false;
                        for (int i = 0; i < roseImplementorClass.GetRealizeRelations().Count; i++)
                        {
                            RationalRose.RoseRealizeRelation roseRealization=roseImplementorClass.GetRealizeRelations().GetAt((short)(i + 1));
                            RationalRose.RoseClass superClass = roseRealization.GetSupplierClass();
                            if (roseAbstarctionClass.Equals(superClass))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            RationalRose.RoseCategory category = roseAbstarctionClass.ParentCategory;
                            string fullName = "";
                            while (!category.Equals((roseAbstarctionClass.Application as RationalRose.RoseApplication).CurrentModel.RootCategory))
                            {
                                fullName = category.Name + "::" + fullName;
                                category = category.ParentCategory;
                            }
                            fullName = category.Name + "::" + fullName + roseAbstarctionClass.Name;
                            roseImplementorClass.AddRealizeRel(Name, fullName);
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
