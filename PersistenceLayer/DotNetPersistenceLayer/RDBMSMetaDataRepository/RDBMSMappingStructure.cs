using System.Linq;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    /// <MetaDataID>{30062563-AC77-40C7-883D-093E296B53CC}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{30062563-AC77-40C7-883D-093E296B53CC}"), MetaDataRepository.Persistent()]
    public class Structure : OOAdvantech.MetaDataRepository.Structure, OOAdvantech.RDBMSMetaDataRepository.MappedClassifier
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                var implementationUnit = base.ImplementationUnit;
                if (implementationUnit != null && implementationUnit.Context == null)
                {
                    if (PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                    {
                        OOAdvantech.Linq.Storage theStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                        var storage = (from metastorage in theStorage.GetObjectCollection<Storage>() select metastorage).FirstOrDefault();
                        implementationUnit.Context = storage;

                    }
                }
                return implementationUnit;
            }
        }

        /// <MetaDataID>{cb19c1bd-b66e-4f1f-a092-b4eb89ecd3c6}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                return;

            Classifier OriginClassifier = (Classifier)OriginMetaObject;

            if (OriginClassifier.IsTemplate)
            {
                if (_Name != OriginClassifier.Name)
                {
                    _Name = OriginClassifier.Name;
                    _CaseInsensitiveName = null;
                }


                if (_Namespace == null && PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties) != null)
                    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Namespace", typeof(MetaDataRepository.MetaObject));
                if (_Namespace.Value == null && OriginClassifier.Namespace != null)
                {
                    _Namespace.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginMetaObject.Namespace, this) as MetaDataRepository.Namespace;


                    if (_Namespace.Value == null)
                    {
                        _Namespace.Value = (Namespace)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginMetaObject.Namespace, this);
                        if (_Namespace.Value != null)
                            _Namespace.Value.ShallowSynchronize(OriginClassifier.Namespace);
                    }
                    if (_Namespace.Value != null)
                        _Namespace.Value.AddOwnedElement(this);
                }



                if (_ImplementationUnit.Value == null && OriginClassifier.ImplementationUnit != null)
                {
                    _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginMetaObject.ImplementationUnit, this) as MetaDataRepository.Component;
                    if (_ImplementationUnit.Value == null)
                        _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginMetaObject.ImplementationUnit, this);
                }
                return;
            }
            base.Synchronize(OriginMetaObject);
            _Persistent = (OriginMetaObject as MetaDataRepository.Structure).Persistent;
        }

        public override void ShallowSynchronize(OOAdvantech.MetaDataRepository.MetaObject originClassifier)
        {
            base.ShallowSynchronize(originClassifier);
            _Persistent = (originClassifier as MetaDataRepository.Structure).Persistent;
            if (_Persistent)
                Synchronize(originClassifier);

        }
        #region MappedClassifier Members

        /// <MetaDataID>{e69e1a23-79f4-422c-815f-66513f4709ca}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> GetObjectIdentityTypes(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells)
        {
            System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
            foreach (StorageCell storageCell in (this as MappedClassifier).ClassifierLocalStorageCells)
            {
                MetaDataRepository.ObjectIdentityType objectIdentityType = storageCell.ObjectIdentityType;
                if (!objectIdentityTypes.Contains(objectIdentityType))
                    objectIdentityTypes.Add(objectIdentityType);
            }
            return objectIdentityTypes;
        }

        /// <MetaDataID>{d947d61f-8c5e-4690-bffa-1b8c98735807}</MetaDataID>
        public View TypeView
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        /// <MetaDataID>{1dc9e3dd-d900-44e7-8126-6036ba387593}</MetaDataID>
        public bool HasPersistentObjects
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        /// <MetaDataID>{a2923117-3dcc-433e-b22f-4d6604da7fd6}</MetaDataID>
        public View GetTypeView(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{85fcdafc-d5ba-45b7-818b-f5e26047dd25}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(System.DateTime TimePeriodStartDate, System.DateTime TimePeriodEndDate)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{bbb991a0-7793-47a0-b273-9e9971c74890}</MetaDataID>
        public StorageCell GetStorageCell(object ObjectID)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

 
        /// <MetaDataID>{630dc1b2-d917-423d-b26f-0e085eb56560}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> ClassifierLocalStorageCells
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }


        /// <MetaDataID>{d393a5b5-77ea-4738-a31b-525b7a84e3d6}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
