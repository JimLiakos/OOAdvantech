namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{54B39C49-8C82-4A0E-AA9E-90B6A97E677C}</MetaDataID>
	/// <summary>A relationship is a connection among model elements.
	/// In the metamodel, Relationship is a term of convenience without any specific semantics. It is abstract.
	/// Children of Relationship are Association, Dependency, Realization and Generalization. </summary>
	[MetaDataRepository.BackwardCompatibilityID("{54B39C49-8C82-4A0E-AA9E-90B6A97E677C}")]
	[MetaDataRepository.Persistent()]
	public abstract class Relationship : MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }
    }
}
