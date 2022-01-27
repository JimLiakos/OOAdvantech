using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech
{


    /// <MetaDataID>{cfa63701-40aa-4507-824c-30b1d5bc0383}</MetaDataID>
    public interface IDeviceOOAdvantechCore
    {
        
        void Signin(AuthProvider provider);
        void SignOut();
        

        bool IsinSleepMode { get; set; }

      
      
        event MessageReceivedHandle MessageReceived;

        event KeyboardChangeStateHandle KeyboordChangeState;

        /// <MetaDataID>{d03f1e4b-d600-4ac2-b65a-5b2cda8343e0}</MetaDataID>
        double ScreeHeight { get; }
        /// <MetaDataID>{39a262f4-a046-4f6f-9474-1fd9aa5068f2}</MetaDataID>
        double ScreeWidth { get; }
        /// <MetaDataID>{6113173f-8a0e-4e96-97ca-e768d6564215}</MetaDataID>
        IList<SIMCardData> LinesPhoneNumbers { get; }
        /// <MetaDataID>{2b23f86a-f073-4eba-9f42-ec6a32d8d068}</MetaDataID>
        SIMCardData GetLinePhoneNumber(int lineIndex);

        /// <MetaDataID>{0b5147e0-7478-44ce-ba51-42ed65d5da6c}</MetaDataID>
        IReadOnlyList<SimCard> GetSimCards();


        /// <MetaDataID>{4565051e-59de-425c-91e9-99e493ca4c59}</MetaDataID>
        String DeviceID { get; }


        string FirebaseToken { get; }


        void PlaySound();
        Task<string> EmailSignUp(string email, string password);
        Task<string> EmailSignIn(string email, string password);
        void SendPasswordResetEmail(string email);

        bool IsBackgroundServiceStarted { get; }

        bool RunInBackground(Action action, BackgroundServiceState serviceState);

        void StopBackgroundService();
    }

    /// <MetaDataID>{a0246554-4359-4a5a-a501-b3a7824c6b12}</MetaDataID>
    public enum AuthProvider
    {
        Google,
        Facebook
    }
    /// <MetaDataID>{f5676dc6-a065-4d78-a837-912cb449bed1}</MetaDataID>
    public interface IBackgroundService
    {

        

        bool IsServiceStarted { get; }
        bool Run(Action action, BackgroundServiceState serviceState);

        void Stop();

    }
    public delegate void MessageReceivedHandle(IRemoteMessage message);

    


    /// <MetaDataID>{65201611-2f61-4ab2-9aa3-ab811be6ea62}</MetaDataID>
    public interface IRemoteMessage
    {
        IDictionary<string, string> Data { get; }
        string From { get; }
        string MessageId { get; }
        string MessageType { get; }
        DateTime SentTime { get; }
        string To { get; }
    }

    /// <MetaDataID>{d2445458-7d5d-4cc1-ae85-45143f5d2944}</MetaDataID>
    [BackwardCompatibilityID("{d2445458-7d5d-4cc1-ae85-45143f5d2944}")]
    [Persistent()]
    public struct SIMCardData
    {
        /// <MetaDataID>{f948c57e-dcec-498d-a329-1955cecf047a}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        public string SIMCardIdentity;
        /// <MetaDataID>{63f275ba-64f5-468b-afcd-df3039c6390f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        public string SIMCardPhoneNumber;
        /// <MetaDataID>{89a814c5-8f76-4560-aff3-d9e29ba840d2}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+3")]
        public string SIMCardDescription;


        /// <MetaDataID>{84114d05-0b5c-4bd0-a679-216247429441}</MetaDataID>
        public static bool operator ==(SIMCardData left, SIMCardData right)
        {
            if (left.SIMCardIdentity == right.SIMCardIdentity)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{d155a02e-5578-4b15-a7f3-ae939974ac45}</MetaDataID>
        public static bool operator !=(SIMCardData left, SIMCardData right)
        {
            return !(left == right);
        }
    }


    /// <MetaDataID>{03a7d040-8212-41b3-89cf-1b7654f36831}</MetaDataID>
    public class SimCard
    {
        /// <MetaDataID>{5b92dd73-58c1-4bc9-a587-6a8f525da226}</MetaDataID>
        public SimCard()
        {
            MSISDN = new List<string>();
        }

        /// <summary>
        ///     Integrated Circuit Card ID.
        /// </summary>
        /// <remarks>Integrated Circuit Card Identifier identifies each SIM internationally.</remarks>
        /// <MetaDataID>{46026447-173f-4934-9489-3d569ae375c7}</MetaDataID>
        public string ICCID { get; set; }

        /// <summary>
        ///     Mobile Country Code
        /// </summary>
        /// <remarks>Identifies the country of origin by a two- or three-digit code.</remarks>
        /// <MetaDataID>{d779fb1f-9949-4180-9d2d-f5cacdb9d348}</MetaDataID>
        public string MCC { get; set; }

        /// <summary>
        ///     International Mobile Subscriber Identity.
        /// </summary>
        /// <remarks>
        ///     International mobile Subscriber Identity is used to identify the user of a cellular network and is a unique
        ///     identification associated with all cellular networks. It is stored as a 64 bit field and is sent by the phone to
        ///     the network. It is also used for acquiring other details of the mobile in the HLR or as locally copied in the
        ///     Visitor Location Register. It consists of three parts:
        ///     - Mobile Country Code(MCC) : First 3 digits of IMSI gives you MCC.
        ///     - Mobile Network Code (MNC) : Next 2 or 3 digits give you this info.
        ///     - Mobile Station ID (MSID) : Rest of the digits. Gives away the network you are using like IS-95, TDMA , GSM etc.
        /// </remarks>
        /// <MetaDataID>{a5920c0a-45c3-416a-bd89-028c92b2895f}</MetaDataID>
        public string IMSI { get; set; }

        /// <MetaDataID>{c9d308bd-e8fc-4d78-afa9-a8437d025319}</MetaDataID>
        public string IMEI { get; set; }

        /// <MetaDataID>{a882eeda-73ab-41f4-9355-32d74876b907}</MetaDataID>
        public string NetworkOperatorName;



        /// <summary>
        ///     Mobile Station ID.
        /// </summary>
        /// <remarks>The network you are using like IS-95, TDMA , GSM etc.</remarks>
        /// <MetaDataID>{0ec38620-8d48-4976-bfee-754c609e3249}</MetaDataID>
        public string MSID { get; set; }

        /// <summary>
        ///     Mobile Network Code.
        /// </summary>
        /// <MetaDataID>{2a05fe49-8cde-4d03-9b13-d17e07f7b074}</MetaDataID>
        public string MNC { get; set; }

        /// <summary>
        ///     Mobile Subscriber International ISDN Number.
        /// </summary>
        /// <remarks>
        ///     Mobile Station ISDN number is the full phone number of a subscriber, including the national country code (e.g.
        ///     1 for US, 44 for UK, etc.). The purpose of the MSISDN is simply to allow a device to be called. A subscriber can
        ///     have multiple MSISDNs (e.g. one phone number for business, one for personal calls, one for fax, etc.), but
        ///     generally only one IMSI. The MSISDN does not need to be stored on the SIM card. In cases where it is stored on the
        ///     SIM, the main reason is so that the user can use check to see what their own MSISDN is (in case they forget). The
        ///     MSISDN is never signaled to of from the device.
        /// </remarks>
        /// <MetaDataID>{1c0669e8-4aa9-471f-b7f2-2e96e48d0269}</MetaDataID>
        public IReadOnlyList<string> MSISDN { get; set; }
        /// <MetaDataID>{d90b4af5-0db4-429a-b693-93089e324cf1}</MetaDataID>
        public string PhoneNumber { get; set; }
    }


    /// <MetaDataID>{cc636c9b-8ab6-423f-ba47-fb9b2db993b1}</MetaDataID>
    public interface IBatteryInfo
    {
        void StartSetting();

        bool CheckIsEnableBatteryOptimizations();
    }


    /// <MetaDataID>{15fb0872-dc44-4348-b71d-670ce119a327}</MetaDataID>
    public interface IAppLifeTime
    {
        event EventHandler ApplicationResuming;

        event EventHandler ApplicationSleeping;

        SerializeTaskScheduler SerializeTaskScheduler { get; }
    }

    public enum KeybordStatus
    {
        KeyboardHiden,
        KeyboardVisible
    }

    public delegate void KeyboardChangeStateHandle(KeybordStatus keybordStatus);


    /// <MetaDataID>{86be3bae-7dca-45e1-8e54-9b1e142b144f}</MetaDataID>
     public class BackgroundServiceState
    {
        public bool Terminate;

    }
}
