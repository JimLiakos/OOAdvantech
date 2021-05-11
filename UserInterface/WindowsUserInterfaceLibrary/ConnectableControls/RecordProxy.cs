using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Reflection.Emit;
using System.Reflection;
using System.Threading;

namespace ConnectableControls.ListView
{

    /// <MetaDataID>{cabd4d89-10e7-4a74-ab46-9e9074871ef6}</MetaDataID>
    public class RecordProxy : RealProxy, System.Runtime.Remoting.IRemotingTypeInfo
    {

        //TODO θα πρέπει όλοι να δουλεύουν απο AccessorBuilder
        #region Creates Dynamic Type
        public static void CreateProperty(TypeBuilder typeBuilder, Type propertyType, string propertyName)
        {
            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use an array with no elements: new Type[] {})
            PropertyBuilder custNamePropBldr = typeBuilder.DefineProperty(propertyName,
                                                             System.Reflection.PropertyAttributes.HasDefault,
                                                             propertyType,
                                                             null);

            // The property set and property get methods require a special
            // set of attributes.


            MethodAttributes getSetAttr = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.Abstract | MethodAttributes.SpecialName;
            //MethodAttributes.Public | MethodAttributes.SpecialName |
            //    MethodAttributes.HideBySig | MethodAttributes.Abstract;

            // Define the "get" accessor method for CustomerName.
            MethodBuilder custNameGetPropMthdBldr =
                typeBuilder.DefineMethod("get_" + propertyName,
                                           getSetAttr,
                                           propertyType,
                                           Type.EmptyTypes);


            // Define the "set" accessor method for CustomerName.
            MethodBuilder custNameSetPropMthdBldr =
                typeBuilder.DefineMethod("set_" + propertyName,
                                           getSetAttr,
                                           null,
                                           new Type[] { propertyType });



            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
            custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);
        }
        private static AssemblyBuilder assBuilder = null;
        private static ModuleBuilder modBuilder = null;
        private static void GenerateAssemblyAndModule()
        {
            if (assBuilder == null)
            {

                AssemblyName draAssemblyName = new AssemblyName();
                draAssemblyName.Name = "DynamicDataRowAdapter";
                AppDomain thisDomain = Thread.GetDomain();
                //TODO:figure out parm list to use for isSynchronized parm = true;
                assBuilder = thisDomain.DefineDynamicAssembly(draAssemblyName, AssemblyBuilderAccess.Run);
                modBuilder = assBuilder.DefineDynamicModule(assBuilder.GetName().Name, false);
            }
        }
        static public TypeBuilder GetInterfaceTypeBuilder(string interfaceName)
        {
            if (modBuilder == null)
                GenerateAssemblyAndModule();

            return modBuilder.DefineType(interfaceName, TypeAttributes.Interface | TypeAttributes.Abstract);

        }
        #endregion


        public override bool Equals(object obj)
        { 
            return base.Equals(obj);
        }
        /// <MetaDataID>{a1db8797-2511-4837-89c8-a7b9a4de9a9f}</MetaDataID>
        public bool CanCastTo(Type castType, object o)
        {
            if (castType == typeof(RecordProxy))
                return true;
            bool CanCast = false;
            try
            {
                CanCast = castType.IsInstanceOfType(TheRealObject);
            }
            finally
            {
            }
            return CanCast;
        }
        ~RecordProxy()
        {

        }

        public object TheRealObject;
        Type Type;
        object DisplayedObj;
        OOAdvantech.UserInterface.Runtime.IPresentationObject DisplayedPresentationObj;
        ConnectableControls.ListConnection ListView;
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection;
        OOAdvantech.MetaDataRepository.Classifier RealObjectType;
        /// <MetaDataID>{c88291d1-4c5b-4c67-a32e-9631c64dc372}</MetaDataID>
        public RecordProxy(ConnectableControls.ListConnection listView, Type type, object displayedObj, OOAdvantech.UserInterface.Runtime.IPresentationObject displayedPresentationObj)
            : base(type)
        {
            System.Diagnostics.Debug.WriteLine(type.FullName+"  :  "+ GetHashCode().ToString());
            ListView = listView;
            UserInterfaceObjectConnection = listView.UserInterfaceObjectConnection;
            RealObjectType = listView.PresentationObjectType;
            if (Cells == null)
                Cells = new Cell[listView.Columns.Count];

            DisplayedObj = displayedObj;
            DisplayedPresentationObj = displayedPresentationObj;
            Type = type;
            if (DisplayedPresentationObj != null)
                TheRealObject = DisplayedPresentationObj;
            else
                TheRealObject = DisplayedObj;

        }


        static internal bool InInvoke = false;
        Cell[] Cells;

        /// <MetaDataID>{d71e290f-a8de-401b-ab58-3f5c3006077a}</MetaDataID>
        public override IMessage Invoke(IMessage msg)
        {
            try
            {

                InInvoke = true;
                Object ret = null;

                IMethodMessage MethodMessage = msg as IMethodMessage;
                IMethodReturnMessage retMsg = null;
                Object[] outArgs = null;
                if (MethodMessage != null)
                {

                    if (MethodMessage.MethodName == "GetType")
                        ret = Type;
                    else if (MethodMessage.MethodBase.DeclaringType == typeof(object))
                    {
                        if (MethodMessage.MethodName == "Equals" && GetTransparentProxy() == MethodMessage.Args[0])
                            ret = true;
                        else
                            ret = MethodMessage.MethodBase.Invoke(TheRealObject, MethodMessage.Args);
                    }
                    //else if (MethodMessage.MethodName == "GetHashCode")
                    //    ret = TheRealObject.GetHashCode();
                    else if (MethodMessage.ArgCount == 0)
                    {
                        //System.Diagnostics.Debug.WriteLine(MethodMessage.MethodName);
                        if (MethodMessage.MethodBase.IsSpecialName)
                        {
                            string columnNamne = MethodMessage.MethodName.Substring(4);

                            foreach (ConnectableControls.ListView.IColumn column in ListView.Columns)
                            {
                                if (column.Name == columnNamne)
                                {
                                    bool returnValueAsCollection = false;
                                    Cell cell = Cells[column.Order];
                                    if (cell == null)
                                    {
                                        cell = new Cell(this, column);
                                        Cells[column.Order] = cell;
                                    }

                                    if (string.IsNullOrEmpty(column.ColumnMetaData.DisplayMember))
                                        ret = UserInterfaceObjectConnection.GetDisplayedValue(TheRealObject, RealObjectType, column.ColumnMetaData.Path, cell, out returnValueAsCollection);
                                    else
                                        ret = UserInterfaceObjectConnection.GetDisplayedValue(TheRealObject, RealObjectType, column.Path + "." + column.ColumnMetaData.DisplayMember, cell, out returnValueAsCollection);
                                    if (ret is System.DBNull)
                                    {
                                        ret = OOAdvantech.AccessorBuilder.GetDefaultValue((MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType);

                                    }

                                    break;

                                }

                            }
                        }
                        else
                            ret = TheRealObject.GetType().GetProperty(MethodMessage.MethodBase.Name.Substring(4)).GetValue(TheRealObject, null);
                        //if ((MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType.GetInterfaces().Contains<Type>(typeof(System.Collections.IList)))
                        //{
                        //    System.Collections.IList list = System.Activator.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(typeof(object))) as System.Collections.IList;

                        //    foreach (object obj in (ret as System.Collections.IEnumerable))
                        //    {
                        //        list.Add(new RecordProxy(obj, Type).GetTransparentProxy());
                        //    }
                        //    ret = list;
                        //}
                    }
                    else
                    {
                        if (MethodMessage.MethodBase.IsSpecialName)
                        {
                            string columnNamne = MethodMessage.MethodName.Substring(4);
                            foreach (OOAdvantech.UserInterface.Column column in ListView.ListViewMetaData.Columns)
                            {
                                if (column.Name == columnNamne)
                                {
                                    bool returnValueAsCollection = false;
                                    if (string.IsNullOrEmpty(column.DisplayMember))
                                        UserInterfaceObjectConnection.SetValue(TheRealObject, MethodMessage.Args[0], RealObjectType, column.Path);
                                    else
                                        UserInterfaceObjectConnection.SetValue(TheRealObject, MethodMessage.Args[0], RealObjectType, column.Path + "." + column.DisplayMember);
                                    break;

                                }
                            }
                        }
                        else if (TheRealObject.GetType().GetProperty(MethodMessage.MethodBase.Name.Substring(4)) != null)
                            TheRealObject.GetType().GetProperty(MethodMessage.MethodBase.Name.Substring(4)).SetValue(TheRealObject, MethodMessage.Args[0], null);
                        else
                            ret = MethodMessage.MethodBase.Invoke(TheRealObject, MethodMessage.Args);
                    }


                }

                
                if (ret == null &&
                    (MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType.IsValueType &&
                                        (MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType!=typeof(void))
                    ret=OOAdvantech.AccessorBuilder.GetDefaultValue((MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType);

                retMsg = new ReturnMessage(
                    ret,           //Object ret
                    outArgs,       //Object[] outArgs
                    0,             //int outArgsCount
                    null,          //LogicalCallContext callCtx
                    (IMethodCallMessage)msg   //IMethodCallMessage mcm
                    );
                return retMsg;
            }
            finally
            {
                InInvoke = false;
            }

        }


        #region IRemotingTypeInfo Members


        public string TypeName
        {
            get
            {
                return Type.Name;
            }
            set
            {

            }
        }

        #endregion



        internal class Cell : OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
        {
           internal RecordProxy RecordProxy;
           internal IColumn Column;
            public Cell(RecordProxy recordProxy, IColumn column)
            {
                RecordProxy = recordProxy;
                Column = column;
            }


            #region IPathDataDisplayer Members

            public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
            {
                get 
                {
                    return RecordProxy.UserInterfaceObjectConnection;
                }
            }

            public OOAdvantech.Collections.Generic.List<string> Paths
            {
                get
                {
                    return Column.Paths;
                }
            }

            public bool HasLockRequest
            {
                get 
                {
                    return false;
                }
            }

            public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
            {
                RecordProxy.DisplayedValueChanged(this);
            }

            public void LockStateChange(object sender)
            {
                
            }

            #endregion

            #region IPathDataDisplayer Members

            public object Path
            {
                get
                {
                    return "";
                }
                set
                {
                    
                }
            }

            public void LoadControlValues()
            {
                
            }

            public void SaveControlValues()
            {
                
            }

            #endregion
        }

        internal void DisplayedValueChanged(Cell cell)
        {
            int index= (ListView.HostingListView.DataSource as List<object>).IndexOf(GetTransparentProxy());
            ListView.HostingListView.RefreshRowCell(index, cell.Column);
        }

        internal static object GetObject(object _object)
        {
            if (System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) != null)
                return (System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) as RecordProxy).TheRealObject;
            else
                return _object;
         
        }
    }
}
