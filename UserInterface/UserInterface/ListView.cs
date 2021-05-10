namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using OOAdvantech;

    /// <MetaDataID>{7332D843-B4D9-4275-86AF-4A4117D0FBE3}</MetaDataID>
    public class ListView : Control
    {
        

        /// <exclude>Excluded</exclude>
        private MenuCommand _EditMenuCommand;
        [Association("EditMenu", typeof(UserInterface.MenuCommand), Roles.RoleA, "F33050A5-02AC-48cb-8A20-AEB1F69E175C")]
        [PersistentMember("_EditMenuCommand")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]
        public OOAdvantech.UserInterface.MenuCommand EditMenuCommand
        {
            get
            {
                return _EditMenuCommand;
            }
            set
            {
                if (_EditMenuCommand != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _EditMenuCommand = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        


        /// <exclude>Excluded</exclude>
        private MenuCommand _DeleteMenuCommand;
        [Association("DeleteMenu", typeof(UserInterface.MenuCommand), Roles.RoleA, "247ff48a-45c0-4f4c-9e19-b96f38ab71ae")]
        [PersistentMember("_DeleteMenuCommand")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]
        public OOAdvantech.UserInterface.MenuCommand DeleteMenuCommand
        {
            get
            {
                return _DeleteMenuCommand;
            }
            set
            {
                if (_DeleteMenuCommand != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeleteMenuCommand = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        


        /// <exclude>Excluded</exclude>
        private MenuCommand _InsertMenuCommand;
        [Association("InsertMenu", typeof(UserInterface.MenuCommand), Roles.RoleA, "016B6C48-2C79-48a5-83E7-2C37A1CFC74F")]
        [PersistentMember("_InsertMenuCommand")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]
        public OOAdvantech.UserInterface.MenuCommand InsertMenuCommand
        {
            get
            {
                return _InsertMenuCommand;
            }
            set
            {
                if (_InsertMenuCommand != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _InsertMenuCommand = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9184EE3C-0FDF-4610-8EEA-70A8F31CB0BF}</MetaDataID>
        private MenuCommand _Menu;
        /// <MetaDataID>{3BF1CF90-3728-43EF-B337-46B802FAECDC}</MetaDataID>
        [Association("Menu", typeof(UserInterface.MenuCommand), Roles.RoleA, "{A33BB9E0-B100-4213-92E9-6BDD0CD676C7}")]
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
                        _Menu.Name = "ListViewMainMenu";
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
        private OperationCall _EditOperation;
        [Association("Edit", typeof(UserInterface.OperationCall), Roles.RoleA, "{1A85E343-C95D-4c80-BC22-A0EF50655B29}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_EditOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall EditOperation
        {
            get
            {
                return _EditOperation;
            }
            set
            {
                if (_EditOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _EditOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        //
    


        /// <exclude>Excluded</exclude>
        private OperationCall _AllowDropOperation;
        [Association("AllowDrop", typeof(UserInterface.OperationCall), Roles.RoleA, "{86E2FD11-F013-4460-81AC-976CD49B5CF3}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_AllowDropOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall AllowDropOperation
        {
            get
            {
                return _AllowDropOperation;
            }
            set
            {
                if (_AllowDropOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AllowDropOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        private OperationCall _DragDropOperation;
        [Association("DragDrop", typeof(UserInterface.OperationCall), Roles.RoleA, "{819BE92A-4F5D-4033-B332-243CFDD2AAFF}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_DragDropOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall DragDropOperation
        {
            get
            {
                return _DragDropOperation;
            }
            set
            {
                if (_DragDropOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DragDropOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        
        /// <exclude>Excluded</exclude>
        private OperationCall _CutOperation;
        [Association("Cut", typeof(UserInterface.OperationCall), Roles.RoleA, "{C4A34B53-8DE5-4e08-B891-A0564D553E35}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_CutOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall CutOperation
        {
            get
            {
                return _CutOperation;
            }
            set
            {
                if (_CutOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CutOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8E2770D2-6683-4BCA-81D6-6372D86DB8AC}</MetaDataID>
        private OperationCall _InsertOperation;
        /// <MetaDataID>{A4D396EE-0658-40B9-82E8-445B4076F8B8}</MetaDataID>
        [Association("Insert", typeof(UserInterface.OperationCall), Roles.RoleA, "{445A6871-CFBB-4772-95F3-478CEE1C3220}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_InsertOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall InsertOperation
        {
            get
            {
                return _InsertOperation;
            }
            set
            {
                if (_InsertOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _InsertOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        private OperationCall _LoadListOperation;
        [Association("LoadList", typeof(UserInterface.OperationCall), Roles.RoleA, "{436C15A1-919B-41b4-8051-39A0868C2312")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_LoadListOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall LoadListOperation
        {
            get
            {
                return _LoadListOperation;
            }
            set
            {
                if (_LoadListOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LoadListOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        
        /// <exclude>Excluded</exclude>
        private OperationCall _BeforeShowContextMenuOperation;
        [Association("ShowContextMenu", typeof(UserInterface.OperationCall), Roles.RoleA, "{ABBFBADE-AA86-436d-953D-0560DBDECE0E}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_BeforeShowContextMenuOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall BeforeShowContextMenuOperation
        {
            get
            {
                return _BeforeShowContextMenuOperation;
            }
            set
            {
                if (_BeforeShowContextMenuOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BeforeShowContextMenuOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{95C997EC-C1A0-42D9-970D-76AE4FFDBC52}</MetaDataID>
        private OperationCall _DeleteOperation;
        /// <MetaDataID>{7E46E127-5EDF-4DE8-B7AC-3F11C7CEEBC6}</MetaDataID>
        [Association("Delete", typeof(UserInterface.OperationCall), Roles.RoleA, "{6714B444-64D3-4687-A54B-A23D1E0AD9B7}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_DeleteOperation")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.OperationCall DeleteOperation
        {
            get
            {
                return _DeleteOperation;
            }
            set
            {
                if (_DeleteOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeleteOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }






        /// <MetaDataID>{732A7517-E3D5-4CD1-A239-B97C108B2551}</MetaDataID>
        private System.Collections.Generic.List<OOAdvantech.UserInterface.Column> SortedColumns = new System.Collections.Generic.List<Column>();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{19523E2C-DC28-444A-8013-869414536D35}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Column> _Columns;
        /// <MetaDataID>{3154051A-285C-454E-9DBB-5EA9ADBAB065}</MetaDataID>
        [Association("ListColums", typeof(UserInterface.Column), Roles.RoleA, "{31E145CD-C31B-495C-A649-1B7E40320A2B}")]
        [PersistentMember("_Columns")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.UserInterface.Column> Columns
        {

            set
            {
            
            }
            get
            {
                OOAdvantech.Collections.Generic.Set<Column> columns = new OOAdvantech.Collections.Generic.Set<Column>();

                SortColumns();
                foreach (Column column in SortedColumns)
                    columns.Add(column);
                return columns;
            }
        }



        /// <MetaDataID>{D01E181C-AFF4-48BA-AD7C-6FCB537FBF49}</MetaDataID>
        void SortColumns()
        {
            SortedColumns.Clear();
            foreach (Column column in _Columns)
                SortedColumns.Add(column);
            SortedColumns.Sort(new PositionCompare<Column>());
            for (short k = 0; k != SortedColumns.Count; k++)
            {
                Column column = SortedColumns[k];
                if (column.Position != k)
                {
                    using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition())
                    {
                        for (short i = 0; i != SortedColumns.Count; i++)
                        {
                            column = SortedColumns[i];
                            if (column != null)
                            {
                                column.Position = i;
                                stateTransition.Consistent = true;
                            }
                        }

                        stateTransition.Consistent = true;
                    }
                    return;
                }

            }
        }


        /// <MetaDataID>{0E27C3AE-3F9C-4A06-A14A-4B7D09C9FFB6}</MetaDataID>
        public Column NewColumn(short index, string name)
        {
            
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                Column newColumn = objectStorage.NewObject(typeof(Column)) as Column;
                newColumn.Name = name;
                AddColumn(index, newColumn);
                stateTransition.Consistent = true;

                return newColumn;
            }


        }
        /// <MetaDataID>{C484B1BB-65C1-4A64-84D4-374E45FBBB90}</MetaDataID>
        public void AddColumn(short index, OOAdvantech.UserInterface.Column column)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                SortColumns();

                if (SortedColumns.Contains(column))
                {
                    SortedColumns.Remove(column);
                    SortedColumns.Insert(index, column);
                }
                else
                    SortedColumns.Insert(index, column);

                if (!_Columns.Contains(column))
                    _Columns.Add(column);
                for (short i = 0; i != SortedColumns.Count; i++)
                {
                    column = SortedColumns[i] as Column;
                    if (column != null)
                        column.Position = i;
                }
                stateTransition.Consistent = true;
            }



        }
        /// <MetaDataID>{DF0013C8-0BCB-45F3-B914-6FC65F3B2198}</MetaDataID>
        public Column NewColumn(string name)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                Column newColumn = objectStorage.NewObject(typeof(Column)) as Column;
                newColumn.Text = name;
                AddColumn(newColumn);
                stateTransition.Consistent = true;
                return newColumn;
            }
        }


        /// <MetaDataID>{9525FF4E-27A9-4F4B-823D-55A2126B085D}</MetaDataID>
        public void AddColumn(Column column)
        {
            if (column == null)
                return;
            if (_Columns.Contains(column))
                return;

            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                column.Position = (short)_Columns.Count;
                if (PersistenceLayer.ObjectStorage.GetStorageOfObject(column) == null)
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(column);
                _Columns.Add(column);
                SortColumns();
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{923E7EF3-F804-48D1-ACA5-74900E9CED09}</MetaDataID>
        public void RemoveColumn(Column column)
        {
            if (column == null)
                return;
            if (!_Columns.Contains(column))
                return;

            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                _Columns.Remove(column);
                SortColumns();
                stateTransition.Consistent = true;
            }


        }

    }
}
