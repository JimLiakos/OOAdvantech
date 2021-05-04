using System;
using System.Collections.Generic;
using System.Text;
using MetaDataRepository = OOAdvantech.MetaDataRepository;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{da84de51-0526-4ff2-b2e8-ad1fec5110cd}</MetaDataID>
    internal class MetaObjectMapper
    {
       static System.Collections.Generic.Dictionary<string, string> TypesFullNameDictionary = new Dictionary<string, string>();
        static MetaObjectMapper()
        {
            TypesFullNameDictionary[typeof(int).FullName] = "int";
            TypesFullNameDictionary[typeof(char).FullName] = "char";
            TypesFullNameDictionary[typeof(bool).FullName] = "bool";
            TypesFullNameDictionary[typeof(void).FullName] = "void";
            TypesFullNameDictionary[typeof(double).FullName] = "double";
            TypesFullNameDictionary[typeof(byte).FullName] = "byte";
            TypesFullNameDictionary[typeof(decimal).FullName] = "decimal";
            TypesFullNameDictionary[typeof(long).FullName] = "long";
            TypesFullNameDictionary[typeof(object).FullName] = "object";
            TypesFullNameDictionary[typeof(sbyte).FullName] = "sbyte";
            TypesFullNameDictionary[typeof(short).FullName] = "short";
            TypesFullNameDictionary[typeof(uint).FullName] = "uint";
            TypesFullNameDictionary[typeof(ulong).FullName] = "ulong";
            TypesFullNameDictionary[typeof(ushort).FullName] = "ushort";
            TypesFullNameDictionary[typeof(string).FullName] = "string";
            

        }

        public static string GetShortNameFor(string fullName)
        {
            if (!TypesFullNameDictionary.ContainsKey(fullName))
                return fullName;
            else
                return TypesFullNameDictionary[fullName];
        }


        /// <MetaDataID>{4E83FD50-0140-4752-8BD8-271CCE209C49}</MetaDataID>
        private static OOAdvantech.Collections.Hashtable MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
        /// <MetaDataID>{E9BBE84F-6F76-4B04-8057-7D08AC67466B}</MetaDataID>
        private static OOAdvantech.Collections.Map MetaObjects = new OOAdvantech.Collections.Map();


        /// <MetaDataID>{4BBBFCFB-CE01-4B1F-BB14-EC1525B3CC95}</MetaDataID>
        public static void AddTypeMap(object roseItem, MetaDataRepository.MetaObject theMetaObject)
        {
            
            if (MetaobjectMapper == null)
                MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
            if (!MetaobjectMapper.Contains(roseItem))
                MetaobjectMapper.Add(roseItem, theMetaObject);
            else if (MetaobjectMapper[roseItem] != theMetaObject)
                throw new System.Exception("Already exist metadata for " + roseItem.ToString());
            if (!MetaObjects.Contains(GetIdentity(theMetaObject).ToString()))
                MetaObjects[GetIdentity(theMetaObject).ToString()] = theMetaObject;



        }

        /// <MetaDataID>{2863C603-3B91-4805-8385-B1A8425E0EE1}</MetaDataID>
        public static void RemoveType(System.Object vsCodeElement)
        {
            if (MetaobjectMapper != null && MetaobjectMapper.Contains(vsCodeElement))
                MetaobjectMapper.Remove(vsCodeElement);
        }
        /// <MetaDataID>{C7266E6A-F35D-4B19-A5F2-69B3FAD55A45}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObject(MetaDataRepository.MetaObjectID identity)
        {
            //if (DisableMetaObjectIdentityMapper)
            //    return null;
            if (identity == null)
                return null;

            if (MetaObjects == null)
                MetaObjects = new OOAdvantech.Collections.Map();
            if (MetaObjects.Contains(identity.ToString()))
                return (MetaDataRepository.MetaObject)MetaObjects[identity.ToString()];



            foreach (System.Collections.DictionaryEntry entry in MetaobjectMapper)
            {
                MetaDataRepository.MetaObject metaObject = entry.Value as MetaDataRepository.MetaObject;
                MetaObjects[GetIdentity(metaObject).ToString()] = metaObject;

            }

            if (MetaObjects.Contains(identity.ToString()))
                return (MetaDataRepository.MetaObject)MetaObjects[identity.ToString()];


            return null;

        }
        public static OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            return new OOAdvantech.MetaDataRepository.MetaObjectID(metaObject.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string);
        }

        /// <MetaDataID>{9112EEE1-7310-4064-852E-C437360C6352}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObjectFor(object roseItem)
        {
            if (MetaobjectMapper == null)
                MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
            if (MetaobjectMapper.Contains(roseItem))
            {
                MetaDataRepository.MetaObject metaObject = (MetaDataRepository.MetaObject)MetaobjectMapper[roseItem];
                return metaObject;
            }

            return null;
        }
        /// <MetaDataID>{6E55C8BD-B217-40AD-80AC-A7ABA8D9492A}</MetaDataID>
        internal static void AddMetaObject(MetaDataRepository.MetaObject theMetaObject, string MetaObjectFullName)
        {
            string identityAsString = GetIdentity(theMetaObject).ToString().Trim();
            if (!MetaObjects.Contains(identityAsString))
                MetaObjects[identityAsString] = theMetaObject;
            else
            {

                MetaDataRepository.MetaObject oldMetaObject = MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                if (oldMetaObject != theMetaObject)
                {

                    
                    MetaDataRepository.MetaObject metaObject = MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                    string fullName = metaObject.FullName;

                    //if (metaObject is Project)
                    //{
                    //    if ((metaObject as Project).VSProject != null)
                    //        FullName = (metaObject as Project).FullName;

                    //}
                    //else if (metaObject is Class)
                    //{
                    //    if ((metaObject as Class).VSClass != null)
                    //        FullName = (metaObject as Class).VSClass.FullName;

                    //}
                    //else if (metaObject is Interface)
                    //{
                    //    if ((metaObject as Interface).VSInterface != null)
                    //        FullName = (metaObject as Interface).VSInterface.FullName;

                    //}
                    //else if (metaObject is Namespace)
                    //{
                    //    FullName = (metaObject as Namespace).FullName;
                    //}
                    //else if (metaObject is Attribute)
                    //{

                    //    if ((metaObject as Attribute).wrMember != null)
                    //        FullName = (metaObject as Attribute).wrMember.DeclaringType.FullName + "." + (metaObject as Attribute).wrMember.Name;
                    //}
                    //else if (metaObject is AssociationEnd)
                    //{
                    //    if ((metaObject as AssociationEnd).WrMemberInfo != null)
                    //        FullName = (metaObject as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (metaObject as AssociationEnd).WrMemberInfo.Name;
                    //}

                    //else if (metaObject is Enumeration)
                    //{
                    //    if ((metaObject as Enumeration).Refer != null)
                    //        FullName = (metaObject as Enumeration).Refer.WrType.FullName;

                    //}
                    //else if (metaObject is Method)
                    //{
                    //    if ((metaObject as Method).WrMethod != null)
                    //        FullName = (metaObject as Method).WrMethod.DeclaringType.FullName + "." + (metaObject as Method).WrMethod.Name;
                    //}

                    //else if (metaObject is Operation)
                    //{
                    //    if ((metaObject as Operation).WrMethod != null)
                    //        FullName = (metaObject as Operation).WrMethod.DeclaringType.FullName + "." + (metaObject as Operation).WrMethod.Name;
                    //}
                    //else if (metaObject is Primitive)
                    //{
                    //    if ((metaObject as Primitive).Refer != null)
                    //        FullName = (metaObject as Primitive).Refer.WrType.FullName;
                    //}
                    //else if (metaObject is Structure)
                    //{
                    //    if ((metaObject as Structure).Refer != null)
                    //        FullName = (metaObject as Structure).Refer.WrType.FullName;
                    //}
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("The " + MetaObjectFullName + " has the same identity with " + fullName);
                    throw new System.Exception("The " + MetaObjectFullName + " has the same identity with " + fullName);
                }
            }
        }


        public static void Clear()
        {
            MetaObjects.Clear();
            MetaobjectMapper.Clear();


        }
    }
}
