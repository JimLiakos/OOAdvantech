using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{6E92C6D5-1E26-4948-9B9F-9F13FAC00608}</MetaDataID>
    static class VSAccessTypeConverter
    {
        /// <MetaDataID>{DF2A0CE5-BBE5-4C5C-A8DE-4F8DB461C1C0}</MetaDataID>
        public static MetaDataRepository.VisibilityKind GetVisibilityKind(EnvDTE.vsCMAccess accessType)
        {
            MetaDataRepository.VisibilityKind visibility;
            switch (accessType)
            {
                case EnvDTE.vsCMAccess.vsCMAccessPrivate:
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate;

                        break;
                    }
                case EnvDTE.vsCMAccess.vsCMAccessProject:
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent;

                        break;
                    }
                case EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected:
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected;

                        break;
                    }
                case EnvDTE.vsCMAccess.vsCMAccessPublic:
                    {
                        visibility = OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic;

                        break;
                    }
                case EnvDTE.vsCMAccess.vsCMAccessProtected:
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

        /// <MetaDataID>{d9c9ce2c-7d05-40e9-a0c8-9411c7c7dd53}</MetaDataID>
        public static EnvDTE.vsCMAccess GetAccessType(MetaDataRepository.VisibilityKind visibility)
        {
            EnvDTE.vsCMAccess accessType;

            switch (visibility)
            {
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate:
                    {
                        accessType = EnvDTE.vsCMAccess.vsCMAccessPrivate;

                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                    {
                        accessType = EnvDTE.vsCMAccess.vsCMAccessProject;

                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected:
                    {
                        accessType = EnvDTE.vsCMAccess.vsCMAccessProjectOrProtected;

                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                    {
                        accessType = EnvDTE.vsCMAccess.vsCMAccessPublic;

                        break;
                    }
                case OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected:
                    {
                        accessType = EnvDTE.vsCMAccess.vsCMAccessProtected;
                        break;
                    }
                default:
                    accessType = EnvDTE.vsCMAccess.vsCMAccessPrivate;
                    break;
            }
            return accessType;

        }

    }
}
