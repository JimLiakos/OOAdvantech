using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{c40b7f55-baa6-4aa3-bc82-6bd54f86748d}</MetaDataID>
    internal class RoseModelItem
    {
        internal RationalRose.RoseItem RoseItem;
        public RoseModelItem(RationalRose.RoseItem roseItem)
        {
            RoseItem = roseItem;
        }
        public string Identity
        {
            get
            {
                return RoseItem.GetPropertyValue("MetaData", "MetaObjectID");
            }
            set
            {
                RoseItem.OverrideProperty("MetaData", "MetaObjectID",value);
            }
        }
    }
}
