using System.Reflection;
namespace OOAdvantech.MSSQLFastPersistenceRunTime.DataObjects
{

    /// <MetaDataID>{415FE2A9-1DEE-413d-8575-8DAFB7CFD1CD}</MetaDataID>
    public class Member
    {
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

                        int OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(relationStream, 0, ref offset, true);

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
        /// <MetaDataID>{7236DC30-3D9E-4374-B887-4418817372ED}</MetaDataID>
        public object LoadMemberData(byte[] byteStream, StorageInstanceRef storageInstanceRef, int offset, out int nextAvailablePos)
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

                    }
                }

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                if (storageInstanceRef.Class.LinkAssociation != null)
                {
                    if ((MetaData is MetaDataRepository.AssociationEnd)&&storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                    {

                        int OID = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(relationStream, 0, ref offset, true);

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

            if (FieldInfo.FieldType == typeof(System.DateTime))
                fieldValue = System.DateTime.MinValue;

            if (FieldInfo.FieldType == typeof(string))
            {
                string value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);

            }
            else if (FieldInfo.FieldType == typeof(bool))
            {
                bool value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(int))
            {
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(long))
            {
                long value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(short))
            {
                short value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(double))
            {
                double value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(ulong))
            {
                ulong value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt64(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(uint))
            {
                uint value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt32(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref  memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(ushort))
            {
                ushort value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToUInt16(byteStream, offset, ref offset, true);
                fieldValue = value;
                //FieldInfo.SetValue(memoryInstamce,value);
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, value);
            }
            else if (FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
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
            else if (FieldInfo.FieldType.BaseType == typeof(System.Enum))
            {
                //TODO:Να τσεκάρω αν δοθλεύει
                int value = OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream, offset, ref offset, true);
                //FieldInfo.SetValue(memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType,value));
                //Member<object>.SetValue(FastFieldAccessor, ref memoryInstamce, System.Enum.ToObject(FieldInfo.FieldType, value));
                fieldValue = System.Enum.ToObject(FieldInfo.FieldType, value);
            }
            else if (FieldInfo.FieldType == typeof(System.Guid))
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
        /// <MetaDataID>{933A071B-CA12-478C-B30F-F2A20EFAABBE}</MetaDataID>
        static byte[] Buffer = new byte[4096];
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
                    //TODO:Error prone 
                    int count = storageInstanceRef.RelResolvers.Count;
                    for (int i = 0; i != count; i++)
                    {
                        RelResolver relResolver = storageInstanceRef.RelResolvers[i] as RelResolver;
                        DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                        if (associationEnd == (MetaData as DotNetMetaDataRepository.AssociationEnd))
                        {
                            byte[] relationStream = relResolver.GetRelationStream();
                            BinaryFormatter.BinaryFormatter.Serialize(relationStream, Buffer, offset, ref offset);
                            memoryStream.Write(Buffer, 0, offset);
                            return;
                        }
                    }
                    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                    if (storageInstanceRef.Class.LinkAssociation != null)
                    {
                        if ((MetaData is MetaDataRepository.AssociationEnd)&& storageInstanceRef.Class.LinkAssociation.Connections.Contains(MetaData as MetaDataRepository.AssociationEnd))
                        {
                            System.Reflection.FieldInfo fieldInfo = null;
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor ;
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
                                int OID = (int)StorageInstanceRef.GetStorageInstanceRef(Value).ObjectID;
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
                if (  FieldInfo.FieldType == typeof(string))
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
                else if (FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                {
                    //System.Xml.XmlDocument value=FieldInfo.GetValue(memoryInstamce) as System.Xml.XmlDocument;
                    System.Xml.XmlDocument value = Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce) as System.Xml.XmlDocument;

                    string xml = null;
                    if (value != null)
                        xml = value.OuterXml;

                    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(xml, Buffer, offset, ref offset);

                }
                else if (FieldInfo.FieldType.BaseType == typeof(System.Enum))
                {
                    //int  value=(int)FieldInfo.GetValue(memoryInstamce);
                    int value = (int)Member<object>.GetValue(FastFieldAccessor.GetValue, memoryInstamce);;
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
            BinaryFormatter.BinaryFormatter.Serialize(TypeFullName, byteStream, offset, ref offset);
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
                TypeFullName = (FastFieldAccessor.MemberInfo as FieldInfo).FieldType.FullName;
            }
            else
            {
                IsAssociationEnd = true;
                FastFieldAccessor = _class.GetFastFieldAccessor(metaObject as DotNetMetaDataRepository.AssociationEnd);
                FieldInfo = (FastFieldAccessor.MemberInfo as FieldInfo);
                TypeFullName = (metaObject as DotNetMetaDataRepository.AssociationEnd).Specification.FullName;
            }
            Name = MetaData.Name;
        }

        /// <MetaDataID>{9D0A9F77-4677-46B9-8EED-5DA990E67949}</MetaDataID>
        public Member(DotNetMetaDataRepository.Class _class, byte[] byteStream, int offset, out int nextAvailablePos)
        {

            MetaObjectIdentity = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            IsAssociationEnd = BinaryFormatter.BinaryFormatter.ToBoolean(byteStream, offset, ref offset, true);
            TypeFullName = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
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
                foreach (DotNetMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
                {
                    if (_class.IsPersistent(attribute) && attribute.Identity.ToString() == MetaObjectIdentity)
                    {
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                        ///System.Reflection.FieldInfo attributeField = _class.GetFieldMember(attribute);

                        if (TypeFullName.Trim() == (fastFieldAccessor.MemberInfo as FieldInfo) .FieldType.FullName)
                        {
                            MetaData = attribute;
                            FieldInfo = fastFieldAccessor.MemberInfo as FieldInfo;
                            FastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                            Name = MetaData.Name;
                        }
                        break;
                    }
                }

            }

            nextAvailablePos = offset;

        }

        /// <MetaDataID>{A9B40393-9E5E-46FC-A158-B78B104FD14B}</MetaDataID>
        public System.Reflection.FieldInfo FieldInfo;
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor;

        /// <MetaDataID>{064C378A-193E-4285-8458-49D96661EED9}</MetaDataID>
        private string TypeFullName;
    }
}
