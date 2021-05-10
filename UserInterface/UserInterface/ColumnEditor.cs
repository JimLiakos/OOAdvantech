using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{885ff068-fe1b-4232-9e60-9d4ddca695e8}</MetaDataID>
    [BackwardCompatibilityID("{885ff068-fe1b-4232-9e60-9d4ddca695e8}")]
    [Persistent()]
    public class ColumnEditor
    {
        /// <exclude>Excluded</exclude>
        string _ColumnEditorTypeFullName;
        /// <MetaDataID>{f1f9de73-1041-4c49-9114-edc8c1adbb34}</MetaDataID>
        [PersistentMember("_ColumnEditorTypeFullName")]
        public string ColumnEditorTypeFullName
        {
            get
            {
                return _ColumnEditorTypeFullName;
            }
            set
            {
                if (_ColumnEditorTypeFullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ColumnEditorTypeFullName = value; 
                        stateTransition.Consistent = true;
                    }
        
                }
            }
        }

        /// <MetaDataID>{34ee44b3-bc11-4adb-b390-fbf5c516e3f1}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink Properties;

        /// <exclude>Excluded</exclude>
        Column _Column;
        [Association("EditColumn", typeof(Column), Roles.RoleB, "b2a926a7-b669-4f09-a66a-46f7c05285f0")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Column")]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.Column Column
        {
            get
            {
                return _Column;
            }
            set
            {
                if (_Column != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Column = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        private string _Name;
        /// <MetaDataID>{195bdad8-bdce-444d-a605-8b9536d90da4}</MetaDataID>
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
