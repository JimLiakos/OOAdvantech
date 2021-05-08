namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using System;
    /// <MetaDataID>{49481703-6ca2-4d67-9b14-7f179c2559a1}</MetaDataID>
    [Serializable]
    public class RootObjectDataSource : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource
    {
        protected RootObjectDataSource(Guid identity ):base(identity)
        {
        }
        protected internal override DataSource Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if(clonedObjects.ContainsKey(this))
                return clonedObjects[this] as RootObjectDataSource;
            RootObjectDataSource newDataSource = new RootObjectDataSource(Identity);

            clonedObjects[this] = newDataSource;
            newDataSource._HasLockRequest = _HasLockRequest;
            base.Copy(newDataSource, clonedObjects);


            return newDataSource;
        }
#if !DeviceDotNet
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("HasLockRequest", _HasLockRequest);
        }
        protected RootObjectDataSource(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            _HasLockRequest=(int)info.GetValue("HasLockRequest" ,typeof(int) );
        }
#endif

        /// <MetaDataID>{9e2c19a4-08c8-467a-a013-9e0114e95f2b}</MetaDataID>
        int _HasLockRequest = -1;
        /// <MetaDataID>{a9210f86-68d7-4fa5-b24f-26f8b7bf400c}</MetaDataID>
        internal bool HasLockRequest
        {
            get
            {
                if (_HasLockRequest == -1)
                {
                    foreach (DataNode dataNode in DataNode.SubDataNodes)
                    {
                        foreach (DataNode lockDataNode in dataNode.SubDataNodes)
                        {
                            if (lockDataNode.Name == "[Lock]")
                            {
                                _HasLockRequest = 1;
                                return true;
                            }
                        }
                    }
                    _HasLockRequest = 0;
                    return false;
                }
                return _HasLockRequest == 1;
            }
        }

        ///// <MetaDataID>{e34ce074-3aa1-4932-8dc1-c8687e2437bf}</MetaDataID>
        //internal override void LoadObjectRelationLinks(DataNode subDataNode)
        //{
            
        //}
        ///// <MetaDataID>{babef7b6-fd68-4c44-b282-acabba331b0e}</MetaDataID>
        //internal override void LoadObjectRelationLinksEx(DataNode subDataNode)
        //{
            
        //}
        /// <MetaDataID>{e30a6825-71f3-4b7b-8f8a-06ddebba1f9d}</MetaDataID>
        public override void BuildTablesRelations()
        {
            try
            {
                base.BuildTablesRelations();
                //foreach (DataNode subDataNode in DataNode.SubDataNodes)
                //{
                //    //if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type== DataNode.DataNodeType.Object
                //    //        && !DataTable.DataSet.Relations.Contains(subDataNode.Alias))
                //    //{

                //    //    if (subDataNode.DataSource.DataTable.Rows.Count == 0)
                //    //    {
                //    //        bool thereIsnotRowToPassFilter = false;
                //    //        if (subDataNode.BranchSearchCriteria.Count > 0)
                //    //            thereIsnotRowToPassFilter = true;

                //    //        foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in subDataNode.BranchSearchCriteria)
                //    //        {
                //    //            if (criterion.IsNULL || !criterion.ConstrainCriterion)
                //    //            {
                //    //                thereIsnotRowToPassFilter = false;
                //    //                break;
                //    //            }
                //    //        }
                //    //        if (thereIsnotRowToPassFilter)
                //    //        {
                //    //            DataTable.Clear();
                //    //            return;
                //    //        }
                //    //    }

                //    //    if (subDataNode.DataSource.DataTable == null || subDataNode.DataSource.DataTable.Rows.Count == 0)
                //    //        continue;


                //    //    System.Data.DataColumn[] parentDataColumns = null;
                //    //    System.Data.DataColumn[] childDataColumns = null;


                //    //    foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, System.Collections.Generic.List<string>> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity])
                //    //    {
                //    //        System.Collections.Generic.List<string> parentColumns = entry.Value;

                //    //        parentDataColumns = new System.Data.DataColumn[parentColumns.Count];
                //    //        childDataColumns = new System.Data.DataColumn[subDataNode.DataSource.DataSourceRelationsColumnsWithParent.Count];
                //    //        int i = 0;
                //    //        foreach (string columnName in parentColumns)
                //    //            parentDataColumns[i++] = DataTable.Columns[columnName];
                //    //        i = 0;

                //    //        foreach (string columnName in subDataNode.DataSource.DataSourceRelationsColumnsWithParent[entry.Key])
                //    //            childDataColumns[i++] = subDataNode.DataSource.DataTable.Columns[columnName];
                //    //        System.Data.DataRelation relation = new System.Data.DataRelation(subDataNode.Alias, childDataColumns, parentDataColumns, false);
                //    //        DataTable.DataSet.Relations.Add(relation);
                //    //        break;
                //    //    }
                //    //}
                //}
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{12393919-d55c-4352-a1d7-81960f6706a8}</MetaDataID>
        public RootObjectDataSource(DataNode dataNode)
            : base(dataNode)
        {
            //DataLoaders[dataNode.ObjectQuery.GetHashCode().ToString()] = new RootObjectDataLoader(dataNode);
            MetaDataRepository.AssociationEnd subDataNodeassociationEnd = dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            if (subDataNodeassociationEnd != null)
            {
                if (!dataNode.Recursive &&
                    ((subDataNodeassociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                    subDataNodeassociationEnd.Association != dataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation &&
                    subDataNodeassociationEnd.Association != dataNode.Classifier.ClassHierarchyLinkAssociation) ||
                    subDataNodeassociationEnd.Association.Specializations.Count > 0))
                {
                    dataNode.ThroughRelationTable = true;
                }
                if(subDataNodeassociationEnd.GetOtherEnd().Multiplicity.IsMany)
                {
                    dataNode.ThroughRelationTable = true;
                }

                if (subDataNodeassociationEnd.Association.MultiplicityType == AssociationType.OneToOne &&
                   !subDataNodeassociationEnd.GetOtherEnd().IsRoleA)
                {
                    dataNode.ThroughRelationTable = true;
                }
            }

        }

        /// <summary>
        ///HaveObjectsToActivate is truein case where there are objects in passive mode 
        ///and system must be activate.    
        /// </summary>
        /// <MetaDataID>{b2767fc2-6005-4946-97a5-0a8597219366}</MetaDataID>
        internal override bool ThereAreObjectsToActivate
        {
            get
            {
                return false;
            }
        }

    
    }
}
