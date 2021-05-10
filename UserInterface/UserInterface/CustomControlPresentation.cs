namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using Transactions;
    /// <MetaDataID>{B78BA593-D49A-48BB-A2A2-14E66E38451F}</MetaDataID>
    [BackwardCompatibilityID("{B78BA593-D49A-48BB-A2A2-14E66E38451F}")]
    [Persistent()]
    public class CustomControlPresentation
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F63A6284-2CD6-4327-A491-883AA2EE55E9}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<PresentationInfo> _PresentationInfos = new OOAdvantech.Collections.Generic.Set<PresentationInfo>();
        /// <MetaDataID>{A38084FC-9AEA-46D5-B092-861B4DF31297}</MetaDataID>
        [Association("CustomPresentation",typeof(OOAdvantech.UserInterface.PresentationInfo),Roles.RoleA,"{0BDE779B-ACA9-454A-86B8-A0B61CD0F83A}")]
        [PersistentMember("_PresentationInfos")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1,1)]
        public OOAdvantech.Collections.Generic.Set<PresentationInfo> PresentationInfos
        {
            get
            { 
                return new OOAdvantech.Collections.Generic.Set<PresentationInfo>(_PresentationInfos, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <MetaDataID>{7B64B0E7-F475-407F-AA03-BEAB76A6FF53}</MetaDataID>
        public void AddPresentationInfo(PresentationInfo presentationInfo)
        {
            if (!_PresentationInfos.Contains(presentationInfo))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _PresentationInfos.Add(presentationInfo);
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{DA992ABE-7459-4CDD-9C7C-0FD5BE1AFA8C}</MetaDataID>
        public void RemovePresentationInfo(PresentationInfo presentationInfo)
        {
            if (_PresentationInfos.Contains(presentationInfo))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _PresentationInfos.Add(presentationInfo);
                    stateTransition.Consistent = true;
                }
            }
        }

        
        /// <MetaDataID>{09ADEBB7-2160-4C35-85A4-19A692C24BA3}</MetaDataID>
        protected ObjectStateManagerLink Properties;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5D91DAB8-E3E4-4D40-BF71-BBA0DB79E84E}</MetaDataID>
        private string _Identity;
        /// <MetaDataID>{43D9F454-5A88-4C45-8173-CB8FCEF3A6B5}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_Identity")]
        public string Identity
        {
            get
            {
                return _Identity;
            }
            set
            {
                if (_Identity != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Identity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
