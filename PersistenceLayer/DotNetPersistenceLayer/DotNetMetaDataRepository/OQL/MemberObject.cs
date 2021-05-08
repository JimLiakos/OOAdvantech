namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{


    /// <MetaDataID>{D95A3A49-F8D8-43F8-8995-A5CC3689686E}</MetaDataID>
    [System.Serializable]
    public class MemberObject : Member
    {
        //public override void LoadRelatedObjects()
        //{
        //    foreach (System.Collections.DictionaryEntry entry in LocalMemoryInstances)
        //    {
        //        GetRelatedObjects(entry.Value as RecordObject);

        //    }
        //}
        /// <MetaDataID>{5661dc76-88f7-439e-9079-e987ad070fe3}</MetaDataID>
        Collections.Generic.Dictionary<object,object> MemoryInstances = new Collections.Generic.Dictionary<object, object>();
        /// <MetaDataID>{4a1dd727-ee82-4b90-96fe-61de6a4c7034}</MetaDataID>
        Collections.Generic.Dictionary<object, object> LocalMemoryInstances = new Collections.Generic.Dictionary<object, object>();
        /// <MetaDataID>{0FE44344-4413-4627-8D83-7256D6665ACA}</MetaDataID>
        public MemberObject(DataNode dataNode, MemberList owner)
            : base(dataNode)
        {

            MemberMedata = dataNode;
            ValueTypePathDiscription = dataNode.ValueTypePathDiscription;

            //string firstColumnName=dataNode.DataSource.SelectListColumnsNames[0] as string;
            //ID = (short)owner.Table.Columns.IndexOf(firstColumnName);
            //if(ID==-1)
            //    ID = (short)owner.Table.Columns.IndexOf(firstColumnName.Replace("[","").Replace("]",""));

            Owner = owner;
            if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                _Name = dataNode.Alias;
            else
                _Name = dataNode.Name;
            DataPath = new DataPath();
            bool hasLockRequest = HasLockRequest;
            //LoadStorageInstances();
        }



        /// <MetaDataID>{c9ea7a9a-52f9-42ca-ac42-19b0b0817df8}</MetaDataID>
        bool MemberValueIsStructureSet;
        /// <MetaDataID>{cfc99b5f-dbda-48ae-a141-728c62332b69}</MetaDataID>
        public MemberObject(DataNode dataNode, MemberList owner, DataPath dataPath, bool memberValueIsStructureSet)
            : base(dataNode)
        {
            MemberValueIsStructureSet = memberValueIsStructureSet;
            DataPath = dataPath;


            MemberMedata = dataNode;

            //string firstColumnName=dataNode.DataSource.SelectListColumnsNames[0] as string;
            //ID = (short)owner.Table.Columns.IndexOf(firstColumnName);
            //if(ID==-1)
            //    ID = (short)owner.Table.Columns.IndexOf(firstColumnName.Replace("[","").Replace("]",""));

            Owner = owner;
            if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                _Name = dataNode.Alias;
            else
                _Name = dataNode.Name;
            bool hasLockRequest = HasLockRequest;
            //LoadStorageInstances();
        }




        /// <MetaDataID>{32910C41-59E7-45F3-9531-ACDE43AFE335}</MetaDataID>
        void GetIDS(System.Array StorageCellIDByteStream, ref  string StorageIdentityStr, ref int StorageCellID)
        {
            short i = 0;
            string StorageCellIDStr = null;
            string byteStr = null;

            foreach (byte Currbyte in StorageCellIDByteStream)
            {
                if (i < 16)
                {

                    System.Convert.ToString(12, 16);

                    byteStr = System.Convert.ToString(Currbyte, 16);
                    if (byteStr.Length == 1)
                        StorageIdentityStr += "0" + byteStr;
                    else
                        StorageIdentityStr += byteStr;
                }
                else
                {
                    byteStr = System.Convert.ToString(Currbyte, 16);
                    if (byteStr.Length == 1)
                        StorageCellIDStr += "0" + byteStr;
                    else
                        StorageCellIDStr += byteStr;
                }
                i++;
            }
            StorageIdentityStr = StorageIdentityStr.Insert(8, "-");
            StorageIdentityStr = StorageIdentityStr.Insert(13, "-");
            StorageIdentityStr = StorageIdentityStr.Insert(18, "-");
            StorageIdentityStr = StorageIdentityStr.Insert(23, "-");
            StorageCellID = System.Convert.ToInt32(StorageCellIDStr, 16);
            StorageIdentityStr = StorageIdentityStr.ToLower();


        }
        /// <MetaDataID>{01BC4CDE-5F31-47F7-B0C4-1F859C08E579}</MetaDataID>
        public class RecordObject
        {
            public object MemoryInstance;
            public IDataRow Row;
            /// <MetaDataID>{A06586FF-813F-402C-80D3-636A0B57ED6A}</MetaDataID>
            public RecordObject(object memoryInstance, IDataRow row)
            {
                MemoryInstance = memoryInstance;
                Row = row;
            }
        }

        /// <MetaDataID>{1EB04645-24C2-4A34-A1D4-DC14E0AE21BB}</MetaDataID>
        public class ObjectWithReletions
        {
            /// <MetaDataID>{5CBDAE8B-5EB8-4BC9-83F1-88371CF23672}</MetaDataID>
            public ObjectWithReletions(object memoryInstance, Collections.Generic.Dictionary<object, object>  objectReletions, object roleA, object roleB)
            {
                MemoryInstance = memoryInstance;
                ObjectReletions = objectReletions;
                RoleA = roleA;
                RoleB = roleB;
            }
            public Collections.Generic.Dictionary<object,object> ObjectReletions;
            public object RoleA;
            public object RoleB;
            public object MemoryInstance;
        }

        ///// <MetaDataID>{28B41FC1-457A-4AB0-A943-CC3D8C658217}</MetaDataID>
        //System.Collections.Generic.Dictionary<StorageCell, System.Collections.Generic.List<object>> InstancesStorageCell = new System.Collections.Generic.Dictionary<StorageCell, System.Collections.Generic.List<object>>();

        ///// <MetaDataID>{F8AFDC19-B6D2-4C62-B45E-431C4281EB5E}</MetaDataID>
        //StorageCell GetStorageCell(int TypeID)
        //{

        //    OQLStatement mOQLStatement=((MemberList)Owner).OQLStatement as OQLStatement;
        //    string MetadataQuery ="SELECT Class FROM "+typeof(RDBMSMetaDataRepository.Class).FullName+" Class ";
        //    Collections.StructureSet aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject( mOQLStatement.ObjectStorage.StorageMetaData).Execute(MetadataQuery  );
        //    foreach( Collections.StructureSet Rowset  in aStructureSet)
        //    {
        //        RDBMSMetaDataRepository.Class mClass=(RDBMSMetaDataRepository.Class)Rowset.Members["Class"].Value;
        //        if(mClass.Persistent)
        //        {
        //            if(mClass.TypeID==TypeID)
        //            {
        //                object TemplateObject=ModulePublisher.ClassRepository.CreateInstance(mClass.FullName,"");
        //                if(TemplateObject==null)
        //                    throw new System.Exception("PersistencyService can't instadiate the "+mClass.FullName);
        //                return new StorageCell(TemplateObject.GetType(),mOQLStatement.ObjectStorage as ObjectStorage,mClass);
        //            }
        //        }
        //    }
        //    throw new System.Exception("Can't find StorageCell");
        //}
        /// <MetaDataID>{BC930069-32AA-45F6-AC94-417CCCA980BE}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{549A8612-E958-4DD6-BDDA-0D719E730562}</MetaDataID>
        public override object Value
        {
            get
			{ 
				IDataRow StorageInstance=((MemberList)Owner).DataRecord;
                DataNode rootDataNode = ((MemberList)Owner).RootDataNode;
                if (!MemberValueIsStructureSet)
                {
                    if (DataPath.Count > 0)
                    {
                        foreach (DataNode relatedDataNode in DataPath)
                        {
                            System.Collections.Generic.ICollection<IDataRow> dataRow = rootDataNode.DataSource.GetRelatedRows(StorageInstance, relatedDataNode);
                            if (dataRow.Count == 0)
                                return null;
                            else
                            {
                                foreach (IDataRow row in dataRow)
                                {
                                    StorageInstance = row;
                                    break;
                                }
                            }
                            rootDataNode = relatedDataNode;

                            //if (StorageInstance.Table.ChildRelations.Contains(dataPath))
                            //{
                            //    System.Data.DataRow[] dataRow = StorageInstance.GetChildRows(dataPath);
                            //    if (dataRow.Length == 0)
                            //        return null;
                            //    StorageInstance = dataRow[0];
                            //}
                            //else if (StorageInstance.Table.ParentRelations.Contains(dataPath))
                            //    StorageInstance = StorageInstance.GetParentRow(dataPath);
                            
                        }
                        return StorageInstance[rootDataNode.DataSource.ObjectIndex];
                    }
                    else
                        return StorageInstance[ValueTypePathDiscription+ "Object"];
                }
                else 
                {
                    System.Collections.Generic.ICollection<IDataRow> storageInstances = new System.Collections.Generic.List<IDataRow>();
                    storageInstances.Add(StorageInstance);

                    foreach (DataNode relatedDataNode in DataPath)
                    {
                        System.Collections.Generic.ICollection<IDataRow> relatedStorageInstances = rootDataNode.DataSource.GetRelatedRows(storageInstances, relatedDataNode);

                        //    new System.Collections.Generic.List<System.Data.DataRow>();
                        //foreach (System.Data.DataRow dataRow in storageInstances)
                        //{

                        //    if (dataRow.Table.ChildRelations.Contains(dataPath))
                        //    {
                        //        System.Data.DataRow[] dataRows = dataRow.GetChildRows(dataPath);
                        //        if (dataRows.Length == 0)
                        //            return new System.Collections.ArrayList();
                        //        else
                        //            relatedStorageInstances.AddRange(dataRows);

                        //    }
                        //    else if (dataRow.Table.ParentRelations.Contains(dataPath))
                        //        relatedStorageInstances.AddRange(dataRow.GetParentRows(dataPath));
                        //}
                        storageInstances = relatedStorageInstances;
                        rootDataNode = relatedDataNode;

                    }
                    int rowRemoveIndex = -1;

                    #region RowRemove code

                    //if (MemberMedata.Type == DataNode.DataNodeType.Object)
                    //    rowRemoveIndex =MemberMedata.DataSource.RowRemoveIndex;
                    
                    #endregion

                    System.Collections.Generic.List<object> values= new System.Collections.Generic.List<object>();
                    if (MemberMedata.Type == DataNode.DataNodeType.Count)
                    {
                        int count = 0;
                        foreach (IDataRow dataRow in storageInstances)
                        {

                            //if (MemberMedata.SearchCondition != null && !MemberMedata.SearchCondition.IsRemovedRow(dataRow, rowRemoveIndex))
                            count++;
                        }
                        return count;
                    }
                    else
                    {
                        foreach (IDataRow dataRow in storageInstances)
                        {
                            //if (MemberMedata.SearchCondition!=null && !MemberMedata.SearchCondition.IsRemovedRow(dataRow, rowRemoveIndex))
                            values.Add(dataRow["Object"]);
                        }
                    }
                    if (values == null || values.Count == 0)
                        return null;
                    return values[0];
                }

                return StorageInstance["Object"];

                string ObjectIDFieldName = "ObjectID" + MemberMedata.GetHashCode();
                object fieldValue = StorageInstance[ObjectIDFieldName];
                if (fieldValue is System.DBNull)
                    return null;
                if (fieldValue == null)
                    return null;
 
                System.Guid objectID = (System.Guid)fieldValue;
                if (!MemoryInstances.ContainsKey(objectID))
                    throw new System.Exception("Error on data retrieve.");
                return (MemoryInstances[objectID] as RecordObject).MemoryInstance;

                //ObjectID mObjectID=new ObjectID();
				
                //foreach(RDBMSMetaDataRepository.IdentityColumn column in MemberMedata.Classifier.ObjectIDColumns)
                //{
                //    object FieldValue=StorageInstance[column.Name+MemberMedata.GetHashCode().ToString()];
                //    if(FieldValue is System.DBNull)
                //        return null;
                //    if(FieldValue==null)
                //        return null;
                //    mObjectID.SetMemberValue(column.ColumnType,FieldValue);
                //}


                //int ObjCellID = (int)StorageInstance["StorageCellHashCode" + MemberMedata.GetHashCode().ToString()]; ;
                //RDBMSMetaDataRepository.StorageCell storageCell = (MemberMedata.ObjectQuery as OQLStatement).StorageCells[ObjCellID];
                

                ////mObjectID.ObjCellID=ObjCellID;

                //if(CashingObjectID!=null)
                //    if(mObjectID.IntObjID==CashingObjectID.IntObjID)
                //        return _Value;

                //OQLStatement mOQLStatement=((MemberList)Owner).OQLStatement as OQLStatement;
                //StorageCell mStorageCell = null;
                //if (InstancesStorageCell.ContainsKey(storageCell))
                //    mStorageCell = InstancesStorageCell[storageCell] as StorageCell;
                //{
                //}
				

   

                //if (mStorageCell!=null&& mStorageCell.MemoryInstances != null && mStorageCell.MemoryInstances.Contains(mObjectID.IntObjID))
                //{
                //    _Value = mStorageCell.MemoryInstances[mObjectID.IntObjID];
                //    CashingObjectID = mObjectID;
                //    return _Value;
                //}
                //StorageInstanceRef mStorageInstanceRef = ((MemberMedata.ObjectQuery as OQLStatement).ObjectStorage as PersistenceLayerRunTime.ObjectStorage).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(storageCell.Type.FullName, "")][mObjectID] as StorageInstanceRef;

				
                //if(mStorageInstanceRef!=null)
                //{
                //    _Value=mStorageInstanceRef.MemoryInstance;
                //    CashingObjectID=mObjectID;
                //    return mStorageInstanceRef.MemoryInstance;
                //}

                //mStorageInstanceRef=(StorageInstanceRef)((MemberMedata.ObjectQuery as OQLStatement).ObjectStorage as ObjectStorage).CreateStorageInstanceRef(ModulePublisher.ClassRepository.CreateInstance(storageCell.Type.FullName,""),mObjectID);
                //mStorageInstanceRef.StorageInstanceSet = storageCell; //Error prone αν κάτι γινεί απο εδώ και κάτω τότε θα έχω cashing ένα object ληψό
                ////mStorageInstanceRef.ObjectID=mObjectID;
                //mStorageInstanceRef.DbDataRecord=StorageInstance;
				

                //if(mStorageInstanceRef.Class.ClassHierarchyLinkAssociation!=null)
                //{
                //    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                //    object RoleA=null;
                //    object RoleB=null;
                //    GetRoleObjectsFromRelationObject(ref RoleA,ref RoleB);
					
                //    System.Reflection.FieldInfo  FieldRoleA=(mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                //    System.Reflection.FieldInfo  FieldRoleB=(mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleBField;;
                ////	mStorageInstanceRef.GetAssociationClassRolesFields(ref FieldRoleA,ref FieldRoleB);
                //    FieldRoleA.SetValue(mStorageInstanceRef.MemoryInstance,RoleA);
                //    FieldRoleB.SetValue(mStorageInstanceRef.MemoryInstance,RoleB);

                //}
                //GetRelatedObjects(mStorageInstanceRef);
                //mStorageInstanceRef.LoadObjectState(MemberMedata.GetHashCode().ToString());
                //mStorageInstanceRef.SnapshotStorageInstance();
                //_Value=mStorageInstanceRef.MemoryInstance;
                //CashingObjectID=mObjectID;
                //return mStorageInstanceRef.MemoryInstance;
			}
            set
            {
            }
        }
        //void AssignRelatedObjects()
        //{
        //    foreach (System.Collections.DictionaryEntry CurrDictionaryEntry in InstancesStorageCell)
        //    {
        //        StorageCell storageCell = CurrDictionaryEntry.Value as StorageCell;
        //        if (storageCell.ObjectsRelations.Count > 0)
        //            storageCell.AssignRelatedObjects();
        //    }


        //}
        //void GetRelatedObjects(RecordObject recordObject)
        //{
        //    Collections.Hashtable relatedObjects = new OOAdvantech.Collections.Hashtable();


        //    MetaDataRepository.AssociationEnd DataNodeAssociationEnd = null;
        //    if (MemberMedata.AssignedMetaObject is MetaDataRepository.AssociationEnd && MemberMedata.ObjectQuery.SelectListItems.Contains(MemberMedata.ParentDataNode))
        //        DataNodeAssociationEnd = MemberMedata.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //    //TODO έαν πεσει φίλτρο στα related objects και η σχέσει δεν είναι many τότε εάν κοπεί το 
        //    //related object από το φίλτρο θα κοπεί και το orginal από την inner join σχέση.
        //    //Εάν καλείψουμε και τις on construction many σχέσεις τότε υπάρχει πρόβλημα γιατί θάπρει να φορτόσουμε
        //    //όλα τα related objects στις on construction σχέσεις και να εφαρμόσουμε το φίλτρο στα 
        //    //related objects όταν ανεβούν στην stravture set.
        //    string MemberMedataID = MemberMedata.GetHashCode().ToString();
        //    string StorageCellHashCodeFieldName = "StorageCellHashCode" + MemberMedataID;
        //    System.Data.DataRow storageInstance = recordObject.Row;
        //    int ObjCellID = (int)storageInstance[StorageCellHashCodeFieldName];
        //    //Temporary of.
        //    //RDBMSMetaDataRepository.StorageCell storageCell = (MemberMedata.ObjectQuery as OQLStatement).StorageCells[ObjCellID];
        //    RDBMSMetaDataRepository.StorageCell storageCell = null;

        //    if (DataNodeAssociationEnd != null 
        //        &&!DataNodeAssociationEnd.GetOtherEnd().Multiplicity.IsMany
        //        && (MemberMedata.ParentDataNode as DataNode).StructureSetMember is MemberObject)
        //    {
        //        MemberList parentMemberList = (MemberMedata.ParentDataNode as DataNode).StructureSetMember.Owner as MemberList;
        //        parentMemberList.DataRecord = recordObject.Row.GetParentRow(Name);
        //        Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();
        //        objects.Add(parentMemberList[MemberMedata.ParentDataNode.Alias].Value);
        //        relatedObjects[DataNodeAssociationEnd.GetOtherEnd().Identity.ToString()] = objects;
        //    }
        //    foreach (DataNode subDataNode in MemberMedata.SubDataNodes)
        //    {
        //        if (!subDataNode.AutoGenerated)
        //            continue;

        //        if (MemberMedata.ObjectQuery.SelectListItems.Contains(subDataNode) && subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //        {
        //            MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //            if (!associationEnd.Multiplicity.IsMany)
        //            {
        //                PersistenceLayer.Member member = Owner[subDataNode.Alias];
        //                (Owner as MemberList).DataRecord = recordObject.Row;
        //                if (member.Value != null)
        //                {
        //                    Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();
        //                    if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
        //                    {
        //                        objects.Add(member.Value);
        //                        relatedObjects[associationEnd.Identity.ToString()] = objects;
        //                    }
        //                    else
        //                    {
        //                        StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Value) as StorageInstanceRef;
        //                        foreach (RelResolver relResolver in storageInstanceRef.RelResolvers)
        //                        {
        //                            if (relResolver.AssociationEnd.Identity == associationEnd.Identity)
        //                            {
        //                                if (storageInstanceRef.Class.HasReferentialIntegrity(relResolver.AssociationEnd) || (relResolver.AssociationEnd.Navigable && relResolver.AssociationEnd.GetOtherEnd().Navigable))
        //                                {
        //                                    storageInstanceRef.Class.GetFieldMember(relResolver.AssociationEnd).SetValue(storageInstanceRef.MemoryInstance, member.Value);
        //                                }
        //                                else
        //                                {
        //                                    StorageInstanceRef relatedStorageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(member.Value) as StorageInstanceRef;
        //                                    relatedStorageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(relResolver.OnObjectDeleted);
        //                                    storageInstanceRef.Class.GetFieldMember(relResolver.AssociationEnd).SetValue(storageInstanceRef.MemoryInstance, member.Value);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();

        //                MemberCollection memberCollection = Owner.Members[subDataNode.Alias] as MemberCollection;
        //                foreach(System.Data.DataRow childRow in recordObject.Row.GetChildRows(Name))
        //                {
        //                    memberCollection.Members.DataRecord = childRow;
        //                    PersistenceLayer.Member member = memberCollection.Members[subDataNode.Alias];
        //                    if(member.Value!=null)
        //                        objects.Add(member.Value);
        //                }

        //                if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
        //                    relatedObjects[associationEnd.Identity.ToString()] = objects;
        //                else
        //                {
        //                    StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Value) as StorageInstanceRef;
        //                    foreach (RelResolver relResolver in storageInstanceRef.RelResolvers)
        //                    {
        //                        if (relResolver.AssociationEnd == associationEnd)
        //                        {
        //                            //TODO 
        //                            relResolver.LoadRelatedObjects(objects,true); 
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    if (relatedObjects.Count > 0)
        //    {
        //        if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
        //        {
        //            StorageCell remoteObjectLoader = GetStorageCell(storageCell as RDBMSMetaDataRepository.StorageCellReference);
        //            ObjectWithReletions objectWithReletions=new ObjectWithReletions(Value,relatedObjects,null,null);
        //            remoteObjectLoader.ObjectsRelations.Add(objectWithReletions); 
        //        }
        //    }
        //}

        ///// <MetaDataID>{29E3C2EE-761D-4E28-90CB-C204885A9338}</MetaDataID>
        //void GetRelatedObjects(PersistenceLayer.StorageInstanceRef StorageInstance)
        //{
        //    MetaDataRepository.AssociationEnd DataNodeAssociationEnd=null;
        //    if(MemberMedata.AssignedMetaObject is MetaDataRepository.AssociationEnd&&MemberMedata.ObjectQuery.SelectListItems.Contains(MemberMedata.ParentDataNode))
        //        DataNodeAssociationEnd=MemberMedata.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //    //TODO έαν πεσει φίλτρο στα related objects και η σχέσει δεν είναι many τότε εάν κοπεί το 
        //    //related object από το φίλτρο θα κοπεί και το orginal από την inner join σχέση.
        //    //Εάν καλείψουμε και τις on construction many σχέσεις τότε υπάρχει πρόβλημα γιατί θάπρει να φορτόσουμε
        //    //όλα τα related objects στις on construction σχέσεις και να εφαρμόσουμε το φίλτρο στα 
        //    //related objects όταν ανεβούν στην stravture set.
        //    foreach(RelResolver relResolver in StorageInstance.RelResolvers)
        //    {
        //        PersistenceLayerRunTime.RelResolver CurrRelResolver=relResolver;
        //        if(DataNodeAssociationEnd!=null&&!DataNodeAssociationEnd.GetOtherEnd().Multiplicity.IsMany)
        //        {
        //            if(CurrRelResolver.AssociationEnd.Identity==DataNodeAssociationEnd.GetOtherEnd().Identity)
        //            {
        //                DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd=CurrRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;

        //                if(StorageInstance.Class.HasReferentialIntegrity(CurrRelResolver.AssociationEnd)||(CurrRelResolver.AssociationEnd.Navigable&&CurrRelResolver.AssociationEnd.GetOtherEnd().Navigable))
        //                {
        //                    StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[MemberMedata.ParentDataNode.Alias].Value);
        //                }							
        //                else
        //                {
        //                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Owner[MemberMedata.ParentDataNode.Alias].Value) as PersistenceLayerRunTime.StorageInstanceRef;
        //                    storageInstanceRef.ObjectDeleted+=new PersistenceLayerRunTime.ObjectDeleted(CurrRelResolver.OnObjectDeleted);
        //                    StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[MemberMedata.ParentDataNode.Alias].Value);
        //                }

        //            }
        //        }
        //        foreach(DataNode CurrSubDataNode in MemberMedata.SubDataNodes)
        //        {
        //            if(MemberMedata.ObjectQuery.SelectListItems.Contains(CurrSubDataNode)&&CurrSubDataNode.AssignedMetaObject is  MetaDataRepository.AssociationEnd)
        //                if(CurrRelResolver.AssociationEnd.Identity==CurrSubDataNode.AssignedMetaObject.Identity)
        //                {
        //                    DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd=CurrRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
        //                    if(!theOtherAssociationEnd.Multiplicity.IsMany)
        //                    {


        //                        //TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  
        //                        if(StorageInstance.Class.HasReferentialIntegrity(CurrRelResolver.AssociationEnd)||(CurrRelResolver.AssociationEnd.Navigable&&CurrRelResolver.AssociationEnd.GetOtherEnd().Navigable))
        //                        {
        //                            StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[CurrSubDataNode.Alias].Value);							
        //                        }							
        //                        else
        //                        {
        //                            PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef =StorageInstanceRef.GetStorageInstanceRef(Owner[MemberMedata.ParentDataNode.Alias].Value) as PersistenceLayerRunTime.StorageInstanceRef;
        //                            storageInstanceRef.ObjectDeleted+=new PersistenceLayerRunTime.ObjectDeleted(CurrRelResolver.OnObjectDeleted);
        //                            StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[CurrSubDataNode.Alias].Value);							
        //                        }
        //                    }
        //                }
        //        }
        //    }
        //}
        ///// <MetaDataID>{5A146D6A-D2B8-4A84-B2B8-CCB13005DB3B}</MetaDataID>
        //void GetRoleObjectsFromRelationObject(ref object RoleA, ref object RoleB, Collections.StructureSet structureSet)
        //{
        //    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
        //    DataNode RoleADataNode=null;
        //    DataNode RoleBDataNode=null;
        //    foreach(DataNode CurrSelectListItem in  MemberMedata.ObjectQuery.SelectListItems)
        //    {
        //        if(CurrSelectListItem.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
        //        {
        //            if((MemberMedata.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity==CurrSelectListItem.AssignedMetaObject.Identity)
        //                if((CurrSelectListItem.Classifier as MetaDataRepository.Classifier).Identity!=(MemberMedata.Classifier as MetaDataRepository.Classifier).Identity)
        //                    RoleADataNode=CurrSelectListItem;
        //                else
        //                    RoleBDataNode=CurrSelectListItem.ParentDataNode as DataNode;
        //            if((MemberMedata.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity==CurrSelectListItem.AssignedMetaObject.Identity)
        //            {
        //                if((CurrSelectListItem.Classifier as MetaDataRepository.Classifier).Identity!=(MemberMedata.Classifier as MetaDataRepository.Classifier).Identity)
        //                    RoleBDataNode=CurrSelectListItem;
        //                else
        //                    RoleADataNode=CurrSelectListItem.ParentDataNode as DataNode;
        //            }
        //        }

        //    }
        //    RoleA = structureSet[RoleADataNode.Alias];
        //    RoleB = structureSet[RoleBDataNode.Alias];

        //}


    }
}
