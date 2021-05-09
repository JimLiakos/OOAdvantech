namespace OOAdvantech.PersistenceLayerRunTime
{

    using System.Collections.Specialized;
    using System.Reflection;

    /// <MetaDataID>{3EFDE02C-C423-4C66-8315-D1C62F789E8A}</MetaDataID>
    /// <summary>Define a collection of StorageInstanceRefs that are logical connections between the operative objects and storage instances of class which defines the member "Class".
    /// Ensure the uniqueness of storage instances in memory. For each storage instance there is on corresponding memory instance (operative object) or nothing. Control the life time of operative objects this means that can define "load all in memory" attribute in persistent class and all storage instance loaded in memory also you can define "delay garbage collection" this means that when load an storage instance stay in memory at least time the time you have defined. </summary>
    public class ClassMemoryInstanceCollection
    {
        /// <MetaDataID>{8C9C101B-138A-407D-A290-B14AC371CD7F}</MetaDataID>
        public System.Type Type;

        /// <MetaDataID>{FDC3137A-FDBD-4095-AE9D-C0E84077EAB6}</MetaDataID>
        public DotNetMetaDataRepository.Class Class;
        /// <MetaDataID>{F5719453-6858-4F8D-A8D2-C9893F94B415}</MetaDataID>
        public ClassMemoryInstanceCollection(System.Type theDotNetMetadata, ObjectStorage storage)
        {
            Type = theDotNetMetadata;

            MetaDataRepository.MetaObject assemblyMetaObject = DotNetMetaDataRepository.Assembly.GetComponent(Type.GetMetaData().Assembly);
            //MetaDataRepository.MetaObject AssemblyMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(Type.GetMetaData().Assembly);
            //if (AssemblyMetaObject == null)
            //    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Type.GetMetaData().Assembly, new DotNetMetaDataRepository.Assembly(Type.GetMetaData().Assembly));

            ///TODO φορτώνει την class χωρίς να φορτωθούν όλα τα meta data του assembly 
            ///αυτός ο τρόπος δεν έχει τεσταριστεί επαρκώς.


            Class = DotNetMetaDataRepository.Type.GetClassifierObject(theDotNetMetadata) as DotNetMetaDataRepository.Class;
            long count = Class.GetRoles(true).Count;


            if (Class == null)
                throw new System.Exception("Persistence service can't retrieve Metadata for type '" + theDotNetMetadata.FullName + "'");

            if (storage.StorageMetaData != null)
            {
                if (PersistenceLayer.ObjectStorage.GetStorageOfObject(storage.StorageMetaData) != null)
                {
                    string Query = "SELECT Class FROM " + typeof(MetaDataRepository.Class).FullName + " Class WHERE Class.MetaObjectIDStream = " + Class.Identity;
                    Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(storage.StorageMetaData).Execute("SELECT Class FROM " + typeof(MetaDataRepository.Class).FullName + " Class WHERE Class.MetaObjectIDStream = \"" + Class.Identity.ToString() + "\" ");
                    foreach (Collections.StructureSet Rowset in aStructureSet)
                    {
                        MetaDataRepository.Class tmpClass = (MetaDataRepository.Class)Rowset["Class"];
                        ((DotNetMetaDataRepository.Class)Class).AddExtensionMetaObject(tmpClass);
                        break;
                    }
                }
            }

        }





        //TODO: δεν πρέπει να είναι public

        /// <metadataid>{A9D6771A-6374-435B-B888-F83E96FFC037}</metadataid>
        /// <summary>ClassObjectDi  vsds sg sgfg ionary </summary>
        public System.Collections.Generic.Dictionary<object, System.WeakReference> StorageInstanceRefs = new System.Collections.Generic.Dictionary<object, System.WeakReference>(10);


        /// <summary>lIAKOS </summary>
        /// <MetaDataID>{7046eb45-02f4-493e-87a6-96fdfc3798f2}</MetaDataID>
        protected virtual void AddOperativeObject(object Index, PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            if (storageInstanceRef == null)
                throw new System.Exception("You try to controll a null object.");

            
            System.WeakReference OldWeakReference=null;
            if (!StorageInstanceRefs.TryGetValue(Index, out OldWeakReference))
            {
                System.WeakReference mWeakReference = new System.WeakReference(storageInstanceRef);
                StorageInstanceRefs.Add(Index, mWeakReference);
            }
            else
            {
                if (OldWeakReference.IsAlive && OldWeakReference.Target != storageInstanceRef)
                    throw new System.Exception("Life time of object with ID " + storageInstanceRef.PersistentObjectID.ToString() + " already controlled.");
                else if (!OldWeakReference.IsAlive)
                {
                    System.WeakReference mWeakReference = new System.WeakReference(storageInstanceRef);
                    StorageInstanceRefs.Add(Index, mWeakReference);
                }
            }
        }
        /// <MetaDataID>{8BE7D7E0-C04F-4281-889D-1B0461D756D8}</MetaDataID>
        protected virtual PersistenceLayerRunTime.StorageInstanceRef GetOperativeObject(object Index)
        {
            System.WeakReference mWeakReference =null;// (System.WeakReference)StorageInstanceRefs[Index];
            if (StorageInstanceRefs.TryGetValue(Index, out mWeakReference))
            {

                StorageInstanceRef storageInstanceRef =mWeakReference.Target as StorageInstanceRef;
                if (storageInstanceRef == null)
                {
                    StorageInstanceRefs.Remove(Index);
                    System.Diagnostics.Debug.WriteLine("WeakReference on null: The object with ID " + Index.ToString() + " Collected without removed from liftime controller.");
                }
                return storageInstanceRef;
            }
            else
                return null;
        }
        /// <metadataid>{677ceb00-91ee-4149-9d31-0c37a76ecc0d}</metadataid>
        /// <summary></summary>
        public PersistenceLayerRunTime.StorageInstanceRef this[object Index]   // long is a 64-bit integer
        {

            get
            {
                return GetOperativeObject(Index);
            }
            set
            {
                AddOperativeObject(Index, value);
            }
        }
        /// <summary>Mistsos </summary>
        /// <MetaDataID>{fca6602c-0f18-42c5-b66a-dc36aa26193f}</MetaDataID>
        public void ObjectDestroyed(StorageInstanceRef aStorageInstanceRef)
        {

            if (!StorageInstanceRefs.ContainsKey(aStorageInstanceRef.PersistentObjectID))
                return;

            StorageInstanceRefs.Remove(aStorageInstanceRef.PersistentObjectID);
            return;

            string tracestring = null;
            if (aStorageInstanceRef.MemoryInstance != null)
            {
                tracestring = "PersistencyContext Collect StorageInstanceRef with type "
                    + aStorageInstanceRef.MemoryInstance.GetType().FullName
                    + " and id :";
            }
            else
                tracestring = "PersistencyContext Collect StorageInstanceRef with id :";
            tracestring += aStorageInstanceRef.PersistentObjectID.ToString();
            System.Diagnostics.Debug.WriteLine(tracestring);
        }
    }
}
