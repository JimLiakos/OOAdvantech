using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Perfon.Core;
using Perfon.Core.Common;
using Perfon.Core.PerfCounterStorages;
using Perfon.Interfaces.Common;
using Perfon.Interfaces.PerfCounterStorage;


namespace OOAdvantech.Remoting.RestApi
{
    /// <summary>
    ///     /// Wrapper on PerfMonitor for Web Api.
    ///     /// It registers filters and handlers thus tracking of request counters.
    ///     /// </summary>
    /// <MetaDataID>{24d9c9e1-8a0d-490d-8c81-a9095535a99d}</MetaDataID>
    public class PerfMonitorForWebApi
    {
        /// <MetaDataID>{4a222eec-1dfd-4742-85ab-09ebc8ebf724}</MetaDataID>
        public PerfMonitor PerfMonitorBase { get; private set; }

        /// <summary>
        ///         /// Settings for Perfon engine
        ///         /// </summary>
        /// <MetaDataID>{54f68b8a-c065-4344-b5ee-b85152b01935}</MetaDataID>
        public PerfonConfiguration Configuration
        {
            get
            {
                return PerfMonitorBase.Configuration;
            }
        }

        /// <summary>
        /// Reports about errors and exceptions occured.
        /// </summary>
        public event EventHandler<IPerfonErrorEventArgs> OnError;


        /// <MetaDataID>{89c8f1e6-d1dc-4b20-bacc-765357c819ec}</MetaDataID>
        public PerfMonitorForWebApi()
        {
            PerfMonitorBase = new PerfMonitor();

            //Bubble up errors
            PerfMonitorBase.OnError += (a, b) =>
            {
                if (OnError != null)
                {
                    OnError(a, b);
                }
            };
        }

        /// <summary>
        ///         /// Register perf counter storages
        ///         /// </summary>
        /// <param name="storage"></param>
        /// <MetaDataID>{7215ee1a-dbb4-4ee1-b2c2-db4bb6492dd2}</MetaDataID>
        public void RegisterStorages(params IPerfomanceCountersStorage[] storage)
        {
            Storage = PerfMonitorBase.RegisterStorages(storage);
        }
        /// <summary>
        ///         /// Easy register some default perf counter storages implemented in the lib, if needed
        ///         /// </summary>
        /// <param name="dbPath"></param>
        /// <MetaDataID>{654a6181-4748-4ad8-a30e-e40ce9945f25}</MetaDataID>
        public void RegisterCSVFileStorage(string dbPath)
        {
            Storage = PerfMonitorBase.RegisterCSVFileStorage(dbPath);
        }
        /// <MetaDataID>{ef9b54cc-d1bd-4ddf-85ff-53be056a1760}</MetaDataID>
        public void RegisterLiteDbStorage(string dbPath)
        {
            Storage = PerfMonitorBase.RegisterLiteDbStorage(dbPath);
        }
        /// <MetaDataID>{4fb89f83-adce-473d-adb5-ef7a8fc0a150}</MetaDataID>
        public void RegisterInMemoryCacheStorage(long expirationInSeconds = 60 * 60)
        {
            Storage = PerfMonitorBase.RegisterInMemoryCacheStorage(expirationInSeconds);
        }
        /// <summary>
        ///         /// Start polling and saving perf counters. Period is in sec
        ///         /// </summary>
        /// <param name="pollPeriod_sec">Poll period, ms</param>
        /// <MetaDataID>{c30c9b97-518e-4b90-9190-06e6658ff39f}</MetaDataID>
        public void Start(ContextMessageDispatcher contextMessageDispatcher, int pollPeriod_sec)
        {
            PerfMonitorBase.Start(pollPeriod_sec);

            //cfg = httpConfiguration;

            //httpConfiguration.Filters.Add(new ExceptionCounterFilter(this.PerfMonitorBase));
            
            contextMessageDispatcher.RegisterInterceptor(new RequestPerfMonitorMessageHandler(this.PerfMonitorBase));
            

            ////httpConfiguration.Services.Add(typeof(PerfMonitorForWebApi), this);

            //httpConfiguration.Properties[EnumKeyNames.PerfMonitorLib.ToString()] = this;

            //httpConfiguration.MapHttpAttributeRoutes();
        }
        /// <summary>
        ///         /// Stops perf counters polling
        ///         /// </summary>
        /// <MetaDataID>{4a0f92f4-51e6-4cf9-877d-cd774ec85e3b}</MetaDataID>
        public void Stop()
        {
            PerfMonitorBase.Stop();

            //if (cfg != null)
            //{
            //    cfg.Properties[EnumKeyNames.PerfMonitorLib.ToString()] = null;
            //}
        }


        /// <MetaDataID>{4e91be81-8fa6-4de5-adbd-136006f2d85c}</MetaDataID>
        public string UIPage
        {
            get
            {
                return PerfMonitorBase.UIPage.Value;
            }
        }
        /// <MetaDataID>{ba8a8d52-ae6d-4066-a9b3-bd02e3da155a}</MetaDataID>
        public string UIPanel
        {
            get
            {
                return PerfMonitorBase.UIPanel;
            }
        }


        /// <summary>
        ///         /// Need to be removed
        ///         /// Inject it into controller!
        ///         /// </summary>
        /// <MetaDataID>{e06196da-3b75-461e-a26f-879a427c57b5}</MetaDataID>
        internal IPerfomanceCountersStorage Storage { get; private set; }


        /// Get counter history track for specified date
        /// Skip is used for periodic polling, allowing to get only recent values not recieved yet
        /// Awaitable
        /// </summary>
        /// <param name="counterName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public Task<IEnumerable<IPerfCounterValue>> QueryCounterValues(string counterName, DateTime? date = null, int skip = 0, string appId = null)
        {
            if (Storage != null)
            {
                return Storage.QueryCounterValues(counterName, date, skip, appId);
            }

            return null;
        }



        /// <MetaDataID>{705cd7f0-a206-4351-b2c7-f42cd2030917}</MetaDataID>
      //  private HttpConfiguration cfg = null;
    }

    /// <summary>
    ///     /// Web Api MessageHandler for tracking number of requests and request processing time.
    ///     /// </summary>
    /// <MetaDataID>{38337db4-bae7-4bc1-89b5-380061b44ef2}</MetaDataID>
    public class RequestPerfMonitorMessageHandler: IMessageDispatcherInterceptor
    {
        /// <summary>
        /// Core PerfMon object managing perfCounters
        /// </summary>
        private PerfMonitor PerfMonitor { get; set; }

        IMessageDispatcherInterceptor _Next;
        public IMessageDispatcherInterceptor Next
        {
            get
            {
                return _Next;
            }

            set
            {
                _Next = value;
            }
        }

        public RequestPerfMonitorMessageHandler(PerfMonitor perfMonitor)
        {
            PerfMonitor = perfMonitor;
        }

        public ResponseData DispatchMessage(RequestData request)
        {
            var st = Stopwatch.StartNew();

            PerfMonitor.RequestNum.Increment();

            var res = Next.DispatchMessage(request);
            
            long lenReq = 0;
            if (request.details != null)
            {
                lenReq += request.details.Length;
            }
            PerfMonitor.BytesTrasmittedReq.Add(lenReq);

            /*
            SO: Don't be concerned by the LoadIntoBufferAsync, unless you actually do streaming content then almost all content is 
             * buffered by the host anyway, so doing it a little earlier in the pipeline will not add any extra overhead.
            */
            long lenResp = res.details.Length;
            
            PerfMonitor.BytesTrasmittedResp.Add(lenResp);

            st.Stop();

            PerfMonitor.RequestProcessTime.Add(st.ElapsedMilliseconds);
            PerfMonitor.RequestMaxProcessTime.Add(st.ElapsedMilliseconds);


            if (!res.IsSucceeded)
            {
                PerfMonitor.BadStatusNum.Increment();
            }

            return res;
        }

        //protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var st = Stopwatch.StartNew();

        //    PerfMonitor.RequestNum.Increment();

        //    var res = await base.SendAsync(request, cancellationToken);

        //    long lenReq = 0;
        //    if (request.Content != null)
        //    {
        //        if (request.Content.Headers.ContentLength.HasValue)
        //        {
        //            lenReq = request.Content.Headers.ContentLength.Value;
        //        }
        //        lenReq += request.Content.Headers.ToString().Length;
        //    }
        //    lenReq += request.RequestUri.OriginalString.Length;
        //    lenReq += request.Headers.ToString().Length;
        //    PerfMonitor.BytesTrasmittedReq.Add(lenReq);

        //    /*
        //    SO: Don't be concerned by the LoadIntoBufferAsync, unless you actually do streaming content then almost all content is 
        //     * buffered by the host anyway, so doing it a little earlier in the pipeline will not add any extra overhead.
        //    */
        //    long lenResp = 0;
        //    if (res.Content != null)
        //    {
        //        await res.Content.LoadIntoBufferAsync();
        //        if (res.Content.Headers.ContentLength.HasValue)
        //        {
        //            lenResp = res.Content.Headers.ContentLength.Value;
        //        }
        //        lenResp += res.Content.Headers.ToString().Length;
        //    }
        //    lenResp += res.Headers.ToString().Length;
        //    PerfMonitor.BytesTrasmittedResp.Add(lenResp);

        //    st.Stop();

        //    PerfMonitor.RequestProcessTime.Add(st.ElapsedMilliseconds);
        //    PerfMonitor.RequestMaxProcessTime.Add(st.ElapsedMilliseconds);


        //    if (!res.IsSuccessStatusCode)
        //    {
        //        PerfMonitor.BadStatusNum.Increment();
        //    }

        //    return res;
        //}

    }
}





