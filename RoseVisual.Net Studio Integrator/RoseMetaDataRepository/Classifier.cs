using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    public class ClassifierPresentation:OOAdvantech.UserInterface.PresentationObject<OOAdvantech.MetaDataRepository.Classifier>
    {
        public ClassifierPresentation(OOAdvantech.MetaDataRepository.Classifier classifier):base(classifier)
        {

        }
        public OOAdvantech.Collections.Generic.Set<ClassifierMember> Members
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<ClassifierMember> members = new OOAdvantech.Collections.Generic.Set<ClassifierMember>();

                foreach (OOAdvantech.MetaDataRepository.Feature feature in RealObject.Features)
                    members.Add(new ClassifierMember(feature));

                return members;
            }
        }
    }
}
