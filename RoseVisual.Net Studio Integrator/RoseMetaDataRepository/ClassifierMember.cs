using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{859b513b-fb6a-4085-ac48-5fab3b990980}</MetaDataID>
    internal class ClassifierMember
    {
        public readonly RationalRose.RoseItem RoseItem;
        public ClassifierMember(RationalRose.RoseItem roseItem)
        {
            RoseItem = roseItem;
            
        }
        public string Name
        {
            get
            {
                return RoseItem.Name;
            }
        }
        public System.Drawing.Image Image
        {
            get
            {
                if (RoseItem is RationalRose.RoseOperation)
                {

                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseOperation).ExportControl.Name);
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

                if (RoseItem is RationalRose.RoseAttribute)
                {
                    OOAdvantech.MetaDataRepository.VisibilityKind visibility = RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseAttribute).ExportControl.Name);
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
                if (RoseItem is RationalRose.RoseRole)
                    return Properties.Resources.NavigableRole;

                return Properties.Resources.PublicOperation;

            }
        }



    }
}
