using System;
using System.Collections.Generic;
using System.Text;
using MetaDataRepository = OOAdvantech.MetaDataRepository;

namespace RoseMetaDataRepository
{

    /// <MetaDataID>{05b50bc3-715e-489c-a33d-b6121630d775}</MetaDataID>
    static class RoseAccessTypeConverter
    {
        /// <MetaDataID>{DF2A0CE5-BBE5-4C5C-A8DE-4F8DB461C1C0}</MetaDataID>
        public static string GetExportControl(OOAdvantech.MetaDataRepository.VisibilityKind visibility)
        {

            string exportControl = null;
            switch (visibility)
            {
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate:
                    {
                        exportControl = "PrivateAccess";
                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected:
                    {
                        exportControl = "ImplementationAccess";
                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                    {
                        exportControl = "ImplementationAccess";
                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                    {
                        exportControl = "PublicAccess";
                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected:
                    {
                        exportControl = "ProtectedAccess";
                        break;
                    }
                default:
                    exportControl = "PrivateAccess";
                    break;
            }
            return exportControl;

        }

        public static OOAdvantech.MetaDataRepository.VisibilityKind GetVisibilityKind(string exportControl)
        {
            OOAdvantech.MetaDataRepository.VisibilityKind visibility ;
            switch (exportControl)
            {
                case "PrivateAccess":
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate;
                        break;
                    }
                //case "ImplementationAccess":
                //    {
                //        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected;
                //        break;
                //    }
                case "ImplementationAccess":
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent;
                        break;
                    }
                case "PublicAccess":
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic;
                        break;
                    }
                case "ProtectedAccess":
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected;
                        break;
                    }
                default:
                    visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate;
                    break;
            }
            return visibility;


        }

    }
}
