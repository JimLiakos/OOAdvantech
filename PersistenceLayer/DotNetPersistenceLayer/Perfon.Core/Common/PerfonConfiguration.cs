using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfon.Core.Common
{
    /// <summary>
    /// Perfon configuration settings
    /// </summary>
    /// <MetaDataID>{ab3d2486-e9a8-4078-b097-8f31ba9b9641}</MetaDataID>
    public class PerfonConfiguration
    {
        public PerfonConfiguration()
        {
            EnablePerfApi = false;
            EnablePerfUIApi = false;

            EnableMonitoringCpuTime = true;

            DoNotStorePerfCountersIfReqLessOrEqThan = 0;

            MinPollPeriod = null;
            MaxPollPeriod = null;
        }

        /// <summary>
        /// Enables getting perf counters from Web Api controller using api/perfcounters
        /// Disabled by default.
        /// </summary>
        public bool EnablePerfApi { get; set; }
        /// <summary>
        /// Enables getting UI html page for view perf counters from Web Api controller using api/perfcountersui and api/perfcountersuipanel
        /// Disabled by default.
        /// </summary>        
        public bool EnablePerfUIApi { get; set; }

        /// <summary>
        /// Enable CPU time monitoring
        /// Enabled by default.
        /// </summary>
        public bool EnableMonitoringCpuTime { get; set; }

        /// <summary>
        /// Do not store perf counter poll results if count of requests is less or equal to this setting
        /// </summary>
        public int DoNotStorePerfCountersIfReqLessOrEqThan { get; set; }

        /// <summary>
        /// TODO: implement adaptive poll period
        /// </summary>
        public int? MinPollPeriod { get; set; }
        public int? MaxPollPeriod { get; set; }

    }
}
