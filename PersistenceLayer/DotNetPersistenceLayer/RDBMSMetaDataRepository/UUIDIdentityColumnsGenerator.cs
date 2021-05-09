using System.Linq;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    using OOAdvantech.Transactions;
	/// <MetaDataID>{BCFC20A8-5EAA-416F-8366-8EE7A4461544}</MetaDataID>
	public class UUIDAutoProduceColumnsGenerator : AutoProduceColumnsGenerator
	{

     
  //      /// <MetaDataID>{5e8ee1f0-bba5-4edf-96b4-a0cc8e52a1ab}</MetaDataID>
		//static OOAdvantech.Collections.Map SystemGuidTypes=new OOAdvantech.Collections.Map();
        /// <MetaDataID>{9068d502-5eda-4fe2-a21c-1254924483f3}</MetaDataID>
		public override MetaDataRepository.ObjectIdentityType GetObjectIdentityType(MetaDataRepository.MetaObject placeIdentifier)
		{


            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                //TODO καλείται και όταν και όταν το MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator δεν είναι RDBMSMetaDataRepository.MetaObjectsStack
                System.Collections.Generic.List<MetaDataRepository.IIdentityPart> identityColums = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier);
                MetaDataRepository.Classifier doteNetGuid = MetaDataRepository.Classifier.GetClassifier(typeof(System.Guid));
                MetaDataRepository.Structure systemGuidType = null;
                if (placeIdentifier is MetaDataRepository.Classifier)
                    systemGuidType = (placeIdentifier.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;
                else if (placeIdentifier.Namespace is MetaDataRepository.Classifier)
                    systemGuidType = (placeIdentifier.Namespace.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;
                else
                    if (placeIdentifier is MetaDataRepository.Feature)
                        systemGuidType = ((placeIdentifier as MetaDataRepository.Feature).Owner.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;

                if (systemGuidType == null && MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator is RDBMSMetaDataRepository.MetaObjectsStack)
                {
                    systemGuidType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(doteNetGuid, placeIdentifier) as MetaDataRepository.Structure;
                    if (systemGuidType == null)
                    {
                        //System.Diagnostics.Debug.Assert(false);
                        systemGuidType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(doteNetGuid, placeIdentifier) as MetaDataRepository.Structure;
                        systemGuidType.Synchronize(doteNetGuid);
                    }

                }

                //Αυτό γιατί αν αποτύχη η transaction η τιμή του name θα γυρίσει σε null
                //TODO πρέπει να λυθεί το πρόβλημα με την χρήση new transaction.
                if (systemGuidType == null || systemGuidType.Name == null)
                {
                    systemGuidType = new RDBMSMetaDataRepository.Structure();
                    objectStorage.CommitTransientObjectState(systemGuidType);
                    systemGuidType.Synchronize(doteNetGuid);
                }

                //IdentityColumn ObjectIDColumn = new IdentityColumn("ObjectID", systemGuidType, "ObjectID", false);
                MetaDataRepository.IdentityPart ObjectIDColumn = new OOAdvantech.MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(System.Guid));
                identityColums.Add(ObjectIDColumn);


                MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(identityColums);
                //foreach (IdentityColumn idColumn in objectIdentityType.Parts)
                //    idColumn.ObjectIdentityType = objectIdentityType; 
          
        

          
                return objectIdentityType;
             
            }

            
			
		}

        /// <MetaDataID>{bb5f21c4-8900-4c2a-b3dd-ba5b9ac58ca4}</MetaDataID>
        public override MetaDataRepository.ObjectIdentityType GetObjectIdentityRefernce(OOAdvantech.MetaDataRepository.MetaObject placeIdentifier, AssociationEnd associationEnd,string associationName)
        {

            //using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Suppress))
            {
                //TODO καλείται και όταν και όταν το MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator δεν είναι RDBMSMetaDataRepository.MetaObjectsStack
                System.Collections.Generic.List<MetaDataRepository.IIdentityPart> identityColums = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier);
                MetaDataRepository.Structure systemGuidType = null;
                MetaDataRepository.Classifier doteNetGuid = MetaDataRepository.Classifier.GetClassifier(typeof(System.Guid));

                //if (associationEnd is RDBMSMetaDataRepository.AssociationEnd &&  associationEnd.ImplementationUnit.Context == null)
                //{
                  
                //    OOAdvantech.Linq.Storage theStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(associationEnd));

                //    var storage = (from metastorage in theStorage.GetObjectCollection<Storage>() select metastorage).FirstOrDefault();
                //    associationEnd.ImplementationUnit.Context = storage;
                //    systemGuidType = (storage  ).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;

                //}
                //else
                {
      
                    if (placeIdentifier is MetaDataRepository.Classifier)
                        systemGuidType = (placeIdentifier.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;
                    else if (placeIdentifier.Namespace is MetaDataRepository.Classifier)
                        systemGuidType = (placeIdentifier.Namespace.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;
                    else
                        if (placeIdentifier is MetaDataRepository.Feature)
                        systemGuidType = ((placeIdentifier as MetaDataRepository.Feature).Owner.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(doteNetGuid) as MetaDataRepository.Structure;
                }
                if (systemGuidType == null && MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator is RDBMSMetaDataRepository.MetaObjectsStack)
                {
                    systemGuidType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(doteNetGuid, placeIdentifier) as MetaDataRepository.Structure;
                    if (systemGuidType == null)
                    {
                        //System.Diagnostics.Debug.Assert(false);
                        systemGuidType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(doteNetGuid, placeIdentifier) as MetaDataRepository.Structure;
                        systemGuidType.Synchronize(doteNetGuid);
                    }

                }

                //Αυτό γιατί αν αποτύχη η transaction η τιμή του name θα γυρίσει σε null
                //TODO πρέπει να λυθεί το πρόβλημα με την χρήση new transaction.
                if (systemGuidType == null || systemGuidType.Name == null)
                {
                    systemGuidType = new RDBMSMetaDataRepository.Structure();
                    objectStorage.CommitTransientObjectState(systemGuidType);
                    systemGuidType.Synchronize(doteNetGuid);
                }
                if (associationEnd.IsRoleA)
                {
                    //IdentityColumn ObjectIDColumn = null;
                    //foreach (var mappedColumn in associationEnd.MappedColumns)
                    //{
                    //    if (mappedColumn.Name == associationName + "_ObjectIDB" &&
                    //        systemGuidType == mappedColumn.Type &&
                    //        mappedColumn is IdentityColumn)
                    //    {

                    //        ObjectIDColumn = mappedColumn as IdentityColumn;
                    //        break;
                    //    }
                    //}
                    //if (ObjectIDColumn == null)

                    //IdentityColumn ObjectIDColumn = new IdentityColumn(associationName + "_ObjectIDB", associationEnd, systemGuidType, "ObjectID", false);
                    MetaDataRepository.IdentityPart ObjectIDColumn = new MetaDataRepository.IdentityPart(associationName + "_ObjectIDB", "ObjectID", typeof(System.Guid));
                    //ObjectIDColumn.AllowNulls = true;

                    identityColums.Add(ObjectIDColumn);
                }
                else
                {
                    //IdentityColumn ObjectIDColumn = null;
                    //foreach (var mappedColumn in associationEnd.MappedColumns)
                    //{
                    //    if (mappedColumn.Name == associationName + "_ObjectIDA" &&
                    //        systemGuidType == mappedColumn.Type &&
                    //        mappedColumn is IdentityColumn)
                    //    {

                    //        ObjectIDColumn = mappedColumn as IdentityColumn;
                    //        break;
                    //    }
                    //}
                    //if (ObjectIDColumn == null)
                    //{
                    //IdentityColumn ObjectIDColumn = new IdentityColumn(associationName + "_ObjectIDA", associationEnd, systemGuidType, "ObjectID", false);
                    MetaDataRepository.IdentityPart ObjectIDColumn = new MetaDataRepository.IdentityPart(associationName + "_ObjectIDA", "ObjectID", typeof(System.Guid));
                    //ObjectIDColumn.AllowNulls = true;
                    //}
                    identityColums.Add(ObjectIDColumn);
                }
                MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(identityColums);
                //foreach (IdentityColumn idColumn in objectIdentityType.Parts)
                //    idColumn.ObjectIdentityType = objectIdentityType;
                return objectIdentityType;
            }
		
        }

        /// <MetaDataID>{7daeccee-7fec-40ba-af4a-5dc60cad58d0}</MetaDataID>
		static System.Collections.Generic.Dictionary<OOAdvantech.PersistenceLayer.ObjectStorage, MetaDataRepository.Primitive> SystemInt32Types = new System.Collections.Generic.Dictionary<PersistenceLayer.ObjectStorage, MetaDataRepository.Primitive>(); 
        /// <MetaDataID>{4f8a1331-8542-4ed4-9bd8-7577f269755b}</MetaDataID>
		public override Collections.Generic.Set<Column> GetAuxiliaryColumns(MetaDataRepository.MetaObject placeIdentifier)
		{

           // using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                Collections.Generic.Set<Column> columns = new OOAdvantech.Collections.Generic.Set<Column>();
                // Δεν είναι καλή ιδέα να χρησιμοποιής MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator γιατί αλλάζει καλήτερα να το ψάχνεις στην storage
                MetaDataRepository.Primitive SystemInt32Type = null; //SystemInt32Types[PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties)] as MetaDataRepository.Primitive;
                SystemInt32Types.TryGetValue(PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties), out SystemInt32Type);
                
                //SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32", placeIdentifier) as MetaDataRepository.Primitive;
                //Αυτό γιατί αν αποτύχη η transaction η τιμή του name θα γυρίσει σε null
                //TODO πρέπει να λυθεί το πρόβλημα με την χρήση new transaction.
                if (SystemInt32Type == null || SystemInt32Type.Name == null)
                {
                    //MetaDataRepository.Namespace mNamespace = new MetaDataRepository.Namespace();
                    //mNamespace.Name = "System";
                    //MetaDataRepository.Primitive mPrimitive = new MetaDataRepository.Primitive();
                    //mPrimitive.Name = "Int32";
                    //mNamespace.AddOwnedElement(mPrimitive);
                    MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                    SystemInt32Type = (Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive, placeIdentifier);
                    SystemInt32Type.Synchronize(mPrimitive);
                    SystemInt32Types[PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties)] = SystemInt32Type;
                }
                Column column = new Column();
                column.Name = "TypeID";
                column.Type = SystemInt32Type;
                column.AllowNulls = false;
                columns.Add(column);



                return columns;
            }
		}

        /// <MetaDataID>{186dda91-db2d-4a38-90aa-65260378e9b8}</MetaDataID>
		public override string[] GetObjectIdentityColumnsName()
		{
			return new string[1]{"ObjectID"};
		}
        /// <MetaDataID>{1ff48c24-8ab9-4f7f-b62d-dcd4ab5d637c}</MetaDataID>
		public override string[] GetAuxiliaryColumnsName()
		{
			return new string[1]{"TypeID"};
		}



	}
}
