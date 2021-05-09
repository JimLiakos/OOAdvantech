using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using OOAdvantech.Remoting;
using OOAdvantech.Collections.Generic;

namespace Family
{
	/// <MetaDataID>{0DAA518F-51BE-4AF6-A3FE-520BDCB798D1}</MetaDataID>
	[BackwardCompatibilityID("{0DAA518F-51BE-4AF6-A3FE-520BDCB798D1}")]
	[Persistent]
	public class Company:MarshalByRefObject,IExtMarshalByRefObject
	{
		/// <MetaDataID>{3E9441AD-AFF0-44ED-8F1A-E5D6E9468976}</MetaDataID>
		public Company()
		{
		}
		/// <MetaDataID>{56877E32-A9C3-4AD8-86C4-0DE01794356C}</MetaDataID>
		public Company(String name)
		{
			_Name=name;
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{D4061682-A461-4744-BF05-2F4CF1BA815A}</MetaDataID>
		private Set<Job> _Employees=new Set<Job>();
		/// <MetaDataID>{340017EE-8FCC-459A-AA27-FE9F269234C7}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[Association("Job",Roles.RoleB,false)]
		[PersistentMember("_Employees")]
		[RoleBMultiplicityRange(0)]
		[AssociationClass(typeof(Family.Job))]
		public Set<Job> Employees
		{
			get
			{
                return new Set<Job>(_Employees);
			}
		}
		/// <MetaDataID>{BC9DC660-4F5C-4B0F-87E9-972830BA765E}</MetaDataID>
	public void AddEmployee(Job job)
	{
		if(job==null)
			return;
		
		if(job.Employee==null)
			throw new System.Exception("the member Employee of job isn't set");

		using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
		{
			_Employees.Add(job);
			stateTransition.Consistent=true;
		}
	}
	/// <MetaDataID>{9F87B1EA-6DA4-4EE6-8845-B3A5C0C384AF}</MetaDataID>
	public void RemoveEmployee(Job job)
	{
		if(job==null)
			return;

		using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
		{
			_Employees.Remove(job);
			stateTransition.Consistent=true;
		}
	}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{40CCC86C-6B1C-47BE-A72B-68805F96D14C}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{5A6398A5-F27A-4B22-BEE3-B46FA8000AFA}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember("_Name")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}

		/// <MetaDataID>{6DF734BC-4E51-43B2-9E8B-703C4784E7F4}</MetaDataID>
		public OOAdvantech.ObjectStateManagerLink Properties;




	}
}
