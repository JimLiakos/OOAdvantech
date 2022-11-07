using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.CodeMetaDataRepository;
using OOAdvantech.Transactions;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{2404b316-daa1-4586-ab27-e076c1b549db}</MetaDataID>
    public class ClassViewPresantation : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>
    {
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{1f928acc-3cc8-4d1b-9e65-2dbba00d2a4f}</MetaDataID>
        public ClassViewPresantation(OOAdvantech.MetaDataRepository.Class _class) :
            base(_class)
        {
            ClassAsTreeNode = new ClassifierTreeNode(_class, null, true);

        }
        /// <MetaDataID>{cb97835f-59dc-4289-8d8a-a5c951ff65fe}</MetaDataID>
        public System.Collections.Generic.List<RDBMSMappingContext> RDBMSMappingContexts
        {
            get
            {
                return (RealObject.ImplementationUnit as OOAdvantech.CodeMetaDataRepository.Project).RDBMSMappingContexts;
            }
        }

        /// <MetaDataID>{45400a35-e1f6-4df2-8a2c-d5a8a9750e15}</MetaDataID>
        public void Refresh()
        {
            if (_SelectedRDBMSMappingContext != null)
            {

                if (_RDBMSClass != null)
                {

                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        try
                        {
                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack(true);

                            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                            _RDBMSClass.Synchronize(RealObject);
                            _RDBMSClass.ImplementationUnit.Name = _RDBMSClass.Name;
                            if (_RDBMSClass.ImplementationUnit.Context == null)
                            {
                                _RDBMSClass.ImplementationUnit.Context = new OOAdvantech.RDBMSMetaDataRepository.Storage("MapingStorage");
                                SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(_RDBMSClass.ImplementationUnit.Context);
                            }

                            _SelectedRDBMSMappingContext.Save();
                            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                            OOAdvantech.MetaDataRepository.StorageCell activeStorageCell = _RDBMSClass.ActiveStorageCell;
                            stateTransition.Consistent = true; ;
                        }
                        catch (System.Exception Error)
                        {
                            throw;
                        }
                    }
                }

                _SelectedRDBMSMappingContext.Save();
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }

        }

        /// <MetaDataID>{b387b3f2-dc81-47f4-bb21-6a3988e70117}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd SelectedAssociationMember
        {
            get
            {
                if (_SelectedMember != null && _SelectedMember.MetaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                {
                    OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEnd = (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(SelectedMember.MetaObject) as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                    
                    return associationEnd;
                }
                else
                    return null;
            }
        }


        /// <MetaDataID>{92f98a2a-1bdd-4aa8-966e-5210a3de246f}</MetaDataID>
        public List<OOAdvantech.RDBMSMetaDataRepository.Class> RelatedConcreteClasses
        {
            get
            {
                List<OOAdvantech.RDBMSMetaDataRepository.Class> relatedConcreteClasses = new List<OOAdvantech.RDBMSMetaDataRepository.Class>();
                if (SelectedAssociationMember != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Classifier classifier in SelectedAssociationMember.Specification.GetAllSpecializeClasifiers())
                    {
                        if (classifier is OOAdvantech.RDBMSMetaDataRepository.Class &&
                            (classifier as OOAdvantech.RDBMSMetaDataRepository.Class).ActiveStorageCell != null)
                        {
                            relatedConcreteClasses.Add(classifier as OOAdvantech.RDBMSMetaDataRepository.Class);
                        }
                    }
                    if (_SelectedRelationConcreteClass == null || !relatedConcreteClasses.Contains(_SelectedRelationConcreteClass))
                    {
                        if (relatedConcreteClasses.Count > 0)
                            SelectedRelationConcreteClass = relatedConcreteClasses[0];
                        else
                            SelectedRelationConcreteClass = null;

                    }
                }
                return relatedConcreteClasses;
            }
        }
        /// <MetaDataID>{0cdee691-2c83-4881-91e6-dbeb8fc40e49}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.Class _SelectedRelationConcreteClass;

        /// <MetaDataID>{545e204a-7468-4250-bbb0-49f85a71f4a0}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink;
        /// <MetaDataID>{b0a15aac-ecae-427a-bc99-b09fc8c0833e}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataRepository.Class SelectedRelationConcreteClass
        {
            get
            {
                if (SelectedAssociationMember == null)
                {
                    _SelectedRelationConcreteClass = null;
                    _TableWithReferenceColumns = null;

                }
                return _SelectedRelationConcreteClass;
            }
            set
            {

                _SelectedRelationConcreteClass = value;
                if (_SelectedRelationConcreteClass != null)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, OOAdvantech.MetaDataRepository.Roles.RoleA);
                    if (pathIdentity.Count > 0)
                        pathIdentity.Pop();

                    if (SelectedAssociationMember.IsRoleA)
                        StorageCellsLink = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetStorageCellsLink(SelectedRelationConcreteClass.ActiveStorageCell, _RDBMSClass.ActiveStorageCell, pathIdentity.ToString(), true);
                    else
                        StorageCellsLink = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetStorageCellsLink(_RDBMSClass.ActiveStorageCell, _SelectedRelationConcreteClass.ActiveStorageCell, pathIdentity.ToString(), true);
                    StorageCellsLink.AutoRelationTableGeneration = false;
                    if (StorageCellsLink.ObjectLinksTable != null )
                        _TableWithReferenceColumns = DataBase.GetTable(StorageCellsLink.ObjectLinksTable.Name);
                    if (StorageCellsLink.ObjectLinksTable != null && StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        SelectManualTableWithReferenceColumns = true;
                    //Force reference columns load
                    var column = FirstRoleReferenceColumn;
                    column = FirstRoleIndexerColumn;
                    
                }
                else
                {
                    StorageCellsLink = null;

                }
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }
        /// <MetaDataID>{8497357b-ae35-4c54-808b-b63de79a3b5a}</MetaDataID>
        public bool ChooseRelatedClass
        {
            get
            {
                if (_SelectedMember != null && _SelectedMember.MetaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                    return true;
                else
                    return false;
            }
        }

        //System.Collections.Generic.List<OOAdvantech.RDBMSDataObjects.Table> RelationTables
        //{
        //    get
        //    {
        //        if(SelectedAssociationMember!=null)
        //        {
        //            if(SelectedAssociationMember.Association.MultiplicityType==OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
        //                return new List<OOAdvantech.RDBMSDataObjects.Table>( DataBase.Tables);
        //            else
        //            {
        //                List<OOAdvantech.RDBMSDataObjects.Table> tables=new List<OOAdvantech.RDBMSDataObjects.Table>();
        //                if(SelectedAssociationMember.Multiplicity.IsMany &&
        //                    SelectedAssociationMember.Specification as OOAdvantech.RDBMSMetaDataRepository.Class)
        //                {

        //                }
        //            }
        //        }

        //    }
        //}

        /// <MetaDataID>{8f05e9d5-7401-4fe7-8fd2-7c488adca5e8}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.Table __TableWithReferenceColumns;
        /// <exclude>Excluded</exclude>
        OOAdvantech.RDBMSDataObjects.Table _TableWithReferenceColumns
        {
            get
            {
                return __TableWithReferenceColumns;
            }
            set
            {
                __TableWithReferenceColumns = value;
            }
        }
        /// <summary>
        /// Defines the table wich will be contains the reference columns 
        /// for relation mapping
        /// </summary>
        /// <MetaDataID>{e87fc8a1-8b09-454d-aaff-e89a91e3c891}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Table TableWithReferenceColumns
        {
            get
            {
                return _TableWithReferenceColumns;
            }
            set
            {
                if (_TableWithReferenceColumns != value && _TableWithReferenceColumns != null)
                {
                    SecondRoleReferenceColumn = null;
                    FirstRoleReferenceColumn = null;
                    FirstRoleIndexerColumn = null;
                }

                _TableWithReferenceColumns = value;
                if ((StorageCellsLink != null &&
                    StorageCellsLink.Type.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                    StorageCellsLink.Type.LinkClass == null) || UseRelationTable)
                {
                    if (_TableWithReferenceColumns != null)
                    {
                        OOAdvantech.RDBMSMetaDataRepository.Table relationTable = null;
                        foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in StorageCellsLink.Namespace.OwnedElements)
                        {
                            if (metaObject is OOAdvantech.RDBMSMetaDataRepository.Table &&
                                metaObject.Name == _TableWithReferenceColumns.Name)
                            {
                                relationTable = metaObject as OOAdvantech.RDBMSMetaDataRepository.Table;
                                break;
                            }
                        }
                        if (relationTable == null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {

                                StorageCellsLink.ObjectLinksTable = new OOAdvantech.RDBMSMetaDataRepository.Table(_TableWithReferenceColumns.Name, StorageCellsLink);
                                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(StorageCellsLink).CommitTransientObjectState(StorageCellsLink.ObjectLinksTable);
                                stateTransition.Consistent = true;
                            }
                        }
                        else
                            StorageCellsLink.ObjectLinksTable = relationTable;

                    }
                    else StorageCellsLink.ObjectLinksTable = null;
                }
                SelectedRDBMSMappingContext.Save();
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }
        /// <MetaDataID>{db8e9a38-742c-45d8-b291-62af233c5b4e}</MetaDataID>
        public string SecondRoleReferenceColumnLabel
        {
            get
            {
                if (!UseRelationTable)
                    return "Ref columns";
                else
                {
                    if (UseRelationTable)
                    {
                        if (SelectedRelationConcreteClass != null)
                            return SelectedRelationConcreteClass.Name + " Ref columns";
                    }
                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToOne ||
                        SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToMany))
                    {
                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEnd = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                        if (!associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Ref columns";
                        if (SelectedRelationConcreteClass != null && !associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Ref columns";

                        if (associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Ref columns";
                        if (SelectedRelationConcreteClass != null && associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Ref columns";

                    }
                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany ||
                       SelectedAssociationMember.Association.LinkClass != null))
                    {
                        if (SelectedRelationConcreteClass != null)
                            return SelectedRelationConcreteClass.Name + " Ref columns";
                    }
                    return "Ref columns";
                }
            }
        }
        /// <MetaDataID>{22d73fe8-8855-4045-9796-cb002187cce5}</MetaDataID>
        public string FirstRoleIndexerColumnLabel
        {
            get
            {
                if (SelectedAssociationMember == null)
                    return "Indexer column";
                else
                {
                    if (!SelectedAssociationMember.Multiplicity.IsMany)
                        return "Indexer column";

                    if (!SelectedAssociationMember.Indexer)
                        return "Indexer column";

                    if(SelectedRelationConcreteClass==null)
                        return "Indexer column";

                    return SelectedRelationConcreteClass.Name + " Indexer column";


                    if (UseRelationTable)
                    {
                        return _RDBMSClass.Name + " Indexer column";
                    }

                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToOne ||
                        SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToMany))
                    {

                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEnd = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                        if (!associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Indexer column";
                        if (SelectedRelationConcreteClass != null && !associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Indexer column";

                        if (associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Indexer column";
                        if (SelectedRelationConcreteClass != null && associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Indexer column";

                    }
                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany ||
                        SelectedAssociationMember.Association.LinkClass != null))
                    {
                        return _RDBMSClass.Name + " Indexer column";
                    }
                    if (StorageCellsLink != null && SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToOne)
                    {
                        if (SelectedAssociationMember.IsRoleA)
                        {
                            if (!ReferenceColumnsOnRoleB)
                                return _RDBMSClass.Name + " Indexer column";
                            else
                                return SelectedRelationConcreteClass.Name + " Indexer column";

                        }
                        else
                        {
                            if (ReferenceColumnsOnRoleB)
                                return _RDBMSClass.Name + " Indexer column";
                            else
                                return SelectedRelationConcreteClass.Name + " Indexer column";
                        }
                    }

                    return "Indexer column";
                }
            }
        }


        /// <MetaDataID>{8a601af8-73da-43d3-b23a-866e9513c6db}</MetaDataID>
        public string FirstRoleReferenceColumnLabel
        {
            get
            {
                if (SelectedAssociationMember == null)
                    return "Ref columns";
                else
                {
                    if (UseRelationTable)
                    {
                        return _RDBMSClass.Name + " Ref columns";
                    }

                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToOne ||
                        SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToMany))
                    {

                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEnd = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                        if (!associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Ref columns";
                        if (SelectedRelationConcreteClass != null && !associationEnd.IsRoleA && StorageCellsLink.RoleAStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Ref columns";

                        if (associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == _RDBMSClass.ActiveStorageCell)
                            return _RDBMSClass.Name + " Ref columns";
                        if (SelectedRelationConcreteClass != null && associationEnd.IsRoleA && StorageCellsLink.RoleBStorageCell == SelectedRelationConcreteClass.ActiveStorageCell)
                            return SelectedRelationConcreteClass.Name + " Ref columns";

                    }
                    if (StorageCellsLink != null && (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany ||
                        SelectedAssociationMember.Association.LinkClass != null))
                    {
                        return _RDBMSClass.Name + " Ref columns";
                    }
                    if (StorageCellsLink != null && SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToOne)
                    {
                        if (SelectedAssociationMember.IsRoleA)
                        {
                            if (!ReferenceColumnsOnRoleB)
                                return _RDBMSClass.Name + " Ref columns";
                            else
                                return SelectedRelationConcreteClass.Name + " Ref columns";

                        }
                        else
                        {
                            if (ReferenceColumnsOnRoleB)
                                return _RDBMSClass.Name + " Ref columns";
                            else
                                return SelectedRelationConcreteClass.Name + " Ref columns";
                        }
                    }

                    return "Ref columns";
                }
            }
        }
        /// <MetaDataID>{cf737def-a38d-4cec-8aa2-e666c041afc9}</MetaDataID>
        public bool UseRelationTable
        {
            get
            {
                if (StorageCellsLink == null)
                    return false;
                if ((SelectManualTableWithReferenceColumns &&
                    SelectedAssociationMember.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                    SelectedAssociationMember.Association.LinkClass == null))
                {
                    List<string> classMappedTableNames = new List<string>();
                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in _RDBMSClass.ActiveStorageCell.MappedTables)
                        classMappedTableNames.Add(table.Name);

                    List<string> reletadClassMappedTableNames = new List<string>();
                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in SelectedRelationConcreteClass.ActiveStorageCell.MappedTables)
                        reletadClassMappedTableNames.Add(table.Name);

                    if (TableWithReferenceColumns == null)

                        return false;
                    else if (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToOne &&
                        classMappedTableNames.Contains(TableWithReferenceColumns.Name)||
                        reletadClassMappedTableNames.Contains(TableWithReferenceColumns.Name))
                    {
                        return false;
                    }
                    else if (SelectedAssociationMember.Multiplicity.IsMany &&
                        reletadClassMappedTableNames.Contains(TableWithReferenceColumns.Name))
                    {
                        return false;
                    }
                    else if (SelectedAssociationMember.GetOtherEnd().Multiplicity.IsMany &&
                        classMappedTableNames.Contains(TableWithReferenceColumns.Name))
                    {
                        return false;
                    }
                    else
                        return true;

                }
                if (SelectedAssociationMember != null &&
                (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany ||
                SelectedAssociationMember.Association.LinkClass != null)
                    ||(StorageCellsLink as OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink).ObjectLinksTable!=null)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{70642668-bd3b-4b67-8828-4e388ee0bb70}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.RDBMSDataObjects.Column> CandidateReferenceColumns
        {
            get
            {
                List<OOAdvantech.RDBMSDataObjects.Column> columns = new List<OOAdvantech.RDBMSDataObjects.Column>();
                if (_TableWithReferenceColumns != null && StorageCellsLink != null && SelectedAssociationMember != null)
                {
                    if (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                        SelectedAssociationMember.Association.LinkClass == null)
                    {
                        foreach (OOAdvantech.RDBMSDataObjects.Column column in _TableWithReferenceColumns.Columns)
                        {
                            foreach (OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn in (StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                            {
                                if (DataBase.TypeDictionary.GetDBType(identityColumn.Type.FullName) == column.Datatype)
                                {
                                    if (!columns.Contains(column))
                                        columns.Add(column);
                                    break;
                                }
                            }
                        }
                        foreach (OOAdvantech.RDBMSDataObjects.Column column in _TableWithReferenceColumns.Columns)
                        {
                            foreach (OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn in (StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                            {
                                if (DataBase.TypeDictionary.GetDBType(identityColumn.Type.FullName) == column.Datatype)
                                {
                                    if (!columns.Contains(column))
                                        columns.Add(column);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEnd = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                        if (associationEnd.IsRoleA)
                        {
                            foreach (OOAdvantech.RDBMSDataObjects.Column column in _TableWithReferenceColumns.Columns)
                            {
                                foreach (OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn in (StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                                {
                                    if (DataBase.TypeDictionary.GetDBType(identityColumn.Type.FullName) == column.Datatype)
                                    {
                                        columns.Add(column);
                                        break;
                                    }
                                }
                            }
                        }
                        if (!associationEnd.IsRoleA)
                        {
                            foreach (OOAdvantech.RDBMSDataObjects.Column column in _TableWithReferenceColumns.Columns)
                            {
                                foreach (OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn in (StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                                {
                                    if (DataBase.TypeDictionary.GetDBType(identityColumn.Type.FullName) == column.Datatype)
                                    {
                                        columns.Add(column);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                return columns;

            }
        }

        /// <MetaDataID>{5a61e1ab-4250-4773-b3dd-8d5fadf45fc3}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.RDBMSDataObjects.Table> CandidateTableWithReferenceColumns
        {
            get
            {
                if (SelectedRelationConcreteClass != null && SelectedAssociationMember != null)
                {
                    if (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany || SelectManualTableWithReferenceColumns)
                        return new List<OOAdvantech.RDBMSDataObjects.Table>(DataBase.Tables);
                    else
                    {
                        List<OOAdvantech.RDBMSDataObjects.Table> tables = new List<OOAdvantech.RDBMSDataObjects.Table>();
                        if (SelectedAssociationMember.Multiplicity.IsMany)
                        {
                            foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in SelectedRelationConcreteClass.ActiveStorageCell.MappedTables)
                                tables.Add(DataBase.GetTable(table.Name));
                            return tables;
                        }
                        else if (SelectedAssociationMember.GetOtherEnd().Multiplicity.IsMany)
                        {
                            foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in _RDBMSClass.ActiveStorageCell.MappedTables)
                                tables.Add(DataBase.GetTable(table.Name));
                            return tables;
                        }
                        else if (SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToOne)
                        {
                            if (SelectedAssociationMember.IsRoleA)
                            {
                                if (!ReferenceColumnsOnRoleB)
                                {
                                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in SelectedRelationConcreteClass.ActiveStorageCell.MappedTables)
                                        tables.Add(DataBase.GetTable(table.Name));
                                }
                                else
                                {

                                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in _RDBMSClass.ActiveStorageCell.MappedTables)
                                        tables.Add(DataBase.GetTable(table.Name));

                                }
                            }
                            else
                            {
                                if (ReferenceColumnsOnRoleB)
                                {
                                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in SelectedRelationConcreteClass.ActiveStorageCell.MappedTables)
                                        tables.Add(DataBase.GetTable(table.Name));
                                }
                                else
                                {

                                    foreach (OOAdvantech.RDBMSMetaDataRepository.Table table in _RDBMSClass.ActiveStorageCell.MappedTables)
                                        tables.Add(DataBase.GetTable(table.Name));

                                }
                            }
                            return tables;
                        }
                        return tables;
                    }
                }
                return new List<OOAdvantech.RDBMSDataObjects.Table>();
            }
        }


        /// <MetaDataID>{7ca6c772-01b4-44be-85cd-d3653361937a}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.Table _MainTable;
        /// <MetaDataID>{142d8329-3a61-4cec-8867-6ce8dc9df60f}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Table MainTable
        {
            get
            {
                if (DataBase != null && _RDBMSClass != null && _RDBMSClass.ActiveStorageCell != null && _RDBMSClass.ActiveStorageCell.MappedTables.Count > 0)
                    _MainTable = DataBase.GetTable(_RDBMSClass.ActiveStorageCell.MainTable.Name);
                return _MainTable;
            }
            set
            {
                _MainTable = value;
                if (_RDBMSClass.ActiveStorageCell.MappedTables.Count == 0 || _RDBMSClass.ActiveStorageCell.MainTable.Name != _MainTable.Name)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(_SelectedRDBMSMappingContext.ObjectStorage);
                    List<OOAdvantech.RDBMSMetaDataRepository.Table> tables = new List<OOAdvantech.RDBMSMetaDataRepository.Table>(
                                          from table in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.Table>()
                                          where table.Name == _MainTable.Name
                                          select table);

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        if (tables.Count == 0)
                            _RDBMSClass.ActiveStorageCell.MainTable = new OOAdvantech.RDBMSMetaDataRepository.Table(_MainTable.Name, _RDBMSClass.ActiveStorageCell);
                        else
                            _RDBMSClass.ActiveStorageCell.MainTable = tables[0];
                        SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(_RDBMSClass.ActiveStorageCell.MainTable);
                        stateTransition.Consistent = true;
                    }
                    SelectedRDBMSMappingContext.Save();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.RDBMSDataObjects.Column _ObjectIDMappedColumn;
        /// <MetaDataID>{7979bf7a-66f9-4451-9557-ca6d3bf850fb}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Column ObjectIDMappedColumn
        {
            get
            {
                if (_ObjectIDMappedColumn == null && _MainTable != null && DataBase != null && _RDBMSClass != null && _RDBMSClass.ActiveStorageCell != null && _RDBMSClass.ActiveStorageCell.MappedTables.Count > 0)
                    if (_RDBMSClass.ActiveStorageCell.MainTable.ObjectIDColumns.Count > 0)
                        _ObjectIDMappedColumn = _MainTable.GetColumn(_RDBMSClass.ActiveStorageCell.MainTable.ObjectIDColumns[0].Name);

                return _ObjectIDMappedColumn;
            }
            set
            {
                _ObjectIDMappedColumn = value;
                try
                {
                    OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack(true);
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                        {
                            //OOAdvantech.RDBMSMetaDataRepository.Attribute attribute = (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(SelectedMember.MetaObject) as OOAdvantech.RDBMSMetaDataRepository.Attribute;
                            System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> columns = new List<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn>(_RDBMSClass.ActiveStorageCell.MainTable.ObjectIDColumns);
                            foreach (OOAdvantech.RDBMSMetaDataRepository.IdentityColumn column in columns)
                                _RDBMSClass.ActiveStorageCell.MainTable.RemoveObjectIDColumn(column);
                            System.Type dataType = DataBase.TypeDictionary.GetTypeForDBType(_ObjectIDMappedColumn.Datatype);
                            if (value != null)
                                _RDBMSClass.ActiveStorageCell.MainTable.AddObjectIDColumn(_ObjectIDMappedColumn.Name, dataType, _ObjectIDMappedColumn.Length, _ObjectIDMappedColumn.IdentityColumn, _ObjectIDMappedColumn.IdentityIncrement);
                            stateTransition.Consistent = true;
                        }
                    }
                    finally
                    {
                        OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                    }
                    SelectedRDBMSMappingContext.Save();
                }
                catch (Exception error)
                {
                }
            }
        }

        /// <MetaDataID>{acc41416-7a6b-41b3-adad-861fcc81ee4d}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.Column _SecodRoleReferenceColumn;
        /// <MetaDataID>{16431dd1-3c98-4e1c-acd4-8e7af1bfe799}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Column SecondRoleReferenceColumn
        {
            get
            {
                if (!UseRelationTable)
                    return null;

                if (SelectedAssociationMember != null && StorageCellsLink != null && SelectedRelationConcreteClass != null)
                {
                    OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> columns = null;
                    if (!UseRelationTable&& StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                    {
                        if (!SelectedAssociationMember.Multiplicity.IsMany && SelectedAssociationMember.GetOtherEnd().Multiplicity.IsMany)
                        {
                            if (SelectedAssociationMember.GetOtherEnd().IsRoleA)
                                columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                            else
                                columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        }
                        else
                        {
                            if (SelectedAssociationMember.IsRoleA)

                                columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                            else
                                columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        }
                    }
                    else
                    {
                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                        if (SelectedRelationConcreteClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                        else
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                        columns = associationEndWithColumns.GetReferenceColumnsFor(StorageCellsLink.ObjectLinksTable);

                    }
                    if (columns.Count > 0)
                    {

                        foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in columns)
                        {
                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, column.MappedAssociationEnd.Role);
                            if (column.CreatorIdentity == pathIdentity.ToString())
                            {
                                _TableWithReferenceColumns = DataBase.GetTable(column.Namespace.Name);
                                if (_TableWithReferenceColumns != null)
                                    _SecodRoleReferenceColumn = _TableWithReferenceColumns.GetColumn(column.Name);
                                else
                                {
                                    _SecodRoleReferenceColumn = null;
                                    _FirstRoleReferenceColumn = null;
                                }

                            }
                        }
                    }
                    else
                        _SecodRoleReferenceColumn = null;

                }
                return _SecodRoleReferenceColumn;
            }
            set
            {
                if (_SecodRoleReferenceColumn != value)
                {
                    _SecodRoleReferenceColumn = value;
                    if (SelectedAssociationMember == null)
                        return;

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {

                        if (!UseRelationTable&&StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            associationEndWithColumns = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveReferenceColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {
                                OOAdvantech.RDBMSMetaDataRepository.Table table = null;
                                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in _RDBMSClass.ActiveStorageCell.Namespace.OwnedElements)
                                {
                                    if (metaObject is OOAdvantech.RDBMSMetaDataRepository.Table && metaObject.Name == TableWithReferenceColumns.Name)
                                    {
                                        table = metaObject as OOAdvantech.RDBMSMetaDataRepository.Table;
                                        break;
                                    }
                                }
                                OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn refIdentityColumn = null;
                                if (column != null)
                                {
                                    if (!(column is OOAdvantech.RDBMSMetaDataRepository.IdentityColumn))
                                    {
                                        table.RemoveColumn(column);
                                        column = null;
                                    }
                                    refIdentityColumn = column as OOAdvantech.RDBMSMetaDataRepository.IdentityColumn;
                                }
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn = null;
                                if (SelectedAssociationMember.IsRoleA)
                                    identityColumn = (StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns[0];
                                else
                                    identityColumn = (StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns[0];

                                if (refIdentityColumn == null)
                                {
                                    refIdentityColumn = new OOAdvantech.RDBMSMetaDataRepository.IdentityColumn(_SecodRoleReferenceColumn.Name, identityColumn.Type, identityColumn.ColumnType, false);
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();

                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(refIdentityColumn);
                                    table.AddColumn(refIdentityColumn);
                                }
                                else
                                {
                                    refIdentityColumn.ColumnType = identityColumn.ColumnType;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();
                                    refIdentityColumn.AllowNulls = true;
                                }

                                associationEndWithColumns.AddReferenceColumn(refIdentityColumn);
                            }
                        }
                        else
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            if (SelectedRelationConcreteClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                            else
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveReferenceColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {
                                OOAdvantech.RDBMSMetaDataRepository.Table table = StorageCellsLink.ObjectLinksTable;

                                OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn refIdentityColumn = null;
                                if (column != null)
                                {
                                    if (!(column is OOAdvantech.RDBMSMetaDataRepository.IdentityColumn))
                                    {
                                        table.RemoveColumn(column);
                                        column = null;
                                    }
                                    refIdentityColumn = column as OOAdvantech.RDBMSMetaDataRepository.IdentityColumn;
                                }
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn = null;

                                identityColumn = SelectedRelationConcreteClass.ActiveStorageCell.MainTable.ObjectIDColumns[0];

                                if (refIdentityColumn == null)
                                {
                                    refIdentityColumn = new OOAdvantech.RDBMSMetaDataRepository.IdentityColumn(_SecodRoleReferenceColumn.Name, identityColumn.Type, identityColumn.ColumnType, false);
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();

                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(refIdentityColumn);
                                    table.AddColumn(refIdentityColumn);
                                }
                                else
                                {
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();
                                    refIdentityColumn.ColumnType = identityColumn.ColumnType;
                                }

                                associationEndWithColumns.AddReferenceColumn(refIdentityColumn);
                            }
                        }

                        stateTransition.Consistent = true;
                    }
                    SelectedRDBMSMappingContext.Save();
                }

            }
        }

        /// <MetaDataID>{e26ee8d4-5c18-4172-998b-a0fc7cf4c15e}</MetaDataID>
        public bool FirstRoleHasIndexerColumn
        {
            get
            {
                if (SelectedAssociationMember != null)
                    if (SelectedAssociationMember.Indexer)
                        return true;
                return false;
            }
        }


        /// <MetaDataID>{5866a148-e6a4-4e0f-b594-8c1441f26a57}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.Column _FirstRoleIndexerColumn;
        /// <MetaDataID>{365e4f6d-c546-4013-87cb-4384dd71dbb8}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Column FirstRoleIndexerColumn
        {
            get
            {
                if (SelectedAssociationMember != null && StorageCellsLink != null)
                {

                    if (!UseRelationTable && StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                    {
                        if (SelectedAssociationMember.Multiplicity.IsMany && TableWithReferenceColumns != null)
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithReferenceColumns=StorageCellsLink.GetAssociationEndWithReferenceColumns();
                            OOAdvantech.RDBMSMetaDataRepository.Column column = null;
                            if(associationEndWithReferenceColumns.IsRoleA)
                                column = associationEndWithReferenceColumns.GetIndexerColumnFor(StorageCellsLink.RoleAStorageCell);
                            else
                                column = associationEndWithReferenceColumns.GetIndexerColumnFor(StorageCellsLink.RoleBStorageCell);
                            if(column!=null)
                                _FirstRoleIndexerColumn = TableWithReferenceColumns.GetColumn(column.Name);

                        }

                        //if (!SelectedAssociationMember.Multiplicity.IsMany && SelectedAssociationMember.GetOtherEnd().Multiplicity.IsMany)
                        //{
                        //    if (SelectedAssociationMember.GetOtherEnd().IsRoleA)
                        //        columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //    else
                        //        columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //}
                        //else
                        //{
                        //    if (SelectedAssociationMember.IsRoleA)
                        //    {
                        //        columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //        if (columns.Count == 0)
                        //        {
                        //            columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //            if (columns.Count > 0)
                        //                ReferenceColumnsOnRoleB = true;
                        //        }

                        //    }
                        //    else
                        //    {
                        //        columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //        if (columns.Count == 0)
                        //            columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //        else
                        //            ReferenceColumnsOnRoleB = true;

                        //        //columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        //    }
                        //}
                    }
                    else
                    {
                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                        if (_RDBMSClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                        else
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                        OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                        OOAdvantech.RDBMSMetaDataRepository.Column column = associationEndWithColumns.GetIndexerColumnFor(StorageCellsLink.ObjectLinksTable);
                        if (column != null)
                            _FirstRoleIndexerColumn = _TableWithReferenceColumns.GetColumn(column.Name);
                        else
                            _FirstRoleIndexerColumn = null;
                    }

                }
                else
                {
                    _FirstRoleIndexerColumn = null;
                }
                return _FirstRoleIndexerColumn;
            }
            set
            {
                if (_FirstRoleIndexerColumn != value)
                {
                    _FirstRoleIndexerColumn = value;
                    if (SelectedAssociationMember == null)
                        return;

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        if (!UseRelationTable&&StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            associationEndWithColumns = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                            if (ReferenceColumnsOnRoleB && associationEndWithColumns.IsRoleA)
                                associationEndWithColumns = associationEndWithColumns.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveIndexerColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {

                                OOAdvantech.RDBMSMetaDataRepository.Table table = null;
                                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in _RDBMSClass.ActiveStorageCell.Namespace.OwnedElements)
                                {
                                    if (metaObject is OOAdvantech.RDBMSMetaDataRepository.Table && metaObject.Name == TableWithReferenceColumns.Name)
                                    {
                                        table = metaObject as OOAdvantech.RDBMSMetaDataRepository.Table;
                                        break;
                                    }
                                }
                                //OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.Column indexerColumn = table.GetColumn(value.Name);
                                if (indexerColumn == null)
                                {
                                    OOAdvantech.MetaDataRepository.Primitive systemInt32Type = (table.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(int))) as OOAdvantech.MetaDataRepository.Primitive;
                                    if (systemInt32Type == null || systemInt32Type.Name == null)
                                    {
                                        OOAdvantech.MetaDataRepository.Primitive mPrimitive = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(int)) as OOAdvantech.MetaDataRepository.Primitive;
                                        systemInt32Type = new OOAdvantech.RDBMSMetaDataRepository.Primitive();
                                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(systemInt32Type);
                                        systemInt32Type.Synchronize(mPrimitive);
                                    }
                                    indexerColumn = new OOAdvantech.RDBMSMetaDataRepository.Column(_FirstRoleIndexerColumn.Name, systemInt32Type, _FirstRoleIndexerColumn.Length, _FirstRoleIndexerColumn.AllowNulls, _FirstRoleIndexerColumn.IdentityColumn, _FirstRoleIndexerColumn.IdentityIncrement);
                                    indexerColumn.AllowNulls = true;
                                    indexerColumn.CreatorIdentity = pathIdentity.ToString();
                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(indexerColumn);
                                    table.AddColumn(indexerColumn);
                                }
                                else
                                {
                                    indexerColumn.AllowNulls = true;
                                    indexerColumn.CreatorIdentity = pathIdentity.ToString();
                                }
                                associationEndWithColumns.AddIndexerColumn(indexerColumn);
                            }
                        }
                        else
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            if (_RDBMSClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                            else
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveIndexerColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {
                                OOAdvantech.RDBMSMetaDataRepository.Table table = StorageCellsLink.ObjectLinksTable;

                                OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.Column indexerColumn = null;
                                if (column != null)
                                {
                                    if (!(column.Type.FullName!=typeof(int).FullName))
                                    {
                                        table.RemoveColumn(column);
                                        column = null;
                                    }
                                    indexerColumn = column as OOAdvantech.RDBMSMetaDataRepository.IdentityColumn;
                                }

                                if (indexerColumn == null)
                                {
                                    OOAdvantech.MetaDataRepository.Primitive systemInt32Type = (table.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(int))) as OOAdvantech.MetaDataRepository.Primitive;
                                    if (systemInt32Type == null || systemInt32Type.Name == null)
                                    {
                                        OOAdvantech.MetaDataRepository.Primitive mPrimitive=OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(int)) as OOAdvantech.MetaDataRepository.Primitive;
                                        systemInt32Type = new OOAdvantech.RDBMSMetaDataRepository.Primitive();
                                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(systemInt32Type);
                                        systemInt32Type.Synchronize(mPrimitive);
                                    }
                                    indexerColumn = new OOAdvantech.RDBMSMetaDataRepository.Column(_FirstRoleIndexerColumn.Name, systemInt32Type, _FirstRoleIndexerColumn.Length, _FirstRoleIndexerColumn.AllowNulls, _FirstRoleIndexerColumn.IdentityColumn, _FirstRoleIndexerColumn.IdentityIncrement);
                                    indexerColumn.AllowNulls = true;
                                    indexerColumn.CreatorIdentity = pathIdentity.ToString();
                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(indexerColumn);
                                    table.AddColumn(indexerColumn);
                                }
                                else
                                {
                                    indexerColumn.AllowNulls = true;
                                    indexerColumn.CreatorIdentity = pathIdentity.ToString();
                                }
                                associationEndWithColumns.AddIndexerColumn(indexerColumn);
                            }
                        }
                        stateTransition.Consistent = true;
                    }
                    SelectedRDBMSMappingContext.Save();
                }

            }
        }


        /// <MetaDataID>{127dc915-17ea-4125-b135-72e6c6fbada8}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.Column _FirstRoleReferenceColumn;
        /// <MetaDataID>{df167440-4030-4929-a8ce-463a816814db}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Column FirstRoleReferenceColumn
        {
            get
            {
                if (SelectedAssociationMember != null && StorageCellsLink != null)
                {
                    OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> columns = null;
                    if (!UseRelationTable && StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                    {
                        if (!SelectedAssociationMember.Multiplicity.IsMany && SelectedAssociationMember.GetOtherEnd().Multiplicity.IsMany)
                        {
                            if (SelectedAssociationMember.GetOtherEnd().IsRoleA)
                                columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                            else
                                columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                        }
                        else
                        {
                            if (SelectedAssociationMember.IsRoleA)
                            {
                                columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                                if (columns.Count == 0)
                                {
                                    columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                                    if (columns.Count > 0)
                                        ReferenceColumnsOnRoleB = true;
                                }

                            }
                            else
                            {
                                columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                                if (columns.Count == 0)
                                    columns = (SelectedAssociationMember.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                                else
                                    ReferenceColumnsOnRoleB = true;

                                //columns = SelectedAssociationMember.GetReferenceColumnsFor(StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                            }
                        }
                    }
                    else
                    {
                        OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                        if (_RDBMSClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                        else
                            associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                        columns = associationEndWithColumns.GetReferenceColumnsFor(StorageCellsLink.ObjectLinksTable);
                    }
                    if (columns.Count > 0)
                    {

                        foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in columns)
                        {
                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, column.MappedAssociationEnd.Role);
                            if (column.CreatorIdentity == pathIdentity.ToString())
                            {
                                _TableWithReferenceColumns = DataBase.GetTable(column.Namespace.Name);
                                if (_TableWithReferenceColumns != null)
                                    _FirstRoleReferenceColumn = _TableWithReferenceColumns.GetColumn(column.Name);
                                else
                                {
                                    _SecodRoleReferenceColumn = null;
                                    _FirstRoleReferenceColumn = null;
                                }

                            }
                        }
                    }
                    else
                        _FirstRoleReferenceColumn = null;

                }
                else
                {
                    _FirstRoleReferenceColumn = null;
                }
                return _FirstRoleReferenceColumn;
            }
            set
            {
                if (_FirstRoleReferenceColumn != value)
                {
                    _FirstRoleReferenceColumn = value;
                    if (SelectedAssociationMember == null)
                        return;

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        if (!UseRelationTable && StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            associationEndWithColumns = (SelectedAssociationMember.Association as OOAdvantech.RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
                            if (ReferenceColumnsOnRoleB && associationEndWithColumns.IsRoleA)
                                associationEndWithColumns = associationEndWithColumns.GetOtherEnd() as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveReferenceColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {

                                OOAdvantech.RDBMSMetaDataRepository.Table table = null;
                                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in _RDBMSClass.ActiveStorageCell.Namespace.OwnedElements)
                                {
                                    if (metaObject is OOAdvantech.RDBMSMetaDataRepository.Table && metaObject.Name == TableWithReferenceColumns.Name)
                                    {
                                        table = metaObject as OOAdvantech.RDBMSMetaDataRepository.Table;
                                        break;
                                    }
                                }
                                OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn refIdentityColumn = null;
                                if (column != null)
                                {
                                    if (!(column is OOAdvantech.RDBMSMetaDataRepository.IdentityColumn))
                                    {
                                        table.RemoveColumn(column);
                                        column = null;
                                    }
                                    refIdentityColumn = column as OOAdvantech.RDBMSMetaDataRepository.IdentityColumn;
                                }
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn = null;
                                if (SelectedAssociationMember.IsRoleA)
                                    identityColumn = (StorageCellsLink.RoleBStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns[0];
                                else
                                    identityColumn = (StorageCellsLink.RoleAStorageCell as OOAdvantech.RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns[0];

                                if (refIdentityColumn == null)
                                {
                                    refIdentityColumn = new OOAdvantech.RDBMSMetaDataRepository.IdentityColumn(_FirstRoleReferenceColumn.Name, identityColumn.Type, identityColumn.ColumnType, false);
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();

                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(refIdentityColumn);
                                    table.AddColumn(refIdentityColumn);
                                }
                                else
                                {
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.ColumnType = identityColumn.ColumnType;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();

                                }

                                associationEndWithColumns.AddReferenceColumn(refIdentityColumn);
                            }
                        }
                        else
                        {
                            OOAdvantech.RDBMSMetaDataRepository.AssociationEnd associationEndWithColumns = null;
                            if (_RDBMSClass.ActiveStorageCell == StorageCellsLink.RoleAStorageCell)
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleB as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;
                            else
                                associationEndWithColumns = SelectedAssociationMember.Association.RoleA as OOAdvantech.RDBMSMetaDataRepository.AssociationEnd;

                            OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AssociationTreeNode, associationEndWithColumns.Role);
                            associationEndWithColumns.RemoveReferenceColumsFor(this.StorageCellsLink, pathIdentity);
                            if (value != null)
                            {
                                OOAdvantech.RDBMSMetaDataRepository.Table table = StorageCellsLink.ObjectLinksTable;

                                OOAdvantech.RDBMSMetaDataRepository.Column column = table.GetColumn(value.Name);
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn refIdentityColumn = null;
                                if (column != null)
                                {
                                    if (!(column is OOAdvantech.RDBMSMetaDataRepository.IdentityColumn))
                                    {
                                        table.RemoveColumn(column);
                                        column = null;
                                    }
                                    refIdentityColumn = column as OOAdvantech.RDBMSMetaDataRepository.IdentityColumn;
                                }
                                OOAdvantech.RDBMSMetaDataRepository.IdentityColumn identityColumn = null;
                                identityColumn = _RDBMSClass.ActiveStorageCell.MainTable.ObjectIDColumns[0];

                                if (refIdentityColumn == null)
                                {
                                    refIdentityColumn = new OOAdvantech.RDBMSMetaDataRepository.IdentityColumn(_FirstRoleReferenceColumn.Name, identityColumn.Type, identityColumn.ColumnType, false);
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();

                                    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(refIdentityColumn);
                                    table.AddColumn(refIdentityColumn);
                                }
                                else
                                {
                                    refIdentityColumn.AllowNulls = true;
                                    refIdentityColumn.ColumnType = identityColumn.ColumnType;
                                    refIdentityColumn.CreatorIdentity = pathIdentity.ToString();
                                }

                                associationEndWithColumns.AddReferenceColumn(refIdentityColumn);
                            }
                        }

                        stateTransition.Consistent = true;
                    }
                    SelectedRDBMSMappingContext.Save();
                }

            }
        }


        /// <MetaDataID>{5b5e2799-5868-425c-a74b-fdb7185cf18b}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.Structure GetMappinStructure(OOAdvantech.MetaDataRepository.Structure structure)
        {
            OOAdvantech.RDBMSMetaDataRepository.Structure RDBMSStructure = (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(structure) as OOAdvantech.RDBMSMetaDataRepository.Structure;
            if (RDBMSStructure == null || RDBMSStructure.Features.Count == 0)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(TransactionOption.RequiresNew))
                {

                    try
                    {
                        OOAdvantech.Transactions.Transaction trans = OOAdvantech.Transactions.Transaction.Current;
                        OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack(true);
                        OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                        if (RDBMSStructure == null)
                            RDBMSStructure = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(RealObject, _SelectedRDBMSMappingContext.ObjectStorage.NewObject<OOAdvantech.RDBMSMetaDataRepository.Class>()) as OOAdvantech.RDBMSMetaDataRepository.Structure;
                        RDBMSStructure.Synchronize(structure);
                        //RDBMSStructure.ImplementationUnit.Name = _RDBMSClass.Name;
                        //if (RDBMSStructure.ImplementationUnit.Context == null)
                        //{
                        //    _RDBMSClass.ImplementationUnit.Context = new OOAdvantech.RDBMSMetaDataRepository.Storage("MapingStorage");
                        //    SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(_RDBMSClass.ImplementationUnit.Context);
                        //}


                        OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                        OOAdvantech.MetaDataRepository.StorageCell activeStorageCell = _RDBMSClass.ActiveStorageCell;
                        stateTransition.Consistent = true; ;
                    }
                    catch (System.Exception Error)
                    {
                        throw;
                    }
                }

                _SelectedRDBMSMappingContext.Save();
                (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).Refresh();

            }
            return RDBMSStructure;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.RDBMSDataObjects.Column _MappedColumn;
        /// <MetaDataID>{f5c5f14d-26c6-4aae-9ddc-68cf43c716b4}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Column MappedColumn
        {
            get
            {

                return _MappedColumn;
            }
            set
            {
                _MappedColumn = value;
                try
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                    {
                        if ((SelectedMember.MetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner is OOAdvantech.MetaDataRepository.Structure)
                        {
                            OOAdvantech.RDBMSMetaDataRepository.Structure structure = GetMappinStructure((SelectedMember.MetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner as OOAdvantech.MetaDataRepository.Structure);
                        }
                        OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AttributeTreeNode);
                        OOAdvantech.RDBMSMetaDataRepository.Attribute attribute = (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(SelectedMember.MetaObject) as OOAdvantech.RDBMSMetaDataRepository.Attribute;
                        System.Collections.Generic.List<OOAdvantech.RDBMSMetaDataRepository.Column> columns = attribute.GetColumnsFor(_RDBMSClass.ActiveStorageCell);
                        foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in columns)
                            if (column.CreatorIdentity == pathIdentity.ToString())
                            {
                                attribute.RemoveColumn(column);
                                //(column.Namespace as OOAdvantech.RDBMSMetaDataRepository.Table).RemoveColumn(column);
                            }
                        if (value != null)
                        {



                            OOAdvantech.RDBMSMetaDataRepository.Column column = _RDBMSClass.ActiveStorageCell.MainTable.GetColumn(value.Name);
                            if (column == null)
                            {
                                column = attribute.AddColumnToTableOrUpdate(_RDBMSClass.ActiveStorageCell.MainTable, value.Name,false);
                                
                            }
                            else if (column.MappedAttribute != attribute)
                            {
                                if(column.MappedAttribute!=null)
                                    column.MappedAttribute.RemoveColumn(column);
                                column = attribute.AddColumnToTableOrUpdate(_RDBMSClass.ActiveStorageCell.MainTable, value.Name, false);
                            }


                            column.CreatorIdentity = pathIdentity.ToString();
                            column.IsIdentity = _MappedColumn.IdentityColumn;
                            column.IdentityIncrement = _MappedColumn.IdentityIncrement;
                            column.AllowNulls = _MappedColumn.AllowNulls;
                            column.Length = _MappedColumn.Length;

                        }
                        stateTransition.Consistent = true;
                    }
                    SelectedRDBMSMappingContext.Save();


                }
                catch (Exception error)
                {


                }
            }
        }

        /// <MetaDataID>{5aa21df3-d591-4263-a08a-a21384f59bb0}</MetaDataID>
        private OOAdvantech.MetaDataRepository.ValueTypePath GetValueTypePath(AttributeTreeNode attributeTreeNode)
        {
            OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            List<OOAdvantech.MetaDataRepository.MetaObjectID> identities = new List<OOAdvantech.MetaDataRepository.MetaObjectID>();
            if ((attributeTreeNode.MetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner is OOAdvantech.MetaDataRepository.Structure &&
            attributeTreeNode.Parent.Parent.MetaObject is OOAdvantech.MetaDataRepository.Attribute)
            {
                identities.Insert(0, attributeTreeNode.MetaObject.Identity);
            }

            while ((attributeTreeNode.MetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner is OOAdvantech.MetaDataRepository.Structure &&
                attributeTreeNode.Parent.Parent.MetaObject is OOAdvantech.MetaDataRepository.Attribute)
            {

                identities.Insert(0, attributeTreeNode.Parent.Parent.MetaObject.Identity);
                attributeTreeNode = attributeTreeNode.Parent.Parent as AttributeTreeNode;
            }
            foreach (OOAdvantech.MetaDataRepository.MetaObjectID identity in identities)
                valueTypePath.Push(identity);
            return valueTypePath;

        }
        /// <MetaDataID>{b029126f-5301-445d-a23e-e842a0f6148e}</MetaDataID>
        private OOAdvantech.MetaDataRepository.ValueTypePath GetValueTypePath(AssociationTreeNode associationTreeNode, OOAdvantech.MetaDataRepository.Roles role)
        {
            AttributeTreeNode attributeTreeNode = null;
            OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            List<OOAdvantech.MetaDataRepository.MetaObjectID> identities = new List<OOAdvantech.MetaDataRepository.MetaObjectID>();
            if ((associationTreeNode.MetaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Namespace is OOAdvantech.MetaDataRepository.Structure &&
                associationTreeNode.Parent.Parent.MetaObject is OOAdvantech.MetaDataRepository.Attribute)
            {
                if (role == OOAdvantech.MetaDataRepository.Roles.RoleA)
                    identities.Insert(0, (associationTreeNode.MetaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Association.RoleA.Identity);
                else
                    identities.Insert(0, (associationTreeNode.MetaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Association.RoleB.Identity);
                identities.Insert(0, associationTreeNode.Parent.Parent.MetaObject.Identity);
                attributeTreeNode = associationTreeNode.Parent.Parent as AttributeTreeNode;
            }
            if (attributeTreeNode != null)
            {
                while ((attributeTreeNode.MetaObject as OOAdvantech.MetaDataRepository.Attribute).Owner is OOAdvantech.MetaDataRepository.Structure &&
                         attributeTreeNode.Parent.Parent.MetaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    identities.Insert(0, attributeTreeNode.Parent.MetaObject.Identity);
                    attributeTreeNode = attributeTreeNode.Parent as AttributeTreeNode;
                }
            }
            foreach (OOAdvantech.MetaDataRepository.MetaObjectID identity in identities)
                valueTypePath.Push(identity);
            return valueTypePath;

        }


        /// <MetaDataID>{e45d6bca-7a62-4445-acbf-b78803015158}</MetaDataID>
        bool OnReferenceColumnsOnRoleBSet ;
        /// <MetaDataID>{b0deef60-ed5c-48b4-befa-845f3eecc77d}</MetaDataID>
        bool _ReferenceColumnsOnRoleB;
        /// <MetaDataID>{5b260c50-42df-4b03-9917-73b108cbad2d}</MetaDataID>
        public bool ReferenceColumnsOnRoleB 
        {
            get
            {
                return _ReferenceColumnsOnRoleB;
            }
            set
            {
                
                if (_ReferenceColumnsOnRoleB != value)
                {

                    if (!OnReferenceColumnsOnRoleBSet)
                    {
                        try
                        {
                            OnReferenceColumnsOnRoleBSet = true;
                            TableWithReferenceColumns = null;
                            _ReferenceColumnsOnRoleB = value;
                            if (ObjectChangeState != null)
                                ObjectChangeState(this, null);

                        }
                        finally
                        {
                            OnReferenceColumnsOnRoleBSet = false;
                        }
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        bool _SelectManualTableWithReferenceColumns;
        /// <MetaDataID>{89123945-ae74-4a87-b71f-134b248378dd}</MetaDataID>
        public bool SelectManualTableWithReferenceColumns
        {
            get
            {
                return _SelectManualTableWithReferenceColumns;
            }
            set
            {
                if (_SelectManualTableWithReferenceColumns != value)
                {
                    _SelectManualTableWithReferenceColumns = value;
                    if (!_SelectManualTableWithReferenceColumns && TableWithReferenceColumns != null)
                        if (!CandidateTableWithReferenceColumns.Contains(TableWithReferenceColumns))
                        {
                            if (StorageCellsLink != null &&
                                SelectedAssociationMember != null &&
                                SelectedAssociationMember.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                                StorageCellsLink.ObjectLinksTable != null)
                            {
                                StorageCellsLink.ObjectLinksTable = null;
                                _SelectedRDBMSMappingContext.Save();
                            }

                            _TableWithReferenceColumns = null;
                        }
                    if (!_SelectManualTableWithReferenceColumns && 
                        StorageCellsLink != null&&
                        SelectedAssociationMember.Association.LinkClass==null&&
                        SelectedAssociationMember.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany&&
                        StorageCellsLink.ObjectLinksTable != null)
                    {
                        StorageCellsLink.ObjectLinksTable = null;
                        _SelectedRDBMSMappingContext.Save();
                    }
                    if (_SelectManualTableWithReferenceColumns)
                        ReferenceColumnsOnRoleB = false;

                    ObjectChangeState(this, null);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MetaObjectTreeNode _SelectedMember;
        /// <MetaDataID>{424638c9-4ad8-467a-bd91-dcb9ae84a79c}</MetaDataID>
        public MetaObjectTreeNode SelectedMember
        {
            get
            {
                return _SelectedMember;
            }
            set
            {
                _SelectedMember = null;
                ReferenceColumnsOnRoleB = false;
                _MappedColumn = null;
                StorageCellsLink = null;
                SelectManualTableWithReferenceColumns = false;
                _SelectedMember = value;
                if (_SelectedMember == null)
                    return; 
                OOAdvantech.RDBMSMetaDataRepository.Attribute attribute = (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(SelectedMember.MetaObject) as OOAdvantech.RDBMSMetaDataRepository.Attribute;
                if (attribute != null && attribute.Type is OOAdvantech.MetaDataRepository.Structure && (attribute.Type as OOAdvantech.MetaDataRepository.Structure).Persistent)
                {
                    _SelectedMember = null;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);

                    return;
                }
                if (attribute != null && SelectionColumnsTable != null)
                {
                    OOAdvantech.MetaDataRepository.ValueTypePath pathIdentity = GetValueTypePath(SelectedMember as AttributeTreeNode);
                    foreach (OOAdvantech.RDBMSMetaDataRepository.Column column in attribute.GetColumnsFor(_RDBMSClass.ActiveStorageCell))
                    {
                        if (pathIdentity.ToString() == column.CreatorIdentity)
                        {
                            _MappedColumn = _MainTable.GetColumn(column.Name);
                            break;
                        }
                    }
                }

                if (!(_SelectedMember.MetaObject is OOAdvantech.MetaDataRepository.AssociationEnd))
                {
                    _SelectedRelationConcreteClass = null;
                    _TableWithReferenceColumns = null;
                    _SecodRoleReferenceColumn = null;
                }

                //if (StorageCellsLink != null &&
                //    StorageCellsLink.ObjectLinksTable != null &&
                //    StorageCellsLink.Type.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                //{
                //    SelectManualTableWithReferenceColumns = true;
                //}

                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }

        }

        /// <MetaDataID>{85b74fc2-fe31-46fa-88cb-b6eb3c7c931f}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.Table SelectionColumnsTable
        {
            get
            {
                if (_SelectedMember != null && (_SelectedMember.MetaObject is OOAdvantech.MetaDataRepository.Attribute) &&
                    RealObject.IsPersistent((_SelectedMember.MetaObject as OOAdvantech.MetaDataRepository.Attribute)))
                    return _MainTable;
                return null;
            }
        }


        /// <MetaDataID>{5ef62dd1-8172-42c9-ab49-76fbf4326e4f}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.Class _RDBMSClass;
        /// <MetaDataID>{2cb4cd53-d590-43c8-849f-9c5606fdcba2}</MetaDataID>
        RDBMSMappingContext _SelectedRDBMSMappingContext;
        /// <MetaDataID>{2763331c-ef72-406b-83f0-c736e6aa6845}</MetaDataID>
        public OOAdvantech.RDBMSDataObjects.DataBase DataBase;
        /// <MetaDataID>{fe19508c-8d4b-48b3-b8a0-5170dd19f93c}</MetaDataID>
        public RDBMSMappingContext SelectedRDBMSMappingContext
        {
            get
            {
                return _SelectedRDBMSMappingContext;
            }
            set
            {
                _SelectedRDBMSMappingContext = value;
                if (_SelectedRDBMSMappingContext == null)
                {
                    _DataBaseConnection = null;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);

                    return;
                }


                string query = "SELECT class FROM " + typeof(OOAdvantech.MetaDataRepository.Class).FullName + " class WHERE class.MetaObjectIDStream = \"" + RealObject.Identity.ToString() + "\"";
                OOAdvantech.Collections.StructureSet aStructureSet = _SelectedRDBMSMappingContext.ObjectStorage.Execute(query);

                foreach (OOAdvantech.Collections.StructureSet rowset in aStructureSet)
                {
                    _RDBMSClass = rowset["class"] as OOAdvantech.RDBMSMetaDataRepository.Class;
                    break;
                }
                if (_RDBMSClass == null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        try
                        {
                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack(true);
                            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                            _RDBMSClass = OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(RealObject, _SelectedRDBMSMappingContext.ObjectStorage.NewObject<OOAdvantech.RDBMSMetaDataRepository.Class>()) as OOAdvantech.RDBMSMetaDataRepository.Class;
                            _RDBMSClass.Synchronize(RealObject);
                            _RDBMSClass.ImplementationUnit.Name = _RDBMSClass.Name;
                            if (_RDBMSClass.ImplementationUnit.Context == null)
                            {
                                _RDBMSClass.ImplementationUnit.Context = new OOAdvantech.RDBMSMetaDataRepository.Storage("MapingStorage");
                                SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(_RDBMSClass.ImplementationUnit.Context);
                            }

                            _SelectedRDBMSMappingContext.Save();
                            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                            OOAdvantech.MetaDataRepository.StorageCell activeStorageCell = _RDBMSClass.ActiveStorageCell;
                            stateTransition.Consistent = true; 
                        }
                        catch (System.Exception Error)
                        {
                            throw;
                        }
                    }

                    _SelectedRDBMSMappingContext.Save();
                }
                else
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        try
                        {
                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.RDBMSMetaDataRepository.MetaObjectsStack(true);
                            
                            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                            _RDBMSClass.Synchronize(RealObject);
                            _RDBMSClass.ImplementationUnit.Name = _RDBMSClass.Name;
                            if (_RDBMSClass.ImplementationUnit.Context == null)
                            {
                                _RDBMSClass.ImplementationUnit.Context = new OOAdvantech.RDBMSMetaDataRepository.Storage("MapingStorage");
                                SelectedRDBMSMappingContext.ObjectStorage.CommitTransientObjectState(_RDBMSClass.ImplementationUnit.Context);
                            }

                            _SelectedRDBMSMappingContext.Save();
                            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
                            OOAdvantech.MetaDataRepository.StorageCell activeStorageCell = _RDBMSClass.ActiveStorageCell;
                            stateTransition.Consistent = true; ;
                        }
                        catch (System.Exception Error)
                        {
                            throw;
                        }
                    }

                    _SelectedRDBMSMappingContext.Save();

                }
                (_RDBMSClass.ActiveStorageCell.Namespace as OOAdvantech.RDBMSMetaDataRepository.Storage).Refresh();

                _DataBaseConnection = null;


                if (_SelectedRDBMSMappingContext != null && DataBaseConnection != null)
                    DataBase = OOAdvantech.RDBMSDataObjects.DataBase.GetDataBase(DataBaseConnection.ConnectionString, DataBaseConnection.RDBMSDataBaseType);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);

            }
        }
        /// <MetaDataID>{066091c3-ccdf-4d4d-a4aa-3cc6f46b14ae}</MetaDataID>
        public readonly VSMetadataRepositoryBrowser.ClassifierTreeNode ClassAsTreeNode;

        /// <MetaDataID>{4abd0a5e-2f2c-4955-8385-4a034e93e72e}</MetaDataID>
        OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection _DataBaseConnection;
        /// <MetaDataID>{a5bce892-70fd-48bc-b9b4-d6f97963400b}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection DataBaseConnection
        {
            get
            {
                if (_DataBaseConnection == null && _SelectedRDBMSMappingContext != null)
                {

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(_SelectedRDBMSMappingContext.ObjectStorage);
                    var dataBaseConnections = from dataBaseConnection in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection>()
                                              select dataBaseConnection;
                    foreach (OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection dataBaseConnection in dataBaseConnections)
                    {
                        _DataBaseConnection = dataBaseConnection;
                        break;
                    }
                }
                return _DataBaseConnection;
            }
        }

        /// <MetaDataID>{9a05b3af-6a83-413a-b45e-39fdc7ae73bd}</MetaDataID>
        public bool ChooseTableWithReferenceColumns
        {
            get
            {
                if (SelectedRelationConcreteClass != null)
                    return true;
                else
                    return false;
            }
        }
        /// <MetaDataID>{cb7512d4-775b-4cc8-ae4f-38c92132ade5}</MetaDataID>
        public bool ChooseRoleWithReferenceColumns
        {
            get
            {
                if (SelectManualTableWithReferenceColumns)
                    return false;
                if (SelectedAssociationMember != null &&
                    SelectedAssociationMember.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.OneToOne )
                    return true;

                return false;
            }
        }





    }
}
