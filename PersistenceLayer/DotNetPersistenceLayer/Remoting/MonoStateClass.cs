
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.Serialization;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;

#endif

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{c7b3a5f5-e5fe-4269-aba1-54852689b08e}</MetaDataID>

    public class MonoStateClass : MarshalByRefObject, IExtMarshalByRefObject
    {

        public MonoStateClass()
        {
            lock (MonoStateClassesInstance)
            {
                if (!MonoStateClassesInstance.ContainsKey(GetType()))
                    MonoStateClassesInstance[GetType()] = this;
                else
                    throw new MonoStateClassException("System can't create two instance for monostate class '" + GetType().FullName + "'");
            }
            System.Runtime.Remoting.RemotingServices.Marshal(this, @"#MonoStateClass#" + GetType().FullName + @"\" + RemotingServices.MonoStateClassChannelUri);
        }
        static System.Collections.Generic.Dictionary<System.Type, MonoStateClass> MonoStateClassesInstance = new Dictionary<System.Type, MonoStateClass>();

        public static MonoStateClass GetInstance(System.Type type)
        {
            return GetInstance(type, false);
        }
        public static MonoStateClass GetInstance(System.Type type, bool create)
        {

            MonoStateClass instance = null;
            lock (MonoStateClassesInstance)
            {
                MonoStateClassesInstance.TryGetValue(type, out instance);
            }
            if (instance != null)
                return instance;

            if (create)
            {
                try
                {
                    instance = System.Activator.CreateInstance(type) as MonoStateClass;
                }
                catch (System.Reflection.TargetInvocationException error)
                {
                    if (error.InnerException is MonoStateClassException)
                    {
                        lock (MonoStateClassesInstance)
                        {
                            MonoStateClassesInstance.TryGetValue(type, out instance);
                            return instance;
                        }
                    }
                    throw error;

                }
                lock (MonoStateClassesInstance)
                {
                    if (!MonoStateClassesInstance.ContainsKey(type))
                    {
                        MonoStateClassesInstance[type] = instance;
                        return instance;
                    }
                    else
                        return MonoStateClassesInstance[type];
                }
            }
            else
                return null;
        }


    }
    /// <MetaDataID>{ff8e3f9d-15f4-429d-b79f-2830c07e0436}</MetaDataID>
    public class MonoStateClassException : System.Exception
    {
        public MonoStateClassException()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public MonoStateClassException(string message) : base(message)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public MonoStateClassException(string message, System.Exception innerException) : base(message, innerException)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with serialized data.
        //
        // Parameters:
        //   info:
        //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        //     object data about the exception being thrown.
        //
        //   context:
        //     The System.Runtime.Serialization.StreamingContext that contains contextual information
        //     about the source or destination.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The info parameter is null.
        //
        //   T:System.Runtime.Serialization.SerializationException:
        //     The class name is null or System.Exception.HResult is zero (0).
        [SecuritySafeCritical]
        protected MonoStateClassException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

    }
}
