using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using OOAdvantech;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace TestProject
{
	public struct StructTest:InterfaceC
	{

		void fooA()
		{
		}
		public void fooB()
		{
		}
		public interface kos
		{
			int num();
		}
		public kos kor;

	}
	public interface kosa
	{

		int num();
	}
	public interface InterfaceA
	{

	}
	public interface InterfaceB
	{

	}
	public interface InterfaceC:InterfaceB,InterfaceA
	{

	}

	public interface InterfaceD:InterfaceC
	{

	}
	public interface InterfaceE:InterfaceC
	{

	}


	public class ClassA:InterfaceC
	{

	}

    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{4AB3EE32-B1B7-4175-B990-0CFE2772064F}")]
    [OOAdvantech.MetaDataRepository.Persistent]
    public class PClassG
    {
        PClassG()
        {
        }
        OOAdvantech.ExtensionProperties ExtensionProperties;

        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        [OOAdvantech.MetaDataRepository.PersistentMember]
        public string Name="Katsa";
  
    }


    [Serializable]
    public class ClassG 
    {
        public string Name;
        public ClassG innerClassG;

    }

	public class ClassB:ClassA
	{

	}

	public class ClassC:ClassB
	{
		public void Test(object sender,System.Windows.Forms.DragEventArgs e)
		{

		}

		public static void TestStatic(object sender,System.Windows.Forms.DragEventArgs e)
		{

		}


		OOAdvantech.MetaDataRepository.Roles Foo(OOAdvantech.MetaDataRepository.Roles role)
		{
			return OOAdvantech.MetaDataRepository.Roles.RoleA;
		}

	}
	
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class TestDlg : Form
		//##ModelId={65AEB17F-2257-454B-9389-339DC0ED5601}
	{
		
		//##ModelId={5EDE0532-ACC2-4CA9-9A23-6765B3DFFBB9}
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.DataGrid dataGrid1;
		public System.Data.DataSet mDataSet;
		public System.Data.DataSet dataSet1;
		private System.Data.DataTable dataTable1;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		//##ModelId={9DA699B0-D24D-48D9-B627-E5FA84F9E04C}
		private System.ComponentModel.Container components = null;
		[OOAdvantech.MetaDataRepository.Persistent]
		public event DragEventHandler Test ;

		public ClassC classC;
		//##ModelId={BECF550D-911F-4F8B-9A71-7085A83CE503}
		public TestDlg()
		{ 
			classC=new ClassC();
			//	Test+=new DragEventHandler(classC.Test);
			//	Test+=new DragEventHandler(ClassC.TestStatic);
			
			TestProject.StructTest ll;
			
			//
			// Required for Windows Form Designer support
			// 
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		//##ModelId={EDFFF6CE-5429-426D-B4FE-66035C9DC431}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		static void ParsTest()
		{
			/*Parser.Parser parser=new Parser.Parser();
			parser.SetGrammarPath("G:\\PersistenceLayer\\OQLParser\\OQLParser.gmr");
			parser.Parse("SELECT  TheAddress FROM PersistenceLayerTestPrj.Address TheAddress");
			parser=null;*/
		}
		[DllImport("mscoree.dll", EntryPoint="GetCORSystemDirectory",  SetLastError=true,
			 CharSet=CharSet.Unicode, ExactSpelling=true,
			 CallingConvention=CallingConvention.StdCall)]
		public static extern int GetCORSystemDirectory([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder buffer,int BufferLength,ref int Length);
		
																																																				   

		#region Windows Form Designer generated code
		/// <summary>
		///  Mitsos say Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		//##ModelId={D3846F25-B2D3-46C5-90CB-3EF74BB24FF5}
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.dataSet1 = new System.Data.DataSet();
			this.dataTable1 = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			this.dataColumn2 = new System.Data.DataColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(96, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 48);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(72, 72);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(136, 48);
			this.button2.TabIndex = 1;
			this.button2.Text = "ApplicationStarter";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// dataGrid1
			// 
			this.dataGrid1.DataMember = "";
			this.dataGrid1.DataSource = this.dataSet1;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(24, 288);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(560, 80);
			this.dataGrid1.TabIndex = 2;
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "NewDataSet";
			this.dataSet1.EnforceConstraints = false;
			this.dataSet1.Locale = new System.Globalization.CultureInfo("el-GR");
			this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
																		  this.dataTable1});
			// 
			// dataTable1
			// 
			this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
																			  this.dataColumn1,
																			  this.dataColumn2});
			this.dataTable1.Constraints.AddRange(new System.Data.Constraint[] {
																				  new System.Data.UniqueConstraint("Constraint1", new string[] {
																																				   "Column1"}, true)});
			this.dataTable1.PrimaryKey = new System.Data.DataColumn[] {
																		  this.dataColumn1};
			this.dataTable1.TableName = "Table1";
			// 
			// dataColumn1
			// 
			this.dataColumn1.AllowDBNull = false;
			this.dataColumn1.ColumnName = "Column1";
			// 
			// dataColumn2
			// 
			this.dataColumn2.ColumnName = "Column2";
			// 
			// TestDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 594);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "TestDlg";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.TestDlg_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		static object NewObject(Type type,params object[] ctorParams)
		{
			object[] Params=new object[ctorParams.Length-1];
			int i=0;
			for(i=0;i!=ctorParams.Length-1;i++)
				Params[i]=ctorParams[i];
			Type[] ParamTypes=ctorParams[i] as Type[];
			ConstructorInfo constructorInfo =type.GetConstructor(BindingFlags.Instance | BindingFlags.Public,null,ParamTypes,null);
			return constructorInfo.Invoke(Params);



		}
		static public void UnhandledException(object sender,UnhandledExceptionEventArgs e)
		{
			System.Windows.Forms.MessageBox.Show("Ere");

		}


		static OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock=new OOAdvantech.Synchronization.ReaderWriterLock();
		static void SynchroCall()
		{
			ReaderWriterLock.AcquireWriterLock(3000);
			try
			{
						
				System.Threading.Thread.Sleep(13000);
			}
			finally
			{
				ReaderWriterLock.ReleaseWriterLock();
			}
		}
		internal delegate void ActionRequest();
		static void SynchroCall2()
		{
			ReaderWriterLock.AcquireWriterLock(3000);
			try
			{
				System.Threading.Thread.Sleep(13000);
			}
			finally
			{
				ReaderWriterLock.ReleaseWriterLock();
			}
		}
		//##ModelId={93C4BE4D-1775-41F8-A120-720FA5951A07}
		static void Main() 
		{



//			ActionRequest  CommitRequest= new  ActionRequest(SynchroCall2);
//			CommitRequest.BeginInvoke(null,null);
//		
//			System.Threading.Thread.Sleep(1000);
//			try
//			{
//				SynchroCall();
//
//			}
//			catch(System.Exception error)
//			{
//				int ertr=0;
//			}
//
//			 



			try
			{




                try
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage mStorageSession;

                    try
                    {
                        mStorageSession = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage
                            ("SynchronizeTest", @"C:\Temp\SynchronizeTest.mtd", "OOAdvantech.XMLPersistenceRunTime.StorageProvider");
                    }
                    catch (System.Exception Error)
                    {
                        mStorageSession = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("SynchronizeTest", @"C:\Temp\SynchronizeTest.mtd", "OOAdvantech.XMLPersistenceRunTime.StorageProvider",true);

                    }
                    PClassG temp= mStorageSession.NewObject(typeof(PClassG)) as PClassG;

                }
                catch (Exception error)
                {


                }        
                return;



				System.Collections.ArrayList errors=new OOAdvantech.Collections.ArrayList();
//				OOAdvantech.DotNetMetaDataRepository.Assembly assembly12=new OOAdvantech.DotNetMetaDataRepository.Assembly(typeof(BusinessModelView.Diagram).Assembly);
//				assembly12.ErrorCheck(ref errors);


				//string assFile="X:\\source\\OpenVersions\\PersistenceLayer\\DotNetPersistenceLayer\\DotNetMetaDataRepository\\bin\\Debug\\DotNetMetaDataRepository.dll";
				string assFile="X:\\source\\OpenVersions\\PersistenceLayer\\DotNetPersistenceLayer\\AbstractClassPersistency\\bin\\debug\\AbstractClassPersistency.dll";
				//string assFile="X:\\source\\OpenVersions\\PersistenceLayer\\DotNetPersistenceLayer\\MSSQLFastPersistenceRunTime\\bin\\Debug\\MSSQLFastPersistenceRunTime.dll";
				//string assFile="X:\\source\\OpenVersions\\PersistenceLayer\\DotNetPersistenceLayer\\TestingDrawing\\bin\\Debug\\BusinessModelView.dll";

				ModulePublisher.ClassRepository.Publish(assFile,"","C:\\Resault.txt",false);
				System.Xml.XmlDocument xmlDocument=new System.Xml.XmlDocument();
	
				

				TestDlg testDlg=new TestDlg();

				Delegate _delegate=Delegate.CreateDelegate(typeof(System.Windows.Forms.DragEventHandler),testDlg.classC,"Test");

				
				Delegate _delegate1=Delegate.CreateDelegate(typeof(System.Windows.Forms.DragEventHandler),typeof(ClassC),"TestStatic");
				//testDlg.Test=_delegate as System.Windows.Forms.DragEventHandler ;
				System.Reflection.EventInfo EInfo= testDlg.GetType().GetEvent("Test");
				EInfo.AddEventHandler(testDlg,_delegate);
				EInfo.AddEventHandler(testDlg,_delegate1);

				
				//Delegate.Combine(testDlg.Test,_delegate);
				//	System.Delegate[] tts= testDlg.Test.GetInvocationList();
				
				testDlg.Test(null,null);


				
	


				
				//(wrMember as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf

		
				OOAdvantech.DotNetMetaDataRepository.Assembly assembly1=new OOAdvantech.DotNetMetaDataRepository.Assembly(typeof(OOAdvantech.MetaDataRepository.Attribute).Assembly);
				
//				OOAdvantech.DotNetMetaDataRepository.Assembly assembly=new OOAdvantech.DotNetMetaDataRepository.Assembly(typeof(AbstractionsAndPersistency.IOrder).Assembly);
				


				
				//System.Windows.Forms.MessageBox.Show("as");
				bool ere=assembly1.ErrorCheck(ref errors);
//				ere=assembly.ErrorCheck(ref errors);
				
				//embeddedOQLParser.Parse("");
				int werwe=0;

//				ere=assembly.ErrorCheck(ref errors);
//				System.Diagnostics.Debug.WriteLine(errors);
//					OOAdvantech.MetaDataRepository.Classifier _class= OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(typeof(AbstractionsAndPersistency.IOrderDetail)) as OOAdvantech.MetaDataRepository.Classifier;
//					OOAdvantech.MetaDataRepository.Classifier _class2= OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(typeof(AbstractionsAndPersistency.IProductPrice)) as OOAdvantech.MetaDataRepository.Classifier;
				
				
				return;

			
				OOAdvantech.DotNetMetaDataRepository.Class _class;
				//OOAdvantech.DotNetMetaDataRepository.Class _class= OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(typeof(AbstractionsAndPersistency.MProductPrice)) as OOAdvantech.DotNetMetaDataRepository.Class;
				//OOAdvantech.DotNetMetaDataRepository.Interface _interface= OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(typeof(AbstractionsAndPersistency.IProductPrice)) as OOAdvantech.DotNetMetaDataRepository.Interface;
			//	OOAdvantech.MetaDataRepository.Association ass=_class.ClassHierarchyLinkAssociation;
//				OOAdvantech.DotNetMetaDataRepository.Interface _interface= OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(typeof(AbstractionsAndPersistency.IPriceList)) as OOAdvantech.DotNetMetaDataRepository.Interface;

			//	System.Reflection.FieldInfo fieldInfo=_class.LinkClassRoleAField;


				foreach(OOAdvantech.MetaDataRepository.Feature feature in _class.Features)
				{
					int fdfdds=0;
				}
							 
				return;
				Application.Run(new TestDlg() );
				

				//MOVCUserInterface.TestForm2 testForm2=new MOVCUserInterface.TestForm2();
				//Application.Run(testForm2);
			}
			catch(System.Exception mException)
			{
				System.Windows.Forms.MessageBox.Show(mException.StackTrace);

			}
			//Application.Run(new TestDlg());
		}

        static private void SerializeDataSet(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ClassG));

            // Creates a DataSet; adds a table, column, and ten rows.
            //DataSet ds = new DataSet("myDataSet");
            //DataTable t = new DataTable("table1");
            //DataColumn c = new DataColumn("thing");
            //t.Columns.Add(c);
            //ds.Tables.Add(t);
            //DataRow r;
            //for (int i = 0; i < 10; i++)
            //{
            //    r = t.NewRow();
            //    r[0] = "Thing " + i;
            //    t.Rows.Add(r);
            //}
            ClassG cg = new ClassG();
            cg.Name = "Kitsos";
            cg.innerClassG = new ClassG();
            cg.innerClassG.Name = "mitsos";

            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, cg);
            writer.Close();
        }


		//##ModelId={11C8663D-DA51-465F-AEBC-8335A5F7A149}
		private void button1_Click(object sender, EventArgs e)
		{
			try
			{

				//	"Provider=OraOLEDB.Oracle.1;Persist Security Info=False;User ID=system;Data Source=ORACLEDB"
				System.Data.OleDb.OleDbConnection connection=new System.Data.OleDb.OleDbConnection("Provider=OraOLEDB.Oracle;Password=astraxan;Persist Security Info=True;OLEDB.NET=true;User ID=system;Data Source=ORACLEDB");
				connection.Open();
				System.Data.OleDb.OleDbCommand command=connection.CreateCommand();
				command.CommandText="select \"LIAKOS\".\"NAME\",\"LIAKOS\".\"AGE\" from \"LIAKOS\"";
				System.Data.OleDb.OleDbDataReader reader= command.ExecuteReader();
				bool wtw=reader.HasRows;
				int aas=0;

				while (reader.Read()) 
				{
					object Name=reader["NAME"];
					object Age=reader["AGE"];
					int aww=0;
				}

 
			}
			catch(System.Exception Error)
			{
				int tt=0;
			}

			
		}
		//##ModelId={09A710E0-70DF-40A6-967F-2FC1FBF4EF57}
		private void TestDlg_Load(object sender, System.EventArgs e)
		{
		
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			
			
			//Application.Run(new MOVCUserInterface.TestForm2());
		}
	}

	/*

		class TypeA  
		{
		}


		class TypeB:TypeA  
		{
		}

		class TypeC:TypeB  
		{
		}

		class mTypeA  
		{
		}


		class mTypeB:mTypeA  
		{
		}

		class mTypeC:mTypeB  
		{
		}

		class MyType  
		{
			private int _age;
			private string _name;

			public MyType (string incName, int incAge)
			{
				_name = incName;
				_age = incAge;
			}
			public MyType (mTypeA incName, TypeA incAge)
			{
				int t=0;
			}
			public MyType (mTypeB incName, TypeA incAge)
			{
				int t=0;
			}


		}*/

	
}
