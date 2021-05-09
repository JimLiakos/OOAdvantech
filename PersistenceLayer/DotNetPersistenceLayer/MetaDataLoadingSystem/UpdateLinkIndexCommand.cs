using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{fd570674-6b58-4a4b-a8d2-5b0b9b75482f}</MetaDataID>
    public class UpdateLinkIndexCommand : PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand
    {
        public UpdateLinkIndexCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
            base(objectStorage, roleA, roleB, linkInitiatorAssociationEnd, index)
        {

        }
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }

        public override void Execute()
        {
            //List<OOAdvantech.PersistenceLayerRunTime.GroupIndexChange> groupIndexChanges = Collection.GetIndexChanges(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
        }
    }
}
