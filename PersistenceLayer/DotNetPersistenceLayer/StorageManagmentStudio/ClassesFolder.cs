using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace StorageManagmentStudio
{
    /// <MetaDataID>{0a377179-fe70-498a-b31d-81caa46556c3}</MetaDataID>
    public class ClassesFolder : MarshalByRefObject, IStorageMetadataNode
    {
        public ClassesFolder(List<PersistentClass> persClasses)
        {
            _SubNodes = persClasses.OfType<IStorageMetadataNode>().ToList();
        }
        #region IStorageMetadataNode Members

        public string Name
        {
            get
            {
               return "Classes";
            }
            set
            {
                
            }
        }

        List<IStorageMetadataNode> _SubNodes;
        public IList<IStorageMetadataNode> SubNodes
        {
            get
            {
                if (_SubNodes == null)
                    _SubNodes = new List<IStorageMetadataNode>();
                return _SubNodes;

            }
        }

        public System.Windows.Media.ImageSource NodeIco
        {
            get 
            {
                return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/CloseFolder.png", UriKind.Relative));
            }
        }

        #endregion
    }
}
