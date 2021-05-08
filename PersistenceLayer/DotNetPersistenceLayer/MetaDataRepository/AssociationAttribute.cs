using System;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{67a342ec-d73d-4017-8640-7b31c494122c}</MetaDataID>
    public enum Roles
    {
        RoleA,
        RoleB
    };

    /// <MetaDataID>{E3A8C789-FCA3-42D6-A8BD-D05ABE87C37E}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class RoleAMultiplicityRangeAttribute : System.Attribute
    {

        /// <MetaDataID>{1F2D6ACC-8E30-4011-BD27-059BB9AF6EDE}</MetaDataID>
        public RoleAMultiplicityRangeAttribute(ulong lowLimit)
        {
            Multiplicity = new MultiplicityRange(lowLimit);
        }

        /// <MetaDataID>{ADCB0966-DBD7-48A0-80BB-C5FCECED1F03}</MetaDataID>
        public RoleAMultiplicityRangeAttribute(ulong lowLimit, ulong highLimit)
        {
            Multiplicity = new MultiplicityRange(lowLimit, highLimit);
        }

        /// <MetaDataID>{CBC60692-5D7C-40F2-A120-CA9D00482AED}</MetaDataID>
        public RoleAMultiplicityRangeAttribute()
        {
            Multiplicity = new MultiplicityRange();
        }

        /// <MetaDataID>{9DDD2034-1A1C-4CF1-9F8A-12D76D166B31}</MetaDataID>
        public readonly MultiplicityRange Multiplicity;
    }

    /// <MetaDataID>{3754EA18-7873-4742-8F4E-C9CB4B169CD6}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class RoleBMultiplicityRangeAttribute : System.Attribute
    {

        /// <MetaDataID>{FDD22006-86DF-449E-A7E1-2F7DE43BD01E}</MetaDataID>
        public RoleBMultiplicityRangeAttribute(ulong lowLimit)
        {
            Multiplicity = new MultiplicityRange(lowLimit);
        }

        /// <MetaDataID>{1DCC55AC-17A9-4C64-8800-AD159D1D224D}</MetaDataID>
        public RoleBMultiplicityRangeAttribute(ulong lowLimit, ulong highLimit)
        {
            Multiplicity = new MultiplicityRange(lowLimit, highLimit);
        }

        /// <MetaDataID>{BBF122CA-0DFE-4B70-A6DB-1516EFA0B270}</MetaDataID>
        public RoleBMultiplicityRangeAttribute()
        {
            Multiplicity = new MultiplicityRange();
        }

        /// <MetaDataID>{804870CC-C1C6-4DFF-BC69-64CB3956CEE2}</MetaDataID>
        public readonly MultiplicityRange Multiplicity;
    }


    /// <MetaDataID>{4346a41d-b723-400c-b155-6fbd12154791}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class IgnoreErrorCheckAttribute : System.Attribute
    {

    }





    /// <summary>
    /// </summary>
    /// <MetaDataID>{D702F5D7-1F84-4432-88CB-31111ADA7E2B}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class AssociationAttribute : System.Attribute
    {
        /// <MetaDataID>{A05BEA12-3F02-46D9-AB74-389626008A44}</MetaDataID>
        public string AssociationName;
        /// <MetaDataID>{e7544369-fd31-4f78-9f3a-83be401efa1b}</MetaDataID>
        public string Identity;
        /// <MetaDataID>{e4bc6a02-9b43-4af1-8111-8fe6b4e099a8}</MetaDataID>
        public MetaDataRepository.Roles Role;

        /// <MetaDataID>{cb6ad89a-26cd-41f2-ab23-9b55ed51cf58}</MetaDataID>
        public bool Indexer;
        // public System.Type GeneralAssociationHostType;
        /// <MetaDataID>{28df2db6-79e8-47a7-88b0-97b942879a89}</MetaDataID>
        public string GeneralAssociationIdentity;

        /// <MetaDataID>{bce09850-1226-4740-abed-4d6011d23ea7}</MetaDataID>

        public AssociationAttribute(string associationName, System.Type otherEndType, Roles role, bool indexer, string identity)
            : this(associationName, otherEndType, role, indexer, identity, null)
        {

        }

        /// <MetaDataID>{b0387aab-646e-4823-9261-ed3c3d3e6aca}</MetaDataID>
        public AssociationAttribute(string associationName, Roles role, bool indexer, string identity)
            : this(associationName, null, role, indexer, identity, null)
        {
        }
        /// <MetaDataID>{e7265914-f94c-487b-9aed-5de3a1a2a1c6}</MetaDataID>
        public AssociationAttribute(string associationName, System.Type otherEndType, Roles role, bool indexer, string identity/*,System.Type generalAssociationHostType*/, string generalAssociationIdentity)
        {
            //            GeneralAssociationHostType = generalAssociationHostType;
            GeneralAssociationIdentity = generalAssociationIdentity;

            Indexer = indexer;
            if (identity != null)
            {
                Identity = identity;

                if (Identity != null)
                    Identity = Identity.Trim();
            }

            AssociationName = associationName;
            if (AssociationName != null)
                AssociationName = AssociationName.Trim();

            Role = role;
            _OtherEndType = otherEndType;
            if (role == Roles.RoleA)
                IsRoleA = true;
            else
                IsRoleA = false;
        }


        /// <MetaDataID>{638d4245-bc9e-406e-a1ea-0eb35ee5005f}</MetaDataID>
        public AssociationAttribute(string associationName, System.Type otherEndType, Roles role, bool indexer)
            : this(associationName, otherEndType, role, indexer, null, null)
        {

        }
        /// <MetaDataID>{f82ab685-13c5-4aae-8ebd-9e53f5bd251d}</MetaDataID>
        public AssociationAttribute(string associationName, Roles role, bool indexer)
            : this(associationName, null, role, indexer, null, null)
        {

        }

        /// <MetaDataID>{d008f3dc-6a75-4875-beb3-f058be3bd8b3}</MetaDataID>
        public AssociationAttribute(string associationName, System.Type otherEndType, Roles role, string identity)
            : this(associationName, otherEndType, role, false, identity, null)
        {
            //Identity=identity;
            //AssociationName=associationName;
            //if(Identity!=null)
            //    Identity=Identity.Trim();
            //if(AssociationName!=null)
            //    AssociationName=AssociationName.Trim();

            //Role=role;
            //OtherEndType=otherEndType;
            //if(role==Roles.RoleA)
            //    IsRoleA=true;
            //else
            //    IsRoleA=false;
        }
        /// <MetaDataID>{68dba813-4475-439c-9d4f-25cad7e85b0b}</MetaDataID>
        public AssociationAttribute(string associationName, Roles role, string identity)
            : this(associationName, null, role, false, identity, null)
        {
        }



        /// <MetaDataID>{573112b1-0d45-41cf-9e37-97c25b217429}</MetaDataID>
        public AssociationAttribute(string associationName, System.Type otherEndType, Roles role, string identity, string generalAssociationIdentity)
            : this(associationName, otherEndType, role, false, identity, generalAssociationIdentity)
        {
            //Identity=identity;
            //AssociationName=associationName;
            //if(Identity!=null)
            //    Identity=Identity.Trim();
            //if(AssociationName!=null)
            //    AssociationName=AssociationName.Trim();

            //Role=role;
            //OtherEndType=otherEndType;
            //if(role==Roles.RoleA)
            //    IsRoleA=true;
            //else
            //    IsRoleA=false;
        }
        /// <MetaDataID>{290587f9-8834-4b03-b19b-9ae408d42a2e}</MetaDataID>
        public AssociationAttribute(string associationName, Roles role, string identity, string generalAssociationIdentity)
            : this(associationName, null, role, false, identity, generalAssociationIdentity)
        {
        }


        ///// <MetaDataID>{BD71B2B0-2BA6-4186-BAF7-BC8EE4162186}</MetaDataID>
        //public AssociationAttribute(string associationName,System.Type otherEndType,Roles role):this(associationName,otherEndType,role,false,null,null)
        //{

        //    //AssociationName=associationName;
        //    //if(AssociationName!=null)
        //    //    AssociationName=AssociationName.Trim();

        //    //OtherEndType=otherEndType;
        //    //Role=role;
        //    //if(role==Roles.RoleA)
        //    //    IsRoleA=true;
        //    //else
        //    //    IsRoleA=false;
        //} 
        /// <MetaDataID>{FEB2B18C-09D1-4EBA-AF50-DD19C4B889C5}</MetaDataID>
        public bool IsRoleA = true;
        /// <MetaDataID>{245ff58a-26c8-409c-81e9-7aef284ed923}</MetaDataID>
        public System.Type _OtherEndType;
        /// <MetaDataID>{377BD041-5415-41BB-8FD9-331FFAB736FC}</MetaDataID>
        public System.Type OtherEndType
        {
            get
            {
                if (_OtherEndType == null)
                    throw new System.Exception("OtherEndType==null");
                return _OtherEndType;
            }
            set
            {
                _OtherEndType = value;
            }
        }
    }
}
