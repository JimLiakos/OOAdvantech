using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;
using AbstractionsAndPersistency;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace UserInterfaceTest
{
    /// <MetaDataID>{5b362e44-5775-44c2-9e66-dd5a063dd781}</MetaDataID>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.HandleCreated += new EventHandler(Form1_HandleCreated);
            this.Load += new EventHandler(Form1_Load);
            
        }

        void Form1_Load(object sender, EventArgs e)
        {
            
        }

        void Form1_HandleCreated(object sender, EventArgs e)
        {
            
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        AbstractionsAndPersistency.IOrder order = null;
        private void button1_Click(object sender, EventArgs e)
        {
            


         
            try
            {
                //PrintDialog te = new PrintDialog();
                //te.ShowDialog();

                //if (order != null)
                //{
                //    //Form2 formA = new Form2();
                //    //formA.ViewControlObject.Instance = order;

                //    //formA.Show();
                //    DevExpressForm dxForm = new DevExpressForm();
                //    dxForm.Connection.Instance = order;
                //    dxForm.Show();

                //    return;
                //}
 

                ObjectStorage objectStorage = null;
                objectStorage = Form3.OpenStorage();
              //@"localhost\Debug",
              //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                //objectStorage = ObjectStorage.OpenStorage("Abstractions",
                //                                            @"c:\Abstractions.xml",
                //                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");



                //objectStorage = ObjectStorage.NewStorage("Abstractions",
                //                                            @"c:\Abstractions.xml",
                //                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider",true);

                //AbstractionsAndPersistency.Order morder = new AbstractionsAndPersistency.Order();
                //objectStorage.CommitTransientObjectState(morder);
                //Form2 forma = new Form2();
                //forma.ViewControlObject.Instance = morder;
                //forma.ViewControlObject.ContainerControl = forma;
                //forma.ShowDialog();
                //return;
                   


 


                string objectQuery = "SELECT [order] " +
                                  " FROM AbstractionsAndPersistency.IOrder [order] ";


                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var result = from morder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                             select morder;

                order = result.ToList()[0]; 


                //StructureSet objectSet = objectStorage.Execute(objectQuery);

                
                //foreach (StructureSet objectSetInstance in objectSet)
                //{
                //    order = objectSet["order"] as AbstractionsAndPersistency.IOrder;
                //    break;

                //}

                //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                //var result = from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                //             where theOrder == order
                //             select theOrder;

                //foreach (var var in result)
                //{

                //}
             

                //NickTestForm test = new NickTestForm();
                //test.ShowDialog();

                //Form3 mform = new Form3();
                //mform.Connection.Instance = order;
                //mform.ShowDialog();
                //return;

                //Form6 mform =new Form6();
                //mform.Connection.Instance = order;
                //mform.ShowDialog();
                //return;


                //DevExpressForm dxForma = new DevExpressForm();
                //dxForma.Connection.Instance = order;
                //dxForma.ShowDialog();
                //dxForma.Dispose();

                //DevExpressForm dxFormb = new DevExpressForm();
                //dxFormb.Connection.Instance = order;
                //dxFormb.Show();
                //return;
 
                 

                Form2 form = new Form2();
                form.ViewControlObject.Instance = order;
                form.ViewControlObject.ContainerControl = form;
                form.Show();


                //Controls.Add(form); 

                 
            }
            catch (Exception error)
            {


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            GC.Collect();
            GC.WaitForPendingFinalizers();
            return;
            //TestEvent testEvent = new TestEvent();
          //  button2.Click+=new EventHandler(testEvent.mbutton_Click);
           // testEvent = null;
            ClientsList form = new ClientsList();
            

            form.Show();


            



            return;



            Bitmap myBitmaq = new Bitmap(@"c:\R1200GS.JPG");
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();


            myBitmaq.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] asa = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(asa, 0, (int)memStream.Length);

            System.Xml.Serialization.XmlSerializer xmlSerializear = new System.Xml.Serialization.XmlSerializer(typeof(byte[]));

            System.IO.StringWriter dss = new System.IO.StringWriter();
            xmlSerializear.Serialize(dss, asa);
            System.IO.StringReader reader = new System.IO.StringReader(dss.ToString());

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            reader.ReadLine();
           // doc.LoadXml(reader.ReadToEnd());
            doc.LoadXml("<Main/>");
            doc.DocumentElement.AppendChild(doc.CreateElement("ProductImage")).InnerXml = reader.ReadToEnd();
            doc.Save(@"C:\Tmp.xml");

            Bitmap mayBitmaq = new Bitmap(memStream);
         //   pictureBox1.Image = mayBitmaq;
            System.Drawing.Icon dd;
            

        }

        private void OrdersReportBtn_Click(object sender, EventArgs e)
        {




            try
            {
                DXConnectableControls.XtraReports.Design.ReportDesignForm reportDesignForm = new DXConnectableControls.XtraReports.Design.ReportDesignForm();
                reportDesignForm.NewReport();
                //OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource = (reportDesignForm.Report as OOAdvantech.UserInterface.ReportObjectDataSource.IReport).ReportDataSource;
                //reportDataSource.AssemblyFullName = typeof(IOrder).Assembly.FullName;
                //reportDataSource.TypeFullName = typeof(IOrder).FullName;
                //reportDesignForm.Report.ResumeDataSource();
                //int tt= reportDataSource.DataSourceMembers.Count;
                //reportDesignForm.OpenReport(@"c:\report1.repx");
                //DXConnectableControls.XtraReports.UI.Report report = DXConnectableControls.XtraReports.UI.Report.FromFile(@"c:\report1.repx",true) as DXConnectableControls.XtraReports.UI.Report;
                //reportDesignForm.OpenReport(report);
                reportDesignForm.ShowDialog();
                bool exit = true;
                if (exit)
                    return;

                DXConnectableControls.XtraReports.UI.Report report = DXConnectableControls.XtraReports.UI.Report.FromFile(@"D:\Documents and Settings\jim\My Documents\OrdersReport.repx", true) as DXConnectableControls.XtraReports.UI.Report;
                //List<object> results = new List<object>();
                //foreach (object obj in ReportsData.OrdersReportData.Result)
                //    results.Add(obj); 
                ////ReportsData.OrdersReportData.Result
                ////OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(Form3.OpenStorage());
                ////List<IOrder> orders = new List<IOrder>(from order in storage.GetObjectCollection<IOrder>() select order);
                //(report.DataSource as ConnectableControls.ReportData.ReportData).Data = results;
                report.ShowPreview();
            }
            catch (Exception error)
            {

                
            }




        }

      

    }
    /// <MetaDataID>{455fa607-12bc-41b0-9f32-9f932fbed787}</MetaDataID>
    class Mycollection<T,  K > 
    {
        public void Add(T p1,K p2)
        {

        }
        List<T> getTT()
        {
            return new List<T>();
        }
        

    }

    /// <MetaDataID>{dbfcab0a-3e3d-4e2e-9bbd-0b0344c6afdb}</MetaDataID>
    public class TestEvent
    {

        public TestEvent()
        {

        }

        public void mbutton_Click(object sender, EventArgs e)
        {

        }
        ~TestEvent()
        {

        }


    }

}