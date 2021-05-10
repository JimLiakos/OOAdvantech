using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{b4781981-b89b-41b3-b53c-304f292d5503}</MetaDataID>
    public class AssociationEndTypeTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{730c9018-7f25-45f4-b817-94f787292cd1}</MetaDataID>
        AssociationEnd AssociationEnd;
        /// <MetaDataID>{26a40651-4d19-4a11-9fd7-78b052a16005}</MetaDataID>
        public AssociationEndTypeTreeNode(AssociationEnd associationEnd, MetaObjectTreeNode parent)
            : base(associationEnd,parent)
        {
            AssociationEnd = associationEnd;
        }
        /// <MetaDataID>{03823fff-edca-49d5-bcde-58a7d6d2b969}</MetaDataID>
        string _Name;

        /// <MetaDataID>{5085ff67-834d-46fb-b1d1-5fb92e5f89e6}</MetaDataID>
        public override string Name
        {
            get
            {
                Classifier associationEndType = AssociationEnd.CollectionClassifier;
                if (associationEndType == null)
                    associationEndType = AssociationEnd.Specification;

                //if (associationEndType.IsBindedClassifier)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateInstantiationName(associationEndType.TemplateBinding, "");
                //else if (associationEndType.IsTemplate)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateName(associationEndType.OwnedTemplateSignature, "");
                //else
                //    _Name = associationEndType.FullName;
                //return _Name;

                
                if (AssociationEnd is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetShortTypeFullName(associationEndType, (AssociationEnd as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement);
                else
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTypeFullName(associationEndType, default( EnvDTE.Project));
                return _Name;
            }
        }
        /// <MetaDataID>{257037e2-cb0b-4b02-8256-05af8f75a871}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                return base.ContainedObjects;
            }
        }

        /// <MetaDataID>{ce643987-7b7a-4d32-95c7-9452f069a65a}</MetaDataID>
        static Bitmap _Image;
        /// <MetaDataID>{753a970f-6152-4673-af96-47494762c733}</MetaDataID>
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
