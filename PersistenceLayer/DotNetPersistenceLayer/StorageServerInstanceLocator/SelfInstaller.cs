using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration.Install;

namespace StorageServerInstanceLocator
{
    /// <MetaDataID>{d0af018b-e677-4a28-b8ef-b76bb8a60ffe}</MetaDataID>
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
