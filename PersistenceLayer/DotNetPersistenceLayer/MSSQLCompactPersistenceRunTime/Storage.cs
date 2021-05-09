using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.RDBMSDataObjects;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{

    [BackwardCompatibilityID("{C3023A0F-1DCE-49a2-BFE6-D9ADCEE1A79D}")]
    [Persistent("<ExtMetaData><RDBMSInheritanceMapping>OneTablePerConcreteClass</RDBMSInheritanceMapping></ExtMetaData>")]
    public class Storage : RDBMSMetaDataRepository.Storage
    {

        /// <MetaDataID>{276C7983-B380-4CD2-B887-0306D331384B}</MetaDataID>
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


                    //					PersistenceLayer.ObjectStorage objectStorage=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
                    //					PersistenceLayer.StructureSet aStructureSet=objectStorage.Execute("SELECT Components FROM "+typeof(MetaDataRepository.MetaObject).FullName+" Components WHERE MetaObjectIDStream = \""+Component.Identity.ToString()+"\" ");

                    //					foreach( RDBMSMetaDataRepository.Component CurrComponent  in Components)
                    //					{								
                    //						if(CurrComponent.Identity==Component.Identity)
                    //						{
                    //							mComponent=CurrComponent;
                    //							break;
                    //						}
                    //					}

                    if (mComponent == null)
                    {
                        mComponent = (RDBMSMetaDataRepository.Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(Component, this);
                        mComponent.Context = this;
                        _Components.Add(mComponent);
                    }

                    System.Collections.Hashtable dependencies = new System.Collections.Hashtable();
                    GetAllDependencies(ref dependencies, Component);

                    foreach (System.Collections.DictionaryEntry entry in dependencies)
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

                    MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSMetaDataRepository.MetaObjectsStack();
                    MetaDataRepository.SynchronizerSession.StartSynchronize();

                    mComponent.BuildMappingElement(this);

                    ///}
                    //catch(System.Exception Error)
                    //{


                    //}
                    MetaDataRepository.SynchronizerSession.StopSynchronize();

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

            System.Collections.ArrayList components = new System.Collections.ArrayList();
            foreach (string Component in assembliesFullNames)
            {
                System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(Component);
                 
                if (dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length == 0)
                    throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");
                //OOAdvantech.MetaDataRepository.Component.GetClassifier()
                DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
                if (mAssembly == null)
                    mAssembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
                System.Collections.ArrayList errors = new System.Collections.ArrayList();
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
                foreach (DotNetMetaDataRepository.Assembly component in components)
                    RegisterComponent(component);
                Build();
                stateTransition.Consistent = true;
            }


        }
        /// <MetaDataID>{C9620952-390E-4374-8E7F-672767DCA872}</MetaDataID>
        public override void RegisterComponent(string Component)
        {
            //TODO Error prone  εάν περάσει λάθος string ...
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.Load(Component);
            object[] objects = dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length == 0)
                throw new System.Exception("You must declare in assemblyInfo file of  '" + dotNetAssembly.FullName + " the OOAdvantech.MetaDataRepository.BuildAssemblyMetadata attribute");

            DotNetMetaDataRepository.Assembly mAssembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
            if (mAssembly == null)
                mAssembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
            System.Collections.ArrayList errors = new System.Collections.ArrayList();
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

                RegisterComponent(mAssembly);
                Build();
                stateTransition.Consistent = true;
            }
        }

        public Storage(string storageName,string storageLocation)
        {
            _StorageName=storageName;
            _StorageLocation = storageLocation;
            _StorageType = "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider";
        }

        public void UpdateSchema()
        {
            // Error prone να τσεκαριστή όταν δεν καλλειται απο new storage

            DatabaseConnect(true);
            object result = null;
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
            {
                UpdateDataBaseMetadata();
                transactionScope.Complete();
            }
        }

        public void UpdateDataBaseMetadata()
        {
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
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{89977103-8713-4F33-862A-C3E5B90F5182}</MetaDataID>
        private DataBase _StorageDataBase;
        /// <MetaDataID>{B70D7637-4B6C-41AB-B0DF-EAB9D9CD354B}</MetaDataID>
        public DataBase StorageDataBase
        {
            get
            {
                if (_StorageDataBase == null)
                    _StorageDataBase = GetDataBase(_StorageLocation, _StorageName);
                return _StorageDataBase;
            }
        }


        DataBase GetDataBase(string dataBaseName,string dataBaseLocation )
        {
            string connectionString = @"Data Source=" + dataBaseLocation; // @"Initial Catalog=master;Integrated Security=True;Data Source=localhost\SQLExpress";
            System.Data.SqlServerCe.SqlCeConnection connection= new System.Data.SqlServerCe.SqlCeConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                    return new DataObjects.DataBase(this, dataBaseName);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.", Error);
            }
            finally 
            {
                connection.Close(); 
            }

            return null;
        }


    }

}
