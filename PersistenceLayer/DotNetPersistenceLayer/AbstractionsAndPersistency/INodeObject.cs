using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;

    /// <MetaDataID>{d2a589b0-b60b-4cbf-8ce3-b2cfc11b9579}</MetaDataID>
    [BackwardCompatibilityID("{d2a589b0-b60b-4cbf-8ce3-b2cfc11b9579}")]
    public interface INodeObject
    {
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{4150d429-228c-4ef2-b08c-11969ba901c6}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<INodeObject> SubNodeObjects
        {
            get;
        }
        /// <MetaDataID>{f4a6626b-c9f9-46d4-b06d-0769676ecf54}</MetaDataID>
        string Name
        {
            get;
            set;
        }
       
    }
}
