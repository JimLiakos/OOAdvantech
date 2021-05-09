using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace TransactionCoordinator
{
    /// <MetaDataID>{b6f9166d-7c26-404e-9990-d37d632a680a}</MetaDataID>
    [RunInstaller(true)]
    public partial class TransactionCoordinatorInstaller : Installer
    {
        public TransactionCoordinatorInstaller()
        {
            InitializeComponent();
        }
    }
}