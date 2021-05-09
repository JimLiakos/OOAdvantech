using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace StorageManagmentStudio
{
    /// <MetaDataID>{45892218-018f-4bfb-8cc3-12b61b4c747b}</MetaDataID>
    public class UsersFolder : MarshalByRefObject, IStorageMetadataNode
    {
         public readonly OOAdvantech.MetaDataRepository.StorageServer StorageServer;

        public UsersFolder(OOAdvantech.MetaDataRepository.StorageServer storageServer)
        {
            StorageServer = storageServer;


              var serverUsers = OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(storageServer,
                        storageRef =>
                            new
                            {
                                Users = from user in storageRef.Users
                                                    select new {ActualUser=user,userName=user.Name}
                                                    });

            foreach(var user in serverUsers .Users)
                _SubNodes.Add(new UserNode(user.ActualUser,user.userName));
        }

        #region IStorageMetadataNode Members

        public string Name
        {
            get
            {
                return "Users";
            }
            set
            {
                
            }
        }

        List<IStorageMetadataNode> _SubNodes = new List<IStorageMetadataNode>(); 
        public IList<IStorageMetadataNode> SubNodes
        {
            get { return _SubNodes; }
        }

        public System.Windows.Media.ImageSource NodeIco
        {
            get 
            {
                return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/CloseFolder.png", UriKind.Relative));
            }
        }

        #endregion

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        internal void Refresh()
        {

            
            _SubNodes.Clear();
            var serverUsers = OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(StorageServer,
                     storageRef =>
                         new
                         {
                             Users = from user in storageRef.Users
                                     select new { ActualUser = user, userName = user.Name }
                         });

            foreach (var user in serverUsers.Users)
                _SubNodes.Add(new UserNode(user.ActualUser, user.userName));


            if (ObjectChangeState != null)
            {
                ObjectChangeState(this, "SubNodes");
            }
        }
    }
}
