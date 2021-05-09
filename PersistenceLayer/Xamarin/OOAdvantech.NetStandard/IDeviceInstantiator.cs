using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{
    /// <MetaDataID>{68ba2a58-5519-4e96-b8a1-4f1e53f1c0e7}</MetaDataID>
    public interface IDeviceInstantiator
    {
        object GetDeviceSpecific(System.Type type);

    }
}
