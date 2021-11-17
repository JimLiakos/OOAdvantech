using System;

namespace OOAdvantech.MetaDataRepository
{
    /// <summary>
    /// </summary>
    /// <MetaDataID>{A4901CDE-7662-4C48-BCEF-9C7C852A7517}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class BuildAssemblyMetadata : System.Attribute
    {
        public string MappingVersion;
        /// <MetaDataID>{AA7F9D74-BD15-492A-9669-3DEC12A4EEB8}</MetaDataID>
        public BuildAssemblyMetadata(string mappingVersion = "")
        {
            MappingVersion = mappingVersion;
            // 
            // TODO: Add constructor logic here
            //
        }
    }

  
}
