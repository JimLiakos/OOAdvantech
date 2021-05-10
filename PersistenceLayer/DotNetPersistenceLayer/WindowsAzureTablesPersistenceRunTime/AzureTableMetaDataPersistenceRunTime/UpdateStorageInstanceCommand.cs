

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{A1646E36-A355-4F5C-A04E-F167DEF62DF2}</MetaDataID>
    public class UpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
    {
        public UpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef)
            : base(updatedStorageInstanceRef)
        {
        }
        static object[] itemArr = new object[] { null, null, null };
        internal bool FromNewCommand = false;
        /// <MetaDataID>{6D66B69B-AA5A-4AE6-B84A-826D4871F319}</MetaDataID>
        /// <summary>With this method execute the command. </summary>
        public override void Execute()
        {
            var objectBLOBDataTable = ((UpdatedStorageInstanceRef.ObjectStorage as ObjectStorage).StorageMetaData as Storage).ObjectBLOBDataTable;

            if (FromNewCommand)
            {
                ObjectBLOBData objectBLOBData = new ObjectBLOBData("AAA", UpdatedStorageInstanceRef.PersistentObjectID.GetMemberValue("ObjectID").ToString());

                (UpdatedStorageInstanceRef as StorageInstanceRef).ObjectBLOBData = objectBLOBData;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                (UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);


                int length = 0;
                length = (int)memoryStream.Length;
                byte[] BLOB = new byte[length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);

                objectBLOBData.ObjectData = BLOB;


                try
                {
                    int offset = 0;
                    (UpdatedStorageInstanceRef as StorageInstanceRef).ValidateObjectState(BLOB, offset, out offset);

                }
                catch (System.Exception error)
                {
                }
                

                objectBLOBData.ClassBLOBSID = (UpdatedStorageInstanceRef as StorageInstanceRef).SerializationMetada.ID.ToString();
               
                try
                {

                    Microsoft.Azure.Cosmos.Table.TableBatchOperation TableBatchOperation = ((UpdatedStorageInstanceRef as StorageInstanceRef).ObjectStorage as ObjectStorage).GetTableBatchOperation(objectBLOBDataTable);
                    TableBatchOperation.Insert(objectBLOBData);
                    //TableOperation insertOperation = TableOperation.Insert(objectBLOBData);
                    //objectBLOBDataTable.Execute(insertOperation);
                    //TableBatchOperation TableBatchOperation = new TableBatchOperation();
                }

#if DEBUG
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
#endif
                finally
                {
                }

            }
            else
            {
                bool hasChangeState = false;
                foreach (RelResolver relResolver in UpdatedStorageInstanceRef.RelResolvers)
                {
                    if (HasChanges(relResolver))
                        hasChangeState = true;
                }

                if (!hasChangeState && !UpdatedStorageInstanceRef.HasChangeState())
                    return;


                ObjectBLOBData objectBLOBData = (UpdatedStorageInstanceRef as StorageInstanceRef).ObjectBLOBData;


                if (UpdatedStorageInstanceRef.PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
                {

                }
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                (UpdatedStorageInstanceRef as StorageInstanceRef).SaveObjectState(memoryStream);
                byte[] BLOB = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(BLOB, 0, (int)memoryStream.Length);

                objectBLOBData.ObjectData = BLOB;

                try
                {
                    int offset = 0;
                    (UpdatedStorageInstanceRef as StorageInstanceRef).ValidateObjectState(BLOB, offset, out offset);

                }
                catch (System.Exception error)
                {
                }


                try
                {

                    Microsoft.Azure.Cosmos.Table.TableBatchOperation TableBatchOperation = ((UpdatedStorageInstanceRef as StorageInstanceRef).ObjectStorage as ObjectStorage).GetTableBatchOperation(objectBLOBDataTable);
                    TableBatchOperation.InsertOrReplace(objectBLOBData);


                    //TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(objectBLOBData);
                    //objectBLOBDataTable.Execute(insertOrReplaceOperation);
                    //TableBatchOperation TableBatchOperation = new TableBatchOperation();
                }

#if DEBUG
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
#endif
                finally
                {
                }


            }

        }
        public bool HasChanges(PersistenceLayerRunTime.RelResolver relResolver)
        {

            //System.Reflection.FieldInfo associationEndFieldInfo=relResolver.FieldInfo;
            AccessorBuilder.FieldPropertyAccessor associationEndFastFieldAccessor = relResolver.FastFieldAccessor;
            if (relResolver.AssociationEnd.Multiplicity.IsMany)// associationEndFieldInfo.FieldType==typeof(PersistenceLayer.ObjectContainer)||associationEndFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            {
                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)associationEndFastFieldAccessor.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                if (theObjectContainer == null)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");
                PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                if (mObjectCollection == null || mObjectCollection.RelResolver != relResolver)
                    throw new System.Exception("The collection object " + UpdatedStorageInstanceRef.Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");

                return mObjectCollection.HasChanges;
            }
            else
            {
                //object NewValue=associationEndFieldInfo.GetValue(UpdatedStorageInstanceRef.MemoryInstance);
                object NewValue = Member<object>.GetValue(associationEndFastFieldAccessor.GetValue, UpdatedStorageInstanceRef.MemoryInstance);
                object OldValue = relResolver.OriginalRelatedObject;

                if (NewValue != OldValue)
                    return true;
                return false;
            }

        }
    }
}