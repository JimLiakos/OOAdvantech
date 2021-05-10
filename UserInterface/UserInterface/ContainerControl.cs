namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using Transactions;
    /// <MetaDataID>{FC20F1FF-072F-4749-AA2B-D471DC70B935}</MetaDataID>
    [BackwardCompatibilityID("{FC20F1FF-072F-4749-AA2B-D471DC70B935}")]
    [Persistent()]
    public class ContainerControl : Control
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{02797FB3-7E31-494F-B3BF-EF4B4D574515}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Control> _Controls = new OOAdvantech.Collections.Generic.Set<Control>();
        /// <MetaDataID>{8D86A8D4-ABF8-4617-97E3-FEDD2E6471BA}</MetaDataID>
        [Association("HostedControl", typeof(OOAdvantech.UserInterface.Control), Roles.RoleA, "{5F15136F-FA5A-4D2A-AF59-0B100DC6E6B8}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Controls")]
        [RoleAMultiplicityRange(1)]
        public OOAdvantech.Collections.Generic.Set<Control> Controls
        {
            get
            { 
                return new OOAdvantech.Collections.Generic.Set<Control>(_Controls, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8585E9CF-ACB7-4296-B7FC-F1B17F65DDB7}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Component> _Components = new OOAdvantech.Collections.Generic.Set<Component>();

        /// <MetaDataID>{247BC749-04BA-4A4D-8F58-ED88F0A9EAE9}</MetaDataID>
        [Association("ContainedComponent", typeof(UserInterface.Component), Roles.RoleA, "{70EDDA9F-F88A-44BD-AACF-6EB83A80AC31}")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Components")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<Component> Components
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<Component>(_Components, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <MetaDataID>{6f06b247-f1a2-4605-b2ec-24eda95fe2fa}</MetaDataID>
        public void AddComponent(Component component)
        {
            if (!_Components.Contains(component))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Components.Add(component);
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{8e15637e-4a88-4a74-aff0-f86921cd8b17}</MetaDataID>
        public void RemoveComponent(Component component)
        {
            if (_Components.Contains(component))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Components.Remove(component);
                    stateTransition.Consistent = true;
                }
            }

        }
        /// <MetaDataID>{4fdd4bc7-eb98-401f-abed-19267d3876ad}</MetaDataID>
        public void AddControl(Control control)
        {
            if (!_Controls.Contains(control))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Controls.Add(control);
                    //AddComponent(control);
                    stateTransition.Consistent = true;
                }
            }
            //else
            //    AddComponent(control);
        }
        /// <MetaDataID>{30630e67-881b-4c1a-8301-c1911327b847}</MetaDataID>
        public void RemoveControl(Control control)
        {
            if (_Controls.Contains(control))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                   // RemoveComponent(control);
                    _Controls.Remove(control);
                    stateTransition.Consistent = true;
                }
            }
            //else
            //    RemoveComponent(control);

        }

    }
}
