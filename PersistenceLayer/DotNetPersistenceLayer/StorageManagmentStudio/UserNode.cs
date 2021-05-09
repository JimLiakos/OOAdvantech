using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace StorageManagmentStudio
{
   public  class UserNode:MarshalByRefObject,IStorageMetadataNode
    {
       OOAdvantech.Security.User User;
       string UserName;
       public UserNode(OOAdvantech.Security.User user,string userName)
       {
           User = user;
           UserName = userName;
       }

       #region IStorageMetadataNode Members

       public string Name
       {
           get
           {
               return UserName;
           }
           set
           {
               
           }
       }

       IList<IStorageMetadataNode> _SubNodes = new List<IStorageMetadataNode>();
       public IList<IStorageMetadataNode> SubNodes
       {
           get { return _SubNodes; }
       }

       public System.Windows.Media.ImageSource NodeIco
       {
           get 
           {
               return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/User.png", UriKind.Relative));
           }
       }

       #endregion
    }
}
