using System;
using System.Reflection;
using System.Linq;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{FED8868B-BCB2-44E3-A53B-2CA89B7D05A5}</MetaDataID>
    public class MetaObjectMapper
    {
        /// <MetaDataID>{44ebd05e-8cdd-4eb4-8bf1-c80b7507bb52}</MetaDataID>
        public static bool DisableMetaObjectIdentityMapper = false;
        /// <MetaDataID>{81CF3448-BA9F-4F9C-9637-3E1DD09AC17F}</MetaDataID>
        private static System.Collections.Generic.Dictionary<object, MetaDataRepository.MetaObject> MetaobjectMapper;


        static readonly object MetaObjectMapperLock = new object();
        /// <MetaDataID>{24DFD23D-E3BD-4078-94A2-A78FE0898082}</MetaDataID>
        public static void AddTypeMap(System.Object reflactionObject, MetaDataRepository.MetaObject theMetaObject)
        {
            lock (MetaObjectMapperLock)
            {
                reflactionObject = GetReflactionObjectID(reflactionObject);
                if (MetaobjectMapper == null)
                    MetaobjectMapper = new System.Collections.Generic.Dictionary<object, OOAdvantech.MetaDataRepository.MetaObject>();

                MetaDataRepository.MetaObject existingMetaObject = null;
                if (MetaobjectMapper.TryGetValue(reflactionObject, out existingMetaObject))
                {
                    if (existingMetaObject != theMetaObject)
                        throw new System.Exception("Already exist metadata for " + reflactionObject.ToString());
                }
                else
                    MetaobjectMapper.Add(reflactionObject, theMetaObject);
            }

        }
        /// <MetaDataID>{7F64934F-A969-49E7-9EA3-85A40944525E}</MetaDataID>
		public static void RemoveType(System.Object reflactionObject)
        {
            reflactionObject = GetReflactionObjectID(reflactionObject);
            if (MetaobjectMapper != null && MetaobjectMapper.ContainsKey(reflactionObject))
                MetaobjectMapper.Remove(reflactionObject);
        }

        /// <MetaDataID>{9e0d73e2-aec4-44dc-b0df-5331040d0bfd}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObject(MetaDataRepository.MetaObjectID identity)
        {
            return FindMetaObject(identity, null);

        }

        /// <MetaDataID>{5A434073-EE0E-441A-982D-F1E54C869C07}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObject(MetaDataRepository.MetaObjectID identity, System.Reflection.Assembly assembly)
        {
            if (identity == null)
                return null;

            lock (MetaObjectMapperLock)
            {
                MetaDataRepository.MetaObject metaObject = null;

                if (MetaObjects == null)
                    MetaObjects = new Collections.Generic.Dictionary<string, Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>>();

                foreach (var implementationUnitMetaObjects in MetaObjects.Values)
                {
                    if (implementationUnitMetaObjects.TryGetValue(identity.ToString(), out metaObject))
                        return metaObject;
                }
            }



            //foreach (MetaDataRepository.MetaObject metaObject in MetaobjectMapper.Values)
            //    MetaObjects[metaObject.Identity.ToString()]=metaObject;

            //if(MetaObjects.Contains(identity.ToString()))
            //    return (MetaDataRepository.MetaObject)MetaObjects[identity.ToString()];

            return null;

        }
        /// <MetaDataID>{57CFA637-72E5-448F-8390-E7B351A2D6E3}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObjectFor(object reflactionObject)
        {
            lock (MetaObjectMapperLock)
            {
                reflactionObject = GetReflactionObjectID(reflactionObject);
                if (MetaobjectMapper == null)
                    MetaobjectMapper = new System.Collections.Generic.Dictionary<object, OOAdvantech.MetaDataRepository.MetaObject>();

                MetaDataRepository.MetaObject metaObject;
                MetaobjectMapper.TryGetValue(reflactionObject, out metaObject);
                return metaObject;
            }

        }

        /// <MetaDataID>{65c98dcd-13c0-46f6-913d-e8d7f5b5bdc7}</MetaDataID>
        private static object GetReflactionObjectID(object reflactionObject)
        {
#if DeviceDotNet

            //if(reflactionObject is System.Type)
            //{
            //    if((reflactionObject as System.Type).AssemblyQualifiedName==null)
            //        return  (reflactionObject as System.Type).DeclaringType.AssemblyQualifiedName + "_" + (reflactionObject as System.Type).Name;

            //    return (reflactionObject as System.Type).AssemblyQualifiedName;
            //}
            //else if (reflactionObject is System.Reflection.PropertyInfo)
            //{
            //    return (reflactionObject as System.Reflection.PropertyInfo).DeclaringType.AssemblyQualifiedName + "." + (reflactionObject as System.Reflection.PropertyInfo).Name;
            //}
            //else if (reflactionObject is System.Reflection.MethodInfo)
            //{
            //}
            //else if (reflactionObject is System.Reflection.FieldInfo)
            //{
            //    return (reflactionObject as System.Reflection.FieldInfo).DeclaringType.AssemblyQualifiedName + "." + (reflactionObject as System.Reflection.FieldInfo).Name;
            //}
            //else if (reflactionObject is System.Reflection.Assembly)
            //{
            //    return (reflactionObject as System.Reflection.Assembly).FullName;
            //}


            return reflactionObject;
#else
            return reflactionObject;
#endif
        }

        /// <MetaDataID>{04b8845c-f74f-4bae-ae68-ed710bdde392}</MetaDataID>
        static internal void RemoveMetaObject(MetaDataRepository.MetaObject theMetaObject)
        {
            if (theMetaObject == null)
                return;
            lock (MetaObjectMapperLock)
            {
                string identityAsString = theMetaObject.Identity.ToString().Trim();


                string implementationUnitIdentity = "";
                if (theMetaObject.ImplementationUnit != null)
                {
                    implementationUnitIdentity = theMetaObject.ImplementationUnit.Identity.ToString();
                    if (theMetaObject.ImplementationUnit is Assembly)
                        implementationUnitIdentity += (theMetaObject.ImplementationUnit as Assembly).WrAssembly.GetHashCode().ToString();
                }

                if (!MetaObjects.ContainsKey(implementationUnitIdentity))
                    MetaObjects[implementationUnitIdentity] = new Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();

                Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> implementationUnitMetaObjects = MetaObjects[implementationUnitIdentity];
                if (DisableMetaObjectIdentityMapper)
                {
                    implementationUnitMetaObjects[identityAsString] = theMetaObject;
                    return;
                }

                if (implementationUnitMetaObjects.ContainsKey(identityAsString))
                    implementationUnitMetaObjects.Remove(identityAsString);
            }

        }
        /// <MetaDataID>{273F9C08-16B4-4637-8261-EE7B659E66A9}</MetaDataID>
        static internal void AddMetaObject(MetaDataRepository.MetaObject theMetaObject, string MetaObjectFullName)
        {

            lock (MetaObjectMapperLock)
            {
                if (theMetaObject.Identity.ToString().Trim() == "M:{3d5670ae-e925-4c84-801f-de7fc948144b}.GetFlavoursServicesContext[String]")
                {

                }
                string identityAsString = theMetaObject.Identity.ToString().Trim();
                string implementationUnitIdentity = "";
                if (theMetaObject.ImplementationUnit != null)
                {
                    implementationUnitIdentity = theMetaObject.ImplementationUnit.Identity.ToString();
                    if (theMetaObject.ImplementationUnit is Assembly)
                        implementationUnitIdentity += (theMetaObject.ImplementationUnit as Assembly).WrAssembly.GetHashCode().ToString();
                }

                if (!MetaObjects.ContainsKey(implementationUnitIdentity))
                    MetaObjects[implementationUnitIdentity] = new Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();

                Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> implementationUnitMetaObjects = MetaObjects[implementationUnitIdentity];
                if (DisableMetaObjectIdentityMapper)
                {
                    implementationUnitMetaObjects[identityAsString] = theMetaObject;
                    return;
                }

                if (!implementationUnitMetaObjects.ContainsKey(identityAsString))
                    implementationUnitMetaObjects[identityAsString] = theMetaObject;
                else
                {
                    MetaDataRepository.MetaObject oldMetaObject = implementationUnitMetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                    if (oldMetaObject != theMetaObject)
                    {

                        string FullName = null;
                        //MetaDataRepository.MetaObject metaObject=MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                        if (oldMetaObject is Assembly)
                        {
                            if ((oldMetaObject as Assembly).WrAssembly != null)
                                FullName = (oldMetaObject as Assembly).WrAssembly.FullName;

                        }
                        else if (oldMetaObject is Attribute)
                        {

                            if ((oldMetaObject as Attribute).wrMember != null)
                                FullName = (oldMetaObject as Attribute).wrMember.DeclaringType.FullName + "." + (oldMetaObject as Attribute).wrMember.Name;
                        }
                        else if (oldMetaObject is AssociationEnd)
                        {
                            if ((oldMetaObject as AssociationEnd).WrMemberInfo != null)
                                FullName = (oldMetaObject as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (oldMetaObject as AssociationEnd).WrMemberInfo.Name;
                        }
                        else if (oldMetaObject is Class)
                        {
                            if ((oldMetaObject as Class).Refer != null)
                                FullName = (oldMetaObject as Class).Refer.WrType.FullName;
                        }
                        else if (oldMetaObject is AttributeRealization)
                        {
                            if ((oldMetaObject as AttributeRealization).PropertyMember != null)
                                FullName = (oldMetaObject as AttributeRealization).PropertyMember.DeclaringType.FullName + "." + (oldMetaObject as AttributeRealization).PropertyMember.Name;

                            if ((oldMetaObject as AttributeRealization).FieldMember != null)
                                FullName = (oldMetaObject as AttributeRealization).FieldMember.DeclaringType.FullName + "." + (oldMetaObject as AttributeRealization).FieldMember.Name;

                        }
                        else if (oldMetaObject is AssociationEndRealization)
                        {
                            if ((oldMetaObject as AssociationEndRealization).PropertyMember != null)
                                FullName = (oldMetaObject as AssociationEndRealization).PropertyMember.DeclaringType.FullName + "." + (oldMetaObject as AssociationEndRealization).PropertyMember.Name;
                            if ((oldMetaObject as AssociationEndRealization).FieldMember != null)
                                FullName = (oldMetaObject as AssociationEndRealization).FieldMember.DeclaringType.FullName + "." + (oldMetaObject as AssociationEndRealization).FieldMember.Name;
                        }
                        else if (oldMetaObject is Enumeration)
                        {
                            if ((oldMetaObject as Enumeration).Refer != null)
                                FullName = (oldMetaObject as Enumeration).Refer.WrType.FullName;
                        }
                        else if (oldMetaObject is Method)
                        {
                            if ((oldMetaObject as Method).WrMethod != null)
                                FullName = (oldMetaObject as Method).WrMethod.DeclaringType.FullName + "." + (oldMetaObject as Method).WrMethod.Name;
                        }
                        else if (oldMetaObject is Namespace)
                        {
                            FullName = (oldMetaObject as Namespace).FullName;
                        }
                        else if (oldMetaObject is Operation)
                        {
                            if ((oldMetaObject as Operation).WrMethod != null)
                                FullName = (oldMetaObject as Operation).WrMethod.DeclaringType.FullName + "." + (oldMetaObject as Operation).WrMethod.Name;
                        }
                        else if (oldMetaObject is Primitive)
                        {
                            if ((oldMetaObject as Primitive).Refer != null)
                                FullName = (oldMetaObject as Primitive).Refer.WrType.FullName;
                        }
                        else if (oldMetaObject is Structure)
                        {
                            if ((oldMetaObject as Structure).Refer != null)
                                FullName = (oldMetaObject as Structure).Refer.WrType.FullName;
                        }



                        if (theMetaObject is Assembly)
                        {
                            if ((theMetaObject as Assembly).WrAssembly != null)
                                MetaObjectFullName = (theMetaObject as Assembly).WrAssembly.FullName;
                        }
                        else if (theMetaObject is Attribute)
                        {
                            if ((theMetaObject as Attribute).wrMember != null)
                                MetaObjectFullName = (theMetaObject as Attribute).wrMember.DeclaringType.FullName + "." + (theMetaObject as Attribute).wrMember.Name;
                        }
                        else if (theMetaObject is AssociationEnd)
                        {
                            if ((theMetaObject as AssociationEnd).WrMemberInfo != null)
                                MetaObjectFullName = (theMetaObject as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (theMetaObject as AssociationEnd).WrMemberInfo.Name;
                        }
                        else if (theMetaObject is Class)
                        {
                            if ((theMetaObject as Class).Refer != null)
                                MetaObjectFullName = (theMetaObject as Class).Refer.WrType.FullName;
                        }
                        else if (theMetaObject is AttributeRealization)
                        {
                            if ((theMetaObject as AttributeRealization).PropertyMember != null)
                                MetaObjectFullName = (theMetaObject as AttributeRealization).PropertyMember.DeclaringType.FullName + "." + (theMetaObject as AttributeRealization).PropertyMember.Name;

                            if ((theMetaObject as AttributeRealization).FieldMember != null)
                                MetaObjectFullName = (theMetaObject as AttributeRealization).FieldMember.DeclaringType.FullName + "." + (theMetaObject as AttributeRealization).FieldMember.Name;
                        }
                        else if (theMetaObject is AssociationEndRealization)
                        {
                            if ((theMetaObject as AssociationEndRealization).PropertyMember != null)
                                MetaObjectFullName = (theMetaObject as AssociationEndRealization).PropertyMember.DeclaringType.FullName + "." + (theMetaObject as AssociationEndRealization).PropertyMember.Name;
                            if ((theMetaObject as AssociationEndRealization).FieldMember != null)
                                MetaObjectFullName = (theMetaObject as AssociationEndRealization).FieldMember.DeclaringType.FullName + "." + (theMetaObject as AssociationEndRealization).FieldMember.Name;
                        }
                        else if (theMetaObject is Enumeration)
                        {
                            if ((theMetaObject as Enumeration).Refer != null)
                                MetaObjectFullName = (theMetaObject as Enumeration).Refer.WrType.FullName;
                        }
                        else if (theMetaObject is Method)
                        {
                            if ((theMetaObject as Method).WrMethod != null)
                                MetaObjectFullName = (theMetaObject as Method).WrMethod.DeclaringType.FullName + "." + (theMetaObject as Method).WrMethod.Name;
                        }
                        else if (theMetaObject is Namespace)
                        {
                            MetaObjectFullName = (theMetaObject as Namespace).FullName;
                        }
                        else if (theMetaObject is Operation)
                        {
                            if ((theMetaObject as Operation).WrMethod != null)
                                MetaObjectFullName = (theMetaObject as Operation).WrMethod.DeclaringType.FullName + "." + (theMetaObject as Operation).WrMethod.Name;
                        }
                        else if (theMetaObject is Primitive)
                        {
                            if ((theMetaObject as Primitive).Refer != null)
                                MetaObjectFullName = (theMetaObject as Primitive).Refer.WrType.FullName;
                        }
                        else if (theMetaObject is Structure)
                        {
                            if ((theMetaObject as Structure).Refer != null)
                                MetaObjectFullName = (theMetaObject as Structure).Refer.WrType.FullName;
                        }


#if DEBUG
                        bool throwException = true;
                        if (throwException)
                            throw new System.Exception("The " + MetaObjectFullName + " has the same identity with " + FullName);
#endif
                    }
                }
            }
        }

        /// <MetaDataID>{75BC4CEF-2601-4102-BBA2-D74A2A415D54}</MetaDataID>
        static OOAdvantech.Collections.Generic.Dictionary<string, OOAdvantech.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>> MetaObjects = new Collections.Generic.Dictionary<string, Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>>();
    }
}
