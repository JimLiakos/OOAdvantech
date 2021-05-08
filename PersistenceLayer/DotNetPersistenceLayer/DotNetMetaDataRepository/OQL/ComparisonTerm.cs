using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{F7EFCA18-EE45-4B0E-A83B-E14BC4D5DA25}</MetaDataID>
    /// <summary>Define an abstract class for comparison terms. Firstly has a static method (GetComparisonTermFor) which allows it to operate as factory class. Secondly has abstract method (GetCompareExpression) which returns the comparison expression. The expression is relative to the implementation class. </summary>
    [Serializable]
    public abstract class ComparisonTerm
    {
        /// <summary>Define the type of value of comparison term (string, long, double, integer  etc) </summary>
        /// <MetaDataID>{CDEF5B9F-593A-4609-AC94-1C47FAF55919}</MetaDataID>
        public virtual System.Type ValueType
        {
            get
            {
                return typeof(object);
            }
        }
        /// <MetaDataID>{C1D73E8A-9365-4C05-8FD0-F1D2B05321A8}</MetaDataID>
        public virtual string TranslatedExpression
        {
            get
            {
                return null;
            }
        }
        /// <summary>Define the OQL statement which contain this comparison term </summary>
        /// <MetaDataID>{E2F85560-BEC6-4885-93CB-954C027D5836}</MetaDataID>
        [NonSerialized]
        public ObjectQuery _OQLStatement;

        /// <MetaDataID>{a43bfe65-f34a-48a3-a5a0-6cfcc38cca6a}</MetaDataID>
        public ObjectQuery OQLStatement
        {
            get
            {
                return _OQLStatement;
            }
            set
            {
                _OQLStatement = value;
            }
        }

        public enum ComparisonTermType { ObjectAttribute = 0, Object, Parameter, Literal, ObjectID };

        
        /// <summary>Define the parser node which contains the information for comparison term. The type. the value etc. </summary>
        /// <MetaDataID>{0D107008-82C9-4C76-92B6-2FAC88D3EA32}</MetaDataID>
        [NonSerialized]
        public Parser.ParserNode ComparisonTermParserNode;

        /// <MetaDataID>{D4BB83DC-AC90-47A9-ADC1-E17171ABC037}</MetaDataID>
        /// <summary>Return a specialized ComparisonTerm for the parser node. For instance if the parser node is parameter return an Parameter ComparisonTerm. </summary>
        /// <param name="comparisonTermParserNode">Define the parser node which contains the information for comparison term. The type, the value etc. </param>
        /// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
        internal static ComparisonTerm GetComparisonTermFor(Parser.ParserNode comparisonTermParserNode, ObjectsContextQuery oqlStatement)
        {
            ComparisonTermType type = GetType(comparisonTermParserNode, oqlStatement);
            return oqlStatement.CreateComparisonTerm(type, comparisonTermParserNode);

        }
        /// <MetaDataID>{9A495EA5-EFA2-4791-951E-9FEEA4C52163}</MetaDataID>
        /// <summary>Define the object constructor </summary>
        /// <param name="comparisonTermParserNode">Define the parser node which contains the information for comparison term. The type. the value etc. </param>
        /// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
        internal ComparisonTerm(Parser.ParserNode comparisonTermParserNode, ObjectQuery oqlStatement)
        {
            ComparisonTermParserNode = comparisonTermParserNode;
            OQLStatement = oqlStatement;
        }

        /// <MetaDataID>{1c64eee1-a7a4-48db-804a-8fbb901f6d74}</MetaDataID>
        internal ComparisonTerm(ObjectQuery oqlStatement)
        {
            OQLStatement = oqlStatement;
        }

        /// <MetaDataID>{0E506B97-5B19-4519-BCC5-4DDF624F6210}</MetaDataID>
        /// <summary>Extract the type information from the parser node. For example literal, parameter, object ID etc. </summary>
        /// <param name="comparisonTerm">Define the parser node which contains the information for comparison term. The type. the value etc. </param>
        /// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
        private static ComparisonTermType GetType(Parser.ParserNode comparisonTerm, ObjectsContextQuery oqlStatement)
        {
            string TypeName = comparisonTerm.ChildNodes.GetFirst().Name;
            if (TypeName == "ObjectID")
                return ComparisonTermType.ObjectID;

            if (TypeName == "Literal")
                return ComparisonTermType.Literal;
            if (TypeName == "Parameter")
                return ComparisonTermType.Parameter;
            if (TypeName == "Path")
            {
                DataNode PathCorespondingObjectCollection = (DataNode)oqlStatement.PathDataNodeMap[comparisonTerm.ChildNodes.GetFirst()];
                if (PathCorespondingObjectCollection != null)
                {
                    if (PathCorespondingObjectCollection.AssignedMetaObject is MetaDataRepository.Attribute)
                        return ComparisonTermType.ObjectAttribute;
                    if (PathCorespondingObjectCollection.AssignedMetaObject is MetaDataRepository.Classifier)
                        return ComparisonTermType.Object;
                    if (PathCorespondingObjectCollection.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        if (((MetaDataRepository.AssociationEnd)PathCorespondingObjectCollection.AssignedMetaObject).Specification != null)
                            return ComparisonTermType.Object;
                    }
                }
            }
            throw new System.Exception("Error on " + comparisonTerm.ParentNode.Value + " : Critirion term with unknown type");

        }
        /// <MetaDataID>{90CA7841-2A4F-4065-AF5E-92859F6FC5F4}</MetaDataID>
        /// <summary>Return the SQL expression for the comparison term pair. The token (comparison term pair) means the comparison term object which implements the function and the function parameter theOtherComparisonTerm </summary>
        /// <param name="comparisonType">Define the type of comparison (Equal, Not Equal, Greater Than, Less Than etc.) </param>
        /// <param name="theOtherComparisonTerm">Define the second term of comparison. </param>
        public abstract string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm);

        abstract internal ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects);
    }
}
