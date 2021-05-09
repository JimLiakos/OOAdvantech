using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{c7b3a5f5-e5fe-4269-aba1-54852689b08e}</MetaDataID>
   	
	public class MonoStateClass:MarshalByRefObject,IExtMarshalByRefObject
	{

        public MonoStateClass()
        {
            
            if (!MonoStateClassesInstance.ContainsKey(GetType()))
                MonoStateClassesInstance[GetType()] = this;
            else
                throw new System.Exception("System can't create two instance for monostate class '" + GetType().FullName + "'");
            
            System.Runtime.Remoting.RemotingServices.Marshal(this, @"#MonoStateClass#" + GetType().FullName + @"\" + RemotingServices.MonoStateClassChannelUri); 
        }
        static System.Collections.Generic.Dictionary<System.Type, MonoStateClass> MonoStateClassesInstance = new Dictionary<Type, MonoStateClass>();

        public static MonoStateClass GetInstance(Type type )
        {
            return GetInstance(type, false);
        }
        public static MonoStateClass GetInstance(Type type,bool create)
        {
            lock (MonoStateClassesInstance)
            {
                if (MonoStateClassesInstance.ContainsKey(type))
                    return MonoStateClassesInstance[type];
                else if (create)
                {
                    MonoStateClass monoStateClass = System.Activator.CreateInstance(type) as MonoStateClass;
                    MonoStateClassesInstance[type] = monoStateClass;
                    return monoStateClass;
                }
                else
                    return null;
            }
        }

    }
}
