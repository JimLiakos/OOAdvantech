using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System.Linq;
namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{E7523E85-80F0-4406-A3B6-9FF9B6DAE991}</MetaDataID>
    internal class UISession
    {

        public enum SessionState
        {
            UpdateControlValues,
            UserInteraction,
            Terminated,
            UpdateCashingData
        }




        /// <MetaDataID>{3ddd8720-7074-4ba3-bc7c-2e5b98af925c}</MetaDataID>
        internal Dictionary<Type, List<string>> TypesPaths = new Dictionary<Type, List<string>>();



        /// <summary>
        /// This method returns the original object if the obj is wrapped from UIProxy object else return the obj
        /// </summary>
        /// <param name="obj">Defines the object where maybe is a UIProxy that wrap the original object.</param>
        /// <returns>The original object if the obj is UIProxy wrap</returns>
        /// <MetaDataID>{c85527ee-11d1-420d-b76e-ae37af9aa7a7}</MetaDataID>
        internal static object GetRealObject(object obj)
        {
            //#region review
            //if (obj is MarshalByRefObject && System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) is UIProxy)
            //    obj = (System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) as UIProxy)._RealTransparentProxy;
            //#endregion
            return obj;
        }


        /// <MetaDataID>{bf1fa36e-e26a-47ca-85ce-0e9ae311586c}</MetaDataID>
        static System.Type MetaDataRepositoryAssemblyType;
        /// <MetaDataID>{b483ba07-77f7-4d6a-885c-ae7c3cdd0ff5}</MetaDataID>
        static System.Type IDEManagerType;

        /// <MetaDataID>{8328cce0-9da7-4273-b167-0f9996f93ec5}</MetaDataID>
        internal bool CanAccessValue(object _object, Type type, string path, ObjectChangeStateManager pathDataDisplayer)
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

                        System.Reflection.MemberInfo memberInfo = GetMember(type, path);

                        if (memberInfo is System.Reflection.PropertyInfo && !(memberInfo as System.Reflection.PropertyInfo).CanRead)
                            return false;

                        if (_object != null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (_object == null)
                            return false;
                        string memberName = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);

                        if (memberInfo is System.Reflection.PropertyInfo && !(memberInfo as System.Reflection.PropertyInfo).CanRead)
                            return false;


                        DisplayedValue displayedValue = null;
                        DisplayedValue parentDisplayedValue = null;
                        if (TryGetDisplayedValue(_object, out parentDisplayedValue) && parentDisplayedValue.Members.ContainsKey(memberName))
                            displayedValue = parentDisplayedValue.Members[memberName][0];
                        else
                        {
                            if (parentDisplayedValue == null)
                                parentDisplayedValue = GetDisplayedValue(_object);

                            object subNodeobj = GetMemberValue(_object, memberInfo);
                            subNodeobj = GetRealObject(subNodeobj);
                            if (!parentDisplayedValue.Members.ContainsKey(memberName))
                                parentDisplayedValue.Members.Add(memberName, new Member(memberName, parentDisplayedValue, type, GetMemberType(memberInfo)));

                            if (subNodeobj != null)
                            {
                                if (!TryGetDisplayedValue(subNodeobj, out displayedValue))
                                    displayedValue = new DisplayedValue(subNodeobj, this);
                                parentDisplayedValue.Members[memberName][0] = displayedValue;
                            }
                        }
                        if (pathDataDisplayer != null)
                        {
                            pathDataDisplayer.AddDataPathNode(parentDisplayedValue.Members[memberName]);
                            parentDisplayedValue.Members[memberName].AddPathDataDisplayer(pathDataDisplayer);
                        }

                        if (displayedValue != null && displayedValue.Value != null)
                            return CanAccessValue(displayedValue.Value, GetMemberType(memberInfo), path, pathDataDisplayer);
                        else
                            return false;
                    }

                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return false;

        }

        /// <MetaDataID>{72a87108-8b6b-479e-ac3c-f98184117553}</MetaDataID>
        internal bool CanEditValue(object _object, Type type, string path, ObjectChangeStateManager pathDataDisplayer)
        {
            try
            {
                if (IsReadOnly(type, path))
                    return false;

                if (type == null && path == null)
                    return false;

                if (!string.IsNullOrEmpty(path))
                {
                    if (path == "(ViewControlObject)")
                        return true;
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        if (_object != null)
                        {
                            DisplayedValue displayedValue = GetDisplayedValue(_object);
                            string memberName = path;
                            if (pathDataDisplayer != null && displayedValue.Members.ContainsKey(memberName))
                            {
                                pathDataDisplayer.AddDataPathNode(displayedValue.Members[memberName]);
                                displayedValue.Members[memberName].AddPathDataDisplayer(pathDataDisplayer);
                                if (displayedValue.HasLockRequest && !displayedValue.AssignedToLockControlSession)
                                {
                                    AttachTransactionLockEvents(displayedValue);
                                    foreach (ClientLockControlSession clientLockControlSession in ServerLockControlSessions.Values)
                                        clientLockControlSession.Synchronize();
                                }
                            }
                            if (displayedValue != null && displayedValue.Members.ContainsKey(path) && displayedValue.Members[path].IsLockedFor(pathDataDisplayer))
                                return false;
                            else
                                return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        if (_object == null)
                            return false;
                        string memberName = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);

                        DisplayedValue displayedValue = null;
                        DisplayedValue parentDisplayedValue = null;
                        if (TryGetDisplayedValue(_object, out parentDisplayedValue) && parentDisplayedValue.Members.ContainsKey(memberName))
                            displayedValue = parentDisplayedValue.Members[memberName][0];
                        else
                        {
                            if (parentDisplayedValue == null)
                                parentDisplayedValue = GetDisplayedValue(_object);

                            object subNodeobj = GetMemberValue(_object, memberInfo);
                            subNodeobj = GetRealObject(subNodeobj);
                            if (!parentDisplayedValue.Members.ContainsKey(memberName))
                                parentDisplayedValue.Members.Add(memberName, new Member(memberName, parentDisplayedValue, type, GetMemberType(memberInfo)));
                            if (subNodeobj != null)
                            {
                                if (!TryGetDisplayedValue(subNodeobj, out displayedValue))
                                    displayedValue = new DisplayedValue(subNodeobj, this);
                                parentDisplayedValue.Members[memberName][0] = displayedValue;
                            }
                        }
                        if (pathDataDisplayer != null)
                        {
                            pathDataDisplayer.AddDataPathNode(parentDisplayedValue.Members[memberName]);
                            parentDisplayedValue.Members[memberName].AddPathDataDisplayer(pathDataDisplayer);
                        }
                        if (GetMemberType(memberInfo).IsValueType && parentDisplayedValue.Members[memberName].IsLockedFor(pathDataDisplayer))
                            return false;

                        if (displayedValue != null && displayedValue.Value != null)
                            return CanEditValue(displayedValue.Value, GetMemberType(memberInfo), path, pathDataDisplayer);
                        else
                            return false;
                    }

                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return false;

        }

        /// <MetaDataID>{3afc9a2b-2e44-41e9-9851-c2f6c01daabd}</MetaDataID>
        internal System.Collections.Generic.List<MemberChange> GetChanges(object obj, Type type, string path, MemberChangeEventArg memberChangeEventArg, IPathDataDisplayer pathDataDisplayer)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                try
                {

                    if (type == null && path != null && string.IsNullOrEmpty(path))
                        throw new System.Exception("You can't retrieve the displayed value of path because the type parameter is null.");
                    if (obj == null)
                        return new List<MemberChange>(); // return null display value

                    obj = GetRealObject(obj);

                    DisplayedValue parentDisplayedValue = null;
                    if (!TryGetDisplayedValue(obj, out parentDisplayedValue)) //If there isn't display value for object create one
                        parentDisplayedValue = new DisplayedValue(obj, this);

                    if (string.IsNullOrEmpty(path)) // If there isn't path the return the display value for object
                        return new List<MemberChange>();
                    else
                    {
                        int nPos = path.IndexOf(".");
                        if (nPos == -1)
                        {
                            #region Retrieves displayed value and attach the pathDataDisplayer control to displayed values tree
                            string memberName = path;
                            DisplayedValue memberDisplayedValue = null;
                            if (parentDisplayedValue.Members.ContainsKey(memberName))
                                memberDisplayedValue = parentDisplayedValue.Members[memberName][0];
                            else
                            {
                                Dictionary<string, Member> m_Members = new Dictionary<string, Member>(parentDisplayedValue.Members);
                                #region Gets the value of object member and create a new display value.
                                System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                                if (memberInfo == null)
                                    throw new System.Exception("There isn't member in '" + type.FullName + "' with name '" + memberName + ".");
                                System.Type memberType = GetMemberType(memberInfo);
                                object memberValue = GetMemberValue(obj, memberInfo);
                                parentDisplayedValue.Members.Add(memberName, new Member(memberName, parentDisplayedValue, type, memberType));

                                if (!(memberValue != null && TryGetDisplayedValue(memberValue, out memberDisplayedValue)))
                                {
                                    memberDisplayedValue = new DisplayedValue(memberValue, parentDisplayedValue.Members[memberName].MemberType, UISession.CurrentUserInterfaceSession);
                                    if (memberValue != null && memberDisplayedValue.ChangeStateExist)
                                        UISession.CurrentUserInterfaceSession[memberValue] = memberDisplayedValue;
                                }
                                parentDisplayedValue.Members[memberName][0] = memberDisplayedValue;
                                #endregion
                            }

                            if (memberChangeEventArg.Type == ChangeType.ValueChanged)
                            {
                                System.Collections.Generic.List<MemberChange> memberChanges = new List<MemberChange>();
                                MemberChange memberChange = new MemberChange();
                                memberChange.Type = ChangeType.ValueChanged;
                                memberChange.DisplayedValue = memberDisplayedValue;
                                memberChanges.Add(memberChange);
                                return memberChanges;

                            }
                            else
                            {
                                if (memberChangeEventArg.Type == ChangeType.ItemsRemoved &&
                                    !memberDisplayedValue.Members["Items"].ValuesCollection.Contains(memberChangeEventArg.Value))
                                {
                                    System.Collections.Generic.List<MemberChange> memberChanges = new List<MemberChange>();
                                    MemberChange memberChange = new MemberChange();
                                    memberChange.Type = ChangeType.ItemsRemoved;
                                    memberChange.DisplayedValue = memberChangeEventArg.Value;
                                    memberChange.Index = memberChangeEventArg.Index;
                                    memberChanges.Add(memberChange);
                                    return memberChanges;
                                }
                                if (memberChangeEventArg.Type == ChangeType.ItemsAdded &&
                                    memberDisplayedValue.Members["Items"].ValuesCollection.Count > memberChangeEventArg.Index &&
                                    memberDisplayedValue.Members["Items"].ValuesCollection[memberChangeEventArg.Index] == memberChangeEventArg.Value)
                                {
                                    System.Collections.Generic.List<MemberChange> memberChanges = new List<MemberChange>();
                                    MemberChange memberChange = new MemberChange();
                                    memberChange.Type = ChangeType.ItemsAdded;
                                    memberChange.DisplayedValue = memberChangeEventArg.Value;
                                    memberChange.Index = memberChangeEventArg.Index;
                                    memberChanges.Add(memberChange);
                                    return memberChanges;


                                }



                            }
                            #endregion

                        }
                        else
                        {
                            #region Retrieves displayed object node, and attach the pathDataDisplayer control to displayed values tree node and continue with the remaining path

                            string memberName = path.Substring(0, nPos);//Retrieve member from path
                            path = path.Substring(nPos + 1);//Retrieve the remainning path
                            DisplayedValue displayedValue = null;
                            if (TryGetDisplayedValue(obj, out displayedValue) && displayedValue.Members.ContainsKey(memberName))
                            {
                                #region Retrieve displayed value and type of member the displayed value for member already exist
                                displayedValue = displayedValue.Members[memberName][0];
                                Type memberType = GetType(type, memberName);
                                #endregion
                                if (OOAdvantech.Transactions.ObjectStateTransition.IsLocked(parentDisplayedValue.Members[memberName]))
                                    return new System.Collections.Generic.List<MemberChange>();

                                parentDisplayedValue.Members[memberName][0] = displayedValue;

                                //if the member value isn't null continue with remain path
                                if (displayedValue != null && displayedValue.Value != null)
                                    return GetChanges(displayedValue.Value, memberType, path, memberChangeEventArg, pathDataDisplayer);
                                else
                                    return new List<MemberChange>();
                            }
                            else
                            {
                                #region Retrieve value and type of member
                                System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                                if (memberInfo == null)
                                    throw new System.Exception("There isn't member in '" + type.FullName + "' with name '" + memberName + ".");

                                Type memberType = GetMemberType(memberInfo);
                                object subNodeobj = GetMemberValue(obj, memberInfo);
                                subNodeobj = GetRealObject(subNodeobj);
                                #endregion

                                #region Retrieve displayed value for member value if it isn't exist create one
                                if (subNodeobj == null || !TryGetDisplayedValue(subNodeobj, out displayedValue))
                                    displayedValue = new DisplayedValue(subNodeobj, UISession.CurrentUserInterfaceSession);
                                #endregion

                                #region Update displayed values tree and attach the control to the displayed value auto update system

                                if (!parentDisplayedValue.Members.ContainsKey(memberName))
                                    parentDisplayedValue.Members.Add(memberName, new Member(memberName, parentDisplayedValue, type, memberType));
                                parentDisplayedValue.Members[memberName][0] = displayedValue;
                                #endregion

                                //if the member value isn't null continue with remain path
                                if (displayedValue != null && displayedValue.Value != null)
                                    return GetChanges(displayedValue.Value, memberType, path, memberChangeEventArg, pathDataDisplayer);
                                else
                                    return new List<MemberChange>();
                            }
                            #endregion

                        }

                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                finally
                {
                    //#region Remove object connection control from stack
                    //if (currentViewControlObjectLoaded)
                    //    CurrentViewControlObject = null;
                    //#endregion

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
            return new List<MemberChange>();



        }
        /// <MetaDataID>{cf14846e-a823-4a59-a278-39a81145828e}</MetaDataID>
        static System.Collections.Generic.Dictionary<Type, IValueType> ValueTypes = new Dictionary<Type, IValueType>();
        /// <MetaDataID>{9ec68d9c-16de-4ba9-a9f0-757f1c9e37c9}</MetaDataID>
        public static object CopyValue(object value)
        {
            if (value == null)
                return null;
            Type type = value.GetType();
            if (value.GetType().IsValueType)
            {
                IValueType valueType = null;
                if (!ValueTypes.TryGetValue(type, out valueType))
                {
                    valueType = Activator.CreateInstance(typeof(BoxedValue<>).MakeGenericType(type), value) as IValueType;
                    ValueTypes[type] = valueType;
                }
                return valueType.CopyValue(value);

            }
            else
                return value;

        }

        /// <MetaDataID>{fce0ab6e-fe2f-476a-b2b6-217e3bffb2f8}</MetaDataID>
        internal bool SetValue(object obj, object value, Type type, string path)
        {
            bool pathValueChanged = false;
            if (string.IsNullOrEmpty(path))
                return pathValueChanged;
            try
            {
                if (obj == null)
                    return pathValueChanged;

                //retrieves the original object if the obj is UIProxy 
                obj = GetRealObject(obj);

                if (!ContainsKey(obj) && !obj.GetType().IsValueType)
                    return pathValueChanged;

                DisplayedValue parentDisplayedValue = this[obj];

                {
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        object setValue = value;
                        System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                        Type memberType = GetMemberType(memberInfo);

                        object oldValue = null;

                        //TODO το InternalLoadDisplayedValues αφηνει parentDisplayedValue.Members[path][0] null δεν σετάρει null display value 
                        if (parentDisplayedValue != null && parentDisplayedValue.Members.ContainsKey(path) && parentDisplayedValue.Members[path][0] != null)
                            oldValue = parentDisplayedValue.Members[path][0].Value;

                        if (parentDisplayedValue != null && parentDisplayedValue.Members.ContainsKey(path) &&
                            !memberType.IsValueType &&
                            object.Equals(oldValue, value))
                        {
                            //Don't set for the same values
                            //TODO σε μερικές περιπτώσεις μπορεί να χρειάζεται object.ReferenceEquals
                            return pathValueChanged;
                        }

                        //system change the real set value to UIProxy for presentation object members 
                        //if (obj is IPresentationObject
                        //    && (obj as IPresentationObject).GetRealObject() is MarshalByRefObject
                        //    && System.Runtime.Remoting.RemotingServices.GetRealProxy((obj as IPresentationObject).GetRealObject()) is UIProxy)
                        //{
                        //    setValue = GenerateProxy(setValue);
                        //}
                        if (!parentDisplayedValue.Members.ContainsKey(path))
                            parentDisplayedValue.Members.Add(path, new Member(path, parentDisplayedValue, type, memberType));
                        try
                        {
                            parentDisplayedValue.Members[path].SuspendMemberValueUpdate = true;
                            //System rejects the object change event for this member

                            SetMemberValue(obj, memberInfo, setValue);
                            pathValueChanged = true;
                        }
                        finally
                        {
                            //System end the object change state event regection
                            parentDisplayedValue.Members[path].SuspendMemberValueUpdate = false;
                        }
                        if (value == null)
                            parentDisplayedValue.Members[path][0] = new DisplayedValue(value, this);
                        else
                        {

                            if (value != null && value.GetType().IsValueType)
                                value = CopyValue(value);// (Activator.CreateInstance(typeof(BoxedValue<>).MakeGenericType(value.GetType()), value) as IValueType).CopyValue();

                            DisplayedValue displayedValue = null;
                            if (TryGetDisplayedValue(value, out displayedValue))
                                parentDisplayedValue.Members[path][0] = displayedValue;
                            else
                                parentDisplayedValue.Members[path][0] = new DisplayedValue(value, this);
                        }
                        if (MasterUserInterfaceSession != null)
                        {

                            MasterUserInterfaceSession.ObjectChangeState(parentDisplayedValue.Value, path);

                        }
                    }
                    else
                    {
                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        if (parentDisplayedValue != null && parentDisplayedValue.Members.ContainsKey(member))
                        {
                            Type memberType = GetType(type, member);
                            if (memberType.IsValueType)
                            {

                                DisplayedValue oldDisplayValue = parentDisplayedValue.Members[member][0];
                                object newValue = null;
                                if (oldDisplayValue.Value != null)
                                    newValue = CopyValue(oldDisplayValue.Value);// (Activator.CreateInstance(typeof(BoxedValue<>).MakeGenericType(oldDisplayValue.Value.GetType()), oldDisplayValue.Value) as IValueType).CopyValue();

                                pathValueChanged = SetValue(newValue, value, memberType, path);
                                if (pathValueChanged)
                                {

                                    parentDisplayedValue.Members[member][0] = new DisplayedValue(OOAdvantech.AccessorBuilder.GetDefaultValue(memberType), this);
                                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                                    SetMemberValue(obj, memberInfo, newValue);
                                    parentDisplayedValue.Members[member][0] = new DisplayedValue(newValue, this);
                                    pathValueChanged = true;
                                }
                            }

                            else
                                pathValueChanged = SetValue(parentDisplayedValue.Members[member][0].Value, value, memberType, path);

                        }
                        else
                        {
                            System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                            pathValueChanged = SetValue(GetMemberValue(obj, memberInfo), value, GetMemberType(memberInfo), path);
                        }
                    }
                }
                return pathValueChanged;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{9939a079-c8e8-4814-bb0f-6559b309d7fe}</MetaDataID>
        private void ObjectChangeState(object _object, string member)
        {
            lock (_DisplayedValues)
            {
                if (_DisplayedValues != null && _DisplayedValues.ContainsKey(_object))
                    _DisplayedValues[_object].OnObjectChangeState(_object, member);
            }
        }

        /// <MetaDataID>{b531fcef-6ec7-4cc8-8ce4-a06d87c36776}</MetaDataID>
        internal DisplayedValue GetDisplayedValue(object obj, Type type, string path, ObjectChangeStateManager pathDataDisplayer)
        {
            try
            {


                try
                {

                    if (type == null && path != null && string.IsNullOrEmpty(path))
                        throw new System.Exception("You can't retrieve the displayed value of path because the type parameter is null.");
                    if (obj == null)
                        return new DisplayedValue(null, this); // return null display value


                    obj = GetRealObject(obj);

                    DisplayedValue displayedValue = null;
                    if (!TryGetDisplayedValue(obj, out displayedValue)) //If there isn't display value for object create one
                    {
                        displayedValue = new DisplayedValue(obj, this);

                    }
                    if (string.IsNullOrEmpty(path)) // If there isn't path the return the display value for object
                        return displayedValue;
                    else
                    {
                        DisplayedValue parentDisplayedValue = this[obj];

                        List<string> paths = null;
                        if (!TypesPaths.TryGetValue(type, out paths))
                        {
                            paths = new List<string>();
                            TypesPaths.Add(type, paths);
                        }
                        if (!paths.Contains(path))
                            paths.Add(path);
                        parentDisplayedValue.AddType(type);


                        int nPos = path.IndexOf(".");
                        if (nPos == -1)
                        {
                            #region Retrieves displayed value and attach the pathDataDisplayer control to displayed values tree
                            string memberName = path;
                            DisplayedValue memberDisplayedValue = null;

                            if (!parentDisplayedValue.Members.ContainsKey(memberName) || !parentDisplayedValue.Members[memberName].Loaded)// ||
                            //(parentDisplayedValue.Members[memberName].HasValuesCollection && (parentDisplayedValue.Members[memberName][0] == null || parentDisplayedValue.Members[memberName][0].Value == null)))
                            {


                                #region Gets the value of object member and create a new display value.
                                object memberValue = null;
                                System.Type memberType = null;
                                System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                                if (memberInfo != null)
                                {
                                    //throw new System.Exception("There isn't member in '" + type.FullName + "' with name '" + memberName + ".");
                                    memberType = GetMemberType(memberInfo);
                                    memberValue = GetMemberValue(obj, memberInfo);
                                }
                                else
                                    return new DisplayedValue(null, this);

                                try
                                {
                                    if (!(memberValue != null && this.TryGetDisplayedValue(memberValue, out memberDisplayedValue)))
                                    {
                                        memberDisplayedValue = new DisplayedValue(memberValue, memberType, this);
                                        if (memberValue != null && !memberValue.GetType().IsValueType)
                                            this[memberValue] = memberDisplayedValue;
                                    }
                                    else if (memberValue == null)
                                        memberDisplayedValue = new DisplayedValue(null, memberType, this);



                                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                                    {
                                        parentDisplayedValue.Members[memberName] = new Member(memberName, parentDisplayedValue, type, memberType);
                                        parentDisplayedValue.Members[memberName][0] = memberDisplayedValue;
                                        
                                    }


                                    
                                }
                                catch (System.Exception error)
                                {
                                    throw;
                                }
                                #endregion
                            }
                            else
                            {

                                memberDisplayedValue = parentDisplayedValue.Members[memberName][0];
                            }

                            if (pathDataDisplayer != null)
                            {
                                pathDataDisplayer.AddDataPathNode(parentDisplayedValue.Members[memberName]);
                                parentDisplayedValue.Members[memberName].AddPathDataDisplayer(pathDataDisplayer);
                            }
                            //TODO γιατί null δεν θα έπρεπε
                            if (memberDisplayedValue == null)
                                return new DisplayedValue(null, this);
                            return memberDisplayedValue;
                            #endregion

                        }
                        else
                        {
                            #region Retrieves displayed object node, and attach the pathDataDisplayer control to displayed values tree node and continue with the remaining path

                            string memberName = path.Substring(0, nPos);//Retrieve member from path
                            path = path.Substring(nPos + 1);//Retrieve the remainning path

                            if (TryGetDisplayedValue(obj, out displayedValue) && displayedValue.Members.ContainsKey(memberName))
                            {
                                #region Retrieve displayed value and type of member the displayed value for member already exist
                                displayedValue = this[obj].Members[memberName][0];
                                Type memberType = GetType(type, memberName);
                                #endregion
                                if (displayedValue == null)
                                    displayedValue = new DisplayedValue(null, this);
                                if (OOAdvantech.Transactions.ObjectStateTransition.IsLocked(parentDisplayedValue.Members[memberName]))
                                    displayedValue = parentDisplayedValue.Members[memberName][0];
                                else
                                    parentDisplayedValue.Members[memberName][0] = displayedValue;

                                if (pathDataDisplayer != null)
                                {
                                    pathDataDisplayer.AddDataPathNode(parentDisplayedValue.Members[memberName]);
                                    parentDisplayedValue.Members[memberName].AddPathDataDisplayer(pathDataDisplayer);
                                }


                                //if the member value isn't null continue with remain path
                                if (displayedValue != null && displayedValue.Value != null)
                                    return GetDisplayedValue(displayedValue.Value, memberType, path, pathDataDisplayer);
                                else
                                {
                                    return new DisplayedValue(null, this);
                                }
                            }
                            else
                            {
                                #region Retrieve value and type of member
                                System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                                if (memberInfo == null)
                                    throw new System.Exception("There isn't member in '" + type.FullName + "' with name '" + memberName + ".");

                                Type memberType = GetMemberType(memberInfo);
                                object subNodeobj = GetMemberValue(obj, memberInfo);
                                subNodeobj = GetRealObject(subNodeobj);
                                #endregion

                                #region Retrieve displayed value for member value if it isn't exist create one
                                if (subNodeobj == null || !TryGetDisplayedValue(subNodeobj, out displayedValue))
                                {
                                    displayedValue = new DisplayedValue(subNodeobj, this);
                                }
                                else
                                    displayedValue = this[subNodeobj];
                                #endregion

                                #region Update displayed values tree and attach the control to the displayed value auto update system

                                if (!parentDisplayedValue.Members.ContainsKey(memberName))
                                    parentDisplayedValue.Members.Add(memberName, new Member(memberName, parentDisplayedValue, type, memberType));
                                parentDisplayedValue.Members[memberName][0] = displayedValue;
                                if (pathDataDisplayer != null)
                                    pathDataDisplayer.AddDataPathNode(parentDisplayedValue.Members[memberName]);
                                #endregion

                                //if the member value isn't null continue with remain path
                                if (displayedValue != null && displayedValue.Value != null)
                                    return GetDisplayedValue(displayedValue.Value, memberType, path, pathDataDisplayer);
                                else
                                    return displayedValue;
                            }
                            #endregion
                        }

                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                finally
                {
                    //#region Remove object connection control from stack
                    //if (currentViewControlObjectLoaded)
                    //    CurrentViewControlObject = null;
                    //#endregion

                }
            }
            finally
            {
                // ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
            return null;
        }


        /// <MetaDataID>{94a5973b-b552-4f7a-b50d-18fede00761a}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    OOAdvantech.MetaDataRepository.MetaObject metaObject = classifier.GetMember(source);
                    if (metaObject != null && metaObject is MetaDataRepository.Attribute)
                    {
                        return (metaObject as MetaDataRepository.Attribute).Type;
                    }
                    if (metaObject != null && metaObject is MetaDataRepository.AssociationEnd)
                    {
                        if ((metaObject as MetaDataRepository.AssociationEnd).CollectionClassifier != null)
                            return (metaObject as MetaDataRepository.AssociationEnd).CollectionClassifier;
                        else
                            return (metaObject as MetaDataRepository.AssociationEnd).Specification;
                    }
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == source)
                            return attribute.Type;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == source)
                            if (associationEnd.CollectionClassifier != null)
                                return associationEnd.CollectionClassifier;
                            else
                                return associationEnd.Specification;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == member)
                            return GetClassifier(attribute.Type, source); ;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == member)
                            if (associationEnd.CollectionClassifier != null)
                                return GetClassifier(associationEnd.CollectionClassifier, source);

                            else
                                return GetClassifier(associationEnd.Specification, source);
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Retrieves the value  from path with start object the obj parameter
        /// System retrieve value from business object not from caching data in user interfce.
        /// This method participates in user interface update scenario. 
        /// This scenario triggered from object change state event.
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="type">Type where system search for member. 
        /// Member is the first part of path</param>
        /// <param name="path">The path where system follow to get the value.</param>
        /// <returns>The value of path</returns>
        /// <MetaDataID>{bd76a6fa-5cdc-4db0-ac42-51cf2f2b47fa}</MetaDataID>
        static internal object GetValue(object obj, Type type, string path)
        {
            if (obj == null)
                return null;


            if (path != null && path.Length > 0)
            {
                int nPos = path.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;

                        if (propertyInfo.CanRead)
                            return propertyInfo.GetValue(obj, null);
                        else
                            return null;
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return fieldInfo.GetValue(obj);
                    }
                }
                else
                {
                    string member = path.Substring(0, nPos);
                    path = path.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        object subNodeobj = propertyInfo.GetValue(obj, null);
                        return GetValue(subNodeobj, propertyInfo.PropertyType, path);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                        object subNodeobj = fieldInfo.GetValue(obj);
                        return GetValue(subNodeobj, fieldInfo.FieldType, path);
                    }

                }

            }
            return null;

        }

        #region Batch load values to session
        /// <MetaDataID>{4a3922e3-f2c3-4fea-9924-ff762ca16ba6}</MetaDataID>
        internal void LoadDisplayedValues(PathNode objectNode, OOAdvantech.Collections.StructureSet structureSet, Type type, int recursiveStep)
        {
            if (objectNode.Paths.Count == 0)
                return;
            Member member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, type);
            LoadDisplayedValues(objectNode, structureSet, member, recursiveStep);
        }
        /// <MetaDataID>{2b02f1f5-46bf-40a4-8abc-07386610372d}</MetaDataID>
        internal void LoadDisplayedValues(PathNode objectNode, OOAdvantech.Collections.StructureSet structureSet, Runtime.Member member, int recursiveStep)
        {
            if (objectNode.Paths.Count == 0)
                return;

            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                {
                    InternalLoadDisplayedValues(objectNode, structureSet, member, recursiveStep);
                    stateTransition.Consistent = true;
                }
            }
            else
                InternalLoadDisplayedValues(objectNode, structureSet, member, recursiveStep);

        }
        /// <MetaDataID>{ffd6d5fa-5d0d-4093-8386-d75803d094f5}</MetaDataID>
        void InternalLoadDisplayedValues(PathNode objectNode, OOAdvantech.Collections.StructureSet structureSet, Runtime.Member member, int recursiveStep)
        {

            try
            {
                if (member == null)
                    member = new OOAdvantech.UserInterface.Runtime.Member(objectNode.Name, null, null, null);
                if (member.IsEnumerable)
                {
                    Runtime.DisplayedValue displayedValue = new OOAdvantech.UserInterface.Runtime.DisplayedValue(null, member.MemberType, this);
                    //Runtime.Member subMember = displayedValue.Members["Items"];
                    InternalLoadDisplayedValues(objectNode, structureSet, displayedValue.Members["Items"], recursiveStep);
                    if (!member.HasValuesCollection || member[0] == null)
                        member[0] = displayedValue;
                    else
                    {
                        #region Search for the changes and apply them
                        if (Transactions.ObjectStateTransition.IsLocked(member[0].Members["Items"]))
                            return;
                        List<DisplayedValue> loadedValues = member[0].Members["Items"].ValuesCollection;
                        List<DisplayedValue> newValues = displayedValue.Members["Items"].ValuesCollection;
                        foreach (DisplayedValue itemDisplayedValue in loadedValues)
                        {
                            if (!newValues.Contains(itemDisplayedValue))
                                member[0].Members["Items"].Remove(itemDisplayedValue);
                        }
                        int i = 0;
                        foreach (DisplayedValue itemDisplayedValue in newValues)
                        {
                            //TODO  πως αντιδρά με τα index
                            if (!loadedValues.Contains(itemDisplayedValue))
                                member[0].Members["Items"].Add(itemDisplayedValue, i);

                            i++;
                        }
                        #endregion
                    }
                }
                else
                {
                    int i = 0;
                    if (Transactions.ObjectStateTransition.IsLocked(member) && member.HasChange)
                        return;


                    foreach (OOAdvantech.Collections.StructureSet structureSetInstance in structureSet)
                    {
                        object value = structureSetInstance[objectNode.FullNameAsAlias];

                        Runtime.DisplayedValue displayedValue = GetDisplayedValue(value);

                        if (member[i] != null)
                            member[i] = displayedValue;
                        else
                            member.Add(displayedValue);
                        i++;
                        foreach (PathNode objectMemberNode in objectNode.Members)
                        {
                            Type memberType = null;
                            if (!displayedValue.Members.ContainsKey(objectMemberNode.Name))
                            {
                                memberType = GetType(member.MemberType, objectMemberNode.Name);
                                if (memberType != null)
                                    displayedValue.Members.Add(objectMemberNode.Name, new Runtime.Member(objectMemberNode.Name, displayedValue, member.MemberType, memberType));
                            }
                            else
                                memberType = displayedValue.Members[objectMemberNode.Name].MemberType;

                            if (structureSetInstance.ContainsMember(objectMemberNode.FullNameAsAlias) &&
                                !structureSetInstance.IsDerivedMember(objectMemberNode.FullNameAsAlias))
                            {
                                object memberObject = structureSetInstance[objectMemberNode.FullNameAsAlias];
                                if (memberObject is System.DBNull && memberType != null)
                                    memberObject = AccessorBuilder.GetDefaultValue(memberType);

                                displayedValue.Members[objectMemberNode.Name][0] = GetDisplayedValue(memberObject);
                            }
                            else
                            {
                                if (structureSetInstance.ContainsMember(objectMemberNode.Name))
                                    InternalLoadDisplayedValues(objectMemberNode, structureSetInstance[objectMemberNode.Name] as OOAdvantech.Collections.StructureSet, displayedValue.Members[objectMemberNode.Name], -1);
                            }
                        }
                        if (objectNode.Recursive)
                        {
                            if (recursiveStep == -1)
                                recursiveStep = objectNode.RecursiveSteps;
                            if (recursiveStep > 0)
                            {
                                if (structureSetInstance.ContainsMember(objectNode.Name))
                                {
                                    Type type = GetType(member.MemberType, objectNode.Name);
                                    if (type != null)
                                        displayedValue.Members.Add(objectNode.Name, new Runtime.Member(objectNode.Name, displayedValue, member.MemberType, type));
                                    recursiveStep--;
                                    InternalLoadDisplayedValues(objectNode, structureSetInstance[objectNode.Name] as OOAdvantech.Collections.StructureSet, displayedValue.Members[objectNode.Name], recursiveStep);
                                    recursiveStep++;
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
        #endregion


        /// <MetaDataID>{8ca05910-20f9-4f4e-b04e-94edb7c64e19}</MetaDataID>
        static OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();


        /// <MetaDataID>{49ad1c0d-6b68-4850-b657-1833cb7458f9}</MetaDataID>
        internal static object GenerateProxy(object _object)
        {
            //if (_object is MarshalByRefObject && !_object.GetType().IsCOMObject)
            //    //    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_object as MarshalByRefObject))
            //    if (!(System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) is UIProxy))
            //        return new UIProxy(_object as MarshalByRefObject, _object.GetType()).GetTransparentProxy();

            //if (_object is MarshalByRefObject
            //    && !(System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) is UIProxy)
            //    && !_object.GetType().IsCOMObject)
            //    return new UIProxy(_object as MarshalByRefObject, _object.GetType()).GetTransparentProxy();

            return _object;

        }




        /// <MetaDataID>{38286834-d637-48ab-bff6-b3aa83ee423d}</MetaDataID>
        static public Type GetType(Type type, string path)
        {
            if (type == null)
                return null;
            if (!string.IsNullOrEmpty(path))
            {
                int nPos = path.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                    return GetMemberType(memberInfo);
                }
                else
                {
                    string member = path.Substring(0, nPos);
                    path = path.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    System.Type memberType = GetMemberType(memberInfo);
                    //TODO υπαρχει πρόβλημα όταν το type είναι object collection και όχι gegneric collection
                    return GetType(memberType, path);
                }
            }
            else
                return type;
            return null;
        }
        /// <MetaDataID>{7b2ca4ab-5082-4154-b406-021297d9478e}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(System.Type type)
        {
            if (MetaDataRepositoryAssemblyType == null)
                MetaDataRepositoryAssemblyType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "");
            return MetaDataRepositoryAssemblyType.GetMethod("GetClassifier", new Type[1] { typeof(Type) }).Invoke(null, new object[1] { type }) as OOAdvantech.MetaDataRepository.Classifier;
        }
        /// <MetaDataID>{d88adbad-bc8c-4317-ada9-a127a4982382}</MetaDataID>
        static public OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, string assemblyData, bool caseSensitive, UserInterfaceObjectConnection objectConnection)
        {
            if (MetaDataRepositoryAssemblyType == null)
            {



                MetaDataRepositoryAssemblyType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version = 1.0.2.0, Culture = neutral, PublicKeyToken = 11a79ce55c18c4e7");
                try
                {
                    object VSStudio = null;
#if Net4
                    IDEManagerType = ModulePublisher.ClassRepository.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager", "CodeMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=9ce9f0a461f2c1a5");
#else
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("BuildControllAddin, Version=1.0.3.0, Culture=neutral, PublicKeyToken=b768d6b172456496");

                    VSStudio = assembly.GetType("BuildControllAddin.Connect").GetField("DTEObject").GetValue(null);
                    if (VSStudio != null || System.AppDomain.CurrentDomain.DomainManager.GetType().FullName == "Microsoft.VisualStudio.Platform.VsAppDomainManager")
                    {
                        System.Reflection.Assembly codeMetaDataRepositoryAssembly = System.Reflection.Assembly.Load("CodeMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=a849addb868b59ea");
                        IDEManagerType = codeMetaDataRepositoryAssembly.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager");
                    }
#endif

                }
                catch (System.Exception error)
                {
                }

            }


            if (IDEManagerType != null && objectConnection.State == ViewControlObjectState.DesigneMode)
            {

                if (objectConnection.MasterViewControlObject != null)
                    objectConnection = objectConnection.MasterViewControlObject;
                return IDEManagerType.GetMethod("GetClassifier", new Type[4] { typeof(string), typeof(string), typeof(bool), typeof(System.ComponentModel.Component) }).Invoke(null, new object[4] { fullName, assemblyData, caseSensitive, null }) as OOAdvantech.MetaDataRepository.Classifier;
            }
            else
                return MetaDataRepositoryAssemblyType.GetMethod("FindClassifier", new Type[4] { typeof(string), typeof(string), typeof(bool), typeof(System.Reflection.Assembly) }).Invoke(null, new object[4] { fullName, assemblyData, caseSensitive, System.Reflection.Assembly.GetEntryAssembly() }) as OOAdvantech.MetaDataRepository.Classifier;


        }


        /// <MetaDataID>{12dcd8fc-af00-4401-9717-8ffc4ca12715}</MetaDataID>
        static public OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, string assemblyName, bool caseSensitive, System.ComponentModel.Component component)
        {
            if (MetaDataRepositoryAssemblyType == null)
            {

                MetaDataRepositoryAssemblyType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "");
                try
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("BuildControllAddin, Version=1.0.2.0, Culture=neutral, PublicKeyToken=b768d6b172456496");


                    object VSStudio = null;
#if Net4
                    IDEManagerType = ModulePublisher.ClassRepository.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager", "CodeMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=9ce9f0a461f2c1a5");
#else
                    VSStudio = assembly.GetType("BuildControllAddin.Connect").GetField("DTEObject").GetValue(null);
                    if (VSStudio != null)
                    {
                        IDEManagerType = ModulePublisher.ClassRepository.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager", "CodeMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=9ce9f0a461f2c1a5");
                        System.Reflection.Assembly codeMetaDataRepositoryAssembly = System.Reflection.Assembly.Load("CodeMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=a849addb868b59ea");
                        IDEManagerType = codeMetaDataRepositoryAssembly.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager");
                    }
#endif


                }
                catch (System.Exception error)
                {
                }

            }
            if (IDEManagerType != null)
                return IDEManagerType.GetMethod("GetClassifier", new Type[4] { typeof(string), typeof(string), typeof(bool), typeof(System.ComponentModel.Component) }).Invoke(null, new object[4] { fullName, assemblyName, caseSensitive, component }) as OOAdvantech.MetaDataRepository.Classifier;
            else
                return MetaDataRepositoryAssemblyType.GetMethod("FindClassifier", new Type[4] { typeof(string), typeof(string), typeof(bool), typeof(System.Reflection.Assembly) }).Invoke(null, new object[2] { fullName, caseSensitive }) as OOAdvantech.MetaDataRepository.Classifier;

        }

        /// <summary>
        /// Return the member metadata form parameter type with name the parameter memberName.
        /// If the there isn't then return null. If there are more than one in hierarchy then return the member which
        /// is declared in type of parameter type.
        /// Method is useful for member which is proprty or field.
        /// </summary>
        /// <param name="type">Defines the type where method look for member</param>
        /// <param name="memberName">Defines the member name</param>
        /// <returns>Member metadata object</returns>
        /// <MetaDataID>{a07dd120-d5db-4026-8dcf-72a2b19aada6}</MetaDataID>
        internal static System.Reflection.MemberInfo GetMember(Type type, string memberName)
        {
            System.Reflection.MemberInfo[] members = type.GetMember(memberName);
            if (members.Length > 0)
                return members[0];
            else
            {
                if (MetaDataRepositoryAssemblyType == null)
                    MetaDataRepositoryAssemblyType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "");
                OOAdvantech.MetaDataRepository.Classifier clasifier = MetaDataRepositoryAssemblyType.GetMethod("GetClassifier", new Type[1] { typeof(System.Type) }).Invoke(null, new object[1] { type }) as OOAdvantech.MetaDataRepository.Classifier;
                if (clasifier != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in clasifier.GetAttributes(true))
                    {
                        if (attribute.Name == memberName)
                            return attribute.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in clasifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == memberName)
                            return associationEnd.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
                    }
                }
                return null;
            }
        }
        /// <MetaDataID>{ed565836-6bcb-4630-8c55-b6bcde99ef91}</MetaDataID>
        private static Type GetMemberType(System.Reflection.MemberInfo memberInfo)
        {
            Type memberType = null;
            if (memberInfo is System.Reflection.PropertyInfo)
                memberType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
            if (memberInfo is System.Reflection.FieldInfo)
                memberType = (memberInfo as System.Reflection.FieldInfo).FieldType;
            return memberType;
        }
        /// <MetaDataID>{b65216f4-f7ea-48ac-856c-40f87c0d95f1}</MetaDataID>
        static public string[] GetExtraPathsFor(Type type, string path)
        {
            if (type == null)
                return new string[0];
            if (!string.IsNullOrEmpty(path))
            {
                int nPos = path.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                    return CollectionObjectPaths.GetPathsFor(memberInfo);
                }
                else
                {
                    string member = path.Substring(0, nPos);
                    path = path.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    System.Type memberType = GetMemberType(memberInfo);
                    //TODO υπαρχει πρόβλημα όταν το type είναι object collection και όχι gegneric collection
                    return GetExtraPathsFor(memberType, path);
                }
            }

            return new string[0];

        }


        /// <MetaDataID>{2ac4c865-dece-4416-a47c-af2289373f43}</MetaDataID>
        private static object GetMemberValue(object obj, System.Reflection.MemberInfo memberInfo)
        {
            try
            {

                object memberValue = null;
                if (memberInfo is System.Reflection.PropertyInfo)
                    memberValue = (memberInfo as System.Reflection.PropertyInfo).GetValue(obj, null);
                if (memberInfo is System.Reflection.FieldInfo)
                    memberValue = (memberInfo as System.Reflection.FieldInfo).GetValue(obj);

                return GetRealObject(memberValue);
            }
            catch (Exception error)
            {

                //TODO θα πρέπει να ενημερώνεται το user interafce αν θα πνήξει το exception


                throw;


            }
        }
        /// <MetaDataID>{9be85d83-3a2e-442b-8a1a-c20cd30719b5}</MetaDataID>
        private static void SetMemberValue(object obj, System.Reflection.MemberInfo memberInfo, object value)
        {
            try
            {
                if (memberInfo is System.Reflection.PropertyInfo)
                    (memberInfo as System.Reflection.PropertyInfo).SetValue(obj, value, null);
                if (memberInfo is System.Reflection.FieldInfo)
                    (memberInfo as System.Reflection.FieldInfo).SetValue(obj, value);
            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{cf378f6d-1287-41b2-86bd-1f656d5849d2}</MetaDataID>
        static internal bool IsReadOnly(Type type, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, source);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        return !propertyInfo.CanWrite;
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return fieldInfo.IsInitOnly;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);



                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        //if (!propertyInfo.CanWrite)
                        //    return true;
                        if (propertyInfo.PropertyType.IsValueType && !propertyInfo.CanWrite)
                            return true;
                        return IsReadOnly(propertyInfo.PropertyType, source);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        if (fieldInfo.FieldType.IsValueType && fieldInfo.IsInitOnly)
                            return true;
                        return IsReadOnly(fieldInfo.FieldType, source);
                    }
                }
            }
            if (type != null)
                System.Diagnostics.Debug.WriteLine("System can't find member '" + source + "' on type " + type.FullName + ".");
            else
                System.Diagnostics.Debug.WriteLine("System can't find member '" + source + "'.");

            return true;


        }

        /// <MetaDataID>{98f256aa-ab65-4c0f-8fd3-d3493119a46d}</MetaDataID>
        SessionState _State = SessionState.UserInteraction;
        /// <MetaDataID>{d7477e73-bf67-4794-90c5-9bdfd5be5eb1}</MetaDataID>
        public SessionState State
        {
            get
            {
                if (Transaction != null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                    _State = SessionState.Terminated;
                return _State;
            }
            private set
            {
                lock (this)
                {
                    _State = value;
                }
            }
        }

        public readonly int MainThreadID;
        /// <MetaDataID>{e32e6f17-cf33-4ad8-bff9-ba1e87060fa6}</MetaDataID>
        public readonly Transactions.Transaction Transaction;
        /// <MetaDataID>{46e3d446-867d-4229-80c0-2e9d1be91e28}</MetaDataID>
        UISession(Transactions.Transaction transaction)
        {
            MainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (transaction != null)
            {
                Transaction = transaction;
                transaction.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompletted);
            }
        }

        /// <MetaDataID>{c4a39af6-da20-4356-b30c-b0c6c96aff52}</MetaDataID>
        internal UserInterfaceObjectConnection StartingUserInterfaceObjectConnection;


        /// <MetaDataID>{2e379ebc-c6a1-4077-8c01-3c68d5b27673}</MetaDataID>
        internal FormObjectConnection StartingFormObjectConnection
        {
            get
            {
                if (StartingUserInterfaceObjectConnection is FormObjectConnection)
                    return StartingUserInterfaceObjectConnection as FormObjectConnection;
                else if (StartingUserInterfaceObjectConnection != null)
                    return StartingUserInterfaceObjectConnection.MasterViewControlObject as FormObjectConnection;
                else
                    return null;

            }
        }

        //internal FormObjectConnection StartingFormObjectConnection;




        /// <MetaDataID>{d37c1871-2908-4884-af6f-cd970b96e0b7}</MetaDataID>
        internal void StartUpdateCashingData()
        {
            System.Threading.Monitor.Enter(this);
            if (State != SessionState.UserInteraction)
            {
                System.Threading.Monitor.Exit(this);
                throw new System.Exception("User Interface Session in Wrong State");
            }
            State = SessionState.UpdateCashingData;
        }

        /// <MetaDataID>{fe6ba7f3-12e5-46fe-a569-07390c9983fb}</MetaDataID>
        internal void EndUpdateCashingData()
        {
            State = SessionState.UserInteraction;
            System.Threading.Monitor.Exit(this);
        }


        /// <MetaDataID>{399ed5de-b6a1-4591-b7f2-588a1e250960}</MetaDataID>
        int ControlValuesUpdateRefCount = 0;

        /// <MetaDataID>{ca7c0bca-360c-479f-bcf9-6be225ece274}</MetaDataID>
        internal void StartControlValuesUpdate()
        {

            //TODO Αυτός ο μηχανισμός θα πρέπει να προστατευτή από τυχών exception στο ενδιάμεσο του κύκλου
            System.Threading.Monitor.Enter(this);
            if (State != SessionState.UpdateControlValues && State != SessionState.UserInteraction)
            {
                System.Threading.Monitor.Exit(this);
                throw new System.Exception("User Interface Session in Wrong State");
            }

            State = SessionState.UpdateControlValues;
            ControlValuesUpdateRefCount++;
        }

        public bool CannotEnterUpdateControlValuesMode
        {
            get
            {
                if (State != SessionState.UpdateControlValues && State != SessionState.UserInteraction)
                    return true;
                else
                    return false;
            }
        }
        /// <MetaDataID>{12930c3e-d0a6-46e6-8390-1e5743fb631f}</MetaDataID>
        internal void EndControlValuesUpdate()
        {
            //TODO Αυτός ο μηχανισμός θα πρέπει να προστατευτή από τυχών exception στο ενδιάμεσο του κύκλου

            ControlValuesUpdateRefCount--;
            if (ControlValuesUpdateRefCount < 0)
                throw new System.Exception("ControlValuesUpdateRefCoun error");
            if (ControlValuesUpdateRefCount == 0)
                State = SessionState.UserInteraction;

            System.Threading.Monitor.Exit(this);
        }



        /// <MetaDataID>{25ef9151-f022-45d8-9fd3-4bb8205c2e4e}</MetaDataID>
        internal DisplayedValue GetDisplayedValue(object obj)
        {
            if (obj == null)
                return new DisplayedValue(null, this);


            DisplayedValue displayedValue;
            if (!TryGetDisplayedValue(obj, out displayedValue))
            {
                displayedValue = new DisplayedValue(obj, this);
                if (!obj.GetType().IsValueType)
                {
                    lock (_DisplayedValues)
                    {
                     
                        _DisplayedValues[obj] = displayedValue;
                    }
                }
            }
            return displayedValue;
        }

        /// <MetaDataID>{9fd6b778-4ffc-4026-a4c3-031fbaf7848a}</MetaDataID>
        internal void UpdateValue(object obj, object value, Type type, string path, string fullPath)
        {
            try
            {

                DisplayedValue parentDisplayedValue = null;
                if (!TryGetDisplayedValue(obj, out parentDisplayedValue))
                    return;


                if (path != null && path.Length > 0)
                {
                    Type memberType = null;
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {


                        //TODO όταν η value είναι collection και έχει αλλάξη το περιεχόμενο και όχι η 
                        //Collection τότε πνίγει το event.
                        if (parentDisplayedValue.Members.ContainsKey(path) && parentDisplayedValue.Members[path][0] != null &&
                            parentDisplayedValue.Members[path][0].Value == value)
                        {
                            if (Transactions.ObjectStateTransition.IsLocked(parentDisplayedValue.Members[path]))
                                return;                                                 //is locked from nested transaction
                            if (parentDisplayedValue.Members[path].IsEnumerable)
                            {
                                Member member = parentDisplayedValue.Members[path];
                                #region Search for the changes and apply them
                                using (ObjectStateTransition stateTransition = new ObjectStateTransition(member, TransactionOption.Supported))
                                {
                                    List<DisplayedValue> loadedValues = parentDisplayedValue.Members[path][0].Members["Items"].ValuesCollection;

                                    List<DisplayedValue> newValues = new List<DisplayedValue>();
                                    if (value != null)
                                    {
                                        foreach (object _object in value as System.Collections.IEnumerable)
                                            newValues.Add(parentDisplayedValue.UserInterfaceSession.GetDisplayedValue(_object));
                                    }

                                    foreach (DisplayedValue itemDisplayedValue in loadedValues)
                                    {
                                        if (!newValues.Contains(itemDisplayedValue))
                                            member[0].Members["Items"].Remove(itemDisplayedValue);
                                    }
                                    int i = 0;
                                    foreach (DisplayedValue itemDisplayedValue in newValues)
                                    {
                                        //TODO  πως αντιδρά με τα index
                                        if (!loadedValues.Contains(itemDisplayedValue))
                                            member[0].Members["Items"].Add(itemDisplayedValue, i);
                                        i++;
                                    }
                                    member.UpdateUserInterface();


                                    foreach (DisplayedValue itemDisplayedValue in newValues)
                                    {

                                        //TODO  πως αντιδρά με τα index
                                        if (loadedValues.IndexOf(itemDisplayedValue) != newValues.IndexOf(itemDisplayedValue))
                                        {
                                            int newIndex = newValues.IndexOf(itemDisplayedValue);
                                            member[0].Members["Items"].Remove(itemDisplayedValue);

                                            member[0].Members["Items"].Add(itemDisplayedValue, newIndex);
                                        }
                                    }
                                    stateTransition.Consistent = true;
                                }


                                #endregion
                            }

                            return;
                        }
                        if (!parentDisplayedValue.Members.ContainsKey(path))
                        {
                            System.Reflection.FieldInfo fieldInfo = type.GetField(path);
                            if (fieldInfo != null)
                            {
                                parentDisplayedValue.Members.Add(path, new Member(path, parentDisplayedValue, type, fieldInfo.FieldType));
                                memberType = fieldInfo.FieldType;
                            }
                            else
                            {
                                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(path);
                                if (propertyInfo != null)
                                {
                                    parentDisplayedValue.Members.Add(path, new Member(path, parentDisplayedValue, type, propertyInfo.PropertyType));
                                    memberType = propertyInfo.PropertyType;
                                }
                                else
                                    parentDisplayedValue.Members.Add(path, new Member(path, parentDisplayedValue, type, null));
                            }


                        }
                        DisplayedValue displayedValue = null;
                        if (value != null && TryGetDisplayedValue(value, out displayedValue))
                            parentDisplayedValue.Members[path][0] = displayedValue;
                        else
                        {
                            if (parentDisplayedValue.Members[path].IsEnumerable)
                                parentDisplayedValue.Members[path][0] = new DisplayedValue(value, parentDisplayedValue.Members[path].MemberType, this);
                            else
                                parentDisplayedValue.Members[path][0] = new DisplayedValue(value, this);
                        }
                        return;


                    }
                    else
                    {
                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                            UpdateValue(propertyInfo.GetValue(obj, null), value, propertyInfo.PropertyType, path, fullPath);
                            return;
                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {
                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                            UpdateValue(fieldInfo.GetValue(obj), value, fieldInfo.FieldType, path, fullPath);
                            return;
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }

        }










        /// <exclude>Excluded</exclude>
        static Dictionary<string, UISession> _UserInterfaceSessions = new Dictionary<string, UISession>();
        /// <summary>
        /// This property return a collection with displayed values where belong to the current transaction
        /// if there isn't transaction on stack return a collection with the displayed values where hasn't transaction.
        /// The the displaye values of nested transaction and master transaction are the same.
        /// </summary>
        /// <MetaDataID>{22bf8230-51fa-435f-a722-aa32c281628c}</MetaDataID>
        static public UISession CurrentUserInterfaceSession
        {
            get
            {
                try
                {
                    if (OOAdvantech.Transactions.Transaction.Current == null)
                    {
                        if (!_UserInterfaceSessions.ContainsKey("null_transaction"))
                            _UserInterfaceSessions["null_transaction"] = new UISession(null);
                        return _UserInterfaceSessions["null_transaction"];//.DisplayedValues;
                    }
                    else
                    {
                        OOAdvantech.Transactions.Transaction transaction = OOAdvantech.Transactions.Transaction.Current;
                        while (transaction.OriginTransaction != null)
                            transaction = transaction.OriginTransaction;
                        if (transaction != null && transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                            throw new System.Exception("Transaction in " + transaction.Status.ToString() + " state");

                        if (_UserInterfaceSessions.ContainsKey(transaction.LocalTransactionUri.ToString()))
                            return _UserInterfaceSessions[transaction.LocalTransactionUri.ToString()];//.DisplayedValues;
                        else
                        {
                            _UserInterfaceSessions[transaction.LocalTransactionUri.ToString()] = new UISession(transaction);

                            return _UserInterfaceSessions[transaction.LocalTransactionUri.ToString()];//.DisplayedValues;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Defines a transaction completted event handler
        /// The main work of this operation is to disconnect the displayed values from the busyness objects
        /// and removes those from collection with available displayd values for the transaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <MetaDataID>{0e9da1d1-7e2e-4ea2-a4b3-c8d85493ca7b}</MetaDataID>
        void TransactionCompletted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(TransactionCompletted);
            _UserInterfaceSessions.Remove(transaction.LocalTransactionUri.ToString());

            System.Collections.Generic.List<DisplayedValue> displayedValues = null;
            lock (_DisplayedValues)
            {
                displayedValues = new List<DisplayedValue>(_DisplayedValues.Values);
                _DisplayedValues.Clear();
            }


            foreach (DisplayedValue displayedValue in displayedValues)
                displayedValue.RemoveEventHandler();
            AllDisplayedValues.Clear();

            Synchronize();
            GC.Collect();

        }
        /// <MetaDataID>{e76e71cc-72ee-4d8f-8e62-26cf5d3f5268}</MetaDataID>
        internal List<DisplayedValue> AllDisplayedValues = new List<DisplayedValue>();
        /// <MetaDataID>{0ec5a201-7f41-4cac-b9f1-8eee497df54c}</MetaDataID>
        internal void Add(DisplayedValue value)
        {
            lock (_DisplayedValues)
            {
             
                if (value.Value != null)
                    _DisplayedValues[value.Value] = value;
                AllDisplayedValues.Add(value);
            }
        }
        /// <MetaDataID>{3ca8d914-1141-4e86-a951-d084ea9a80e7}</MetaDataID>
        internal DisplayedValue this[object key]
        {
            get
            {
                lock (_DisplayedValues)
                {
                    DisplayedValue displayedValue = null;
                    if (!_DisplayedValues.TryGetValue(key, out displayedValue))
                    {
                        if (key.GetType().IsValueType)
                            return new DisplayedValue(key, this);
                    }
                    return displayedValue;
                }
            }
            set
            {
                lock (_DisplayedValues)
                {

                    _DisplayedValues[key] = value;
                }
            }
        }
        /// <MetaDataID>{95c37aa7-629b-436f-a899-2d69c7fbae93}</MetaDataID>
        internal bool TryGetDisplayedValue(object key, out DisplayedValue displayedValue)
        {

            lock (_DisplayedValues)
            {
                return _DisplayedValues.TryGetValue(key, out displayedValue);
            }
        }


        /// <MetaDataID>{6c9ac22d-5f76-475a-bbe0-77294ff0178c}</MetaDataID>
         Dictionary<object, DisplayedValue> _DisplayedValues = new Dictionary<object, DisplayedValue>();

        public Dictionary<object, DisplayedValue> DisplayedValues
        {
            get
            {
                lock(_DisplayedValues)
                {
                    return new Dictionary<object, DisplayedValue>(_DisplayedValues);
                }
            }
        }


        /// <MetaDataID>{50e67c8d-3413-457d-b9a3-8ef7abf86c89}</MetaDataID>
        public bool ContainsKey(object key)
        {
            lock (_DisplayedValues)
            {
                return _DisplayedValues.ContainsKey(key);
            }
        }

        internal class ClientLockControlSession
        {

            UISession UserInterfaceSession;
            WeakReferenceObjectLockedConsumer WeakReferenceObjectLockedConsumer;
            public readonly Transactions.ServerLockControlSession ServerLockControlSession;
            System.Collections.Generic.List<DisplayedValue> JustCreatedDisplayedValue = new List<DisplayedValue>();
            public ClientLockControlSession(Transactions.ServerLockControlSession serverLockControlSession, UISession userInterfaceSession)
            {
                UserInterfaceSession = userInterfaceSession;
                ServerLockControlSession = serverLockControlSession;
                WeakReferenceObjectLockedConsumer = new WeakReferenceObjectLockedConsumer(this, serverLockControlSession);

            }


            ~ClientLockControlSession()
            {
                try
                {

                    WeakReferenceObjectLockedConsumer.Dispose();
                    WeakReferenceObjectLockedConsumer = null;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            public void Add(DisplayedValue justCreatedDisplayedValue)
            {
                if (!justCreatedDisplayedValue.AssignedToLockControlSession)
                {
                    JustCreatedDisplayedValue.Add(justCreatedDisplayedValue);
                    justCreatedDisplayedValue.AssignedToLockControlSession = true;
                }
            }

            public event Transactions.ServerLockControlSession.ObjectLockEventHandler ObjectLocked;
            public void Synchronize()
            {
                if (JustCreatedDisplayedValue.Count == 0)
                    return;

                bool remoteCall = Remoting.RemotingServices.IsOutOfProcess(ServerLockControlSession);
                OOAdvantech.Collections.Generic.List<object> lockRequestObjects = new OOAdvantech.Collections.Generic.List<object>();
                foreach (DisplayedValue displayedValue in JustCreatedDisplayedValue)
                    lockRequestObjects.Add(displayedValue.Value);
                UISession uiSession = UserInterfaceSession;
                foreach (System.Collections.Generic.KeyValuePair<object, object> entry in ServerLockControlSession.Assign(lockRequestObjects, remoteCall))
                {
                    if (remoteCall)
                    {
                        DisplayedValue displayedValue = null;
                        lock (uiSession._DisplayedValues)
                        {
                            displayedValue = uiSession._DisplayedValues[entry.Key];
                        }
                        try
                        {
                            //υπάρχει περίπτωση λόγω multithread να έχει γίνει commit/abord

                            displayedValue.LockTransaction = Transactions.TransactionManager.UnMarshal(entry.Value as string);
                            foreach (Member member in displayedValue.Members.Values)
                            {
                                member.Locked = true;
                                if (member.MemberType.IsValueType && !member.MemberType.IsPrimitive)
                                    member.LockMemberValueTypeObjects(displayedValue.LockTransaction);
                            }
                        }
                        catch (System.Exception error)
                        {

                        }

                    }
                    else
                    {
                        DisplayedValue displayedValue = null;
                        lock (uiSession._DisplayedValues)
                        {
                            displayedValue =uiSession._DisplayedValues[entry.Key];
                        }
                        displayedValue.LockTransaction = entry.Value as Transactions.Transaction;
                        foreach (Member member in displayedValue.Members.Values)
                        {
                            member.Locked = true;
                            if (member.MemberType.IsValueType && !member.MemberType.IsPrimitive)
                                member.LockMemberValueTypeObjects(displayedValue.LockTransaction);

                        }

                    }
                }
                JustCreatedDisplayedValue.Clear();


            }



            internal void OnObjectLocked(object _object, string[] membersNames, OOAdvantech.Transactions.Transaction transaction)
            {
                if (ObjectLocked != null)
                    ObjectLocked(_object, membersNames, transaction);
            }
        }
        /// <MetaDataID>{494506c7-b0ea-49ea-83f5-6f8c656bbb76}</MetaDataID>
        System.Collections.Generic.Dictionary<string, ClientLockControlSession> ServerLockControlSessions = new Dictionary<string, ClientLockControlSession>();

        /// <MetaDataID>{49b68884-cf6c-4be4-b0a9-d596cb84e372}</MetaDataID>
        public void Synchronize()
        {
            Remoting.RemotingServices.SubscribeEventConsumers(Subscriptions);
            Remoting.RemotingServices.UnsubscribeEventConsumers(Unsubscriptions);
            Subscriptions.Clear();
            Unsubscriptions.Clear();


            System.Collections.Generic.List<DisplayedValue> lockRequestDisplayedValues = new List<DisplayedValue>();
            List<DisplayedValue> displayedValues = null;
            lock(_DisplayedValues)
            {
                displayedValues = _DisplayedValues.Values.ToList();
            }
            foreach (DisplayedValue displayedValue in displayedValues)
            {
                if (displayedValue.HasLockRequest && !displayedValue.AssignedToLockControlSession)
                {

                    AttachTransactionLockEvents(displayedValue);
                }
            }

            foreach (ClientLockControlSession clientLockControlSession in ServerLockControlSessions.Values)
                clientLockControlSession.Synchronize();
            //OOAdvantech.Remoting.su





        }

        /// <MetaDataID>{310b3526-285e-4a12-a6ee-eab665de680b}</MetaDataID>
        internal void AttachTransactionLockEvents(DisplayedValue displayedValue)
        {
            if (Remoting.RemotingServices.IsOutOfProcess(displayedValue.Value as MarshalByRefObject))
            {
                string channelUri = Remoting.RemotingServices.GetChannelUri(displayedValue.Value as MarshalByRefObject);
                ClientLockControlSession clientLockControlSession = null;
                if (!ServerLockControlSessions.TryGetValue(channelUri, out clientLockControlSession))
                {
                    clientLockControlSession = new ClientLockControlSession(Remoting.RemotingServices.CreateRemoteInstance(channelUri, typeof(Transactions.ServerLockControlSession).FullName, typeof(Transactions.ServerLockControlSession).Assembly.FullName) as Transactions.ServerLockControlSession, this);
                    clientLockControlSession.ObjectLocked += new OOAdvantech.Transactions.ServerLockControlSession.ObjectLockEventHandler(ObjectLocked);
                    ServerLockControlSessions.Add(channelUri, clientLockControlSession);

                }
                clientLockControlSession.Add(displayedValue);

            }
            else
            {
                ClientLockControlSession clientLockControlSession = null;
                if (!ServerLockControlSessions.TryGetValue("localProcess", out clientLockControlSession))
                {
                    clientLockControlSession = new ClientLockControlSession(new Transactions.ServerLockControlSession(), this);
                    clientLockControlSession.ObjectLocked += new OOAdvantech.Transactions.ServerLockControlSession.ObjectLockEventHandler(ObjectLocked);
                    ServerLockControlSessions.Add("localProcess", clientLockControlSession);

                }
                clientLockControlSession.Add(displayedValue);
            }
        }

        /// <MetaDataID>{7ac4678a-c3e4-4de8-9785-941f95da8680}</MetaDataID>
        void ObjectLocked(object _object, string[] membersNames, OOAdvantech.Transactions.Transaction transaction)
        {
            if (Transaction != null && Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                return;
            DisplayedValue displayedValue = null;
            bool exist = false;
            lock (_DisplayedValues)
            {
                exist = _DisplayedValues.TryGetValue(_object, out displayedValue);
            }
            if (exist)
            {
                displayedValue.LockTransaction = transaction;

                if (membersNames == null)
                {
                    foreach (Member member in displayedValue.Members.Values)
                        if (member.MemberType.IsValueType && !member.MemberType.IsPrimitive)
                            member.LockMemberValueTypeObjects(transaction);
                }
                else
                {
                    System.Collections.Generic.List<string> namesOfMembers = new List<string>(membersNames);
                    foreach (Member member in displayedValue.Members.Values)
                    {
                        if (namesOfMembers.Contains(member.Name) && member.MemberType.IsValueType && !member.MemberType.IsPrimitive)
                            member.LockMemberValueTypeObjects(transaction);
                    }
                }

                //TODO υπάρχει περίπτωση να γίνει commit/abord κατα μεχρι να γίνει subscribe το event;
                using (Transactions.SystemStateTransition outStateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                {
                    if (Transaction != null)
                    {
                        using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                        {
                            if (membersNames == null)
                            {
                                foreach (Member member in new List<Member>(displayedValue.Members.Values))
                                    member.Locked = true;
                            }
                            else
                            {
                                System.Collections.Generic.List<string> namesOfMembers = new List<string>(membersNames);
                                foreach (Member member in new List<Member>(displayedValue.Members.Values))
                                {
                                    if (namesOfMembers.Contains(member.Name))
                                        member.Locked = true;
                                }
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        if (membersNames == null)
                        {
                            foreach (Member member in displayedValue.Members.Values)
                                member.Locked = true;
                        }
                        else
                        {
                            System.Collections.Generic.List<string> namesOfMembers = new List<string>(membersNames);
                            foreach (Member member in displayedValue.Members.Values)
                            {
                                if (namesOfMembers.Contains(member.Name))
                                    member.Locked = true;
                            }
                        }
                    }
                }
            }
        }






        class WeakReferenceObjectLockedConsumer : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IDisposable
        {
            Transactions.ServerLockControlSession ServerLockControlSession;
            System.WeakReference WeakRefernceEventHandler;
            public WeakReferenceObjectLockedConsumer(ClientLockControlSession clientLockControlSession, Transactions.ServerLockControlSession serverLockControlSession)
            {

                WeakRefernceEventHandler = new WeakReference(clientLockControlSession);
                ServerLockControlSession = serverLockControlSession;
                ServerLockControlSession.ObjectLocked += new OOAdvantech.Transactions.ServerLockControlSession.ObjectLockEventHandler(ObjectLocked);
                System.AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            }

            void OnProcessExit(object sender, EventArgs e)
            {
                Dispose();
            }

            public void ObjectLocked(object _object, string[] membersNames, OOAdvantech.Transactions.Transaction transaction)
            {

                ClientLockControlSession clientLockControlSession = WeakRefernceEventHandler.Target as ClientLockControlSession;
                if (clientLockControlSession != null)
                    clientLockControlSession.OnObjectLocked(_object, membersNames, transaction);
                else
                {
                    ServerLockControlSession.ObjectLocked -= new OOAdvantech.Transactions.ServerLockControlSession.ObjectLockEventHandler(ObjectLocked);
                    ServerLockControlSession = null;
                }
            }


            public void Dispose()
            {
                if (ServerLockControlSession != null)
                {
                    ServerLockControlSession.ObjectLocked -= new OOAdvantech.Transactions.ServerLockControlSession.ObjectLockEventHandler(ObjectLocked);
                    ServerLockControlSession = null;
                }
            }


        }

        /// <MetaDataID>{ba2b3614-b551-411e-955c-772e425b1b65}</MetaDataID>
        internal static UISession CreateUserInterfaceSession(UserInterfaceObjectConnection startingUserInterfaceObjectConnection)
        {

            UISession uiSession = new UISession(null);
            uiSession.StartingUserInterfaceObjectConnection = startingUserInterfaceObjectConnection;
            return uiSession;
        }

        /// <MetaDataID>{6e65a3cb-4531-4818-a3bf-760525be880f}</MetaDataID>
        internal static UISession GetUserInterfaceSession(OOAdvantech.Transactions.Transaction transaction)
        {
            if (transaction == null)
                return null;
            while (transaction.OriginTransaction != null)
                transaction = transaction.OriginTransaction;
            if (transaction != null && transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                return null;

            if (_UserInterfaceSessions.ContainsKey(transaction.LocalTransactionUri.ToString()))
                return _UserInterfaceSessions[transaction.LocalTransactionUri.ToString()];
            return null;


        }

        /// <MetaDataID>{23bc73eb-c3eb-415e-af7a-709d0a11fe71}</MetaDataID>
        internal static UISession CreateDerivedSession(Transaction commitedTransaction , Transaction newTransaction, Dictionary<object, DisplayedValue> displayedValues, List<DisplayedValue> allDisplayedValues, Dictionary<Type, List<string>> typesPaths)
        {
            try
            {


                
                while (newTransaction.OriginTransaction != null)
                    newTransaction = newTransaction.OriginTransaction;
                if (newTransaction != null && newTransaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                    throw new System.Exception("Transaction in " + newTransaction.Status.ToString() + " state");

                if (_UserInterfaceSessions.ContainsKey(newTransaction.LocalTransactionUri.ToString()))
                    return _UserInterfaceSessions[newTransaction.LocalTransactionUri.ToString()];//.DisplayedValues;
                else
                {
                    UISession userInterfaceSession = new UISession(newTransaction);

                    using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(newTransaction))
                    {

                        userInterfaceSession._DisplayedValues = displayedValues;
                        userInterfaceSession.AllDisplayedValues = allDisplayedValues;

                        userInterfaceSession.TypesPaths = typesPaths;
                        foreach (DisplayedValue displayedValue in userInterfaceSession.AllDisplayedValues)
                        {

                            displayedValue.AssignedToLockControlSession = false;
                            foreach (Member member in displayedValue.Members.Values)
                                member.TransferValues(commitedTransaction, newTransaction);
                            displayedValue.UserInterfaceSession = userInterfaceSession;
                            displayedValue.AttatchEventHandlers();
                        }
                        _UserInterfaceSessions[newTransaction.LocalTransactionUri.ToString()] = userInterfaceSession;

                        stateTransition.Consistent = true;
                    }


                    return userInterfaceSession;
                }

            }
            catch (System.Exception error)
            {
                throw;
            }

        }



        /// <MetaDataID>{2ee17705-e6e0-4877-87d0-5bc62cf66bdc}</MetaDataID>
        List<EventSubscrioption> Subscriptions = new List<EventSubscrioption>();
        /// <MetaDataID>{8b98adf8-9d63-4473-895b-b058b80d8646}</MetaDataID>
        List<EventSubscrioption> Unsubscriptions = new List<EventSubscrioption>();

        /// <MetaDataID>{a3a2431a-0619-40dd-a976-7d37c662238c}</MetaDataID>
        internal void UnsubscribeFromEvent(System.Reflection.EventInfo eventInfo, object obj, ObjectChangeStateHandle objectChangeStateHandle)
        {
            if (obj is MarshalByRefObject && OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(obj as MarshalByRefObject))
                Unsubscriptions.Add(new EventSubscrioption(eventInfo, obj, objectChangeStateHandle));
            else
                eventInfo.RemoveEventHandler(obj, objectChangeStateHandle);

        }

        /// <MetaDataID>{8514c560-7460-4b5a-a66f-27decb90237e}</MetaDataID>
        internal void SubscribeOnEvent(System.Reflection.EventInfo eventInfo, object obj, ObjectChangeStateHandle objectChangeStateHandle)
        {
            if (obj is MarshalByRefObject && OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(obj as MarshalByRefObject))
                Subscriptions.Add(new EventSubscrioption(eventInfo, obj, objectChangeStateHandle));
            else

                eventInfo.AddEventHandler(obj, objectChangeStateHandle);
        }

        /// <MetaDataID>{f0cdbab3-e6ab-4a9a-b33e-7a368939e66a}</MetaDataID>
        internal UISession MasterUserInterfaceSession;


        /// <MetaDataID>{0e045540-45cf-478c-881c-df46d9ee1cd9}</MetaDataID>
        internal static OOAdvantech.MetaDataRepository.MetaObject GetMetaObject(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    MetaDataRepository.MetaObject metaObject = classifier.GetMember(source);
                    if (metaObject is OOAdvantech.MetaDataRepository.Attribute || metaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                        return metaObject;


                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == source)
                            return attribute;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == source)
                            return associationEnd;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);

                    MetaDataRepository.MetaObject metaObject = classifier.GetMember(source);
                    if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                        return GetMetaObject((metaObject as OOAdvantech.MetaDataRepository.Attribute).Type, source); ;

                    if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                    {
                        if ((metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).CollectionClassifier != null)
                            return GetMetaObject((metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).CollectionClassifier, source);
                        else
                            return GetMetaObject((metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Specification, source);
                    }
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == member)
                            return GetMetaObject(attribute.Type, source); ;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == member)
                            if (associationEnd.CollectionClassifier != null)
                                return GetMetaObject(associationEnd.CollectionClassifier, source);

                            else
                                return GetMetaObject(associationEnd.Specification, source);
                    }
                }
            }
            return null;
        }


    }


    /// <MetaDataID>{2b5d4adc-7c82-4952-b009-acf2fd055137}</MetaDataID>
    interface IValueType
    {
        /// <MetaDataID>{f3f833d0-3b3c-49ae-86d0-17bdee459083}</MetaDataID>
        object CopyValue(object value);
    }
    /// <MetaDataID>{c264a9e2-96b4-4654-aa10-13036ffa2a7b}</MetaDataID>
    struct BoxedValue<T> : IValueType
    {
        /// <MetaDataID>{24c41d1b-af5c-4d3f-bf0e-af785748977b}</MetaDataID>
        public readonly T Value;
        /// <MetaDataID>{9211588b-3e5c-4863-ac81-3ee9a87c4492}</MetaDataID>
        public BoxedValue(object value)
        {
            Value = (T)value;
        }

        #region IValueType Members

        /// <MetaDataID>{4248d675-34f3-4b14-a36b-be26a732758d}</MetaDataID>
        public object CopyValue(object value)
        {
            T unBoxedValue = (T)value;
            return unBoxedValue;
        }

        #endregion
    }
}

