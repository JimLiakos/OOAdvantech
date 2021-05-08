using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{5bf33746-0dc4-4fa3-aaaf-6cfacd055b15}</MetaDataID>
    [BackwardCompatibilityID("{5bf33746-0dc4-4fa3-aaaf-6cfacd055b15}")]
    [Serializable]
    public class ObjectIdentityType
    {

        /// <MetaDataID>{df82d91b-e982-48b0-b2a8-d3b6b3f59350}</MetaDataID>
        static Dictionary<ObjectIdentityType, ObjectIdentityType> ObjectIdentityTypes = new Dictionary<ObjectIdentityType, ObjectIdentityType>();

        /// <MetaDataID>{e780aedb-f9de-4c8a-b56f-5c7fa2ec9058}</MetaDataID>
        public ObjectIdentityType OriginObjectIdentityType
        {
            get
            {
                ObjectIdentityType originObjectIdentityType = null;
                if (!ObjectIdentityTypes.TryGetValue(this, out originObjectIdentityType))
                {
                    List<IIdentityPart> parts = new List<IIdentityPart>();
                    foreach (var part in _Parts)
                        parts.Add(new IdentityPart(part.PartTypeName,part.PartTypeName,part.Type));
                    originObjectIdentityType=new ObjectIdentityType(parts);
                    ObjectIdentityTypes [originObjectIdentityType]=originObjectIdentityType;

                }
                return originObjectIdentityType;

            }
        }


        public PersistenceLayer.ObjectID Parse(string objectIdentity,string storageIdentity=null)
        {
            var objectIDParts = objectIdentity.Split(',');
            if (objectIDParts.Length != objectIDParts.Length)
                throw new Exception("objectIdentity string has invalid format");

            int i = 0;
            object[] partValues = new object[Parts.Count];
            foreach(var identityPart in Parts)
            {
                if(identityPart.Type==typeof(System.Guid))
                    partValues[i] =new System.Guid(objectIDParts[i]);
                else
                    partValues[i] = Convert.ChangeType(objectIDParts[i], identityPart.Type,new System.Globalization.CultureInfo("en-US"));
                i++;
            }

            return new PersistenceLayer.ObjectID(this, partValues, storageIdentity);


        }

        /// <exclude>Excluded</exclude>
        readonly List<IIdentityPart> _Parts;  
        /// <MetaDataID>{5b6bede3-b255-44bd-9e6e-4ab89d7dd8f9}</MetaDataID>
        ObjectStateManagerLink Properties;
        [Association("IdentityTypeParts", typeof(IIdentityPart), Roles.RoleA, "b378f7ea-7128-4b68-a84a-74005800e124")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public IList<IIdentityPart> Parts
        {
            get
            {
                
                return new ReadOnlyCollection<IIdentityPart>(_Parts);
            }
        }
        /// <MetaDataID>{3c3b4416-a558-46bd-8e19-2ebc9b1d37ff}</MetaDataID>
        public int PartsCount
        {
            get
            {
                return _Parts.Count;
            }
        }

        /// <MetaDataID>{69ea5abb-338c-4098-a3c4-9927a10e7bdc}</MetaDataID>
        protected ObjectIdentityType()
        {
        }

     


        /// <MetaDataID>{f518f0bc-ad2b-42c9-993a-bc02b4f6ac62}</MetaDataID>
        public ObjectIdentityType(List<IIdentityPart> parts)
        {
            _Parts = new List<IIdentityPart>(parts);
        }

        /// <MetaDataID>{979bcc09-3ba4-4db3-97ef-e6b4336ad77b}</MetaDataID>
        public ObjectIdentityType(ObjectIdentityType copyObjectIdentityType)
        {
            _Parts = new List<IIdentityPart>();
            foreach (IIdentityPart part in copyObjectIdentityType.Parts)
                _Parts.Add(new IdentityPart(part));
        }


        //[Association("IdentityTypeParts", typeof(IIdentityPart), Roles.RoleA, "b378f7ea-7128-4b68-a84a-74005800e124")]
        //[RoleAMultiplicityRange(1)]
        //[RoleBMultiplicityRange(0)]
        //public List<IIdentityPart> Parts
        //{
        //    get
        //    {
        //        return new List<IIdentityPart>(_Parts) ;
        //    }
        //}





        /// <exclude>Excluded</exclude>
        public override int GetHashCode()
        {
            if (_Parts == null)
                return GetHashCode();
            int num = -1162279000;
            foreach (IIdentityPart identityPart in _Parts)
                num = (-1521134295 * num) + GetHashCode(identityPart);
            return num;
        }
        /// <exclude>Excluded</exclude>
        private int GetHashCode(IIdentityPart identityPart)
        {
            int num = -1162279000;
            num = (-1521134295 * num) + identityPart.PartTypeName.GetHashCode();
            num = (-1521134295 * num) + identityPart.Type.GetHashCode();
            return num;
        }

        /// <exclude>Excluded</exclude>
        public static bool operator ==(ObjectIdentityType left, ObjectIdentityType right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return true;
            if (object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null))
                return false;
            if (!object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }
        /// <exclude>Excluded</exclude>
        public static bool operator !=(ObjectIdentityType left, ObjectIdentityType right)
        {
            return !(left == right);
        }



        /// <exclude>Excluded</exclude>
        public override bool Equals(object obj)
        {
            if (!(obj is ObjectIdentityType))
                return false;

            if (((ObjectIdentityType)obj)._Parts == null && _Parts == null)
                return true;
            if ((((ObjectIdentityType)obj)._Parts == null || _Parts == null) && ((ObjectIdentityType)obj)._Parts != _Parts)
                return false;

            foreach (IIdentityPart identityPart in ((ObjectIdentityType)obj)._Parts)
            {
                bool retValue = false;

                foreach (IIdentityPart part in _Parts)
                {
                    if (Equals(identityPart, part))
                    {
                        retValue = true;
                        break;
                    }
                    if (!retValue)
                        return false;
                }
            }
            return true;
        }
        /// <exclude>Excluded</exclude>
        bool Equals(IIdentityPart left, IIdentityPart right)
        {
            return left.PartTypeName == right.PartTypeName && left.Type == right.Type;
        }

        internal ObjectIdentityType Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as ObjectIdentityType;

            ObjectIdentityType newObjectIdentityType = new ObjectIdentityType(this);
            clonedObjects[this] = newObjectIdentityType;
            return newObjectIdentityType; 
        }
    }
    /// <MetaDataID>{ce12d5fe-f082-4e69-a61c-79f144b508c0}</MetaDataID>
    public interface IIdentityPart
    {
        /// <MetaDataID>{011d49a7-5eaf-4248-b89e-ae0e25f6f621}</MetaDataID>
        string Name
        {
            get;
        }

        /// <MetaDataID>{44a5fe53-9085-4aa2-badf-789d12881329}</MetaDataID>
        string Alias
        {
            get;
        }

        /// <MetaDataID>{2a85e3fd-9ba2-428f-9cb3-7468e63fd022}</MetaDataID>
        string PartTypeName
        {
            get;
        }
        /// <MetaDataID>{c1d896de-75c4-4115-8a3a-78fb6acab126}</MetaDataID>
        Type Type
        {
            get;
        }
    }


    /// <MetaDataID>{9b7269da-d891-4e3b-a9ed-b54437d70249}</MetaDataID>
    [BackwardCompatibilityID("{9b7269da-d891-4e3b-a9ed-b54437d70249}")]
    [Persistent]
    [Serializable]
    public class IdentityPart : IIdentityPart
    {

        /// <MetaDataID>{02b040d9-7fcb-4b3a-8781-5101f2e9a201}</MetaDataID>
        ObjectStateManagerLink Properties;
        /// <MetaDataID>{523d9ce8-b98d-4ba3-b3c7-37a9e502c910}</MetaDataID>
        protected IdentityPart()
        {
        }
        /// <MetaDataID>{f6ef7030-726b-47b1-a60d-e90d55a35add}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember("_TypeFullName")]
        private string TypeFullName;

        /// <exclude>Excluded</exclude>
        string _Alias;
  
        /// <MetaDataID>{cacd4dd6-91c9-4477-97ad-d871ba7356a2}</MetaDataID>
        [PersistentMember("_Alias"), BackwardCompatibilityID("+1")]
        public string Alias
        {
            get
            {
                return _Alias;
            }
            set
            {
                _Alias = value;
            }
        }
        /// <MetaDataID>{01e73893-4483-4b4a-9417-c57c433e9e57}</MetaDataID>
        public IdentityPart(string name, string typeName, System.Type type)
        {
            _Name = name;
            _PartTypeName = typeName;
            _Type = type;
            TypeFullName = _Type.FullName;
        }

        /// <MetaDataID>{232e5a26-1a24-4fb3-92c1-dab15b40926c}</MetaDataID>
        public IdentityPart(IIdentityPart copyIdentityPart)
        {
            _Name = copyIdentityPart.Name;
            _PartTypeName = copyIdentityPart.PartTypeName;
            _Type = copyIdentityPart.Type;
            TypeFullName = _Type.FullName;
        }

     

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{6c3996a4-0fcd-412c-abbf-2463601f6991}</MetaDataID>
        [PersistentMember("_Name")]
        public string Name
        {
            set
            {
            
            }
            get
            {
                return _Name;
            }
        }

        /// <exclude>Excluded</exclude>
        string _PartTypeName;
        /// <MetaDataID>{14862a13-1ab0-4a73-8dbf-57b865d058cf}</MetaDataID>
        [PersistentMember("_PartTypeName")]
        public string PartTypeName
        {
            set
            {
            
            }
            get
            {
                return _PartTypeName;
            }
        }

        /// <exclude>Excluded</exclude>
        Type _Type;
        /// <MetaDataID>{9ddac621-509c-4a9a-86e3-8940c2eee33b}</MetaDataID>
        public System.Type Type
        {
            set
            {
            
            }
            get
            {
                if (_Type == null && !String.IsNullOrEmpty(TypeFullName))
                    return ModulePublisher.ClassRepository.GetType(TypeFullName,"");
                return _Type;

            }
        }
    }
}
