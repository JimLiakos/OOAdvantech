using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using OOAdvantech.Collections.Generic;

using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

 
namespace Client
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ClientForm : System.Windows.Forms.Form
	{
        public string mitsos = "kk";
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        } 
		
		private System.Windows.Forms.Button NewObject;
		private System.Windows.Forms.Button CreateStorage;
		private System.Windows.Forms.Button GetObject;
		private System.Windows.Forms.Button ChangeObjectState;
		private System.Windows.Forms.Button LiveAtRelationTest;
		private System.Windows.Forms.Button GetPersonsAtAddress;
		private System.Windows.Forms.Button AddFamilies;
		private System.Windows.Forms.Button LiveAtFamilyHome;
		private System.Windows.Forms.Button NewEmployee;
		private System.Windows.Forms.Button GetPersons;
		private System.Windows.Forms.Button AddCompanyEmployee;
		private System.Windows.Forms.Button GetEmployees;
		private System.Windows.Forms.Button Delete;
		private System.Windows.Forms.Button Sample;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ClientForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //OOAdvantech.Transactions.Transaction

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.NewObject = new System.Windows.Forms.Button();
			this.CreateStorage = new System.Windows.Forms.Button();
			this.GetObject = new System.Windows.Forms.Button();
			this.ChangeObjectState = new System.Windows.Forms.Button();
			this.LiveAtRelationTest = new System.Windows.Forms.Button();
			this.GetPersonsAtAddress = new System.Windows.Forms.Button();
			this.AddFamilies = new System.Windows.Forms.Button();
			this.LiveAtFamilyHome = new System.Windows.Forms.Button();
			this.NewEmployee = new System.Windows.Forms.Button();
			this.GetPersons = new System.Windows.Forms.Button();
			this.AddCompanyEmployee = new System.Windows.Forms.Button();
			this.GetEmployees = new System.Windows.Forms.Button();
			this.Delete = new System.Windows.Forms.Button();
			this.Sample = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// NewObject
			// 
			this.NewObject.Location = new System.Drawing.Point(16, 48);
			this.NewObject.Name = "NewObject";
			this.NewObject.Size = new System.Drawing.Size(96, 32);
			this.NewObject.TabIndex = 0;
			this.NewObject.Text = "NewObject";
			this.NewObject.Click += new System.EventHandler(this.NewObject_Click);
			// 
			// CreateStorage
			// 
			this.CreateStorage.Location = new System.Drawing.Point(16, 8);
			this.CreateStorage.Name = "CreateStorage";
			this.CreateStorage.Size = new System.Drawing.Size(96, 32);
			this.CreateStorage.TabIndex = 1;
			this.CreateStorage.Text = "CreateStorage";
			this.CreateStorage.Click += new System.EventHandler(this.CreateStorage_Click);
			// 
			// GetObject
			// 
			this.GetObject.Location = new System.Drawing.Point(16, 88);
			this.GetObject.Name = "GetObject";
			this.GetObject.Size = new System.Drawing.Size(96, 32);
			this.GetObject.TabIndex = 2;
			this.GetObject.Text = "GetObject";
			this.GetObject.Click += new System.EventHandler(this.GetObject_Click);
			// 
			// ChangeObjectState
			// 
			this.ChangeObjectState.Location = new System.Drawing.Point(16, 128);
			this.ChangeObjectState.Name = "ChangeObjectState";
			this.ChangeObjectState.Size = new System.Drawing.Size(96, 32);
			this.ChangeObjectState.TabIndex = 3;
			this.ChangeObjectState.Text = "ChangeObjectState";
			this.ChangeObjectState.Click += new System.EventHandler(this.ChangeObjectState_Click);
			// 
			// LiveAtRelationTest
			// 
			this.LiveAtRelationTest.Location = new System.Drawing.Point(16, 168);
			this.LiveAtRelationTest.Name = "LiveAtRelationTest";
			this.LiveAtRelationTest.Size = new System.Drawing.Size(96, 32);
			this.LiveAtRelationTest.TabIndex = 4;
			this.LiveAtRelationTest.Text = "Create Persons and set Address";
			this.LiveAtRelationTest.Click += new System.EventHandler(this.LiveAtRelationTest_Click);
			// 
			// GetPersonsAtAddress
			// 
			this.GetPersonsAtAddress.Location = new System.Drawing.Point(16, 208);
			this.GetPersonsAtAddress.Name = "GetPersonsAtAddress";
			this.GetPersonsAtAddress.Size = new System.Drawing.Size(96, 32);
			this.GetPersonsAtAddress.TabIndex = 5;
			this.GetPersonsAtAddress.Text = "Get Persons at Address";
			this.GetPersonsAtAddress.Click += new System.EventHandler(this.GetPersonsAtAddress_Click);
			// 
			// AddFamilies
			// 
			this.AddFamilies.Location = new System.Drawing.Point(128, 8);
			this.AddFamilies.Name = "AddFamilies";
			this.AddFamilies.Size = new System.Drawing.Size(96, 32);
			this.AddFamilies.TabIndex = 6;
			this.AddFamilies.Text = "AddFamilies";
			this.AddFamilies.Click += new System.EventHandler(this.AddFamilies_Click);
			// 
			// LiveAtFamilyHome
			// 
			this.LiveAtFamilyHome.Location = new System.Drawing.Point(128, 48);
			this.LiveAtFamilyHome.Name = "LiveAtFamilyHome";
			this.LiveAtFamilyHome.Size = new System.Drawing.Size(96, 32);
			this.LiveAtFamilyHome.TabIndex = 7;
			this.LiveAtFamilyHome.Text = "Get childs lives at family home";
			this.LiveAtFamilyHome.Click += new System.EventHandler(this.LiveAtFamilyHome_Click);
			// 
			// NewEmployee
			// 
			this.NewEmployee.Location = new System.Drawing.Point(128, 88);
			this.NewEmployee.Name = "NewEmployee";
			this.NewEmployee.Size = new System.Drawing.Size(96, 32);
			this.NewEmployee.TabIndex = 8;
			this.NewEmployee.Text = "NewEmployee";
			this.NewEmployee.Click += new System.EventHandler(this.NewEmployee_Click);
			// 
			// GetPersons
			// 
			this.GetPersons.Location = new System.Drawing.Point(128, 128);
			this.GetPersons.Name = "GetPersons";
			this.GetPersons.Size = new System.Drawing.Size(96, 32);
			this.GetPersons.TabIndex = 9;
			this.GetPersons.Text = "GetPersons";
			this.GetPersons.Click += new System.EventHandler(this.GetPersons_Click);
			// 
			// AddCompanyEmployee
			// 
			this.AddCompanyEmployee.Location = new System.Drawing.Point(128, 168);
			this.AddCompanyEmployee.Name = "AddCompanyEmployee";
			this.AddCompanyEmployee.Size = new System.Drawing.Size(96, 32);
			this.AddCompanyEmployee.TabIndex = 10;
			this.AddCompanyEmployee.Text = "Add Company Employees";
			this.AddCompanyEmployee.Click += new System.EventHandler(this.AddCompanyEmployee_Click);
			// 
			// GetEmployees
			// 
			this.GetEmployees.Location = new System.Drawing.Point(128, 208);
			this.GetEmployees.Name = "GetEmployees";
			this.GetEmployees.Size = new System.Drawing.Size(96, 32);
			this.GetEmployees.TabIndex = 11;
			this.GetEmployees.Text = "Get Employees";
			this.GetEmployees.Click += new System.EventHandler(this.GetEmployees_Click);
			// 
			// Delete
			// 
			this.Delete.Location = new System.Drawing.Point(16, 248);
			this.Delete.Name = "Delete";
			this.Delete.Size = new System.Drawing.Size(96, 32);
			this.Delete.TabIndex = 12;
			this.Delete.Text = "Delete";
			this.Delete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// Sample
			// 
			this.Sample.Location = new System.Drawing.Point(128, 248);
			this.Sample.Name = "Sample";
			this.Sample.Size = new System.Drawing.Size(96, 32);
			this.Sample.TabIndex = 13;
			this.Sample.Text = "Sample";
			this.Sample.Click += new System.EventHandler(this.Sample_Click);
			// 
			// ClientForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 285);
			this.Controls.Add(this.Sample);
			this.Controls.Add(this.Delete);
			this.Controls.Add(this.GetEmployees);
			this.Controls.Add(this.AddCompanyEmployee);
			this.Controls.Add(this.GetPersons);
			this.Controls.Add(this.NewEmployee);
			this.Controls.Add(this.LiveAtFamilyHome);
			this.Controls.Add(this.AddFamilies);
			this.Controls.Add(this.GetPersonsAtAddress);
			this.Controls.Add(this.LiveAtRelationTest);
			this.Controls.Add(this.ChangeObjectState);
			this.Controls.Add(this.GetObject);
			this.Controls.Add(this.CreateStorage);
			this.Controls.Add(this.NewObject);
			this.Name = "ClientForm";
			this.Text = "ClientForm";
			this.Load += new System.EventHandler(this.ClientForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		static void ChangeDate(System.IO.DirectoryInfo di)
		{
			System.TimeSpan ts=new TimeSpan(32,0,0,0,0);
			foreach(System.IO.FileInfo fi in di.GetFiles())
			{
				fi.LastWriteTime=fi.LastWriteTime-ts;
				fi.CreationTime=fi.CreationTime-ts;
			}
			foreach(System.IO.DirectoryInfo _di in di.GetDirectories())
				ChangeDate(_di);


		}


        static void InstallDefaultsToStorage(ObjectStorage Storage)
        {
        }
      	/// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
        static void Main()
        {


            Application.Run(new ClientForm());


        }

        static void connection_StateChange(object sender, StateChangeEventArgs e)
        {
            
        }

		private void NewObject_Click(object sender, System.EventArgs e)
		{

			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				Family.Person person=storageSession.NewObject(typeof(Family.Person),"Jack",parametersTypes) as Family.Person;
				sysStateTransition.Consistent=true;;

			}
		}

		private void CreateStorage_Click(object sender, System.EventArgs e)
		{


			
			/*
			try
			{
				ObjectStorage storageSession=null;
				try
				{
					storageSession=	ObjectStorage.OpenStorage("ProjectManagement",
						System.Net.Dns.GetHostName(),
						"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				}
				catch(OOAdvantech.PersistenceLayer.StorageException Error)
				{
					if(Error.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
					{
						storageSession=ObjectStorage.NewStorage("ProjectManagement",
							System.Net.Dns.GetHostName(),
							"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
					}
					else
						throw Error;
				}
				catch(System.Exception Error)
				{
					int tt=0;
				}
				try
				{
					System.TimeSpan NewTimeSpan;
					System.TimeSpan ComiteTimeSpan;
					System.DateTime before;
					System.DateTime After;
					before=System.DateTime.Now;
					Cursor=Cursors.WaitCursor;

					//Pro

					storageSession.StorageMetaData.RegisterComponent(typeof(ProjectManagement.StorageObject).Assembly.FullName);
					storageSession.StorageMetaData.Build();
					After=System.DateTime.Now;
					ComiteTimeSpan=After-before;
					System.Diagnostics.Debug.WriteLine(ComiteTimeSpan.ToString());

				}
				catch(System.Exception Errore)
				{
					int  sdf=0;
				}
			}
			catch(System.Exception Errore)
			{
				int  sdf=0;
			}
			return;*/
            try
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    ObjectStorage.DeleteStorage("Family",
                         @"127.0.0.1\Debug",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                    stateTransition.Consistent = true;
                }
            }
            catch (System.Exception mError)
            {

            }
            bool flag = true;
            if (flag)
                return;
            System.DateTime start = System.DateTime.Now;
            System.TimeSpan tSpan ;
			try
			{
				ObjectStorage storageSession=null;
				try
				{
					storageSession=	ObjectStorage.OpenStorage("Family",
                         @"127.0.0.1\Debug",
						"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				}
				catch(OOAdvantech.PersistenceLayer.StorageException Error)
				{
					if(Error.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
					{
						storageSession=ObjectStorage.NewStorage("Family",
                            @"127.0.0.1\Debug",
							"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
					}
					else
						throw Error;
				}
				catch(System.Exception Error)
				{
					int tt=0;
				}
				try
				{
                    tSpan = System.DateTime.Now - start;
                    start = System.DateTime.Now;
                    System.Diagnostics.Debug.WriteLine(tSpan.ToString());

                    storageSession.StorageMetaData.RegisterComponent(typeof(Family.Job).Assembly.FullName);

                    tSpan = System.DateTime.Now - start;
                    start = System.DateTime.Now;
                    System.Diagnostics.Debug.WriteLine(tSpan.ToString());

                    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.Client).Assembly.FullName);



					//storageSession.StorageMetaData.Build();
				}
				catch(System.Exception Errore)
				{
					int  sdf=0;
				}
			}
			catch(System.Exception Errore)
			{
				int  sdf=0;
			}
            tSpan = System.DateTime.Now - start;
            System.Diagnostics.Debug.WriteLine(tSpan.ToString());

		}

		private void GetObject_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor=Cursors.WaitCursor;

				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name='Jack'";
				//SELECT person FROM Family.Person person WHERE person.Name='Jack'
				StructureSet objectSet=storageSession.Execute(objectQuery);
				foreach(StructureSet objectSetInstance in objectSet)
				{
					Family.Person person = objectSetInstance["person"] as Family.Person;
					System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
				}
			}
			finally
			{
				Cursor=Cursors.Default;
			}


		}

		private void ChangeObjectState_Click(object sender, System.EventArgs e)
		{
			Family.Person person=null;
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name='Jack'";
			//SELECT person FROM Family.Person person WHERE person.Name='Jack'
			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				person = objectSetInstance["person"] as Family.Person;
				System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
			}
			if(person!=null)
				person.Name="George";


		}

		private void LiveAtRelationTest_Click(object sender, System.EventArgs e)
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				
				Family.Person Jack=storageSession.NewObject(typeof(Family.Person),"Jack",new System.Type[1]{typeof(string)}) as Family.Person;
				Family.Person John=storageSession.NewObject(typeof(Family.Person),"John",new System.Type[1]{typeof(string)}) as Family.Person;
				Family.Person George=storageSession.NewObject(typeof(Family.Person),"George",new System.Type[1]{typeof(string)}) as Family.Person;

				
				Family.Address address=storageSession.NewObject(typeof(Family.Address),
											"LONDON","PADDINGTON","SUSSEX GARDENS 35",
											new System.Type[3]{typeof(string),
																typeof(string),
																typeof(string)})as Family.Address;

				Jack.Address=address;
				John.Address=address;
				George.Address=address;
				sysStateTransition.Consistent=true;
			}

		}

		private void GetPersonsAtAddress_Click(object sender, System.EventArgs e)
		{
			
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Address.City='LONDON' ";
			//"SELECT person FROM "+Family.Person person WHERE persona.Address.City='LONDON' ";
			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person = objectSetInstance["person"] as Family.Person;
				System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
			}
		}

		private void AddFamilies_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor=Cursors.WaitCursor;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{
								
					Family.Person father=storageSession.NewObject(typeof(Family.Person),"Jack",new System.Type[1]{typeof(string)}) as Family.Person;
					Family.Person mother=storageSession.NewObject(typeof(Family.Person),"Mary",new System.Type[1]{typeof(string)}) as Family.Person;
					Family.Person firstChild=storageSession.NewObject(typeof(Family.Person),"George",new System.Type[1]{typeof(string)}) as Family.Person;
					firstChild.AddParent(father);
					firstChild.AddParent(mother);

					Family.Person secondChild=storageSession.NewObject(typeof(Family.Person),"Nick",new System.Type[1]{typeof(string)}) as Family.Person;
					secondChild.AddParent(father);
					secondChild.AddParent(mother);

					Family.Person thirdChild=storageSession.NewObject(typeof(Family.Person),"Loosy",new System.Type[1]{typeof(string)}) as Family.Person;
					thirdChild.AddParent(father);
					thirdChild.AddParent(mother);

	
					Family.Address familyAddress=storageSession.NewObject(typeof(Family.Address),
						"LONDON","PADDINGTON","SUSSEX GARDENS 35",
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)})as Family.Address;

					Family.Address secondChildAddress=storageSession.NewObject(typeof(Family.Address),
						"LONDON","PADDINGTON","SUSSEX GARDENS 5",
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)})as Family.Address;

					father.Address=mother.Address=firstChild.Address=thirdChild.Address=familyAddress;
					secondChild.Address=secondChildAddress;

					father=storageSession.NewObject(typeof(Family.Person),"John",new System.Type[1]{typeof(string)}) as Family.Person;
					mother=storageSession.NewObject(typeof(Family.Person),"sara",new System.Type[1]{typeof(string)}) as Family.Person;
					firstChild=storageSession.NewObject(typeof(Family.Person),"Lora",new System.Type[1]{typeof(string)}) as Family.Person;
					firstChild.AddParent(father);
					firstChild.AddParent(mother);

					secondChild=storageSession.NewObject(typeof(Family.Person),"Paul",new System.Type[1]{typeof(string)}) as Family.Person;
					secondChild.AddParent(father);
					secondChild.AddParent(mother);


					
					familyAddress=storageSession.NewObject(typeof(Family.Address),
						"LONDON","PADDINGTON","SUSSEX GARDENS 110",
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)})as Family.Address;

					secondChildAddress=storageSession.NewObject(typeof(Family.Address),
						"LONDON","PADDINGTON","SUSSEX GARDENS 98",
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)})as Family.Address;
					father.Address=mother.Address=firstChild.Address=familyAddress;
					secondChild.Address=secondChildAddress;
					sysStateTransition.Consistent=true;;



				}
			}
			catch(System.Exception Error)
			{
				int hhs=0;
			}
			finally
			{
				Cursor=Cursors.Default;
			}

		}

		private void LiveAtFamilyHome_Click(object sender, System.EventArgs e)
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT person,person.Name name "+
				"FROM "+typeof(Family.Person).FullName +" person "+
				"WHERE person.Address = person.Parents.Address";
			//"SELECT person,person.Name name FROM Family.Person person WHERE person.Address = person.Parents.Address"
			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person = objectSetInstance["person"] as Family.Person;
				System.Diagnostics.Debug.WriteLine("Person name:"+objectSetInstance["name"] as string); 
			}

		}

		private void NewEmployee_Click(object sender, System.EventArgs e)
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				System.Type[] parametersTypes=new System.Type[2]{typeof(string),typeof(double)};
				Family.Employee wmployee=storageSession.NewObject(typeof(Family.Employee),
														"Michael",1500,new System.Type[2]{typeof(string),typeof(double)}) as Family.Employee;
				sysStateTransition.Consistent=true;;
			}
		}

		private void GetPersons_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor=Cursors.WaitCursor;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person ";
				//SELECT person FROM Family.Person person 
				StructureSet objectSet=storageSession.Execute(objectQuery);
				foreach(StructureSet objectSetInstance in objectSet)
				{
					Family.Person person = objectSetInstance["person"] as Family.Person;
					System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
				}
			}
			finally
			{
				Cursor=Cursors.Default;
			}
		}

		private void AddCompanyEmployee_Click(object sender, System.EventArgs e)
		{
			Cursor=Cursors.WaitCursor;
			try
			{
				Family.Company company=null;
				Family.Employee employee=null;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{
					System.Type[] employeeParameterTypes=new System.Type[2]{typeof(string),typeof(double)};
					Family.Employee jack=storageSession.NewObject(typeof(Family.Employee),
						"Jack",1500,employeeParameterTypes) as Family.Employee;
					/**/Family.Employee lorka=storageSession.NewObject(typeof(Family.Employee),
						"Lorka",1200,employeeParameterTypes) as Family.Employee;
					Family.Employee looberk=storageSession.NewObject(typeof(Family.Employee),
						"Looberk",1600,employeeParameterTypes) as Family.Employee;
				
					System.Type[] jobParameterTypes=new System.Type[2]{typeof(string),typeof(DateTime)};
					Family.Job electrician=storageSession.NewObject(typeof(Family.Job),
						"Electrician",DateTime.Parse("10/11/2004"),jobParameterTypes) as Family.Job;
					electrician.Employee=jack;

					/**/Family.Job electronic=storageSession.NewObject(typeof(Family.Job),
						"Electronic",DateTime.Parse("4/3/2003"),jobParameterTypes) as Family.Job;
					electronic.Employee=lorka;
				
					Family.Job manager=storageSession.NewObject(typeof(Family.Job),
						"Manager",DateTime.Parse("1/1/2003"),jobParameterTypes) as Family.Job;
					manager.Employee=looberk;

					System.Type[] companyParameterTypes=new System.Type[1]{typeof(string)};
					Family.Company genecom=storageSession.NewObject(typeof(Family.Company),
						"Genecom",companyParameterTypes) as Family.Company;
					company=genecom;
					electrician.Employer=company;
					employee=jack;
					genecom.AddEmployee(electrician);
					genecom.AddEmployee(electronic);
					genecom.AddEmployee(manager);
					sysStateTransition.Consistent=true;

				}
				int ttwer=0;
                Set<Family.Job> employees = employee.Employers;
				Set<Family.Job> employers=company.Employees;
				foreach(Family.Job job in company.Employees)
				{
					Family.Company _Company=job.Employer;
					Family.Employee _employee=job.Employee as Family.Employee;
					long count=_employee.Employers.Count;
					int wfae=0;

				}
			}
			catch(System.Exception Error)
			{
				int aw=0;
			}
			finally
			{
				Cursor=Cursors.Default;
			}
		
		}

		private void GetEmployees_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor=Cursors.WaitCursor;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			

				string objectQuery="SELECT company.Employees employee "+
					"FROM "+typeof(Family.Company).FullName+" company "+
					"WHERE company.Employees.Job.StartingDate<1/1/2004 AND company.Name = 'Genecom'";

				//SELECT company.Job.Employee employee 
				//FROM Family.Company company 
				//WHERE company.Job.StartingDate<1/1/2004 AND company.Name = 'Genecom'

				
				objectQuery="SELECT employee employee.Job.StartingDate,employee.Parents.Address.City,employee.Employer.Name"+
					"FROM "+typeof(Family.Employee).FullName+" employee  "+
					"WHERE employee.Job.StartingDate<1/1/2004 AND employee.Job.Employer.Name = 'Genecom'";

				//SELECT employee 
				//FROM Family.Employee employee  
				//WHERE employee.Job.StartingDate<1/1/2004 AND employee.Job.Employer.Name = 'Genecom'"

				objectQuery="SELECT employee "+
					"FROM "+typeof(Family.Employee).FullName+" employee  ";

				StructureSet objectSet=storageSession.Execute(objectQuery);
				foreach(StructureSet objectSetInstance in objectSet)
				{
					Family.Employee employee = objectSetInstance["employee"] as Family.Employee;
					
					System.Diagnostics.Debug.WriteLine("Person name:"+employee.Name); 
					foreach(Family.Job job in employee.Employers)
					{
						string eName=job.Employee.Name;

						string cName=job.Employer.Name;
						int kk=0;


					}
				}
			}
			catch(System.Exception Error)
			{
				int herw=0;
			}
			finally
			{
				Cursor=Cursors.Default;

			}
		}

		private void Delete_Click(object sender, System.EventArgs e)
		{
            GC.Collect();
            return;
			try
			{
				Cursor=Cursors.WaitCursor;

				Cursor=Cursors.WaitCursor;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
				string objectQuery="SELECT employee "+
					"FROM "+typeof(Family.Employee).FullName+" employee  ";

				StructureSet objectSet=storageSession.Execute(objectQuery);
				Family.Job		_job=null;
				Family.Employee _employee=null;
				Family.Company _company=null;

				foreach(StructureSet objectSetInstance in objectSet)
				{
					Family.Employee employee = objectSetInstance["employee"] as Family.Employee;
					foreach(Family.Job job in employee.Employers)
					{
						_job=job;
						_employee=_job.Employee as Family.Employee;
						_company=_job.Employer;
						break;
					}
					if(_job!=null)
						break;
				}
				long fCount=_employee.Employers.Count;
				long sCount=_company.Employees.Count;
				_company.RemoveEmployee(_job);
				fCount=_employee.Employers.Count;
				sCount=_company.Employees.Count;
				int ee=0;



			}
			catch(System.Exception Error)
			{

				int sdf=Error.Message.Length;
				sdf++;

			}
			finally
			{
				Cursor=Cursors.Default;
			}



			/*try ReferentialIntegrity delete*/
/*
			try
			{
				Cursor=Cursors.WaitCursor;
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				string objectQuery="SELECT address FROM "+typeof(Family.Address).FullName+" address";
				//SELECT person FROM Family.Person person WHERE person.Name='Jack'
				using(SystemStateTransition stateTransition=new SystemStateTransition())
				{
 
					StructureSet objectSet=storageSession.Execute(objectQuery);
					foreach(StructureSet objectSetInstance in objectSet)
					{
						Family.Address address = objectSetInstance["address"] as Family.Address;
						//System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
						ObjectStorage.DeleteObject(address);
					}
					stateTransition.Consistent=true;
				}



			}
			catch(System.Exception Error)
			{

				int sdf=Error.Message.Length;
				sdf++;

			}
			finally
			{
				Cursor=Cursors.Default;
			}
*/

			/* delete and cascade delete
			try
			{
				Cursor=Cursors.WaitCursor;

				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name='Jack'";
				//SELECT person FROM Family.Person person WHERE person.Name='Jack'
				using(SystemStateTransition stateTransition=new SystemStateTransition())
				{
 
					StructureSet objectSet=storageSession.Execute(objectQuery);
					foreach(StructureSet objectSetInstance in objectSet)
					{
						Family.Person person = objectSetInstance["person"] as Family.Person;
						System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
						ObjectStorage.DeleteObject(person);
					}
					stateTransition.Consistent=true;
				}
			}
			finally
			{
				Cursor=Cursors.Default;
			}*/
		
		}
		public class COMApi
		{
			[System.Runtime.InteropServices.DllImport("ole32.dll")]
			public static extern int GetRunningObjectTable(int reserved, out System.Runtime.InteropServices.UCOMIRunningObjectTable pROT) ;
			
		}
        //void BinaryFormat()
        //{
		
        //    byte[] byteStream=new byte[4096];
        //    byte[] headerStream=new byte[4];
        //    int nextAvailablePos=0;
        //    int offset=4;
			
			
        //    short _short=670;
        //    double _double=23.567;
        //    long _long=-0xacf3a2c53;
        //    float _float=2144.765F;
        //    bool _bool=true;
        //    decimal _decimal=899676759789.97567856569m;
        //    DateTime _date=DateTime.Now;
        //    sbyte _sbyte=-30;
        //    string _string="OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_short,byteStream,offset);";
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_string,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_short,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_double,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_long,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_float,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_bool,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_decimal,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_date,byteStream,offset,ref offset);
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(_sbyte,byteStream,offset,ref offset);
			
        //    int temp=0;
			
        //    OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(offset-4,byteStream,0,ref temp);

        //    System.IO.MemoryStream memoryStream =new MemoryStream();
        //    memoryStream.Write(byteStream,0,offset);
        //    memoryStream.Write(byteStream,0,offset);

        //    memoryStream.Position=0;
        //    memoryStream.Read(headerStream,0,4);
        //    int BLOBsize=OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(headerStream,0,ref temp);
        //    memoryStream.Position=BLOBsize+4;
        //    memoryStream.Read(headerStream,0,4);
        //    BLOBsize=OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(headerStream,0,ref temp);


        //    byteStream=new byte[BLOBsize];
			
        //    memoryStream.Read(byteStream,0,BLOBsize);
			





        //    offset=0;

        //    string n_string=OOAdvantech.BinaryFormatter.BinaryFormatter.ToString(byteStream,offset,ref offset);
        //    short n_short=OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt16(byteStream,offset,ref offset);
        //    double n_double=OOAdvantech.BinaryFormatter.BinaryFormatter.ToDouble(byteStream,offset,ref offset);
        //    long n_long=OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt64(byteStream,offset,ref offset);
        //    float n_float=OOAdvantech.BinaryFormatter.BinaryFormatter.ToSingle(byteStream,offset,ref offset);
        //    bool n_bool=OOAdvantech.BinaryFormatter.BinaryFormatter.ToBoolean(byteStream,offset,ref offset);
        //    decimal n_decimal=OOAdvantech.BinaryFormatter.BinaryFormatter.ToDecimal(byteStream,offset,ref offset);
        //    DateTime n_date=OOAdvantech.BinaryFormatter.BinaryFormatter.ToDateTime(byteStream,offset,ref offset);
        //    sbyte n_sbyte=OOAdvantech.BinaryFormatter.BinaryFormatter.ToSByte(byteStream,offset,ref offset);


        //    int erta=-0xf3a2c53;
			
        ///*	for(int ia=0;ia<000002;ia++)
        //    {
        //        OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(erta,byteStream,5);
        //        int aa=OOAdvantech.BinaryFormatter.BinaryFormatter.ToInt32(byteStream,5);
        //        int werw=0;
        //    }*/
        //}
		int TestCount=4000;
		ObjectStorage storageSession=null;

		

		void WriteData()
		{
			
			System.TimeSpan NewTimeSpan;
			System.TimeSpan ComiteTimeSpan;
			System.DateTime before;
			System.DateTime After;
			before=System.DateTime.Now;

			//System.Type[] parametersTypes=new System.Type[2]{typeof(string),typeof(double)};
			using(OOAdvantech.Transactions.SystemStateTransition stateTransition=new SystemStateTransition())
			{
				//before=System.DateTime.Now;
				//for(int i=0;i!=TestCount;i++)
				//	storageSession.NewObject(typeof(Family.Employee),"mitsos",1230,parametersTypes);

				OOAdvantech.MetaDataRepository.Class classAtributeType= storageSession.NewObject(typeof(OOAdvantech.MetaDataRepository.Class))as OOAdvantech.MetaDataRepository.Class;
				classAtributeType.Name="Int32";
				for(int i=0;i!=TestCount;i++)
				{
					OOAdvantech.MetaDataRepository.Class _class= storageSession.NewObject(typeof(OOAdvantech.MetaDataRepository.Class))as OOAdvantech.MetaDataRepository.Class;
					_class.Name="Employee";

					_class.AddAttribute("Age",classAtributeType,""); 

					
				}
	
				//After=System.DateTime.Now;
				//NewTimeSpan=After-before;
				//System.Diagnostics.Debug.WriteLine(NewTimeSpan.ToString());
				

				stateTransition.Consistent=true;
			}
			After=System.DateTime.Now;
			ComiteTimeSpan=After-before;
			System.Diagnostics.Debug.WriteLine(ComiteTimeSpan.ToString());
			int wewte=0;

		}

		ObjectStorage TestCaseStorage;
		private void Sample_Click(object sender, System.EventArgs e)
		{
            try
            {

                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                string objectQuery = @"#OQL: SELECT order, order.Name OrderName ,  order.OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder order  WHERE order.Name <> 'order.Client.Name'   OR (order.OrderDetails.Price.Product.Name  = 'sprite' AND order.OrderDetails.Price.Product.Name ='kes') #";

                int count = 0;
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                    //order.ObjectChangeState += new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
                    //order.Update();
                    //order.ObjectChangeState -= new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
                    //order.Update();
                }
                


            }
            catch (System.Exception Error)
            {


            }
            GC.Collect();

            return;

        
		

			/*System.Reflection.BindingFlags bindingFlags  =System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.DeclaredOnly;
			
			System.Type orderType =typeof(AbstractionsAndPersistency.Order);
			System.Type subOrderType =typeof(AbstractionsAndPersistency.SubOrder);
			System.Type subSubOrderType =typeof(AbstractionsAndPersistency.SubSubOrder);
			System.Reflection.MethodInfo  orderAbstrMethod=orderType.GetMethod("AbstrMethod",bindingFlags); 
			System.Reflection.MethodInfo  orderVirtMethod=orderType.GetMethod("VirtMethod",bindingFlags); 
			System.Reflection.MethodInfo  orderget_OrderDetails=orderType.GetMethod("get_OrderDetails",bindingFlags); 

			System.Reflection.MethodInfo  subOrderAbstrMethod=subOrderType.GetMethod("AbstrMethod",bindingFlags); 
			System.Reflection.MethodInfo  subOrderVirtMethod=subOrderType.GetMethod("VirtMethod",bindingFlags); 
			System.Reflection.MethodInfo  subOrderMethod=subOrderType.GetMethod("Method",bindingFlags); 
			System.Reflection.MethodInfo basedef=subOrderVirtMethod.GetBaseDefinition();

			System.Reflection.MethodInfo  subSubOrderVirtMethod=subSubOrderType.GetMethod("VirtMethod",bindingFlags); 

			basedef=orderAbstrMethod.GetBaseDefinition();
			if(basedef==orderAbstrMethod)
			{
				int wwrq=0;
			}
			basedef=subSubOrderVirtMethod.GetBaseDefinition();
			basedef=subOrderMethod.GetBaseDefinition();

			int ttere=0;




			return;


			try
			{
				storageSession=ObjectStorage.OpenStorage
					("AbstractionsAndPersistency","G:\\AbstractionsAndPersistency.mtd","OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
			}
			catch(System.Exception Error)
			{
				int nwe=0;
				storageSession=ObjectStorage.NewStorage
					("AbstractionsAndPersistency","G:\\AbstractionsAndPersistency.mtd","OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider",true);

			}*/


			try
			{
				TestCaseStorage=ObjectStorage.OpenStorage("TestCasesStorage","LocalHost","OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider",false);
			}
			catch(StorageException StorageError)
			{
				
				if(StorageError.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
				{
					try
					{
						TestCaseStorage=ObjectStorage.NewStorage("TestCasesStorage","LocalHost","OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider",false);
					}
					catch(System.Exception Error)
					{
						System.Windows.Forms.MessageBox.Show("Problem with TestCase storage");
					}

				}
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Problem with TestCase storage");
			}




			try
			{
				try
				{
					storageSession=ObjectStorage.OpenStorage
						("Family" /*"Abstractions"*/,System.Net.Dns.GetHostName(),"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				}
				catch(OOAdvantech.PersistenceLayer.StorageException Error)
				{
					int ttt=0;

//					if(Error.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
//					{
//					
//						storageSession=ObjectStorage.NewStorage
//							("Abstractions",System.Net.Dns.GetHostName(),"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
//						int nwe=0;
//					}
//					else throw new System.Exception(Error.Message,Error);
				}
			 
//				storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.Order).Assembly.FullName);
//				storageSession.StorageMetaData.Build();

				Family.Employee employee=null;

				string query="SELECT orderDetail,orderDetail.Order order FROM "+typeof(AbstractionsAndPersistency.IOrderDetail).FullName+" orderDetail ";
				query="SELECT employee "+"FROM "+typeof(Family.Employee).FullName+" employee  ";

				OOAdvantech.PersistenceLayer.StructureSet afStructureSet=storageSession.Execute(query);
			
				foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in afStructureSet)
				{
					
					employee=Rowset["employee"] as Family.Employee;
					
					break;
				}


				query="SELECT orderDetail "+
					"FROM AbstractionsAndPersistency.Order theOrder , theOrder.OrderDetails orderDetail "+
					"WHERE  (theOrder = ObjectID(ObjectID = '5b4a4f84-dc41-4b7c-9d4b-f92dd70721fd',ObjCellID = 88) OR theOrder.Name='Loasas')";

				query="SELECT employee.Job ,employee.Parents.Address.City,employee.Parents.Name ParentName ,employee.Parents "+//,employee , employee.Job.StartingDate, employee.Employers.Name CompanyName "+
					"FROM "+typeof(Family.Employee).FullName+" employee  "+
					"WHERE employee.Job.StartingDate<1/1/2004 AND employee.Job.Employer.Name = 'Genecom'";

				query="SELECT employee.Employers.Name,employee.Parents.Address.City,employee.Parents.Name ParentName ,employee.Parents "+//,employee , employee.Job.StartingDate, employee.Employers.Name CompanyName "+
					"FROM "+typeof(Family.Employee).FullName+" employee  "+
					"WHERE employee.Job.StartingDate<1/1/2004 AND employee.Job.Employer.Name = @Name AND employee.Job.Employer=employee.Job.Employer AND "+
					"ObjectID(ObjectID = 'A3D560C2-970F-4F52-9D5F-638FF2D833F0',ObjCellID = 247)=employee.Job.Employer"+
					" AND employee.Parents=@Employee";

				OOAdvantech.Collections.Generic.Dictionary<string,object> parameters=new Dictionary<string,object>(1);
				//employee=new Family.Employee("as",12);
				parameters["@Name"]="Genecom";
				parameters["@Age"]=12;
				parameters["@Employee"]=employee;

				


					

//				query="SELECT job, job.Employee.Name ,job.Employer.Name "+//,employee.Parents.Address.City,employee.Parents.Name ParentName ,employee , employee.Job.StartingDate, employee.Employers.Name CompanyName "+
//					"FROM "+typeof(Family.Job).FullName+" job "+
//					"WHERE  (job = ObjectID(ObjectID = 'A3D560C2-970F-4F52-9D5F-638FF2D833F0',ObjCellID = 247))";
				


				OOAdvantech.PersistenceLayer.StructureSet aStructureSet=storageSession.Execute(query,parameters);
			
				foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
				{
					//AbstractionsAndPersistency.IOrder order=Rowset["order"] as AbstractionsAndPersistency.IOrder;
					AbstractionsAndPersistency.IOrderDetail	orderDetail=Rowset["orderDetail"] as AbstractionsAndPersistency.IOrderDetail;
					//Set  orderDetails =order.OrderDetails;
					break;
				}
//				using(SystemStateTransition stateTransition=new SystemStateTransition())
//				{
//
//					AbstractionsAndPersistency.Order order=storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
//					order.Name="Loasas";
//					//AbstractionsAndPersistency.OrderDetail orderDetail= new AbstractionsAndPersistency.OrderDetail();// storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
//					AbstractionsAndPersistency.OrderDetail orderDetail= storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
//
//					order.AddItem(orderDetail);
//					stateTransition.Consistent=true;
//				}/**/
			}
			catch(System.Exception err)
			{
				int wer=0;
			}

			return ;


			System.TimeSpan NewTimeSpan;
			System.TimeSpan ComiteTimeSpan;
			System.DateTime before;
			System.DateTime After;
			before=System.DateTime.Now;
			Cursor=Cursors.WaitCursor;

			try
			{
				try
				{

					if(storageSession==null)
						storageSession=OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("MSSQLFast",System.Net.Dns.GetHostName(),"OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider");

				}
				catch(OOAdvantech.PersistenceLayer.StorageException Error)
				{
					if(Error.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
					{
						storageSession=ObjectStorage.NewStorage("MSSQLFast",
							System.Net.Dns.GetHostName(),
							"OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider",true);
						string assemblyFullName= typeof(OOAdvantech.MetaDataRepository.Class).Assembly.FullName;
						storageSession.StorageMetaData.RegisterComponent(assemblyFullName);
					}
					else
						throw Error;
				}
				After=System.DateTime.Now;
				ComiteTimeSpan=After-before;
				System.Diagnostics.Debug.WriteLine(ComiteTimeSpan.ToString());
			/*using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{

					System.Type[] employeeParameterTypes=new System.Type[2]{typeof(string),typeof(double)};
					Family.Employee jack=storageSession.NewObject(typeof(Family.Employee),
						"Jack",1500,employeeParameterTypes) as Family.Employee;

					System.Type[] jobParameterTypes=new System.Type[2]{typeof(string),typeof(DateTime)};
					Family.Job electrician=storageSession.NewObject(typeof(Family.Job),
						"Electrician",DateTime.Parse("10/11/2004"),jobParameterTypes) as Family.Job;
					
					electrician.Employee=jack;
					
					System.Type[] companyParameterTypes=new System.Type[1]{typeof(string)};
					Family.Company genecom=storageSession.NewObject(typeof(Family.Company),
						"Genecom",companyParameterTypes) as Family.Company;
					genecom.AddEmployee(electrician);

					sysStateTransition.Consistent=true;
				}
			
	
				
				//string Query="SELECT employee FROM "+typeof(Family.Employee).FullName+" employee WHERE employee.Name = 'Kitsos'";
				string Query="SELECT Class FROM "+typeof(OOAdvantech.MetaDataRepository.Class).FullName+" Class WHERE Name = 'Employee'";
				before=System.DateTime.Now;
				StructureSet objectSet=storageSession.Execute(Query);

				
				foreach(StructureSet objectSetInstance in objectSet)
				{
					OOAdvantech.MetaDataRepository.Class _class=objectSetInstance["Class"] as OOAdvantech.MetaDataRepository.Class;

					int were=0;
				}*/

				After=System.DateTime.Now;
				ComiteTimeSpan=After-before;
				System.Diagnostics.Debug.WriteLine(ComiteTimeSpan.ToString());


				WriteData();
			}
			catch(System.Exception Error)
			{
				int ete=0;
			}
			Cursor=Cursors.Default;
			return;
			


			try
			{
	

				Cursor=Cursors.WaitCursor;

				System.DateTime befor= System.DateTime.Now;
				if(storageSession ==null)
				{
					storageSession = ObjectStorage.NewStorage("MSSQLFast",System.Net.Dns.GetHostName(),"OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider",true);
					storageSession.StorageMetaData.RegisterComponent(typeof(Family.Employee).Assembly.FullName); 
				}
				string Query="SELECT employee FROM "+typeof(Family.Employee).FullName+" employee WHERE employee.Name = 'Kitsos'";
				storageSession.Execute(Query);

				//WriteData();
				System.DateTime after = System.DateTime.Now;
				System.TimeSpan tt=after-befor;
				string ertwer=tt.ToString();
				System.Diagnostics.Debug.WriteLine(ertwer);
				int tsst=0;
				




				
				

				return;

			}
			catch(System.Exception Error)
			{
				int ew4r=0;
			}
			finally
			{
				Cursor=Cursors.Default;
			}
			
			//BinaryFormat();
			


			return;
			System.Collections.ArrayList ert=new System.Collections.ArrayList(4000);
			for(int i=0;i<00001;i++)
			{
				ert.Add(new Family.Person("Mitsos"));

			}
			System.IO.MemoryStream aerr=new MemoryStream(100);
			int sdf=123;

			//aerr.Write(System.Convert.to

			

			Stream s = File.Open("foo.bin", FileMode.Create, FileAccess.ReadWrite);
			BinaryFormatter b = new BinaryFormatter();

			
			b.Serialize(s, new Family.Person("Mitsos"));
			
			
			s.Close();
			

			
			Stream r = File.Open("foo.bin", FileMode.Open, FileAccess.Read);
			BinaryFormatter c = new BinaryFormatter();
			System.Collections.ArrayList p = (System.Collections.ArrayList) c.Deserialize(r);
			//(p[20000] as Family.Person).Name=
			r.Close();

			
			Family.Person peras= OOAdvantech.Remoting.RemotingServices.CreateInstance("tcp://"+System.Net.Dns.GetHostName()+":4000",typeof(Family.Person).FullName)as Family.Person;
			peras.AdvanceAge();
			return;
			
			Color bkcol=Sample.BackColor;
			try
			{
				Sample.BackColor=Color.Red;
				Cursor=Cursors.WaitCursor;
	
					storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				ObjectStorage vmStorageSession=ObjectStorage.OpenStorage("Family",
					"vmWin2000",
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				using(SystemStateTransition stateTransition =new SystemStateTransition())
				{

					System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
					Family.Person person=storageSession.NewObject(typeof(Family.Person),"Jack",parametersTypes) as Family.Person;
					Family.Person vmperson=vmStorageSession.NewObject(typeof(Family.Person),"Jack",parametersTypes) as Family.Person;
					stateTransition.Consistent=true;
				}
			}
			catch(System.Exception Error)
			{
				int ttr=0;
			}
			finally
			{
				Sample.BackColor=bkcol;
				Cursor=Cursors.Default;
			}



			return;/**/
			System.Runtime.InteropServices.UCOMIEnumMoniker Enum=null;
			System.Runtime.InteropServices.UCOMIMoniker[]  UCOMIMonikers=new System.Runtime.InteropServices.UCOMIMoniker[1]; 
			System.Runtime.InteropServices.UCOMIRunningObjectTable ROT=null;
	

			
			
			if(ROT==null)
				COMApi.GetRunningObjectTable(0,out ROT);

			ROT.EnumRunning(out Enum);
			Enum.Reset();
			int res;
			int Count=0;
			while(Enum.Next(1,UCOMIMonikers,out res)==0)
			{
				
				
				Count++;
				int tert=0;
			}


			Family.Person mPerson=new Family.Person();
			using(ObjectStateTransition stateTransition =new ObjectStateTransition(mPerson))
			{
				stateTransition.Consistent=true;
			}

			ROT.EnumRunning(out Enum);
			Enum.Reset();
			Count=0;
			while(Enum.Next(1,UCOMIMonikers,out res)==0)
			{
				
				
				Count++;
				int tert=0;
			}
			int taat=0;



			/*

			//"jack"

			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			string objectQuery="SELECT employee FROM "+typeof(Family.Employee).FullName+" employee WHERE employee.Name='jack'";
			//SELECT person FROM Family.Person person 
			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Employee employee= objectSetInstance["employee"] as Family.Employee;
				employee.GetCompany("Electrician");
				
			}*/

		}

		private void ClientForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("MouseMove");
		
		}
        System.Diagnostics.Process process;

		private void ClientForm_Load(object sender, System.EventArgs e)
		{
		}
        void OnProcessExit(object sender, EventArgs e)
        {
            int ttrt = 6;

        }
	}
}

