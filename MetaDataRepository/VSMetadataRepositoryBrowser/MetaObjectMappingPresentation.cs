using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using VSMetadataRepositoryBrowser.RDBMSMapping;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{8ae4d66e-e72d-4409-988f-15e8c7382c94}</MetaDataID>
    public class MetaObjectMappingPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.MetaObject>
    {
        /// <MetaDataID>{59828d90-ebad-444f-951b-0a15146fd414}</MetaDataID>
        public MetaObjectMappingPresentation(MetaObject metaObject)
            : base(metaObject)
        {
           
        }


        /// <MetaDataID>{621e338a-e678-43f1-85fd-9020f4a3d5fa}</MetaDataID>
        public ConnectableControls.DynamicViewContainer.UserViewControlIdentity MetaObjectView
        { 
            get
            { 
                if(RealObject is Class)
                    return ConnectableControls.UserControl.GetUserControlIdentity<ClassView>();
                if (RealObject is OOAdvantech.CodeMetaDataRepository.Project)
                     return ConnectableControls.UserControl.GetUserControlIdentity<ProjectView>();
                return ConnectableControls.DynamicViewContainer.UserViewControlIdentity.Empty;
            }
        }

    }
}
