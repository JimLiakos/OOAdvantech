using System;

using System.Collections.Generic;
using System.Text;

namespace OOAdvantech
{
    /// <MetaDataID>{aa2392ee-15e6-4e4f-a978-4bada2239663}</MetaDataID>
    public struct ObjectInstadiator
    {
        System.Reflection.ConstructorInfo ConstructorInfo;
        public ObjectInstadiator(System.Reflection.ConstructorInfo constructorInfo)
        {
            ConstructorInfo = constructorInfo;
        }
        object InstantiateObjectHandlerImplementor()
        {
            throw new NotImplementedException();
        }
    }
    /// <MetaDataID>{cf34eaa0-7001-44db-a638-b9eb396c0d61}</MetaDataID>
    public struct FieldPropertySetImplementor
    {
        System.Reflection.FieldInfo FieldInfo;
        System.Reflection.PropertyInfo PropertyInfo;
        public FieldPropertySetImplementor(System.Reflection.FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            PropertyInfo = null;
        }

        public FieldPropertySetImplementor(System.Reflection.PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            FieldInfo = null;
        }
        public object SetHandlerImplementor(object source, object value)
        {
            if (FieldInfo != null)
            {
                if (source is IObjectState)
                {
                    ObjectMemberGetSet result = (source as IObjectState).SetMemberValue("", FieldInfo,value);
                    if (result != ObjectMemberGetSet.MemberValueSetted)
                        FieldInfo.SetValue(source, value);
                }
                else
                    FieldInfo.SetValue(source, value);


            }
            else
                PropertyInfo.SetValue(source, value, null);
            return source;
        }
    }
    /// <MetaDataID>{d04c5787-4d0c-4eac-958b-50790f52aa6b}</MetaDataID>
    public struct FieldPropertyGetImplementor
    {
        System.Reflection.FieldInfo FieldInfo;
        System.Reflection.PropertyInfo PropertyInfo;
        public FieldPropertyGetImplementor(System.Reflection.FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            PropertyInfo = null;
        }

        public FieldPropertyGetImplementor(System.Reflection.PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            FieldInfo = null;
        }


        public object GetHandlerImplementor(object source)
        {

            if (FieldInfo != null)
            {
                if(source is IObjectState)
                {
                    object value = (source as IObjectState).GetMemberValue("", FieldInfo);
                    if (value is ObjectMemberGetSet)
                        return FieldInfo.GetValue(source);
                    else
                        return value;
                }
                return FieldInfo.GetValue(source);
            }
            return PropertyInfo.GetValue(source, null);
        }
    }
    /// <MetaDataID>{5cd0e585-e226-4ae0-b54b-ae7978c3dfb9}</MetaDataID>
    public struct MethodInvokeImplementor
    {
        System.Reflection.MethodInfo MemberInfo;
        public MethodInvokeImplementor(System.Reflection.MethodInfo memberInfo)
        {
            MemberInfo = memberInfo;
            ConstructorInfo = null;
        }
        System.Reflection.ConstructorInfo ConstructorInfo;
        public MethodInvokeImplementor(System.Reflection.ConstructorInfo constructorInfo)
        {
            MemberInfo = null;
            ConstructorInfo = constructorInfo;

        }

        public object FastInvokeHandlerImplementor(object target, object[] paramters)
        {
            if (ConstructorInfo != null)
                return ConstructorInfo.Invoke(paramters);
            else
                return MemberInfo.Invoke(target, paramters); 

            //throw new NotImplementedException();
        }
    }

}



