using OOAdvantech.Linq;
using System.Linq;
namespace OOAdvantech.RDBMSMetaDataRepository
{
    using System;
    using MetaDataRepository;
    /// <MetaDataID>{4E02E4EA-E7FA-40D5-A290-81B4BB6BF530}</MetaDataID>
    [BackwardCompatibilityID("{4E02E4EA-E7FA-40D5-A290-81B4BB6BF530}")]
    [Persistent()]
    public class Storage : MetaDataRepository.Storage
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(StorageMetaObjects))
            {
                if (value == null)
                    StorageMetaObjects = default(System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    StorageMetaObjects = (System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(StorageMetaObjects))
                return StorageMetaObjects;


            return base.GetMemberValue(token, member);
        }

        public virtual string TablePrefix
        {
            get
            {
                return "T_";
            }
        }

        public virtual string CompositeNameSeparatorSign
        {
            get
            {
                return "_";
            }
        }

        virtual public bool SupportStoreProcedures { get => true; }
        virtual public bool SupportForeignKeys{ get => true; }

        virtual public bool SupportPrimaryKeys { get => true; }


        virtual public bool SupportViews { get => true; }
        /// <MetaDataID>{b1da1e62-b417-4382-aa3a-ec3c46100647}</MetaDataID>
        protected Storage()
        {

        }
        /// <MetaDataID>{332b927a-973c-4b8f-9bae-070b26bf2699}</MetaDataID>
        public Storage(string name)
        {
            _Name = name;
        }
        /// <MetaDataID>{b558433a-4ca7-45b6-bdbd-356707e3cb8a}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{be341a04-05a9-42d6-8701-8a942c45f3b6}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{18842386-aca1-4946-a505-a76ebbc2614a}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{0aa12342-ce25-4d6e-b120-4fda8a88a0e9}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            System.Xml.Linq.XDocument mappingDataDocument = null;
            if (!string.IsNullOrEmpty(mappingDataResourceName))
            {
                foreach (string name in dotNetAssembly.GetManifestResourceNames())
                {
                    if (name.IndexOf(string.Format("Mapping_Contexts.{0}.xml", mappingDataResourceName)) != -1)
                    {

                        using (System.IO.Stream stream = dotNetAssembly.GetManifestResourceStream(name))
                        {
                            mappingDataDocument = System.Xml.Linq.XDocument.Load(stream);
                        }
                        break;
                    }
                }
            }
            if (mappingDataDocument != null)
                RegisterComponent(assemblyFullName, mappingDataDocument);
            else
                RegisterComponent(assemblyFullName, default(System.Xml.Linq.XDocument));
        }
        /// <MetaDataID>{40708b79-ba34-423d-85ee-aa52c9d9d8ff}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames, System.Collections.Generic.Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{c6e87341-5b67-4c03-8a56-d19e28ef4fb8}</MetaDataID>
		public override void MakeNameUnaryInNamesapce(MetaObject metaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                bool ensureUnaryName = true;
                string unaryName = metaObject.Name;
                int count = 1;
                while (ensureUnaryName)
                {
                    ensureUnaryName = false;
                    foreach (MetaDataRepository.MetaObject ownedElement in _OwnedElements)
                    {
                        if (ownedElement == metaObject)
                            continue;
                        if (ownedElement.Name.Trim().ToLower() == metaObject.Name.Trim().ToLower())
                        {
                            metaObject.Name = unaryName + "_" + count.ToString();
                            ensureUnaryName = true;
                            count++;
                            break;
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }



        /// <MetaDataID>{7e53e853-53ba-4ceb-bcd6-ac1bfdfaba9f}</MetaDataID>
		public MetaDataRepository.MetaObject GetEquivalentMetaObject(string Identity, System.Type MetaObjectType)
        {
            var storageMetaObjects = GetStorageMetaObjects();
            //if (StorageMetaObjects == null)
            //{
            //    StorageMetaObjects =new System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();

            //    string query = "SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject ";
            //    Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(query);
            //    foreach (Collections.StructureSet Rowset in structureSet)
            //    {
            //        MetaDataRepository.MetaObject eqMetaObject = Rowset["MetaObject"] as MetaDataRepository.MetaObject;
            //        if (!StorageMetaObjects.ContainsKey(eqMetaObject.Identity.ToString()))
            //            StorageMetaObjects.Add(eqMetaObject.Identity.ToString(), eqMetaObject);
            //        else
            //        {

            //        }
            //    }
            //}
            if (storageMetaObjects.ContainsKey(Identity))
                return storageMetaObjects[Identity];



            //string Query=null;
            //         Collections.StructureSet aStructureSet = null;



            //if(MetaObjectType ==typeof(MetaDataRepository.Classifier))
            //{
            //             Query = "SELECT Classifier FROM " + typeof(MetaDataRepository.Classifier).FullName + " Classifier WHERE Classifier.MetaObjectIDStream = \"" + Identity + "\"";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		MetaDataRepository.Classifier classifier =Rowset["Classifier"] as MetaDataRepository.Classifier;
            //                 if (!StorageMetaObjects.ContainsKey(Identity))
            //                     StorageMetaObjects.Add(Identity, classifier);
            //                 else
            //                 {
            //                 }
            //		return classifier ;
            //	}
            //	return null;
            //}


            //if(MetaObjectType ==typeof(MetaDataRepository.Class))
            //{
            //             Query = "SELECT Class FROM " + typeof(RDBMSMetaDataRepository.Class).FullName + " Class WHERE Class.MetaObjectIDStream = \"" + Identity + "\"";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		RDBMSMetaDataRepository.Class  Class=Rowset["Class"] as RDBMSMetaDataRepository.Class;
            //                 if (!StorageMetaObjects.ContainsKey(Identity))
            //                     StorageMetaObjects.Add(Identity, Class);
            //                 else
            //                 {
            //                 }
            //		return Class;
            //	}
            //	return null;
            //}
            //if(MetaObjectType ==typeof(MetaDataRepository.AssociationEnd))
            //{
            //             Query = "SELECT AssociationEnd FROM " + typeof(RDBMSMetaDataRepository.AssociationEnd).FullName + " AssociationEnd WHERE AssociationEnd.MetaObjectIDStream = \"" + Identity + "\"";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		RDBMSMetaDataRepository.AssociationEnd  AssociationEnd=Rowset["AssociationEnd"] as RDBMSMetaDataRepository.AssociationEnd;
            //                 if (!StorageMetaObjects.ContainsKey(Identity))
            //                     StorageMetaObjects.Add(Identity, AssociationEnd);
            //                 else
            //                 {
            //                 }

            //		return AssociationEnd;
            //	}
            //	return null;

            //}
            //if(MetaObjectType ==typeof(MetaDataRepository.Association))
            //{
            //             Query = "SELECT Association FROM " + typeof(RDBMSMetaDataRepository.Association).FullName + " Association WHERE Association.MetaObjectIDStream = \"" + Identity + "\"";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		RDBMSMetaDataRepository.Association  Association=Rowset["Association"] as RDBMSMetaDataRepository.Association;
            //                 if (!StorageMetaObjects.ContainsKey(Identity))
            //                     StorageMetaObjects.Add(Identity, Association);
            //                 else
            //                 {
            //                 }

            //		return Association;
            //	}
            //	return null;
            //}
            //if(MetaObjectType ==typeof(RDBMSMetaDataRepository.StorageCellReference))
            //{
            //             Query = "SELECT StorageCell FROM " + typeof(RDBMSMetaDataRepository.StorageCellReference).FullName + " StorageCell WHERE StorageCell.MetaObjectIDStream = \"" + Identity + "\"";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		RDBMSMetaDataRepository.StorageCellReference StorageCell=Rowset["StorageCell"] as RDBMSMetaDataRepository.StorageCellReference;
            //                 StorageMetaObjects[Identity] = StorageCell;
            //		return StorageCell;
            //	}


            //	Query ="SELECT StorageCell FROM "+typeof(RDBMSMetaDataRepository.StorageCellReference).FullName+" StorageCell ";
            //	aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //             foreach (Collections.StructureSet Rowset in aStructureSet)
            //	{
            //		RDBMSMetaDataRepository.StorageCellReference StorageCell=Rowset["StorageCell"] as RDBMSMetaDataRepository.StorageCellReference;
            //		int StorageCellID=StorageCell.SerialNumber;
            //		string  StorageCellIDStr="ref_"+StorageCell.StorageIdentity+"_"+StorageCellID.ToString();
            //		if(StorageCellIDStr==Identity)
            //			return StorageCell;
            //	}
            //	return null;
            //}



            //         Query = "SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject WHERE MetaObject.MetaObjectIDStream = \"" + Identity + "\"";
            //aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //         foreach (Collections.StructureSet Rowset in aStructureSet)
            //{
            //             MetaDataRepository.MetaObject metaObject = Rowset["MetaObject"] as MetaDataRepository.MetaObject;
            //             if (!StorageMetaObjects.ContainsKey(Identity))
            //                 StorageMetaObjects.Add(Identity, metaObject);
            //             else
            //             {
            //             }

            //             return metaObject;
            //}
            return null;

        }
        /// <MetaDataID>{67fa99d5-e695-477c-9b07-faca070ff7f2}</MetaDataID>
        public void Refresh()
        {
            StorageMetaObjects = null;
        }
        /// <MetaDataID>{d842c844-829b-48d2-add0-c594239ff9f2}</MetaDataID>
        protected System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> StorageMetaObjects;
        /// <MetaDataID>{7e0474aa-b9d0-48cb-969e-815581d58d39}</MetaDataID>
		public MetaDataRepository.MetaObject GetEquivalentMetaObject(MetaDataRepository.MetaObject metaObject)
        {
            if (metaObject == null)
                throw new System.Exception("there isn't equivalent MetaObject for null MetaObject");
            System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> storageMetaObjects = GetStorageMetaObjects();


            MetaDataRepository.MetaObject equivaletnMetaObject = null;
            if (storageMetaObjects.TryGetValue(metaObject.Identity.ToString(), out equivaletnMetaObject))
                return equivaletnMetaObject;
            else
                return null;


            //string Query = null;
            //Collections.StructureSet aStructureSet = null;
            //if (metaObject is MetaDataRepository.Class)
            //{
            //    Query = "SELECT Class FROM " + typeof(RDBMSMetaDataRepository.Class).FullName + " Class WHERE Class.MetaObjectIDStream = \"" + metaObject.Identity + "\"";
            //    aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //    foreach (Collections.StructureSet Rowset in aStructureSet)
            //    {
            //        RDBMSMetaDataRepository.Class _class = Rowset["Class"] as RDBMSMetaDataRepository.Class;
            //        StorageMetaObjects[metaObject.Identity.ToString()] = _class;
            //        return _class;
            //    }
            //    return null;
            //}
            //if (metaObject is MetaDataRepository.AssociationEnd)
            //{
            //    Query = "SELECT AssociationEnd FROM " + typeof(RDBMSMetaDataRepository.AssociationEnd).FullName + " AssociationEnd WHERE AssociationEnd.MetaObjectIDStream = \"" + metaObject.Identity + "\"";
            //    aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //    foreach (Collections.StructureSet Rowset in aStructureSet)
            //    {
            //        RDBMSMetaDataRepository.AssociationEnd associationEnd = Rowset["AssociationEnd"] as RDBMSMetaDataRepository.AssociationEnd;
            //        StorageMetaObjects[metaObject.Identity.ToString()] = associationEnd;
            //        return associationEnd;
            //    }
            //    StorageMetaObjects[metaObject.Identity.ToString()] = null;
            //    return null;

            //}
            //if (metaObject is MetaDataRepository.Association)
            //{
            //    Query = "SELECT Association FROM " + typeof(RDBMSMetaDataRepository.Association).FullName + " Association WHERE Association.MetaObjectIDStream = \"" + metaObject.Identity + "\"";
            //    aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //    foreach (Collections.StructureSet Rowset in aStructureSet)
            //    {
            //        RDBMSMetaDataRepository.Association association = Rowset["Association"] as RDBMSMetaDataRepository.Association;
            //        StorageMetaObjects[metaObject.Identity.ToString()] = association;
            //        return association;
            //    }
            //    StorageMetaObjects[metaObject.Identity.ToString()] = null;
            //    return null;
            //}

            //Query = "SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject WHERE MetaObject.MetaObjectIDStream = \"" + metaObject.Identity + "\"";
            //aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(Query);
            //foreach (Collections.StructureSet Rowset in aStructureSet)
            //{
            //    MetaDataRepository.MetaObject eqMetaObject = Rowset["MetaObject"] as MetaDataRepository.MetaObject;
            //    StorageMetaObjects[metaObject.Identity.ToString()] = eqMetaObject;
            //    return eqMetaObject;
            //}
            //StorageMetaObjects[metaObject.Identity.ToString()] = null;
            //return null;
        }

        private System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> GetStorageMetaObjects()
        {
            lock (this)
            {
                if (StorageMetaObjects == null)
                    StorageMetaObjects = new System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();
            }
            lock (StorageMetaObjects)
            {

                if (StorageMetaObjects.Count == 0)
                {
                    //StorageMetaObjects = new System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();

                    Linq.Storage metaDataStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties));
                    //(from metaObject in metaDataStorage.GetObjectCollection< MetaDataRepository.MetaObject >()
                    //select metaObject).to
                    //System.Collections.Generic.List<MetaObject> metas = new System.Collections.Generic.List<MetaObject>();
                    string query = "SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject ";
                    Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute(query);
                    foreach (Collections.StructureSet Rowset in structureSet)
                    {
                        MetaDataRepository.MetaObject eqMetaObject = Rowset["MetaObject"] as MetaDataRepository.MetaObject;
                        StorageMetaObjects[eqMetaObject.Identity.ToString()] = eqMetaObject;

                    }
                }
                return StorageMetaObjects;
            }
        }

        public override bool CheckForVersionUpgrate(string assemblyFullName)
        {
            DotNetMetaDataRepository.Assembly mAssembly = null;
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");
            mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;

            OOAdvantech.Linq.Storage linqStorage = new Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));
            var storageComponent = (from component in linqStorage.GetObjectCollection<RDBMSMetaDataRepository.Component>() select component).ToList().Where(x => x.Identity.ToString() == mAssembly.Identity.ToString()).FirstOrDefault();
            if (storageComponent == null )
                return true;

            Version storageComponentMappingVersion = null;
            if (!Version.TryParse(storageComponent.MappingVersion, out storageComponentMappingVersion))
                storageComponentMappingVersion = new Version();

            Version componentMappingVersionVersion = null;
            if (!Version.TryParse(mAssembly.MappingVersion, out componentMappingVersionVersion))
                componentMappingVersionVersion = new Version();

            if (componentMappingVersionVersion > storageComponentMappingVersion)
                return true;

            return false;
        }
    }
}
