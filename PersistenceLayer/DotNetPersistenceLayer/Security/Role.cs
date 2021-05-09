using OOAdvantech.MetaDataRepository;
namespace OOAdvantech.Security
{
    /// <MetaDataID>{e8daa830-eea9-4290-a374-31f20cdd1010}</MetaDataID>
     
    [BackwardCompatibilityID("{e8daa830-eea9-4290-a374-31f20cdd1010}")]
    [Persistent()]
    public class Role : OOAdvantech.Security.Subject
    {
        
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<User> _AssignedUsers=new OOAdvantech.Collections.Generic.Set<User>();
        [Association("UserRole", typeof(User), MetaDataRepository.Roles.RoleB, "a370ecad-f38b-472a-bc05-6b88f44c1000")]
        [PersistentMember("_AssignedUsers")]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<User> AssignedUsers
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<User>(_AssignedUsers,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        
        }
        /// <exclude>Excluded</exclude>
        bool _IsOSGroup;
        /// <MetaDataID>{f11b7fa3-1b8d-45cc-a348-a93fca743aa4}</MetaDataID>
        [PersistentMember("_IsOSGroup")]
        [BackwardCompatibilityID("+1")]
        public bool IsOSGroup
        {
            get
            {
                return default(bool);
            }
            set
            {
            }
        }

    }
}
