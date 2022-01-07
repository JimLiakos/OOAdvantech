using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterfaceTest
{
    /// <MetaDataID>{db057f9d-3323-4f6d-885d-90ced8cd0f6f}</MetaDataID>
    public class FolderPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<AbstractionsAndPersistency.INodeObject>
    {
        AbstractionsAndPersistency.INodeObject NodeObject;
        public FolderPresentation(AbstractionsAndPersistency.INodeObject nodeObject):base(nodeObject)
        {
            NodeObject = nodeObject;

        }
        public String FullName
        {
            get
            {
                if (NodeObject is AbstractionsAndPersistency.Folder)
                    return (NodeObject as AbstractionsAndPersistency.Folder).Directory.FullName;
                else
                    return NodeObject.Name;

            }
        }

    }
}
