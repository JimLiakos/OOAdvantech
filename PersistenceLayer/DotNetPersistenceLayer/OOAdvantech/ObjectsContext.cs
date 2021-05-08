using OOAdvantech.Remoting;

using System.Collections.Generic;
using System.Text;


#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif




namespace OOAdvantech
{
    
    /// <summary>
    /// Objects Context is the context where manages the life cycle of object.
    /// In case of persistent object, Objects Context manages also active and passive state.
    /// </summary>
    /// <MetaDataID>{bfc09c48-2bad-44f8-998b-41f4f34b2e13}</MetaDataID>
    public abstract class ObjectsContext : MarshalByRefObject, Remoting.IExtMarshalByRefObject
    {

        /// <MetaDataID>{4e147240-ce55-423f-af32-c7ebab22bf6c}</MetaDataID>
        public static void Init(System.Reflection.Assembly[] assemblies=null )
        {    
            ModulePublisher.ClassRepository.Init();
        }
        /// <MetaDataID>{7d0df1b8-7d51-4247-ae5f-9fc35baa612f}</MetaDataID>
        public abstract string Identity
        {
            get;
        }
    }
}
