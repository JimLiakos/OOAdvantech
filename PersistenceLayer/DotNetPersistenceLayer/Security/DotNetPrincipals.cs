using OOAdvantech.Remoting;
using System;




#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif



namespace OOAdvantech.Security
{
    /// <MetaDataID>{d3823cc7-dfc3-41d3-b3b9-811635099cf5}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{d3823cc7-dfc3-41d3-b3b9-811635099cf5}")]
    [MetaDataRepository.Persistent()]
    public class DotNetPrincipal :MarshalByRefObject, OOAdvantech.Security.IOutOfStorageAccountInfo
    {

        protected OOAdvantech.ObjectStateManagerLink ObjectStateManagerLink;

        /// <MetaDataID>{f73f72cd-7160-448c-9846-f852ba14be06}</MetaDataID>
        public void RefreshSecuritySubject(Subject subject)
        {
             
        }

    }
}
