namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using Transactions;

    /// <MetaDataID>{DFDC95D0-0D26-4094-9CD3-CCA4376D68F6}</MetaDataID>
    [BackwardCompatibilityID("{DFDC95D0-0D26-4094-9CD3-CCA4376D68F6}")]
    [Persistent()]
    public class MemberAccessControl : ContainerControl
    {


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3D7F3CED-28CA-47FA-95A1-56CCE7A8141B}</MetaDataID>
        private string _MemberPath;
        /// <MetaDataID>{C2EBE39F-7C84-425C-9AA7-785EEEE982EC}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_MemberPath")]
        public string MemberPath
        {
            get
            {
                return _MemberPath;
            }
            set
            {
                if (_MemberPath != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _MemberPath = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
