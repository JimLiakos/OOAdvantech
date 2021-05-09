using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace System.Timers
{
    //
    // Summary:
    //     Represents the method that will handle the System.Timers.Timer.Elapsed event
    //     of a System.Timers.Timer.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     An System.Timers.ElapsedEventArgs object that contains the event data.
    public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);

    //
    // Summary:
    //     Provides data for the System.Timers.Timer.Elapsed event.
    /// <MetaDataID>{e865d77b-4cf5-4d0b-a84f-1deb2700f711}</MetaDataID>
    public class ElapsedEventArgs : EventArgs
    {
        //
        // Summary:
        //     Gets the time the System.Timers.Timer.Elapsed event was raised.
        //
        // Returns:
        //     The time the System.Timers.Timer.Elapsed event was raised.
        public DateTime SignalTime { get; internal set; }
    }


    public delegate void TimerCallback(object state);
    /// <MetaDataID>{8740f6a2-c054-49f2-a01b-2f18179e68cd}</MetaDataID>
    public class Timer:IDisposable
    {

        public event ElapsedEventHandler Elapsed;
        private TimeSpan Timespan;
        private readonly TimerCallback Callback;
        private readonly object State;
        
        private CancellationTokenSource cancellation= new CancellationTokenSource();

        public Timer(TimerCallback callback, object state, TimeSpan timespan)
        {
            this.Timespan = timespan;
            this.Callback = callback;
            this.State = state;
            
        }

        public Timer()
        {

        }
        
        public double Interval
        {
            get
            {
                return Timespan.TotalMilliseconds;
            }
            set
            {
                Timespan =TimeSpan.FromMilliseconds(value);
            }
        }

        public bool Enabled { get; internal set; }

        public void Start()
        {
            Enabled = true;
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(this.Timespan,
                () =>
                {
                    if (cts.IsCancellationRequested) return false;
                    
                    Callback?.Invoke(State);
                    Elapsed?.Invoke(this, new ElapsedEventArgs() { SignalTime = System.DateTime.Now });
                    return true; // or true for periodic behavior
                });
        }

        public void Stop()
        {
            Enabled = false;
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
