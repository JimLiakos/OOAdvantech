using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

using EnvDTE;
using Microsoft.Win32;
using System.Security.Principal;
using OOAdvantech.Security;

namespace MsdevManager
{
    /// <MetaDataID>{e044af34-daf3-4a87-8bdf-dce50627f31e}</MetaDataID>
    public enum ROTFlags
    {
        REGISTRATIONKEEPSALIVE = 0x1,
        ALLOWANYCLIENT = 0x2
    }
    /// <summary>
    /// Utility class to get you a list of the running instances of the Microsoft Visual 
    /// Studio IDE.  The list is obtained by looking at the system's Running Object Table (ROT)
    /// </summary>
    /// Other ways to get a pointer to a VisualStudio instance:
    /// EnvDTE.DTE dte = (EnvDTE.DTE) System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.7.1");
    /// <MetaDataID>{c906b7fd-7823-40d8-944c-3f4711cc01e1}</MetaDataID>

    public class Msdev
    {
        #region Interop imports

        /// <MetaDataID>{90ebda4b-0edb-472b-8762-c3d86ae71895}</MetaDataID>
        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved, out UCOMIRunningObjectTable prot);
        /// <MetaDataID>{9c69785d-cad5-47e2-8756-5ff14569d9d7}</MetaDataID>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
        public static extern int CreateFileMoniker(string pathName, out UCOMIMoniker pMoniker);

        /// <MetaDataID>{302533ad-dbac-4dfa-b59a-20654a3018a8}</MetaDataID>
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out UCOMIBindCtx ppbc);

        /// <MetaDataID>{8113c847-f50e-4f59-b194-6e047466eb58}</MetaDataID>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <MetaDataID>{7386f260-781e-4640-8d8d-9250f6d2b1e7}</MetaDataID>
        private const int SW_RESTORE = 9;
        /// <MetaDataID>{a2333c61-cdaa-4904-b4d3-9fe0d7fb4153}</MetaDataID>
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <MetaDataID>{6275f2af-d6b4-4882-b0db-19c70e489a6a}</MetaDataID>
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        #endregion

        /// <MetaDataID>{5d676014-1429-40e7-91e6-b45346ba5d65}</MetaDataID>
        static System.Collections.Generic.Dictionary<EnvDTE.DTE, string> IDEsProcessIDs = new System.Collections.Generic.Dictionary<DTE, string>();
        /// <MetaDataID>{53139546-c3e5-449b-b6fb-e1347dfbaa67}</MetaDataID>
        public static string GetIDEProcessID(EnvDTE.DTE ide)
        {
            if (IDEsProcessIDs.ContainsKey(ide))
                return IDEsProcessIDs[ide];
            Hashtable runningInstances = GetIDEInstances(true);
            IDictionaryEnumerator enumerator = runningInstances.GetEnumerator();

            while (enumerator.MoveNext())
            {
                DTE currentIDE = (DTE)enumerator.Value;
                if (ide == currentIDE)
                {
                    string description = enumerator.Key as string;
                    if (description.IndexOf(":") != -1)
                    {
                        string processId = description.Substring(description.IndexOf(":") + 1);
                        IDEsProcessIDs.Add(ide, processId);
                        return processId;
                    }
                    
                }
            }
            return null;

        }

        public static System.Collections.Generic.List<string> GetVS2022PIDS()
        {

            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

            int numFetched;
            UCOMIRunningObjectTable runningObjectTable;
            UCOMIEnumMoniker monikerEnumerator;
            UCOMIMoniker[] monikers = new UCOMIMoniker[1];


            GetRunningObjectTable(0, out runningObjectTable);

            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, out numFetched) == 0)
            {
                try
                {
                    UCOMIBindCtx ctx;
                    CreateBindCtx(0, out ctx);

                    string runningObjectName;
                    monikers[0].GetDisplayName(ctx, null, out runningObjectName);
                    Guid clsid;

                    if (runningObjectName.IndexOf("!Microsoft Visual Studio Telemetry:") == 0)
                        result.Add(runningObjectName.Replace("!Microsoft Visual Studio Telemetry:", ""));
                    
                }
                catch (System.Exception error)
                {

                }
            }

            return result;
        }
        /// <summary>
        /// Get the DTE object for the instance of Visual Studio IDE that has 
        /// the specified solution open.
        /// </summary>
        /// <param name="solutionFile">The absolute filename of the solution</param>
        /// <returns>Corresponding DTE object or null if no such IDE is running</returns>
        /// <MetaDataID>{e2bba80a-8941-4b4c-a49b-c0877a508cb5}</MetaDataID>
        public static EnvDTE.DTE GetIDEInstance(string solutionFile)
        {
            Hashtable runningInstances = GetIDEInstances(true);
            IDictionaryEnumerator enumerator = runningInstances.GetEnumerator();
            _DTE ide = null;
            string SolName = "tmp";
            while (enumerator.MoveNext())
            {

                try
                {
                    ide = (_DTE)enumerator.Value;
                    if (ide != null)
                    {
                        SolName = ide.Solution.FullName;
                        System.Diagnostics.Debug.WriteLine(SolName);
                        if (ide.Solution.FullName.Trim().ToLower() == solutionFile.Trim().ToLower())
                        {
                            return (EnvDTE.DTE)ide;
                        }
                    }
                }
                catch { }
            }
            ide = null;
            if (System.IO.File.Exists(solutionFile))
            {
                try
                {
                    var vs2022Pids = GetVS2022PIDS();
                    foreach (string vs2022Pid in vs2022Pids)
                    {
                        try
                        {
                            var idManager = OOAdvantech.CodeMetaDataRepository.Project.GetIDEManager(vs2022Pid);
                            var ss = idManager.Solution.FullName;

                        }
                        catch (Exception error)
                        {

                            
                        }
                    }


                    RegistryKey devKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Rational Software\\Rose\\AddIns\\UML TO VS");

                    string idePath = (string)devKey.GetValue("VSPath");
                    System.Diagnostics.Process process = null;
                    if (!string.IsNullOrWhiteSpace(idePath))
                    {
                        process = new System.Diagnostics.Process();
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.Arguments = solutionFile;
                        process.StartInfo.FileName = idePath;
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.Verb = "runas";
                        process.Start();
                    }
                    else
                        process = System.Diagnostics.Process.Start(solutionFile);
                    int count = 60;
                    while (count > 0)
                    {
                        System.Threading.Thread.Sleep(500);
                        count--;

                        runningInstances = GetIDEInstances(true);
                        enumerator = runningInstances.GetEnumerator();

                        while (enumerator.MoveNext())
                        {

                            try
                            {
                                ide = (_DTE)enumerator.Value;
                                if (ide != null)
                                {
                                    if (ide.Solution.FullName == solutionFile)
                                    {
                                        return (EnvDTE.DTE)ide;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch (System.Exception error)
                {

                }
            }
            return null;
        }

        /// <summary>
        /// Raises an instance of the Visual Studio IDE to the foreground.
        /// </summary>
        /// <param name="ide">The DTE object for the IDE you would like to raise to the foreground</param>
        /// <MetaDataID>{1f7151dc-ad07-476a-a6a0-c6167f618730}</MetaDataID>
        public static void ShowIDE(EnvDTE.DTE ide)
        {
            // To show an existing IDE, we get the HWND for the MainWindow
            // and do a little interop to bring the desired IDE to the
            // foreground.  I tried some of the following other potentially
            // promising approaches but could only succeed in getting the
            // IDE's taskbar button to flash.  Ex:
            //
            //   ide.MainWindow.Activate();
            //   ide.MainWindow.SetFocus();
            //   ide.MainWindow.Visible = true;
            //   ide.MainWindow.WindowState = EnvDTE.vsWindowState.vsWindowStateMinimize;
            //   ide.MainWindow.WindowState = EnvDTE.vsWindowState.vsWindowStateMaximize;

            System.IntPtr hWnd = (System.IntPtr)ide.MainWindow.HWnd;
            if (IsIconic(hWnd))
            {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }
            SetForegroundWindow(hWnd);
            ide.MainWindow.Visible = true;
        }

        /// <MetaDataID>{33252c4b-4d5e-4a1b-8a20-dace3eb686e7}</MetaDataID>
        public static void ShowIDE(string solutionFile)
        {
            EnvDTE.DTE ide = Msdev.GetIDEInstance(solutionFile);
            if (ide != null)
            {
                ShowIDE(ide);
            }
            else
            {
                // To create a new instance of the IDE, opened to the selected solution we
                // could try:
                // 
                //   Type dteType = Type.GetTypeFromProgID("VisualStudio.DTE.7.1");
                //   EnvDTE.DTE dte = Activator.CreateInstance(dteType) as EnvDTE.DTE;
                //   dte.MainWindow.WindowState = EnvDTE.vsWindowState.vsWindowStateMaximize;
                //   dte.MainWindow.Visible = true;
                //   dte.Solution.Open( solutionFile.Filename );
                //
                // This works but the new devenv.exe process does not exit when you close the
                // IDE.  You could then just reattach as described and the closed IDE would 
                // quickly redisplay (possibly useful as a feature).
                //
                // Instead we lookup the path to the IDE executable in the registry and
                // just start another process.

                RegistryKey devKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Rational Software\\Rose\\AddIns\\UML TO VS");
                
                string idePath = (string)devKey.GetValue("VSPath");

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.RedirectStandardOutput = false;
                p.StartInfo.Arguments = solutionFile;
                p.StartInfo.FileName = idePath;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
        }

        /// <MetaDataID>{fa30c0a7-c20e-4f15-8859-da22b93fe376}</MetaDataID>
        public static System.Collections.Generic.List<EnvDTE.Solution> GetOpenSolutions()
        {
            System.Collections.Generic.List<EnvDTE.Solution> Solutions = new System.Collections.Generic.List<Solution>();

            foreach (System.Collections.DictionaryEntry entry in GetIDEInstances(true))
                Solutions.Add((entry.Value as EnvDTE.DTE).Solution);
            return Solutions;
        }
        /// <summary>
        /// Get a table of the currently running instances of the Visual Studio .NET IDE.
        /// </summary>
        /// <param name="openSolutionsOnly">Only return instances that have opened a solution</param>
        /// <returns>A hashtable mapping the name of the IDE in the running object table to the corresponding DTE object</returns>
        /// <MetaDataID>{bc8b5615-1a13-4632-8398-d8efc397f026}</MetaDataID>
        public static Hashtable GetIDEInstances(bool openSolutionsOnly)
        {
            Hashtable runningIDEInstances = new Hashtable();

            Hashtable runningObjects = GetRunningObjectTable();

            IDictionaryEnumerator rotEnumerator = runningObjects.GetEnumerator();
            while (rotEnumerator.MoveNext())
            {
                string candidateName = (string)rotEnumerator.Key;
                if (!candidateName.StartsWith("!VisualStudio.DTE"))
                    continue;

                _DTE ide = rotEnumerator.Value as _DTE;
                _Solution solution = rotEnumerator.Value as _Solution;
                if (ide == null)
                    continue;

                if (openSolutionsOnly)
                {
                    try
                    {
                        string solutionFile = null;
                        if (ide != null)
                            solutionFile = ide.Solution.FullName;

                        if (solution != null)
                            solutionFile = solution.FullName;


                        if (solutionFile != String.Empty)
                        {
                            if (ide != null)
                                runningIDEInstances[candidateName] = ide;
                            if (solution != null)
                                runningIDEInstances[candidateName] = solution.DTE;
                        }
                    }
                    catch { }
                }
                else
                {
                    if (ide != null)
                        runningIDEInstances[candidateName] = ide;
                    if (solution != null)
                        runningIDEInstances[candidateName] = solution.DTE;
                }
            }
            return runningIDEInstances;
        }



        /// <summary>
        /// Get a snapshot of the running object table (ROT).
        /// </summary>
        /// <returns>A hashtable mapping the name of the object in the ROT to the corresponding object</returns>
        /// <MetaDataID>{c363a972-b6a8-4d6c-b099-b9413930d0bd}</MetaDataID>
        [STAThread]
        public static Hashtable GetRunningObjectTable()
        {

            Hashtable result = new Hashtable();

            int numFetched;
            UCOMIRunningObjectTable runningObjectTable;
            UCOMIEnumMoniker monikerEnumerator;
            UCOMIMoniker[] monikers = new UCOMIMoniker[1];


            GetRunningObjectTable(0, out runningObjectTable);

            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, out numFetched) == 0)
            {
                try
                {
                    UCOMIBindCtx ctx;
                    CreateBindCtx(0, out ctx);

                    string runningObjectName;
                    monikers[0].GetDisplayName(ctx, null, out runningObjectName);
                    Guid clsid;

                    object runningObjectVal;
                    runningObjectTable.GetObject(monikers[0], out runningObjectVal);
                    if (runningObjectVal is EnvDTE.DTE || runningObjectVal is EnvDTE._Solution)
                        result[runningObjectName] = runningObjectVal;
                    else
                    {
                        var isSolution = runningObjectVal is EnvDTE._Solution;
                        Marshal.ReleaseComObject(runningObjectVal);
                    }
                }
                catch (System.Exception error)
                {

                }
            }

            return result;
        }

        /// <MetaDataID>{dce92be7-3648-4fc0-8ecc-518737bd35e1}</MetaDataID>
        public static bool CompareInstances(Hashtable instances1, Hashtable instances2)
        {
            bool changed = false;
            foreach (string instances1Key in instances1.Keys)
            {
                if (!instances2.ContainsKey(instances1Key))
                {
                    changed = true;
                    break;
                }
            }

            if (!changed)
            {
                foreach (string instances2Key in instances2.Keys)
                {
                    if (!instances1.ContainsKey(instances2Key))
                    {
                        changed = true;
                        break;
                    }
                }
            }

            return changed;
        }
    }

    /// <MetaDataID>{f524e0a5-9934-40f0-bfa2-3acd089e61d4}</MetaDataID>
    public class MsdevMonitorThread
    {
        public delegate void MonitorMsdevHandler();
        public event MonitorMsdevHandler Changed;

        /// <MetaDataID>{fa184fce-9836-4bdd-9058-a299e5789b6e}</MetaDataID>
        private System.Threading.Thread m_thread = null;
        //    private ISynchronizeInvoke m_invokeObject = null;
        /// <MetaDataID>{c38be570-6b03-423a-a48d-dbbcc055bbad}</MetaDataID>
        private int m_period = 2000;
        /// <MetaDataID>{fd1ff936-ac11-415d-bbc4-5fe5ceebc39f}</MetaDataID>
        private bool m_isRunning = false;
        /// <MetaDataID>{c65879f3-923b-47a0-ba38-c29cb65704b3}</MetaDataID>
        private bool m_openSolutionsOnly = false;

        /// <MetaDataID>{1840764e-4ea9-4e77-bb4b-1730578e7756}</MetaDataID>
        public MsdevMonitorThread(bool openSolutionsOnly)
        {
            //  m_invokeObject = invokeObject;
            m_openSolutionsOnly = openSolutionsOnly;
        }

        /// <MetaDataID>{4f51af4d-cb67-4203-8ea9-e429474a60ba}</MetaDataID>
        ~MsdevMonitorThread()
        {
            Stop();
        }

        /// <MetaDataID>{c25b6d73-6f85-4bfa-9152-2d2b5583fa0e}</MetaDataID>
        public void Start()
        {
            m_isRunning = true;
            if (m_thread == null)
                m_thread = new System.Threading.Thread(new ThreadStart(ThreadMain));
            m_thread.Start();
        }

        /// <MetaDataID>{079f4076-7de6-4f16-8957-fc94f3c3a8cb}</MetaDataID>
        public void Stop()
        {
            m_isRunning = false;
            m_thread = null;
        }

        /// <MetaDataID>{55b24444-d620-4c60-9a7b-465f272bf008}</MetaDataID>
        private void ThreadMain()
        {
            // Take a snapshot of the currently running instances of Visual Studio
            // We'll also separately keep track of the solution files that each
            // instance has open at this time.  We'll use it to detect when an 
            // IDE has loaded or unloaded a solution.

            Hashtable snapshotInstances = Msdev.GetIDEInstances(m_openSolutionsOnly);
            Hashtable snapshotSolutions = new Hashtable();
            foreach (string snapshotKey in snapshotInstances.Keys)
            {
                string solutionFile = String.Empty;
                try
                {
                    EnvDTE.DTE ide = (EnvDTE.DTE)snapshotInstances[snapshotKey];
                    solutionFile = ide.Solution.FullName;
                }
                catch { }

                snapshotSolutions[snapshotKey] = solutionFile;
            }

            // We'll just keep looping in this thread, periodically checking the
            // currently running list of IDE's.  If there is any change we'll 
            // raise a Changed event.

            while (m_isRunning)
            {
                System.Threading.Thread.Sleep(m_period);
                if (Changed != null)
                {
                    Hashtable currentInstances = Msdev.GetIDEInstances(m_openSolutionsOnly);
                    bool changed = Msdev.CompareInstances(snapshotInstances, currentInstances);
                    if (changed)
                    {
                        //m_invokeObject.BeginInvoke(Changed, null);
                        Changed();
                        snapshotInstances = currentInstances;
                    }
                    else
                    {
                        foreach (string currentKey in currentInstances.Keys)
                        {
                            string prevSolutionFile = (string)snapshotSolutions[currentKey];
                            string currentSolutionFile = String.Empty;
                            try
                            {
                                EnvDTE.DTE ide = (EnvDTE.DTE)currentInstances[currentKey];
                                currentSolutionFile = ide.Solution.FullName;
                            }
                            catch { }
                            if (prevSolutionFile != currentSolutionFile)
                            {
                                //m_invokeObject.BeginInvoke(Changed, null);
                                Changed();
                                snapshotInstances = currentInstances;
                                snapshotSolutions[currentKey] = currentSolutionFile;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
