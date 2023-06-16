using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    public class RDBMSMappingException : Exception
    {
        public readonly MetaObject MetaObject;

        public RDBMSMappingException(OOAdvantech.MetaDataRepository.MetaObject metaObject, string message):base(message)
        {
            MetaObject=metaObject;
        }

        public RDBMSMappingException(OOAdvantech.MetaDataRepository.MetaObject metaObject,string message, Exception innerException) : base(message,innerException)
        {
            MetaObject=metaObject;
        }

    }
}
