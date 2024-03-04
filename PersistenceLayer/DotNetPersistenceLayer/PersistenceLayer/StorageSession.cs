namespace OOAdvantech.PersistenceLayer
{
    using Remoting;
    using System;
    using System.Collections.Generic;
#if DeviceDotNet
    // using Collections = System.Collections;
#endif

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif

    public delegate void ObjectStorageResolveHandler(object sender, string StorageIdentity);
    public delegate void ObjectStorageLoadeHandler(object sender, string StorageIdentity);



    /// <MetaDataID>{3096B31D-B5CC-4B5C-823B-41CE91DA5C98}</MetaDataID>
    /// <summary>A StorageSession object represents a unique session with 
    /// a object storage. 
    /// In the case of a client/server database system, it may be 
    /// equivalent to an actual network connection to the server. </summary>
    public abstract class ObjectStorage : ObjectsContext
    {

        //public static MetaDataRepository.IErrorLog ErrorLog;
        /// <MetaDataID>{b096806d-ed23-4129-9792-a6deaf42fb8f}</MetaDataID>
        public virtual bool IsReadonly
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{e91b5c0d-61c4-4acf-a0ff-2fd8ee804cdc}</MetaDataID>
        public virtual void Backup(IBackupArchive archive)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{2f44c73e-6a6c-466e-a250-a00f38d4d13a}</MetaDataID>
        abstract public string GetPersistentObjectUri(object obj);

        public static event ObjectStorageResolveHandler ObjectStorageResolve
        {
            add
            {
                PersistencyService.ObjectStorageResolve += value;
            }
            remove
            {
                PersistencyService.ObjectStorageResolve -= value;
            }
        }

        public static event ObjectStorageLoadeHandler ObjectStorageLoad
        {
            add
            {
                PersistencyService.ObjectStorageLoad += value;
            }
            remove
            {
                PersistencyService.ObjectStorageLoad -= value;
            }
        }

        /// <MetaDataID>{e33b2fbe-5ce7-479e-b135-8838dc284028}</MetaDataID>
        public abstract void MoveObject(object persistentObject);


        /// <MetaDataID>{10b11e44-39ed-4732-b7f1-0b06c8c91026}</MetaDataID>
        public override string Identity
        {
            get
            {
                return StorageMetaData.StorageIdentity;
            }
        }
        /// <MetaDataID>{51fda8c4-66b4-49bc-a160-d88347f2e1c3}</MetaDataID>
        public override string ToString()
        {
            if (StorageMetaData != null)
                return StorageMetaData.StorageName + " : " + StorageMetaData.StorageIdentity + " " + StorageMetaData.StorageLocation;
            return base.ToString();
        }



        /// <MetaDataID>{3BE94029-4BDC-4C2E-AFBB-7C3BBEB0D287}</MetaDataID>
        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation. </summary>
        /// <param name="storageMetadata">This parameter defines the metadata of new storage. Actually it is metadata from other storage that cloned in new storage. </param>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="StorageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <param name="InProcess">Define the possibility to the objects of storage to run in process
        /// which open a session with storage. 
        /// This means that if two processes try to open session with the same storage only one will gain the control. 
        /// All storage types doesn't allow this possibility, some allow and others not. </param>
        public static ObjectStorage NewStorage(Storage storageMetadata, string StorageName, string StorageLocation, string StorageType, bool InProcess)
        {
            return default(ObjectStorage);
        }
        //#if !DeviceDotNet
        /// <MetaDataID>{73594137-3720-48C5-8525-A47D0A7C7DAD}</MetaDataID>
        public abstract Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate);

        /// <MetaDataID>{F233F4D7-6327-4104-9420-696611432F26}</MetaDataID>
        public abstract OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCell, string ofTypeIdentity = null);

        /// <MetaDataID>{75BA88E5-01BE-456A-AF4F-A0DE58C2B258}</MetaDataID>
        public abstract Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(MetaDataRepository.Classifier classifier);

        /// <MetaDataID>{DCE3F3A4-5569-45CD-B0C7-0E0907C5E53A}</MetaDataID>
        public abstract OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole, string ofTypeIdentity = null);



        /// <MetaDataID>{ee9ab3c8-f821-4c4a-852f-052de8773012}</MetaDataID>
        public abstract MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber);

        /// <MetaDataID>{69D4B685-85C4-49A9-97BB-9AB05AD1ADD8}</MetaDataID>
        public abstract MetaDataRepository.StorageCell GetStorageCell(object ObjectID);


        //#else
        //         /// <MetaDataID>{73594137-3720-48C5-8525-A47D0A7C7DAD}</MetaDataID>
        //         protected abstract Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate);
        //        /// <MetaDataID>{F233F4D7-6327-4104-9420-696611432F26}</MetaDataID>
        //         protected abstract Collections.Generic.Set<MetaDataRepository.StorageCell> GetLinkedStorageCells(MetaDataRepository.AssociationEnd associationEnd,MetaDataRepository.ValueTypePath  valueTypePath,Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCell);
        //        /// <MetaDataID>{75BA88E5-01BE-456A-AF4F-A0DE58C2B258}</MetaDataID>
        //        abstract  protected Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(MetaDataRepository.Classifier classifier);
        //        /// <MetaDataID>{DCE3F3A4-5569-45CD-B0C7-0E0907C5E53A}</MetaDataID>
        //         protected abstract Collections.Generic.Set<MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(MetaDataRepository.Association association, Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCells, MetaDataRepository.Roles storageCellsRole);


        //        /// <MetaDataID>{69D4B685-85C4-49A9-97BB-9AB05AD1ADD8}</MetaDataID>
        //         protected abstract MetaDataRepository.StorageCell GetStorageCell(object ObjectID);

        //#endif


        /// <MetaDataID>{94945BEF-3659-4123-AA02-4F1B08CB56D7}</MetaDataID>

        protected OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <MetaDataID>{5D58FD9B-441A-4CE5-B36C-2C55BAF5FA8D}</MetaDataID>
        public static void DeleteStorage(string StorageName, string StorageLocation, string StorageType)
        {
            PersistencyService.DeleteStorage(StorageName, StorageLocation, StorageType);
        }

        /// <MetaDataID>{543828ff-06c4-4495-addc-76700db1399a}</MetaDataID>
        public static bool IsPersistent(object _object)
        {
            if (_object == null)
                return false;


            try
            {
                return PersistenceLayer.ObjectStorage.PersistencyService.IsPersistent(_object);
            }
            catch (System.Exception Error)
            {
                int hh = 0;
                throw;
            }


            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object);
            if (storageInstanceRef != null && storageInstanceRef.IsPersistent)
                return true;
            return false;

        }

        /// <MetaDataID>{AB18DFBF-11BB-48CD-931E-BFD155AA8E67}</MetaDataID>
        /// <summary>This method creates a new object with type that defined from parameter type. </summary>
        /// <param name="type">Define the type of object that will be create. </param>
        /// <param name="ctorParams">With ctorParams you can call other than default object constructor.
        ///  If you use the ctorParams the last parameter must be a type array with the types of 
        /// parameters of constructor, which you want to call. It is useful for type check. 
        /// You can avoid wrong constructor call. </param>
        public abstract object NewTransientObject(Type type, Type[] paramsTypes, params object[] ctorParams);

        /// <MetaDataID>{e709582c-fd86-4025-a038-240e364d27f9}</MetaDataID>
        public abstract object NewTransientObject(Type type);


        /// <MetaDataID>{8433f2e0-986d-4cf6-8492-5e80d069bdf8}</MetaDataID>
        public T NewTransientObject<T>()
        {
            return (T)NewTransientObject(typeof(T));
        }

        /// <MetaDataID>{df8d0442-ccb9-4d02-98ec-bb1b475c0f3b}</MetaDataID>
        public T NewTransientObject<T>(Type[] paramsTypes, params object[] ctorParams)
        {
            return (T)NewTransientObject(typeof(T), paramsTypes, ctorParams);
        }

        /// <summary>Executes the specified query in OQLStatement parameter. </summary>
        /// <param name="OQLStatement">A String value that contains the OQL statement </param>
        /// <param name="parameters">Define the values of parameters that can use in OQL expression. </param>
        /// <returns>Return a StructureSet object that contain the result of object query statement. </returns>
        /// <example>
        /// 	<code lang="C#">
        /// StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
        /// string OQLQuery="SELECT person.Name Name,person.Address.Street Street "+
        /// "FROM Person person "+
        /// "WHERE person.Parent.Address.City=person.City AND person.City=@City";
        /// OOAdvantech.Collections.Map parameters=new OOAdvantech.Collections.Map();
        /// parameters["@City"]="London";
        /// StructureSet structureSet=storage.Execute(OQLQuery,parameters);
        /// foreach(StructureSet structureSetInstance in StructureSet)
        /// {
        /// 	string name=structureSetInstance["Name"] as string;
        /// 	string street=structureSetInstance["Street"] as string;
        /// 	System.Console.WriteLine("Name: "+name+", Street: "+street);
        /// }
        /// structureSet.Close();
        /// </code>
        /// 	<code lang="Visual Basic">
        /// Dim storage As StorageSession
        /// Dim structureSet As StructureSet
        /// Dim OQLQuery As String
        /// storage = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
        /// OQLQuery = "SELECT person.Name Name,person.Address.Street Street "
        /// OQLQuery += "FROM Person person "
        /// OQLQuery += "WHERE person.Parent.Address.City=person.City AND person.City=@City"
        /// Dim parameters As New OOAdvantech.Collections.Map
        /// parameters("@City") = "London"
        /// structureSet = storage.Execute(OQLQuery, parameters)
        /// For Each structureSetInstance As StructureSet In structureSet
        /// 	Dim name As String = structureSetInstance("Name")
        /// 	Dim street As String = structureSetInstance("Street")
        /// 	Console.WriteLine("Name: " + name + ", Street: " + street)
        /// Next
        /// structureSet.Close();
        /// </code>
        /// </example>
        /// <MetaDataID>{2317D690-9012-439C-8C11-9363A51C2BA0}</MetaDataID>
        public abstract Collections.StructureSet Execute(string OQLStatement, Collections.Generic.Dictionary<string, object> parameters);

        ///// <summary>Parse the specified query in OQLStatement parameter.
        ///// Return a StructureSet object with meta data of query statement. </summary>
        ///// <param name="OQLStatement">A String value that contains the OQL statement </param>
        ///// <MetaDataID>{CB433922-3D17-441D-863A-510594924477}</MetaDataID>
        //public abstract Collections.StructureSet Parse(string OQLStatement);

        /// <summary>The object deleted if passes all the criteria for object 
        /// deletion (Referential integrity et cetera). 
        /// If it fails to delete the object then abort the transaction. 
        /// If the object is transient then simply return. </summary>
        /// <param name="thePersistentObject">The persistent object, which will be deleted. Remember that the object has implicitly connection with storage instance. </param>
        /// <MetaDataID>{8F583B55-2366-4B74-9344-27B96729DECF}</MetaDataID>
        public static void DeleteObject(object thePersistentObject)
        {
            if (thePersistentObject == null)
                throw new System.Exception("null object can't be deleted");
            DeleteObject(thePersistentObject, DeleteOptions.EnsureObjectDeletion);
        }



        //public static bool CanDeleteObject(object thePersistentObject)
        //{
        //    if (thePersistentObject == null)
        //        throw new System.Exception("null object can't be deleted");

        //    ObjectStorage objectStorage = GetStorageOfObject(thePersistentObject);
        //    if (objectStorage == null)
        //        return true;

        //   return objectStorage.InternalCanDeleteObject(thePersistentObject);
        //}
        /// <MetaDataID>{c790449d-b163-48d7-90b6-c3b5abd72dd4}</MetaDataID>
        public abstract bool HasReferencialintegrityConstrain(object thePersistentObject);


        //  public abstract bool HasReferencialintegrityconstrain(object @object);

        /// <summary>The object deleted if passes all the criteria for object deletion (Referential integrity et cetera).
        /// If it fails to delete the object and deleteOption argument is 
        /// EnsureObjectDeletion then abort the transaction. 
        /// If the deleteOption argument is TryToDelete then simply return.  
        /// If the object is transient then simply return. </summary>
        /// <param name="thePersistentObject">The persistent object, which will be deleted. Remember that the object has implicitly connection with storage instance. </param>
        /// <param name="deleteOption">The deleteOption argument defines what persistency system do when the object delete failed. 
        /// If deleteOption is TryToDelete then there is nothing to do simply try.
        /// If deleteOption is EnsureObjectDeletion then abort transaction. </param>
        /// <MetaDataID>{F1FE60A1-72B9-4828-A6A2-51FF71EA268C}</MetaDataID>
        public static void DeleteObject(object thePersistentObject, DeleteOptions deleteOption)
        {
            ObjectStorage objectStorage = GetStorageOfObject(thePersistentObject);
            if (objectStorage == null)
                return;
            objectStorage.Delete(thePersistentObject, deleteOption);
        }



        /// <summary>Delete the object from storage. </summary>
        /// <param name="thePersistentObject">The persistent object, which will be deleted. Remember that the object has implicitly connection with storage instance. </param>
        /// <param name="deleteOption">The deleteOption argument defines what persistency system do when the object delete failed. 
        /// If deleteOption is TryToDelete then there is nothing to do simply try.
        /// If deleteOption is EnsureObjectDeletion then abort transaction. </param>
        /// <MetaDataID>{168E9EF2-9175-4D60-83B8-6F7381DF7D77}</MetaDataID>
        protected abstract void Delete(object thePersistentObject, DeleteOptions deleteOption);

        /// <summary>Some times it is more useful to use the CommitObjectState method to save the object state, 
        /// than use the object state transition technique from transactions system. 
        /// The call of CommitObjectState method is more risky, because 
        /// if something goes wrong when you change the object state the object can't roll back to
        /// original state and program can't reach to the code that call the CommitObjectState method. </summary>
        /// <param name="Object">Define the object that will commit its state. </param>
        /// <MetaDataID>{A3E4CF94-A2FE-426F-A531-01EB8C03DD92}</MetaDataID>
		public static void CommitObjectState(object Object)
        {
            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(Object))
            {
                StateTransition.Consistent = true; ;
            }
        }




        /// <summary>This method creates a new object with type that defined from parameter type. </summary>
        /// <param name="type">Define the type of object that will be create. </param>
        /// <param name="ctorParams">With ctorParams you can call other than default object constructor.
        ///  If you use the ctorParams the last parameter must be a type array with the types of 
        /// parameters of constructor, which you want to call. It is useful for type check. 
        /// You can avoid wrong constructor call. </param>
        /// <example>
        /// 	<code lang="C#">
        /// StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
        /// //Person constructor
        /// //Person(string firstName,string lastName,int age)
        /// System.Type[] parametersTypes=new System.Type[3]{typeof(string),typeof(string),typeof(int)};
        /// Person person=storage.NewObject(typeof(Person),"George","Adams",32,parametersTypes) as Person;
        /// </code>
        /// 	<code lang="Visual Basic">
        /// Dim storage As StorageSession = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
        /// 'Person constructor
        /// 'Person(string firstName,string lastName,int age)
        /// Dim parametersTypes() As Type = {GetType(String), GetType(String), GetType(Integer)}
        /// Dim person As Person = storage.NewObject(GetType(Person), "George", "Adams", 32, parametersTypes)
        /// </code>
        /// </example>
        /// <MetaDataID>{D87C2B54-1938-4A4E-8313-FF48F67DF6CD}</MetaDataID>
        public abstract object NewObject(Type type, Type[] paramsTypes, params object[] ctorParams);

        /// <MetaDataID>{dbf87504-1c89-4ee4-b951-12edb6063866}</MetaDataID>
        public abstract object NewObject(Type type);
        /// <MetaDataID>{67aed712-478b-4040-8c1f-185deef526f1}</MetaDataID>
        public T NewObject<T>()
        {
            return (T)NewObject(typeof(T));
        }

        /// <MetaDataID>{df8d0442-ccb9-4d02-98ec-bb1b475c0f3b}</MetaDataID>
        public T NewObject<T>(Type[] paramsTypes, params object[] ctorParams)
        {
            return (T)NewObject(typeof(T), paramsTypes, ctorParams);
        }

        /// <MetaDataID>{5D58FD9B-441A-4CE5-B36C-2C55BAF5FA8D}</MetaDataID>
		public static ObjectStorage OpenStorage(string StorageName, string StorageLocation, string StorageType, bool InProcess, string userName = "", string password = "")
        {

            return PersistencyService.OpenStorage(StorageName, StorageLocation, StorageType);
        }

        /// <summary>Open a session to access the storage that defined from the parameters.
        /// If something goes wrong then raise <see cref="StorageException">
        /// 	</see>. </summary>
        /// <param name="storageName">The name of Object Storage </param>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. </param>
        /// <param name="storageType">This parameter used to specify the type of new storage.
        /// Actual is the full name of Storage Provider for example 
        /// OOAdvantech.MSSQLPersistenceRunTime.StorageProvider. </param>
        /// <MetaDataID>{DDCD5802-04AA-45FE-B75F-CE8664B75863}</MetaDataID>
        public static ObjectStorage OpenStorage(string storageName, object rawStorageData, string storageType)
        {
            string storageProvider = null;
            if (StorageProviders.TryGetValue(storageType, out storageProvider))
                storageType = storageProvider;
            return PersistencyService.OpenStorage(storageName, rawStorageData, storageType);
        }



        /// <summary>
        /// Open a session to access the storage that defined from the parameters.
        /// System return the same object  if you call method more than once for the same storage
        /// If something goes wrong then raise <see cref="StorageException">
        /// 	</see>. </summary>
        /// <param name="StorageName">The name of Object Storage </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe it is a SQL server name or URL (Uniform Resource Locator). </param>
        /// <param name="storageType">This parameter used to specify the type of new storage.
        /// Actual is the full name of Storage Provider for example 
        /// OOAdvantech.MSSQLPersistenceRunTime.StorageProvider. </param>
        /// <MetaDataID>{58834B72-37CA-4F14-BDEB-3559AE0E665E}</MetaDataID>
        public static ObjectStorage OpenStorage(string StorageName, string StorageLocation, string storageType, string userName = "", string password = "")
        {

            string storageProvider = null;
            if (StorageProviders.TryGetValue(storageType, out storageProvider))
                storageType = storageProvider;


            return PersistencyService.OpenStorage(StorageName, StorageLocation, storageType, userName, password);
        }

        /// <MetaDataID>{92d67ee4-f8ed-4548-b696-accbb363d8da}</MetaDataID>
        public static void UpdateOperativeOperativeObjects(string storageIdentity)
        {
            PersistencyService.UpdateOperativeOperativeObjects(storageIdentity);
        }


        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation.
        /// The schema of storage will be the same with the OriginalStorage. </summary>
        /// <param name="storageMetadata">This parameter defines the metadata of new storage. Actually it is metadata from other storage that cloned in new storage. </param>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{81120A5D-356C-4350-B3D2-3C6A937CFA74}</MetaDataID>
		public static ObjectStorage NewStorage(Storage storageMetadata, string StorageName, string StorageLocation, string storageType)
        {
            string storageProvider = null;
            if (StorageProviders.TryGetValue(storageType, out storageProvider))
                storageType = storageProvider;
            return PersistencyService.NewStorage(storageMetadata, StorageName, StorageLocation, storageType, false);
        }
        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation. </summary>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{FFB7D927-CBEA-4E29-856A-ADE660045143}</MetaDataID>
        public static ObjectStorage NewStorage(string StorageName, string StorageLocation, string storageType, string userName = "", string password = "")
        {
            string storageProvider = null;
            if (StorageProviders.TryGetValue(storageType, out storageProvider))
                storageType = storageProvider;
            return PersistencyService.NewStorage(null, StorageName, StorageLocation, storageType, false, userName, password);
        }

        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation. </summary>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{FFB7D927-CBEA-4E29-856A-ADE660045143}</MetaDataID>
        public static ObjectStorage NewLogicalStorage(string hostingStorageName, string hostingStorageLocation, string hostingStorageType, string logicalStorageName)
        {
            string storageProvider = null;
            if (StorageProviders.TryGetValue(hostingStorageType, out storageProvider))
                hostingStorageType = storageProvider;
            return PersistencyService.NewLogicalStorage(hostingStorageName, hostingStorageLocation, hostingStorageType, logicalStorageName, false);
        }

        /// <MetaDataID>{93d2af58-307c-4ae1-bc0f-f03c9ce8a83d}</MetaDataID>
        public static ObjectStorage NewLogicalStorage(ObjectStorage hostingStorage, string logicalStorageName)
        {

            return PersistencyService.NewLogicalStorage(hostingStorage, logicalStorageName);
        }

        /// <MetaDataID>{148f0b13-fbd5-451a-9cbe-6c98456ad9c4}</MetaDataID>
        public static object GetObjectFromUri(string persistentUri)
        {
            if(persistentUri == null) 
                return null;
            object @object = null;
            string[] persistentObjectUriParts = persistentUri.Split('\\');
            string storageIdentity = persistentObjectUriParts[0];
            var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(storageIdentity);
            if (storageMetaData != null && storageMetaData.MultipleObjectContext)
            {
                var objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
                @object = objectStorage.GetObject(persistentUri);
            }
            return @object;
        }
        public static ObjectStorage OpenStorage(string storageIdentity)
        {
            var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(storageIdentity);
            if (storageMetaData == null)
            {
                throw new StorageException("There isn't public record for the objects storage with this identity.",
                   StorageException.ExceptionReason.StorageDoesnotExist);
            }

            if (!storageMetaData.MultipleObjectContext)
                throw new StorageException("Storage cannot be run in multiple object context", StorageException.ExceptionReason.StorageOpensOnlyInSingleObjectContext);

            var objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
            return objectStorage;

        }

        /// <MetaDataID>{2c7035bf-37a9-4ef4-926f-8fe64a7656ed}</MetaDataID>
        public static T GetObjectFromUri<T>(string persistentUri) where T : class
        {
            if (persistentUri == null)
                return default(T);
            object @object = null;
            string[] persistentObjectUriParts = persistentUri.Split('\\');
            string storageIdentity = persistentObjectUriParts[0];
            var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(storageIdentity);
            if (storageMetaData != null && storageMetaData.MultipleObjectContext)
            {
                var objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
                @object = objectStorage.GetObject(persistentUri);
            }
            return @object as T;
        }

        /// <MetaDataID>{3a304faa-ec3f-4349-ad9d-cd1694034139}</MetaDataID>
        public static MetaDataRepository.StorageMetaData GetStorageFromUri(string persistentUri)
        {
            object @object = null;
            string[] persistentObjectUriParts = persistentUri.Split('\\');
            string storageIdentity = persistentObjectUriParts[0];
            var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(storageIdentity);
            return storageMetaData;
        }

        /// <MetaDataID>{703ea44e-ca59-40cb-9195-b0b76a00ca19}</MetaDataID>
        public abstract object GetObject(string persistentUri);


        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation. </summary>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="StorageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <param name="InProcess">Define the possibility to the objects of storage to run in process
        /// which open a session with storage. 
        /// This means that if two processes try to open session with the same storage only one will gain the control. 
        /// All storage types doesn't allow this possibility, some allow and others not. </param>
        /// <MetaDataID>{0FEAEFED-3186-45DB-BFD3-DF5A3906F8F8}</MetaDataID>
        public static ObjectStorage NewStorage(string StorageName, string StorageLocation, string storageType, bool InProcess)
        {

            return PersistencyService.NewStorage(null, StorageName, StorageLocation, storageType, InProcess);
        }

        /// <MetaDataID>{ed20c440-2c81-4d20-8a19-26e9e6d27087}</MetaDataID>
        public static void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool InProcess, string userName = "", string password = "", bool overrideObjectStorage = false)
        {
            PersistencyService.Restore(archive, storageName, storageLocation, storageType, InProcess, userName, password, overrideObjectStorage);
        }

        /// <MetaDataID>{edb1711c-b65b-485c-b3ea-2dd730dc8246}</MetaDataID>
        public static void Repair(string storageName, string storageLocation, string storageType, bool InProcess)
        {
            PersistencyService.Repair(storageName, storageLocation, storageType, InProcess);
        }




        /// <MetaDataID>{8E289434-78E9-4513-AF65-22A82EC3922C}</MetaDataID>
        /// <param name="StorageName">Define the name of new storage. </param>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. </param>
        public static ObjectStorage NewStorage(string StorageName, object rawStorageData, string storageType)
        {
            string storageProvider = null;
            if (StorageProviders.TryGetValue(storageType, out storageProvider))
                storageType = storageProvider;
            return PersistencyService.NewStorage(null, StorageName, rawStorageData, storageType);
        }




        /// <MetaDataID>{cc5a29aa-347a-42d4-a204-f03f658a31fb}</MetaDataID>
        public static List<string> GetStorageProviders()
        {
            return new List<string>(StorageProviders.Keys);
        }




        /// <MetaDataID>{6219a9c7-8ce3-477c-a450-263a9d53d3e3}</MetaDataID>
        internal protected static System.Collections.Generic.Dictionary<string, string> StorageProviders = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"Microsoft SQL Server","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"},
            {"Microsoft SQL Server Compact","OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider"},
            {"XML File","OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"},
            {"Oracle Database","OOAdvantech.OraclePersistenceRunTime.StorageProvider"},
            {"In Process Microsoft SQL Server","OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider"},
            {"In Process Oracle Database","OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider"}
        };



        /// <summary>This method can be used in the case where you want to create a transient 
        /// object and later decide to store in storage. 
        /// The process of transient object must be the same as the process that 
        /// runs the others object of storage. </summary>
        /// <param name="Object">Define the object that will be persistent. </param>
        /// <MetaDataID>{0999AB00-EDD9-4513-BEA5-0665A5832D61}</MetaDataID>
        public abstract void CommitTransientObjectState(object Object);
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{924FEFB9-B8A0-46A6-AFCF-95A7B19D357F}</MetaDataID>
        protected static IPersistencyService _PersistencyService;

        /// <MetaDataID>{e2f6ca57-f1e1-4658-bd48-72f162679617}</MetaDataID>
        static object PersistencyServiceLock = new object();

        /// <summary>Used from static method of storage session class to
        /// communicate with core object of persistence system. 
        /// It is useful for storage creation or for opening a storage session
        /// with an existing storage. </summary>
        /// <MetaDataID>{53EE346D-3ED3-4282-BA4F-37357A6E5F45}</MetaDataID>
        public static IPersistencyService PersistencyService
        {
            set
            {
            }
            get
            {
                lock (PersistencyServiceLock)
                {

                    if (null == _PersistencyService)
                    {

#if !DeviceDotNet

                        //"PersistenceLayerRunTime, Culture=neutral, PublicKeyToken=95eeb2468d93212b"

                        if (System.Environment.Version.Major == 4)
                        {

                            _PersistencyService = (IPersistencyService)MonoStateClass.GetInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"), true);
                        }
                        else
                        {

                            _PersistencyService = (IPersistencyService)MonoStateClass.GetInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"), true);

                        }
#else

                        _PersistencyService = (IPersistencyService)AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    

#endif
                    }
                }
                return _PersistencyService;
            }
        }


        /// <summary>This method retrieves a storage session with storage of object if it is persistent. If the object is transient return null. </summary>
        /// <param name="_object">Define the object from which you wand retrieve the storage session. </param>
        /// <MetaDataID>{5E8CE175-8E8A-424E-801A-2BCED2AA4816}</MetaDataID>
        public static ObjectStorage GetStorageOfObject(object _object)
        {
            bool remoteObject = false;
#if !DeviceDotNet
            remoteObject = Remoting.RemotingServices.IsOutOfProcess(_object as System.MarshalByRefObject);
#endif

            if (_object is MarshalByRefObject && remoteObject)
                return PersistencyService.GetStorageOfObject(_object);
            else
            {
                // Error prone πρόβλημα αν το object είναι σε άλλο process και το ExtensionProperties member δε είναι public
                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object);
                // Error prone Client StorageSession εαν είναι remote
                if (storageInstanceRef != null)
                {
#if !DeviceDotNet
                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageInstanceRef) || OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageInstanceRef.ObjectStorage))
                        return AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ClientStorageSession", ""), new System.Type[1] { typeof(PersistenceLayer.ObjectStorage) }, storageInstanceRef.ObjectStorage) as PersistenceLayer.ObjectStorage;
#endif
                    return storageInstanceRef.ObjectStorage;
                }
                else
                    return null;
            }
        }

        /// <MetaDataID>{214d0009-ed00-4e3c-bd7f-7f72f2d91b7d}</MetaDataID>
        public static ObjectStorage GetStorage(OOAdvantech.PersistenceLayer.Storage storageMetaData)
        {
            var objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(storageMetaData);
            if (objectStorage == null)
                return OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
            else
                return objectStorage;
            bool remoteObject = false;
            //#if !DeviceDotNet
            //            remoteObject = Remoting.RemotingServices.IsOutOfProcess(_object as System.MarshalByRefObject);
            //#endif

            //            if (_object is System.MarshalByRefObject && remoteObject)
            //                return PersistencyService.GetStorageOfObject(_object);
            //            else
            //            {
            //                // Error prone πρόβλημα αν το object είναι σε άλλο process και το ExtensionProperties member δε είναι public
            //                StorageInstanceRef storageInstanceRef = OOAdvantech.ExtensionProperties.GetExtensionPropertiesFromObject(_object).StorageInstanceRef;
            //                // Error prone Client StorageSession εαν είναι remote
            //                if (storageInstanceRef != null)
            //                {
            //#if !DeviceDotNet
            //                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageInstanceRef) || OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageInstanceRef.ObjectStorage))
            //                        return AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ClientStorageSession", ""), new System.Type[1] { typeof(PersistenceLayer.ObjectStorage) }, storageInstanceRef.ObjectStorage) as PersistenceLayer.ObjectStorage;
            //#endif
            //                    return storageInstanceRef.ObjectStorage;
            //                }
            //                else
            //                    return null;
            //            }
        }
        /// <summary>This method retrieves a storage session with storage of object if it is persistent from extension properties.
        /// If the object is transient return null. The extension properties 
        /// collection is a link of object with persistency subsystem or 
        /// transaction subsystem or other functional areas of 
        /// OOAdvance system. </summary>
        /// <param name="extensionProperties">The extension properties collection is a link of object with persistency subsystem or transaction subsystem or other functional areas of OOAdvance system. </param>
        /// <MetaDataID>{46FCBEE8-D414-43A0-A676-6D8119EA7931}</MetaDataID>
		public static ObjectStorage GetStorageOfObject(OOAdvantech.ObjectStateManagerLink extensionProperties)
        {
            if (extensionProperties == null)
                return null;
            StorageInstanceRef storageInstanceRef = extensionProperties.StorageInstanceRef;
            if (storageInstanceRef != null)
                return storageInstanceRef.ObjectStorage;
            else
                return null;
        }



        /// <summary>This property defines the Meta data of storage. </summary>
        /// <MetaDataID>{68FC876D-EFC6-4D30-A585-C9FA99258BF8}</MetaDataID>
        public abstract Storage StorageMetaData
        {

            get;


        }

        /// <summary>Executes the specified query in OQLStatement parameter </summary>
        /// <param name="OQLStatement">A String value that contains the OQL statement </param>
        /// <returns>Return a StructureSet object that contain the result of object query statement. </returns>
        /// <example>
        /// 	<code lang="C#">
        /// StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
        /// string OQLQuery="SELECT person.Name Name,person.Address.Street Street "+
        /// "FROM Person person "+
        /// "WHERE person.Parent.Address.City=person.City";
        /// StructureSet structureSet=storage.Execute(OQLQuery);
        /// foreach(StructureSet structureSetInstance in StructureSet)
        /// {
        /// 	string name=structureSetInstance["Name"] as string;
        /// 	string street=structureSetInstance["Street"] as string;
        /// 	System.Console.WriteLine("Name: "+name+", Street: "+street);
        /// }
        /// structureSet.Close();
        /// </code>
        /// 	<code lang="Visual Basic">
        /// Dim storage As StorageSession
        /// Dim structureSet As StructureSet
        /// Dim OQLQuery As String
        /// storage = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
        /// OQLQuery = "SELECT person.Name Name,person.Address.Street Street "
        /// OQLQuery += "FROM Person person "
        /// OQLQuery += "WHERE person.Parent.Address.City=person.City"
        /// structureSet = storage.Execute(OQLQuery)
        /// For Each structureSetInstance As StructureSet In structureSet
        /// 	Dim name As String = structureSetInstance("Name")
        /// 	Dim street As String = structureSetInstance("Street")
        /// 	Console.WriteLine("Name: " + name + ", Street: " + street)
        /// Next
        /// structureSet.Close();
        /// </code>
        /// </example>
        /// <MetaDataID>{0B88011C-F845-40FA-80B8-187594490D19}</MetaDataID>
        public abstract Collections.StructureSet Execute(string OQLStatement);


        /// <MetaDataID>{17ab0210-e98f-4550-bb22-f6ab92882f8d}</MetaDataID>
        public abstract object GetObjectID(object persistentObject);
    }






    /// <MetaDataID>{ed1f9885-e3cc-4675-b1db-afaf66897e70}</MetaDataID>
    public static class ObjectStorageExtraOperators
    {
        //public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> value);

        /// <MetaDataID>{faad62c1-2a3a-4d76-be04-950a02519378}</MetaDataID>
        public static void MoveObject<TSource, TResult>(this ObjectStorage objectStorage, TSource source, System.Linq.Expressions.Expression<Func<TSource, TResult>> expression) where TSource : class
        {

        }
        /// <MetaDataID>{9223bba1-5e69-4c5b-aae1-de2d596aeb65}</MetaDataID>
        public static void MoveObject<TSource, TResult>(this ObjectStorage objectStorage, TSource source, System.Linq.Expressions.Expression<Func<TSource, TResult>> expression, System.Linq.Expressions.Expression<Func<TSource, TResult>> extraExpression) where TSource : class
        {

        }



    }
    /// <MetaDataID>{106ba9a6-5bd8-418a-8e12-20313b5606a5}</MetaDataID>
    public static class ObjectStorageMoveOptionsExtraOperators
    {
        /// <MetaDataID>{1440f04b-affe-4970-a280-0bdffaff05a6}</MetaDataID>
        public static TSource GetInside<TSource>(this TSource source, params object[] list) { return default(TSource); }
        /// <MetaDataID>{dc1c6433-76f5-4768-9895-475dd550b154}</MetaDataID>
        public static TSource LetOut<TSource>(this TSource source, params object[] list) { return default(TSource); }
    }


}
