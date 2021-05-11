using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.ListView
{
    /// <MetaDataID>{d6a73ad5-f7ac-4a54-8e5a-73978236e57d}</MetaDataID>
    public interface IRow
    {
        object CollectionObject
        {
            get;
        }
        int Index
        {
            get;
            
        }
        object PresentationObject
        {
            get;
        }


    }
}
