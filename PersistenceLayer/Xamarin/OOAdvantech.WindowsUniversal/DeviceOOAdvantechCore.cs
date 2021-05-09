using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;

namespace OOAdvantech.WindowsUniversal
{
    /// <MetaDataID>{742ee472-0d13-4a1e-96fd-e9e7296e0de8}</MetaDataID>
    class DeviceOOAdvantechCore : IDeviceOOAdvantechCore
    {
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
        }

        List<SIMCardData> _LinesPhoneNumbers;
        public IList<SIMCardData> LinesPhoneNumbers
        {
            get
            {
                return _LinesPhoneNumbers;
            }
        }

        public SIMCardData GetLinePhoneNumber(int lineIndex)
        {
            return _LinesPhoneNumbers[lineIndex];
        }


        /// <summary>
        ///     Will only return details of a SINGLE simcard.
        /// </summary>
        /// <remarks>TODO: Add support for multiple sim cards. See issue #1</remarks>
        public IReadOnlyList<SimCard> GetSimCards()
        {
            var results = new List<SimCard>();

            try
            {
                var modem = MobileBroadbandModem.GetDefault();
                if (modem == null)
                {
                    return results.AsReadOnly();
                }

                var account = modem.CurrentAccount;
                if (account == null)
                {
                    return results.AsReadOnly();
                }

                var simCard = new SimCard();

                simCard.ICCID = account.CurrentDeviceInformation.SimIccId;
                simCard.IMSI = account.CurrentDeviceInformation.SubscriberId;
                simCard.MSISDN = modem.DeviceInformation.TelephoneNumbers;

                simCard.MCC = ExtractMCC(simCard.IMSI);
                simCard.MNC = ExtractMNC(simCard.IMSI);
                simCard.MSID = ExtractMSID(simCard.IMSI);

                results.Add(simCard);

            }
            catch (Exception error)
            {

            }
            return results.AsReadOnly();
        }

        /// <summary>
        ///     Mobile Country Code(MCC) : First 3 digits of IMSI gives you MCC.
        /// </summary>
        private static string ExtractMCC(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;

            var operatorId = imsi.Substring(0, 5);
            var mccId = operatorId.Substring(0, 3);

            return mccId;
            ;
        }

        /// <summary>
        ///     Mobile Network Code (MNC) : Next 2 or 3 digits give you this info.
        /// </summary>
        private static string ExtractMNC(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;
            var operatorId = imsi.Substring(0, 5);
            var mncId = operatorId.Substring(3, 2);

            return mncId;
        }

        /// <summary>
        ///     Mobile Station ID (MSID) : Rest of the digits. Gives away the network you are using like IS-95, TDMA , GSM etc.
        /// </summary>
        private static string ExtractMSID(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;

            var msid = imsi.Substring(6);

            return msid;
        }
    }
}
