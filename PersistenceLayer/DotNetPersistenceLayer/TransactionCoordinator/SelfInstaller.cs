using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration.Install;

namespace TransactionCoordinator
{
    /// <MetaDataID>{b4e95ee2-956d-42c8-acb4-e2fe64694ca6}</MetaDataID>
    public static class SelfInstaller
    {
        private static readonly string _exePath =
            Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
