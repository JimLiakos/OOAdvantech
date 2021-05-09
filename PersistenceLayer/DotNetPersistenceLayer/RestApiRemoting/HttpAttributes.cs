using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{fbe20c8f-af60-4910-a602-09c5935c9ca0}</MetaDataID>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property)]
    public class HttpVisible : System.Attribute
    {

    }

    /// <MetaDataID>{529b1cb0-496a-4c73-894f-cc650f8ba870}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class HttpInVisible : System.Attribute
    {

    }

    /// <MetaDataID>{cabc24a9-dc8d-450d-9d95-a61d6926e2e2}</MetaDataID>
    public static class HttpMetaOperators
    {
        public static bool IsHttpVisible(this Classifier classifier, Feature feature)
        {


            bool classifierIsHttpVisible = false;
            var type = classifier.GetExtensionMetaObject<System.Type>();
            var classifierAttributes = type.GetCustomAttributes(typeof(HttpVisible), false);
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
    }

    /// <MetaDataID>{7381a010-2e92-4df3-82d1-eb13e3acbca2}</MetaDataID>
    public class ProxyType
    {

        string _Name;
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
        string _FullName;
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

        public ProxyType(Type type)
        {
            var classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);

            _FullName = type.AssemblyQualifiedName;
            _Name = type.Name;

            _Methods = (from method in classifier.GetFeatures(false).OfType<Operation>()
                        where method.Visibility == VisibilityKind.AccessPublic &&
                        classifier.IsHttpVisible(method)
                        select method.Name).Distinct().ToList();

            _Properties = (from method in classifier.GetFeatures(false).OfType<Attribute>()
                           where method.Visibility == VisibilityKind.AccessPublic &&
                           classifier.IsHttpVisible(method)
                           select method.Name).Distinct().ToList();

            if (classifier is Class)
                _Interfaces = (from _interface in (classifier as Class).GetAllInterfaces()
                               select new ProxyType(_interface.GetExtensionMetaObject<System.Type>())).ToList();
            else
                _Interfaces = new List<ProxyType>();

        }

        List<ProxyType> _Interfaces;
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

        public ProxyType()
        {

        }

        List<string> _Methods;
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

        List<string> _Properties;
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

    }


}
