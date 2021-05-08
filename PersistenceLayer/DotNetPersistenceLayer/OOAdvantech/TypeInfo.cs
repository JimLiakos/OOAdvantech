using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OOAdvantech
{
    /// <MetaDataID>{52cfdc3a-146d-4525-90a4-86fe357bfbbb}</MetaDataID>
    public class TypeInfo
    {
        private Type Type;

        public TypeInfo(Type type)
        {
            this.Type = type;
        }

        public bool IsValueType
        {
            get
            {
                return Type.IsValueType;
            }
        }

        internal System.Reflection.FieldInfo GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return Type.GetField(name, bindingAttr);
        }

        internal System.Reflection.FieldInfo GetField(string name)
        {
            return Type.GetField(name);
        }

        public bool IsGenericType
        {
            get
            {
                return Type.IsGenericType;
            }
        }

        public Type BaseType
        {
            get
            {
                return Type.BaseType;
            }
        }

        public System.Type[] GetGenericArguments()
        {
            return Type.GetGenericArguments();
        }

        internal System.Type[] GetInterfaces()
        {
            return Type.GetInterfaces();
        }

        internal bool IsAssignableFrom(System.Type seqType)
        {

            return Type.IsAssignableFrom(seqType);
        }

        public bool IsAbstract
        {
            get
            {
                return Type.IsAbstract;
            }
        }

        public bool IsClass
        {
            get
            {
                return Type.IsClass;
            }
        }
        public bool IsInterface
        {
            get
            {
                return Type.IsInterface;
            }
        }



        internal System.Reflection.FieldInfo[] GetFields(System.Reflection.BindingFlags bindingAttr)
        {

            return Type.GetFields(bindingAttr);
        }

        internal System.Reflection.PropertyInfo GetProperty(string name, System.Reflection.BindingFlags bindingAtt)
        {
            return Type.GetProperty(name, bindingAtt);
        }
        internal System.Reflection.MethodInfo GetMethod(string methodName)
        {
            return Type.GetMethod(methodName);
        }

        internal System.Reflection.ConstructorInfo GetConstructor(System.Reflection.BindingFlags bindingAttr, Type[] parameterTypes)
        {
            return Type.GetConstructor(bindingAttr, null, parameterTypes, null);
        }

        internal System.Reflection.PropertyInfo GetProperty(string name)
        {
            return Type.GetProperty(name);
        }

        internal object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return Type.GetCustomAttributes(attributeType, inherit);
        }

        public bool IsInstanceOfType(object _object)
        {
            return Type.IsInstanceOfType(_object);
        }

        internal System.Reflection.MethodInfo GetMethod(string methodName, System.Reflection.BindingFlags bindingFlags)
        {
            return Type.GetMethod(methodName, bindingFlags);
        }
        internal System.Reflection.MethodInfo[] GetMethods(System.Reflection.BindingFlags bindingFlags)
        {
            return Type.GetMethods(bindingFlags);
        }

        internal System.Reflection.MethodInfo[] GetMethods()
        {
            return Type.GetMethods();
        }

        internal System.Reflection.MemberInfo[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return Type.GetMember(name, bindingAttr);
        }

        //internal object GetMember(string memberName, int p)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Reflection.Assembly Assembly
        {
            get
            {
                return Type.Assembly;
            }
        }

        internal System.Reflection.MethodInfo GetMethod(string methodName, System.Type[] parameterTypes)
        {
            return Type.GetMethod(methodName, parameterTypes);
        }

        internal bool IsSubclassOf(System.Type type)
        {
            return Type.IsSubclassOf(type);
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                return Type.IsGenericTypeDefinition;
            }
        }

        internal System.Reflection.ConstructorInfo[] GetConstructors()
        {
            return Type.GetConstructors();
        }



        internal System.Type[] GetNestedTypes(System.Reflection.BindingFlags bindingFlag)
        {
            return Type.GetNestedTypes();
        }

        internal System.Reflection.PropertyInfo[] GetProperties(System.Reflection.BindingFlags bindingFlag)
        {
            return Type.GetProperties(bindingFlag);
        }



        public bool IsEnum
        {
            get
            {
                return Type.IsEnum;
            }
        }

        public bool IsNestedPrivate
        {
            get
            {
                return Type.IsNestedPrivate;
            }
        }
        public bool IsNestedPublic
        {
            get
            {
                return Type.IsNestedPublic;
            }
        }
        public bool IsNestedFamily
        {
            get
            {
                return Type.IsNestedFamily;
            }
        }

        public bool IsNestedAssembly
        {
            get
            {
                return Type.IsNestedAssembly;
            }
        }


        public bool IsPublic
        {
            get
            {
                return Type.IsPublic;
            }
        }

        public bool IsNotPublic
        {
            get
            {
                return Type.IsNotPublic;
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return Type.IsPrimitive;
            }
        }

        public bool ContainsGenericParameters
        {
            get
            {
                return Type.ContainsGenericParameters;
            }
        }

        internal object GetInterface(string interfaceName)
        {
            return Type.GetInterface(interfaceName);
        }

        internal System.Reflection.PropertyInfo[] GetProperties()
        {
            return Type.GetProperties();
        }

        internal EventInfo GetEvent(string name)
        {
            return Type.GetEvent(name);
        }

        internal EventInfo[] GetEvents()
        {
            return Type.GetEvents();
        }
    }

}
