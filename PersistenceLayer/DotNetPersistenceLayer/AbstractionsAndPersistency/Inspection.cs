using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
namespace AbstractionsAndPersistency
{
    /// <MetaDataID>{b5b9926b-acb7-470e-8397-8ec6c35a7447}</MetaDataID>
    [BackwardCompatibilityID("{b5b9926b-acb7-470e-8397-8ec6c35a7447}")]
    [Persistent]
    public class Inspection : IInspection
    {
        /// <exclude>Excluded</exclude>
        IOrder _Order;
        /// <MetaDataID>{2ef20440-3b14-4fa7-9416-98daa26c5874}</MetaDataID>
        [PersistentMember("_Order")]
        [BackwardCompatibilityID("+2")]
        public IOrder Order
        {
            get
            {
                return _Order;
            }
            set
            {
                if (_Order == value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Order = value;
                        stateTransition.Consistent = true;
                    }

                }

            }
        }

        /// <exclude>Excluded</exclude>
        private OOAdvantech.ObjectStateManagerLink Properties = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{df711acb-f2a9-496c-a74a-c5a4f5582719}</MetaDataID>
        [BackwardCompatibilityID("+1")]
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









    }
}
