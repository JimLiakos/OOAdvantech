using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{86c9fe14-ecfc-4515-a0f5-b488cf0fed6b}</MetaDataID>
    public interface IObjectStorage
    {
        /// <MetaDataID>{a976f781-7011-4f55-990e-ff3638ea5b05}</MetaDataID>
        void UnprotectedDelete(object thePersistentObject, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption);
    }
}
