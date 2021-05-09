using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualStudioUMLCodeGenerator
{
    /// <MetaDataID>{e57f55dc-1ed6-4178-8aca-e5a00fbe6194}</MetaDataID>
    public class ComponentPresentationObject : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Component>
    {

        public ComponentPresentationObject(OOAdvantech.MetaDataRepository.Component component)
            : base(component)
        {
           
        }

        public List<RealizedClassifierPresentationObject> Classifiers
        {
            get
            {
                return new List<RealizedClassifierPresentationObject>();
            }
        }
    }
}
