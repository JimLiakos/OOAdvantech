﻿using Microneme.ObjectOrientedAppsDevToolBox;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OOAdvantech.MetaDataRepository;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace OOAppDevToolBox
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(ObjectOrientedAppDevToolBoxPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]

    [ProvideToolWindow(typeof(ClassViewToolWindow))]
    public sealed class ObjectOrientedAppDevToolBoxPackage : AsyncPackage, VSMetadataRepositoryBrowser.IVSPackage//, IVsSolutionEvents
    {
        /// <summary>
        /// ObjectOrientedAppDevToolBoxPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "94548e33-84ec-4142-9037-a80ee50d84ad";
        internal static VSMetadataRepositoryBrowser.IVSPackage VSPackage;

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
            VSPackage = this;

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ClassViewToolWindowCommand.InitializeAsync(this);



            if (DTEConnection == null)
            {
                DTEConnection = new DTEConnection();

                EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
                
                DTEConnection.OnConnection(dte, this);
            }



            IVsSolution pSolution = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;

            IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();

            //pSolution.AdviseSolutionEvents

        }



        static OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager;
        #endregion
    }

    
}
