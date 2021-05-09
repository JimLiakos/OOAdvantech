using System.Linq;
namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{1B30F9AC-A914-4AF1-BF01-98D8E2376BCA}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{1B30F9AC-A914-4AF1-BF01-98D8E2376BCA}")]
	[MetaDataRepository.Persistent("<ExtMetaData><InheritanceMapping>OneTablePerConcreteClass</InheritanceMapping></ExtMetaData>")]
	public class Primitive : MetaDataRepository.Primitive
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                var implementationUnit = base.ImplementationUnit;
                if (implementationUnit != null && implementationUnit.Context == null)
                {
                    if (PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                    {
                        OOAdvantech.Linq.Storage theStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                        var storage = (from metastorage in theStorage.GetObjectCollection<Storage>() select metastorage).FirstOrDefault();
                        implementationUnit.Context = storage;

                    }
                }
                return implementationUnit;
            }
        }
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        { 
            base.Synchronize(originMetaObject);
        }
       
	}
}
