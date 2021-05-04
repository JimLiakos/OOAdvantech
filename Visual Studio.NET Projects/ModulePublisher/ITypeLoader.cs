using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulePublisher
{
    /// <MetaDataID>{a823195e-e929-4368-8dfd-bc2ddedcd1cf}</MetaDataID>
    public interface ITypeLoader
    {
        
        Type GetType(string classFullName, string assemblyData);
        System.Reflection.Assembly LoadAssembly(string assemblyFullName);
    }
}
