namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

	/// <MetaDataID>{D95A3A49-F8D8-43F8-8995-A5CC3689686E}</MetaDataID>
	public class MemberObject : Member
	{
        public override void LoadRelatedObjects()
        {
            foreach (System.Collections.DictionaryEntry entry in LocalMemoryInstances)
            {
                GetRelatedObjects(entry.Value as RecordObject);

            }
        }
		/// <MetaDataID>{593A9EDE-1164-41BD-B162-E93B39487F0E}</MetaDataID>
		void LoadStorageInstances()
		{
			System.Data.DataTable dataTable=(Owner as MemberList).RootDataNode.DataSource.DataTable;
			string MemberMedataID=MemberMedata.GetHashCode().ToString();
			
			OQLStatement mOQLStatement=((MemberList)Owner).OQLStatement as OQLStatement;

            string ObjectIDFieldName = "ObjectID" + MemberMedataID;
            string StorageCellHashCodeFieldName = "StorageCellHashCode" + MemberMedataID;
			
            foreach (System.Data.DataRow StorageInstance in dataTable.Rows)
            {
                object FieldValue = StorageInstance[ObjectIDFieldName];
                if (FieldValue is System.DBNull)
                    continue;
                if (FieldValue == null)
                    continue;
                System.Guid objectID = (System.Guid)FieldValue;
                int ObjCellID = (int)StorageInstance[StorageCellHashCodeFieldName];
                //Temporary of.
                //RDBMSMetaDataRepository.StorageCell storageCell = (MemberMedata.ObjectQuery as OQLStatement).StorageCells[ObjCellID];
                RDBMSMetaDataRepository.StorageCell storageCell = null;
                if (!(storageCell is RDBMSMetaDataRepository.StorageCellReference))
                {
                    object _object;

                    StorageInstanceRef storageInstanceRef = ((MemberMedata.ObjectQuery as OQLStatement).ObjectStorage as PersistenceLayerRunTime.ObjectStorage).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(storageCell.Type.FullName, "")][objectID] as StorageInstanceRef;
                    if (storageInstanceRef != null)
                    {
                        _object = storageInstanceRef.MemoryInstance;
                    }
                    else
                    {
                        storageInstanceRef = (StorageInstanceRef)((MemberMedata.ObjectQuery as OQLStatement).ObjectStorage as ObjectStorage).CreateStorageInstanceRef(ModulePublisher.ClassRepository.CreateInstance(storageCell.Type.FullName, ""), objectID);
                        storageInstanceRef.StorageInstanceSet = storageCell;
                        storageInstanceRef.DbDataRecord = StorageInstance;
                        //if (storageInstanceRef.Class.ClassHierarchyLinkAssociation != null)
                        //{
                        //    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                        //    object RoleA = null;
                        //    object RoleB = null;
                        //    GetRoleObjectsFromRelationObject(ref RoleA, ref RoleB);
                        //    System.Reflection.FieldInfo FieldRoleA = (mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                        //    System.Reflection.FieldInfo FieldRoleB = (mStorageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleBField; ;
                        //    FieldRoleA.SetValue(storageInstanceRef.MemoryInstance, RoleA);
                        //    FieldRoleB.SetValue(storageInstanceRef.MemoryInstance, RoleB);
                        //}
                        //GetRelatedObjects(mStorageInstanceRef);
                        storageInstanceRef.LoadObjectState(MemberMedata.GetHashCode().ToString());
                        storageInstanceRef.SnapshotStorageInstance();
                        _object = storageInstanceRef.MemoryInstance;

                    }

                    MemoryInstances[objectID] = new RecordObject(_object, StorageInstance);
                }
                else
                {
                    int outStorageCellIDColumnID = -1;

                    RDBMSMetaDataRepository.StorageCellReference outStorageCell = storageCell as RDBMSMetaDataRepository.StorageCellReference;
                    {
                        string memberMedataID=MemberMedata.GetHashCode().ToString();
                        StorageCell remoteObjectLoader = GetStorageCell(outStorageCell);
                        if (remoteObjectLoader.InstancesData.Columns.Count == 0)
                        {

                            //Temporary of.
                            //foreach (string selceColumnName in MemberMedata.DataSource.SelectColumnsNames)
                            //{
                            //    System.Type ColumnDataType = dataTable.Columns[selceColumnName + memberMedataID].DataType;
                            //    //string ColumnName = dataTable.Columns[i].ColumnName;
                            //    //ColumnName = ColumnName.Replace(MemberMedataID, "");
                            //    if (selceColumnName == "StorageCellHashCode")
                            //    {
                            //        //outStorageCellIDColumnID = i;
                            //        continue;
                            //    }
                            //    remoteObjectLoader.InstancesData.Columns.Add(new System.Data.DataColumn(selceColumnName, ColumnDataType));
                            //}
                        }
                        int k = 0;
                        System.Data.DataRow DestinationDataRow = remoteObjectLoader.InstancesData.NewRow();
                        //Temporary of.
                        //foreach (string columnName in MemberMedata.DataSource.SelectColumnsNames)
                        //{
                        //    if (columnName == "StorageCellHashCode")
                        //        continue;
                        //    DestinationDataRow[columnName] = StorageInstance[columnName+memberMedataID];
                        //    k++;
                        //}
                        MemoryInstances[objectID] = new RecordObject(null, StorageInstance);
                        remoteObjectLoader.InstancesData.Rows.Add(DestinationDataRow);
                    }
                }
            }
            LocalMemoryInstances=MemoryInstances.Clone() as Collections.Hashtable;
            foreach (System.Collections.DictionaryEntry CurrDictionaryEntry in InstancesStorageCell)
            {
                StorageCell CurrStorageCell = CurrDictionaryEntry.Value as StorageCell;
                CurrStorageCell.LoadObjects();
                foreach (System.Collections.DictionaryEntry entry in CurrStorageCell.MemoryInstances)
                {
                    (MemoryInstances[entry.Key] as RecordObject).MemoryInstance = entry.Value;
                }
            }
		}
        Collections.Hashtable MemoryInstances = new OOAdvantech.Collections.Hashtable();
        Collections.Hashtable LocalMemoryInstances = new OOAdvantech.Collections.Hashtable();
		/// <MetaDataID>{0FE44344-4413-4627-8D83-7256D6665ACA}</MetaDataID>
		public MemberObject(DataNode dataNode,MemberList owner)
		{
            
			MemberMedata=dataNode;
            dataNode.StructureSetMember = this;
            //string firstColumnName=dataNode.DataSource.SelectListColumnsNames[0] as string;
            //ID = (short)owner.Table.Columns.IndexOf(firstColumnName);
            //if(ID==-1)
            //    ID = (short)owner.Table.Columns.IndexOf(firstColumnName.Replace("[","").Replace("]",""));

			Owner=owner;
            if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                _Name = dataNode.Alias;
            else
                _Name = dataNode.Name;
			LoadStorageInstances();
		}
		/// <MetaDataID>{32910C41-59E7-45F3-9531-ACDE43AFE335}</MetaDataID>
		void GetIDS(System.Array StorageCellIDByteStream,ref  string StorageIdentityStr,ref int StorageCellID)
		{
			short i=0;
			string StorageCellIDStr=null ;
			string byteStr=null;
			
			foreach(byte Currbyte in StorageCellIDByteStream)
			{
				if(i<16)
				{
					
					System.Convert.ToString(12,16);
					
					byteStr=System.Convert.ToString(Currbyte,16);
					if(byteStr.Length==1)
						StorageIdentityStr+="0"+byteStr;
					else
						StorageIdentityStr+=byteStr;
				}
				else
				{
					byteStr= System.Convert.ToString(Currbyte,16);
					if(byteStr.Length==1)
						StorageCellIDStr+="0"+byteStr;
					else
						StorageCellIDStr+=byteStr;
				}
				i++;
			}
			StorageIdentityStr=StorageIdentityStr.Insert(8,"-");
			StorageIdentityStr=StorageIdentityStr.Insert(13,"-");
			StorageIdentityStr=StorageIdentityStr.Insert(18,"-");
			StorageIdentityStr=StorageIdentityStr.Insert(23,"-");
			StorageCellID=System.Convert.ToInt32(StorageCellIDStr,16);
			StorageIdentityStr=StorageIdentityStr.ToLower();


		}
        public class RecordObject
        {
            public object MemoryInstance;
            public System.Data.DataRow Row;
            public RecordObject(object memoryInstance, System.Data.DataRow row)
            {
                MemoryInstance = memoryInstance;
                Row = row;
            }
        }

        public class ObjectWithReletions
        {
            public ObjectWithReletions(object memoryInstance, Collections.Hashtable objectReletions, object roleA, object roleB)
            {
                MemoryInstance = memoryInstance;
                ObjectReletions = objectReletions;
                RoleA = roleA;
                RoleB = roleB;
            }
            public Collections.Hashtable ObjectReletions;
            public object RoleA;
            public object RoleB;
            public object MemoryInstance;
        }

		
		/// <MetaDataID>{EDBC6E1F-3B49-434F-93C9-C5005F6EF493}</MetaDataID>
		class StorageCell
		{
            internal OOAdvantech.Collections.ArrayList ObjectsRelations = new OOAdvantech.Collections.ArrayList(); 

			/// <MetaDataID>{9898C73F-D647-4EF1-B1D8-EBECAEF71A90}</MetaDataID>
			public StorageCell(System.Type objectsType,ObjectStorage objectStorageSession,RDBMSMetaDataRepository.Class mClass)
			{
				ObjectsType=objectsType;
				ObjectStorageSession=objectStorageSession;
				Class=mClass;
			}
			/// <MetaDataID>{75A1B56B-7E6D-4B21-932C-5C3300F89DA8}</MetaDataID>
			public object GetObject(ObjectID objectID,System.Data.DataRow dataRow)
			{
				return null;

			}
			/// <MetaDataID>{DE66DD7E-F819-4464-B7A6-0E5E65917AFA}</MetaDataID>
			public System.Type ObjectsType=null;
			/// <MetaDataID>{670737C0-E1B5-4A1F-9846-80D48D0D4560}</MetaDataID>
			public ObjectStorage ObjectStorageSession=null;
			/// <MetaDataID>{DD1BABF4-9A57-496E-BDDD-A433FB81BA72}</MetaDataID>
			public RDBMSMetaDataRepository.Class Class=null;

			/// <MetaDataID>{6AB1A280-6081-42F4-AE51-2EC0D83FAE29}</MetaDataID>
			public System.Data.DataTable InstancesData=new System.Data.DataTable();
			/// <MetaDataID>{3114169F-AAE1-4886-BEC2-014372C78742}</MetaDataID>
			public System.Collections.Hashtable MemoryInstances;

			/// <MetaDataID>{2E406DCE-1EB0-4D3E-9325-13FF70C1FE0C}</MetaDataID>
			public void LoadObjects()
			{
				MemoryInstances=ObjectStorageSession.GetObjects(InstancesData,ObjectsType);
			}


            internal void AssignRelatedObjects()
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }
		/// <MetaDataID>{28B41FC1-457A-4AB0-A943-CC3D8C658217}</MetaDataID>
		System.Collections.Hashtable InstancesStorageCell=new System.Collections.Hashtable();
		/// <MetaDataID>{38E08564-190A-48F5-BAAF-9B4624A7310D}</MetaDataID>
        StorageCell GetStorageCell(RDBMSMetaDataRepository.StorageCellReference outStorageCell)
		{

            if (InstancesStorageCell.Contains(outStorageCell))
                return (StorageCell)InstancesStorageCell[outStorageCell];
            PersistenceLayerRunTime.ClientSide.ObjectStorageAgent OutStorageSession = PersistenceLayer.ObjectStorage.OpenStorage(outStorageCell.StorageName, outStorageCell.StorageLocation, "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider") as PersistenceLayerRunTime.ClientSide.ObjectStorageAgent;
            System.Type type = ModulePublisher.ClassRepository.GetType(outStorageCell.Type.FullName, "");
            if (type == null)
                throw new System.Exception("PersistencyService can't instadiate the " + outStorageCell.Type.FullName);
            StorageCell remoteObjectLoader=new StorageCell(type, OutStorageSession.ServerSideStorageSession  as ObjectStorage , outStorageCell.Type as RDBMSMetaDataRepository.Class);
            InstancesStorageCell.Add(outStorageCell, remoteObjectLoader);
            return remoteObjectLoader;
		}
		/// <MetaDataID>{F8AFDC19-B6D2-4C62-B45E-431C4281EB5E}</MetaDataID>
		StorageCell GetStorageCell(int TypeID)
		{
			
			OQLStatement mOQLStatement=((MemberList)Owner).OQLStatement as OQLStatement;
			string MetadataQuery ="SELECT Class FROM "+typeof(RDBMSMetaDataRepository.Class).FullName+" Class ";
			PersistenceLayer.StructureSet aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject( mOQLStatement.ObjectStorage.StorageMetaData).Execute(MetadataQuery  );
			foreach( PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				RDBMSMetaDataRepository.Class mClass=(RDBMSMetaDataRepository.Class)Rowset.Members["Class"].Value;
				if(mClass.Persistent)
				{
					if(mClass.TypeID==TypeID)
					{
						object TemplateObject=ModulePublisher.ClassRepository.CreateInstance(mClass.FullName,"");
						if(TemplateObject==null)
							throw new System.Exception("PersistencyService can't instadiate the "+mClass.FullName);
						return new StorageCell(TemplateObject.GetType(),mOQLStatement.ObjectStorage as ObjectStorage,mClass);
					}
				}
			}
			throw new System.Exception("Can't find StorageCell");
		}
		/// <MetaDataID>{79218406-2BF8-4A09-BCC3-FB44A9FBF9FD}</MetaDataID>
		ObjectID CashingObjectID=null;
		/// <MetaDataID>{BC930069-32AA-45F6-AC94-417CCCA980BE}</MetaDataID>
		private object _Value;
		/// <MetaDataID>{549A8612-E958-4DD6-BDDA-0D719E730562}</MetaDataID>
		public override object Value
		{
			get
			{ 
				System.Data.DataRow StorageInstance=((MemberList)Owner).DataRecord;

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
        void AssignRelatedObjects()
        {
            foreach (System.Collections.DictionaryEntry CurrDictionaryEntry in InstancesStorageCell)
            {
                StorageCell storageCell = CurrDictionaryEntry.Value as StorageCell;
                if (storageCell.ObjectsRelations.Count > 0)
                    storageCell.AssignRelatedObjects();
            }


        }
        void GetRelatedObjects(RecordObject recordObject)
        {
            Collections.Hashtable relatedObjects = new OOAdvantech.Collections.Hashtable();


            MetaDataRepository.AssociationEnd DataNodeAssociationEnd = null;
            if (MemberMedata.AssignedMetaObject is MetaDataRepository.AssociationEnd && MemberMedata.ObjectQuery.SelectListItems.Contains(MemberMedata.ParentDataNode))
                DataNodeAssociationEnd = MemberMedata.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            //TODO έαν πεσει φίλτρο στα related objects και η σχέσει δεν είναι many τότε εάν κοπεί το 
            //related object από το φίλτρο θα κοπεί και το orginal από την inner join σχέση.
            //Εάν καλείψουμε και τις on construction many σχέσεις τότε υπάρχει πρόβλημα γιατί θάπρει να φορτόσουμε
            //όλα τα related objects στις on construction σχέσεις και να εφαρμόσουμε το φίλτρο στα 
            //related objects όταν ανεβούν στην stravture set.
            string MemberMedataID = MemberMedata.GetHashCode().ToString();
            string StorageCellHashCodeFieldName = "StorageCellHashCode" + MemberMedataID;
            System.Data.DataRow storageInstance = recordObject.Row;
            int ObjCellID = (int)storageInstance[StorageCellHashCodeFieldName];
            //Temporary of.
            //RDBMSMetaDataRepository.StorageCell storageCell = (MemberMedata.ObjectQuery as OQLStatement).StorageCells[ObjCellID];
            RDBMSMetaDataRepository.StorageCell storageCell = null;

            if (DataNodeAssociationEnd != null 
                &&!DataNodeAssociationEnd.GetOtherEnd().Multiplicity.IsMany
                && (MemberMedata.ParentDataNode as DataNode).StructureSetMember is MemberObject)
            {
                MemberList parentMemberList = (MemberMedata.ParentDataNode as DataNode).StructureSetMember.Owner as MemberList;
                parentMemberList.DataRecord = recordObject.Row.GetParentRow(Name);
                Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();
                objects.Add(parentMemberList[MemberMedata.ParentDataNode.Alias].Value);
                relatedObjects[DataNodeAssociationEnd.GetOtherEnd().Identity.ToString()] = objects;
            }
            foreach (DataNode subDataNode in MemberMedata.SubDataNodes)
            {
                if (!subDataNode.AutoGenerated)
                    continue;

                if (MemberMedata.ObjectQuery.SelectListItems.Contains(subDataNode) && subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    if (!associationEnd.Multiplicity.IsMany)
                    {
                        PersistenceLayer.Member member = Owner[subDataNode.Alias];
                        (Owner as MemberList).DataRecord = recordObject.Row;
                        if (member.Value != null)
                        {
                            Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();
                            if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
                            {
                                objects.Add(member.Value);
                                relatedObjects[associationEnd.Identity.ToString()] = objects;
                            }
                            else
                            {
                                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Value) as StorageInstanceRef;
                                foreach (RelResolver relResolver in storageInstanceRef.RelResolvers)
                                {
                                    if (relResolver.AssociationEnd.Identity == associationEnd.Identity)
                                    {
                                        if (storageInstanceRef.Class.HasReferentialIntegrity(relResolver.AssociationEnd) || (relResolver.AssociationEnd.Navigable && relResolver.AssociationEnd.GetOtherEnd().Navigable))
                                        {
                                            storageInstanceRef.Class.GetFieldMember(relResolver.AssociationEnd).SetValue(storageInstanceRef.MemoryInstance, member.Value);
                                        }
                                        else
                                        {
                                            StorageInstanceRef relatedStorageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(member.Value) as StorageInstanceRef;
                                            relatedStorageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(relResolver.OnObjectDeleted);
                                            storageInstanceRef.Class.GetFieldMember(relResolver.AssociationEnd).SetValue(storageInstanceRef.MemoryInstance, member.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Collections.ArrayList objects = new OOAdvantech.Collections.ArrayList();

                        MemberCollection memberCollection = Owner.Members[subDataNode.Alias] as MemberCollection;
                        foreach(System.Data.DataRow childRow in recordObject.Row.GetChildRows(Name))
                        {
                            memberCollection.Members.DataRecord = childRow;
                            PersistenceLayer.Member member = memberCollection.Members[subDataNode.Alias];
                            if(member.Value!=null)
                                objects.Add(member.Value);
                        }

                        if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
                            relatedObjects[associationEnd.Identity.ToString()] = objects;
                        else
                        {
                            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Value) as StorageInstanceRef;
                            foreach (RelResolver relResolver in storageInstanceRef.RelResolvers)
                            {
                                if (relResolver.AssociationEnd == associationEnd)
                                {
                                    //TODO 
                                    relResolver.LoadRelatedObjects(objects,true); 
                                }
                            }

                        }
                    }
                }
            }
            if (relatedObjects.Count > 0)
            {
                if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
                {
                    StorageCell remoteObjectLoader = GetStorageCell(storageCell as RDBMSMetaDataRepository.StorageCellReference);
                    ObjectWithReletions objectWithReletions=new ObjectWithReletions(Value,relatedObjects,null,null);
                    remoteObjectLoader.ObjectsRelations.Add(objectWithReletions); 
                }
            }
        }

		/// <MetaDataID>{29E3C2EE-761D-4E28-90CB-C204885A9338}</MetaDataID>
		void GetRelatedObjects(StorageInstanceRef StorageInstance)
		{
			MetaDataRepository.AssociationEnd DataNodeAssociationEnd=null;
			if(MemberMedata.AssignedMetaObject is MetaDataRepository.AssociationEnd&&MemberMedata.ObjectQuery.SelectListItems.Contains(MemberMedata.ParentDataNode))
				DataNodeAssociationEnd=MemberMedata.AssignedMetaObject as MetaDataRepository.AssociationEnd;
			//TODO έαν πεσει φίλτρο στα related objects και η σχέσει δεν είναι many τότε εάν κοπεί το 
            //related object από το φίλτρο θα κοπεί και το orginal από την inner join σχέση.
            //Εάν καλείψουμε και τις on construction many σχέσεις τότε υπάρχει πρόβλημα γιατί θάπρει να φορτόσουμε
            //όλα τα related objects στις on construction σχέσεις και να εφαρμόσουμε το φίλτρο στα 
            //related objects όταν ανεβούν στην stravture set.
			foreach(RelResolver relResolver in StorageInstance.RelResolvers)
			{
				PersistenceLayerRunTime.RelResolver CurrRelResolver=relResolver;
				if(DataNodeAssociationEnd!=null&&!DataNodeAssociationEnd.GetOtherEnd().Multiplicity.IsMany)
				{
					if(CurrRelResolver.AssociationEnd.Identity==DataNodeAssociationEnd.GetOtherEnd().Identity)
					{
						DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd=CurrRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
					
						if(StorageInstance.Class.HasReferentialIntegrity(CurrRelResolver.AssociationEnd)||(CurrRelResolver.AssociationEnd.Navigable&&CurrRelResolver.AssociationEnd.GetOtherEnd().Navigable))
						{
							StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[MemberMedata.ParentDataNode.Alias].Value);
						}							
						else
						{
							PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(Owner[MemberMedata.ParentDataNode.Alias].Value) as PersistenceLayerRunTime.StorageInstanceRef;
							storageInstanceRef.ObjectDeleted+=new PersistenceLayerRunTime.ObjectDeleted(CurrRelResolver.OnObjectDeleted);
							StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[MemberMedata.ParentDataNode.Alias].Value);
						}

					}
				}
				foreach(DataNode CurrSubDataNode in MemberMedata.SubDataNodes)
				{
					if(MemberMedata.ObjectQuery.SelectListItems.Contains(CurrSubDataNode)&&CurrSubDataNode.AssignedMetaObject is  MetaDataRepository.AssociationEnd)
						if(CurrRelResolver.AssociationEnd.Identity==CurrSubDataNode.AssignedMetaObject.Identity)
						{
							DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd=CurrRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
							if(!theOtherAssociationEnd.Multiplicity.IsMany)
							{


								//TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  
								if(StorageInstance.Class.HasReferentialIntegrity(CurrRelResolver.AssociationEnd)||(CurrRelResolver.AssociationEnd.Navigable&&CurrRelResolver.AssociationEnd.GetOtherEnd().Navigable))
								{
									StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[CurrSubDataNode.Alias].Value);							
								}							
								else
								{
									PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef =StorageInstanceRef.GetStorageInstanceRef(Owner[MemberMedata.ParentDataNode.Alias].Value) as PersistenceLayerRunTime.StorageInstanceRef;
									storageInstanceRef.ObjectDeleted+=new PersistenceLayerRunTime.ObjectDeleted(CurrRelResolver.OnObjectDeleted);
									StorageInstance.Class.GetFieldMember(theOtherAssociationEnd).SetValue(StorageInstance.MemoryInstance,Owner[CurrSubDataNode.Alias].Value);							
								}
							}
						}
				}
			}
		}
		/// <MetaDataID>{5A146D6A-D2B8-4A84-B2B8-CCB13005DB3B}</MetaDataID>
        void GetRoleObjectsFromRelationObject(ref object RoleA, ref object RoleB, PersistenceLayer.StructureSet structureSet)
		{
			//TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
			DataNode RoleADataNode=null;
			DataNode RoleBDataNode=null;
			foreach(DataNode CurrSelectListItem in  MemberMedata.ObjectQuery.SelectListItems)
			{
				if(CurrSelectListItem.AssignedMetaObject is RDBMSMetaDataRepository.AssociationEnd)
				{
					if((MemberMedata.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity==CurrSelectListItem.AssignedMetaObject.Identity)
						if((CurrSelectListItem.Classifier as MetaDataRepository.Classifier).Identity!=(MemberMedata.Classifier as MetaDataRepository.Classifier).Identity)
                            RoleADataNode=CurrSelectListItem;
						else
							RoleBDataNode=CurrSelectListItem.ParentDataNode as DataNode;
					if((MemberMedata.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity==CurrSelectListItem.AssignedMetaObject.Identity)
					{
						if((CurrSelectListItem.Classifier as MetaDataRepository.Classifier).Identity!=(MemberMedata.Classifier as MetaDataRepository.Classifier).Identity)
							RoleBDataNode=CurrSelectListItem;
						else
							RoleADataNode=CurrSelectListItem.ParentDataNode as DataNode;
					}
				}

			}
            RoleA = structureSet[RoleADataNode.Alias];
            RoleB = structureSet[RoleBDataNode.Alias];

		}
	

	}
}
