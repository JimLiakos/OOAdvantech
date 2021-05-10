using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{3229e511-71b0-4589-80e7-cb22ba9dad2e}</MetaDataID>
    public interface IOperationCallerSource:IConnectableControl
    {
        /// <MetaDataID>{d76628c2-d2a1-47c1-b233-3139cfcf7e3b}</MetaDataID>
        string[] PropertiesNames
        {
            get;
        }

        /// <MetaDataID>{d02b64cb-eb2c-45ed-a317-15ccfc6326c5}</MetaDataID>
        object GetPropertyValue(string propertyName);

        /// <MetaDataID>{5e7cf9eb-d482-426d-83ba-3f098ab955e9}</MetaDataID>
        void SetPropertyValue(string propertyName, object value);

        /// <MetaDataID>{98ab423f-cd51-4310-99ea-b54277fdc750}</MetaDataID>
        Classifier GetPropertyType(string propertyName);

        /// <MetaDataID>{c6ac12c3-fb73-42ed-8e2b-2b44e27f0b11}</MetaDataID>
        bool ContainsProperty(string propertyName);

    }
}
