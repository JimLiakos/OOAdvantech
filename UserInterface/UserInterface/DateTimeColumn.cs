namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;



    /// <summary>
    ///     Specifies the date and time format 
    ///     control displays.
    /// </summary>
    /// <MetaDataID>{06ffe492-4cd9-4b47-b919-87f2fca81c52}</MetaDataID>
    public enum DateTimePickerFormat
    {
        
        /// <summary>
        ///     The DateTime control displays the date/time value
        ///     in the long date format set by the user's operating system.
        /// </summary>
        Long = 1,

        /// <summary>
        ///     The DateTime control displays the date/time value
        ///     in the short date format set by the user's operating system.
        /// </summary>
        Short = 2,
        /// <summary>
        ///     The DateTime control displays the date/time value
        ///     in the time format set by the user's operating system.
        /// </summary>
        Time = 4,
        /// <summary>
        ///     The DateTime control displays the date/time value
        ///     in a custom format.
        /// </summary>
        Custom = 8,
    }

    /// <MetaDataID>{E38BEE74-85DE-4D5B-9511-6A8FB882ECBB}</MetaDataID>
    public class DateTimeColumn : Column
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E2FFDA11-C477-4FBD-8EEB-A8CBD5CB0C11}</MetaDataID>
        private string _CustomDateTimeFormat;
        /// <MetaDataID>{E708122C-77E0-4574-BC2D-1389B624B1E7}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_CustomDateTimeFormat")]
        public string CustomDateTimeFormat
        {
            get
            {
                return _CustomDateTimeFormat;
            }
            set
            {
                if (_CustomDateTimeFormat != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CustomDateTimeFormat = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{20DECC0C-1BA7-49CE-8587-4DFAD1C15807}</MetaDataID>
        private DateTimePickerFormat _DateTimeFormat;
        /// <MetaDataID>{D687E902-331D-43F7-9724-BA3F3F4C7C5F}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_DateTimeFormat")]
        public DateTimePickerFormat DateTimeFormat
        {
            get
            {
                return _DateTimeFormat;
            }
            set
            {
                if (_DateTimeFormat != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DateTimeFormat = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{36532550-3E4A-43D6-A766-984E84204D71}</MetaDataID>
        public DateTimeColumn(Column copyColumn)
            : base(copyColumn)
        {

        }
        /// <MetaDataID>{AFADA4CD-0E58-4537-BC5E-54F2F9A9264F}</MetaDataID>
        public DateTimeColumn()
        {
        }
    }
}
