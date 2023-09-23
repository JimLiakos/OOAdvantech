using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{
    public interface IDevicePermissions
    {
        Task<bool> CheckPermissionsForQrcodeScan();
        Task<bool> RequestPermissionsForQRCodeScan();

    }
}
