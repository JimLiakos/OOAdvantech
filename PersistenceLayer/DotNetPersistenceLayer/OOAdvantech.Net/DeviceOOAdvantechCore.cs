using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Net
{
    /// <MetaDataID>{1700fcef-9ec0-4ddf-a61d-741d0e275f19}</MetaDataID>
    public class DeviceOOAdvantechCore: IDeviceOOAdvantechCore
    {
        
        /// <MetaDataID>{0e62da4f-1265-43fc-b94c-560dd2073087}</MetaDataID>
        public static List<SIMCardData> LinesPhoneNumbers = new List<SIMCardData>();

        /// <MetaDataID>{1665960d-62e6-4d5c-956b-48563064f4f2}</MetaDataID>
        IList<SIMCardData> IDeviceOOAdvantechCore.LinesPhoneNumbers
        {
            get
            {
                return LinesPhoneNumbers.AsReadOnly();
            }
        }

        /// <MetaDataID>{519109f9-eaa3-46a6-a8dc-c4bf573d09ff}</MetaDataID>
        public double ScreeHeight
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{57ccfc00-2166-41b7-b99d-5fc5ca6f354f}</MetaDataID>
        public double ScreeWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{101387b9-bdf2-49a5-87fc-7d7b46a3e461}</MetaDataID>
        string _DeviceID;

        /// <MetaDataID>{6c66db6f-d659-4bcf-b45d-b1e97235f301}</MetaDataID>
        public string DeviceID
        {
            get
            {
                if (_DeviceID == null)
                    _DeviceID = GetdeviceID();
                return _DeviceID;
            }
            set
            {
                _DeviceID = value;
            }
        }

        public string FirebaseToken =>  null;

        /// <MetaDataID>{93227076-f4af-4e03-bf2a-0ea2da10dd89}</MetaDataID>
        public static string GetdeviceID()
        {
            if (!string.IsNullOrWhiteSpace(DebugDeviceID))
                return DebugDeviceID;

            string deviceID = null;
            string hId = null;
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = null;
            try
            {
                mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                mbsList = mbs.Get();
                foreach (ManagementBaseObject o in mbsList)
                {
                    ManagementObject mo = (ManagementObject)o;

                    hId = BitConverter.ToString(Encoding.UTF8.GetBytes(mo["ProcessorID"].ToString().Trim()));
                    deviceID += hId;
                    //deviceID += "Win32_processor " + hId;
                    Console.WriteLine(hId);
                }
            }
            catch (Exception error)
            {
            }
            try
            {
                mbs = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                mbsList = mbs.Get();
                foreach (ManagementBaseObject o in mbsList)
                {
                    ManagementObject mo = (ManagementObject)o;

                    hId = BitConverter.ToString(Encoding.UTF8.GetBytes(mo["SerialNumber"].ToString().Trim()));
                    deviceID += hId;
                    //deviceID += "Win32_BaseBoard " + hId;
                    Console.WriteLine(hId);
                }
            }
            catch (Exception error)
            {
            }
            try
            {
                mbs = new ManagementObjectSearcher("Select * From Win32_BIOS");
                mbsList = mbs.Get();
                foreach (ManagementBaseObject o in mbsList)
                {
                    ManagementObject mo = (ManagementObject)o;
                    hId = BitConverter.ToString(Encoding.UTF8.GetBytes(mo["SerialNumber"].ToString().Trim()));
                    //deviceID += "Win32_BIOS " + hId;
                    deviceID += hId;
                    Console.WriteLine(hId);
                }
            }
            catch (Exception error)
            {
            }
            //Win32_DiskDrive::SerialNumber
            //try
            //{
            //    string osDiskSerialNumber = GetHardSerial();
            //    //deviceID += "Win32_DiskDrive " + osDiskSerialNumber;
            //    deviceID += osDiskSerialNumber;
            //}
            //catch (Exception error)
            //{
            //}
            deviceID = deviceID.Replace("-", "");
            return deviceID;
        }


        /// <MetaDataID>{1b90196e-b882-4167-a7a9-cde82f7856d0}</MetaDataID>
        public SIMCardData GetLinePhoneNumber(int lineIndex)
        {
            if (lineIndex < LinesPhoneNumbers.Count)
                return LinesPhoneNumbers[lineIndex];
            else
                return default(SIMCardData);
        }
        /// <MetaDataID>{83f7a8e1-a738-423f-ae75-de3fc0f0482c}</MetaDataID>
        List<SimCard> SimCards = new List<SimCard>();
        public static string DebugDeviceID;

        /// <MetaDataID>{5b493292-06ab-4cd5-8bbb-5d8fee00fe43}</MetaDataID>
        public IReadOnlyList<SimCard> GetSimCards()
        {
            return SimCards.AsReadOnly();
        }
    }
}
