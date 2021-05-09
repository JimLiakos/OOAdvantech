using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.Transactions;
using Microsoft.Data.ConnectionUI;
using System.Windows.Media.Imaging;
namespace StorageManagmentStudio
{
    /// <MetaDataID>{34b65763-4962-4569-a4c3-b17e9850a358}</MetaDataID>
    public class StoragesFolder:MarshalByRefObject,IStorageMetadataNode
    {

        public readonly OOAdvantech.MetaDataRepository.StorageServer StorageServer;

        public StoragesFolder(OOAdvantech.MetaDataRepository.StorageServer storageServer)
        {
            StorageServer = storageServer;
        }
        #region IStorageMetadataNode Members

        /// <MetaDataID>{40d443cf-d772-478a-95a5-a11d6c420533}</MetaDataID>
        public string Name
        {
            get
            {
                return "Storages";
            }
            set
            {

            }
        }
        /// <exclude>Excluded</exclude>
        List<IStorageMetadataNode> _SubNodes;
        /// <MetaDataID>{d71a4cc3-89c9-43ee-b2eb-194ae9ddf3ee}</MetaDataID>
        public IList<IStorageMetadataNode> SubNodes
        {
            get
            {
                if (_SubNodes == null)
                {

                    
                    _SubNodes = new List<IStorageMetadataNode>();
                    foreach (var storage in StorageServer.Storages)
                    {
                        _SubNodes.Add(new StoragePresentation(storage));
                    }
                }
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
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        internal void AddStorage()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();

            dcd.DataSources.Add(DataSource.OracleDataSource);
            dcd.DataSources.Add(DataSource.SqlDataSource);
            dcd.DataSources.Add(DataSource.SqlFileDataSource);
            dcd.SelectedDataSource = DataSource.SqlDataSource;
            if (DataConnectionDialog.Show(dcd) == System.Windows.Forms.DialogResult.OK)
            {

                if (dcd.SelectedDataSource.DefaultProvider.TargetConnectionType.Namespace == "System.Data.SqlClient")
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(dcd.ConnectionString);
                    string storageName = builder.InitialCatalog;
                    StorageServer.AttachStorage(storageName, dcd.SelectedDataSource.DisplayName, dcd.ConnectionString);
                    _SubNodes = null;
                }
                //dcd.ConnectionString
                //OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageProviders();
                //(RealObject as StoragesFolder).StorageServer.AttachStorage(dcd.SelectedDataSource.DisplayName, dcd.ConnectionString);
            }

            if (ObjectChangeState != null)
                ObjectChangeState(this, null);
        }
    }
}
