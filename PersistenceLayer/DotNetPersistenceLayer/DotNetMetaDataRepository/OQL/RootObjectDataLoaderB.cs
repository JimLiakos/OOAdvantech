namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using OOAdvantech.Collections.Generic;
    using SubDataNodeIdentity = System.Guid;
    /// <MetaDataID>{b68e978e-7af6-48a3-954d-7607bd241d31}</MetaDataID>
    public class RootObjectDataLoader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader
    {
        public RootObjectDataLoader(DataNode dataNode)
            : base(dataNode)
        {
            System.Diagnostics.Debug.WriteLine(dataNode.FullName);

        }
        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        //{
            
        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{
            
        //}
        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, object relatedObject)
        //{
            
        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{
            
        //}

        /// <summary>If it is true then the data source table has the identity coloumns of foreign key relation,
        /// otherwise table has the reference columns. Always the identity columns has unique constrain </summary>
        
        //internal override void LoadObjectRelationLinks(DataNode subDataNode)
        //{

        //}
        //internal override void SetValueTypeValues(DataNode subDataNode)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}

        public override System.Collections.Generic.List<string> RecursiveParentReferenceColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    throw new System.Exception("The DataNode isn't recursive");
                return DataSourceRelationsColumnsWithParent;

                
            }
        }

        public override System.Collections.Generic.List<string> RecursiveParentColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    throw new System.Exception("The DataNode isn't recursive");

                List<string> oidColumns = new List<string>();
                oidColumns.Add("ObjectID");
                return oidColumns;
            }
        }

        public override System.Collections.Generic.List<string> DataSourceRelationsColumnsWithParent
        {
            get
            {
                List<string> childRelationColumns = new List<string>();

                MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                MetaDataRepository.Attribute attribute = DataNode.AssignedMetaObject as MetaDataRepository.Attribute;

                if (associationEnd != null &&
                    (HasRelationIdentityColumns
                    || (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                    associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation)))
                {
                    childRelationColumns.Add("ObjectID");
                }
                else
                {
                    if (associationEnd != null)
                    {
                        if (associationEnd.IsRoleA)
                            childRelationColumns.Add(associationEnd.Association.Name + "RoleB_OID");
                        else
                            childRelationColumns.Add(associationEnd.Association.Name + "RoleA_OID");
                    }

                }
                if (attribute != null)
                    childRelationColumns.Add(attribute.Name + "RoleB_OID");

                return childRelationColumns;

            }
        }

        public override OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {
                Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>> parentRelationColumns = new Dictionary<SubDataNodeIdentity, System.Collections.Generic.List<string>>();
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                    if (associationEnd != null)
                    {
                        associationEnd = associationEnd.GetOtherEnd();
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        if (HasSubNodeRelationIdentityColumns(subDataNode)&&
                            !(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                            associationEnd.Association != DataNode.Classifier.ClassHierarchyLinkAssociation))
                        {
                            if (associationEnd.IsRoleA)
                                dataColumns.Add(associationEnd.Association.Name + "RoleB_OID");
                            else
                                dataColumns.Add(associationEnd.Association.Name + "RoleA_OID");
                        }
                        else
                        {

                            dataColumns.Add("ObjectID");
                        }

                        parentRelationColumns[subDataNode.Identity] = dataColumns;
                    }
                    if (attribute != null && subDataNode.DataSource != null)
                    {
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        dataColumns.Add("ObjectID");
                        parentRelationColumns[subDataNode.Identity] = dataColumns;
                    }

                }

                return parentRelationColumns;


            }
        }

        //public override Storage LoadFromStorage
        //{
        //    get { throw new System.Exception("The method or operation is not implemented."); }
        //}

        //public override OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage
        //{
        //    get { throw new System.Exception("The method or operation is not implemented."); }
        //}

        public override Classifier Classifier
        {
            get { return DataNode.Classifier; }
        }

        public override void LoadDataLocally()
        {
            

            #region Add table columns
            Data.TableName = DataNode.Name;
            Data.Columns.Add("ObjectID", typeof(int));//Add Object identity columns
            


            if ((DataNode.DataSource as RootObjectDataSource).HasLockRequest)
                Data.Columns.Add("Locked", typeof(OOAdvantech.Transactions.Transaction));

            #region Adds columns for the relation with the parent data node
            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            if (associationEnd != null &&
                !HasRelationIdentityColumns)
            //&&
            //!(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
            //  associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation))
            {
                if (associationEnd.IsRoleA)
                    Data.Columns.Add(associationEnd.Association.Name + "RoleB_OID", typeof(int));
                else
                    Data.Columns.Add(associationEnd.Association.Name + "RoleA_OID", typeof(int));

            }
            else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {
                //DataNode.HasRelationIdentityColumns = true;

                Data.Columns.Add((DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID", typeof(int));

            }

            #endregion

            #region Adds columns for the relation with the subDataNodes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (subDataNodeassociationEnd != null && HasSubNodeRelationIdentityColumns(subDataNode))
                {
                    if (subDataNodeassociationEnd.IsRoleA)
                    {
                        if (!Data.Columns.Contains(subDataNodeassociationEnd.Association.Name + "RoleA_OID"))
                            Data.Columns.Add(subDataNodeassociationEnd.Association.Name + "RoleA_OID", typeof(int));
                    }
                    else
                    {
                        if (!Data.Columns.Contains(subDataNodeassociationEnd.Association.Name + "RoleB_OID"))
                            Data.Columns.Add(subDataNodeassociationEnd.Association.Name + "RoleB_OID", typeof(int));
                    }
                }
                else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.DataSource != null)
                {
                    if (!Data.Columns.Contains((subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID"))
                        Data.Columns.Add((subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID", typeof(int));

                }
            }
            #endregion

            #region Adds columns for the object Attributes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                    object Value = attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole");
                    bool IsAssociationClassRole = false;
                    if (Value != null)
                        IsAssociationClassRole = (bool)Value;
                    if (IsAssociationClassRole)
                    {
                        Data.Columns.Add(subDataNode.Name, typeof(string));
                    }
                    else
                    {
                        System.Type type = ModulePublisher.ClassRepository.GetType((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).Type.FullName, "");
                        Data.Columns.Add(subDataNode.Name, type);
                    }
                }
            }
            #endregion

            #region Adds columns for the object if it is necessary
            //  if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                System.Type objectType = null;
                objectType = ModulePublisher.ClassRepository.GetType(DataNode.Classifier.FullName, "");
                Data.Columns.Add("Object", objectType);
            }
            #endregion
            #endregion


            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (subDataNodeassociationEnd != null &&
                    subDataNodeassociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                    subDataNodeassociationEnd.Association != DataNode.Classifier.ClassHierarchyLinkAssociation)
                {

                    System.Collections.Generic.List<string> parentRelationColumns = new System.Collections.Generic.List<string>();
                    System.Collections.Generic.List<string> childRelationColumns = new System.Collections.Generic.List<string>();
                    if (subDataNodeassociationEnd.IsRoleA)
                    {
                        parentRelationColumns.Add(subDataNodeassociationEnd.Association.Name + "RoleB_OID");
                        childRelationColumns.Add(subDataNodeassociationEnd.Association.Name + "RoleA_OID");
                    }
                    else
                    {
                        parentRelationColumns.Add(subDataNodeassociationEnd.Association.Name + "RoleA_OID");
                        childRelationColumns.Add(subDataNodeassociationEnd.Association.Name + "RoleB_OID");

                    }
                    RelationshipData relationshipData = new RelationshipData(parentRelationColumns, childRelationColumns, new DataLoader.DataTable(false));
                    relationshipData.Data.Columns.Add(subDataNodeassociationEnd.Association.Name + "RoleA_OID", typeof(string));
                    relationshipData.Data.Columns.Add(subDataNodeassociationEnd.Association.Name + "RoleB_OID", typeof(string));
                    RelationshipsData.Add((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString(), relationshipData);

                }
            }

            if ((DataNode.ObjectQuery as QueryOnRootObject).RootObject != null)
                GetDataNodeObjects((DataNode.ObjectQuery as QueryOnRootObject).RootObject, DataNode.HeaderDataNode, new System.Collections.Generic.Dictionary<int, object>());

            if ((DataNode.ObjectQuery as QueryOnRootObject).ObjectCollection != null)
            {
                foreach (object _object in (DataNode.ObjectQuery as QueryOnRootObject).ObjectCollection)
                {
                    GetDataNodeObjects(_object, DataNode.HeaderDataNode, new System.Collections.Generic.Dictionary<int, object>());
                }
            }



            // throw new System.Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// If it is true the data source table has the identity coloumns 
        ///of foreign key relation, otherwise table has the reference columns. 
        ///Always the identity columns has unique constrain 
        ///</summary>
        public override bool HasRelationIdentityColumns
        {
            get
            {
                //if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                //    return true;

                if (DataNode.AssignedMetaObject is AssociationEnd)
                {
                    AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
                    if (!associationEnd.Multiplicity.IsMany &&  //zero ore one
                        associationEnd.GetOtherEnd().Multiplicity.IsMany)
                    {
                        return true;
                    }
                    else if (associationEnd.Association.MultiplicityType == AssociationType.ManyToMany &&
                        DataNode.Classifier.ClassHierarchyLinkAssociation != associationEnd.Association)
                    {
                        return true;
                    }
                    else if (associationEnd.Association.MultiplicityType == AssociationType.OneToOne&&
                       !associationEnd.IsRoleA)
                    {
                        return true;
                    }
                    else 
                        return false;

                }
                else
                    return false;


            }
        }
        private bool HasSubNodeRelationIdentityColumns(DataNode subDataNode)
        {
            if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                return true;

            if ((subDataNode.AssignedMetaObject is AssociationEnd) && (subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                !(subDataNode.AssignedMetaObject as AssociationEnd).Multiplicity.IsMany)
            {
                return true;
            }
            else if ((subDataNode.AssignedMetaObject is AssociationEnd) &&
                (subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType == AssociationType.ManyToMany &&
                subDataNode.Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as AssociationEnd).Association)
            {
                return true;
            }
            else
                return false;

        }

        private void LoadObjectInTable(object parentNodeObject, object obj, System.Collections.Generic.Dictionary<int, object> values)
        {
            System.Data.DataRow row = null;
            int OID = 0;
            if (obj != null && obj.GetType().IsValueType)
            {
                if (DataNodeObjects.Contains(parentNodeObject))
                    return;
                DataNodeObjects.Add(parentNodeObject);


                row = Data.NewRow();
                 OID = parentNodeObject.GetHashCode();
                row["ObjectID"] = OID;

            }
            else
            {
                if (DataNodeObjects.Contains(obj))
                    return;
                DataNodeObjects.Add(obj);


                row = Data.NewRow();
                 OID = obj.GetHashCode();
                row["ObjectID"] = OID;
            }

            //if ((DataNode.DataSource as RootObjectDataSource).HasLockRequest)
            //{
            //    System.Collections.Generic.List<Transactions.Transaction> transactions = Transactions.ObjectStateTransition.GetTransaction(obj);
            //    if (transactions.Count > 0 && transactions[0] != Transactions.Transaction.Current)
            //        row["Locked"] = transactions[0];
            //    else
            //        row["Locked"] = null;
            //}

            #region Loads object the object if it is necessary
            //if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                System.Type objectType = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type; ;
                int ObjID = OID;
                try
                {
                    row["Object"] = obj;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            #endregion

            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                #region Loads columns for the object Attributes
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    object value = null;
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                        value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember.GetValue(obj, null);
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                    {
                        //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
                        //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                        value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                    }
                    row[subDataNode.Name] = value;
                }
                #endregion

                #region Loads columns for the relation with the subDataNodes
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource.HasRelationIdentityColumns)
                {
                    MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    if (subDataNodeassociationEnd != null)
                    {
                        if (Classifier.ClassHierarchyLinkAssociation == subDataNodeassociationEnd.Association)
                        {
                            if (Classifier is DotNetMetaDataRepository.Interface)
                            {
                                if (subDataNodeassociationEnd.IsRoleA)
                                {
                                    object roleAObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);
                                    if (roleAObject != null)
                                        row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
                                }
                                else
                                {
                                    object roleBObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);
                                    if (roleBObject != null)
                                        row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
                                }

                            }
                            if (Classifier is DotNetMetaDataRepository.Class)
                            {
                                if (subDataNodeassociationEnd.IsRoleA)
                                {
                                    //object roleAObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
                                    //object roleAObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
                                    object roleAObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj );
                                    if (roleAObject != null)
                                        row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
                                }
                                else
                                {
                                    //object roleBObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
                                    //object roleBObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
                                    object roleBObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
                                    if (roleBObject != null)
                                        row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
                                }

                            }

                        }
                        else
                        {
                            System.Collections.Generic.Dictionary<int, object> objects = new System.Collections.Generic.Dictionary<int, object>();
                            foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
                            {
                                DataLoader dataLoader = entry.Value;
                                (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
                            }

                            if (objects.Count > 1)
                                throw new System.Exception("Invalid relation");
                            if (objects.Count > 0)
                            {
                                
                                foreach (object item in objects.Values)
                                {

                                    if (subDataNodeassociationEnd.IsRoleA)
                                        row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
                                    else
                                        row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode();
                                    break;

                                }
                            }

                        }
                    }
                    //else if (subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    //{
                    //    System.Collections.ArrayList objects = new System.Collections.ArrayList();
                    //    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
                    //    {
                    //        DataLoader dataLoader = entry.Value;
                    //        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
                    //    }

                    //    if (objects.Count > 1)
                    //        throw new System.Exception("Invalid relation");
                    //    if (objects.Count > 0)
                    //        row[subDataNode.AssignedMetaObject.Name + "RoleA_OID"] = objects[0].GetHashCode();
                    //}
                }
                #endregion


            }


            #region Loads with columns for the relation with the parent data node
            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            if (associationEnd != null &&
                !HasRelationIdentityColumns &&
                !(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation))
            {
                if (associationEnd.IsRoleA)
                    row[associationEnd.Association.Name + "RoleB_OID"] = parentNodeObject.GetHashCode();
                else
                    row[associationEnd.Association.Name + "RoleA_OID"] = parentNodeObject.GetHashCode();

            }
            else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {
                row[(DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID"] = parentNodeObject.GetHashCode();
            }


            #endregion



            //if (DataNode.LocalFilter == null)
                Data.Rows.Add(row);
            //else if (DataNode.LocalFilter.DoesRowPassCondition(row, DataNode))
            //    Data.Rows.Add(row);
            if (DataNode.Path.Recursive && RecursiveStep<DataNode.Path.RecursiveSteps)
            {
                RecursiveStep++;
                GetDataNodeObjects(obj, DataNode.ParentDataNode, values);
                RecursiveStep--;
            }
        }
        int RecursiveStep = 0;
        System.Collections.ArrayList DataNodeObjects = new System.Collections.ArrayList();
        public void GetDataNodeObjects(object obj, DataNode dataNode, System.Collections.Generic.Dictionary<int,object> values)
        {
            try
            {
                if (dataNode == DataNode && !values.ContainsKey(obj.GetHashCode()))
                {
                    LoadObjectInTable(null, obj, values);
                    values.Add(obj.GetHashCode(),obj);
                    return;
                }
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (this.DataNode.IsPathNode(subDataNode))
                    {


                        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                        {
                            MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                            object memberObject = null;

                            if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                            {
                                if (associationEnd.IsRoleA)
                                {
                                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
                                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

                                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
                                    {
                                        //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
                                        //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
                                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
                                    }
                                }
                                else
                                {
                                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
                                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

                                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
                                    {
                                        //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
                                        //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
                                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);

                                    }
                                }
                                GetDataNodeObjects(memberObject, subDataNode, values);
                            }
                            else
                            {
                                if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember!=null)
                                    memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
                                else
                                {
                                    //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                    //memberObject = fieldInfo.GetValue(obj);

                                    //memberObject = Member<object>.GetValue(obj, fieldInfo);
                                    memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
                                }
                                if (memberObject == null)
                                    return;
                                if (associationEnd.CollectionClassifier != null)
                                {

                                    System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                    enumerator.Reset();
                                    while (enumerator.MoveNext())
                                    {
                                        object collectionObj = enumerator.Current;
                                        GetDataNodeObjects(collectionObj, subDataNode, values);
                                    }

                                }
                                else
                                {
                                    GetDataNodeObjects(memberObject, subDataNode, values);

                                }
                            }
                        }

                        if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource != null)
                        {
                            MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                            object memberObject = null;

                            if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember!=null)
                                memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                            else
                                memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(obj);
                            if (memberObject == null)
                                return;
                            {
                                if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
                                {
                                    if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == subDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
                                    {
                                        System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                        enumerator.Reset();
                                        while (enumerator.MoveNext())
                                        {
                                            object collectionObj = enumerator.Current;
                                            GetDataNodeObjects(collectionObj, subDataNode, values);

                                        }
                                    }
                                    else
                                    {
                                        GetDataNodeObjects(memberObject, subDataNode, values);
                                    }
                                }
                            }
                        }





                        return;

                    }
                    if (subDataNode == DataNode)
                    {

                        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                        {
                            MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                            object memberObject = null;

                            if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                            {
                                if (associationEnd.IsRoleA)
                                {
                                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
                                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

                                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
                                    {
                                        //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
                                        //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
                                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
                                    }
                                }
                                else
                                {
                                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
                                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

                                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
                                    {
                                        //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
                                        //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
                                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
                                    }
                                }
                                if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
                                {
                                    LoadObjectInTable(obj, memberObject, values);
                                    values.Add(memberObject.GetHashCode(), memberObject);
                                }




                            }
                            else
                            {



                                if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember!= null)
                                    memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
                                else
                                {
                                    //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                    //memberObject = fieldInfo.GetValue(obj);
                                    //memberObject = Member<object>.GetValue(obj, fieldInfo);
                                    memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
                                }
                                if (memberObject == null)
                                    return;
                                if (associationEnd.CollectionClassifier != null)
                                {

                                    System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                    enumerator.Reset();
                                    while (enumerator.MoveNext())
                                    {
                                        object collectionObj = enumerator.Current;
                                        if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
                                        {
                                            LoadObjectInTable(obj, collectionObj, values);
                                            values.Add(collectionObj.GetHashCode(), collectionObj);
                                        }
                                    }
                                }
                                else
                                {
                                    if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
                                    {
                                        LoadObjectInTable(obj, memberObject, values);
                                        values.Add( memberObject.GetHashCode(),memberObject);
                                    }
                                }
                            }


                            return;

                        }
                        if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.DataSource != null)
                        {
                            MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                            object memberObject = null;
                            System.Type memberType=null;

                            if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                                memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                            else
                            {
                                //System.Reflection.FieldInfo fieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                ////memberObject = fieldInfo.GetValue(obj);
                                //memberObject = Member<object>.GetValue(obj, fieldInfo);
                                memberObject = Member<object>.GetValue((attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);

                            }

                            if (memberObject == null)
                                return;
                            {
                                if (memberObject != null &&!memberObject.GetType().IsValueType&& !values.ContainsKey(memberObject.GetHashCode()))
                                {
                                    if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
                                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
                                    {
                                        System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator",  new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                        enumerator.Reset();
                                        while (enumerator.MoveNext())
                                        {
                                            object collectionObj = enumerator.Current;
                                            if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
                                            {
                                                LoadObjectInTable(obj, collectionObj, values);
                                                values.Add(collectionObj.GetHashCode(), collectionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LoadObjectInTable(obj, memberObject, values);
                                        values.Add(memberObject.GetHashCode(), memberObject);
                                    }
                                }
                                if (memberObject != null && memberObject.GetType().IsValueType)
                                {
                                    LoadObjectInTable(obj, memberObject, values);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception Error)
            {
                throw;

            }
        }

        ///// <summary>
        ///// Return the member metadata form parameter type with name the parameter memberName.
        ///// If the there isn't then return null. If there are more than one in hierarchy then return the member which
        ///// is declared in type of parameter type.
        ///// Method is useful for member which is proprty or field.
        ///// </summary>
        ///// <param name="type">Defines the type where method look for member</param>
        ///// <param name="memberName">Defines the member name</param>
        ///// <returns>Member metadata object</returns>
        //static private System.Reflection.MemberInfo GetMember(System.Type type, string memberName)
        //{
        //    System.Reflection.MemberInfo[] members = type.GetMember(memberName);
        //    if (members.Length > 0)
        //        return members[0];
        //    else
        //    {
        //        OOAdvantech.MetaDataRepository.Classifier clasifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;
        //        if (clasifier == null)
        //        {
        //            if (type != null)
        //            {
        //                OOAdvantech.DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type.Assembly) as OOAdvantech.DotNetMetaDataRepository.Assembly;
        //                if (assembly == null)
        //                    assembly = new OOAdvantech.DotNetMetaDataRepository.Assembly(type.Assembly);
        //                long count = assembly.Residents.Count;

        //            }
        //        }
        //        clasifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;


        //        if (clasifier != null)
        //        {
        //            foreach (OOAdvantech.MetaDataRepository.Attribute attribute in clasifier.GetAttributes(true))
        //            {
        //                if (attribute.Name == memberName)
        //                    return attribute.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
        //            }
        //            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in clasifier.GetRoles(true))
        //            {
        //                if (associationEnd.Name == memberName)
        //                    return associationEnd.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
        //            }

        //        }


        //        return null;
        //    }
        //}


        public override System.Collections.ArrayList GetDataColoumns()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override object GetObject(System.Data.DataRow row, out bool loadObjectLinks)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }
}
