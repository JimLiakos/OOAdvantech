namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime.DataObjects
{
    ///<MetaDataID>{2A54354F-DF89-4B6B-BBDE-EF55ACD97542}</MetaDataID>
    public class ClassBLOB
    {

        /// <MetaDataID>{553227A1-7B76-4861-A42C-B7698E5FF4DA}</MetaDataID>
        public int ID;
        /// <MetaDataID>{1031CE81-DB99-406D-9DAC-8297C1C12E3E}</MetaDataID>
        public void Serialize(byte[] byteStream, int offset, out int nextAvailablePos)
        {
            BinaryFormatter.BinaryFormatter.Serialize(Class.Identity.ToString(), byteStream, offset, ref offset);
            System.Type type = Class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            BinaryFormatter.BinaryFormatter.Serialize(type.FullName, byteStream, offset, ref offset);
            BinaryFormatter.BinaryFormatter.Serialize(type.GetMetaData().Assembly.FullName, byteStream, offset, ref offset);
            foreach (Member member in Members)
                member.Serialize(byteStream, offset, out offset);
            nextAvailablePos = offset;

        }
        /// <MetaDataID>{DF2CF68A-A910-454F-B96E-8CAD58101AA8}</MetaDataID>
        public ClassBLOB(DotNetMetaDataRepository.Class _class)
        {

            //System.Diagnostics.Debug.WriteLine(WrType.Namespace + "." + WrType.Name);

            //System.Diagnostics.Debug.WriteLine(_class.FullName);
            _Members = new OOAdvantech.Collections.Generic.List<Member>();
            Class = _class;
            Refresh();
           
        }

        byte[] ByteStream;
        int MembersOffset;
        /// <MetaDataID>{AA9F6D58-FB70-4104-817F-04BE7506032D}</MetaDataID>
        public ClassBLOB(byte[] byteStream, int offset)
        {
            ByteStream = byteStream;
            ///TODO Nα γίνει test case όπου σε μιά class ενώ έχει serialized data να άλλάξει όνομα  και identity ένα field / property
            string MetaObjeIdentity = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            string typeFullName = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            string AssemblyFullName = BinaryFormatter.BinaryFormatter.ToString(byteStream, offset, ref offset);
            System.Diagnostics.Debug.WriteLine(AssemblyFullName);

            while (Storage.BackwardCompatibilities.ContainsKey(AssemblyFullName))
                AssemblyFullName = Storage.BackwardCompatibilities[AssemblyFullName];

            

            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( AssemblyFullName));
            DotNetMetaDataRepository.Assembly _assembly = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(assembly) as DotNetMetaDataRepository.Assembly;


            System.Type type = assembly.GetType(typeFullName);
            if (_assembly == null)
            {
                using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition())
                {
                    _assembly = OOAdvantech.DotNetMetaDataRepository.Assembly.GetComponent(assembly) as DotNetMetaDataRepository.Assembly;
                    stateTransition.Consistent = true;
                }
            }

            Class = DotNetMetaDataRepository.Type.GetClassifierObject(type) as DotNetMetaDataRepository.Class;
            MembersOffset = offset;
            //long rolesCount = Class.Roles.Count;
            //while (offset < byteStream.Length)
            //{
            //    Member member = new Member(Class, byteStream, offset, out offset);
            //    Members.Add(member);
            //}
            //Refresh();
        }
        /// <MetaDataID>{A7CDA12F-D8BE-4106-9A24-A1256C79E0DA}</MetaDataID>
        public bool HasChange = false;


        /// <MetaDataID>{5F579C13-B629-4219-996D-4D83A2E0E4CF}</MetaDataID>
        void Refresh()
        {

            
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            if (Class.LinkAssociation != null)
            {
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Class.LinkAssociation.Connections)
                {
                    Member associationEndMember = null;
                    foreach (Member member in Members)
                    {
                        if (member.MetaData == associationEnd)
                        {
                            associationEndMember = member;
                            break;
                        }
                    }
                    if (associationEndMember != null)
                        continue;
                    associationEndMember = new Member(Class, associationEnd);
                    Members.Add(associationEndMember);
                    HasChange = true;
                }
            }
            foreach (OOAdvantech.MetaDataRepository.Attribute attribute in Class.GetAttributes(true))
            {
                if (Class.IsPersistent(attribute))
                {
                    Member attributeMember = null;
                    foreach (Member member in Members)
                    {
                        if (member.MetaData == attribute)
                        {
                            attributeMember = member;
                            break;
                        }
                    }
                    if (attributeMember != null)
                        continue;
                    attributeMember = new Member(Class, attribute);
                    Members.Add(attributeMember);
                    HasChange = true;
                }
            }

            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Class.GetAssociateRoles(true))
            {
                if (associationEnd.Navigable && Class.IsPersistent(associationEnd))
                {
                    Member associationEndMember = null;
                    foreach (Member member in Members)
                    {
                        if (member.MetaData == associationEnd)
                        {
                            associationEndMember = member;
                            break;
                        }
                    }
                    if (associationEndMember != null)
                        continue;
                    associationEndMember = new Member(Class, associationEnd);
                    Members.Add(associationEndMember);
                    HasChange = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.List<Member> _Members=null;
        /// <MetaDataID>{AF05F644-CA1E-4E80-A205-DDABA00D6D4F}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<Member> Members
        {
            get
            {
                if (_Members == null)
                {
                    _Members = new OOAdvantech.Collections.Generic.List<Member>();

                    int offset = MembersOffset;
                    long rolesCount = Class.Roles.Count;
                    while (offset < ByteStream.Length)
                    {
                        Member member = new Member(Class, ByteStream, offset, out offset);
                        Members.Add(member);
                    }
                    Refresh();

                }
                return _Members;
            }
        }
        /// <MetaDataID>{198B7135-E055-477F-8A78-AC00C7D27FB1}</MetaDataID>
        public OOAdvantech.DotNetMetaDataRepository.Class Class;


    }
}
