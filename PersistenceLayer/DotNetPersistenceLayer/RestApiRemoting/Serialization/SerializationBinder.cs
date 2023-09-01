using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;
using static OOAdvantech.Json.Serialization.DefaultSerializationBinder;

#if NetStandard
using TypeNameKey = OOAdvantech.Json.Utilities.StructMultiKey<string,string>;
#else
using TypeNameKey = OOAdvantech.Json.Utilities.StructMultiKey<string, string>;
#endif

namespace OOAdvantech.Remoting.RestApi.Serialization
{

    /// <MetaDataID>{2b7851b1-e8da-4690-ba7b-a85d2c577549}</MetaDataID>
    public class SerializationBinder : DefaultSerializationBinder
    {


        JsonSerializationFormat SerializationFormat;
        //bool Web;
        ThreadSafeStore<TypeNameKey, Type> _typeCache;
        public SerializationBinder(JsonSerializationFormat serializationFormat)
        {
            //  Web = web;

            SerializationFormat = serializationFormat;

            _typeCache = new ThreadSafeStore<TypeNameKey, Type>(GetTypeFromTypeNameKey);
        }
        public static Dictionary<string, Type> NamesTypesDictionary = new Dictionary<string, Type>();
        public static Dictionary<Type, string> TypesNamesDictionary = new Dictionary<Type, string>();


        private Type GetTypeFromTypeNameKey(TypeNameKey typeNameKey)
        {
#if NetStandard
            string assemblyName = typeNameKey.Value1;
            string typeName = typeNameKey.Value2;
#elif Json4
            string assemblyName = typeNameKey.Value1;
            string typeName = typeNameKey.Value2;
#else
            string assemblyName = typeNameKey.AssemblyName;
            string typeName = typeNameKey.TypeName;

#endif
            if (assemblyName != null)
            {
                Assembly assembly;

#if !(DOTNET || PORTABLE40 || PORTABLE)
                // look, I don't like using obsolete methods as much as you do but this is the only way
                // Assembly.Load won't check the GAC for a partial name
#pragma warning disable 618, 612
                assembly = Assembly.LoadWithPartialName(assemblyName);
#pragma warning restore 618, 612
#elif DOTNET || PORTABLE
                assembly = Assembly.Load(new AssemblyName(assemblyName));
#else
                assembly = Assembly.Load(assemblyName);
#endif

#if HAVE_APP_DOMAIN
                if (assembly == null)
                {
                    // will find assemblies loaded with Assembly.LoadFile outside of the main directory
                    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly a in loadedAssemblies)
                    {
                        // check for both full name or partial name match
                        if (a.FullName == assemblyName || a.GetName().Name == assemblyName)
                        {
                            assembly = a;
                            break;
                        }
                    }
                }
#endif

                if (assembly == null)
                {
                    try
                    {
                        return Type.GetType(typeName);
                    }
                    catch (Exception error)
                    {
                        throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, assemblyName));

                    }
                    
                }
                Type type = null;
                if (typeName.IndexOf('`') >= 0)
                {
                    // if generic type, try manually parsing the type arguments for the case of dynamically loaded assemblies
                    // example generic typeName format: System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
                    try
                    {
                        type = GetGenericTypeFromTypeName(typeName, assembly);
                    }
                    catch (Exception ex)
                    {
                        throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName), ex);
                    }
                }
                else
                    type = assembly.GetType(typeName);


                if (type == null)
                {
                    throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
                }

                
                return type;
            }
            else
            {
                return Type.GetType(typeName);
            }
        }
        private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
        {
            Type type = null;
            int openBracketIndex = typeName.IndexOf('[');
            int closeBracketIndex = typeName.LastIndexOf(']');
         
           
            if (openBracketIndex >= 0)
            {
                string genericTypeDefName = typeName.Substring(0, openBracketIndex);
                Type genericTypeDef = assembly.GetType(genericTypeDefName);
                if (genericTypeDef != null)
                {
                    List<Type> genericTypeArguments = new List<Type>();
                    int scope = 0;
                    int typeArgStartIndex = 0;
                    int endIndex = typeName.Length - 1;
                    for (int i = openBracketIndex + 1; i < endIndex; ++i)
                    {
                        if (i >= typeName.Length)
                        {

                        }
                        char current = typeName[i];
                        switch (current)
                        {
                            case '[':
                                if (scope == 0)
                                {
                                    typeArgStartIndex = i + 1;
                                }
                                ++scope;
                                break;
                            case ']':
                                --scope;
                                if (scope == 0)
                                {
                                    string typeArgAssemblyQualifiedName = typeName.Substring(typeArgStartIndex, i - typeArgStartIndex);
                                    string scopeTypeName = null;
#if DeviceDotNet
                                    TypeNameKey typeNameKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName);
#if NetStandard
                                    string assemblyName = typeNameKey.Value1;
                                    scopeTypeName = typeNameKey.Value2;
#else
                                    string assemblyName = typeNameKey.AssemblyName;
                                    scopeTypeName  = typeNameKey.TypeName;
#endif
                                    genericTypeArguments.Add(BindToType(assemblyName, scopeTypeName ));
#elif Json4
                                    TypeNameKey typeNameKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName);
                                    string assemblyName = typeNameKey.Value1;
                                    scopeTypeName = typeNameKey.Value2;

                                    var argType = BindToType(assemblyName, scopeTypeName);
                                    if (argType == null)
                                        throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
                                    genericTypeArguments.Add(argType);
#else
                                    string theTypeName;
                                    string assemblyName;
                                    ReflectionUtils.SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName, out theTypeName, out assemblyName);
                                    var argType =BindToType(assemblyName, theTypeName);
                                    
                                    if(argType==null)
                                        throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));

                                    genericTypeArguments.Add(argType);
#endif
                                }
                                break;
                        }
                    }

                    try
                    {
                        if(genericTypeArguments.Count>0)
                            type = genericTypeDef.MakeGenericType(genericTypeArguments.ToArray());
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }
            }

            return type;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type type = null;
            //if (assemblyName == null)
            //if(typeName == "FlavourBusinessManager.RoomService.ItemPreparation")
            //{

            //}
            if (NamesTypesDictionary.TryGetValue(typeName, out type))
                return type;

#if DeviceDotNet
            return _typeCache.Get(new TypeNameKey(assemblyName, typeName));
#else
            type = _typeCache.Get(new TypeNameKey(assemblyName, typeName));
            if (type == null)
            {
                type = ModulePublisher.ClassRepository.GetType(typeName, assemblyName);
                return type;
            }
            else
            {
                var overrideType = GetGenericTypeFromTypeName(typeName, type.Assembly);
                if (overrideType != null)
                    return overrideType;
                return type;
            }

            //return base.BindToType(assemblyName, typeName);
#endif
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization)
            {
                if (serializedType.GetMetaData().IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(List<>))
                    serializedType = typeof(List<>);
                if (serializedType.GetMetaData().IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    serializedType = typeof(Dictionary<,>);
                if (serializedType == typeof(System.Collections.Hashtable))
                    serializedType = typeof(Dictionary<,>);
                if (serializedType?.BaseType?.FullName == "System.Array")
                    serializedType = serializedType.BaseType;
                if (TypesNamesDictionary.TryGetValue(serializedType, out typeName))
                    assemblyName = null;
                else if (serializedType.GetMetaData().IsGenericType && TypesNamesDictionary.TryGetValue(serializedType.GetGenericTypeDefinition(), out typeName))
                    assemblyName = null;
                else
                {
                    //serializedType.GetCustomAttribute<>
                    base.BindToName(serializedType, out assemblyName, out typeName);
                }
            }
            else
            {
                //if (TypesNamesDictionary.TryGetValue(serializedType, out typeName))
                //    assemblyName = null;
                base.BindToName(serializedType, out assemblyName, out typeName);
            }
        }

    }
    /// <MetaDataID>{f7083664-584c-48c3-b49c-acfa86f7684a}</MetaDataID>
    internal class ThreadSafeStore<TKey, TValue>
    {
        private readonly object _lock = new object();
        private Dictionary<TKey, TValue> _store;
        private readonly System.Func<TKey, TValue> _creator;

        public ThreadSafeStore(System.Func<TKey, TValue> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            _creator = creator;
            _store = new Dictionary<TKey, TValue>();
        }

        public TValue Get(TKey key)
        {
            TValue value;
            if (!_store.TryGetValue(key, out value))
            {
                return AddValue(key);
            }

            return value;
        }

        private TValue AddValue(TKey key)
        {
            TValue value = _creator(key);

            lock (_lock)
            {
                if (_store == null)
                {
                    _store = new Dictionary<TKey, TValue>();
                    _store[key] = value;
                }
                else
                {
                    // double check locking
                    TValue checkValue;
                    if (_store.TryGetValue(key, out checkValue))
                    {
                        return checkValue;
                    }

                    Dictionary<TKey, TValue> newStore = new Dictionary<TKey, TValue>(_store);
                    newStore[key] = value;

#if HAVE_MEMORY_BARRIER
                    Thread.MemoryBarrier();
#endif
                    _store = newStore;
                }

                return value;
            }
        }
    }


}
