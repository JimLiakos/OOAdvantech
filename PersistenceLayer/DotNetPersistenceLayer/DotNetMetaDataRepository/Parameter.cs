using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{1e49ba02-13f5-4985-8d20-a9d9981a23f4}</MetaDataID>
    public class Parameter:MetaDataRepository.Parameter
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(WrParameter))
            {
                if (value == null)
                    WrParameter = default(System.Reflection.ParameterInfo);
                else
                    WrParameter = (System.Reflection.ParameterInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(WrParameter))
                return WrParameter;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{873ef119-7d34-453f-ba6a-ee5e132ccf68}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{969ec130-a73b-4a40-9ab9-9b1fbb472ce1}</MetaDataID>
        System.Reflection.ParameterInfo WrParameter;
        /// <MetaDataID>{4ad1a9a1-800d-4a95-a1db-e01c45c22d95}</MetaDataID>
        Parameter()
        {
        }
        /// <MetaDataID>{58c1d950-ab5a-4a07-9adf-34fdde757599}</MetaDataID>
        public Parameter(System.Reflection.ParameterInfo wrParameter)
        {
            WrParameter = wrParameter;
            _Name = wrParameter.Name;

        }
        private System.Collections.Generic.List<object> ExtensionMetaObjects;

        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.Generic.List<object>(1);
                    ExtensionMetaObjects.Add(WrParameter);
                }
                return ExtensionMetaObjects.ToList();
            }
        }
        /// <MetaDataID>{64d23e81-26fe-4996-8de9-1772e142b3c0}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier Type
        {

            get
            { 
                if (_Type == null)
                {
                    if (WrParameter.ParameterType.IsGenericParameter)
                        return null;
                    System.Type parameterType = WrParameter.ParameterType;
                    
                    _Type = DotNetMetaDataRepository.Type.GetClassifierObject(parameterType);
                }
                return _Type;
            }

            set
            {
                // base.Type = value;
            }
        }

        /// <MetaDataID>{5a6c2ae1-bd7b-4b7f-9559-4950e7f9bd10}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.TemplateParameter ParameterizedType
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    if (_ParameterizedType == null)
                    {
                        System.Type parameterType = WrParameter.ParameterType;

                        if (!parameterType.IsGenericParameter)
                            return null;
                        else
                        {
                            _ParameterizedType = MetaObjectMapper.FindMetaObjectFor(parameterType) as MetaDataRepository.TemplateParameter;
                            if (_ParameterizedType == null)
                                _ParameterizedType = new TemplateParameter(new Type(parameterType));
                        }
                    }
                    return _ParameterizedType;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            set
            {

            }
        } 
    }
}
