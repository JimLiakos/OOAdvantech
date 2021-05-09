using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{51c0ec0b-ad67-4bcb-80a5-6a5e1701aeed}</MetaDataID>
    public enum ContextState
    {
        Active,
        Prepare,
        Commit,
        Abort,
        InDoubt
    }

    /// <MetaDataID>{07787414-f45a-4728-8f93-66ee6b75e482}</MetaDataID>
    public interface ITransactionContext : Transactions.ITransactionContextExtender
    {
        /// <MetaDataID>{b34ebcdc-0f26-402b-ac1d-b429c3322a9d}</MetaDataID>
        System.Collections.Generic.List<object> EnlistObjects
        {
            get;
        }

        /// <MetaDataID>{4446bbfc-0631-44e9-bf6e-0fb6ac91ac30}</MetaDataID>
        void EnlistCommand(Commands.Command command);
        /// <MetaDataID>{0816f084-7d2d-4a0e-a96b-ee3ff5454f9e}</MetaDataID>
        bool ContainCommand(string Identity);
        /// <MetaDataID>{29711e46-ad91-4ab2-ade9-cbe03489965e}</MetaDataID>
        System.Collections.Generic.Dictionary<string, OOAdvantech.PersistenceLayerRunTime.Commands.Command> EnlistedCommands
        {
            get;
        }

        /// <MetaDataID>{582ca6f2-3a5c-433b-9cac-f290124f60b3}</MetaDataID>
        void AddSlaveTransactionContext(ITransactionContext transactionContext);
        /// <MetaDataID>{df57c910-e2ba-41d8-85cf-994c2df41419}</MetaDataID>
        int ProcessCommands(int executionOrder);
    }

    /// <MetaDataID>{f01b07db-d709-47e8-97f8-f2361a42bb61}</MetaDataID>
    public interface ITransactionContextManager
    {
        /// <MetaDataID>{6277f8a9-cfde-48b1-b0ff-d97f1e1dad8d}</MetaDataID>
        ITransactionContext GetMasterTransactionContext(string globalTransactionUri);
    }


}
