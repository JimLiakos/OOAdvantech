using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{9af41ccb-28bf-4a61-ad28-ed8b359ddb0f}</MetaDataID>
    public class ParameterTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{c45867d9-b627-43aa-ae63-a694ee0747a9}</MetaDataID>
          Parameter Paramete;
          /// <MetaDataID>{c84c8f60-43b6-42a8-bd2b-0a5da5b5fb07}</MetaDataID>
        Operation Operation;
        /// <MetaDataID>{7790bcb6-749d-42f2-a0a5-fad4f8b34126}</MetaDataID>
        public ParameterTreeNode(Parameter parameter, Operation operation, MetaObjectTreeNode parent)
            : base(parameter,parent)
        {
            Paramete = parameter;
            Operation = operation;
        }

        /// <MetaDataID>{a4b11038-8ac7-477c-9f7d-e015220f6703}</MetaDataID>
        string _Name;

        /// <MetaDataID>{f282b497-6c18-45e0-915d-c6a46795e796}</MetaDataID>
        public override string Name
        {
            get
            {

                if (Paramete.Type == null)
                    _Name = Paramete.Name;
                else
                {
                    if (Operation is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                        _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetShortTypeFullName(Paramete.Type, (Operation as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement);
                    else
                        _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTypeFullName(Paramete.Type, default(EnvDTE.Project));
                    _Name = Paramete.Name + " : " + _Name;
                    return _Name;

                    //if (Paramete.Type.IsBindedClassifier)
                    //    _Name = Paramete.Name + " : " + OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateInstantiationName(Paramete.Type.TemplateBinding, "");
                    //else if (Paramete.Type.IsTemplate)
                    //    _Name = Paramete.Name + " : " + OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateName(Paramete.Type.OwnedTemplateSignature, "");
                    //else
                    //    _Name = Paramete.Name + " : " + Paramete.Type.FullName;
                }
                return _Name;
            }
        }
        /// <MetaDataID>{8473ed8d-51dc-463f-af64-9ac1305fe398}</MetaDataID>
        static Bitmap _Image;
        /// <MetaDataID>{26c16043-413e-432d-a4fe-c003c6e75859}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSObject_Parameter;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }
    }
}
