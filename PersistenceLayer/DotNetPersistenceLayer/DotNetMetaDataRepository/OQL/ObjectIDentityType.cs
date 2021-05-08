using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    public class ObjectIDentityType
    {
    }
    interface IdentityColumn
    {
        string Name
        {
            get;
        }

        string TypeName
        {
            get;
        }

        Type Type
        {
            get;
        }
    }
}
