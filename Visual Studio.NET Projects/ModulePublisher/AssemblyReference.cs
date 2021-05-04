using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
namespace ModulePublisher
{
#if DeviceDotNet 
    /// <MetaDataID>{6b04dac7-9b1f-41e4-bbcd-55a69b14dee0}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class AssemblyReferences : System.Attribute
    {
        public AssemblyReferences(string[] references)
        {
            References = references.ToList();

        }
        public readonly System.Collections.Generic.List<string> References;
    }
#endif
}

