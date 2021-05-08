namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{76ABF67D-FA1A-4784-BDF4-E0299B21E1FA}</MetaDataID>
	[BackwardCompatibilityID("{76ABF67D-FA1A-4784-BDF4-E0299B21E1FA}")]
	[Persistent()]
	public class Model : Package
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
