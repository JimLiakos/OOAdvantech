using OOAdvantech.UserInterface.Runtime;
using System.Windows.Controls;
using OOAdvantech.Transactions;

using System.DirectoryServices;
using System;


namespace StorageManagmentStudio
{
    /// <MetaDataID>{87b31064-4e54-484b-adf2-29fa05dc93d8}</MetaDataID>
    public class StorageMetadataNodePresentation : PresentationObject<IStorageMetadataNode>
    {


        
        public string Name
        {
            get
            {
                return RealObject.Name;
            }
        }
        public StorageMetadataNodePresentation()
            : base(default(IStorageMetadataNode))
        {
        }

        public StorageMetadataNodePresentation(IStorageMetadataNode storageMetadataNode)
            : base(storageMetadataNode)
        {
            //Enum.GetValues(AbstractionsAndPersistency.OrderState)
        }
        ContextMenu _ContextMenu;
        public ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null && RealObject is StoragesFolder)
                {
                    _ContextMenu = new ContextMenu();
                    MenuItem menuItem = new MenuItem() { Header = "Attach Storage" };
                    menuItem.Click += new System.Windows.RoutedEventHandler(AddStorage_Click);
                    _ContextMenu.Items.Add(menuItem);
                }
                if (_ContextMenu == null && RealObject is UsersFolder)
                {
                    _ContextMenu = new ContextMenu();
                    MenuItem menuItem = new MenuItem() { Header = "New User" };
                    menuItem.Click += new System.Windows.RoutedEventHandler(NewUser_Click);
                    _ContextMenu.Items.Add(menuItem);
                }
                return _ContextMenu;
            }
            set
            {

            }
        }

        public class Wpf32Window : System.Windows.Forms.IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public Wpf32Window(System.Windows.Window wpfWindow)
            {
                Handle = new System.Windows.Interop.WindowInteropHelper(wpfWindow).Handle;
            }
        }

        void NewUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                NewUserWindows newUserWindows = new NewUserWindows();
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject((RealObject as UsersFolder).StorageServer);
                OOAdvantech.Security.User user = objectStorage.NewObject<OOAdvantech.Security.User>();

                newUserWindows.Connection.UserInterfaceObjectConnection.Instance = user;

                System.Diagnostics.Debug.WriteLine(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
                if (newUserWindows.ShowDialog().Value)
                {
                    
                    (RealObject as UsersFolder).StorageServer.AddUser(user);
                    stateTransition.Consistent = true;
                }

                
            }
            (RealObject as UsersFolder).Refresh();
            return;


            //ObjectTypes allowedTypes = ObjectTypes.None;
            //allowedTypes |= ObjectTypes.Users;
            //allowedTypes |= ObjectTypes.Groups;
            //allowedTypes |= ObjectTypes.Contacts;
            //allowedTypes |= ObjectTypes.Computers;
            //ObjectTypes defaultTypes = ObjectTypes.None;
            //defaultTypes |= ObjectTypes.Users;
            //defaultTypes |= ObjectTypes.Groups;
            //defaultTypes |= ObjectTypes.Contacts;
            //defaultTypes |= ObjectTypes.Computers;
            //Locations allowedLocations = Locations.None;
            //allowedLocations |= Locations.All;
            //Locations defaultLocations = Locations.None;
            //defaultLocations |= Locations.JoinedDomain;


            //// Show dialog
            //DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog();
            //picker.AllowedObjectTypes = allowedTypes;
            //picker.DefaultObjectTypes = defaultTypes;
            //picker.AllowedLocations = allowedLocations;
            //picker.DefaultLocations = defaultLocations;
            ////picker.MultiSelect = chkMultiSelect.Checked;
            ////picker.TargetComputer = txtTargetComputer.Text;
            //try
            //{
                
            //    System.Windows.Forms.DialogResult dialogResult = picker.ShowDialog(new Wpf32Window(System.Windows.Window.GetWindow(this.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject)));
            //  if (dialogResult == System.Windows.Forms.DialogResult.OK)
            //    {
            //        DirectoryObject[] results;
            //        results = picker.SelectedObjects;
            //        DirectoryEntry localMachine = new DirectoryEntry(results[0].Path);
            //        foreach (var prop in localMachine.Properties.PropertyNames)
            //        {

            //        }

            //    }
            //}
            //catch (System.Exception error)
            //{


            //}
        }
        void AddStorage_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            UserInterfaceObjectConnection userInterfaceObjectConnection = WPFUIElementObjectBind.ViewControlObject.GetUserInterfaceObjectConnection(sender as System.Windows.FrameworkElement);
            using (SystemStateTransition stateTransition = new SystemStateTransition(userInterfaceObjectConnection.Transaction))
            {
                (RealObject as StoragesFolder).AddStorage();
               

                stateTransition.Consistent = true;
            }

            //UserInterfaceObjectConnection.GetUserInterfaceObjectConnection(sender as System.Windows.FrameworkElement);
        }
    }
}

