using System.Windows;
using System.Windows.Controls;

namespace OOAppDevToolBox
{
    public partial class MyToolWindowControl : UserControl
    {
        public MyToolWindowControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            VS.MessageBox.Show("OOAppDevToolBox", "Button clicked");
        }
    }
}