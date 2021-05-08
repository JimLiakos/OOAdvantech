using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    /// <summary>
    /// Derived DataNode refers to other data node which loads data
    /// </summary>
    /// <MetaDataID>{3864ec92-98e0-4296-a6a7-1d3df4f2510e}</MetaDataID>
    [Serializable]
    public class DerivedDataNode:DataNode
    {

        DerivedDataNode(Guid identity,DataNode orgDataNode)
            : base(identity)
        {
            OrgDataNode = orgDataNode;
        }
        internal override DataNode Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataNode;

            var newDataNode = new DerivedDataNode(Identity,OrgDataNode.Clone(clonedObjects));
            clonedObjects[this] = newDataNode;
            Copy(newDataNode, clonedObjects);
            return newDataNode;
        }

        public override Classifier Classifier
        {
            get
            {
                return OrgDataNode.Classifier;
            }
            set
            {
                //base.Classifier = value;
            }
        }
        internal override void GetObjectsContexts(Collections.Generic.Dictionary<string, ObjectsContextMetadataDistributionManager> queryObjectsContexts)
        {
            
        }
        internal override bool IsDataSource
        {
            get
            {
                return OrgDataNode.IsDataSource;
            }
        }
        internal override bool IsDataSourceMember
        {
            get
            {
                return OrgDataNode.IsDataSourceMember;
            }
        }

        /// <summary>
        /// This method check dataNode parameter if it is derived data node return original data node
        /// else return dataNode  parameter
        /// </summary>
        /// <param name="dataNode">
        /// Defines the data node for check
        /// </param>
        /// <returns>
        /// Original DataNode if dataNode parameter is derived DataNode else returns the parameter DataNode
        /// </returns>
        public static DataNode GetOrgDataNode(DataNode dataNode)
        {
            if (dataNode is DerivedDataNode)
                return (dataNode as DerivedDataNode).OrgDataNode;
            else
                return dataNode;

        }


        public static DerivedDataNode GetDerivedDataNodeFor(DerivedDataNode rootDerivedDataNode, DataNode subDataNode)
        {
            if (subDataNode.IsSameOrParentDataNode(rootDerivedDataNode.OrgDataNode))
            {
                if (subDataNode == rootDerivedDataNode.OrgDataNode)
                    return rootDerivedDataNode;

                var derivedSubDataNode = (from derivedDataNode in rootDerivedDataNode.SubDataNodes.OfType<DerivedDataNode>()
                                          where subDataNode.IsSameOrParentDataNode(derivedDataNode.OrgDataNode)
                                          select derivedDataNode).FirstOrDefault();
                if (derivedSubDataNode != null)
                    return GetDerivedDataNodeFor(derivedSubDataNode, subDataNode);
                else
                {

                    var newDerivedSubDataNode = new DerivedDataNode((from dataNode in rootDerivedDataNode.OrgDataNode.SubDataNodes
                                              where subDataNode.IsSameOrParentDataNode(dataNode)
                                              select dataNode).FirstOrDefault());
                    newDerivedSubDataNode.ParentDataNode = rootDerivedDataNode;

                    return GetDerivedDataNodeFor(derivedSubDataNode, subDataNode);
                }
            }
            else throw new System.Exception("The 'rootDerivedDataNode'.OrgDataNode isn't parent of 'subDataNode'");
        }

        /// <summary>
        /// Defines the original DataNode
        /// </summary>
        public readonly DataNode OrgDataNode;
        public DerivedDataNode(DataNode orgDataNode)
            : base(orgDataNode.ObjectQuery)
        {
            OrgDataNode = orgDataNode;
            if (OrgDataNode is DerivedDataNode)
                OrgDataNode = (OrgDataNode as DerivedDataNode).OrgDataNode;

            Name = OrgDataNode.Name;
            if(OrgDataNode.Alias!=null)
                _Alias = ObjectQuery.GetValidAlias(OrgDataNode.Alias);
        }

        public override DataNode.DataNodeType Type
        {
            get
            {
                return DataNodeType.DerivedDataNode;
            }
            set
            {
                
            }
        }

        public override MetaObject AssignedMetaObject
        {
            get
            {
                if (ParentDataNode == null || ParentDataNode.Type == DataNodeType.Key)
                    return base.AssignedMetaObject;
                else
                    return OrgDataNode.AssignedMetaObject;
            }
            set
            {
                base.AssignedMetaObject = value;
            }
        } 


        public override DataSource DataSource
        {
            get
            {
                return OrgDataNode.DataSource;
            }
        }

        public override bool ParticipateInAggregateFunction
        {
            get
            {
                return base.ParticipateInAggregateFunction;
            }
            set
            {
                base.ParticipateInAggregateFunction = value;
                OrgDataNode.ParticipateInAggregateFunction = value;
            }
        }
        public override bool ParticipateInGroopByAsGrouped
        {
            get
            {
                return base.ParticipateInGroopByAsGrouped;
            }
            set
            {
                base.ParticipateInGroopByAsGrouped = value;
                OrgDataNode.ParticipateInGroopByAsGrouped = value;
            }
        }
        public override bool ParticipateInGroopByAsKey
        {
            get
            {
                return base.ParticipateInGroopByAsKey;
            }
            set
            {
                base.ParticipateInGroopByAsKey = value;
                OrgDataNode.ParticipateInGroopByAsKey = value;
            }
        }
        public override bool ParticipateInSelectClause
        {
            get
            {
                return base.ParticipateInSelectClause;
            }
            set
            {
                base.ParticipateInSelectClause = value;
                OrgDataNode.ParticipateInSelectClause = value;
            }
        }
        public override bool ParticipateInWereClause
        {
            get
            {
                return base.ParticipateInWereClause;
            }
            set
            {
                base.ParticipateInWereClause = value;
                OrgDataNode.ParticipateInWereClause = value;
            }
        }

        //public override string Alias
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(_Alias))
        //            return _Alias;
        //        else if (ParentDataNode != null)
        //            _Alias = ObjectQuery.GetValidAlias(OrgDataNode.Alias);
        //        return _Alias;
        //    }
        //    set
        //    {
        //        base.Alias = value;
        //    }
        //}

        

    }
}
