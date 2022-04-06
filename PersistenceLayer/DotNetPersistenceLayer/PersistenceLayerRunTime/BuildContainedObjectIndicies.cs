using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
    /// <MetaDataID>{2ceca593-2d80-45d7-8d6d-2e47398f3f96}</MetaDataID>
    public abstract class BuildContainedObjectIndicies : Command
    {

        /// <MetaDataID>{471fd5ef-2160-4c43-8710-839c6f865bed}</MetaDataID>
        protected readonly IndexedCollection Collection;

        protected readonly System.Globalization.CultureInfo Culture;

        /// <MetaDataID>{47535a39-c689-468f-9dc0-28b2774f0399}</MetaDataID>
        public BuildContainedObjectIndicies(IndexedCollection collection)
        {
            Collection = collection;

            if (collection.RelResolver.Multilingual)
                Culture = OOAdvantech.CultureContext.CurrentCultureInfo;

        }
        /// <MetaDataID>{6c920464-5e00-44b8-83b7-529253c24bc5}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }

        /// <MetaDataID>{8b6dcfea-d48d-4aa7-8f16-83edd15d60ab}</MetaDataID>
        public override int ExecutionOrder
        {
            get
            {
                return 75;
            }
        }

        /// <MetaDataID>{e432160b-4ff5-47af-aa02-f08b9285bf09}</MetaDataID>
        public static string GetIdentity(IndexedCollection collection)
        {
            string culture = "";
            if (collection.RelResolver.Multilingual)
                culture = OOAdvantech.CultureContext.CurrentCultureInfo.Name;

            return "indexRebuild"+ culture + collection.RelResolver.Owner.MemoryID.ToString() + collection.RelResolver.AssociationEnd.Identity.ToString();
        }
        /// <MetaDataID>{7e5a8365-5b18-493d-b2c5-39466460e240}</MetaDataID>
        public override string Identity
        {
            get
            {
                string culture = "";
                if (this.Culture != null)
                    culture = this.Culture.Name;

                return "indexRebuild" + culture + Collection.RelResolver.Owner.MemoryID.ToString() + Collection.RelResolver.AssociationEnd.Identity.ToString();
            }
        }

    }
}
