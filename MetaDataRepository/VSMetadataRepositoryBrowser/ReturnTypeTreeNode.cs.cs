using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{69f6f192-e0b0-4acd-b85e-0513fe9c4d2d}</MetaDataID>
    public class ReturnTypeTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{72504d9e-7d74-4d39-a727-f8f565b6eabb}</MetaDataID>
        Operation Operation;
        /// <MetaDataID>{b28c992c-e015-487f-b890-0521e1919192}</MetaDataID>
        public ReturnTypeTreeNode(Operation operation, MetaObjectTreeNode parent)
            : base(operation,parent)
        {
            Operation = operation;
        }
        /// <MetaDataID>{e2733b95-fda9-4d36-bab6-f3a1774ff971}</MetaDataID>
        string _Name;

        /// <MetaDataID>{c5cdd6b3-0331-4a38-a4ae-e7edba637090}</MetaDataID>
        public override string Name
        {
            get
            {
                //if (Operation.ReturnType.IsBindedClassifier)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateInstantiationName(Operation.ReturnType.TemplateBinding, "");
                //else if (Operation.ReturnType.IsTemplate)
                //    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateName(Operation.ReturnType.OwnedTemplateSignature, "");
                //else
                //    _Name = Operation.ReturnType.FullName;
                //return _Name;

                if (Operation is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetShortTypeFullName(Operation.ReturnType,(Operation as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement);
                else
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTypeFullName(Operation.ReturnType,default(EnvDTE.Project));
                return _Name;
            }
        }
        /// <MetaDataID>{85a4dc50-fe76-49de-ac4d-0f5c24539a85}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                return base.ContainedObjects;
            }
        }

        /// <MetaDataID>{552ae851-f97b-4692-8314-0084541f3a88}</MetaDataID>
        static Bitmap _Image;
        /// <MetaDataID>{aedece48-a039-4f94-91eb-eb13ff923680}</MetaDataID>
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
