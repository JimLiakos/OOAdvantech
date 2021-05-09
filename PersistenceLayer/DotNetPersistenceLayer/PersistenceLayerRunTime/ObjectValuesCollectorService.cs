using OOAdvantech.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

#if DeviceDotNet
namespace OOAdvantech.Remoting
{
    public interface IObjectValuesCollectorService
    {
        /// <MetaDataID>{4c6895a0-00fe-497c-8eb0-dafbb37baf05}</MetaDataID>
        Collections.StructureSet GetValues(object _object, OOAdvantech.Collections.Generic.List<string> paths);
        /// <MetaDataID>{e97ae5b0-4f16-4b13-a084-98caffa9c6d3}</MetaDataID>
        Collections.StructureSet GetValues(OOAdvantech.Collections.Generic.List<object> objectCollection, Type type, OOAdvantech.Collections.Generic.List<string> paths);
    }
}
#endif
namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{1E241738-B463-41fa-A3A1-C085F607BA50}</MetaDataID>
    public class ObjectValuesCollectorService:MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantech.Remoting.IObjectValuesCollectorService
    {

#region IObjectValuesCollectorService Members

        /// <MetaDataID>{53a4d532-43ba-46dd-944d-7767de2326a9}</MetaDataID>
        /// <summary>teste</summary>
        OOAdvantech.Collections.StructureSet OOAdvantech.Remoting.IObjectValuesCollectorService.GetValues(object _object, OOAdvantech.Collections.Generic.List<string> paths)
        {
            
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(new MetaDataRepository.ObjectQueryLanguage.StructureSet(_object,paths));
            
        }

        /// <MetaDataID>{80a6090d-0b9a-4970-8717-ae7f52b4bb59}</MetaDataID>
        public OOAdvantech.Collections.StructureSet GetValues(OOAdvantech.Collections.Generic.List<object> objectCollection,Type type,OOAdvantech.Collections.Generic.List<string> paths)
        {
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent( new MetaDataRepository.ObjectQueryLanguage.StructureSet(objectCollection,type, paths));
        }

#endregion
    }
}
