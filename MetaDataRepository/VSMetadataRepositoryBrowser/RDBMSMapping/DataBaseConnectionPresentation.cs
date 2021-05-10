using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    /// <MetaDataID>{408d2fd7-f94b-4129-a985-16fa7c2a3263}</MetaDataID>
    public class DataBaseConnectionPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection>
    {
        public DataBaseConnectionPresentation(OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection dataBaseConnection):base(dataBaseConnection)
        {

        }

    }
}
