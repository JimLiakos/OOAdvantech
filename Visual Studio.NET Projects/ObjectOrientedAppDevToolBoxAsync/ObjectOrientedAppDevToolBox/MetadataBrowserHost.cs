using System.Security.Permissions;
using System.Windows.Forms;



namespace Microneme.ObjectOrientedAppsDevToolBox
{
    /// <summary>
    /// Summary description for MyControl.
    /// </summary>
    /// <MetaDataID>{ebbde74b-b1ec-45ea-be51-c98f5d3457c6}</MetaDataID>
    public partial class MetadataBrowserHost : UserControl
    {
        /// <MetaDataID>{ff82d793-2112-4f08-b392-0cee6cb54857}</MetaDataID>
        internal VSMetadataRepositoryBrowser.MetadataRepositoryBrowser MetadataRepositoryBrowser;
        /// <MetaDataID>{5584db27-edc6-49d2-aa92-9f3a9b5b0c00}</MetaDataID>
        public MetadataBrowserHost()
        {
            
            InitializeComponent();
        }
        /// <MetaDataID>{e25ab1da-10c1-4900-a550-38724c469ca6}</MetaDataID>
        internal void LoadMetadataRepositoryBrowser(VSMetadataRepositoryBrowser.IVSPackage vsPackage)
        {
            if (MetadataRepositoryBrowser == null)
            {
                MetadataRepositoryBrowser = new VSMetadataRepositoryBrowser.MetadataRepositoryBrowser(vsPackage);
                try
                {
                    MetadataRepositoryBrowser.Dock = DockStyle.Fill;
                    Controls.Add(MetadataRepositoryBrowser);
                }
                catch (System.Exception error)
                {
                }
            }
        }

        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        /// <MetaDataID>{0e6fe32c-3bb3-4d5c-bf62-22e204325669}</MetaDataID>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
              // If we're the top-level form or control, we need to do the mnemonic handling
              if (charCode != ' ' && ProcessMnemonic(charCode))
              {
                    return true;
              }
              return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// Enable the IME status handling for this control.
        /// </summary>
        /// <MetaDataID>{0a2916cf-3dea-4b6f-8cae-862eae17bc3d}</MetaDataID>
        protected override bool CanEnableIme
        {
            get
            {
                return true;
            }
        }

     
    }
}
