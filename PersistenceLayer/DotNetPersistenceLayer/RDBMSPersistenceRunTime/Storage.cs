using OOAdvantech.RDBMSDataObjects;
using OOAdvantech.DotNetMetaDataRepository;
using System.Linq;
namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{CB25317A-EFDC-4CB2-A5D7-A31C1F91FA77}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{CB25317A-EFDC-4CB2-A5D7-A31C1F91FA77}")]
    [MetaDataRepository.Persistent("<ExtMetaData><RDBMSInheritanceMapping>OneTablePerConcreteClass</RDBMSInheritanceMapping></ExtMetaData>")]
    public class Storage : RDBMSMetaDataRepository.Storage
    {
        /// <MetaDataID>{C04BE9D6-F5DD-4980-ADE0-4306FA4B0A91}</MetaDataID>
        Storage()
        {
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
            //_StorageType = "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider";
        }


        /// <MetaDataID>{1cf6a435-6676-4bc0-8020-dbcbdd72b087}</MetaDataID>
        public Storage(string storageName, string storageLocation, string storageType, string nativeStorageID, RDBMSDataObjects.DataBase dataBase)
        {
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
            _StorageName = storageName;
            _StorageLocation = storageLocation;
            _StorageType = storageType;
            _StorageDataBase = dataBase;
            _NativeStorageID = nativeStorageID;
            dataBase.Storage = this;

        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{89977103-8713-4F33-862A-C3E5B90F5182}</MetaDataID>
        private DataBase _StorageDataBase;
        /// <MetaDataID>{B70D7637-4B6C-41AB-B0DF-EAB9D9CD354B}</MetaDataID>
        public DataBase StorageDataBase
        {
            get
            {
                //if(_StorageDataBase==null)
                //    _StorageDataBase=GetDataBase(_StorageLocation, _StorageName);
                return _StorageDataBase;
            }
            set
            {
                _StorageDataBase = value;
                value.Storage = this;
            }
        }

        /// <MetaDataID>{fea3c1e2-7a1a-4cbe-bb22-f371a099b4d8}</MetaDataID>
        PersistenceLayerRunTime.ObjectStorage _MetadataStorage = null;
        /// <MetaDataID>{833d0a31-0667-4ca3-986e-a73a552c3657}</MetaDataID>
        public PersistenceLayerRunTime.ObjectStorage MetadataStorage
        {
            get
            {
                if (_MetadataStorage == null)
                    _MetadataStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this.Properties) as PersistenceLayerRunTime.ObjectStorage;
                return _MetadataStorage;
            }
        }



        ///// <MetaDataID>{8EE847F7-21C3-4DA3-8453-A96BE14225C8}</MetaDataID>
        //void Build()
        //{
        //    //TODO: υπάρχει πρόβλημα όταν υπάρχει class σε δύο διαφορετικά namespaces με το ιδιο όνομα
        //    UpdateSchema();

        //}

        /// <MetaDataID>{276C7983-B380-4CD2-B887-0306D331384B}</MetaDataID>
        public void RegisterComponent(MetaDataRepository.Component Component, System.Xml.Linq.XDocument mappingData)
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

                    System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, OOAdvantech.MetaDataRepository.MetaObject> dependencies =  new System.Collections.Generic.Dictionary<MetaDataRepository.MetaObject,MetaDataRepository.MetaObject>();
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

                    mComponent.BuildMappingElement(this, mappingData);
                    mComponent.Synchronize(Component);

                    string myName = (string)mComponent.GetPropertyValue(typeof(string), "Persosnal", "Myname");
                    mComponent.PutPropertyValue("Persosnal", "Myname", "mitsos");

                    MetaDataRepository.SynchronizerSession.StopSynchronize();

                    //MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();

                    mComponent.BuildMappingElement(this, mappingData);


                    MetaDataRepository.SynchronizerSession.StopSynchronize();

                    StateTransition.Consistent = true; ;
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
            }

        }


        /// <MetaDataID>{3c43f535-b58b-4816-be27-c367b42abc7b}</MetaDataID>
        public void RegisterComponent(MetaDataRepository.Component[] Components, System.Xml.Linq.XDocument mappingData)
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
                    foreach (MetaDataRepository.Component component in Components)
                    {

                        RDBMSMetaDataRepository.Component mComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(component, this) as RDBMSMetaDataRepository.Component;
                        if (mComponent == null)
                        {
                            mComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(component, this);
                            mComponent.Context = this;
                            _Components.Add(mComponent);
                        }
                    }
                    foreach (MetaDataRepository.Component component in Components)
                    {
                        RDBMSMetaDataRepository.Component mComponent = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(component, this) as RDBMSMetaDataRepository.Component;
                        System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, OOAdvantech.MetaDataRepository.MetaObject> dependencies = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, OOAdvantech.MetaDataRepository.MetaObject>();
                        GetAllDependencies(ref dependencies, component);

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
                        mComponent.Synchronize(component);
                        string myName = (string)mComponent.GetPropertyValue(typeof(string), "Persosnal", "Myname");
                        mComponent.PutPropertyValue("Persosnal", "Myname", "mitsos");

                        MetaDataRepository.SynchronizerSession.StopSynchronize();

                        //MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                        MetaDataRepository.SynchronizerSession.StartSynchronize();

                        mComponent.BuildMappingElement(this, mappingData);


                        MetaDataRepository.SynchronizerSession.StopSynchronize();
                    }

                    StateTransition.Consistent = true; ;
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception(Error.Message, Error);
                }
            }

        }

        /// <MetaDataID>{5999A140-4A44-47AF-A00F-8FF88343ACC9}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames)
        {

            System.Collections.Generic.List<DotNetMetaDataRepository.Assembly> components = new System.Collections.Generic.List<OOAdvantech.DotNetMetaDataRepository.Assembly>();
            foreach (string Component in assembliesFullNames)
            {
                System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( Component));
                object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                if (objects.Length == 0)
                    throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

                DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
                if (mAssembly == null)
                    mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
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
                components.Add(mAssembly);
            }


            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {

                //foreach(DotNetMetaDataRepository.Assembly  component in components)
                RegisterComponent(components.ToArray(), null);

                Commands.UpdateStorageSchema updateStorageSchema = new Commands.UpdateStorageSchema(ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType) as PersistenceLayerRunTime.ObjectStorage);
                if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateStorageSchema);

                //UpdateSchema();
                StorageMetaObjects = null;
                stateTransition.Consistent = true;
            }


        }
        /// <MetaDataID>{C9620952-390E-4374-8E7F-672767DCA872}</MetaDataID>
        public override void RegisterComponent(string Component, System.Collections.Generic.List<string> types = null)
        {
            RegisterComponent(Component, default(string),types);

        }

        /// <MetaDataID>{d14798d4-6892-4115-acbf-2e2b422b48d4}</MetaDataID>
        public override void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData)
        {
            //TODO Error prone  εάν περάσει λάθος string ...
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( assemblyFullName));
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");
            DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            if (mAssembly == null)
                mAssembly = DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
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
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {

                RegisterComponent(mAssembly, mappingData);


                OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema(ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType) as PersistenceLayerRunTime.ObjectStorage);
                if (!StorageDataBase.RDBMSSQLScriptGenarator.SupportAddRemoveKeys)
                {
                   CreateStorageCellsLinks();


                    //foreach (var association in (from association in linqStorage.GetObjectCollection<RDBMSMetaDataRepository.Association>()
                    //                             select association))
                    //{


                    //    if (association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&
                    //        association.LinkClass == null)
                    //    {
                    //        var roleAClasses = association.RoleA.Specification.GetAllSpecializeClasifiers().OfType<RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();
                    //        var roleBClasses = association.RoleB.Specification.GetAllSpecializeClasifiers().OfType<RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();

                    //        foreach (var roleAClass in roleAClasses)
                    //        {
                    //            foreach (var roleAstorageCell in roleAClass.StorageCells)
                    //            {
                    //                foreach (var roleBClass in roleBClasses)
                    //                {
                    //                    foreach (var roleBstorageCell in roleBClass.StorageCells)
                    //                    {
                                            
                    //                        var objectCollectionsLink = association.GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                    //                    }
                    //                }
                    //            }
                    //        }

                    //    }
                    //}
                }

                if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateStorageSchema);
                //UpdateSchema();
                StorageMetaObjects = null;
                stateTransition.Consistent = true;
            }

        }


        private void CreateStorageCellsLinks()
        {
            var linqStorage = new OOAdvantech.Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

            System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Association, System.Collections.Generic.List<string>> associations = new System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Association, System.Collections.Generic.List<string>>();

            var persistentClasses = (from _class in linqStorage.GetObjectCollection<RDBMSMetaDataRepository.Class>()
                                     where _class.Persistent
                                     select _class).ToList();

            var storageAssociationEnds = (from associationEnd in linqStorage.GetObjectCollection<RDBMSMetaDataRepository.AssociationEnd>()
                                          select associationEnd).ToDictionary(x => x.Identity);

            foreach (var persistentClass in persistentClasses)
            {

                if (persistentClass.ClassHierarchyLinkAssociation!= null)
                {
                    var roleAClasses = persistentClass.ClassHierarchyLinkAssociation.RoleA.Specification.GetAllSpecializeClasifiers().OfType<OOAdvantech.RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();
                    var roleBClasses = persistentClass.ClassHierarchyLinkAssociation.RoleB.Specification.GetAllSpecializeClasifiers().OfType<OOAdvantech.RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();
                    foreach (var roleAClass in roleAClasses)
                    {
                        foreach (var roleBClass in roleBClasses)
                        {
                            foreach (var roleAstorageCell in roleAClass.StorageCells)
                            {
                                foreach (var roleBstorageCell in roleBClass.StorageCells)
                                {
                                    var objectCollectionsLink = (persistentClass.ClassHierarchyLinkAssociation as RDBMSMetaDataRepository.Association).GetStorageCellsLink(roleAstorageCell, roleBstorageCell, "", true);
                                    objectCollectionsLink.AddAssotiationClassStorageCell(persistentClass.ActiveStorageCell);
                                    objectCollectionsLink.UpdateForeignKeys();
                                }
                            }
                        }
                    }
                }
                foreach (var associationEnd in persistentClass.GetAssociateRoles(true))
                {
                    if (persistentClass.IsPersistent(associationEnd))
                    {
                        var relatedClasses = associationEnd.Specification.GetAllSpecializeClasifiers().OfType<OOAdvantech.RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();
                        if (associationEnd.Specification is OOAdvantech.RDBMSMetaDataRepository.Class)
                            relatedClasses.Add(associationEnd.Specification as OOAdvantech.RDBMSMetaDataRepository.Class);
                        if (associationEnd.IsRoleA)
                        {
                            var roleB = persistentClass;
                            foreach (var roleA in relatedClasses)
                            {
                                string vaueTypePath = "";
                                if (associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&
                                    associationEnd.Association.LinkClass == null)
                                {
                                    foreach( var roleAstorageCell in roleA.StorageCells)
                                    {
                                        foreach (var roleBstorageCell in roleB.StorageCells)
                                        {
                                            var objectCollectionsLink = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                            objectCollectionsLink.UpdateForeignKeys();
                                        }
                                    }
                                }


                                //var objectCollectionsLink =(associationEnd.Association as RDBMSMetaDataRepository.Association) .GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                            }
                        }
                        else
                        {
                            var roleA = persistentClass;
                            foreach (var roleB in relatedClasses)
                            {
                                string vaueTypePath = "";
                                if (associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&
                                  associationEnd.Association.LinkClass == null)
                                {
                                    foreach (var roleAstorageCell in roleA.StorageCells)
                                    {
                                        foreach (var roleBstorageCell in roleB.StorageCells)
                                        {
                                            var objectCollectionsLink = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                            objectCollectionsLink.UpdateForeignKeys();
                                        }
                                    }

                                }
                                //var objectCollectionsLink =(associationEnd.Association as RDBMSMetaDataRepository.Association) .GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);

                            }
                        }
                    }
                }
                foreach (var attribute in persistentClass.GetAttributes(true))
                {

                    if (persistentClass.IsPersistent(attribute))
                    {
                        if (attribute.Type is RDBMSMetaDataRepository.Structure)
                        {
                            MetaDataRepository.ValueTypePath valueTypePath = new MetaDataRepository.ValueTypePath();
                            System.Collections.Generic.List<MetaDataRepository.ValueTypePath> associationEndsValuePaths = GetValueTypeAssocitionEnds(attribute, valueTypePath);


                            foreach (var associationEndValueTypePath in associationEndsValuePaths)
                            {
                                var associationEndIdentity = associationEndValueTypePath.Pop();
                                string vaueTypePath = associationEndValueTypePath.ToString();

                                var associationEnd = storageAssociationEnds[associationEndIdentity];
                                var relatedClasses = associationEnd.Specification.GetAllSpecializeClasifiers().OfType<OOAdvantech.RDBMSMetaDataRepository.Class>().Where(_class => _class.Persistent).ToList();
                                if (associationEnd.Specification is OOAdvantech.RDBMSMetaDataRepository.Class)
                                    relatedClasses.Add(associationEnd.Specification as OOAdvantech.RDBMSMetaDataRepository.Class);
                                if (associationEnd.IsRoleA)
                                {
                                    var roleB = persistentClass;
                                    foreach (var roleA in relatedClasses)
                                    {
                                        if (associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&
                                        associationEnd.Association.LinkClass == null)
                                        {
                                            foreach (var roleAstorageCell in roleA.StorageCells)
                                            {
                                                foreach (var roleBstorageCell in roleB.StorageCells)
                                                {
                                                    var objectCollectionsLink = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                                    objectCollectionsLink.UpdateForeignKeys();
                                                }
                                            }

                                        }
                                        //var objectCollectionsLink =(associationEnd.Association as RDBMSMetaDataRepository.Association) .GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                    }
                                }
                                else
                                {
                                    var roleA = persistentClass;
                                    foreach (var roleB in relatedClasses)
                                    {
                                        if (associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&
                                        associationEnd.Association.LinkClass == null)
                                        {
                                            foreach (var roleAstorageCell in roleA.StorageCells)
                                            {
                                                foreach (var roleBstorageCell in roleB.StorageCells)
                                                {
                                                    var objectCollectionsLink = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                                    objectCollectionsLink.UpdateForeignKeys();
                                                }
                                            }

                                        }
                                        //var objectCollectionsLink =(associationEnd.Association as RDBMSMetaDataRepository.Association) .GetStorageCellsLink(roleAstorageCell, roleBstorageCell, vaueTypePath, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private System.Collections.Generic.List<MetaDataRepository.ValueTypePath> GetValueTypeAssocitionEnds(MetaDataRepository.Attribute attribute, MetaDataRepository.ValueTypePath valueTypePath)
        {
            System.Collections.Generic.List<MetaDataRepository.ValueTypePath> valuTypeAssocitionEnds = new System.Collections.Generic.List<MetaDataRepository.ValueTypePath>();
            valueTypePath.Push(attribute.Identity);
            foreach (var associationEnd in attribute.Type.GetAssociateRoles(true))
            {
                if ((attribute.Type as RDBMSMetaDataRepository.Structure).IsPersistent(associationEnd))
                {
                    MetaDataRepository.ValueTypePath associationEndValueTypePath = new MetaDataRepository.ValueTypePath(valueTypePath);
                    associationEndValueTypePath.Push(associationEnd.Identity);
                    valuTypeAssocitionEnds.Add(associationEndValueTypePath);
                }
            }

            foreach (var valuTypeAttribute in attribute.Type.GetAttributes(true))
            {

                if ((attribute.Type as RDBMSMetaDataRepository.Structure).IsPersistent(valuTypeAttribute))
                {
                    if (valuTypeAttribute.Type is RDBMSMetaDataRepository.Structure)
                        valuTypeAssocitionEnds.AddRange(GetValueTypeAssocitionEnds(valuTypeAttribute, valueTypePath));
                }
            }

            return valuTypeAssocitionEnds;
        }
        /// <MetaDataID>{0790b618-aafb-47a3-af2f-44b09ac9566c}</MetaDataID>
        public override void RegisterComponent(string[] assembliesFullNames, System.Collections.Generic.Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData)
        {

            DotNetMetaDataRepository.Assembly[] assemblies = new DotNetMetaDataRepository.Assembly[assembliesFullNames.Length];

            int i = 0;
            //TODO Error prone  εάν περάσει λάθος string ...
            foreach (string assemblyFullName in assembliesFullNames)
            {
                System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assemblyFullName));
                object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
                if (objects.Length == 0)
                    throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");
                DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
                if (mAssembly == null)
                    mAssembly =  DotNetMetaDataRepository.Assembly.GetComponent(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
                System.Collections.Generic.List<MetaDataError> errors = new System.Collections.Generic.List<MetaDataError>();
                bool hasErrors = mAssembly.ErrorCheck(ref errors);
                assemblies[i++] = mAssembly;
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
            }
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                i = 0;
                foreach (string assemblyFullName in assembliesFullNames)
                    RegisterComponent(assemblies[i++], assembliesMappingData[assemblyFullName]);
                OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema(ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType) as PersistenceLayerRunTime.ObjectStorage);
                if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(updateStorageSchema);
                //UpdateSchema();
                StorageMetaObjects = null;
                stateTransition.Consistent = true;
            }
        }



        /// <MetaDataID>{8B17003C-DCFC-4982-A521-9D9FEA1CBA22}</MetaDataID>
        //internal System.ServiceProcess.ServiceController SQLServices; //MSSQLSERVER
        //	/// <MetaDataID>{0A284092-1E22-4464-ACCB-DB0927DFF3CA}</MetaDataID>
        //	internal SQLDMO.SQLServer mSQLServer;
        /// <MetaDataID>{D1B03755-4047-4BC1-8A74-136A89B27146}</MetaDataID>
        //	internal SQLDMO.Database mDatabase;


        ///// <MetaDataID>{BDA64E3B-8A51-4ADA-AD39-4069AA104D02}</MetaDataID>
        //void CreateDataBase(string ServerName,string DataBaseName)
        //{
        //    string ConnectionString = "Integrated Security=True;Initial Catalog=master;Data Source=" + ServerName + @"\SQLExpress";
        //    System.Data.SqlClient.SqlConnection Connection=new System.Data.SqlClient.SqlConnection(ConnectionString);
        ////	System.Data.SqlClient.SqlTransaction Transaction;
        //    try
        //    {
        //        Connection.Open();
        //    }
        //    catch(System.Exception Error)
        //    {
        //        throw new System.Exception("The storage location "+ServerName+" can't be accessed.",Error);
        //    }

        //    System.Data.SqlClient.SqlCommand Command=new System.Data.SqlClient.SqlCommand("CREATE DATABASE "+DataBaseName  ,Connection);
        //    string CommandText=null;
        //    try
        //    {

        //        Command.ExecuteNonQuery();
        //        Connection.Close();
        //        //Connection.ConnectionString = "Integrated Security=True;Initial Catalog=" + DataBaseName + ";Data Source=" + ServerName + @"\SQLExpress";
        //        //Connection.Open();


        //        //Transaction= Connection.BeginTransaction();
        //        //Command.Connection=Connection;
        //        //Command.Transaction=Transaction;
        //        //CommandText="CREATE TABLE MetaDataTable("+
        //        //    "ID int NOT NULL,"+
        //        //    "MetaData image NULL)  "+
        //        //    "ON [PRIMARY]	 TEXTIMAGE_ON [PRIMARY] "+
        //        //    "CREATE TABLE T_GlobalObjectCollectionIDs ("+
        //        //    "InStoragelID int NOT NULL  ,"+
        //        //    "ObjectCollectionID binary (20) NOT NULL, "+
        //        //    "OutStorageID int NOT NULL  "+
        //        //    ") ON [PRIMARY] ";


        //    }
        //    catch(System.Exception Error)
        //    {
        //        Connection.Close();
        //        if(StorageDataBase!=null)
        //            throw new System.Exception("DataBase with name '"+DataBaseName+"' already exist");
        //        else
        //            throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //    }
        //    //try
        //    //{
        //    //    Command.CommandText=CommandText;
        //    //    Command.ExecuteNonQuery();

        //    //    CommandText="CREATE Procedure dbo.UpdateMetaData "+
        //    //        "@MetaData image "+
        //    //        "AS "+
        //    //        "UPDATE    MetaDataTable SET MetaData =@MetaData  WHERE     (ID = 1) ";

        //    //    Command.CommandText=CommandText;
        //    //    Command.ExecuteNonQuery();

        //    //    Transaction.Commit();

        //    //}
        //    //catch(System.Exception Error)
        //    //{
        //    //    Connection.Close();
        //    //    throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //    //}
        //    //Connection.Close();
        //    if(StorageDataBase==null)
        //        throw new System.Exception("can't create  DataBase with name '"+DataBaseName+"'.");
        //}



        //        /// <MetaDataID>{189CA0C4-1E58-4B07-BD5E-8386D2853B36}</MetaDataID>
        //        internal void DatabaseConnect(bool Create)
        //        {
        //            if(StorageDataBase==null&&!Create)
        //                throw new System.Exception("The storage with name '"+_StorageName+"' doesn't exist.");


        //            if(StorageDataBase==null&&Create)
        //                CreateDataBase( _StorageLocation,_StorageName);

        //            return;
        /// *
        /// <MetaDataID>{58b9bcc0-42f6-402a-9fbe-67ae6261e590}</MetaDataID>
        //            try
        //            {
        //                if(mSQLServer==null)
        //                    throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.");
        //                SQLDMO.SQLDMO_SVCSTATUS_TYPE Status=mSQLServer.Status;
        //                if(Status!=SQLDMO.SQLDMO_SVCSTATUS_TYPE.SQLDMOSvc_Running)
        //                    throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.");
        //            }
        //            catch(System.Runtime.InteropServices.ExternalException Error)
        //            {
        //                throw new System.Exception("The storage location "+_StorageLocation+" can't be accessed.",Error);
        //            }

        //            foreach(SQLDMO.Database CurrDatabase in mSQLServer.Databases)
        //            {
        //                if(CurrDatabase.Name==_StorageName)
        //                {
        //                    mDatabase=CurrDatabase;
        //                    break;
        //                }
        //            }
        //            if(mDatabase==null&&Create)
        //            {
        //                mDatabase=new SQLDMO.DatabaseClass();
        //                mDatabase.Name=_StorageName;
        //                mSQLServer.Databases.Add(mDatabase);
        //            }*/
        //        }
        ///// <MetaDataID>{A8E5101B-13F7-44E8-B1B1-571FBF5BF533}</MetaDataID>
        //internal bool ServerContainsDatabase(string DatabaseName)
        //{
        //    if(GetDataBase(_StorageLocation, _StorageName)==null)
        //        return false;
        //    else
        //        return true;
        //}


        public void UpdateSchema(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells, System.Collections.Generic.List<MetaDataRepository.StorageCellsLink> storageCellsLinks)
        {
            StorageMetaObjects = null;
#if! DeviceDotNet
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
            {
#endif

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();
                    (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase = StorageDataBase;
                    try
                    {
                        StorageDataBase.Update(storageCells,storageCellsLinks);
                    }
                    catch (System.Exception Error)
                    {
                        throw new System.Exception(Error.Message, Error);
                    }
                    finally
                    {

                        (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase = null;
                        MetaDataRepository.SynchronizerSession.StopSynchronize();
                        bool throwexception = false;
                        if (throwexception)
                        {
                            throw new System.Exception("Liakos");
                        }


                        StateTransition.Consistent = true;
                        MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    }
                }
#if! DeviceDotNet
                transactionScope.Complete();
            }
#endif
        }
        /// <MetaDataID>{8B388409-FC2E-438C-BD0C-81CB940DE717}</MetaDataID>
        public void UpdateSchema()
        {
            StorageMetaObjects = null;
#if! DeviceDotNet
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
            {
#endif

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();
                    (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase = StorageDataBase;
                    try
                    {
                        StorageDataBase.Update();
                    }
                    catch (System.Exception Error)
                    {
                        throw new System.Exception(Error.Message, Error);
                    }
                    finally
                    {

                        (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).theSynchronizedDataBase = null;
                        MetaDataRepository.SynchronizerSession.StopSynchronize();
                        bool throwexception = false;
                        if (throwexception)
                        {
                            throw new System.Exception("Liakos");
                        }


                        StateTransition.Consistent = true;
                        MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    }
                }
#if! DeviceDotNet
                transactionScope.Complete();
            }
#endif
        }
        //	/// <MetaDataID>{710A6B05-5540-4453-BBC4-8239E64CAABE}</MetaDataID>
        //	private COMPlusTransaction TransactionObject=null;
        //        /// <MetaDataID>{D760F717-623E-41B0-803B-A0D854F574DA}</MetaDataID>
        //        public void UpdateSchema()
        //        {
        //            // Error prone να τσεκαριστή όταν δεν καλλειται απο new storage
        //#if! DeviceDotNet 
        //            using(System.Transactions.TransactionScope transactionScope=new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
        //            {
        //#endif
        //                UpdateDataBaseMetadata();
        //#if! DeviceDotNet
        //                transactionScope.Complete();
        //            }
        //#endif

        //        }

        /// <MetaDataID>{42b7d3c0-e529-4d55-913e-44513a3a1458}</MetaDataID>
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
