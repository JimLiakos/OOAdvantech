namespace OOAdvantech.RDBMSMetaDataRepository
{
    using OOAdvantech.MetaDataRepository;
    using System.Linq;

    /// <MetaDataID>{0E577619-38DB-427E-A074-6CB8F575BD50}</MetaDataID>
    [BackwardCompatibilityID("{0E577619-38DB-427E-A074-6CB8F575BD50}")]
    [Persistent()]
    public class Attribute : MetaDataRepository.Attribute
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(AttributeColumnsCache))
            {
                if (value == null)
                    AttributeColumnsCache = default(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column>>);
                else
                    AttributeColumnsCache = (System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column>>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(StorageCellsColumns))
            {
                if (value == null)
                    StorageCellsColumns = default(System.Collections.Generic.Dictionary<OOAdvantech.RDBMSMetaDataRepository.StorageCell, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column>>);
                else
                    StorageCellsColumns = (System.Collections.Generic.Dictionary<OOAdvantech.RDBMSMetaDataRepository.StorageCell, System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column>>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(MappedColumns))
            {
                if (value == null)
                    MappedColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    MappedColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(AttributeColumnsCache))
                return AttributeColumnsCache;

            if (member.Name == nameof(StorageCellsColumns))
                return StorageCellsColumns;

            if (member.Name == nameof(MappedColumns))
                return MappedColumns;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{798a910e-efb7-4b50-9161-ccd7716726aa}</MetaDataID>
        public System.Collections.Generic.List<Column> GetColumns(string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            // StorageCellsColumns.Clear();
            //TODO να γραφτεί test case για την περίπτωση που προστεθεί persistent attribute με τον όρο new στην
            //sub class και έχει ίδιο όνομα με persistent attribute της parent class.

            System.Collections.Generic.List<Column> columns = new System.Collections.Generic.List<Column>();


            pathIdentity.Push(Identity);
            string creatorIdentity = "";
            if (Owner is Structure)
                creatorIdentity = pathIdentity.ToString();

            try
            {


                if (Type is MetaDataRepository.Structure && (Type as MetaDataRepository.Structure).Persistent)
                {
                    #region Update attribute columns

                    Collections.Generic.Set<MetaDataRepository.Attribute> attributes = Type.GetAttributes(true);

                    foreach (Attribute attribute in attributes)
                    {
                        if ((Type as MetaDataRepository.Structure).IsPersistent(attribute))
                            columns.AddRange(attribute.GetColumns(path + "_" + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity));
                    }
                    #endregion

                    #region Update association relationship columns

                    foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                    {
                        if (associationEnd.Association.HasPersistentObjectLink)
                            foreach (Column column in GetReferenceColumns(associationEnd, path, pathIdentity))
                            {
                                // column.MappedAssociationEnd = associationEnd;
                                columns.Add(column);
                            }
                        if (associationEnd.Indexer)
                        {
                            Column indexerColumn = GetIndexerColumn(associationEnd, path, pathIdentity);
                            if (indexerColumn != null)
                                columns.Add(indexerColumn);
                        }
                    }

                    #endregion

                }
                else
                {
                    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                    try
                    {
                        object mLength = null;
                        mLength = GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                        Column newColumn = null;
                        string valuePath = "";
                        if (Owner is MetaDataRepository.Structure)
                            valuePath = creatorIdentity;
                        if (mLength != null)
                            newColumn = new Column(this, (int)mLength, true, false, 1, valuePath);
                        else
                            newColumn = new Column(this, 0, true, false, 1, valuePath);
                        newColumn.Name = path;
                        columns.Add(newColumn);
                    }
                    finally
                    {
                        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                    }
                }

                return columns;
            }
            finally
            {
                pathIdentity.Pop();
            }
        }

        /// <MetaDataID>{bb6f6d19-943c-43ee-a88e-b2f6b5fbe856}</MetaDataID>
        System.Collections.Generic.List<Column> AddColumnToTableOrUpdate(Table table, string path, MetaDataRepository.ValueTypePath pathIdentity, bool isMultilingual)
        {
            return AddColumnToTableOrUpdate(table, "", path, pathIdentity, isMultilingual);
        }

        /// <MetaDataID>{b6acf8fd-4dcd-4e9f-9a34-2934e8c5a391}</MetaDataID>
        System.Collections.Generic.List<Column> AddColumnToTableOrUpdate(Table table, string columnName, string path, MetaDataRepository.ValueTypePath pathIdentity, bool isMultilingual)
        {
            StorageCellsColumns.Clear();
            //TODO να γραφτεί test case για την περίπτωση που προστεθεί persistent attribute με τον όρο new στην
            //sub class και έχει ίδιο όνομα με persistent attribute της parent class.


            MetaDataRepository.Class systemStringType = null;
            if (isMultilingual)
            {


                systemStringType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.String", this) as MetaDataRepository.Class;
                if (systemStringType == null)
                {
                    MetaDataRepository.Class mClass = MetaDataRepository.Classifier.GetClassifier(typeof(string)) as MetaDataRepository.Class;
                    systemStringType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mClass, this) as MetaDataRepository.Class;
                    if (systemStringType == null)
                        systemStringType = (Class)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mClass, this);
                    systemStringType.Synchronize(mClass);
                }
                
            }


            System.Collections.Generic.List<Column> columns = new System.Collections.Generic.List<Column>();


            if (!string.IsNullOrEmpty(path))
                path += "_";

            pathIdentity.Push(Identity);
            string creatorIdentity = "";
            if (Owner is Structure)
                creatorIdentity = pathIdentity.ToString();


            try
            {


                if (Type is MetaDataRepository.Structure && (Type as MetaDataRepository.Structure).Persistent)
                {
                    #region Update attribute columns

                    Collections.Generic.Set<MetaDataRepository.Attribute> attributes = Type.GetAttributes(true);

                    foreach (Attribute attribute in attributes)
                    {

                        
                        if ((Type as MetaDataRepository.Structure).IsPersistent(attribute))
                            columns.AddRange(attribute.AddColumnToTableOrUpdate(table, path + Name, pathIdentity,isMultilingual));
                    }
                    foreach (Column column in table.ContainedColumns)
                    {
                        if(isMultilingual)
                        {

                            if (column.MappedAttribute != null && !attributes.Contains(column.MappedAttribute) && (column.MappedAttribute == this || Type == column.MappedAttribute.Owner || Type.IsA(column.MappedAttribute.Owner)))
                                table.RemoveColumn(column);

                        }

                        if (column.MappedAttribute != null && !attributes.Contains(column.MappedAttribute) && (column.MappedAttribute == this || Type == column.MappedAttribute.Owner || Type.IsA(column.MappedAttribute.Owner)))
                            table.RemoveColumn(column);
                    }


                    #endregion

                    #region Update association relationship columns

                    foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                    {
                        if (associationEnd.Association.HasPersistentObjectLink)
                            foreach (Column column in associationEnd.AddReferenceColumnsToTable(table, path + Name, pathIdentity, null))
                                columns.Add(column);
                    }

                    #endregion

                }
                else
                {

                    foreach (Column column in GetColumnsFor(table.TableCreator as StorageCell))
                    {
                        if (column != null && column.Namespace == table && column.CreatorIdentity == creatorIdentity)
                        {
                            MetaDataRepository.AttributeRealization attributeRealization = (table.TableCreator as StorageCell).Type.GetAttributeRealization(this);
                            column.Name = path + CaseInsensitiveName;
                            if (isMultilingual)
                                column.Type = systemStringType;
                            else
                                column.Type = Type;
                            object mLength = null;

                            if (attributeRealization != null)
                                mLength = attributeRealization.GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                            else
                                mLength = GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                            Column newColumn = null;
                            if (mLength != null)
                                column.Length = (int)mLength;

                            if(columns.Count==0)
                                columns.Add(column);
                            else
                            {
                                if(column.Namespace is Table)
                                    (column.Namespace as Table).RemoveColumn(column);
                                StorageCellsColumns.Clear();
                            }
                            
                        }
                    }
                    if (columns.Count > 0)
                        return columns;


                    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                    try
                    {

                        using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {

                            MetaDataRepository.AttributeRealization attributeRealization = (table.TableCreator as StorageCell).Type.GetAttributeRealization(this);
                            object mLength = null;
                            if (attributeRealization != null)
                                mLength = attributeRealization.GetPropertyValue(typeof(int), "Persistent", "SizeOf");
                            else
                                mLength = GetPropertyValue(typeof(int), "Persistent", "SizeOf");

                            Column newColumn = null;
                            string valuePath = "";
                            if (Owner is MetaDataRepository.Structure)
                                valuePath = creatorIdentity;

                            if (mLength != null)
                                newColumn = new Column(this, (int)mLength, true, false, 1, valuePath);
                            else
                                newColumn = new Column(this, 0, true, false, 1, valuePath);
                            if (string.IsNullOrEmpty(columnName))
                                newColumn.Name = path + CaseInsensitiveName;
                            else
                                newColumn.Name = columnName;
                            if (attributeRealization != null)
                                newColumn.MappedAttributeRealizationIdentity = attributeRealization.Identity.ToString();
                            
                            if (isMultilingual)
                                newColumn.Type = systemStringType;

                            MappedColumns.Add(newColumn);
                            //table.MakeNameUnaryInNamesapce(newColumn);
                            table.AddColumn(newColumn);
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(newColumn);
                            columns.Add(newColumn);
                            StateTransition.Consistent = true;
                        }
                    }
                    finally
                    {
                        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                    }
                }


                return columns;
            }
            finally
            {
                //if (Owner is Structure)
                pathIdentity.Pop();
                StorageCellsColumns.Clear();
            }




        }

        /// <MetaDataID>{a8222830-5967-4f15-a313-92cf7b1290c8}</MetaDataID>
        public System.Collections.Generic.List<Column> GetColumns()
        {
            return GetColumns("", new ValueTypePath());
        }
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Column>> AttributeColumnsCache = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Column>>();
        /// <MetaDataID>{3601d694-3a0d-4ad2-96c8-29aef32ff220}</MetaDataID>
        public System.Collections.Generic.List<Column> GetColumns(string classHerarchyCaseInsensitiveUniqueName)
        {
            System.Collections.Generic.List<Column> columns = null;
            if (!AttributeColumnsCache.TryGetValue(classHerarchyCaseInsensitiveUniqueName, out columns))
            {
                columns = GetColumns(classHerarchyCaseInsensitiveUniqueName, new ValueTypePath());
                AttributeColumnsCache[classHerarchyCaseInsensitiveUniqueName] = columns;
            }

            return columns;
        }

        /// <MetaDataID>{b941f9df-4cd3-4890-b281-7377950e916e}</MetaDataID>
        public System.Collections.Generic.List<ValueTypePathMember> GetValuTypeAssociationEnds(string classHerarchyCaseInsensitiveUniqueName)
        {
            return GetValuTypeAssociationEnds(classHerarchyCaseInsensitiveUniqueName, new ValueTypePath());
        }
        /// <MetaDataID>{64366e85-7731-4c73-b8ae-3103bdd7ea10}</MetaDataID>
        System.Collections.Generic.List<ValueTypePathMember> GetValuTypeAssociationEnds(string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            System.Collections.Generic.List<ValueTypePathMember> ValuTypeAssociationEnds = new System.Collections.Generic.List<ValueTypePathMember>();
            pathIdentity.Push(Identity);
            string creatorIdentity = "";
            if (Owner is Structure)
                creatorIdentity = pathIdentity.ToString();
            try
            {
                if (Type is MetaDataRepository.Structure && (Type as MetaDataRepository.Structure).Persistent)
                {
                    foreach (Attribute attribute in Type.GetAttributes(true))
                    {
                        if (attribute.IsPersistentValueType)
                            ValuTypeAssociationEnds.AddRange(attribute.GetValuTypeAssociationEnds(path + "_" + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity));
                    }
                    #region Update association relationship columns

                    foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                    {
                        if (associationEnd.Association.HasPersistentObjectLink)
                            ValuTypeAssociationEnds.Add(new ValueTypePathMember(new ValueTypePath(pathIdentity), path, associationEnd));
                    }

                    #endregion

                }


                return ValuTypeAssociationEnds;
            }
            finally
            {
                pathIdentity.Pop();
            }
        }





        /// <MetaDataID>{015D321D-6AA9-43CC-8F28-E025DF024C28}</MetaDataID>
        public System.Collections.Generic.List<Column> AddColumnToTableOrUpdate(Table table, bool isMultilingual)
        {
            return AddColumnToTableOrUpdate(table, "", new ValueTypePath(), isMultilingual);
        }
        /// <MetaDataID>{550d31e3-2cd9-4fee-ab91-84d751669ac9}</MetaDataID>
        public Column AddColumnToTableOrUpdate(Table table, string columnName, bool isMultilingual)
        {
            return AddColumnToTableOrUpdate(table, columnName, "", new ValueTypePath(), isMultilingual)[0];
        }

        /// <MetaDataID>{382b5df2-0050-4eb3-bc16-6e74f8fee83b}</MetaDataID>
        System.Collections.Generic.Dictionary<StorageCell, System.Collections.Generic.List<Column>> StorageCellsColumns = new System.Collections.Generic.Dictionary<StorageCell, System.Collections.Generic.List<Column>>();
        /// <MetaDataID>{06D45270-BF1E-41F8-B1A1-B2E8C26EE7C7}</MetaDataID>
        public System.Collections.Generic.List<Column> GetColumnsFor(StorageCell storageCell)
        {


            System.Collections.Generic.List<Column> columns = null;
            if (StorageCellsColumns.TryGetValue(storageCell, out columns))
            {
                return columns.ToList();
            }

            columns = new System.Collections.Generic.List<Column>();
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                //long mcount = MappedColumns.Count;
                foreach (Column column in MappedColumns.ToList())
                {
                    if (column.Namespace != null)
                    {
                        if ((column.Namespace as Table).TableCreator as StorageCell == storageCell)
                        {
                            //column.Name=Name; //error prone
                            columns.Add(column);
                        }
                    }
                    else
                    {
                        RemoveColumn(column);
                    }

                }
                StorageCellsColumns[storageCell] = columns;

                return columns.ToList();
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }

        /// <MetaDataID>{D804F7DC-0B07-461E-9E68-6FB18F261A42}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    AttributeColumnsCache = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Column>>();
                    base.Synchronize(OriginMetaObject);
                    foreach (object CurrObject in OriginMetaObject.GetExtensionMetaObjects())
                    {
                        if (typeof(System.Reflection.FieldInfo).GetMetaData().IsInstanceOfType(CurrObject))
                        {
                            System.Reflection.FieldInfo mFieldInfo = (System.Reflection.FieldInfo)CurrObject;
                            foreach (MetaDataRepository.PersistentMember CurrAttribute in mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), true))
                            {
                                if (!mFieldInfo.FieldType.GetMetaData().IsValueType)
                                    PutPropertyValue("Persistent", "SizeOf", CurrAttribute.Length);
                                break;
                            }
                        }


                        if (typeof(System.Reflection.PropertyInfo).GetMetaData().IsInstanceOfType(CurrObject))
                        {
                            System.Reflection.PropertyInfo mPropertyInfo = (System.Reflection.PropertyInfo)CurrObject;
                            foreach (MetaDataRepository.PersistentMember CurrAttribute in mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), true))
                            {
                                if (!mPropertyInfo.PropertyType.GetMetaData().IsValueType)
                                    PutPropertyValue("Persistent", "SizeOf", CurrAttribute.Length);
                                break;
                            }
                        }
                    }
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }



        }
        //TODO Error Prone Thread Safety
        /// <MetaDataID>{D4FD75C3-28D5-478D-A578-FB0CE03637F5}</MetaDataID>
        [Association("AttributeColumnMapping", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{453C9F8A-B2A0-4DFB-916F-8D93A26F8473}")]
        [PersistentMember("MappedColumns")]
        [RoleAMultiplicityRange(1)]
        private Collections.Generic.Set<Column> MappedColumns = new Collections.Generic.Set<Column>();



        /// <MetaDataID>{5179fe62-eddd-47af-afa4-f333053d85ef}</MetaDataID>
        System.Collections.Generic.List<RDBMSMetaDataRepository.Column> GetReferenceColumns(AssociationEnd associationEnd)
        {
            return GetReferenceColumns(associationEnd, CaseInsensitiveName, new ValueTypePath());
        }

        /// <MetaDataID>{ea5e07dc-bb5e-4770-a538-eded91353e81}</MetaDataID>
        System.Collections.Generic.List<RDBMSMetaDataRepository.Column> GetReferenceColumns(AssociationEnd associationEnd, string classHerarchyCaseInsensitiveUniqueName)
        {
            return GetReferenceColumns(associationEnd, classHerarchyCaseInsensitiveUniqueName, new ValueTypePath());
        }



        /// <MetaDataID>{3af8ce89-6923-4789-ae48-71231cc9c7f1}</MetaDataID>
        Column GetIndexerColumn(AssociationEnd associationEnd, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            if (!IsPersistentValueType)
                return null;
            if (!associationEnd.Indexer)
                return null;
            pathIdentity.Push(Identity);

            try
            {
                foreach (AssociationEnd structureAssociationEnd in Type.GetRoles(true))
                {
                    if (structureAssociationEnd == associationEnd)
                    {

                        Column column = structureAssociationEnd.GetIndexerColumn(Type, path);
                        if (column == null)
                            return null;
                        column.CreatorIdentity = pathIdentity.ToString();
                        return column;
                    }
                }
                foreach (Attribute attribute in Type.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType)
                    {
                        Column column = attribute.GetIndexerColumn(associationEnd, path + "_" + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity);
                        if (column != null)
                            return column;
                    }
                }
                return null;
            }
            finally
            {
                pathIdentity.Pop();

            }
        }



        /// <MetaDataID>{78f5d03a-05d1-48fe-8c2f-4a6e90ca56fd}</MetaDataID>
        System.Collections.Generic.List<RDBMSMetaDataRepository.Column> GetReferenceColumns(AssociationEnd associationEnd, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            if (!string.IsNullOrEmpty(path))
                path += "_";

            if (!IsPersistentValueType)
                return null;
            if (pathIdentity.Count > 0 && pathIdentity.Peek() != Identity)
                pathIdentity.Push(Identity);
            //MetaDataRepository.ObjectIdentityType objectIdentityType = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this);
            System.Collections.Generic.List<Column> referenceColumns = new System.Collections.Generic.List<Column>();
            try
            {
                foreach (AssociationEnd structureAssociationEnd in Type.GetRoles(true))
                {
                    if (structureAssociationEnd == associationEnd)
                    {
                        pathIdentity.Push(associationEnd.Identity);

                        MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = structureAssociationEnd.GetReferenceObjectIdentityType(Type);//, new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>() { objectIdentityType})[0];
                        foreach (MetaDataRepository.IIdentityPart part in referenceObjectIdentityType.Parts)
                        {
                            IdentityColumn referencColumn = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetIdentityColumn(part, this);
                            referencColumn.Name = path + part.Name;
                            referencColumn.CreatorIdentity = pathIdentity.ToString();
                            referencColumn.MappedAssociationEnd = associationEnd;
                            referencColumn.ObjectIdentityType = referenceObjectIdentityType;
                            referenceColumns.Add(referencColumn);
                        }
                        return referenceColumns;
                    }
                }
                foreach (Attribute attribute in Type.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType)
                    {
                        referenceColumns = attribute.GetReferenceColumns(associationEnd, path + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity);
                        if (referenceColumns != null)
                            return referenceColumns;
                    }
                }
                return null;
            }
            finally
            {
                pathIdentity.Pop();
            }
        }

        public string GetReferenceStorageIdentityColumnName(AssociationEnd associationEnd)
        {
            return GetReferenceStorageIdentityColumnName(associationEnd, CaseInsensitiveName, new ValueTypePath());
        }
        public string GetReferenceStorageIdentityColumnName(AssociationEnd associationEnd, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {


            if (!string.IsNullOrEmpty(path))
                path += "_";

            if (!IsPersistentValueType)
                return "";
            pathIdentity.Push(Identity);

            try
            {
                foreach (AssociationEnd structureAssociationEnd in Type.GetRoles(true))
                {
                    if (structureAssociationEnd == associationEnd)

                        return path + structureAssociationEnd.ReferenceStorageIdentityColumnName;
                }
                foreach (Attribute attribute in Type.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType)
                    {
                        string referenceStorageIdentityColumnName = attribute.GetReferenceStorageIdentityColumnName(associationEnd, path + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity);
                        if (!string.IsNullOrEmpty(referenceStorageIdentityColumnName))
                            return referenceStorageIdentityColumnName;
                    }
                }
                return "";
            }
            finally
            {
                pathIdentity.Pop();
            }
        }
        /// <MetaDataID>{feb5aff0-c0dc-43f7-a64f-28de3f420d62}</MetaDataID>
        public System.Collections.Generic.List<ObjectIdentityType> GetReferenceObjectIdentityTypes(AssociationEnd associationEnd, System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes)
        {

            return GetReferenceObjectIdentityTypes(associationEnd, objectIdentityTypes, CaseInsensitiveName, new ValueTypePath());
        }

        /// <MetaDataID>{ca0ffd1c-ab19-4a7a-9302-148e11493f92}</MetaDataID>
        public System.Collections.Generic.List<ObjectIdentityType> GetReferenceObjectIdentityTypes(AssociationEnd associationEnd, System.Collections.Generic.List<ObjectIdentityType> ObjectIdentityTypes, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {


            if (!string.IsNullOrEmpty(path))
                path += "_";

            if (!IsPersistentValueType)
                return new System.Collections.Generic.List<ObjectIdentityType>();
            pathIdentity.Push(Identity);

            try
            {
                foreach (AssociationEnd structureAssociationEnd in Type.GetRoles(true))
                {
                    if (structureAssociationEnd == associationEnd)
                    {
                        System.Collections.Generic.List<ObjectIdentityType> referenceColumns = new System.Collections.Generic.List<ObjectIdentityType>();
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in structureAssociationEnd.GetReferenceObjectIdentityTypes(Type, ObjectIdentityTypes))
                        {
                            System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                parts.Add(new IdentityPart(path + part.Name, part.PartTypeName, part.Type));
                            referenceColumns.Add(new ObjectIdentityType(parts));
                        }
                        return referenceColumns;
                    }
                }
                foreach (Attribute attribute in Type.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType)
                    {
                        System.Collections.Generic.List<ObjectIdentityType> referenceColumns = attribute.GetReferenceObjectIdentityTypes(associationEnd, ObjectIdentityTypes, path + Type.GetClassHerarchyCaseInsensitiveUniqueNames(attribute), pathIdentity);
                        if (referenceColumns.Count > 0)
                            return referenceColumns;
                    }
                }
                return new System.Collections.Generic.List<ObjectIdentityType>();
            }
            finally
            {
                pathIdentity.Pop();
            }
        }

        /// <MetaDataID>{344e7b93-bab4-4c00-a07c-9812701e87f5}</MetaDataID>
        public string GetAttributeColumnName(OOAdvantech.MetaDataRepository.Attribute attribute)
        {
            return GetAttributeColumn(attribute, CaseInsensitiveName, new ValueTypePath()).Name;

        }
        /// <MetaDataID>{67f24b73-682a-4836-9427-c34f3dc822c6}</MetaDataID>
        public RDBMSMetaDataRepository.Column GetAttributeColumn(OOAdvantech.MetaDataRepository.Attribute attribute)
        {
            return GetAttributeColumn(attribute, CaseInsensitiveName, new ValueTypePath());
        }
        /// <MetaDataID>{53a476eb-10fc-478b-99ea-7d4956f244d9}</MetaDataID>
        public RDBMSMetaDataRepository.Column GetAttributeColumn(OOAdvantech.MetaDataRepository.Attribute attribute, string caseInsensitiveName)
        {
            return GetAttributeColumn(attribute, caseInsensitiveName, new ValueTypePath());
        }

        /// <MetaDataID>{7e94016b-53aa-41a7-877f-dfa5f27ce6f2}</MetaDataID>
        RDBMSMetaDataRepository.Column GetAttributeColumn(OOAdvantech.MetaDataRepository.Attribute attribute, string path, MetaDataRepository.ValueTypePath pathIdentity)
        {
            if (!string.IsNullOrEmpty(path))
                path += "_";

            if (!IsPersistentValueType)
                return null;
            pathIdentity.Push(Identity);
            try
            {
                foreach (Attribute structureAttribute in Type.GetAttributes(true))
                {
                    if (structureAttribute.Identity == attribute.Identity)
                    {
                        Column column = new Column(path + attribute.CaseInsensitiveName, attribute.Type);//, 0, true, false, false, pathIdentity.ToString());
                        column.CreatorIdentity = pathIdentity.ToString();
                        return column;
                    }
                }
                foreach (Attribute structureAttribute in Type.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType)
                    {
                        Column column = structureAttribute.GetAttributeColumn(attribute, path + attribute.CaseInsensitiveName, pathIdentity);
                        if (column != null)
                            return column;
                    }
                }
            }
            finally
            {
                pathIdentity.Pop();
            }
            return null;
        }


        /// <MetaDataID>{289bbd05-a8eb-4af9-8cb5-3c4920b47568}</MetaDataID>
        public void RemoveColumn(Column column)
        {

            using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                if (MappedColumns.Contains(column))
                {
                    MappedColumns.Remove(column);

                    if (column.Namespace is Table)
                        (column.Namespace as Table).RemoveColumn(column);
                }
                StorageCellsColumns.Clear();
                stateTransition.Consistent = true;
            }


        }


    }
}
