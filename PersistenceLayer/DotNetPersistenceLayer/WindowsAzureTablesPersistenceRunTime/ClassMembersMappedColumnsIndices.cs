using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CreatorIdentity = System.String;
using PartTypeName = System.String;


namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{

    /// <summary>
    /// Has class members mapped columns indices on data table row 
    /// </summary>
    /// <MetaDataID>{a5add89a-2008-4610-b2cc-cb423fe776d6}</MetaDataID>
    internal class ClassMembersMappedColumnsIndices
    {
        /// <summary>
        /// This member keeps dictionaries with attributes indices on data table row. 
        /// </summary>
        /// <MetaDataID>{57ec23d1-34e0-4b07-9749-dcf6b7912117}</MetaDataID>
        public Dictionary<MetaDataRepository.MetaObjectID, Dictionary<string, int>> AttributeIndices;
        /// <summary>
        /// This member keeps dictionaries with association end indices indices on data table row. 
        /// </summary>
        /// <MetaDataID>{b6145795-1181-441e-b09c-45a31cf603d5}</MetaDataID>
        public Dictionary<CreatorIdentity, Dictionary<MetaDataRepository.MetaObjectID, Dictionary<MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>>> AssociationEndIndices;

        ///<summary>
        ///
        ///</summary>
        /// <MetaDataID>{8c0097e8-23e0-4ef3-912f-bf7828d1cbc5}</MetaDataID>
        public Dictionary<CreatorIdentity, Dictionary<MetaDataRepository.MetaObjectID, int>> AssociationEndIndexerColumnIndices = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, int>>();
    }


}
