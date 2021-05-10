using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration.Install;

namespace PublishModuleService
{
    /// <MetaDataID>{3fa791f7-931a-4f11-8d04-7105e5877cd8}</MetaDataID>
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
