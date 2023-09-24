using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{

    /// <MetaDataID>{e47a8e96-0dbb-4db0-9597-4a8f9b3f3536}</MetaDataID>
    [HttpVisible]
    public interface IDevicePermissions
    {
        Task<bool> CheckPermissionsForQRCodeScan();


        /// <summary>
        /// Request Permission to access infrastructure for QR code scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        Task<bool> RequestPermissionsForQRCodeScan();


        void ShowAppPermissions();

        string DeviceName { get; }

    }
}
