global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Threading;
using Microneme.OOAppDevToolBox;
using OOAdvantech.MetaDataRepository;
using Microsoft.VisualStudio;

namespace OOAppDevToolBox
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(MyToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.OOAppDevToolBoxString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]

    public sealed class OOAppDevToolBoxPackage : ToolkitPackage, VSMetadataRepositoryBrowser.IVSPackage
    {
        /// <summary>
        /// OOAppDevToolBoxPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "d1ec41a7-36ea-4544-ae45-052b1e2ba477";

        internal static VSMetadataRepositoryBrowser.IVSPackage VSPackage;
        #region Package Members

        ///// <summary>
        ///// Initialization of the package; this method is called right after the package is sited, so this is the place
        ///// where you can put all the initialization code that rely on services provided by VisualStudio.
        ///// </summary>
        ///// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        ///// <param name="progress">A provider for progress updates.</param>
        ///// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        //protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        //{
        //    // When initialized asynchronously, the current thread may be a background thread at this point.
        //    // Do any initialization that requires the UI thread after switching to the UI thread.

        //    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        //    System.Windows.Forms.MessageBox.Show("Hello");
        //    await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        //}


        public void ShowORMToolWindow(MetaObject metaObject)
        {
            try
            {
                //Microneme.MetaDataRepository.RDBMSMappingToolWindows.RDBMSMappingToolWindow window = this.FindToolWindow(typeof(Microneme.MetaDataRepository.RDBMSMappingToolWindows.RDBMSMappingToolWindow), 0, true) as Microneme.MetaDataRepository.RDBMSMappingToolWindows.RDBMSMappingToolWindow;
                ////ToolWindowPane window = this.CreateToolWindow(typeof(Company.MetaDataRepository.RDBMSMappingToolWindows.RDBMSMappingToolWindow), 0);

                //window.ShowMetaObject(metaObject);
                //if ((window == null) || (window.Frame == null))
                //{
                //    throw new COMException("Can not create window.");
                //}

                //IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                //Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
            catch (Exception error)
            {
            }
        }


        #region Package Members

        DTEConnection DTEConnection;


        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {

     
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            //System.Windows.Forms.MessageBox.Show("Hello");

            await this.RegisterCommandsAsync();
            this.RegisterToolWindows();

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            VSPackage = this;



            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);



            try
            {
                await ClassViewToolWindowCommand.InitializeAsync(this);

                if (DTEConnection == null)
                {
                    DTEConnection = new DTEConnection();
                    EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
                    DTEConnection.OnConnection(dte, this);
                }


                IVsSolution pSolution = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
                IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();
                var solution = IDEManager.Solution;

            }
            catch (Exception error)
            {

            }
        }



        static OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager;
        #endregion

        #endregion
    }
}