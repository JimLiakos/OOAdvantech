using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace OOAdvantech.PersistenceLayer
{

    /// <MetaDataID>{46ec2c10-7335-4d64-ac52-6dd97786cca5}</MetaDataID>
    public interface IStorageLocatorEx
    {
        /// <MetaDataID>{3abfef75-5b03-4a2f-9a0d-09154605b7ce}</MetaDataID>
        MetaDataRepository.StorageMetaData GetSorageMetaData(string storageIdentity);

    }


    /// <MetaDataID>{64735171-9d44-4963-ab97-247898d85ee2}</MetaDataID>
    public class StorageServerInstanceLocator : Remoting.MonoStateClass//,IStorageServerInstanceLocator
    {
        public static void AddStorageLocatorExtender(IStorageLocatorEx storageLocatorExtender)
        {
            if(!StorageLocatorExtenders.Contains(storageLocatorExtender))
                StorageLocatorExtenders.Add(storageLocatorExtender);
        }

        static List< IStorageLocatorEx> StorageLocatorExtenders=new List<IStorageLocatorEx>();

        static XDocument StoragesMetaDataDoc;
        static List<MetaDataRepository.StorageMetaData> StoragesMetaData = new List<MetaDataRepository.StorageMetaData>();
        public MetaDataRepository.StorageMetaData GetSorageMetaData(string storageIdentity)
        {
            lock (this)
            {
                var storage =
                   (from storageMetaData in StoragesMetaData
                    where storageMetaData.StorageIdentity == storageIdentity
                    select storageMetaData).FirstOrDefault();
                if (storage != null)
                    return storage;
                else
                {
                    foreach (var storageLocatorExtender in StorageLocatorExtenders)
                    {
                        try
                        {
                            var storageMetaData = storageLocatorExtender.GetSorageMetaData(storageIdentity);
                            if (storageMetaData != null && !string.IsNullOrWhiteSpace(storageMetaData.StorageIdentity))
                                return storageMetaData;

                        }
                        catch (Exception error)
                        {
                        }
                    }

                    return new MetaDataRepository.StorageMetaData();
                } 
            }
        }

        public void PublishStorageMetaData(MetaDataRepository.StorageMetaData storageMetaData)
        {
            lock (this)
            {
                if ((from theStorageMetaData in StoragesMetaData
                     where theStorageMetaData.StorageIdentity == storageMetaData.StorageIdentity
                     select theStorageMetaData).Count() == 0)
                {
                    StoragesMetaData.Add(storageMetaData);
                } 
            }
        }
        // private static StorageServerInstanceLocator MonoStateInstance;

        //static StorageServerInstanceLocator()
        //{


        //} 

        /// <exclude>Excluded</exclude>
        static bool _InStorageService;
        /// <MetaDataID>{b14fa49d-6ee2-4cc4-b195-c71e30c8237f}</MetaDataID>
        static public bool InStorageService
        {
            get
            {

                return _InStorageService;
            }
            internal set
            {
                _InStorageService = value;
            }
        }


        /// <MetaDataID>{8f89ac67-26ee-4196-b166-f604dae54d1f}</MetaDataID>
        public StorageServerInstanceLocator()
        {
            //if (MonoStateInstance == null)
            //    MonoStateInstance = this;

        }
        /// <MetaDataID>{aca2ed43-8973-4ad9-8bd2-69316ae22143}</MetaDataID>
        static public StorageServerInstanceLocator Current
        {
            get
            {
                StorageServerInstanceLocator storageServerInstanceLocator = StorageServerInstanceLocator.GetInstance(typeof(StorageServerInstanceLocator),true) as StorageServerInstanceLocator;
                   return storageServerInstanceLocator;
            }
        }
        ///// <MetaDataID>{fd567da6-b530-4b0f-b316-9e4ca3d7a7a1}</MetaDataID>
        //public static StorageServerInstanceLocator GetStorageServerInstanceLocator()
        //{
        //    if (Current == null)
        //        Current= MonoStateClass.GetInstance(typeof(StorageServerInstanceLocator)) as StorageServerInstanceLocator;
        //        return new StorageServerInstanceLocator();
        //    else return Current;


        //}


#if !DeviceDotNet
        /// <MetaDataID>{0c20b705-bb9f-4f1d-8328-37cdf2f66ea5}</MetaDataID>
        public System.ServiceProcess.ServiceController GetServiceForInstance(String instanceName)
        {
            if (instanceName == null || instanceName.Trim().Length == 0)
                return null;
            if (instanceName.Trim().ToLower() == "default")
                return new System.ServiceProcess.ServiceController("StorageServer");
            else
                return new System.ServiceProcess.ServiceController("StorageServer$" + instanceName);
        }

        public bool ServiceForInstanceExist(String instanceName)
        {
            if (instanceName == null || instanceName.Trim().Length == 0)
                return false;
            string serviceName;
            if (instanceName.Trim().ToLower() == "default")
                serviceName="StorageServer";
            else
                serviceName = "StorageServer$" + instanceName;

            return System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName) != null;
        }
        /// <MetaDataID>{e6bcf4c8-afdc-49cf-96ab-a17e0d41dfdb}</MetaDataID>
        public void Start(String instanceName)
        {
            GetServiceForInstance(instanceName).Start();
        }

        /// <MetaDataID>{6856a9a8-fe35-4b3d-a33b-c001c20f3485}</MetaDataID>
        public void Stop(String instanceName)
        {
            GetServiceForInstance(instanceName).Stop();
        }
        /// <MetaDataID>{f85d4a96-9ff4-426e-b9c8-2508e1c998be}</MetaDataID>
        public void Pause(string instanceName)
        {
            GetServiceForInstance(instanceName).Pause();
        }


        /// <MetaDataID>{f9e9019d-b1ef-4204-8475-7ed73c09f3b0}</MetaDataID>
        public string[] GetStorageServerInstances()
        {
            //#region MonoStateClass
            //if (this != MonoStateInstance)
            //    return MonoStateInstance.GetStorageServerInstances();
            //#endregion

            Microsoft.Win32.RegistryKey registryKey =Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\HandySoft\StorageServerInstances\");
            if (registryKey == null)
                return new string[0];

            System.Collections.Hashtable instancesInRegistry=new System.Collections.Hashtable();
            foreach(string instanceName in registryKey.GetSubKeyNames())
                instancesInRegistry[instanceName.Trim().ToLower()]=instanceName;
            System.Collections.ArrayList instanceNames=new System.Collections.ArrayList();

            foreach (System.ServiceProcess.ServiceController serviceController in System.ServiceProcess.ServiceController.GetServices())
            {
                string serviceName = serviceController.ServiceName;
                if (serviceName.IndexOf("StorageServer$") == 0)
                {
                    string instanceName = serviceName.Substring("StorageServer$".Length).Trim().ToLower();
                    if (instancesInRegistry.Contains(instanceName))
                        instanceNames.Add(instancesInRegistry[instanceName]);
                }
                if (serviceName == "StorageServer")
                {
                    if (instancesInRegistry.Contains("default"))
                        instanceNames.Add(instancesInRegistry["default"]);

                }
                
            }
            string[] stringArray = new string[instanceNames.Count];
            int i = 0;
            foreach (string instanceName in instanceNames)
            {
                stringArray[i] = instanceName;
                i++;
            }
            return stringArray; 
        }
        /// <MetaDataID>{e502a679-da37-486b-8df7-83dd861552e2}</MetaDataID>
        public PersistenceLayer.IPersistencyService GetPersistencyService(string instanceName)
        {
            //#region MonoStateClass
            //if (this != MonoStateInstance)
            //    return MonoStateInstance.GetPersistencyService(instanceName);
            //#endregion
            Microsoft.Win32.RegistryKey storageServerKey =null;
            if(instanceName==null||instanceName.Trim().Length==0)
                storageServerKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\HandySoft\StorageServerInstances\Default", false);
            else
                storageServerKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\HandySoft\StorageServerInstances\" + instanceName, false);
            if (storageServerKey == null)
                throw new System.Exception(string.Format("There isn't server with instance name {0} at {1}.", instanceName,System.Net.Dns.GetHostName()));
            string TCPPort = storageServerKey.GetValue("TCPPort") as string;

            Remoting.RemotingServices remotingServices = Remoting.RemotingServices.GetRemotingServices("tcp://127.0.0.1:"+TCPPort) as Remoting.RemotingServices;
            return remotingServices.CreateInstance("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b") as PersistenceLayer.IPersistencyService;
        }

        /// <MetaDataID>{af5a9902-2223-43c7-87ca-7c41e59c8362}</MetaDataID>
        public string GetTCPPort(string instanceName)
        {
            //#region MonoStateClass
            //if (this != MonoStateInstance)
            //    return MonoStateInstance.GetTCPPort(instanceName);
            //#endregion

            Microsoft.Win32.RegistryKey storageServerKey = null;
            if (instanceName == null || instanceName.Trim().Length == 0)
                storageServerKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\HandySoft\StorageServerInstances\Default", false);
            else
                storageServerKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\HandySoft\StorageServerInstances\" + instanceName, false);
            return (storageServerKey.GetValue("TCPPort") as string).Trim();
        }

        /// <MetaDataID>{a42a172f-34ec-43e0-a076-cb5886bd767c}</MetaDataID>
        public System.ServiceProcess.ServiceControllerStatus GetServiceStatus(string CurrentInstanceName)
        {
            System.ServiceProcess.ServiceController serviceController = GetServiceForInstance(CurrentInstanceName);
            if (serviceController != null)
            {
                serviceController.Refresh();
                return serviceController.Status;
            }
            else
                return System.ServiceProcess.ServiceControllerStatus.Stopped;
        }
#endif

    }
}
