
using OOAdvantech.Remoting.RestApi;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

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

        /// <MetaDataID>{89429b7a-88a5-452e-8f0c-e340bec51a8f}</MetaDataID>
        public static bool IsHttpVisible(this Classifier classifier, AssociationEnd associationEnd)
        {

            bool classifierIsHttpVisible = false;
            var type = classifier.GetExtensionMetaObject<System.Type>();
            var classifierAttributes = type.GetMetaData().GetCustomAttributes(typeof(HttpVisible), false);
            classifierIsHttpVisible = classifierAttributes.Length > 0;

            if (associationEnd == null)
                return false;


            System.Reflection.MemberInfo memberInfo = associationEnd.GetExtensionMetaObject<System.Reflection.FieldInfo>();
            if (memberInfo == null)
                memberInfo = associationEnd.GetExtensionMetaObject<System.Reflection.PropertyInfo>();

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

        //###

        /// <MetaDataID>{f04a37d5-589e-4c92-aefd-f389c7e1f7a3}</MetaDataID>
        public static bool IsOnDemandCachedMember(this Classifier classifier, AssociationEnd associationEnd)
        {
            if (associationEnd.Namespace == classifier)
            {

                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
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


        /// <MetaDataID>{f15e1451-860f-4096-9983-b8187565d13c}</MetaDataID>
        public static bool IsOnDemandCachedMember(this Classifier classifier, Feature feature)
        {
            if (feature.Owner == classifier)
            {
                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (feature).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (feature).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
                if (memberAttributes.Length > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }








        /// <MetaDataID>{5183e9e4-6a1c-43df-b1f7-b0128bb02969}</MetaDataID>
        public static bool IsReferencedCachedMember(this Classifier classifier, Feature feature)
        {
            if (feature.Owner == classifier)
            {
                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (feature).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (feature).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
                if (memberAttributes.Length > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }






        /// <MetaDataID>{40fd58f9-3810-417a-8872-290b8c68b411}</MetaDataID>
        public static bool IsReferencedCachedMember(this Classifier classifier, AssociationEnd associationEnd)
        {
            if (associationEnd.Namespace == classifier)
            {

                object[] memberAttributes = null;
                System.Reflection.MemberInfo memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.PropertyInfo>();
                if (memberInfo != null)
                {
                    memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
                    if (memberAttributes.Length > 0)
                        return true;
                }
                memberInfo = (associationEnd).GetExtensionMetaObject<System.Reflection.FieldInfo>();
                if (memberInfo == null)
                    return false;

                memberAttributes = memberInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
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




        /// <MetaDataID>{20598ab8-6a80-4a60-8c67-f62f37125bac}</MetaDataID>
        public static bool IsCachedMember(this FieldInfo fieldInfo)
        {
            var memberAttributes = fieldInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }
        /// <MetaDataID>{52c6d9df-e1bb-460d-85fb-26e1066e2a98}</MetaDataID>
        public static bool IsCachedMember(this PropertyInfo propertyInfo)
        {
            var memberAttributes = propertyInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }
        /// <MetaDataID>{8bcebbaa-a670-4325-8c76-348ffcc1b212}</MetaDataID>
        public static bool IsCachedOnDemandMember(this FieldInfo fieldInfo)
        {
            var memberAttributes = fieldInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }

        /// <MetaDataID>{72e05f13-596e-4049-b4d1-3fd4e49aea9b}</MetaDataID>
        public static bool IsCachedOnDemandMember(this PropertyInfo propertyInfo)
        {
            var memberAttributes = propertyInfo.GetCustomAttributes(typeof(OnDemandCachingDataOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }
        /// <MetaDataID>{bf6f28e7-9d7b-48d7-8774-8730d1e1eb40}</MetaDataID>
        public static bool IsCachedReferenceOnlyMember(this FieldInfo fieldInfo)
        {
            var memberAttributes = fieldInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }

        /// <MetaDataID>{52222978-ecc1-4cae-b3f3-391b9c0cbcc5}</MetaDataID>
        public static bool IsCachedReferenceOnlyMember(this PropertyInfo propertyInfo)
        {
            var memberAttributes = propertyInfo.GetCustomAttributes(typeof(CachingOnlyReferenceOnClientSide), false);
            if (memberAttributes.Length > 0)
                return true;
            return false;
        }
    }

    /// <MetaDataID>{7381a010-2e92-4df3-82d1-eb13e3acbca2}</MetaDataID>
    public class ProxyType
    {


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

        // List<string> CachingMembersNames
        //{
        //    get
        //    {

        //        List<string> cachingMembersNames = CachingClientSideAttributeProperties.Select(x => x.Name).
        //            Union(CachingClientSideAttributeRealizationProperties.Select(x => x.Name)).
        //            Union(CachingClientSideAssociationEndRealizationProperties.Select(x => x.Name)).
        //            Union(CachingClientSideAssociationEndProperties.Select(x => x.Name)).Distinct().ToList();



        //        if (this.BaseProxyType != null)
        //            cachingMembersNames = cachingMembersNames.Union(BaseProxyType.CachingMembersNames).ToList();

        //        if (Interfaces != null)
        //            foreach (var _interface in Interfaces)
        //                cachingMembersNames = cachingMembersNames.Union(_interface.CachingMembersNames).ToList();

        //        return cachingMembersNames;

        //    }
        //}

        /// <MetaDataID>{eaeebe61-fa1f-456b-9ea2-2dcaa510c7c2}</MetaDataID>
        [Json.JsonIgnore]
        internal List<string> ReferenceCachingMembersNames
        {
            get
            {



                List<string> cachingMembersNames = ReferenceCachingClientSideAttributeProperties.Select(x => x.Name).
                    Union(ReferenceCachingClientSideAttributeRealizationProperties.Select(x => x.Name)).
                    Union(ReferenceCachingClientSideAssociationEndRealizationProperties.Select(x => x.Name)).
                    Union(ReferenceCachingClientSideAssociationEndProperties.Select(x => x.Name)).Distinct().ToList();


                if (this.BaseProxyType != null)
                    cachingMembersNames = cachingMembersNames.Union(BaseProxyType.ReferenceCachingMembersNames).ToList();

                if (Interfaces != null)
                    foreach (var _interface in Interfaces)
                        cachingMembersNames = cachingMembersNames.Union(_interface.ReferenceCachingMembersNames).ToList();



                return cachingMembersNames;

            }
        }


        /// <MetaDataID>{d705db1a-4a51-4375-ae14-d5c6add72130}</MetaDataID>
        List<string> _OnDemandCachingMembersNames = new List<string>();

        /// <MetaDataID>{743bfbcf-25d4-43ee-9cff-312fdc187349}</MetaDataID>
        public List<string> OnDemandCachingMembersNames
        {
            get
            {

                if (Type != null)
                {
                    List<string> cachingMembersNames = OnDemandCachingClientSideAttributeProperties.Select(x => x.Name).
                        Union(OnDemandCachingClientSideAttributeRealizationProperties.Select(x => x.Name)).
                        Union(OnDemandCachingClientSideAssociationEndRealizationProperties.Select(x => x.Name)).
                        Union(OnDemandCachingClientSideAssociationEndProperties.Select(x => x.Name)).Distinct().ToList();

                    if (this.BaseProxyType != null)
                        cachingMembersNames = cachingMembersNames.Union(BaseProxyType.OnDemandCachingMembersNames).ToList();

                    if (Interfaces != null)
                        foreach (var _interface in Interfaces)
                            cachingMembersNames = cachingMembersNames.Union(_interface.OnDemandCachingMembersNames).ToList();

                    return cachingMembersNames;
                }
                else
                    return _OnDemandCachingMembersNames;

            }
            set
            {
                _OnDemandCachingMembersNames = value;
            }
        }


        /// <MetaDataID>{ccf8067e-7dc8-4078-bbbb-df645ef09d20}</MetaDataID>
        public void CachingObjectMembersValue(object _object, Dictionary<string, object> membersValues, bool referenceOnlyCaching, CachingMetaData cachingMetaData)
        {
            List<string> cachingClientSideMembers = new List<string>();

            if (!referenceOnlyCaching)
            {
                foreach (var attribute in CachingClientSideAttributeProperties)
                {
                    object value = attribute.GetValue(_object);
                    membersValues[attribute.Name] = value;
                }

                foreach (var attributeRealization in CachingClientSideAttributeRealizationProperties.Union(ReferenceCachingClientSideAttributeRealizationProperties))
                {
                    object value = attributeRealization.GetValue(_object);
                    membersValues[attributeRealization.Name] = value;
                }

                foreach (var associationEndRealization in CachingClientSideAssociationEndRealizationProperties.Union(ReferenceCachingClientSideAssociationEndRealizationProperties))
                {
                    object value = associationEndRealization.GetValue(_object);
                    membersValues[associationEndRealization.Name] = value;
                }


                foreach (var associationEnd in CachingClientSideAssociationEndProperties.Union(ReferenceCachingClientSideAssociationEndProperties))
                {
                    object value = associationEnd.GetValue(_object);
                    membersValues[associationEnd.Name] = value;
                }

                // client side update caching extra members values
                if (cachingMetaData?.CachingMembers?.TryGetValue("", out cachingClientSideMembers) == true)
                {
                    foreach (string memberName in cachingClientSideMembers)
                    {
                        if (!membersValues.ContainsKey(memberName))
                        {
                            var property = this.Type.GetProperty(memberName);
                            if (property != null)
                            {
                                membersValues[memberName] = property.GetValue(_object);
                            }
                            else
                            {
                                var field = this.Type.GetField(memberName);
                                if (field != null)
                                    membersValues[memberName] = field.GetValue(_object);
                            }
                        }

                    }

                }

            }

            //scoop caching data allowed in reference only request  
            if (cachingMetaData?.CachingMembers?.TryGetValue(FullName, out cachingClientSideMembers) == true)
            {

                Type type = this.Type;

                foreach (string memberName in cachingClientSideMembers)
                {
                    if (!membersValues.ContainsKey(memberName))
                    {
                        try
                        {
                            PropertyInfo property = null;
                            FieldInfo field = null;
                            property = type.GetProperty(memberName);
                            if (property == null)
                                field = this.Type.GetField(memberName);

                            if (field == null && property == null)
                            {
                                var classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);
                                var attribute = classifier.GetFeatures(true).OfType<Attribute>().Where(x => x.Name == memberName).FirstOrDefault();
                                if (attribute != null)
                                {
                                    property = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;
                                    if (property == null)
                                        field = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                }
                                else
                                {
                                    var associationEnd = classifier.GetAssociateRoles(true).OfType<AssociationEnd>().Where(x => x.Name == memberName).FirstOrDefault();
                                    if (associationEnd != null)
                                    {
                                        property = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;
                                        if (property == null)
                                            field = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                    }
                                }
                            }
                            if (property != null)
                                membersValues[memberName] = property.GetValue(_object);
                            else
                            {
                                if (field != null)
                                    membersValues[memberName] = field.GetValue(_object);
                            }
                        }
                        catch (Exception error)
                        {
                        }
                    }
                    else
                    {

                    }

                }
            }


            BaseProxyType?.CachingObjectMembersValue(_object, membersValues, referenceOnlyCaching, cachingMetaData);

            foreach (var _interface in _Interfaces)
                _interface.CachingObjectMembersValue(_object, membersValues, referenceOnlyCaching, cachingMetaData);
        }


        /// <MetaDataID>{fa5f73d8-e4f2-487a-b00e-a8d5b3d23aea}</MetaDataID>
        List<Attribute> CachingClientSideAttributeProperties;

        /// <MetaDataID>{01e0dd81-9010-4ae9-bd76-77a8e7758459}</MetaDataID>
        List<AttributeRealization> CachingClientSideAttributeRealizationProperties;

        /// <MetaDataID>{9204d227-91eb-4ef7-b081-62a49b331bd3}</MetaDataID>
        List<AssociationEnd> CachingClientSideAssociationEndProperties;

        /// <MetaDataID>{e5ac9d52-fa78-4c06-8253-c246bc21c030}</MetaDataID>
        List<Attribute> ReferenceCachingClientSideAttributeProperties;
        /// <MetaDataID>{38c99616-fcd6-4d24-899d-eb30b4f5b2b2}</MetaDataID>
        List<AttributeRealization> ReferenceCachingClientSideAttributeRealizationProperties;
        /// <MetaDataID>{7daf0149-5579-4089-8383-75324f0b1001}</MetaDataID>
        List<AssociationEndRealization> ReferenceCachingClientSideAssociationEndRealizationProperties;
        /// <MetaDataID>{4f213b44-2b22-4cd7-a1d8-b2032cdfd344}</MetaDataID>
        List<AssociationEnd> ReferenceCachingClientSideAssociationEndProperties;
        /// <MetaDataID>{49002bb3-d9a8-4600-a291-9dd676c36524}</MetaDataID>
        List<Attribute> OnDemandCachingClientSideAttributeProperties;
        /// <MetaDataID>{c56f868c-501d-4e68-97b9-588c6fa61973}</MetaDataID>
        List<AttributeRealization> OnDemandCachingClientSideAttributeRealizationProperties;
        /// <MetaDataID>{397740f9-ffb8-4ee2-a936-1ff9dc62aa15}</MetaDataID>
        List<AssociationEndRealization> OnDemandCachingClientSideAssociationEndRealizationProperties;
        /// <MetaDataID>{fde2e502-0af5-4441-80ee-7e4397369d5e}</MetaDataID>
        List<AssociationEnd> OnDemandCachingClientSideAssociationEndProperties;


        /// <MetaDataID>{36577234-561a-4d0b-a711-dbb8f771162c}</MetaDataID>
        List<AssociationEndRealization> CachingClientSideAssociationEndRealizationProperties;
        /// <MetaDataID>{0ebb8b54-9af1-41e6-9823-59eccb5bec71}</MetaDataID>
        ProxyType(Type type)
        {
            Type = type;
            var classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);

            _FullName = type.FullName;

            AssemblyQualifiedName = type.AssemblyQualifiedName;


            _Name = type.Name;
            int i = 0;
            if (_Name == "IServicesContextPresentation")
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

            _Properties.AddRange((from method in classifier.GetAssociateRoles(false).OfType<AssociationEnd>()
                                  where method.Visibility == VisibilityKind.AccessPublic &&
                                  classifier.IsHttpVisible(method)
                                  select method.Name).Distinct().ToList());

            _Events = (from _event in classifier.GetExtensionMetaObject<Type>().GetMetaData().GetEvents()
                       where classifier.IsHttpVisible(_event)
                       select _event.Name).Distinct().ToList();

            this.ObjectChangeStateEventName = (from _event in classifier.GetExtensionMetaObject<Type>().GetMetaData().GetEvents()
                                               where classifier.IsHttpVisible(_event) && _event.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle)
                                               select _event.Name).FirstOrDefault();

            ObjectChangeState = (from _event in classifier.GetExtensionMetaObject<Type>().GetMetaData().GetEvents()
                                 where _event.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle)
                                 select _event).FirstOrDefault();

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


            //###
            ReferenceCachingClientSideAttributeProperties = (from attribute in classifier.GetFeatures(false).OfType<Attribute>()
                                                             where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                             classifier.IsReferencedCachedMember(attribute)
                                                             select attribute).Distinct().ToList();

            ReferenceCachingClientSideAttributeRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AttributeRealization>()
                                                                        where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                                        classifier.IsReferencedCachedMember(attribute)
                                                                        select attribute).Distinct().ToList();

            ReferenceCachingClientSideAssociationEndRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AssociationEndRealization>()
                                                                             where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                                             classifier.IsReferencedCachedMember(attribute)
                                                                             select attribute).Distinct().ToList();


            ReferenceCachingClientSideAssociationEndProperties = (from associationEnd in classifier.GetAssociateRoles(false)
                                                                  where associationEnd.Visibility == VisibilityKind.AccessPublic &&
                                                                  classifier.IsReferencedCachedMember(associationEnd)
                                                                  select associationEnd).Distinct().ToList();


            //###
            OnDemandCachingClientSideAttributeProperties = (from attribute in classifier.GetFeatures(false).OfType<Attribute>()
                                                            where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                            classifier.IsOnDemandCachedMember(attribute)
                                                            select attribute).Distinct().ToList();

            OnDemandCachingClientSideAttributeRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AttributeRealization>()
                                                                       where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                                       classifier.IsOnDemandCachedMember(attribute)
                                                                       select attribute).Distinct().ToList();

            OnDemandCachingClientSideAssociationEndRealizationProperties = (from attribute in classifier.GetFeatures(false).OfType<AssociationEndRealization>()
                                                                            where attribute.Visibility == VisibilityKind.AccessPublic &&
                                                                            classifier.IsOnDemandCachedMember(attribute)
                                                                            select attribute).Distinct().ToList();


            OnDemandCachingClientSideAssociationEndProperties = (from associationEnd in classifier.GetAssociateRoles(false)
                                                                 where associationEnd.Visibility == VisibilityKind.AccessPublic &&
                                                                 classifier.IsOnDemandCachedMember(associationEnd)
                                                                 select associationEnd).Distinct().ToList();



            var cachingClientSidePropertiesNames = (from attribute in CachingClientSideAttributeProperties select attribute.Name).ToList();

            if (cachingClientSidePropertiesNames.Count > 0)
                HasCachingClientSideProperties = true;

            if (type.BaseType != null && type.BaseType != typeof(object) && type.BaseType != typeof(MarshalByRefObject))
                this.BaseProxyType = new ProxyType(type.BaseType);
            if (BaseProxyType != null && BaseProxyType.HasCachingClientSideProperties)
                HasCachingClientSideProperties = true;


            if (classifier is Class)
                _Interfaces = (from _interface in (classifier as Class).GetAllInterfaces()
                               select new ProxyType(_interface.GetExtensionMetaObject<System.Type>())).ToList();
            else
                _Interfaces = new List<ProxyType>();

            if (_Interfaces.Where(x => x.HasCachingClientSideProperties).FirstOrDefault() != null)
                HasCachingClientSideProperties = true;

        }


        /// <MetaDataID>{e820c37f-b24f-4c91-9a89-04348c496cb2}</MetaDataID>
        static Dictionary<Type, ProxyType> ProxyTypes = new Dictionary<Type, ProxyType>();
        /// <MetaDataID>{500ffd8c-5a53-4470-aec8-514ddbe88130}</MetaDataID>
        public static ProxyType GetProxyType(Type type)
        {
            lock (ProxyTypes)
            {
                if (!ProxyTypes.TryGetValue(type, out ProxyType proxyType))
                {
                    proxyType = new ProxyType(type);
                    ProxyTypes[type] = proxyType;
                }
                return proxyType;
            }
        }

        /// <MetaDataID>{7165299d-d854-4e44-82d2-e208e81c0fe2}</MetaDataID>
        System.Reflection.EventInfo ObjectChangeState;
        /// <MetaDataID>{c14cf403-5ccd-4692-9d58-3ed11e3e3598}</MetaDataID>
        internal System.Reflection.EventInfo GetObjectChangeState()
        {
            if (ObjectChangeState != null)
                return ObjectChangeState;
            System.Reflection.EventInfo objectChangeState = null;


            if (BaseProxyType != null)
                objectChangeState = BaseProxyType.GetObjectChangeState();

            if (objectChangeState != null)
                return objectChangeState;

            if (Interfaces != null)
                objectChangeState = Interfaces.Where(x => x.GetObjectChangeState() != null).Select(x => x.GetObjectChangeState()).FirstOrDefault();

            return objectChangeState;
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
            //  CachingClientSideAssociationEndProperties=new List<AssociationEnd>()
            ReferenceCachingClientSideAttributeProperties = new List<Attribute>();
            ReferenceCachingClientSideAttributeRealizationProperties = new List<AttributeRealization>();
            ReferenceCachingClientSideAssociationEndRealizationProperties = new List<AssociationEndRealization>();
            ReferenceCachingClientSideAssociationEndProperties = new List<AssociationEnd>();

            CachingClientSideAttributeProperties = new List<Attribute>();
            CachingClientSideAttributeRealizationProperties = new List<AttributeRealization>();
            CachingClientSideAssociationEndRealizationProperties = new List<AssociationEndRealization>();
            CachingClientSideAssociationEndProperties = new List<AssociationEnd>();

            OnDemandCachingClientSideAttributeProperties = new List<Attribute>();
            OnDemandCachingClientSideAttributeRealizationProperties = new List<AttributeRealization>();
            OnDemandCachingClientSideAssociationEndRealizationProperties = new List<AssociationEndRealization>();
            OnDemandCachingClientSideAssociationEndProperties = new List<AssociationEnd>();

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
        ///// <MetaDataID>{a75edeaf-67de-4f0c-be8a-570866372718}</MetaDataID>
        //public List<string> CachingClientSidePropertiesNames
        //{
        //    get
        //    {
        //        return _CachingClientSidePropertiesNames;
        //    }
        //    set
        //    {
        //        _CachingClientSidePropertiesNames = value;
        //    }
        //}

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
        /// <MetaDataID>{9019fce0-fb81-4a7b-8ee0-452aa3b899d1}</MetaDataID>
        private Type Type;

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

        /// <MetaDataID>{19138a51-3a18-4a6a-aad0-484022b7d401}</MetaDataID>
        public bool HasCachingClientSideProperties { get; internal set; }

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



    /// <MetaDataID>{89b4541c-9b5c-4fd4-8769-537fdfc1970c}</MetaDataID>
    public static class ProxyTypeExtension
    {
        /// <MetaDataID>{af600952-4383-4661-b093-6a6e03fadb30}</MetaDataID>
        static Dictionary<string, Type> NativeTypes = new Dictionary<string, Type>();
        /// <MetaDataID>{d2421b08-d3b9-4bed-9756-3feb2556e28d}</MetaDataID>
        public static Type GetNativeType(this ProxyType proxyType)
        {
            Type type = null;
            if (!string.IsNullOrWhiteSpace(proxyType.AssemblyQualifiedName) && NativeTypes.TryGetValue(proxyType.AssemblyQualifiedName, out type))
                return type;

            if (!string.IsNullOrWhiteSpace(proxyType.FullName) && NativeTypes.TryGetValue(proxyType.FullName, out type))
                return type;


            type = Type.GetType(proxyType.AssemblyQualifiedName);
            if (type == null)
            {
                if (!Remoting.RestApi.Serialization.SerializationBinder.NamesTypesDictionary.TryGetValue(proxyType.FullName, out type))
                    type = Type.GetType(proxyType.FullName);
            }
            if (type != null)
            {
                if (!string.IsNullOrWhiteSpace(proxyType.AssemblyQualifiedName))
                    NativeTypes[proxyType.AssemblyQualifiedName] = type;
                else if (!string.IsNullOrWhiteSpace(proxyType.FullName))
                    NativeTypes[proxyType.FullName] = type;
            }
            return type;
        }

    }
}
