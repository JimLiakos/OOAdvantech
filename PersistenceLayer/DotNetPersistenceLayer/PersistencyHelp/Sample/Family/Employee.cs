using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections.Generic;

namespace Family
{
    /// <MetaDataID>{E9D67233-E834-45FE-8246-A772E0698E2D}</MetaDataID>
    [BackwardCompatibilityID("{E9D67233-E834-45FE-8246-A772E0698E2D}")]
    [Persistent()]
    public class Employee : Person
    {
        /// <MetaDataID>{1CAF51C7-86BB-42A7-9AD0-6CBE1E73D056}</MetaDataID>
        public override Address Address
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{872E8535-E4B8-4B94-9191-D63269BD087B}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Job> _Employers = new Set<Job>();
        /// <MetaDataID>{EBD1C578-A676-4C89-861A-C8B6978A8279}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [Association("Job", Roles.RoleA,false)]
        [PersistentMember("_Employers")]
        [RoleAMultiplicityRange(0)]
        [AssociationClass(typeof(Family.Job))]
        public Set<Job> Employers
        {
            get
            {
                return new Set<Job>(_Employers);
            }
        }

        /// <MetaDataID>{5b839bf6-34e3-4ded-af7e-17056ed2aa8a}</MetaDataID>
        [PersistentMember("Department")]
        public string Department;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{577AF56B-3A6C-41BC-AC37-96A385DA9067}</MetaDataID>
        private double _Salary;
        /// <MetaDataID>{A66E6156-BF8F-42CC-992E-7A9D974E33A2}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_Salary")]
        public double Salary
        {
            get
            {
                return _Salary;
            }
            set
            {
                if (_Salary != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        _Salary = value;
                        objStateTransition.Consistent = true; ;
                    }
                }
            }
        }


        /// <MetaDataID>{17D38ED8-E950-41C2-8BF8-B658305D0EBA}</MetaDataID>
        public Employee()
        {

        }
        /// <MetaDataID>{8CBD0998-1922-42FF-A7D7-26304021AC11}</MetaDataID>
        public Employee(string name, double salary)
            : base(name)
        {
            _Salary = salary;
        }
        /// <MetaDataID>{598EED59-DB5D-42AB-A0AC-9DC7FB00DB9E}</MetaDataID>
        public Company GetCompany(string jobName)
        {

            ObjectStorage storageSession = ObjectStorage.GetStorageOfObject(Properties);

            string objectQuery = "SELECT employee.Employers company FROM " + typeof(Employee).FullName + " employee WHERE employee=@this AND employee.Job.Name=@jobName";
            OOAdvantech.Collections.Generic.Dictionary<string, object> parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>(2);
            parameters["@this"] = this;
            parameters["@jobName"] = jobName;
            OOAdvantech.Collections.StructureSet objectSet = storageSession.Execute(objectQuery, parameters);
            foreach (OOAdvantech.Collections.StructureSet objectSetInstance in objectSet)
            {
                Company company = objectSetInstance["company"] as Company;
                System.Diagnostics.Debug.WriteLine(company.Name);
                return company;
            }
            return null;


        }




    }
}
