using System;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using OOAdvantech.Collections;

namespace PersistencySystemTester
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    /// <MetaDataID>{76b7f36b-d668-42c6-83f5-12a7f297c564}</MetaDataID>
	public class CreateStorageTestCase
	{


		public void TriesToCommitOutOfStorageProcessObject()
		{
			try
			{ 
				ObjectStorage objectStorage=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{

					Family.Person person=new Family.Person("Lorka");
					objectStorage.CommitTransientObjectState(person);
					sysStateTransition.Consistent=true;
				}
			}
			catch(System.Exception Error)
			{
				int er=0;
			}


		}

		public void CommitTransientObject()
		{
			try
			{
				ObjectStorage objectStorage=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
                Family.Person person = objectStorage.NewTransientObject(typeof(Family.Person), parametersTypes, "Jack") as Family.Person;
				using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{
					objectStorage.CommitTransientObjectState(person);
					sysStateTransition.Consistent=true;
				}
			}
			catch(System.Exception Error)
			{
				int er=0;
			}


		}

		/// <summary>
		/// Create a new object in storage Family with help of object constructor with parameters.
		/// </summary>
		public void PersistentObjectCreation()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			Family.Person person=null;
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
                person = storageSession.NewObject(typeof(Family.Person), parametersTypes, "Jack") as Family.Person;
				sysStateTransition.Consistent=true;
			}

			if(person!=null)
			{
				string objectQuery="SELECT person FROM  Family.Person person WHERE person = "+StorageInstanceRef.GetStorageInstanceRef(person).ObjectID.ToString();
//
//				System.ServiceProcess.ServiceController PersistencyServiceController=new System.ServiceProcess.ServiceController("PersistencyService");
//				PersistencyServiceController.Stop();
//				System.Threading.Thread.Sleep(1000);
//				PersistencyServiceController.Start();
//				int Count=20;
//				while(PersistencyServiceController.Status!=System.ServiceProcess.ServiceControllerStatus.Running&&Count>=0)
//				{
//					
//					System.Threading.Thread.Sleep(1000);
//					PersistencyServiceController.Refresh();
//					Count--;
//				}

				storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				
				StructureSet objectSet=storageSession.Execute(objectQuery);
				foreach(StructureSet objectSetInstance in objectSet)
				{
					person = objectSetInstance["person"] as Family.Person;
					System.Diagnostics.Debug.WriteLine("Person name:"+person.Name); 
				}

				int mtt=0;
			}
			
		}
		/// <summary>
		/// The main role of this test case is to test the case where we add 
		/// the object A in a collection of object B and B in a collection of object A. 
		/// The two collections belong to the same Association. The result of this action is the production 
		/// of exactly one Link command from the persistence system. The two objects runs in the same process.
		/// We have to put a brake point at OOAdvantech.PersistenceLayerRunTime.Commands.LinkObjectsCommand.Execute
		/// </summary>
		public void DoubleLinkCommandCheck()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				Family.Person parent=null;
				Family.Person child=null;

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
                parent = storageSession.NewObject(typeof(Family.Person), parametersTypes, "Parent") as Family.Person;
                child = storageSession.NewObject(typeof(Family.Person), parametersTypes, "Child") as Family.Person;
				child.AddParent(parent);
				parent.AddChild(child);
				sysStateTransition.Consistent=true;
			}

		}



		public void AbortObjectsLinkChanges()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				Family.Person parent=null;
				Family.Person child=null;

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
                parent = storageSession.NewObject(typeof(Family.Person), parametersTypes, "Parent") as Family.Person;
                child = storageSession.NewObject(typeof(Family.Person), parametersTypes, "Child") as Family.Person;
				child.AddParent(parent);
				parent.AddChild(child);
				//sysStateTransition.Consistent=true;
			}

		}
		/// <summary>
		/// Retrieve a parent and his child from storage 
		/// adds parent to the child and child to the parent and transaction doesn't commited;
		/// This test case is useful for check of RelationChangements under transaction abort  
		/// </summary>
		public void AddAnAlreadyExistObjectToCollection()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;

			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.AddParent(parent);
				parent.AddChild(child);
				sysStateTransition.Consistent=true;
			}


		}


		public void DeleteANoneExistObjectFromCollection()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;

			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.RemoveParent(parent);
				parent.RemoveChild(child);
				sysStateTransition.Consistent=true;
			}
		}

		public void DeleteAExistObjectButNoLoadedFromCollection()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;

			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.RemoveParent(parent);
				parent.RemoveChild(child);
				sysStateTransition.Consistent=true;
			}
		}

		public void UnlinkObjectsInManyToManyRelationship()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;

			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.RemoveParent(parent);
				//parent.RemoveChild(child);
				sysStateTransition.Consistent=true;
			}
		}
		public void AddTransientObjectToManyToManyPersistentRelationship()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";
			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;
			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				string channelUri =OOAdvantech.Remoting.RemotingServices.GetChannelUri(parent);
				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				Family.Person person=OOAdvantech.Remoting.RemotingServices.CreateInstance(channelUri,typeof(Family.Person).FullName,"",parametersTypes,"Child") as Family.Person ;
				parent.AddChild(person);
				bool hasChild=parent.HasChild(person);
				long tt=parent.Childrens.Count;
				
				sysStateTransition.Consistent=true;
			}
		}

		public void DeleteTransientObjectFromManyToManyPersistentRelationship()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";
			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;
			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				string channelUri =OOAdvantech.Remoting.RemotingServices.GetChannelUri(parent);
				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				Family.Person person=OOAdvantech.Remoting.RemotingServices.CreateInstance(channelUri,typeof(Family.Person).FullName,"",parametersTypes,"Child") as Family.Person ;
				parent.RemoveChild(person);
				long tt=parent.Childrens.Count;
				sysStateTransition.Consistent=true;
			}
		}


		public void UnlinkObjectsInManyToManyRelationshipB()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;
			}

			long  parentsCount=child.Parents.Count;
			long ChildrensCount=parent.Childrens.Count;

			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.RemoveParent(parent);
				//parent.RemoveChild(child);
				sysStateTransition.Consistent=true;
			}
		}

		public void AddToRoleACollectionAndRemoveFromRoleBCollection()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person WHERE person.Name = 'Parent' OR person.Name = 'Child'";

			Family.Person parent=null;
			Family.Person child=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				Family.Person person=objectSetInstance["person"] as Family.Person;

				if(person.Name=="Parent")
					parent=person;
				else
					child=person;
			}

//			long  parentsCount=child.Parents.Count;
//			long ChildrensCount=parent.Childrens.Count;

			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				child.RemoveParent(parent);
				parent.AddChild(child);
				sysStateTransition.Consistent=true;
			}
		}




		public void GetObject()
		{
			try
			{
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
			}
		}

		public void ChangeObjectState()
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

		public void LiveAtRelationTest()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

                Family.Person Jack = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Jack") as Family.Person;
                Family.Person John = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "John") as Family.Person;
                Family.Person George = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "George") as Family.Person;

				
				Family.Address address=storageSession.NewObject(typeof(Family.Address),
					new System.Type[3]{typeof(string),
										  typeof(string),
										  typeof(string)},
					"LONDON","PADDINGTON","SUSSEX GARDENS 35")as Family.Address;

				Jack.Address=address;
				John.Address=address;
				George.Address=address;
				sysStateTransition.Consistent=true;
			}

		}

		public void GetPersonsAtAddress()
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



		public void Delete()
		{
			try
			{
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			
				string objectQuery="SELECT person "+
					"FROM "+typeof(Family.Person).FullName+" person  ";

				StructureSet objectSet=storageSession.Execute(objectQuery);
				Family.Person person=null;
				foreach(StructureSet objectSetInstance in objectSet)
				{
					person = objectSetInstance["person"] as Family.Person;
						break;
				}
				using(SystemStateTransition stateTransition=new SystemStateTransition())
				{

					ObjectStorage.DeleteObject(person);
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
			}
		}

		public void AddFamilies()
		{
			try
			{
				ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
					System.Net.Dns.GetHostName(),
					"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				using(SystemStateTransition sysStateTransition=new SystemStateTransition())
				{

                    Family.Person father = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Jack") as Family.Person;
                    Family.Person mother = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Mary") as Family.Person;
                    Family.Person firstChild = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "George") as Family.Person;
					firstChild.AddParent(father);
					firstChild.AddParent(mother);

                    Family.Person secondChild = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Nick") as Family.Person;
					secondChild.AddParent(father);
					secondChild.AddParent(mother);

                    Family.Person thirdChild = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Loosy") as Family.Person;
					thirdChild.AddParent(father);
					thirdChild.AddParent(mother);

	
					Family.Address familyAddress=storageSession.NewObject(typeof(Family.Address),
						                new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)},
                                              "LONDON","PADDINGTON","SUSSEX GARDENS 35")as Family.Address;

					Family.Address secondChildAddress=storageSession.NewObject(typeof(Family.Address),
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)},
                                            "LONDON","PADDINGTON","SUSSEX GARDENS 5")as Family.Address;

					father.Address=mother.Address=firstChild.Address=thirdChild.Address=familyAddress;
					secondChild.Address=secondChildAddress;

                    father = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "John") as Family.Person;
                    mother = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "sara") as Family.Person;
                    firstChild = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Lora") as Family.Person;
					firstChild.AddParent(father);
					firstChild.AddParent(mother);

                    secondChild = storageSession.NewObject(typeof(Family.Person), new System.Type[1] { typeof(string) }, "Paul") as Family.Person;
					secondChild.AddParent(father);
					secondChild.AddParent(mother);


					
					familyAddress=storageSession.NewObject(typeof(Family.Address),
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)},
                        "LONDON", "PADDINGTON", "SUSSEX GARDENS 110") as Family.Address;

					secondChildAddress=storageSession.NewObject(typeof(Family.Address),
						new System.Type[3]{
											  typeof(string),
											  typeof(string),
											  typeof(string)},
                        "LONDON", "PADDINGTON", "SUSSEX GARDENS 98") as Family.Address;
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
			}

		}


		public void LiveAtFamilyHome()
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

		public void NewEmployee()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				System.Type[] parametersTypes=new System.Type[2]{typeof(string),typeof(double)};
				Family.Employee wmployee=storageSession.NewObject(typeof(Family.Employee),new System.Type[2] { typeof(string), typeof(double) },
                    1500,  "Michael") as Family.Employee;
				sysStateTransition.Consistent=true;;
			}
		}

		public void GetPersons()
		{
			try
			{
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
			}
		}

		public void AddCompanyEmployee()
		{
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
					Family.Employee jack=storageSession.NewObject(typeof(Family.Employee),employeeParameterTypes,
						"Jack",1500) as Family.Employee;
					/**/Family.Employee lorka=storageSession.NewObject(typeof(Family.Employee),employeeParameterTypes,
							"Lorka",1200) as Family.Employee;
					Family.Employee looberk=storageSession.NewObject(typeof(Family.Employee),employeeParameterTypes,
						"Looberk",1600) as Family.Employee;
				
					System.Type[] jobParameterTypes=new System.Type[2]{typeof(string),typeof(DateTime)};
					Family.Job electrician=storageSession.NewObject(typeof(Family.Job),jobParameterTypes,
						"Electrician",DateTime.Parse("10/11/2004")) as Family.Job;
					electrician.Employee=jack;

					/**/Family.Job electronic=storageSession.NewObject(typeof(Family.Job),jobParameterTypes,
							"Electronic",DateTime.Parse("4/3/2003")) as Family.Job;
					electronic.Employee=lorka;
				
					Family.Job manager=storageSession.NewObject(typeof(Family.Job),jobParameterTypes,
						"Manager",DateTime.Parse("1/1/2003")) as Family.Job;
					manager.Employee=looberk;

					System.Type[] companyParameterTypes=new System.Type[1]{typeof(string)};
					Family.Company genecom=storageSession.NewObject(typeof(Family.Company),companyParameterTypes,
						"Genecom") as Family.Company;
					company=genecom;
					electrician.Employer=company;
					employee=jack;
					genecom.AddEmployee(electrician);
					genecom.AddEmployee(electronic);
					genecom.AddEmployee(manager);
					sysStateTransition.Consistent=true;

				}
				int ttwer=0;
	
			}
			catch(System.Exception Error)
			{
				int aw=0;
			}
			finally
			{
			}
		
		}

		public void GetEmployees()
		{
			try
			{
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

			}
		}
		/// <summary>
		/// It creates a storage with name family if does not already exist 
		/// and afterwards register the assemply family in it.
		/// </summary>
		public void FamilyStorageCreation()
		{
			
			try
			{
				ObjectStorage storageSession=null;
				try
				{
					storageSession=	ObjectStorage.OpenStorage("Family",
						System.Net.Dns.GetHostName(),
						"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
				}
				catch(OOAdvantech.PersistenceLayer.StorageException Error)
				{
					if(Error.Reason==StorageException.ExceptionReason.StorageDoesnotExist)
					{
						storageSession=ObjectStorage.NewStorage("Family",
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

					storageSession.StorageMetaData.RegisterComponent(typeof(Family.Person).Assembly.FullName);
//					storageSession.StorageMetaData.Build();
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

		}

		public void MitsosNew()
		{
		
		}

	}

}
