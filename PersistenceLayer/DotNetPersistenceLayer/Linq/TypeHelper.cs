

using System.Collections.Generic;
//using System.Reflection;
using OOAdvantech;
namespace OOAdvantech.Linq
{
    /// <MetaDataID>{cf52b825-33bf-453c-a7f6-8a5f5c90eb48}</MetaDataID>
    internal class TypeHelper
    {
        /// <MetaDataID>{ab132d6f-a573-49ae-8641-893be7da5ce5}</MetaDataID>
        internal static bool IsNullableType(System.Type type)
        {
            
            return (((type != null) && type.GetMetaData().IsGenericType) && (type.GetGenericTypeDefinition() == typeof(System.Nullable<>)));
        }

        /// <MetaDataID>{f1fc7682-b45a-4d5e-a8be-98b6d799d926}</MetaDataID>
        internal static System.Type GetElementType(System.Type seqType)
        {
            System.Type type = FindIEnumerable(seqType);
            if (type == null)
            {
                return seqType;
            }
            return type.GetMetaData().GetGenericArguments()[0];
        }

        /// <MetaDataID>{47d2ecba-28f1-4892-8c53-42586130c343}</MetaDataID>
        internal static System.Type FindIEnumerable(System.Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(new[] { seqType.GetElementType() });
                }
                if (seqType.GetMetaData().IsGenericType)
                {
                    foreach (System.Type type in seqType.GetMetaData().GetGenericArguments())
                    {
                        System.Type type2 = typeof(IEnumerable<>).MakeGenericType(new[] { type });
                        if (type2.GetMetaData().IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                System.Type[] interfaces = seqType.GetMetaData().GetInterfaces();
                if ((interfaces != null) && (interfaces.Length > 0))
                {
                    foreach (System.Type type3 in interfaces)
                    {
                        System.Type type4 = FindIEnumerable(type3);
                        if (type4 != null)
                        {
                            return type4;
                        }
                    }
                }
                if ((seqType.GetMetaData().BaseType != null) && (seqType.GetMetaData().BaseType != typeof(object)))
                {
                    return FindIEnumerable(seqType.GetMetaData().BaseType);
                }
            }
            return null;
        }

        /// <summary>
        /// What is the type of the current member?
        /// </summary>
        /// <MetaDataID>{91e1d8af-61b8-4058-b6dd-e8177bb44e12}</MetaDataID>
        internal static System.Type GetMemberType(System.Reflection.MemberInfo mi)
        {
            System.Reflection.FieldInfo info = mi as System.Reflection.FieldInfo;
            if (info != null)
                return info.FieldType;

            System.Reflection.PropertyInfo info2 = mi as System.Reflection.PropertyInfo;
            if (info2 != null)
                return info2.PropertyType;

            System.Reflection.EventInfo info3 = mi as System.Reflection.EventInfo;
            if (info3 != null)
                return info3.EventHandlerType;

            return null;
        }

        /// <summary>
        /// If mi is a Property, then its sets it value to value
        /// If mi is a Field, it assigns value to it
        /// </summary>
        /// <MetaDataID>{fc453165-e77a-4abd-a1af-c65bdf04d5b6}</MetaDataID>
        internal static void SetMemberValue(
          object instance, System.Reflection.MemberInfo mi, object value)
        {
            System.Reflection.FieldInfo info = mi as System.Reflection.FieldInfo;
            if (info != null)
            {
#if DeviceDotNet
                info.SetValue(instance, value);
#else
                info.SetValue(instance, value, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, null);
#endif

                return;
            }

            System.Reflection.PropertyInfo info2 = mi as System.Reflection.PropertyInfo;
            if (info2 != null)
            {
#if DeviceDotNet
                info2.SetValue(instance, value);
#else
                info2.SetValue(instance, value, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, null, null);
#endif
                return;
            }

            throw new System.NotSupportedException("The member type is not supported!");
        }

        /// <summary>
        /// If mi is a Property, then it returns its value
        /// If mi is a Field, then it returns its value
        /// </summary>
        /// <MetaDataID>{11c986b9-2e1a-41da-9445-188f9f64188f}</MetaDataID>
        internal static object GetMemberValue(object instance, System.Reflection.MemberInfo mi)
        {
            System.Reflection.FieldInfo info = mi as System.Reflection.FieldInfo;
            if (info != null)
            {
                return info.GetValue(instance);
            }

            System.Reflection.PropertyInfo info2 = mi as System.Reflection.PropertyInfo;
            if (info2 != null)
            {
#if DeviceDotNet
                return info2.GetValue(instance);
#else
                return info2.GetValue(instance, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, null, null);return info2.GetValue(instance, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, null, null);
#endif

            }

            throw new System.NotSupportedException("The member type is not supported!");
        }


        /// <MetaDataID>{56379edb-0d60-41e2-811e-c701333a41ae}</MetaDataID>
        internal static MetaDataRepository.MetaObject GetClassifierMember(MetaDataRepository.Classifier declaringClassifier, System.Reflection.MemberInfo netNativeMember)
        {
            MetaDataRepository.MetaObject classifierMember = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierMember(declaringClassifier, netNativeMember);
            if (classifierMember is MetaDataRepository.Attribute &&
                declaringClassifier.LinkAssociation != null &&
                classifierMember.GetPropertyValue<bool>("MetaData", "AssociationClassRole"))
            {
                if (classifierMember.GetPropertyValue<bool>("MetaData", "IsRoleA"))
                    classifierMember = declaringClassifier.LinkAssociation.RoleA;
                else
                    classifierMember = declaringClassifier.LinkAssociation.RoleB;

            }
            if (classifierMember is MetaDataRepository.AssociationEndRealization)
                classifierMember = (classifierMember as MetaDataRepository.AssociationEndRealization).Specification;

            if (classifierMember is MetaDataRepository.AttributeRealization)
                classifierMember = (classifierMember as MetaDataRepository.AttributeRealization).Specification;
            return classifierMember;
        }


        /// <MetaDataID>{e9b1b42a-122c-49ab-9339-01ecb0b4a1de}</MetaDataID>
        internal static bool IsMemberGroupingKey(System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo.Name == "Key" && memberInfo.DeclaringType.GetMetaData().IsGenericType && memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>);
        }

        /// <MetaDataID>{2241f6d7-fde0-4a37-be22-ee089ff2efec}</MetaDataID>
        internal static bool IsMemberGroupingItem(System.Reflection.MemberInfo memberInfo, System.Type IGroupingType)
        {
            return IGroupingType.GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>) && IGroupingType.GetMetaData().GetGenericArguments()[1] == memberInfo.DeclaringType;
        }
    }
}