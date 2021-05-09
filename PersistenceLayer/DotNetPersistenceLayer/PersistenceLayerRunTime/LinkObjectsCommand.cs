using OOAdvantech.DotNetMetaDataRepository;

namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
    /// <MetaDataID>{6022A415-B043-493C-B091-9F4D8675338D}</MetaDataID>
    /// <summary></summary>
    public abstract class LinkObjectsCommand : LinkCommand
    {

        protected bool Multilingual;
        protected System.Globalization.CultureInfo Culture;


        //TODO: θα πρέπει να φτιαχτή σενάριο όπου μια σχέση με navigation προς την μία πλευρά και
        ///multiplicity 0,1 από την άλλη πλευρά να έχει και εκεί κατα λάθος 0,1 αλλά ουσιαστικά να είναι many
        ///θα πρέπει το σύστημα να κάνει multiplicity check και να το ποιάνει αλλιος  δημιουργούνται 
        ///δευτερογενοί προβλήματα που δίσκολα ανιχνεύει ο χρήστης την προέλευση του προβλήματος.


        /// <MetaDataID>{8891573F-7019-41DA-BF44-98177938E19A}</MetaDataID>
        public LinkObjectsCommand(ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index) :
            base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd)
        {
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
        /// <MetaDataID>{858569ba-8020-492b-8ab6-c7bf473768fb}</MetaDataID>
        public int RoleAIndex = -1;
        /// <MetaDataID>{16e253a9-a180-44c4-95dd-2364c0c2e818}</MetaDataID>
        public int RoleBIndex = -1;
        /// <MetaDataID>{3e446f55-8911-43d6-87b4-d429c31b640c}</MetaDataID>
        string _Identity;
        /// <MetaDataID>{CE82F8D5-05D1-40FA-B37F-4C4CEB085CD3}</MetaDataID>
        public override string Identity
        {
            get
            {
                if (_Identity == null)
                {
                    if (IsMultilingualLink(RoleA, RoleB, this.LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association))
                        _Identity = "mllink_" + CultureContext.CurrentNeutralCultureInfo.Name + "_" + ObjectStorage.StorageMetaData.StorageIdentity + RoleA.MemoryID.ToString() + RoleA.ValueTypePath.ToString() + RoleB.MemoryID.ToString() + RoleB.ValueTypePath.ToString() + this.LinkInitiatorAssociationEnd.Association.Identity;
                    else
                        _Identity = "link" + ObjectStorage.StorageMetaData.StorageIdentity + RoleA.MemoryID.ToString() + RoleA.ValueTypePath.ToString() + RoleB.MemoryID.ToString() + RoleB.ValueTypePath.ToString() + this.LinkInitiatorAssociationEnd.Association.Identity;
                }
                return _Identity;
            }
        }
        /// <MetaDataID>{D612B4B4-FC98-49A4-986B-7391E1A438D5}</MetaDataID>
        public static string GetIdentity(PersistenceLayerRunTime.ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, DotNetMetaDataRepository.Association association)
        {

            if (IsMultilingualLink(roleA, roleB, association))
                return "mllink_"+ CultureContext.CurrentNeutralCultureInfo.Name+"_" + objectStorage.StorageMetaData.StorageIdentity + roleA.MemoryID.ToString() + roleA.ValueTypePath.ToString() + roleB.MemoryID.ToString() + roleB.ValueTypePath.ToString() + association.Identity;
            else
                return "link" + objectStorage.StorageMetaData.StorageIdentity + roleA.MemoryID.ToString() + roleA.ValueTypePath.ToString() + roleB.MemoryID.ToString() + roleB.ValueTypePath.ToString() + association.Identity;

        }




        /// <MetaDataID>{CE3BDFCA-6854-4CA3-9735-9645842B45F1}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected bool SubTransactionCmdsProduced;



        /// <MetaDataID>{CE2ED177-8EF1-4251-9F2B-CEA4CE79BAD4}</MetaDataID>
        public override void Execute()
        {

            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            if (LinkInitiatorAssociationEnd == null)
                throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
            #endregion

            //DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd=OwnerAssociationEnd.GetOtherEnd() as DotNetMetaDataRepository.AssociationEnd;

            MetaDataRepository.Association association = LinkInitiatorAssociationEnd.Association;
            MetaDataRepository.Roles commandInitiatorRole;
            if (LinkInitiatorAssociationEnd.IsRoleA)
                commandInitiatorRole = MetaDataRepository.Roles.RoleA;
            else
                commandInitiatorRole = MetaDataRepository.Roles.RoleB;


            if (RoleB.Class.HasReferentialIntegrity(association.RoleA as DotNetMetaDataRepository.AssociationEnd))
            {
                if (RoleA.StorageIdentity == ObjectStorage.StorageIdentity)
                    RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
            }
            if (RoleA.Class.HasReferentialIntegrity(association.RoleB as DotNetMetaDataRepository.AssociationEnd))
            {
                if (RoleB.StorageIdentity == ObjectStorage.StorageIdentity)
                    RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
            }

            if (association.LinkClass == null)
            {
                if (commandInitiatorRole == MetaDataRepository.Roles.RoleA)
                {
                    if (association.RoleB.Navigable)
                        RoleA.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent(association.RoleB as DotNetMetaDataRepository.AssociationEnd), RoleB.MemoryInstance, true);
                }
                else
                {
                    if (association.RoleA.Navigable)
                        RoleB.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent(association.RoleA as DotNetMetaDataRepository.AssociationEnd), RoleA.MemoryInstance, true);
                }
            }

        }
        /// <MetaDataID>{8F9BAC27-A64E-40C6-9556-DB3AFA4E169F}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }


        /// <MetaDataID>{A8CB0C44-030E-453C-A5F9-C0203E3A2EC4}</MetaDataID>
        /// <exclude>Excluded</exclude>
        public string AssociationName;

        /// <MetaDataID>{EB30C51B-300D-46BD-83CE-056D276A1CCB}</MetaDataID>
        public override int ExecutionOrder
        {
            get
            {
                return 40;
            }
        }
    }
}
