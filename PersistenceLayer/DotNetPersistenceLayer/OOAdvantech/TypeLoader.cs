using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OOAdvantech
{
    /// <MetaDataID>{dcaf7692-e259-49b2-bec2-c0d3b7f6ad4e}</MetaDataID>
    public class TypeLoader : ModulePublisher.ITypeLoader
    {
        static System.Reflection.Assembly DotNetMetaDataRepository;
        static System.Reflection.Assembly PersistenceLayerRunTime;
        static System.Reflection.Assembly Parser;
        static System.Reflection.Assembly RDBMSMetaDataRepository;
        static System.Reflection.Assembly RDBMSMetaDataPersistenceRunTime;
        static System.Reflection.Assembly RDBMSPersistenceRunTime;
        static System.Reflection.Assembly MetaDataLoadingSystem;
        static System.Reflection.Assembly MSSQLPersistenceRunTime;
        static System.Reflection.Assembly MSSQLFastPersistenceRunTime;

        static System.Reflection.Assembly LoadModule(string moduleName)
        {
             
            foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (loadedAssembly.GetName().Name == moduleName)
                    return loadedAssembly;
            }

            System.Reflection.Assembly encryptedAssembly = null;
            var assembly = typeof(TypeLoader).Assembly;
            var assemblyResourceName = string.Format("OOAdvantech.Libs.{0}.dll", moduleName);
            var debugInfoResourceName = string.Format("OOAdvantech.Libs.{0}.pdb", moduleName);


            using (Stream stream = assembly.GetManifestResourceStream(assemblyResourceName))
            {
                byte[] rawAssembly = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(rawAssembly, 0, rawAssembly.Length);
                using (Stream debugStream = assembly.GetManifestResourceStream(debugInfoResourceName))
                {
                    byte[] rawDebugInfo = new byte[debugStream.Length];
                    debugStream.Position = 0;
                    debugStream.Read(rawDebugInfo, 0, rawDebugInfo.Length);

                    //DotNetMetaDataRepository = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(rawAssembly);
                    encryptedAssembly = AssemblyNativeCode.AssemblyLoader.LoadRawAssemblyDebug(rawAssembly, rawDebugInfo);
                    return encryptedAssembly;
                }
            }
        }


        public System.Reflection.Assembly LoadAssembly(string assemblyFullName)
        {
            LoadEncryptedModules();

            if (DotNetMetaDataRepository != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, DotNetMetaDataRepository.FullName))
                return DotNetMetaDataRepository;
            if (PersistenceLayerRunTime != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, PersistenceLayerRunTime.FullName))
                return PersistenceLayerRunTime;
            if (Parser != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, Parser.FullName))
                return Parser;
            if (RDBMSMetaDataRepository != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, RDBMSMetaDataRepository.FullName))
                return RDBMSMetaDataRepository;
            if (RDBMSMetaDataPersistenceRunTime != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, RDBMSMetaDataPersistenceRunTime.FullName))
                return RDBMSMetaDataPersistenceRunTime;
            if (RDBMSPersistenceRunTime != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, RDBMSPersistenceRunTime.FullName))
                return RDBMSPersistenceRunTime;
            if (MetaDataLoadingSystem != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, MetaDataLoadingSystem.FullName))
                return MetaDataLoadingSystem;
            if (MSSQLPersistenceRunTime != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, MSSQLPersistenceRunTime.FullName))
                return MSSQLPersistenceRunTime;
            if (MSSQLFastPersistenceRunTime != null && ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyFullName, MSSQLFastPersistenceRunTime.FullName))
                return MSSQLFastPersistenceRunTime;

            return null;
        }


        public Type GetType(string classFullName, string assemblyData)
        {
            //return null;
            LoadEncryptedModules();
            Type type = null;
            type = GetTypeFromAssembly(classFullName, assemblyData, DotNetMetaDataRepository);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, PersistenceLayerRunTime);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, Parser);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, RDBMSMetaDataRepository);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, RDBMSMetaDataPersistenceRunTime);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, RDBMSPersistenceRunTime);
            if (type != null)
                return type;
            type = GetTypeFromAssembly(classFullName, assemblyData, MetaDataLoadingSystem);
            if (type != null)
                return type;

            type = GetTypeFromAssembly(classFullName, assemblyData, MSSQLPersistenceRunTime);
            if (type != null)
                return type;

            type = GetTypeFromAssembly(classFullName, assemblyData, MSSQLFastPersistenceRunTime);
            if (type != null)
                return type;



            return null;
        }

        private static void LoadEncryptedModules()
        {

            //if (DotNetMetaDataRepository == null)
            //    DotNetMetaDataRepository = LoadModule("DotNetMetaDataRepository");
            //if (PersistenceLayerRunTime == null)
            //    PersistenceLayerRunTime = LoadModule("PersistenceLayerRunTime");
            //if (Parser == null)
            //    Parser = LoadModule("Parser");
            ////if (RDBMSMetaDataRepository == null)
            ////    RDBMSMetaDataRepository = LoadModule("RDBMSMetaDataRepository");
            //if (RDBMSMetaDataPersistenceRunTime == null)
            //    RDBMSMetaDataPersistenceRunTime = LoadModule("RDBMSMetaDataPersistenceRunTime");
            //if (RDBMSPersistenceRunTime == null)
            //    RDBMSPersistenceRunTime = LoadModule("RDBMSPersistenceRunTime");
            //if (MetaDataLoadingSystem == null)
            //    MetaDataLoadingSystem = LoadModule("MetaDataLoadingSystem");
            //if (MSSQLPersistenceRunTime == null)
            //    MSSQLPersistenceRunTime = LoadModule("MSSQLPersistenceRunTime");
            //if (MSSQLFastPersistenceRunTime == null)
            //    MSSQLFastPersistenceRunTime = LoadModule("MSSQLFastPersistenceRunTime");


        }

        private static Type GetTypeFromAssembly(string classFullName, string assemblyData, System.Reflection.Assembly assembly)
        {

            if (assembly == null)
                return null;
            if (string.IsNullOrEmpty(assemblyData) || ModulePublisher.ClassRepository.ClassVersionDataMatchAllWithAssembly(assemblyData, assembly.FullName))
            {
                Type type = assembly.GetType(classFullName);
                if (type != null)
                    return type;
            }
            if (string.IsNullOrEmpty(assemblyData) || ModulePublisher.ClassRepository.ClassVersionDataMatchWithAssembly(assemblyData, assembly.FullName))
            {
                Type type = assembly.GetType(classFullName);
                if (type != null)
                    return type;
            }

            return null;
        }
    }
}
