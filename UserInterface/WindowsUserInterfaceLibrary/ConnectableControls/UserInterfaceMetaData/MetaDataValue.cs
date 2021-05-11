using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterfaceMetaData
{
    /// <MetaDataID>{3e2a6f92-3905-41bb-b87c-95cc2c78dd61}</MetaDataID>
    [Serializable]
    public class MetaDataValue
    {

        virtual public string XMLMetaData
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
        public MetaDataValue(string metaDataAsXMLString, string description)
        {
            MetaDataAsXML = metaDataAsXMLString;
            Descripion = description;
        } 
        System.Exception Error;
        public MetaDataValue(System.Exception error)
        {
            Error = error;
        }
        /// <MetaDataID>{CB2B6C91-883F-47AE-90F2-8FE52A2679B0}</MetaDataID>
        public MetaDataValue(byte[] metaDataAsBinary, string description)
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
