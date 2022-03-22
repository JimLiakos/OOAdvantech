using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.Azure.Cosmos.Table;

using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{e299b644-6ded-43c8-9ef9-fb44a11186e3}</MetaDataID>
    public class Member
    {
        /// <MetaDataID>{95bd9f7d-a983-4d36-aee7-4824b495be0b}</MetaDataID>
        public void LoadRelatedObjectsData(byte[] byteStream, StorageInstanceRef storageInstanceRef, int offset, out int nextAvailablePos)
        {

            if (IsAssociationEnd)
            {
                byte[] relationStream = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBytes(byteStream, offset, ref offset);
                nextAvailablePos = offset;

                int count = storageInstanceRef.RelResolvers.Count;

                for (int i = 0; i != count; i++)
                {
                    RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                    DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                    if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                    {

                        relResolver.RelationDataSream = relationStream;

                        return;
                    }
                }

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                if (storageInstanceRef.Class.LinkAssociation != null)
                {
                    if ((MetaData is MetaDataRepository.AssociationEnd) && storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                    {

                        Guid OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToGuid(relationStream, 0, ref offset, true);

                        if ((MetaData as DotNetMetaDataRepository.AssociationEnd).IsRoleA)
                            storageInstanceRef.LinkClassRoleAOID = OID;
                        else
                            storageInstanceRef.LinkClassRoleBOID = OID;
                        return;
                        int werw = 0;
                    }
                }
                nextAvailablePos = offset;
                return;
            }
            else
                throw new System.Exception("Invalid member");
        }

        public void LoadMemberData(byte[] byteStream, int offset, out int nextAvailablePos, ElasticTableEntity entity)
        {
            nextAvailablePos = offset;

            if (Type == typeof(string))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                nextAvailablePos = offset;

                if (Name == "RowKey")
                    entity.RowKey = value;
                else if (Name == "PartitionKey")
                    entity.PartitionKey = value;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(byte[]))
            {
                byte[] value;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToBytes(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;

            }
            else if (Type == typeof(bool))
            {
                bool? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(DateTime))
            {
                DateTime? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToDateTime(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(int))
            {
                int? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(long))
            {
                long? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(double))
            {
                double? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
            else if (Type == typeof(System.Guid))
            {
                Guid? value = null;
                OOAdvantech.BinaryFormatter.BinaryFormatter.ToGuid(byteStream, offset, ref offset, out value);
                nextAvailablePos = offset;
                if (value == null)
                    entity[Name] = null;
                else
                    entity[Name] = value;
            }
        }

        /// <MetaDataID>{7236DC30-3D9E-4374-B887-4418817372ED}</MetaDataID>
        public object LoadMemberData(byte[] byteStream, StorageInstanceRef storageInstanceRef, int offset, out int nextAvailablePos)
        {


            if (IsAssociationEnd)
            {

                if (storageInstanceRef.PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
                {
                    if (Name == "Generalizations")
                    {

                    }
                    if (Name == "Specializations")
                    {

                    }

                }

                byte[] relationStream = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBytes(byteStream, offset, ref offset);
                nextAvailablePos = offset;

                int count = storageInstanceRef.RelResolvers.Count;

                for (int i = 0; i != count; i++)
                {
                    RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                    if ((relResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToMany || relResolver.AssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                   relResolver.AssociationEnd.GetOtherEnd().Navigable &&
                   relResolver.AssociationEnd.Multiplicity.IsMany)
                    {
                        continue;
                    }

                    DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                    if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                    {
                        relResolver.RelationDataSream = relationStream;

                    }
                }

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                if (storageInstanceRef.Class.LinkAssociation != null)
                {
                    if ((MetaData is MetaDataRepository.AssociationEnd) && storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                    {

                        Guid OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToGuid(relationStream, 0, ref offset, true);

                        if ((MetaData as DotNetMetaDataRepository.AssociationEnd).IsRoleA)
                            storageInstanceRef.LinkClassRoleAOID = OID;
                        else
                            storageInstanceRef.LinkClassRoleBOID = OID;
                        return null; ;
                        int werw = 0;
                    }
                }
                nextAvailablePos = offset;
                return null;
            }
            object memoryInstamce = storageInstanceRef.MemoryInstance;
            ///TODO: Να συμπληρώσω και τα άλλα types
            object fieldValue = null;


            if (Type == typeof(System.DateTime))
                fieldValue = System.DateTime.MinValue;

            if (Type == typeof(string))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);

            }
            else if (Type == typeof(bool))
            {
                bool value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(int))
            {
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(long))
            {
                long value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(short))
            {
                short value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(double))
            {
                double value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(ulong))
            {
                ulong value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(uint))
            {
                uint value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(ushort))
            {
                ushort value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(XDocument))
            {

                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    XDocument xmlDocument = XDocument.Parse(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }

#if !DeviceDotNet
            else if (Type == typeof(System.Xml.XmlDocument))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }
#endif
            else if (Type.GetMetaData().BaseType == typeof(System.Enum))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                //FieldInfo.SetValue(memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType,value));
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType, value));
                fieldValue = System.Enum.ToObject(FieldInfo.FieldType, value);
            }
            else if (Type == typeof(System.Guid))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                System.Guid guid = new System.Guid(value);
                fieldValue = guid;
                //FieldInfo.SetValue(memoryInstamce, guid);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, guid);
            }

            nextAvailablePos = offset;
            return fieldValue;


        }



        public object LoadMemberDataAndValidate(byte[] byteStream, StorageInstanceRef storageInstanceRef, int offset, out int nextAvailablePos)
        {


            if (IsAssociationEnd)
            {

                if (storageInstanceRef.PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
                {
                    if (Name == "Generalizations")
                    {

                    }
                    if (Name == "Specializations")
                    {

                    }

                }

                byte[] relationStream = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBytes(byteStream, offset, ref offset);
                nextAvailablePos = offset;

                int count = storageInstanceRef.RelResolvers.Count;

                for (int i = 0; i != count; i++)
                {

                    RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                    if (relResolver.RelationDataSream == null && relationStream == null)
                        return true;

                    if (relResolver.RelationDataSream == null && relationStream.Length == 0)
                        return true;

                    DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                    if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                    {
                        if (relResolver.RelationDataSream != null && relationStream != null)
                        {
                            return relResolver.RelationDataSream.SequenceEqual(relationStream);
                        }
                        else if (relResolver.RelationDataSream == null && relationStream == null)
                            return true;
                        else
                            return false;
                    }
                }

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                if (storageInstanceRef.Class.LinkAssociation != null)
                {
                    if ((MetaData is MetaDataRepository.AssociationEnd) && storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                    {

                        Guid OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToGuid(relationStream, 0, ref offset, true);

                        if ((MetaData as DotNetMetaDataRepository.AssociationEnd).IsRoleA)
                            return storageInstanceRef.LinkClassRoleAOID == OID;
                        else
                            return storageInstanceRef.LinkClassRoleBOID == OID;
                        return null; ;
                        int werw = 0;
                    }
                }
                nextAvailablePos = offset;
                return null;
            }
            object memoryInstamce = storageInstanceRef.MemoryInstance;
            ///TODO: Να συμπληρώσω και τα άλλα types
            object fieldValue = null;


            if (Type == typeof(System.DateTime))
                fieldValue = System.DateTime.MinValue;

            if (Type == typeof(string))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);

            }
            else if (Type == typeof(bool))
            {
                bool value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(int))
            {
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(long))
            {
                long value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(short))
            {
                short value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(double))
            {
                double value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(ulong))
            {
                ulong value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(uint))
            {
                uint value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(ushort))
            {
                ushort value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(XDocument))
            {

                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    XDocument xmlDocument = XDocument.Parse(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }

#if !DeviceDotNet
            else if (Type == typeof(System.Xml.XmlDocument))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }
#endif
            else if (Type.GetMetaData().BaseType == typeof(System.Enum))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                //FieldInfo.SetValue(memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType,value));
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType, value));
                fieldValue = System.Enum.ToObject(FieldInfo.FieldType, value);
            }
            else if (Type == typeof(System.Guid))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                System.Guid guid = new System.Guid(value);
                fieldValue = guid;
                //FieldInfo.SetValue(memoryInstamce, guid);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, guid);
            }

            nextAvailablePos = offset;
            return fieldValue;


        }


        public object ValidateMemberData(byte[] byteStream, StorageInstanceRef storageInstanceRef, int offset, out int nextAvailablePos)
        {


            if (IsAssociationEnd)
            {
                byte[] relationStream = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBytes(byteStream, offset, ref offset);
                nextAvailablePos = offset;

                int count = storageInstanceRef.RelResolvers.Count;

                //for (int i = 0; i != count; i++)
                //{
                //    RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                //    DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                //    if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                //    {
                //        relResolver.RelationDataSream = relationStream;

                //    }
                //}

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                if (storageInstanceRef.Class.LinkAssociation != null)
                {
                    if ((MetaData is MetaDataRepository.AssociationEnd) && storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                    {

                        Guid OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToGuid(relationStream, 0, ref offset, true);

                        //if ((MetaData as DotNetMetaDataRepository.AssociationEnd).IsRoleA)
                        //    storageInstanceRef.LinkClassRoleAOID = OID;
                        //else
                        //    storageInstanceRef.LinkClassRoleBOID = OID;
                        return null; ;
                        int werw = 0;
                    }
                }
                nextAvailablePos = offset;
                return null;
            }
            object memoryInstamce = storageInstanceRef.MemoryInstance;
            ///TODO: Να συμπληρώσω και τα άλλα types
            object fieldValue = null;


            if (Type == typeof(System.DateTime))
                fieldValue = System.DateTime.MinValue;

            if (Type == typeof(string))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);

            }
            else if (Type == typeof(bool))
            {
                bool value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(int))
            {
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(long))
            {
                long value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(short))
            {
                short value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(double))
            {
                double value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(ulong))
            {
                ulong value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(uint))
            {
                uint value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (Type == typeof(ushort))
            {
                ushort value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (Type == typeof(XDocument))
            {

                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    XDocument xmlDocument = XDocument.Parse(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }

#if !DeviceDotNet
            else if (Type == typeof(System.Xml.XmlDocument))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                if (value != null && value.Trim().Length != 0)
                {
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(value);
                    //FieldInfo.SetValue(memoryInstamce,xmlDocument);
                    //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, xmlDocument);
                    fieldValue = xmlDocument;
                }
            }
#endif
            else if (Type.GetMetaData().BaseType == typeof(System.Enum))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                //FieldInfo.SetValue(memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType,value));
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType, value));
                fieldValue = System.Enum.ToObject(FieldInfo.FieldType, value);
            }
            else if (Type == typeof(System.Guid))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                System.Guid guid = new System.Guid(value);
                fieldValue = guid;
                //FieldInfo.SetValue(memoryInstamce, guid);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, guid);
            }

            nextAvailablePos = offset;
            return fieldValue;


        }

        public void SaveMemberData(System.Byte[] byteStream, EntityProperty entityProperty, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            try
            {
                if (Type == typeof(string))
                {
                    string value = entityProperty.StringValue;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);
                }
                else if (Type == typeof(bool))
                {
                    //bool value=(bool)FieldInfo.GetValue(memoryInstamce) ;
                    bool? value = entityProperty.BooleanValue;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }
                else if (Type == typeof(System.Int32))
                {
                    //int value=(int)FieldInfo.GetValue(memoryInstamce)  ;
                    int? value = entityProperty.Int32Value;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }
                else if (Type == typeof(System.Int64))
                {
                    //long value=(long)FieldInfo.GetValue(memoryInstamce);
                    long? value = entityProperty.Int64Value;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }

                else if (Type == typeof(double))
                {
                    //ulong value=(ulong)FieldInfo.GetValue(memoryInstamce);
                    double? value = entityProperty.DoubleValue.Value;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }
                else if (Type == typeof(DateTime))
                {
                    DateTime? value = entityProperty.DateTime;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }
                else if (Type == typeof(byte[]))
                {
                    byte[] value = entityProperty.BinaryValue;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, OOAdvantech.BinaryFormatter.ByteStreamType.Medium, byteStream, offset, ref offset);

                }

                else if (Type == typeof(Guid))
                {
                    Guid? value = entityProperty.GuidValue;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, byteStream, offset, ref offset);

                }
                nextAvailablePos = offset;

            }
#if DEBUG
            catch (System.Exception Error)
            {
                throw new System.Exception(Error.Message, Error);
            }
#endif
            finally
            {
                //  ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

            }

        }
        public void SaveMemberData(System.Byte[] byteStream, string valueString, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(valueString, byteStream, offset, ref offset);
            nextAvailablePos = offset;

        }


        /// <MetaDataID>{933A071B-CA12-478C-B30F-F2A20EFAABBE}</MetaDataID>
        static byte[] Buffer = new byte[65536];
        ///// <MetaDataID>{B050E1FD-332F-4E26-BD95-636E7B490858}</MetaDataID>
        //static System.Threading.ReaderWriterLock ReaderWriterLock = new System.Threading.ReaderWriterLock();
        /// <MetaDataID>{D9BE173D-FE9D-4C0B-9A05-E55122066740}</MetaDataID>
        public void SaveMemberData(System.IO.MemoryStream memoryStream, StorageInstanceRef storageInstanceRef)
        {

            int offset = 0;

            // System.Threading.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(1000);
            try
            {
                if (IsAssociationEnd)
                {
                    if (storageInstanceRef.PersistentObjectID.ToString() == "43e99dd3-7a05-4a9a-90f6-3e73a9e593d2")
                    {
                        if (Name == "Generalizations")
                        {
                        }
                        if (Name == "Specializations")
                        {
                        }
                    }
                    if (MetaData == null)
                    {
                        byte[] relationStream = null;
                        BinaryFormatter.BinaryFormatter.Serialize(relationStream, Buffer, offset, ref offset);
                        memoryStream.Write(Buffer, 0, offset);
                        return;
                    }
                    //TODO:Error prone 
                    //if(storageInstanceRef.MemoryInstance.GetType().FullName == "OOAdvantech.RDBMSMetaDataRepository.Component")
                    //{

                    //}
                    int count = storageInstanceRef.RelResolvers.Count;
                    for (int i = 0; i != count; i++)
                    {
                        RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                        DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                        if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                        {
                            //if(associationEnd.Name == "Residents")
                            //{
                            //    var sdsd = (storageInstanceRef.MemoryInstance as MetaDataRepository.Component).Residents.Select(x => x.FullName).ToArray();
                            //}
                            byte[] relationStream = relResolver.GetRelationStream();
                            if (relationStream != null && relationStream.Length > 3000)
                            {

                            }

                            if (relationStream != null && relationStream.Length > Buffer.Length - offset)
                            {

                            }
                            BinaryFormatter.BinaryFormatter.Serialize(relationStream, Buffer, offset, ref offset);
                            memoryStream.Write(Buffer, 0, offset);
                            return;
                        }
                    }
                    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                    if (storageInstanceRef.Class.LinkAssociation != null)
                    {
                        if ((MetaData is MetaDataRepository.AssociationEnd) && storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                        {
                            System.Reflection.FieldInfo fieldInfo = null;
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor;
                            if ((MetaData as DotNetMetaDataRepository.AssociationEnd).IsRoleA)
                            {
                                fieldInfo = (storageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                                fastFieldAccessor = (storageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor;
                            }
                            else
                            {
                                fieldInfo = (storageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleBField;
                                fastFieldAccessor = (storageInstanceRef.Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor;
                            }

                            byte[] relationStream = null;
                            //object Value=fieldInfo.GetValue(storageInstanceRef.MemoryInstance);
                            object Value = Member<object>.GetValue(fastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);

                            offset = 0;
                            if (Value != null)
                            {
                                relationStream = new byte[4];
                                int OID = StorageInstanceRef.GetStorageInstanceRef(Value).PersistentObjectID.GetTypedMemberValue<int>("ObjectID");
                                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(OID, relationStream, offset, ref offset, true);
                            }
                            offset = 0;
                            BinaryFormatter.BinaryFormatter.Serialize(relationStream, Buffer, offset, ref offset);
                            memoryStream.Write(Buffer, 0, offset);
                            return;
                            int werw = 0;
                        }
                    }

                    int erer = 0;
                    return;
                }
                //TODO:Να συμπληρώσω και τα άλλα types
                //TODO:Να φτιάξω test case όπου η class έχει αναβαθμιστεί με την πρόσθεσηκη κάποιων member και αφαίρεση κάποιων άλλων. 

                object memoryInstamce = storageInstanceRef.MemoryInstance;
                if (FieldInfo.FieldType == typeof(string))
                {
                    //string value=FieldInfo.GetValue(memoryInstamce) as string;
                    string value = Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce) as string;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset);
                }
                else if (FieldInfo.FieldType == typeof(bool))
                {
                    //bool value=(bool)FieldInfo.GetValue(memoryInstamce) ;
                    bool value = (bool)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(int))
                {
                    //int value=(int)FieldInfo.GetValue(memoryInstamce)  ;
                    int value = (int)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(long))
                {
                    //long value=(long)FieldInfo.GetValue(memoryInstamce);
                    long value = (long)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(short))
                {
                    //short value=(short)FieldInfo.GetValue(memoryInstamce);
                    short value = (short)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(ulong))
                {
                    //ulong value=(ulong)FieldInfo.GetValue(memoryInstamce);
                    ulong value = (ulong)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(uint))
                {
                    //uint value=(uint)FieldInfo.GetValue(memoryInstamce);
                    uint value = (uint)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(ushort))
                {
                    //ushort value=(ushort)FieldInfo.GetValue(memoryInstamce);
                    ushort value = (ushort)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);

                }
                else if (FieldInfo.FieldType == typeof(double))
                {
                    //double value=(double)FieldInfo.GetValue(memoryInstamce);
                    double value = (double)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);
                }
                else if (FieldInfo.FieldType == typeof(XDocument))
                {
                    //System.Xml.XmlDocument value=FieldInfo.GetValue(memoryInstamce) as System.Xml.XmlDocument;
                    XDocument value = Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce) as XDocument;

                    string xml = null;
                    if (value != null)
                        xml = value.ToString();
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(xml, Buffer, offset, ref offset);
                }
#if !DeviceDotNet
                else if (FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                {
                    //System.Xml.XmlDocument value=FieldInfo.GetValue(memoryInstamce) as System.Xml.XmlDocument;
                    System.Xml.XmlDocument value = Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce) as System.Xml.XmlDocument;

                    string xml = null;
                    if (value != null)
                        xml = value.OuterXml;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(xml, Buffer, offset, ref offset);
                }
#endif
                else if (FieldInfo.FieldType.GetMetaData().BaseType == typeof(System.Enum))
                {
                    //int  value=(int)FieldInfo.GetValue(memoryInstamce);
                    int value = (int)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce); ;
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value, Buffer, offset, ref offset, true);
                }
                else if (FieldInfo.FieldType == typeof(System.Guid))
                {
                    //System.Guid value=(System.Guid)FieldInfo.GetValue(memoryInstamce);
                    System.Guid value = (System.Guid)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);
                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value.ToString(), Buffer, offset, ref offset);
                }

                memoryStream.Write(Buffer, 0, offset);
            }
#if DEBUG
            catch (System.Exception Error)
            {
                throw new System.Exception(Error.Message, Error);
            }
#endif
            finally
            {
                if (memoryStream.Length > 20000)
                {

                }
                //  ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

            }



        }
        /// <MetaDataID>{E09419DB-D709-4992-9773-8BDA3F259300}</MetaDataID>
        public OOAdvantech.MetaDataRepository.MetaObject MetaData;

        /// <MetaDataID>{F2884A39-CE3E-4AB7-89D6-0C41FC8ACB85}</MetaDataID>
        /// <exclude>Excluded</exclude>
        internal bool IsAssociationEnd = false;
        /// <MetaDataID>{4815FCF2-9F24-45E6-B697-206582358F6F}</MetaDataID>
        /// <exclude>Excluded</exclude>
        public string Name;
        /// <MetaDataID>{48F5A80B-0597-482A-998E-B94DDAAF8E66}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private string MetaObjectIdentity = null;
        /// <MetaDataID>{9D8297B0-9C73-49F9-AEB1-2073DE3215CC}</MetaDataID>
        public void Serialize(byte[] byteStream, int offset, out int nextAvailablePos)
        {

            if (MetaData != null)
                BinaryFormatter.BinaryFormatter.Serialize(MetaData.Identity.ToString(), byteStream, offset, ref offset);
            else
                BinaryFormatter.BinaryFormatter.Serialize(MetaObjectIdentity, byteStream, offset, ref offset);
            BinaryFormatter.BinaryFormatter.Serialize(IsAssociationEnd, byteStream, offset, ref offset, true);
            BinaryFormatter.BinaryFormatter.Serialize(TypeFullName + "->", byteStream, offset, ref offset);
            BinaryFormatter.BinaryFormatter.Serialize(TypeAssemblyName, byteStream, offset, ref offset);
            nextAvailablePos = offset;
        }

        /// <MetaDataID>{EF144D34-AADF-4969-8E16-87E892DA16E8}</MetaDataID>
        public Member(DotNetMetaDataRepository.Class _class, MetaDataRepository.MetaObject metaObject)
        {
            MetaData = metaObject;
            MetaObjectIdentity = MetaData.Identity.ToString();
            if (metaObject is DotNetMetaDataRepository.Attribute)
            {
                IsAssociationEnd = false;

                FastFieldAccessor = _class.GetFastFieldAccessor((metaObject as DotNetMetaDataRepository.Attribute));
                FieldInfo = (FastFieldAccessor.MemberInfo as FieldInfo);
                Type = (FastFieldAccessor.MemberInfo as FieldInfo).FieldType;
                TypeFullName = Type.FullName;
                TypeAssemblyName = Type.GetMetaData().Assembly.FullName;

            }
            else
            {
                IsAssociationEnd = true;
                FastFieldAccessor = _class.GetFastFieldAccessor(metaObject as DotNetMetaDataRepository.AssociationEnd);
                FieldInfo = (FastFieldAccessor.MemberInfo as FieldInfo);
                Type = (metaObject as DotNetMetaDataRepository.AssociationEnd).Specification.GetExtensionMetaObject<System.Type>();
                TypeFullName = Type.FullName;
                TypeAssemblyName = Type.GetMetaData().Assembly.FullName;
            }
            Type = ModulePublisher.ClassRepository.GetType(TypeFullName, TypeAssemblyName);
            Name = MetaData.Name;
        }
        /// <MetaDataID>{6ac35e6f-4d73-4820-83db-09b80f379743}</MetaDataID>
        System.Type Type;
        /// <MetaDataID>{9D0A9F77-4677-46B9-8EED-5DA990E67949}</MetaDataID>
        public Member(DotNetMetaDataRepository.Class _class, byte[] byteStream, int offset, out int nextAvailablePos)
        {

            MetaObjectIdentity = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            IsAssociationEnd = BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
            TypeFullName = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            if (TypeFullName.IndexOf("->") != -1)
            {
                TypeAssemblyName = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                TypeFullName = TypeFullName.Replace("->", "");
            }

            Type = ModulePublisher.ClassRepository.GetType(TypeFullName, TypeAssemblyName);
            if (IsAssociationEnd)
            {
                foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in _class.GetAssociateRoles(true))
                {
                    if (_class.IsPersistent(associationEnd))
                    {
                        if (associationEnd.Identity.ToString() == MetaObjectIdentity)
                        {
                            if (TypeFullName.Trim() == associationEnd.Specification.FullName)
                            {
                                MetaData = associationEnd;
                                //FieldInfo = _class.GetFieldMember(associationEnd);
                                FastFieldAccessor = _class.GetFastFieldAccessor(associationEnd);
                                FieldInfo = (FastFieldAccessor.MemberInfo as FieldInfo);
                                Name = MetaData.Name;
                            }
                            break;
                        }
                    }
                }
                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                if (_class.LinkAssociation != null)
                {
                    foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in _class.LinkAssociation.Connections)
                    {

                        if (associationEnd.Identity.ToString() == MetaObjectIdentity)
                        {
                            if (TypeFullName.Trim() == associationEnd.Specification.FullName)
                            {
                                MetaData = associationEnd;
                                //FieldInfo = _class.GetFieldMember(associationEnd);
                                FastFieldAccessor = _class.GetFastFieldAccessor(associationEnd);
                                FieldInfo = (FastFieldAccessor.MemberInfo as FieldInfo);
                                Name = MetaData.Name;
                            }
                            break;
                        }

                    }
                }
            }
            else
            {

                MetaDataRepository.Feature feature = _class.GetFeature(MetaObjectIdentity, true);
                if (feature == null)
                    feature = _class.GetFeature("AR_" + MetaObjectIdentity, true);


                DotNetMetaDataRepository.Attribute attribute = null;
                if (feature != null && feature is DotNetMetaDataRepository.AttributeRealization)
                    attribute = (feature as DotNetMetaDataRepository.AttributeRealization).Specification as DotNetMetaDataRepository.Attribute;

                if (feature != null && feature is DotNetMetaDataRepository.Attribute)
                    attribute = feature as DotNetMetaDataRepository.Attribute;

                if (attribute != null && _class.IsPersistent(attribute))
                {
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                    ///System.Reflection.FieldInfo attributeField = _class.GetFieldMember(attribute);
                    if (TypeFullName.Trim() == (fastFieldAccessor.MemberInfo as FieldInfo).FieldType.FullName)
                    {
                        MetaData = attribute;
                        FieldInfo = fastFieldAccessor.MemberInfo as FieldInfo;
                        FastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                        Name = MetaData.Name;
                    }
                }
            }
            nextAvailablePos = offset;
        }

        public Member(string memberName, Type memberType)
        {
            Name = memberName;
            Type = memberType;
        }

        public readonly EdmType EdmType;
        public Member(string memberName, EdmType memberType)
        {
            Name = memberName;
            EdmType = memberType;
            switch (memberType)
            {
                case EdmType.String:
                    {
                        Type = typeof(string);
                        break;
                    }

                case EdmType.DateTime:
                    {
                        Type = typeof(System.DateTime);
                        break;
                    }
                case EdmType.Int32:
                    {
                        Type = typeof(System.Int32);
                        break;
                    }
                case EdmType.Int64:
                    {
                        Type = typeof(System.Int64);
                        break;
                    }
                case EdmType.Boolean:
                    {
                        Type = typeof(System.Boolean);
                        break;
                    }
                case EdmType.Binary:
                    {
                        Type = typeof(System.Byte[]);
                        break;
                    }
                case EdmType.Double:
                    {
                        Type = typeof(System.Double);
                        break;
                    }
                case EdmType.Guid:
                    {
                        Type = typeof(System.Guid);
                        break;
                    }

            }
        }

        /// <MetaDataID>{A9B40393-9E5E-46FC-A158-B78B104FD14B}</MetaDataID>
        public System.Reflection.FieldInfo FieldInfo;
        /// <MetaDataID>{9afb21f1-6302-410e-be93-f80746c3e95b}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor;

        /// <MetaDataID>{064C378A-193E-4285-8458-49D96661EED9}</MetaDataID>
        private string TypeFullName;

        /// <MetaDataID>{a7b83bb4-0232-4c7c-818b-c79d3ad3a1ae}</MetaDataID>
        private string TypeAssemblyName;
        private string v;
    }
}
