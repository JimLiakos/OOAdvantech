using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Remoting;
using System;

namespace Family
{
	/// <MetaDataID>{21FAA608-30F0-4A82-84E7-E4AB59A063E3}</MetaDataID>
	[BackwardCompatibilityID("{21FAA608-30F0-4A82-84E7-E4AB59A063E3}")]
	[Persistent()]
	[AssociationClass(typeof(Family.Company),typeof(Family.Employee),"Job")]
	public class Job:MarshalByRefObject,IExtMarshalByRefObject
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{340A178E-51C6-4746-B2CD-04567EC5870B}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{16BE0A6E-D9C9-42F5-9657-54AE8CB8BF8E}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember(PersistencyFlag.LazyFetching,"_Name")]
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
		/// <MetaDataID>{6DA47A21-6753-47EB-82DD-BFB736FF190D}</MetaDataID>
		[BackwardCompatibilityID("+3")]
		[PersistentMember()]
		public DateTime StartingDate;
		/// <MetaDataID>{21ED7DB8-A2DB-4A9D-96EC-8FA15411458F}</MetaDataID>
		public Job()
		{
		}
		/// <MetaDataID>{7E81455A-A29C-4B44-8BCA-B9357091BB13}</MetaDataID>
		public Job(string name, DateTime startingDate)
		{
			_Name=name;
			 StartingDate=startingDate;
		}
		/// <MetaDataID>{1F15D6F4-D36A-4AFD-9947-BA841926D708}</MetaDataID>
		public OOAdvantech.ObjectStateManagerLink Properties;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{28027EBF-9C60-451D-8E51-0543B19BE185}</MetaDataID>
		private Employee _Employee;
		/// <MetaDataID>{43BE7DC4-A8B2-4C1C-90B6-F7A0064DA70F}</MetaDataID>
		[AssociationClassRole(Roles.RoleB,"_Employee")]
		public Employee Employee
		{
			get
			{
				return _Employee;
			}
			set
			{
				if(_Employee==value)
					return;
				using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
				{
					_Employee=value;
					stateTransition.Consistent=true;
				}
			}
		}
		
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{55B08659-6780-487C-A61F-E8F7C274AB7F}</MetaDataID>
		private Company _Employer;
		/// <MetaDataID>{A542640B-FE38-4977-995E-322BC4089155}</MetaDataID>
		[AssociationClassRole(Roles.RoleA,"_Employer")]
		public Company Employer
		{
			get
			{
				return _Employer;
			}
			set
			{
				if(_Employer==value)
					return;
				using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
				{
					_Employer=value;
					stateTransition.Consistent=true;
				}
			}
		}
	}
}
