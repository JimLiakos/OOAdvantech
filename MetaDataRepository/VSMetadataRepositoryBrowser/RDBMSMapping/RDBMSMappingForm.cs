using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    /// <MetaDataID>{50d6b6b6-dbb1-4e2a-98d9-44de50bc76f0}</MetaDataID>
    public partial class RDBMSMappingForm : Form
    {
      
         IVSPackage VSPackage;
        /// <MetaDataID>{c3ee9d73-56ac-46a6-b677-38fd1c457fad}</MetaDataID>
        OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();
        /// <MetaDataID>{efe81d76-5580-441a-9e83-558d55ac8e2e}</MetaDataID>
        public RDBMSMappingForm(IVSPackage vsPackage)
        {
            InitializeComponent();
            TopLevel = false;
            Visible = true;
            VSPackage = vsPackage;
             
        }
        RDBMSMappingForm()
        { 
        }

        public void ShowMetaObject(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            
            Connection.Instance = metaObject;

        }
    }
}
