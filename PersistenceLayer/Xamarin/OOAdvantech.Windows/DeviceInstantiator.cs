using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(OOAdvantech.Windows.DeviceInstantiator))]
namespace OOAdvantech.Windows
{


    /// <MetaDataID>{a63effd4-2a59-480e-be8c-1f3fb2eb519c}</MetaDataID>
    public class DeviceInstantiator : IDeviceInstantiator
    {
        public object GetDeviceSpecific(System.Type type)
        {
            return null;
        }
    }
}
