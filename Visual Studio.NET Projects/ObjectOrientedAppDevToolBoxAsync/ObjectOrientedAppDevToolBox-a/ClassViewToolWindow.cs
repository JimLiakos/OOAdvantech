using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace OOAppDevToolBox
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("1b465c26-cd6b-4022-ba5b-a2c7ef9df285")]
    public class ClassViewToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassViewToolWindow"/> class.
        /// </summary>
        public ClassViewToolWindow() : base(null)
        {
            this.Caption = "OOAdvantech";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new ClassViewToolWindowControl();
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            try
            {
                if ((this.Content as ClassViewToolWindowControl).MetadataBrowserHost.MetadataRepositoryBrowser != null && (this.Content as ClassViewToolWindowControl).MetadataBrowserHost.MetadataRepositoryBrowser.DTE == null)
                {
                    EnvDTE.DTE dte = (this.GetService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE))) as EnvDTE.DTE;
                    (this.Content as ClassViewToolWindowControl).MetadataBrowserHost.MetadataRepositoryBrowser.DTE = dte;
                }
            }
            catch (Exception error)
            {
            }

        }
    }
}
