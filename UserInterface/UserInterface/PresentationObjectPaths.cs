using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{f3fedc23-c44b-402c-beed-d4b41b5293b0}</MetaDataID>
    [AttributeUsage(AttributeTargets.Class)]
    public class PresentationObjectPaths:System.Attribute
    {

        public readonly string[] Paths;
        public PresentationObjectPaths(string[] paths)
        {
            Paths = paths;
        }
        public static string[] GetExtraPathsFor(System.Type type)
        {

            object[] attributes = type.GetCustomAttributes(typeof(PresentationObjectPaths), true);
            int pathsCount = 0;
            foreach (PresentationObjectPaths attribute in attributes)
                pathsCount += attribute.Paths.Length;

            string[] paths = new string[pathsCount];
            int i = 0;
            foreach (PresentationObjectPaths attribute in attributes)
                foreach (string path in attribute.Paths)
                    paths[i++] = path;
            return paths;

        }
    }
}
