using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Transactions
{

    /// <MetaDataID>{549C9A79-5E2F-48C0-A381-A8CB6C6C99FF}</MetaDataID>
    public interface ITransactionEnlistment
    {
        /// <summary>Objects state manager inform that abort changes of the transaction.</summary>
        /// <MetaDataID>{9DBE61D8-8431-48D2-B8A4-8DCF9FEF771C}</MetaDataID>
        void AbortRequestDone();
        ///// <summary>Objects state manager inform that is ready to commit transaction. This is phase one of the two-phase commit protocol.</summary>
        ///// <MetaDataID>{0F724D60-E65A-4D4B-A600-BCCD12EF7C6B}</MetaDataID>
        //void PrepareRequestDone();
        /// <summary>Objects state manager inform that commit the objects state that participated in transaction. This is phase two of the two-phase commit protocol.</summary>
        /// <MetaDataID>{2A0EEA53-9E3B-487B-9C37-A1D38BD69B8A}</MetaDataID>
        void CommitRequestDone();

        /// <summary>Objects state manager inform that is ready to commit transaction. This is phase one of the two-phase commit protocol.</summary>
        /// <MetaDataID>{0CA8B8A1-8AF8-40A4-8B8D-D1ACC45B844D}</MetaDataID>
        void PrepareRequestDone();

    }

}