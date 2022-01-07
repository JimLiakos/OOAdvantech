using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OOAdvantech.PersistenceLayer;
namespace UserInterfaceTest
{

    /// <MetaDataID>{63248839-1e7f-4941-8209-df3513152967}</MetaDataID>
    public partial class Form3 : Form
    {

        /// <MetaDataID>{8bf5e6c4-d35d-4f5c-9e39-87bb4d267d2a}</MetaDataID>
        public static ObjectStorage OpenStorage()
        {
            //string storageLocation = @"localhost\sqlexpress";
            //string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";
            return ObjectStorage.OpenStorage("Abstractions",
              //@"e:\Abstractions.xml",
              //"OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
              @"localhost",
              "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
            //@"localhost\sqlexpress",
            //     "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider");
        }
        /// <MetaDataID>{b8470d4f-939c-464d-9d21-8e57fd182b2d}</MetaDataID>
        [STAThread]
        static void Main()

        {

            System.Windows.Forms.MessageBox.Show(System.Environment.UserName);
            //System.Windows.Forms.OpenFileDialog ff = new System.Windows.Forms.OpenFileDialog();
            //ff.ShowDialog();


            //Type type= (ReportsData.OrdersReportData.Result as System.Linq.IQueryable).ElementType;

            //OOAdvantech.Transactions.Transaction trans = new OOAdvantech.Transactions.CommittableTransaction();
            //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(trans))
            //{
            //    bool tt = OOAdvantech.Transactions.Transaction.Current == trans;
            //    System.Collections.Generic.Dictionary<object, object> test = new Dictionary<object, object>();
            //    test.Add(OOAdvantech.Transactions.Transaction.Current," trans");

            //    bool er = test.ContainsKey(trans);


            //    stateTransition.Consistent = true;
            //}


            //System.Reflection.Assembly asa=System.Reflection.Assembly.LoadFile(@"X:\source\OpenVersions\PersistenceLayer\DotNetPersistenceLayer\DevicePersistencyCore\bin\Debug\DevicePersistencyCore.dll");
            //string PublicKey="";
            //foreach (byte mByte in asa.GetName().GetPublicKey())
            //{
            //    PublicKey += mByte.ToString("X2");
            //}
            OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrderDetail> ert = new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrderDetail>();



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new Form1());

            //try
            //{
            //    ObjectStorage objectStorage = null;
            //    objectStorage = ObjectStorage.OpenStorage("Abstractions",
            //                                                @"c:\Abstractions.xml",
            //                                                "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            //    string objectQuery = "#OQL: SELECT order " +
            //                      " FROM AbstractionsAndPersistency.IOrder order #";

            //    StructureSet objectSet = objectStorage.Execute(objectQuery);
            //    AbstractionsAndPersistency.IOrder order = null;
            //    foreach (StructureSet objectSetInstance in objectSet)
            //    {
            //        order = objectSet["order"] as AbstractionsAndPersistency.IOrder;
            //        break;

            //    }

            //    Application.Run(new Form3(order));
            //}
            //catch (Exception error)
            //{


            //}

        }

        /// <MetaDataID>{e25117ab-356e-4b43-8bdc-3ae62aca6fdb}</MetaDataID>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        /// <MetaDataID>{02bcedd0-cc13-4394-9064-eb3f4dc3a25f}</MetaDataID>
        public Form3()
        {
            InitializeComponent();
        }
        /// <MetaDataID>{bb26cb68-8b02-46c3-bd67-6feec2ee8e7d}</MetaDataID>
        public Form3(object _object)
        {
            InitializeComponent();
            Connection.ContainerControl = this;
            Connection.Instance = _object;


        }

        /// <MetaDataID>{790b38e4-1b89-499f-b130-b0a04d607aae}</MetaDataID>
        private void TestBtn_Click(object sender, EventArgs e)
        {

        }

        //private void TestBtn_Click(object sender, EventArgs e)
        //{
        //    viewControlObject1.Instance = new AbstractionsAndPersistency.Order();
        //    viewControlObject1.ContainerControl = this;

        //}

        //private void Save_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}

        //private void Form3_Load(object sender, EventArgs e)
        //{

        //    (List.ColumnModel.Columns[5].Editor as XPTable.Editors.ComboBoxCellEditor).Items.AddRange(new string[] { "Blues", "Classical", "Comedy", "Rock", "Other" });

        //}
    }


}