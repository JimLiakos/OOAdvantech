namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
	/// <MetaDataID>{2D82D36E-8D04-42ED-A92C-80379DF9C4F2}</MetaDataID>
	public class RelResolver : OOAdvantech.PersistenceLayerRunTime.RelResolver
    {


        public override bool Contains(object obj)
        {
            if (obj == null)
                return false;
            if (AssociationEnd.Multiplicity.IsMany)
            {
                if (InternalLoadedRelatedObjects != null && InternalLoadedRelatedObjects.Contains(obj))
                    return true;
                if (!IsCompleteLoaded)
                {
                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceRef(obj) as PersistenceLayerRunTime.StorageInstanceRef;
                    if (storageInstanceRef == null || storageInstanceRef.PersistentObjectID == null)
                        return false;
                    Load("");
                    //Load(AssociationEnd.Name+" = "+storageInstanceRef.ObjectID.ToString());
                    return InternalLoadedRelatedObjects.Contains(obj);
                }
                else
                    return false;
            }
            else
            {
                return RelatedObject == obj;
            }
        } 

        public byte[] GetRelationStream()
		{
			byte[] relationDataSream=null;
			//System.Reflection.FieldInfo associationEndFieldInfo= Owner.Class.GetFieldMember(AssociationEnd as DotNetMetaDataRepository.AssociationEnd);
			if(AssociationEnd.Multiplicity.IsMany)// associationEndFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||associationEndFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
			{
				//Παράγει τα transaction command σε περίπτωση που το Field είναι συνδεδεμένο 
				//με Persistency Layer collection.
                OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastPropertyAccessor = Owner.Class.GetFastFieldAccessor(AssociationEnd as DotNetMetaDataRepository.AssociationEnd);
                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)fastPropertyAccessor.GetValue(Owner.MemoryInstance);
				if(theObjectContainer==null)
					throw new System.Exception("The collectio object "+Owner.Class.FullName+"."+AssociationEnd.Name+" has loose the connection with storage.");

				PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection=PersistenceLayerRunTime.StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;

				//TODO:Σε περίπτωση που σε μια σχέση one to many το persistence layer 
				//αρχικοποιήσει ένα collection field και ο χρήστης εκχώρηση σε αυτό το field 
				// άλλη collection τότε το persistency layer θα πρέπει να εγείρει exception.
				//Test

				if(mObjectCollection==null||mObjectCollection.RelResolver!=this)
					throw new System.Exception("The collection object "+Owner.Class.FullName+"."+AssociationEnd.Name+" has loose the connection with storage.");

				int count=mObjectCollection.Count;
               bool allowTransient  =Owner.Class.AllowTransient(AssociationEnd);
               if (count > 0)
               {
                   relationDataSream = new byte[(mObjectCollection.Count) * 4];
                   int offset = 0;
                   //OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(mObjectCollection.Count,relationDataSream,offset,ref offset);
                   foreach (object _object in mObjectCollection)
                   {
                       var storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object);
                       if (storageInstanceRef == null && allowTransient)
                           continue;
                       PersistenceLayer.ObjectID objectID = storageInstanceRef.PersistentObjectID;
                       if (objectID == null)
                       {
                           int tat = 0;
                       }

                       int OID = (int)objectID.GetMemberValue("ObjectID");
                       OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(OID, relationDataSream, offset, ref offset, true);
                   }
               }
				
				
			}
			else
			{
				//System.Reflection.FieldInfo associationFieldInfo=Owner.Class.GetFieldMember(AssociationEnd as DotNetMetaDataRepository.AssociationEnd);
                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Owner.Class.GetFastFieldAccessor(AssociationEnd as DotNetMetaDataRepository.AssociationEnd);
				//object Value=associationFieldInfo.GetValue(Owner.MemoryInstance);
                object Value = Member<object>.GetValue(fastFieldAccessor.GetValue, Owner.MemoryInstance);

				int offset=0;
				if(Value!=null)
				{
					relationDataSream=new byte[4];
					//OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize((int)1,relationDataSream,offset,ref offset);
                    int OID = StorageInstanceRef.GetStorageInstanceRef(Value).PersistentObjectID.GetTypedMemberValue<int>("ObjectID");
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(OID, relationDataSream, offset, ref offset, true);
				}
				/*else
				{
					relationDataSream=new byte[4];
					OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize((int)0,relationDataSream,offset,ref offset);
				}*/
			}
			RelationDataSream=relationDataSream;
			return relationDataSream;

		}


		internal byte[] RelationDataSream;
		/// <MetaDataID>{6DA556CA-B8DF-475C-8CC3-3F577595E4FD}</MetaDataID>
		public override  System.Collections.Generic.List<object> GetLinkedObjects(string criterion)
		{

			var storageInstanceRefs=GetLinkedStorageInstanceRefs(false);


            OOAdvantech.Collections.Generic.List<object> Objects = new Collections.Generic.List<object>(storageInstanceRefs.Count);
			foreach(StorageInstanceRef _object in storageInstanceRefs)
			{
				if(_object.PersistentObjectID==null)
				{
					//TODO το object έχει σβηστεί κανονικά δεν πρέπει να έρχετε το σύστημα σε αυτό το σημείο
					continue;
				}
				Objects.Add(_object.MemoryInstance);
			}
			
			//TODO:θα πρέπει να δουλεύει μα return null για λόγους ταχύτητας και μνήμης

			return Objects;
		}
 


	
		/// <MetaDataID>{75CED836-C236-4CC6-A2E3-653B5FC9B7F0}</MetaDataID>
        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
		{
		
		}
		/// <MetaDataID>{CBEEE524-C276-4481-8F60-01D559248C7E}</MetaDataID>
		public override long GetLinkedObjectsCount()
		{
            if (!IsCompleteLoaded)
                CompleteLoad();
			return InternalLoadedRelatedObjects.Count;
		}
		/// <MetaDataID>{7C25A119-74A8-4952-8E34-91CFFD19B1C9}</MetaDataID>
        public override System.Collections.Generic.List<object> GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
		{
            OOAdvantech.Collections.Generic.List<object> Objects = new Collections.Generic.List<object>();
			if(RelationDataSream==null)
				return Objects;
			int offset=0;
			int Count=RelationDataSream.Length/4;// OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(RelationDataSream,offset,ref offset);
			for(int i=0;i!=Count;i++)
			{
                int OID = BinaryFormatter.BinaryFormatter.ToInt32(RelationDataSream, offset, ref offset, true);
                StorageInstanceRef _object = null;

                    (Owner.ObjectStorage as AdoNetObjectStorage).StorageObjects.TryGetValue(new ObjectID( OID),out _object);

				//TODO αυτό είναι γρήγορα αλλά όχι και σοστώ γιατί δεν είναι σίγουρο ότι ένα object
				//δεν υπάρχει γιατί κάποιος το έσβησε
				//TODO οταν έχω μια σχέση όπως μεταξή τoy person και της address με navigate από το person στην address
				//και η address δεν εχει referencial integrity και σβήσο την address τότε κάποιο από τα object person
				//μπορεί να διχνει σε ένα αντικείμενο που έχει σβησθεί λάθος
				if(_object==null)
					continue;

				if(_object==null)
					throw new System.Exception("System can't retrieve object with id "+OID.ToString());

				if(_object.PersistentObjectID==null)
				{
					//TODO το object έχει σβηστεί κανονικά δεν πρέπει να έρχετε το σύστημα σε αυτό το σημείο
					continue;
				}

				Objects.Add(_object);
				int etwer=0;
			}
			//TODO:θα πρέπει να δουλεύει μα return null για λόγους ταχύτητας και μνήμης

			return Objects;
			
		}
	}
}
