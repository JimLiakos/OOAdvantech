using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{67a3a68c-5ed9-4132-bf8a-21ef027e7d66}</MetaDataID>
    public class StructuralFeatureTypeTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{ca1b78dc-2955-407e-a53a-e527a711a732}</MetaDataID>
        StructuralFeature StructuralFeature;
        /// <MetaDataID>{513db336-e76d-4dcd-9273-051dccdd93f1}</MetaDataID>
        public StructuralFeatureTypeTreeNode(StructuralFeature structuralFeature, MetaObjectTreeNode parent)
            : base(structuralFeature,parent)
        {
            StructuralFeature = structuralFeature;
        }
        /// <MetaDataID>{77051039-497b-44d0-86b8-4ebb8cfcb5a5}</MetaDataID>
        string _Name;

        /// <MetaDataID>{78303c03-9d28-478d-a8aa-261617210f30}</MetaDataID>
        public override string Name
        {
            get
            {
                //if (StructuralFeature.Type.IsBindedClassifier)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateInstantiationName(StructuralFeature.Type.TemplateBinding, "");
                //else if (StructuralFeature.Type.IsTemplate)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateName(StructuralFeature.Type.OwnedTemplateSignature, "");
                //else
               
                if (StructuralFeature is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetShortTypeFullName(StructuralFeature.Type, (StructuralFeature as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement);
                else
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTypeFullName(StructuralFeature.Type,default(EnvDTE.Project) );
                return _Name;
            }
        }
        /// <MetaDataID>{38f46e05-190d-4b55-9851-57e1dfc9f98d}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                return base.ContainedObjects;
            }
        }

        /// <MetaDataID>{dd130283-297a-485d-936e-e70bc14109cf}</MetaDataID>
        static Bitmap _Image;
        /// <MetaDataID>{09acb64e-f7b2-461f-8282-604da68b3f20}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSObject_Return_Type;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }
    }
}
