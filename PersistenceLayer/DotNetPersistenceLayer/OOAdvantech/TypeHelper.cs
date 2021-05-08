using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OOAdvantech
{
    /// <MetaDataID>{33538d99-c464-43d1-bd4f-060e631ce4b3}</MetaDataID>
    public static class TypeHelper
    {
        /// <MetaDataID>{ab132d6f-a573-49ae-8641-893be7da5ce5}</MetaDataID>
        public static bool IsNullableType(System.Type type)
        {
            return (((type != null) && GetMetaData(type).IsGenericType) && (type.GetGenericTypeDefinition() == typeof(System.Nullable<>)));
        }

        public static TypeInfo GetMetaData(this Type type)
        {
            return new TypeInfo(type);
        }


#if DeviceDotNet
        public static bool IsField(this System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo is FieldInfo;
        }

        //public static object[] GetCustomAttributes(this System.Reflection.MemberInfo memberInfo, System.Type type, bool inherit)
        //{
        //    return (from customAttribute in memberInfo.CustomAttributes
        //            where customAttribute.AttributeType == type
        //            select customAttribute).ToArray();
        //}
        public static bool IsMethod(this System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo is MethodInfo;
        }

        public static object[] GetCustomAttributes(this MemberInfo element)
        {
            return System.Reflection.CustomAttributeExtensions.GetCustomAttributes(element).ToArray();
        }


        public static object[] GetCustomAttributes(this MemberInfo element,bool inherit)
        {
            return System.Reflection.CustomAttributeExtensions.GetCustomAttributes(element,inherit).ToArray();
        }

        public static object[] GetCustomAttributes(this MemberInfo element,System.Type attributeType, bool inherit)
        {
            return System.Reflection.CustomAttributeExtensions.GetCustomAttributes(element, attributeType, inherit).ToArray();
        }


        public static bool IsProperty(this System.Reflection.MemberInfo memberInfo)
        {
           
            return memberInfo is PropertyInfo;
        }
        public static MethodInfo GetGetMethod(this System.Reflection.PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }
        public static MethodInfo GetSetMethod(this System.Reflection.PropertyInfo propertyInfo)
        {
            return propertyInfo.SetMethod;
        }
#else
        public static bool IsField(this System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo.MemberType == System.Reflection.MemberTypes.Field;
        }
        public static bool IsProperty(this System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo.MemberType == System.Reflection.MemberTypes.Property;
        }

        public static bool IsMethod(this System.Reflection.MemberInfo memberInfo)
        {
            return memberInfo.MemberType == System.Reflection.MemberTypes.Method;
        }
#endif

        public static bool HasCustomAttribute(this System.Reflection.MemberInfo memberInfo, Type attributeType)
        {
#if !DeviceDotNet
            return memberInfo.GetCustomAttributes(attributeType, true).Length > 0;
#else
            return memberInfo.GetCustomAttribute(attributeType) != null;
#endif
        }

        public static bool HasCustomAttribute(this System.Type type, Type attributeType)
        {
#if !DeviceDotNet
            return type.GetCustomAttributes(attributeType, true).Length > 0;
#else
            return type.GetTypeInfo().GetCustomAttribute(attributeType) != null;
#endif
        }
        /// <MetaDataID>{1819727d-0c1c-4920-a3b7-ad537554fb98}</MetaDataID>
        public static bool IsEnumerable(System.Type type)
        {
            System.Type enumType = FindIEnumerable(type);
            if (enumType == null)
                  return false;
              else
                  return true;
        }

        /// <MetaDataID>{f1fc7682-b45a-4d5e-a8be-98b6d799d926}</MetaDataID>
        public static System.Type GetElementType(System.Type seqType)
        {
            System.Type type = FindIEnumerable(seqType);
            if (type == null)
            {
                return seqType;
            }
            return GetMetaData( type).GetGenericArguments()[0];
        }

        /// <MetaDataID>{47d2ecba-28f1-4892-8c53-42586130c343}</MetaDataID>
        public static System.Type FindIEnumerable(System.Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(new[] { seqType.GetElementType() });
                }
                if (GetMetaData(seqType).IsGenericType)
                {
                    foreach (System.Type type in GetMetaData(seqType).GetGenericArguments())
                    {
                        System.Type type2 = typeof(IEnumerable<>).MakeGenericType(new[] { type });
                        if (GetMetaData( type2).IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                System.Type[] interfaces = GetMetaData(seqType).GetInterfaces();
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
                if ((GetMetaData(seqType).BaseType != null) && (GetMetaData(seqType).BaseType != typeof(object)))
                {
                    return FindIEnumerable(GetMetaData(seqType).BaseType);
                }
            }
            return null;
        }

        private static Type FindIEnumerable(object baseType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What is the type of the current member?
        /// </summary>
        /// <MetaDataID>{91e1d8af-61b8-4058-b6dd-e8177bb44e12}</MetaDataID>
        public static System.Type GetMemberType(MemberInfo mi)
        {
            FieldInfo info = mi as FieldInfo;
            if (info != null)
                return info.FieldType;

            PropertyInfo info2 = mi as PropertyInfo;
            if (info2 != null)
                return info2.PropertyType;

            EventInfo info3 = mi as EventInfo;
            if (info3 != null)
                return info3.EventHandlerType;

            return null;
        }

   
    }
#if DeviceDotNet
    /// <MetaDataID>{a0a4a02a-45f6-417e-9e8f-c1a784ade7cc}</MetaDataID>
    public class CultureInfoHelper
    {
        static System.Collections.Generic.Dictionary<int, string> CultureInfoDictionary = new System.Collections.Generic.Dictionary<int, string>() { { 54, "af" }, { 1078, "af-ZA" }, { 94, "am" }, { 1118, "am-ET" }, { 1, "ar" }, { 14337, "ar-AE" }, { 15361, "ar-BH" }, { 5121, "ar-DZ" }, { 3073, "ar-EG" }, { 2049, "ar-IQ" }, { 11265, "ar-JO" }, { 13313, "ar-KW" }, { 12289, "ar-LB" }, { 4097, "ar-LY" }, { 6145, "ar-MA" }, { 8193, "ar-OM" }, { 16385, "ar-QA" }, { 1025, "ar-SA" }, { 10241, "ar-SY" }, { 7169, "ar-TN" }, { 9217, "ar-YE" }, { 122, "arn" }, { 1146, "arn-CL" }, { 77, "as" }, { 1101, "as-IN" }, { 44, "az" }, { 29740, "az-Cyrl" }, { 2092, "az-Cyrl-AZ" }, { 30764, "az-Latn" }, { 1068, "az-Latn-AZ" }, { 109, "ba" }, { 1133, "ba-RU" }, { 35, "be" }, { 1059, "be-BY" }, { 2, "bg" }, { 1026, "bg-BG" }, { 69, "bn" }, { 2117, "bn-BD" }, { 1093, "bn-IN" }, { 81, "bo" }, { 1105, "bo-CN" }, { 126, "br" }, { 1150, "br-FR" }, { 30746, "bs" }, { 25626, "bs-Cyrl" }, { 8218, "bs-Cyrl-BA" }, { 26650, "bs-Latn" }, { 5146, "bs-Latn-BA" }, { 3, "ca" }, { 1027, "ca-ES" }, { 2051, "ca-ES-valencia" }, { 92, "chr" }, { 31836, "chr-Cher" }, { 1116, "chr-Cher-US" }, { 131, "co" }, { 1155, "co-FR" }, { 5, "cs" }, { 1029, "cs-CZ" }, { 82, "cy" }, { 1106, "cy-GB" }, { 6, "da" }, { 1030, "da-DK" }, { 7, "de" }, { 3079, "de-AT" }, { 2055, "de-CH" }, { 1031, "de-DE" }, { 5127, "de-LI" }, { 4103, "de-LU" }, { 31790, "dsb" }, { 2094, "dsb-DE" }, { 101, "dv" }, { 1125, "dv-MV" }, { 8, "el" }, { 1032, "el-GR" }, { 9, "en" }, { 9225, "en-029" }, { 3081, "en-AU" }, { 10249, "en-BZ" }, { 4105, "en-CA" }, { 2057, "en-GB" }, { 15369, "en-HK" }, { 6153, "en-IE" }, { 16393, "en-IN" }, { 8201, "en-JM" }, { 17417, "en-MY" }, { 5129, "en-NZ" }, { 13321, "en-PH" }, { 18441, "en-SG" }, { 11273, "en-TT" }, { 1033, "en-US" }, { 7177, "en-ZA" }, { 12297, "en-ZW" }, { 10, "es" }, { 22538, "es-419" }, { 11274, "es-AR" }, { 16394, "es-BO" }, { 13322, "es-CL" }, { 9226, "es-CO" }, { 5130, "es-CR" }, { 7178, "es-DO" }, { 12298, "es-EC" }, { 3082, "es-ES" }, { 4106, "es-GT" }, { 18442, "es-HN" }, { 2058, "es-MX" }, { 19466, "es-NI" }, { 6154, "es-PA" }, { 10250, "es-PE" }, { 20490, "es-PR" }, { 15370, "es-PY" }, { 17418, "es-SV" }, { 21514, "es-US" }, { 14346, "es-UY" }, { 8202, "es-VE" }, { 37, "et" }, { 1061, "et-EE" }, { 45, "eu" }, { 1069, "eu-ES" }, { 41, "fa" }, { 1065, "fa-IR" }, { 103, "ff" }, { 31847, "ff-Latn" }, { 2151, "ff-Latn-SN" }, { 11, "fi" }, { 1035, "fi-FI" }, { 100, "fil" }, { 1124, "fil-PH" }, { 56, "fo" }, { 1080, "fo-FO" }, { 12, "fr" }, { 2060, "fr-BE" }, { 3084, "fr-CA" }, { 9228, "fr-CD" }, { 4108, "fr-CH" }, { 12300, "fr-CI" }, { 11276, "fr-CM" }, { 1036, "fr-FR" }, { 15372, "fr-HT" }, { 5132, "fr-LU" }, { 14348, "fr-MA" }, { 6156, "fr-MC" }, { 13324, "fr-ML" }, { 8204, "fr-RE" }, { 10252, "fr-SN" }, { 98, "fy" }, { 1122, "fy-NL" }, { 60, "ga" }, { 2108, "ga-IE" }, { 145, "gd" }, { 1169, "gd-GB" }, { 86, "gl" }, { 1110, "gl-ES" }, { 116, "gn" }, { 1140, "gn-PY" }, { 132, "gsw" }, { 1156, "gsw-FR" }, { 71, "gu" }, { 1095, "gu-IN" }, { 104, "ha" }, { 31848, "ha-Latn" }, { 1128, "ha-Latn-NG" }, { 117, "haw" }, { 1141, "haw-US" }, { 13, "he" }, { 1037, "he-IL" }, { 57, "hi" }, { 1081, "hi-IN" }, { 26, "hr" }, { 4122, "hr-BA" }, { 1050, "hr-HR" }, { 46, "hsb" }, { 1070, "hsb-DE" }, { 14, "hu" }, { 1038, "hu-HU" }, { 43, "hy" }, { 1067, "hy-AM" }, { 33, "id" }, { 1057, "id-ID" }, { 112, "ig" }, { 1136, "ig-NG" }, { 120, "ii" }, { 1144, "ii-CN" }, { 15, "is" }, { 1039, "is-IS" }, { 16, "it" }, { 2064, "it-CH" }, { 1040, "it-IT" }, { 93, "iu" }, { 30813, "iu-Cans" }, { 1117, "iu-Cans-CA" }, { 31837, "iu-Latn" }, { 2141, "iu-Latn-CA" }, { 17, "ja" }, { 1041, "ja-JP" }, { 4096, "jv" }, { 55, "ka" }, { 1079, "ka-GE" }, { 63, "kk" }, { 1087, "kk-KZ" }, { 111, "kl" }, { 1135, "kl-GL" }, { 83, "km" }, { 1107, "km-KH" }, { 75, "kn" }, { 1099, "kn-IN" }, { 18, "ko" }, { 1042, "ko-KR" }, { 87, "kok" }, { 1111, "kok-IN" }, { 146, "ku" }, { 31890, "ku-Arab" }, { 1170, "ku-Arab-IQ" }, { 64, "ky" }, { 1088, "ky-KG" }, { 110, "lb" }, { 1134, "lb-LU" }, { 84, "lo" }, { 1108, "lo-LA" }, { 39, "lt" }, { 1063, "lt-LT" }, { 38, "lv" }, { 1062, "lv-LV" }, { 129, "mi" }, { 1153, "mi-NZ" }, { 47, "mk" }, { 1071, "mk-MK" }, { 76, "ml" }, { 1100, "ml-IN" }, { 80, "mn" }, { 30800, "mn-Cyrl" }, { 1104, "mn-MN" }, { 31824, "mn-Mong" }, { 2128, "mn-Mong-CN" }, { 3152, "mn-Mong-MN" }, { 124, "moh" }, { 1148, "moh-CA" }, { 78, "mr" }, { 1102, "mr-IN" }, { 62, "ms" }, { 2110, "ms-BN" }, { 1086, "ms-MY" }, { 58, "mt" }, { 1082, "mt-MT" }, { 85, "my" }, { 1109, "my-MM" }, { 31764, "nb" }, { 1044, "nb-NO" }, { 97, "ne" }, { 2145, "ne-IN" }, { 1121, "ne-NP" }, { 19, "nl" }, { 2067, "nl-BE" }, { 1043, "nl-NL" }, { 30740, "nn" }, { 2068, "nn-NO" }, { 20, "no" }, { 108, "nso" }, { 1132, "nso-ZA" }, { 130, "oc" }, { 1154, "oc-FR" }, { 114, "om" }, { 1138, "om-ET" }, { 72, "or" }, { 1096, "or-IN" }, { 70, "pa" }, { 31814, "pa-Arab" }, { 2118, "pa-Arab-PK" }, { 1094, "pa-IN" }, { 21, "pl" }, { 1045, "pl-PL" }, { 140, "prs" }, { 1164, "prs-AF" }, { 99, "ps" }, { 1123, "ps-AF" }, { 22, "pt" }, { 1046, "pt-BR" }, { 2070, "pt-PT" }, { 134, "qut" }, { 1158, "qut-GT" }, { 107, "quz" }, { 1131, "quz-BO" }, { 2155, "quz-EC" }, { 3179, "quz-PE" }, { 23, "rm" }, { 1047, "rm-CH" }, { 24, "ro" }, { 2072, "ro-MD" }, { 1048, "ro-RO" }, { 25, "ru" }, { 1049, "ru-RU" }, { 135, "rw" }, { 1159, "rw-RW" }, { 79, "sa" }, { 1103, "sa-IN" }, { 133, "sah" }, { 1157, "sah-RU" }, { 89, "sd" }, { 31833, "sd-Arab" }, { 2137, "sd-Arab-PK" }, { 59, "se" }, { 3131, "se-FI" }, { 1083, "se-NO" }, { 2107, "se-SE" }, { 91, "si" }, { 1115, "si-LK" }, { 27, "sk" }, { 1051, "sk-SK" }, { 36, "sl" }, { 1060, "sl-SI" }, { 30779, "sma" }, { 6203, "sma-NO" }, { 7227, "sma-SE" }, { 31803, "smj" }, { 4155, "smj-NO" }, { 5179, "smj-SE" }, { 28731, "smn" }, { 9275, "smn-FI" }, { 29755, "sms" }, { 8251, "sms-FI" }, { 119, "so" }, { 1143, "so-SO" }, { 28, "sq" }, { 1052, "sq-AL" }, { 31770, "sr" }, { 27674, "sr-Cyrl" }, { 7194, "sr-Cyrl-BA" }, { 3098, "sr-Cyrl-CS" }, { 12314, "sr-Cyrl-ME" }, { 10266, "sr-Cyrl-RS" }, { 28698, "sr-Latn" }, { 6170, "sr-Latn-BA" }, { 2074, "sr-Latn-CS" }, { 11290, "sr-Latn-ME" }, { 9242, "sr-Latn-RS" }, { 48, "st" }, { 1072, "st-ZA" }, { 29, "sv" }, { 2077, "sv-FI" }, { 1053, "sv-SE" }, { 65, "sw" }, { 1089, "sw-KE" }, { 90, "syr" }, { 1114, "syr-SY" }, { 73, "ta" }, { 1097, "ta-IN" }, { 2121, "ta-LK" }, { 74, "te" }, { 1098, "te-IN" }, { 40, "tg" }, { 31784, "tg-Cyrl" }, { 1064, "tg-Cyrl-TJ" }, { 30, "th" }, { 1054, "th-TH" }, { 115, "ti" }, { 2163, "ti-ER" }, { 1139, "ti-ET" }, { 66, "tk" }, { 1090, "tk-TM" }, { 50, "tn" }, { 2098, "tn-BW" }, { 1074, "tn-ZA" }, { 31, "tr" }, { 1055, "tr-TR" }, { 49, "ts" }, { 1073, "ts-ZA" }, { 68, "tt" }, { 1092, "tt-RU" }, { 95, "tzm" }, { 31839, "tzm-Latn" }, { 2143, "tzm-Latn-DZ" }, { 30815, "tzm-Tfng" }, { 4191, "tzm-Tfng-MA" }, { 128, "ug" }, { 1152, "ug-CN" }, { 34, "uk" }, { 1058, "uk-UA" }, { 32, "ur" }, { 2080, "ur-IN" }, { 1056, "ur-PK" }, { 67, "uz" }, { 30787, "uz-Cyrl" }, { 2115, "uz-Cyrl-UZ" }, { 31811, "uz-Latn" }, { 1091, "uz-Latn-UZ" }, { 42, "vi" }, { 1066, "vi-VN" }, { 136, "wo" }, { 1160, "wo-SN" }, { 52, "xh" }, { 1076, "xh-ZA" }, { 106, "yo" }, { 1130, "yo-NG" }, { 30724, "zh" }, { 2052, "zh-CN" }, { 4, "zh-Hans" }, { 31748, "zh-Hant" }, { 3076, "zh-HK" }, { 5124, "zh-MO" }, { 4100, "zh-SG" }, { 1028, "zh-TW" }, { 53, "zu" }, { 1077, "zu-ZA" } };
        public static System.Globalization.CultureInfo GetCultureInfo(int lcid)
        {
            if (CultureInfoDictionary.ContainsKey(lcid))
                return new System.Globalization.CultureInfo(CultureInfoDictionary[lcid]);
            else
                return null;
        }
    }
#else

    /// <MetaDataID>{9c430a8a-7480-4ee8-9b7d-dad76647537e}</MetaDataID>
    public class CultureInfoHelper
    {
        public static System.Globalization.CultureInfo GetCultureInfo(int lcid)
        {
            return new System.Globalization.CultureInfo(lcid);
        }
    }
#endif
}
