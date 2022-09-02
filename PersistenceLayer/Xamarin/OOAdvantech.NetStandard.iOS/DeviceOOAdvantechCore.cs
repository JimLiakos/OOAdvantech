using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AudioToolbox;

namespace OOAdvantech.iOS
{
    public class DeviceOOAdvantechCore : IDeviceOOAdvantechCore
    {

        static event MessageReceivedHandle internalMessageReceived;
        public event KeyboardChangeStateHandle KeyboordChangeState;

        event MessageReceivedHandle IDeviceOOAdvantechCore.MessageReceived
        {
            add
            {
                internalMessageReceived += value;

            }

            remove
            {
                internalMessageReceived -= value;

            }
        }


        public void Signin(AuthProvider provider)
        {
            if (provider == AuthProvider.Google)
                Authentication.iOS.FirebaseAuthentication.GoogleSignIn();

            if (provider == AuthProvider.Facebook)
                Authentication.iOS.FirebaseAuthentication.FacebookSignIn();


            // Authentication.Droid.FirebaseAuthentication.GoogleSignIn();
        }

        public void EmailSignUp(string email, string password)
        {
            Authentication.iOS.FirebaseAuthentication.EmailSignUp(email, password);
        }
        public void EmailSignIn(string email, string password)
        {
            Authentication.iOS.FirebaseAuthentication.EmailSignIn(email, password);
        }
        public void SignOut()
        {
            //Authentication.Droid.FirebaseAuthentication.SignOut();
        }

        public static void MessageReceived(IRemoteMessage message)
        {
            internalMessageReceived?.Invoke(message);
        }

        static string _FirebaseToken;

        public string FirebaseToken
        {
            get
            {
                return _FirebaseToken;
            }
        }
     

        public DeviceOOAdvantechCore()
        {
            _LinesPhoneNumbers = new List<SIMCardData>();
            List<string> args = new List<string>() { "SIM Card 6972992632;73488bc19e444f51a031fe2b72bdee38", "SIM Card  6972992635;39a84c60bbf2460cb7e3d1f6b579d75b", "SIM Card 6972992638;2d40cc3a6b4b4626b18ade71132e19a3" };
            foreach (var arg in args)
            {
                string simDescription = arg.Substring(0, arg.IndexOf(";"));
                string simIdentity = arg.Substring(arg.IndexOf(";") + 1);
                _LinesPhoneNumbers.Add(new OOAdvantech.SIMCardData() { SIMCardIdentity = simIdentity, SIMCardDescription = simDescription });
            }

            //foreach (var simCard in GetSimCards())
            //{
            //    _LinesPhoneNumbers.Add(new OOAdvantech.SIMCardData() { SIMCardIdentity = simCard.ICCID,SIMCardDescription = simCard.NetworkOperatorName+" "+simCard.PhoneNumber+" "+simCard.ICCID });
            //}
        }

        List<SIMCardData> _LinesPhoneNumbers;
        public IList<SIMCardData> LinesPhoneNumbers
        {
            get
            {
                return _LinesPhoneNumbers;
            }
        }

        public double ScreeHeight
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double ScreeWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string _DeviceID;
        public string DeviceID
        {
            get
            {
                if (_DeviceID == null)
                    _DeviceID = GetDeviceUniqueID();

                return _DeviceID;

            }
        }

        public bool ForegroundService => false;

        public bool IsForegroundServiceStarted => false;

        static bool _IsinSleepMode;
        public bool IsinSleepMode { get =>  _IsinSleepMode; set => _IsinSleepMode=value; }

        public bool IsBackgroundServiceStarted => throw new NotImplementedException();

        public SIMCardData GetLinePhoneNumber(int lineIndex)
        {
            return _LinesPhoneNumbers[lineIndex];
        }

        public IReadOnlyList<SimCard> GetSimCards()
        {
            var results = new List<SimCard>();

            //var mTelephonyMgr = (TelephonyManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.TelephonyService);

            //SubscriptionManager sm = Android.Telephony.SubscriptionManager.From(Android.App.Application.Context);
            //IList<SubscriptionInfo> sis = sm.ActiveSubscriptionInfoList;
            //if (sis != null)
            //{
            //    foreach (SubscriptionInfo si in sis)
            //    {
            //        string carrier = si.CarrierName;
            //        string iccId = si.IccId;
            //        string phoneNum = si.Number;
            //        SimCard simCard = new SimCard();
            //        simCard.PhoneNumber = si.Number;
            //        simCard.ICCID = si.IccId;
            //        simCard.MCC = si.Mcc.ToString();
            //        simCard.MNC = si.Mnc.ToString();
            //        simCard.IMSI = mTelephonyMgr.SubscriberId;
            //        simCard.IMEI = mTelephonyMgr.Imei;
            //        simCard.NetworkOperatorName = si.DisplayName;
            //        results.Add(simCard);

            //    }
            //}
            return results.AsReadOnly();
        }



        public static String GetDeviceUniqueID()
        {

           string id = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            return id;
           
        }

        public void StartForegroundService()
        {
        }

        public void StopForegroundService()
        {
        }

        public void PlaySound()
        {
            SystemSound systemSound = new SystemSound(1320);
            systemSound.PlayAlertSound();
        }
        public static void InitFirebase(string firebaseToken, string googleAuthWebClientID)
        {
            _FirebaseToken = firebaseToken;
            Authentication.iOS.FirebaseAuthentication.Init( googleAuthWebClientID);
        }

        Task<string> IDeviceOOAdvantechCore.EmailSignUp(string email, string password)
        {
            throw new NotImplementedException();
        }

        Task<string> IDeviceOOAdvantechCore.EmailSignIn(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void SendPasswordResetEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool RunInBackground(Action action, BackgroundServiceState serviceState)
        {
            throw new NotImplementedException();
        }

        public void StopBackgroundService()
        {
            throw new NotImplementedException();
        }
    }
    public class RemoteMessage : IRemoteMessage
    {
        public IDictionary<string, string> Data { get; set; }

        public string From { get; set; }

        public string MessageId { get; set; }

        public string MessageType { get; set; }

        public DateTime SentTime { get; set; }

        public string To { get; set; }
    }


}
