using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if DeviceDotNet
using Xamarin.Essentials;
#endif

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{7236168b-6c4f-4ebc-af06-16a900143cdc}</MetaDataID>
    public class Connectivity
    {

        private static Connectivity standarConnectivity = new Connectivity();

        static object ChannelConnectivitiesLock = new object();
        private static Dictionary<string, Connectivity> ChannelConnectivities = new Dictionary<string, Connectivity>();
        public static Connectivity GetConnectivity(string channelUri = null)
        {
            lock (ChannelConnectivitiesLock)
            {

                if (channelUri == null)
                    return standarConnectivity;

                if (!ChannelConnectivities.TryGetValue(channelUri, out Connectivity connectivity))
                {
                    connectivity = new Connectivity(channelUri);
                    ChannelConnectivities[channelUri] = connectivity;

                }
                return connectivity;
            }
        }

        event EventHandler<ConnectivityChangedEventArgs> _ConnectivityChanged;
        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged
        {
            add
            {
                if (_ConnectivityChanged == null)
                    startConectivityMonitoring();
                _ConnectivityChanged += value;
            }

            remove
            {

                _ConnectivityChanged -= value;
                if (_ConnectivityChanged == null)
                    StopConectivityMonitoring();

            }
        }



        private void StopConectivityMonitoring()
        {
#if DeviceDotNet
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            ConectivityMonitoringTimer.Stop();
#endif

        }

        System.Timers.Timer ConectivityMonitoringTimer = new System.Timers.Timer(3000);
        private string ChannelUri;
        private ComunicationChannelConectivity LastComunicationChannelConectivityState;

        public static NetworkAccess NetworkAccess
        {

            get
            {

#if DeviceDotNet
                return GetNetworkAccess(Xamarin.Essentials.Connectivity.NetworkAccess);
#else
                return NetworkAccess.Unknown;
#endif
            }
        }

        public static IEnumerable<ConnectionProfile> ConnectionProfiles
        {
            get
            {
#if DeviceDotNet
                return Xamarin.Essentials.Connectivity.ConnectionProfiles.Select(x => GetConnectionProfile(x));
#else
                return new List<ConnectionProfile>();
#endif
            }
        }

        public Connectivity(string channelUri)
        {
            this.ChannelUri = channelUri;
            ConectivityMonitoringTimer.Elapsed += ConectivityMonitoringTimer_Elapsed;

        }

        public ComunicationChannelConectivity ComunicationChannelConectivity
        {
            get
            {
                if (ChannelUri != null)
                {
                    var clientSessionPart = RenewalManager.GetSessions().Where(x => x.Key.StartsWith(ChannelUri)).Select(x => x.Value).FirstOrDefault();
                    if (clientSessionPart != null)
                    {
                        var channelState = (clientSessionPart as ClientSessionPart)?.ChannelState;
                        return new ComunicationChannelConectivity(NetworkAccess, (clientSessionPart as ClientSessionPart).ChannelState);
                    }
                    else
                        return new ComunicationChannelConectivity(NetworkAccess, ChannelState.None);
                }

                return new ComunicationChannelConectivity(NetworkAccess, ChannelState.None);
            }
        }

        public Connectivity()
        {
            ConectivityMonitoringTimer.Elapsed += ConectivityMonitoringTimer_Elapsed;
        }

        private void ConectivityMonitoringTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                ConectivityMonitoringTimer.Interval = 1000;
                var lastState = LastComunicationChannelConectivityState?.ChannelState;
                var currentState = ComunicationChannelConectivity.ChannelState;
                if (ChannelUri != null && LastComunicationChannelConectivityState?.ChannelState != ComunicationChannelConectivity.ChannelState)
                {
                    LastComunicationChannelConectivityState = this.ComunicationChannelConectivity;
                    _ConnectivityChanged?.Invoke(sender, new ConnectivityChangedEventArgs(NetworkAccess, ConnectionProfiles, LastComunicationChannelConectivityState));

                }


            }
            catch (Exception error)
            {

                throw;
            }
        }

        private void startConectivityMonitoring()
        {
#if DeviceDotNet
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
#endif
            if (ChannelUri != null)
            {
                LastComunicationChannelConectivityState = ComunicationChannelConectivity;
                if (LastComunicationChannelConectivityState.ChannelState == ChannelState.None)
                    LastComunicationChannelConectivityState = null;

                    ConectivityMonitoringTimer.Start();
            }


        }
#if DeviceDotNet
        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {

            List<ConnectionProfile> connectionProfiles = new List<ConnectionProfile>();
            foreach (var theConnectionProfile in e.ConnectionProfiles)
            {
                ConnectionProfile connectionProfile = ConnectionProfile.Unknown;
                connectionProfile = GetConnectionProfile(theConnectionProfile);
                connectionProfiles.Add(connectionProfile);
            }


            if (ChannelUri != null)
                _ConnectivityChanged?.Invoke(sender, new ConnectivityChangedEventArgs(GetNetworkAccess(e.NetworkAccess), connectionProfiles, this.ComunicationChannelConectivity));
            else
                _ConnectivityChanged?.Invoke(sender, new ConnectivityChangedEventArgs(GetNetworkAccess(e.NetworkAccess), connectionProfiles));
        }

        private static ConnectionProfile GetConnectionProfile(Xamarin.Essentials.ConnectionProfile theConnectionProfile)
        {
            ConnectionProfile connectionProfile;
            switch (theConnectionProfile)
            {
                case Xamarin.Essentials.ConnectionProfile.Unknown:
                    connectionProfile = ConnectionProfile.Unknown;
                    break;
                case Xamarin.Essentials.ConnectionProfile.Cellular:
                    connectionProfile = ConnectionProfile.Cellular;
                    break;
                case Xamarin.Essentials.ConnectionProfile.Bluetooth:
                    connectionProfile = ConnectionProfile.Bluetooth;
                    break;
                case Xamarin.Essentials.ConnectionProfile.WiFi:
                    connectionProfile = ConnectionProfile.WiFi;
                    break;
                case Xamarin.Essentials.ConnectionProfile.Ethernet:
                    connectionProfile = ConnectionProfile.Ethernet;
                    break;
                default:
                    connectionProfile = ConnectionProfile.Unknown;
                    break;
            }
            return connectionProfile;
        }

        private static NetworkAccess GetNetworkAccess(Xamarin.Essentials.NetworkAccess theNetworkAccess)
        {
            NetworkAccess networkAccess;
            switch (theNetworkAccess)
            {
                case Xamarin.Essentials.NetworkAccess.Unknown:
                    networkAccess = NetworkAccess.Unknown;
                    break;
                case Xamarin.Essentials.NetworkAccess.Local:
                    networkAccess = NetworkAccess.Local;
                    break;
                case Xamarin.Essentials.NetworkAccess.None:
                    networkAccess = NetworkAccess.None;
                    break;
                case Xamarin.Essentials.NetworkAccess.ConstrainedInternet:
                    networkAccess = NetworkAccess.ConstrainedInternet;
                    break;
                case Xamarin.Essentials.NetworkAccess.Internet:
                    networkAccess = NetworkAccess.Internet;
                    break;
                default:
                    networkAccess = NetworkAccess.Unknown;
                    break;
            }
            return networkAccess;
        }
#endif
#if !DeviceDotNet
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
#endif

    }

    /// <MetaDataID>{573f8585-0e11-42b5-afb6-06b842db2e82}</MetaDataID>
    public class ConnectivityChangedEventArgs : EventArgs
    {
        public ConnectivityChangedEventArgs(NetworkAccess access, IEnumerable<ConnectionProfile> connectionProfiles, ComunicationChannelConectivity comunicationChannelConectivity = null)
        {
            NetworkAccess = access;
            ConnectionProfiles = connectionProfiles;
            ComunicationChannelConectivity = comunicationChannelConectivity;
        }



        public ComunicationChannelConectivity ComunicationChannelConectivity { get; }


        public NetworkAccess NetworkAccess { get; }

        public IEnumerable<ConnectionProfile> ConnectionProfiles { get; }

        public override string ToString() =>
            $"{nameof(NetworkAccess)}: {NetworkAccess}, " +
            $"{nameof(ConnectionProfiles)}: [{string.Join(", ", ConnectionProfiles)}]";
    }

    /// <MetaDataID>{84ae0d88-02ba-4a41-8466-e01b79039752}</MetaDataID>
    public enum ConnectionProfile
    {
        //
        // Summary:
        //     Other unknown type of connection.
        Unknown,
        //
        // Summary:
        //     The bluetooth data connection.
        Bluetooth,
        //
        // Summary:
        //     The mobile/cellular data connection.
        Cellular,
        //
        // Summary:
        //     The ethernet data connection.
        Ethernet,
        //
        // Summary:
        //     The WiFi data connection.
        WiFi
    }


    /// <MetaDataID>{86508f81-bd41-4750-a852-b43cfdd21856}</MetaDataID>
    public enum NetworkAccess
    {
        //
        // Summary:
        //     The state of the connectivity is not known.
        Unknown,
        //
        // Summary:
        //     No connectivity.
        None,
        //
        // Summary:
        //     Local network access only.
        Local,
        //
        // Summary:
        //     Limited internet access.
        ConstrainedInternet,
        //
        // Summary:
        //     Local and Internet access.
        Internet
    }
    /// <MetaDataID>{195f0eb6-0e32-4242-8657-9b68266fee67}</MetaDataID>
    public class ComunicationChannelConectivity
    {
        public ComunicationChannelConectivity(NetworkAccess networkAccess, ChannelState channelState)
        {
            NetworkAccess = networkAccess;
            ChannelState = channelState;
        }

        public NetworkAccess NetworkAccess { get; } = NetworkAccess.Unknown;

        public ChannelState ChannelState { get; }
    }

}
