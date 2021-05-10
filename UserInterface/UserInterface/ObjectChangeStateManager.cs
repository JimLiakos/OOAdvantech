using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{156dce09-c4d4-4031-b97d-7cbf02f88682}</MetaDataID>
    internal class ObjectChangeStateManager
    {
        /// <MetaDataID>{8fb107ef-bd0f-46db-a40e-7bcc5f339e96}</MetaDataID>
        internal readonly IPathDataDisplayer PathDataDisplayer;
        /// <MetaDataID>{043f7570-fca5-4c25-8b36-19c0d0eb3e4d}</MetaDataID>
        public ObjectChangeStateManager(IPathDataDisplayer pathDataDisplayer)
        {
            
            PathDataDisplayer  = pathDataDisplayer; 
        }
        /// <MetaDataID>{effa7bc5-c151-42af-b492-2a3c5ad1933c}</MetaDataID>
        OOAdvantech.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.Member> DataPathNodes = new OOAdvantech.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.Member>();

        /// <MetaDataID>{1de7584e-e9a3-43fd-852a-515d162e7dfc}</MetaDataID>
        public void ReleaseDataPathNodes()
        {
            foreach (Member member in DataPathNodes)
            {
                member.RemovePathDataDisplayer(this);
                member.Change -= new MemberChangeHandler(OnDisplayedValueChanged);
                member.LockStateChange -= new MemberLockStateChangeHandler(OnMemberLockStateChange);
            }
            DataPathNodes.Clear();
        }
        /// <MetaDataID>{0330c96e-f6be-46e0-8b8c-6eba0fd26a43}</MetaDataID>
        public void RemoveDataPathNode(OOAdvantech.UserInterface.Runtime.Member member)
        {
            DataPathNodes.Remove(member);
        }
        /// <MetaDataID>{21c4a570-4858-403c-959a-1814b1546ede}</MetaDataID>
        public void AddDataPathNode(OOAdvantech.UserInterface.Runtime.Member member)
        {
            if (!DataPathNodes.Contains(member))
            {
                DataPathNodes.Add(member);
                member.Change += new MemberChangeHandler(OnDisplayedValueChanged);
                member.LockStateChange += new MemberLockStateChangeHandler(OnMemberLockStateChange);
            }
        }

        /// <MetaDataID>{e9494c06-ed58-49b2-bddc-22b5b99b6f8c}</MetaDataID>
        void OnMemberLockStateChange(object sender)
        {
            PathDataDisplayer.LockStateChange(sender);
            
        }

        /// <MetaDataID>{c51d34d6-3729-4897-8332-aafd6f227ff7}</MetaDataID>
        void OnDisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            PathDataDisplayer.DisplayedValueChanged(sender, change);
        }
      


        
    }
}
