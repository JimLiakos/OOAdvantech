using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{90d9c007-0cd6-4c5d-9c72-c34048f6d1fb}</MetaDataID>
    public class AssociationEnd : OOAdvantech.MetaDataRepository.AssociationEnd, IVSUMLModelItemWrapper
    {
        protected AssociationEnd()
        {

        }

        public override string Name
        {
            get
            {
                if(VSUMLAssociationEnd!=null)
                    _Name = VSUMLAssociationEnd.Name;
                return base.Name;
            }
            set
            {
                if(VSUMLAssociationEnd!=null)
                    VSUMLAssociationEnd.Name = value;
                base.Name = value;
            }
        }

        public void Refresh()
        {

        }

        public override bool Indexer
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Indexer");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    _Indexer= value;
                else
                    _Indexer = false;
                return _Indexer;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Indexer", value.ToString());
                _Indexer = value;
            }
        }

        public override bool LazyFetching
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "IsLazyFetching");
                _LazyFetching = false;
                if (bool.TryParse(propertyValue, out _LazyFetching))
                    return _LazyFetching;
                else
                    _LazyFetching= false;
                return _LazyFetching;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "IsLazyFetching", value.ToString());
                _LazyFetching = value;
            }
        }

        public override bool CascadeDelete
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "IsCascadeDelete");
                _CascadeDelete = false;
                if (bool.TryParse(propertyValue, out _CascadeDelete))
                    return _CascadeDelete;
                else
                    _CascadeDelete = false;
                return _CascadeDelete;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "IsCascadeDelete", value.ToString());
                _CascadeDelete = value;
            }
        }
        public override bool ReferentialIntegrity
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "IsReferentialIntegrity");
                _ReferentialIntegrity = false;
                if (bool.TryParse(propertyValue, out _ReferentialIntegrity))
                    return _ReferentialIntegrity;
                else
                    _ReferentialIntegrity = false;
                return _ReferentialIntegrity;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "IsReferentialIntegrity", value.ToString());
                _ReferentialIntegrity = value;
            }
        }

        public override bool AllowTransient
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "IsAllowTransient");
                _AllowTransient = false;
                if (bool.TryParse(propertyValue, out _AllowTransient))
                    return _AllowTransient;
                else
                    _AllowTransient = false;
                return _AllowTransient;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "IsAllowTransient", value.ToString());
                _AllowTransient = value;
            }
        }

        public ModelElement ModelElement
        {
            get
            {
                return VSUMLAssociationEnd as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }


        public override bool Navigable
        {
            get
            {
                _Navigable=VSUMLAssociationEnd.Association.NavigableOwnedEnds.ToList().Contains(VSUMLAssociationEnd);
                return _Navigable;
                
            }
            set
            {
                VSUMLAssociationEnd.SetNavigable(value);
                
            }
        }
        public override bool Persistent
        {
            get
            {
                if (ModelElement.GetStereotypePropertyValue("Persistent") == "Persistent")
                    _Persistent = true;
                else
                    _Persistent = false;

                return base.Persistent;
            }
            set
            {
                base.Persistent = value;
                if (base.Persistent)
                    ModelElement.SetStereotypePropertyValue("Persistent", "Persistent");
                else
                    ModelElement.SetStereotypePropertyValue("Persistent", "Transient");
            }
        }
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }

        internal void GenerateBackwardCompatibilityID()
        {
            if (string.IsNullOrEmpty((VSUMLAssociationEnd as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "Identity")))
            {
                uint nextMemember = (Namespace as MetaDataRepository.Classifier).GetNextAutoGenMemberID();
                (VSUMLAssociationEnd as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "Identity", "+" + nextMemember);
                PutPropertyValue("MetaData", "BackwardCompatibilityID", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));
                this.MetaObjectChangeState();
            }
        }

        public readonly IProperty VSUMLAssociationEnd;
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        public AssociationEnd(IProperty vsUMLAssociationEnd, MetaDataRepository.Classifier specification, MetaDataRepository.Classifier owner, OOAdvantech.MetaDataRepository.Roles roleType, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
            : base(vsUMLAssociationEnd.Name, specification, roleType)
        {

            VSUMLAssociationEnd = vsUMLAssociationEnd;
            VSUmlModel = iModel;
            _Name = vsUMLAssociationEnd.Name;


            _Namespace.Value = owner;

            if ((VSUMLAssociationEnd as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "AsProperty") == "True" || owner is MetaDataRepository.Interface)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                bool getMethod = (VSUMLAssociationEnd as ModelElement).GetExtensionData().GetPropertyValue("Csharp", "GenerateGetOperation") == "True";
                bool setMethod = (VSUMLAssociationEnd as ModelElement).GetExtensionData().GetPropertyValue("Csharp", "GenerateSetOperation") == "True";
                PutPropertyValue("MetaData", "Getter", getMethod);
                PutPropertyValue("MetaData", "Setter", setMethod);

            }
            else
                PutPropertyValue("MetaData", "AsProperty", false);

            PutPropertyValue("MetaData", "ImplementationMember", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "ImplementationField"));
            string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Indexer");
            bool value = false;
            if (bool.TryParse(propertyValue, out value))
                _Indexer = value;
            else
                _Indexer = false;
            _HasBehavioralSettings = true;

        }



        internal AssociationEnd(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }

        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {

            MetaDataRepository.AssociationEnd originAssociationEnd = originMetaObject as MetaDataRepository.AssociationEnd;
            _IsRoleA = originAssociationEnd.IsRoleA;
            if (VSUMLAssociationEnd == null)
            {
                MetaDataRepository.Classifier roleAClassifier = MetaObjectMapper.FindMetaObject(MetaObjectMapper.GetIdentity(originAssociationEnd.Association.RoleA.Namespace)) as MetaDataRepository.Classifier;
                MetaDataRepository.Classifier roleBClassifier = MetaObjectMapper.FindMetaObject(MetaObjectMapper.GetIdentity(originAssociationEnd.Association.RoleB.Namespace)) as MetaDataRepository.Classifier;
                 VSUmlModel.CreateAssociation((roleAClassifier as IVSUMLModelItemWrapper).ModelElement as IClassifier,(roleBClassifier as IVSUMLModelItemWrapper).ModelElement as IClassifier);
            }

            //base.Synchronize(OriginMetaObject);
        }


        public override MetaDataRepository.Classifier CollectionClassifier
        {
            get
            {

                if (string.IsNullOrWhiteSpace(ModelElement.GetExtensionData().GetPropertyValue("MetaData", "CollectionType")))
                    _CollectionClassifier = null;
                else if (_CollectionClassifier == null)
                    _CollectionClassifier =  UnknownClassifier.GetClassifier(ModelElement.GetExtensionData().GetPropertyValue("MetaData", "CollectionType"));
                else if (_CollectionClassifier.FullName!= ModelElement.GetExtensionData().GetPropertyValue("MetaData", "CollectionType"))
                    _CollectionClassifier = UnknownClassifier.GetClassifier(ModelElement.GetExtensionData().GetPropertyValue("MetaData", "CollectionType"));
                return base.CollectionClassifier;
            }
        }
        
        static Parser.Parser _MultiplicityParser;
        internal static Parser.Parser MultiplicityParser
        {
            get
            {
                if (_MultiplicityParser == null)
                {
                    _MultiplicityParser = new Parser.Parser();
                    string[] Resources = typeof(AssociationEnd).Assembly.GetManifestResourceNames();
                    //using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.CompositeOQL.GMR"))
                    using (System.IO.Stream Grammar = typeof(AssociationEnd).Assembly.GetManifestResourceStream("OOAdvantech.VSUMLMetaDataRepository.Multiplicity.cgt"))
                    {
                        byte[] bytes = new byte[Grammar.Length];
                        Grammar.Read(bytes, 0, (int)Grammar.Length);
                        _MultiplicityParser.SetGrammar(bytes, (int)Grammar.Length);
                        Grammar.Close();
                    }
                }
                return _MultiplicityParser;
            }
        }

        MetaDataRepository.MultiplicityRange GetMultiplicityRange(string multilicityString)
        {
            try
            {
                if (string.IsNullOrEmpty(multilicityString))
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange();
                MultiplicityParser.Parse(multilicityString);
                if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Exactly"] != null)
                {
                    string value = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Exactly"] as Parser.ParserNode).Value;
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value), ulong.Parse(value));
                }
                else if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["Unspecified"] != null)
                {
                    return new OOAdvantech.MetaDataRepository.MultiplicityRange(0);
                    int adsasd = 0;
                }
                else if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"] != null)
                {
                    string value = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"] as Parser.ParserNode).Value;
                    if (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["TillStatament"]["UpLimit"]["Many"] != null)
                    {
                        return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value));
                    }
                    else
                    {
                        string upValue = (MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["TillStatament"]["UpLimit"] as Parser.ParserNode).Value;

                        return new OOAdvantech.MetaDataRepository.MultiplicityRange(ulong.Parse(value), ulong.Parse(upValue));
                    }
                    //MultiplicityParser.theRoot["Program"]["MultiplicityStatament"]["LowLimit"]["TillStatament"]["UpLimit"]
                    int adsasd = 0;
                }



            }
            catch (System.Exception error)
            {

            }
            return new OOAdvantech.MetaDataRepository.MultiplicityRange();


        }
 


        public override OOAdvantech.MetaDataRepository.Namespace Namespace
        {
            get
            {
                if (_Namespace.Value != null)
                    return _Namespace.Value;
                return GetOtherEnd().Specification;
            }
        }

        
        public override OOAdvantech.MetaDataRepository.MultiplicityRange Multiplicity
        {
            get
            {
                return GetMultiplicityRange(VSUMLAssociationEnd.GetBounds());
            }
           
        }
        internal void SetMultiplicity(string value)
        {
            VSUMLAssociationEnd.SetBounds(GetMultiplicityRange(value).ToString());
        }


        
    }
}
