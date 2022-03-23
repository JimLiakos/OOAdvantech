using System.Linq;
using OOAdvantech.Transactions;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    /// <MetaDataID>{8DD09765-63C6-4F1F-9087-3BF4E5DF1C4D}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{8DD09765-63C6-4F1F-9087-3BF4E5DF1C4D}")]
    [MetaDataRepository.Persistent()]
    public class Component : MetaDataRepository.Component
    {

        /// <MetaDataID>{c5c36634-24ad-40e7-8563-1cf551654fc7}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{07cba1f2-08ff-4aa0-af98-dd29dcb58ee4}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{7ad3c006-646d-4077-b1c1-cd0ab6ad4bc2}</MetaDataID>
        public Component()
        {


        }
        /// <MetaDataID>{41d7c754-739d-4942-9771-322c236b9f0e}</MetaDataID>
        [MetaDataRepository.ObjectActivationCall]
        public void OnActivate()
        {
#if	!DeviceDotNet

            string assemblyFullName = GetPropertyValue<string>("MetaData", "AssemblyFullName");
            if (!string.IsNullOrEmpty(assemblyFullName))
                ModulePublisher.ClassRepository.LoadAssembly(assemblyFullName);
#endif

        }


        /// <MetaDataID>{04362F9C-4715-4CA4-8F6B-7EB458FB9679}</MetaDataID>
        public override bool HasPersistentClasses
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{cd1dab04-a182-490c-82b0-4d66ff1c61e2}</MetaDataID>
        private Association GetColumnMappedAssociationEnd(StorageCellsLink storageCellsLink, OOAdvantech.MetaDataRepository.Classifier Type, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePathIdentity)
        {

            if (valueTypePathIdentity == null)
                valueTypePathIdentity = new OOAdvantech.MetaDataRepository.ValueTypePath();

            if (string.IsNullOrEmpty(storageCellsLink.ValueTypePath))
            {
                foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                {
                    if (associationEnd.Association.Identity == storageCellsLink.Type.Identity)
                    {
                        return associationEnd.Association as Association;
                        break;
                    }
                }
            }
            else
            {
                foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                {
                    try
                    {
                        valueTypePathIdentity.Push(associationEnd.Identity);
                        if (associationEnd.Association.Identity == storageCellsLink.Type.Identity)
                        {
                            return associationEnd.Association as Association;
                            break;
                        }
                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
            }

            foreach (Attribute attribute in Type.GetAttributes(true))
            {
                if (attribute.IsPersistentValueType)
                {
                    valueTypePathIdentity.Push(attribute.Identity);
                    try
                    {
                        Association mappedAttribute = GetColumnMappedAssociationEnd(storageCellsLink, attribute.Type, valueTypePathIdentity);
                        if (mappedAttribute != null)
                            return mappedAttribute;
                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
            }
            return null;

        }



        /// <MetaDataID>{0649BB01-B8C7-416A-8A5B-42F2AC3C437D}</MetaDataID>
        public void BuildMappingElement(PersistenceLayer.Storage HostObjectStorage, System.Xml.Linq.XDocument mappingData)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            System.Collections.Generic.List<StorageCellsLink> storageCellsLinks = new System.Collections.Generic.List<StorageCellsLink>();
            try
            {
                System.Collections.Generic.Dictionary<MetaDataRepository.MetaObjectID, Class> mappedClasses = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Class>();
                if (mappingData != null)// !string.IsNullOrEmpty(mappingData))
                {
                    System.Xml.Linq.XDocument document = mappingData;
                    //new System.Xml.XmlDocument();
                    //document.LoadXml(mappingData);


                    PersistenceLayer.ObjectStorage ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage("RDBMSMapingStorage", document, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
                    foreach (Class _class in from _class in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.Class>()
                                             select _class)
                    {
                        mappedClasses[_class.Identity] = _class;
                    }
                    storageCellsLinks = new System.Collections.Generic.List<StorageCellsLink>(from storageCellsLink in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>()
                                                                                              select storageCellsLink).ToList();
                    storageCellsLinks = (from storageCellsLink in storageCellsLinks
                                         where storageCellsLink.Type.RoleA != null && storageCellsLink.Type.RoleB != null && storageCellsLink.Type.RoleA.Specification != null && storageCellsLink.Type.RoleB.Specification != null
                                         select storageCellsLink).ToList();

                }

                if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                    return;
                MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);

                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    foreach (MetaDataRepository.Dependency CurrDependency in ClientDependencies)
                    {
                        try
                        {
                            ((Component)CurrDependency.Supplier).BuildMappingElement(HostObjectStorage, null);
                        }
                        catch (System.Exception error)
                        {
                        }
                    }
                    foreach (Class @class in Residents.OfType<Class>())
                    {
                        {
                            if (mappedClasses.ContainsKey(@class.Identity))
                            {
                                @class.BuildMappingElement();
                                @class.UpdateMappingElement((mappedClasses[@class.Identity] as Class));
                            }
                            else
                                @class.BuildMappingElement();
                        }
                    }

                    //System.Collections.Generic.Dictionary<MetaDataRepository.Association, MetaDataRepository.Association> Associations = new System.Collections.Generic.Dictionary<MetaDataRepository.Association, MetaDataRepository.Association>();
                    System.Collections.Generic.Dictionary<MetaDataRepository.Generalization, MetaDataRepository.Generalization> Generalizations = new System.Collections.Generic.Dictionary<MetaDataRepository.Generalization, MetaDataRepository.Generalization>();


                    foreach (MetaDataRepository.MetaObject metaObject in Residents)
                    {
                        if (metaObject is MetaDataRepository.Class)
                        {
                            MetaDataRepository.Class CurrClass = (MetaDataRepository.Class)metaObject;
                            if (!CurrClass.Persistent)
                                continue;

                            bool ClassHierarchyMembers = true;
                            foreach (Generalization CurrGeneralization in CurrClass.Generalizations)
                            {
                                if (CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerClass)
                                    ClassHierarchyMembers = false;
                            }
                            //foreach(MetaDataRepository.AssociationEnd CurrAssociationEnd in CurrClass.GetAssociateRoles(ClassHierarchyMembers))
                            //{
                            //    if(!Associations.ContainsKey(CurrAssociationEnd.Association))
                            //        Associations.Add(CurrAssociationEnd.Association,CurrAssociationEnd.Association);
                            //}
                            //if(CurrClass.ClassHierarchyLinkAssociation!=null)
                            //    if(!Associations.ContainsKey(CurrClass.ClassHierarchyLinkAssociation))
                            //        Associations.Add(CurrClass.ClassHierarchyLinkAssociation,CurrClass.ClassHierarchyLinkAssociation);

                            foreach (MetaDataRepository.Generalization CurrGeneralization in CurrClass.Generalizations)
                            {
                                if (!Generalizations.ContainsKey(CurrGeneralization))
                                    Generalizations.Add(CurrGeneralization, CurrGeneralization);
                            }

                        }
                        else
                        {
                            if (metaObject is MetaDataRepository.Interface)
                            {
                                Interface _interface = metaObject as Interface;
                                _interface.BuildMappingElement();
                            }
                        }
                    }

                    foreach (var CurrDictionaryEntry in Generalizations)
                    {
                        Generalization CurrGeneralization = CurrDictionaryEntry.Value as Generalization;
                        CurrGeneralization.BuildMappingElement();
                    }
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                    foreach (RDBMSMetaDataRepository.Key key in from key in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.Key>()
                                                                where !key.IsPrimaryKey
                                                                select key)
                    {
                        if (key.Columns.Count != key.ReferedColumns.Count || key.Columns.Count == 0)
                            key.OriginTable.RemoveForeignKey(key);
                    }

                    foreach (StorageCellsLink storageCellsLink in from storageCellsLink in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>()
                                                                  select storageCellsLink)
                    {
                        if (storageCellsLink.ObjectLinksTable != null &&
                               storageCellsLink.Type.RoleA != null && storageCellsLink.Type.RoleB != null &&
                               storageCellsLink.Type.RoleA.Specification != null && storageCellsLink.Type.RoleB.Specification != null)
                        {
                            storageCellsLink.UpdateForeignKeys();
                            storageCellsLinks.Add(storageCellsLink);
                        }
                    }
                    foreach (StorageCellsLink storageCellsLink in storageCellsLinks)
                    {
                        if ((storageCellsLink.RoleAStorageCell is StorageCellReference || storageCellsLink.RoleBStorageCell is StorageCellReference))
                            continue;

                        StorageCell roleAStorageCell = null;
                        StorageCell roleBStorageCell = null;
                        foreach (MetaDataRepository.MetaObject metaObject in (HostObjectStorage as Storage).OwnedElements)
                        {

                            if (metaObject is StorageCell &&
                                (metaObject as StorageCell).Type.Identity == storageCellsLink.RoleAStorageCell.Type.Identity &&
                                (metaObject as StorageCell).MainTable.Name == (storageCellsLink.RoleAStorageCell as StorageCell).MainTable.Name)
                                roleAStorageCell = metaObject as StorageCell;
                            if (metaObject is StorageCell &&
                                (metaObject as StorageCell).Type.Identity == storageCellsLink.RoleBStorageCell.Type.Identity &&
                                (metaObject as StorageCell).MainTable.Name == (storageCellsLink.RoleBStorageCell as StorageCell).MainTable.Name)
                                roleBStorageCell = metaObject as StorageCell;
                            if (roleAStorageCell != null && roleBStorageCell != null)
                                break;
                        }
                        //Association association = GetColumnMappedAssociationEnd(storageCellsLink, roleBStorageCell.Type, new OOAdvantech.MetaDataRepository.ValueTypePath());


                        //StorageCellsLink newStorageCellsLink= association.GetStorageCellsLink(roleAStorageCell, roleBStorageCell, storageCellsLink.ValueTypePath, true);
                        //storageCellsLink.ValueTypePath = newStorageCellsLink.ValueTypePath;
                        //if (newStorageCellsLink.ValueTypePath == null)
                        //    newStorageCellsLink.ValueTypePath = "";

                        if (storageCellsLink.ObjectLinksTable != null &&
                            storageCellsLink.Type.RoleA != null && storageCellsLink.Type.RoleB != null &&
                            storageCellsLink.Type.RoleA.Specification != null && storageCellsLink.Type.RoleB.Specification != null)
                        {
                            //Table objectLinksTable = new Table(storageCellsLink.ObjectLinksTable.Name, newStorageCellsLink);
                            //OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(objectLinksTable);
                            (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(storageCellsLink.ObjectLinksTable, storageCellsLink.ValueTypePath, new MetaDataRepository.ValueTypePath(storageCellsLink.ValueTypePath), (roleBStorageCell as StorageCell).ObjectIdentityType);
                            (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(storageCellsLink.ObjectLinksTable, storageCellsLink.ValueTypePath, new MetaDataRepository.ValueTypePath(storageCellsLink.ValueTypePath), (roleAStorageCell as StorageCell).ObjectIdentityType);


                            //foreach (Column column in storageCellsLink.ObjectLinksTable.ContainedColumns)
                            //{
                            //    if (column is IdentityColumn)
                            //    {

                            //           IdentityColumn identityColumn = column as IdentityColumn;
                            //        IdentityColumn newIdentityColumn = OOAdvantech.RDBMSMetaDataRepository.AutoProduceColumnsGenerator.GetObjectIdentityColumn(identityColumn.Name, identityColumn.Type, this);
                            //        newIdentityColumn.ColumnType = identityColumn.ColumnType;
                            //        newIdentityColumn.Name = identityColumn.Name;
                            //        newIdentityColumn.Length = identityColumn.Length;
                            //        newIdentityColumn.IsIdentity = identityColumn.IsIdentity;
                            //        newIdentityColumn.IdentityIncrement = identityColumn.IdentityIncrement;
                            //        newIdentityColumn.AllowNulls = identityColumn.AllowNulls;
                            //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(newIdentityColumn);
                            //        objectLinksTable.AddColumn(newIdentityColumn);
                            //        if (identityColumn.MappedAssociationEnd != null)
                            //        {
                            //            AssociationEnd associationEnd = null;
                            //            if (identityColumn.MappedAssociationEnd.Identity == storageCellsLink.Type.RoleA.Identity)
                            //                associationEnd = newStorageCellsLink.Type.RoleA as AssociationEnd;
                            //            if (identityColumn.MappedAssociationEnd.Identity == storageCellsLink.Type.RoleB.Identity)
                            //                associationEnd = newStorageCellsLink.Type.RoleB as AssociationEnd;
                            //            if (identityColumn.MappedAssociationEnd.IsRoleA)
                            //                (associationEnd.Association.RoleA as AssociationEnd).AddReferenceColumn(newIdentityColumn);
                            //            else
                            //                (associationEnd.Association.RoleB as AssociationEnd).AddReferenceColumn(newIdentityColumn);
                            //        }
                            //    }
                            //    else if (column.IndexerAssociationEnd!=null)
                            //    {
                            //        Column indexerColumn = new Column();
                            //        indexerColumn.Name = column.Name;
                            //        indexerColumn.Length = column.Length;
                            //        indexerColumn.IsIdentity = column.IsIdentity;
                            //        indexerColumn.IdentityIncrement = column.IdentityIncrement;
                            //        indexerColumn.AllowNulls = column.AllowNulls;
                            //        MetaDataRepository.Classifier columnType =MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(column.Type,objectLinksTable) as MetaDataRepository.Classifier;
                            //        if (columnType == null)
                            //        {
                            //            columnType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(column.Type, objectLinksTable) as MetaDataRepository.Classifier;
                            //            columnType.Synchronize(column.Type);
                            //        }
                            //        indexerColumn.Type = columnType;
                            //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                            //        objectLinksTable.AddColumn(indexerColumn);

                            //        if (column.IndexerAssociationEnd.IsRoleA)
                            //            (newStorageCellsLink.Type.RoleA as AssociationEnd).AddIndexerColumn(indexerColumn);
                            //        else
                            //            (newStorageCellsLink.Type.RoleB as AssociationEnd).AddIndexerColumn(indexerColumn);

                            //    }

                            //}
                            //newStorageCellsLink.ObjectLinksTable = objectLinksTable;
                        }

                        //if(string.IsNullOrEmpty(storageCellsLink.ValueTypePath))
                        //newStorageCellsLink.UpdateForeignKeys();







                    }



                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        /// <MetaDataID>{80847D2F-E71F-46EB-B84B-25FC4B46E2EF}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {


            if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                return;


            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _MappingVersion = (OriginMetaObject as OOAdvantech.MetaDataRepository.Component).MappingVersion;

                    if (((MetaDataRepository.Component)OriginMetaObject).HasPersistentClasses)
                        base.Synchronize(OriginMetaObject);
                    else
                        Name = OriginMetaObject.Name;
                    StateTransition.Consistent = true; ;
                }
                MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
    }
}
