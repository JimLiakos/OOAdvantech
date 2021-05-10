using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace OOAdvantech.UserInterface.ReportObjectDataSource
{
    /// <MetaDataID>{6a6b3ace-83b5-43c4-9ef3-200917548b40}</MetaDataID>
    [BackwardCompatibilityID("{6a6b3ace-83b5-43c4-9ef3-200917548b40}"), Persistent()]
    public class Member : OOAdvantech.UserInterface.Component
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Attribute _Attribute;
        /// <MetaDataID>{dfd19f6d-6988-426b-a630-700cb7f6217e}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Attribute Attribute
        {
            get
            {
                return _Attribute;
            }
            set
            {
                _Attribute = value;
                if (_Attribute != null)
                    MetaObjectIdentity = _Attribute.Identity.ToString();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.AssociationEnd _AssociationEnd;
        /// <MetaDataID>{3164bb30-41f8-439b-bee5-06c58e3cd603}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd
        {
            get
            {
                return _AssociationEnd;
            }
            set
            {
                _AssociationEnd = value;
                if (_AssociationEnd != null)
                    MetaObjectIdentity = _AssociationEnd.Identity.ToString();
            }
        }

        /// <exclude>Excluded</exclude>   
        string _MetaObjectIdentity;
        /// <MetaDataID>{91d7cf09-5a86-49b0-a680-e9264640a5c7}</MetaDataID>
        [PersistentMember("_MetaObjectIdentity")]
        [BackwardCompatibilityID("+3")]
        public string MetaObjectIdentity
        {
            get
            {
                return _MetaObjectIdentity;
            }
            set
            {
                if (_MetaObjectIdentity != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MetaObjectIdentity = value; 
                        stateTransition.Consistent = true;
                    }
        
                }

            }
        }
        /// <MetaDataID>{c5bb6213-1db7-4030-87ce-f9cab4e82516}</MetaDataID>
        ReportDataSource __DataSource;
        /// <exclude>Excluded</exclude>
        ReportDataSource _DataSource
        {
            get
            {
                return __DataSource;
            }
            set
            {
                __DataSource = value;
            }
        }
        [Association("ReportDataSourceMember", typeof(ReportDataSource), Roles.RoleB, "d75b3b93-594b-4b30-bf49-cf451809cbbd")]
        [ AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("__DataSource"), RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                if (_DataSource != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DataSource = value; 
                        stateTransition.Consistent = true;
                    }
        
                }
            }
        }
        /// <MetaDataID>{170991d9-cbae-4c83-af7a-762d95dcec0d}</MetaDataID>
        public Member()
        {

        }
        /// <MetaDataID>{7341a496-37e6-495b-9626-db699e33e2b1}</MetaDataID>
        object tmp;
          /// <MetaDataID>{2a197fb8-2e41-45fb-8c45-e9aa10f6e3b8}</MetaDataID>
        public Member(string name, ReportDataSource dataSource)
        {
            _Name = name;
            _DataSource = dataSource;
            tmp = dataSource;
        }

        /// <MetaDataID>{d434fe5e-4975-41d0-9eda-75600716b02a}</MetaDataID>
        internal Member(OOAdvantech.MetaDataRepository.Attribute attribute, ReportDataSource dataSource)
        {
            _Attribute = attribute;
            if (_Attribute != null)
            {
                _Name = _Attribute.Name;
                _MetaObjectIdentity = _Attribute.Identity.ToString();
            }
            _DataSource = dataSource;
        }

        /// <MetaDataID>{980b8ced-1a9f-4fa0-8c4a-37e9c742342c}</MetaDataID>
        internal Member(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, ReportDataSource dataSource)
        {
            _AssociationEnd = associationEnd;
            if (_AssociationEnd != null)
            {
                _Name = _AssociationEnd.Name;
                _MetaObjectIdentity = _AssociationEnd.Identity.ToString();
            }

            _DataSource = dataSource;
        }


        /// <MetaDataID>{b0588795-ab1d-42b1-b517-8e95cfc6a6c4}</MetaDataID>
        public Classifier OrgType
        {
            get
            {
                if (Attribute != null)
                    return Attribute.Type;
                else if (AssociationEnd  != null)
                    return AssociationEnd.Specification;
                else
                    return this.DataSource.Type;
            }

        }

        /// <MetaDataID>{a571484f-2be5-4329-a9fe-01cc39a6f8de}</MetaDataID>
        public OOAdvantech.MetaDataRepository.MetaObject MetaObject
        {
            get
            {
                if (Attribute != null)
                    return Attribute;
                return AssociationEnd;
            }
        }

        /// <MetaDataID>{b88d896c-9e48-4b91-9184-3843464f67b3}</MetaDataID>
        public Classifier Type
        {
            get
            {
                if (Attribute != null)
                {
                    if (!string.IsNullOrEmpty(Path))
                        return OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(Attribute.Type, Path);
                    return Attribute.Type;
                }
                else if (AssociationEnd != null)
                {
                    if (!string.IsNullOrEmpty(Path))
                        return OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(AssociationEnd.Specification, Path);
                    return AssociationEnd.Specification;
                }
                else
                {
                      if (!string.IsNullOrEmpty(Path))
                          return OOAdvantech.UserInterface.Runtime.UISession.GetClassifier(DataSource.Type, Path);
                      return null; ;
                    
                }

            }

        }
        /// <exclude>Excluded</exclude>    
        string _Path;
        /// <MetaDataID>{1dd8167e-91e6-43c2-91f6-c5dbaf5c4b4a}</MetaDataID>
        [PersistentMember("_Path")]
        [BackwardCompatibilityID("+1")]
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
                    if (DataSource != null)
                        DataSource.CheckDataSourceTypeConsistency();

                }
            }


        }
    }
}
