using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;


namespace OOAdvantech.UserInterface.Runtime
{

    public delegate void MemberChangeHandler(object sender, MemberChangeEventArg change);
    public delegate void MemberLockStateChangeHandler(object sender);
    /// <MetaDataID>{99b65607-87cc-4537-8ea2-9cd8b838ee99}</MetaDataID>
    public enum ChangeType
    {
        ValueChanged,
        ItemsAdded,
        ItemsRemoved,
        None,
        LockChanged
    }

    /// <MetaDataID>{32995eb3-9a6d-4631-86d1-d6a2221f29ba}</MetaDataID>
    public struct MemberChange
    {
        /// <MetaDataID>{c66c5556-da55-4d0b-9c82-f2652211cfe8}</MetaDataID>
        public ChangeType Type;
        /// <MetaDataID>{c3536c8a-e671-4bc9-ad7c-32b655696c94}</MetaDataID>
        public int Index;
        /// <MetaDataID>{414198a6-24b2-4c6a-a84b-b91470404e56}</MetaDataID>
        internal DisplayedValue DisplayedValue;
        /// <MetaDataID>{19e7199b-2150-4964-948e-478f702426f7}</MetaDataID>
        public object Value
        {
            get
            {
                if (DisplayedValue == null)
                    return null;
                else
                    return DisplayedValue.Value;

            }
        }

    }
    /// <MetaDataID>{84b60d6c-046f-4a2f-9224-269885787b5c}</MetaDataID>
    public struct MemberChangeEventArg
    {
        /// <MetaDataID>{7d8b9ce3-3f32-4a32-b006-627776885018}</MetaDataID>
        internal MemberChangeEventArg(Member member, ChangeType type, DisplayedValue value, DisplayedValue memberOwner)
            : this(member, type, value, -1, memberOwner)
        {

        }
        /// <MetaDataID>{9c5d7990-93fc-4c8c-b71d-5986fe568660}</MetaDataID>
        public readonly object MemberOwner;
        /// <MetaDataID>{b9c8516d-d3af-469c-85e7-4b43cf3d39e1}</MetaDataID>
        public readonly string MemberName;

        /// <MetaDataID>{05830d33-82c6-49e2-b08e-665b262e948a}</MetaDataID>
        internal MemberChangeEventArg(Member member, ChangeType type, DisplayedValue value, int index, DisplayedValue memberOwner)
        {
            MemberOwner = memberOwner.Value;
            Type = type;
            Value = value;
            Index = index;
            Member = member;
            MemberName = member.Name;
            Transaction = OOAdvantech.Transactions.Transaction.Current;
        }
        /// <MetaDataID>{2e3e1fc8-dfc3-4029-b5c1-a037dea4309e}</MetaDataID>
        internal MemberChangeEventArg(Member member, DisplayedValue value, DisplayedValue memberOwner)
        {
            MemberOwner = memberOwner.Value;
            Type = ChangeType.ValueChanged;
            Value = value;
            Index = -1;
            Member = member;
            MemberName = member.Name;
            Transaction = OOAdvantech.Transactions.Transaction.Current;
        }


        /// <MetaDataID>{86afd261-c6cb-4630-8d34-2b6581092735}</MetaDataID>
        internal readonly Member Member;
        /// <MetaDataID>{395fefa7-639b-4091-b1db-910b4aa95979}</MetaDataID>
        public readonly ChangeType Type;
        /// <MetaDataID>{3ce8df91-b8e4-4291-a9e3-c7017c0d4bae}</MetaDataID>
        internal readonly int Index;
        /// <MetaDataID>{43591699-7e1a-4e40-99e9-e3d358d9290c}</MetaDataID>
        internal readonly DisplayedValue Value;
        /// <MetaDataID>{192bd291-e69d-4258-8c08-adf8c35ab916}</MetaDataID>
        internal OOAdvantech.Transactions.Transaction Transaction;
    }



    /// <MetaDataID>{E309AE73-F63A-4516-AF2A-D9D22529BA43}</MetaDataID>
    [Transactional]
    internal class Member : ITransactionalObject
    {
        class MemberValue
        {
            public MemberValue(List<DisplayedValue> memberValues, bool hasChange)
            {
                _HasChange = hasChange;
                MemberValues = memberValues;
            }
            bool _HasChange;
            public bool HasChange
            {
                get
                {
                    return _HasChange;
                }
                set
                {
                    _HasChange = value;
                }
            }


            public List<DisplayedValue> MemberValues;
        }
        /// <MetaDataID>{597ef21f-ee79-4907-bdd1-d33e5f36c5e4}</MetaDataID>
        [MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        [MetaDataRepository.Association("ObjectMembers", typeof(DisplayedValue), MetaDataRepository.Roles.RoleB, "79eeb7a7-8292-42f1-873e-6335b481e182")]
        [MetaDataRepository.IgnoreErrorCheck]
        internal DisplayedValue Owner;

        delegate void UpdateUserInterfaceHandle();
        /// <MetaDataID>{f00b555b-f4d1-4096-9c66-8fc619450eb7}</MetaDataID>
        bool _Locked = false;
        /// <MetaDataID>{b4948e23-7eb1-4600-adf9-68b802944b6e}</MetaDataID>
        public bool Locked
        {
            private get
            {
                if (Owner != null && Owner.UserInterfaceSession.State == UISession.SessionState.Terminated)
                    return true;
                return _Locked;
            }
            set
            {
                if (_Locked != value)
                {
                    _Locked = value;
                    if (Change != null && !SuspendMemberValueUpdate)
                    {


                        if (Name == "Items")
                            return;

                        if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                        {

                            if (MemberChangeEvents == null)
                                MemberChangeEvents = new Queue<MemberChangeEventArg>();
                            MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.LockChanged, null, Owner));
                        }
                        else
                        {
                            try
                            {
                                Owner.UserInterfaceSession.StartControlValuesUpdate();
                                LockStateChange(this);
                                //Change(this, new MemberChangeEventArg(this, ChangeType.LockChanged, null, -1));
                            }
                            finally
                            {
                                Owner.UserInterfaceSession.EndControlValuesUpdate();
                            }
                        }
                    }
                }
                else
                    _Locked = value;

            }
        }
        /// <MetaDataID>{eaebb12c-1105-481f-ab0f-e82fd0e4866a}</MetaDataID>
        internal bool IsLockedFor(ObjectChangeStateManager pathDataDisplayer)
        {
            if (Owner == null || !_Locked)
                return _Locked;

            Transaction lockTransaction = Owner.LockTransaction;
            if (lockTransaction == null || lockTransaction.Status != TransactionStatus.Continue)
                return _Locked;

            if (pathDataDisplayer.PathDataDisplayer.UserInterfaceObjectConnection == null)
                return _Locked;

            Transaction pathDataDisplayerTransaction = pathDataDisplayer.PathDataDisplayer.UserInterfaceObjectConnection.Transaction;
            if (pathDataDisplayerTransaction == lockTransaction)
                return false;

            while (pathDataDisplayerTransaction != null && pathDataDisplayerTransaction.OriginTransaction != null && pathDataDisplayerTransaction != lockTransaction)
                pathDataDisplayerTransaction = pathDataDisplayerTransaction.OriginTransaction;
            if (pathDataDisplayerTransaction == lockTransaction)
                return false;
            else
                return true;

        }






        /// <MetaDataID>{75d0e383-d8df-4382-b899-7f5f8f491d2e}</MetaDataID>
        [MetaDataRepository.Association("DisplayMemberValues", typeof(ObjectChangeStateManager), MetaDataRepository.Roles.RoleA, "7f7a9ce6-114c-4a91-9123-9d9dbb4f4f17")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        public System.Collections.Generic.List<ObjectChangeStateManager> PathDataDisplayers = new List<ObjectChangeStateManager>();


        /// <MetaDataID>{4995ab5d-a0bb-4f72-8647-0b653c901320}</MetaDataID>
        [MetaDataRepository.Association("ValuesOfMember", typeof(DisplayedValue), MetaDataRepository.Roles.RoleA, "40f1c173-5fba-437f-a23a-ae0df794f26a")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.IgnoreErrorCheck]
        readonly Dictionary<string, MemberValue> Values;

        public event MemberChangeHandler Change;


        public event MemberLockStateChangeHandler LockStateChange;
        /// <MetaDataID>{e80b99b9-2bc9-42be-85bf-04b5329e17da}</MetaDataID>
        static DisplayedValue operationPseudoDisplayedValue = new DisplayedValue(Guid.NewGuid(), null);

        /// <MetaDataID>{afad4fb1-eeb2-490c-aa95-d69dabe6328f}</MetaDataID>
        static void GetAllNestedTransactions(Transaction transaction, List<Transaction> nestedTransactions)
        {
            nestedTransactions.AddRange(transaction.NestedTransactions);
            foreach (Transaction nestedTransaction in transaction.NestedTransactions)
                GetAllNestedTransactions(nestedTransaction, nestedTransactions);
        }
        /// <MetaDataID>{32d222b5-f19a-48c1-8292-76caa39bf168}</MetaDataID>
        System.Collections.Generic.Queue<MemberChangeEventArg> MemberChangeEvents;
        /// <MetaDataID>{ADC34E08-D709-4033-A227-1622F21CFD2B}</MetaDataID>
        internal DisplayedValue this[int index]
        {
            get
            {


                if (IsOperation)
                    return operationPseudoDisplayedValue;


                string localTransactionUri = "null_transaction";
                if (Transaction.Current != null)
                {
                    //In case where there are values for the current transactio return this value
                    //otherwise look in nested transaction chain if there aren't values 
                    Transaction currTransaction = Transaction.Current;
                    while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()))
                        currTransaction = currTransaction.OriginTransaction;
                    if (currTransaction != null)
                        localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                }
                if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] == null)
                    Values.Remove(localTransactionUri);

                if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri].MemberValues.Count > index)
                    return Values[localTransactionUri].MemberValues[index];

                if (Transaction.Current != null)
                {
                    List<Transaction> nestedTransactions = new List<Transaction>();
                    GetAllNestedTransactions(Transaction.Current, nestedTransactions);
                    foreach (Transaction transaction in nestedTransactions)
                    {
                        MemberValue memberValue = null;
                        //List<DisplayedValue> values = null;

                        if (Values.TryGetValue(transaction.LocalTransactionUri, out memberValue))
                        {
                            if (memberValue != null && memberValue.MemberValues != null)
                                return memberValue.MemberValues[index];
                        }
                    }
                }
                return null;// new DisplayedValue(null, Owner.ViewControlObject);
            }
            set
            {
                if (!IsOperation)
                {
                    //TODO στο σεναρια set object value, και object change state σεταριζόταν η value 
                    //με SuspendEvent και δεν γινόταν fire change event όταν το uissesion σετάριζε το member παραάκτω
                    //χωρις SuspendEvent η value δεν άλλαζε και δεν γινόταν παλι fire change event.
                    if (SuspendMemberValueUpdate)
                        return;
                    //********************


                    bool sameValue = false;
                    if (Owner.UserInterfaceSession.State == UISession.SessionState.UpdateCashingData)
                    {
                        sameValue = true;
                        if (Owner.Value != null && Owner.Value.GetType().IsValueType && ObjectStateTransition.GetTransaction(this).Count == 0)
                        {
                            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                            {

                                foreach (string key in new List<string>(Values.Keys))
                                {
                                    if (Values[key] == null)
                                    {
                                        Values[key] = new MemberValue(new List<DisplayedValue>(), false);
                                        Values[key].MemberValues.Add(value);
                                    }
                                    else
                                    {
                                        if (Values[key].MemberValues[index] != value)
                                        {
                                            sameValue = false;
                                            Values[key].MemberValues[index] = value;
                                        }
                                    }
                                }
                                stateTransition.Consistent = true;
                            }


                        }
                        else
                        {
                            foreach (string key in new List<string>(Values.Keys))
                            {
                                if (Values[key] == null)
                                {
                                    Values[key] = new MemberValue(new List<DisplayedValue>(), false);
                                    Values[key].MemberValues.Add(value);
                                }
                                else
                                {
                                    if (Values[key].MemberValues[index] != value)
                                    {
                                        sameValue = false;
                                        Values[key].MemberValues[index] = value;
                                    }
                                }
                            }
                        }


                    }
                    else
                    {
                        if (OOAdvantech.Transactions.ObjectStateTransition.IsLocked(this))
                            return;

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                        {
                            string localTransactionUri = "null_transaction";

                            Transaction currTransaction = Transaction.Current;

                            if (currTransaction != null)
                            {
                                while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                                    currTransaction = currTransaction.OriginTransaction;

                                localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                            }
                            if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] == null)
                                Values.Remove(localTransactionUri);


                            MemberValue memberValue = null;
                            if (!Values.TryGetValue(localTransactionUri, out memberValue))
                            {
                                Values[localTransactionUri] = new MemberValue(new List<DisplayedValue>(10), false);
                            }
                            if (memberValue == null)
                            {
                                Values[localTransactionUri] = new MemberValue(new List<DisplayedValue>(10), false);
                            }


                            if (Values[localTransactionUri].MemberValues.Count == 0 && index == 0)
                            {
                                Values[localTransactionUri].MemberValues.Add(value);
                                if (PathDataDisplayers.Count > 0)
                                    Values[localTransactionUri].HasChange = true;

                                //index = Values[localTransactionUri].Count;
                            }
                            else
                            {
                                if (Values[localTransactionUri].MemberValues[index] != value)
                                {
                                    Values[localTransactionUri].MemberValues[index] = value;
                                    if (PathDataDisplayers.Count > 0)
                                        Values[localTransactionUri].HasChange = true;

                                }
                                else
                                    sameValue = true;
                            }

                            //    if (currTransaction == null || currTransaction.OriginTransaction == null)
                            //        break;
                            //    else
                            //    {
                            //        currTransaction = currTransaction.OriginTransaction;
                            //        localTransactionUri = currTransaction.LocalTransactionUri;
                            //    }
                            //}
                            //while (currTransaction != null && currTransaction.OriginTransaction != null)

                            ////while (currTransaction != null && currTransaction.OriginTransaction != null)
                            ////    currTransaction = currTransaction.OriginTransaction;


                            stateTransition.Consistent = true;
                        }
                    }
                    if (sameValue)
                        return;
                }
                if (Change != null && !SuspendMemberValueUpdate)
                {


                    if (Name == "Items")
                        return;

                    if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                    {

                        if (MemberChangeEvents == null)
                            MemberChangeEvents = new Queue<MemberChangeEventArg>();
                        MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.ValueChanged, null, Owner));
                    }
                    else
                    {
                        if (Owner.UserInterfaceSession.State != OOAdvantech.UserInterface.Runtime.UISession.SessionState.UpdateCashingData)
                        {
                            try
                            {
                                Owner.UserInterfaceSession.StartControlValuesUpdate();
                                Change(this, new MemberChangeEventArg(this, ChangeType.ValueChanged, null, Owner));
                            }
                            finally
                            {
                                Owner.UserInterfaceSession.EndControlValuesUpdate();
                            }
                        }
                        else
                        {
                            if (MemberChangeEvents == null)
                                MemberChangeEvents = new Queue<MemberChangeEventArg>();
                            MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.ValueChanged, null, Owner));

                        }
                    }
                }
            }
        }

        /// <MetaDataID>{a2f1b079-b41a-409f-926e-bfe9118470f0}</MetaDataID>
        internal bool Loaded
        {
            get
            {

                if (IsOperation)
                    return true;
                string localTransactionUri = "null_transaction";
                if (Transaction.Current != null)
                {
                    Transaction currTransaction = Transaction.Current;
                    while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()))
                        currTransaction = currTransaction.OriginTransaction;
                    if (currTransaction != null)
                        localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                }
                if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{fa2cf513-f329-4745-822c-5654aaa71a86}</MetaDataID>
        internal void Remove(DisplayedValue value)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                string localTransactionUri = "null_transaction";

                Transaction currTransaction = Transaction.Current;
                if (currTransaction != null)
                {
                    while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                        currTransaction = currTransaction.OriginTransaction;

                    localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                }
                int index = -1;
                if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] == null)
                    Values.Remove(localTransactionUri);


                if (!Values.ContainsKey(localTransactionUri))
                    Values[localTransactionUri] = new MemberValue(new List<DisplayedValue>(), false);
                if (Values[localTransactionUri].MemberValues.Contains(value))
                {
                    index = Values[localTransactionUri].MemberValues.IndexOf(value);
                    Values[localTransactionUri].MemberValues.Remove(value);
                    if (PathDataDisplayers.Count > 0)
                        Values[localTransactionUri].HasChange = true;


                    if (Change != null && !SuspendMemberValueUpdate)
                    {

                        if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                        {
                            if (MemberChangeEvents == null)
                                MemberChangeEvents = new Queue<MemberChangeEventArg>();
                            MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.ItemsRemoved, value, index, Owner));

                        }
                        else
                        {
                            try
                            {
                                Owner.UserInterfaceSession.StartControlValuesUpdate();
                                Change(this, new MemberChangeEventArg(this, ChangeType.ItemsRemoved, value, index, Owner));
                            }
                            finally
                            {
                                Owner.UserInterfaceSession.EndControlValuesUpdate();
                            }
                        }
                    }

                }
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{1da97004-ee69-4bc1-8105-0d19903dd268}</MetaDataID>
        internal void Add(DisplayedValue value)
        {
            Add(value, -1);
        }
        /// <MetaDataID>{0A13A7C0-F795-4DAD-8A73-AA9A49E3D613}</MetaDataID>
        internal void Add(DisplayedValue value, int index)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                try
                {
                    string localTransactionUri = "null_transaction";

                    Transaction currTransaction = Transaction.Current;
                    if (currTransaction != null)
                    {
                        while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                            currTransaction = currTransaction.OriginTransaction;

                        localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                    }

                    if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] == null)
                        Values.Remove(localTransactionUri);
                    bool initialize = false;
                    if (!Values.ContainsKey(localTransactionUri))
                    {
                        Values[localTransactionUri] = new MemberValue(new List<DisplayedValue>(), false);
                        initialize = true;
                    }

                    if (!Values[localTransactionUri].MemberValues.Contains(value))
                    {
                        if (index == -1)
                        {
                            Values[localTransactionUri].MemberValues.Add(value);
                            index = Values[localTransactionUri].MemberValues.Count - 1;
                        }
                        else if (index < Values[localTransactionUri].MemberValues.Count)
                            Values[localTransactionUri].MemberValues.Insert(index, value);
                        else
                        {
                            Values[localTransactionUri].MemberValues.Add(value);
                            index = Values[localTransactionUri].MemberValues.Count - 1;
                        }
                        if (PathDataDisplayers.Count > 0)
                            Values[localTransactionUri].HasChange = true;


                        if (Change != null && !SuspendMemberValueUpdate)
                        {


                            //if (Name == "Items")
                            //    return;
                            if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                            {

                                if (MemberChangeEvents == null)
                                    MemberChangeEvents = new Queue<MemberChangeEventArg>();
                                MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.ItemsAdded, value, index, Owner));
                            }
                            else
                            {
                                try
                                {
                                    Owner.UserInterfaceSession.StartControlValuesUpdate();
                                    Change(this, new MemberChangeEventArg(this, ChangeType.ItemsAdded, value, index, Owner));
                                }
                                finally
                                {
                                    Owner.UserInterfaceSession.EndControlValuesUpdate();
                                }
                            }
                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                stateTransition.Consistent = true;
            }



        }
        /// <MetaDataID>{24F1B4B8-531A-4967-AFDF-8333DCFB494C}</MetaDataID>
        Member()
        {
            Values = new Dictionary<string, MemberValue>();
        }

        /// <MetaDataID>{12be348e-108e-471a-b633-b115fc164e5b}</MetaDataID>
        internal readonly Type OwnerType;

        /// <MetaDataID>{3072A5BC-BFAB-448F-BC5A-9F29C178E143}</MetaDataID>
        internal Member(string name, DisplayedValue owner, System.Type ownerType, System.Type type)
            : this()
        {
            try
            {
                MemberType = type;
                Owner = owner;
                Name = name;
                OwnerType = ownerType;
                if (OwnerType == null && owner != null)
                    OwnerType = Owner.Value.GetType();
            }
            catch (System.Exception error)
            {
                throw;
            }







        }
        /// <MetaDataID>{c8c92e6e-78f9-44fa-9a0f-964a2d51eee9}</MetaDataID>
        public readonly Type MemberType;
        /// <MetaDataID>{964f5b16-90b1-4633-9abe-8b951d67a500}</MetaDataID>
        public readonly bool IsOperation = false;
        /// <MetaDataID>{130084d4-a03c-41d5-ac0b-bf659be0e5c6}</MetaDataID>
        internal Member(string name, DisplayedValue owner, System.Type ownerType, System.Type type, bool isOperation)
            : this()
        {
            try
            {
                MemberType = type;
                IsOperation = isOperation;
                Owner = owner;
                Name = name;
                OwnerType = ownerType;
                if (OwnerType == null)
                    OwnerType = Owner.Value.GetType();
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{cd9aa85e-396c-4913-a580-ea13badf4b80}</MetaDataID>
        public bool IsEnumerable
        {
            get
            {
                //TODO κανονικά το MemberType δεν πρέπει να είναι null
                if (MemberType == null)
                    return false;
                return OOAdvantech.TypeHelper.IsEnumerable(MemberType);
            }
        }
        /// <MetaDataID>{5114fc48-ebc9-4ed3-aaf3-4b7baf1597cc}</MetaDataID>
        public bool HasValuesCollection
        {
            get
            {
                string localTransactionUri = "null_transaction";
                if (Transaction.Current != null)
                {
                    //In case where there are values for the current transactio return this value
                    //otherwise look in nested transaction chain if there aren't values 
                    Transaction currTransaction = Transaction.Current;
                    while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                        currTransaction = currTransaction.OriginTransaction;
                    if (currTransaction != null)
                        localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                }
                if (Values != null && Values.ContainsKey(localTransactionUri) && IsEnumerable)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{5b4584b5-e215-4f15-9415-1c20b30c1a85}</MetaDataID>
        internal bool SuspendMemberValueUpdate = false;
        /// <MetaDataID>{375e7a6d-f56f-4276-b894-d63e2f5313b6}</MetaDataID>
        internal List<DisplayedValue> ValuesCollection
        {
            get
            {
                try
                {
                    SuspendMemberValueUpdate = true;
                    //Λύνει το πρόβλημα όπου σε ένα insert row κατευαίνει στο buisness model 
                    //έρχετε πίσω αλλά δέν υπάρχει member και ο κώδικας σκάει εδώ φορτώνεται χωρ
                    //if (Owner.UserInterfaceSession.State == UISession.SessionState.UserInteraction)
                    //    SuspendMemberValueUpdate = false;
                    string localTransactionUri = "null_transaction";
                    if (Transaction.Current != null)
                    {
                        //In case where there are values for the current transactio return this value
                        //otherwise look in nested transaction chain if there aren't values 
                        Transaction currTransaction = Transaction.Current;
                        while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                            currTransaction = currTransaction.OriginTransaction;
                        if (currTransaction != null)
                            localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                    }
                    if (Values.ContainsKey(localTransactionUri) && Values[localTransactionUri] == null)
                        Values.Remove(localTransactionUri);

                    //TODO
                    if (Values.Count == 0 && Owner.Value != null && Owner.Value.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
                        && Owner.Value.GetType().GetMethod("GetEnumerator").ReturnType.IsGenericType)
                    {
                        System.Collections.IEnumerator enumerator = Owner.Value.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(Owner.Value, new object[0]) as System.Collections.IEnumerator;
                        enumerator.Reset();
                        while (enumerator.MoveNext())
                        {
                            DisplayedValue displayedObj = this.Owner.UserInterfaceSession.GetDisplayedValue(enumerator.Current);
                            Add(displayedObj);
                        }
                    }
                    else if (!Values.ContainsKey(localTransactionUri))
                    {
                        if (Owner.Value != null)
                        {
                            System.Collections.IEnumerator enumerator = Owner.Value.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(Owner.Value, new object[0]) as System.Collections.IEnumerator;
                            enumerator.Reset();
                            while (enumerator.MoveNext())
                            {
                                DisplayedValue displayedObj = this.Owner.UserInterfaceSession.GetDisplayedValue(enumerator.Current);
                                Add(displayedObj);
                            }
                        }
                    }

                    if (Transaction.Current != null)
                    {
                        //In case where there are values for the current transactio return this value
                        //otherwise look in nested transaction chain if there aren't values 
                        Transaction currTransaction = Transaction.Current;
                        while (currTransaction != null && !Values.ContainsKey(currTransaction.LocalTransactionUri.ToString()) && currTransaction.OriginTransaction != null)
                            currTransaction = currTransaction.OriginTransaction;
                        if (currTransaction != null)
                            localTransactionUri = currTransaction.LocalTransactionUri.ToString();
                    }
                    if (!Values.ContainsKey(localTransactionUri))
                    {
                        Values[localTransactionUri] = new MemberValue(new List<DisplayedValue>(), false);
                        return Values[localTransactionUri].MemberValues;


                    }

                    List<DisplayedValue> collection = new List<DisplayedValue>(Values[localTransactionUri].MemberValues);





                    return collection;
                }
                finally
                {
                    SuspendMemberValueUpdate = false;
                }

            }
        }

        //    /// <MetaDataID>{8C33E7F7-FBE6-43DA-8366-6456F23C50C5}</MetaDataID>
        //  readonly Dictionary<string, List<DisplayedValue>> Values;
        /// <MetaDataID>{BA6BE147-5575-4392-9EFD-1F2DA3D87951}</MetaDataID>
        public string Name;

        #region TransactionalObject Members

        /// <MetaDataID>{ED30ED6A-3E53-468B-9A9C-DADA5BCEAD89}</MetaDataID>
        public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            MemberValue memberValue = null;
            if (Values.TryGetValue(transaction.LocalTransactionUri, out memberValue))
            {
                if (memberValue != null)
                    memberValue.HasChange = false;
            }
        }
        /// <MetaDataID>{7905b269-cf97-48a7-857d-35f48cf86006}</MetaDataID>
        public void MarkChanges(Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{11B2754A-2E9D-4423-8E23-20B6E2958C38}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {




            if (transaction.OriginTransaction != null && !Values.ContainsKey(transaction.OriginTransaction.LocalTransactionUri.ToString()))
                MarkChanges(transaction.OriginTransaction);

            transaction.TransactionCompleted += new TransactionCompletedEventHandler(TransactionCompletted);
            if (transaction.OriginTransaction != null && Values.ContainsKey(transaction.OriginTransaction.LocalTransactionUri.ToString()) && Values[transaction.OriginTransaction.LocalTransactionUri.ToString()] != null)
            {

                List<DisplayedValue> oldTransactionValues = new List<DisplayedValue>(Values[transaction.OriginTransaction.LocalTransactionUri.ToString()].MemberValues);
                //if (IsEnumerable && this[0].Members.Count == 1 && this[0] != null && this[0].Members.ContainsKey("Items"))
                //{

                //    oldTransactionValues = new List<DisplayedValue>(Values[transaction.OriginTransaction.LocalTransactionUri.ToString()].MemberValues);
                //    var items = Values[transaction.OriginTransaction.LocalTransactionUri.ToString()].MemberValues[0];
                //    //MemberType.GetGenericArguments()[0]
                //    System.Collections.IList list = System.Activator.CreateInstance(typeof(List<>).MakeGenericType(DisplayedValue.GetElementType(items.Value.GetType()))) as System.Collections.IList;

                //    foreach (var obj in items.Members["Items"].ValuesCollection.Select(x => x.Value).ToList())
                //        list.Add(obj);

                //    DisplayedValue displayedValue = new DisplayedValue(list,  items.UserInterfaceSession);

                //   //oldTransactionValues = new List<DisplayedValue>() { displayedValue };
                //}
                //else
                //    oldTransactionValues = new List<DisplayedValue>(Values[transaction.OriginTransaction.LocalTransactionUri.ToString()].MemberValues);
                Values[transaction.LocalTransactionUri.ToString()] = new MemberValue(oldTransactionValues, Values[transaction.OriginTransaction.LocalTransactionUri.ToString()].HasChange);

            }
            else if (Values.ContainsKey("null_transaction"))
            {
                List<DisplayedValue> oldTransactionValues = new List<DisplayedValue>(Values["null_transaction"].MemberValues);
                Values[transaction.LocalTransactionUri.ToString()] = new MemberValue(oldTransactionValues, Values["null_transaction"].HasChange);
            }
            else
            {
                Values[transaction.LocalTransactionUri.ToString()] = null;

            }


        }
        /// <MetaDataID>{334d3b07-655f-42e6-8f22-96b23fe001dd}</MetaDataID>
        public void UpdateUserInterface()
        {
            try
            {
                if (Owner != null)
                {
                    if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                    {
                        if (MemberChangeEvents == null)
                            MemberChangeEvents = new Queue<MemberChangeEventArg>();
                        MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, null, Owner));

                    }
                    else
                    {

                        if (Change != null && !SuspendMemberValueUpdate)
                        {


                            if (Owner.UserInterfaceSession.StartingUserInterfaceObjectConnection.State != ViewControlObjectState.Passive)
                            {
                                try
                                {
                                    Owner.UserInterfaceSession.StartControlValuesUpdate();
                                    Change(this, new MemberChangeEventArg(this, null, Owner));
                                }
                                finally
                                {
                                    Owner.UserInterfaceSession.EndControlValuesUpdate();
                                }
                            }
                        }

                    }
                }
            }
            catch (System.Exception error)
            {
                throw;

            }

        }

        /// <MetaDataID>{5c8e049f-0216-47a8-ba9c-5e6a39b09eaa}</MetaDataID>
        void TransactionCompletted(Transaction transaction)
        {
            try
            {
                if (transaction.Status != TransactionStatus.Committed)
                {
                    if (transaction.OriginTransaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction.OriginTransaction))
                        {
                            UpdateUserInterface();
                            stateTransition.Consistent = true;
                        }


                    }
                    else
                    {
                        if (Owner == null || Owner.UserInterfaceSession.State != UISession.SessionState.Terminated)
                            UpdateUserInterface();




                    }


                }
                transaction.TransactionCompleted -= new TransactionCompletedEventHandler(TransactionCompletted);
            }
            catch (System.Exception Error)
            {
                throw;
            }

        }

        /// <MetaDataID>{F1CC1071-1452-4F82-93AE-9C5D79D12CA1}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {
            try
            {
                Values[masterTransaction.LocalTransactionUri.ToString()] = Values[nestedTransaction.LocalTransactionUri.ToString()];
                Values.Remove(nestedTransaction.LocalTransactionUri.ToString());
            }
            catch (System.Exception error)
            {
            }
        }

        public void EnsureSnapshot(Transaction transaction)
        {

        }

        /// <MetaDataID>{D33A6E16-23B6-4F8F-92B0-3137C416AA3B}</MetaDataID>
        public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            try
            {


                if (transaction.OriginTransaction != null && Values.ContainsKey(transaction.LocalTransactionUri.ToString()) &&
                Values.ContainsKey(transaction.OriginTransaction.LocalTransactionUri.ToString()))
                {
                    if (Change != null)
                    {

                        //if (Name == "Items") //'Items' member is embedded of enumerable member
                        //    return;          //transactions changes controlled from enumerable member    

                        if (Owner.UserInterfaceSession.StartingFormObjectConnection.InvokeRequired ||
                            Owner.UserInterfaceSession.CannotEnterUpdateControlValuesMode)
                        {

                            if (MemberChangeEvents == null)
                                MemberChangeEvents = new Queue<MemberChangeEventArg>();
                            MemberChangeEvents.Enqueue(new MemberChangeEventArg(this, ChangeType.ValueChanged, null, Owner));
                        }
                        else
                        {
                            try
                            {
                                Owner.UserInterfaceSession.StartControlValuesUpdate();
                                Change(this, new MemberChangeEventArg(this, ChangeType.ValueChanged, null, Owner));
                            }
                            finally
                            {
                                Owner.UserInterfaceSession.EndControlValuesUpdate();
                            }
                        }
                    }
                }
            }
            finally
            {
                Values.Remove(transaction.LocalTransactionUri.ToString());
                while (transaction.OriginTransaction != null)
                {
                    transaction = transaction.OriginTransaction;
                    if (Values.ContainsKey(transaction.LocalTransactionUri) && Values[transaction.LocalTransactionUri] == null)
                        Values.Remove(transaction.LocalTransactionUri.ToString());
                    else break;
                }
                if (Values.Count == 0 && Owner != null)
                {
                    if (!(Name == "Items" && Owner.IsEnumerable))//Items is auto generated
                        Owner.Members.Remove(Name);
                    else
                    {

                    }
                }
            }


        }

        #endregion

        ///// <MetaDataID>{67310b76-6022-4e47-8279-4df9f1609d43}</MetaDataID>
        //internal void GetPaths(List<string> paths)
        //{

        //    foreach (ObjectChangeStateManager pathDataDisplayer in PathDataDisplayers)
        //    {
        //        foreach (string path in pathDataDisplayer.PathDataDisplayer.Paths)
        //        {
        //            if (!paths.Contains(path))
        //                paths.Add(path);
        //        }
        //    }

        //    //System.Collections.Generic.List<string> accessedMembersFullNames, string path, List<string> paths

        //    //if(accessedMembersFullNames.Contains(OwnerType.FullName+"."+Name))
        //    //    return;
        //    //accessedMembersFullNames.Add(OwnerType.FullName+"."+Name);

        //    //if(!string.IsNullOrEmpty(path))
        //    //    path+=".";
        //    //path += Name;
        //    //if (ValuesCollection.Count == 0)
        //    //{
        //    //    paths.Add(path);
        //    //    return;
        //    //}
        //    //else
        //    //{
        //    //    if (IsEnumerable && this[0].MemberValues["Items"].ValuesCollection.Count>0)
        //    //    {

        //    //        foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in this[0].MemberValues["Items"][0].MemberValues)
        //    //            entry.Value.GetPaths(accessedMembersFullNames,path, paths);
        //    //        return;


        //    //        //this[0]

        //    //    }
        //    //    if (this[0].MemberValues.Count == 0)
        //    //    {
        //    //        paths.Add(path);
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in this[0].MemberValues)
        //    //            entry.Value.GetPaths(accessedMembersFullNames,path, paths);
        //    //    }
        //    //}




        //}



        /// <MetaDataID>{2666e553-9f9a-4095-9fda-9a195e21dd3b}</MetaDataID>
        internal void ReleasePathDataDisplayer()
        {
            foreach (ObjectChangeStateManager pathDataDisplayer in new List<ObjectChangeStateManager>(PathDataDisplayers))
            {
                RemovePathDataDisplayer(pathDataDisplayer);
                pathDataDisplayer.RemoveDataPathNode(this);
            }
        }
        /// <MetaDataID>{018ae4e0-3365-4097-8212-6ed4b339ac5b}</MetaDataID>
        public void RemovePathDataDisplayer(ObjectChangeStateManager pathDataDisplayer)
        {
            if (PathDataDisplayers.Contains(pathDataDisplayer))
                PathDataDisplayers.Remove(pathDataDisplayer);
            if (PathDataDisplayers.Count == 0 && Owner != null && Owner.UserInterfaceSession.StartingUserInterfaceObjectConnection != null)
                Owner.UserInterfaceSession.StartingUserInterfaceObjectConnection.RefreshPathDataDisplayers -= new EventHandler(OnRefreshPathDataDisplayers);
        }

        /// <MetaDataID>{0b0d1b3c-816d-4e00-8f76-20416b3650cb}</MetaDataID>
        public void AddPathDataDisplayer(ObjectChangeStateManager pathDataDisplayer)
        {
            if (pathDataDisplayer != null && pathDataDisplayer.PathDataDisplayer.HasLockRequest)
                Owner.HasLockRequest = true;

            if (!PathDataDisplayers.Contains(pathDataDisplayer))
                PathDataDisplayers.Add(pathDataDisplayer);
            if (PathDataDisplayers.Count == 1 && Owner != null && Owner.UserInterfaceSession.StartingUserInterfaceObjectConnection != null)
                Owner.UserInterfaceSession.StartingUserInterfaceObjectConnection.RefreshPathDataDisplayers += new EventHandler(OnRefreshPathDataDisplayers);


            //if (IsEnumerable)
            //{
            //    try
            //    {
            //        if (pathDataDisplayer != null)
            //        {
            //            if (this[0] != null)
            //            {
            //                //TODO Γιατί δέν υπάρχει member "Items"
            //                if (!this[0].Members.ContainsKey("Items"))
            //                {
            //                    System.Reflection.MethodInfo methodInfo = MemberType.GetMethod("GetEnumerator", new System.Type[0]);
            //                    this[0].Members.Add("Items", new Member("Items", this[0], MemberType, methodInfo.ReturnType.GetGenericArguments()[0]));
            //                }
            //                pathDataDisplayer.AddDataPathNode(this[0].Members["Items"]);
            //                this[0].Members["Items"].AddPathDataDisplayer(pathDataDisplayer);

            //                //  int loadedCount = this[0].Members["Items"].ValuesCollection.Count;
            //            }
            //        }

            //    }
            //    catch (System.Exception error)
            //    {
            //        throw;
            //    }


            //}
        }

        /// <MetaDataID>{21f36be9-d932-4ecb-9491-8c13bb19c7c8}</MetaDataID>
        void OnRefreshPathDataDisplayers(object sender, EventArgs e)
        {
            if (MemberChangeEvents != null)
            {
                while (MemberChangeEvents.Count != 0)
                {
                    MemberChangeEventArg memberChangeEventArg = MemberChangeEvents.Dequeue();
                    if (Change != null)
                    {
                        if (memberChangeEventArg.Transaction != null)
                        {
                            Transaction refreshTransaction = memberChangeEventArg.Transaction;
                            Transaction transaction = Transaction.Current;
                            while (transaction != null && transaction != memberChangeEventArg.Transaction)
                                transaction = transaction.OriginTransaction;
                            if (transaction == memberChangeEventArg.Transaction)
                                refreshTransaction = Transaction.Current;

                            using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(refreshTransaction))
                            {
                                try
                                {
                                    if (memberChangeEventArg.Type == ChangeType.LockChanged)
                                        LockStateChange(this);
                                    else
                                        Change(this, memberChangeEventArg);
                                    stateTransition.Consistent = true;
                                }
                                catch (Exception error)
                                {
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            if (memberChangeEventArg.Type == ChangeType.LockChanged)
                                LockStateChange(this);
                            else
                                Change(this, memberChangeEventArg);

                        }

                    }
                }
            }
        }





        /// <MetaDataID>{84a48654-2f57-4df7-b05d-fd361b202d8b}</MetaDataID>
        internal void TransferValues(Transaction commitedTransaction, Transaction newTransaction)
        {
            List<DisplayedValue> newValues = new List<DisplayedValue>();

            bool hasChange = false;
            MemberValue memberValue = null;
            if (commitedTransaction != null && Values.TryGetValue(commitedTransaction.LocalTransactionUri, out memberValue))
            {
                hasChange |= memberValue.HasChange;
                newValues.AddRange(memberValue.MemberValues);
            }

            foreach (var memberValueEntry in Values)
            {
                if (commitedTransaction != null && commitedTransaction.LocalTransactionUri == memberValueEntry.Key)
                    continue;
                memberValue = memberValueEntry.Value;
                hasChange |= memberValue.HasChange;
                newValues.AddRange(memberValue.MemberValues);
            }



            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                Values.Clear();
                Values[newTransaction.LocalTransactionUri] = new MemberValue(newValues, hasChange);
                stateTransition.Consistent = true;
            }




        }



        /// <MetaDataID>{2e7379d7-0246-426d-a465-6d22e43f2991}</MetaDataID>
        internal void LockMemberValueTypeObjects(Transaction transaction)
        {
            foreach (MemberValue memberValue in Values.Values)
            {
                if (memberValue == null)
                    continue; //TODO γιατί είναι null

                foreach (DisplayedValue displayedValue in memberValue.MemberValues)
                {
                    if (displayedValue.Value != null)

                        displayedValue.LockTransaction = transaction;
                    foreach (Member member in displayedValue.Members.Values)
                        if (member.MemberType.IsValueType && !member.MemberType.IsPrimitive)
                            member.LockMemberValueTypeObjects(transaction);

                    //TODO υπάρχει περίπτωση να γίνει commit/abord κατα μεχρι να γίνει subscribe το event;
                    using (Transactions.SystemStateTransition outStateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                    {
                        if (Owner.UserInterfaceSession.Transaction != null)
                        {
                            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Owner.UserInterfaceSession.Transaction))
                            {
                                foreach (Member member in displayedValue.Members.Values)
                                    member.Locked = true;
                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            foreach (Member member in displayedValue.Members.Values)
                                member.Locked = true;
                        }
                    }
                }
            }
        }

        public bool ObjectHasGhanged(TransactionRunTime transaction)
        {
            MemberValue memberValue = null;
            if (Values.TryGetValue(transaction.LocalTransactionUri, out memberValue))
            {
                return memberValue.HasChange;
            }
            return false;
        }



        /// <MetaDataID>{b4105cb2-9ecb-450f-a7d9-b687832a6583}</MetaDataID>
        internal bool HasChange
        {
            get
            {
                foreach (MemberValue memberValue in Values.Values)
                {
                    if (memberValue != null && memberValue.HasChange)
                        return true;
                }
                return false;
            }

        }
    }
}
