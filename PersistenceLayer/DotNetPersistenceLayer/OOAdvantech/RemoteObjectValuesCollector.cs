using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Remoting
{

    /// <MetaDataID>{693cf41a-be79-4bc6-ba8b-ae87eb45d31e}</MetaDataID>
    public interface IObjectValuesCollectorService
    {
        /// <MetaDataID>{4c6895a0-00fe-497c-8eb0-dafbb37baf05}</MetaDataID>
        Collections.StructureSet GetValues(object _object, OOAdvantech.Collections.Generic.List<string> paths);
        /// <MetaDataID>{e97ae5b0-4f16-4b13-a084-98caffa9c6d3}</MetaDataID>
        Collections.StructureSet GetValues(OOAdvantech.Collections.Generic.List<object> objectCollection, Type type, OOAdvantech.Collections.Generic.List<string> paths);
    }

    /// <MetaDataID>{76267040-0861-4844-b91a-ae3811d982d4}</MetaDataID>
    public class RemoteObjectValuesCollector:MarshalByRefObject
    {
        
        static public Collections.StructureSet GetValues(MarshalByRefObject remoteObject, OOAdvantech.Collections.Generic.List<string> paths)
        {
            if (paths.Count == 0)
                return null;
            IObjectValuesCollectorService objectValuesCollectorService=null;
            if (System.Environment.Version.Major == 4)
            {
                if (Remoting.RemotingServices.IsOutOfProcess(remoteObject))
                {
                    objectValuesCollectorService = Remoting.RemotingServices.CreateRemoteInstance(Remoting.RemotingServices.GetChannelUri(remoteObject), "OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0,  Culture=neutral, PublicKeyToken=95eeb2468d93212b") as IObjectValuesCollectorService;
                    return objectValuesCollectorService.GetValues(remoteObject, paths);//ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.ClientSide.StructureSetAgent", "", , new Type[1] { typeof(Collections.StructureSet) }) as Collections.StructureSet;
                }
                else
                {
                    objectValuesCollectorService = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0,  Culture=neutral, PublicKeyToken=95eeb2468d93212b")) as IObjectValuesCollectorService;
                    return objectValuesCollectorService.GetValues(remoteObject, paths);
                }
            }
            else
            {
                ///TODO να γραφτεί τεστ σενάριο για την περίπτωση που κοπεί η επικοινωνία με τον απέναντη
                if (Remoting.RemotingServices.IsOutOfProcess(remoteObject))
                {
                    objectValuesCollectorService = Remoting.RemotingServices.CreateRemoteInstance(Remoting.RemotingServices.GetChannelUri(remoteObject), "OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b") as IObjectValuesCollectorService;
                    return objectValuesCollectorService.GetValues(remoteObject, paths);//ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.ClientSide.StructureSetAgent", "", , new Type[1] { typeof(Collections.StructureSet) }) as Collections.StructureSet;
                }
                else
                {
                    objectValuesCollectorService = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b")) as IObjectValuesCollectorService;
                    return objectValuesCollectorService.GetValues(remoteObject, paths);
                }
            }
        }

        static public Collections.StructureSet GetValues(System.Collections.IEnumerable objectCollection,Type type, OOAdvantech.Collections.Generic.List<string> paths)
        {
            System.Collections.Generic.Dictionary<string, OOAdvantech.Collections.Generic.List<object>> valuesCollectors = new Dictionary<string, OOAdvantech.Collections.Generic.List<object>>();

            foreach (object obj in objectCollection)
            {
                if (obj is MarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(obj as MarshalByRefObject))
                {
                    string channelUri = Remoting.RemotingServices.GetChannelUri(obj as MarshalByRefObject);
                    if (!valuesCollectors.ContainsKey(channelUri))
                        valuesCollectors[channelUri] = new OOAdvantech.Collections.Generic.List<object>();
                    valuesCollectors[channelUri].Add(obj);
                }
                else
                {
                    if (!valuesCollectors.ContainsKey("(local)"))
                        valuesCollectors["(local)"] = new OOAdvantech.Collections.Generic.List<object>();
                    valuesCollectors["(local)"].Add(obj);
                }

            }

            System.Collections.Generic.List<Collections.StructureSet> structureSets = new List<OOAdvantech.Collections.StructureSet>();
            foreach (System.Collections.Generic.KeyValuePair<string, OOAdvantech.Collections.Generic.List<object>> entry in valuesCollectors)
            {
                IObjectValuesCollectorService objectValuesCollectorService = null;
                if (System.Environment.Version.Major == 4)
                {

                    if (entry.Key == "(local)")
                    {

                        objectValuesCollectorService = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0,  Culture=neutral, PublicKeyToken=95eeb2468d93212b")) as IObjectValuesCollectorService;
                        structureSets.Add(objectValuesCollectorService.GetValues(entry.Value, type, paths));
                    }
                    else
                    {
                        objectValuesCollectorService = Remoting.RemotingServices.CreateRemoteInstance(entry.Key, "OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0,  Culture=neutral, PublicKeyToken=95eeb2468d93212b") as IObjectValuesCollectorService;
                        structureSets.Add(objectValuesCollectorService.GetValues(entry.Value, type, paths));
                    }

                }
                else
                {
                    if (entry.Key == "(local)")
                    {

                        objectValuesCollectorService = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b")) as IObjectValuesCollectorService;
                        structureSets.Add(objectValuesCollectorService.GetValues(entry.Value, type, paths));
                    }
                    else
                    {
                        objectValuesCollectorService = Remoting.RemotingServices.CreateRemoteInstance(entry.Key, "OOAdvantech.PersistenceLayerRunTime.ObjectValuesCollectorService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b") as IObjectValuesCollectorService;
                        structureSets.Add(objectValuesCollectorService.GetValues(entry.Value, type, paths));
                    }

                }
            }



            if (structureSets.Count == 1)// && !Remoting.RemotingServices.IsOutOfProcess(structureSets[0]))
                return structureSets[0];
            else
            {
                if (structureSets.Count == 0)
                    return null;

                throw new System.NotImplementedException("There isn't implementation for multiply StructureSets");
//#if Net4
//                return AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StructureSetAgent", "DotNetMetaDataRepository, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c"), new Type[1] { typeof(System.Collections.Generic.List<Collections.StructureSet>) }, structureSets) as Collections.StructureSet;
//#else
                return AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StructureSetAgent", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7"), new Type[1] { typeof(System.Collections.Generic.List<Collections.StructureSet>) }, structureSets) as Collections.StructureSet;
//#endif
            }
        }
    }
}
