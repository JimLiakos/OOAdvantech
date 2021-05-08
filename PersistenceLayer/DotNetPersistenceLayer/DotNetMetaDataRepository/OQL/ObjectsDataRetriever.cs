using System.Collections.Generic;
using System;
using OOAdvantech.Remoting;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{5d77acc7-36d9-4a7b-bf3e-ba871b859739}</MetaDataID>
    [Serializable]
    public class ObjectsDataRetriever
    {

        public ObjectsDataRetriever(OOAdvantech.Collections.Generic.List<ObjectData> objects, DataNode objectsDataNode)
        {
            Objects = objects;
            ObjectsDataNode = objectsDataNode;
        }
        [RoleAMultiplicityRange(1)]
        [Association("", Roles.RoleA, "8e9bbfa3-1968-462b-b074-3d48b01d82bd")]
        [IgnoreErrorCheck]
        public List<ObjectData> Objects;
        
         
        [Association("", Roles.RoleA, "1074256b-a3e0-47ad-9b57-76b00ea89a1f")]
        [IgnoreErrorCheck]
        public DistributedObjectQuery ObjectQuery;

        [Association("", Roles.RoleA, "84987434-3f07-4a61-80db-24e447d4985a")]
        [IgnoreErrorCheck]
        public DataNode ObjectsDataNode;
    }

    /// <MetaDataID>{9bc30950-520f-43ca-b5bf-af1b0a1bd05f}</MetaDataID>
    [Serializable]
    public class ObjectData
    {

        public ObjectData(object _object, System.Guid parentOID,List<MetaObjectID> lazyFetchingMembersIdentities)
        {
            _Object = _object;
            ParentOID = parentOID;
            LazyFetchingMembersIdentities = lazyFetchingMembersIdentities;
            CheckForPartialLoad();
        }


        public ObjectData(object _object, System.Guid parentOID )
        {
            _Object = _object;
            ParentOID = parentOID;
        }

        /// <exclude>Excluded</exclude>
        bool _PartialLoaded;
        
        public bool PartialLoaded
        {
            get
            {
                return _PartialLoaded;
            }
        }

        /// <exclude>Excluded</exclude>
        PersistenceLayer.StorageInstanceRef _StorageInstanceRef;
        
        public PersistenceLayer.StorageInstanceRef StorageInstanceRef
        {
            get
            {
                return _StorageInstanceRef;
            }
        }



        public ObjectData(object _object, List<MetaObjectID> lazyFetchingMembersIdentities)
        {
            _Object = _object;
            LazyFetchingMembersIdentities = lazyFetchingMembersIdentities;
            CheckForPartialLoad();
            
        }

        internal void CheckForPartialLoad()
        {
            if (!Remoting.RemotingServices.IsOutOfProcess(_Object as MarshalByRefObject) && PersistenceLayer.ObjectStorage.IsPersistent(_Object))
            {
                _StorageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_Object);

                foreach (var assotiationEnd in GetLazyFetchingMembers(MetaDataRepository.Classifier.GetClassifier(_Object.GetType()), LazyFetchingMembersIdentities))
                {
                    _PartialLoaded = !StorageInstanceRef.IsRelationLoaded(assotiationEnd);
                    if (PartialLoaded)
                        break;
                }

            }
        }
        public static OOAdvantech.Collections.Generic.List<MetaObjectID> GetLazyFetchingMembers(Classifier classifier, List<DataNode> subDataNodes)
        {
            OOAdvantech.Collections.Generic.List<MetaObjectID> lazyFetchingMembers = new OOAdvantech.Collections.Generic.List<MetaObjectID>();
            foreach (DataNode subDataNode in subDataNodes)
            {
                if (subDataNode.AssignedMetaObject is AssociationEnd)
                    lazyFetchingMembers.Add(subDataNode.AssignedMetaObject.Identity);
            }
            return lazyFetchingMembers;
        }

        internal static List<AssociationEnd> GetLazyFetchingMembers(Classifier classifier, List<MetaObjectID> lazyFetchingMembersIdentities)
        {
            List<AssociationEnd> lazyFetchingMembers = new List<AssociationEnd>();
            foreach (var associationEnd in classifier.GetAssociateRoles(true))
            {
                if (lazyFetchingMembersIdentities.Contains(associationEnd.Identity))
                    lazyFetchingMembers.Add(associationEnd); 
            }
            return lazyFetchingMembers;
        }


        List<MetaObjectID> LazyFetchingMembersIdentities;
        public ObjectData(object _object, PersistenceLayer.ObjectID parentObjectID, int parentObjectContextIdentity, List<MetaObjectID> lazyFetchingMembersIdentities)
        {
            _Object = _object;
            ParentObjectID = parentObjectID;
            ParentObjectContextIdentity = parentObjectContextIdentity;
            LazyFetchingMembersIdentities = lazyFetchingMembersIdentities;
            CheckForPartialLoad();
        }

        public OOAdvantech.PersistenceLayer.ObjectID ParentObjectID;
        public int ParentObjectContextIdentity;

        static ObjectIdentityType IdentityType;

        OOAdvantech.PersistenceLayer.ObjectID _ObjectID;
        public OOAdvantech.PersistenceLayer.ObjectID ObjectID
        {
            get
            {
                if(IdentityType ==null)
                    IdentityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });


                if (_ObjectID == null && !Remoting.RemotingServices.IsOutOfProcess(_Object as MarshalByRefObject))
                    _ObjectID = new OOAdvantech.PersistenceLayer.ObjectID(IdentityType, new object[1] { System.Guid.NewGuid() }, 0);
                return _ObjectID;
            }
        }

        public Dictionary<object, ObjectData> ParentDataNodeRelatedObjects = new Dictionary<object, ObjectData>();
        public System.Guid ParentOID;
        public object _Object;
        public Dictionary<string, Dictionary<object, ObjectData>> RelatedObjects = new Dictionary<string, Dictionary<object, ObjectData>>();


    }
}
