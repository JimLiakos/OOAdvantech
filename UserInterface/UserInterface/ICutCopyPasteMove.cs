using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{f8ed07f1-ff1b-42a3-9d10-fecb42350e5e}</MetaDataID>
    public interface ICutCopyPasteMoveDragDrop
    {
        /// <MetaDataID>{c05e4366-9e8c-4361-9c8f-02a41cf0ade4}</MetaDataID>
        void CutObject(object obj);
        /// <MetaDataID>{43dbe45d-4a92-4a76-8629-8a5532cd242d}</MetaDataID>
        void PasteObject(object obj);
        /// <MetaDataID>{79030312-65f1-4298-a4cc-fa6266f51a4f}</MetaDataID>
        UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get;
        }
        

    }
}
