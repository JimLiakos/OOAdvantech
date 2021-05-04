using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{e9ddbe9a-cae2-486a-a59d-6de44a6aaf66}</MetaDataID>
    internal class ErrorLog:OOAdvantech.MetaDataRepository.IErrorLog
    {
        RationalRose.RoseApplication RoseApplication;
        public ErrorLog(RationalRose.RoseApplication roseApplication)
        {
            RoseApplication = roseApplication;
        }
        public void WriteError(string error)
        {
            RoseApplication.WriteErrorLog(error); 
        }
    }
}
