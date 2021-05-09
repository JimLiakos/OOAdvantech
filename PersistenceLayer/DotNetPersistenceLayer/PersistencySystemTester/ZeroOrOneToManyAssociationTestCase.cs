using System;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using OOAdvantech.Collections;

namespace PersistencySystemTester
{
    /// <summary></summary>
    /// <MetaDataID>{cab428cc-ef9c-4303-8b35-6d6a4ca007c0}</MetaDataID>
	public class ZeroOrOneToManyAssociationTestCase
	{

		
		public void SaveATransientObject()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			Family.Person person=null;
			Family.Dog dog=null;

			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				
				System.Type[]  parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				dog=storageSession.NewObject(typeof(Family.Dog),parametersTypes,"Spitha","husky") as Family.Dog;


				parametersTypes=new System.Type[1]{typeof(string)};
				person=OOAdvantech.Remoting.RemotingServices.CreateInstance(OOAdvantech.Remoting.RemotingServices.GetChannelUri(dog),
                    typeof(Family.Person).FullName, "", parametersTypes, "Person") as Family.Person;
					
				//dog.Owner=person;
				person.AddDog(dog);

				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				dog=storageSession.NewObject(typeof(Family.Dog),parametersTypes,"SpithaB","husky") as Family.Dog;
				person.AddDog(dog);
				sysStateTransition.Consistent=true;
			}

			storageSession.CommitTransientObjectState(person);

		}

		public void AssignNewObjectCollection()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			Family.Person person=null;
			Family.Dog dog=null;

			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};

				dog=storageSession.NewObject(typeof(Family.Dog),parametersTypes,"Spitha","husky") as Family.Dog;
				dog.Owner=person;
				person.AddDog(dog);
				sysStateTransition.Consistent=true;
			}
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				person.Name="Ermis";
				person.ReAssignDogsObjectCollection();
				sysStateTransition.Consistent=true;
			}

		}
		public void DoubleLinkCommandCheckOneToMany()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

			
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{
				Family.Person person=null;
				Family.Dog dog=null;
				

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};

				dog=storageSession.NewObject(typeof(Family.Dog),parametersTypes,"Spitha","husky") as Family.Dog;
				dog.Owner=person;
				person.AddDog(dog);
				sysStateTransition.Consistent=true;
			}

		}

		public void TryToAssigneTwoObjectToZerOrOneAssocitionEnd()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT dog FROM "+typeof(Family.Dog).FullName+" dog WHERE dog.Name = 'Spitha'";

			Family.Dog dog=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				dog=objectSetInstance["dog"] as Family.Dog;

			}

			Family.Person person=null;
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person1") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				person.AddDog(dog);
				sysStateTransition.Consistent=true;
			}
			long tmp =person.Dogs.Count;
			int eree=0;

		}

		public void AssigneObjectToOtherObjectOnZeroOrOneAssocitionEnd()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT dog FROM "+typeof(Family.Dog).FullName+" dog WHERE dog.Name = 'Spitha'";

			Family.Dog dog=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				dog=objectSetInstance["dog"] as Family.Dog;
				break;
			}

			Family.Person person=null;
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person1") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				dog.Owner.RemoveDog(dog);
				dog.Owner=person;
				sysStateTransition.Consistent=true;
			}
			long tmp =person.Dogs.Count;
			int eree=0;

		}

		public void AssigneOtherObjectToLezyFetchingZeroOrOneAssocitionEnd()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT dog FROM "+typeof(Family.Dog).FullName+" dog WHERE dog.Name = 'Spitha'";

			Family.Dog dog=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				dog=objectSetInstance["dog"] as Family.Dog;
				break;
			}

			Family.Person person=null;
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person1") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				dog.Owner=person;
				sysStateTransition.Consistent=true;
			}
			long tmp =person.Dogs.Count;
			int eree=0;

		}

		public void AssigneOtherObjectToLezyFetchingZeroOrOneAssocitionEndB()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT dog FROM "+typeof(Family.Dog).FullName+" dog WHERE dog.Name = 'Spitha'";

			Family.Dog dog=null;

			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				dog=objectSetInstance["dog"] as Family.Dog;
				break;
			}

			Family.Person person=null;
			Family.Person personB=null;
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				System.Type[] parametersTypes=new System.Type[1]{typeof(string)};
				person=storageSession.NewObject(typeof(Family.Person),parametersTypes,"Person1") as Family.Person;
				personB=storageSession.NewObject(typeof(Family.Person),parametersTypes,"PersonB") as Family.Person;
				parametersTypes=new System.Type[2]{typeof(string),typeof(string)};
				dog.Owner=person;
				personB.AddDog(dog);
				sysStateTransition.Consistent=true;
			}
			long tmp =person.Dogs.Count;
			int eree=0;

		}


		public void RemoveAllObjectsFromAssocitionEndWithManyMultiplicity()
		{
			ObjectStorage storageSession=ObjectStorage.OpenStorage("Family",
				System.Net.Dns.GetHostName(),
				"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


			string objectQuery="SELECT person FROM "+typeof(Family.Person).FullName+" person ";

			Family.Dog dog=null;

			Family.Person person=null;
			StructureSet objectSet=storageSession.Execute(objectQuery);
			foreach(StructureSet objectSetInstance in objectSet)
			{
				person=objectSetInstance["person"] as Family.Person;
				break;
			}

			
			using(SystemStateTransition sysStateTransition=new SystemStateTransition())
			{

				foreach(Family.Dog tmpDog in person.Dogs)
				{
					dog=tmpDog;
					person.RemoveDog(dog);
				}
				sysStateTransition.Consistent=true;
			}

			Family.Person owner= dog.Owner;
			long tmp =person.Dogs.Count;
			int eree=0;

		}


	}
}
