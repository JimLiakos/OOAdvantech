namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using OOAdvantech;
    /// <MetaDataID>{23713321-F4ED-46BD-9BC2-095502CE23D6}</MetaDataID>
    [BackwardCompatibilityID("{23713321-F4ED-46BD-9BC2-095502CE23D6}"), Persistent()]
    public class Column
    {

        /// <exclude>Excluded</exclude> 
        ColumnEditor _Editor;
        [Association("EditColumn", typeof(UserInterface.ColumnEditor), Roles.RoleA, "b2a926a7-b669-4f09-a66a-46f7c05285f0")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Editor")]
        [RoleAMultiplicityRange(0, 1)]
        public OOAdvantech.UserInterface.ColumnEditor Editor
        {
            get
            {
                return _Editor;
            }
            set
            {
                if (_Editor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Editor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{2960A4FB-5EBA-4C54-9C4F-34B9A2947E43}</MetaDataID>
        public Column(Column copyColumn)
        {
            this.Alignment = copyColumn.Alignment;
            this.ConnectedObjectAutoUpdate= copyColumn.ConnectedObjectAutoUpdate;
            this.DisplayMember= copyColumn.DisplayMember;
            this.Editable = copyColumn.Editable;
            this.Enabled = copyColumn.Enabled;
            this.Format = copyColumn.Format;

            this.Menu= copyColumn.Menu;
            this.Name = copyColumn.Name;
            this.Path= copyColumn.Path;
            this.Position = copyColumn.Position;
            this.Selectable = copyColumn.Selectable;
            this.Sortable = copyColumn.Sortable;
            this.Text = copyColumn.Text;
            this.ToolTipText = copyColumn.ToolTipText;
            this.Visible = copyColumn.Visible;
            this.Width = copyColumn.Width;
            


        }
        /// <MetaDataID>{736F48F6-D183-4994-A536-F5C55F3ECBCC}</MetaDataID>
        protected Column()
        {
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1748C408-7823-4A92-AF82-C53C751AD810}</MetaDataID>
        private short _Position;
        /// <MetaDataID>{CFC83F9C-65CD-4215-A8EF-5C560AFF7E34}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        [PersistentMember("_Position")]
        public short Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Position = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{74C27FCE-AFB9-49F9-8C22-CF4C599EA600}</MetaDataID>
        private MenuCommand _Menu;
        /// <MetaDataID>{6E305DA4-1CB2-4449-8AED-33847DA98227}</MetaDataID>
        [Association("ColumnMenu", typeof(UserInterface.MenuCommand), Roles.RoleA, "{83C27B79-979E-4254-91E1-99A2030155F9}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Menu")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.UserInterface.MenuCommand Menu
        {
            get
            {
                if (_Menu == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Menu = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(MenuCommand)) as MenuCommand;
                        _Menu.Name = "ColumnMainMenu";
                        stateTransition.Consistent = true;
                    }
                }
                return _Menu;

            }
            set
            {
                if (_Menu != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Menu = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5E14007E-6849-4B32-A802-CB59C0D68FFA}</MetaDataID>
        private string _Path;
        /// <MetaDataID>{BDDF02E4-DF00-495E-803C-FBEB127EA80E}</MetaDataID>
        [BackwardCompatibilityID("+14")]
        [PersistentMember("_Path")]
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (_Path != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Path = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E3FE5776-8B71-4761-84B9-96A2FDAD7E37}</MetaDataID>
        private string _DisplayMember;
        /// <MetaDataID>{17BB29CE-9D39-43F0-939D-FDBA3B3C903B}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        [PersistentMember("_DisplayMember")]
        public string DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                if (_DisplayMember != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DisplayMember = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6CCEE722-5898-482F-9AF0-FD3DE9B60BA5}</MetaDataID>
        private bool _ConnectedObjectAutoUpdate;
        /// <MetaDataID>{67C9CB56-37A9-420D-A1E8-749CC30B8D7E}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        [PersistentMember("_ConnectedObjectAutoUpdate")]
        public bool ConnectedObjectAutoUpdate
        {
            get
            {
                return _ConnectedObjectAutoUpdate;
            }
            set
            {
                if (_ConnectedObjectAutoUpdate != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ConnectedObjectAutoUpdate = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A52F8357-A352-40DF-AA7F-070D68689737}</MetaDataID>
        private bool _Visible = true;
        /// <summary>Gets or sets the whether the Column is displayed </summary>
        /// <MetaDataID>{2EB9FF3C-D6DC-4A8B-AC21-44FE0865D553}</MetaDataID>
        [BackwardCompatibilityID("+17")]
        [PersistentMember("_Visible")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                if (_Visible != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Visible = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{05A9A389-AC59-4580-BAD1-3CD6B8906539}</MetaDataID>
        private int _Width = 75;
        /// <summary>Gets or sets the width of the Column </summary>
        /// <MetaDataID>{76C2EB29-1042-40C7-8093-0EEC80A3A840}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember("_Width")]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (_Width != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{AE676FB1-5146-4444-BC3F-98CD5CA2F379}</MetaDataID>
        private string _Text;
        /// <summary>Gets or sets the text displayed on the Column header </summary>
        /// <MetaDataID>{9C1F9C8C-1144-4E82-8824-F88EC0F81AB5}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [PersistentMember("_Text")]
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (_Text != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Text = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        
        ///// <exclude>Excluded</exclude>
        //private string _Editor;
        ///// <summary>Gets or sets the text displayed on the Column header </summary>
        ///// <MetaDataID>{CC4A19DF-C8CF-4ea0-87AE-A7A5AC860A19}</MetaDataID>
        //[BackwardCompatibilityID("+18")]
        //[PersistentMember("_Editor")]
        //public string Editor
        //{
        //    get
        //    {
        //        return _Editor;
        //    }
        //    set
        //    {
        //        if (_Editor != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Editor = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3730582B-EE23-4BC0-8562-CFB4765EC1D8}</MetaDataID>
        private string _ToolTipText;

        /// <summary>Gets or sets the ToolTip text associated with the Column </summary>
        /// <MetaDataID>{8C700B81-BEF7-4A33-B400-C801204AD52C}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        [PersistentMember("_ToolTipText")]
        public string ToolTipText
        {
            get
            {
                return _ToolTipText;
            }
            set
            {
                if (_ToolTipText != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ToolTipText = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{50CB6B54-3879-4CEC-B852-60F43DA1E462}</MetaDataID>
        private bool _Sortable=true;
        /// <summary>Gets or sets whether the Column is able to be sorted </summary>
        /// <MetaDataID>{D4E164EB-A8F1-48E1-AA90-69C2B080FEBE}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        [PersistentMember("_Sortable")]
        public bool Sortable
        {
            get
            {
                return _Sortable;
            }
            set
            {
                if (_Sortable != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Sortable = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7BF5DA3F-2DD4-44C9-9005-13D3BABF56B0}</MetaDataID>
        private bool _Selectable=true;

        /// <summary>Gets or sets a value indicating whether the Column's Cells can be selected </summary>
        /// <MetaDataID>{ECD40DCC-58EA-4A0D-8AC8-F02C1A4E50A0}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember("_Selectable")]
        public bool Selectable
        {
            get
            {
                return _Selectable;
            }
            set
            {
                if (_Selectable != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Selectable = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9C15DCC8-248D-4416-9A26-641EE8385B5E}</MetaDataID>
        private string _Format;
        /// <summary>Gets or sets the string that specifies how a Column's Cell contents 
        /// are formatted </summary>
        /// <MetaDataID>{893A0373-8E57-4BE3-A807-6797567E57F5}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_Format")]
        public string Format
        {
            get
            {
                return _Format;
            }
            set
            {
                if (_Format != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Format = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6CB151D7-F6F5-4D47-B58A-A43D6E5B1E18}</MetaDataID>
        private bool _Enabled = true;
        /// <MetaDataID>{016DF483-401E-4D3E-9DA6-BB71445D35FA}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember("_Enabled")]
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                if (_Enabled != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Enabled = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{269ADBB0-968F-40C2-816E-99373B2DC2DA}</MetaDataID>
        private bool _Editable = false;

        /// <summary>Gets or sets a value indicating whether the Column's Cells contents 
        /// are able to be edited </summary>
        /// <MetaDataID>{2AB5852A-4D9F-4B39-A727-E7B498519A14}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember("_Editable")]
        public bool Editable
        {
            get
            {
                return _Editable;
            }
            set
            {
                if (_Editable != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Editable = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1EFE27C0-CE68-44EA-A876-840B62CE7CA7}</MetaDataID>
        private ColumnAlignment _Alignment = ColumnAlignment.Left;
        /// <MetaDataID>{7DA3B23E-EF81-4F25-A7B1-AF6856A58E37}</MetaDataID>
        /// <summary>Gets or sets the horizontal alignment of the Column's Cell contents </summary>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Alignment")]
        public OOAdvantech.UserInterface.ColumnAlignment Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                if (_Alignment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Alignment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{95E070D3-E62A-43D4-8F9E-4C5F7E5B411F}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        private OOAdvantech.ObjectStateManagerLink Properties;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{75E37357-DD6B-428F-91F5-61C04061C02B}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{5A587C63-28CA-405C-B18B-E7E2BA1A3F75}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_Name) && string.IsNullOrEmpty(Text))
                    return /*"Column"+*/ GetHashCode().ToString();
                if (string.IsNullOrEmpty(_Name))
                    return Text;
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
