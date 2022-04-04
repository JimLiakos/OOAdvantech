using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;
using System.Linq;
namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{6b0f55b5-c673-4733-9fa7-62114ae2c5e4}</MetaDataID>
    public enum ViewControlObjectState
    {
        DesigneMode,
        Initialize,
        UserInteraction,
        SaveControlValues,
        Passive
    }

    public delegate void ComponentNameChangedEventHandler(object sender, ComponentNameChangedEventArgs e);
    /// <MetaDataID>{b07fa272-fdc6-4238-b4dd-83abf5420ad6}</MetaDataID>
    public sealed class ComponentNameChangedEventArgs : EventArgs
    {


        /// <MetaDataID>{372e2459-2797-4ee2-a1d3-72c5ff0cc5fc}</MetaDataID>
        public ComponentNameChangedEventArgs(object component, string oldName, string newName)
        {
            _Component = component;
            _NewName = newName;
            _OldName = oldName;

        }
        /// <exclude>Excluded</exclude>
        object _Component;
        /// <MetaDataID>{9c2ae1b3-272d-440d-a64f-d6a39c856f74}</MetaDataID>
        public object Component
        {
            get
            {
                return _Component;
            }
        }


        /// <exclude>Excluded</exclude>
        string _NewName;
        /// <MetaDataID>{ec84797d-d1f8-4674-931e-dd66722dcc54}</MetaDataID>
        public string NewName { get { return _NewName; } }
        /// <exclude>Excluded</exclude>
        string _OldName;
        /// <MetaDataID>{6743dfcc-9cbc-42f5-853b-008bfebdb271}</MetaDataID>
        public string OldName { get { return _OldName; } }
    }


    public delegate void FormClosedEventHandler(object sender);
    public delegate void BeforeTransactionCommitEventHandler(UserInterfaceObjectConnection sender);
    //TODO πρέπει να γίνει internal
    /// <MetaDataID>{43fd8409-d139-4df8-ad7e-4f75a407a6bd}</MetaDataID>
    public interface IPresentationContextViewControl
    {
        /// <MetaDataID>{7094f488-fe11-42b8-a437-22333b9e3246}</MetaDataID>
        bool InvokeRequired
        {
            get;
        }
        /// <MetaDataID>{984e24b7-b7a4-4952-b17c-635b0affe188}</MetaDataID>
        void OnBeforeViewControlObjectInitialization();
        /// <MetaDataID>{df5de2ad-342c-457e-bc29-b9729aad8972}</MetaDataID>
        void OnAfterViewControlObjectInitialization();

        /// <MetaDataID>{a74dc6b1-cf61-4d0a-b359-c90f4d26f2f3}</MetaDataID>
        object SynchroInvoke(Delegate method, params object[] args);

        /// <MetaDataID>{49be1729-f8b3-49aa-89e9-cab5ab93d523}</MetaDataID>
        string Name
        {
            get;
        }
        /// <MetaDataID>{8d54a553-ac79-4453-b3ad-91e4bd41babd}</MetaDataID>
        string HostControlName
        {
            get;
        }
        /// <MetaDataID>{fd865e46-3d96-472d-9be9-801e90c19996}</MetaDataID>
        object ContainerControl
        {
            get;
        }
        /// <MetaDataID>{dbed63b7-6cb9-4757-959d-7bdf3121b46d}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier ContainerControlType
        {
            get;
        }

        /// <MetaDataID>{92de1bf5-0049-4baa-840f-f3c0942adece}</MetaDataID>
        bool Removed
        {
            get;
        }



        /// <MetaDataID>{473225de-ba56-4e08-883a-2f250a633275}</MetaDataID>
        void DisableAllControls();
    }


    /// <MetaDataID>{aa197024-89c8-484a-b187-51290620eebf}</MetaDataID>
    public class UserInterfaceObjectConnection
    {

        System.Globalization.CultureInfo _Culture;
        public System.Globalization.CultureInfo Culture
        {
            get
            {
                return _Culture;
            }
            set
            {
                if (_Culture != value)
                {


                    if (Instance != null)
                    {
                        if (_Culture != null)
                        {
                            _Culture = value;
                            RefreshUIElements();
                        }
                    }
                    _Culture = value;

                }
            }
        }

        bool _UseDefaultCultureWhenValueMissing;

        public bool UseDefaultCultureWhenValueMissing
        {
            get
            {
                return _UseDefaultCultureWhenValueMissing;
            }
            set
            {
                if (_UseDefaultCultureWhenValueMissing != value)
                {
                    _UseDefaultCultureWhenValueMissing = value;

                    if (Instance != null)
                    {

                        RefreshUIElements();
                        try
                        {
                            //if (_Culture != null)
                            //{
                            //    using (OOAdvantech.CultureContext cultureContext = new CultureContext(_Culture, UseDefaultCultureWhenValueMissing))
                            //    {
                            //        ObjectChangeState(Instance, null);
                            //    }
                            //}
                            //else
                            //    ObjectChangeState(Instance, null);

                        }
                        catch (Exception error)
                        {
                        }
                    }
                }
            }
        }


        private void RefreshUIElements()
        {

            if (Culture != null)
            {
                if (Transaction != null && Transaction.Status == TransactionStatus.Continue)
                {

                    Transaction.RunAsynch(new Action(() =>
                    {

                        using (SystemStateTransition supressTransactionStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(Transaction))
                            {
                                using (CultureContext cultureContext = new CultureContext(this.Culture, this.UseDefaultCultureWhenValueMissing))
                                {

                                    foreach (var uiProxy in this.GetAllUIProxies())
                                    {
                                        if (uiProxy.DisplayedValue != null)
                                            this.ObjectChangeState(uiProxy.DisplayedValue.Value, null);
                                    }
                                }
                                stateTransition.Consistent = true;
                            } 
                            
                        }

                    }));

                }
                else
                {
                    Transaction.RunAsynch(new Action(() =>
                    {

                        using (CultureContext cultureContext = new CultureContext(this.Culture, this.UseDefaultCultureWhenValueMissing))
                        {

                            foreach (var uiProxy in this.GetAllUIProxies())
                            {
                                if (uiProxy.DisplayedValue != null)
                                    this.ObjectChangeState(uiProxy.DisplayedValue.Value, null);
                            }
                        }
                    }));

                }
            }
        }
        /// <MetaDataID>{d7c7e25d-c606-4f7e-9d3e-87272fb1beb4}</MetaDataID>
        bool _IniateTransactionOnInstanceSet;
        /// <MetaDataID>{10b15b6d-7ac9-4a6f-add3-c23bfad3dacf}</MetaDataID>
        public virtual bool IniateTransactionOnInstanceSet
        {
            get
            {
                return _IniateTransactionOnInstanceSet;
            }
            set
            {
                _IniateTransactionOnInstanceSet = value;
            }
        }


        /// <MetaDataID>{d21453d2-63c7-43b0-8a34-b4e8507f5435}</MetaDataID>
        protected void InitiateTransaction()
        {
            if (TransactionOption == OOAdvantech.Transactions.TransactionOption.Required ||
                TransactionOption == OOAdvantech.Transactions.TransactionOption.Supported)
            {
                if (_Transaction == null
                    && OOAdvantech.Transactions.Transaction.Current != null
                    && OOAdvantech.Transactions.Transaction.Current.Status == TransactionStatus.Continue)
                {
                    _Transaction = OOAdvantech.Transactions.Transaction.Current;
                }

                if (_Transaction == null && TransactionOption == OOAdvantech.Transactions.TransactionOption.Required)
                {
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                    TransactionOwner = true;
                }
            }
            else if (TransactionOption == OOAdvantech.Transactions.TransactionOption.RequiredNested &&
                 _Transaction == null)
            {
                
                if (OOAdvantech.Transactions.Transaction.Current != null)
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction(OOAdvantech.Transactions.Transaction.Current);
                else
                    _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                TransactionOwner = true;
            }
            if (TransactionOption == OOAdvantech.Transactions.TransactionOption.RequiresNew)
            {
                _Transaction = new OOAdvantech.Transactions.CommittableTransaction();
                TransactionOwner = true;
            }
            TransactionInitialized = true;

        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Transactions.TransactionOption _TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
        /// <MetaDataID>{95413dda-1cd0-481b-be92-787181d6a027}</MetaDataID>
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return _TransactionOption;
            }
            set
            {
                _TransactionOption = value;
            }
        }
        /// <exclude>Excluded</exclude>
        TimeSpan _TransactionObjectLockTimeOut = TimeSpan.Zero;
        /// <MetaDataID>{984b17cc-e752-4e20-a3dd-466bbeb69185}</MetaDataID>
        public virtual TimeSpan TransactionObjectLockTimeOut
        {
            get
            {
                if (State == ViewControlObjectState.DesigneMode)
                    return _TransactionObjectLockTimeOut;
                else
                {
                    if (TransactionOption == TransactionOption.Supported && MasterViewControlObject != null)
                        return MasterViewControlObject.TransactionObjectLockTimeOut;


                    if (_TransactionObjectLockTimeOut == TimeSpan.Zero)
                        return OOAdvantech.Transactions.TransactionManager.ObjectEnlistmentTimeOut;
                    else
                        return _TransactionObjectLockTimeOut;
                }
            }
            set
            {
                _TransactionObjectLockTimeOut = value;
            }
        }




        /// <MetaDataID>{11360025-fc9e-4faf-9a42-453f7ffcaf2a}</MetaDataID>
        public bool TransactionInitialized = false;

        public bool TransactionOwner = false;
        /// <exclude>Excluded</exclude>
        protected internal OOAdvantech.Transactions.Transaction _Transaction;
        /// <MetaDataID>{db64198a-7919-42e5-84c5-e955fd2e2c47}</MetaDataID>
        public virtual OOAdvantech.Transactions.Transaction Transaction
        {
            get
            {
                if (State == ViewControlObjectState.DesigneMode)
                    return null;

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




        /// <MetaDataID>{8ce5f436-b2a9-4028-8c0b-862747a2da6e}</MetaDataID>
        internal System.Collections.Generic.List<UISession> UsedUISessions = new List<UISession>();
        /// <MetaDataID>{53126680-fc2a-4b5a-80c2-0bb69ea6747b}</MetaDataID>
        internal UISession _UserInterfaceSession;
        /// <MetaDataID>{6ae1e106-1d71-4e6c-a829-8729c9a0ec70}</MetaDataID>
        internal UISession UserInterfaceSession
        {
            get
            {
                if (Transactions.Transaction.Current != null)
                {
                    UISession uiSession = UISession.CurrentUserInterfaceSession;
                    if (!UsedUISessions.Contains(uiSession))
                        UsedUISessions.Add(uiSession);
                    return uiSession;
                }
                else
                {
                    if (Transaction != null)
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction, TransactionObjectLockTimeOut))
                        {
                            try
                            {
                                UISession uiSession = UISession.CurrentUserInterfaceSession;
                                if (!UsedUISessions.Contains(uiSession))
                                    UsedUISessions.Add(uiSession);
                                return uiSession;
                            }
                            finally
                            {
                                stateTransition.Consistent = true;
                            }
                        }
                    }


                    else
                    {
                        if (MasterViewControlObject != null)
                        {
                            UISession uiSession = MasterViewControlObject.UserInterfaceSession;
                            if (uiSession != null && !UsedUISessions.Contains(uiSession))
                                UsedUISessions.Add(uiSession);
                            return uiSession;
                        }
                        else
                        {
                            if (!UsedUISessions.Contains(_UserInterfaceSession))
                                UsedUISessions.Add(_UserInterfaceSession);

                            return _UserInterfaceSession;
                        }
                    }
                }
            }
        }
        /// <MetaDataID>{2471c96d-ce68-4c35-acfa-ebde3916fcd0}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <MetaDataID>{7ddef904-4d2c-4a92-86ca-7116b1223aeb}</MetaDataID>
        public override string ToString()
        {
            return Name;
        }


        /// <MetaDataID>{1abe706e-fc8d-4cd7-9248-65f1a14cb36f}</MetaDataID>
        public void ObjectChangeState(object _object, string member)
        {
            UISession transactionUISession = UserInterfaceSession;
            foreach (UISession uiSession in UsedUISessions)
            {
                DisplayedValue displayedValue = uiSession.GetDisplayedValue(_object);
                if (displayedValue != null)
                    displayedValue.OnObjectChangeState(_object, member);
            }
        }





        /// <MetaDataID>{e72ed7c2-5fe4-4bc1-87cc-68f4ae3b6070}</MetaDataID>
        public virtual void RefreshUserInterface()
        {

            if (State == ViewControlObjectState.Passive)
                return;

            try
            {
                UserInterfaceSession.StartControlValuesUpdate();
                if (State == ViewControlObjectState.Passive)
                    return;
                if (Transaction != null)
                {
                    using (SystemStateTransition clearStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(Transaction))
                        {
                            using (OOAdvantech.CultureContext cultureContext = new CultureContext(Culture, UseDefaultCultureWhenValueMissing))
                            {
                                if (RefreshPathDataDisplayers != null)
                                    RefreshPathDataDisplayers(this, EventArgs.Empty);
                                if (UserInterfaceSession.StartingUserInterfaceObjectConnection == this)
                                    UserInterfaceSession.Synchronize();
                            }
                            stateTransition.Consistent = true;
                        }
                        clearStateTransition.Consistent = true;
                    }
                }
                else
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        using (OOAdvantech.CultureContext cultureContext = new CultureContext(Culture, UseDefaultCultureWhenValueMissing))
                        {
                            if (RefreshPathDataDisplayers != null)
                                RefreshPathDataDisplayers(this, EventArgs.Empty);
                            if (UserInterfaceSession.StartingUserInterfaceObjectConnection == this)
                                UserInterfaceSession.Synchronize();
                        }

                        stateTransition.Consistent = true;
                    }
                }


                foreach (object component in _ControlledComponents)
                {
                    if (component is UserInterfaceObjectConnection)
                        (component as UserInterfaceObjectConnection).RefreshUserInterface();
                }
            }
            finally
            {
                UserInterfaceSession.EndControlValuesUpdate();
            }
        }
        public event EventHandler RefreshPathDataDisplayers;


        public event EventHandler InstanceChanged;


        /// <MetaDataID>{9c075f4d-e585-4dbf-a303-9714b0886cbe}</MetaDataID>
        public object ControlObject(object _object)
        {
            //if (_object is MarshalByRefObject && !_object.GetType().IsCOMObject)
            //    //    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_object as MarshalByRefObject))
            //    if (!(System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) is UIProxy))
            //        return new UIProxy(_object as MarshalByRefObject, _object.GetType(), this).GetTransparentProxy();
            return _object;


        }
        /// <MetaDataID>{902ccb09-f9e0-4d27-a604-4ddf65e0e8d6}</MetaDataID>
        public readonly IPresentationContextViewControl PresentationContextViewControl;

        /// <MetaDataID>{4754483a-4239-4aae-a74c-ed51da49e33b}</MetaDataID>
        public void PresentationContextViewControlChange()
        {
            if (PresentationContextViewControl.Removed)
            {
                MasterViewControlObject = null;
                foreach (object controlledComponent in ControlledComponents)
                {
                    if (controlledComponent is IConnectableControl)
                        (controlledComponent as IConnectableControl).UserInterfaceObjectConnection = null;
                    if (controlledComponent is UserInterfaceObjectConnection)
                        (controlledComponent as UserInterfaceObjectConnection).MasterViewControlObject = null;
                }
            }
        }

        /// <MetaDataID>{7f0f842f-9093-45b4-8f29-8811bdb92d48}</MetaDataID>
        public UserInterfaceObjectConnection(IPresentationContextViewControl presentationContextViewControl)
        {
            PresentationContextViewControl = presentationContextViewControl;

        }


        #region Designe Time Code
        /// <MetaDataID>{c7cdabe1-80bc-4be9-a254-c5a352b9e662}</MetaDataID>
        public string Name
        {
            get
            {
                return PresentationContextViewControl.Name;
            }
        }

        /// <MetaDataID>{796ed3a6-f4b4-47c9-b393-06b0d4335af2}</MetaDataID>
        public string HostControlName
        {
            get
            {
                return PresentationContextViewControl.HostControlName;
            }
        }



        /// <summary>
        /// This operation search for the control with name the controlName value.
        /// If there isn't control return null.
        /// </summary>
        /// <param name="controlName">
        /// Define the name of control which system want
        /// </param>
        /// <returns>
        /// The control which has the same name with the parameter
        /// </returns>
        /// <MetaDataID>{80BDCF6A-87BC-4490-A80B-A02BCD60E66D}</MetaDataID>
        public IObjectMemberViewControl GetControlWithName(string controlName)
        {
            //if (MasterViewControlObject != null)
            //    return MasterViewControlObject.InternalGetControlWithName(controlName);
            //else
            return InternalGetControlWithName(controlName);

        }
        /// <MetaDataID>{b2c58202-eda7-4e71-83f2-921c7b7a2cdd}</MetaDataID>
        IObjectMemberViewControl InternalGetControlWithName(string controlName)
        {
            foreach (object component in _ControlledComponents)
            {
                if (component is IObjectMemberViewControl && (component as IObjectMemberViewControl).Name == controlName)
                    return component as IObjectMemberViewControl;
                if (component is UserInterfaceObjectConnection && (component as UserInterfaceObjectConnection).ContainerControl == ContainerControl)
                {
                    IObjectMemberViewControl control = (component as UserInterfaceObjectConnection).InternalGetControlWithName(controlName);
                    if (control != null)
                        return control;
                }
            }
            return null;
        }


        /// <MetaDataID>{fb665853-da82-421e-8e1f-b1d907e71034}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            foreach (object component in _ControlledComponents)
            {
                IObjectMemberViewControl memberViewControl = component as IObjectMemberViewControl;
                if (memberViewControl != null)
                {
                    try
                    {
                        hasErrors |= memberViewControl.ErrorCheck(ref errors);
                    }
                    catch (Exception error)
                    {
                    }
                }
            }

            if (_ObjectType == null)
                if (!string.IsNullOrEmpty(ViewObjectTypeFullName))
                    _ObjectType = UISession.GetClassifier(ViewObjectTypeFullName, AssemblyMetadata, true, this);
            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {

                _PresentationObjectType = UISession.GetClassifier(PresentationObjectTypeFullName, "", true, this) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ViewControlObject '" + Name + "' has invalid PresentationObject.", HostControlName));
                else
                {
                    if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(_PresentationObjectType as OOAdvantech.MetaDataRepository.Class, _ObjectType))
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ViewControlObject '" + Name + "' You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", HostControlName));
                }
            }
            return hasErrors;
        }
        /// <MetaDataID>{7a1839ac-e03f-4461-a91d-13c4a134f647}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string PropertyDescriptor)
        {
            try
            {
                //TODO check PresentationObjectType


                if (metaObject is OOAdvantech.MetaDataRepository.Class && PropertyDescriptor == "AssignPresentationObjectType")
                {
                    if (_ObjectType == null)
                        if (!string.IsNullOrEmpty(ViewObjectTypeFullName))
                            _ObjectType = UISession.GetClassifier(ViewObjectTypeFullName, AssemblyMetadata, true, this);


                    if (OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(metaObject as OOAdvantech.MetaDataRepository.Class, _ObjectType))
                        return true;
                    else
                        throw new System.Exception("You can't assign the '" + metaObject.FullName + "' to the PresentationObjectType.\n Check the property rules.");
                }
                if (PropertyDescriptor == "AssignPresentationObjectType")
                    throw new System.Exception("You can't assign the '" + metaObject.FullName + "' to the PresentationObjectType.\n Check the property rules.");
                if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                    return true;
                return false;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }






        public event ComponentNameChangedEventHandler ComponentNameChanged;
        /// <MetaDataID>{58359f1a-199f-4c11-8ec8-893b78f65746}</MetaDataID>
        public virtual void HostFormComponentNameChanged(object sender, ComponentNameChangedEventArgs e)
        {
            if (ComponentNameChanged != null)
                ComponentNameChanged(sender, e);
        }

        #endregion

        public event BeforeTransactionCommitEventHandler BeforeTransactionCommit;
        public event FormClosedEventHandler FormClosed;
        /// <MetaDataID>{6493c0d4-025d-41e1-8ab8-59c87005a1fd}</MetaDataID>
        public virtual void HostFormClosed(DialogResult dialogResult)
        {
            if (FormClosed != null)
                FormClosed(dialogResult);
            foreach (IPresentationObject presentationObject in PresentationObjects.Values)
            {
                try
                {
                    presentationObject.FormClosed();
                }
                catch (Exception error)
                {
                }
            }
            if (Instance is IPresentationObject)
            {
                try
                {
                    (Instance as IPresentationObject).FormClosed();
                }
                catch (Exception error)
                {
                }
            }
             

        }
        /// <MetaDataID>{95a1acc2-ee5f-4227-a41c-0e793c000f37}</MetaDataID>
        protected void RaiseBeforeTransactionCommit()
        {
            if (BeforeTransactionCommit != null)
                BeforeTransactionCommit(this);
        }


        /// <MetaDataID>{16086b22-dd15-4efa-8db3-da58d9a0131e}</MetaDataID>
        public bool InvokeRequired
        {
            get
            {
                return PresentationContextViewControl.InvokeRequired;
            }
        }

        /// <MetaDataID>{80cc6c39-72da-427a-a13e-6dd8ac76cd93}</MetaDataID>
        public object SynchroInvoke(Delegate method, params object[] args)
        {

            return PresentationContextViewControl.SynchroInvoke(method, args);
        }







        ///// <summary>
        ///// Retrieves the value  from path with start object the obj parameter
        ///// System retrieve value from business object not from caching data in user interfce.
        ///// This method participates in user interface update scenario. 
        ///// This scenario triggered from object change state event.
        ///// </summary>
        ///// <param name="obj">object as root of path</param>
        ///// <param name="type">Type where system search for member. 
        ///// Member is the first part of path</param>
        ///// <param name="path">The path where system follow to get the value.</param>
        ///// <returns>The value of path</returns>
        ///// <MetaDataID>{41590259-dcdb-4d69-bcbf-008c8c9f5f09}</MetaDataID>
        //internal object InternalGetValue(object obj, Type type, string path)
        //{
        //    if (obj == null)
        //        return null;

        //    //  if (!IsActive)
        //    //  throw new System.Exception("You can't use GetValue method. Check the IsActive preperty before call this method");


        //    if (path != null && path.Length > 0)
        //    {
        //        int nPos = path.IndexOf(".");
        //        if (nPos == -1)
        //        {
        //            System.Reflection.MemberInfo memberInfo = GetMember(type, path);
        //            if (memberInfo is System.Reflection.PropertyInfo)
        //            {
        //                System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
        //                return propertyInfo.GetValue(obj, null);
        //            }

        //            if (memberInfo is System.Reflection.FieldInfo)
        //            {
        //                System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
        //                return fieldInfo.GetValue(obj);
        //            }
        //        }
        //        else
        //        {
        //            string member = path.Substring(0, nPos);
        //            path = path.Substring(nPos + 1);
        //            System.Reflection.MemberInfo memberInfo = GetMember(type, member);
        //            if (memberInfo is System.Reflection.PropertyInfo)
        //            {
        //                System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
        //                object subNodeobj = propertyInfo.GetValue(obj, null);
        //                return InternalGetValue(subNodeobj, propertyInfo.PropertyType, path);
        //            }

        //            if (memberInfo is System.Reflection.FieldInfo)
        //            {
        //                System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

        //                object subNodeobj = fieldInfo.GetValue(obj);
        //                return InternalGetValue(subNodeobj, fieldInfo.FieldType, path);
        //            }

        //        }

        //    }
        //    return null;

        //}




        /// <MetaDataID>{5e236d92-c048-4657-820b-7de82d18c3ae}</MetaDataID>
        bool IsReadOnly(string path)
        {
            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return true;
            }
            System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
            return UISession.IsReadOnly(type, path);
        }

        /// <MetaDataID>{aeb45e08-ffc2-4717-9dee-6e50946ef356}</MetaDataID>
        bool IsReadOnly(Type type, string path)
        {
            return UISession.IsReadOnly(type, path);
        }




        /// <MetaDataID>{5ef1e6f4-c0c1-4098-a2dc-e28e336f5f93}</MetaDataID>
        bool CanAccessValue(string path, IPathDataDisplayer pathDataDisplayer)
        {
            if (PresentationObjectType == null)
                return false;
            return CanAccessValue(PresentationObject, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as System.Type, path, pathDataDisplayer);
        }






        /// <MetaDataID>{d4cfc605-c81d-4a6f-93c9-3a6d2b153e83}</MetaDataID>
        public bool CanEditValue(string path, IPathDataDisplayer pathDataDisplayer)
        {
            if (PresentationObjectType == null)
                return false;
            return CanEditValue(PresentationObject, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as System.Type, path, pathDataDisplayer);
        }



        /// <MetaDataID>{5ef1e6f4-c0c1-4098-a2dc-e28e336f5f93}</MetaDataID>
        public bool Isloaded(string path)
        {
            if (PresentationObjectType == null)
                return false;
            return Isloaded(PresentationObject, PresentationObjectType, path);
        }
        /// <MetaDataID>{a863cc0d-00c9-4dad-be78-44fcf6d87f25}</MetaDataID>
        bool InternalIsloaded(object obj, Type type, string path)
        {
            try
            {

                if (type == null && path == null)
                    return false;

                if (!string.IsNullOrEmpty(path))
                {
                    if (path == "(ViewControlObject)")
                        return true;
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        DisplayedValue displayedValue = null;
                        if (obj != null && UserInterfaceSession.TryGetDisplayedValue(obj, out displayedValue) && displayedValue.Members.ContainsKey(path))
                            return true;
                        else
                            return false;

                    }
                    else
                    {
                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = UISession.GetMember(type, member);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;

                            DisplayedValue displayedValue = null;
                            //TODO DisplayedValue.DisplayedValues.ContainsKey(obj) στο else τα γράφω κανονικά
                            if (UserInterfaceSession.TryGetDisplayedValue(obj, out displayedValue) && displayedValue.Members.ContainsKey(member))
                                displayedValue = UserInterfaceSession[obj].Members[member][0];
                            else
                                return false;

                            if (displayedValue != null && displayedValue.Value != null)
                            {

                                UserInterfaceSession[displayedValue.Value] = displayedValue;
                                return InternalIsloaded(displayedValue.Value, propertyInfo.PropertyType, path);
                            }
                            else
                                return false;
                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {
                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                            DisplayedValue displayedValue = null;
                            if (UserInterfaceSession.TryGetDisplayedValue(obj, out displayedValue) && displayedValue.Members.ContainsKey(path))
                                displayedValue = displayedValue.Members[path][0];
                            else
                                return false;

                            if (displayedValue != null && displayedValue.Value != null)
                            {
                                UserInterfaceSession[displayedValue.Value] = displayedValue;
                                return InternalIsloaded(displayedValue.Value, fieldInfo.FieldType, path);
                            }
                            else
                                return false;
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return false;

        }


        /// <MetaDataID>{f217080e-7100-454b-bb8f-36c5630f5a53}</MetaDataID>
        public string[] GetExtraPathsFor(string path)
        {
            if (PresentationObjectType != null)
                return UISession.GetExtraPathsFor(PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type, path);
            else
                return new string[0];
        }


        /// <MetaDataID>{94a5973b-b552-4f7a-b50d-18fede00761a}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            return UISession.GetClassifier(classifier, source);
            //if (source != null && source.Length > 0)
            //{
            //    int nPos = source.IndexOf(".");
            //    if (nPos == -1)
            //    {
            //        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
            //        {
            //            if (attribute.Name == source)
            //                return attribute.Type;
            //        }
            //        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            //        {
            //            if (associationEnd.Name == source)
            //                if (associationEnd.CollectionClassifier != null)
            //                    return associationEnd.CollectionClassifier;
            //                else
            //                    return associationEnd.Specification;
            //        }
            //    }
            //    else
            //    {
            //        string member = source.Substring(0, nPos);
            //        source = source.Substring(nPos + 1);
            //        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
            //        {
            //            if (attribute.Name == member)
            //                return GetClassifier(attribute.Type, source); ;
            //        }
            //        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            //        {
            //            if (associationEnd.Name == member)
            //                if (associationEnd.CollectionClassifier != null)
            //                    return GetClassifier(associationEnd.CollectionClassifier, source);

            //                else
            //                    return GetClassifier(associationEnd.Specification, source);
            //        }
            //    }
            //}
            //return null;
        }
        /// <MetaDataID>{abebe484-0908-4086-aa64-519d4a8be3fb}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.MetaObject GetMetaObject(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            return UISession.GetMetaObject(classifier, source);
            //if (source != null && source.Length > 0)
            //{
            //    int nPos = source.IndexOf(".");
            //    if (nPos == -1)
            //    {
            //        MetaDataRepository.MetaObject metaObject = classifier.GetMember(source);
            //        if (metaObject is OOAdvantech.MetaDataRepository.Attribute || metaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
            //            return metaObject;
            //        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
            //        {
            //            if (attribute.Name == source)
            //                return attribute;
            //        }
            //        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            //        {
            //            if (associationEnd.Name == source)
            //                return associationEnd;
            //        }
            //    }
            //    else
            //    {
            //        string member = source.Substring(0, nPos);
            //        source = source.Substring(nPos + 1);
            //        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
            //        {
            //            if (attribute.Name == member)
            //                return GetMetaObject(attribute.Type, source); ;
            //        }
            //        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            //        {
            //            if (associationEnd.Name == member)
            //                if (associationEnd.CollectionClassifier != null)
            //                    return GetMetaObject(associationEnd.CollectionClassifier, source);
            //                else
            //                    return GetMetaObject(associationEnd.Specification, source);
            //        }
            //    }
            //}
            //return null;
        }

        /// <MetaDataID>{39e7043d-8bc1-49f8-a1c3-824cf2c52166}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifier(string path)
        {
            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control component.");
                return null;
            }
            if (string.IsNullOrEmpty(path))
                return null;
            if (path == "(ViewControlObject)")
                return PresentationObjectType;
            return GetClassifier(PresentationObjectType, path);

        }
        /// <MetaDataID>{74cb548b-482d-4323-9571-ae1a109fe04a}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifier(string typeFullName, bool caseSensitive)
        {
            return UISession.GetClassifier(typeFullName, "", true, this);
        }
        /// <MetaDataID>{f7c0c2b6-ce78-4f6e-992b-4862cb60b0b6}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(Type type)
        {
            return UISession.GetClassifier(type);
        }



        /// <MetaDataID>{b3f41319-9583-4e4d-8d0d-fccfc9a98ea9}</MetaDataID>
        public static Type GetType(System.Type type, string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return UISession.GetType(type, path);
        }

        /// <MetaDataID>{36a2863e-1568-4305-8b91-7ac72a2bb4f5}</MetaDataID>
        public Type GetType(string path)
        {

            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control component.");
                return null;
            }
            if (string.IsNullOrEmpty(path))
                return null;

            //if (Instance== null)
            //{
            //    System.Diagnostics.Debug.WriteLine("There isn't instance on view control component.");
            //    return null;
            //}


            System.Type type = PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type;
            if (path == "(ViewControlObject)")
                return type;
            if (path.IndexOf("(ViewControlObject)") == 0)
                path = path.Substring("(ViewControlObject).".Length);


            return UISession.GetType(type, path);



        }




        /// <MetaDataID>{939f9eb3-acec-49e8-b474-05477616e872}</MetaDataID>
        //[ThreadStatic]
        //internal static UserInterfaceObjectConnection CurrentViewControlObject = null;




        protected System.Collections.Generic.Dictionary<object, IPresentationObject> PresentationObjects = new Dictionary<object, IPresentationObject>();
        /// <MetaDataID>{606ca838-5acc-453e-8cb0-0984716d5051}</MetaDataID>
        public IPresentationObject GetPresentationObject(object obj, OOAdvantech.MetaDataRepository.Class presentationObjectType, Type type)
        {
            if (presentationObjectType == null)
                return null;
            if (obj == null)
                return null;

            IPresentationObject presentationObject = null;
            if (!PresentationObjects.TryGetValue(obj, out presentationObject))
            {
                System.Reflection.ConstructorInfo contructor = null;

                System.Type dotNetpresentationObjectType = (presentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                contructor = dotNetpresentationObjectType.GetConstructor(new System.Type[] { type });
                if (contructor == null)
                {
                    foreach (var ctr in dotNetpresentationObjectType.GetConstructors())
                    {
                        if (ctr.GetParameters().Length == 1)
                        {
                            if (ctr.GetParameters()[0].ParameterType.IsSubclassOf(type))
                            {
                                contructor = ctr;
                                break;
                            }

                        }
                    }
                }
                presentationObject = contructor.Invoke(new object[1] { obj }) as IPresentationObject;

                while (dotNetpresentationObjectType.Assembly != typeof(UserInterfaceObjectConnection).Assembly && dotNetpresentationObjectType.BaseType != null)
                    dotNetpresentationObjectType = dotNetpresentationObjectType.BaseType;
                dotNetpresentationObjectType.GetProperty("UserInterfaceObjectConnection").SetValue(presentationObject, this, null);



                //presentationObject.UserInterfaceObjectConnection = this;
                DisplayedValue displayedValue = new DisplayedValue(presentationObject, UserInterfaceSession);

                //GetDisplayedValue(presentationObject, presentationObjectType, "RealObject", null);
                PresentationObjects.Add(obj, presentationObject);
            }

            return presentationObject;
        }


        /// <MetaDataID>{04fcb7b3-1ae7-4163-90ed-ba72139f1177}</MetaDataID>
        public object GetDisplayedValue(string path, IPathDataDisplayer pathDataDisplayer, out bool returnValueAsCollection)
        {
            OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = GetDisplayedValue(path as string, GetObjectChangeStateManager(pathDataDisplayer));
            if (!displayedValue.Members.ContainsKey("Items"))
            {
                returnValueAsCollection = false;
                return displayedValue.Value;
            }
            else
            {
                returnValueAsCollection = true;
                return GetCollection(path as string, null);
            }
        }



        /// <MetaDataID>{352a6c4d-3c97-41d3-acea-99838998231a}</MetaDataID>
        public void Control(object obj)
        {
            DisplayedValue displayedValue = GetDisplayedValue(obj);
        }


        /// <MetaDataID>{097c5eab-6357-497a-b08c-266b350c67da}</MetaDataID>
        public object GetDisplayedValue(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, IPathDataDisplayer pathDataDisplayer, out bool returnValueAsCollection)
        {
            try
            {
                Type type = classifier.GetExtensionMetaObject(typeof(Type)) as Type;
                DisplayedValue displayedValue = GetDisplayedValue(obj, type, path, GetObjectChangeStateManager(pathDataDisplayer));
                if (!displayedValue.Members.ContainsKey("Items"))
                {
                    returnValueAsCollection = false;
                    return displayedValue.Value;
                }
                else
                {
                    returnValueAsCollection = true;

                    return GetCollection(obj, classifier, path, null);
                }
            }
            catch (System.Exception error)
            {
                throw;
            }


        }


        /// <MetaDataID>{a8f8efad-a7c9-4b8e-a971-fad65ee09730}</MetaDataID>
        System.Collections.Generic.Dictionary<IPathDataDisplayer, ObjectChangeStateManager> ObjectChangeStateManagers = new Dictionary<IPathDataDisplayer, ObjectChangeStateManager>();
        /// <MetaDataID>{1d737e49-cf8f-4325-8b84-4327315450dc}</MetaDataID>
        internal ObjectChangeStateManager GetObjectChangeStateManager(IPathDataDisplayer pathDataDisplayer)
        {
            if (pathDataDisplayer == null)
                return null;
            else
            {
                ObjectChangeStateManager objectChangeStateManager = null;
                if (ObjectChangeStateManagers.TryGetValue(pathDataDisplayer, out objectChangeStateManager))
                    return objectChangeStateManager;
                else
                {
                    objectChangeStateManager = new ObjectChangeStateManager(pathDataDisplayer);
                    ObjectChangeStateManagers.Add(pathDataDisplayer, objectChangeStateManager);
                    return objectChangeStateManager;

                }


            }
        }



        /// <summary>
        /// This method returns the displayed value, 
        /// for the value of path. The root of path is the instance of view control object.
        /// </summary>
        /// <param name="path">The path which follow the system to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <param name="presentationObjectType">Defines a wraper class for the value of displayed value.</param>
        /// <returns>Displayd value</returns>
        /// <MetaDataID>{cd864bdc-5a73-40a5-9904-6951b2e8bd78}</MetaDataID>
        internal DisplayedValue GetDisplayedValue(string path, ObjectChangeStateManager pathDataDisplayer)
        {

            if (ObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return null;
            }

            System.Type type = PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type;
            if (path == "(ViewControlObject)")
            {
                if (PresentationObject == null)
                    return new DisplayedValue(PresentationObject, UserInterfaceSession);
                DisplayedValue displayedValue = null;
                if (!UserInterfaceSession.TryGetDisplayedValue(PresentationObject, out displayedValue))
                {
                    displayedValue = new DisplayedValue(PresentationObject, UserInterfaceSession);
                    UserInterfaceSession[PresentationObject] = displayedValue;
                }
                return displayedValue;
            }
            if (path.IndexOf("(ViewControlObject)") == 0)
                path = path.Substring("(ViewControlObject).".Length);


            return GetDisplayedValue(PresentationObject, type, path, pathDataDisplayer);
        }




        //$$**$$**

        /// <summary>
        /// Retrieves the displayd value from path with start object the obj parameter
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="classifier">Type where system search for member. Member is the first part of path</param>
        /// <param name="path">The path which follow the system to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <returns>Displayd value</returns>
        /// <MetaDataID>{1c6fc745-93a8-464f-a889-49b16cb32e37}</MetaDataID>
        internal DisplayedValue GetDisplayedValue(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, ObjectChangeStateManager pathDataDisplayer)
        {
            Type type = classifier.GetExtensionMetaObject(typeof(Type)) as Type;
            return GetDisplayedValue(obj, type, path, pathDataDisplayer);
        }


        //$$**$$**




        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.List<object> _ControlledComponents = new OOAdvantech.Collections.Generic.List<object>();
        /// <MetaDataID>{b7f449c7-65f5-4b84-812c-6e6eb74fa4c3}</MetaDataID>
        ///<summary>
        ///The components under control of connection.
        ///</summary>
        public OOAdvantech.Collections.Generic.List<object> ControlledComponents
        {
            set
            {

            }
            get
            {
                return new OOAdvantech.Collections.Generic.List<object>(_ControlledComponents);
            }

        }

        /// <MetaDataID>{574ab064-08f6-4d65-bfdb-5eee48173915}</MetaDataID>
        public void AddControlledComponent(object component)
        {
            if (!_ControlledComponents.Contains(component))
                _ControlledComponents.Add(component);
        }
        /// <MetaDataID>{662df6ec-bdb7-4223-96eb-3f7b9deb5472}</MetaDataID>
        public void RemoveControlledComponent(object component)
        {
            if (_ControlledComponents.Contains(component))
                _ControlledComponents.Remove(component);
        }
        /// <MetaDataID>{26920b7a-b6f5-4747-90cc-f8f59df33c95}</MetaDataID>
        public object ContainerControl
        {
            get
            {
                return PresentationContextViewControl.ContainerControl;
            }
        }
        /// <MetaDataID>{0d81d3b4-c88f-4805-81c5-748e84e9a1d2}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier ContainerControlType
        {
            get
            {
                return PresentationContextViewControl.ContainerControlType;
            }
        }
        /// <summary>
        /// This operation collect the view control object of form or control 
        /// </summary>
        /// <returns>
        /// A collection with view control objects
        /// </returns>
        /// <MetaDataID>{C0A0A1A6-48F9-44D7-9A69-CFA3FC4F5223}</MetaDataID>
        public System.Collections.Generic.List<UserInterfaceObjectConnection> GetHostContolViewControlObjects()
        {

            if (MasterViewControlObject != null)
                return MasterViewControlObject.GetHostContolViewControlObjects();
            else
            {
                System.Collections.Generic.List<UserInterfaceObjectConnection> viewControlObjects = new List<UserInterfaceObjectConnection>();
                viewControlObjects.Add(this);

                foreach (object component in _ControlledComponents)
                {
                    if (component is UserInterfaceObjectConnection && (component as UserInterfaceObjectConnection).ContainerControl == ContainerControl)
                        viewControlObjects.Add(component as UserInterfaceObjectConnection);
                }
                return viewControlObjects;
            }

        }



        /// <exclude>Excluded</exclude>
        protected UserInterfaceObjectConnection _MasterViewControlObject;
        /// <MetaDataID>{158155b8-683c-48fa-bbff-cb059f8f9aac}</MetaDataID>
        [OOAdvantech.MetaDataRepository.Association("ParentUserInterfaceObjectConnection", typeof(OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection), OOAdvantech.MetaDataRepository.Roles.RoleA, "{6B1F92A4-E844-475c-8778-2592E6C9B597}")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(0, 1)]
        virtual public UserInterfaceObjectConnection MasterViewControlObject
        {
            set
            {
                if (_MasterViewControlObject != null)
                {
                    _MasterViewControlObject.RemoveControlledComponent(this);
                    ReleaseDataPathNodes();
                }

                if (value != null && value.MasterViewControlObject == null && !(value is FormObjectConnection))
                {
                    //if (value.State != ViewControlObjectState.DesigneMode)
                    //    return;// throw new System.Exception("System can't find the FormConnectionControl");
                    //else
                    _MasterViewControlObject = value;
                }
                if (value != null && value.MasterViewControlObject == null && (value is FormObjectConnection))
                {
                    _MasterViewControlObject = value;
                    foreach (object obj in ControlledComponents)
                    {
                        if (obj is UserInterfaceObjectConnection)
                        {
                            (obj as UserInterfaceObjectConnection).MasterViewControlObject = this;
                        }

                    }
                }
                if (value != null && value.MasterViewControlObject != null)
                    _MasterViewControlObject = value.MasterViewControlObject;
                if (value == null)
                    _MasterViewControlObject = value;

                if (_MasterViewControlObject != null)
                    _MasterViewControlObject.AddControlledComponent(this);
            }
            get
            {
                return _MasterViewControlObject;
            }
        }

        /// <exclude>Excluded</exclude>
        ViewControlObjectState _State = ViewControlObjectState.Initialize;

        /// <MetaDataID>{3d655d72-2053-4056-8d4b-e4cd80416b93}</MetaDataID>
        public ViewControlObjectState State
        {
            get
            {
                if (_MasterViewControlObject != null)
                    return _MasterViewControlObject.State;
                else
                    return _State;
            }
            set
            {
                if (_State == ViewControlObjectState.Passive && value != ViewControlObjectState.Initialize)
                    return;

                if (_MasterViewControlObject != null)
                    _MasterViewControlObject.State = value;
                else
                {
                    ViewControlObjectState oldState = _State;
                    _State = value;
                    if (_State != oldState)
                        UserInterfaceObjectConnectionChangeState(oldState, _State);
                }


            }
        }




        /// <exclude>Excluded</exclude>
        protected object _Instance;
        /// <MetaDataID>{ce514c8b-0839-4762-8af6-a19848f4e833}</MetaDataID>
        public object Instance
        {
            get
            {
                return _Instance;

            }
            set
            {
                SetInstance(value);
            }
        }

        /// <exclude>Excluded</exclude>
        public string PresentationObjectTypeFullName;
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{f05ffef6-ef8c-4452-9d9b-16e3a803cb4f}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier PresentationObjectType
        {
            get
            {
                if (ObjectType == null)
                    return null;
                if (_PresentationObjectType != null)
                    return _PresentationObjectType;
                else
                {
                    if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
                    {
                        _PresentationObjectType = UISession.GetClassifier(PresentationObjectTypeFullName, "", true, this) as OOAdvantech.MetaDataRepository.Class;
                        if (_PresentationObjectType != null)
                            return _PresentationObjectType;
                    }
                    return ObjectType;
                }

            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Class)
                    _PresentationObjectType = value as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType != null)
                    PresentationObjectTypeFullName = _PresentationObjectType.FullName;
                if (value == null)
                {
                    PresentationObjectTypeFullName = "";
                    _PresentationObjectType = null;
                }

            }
        }
        /// <exclude>Excluded</exclude>
        protected object _PresentationObject;
        /// <MetaDataID>{642e646b-4afc-463d-a987-e2f6cdefa602}</MetaDataID>
        public object PresentationObject
        {
            get
            {
                if (_PresentationObjectType != null && _Instance != null && _PresentationObject == null)
                {
                    _PresentationObject = GetPresentationObject(_Instance, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);
                }
                if (_PresentationObject == null)
                    return _Instance;
                else
                    return _PresentationObject;
            }
        }


        /// <exclude>Excluded</exclude>
        string _ViewObjectTypeFullName;
        /// <MetaDataID>{0f2c46cd-5529-4b7a-a024-f8413cd7f7a5}</MetaDataID>
        public string ViewObjectTypeFullName
        {
            get
            {
                return _ViewObjectTypeFullName;
            }
            set
            {
                _ViewObjectTypeFullName = value;
                _ObjectType = null;
            }
        }

        /// <MetaDataID>{69128a52-bd45-4709-98b9-444ed46b7b73}</MetaDataID>
        public string AssemblyMetadata;

        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Classifier _ObjectType;
        /// <MetaDataID>{bcff5b64-9282-46a6-bfc1-6cb223b09d05}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier ObjectType
        {
            get
            {
                if (_ObjectType != null)
                    return _ObjectType;
                if (!string.IsNullOrEmpty(ViewObjectTypeFullName))
                {
                    _ObjectType = UISession.GetClassifier(ViewObjectTypeFullName, AssemblyMetadata, true, this);
                }
                return _ObjectType;
            }
        }
        /// <MetaDataID>{cc89d5ff-a23f-430e-9279-94fed822019c}</MetaDataID>
        public void SetInstance(object value)
        {
            _UseDefaultCultureWhenValueMissing = CultureContext.UseDefaultCultureValue;
            Culture = CultureContext.CurrentCultureInfo;
            //for initialization
            MetaDataRepository.Classifier presentationObjectType = PresentationObjectType;

            if (State != ViewControlObjectState.DesigneMode)
            {
                if (ObjectType == null && value != null)
                    throw new System.Exception("Error in metadata of connenction control");
                if (value != null && !(ObjectType.GetExtensionMetaObject(typeof(Type)) as Type).IsInstanceOfType(value))
                    throw new System.Exception("Connenction control type mismatch");

            }

            object oldInstanceValue = _Instance;
            if (_Instance != value)
                _PresentationObject = null;
            _Instance = value;

            if (State == ViewControlObjectState.Initialize || State == ViewControlObjectState.DesigneMode)
                return;


            #region Precondition check
            //if (ContainerControl == null)
            //    throw new System.Exception(string.Format("There isn't ContainerControl ({0})", Name));

            if (this.GetType() != typeof(FormObjectConnection) && MasterViewControlObject == null)
                throw new System.Exception(string.Format("There isn't 'FormConnectionControl' as root of ViewControlObject tree"));
            #endregion


            if (_Instance != oldInstanceValue)
            {

                if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                    return;

                if (_Instance != null && IniateTransactionOnInstanceSet)
                {
                    UISession uiSession = null;
                    if (MasterViewControlObject != null)
                        uiSession = MasterViewControlObject.UserInterfaceSession;


                    if (_Transaction != null)
                        (_Transaction as CommittableTransaction).Abort();

                    InitiateTransaction();

                    using (SystemStateTransition masterStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction, TransactionObjectLockTimeOut))
                        {
                            try
                            {
                                UserInterfaceSession.MasterUserInterfaceSession = uiSession;
                                //TODO ενώ δεν την δημειουργεί το  FormObjectConnection εγώ το χρεώνω στο FormObjectConnection
                                if (UserInterfaceSession.StartingUserInterfaceObjectConnection == null)
                                    UserInterfaceSession.StartingUserInterfaceObjectConnection = this;

                                if (value != null && ObjectType != null)
                                    _PresentationObject = GetPresentationObject(value, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                                LoadControlsData();

                                #region review
                                if (InstanceChanged != null)
                                    InstanceChanged(this, EventArgs.Empty);
                                #endregion

                            }
                            finally
                            {
                                stateTransition.Consistent = true;
                            }
                        }
                        masterStateTransition.Consistent = true;
                    }
                    return;

                }
                if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction, TransactionObjectLockTimeOut))
                    {
                        try
                        {
                            if (value != null && ObjectType != null)
                                _PresentationObject = GetPresentationObject(value, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                            LoadControlsData();

                            #region review
                            if (InstanceChanged != null)
                                InstanceChanged(this, EventArgs.Empty);
                            #endregion

                        }
                        finally
                        {
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                {
                    if (Transaction == null)
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            if (value != null && ObjectType != null)
                                _PresentationObject = GetPresentationObject(value, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);
                            LoadControlsData();
                            #region review
                            if (InstanceChanged != null)
                                InstanceChanged(this, EventArgs.Empty);
                            #endregion

                            stateTransition.Consistent = true;
                        }


                    }
                    else
                    {
                        if (value != null && ObjectType != null)
                            _PresentationObject = GetPresentationObject(value, _PresentationObjectType, ObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                        LoadControlsData();

                        #region review
                        if (InstanceChanged != null)
                            InstanceChanged(this, EventArgs.Empty);
                        #endregion
                    }

                }
            }
        }

        //protected void CreatePresentationObject(object value)
        //{
        //    _PresentationObject = null;
        //    if (PresentationObjectType != null)
        //    {
        //        if (_ObjectType == null)
        //            if (!string.IsNullOrEmpty(ViewObjectTypeFullName))
        //                _ObjectType = GetClassifier(ViewObjectTypeFullName, true);

        //        if (_PresentationObjectType == null && !string.IsNullOrEmpty(PresentationObjectTypeFullName))
        //        {
        //            _PresentationObjectType = GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
        //            if (_PresentationObjectType != null)
        //            {

        //                System.Reflection.ConstructorInfo contructor = ((_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type).GetConstructor(new System.Type[] { _ObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type });
        //                _PresentationObject = contructor.Invoke(new object[1] { value });
        //            }


        //        }
        //        else if (_PresentationObjectType != null)
        //        {
        //            System.Reflection.ConstructorInfo contructor = ((_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type).GetConstructor(new System.Type[] { _ObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type });
        //            _PresentationObject = contructor.Invoke(new object[1] { value });


        //        }


        //    }
        //}

        /// <summary>
        ///  Disconnect controls from displayed values tree.
        ///  Removes the event handlers where consume the object change state events
        /// </summary>
        /// <MetaDataID>{ba22e94f-120b-49cb-a3b6-82d07485f5b5}</MetaDataID>
        internal void ReleaseDataPathNodes()
        {
            foreach (object component in _ControlledComponents)
            {

                if (component is UserInterfaceObjectConnection)
                    (component as UserInterfaceObjectConnection).ReleaseDataPathNodes();

            }
            foreach (ObjectChangeStateManager ObjectChangeStateManager in ObjectChangeStateManagers.Values)
                ObjectChangeStateManager.ReleaseDataPathNodes();

            ObjectChangeStateManagers.Clear();
        }
        /// <MetaDataID>{1592868e-3b76-4253-8e4b-c0b9e8a6c3d4}</MetaDataID>
        public void ReleaseDataPathNodes(IPathDataDisplayer pathDataDisplayer)
        {
            if (pathDataDisplayer == null)
                return;
            else
            {
                ObjectChangeStateManager objectChangeStateManager = null;
                if (ObjectChangeStateManagers.TryGetValue(pathDataDisplayer, out objectChangeStateManager))
                    objectChangeStateManager.ReleaseDataPathNodes();
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.PathNode _RootObjectNode;
        /// <MetaDataID>{7107ceda-f531-45d1-b04a-a33628f86c87}</MetaDataID>
        public OOAdvantech.UserInterface.PathNode RootObjectNode
        {
            get
            {
                try
                {
                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                    {
                        if (_RootObjectNode == null)
                        {
                            _RootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                            foreach (object component in _ControlledComponents)
                            {
                                if (component is IPathDataDisplayer)
                                {
                                    if ((component as IPathDataDisplayer).Paths == null)
                                        continue;

                                    foreach (string path in (component as IPathDataDisplayer).Paths)
                                    {
                                        if (string.IsNullOrEmpty(path))
                                            continue;
                                        System.Diagnostics.Debug.WriteLine(path);
                                        if (_PresentationObjectType != null && path.IndexOf("RealObject.") != 0)
                                            continue;

                                        if (path.IndexOf("RealObject.") == 0)
                                            _RootObjectNode.AddPath(path.Substring("RealObject.".Length));
                                        else
                                            _RootObjectNode.AddPath(path);
                                    }

                                }
                            }
                            if (_PresentationObjectType != null)
                            {
                                foreach (string path in PresentationObjectPaths.GetExtraPathsFor(_PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type))
                                {
                                    if (path.IndexOf("RealObject.") == 0)
                                        _RootObjectNode.AddPath(path.Substring("RealObject.".Length));
                                }
                            }
                        }
                        else if (State == ViewControlObjectState.DesigneMode)
                        {
                            foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                            {
                                if (component is IPathDataDisplayer)
                                {
                                    foreach (string path in (component as IPathDataDisplayer).Paths)
                                    {
                                        System.Diagnostics.Debug.WriteLine(path);
                                        _RootObjectNode.AddPath(path);
                                    }
                                }
                            }
                        }
                        stateTransition.Consistent = true;
                    }
                }
                catch (System.Exception error)
                {

                }
                return _RootObjectNode;

            }
            set
            {
            }

        }


        /// <MetaDataID>{ed11a960-49e7-45e9-b13f-8cdeeb581a02}</MetaDataID>
        internal protected void UserInterfaceObjectConnectionChangeState(ViewControlObjectState oldState, ViewControlObjectState newState)
        {



            #region review
            //if (BeforeLoadControlsData != null)
            //    BeforeLoadControlsData(this, EventArgs.Empty);
            #endregion

            for (int i = 0; i < _ControlledComponents.Count; i++)
            {
                object component = _ControlledComponents[i];
                if (component is IConnectableControl)
                    (component as IConnectableControl).UserInterfaceObjectConnectionChangeState(oldState, newState);
                if (component is UserInterfaceObjectConnection)
                    (component as UserInterfaceObjectConnection).UserInterfaceObjectConnectionChangeState(oldState, newState);
            }

        }

        /// <MetaDataID>{d58d6ea0-1df1-4956-aed7-6b7fe3a4ad42}</MetaDataID>
        internal protected void LoadControlsData()
        {
            //if (this.ObjectType == null)
            //    return;
            ViewControlObjectState oldState = ViewControlObjectState.Initialize;
            try
            {
                UserInterfaceSession.StartControlValuesUpdate();

                oldState = State;
                #region review
                //if (BeforeLoadControlsData != null)
                //    BeforeLoadControlsData(this, EventArgs.Empty);
                #endregion
                if (_Instance != null)
                {
                    if (_Instance is MarshalByRefObject)//&& OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_Instance))
                    {

                        try
                        {
                            if (MasterViewControlObject == null)
                            {
                                OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(_Instance as MarshalByRefObject, RootObjectNode.Paths);
                                //RootObjectNode.LoadDisplayedValues(structureSet, null);
                                if (structureSet != null)
                                    UserInterfaceSession.LoadDisplayedValues(RootObjectNode, structureSet, new OOAdvantech.UserInterface.Runtime.Member(RootObjectNode.Name, null, null, ObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type), -1);
                            }
                            DisplayedValue disp = null;
                            UserInterfaceSession.TryGetDisplayedValue(Instance, out disp);



                        }
                        catch (System.Exception error)
                        {

                        }

                    }
                    if (!UserInterfaceSession.ContainsKey(PresentationObject) && !PresentationObject.GetType().IsValueType)
                        UserInterfaceSession[PresentationObject] = new DisplayedValue(PresentationObject, UserInterfaceSession);

                    for (int i = 0; i < _ControlledComponents.Count; i++)
                    {
                        object component = _ControlledComponents[i];

                        if (component is IPathDataDisplayer)
                            (component as IPathDataDisplayer).LoadControlValues();
                        if (component is UserInterfaceObjectConnection)
                            (component as UserInterfaceObjectConnection).LoadControlsData();

                        if (component is IConnectableControl)
                        {
                            foreach (IDependencyProperty dependencyProperty in (component as IConnectableControl).DependencyProperties)
                            {
                                if (!string.IsNullOrEmpty(dependencyProperty.Path))
                                    dependencyProperty.LoadPropertyValue();

                            }
                        }

                    }
                }
                else
                {
                    ReleaseDataPathNodes();
                    //DisplayedValue.DisplayedValue.DisplayedValues.Clear();
                    foreach (object component in _ControlledComponents)
                    {
                        if (component is IPathDataDisplayer)
                            (component as IPathDataDisplayer).LoadControlValues();
                        if (component is UserInterfaceObjectConnection)
                            (component as UserInterfaceObjectConnection).LoadControlsData();
                    }
                }
            }
            finally
            {
                UserInterfaceSession.EndControlValuesUpdate();
            }

        }

        /// <MetaDataID>{136131ec-78c0-4710-984a-607d5fe80c55}</MetaDataID>
        public void SetValue(object obj, object value, OOAdvantech.MetaDataRepository.Classifier classifier, string path)
        {

            Type type = classifier.GetExtensionMetaObject(typeof(Type)) as Type;
            if (IsReadOnly(type, path))
                return;
            SetValue(obj, value, type, path);

        }


        /// <MetaDataID>{5b60326f-f26d-41b2-b295-b389a67777fc}</MetaDataID>
        public void SetValue(object value, string path)
        {
            if (IsReadOnly(path))
                return;

            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return;
            }
            System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
            object obj = PresentationObject;
            SetValue(obj, value, type, path);
        }

        /// <MetaDataID>{0dc9c700-0b2b-4a47-ab6c-8124316d6fd6}</MetaDataID>
        void SetValue(object obj, object value, Type type, string path)
        {
            if (IsReadOnly(type, path))
                return;
            if (UserInterfaceSession.State == UISession.SessionState.UserInteraction &&
                           (State == ViewControlObjectState.UserInteraction || State == ViewControlObjectState.SaveControlValues))
            {
                if (Transaction != null && !DragDropActionManager.UnderOwnedTransaction)
                {
                    //TODO κάνει menucommand click με new transaction στο operation ανοίγει το νέο παράθυρο παλυ με  new transaction στο viewcotrol  και αν κάτι κάνει 
                    //LostFocus και έρθει εδώ με την transaction του μενού και εβγάζε "LogicalThread has already transaction" 
                    //πριν την κάνω suppress να ελεχθουν τα invoke
                    using (SystemStateTransition clearStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {

                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction, TransactionObjectLockTimeOut))
                        {
                            if (UserInterfaceSession.State != UISession.SessionState.UpdateControlValues && UserInterfaceSession.State != UISession.SessionState.Terminated)
                            {


                                if (path == "(ViewControlObject)")
                                    Instance = value;
                                else
                                {
                                    if(Culture!= CultureContext.CurrentCultureInfo||UseDefaultCultureWhenValueMissing!= CultureContext.UseDefaultCultureValue)
                                        UserInterfaceSession.SetValue(obj, value, type, path, true);
                                    else
                                        UserInterfaceSession.SetValue(obj, value, type, path, false);
                                }
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                {
                    if (Transaction == null)
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            if (UserInterfaceSession.State == UISession.SessionState.UserInteraction &&
                           (State == ViewControlObjectState.UserInteraction || State == ViewControlObjectState.SaveControlValues))
                            {


                                if (path == "(ViewControlObject)")
                                    Instance = value;
                                else
                                {
                                    if (Culture != CultureContext.CurrentCultureInfo || UseDefaultCultureWhenValueMissing != CultureContext.UseDefaultCultureValue)
                                        UserInterfaceSession.SetValue(obj, value, type, path,true);
                                    else
                                        UserInterfaceSession.SetValue(obj, value, type, path, false);

                                }
                            }

                            stateTransition.Consistent = true;
                        }


                    }
                    else
                    {


                        //testin
                        if (UserInterfaceSession.State == UISession.SessionState.UserInteraction &&
                                (State == ViewControlObjectState.UserInteraction || State == ViewControlObjectState.SaveControlValues))
                        {


                            if (path == "(ViewControlObject)")
                                Instance = value;
                            else
                                UserInterfaceSession.SetValue(obj, value, type, path);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Defines the transaction which replace the committed transaction on save;   
        /// </summary>
       protected static Dictionary<string, Transaction> OnSaveReplacedTransaction = new Dictionary<string, Transaction>();

        /// <MetaDataID>{5199001d-87ee-4ceb-978a-4a090bb5b4b2}</MetaDataID>
        protected bool BeContinue = false;
        /// <MetaDataID>{db571dc9-3cef-4093-b571-1d877ceb8881}</MetaDataID>
        public void Save()
        {
            if (IniateTransactionOnInstanceSet || MasterViewControlObject == null)
            {
                if (_Transaction is OOAdvantech.Transactions.CommittableTransaction)
                {
                    if (_Transaction.NestedTransactions.Count > 0)
                        throw new OOAdvantech.Transactions.TransactionException("There are open nested transactions");
                }


                SaveControlData();
                if (_Transaction is OOAdvantech.Transactions.CommittableTransaction)
                {
                    //using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        //using (SystemStateTransition stateTransition = new SystemStateTransition(_Transaction))
                        {
                            var culture = Culture;
                            if (culture == null)
                                culture = System.Globalization.CultureInfo.CurrentCulture;
                            using (OOAdvantech.CultureContext cultureContext = new CultureContext(culture, UseDefaultCultureWhenValueMissing))
                            {

                                RefreshUserInterface();
                                UISession oldUISession = UISession.GetUserInterfaceSession(_Transaction);
                                Transactions.Transaction newTransaction = null;
                                if (_Transaction.OriginTransaction != null)
                                    newTransaction = new OOAdvantech.Transactions.CommittableTransaction(_Transaction.OriginTransaction);
                                else
                                    newTransaction = new OOAdvantech.Transactions.CommittableTransaction();
                                Dictionary<object, DisplayedValue> displayedValues = oldUISession.DisplayedValues;
                                Dictionary<Type, List<string>> TypesPaths = new Dictionary<Type, List<string>>(oldUISession.TypesPaths);
                                List<DisplayedValue> allDisplayedValues = new List<DisplayedValue>(oldUISession.AllDisplayedValues);
                                BeContinue = true;
                                OnSaveReplacedTransaction[_Transaction.LocalTransactionUri] = newTransaction;

                                RaiseBeforeTransactionCommit();
                                (_Transaction as OOAdvantech.Transactions.CommittableTransaction).Commit();

                                OnSaveReplacedTransaction.Remove(_Transaction.LocalTransactionUri);

                                UISession newUISession = UISession.CreateDerivedSession(_Transaction, newTransaction, displayedValues, allDisplayedValues, TypesPaths);


                                newUISession.StartingUserInterfaceObjectConnection = this;
                                newUISession.MasterUserInterfaceSession = oldUISession.MasterUserInterfaceSession;
                                _Transaction = newTransaction;
                                using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(_Transaction))
                                {
                                    newUISession.Synchronize();
                                    stateTransition.Consistent = true;
                                }
                            }
                            //stateTransition.Consistent = true;
                        }
                        //suppressStateTransition.Consistent = true;
                    }


                }
                BeContinue = false;
            }
            else
            {
                MasterViewControlObject.Save();

            }
        }


        /// <MetaDataID>{1842eaf8-547d-4985-aaf3-729932161f87}</MetaDataID>
        internal void SaveControlData()
        {
            try
            {

                foreach (object component in _ControlledComponents)
                {
                    if (component is UserInterfaceObjectConnection)
                        (component as UserInterfaceObjectConnection).SaveControlData();
                }
                foreach (object component in _ControlledComponents)
                {
                    IPathDataDisplayer ObjectMemberViewControl = component as IPathDataDisplayer;
                    if (ObjectMemberViewControl != null)
                        ObjectMemberViewControl.SaveControlValues();
                    //if (component is UserInterfaceObjectConnection)
                    //    (component as UserInterfaceObjectConnection).SaveControlData();

                }
            }
            catch (System.Exception error)
            {
                throw;

            }
        }


        /// <MetaDataID>{99315aee-8091-469a-8658-f9c1df3f6692}</MetaDataID>
        public object Invoke(object instance, System.Reflection.MethodInfo methodInfo, object[] parameters)
        {
            return Invoke(instance, methodInfo, parameters, TransactionOption);
        }
        /// <MetaDataID>{7c0b8a3d-f250-4010-950d-e7fd98c6e9f5}</MetaDataID>
        public object Invoke(object instance, System.Reflection.MethodInfo methodInfo, object[] parameters, OOAdvantech.Transactions.TransactionOption transactionOption)
        {

            try
            {
                if (Culture != null)
                {
                    using (OOAdvantech.CultureContext cultureContext = new CultureContext(Culture, UseDefaultCultureWhenValueMissing))
                    {
                        return InternalInvoke(ref instance, methodInfo, parameters, transactionOption);
                    }
                }
                else
                {
                    return InternalInvoke(ref instance, methodInfo, parameters, transactionOption);
                }

            }
            finally
            {
                //#region Remove object connection control from stack
                //if (currentViewControlObjectLoaded)
                //    CurrentViewControlObject = null;
                //#endregion
            }

        }

        private object InternalInvoke(ref object instance, System.Reflection.MethodInfo methodInfo, object[] parameters, TransactionOption transactionOption)
        {

            #region Gets real objects if there are UIProxy objects
            instance = UISession.GetRealObject(instance);
            int i = 0;
            foreach (object parameterObject in parameters)
                parameters[i++] = UISession.GetRealObject(parameterObject);
            #endregion
            if (State == ViewControlObjectState.Initialize)
                return methodInfo.Invoke(instance, parameters);
            if (State == ViewControlObjectState.Passive)
                return null;

            if (Transaction != null && (transactionOption != OOAdvantech.Transactions.TransactionOption.Suppress && !DragDropActionManager.UnderOwnedTransaction))
            {
                object retValue = null;
                OOAdvantech.Transactions.Transaction transaction = Transaction;
                if (OOAdvantech.Transactions.Transaction.Current != null && OOAdvantech.Transactions.Transaction.Current != transaction)
                {
                    OOAdvantech.Transactions.Transaction originTransaction = OOAdvantech.Transactions.Transaction.Current.OriginTransaction;
                    while (originTransaction != null && originTransaction != transaction)
                        originTransaction = originTransaction.OriginTransaction;
                    if (originTransaction == transaction)
                        transaction = OOAdvantech.Transactions.Transaction.Current;
                }
                using (SystemStateTransition supressTransactionStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(transaction))
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(transactionOption, TransactionObjectLockTimeOut))
                        {
                            //System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
                            try
                            {
                                retValue = methodInfo.Invoke(instance, parameters);
                            }
                            catch (System.Exception error)
                            {
                                if (OOAdvantech.Transactions.Transaction.Current.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                                {
                                    if (error is System.Reflection.TargetInvocationException)
                                        innerStateTransition.StateTransitionTransaction.Abort(error.InnerException);
                                    else
                                        innerStateTransition.StateTransitionTransaction.Abort(error);
                                }

                                throw;
                            }
                            if (innerStateTransition.StateTransitionTransaction.Status == TransactionStatus.Continue ||
                                innerStateTransition.StateTransitionTransaction.Status == TransactionStatus.Committed)
                                innerStateTransition.Consistent = true;
                        }
                        stateTransition.Consistent = true;
                        return retValue;
                    } 
                    
                }



            }
            else
            {
                if (transactionOption == OOAdvantech.Transactions.TransactionOption.Suppress || DragDropActionManager.UnderOwnedTransaction)
                    return methodInfo.Invoke(instance, parameters);
                else
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(transactionOption, TransactionObjectLockTimeOut))
                    {
                        object retValue = methodInfo.Invoke(instance, parameters);

                        if (stateTransition.StateTransitionTransaction == null || stateTransition.StateTransitionTransaction.Status == TransactionStatus.Continue ||
                            stateTransition.StateTransitionTransaction.Status == TransactionStatus.Committed)
                            stateTransition.Consistent = true;
                        return retValue;

                    }

                }
            }
        }

        /// <MetaDataID>{f98feedc-119e-48a2-bcdc-f8232af841b1}</MetaDataID>
        public object Invoke(System.Reflection.MethodInfo methodInfo, object[] parameters, OOAdvantech.Transactions.TransactionOption transactionOption)
        {
            return Invoke(Instance, methodInfo, parameters, transactionOption);
        }
        /// <MetaDataID>{9e2ea41b-7f49-4ed3-8d90-eb617f8f2812}</MetaDataID>
        public object Invoke(System.Reflection.MethodInfo methodInfo, object[] parameters)
        {
            return Invoke(Instance, methodInfo, parameters);
        }





        /// <MetaDataID>{fc19e697-aee6-454f-a0d1-c71765d3a996}</MetaDataID>
        public void BatchLoadPathsValues(object obj, System.Type type, OOAdvantech.Collections.Generic.List<string> paths)
        {
            if (State == ViewControlObjectState.Passive)
                return;
            if (obj is System.Collections.IEnumerator)
                BatchLoadPathsValues(obj as System.Collections.IEnumerator, type, paths);
            else
            {
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    Transaction transaction = ScoopTransaction;
                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            try
                            {
                                stateTransition.Consistent = true;

                                InternalBatchLoadValues(obj, type, paths);

                            }
                            catch
                            {
                                stateTransition.Consistent = false;
                                throw;
                            }
                        }
                    }
                    else
                        InternalBatchLoadValues(obj, type, paths);
                }
            }

        }

        /// <MetaDataID>{7119e560-4935-4a8f-9662-bde200ab298c}</MetaDataID>
        private void InternalBatchLoadValues(object obj, System.Type type, OOAdvantech.Collections.Generic.List<string> paths)
        {
            if (obj is MarshalByRefObject)
            {
                OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(obj as MarshalByRefObject, paths);
                PathNode objectNode = new PathNode("Root", null);
                if (structureSet != null)
                {
                    foreach (string path in paths)
                    {
                        if (path == "Root")
                            continue;
                        if (path.IndexOf("Root.") == 0)
                            objectNode.AddPath(path.Substring("Root.".Length));
                        else
                            objectNode.AddPath(path);
                    }
                    UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, type, -1);
                }
            }
        }

        /// <MetaDataID>{78287a44-fd4d-499d-bdc0-2af863cbb705}</MetaDataID>
        public void BatchLoadPathsValues(System.Collections.IEnumerator enumerator, System.Type type, OOAdvantech.Collections.Generic.List<string> paths)
        {

            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                return;
            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            InternalBatchLoadValues(enumerator, type, paths);
                            enumerator.Reset();
                            OOAdvantech.Collections.ArrayList objectCollection = new OOAdvantech.Collections.ArrayList();
                            while (enumerator.MoveNext())
                                objectCollection.Add(enumerator.Current);
                            OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, paths);
                            if (structureSet == null)
                                return;
                            PathNode objectNode = new PathNode("Root", null);
                            foreach (string path in paths)
                            {
                                if (path == "Root")
                                    continue;
                                if (path.IndexOf("Root.") == 0)
                                    objectNode.AddPath(path.Substring("Root.".Length));
                                else
                                    objectNode.AddPath(path);

                            }
                            Member member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, type);
                            UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, member, -1);
                            return;
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                {
                    InternalBatchLoadValues(enumerator, type, paths);
                    enumerator.Reset();
                    OOAdvantech.Collections.ArrayList objectCollection = new OOAdvantech.Collections.ArrayList();
                    while (enumerator.MoveNext())
                        objectCollection.Add(enumerator.Current);
                    OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, paths);
                    if (structureSet == null)
                        return;

                    PathNode objectNode = new PathNode("Root", null);
                    foreach (string path in paths)
                    {
                        if (path == "Root")
                            continue;
                        if (path.IndexOf("Root.") == 0)
                            objectNode.AddPath(path.Substring("Root.".Length));
                        else
                            objectNode.AddPath(path);

                    }
                    Member member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, type);
                    UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, member, -1);

                }
            }



            //try
            //{
            //    using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            //    {
            //        Transaction transaction = ScoopTransaction;
            //        if (transaction != null)
            //        {
            //            using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
            //            {
            //                try
            //                {
            //                    stateTransition.Consistent = true;
            //                    InternalBatchLoadValues(enumerator, type, paths);
            //                }
            //                catch
            //                {
            //                    stateTransition.Consistent = false;
            //                    throw;
            //                }
            //            }
            //        }
            //        else
            //            InternalBatchLoadValues(enumerator, type, paths);
            //    }





            //    if (Transaction == null)
            //    {
            //        enumerator.Reset();
            //        OOAdvantech.Collections.ArrayList objectCollection = new OOAdvantech.Collections.ArrayList();
            //        while (enumerator.MoveNext())
            //            objectCollection.Add(enumerator.Current);

            //        OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, paths);
            //        PathNode objectNode = new PathNode("Root", null);
            //        foreach (string path in paths)
            //        {
            //            if (path == "Root")
            //                continue;
            //            if (path.IndexOf("Root.") == 0)
            //                objectNode.AddPath(path.Substring("Root.".Length));
            //            else
            //                objectNode.AddPath(path);

            //        }

            //        //objectNode.LoadDisplayedValues(structureSet, null);
            //        UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, new OOAdvantech.UserInterface.Runtime.Member("Root", null, null, type), -1);
            //    }
            //    else
            //    {
            //        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction, TransactionObjectLockTimeOut))
            //        {
            //            enumerator.Reset();
            //            OOAdvantech.Collections.ArrayList objectCollection = new OOAdvantech.Collections.ArrayList();
            //            while (enumerator.MoveNext())
            //                objectCollection.Add(enumerator.Current);
            //            OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, paths);
            //            PathNode objectNode = new PathNode("Root", null);
            //            foreach (string path in paths)
            //            {
            //                if (path == "Root")
            //                    continue;
            //                if (path.IndexOf("Root.") == 0)
            //                    objectNode.AddPath(path.Substring("Root.".Length));
            //                else
            //                    objectNode.AddPath(path);

            //            }
            //            Member member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, type);
            //            UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, member, -1);

            //            stateTransition.Consistent = true;

            //        }
            //    }
            //}
            //catch (System.Exception error)
            //{
            //    throw;

            //}

        }

        /// <MetaDataID>{0fb04500-4e68-4bfb-9a10-6e4fa701a0b6}</MetaDataID>
        private void InternalBatchLoadValues(System.Collections.IEnumerator enumerator, System.Type type, OOAdvantech.Collections.Generic.List<string> paths)
        {
            enumerator.Reset();
            OOAdvantech.Collections.ArrayList objectCollection = new OOAdvantech.Collections.ArrayList();
            while (enumerator.MoveNext())
                objectCollection.Add(enumerator.Current);
            OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, paths);

            if (structureSet == null)
                return;
            PathNode objectNode = new PathNode("Root", null);
            foreach (string path in paths)
            {
                if (path == "Root")
                    continue;
                if (path.IndexOf("Root.") == 0)
                    objectNode.AddPath(path.Substring("Root.".Length));
                else
                    objectNode.AddPath(path);

            }
            Member member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, type);
            UserInterfaceSession.LoadDisplayedValues(objectNode, structureSet, member, -1);
        }

        /// <MetaDataID>{40b8b786-86c7-4df7-bfae-98e0baf0b397}</MetaDataID>
        internal object GetCollection(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, ObjectChangeStateManager pathDataDisplayer)
        {
            DisplayedValue collectionDisplayedValue = GetDisplayedValue(obj, classifier, path, pathDataDisplayer);

            System.Collections.ArrayList objectCollection = new System.Collections.ArrayList();
            ////TODO Τα "Items" πρέπει να παράγονται αυτόματα αν δεν υπάρχουν 
            if (collectionDisplayedValue.Members.ContainsKey("Items"))
            {

                Transaction transaction = ScoopTransaction;
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {

                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            try
                            {
                                stateTransition.Consistent = true;
                                foreach (DisplayedValue displayedValue in collectionDisplayedValue.Members["Items"].ValuesCollection)
                                    objectCollection.Add(displayedValue.Value);
                                return objectCollection;
                            }
                            catch
                            {
                                stateTransition.Consistent = false;
                                throw;
                            }
                        }
                    }
                    else
                    {
                        foreach (DisplayedValue displayedValue in collectionDisplayedValue.Members["Items"].ValuesCollection)
                            objectCollection.Add(displayedValue.Value);
                        return objectCollection;
                    }

                }

            }
            else
                return collectionDisplayedValue.Value;
        }
        /// <MetaDataID>{04cb0a40-dc89-430c-b003-715585431726}</MetaDataID>
        internal object GetCollection(string _Path, IPathDataDisplayer pathDataDisplayer)
        {
            DisplayedValue collectionDisplayedValue = GetDisplayedValue(_Path, GetObjectChangeStateManager(pathDataDisplayer));
            System.Collections.ArrayList objectCollection = new System.Collections.ArrayList();

            ////TODO Τα "Items" πρέπει να παράγονται αυτόματα αν δεν υπάρχουν 
            if (collectionDisplayedValue.Members.ContainsKey("Items"))
            {
                Transaction transaction = ScoopTransaction;
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {

                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            try
                            {
                                stateTransition.Consistent = true;
                                foreach (DisplayedValue displayedValue in collectionDisplayedValue.Members["Items"].ValuesCollection)
                                    objectCollection.Add(displayedValue.Value);
                                return objectCollection;

                            }
                            catch
                            {
                                stateTransition.Consistent = false;
                                throw;
                            }
                        }
                    }
                    else
                    {
                        foreach (DisplayedValue displayedValue in collectionDisplayedValue.Members["Items"].ValuesCollection)
                            objectCollection.Add(displayedValue.Value);
                        return objectCollection;
                    }
                }
            }
            else
                return collectionDisplayedValue.Value;

        }
        /// <MetaDataID>{c5e6796a-d63a-4cd3-bca5-a1d8833ac0e4}</MetaDataID>
        public void InsertCollectionObject(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, object value, IPathDataDisplayer pathDataDisplayer, int index)
        {
            ObjectChangeStateManager objectChangeStateManager = GetObjectChangeStateManager(pathDataDisplayer);
            DisplayedValue collectionDisplayedValue = GetDisplayedValue(obj, classifier, path, objectChangeStateManager);
            ////TODO Τα "Items" πρέπει να παράγονται αυτόματα αν δεν υπάρχουν 

            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            collectionDisplayedValue.Members["Items"].Add(GetDisplayedValue(value), index);
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    collectionDisplayedValue.Members["Items"].Add(GetDisplayedValue(value), index);
            }

        }
        /// <MetaDataID>{f71918ea-a420-46f7-8d29-52552e5d6994}</MetaDataID>
        public void RemoveCollectionObject(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, object value)
        {
            DisplayedValue collectionDisplayedValue = GetDisplayedValue(obj, classifier, path, null);
            ////TODO Τα "Items" πρέπει να παράγονται αυτόματα αν δεν υπάρχουν 
            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            collectionDisplayedValue.Members["Items"].Remove(GetDisplayedValue(value));
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    collectionDisplayedValue.Members["Items"].Remove(GetDisplayedValue(value));
            }

        }
        /// <MetaDataID>{1d6165e9-a307-45dc-b685-daae3e9ad371}</MetaDataID>
        public System.Collections.Generic.List<MemberChange> GetChanges(object obj, Type type, string path, MemberChangeEventArg memberChangeEventArg, IPathDataDisplayer pathDataDisplayer)
        {
            if (State == ViewControlObjectState.Passive)
            {
                MemberChange memberChange = new MemberChange();
                memberChange.Type = ChangeType.None;
                System.Collections.Generic.List<MemberChange> memberChanges = new List<MemberChange>();
                memberChanges.Add(memberChange);
                return memberChanges;
            }
            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            return UserInterfaceSession.GetChanges(obj, type, path, memberChangeEventArg, pathDataDisplayer);
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    return UserInterfaceSession.GetChanges(obj, type, path, memberChangeEventArg, pathDataDisplayer);
            }

        }
        /// <MetaDataID>{3297902d-7d93-4daa-b849-90a31b7b4aef}</MetaDataID>
        bool CanAccessValue(object obj, Type type, string path, IPathDataDisplayer pathDataDisplayer)
        {
            try
            {
                if (State == ViewControlObjectState.Passive)
                    return false;
                if (Transaction != null && Transaction.Status != TransactionStatus.Continue)
                    return false;
                Transaction transaction = ScoopTransaction;
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {

                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            try
                            {
                                stateTransition.Consistent = true;
                                return UserInterfaceSession.CanAccessValue(obj, type, path, GetObjectChangeStateManager(pathDataDisplayer));
                            }
                            catch
                            {
                                stateTransition.Consistent = false;
                                throw;
                            }
                        }
                    }
                    else
                        return UserInterfaceSession.CanAccessValue(obj, type, path, GetObjectChangeStateManager(pathDataDisplayer));
                }

            }
            catch (System.Exception error)
            {
                throw;

            }

        }
        /// <MetaDataID>{9798f1ac-3054-4496-89dc-b885878be701}</MetaDataID>
        public bool CanEditValue(object obj, Type type, string path, IPathDataDisplayer pathDataDisplayer)
        {
            if (State == ViewControlObjectState.Passive)
                return false;

            if (Transaction != null && Transaction.Current == null && Transaction.Status != TransactionStatus.Continue)
                return false;

            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                Transaction transaction = Transaction;
                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            return UserInterfaceSession.CanEditValue(obj, type, path, GetObjectChangeStateManager(pathDataDisplayer)); ;
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    return UserInterfaceSession.CanEditValue(obj, type, path, GetObjectChangeStateManager(pathDataDisplayer)); ;
            }

        }
        /// <MetaDataID>{3297902d-7d93-4daa-b849-90a31b7b4aef}</MetaDataID>
        public bool Isloaded(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path)
        {
            if (State == ViewControlObjectState.Passive)
                return false;
            if (Transaction != null && Transaction.Status != TransactionStatus.Continue)
                return false;
            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            return InternalIsloaded(obj, classifier.GetExtensionMetaObject(typeof(Type)) as System.Type, path);
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    return InternalIsloaded(obj, classifier.GetExtensionMetaObject(typeof(Type)) as System.Type, path);
            }
        }
        /// <summary>
        /// The main work of this function is to load the transaction of user interface on stack and forward the call
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="type">Type where system search for member. Member is the first part of path</param>
        /// <param name="path">The path where system follow to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <returns>The displayd value of path</returns>
        /// <MetaDataID>{b049fd97-bd23-4a5e-acf3-35f727b25d54}</MetaDataID>
        DisplayedValue GetDisplayedValue(object obj, Type type, string path, ObjectChangeStateManager pathDataDisplayer)
        {
            if (!string.IsNullOrEmpty(path) && path.IndexOf("Control: ") == 0)
                return new DisplayedValue(null, UserInterfaceSession);
            if (State == ViewControlObjectState.Passive)
                throw new System.Exception("System can't retrieve displayed values in Passive mode");


            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                return new DisplayedValue(null, UserInterfaceSession);

            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            return UserInterfaceSession.GetDisplayedValue(obj, type, path, pathDataDisplayer);
                        }
                        catch (Exception error)
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                    return UserInterfaceSession.GetDisplayedValue(obj, type, path, pathDataDisplayer);
            }


        }




        /// <MetaDataID>{826953f8-4714-42d6-8c7f-6f54ccd2e4df}</MetaDataID>
        internal Transaction ScoopTransaction
        {
            get
            {
                if (Transaction != null &&
                     (Transaction.Current == null || Transaction.Current.Status != TransactionStatus.Continue))
                {
                    return Transaction;
                }
                else
                {
                    if (Transaction == null)
                        return null;
                    else
                        return Transaction.Current;
                }
            }
        }
        /// <summary>
        /// Retrieve the displayed value of object
        /// </summary>
        /// <param name="obj">the object of displayed value </param>
        /// <returns>The displayd value of object</returns>
        /// <MetaDataID>{b71a7b28-f7af-4598-bbf7-8053e1365935}</MetaDataID>
        public DisplayedValue GetDisplayedValue(object obj)
        {
            if (obj == null)
                return new DisplayedValue(null, UserInterfaceSession);
            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                return new DisplayedValue(null, UserInterfaceSession);

            Transaction transaction = ScoopTransaction;
            using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (transaction != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                    {
                        try
                        {
                            stateTransition.Consistent = true;
                            DisplayedValue displayedValue = null;
                            if (!UserInterfaceSession.TryGetDisplayedValue(obj, out displayedValue))
                                displayedValue = new DisplayedValue(obj, UserInterfaceSession);
                            return displayedValue;
                        }
                        catch
                        {
                            stateTransition.Consistent = false;
                            throw;
                        }
                    }
                }
                else
                {
                    DisplayedValue displayedValue = null;
                    if (!UserInterfaceSession.TryGetDisplayedValue(obj, out displayedValue))
                        displayedValue = new DisplayedValue(obj, UserInterfaceSession);
                    return displayedValue;
                }
            }
        }


        /// <MetaDataID>{5ec42353-7616-4538-b33c-b0b9fff6e65a}</MetaDataID>
        public void AddCollectionObject(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, object value, IPathDataDisplayer pathDataDisplayer, int index)
        {
            InsertCollectionObject(obj, classifier, path, value, pathDataDisplayer, index);
        }

        /// <MetaDataID>{9606a4d4-9166-4dd2-911a-d4ec2dfa1932}</MetaDataID>
        public void AddCollectionObject(object obj, string path, IPathDataDisplayer pathDataDisplayer, int index)
        {
            if (!string.IsNullOrEmpty(path) && path.IndexOf("Control: ") == 0)
                return;

            AddCollectionObject(PresentationObject, PresentationObjectType, path, obj, pathDataDisplayer, index);
        }






        /// <MetaDataID>{8af8ed58-1ea0-45c6-ab31-3be9ea120349}</MetaDataID>
        public void RemoveCollectionObject(object obj, string path, IPathDataDisplayer pathDataDisplayer)
        {
            if (!string.IsNullOrEmpty(path) && path.IndexOf("Control: ") == 0)
                return;
            RemoveCollectionObject(PresentationObject, PresentationObjectType, path, obj);
        }




        /// <MetaDataID>{1d76eb71-0fa6-4dc6-bfd2-22626343b576}</MetaDataID>
        public System.Collections.Generic.List<MemberChange> GetChanges(string path, MemberChangeEventArg memberChangeEventArg, IPathDataDisplayer pathDataDisplayer)
        {
            if (memberChangeEventArg.Type == ChangeType.ValueChanged)
            {
                MemberChange memberChange = new MemberChange();
                memberChange.Type = ChangeType.ValueChanged;
                memberChange.DisplayedValue = GetDisplayedValue(path, GetObjectChangeStateManager(pathDataDisplayer));

                memberChange.Index = -1;
                List<MemberChange> memberChanges = new List<MemberChange>();
                memberChanges.Add(memberChange);
                return memberChanges;
            }
            else
                return GetChanges(PresentationObject, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as System.Type, path, memberChangeEventArg, pathDataDisplayer);
        }

        /// <MetaDataID>{03498a65-0eae-4c54-b3cb-0eb7bffe5a1b}</MetaDataID>
        public List<MemberChange> GetChanges(MemberChangeEventArg change, IPathDataDisplayer pathDataDisplayer)
        {
            return GetChanges(change.MemberOwner, PresentationObjectType.GetExtensionMetaObject(change.MemberOwner.GetType()) as System.Type, change.MemberName, change, pathDataDisplayer);
        }






        /// <MetaDataID>{3360be22-0e61-471c-80f0-6130d5bdb0d5}</MetaDataID>
        public void UpdateUserInterfaceFor(object viewEditObject)
        {
            DisplayedValue displayedValue = GetDisplayedValue(viewEditObject);
            if (displayedValue != null && displayedValue.Value != null)
                displayedValue.UpdateUserInterface();

        }



        /// <MetaDataID>{bc85fb00-db28-4743-a63a-babb77a0124a}</MetaDataID>
        public OperetionCallerSourceCallers OperationCallers = new OperetionCallerSourceCallers();
        /// <MetaDataID>{6e7b7439-f6b2-44dd-a6d8-51c1d86ea9f9}</MetaDataID>
        public bool CanCall(IOperationCallerSource operetionCallerSource, OperationCall operationCall)
        {
            if (operationCall == null || operetionCallerSource == null)
                return false;

            if (OperationCallers[operetionCallerSource][operationCall].Operation != null)
                return true;
            else
                return false;

        }

        public List<UIProxy> GetAllUIProxies()
        {
            if (UserInterfaceSession != null)
                return UserInterfaceSession.DisplayedValues.Values.Select(x => x.GetUIProxy(this, false)).Where(x => x != null).ToList();
            else
                return new List<UIProxy>();

        }

        //object Invoke(OperationCall operationCall, IOperetionCallerSource operetionCallerSource)
        //{
        //    Dictionary<OperationCall, OperationCaller> operationCallsCallers = null;
        //    if (!OperationCalls.TryGetValue(operetionCallerSource, out operationCallsCallers))
        //    {
        //        operationCallsCallers =new Dictionary<OperationCall,OperationCaller>();
        //        OperationCalls[operetionCallerSource] = operationCallsCallers;
        //    }
        //    OperationCaller operationCaller = null;
        //    if (!operationCallsCallers.TryGetValue(operationCall, out operationCaller))
        //    {
        //        operationCaller = new OperationCaller(operationCall, operetionCallerSource);
        //        operationCallsCallers[operationCall]=operationCaller; 

        //    }
        //    return operationCaller.Invoke();
        //}



    }
    /// <MetaDataID>{4a3e37ef-c598-47b6-a24b-889e2266993d}</MetaDataID>
    public class OperetionCallerSourceCallers : Dictionary<IOperationCallerSource, OperationCallDictionary>
    {
        /// <MetaDataID>{1ddf290d-dbb9-4e32-ab31-7a9acd9081c9}</MetaDataID>
        public OperationCallDictionary this[IOperationCallerSource operetionCallerSource]
        {
            get
            {
                OperationCallDictionary operationCallDictionary = null;
                if (!base.TryGetValue(operetionCallerSource, out operationCallDictionary))
                {
                    operationCallDictionary = new OperationCallDictionary(operetionCallerSource);
                    base[operetionCallerSource] = operationCallDictionary;
                }
                return operationCallDictionary;
            }
        }


    }

    /// <MetaDataID>{dac147e3-f34c-4ed5-964c-8876763560ee}</MetaDataID>
    public class OperationCallDictionary : Dictionary<OperationCall, OperationCaller>
    {
        /// <MetaDataID>{cb14ac8e-5cd6-4f46-9593-95ee24414472}</MetaDataID>
        IOperationCallerSource OperetionCallerSource;
        /// <MetaDataID>{06208e76-63f1-4397-8c87-951570978214}</MetaDataID>
        public OperationCallDictionary(IOperationCallerSource operetionCallerSource)
        {
            OperetionCallerSource = operetionCallerSource;
        }

        /// <MetaDataID>{308e4412-df05-4eef-97a1-b9b1ab17c25e}</MetaDataID>
        public OperationCaller this[OperationCall operationCall]
        {
            get
            {
                OperationCaller operationCaller = null;
                if (!base.TryGetValue(operationCall, out operationCaller))
                {
                    operationCaller = new OperationCaller(operationCall, OperetionCallerSource);
                    base[operationCall] = operationCaller;

                }
                return operationCaller;
            }
        }
    }
}
