using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ModulePublisher;

namespace OOAdvantech
{
    /// <MetaDataID>{77f9518f-786b-4ba7-a56c-f3c0802fa202}</MetaDataID>
    public class TypeLoader : ModulePublisher.ITypeLoader
    {
        Type ITypeLoader.GetType(string classFullName, string assemblyData)
        {

            if (string.IsNullOrWhiteSpace(assemblyData))
            {
                foreach (var loadedAssembly in LoadedAssemblies.Values)
                {
                    var type = loadedAssembly.GetType(classFullName);
                    if(type!=null)
                        return type;
                }

            }
            else
            {
                var assembly = System.Reflection.Assembly.Load(new AssemblyName(assemblyData));
                System.Type type = assembly.GetType(classFullName);
                if(type!=null)
                    SetAssemblyMetaData(type);
                else
                {

                }
                return type;
            }

            


            //System.Reflection.Assembly.Load(new AssemblyName())
            //sds.GetType()

            throw new NotImplementedException();
        }

        static Dictionary<string, System.Reflection.Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        public static void SetAssemblyMetaData(Type type)
        {
            if(!LoadedAssemblies.ContainsKey(type.GetMetaData().Assembly.FullName))
                LoadedAssemblies[type.GetMetaData().Assembly.FullName] = type.GetMetaData().Assembly;
        }




        Assembly ITypeLoader.LoadAssembly(string assemblyFullName)
        {

            if (string.IsNullOrWhiteSpace(assemblyFullName))
            {
                foreach (var loadedAssembly in LoadedAssemblies.Values)
                {
                    if (loadedAssembly.FullName == assemblyFullName)
                        return loadedAssembly;
                }
                return null;
            }
            else
            {
                var assembly = System.Reflection.Assembly.Load(new AssemblyName(assemblyFullName));
                return assembly;
            }
        }
    }
}
