using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms
{
    /// <MetaDataID>{bf9dc752-a6bc-44f3-9e1a-b2ac25911f7c}</MetaDataID>
    public class DependencyService
    {


        static DependencyService()
        {
            Xamarin.Forms.DependencyService.Register<OOAdvantech.Net.DeviceInstantiator>();
        }


        /// <MetaDataID>{00088c1b-92cc-4011-a7be-6346da9ab8b2}</MetaDataID>
        public static T Get<T>() where T : class
        {
            foreach (var type in Types)
            {
                if (typeof(T).IsAssignableFrom(type))
                    return System.Activator.CreateInstance(type) as T;
            }
            return null;
        }

        /// <MetaDataID>{897e7141-1922-4b35-9bc2-696077f58ed1}</MetaDataID>
        static List<Type> Types = new List<Type>();

        /// <MetaDataID>{883ce2e1-992e-49d1-80d3-f6afec9e06f2}</MetaDataID>
        public static void Register<T>() where T : class
        {
            Register(typeof(T));
        }

        /// <MetaDataID>{99e48f23-bb96-4385-b841-f43697bcc72f}</MetaDataID>
        public static void Register(Type type)
        {
            if (!Types.Contains(type))
                Types.Add(type);
        }
    }

    //
    // Summary:
    //     An attribute that indicates that the specified type provides a concrete implementation
    //     of a needed interface.
    //
    // Remarks:
    //     To be added.
    /// <MetaDataID>{a286b84d-2147-4e68-bac7-06660b401a06}</MetaDataID>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class DependencyAttribute : Attribute
    {
        /// <MetaDataID>{2795fdc2-2e2e-454b-9a61-e218e80d9fc4}</MetaDataID>
        public DependencyAttribute(Type implementorType)
        {
            DependencyService.Register(implementorType);
        }
    }
}
