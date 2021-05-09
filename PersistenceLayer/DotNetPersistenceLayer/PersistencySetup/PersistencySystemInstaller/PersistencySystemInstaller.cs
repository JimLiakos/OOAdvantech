using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace PersistencySystemInstaller
{
    [RunInstaller(true)]
    public partial class PersistencySystemInstaller : Installer
    {
        public PersistencySystemInstaller()
        {
            InitializeComponent();
        }
    }
}
