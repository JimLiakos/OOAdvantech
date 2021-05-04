using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{c94ca6c7-5c90-4025-89f2-d4f6d0b2cdc8}</MetaDataID>
    internal class SychronizationItem : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.MetaObject>
    {

        /// <MetaDataID>{747c6a82-5976-4dc0-82b4-86152b05c605}</MetaDataID>
        public SychronizationItem(OOAdvantech.MetaDataRepository.MetaObject metaObject)
            : base(metaObject)
        {
        }

        bool _Delete;
        public bool Delete
        {
            get
            {
                return _Delete;
            }
            set
            {
                _Delete = value;
            }
        }

        /// <MetaDataID>{423c3b3d-81e5-41ae-bf7c-ba6b19e5012b}</MetaDataID>
        public string Name
        {
            get
            {
                return RealObject.Name;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                if (RealObject is OOAdvantech.MetaDataRepository.Operation )
                {

                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Operation).Visibility;//RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseOperation).ExportControl.Name);
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                        return Properties.Resources.ImplOperation;
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                        return Properties.Resources.PrivateOperation;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                        return Properties.Resources.ProtectedOperation;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        return Properties.Resources.PublicOperation;
                }

                if (RealObject is OOAdvantech.MetaDataRepository.Attribute )
                {
                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Attribute).Visibility;// RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseAttribute).ExportControl.Name);
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                        return Properties.Resources.ImplAttr;
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                        return Properties.Resources.PrivateAttribute;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                        return Properties.Resources.PublicAttribute;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        return Properties.Resources.PublicAttribute;

                }
                if (RealObject is OOAdvantech.MetaDataRepository.Method)
                {

                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Method).Visibility;//RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseOperation).ExportControl.Name);
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                        return Properties.Resources.ImplOperation;
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                        return Properties.Resources.PrivateOperation;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                        return Properties.Resources.ProtectedOperation;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        return Properties.Resources.PublicOperation;
                }

                if (RealObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                {
                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Visibility;// RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseAttribute).ExportControl.Name);
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                        return Properties.Resources.ImplAttr;
                    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                        return Properties.Resources.PrivateAttribute;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                        return Properties.Resources.PublicAttribute;
                    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        return Properties.Resources.PublicAttribute;

                }
                if (RealObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                    return Properties.Resources.NavigableRole;
                if (RealObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                    return Properties.Resources.NavigableRole;

                return Properties.Resources.PublicOperation;

            }
        }




    }

}
