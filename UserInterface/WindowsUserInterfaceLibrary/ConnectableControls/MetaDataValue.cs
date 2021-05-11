using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls
{

    /// <MetaDataID>{33723bd8-86ef-4ca7-8297-20c011ade382}</MetaDataID>
    [Serializable]
    public class MetaDataValue:UserInterfaceMetaData.MetaDataValue
    {

        public override string XMLMetaData
        {
            get
            {
                return MetaDataAsXML;
            }
            set
            {
                MetaDataAsXML = value;
            }
        }        
  
        string MetaDataAsXML;
        public byte[] MetaDataAsBinary;
        String Descripion;
        /// <MetaDataID>{97F70125-2431-455F-84B2-7B8B5BBC67D3}</MetaDataID>
        private MetaDataValue(string metaDataAsXMLString, string description):base(metaDataAsXMLString,description)
        {
            MetaDataAsXML = metaDataAsXMLString;
            Descripion = description;
        }
        System.Exception Error;
        private MetaDataValue(System.Exception error):base(error)
        {
            Error = error;
        }
        /// <MetaDataID>{CB2B6C91-883F-47AE-90F2-8FE52A2679B0}</MetaDataID>
        private MetaDataValue(byte[] metaDataAsBinary, string description):base(metaDataAsBinary,description)
        {
            Descripion = description;
            MetaDataAsBinary = metaDataAsBinary;
        }

        [NonSerialized]
        public object MetaDataAsObject;

        /// <MetaDataID>{4FBCADA2-64BC-48DB-868B-3DD2CA936D6B}</MetaDataID>
        public override string ToString()
        {
            if (Error != null)
                return Error.ToString();
            if (!string.IsNullOrEmpty(Descripion))
                return Descripion;
            return "(MetaData)";
        }
    }
   

}
