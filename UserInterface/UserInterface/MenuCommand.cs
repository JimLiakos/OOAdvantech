

namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech;
    using OOAdvantech.Transactions;
    using System;

    /// <MetaDataID>{DB6320F4-BD19-44A7-ACF1-B2346F776C4F}</MetaDataID>
    [BackwardCompatibilityID("{DB6320F4-BD19-44A7-ACF1-B2346F776C4F}")]
    [Persistent()]
    public class MenuCommand
    {

  
 



        /// <MetaDataID>{5404fd36-a4fd-4333-85a9-f50b3eb35f0c}</MetaDataID>
        public MenuCommand()
        {
            _CommandID = System.Guid.NewGuid().ToString();

        }
        /// <MetaDataID>{b781eebe-032f-4447-a9c1-16bb8a4a314d}</MetaDataID>
        public MenuCommand(MenuCommand copy)
        {
            _Name = copy.Name;
            _OnCommandOperationCall = copy.OnCommandOperationCall;
            _IsEnableOperationCall = copy._IsEnableOperationCall;
            _IsVisibleOperationCall = copy._IsVisibleOperationCall;
            _Position = copy.Position;
            _RefreshOwnerControl = copy.RefreshOwnerControl;
            _TransactionOption = copy.TransactionOption;
            _ViewEditForm = copy.ViewEditForm;
            _ViewEditObjectSource = copy.ViewEditObjectSource;
            _CommandID = copy.CommandID;

            foreach (MenuCommand subMenu in copy.SubMenuCommands)
                _SubMenuCommands.Add(new MenuCommand(subMenu));
        }
        //public event EventHandler Click;

        public event MenuCommandClickedHandler Click;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{93A73E1E-A48E-44F8-9FDB-6BFE79E97C2F}</MetaDataID>
        private bool _RefreshOwnerControl;
        /// <MetaDataID>{A41BF537-EE4D-459B-A4BA-A39C131BFCF4}</MetaDataID>
        [BackwardCompatibilityID("+19")]
        [PersistentMember("_RefreshOwnerControl")]
        public bool RefreshOwnerControl
        {
            get
            {
                
                return _RefreshOwnerControl;
            }
            set
            {
                if (_RefreshOwnerControl != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _RefreshOwnerControl = value;
                    }
                    else
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _RefreshOwnerControl = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6459EAE6-5603-46C2-BAA5-2F2095B8E91F}</MetaDataID>
        private OperationCall _OnCommandOperationCall;
        /// <MetaDataID>{F96FF242-79A7-46EB-99CA-9DBA0BA3767A}</MetaDataID>
        [Association("MenuOperationCall",typeof(OOAdvantech.UserInterface.OperationCall),Roles.RoleA,"{843C03B1-FB07-4E50-8740-E58F1C992B58}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.ReferentialIntegrity|PersistencyFlag.CascadeDelete)]
        [PersistentMember("_OnCommandOperationCall")]
        [RoleAMultiplicityRange(1,1)]
        [RoleBMultiplicityRange(0)]
        public OperationCall OnCommandOperationCall
        {
            get
            {
                return _OnCommandOperationCall;
            }
            set
            {
                if (_OnCommandOperationCall != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _OnCommandOperationCall = value;
                    }
                    else
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _OnCommandOperationCall = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OperationCall _IsEnableOperationCall;

        /// <MetaDataID>{5030909e-ee23-4635-b603-d24300145488}</MetaDataID>
        [Association("MenuIsEnableOperationCall", Roles.RoleA, "38613312-b65d-4e15-bf56-b7a4a2f55c4d")]
        [MetaDataRepository.PersistentMember("_IsEnableOperationCall")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        public OperationCall IsEnableOperationCall
        {
            get
            {
                return _IsEnableOperationCall;
            }
            set
            {
                if (_IsEnableOperationCall != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _IsEnableOperationCall = value;
                    }
                    else
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _IsEnableOperationCall = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OperationCall _IsVisibleOperationCall;

        [Association("MenuIsVisibleOperationCall", Roles.RoleA, "ce3fa56a-8cba-408a-8fd4-343e306c6d94")]
        [PersistentMember("_IsVisibleOperationCall")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        public OperationCall IsVisibleOperationCall
        {
            get
            {
                return _IsVisibleOperationCall;
            }
            set
            {
                if (_IsVisibleOperationCall != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _IsVisibleOperationCall = value;
                    }
                    else
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _IsVisibleOperationCall = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8D842BE0-3407-4A65-B9B1-567600A272E9}</MetaDataID>
        private OOAdvantech.Transactions.TransactionOption _TransactionOption=TransactionOption.Supported;
        /// <MetaDataID>{46136172-CBDA-4E60-BDC1-8B2F590AE777}</MetaDataID>
        [BackwardCompatibilityID("+17")]
        [PersistentMember("_TransactionOption")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return _TransactionOption;
            }
            set
            {
                if (_TransactionOption != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _TransactionOption = value;
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _TransactionOption = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E6FDA5F5-5091-4848-9463-6FA9B3544E6B}</MetaDataID>
        private string _ViewEditObjectSource;
        /// <MetaDataID>{9E72C5B7-CD76-4BA3-A05F-1F56115B34B3}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        [PersistentMember("_ViewEditObjectSource")]
        public string ViewEditObjectSource
        {
            get
            {
                return _ViewEditObjectSource;
            }
            set
            {
                if (_ViewEditObjectSource != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _ViewEditObjectSource = value;
                    }
                    else
                    {

                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _ViewEditObjectSource = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E54739E6-E5AD-4BC8-9429-C37BA7907EFD}</MetaDataID>
        private string _ViewEditForm;
        /// <MetaDataID>{EBACE42A-DA51-451F-AEB3-639C3CC945EA}</MetaDataID>
        [BackwardCompatibilityID("+15")]
        [PersistentMember("_ViewEditForm")]
        public string ViewEditForm
        {
            get
            {
                
                return _ViewEditForm;
            }
            set
            {
                if (_ViewEditForm != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _ViewEditForm = value;
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _ViewEditForm = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A40D5E18-B577-4D34-ABCC-A683215BA0CC}</MetaDataID>
        private short _Position;
        /// <MetaDataID>{C1873E29-76A5-4FEB-9652-E103C6FEAF4B}</MetaDataID>
        [BackwardCompatibilityID("+6")]
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
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _Position = value;
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _Position = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A837B44A-DC9E-41F2-B945-3243F97CD245}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<MenuCommand> _SubMenuCommands = new OOAdvantech.Collections.Generic.Set<MenuCommand>();
        /// <MetaDataID>{FA99E109-06B3-403E-B937-18522875EDAA}</MetaDataID>
        [Association("SubMenu",typeof(OOAdvantech.UserInterface.MenuCommand),Roles.RoleA,"{B0E7E887-5C65-41C5-A9EE-48FE580A15BB}")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity|PersistencyFlag.CascadeDelete)]
        [PersistentMember("_SubMenuCommands")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<MenuCommand> SubMenuCommands
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<MenuCommand> menuCommands = new OOAdvantech.Collections.Generic.Set<MenuCommand>();
                SortSubMenuCommands();
                foreach (MenuCommand command in SortedSubMenuCommands)
                    menuCommands.Add(command);
                return menuCommands;
            }
        }

        /// <exclude>Excluded</exclude>
        private string _CommandID;
        /// <MetaDataID>{C04125BA-639C-4648-AF85-43FD2180E940}</MetaDataID>
        [BackwardCompatibilityID("+20")]
        [PersistentMember("_CommandID")]
        public string CommandID
        {
            get
            {
                
                
                if (string.IsNullOrEmpty(_CommandID))
                    _CommandID = System.Guid.NewGuid().ToString();
                return _CommandID;
            }
            set
            {
                if (_CommandID != value)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _CommandID = value;
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _CommandID = value;
                            stateTransition.Consistent = true;

                        }
                    }
                }
            }
        }

        /// <MetaDataID>{f4551dfb-9fdc-4937-941a-2de78d5b4fdf}</MetaDataID>
        public object Tag;


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B26E169C-2458-4C16-AC25-C1D5A96DA3E6}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{6562EA92-A62F-4440-9E17-3E5084336278}</MetaDataID>
        [BackwardCompatibilityID("+4")]
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
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        _Name = value;
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                        {
                            _Name = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }
        /// <MetaDataID>{D19904D1-AA3D-4E62-BB26-B3725E3CDCA2}</MetaDataID>
        private ObjectStateManagerLink Properties;


        /// <MetaDataID>{D0BC71E6-AC84-41DA-8077-540F1CF20280}</MetaDataID>
        private System.Collections.Generic.List<MenuCommand> SortedSubMenuCommands = new System.Collections.Generic.List<MenuCommand>();
        /// <MetaDataID>{D01E181C-AFF4-48BA-AD7C-6FCB537FBF49}</MetaDataID>
        void SortSubMenuCommands()
        {
            SortedSubMenuCommands.Clear();
            foreach (MenuCommand subMenuCommand in _SubMenuCommands)
                SortedSubMenuCommands.Add(subMenuCommand);
            SortedSubMenuCommands.Sort(new PositionCompare<MenuCommand>());
            for (short k = 0; k != SortedSubMenuCommands.Count; k++)
            {
                MenuCommand command = SortedSubMenuCommands[k];
                if (command.Position != k)
                {
                    if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
                    {
                        for (short i = 0; i != SortedSubMenuCommands.Count; i++)
                        {
                            command = SortedSubMenuCommands[i];
                            if (command != null)
                            {
                                command.Position = i;
                            }
                        }
                    }
                    else
                    {
                        using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition())
                        {
                            for (short i = 0; i != SortedSubMenuCommands.Count; i++)
                            {
                                command = SortedSubMenuCommands[i];
                                if (command != null)
                                {
                                    command.Position = i;
                                    stateTransition.Consistent = true;
                                }
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    return;
                }
            }



        }


        /// <MetaDataID>{0E27C3AE-3F9C-4A06-A14A-4B7D09C9FFB6}</MetaDataID>
        public MenuCommand NewSubCommand(short index, string name)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                MenuCommand newMenuCommand = objectStorage.NewObject(typeof(MenuCommand)) as MenuCommand;
                newMenuCommand.Name = name;
                AddSubCommand(index,newMenuCommand);
                stateTransition.Consistent = true;
                
                return newMenuCommand;
            }


        }
        /// <MetaDataID>{C484B1BB-65C1-4A64-84D4-374E45FBBB90}</MetaDataID>
        public void AddSubCommand(short index, MenuCommand menuCommand)
        {
            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
            {
                SortSubMenuCommands();
                if (SortedSubMenuCommands.Contains(menuCommand))
                {
                    SortedSubMenuCommands.Remove(menuCommand);
                    SortedSubMenuCommands.Insert(index, menuCommand);
                }
                else
                    SortedSubMenuCommands.Insert(index, menuCommand);

                if (!_SubMenuCommands.Contains(menuCommand))
                    _SubMenuCommands.Add(menuCommand);
                for (short i = 0; i != SortedSubMenuCommands.Count; i++)
                {
                    MenuCommand command = SortedSubMenuCommands[i] as MenuCommand;
                    if (command != null)
                        command.Position = i;
                }
            }
            else
            {
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    SortSubMenuCommands();
                    if (SortedSubMenuCommands.Contains(menuCommand))
                    {
                        SortedSubMenuCommands.Remove(menuCommand);
                        SortedSubMenuCommands.Insert(index, menuCommand);
                    }
                    else
                        SortedSubMenuCommands.Insert(index, menuCommand);

                    if (!_SubMenuCommands.Contains(menuCommand))
                        _SubMenuCommands.Add(menuCommand);
                    for (short i = 0; i != SortedSubMenuCommands.Count; i++)
                    {
                        MenuCommand command = SortedSubMenuCommands[i] as MenuCommand;
                        if (command != null)
                        {
                            command.Position = i;
                            stateTransition.Consistent = true;
                        }
                    }
                    stateTransition.Consistent = true;
                }
            }



        }
        /// <MetaDataID>{DF0013C8-0BCB-45F3-B914-6FC65F3B2198}</MetaDataID>
        public MenuCommand NewSubCommand(string name)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using(Transactions.ObjectStateTransition stateTransition =new Transactions.ObjectStateTransition(this))
            {
                MenuCommand newMenuCommand= objectStorage.NewObject(typeof(MenuCommand)) as MenuCommand;
                newMenuCommand.Name = name;
                AddSubCommand(newMenuCommand);
                stateTransition.Consistent = true;
                return newMenuCommand;
            }
        }


        /// <MetaDataID>{9525FF4E-27A9-4F4B-823D-55A2126B085D}</MetaDataID>
        public void AddSubCommand(MenuCommand menuCommand)
        {
            if (menuCommand == null)
                return;
            if (_SubMenuCommands.Contains(menuCommand))
                return;
            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
            {
                using (SystemStateTransition sst = new SystemStateTransition(TransactionOption.Suppress))
                {
                    menuCommand.Position = (short)_SubMenuCommands.Count;
                    _SubMenuCommands.Add(menuCommand);
                    SortSubMenuCommands();

                    sst.Consistent = true;
                }
            }
            else
            {
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    menuCommand.Position = (short)_SubMenuCommands.Count;
                    _SubMenuCommands.Add(menuCommand);
                    SortSubMenuCommands();
                    stateTransition.Consistent = true;
                }
            }

        }

        /// <MetaDataID>{923E7EF3-F804-48D1-ACA5-74900E9CED09}</MetaDataID>
        public void RemoveSubCommand(MenuCommand menuCommand)
        {
            if(menuCommand==null)
                return;
            if (!_SubMenuCommands.Contains(menuCommand))
                return;
            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) == null)
            {
                _SubMenuCommands.Remove(menuCommand);
                SortSubMenuCommands();

            }
            else
            {
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    _SubMenuCommands.Remove(menuCommand);
                    SortSubMenuCommands();
                    stateTransition.Consistent = true;
                }
            }


        }

        /// <MetaDataID>{bb0367d4-8e12-4c27-a58a-2ca7ff53e6c6}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection;

        /// <MetaDataID>{23f64935-908f-4539-83b3-150cafc514c6}</MetaDataID>
        public void RaiseMenuClickEvent(object sender)
        {
            if (Click != null)
                Click(new MenuCommandClickedEventArg(sender, this));
            //if (Click != null)
            //    Click(sender,EventArgs.Empty);

        }
        /// <MetaDataID>{8d7f00c7-a4c6-49bd-95d5-98f4ff8e3399}</MetaDataID>
        public void MenuClicked(object sender)
        {
            System.Reflection.MethodInfo methodInfo = GetType().GetMethod("RaiseMenuClickEvent");
            UserInterfaceObjectConnection.Invoke(this, methodInfo, new object[1] { sender }, TransactionOption);
        }

        
    }
    public delegate void MenuCommandClickedHandler(MenuCommandClickedEventArg menuCommandEventArgs);

    /// <MetaDataID>{49eee9de-1d33-4e40-bfe2-e16e5aafc22f}</MetaDataID>
    public class MenuCommandClickedEventArg
    {
        /// <MetaDataID>{4b4a0630-5688-45a0-a070-89a1fcdcbb09}</MetaDataID>
        public readonly object Sender;
        /// <MetaDataID>{bdb81ec8-f90d-468c-8ce4-c8a18b0051cf}</MetaDataID>
        public readonly MenuCommand ClickedMenuCommand;
        /// <MetaDataID>{dc2a6ba3-11a7-4c0f-bc45-b5ffd84ce288}</MetaDataID>
        public MenuCommandClickedEventArg(object sender, MenuCommand clickedMenuCommand)
        {
            Sender = sender;
            ClickedMenuCommand = clickedMenuCommand;
        }
    }
}
