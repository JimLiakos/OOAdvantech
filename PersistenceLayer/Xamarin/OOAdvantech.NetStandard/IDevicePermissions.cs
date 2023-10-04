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
        /// <MetaDataID>{b1364594-fec8-40d8-ace1-70b87c4497d8}</MetaDataID>
        Task<bool> CheckPermissionsForQRCodeScan();


        /// <summary>
        /// Request Permission to access infrastructure for QR code scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        /// <MetaDataID>{f88b82f4-cbfd-4120-a294-fce64a3bf805}</MetaDataID>
        Task<bool> RequestPermissionsForQRCodeScan();


        /// <MetaDataID>{4e74db51-609e-47f5-bdee-25732761bd8d}</MetaDataID>
        void ShowAppPermissions();

        /// <MetaDataID>{0dd4880b-0818-4774-b648-0560eeced844}</MetaDataID>
        string DeviceName { get; }

    }
}
