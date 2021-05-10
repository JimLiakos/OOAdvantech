using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.UserInterface.Runtime;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{bbcc92d2-7772-49c9-8556-0270037cd606}</MetaDataID>
    public enum DragDropTransactionOptions
    {
        None,
        UseDestinationTransaction, 
        UseSourceTransaction, 
        RequiresNewOnMoveForDifferentTransactions, 
        RequiresNewOnMove 

    }
    /// <MetaDataID>{ebed0772-640c-49ed-b10f-9bf06b11ef58}</MetaDataID>
    public class DragDropActionManager
    {
        public readonly DragDropTransactionOptions TransactionOptions;
        public readonly object DragedObject;
        ICutCopyPasteMoveDragDrop Source;
        public DragDropActionManager(ICutCopyPasteMoveDragDrop source, object dragedObject, DragDropTransactionOptions transactionOptions)
        {
            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
            {
                using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {
                    TransactionOptions = transactionOptions;
                    DragedObject = dragedObject;
                    Source = source;
                    innerStateTransition.Consistent = true;
                }
                stateTransition.Consistent = true;
            }
        }
        [ThreadStatic]
        static internal bool UnderOwnedTransaction = false;
        public void DropObject(OOAdvantech.DragDropMethod dragDropMethod, ICutCopyPasteMoveDragDrop target)
        {

            if (dragDropMethod == DragDropMethod.Copy)
                target.PasteObject(DragedObject);
            else if (dragDropMethod == DragDropMethod.Move)
            {
                try
                {
                    UnderOwnedTransaction = true;
                    if ((TransactionOptions == DragDropTransactionOptions.RequiresNewOnMoveForDifferentTransactions &&
                        target.UserInterfaceObjectConnection.Transaction != Source.UserInterfaceObjectConnection.Transaction) ||
                        TransactionOptions == DragDropTransactionOptions.RequiresNewOnMove)
                    {

                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                        {
                            using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                            {
                                target.PasteObject(DragedObject);
                                Source.CutObject(DragedObject);
                                innerStateTransition.Consistent = true;
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else if (TransactionOptions == DragDropTransactionOptions.UseDestinationTransaction&&target.UserInterfaceObjectConnection.Transaction!=null)
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                        {
                            using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(target.UserInterfaceObjectConnection.Transaction))
                            {
                                target.PasteObject(DragedObject);
                                Source.CutObject(DragedObject);
                                innerStateTransition.Consistent = true;
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else if (TransactionOptions == DragDropTransactionOptions.UseSourceTransaction && Source.UserInterfaceObjectConnection.Transaction != null)
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                        {
                            using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(Source.UserInterfaceObjectConnection.Transaction))
                            {
                                target.PasteObject(DragedObject);
                                Source.CutObject(DragedObject);
                                innerStateTransition.Consistent = true;
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        UnderOwnedTransaction = false;
                        target.PasteObject(DragedObject);
                        Source.CutObject(DragedObject);
                    }


                }
                finally
                {
                    UnderOwnedTransaction = false; 

                }
            }
        }

    }
}
