using OOAdvantech.MetaDataRepository;
namespace OOAdvantech.Security
{
    /// <MetaDataID>{6737b1f2-42e5-42fc-bdf9-1a8230d6f57e}</MetaDataID>
    [BackwardCompatibilityID("{6737b1f2-42e5-42fc-bdf9-1a8230d6f57e}")]
    [MetaDataRepository.Persistent()]
    public class User : OOAdvantech.Security.Subject
    {
        /// <exclude>Excluded</exclude>
        Collections.Generic.Set<Role> _AssignedRoles = new OOAdvantech.Collections.Generic.Set<Role>();
        [Association("UserRole", typeof(Role), MetaDataRepository.Roles.RoleA, "a370ecad-f38b-472a-bc05-6b88f44c1000")]
        [PersistentMember("_AssignedRoles")]
        [RoleAMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<Role> AssignedRoles
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<Role>(_AssignedRoles);
            }
      
        }
        /// <exclude>Excluded</exclude>
        bool _IsOSUser;
        /// <MetaDataID>{2fb3bfb7-7d71-4b35-b2db-6508ae01c84b}</MetaDataID>
        [PersistentMember("_IsOSUser"), BackwardCompatibilityID("+2")]
        public bool IsOSUser
        {
            get
            {
                return _IsOSUser;
            }
            set
            {
            }
        }

        /// <exclude>Excluded</exclude>
        string _Password;
        /// <MetaDataID>{f471d828-21d0-4604-89f9-ca86f450c716}</MetaDataID>
        [PersistentMember("_Password")]
        [MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
            }
        }
    }
}
