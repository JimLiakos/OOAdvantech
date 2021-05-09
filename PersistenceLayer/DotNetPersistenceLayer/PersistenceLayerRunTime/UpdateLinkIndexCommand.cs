using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
    /// <MetaDataID>{885be7db-f75c-4291-bc39-08b740c4fafa}</MetaDataID>
    public abstract class UpdateLinkIndexCommand : Command
    {
        public OOAdvantech.MetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
        {
            get;
            protected set;
        }

        public UpdateLinkIndexCommand(ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, AssociationEndAgent linkInitiatorAssociationEnd,int index)
        {
            ObjectStorage = objectStorage;
            RoleA = roleA;
            RoleB = roleB;
            LinkInitiatorAssociationEnd = linkInitiatorAssociationEnd.RealAssociationEnd;

            if (linkInitiatorAssociationEnd.RealAssociationEnd.Indexer)
                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;

            if (linkInitiatorAssociationEnd.RealAssociationEnd.GetOtherEnd().Indexer)
                if (linkInitiatorAssociationEnd.RealAssociationEnd.GetOtherEnd().IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;
        }
        /// <exclude>Excluded</exclude>
        string _Identity;
        /// <MetaDataID>{CE82F8D5-05D1-40FA-B37F-4C4CEB085CD3}</MetaDataID>
        public override string Identity
        {
            get
            {
                if (_Identity == null)
                    _Identity = "linkIndex" + ObjectStorage.StorageMetaData.StorageIdentity + RoleA.MemoryID.ToString() + RoleA.ValueTypePath.ToString() + RoleB.MemoryID.ToString() + RoleB.ValueTypePath.ToString() + this.LinkInitiatorAssociationEnd.Association.Identity;
                return _Identity;
            }
        }
        public static string GetIdentity(PersistenceLayerRunTime.ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, DotNetMetaDataRepository.Association association)
        {
            return "linkIndex" + objectStorage.StorageMetaData.StorageIdentity + roleA.MemoryID.ToString() + roleA.ValueTypePath.ToString() + roleB.MemoryID.ToString() + roleB.ValueTypePath.ToString() + association.Identity;
        }

        public override int ExecutionOrder
        {
            get
            {
                return 75;
            }
        }

        public int RoleAIndex = -1;
        public int RoleBIndex = -1;


        public ObjectStorage ObjectStorage;

        /// <MetaDataID>{FFBE9DEA-6BD6-41A9-A28D-AC3D771B85A2}</MetaDataID>
        public StorageInstanceAgent RoleB;
        /// <MetaDataID>{037C5269-0050-43DE-9C04-8D770AF52D69}</MetaDataID>
        public StorageInstanceAgent RoleA;




    }
}
