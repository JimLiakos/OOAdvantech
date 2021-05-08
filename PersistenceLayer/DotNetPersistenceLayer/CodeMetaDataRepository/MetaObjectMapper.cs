using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{1F34C0E0-9D95-4CD0-8552-ADE3A0DF9FCD}</MetaDataID>
    public class MetaObjectMapper
    {


        /// <MetaDataID>{4E83FD50-0140-4752-8BD8-271CCE209C49}</MetaDataID>
        private static OOAdvantech.Collections.Hashtable MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
        /// <MetaDataID>{E9BBE84F-6F76-4B04-8057-7D08AC67466B}</MetaDataID>
        private static OOAdvantech.Collections.Map MetaObjects = new OOAdvantech.Collections.Map();
         
     
        /// <MetaDataID>{4BBBFCFB-CE01-4B1F-BB14-EC1525B3CC95}</MetaDataID>
        public static void AddTypeMap(object vsCodeElement, MetaDataRepository.MetaObject theMetaObject)
        {
            if (MetaobjectMapper == null)
                MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
            if (!MetaobjectMapper.Contains(vsCodeElement))
                MetaobjectMapper.Add(vsCodeElement, theMetaObject);
            else if (MetaobjectMapper[vsCodeElement] != theMetaObject)
                throw new System.Exception("Already exist metadata for " + vsCodeElement.ToString());

            //if ((MetaobjectMapper[vsCodeElement] as OOAdvantech.MetaDataRepository.MetaObject).Identity != theMetaObject.Identity)
            //{
            //    throw new System.Exception("Already exist metadata for " + vsCodeElement.ToString());
            //}
            //else
            //{
            //    MetaobjectMapper[vsCodeElement] = theMetaObject;
            //}
        }

        /// <MetaDataID>{95e587f9-b378-46f4-9bde-18723e0e4d00}</MetaDataID>
        public static void RemoveMetaObject(MetaDataRepository.MetaObject theMetaObject)
        {
            System.Collections.ArrayList removedKeys=new System.Collections.ArrayList();
            foreach (System.Collections.DictionaryEntry entry in MetaobjectMapper)
            {
                if (entry.Value == theMetaObject)
                    removedKeys.Add(entry.Key);
            }

            foreach (Object key in removedKeys)
                MetaobjectMapper.Remove(key);
            removedKeys.Clear();
            foreach (System.Collections.DictionaryEntry entry in MetaObjects)
            {
                if (entry.Value == theMetaObject)
                    removedKeys.Add(entry.Key);
            }
            foreach (Object key in removedKeys)
                MetaObjects.Remove(key);


        }

        /// <MetaDataID>{2863C603-3B91-4805-8385-B1A8425E0EE1}</MetaDataID>
        public static void RemoveType(System.Object vsCodeElement)
        {
            if (MetaobjectMapper.Count == 0)
                return;
            if (vsCodeElement != null && MetaobjectMapper != null && MetaobjectMapper.Contains(vsCodeElement))
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
            string identity = metaObject.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string;
            if (identity == null)
                return metaObject.Identity;
            return new OOAdvantech.MetaDataRepository.MetaObjectID(identity);

        }
        /// <MetaDataID>{9112EEE1-7310-4064-852E-C437360C6352}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObjectFor(object vsCodeElement)
        {
            if (MetaobjectMapper == null)
                MetaobjectMapper = new OOAdvantech.Collections.Hashtable();
            if (MetaobjectMapper.Contains(vsCodeElement))
            {
                MetaDataRepository.MetaObject metaObject = (MetaDataRepository.MetaObject)MetaobjectMapper[vsCodeElement];
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

                    string FullName = null;
                    MetaDataRepository.MetaObject metaObject = MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                    
                    
                    if (metaObject is Project)
                    {
                        if ((metaObject as Project).VSProject!= null)
                            FullName = (metaObject as Project).FullName;

                    }
                    else if (metaObject is Class)
                    {
                        if ((metaObject as Class).VSClass!= null)
                            FullName = (metaObject as Class).VSClass.FullName;

                    }
                    else if (metaObject is Interface)
                    {
                        if ((metaObject as Interface).VSInterface!= null)
                            FullName = (metaObject as Interface).VSInterface.FullName;

                    }
                    else if (metaObject is Namespace)
                    {
                        FullName = (metaObject as Namespace).FullName;
                    }
                    else
                    {
                        FullName = metaObject.FullName;
                    }
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

                    throw new System.Exception("The " + MetaObjectFullName + " has the same identity with " + FullName);
                }
            }
        }


        /// <MetaDataID>{387251fd-4ac2-4206-8f24-ee25a37ac0b6}</MetaDataID>
        public static void Clear()
        {
            MetaObjects.Clear();
            MetaobjectMapper.Clear();

            
        }
    }
}
