using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{c0ec0797-5ff3-4191-b45e-a7b175046188}</MetaDataID>
   public interface IVSUMLModelItemWrapper
    {
        /// <MetaDataID>{208ea7f3-fb98-4382-82ed-93a6b7397b05}</MetaDataID>
       Microsoft.VisualStudio.Modeling.ModelElement ModelElement
       {
           get;
       }

       /// <MetaDataID>{4332b38a-c799-4665-817f-69e2498cf446}</MetaDataID>
       Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
       {
           get;
       }




       void Refresh();
    }
}
