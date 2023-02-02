using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using Perfon.Core.Notifications;
using Perfon.Interfaces.PerfCounterStorage;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{97032b2c-a5c5-4e0b-8140-1b6e90105f63}</MetaDataID>
    public sealed class IsolatedContext : IDisposable
    {

        public static string CurrentContextID;
        /// <MetaDataID>{31673c74-735b-4633-a21b-aafa1700f523}</MetaDataID>
        static Dictionary<string, IsolatedContext> IsoletedContexts = new Dictionary<string, IsolatedContext>();

        /// <MetaDataID>{6b10d3e7-b378-4791-9898-a3762f6adce4}</MetaDataID>
        private AppDomain AppDomain;

        /// <exclude>Excluded</exclude> 
        private ContextMessageDispatcher _value;


        /// <MetaDataID>{d3d173bd-7d16-4647-b30e-d6861144b1b4}</MetaDataID>
        public static void AssignAppDomain(string contextID, AppDomain appDomain)
        {
            var appDomainIsoletedContext = IsoletedContexts.Where(x => x.Value.AppDomain == appDomain).Select(X => X.Value).FirstOrDefault();
            if (appDomainIsoletedContext == null)
                IsoletedContexts[contextID] = new IsolatedContext(contextID, appDomain);
            else
                IsoletedContexts[contextID] = appDomainIsoletedContext;

        }
        /// <MetaDataID>{673d86b9-63f5-414c-ab19-185c9a345c7a}</MetaDataID>
        public IsolatedContext(string contextID, AppDomain appDomain)
        {
            AppDomain = appDomain;// AppDomain.CreateDomain("Isolated:" + contextID,             null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(ContextMessageDispatcher);

            _value = (ContextMessageDispatcher)AppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);


            _value.StartUp(AppDomainInitializeType);

            _value.ContextID = contextID;

        }

        /// <MetaDataID>{ec50eb3d-0292-42c8-b3e2-cfd0cb693db9}</MetaDataID>
        static internal IsolatedContext GetIsolatedContext(string contextID)
        {
            lock (IsoletedContexts)
            {
                IsolatedContext isolatedContext = null;
                if (!IsoletedContexts.TryGetValue(contextID, out isolatedContext))
                {
                    isolatedContext = new IsolatedContext(contextID);
                    IsoletedContexts[contextID] = isolatedContext;
                }
                return isolatedContext;
            }
        }

        /// <MetaDataID>{18d267df-71d0-46b0-8c83-3cc876bd8ce3}</MetaDataID>
        string ContextID;

        ///// <MetaDataID>{6f256a3c-cfb5-43d8-a623-11f93c8bcdd4}</MetaDataID>
        //public static void Dispose(string contextID)
        //{
        //    IsolatedContext isolatedContext = null;
        //    if (IsoletedContexts.TryGetValue(contextID, out isolatedContext))
        //        isolatedContext.Dispose();
        //}

        /// <MetaDataID>{ab8d8121-c89f-4364-84fd-e0921be3c201}</MetaDataID>
        public static Type AppDomainInitializeType;

        /// <MetaDataID>{eb9a167d-219c-4256-ba83-0e30a3507a93}</MetaDataID>
        public IsolatedContext(string contextID)
        {
            ContextID = contextID;
            AppDomain = AppDomain.CreateDomain("Isolated:" + contextID,
               null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(ContextMessageDispatcher);

            _value = ((ContextMessageDispatcher)AppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName)).CurrentContextMessageDispatcher;
         

            _value.ContextID = contextID;
            _value.StartUp(AppDomainInitializeType);
            System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel IsolatedContext launch {0}", ContextID));
        }

        /// <exclude>Excluded</exclude>
        EventHandler _IsolatedContextUnloaded;

        public event EventHandler IsolatedContextUnloaded
        {
            add
            {
                
                
                _IsolatedContextUnloaded += value;

                int count = 0;
                if (_IsolatedContextUnloaded != null && _IsolatedContextUnloaded.GetInvocationList() != null)
                    count = _IsolatedContextUnloaded.GetInvocationList().Length;
                else if (_IsolatedContextUnloaded != null)
                    count = 1;


            }
            remove
            {
                _IsolatedContextUnloaded -= value;

                int count = 0;
                if (_IsolatedContextUnloaded != null && _IsolatedContextUnloaded.GetInvocationList() != null)
                    count = _IsolatedContextUnloaded.GetInvocationList().Length;
                else if (_IsolatedContextUnloaded != null)
                    count = 1;

            } 
        }
        /// <MetaDataID>{f4e2b0cd-337e-4b63-b5ca-2b9aed8ffe10}</MetaDataID>
        public static ResponseData DispatchMessage(RequestData request)
        {
            try
            {
                
                if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
                {
                    if (request.InternalChannelUri == "055980081b674aec9e774e8403cdc972")
                    {
                    }
                    if (request.InternalChannelUri == "7f9bde62e6da45dc8c5661ee2220a7b0")
                    {
                    }
                    var responseData = IsolatedContext.GetIsolatedContext(request.InternalChannelUri).InternalDispatchMessage(request);
                    responseData.BidirectionalChannel = true;
                    return responseData;

                }
                else
                {
                    var responseData = MessageDispatcher.MessageDispatch(request);
                    responseData.BidirectionalChannel = true;
                    return responseData;
                }

            }
            catch (Exception error)
            {

                throw;
            }
        }


        /// <MetaDataID>{e96e8702-c1bf-496b-8d64-cd4aee43bd6e}</MetaDataID>
        ResponseData InternalDispatchMessage(RequestData request)
        {

            //WebSocketServer = request.EventCallBackChannel as WebSocketServer;

            try
            {
                
                return _value.DispatchMessage(request);
            }
            catch (Exception error)
            {
                if (error.HResult == -2146233077)
                {
                    Type type = typeof(ContextMessageDispatcher);
                    _value = ((ContextMessageDispatcher)AppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName)).CurrentContextMessageDispatcher;

                 

                    return _value.DispatchMessage(request);

                }
                else
                    throw;


            }
        }

        /// <MetaDataID>{960661c4-072f-4462-936e-4d7c7a8230db}</MetaDataID>
        ContextMessageDispatcher ContextMessageDispatcher
        {
            get
            {
                return _value;
            }
        }

        


        /// <MetaDataID>{e87a6298-240f-4206-b03f-e22c7c8d4b99}</MetaDataID>
        public void Dispose()
        {
            var datetime = DateTime.Now;
            string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
            System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel IsolatedContext UnloadIsolatedContext {0} ", timestamp));

            _IsolatedContextUnloaded?.Invoke(this, EventArgs.Empty);
            lock (this)
            {
                if (AppDomain != null)
                {
                    ContextMessageDispatcher.CurrentContextMessageDispatcher.Stop();
                    //long asds = _domain.MonitoringTotalAllocatedMemorySize;
                    AppDomain.Unload(AppDomain);
                    AppDomain = null;
                }
            }
            System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel IsolatedContext UnloadIsolatedContext {0}", ContextID));
            
        }

        /// <MetaDataID>{77a4ab5c-08b5-4b46-8608-552084ea1256}</MetaDataID>
        public static void UnloadIsolatedContext(string contextID)
        {
            lock (IsoletedContexts)
            {
                IsolatedContext isolatedContext = null;
                if (IsoletedContexts.TryGetValue(contextID, out isolatedContext))
                {
                    IsoletedContexts.Remove(contextID);
                    isolatedContext.Dispose();
                }
            }
        }
    }

    /// <MetaDataID>{8f7292f1-392b-495f-8ff0-10b5af863ec7}</MetaDataID>
    public class ContextMessageDispatcher : MarshalByRefObject, IMessageDispatcherInterceptor
    {

        /// <MetaDataID>{82bebd0b-408b-48a9-8549-956f796239f2}</MetaDataID>
        static ContextMessageDispatcher()
        {
            //try
            //{
            //    System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromMinutes(20);
            //    System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(10);
            //    System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromMinutes(10);/**/

            //}
            //catch (Exception error)
            //{
            //}
        }
        /// <MetaDataID>{763ee256-329e-4410-ac64-33e46bdae5ce}</MetaDataID>
        public ContextMessageDispatcher()
        {
            if (_CurrentContextMessageDispatcher == null)
            {
                _CurrentContextMessageDispatcher = this;
                MessageDispatcherInterceptor = this;
            }

        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(20);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(10);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(10);
            }
            return lease;
        }

        internal static ContextMessageDispatcher Current
        {
            get
            {
                return _CurrentContextMessageDispatcher;
            }
        }

        /// <MetaDataID>{84b0815c-0551-4716-96d5-d5b716e1adc3}</MetaDataID>
        static ContextMessageDispatcher _CurrentContextMessageDispatcher;
        /// <MetaDataID>{29b68c97-f447-411c-8315-f9ffe320222f}</MetaDataID>
        public ContextMessageDispatcher CurrentContextMessageDispatcher
        {
            get
            {
                return _CurrentContextMessageDispatcher;
            }
        }

        /// <MetaDataID>{d16442e8-3bb9-4ba8-83e4-bbae374ad199}</MetaDataID>
        string _ContextID;
        /// <MetaDataID>{decdfd60-da70-47d7-a462-214f850aae15}</MetaDataID>
        public string ContextID
        {
            get
            {
                return _ContextID;
            }
            set
            {
                _ContextID = value;
            }
        }

        /// <MetaDataID>{b52f3eef-53f8-4ffa-b120-341c30db899f}</MetaDataID>
        IMessageDispatcherInterceptor IMessageDispatcherInterceptor.Next
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        /// <MetaDataID>{5b902418-6634-4934-a424-c98c6dbf0d32}</MetaDataID>
        ResponseData IMessageDispatcherInterceptor.DispatchMessage(RequestData request)
        {
            try
            {
                return MessageDispatcher.MessageDispatch(request);
            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{2dff26a0-15fe-40c0-8172-8cc2625fa8c4}</MetaDataID>
        IMessageDispatcherInterceptor MessageDispatcherInterceptor;

        /// <MetaDataID>{5bb93103-7115-45b8-9079-084b34fbc54b}</MetaDataID>
        WebSocketServer WebSocketServer;
        /// <MetaDataID>{4c16fcf7-992b-4e6d-93a3-959b31386487}</MetaDataID>
        public ResponseData DispatchMessage(RequestData request)
        {
            WebSocketServer = request.EventCallBackChannel as WebSocketServer;

            //...
            return CurrentContextMessageDispatcher.MessageDispatcherInterceptor.DispatchMessage(request);
        }
        /// <MetaDataID>{8574469b-b77c-4898-83b3-bbbc04336637}</MetaDataID>
        protected PerfMonitorForWebApi PerfMonitor { get; set; }
        /// <MetaDataID>{36892500-5bf6-42b4-b57e-d303f8a6fb06}</MetaDataID>
        public void StartUp(Type type)
        {

            if (CurrentContextMessageDispatcher == this)
            {
                

                if (type != null)
                    (Activator.CreateInstance(type) as IAppDomainInitializer).OnStart(ContextID);

                PerfMonitor = new PerfMonitorForWebApi();
                // Register storage
                var storageType = ConfigurationManager.AppSettings["StorageType"];
                var storageConnString = ConfigurationManager.AppSettings["StorageConnectionString"];
                if (storageType == null || storageType == null)
                {
                    ////PerfMonitor.RegisterCSVFileStorage(AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["DB_Path"]);
                    //PerfMonitor.RegisterInMemoryCacheStorage(60);
                    //PerfMonitor.RegisterLiteDbStorage(AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["DB_Path"]);
                }
                else
                {
                    Type sType = Type.GetType(storageType, true);
                    var storage = (Activator.CreateInstance(sType, storageConnString)) as IPerfomanceCountersStorage;
                    if (type == null || storage == null)
                    {
                        PerfMonitor_OnError(this, new Perfon.Core.Common.PerfonErrorEventArgs(type == null ? "null" : sType.ToString() + ", " + storage == null ? "null" : storage.ToString()));
                    }
                    else
                    {
                        PerfMonitor.RegisterStorages(storage);
                    }
                }

                //Subscribe on error events
                PerfMonitor.OnError += PerfMonitor_OnError;

                //Create and subscribe on perf counter value threshold violation events
                var thr1 = new ThresholdMaxNotification(500);
                thr1.OnThresholdViolated += (a, b) => Console.WriteLine(b.Message);
                thr1.OnThresholdViolationRecovered += (a, b) => Console.WriteLine(b.Message);
                PerfMonitor.PerfMonitorBase.RequestNum.AddThreshold(thr1);

                //Change some default settings if needed
                PerfMonitor.Configuration.DoNotStorePerfCountersIfReqLessOrEqThan = 0; //Do not store perf values if RequestsNum = 0 during poll period
                PerfMonitor.Configuration.EnablePerfApi = true; // Enable getting perf values by API GET addresses 'api/perfcounters' and  'api/perfcounters/{name}'
                PerfMonitor.Configuration.EnablePerfUIApi = true; // Enable getting UI html page with perf counters values by API GET 'api/perfcountersui' or 'api/perfcountersuipanel'

                //Register Windows Perf Counters if neede
                //PerfMonitor.PerfMonitorBase.AddWindowsPerfCounter("% Processor Time", "_Total");

                // Start counters polling
                //PerfMonitor.Start(this, 5);
            }
            else
                CurrentContextMessageDispatcher.StartUp(type);




        }
        /// <MetaDataID>{56ed5c40-181e-464a-9145-3b6971adbd28}</MetaDataID>
        void PerfMonitor_OnError(object sender, Perfon.Interfaces.Common.IPerfonErrorEventArgs e)
        {
            // File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\errors.log", "\n" + DateTime.Now.ToString() + " " + e.Message);
            Console.WriteLine("PerfLibForWebApi:" + e.Message);
        }

        /// <MetaDataID>{cfd51470-d704-4d2f-8050-7057534ceb51}</MetaDataID>
        internal void RegisterInterceptor(IMessageDispatcherInterceptor Interceptor)
        {
            Interceptor.Next = CurrentContextMessageDispatcher.MessageDispatcherInterceptor;
            CurrentContextMessageDispatcher.MessageDispatcherInterceptor = Interceptor;
        }

        /// <MetaDataID>{afae0634-3cf4-4d18-94a9-aec23631380a}</MetaDataID>
        public void Stop()
        {
            if (CurrentContextMessageDispatcher != this)
                CurrentContextMessageDispatcher.Stop();
            else
            {
                if (PerfMonitor != null)
                    PerfMonitor.Stop();
            }
        }
    }


    /// <MetaDataID>{2cb184ca-4222-4d35-af51-f3366a128c84}</MetaDataID>
    public interface IMessageDispatcherInterceptor
    {
        /// <MetaDataID>{1888926b-6534-4b87-bd98-7f9522792aec}</MetaDataID>
        IMessageDispatcherInterceptor Next { get; set; }

        /// <MetaDataID>{9d36be01-35d5-436b-b989-ac9fd5120268}</MetaDataID>
        ResponseData DispatchMessage(RequestData request);
    }

    /// <MetaDataID>{f7e2053b-6a98-4cb3-ac0a-f8952c9a6d0d}</MetaDataID>
    public interface IAppDomainInitializer
    {
        /// <MetaDataID>{0020c284-b9a3-463e-be8d-2abdce04dca4}</MetaDataID>
        bool OnStart(string contextID);
    }

}
