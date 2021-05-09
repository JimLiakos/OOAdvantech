namespace  OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <MetaDataID>{2D82D36E-8D04-42ED-A92C-80379DF9C4F2}</MetaDataID>
	public class RelResolver : OOAdvantech.PersistenceLayerRunTime.RelResolver
	{

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
				if(count>0)
				{
					relationDataSream=new byte[(mObjectCollection.Count)*4];
					int offset=0;
					//OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(mObjectCollection.Count,relationDataSream,offset,ref offset);
					foreach(object _object in mObjectCollection)
					{
						object objectID=StorageInstanceRef.GetStorageInstanceRef(_object).ObjectID;
						if(objectID==null)
						{
							int tat=0;
						}

						int OID=(int)objectID;
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
					int OID=(int)StorageInstanceRef.GetStorageInstanceRef(Value).ObjectID;
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
		public override System.Collections.ArrayList GetLinkedObjects(string criterion)
		{

			System.Collections.ArrayList storageInstanceRefs=GetLinkedStorageInstanceRefs(false);


			System.Collections.ArrayList Objects=new System.Collections.ArrayList(storageInstanceRefs.Count);
			foreach(StorageInstanceRef _object in storageInstanceRefs)
			{
				if(_object.ObjectID==null)
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
			return _LoadedRelatedObjects.Count;
		}
		/// <MetaDataID>{7C25A119-74A8-4952-8E34-91CFFD19B1C9}</MetaDataID>
		public override System.Collections.ArrayList GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
		{
			System.Collections.ArrayList Objects=new System.Collections.ArrayList();
			if(RelationDataSream==null)
				return Objects;
			int offset=0;
			int Count=RelationDataSream.Length/4;// OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(RelationDataSream,offset,ref offset);
			for(int i=0;i!=Count;i++)
			{
                int OID = BinaryFormatter.BinaryFormatter.ToInt32(RelationDataSream, offset, ref offset, true);
                StorageInstanceRef _object = (Owner.ObjectStorage as AdoNetObjectStorage).StorageObjects[OID] as StorageInstanceRef;

				//TODO αυτό είναι γρήγορα αλλά όχι και σοστώ γιατί δεν είναι σίγουρο ότι ένα object
				//δεν υπάρχει γιατί κάποιος το έσβησε
				//TODO οταν έχω μια σχέση όπως μεταξή τoy person και της address με navigate από το person στην address
				//και η address δεν εχει referencial integrity και σβήσο την address τότε κάποιο από τα object person
				//μπορεί να διχνει σε ένα αντικείμενο που έχει σβησθεί λάθος
				if(_object==null)
					continue;

				if(_object==null)
					throw new System.Exception("System can't retrieve object with id "+OID.ToString());

				if(_object.ObjectID==null)
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
