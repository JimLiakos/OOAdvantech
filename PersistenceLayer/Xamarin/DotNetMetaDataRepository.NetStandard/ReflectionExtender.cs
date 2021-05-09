using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OOAdvantech.DotNetMetaDataRepository
{
    public static class ReflectionExtender
    {
        /// <summary>
        ///  Gets the custom attributes for this assembly as specified by type.
        /// </summary>
        /// <param name="assembly">
        /// Extended object
        /// </param>
        /// <param name="attributeType">
        /// The System.Type for which the custom attributes are to be returned.
        /// </param>
        /// <param name="inherit">
        /// This argument is ignored for objects of type System.Reflection.Assembly.
        /// </param>
        /// <returns>
        ///  An array of type Object containing the custom attributes for this assembly
        ///  as specified by attributeType.
        /// </returns>

        public static object[] GetCustomAttributes(this System.Reflection.Assembly assembly,System.Type attributeType, bool inherit)
        {
            return new object[1] { assembly.GetCustomAttribute(attributeType) };
        }
        public static object[] GetCustomAttributes(this System.Reflection.Assembly assembly, bool inherit)
        {
            return assembly.CustomAttributes.ToArray();
        }

        //
        // Summary:
        //     Gets the System.Type object with the specified name in the assembly instance,
        //     with the options of ignoring the case, and of throwing an exception if the
        //     type is not found.
        //
        // Parameters:
        //   name:
        //     The full name of the type.
        //
        //   throwOnError:
        //     true to throw an exception if the type is not found; false to return null.
        //
        //   ignoreCase:
        //     true to ignore the case of the type name; otherwise, false.
        //
        // Returns:
        //     A System.Type object that represents the specified class.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     name is invalid.  -or- The length of name exceeds 1024 characters.
        //
        //   System.ArgumentNullException:
        //     name is null.
        //
        //   System.TypeLoadException:
        //     throwOnError is true, and the type cannot be found.
        //
        //   System.IO.FileNotFoundException:
        //     name requires a dependent assembly that could not be found.
        //
        //   System.IO.FileLoadException:
        //     name requires a dependent assembly that was found but could not be loaded.
        //      -or- The current assembly was loaded into the reflection-only context, and
        //     name requires a dependent assembly that was not preloaded.
        //
        //   System.BadImageFormatException:
        //     name requires a dependent assembly, but the file is not a valid assembly.
        //     -or- name requires a dependent assembly which was compiled for a version
        //     of the runtime later than the currently loaded version.
        public static System.Type GetType(this System.Reflection.Assembly assembly, string name, bool throwOnError, bool ignoreCase)
        {
            return assembly.GetType(name, throwOnError, ignoreCase);
        }
        //public static AssemblyName[] GetReferencedAssemblies(this System.Reflection.Assembly assembly)
        //{
        //    return assembly.GetReferencedAssemblies();
        //}

        public static System.Type[] GetTypes(this System.Reflection.Assembly assembly)
        {
            return (from type in assembly.DefinedTypes
                    select type.AsType()).ToArray();
            
        }

        public static object[] GetCustomAttributes(this System.Reflection.MemberInfo memberInfo, System.Type attributeType, bool inherit)
        {
            return System.Reflection.CustomAttributeExtensions.GetCustomAttributes(memberInfo, attributeType, inherit).OfType<object>().ToArray();
        }


   

        private static bool IsMethodsEqual(System.Reflection.MethodInfo FirstMethod, System.Reflection.MethodInfo SecondMethod)
        {

            if (FirstMethod.Name != SecondMethod.Name)
                return false;
            if (FirstMethod.ReturnType != SecondMethod.ReturnType)
                return false;
            System.Reflection.ParameterInfo[] FirstMethodParameters = FirstMethod.GetParameters();
            System.Reflection.ParameterInfo[] SecondMethodParameters = SecondMethod.GetParameters();
            if (FirstMethodParameters.Length != SecondMethodParameters.Length)
                return false;
            for (long i = 0; i < FirstMethodParameters.Length; i++)
            {
                if (FirstMethodParameters[i].ParameterType != SecondMethodParameters[i].ParameterType)
                    return false;
            }
            return true;
        }
        public static MethodInfo GetBaseDefinition(this System.Reflection.MethodInfo methodInfo)
        {
            MethodInfo baseMethodInfo = null;
            System.Type baseType = methodInfo.DeclaringType.GetTypeInfo().BaseType;
            while (baseType != null)
            {
                var baseTypeMethodInfo = (from method in baseType.GetTypeInfo().DeclaredMethods
                                          where IsMethodsEqual(method, methodInfo)
                                          select method).FirstOrDefault();
                if (baseTypeMethodInfo != null)
                    baseMethodInfo = baseTypeMethodInfo;
                baseType = baseType.GetTypeInfo().BaseType;
            }
            if (baseMethodInfo == null)
                return methodInfo;
            return baseMethodInfo;
        }

        public static MethodInfo[] GetAccessors(this System.Reflection.PropertyInfo propertyInfo)
        {
            return propertyInfo.GetAccessors(true);
        }

        public static MethodInfo[] GetAccessors(this System.Reflection.PropertyInfo propertyInfo,bool nonPublic)
        {
            if (propertyInfo.SetMethod != null && propertyInfo.GetMethod != null)
                return new MethodInfo[2] { propertyInfo.GetMethod, propertyInfo.SetMethod };

            if (propertyInfo.SetMethod != null)
                return new MethodInfo[1] { propertyInfo.SetMethod };

            if (propertyInfo.GetMethod != null)
                return new MethodInfo[1] { propertyInfo.GetMethod };

            return new MethodInfo[0];
        }

        //
        // Summary:
        //     When overridden in a derived class, returns the public or non-public get
        //     accessor for this property.
        //
        // Parameters:
        //   nonPublic:
        //     Indicates whether a non-public get accessor should be returned. true if a
        //     non-public accessor is to be returned; otherwise, false.
        //
        // Returns:
        //     A MethodInfo object representing the get accessor for this property, if nonPublic
        //     is true. Returns null if nonPublic is false and the get accessor is non-public,
        //     or if nonPublic is true but no get accessors exist.
        //
        // Exceptions:
        //   System.Security.SecurityException:
        //     The requested method is non-public and the caller does not have System.Security.Permissions.ReflectionPermission
        //     to reflect on this non-public method.
        public static MethodInfo GetGetMethod(this System.Reflection.PropertyInfo propertyInfo,bool nonPublic)
        {
            return propertyInfo.GetGetMethod(nonPublic);
        }


        //
        // Summary:
        //     When overridden in a derived class, returns the set accessor for this property.
        //
        // Parameters:
        //   nonPublic:
        //     Indicates whether the accessor should be returned if it is non-public. true
        //     if a non-public accessor is to be returned; otherwise, false.
        //
        // Returns:
        //     Value Condition A System.Reflection.MethodInfo object representing the Set
        //     method for this property. The set accessor is public.  -or- nonPublic is
        //     true and the set accessor is non-public. nullnonPublic is true, but the property
        //     is read-only.  -or- nonPublic is false and the set accessor is non-public.
        //      -or- There is no set accessor.
        //
        // Exceptions:
        //   System.Security.SecurityException:
        //     The requested method is non-public and the caller does not have System.Security.Permissions.ReflectionPermission
        //     to reflect on this non-public method.
        public static MethodInfo GetSetMethod(this System.Reflection.PropertyInfo propertyInfo,bool nonPublic)
        {
            return propertyInfo.GetGetMethod(nonPublic);
        }
    }
}
