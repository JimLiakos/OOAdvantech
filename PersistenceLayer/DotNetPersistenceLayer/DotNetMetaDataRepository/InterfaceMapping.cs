using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <summary>
    /// Retrieves the mapping of an interface into the actual methods on a class
    /// that implements that interface.
    /// </summary>
    /// <MetaDataID>{10d061a1-8f6d-4749-8f54-110a29e3aab8}</MetaDataID>
    public struct InterfaceMapping
    {
        /// <summary>
        /// Shows the methods that are defined on the interface.
        /// </summary>
        /// <MetaDataID>{aefaa65a-7149-4854-beac-f827e44afc2a}</MetaDataID>
        public MethodInfo[] InterfaceMethods;
        /// <summary>
        /// Shows the type that represents the interface.
        /// </summary>
        /// <MetaDataID>{a59d3b47-a23f-49f0-a8ea-579922f8453c}</MetaDataID>
        public System.Type InterfaceType;

        /// <summary>
        /// Shows the methods that implement the interface.
        /// </summary>
        /// <MetaDataID>{750893a4-f68d-42fe-8bb7-f7b0c9c9916e}</MetaDataID>
        public MethodInfo[] TargetMethods;


        /// <summary>
        /// Represents the type that was used to create the interface mapping.
        /// </summary>
        /// <MetaDataID>{df81e65b-b48a-4468-83ee-13e7af570e98}</MetaDataID>
        public System.Type TargetType;
    }
}
