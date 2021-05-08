namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{A9C06BCD-2763-4CBC-B9E9-7FEA4180EE87}</MetaDataID>
	public abstract class BehavioralFeature : Feature
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
