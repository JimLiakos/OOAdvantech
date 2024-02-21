
using EnvDTE;
using Microneme.ObjectOrientedAppsDevToolBox;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace ObjectOrientedAppDevToolBox
{
    /// <summary>
    /// Interaction logic for ClassViewToolWindowControl.
    /// </summary>
    public partial class ClassViewToolWindowControl : UserControl
    {
        internal MetadataBrowserHost MetadataBrowserHost;
        private DTE EnvDTE;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassViewToolWindowControl"/> class.
        /// </summary>
        public ClassViewToolWindowControl()
        {
            this.InitializeComponent();
            MetadataBrowserHost = new MetadataBrowserHost();
            MetadataBrowserHost.LoadMetadataRepositoryBrowser(ObjectOrientedAppDevToolBoxPackage.VSPackage);
            WinformHost.Child = MetadataBrowserHost;
            Loaded += ClassViewToolWindowControl_Loaded;
            EnvDTE= ObjectOrientedAppDevToolBoxPackage.VSPackage.DTEConnection.DTEObject;
        }

        private void ClassViewToolWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MetadataBrowserHost.MetadataRepositoryBrowser != null && MetadataBrowserHost.MetadataRepositoryBrowser.DTE == null)
                {

                    MetadataBrowserHost.MetadataRepositoryBrowser.DTE = EnvDTE;
                }
            }
            catch (Exception error)
            {
            }
        }



        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ClassViewToolWindow");
        }


    }
}