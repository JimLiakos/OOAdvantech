using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    /// <MetaDataID>{d3c3a6c5-f721-4a49-ad33-c18ecdd58b3e}</MetaDataID>
    public class RDBMSMappingException : Exception
    {
        public readonly MetaObject MetaObject;

        public RDBMSMappingException(OOAdvantech.MetaDataRepository.MetaObject metaObject, string message) : base(message)
        {
            MetaObject=metaObject;
        }

        public RDBMSMappingException(OOAdvantech.MetaDataRepository.MetaObject metaObject, string message, Exception innerException) : base(message, innerException)
        {
            MetaObject=metaObject;
        }

    }
}
