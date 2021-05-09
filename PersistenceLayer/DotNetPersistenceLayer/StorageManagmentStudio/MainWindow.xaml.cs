using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OOAdvantech.PersistenceLayer;

namespace StorageManagmentStudio
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// <MetaDataID>{2cbe09d3-8fcf-4c45-bd82-7a75700c1a66}</MetaDataID>
    public partial class MainWindow : Window
    {
        /// <MetaDataID>{7f0ae043-499f-4229-af3d-ab9de70df8de}</MetaDataID>
        public MainWindow()
        {

            InitializeComponent();


            Connection.Instance = new StorageServersManager();

            
            //string storageName = "Abstractions";
            //string storageLocation = @"localhost\Debug";
            //string storageType = "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider";
            //ObjectStorage objectStorage = null;
            try
            {
                //objectStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

                ////using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                ////{
                ////     storageSession.NewObject<AbstractionsAndPersistency.Client>();
                ////    //stateTransition.Consistent = true;
                ////}


                //var clients = (from client in new OOAdvantech.Linq.Storage(objectStorage).GetObjectCollection<AbstractionsAndPersistency.IClient>()
                //               select client).ToArray();

                //var vclient = clients.First();
                //DataContext = vclient;
                //AbstractionsAndPersistency.Client lClient = new AbstractionsAndPersistency.Client("Liakos_2");
                //OOAdvantech.UserInterface.Runtime.UIProxy uiProxy = new OOAdvantech.UserInterface.Runtime.UIProxy(lClient, lClient.GetType());
                //DataContext = uiProxy.GetTransparentProxy();
                //OOAdvantech.UserInterface.Runtime.UIProxy uiProxy = new OOAdvantech.UserInterface.Runtime.UIProxy(new StorageManagmentStudio.StorageServersManager(), typeof(StorageManagmentStudio.StorageServersManager));

                //DataContext = uiProxy.GetTransparentProxy();// as StorageManagmentStudio.StorageServersManager;



            }
            catch (Exception error)
            {


            }

        }

        /// <MetaDataID>{ba8736be-2d1e-47b4-8284-c5274faeee52}</MetaDataID>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //Image simpleImage = new Image();
            //simpleImage.Width = 200;
            //simpleImage.Margin = new Thickness(5);

            //// Create source.
            //BitmapImage bi = new BitmapImage();
            //// BitmapImage.UriSource must be in a BeginInit/EndInit block.
            //bi.BeginInit();
            //bi.UriSource = new Uri(@"/Resources/DataServer.png", UriKind.RelativeOrAbsolute);
            //bi.EndInit();
            //// Set the image source.
            //MyImage.Source = bi;

            //UpdateLayout();



        }

        /// <MetaDataID>{5301d2c6-dfba-4546-bd59-bd0195031fcc}</MetaDataID>
        private void Image_MouseUp()
        {

        }







    }
}
