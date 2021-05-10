using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;

namespace OOAdvantech.UserInterface.ReportObjectDataSource
{

    /// <MetaDataID>{84c33263-1766-4f89-9073-b9b7eadd7a8a}</MetaDataID>
    public class ObjectSateProvider : RealProxy, System.Runtime.Remoting.IRemotingTypeInfo
    {

    
        public static object GetReportData(object obj, ReportDataSource reportDataSource, OOAdvantech.UserInterface.PathNode rootObjectNode)
        {
            Type type = reportDataSource.Type.GetExtensionMetaObject(typeof(Type)) as Type;
            OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(obj as MarshalByRefObject, rootObjectNode.Paths);
            PathNode objectNode = new PathNode("Root", null);
            if (structureSet != null)
            {
                OOAdvantech.UserInterface.Runtime.UISession UISession = OOAdvantech.UserInterface.Runtime.UISession.CreateUserInterfaceSession(null);
                UISession.LoadDisplayedValues(rootObjectNode, structureSet, reportDataSource.Type.GetExtensionMetaObject(typeof(Type)) as Type, -1);
                return new ObjectSateProvider(reportDataSource.DataSourceType, obj, UISession, reportDataSource).GetTransparentProxy(); 
            }
            return null;
        }

        public static System.Collections.ICollection GetReportData(System.Collections.IList objectCollection, ReportDataSource reportDataSource, OOAdvantech.UserInterface.PathNode rootObjectNode)
        {
            if (reportDataSource.Type.Name.IndexOf("<>f__AnonymousType") ==0)
            {
                OOAdvantech.UserInterface.Runtime.UISession UISession = OOAdvantech.UserInterface.Runtime.UISession.CreateUserInterfaceSession(null);
                List<object> collection = new List<object>();
                foreach (object obj in objectCollection)
                    collection.Add(new ObjectSateProvider(reportDataSource.DataSourceType, obj, UISession, reportDataSource).GetTransparentProxy());
                return collection;
            }
            if (reportDataSource.Type == null)
                return new System.Collections.ArrayList();
            Type type = reportDataSource.Type.GetExtensionMetaObject(typeof(Type)) as Type;

            OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(objectCollection, type, rootObjectNode.Paths);

            PathNode objectNode = new PathNode("Root", null);
            if (structureSet != null)
            {
                OOAdvantech.UserInterface.Runtime.UISession UISession = OOAdvantech.UserInterface.Runtime.UISession.CreateUserInterfaceSession(null);
                UISession.LoadDisplayedValues(rootObjectNode, structureSet, reportDataSource.Type.GetExtensionMetaObject(typeof(Type)) as Type, -1);

                List<object> collection = new List<object>();
                foreach (object obj in objectCollection)
                    collection.Add(new ObjectSateProvider(reportDataSource.DataSourceType, obj, UISession, reportDataSource).GetTransparentProxy());
                return collection;

            }
            return null;
        }



        

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public bool CanCastTo(Type castType, object o)
        {
            if (castType == typeof(ObjectSateProvider))
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
        ~ObjectSateProvider()
        {

        }
        

        public object TheRealObject;
        Type Type;
        object DisplayedObj;
        OOAdvantech.UserInterface.Runtime.UISession UISession;
        ReportDataSource ReportDataSource;
        internal ObjectSateProvider(Type type, object obj, OOAdvantech.UserInterface.Runtime.UISession uiSession, ReportDataSource reportDataSource)
            : base(type)
        {
            ReportDataSource = reportDataSource;
            UISession = uiSession;
            DisplayedObj = obj;
            Type = type;
                TheRealObject = DisplayedObj;

        }


        static internal bool InInvoke = false;
        
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
                        string member = MethodMessage.MethodName.Substring(4);
                        OOAdvantech.UserInterface.Component component = ReportDataSource.GetMember(member);
                        if (component is OOAdvantech.UserInterface.ReportObjectDataSource.Member)
                        {
                            if (!string.IsNullOrEmpty((component as OOAdvantech.UserInterface.ReportObjectDataSource.Member).Path))
                            {
                                if ((component as OOAdvantech.UserInterface.ReportObjectDataSource.Member).MetaObject != null)
                                {
                                    member = (component as OOAdvantech.UserInterface.ReportObjectDataSource.Member).MetaObject.Name;
                                    member = member + "." + (component as OOAdvantech.UserInterface.ReportObjectDataSource.Member).Path;
                                }
                                else
                                    member = (component as OOAdvantech.UserInterface.ReportObjectDataSource.Member).Path;
                            }
                            OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = UISession.GetDisplayedValue(TheRealObject, TheRealObject.GetType(), member, null);
                            if (displayedValue != null)
                                ret = displayedValue.Value;
                        }
                        else
                        {

                            ReportDataSource detailReportDataSource = component as ReportDataSource;
                            if (!string.IsNullOrEmpty(detailReportDataSource.Path))
                            {
                                //member = member + "." + detailReportDataSource.Path;
                                if (detailReportDataSource.MetaObject != null)
                                {
                                    member = detailReportDataSource.MetaObject.Name;
                                    member = member + "." + detailReportDataSource.Path;
                                }
                                else
                                    member = detailReportDataSource.Path;

                            }
                            if (detailReportDataSource.IsCollection)
                            {
                                OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = UISession.GetDisplayedValue(TheRealObject, TheRealObject.GetType(), member, null);
                                System.Collections.ArrayList list = new System.Collections.ArrayList();
                                //if (displayedValue.Value != null)
                                //{
                                foreach (OOAdvantech.UserInterface.Runtime.DisplayedValue itemDisplayedValue in displayedValue.Members["Items"].ValuesCollection)
                                {
                                    object obj = itemDisplayedValue.Value;

                                    //foreach (object obj in displayedValue.Value as System.Collections.ICollection)
                                    //{
                                    list.Add(new ObjectSateProvider(detailReportDataSource.DataSourceType, obj, UISession, detailReportDataSource).GetTransparentProxy());
                                    //}
                                }
                                ret = list;
                            }
                            else
                            {
                                OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = UISession.GetDisplayedValue(TheRealObject, TheRealObject.GetType(), member, null);
                                if (displayedValue.Value != null)
                                    ret = new ObjectSateProvider(detailReportDataSource.DataSourceType, displayedValue.Value, UISession, detailReportDataSource).GetTransparentProxy();
                                else
                                    ret = null;

                            }




                        }

                        if (MethodMessage.MethodName == "get_Name" && !(ret is string))
                        {
                            int rtr = 0;
                        }


                    }
                    else
                    {

                    }


                }



                retMsg = new ReturnMessage(
                    ret,           //Object ret
                    outArgs,       //Object[] outArgs
                    0,             //int outArgsCount
                    null,          //LogicalCallContext callCtx
                    (IMethodCallMessage)msg   //IMethodCallMessage mcm
                    );
                return retMsg;
            }
            catch (System.Exception error)
            {
                throw;
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




        //internal static object GetObject(object _object)
        //{
        //    return (System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) as RecordProxy).TheRealObject;

        //}
    }

}
