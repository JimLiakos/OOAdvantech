using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{11b9b029-c490-46f8-b1bc-3e72c31a54e8}</MetaDataID>
    internal interface ILINQObjectQuery
    {
        /// <MetaDataID>{6151103a-871e-49c5-a7f6-d6f747d1d7e0}</MetaDataID>
        [Association("", Roles.RoleA, "12026e34-ac3e-4f13-b80e-39194cee6f20")]
        IDynamicTypeDataRetrieve QueryResult
        {
            set;
            get;
        }

        /// <MetaDataID>{434c0dc3-0076-4ee9-a4ed-97a5ecc40cc6}</MetaDataID>
        OOAdvantech.Linq.Translators.QueryTranslator Translator { get; }
        /// <MetaDataID>{0cb4b218-f759-4b70-b55f-6c8d88802a3d}</MetaDataID>
        //IDynamicTypeDataRetrieve SearchResult
        //{
        //    get;
        //    set;
        //}


        void AddSelectListItem(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode MemberDataNode);


        /// <MetaDataID>{22ddb592-1ecd-4ab6-bd95-a697b3eb5f98}</MetaDataID>
        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery ObjectQuery { get; }
    }
}
