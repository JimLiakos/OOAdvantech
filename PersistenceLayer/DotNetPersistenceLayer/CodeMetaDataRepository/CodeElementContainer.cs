using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{9F614F6B-ABCF-44CF-A6D8-523DE4DBAF43}</MetaDataID>
    public interface CodeElementContainer
    {
        /// <MetaDataID>{3A9FEED0-6815-40A4-A682-B81F1F764FEA}</MetaDataID>
        EnvDTE.CodeElement CodeElement
        {
            get;
        }
        /// <MetaDataID>{4d1efb70-59c4-4587-af06-a67da6e44b54}</MetaDataID>
        EnvDTE.vsCMElement Kind
        {
            get; 
        }
        /// <MetaDataID>{7a508c35-c0c7-4d25-a7ed-2021c68109f0}</MetaDataID>
        MetaDataRepository.MetaObjectID Identity
        {
            get;
        }

        /// <MetaDataID>{026c0210-00a1-47a3-be5a-842aded60f4d}</MetaDataID>
        bool ContainCodeElement(EnvDTE.CodeElement codeElement, object itsParent);
        /// <MetaDataID>{92486889-5F20-4B87-89DB-77C5EEC73CD3}</MetaDataID>
        void RefreshCodeElement(EnvDTE.CodeElement codeElement);
        /// <MetaDataID>{82C53550-7123-4021-8F62-59AC2FDD139C}</MetaDataID>
        void RefreshStartPoint();
        /// <MetaDataID>{B05CE13A-B03F-4182-9056-A3B001369F8E}</MetaDataID>
        int Line { get; set; }
        /// <MetaDataID>{6EF67D51-72F6-474E-A001-563090D1FF51}</MetaDataID>
        int LineCharOffset { get; }

        /// <MetaDataID>{B33E55AD-2005-4F45-8CEC-40F260E1A5A7}</MetaDataID>
        void CodeElementRemoved(EnvDTE.ProjectItem projectItem);

        /// <MetaDataID>{3685ddd5-ffa8-42a4-8070-5d103b3655e5}</MetaDataID>
        void CodeElementRemoved();


        /// <MetaDataID>{54e0c2e8-cd6f-459f-a6ae-e2e90b8cc592}</MetaDataID>
        ProjectItem ProjectItem
        {
            get;
        }
        /// <MetaDataID>{4b654587-a4e6-4cb2-80b0-a325f7078f3d}</MetaDataID>
        string CurrentProgramLanguageFullName
        {
            get;
        }

        /// <MetaDataID>{96e0eb6f-3cae-428b-85ff-41f12ea46e8b}</MetaDataID>
        string CurrentProgramLanguageName
        {
            get;
        }




        /// <MetaDataID>{573f650f-3fdd-4432-a354-b8bc32243307}</MetaDataID>
        int GetLine(ProjectItem projectItem);

        /// <MetaDataID>{12a93564-9987-4009-9251-fe70008594d5}</MetaDataID>
        int GetLineCharOffset(ProjectItem projectItem);

        /// <MetaDataID>{a9495f36-5ec4-470b-82ac-f1afcccee6a3}</MetaDataID>
        void LineChanged(ProjectItem projectItem, int linesDown);
    }
}
