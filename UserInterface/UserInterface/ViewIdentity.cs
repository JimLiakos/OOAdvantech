using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{abee1c9a-e2cb-4cf3-a1c3-4e99c114628a}</MetaDataID>
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewIdentity:System.Attribute
    {
        /// <MetaDataID>{bbdf5203-cdb4-4fef-aa04-03b0dcc008e7}</MetaDataID>
        public readonly string Identity;
        /// <MetaDataID>{22bc4d2c-1003-49af-a650-cf244d923ea0}</MetaDataID>
        public ViewIdentity(string identity)
        {
            Identity = identity;
        }
        
    }
}
