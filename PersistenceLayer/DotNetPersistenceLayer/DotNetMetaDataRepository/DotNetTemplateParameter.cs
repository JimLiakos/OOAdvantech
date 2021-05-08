namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{0D532879-DFD1-4D51-A3DA-6D4090548E8C}</MetaDataID>
    public class TemplateParameter : OOAdvantech.MetaDataRepository.TemplateParameter
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(Refer))
            {
                if (value == null)
                    Refer = default(OOAdvantech.DotNetMetaDataRepository.Type);
                else
                    Refer = (OOAdvantech.DotNetMetaDataRepository.Type)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(Refer))
                return Refer;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{72728ace-5df8-4142-ac2d-e81f3aaf7426}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{30595192-dc74-4351-bfc3-4bd5e16b0917}</MetaDataID>
        protected TemplateParameter()
        {
        }
        /// <MetaDataID>{28cb1847-1466-45e0-872a-c35afc3f812b}</MetaDataID>
        public TemplateParameter(string name)
            : base(name)
        {

        }
        /// <MetaDataID>{eecd0e8c-b785-4484-b9b7-a0d4b10c7ea2}</MetaDataID>
        internal Type Refer;
        /// <MetaDataID>{b0d36b5e-ed21-4dd3-add4-7e3a10673077}</MetaDataID>
        public TemplateParameter(DotNetMetaDataRepository.Type theType)
        {
            if (theType.WrType.IsGenericParameter)
            {
                _ImplementationUnit.Value = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType.GetMetaData().Assembly) as Assembly;
                _Name = theType.Name;
                Refer = theType;
                Visibility = Refer.Visibility;
                DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(theType.WrType, this);

                if (Refer.IsNestedType)
                    return;
                if (!string.IsNullOrEmpty(theType.WrType.Namespace))
                {
                    Namespace mNamespace = Type.GetNameSpace(theType.WrType.Namespace);
                    //Namespace mNamespace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType.Namespace);
                    //if (mNamespace == null)
                    //    mNamespace = new Namespace(theType.WrType.Namespace);
                    mNamespace.AddOwnedElement(this);
                    SetNamespace(mNamespace);
                }
            }
            else
                if (string.IsNullOrEmpty(theType.WrType.Namespace))
                    throw new System.Exception("The type " + theType.WrType.Name + " isn't generic parameter.");
                else
                    throw new System.Exception("The type " + theType.WrType.Namespace + "." + theType.WrType.Name + " isn't generic parameter.");

        }

		

    }
}
