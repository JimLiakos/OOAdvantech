using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{9aca7702-c8f2-469f-8bc4-83e2942254dc}</MetaDataID>
    public class TemplateSignature : MetaDataRepository.TemplateSignature
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{863b17f4-c4ab-4986-8586-3f935042f893}</MetaDataID>
        public TemplateSignature(MetaDataRepository.ITemplateable template)
            : base(template)
        {

        }
        /// <MetaDataID>{6295643e-92f5-415a-af36-5fad33e4b116}</MetaDataID>
        protected TemplateSignature()
        {
        }
    }
}
