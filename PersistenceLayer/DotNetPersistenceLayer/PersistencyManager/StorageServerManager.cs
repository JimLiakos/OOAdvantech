using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PersistencyManager
{
    /// <MetaDataID>{fed9c224-74ab-4000-b56d-966a91446678}</MetaDataID>
    public partial class StorageServerManager : Form
    {
        string ComputerName = System.Net.Dns.GetHostName();
        public StorageServerManager()
        {
            InitializeComponent();

            
            //OOAdvantech.PersistenceLayer.StorageServerInstanceLocator locator = OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.GetStorageServerInstanceLocator();

            OOAdvantech.Remoting.RemotingServices theLocalRemotingServices = OOAdvantech.Remoting.RemotingServices.GetRemotingServices("tcp://localhost:9060") as OOAdvantech.Remoting.RemotingServices;
            OOAdvantech.PersistenceLayer.StorageServerInstanceLocator locator = theLocalRemotingServices.CreateInstance(typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).ToString(), typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).Assembly.FullName) as OOAdvantech.PersistenceLayer.StorageServerInstanceLocator;
            foreach (string instanceName in locator.GetStorageServerInstances())
            {
                if (instanceName.Trim().ToLower() == "default")
                    StorageServerInstances.Items.Add(ComputerName.ToUpper());
                else
                    StorageServerInstances.Items.Add(ComputerName.ToUpper() + @"\" + instanceName);
            }
            if (StorageServerInstances.Items.Count > 0)
                StorageServerInstances.SelectedIndex = 0;

        }

        private void StorageServerManager_Load(object sender, EventArgs e)
        {

        }
    }
}