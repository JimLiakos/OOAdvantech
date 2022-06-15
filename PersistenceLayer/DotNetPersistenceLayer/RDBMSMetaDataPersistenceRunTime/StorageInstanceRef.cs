namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
	/// <MetaDataID>{87FC5239-DE87-4462-8A02-8698C91DC53A}</MetaDataID>
	public class StorageInstanceRef : OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef
	{

        /// <MetaDataID>{ad86f68e-b35b-44ba-8b23-151a22bdaffb}</MetaDataID>
        OOAdvantech.MetaDataRepository.StorageCell _StorageInstanceSet;
        public override OOAdvantech.MetaDataRepository.StorageCell StorageInstanceSet
        {
            get 
            { 
                if(_StorageInstanceSet==null)                    
                    _StorageInstanceSet=new StorageCell(ObjectStorage.StorageMetaData.StorageIdentity,Class,ObjectStorage.StorageMetaData as MetaDataRepository.Namespace,LifeTimeController);
                return _StorageInstanceSet;
            }
        }
		/// <MetaDataID>{7933B97E-428C-47E6-BBA1-A8EADFC47285}</MetaDataID>
		public readonly DataObjects.ClassBLOB SerializationMetada;
		/// <MetaDataID>{43A985E6-225F-4C25-A0B8-C93701547847}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayerRunTime.ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID, bool doubleStorageInstanceRefCheck = true)
            : base(memoryInstance, activeStorageSession,null, objectID, doubleStorageInstanceRefCheck)
		{
            SerializationMetada=(ObjectStorage.StorageMetaData as Storage).GetClassBLOB(Class as DotNetMetaDataRepository.Class);
		}
		/// <MetaDataID>{70DC887E-37DC-4C7C-8AAE-359E64544CD5}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
		{
			return new RelResolver(this,associationEnd,fastFieldAccessor);
		}
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RelResolver(owner, associationEnd, fastFieldAccessor);
            
        }
		/// <MetaDataID>{A523B5AF-7480-4275-98F4-B5592364E61A}</MetaDataID>
		public void LoadObjectState(byte[] byteStream,int offset,out int nextAvailablePos )
		{
            System.Collections.Generic.List<ValueOfAttribute> valuesOfAttribute = GetPersistentAttributeMetaData();
            

			foreach(DataObjects.Member member in SerializationMetada.Members)
			{
				if(offset==byteStream.Length)
					break;
				object value =member.LoadMemberData(byteStream,this,offset,out offset);
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
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, valueOfAttribute.Culture));
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, value);
                            break;
                        }
                    }
                }
			}
			nextAvailablePos=offset;

		}
		internal int LinkClassRoleAOID=0;
		internal int LinkClassRoleBOID=0;

		public void ResolveRelationships()
		{

			//TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
			if(Class.LinkAssociation!=null)
			{
                StorageInstanceRef roleAObject = (ObjectStorage as AdoNetObjectStorage).StorageObjects[new ObjectID( LinkClassRoleAOID)] as StorageInstanceRef;
                StorageInstanceRef roleBObject = (ObjectStorage as AdoNetObjectStorage).StorageObjects[new ObjectID(LinkClassRoleBOID)] as StorageInstanceRef;
				//(Class as DotNetMetaDataRepository.Class).LinkClassRoleAField.SetValue(MemoryInstance,roleAObject.MemoryInstance);
                object memoryInstance = MemoryInstance;
                Member<object>.SetValueImplicitly((Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor, ref memoryInstance, roleAObject.MemoryInstance);
				//(Class as DotNetMetaDataRepository.Class).LinkClassRoleBField.SetValue(MemoryInstance,roleBObject.MemoryInstance);
                
                Member<object>.SetValueImplicitly((Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor, ref memoryInstance, roleBObject.MemoryInstance);
			}
	


			foreach (PersistenceLayerRunTime.RelResolver relResolver in RelResolvers)
			{
				PersistenceLayerRunTime.RelResolver mResolver=relResolver;
				DotNetMetaDataRepository.AssociationEnd associationEnd=mResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;

				//System.Reflection.FieldInfo CurrFieldInfo=Class.GetFieldMember(associationEnd);
                if (associationEnd.Multiplicity.IsMany)
                    continue;
                //if(CurrFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||CurrFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                //    continue;
				
				if(!Class.IsLazyFetching( mResolver.AssociationEnd))
					LazyFetching(mResolver,MemoryInstance.GetType());
			}
		}

		/// <MetaDataID>{0840127A-0D5D-46CD-B2D3-B4D13D3E037B}</MetaDataID>
		public void SaveObjectState(System.IO.MemoryStream memoryStream)
		{
            bool loadTest = false;
            foreach (DataObjects.Member member in SerializationMetada.Members)
            {
                member.SaveMemberData(memoryStream, this);
                if(member.Name=="DataBaseColumnName")
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
        public override OOAdvantech.PersistenceLayer.ObjectID PersistentObjectID
		{
			get
			{
				return _PersistentObjectID;
			}
			set
			{
				
				if(value==null)
				{
                    if (base.PersistentObjectID != null && (ObjectStorage as AdoNetObjectStorage).StorageObjects.ContainsKey(base.PersistentObjectID))
                        (ObjectStorage as AdoNetObjectStorage).StorageObjects.Remove(base.PersistentObjectID);
					base.PersistentObjectID=value;
				}
				else
				{
                    if (base.PersistentObjectID != value)
                    {
                        base.PersistentObjectID = value;
                        (ObjectStorage as AdoNetObjectStorage).StorageObjects.Add(value, this);
                    }
				}
					
				
				
				
			}
		}

		

	}
}
