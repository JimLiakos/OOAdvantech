using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXConnectableControls
{
    /// <MetaDataID>{f6958f2a-6f50-47ba-b81a-04c39dae1f9d}</MetaDataID>
    public class DevXpressModules
    {

        public static bool ModulesLoaded;
        public static void LoadModules()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad1;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Windows.Forms.MessageBox.Show("LoadModules");
            var sdsd=AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraReports_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_Charts_v9_3_Core);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_Data_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_Utils_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraBars_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraCharts_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraEditors_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraGrid_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraLayout_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraNavBar_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraPivotGrid_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraPivotGrid_v9_3_Core);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraPrinting_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraRichEdit_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraTreeList_v9_3);
            sdsd = AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraVerticalGrid_v9_3);
        }

        private static void CurrentDomain_AssemblyLoad1(object sender, AssemblyLoadEventArgs args)
        {
            
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show((e.ExceptionObject as Exception).Message + Environment.NewLine + (e.ExceptionObject as Exception).StackTrace);
            if ((e.ExceptionObject as Exception).InnerException != null)
            {
                System.Windows.Forms.MessageBox.Show((e.ExceptionObject as Exception).InnerException.Message + Environment.NewLine + (e.ExceptionObject as Exception).InnerException.StackTrace);
            }
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {

        }
        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.IndexOf("DevExpress.XtraReports.v9.3,") == 0)
            {
                System.Windows.Forms.MessageBox.Show(args.Name);
                return AssemblyNativeCode.AssemblyLoader.LoadRawAssembly(Properties.Resources.DevExpress_XtraReports_v9_3);
                //ModulePublisher.ClassRepository.LoadFrom()
            }
            
            return null;
        }
    }
}
