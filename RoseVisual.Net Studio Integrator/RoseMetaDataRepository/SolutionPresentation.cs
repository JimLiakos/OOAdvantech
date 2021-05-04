using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{cf7b3eb1-9cf9-4f8f-a23b-e9e94910d02d}</MetaDataID>
    [System.Runtime.InteropServices.ComVisible(false)]
    internal class SolutionPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<EnvDTE.Solution>
    {
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public SolutionPresentation(EnvDTE.Solution solution)
            : base(solution)
        {
         
        }
        public string Name
        {
            get
            {
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(RealObject.FileName);
                    return fileInfo.Name;

                }
                catch (Exception error)
                {
                    return "";
                }
            }
        }
        public string FileName
        {
            get
            {
                try
                {
                    return RealObject.FileName;
                }
                catch (Exception error)
                {
                    return "";
                }


            }
            set
            {

            }
        }
     


       

    }
}
