namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    /// <MetaDataID>{F2C5B0A6-0F1E-41C7-9D0B-1C5E70B037F9}</MetaDataID>
    [BackwardCompatibilityID("{F2C5B0A6-0F1E-41C7-9D0B-1C5E70B037F9}")]
    [Persistent()]
    public class ParameterLoader
    {
        /// <exclude>Excluded</exclude>
        private bool _RefreshFrom;

        /// <summary>When is true the user interface subsystem refresh control that display values which are related with the object of parameter. The refresh action happens after the operation call and in transaction of the form.</summary>
        /// <MetaDataID>{cd3ac52a-d475-46f6-993f-605a7eb2f56b}</MetaDataID>
        [PersistentMember("_RefreshFrom")]
        [BackwardCompatibilityID("+7")]
        public bool RefreshFrom
        {
            get
            {
                return _RefreshFrom;
            }
            set
            {
                if (_RefreshFrom != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RefreshFrom = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4883C847-A9B4-49A7-A163-B8FB2DA5E4E9}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{F6100617-B1AD-4B53-8D4B-EBCC0B32419A}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6AE58AE1-6A39-4EEA-8F54-D748D3CDBA8D}</MetaDataID>
        private short _Position = 0;
        /// <MetaDataID>{D3A57AA5-BAE4-4D0E-A737-59D481BA5907}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember("_Position")]
        public short Position
        {
            get
            {
                return _Position;
            }
            set
            {
                ///TODO θα πρέπει να καταργηθεί το position και να χρησιμοποιηθεί indexer association
                if (_Position != value)
                {
                    if (OOAdvantech.Transactions.ObjectStateTransition.GetTransaction(this).Count > 0)
                        _Position = value;
                    else
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _Position = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DF2ECBB6-B809-43F1-AF37-92C13E22F0B3}</MetaDataID>
        private string _ParameterType;
        /// <MetaDataID>{AF22CD14-B799-4646-ABB9-1FBEA79B71F7}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_ParameterType")]
        public string ParameterType
        {
            get
            {
                return _ParameterType;
            }
            set
            {
                if (_ParameterType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ParameterType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{E4EA26BD-385B-49A7-83F5-FA1294241087}</MetaDataID>
        private ObjectStateManagerLink Properties;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{31610794-AF3E-4F5F-A8BA-3B5E9D0997D5}</MetaDataID>
        private string _Source;

        /// <MetaDataID>{740AAF35-1DF4-498F-98BC-D828F7713551}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Source")]
        public string Source
        {
            get
            {
                return _Source;
            }
            set
            {
                if (_Source != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Source = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
