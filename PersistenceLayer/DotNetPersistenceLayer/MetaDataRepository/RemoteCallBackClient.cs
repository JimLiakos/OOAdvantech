using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{fa53520a-4515-49d1-b8b1-9d5b92d0e667}</MetaDataID>
    [AttributeUsage(AttributeTargets.Event|AttributeTargets.Method)]
    public class Asynchronous : System.Attribute
    {
        
    }

    /// <MetaDataID>{90369f2d-dec0-49e8-98fe-23c016b99322}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowEventCallAsynchronousAttribute : System.Attribute
    {

    }
}
