using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{4d015b0a-53c0-426a-923b-f34859d9725c}</MetaDataID>
    public class AssociationPresentationObject : PresentationObject<Association>
    {
        /// <MetaDataID>{417af99d-34d7-4a92-9605-f2101e91480c}</MetaDataID>
        public AssociationPresentationObject(Association association)
            : base(association)
        {

        }

        /// <MetaDataID>{b21ee88f-7af4-4788-9aeb-fb9d8a0f9fe8}</MetaDataID>
        AssociationEndPresentationObject _RoleA = null;
        /// <MetaDataID>{9096e135-e9a5-4334-8a0b-24b084c6d89c}</MetaDataID>
        public AssociationEndPresentationObject RoleA
        {
            get
            {
                if (_RoleA == null)
                    _RoleA = new AssociationEndPresentationObject(RealObject.RoleA as AssociationEnd);

                return _RoleA;
            }
        }

        /// <MetaDataID>{966debbb-65ce-4301-929b-1195404f8348}</MetaDataID>
        AssociationEndPresentationObject _RoleB = null;
        /// <MetaDataID>{90ee7ea7-63fc-49da-865e-5fd09a7e8830}</MetaDataID>
        public AssociationEndPresentationObject RoleB
        {
            get
            {
           
                if (_RoleB == null)
                    _RoleB = new AssociationEndPresentationObject(RealObject.RoleB as AssociationEnd);
                return _RoleB;
            }
        }

    }
}
