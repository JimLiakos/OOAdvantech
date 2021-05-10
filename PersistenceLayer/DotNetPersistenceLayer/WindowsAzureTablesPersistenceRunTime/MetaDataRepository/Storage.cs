﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Azure.Cosmos.Table;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{5263e16d-290b-456c-b85f-dd26356b168c}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{5263e16d-290b-456c-b85f-dd26356b168c}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class Storage : RDBMSMetaDataRepository.Storage
    {

        /// <MetaDataID>{b682ce60-ea0e-4d75-bd23-743af87b5546}</MetaDataID>
        protected Storage()
        {
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
        }


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
        public override void RegisterComponent(string assemblyFullName)
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
            CloudStorageAccount account = (ObjectStorage.GetStorageOfObject(this) as WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ObjectStorage).Account;
            CloudTableClient cloudTablesClient = account.CreateCloudTableClient();
            CloudTable storagesMetadataTable = cloudTablesClient.GetTableReference("StoragesMetadata");
            BeginSynchronous(storagesMetadataTable);
            try
            {
                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {
                    RegisterComponent(mAssembly);

                    //OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema(ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType) as PersistenceLayerRunTime.ObjectStorage);
                    //if (!StorageDataBase.RDBMSSQLScriptGenarator.SupportAddRemoveKeys)
                    //{
                    //    CreateStorageCellsLinks();
                    //}
                    //if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    //    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateStorageSchema);
                    //StorageMetaObjects = null;
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                EndSynchronous(storagesMetadataTable);
            }


        }






        public override void RegisterComponent(string[] assembliesFullNames, Dictionary<string, XDocument> assembliesMappingData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{71544717-9d54-4a6d-b5e3-54cb030a096d}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
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

                        if (column.MappedAssociationEnd != null && column.MappedAssociationEnd.GetOtherEnd()!=null && @class.GetAssociationEndRealization(column.MappedAssociationEnd.GetOtherEnd()) != null)
                            column.MappedAssociationEndRealizationIdentity = @class.GetAssociationEndRealization(column.MappedAssociationEnd.GetOtherEnd()).Identity.ToString();

                    }
                }

                stateTransition.Consistent = true;
            }

        }

        public void RegisterComponent(MetaDataRepository.Component Component)
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

                    StateTransition.Consistent = true; ;
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
            }


        }
        void UpdateRelations()
        {
            string Query = "SELECT storageCellsLink FROM " + typeof(RDBMSMetaDataRepository.StorageCellsLink).FullName + " storageCellsLink";
            Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(this).Execute(Query);

            try
            {
                foreach (Collections.StructureSet Rowset in structureSet)
                {
                    RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset["storageCellsLink"];

                    if (storageCellsLink.Type.RoleA == null || storageCellsLink.Type.RoleB == null)
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
        private void EndSynchronous(CloudTable storagesMetadataTable)
        {
            AzureStorageMetadata = (from storageMetada in storagesMetadataTable.CreateQuery<StorageMetadata>()
                                    where storageMetada.StorageName == StorageName && storageMetada.StorageIdentity == StorageIdentity
                                    select storageMetada).FirstOrDefault();

            AzureStorageMetadata.UnderConstruction = false;
            TableOperation updateOperation = TableOperation.Replace(AzureStorageMetadata);
            storagesMetadataTable.Execute(updateOperation);
        }

        private void BeginSynchronous(CloudTable storagesMetadataTable)
        {
            while (AzureStorageMetadata.UnderConstruction)
            {
                System.Threading.Thread.Sleep(100);
                AzureStorageMetadata = (from storageMetada in storagesMetadataTable.CreateQuery<StorageMetadata>()
                                        where storageMetada.StorageName == StorageName && storageMetada.StorageIdentity == StorageIdentity
                                        select storageMetada).FirstOrDefault();
            }
            while (true)
            {
                try
                {

                    AzureStorageMetadata.UnderConstruction = true;
                    TableOperation updateOperation = TableOperation.Replace(AzureStorageMetadata);
                    storagesMetadataTable.Execute(updateOperation);
                    break;
                }
                catch (Exception error)
                {
                    if (error.Message == "The remote server returned an error: (412) Precondition Failed.")
                    {
                        while (AzureStorageMetadata.UnderConstruction)
                        {
                            System.Threading.Thread.Sleep(100);
                            AzureStorageMetadata = (from storageMetada in storagesMetadataTable.CreateQuery<StorageMetadata>()
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