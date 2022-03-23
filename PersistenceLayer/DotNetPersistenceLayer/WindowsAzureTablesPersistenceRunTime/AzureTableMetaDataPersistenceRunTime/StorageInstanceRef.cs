using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{b97c278d-9909-4c76-8b2a-40f59973fcf0}</MetaDataID>
    public class StorageInstanceRef : OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef
    {


        internal ObjectBLOBData ObjectBLOBData;

        /// <MetaDataID>{ad86f68e-b35b-44ba-8b23-151a22bdaffb}</MetaDataID>
        OOAdvantech.MetaDataRepository.StorageCell _StorageInstanceSet;
        /// <MetaDataID>{21f8088e-428e-43f4-9224-5182f6b49122}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell StorageInstanceSet
        {
            get
            {
                if (_StorageInstanceSet == null)
                    _StorageInstanceSet = new StorageCell(ObjectStorage.StorageMetaData.StorageIdentity, Class, ObjectStorage.StorageMetaData as MetaDataRepository.Namespace, LifeTimeController);
                return _StorageInstanceSet;
            }
        }
        /// <MetaDataID>{7933B97E-428C-47E6-BBA1-A8EADFC47285}</MetaDataID>
        public readonly ClassBLOB SerializationMetada;
        /// <MetaDataID>{43A985E6-225F-4C25-A0B8-C93701547847}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayerRunTime.ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, activeStorageSession, objectID)
        {
            SerializationMetada = (ObjectStorage.StorageMetaData as Storage).GetClassBLOB(Class as DotNetMetaDataRepository.Class);
        }
        /// <MetaDataID>{70DC887E-37DC-4C7C-8AAE-359E64544CD5}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            return new RelResolver(this, associationEnd, fastFieldAccessor);
        }
        /// <MetaDataID>{fb199bd1-53ed-47d9-8ffc-535fc9d6024d}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RelResolver(owner, associationEnd, fastFieldAccessor);

        }
        public void ValidateObjectState(byte[] byteStream, int offset, out int nextAvailablePos)
        {
            System.Collections.Generic.List<ValueOfAttribute> valuesOfAttribute = GetPersistentAttributeMetaData();

            if (PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
            {

            }
            foreach (Member member in SerializationMetada.Members)
            {

                if (offset == byteStream.Length)
                    break;
                object value = member.LoadMemberDataAndValidate(byteStream, this, offset, out offset);
                if (!member.IsAssociationEnd)
                {
                    foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData())
                    {
                        if (valueOfAttribute.Attribute.Name == "MetaObjectIDStream" && (value as string) == "{998AC7AE-AD3D-469D-8E8B-36FC5D59D3C4}")
                        {
                            int tyt = 0;
                        }
                        if (valueOfAttribute.FieldInfo == member.FieldInfo && valueOfAttribute.FieldInfo != null)
                        {
                            var vvalue = Member<object>.GetValue(member.FastFieldAccessor.GetValue, MemoryInstance);
                            if (value != value)
                            {

                            }

                            //SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null));
                            //SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, value);
                            break;
                        }
                    }
                }
                else
                {
                    if (value is bool && !(bool)value)
                    {

                    }

                }
            }


            nextAvailablePos = offset;

        }


        /// <MetaDataID>{A523B5AF-7480-4275-98F4-B5592364E61A}</MetaDataID>
        public void LoadObjectState(byte[] byteStream, int offset, out int nextAvailablePos)
        {
            System.Collections.Generic.List<ValueOfAttribute> valuesOfAttribute = GetPersistentAttributeMetaData();


            foreach (Member member in SerializationMetada.Members)
            {

                if (offset == byteStream.Length)
                    break;
                object value = member.LoadMemberData(byteStream, this, offset, out offset);
                if (!member.IsAssociationEnd)
                {
                    foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData())
                    {
                        if (valueOfAttribute.Attribute.Name == "MetaObjectIDStream" && (value as string) == "{998AC7AE-AD3D-469D-8E8B-36FC5D59D3C4}")
                        {
                            int tyt = 0;
                        }
                        if (valueOfAttribute.FieldInfo == member.FieldInfo && valueOfAttribute.FieldInfo != null)
                        {
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null));
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, value);
                            break;
                        }
                    }
                }
            }
            nextAvailablePos = offset;

        }



        /// <MetaDataID>{f680e109-e0e5-4b56-b975-be6e84a71d17}</MetaDataID>
        internal Guid LinkClassRoleAOID = Guid.Empty;
        /// <MetaDataID>{12476577-a9df-4934-8ae7-703cf5203c34}</MetaDataID>
        internal Guid LinkClassRoleBOID = Guid.Empty;



        /// <MetaDataID>{0585dba6-9f1e-4adf-9a5a-acd227767174}</MetaDataID>
        public void ResolveRelationships()
        {

            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            if (Class.LinkAssociation != null)
            {
                StorageInstanceRef roleAObject = (ObjectStorage as ObjectStorage).StorageObjects[new ObjectID(LinkClassRoleAOID)] as StorageInstanceRef;
                StorageInstanceRef roleBObject = (ObjectStorage as ObjectStorage).StorageObjects[new ObjectID(LinkClassRoleBOID)] as StorageInstanceRef;
                //(Class as DotNetMetaDataRepository.Class).LinkClassRoleAField.SetValue(MemoryInstance,roleAObject.MemoryInstance);
                object memoryInstance = MemoryInstance;
                Member<object>.SetValueImplicitly((Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor, ref memoryInstance, roleAObject.MemoryInstance);
                //(Class as DotNetMetaDataRepository.Class).LinkClassRoleBField.SetValue(MemoryInstance,roleBObject.MemoryInstance);

                Member<object>.SetValueImplicitly((Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor, ref memoryInstance, roleBObject.MemoryInstance);
            }



            foreach (PersistenceLayerRunTime.RelResolver relResolver in RelResolvers)
            {

                PersistenceLayerRunTime.RelResolver mResolver = relResolver;
                DotNetMetaDataRepository.AssociationEnd associationEnd = mResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;

                if (associationEnd.Multiplicity.IsMany)
                    continue;

                //if(CurrFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||CurrFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                //    continue;

                //if (!Class.IsLazyFetching(mResolver.AssociationEnd))
                //    LazyFetching(mResolver, MemoryInstance.GetType());

                #region MyRegion resolver

                if ((mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToMany || mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                       mResolver.AssociationEnd.GetOtherEnd().Navigable &&
                       mResolver.AssociationEnd.Multiplicity.IsMany)
                {
                    continue;
                }

                if (!Class.IsLazyFetching(mResolver.AssociationEnd) ||
                    ((mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToMany || mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                    mResolver.AssociationEnd.GetOtherEnd().Multiplicity.IsMany))
                {
                    LazyFetching(mResolver, MemoryInstance.GetType());

                    if ((mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToMany || mResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                        mResolver.AssociationEnd.GetOtherEnd().Navigable &&
                        mResolver.AssociationEnd.GetOtherEnd().Multiplicity.IsMany)
                    {
                        if (mResolver.RelatedObject != null)
                        {

                            var storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(mResolver.RelatedObject) as StorageInstanceRef;
                            var relationResolver = storageInstanceRef.RelResolvers.Where(x => x.AssociationEnd == mResolver.AssociationEnd.GetOtherEnd()).FirstOrDefault() as RelResolver;

                            if (relationResolver.AssociationEnd.Name == "Residents")
                            {

                            }
                            if (relationResolver.LinkedStorageInstanceRefs == null)
                                relationResolver.LinkedStorageInstanceRefs = new List<StorageInstanceRef>();

                            if(relationResolver.IsCompleteLoaded)
                            {

                            }
                            if (!relationResolver.LinkedStorageInstanceRefs.Contains(this))
                                relationResolver.LinkedStorageInstanceRefs.Add(this);
                        }
                    }
                }
                #endregion
            }
        }

        /// <MetaDataID>{0840127A-0D5D-46CD-B2D3-B4D13D3E037B}</MetaDataID>
        public void SaveObjectState(System.IO.MemoryStream memoryStream)
        {
            if (PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
            {

            }
            //if(MemoryInstance is OOAdvantech.MetaDataRepository.Namespace  &&(MemoryInstance as OOAdvantech.MetaDataRepository.Namespace).FullName == "System.Collections.Generic")
            //{
            //    var sdssda = (MemoryInstance as OOAdvantech.MetaDataRepository.Namespace).OwnedElements.ToArray();
            //    var sdssd = (MemoryInstance as OOAdvantech.MetaDataRepository.Namespace).OwnedElements.Select(x => x.FullName).ToArray();
            //}
            bool loadTest = false;
            foreach (Member member in SerializationMetada.Members)
            {
                member.SaveMemberData(memoryStream, this);
                if (member.Name == "DataBaseColumnName")
                    loadTest = true;
            }


            //if (loadTest)
            //{

            //    byte[] BLOB = new byte[memoryStream.Length];
            //    memoryStream.Position = 0;
            //    memoryStream.Read(BLOB, 0, (int)memoryStream.Length);

            //    int offset = 0;
            //    byte[] byteStream = BLOB;
            //    LoadObjectState(byteStream, offset, out offset);
            //}


        }
        /// <MetaDataID>{a6504ede-a7e4-4fb8-990a-7a6c84475089}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.ObjectID PersistentObjectID
        {
            get
            {
                return _PersistentObjectID;
            }
            set
            {

                if (value == null)
                {
                    if (base.PersistentObjectID != null && (ObjectStorage as ObjectStorage).StorageObjects.ContainsKey(base.PersistentObjectID))
                        (ObjectStorage as ObjectStorage).StorageObjects.Remove(base.PersistentObjectID);
                    base.PersistentObjectID = value;
                }
                else
                {
                    if (base.PersistentObjectID != value)
                    {
                        base.PersistentObjectID = value;
                        (ObjectStorage as ObjectStorage).StorageObjects.Add(value, this);
                    }
                }




            }
        }



    }
}
