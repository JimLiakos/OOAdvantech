using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{5263e16d-290b-456c-b85f-dd26356b168c}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{5263e16d-290b-456c-b85f-dd26356b168c}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class Storage : RDBMSMetaDataRepository.Storage
    {
        public const string DevelopmentStorage = "UseDevelopmentStorage=true";
        /// <MetaDataID>{b682ce60-ea0e-4d75-bd23-743af87b5546}</MetaDataID>
        protected Storage()
        {
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
        }
        public override bool SupportStoreProcedures => false;

        public override bool SupportViews => false;

        public override bool SupportForeignKeys { get => false; }

        public override bool SupportPrimaryKeys { get => false; }

        /// <MetaDataID>{802c5d63-6d2b-4a6a-bfc4-5511b1dac7fe}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, StorageMetadata azureStorageMetadata)
        {
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
            _StorageName = storageName;
            _StorageLocation = storageLocation;
            _StorageType = storageType;
            AzureStorageMetadata = azureStorageMetadata;

        }
        static internal string GetTablePrefixFor(string storagePrefix)
        {
            return "T" + storagePrefix;
        }
        public override string TablePrefix
        {
            get
            {
                return "T" + AzureStorageMetadata.StoragePrefix;
            }
        }

        public override string CompositeNameSeparatorSign
        {
            get
            {
                return "";
            }
        }


        internal StorageMetadata AzureStorageMetadata;

        /// <MetaDataID>{7b25bc4d-c94d-43f8-987b-d9d9d904beef}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ed157954-bff6-4a4d-a28c-934c00fec41b}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, System.Collections.Generic.List<string> types = null)
        {
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;

            //DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            //if (mAssembly == null)
            //    mAssembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
            System.Collections.Generic.List<MetaDataError> errors = new System.Collections.Generic.List<MetaDataError>();
            bool hasErrors = mAssembly.ErrorCheck(ref errors);
            if (hasErrors)
            {
                string ErrorMessage = null;
                foreach (MetaDataError error in errors)
                {
                    if (ErrorMessage != null)
                        ErrorMessage += "\n";
                    ErrorMessage += error.ErrorMessage;
                }
                throw new System.Exception(ErrorMessage);
            }
            Azure.Data.Tables.TableServiceClient tablesAccount = (ObjectStorage.GetStorageOfObject(this) as WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ObjectStorage).TablesAccount;
            Azure.Data.Tables.TableClient storagesMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");
            BeginSynchronous(storagesMetadataTable_a);

            //CloudStorageAccount account = (ObjectStorage.GetStorageOfObject(this) as WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ObjectStorage).Account;
            //CloudTableClient cloudTablesClient = account.CreateCloudTableClient();
            //CloudTable storagesMetadataTable = cloudTablesClient.GetTableReference("StoragesMetadata");
            //BeginSynchronous(storagesMetadataTable);
            try
            {
                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {
                    RegisterComponent(mAssembly);

                    StorageMetaObjects = null;//Force system to update cache of storage meta objects

                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                //EndSynchronous(storagesMetadataTable);
                EndSynchronous(storagesMetadataTable_a);
            }


        }






        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, XDocument> assembliesMappingData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{71544717-9d54-4a6d-b5e3-54cb030a096d}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName, System.Collections.Generic.List<string> types = null)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{1ac18616-b3cc-4325-8f9f-eaec9f9d153c}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, XDocument mappingData)
        {
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
            MetaDataRepository.SynchronizerSession.StartSynchronize();

            RDBMSMetaDataRepository.Component mComponent = null;
            mComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mAssembly, this) as RDBMSMetaDataRepository.Component;


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var @class in mComponent.Residents.OfType<RDBMSMetaDataRepository.Class>().Where(x => x.Persistent))
                {
                    foreach (var column in @class.ActiveStorageCell.MainTable.ContainedColumns)
                    {
                        if (column.MappedAttribute != null && @class.GetAttributeRealization(column.MappedAttribute) != null)
                            column.MappedAttributeRealizationIdentity = @class.GetAttributeRealization(column.MappedAttribute).Identity.ToString();

                        if (column.MappedAssociationEnd != null && column.MappedAssociationEnd.GetOtherEnd() != null && @class.GetAssociationEndRealization(column.MappedAssociationEnd.GetOtherEnd()) != null)
                            column.MappedAssociationEndRealizationIdentity = @class.GetAssociationEndRealization(column.MappedAssociationEnd.GetOtherEnd()).Identity.ToString();

                    }
                }

                stateTransition.Consistent = true;
            }

        }

        public void RegisterComponent(MetaDataRepository.Component Component, System.Collections.Generic.List<string> types = null)
        {



            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {

                try
                {
                    if (_StorageIdentity == null)
                    {
                        _StorageIdentity = System.Guid.NewGuid().ToString();
                        PersistenceLayer.ObjectStorage.CommitObjectState(this);
                    }


                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();





                    MetaDataRepository.SynchronizerSession.StartSynchronize();

                    RDBMSMetaDataRepository.Component mComponent = null;
                    mComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(Component, this) as RDBMSMetaDataRepository.Component;
                    if (mComponent == null)
                    {
                        mComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(Component, this);
                        mComponent.Context = this;
                        _Components.Add(mComponent);
                    }



                    System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, OOAdvantech.MetaDataRepository.MetaObject> dependencies = new System.Collections.Generic.Dictionary<MetaDataRepository.MetaObject, MetaDataRepository.MetaObject>();
                    GetAllDependencies(ref dependencies, Component);

                    foreach (var entry in dependencies)
                    {
                        MetaDataRepository.Component referenceComponent = entry.Value as MetaDataRepository.Component;
                        RDBMSMetaDataRepository.Component rdbmsReferenceComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(referenceComponent, this) as RDBMSMetaDataRepository.Component;
                        if (rdbmsReferenceComponent == null)
                        {
                            rdbmsReferenceComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(referenceComponent, this);
                            rdbmsReferenceComponent.Context = this;
                            _Components.Add(rdbmsReferenceComponent);
                        }
                    }

                    //CheckForUnusedMetaData();
                    mComponent.Synchronize(Component);

                    string myName = (string)mComponent.GetPropertyValue(typeof(string), "Persosnal", "Myname");
                    mComponent.PutPropertyValue("Persosnal", "Myname", "mitsos");

                    MetaDataRepository.SynchronizerSession.StopSynchronize();


                    MetaDataRepository.SynchronizerSession.StartSynchronize();
                    mComponent.BuildMappingElement(this, null);

                    foreach (var _class in mComponent.Residents.OfType<RDBMSMetaDataRepository.Class>().Where(x => !x.Abstract && x.Persistent))
                    {
                        foreach (var column in _class.StorageCells.OfType<RDBMSMetaDataRepository.StorageCell>().SelectMany(x => x.MappedTables).SelectMany(x => x.ContainedColumns).ToList())
                        {
                            if (!column.DataBaseColumnNameHasValue)
                                column.DataBaseColumnName = column.Name;
                        }
                    }

                    UpdateRelations();

                    MetaDataRepository.SynchronizerSession.StopSynchronize();

                    CheckForUnusedMetaData();

                    StateTransition.Consistent = true; ;
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
            }


        }

        private void CheckForUnusedMetaData()
        {
            Dictionary<string, List<MetaObject>> metaObjects = new Dictionary<string, List<MetaObject>>();
            List<MetaObject> storageMetaObjects = new List<MetaObject>();

            Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(this).Execute("SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject ");//WHERE MetaObjectIDStream = \""+OriginMetaObject.Identity.ToString()+"\" ");

            foreach (Collections.StructureSet Rowset in aStructureSet)
            {
                MetaDataRepository.MetaObject metaObject = (MetaDataRepository.MetaObject)Rowset["MetaObject"];
                storageMetaObjects.Add(metaObject);

            }

            var keys = storageMetaObjects.OfType<RDBMSMetaDataRepository.Key>().ToList();

            var storageCells = storageMetaObjects.OfType<RDBMSMetaDataRepository.StorageCell>().Where(x => x.MappedTables.Count == 0).ToList();


            List<RDBMSMetaDataRepository.Column> activeColumns = new List<RDBMSMetaDataRepository.Column>();

            activeColumns.AddRange((from table in storageMetaObjects.OfType<RDBMSMetaDataRepository.Table>()
                                    from identityColumn in table.ObjectIDColumns
                                    select identityColumn).ToList());
            activeColumns.AddRange((from table in storageMetaObjects.OfType<RDBMSMetaDataRepository.Table>()
                                    where table.ReferentialIntegrityColumn != null
                                    select table.ReferentialIntegrityColumn).ToList());

            activeColumns.AddRange((from table in storageMetaObjects.OfType<RDBMSMetaDataRepository.Table>()
                                    from column in table.ContainedColumns
                                    select column).ToList());

            activeColumns.AddRange((from view in storageMetaObjects.OfType<RDBMSMetaDataRepository.View>()
                                    from column in view.ViewColumns
                                    select column).ToList());

            var freeColumns = storageMetaObjects.OfType<RDBMSMetaDataRepository.Column>().Where(x => !activeColumns.Contains(x)).ToList();
            freeColumns.AddRange(storageMetaObjects.OfType<RDBMSMetaDataRepository.IdentityColumn>().Where(x => !activeColumns.Contains(x)).ToList());
            if (freeColumns.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage generlizations.");
            foreach (var column in freeColumns)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(column);
            }



            var generlizations = storageMetaObjects.OfType<MetaDataRepository.Generalization>().Where(x => x.Child == null || x.Parent == null).ToList();

            if (generlizations.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage generlizations.");

            foreach (var generlization in generlizations)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(generlization);
            }

            var realizations = storageMetaObjects.OfType<MetaDataRepository.Realization>().Where(x => x.Abstarction == null || x.Implementor == null).ToList();
            if (realizations.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage realizations.");




            foreach (var realization in realizations)
            {
                PersistenceLayer.ObjectStorage.DeleteObject(realization);
            }
            realizations = storageMetaObjects.OfType<MetaDataRepository.Realization>().Where(x => x.Abstarction?.ImplementationUnit == null || (x.Implementor as MetaObject)?.ImplementationUnit == null).ToList();
            if(realizations.Count > 0)
            {

            }


            var associationEnds = storageMetaObjects.OfType<RDBMSMetaDataRepository.AssociationEnd>().Where(x => x.Association == null || x.Association.RoleA?.Specification == null || x.Association.RoleB?.Specification == null).ToList();

            var identityColumns = storageMetaObjects.OfType<RDBMSMetaDataRepository.IdentityColumn>().ToList();
            var columns = storageMetaObjects.OfType<RDBMSMetaDataRepository.Column>().ToList();



            var orphan_tables = storageMetaObjects.OfType<RDBMSMetaDataRepository.Table>().Where(x => x.Namespace == null).ToList();

            if (orphan_tables.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage orphan tables.");

            foreach (var orphan_table in orphan_tables)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(orphan_table);
            }

            var dynamicTypes = storageMetaObjects.Where(x => x.ToString().IndexOf("<") == 0).ToList();

            if (dynamicTypes.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage dynamic types.");
            foreach (var dynamicType in dynamicTypes)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(dynamicType);
            }
            foreach (var entry in (from primitive in storageMetaObjects.OfType<RDBMSMetaDataRepository.Primitive>()
                                   group primitive by primitive.Identity.ToString() into doublePripetives
                                   select new { identity = doublePripetives.Key, PrimitiveEntries = doublePripetives.ToList() }))
            {
                while (entry.PrimitiveEntries.ToList().Count > 1)
                {
                    //System.Diagnostics.Debug.Assert(false, "garbage multiple primitives.");

                    //break;
                    var _primitive = entry.PrimitiveEntries.Last();
                    foreach (var column in activeColumns.Where(x => x.Type == _primitive))
                    {
                        column.Type = entry.PrimitiveEntries.First();
                    }
                    PersistenceLayer.ObjectStorage.DeleteObject(_primitive);
                    entry.PrimitiveEntries.Remove(_primitive);

                }
            }

            var genericClasses = (from meta in storageMetaObjects.OfType<RDBMSMetaDataRepository.Class>()
                                  where IdentityWithHashCode(meta.Identity.ToString())
                                  orderby meta.Identity.ToString()
                                  select meta).ToList();
            if (genericClasses.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage generic classes.");
            foreach (var generic in genericClasses)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(generic);
            }
            var genericInterfaces = (from meta in storageMetaObjects.OfType<RDBMSMetaDataRepository.Interface>()
                                     where IdentityWithHashCode(meta.Identity.ToString())
                                     orderby meta.Identity.ToString()
                                     select meta).ToList();

            if (genericInterfaces.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage generic interfaces.");
            foreach (var generic in genericInterfaces)
            {

                PersistenceLayer.ObjectStorage.DeleteObject(generic);
            }
            var features = storageMetaObjects.OfType<MetaDataRepository.Feature>().Where(x => x.Owner == null && !(x is OOAdvantech.RDBMSMetaDataRepository.StoreProcedure)).ToList();
            //var emtysting = storageMetaObjects.Where(x => string.IsNullOrWhiteSpace(x.ToString())).ToArray();
            if (features.Count > 0)
                System.Diagnostics.Debug.Assert(false, "garbage Features.");
            foreach (var feature in features)
            {





                PersistenceLayer.ObjectStorage.DeleteObject(feature);
            }

            var storageCellsLinks = storageMetaObjects.OfType<RDBMSMetaDataRepository.StorageCellsLink>().Where(x => x.Type != null && x.ObjectLinksTable != null).ToList();
            foreach (var associationEnd in associationEnds)
            {
                var spec = associationEnd.Specification;
                if (associationEnd.Association != null)
                {
                    var specA = associationEnd.Association.RoleA?.Specification;
                    var specB = associationEnd.Association.RoleB?.Specification;
                    if (associationEnd.Association.RoleA != null)
                        OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(associationEnd.Association.RoleA);
                    if (associationEnd.Association.RoleB != null)
                        OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(associationEnd.Association.RoleB);
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(associationEnd.Association);

                }
                else
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(associationEnd);
            }
        }

        private bool IdentityWithHashCode(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                if (identity.LastIndexOf("]") != -1)
                {
                    string hasCodeStr = identity.Substring(identity.LastIndexOf("]") + 1);
                    int hasCode = 0;
                    return int.TryParse(hasCodeStr, out hasCode);
                }
            }
            return false;

        }

        void UpdateRelations()
        {
            Dictionary<string, List<MetaObject>> metaObjects = new Dictionary<string, List<MetaObject>>();
            List<MetaObject> storageMetaObjects = new List<MetaObject>();

            Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(this).Execute("SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject ");//WHERE MetaObjectIDStream = \""+OriginMetaObject.Identity.ToString()+"\" ");

            foreach (Collections.StructureSet Rowset in aStructureSet)
            {
                MetaDataRepository.MetaObject metaObject = (MetaDataRepository.MetaObject)Rowset["MetaObject"];
                storageMetaObjects.Add(metaObject);
                if (metaObject.Identity.ToString() == null)
                {
                    int were = 0;
                }
            }

            var associationEndDictionary = (from metaObject in storageMetaObjects.OfType<RDBMSMetaDataRepository.AssociationEnd>()
                                            group metaObject by metaObject.Identity.ToString() into identityMetaObjecst
                                            select identityMetaObjecst).ToDictionary(x => x.Key);



            //List<MetaObject> IdMetaobjects = null;
            //if (!metaObjects.TryGetValue(metaObject.Identity.ToString(), out IdMetaobjects))
            //{
            //    IdMetaobjects = new List<MetaObject>();
            //    metaObjects[metaObject.Identity.ToString()] = IdMetaobjects;
            //}

            //IdMetaobjects.Add(metaObject);
            //}
            var sso = metaObjects.Where(x => x.Value.Count > 1).ToArray();
            string Query = "SELECT storageCellsLink FROM " + typeof(RDBMSMetaDataRepository.StorageCellsLink).FullName + " storageCellsLink";
            Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(this).Execute(Query);


            try
            {
                foreach (Collections.StructureSet Rowset in structureSet)
                {
                    RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset["storageCellsLink"];

                    if (storageCellsLink.Type != null && storageCellsLink.Type.Connections.Count != 2)
                    {
                        if (storageCellsLink.Type.Connections.Count > 2)
                        {

                        }
                    }
                    if (storageCellsLink.Type == null || storageCellsLink.Type.RoleA == null || storageCellsLink.Type.RoleB == null)
                        continue;

                    if (storageCellsLink.RoleAMultiplicityIsMany != storageCellsLink.Type.RoleA.Multiplicity.IsMany ||
                        storageCellsLink.RoleBMultiplicityIsMany != storageCellsLink.Type.RoleB.Multiplicity.IsMany)
                    {
                        //if (storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToOne
                        //    && storageCellsLink.Type.MultiplicityType == MetaDataRepository.AssociationType.OneToMany)
                        //    OneToManyTransferRelationData(storageCellsLink);
                        //else if ((storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToOne ||
                        //    storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToMany ||
                        //    storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                        //    storageCellsLink.Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                        //    ManyToManyTransferRelationData(storageCellsLink);

                        //(storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).RemoveUnusedReferenceColums();
                        //(storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).RemoveUnusedReferenceColums();

                    }
                }
            }
            catch (Exception error)
            {


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

            if (storageComponent == null)
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
    

        private void EndSynchronous(Azure.Data.Tables.TableClient storagesMetadataTable)
        {
            AzureStorageMetadata = (from storageMetada in storagesMetadataTable.Query<StorageMetadata>()
                                    where storageMetada.StorageName == StorageName && storageMetada.StorageIdentity == StorageIdentity
                                    select storageMetada).FirstOrDefault();

            AzureStorageMetadata.UnderConstruction = false;
            storagesMetadataTable.UpdateEntity(AzureStorageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);

        }



        private void BeginSynchronous(Azure.Data.Tables.TableClient storagesMetadataTable)
        {
            while (AzureStorageMetadata.UnderConstruction)
            {
                System.Threading.Thread.Sleep(100);
                AzureStorageMetadata = (from storageMetada in storagesMetadataTable.Query<StorageMetadata>()
                                        where storageMetada.StorageName == StorageName && storageMetada.StorageIdentity == StorageIdentity
                                        select storageMetada).FirstOrDefault();
            }
            while (true)
            {
                try
                {

                    AzureStorageMetadata.UnderConstruction = true;
                    storagesMetadataTable.UpdateEntity(AzureStorageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                    break;
                }
                catch (Exception error)
                {
                    if (error.Message == "The remote server returned an error: (412) Precondition Failed.")
                    {
                        while (AzureStorageMetadata.UnderConstruction)
                        {
                            System.Threading.Thread.Sleep(100);
                            AzureStorageMetadata = (from storageMetada in storagesMetadataTable.Query<StorageMetadata>()
                                                    where storageMetada.StorageName == StorageName && storageMetada.StorageIdentity == StorageIdentity
                                                    select storageMetada).FirstOrDefault();
                        }
                    }
                }
            }
        }


        public override string StorageName { get => base.StorageName; set { } }

        public override string Name { get => base.Name; set { } }

        internal void SetStorageName(string storageName)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Name = storageName;
                _StorageName = storageName;

                stateTransition.Consistent = true;
            }
        }

        void GetAllDependencies(ref System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, OOAdvantech.MetaDataRepository.MetaObject> dependencies, MetaDataRepository.Component component)
        {

            foreach (MetaDataRepository.Dependency dependency in component.ClientDependencies)
            {
                if (!dependencies.ContainsKey(dependency.Supplier))
                {
                    object[] objects = (dependency.Supplier as DotNetMetaDataRepository.Assembly).WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                    if (objects.Length == 0)
                        continue;

                    dependencies.Add(dependency.Supplier, dependency.Supplier);
                    GetAllDependencies(ref dependencies, dependency.Supplier as MetaDataRepository.Component);
                }
            }
        }

    }
}
