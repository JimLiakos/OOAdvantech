using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{

    /// <MetaDataID>{afbfce71-1b8b-4bd3-924c-6a4b0a1afa82}</MetaDataID>
    public class FormObjectConnection : UserInterfaceObjectConnection
    {
        /// <MetaDataID>{9ec07818-825a-4e74-8d32-9848bae488bf}</MetaDataID>
        public FormObjectConnection(IPresentationContextViewControl presentationContextViewControl)
            : base(presentationContextViewControl)
        {

        }
        /// <MetaDataID>{a37144fa-fe47-4f78-93b6-c34b5e3baf4a}</MetaDataID>
        public override OOAdvantech.Transactions.Transaction Transaction
        {
            get
            {
                if (State == ViewControlObjectState.DesigneMode)
                    return null;
                if (!TransactionInitialized)
                    throw new System.Exception("Error in FormConnectionControl initialization");

                if (_Transaction != null)
                    return _Transaction;
                else if (MasterViewControlObject != null)
                {
                    return MasterViewControlObject.Transaction;
                }
                if (_Transaction is OOAdvantech.Transactions.CommittableTransaction)
                    return (_Transaction as OOAdvantech.Transactions.CommittableTransaction).Transaction;
                else
                    return _Transaction;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _RollbackOnNegativeAnswer = true;
        /// <MetaDataID>{cae4265e-49c3-4ff6-9d9f-742c2abd4275}</MetaDataID>
        public bool RollbackOnNegativeAnswer
        {
            get
            {
                return _RollbackOnNegativeAnswer;
            }
            set
            {
                _RollbackOnNegativeAnswer = value;
            }
        }
        /// <exclude>Excluded</exclude>
        protected internal bool _RollbackOnExitWithoutAnswer = true;
        /// <MetaDataID>{4ea151df-c286-4c32-b6ea-5382f1b1772b}</MetaDataID>
        public bool RollbackOnExitWithoutAnswer
        {
            get
            {
                return _RollbackOnExitWithoutAnswer;
            }
            set
            {
                _RollbackOnExitWithoutAnswer = value;
            }
        }

        /// <MetaDataID>{81963535-a76d-43cb-b063-8dac6154a04a}</MetaDataID>
        bool _CreatePresentationObjectAnyway = false;
        /// <MetaDataID>{78e172a6-8af1-4549-a7b0-1f762ae6cbfb}</MetaDataID>
        public bool CreatePresentationObjectAnyway
        {
            get
            {
                return _CreatePresentationObjectAnyway;
            }
            set
            {
                _CreatePresentationObjectAnyway = value;
            }
        }

        /// <MetaDataID>{26dda2be-2675-4a4e-81c0-a7d26721578b}</MetaDataID>
        public override UserInterfaceObjectConnection MasterViewControlObject
        {
            get
            {
                return null;
            }
            set
            {
                if (value != null && value != this)
                    throw new System.Exception("The FormConnectionControl can't be child in ViewControlObject chain");
            }
        }
        /// <MetaDataID>{bcf4669c-9625-407b-af78-b7259d192122}</MetaDataID>
        public override void HostFormClosed(DialogResult dialogResult)
        {
            if ((dialogResult == DialogResult.OK ||
                (dialogResult == DialogResult.None && !_RollbackOnExitWithoutAnswer)
                || !RollbackOnNegativeAnswer))
            {
                //Commit transaction
                SaveControlData();
                State = ViewControlObjectState.Passive;

                ReleaseDataPathNodes();
                if (_Transaction is OOAdvantech.Transactions.CommittableTransaction && TransactionOwner)
                {
                    if (_Transaction.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                    {
                        try
                        {
                            RaiseBeforeTransactionCommit();
                            (_Transaction as OOAdvantech.Transactions.CommittableTransaction).Commit();
                        }
                        catch (Exception error)
                        {
                            _Transaction.Abort(error);
                        }
                    }
                }
            }
            else
            {
                State = ViewControlObjectState.Passive;
                ReleaseDataPathNodes();
                if (_Transaction != null && _Transaction.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                    _Transaction.Abort(null);
            }
            _Transaction = null;
            TransactionInitialized = false;
            base.HostFormClosed(dialogResult);
        }




        /// <MetaDataID>{fd8328bc-ff97-41d3-813a-45be924c7782}</MetaDataID>
        public override bool IniateTransactionOnInstanceSet
        {
            get
            {
                return base.IniateTransactionOnInstanceSet;
            }
            set
            {

            }
        }

        /// <MetaDataID>{378ddad0-223e-4865-ac23-8a0bcbd4bae9}</MetaDataID>
        public void HostFormLoad()
        {

            //for initialization
            MetaDataRepository.Classifier presentationObjectType = PresentationObjectType;

            State = ViewControlObjectState.Initialize;


            InitiateTransaction();
            using (OOAdvantech.Transactions.SystemStateTransition mstateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
            {
                if (_Transaction != null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(_Transaction))
                    {
                        try
                        {
                            if (UserInterfaceSession.StartingUserInterfaceObjectConnection == null)
                                UserInterfaceSession.StartingUserInterfaceObjectConnection = this;
                            PresentationContextViewControl.OnBeforeViewControlObjectInitialization();
                            if (CreatePresentationObjectAnyway)
                                _PresentationObject = GetPresentationObject(null, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);
                            LoadControlsData();
                            PresentationContextViewControl.OnAfterViewControlObjectInitialization();
                        }
                        finally
                        {
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                {

                    _UserInterfaceSession = UISession.CreateUserInterfaceSession(this);
                    UsedUISessions.Add(_UserInterfaceSession);
                    PresentationContextViewControl.OnBeforeViewControlObjectInitialization();
                    LoadControlsData();
                    PresentationContextViewControl.OnAfterViewControlObjectInitialization();
                    UserInterfaceSession.Synchronize();
                }
                foreach (IPresentationObject presentationObject in PresentationObjects.Values)
                {
                    try
                    {
                        presentationObject.Initialize();
                    }
                    catch (Exception error)
                    {
                    }
                }
                mstateTransition.Consistent = true;
            }
            State = ViewControlObjectState.UserInteraction;
            if (Transaction != null)
                Transaction.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);

            return;

            //###################### OLD Code;
            if (TransactionOption == OOAdvantech.Transactions.TransactionOption.Required ||
              TransactionOption == OOAdvantech.Transactions.TransactionOption.Supported)
            {
                if (_Transaction == null)
                    _Transaction = OOAdvantech.Transactions.Transaction.Current;
                if (_Transaction == null && TransactionOption == OOAdvantech.Transactions.TransactionOption.Required)
                {
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                    TransactionOwner = true;
                }

                TransactionInitialized = true;
            }
            if (TransactionOption == OOAdvantech.Transactions.TransactionOption.RequiredNested &&
                 _Transaction == null)
            {
                if (OOAdvantech.Transactions.Transaction.Current != null)
                {
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction(OOAdvantech.Transactions.Transaction.Current);
                    TransactionOwner = true;
                }
                else
                {
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                    TransactionOwner = true;
                }

                TransactionInitialized = true;
            }
            if (TransactionOption == OOAdvantech.Transactions.TransactionOption.RequiresNew)
            {
                using (OOAdvantech.Transactions.SystemStateTransition mstateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                {
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                    TransactionOwner = true;
                    TransactionInitialized = true;
                    if (Transaction != null)
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                        {
                            try
                            {
                                //TODO //if (_Transaction is OOAdvantech.Transactions.CommittableTransaction)
                                if (UserInterfaceSession.StartingUserInterfaceObjectConnection == null)
                                    UserInterfaceSession.StartingUserInterfaceObjectConnection = this;

                                PresentationContextViewControl.OnBeforeViewControlObjectInitialization();
                                if (CreatePresentationObjectAnyway)
                                    _PresentationObject = GetPresentationObject(null, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                                LoadControlsData();
                                PresentationContextViewControl.OnAfterViewControlObjectInitialization();
                            }
                            finally
                            {
                                stateTransition.Consistent = true;
                            }
                        }
                    }
                    else
                    {
                        PresentationContextViewControl.OnBeforeViewControlObjectInitialization();

                        if (CreatePresentationObjectAnyway)
                            _PresentationObject = GetPresentationObject(null, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                        LoadControlsData();
                        PresentationContextViewControl.OnAfterViewControlObjectInitialization();

                    }

                }
            }
            else
            {
                TransactionInitialized = true;
                if (Transaction != null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                    {
                        try
                        {
                            if (UserInterfaceSession.StartingUserInterfaceObjectConnection == null)
                                UserInterfaceSession.StartingUserInterfaceObjectConnection = this;

                            PresentationContextViewControl.OnBeforeViewControlObjectInitialization();

                            LoadControlsData();
                            PresentationContextViewControl.OnAfterViewControlObjectInitialization();
                            UserInterfaceSession.Synchronize();
                            foreach (IPresentationObject presentationObject in PresentationObjects.Values)
                            {
                                try
                                {
                                    presentationObject.Initialize();
                                }
                                catch (Exception error)
                                {
                                }
                            }

                        }
                        catch (System.Exception error)
                        {
                            throw;
                        }
                        finally
                        {
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                {

                    using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Suppress))
                    {
                        _UserInterfaceSession = UISession.CreateUserInterfaceSession(this);
                        PresentationContextViewControl.OnBeforeViewControlObjectInitialization();
                        LoadControlsData();
                        PresentationContextViewControl.OnAfterViewControlObjectInitialization();
                        UserInterfaceSession.Synchronize();
                        stateTransition.Consistent = true;
                    }


                }

            }
            State = ViewControlObjectState.UserInteraction;
            if (Transaction != null)
                Transaction.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);


        }
        delegate void DisableAllControlsHandle();
        /// <MetaDataID>{b40da5db-ef75-427b-923b-1f1758b229e7}</MetaDataID>
        void OnTransactionCompletted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);
            if (!BeContinue)
            {
                if (State != ViewControlObjectState.Passive)
                {
                    Transactions.Transaction newTransaction = null;
                    if (transaction.Status == Transactions.TransactionStatus.Committed && OnSaveReplacedTransaction.TryGetValue(transaction.LocalTransactionUri, out newTransaction))
                    {
                        _Transaction = newTransaction;
                        Transaction.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);
                    }
                    else
                    {
                        State = ViewControlObjectState.Passive;
                        if (PresentationContextViewControl.InvokeRequired)
                            PresentationContextViewControl.SynchroInvoke(new DisableAllControlsHandle(PresentationContextViewControl.DisableAllControls));
                        else
                            PresentationContextViewControl.DisableAllControls();
                    }
                }
            }
        }



    }
}
