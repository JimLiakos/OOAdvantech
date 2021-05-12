namespace AbstractionsAndPersistency
{
    using System;
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Remoting;
    using OOAdvantech.Transactions;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{bd43038e-6add-4a57-bdf5-355b6bd4a351}</MetaDataID>
    [BackwardCompatibilityID("{282CAC9D-8B97-4edd-A47B-AE1FE67AF223}")]
    public interface ICategory
    {
      
        /// <exclude>Excluded</exclude>
        [Association("SubCategory", typeof(ICategory), Roles.RoleB, "f84ac84b-611d-48d6-aa49-a144543f7def")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching | PersistencyFlag.ReferentialIntegrity)]
        //[RoleBMultiplicityRange(0,1)]
        [RoleBMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.ICategory> Parent
        {
            get;
            set;
        } 

        /// <MetaDataID>{9755b66d-68f6-48d5-839c-019a715ad08b}</MetaDataID>
        void AddSubCategory(ICategory category);


        /// <MetaDataID>{91bc2116-c3a2-41f0-90c7-d839eec84ea0}</MetaDataID>
        void DeleteSubCategory(ICategory category);


        /// <exclude>Excluded</exclude>
        [Association("SubCategory", typeof(ICategory), Roles.RoleA, "f84ac84b-611d-48d6-aa49-a144543f7def")]
        [RoleAMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.ICategory> SubCategories
        {
            get;

        }

        /// <MetaDataID>{25a7c1b1-1517-4085-9860-8450feede197}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        bool Root
        {
            set;
            get;
        }

        /// <MetaDataID>{25a7c1b1-1517-4085-9860-8450feede197}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Name
        {
            set;
            get;
        }
    }



    /// <MetaDataID>{8a180f3f-0aa7-4010-b49e-5a515c76f3fe}</MetaDataID>
    [BackwardCompatibilityID("{8a180f3f-0aa7-4010-b49e-5a515c76f3fe}"), Persistent()]
    public class Category : MarshalByRefObject, ICategory,OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink Properties; 
        ///// <exclude>Excluded</exclude>
        //private OOAdvantech.Member<AbstractionsAndPersistency.ICategory> _Parent=new OOAdvantech.Member<ICategory>();
        /// <MetaDataID>{bb2232c8-e20a-4e7c-8c8c-b24a480ec816}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.ICategory> _Parent;
        /// <MetaDataID>{02175822-91db-47a7-bed0-355d1a3f211e}</MetaDataID> _Parent
        [ PersistentMember("_Parent")]
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.ICategory> Parent
        {
            get
            {
                //return _Parent.Value;
                return _Parent;
            }
            set
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {

                    //_Parent.Value = value;
                    objStateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{6759c6fc-a11a-4187-97ed-9f9219d241a7}</MetaDataID>
        public void AddSubCategory(ICategory category)
        {
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {
                _SubCategories.Add(category);
                objStateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{a2874184-7e3c-40bb-9355-39c8c4b99ab9}</MetaDataID>
        public void DeleteSubCategory(ICategory category)
        {
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {
                _SubCategories.Remove(category);
                objStateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ICategory> _SubCategories = new OOAdvantech.Collections.Generic.Set<ICategory>();
        /// <MetaDataID>{12fdc08f-c748-490c-882b-a8ef9967644d}</MetaDataID>
        [PersistentMember("_SubCategories")]
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.ICategory> SubCategories
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<ICategory>(_SubCategories,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }

        }




        /// <exclude>Excluded</exclude>
        private bool _Root;

        /// <MetaDataID>{25a7c1b1-1517-4085-9860-8450feede197}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember("_Root")]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public bool Root
        {
            set
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _Root = value;
                    objStateTransition.Consistent = true;
                }
            }
            get
            {
                return _Root;
            }
        }
      
  

        /// <exclude>Excluded</exclude>
        private string _Name;

        /// <MetaDataID>{25a7c1b1-1517-4085-9860-8450feede197}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember("_Name")]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Name
        {
            set
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    objStateTransition.Consistent = true;
                }
            }
            get
            {
                return _Name;
            }
        }
    }
}
