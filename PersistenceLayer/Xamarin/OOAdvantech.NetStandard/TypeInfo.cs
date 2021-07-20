using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
#if PORTABLE
using System.PCL.Reflection;
#endif

namespace OOAdvantech
{
    /// <MetaDataID>{2a434aa0-5bf5-4a3a-ae9b-400025ec2bf0}</MetaDataID>
    public class TypeInfo
    {
        private Type Type;

        public TypeInfo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            this.Type = type;
        }

        public bool IsGenericType
        {
            get
            {
                return Type.GetTypeInfo().IsGenericType;
            }
        }
        public bool IsGenericTypeDefinition
        {
            get
            {
                return Type.GetTypeInfo().IsGenericTypeDefinition;
            }
        }
        public bool ContainsGenericParameters
        {
            get
            {
                return Type.GetTypeInfo().ContainsGenericParameters;
            }
        }
        public bool IsValueType
        {
            get
            {
                return Type.GetTypeInfo().IsValueType;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return Type.GetTypeInfo().IsAbstract;
            }
        }
        public bool IsClass
        {
            get
            {
                return Type.GetTypeInfo().IsClass;
            }
        }
        public bool IsInterface
        {
            get
            {
                return Type.GetTypeInfo().IsInterface;
            }
        }

        public Type BaseType
        {
            get
            {
                return Type.GetTypeInfo().BaseType;
            }
        }

        public Assembly Assembly
        {
            get
            {
                return Type.GetTypeInfo().Assembly;
            }
        }

        public bool IsNestedPrivate
        {
            get
            {
                return Type.GetTypeInfo().IsNestedPrivate;
            }
        }

        public bool IsNestedPublic
        {
            get
            {
                return Type.GetTypeInfo().IsNestedPublic;
            }
        }

        public bool IsNestedFamily
        {
            get
            {
                return Type.GetTypeInfo().IsNestedFamily;
            }
        }

        public bool IsNestedAssembly
        {
            get
            {
                return Type.GetTypeInfo().IsNestedAssembly;
            }
        }

        public bool IsEnum
        {
            get
            {
                return Type.GetTypeInfo().IsEnum;
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return Type.GetTypeInfo().IsPrimitive;
            }
        }

        public bool IsPublic
        {
            get
            {
                return Type.GetTypeInfo().IsPublic;
            }

        }

        public bool IsNotPublic
        {
            get
            {
                return Type.GetTypeInfo().IsNotPublic;
            }
        }

        internal FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            System.Reflection.FieldInfo[] fieldInfos = (from field in Type.GetRuntimeFields()
                                                        where BindingFlagsMatch(field, bindingAttr)
                                                        select field).ToArray();
            return fieldInfos;
        }
        internal System.Reflection.FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            System.Reflection.FieldInfo fieldInfo = (from field in Type.GetRuntimeFields()
                                                     where field.Name == name && BindingFlagsMatch(field, bindingAttr)
                                                     select field).FirstOrDefault();

            return fieldInfo;
        }

        internal System.Reflection.FieldInfo GetField(string name)
        {
            System.Reflection.FieldInfo fieldInfo = (from field in Type.GetRuntimeFields()
                                                     where field.Name == name 
                                                     select field).FirstOrDefault();

            return fieldInfo;
        }

        internal System.Reflection.PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            var propertyInfo = (from field in Type.GetRuntimeProperties()
                                where field.Name == name && BindingFlagsMatch(field, bindingAttr)
                                select field).FirstOrDefault();

            return propertyInfo;
        }



        internal System.Reflection.PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            System.Reflection.PropertyInfo[] propertyInfos = (from propertyInfo in Type.GetRuntimeProperties()
                                                        where BindingFlagsMatch(propertyInfo, bindingAttr)
                                                        select propertyInfo).ToArray();
            return propertyInfos;
        }
        internal System.Reflection.PropertyInfo[] GetProperties()
        {
            return (from field in Type.GetRuntimeProperties()
                    select field).ToArray();
        }

        internal System.Reflection.MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            List<System.Reflection.MemberInfo> members = new List<MemberInfo>(); ;
            var fieldInfo = GetField(name, bindingAttr);
            if (fieldInfo != null)
                members.Add(fieldInfo);

            var propertyInfo = GetProperty(name, bindingAttr);
            if (propertyInfo != null)
                members.Add(propertyInfo);

            members.AddRange(GetMethods(name, bindingAttr).OfType<MemberInfo>());
            return members.ToArray();
        }



        internal PropertyInfo GetProperty(string name)
        {
            return Type.GetRuntimeProperty(name);
        }

        internal System.Reflection.MethodInfo GetMethod(string methodName, System.Type[] types)
        {

            return Type.GetRuntimeMethod(methodName, types);
        }
        public System.Reflection.MethodInfo GetMethod(string methodName)
        {

            System.Reflection.MethodInfo methodInfo = (from method in Type.GetRuntimeMethods()
                                                       where method.Name == methodName && BindingFlagsMatch(method, BindingFlags.Public)
                                                       select method).FirstOrDefault();
            return methodInfo;
        }

        //public ConstructorInfo[] GetConstructors()
        //{
        //    return (from ctor in Type.GetTypeInfo().DeclaredConstructors
        //                                                   ///where method.Name == methodName && BindingFlagsMatch(method, BindingFlags.Public)
        //                                               select ctor).ToArray();
        //}
        internal System.Reflection.MethodInfo GetMethod(string methodName, BindingFlags bindingAttr)
        {
            System.Reflection.MethodInfo methodInfo = (from method in Type.GetRuntimeMethods()
                                                       where method.Name == methodName && BindingFlagsMatch(method, bindingAttr)
                                                       select method).FirstOrDefault();

            return methodInfo;
        }
        internal System.Reflection.MethodInfo[] GetMethods(string methodName, BindingFlags bindingAttr)
        {
            return (from method in Type.GetRuntimeMethods()
                    where method.Name == methodName && BindingFlagsMatch(method, bindingAttr)
                    select method).ToArray();
        }


        internal System.Reflection.MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return (from method in Type.GetRuntimeMethods()
                    where BindingFlagsMatch(method, bindingAttr)
                    select method).ToArray();
        }

        internal System.Reflection.MethodInfo[] GetMethods()
        {
            return (from method in Type.GetRuntimeMethods()
                    select method).ToArray();
        }


        internal ConstructorInfo GetConstructor(BindingFlags bindingAttr, Type[] parameterTypes)
        {
            System.Reflection.ConstructorInfo[] ctrInfos = (from ctrInfo in Type.GetTypeInfo().DeclaredConstructors
                                                            where BindingFlagsMatch(ctrInfo, bindingAttr)
                                                            select ctrInfo).ToArray();

            foreach (var ctrInfo in ctrInfos)
            {
                var ctrParameterTypes = (from parameter in ctrInfo.GetParameters()
                                         select parameter.ParameterType).ToArray();

                if (ctrParameterTypes.Length != parameterTypes.Length)
                    continue;

                bool typesMatch = true;
                for (int i = 0; i != parameterTypes.Length; i++)
                {
                    if (!ctrParameterTypes[i].GetTypeInfo().IsAssignableFrom(parameterTypes[i].GetTypeInfo()))
                    {
                        typesMatch = false;
                        break;
                    }
                }
                if (typesMatch)
                    return ctrInfo;
            }
            return null;

        }

        internal EventInfo[] GetEvents()
        {
            return Type.GetRuntimeEvents().ToArray();
        }

        private bool BindingFlagsMatch(MethodBase method, BindingFlags bindingFlag)
        {

            if (method.IsStatic && ((bindingFlag | BindingFlags.Static) != 0))
                return true;
            if (method.IsPublic && ((bindingFlag | BindingFlags.Public) != 0))
                return true;
            if (method.IsPrivate && ((bindingFlag | BindingFlags.NonPublic) != 0))
                return true;
            if (method.IsFamily && ((bindingFlag | BindingFlags.NonPublic) != 0))
                return true;
            if (!method.IsStatic && ((bindingFlag | BindingFlags.Instance) != 0))
                return true;
            if (method.DeclaringType != Type && ((bindingFlag | BindingFlags.DeclaredOnly) != 0))
                return false;
            else
                return false;
        }

        private bool BindingFlagsMatch(PropertyInfo property, BindingFlags bindingFlag)
        {
            if (property.GetMethod != null)
                return BindingFlagsMatch(property.GetMethod, bindingFlag);
            else
                return BindingFlagsMatch(property.SetMethod, bindingFlag);

        }

        private bool BindingFlagsMatch(FieldInfo fieldInfo, BindingFlags bindingFlag)
        {
            if (fieldInfo.DeclaringType != Type && ((bindingFlag | BindingFlags.DeclaredOnly) != 0))
                return false;
            else
                return true;

            if (fieldInfo.IsStatic && ((bindingFlag | BindingFlags.Static) != 0))
                return true;
            if (fieldInfo.IsPublic && ((bindingFlag | BindingFlags.Public) != 0))
                return true;
            if (fieldInfo.IsPrivate && ((bindingFlag | BindingFlags.NonPublic) != 0))
                return true;
            if (fieldInfo.IsFamily && ((bindingFlag | BindingFlags.NonPublic) != 0))
                return true;
            if (!fieldInfo.IsStatic && ((bindingFlag | BindingFlags.Instance) != 0))
                return true;
          
        }
        internal object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return Type.GetTypeInfo().GetCustomAttributes(attributeType, inherit).ToArray();
        }

        internal Type[] GetInterfaces()
        {
            return Type.GetTypeInfo().ImplementedInterfaces.ToArray();
        }

        public System.Type[] GetGenericArguments()
        {
            if(Type.GetTypeInfo().IsGenericTypeDefinition)
                return Type.GetTypeInfo().GenericTypeParameters;
            else
                return Type.GetTypeInfo().GenericTypeArguments;
        }

        internal bool IsAssignableFrom(Type seqType)
        {
            if (seqType == null)
                return false;
            return Type.GetTypeInfo().IsAssignableFrom(seqType.GetTypeInfo());
        }

        public bool IsInstanceOfType(object o)
        {
            return o != null && Type.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo());
        }

        internal bool IsSubclassOf(Type type)
        {
            return Type.GetTypeInfo().IsSubclassOf(type);
        }

        public ConstructorInfo[] GetConstructors()
        {
            return Type.GetTypeInfo().DeclaredConstructors.ToArray();
        }

        internal Type[] GetNestedTypes(BindingFlags bindingFlags)
        {
            return (from nestedType in Type.GetTypeInfo().DeclaredNestedTypes
                    where BindingFlagsMatchNetstedType(nestedType, bindingFlags)
                    select nestedType.AsType()).ToArray();

        }

        private bool BindingFlagsMatchNetstedType(System.Reflection.TypeInfo nestedType, BindingFlags bindingFlag)
        {
            if (nestedType.IsNestedPublic && ((bindingFlag | BindingFlags.Public) != 0))
                return true;
            if (!nestedType.IsNestedPublic && ((bindingFlag | BindingFlags.NonPublic) != 0))
                return true;

            return false;
        }

        internal Type GetInterface(string name)
        {
            return (from type in Type.GetTypeInfo().ImplementedInterfaces
             where type.Name == name
             select type).FirstOrDefault();
        }

        public EventInfo GetEvent(string name)
        {
            return Type.GetRuntimeEvent(name);
            
        }
    }


 
}

namespace System.PCL.Reflection
{


#if PORTABLE
    // Summary:
    //     Specifies flags that control binding and the way in which the search for
    //     members and types is conducted by reflection.
    /// <MetaDataID>{e87b50e3-e8ee-4a26-b628-6aeafb47b1a2}</MetaDataID>
    public enum BindingFlags
    {
        // Summary:
        //     Specifies no binding flag.
        Default = 0,
        //
        // Summary:
        //     Specifies that the case of the member name should not be considered when
        //     binding.
        IgnoreCase = 1,
        //
        // Summary:
        //     Specifies that only members declared at the level of the supplied type's
        //     hierarchy should be considered. Inherited members are not considered.
        DeclaredOnly = 2,
        //
        // Summary:
        //     Specifies that instance members are to be included in the search.
        Instance = 4,
        //
        // Summary:
        //     Specifies that static members are to be included in the search.
        Static = 8,
        //
        // Summary:
        //     Specifies that public members are to be included in the search.
        Public = 16,
        //
        // Summary:
        //     Specifies that non-public members are to be included in the search.
        NonPublic = 32,
        //
        // Summary:
        //     Specifies that public and protected static members up the hierarchy should
        //     be returned. Private static members in inherited classes are not returned.
        //     Static members include fields, methods, events, and properties. Nested types
        //     are not returned.
        FlattenHierarchy = 64,
        //
        // Summary:
        //     Specifies that a method is to be invoked. This must not be a constructor
        //     or a type initializer.
        InvokeMethod = 256,
        //
        // Summary:
        //     Specifies that Reflection should create an instance of the specified type.
        //     Calls the constructor that matches the given arguments. The supplied member
        //     name is ignored. If the type of lookup is not specified, (Instance | Public)
        //     will apply. It is not possible to call a type initializer.
        CreateInstance = 512,
        //
        // Summary:
        //     Specifies that the value of the specified field should be returned.
        GetField = 1024,
        //
        // Summary:
        //     Specifies that the value of the specified field should be set.
        SetField = 2048,
        //
        // Summary:
        //     Specifies that the value of the specified property should be returned.
        GetProperty = 4096,
        //
        // Summary:
        //     Specifies that the value of the specified property should be set. For COM
        //     properties, specifying this binding flag is equivalent to specifying PutDispProperty
        //     and PutRefDispProperty.
        SetProperty = 8192,
        //
        // Summary:
        //     Specifies that the PROPPUT member on a COM object should be invoked. PROPPUT
        //     specifies a property-setting function that uses a value. Use PutDispProperty
        //     if a property has both PROPPUT and PROPPUTREF and you need to distinguish
        //     which one is called.
        PutDispProperty = 16384,
        //
        // Summary:
        //     Specifies that the PROPPUTREF member on a COM object should be invoked. PROPPUTREF
        //     specifies a property-setting function that uses a reference instead of a
        //     value. Use PutRefDispProperty if a property has both PROPPUT and PROPPUTREF
        //     and you need to distinguish which one is called.
        PutRefDispProperty = 32768,
        //
        // Summary:
        //     Specifies that types of the supplied arguments must exactly match the types
        //     of the corresponding formal parameters. Reflection throws an exception if
        //     the caller supplies a non-null Binder object, since that implies that the
        //     caller is supplying BindToXXX implementations that will pick the appropriate
        //     method.
        ExactBinding = 65536,
        //
        // Summary:
        //     Not implemented.
        SuppressChangeType = 131072,
        //
        // Summary:
        //     Returns the set of members whose parameter count matches the number of supplied
        //     arguments. This binding flag is used for methods with parameters that have
        //     default values and methods with variable arguments (varargs). This flag should
        //     only be used with System.Type.InvokeMember(System.String,BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[]).
        OptionalParamBinding = 262144,
        //
        // Summary:
        //     Used in COM interop to specify that the return value of the member can be
        //     ignored.
        IgnoreReturn = 16777216,
    }

#endif

}



