using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{6781a9ca-e3d5-43c0-b569-03f9e0704098}</MetaDataID>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class CollectionObjectPaths : System.Attribute
    {
        public readonly string[] Paths;
        public CollectionObjectPaths(string[] paths)
        {
            Paths = paths;
        }

        public static string[] GetPathsFor(System.Reflection.MemberInfo member)
        {
            if (member == null)
                return new string[0];



            if (member is System.Reflection.MethodInfo 
                && member.Name.IndexOf("get_") == 0
                && (member as System.Reflection.MethodInfo).DeclaringType.GetProperty(member.Name.Substring("get_".Length))!=null)
            {
                member = (member as System.Reflection.MethodInfo).DeclaringType.GetProperty(member.Name.Substring("get_".Length));
            }
          

            object[] attributes = member.GetCustomAttributes(typeof(CollectionObjectPaths), true);
            int pathsCount = 0;
            foreach (CollectionObjectPaths attribute in attributes)
                pathsCount += attribute.Paths.Length;

            string[] paths = new string[pathsCount];
            int i = 0;
            foreach (CollectionObjectPaths attribute in attributes)
                foreach (string path in attribute.Paths)
                    paths[i++] = path;
            return paths;

        }
    }
}
