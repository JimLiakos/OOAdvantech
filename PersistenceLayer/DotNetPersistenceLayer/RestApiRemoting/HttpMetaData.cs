using System;
using System.Collections.Generic;
using System.Linq;

namespace OOAdvantech.MetaDataRepository
{

    /// <MetaDataID>{cabc24a9-dc8d-450d-9d95-a61d6926e2e2}</MetaDataID>
    public static class HttpMetaOperators
    {
        /// <MetaDataID>{b99fdda4-10d9-46c5-8a57-eaeac3bdf7d4}</MetaDataID>
        public static bool IsHttpVisible(this Classifier classifier, Feature feature)
        {


            bool classifierIsHttpVisible = false;
            var type = classifier.GetExtensionMetaObject<System.Type>();
            var classifierAttributes = type.GetMetaData().GetCustomAttributes(typeof(HttpVisible), false);
            classifierIsHttpVisible = classifierAttributes.Length > 0;
            if (feature is Operation && feature.Owner == classifier)
            {
                var methodInfo = (feature as Operation).GetExtensionMetaObject<System.Reflection.MethodInfo>();
                if (methodInfo == null)
                    return false;
                if (classifierIsHttpVisible)
                {
                    var methodAttributes = methodInfo.GetCustomAttributes(typeof(HttpInVisible), false);
                    if (methodAttributes.Length > 0)
                        return false;
                    else
                        return true;
                }
                else
                {
                    var methodAttributes = methodInfo.GetCustomAttributes(typeof(HttpVisible), false);
                    if (methodAttributes.Length > 0)
                        return true;
                    else
                        return false;
                }
            }

            if (classifier is Class && feature is Operation && feature.Owner != classifier)
            {
                var operation = feature as Operation;
                var method = (from _method in classifier.Features.OfType<Method>()
                              where _method.Specification == operation
                              select _method).FirstOrDefault();
                if (method != null)
                {
                    var methodInfo = method.GetExtensionMetaObject<System.Reflection.MethodInfo>();
                    if (classifierIsHttpVisible)
                    {
                        var methodAttributes = methodInfo.GetCustomAttributes(typeof(HttpInVisible), false);
                        if (methodAttributes.Length > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        var methodAttributes = methodInfo.GetCustomAttributes(typeof(HttpVisible), false);
                        if (methodAttributes.Length > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }


            if (feature is Attribute && feature.Owner == classifier)
            {
                System.Reflection.MemberInfo memberInfo = (feature as Attribute).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    memberInfo = (feature as Attribute).GetExtensionMetaObject<System.Reflection.PropertyInfo>();

                if (memberInfo == null)
                    return false;
                if (classifierIsHttpVisible)
                {
                    var memberAttributes = memberInfo.GetCustomAttributes(typeof(HttpInVisible), false);
                    if (memberAttributes.Length > 0)
                        return false;
                    else
                        return true;
                }
                else
                {
                    var memberAttributes = memberInfo.GetCustomAttributes(typeof(HttpVisible), false);
                    if (memberAttributes.Length > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }


        /// <MetaDataID>{80daba3c-8b34-4777-928d-d4cc295763ca}</MetaDataID>
        public static bool IsHttpVisible(this Classifier classifier, System.Reflection.EventInfo eventInfo)
        {

            bool classifierIsHttpVisible = false;
            var type = classifier.GetExtensionMetaObject<System.Type>();
            var classifierAttributes = type.GetMetaData().GetCustomAttributes(typeof(HttpVisible), false);
            classifierIsHttpVisible = classifierAttributes.Length > 0;

            if (eventInfo == null)
                return false;
            if (classifierIsHttpVisible)
            {
                var methodAttributes = eventInfo.GetCustomAttributes(typeof(HttpInVisible), false);
                if (methodAttributes.Length > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                var methodAttributes = eventInfo.GetCustomAttributes(typeof(HttpVisible), false);
                if (methodAttributes.Length > 0)
                    return true;
                else
                    return false;
            }


            //return false;
        }

        /// <MetaDataID>{cfb07d94-0973-465c-98e8-e014b2f4e081}</MetaDataID>
        public static bool IsHttpCachedMember(this Classifier classifier, AssociationEnd associationEnd)
        {
            if (associationEnd.Namespace == classifier)
            {

                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingDataOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingDataOnClientSide), false);
                if (memberAttributes.Length > 0)
                    return true;
                else
                    return false;

                //System.Reflection.MemberInfo memberInfo = (feature).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                //if (memberInfo == null)
                //    memberInfo = (feature).GetExtensionMetaObject<System.Reflection.PropertyInfo>();

                //if (memberInfo == null)
                //    return false;


                //var memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingDataOnClientSide), false);
                //if (memberAttributes.Length > 0)
                //    return true;
                //else
                //    return false;

            }
            return false;
        }
        /// <MetaDataID>{3050eeba-8f94-467d-940b-f3e3a577b219}</MetaDataID>
        public static bool IsHttpCachedMember(this Classifier classifier, Feature feature)
        {
            if (feature.Owner == classifier)
            {
                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (feature).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingDataOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (feature).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingDataOnClientSide), false);
                if (memberAttributes.Length > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }
    }

    /// <MetaDataID>{7381a010-2e92-4df3-82d1-eb13e3acbca2}</MetaDataID>
    public class ProxyType
    {

        /// <MetaDataID>{34b7bbeb-259f-4c26-bf4d-e12ab8480c7c}</MetaDataID>
        public bool Paired;

        /// <MetaDataID>{5f36baf1-46af-4fd4-8dd2-fe6f13753859}</MetaDataID>
        public string ObjectChangeStateEventName { get; set; }

        /// <MetaDataID>{f4c76c55-8762-4542-ba70-fe822cb8682f}</MetaDataID>
        string _Name;
        /// <MetaDataID>{9f8fe128-c2ee-44e2-85b8-bd8bcd7aeac0}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }

        }
        /// <MetaDataID>{23d4809b-615c-45bd-bb8b-bac5052ce77d}</MetaDataID>
        string _FullName;
        /// <MetaDataID>{8efb710e-dcdd-4369-b2c4-cf66ae5e8f4d}</MetaDataID>
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
            }
        }

        /// <MetaDataID>{ccf8067e-7dc8-4078-bbbb-df645ef09d20}</MetaDataID>
        public void CachingObjectMembersValue(object _object, Dictionary<string, object> membersValues)
        {
            foreach (var attribute in CachingClientSideAttributeProperties)
            {
                object value = attribute.GetValue(_object);
                membersValues[attribute.Name] = value;
            }

            foreach (var attributeRealization in CachingClientSideAttributeRealizationProperties)
            {
                object value = attributeRealization.GetValue(_object);
                membersValues[attributeRealization.Name] = value;
            }

            foreach (var associationEndRealization in CachingClientSideAssociationEndRealizationProperties)
            {
                object value = associationEndRealization.GetValue(_object);
                membersValues[associationEndRealization.Name] = value;
            }


            foreach (var associationEnd in CachingClientSideAssociationEndProperties)
            {
                object value = associationEnd.GetValue(_object);
                membersValues[associationEnd.Name] = value;
            }

            foreach (var _interface in _Interfaces)
                _interface.CachingObjectMembersValue(_object, membersValues);
        }


        /// <MetaDataID>{fa5f73d8-e4f2-487a-b00e-a8d5b3d23aea}</MetaDataID>
        List<Attribute> CachingClientSideAttributeProperties;

        /// <MetaDataID>{01e0dd81-9010-4ae9-bd76-77a8e7758459}</MetaDataID>
        List<AttributeRealization> CachingClientSideAttributeRealizationProperties;

        /// <MetaDataID>{9204d227-91eb-4ef7-b081-62a49b331bd3}</MetaDataID>
        List<AssociationEnd> CachingClientSideAssociationEndProperties;


        /// <MetaDataID>{36577234-561a-4d0b-a711-dbb8f771162c}</MetaDataID>
        List<AssociationEndRealization> CachingClientSideAssociationEndRealizationProperties;
        /// <MetaDataID>{0ebb8b54-9af1-41e6-9823-59eccb5bec71}</MetaDataID>
        public ProxyType(Type type)
        {
            var classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);

            _FullName = type.FullName;

            AssemblyQualifiedName = type.AssemblyQualifiedName;

            _Name = type.Name;
            int i = 0;
            if(_Name== "IServicesContextPresentation")
            {
                i = _Name.Length;
            }


            _Methods = (from method in classifier.GetFeatures(false).OfType<Operation>()
                        where method.Visibility == VisibilityKind.AccessPublic &&
                        classifier.IsHttpVisible(method)
                        select method.Name).Distinct().ToList();

            _Properties = (from method in classifier.GetFeatures(false).OfType<Attribute>()
                           where method.Visibility == VisibilityKind.AccessPublic &&
                           classifier.IsHttpVisible(method)
                           select method.Name).Distinct().ToList();

            _Events = (from _event in classifier.GetExtensionMetaObject<Type>().GetMetaData().GetEvents()
                       where classifier.IsHttpVisible(_event)
                       select _event.Name).Distinct().ToList();

            this.ObjectChangeStateEventName = (from _event in classifier.GetExtensionMetaObject<Type>().GetMetaData().GetEvents()
                                               where classifier.IsHttpVisible(_event) && _event.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle)
                                               select _event.Name).FirstOrDefault();


            CachingClientSideAttributeProperties = (from attribute in classifier.GetFeatures(false).OfType<Attribute>()
                                                    where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                    classifier.IsHttpCachedMember(attribute)
                                                    select attribute).Distinct().ToList();

            CachingClientSideAttributeRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AttributeRealization>()
                                                               where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                               classifier.IsHttpCachedMember(attribute)
                                                               select attribute).Distinct().ToList();

            CachingClientSideAssociationEndRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AssociationEndRealization>()
                                                                    where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                                    classifier.IsHttpCachedMember(attribute)
                                                                    select attribute).Distinct().ToList();


            CachingClientSideAssociationEndProperties = (from associationEnd in classifier.GetAssociateRoles(false)
                                                         where associationEnd.Visibility == VisibilityKind.AccessPublic &&
                                                         classifier.IsHttpCachedMember(associationEnd)
                                                         select associationEnd).Distinct().ToList();



            _CachingClientSidePropertiesNames = (from attribute in CachingClientSideAttributeProperties select attribute.Name).ToList();


            if (type.BaseType != null && type.BaseType != typeof(object) && type.BaseType != typeof(MarshalByRefObject))
                this.BaseProxyType = new ProxyType(type.BaseType);

            if (classifier is Class)
                _Interfaces = (from _interface in (classifier as Class).GetAllInterfaces()
                               select new ProxyType(_interface.GetExtensionMetaObject<System.Type>())).ToList();
            else
                _Interfaces = new List<ProxyType>();

        }

        /// <MetaDataID>{a90cd051-6e30-4dc8-81ce-c84543f71ba6}</MetaDataID>
        List<ProxyType> _Interfaces;
        /// <MetaDataID>{224805e9-927a-47d0-a71c-8b425942ccf4}</MetaDataID>
        public List<ProxyType> Interfaces
        {
            get
            {
                return _Interfaces;
            }
            set
            {
                _Interfaces = value;
            }

        }

        /// <MetaDataID>{84815faa-38d7-473f-8ed7-cf24f68be465}</MetaDataID>
        ProxyType BaseProxyType { get; set; }

        /// <MetaDataID>{cf2fbbb2-7361-41e1-b225-e078b0a80de4}</MetaDataID>
        public ProxyType()
        {

        }

        /// <exclude>Excluded</exclude>
        List<string> _Methods;
        /// <MetaDataID>{2820a84e-277a-4a3f-85da-ec8dbe15171b}</MetaDataID>
        public List<string> Methods
        {
            get
            {
                return _Methods;
            }
            set
            {
                _Methods = value;
            }
        }


        /// <MetaDataID>{629bcfed-ec7e-40f1-b947-3af07d9fc6d3}</MetaDataID>
        List<string> _CachingClientSidePropertiesNames;
        /// <MetaDataID>{a75edeaf-67de-4f0c-be8a-570866372718}</MetaDataID>
        public List<string> CachingClientSidePropertiesNames
        {
            get
            {
                return _CachingClientSidePropertiesNames;
            }
            set
            {
                _CachingClientSidePropertiesNames = value;
            }
        }

        /// <exclude>Excluded</exclude>
        List<string> _Properties;
        /// <MetaDataID>{c7410594-cb5c-4a58-904b-40c455fe984e}</MetaDataID>
        public List<string> Properties
        {
            get
            {
                return _Properties;
            }
            set
            {
                _Properties = value;
            }
        }

        /// <exclude>Excluded</exclude>
        List<string> _Events;
        /// <MetaDataID>{5367e5af-b708-4918-b72f-6ede53c23ac3}</MetaDataID>
        public List<string> Events
        {
            get
            {
                return _Events;
            }
            set
            {
                _Events = value;
            }
        }

        /// <MetaDataID>{2d4f9ce8-dfa1-4a63-aaba-a0a40ed4a03f}</MetaDataID>
        string _AssemblyQualifiedName;
        /// <MetaDataID>{1c70c4b8-f87b-4de7-a297-bfed5d513172}</MetaDataID>
        public string AssemblyQualifiedName
        {
            get
            {
                return _AssemblyQualifiedName;
            }
            set
            {
                _AssemblyQualifiedName = value;
            }
        }

        /// <MetaDataID>{3214f3f7-779a-4d2c-ba54-c1578ff71e1a}</MetaDataID>
        internal bool CanCastTo(Type fromType)
        {
            if (this.AssemblyQualifiedName == fromType.AssemblyQualifiedName)
                return true;

            foreach (var _interface in Interfaces)
            {
                if (_interface.CanCastTo(fromType))
                    return true;
            }
            if (BaseProxyType != null)
                return BaseProxyType.CanCastTo(fromType);

            return false;

        }
    }
}
