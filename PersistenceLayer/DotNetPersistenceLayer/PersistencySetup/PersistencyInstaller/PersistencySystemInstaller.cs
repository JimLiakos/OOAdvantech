using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security.Principal;

namespace OOAdvantech.PersistenceLayer
{
    /// <summary>
    /// Summary description for PersistencySystemInstalle.
    /// </summary>
    /// <MetaDataID>{7F39B09A-1333-4857-886E-8882D6BEF9F0}</MetaDataID>
    [RunInstaller(true)]
    public class PersistencySystemInstaller : System.Configuration.Install.Installer
    {
         
        /// <MetaDataID>{9F7651D2-1593-4A95-B14D-C7EC46AA56A1}</MetaDataID>
        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted(savedState);

            try
            {
                System.ServiceProcess.ServiceController ooadvanthechTransactionsServer = new System.ServiceProcess.ServiceController("OOAdvantechTransactionCoordinator");
                ooadvanthechTransactionsServer.Start();
                System.ServiceProcess.ServiceController storageServerInstanceLocator = new System.ServiceProcess.ServiceController("StorageServerInstanceLocator");
                storageServerInstanceLocator.Start();
                System.ServiceProcess.ServiceController storageServer = new System.ServiceProcess.ServiceController("StorageServer");
                storageServer.Start();
                System.Diagnostics.Process.Start(Location + "PersistencyManager.exe");
            }
            catch (System.Exception)
            {
            }

        }

        /// <MetaDataID>{9D3D2D97-EBCB-4259-BB39-67E31E5EC81E}</MetaDataID>
        protected override void OnBeforeRollback(IDictionary savedState)
        {
            base.OnBeforeRollback(savedState);
            UnRegister();


        }

        /// <MetaDataID>{CCE77634-953C-421C-8D6B-F7109EB00024}</MetaDataID>
        void UnRegister()
        {
            string commandLine = null;
            string arguments = null;
            long Count = 0;
            System.Reflection.Assembly assembly = null;
            System.Diagnostics.ProcessStartInfo ProcessStartInfo = null;
            System.Diagnostics.Process process = null;

            try
            {


                #region Remove PersistencyService

                try
                {
                    System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
                    string PersistencyManagerFileName = Location + "PersistencyManager.exe";
                    foreach (System.Diagnostics.Process _process in processes)
                    {
                        try
                        {
                            if (_process.MainModule == null)
                                continue;
                        }
                        catch (System.Exception)
                        {
                            continue;
                        }

                        if (_process.MainModule.FileName.Trim().ToLower() == PersistencyManagerFileName.Trim().ToLower())
                            try { _process.Kill(); }
                            catch (System.Exception) { }
                    }
                }
                catch (System.Exception)
                {
                }
                try
                {
                    System.ServiceProcess.ServiceController ooadvanthechTransactionsServer = new System.ServiceProcess.ServiceController("OOAdvantechTransactionCoordinator");
                    if (ooadvanthechTransactionsServer.CanStop)
                        ooadvanthechTransactionsServer.Stop();
                    System.ServiceProcess.ServiceController storageServerInstanceLocator = new System.ServiceProcess.ServiceController("StorageServerInstanceLocator");
                    if (storageServerInstanceLocator.CanStop)
                        storageServerInstanceLocator.Stop();
                    System.ServiceProcess.ServiceController storageServer = new System.ServiceProcess.ServiceController("StorageServer");
                    if (storageServer.CanStop)
                        storageServer.Stop();

                }
                catch (System.Exception Error)
                {
                }



                try
                {
                    commandLine = Location + "InstallUtil.exe";
                    arguments = "/u \"" + Location + "PersistencyService.exe\"";
                    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);

                    //ProcessStartInfo.UseShellExecute=false;
                    ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process = System.Diagnostics.Process.Start(ProcessStartInfo);
                    Count = 0;
                    while (!process.HasExited && Count < 40)
                    {
                        Count++;
                        System.Threading.Thread.Sleep(200);
                    }

                    commandLine = Location + "InstallUtil.exe";
                    arguments = "/u \"" + Location + "TransactionCoordinator.exe\"";
                    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);

                    //ProcessStartInfo.UseShellExecute=false;
                    ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process = System.Diagnostics.Process.Start(ProcessStartInfo);
                    Count = 0;
                    while (!process.HasExited && Count < 40)
                    {
                        Count++;
                        System.Threading.Thread.Sleep(200);
                    }
                    commandLine = Location + "InstallUtil.exe";
                    arguments = "/u \"" + Location + "StorageServerInstanceLocator.exe\"";
                    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);

                    //ProcessStartInfo.UseShellExecute=false;
                    ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process = System.Diagnostics.Process.Start(ProcessStartInfo);
                    Count = 0;
                    while (!process.HasExited && Count < 40)
                    {
                        Count++;
                        System.Threading.Thread.Sleep(200);
                    }
                }
                catch (System.Exception)
                {

                }
                #endregion


                #region OOAdvantech from COM+

                //UnRegisterComPlusApp("OOAdvantech");

                try
                {
                    System.IO.File.Delete(Location + "OOAdvantech.tlb");
                }
                catch (System.Exception)
                {
                }
                #endregion

                #region Remove modules registry data
                Register(Location, Location, "Linq.dll", true, false);// Linq.dll

                Register(Location, Location + "modules\\", "mssqlrt.mod", true, true);//MSSQLPersistenceRunTime.dll
                Register(Location, Location + "modules\\", "mssqlfrt.mod", true, true);//MSSQLFastPersistenceRunTime.dll
                Register(Location, Location + "modules\\", "rdbmsrt.mod", true, true);//RDBMSPersistenceRunTime.dll
                Register(Location, Location + "modules\\", "rdbmsmdrt.mod", true, true);//RDBMSMetaDataPersistenceRunTime.dll
                Register(Location, Location + "modules\\", "rdbmsmap.mod", true, true);//RDBMSMetaDataRepository.dll
                Register(Location, Location + "modules\\", "mdatrt.mod", true, true);//MetaDataLoadingSystem.dll
                Register(Location, Location + "modules\\", "plrt.mod", true, true);//PersistenceLayerRunTime.dll
                Register(Location, Location + "modules\\", "dotnetmdr.mod", true, true);//DotNetMetaDataRepository.dll
                Register(Location, Location + "modules\\", "codemdr.mod", true, true);//CodeMetaDataRepository.dll
                Register(Location, Location + "modules\\", "parser.mod", true, true);//Parser.dll

                //Register(Location, Location + "modules\\", "CsOQLParser.mod", true, true);//MSSQLPersistenceRunTime.dll
                //Register(Location,Location,"ParserWr.dll",true,false);
                //Register(Location,Location,"TransactionManagmentSystem.dll",true,false);
                Register(Location, Location, "OOAdvantechFacade.dll", true, false);
                Register(Location, Location, "OOAdvantech.dll", true, false);
                Register(Location, Location, "PersistenceLayerRunTimeFacade.dll", true, false);
                //  Register(Location, Location, "CodeMetaDataRepository.dll", true, false);
                Register(Location, Location, "AssemblyNativeCode.dll", true, false);
                //  Register(Location, Location, "BinaryFormatter.dll", true, false);
                //  if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                // Register(Location, Location + @"x64\", "BinaryFormatter.dll", true, false);

                Register(Location, Location, "ModulePublisher.dll", true, false);
                #endregion


                #region Remove AssemblyNativeCode.dll,ModulePublisher.dll from GAC


                RemoveFromGac(Location + "AssemblyNativeCode.dll");
                RemoveFromGac(Location + "ModulePublisher.dll");
                RemoveFromGac(Location + "OOAdvantech.dll");
                //RemoveFromGac(Location + "BinaryFormatter.dll");
                //if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                //  RemoveFromGac(Location + @"x64\BinaryFormatter.dll");
                //System.Windows.Forms.MessageBox.Show("Check GAC");


                //try
                //{
                //    assembly = System.Reflection.Assembly.LoadFile(Location + "ModulePublisher.dll");
                //    commandLine = Location + "GACutil.exe";
                //    arguments = "-u \"" + assembly.FullName + "\"";
                //    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //    ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //    process = System.Diagnostics.Process.Start(ProcessStartInfo);
                //    Count = 0;
                //    while (!process.HasExited && Count < 40)
                //    {
                //        Count++;
                //        System.Threading.Thread.Sleep(200);
                //    }
                //}
                //catch (System.Exception)
                //{
                //}

                //try
                //{
                //    assembly = System.Reflection.Assembly.LoadFile(Location + "AssemblyNativeCode.dll");
                //    commandLine = Location + "GACutil.exe";
                //    arguments = "-u \"" + assembly.FullName + "\"";
                //    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //    ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //    process = System.Diagnostics.Process.Start(ProcessStartInfo);
                //    while (!process.HasExited && Count < 40)
                //    {
                //        Count++;
                //        System.Threading.Thread.Sleep(200);
                //    }
                //}
                //catch (System.Exception)
                //{
                //}
                #endregion

                try
                {

                    //System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(System.Environment.SystemDirectory);
                    //if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                    //{
                    //    System.IO.File.Delete(directoryInfo.Parent.FullName + @"\SystemWow64\NativeCodeAsseblyLoader.dll");
                    //    System.IO.File.Delete(directoryInfo.Parent.FullName + @"\System32\NativeCodeAsseblyLoader.dll");
                    //}
                    //else
                    //{
                    //    System.IO.File.Delete(directoryInfo.Parent.FullName + @"\System32\NativeCodeAsseblyLoader.dll");
                    //}

                }
                catch (Exception error)
                {


                }
                try
                {
                    System.IO.File.Delete(Location + "persistencyservice.InstallLog");
                }
                catch (System.Exception)
                {
                }
            }
            catch (System.Exception)
            {

            }

        }
        public override void Uninstall(IDictionary savedState)
        {
            System.Windows.Forms.MessageBox.Show("Uninstall");
            base.Uninstall(savedState);
        }
        
        /// <MetaDataID>{0B211379-88E7-401A-875E-FA93FDA0BD2E}</MetaDataID>
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
           // System.Windows.Forms.MessageBox.Show("OnBeforeUninstall");
            int count = 10;
            try
            {
                System.ServiceProcess.ServiceController ooadvanthechTransactionsServer = new System.ServiceProcess.ServiceController("OOAdvantechTransactionCoordinator");
                if (ooadvanthechTransactionsServer.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                    ooadvanthechTransactionsServer.Stop();
                count = 10;
                while (ooadvanthechTransactionsServer.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    System.Threading.Thread.Sleep(100);
                    count--;
                    if (count < 0)
                        break;
                }
            }
            catch (Exception error) { }
            try
            {
                System.ServiceProcess.ServiceController storageServerInstanceLocator = new System.ServiceProcess.ServiceController("StorageServerInstanceLocator");
                if (storageServerInstanceLocator.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                    storageServerInstanceLocator.Stop();
                count = 10;
                while (storageServerInstanceLocator.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    System.Threading.Thread.Sleep(100);
                    count--;
                    if (count < 0)
                        break;
                }

            }
            catch (Exception error) { }
            try
            {
                System.ServiceProcess.ServiceController storageServer = new System.ServiceProcess.ServiceController("StorageServer");
                if (storageServer.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                    storageServer.Stop();
                count = 10;
                while (storageServer.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    System.Threading.Thread.Sleep(100);
                    count--;
                    if (count < 0)
                        break;
                }

            }
            catch (Exception error) { }
            UnRegister();
            base.OnBeforeUninstall(savedState);



            

        }
        /// <MetaDataID>{D07650C0-E6CF-464D-981F-8010409AED77}</MetaDataID>
        protected void Register(string modulePublisherLocation, string assemblyFileLocation, string assemblyFile, bool remove, bool encrypted)
        {
            try
            {

                string commandLine = "\"" + assemblyFileLocation + assemblyFile + "\"";
                if (remove)
                    commandLine += " /u";
                if (encrypted)
                    commandLine += " /enc";
                commandLine += " /s";

                //System.Windows.Forms.MessageBox.Show(modulePublisherLocation+"ModulePublisherHostProcess.exe  "+commandLine);

                System.Diagnostics.Process RegProcess = System.Diagnostics.Process.Start(modulePublisherLocation + "ModulePublisherHostProcess.exe", commandLine);
                while (!RegProcess.HasExited)
                    System.Threading.Thread.Sleep(200);
                if (RegProcess.ExitCode < 0)
                    throw new System.Exception("Error on " + assemblyFile + " registration. Error code =" + RegProcess.ExitCode.ToString());
            }
            catch (System.Exception Error)
            {
                if (remove == true)
                    return;
                throw Error;
            }


        }

        public void UnRegisterComPlusApp(string appName)
        {
            //try
            //{
            //    COMAdmin.ICOMAdminCatalog COMAdminCatalog=new COMAdmin.COMAdminCatalogClass();
            //    COMAdmin.COMAdminCatalogCollection apps=COMAdminCatalog.GetCollection("Applications")as COMAdmin.COMAdminCatalogCollection;
            //    COMAdmin.COMAdminCatalogObject app=null;
            //    apps.Populate();
            //    int Count=apps.Count;
            //    for(int i=0;i!=Count;i++)
            //    {
            //        COMAdmin.COMAdminCatalogObject currApp = apps.get_Item(i) as COMAdmin.COMAdminCatalogObject;
            //        string currAppName=currApp.Name as string;
            //        currAppName=currAppName.ToLower().Trim();
            //        appName=appName.ToLower().Trim();
            //        if(appName==currAppName)
            //        {
            //            apps.Remove(i);
            //            apps.SaveChanges();
            //            break;
            //        }
            //    }
            //}
            //catch(System.Exception)
            //{
            //}
        }
        public void RegisterComPlusApp(string appName, string appDesc, string appID, string dllName, int ShutdownAfter, bool RunForever, bool applicationAccessChecksEnabled)
        {
            //COMAdmin.ICOMAdminCatalog COMAdminCatalog=new COMAdmin.COMAdminCatalogClass();
            //COMAdmin.COMAdminCatalogCollection apps=COMAdminCatalog.GetCollection("Applications")as COMAdmin.COMAdminCatalogCollection;
            //COMAdmin.COMAdminCatalogObject app=null;
            //apps.Populate();
            //int Count=apps.Count;
            //for(int i=0;i!=Count;i++)
            //{
            //    COMAdmin.COMAdminCatalogObject currApp = apps.get_Item(i) as COMAdmin.COMAdminCatalogObject;
            //    string CurrAppID=currApp.get_Value("ID") as string;
            //    appID=appID.ToLower();
            //    CurrAppID=CurrAppID.ToLower();
            //    if(CurrAppID==appID)
            //    {
            //        apps.Remove(i);
            //        apps.SaveChanges();
            //        break;
            //    }
            //}
            //app=apps.Add() as COMAdmin.COMAdminCatalogObject;
            //app.set_Value("Name",appName);
            //app.set_Value("ID",appID);
            //app.set_Value("Description",appDesc);
            //app.set_Value("ApplicationAccessChecksEnabled",true);
            //app.set_Value("AccessChecksLevel",COMAdmin.COMAdminAccessChecksLevelOptions.COMAdminAccessChecksApplicationComponentLevel);
            //app.set_Value("ShutdownAfter",ShutdownAfter);
            //app.set_Value("RunForever",RunForever);
            //app.set_Value("ApplicationAccessChecksEnabled",applicationAccessChecksEnabled);
            //apps.SaveChanges();
            //COMAdminCatalog.InstallComponent(appName,dllName,"","");

        }

        public static bool IsAdmin
        {
            get
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        } 

        /// <MetaDataID>{F131E450-7DEF-43A0-B9FE-212CB5A7D934}</MetaDataID>
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            try
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(System.Environment.SystemDirectory);
                try
                {
                    //if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                    //{
                    //    System.IO.File.Copy(Location + @"NativeCodeAsseblyLoader.dll", directoryInfo.Parent.FullName + @"\SystemWow64\NativeCodeAsseblyLoader.dll", true);
                    //    System.IO.File.Copy(Location + @"x64\NativeCodeAsseblyLoader.dll", directoryInfo.Parent.FullName + @"\System32\NativeCodeAsseblyLoader.dll", true);
                    //}
                    //else
                    //{
                    //    System.IO.File.Copy(Location + @"NativeCodeAsseblyLoader.dll", directoryInfo.Parent.FullName + @"\System32\NativeCodeAsseblyLoader.dll", true);
                    //}
                }
                catch (System.Exception error)
                {
                }

                AddToGac(Location + "AssemblyNativeCode.dll");
                AddToGac(Location + "ModulePublisher.dll");
                AddToGac(Location + "OOAdvantech.dll");
                string arguments = null;
                System.Diagnostics.Process process = null;
     


                //AddToGac(Location + "BinaryFormatter.dll");
                //if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                //AddToGac(Location + @"x64\BinaryFormatter.dll");
                //System.Windows.Forms.MessageBox.Show("Check GAC");




                //commandLine = Location + "GACutil.exe";
                //arguments = "-if \"" + Location + "ModulePublisher.dll\"";


                //ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //process = System.Diagnostics.Process.Start(ProcessStartInfo);
                //Count = 0;
                //while (!process.HasExited && Count < 40)
                //{
                //    Count++;
                //    System.Threading.Thread.Sleep(200);
                //}

                //commandLine = Location + "GACutil.exe";
                //arguments = "-if \"" + Location + "OOAdvantech.dll\"";


                //ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //process = System.Diagnostics.Process.Start(ProcessStartInfo);
                //Count = 0;
                //while (!process.HasExited && Count < 40)
                //{
                //    Count++;
                //    System.Threading.Thread.Sleep(200);
                //}



                Register(Location, Location, "AssemblyNativeCode.dll", false, false);
                //Register(Location, Location, "BinaryFormatter.dll", false, false);
                //if (System.IO.Directory.Exists(directoryInfo.Parent.FullName + @"\SystemWow64"))
                //  Register(Location, Location+@"x64\", "BinaryFormatter.dll", false, false);


                Register(Location, Location, "ModulePublisher.dll", false, false);

                //System.Windows.Forms.MessageBox.Show("RegisterOOAdvantech.dll");
                Register(Location, Location, "OOAdvantech.dll", false, false);
                //Register(Location, Location, "CodeMetaDataRepository.dll", false, false);


                //UnRegisterComPlusApp("OOAdvantech");


                Register(Location, Location + "modules\\", "codemdr.mod", false, true);
                //				System.Windows.Forms.MessageBox.Show("Parser.dll");
                Register(Location, Location + "modules\\", "parser.mod", false, true);//Parser.dll
                //				System.Windows.Forms.MessageBox.Show("DotNetMetaDataRepository.dll");
                Register(Location, Location + "modules\\", "dotnetmdr.mod", false, true);//DotNetMetaDataRepository.dll
                //				System.Windows.Forms.MessageBox.Show("PersistenceLayerRunTime.dll");
                Register(Location, Location + "modules\\", "plrt.mod", false, true);//PersistenceLayerRunTime.dll
                //				System.Windows.Forms.MessageBox.Show("MetaDataLoadingSystem.dll");
                Register(Location, Location + "modules\\", "mdatrt.mod", false, true);//MetaDataLoadingSystem.dll
                //				System.Windows.Forms.MessageBox.Show("RDBMSMetaDataRepository.dll");
                Register(Location, Location + "modules\\", "rdbmsmap.mod", false, true);//RDBMSMetaDataRepository.dll

                Register(Location, Location + "modules\\", "rdbmsmdrt.mod", false, true);//RDBMSMetaDataPersistenceRunTime.dll

                Register(Location, Location + "modules\\", "rdbmsrt.mod", false, true);//RDBMSPersistenceRunTime.dll


                //				System.Windows.Forms.MessageBox.Show("MSSQLPersistenceRunTime.dll");

                Register(Location, Location + "modules\\", "mssqlfrt.mod", false, true);//MSSQLFastPersistenceRunTime.dll

                Register(Location, Location + "modules\\", "mssqlrt.mod", false, true);//MSSQLPersistenceRunTime.dll
                Register(Location, Location, "Linq.dll", false, false); //Linq.dll

                //				System.Windows.Forms.MessageBox.Show("InstallUtil.exe");

                //Register(Location, Location + "modules\\", "CsOQLParser.mod", false, true);//MSSQLPersistenceRunTime.dll
                //				System.Windows.Forms.MessageBox.Show("InstallUtil.exe");


                string commandLine = Location + "InstallUtil.exe";
                arguments = "\"" + Location + "PersistencyService.exe\"";
                System.Diagnostics.ProcessStartInfo ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //ProcessStartInfo.UseShellExecute=false;
                ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process = System.Diagnostics.Process.Start(ProcessStartInfo);
                int Count = 0;
                while (!process.HasExited && Count < 15)
                {
                    Count++;
                    System.Threading.Thread.Sleep(200);
                }

                commandLine = Location + "InstallUtil.exe";
                arguments = "\"" + Location + "TransactionCoordinator.exe\"";
                ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //ProcessStartInfo.UseShellExecute=false;
                ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process = System.Diagnostics.Process.Start(ProcessStartInfo);
                Count = 0;
                while (!process.HasExited && Count < 15)
                {
                    Count++;
                    System.Threading.Thread.Sleep(200);
                }


                commandLine = Location + "InstallUtil.exe";
                arguments = "\"" + Location + "StorageServerInstanceLocator.exe\"";
                ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                //ProcessStartInfo.UseShellExecute=false;
                ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process = System.Diagnostics.Process.Start(ProcessStartInfo);
                Count = 0;
                while (!process.HasExited && Count < 15)
                {
                    Count++;
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (System.Exception Error)
            {
                System.Windows.Forms.MessageBox.Show(Error.Message + " " + Error.StackTrace);
                throw Error;
            }


        }

        private void AddToGac(string assemblyFile)
        {
            try
            {
                string commandLine = Location + "GACutil.exe";
                string arguments = "-if \"" + assemblyFile + "\"";

                System.Diagnostics.ProcessStartInfo ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //System.Windows.Forms.MessageBox.Show(commandLine+"  "+arguments);
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(ProcessStartInfo);
                long Count = 0;
                while (!process.HasExited && Count < 40)
                {
                    Count++;
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception error)
            {
            }
        }

        private void RemoveFromGac(string assemblyFile)
        {
            try
            {
                string commandLine = Location + "GACutil.exe";
                string arguments = "-u \"" + assemblyFile + "\"";

                System.Diagnostics.ProcessStartInfo ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(commandLine, arguments);
                ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //System.Windows.Forms.MessageBox.Show(commandLine+"  "+arguments);
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(ProcessStartInfo);
                long Count = 0;
                while (!process.HasExited && Count < 40)
                {
                    Count++;
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception error)
            {
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        string Location;
        /// <MetaDataID>{2D5B4D2E-9982-4ED5-8E96-BECC1AD981D4}</MetaDataID>
        public PersistencySystemInstaller()
        {
            //HKEY_CURRENT_USER\Software\Hydrogen\OOAdvanceTech
            Location = GetType().Assembly.Location;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(Location);
            Location = fileInfo.DirectoryName + "\\";
            //System.Windows.Forms.MessageBox.Show("Location");
            //Microsoft.Win32.RegistryKey appKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Hydrogen\OOAdvanceTech");
            //Location = appKey.GetValue("ApplicationPath") as string;
            //System.Windows.Forms.MessageBox.Show(Location);
            //appKey.Close(); 




            // This call is required by the Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <MetaDataID>{9F2DE431-7B0B-4BF9-94FA-5613901E5712}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// <MetaDataID>{5BCB7E36-3DF0-439A-9199-6F6955B2C4A3}</MetaDataID>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
