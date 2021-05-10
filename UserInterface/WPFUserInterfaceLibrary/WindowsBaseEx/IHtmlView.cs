using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIBaseEx
{
    /// <MetaDataID>{fa620837-db5f-4ee7-8f1d-b26975dc1183}</MetaDataID>
    public interface IHtmlView
    {
        /// <MetaDataID>{861162d5-97c4-4a6a-b089-3a0f4aa73e3f}</MetaDataID>
        object InvockeJSMethod(string methodName, object[] args, bool _async = false);
    }
}
