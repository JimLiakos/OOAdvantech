using Microneme.OOAppDevToolBox;
using System.Windows;
using System.Windows.Controls;

namespace OOAppDevToolBox
{
    public partial class MyToolWindowControl : UserControl
    {
        public MyToolWindowControl()
        {
            InitializeComponent();
            Loaded += MyToolWindowControl_Loaded;
        }

        private void MyToolWindowControl_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
                if (ClassView.MetadataBrowserHost.MetadataRepositoryBrowser != null && (this.Content as ClassViewToolWindowControl).MetadataBrowserHost.MetadataRepositoryBrowser.DTE == null)
                {

                    ClassView.MetadataBrowserHost.MetadataRepositoryBrowser.DTE = VisualStudioEventBridge.VisualStudioEvents.DTEObject as EnvDTE.DTE;
                }
            }
            catch (Exception error)
            {
            }

        }
    

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            VS.MessageBox.Show("OOAppDevToolBox", "Button clicked");
        }
    }
}