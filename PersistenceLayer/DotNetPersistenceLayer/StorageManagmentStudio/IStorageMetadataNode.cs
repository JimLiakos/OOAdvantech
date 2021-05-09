using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StorageManagmentStudio
{
    /// <MetaDataID>{9cf35025-6b73-4933-8eca-dfc27f864d16}</MetaDataID>
   public interface IStorageMetadataNode
    {
        /// <MetaDataID>{d91eb456-9902-44e8-866b-c220c019aac6}</MetaDataID>
        string Name { get; set; }

        /// <MetaDataID>{7169ba31-668d-45b2-a0a1-474b74bf8c47}</MetaDataID>
        IList<IStorageMetadataNode> SubNodes { get;  }


        ImageSource NodeIco
        {
            get;
        }

    }
}
