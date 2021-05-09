namespace OOAdvantech.MSSQLPersistenceRunTime
{
    using ObjectID = RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{B225E44F-39F0-487B-A3C5-255398280CAD}</MetaDataID>
	public class ClassMemoryInstanceCollection : PersistenceLayerRunTime.ClassMemoryInstanceCollection
	{
		/// <MetaDataID>{12C4AE8E-7E6E-414C-9868-C34B4924E1B1}</MetaDataID>
		public ClassMemoryInstanceCollection(System.Type theDotNetMetadata,PersistenceLayerRunTime.ObjectStorage theOwnerStorageSession)
			:base(theDotNetMetadata,theOwnerStorageSession)
		{

			ObjectStorage objectStorage=theOwnerStorageSession as ObjectStorage; 

			RDBMSMetaDataRepository.Class rdbmsMappingClass=(objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(Class) as RDBMSMetaDataRepository.Class;
			if(rdbmsMappingClass==null)
				throw new System.Exception("There isn't mapping data for class "+Class.Name+"."); 
			((DotNetMetaDataRepository.Class)Class).AddExtensionMetaObject(rdbmsMappingClass);
			
			foreach(DotNetMetaDataRepository.AssociationEnd associationEnd in Class.GetAssociateRoles(true))
			{
				if(associationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd))==null)
				{
                    RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd = (objectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                    if (rdbmsAssociationEnd == null)
                    {
                      //  throw new System.Exception("Problem with mapping of " + Class.Name + " class.");
                    }
                    else
                        associationEnd.AddExtensionMetaObject(rdbmsAssociationEnd);
				}
			}

			foreach(DotNetMetaDataRepository.AssociationEnd associationEnd in Class.GetRoles(true))
			{
				if(associationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd))==null)
				{
					RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd=(objectStorage.StorageMetaData  as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                    if (rdbmsAssociationEnd == null)
                    {
                        //throw new System.Exception("Problem with mapping of " + Class.Name + " class.");
                    }
                    else
                        associationEnd.AddExtensionMetaObject(rdbmsAssociationEnd);

				}
			}

            AssignRDBMSAttributeAsExtensionMetaObjects(Class, objectStorage);
            //foreach(DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
            //{
            //    RDBMSMetaDataRepository.Attribute rdbmsAttribute=(objectStorage.StorageMetaData  as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
            //    if(rdbmsAttribute==null&&Class.IsPersistent(attribute))
            //        throw new System.Exception("Problem with mapping of "+Class.Name+" class."); 
				
            //    if(rdbmsAttribute!=null)
            //        attribute.AddExtensionMetaObject(rdbmsAttribute);

            //}
		}
        void AssignRDBMSAttributeAsExtensionMetaObjects(DotNetMetaDataRepository.Class _class ,ObjectStorage objectStorage)
        {
            foreach (DotNetMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null && _class.IsPersistent(attribute))
                    throw new System.Exception("Problem with mapping of " + _class.Name + " class.");

                if (rdbmsAttribute != null)
                    attribute.AddExtensionMetaObject(rdbmsAttribute);

                if (attribute.Type is DotNetMetaDataRepository.Structure &&
                    _class.IsPersistent(attribute) &&
                    (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AssignRDBMSAttributeAsExtensionMetaObjects(attribute.Type as DotNetMetaDataRepository.Structure, objectStorage);
                }
            }

        }
        void AssignRDBMSAttributeAsExtensionMetaObjects(DotNetMetaDataRepository.Structure structure, ObjectStorage objectStorage)
        {
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null && structure.IsPersistent(attribute))
                    throw new System.Exception("Problem with mapping of " + structure.Name + " class.");

                if (rdbmsAttribute != null)
                    attribute.AddExtensionMetaObject(rdbmsAttribute);

                if (attribute.Type is DotNetMetaDataRepository.Structure &&
                    structure.IsPersistent(attribute) &&
                    (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AssignRDBMSAttributeAsExtensionMetaObjects(attribute.Type as DotNetMetaDataRepository.Structure, objectStorage);
                }
            }


        }
		/// <summary>lIAKOS</summary>
		/// <MetaDataID>{6B280E85-FB75-46EB-9C93-C1FF481C3ED3}</MetaDataID>
		protected override void AddOperativeObject(object Index, PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
			//this.StorageInstanceRefs
			
			if(storageInstanceRef==null)
				throw new System.Exception("You try to controll a null object.");
			ObjectID mObjectID=(ObjectID)Index;
		
			if(!StorageInstanceRefs.Contains(mObjectID.ObjCellID))
				StorageInstanceRefs.Add(mObjectID.ObjCellID,new System.Collections.Hashtable(100));
			System.Collections.Hashtable OperativeObjects=(System.Collections.Hashtable)StorageInstanceRefs[mObjectID.ObjCellID];
			
			System.WeakReference mWeakReference=new System.WeakReference(storageInstanceRef);
			if(mWeakReference.Target==null)
				throw new System.Exception("You try to controll a null object.");
				
			if(!OperativeObjects.Contains(mObjectID.IntObjID))
				OperativeObjects.Add(mObjectID.IntObjID,mWeakReference);
			else
				throw new System.Exception("Life time of object with ID "+storageInstanceRef.ObjectID.ToString()+" already controlled.");
		}
		/// <MetaDataID>{0F2EC8A4-8D69-429B-8F00-7DC60AB7ACE0}</MetaDataID>
		protected override PersistenceLayerRunTime.StorageInstanceRef GetOperativeObject(object Index)
		{
			ObjectID mObjectID=(ObjectID)Index;
			if(!StorageInstanceRefs.Contains(mObjectID.ObjCellID))
				return null;

			System.Collections.Hashtable OperativeObjects=(System.Collections.Hashtable)StorageInstanceRefs[mObjectID.ObjCellID];
			if(!OperativeObjects.Contains(mObjectID.IntObjID))
				return  null;
			System.WeakReference mWeakReference=(System.WeakReference )OperativeObjects[mObjectID.IntObjID];
			PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef=(PersistenceLayerRunTime.StorageInstanceRef)mWeakReference.Target;
			if(StorageInstanceRef==null)
				OperativeObjects.Remove(mObjectID.IntObjID);
			return StorageInstanceRef;
		}
	}
}
